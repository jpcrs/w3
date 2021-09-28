using System.Windows.Forms;
using w3;
using w3.Desktop;

var virtualDesktopManager = new DesktopManager();
virtualDesktopManager.InitDesktops();

var windowManager = new WindowManager();
windowManager.SetWindowVisibility(false);

var keyInterceptor = new KeyInterceptor(virtualDesktopManager, new w3.Window.WindowList());
Application.Run();
KeyInterceptor.UnhookWindowsHookEx(keyInterceptor.HookId);
