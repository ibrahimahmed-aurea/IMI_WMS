using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public class ProgressView : IDisposable
    {
        private Thread progressThread;
        private Dispatcher dispatcher;
        private ProgressWindow window;
        private EventWaitHandle sync;
        private bool disposed;

        public ProgressView()
        {
            sync = new EventWaitHandle(false, EventResetMode.AutoReset);

            progressThread = new Thread(() =>
            {
                window = new ProgressWindow();
                dispatcher = window.Dispatcher;
                sync.Set();
                Dispatcher.Run();
            }
            );

            progressThread.SetApartmentState(ApartmentState.STA);
            progressThread.Start();

            sync.WaitOne();
        }

        ~ProgressView()
        {
            Dispose(false);
        }

        public void SetBounds(double left, double top, double width, double height)
        {
            dispatcher.Invoke(new Action(delegate()
            {
                window.Left = left;
                window.Top = top;
                window.Width = width;
                window.Height = height;
            }));
        }

        public void Show()
        {
            dispatcher.Invoke(new Action(delegate()
            {
                window.Show();
            }));
        }

        public void Hide()
        {
            dispatcher.Invoke(new Action(delegate()
            {
                window.Hide();
            }));
        }

        public void Close()
        {
            Dispose();
        }
                        
        public void Dispose()
        {
            if (!disposed)
                Dispose(true);

            disposed = true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    try
                    {
                        dispatcher.Invoke(new Action(delegate()
                        {
                            window.Close();
                        }));
                    }
                    finally
                    {
                        dispatcher.InvokeShutdown();
                    }
                }
                finally
                {
                    sync.Close();
                }
            }
        }
    }

}
