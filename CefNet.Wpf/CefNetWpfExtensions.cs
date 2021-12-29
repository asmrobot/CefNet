using CefNet.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CefNet.Wpf
{
	public static class CefNetWpfExtensions
	{
		private delegate object PropertyGetterInvokeDelegate(object obj, object[] parameters);

		private static PropertyGetterInvokeDelegate GetIsExtendedKey;

		static CefNetWpfExtensions()
		{
			try
			{
				PropertyInfo propertyInfo = typeof(KeyEventArgs).GetProperty("IsExtendedKey", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance, null, typeof(bool), Type.EmptyTypes, null);
				if (propertyInfo != null)
				{
					MethodInfo method = propertyInfo.GetGetMethod(true);
					if (method != null)
					{
						GetIsExtendedKey = method.Invoke;
					}
				}
			}
			catch { }
		}

#pragma warning disable IDE0060
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void Invoke<TEventArgs>(this RoutedEvent routedEvent, UIElement sender, TEventArgs e)
			where TEventArgs : class
		{
			sender.RaiseEvent(e as RoutedEventArgs);
		}
#pragma warning restore IDE0060

		public static void Union(this ref Int32Rect self, CefRect rect)
		{
			int x = Math.Min(self.X, rect.X);
			int right = Math.Max(self.X + self.Width, rect.X + rect.Width);
			int y = Math.Min(self.Y, rect.Y);
			int bottom = Math.Max(self.Y + self.Height, rect.Y + rect.Height);
			self = new Int32Rect(x, y, right - x, bottom - y);
		}

		public static VirtualKeys ToVirtualKey(this Key key)
		{
			if (key >= Key.LeftShift && key <= Key.RightAlt)
				return (VirtualKeys)((key - Key.LeftShift) >> 1) | VirtualKeys.ShiftKey; // VK_SHIFT, VK_CONTROL, VK_MENU
			if (key == Key.System)
				return VirtualKeys.Menu; // VK_MENU
			return (VirtualKeys)KeyInterop.VirtualKeyFromKey(key);
		}

		public static bool IsExtendedKey(this KeyEventArgs e)
		{
			return (GetIsExtendedKey != null) ? (bool)GetIsExtendedKey(e, null) : false;
		}

		public static Color ToColor(this CefColor color)
		{
			return Color.FromArgb(color.A, color.R, color.G, color.B);
		}

		/// <summary>
		/// Converts a drag drop effects to the CEF dragging operation mask.
		/// </summary>
		/// <param name="self">The drag drop effects.</param>
		/// <returns></returns>
		public static CefDragOperationsMask ToCefDragOperationsMask(this DragDropEffects self)
		{
			CefDragOperationsMask effects = CefDragOperationsMask.None;
			if (self.HasFlag(DragDropEffects.All))
				effects |= CefDragOperationsMask.Every;
			if (self.HasFlag(DragDropEffects.Copy))
				effects |= CefDragOperationsMask.Copy;
			if (self.HasFlag(DragDropEffects.Move))
				effects |= CefDragOperationsMask.Move;
			if (self.HasFlag(DragDropEffects.Link))
				effects |= CefDragOperationsMask.Link;
			return effects;
		}

		/// <summary>
		/// Converts the CEF dragging operation mask to drag drop effects.
		/// </summary>
		/// <param name="self">The CEF dragging operation mask.</param>
		/// <returns></returns>
		public static DragDropEffects ToDragDropEffects(this CefDragOperationsMask self)
		{
			DragDropEffects effects = DragDropEffects.None;
			if (self.HasFlag(CefDragOperationsMask.Every))
				effects |= DragDropEffects.All;
			if (self.HasFlag(CefDragOperationsMask.Copy))
				effects |= DragDropEffects.Copy;
			if (self.HasFlag(CefDragOperationsMask.Move))
				effects |= DragDropEffects.Move;
			if (self.HasFlag(CefDragOperationsMask.Link))
				effects |= DragDropEffects.Link;
			return effects;
		}

		/// <summary>
		/// Gets the current state of the SHIFT, CTRL, and ALT keys, as well as the state of the mouse buttons.
		/// </summary>
		/// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
		/// <returns></returns>
		public static CefEventFlags GetModifiers(this DragEventArgs e)
		{
			CefEventFlags flags = CefEventFlags.None;
			DragDropKeyStates state = e.KeyStates;
			if (state.HasFlag(DragDropKeyStates.LeftMouseButton))
				flags |= CefEventFlags.LeftMouseButton;
			if (state.HasFlag(DragDropKeyStates.RightMouseButton))
				flags |= CefEventFlags.RightMouseButton;
			if (state.HasFlag(DragDropKeyStates.ShiftKey))
				flags |= CefEventFlags.ShiftDown;
			if (state.HasFlag(DragDropKeyStates.ControlKey))
				flags |= CefEventFlags.ControlDown;
			if (state.HasFlag(DragDropKeyStates.MiddleMouseButton))
				flags |= CefEventFlags.MiddleMouseButton;
			if (state.HasFlag(DragDropKeyStates.AltKey))
				flags |= CefEventFlags.AltDown;
			return flags;
		}

		/// <summary>
		/// Gets the drag data.
		/// </summary>
		/// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
		public static CefDragData GetCefDragData(this DragEventArgs e)
		{
			CefDragData dragData;
			if (e.Data.GetDataPresent(nameof(CefDragData)))
			{
				dragData = (CefDragData)e.Data.GetData(nameof(CefDragData));
				if (dragData != null)
				{
					dragData.ResetFileContents();
					return dragData;
				}
			}

			dragData = new CefDragData();

			string[] formats = e.Data.GetFormats();

			if (formats.Contains(DataFormats.FileDrop))
			{
				foreach (string filePath in (string[])e.Data.GetData(DataFormats.FileDrop))
				{
					dragData.AddFile(filePath.Replace("\\", "/"), Path.GetFileName(filePath));
				}
			}

			bool isUrl = false;
			string s = GetUrlString(e.Data, formats);
			if (!string.IsNullOrWhiteSpace(s))
			{
				isUrl = true;
				dragData.LinkUrl = s;
			}

			if (formats.Contains(DataFormats.UnicodeText))
			{
				s = (string)e.Data.GetData(DataFormats.UnicodeText);

				if (!isUrl && Uri.IsWellFormedUriString(s, UriKind.Absolute))
					dragData.LinkUrl = s;
				dragData.FragmentText = s;
			}
			else if (formats.Contains(DataFormats.Text))
			{
				s = (string)e.Data.GetData(DataFormats.Text);

				if (!isUrl && Uri.IsWellFormedUriString(s, UriKind.Absolute))
					dragData.LinkUrl = s;
				dragData.FragmentText = s;
			}

			if (formats.Contains(DataFormats.Html))
			{
				dragData.FragmentHtml = (string)e.Data.GetData(DataFormats.Html);
			}
			else if (formats.Contains(CefNetDragData.DataFormatTextHtml))
			{
				dragData.FragmentHtml = Encoding.UTF8.GetString(((MemoryStream)e.Data.GetData(CefNetDragData.DataFormatTextHtml)).ToArray());
			}
			return dragData;
		}

		private static string GetUrlString(IDataObject data, string[] formats)
		{
			if (formats.Contains(CefNetDragData.DataFormatUnicodeUrl))
				return GetUrlFromDragDropData(data, CefNetDragData.DataFormatUnicodeUrl, Encoding.Unicode);

			if (formats.Contains(CefNetDragData.DataFormatUrl))
				return GetUrlFromDragDropData(data, CefNetDragData.DataFormatUrl, Encoding.ASCII);

			return null;
		}

		private static string GetUrlFromDragDropData(IDataObject data, string format, Encoding encoding)
		{
			using (TextReader reader = new StreamReader((Stream)data.GetData(format), encoding, true, -1, false))
			{
				return reader.ReadToEnd().TrimEnd('\0');
			}
		}



#if DEBUG
		internal static void Save(this BitmapSource source, string filename)
		{
			using (var file = new FileStream(filename, FileMode.Create))
			{
				PngBitmapEncoder encoder = new PngBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(source));
				encoder.Save(file);
				file.Flush();
			}

		}
#endif

	}
}
