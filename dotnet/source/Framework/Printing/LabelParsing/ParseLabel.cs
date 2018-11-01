using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;
using System.Xml;

namespace Imi.Framework.Printing.LabelParsing
{
    public class ParseLabel
    {
        private const string RE_LABELFILE = @"(?s:^((?<!<)<%(?<CodePart>.*?(?=%>(?!>)))%>(?!>)\u0200*[\r\n]{0,2})?(?<TemplatePart>.+)$)";

        private Type ParseTemplateClass { get; set; }
        private object ParseTemplateInstance { get; set; }
        private string TemplatePart { get; set; }
        private string TemplatePartFixedCRLF { get; set; }
        private string CodePart { get; set; }
        
        public ParseLabel()
        {
            CodePart = string.Empty;
            TemplatePart = string.Empty;
            TemplatePartFixedCRLF = string.Empty;
            ParseTemplateInstance = null;
            ParseTemplateClass = null;
        }

        
        public static IList<string> GetVariablesFromCodeBlock(string labelTemplate)
        {
            List<string> variables = new List<string>();

            Match match = Regex.Match(labelTemplate, RE_LABELFILE);

            if (match.Success)
            {
                // Get code section
                string codePart = match.Groups["CodePart"].Value;

                // Get all variable names set with SetVariable("xxxx", ???
                string pattern = @"SetVariable\s*\(\s*""(?<VarName>[^""]+)""";

                MatchCollection matches = Regex.Matches(codePart, pattern);

                foreach (Match varMatch in matches)
                {
                    variables.Add(varMatch.Groups["VarName"].Value);
                }
            }

            return variables;
        }
        

        public bool LoadTemplate(string labelTemplate, out string compileErrors)
        {
            compileErrors = string.Empty;
            
            Match match = Regex.Match(labelTemplate, RE_LABELFILE);

            string newCodePart = string.Empty;
            string newCodePartSHA1 = string.Empty;

            if (match.Success)
            {
                // Get template section
                TemplatePart = match.Groups["TemplatePart"].Value;

                // Fix CRLF for the templatepart
                TemplatePartFixedCRLF = ReplaceCRLF(TemplatePart);

                // Get code section
                newCodePart = match.Groups["CodePart"].Value;

                // Trim the codepart
                newCodePart = newCodePart.Trim(new char[] { '\r', '\n', ' ' });

                // Replace <<% to <% and replace %>> to %>
                newCodePart = newCodePart.Replace("<<%", "<%").Replace("%>>", "%>");

                // Is there a codepart left?
                if (!string.IsNullOrEmpty(newCodePart))
                {
                    // Check if we should create a new instance, if there is a codepart
                    if (ParseTemplateInstance == null ||
                        ParseTemplateClass == null)
                    {
                        // Overwrite code parts
                        CodePart = newCodePart;
                        
                        // Create the instance for the codepart
                        if (!CreateInstance(out compileErrors))
                            return false;
                    }
                }
                else
                {
                    // No codepart, clear all code, instance and class
                    CodePart = string.Empty;
                    ParseTemplateInstance = null;
                    ParseTemplateClass = null;
                }
                            
                return true;
            }

            return false;
        }

        public string Execute(XmlDocument xmlDocument, out string setVariableErrors)
        {
            return DoExecute(xmlDocument, true, out setVariableErrors);
        }

        public string Execute(XmlDocument xmlDocument)
        {
            string setVariableErrors;
            return DoExecute(xmlDocument, false, out setVariableErrors);
        }
        
        private string ReplaceCRLF(string replaceStr)
        {
            // Using explicit linebreaks ??
            if (Regex.Match(replaceStr, @"(?<!\\)\\r").Success ||
                Regex.Match(replaceStr, @"(?<!\\)\\n").Success)
            {
                // First remove all real linebreaks
                replaceStr = replaceStr.Replace("\r", string.Empty);
                replaceStr = replaceStr.Replace("\n", string.Empty);

                // Then replace the specified linebreaks with real ones
                replaceStr = replaceStr.Replace(@"\r", "\r");
                replaceStr = replaceStr.Replace(@"\n", "\n");
            }

            // Replace all \\r to \r and all \\n to \n
            replaceStr = replaceStr.Replace(@"\\r", @"\r");
            replaceStr = replaceStr.Replace(@"\\n", @"\n");

            return replaceStr;
        }

