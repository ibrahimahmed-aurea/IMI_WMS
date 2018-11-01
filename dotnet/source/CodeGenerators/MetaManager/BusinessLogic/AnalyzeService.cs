using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Transaction.Interceptor;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.DataAccess.Dao;
using Cdc.MetaManager.DataAccess;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using Spring.Data.NHibernate.Support;
using NHibernate;
using System.Reflection;
using System.IO;
using Cdc.Framework.Parsing.PLSQLPackageSpecification;
using Cdc.Framework.Parsing.OracleSQLParsing;
using Cdc.Framework.ExtensionMethods;
using Cdc.MetaManager.BusinessLogic.Helpers;

namespace Cdc.MetaManager.BusinessLogic
{
    public class AnalyzeService : IAnalyzeService
    {
        public IServiceMethodDao ServiceMethodDao { get; set; }
        public IDialogDao DialogDao { get; set; }
        public IUXActionDao UXActionDao { get; set; }
        public IDialogService DialogService { get; set; }
        public IIssueDao IssueDao { get; set; }
        public IPackageDao PackageDao { get; set; }
        public IStoredProcedureDao StoredProcedureDao { get; set; }
        public IQueryDao QueryDao { get; set; }
        public ISchemaDao SchemaDao { get; set; }
        public IPropertyDao PropertyDao { get; set; }
        public IApplicationDao ApplicationDao { get; set; }

        private static CallbackService Callback = null;

        public AnalyzeService()
        {
            Callback = new CallbackService();
        }

        public AnalyzeIssueTree Check(Guid backendApplicationId,
                                      Guid frontendApplicationId,
                                      bool checkStoredProcs,
                                      string specDirectory,
                                      bool checkSQL,
                                      string connectionString,
                                      bool checkMaps,
                                      bool checkDialogs,
                                      CallbackDelegate callback)
        {
            AnalyzeIssueTree issueTree = new AnalyzeIssueTree();

            Callback.SetCallback(callback);

            Callback.Do("Initializing", "Fetching data from database...");

            Application backendApplication = ApplicationDao.FindById(backendApplicationId);
            Application frontendApplicationn = ApplicationDao.FindById(frontendApplicationId);

            int passes = (checkStoredProcs ? 1 : 0) + // One pass for stored procedure checks
                         (checkSQL ? 1 : 0) + // and one pass for SQL's
                         (checkMaps ? 1 : 0) + // and one pass for the maps
                         (checkDialogs ? 1 : 0); // and one pass for the dialogs

            int currentPass = 1;

            if (checkStoredProcs)
            {
                using (new SessionScope())
                {
                    AnalyzeIssueNode node = new AnalyzeIssueNode(string.Format("Pass {0} of {1} - Check Stored Procedures in Packages", currentPass++, passes));

                    // Create Issues for all stored procedures
                    CheckStoredProceduresAgainstSpec(node, backendApplicationId, specDirectory);

                    // Delete all issues that hasn't been found or created by this type
                    DeleteNonIssues(backendApplicationId, node, IssueObjectType.StoredProcedure);

                    issueTree.IssueNodes.Add(node);
                }
            }

            if (checkSQL)
            {
                using (new SessionScope())
                {
                    AnalyzeIssueNode node = new AnalyzeIssueNode(string.Format("Pass {0} of {1} - Check all Queries", currentPass++, passes));

                    // Create Issues for all stored procedures
                    CheckQueriesAgainstDatabase(node, backendApplicationId);

                    // Delete all issues that hasn't been found or created by this type
                    DeleteNonIssues(backendApplicationId, node, IssueObjectType.Query);

                    issueTree.IssueNodes.Add(node);
                }
            }

            if (checkMaps)
            {
                using (new SessionScope())
                {
                    AnalyzeIssueNode node = new AnalyzeIssueNode(string.Format("Pass {0} of {1} - Check all Maps for Backend", currentPass++, passes));

                    CheckAllMaps(node, backendApplicationId);

                    // Delete all issues that hasn't been found or created by this type
                    DeleteNonIssues(backendApplicationId, node, IssueObjectType.ServiceMethod);

                    issueTree.IssueNodes.Add(node);
                }
            }

            if (checkDialogs)
            {
                using (new SessionScope())
                {
                    AnalyzeIssueNode node = new AnalyzeIssueNode(string.Format("Pass {0} of {1} - Check all Dialogs", currentPass++, passes));

                    CheckAllDialogs(node, frontendApplicationId);

                    // Delete all issues that hasn't been found or created by this type
                    DeleteNonIssues(frontendApplicationId, node, IssueObjectType.Dialog);

                    issueTree.IssueNodes.Add(node);
                }
            }

            Callback.SetCallback(null);

            return issueTree;
        }

        [Transaction(ReadOnly = false)]
        public void CheckStoredProceduresAgainstSpec(AnalyzeIssueNode parentNode, Guid applicationId, string specDirectory)
        {
            Application application = ApplicationDao.FindById(applicationId);

            // Fetch all packages for the application
            IList<Package> packages = PackageDao.FindAllByApplicationId(applicationId);

            if (packages != null && packages.Count > 0)
            {
                // Set the same text for passtext as the parentNodes name
                Callback.Initialize(parentNode.Name, packages.Count);

                // Loop through all packages
                foreach (Package package in packages)
                {
                    Callback.Next(string.Format("Checking Package ({0} / {1}) - {2}...",
                                                CallbackService.C_CURRENTSTEP,
                                                CallbackService.C_MAXSTEP,
                                                package.Name));

                    // Create the node for the package
                    AnalyzeIssueNode currentPackageNode = new AnalyzeIssueNode(string.Format("{0} [{1}]", package.Name, package.Id));

                    // Check if the package filename is found in the directory specified.
                    string fileName = Path.Combine(specDirectory, Path.GetFileName(package.Filename));

                    if (File.Exists(fileName))
                    {
                        // If the package should be updated in the database or not.
                        bool updatePackage = false;

                        PLSQLSpec parsedFile = PLSQLSupportedSpec.ParseSpecFile(fileName);

                        if (parsedFile != null)
                        {
                            // Fetch all procedures for the package
                            IList<StoredProcedure> storedProcedures = StoredProcedureDao.FindAllByPackageId(package.Id);

                            //updatePackage = true;

                            int i = 1;

                            // Loop through all procedures in package
                            foreach (StoredProcedure procedure in storedProcedures)
                            {
                                bool isMatched;

                                Callback.Update(string.Format("Checking Package \"{0}\" - ({1}/{2}) {3}",
                                                              package.Name,
                                                              i++,
                                                              storedProcedures.Count,
                                                              procedure.ProcedureName));

                                DoCheckStoredProcedure(currentPackageNode, application, procedure, parsedFile, out isMatched);

                                /*
                                if (!isMatched)
                                    updatePackage = false;
                                */
                            }
                        }

                        // The package should be updated with the current file since it has been
                        // verified that all procedures are in sync.
                        /*
                        if (updatePackage)
                        {
                            Callback.Update(string.Format("Updating Package \"{0}\"...", package.Name));

                            package.Filename = parsedFile.FileNameParsed;
                            package.Hash = parsedFile.PackageHash;
                            package.Name = parsedFile.PackageName;
                            package.Size = parsedFile.PackageLength;

                            PackageDao.SaveOrUpdate(package);
                        }
                        */
                    }
                    else
                    {
                        // No file found. Assume that Package has been removed.
                        // Mark all procedures for the package as removed.

                        // Fetch all procedures for the package
                        IList<StoredProcedure> storedProcedures = StoredProcedureDao.FindAllByPackageId(package.Id);

                        // Loop through all procedures in package
                        foreach (StoredProcedure procedure in storedProcedures)
                        {
                            Callback.Update(string.Format("Checking Package \"{0}\" - Stored Procedure {1} has been removed.", package.Name, procedure.ProcedureName));

                            // Create the node for the package
                            AnalyzeIssueNode storedProcNode = CreateStoredProcedureNode(application, procedure);

                            storedProcNode.IssueList.AddError(string.Format("The Stored Procedure ({0}) \"{1}\" and the Package has been removed (spec file not found)!", procedure.Id, procedure.ProcedureName), procedure);

                            // Add the stored procedure node to the package node
                            currentPackageNode.Children.Add(storedProcNode);
                        }
                    }

                    if (currentPackageNode.Children.Count > 0)
                        parentNode.Children.Add(currentPackageNode);
                }
            }
        }

        private void CheckQueriesAgainstDatabase(AnalyzeIssueNode parentNode, Guid applicationId)
        {
            Application application = ApplicationDao.FindById(applicationId);

            // Fetch all queries for the application
            IList<Query> queries = QueryDao.FindAllByApplicationId(applicationId);

            if (queries != null && queries.Count > 0)
            {
                // Set the same text for passtext as the parentNodes name
                Callback.Initialize(parentNode.Name, queries.Count);

                // Get the connectionstring from the schema for the application
                string connectionString = SchemaDao.FindByApplicationId(application.Id).ConnectionString;

                // Loop through all queries
                foreach (Query query in queries)
                {
                    DoCheckQuery(parentNode, application, query, connectionString);
                }
            }
        }

        [Transaction(ReadOnly = false)]
        private void CheckAllMaps(AnalyzeIssueNode parentNode, Guid applicationId)
        {

            Application application = ApplicationDao.FindById(applicationId);

            IList<ServiceMethod> serviceMethods = ServiceMethodDao.FindAllByApplicationId(applicationId);

            List<Guid> validServiceMethodIdList = new List<Guid>();

            // Set the same text for passtext as the parentNodes name
            Callback.Initialize(parentNode.Name, serviceMethods.Count);

            foreach (ServiceMethod serviceMethod in serviceMethods)
            {
                DoCheckServiceMethod(parentNode, application, serviceMethod);
            }

        }

