using System;
using System.Collections.Specialized;
using System.Collections;
using Imi.Framework.Shared;
using Imi.CodeGenerators.WebServices.Framework;

// ----------------------------------------------------------------------------

namespace Imi.CodeGenerators.WebServices.Spec2XML
{
  
  // ----------------------------------------------------------------------------

  class PLSQLSpecParam
  {
    public string ParamName;
    public bool DirIn = false;
    public bool DirOut = false;
    public string TableName = "";
    public string ColumnName = "";
    public string UnconnectedType = "";
    public string DefaultValue = "";
    public bool Mandatory = false;

    public PLSQLSpecParam( string aParamName )
    {
      ParamName = aParamName;
    }
  }

  // ----------------------------------------------------------------------------

  class PLSQLSpecProc
  {
    public string ProcName;
    public ArrayList Params;
    public string UnconnectedReturnType = "";

    public PLSQLSpecProc( string aProcName )
    {
      ProcName = aProcName;
      Params = new ArrayList();
    }
  }

  // ----------------------------------------------------------------------------
  
  class PLSQLSpecPackage
  {
    public string PackageName;
    public ArrayList Procedures;

    public PLSQLSpecPackage( string aPackageName )
    {
      PackageName = aPackageName;
      Procedures = new ArrayList();
    }
  }

  // ----------------------------------------------------------------------------
  
  class PLSQLSpecParser
  {
    public PLSQLSpecPackage Package;

    // ----------------------------------------------------------------------------
    
