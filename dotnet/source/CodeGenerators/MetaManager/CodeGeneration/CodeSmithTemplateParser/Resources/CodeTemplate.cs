using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using Cdc.CodeGeneration.Caching;

namespace CodeSmith.Engine
{
    public class CodeTemplate
    {
        protected StringBuilder __text = new StringBuilder();
        protected string currentFilename = null;

        public TextWriter Response
        {
            get
            {
                return new StringWriter(__text);
            }
        }

        public CodeTemplate() { }

        public T Create<T>() where T : CodeTemplate, new()
        {
            return new T();
        }

        public void SetProperty(string name, object value)
        {
            PropertyInfo property = this.GetType().GetProperty(name);

            // Check if property was found
            if (property != null)
            {
                property.SetValue(this, value, null);
            }
        }

        public void RenderToFile(string filename, bool overwrite)
        { 
            RenderToFile(filename, overwrite, Encoding.UTF8);
        }

        public void RenderToFile(string filename, bool overwrite, Encoding encoding)
        {
            FileMode fm;

            try
            {
                if (!overwrite && File.Exists(filename))
                {
                    fm = FileMode.Append;
                }
                else
                {
                    fm = FileMode.Create;

                    if (!Directory.Exists(Path.GetDirectoryName(filename)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(filename));
                    }
                }

                currentFilename = filename;
                FileCacheManager.WriteFile(filename, RenderToString(), fm, encoding);
            }
            catch (System.IO.PathTooLongException ex)
            {
                string errorText = string.Format(" The path and/or the filename is too long!\n\n" +
                                                 "Error occured in TemplateFile:\t\t{0}\n\n" +
                                                 "Error when trying to create the directory and/or file:\n" +
                                                 "Filename ({2} chars):\t\t{1}\n" +
                                                 "Path ({4} chars):\t{3}\n" +
                                                 "Total Length of Filename + Path:\t{5} chars\n\n" +
                                                 "Original Exception:\n{6}"
                                                 , this.GetType().Name
                                                 , Path.GetFileName(filename)
                                                 , Path.GetFileName(filename).Length.ToString()
                                                 , filename.Substring(0, filename.LastIndexOf("\\") + 1)
                                                 , filename.Substring(0, filename.LastIndexOf("\\") + 1).Length.ToString()
                                                 , (Path.GetFileName(filename).Length + filename.Substring(0, filename.LastIndexOf("\\") + 1).Length).ToString()
                                                 , ex.ToString());

                throw new System.IO.PathTooLongException(errorText);
            }
        }

        public void Render(TextWriter writer)
        {
            writer.Write(RenderToString());
        }

        protected void TagFile(string tag)
        {
            if (!string.IsNullOrEmpty(currentFilename))
            {
                // Tag C# file
                if (currentFilename.ToLower().EndsWith(".cs"))
                {
                    __text.AppendLine(string.Format(@"// {0}", tag));
                }
            }
        }

        public string RenderToString()
        {
            // Should be overrided in each class
            Render();
            return FinalRender();
        }

        public virtual string OriginalTemplateName
        {
            get
            {
                return string.Empty;
            }
        }

        public virtual void Render()
        {
            return;
        }

        protected string FinalRender()
        {
            try
            {
                return __text.ToString();
            }
            finally
            {
                __text = new StringBuilder();
            }
        }

    }
}

