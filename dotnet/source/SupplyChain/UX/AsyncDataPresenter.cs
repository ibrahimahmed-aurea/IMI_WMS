using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.UX;
using System.ComponentModel;
using Imi.SupplyChain.UX.Views;
using Microsoft.Practices.CompositeUI;
using Imi.Framework.UX.Services;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.ObjectBuilder;
using System.Reflection;
using System.Threading;
using System.Windows.Threading;
using System.Windows;
using Imi.SupplyChain.UX.Infrastructure;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace Imi.SupplyChain.UX
{
    public delegate void ProgressUpdatedDelegate(object state, int progressPercentage);

    //Partitioning variables
    public class PartitioningVariables
    {
        public Imi.Framework.Services.IPartitioningRequest ServiceRequest = null;
        public List<object> PartitionBuffer = new List<object>();
        //public int _lastPartitionNumber = 0;
        public bool DataFetchInProgress = false;
        public bool PreparingNextPartition = false;
        public bool ExportSession = false;
    }

    public class PartitioningSessionsHandler
    {
        private PartitioningVariables[] _partitioningSessions = new PartitioningVariables[2];

        public bool SortingOnServer { get; set; }

        public PartitioningVariables GetSearchSession(bool createNew = false)
        {
            PartitioningVariables searchSession = null;
            //Find free search session
            for (int i = 0; i < _partitioningSessions.Length; i++)
            {
                if (_partitioningSessions[i] != null)
                {
                    if (!_partitioningSessions[i].ExportSession)
                    {
                        searchSession = _partitioningSessions[i];
                        break;
                    }
                }
            }

            if (searchSession == null && createNew)
            {
                for (int i = 0; i < _partitioningSessions.Length; i++)
                {
                    if (_partitioningSessions[i] == null)
                    {
                        _partitioningSessions[i] = new PartitioningVariables();
                        searchSession = _partitioningSessions[i];
                        break;
                    }
                }
            }

            return searchSession;
        }

        public PartitioningVariables GetExportSession()
        {
            PartitioningVariables exportSession = null;
            //Find export session
            for (int i = 0; i < _partitioningSessions.Length; i++)
            {
                if (_partitioningSessions[i] != null)
                {
                    if (_partitioningSessions[i].ExportSession)
                    {
                        exportSession = _partitioningSessions[i];
                        break;
                    }
                }
            }

            return exportSession;
        }

        public List<PartitioningVariables> GetAllSessions()
        {
            List<PartitioningVariables> result = new List<PartitioningVariables>();

            for (int i = 0; i < _partitioningSessions.Length; i++)
            {
                if (_partitioningSessions[i] != null)
                {
                    result.Add(_partitioningSessions[i]);
                }
            }

            return result;
        }

        public bool TryStartExport()
        {
            PartitioningVariables exportSession = GetExportSession();

            if (exportSession == null)
            {
                PartitioningVariables searchSession = GetSearchSession();

                if (searchSession != null)
                {
                    searchSession.ExportSession = true;
                    return true;
                }
            }

            return false;
        }

        public void RemoveExportSession()
        {
            for (int i = 0; i < _partitioningSessions.Length; i++)
            {
                if (_partitioningSessions[i] != null)
                {
                    if (_partitioningSessions[i].ExportSession)
                    {
                        _partitioningSessions[i] = null;
                        break;
                    }
                }
            }
        }
    }

    public class AsyncDataPresenter<TService, TView> : DataPresenter<TView>
        where TView : class
    {
        private bool isCancelled;
        private EventWaitHandle searchWaitEvent;
        private EventWaitHandle cancelPendingWaitEvent;
        private delegate void SearchCompletedDelegate(object data, Exception error);
        private ISearchProgressView progressView;

        #region Partitioning


        private PartitioningSessionsHandler _partitioningSessionHandler = null;

        public PartitioningSessionsHandler PartitioningSessionsHandler
        {
            get
            {
                if (_partitioningSessionHandler == null)
                {
                    _partitioningSessionHandler = new PartitioningSessionsHandler();
                }

                return _partitioningSessionHandler;
            }
        }

        #endregion


        public TService Service
        {
            get;
            set;
        }

        public AsyncDataPresenter(TService serviceInstance)
        {
            this.Service = serviceInstance;
            searchWaitEvent = new EventWaitHandle(true, EventResetMode.AutoReset);
            cancelPendingWaitEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
        }
        
        public virtual void ExecuteSearch(object parameters)
        {
            /* Abort any previous searches that have been queued up */
            cancelPendingWaitEvent.Set();
            cancelPendingWaitEvent.Reset();

            /* Enqueue search for background processing */
            ThreadPool.QueueUserWorkItem(SearchThread, parameters);
        }
                
        public void SearchCompletedCallback(object data, Exception error)
        {
            try
            {
                OnProgressUpdated(null, 100);
            }
            finally
            {
                try
                {
                    if (error == null)
                    {
                        PresentData(data);
                    }
                    else
                    {
                        if (!isCancelled)
                        {
                            bool showLocalError = false;
                            
                            if (View is IErrorHandler)
                            {
                                if (View is IDataView)
                                    showLocalError = ((IDataView)View).IsDetailView;
                                else
                                    showLocalError = true;
                            }
                            
                            bool showError = true;
                                                  
                            if (showLocalError)
                                showError = !(View as IErrorHandler).HandleError(error);
                            
                            if (showError)
                                ShellInteractionService.ShowMessageBox(StringResources.Search_Error, error.Message, error.ToString(), Infrastructure.MessageBoxButton.Ok, Infrastructure.MessageBoxImage.Error);
                        }
                    }
                }
                finally
                {
                    searchWaitEvent.Set();
                }
            }
        }

        protected void UpdateProgress(object state, int progressPercentage)
        {
            Application.Current.Dispatcher.Invoke(new ProgressUpdatedDelegate(OnProgressUpdated), state, progressPercentage);
        }

        private void SearchThread(object state)
        {
            int handle = WaitHandle.WaitAny(new WaitHandle[] {searchWaitEvent, cancelPendingWaitEvent});

            /* No other search in progress, continue execute current */
            if (handle == 0)
            {
                isCancelled = false;
                Exception error = null;
                object data = null;

                try
                {
                    UpdateProgress(null, 0);

                    data = ExecuteSearchAsync(state);
                }
                catch (Exception ex)
                {
                    error = ex;
                }
                finally
                {
                    if (Application.Current != null)
                    {
                        try
                        {
                            Application.Current.Dispatcher.Invoke(new SearchCompletedDelegate(SearchCompletedCallback), data, error);
                        }
                        catch (NullReferenceException)
                        { 
                            /* Application terminated after null check */
                        }
                    }
                }
            }
        }
        
        protected virtual void OnProgressUpdated(object state, int progressPercentage)
        {
            IDataView dataView = View as IDataView;
            
            if (dataView != null)
            {
                if (dataView.IsDetailView)
                {
                    dataView.UpdateProgress(progressPercentage);
                }
                else
                {
                    IWorkspace modalWorkspace = WorkItem.Workspaces["modalWorkspace"];

                    if (progressPercentage < 100)
                    {
                        if (progressView == null)
                        {
                            progressView = WorkItem.SmartParts.AddNew<SearchProgressView>();
                            progressView.CancelCallback = () =>
                            {
                                Cancel();
                            };

                            modalWorkspace.Show(progressView);
                        }
                    }
                    else
                    {
                        modalWorkspace.Close(progressView);
                        WorkItem.SmartParts.Remove(progressView);
                        progressView = null;
                    }
                }
            }
        }

        protected virtual object ExecuteSearchAsync(object parameters)
        {
            return null;
        }

        protected virtual void AbortSearchDataFetch(bool wait = false)
        {
        }

        protected virtual void PresentData(object data)
        {
            if (View is IDataView)
                (View as IDataView).PresentData(data);
        }
                
        public void Cancel()
        {
            if (!isCancelled)
            {
                AbortSearchDataFetch(true);

                IAbortable abortable = Service as IAbortable;

                if (abortable != null)
                {
                    isCancelled = true;

                    try
                    {
                        abortable.Abort();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
               
    }
}
