using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.CodeGeneration.CodeSmithTemplateParser.TemplateObjects;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Cdc.MetaManager.CodeGeneration.CodeSmithTemplateParser
{
    public class TemplateParser
    {
        private List<TemplateObject> TemplateObjectList { get; set; }

        public TemplateParser()
        {
            TemplateObjectList = new List<TemplateObject>();
        }

        public List<TemplateObject> Parse(string content)
        {
            TemplateObjectList.Clear();

            string parsing = content;

            // Replace comments with nothing
            parsing = Regex.Replace(parsing, RegExConstants.RE_CS_COMMENTS, string.Empty);

            // Find all Tags
            parsing = Regex.Replace(parsing, RegExConstants.RE_CS_TAGS, new MatchEvaluator(MatchEvaluatorTags));

            // Find the Script
            parsing = Regex.Replace(parsing, RegExConstants.RE_CS_SCRIPT, new MatchEvaluator(MatchEvaluatorScript));

            // Find Normal text with parameters and C# code blocks.
            ParseTextCSharpBlock(parsing);

            return TemplateObjectList;
        }

        private void ParseTextCSharpBlock(string parsing)
        {
            // Now split on the CSharp coding pieces found
            List<string> sharpList = Regex.Split(parsing, RegExConstants.RE_CS_CSHARP).ToList();

            // Loop through the list first is Text, second is CSharp etc.
            bool isText = true;

            for (int i = 0; i < sharpList.Count; i++)
            {
                string currentBlock = sharpList[i];

                // Check if block is empty when removing all newlines in the end of the string
                if (Regex.Replace(currentBlock, @"[\r\n]*$", string.Empty) == string.Empty)
                {
                    isText = !isText;
                    continue;
                }

                // Check if there are one or more newlines in the end. In that case remove the last one.
                if (isText && Regex.Match(currentBlock, @"(\r\n)+$").Success)
                {
                    currentBlock = currentBlock.Remove(currentBlock.Length - 2);
                }

                if (isText)
                {
                    ParseTextParameters(currentBlock);
                }
                else
                {
                    TemplateObjectList.Add(new TemplateObject(typeof(TagCSharp), new TagCSharp(currentBlock)));
                }

                isText = !isText;
            }
        }

        private void ParseTextParameters(string currentBlock)
        {
            // Text needs to be parsed further before we're done.
            // First we need to find all parameters
            List<string> textParamList = Regex.Split(currentBlock, RegExConstants.RE_CS_PARAMETER).ToList();

            // Loop through the list first is Text, second is parameter etc.
            bool isText = true;

            for (int i = 0; i < textParamList.Count; i++)
            {
                string textParamBlock = textParamList[i];

                if (isText)
                {
                    if (!string.IsNullOrEmpty(textParamBlock))
                    {
                        TemplateObjectList.Add(new TemplateObject(typeof(TagText), new TagText(textParamBlock)));
                    }
                }
                else
                {
                    TemplateObjectList.Add(new TemplateObject(typeof(TagParameter), new TagParameter(textParamBlock)));
                }

                isText = !isText;
            }
        }

        private string MatchEvaluatorTags(Match m)
        {
            Type tagType = null;
            object tagObject = null;

            switch (m.Groups[RegExConstants.RE_CS_TAGS_TAGNAME].Value)
            {
                case "Assembly":
                    tagType = typeof(TagAssembly);
                    tagObject = new TagAssembly();
                    break;
                case "CodeTemplate":
                    tagType = typeof(TagCodeTemplate);
                    tagObject = new TagCodeTemplate();
                    break;
                case "Import":
                    tagType = typeof(TagImport);
                    tagObject = new TagImport();
                    break;
                case "Property":
                    tagType = typeof(TagProperty);
                    tagObject = new TagProperty();
                    break;
                case "Register":
                    tagType = typeof(TagRegister);
                    tagObject = new TagRegister();
                    break;
                default:
                    tagType = null;
                    tagObject = null;
                    break;
            }

            // Only parse if it's a known tag
            if (tagType != null)
            {
                if (m.Groups[RegExConstants.RE_CS_TAGS_KEY].Captures.Count > 0)
                {
                    for (int i = 0; i < m.Groups[RegExConstants.RE_CS_TAGS_KEY].Captures.Count; i++)
                    {
                        SetProperty(tagType, 
                                    tagObject, 
                                    m.Groups[RegExConstants.RE_CS_TAGS_KEY].Captures[i].Value, 
                                    m.Groups[RegExConstants.RE_CS_TAGS_VALUE].Captures[i].Value);
                    }
                }

                // Add the templateobject to the list
                TemplateObjectList.Add(new TemplateObject(tagType, tagObject));
            }

            return string.Empty;
        }

        private static void SetProperty(Type tagType, object tagObject, string propertyName, string propertyValue)
        {
            PropertyInfo property = tagType.GetProperty(propertyName);

            // Check if property was found in tagType class
            if (property != null)
            {
                // If property is a boolean then try to convert it
                if (property.PropertyType == typeof(System.Boolean))
                {
                    property.SetValue(tagObject, bool.Parse(propertyValue), null);
                }
                else
                {
                    property.SetValue(tagObject, propertyValue, null);
                }
            }
        }

        private string MatchEvaluatorScript(Match m)
        {
            Type tagType = typeof(TagScript);
            TagScript tagObject = new TagScript();

            // Set the text
            SetProperty(tagType,
                        tagObject,
                        "Text",
                        m.Groups[RegExConstants.RE_CS_SCRIPT_TEXT].Value);

            if (m.Groups[RegExConstants.RE_CS_SCRIPT_KEY].Captures.Count > 0)
            {
                for (int i = 0; i < m.Groups[RegExConstants.RE_CS_SCRIPT_KEY].Captures.Count; i++)
                {
                    SetProperty(tagType,
                                tagObject,
                                m.Groups[RegExConstants.RE_CS_SCRIPT_KEY].Captures[i].Value,
                                m.Groups[RegExConstants.RE_CS_SCRIPT_VALUE].Captures[i].Value);
                }
            }

            // Add the templateobject to the list
            TemplateObjectList.Add(new TemplateObject(tagType, tagObject));

            return string.Empty;
        }

    }
}
