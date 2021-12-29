using CefNet.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace CefNet.Internal
{
	internal sealed class WpfContextMenuRunner
	{
		private readonly MenuModel Model;
		private CefRunContextMenuCallback Callback;
		private ContextMenu Menu;

		public WpfContextMenuRunner(CefMenuModel model, CefRunContextMenuCallback callback)
		{
			Model = MenuModel.FromCefMenu(model);
			Callback = callback;
		}

		public void Build()
		{
			if (Menu != null)
				throw new InvalidOperationException();

			Menu = new ContextMenu();
			Menu.Closed += Menu_Closed;
			Build(Model, Menu.Items);
		}

		private void Menu_Closed(object sender, RoutedEventArgs e)
		{
			Cancel();
		}

		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			var clickedItem = sender as MenuItem;
			object cid = clickedItem?.Tag;
			if (cid != null)
			{
				Callback.Continue((int)cid, CefEventFlags.LeftMouseButton);
				Callback = null;
			}
		}

		private void Build(MenuModel model, ItemCollection menu)
		{
			int count = model.Count;
			for (int i = 0; i < count; i++)
			{
				bool isSubmenu = false;
				MenuItem menuItem;
				switch (model.GetTypeAt(i))
				{
					case CefMenuItemType.Separator:
						menu.Add(new Separator());
						continue;
					case CefMenuItemType.Check:
						menuItem = new MenuItem();
						menuItem.IsCheckable = true;
						menuItem.IsChecked = model.IsCheckedAt(i);
						break;
					case CefMenuItemType.Radio:
						menuItem = new MenuItem();
						menuItem.IsCheckable = true;
						menuItem.Icon = new RadioButton() { IsChecked = model.IsCheckedAt(i) };
						break;
					case CefMenuItemType.Command:
						menuItem = new MenuItem();
						break;
					case CefMenuItemType.Submenu:
						isSubmenu = true;
						menuItem = new MenuItem();
						if (model.IsEnabledAt(i))
						{
							Build(model.GetSubMenuAt(i), menuItem.Items);
						}
						break;
					default:
						continue;
				}
				if (!isSubmenu)
				{
					menuItem.Click += MenuItem_Click;
					menuItem.Tag = model.GetCommandIdAt(i);
				}
				menuItem.Header = model.GetLabelAt(i).Replace('&', '_');
				menuItem.IsEnabled = model.IsEnabledAt(i);
				menuItem.Foreground = model.GetColorAt(i, CefMenuColorType.Text, out CefColor color) ? new SolidColorBrush(color.ToColor()) : SystemColors.MenuTextBrush;
				menu.Add(menuItem);
			}
		}

		public void RunMenuAt(Control control, Point point)
		{
			Menu.PlacementTarget = control;
			Menu.Placement = PlacementMode.Relative;
			Menu.HorizontalOffset = point.X;
			Menu.VerticalOffset = point.Y;
			Menu.IsOpen = true;
		}

		public void Cancel()
		{
			Callback?.Cancel();
			Callback = null;
		}

	}
}
