using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace w3.Desktop
{
    public class DesktopManager
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
    }
}
