using System;
using System.Drawing;
using System.Windows.Forms;
using w3.Interop;
using w3.Model;

namespace w3
{
    public class WindowManager
    {
        private bool _visible = true;

        public WindowManager()
        {
            AddTaskBarIcon();
        }

        private void AddTaskBarIcon()
        {
            var notifyIcon = new NotifyIcon();

            notifyIcon.DoubleClick += (s, e) =>
            {
                _visible = !_visible;
                SetWindowVisibility(_visible);
            };

            notifyIcon.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            notifyIcon.Visible = true;
            notifyIcon.Text = Application.ProductName;

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Exit", null, (s, e) => { Application.Exit(); });
            notifyIcon.ContextMenuStrip = contextMenu;
        }

        public void SetWindowVisibility(bool visible)
        {
            IntPtr hWnd = Win32.FindWindow(null, Console.Title);
            if (hWnd != IntPtr.Zero)
            {
                if (visible) 
                    Win32.ShowWindow(hWnd, ShowWindowEnum.Show);
                else 
                    Win32.ShowWindow(hWnd, ShowWindowEnum.Hide);
            }
        }
    }
}