        [Transaction(ReadOnly = false)]
        private void CheckAllDialogs(AnalyzeIssueNode parentNode, Guid frontendApplicationId)
        {
            Application application = ApplicationDao.FindById(frontendApplicationId);


            IList<Dialog> dialogs = DialogDao.FindAllDialogsWithInterfaceView(frontendApplicationId);

            // Set the same text for passtext as the parentNodes name
            Callback.Initialize(parentNode.Name, dialogs.Count);

            foreach (Dialog dialog in dialogs)
            {
                // Initialize dialog a bit more
                NHibernateUtil.Initialize(dialog.RootViewNode);

                if (dialog.RootViewNode != null)
                    NHibernateUtil.Initialize(dialog.RootViewNode.Children);

                DoCheckDialog(parentNode, application, dialog);
            }

        }

        [Transaction(ReadOnly = false)]
        public AnalyzeIssueTree CheckSingleQuery(Guid backendApplicationId, Guid queryId)
        {
            AnalyzeIssueTree issueTree = new AnalyzeIssueTree();

            // No callbacks used here
            Callback.SetCallback(null);


            AnalyzeIssueNode parentNode = new AnalyzeIssueNode("Check of single Service Method");

            Application application = ApplicationDao.FindById(backendApplicationId);

            // Get the connectionstring from the schema for the application
            string connectionString = SchemaDao.FindByApplicationId(application.Id).ConnectionString;

            try
            {
                Query query = QueryDao.FindById(queryId);

                DoCheckQuery(parentNode, application, query, connectionString);
            }
            catch (ObjectNotFoundException) { }

            DeleteNonIssues(backendApplicationId, parentNode, IssueObjectType.Query, queryId);

            issueTree.IssueNodes.Add(parentNode);


            return issueTree;
        }

        [Transaction(ReadOnly = false)]
        public AnalyzeIssueTree CheckSingleStoredProcedure(Guid backendApplicationId, Guid storedProcedureId, string specDirectory)
        {
            AnalyzeIssueTree issueTree = new AnalyzeIssueTree();

            // No callbacks used here
            Callback.SetCallback(null);


            AnalyzeIssueNode parentNode = new AnalyzeIssueNode("Check of single Stored Procedure");

            Application application = ApplicationDao.FindById(backendApplicationId);

            try
            {
                StoredProcedure storedProcedure = StoredProcedureDao.FindById(storedProcedureId);

                // Check if the package filename is found in the directory specified.
                string fileName = Path.Combine(specDirectory, Path.GetFileName(storedProcedure.Package.Filename));

                PLSQLSpec parsedFile = PLSQLSupportedSpec.ParseSpecFile(fileName);
                bool isMatched;

                DoCheckStoredProcedure(parentNode, application, storedProcedure, parsedFile, out isMatched);
            }
            catch (ObjectNotFoundException) { }

            DeleteNonIssues(application.Id, parentNode, IssueObjectType.StoredProcedure, storedProcedureId);

            issueTree.IssueNodes.Add(parentNode);


            return issueTree;
        }

        [Transaction(ReadOnly = false)]
        public AnalyzeIssueTree CheckSingleServiceMethod(Guid backendApplicationId, Guid serviceMethodId)
        {
            AnalyzeIssueTree issueTree = new AnalyzeIssueTree();

            // No callbacks used here
            Callback.SetCallback(null);

            AnalyzeIssueNode parentNode = new AnalyzeIssueNode("Check of single Service Method");

            Application application = ApplicationDao.FindById(backendApplicationId);

            ServiceMethod serviceMethod = ServiceMethodDao.FindById(serviceMethodId);

            if (serviceMethod != null)
            {
                DoCheckServiceMethod(parentNode, application, serviceMethod);
            }

            DeleteNonIssues(backendApplicationId, parentNode, IssueObjectType.ServiceMethod, serviceMethodId);

            issueTree.IssueNodes.Add(parentNode);


            return issueTree;
        }

        [Transaction(ReadOnly = false)]
        public AnalyzeIssueTree CheckSingleDialog(Guid frontendApplicationId, Guid dialogId)
        {
            AnalyzeIssueTree issueTree = new AnalyzeIssueTree();

            // No callbacks used here
            Callback.SetCallback(null);

            AnalyzeIssueNode parentNode = new AnalyzeIssueNode("Check of single Dialog");

            Application application = ApplicationDao.FindById(frontendApplicationId);

            Dialog dialog = DialogDao.FindById(dialogId);

            if (dialog != null)
            {
                DoCheckDialog(parentNode, application, dialog);
            }

            DeleteNonIssues(frontendApplicationId, parentNode, IssueObjectType.Dialog, dialogId);

            issueTree.IssueNodes.Add(parentNode);


            return issueTree;
        }

        private void DoCheckQuery(AnalyzeIssueNode parentNode, Application application, Query query, string connectionString)
        {
            Callback.Next(string.Format("Checking Query ({0} / {1}) - {2}...",
                                        CallbackService.C_CURRENTSTEP,
                                        CallbackService.C_MAXSTEP,
                                        query.Name));

            // Create the node for the query
            AnalyzeIssueNode currentQueryNode = new AnalyzeIssueNode(application, IssueObjectType.Query, query.Id);

            currentQueryNode.Name = query.Name;

            OracleQuery oracleQuery = null;

            // Analyzing the query
            if (query.QueryType == QueryType.QueryForService)
            {
                oracleQuery = OracleQueryAnalyzer.Analyze(query.SqlStatement, connectionString);
            }
            else if (query.QueryType == QueryType.QueryForReport)
            {
                oracleQuery = OracleQueryAnalyzer.Analyze(query.SqlStatement, connectionString, true);
            }

            if (oracleQuery != null)
            {
                if (oracleQuery.ParseErrors.Count == 0)
                {
                    // Add warnings if any exist
                    if (oracleQuery.ParseWarnings.Count > 0)
                    {
                        foreach (string warning in oracleQuery.ParseWarnings)
                        {
                            currentQueryNode.IssueList.AddWarning(warning);
                        }
                    }
                }
                else
                {
                    foreach (string error in oracleQuery.ParseErrors)
                    {
                        currentQueryNode.IssueList.AddError(error);
                    }
                }
            }

            // Only add if there are any issues
            if (currentQueryNode.IssueList.Count > 0)
                parentNode.Children.Add(currentQueryNode);
        }
                
        private void DoCheckStoredProcedure(AnalyzeIssueNode parentNode, Application application, StoredProcedure procedure, PLSQLSpec parsedFile, out bool isMatched)
        {
            isMatched = false;

            // Try to find the procedure in the parsed file by name which
            // can result in more than one since we support overloaded procedures.
            IList<PLSQLProcedure> parsedProcedures = parsedFile.Procedures.Where(p => p.Name == procedure.ProcedureName).ToList();

            if (parsedProcedures.Count == 1)
            {
                string parameterError;

                if (!DoProceduresMatch(procedure, parsedProcedures[0], out parameterError))
                {
                    // Create the node for the stored procedure
                    AnalyzeIssueNode storedProcNode = CreateStoredProcedureNode(application, procedure);

                    storedProcNode.IssueList.AddError(string.Format("The Stored Procedure ({0}) \"{1}.{2}\" doesn't match with the spec." + Environment.NewLine +
                                                                    "{3}",
                                                                    procedure.Id,
                                                                    procedure.Package.Name,
                                                                    procedure.ProcedureName,
                                                                    parameterError),
                                                                    procedure);

                    // Add the stored procedure node to the package node
                    parentNode.Children.Add(storedProcNode);
                }
                else
                {
                    // The procedure is found and matches
                    isMatched = true;
                }
            }
            else if (parsedProcedures.Count > 1)
            {
                // Check if any of the found procedures matches
                bool anyMatch = false;

                string parameterError;

                foreach (PLSQLProcedure parsedProcedure in parsedProcedures)
                {
                    if (parsedProcedure.Status == ProcedureStatus.Valid)
                    {
                        if (DoProceduresMatch(procedure, parsedProcedure, out parameterError))
                        {
                            anyMatch = true;
                            isMatched = true;
                            break;
                        }
                    }
                }

                if (!anyMatch)
                {
                    // Create the node for the stored procedure
                    AnalyzeIssueNode storedProcNode = CreateStoredProcedureNode(application, procedure);

                    // Create the issue
                    storedProcNode.IssueList.AddError(string.Format("The Stored Procedure ({0}) \"{1}.{2}\" (overloaded) doesn't match any of the procedures in the spec.",
                                                                    procedure.Id,
                                                                    procedure.Package.Name,
                                                                    procedure.ProcedureName),
                                                      procedure);

                    // Add the stored procedure node to the package node
                    parentNode.Children.Add(storedProcNode);
                }
            }
            // No parsed procedure found with that name. Tell the user that the procedure should be deleted.
            else
            {
                // Create the node for the package
                AnalyzeIssueNode storedProcNode = CreateStoredProcedureNode(application, procedure);

                storedProcNode.IssueList.AddError(string.Format("The Stored Procedure ({0}) \"{1}.{2}\" is not found in the spec file!",
                                                                procedure.Id,
                                                                procedure.Package.Name,
                                                                procedure.ProcedureName),
                                                  procedure);

                // Add the stored procedure node to the package node
                parentNode.Children.Add(storedProcNode);
            }
        }

        private static AnalyzeIssueNode CreateStoredProcedureNode(Application application, StoredProcedure procedure)
        {
            AnalyzeIssueNode storedProcNode = new AnalyzeIssueNode(application, IssueObjectType.StoredProcedure, procedure.Id);

            storedProcNode.Name = string.Format("{0}.{1}", procedure.Package.Name, procedure.ProcedureName);

            return storedProcNode;
        }

