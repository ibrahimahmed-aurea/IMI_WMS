using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.Framework.Parsing.OracleSQLParsing
{
    public static class RegexConstants
    {
        // Regex for finding comments anywhere in a SQL statement that is not
        // inside any text. 
        public const string SQL_COMMENT = "comment";
        public const string SQL_COMMENT_FIND = @"(?imn:(?<comment>(--.*)|(/\*.*\*/))|('[^']*?'))";

        // Parameters for finding parameters
        public const string SQL_PARAMETER_PARAMETER = "parameter";

        // Regex for finding parameters anywhere in a SQL statement that is not
        // inside any text. Example: ' :test' wouldn't be matched. (([A-Za-z]\w*)\.?)+[^\.]
        public const string SQL_PARAMETER_FIND = @"(?imn:" +            // Case insensitive, multiline, explicit captures
                                                  @"(" +                // Start of finding multiple parameters
                                                   @"[^':]*" +          // Find everything up to a ' or :
                                                   @"(" +               // Start of finding a parameter or a textstring
                                                    @"(:" +             // Start of finding a parameter that starts with colon :
                                                     @"(?<parameter>" + // Catching the parameter
                                                      @"[A-Za-z]\w*" +  // Parameter should start with alpha character and continue with one or many word characters
                                                     @")" +             // End of catching the parameter
                                                    @")" +              // End of matching parameter
                                                    @"|" +              // OR
                                                    @"(" +              // Start of finding textstring
                                                     @"'" +             // Textstring starts with a '
                                                     @"[^']*?" +        // Anything but a '
                                                     @"'" +             // Textstring ends with a '
                                                    @")" +              // End of finding textstring
                                                   @")" +               // End of finding parameter or textstring
                                                  @")+" +               // Try to find one or more parameters
                                                 @")";

        // Regex for finding parameters anywhere in a SQL statement that is not
        // inside any text. Example: ' :test' wouldn't be matched. (([A-Za-z]\w*)\.?){1,2}(?!\.)
        public const string SQL_PARAMETERSPECIAL_FIND = @"(?imn:" +            // Case insensitive, multiline, explicit captures
                                                         @"(" +                // Start of finding multiple parameters
                                                          @"[^':]*" +          // Find everything up to a ' or :
                                                          @"(" +               // Start of finding a parameter or a textstring
                                                           @"(:" +             // Start of finding a parameter that starts with colon :
                                                            @"(?<parameter>" + // Catching the parameter
                                                             @"(([A-Za-z]\w*)\.?){1,2}(?!\.)" +  // Parameter can be one or two sections starting with alpha character and is seperated with a dot
                                                            @")" +             // End of catching the parameter
                                                           @")" +              // End of matching parameter
                                                           @"|" +              // OR
                                                           @"(" +              // Start of finding textstring
                                                            @"'" +             // Textstring starts with a '
                                                            @"[^']*?" +        // Anything but a '
                                                            @"'" +             // Textstring ends with a '
                                                           @")" +              // End of finding textstring
                                                          @")" +               // End of finding parameter or textstring
                                                         @")+" +               // Try to find one or more parameters
                                                        @")";

        /*************************************************************************************/

        // Main SQL RegEx Match Parameters
        public const string SQL_MAIN_SELECTROWS = "selectrows";
        public const string SQL_MAIN_DISTINCT = "distinct";
        public const string SQL_MAIN_FROMROWS = "fromrows";
        public const string SQL_MAIN_WHEREROWS = "whererows";
        public const string SQL_MAIN_SQLSPLITTER = "sqlsplitter";

        // Main SQL RegEx for finding Select, From and Where part.
        public const string SQL_MAIN = @"(?imsn:" +
                                        @"(\s*(?<sqlsplitter>union|intersect|minus))?" +
                                        @"(\s*select" +
                                         @"(" +
                                           @"\s+" +
                                           @"(?<distinct>distinct)" +
                                           @"(?=\s)" +
                                         @")?" +
                                         @"(\s*" +
                                          @"(?<selectrows>" +
                                           @"(" +
                                            @"(?!\s*,|" +
                                               @"\s*(?<!\w+)from\s" +
                                            @")" +
                                            @"([^\(,])" +
                                            @"|" +
                                            @"(" +
                                             @"\(" +
                                               @"(?>" +
                                                @"[^\(\)]+" +
                                                @"|" +
                                                @"\((?<Depth>)" +
                                                @"|" +
                                                @"\)(?<-Depth>)" +
                                               @")*" +
                                               @"(?(Depth)(?!))" +
                                             @"\)" +
                                            @")" +
                                           @")+" +
                                          @")" +
                                          @"(\s*,|(?=\s*from))" +
                                         @")+" +
                                        @")" +
                                        @"(\s*from" +
                                         @"(\s*" +
                                          @"(?<fromrows>" +
                                           @"(" +
                                            @"(?!\s*,|" +
                                               @"\s*(?<!\w+)where\s|" +
                                               @"\s*(?<!\w+)order\s+by\s|" +
                                               @"\s*(?<!\w+)group\s+by\s|" +
                                               @"\s*(?<!\w+)union\s|" +
                                               @"\s*(?<!\w+)having\s|" +
                                               @"\s*(?<!\w+)intersect\s|" +
                                               @"\s*(?<!\w+)minus\s|" +
                                               @"\s*$" +
                                            @")" +
                                            @"([^\(,])" +
                                            @"|" +
                                            @"(" +
                                             @"\(" +
                                               @"(?>" +
                                                @"[^\(\)]+" +
                                                @"|" +
                                                @"\((?<Depth>)" +
                                                @"|" +
                                                @"\)(?<-Depth>)" +
                                               @")*" +
                                               @"(?(Depth)(?!))" +
                                             @"\)" +
                                            @")" +
                                           @")+" +
                                          @")" +
                                          @"(\s*," +
                                           @"|" +
                                           @"(\s*where" +
                                            @"(\s*" +
                                             @"(?<whererows>" +
                                              @"(" +
                                               @"(between\s+.*?\sand\s)?" +
                                               @"(?!\s*(?<!\w+)and\s|" +
                                                  @"\s*(?<!\w+)order\s+by\s|" +
                                                  @"\s*(?<!\w+)group\s+by\s|" +
                                                  @"\s*(?<!\w+)union\s|" +
                                                  @"\s*(?<!\w+)having\s|" +
                                                  @"\s*(?<!\w+)intersect\s|" +
                                                  @"\s*(?<!\w+)minus\s|" +
                                                  @"\s*$" +
                                               @")" +
                                               @"([^\(])" +
                                               @"|" +
                                               @"(" +
                                                @"\(" +
                                                  @"(?>" +
                                                   @"[^\(\)]+" +
                                                   @"|" +
                                                   @"\((?<Depth>)" +
                                                   @"|" +
                                                   @"\)(?<-Depth>)" +
                                                  @")*" +
                                                  @"(?(Depth)(?!))" +
                                                @"\)" +
                                               @")" +
                                              @")+" +
                                             @")" +
                                             @"(\s*and)?" +
                                            @")+" +
                                           @")?" +
                                          @")" +
                                         @")+" +
                                        @")" +
                                        @"(" +
                                         @"(\s*$)" +
                                         @"|" +
                                         @"(\s*order\s+by.*)" +
                                         @"|" +
                                         @"(\s*group\s+by.*)" +
                                         @"|" +
                                         @"(\s*having.*)" +
                                         @"|" +
                                         @"(\s*(union|intersect|minus))" +
                                        @")" +
                                       @")";

        /*************************************************************************************/

        // SQL for Select RegEx Match Parameters
        public const string SQL_SELECTROW_TABLE = "table";
        public const string SQL_SELECTROW_COLUMN = "column";
        public const string SQL_SELECTROW_ALIAS = "alias";
        public const string SQL_SELECTROW_FUNCTION = "function";
        public const string SQL_SELECTROW_FUNCTIONPARAMETERS = "functionparameters";
        public const string SQL_SELECTROW_UNKNOWN = "unknown";

        // SQL for Select RegEx used on each seperate "selectrow".
        public const string SQL_SELECTROW = @"(?imsn:" +
                                             @"(" +
                                              @"\A" +
                                              @"(" +
                                               @"(?<table>[A-Za-z]\w*)" +
                                               @"\." +
                                              @")?" +
                                              @"(?<column>[A-Za-z]\w*)" +
                                              @"(" +
                                               @"\s+" +
                                               @"(?<alias>[A-Za-z]\w*)" +
                                              @")?" +
                                              @"\z" +
                                             @")" +
                                             @"|" +
                                             @"(" +
                                              @"\A" +
                                              @"(?<function>[A-Za-z]\w*)" +
                                              @"(?<functionparameters>" +
                                               @"\(" +
                                                 @"(?>" +
                                                  @"[^\(\)]+" +
                                                  @"|" +
                                                  @"\((?<Depth>)" +
                                                  @"|" +
                                                  @"\)(?<-Depth>)" +
                                                 @")*" +
                                                 @"(?(Depth)(?!))" +
                                               @"\)" +
                                              @")" +
                                              @"\s*" +
                                              @"(?<alias>[A-Za-z]\w*)" +
                                              @"\z" +
                                             @")" +
                                             @"|" +
                                             @"(" +
                                              @"\A(?<unknown>.*)\s+(?<alias>[A-Za-z]\w*)\z" +
                                             @")" +
                                            @")";

        /*************************************************************************************/

        // SQL for From RegEx Match Parameters
        public const string SQL_FROMROW_TABLENAME = "tablename";
        public const string SQL_FROMROW_ALIAS = "alias";
        public const string SQL_FROMROW_SUBSELECT = "subselect";

        // SQL for From RegEx used on each seperate "fromrow".
        public const string SQL_FROMROW = @"(?imn:" +
                                   @"(" +
                                    @"(" +
                                     @"(?<tablename>[A-Za-z]\w*)" +
                                     @"\s*" +
                                     @"(?<alias>[A-Za-z]\w*)?" +
                                    @")" +
                                    @"|" +
                                    @"(" +
                                     @"(?<subselect>" +
                                      @"\(" +
                                        @"(?>" +
                                         @"[^\(\)]+" +
                                         @"|" +
                                         @"\((?<Depth>)" +
                                         @"|" +
                                         @"\)(?<-Depth>)" +
                                        @")*" +
                                        @"(?(Depth)(?!))" +
                                      @"\)" +
                                     @")" +
                                     @"\s+" +
                                     @"(?<alias>[A-Za-z]\w*)" +
                                    @")" +
                                   @")" +
                                  @")";

        /*************************************************************************************/

        // SQL for Where RegEx Match Parameters
        public const string SQL_WHEREROW_TABLE = "table";
        public const string SQL_WHEREROW_COLUMN = "column";
        public const string SQL_WHEREROW_PARAMETER = "parameter";
        public const string SQL_WHEREROW_FUNCTION = "function";
        public const string SQL_WHEREROW_FUNCTIONPARAMETERS = "functionparameters";

        // SQL for Where RegEx used on each seperate "whererow" where there is an existing parameter
        public const string SQL_WHEREROW = @"(?imn:" +
                                            @"(" +
                                             @"(" +
                                              @"(?<table>[A-Za-z]\w*)" +
                                              @"\." +
                                             @")?" +
                                             @"(?<column>[A-Za-z]\w*)" +
                                             @"(\(\+\))?" +
                                             @"\s*" +
                                             @"((not\s+)?like|=|<|>|>=|<=|<>|!=)" +
                                             @"\s*" +
                                             @":(?<parameter>[A-Za-z]\w*)" +
                                            @")" +
                                            @"|" +
                                            @"(" +
                                             @":(?<parameter>[A-Za-z]\w*)" +
                                             @"\s*" +
                                             @"((not\s+)?like|=|<|>|>=|<=|<>|!=)" +
                                             @"\s*" +
                                             @"(" +
                                              @"(?<table>[A-Za-z]\w*)" +
                                              @"\." +
                                             @")?" +
                                             @"(?<column>[A-Za-z]\w*)" +
                                             @"(\(\+\))?" +
                                             @"[^\(]*$" +
                                            @")" +
                                            @"|" +
                                            @"(" +
                                             @"(?<function>[A-Za-z]\w*)" +
                                             @"(?<functionparameters>" +
                                              @"\(" +
                                                @"(?>" +
                                                 @"[^\(\)]+" +
                                                 @"|" +
                                                 @"\((?<Depth>)" +
                                                 @"|" +
                                                 @"\)(?<-Depth>)" +
                                                @")*" +
                                                @"(?(Depth)(?!))" +
                                              @"\)" +
                                             @")" +
                                             @"\s*" +
                                             @".*?" +
                                             @"((not\s+)?like|=|<|>|>=|<=|<>|!=)" +
                                             @"\s*" +
                                             @":(?<parameter>[A-Za-z]\w*)" +
                                            @")" +
                                            @"|" +
                                            @"(" +
                                             @":(?<parameter>[A-Za-z]\w*)" +
                                             @"\s*" +
                                             @"((not\s+)?like|=|<|>|>=|<=|<>|!=)" +
                                             @"\s*" +
                                             @"(?<function>[A-Za-z]\w*)" +
                                             @"(?<functionparameters>" +
                                              @"\(" +
                                                @"(?>" +
                                                 @"[^\(\)]+" +
                                                 @"|" +
                                                 @"\((?<Depth>)" +
                                                 @"|" +
                                                 @"\)(?<-Depth>)" +
                                                @")*" +
                                                @"(?(Depth)(?!))" +
                                              @"\)" +
                                             @")" +
                                            @")" +
                                            @"|" +
                                            @"(" +
                                             @"(" +
                                              @"(?<table>[A-Za-z]\w*)" +
                                              @"\." +
                                             @")?" +
                                             @"(?<column>[A-Za-z]\w*)" +
                                             @"(\(\+\))?" +
                                             @"\s*" +
                                             @"((not\s+)?like|=|<|>|>=|<=|<>|!=)" +
                                             @"\s*" +
                                             @"(?<function>[A-Za-z]\w*)" +
                                             @"(?<functionparameters>" +
                                              @"\(" +
                                                @"(?>" +
                                                 @"(" +
                                                  @"(:(?<parameter>(([A-Za-z]\w*)\.?){1,2}(?!\.)))" +
                                                  @"|" +
                                                  @"[^\(\)]+" +
                                                 @")" +
                                                 @"|" +
                                                 @"\((?<Depth>)" +
                                                 @"|" +
                                                 @"\)(?<-Depth>)" +
                                                @")*" +
                                                @"(?(Depth)(?!))" +
                                              @"\)" +
                                             @")" +
                                            @")" +
                                            @"|" +
                                            @"(" +
                                             @"(?<function>[A-Za-z]\w*)" +
                                             @"(?<functionparameters>" +
                                              @"\(" +
                                                @"(?>" +
                                                 @"(" +
                                                  @"(:(?<parameter>(([A-Za-z]\w*)\.?){1,2}(?!\.)))" +
                                                  @"|" +
                                                  @"[^\(\)]+" +
                                                 @")" +
                                                 @"|" +
                                                 @"\((?<Depth>)" +
                                                 @"|" +
                                                 @"\)(?<-Depth>)" +
                                                @")*" +
                                                @"(?(Depth)(?!))" +
                                              @"\)" +
                                             @")" +
                                             @"\s*" +
                                             @".*?" +
                                             @"((not\s+)?like|=|<|>|>=|<=|<>|!=)" +
                                             @"\s*" +
                                             @"(" +
                                              @"(?<table>[A-Za-z]\w*)" +
                                              @"\." +
                                             @")?" +
                                             @"(?<column>[A-Za-z]\w*)" +
                                             @"(\(\+\))?" +
                                            @")" +
                                           @")";

        // SQL for Where RegEx used on each seperate "whererow" where there is an existing parameter
        // The special means it can have parameters with including dots.
        public const string SQL_WHEREROWSPECIAL = @"(?imn:" +
                                                   @"(" +
                                                    @"(" +
                                                     @"(?<table>[A-Za-z]\w*)" +
                                                     @"\." +
                                                    @")?" +
                                                    @"(?<column>[A-Za-z]\w*)" +
                                                    @"(\(\+\))?" +
                                                    @"\s*" +
                                                    @"((not\s+)?like|=|<|>|>=|<=|<>|!=)" +
                                                    @"\s*" +
                                                    @":(?<parameter>(([A-Za-z]\w*)\.?){1,2}(?!\.))" +
                                                   @")" +
                                                   @"|" +
                                                   @"(" +
                                                    @":(?<parameter>(([A-Za-z]\w*)\.?){1,2}(?!\.))" +
                                                    @"\s*" +
                                                    @"((not\s+)?like|=|<|>|>=|<=|<>|!=)" +
                                                    @"\s*" +
                                                    @"(" +
                                                     @"(?<table>[A-Za-z]\w*)" +
                                                     @"\." +
                                                    @")?" +
                                                    @"(?<column>[A-Za-z]\w*)" +
                                                    @"(\(\+\))?" +
                                                    @"[^\(]*$" +
                                                   @")" +
                                                   @"|" +
                                                   @"(" +
                                                    @"(?<function>[A-Za-z]\w*)" +
                                                    @"(?<functionparameters>" +
                                                     @"\(" +
                                                       @"(?>" +
                                                        @"[^\(\)]+" +
                                                        @"|" +
                                                        @"\((?<Depth>)" +
                                                        @"|" +
                                                        @"\)(?<-Depth>)" +
                                                       @")*" +
                                                       @"(?(Depth)(?!))" +
                                                     @"\)" +
                                                    @")" +
                                                    @"\s*" +
                                                    @".*?" +
                                                    @"((not\s+)?like|=|<|>|>=|<=|<>|!=)" +
                                                    @"\s*" +
                                                    @":(?<parameter>(([A-Za-z]\w*)\.?){1,2}(?!\.))" +
                                                   @")" +
                                                   @"|" +
                                                   @"(" +
                                                    @":(?<parameter>(([A-Za-z]\w*)\.?){1,2}(?!\.))" +
                                                    @"\s*" +
                                                    @"((not\s+)?like|=|<|>|>=|<=|<>|!=)" +
                                                    @"\s*" +
                                                    @"(?<function>[A-Za-z]\w*)" +
                                                    @"(?<functionparameters>" +
                                                     @"\(" +
                                                       @"(?>" +
                                                        @"[^\(\)]+" +
                                                        @"|" +
                                                        @"\((?<Depth>)" +
                                                        @"|" +
                                                        @"\)(?<-Depth>)" +
                                                       @")*" +
                                                       @"(?(Depth)(?!))" +
                                                     @"\)" +
                                                    @")" +
                                                   @")" +
                                                   @"|" +
                                                   @"(" +
                                                    @"(" +
                                                     @"(?<table>[A-Za-z]\w*)" +
                                                     @"\." +
                                                    @")?" +
                                                    @"(?<column>[A-Za-z]\w*)" +
                                                    @"(\(\+\))?" +
                                                    @"\s*" +
                                                    @"((not\s+)?like|=|<|>|>=|<=|<>|!=)" +
                                                    @"\s*" +
                                                    @"(?<function>[A-Za-z]\w*)" +
                                                    @"(?<functionparameters>" +
                                                     @"\(" +
                                                       @"(?>" +
                                                        @"(" +
                                                         @"(:(?<parameter>(([A-Za-z]\w*)\.?){1,2}(?!\.)))" +
                                                         @"|" +
                                                         @"[^\(\)]+" +
                                                        @")" +
                                                        @"|" +
                                                        @"\((?<Depth>)" +
                                                        @"|" +
                                                        @"\)(?<-Depth>)" +
                                                       @")*" +
                                                       @"(?(Depth)(?!))" +
                                                     @"\)" +
                                                    @")" +
                                                   @")" +
                                                   @"|" +
                                                   @"(" +
                                                    @"(?<function>[A-Za-z]\w*)" +
                                                    @"(?<functionparameters>" +
                                                     @"\(" +
                                                       @"(?>" +
                                                        @"(" +
                                                         @"(:(?<parameter>(([A-Za-z]\w*)\.?){1,2}(?!\.)))" +
                                                         @"|" +
                                                         @"[^\(\)]+" +
                                                        @")" +
                                                        @"|" +
                                                        @"\((?<Depth>)" +
                                                        @"|" +
                                                        @"\)(?<-Depth>)" +
                                                       @")*" +
                                                       @"(?(Depth)(?!))" +
                                                     @"\)" +
                                                    @")" +
                                                    @"\s*" +
                                                    @".*?" +
                                                    @"((not\s+)?like|=|<|>|>=|<=|<>|!=)" +
                                                    @"\s*" +
                                                    @"(" +
                                                     @"(?<table>[A-Za-z]\w*)" +
                                                     @"\." +
                                                    @")?" +
                                                    @"(?<column>[A-Za-z]\w*)" +
                                                    @"(\(\+\))?" +
                                                   @")" +
                                                  @")";

        /*************************************************************************************/

    }
}
