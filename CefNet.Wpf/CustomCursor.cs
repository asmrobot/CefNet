using CefNet.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CefNet.Wpf
{
	public sealed class CustomCursor
	{
		public unsafe static Cursor Create(ref CefCursorInfo cursorInfo)
		{
			CefSize size = cursorInfo.Size;
			if (size.Width > 0 && size.Height > 0 && cursorInfo.Buffer != IntPtr.Zero)
			{
				try
				{
					var bufferSize = size.Width * size.Height * 4;
					int ICON_HEADER_SIZE = sizeof(ICONDIR);
					var stream = new MemoryStream();
					{
						DpiScale dpi = OffscreenGraphics.DpiScale;
						var source = BitmapSource.Create(size.Width, size.Height, dpi.PixelsPerInchX, dpi.PixelsPerInchY, PixelFormats.Bgra32, null, cursorInfo.Buffer, bufferSize, size.Width << 2);
						if (stream.Seek(ICON_HEADER_SIZE, SeekOrigin.Begin) != ICON_HEADER_SIZE)
						{
							stream.Seek(0, SeekOrigin.Begin);
							stream.Write(new byte[ICON_HEADER_SIZE], 0, ICON_HEADER_SIZE);
						}

						var png = new PngBitmapEncoder();
						png.Frames.Add(BitmapFrame.Create(source));
						png.Save(stream);
						stream.Seek(0, SeekOrigin.Begin);
					}

					CefPoint hotSpot = cursorInfo.Hotspot;

					var icon = new ICONDIR();
					icon.IconType = 2;
					icon.ImagesCount = 1;
					icon.Width = (byte)size.Width;
					icon.Height = (byte)size.Height;
					icon.HotSpotX = (short)hotSpot.X;
					icon.HotSpotY = (short)hotSpot.Y;
					icon.BytesInRes = (int)stream.Length - ICON_HEADER_SIZE;
					icon.ImageOffset = ICON_HEADER_SIZE;

					using (var iconHead = new UnmanagedMemoryStream(icon._data, ICON_HEADER_SIZE))
					{
						iconHead.CopyTo(stream);
						stream.Seek(0, SeekOrigin.Begin);
					}

					return new Cursor(stream);
				}
				catch (AccessViolationException) { throw; }
				catch { }
			}
			return Cursors.Arrow;
		}

		[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 22)]
		unsafe struct ICONDIR
		{
			[FieldOffset(0)]
			public fixed byte _data[22];

			/// <summary>
			/// Reserved. Must always be 0.
			/// </summary>
			[FieldOffset(0)]
			public short IDReserved;
			/// <summary>
			/// Specifies image type: 1 for icon (.ICO) image, 2 for cursor (.CUR) image.
			/// </summary>
			[FieldOffset(2)]
			public short IconType;
			/// <summary>
			/// Specifies number of images in the file.
			/// </summary>
			[FieldOffset(4)]
			public short ImagesCount;
			/// <summary>
			/// Specifies image width in pixels. Can be any number between 0 and 255. Value 0 means image width is 256 pixels.
			/// </summary>
			[FieldOffset(6)]
			public byte Width;
			/// <summary>
			/// Specifies image height in pixels. Can be any number between 0 and 255. Value 0 means image height is 256 pixels.
			/// </summary>
			[FieldOffset(7)]
			public byte Height;
			/// <summary>
			/// Specifies number of colors in the color palette. Should be 0 if the image does not use a color palette.
			/// </summary>
			[FieldOffset(8)]
			public byte ColorCount;
			/// <summary>
			/// Reserved. Should be 0.
			/// </summary>
			[FieldOffset(9)]
			public byte IDEReserved;
			/// <summary>
			/// Specifies the horizontal coordinates of the hotspot in number of pixels from the left.
			/// </summary>
			[FieldOffset(10)]
			public short HotSpotX;
			/// <summary>
			/// Specifies the vertical coordinates of the hotspot in number of pixels from the top.
			/// </summary>
			[FieldOffset(12)]
			public short HotSpotY;
			/// <summary>
			/// Specifies the size of the image's data in bytes
			/// </summary>
			[FieldOffset(14)]
			public int BytesInRes;
			/// <summary>
			/// Specifies the offset of BMP or PNG data from the beginning of the ICO/CUR file
			/// </summary>
			[FieldOffset(18)]
			public int ImageOffset;
			
		}


	}
}