        private bool DoProceduresMatch(StoredProcedure procedure, PLSQLProcedure parsedProcedure, out string noMatchResult)
        {
            bool match = false;

            noMatchResult = string.Empty;

            if (procedure != null && parsedProcedure != null)
            {
                StoredProcedure readProc = null;

                // Check if ref cursor procedure
                if (procedure.IsReturningRefCursor ?? false)
                {
                    // Create the ref cursor procedure
                    RefCurStoredProcedure refCurProc = RefCurStoredProcedure.Create(procedure.ProcedureName, procedure.Package.Name, procedure.Package.Schema.ConnectionString);

                    // Check if procedure is valid
                    if (refCurProc.Status == RefCurStoredProcedureStatus.Valid)
                    {
                        // Convert the RefCurStoredProcedure to a StoredProcedure
                        readProc = StoredProcedureHelper.CreateStoredProcedure(parsedProcedure, refCurProc);
                    }
                    else
                    {
                        noMatchResult = string.Format("Error when trying to import procedure {0} for matching that has a Ref Cursor parameter!" + Environment.NewLine + Environment.NewLine, refCurProc.Name) +
                                        string.Format("Status: {0}", refCurProc.Status.ToString()) + Environment.NewLine + Environment.NewLine +
                                                      refCurProc.ErrorText;

                        return false;
                    }
                }
                else
                {
                    readProc = StoredProcedureHelper.CreateStoredProcedure(parsedProcedure);
                }

                if (readProc != null)
                {
                    // Check that IsReturningRefCursor flag hasn't changed or we can't support this.
                    if ((procedure.IsReturningRefCursor ?? false) != (readProc.IsReturningRefCursor ?? false))
                    {
                        if (procedure.IsReturningRefCursor ?? false)
                            noMatchResult = string.Format("It's not possible to update a procedure when it has changed from beeing\n" +
                                                          "a procedure with a ref cursor to not having a ref cursor as a parameter.\n" +
                                                          "You have to delete the initial stored procedure {0} and import it again.", procedure.ProcedureName);
                        else
                            noMatchResult = string.Format("It's not possible to update a procedure when it has changed from not having\n" +
                                                          "a ref cursor to now having a ref cursor as a parameter.\n\n" +
                                                          "You have to delete the initial stored procedure and import it again.", procedure.ProcedureName);

                        return false;
                    }

                    // Check name of ref cursor parameter
                    if ((procedure.IsReturningRefCursor ?? false) &&
                        procedure.RefCursorParameterName != readProc.RefCursorParameterName)
                    {
                        noMatchResult = "The Ref Cursor parameter name has changed!";

                        return false;
                    }

                    if (procedure.ProcedureName == readProc.ProcedureName &&
                        procedure.Properties.Count == readProc.Properties.Count)
                    {
                        int errorCount = 0;
                        int lastSequence = -1;
                        string parameterErrors = string.Empty;

                        match = true;
                        bool sequenceError = false;

                        foreach (ProcedureProperty property in procedure.Properties.OrderBy(p => p.Sequence))
                        {
                            if (!sequenceError)
                            {
                                if (lastSequence == -1)
                                {
                                    if (property.Sequence != 1)
                                    {
                                        match = false;
                                        sequenceError = true;
                                        parameterErrors += "Sequence is wrong, doesn't start with one (1)!" + Environment.NewLine;
                                        errorCount++;
                                    }
                                }
                                else if (property.Sequence == lastSequence)
                                {
                                    match = false;
                                    sequenceError = true;
                                    parameterErrors += "Sequences doesn't match!" + Environment.NewLine;
                                    errorCount++;
                                }

                                lastSequence = property.Sequence;
                            }

                            // Create a ProcedureProperty from the parsedProcedure properties
                            IList<ProcedureProperty> parameters = readProc.Properties.Where(p => p.Name == property.Name).ToList();

                            if (parameters.Count == 0)
                            {
                                match = false;
                                parameterErrors += string.Format("The parameter \"{0}\" doesn't exist!", property.Name) + Environment.NewLine;
                                errorCount++;
                                continue;
                            }
                            else if (parameters.Count > 1)
                            {
                                match = false;
                                parameterErrors += string.Format("More than one parameter named \"{0}\"!", property.Name) + Environment.NewLine;
                                errorCount++;
                                continue;
                            }

                            ProcedureProperty parameter = parameters[0];

                            try
                            {
                                property.CheckIfEqual(parameter);
                            }
                            catch (Exception ex)
                            {
                                match = false;
                                parameterErrors += ex.Message + Environment.NewLine;
                                errorCount++;
                            }
                        }

                        if (!match)
                        {
                            noMatchResult = string.Format("{0} of {1} parameter(s) doesn't match." + Environment.NewLine +
                                                          "{2}",
                                                          errorCount,
                                                          procedure.Properties.Count,
                                                          parameterErrors);
                        }
                    }
                    else
                    {
                        noMatchResult = "The number of parameters differs!";
                    }
                }
            }

            return match;
        }

        private void DeleteNonIssues(Guid applicationId, AnalyzeIssueNode node, IssueObjectType issueObjectType)
        {
            DeleteNonIssues(applicationId, node, issueObjectType, Guid.Empty);
        }

        [Transaction(ReadOnly = false)]
        private void DeleteNonIssues(Guid applicationId, AnalyzeIssueNode node, IssueObjectType issueObjectType, Guid issueObjectId)
        {
            if (node != null)
            {
                IList<Issue> issues = node.GetAllIssuesBelowNode(issueObjectType);

                if (issues.Count > 0)
                {
                    IssueDao.DeleteIssues(applicationId, issueObjectType, issueObjectId, issues);
                }
            }
        }

        [Transaction(ReadOnly = false)]
        public void DeleteIssues(Guid applicationId, IssueObjectType objectType, Guid objectId)
        {
            IssueDao.DeleteIssues(applicationId, objectType, objectId);
        }

        [Transaction(ReadOnly = true)]
        public Issue FindIssue(Guid applicationId, IssueObjectType objectType, Guid objectId, string title, string text)
        {
            return IssueDao.FindIssue(applicationId, objectType, objectId, title, text);
        }

        [Transaction(ReadOnly = true)]
        public IList<Issue> FindAllIssues(Guid applicationId)
        {
            return IssueDao.FindAllIssues(applicationId);
        }

        [Transaction(ReadOnly = true)]
        public IList<Issue> FindAllIssuesByType(Guid applicationId, IssueObjectType type)
        {
            IList<Issue> issueList = IssueDao.FindAllIssuesByType(applicationId, type);

            // Check if bugtype then fetch some more information
            if (type == IssueObjectType.Bug)
            {
                foreach (Issue issue in issueList)
                {
                    Dialog dialog = DialogDao.FindById(issue.ObjectId);
                    issue.DialogCreator = dialog.CreatorName;
                }
            }

            return issueList;
        }

        [Transaction(ReadOnly = true)]
        public IList<Issue> FindAllIssuesByTypeAndObjectId(Guid applicationId, IssueObjectType type, Guid objectId)
        {
            return IssueDao.FindAllIssuesByTypeAndObjectId(applicationId, type, objectId);
        }

        [Transaction(ReadOnly = false)]
        public Issue SaveOrUpdateIssue(Issue issue)
        {
            return IssueDao.SaveOrUpdate(issue);
        }

        private void DoCheckDialog(AnalyzeIssueNode parentNode, Application application, Dialog dialog)
        {
            Callback.Next(string.Format("Checking Dialog ({0} / {1}) - {2}: ",
                                        CallbackService.C_CURRENTSTEP,
                                        CallbackService.C_MAXSTEP,
                                        dialog.Name), true);

            // Create the topnode
            AnalyzeIssueNode topNode = new AnalyzeIssueNode(application, IssueObjectType.Dialog, dialog.Id);

            topNode.Name = dialog.Name;

            // First check that the dialog points to a view with an interface
            if (dialog.InterfaceView == null)
            {
                topNode.IssueList.AddError("Dialog is missing an InterfaceView.", dialog);
            }

            // Check searchpanel
            if (dialog.Type == DialogType.Overview)
            {
                if (dialog.SearchPanelView == null)
                {
                    topNode.IssueList.AddError("Dialog is missing a SearchPanel.", dialog);
                }
                else
                {
                    Callback.Update("Checking SearchPanel");
                    View searchPanelView = MetaManagerServices.GetModelService().GetInitializedDomainObject<View>(dialog.SearchPanelView.Id);
                    View interfaceView = MetaManagerServices.GetModelService().GetInitializedDomainObject<View>(dialog.InterfaceView.Id);
                    // Add this ViewNode to the parent node.
                    AnalyzeIssueNode currentNode = topNode.AddChild(string.Format("SearchPanelView - ({0}) {1}", searchPanelView.Id, searchPanelView.Name), searchPanelView);

                    // Check the searchpanel
                    CheckView(searchPanelView, currentNode, true);

                    // Check that the searchpanels requestmap is the same as the interface views requestmap
                    if (interfaceView.RequestMap.Id != searchPanelView.RequestMap.Id)
                    {
                        currentNode.IssueList.AddError(string.Format("The RequestMap ({0}) for the searchpanel is not the same as the Interface Views RequestMap ({1}).",
                                                                     searchPanelView.RequestMap.Id,
                                                                     interfaceView.RequestMap.Id));
                    }
                }
            }

            // Check if there are any double actions in this Dialog
            IList<UXAction> nonUniqueActionList = UXActionDao.FindNonUniqueActionsByDialogId(dialog.Id);

            if (nonUniqueActionList.Count > 0)
            {
                foreach (UXAction action in nonUniqueActionList)
                {
                    topNode.IssueList.AddError(string.Format("The Action ({0}) \"{1}\" is defined more than once in this dialog.", action.Id, action.Name), action);
                }
            }

            ViewNode currentViewNode = dialog.RootViewNode;

            CheckViewNodeTree(topNode, dialog, currentViewNode);

            if (topNode.GetAllIssuesBelowNode(IssueObjectType.Dialog).Count > 0)
                parentNode.Children.Add(topNode);
        }

