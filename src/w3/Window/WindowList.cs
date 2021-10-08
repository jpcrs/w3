using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using w3.Interop;
using w3.Model;

namespace w3.Window
{
    public class WindowList
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr hWndChildAfter, string className,  string windowTitle);

        private bool IsCloacked(IntPtr hwnd)
        {
            Win32.DwmGetWindowAttribute(hwnd, (int)DwmWindowAttribute.DWMWA_CLOAKED, out bool isCloacked, Marshal.SizeOf(typeof(bool)));
            return isCloacked;
        }

        private bool IsAltTabWindow(IntPtr hwnd)
        {
            var hwndWalk = Win32.GetAncestor(hwnd, 3);

            IntPtr hwndTry;
            while ((hwndTry = Win32.GetLastActivePopup(hwndWalk)) != hwndTry)
            {
                if (Win32.IsWindowVisible(hwndTry)) break;
                hwndWalk = hwndTry;
            }
            return hwndWalk == hwnd;
        }

        private bool HasSomeExtendedWindowsStyles(IntPtr hwnd)
        {
            const int GWL_EXSTYLE = -20;
            const uint WS_EX_TOOLWINDOW = 0x00000080U;

            uint i = Win32.GetWindowLong(hwnd, GWL_EXSTYLE);
            if ((i & (WS_EX_TOOLWINDOW)) != 0)
            {
                return true;
            }

            return false;
        }

        public List<DetectedWindow> GetWindows()
        {
            var shellWindow = Win32.GetShellWindow();
            var windows = new List<DetectedWindow>();
            Win32.EnumWindows(delegate (IntPtr handle, IntPtr lParam)
            {
                if (handle == shellWindow)
                    return true;

                if (!Win32.IsWindowVisible(handle))
                    return true;

                //if (Win32.IsIconic(handle))
                //    return true;

                if (IsCloacked(handle))
                    return true;

                if (HasSomeExtendedWindowsStyles(handle))
                    return true;

                if (!IsAltTabWindow(handle))
                    return true;

                var length = Win32.GetWindowTextLength(handle);

                if (length == 0)
                    return true;

                var builder = new StringBuilder(length);

                Win32.GetWindowText(handle, builder, length + 1);
                Win32.GetWindowRect(handle, out Rect rect);
                windows.Add(new DetectedWindow(handle, rect, builder.ToString()));

                return true;
            }, IntPtr.Zero);

            return windows;
        }

        public IntPtr GetShellTray()
        {
            return Win32.FindWindow("Shell_TrayWnd", null);
        }

        public IntPtr GetTaskBar()
        {
            var shellTray = GetShellTray();
            var hWndRebar = FindWindowEx(shellTray, (IntPtr)0, "ReBarWindow32", null);
            var hWndMSTaskSwWClass = FindWindowEx(hWndRebar, (IntPtr)0, "MSTaskSwWClass", null);
            var hWndMSTaskListWClass = FindWindowEx(hWndMSTaskSwWClass, (IntPtr)0, "MSTaskListWClass", null);
            return hWndMSTaskListWClass;
        }
    }
}
