using System;
using System.Drawing;
using System.Windows.Forms;
using w3;
using w3.Desktop;
using w3.Model;

var manager = new VirtualDesktopManager();
manager.InitDesktops();

var notifyIcon = new NotifyIcon();
var visible = true;
SetConsoleWindowVisibility(false);
notifyIcon.DoubleClick += (s, e) =>
{
    visible = !visible;
    SetConsoleWindowVisibility(visible);
};

notifyIcon.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
notifyIcon.Visible = true;
notifyIcon.Text = Application.ProductName;

var contextMenu = new ContextMenuStrip();
contextMenu.Items.Add("Exit", null, (s, e) => { Application.Exit(); });
notifyIcon.ContextMenuStrip = contextMenu;

InterceptKeys._hookID = InterceptKeys.SetHook(InterceptKeys.HookCallback);
Application.Run();
InterceptKeys.UnhookWindowsHookEx(InterceptKeys._hookID);

static void SetConsoleWindowVisibility(bool visible)
{
    IntPtr hWnd = Win32.FindWindow(null, Console.Title);
    if (hWnd != IntPtr.Zero)
    {
        if (visible) 
            Win32.ShowWindow(hWnd, ShowWindowEnum.Show); //1 = SW_SHOWNORMAL
        else 
            Win32.ShowWindow(hWnd, ShowWindowEnum.Hide); //0 = SW_HIDE
    }
}