        private void CheckViewNodeTree(AnalyzeIssueNode parentNode, Dialog dialog, ViewNode viewNode)
        {
            Callback.Update(string.Format("Checking ViewNode for View \"{0}\"", viewNode.View.Name));

            // Add this ViewNode to the parent node.
            AnalyzeIssueNode currentNode = parentNode.AddChild(string.Format("ViewNode - ({0})", viewNode.Id), viewNode);

            // Check the view-to-view map on the viewnode
            if (viewNode.ViewMap != null)
            {
                CheckPropertyMap(viewNode.ViewMap, currentNode, "View-To-View Map", false, true);

                CheckPropertyMaps(viewNode.View.RequestMap, viewNode.ViewMap, currentNode.AddChild(string.Format("View-To-View Map ({0})", viewNode.ViewMap.Id)));
            }
            else if (viewNode.ViewMap == null || viewNode.ViewMap.MappedProperties.Count == 0)
            {
                // If there is mappedproperties in requestmap for the view then
                // there should be a view-to-view map if we can find a parent with
                // a responsemap.
                if (viewNode.View.RequestMap != null && viewNode.View.RequestMap.MappedProperties.Count > 0)
                {
                    ViewNode upNode = viewNode.Parent;

                    while (upNode != null)
                    {
                        if ((upNode.View.ResponseMap != null) && (upNode.View.ResponseMap.MappedProperties.Count > 0))
                            break;

                        upNode = upNode.Parent;
                    }

                    if (upNode != null)
                    {
                        currentNode.IssueList.AddError("The View-To-View map is missing!", viewNode);
                    }
                }
            }

            // Check the ViewActions
            AnalyzeIssueNode actionNode = currentNode.AddChild("ViewActions");

            foreach (ViewAction viewAction in viewNode.ViewActions)
            {
                AnalyzeIssueNode currentActionNode = actionNode.AddChild(string.Format("ViewAction ({0}) connected to Action ({1}) \"{2}\"",
                                                                         viewAction.Id,
                                                                         viewAction.Action.Id,
                                                                         viewAction.Action.Name));

                // Check if ViewAction is of type PlaceHolder, there shouldn't be any of those
                // since they should be replaced.
                if (viewAction.Type == ViewActionType.PlaceHolder)
                {
                    currentActionNode.IssueList.AddError("The ViewAction is a Placeholder and should be replaced with the correct Action!");
                }
                else
                {
                    // In an Overview and Drilldown only allow "None" and "Update" actions
                    if (dialog.Type == DialogType.Overview || dialog.Type == DialogType.Drilldown)
                    {
                        // Check all actions that if they are starting a new dialog they should have the DialogResult
                        // set to "None" and nothing else.
                        if (viewAction.Action.Dialog != null &&
                            viewAction.Action.DialogResult != UXDialogResult.None)
                        {
                            currentActionNode.IssueList.AddError("The ViewAction has the wrong DialogResult set! It should be set to \"None\".");
                        }

                        if (viewAction.Action.ServiceMethod != null &&
                            viewAction.Action.DialogResult != UXDialogResult.Update &&
                            viewAction.Action.ServiceMethod.Name.ToUpper().StartsWith("DELETE"))
                        {
                            currentActionNode.IssueList.AddWarning(string.Format("The ViewAction is starting the ServiceMethod ({0}) \"{1}\". If this is a delete action then it should have the DialogResult set to \"Update\".",
                                                                                 viewAction.Action.ServiceMethod.Id,
                                                                                 viewAction.Action.ServiceMethod.Name), viewAction.Action);
                        }
                    }
                    else if (dialog.Type == DialogType.Create || dialog.Type == DialogType.Modify)
                    {
                        if (viewAction.Action.ServiceMethod != null &&
                            viewAction.Action.DialogResult != UXDialogResult.Ok)
                        {
                            currentActionNode.IssueList.AddWarning(string.Format("The ViewAction is starting the ServiceMethod ({0}) \"{1}\". If this is the Servicemethod that saves the result then it should have the DialogResult set to \"OK\".",
                                                                                 viewAction.Action.ServiceMethod.Id,
                                                                                 viewAction.Action.ServiceMethod.Name), viewAction.Action);
                        }
                    }

                    // Check if the action is updated/syncronized
                    if (viewAction.Action.MappedToObject != null)
                    {
                        PropertyMap map = null;
                        AnalyzeIssueNode actionMapNode = null;

                        switch (viewAction.Action.MappedToObject.ActionType)
                        {
                            case UXActionType.CustomDialog:
                                break;
                            case UXActionType.Dialog:

                                if (viewAction.Action.Dialog.InterfaceView != null &&
                                    viewAction.Action.Dialog.InterfaceView.RequestMap != null)
                                {
                                    map = viewAction.Action.Dialog.InterfaceView.RequestMap;

                                    actionMapNode = currentActionNode.AddChild(string.Format("RequestMap ({0}) for Dialog ({1}) \"{2}\"",
                                                                               map.Id,
                                                                               viewAction.Action.Dialog.Id,
                                                                               viewAction.Action.Dialog.Name));
                                }
                                else
                                {
                                    currentActionNode.IssueList.AddError(string.Format("The ViewAction is starting the Dialog ({0}) \"{1}\" which is missing an InterfaceView and/or a RequestMap.",
                                                                                viewAction.Action.Dialog.Id,
                                                                                viewAction.Action.Dialog.Name), viewAction.Action);
                                }
                                break;

                            case UXActionType.ServiceMethod:

                                if (viewAction.Action.ServiceMethod.RequestMap != null)
                                {
                                    map = viewAction.Action.ServiceMethod.RequestMap;

                                    actionMapNode = currentActionNode.AddChild(string.Format("RequestMap ({0}) for ServiceMethod ({1}) \"{2}\"",
                                                                               map.Id,
                                                                               viewAction.Action.ServiceMethod.Id,
                                                                               viewAction.Action.ServiceMethod.Name));
                                }
                                else
                                {
                                    currentActionNode.IssueList.AddError(string.Format("The ViewAction is starting the ServiceMethod ({0}) \"{1}\" which is missing a RequestMap.",
                                                                                viewAction.Action.ServiceMethod.Id,
                                                                                viewAction.Action.ServiceMethod.Name), viewAction.Action);
                                }
                                break;

                            case UXActionType.Workflow:

                                if (viewAction.Action.Workflow.RequestMap != null)
                                {
                                    map = viewAction.Action.Workflow.RequestMap;

                                    actionMapNode = currentActionNode.AddChild(string.Format("RequestMap ({0}) for Workflow ({1}) \"{2}\"",
                                                                               map.Id,
                                                                               viewAction.Action.Workflow.Id,
                                                                               viewAction.Action.Workflow.Name));
                                }
                                else
                                {
                                    currentActionNode.IssueList.AddError(string.Format("The ViewAction is starting the Workflow ({0}) \"{1}\" which is missing a RequestMap.",
                                                                                viewAction.Action.Workflow.Id,
                                                                                viewAction.Action.Workflow.Name), viewAction.Action);
                                }
                                break;
                        }

                        // There is no source map for the CustomDialog. It's defined for the Action.
                        if (viewAction.Action.MappedToObject.ActionType != UXActionType.CustomDialog)
                        {
                            // Check the map if it's set
                            if (map != null &&
                                viewAction.Action.RequestMap != null)
                            {
                                CheckPropertyMaps(map,
                                                  viewAction.Action.RequestMap,
                                                  actionMapNode);
                            }
                            else
                            {
                                // Either action has a map and the source of the action hasn't or the other way around.
                                // It's ok if both are null since then a map isn't needed anyway.
                                if (map == null)
                                {
                                    if (viewAction.Action.RequestMap != null)
                                    {
                                        actionNode.IssueList.AddError(string.Format("The Action ({0}) \"{1}\", that the ViewAction ({2}) is connected to, should not have a RequestMap since the source to the Action doesn't have one.",
                                                                                    viewAction.Action.Id,
                                                                                    viewAction.Action.Name,
                                                                                    viewAction.Id), viewAction.Action);
                                    }
                                }
                                else if (viewAction.Action.RequestMap == null)
                                {
                                    if (map != null)
                                    {
                                        actionNode.IssueList.AddError(string.Format("The Action ({0}) \"{1}\", that the ViewAction ({2}) is connected to, is required to have a RequestMap since the source to the Action have one.",
                                                                                    viewAction.Action.Id,
                                                                                    viewAction.Action.Name,
                                                                                    viewAction.Id), viewAction.Action);
                                    }
                                }
                            }
                        }

                        // Now check the Actions RequestMap with the ViewActions map to that map.
                        if (viewAction.Action.RequestMap != null &&
                            viewAction.ViewToActionMap != null)
                        {
                            CheckPropertyMaps(viewAction.Action.RequestMap,
                                              viewAction.ViewToActionMap,
                                              currentActionNode.AddChild(string.Format("RequestMap ({0}) for the Action",
                                                                         map.Id,
                                                                         viewAction.Action.Id,
                                                                         viewAction.Action.Name)));
                        }
                        else
                        {
                            // Either Action have no requestmap (not so likely) or the ViewAction
                            // have no map to the action.
                            if (viewAction.Action.RequestMap == null)
                            {
                                if (viewAction.ViewToActionMap != null)
                                {
                                    actionNode.IssueList.AddError(string.Format("The ViewAction ({0}) have a ViewToActionMap but the Action ({1}) \"{2}\" doesn't require any parameters (RequestMap is null).",
                                                                                viewAction.Id,
                                                                                viewAction.Action.Id,
                                                                                viewAction.Action.Name), viewAction);
                                }
                            }
                            else if (viewAction.ViewToActionMap == null)
                            {
                                if (viewAction.Action.RequestMap != null)
                                {
                                    actionNode.IssueList.AddError(string.Format("The ViewAction ({0}) is missing the map (ViewToActionMap) to the Action ({1}) \"{2}\" which is required.",
                                                                                viewAction.Id,
                                                                                viewAction.Action.Id,
                                                                                viewAction.Action.Name), viewAction);
                                }
                            }
                        }
                    }

                    if (viewAction.ViewToActionMap != null)
                    {
                        CheckPropertyMap(viewAction.ViewToActionMap, actionNode, "View-To-Action Map", false, true);
                    }
                }
            }


            // Check the view
            CheckView(currentNode, dialog, viewNode);

            // Check viewnodes childrens.
            if (viewNode.Children.Count > 0)
            {
                foreach (ViewNode child in viewNode.Children)
                {
                    CheckViewNodeTree(parentNode, dialog, child);
                }
            }

        }

