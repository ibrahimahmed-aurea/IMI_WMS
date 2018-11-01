/*
  File           : 

  Description    : Internal classes for inserting inbound data into queue tables.
                   This code was generated, do not edit.

*/
using System;
using System.Text;
using System.Data;
using System.Data.Common;

namespace Imi.Wms.WebServices.MAPIIn
{
  public class InsertHandler
  {
      public string _Debug()
      {
          return "Generated on   : 2010-05-07 16:22:29\r\n" +
                 "Generated by   : SWG\\olla@IMIPC1091\r\n" +
                 "Generated in   : C:\\project\\views\\olla_5.2E.2_ss\\dotnet\\source\\CodeGenerators\\wscc\r\n";
      }
  }

  public class MovementPickUp_01Insert : SegmentImpl
  {

    public MovementPickUp_01Insert(WSBase owner)
        : base(owner)
    {
        StringBuilder s = new StringBuilder("insert into MAPI_IN_01_MOVM_PICKUP ( ");
        s.Append(" MAPI_IN_ID");
        s.Append(",SEQNUM");
        s.Append(",OPCODE");
        s.Append(",MaterialHandlingSystemId");
        s.Append(",MovementOrder");
        s.Append(",SequenceNumber");
        s.Append(",MovementTaskStatus");

        s.Append(" ) values ( ");

        s.Append(" :MAPI_IN_ID");
        s.Append(",:SEQNUM");
        s.Append(",:OPCODE");
        s.Append(",:MaterialHandlingSystemId");
        s.Append(",:MovementOrder");
        s.Append(",:SequenceNumber");
        s.Append(",:MovementTaskStatus");

        s.Append(" )");

        fStmt.CommandText = s.ToString();

        fStmt.Parameters.Add(StringParam("MAPI_IN_ID", 35));
        fStmt.Parameters.Add(NumberParam("SEQNUM", 9, 0));
        fStmt.Parameters.Add(StringParam("OPCODE", 1));
        fStmt.Parameters.Add(StringParam("MaterialHandlingSystemId", 35));
        fStmt.Parameters.Add(NumberParam("MovementOrder", 8, 0));
        fStmt.Parameters.Add(NumberParam("SequenceNumber", 5, 0));
        fStmt.Parameters.Add(StringParam("MovementTaskStatus", 255));

        fStmt.Prepare();

    }

    public void Process(ref MessageTransaction trans, SegmentImpl parent, MovementPickUp_01Doc p)
    {
        StringBuilder error = new StringBuilder();

        if (p == null)
        {
            // No data is available - abort
            throw new NullReferenceException("Failed to process message " + p.GetType() + ". Message structure is empty (null).");
        }

        fStmt.Transaction = trans.Transaction;

        if (p.OPCODE == null)
        {
            error.AppendLine("Invalid Opcode (null) in " + p.GetType());
        }
        else
        {
            bool OpcodeValid = false;
            OpcodeValid |= (p.OPCODE == "0");
            OpcodeValid |= (p.OPCODE == "1");

            if (!OpcodeValid)
            {
                error.AppendLine("Opcode not supported/invalid (" + p.OPCODE + ") in " + p.GetType());
            }
        }

        (fStmt.Parameters["MAPI_IN_ID"] as IDbDataParameter).Value = StringValue(trans.MapiInId);

        (fStmt.Parameters["SEQNUM"] as IDbDataParameter).Value = NumberValue(trans.TransSeq);

        if (p.OPCODE != null)
        {
            if (p.OPCODE.Length > 1)
                error.AppendLine("Value for MovementPickUp_01Doc.OPCODE too long, max 1 chars");

            (fStmt.Parameters["OPCODE"] as IDbDataParameter).Value = p.OPCODE;
        }
        else
            (fStmt.Parameters["OPCODE"] as IDbDataParameter).Value = DBNull.Value;

        if (p.MaterialHandlingSystemId != null)
        {
            if (p.MaterialHandlingSystemId.Length > 35)
                error.AppendLine("Value for MovementPickUp_01Doc.MaterialHandlingSystemId too long, max 35 chars");

            if (p.MaterialHandlingSystemId.Length == 0)
                error.AppendLine("Zero length for mandatory parameter MovementPickUp_01Doc.MaterialHandlingSystemId not allowed");

            (fStmt.Parameters["MaterialHandlingSystemId"] as IDbDataParameter).Value = p.MaterialHandlingSystemId;
        }
        else
            error.AppendLine("Null value for mandatory parameter MovementPickUp_01Doc.MaterialHandlingSystemId not allowed");


        if (p.MovementOrder != null)
        {
            (fStmt.Parameters["MovementOrder"] as IDbDataParameter).Value = p.MovementOrder;
        }
        else
            error.AppendLine("Null value for mandatory parameter MovementPickUp_01Doc.MovementOrder not allowed");


        if (p.SequenceNumber != null)
        {
            (fStmt.Parameters["SequenceNumber"] as IDbDataParameter).Value = p.SequenceNumber;
        }
        else
            error.AppendLine("Null value for mandatory parameter MovementPickUp_01Doc.SequenceNumber not allowed");


        if (p.MovementTaskStatus != null)
        {
            if (p.MovementTaskStatus.Length > 255)
                error.AppendLine("Value for MovementPickUp_01Doc.MovementTaskStatus too long, max 255 chars");

            if (p.MovementTaskStatus.Length == 0)
                error.AppendLine("Zero length for mandatory parameter MovementPickUp_01Doc.MovementTaskStatus not allowed");

            (fStmt.Parameters["MovementTaskStatus"] as IDbDataParameter).Value = p.MovementTaskStatus;
        }
        else
            error.AppendLine("Null value for mandatory parameter MovementPickUp_01Doc.MovementTaskStatus not allowed");


        if (error.Length > 0)
        {
            throw (new Exception(error.ToString()));
        }

        trans.TransSeq++;

        fStmt.ExecuteNonQuery();

      }
  }

