using System;

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
}
