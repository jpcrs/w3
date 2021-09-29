using System.Windows.Forms;
using w3;
using w3.Desktop;
using w3.Window;

var virtualDesktopManager = new DesktopManager(new WindowList());
virtualDesktopManager.InitWorkspaces();

var windowManager = new WindowManager();
windowManager.SetWindowVisibility(false);

var keyInterceptor = new KeyInterceptor(virtualDesktopManager);
Application.Run();
KeyInterceptor.UnhookWindowsHookEx(keyInterceptor.HookId);
