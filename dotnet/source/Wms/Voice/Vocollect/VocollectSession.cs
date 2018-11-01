using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using Imi.Framework.Messaging.Engine;

namespace Imi.Wms.Voice.Vocollect
{
    /// <summary>
    /// Represents a catch measure entry for a LUM.
    /// </summary>
    public class CatchMeasure
    {
        private double measurement;

        /// <summary>
        /// Lot quantity.
        /// </summary>
        public double Measurement
        {
            get
            {
                return measurement;
            }
            set
            {
                measurement = value;
            }
        }
    }

    /// <summary>
    /// Represents a production lot.
    /// </summary>
    public class ProductionLot
    {
        private string prodlot;
        private double quantity;
    
        /// <summary>
        /// Lot quantity.
        /// </summary>
        public double Quantity
        {
            get
            {
                return quantity;
            }
            set
            {
                quantity = value;
            }
        }
	    
        /// <summary>
        /// Lot identity.
        /// </summary>
        public string LotId
        {
            get 
            { 
                return prodlot; 
            }
            set
            { 
                prodlot = value;
            }
        }
    }

    /// <summary>
    /// Represents a voice terminal session.
    /// </summary>
    public class VocollectSession : PropertyCollection, IDisposable
    {
        private PropertyCollection currentAssignmentData;
        private MultiPartMessage lastRequestMessage;
        private MultiPartMessage lastResponseMessage;
        private DateTime timeStamp;
        private Collection<ProductionLot> productionLotCollection;
        private Collection<CatchMeasure> catchMeasureCollection;
        private bool successful;
        private EventWaitHandle lockWaitEvent;
        private EventWaitHandle threadAbortWaitEvent;
        private EventWaitHandle[] lockWaitArray;
        private bool disposed = false;
                                	                              	
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="VocollectSession"/> class.</para>
        /// </summary>
        public VocollectSession()
        {
            lockWaitEvent = new EventWaitHandle(true, EventResetMode.ManualReset);
            threadAbortWaitEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
            lockWaitArray = new EventWaitHandle[2];
            lockWaitArray[0] = lockWaitEvent;
            lockWaitArray[1] = threadAbortWaitEvent;

            Clear();
        }

        ~VocollectSession()
        {
            Dispose(false);
        }

        /// <summary>
        /// Returns a collection of registered production lots for the current pick line.
        /// </summary>
        public Collection<ProductionLot> ProductionLots
        {
            get
            {
                return productionLotCollection;
            }
        }
                        
        /// <summary>
        /// Returns a collection of measurements captured per LUM.
        /// </summary>
        public Collection<CatchMeasure> CatchMeasureEntries
        {
            get
            {
                return catchMeasureCollection;
            }
        }

        public bool LastOperationSuccessful
        {
            get 
            { 
                return successful; 
            }
            set
            { 
                successful = value; 
            }
        }
        
        public PropertyCollection CurrentAssignmentData
        {
            get
            {
                return currentAssignmentData;
            }
            set
            {
                currentAssignmentData = value;
            }
        }
        
        public override void Clear()
        {
            currentAssignmentData = null;
            timeStamp = DateTime.MinValue;
            lastRequestMessage = null;
            lastResponseMessage = null;
            productionLotCollection = new Collection<ProductionLot>();
            catchMeasureCollection = new Collection<CatchMeasure>();
            lockWaitEvent.Set();
            threadAbortWaitEvent.Reset();
            base.Clear();
        }

        /// <summary>
        /// Aquires a lock for the session to ensure that only one thread is accessing it at once.
        /// This method will return false if a lock is already held.
        /// </summary>
        /// <param name="millisecondsTimeout">Timeout in milliseconds.</param>
        /// <returns>True if the lock was acquired, otherwise false.</returns>
        public bool AcquireLock(int millisecondsTimeout)
        {
            if (disposed)
                throw new ObjectDisposedException("VocollectSession");
                        
            threadAbortWaitEvent.Set();
            threadAbortWaitEvent.Reset();
                                    
            switch (EventWaitHandle.WaitAny(lockWaitArray, millisecondsTimeout, false))
            { 
                case EventWaitHandle.WaitTimeout:
                    return false;

                case 0:
                    
                    if (disposed)
                        throw new ObjectDisposedException("VocollectSession");
                    
                    lockWaitEvent.Reset();
                    
                    return true;

                case 1:
                    return false;
            }

            return false;

        }

        /// <summary>
        /// Releases the lock so that it can be acquired by a new thread.
        /// </summary>
        /// <param name="successful">True if the request was successful.</param>
        public void ReleaseLock(bool successful)
        {
            if (disposed)
                throw new ObjectDisposedException("VocollectSession");

            this.successful = successful;
            lockWaitEvent.Set();
        }
        
        public MultiPartMessage LastRequestMessage
        {
            get
            {
                return lastRequestMessage;
            }
            set
            {
                lastRequestMessage = value;
            }
        }

        public MultiPartMessage LastResponseMessage
        {
            get
            {
                return lastResponseMessage;
            }
            set
            {
                lastResponseMessage = value;
            }
        }
        
        /// <summary>
        /// Returns the time stamp of the last received message.
        /// </summary>
        public DateTime LastTimeStamp
        {
            get
            {
                return timeStamp;
            }
            set
            {
                timeStamp = value;
            }
        }

        #region IDisposable Members

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                try
                {
                    if (disposing)
                    {
                        Clear();
                    }
                }
                finally
                {
                    try
                    {
                        try
                        {
                            lockWaitEvent.Close();
                        }
                        finally
                        {
                            threadAbortWaitEvent.Close();
                        }
                    }
                    finally
                    {
                        disposed = true;
                    }
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
