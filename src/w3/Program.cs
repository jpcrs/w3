using System;
using w3;

var windowList = new WindowList();
var windows = windowList.GetWindows();
foreach (var win in windows)
{
    Console.WriteLine($"{win.Handle} - {win.Name}");
}

//SetForegroundWindow(windows.FirstOrDefault(x => x.Name.Contains("Edge"))?.Handle ?? IntPtr.Zero);
//await Task.Delay(5000);
//SetForegroundWindow(windows.FirstOrDefault(x => x.Name.Contains("Telegram"))?.Handle ?? IntPtr.Zero);
