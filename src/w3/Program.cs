using System;
using System.Linq;
using System.Windows.Forms;
using w3;
using w3.Desktop;
using w3.Graph;
using w3.Hotkeys;
using w3.Interop;
using w3.Window;

var windowList = new WindowList();
var bar = new Bar(windowList);
var virtualDesktopManager = new DesktopManager(bar.Form, windowList);
virtualDesktopManager.InitWorkspaces();

var windowManager = new WindowManager();
windowManager.SetWindowVisibility(false);

var configParser = new ConfigParser(virtualDesktopManager);

var keyInterceptor = new KeyInterceptor(configParser);

Application.EnableVisualStyles();
Application.Run(bar.Form);
KeyInterceptor.UnhookWindowsHookEx(keyInterceptor.HookId);
