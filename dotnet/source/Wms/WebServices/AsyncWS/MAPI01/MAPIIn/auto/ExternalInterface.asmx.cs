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

namespace Imi.Wms.WebServices.MAPIIn
{
  [WebService(Namespace="http://im.se/wms/webservices/", Description="MAPI Inbound (Receiver) interface 7.0.1 generated on 2010-05-07 16:22:30")]
  [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
  public class InboundInterface : WSBase
  {
    public string _Debug()
    {
      return "Generated on   : 2010-05-07 16:22:30\r\n" +
             "Generated by   : SWG\\olla@IMIPC1091\r\n" +
             "Generated in   : C:\\project\\views\\olla_5.2E.2_ss\\dotnet\\source\\CodeGenerators\\wscc\r\n";
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
    public void MovementPickUp_01( string MHId, string TransactionId, MovementPickUp_01Doc aMovementPickUp_01Doc )
    {
      EnterProc();

      MovementPickUp_01Insert aMovementPickUp_01Handler;

      try
      {
        MessageTransaction mt = BeginWebmethod( MHId, TransactionId, "MOVM_PICKUP_01" );

        try
        {
          aMovementPickUp_01Handler = new MovementPickUp_01Insert( this );
        }
        catch (Exception e)
        {
          Exception InternalError = new Exception( "InternalError: Building insert handler", e );
          throw (InternalError);
        }

        try
        {
          if ( aMovementPickUp_01Doc == null )
          {
            Exception InternalError = new Exception( "DataError: Root object cannot be null" );
            throw (InternalError);
          }
          aMovementPickUp_01Handler.Process( ref mt, null, aMovementPickUp_01Doc );
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
    public void MovementDrop_01( string MHId, string TransactionId, MovementDrop_01Doc aMovementDrop_01Doc )
    {
      EnterProc();

      MovementDrop_01Insert aMovementDrop_01Handler;

      try
      {
        MessageTransaction mt = BeginWebmethod( MHId, TransactionId, "MOVM_DROP_01" );

        try
        {
          aMovementDrop_01Handler = new MovementDrop_01Insert( this );
        }
        catch (Exception e)
        {
          Exception InternalError = new Exception( "InternalError: Building insert handler", e );
          throw (InternalError);
        }

        try
        {
          if ( aMovementDrop_01Doc == null )
          {
            Exception InternalError = new Exception( "DataError: Root object cannot be null" );
            throw (InternalError);
          }
          aMovementDrop_01Handler.Process( ref mt, null, aMovementDrop_01Doc );
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
    public void HandlingUnitStatus_01( string MHId, string TransactionId, HandlingUnitStatus_01Doc aHandlingUnitStatus_01Doc )
    {
      EnterProc();

      HandlingUnitStatus_01Insert aHandlingUnitStatus_01Handler;

      try
      {
        MessageTransaction mt = BeginWebmethod( MHId, TransactionId, "STATUS_01" );

        try
        {
          aHandlingUnitStatus_01Handler = new HandlingUnitStatus_01Insert( this );
        }
        catch (Exception e)
        {
          Exception InternalError = new Exception( "InternalError: Building insert handler", e );
          throw (InternalError);
        }

        try
        {
          if ( aHandlingUnitStatus_01Doc == null )
          {
            Exception InternalError = new Exception( "DataError: Root object cannot be null" );
            throw (InternalError);
          }
          aHandlingUnitStatus_01Handler.Process( ref mt, null, aHandlingUnitStatus_01Doc );
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
