using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using Modern.Forms;

namespace CefNet.Internal
{
	sealed class ModernFormsContextMenuRunner : IDisposable
	{
		private CefContextMenuParams MenuParams;
		private CefMenuModel Model;
		private CefRunContextMenuCallback Callback;
		internal ContextMenu Menu;
		private ConditionalWeakTable<MenuItem, object> MenuItemsTags = new ConditionalWeakTable<MenuItem, object>();

		public ModernFormsContextMenuRunner(CefContextMenuParams menuParams, CefMenuModel model, CefRunContextMenuCallback callback)
		{
			MenuParams = menuParams;
			Model = model;
			Callback = callback;
		}

		private void Menu_ItemClicked(object sender, MouseEventArgs e)
		{
			//object cid = e.ClickedItem.Tag;
			MenuItemsTags.TryGetValue((MenuItem)sender, out object cid);
			if (cid != null)
			{
				Callback.Continue((int)cid, CefEventFlags.LeftMouseButton);
				Callback = null;
			}
		}

		private void Menu_Closed(object sender, EventArgs e)
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

			Menu = new CustomContextMenu();
			((CustomContextMenu)Menu).Closed += Menu_Closed;
			//Menu.ItemClicked += Menu_ItemClicked;
			Build(Model, Menu.Items);
		}

		class CustomContextMenu : ContextMenu
		{
			private Timer timer;

			public event EventHandler Closed;

			public CustomContextMenu()
			{
				timer = new Timer(CheckVisibility);
			}

			private void CheckVisibility(object nullState)
			{
				if (CefNetApplication.Instance.CheckAccess())
				{
					if (!this.Visible)
					{
						Timer t = Interlocked.Exchange(ref timer, null);
						if (t != null)
						{
							t.Change(Timeout.Infinite, Timeout.Infinite);
							Closed?.Invoke(this, EventArgs.Empty);
						}
						this.Dispose();
					}
				}
				else
				{
					Application.RunOnUIThread(() => CheckVisibility(null));
				}
			}

			public override void Show(Point location)
			{
				base.Show(location);
				timer?.Change(1000, 1000);
			}

			protected override void Dispose(bool disposing)
			{
				Interlocked.Exchange(ref timer, null)?.Dispose();

				#region Fix memory leak (release any root references to the ContextMenu)

				FieldInfo fieldInfo = typeof(MenuDropDown).GetField("popup", BindingFlags.Instance | BindingFlags.NonPublic);
				var popup = fieldInfo?.GetValue(this) as PopupWindow;
				popup?.Close();

				fieldInfo = typeof(Theme).GetField("ThemeChanged", BindingFlags.Static | BindingFlags.NonPublic);
				EventHandler eh = fieldInfo?.GetValue(null) as EventHandler;
				if (eh != null)
				{
					foreach (Delegate handler in eh.GetInvocationList())
					{
						if (handler.Target == this)
						{
							var eventInfo = typeof(Theme).GetEvent("ThemeChanged", BindingFlags.Static | BindingFlags.Public);
							eventInfo?.RemoveEventHandler(null, handler);
						}
						else if (handler.Target is ScrollableControl && handler.Target.GetType().Name == "ControlAdapter")
						{
							PropertyInfo propInfo = handler.Target.GetType().GetProperty("Controls");
							var controls = propInfo?.GetValue(handler.Target) as ControlCollection;
							if (controls != null && controls.Contains(this))
							{
								controls.Remove(this);
							}
						}
					}
				}

				#endregion

				base.Dispose(disposing);
			}
		}


		private void Build(CefMenuModel model, MenuItemCollection menu)
		{
			CefColor color = default;
			int count = model.Count;
			for (int i = 0; i < count; i++)
			{
				MenuItem menuItem;
				switch (model.GetTypeAt(i))
				{
					case CefMenuItemType.Separator:
						menu.Add(new MenuSeparatorItem());
						continue;
					case CefMenuItemType.Check:
						menuItem = new MenuItem();
						//menuItem.CheckOnClick = true;
						//menuItem.Checked = model.IsCheckedAt(i);
						break;
					case CefMenuItemType.Radio:
						menuItem = new MenuItem();
						//menuItem.Checked = model.IsCheckedAt(i);
						break;
					case CefMenuItemType.Command:
						menuItem = new MenuItem();
						break;
					case CefMenuItemType.Submenu:
						menuItem = new MenuItem();
						if (model.IsEnabledAt(i))
						{
							//menuItem.DropDownItemClicked += Menu_ItemClicked;
							Build(model.GetSubMenuAt(i), menuItem.Items);
						}
						break;
					default:
						continue;
				}
				menuItem.Text = model.GetLabelAt(i).Replace("&", "");
				menuItem.Click += Menu_ItemClicked;
				menuItem.Enabled = model.IsEnabledAt(i);
				//menuItem.Tag = model.GetCommandIdAt(i);
				MenuItemsTags.Add(menuItem, model.GetCommandIdAt(i));
				//menuItem.ForeColor = model.GetColorAt(i, CefMenuColorType.Text, ref color) ? Color.FromArgb(color.ToArgb()) : SystemColors.ControlText;
				menu.Add(menuItem);
			}
		}

		public void Cancel()
		{
			Callback?.Cancel();
			Application.RunOnUIThread(this.Dispose);
		}

	}
}
