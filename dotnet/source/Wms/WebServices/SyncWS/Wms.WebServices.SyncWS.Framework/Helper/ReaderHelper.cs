using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Imi.Wms.WebServices.SyncWS.Framework
{
    public class ReaderHelper
    {
        public static string GetString(IDataReader reader, int i)
        {
            if (reader.IsDBNull(i))
                return null;
            else
            {
                try
                {
                    return reader.GetString(i);
                }
                catch (Exception e)
                {
                    Exception ne = new Exception(
                        "GetString: Error reading database field number " + reader[i].ToString() +
                        ", it is of type " + reader[i].GetType().ToString(), e);
                    throw ne;
                }
            }
        }
        public static bool GetBoolNotNull(IDataReader reader, int i)
        {
            Nullable<bool> value = GetBool(reader, i);
            if (value != null)
                return (bool)value;
            else
                return false; // default
        }
        public static Nullable<bool> GetBool(IDataReader reader, int i)
        {
            if (reader.IsDBNull(i))
                return null;
            else
            {
                try
                {
                    return (reader.GetString(i) == "Y");
                }
                catch (Exception e)
                {
                    Exception ne = new Exception(
                        "GetBool: Error reading database field number " + reader[i].ToString() +
                        ", it is of type " + reader[i].GetType().ToString(), e);
                    throw ne;
                }
            }
        }
        public static int GetInt32NotNull(IDataReader reader, int i)
        {
            Nullable<int> value = GetInt32(reader, i);
            if (value != null)
                return (Int32)value;
            else
                return 0; // default
        }
        public static Nullable<int> GetInt32(IDataReader reader, int i)
        {
            if (reader.IsDBNull(i))
                return null;
            else
            {
                try
                {
                    return Convert.ToInt32(reader.GetDecimal(i));
                }
                catch (Exception e)
                {
                    Exception ne = new Exception(
                        "GetInt32: Error reading database field number " + reader[i].ToString() +
                        ", it is of type " + reader[i].GetType().ToString(), e);
                    throw ne;
                }
            }
        }
        public static decimal GetDecimalNotNull(IDataReader reader, int i)
        {
            Nullable<decimal> value = GetDecimal(reader, i);
            if (value != null)
                return (decimal)value;
            else
                return 0; // default
        }
        public static Nullable<decimal> GetDecimal(IDataReader reader, int i)
        {
            if (reader.IsDBNull(i))
                return null;
            else
            {
                try
                {
                    return reader.GetDecimal(i);
                }
                catch (Exception e)
                {
                    Exception ne = new Exception(
                        "GetDecimal: Error reading database field number " + reader[i].ToString() +
                        ", it is of type " + reader[i].GetType().ToString(), e);
                    throw ne;
                }
            }
        }
        public static Nullable<DateTime> GetDateTime(IDataReader reader, int i)
        {
            if (reader.IsDBNull(i))
                return null;
            else
            {
                try
                {
                    return reader.GetDateTime(i);
                }
                catch (Exception e)
                {
                    Exception ne = new Exception(
                        "GetString: Error reading database field number " + reader[i].ToString() +
                        ", it is of type " + reader[i].GetType().ToString(), e);
                    throw ne;
                }
            }
        }
        public static ArrayList Read(
            IDataReader reader, 
            IDataReadble readble, 
            Nullable<int> skipNoFirstRows, 
            Nullable<int> maxRows, 
            out int totalRows)
        {
            ArrayList xList = new ArrayList();

            totalRows = 0;
            int readRows = 0;

            while (reader.Read())
            {
                bool doRead = true;

                if (skipNoFirstRows != null)
                    if (totalRows < skipNoFirstRows) // both are zero-based
                        doRead = false;


                if (maxRows != null)
                    if (readRows >= maxRows) // the quota was reached the lap before
                        doRead = false;

                if (doRead)
                {
                    xList.Add(readble.Read(reader));
                    readRows++;
                }

                totalRows++;
            }

            reader.Close();

            return xList;
        }
        public static ArrayList MakeTestData(int count, IDataReadble readble)
        {
            ArrayList xList = new ArrayList();

            for (int i = 0; i < count; i++)
            {
                xList.Add(readble.MakeTestData());
            } 
            return xList;
        }
    }
}
