#if MODERNFORMS
using Modern.Forms;
#else
using System.Windows.Forms;
#endif

namespace CefNet
{
	public static class CefNetWindowsFormsExtensions
	{
		public static TabControl FindTabControl(this TabPage tab)
		{
			Control control = tab;
			while (control != null)
			{
				if (control is TabControl tabControl)
					return tabControl;
				control = control.Parent;
			}
			return null;
		}
	}
}
