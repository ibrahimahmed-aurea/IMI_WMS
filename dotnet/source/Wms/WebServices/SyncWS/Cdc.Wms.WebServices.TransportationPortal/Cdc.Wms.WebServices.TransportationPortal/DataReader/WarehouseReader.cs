using System;
using System.Data;
using System.Collections;
using Imi.Framework.Job.Data;

using Imi.Wms.WebServices.SyncWS.Framework;
using Cdc.Wms.WebServices.TransportationPortal;

namespace Cdc.Wms.WebServices.TransportationPortal.Entities
{
    public partial class Route : IDataReadble
    {
        /*
         */
        IDataReadble IDataReadble.Read(IDataReader reader)
        {
            Route x = new Route();
            int i = 0;

            string DepartureIdentity = ReaderHelper.GetString(reader, i++);
            x.VehicleIdentity = ReaderHelper.GetString(reader, i++);
            Nullable<DateTime> dtm = ReaderHelper.GetDateTime(reader, i++);
            if (dtm != null)
                x.EstimatedTimeOfDeparture = (DateTime)dtm;
            else
                x.EstimatedTimeOfDeparture = DateTime.MinValue;
            x.RouteIdentity = ReaderHelper.GetString(reader, i++);
            x.Name = ReaderHelper.GetString(reader, i++);
            x.TotalUnloadingTimeInSeconds = ReaderHelper.GetInt32(reader, i++);
            x.TotalDrivingTimeInSeconds = ReaderHelper.GetInt32(reader, i++);
            x.TotalTimeInSeconds = ReaderHelper.GetInt32(reader, i++);
            x.TotalDistance = (double?)ReaderHelper.GetDecimal(reader, i++);

            return x;
        }

        IDataReadble IDataReadble.MakeTestData()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public partial class RouteStop : IDataReadble
    {
        /*
            type WarehouseRow_Type is record
            ( stockNo           WH.WHID%type
            ,name               WH.WHNAME%type );
         */
        IDataReadble IDataReadble.Read(IDataReader reader)
        {
            RouteStop x = new RouteStop();
            int i = 0;

            x.StopSequence = ReaderHelper.GetInt32(reader, i++);
            x.DistanceFromPreviousStop = ReaderHelper.GetInt32(reader, i++);
            Nullable<DateTime> dtm = ReaderHelper.GetDateTime(reader, i++);
            if (dtm != null)
                x.EstimatedArrivalTime = (DateTime)dtm;
            else
                x.EstimatedArrivalTime = DateTime.MinValue;
            x.Instructions = ReaderHelper.GetString(reader, i++);
            x.TotalUnloadingTimeInSeconds = ReaderHelper.GetInt32(reader, i++);
            x.TotalDrivingTimeInSeconds = ReaderHelper.GetInt32(reader, i++);
            x.TotalTimeInSeconds = ReaderHelper.GetInt32(reader, i++);
            x.Country = ReaderHelper.GetString(reader, i++);
            x.NodeIdentity = ReaderHelper.GetString(reader, i++);
            x.CustomerIdentity = ReaderHelper.GetString(reader, i++);
            x.Name1 = ReaderHelper.GetString(reader, i++);
            x.Name2 = ReaderHelper.GetString(reader, i++);
            x.Name3 = ReaderHelper.GetString(reader, i++);
            x.Name4 = ReaderHelper.GetString(reader, i++);
            x.Name5 = ReaderHelper.GetString(reader, i++);
            x.Address1 = ReaderHelper.GetString(reader, i++);
            x.Address2 = ReaderHelper.GetString(reader, i++);
            x.Address3 = ReaderHelper.GetString(reader, i++);
            x.Address4 = ReaderHelper.GetString(reader, i++);
            x.City = ReaderHelper.GetString(reader, i++);
            x.ZipCode = ReaderHelper.GetString(reader, i++);
            x.CountryCode = ReaderHelper.GetString(reader, i++);
            x.Region = ReaderHelper.GetString(reader, i++);
            x.Latitude = ReaderHelper.GetString(reader, i++);
            x.Longitude = ReaderHelper.GetString(reader, i++);
            x.ContactPerson = ReaderHelper.GetString(reader, i++);
            x.ContactPhone = ReaderHelper.GetString(reader, i++);
            x.ContactEmail = ReaderHelper.GetString(reader, i++);

            return x;
        }

        IDataReadble IDataReadble.MakeTestData()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    public partial class RouteSearchParameters : IDataReadble
    {
        #region IReadble Members

        public IDataReadble Read(IDataReader reader)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IDataReadble MakeTestData()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }

}
