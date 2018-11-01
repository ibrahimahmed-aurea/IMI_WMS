using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.CodeGeneration.CodeSmithTemplateParser
{
    public class RegExConstants
    {
        // -----------------------------------------------------------------------------------------
        public const string RE_CS_COMMENTS = @"(?ms:" +
                                               @"<%--.*?--%>\s?" +
                                             @")";

        // -----------------------------------------------------------------------------------------
        public const string RE_CS_TAGS_TAGNAME = "TagName";
        public const string RE_CS_TAGS_KEY = "Key";
        public const string RE_CS_TAGS_VALUE = "Value";

        public const string RE_CS_TAGS = @"(?imns:" +
                                          @"^<%@\s?(?<TagName>\w+)\s+((?<Key>\w+)\s*=\s*""(?<Value>[^""]*)""\s+)+%>\s*?$" +
                                         @")";

        // -----------------------------------------------------------------------------------------
        public const string RE_CS_SCRIPT_KEY = "Key";
        public const string RE_CS_SCRIPT_VALUE = "Value";
        public const string RE_CS_SCRIPT_TEXT = "Text";

        public const string RE_CS_SCRIPT = @"(?imns:" +
                                            @"<script\s*" +
                                             @"((?<Key>\w+)=""(?<Value>[^""]*)""\s*)+" +
                                            @">(\s+^)?" +
                                            @"(?<Text>.+(?=</script>))" +
                                            @"</script>" +
                                           @")";

        // -----------------------------------------------------------------------------------------
        public const string RE_CS_CSHARP_TEXT = "Text";

        public const string RE_CS_CSHARP = @"(?imns:" +
                                             @"[\x20\t]*" +
                                             @"<%" +
                                              @"(?!=)" +
                                              @"(\s+^)?" +
                                              @"(?<Text>" +
                                               @".+?(?=\s*?%>)" +
                                              @")" +
                                             @"\s*?%>([\x20\t]*\r\n^)?" +
                                           @")";

        // -----------------------------------------------------------------------------------------
        public const string RE_CS_PARAMETER_TEXT = "Text";

        public const string RE_CS_PARAMETER = @"(?imns:" +
                                                @"<%=" +
                                                 @"\s*" +
                                                 @"(?<Text>" +
                                                  @".+?(?=\s*%>)" +
                                                 @")" +
                                                @"\s*?%>" +
                                              @")";

        // -----------------------------------------------------------------------------------------
        public const string RE_CS_USINGSTATEMENTS_USING = "UsingStatements";
        public const string RE_CS_USINGSTATEMENTS_RESTOFFILE = "RestOfFile";

        public const string RE_CS_USINGSTATEMENTS = @"(?imns:" +
                                                      @"(?<UsingStatements>.*?)" +
                                                      @"(?=^public\sclass)" +
                                                      @"(?<RestOfFile>.*)" +
                                                    @")";
    
    }
}
