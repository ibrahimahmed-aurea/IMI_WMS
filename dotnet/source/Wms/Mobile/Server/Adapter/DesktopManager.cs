using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Imi.Wms.Mobile.Server
{
    [Flags]
    public enum AccessRights : uint
    {
        DESKTOP_READOBJECTS = 0x00000001,
        DESKTOP_CREATEWINDOW = 0x00000002,
        DESKTOP_CREATEMENU = 0x00000004,
        DESKTOP_HOOKCONTROL = 0x00000008,
        DESKTOP_JOURNALRECORD = 0x00000010,
        DESKTOP_JOURNALPLAYBACK = 0x00000020,
        DESKTOP_ENUMERATE = 0x00000040,
        DESKTOP_WRITEOBJECTS = 0x00000080,
        DESKTOP_SWITCHDESKTOP = 0x00000100,

        GENERIC_ALL = (DESKTOP_READOBJECTS | DESKTOP_CREATEWINDOW | DESKTOP_CREATEMENU |
            DESKTOP_HOOKCONTROL | DESKTOP_JOURNALRECORD | DESKTOP_JOURNALPLAYBACK |
            DESKTOP_ENUMERATE | DESKTOP_WRITEOBJECTS | DESKTOP_SWITCHDESKTOP)
    };
         
    public class DesktopManager : IDisposable
    {
        [DllImport("user32.dll", SetLastError=true)]
        private static extern IntPtr CreateDesktopEx(string lpszDesktop,
            IntPtr lpszDevice,
            IntPtr pDevmode,
            int dwFlags,
            uint dwDesiredAccess,
            IntPtr lpsa,
            ulong uHeapSize,
            IntPtr pVoid);
                
        [DllImport("user32.dll", SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseDesktop(IntPtr handle);
        
        private bool _isDisposed;
        private IntPtr _desktop;

        ~DesktopManager()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Initialize(ulong heapSize)
        {
            DesktopName = string.Format("IMI_MOBILE_{0}", Guid.NewGuid().ToString());

            _desktop = CreateDesktopEx(DesktopName, IntPtr.Zero, IntPtr.Zero, 0, (uint)AccessRights.GENERIC_ALL, IntPtr.Zero, heapSize, IntPtr.Zero);
            
            if (_desktop == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public string DesktopName { get; private set; }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (_desktop != null)
                {
                    CloseDesktop(_desktop);
                }
            }

            _isDisposed = true;
        }
    }
}