        private void CheckPropertyMaps(PropertyMap fromMap, PropertyMap toMap, AnalyzeIssueNode issueNode)
        {
            if (fromMap != null)
            {
                fromMap = MetaManagerServices.GetModelService().GetInitializedDomainObject<PropertyMap>(fromMap.Id);
            }
            if (toMap != null)
            {
                toMap = MetaManagerServices.GetModelService().GetInitializedDomainObject<PropertyMap>(toMap.Id);
            }


            // Check the map if it's set
            if (fromMap != null &&
                toMap != null)
            {
                foreach (MappedProperty fromProp in fromMap.MappedProperties)
                {
                    // Try to find a link to the fromproperty from any property in the tomap
                    bool foundProp = (from MappedProperty p in toMap.MappedProperties
                                      where p.Source is MappedProperty &&
                                            p.Source.Id == fromProp.Id
                                      select p).Count() == 1;



                    if (!foundProp)
                    {
                        issueNode.IssueList.AddError(string.Format("The parameter ({0}) \"{1}\" is missing!",
                                                                    fromProp.Id,
                                                                    fromProp.Name), fromProp);
                    }

                }

            }
            else
            {
                // Either action has a map and the source of the action hasn't or the other way around.
                // It's ok if both are null since then a map isn't needed anyway.
                if (fromMap == null)
                {
                    if (toMap != null)
                    {
                        issueNode.IssueList.AddError(string.Format("The FromMap (source) doesn't require any parameters (MappedProperty) since it's set to null, but there is a ToMap defined!"));
                    }
                }
                else if (toMap == null)
                {
                    if (fromMap != null)
                    {
                        issueNode.IssueList.AddError(string.Format("The FromMap (source) has a map defined but the ToMap is not (null)!"));
                    }
                }
            }
        }

        private void CheckView(AnalyzeIssueNode parentNode, Dialog dialog, ViewNode viewNode)
        {
            // Add this View to the parent node.
            AnalyzeIssueNode currentNode = parentNode.AddChild(string.Format("View - ({0}) {1} \"{2}\"", viewNode.View.Id, viewNode.View.Name, viewNode.View.Title), viewNode.View);

            if (viewNode.View.Type == ViewType.Standard)
            {
                CheckView(viewNode.View, currentNode, false);
            }
        }

        private void CheckView(View view, AnalyzeIssueNode currentNode, bool IsSearchPanel)
        {
            // Check if ServiceMethod is set
            if (!IsSearchPanel && view.ServiceMethod == null)
            {
                currentNode.IssueList.AddError("The View is missing a ServiceMethod!", view);
            }
            else
            {
                if (view.RequestMap == null)
                {
                    currentNode.IssueList.AddError("The View is missing a Request Map!", view);
                }
                else
                {
                    CheckPropertyMap(view.RequestMap, currentNode.AddChild(string.Format("RequestMap ({0})", view.RequestMap.Id)), "Views RequestMap", true, false);

                    if (!IsSearchPanel)
                    {
                        CheckPropertyMaps(view.ServiceMethod.RequestMap,
                                          view.RequestMap,
                                          currentNode.AddChild(string.Format("Checking RequestMap ({0}): From ServiceMethod ({1}) \"{2}\"",
                                                                             view.RequestMap.Id,
                                                                             view.ServiceMethod.Id,
                                                                             view.ServiceMethod.Name)));
                    }
                }

                if (view.ResponseMap == null)
                {
                    currentNode.IssueList.AddError("The View is missing a Response Map!", view);
                }
                else
                {
                    CheckPropertyMap(view.ResponseMap, currentNode.AddChild(string.Format("ResponseMap ({0})", view.ResponseMap.Id)), "Views ResponseMap", true, false);

                    if (!IsSearchPanel)
                    {
                        CheckPropertyMaps(view.ServiceMethod.ResponseMap,
                                          view.ResponseMap,
                                          currentNode.AddChild(string.Format("Checking ResponseMap ({0}): From ServiceMethod ({1}) \"{2}\"",
                                                                             view.ResponseMap.Id,
                                                                             view.ServiceMethod.Id,
                                                                             view.ServiceMethod.Name)));
                    }
                }
            }

            // Fetch the view by using the service, this is done since the VisualTree won't be unpacked
            // correctly if we don't.
            View workingView = DialogService.GetViewById(view.Id);

            // Check the components if they are all mapped.
            AnalyzeIssueNode componentNode = currentNode.AddChild("Components");

            List<Guid> serviceMethodsInServiceComponents = new List<Guid>();
            IList<IBindable> bindableComponentsInWorkingView = null;

            // Check if we have a Visual Tree
            if (workingView.VisualTree != null)
            {
                // Check components in the VisualTree
                CheckComponents(workingView, workingView.VisualTree, componentNode, serviceMethodsInServiceComponents);

                // Get a list with all bindable components, this will be used later for check of datasources
                bindableComponentsInWorkingView = GetAllBindableComponents(workingView.VisualTree);
            }
            else
            {
                componentNode.IssueList.AddError("The View hasn't been converted to have a VisualTree!", view);
            }

            // List of mapped property ids that is beeing updated by the datasources. 
            // Only one id can be updated by a datasource. If there is more than one trying to update the
            // same value then you will get a race situation.
            Dictionary<Guid, DataSource> updatedPropertyIds = new Dictionary<Guid, DataSource>();

            // Check datasources
            foreach (DataSource dataSource in view.DataSources)
            {
                AnalyzeIssueNode dataSourceNode = currentNode.AddChild(string.Format("DataSource ({0}) \"{1}\"", dataSource.Id, dataSource.Name));

                // Requestmap - From servicemethods requestmap
                CheckPropertyMaps(dataSource.ServiceMethod.RequestMap, dataSource.RequestMap, dataSourceNode.AddChild("RequestMap"));

                // Responsemap - From views responsemap
                CheckPropertyMaps(dataSource.View.ResponseMap, dataSource.ResponseMap, dataSourceNode.AddChild("ResponseMap"));

                if (dataSource.RequestMap.MappedProperties.Count == 0 &&
                    !serviceMethodsInServiceComponents.Contains(dataSource.ServiceMethod.Id))
                {
                    currentNode.IssueList.AddWarning(string.Format("The View has a DataSource ({0}) that doesn't have any parameters in the RequestMap which will result in that it will only be updated once!", dataSource.Name));
                }

                // Loop the response map to find the mapped properties
                foreach (MappedProperty prop in dataSource.ResponseMap.MappedProperties)
                {
                    if (prop.IsEnabled &&
                        prop.Source != null &&
                        prop.Source is MappedProperty)
                    {
                        if (updatedPropertyIds.ContainsKey(prop.Source.Id))
                        {
                            currentNode.IssueList.AddError(string.Format("The View has a DataSource \"{0}\" that is updating the field {1} in the response map. " +
                                                                         "This field is also updated by the DataSource \"{2}\". Only one DataSource may update one field!",
                                                                         dataSource.Name,
                                                                         prop.Name,
                                                                         updatedPropertyIds[prop.Source.Id].Name));
                        }
                        else
                            updatedPropertyIds.Add(prop.Source.Id, dataSource);

                        // Check if datasource not have defaults for not nullable components
                        if (bindableComponentsInWorkingView != null &&
                            prop.Target != null &&
                            string.IsNullOrEmpty(prop.DefaultValue))
                        {
                            IBindable component = bindableComponentsInWorkingView.Where(c => c.MappedProperty != null && c.MappedProperty.Id == prop.Source.Id).FirstOrDefault();

                            if (component != null)
                            {
                                PropertyInfo nullableProp = component.GetType().GetProperty("IsNullable");

                                if (nullableProp != null)
                                {
                                    bool nullableValue = (bool)nullableProp.GetValue(component, null);

                                    if (!nullableValue)
                                    {
                                        currentNode.IssueList.AddWarning(string.Format("The View has a DataSource \"{0}\" that is updating the field {1} in the response map. " +
                                                                                       "This field has no DefaultValue and the component it is bound to don't accept a null value (IsNullable is not set).",
                                                                                       dataSource.Name,
                                                                                       prop.Name));
                                    }
                                }
                            }
                        }


                    }
                }

            }
        }


        private void CheckPropertyMap(PropertyMap propertyMap, AnalyzeIssueNode currentNode, string mapName, bool requireTarget, bool ignoreDuplicateNames)
        {
            if (!ignoreDuplicateNames)
            {
                // Find non unique names in the propertymap
                IEnumerable<string> nonUniqueNames = from property in propertyMap.MappedProperties
                                                     where !string.IsNullOrEmpty(property.Name)
                                                     group property by property.Name into grouped
                                                     where grouped.Count() > 1
                                                     select grouped.Key;

                if (nonUniqueNames.Count() > 0)
                {
                    foreach (string name in nonUniqueNames)
                    {
                        currentNode.IssueList.AddError(string.Format("One or more MappedProperties has the same name \"{0}\" in the {1}.", name, mapName));
                    }
                }
            }

            foreach (MappedProperty mappedProperty in propertyMap.MappedProperties)
            {
                if ((mappedProperty.Target == null) && requireTarget)
                    currentNode.IssueList.AddError(string.Format("Property \"{0}\" must have a target property specified in the {1}.", mappedProperty.Source.Name, mapName), mappedProperty);
                else if (!mappedProperty.IsMapped && mappedProperty.IsEnabled)
                    currentNode.IssueList.AddError(string.Format("Property \"{0}\" must have a target property or default value in the {1}.", mappedProperty.Source.Name, mapName), mappedProperty);

                if (!string.IsNullOrEmpty(mappedProperty.DefaultValue))
                {
                    try
                    {
                        Type type = mappedProperty.Type;

                        if (type == null)
                            type = mappedProperty.Source.Type;

                        Convert.ChangeType(mappedProperty.DefaultValue, type);
                    }
                    catch (FormatException ex)
                    {
                        currentNode.IssueList.AddError(string.Format("Property \"{0}\" specified in the {1} has an invalid default value: \"{2}\". Error: {3}", mappedProperty.Source.Name, mapName, mappedProperty.DefaultValue, ex.Message), mappedProperty);
                    }
                    catch (InvalidCastException ex)
                    {
                        currentNode.IssueList.AddError(string.Format("Property \"{0}\" specified in the {1} has an invalid default value: \"{2}\". Error: {3}", mappedProperty.Source.Name, mapName, mappedProperty.DefaultValue, ex.Message), mappedProperty);
                    }


                }

            }
        }

