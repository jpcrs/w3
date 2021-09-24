using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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

    public class VirtualDesktopManager
    {
		private IVirtualDesktopManagerInternal VirtualDesktopManagerInternal;
        public VirtualDesktopManager()
        {
			var shell = (IServiceProvider10)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("C2F03A33-21F5-47FA-B4BB-156362A2F239")));
			VirtualDesktopManagerInternal = (IVirtualDesktopManagerInternal)shell.QueryService(new Guid("C5E0CDCA-7B6E-41B2-9FC4-D93975CC467B"), typeof(IVirtualDesktopManagerInternal).GUID);
        }

		public void InitDesktops()
        {
			while (VirtualDesktopManagerInternal.GetCount() < 10)
            {
				VirtualDesktopManagerInternal.CreateDesktop();
            }
        }

		public void GoToDesktop(int index)
        {
			DesktopManager.GoToDesktopNumber(index);
        }

		public void MoveWindowToDesktop(int index)
			=> DesktopManager.MoveWindowToDesktopNumber(DesktopManager.ViewGetFocused(), index);
    }
}