        private string DoExecute(XmlDocument xmlDocument, bool debug, out string setVariableErrors)
        {
            setVariableErrors = string.Empty;

            if (xmlDocument != null)
            {
                // Can only execute if we have an instance and a class set
                if (ParseTemplateInstance != null && ParseTemplateClass != null)
                {
                    // Set the Template Variables
                    ParseTemplateClass.InvokeMember("SetTemplateVariables", BindingFlags.InvokeMethod, null, ParseTemplateInstance, new object[] { xmlDocument });

                    // Get variable message errors
                    if (debug)
                    {
                        object debugTextObject = ParseTemplateClass.InvokeMember("GetErrorMessage", BindingFlags.InvokeMethod, null, ParseTemplateInstance, new object[] { });

                        if (debugTextObject != null && debugTextObject is string && !string.IsNullOrEmpty((string)debugTextObject))
                        {
                            setVariableErrors = (string)debugTextObject;
                            return string.Empty;
                        }

                        setVariableErrors = string.Empty;

                        // Check if all variables exist, if not then show error messages
                        MatchCollection matches = Regex.Matches(TemplatePartFixedCRLF, @"<%=[ ]*(?<Variable>.+?)[ ]*%>");

                        foreach (Match match in matches)
                        {
                            string varName = match.Groups["Variable"].Value;

                            object dataResult = ParseTemplateClass.InvokeMember("GetVariableError", BindingFlags.InvokeMethod, null, ParseTemplateInstance, new object[] { varName });

                            if (dataResult != null && dataResult is string && !string.IsNullOrEmpty((string)dataResult))
                            {
                                setVariableErrors += dataResult.ToString() + Environment.NewLine;
                            }
                        }

                        if (!string.IsNullOrEmpty(setVariableErrors))
                            return string.Empty;
                    }

                    string result = Regex.Replace(TemplatePartFixedCRLF, @"<%=[ ]*(?<Variable>.+?)[ ]*%>",
                                                  delegate(Match myMatch)
                                                  {
                                                      string varName = myMatch.Groups["Variable"].Value;

                                                      object dataResult = ParseTemplateClass.InvokeMember("GetVariable", BindingFlags.InvokeMethod, null, ParseTemplateInstance, new object[] { varName, debug });

                                                      if (dataResult != null && dataResult is string && !string.IsNullOrEmpty((string)dataResult))
                                                          return dataResult.ToString();
                                                      else
                                                          return string.Empty;
                                                  }
                                                  );

                    return result;
                }
                else
                {
                    // No code part so just try to fix the xpaths
                    XmlDocument xmlDoc = xmlDocument;
                    XmlNode xmlData = xmlDoc.SelectSingleNode("/*/Data/*");
                    XmlNode xmlMeta = xmlDoc.SelectSingleNode("/*/MetaData");

                    // Get variable message errors
                    if (debug)
                    {
                        // Check if all variables exist, if not then show error messages
                        MatchCollection matches = Regex.Matches(TemplatePartFixedCRLF, @"<%=[ ]*(?<Variable>.+?)[ ]*%>");

                        foreach (Match match in matches)
                        {
                            string varName = match.Groups["Variable"].Value;

                            try
                            {
                                XmlNode findNode = xmlData.SelectSingleNode(varName);

                                if (findNode != null)
                                    continue;

                                findNode = xmlMeta.SelectSingleNode(varName);

                                if (findNode != null)
                                    continue;

                                setVariableErrors += string.Format("ERROR: Cannot find the XPath expression - {0}", varName) + Environment.NewLine;
                            }
                            catch
                            {
                                setVariableErrors += string.Format("ERROR: Exception when trying to parse the XPath expression - {0}", varName) + Environment.NewLine;
                            }
                        }

                        if (!string.IsNullOrEmpty(setVariableErrors))
                            return string.Empty;
                    }

                    string result = Regex.Replace(TemplatePartFixedCRLF, @"<%=[ ]*(?<Variable>.+?)[ ]*%>",
                                                  delegate(Match myMatch)
                                                  {
                                                      string varName = myMatch.Groups["Variable"].Value;

                                                      try
                                                      {
                                                          XmlNode findNode = xmlData.SelectSingleNode(varName);

                                                          if (findNode != null)
                                                              return findNode.Value;

                                                          findNode = xmlMeta.SelectSingleNode(varName);

                                                          if (findNode != null)
                                                              return findNode.Value;

                                                          return string.Empty; ;
                                                      }
                                                      catch
                                                      {
                                                          return string.Empty;
                                                      }
                                                  }
                                                  );

                    return result;
                }
            }
            else
                return string.Empty;
        }