  public class MovementDrop_01Insert : SegmentImpl
  {

    public MovementDrop_01Insert(WSBase owner)
        : base(owner)
    {
        StringBuilder s = new StringBuilder("insert into MAPI_IN_01_MOVM_DROP ( ");
        s.Append(" MAPI_IN_ID");
        s.Append(",SEQNUM");
        s.Append(",OPCODE");
        s.Append(",MaterialHandlingSystemId");
        s.Append(",MovementOrder");
        s.Append(",SequenceNumber");
        s.Append(",MovementTaskStatus");

        s.Append(" ) values ( ");

        s.Append(" :MAPI_IN_ID");
        s.Append(",:SEQNUM");
        s.Append(",:OPCODE");
        s.Append(",:MaterialHandlingSystemId");
        s.Append(",:MovementOrder");
        s.Append(",:SequenceNumber");
        s.Append(",:MovementTaskStatus");

        s.Append(" )");

        fStmt.CommandText = s.ToString();

        fStmt.Parameters.Add(StringParam("MAPI_IN_ID", 35));
        fStmt.Parameters.Add(NumberParam("SEQNUM", 9, 0));
        fStmt.Parameters.Add(StringParam("OPCODE", 1));
        fStmt.Parameters.Add(StringParam("MaterialHandlingSystemId", 35));
        fStmt.Parameters.Add(NumberParam("MovementOrder", 8, 0));
        fStmt.Parameters.Add(NumberParam("SequenceNumber", 5, 0));
        fStmt.Parameters.Add(StringParam("MovementTaskStatus", 255));

        fStmt.Prepare();

    }

