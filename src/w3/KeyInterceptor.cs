using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using w3.Desktop;
using w3.Interop;

namespace w3
{
    public class KeyInterceptor
    {
        public readonly IntPtr HookId;
        private readonly DesktopManager _manager;
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public KeyInterceptor(DesktopManager manager)
        {
            _manager = manager;
            HookId = SetHook(HookCallback);
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using var curProcess = Process.GetCurrentProcess();
            using var curModule = curProcess.MainModule!;
            return SetWindowsHookEx(Consts.WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName!), 0);
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)Consts.WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (!Convert.ToBoolean(Win32.GetKeyState(91) & 0x8000))
                {
                    return IntPtr.Zero;
                }

                if (vkCode >= 48 && vkCode <= 57)
                {
                    if (Convert.ToBoolean(Win32.GetKeyState(16) & 0x8000))
                    {
                        _manager.MoveWindowToWorkspace(vkCode == 48 ? 9 : vkCode-49);
                        return (IntPtr)1;
                    }
                    _manager.GoToWorkspace(vkCode == 48 ? 9 : vkCode-49);
                    return (IntPtr)1;
                }

                if (Keys.L == (Keys)vkCode)
                {
                    _manager.FocusOnRightWindow();
                    return (IntPtr)1;
                }
                if (Keys.H == (Keys)vkCode)
                {
                    _manager.FocusOnLeftWindow();
                    return (IntPtr)1;
                }
                if (Keys.O == (Keys)vkCode)
                {
                    DesktopManager.SwapWindowsLockScreen();
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

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
