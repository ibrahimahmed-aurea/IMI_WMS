using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Cdc.MetaManager.CodeGeneration.CodeSmithTemplateParser;
using System.CodeDom.Compiler;

namespace CodeSmithTemplatesConvertCompile
{
    public partial class Result : Form
    {
        private const int IndentPixelsPerLevel = 20;

        public ConfigurationSettings Config { get; set; }
        public bool ParseCompileResult { get; set; }

        public string TemplateRootDirectory { get; set; }
        public string SourceDestinationDirectory { get; set; }
        public bool AddDebugInformation { get; set; }
        public string AssemblyFilename { get; set; }
        public List<string> ReferenceList { get; set; }
        public List<string> NotCompileList { get; set; }
        public string SourceNamespace { get; set; }

        public Result()
        {
            InitializeComponent();

            Config = new ConfigurationSettings();
        }

        private void WriteResultLine()
        {
            WriteResultLine(string.Empty);
        }

        private void WriteResultLine(string result)
        {
            WriteResultLine(result, FontStyle.Regular, Color.Black, 1);
        }

        private void WriteResultLine(string result, Color textColor)
        {
            WriteResultLine(result, FontStyle.Regular, textColor, 1);
        }

        private void WriteResultLine(string result, int indentLevel)
        {
            WriteResultLine(result, FontStyle.Regular, Color.Black, indentLevel);
        }

        private void WriteResultLine(string result, FontStyle fontStyle)
        {
            WriteResultLine(result, fontStyle, Color.Black, 0);
        }

        private void WriteResultLine(string result, FontStyle fontStyle, Color textColor)
        {
            WriteResultLine(result, fontStyle, textColor, 0);
        }

        private void WriteResultLine(string result, FontStyle fontStyle, Color textColor, int indentLevel)
        {
            rtbResult.SelectionColor = textColor;
            rtbResult.SelectionFont = new Font(rtbResult.Font, fontStyle);
            rtbResult.SelectionIndent = indentLevel * IndentPixelsPerLevel;

            rtbResult.SelectedText = result + Environment.NewLine;
        }

        private void WriteErrorLine(string errorText)
        {
            rtbResult.SelectionFont = new Font(rtbResult.Font, FontStyle.Bold);
            rtbResult.SelectionColor = Color.Red;
            rtbResult.SelectionIndent = 20;

            rtbResult.SelectedText = errorText + Environment.NewLine;
        }

        private string GetAbsolutePathFromRootDir(string relativePath)
        {
            return GetAbsolutePathFromRootDir(Path.GetDirectoryName(Application.ExecutablePath), relativePath);
        }

        private string GetAbsolutePathFromRootDir(string rootDir, string relativePath)
        {
            return Path.GetFullPath(Path.Combine(rootDir, relativePath));
        }