    public void Process(ref MessageTransaction trans, SegmentImpl parent, MovementDrop_01Doc p)
    {
        StringBuilder error = new StringBuilder();

        if (p == null)
        {
            // No data is available - abort
            throw new NullReferenceException("Failed to process message " + p.GetType() + ". Message structure is empty (null).");
        }

        fStmt.Transaction = trans.Transaction;

        if (p.OPCODE == null)
        {
            error.AppendLine("Invalid Opcode (null) in " + p.GetType());
        }
        else
        {
            bool OpcodeValid = false;
            OpcodeValid |= (p.OPCODE == "0");
            OpcodeValid |= (p.OPCODE == "1");

            if (!OpcodeValid)
            {
                error.AppendLine("Opcode not supported/invalid (" + p.OPCODE + ") in " + p.GetType());
            }
        }

        (fStmt.Parameters["MAPI_IN_ID"] as IDbDataParameter).Value = StringValue(trans.MapiInId);

        (fStmt.Parameters["SEQNUM"] as IDbDataParameter).Value = NumberValue(trans.TransSeq);

        if (p.OPCODE != null)
        {
            if (p.OPCODE.Length > 1)
                error.AppendLine("Value for MovementDrop_01Doc.OPCODE too long, max 1 chars");

            (fStmt.Parameters["OPCODE"] as IDbDataParameter).Value = p.OPCODE;
        }
        else
            (fStmt.Parameters["OPCODE"] as IDbDataParameter).Value = DBNull.Value;

        if (p.MaterialHandlingSystemId != null)
        {
            if (p.MaterialHandlingSystemId.Length > 35)
                error.AppendLine("Value for MovementDrop_01Doc.MaterialHandlingSystemId too long, max 35 chars");

            if (p.MaterialHandlingSystemId.Length == 0)
                error.AppendLine("Zero length for mandatory parameter MovementDrop_01Doc.MaterialHandlingSystemId not allowed");

            (fStmt.Parameters["MaterialHandlingSystemId"] as IDbDataParameter).Value = p.MaterialHandlingSystemId;
        }
        else
            error.AppendLine("Null value for mandatory parameter MovementDrop_01Doc.MaterialHandlingSystemId not allowed");


        if (p.MovementOrder != null)
        {
            (fStmt.Parameters["MovementOrder"] as IDbDataParameter).Value = p.MovementOrder;
        }
        else
            error.AppendLine("Null value for mandatory parameter MovementDrop_01Doc.MovementOrder not allowed");


        if (p.SequenceNumber != null)
        {
            (fStmt.Parameters["SequenceNumber"] as IDbDataParameter).Value = p.SequenceNumber;
        }
        else
            error.AppendLine("Null value for mandatory parameter MovementDrop_01Doc.SequenceNumber not allowed");


        if (p.MovementTaskStatus != null)
        {
            if (p.MovementTaskStatus.Length > 255)
                error.AppendLine("Value for MovementDrop_01Doc.MovementTaskStatus too long, max 255 chars");

            if (p.MovementTaskStatus.Length == 0)
                error.AppendLine("Zero length for mandatory parameter MovementDrop_01Doc.MovementTaskStatus not allowed");

            (fStmt.Parameters["MovementTaskStatus"] as IDbDataParameter).Value = p.MovementTaskStatus;
        }
        else
            error.AppendLine("Null value for mandatory parameter MovementDrop_01Doc.MovementTaskStatus not allowed");


        if (error.Length > 0)
        {
            throw (new Exception(error.ToString()));
        }

        trans.TransSeq++;

        fStmt.ExecuteNonQuery();

      }
  }

  public class HandlingUnitStatus_01Insert : SegmentImpl
  {

    public HandlingUnitStatus_01Insert(WSBase owner)
        : base(owner)
    {
        StringBuilder s = new StringBuilder("insert into MAPI_IN_01_STATUS ( ");
        s.Append(" MAPI_IN_ID");
        s.Append(",SEQNUM");
        s.Append(",OPCODE");
        s.Append(",MaterialHandlingSystemId");
        s.Append(",HandlingUnitId");
        s.Append(",HandlingUnitInStatus");
        s.Append(",HandlingUnitOutStatus");

        s.Append(" ) values ( ");

        s.Append(" :MAPI_IN_ID");
        s.Append(",:SEQNUM");
        s.Append(",:OPCODE");
        s.Append(",:MaterialHandlingSystemId");
        s.Append(",:HandlingUnitId");
        s.Append(",:HandlingUnitInStatus");
        s.Append(",:HandlingUnitOutStatus");

        s.Append(" )");

        fStmt.CommandText = s.ToString();

        fStmt.Parameters.Add(StringParam("MAPI_IN_ID", 35));
        fStmt.Parameters.Add(NumberParam("SEQNUM", 9, 0));
        fStmt.Parameters.Add(StringParam("OPCODE", 1));
        fStmt.Parameters.Add(StringParam("MaterialHandlingSystemId", 35));
        fStmt.Parameters.Add(StringParam("HandlingUnitId", 6));
        fStmt.Parameters.Add(StringParam("HandlingUnitInStatus", 1));
        fStmt.Parameters.Add(StringParam("HandlingUnitOutStatus", 1));

        fStmt.Prepare();

    }

