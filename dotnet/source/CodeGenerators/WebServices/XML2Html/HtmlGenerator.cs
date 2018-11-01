using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using Imi.CodeGenerators.WebServices.Framework;

namespace Imi.CodeGenerators.WebServices.XML2Html
{

  /// <summary>
  /// Summary description for HtmlGenerator.
  /// </summary>
  public class HtmlGenerator : GenericGenerator
  {

    public HtmlGenerator( MessageDefinition messageDef, Package packageDef ) :
      base( messageDef, packageDef ) {}

    private String GetMethods(ParameterType p)
    {
      String methods = "";

      if (p.use.insert)
        methods = "N";

      if (p.use.update){
        if (methods != "") 
        {
          methods += ", ";
        }
        methods += "C";
      }

      if (p.use.delete) 
      {
        if (methods != "") 
        {
          methods += ", ";
        }
        methods += "R";
      }

      return(methods);
    }

    private String GetCommentString(String[] comment)
    {
      StringBuilder s = new StringBuilder();
      bool first = true;

      foreach(String l in comment) 
      {
        String a = l.Replace( "<", "&lt;" );
        a = a.Replace( ">", "&gt;" );

        if(first == false) 
          s.Append("<br/>");

        s.Append(a);
        first = false;
      }

      if(s.Length == 0)
        s.Append("n/a");

      return(s.ToString());
    }

    private String GetCommentString( StructureType st, ParameterType p )
    {
      string[] comment = GetComment( st, p );
      if ( (comment == null) || (comment.Length == 0) )
        return("n/a");

      return ( GetCommentString( comment ) );
    }

    private ParameterType GetParameterFromProcedureNameAndName( string ProcedureName, string ParameterName )
    {
      ParameterType[] pl = GetParameters( ProcedureName );

      if ( pl != null )
        foreach ( ParameterType p in pl )
          if ( p.name == ParameterName )
            return p;

      return null;
    }

    private String GetMandatory( StructureType structure, ParameterType p )
    {
      bool bNew = GetMandatory( GetParameterFromProcedureNameAndName( structure.insertSP, p.name ) );
      bool bChange = GetMandatory( GetParameterFromProcedureNameAndName( structure.updateSP, p.name ) );
      bool bRemove = GetMandatory( GetParameterFromProcedureNameAndName( structure.deleteSP, p.name ) );

      String methods = "";

      if (bNew)
        methods = "N";

      if (bChange)
      {
        if (methods != "") 
        {
          methods += ", ";
        }
        methods += "C";
      }

      if (bRemove) 
      {
        if (methods != "") 
        {
          methods += ", ";
        }
        methods += "R";
      }
      if ( methods == "" )
        methods = "&nbsp;";

      return(methods);
    }

    public void GetOutPut(String structureName,TextWriter tw) 
    {
      StringBuilder s = new StringBuilder();

      int curr = 0;
      int total = messageDef.structures.Length + 1;
      ProgressEventArgs progressArgs;
      progressArgs = new ProgressEventArgs(curr,total);
      OnProgress(progressArgs);

      foreach(StructureType structure in messageDef.structures) 
      {

        if(structureName != "") 
        {
          if(structure.name != structureName)
            continue;
        }

        String interfaceName = structure.name;

        s.Append("<html>\r\n");
        s.Append("<head>\r\n");
        s.Append("<META http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\r\n");
        s.Append("<title>Segment ");
        s.Append(interfaceName);
        s.Append("</title>\r\n");
        s.Append("<link href=\"StructureDescription.css\" rel=\"stylesheet\" type=\"text/css\">\r\n");
        s.Append("</head>\r\n");
        s.Append("<body>\r\n");

        s.Append("<h1>Segment ");
        s.Append(interfaceName);
        s.Append("</h1>\r\n");
        s.Append("<h3>Description:</h3>\r\n");
        if (structure.comment != null)
          s.Append(GetCommentString(structure.comment));
        s.Append("<h3>Attributes:</h3>\r\n");
        s.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\" bordercolor=\"#000000\"><tbody>\r\n");
        s.Append("<tr class=\"tableHeader\"><th class=\"tableHeaderColumn\">Tag</th><th class=\"tableHeaderColumn\">Method</th><th class=\"tableHeaderColumn\">Type</th><th class=\"tableHeaderColumn\">Oracle Type</th><th class=\"tableHeaderColumn\">Mandatory</th><th class=\"tableHeaderColumn\">Note</th></tr>\r\n");

        ParameterType[] parameterList = GetUniqueParameterList(structure);

        foreach(ParameterType p in parameterList) 
        {
          if(! IsExternalField(p))
            continue;

          if(IsExcluded(p.name, structure))
		    continue;
          
          s.Append("<tr class=\"tableRow\"><td class=\"tableRowColumn\">");
          s.Append(GetExternalName(p, structure));
          s.Append("</td>");
          s.Append("</td><td class=\"tableRowColumn\">");
          s.Append(GetMethods(p));
          s.Append("</td><td class=\"tableRowColumn\">");

          String ExternalDataType = GetExternalDataType(p);
          char[] MyChar = {'@'};
          ExternalDataType = ExternalDataType.TrimStart(MyChar);

          s.Append(ExternalDataType);
          s.Append("</td><td class=\"tableRowColumn\">");
          s.Append(GetDBDeclaration(p));
          s.Append("</td><td class=\"tableRowColumn\">");
          s.Append(GetMandatory(structure,p));
          s.Append("</td><td class=\"tableRowColumn\">");
          s.Append(GetCommentString(structure, p));
          s.Append("</td>\r\n</tr>\r\n");

        }

        s.Append("</tbody>\r\n</table>\r\n");
        s.Append("</body>\r\n</html>\r\n");

        progressArgs.Update(++curr);
        OnProgress(progressArgs);

      }

      tw.Write(s.ToString());

      progressArgs.Update(total);
      OnProgress(progressArgs);
    }

    public void Save(String outDirectory) 
    {
      TextWriter writer = null;
      String outFileName = "";
      StringCollection s = new StringCollection();

      foreach(StructureType structure in messageDef.structures) 
      {
        s.Add(structure.name);
      }

      // Once for each structure
      foreach(String structureName in s) 
      {
        try 
        {
          outFileName = outDirectory + "\\" + structureName + ".html";
          writer = new StreamWriter(outFileName);
          GetOutPut(structureName, writer);
          writer.Flush();
          writer.Close();
        }
        catch(Exception e) 
        {
          Console.WriteLine("Failed to open output file '" + outFileName + "'" + Environment.NewLine + 
            e.Message + Environment.NewLine + 
            e.StackTrace);
          return;
        }
        finally 
        {
          if (writer != null)
            writer.Close();
        }
      }
    }

    // Make compiler happy
    public override void GetOutPut(TextWriter tw) 
    {
      GetOutPut("", tw);
    }

  }
}