        private void CheckComponents(View checkingView, UXComponent component, AnalyzeIssueNode currentNode, IList<Guid> serviceMethodsInServiceComponents)
        {
            // Check if the component is mappable, in that case it should be mapped
            if (component is IBindable)
            {
                IBindable bindable = component as IBindable;

                if (bindable.MappedProperty == null)
                    currentNode.IssueList.AddError(string.Format("The component \"{0}\" is not mapped to a property!", UXComponent.GetComponentNamePath(component)), component);

                if (component.Hint == null)
                    currentNode.IssueList.AddWarning(string.Format("The component \"{0}\" has no hint!", UXComponent.GetComponentNamePath(component)), component);


            }

            // Check if Service component (ComboBox for example)
            if (component is UXServiceComponent)
            {
                UXServiceComponent serviceComponent = component as UXServiceComponent;

                // Check if a servicemethod is connected to it
                if (serviceComponent.ServiceMethod == null)
                {
                    currentNode.IssueList.AddError(string.Format("The component \"{0}\" is not connected to a ServiceMethod.", UXComponent.GetComponentNamePath(component)), component);
                }
                else
                {
                    // Add the identity of the servicemethod identity to the list for later comparison.
                    if (!serviceMethodsInServiceComponents.Contains(serviceComponent.ServiceMethod.Id))
                    {
                        serviceMethodsInServiceComponents.Add(serviceComponent.ServiceMethod.Id);
                    }

                    if (serviceComponent.ComponentMap == null)
                    {
                        currentNode.IssueList.AddError(string.Format("The component \"{0}\" is connected to the ServiceMethod ({1}) \"{2}\" but it has not been mapped."
                                                        , UXComponent.GetComponentNamePath(component)
                                                        , serviceComponent.ServiceMethod.Id.ToString()
                                                        , serviceComponent.ServiceMethod.Name)
                                                        , component);
                    }
                    else
                    {
                        AnalyzeIssueNode node = currentNode.AddChild(string.Format("ComponentMap ({0})", serviceComponent.ComponentMap.Id));

                        CheckPropertyMap(serviceComponent.ComponentMap, node, "Component Map", false, false);

                        CheckPropertyMaps(serviceComponent.ServiceMethod.RequestMap,
                                          serviceComponent.ComponentMap,
                                          node);
                    }
                }
            }
            else if (component is UXTwoWayListBox)
            {
                UXTwoWayListBox uxTWLB = (UXTwoWayListBox)component;

                if (uxTWLB.LeftFindServiceMethod != null && uxTWLB.LeftFindServiceMethodMap != null)
                {
                    // Check the left map
                    CheckPropertyMaps(uxTWLB.LeftFindServiceMethod.RequestMap, uxTWLB.LeftFindServiceMethodMap, currentNode.AddChild(string.Format("UXTwoWayListBox \"{0}\" - LeftFindServiceMethodMap ({1})", uxTWLB.Name, uxTWLB.LeftFindServiceMethodMap.Id)));
                }
                else
                {
                    currentNode.IssueList.AddError(string.Format("The UXTwoWayListBox \"{0}\" is missing the Left Find ServiceMethod or the mapping for this!",
                                                                 uxTWLB.Name));
                }

                if (uxTWLB.RightFindServiceMethod != null && uxTWLB.RightFindServiceMethodMap != null)
                {
                    // Check the right map
                    CheckPropertyMaps(uxTWLB.RightFindServiceMethod.RequestMap, uxTWLB.RightFindServiceMethodMap, currentNode.AddChild(string.Format("UXTwoWayListBox \"{0}\" - RightFindServiceMethodMap ({1})", uxTWLB.Name, uxTWLB.RightFindServiceMethodMap.Id)));
                }
                else
                {
                    currentNode.IssueList.AddError(string.Format("The UXTwoWayListBox \"{0}\" is missing the Right Find ServiceMethod or the mapping for this!",
                                                                 uxTWLB.Name));
                }

                if (uxTWLB.AddServiceMethod != null)
                {
                    // Check the add map
                    CheckPropertyMaps(uxTWLB.AddServiceMethod.RequestMap, uxTWLB.AddServiceMethodMap, currentNode.AddChild(string.Format("UXTwoWayListBox \"{0}\" - AddServiceMethodMap ({1})", uxTWLB.Name, uxTWLB.AddServiceMethodMap.Id)));
                }
                else
                {
                    currentNode.IssueList.AddError(string.Format("The UXTwoWayListBox \"{0}\" is missing the Add ServiceMethod!",
                                                                 uxTWLB.Name));
                }

                if (uxTWLB.RemoveServiceMethod != null)
                {
                    // Check the add map
                    CheckPropertyMaps(uxTWLB.RemoveServiceMethod.RequestMap, uxTWLB.RemoveServiceMethodMap, currentNode.AddChild(string.Format("UXTwoWayListBox \"{0}\" - RemoveServiceMethodMap ({1})", uxTWLB.Name, uxTWLB.RemoveServiceMethodMap.Id)));
                }
                else
                {
                    currentNode.IssueList.AddError(string.Format("The UXTwoWayListBox \"{0}\" is missing the Remove ServiceMethod!",
                                                                 uxTWLB.Name));
                }
            }
            else if (component is UXComboDialog)
            {
                UXComboDialog uxCD = (UXComboDialog)component;

                // Check if ComboDialog has a dialog
                if (uxCD.Dialog == null)
                {
                    currentNode.IssueList.AddError(string.Format("The UXComboDialog \"{0}\" is not connected to a Dialog!",
                                                                 uxCD.Name));
                }
                else
                {
                    // Check the ViewMap
                    if (uxCD.Dialog.InterfaceView != null)
                    {
                        if (uxCD.ViewMap != null)
                        {
                            CheckPropertyMaps(uxCD.Dialog.InterfaceView.RequestMap, uxCD.ViewMap, currentNode.AddChild(string.Format("UXComboDialog \"{0}\" - ViewMap ({1})", uxCD.Name, uxCD.ViewMap.Id)));
                        }
                        else
                        {
                            currentNode.IssueList.AddError(string.Format("The UXComboDialog \"{0}\" is not mapped, the RequestMap (ViewMap) is missing!",
                                                                         uxCD.Name));
                        }
                    }
                    else
                    {
                        currentNode.IssueList.AddError(string.Format("The UXComboDialog \"{0}\" is connected to the Dialog ({1}) \"{2}\" which is missing an InterfaceView!",
                                                                     uxCD.Name,
                                                                     uxCD.Dialog.Id,
                                                                     uxCD.Dialog.Name));
                    }

                    if (uxCD.ResultMap != null)
                    {
                        // Check the resultmap
                        CheckPropertyMaps(checkingView.ResponseMap, uxCD.ResultMap, currentNode.AddChild(string.Format("UXComboDialog \"{0}\" - ResultMap ({1})", uxCD.Name, uxCD.ResultMap.Id)));
                    }
                    else
                    {
                        currentNode.IssueList.AddError(string.Format("The UXComboDialog \"{0}\" is not mapped, the ResponseMap (ResultMap) is missing!",
                                                                     uxCD.Name));
                    }
                }
            }

            UXContainer container = null;

            if (component is UXGroupBox)
                container = (component as UXGroupBox).Container;
            else
                container = component as UXContainer;

            if (container != null)
            {
                foreach (UXComponent child in container.Children)
                    CheckComponents(checkingView, child, currentNode, serviceMethodsInServiceComponents);
            }
        }

        private IList<IBindable> GetAllBindableComponents(UXComponent component)
        {
            List<IBindable> bindableComponentsInVisualTree = new List<IBindable>();

            // Check if the component is mappable, in that case it should be added to list
            if (component is IBindable)
            {

                bindableComponentsInVisualTree.Add(component as IBindable);

            }

            UXContainer container = null;

            if (component is UXGroupBox)
                container = (component as UXGroupBox).Container;
            else
                container = component as UXContainer;

            if (container != null)
            {
                foreach (UXComponent child in container.Children)
                    bindableComponentsInVisualTree.AddRange(GetAllBindableComponents(child));
            }

            return bindableComponentsInVisualTree;
        }