    public void Process(ref MessageTransaction trans, SegmentImpl parent, HandlingUnitStatus_01Doc p)
    {
        StringBuilder error = new StringBuilder();

        if (p == null)
        {
            // No data is available - abort
            throw new NullReferenceException("Failed to process message " + p.GetType() + ". Message structure is empty (null).");
        }

        fStmt.Transaction = trans.Transaction;

        if (p.OPCODE == null)
        {
            error.AppendLine("Invalid Opcode (null) in " + p.GetType());
        }
        else
        {
            bool OpcodeValid = false;
            OpcodeValid |= (p.OPCODE == "0");
            OpcodeValid |= (p.OPCODE == "1");

            if (!OpcodeValid)
            {
                error.AppendLine("Opcode not supported/invalid (" + p.OPCODE + ") in " + p.GetType());
            }
        }

        (fStmt.Parameters["MAPI_IN_ID"] as IDbDataParameter).Value = StringValue(trans.MapiInId);

        (fStmt.Parameters["SEQNUM"] as IDbDataParameter).Value = NumberValue(trans.TransSeq);

        if (p.OPCODE != null)
        {
            if (p.OPCODE.Length > 1)
                error.AppendLine("Value for HandlingUnitStatus_01Doc.OPCODE too long, max 1 chars");

            (fStmt.Parameters["OPCODE"] as IDbDataParameter).Value = p.OPCODE;
        }
        else
            (fStmt.Parameters["OPCODE"] as IDbDataParameter).Value = DBNull.Value;

        if (p.MaterialHandlingSystemId != null)
        {
            if (p.MaterialHandlingSystemId.Length > 35)
                error.AppendLine("Value for HandlingUnitStatus_01Doc.MaterialHandlingSystemId too long, max 35 chars");

            if (p.MaterialHandlingSystemId.Length == 0)
                error.AppendLine("Zero length for mandatory parameter HandlingUnitStatus_01Doc.MaterialHandlingSystemId not allowed");

            (fStmt.Parameters["MaterialHandlingSystemId"] as IDbDataParameter).Value = p.MaterialHandlingSystemId;
        }
        else
            error.AppendLine("Null value for mandatory parameter HandlingUnitStatus_01Doc.MaterialHandlingSystemId not allowed");


        if (p.HandlingUnitId != null)
        {
            if (p.HandlingUnitId.Length > 6)
                error.AppendLine("Value for HandlingUnitStatus_01Doc.HandlingUnitId too long, max 6 chars");

            if (p.HandlingUnitId.Length == 0)
                error.AppendLine("Zero length for mandatory parameter HandlingUnitStatus_01Doc.HandlingUnitId not allowed");

            (fStmt.Parameters["HandlingUnitId"] as IDbDataParameter).Value = p.HandlingUnitId;
        }
        else
            error.AppendLine("Null value for mandatory parameter HandlingUnitStatus_01Doc.HandlingUnitId not allowed");


        if (p.HandlingUnitInStatus != null)
        {
            if (p.HandlingUnitInStatus.Length > 1)
                error.AppendLine("Value for HandlingUnitStatus_01Doc.HandlingUnitInStatus too long, max 1 chars");

            if (p.HandlingUnitInStatus.Length == 0)
                error.AppendLine("Zero length for mandatory parameter HandlingUnitStatus_01Doc.HandlingUnitInStatus not allowed");

            (fStmt.Parameters["HandlingUnitInStatus"] as IDbDataParameter).Value = p.HandlingUnitInStatus;
        }
        else
            error.AppendLine("Null value for mandatory parameter HandlingUnitStatus_01Doc.HandlingUnitInStatus not allowed");


        if (p.HandlingUnitOutStatus != null)
        {
            if (p.HandlingUnitOutStatus.Length > 1)
                error.AppendLine("Value for HandlingUnitStatus_01Doc.HandlingUnitOutStatus too long, max 1 chars");

            if (p.HandlingUnitOutStatus.Length == 0)
                error.AppendLine("Zero length for mandatory parameter HandlingUnitStatus_01Doc.HandlingUnitOutStatus not allowed");

            (fStmt.Parameters["HandlingUnitOutStatus"] as IDbDataParameter).Value = p.HandlingUnitOutStatus;
        }
        else
            error.AppendLine("Null value for mandatory parameter HandlingUnitStatus_01Doc.HandlingUnitOutStatus not allowed");


        if (error.Length > 0)
        {
            throw (new Exception(error.ToString()));
        }

        trans.TransSeq++;

        fStmt.ExecuteNonQuery();

      }
  }

}
