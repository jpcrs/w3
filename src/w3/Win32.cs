using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using w3.Model;

namespace w3
{
    public class Win32
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowPlacement(IntPtr hWnd, ref Windowplacement lpwndpl);
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, ShowWindowEnum nCmdShow);
        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hwnd);
        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hwnd);
        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hwnd);
        [DllImport("user32.dll")]
        public static extern IntPtr GetAncestor(IntPtr hwnd, int gaFlags);
        [DllImport("user32.dll")]
        public static extern IntPtr GetLastActivePopup(IntPtr hwnd);
        [DllImport("dwmapi.dll")]
        public static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out bool pvAttribute, int cbAttribute);
        [DllImport("user32.dll")]
        public static extern IntPtr GetShellWindow();
        [DllImport("user32.dll")]
        public static extern int EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsIconic(IntPtr hWnd);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern System.UInt32 GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        public static extern short GetKeyState(int nVirtKey);

        public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);
    }
}