        private void DoCheckServiceMethod(AnalyzeIssueNode parentNode, Application application, ServiceMethod serviceMethod)
        {
            AnalyzeIssueNode topNode = new AnalyzeIssueNode(application, IssueObjectType.ServiceMethod, serviceMethod.Id);

            AnalyzeIssueNode currentNode = topNode;

            Callback.Next(string.Format("Checking ServiceMethod ({0} / {1}) - ({2}) {3}",
                                        CallbackService.C_CURRENTSTEP,
                                        CallbackService.C_MAXSTEP,
                                        serviceMethod.Id,
                                        serviceMethod.Name));

            // Check if ServiceMethods RequestMap is compatible with the Actions RequestMap.
            topNode.Name = serviceMethod.Name;

            if (serviceMethod.MappedToAction != null)
            {
                // Check requestmap and responsemap
                currentNode = topNode.AddChild(string.Format("RequestMap : ServiceMethod Map ({0}) <-> Action Map ({1})", serviceMethod.RequestMap.Id, serviceMethod.MappedToAction.RequestMap.Id), null);
                CheckActionToServicePropertyMap(currentNode.IssueList, serviceMethod.RequestMap, serviceMethod.MappedToAction.RequestMap);

                currentNode = topNode.AddChild(string.Format("ResponseMap : ServiceMethod Map ({0}) <-> Action Map ({1})", serviceMethod.ResponseMap.Id, serviceMethod.MappedToAction.ResponseMap.Id), null);
                CheckActionToServicePropertyMap(currentNode.IssueList, serviceMethod.ResponseMap, serviceMethod.MappedToAction.ResponseMap);

                // Check action to the procedure or sql request and responsemap
                if (serviceMethod.MappedToAction.MappedToObject != null)
                {
                    if (serviceMethod.MappedToAction.MappedToObject.ObjectType == ActionMapTarget.StoredProcedure)
                    {
                        currentNode = topNode.AddChild(string.Format("RequestMap : Action Map ({0}) <-> StoredProcedure (no map)", serviceMethod.MappedToAction.RequestMap.Id), null);
                        CheckProcedureToActionPropertyMap(currentNode.IssueList, serviceMethod.MappedToAction.RequestMap, ((StoredProcedure)serviceMethod.MappedToAction.MappedToObject).Properties, true);

                        currentNode = topNode.AddChild(string.Format("ResponseMap : Action Map ({0}) <-> StoredProcedure (no map)", serviceMethod.MappedToAction.ResponseMap.Id), null);
                        CheckProcedureToActionPropertyMap(currentNode.IssueList, serviceMethod.MappedToAction.ResponseMap, ((StoredProcedure)serviceMethod.MappedToAction.MappedToObject).Properties, false);
                    }
                    else
                    {
                        currentNode = topNode.AddChild(string.Format("RequestMap : Action Map ({0}) <-> Query (no map)", serviceMethod.MappedToAction.RequestMap.Id), null);
                        CheckSqlToActionPropertyMap(currentNode.IssueList, serviceMethod.MappedToAction.RequestMap, ((Query)serviceMethod.MappedToAction.MappedToObject).Properties, true, application.Id);

                        currentNode = topNode.AddChild(string.Format("ResponseMap : Action Map ({0}) <-> Query (no map)", serviceMethod.MappedToAction.ResponseMap.Id), null);
                        CheckSqlToActionPropertyMap(currentNode.IssueList, serviceMethod.MappedToAction.ResponseMap, ((Query)serviceMethod.MappedToAction.MappedToObject).Properties, false, application.Id);
                    }
                }
            }
            else
            {
                topNode.IssueList.AddError("The ServiceMethod is not connected to an action!", serviceMethod);
            }

            parentNode.Children.Add(topNode);
        }

