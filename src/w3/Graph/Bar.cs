using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using w3.Interop;
using w3.Window;

namespace w3.Graph
{
    public class Bar
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public Form Form;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport( "user32.dll", EntryPoint = "GetWindowLongPtr" )]
        public static extern IntPtr GetWindowLongPtr( IntPtr hWnd, GWL nIndex );

        [DllImport( "user32.dll", EntryPoint = "SetWindowLongPtr" )]
        public static extern IntPtr SetWindowLongPtr( IntPtr hWnd, GWL nIndex, IntPtr dwNewLong );

        const long WS_EX_TOPMOST = 0x00000008L;
        private readonly WindowList _windowList;
        private Label _label;
        private readonly Dictionary<int, string> windowsByDesktop = new Dictionary<int, string>();

        public enum GWL : int
        {
            GWL_WNDPROC = (-4),
            GWL_HINSTANCE = (-6),
            GWL_HWNDPARENT = (-8),
            GWL_STYLE = (-16),
            GWL_EXSTYLE = (-20),
            GWL_USERDATA = (-21),
            GWL_ID = (-12)
        }

        public Bar(WindowList windowList)
        {
            var form = new Form();
            form.Height = 30;
            Label label = new Label()
            {
                Width = 300,
                Text = "Biju Joseph, Redmond, WA",
                ForeColor = Color.White,
            };
            _label = label;
            label.MouseDown += new MouseEventHandler(Form1_MouseDown);
            form.Controls.Add(_label);
            form.ShowInTaskbar = false;
            form.TopMost = true;
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.AllowTransparency = true;
            form.BackColor = Color.FromArgb(255, 0, 0, 0);
            form.TransparencyKey = form.BackColor;
            form.Shown += MakeFormInvisible;
            Form = form;
            _windowList = windowList;
            var timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000; // in miliseconds
            timer1.Start();
        }

        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {     
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                _ = SendMessage(Form.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var currentDesktop = VirtualDesktopInterop.GetCurrentDesktopNumber();

            if (!windowsByDesktop.ContainsKey(currentDesktop))
            {
                windowsByDesktop[currentDesktop] = currentDesktop.ToString() + " - ";
            }
            foreach (var window in _windowList.GetWindows().OrderBy(x => x.Handle))
            {
                if (window.Name.Contains("Visual Studio"))
                {
                    windowsByDesktop[currentDesktop] += "VS ";
                }
                else if (window.Name.Contains("Edge"))
                {
                    windowsByDesktop[currentDesktop] += "I ";
                }
                else if (window.Name.Contains("WhatsApp"))
                {
                    windowsByDesktop[currentDesktop] += "W ";
                }
                else
                {
                    windowsByDesktop[currentDesktop] += "? ";
                }
            }

            _label.Text = windowsByDesktop[currentDesktop] + " | ";
            windowsByDesktop.Remove(currentDesktop);
        }

        private void MakeFormInvisible(object sender, EventArgs e)
        {
            SetWindowLongPtr(Form.Handle, GWL.GWL_EXSTYLE, (IntPtr)0x00000080L);
        }
    }
}
