using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.BusinessLogic;

namespace Cdc.MetaManager.GUI
{
    public partial class CompileUXActionRuleForm : Form
    {
        public CompileUXActionRuleForm()
        {
            InitializeComponent();
            testBtn.Enabled = false;
        }

        private Assembly ass;
        private object parameters;
        
        public UXAction UXAction { get; set; }

        private string GetParameterClassName()
        { 
            return string.Format("{0}ActionParameters", UXAction.Name);
        }

        private string GenerateParameterClass()
        {
            string code = string.Format("public class {0}\n{{", GetParameterClassName());

            foreach (MappedProperty property in UXAction.RequestMap.MappedProperties)
            {
                string dataType = property.Type.FullName;

                if (property.Type != typeof(string))
                    dataType += "?";

                code += string.Format("\n\tpublic {0} {1} {{ get; set; }}", dataType, property.Name);
            }

            code += "\n}\n";

            return code;
        }

        private string GenerateStub()
        {
            string code = string.Format("\tpublic bool CanExecute({0} actionParameters)\n\t{{", GetParameterClassName());
            
            if (string.IsNullOrEmpty(UXAction.ConditionRule))
            {
                code += "\n\t\t//Write action condition here\n\t";
            }
            else
            {
                code += UXAction.ConditionRule;
            }

            code += "}\n";

            return code;
        }

        private string GenerateTestCode()
        {
            string code = string.Format("{0}\npublic class Test\n{{\n{1}}}", GenerateParameterClass(), GenerateStub());

            return code;
        }

        private void CompileBtn_Click(object sender, EventArgs e)
        {
            try
            {
                testBtn.Enabled = false;
                propertyGrid1.SelectedObject = null;
                textBox2.Text = string.Empty;
                ass = Compiler.CompileSource(codeEditor.Text);
                textBox2.Text = "Compile successful.";
                InitializeParameterGrid(ass);
                testBtn.Enabled = true;
            }
            catch (Exception ex)
            {
                textBox2.Text = ex.Message;
            }
        }

        private void InitializeParameterGrid(Assembly ass)
        {

            object myClass = ass.CreateInstance("Test");

            if (myClass != null)
            {
                MethodInfo mif = myClass.GetType().GetMethod("CanExecute");

                if (mif == null)
                    throw new Exception("You must provide a method with the following signature: \"public bool CanExecute(object actionParameters)\"");

                ParameterInfo[] pif = mif.GetParameters();

                if ((pif.Count() != 1)
                    || (pif[0].Name != "actionParameters")
                    || (pif[0].ParameterType.Name != GetParameterClassName()))
                {
                    throw new Exception(string.Format("You must provide a method with the following signature: \"public bool CanExecute({0} actionParameters)\"", GetParameterClassName()));
                }

                parameters = Activator.CreateInstance(pif[0].ParameterType);
                propertyGrid1.SelectedObject = parameters;
            }
            else
                throw new Exception("Test class not found.");
        }

        public void Run(Assembly ass)
        {
            try
            {
                object myClass = ass.CreateInstance("Test");
                if (myClass != null)
                {
                    object[] para = new object[] { parameters };
                    object o = myClass.GetType().InvokeMember("CanExecute", BindingFlags.InvokeMethod, null, myClass, para);
                    textBox2.Text = "Method returns " + o;
                }
                else
                    throw new Exception("Class Test not found");
            }
            catch (System.MissingMethodException mme)
            {
                throw new Exception("Method Validate missing", mme);
            }

        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            try
            {
                Run(ass);
                okBtn.Enabled = true;
            }
            catch
            {
                okBtn.Enabled = false;
                throw;
            }
        }

        private void codeEditor_Load(object sender, EventArgs e)
        {
            codeEditor.SetHighlighting("C#");
            codeEditor.TextEditorProperties.ConvertTabsToSpaces = true;
            codeEditor.TextEditorProperties.IndentationSize = 4;
            codeEditor.TextEditorProperties.EnableFolding = true;
            codeEditor.EnableFolding = true;
                        
            codeEditor.Document.DocumentChanged += new ICSharpCode.TextEditor.Document.DocumentEventHandler(Document_DocumentChanged);
        }

        private string GetConditionRule()
        {
            string rule = "";

            int pos = codeEditor.Text.IndexOf("CanExecute");

            if (pos > 0)
            {
                int pos1 = codeEditor.Text.IndexOf("{", pos);
                int pos2 = codeEditor.Text.IndexOf("}", pos1);

                if (((pos2 - pos1) - 1) > 0)
                    rule = codeEditor.Text.Substring(pos1 + 1, (pos2 - pos1) -1);
            }

            return rule;
        }

        void Document_DocumentChanged(object sender, ICSharpCode.TextEditor.Document.DocumentEventArgs e)
        {
            testBtn.Enabled = false;
            okBtn.Enabled = false;
        }

        private void CompileUXActionRuleForm_Load(object sender, EventArgs e)
        {
            codeEditor.Text = GenerateTestCode();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            UXAction.ConditionRule = GetConditionRule();
                        
        }
    }
}
