using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using w3.Desktop;
using w3.Interop;

namespace w3.Hotkeys
{
    public class KeyInterceptor
    {
        public readonly IntPtr HookId;
        private readonly ConfigParser _configParser;

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public KeyInterceptor(ConfigParser configParser)
        {
            HookId = SetHook(HookCallback);
            _configParser = configParser;
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using var curProcess = Process.GetCurrentProcess();
            using var curModule = curProcess.MainModule!;
            return SetWindowsHookEx(Consts.WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName!), 0);
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {

            if (!(nCode >= 0 && wParam == (IntPtr)Consts.WM_KEYDOWN))
            {
                return IntPtr.Zero;
            }

            int vkCode = Marshal.ReadInt32(lParam);
            var command = "";
            if (Convert.ToBoolean(Win32.GetKeyState(91) & 0x8000))
            {
                command += "$mod";
            }
            if (Convert.ToBoolean(Win32.GetKeyState(16) & 0x8000))
            {
                command += "+shift";
            }
            if (vkCode >= 48 && vkCode <= 57)
            {
                var num = vkCode == 48 ? 0 : (vkCode - 49)+1;
                command += "+"+num.ToString();
            }
            else
            {
                var key = ((Keys)vkCode).ToString();
                command += $"+{key}";
            }

            var executed = _configParser.ExecuteCommand(command);
            if (executed)
            {
                return (IntPtr)1;
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
