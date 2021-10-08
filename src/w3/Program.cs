using System;
using System.Linq;
using System.Windows.Forms;
using w3;
using w3.Desktop;
using w3.Hotkeys;
using w3.Window;

var virtualDesktopManager = new DesktopManager(new WindowList());
virtualDesktopManager.InitWorkspaces();

var windowManager = new WindowManager();
windowManager.SetWindowVisibility(false);

var configParser = new ConfigParser(virtualDesktopManager);

var keyInterceptor = new KeyInterceptor(configParser);
Application.Run();
KeyInterceptor.UnhookWindowsHookEx(keyInterceptor.HookId);
