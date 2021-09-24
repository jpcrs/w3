using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using w3.Desktop;
using w3.Model;
using w3.Window;

namespace w3
{
    public class InterceptKeys
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private static LowLevelKeyboardProc _proc = HookCallback;
        public static IntPtr _hookID = IntPtr.Zero;
        private static bool WinDown = false;
        private static WindowList windowList = new WindowList();
        private static VirtualDesktopManager manager = new();

        public static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {

            if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if ((Keys)vkCode == Keys.LWin)
                {
                    WinDown = false;
                }
            }
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if ((Keys)vkCode == Keys.LWin)
                {
                    WinDown = true;
                }

                var windows = windowList.GetWindows().OrderBy(x => x.Handle).ToList();

                if (vkCode >= 48 && vkCode <= 57)
                {
                    if (Convert.ToBoolean(Win32.GetKeyState(16) & 0x8000))
                    {
                        manager.MoveWindowToDesktop(vkCode == 48 ? 9 : vkCode-49);
                        return (IntPtr)1;
                    }
                    manager.GoToDesktop(vkCode == 48 ? 9 : vkCode-49);
                    var windowToFocus = windowList.GetWindows().FirstOrDefault();
                    Win32.SetForegroundWindow(windowToFocus?.Handle ?? Win32.GetForegroundWindow());
                    return (IntPtr)1;
                }

                if (Keys.L == (Keys)vkCode && WinDown)
                {
                    var next = (windows.FindIndex(x => x.Handle == Win32.GetForegroundWindow())+1) % windows.Count;
                    Windowplacement placement = new Windowplacement();
                    Win32.GetWindowPlacement(windows[next].Handle, ref placement);
                    if (placement.showCmd == 2)
                    {
                        //the window is hidden so we restore it
                        Win32.ShowWindow(windows[next].Handle, ShowWindowEnum.Restore);
                    }
                    Win32.SetForegroundWindow(windows[next].Handle);
                    return (IntPtr)1;
                }
                if (Keys.O == (Keys)vkCode && WinDown)
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/C \"tools\\TurnLockOnOff.bat\"",
                        Verb = "runas"
                    };

                    Process.Start(startInfo)?.WaitForExit();
                }
                if (Keys.H == (Keys)vkCode && WinDown)
                {
                    var prev = windows.FindIndex(x => x.Handle == Win32.GetForegroundWindow()) - 1 < 0 
                        ? windows.Count - 1
                        : windows.FindIndex(x => x.Handle == Win32.GetForegroundWindow()) - 1;

                    Windowplacement placement = new Windowplacement();
                    Win32.GetWindowPlacement(windows[prev].Handle, ref placement);
                    if (placement.showCmd == 2)
                    {
                        //the window is hidden so we restore it
                        Win32.ShowWindow(windows[prev].Handle, ShowWindowEnum.Restore);
                    }
                    Win32.SetForegroundWindow(windows[prev].Handle);
                    return (IntPtr)1;
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
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
