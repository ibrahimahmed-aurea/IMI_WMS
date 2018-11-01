using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Cdc.Framework.Parsing.CSharpParsing
{
    public enum ProjectInfoType { Wcf, Wpf, Default, Workflow };

    public class ProjectInfo
    {
        public ProjectInfo() 
        {
            ProjectType = ProjectInfoType.Wcf;
        }

        public string Namespace { get; set; }
        public Guid ProjectGuid { get; set; }
        public DirectoryInfo ProjectDirectory { get; set; }
        private string projectFile;
        public string ProjectFile
        {
            get
            {
                return projectFile;
            }
            set
            {
                FileInfo f = new FileInfo(value);
                projectFile = f.FullName;
                ProjectDirectory = new DirectoryInfo(f.DirectoryName);
            }
        }
        public List<string> LocalNamespaces { get; set; }
        public List<string> UsedNamespaces { get; set; }
        public ProjectInfoType ProjectType { get; set; }
    }
}
