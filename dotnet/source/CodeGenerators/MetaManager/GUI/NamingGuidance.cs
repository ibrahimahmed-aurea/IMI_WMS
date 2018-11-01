using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Cdc.MetaManager.DataAccess;

namespace Cdc.MetaManager.GUI
{
    // Delegation of all procedures that check the names below.
    public delegate bool DelegateCheckName(string testName, bool showErrorMessage);

    // The exception class that is used when throwing an exception if the name isn't right.
    [global::System.Serializable]
    public class NamingGuidanceException : Exception
    {
        public NamingGuidanceException() { }
        public NamingGuidanceException(string message) : base(message) { }
        public NamingGuidanceException(string message, Exception inner) : base(message, inner) { }
        protected NamingGuidanceException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    public static class NamingGuidance
    {

        // ------------------------------------------------------------------------------
        // Name of the query.
        // Format: [NameOfDataModule<DM.>]<NameOfQuery<Qry>>
        // Examples: DepartureDM.VehicleQry, NodesQry
        // ------------------------------------------------------------------------------
        private const string RE_QUERYNAME = @"(?n:" +
                                              @"^" +
                                              @"(" +
                                                @"\w+DM\." +
                                              @")?" +
                                              @"\w+Qry" +
                                              @"$" +
                                            @")";

        public static bool CheckQueryName(string testName, bool showErrorMessage)
        {
            testName = testName ?? string.Empty;

            bool result = Regex.Match(testName, RE_QUERYNAME).Success;

            if (!result)
            {
                HandleError(testName,
                            "Query Name",
                            "[NameOfDataModule<DM.>]<NameOfQuery<Qry>>",
                            "\"DepartureDM.VehicleQry\", \"NodesQry\"", 
                            showErrorMessage);
            }

            return result;
        }

        // ------------------------------------------------------------------------------
        // Name of the action (backend) if it's connected to a Stored Procedure
        // Format: Must start with a verb. 
        //         May NOT start with: "New", "Change", "Remove" (use "Create", "Modify", "Delete" instead).
        //         May NOT end with: "Action", "Qry", "New", "Change", "Remove", "Create", "Modify", "Delete"
        // Examples: CreateDeparture, ModifyVehicleType, DeleteNode
        // ------------------------------------------------------------------------------
        private const string RE_ACTIONNAME_SP = @"(?in:" +
                                                  @"^" +
                                                    @"(?!" +
                                                      @"(" +
                                                        @"(New)" +
                                                        @"|" +
                                                        @"(Change)" +
                                                        @"|" +
                                                        @"(Remove)" +
                                                      @")" +
                                                    @")" +
                                                    @"(?!" +
                                                      @"\w+?" +
                                                      @"(" +
                                                        @"(Action)" +
                                                        @"|" +
                                                        @"(Qry)" +
                                                        @"|" +
                                                        @"(New)" +
                                                        @"|" +
                                                        @"(Change)" +
                                                        @"|" +
                                                        @"(Remove)" +
                                                        @"|" +
                                                        @"(Create)" +
                                                        @"|" +
                                                        @"(Modify)" +
                                                        @"|" +
                                                        @"(Delete)" +
                                                      @")" +
                                                      @"$" +
                                                    @")" +
                                                    @"\w+" +
                                                  @"$" +
                                                @")";

        public static bool CheckActionNameStoredProcedure(string testName, bool showErrorMessage)
        {
            testName = testName ?? string.Empty;

            bool result = Regex.Match(testName, RE_ACTIONNAME_SP).Success;

            if (!result)
            {
                HandleError(testName,
                            "Action Name",
                            "Must start with a verb.\n" +
                              "\tMay NOT start with: \"New\", \"Change\", \"Remove\" (use \"Create\", \"Modify\", \"Delete\" instead).\n" +
                              "\tMay NOT end with: \"Action\", \"Qry\", \"New\", \"Change\", \"Remove\", \"Create\", \"Modify\", \"Delete\"",
                            "\"CreateDeparture\", \"ModifyVehicleType\", \"DeleteNode\"",
                            showErrorMessage);
            }

            return result;
        }

        // ------------------------------------------------------------------------------
        // Name of the action (backend) if it's connected to a Query
        // Format: Must start with: "Find"
        //         May NOT end with: "Action", "Qry"
        // Examples: FindDeparture, FindVehicleType, FindNode
        // ------------------------------------------------------------------------------
        private const string RE_ACTIONNAME_QRY = @"(?in:" +
                                                   @"^" +
                                                     @"Find" +
                                                     @"(?!" +
                                                       @"\w+?" +
                                                       @"(" +
                                                         @"(Action)" +
                                                         @"|" +
                                                         @"(Qry)" +
                                                       @")" +
                                                       @"$" +
                                                     @")" +
                                                     @"\w+" +
                                                   @"$" +
                                                 @")";

        public static bool CheckActionNameQuery(string testName, bool showErrorMessage)
        {
            testName = testName ?? string.Empty;

            bool result = Regex.Match(testName, RE_ACTIONNAME_QRY).Success;

            if (!result)
            {
                HandleError(testName,
                            "Action Name",
                            "Must start with: \"Find\"\n" +
                              "\tMay NOT end with: \"Action\", \"Qry\"",
                            "\"FindDeparture\", \"FindVehicleType\", \"FindNode\"",
                            showErrorMessage);
            }

            return result;
        }

        // ------------------------------------------------------------------------------
        // Name of the ServiceMethod (backend) 
        // Format: Must start with a verb. 
        //         May NOT start with: "New", "Change", "Remove" (use "Create", "Modify", "Delete" instead).
        //         May NOT end with: "Action", "Qry", "Service", "New", "Change", "Remove", "Create", "Modify", "Delete"
        // Examples: FindDeparture, ModifyVehicleType, DeleteNode
        // ------------------------------------------------------------------------------
        private const string RE_SERVICENAMEMETHOD = @"(?in:" +
                                                      @"^" +
                                                        @"(?!" +
                                                          @"(" +
                                                            @"(New)" +
                                                            @"|" +
                                                            @"(Change)" +
                                                            @"|" +
                                                            @"(Remove)" +
                                                          @")" +
                                                        @")" +
                                                        @"(?!" +
                                                          @"\w+?" +
                                                          @"(" +
                                                            @"(Action)" +
                                                            @"|" +
                                                            @"(Qry)" +
                                                            @"|" +
                                                            @"(Service)" +
                                                            @"|" +
                                                            @"(New)" +
                                                            @"|" +
                                                            @"(Change)" +
                                                            @"|" +
                                                            @"(Remove)" +
                                                            @"|" +
                                                            @"(Create)" +
                                                            @"|" +
                                                            @"(Modify)" +
                                                            @"|" +
                                                            @"(Delete)" +
                                                          @")" +
                                                          @"$" +
                                                        @")" +
                                                        @"\w+" +
                                                      @"$" +
                                                    @")";

        public static bool CheckServiceMethodName(string testName, bool showErrorMessage)
        {
            testName = testName ?? string.Empty;

            bool result = Regex.Match(testName, RE_SERVICENAMEMETHOD).Success;

            if (!result)
            {
                HandleError(testName,
                            "ServiceMethod Name",
                            "Must start with a verb.\n" +
                              "\tMay NOT start with: \"New\", \"Change\", \"Remove\" (use \"Create\", \"Modify\", \"Delete\" instead).\n" +
                              "\tMay NOT end with: \"Action\", \"Qry\", \"Service\", \"New\", \"Change\", \"Remove\", \"Create\", \"Modify\", \"Delete\"",
                            "\"FindDeparture\", \"ModifyVehicleType\", \"DeleteNode\"",
                            showErrorMessage);
            }

            return result;
        }

        // ------------------------------------------------------------------------------
        // Name of the target of a MappedProperty (backend & frontend)
        // Format: Must be just one word with no whitespaces. Can't be empty.
        // Examples: DepartureId, VechicleTypeDescription
        // ------------------------------------------------------------------------------
        private const string RE_MAPPEDPROPERTYNAME = @"^[A-Z]{1}\w+$";

        public static bool CheckMappedPropertyName(string testName, bool showErrorMessage)
        {
            testName = testName ?? string.Empty;

            bool result = Regex.Match(testName, RE_MAPPEDPROPERTYNAME).Success;

            if (!result)
            {
                HandleError(testName,
                            "MappedProperty Name",
                            "Must be just one word with no whitespaces. Needs to start with a capital letter and can't be empty.",
                            "\"DepartureId\", \"VechicleTypeDescription\"",
                            showErrorMessage);
            }

            return result;
        }

        // ------------------------------------------------------------------------------
        // Name of the Dialog / View (frontend) 
        // Format: May NOT start with: "New", "Change", "Remove" (use "Create", "Modify", "Delete" instead).
        //         May NOT end with: "New", "Change", "Remove", "Create", "Modify", "Delete" (should start with the verb).
        // Examples: Departure, ModifyDeparture
        // ------------------------------------------------------------------------------

        private const string RE_DIALOGNAME_LEAD_CAP = @"(^([A-Z]))";


        private const string RE_DIALOGNAME = @"(?in:" +
                                               @"^" +
                                                 @"(?!" +
                                                   @"(" +

                                                     @"(New)" +
                                                     @"|" +
                                                     @"(Change)" +
                                                     @"|" +
                                                     @"(Remove)" +
                                                   @")" +
                                                 @")" +
                                                 @"(?!" +
                                                   @"\w+?" +
                                                   @"(" +
                                                     @"(New)" +
                                                     @"|" +
                                                     @"(Change)" +
                                                     @"|" +
                                                     @"(Remove)" +
                                                     @"|" +
                                                     @"(Create)" +
                                                     @"|" +
                                                     @"(Modify)" +
                                                     @"|" +
                                                     @"(Delete)" +
                                                   @")" +
                                                   @"$" +
                                                 @")" +
                                                 @"\w+" +
                                               @"$" +
                                             @")";


        private static bool CheckDialogName(string nameOfField, string testName, bool showErrorMessage)
        {
            testName = testName ?? string.Empty;

            bool result = Regex.Match(testName, RE_DIALOGNAME_LEAD_CAP).Success && Regex.Match(testName, RE_DIALOGNAME).Success;

            if (!result)
            {
                HandleError(testName,
                            nameOfField,
                            "May NOT start with: \"New\", \"Change\", \"Remove\" (use \"Create\", \"Modify\", \"Delete\" instead).\n" +
                            "\tMay NOT end with: \"New\", \"Change\", \"Remove\", \"Create\", \"Modify\", \"Delete\" (should start with the verb)",
                            "\"Departure\", \"ModifyDeparture\"",
                            showErrorMessage);
            }

            return result;
        }

        public static bool CheckDialogName(string testName, bool showErrorMessage)
        {
            return CheckDialogName("Dialog Name", testName, showErrorMessage);
        }

        public static bool CheckViewName(string testName, bool showErrorMessage)
        {
            return CheckDialogName("View Name", testName, showErrorMessage);
        }

        public static bool CheckCustomViewName(string testName, bool showErrorMessage)
        {
            return CheckDialogName("Custom View Name", testName, showErrorMessage);
        }

        // ------------------------------------------------------------------------------
        // Name of the UXAction that starts a Service (frontend) 
        // Format: Must start with "Run" where rest must be in Pascal Case (first character uppercase).
        //         Numbers are allowed after first uppercase character.
        // Examples: RunDeleteDeparture, RunDeleteNode
        // ------------------------------------------------------------------------------
        private const string RE_UXACTIONNAME_SERVICE = @"(?n:" +
                                                         @"^" +
                                                           @"Run" +
                                                           @"(" +
                                                             @"\p{Lu}" +
                                                             @"[\p{Ll}\d]+" +
                                                           @")+" +
                                                         @"$" +
                                                       @")";

        public static bool CheckUXActionServiceName(string testName, bool showErrorMessage)
        {
            testName = testName ?? string.Empty;

            bool result = Regex.Match(testName, RE_UXACTIONNAME_SERVICE).Success;

            if (!result)
            {
                HandleError(testName,
                            "UXAction Name",
                            "Must start with \"Run\" where rest must be in Pascal Case (first character uppercase).\n" +
                              "\tNumbers are allowed after first uppercase character.", 
                            "\"RunDeleteDeparture\", \"RunDeleteNode\"",
                            showErrorMessage);
            }

            return result;
        }

        // ------------------------------------------------------------------------------
        // Name of the UXAction that starts a Dialog (frontend) 
        // Format: Must start with "Show" where rest must be in Pascal Case (first character uppercase).
        //         Numbers are allowed after first uppercase character.
        // Examples: ShowDeparture, ShowModifyNode
        // ------------------------------------------------------------------------------
        private const string RE_UXACTIONNAME_DIALOG = @"(?n:" +
                                                        @"^" +
                                                          @"Show" +
                                                          @"(" +
                                                            @"\p{Lu}" +
                                                            @"[\p{Ll}\d]+" +
                                                          @")+" +
                                                        @"$" +
                                                      @")";

        public static bool CheckUXActionDialogName(string testName, bool showErrorMessage)
        {
            bool result = Regex.Match(testName ?? string.Empty, RE_UXACTIONNAME_DIALOG).Success;

            if (!result)
            {
                HandleError(testName,
                            "UXAction Name",
                            "Must start with \"Show\" where rest must be in Pascal Case (first character uppercase).\n" +
                              "\tNumbers are allowed after first uppercase character.",
                            "\"ShowDeparture\", \"ShowModifyNode\"",
                            showErrorMessage);
            }

            return result;
        }

        // ------------------------------------------------------------------------------
        // Name of a classname. Example classname for Custom Views.
        // Format: Name containing alphanumerical characters.
        //         Dots may be used to seperate the words, but may not be the first or last character.
        // Examples: MyClassName, Much.Longer.ClassName
        // ------------------------------------------------------------------------------
        public const string RE_CLASSNAME = @"^" +
                                             @"(" +
                                               @"\w+\." +
                                             @")*" +
                                             @"(" +
                                               @"\w+" +
                                             @")" +
                                           @"$";

        public static bool CheckClassName(string testName, string nameOfField, bool showErrorMessage)
        {
            bool result = Regex.Match(testName ?? string.Empty, RE_CLASSNAME).Success;

            if (!result)
            {
                HandleError(testName,
                            nameOfField,
                            "Name containing alphanumerical characters.\n" +
                              "\tDots may be used to seperate the words, but may not be the first or last character.",
                            "\"MyClassName\", \"Much.Longer.ClassName\"",
                            showErrorMessage);
            }

            return result;
        }


        // ------------------------------------------------------------------------------
        // Name of a dll without any path information.
        // Format: Name of DLL containing alphanumerical characters, underscore and dots.
        //         Only the name of the file should be entered, no path information.
        //         The name must end with .dll (case doesn't matter).
        // Examples: MyDLL.dll, My.much.longer_filename2.Dll
        // ------------------------------------------------------------------------------
        public const string RE_DLLNAME_ONLY = @"(?in:" +
                                                @"^" +
                                                  @"(" +
                                                    @"\w+\." +
                                                  @")+" +
                                                  @"(" +
                                                    @"dll" +
                                                  @")" +
                                                @"$" +
                                              @")";

        public static bool CheckDLLName(string testName, string nameOfField, bool showErrorMessage)
        {
            bool result = Regex.Match(testName ?? string.Empty, RE_DLLNAME_ONLY).Success;

            if (!result)
            {
                HandleError(testName,
                            nameOfField,
                            "Name of DLL containing alphanumerical characters, underscore and dots.\n" +
                              "\tOnly the name of the file should be entered, no path information.\n" +
                              "\tThe name must end with .dll (case doesn't matter).",
                            "\"MyDLL.dll\", \"My.much.longer_filename2.Dll\"",
                            showErrorMessage);
            }

            return result;
        }

        // ------------------------------------------------------------------------------
        // A GUID contains groups of hexadecimal characters seperated by dashes.
        // Format: First a group of 8 hexadecimal characters (0-9, a-f or A-F) followed by a dash "-".
        //         Then three groups of 4 hexadecimal characters. Each group seperated by a dash.
        //         Then finally a group of 12 hexadecimal characters.
        // Examples: 1234ABCD-123A-456B-789C-9876543210AB
        // ------------------------------------------------------------------------------
        public const string RE_GUID = @"(?in:" +
                                        @"^" +
                                          @"([0-9A-F]){8}-" +
                                          @"(([0-9A-F]){4}-){3}" +
                                          @"([0-9A-F]){12}" +
                                        @"$" +
                                      @")";

        public static bool CheckGUID(string testName, string nameOfField, bool showErrorMessage)
        {
            bool result = Regex.Match(testName ?? string.Empty, RE_GUID).Success;

            if (!result)
            {
                HandleError(testName,
                            nameOfField,
                            "A GUID contains groups of hexadecimal characters seperated by dashes.\n" +
                              "\tFirst a group of 8 hexadecimal characters (0-9, a-f or A-F) followed by a dash \"-\".\n" +
                              "\tThen three groups of 4 hexadecimal characters. Each group seperated by a dash.\n" +
                              "\tThen finally a group of 12 hexadecimal characters.",
                            "\"1234ABCD-123A-456B-789C-9876543210AB\"",
                            showErrorMessage);
            }

            return result;
        }

        // Generic check for fields that shouldn't be empty.
        public static bool CheckGUIDFocus(Control testControl, string nameOfField, bool showErrorMessage)
        {
            bool result = testControl == null ? false :
                CheckGUID(testControl.Text.Trim(), nameOfField, showErrorMessage);

            if (!result && testControl != null)
            {
                testControl.Focus();
            }

            return result;
        }

        // ------------------------------------------------------------------------------

        // Generic check for fields that shouldn't be empty.
        public static bool CheckCaptionFocus(Control testControl, string nameOfField, bool showErrorMessage)
        {
            bool result = testControl == null ? false :
                CheckCaption(testControl.Text.Trim(), nameOfField, showErrorMessage);

            if (!result && testControl != null)
            {
                testControl.Focus();
            }

            return result;
        }

        // Generic check for fields that shouldn't be empty.
        public static bool CheckCaption(string testName, string nameOfField, bool showErrorMessage)
        {
            bool result = !string.IsNullOrEmpty(testName);

            if (!result)
            {
                HandleError(string.Empty,
                            nameOfField,
                            "Any characters, but it may not be empty.",
                            string.Empty,
                            showErrorMessage);
            }

            return result;
        }

        // Generic check for fields that shouldn't be empty and may not contain any whitespaces
        public static bool CheckNameFocus(Control testControl, string nameOfField, bool showErrorMessage)
        {
            bool result = testControl == null ? false :
                CheckName(testControl.Text.Trim(), nameOfField, showErrorMessage);

            if (!result && testControl != null)
            {
                testControl.Focus();
            }

            return result;
        }

        // Generic check for fields that shouldn't be empty and may not contain any whitespaces
        public static bool CheckName(string testName, string nameOfField, bool showErrorMessage)
        {
            bool result = Regex.Match(testName, @"^\w+$").Success;

            if (!result)
            {
                HandleError(testName,
                            nameOfField,
                            "Any single word containing upper and/or lowercase characters,\n" +
                            "\tdigits and underscore. Whitespaces are not allowed.",
                            string.Empty,
                            showErrorMessage);
            }

            return result;
        }

        // Generic check for fields that may be empty but may not contain any whitespaces
        public static bool CheckNameEmptyFocus(Control testControl, string nameOfField, bool showErrorMessage)
        {
            bool result = testControl == null ? false :
                CheckNameEmpty(testControl.Text.Trim(), nameOfField, showErrorMessage);

            if (!result && testControl != null)
            {
                testControl.Focus();
            }

            return result;
        }

        // Generic check for fields that may be empty but may not contain any whitespaces
        public static bool CheckNameEmpty(string testName, string nameOfField, bool showErrorMessage)
        {
            bool result = Regex.Match(testName, @"^\w*$").Success;

            if (string.IsNullOrEmpty(testName))
            {
                result = false;
            }

            if (!result)
            {
                HandleError(testName,
                            nameOfField,
                            "Field may be empty or it should contain a single word containing\n" +
                            "\tupper and/or lowercase characters, digits and underscore.\n" +
                            "\tWhitespaces are not allowed.",
                            string.Empty,
                            showErrorMessage);
            }

            return result;
        }

        // Generic check if field exist in a list
        public static bool CheckNameNotInList(string testName, string nameOfField, string nameOfList, IEnumerable<string> namesInUseList, bool caseSensitive, bool showErrorMessage)
        {
            bool result = (namesInUseList == null) ||
                          (namesInUseList.Count(name => (caseSensitive && name == testName) || (!caseSensitive && name.ToUpper() == testName.ToUpper())) == 0);

            if (!result)
            {
                HandleError(testName,
                            nameOfField,
                            nameOfList,
                            showErrorMessage);
            }

            return result;
        }
                
        public static void ShowErrorMessageBox(string testName, string nameOfField, string format, string example)
        {
            ShowErrorMessageBox(GetErrorText(testName, nameOfField, format, example));
        }

        // ------------------------------------------------------------------------------

        private static void ShowErrorMessageBox(string errorText)
        {
            MessageBox.Show(errorText,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }

        private static string GetNotInListErrorText(string testName, string nameOfField, string nameOfList)
        {
            return string.Format("The {0} already exist in the {1}. The {0} tested was \"{2}\".\nTry again with a different value.",
                                 nameOfField,
                                 nameOfList,
                                 testName);
        }

        private static string GetErrorText(string testName, string nameOfField, string format, string examples)
        {
            return string.Format("The {0} format is not valid (value entered: \"{1}\")!\n\n" + 
                                 "Expected Format:\n" + 
                                   "\t{2}{3}",
                                 nameOfField, 
                                 testName,
                                 format, 
                                 string.IsNullOrEmpty(examples) ? string.Empty : string.Format("\n\nExamples:\n\t{0}", examples)
                                 );
        }

        private static void HandleError(string testName, string nameOfField, string nameOfList, bool showErrorMessage)
        {
            string errorMessage = GetNotInListErrorText(testName, nameOfField, nameOfList);

            HandleError(errorMessage, showErrorMessage);
        }

        private static void HandleError(string testName, string nameOfField, string format, string example, bool showErrorMessage)
        {
            string errorMessage = GetErrorText(testName, nameOfField, format, example);

            HandleError(errorMessage, showErrorMessage);
        }

        private static void HandleError(string errorMessage, bool showErrorMessage)
        {
            if (showErrorMessage)
            {
                ShowErrorMessageBox(errorMessage);
            }
            else
            {
                throw new NamingGuidanceException(errorMessage);
            }
        }

    }
}