        private void CheckProcedureToActionPropertyMap(AnalyzeIssueList issueList, PropertyMap actionMap, IList<ProcedureProperty> storedProdPropList, bool isRequestMap)
        {
            foreach (ProcedureProperty procProp in storedProdPropList)
            {
                // Try to find the mappedproperty in the Action PropertyMap that has a source
                // that is the dbproperty property.
                var actionProperties = (from MappedProperty p in actionMap.MappedProperties
                                        where p.Source.Id == ((IMappableProperty)procProp).Id
                                        select p);

                // Check if the mapped property was found.
                if (actionProperties.Count() == 0)
                {
                    // Property not found. 
                    if (isRequestMap && (procProp.PropertyType == DbPropertyType.In || procProp.PropertyType == DbPropertyType.InOut))
                    {
                        // Check if it's a requestmap and if the property in that case is ingoing
                        // or in/outgoing. In that case it should have been in the map.
                        issueList.AddError(string.Format("Procedure (DBProperty - IMappableProperty) parameter - ({0}) \"{1}\" is missing in the Action map.", procProp.Id, procProp.Name), procProp);
                    }
                    else if (!isRequestMap && (procProp.PropertyType == DbPropertyType.Out || procProp.PropertyType == DbPropertyType.InOut))
                    {
                        // Check if it's a responsemap and if the property in that case is outgoing
                        // or in/outgoing. In that case it should have been in the map.
                        issueList.AddError(string.Format("Procedure (DBProperty - IMappableProperty) parameter - ({0}) \"{1}\" is missing in the Action map.", procProp.Id, procProp.Name), procProp);
                    }
                }
                else
                {
                    // First check if there is a table and column specified. In that case we don't
                    // have to check the datatypes.
                    if (string.IsNullOrEmpty(procProp.OriginalTable) || string.IsNullOrEmpty(procProp.OriginalColumn))
                    {
                        // Check Type's
                        try
                        {
                            Type procPropType = DataModelImporter.GetClrTypeFromDbType(procProp.DbDatatype, procProp.Length, procProp.Precision, procProp.Scale, null);

                            IssueSeverityType issueSeverity;

                            if (!CheckTypeConversion(procPropType, procProp.Length, actionProperties.Last().Type, out issueSeverity))
                            {
                                if (procPropType == typeof(string))
                                {
                                    issueList.Add(issueSeverity,
                                                  string.Format("The Types are different! The ProcedureProperty ({0}) \"{1}\" has the type \"{2}\" (length = {3}) and the Action property ({4}) \"{5}\" has the type \"{6}\".",
                                                                procProp.Id,
                                                                procProp.Name,
                                                                procPropType == null ? "null" : procPropType.ToString(),
                                                                procProp.Length.HasValue ? procProp.Length.Value.ToString() : "null",
                                                                actionProperties.Last().Id,
                                                                actionProperties.Last().Name,
                                                                actionProperties.Last().Type.ToString()),
                                                  null);
                                }
                                else
                                {
                                    issueList.Add(issueSeverity,
                                                  string.Format("The Types are different! The ProcedureProperty ({0}) \"{1}\" has the type \"{2}\" and the Action property ({3}) \"{4}\" has the type \"{5}\".",
                                                                procProp.Id,
                                                                procProp.Name,
                                                                procPropType == null ? "null" : procPropType.ToString(),
                                                                actionProperties.Last().Id,
                                                                actionProperties.Last().Name,
                                                                actionProperties.Last().Type.ToString()),
                                                  null);
                                }
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            if (ex.ParamName == "dbType")
                            {
                                IssueSeverityType issueSeverity;

                                if (!CheckTypeConversion(typeof(string), procProp.Length, actionProperties.Last().Type, out issueSeverity))
                                {
                                    issueList.Add(issueSeverity,
                                                  string.Format("The Types are different! The ProcedureProperty ({0}) \"{1}\" has the type \"{2}\" (length = {3}) and the Action property ({4}) \"{5}\" has the type \"{6}\".",
                                                                procProp.Id,
                                                                procProp.Name,
                                                                "<defaulted to string since type was empty>",
                                                                procProp.Length.HasValue ? procProp.Length.Value.ToString() : "null",
                                                                actionProperties.Last().Id,
                                                                actionProperties.Last().Name,
                                                                actionProperties.Last().Type.ToString()),
                                                  null);
                                }
                            }
                            else
                            {
                                issueList.AddError(string.Format("The ProcedureProperty ({0}) \"{1}\" has a type \"{2}\" with the following error: {3}", procProp.Id, procProp.Name, procProp.DbDatatype == null ? "null" : procProp.DbDatatype, ex.Message), procProp);
                            }
                        }
                    }
                }
            }

            // Try to find mappedproperties that exist in Action map but doesn't
            // have a corresponding mappedproperty in the DBProperties map.
            foreach (MappedProperty actionProp in actionMap.MappedProperties)
            {
                bool exists = false;

                foreach (IMappableProperty dbProp in storedProdPropList.Cast<IMappableProperty>())
                {
                    if (actionProp.Source == dbProp)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    // The mapped property for the servicemap is missing in the actionmap.
                    issueList.AddError(string.Format("Action (MappedProperty) parameter ({0}) \"{1}\" is missing as a procedure parameter and should be removed!", actionProp.Id, actionProp.Name), actionProp);
                }
            }

        }

        private bool CheckTypeConversion(Type fromType, int? fromLength, Type toType, out IssueSeverityType issueSeverityType)
        {
            bool result = true;
            issueSeverityType = IssueSeverityType.None;

            if (fromType != toType)
            {
                // If types are not same then default to not OK
                result = false;
                issueSeverityType = IssueSeverityType.Error;

                if (fromType == typeof(int))
                {
                    if (toType == typeof(long) ||
                        toType == typeof(decimal) ||
                        toType == typeof(double))
                    {
                        issueSeverityType = IssueSeverityType.None;
                        result = true;
                    }
                }
                else if (fromType == typeof(decimal))
                {
                    if (toType == typeof(int) ||
                        toType == typeof(long) ||
                        toType == typeof(double))
                    {
                        issueSeverityType = IssueSeverityType.Warning;
                    }
                }
                else if (fromType == typeof(double))
                {
                    if (toType == typeof(int) ||
                        toType == typeof(long))
                    {
                        issueSeverityType = IssueSeverityType.Warning;
                    }
                    else if (toType == typeof(decimal))
                    {
                        issueSeverityType = IssueSeverityType.None;
                        result = true;
                    }
                }
                else if (fromType == typeof(string))
                {
                    if (toType == typeof(bool))
                    {
                        if (fromLength.HasValue && fromLength.Value == 1)
                        {
                            issueSeverityType = IssueSeverityType.None;
                            result = true;
                        }
                        else
                            issueSeverityType = IssueSeverityType.Warning;
                    }
                    else if (toType == typeof(DateTime))
                    {
                        issueSeverityType = IssueSeverityType.Warning;
                    }
                }
            }

            return result;
        }

        private void CheckSqlToActionPropertyMap(AnalyzeIssueList issueList, PropertyMap actionMap, IList<QueryProperty> queryPropList, bool isRequestMap, Guid applicationId)
        {
            foreach (QueryProperty queryProp in queryPropList)
            {
                // Try to find the mappedproperty in the Action PropertyMap that has a source
                // that is the dbproperty property.
                var actionProperties = (from MappedProperty p in actionMap.MappedProperties
                                        where p.Source.Id == ((IMappableProperty)queryProp).Id
                                        select p);

                // Check if the mapped property was found. If not then there is an error.
                if (actionProperties.Count() == 0)
                {
                    // Property not found. 
                    if (isRequestMap && (queryProp.PropertyType == DbPropertyType.In || queryProp.PropertyType == DbPropertyType.InOut))
                    {
                        // Check if it's a requestmap and if the property in that case is ingoing
                        // or in/outgoing. In that case it should have been in the map.
                        issueList.AddError(string.Format("SQL Query (DBProperty - IMappableProperty) parameter - ({0}) \"{1}\" is missing in the Action map.", queryProp.Id, queryProp.Name), queryProp);
                    }
                    else if (!isRequestMap && (queryProp.PropertyType == DbPropertyType.Out || queryProp.PropertyType == DbPropertyType.InOut))
                    {
                        // Check if it's a responsemap and if the property in that case is outgoing
                        // or in/outgoing. In that case it should have been in the map.
                        issueList.AddError(string.Format("SQL Query (DBProperty - IMappableProperty) parameter - ({0}) \"{1}\" is missing in the Action map.", queryProp.Id, queryProp.Name), queryProp);
                    }

                }
                else
                {
                    // First check if there is a table and column specified. In that case we don't
                    // have to check the datatypes.
                    if (string.IsNullOrEmpty(queryProp.OriginalTable) || string.IsNullOrEmpty(queryProp.OriginalColumn))
                    {
                        // Check Type's
                        try
                        {
                            Type queryPropType = DataModelImporter.GetClrTypeFromDbType(queryProp.DbDatatype, queryProp.Length, queryProp.Precision, queryProp.Scale, null);

                            IssueSeverityType issueSeverity;

                            if (!CheckTypeConversion(queryPropType, queryProp.Length, actionProperties.Last().Type, out issueSeverity))
                            {
                                if (queryPropType == typeof(string))
                                {
                                    issueList.Add(issueSeverity,
                                                  string.Format("The Types are different! The QueryProperty ({0}) \"{1}\" has the type \"{2}\" (length = {3}) and the Action property ({4}) \"{5}\" has the type \"{6}\".",
                                                                queryProp.Id,
                                                                queryProp.Name,
                                                                queryPropType == null ? "null" : queryPropType.ToString(),
                                                                queryProp.Length.HasValue ? queryProp.Length.Value.ToString() : "null",
                                                                actionProperties.Last().Id,
                                                                actionProperties.Last().Name,
                                                                actionProperties.Last().Type.ToString()),
                                                  null);
                                }
                                else
                                {
                                    issueList.Add(issueSeverity,
                                                  string.Format("The Types are different! The QueryProperty ({0}) \"{1}\" has the type \"{2}\" and the Action property ({3}) \"{4}\" has the type \"{5}\".",
                                                                queryProp.Id,
                                                                queryProp.Name,
                                                                queryPropType == null ? "null" : queryPropType.ToString(),
                                                                actionProperties.Last().Id,
                                                                actionProperties.Last().Name,
                                                                actionProperties.Last().Type.ToString()),
                                                  null);
                                }
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            if (ex.ParamName == "dbDataType")
                            {
                                IssueSeverityType issueSeverity;

                                if (!CheckTypeConversion(typeof(string), queryProp.Length, actionProperties.Last().Type, out issueSeverity))
                                {
                                    issueList.Add(issueSeverity,
                                                  string.Format("The Types are different! The QueryProperty ({0}) \"{1}\" has the type \"{2}\" (length = {3}) and the Action property ({4}) \"{5}\" has the type \"{6}\".",
                                                                queryProp.Id,
                                                                queryProp.Name,
                                                                "<defaulted to string since type was empty>",
                                                                queryProp.Length.HasValue ? queryProp.Length.Value.ToString() : "null",
                                                                actionProperties.Last().Id,
                                                                actionProperties.Last().Name,
                                                                actionProperties.Last().Type.ToString()),
                                                  null);
                                }
                            }
                            else
                            {
                                issueList.AddError(string.Format("The QueryProperty ({0}) \"{1}\" has a type \"{2}\" with the following error: {3}", queryProp.Id, queryProp.Name, queryProp.DbDatatype == null ? "null" : queryProp.DbDatatype, ex.Message), queryProp);
                            }
                        }
                    }
                    else
                    {
                        // Check that the property that the action is connected to is the same property
                        // that corresponds to the SQL's column and table.
                        MappedProperty actionMappedProperty = actionProperties.Last();
                        Property actionProperty = actionMappedProperty.TargetProperty;

                        if (actionProperty != null)
                        {
                            // Check if the queryproperty table and column is existing as a real property.
                            Property foundProp = PropertyDao.FindAllByTableAndColumn(queryProp.OriginalTable, queryProp.OriginalColumn, applicationId).FirstOrDefault();

                            if (actionProperty.StorageInfo != null)
                            {
                                if (foundProp != null)
                                {
                                    // Check if it's the same property
                                    if (foundProp.Id != actionProperty.Id)
                                    {
                                        issueList.AddError(string.Format("The QueryProperty ({0}) \"{1}\" points to table and column {2}.{3} but the Actions MappedProperty is connected " +
                                                                         "to the property ({4}) \"{5}\" that corresponds to table and column {6}.{7}.",
                                                                         queryProp.Id,
                                                                         queryProp.Name,
                                                                         queryProp.OriginalTable,
                                                                         queryProp.OriginalColumn,
                                                                         actionProperty.Id,
                                                                         actionProperty.Name,
                                                                         actionProperty.StorageInfo.TableName,
                                                                         actionProperty.StorageInfo.ColumnName), queryProp);
                                    }
                                }
                                else
                                {
                                    // Not found as a real table/column. Could be a view.
                                    // Add as a warning so that the user needs to check that the property is
                                    // correctly selected.
                                    // Check if tablename ends with _VW and if the columnname is the same for the view as the real
                                    // table then it should be ok.
                                    if (!queryProp.OriginalTable.EndsWith("_VW") || queryProp.OriginalColumn != actionProperty.StorageInfo.ColumnName)
                                    {
                                        issueList.AddWarning(string.Format("The Actions MappedProperty ({0}) \"{1}\" is connected to a Property corresponding to table and column {2}.{3}.\r\n" +
                                                                           "The Query Property ({4}) \"{5}\" that the Actions Mapped property points to is not a table stored in the Metadata ({6}.{7}).\r\n" +
                                                                           "This could be a view or it could be a table not yet imported into the MetaManager.\r\n" +
                                                                           "Please check that this is OK to use this property for this field.",
                                                                           actionMappedProperty.Id,
                                                                           actionMappedProperty.Name,
                                                                           actionProperty.StorageInfo.TableName,
                                                                           actionProperty.StorageInfo.ColumnName,
                                                                           queryProp.Id,
                                                                           queryProp.Name,
                                                                           queryProp.OriginalTable,
                                                                           queryProp.OriginalColumn), queryProp);
                                    }
                                }
                            }
                            else
                            {
                                // No storage info found for the action which means that the property is
                                // a custom property.
                                if (foundProp != null)
                                {
                                    // A real property is found. Why has a custom property been chosen if it's
                                    // possible to map to a real property instead?
                                    issueList.AddError(string.Format("The QueryProperty ({0}) \"{1}\" points to table and column {2}.{3} but the Actions MappedProperty is connected " +
                                                                     "to a Custom property ({4}) \"{5}\"!",
                                                                     queryProp.Id,
                                                                     queryProp.Name,
                                                                     queryProp.OriginalTable,
                                                                     queryProp.OriginalColumn,
                                                                     actionProperty.Id,
                                                                     actionProperty.Name), queryProp);
                                }
                            }
                        }
                        else
                        {
                            issueList.AddError(string.Format("The Actions MappedProperty ({0}) \"{1}\" is not connected to a property!", actionMappedProperty.Id, actionMappedProperty.Name), actionMappedProperty);
                        }
                    }
                }
            }

            // Try to find mappedproperties that exist in Action map but doesn't
            // have a corresponding mappedproperty in the DBProperties map.
            foreach (MappedProperty actionProp in actionMap.MappedProperties)
            {
                bool exists = false;

                foreach (IMappableProperty dbProp in queryPropList.Cast<IMappableProperty>())
                {
                    if (actionProp.Source == dbProp)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    // The mapped property for the servicemap is missing in the actionmap.
                    issueList.AddError(string.Format("Action (MappedProperty) parameter ({0}) \"{1}\" is missing as a query parameter and should be removed!", actionProp.Id, actionProp.Name), actionProp);
                }
            }

        }

        private void CheckActionToServicePropertyMap(AnalyzeIssueList issueList, PropertyMap servicePropMap, PropertyMap actionPropMap)
        {
            foreach (MappedProperty actionProp in actionPropMap.MappedProperties)
            {
                // Try to find the mappedproperty in the Service PropertyMap that has a source
                // that is the action property.
                var serviceProperties = (from MappedProperty p in servicePropMap.MappedProperties
                                         where p.Source.Id == actionProp.Id
                                         select p);

                // Check if the mapped property was found. If not then there is an error.
                // An action mappedproperty has no corresponding mappedproperty in the servicemethod.
                if (serviceProperties.Count() == 0)
                {
                    issueList.AddError(string.Format("Action (MappedProperty) parameter - ({0}) \"{1}\" is missing in the ServiceMethod map.", actionProp.Id, actionProp.Name), actionProp);
                }
                else
                {
                    IssueSeverityType issueSeverity;

                    // Check Type's
                    if (!CheckTypeConversion(actionProp.Type, null, serviceProperties.Last().Type, out issueSeverity))
                    {
                        issueList.Add(issueSeverity,
                                      string.Format("The Types are different! The Action MappedProperty ({0}) \"{1}\" has the type \"{2}\" and the ServiceMethods MappedProperty ({3}) \"{4}\" has the type \"{5}\".", actionProp.Id, actionProp.Name, actionProp.Type, serviceProperties.Last().Id, serviceProperties.Last().Name, serviceProperties.Last().Type.ToString()),
                                      null);
                    }
                }
            }

            // Try to find mappedproperties that exist in ServiceMethod map but doesn't
            // have a corresponding mappedproperty in the action map.
            foreach (MappedProperty serviceProp in servicePropMap.MappedProperties)
            {
                bool existsInAction = false;

                foreach (MappedProperty actionProperty in actionPropMap.MappedProperties)
                {
                    if (serviceProp.Source == actionProperty)
                    {
                        existsInAction = true;
                        break;
                    }
                }

                if (!existsInAction)
                {
                    // The mapped property for the servicemap is missing in the actionmap.
                    issueList.AddError(string.Format("ServiceMethod (MappedProperty) parameter ({0}) \"{1}\" is missing in the Action map and should be removed!", serviceProp.Id, serviceProp.Name), serviceProp);
                }
            }
        }

    }
}
