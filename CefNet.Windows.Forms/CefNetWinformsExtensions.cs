using System;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;

namespace CefNet.Windows.Forms
{
	public static class CefNetWinformsExtensions
	{
		public static Rectangle ToRectangle(ref this CefRect self)
		{
			return new Rectangle(self.X, self.Y, self.Width, self.Height);
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
			int state = e.KeyState;
			if ((state & 1) == 1)
				flags |= CefEventFlags.LeftMouseButton;
			if ((state & 2) == 2)
				flags |= CefEventFlags.RightMouseButton;
			if ((state & 4) == 4)
				flags |= CefEventFlags.ShiftDown;
			if ((state & 8) == 8)
				flags |= CefEventFlags.ControlDown;
			if ((state & 16) == 32)
				flags |= CefEventFlags.MiddleMouseButton;
			if ((state & 32) == 32)
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

	}

}