    public PLSQLSpecParser( string spectext )
    {
      int ixCurrent = 0;
      SourceTokenizer t = new SourceTokenizer( spectext, " ();%.:=\r\n\t,/*" );
      string s;
      int ixLast;
      string DefaultValue = "";
      bool bIsFunction = false;

      bool WaitForPackageName = false;
      bool WaitForProcedureName = false;
      bool WaitForParamName = false;
      bool WaitForMandatory = false;
      bool WaitForParamType = false;
      bool WaitForParamSeparator = false;
      bool WaitForReturnType = false;
      bool WaitForDefaultValue = false;

      PLSQLSpecProc CurrentProc = null;
      PLSQLSpecParam CurrentParam = null;

      while ( !t.EOF( ixCurrent ) )
      {
        s = "";
        ixLast = 0;

        if ( t.EatSpace( ref ixCurrent ) )
        {
          // eat space and countine in same lap!
        }

        if (t.IsComment( ixCurrent, "/"+"*", "*"+"/", ref s, ref ixLast ) )
        {
          if ( WaitForMandatory )
          {
            if ( s.Trim().ToUpper() == "MANDATORY" )
            {
              CurrentParam.Mandatory = true;
              WaitForMandatory = false;
            }
          }

          // throw away comments
          ixCurrent = ixLast;
          //Console.WriteLine( "<comment><" + s + ">" );
        }

        else if ( t.Peek( ixCurrent, 0 ).ToUpper() == "PACKAGE" )
        {
          WaitForPackageName = true;
        }

        else if ( WaitForPackageName )
        {
          //Console.WriteLine( "<package><" + t.Peek( ixCurrent, 0 ) + ">" );
          
          Package = new PLSQLSpecPackage( t.Peek( ixCurrent, 0 ) );
          
          WaitForPackageName = false;
        }

        else if ( t.Peek( ixCurrent, 0 ).ToUpper() == "PROCEDURE" )
        {
          WaitForProcedureName = true;
          bIsFunction = false;
        }

        else if ( t.Peek( ixCurrent, 0 ).ToUpper() == "FUNCTION" )
        {
          WaitForProcedureName = true;
          bIsFunction = true;
        }

        else if ( WaitForProcedureName )
        {
          //Console.WriteLine( "  <procedure><" + t.Peek( ixCurrent, 0 ) + ">" );
          string name = t.Peek(ixCurrent, 0);
          
          // Ignore the function Get_Source_Version_Info. It's added by the deploy.
          if (name != "Get_Source_Version_Info")
          {
              CurrentProc = new PLSQLSpecProc(name);
              Package.Procedures.Add( CurrentProc );

              WaitForParamName = true;
          }
          else
          {
            CurrentProc = null;
            WaitForParamName = false;
          }

          WaitForProcedureName = false;
        }

        // sample params
        // ,ALMID_O         out ALM.ALMID%type
        // ,CHK_AI_I            varchar2  := '1'  );
        // ,ALMID_I /* mandatory */ ALM.ALMID%type

        else if ( WaitForParamName ) // ALMID_O
        {
          if ( t.Peek( ixCurrent, 0 ) == "(" )
          {
            // ignore start ( as it is expected
          }
          else
          {
            //Console.WriteLine( "    <param><" + t.Peek( ixCurrent, 0 ) + ">" );
            
            CurrentParam = new PLSQLSpecParam( t.Peek( ixCurrent, 0 ) );
            CurrentProc.Params.Add( CurrentParam );

            WaitForParamName = false;
            WaitForParamType = true;
            WaitForMandatory = true;
          }
        }

        else if ( WaitForParamType )
        {
          if ( t.Peek( ixCurrent, 0 ).ToUpper() == "IN" ) // in ALM.ALMID%type
          {
            //Console.WriteLine( "      <direction><IN>" );
            CurrentParam.DirIn = true;
          }
          else if ( t.Peek( ixCurrent, 0 ).ToUpper() == "OUT" ) // out ALM.ALMID%type
          {
            //Console.WriteLine( "      <direction><OUT>" );
            CurrentParam.DirOut = true;
          }
          else
          {
            if ( ( t.Peek( ixCurrent, 1 ) == "."  ) && 
              ( t.Peek( ixCurrent, 3 ) == "%"  ) &&
              ( t.Peek( ixCurrent, 4 ).ToUpper() == "TYPE"  ) ) // ALM.ALMID%type
            {
              // table.name%type
              //Console.WriteLine( "      <table><" + t.Peek( ixCurrent, 0 ) + ">" );
              //Console.WriteLine( "      <column><" + t.Peek( ixCurrent, 2 ) + ">" );

              CurrentParam.TableName = t.Peek( ixCurrent, 0 );
              CurrentParam.ColumnName = t.Peek( ixCurrent, 2 );

              ixCurrent += 4;
            }

            else // varchar2
            {
              //Console.WriteLine( "      <type><" + t.Peek( ixCurrent, 0 ).ToUpper() + ">" );
              
              CurrentParam.UnconnectedType = t.Peek( ixCurrent, 0 ).ToUpper();
            }

            WaitForParamType = false;
            WaitForParamSeparator = true;
            WaitForMandatory = false;
          }
        }

        else if ( WaitForParamSeparator ) // ","  ")"  ":= '1'"
        {
          WaitForParamSeparator = false;

          if ( t.Peek( ixCurrent, 0 ) == ")" )
          {
            WaitForReturnType = true;
          }

          else if ( t.Peek( ixCurrent, 0 ) == "," )
          {
            WaitForParamName = true;
          }

          else if ( ( t.Peek( ixCurrent, 0 ) == ":" ) &&
            ( t.Peek( ixCurrent, 1 ) == "=" ) )
          {
            WaitForDefaultValue = true;
            ixCurrent += 1;
          }

          else
          {
            Console.WriteLine( "syntax error or unsupported format of spec" );
            if (CurrentProc != null)
              Console.WriteLine("  parsing procedure/function " + CurrentProc.ProcName);
            if (CurrentParam != null)
              Console.WriteLine("  after param " + CurrentParam.ParamName);
            
            Console.WriteLine(" near <" + t.Peek(ixCurrent, 0) + ">");
            Package = null;
          }
        }

        else if ( WaitForDefaultValue ) // '1' 1
        {
            if (t.IsString(ixCurrent, "'", ref DefaultValue, ref ixLast))
                ixCurrent = ixLast;
            else
            {
                // Def.XXXXX
                if (t.Peek(ixCurrent, 1) == ".")
                {
                    DefaultValue = t.Peek(ixCurrent, 0) + t.Peek(ixCurrent, 1) + t.Peek(ixCurrent, 2);
                    ixCurrent += 2;
                }
                else
                    DefaultValue = t.Peek(ixCurrent, 0);
            }

          //Console.WriteLine( "      <default><" + DefaultValue + ">" );

          CurrentParam.DefaultValue = DefaultValue;

          WaitForDefaultValue = false;
          WaitForParamSeparator = true; // redo that step to catch return value after default
        }

        else if ( WaitForReturnType )
        {
          if (bIsFunction)
          {
            if ( t.Peek( ixCurrent, 0 ).ToUpper() == "RETURN" )
            {
              // ignore
            }

            else
            {
              //Console.WriteLine( "    <return type><" + t.Peek( ixCurrent, 0 ).ToUpper() + ">" );

              CurrentProc.UnconnectedReturnType = t.Peek( ixCurrent, 0 ).ToUpper();

              WaitForReturnType = false;
            }
          }

          else if ( t.Peek( ixCurrent, 0 ) == ";" )
          {
            WaitForReturnType = false;
          }

        }

        ixCurrent++;
      }
    }
  }

  // ----------------------------------------------------------------------------
}
