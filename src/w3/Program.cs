using System;
using System.Linq;
using System.Windows.Forms;
using w3;
using w3.Window;

var windowList = new WindowList();
var windows = windowList.GetWindows();
foreach (var win in windows)
{
    Console.WriteLine($"{win.Handle} - {win.Name}");
}

//SetForegroundWindow(windows.FirstOrDefault(x => x.Name.Contains("Edge"))?.Handle ?? IntPtr.Zero);
//await Task.Delay(5000);
//SetForegroundWindow(windows.FirstOrDefault(x => x.Name.Contains("Telegram"))?.Handle ?? IntPtr.Zero);
var x = new InterceptKeysOld();
InterceptKeysOld._hookID = InterceptKeysOld.SetHook(InterceptKeysOld.HookCallback);
Application.Run();
InterceptKeysOld.UnhookWindowsHookEx(InterceptKeysOld._hookID);

Console.ReadLine();


