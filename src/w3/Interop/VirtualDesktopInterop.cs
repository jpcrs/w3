using System;
using System.Runtime.InteropServices;

namespace w3.Interop
{
    public class VirtualDesktopInterop
    {
        [DllImport("tools/VirtualDesktopAccessor.dll")]
        public static extern int GetDesktopCount();

        [DllImport("tools/VirtualDesktopAccessor.dll")]
        public static extern Guid GetDesktopIdByNumber(int number);

        [DllImport("tools/VirtualDesktopAccessor.dll")]
        public static extern void GoToDesktopNumber(int number);

        [DllImport("tools/VirtualDesktopAccessor.dll")]
        public static extern void PinWindow(IntPtr hwnd);

        [DllImport("tools/VirtualDesktopAccessor.dll")]
        public static extern void UnPinWindow(IntPtr hwnd);

        [DllImport("tools/VirtualDesktopAccessor.dll")]
        public static extern void PinApp(IntPtr hwnd);

        [DllImport("tools/VirtualDesktopAccessor.dll")]
        public static extern void UnPinApp(IntPtr hwnd);

        [DllImport("tools/VirtualDesktopAccessor.dll")]
        public static extern bool MoveWindowToDesktopNumber(IntPtr window, int number);

        [DllImport("tools/VirtualDesktopAccessor.dll")]
        public static extern IntPtr ViewGetFocused();

        [DllImport("tools/VirtualDesktopAccessor.dll")]
        public static extern int GetCurrentDesktopNumber();

        [DllImport("tools/VirtualDesktopAccessor.dll")]
        public static extern int GetWindowDesktopNumber(IntPtr hwnd);
    }
}
