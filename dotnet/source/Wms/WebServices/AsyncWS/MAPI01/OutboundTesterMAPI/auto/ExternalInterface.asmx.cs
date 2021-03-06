/*
  File           : 

  Description    : Public interface class for WebService interface for inbound data.
                   This code was generated, do not edit.

*/
using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using Imi.Framework.Shared;

namespace Imi.Wms.WebServices.OutboundTesterMAPI
{
  [WebService(Namespace="http://im.se/wms/webservices/", Description="MAPI Outbound (Sender) interface 7.0.1 generated on 2008-05-12 13:33:59")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class InboundInterface : WSBase
  {
    public string _Debug()
    {
      return "Generated on   : 2008-05-12 13:33:59\r\n" +
             "Generated by   : SWG\\olla@IMIPC1091\r\n" +
             "Generated in   : C:\\project\\views\\olla_dotnet_ss\\dotnet\\source\\CodeGenerators\\wscc\r\n";
    }

    private void Log( bool Enter )
    {

      string path = @"C:\log\Inbound.log";

      using (StreamWriter w = File.AppendText(path))
      {
        w.Write( "{0} {1}.{2}", System.DateTime.Now.ToShortDateString(), 
          System.DateTime.Now.ToLongTimeString(), System.Convert.ToString( System.DateTime.Now.Millisecond ) );

        StackTrace st = new StackTrace(1, true);

        if ( st.FrameCount > 1 )
        {
          StackFrame sf = st.GetFrame(1);
        
          w.Write("{0}{1}{2}", '\t', sf.GetMethod(), '\t' );
        }
        
        if (Enter)
          w.WriteLine( "Enter" );
        else
          w.WriteLine( "Leave" );
      }
    }

    private void EnterProc()
    {
      try
      {
        // Log( true );
      }
      catch
      {
      }
    }

    private void ExitProc()
    {
      try
      {
        // Log( false );
      }
      catch
      {
      }
    }

    [WebMethod]
    public string WhoAmI()
    {
      EnterProc();

      string s = CurrentVersion.VersionName;

      ExitProc();

      return s;
    }


    [WebMethod]
    public void MovementIn_01( string MHId, string TransactionId, MovementIn_01Doc aMovementIn_01Doc )
    {
      EnterProc();
      return;
      MovementIn_01Insert aMovementIn_01Handler;

      try
      {
        MessageTransaction mt = BeginWebmethod( MHId, TransactionId, "MAPI_OUT_MOVEMENT_IN_01" );

        try
        {
          aMovementIn_01Handler = new MovementIn_01Insert( this );
        }
        catch (Exception e)
        {
          Exception InternalError = new Exception( "InternalError: Building insert handler", e );
          throw (InternalError);
        }

        try
        {
          if ( aMovementIn_01Doc == null )
          {
            Exception InternalError = new Exception( "DataError: Root object cannot be null" );
            throw (InternalError);
          }
          aMovementIn_01Handler.Process( ref mt, null, aMovementIn_01Doc );
          GetDataBase().Commit();
          mt.Signal();
        }
        catch (Exception e)
        {
          try
          {
            GetDataBase().Rollback();
          }
          catch (Exception)
          {}
          Exception InternalError = new Exception( "DataError: Error processing data", e );
          throw (InternalError);
        }
      }

      finally
      {
        EndWebmethod();
      }

      ExitProc();

      return;
    }


    [WebMethod]
    public void MovementOut_01( string MHId, string TransactionId, MovementOut_01Doc aMovementOut_01Doc )
    {
      EnterProc();
      return;
      MovementOut_01Insert aMovementOut_01Handler;

      try
      {
        MessageTransaction mt = BeginWebmethod( MHId, TransactionId, "MAPI_OUT_MOVEMENT_OUT_01" );

        try
        {
          aMovementOut_01Handler = new MovementOut_01Insert( this );
        }
        catch (Exception e)
        {
          Exception InternalError = new Exception( "InternalError: Building insert handler", e );
          throw (InternalError);
        }

        try
        {
          if ( aMovementOut_01Doc == null )
          {
            Exception InternalError = new Exception( "DataError: Root object cannot be null" );
            throw (InternalError);
          }
          aMovementOut_01Handler.Process( ref mt, null, aMovementOut_01Doc );
          GetDataBase().Commit();
          mt.Signal();
        }
        catch (Exception e)
        {
          try
          {
            GetDataBase().Rollback();
          }
          catch (Exception)
          {}
          Exception InternalError = new Exception( "DataError: Error processing data", e );
          throw (InternalError);
        }
      }

      finally
      {
        EndWebmethod();
      }

      ExitProc();

      return;
    }


    [WebMethod]
    public void Product_01( string MHId, string TransactionId, Product_01Doc aProduct_01Doc )
    {
      EnterProc();
      return;
      Product_01Insert aProduct_01Handler;

      try
      {
        MessageTransaction mt = BeginWebmethod( MHId, TransactionId, "MAPI_OUT_PRODUCT_01" );

        try
        {
          aProduct_01Handler = new Product_01Insert( this );
        }
        catch (Exception e)
        {
          Exception InternalError = new Exception( "InternalError: Building insert handler", e );
          throw (InternalError);
        }

        try
        {
          if ( aProduct_01Doc == null )
          {
            Exception InternalError = new Exception( "DataError: Root object cannot be null" );
            throw (InternalError);
          }
          aProduct_01Handler.Process( ref mt, null, aProduct_01Doc );
          GetDataBase().Commit();
          mt.Signal();
        }
        catch (Exception e)
        {
          try
          {
            GetDataBase().Rollback();
          }
          catch (Exception)
          {}
          Exception InternalError = new Exception( "DataError: Error processing data", e );
          throw (InternalError);
        }
      }

      finally
      {
        EndWebmethod();
      }

      ExitProc();

      return;
    }


    [WebMethod]
    public void StatusUpdate_01( string MHId, string TransactionId, StatusUpdate_01Doc aStatusUpdate_01Doc )
    {
      EnterProc();

      return;
      StatusUpdate_01Insert aStatusUpdate_01Handler;

      try
      {
        MessageTransaction mt = BeginWebmethod( MHId, TransactionId, "MAPI_OUT_STATUS_UPDAT_01" );

        try
        {
          aStatusUpdate_01Handler = new StatusUpdate_01Insert( this );
        }
        catch (Exception e)
        {
          Exception InternalError = new Exception( "InternalError: Building insert handler", e );
          throw (InternalError);
        }

        try
        {
          if ( aStatusUpdate_01Doc == null )
          {
            Exception InternalError = new Exception( "DataError: Root object cannot be null" );
            throw (InternalError);
          }
          aStatusUpdate_01Handler.Process( ref mt, null, aStatusUpdate_01Doc );
          GetDataBase().Commit();
          mt.Signal();
        }
        catch (Exception e)
        {
          try
          {
            GetDataBase().Rollback();
          }
          catch (Exception)
          {}
          Exception InternalError = new Exception( "DataError: Error processing data", e );
          throw (InternalError);
        }
      }

      finally
      {
        EndWebmethod();
      }

      ExitProc();

      return;
    }

  }
}
