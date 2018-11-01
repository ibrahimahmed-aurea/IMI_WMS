using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.ComponentModel;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Threading;

namespace Imi.Framework.Wpf.Controls
{
    public delegate void QueryCompletedEventHandler(object result, Exception e);
    public delegate void QueryStartedEventHandler();

    public class SearchManager
    {
        private Stack<string> searchStack = new Stack<string>();
        private object lockObject = new object();
        private Timer searchTimer = null;
        private List<DrillDownMenuItem> oldResultItems = null;
        private BackgroundWorker worker;
        public DrillDownMenuItem TopMenuItem { get; set; }
        public event QueryStartedEventHandler SearchStarted;
        public event QueryCompletedEventHandler SearchCompleted;
        public event QueryStartedEventHandler SearchCancelled;
        private bool reset;

        public void Reset()
        {
            reset = true;
        }

        private Timer SearchTimer
        {
            get
            {
                if (searchTimer == null)
                {
                    searchTimer = new Timer();
                    searchTimer.Elapsed += new ElapsedEventHandler(SearchTimerElapsed);
                    searchTimer.Interval = 500;
                    searchTimer.AutoReset = false;
                }

                return searchTimer;
            }
        }

        public void Search(string textToSearch)
        {
            lock (lockObject)
            {
                if ((searchStack.Count == 0) || (!searchStack.Peek().Equals(textToSearch)))
                {
                    searchStack.Push(textToSearch);

                    if (!SearchTimer.Enabled)
                    {
                        SearchTimer.Enabled = true;

                        if (SearchStarted != null)
                        {
                            SearchStarted();
                        }
                    }
                }
            }

        }

        private void SearchTimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Do search
            string textToSearch;

            lock (lockObject)
            {
                if (searchStack.Count > 0)
                {
                    textToSearch = searchStack.Pop();
                    searchStack.Clear();
                }
                else
                {
                    textToSearch = "";
                }

                CancelSearchInternal();

                worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(OnStartSearch);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnSearchCompleted);
                worker.WorkerSupportsCancellation = true;
                worker.RunWorkerAsync(textToSearch);
            }


        }

        private void CancelSearchInternal()
        {
            SearchTimer.Enabled = false;

            if ((worker != null) && (worker.IsBusy))
                worker.CancelAsync();

            searchStack.Clear();
        }

        private void OnSearchCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                if (SearchCompleted != null)
                {
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, SearchCompleted, e.Result, e.Error);
                }
            }
            else
            {
                if (SearchCompleted != null)
                {
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, SearchCompleted, null, e.Error);
                }
            }
        }

        private void OnStartSearch(object sender, DoWorkEventArgs e)
        {
            if (reset)
            {
                reset = false;
                oldResultItems = null;
            }

            string searchText = e.Argument as string;

            List<DrillDownMenuItem> resultItems = new List<DrillDownMenuItem>();

            SearchRecursive(TopMenuItem, searchText, resultItems);

            BackgroundWorker worker = sender as BackgroundWorker;

            // Check if result is same as last time
            if ((worker.CancellationPending) || (HasSameContent(resultItems, oldResultItems)))
            {
                e.Cancel = true;
                oldResultItems = null;
            }
            else
            {
                oldResultItems = resultItems;
                e.Result = resultItems;
            }

        }

        private bool HasSameContent(List<DrillDownMenuItem> newResult, List<DrillDownMenuItem> oldResult)
        {
            if ((oldResult != null) && (newResult.Count() == oldResult.Count()))
            {
                int cnt = 0;

                foreach (DrillDownMenuItem d in newResult)
                {
                    cnt += oldResult.Count(r2 => r2.Id == d.Id);
                }

                return (cnt == newResult.Count());
            }

            return false;
        }

        private int resultListExactMatchIndex = 0;
        private int resultListStartWithMatchIndex = 0;
        private int resultListInMatchIndex = 0;

        private void SearchRecursive(DrillDownMenuItem item, string searchText, IList<DrillDownMenuItem> resultItems, bool recursion = false)
        {
            if (!recursion)
            {
                searchText = searchText.ToLower();
                searchText = searchText.Trim();

                resultListExactMatchIndex = 0;
                resultListStartWithMatchIndex = 0;
                resultListInMatchIndex = 0;
            }

            if (item != null)
            {
                if (!item.IsFolder && !item.IsBackItem)
                {
                    string itemText = item.SearchText.ToLower();
                    int insertIndex = -1;

                    if (itemText == searchText)
                    {
                        insertIndex = resultListExactMatchIndex;
                        resultListExactMatchIndex++;
                        resultListStartWithMatchIndex++;
                        resultListInMatchIndex++;
                    }
                    else if (itemText.StartsWith(searchText))
                    {
                        insertIndex = resultListStartWithMatchIndex;
                        resultListStartWithMatchIndex++;
                        resultListInMatchIndex++;
                    }
                    else if (itemText.Contains(searchText))
                    {
                        insertIndex = resultListInMatchIndex;
                        resultListInMatchIndex++;
                    }

                    if (insertIndex > -1)
                    {
                        if (resultItems.Count(listItem => listItem.Id == item.Id) == 0)
                        {
                            resultItems.Insert(insertIndex, item);
                        }
                    }
                }

                foreach (DrillDownMenuItem child in item.Children)
                    SearchRecursive(child, searchText, resultItems, true);
            }
        }

        public void CancelSearch()
        {
            lock (lockObject)
            {
                CancelSearchInternal();
            }

            if (SearchCancelled != null)
            {
                SearchCancelled();
            }
        }
    }
}