        private bool CreateInstance(out string compileErrors)
        {
            compileErrors = string.Empty;

            ParseTemplateClass = null;
            ParseTemplateInstance = null;

            if (!string.IsNullOrEmpty(CodePart))
            {
                string allCode = "using System; " +
                                 "using System.Xml; " +
                                 "using System.Collections.Generic; " +
                                 "using System.Text.RegularExpressions; " +
                                 "namespace Imi.InRuntime" +
                                 "{ " +
                                 "  public class LabelTemplateClass" +
                                 "  { " +
                                 "    private XmlDocument xml;" +
                                 "    private XmlNode xmlMeta;" +
                                 "    private XmlNode xmlData;" +
                                 "    private Dictionary<string, string> templateVariables = null;" +
                                 "    private string errorText;" +

                                 "    public XmlDocument XmlDoc { get { return xml; } }" +
                                 "    public XmlNode XmlMeta { get { return xmlMeta; } }" +
                                 "    public XmlNode XmlData { get { return xmlData; } }" +

                                 "    public LabelTemplateClass()" +
                                 "    {" +
                                 "      templateVariables = new Dictionary<string, string>();" +
                                 "    }" +

                                 "    private string GetValueFromXPath(XmlNode xmlNode, string xpath)" +
                                 "    {" +
                                 "      try" +
                                 "      {" +
                                 "        XmlNode node = xmlNode.SelectSingleNode(xpath);" +
                                 "        if (node != null)" +
                                 "        {" +
                                 "          if (node.Value == \" \")" +
                                 "            return string.Empty;" +
                                 "          else" +
                                 "            return node.Value;" +
                                 "        }" +
                                 "        else" +
                                 "          return string.Empty;" +
                                 "      }" +
                                 "      catch" +
                                 "      {" +
                                 "        return string.Empty;" +
                                 "      }" +
                                 "    }" +

                                 "    private string ReplaceCRLF(string replaceStr)" +
                                 "    {" +
                                 "      if (Regex.Match(replaceStr, @\"(?<!\\\\)\\\\r\").Success ||" +
                                 "          Regex.Match(replaceStr, @\"(?<!\\\\)\\\\n\").Success)" +
                                 "      {" +
                                 "        replaceStr = replaceStr.Replace(\"\\r\", string.Empty);" +
                                 "        replaceStr = replaceStr.Replace(\"\\n\", string.Empty);" +
                                 "        replaceStr = replaceStr.Replace(@\"\\r\", \"\\r\");" +
                                 "        replaceStr = replaceStr.Replace(@\"\\n\", \"\\n\");" +
                                 "      }" +
                                 "      replaceStr = replaceStr.Replace(@\"\\\\r\", @\"\\r\");" +
                                 "      replaceStr = replaceStr.Replace(@\"\\\\n\", @\"\\n\");" +
                                 "      return replaceStr;" +
                                 "    }" +

                                 "    public bool XPathExist(XmlNode xmlNode, string xpath)" +
                                 "    {" +
                                 "      try" +
                                 "      {" +
                                 "        XmlNode node = xmlNode.SelectSingleNode(xpath);" +
                                 "        return node != null ? true : false;" +
                                 "      }" +
                                 "      catch" +
                                 "      {" +
                                 "        return false;" +
                                 "      }" +
                                 "    }" +

                                 "    public string MetaData(string xpath)" +
                                 "    {" +
                                 "      return GetValueFromXPath(XmlMeta, xpath);" +
                                 "    }" +

                                 "    public string Data(string xpath)" +
                                 "    {" +
                                 "      return GetValueFromXPath(XmlData, xpath);" +
                                 "    }" +

                                 "    private void SetVariable(string name, string value)" +
                                 "    {" +
                                 "      if (!templateVariables.ContainsKey(name))" +
                                 "        templateVariables.Add(name, value);" +
                                 "      else" +
                                 "        templateVariables[name] = value;" +
                                 "    }" +

                                 "    private DateTime GetDateTime(string dateTimeString)" +
                                 "    {" +
                                 "      DateTime date = DateTime.MinValue;" +
                                 "      DateTime.TryParseExact(dateTimeString, @\"yyyy-MM-ddTHH\\:mm\\:ss\", null, System.Globalization.DateTimeStyles.AssumeLocal, out date);" +
                                 "      return date;" +
                                 "    }" +

                                 "    private decimal GetNumber(string numberString)" +
                                 "    {" +
                                 "      decimal dec;" +
                                 "      if (decimal.TryParse(numberString, out dec))" +
                                 "        return dec;" +
                                 "      else" +
                                 "        return decimal.MinValue;" +
                                 "    }" +

                                 "    private bool GetBoolean(string boolString)" +
                                 "    {" +
                                 "        bool parseVal;" +
                                 "        if (bool.TryParse(boolString.Trim(), out parseVal))" +
                                 "            return parseVal;" +
                                 "        else if (boolString.Trim() == \"1\")" +
                                 "            return true;" +
                                 "        else" +
                                 "            return false;" +
                                 "    }" +

                                 "    public string GetVariable(string name, bool debug)" +
                                 "    {" +
                                 "      if (templateVariables.ContainsKey(name))" +
                                 "        return ReplaceCRLF(templateVariables[name]);" +
                                 "      else" +
                                 "      {" +
                                 "        if (XPathExist(XmlData, name))" +
                                 "          return Data(name);" +
                                 "        else if (XPathExist(XmlMeta, name))" +
                                 "          return MetaData(name);" +
                                 "        else" +
                                 "        {" +
                                 "          if (debug)" +
                                 "            return string.Format(\"<ERROR: Cannot find \\\"{0}\\\">\", name);" +
                                 "          else" +
                                 "            return string.Empty;" +
                                 "        }" +
                                 "      }" +
                                 "    }" +

                                 "    public string GetVariableError(string name)" +
                                 "    {" +
                                 "      if (!templateVariables.ContainsKey(name) &&" +
                                 "          !XPathExist(XmlData, name) &&" +
                                 "          !XPathExist(XmlMeta, name) )" +
                                 "      {" +
                                 "        return string.Format(\"ERROR: Cannot find \\\"{0}\\\"\", name);" +
                                 "      }" +
                                 "      return string.Empty;" +
                                 "    }" +

                                 "    public string GetErrorMessage()" +
                                 "    {" +
                                 "      return errorText;" +
                                 "    }" +

                                 "    public void SetTemplateVariables(XmlDocument xmlDocument)" +
                                 "    {" +
                                 "      xml = xmlDocument;" +
                                 "      xmlMeta = xml.SelectSingleNode(\"/*/MetaData\");" +
                                 "      xmlData = xml.SelectSingleNode(\"/*/Data/*\");" +
                                 "      errorText = string.Empty;" +
                                 "      templateVariables.Clear();" +
                                 "      try" +
                                 "      {" +
                                 CodePart + Environment.NewLine +
                                 "      }" +
                                 "      catch(Exception ex)" +
                                 "      {" +
                                 "        errorText = ex.Message + \"\\r\\n\" + ex.StackTrace; " +
                                 "      }" +
                                 "    }" +
                                 "  }" +
                                 "}";


                String[] referenceAssemblies = { "System.dll", "System.Xml.dll" };
                CompilerParameters compilerParams = new CompilerParameters(referenceAssemblies);
                compilerParams.GenerateExecutable = false;
                compilerParams.GenerateInMemory = true;

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("CompilerVersion", "v2.0");

                CSharpCodeProvider compiler = new CSharpCodeProvider(parameters);

                CompilerResults compilerResult = compiler.CompileAssemblyFromSource(compilerParams, allCode);

                if (!compilerResult.Errors.HasErrors &&
                    compilerResult.CompiledAssembly != null)
                {
                    Module module = compilerResult.CompiledAssembly.GetModules()[0];

                    // Get the class
                    ParseTemplateClass = module.GetType("Imi.InRuntime.LabelTemplateClass");

                    // Invoke the class
                    ParseTemplateInstance = Activator.CreateInstance(ParseTemplateClass);

                    return true;
                }
                else
                {
                    compileErrors = string.Empty;

                    foreach (CompilerError error in compilerResult.Errors)
                    {
                        compileErrors += string.Format("{0} (Line {1},Col {2}): {3}\r\n", 
                                                       error.ErrorNumber,
                                                       error.Line.ToString(),
                                                       error.Column.ToString(),
                                                       error.ErrorText);
                    }
                }
            }

            return false;
        }
    }
}
