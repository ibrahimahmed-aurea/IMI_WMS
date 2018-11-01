using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;

namespace Imi.Framework.DataCollection
{
    public class RegexBarcodeDecoder : IBarcodeDecoder
    {
        private IList<RegexDecoderDefinition> decoderDefinitions;
        private int fnc1 = 0;

        public RegexBarcodeDecoder()
        {
            decoderDefinitions = new List<RegexDecoderDefinition>();

            RegexBarcodeDecoderSection section = ConfigurationManager.GetSection(RegexBarcodeDecoderSection.SectionKey) as RegexBarcodeDecoderSection;

            fnc1 = section.FNC1;

            foreach (RegexDecoderDefinitionElement element in section.DecoderDefinitions)
            {
                AddDefinition(element.Expression, element.ApplicationIdentifier, element.ApplicationIdentifierName);
            }
        }            

        private void AddDefinition(string expression, string applicationIdentifier, string applicationIdentifierName)
        {
            RegexDecoderDefinition definition = new RegexDecoderDefinition();
            
            definition.Regex = new Regex(expression);
            definition.ApplicationIdentifierName = applicationIdentifierName;
            definition.ApplicationIdentifier = applicationIdentifier;

            decoderDefinitions.Add(definition);
        }

        public IList<BarcodeSegment> Decode(string text)
        {
            IList<BarcodeSegment> segments = new List<BarcodeSegment>();

            IList<RegexDecoderDefinition> definitions = decoderDefinitions.ToList();

            string[] split = text.Split((char)fnc1);

            for (int i = 0; i < split.Length; i++)
            {

                string textPart = split[i];
                bool matchFound = true;

                while (matchFound)
                {
                    matchFound = false;

                    foreach (RegexDecoderDefinition definition in definitions)
                    {
                        Match match = definition.Regex.Match(textPart);

                        if (match.Success && (!string.IsNullOrEmpty(match.Value)))
                        {
                            bool decimalPointIndicator = false;

                            BarcodeSegment segment = new BarcodeSegment();

                            string AI = string.Empty;

                            if (definition.ApplicationIdentifier.EndsWith("y"))
                            {
                                decimalPointIndicator = true;
                                AI = definition.ApplicationIdentifier.Replace("y", "");
                            }
                            else
                            {
                                AI = definition.ApplicationIdentifier;
                            }

                            segment.ApplicationIdentifier = definition.ApplicationIdentifier;
                            segment.ApplicationIdentifierName = definition.ApplicationIdentifierName;


                            int start = 0;

                            if (match.Value[0] == fnc1)
                            {
                                start++;
                            }

                            if (!string.IsNullOrEmpty(segment.ApplicationIdentifier))
                            {
                                if (definition.Regex.ToString().IndexOf(AI) > -1 && definition.Regex.ToString().IndexOf(AI) < 2)
                                {
                                    start += AI.Length;
                                }
                            }

                            string data = match.Value.Substring(start);

                            if (decimalPointIndicator)
                            {
                                int decimals = Convert.ToInt32(data.Substring(0, 1));
                                data = data.Substring(1, (data.Length - (decimals + 1))) + System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + data.Substring(data.Length - decimals);
                            }

                            segment.Text = data;

                            textPart = textPart.Remove(match.Index, match.Length);

                            segments.Add(segment);

                            matchFound = true;

                            break;
                        }
                    }
                }
            }

            return segments;
        }

    }
}
