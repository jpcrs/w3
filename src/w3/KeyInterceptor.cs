using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using w3.Desktop;
using w3.Interop;
using w3.Model;
using w3.Window;

namespace w3
{
    public class KeyInterceptor
    {
        public readonly IntPtr HookId;
        private readonly WindowList _windowList;
        private readonly DesktopManager _manager;
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public KeyInterceptor(DesktopManager manager, WindowList windowList)
        {
            _manager = manager;
            _windowList = windowList;
            HookId = SetHook(HookCallback);
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(Consts.WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }


        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)Consts.WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                var windows = _windowList.GetWindows().OrderBy(x => x.Handle).ToList();
                if (!Convert.ToBoolean(Win32.GetKeyState(91) & 0x8000))
                {
                    return IntPtr.Zero;
                }

                if (vkCode >= 48 && vkCode <= 57)
                {
                    if (Convert.ToBoolean(Win32.GetKeyState(16) & 0x8000))
                    {
                        _manager.MoveWindowToDesktop(vkCode == 48 ? 9 : vkCode-49);
                        return (IntPtr)1;
                    }
                    _manager.GoToDesktop(vkCode == 48 ? 9 : vkCode-49);
                    var windowToFocus = _windowList.GetWindows().FirstOrDefault();
                    Win32.SetForegroundWindow(windowToFocus?.Handle ?? Win32.GetForegroundWindow());
                    return (IntPtr)1;
                }

                if (Keys.L == (Keys)vkCode)
                {
                    var next = (windows.FindIndex(x => x.Handle == Win32.GetForegroundWindow())+1) % windows.Count;
                    var placement = new Windowplacement();
                    Win32.GetWindowPlacement(windows[next].Handle, ref placement);
                    if (placement.showCmd == 2)
                    {
                        //the window is hidden so we restore it
                        Win32.ShowWindow(windows[next].Handle, ShowWindowEnum.Restore);
                    }
                    Thread.Sleep(20);
                    Win32.SetForegroundWindow(windows[next].Handle);
                    return (IntPtr)1;
                }
                if (Keys.H == (Keys)vkCode)
                {
                    var prev = windows.FindIndex(x => x.Handle == Win32.GetForegroundWindow()) - 1 < 0 
                        ? windows.Count - 1
                        : windows.FindIndex(x => x.Handle == Win32.GetForegroundWindow()) - 1;

                    var placement = new Windowplacement();
                    Win32.GetWindowPlacement(windows[prev].Handle, ref placement);
                    if (placement.showCmd == 2)
                    {
                        //the window is hidden so we restore it
                        Win32.ShowWindow(windows[prev].Handle, ShowWindowEnum.Restore);
                    }
                    Thread.Sleep(20);
                    Win32.SetForegroundWindow(windows[prev].Handle);
                    return (IntPtr)1;
                }
                if (Keys.O == (Keys)vkCode)
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/C \"tools\\TurnLockOnOff.bat\"",
                        Verb = "runas"
                    };

                    Process.Start(startInfo)?.WaitForExit();
                }
            }
            return CallNextHookEx(HookId, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
