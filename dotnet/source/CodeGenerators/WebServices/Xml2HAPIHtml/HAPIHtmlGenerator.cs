using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using Imi.CodeGenerators.WebServices.Framework;
using Imi.Framework.Versioning;

namespace Imi.CodeGenerators.WebServices.XML2HAPIHtml
{

  /// <summary>
  /// Summary description for HAPIHtmlGenerator.
  /// </summary>
  public class HAPIHtmlGenerator : GenericGenerator
  {

    public HAPIHtmlGenerator( MessageDefinition messageDef, Package packageDef ) :
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
      bool bNew    = GetMandatory( GetParameterFromProcedureNameAndName( structure.insertSP, p.name ) );
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

      if(( (structure.insertSP == "" ) || bNew) &&
         ( (structure.updateSP == "" ) || bChange) &&
         ( (structure.deleteSP == "" ) || bRemove))
        methods = "*";

      if (((bNew == false) && (bChange == false) && (bRemove == false)))
        methods = "-";

      if ( methods == "" )
        methods = "&nbsp;";

      return(methods);
    }

    public string GetHAPIDeclaration( ParameterType p )
    {
      
      DataType dt = GetDataType( p );
      String decl = GetHAPIDeclaration(dt);

      return(decl);
    }

    public string GetHAPIDeclaration( DataType dt )
    {
      String decl = "";

      switch ( dt.dataType )
      {
        case DataType.StringType:
          decl = "c" +  Convert.ToString(dt.length);
          break;
        case DataType.DoubleType:
          if(dt.precision > 0) 
          {
            if(dt.scale > 0) 
              decl = "n" + Convert.ToString(dt.precision) + "," + Convert.ToString(dt.scale);
            else
              decl = "n" + Convert.ToString(dt.precision);
          }              
          else
            decl = "n";
          break;
        case DataType.IntegerType:
          if(dt.precision > 0) 
            decl = "n" + Convert.ToString(dt.precision);
          else
            decl = "n9";
          break;
        case DataType.DateTimeType:
          decl = "date";
          break;
        default:
          decl = "Syntax Error";
          break;
      }

      return(decl);
    }

    private String Spacialize(string text) 
    {
      string result = "";

      foreach(char c in text) 
      {
        if((c >= 'A') && (c <= 'Z'))
        {
          if(result != "") 
          {
            result += " ";
          }
        }

        result += c;
      }

      return(result);
    }

