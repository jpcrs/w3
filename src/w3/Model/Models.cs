using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace w3.Model
{
    public class DetectedWindow
    {
        public IntPtr Handle { get; private set; }

        public Rect Bounds { get; private set; }

        public string Name { get; private set; }

        public DetectedWindow(IntPtr handle, Rect bounds, string name)
        {
            Handle = handle;
            Bounds = bounds;
            Name = name;
        }
    }

    public struct Windowplacement
    {
        public int length;
        public int flags;
        public int showCmd;
        public Point ptMinPosition;
        public Point ptMaxPosition;
        public Rectangle rcNormalPosition;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }
}
