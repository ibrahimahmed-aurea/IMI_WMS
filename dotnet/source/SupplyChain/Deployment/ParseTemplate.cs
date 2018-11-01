using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using Imi.SupplyChain.Deployment.Wrappers;
using Imi.SupplyChain.Deployment.Entities;

namespace Imi.SupplyChain.Deployment
{
    public class ParseTemplate
    {
        
        // Pattern for finding product rows
        public const string ProductRowPattern = @"(?imns:" +
                                                  @"{ProductRow}" +
                                                  @"(?<ProductRow>.*?)" +
                                                  @"{/ProductRow}" +
                                                @")";

        // Pattern for nothing published row
        public const string NothingPublishedPattern = @"(?imns:" +
                                                        @"{NothingPublished}" +
                                                        @"(?<NothingPublished>.*?)" +
                                                        @"{/NothingPublished}" +
                                                      @")";

        // Pattern for finding instance rows within product row
        private const string InstanceRowPattern = @"(?imns:" +
                                                    @"{InstanceRow}" +
                                                    @"(?<InstanceRow>.*?)" +
                                                    @"{/InstanceRow}" +
                                                  @")";
                
        // Pattern for fetching product variables
        private const string ProductVariablePattern = @"(?imns:" +
                                                        @"{P:" +
                                                          @"(?<PVar>" +
                                                             @"\w+?" +
                                                          @")" +
                                                        @"}" +
                                                      @")";

        // Pattern for fetching instance variables
        private const string InstanceVariablePattern = @"(?imns:" +
                                                         @"{I:" +
                                                           @"(?<IVar>" +
                                                              @"\w+?" +
                                                           @")" +
                                                         @"}" +
                                                       @")";

        // Pattern for fetching configuration variables
        private const string ConfigurationVariablePattern = @"(?imns:" +
                                                              @"{C:" +
                                                                @"(?<CVar>" +
                                                                   @"\w+?" +
                                                                @")" +
                                                              @"}" +
                                                            @")";

        private IList<ProductStandard> Products { get; set; }
        private ConfigurationSettings Config { get; set; }

        public ParseTemplate(ConfigurationSettings configuration, IList<ProductStandard> products)
        {
            Config = configuration;
            Products = products;
        }

        private bool SomethingPublished()
        {
            bool found = false;

            foreach (ProductStandard product in Products)
            {
                if (product.IsPublished)
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        public bool DoParse(string templateFile, string outFile)
        {
            // Read the content of the templatefile
            string templateContent = string.Empty;
            string parsedContent = string.Empty;

            // Read from the template file
            templateContent = File.ReadAllText(templateFile);

            parsedContent = templateContent;
                                    
            // Replace the Product rows {PR}
            parsedContent = Regex.Replace(parsedContent, ProductRowPattern, new MatchEvaluator(MatchEvaluatorProductRow));

            // Parse configuration variables.
            parsedContent = Regex.Replace(parsedContent, ConfigurationVariablePattern, new MatchEvaluator(MatchEvaluatorConfigurationVariables));

            // Parse nothing published section
            parsedContent = Regex.Replace(parsedContent, NothingPublishedPattern, new MatchEvaluator(MatchEvaluatorNothingPublished));

            // Write all output
            File.WriteAllText(outFile, parsedContent);

            return true;
        }

        private ProductStandard CurrentProduct { get; set; }
        private Instance CurrentInstance { get; set; }
        
        private string MatchEvaluatorProductRow(Match m)
        {
            string parsedContent = string.Empty;

            // Check if anything is published
            if (SomethingPublished())
            {
                // Loop through all products
                foreach (ProductStandard product in Products)
                {
                    string newProduct = string.Empty;

                    // Check if product is published
                    if (product.IsPublished)
                    {
                        CurrentProduct = product;
                        newProduct = Regex.Replace(m.Groups["ProductRow"].Value, InstanceRowPattern, new MatchEvaluator(MatchEvaluatorInstanceRow));
                        newProduct = Regex.Replace(newProduct, ProductVariablePattern, new MatchEvaluator(MatchEvaluatorProductVariables));
                        newProduct = Regex.Replace(newProduct, ConfigurationVariablePattern, new MatchEvaluator(MatchEvaluatorConfigurationVariables));

                        // Add parsed product
                        parsedContent += newProduct;
                    }
                }
            }
            return parsedContent;
        }

        private string MatchEvaluatorNothingPublished(Match m)
        {
            string parsedContent = string.Empty;

            if (!SomethingPublished())
            {
                // Parse configuration variables.
                parsedContent = Regex.Replace(m.Groups["NothingPublished"].Value, ConfigurationVariablePattern, new MatchEvaluator(MatchEvaluatorConfigurationVariables));
            }

            return parsedContent;
        }

        private string MatchEvaluatorInstanceRow(Match m)
        {
            string parsedContent = string.Empty;

            // Loop through all instances
            foreach (Instance instance in CurrentProduct.Instances)
            {
                string newInstance = string.Empty;

                if (instance.Visible)
                {
                    CurrentInstance = instance;
                    newInstance = Regex.Replace(m.Groups["InstanceRow"].Value, ProductVariablePattern, new MatchEvaluator(MatchEvaluatorProductVariables));
                    newInstance = Regex.Replace(newInstance, InstanceVariablePattern, new MatchEvaluator(MatchEvaluatorInstanceVariables));
                    newInstance = Regex.Replace(newInstance, ConfigurationVariablePattern, new MatchEvaluator(MatchEvaluatorConfigurationVariables));

                    // Add parsed instance
                    parsedContent += newInstance;
                }
            }
            return parsedContent;
        }

        private string MatchEvaluatorProductVariables(Match m)
        {
            // Get the property from the product
            string propVal;

            if (GetPropertyValue(CurrentProduct, m.Groups["PVar"].Value, out propVal))
            {
                return propVal;
            }
            else
            {
                // Property not found in product class
                return string.Format(@"[[Product property with the name '{0}' not found.]]", m.Value);
            }
        }

        private string MatchEvaluatorInstanceVariables(Match m)
        {
            // Get the property from the instance
            string propVal;

            if (GetPropertyValue(CurrentInstance, m.Groups["IVar"].Value, out propVal))
            {
                return propVal;
            }
            else
            {
                // Property not found in instance class
                return string.Format(@"[[Instance property with the name '{0}' not found.]]", m.Value);
            }
        }

        private string MatchEvaluatorConfigurationVariables(Match m)
        {
            // Get the property from the configuration
            return Config.Get(m.Groups["CVar"].Value);
        }
                

        private bool GetPropertyValue(object obj, string str, out string fieldValue)
        {
            FieldInfo[] productFields = obj.GetType().GetFields();
            PropertyInfo[] productProps = obj.GetType().GetProperties();

            fieldValue = string.Empty;

            foreach (FieldInfo field in productFields)
            {
                if (field.Name == str)
                {
                    object newValue = field.GetValue(obj);

                    fieldValue = newValue == null ? string.Empty : newValue.ToString();
                    return true;
                }
            }

            foreach (PropertyInfo property in productProps)
            {
                if (property.Name == str)
                {
                    fieldValue = property.GetValue(obj, null).ToString();
                    return true;
                }
            }
            
            return false;
        }



    }
}
