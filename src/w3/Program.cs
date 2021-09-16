using System;
using System.Diagnostics;
using System.Linq;
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

var desktop = new VirtualDesktopManager();
Console.WriteLine($"DesktopId: {desktop.GetWindowDesktopId(edgePtr)}");

InterceptKeys._hookID = InterceptKeys.SetHook(InterceptKeys.HookCallback);
Application.Run();
InterceptKeys.UnhookWindowsHookEx(InterceptKeys._hookID);

Console.ReadLine();