    public void GetOutPut(string[] structureNames, string superStructure, InterfaceTypeDirection dir, TextWriter tw) 
    {
      int curr = 0;
      int total = messageDef.structures.Length + 1;  // whats this ??
      ProgressEventArgs progressArgs = new ProgressEventArgs(curr,total);
      OnProgress(progressArgs);

      HtmlTextWriter hw = new HtmlTextWriter(tw);

      hw.Write("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
      hw.WriteLine();

      // <html xmlns="http://www.w3.org/1999/xhtml">
      hw.WriteBeginTag("html");
      hw.WriteAttribute("xmlns","http://www.w3.org/1999/xhtml");
      hw.Write(HtmlTextWriter.TagRightChar);
      hw.WriteLine(); 
      hw.Indent++;

      //	<head>
      //		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
      //		<title>Product Description</title>
      //		<link rel="stylesheet" href="emx_nav_left.css" type="text/css" media="screen" />
      //		<link rel="stylesheet" href="emx_nav_left_print.css" type="text/css" media="print" />
      //	</head>
      hw.WriteFullBeginTag("head");
      hw.WriteLine(); 
      hw.Indent++;

      hw.WriteFullBeginTag("title");
      hw.Write(Spacialize(superStructure));
      hw.WriteEndTag("title");
      hw.WriteLine();

      hw.WriteBeginTag("link");
      hw.WriteAttribute("href","./css/emx_nav_left.css");
      hw.WriteAttribute("rel","stylesheet");
      hw.WriteAttribute("type","text/css");
      hw.WriteAttribute("media","screen");
      hw.Write(HtmlTextWriter.SelfClosingTagEnd);
      hw.WriteLine();

      hw.WriteBeginTag("link");
      hw.WriteAttribute("href","./css/emx_nav_left_print.css");
      hw.WriteAttribute("rel","stylesheet");
      hw.WriteAttribute("type","text/css");
      hw.WriteAttribute("media","print");
      hw.Write(HtmlTextWriter.SelfClosingTagEnd);
      hw.WriteLine();

      hw.Indent--;
      hw.WriteEndTag("head");
      hw.WriteLine();

	    // <body>
      hw.WriteFullBeginTag("body");
      hw.WriteLine();
      hw.Indent++;

      //		<div id="pagecell1">
      //			<!--pagecell1-->
      //			<div id="breadCrumb">
      hw.WriteBeginTag("div");
      hw.WriteAttribute("id","pagecell1");
      hw.Write(HtmlTextWriter.TagRightChar);
      hw.WriteLine();
      hw.Indent++;
			hw.Write("<!--pagecell1-->");
      hw.WriteLine();

      hw.WriteBeginTag("div");
      hw.WriteAttribute("id","breadCrumb");
      hw.Write(HtmlTextWriter.TagRightChar);
      hw.WriteLine();
      hw.Indent++;

      //				<a href="#">Index</a>
      hw.WriteBeginTag("a");
      hw.WriteAttribute("href","index.html");
      hw.Write(HtmlTextWriter.TagRightChar);
      hw.Write("Index");
      hw.WriteEndTag("a");
      hw.WriteLine();

      //			</div>
      hw.Indent--;
      hw.WriteEndTag("div");
      hw.WriteLine();

      //			<div id="pageName">
      //				<h2>Product</h2>
      //			</div>
      hw.WriteBeginTag("div");
      hw.WriteAttribute("id","pageName");
      hw.Write(HtmlTextWriter.TagRightChar);
      hw.WriteLine();
      hw.Indent++;

			//	<h2>Product</h2>
      hw.WriteBeginTag("h2");
      if(dir == InterfaceTypeDirection.In) 
      {
        hw.WriteAttribute("class","in");
      }
      else
      {
        hw.WriteAttribute("class","out");
      }
      hw.Write(HtmlTextWriter.TagRightChar);
      hw.Write(Spacialize(superStructure));
      if(dir == InterfaceTypeDirection.In) 
      {
        hw.Write(" (In)");
      }
      else
      {
        hw.Write(" (Out)");
      }
      hw.WriteEndTag("h2");
      hw.WriteLine();

      hw.Indent--;
      hw.WriteEndTag("div");
      hw.WriteLine();

			// <div id="pageNav">
			// 	<div id="sectionLinks">
      hw.WriteBeginTag("div");
      hw.WriteAttribute("id","pageNav");
      hw.Write(HtmlTextWriter.TagRightChar);
      hw.WriteLine();
      hw.Indent++;

      hw.WriteBeginTag("div");
      hw.WriteAttribute("id","sectionLinks");
      hw.Write(HtmlTextWriter.TagRightChar);
      hw.WriteLine();
      hw.Indent++;

      foreach(string structureName in structureNames) 
      {
        foreach(StructureType structure in messageDef.structures) 
        {
          if(structureName != "") 
          {
            if(structure.name != structureName)
              continue;
          }

          // <a href="#ProductGlobal">Product Global</a>
          hw.WriteBeginTag("a");
          hw.WriteAttribute("href","#" + structureName);
          hw.Write(HtmlTextWriter.TagRightChar);
          hw.Write(Spacialize(structureName));
          hw.WriteEndTag("a");
          hw.WriteLine();

        }                                        
      }

      hw.Indent--;
      hw.WriteEndTag("div");
      hw.WriteLine();

      hw.Indent--;
      hw.WriteEndTag("div");
      hw.WriteLine();

			// <div id="content">
      hw.WriteBeginTag("div");
      hw.WriteAttribute("id","content");
      hw.Write(HtmlTextWriter.TagRightChar);
      hw.WriteLine();
      hw.Indent++;

      foreach(string structureName in structureNames) 
      {
        foreach(StructureType structure in messageDef.structures) 
        {
          if(structureName != "") 
          {
            if(structure.name != structureName)
              continue;
          }

          String interfaceName = structure.name;
          String niceName = Spacialize(interfaceName);

				  // <div class="description">
          hw.WriteBeginTag("div");
          hw.WriteAttribute("class","description");
          hw.Write(HtmlTextWriter.TagRightChar);
          hw.WriteLine();
          hw.Indent++;
          hw.Write("<!--description " + interfaceName + "-->");
          hw.WriteLine();

          //	<a name="ProductGlobal"/>

          hw.WriteBeginTag("a");
          hw.WriteAttribute("name",interfaceName);
          hw.Write(HtmlTextWriter.SelfClosingTagEnd);
          hw.WriteLine();

          //	<h3>ProductGlobal</h3>
          hw.WriteFullBeginTag("h3");
          hw.Write(niceName);
          hw.WriteEndTag("h3");
          hw.WriteLine();


          hw.WriteFullBeginTag("p");
          hw.Write(GetCommentString(structure.comment));
          hw.WriteEndTag("p");
          hw.WriteLine();

          ParameterType[] parameterList = GetUniqueParameterList(structure);

          bool first = true;

          foreach(ParameterType p in parameterList) 
          {
            if(p.direction == ParameterTypeDirection.Result)
              continue;

            if(first) 
            {
					    // <table border="1" cellpadding="0" cellspacing="0" bordercolor="#000000">
              hw.WriteBeginTag("table");
              hw.WriteAttribute("border","1");
              hw.WriteAttribute("cellpadding","0");
              hw.WriteAttribute("cellspacing","0");
              hw.WriteAttribute("bordercolor","#000000");
              hw.Write(HtmlTextWriter.TagRightChar);
              hw.WriteLine();
              hw.Indent++;

              // <tr>
              hw.WriteFullBeginTag("tr");
              hw.WriteLine();
              hw.Indent++;

              // <th class="colColumn">Column</th>
              // <th class="colMethod">Method</th>
              // <th class="colType">Type</th>
              // <th class="colMandatory">Mandatory</th>
              // <th class="colNote">Note</th>
              hw.WriteBeginTag("th");
              hw.WriteAttribute("class","colColumn");
              hw.Write(HtmlTextWriter.TagRightChar);
              hw.Write("Column");
              hw.WriteEndTag("th");
              hw.WriteLine();

              hw.WriteBeginTag("th");
              hw.WriteAttribute("class","colMethod");
              hw.Write(HtmlTextWriter.TagRightChar);
              hw.Write("Method");
              hw.WriteEndTag("th");
              hw.WriteLine();

              hw.WriteBeginTag("th");
              hw.WriteAttribute("class","colType");
              hw.Write(HtmlTextWriter.TagRightChar);
              hw.Write("Type");
              hw.WriteEndTag("th");
              hw.WriteLine();
            
              if(dir != InterfaceTypeDirection.Out) 
              {
                hw.WriteBeginTag("th");
                hw.WriteAttribute("class","colMandatory");
                hw.Write(HtmlTextWriter.TagRightChar);
                hw.Write("Mand.");
                hw.WriteEndTag("th");
                hw.WriteLine();
              }
            
              hw.WriteBeginTag("th");
              hw.WriteAttribute("class","colNote");
              hw.Write(HtmlTextWriter.TagRightChar);
              hw.Write("Note");
              hw.WriteEndTag("th");
              hw.WriteLine();
            
              hw.Indent--;
              hw.WriteEndTag("tr");
              hw.WriteLine();

              first = false;
            }

            // <tr>
            hw.WriteFullBeginTag("tr");
            hw.WriteLine();
            hw.Indent++;

						// <td class="colColumn">ClientIdentity</td>
						// <td class="colMethod">N, C, R</td>
						// <td class="colType">c17</td>
						// <td class="colMandatory">N, C, R</td>
						// <td class="colNote style1">Client Identity</td>
            hw.WriteBeginTag("td");
            hw.WriteAttribute("class","colColumn");
            hw.Write(HtmlTextWriter.TagRightChar);
            hw.Write(GetExternalName(p, structure));
            hw.WriteEndTag("td");
            hw.WriteLine();
          
            hw.WriteBeginTag("td");
            hw.WriteAttribute("class","colMethod");
            hw.Write(HtmlTextWriter.TagRightChar);
            hw.Write(GetMethods(p));
            hw.WriteEndTag("td");
            hw.WriteLine();
            
            hw.WriteBeginTag("td");
            hw.WriteAttribute("class","colType");
            hw.Write(HtmlTextWriter.TagRightChar);
            String ExternalDataType = GetHAPIDeclaration(p);
            hw.Write(ExternalDataType);
            hw.WriteEndTag("td");
            hw.WriteLine();

            if(dir != InterfaceTypeDirection.Out) 
            {
              hw.WriteBeginTag("td");
              hw.WriteAttribute("class","colMandatory");
              hw.Write(HtmlTextWriter.TagRightChar);
              hw.Write(GetMandatory(structure,p));
              hw.WriteEndTag("td");
              hw.WriteLine();
            }

            hw.WriteBeginTag("td");
            hw.WriteAttribute("class","colNote");
            hw.Write(HtmlTextWriter.TagRightChar);
            hw.Write(GetCommentString(structure, p));
            hw.WriteEndTag("td");
            hw.WriteLine();

            // </tr>
            hw.Indent--;
            hw.WriteEndTag("tr");
            hw.WriteLine();
          }

          if(!first) 
          {
            // </table>
            hw.Indent--;
            hw.WriteEndTag("table");
            hw.WriteLine();

            // </div>
            hw.Indent--;
            hw.WriteEndTag("div");
            hw.WriteLine();
            hw.Write("<!--end description " + interfaceName + "-->");
            hw.WriteLine();
          }

          progressArgs.Update(++curr);
          OnProgress(progressArgs);
        }
      }

      // </div>
      hw.Indent--;
      hw.WriteEndTag("div");
      hw.WriteLine();
      hw.Write("<!--end content-->");
      hw.WriteLine();

      // <div id="siteInfo">&copy;2005 Industri-Matematik AB</div>
      hw.WriteBeginTag("div");
      hw.WriteAttribute("id","siteInfo");
      hw.Write(HtmlTextWriter.TagRightChar);
      hw.Write("Version " + CurrentVersion.VersionName + " generated on " + DateTime.Now);
      hw.WriteBeginTag("br");
      hw.Write(HtmlTextWriter.SelfClosingTagEnd);
      hw.Write("&copy;" + DateTime.Now.Year.ToString() + " Industri-Matematik AB");
      hw.WriteEndTag("div");
      hw.WriteLine();

      // </div>
      hw.Indent--;
      hw.WriteEndTag("div");
      hw.WriteLine();
      hw.Write("<!--end pagecell1-->");
      hw.WriteLine();

      hw.Indent--;
      hw.WriteEndTag("body");
      hw.WriteLine();

      hw.Indent--;
      hw.WriteEndTag("html");
      hw.WriteLine();

      progressArgs.Update(total);
      OnProgress(progressArgs);
    }

    private void CreateHTMLFile(string[] structureNames, string superStructure, InterfaceTypeDirection dir, String outDirectory) 
    {
      TextWriter writer = null;
      String outFileName = "";

      try 
      {
        outFileName = outDirectory + "\\" + superStructure + ".html";
        writer = new StreamWriter(outFileName);
        GetOutPut(structureNames, superStructure, dir, writer);
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

    public void Save(String outDirectory) 
    {
      StringCollection s = new StringCollection();

      foreach(InterfaceType i in messageDef.interfaces) 
      {

        foreach(StructureType st in messageDef.structures) 
        {
          if(st.name == i.structure) 
          {
            if((st.children != null) && (st.children.Length > 0)) 
            {
              string[] lst = new string[st.children.Length];
              int idx = 0;
              foreach(ChildType c in st.children) 
              {
                lst[idx++] = c.name;
              }
              
              CreateHTMLFile(lst, i.name, i.direction, outDirectory);
            }
            else 
            {
              CreateHTMLFile(new string[] { st.name }, i.name, i.direction, outDirectory);
            }
          }
        }
      }

    }

    // Make compiler happy
    public override void GetOutPut(TextWriter tw) 
    {
      GetOutPut(null, "", InterfaceTypeDirection.InOut, tw);
    }

  }
}

