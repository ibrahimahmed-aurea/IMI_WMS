using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace BuildDeploymentKit
{
    class Program
    {
        private enum RevisionType
        {
            DEFAULT,
            DATE,
            NUMBER_IN_FILE,
        }

        static void Main(string[] args)
        {
            RevisionType revisionType = RevisionType.DEFAULT;

            if (File.Exists("revisionType.arg"))
            {
                string revTypeArgTxt = File.ReadAllText("revisionType.arg");

                if (!string.IsNullOrEmpty(revTypeArgTxt))
                {
                    switch (revTypeArgTxt)
                    {
                        case "DATE":
                            revisionType = RevisionType.DATE;
                            break;
                        case "NUMINFILE":
                            revisionType = RevisionType.NUMBER_IN_FILE;
                            break;
                    }
                }
            }
            else
            {
                File.WriteAllText("revisionType.arg", "DEFAULT");
            }


            string revision = "0";
            string lastRevision = "0";

            if (File.Exists("setversion.bat"))
            {
                string filetext = File.ReadAllText("setversion.bat");

                lastRevision = filetext.Substring(filetext.LastIndexOf(".")+1);

                
            }

            switch (revisionType)
            {
                case RevisionType.DEFAULT:
                    revision = (Convert.ToInt32(lastRevision) + 1).ToString();
                    break;

                case RevisionType.DATE:
                    string revisiondate = DateTime.Now.ToString("MMdd");
                    string build = "1";
                    revision = build + revisiondate;
                    if (lastRevision.Length > 4)
                    {
                        string lastRevisionDate = lastRevision.Substring(1, 4);

                        string lastBuild = lastRevision.Substring(0, 1);

                        if (lastRevisionDate == revisiondate)
                        {
                            int newbuild = (Convert.ToInt32(lastBuild) + 1);

                            if (newbuild > 9)
                            {
                                newbuild = 1;
                            }

                            revision = newbuild.ToString() + revisiondate;
                        }
                    }
                    break;
                    
                case RevisionType.NUMBER_IN_FILE:
                    revision = lastRevision;
                    break;
            }
            
            Assembly assembly = Assembly.GetExecutingAssembly();
            string version = assembly.GetName().Version.ToString();
            version = version.Remove(version.LastIndexOf("."));
            string newFileText = string.Format("set VersionNumber={0}.{1}", version, revision);
            File.WriteAllText("setversion.bat", newFileText);
        }
    }
}
