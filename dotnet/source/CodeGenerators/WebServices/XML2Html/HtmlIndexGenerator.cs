using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using Imi.CodeGenerators.WebServices.Framework;

namespace Imi.CodeGenerators.WebServices.XML2Html
{

  /// <summary>
  /// Summary description for HtmlIndexGenerator.
  /// </summary>
  public class HtmlIndexGenerator : GenericGenerator
  {
    public HtmlIndexGenerator( MessageDefinition messageDef, Package packageDef ) :
      base( messageDef, packageDef ) {}

    public void AddStructure(ChildType ct, StringBuilder s)
    {
      StructureType structure = null;

      foreach(StructureType st in messageDef.structures) 
      {
        if(st.name != ct.name)
          continue;

        structure = st;
        break;
      }

      if(structure != null) 
      {
        s.Append("<ul>\r\n");
        s.Append("<li type=\"disc\"><a href=\"");
        s.Append(structure.name);
        s.Append(".html\">");
        s.Append(structure.name);
        s.Append("</a> (");
        s.Append(ct.minOccurrs);
        s.Append("..");
        s.Append(ct.maxOccurrs);
        s.Append(")\r\n");

        if(structure.children != null)
        {
          foreach(ChildType c in structure.children) 
            AddStructure(c,s);
        }

        s.Append("</li></ul>\r\n");
      }
    }

    public override void GetOutPut(TextWriter tw) 
    {
      StringBuilder s = new StringBuilder();

      s.Append("<html>\r\n");
      s.Append("<head>\r\n");
      s.Append("<META http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\r\n");
      s.Append("<title>Interface Index");
      s.Append("</title>\r\n");
      s.Append("<link href=\"StructureDescription.css\" rel=\"stylesheet\" type=\"text/css\">\r\n");
      s.Append("</head>\r\n");
      s.Append("<body>\r\n");
      s.Append("<h1>Interface Index</h1>\r\n");
      s.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\" bordercolor=\"#000000\"><tbody>\r\n");
      s.Append("<tr class=\"tableHeader\">\r\n");
      s.Append("<th class=\"tableHeaderColumn\">Document</th>\r\n");
      s.Append("<th class=\"tableHeaderColumn\">Structure</th>\r\n");
      s.Append("</tr>\r\n");

      // Once for each Interface

      foreach(InterfaceType it in messageDef.interfaces) 
      {
        s.Append("<tr class=\"tableRow\">\r\n");
        s.Append("<td class=\"tableRowColumn\">\r\n");
        s.Append(it.name);
        s.Append("</td>\r\n");
        s.Append("<td class=\"tableRowColumn\">\r\n");

        ChildType ct = new ChildType();
        ct.name = it.structure;
        ct.minOccurrs = "1";
        ct.maxOccurrs = "1";
        AddStructure(ct,s);
        s.Append("</td>\r\n");
      }
      
      s.Append("</table>\r\n");
      s.Append("</body>\r\n</html>\r\n");

      tw.Write(s.ToString());
    }

    public void Save(String outDirectory) 
    {
      TextWriter writer = null;
      String outFileName = "";
      StringCollection s = new StringCollection();

      // Create interface list document
      try 
      {
        outFileName = outDirectory + "\\index.html";
        writer = new StreamWriter(outFileName);
        GetOutPut(writer);
        writer.Flush();
        writer.Close();
      }
      catch(Exception e) 
      {
        Console.WriteLine("Failed to open output file '" + outFileName + "'" + Environment.NewLine + 
          e.Message + Environment.NewLine + 
          e.StackTrace);
        throw(e);
      }
      finally 
      {
        if (writer != null)
          writer.Close();
      }

    }


  }
}

