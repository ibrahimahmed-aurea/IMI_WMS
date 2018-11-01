using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.CodeGeneration.CodeSmithTemplateParser.TemplateObjects;
using System.Text.RegularExpressions;

namespace Cdc.MetaManager.CodeGeneration.CodeSmithTemplateParser
{
    public static class ParsedClassResult
    {
        public static string CreateContents(string nameSpace, string className, string classDefaultInherit, List<TemplateObject> templateObjectList, string templateName)
        {
            StringBuilder result = new StringBuilder();
            int currentIndent = 0;
            int indentIncrease = 4;

            // Check if this is a special case like Core.cst where there is only one TagText 
            // in the whole templateObjectList.
            if (templateObjectList.Count == 1 && 
                templateObjectList[0].TagType == typeof(TagText))
            {
                TagText tText = (TagText)templateObjectList[0].Object;

                // Check if a namespace is defined somewhere, then ignore this
                if (Regex.Matches(tText.Text, @"^namespace\s+\w+\s*$").Count == 0)
                {
                    // Find using statement and i3 the namespace should be inserted
                    MatchCollection mc = Regex.Matches(tText.Text, RegExConstants.RE_CS_USINGSTATEMENTS);

                    // Must be only one match
                    if (mc.Count == 1)
                    {
                        result.Append(mc[0].Groups[RegExConstants.RE_CS_USINGSTATEMENTS_USING].Value);

                        // Add namespace tag
                        result.AppendLine(TemplateCreateNamespacePart(nameSpace, currentIndent));

                        currentIndent += indentIncrease;

                        result.AppendLine(BreakLinesAndOutput(mc[0].Groups[RegExConstants.RE_CS_USINGSTATEMENTS_RESTOFFILE].Value, currentIndent));

                        currentIndent -= indentIncrease;

                        result.AppendLine(string.Format("{0}}}", GetIndent(currentIndent)));

                        return result.ToString();
                    }

                }
            }

            // Create the using statements
            TagImport tagImport = GetTemplateObject<TagImport>(templateObjectList);

            bool usingSystem = false;
            bool usingSystemCollectionsGeneric = false;
            bool usingCodesmithEngine = false;

            while (tagImport != null)
            {
                result.AppendLine(TemplateCreateUsingPart(tagImport, currentIndent));

                if (tagImport.Namespace.Equals("System", StringComparison.CurrentCultureIgnoreCase))
                    usingSystem = true;

                if (tagImport.Namespace.Equals("System.Collections.Generic", StringComparison.CurrentCultureIgnoreCase))
                    usingSystemCollectionsGeneric = true;

                if (tagImport.Namespace.Equals("CodeSmith.Engine", StringComparison.CurrentCultureIgnoreCase))
                    usingCodesmithEngine = true;

                tagImport = GetTemplateObject<TagImport>(templateObjectList);
            }

            // Add System and System.Collections.Generic

            if (!usingSystem)
                result.AppendLine(TemplateCreateUsingPart(new TagImport("System"), currentIndent));

            if (!usingSystemCollectionsGeneric)
                result.AppendLine(TemplateCreateUsingPart(new TagImport("System.Collections.Generic"), currentIndent));

            if (!usingCodesmithEngine)
                result.AppendLine(TemplateCreateUsingPart(new TagImport("CodeSmith.Engine"), currentIndent));

            result.AppendLine();

            // Create namespace tag
            result.AppendLine(TemplateCreateNamespacePart(nameSpace, currentIndent));

            currentIndent += indentIncrease;

            // Find the CodeTemplate object in list
            TagCodeTemplate tagCodeTemplate = GetTemplateObject<TagCodeTemplate>(templateObjectList);

            if (tagCodeTemplate != null && 
                tagCodeTemplate.Inherits != string.Empty)
            {
                result.AppendLine(TemplateCreateClassPart(className, tagCodeTemplate.Inherits, currentIndent));
            }
            else
            {
                result.AppendLine(TemplateCreateClassPart(className, classDefaultInherit, currentIndent));
            }

            currentIndent += indentIncrease;

            // Create the properties
            TagProperty tagProperty = GetTemplateObject<TagProperty>(templateObjectList);

            while (tagProperty != null)
            {
                result.AppendLine(TemplateCreateProperty(tagProperty.Name, tagProperty.Type, currentIndent));

                tagProperty = GetTemplateObject<TagProperty>(templateObjectList);
            }

            result.AppendLine();

            StringBuilder __text = new StringBuilder();

            result.AppendLine(string.Format("{0}public override string OriginalTemplateName {{ get {{ return @\"{1}\"; }} }}\r\n", GetIndent(currentIndent), templateName));

            string templateComment = @"Generated from template: " + templateName;

            // Override the Render function
            result.AppendLine(string.Format("{0}public override void Render()\r\n{0}{{\r\n{1}TagFile(@\"{2}\");", GetIndent(currentIndent), GetIndent(currentIndent + indentIncrease), templateComment));

            // Find all TagCSharp and TagText items and create the code from that
            for (int i = 0; i < templateObjectList.Count; i++)
            {
                TemplateObject obj = templateObjectList[i];

                if (obj.TagType == typeof(TagCSharp))
                {
                    result.Append(BreakLinesAndOutput(((TagCSharp)obj.Object).Code, currentIndent));
                }
                else if ((obj.TagType == typeof(TagText)) || (obj.TagType == typeof(TagParameter)))
                {
                    int lookAheadIndex = i + 1;

                    while (lookAheadIndex < templateObjectList.Count)
                    {
                        if ((templateObjectList[lookAheadIndex].TagType == typeof(TagText)) || (templateObjectList[lookAheadIndex].TagType == typeof(TagParameter))) 
                        {
                            lookAheadIndex++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    // idx points to the next code fragment i.e. not Text or parameters

                    // create single string.format using the text and parameters
                    // advance i to skip ahead 

                    // Do string replace " to "" and the other first ?? togr

                    string argumentList = string.Empty;
                    int stringFormatIndex = 0;

                    for (int i2 = i; i2 < lookAheadIndex; i2++)
                    {
                        argumentList = string.Concat(argumentList,string.Format("{{{0}}}",stringFormatIndex));
                        stringFormatIndex++;
                    }

                    StringBuilder code = new StringBuilder();
                    code.AppendLine(string.Format("__text.AppendFormat(\"{0}\\r\\n\"", argumentList));

                    for (int i3 = i; i3 < lookAheadIndex; i3++)
                    {
                        if (templateObjectList[i3].TagType == typeof(TagText))
                        {
                            TagText tText = (TagText)templateObjectList[i3].Object;

                            // Convert text. Change all " to ""
                            tText.Text = tText.Text.Replace("\"", "\"\"");

                            // Remove all consecutive \n and also all \n that is occuring alone not after a \r
                            tText.Text = Regex.Replace(tText.Text, "((?<=[^\r])\n)|(\n{2,})", string.Empty);
                            code.AppendLine(string.Format(",@\"{0}\"", tText.Text));
                        }
                        else
                        {
                            TagParameter tParam = (TagParameter)templateObjectList[i3].Object;
                            code.AppendLine(string.Format(",{0}", tParam.Parameter));
                        }
                    }

                    code.AppendLine(");");

                    result.Append(string.Format("{0}{1}", GetIndent(currentIndent),code.ToString()));
                    i = lookAheadIndex - 1;
                }
            }

            result.AppendLine(string.Format("{0}return;", GetIndent(currentIndent + indentIncrease)));

            result.AppendLine(string.Format("{0}}}", GetIndent(currentIndent)));

            currentIndent -= indentIncrease;

            result.AppendLine();

            // Add the script section the properties
            TagScript tagScript = GetTemplateObject<TagScript>(templateObjectList);

            while (tagScript != null)
            {
                result.AppendLine(tagScript.Text);

                tagScript = GetTemplateObject<TagScript>(templateObjectList);
            }

            result.AppendLine(string.Format("{0}}}", GetIndent(currentIndent)));

            currentIndent -= indentIncrease;

            result.AppendLine(string.Format("{0}}}", GetIndent(currentIndent)));

            return result.ToString();
        }

        private static string BreakLinesAndOutput(string textBlock, int indent)
        {
            return BreakLinesAndOutput(textBlock, indent, string.Empty, true, string.Empty, true);
        }

        private static string BreakLinesAndOutput(string textBlock, int indent, string prefix, bool addFirstPrefix, string suffix, bool addLastSuffix)
        {
            // Split each line in textblock
            StringBuilder sb = new StringBuilder();
            List<string> stringList = Regex.Split(textBlock, "\r\n").ToList();
            string indentStr = GetIndent(indent);

            for (int i = 0; i < stringList.Count; i++)
            {
                if (i == 0 && !addFirstPrefix)
                {
                    // Ignore if first row is empty (starts with a CRLF) and prefix shouldn't be shown
                    // which means that this row is a continuation of a parameter.
                    if (string.IsNullOrEmpty(stringList[0]))
                    {
                        continue;
                    }

                    if (i >= stringList.Count - 1 && !addLastSuffix)
                    {
                        sb.Append(string.Format("{0}", stringList[i]));
                    }
                    else
                    {
                        sb.AppendLine(string.Format("{0}{1}", stringList[i], suffix));
                    }
                }
                else if (i >= stringList.Count - 1 && !addLastSuffix)
                {
                    sb.Append(string.Format("{0}{1}{2}", indentStr, prefix, stringList[i]));
                }
                else
                {
                    sb.AppendLine(string.Format("{0}{1}{2}{3}", indentStr, prefix, stringList[i], suffix));
                }
            }

            return sb.ToString();
        }

        private static string TemplateCreateScript(string scriptText, int indent)
        {
            string indentStr = GetIndent(indent);
            throw new NotImplementedException();
        }

        private static string TemplateCreateProperty(string propertyName, string propertyType, int indent)
        {
            string indentStr = GetIndent(indent);
            return string.Format("{0}public {1} {2} {{ get; set; }}", indentStr, propertyType, propertyName);
        }

        private static string TemplateCreateClassPart(string className, string inherits, int indent)
        {
            string indentStr = GetIndent(indent);
            return string.Format("{0}public class {1} : {2}\r\n{3}{{", indentStr, className, inherits, indentStr);
        }

        private static string TemplateCreateNamespacePart(string nameSpace, int indent)
        {
            string indentStr = GetIndent(indent);
            string newStr = string.Format("{0}namespace {1}\r\n{0}{{", indentStr, nameSpace);
            return newStr;
        }

        private static string TemplateCreateUsingPart(TagImport tagImport, int indent)
        {
            if (tagImport != null)
            {
                return string.Format("{0}using {1};", GetIndent(indent), tagImport.Namespace);
            }
            else
            {
                return string.Empty;
            }
        }

        private static string GetIndent(int indent)
        {
            if (indent > 0)
            {
                return new StringBuilder().Append(' ', indent).ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        private static T GetTemplateObject<T>(List<TemplateObject> templateObjectList)
        {
            TemplateObject findObject = null;

            // Find the generic type in the TemplateObject List.
            for (int i = 0; i < templateObjectList.Count; i++)
            {
                if (templateObjectList[i].TagType == typeof(T))
                {
                    findObject = templateObjectList[i];
                    break;
                }
            }

            // If it's found then delete it from the list and return the
            // generic type object found.
            if (findObject != null)
            {
                templateObjectList.Remove(findObject);
            }

            return findObject != null ? (T)findObject.Object : default(T);
        }
    }
}
