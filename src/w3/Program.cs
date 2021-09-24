using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using w3;
using w3.Desktop;
using w3.Window;


var windowList = new WindowList();
var windows = windowList.GetWindows();

foreach (var win in windows)
{
    Console.WriteLine($"{win.Handle} - {win.Name}");
}

var edgePtr = windows.FirstOrDefault(x => x.Name.Contains("Edge"))?.Handle ?? IntPtr.Zero;
var manager = new VirtualDesktopManager();
manager.InitDesktops();
manager.GoToDesktop(1);
var desktopIdentifier = DesktopManager.GetDesktopIdByNumber(1);
Console.WriteLine("dll" + desktopIdentifier);

InterceptKeys._hookID = InterceptKeys.SetHook(InterceptKeys.HookCallback);
Application.Run();
InterceptKeys.UnhookWindowsHookEx(InterceptKeys._hookID);

Console.ReadLine();
