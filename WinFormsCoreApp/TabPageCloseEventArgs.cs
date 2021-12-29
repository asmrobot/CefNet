using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Windows.Forms
{
	public class TabPageCloseEventArgs : CancelEventArgs
	{
		public TabPageCloseEventArgs(TabPage tab)
			: this(tab, false)
		{

		}

		public TabPageCloseEventArgs(TabPage tab, bool force)
		{
			this.Tab = tab;
			this.Force = force;
		}

		public TabPage Tab { get; private set; }

		public bool Force { get; private set; }
	}
}
