using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;

namespace Imi.Framework.DataAccess
{
    public enum SortDirections
    {
        None,
        Ascending,
        Descending
    }

    public struct SortParameter
    {
        public string PropertyName {get; set;}
        public SortDirections SortDirection { get; set; }

        public static List<SortParameter> GetListOfSortParameter(object otherList)
        {
            List<SortParameter> resultList = null;

            if (otherList != null && otherList.GetType().Name == typeof(List<object>).Name)
            {
                if (otherList.GetType().GetGenericArguments()[0].FullName == "Imi.Framework.Services.SortParameter")
                {
                    resultList = new List<SortParameter>();

                    foreach (object item in (System.Collections.ICollection)otherList)
                    {
                        Type itemType = item.GetType();

                        if (itemType.Name == typeof(SortParameter).Name)
                        {
                            SortParameter newSortParameter = new SortParameter();

                            newSortParameter.PropertyName = (string)itemType.GetProperty("PropertyName").GetValue(item, null);
                            newSortParameter.SortDirection = (SortDirections)Enum.Parse(typeof(SortDirections), Convert.ToString(itemType.GetProperty("SortDirection").GetValue(item, null)));

                            resultList.Add(newSortParameter);
                        }
                    }
                }
                
            }

            return resultList;
        }
    }

    public class DataPartitioningContainer
    {
        public static int PARTITION_SIZE = 10000;

        private List<object> _partitions = new List<object>();
        private bool _clientThreadActive = false;

        public string Id { get; set; }

        public bool Abort { get; set; }

        public bool DataFetchFinished { get; set; }

        public object SearchParameters { get; set; }

        public List<SortParameter> SortParameters { get; set; }

        public int NumberOfPartitons
        {
            get { return _partitions.Count; }
        }

        public int TotalDataRowsCount { get; set; }

        public bool CountsRows { get; set; }

        public Exception ExceptionToThrow { get; set; }

        public DataPartitioningContainer(string id)
        {
            this.Id = id;
        }

        public object PullFirstPartition()
        {
            while (!DataFetchFinished || _partitions.Count > 0)
            {
                _clientThreadActive = true;
                try
                {
                    if (_partitions.Count > 0)
                    {
                        object tmp = _partitions[0];

                        lock (_partitions)
                        {
                            _partitions.RemoveAt(0);
                        }

                        return tmp;
                    }
                }
                finally
                {
                    _clientThreadActive = false;
                }

                Thread.Sleep(30); //Wait for data fetch thread to fetch next partition
            }

            return null;
        }

        public void PushPartition(object partition, bool islast = false)
        {
            while (_clientThreadActive)
            {
                Thread.Sleep(10);
            }

            lock (_partitions)
            {
                _partitions.Add(partition);
                DataFetchFinished = islast;
            }
        }
    }
}
