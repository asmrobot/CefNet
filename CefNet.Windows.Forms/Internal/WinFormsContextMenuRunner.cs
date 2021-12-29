using CefNet.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CefNet.Internal
{
	sealed class WinFormsContextMenuRunner : IDisposable
	{
		private CefContextMenuParams MenuParams;
		private CefMenuModel Model;
		private CefRunContextMenuCallback Callback;
		internal ContextMenuStrip Menu;

		public WinFormsContextMenuRunner(CefContextMenuParams menuParams, CefMenuModel model, CefRunContextMenuCallback callback)
		{
			MenuParams = menuParams;
			Model = model;
			Callback = callback;
		}

		private void Menu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			object cid = e.ClickedItem.Tag;
			if (cid != null)
			{
				Callback.Continue((int)cid, CefEventFlags.LeftMouseButton);
				Callback = null;
			}
		}

		private void Menu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			Cancel();
		}

		public void Dispose()
		{
			Menu?.Dispose();
		}

		public void Build()
		{
			if (Menu != null)
				throw new InvalidOperationException();

			Menu = new ContextMenuStrip();
			Menu.Closed += Menu_Closed;
			Menu.ItemClicked += Menu_ItemClicked;
			Build(Model, Menu.Items);
		}

		private void Build(CefMenuModel model, ToolStripItemCollection menu)
		{
			CefColor color = default;
			int count = model.Count;
			for (int i = 0; i < count; i++)
			{
				ToolStripMenuItem menuItem;
				switch (model.GetTypeAt(i))
				{
					case CefMenuItemType.Separator:
						menu.Add(new ToolStripSeparator());
						continue;
					case CefMenuItemType.Check:
						menuItem = new ToolStripMenuItem(model.GetLabelAt(i));
						menuItem.CheckOnClick = true;
						menuItem.Checked = model.IsCheckedAt(i);
						break;
					case CefMenuItemType.Radio:
						menuItem = new ToolStripRadioMenuItem(model.GetLabelAt(i));
						menuItem.Checked = model.IsCheckedAt(i);
						break;
					case CefMenuItemType.Command:
						menuItem = new ToolStripMenuItem(model.GetLabelAt(i));
						break;
					case CefMenuItemType.Submenu:
						menuItem = new ToolStripMenuItem(model.GetLabelAt(i));
						if (model.IsEnabledAt(i))
						{
							menuItem.DropDownItemClicked += Menu_ItemClicked;
							Build(model.GetSubMenuAt(i), menuItem.DropDownItems);
						}
						break;
					default:
						continue;
				}
				menuItem.Enabled = model.IsEnabledAt(i);
				menuItem.Tag = model.GetCommandIdAt(i);
				menuItem.ForeColor = model.GetColorAt(i, CefMenuColorType.Text, ref color) ? Color.FromArgb(color.ToArgb()) : SystemColors.ControlText;
				menu.Add(menuItem);
			}
		}

		public void Cancel()
		{
			Callback?.Cancel();
			WindowsFormsSynchronizationContext.Current.Post(_ => { this.Dispose(); }, null);
		}

	}
}