        private bool DoFixPath(ref string path, bool checkIfDirExists)
        {
            if (!string.IsNullOrEmpty(path))
            {
                if (!Path.IsPathRooted(path))
                {
                    path = GetAbsolutePathFromRootDir(path);
                }

                if (checkIfDirExists)
                {
                    return Directory.Exists(path);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        private bool DoFixFiles(List<string> filenameList, bool checkIfFileExists)
        {
            return DoFixFiles(filenameList, checkIfFileExists, string.Empty);
        }

        private bool DoFixFiles(List<string> filenameList, bool checkIfFileExists, string rootDirectory)
        {
            if (filenameList != null && filenameList.Count > 0)
            {
                for (int i = 0; i < filenameList.Count; i++)
                {
                    string tempStr = filenameList[i];

                    if (!DoFixFile(ref tempStr, checkIfFileExists, rootDirectory))
                    {
                        return false;
                    }

                    filenameList[i] = tempStr;
                }
            }
            return true;
        }

        private bool DoFixFile(ref string filename, bool checkIfFileExists)
        {
            return DoFixFile(ref filename, checkIfFileExists, string.Empty);
        }

        private bool DoFixFile(ref string filename, bool checkIfFileExists, string rootDirectory)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                if (!Path.IsPathRooted(filename))
                {
                    if (string.IsNullOrEmpty(rootDirectory))
                    {
                        filename = GetAbsolutePathFromRootDir(filename);
                    }
                    else
                    {
                        filename = GetAbsolutePathFromRootDir(rootDirectory, filename);
                    }
                }

                if (checkIfFileExists)
                {
                    return File.Exists(filename);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        private void Result_Load(object sender, EventArgs e)
        {
            rtbResult.Clear();
            string tempStr = string.Empty;
            List<string> tempStrList = null;

            // Read Configuration
            WriteResultLine("---[ Stage 1: Reading Configuration ]---", FontStyle.Bold);
            WriteResultLine();

            // Template Root Directory
            tempStr = Config.TemplateRootDirectory;

            if (!DoFixPath(ref tempStr, true))
            {
                if (string.IsNullOrEmpty(tempStr))
                {
                    WriteErrorLine("The Template Root Directory is not set!");
                }
                else
                {
                    WriteErrorLine(string.Format("The Template Root Directory \"{0}\" does not exist!", tempStr));
                }
                return;
            }
            else
            {
                TemplateRootDirectory = tempStr;
                WriteResultLine(string.Format("Template Root Directory = \"{0}\"", tempStr));
            }

            // Source Destination Directory
            tempStr = Config.SourceDestinationDirectory;

            if (!DoFixPath(ref tempStr, false))
            {
                if (string.IsNullOrEmpty(tempStr))
                {
                    WriteErrorLine("The Source Destination Directory is not set!");
                }
                return;
            }
            else
            {
                SourceDestinationDirectory = tempStr;
                WriteResultLine(string.Format("Source Destination Directory = \"{0}\"", tempStr));

                // Check if directory exist. Then remove everything and recreate it.
                try
                {
                    int retryCount = 0;

                    while (retryCount < 3)
                    {
                        try
                        {
                            if (Directory.Exists(tempStr))
                            {
                                Directory.Delete(tempStr, true);
                            }

                            break;
                        }
                        catch (System.IO.IOException ex)
                        {
                            if (ex.Message.StartsWith("The directory is not empty."))
                            {
                                string[] files = Directory.GetFiles(tempStr, "*.*", SearchOption.AllDirectories);
                                foreach (string fileName in files) File.Delete(fileName);
                                retryCount++;
                            }
                            else
                                throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteErrorLine(string.Format("Error when trying to clear the Source Destination Directory. Errormessage: {0}", ex.Message));
                    return;
                }

                try
                {
                    Directory.CreateDirectory(tempStr);
                }
                catch (Exception ex)
                {
                    WriteErrorLine(string.Format("Error when trying to create the Source Destination Directory after clearing it. Errormessage: {0}", ex.Message));
                    return;
                }
            }

            // Debug information
            AddDebugInformation = Config.DebugInfo;
            WriteResultLine(string.Format("Add Debug Information = \"{0}\"", AddDebugInformation.ToString()));

            // Source Namespace
            SourceNamespace = Config.SourceNamespace;

            if (string.IsNullOrEmpty(SourceNamespace))
            {
                WriteErrorLine("The Namespace is not set!");
                return;
            }

            // Assembly Filename
            tempStr = Config.AssemblyFilename;

            if (!DoFixFile(ref tempStr, false))
            {
                WriteErrorLine("The Assembly Filename is not set!");
                return;
            }
            else
            {
                AssemblyFilename = tempStr;
                WriteResultLine(string.Format("Assembly Filename = \"{0}\"", tempStr));
            }

            // Check if assemblyfile already exists, and if it does check if it's
            // readonly. If it is then check config to see what we should do.
            if (File.Exists(tempStr) && Config.AskWhenAssemblyIsReadOnly)
            {
                FileAttributes fileAttrib = File.GetAttributes(tempStr);

                // Check if file is readonly
                if ((fileAttrib & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    // File is readonly.
                    if (MessageBox.Show(string.Format("Assembly File \"{0}\" is readonly.\n\nSet it to writable and overwrite it?", tempStr), "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        File.SetAttributes(tempStr, fileAttrib ^ FileAttributes.ReadOnly);

                        MessageBox.Show("REMEMBER to CHECKOUT and CHECKIN the file when you're done changing the template file(s)!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }

            // Reference List
            tempStrList = Config.ReferenceList;

            if (!DoFixFiles(tempStrList, false))
            {
                WriteErrorLine("One or more filenames in the Reference List is empty!");
                return;
            }
            else
            {
                ReferenceList = tempStrList;

                int i = 0;
                foreach (string referenceName in ReferenceList)
                {
                    WriteResultLine(string.Format("Reference ({0}) = \"{1}\"", i++.ToString("0000"), referenceName));
                }
            }

            // Not Compile List
            tempStrList = Config.NotCompileList;

            if (!DoFixFiles(tempStrList, false, SourceDestinationDirectory))
            {
                WriteErrorLine("One or more files/directories in the Not Compile List is empty!");
                return;
            }
            else
            {
                NotCompileList = tempStrList;

                int i = 0;
                foreach (string notCompile in NotCompileList)
                {
                    WriteResultLine(string.Format("Not to Compile ({0}) = \"{1}\"", i++.ToString("0000"), notCompile));
                }
            }

            // Parse Templates
            WriteResultLine();
            WriteResultLine("---[ Stage 2: Parsing Templates ]---", FontStyle.Bold);
            WriteResultLine();

            IList<string> parsedTemplates = FileTemplateParser.ParseTemplates(TemplateRootDirectory, SourceDestinationDirectory, true, SourceNamespace, "CoreTemplate");

            WriteResultLine(string.Format("Templatefiles Parsed ({0}):", parsedTemplates.Count.ToString()), Color.Blue);

            foreach (string template in parsedTemplates)
            {
                WriteResultLine(template, 2);
            }

            // Create assembly
            WriteResultLine();
            WriteResultLine("---[ Stage 3: Creating Assembly ]---", FontStyle.Bold);
            WriteResultLine();

            WriteResultLine(string.Format("References (*.dll) to use when building Assembly ({0}):", ReferenceList.Count.ToString()), Color.Blue);

            foreach (string reference in ReferenceList)
            {
                WriteResultLine(reference, 2);
            }

            WriteResultLine();

            IList<string> sourceFileList = FileTemplateParser.GetSourceFiles(SourceDestinationDirectory, NotCompileList);

            WriteResultLine(string.Format("Sourcefiles (*.cs) to Build Assembly from ({0}):", sourceFileList.Count.ToString()), Color.Blue);

            foreach (string sourceFile in sourceFileList)
            {
                WriteResultLine(sourceFile, 2);
            }

            WriteResultLine();

            CompilerErrorCollection compilerErrors;

            string createdAssembly = FileTemplateParser.CreateAssembly(sourceFileList, ReferenceList, AssemblyFilename, AddDebugInformation, out compilerErrors);

            if (compilerErrors.Count == 0)
            {
                WriteResultLine("Assemblyfile created with success! No errors!", FontStyle.Bold, Color.Green);
                WriteResultLine(string.Format("Assembly Filename: \"{0}\"", createdAssembly), FontStyle.Bold, Color.Green);
                ParseCompileResult = true;
            }
            else
            {
                WriteResultLine("Compilation errors (showing top 10 only):", FontStyle.Bold, Color.Blue, 1);

                for (int i = 0; i < compilerErrors.Count && i < 10; i++)
                {
                    WriteResultLine(string.Format("({0}) {1} (File: \"{2}\", Line:{3}, Col:{4})",
                        compilerErrors[i].ErrorNumber,
                        compilerErrors[i].ErrorText,
                        compilerErrors[i].FileName,
                        compilerErrors[i].Line.ToString(),
                        compilerErrors[i].Column.ToString()), 
                        FontStyle.Bold, 
                        compilerErrors[i].IsWarning ? Color.Orange : Color.Red, 
                        2); 
                }
                WriteResultLine();

                WriteResultLine(string.Format("Assemblyfile NOT created! Number of Error & Warnings: {0}", compilerErrors.Count), FontStyle.Bold, Color.Red);
                ParseCompileResult = false;
            }

            //if (ParseCompileResult)
            //{
            //    Close();
            //}
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Result_Shown(object sender, EventArgs e)
        {
            //if (ParseCompileResult)
            //{
            //    btnClose.Text = "Auto Closing...";
            //    Application.DoEvents();
            //    System.Threading.Thread.Sleep(3000);
            //    Close();
            //}
        }
    }
}
