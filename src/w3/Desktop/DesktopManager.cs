using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using w3.Interop;
using w3.Model;
using w3.Window;

namespace w3.Desktop
{
    [ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("F31574D6-B682-4CDC-BD56-1827860ABEC6")]
	public interface IVirtualDesktopManagerInternal
	{
		int GetCount();
		void MoveViewToDesktop(IntPtr view, Guid desktop);
		bool CanViewMoveDesktops(IntPtr view);
		Guid GetCurrentDesktop();
		void GetDesktops(out IObjectArray desktops);
		[PreserveSig]
		int GetAdjacentDesktop(Guid from, int direction, out Guid desktop);
		void SwitchDesktop(IVirtualDesktop desktop);
		Guid CreateDesktop();
		void RemoveDesktop(Guid desktop, Guid fallback);
		Guid FindDesktop(ref Guid desktopid);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("6D5140C1-7436-11CE-8034-00AA006009FA")]
	internal interface IServiceProvider10
	{
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object QueryService(ref Guid service, ref Guid riid);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("FF72FFDD-BE7E-43FC-9C03-AD81681E88E4")]
	public interface IVirtualDesktop
	{
		bool IsViewVisible(IntPtr view);
		Guid GetId();
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("92CA9DCD-5622-4BBA-A805-5E9F541BD8C9")]
	public interface IObjectArray
	{
		void GetCount(out int count);
		void GetAt(int index, ref Guid iid, [MarshalAs(UnmanagedType.Interface)]out object obj);
	}

    public class DesktopManager
    {
		private IVirtualDesktopManagerInternal VirtualDesktopManagerInternal;
        private readonly WindowList _windowList;

        public DesktopManager(WindowList windowList)
        {
			var shell = (IServiceProvider10)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("C2F03A33-21F5-47FA-B4BB-156362A2F239")));
			VirtualDesktopManagerInternal = (IVirtualDesktopManagerInternal)shell.QueryService(new Guid("C5E0CDCA-7B6E-41B2-9FC4-D93975CC467B"), typeof(IVirtualDesktopManagerInternal).GUID);

            _windowList = windowList;
        }

		public void InitWorkspaces()
        {
			while (VirtualDesktopManagerInternal.GetCount() < 10)
            {
				VirtualDesktopManagerInternal.CreateDesktop();
            }
        }

		public void GoToWorkspace(int index)
        {
			VirtualDesktopInterop.GoToDesktopNumber(index);
            var windowList = _windowList.GetWindows().OrderBy(x => x.Handle).ToList();
            var windowToFocus = _windowList.GetWindows().FirstOrDefault();
            _ = Win32.SetForegroundWindow(windowToFocus?.Handle ?? Win32.GetForegroundWindow());
        }

		public void FocusOnRightWindow()
        {
            var windowList = _windowList.GetWindows().OrderBy(x => x.Handle).ToList();
            var windowToFocus = (windowList.FindIndex(x => x.Handle == Win32.GetForegroundWindow()) + 1) % windowList.Count;

            FocusOnWindow(windowList, windowToFocus);
        }
        
        public void FocusOnLeftWindow()
		{ 
			var windowList = _windowList.GetWindows().OrderBy(x => x.Handle).ToList();
            var windowToFocus = windowList.FindIndex(x => x.Handle == Win32.GetForegroundWindow()) - 1 < 0
                ? windowList.Count - 1
                : windowList.FindIndex(x => x.Handle == Win32.GetForegroundWindow()) - 1;

            FocusOnWindow(windowList, windowToFocus);
		}

		public static void SwapWindowsLockScreen()
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C \"tools\\TurnLockOnOff.bat\"",
                Verb = "runas"
            };

            Process.Start(startInfo)?.WaitForExit();
        }

		public void MoveWindowToWorkspace(int index)
			=> VirtualDesktopInterop.MoveWindowToDesktopNumber(VirtualDesktopInterop.ViewGetFocused(), index);

        private static void FocusOnWindow(List<DetectedWindow> windowList, int windowToFocus)
        {
            var placement = new Windowplacement();
            _ = Win32.GetWindowPlacement(windowList[windowToFocus].Handle, ref placement);
            if (placement.showCmd == 2)
            {
                _ = Win32.ShowWindow(windowList[windowToFocus].Handle, ShowWindowEnum.Restore);
            }
            Thread.Sleep(20);
            _ = Win32.SetForegroundWindow(windowList[windowToFocus].Handle);
        }
    }
}
