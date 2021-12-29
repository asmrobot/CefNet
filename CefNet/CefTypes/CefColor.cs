using CefNet.CApi;
using System.Runtime.InteropServices;


namespace CefNet
{
	/// <summary>
	/// 32-bit ARGB color value, not premultiplied. The color components are always
	/// in a known order. Equivalent to the SkColor type.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct CefColor
	{
		private cef_color_t _instance;

		private CefColor(cef_color_t instance)
		{
			_instance = instance;
		}

		/// <summary>
		/// Gets the alpha component value of this <see cref="CefColor"/> structure.
		/// </summary>
		public byte A
		{
			get { return (byte)((_instance.Base >> 24) & 0xFF); }
		}

		/// <summary>
		/// Gets the red component value of this <see cref="CefColor"/> structure.
		/// </summary>
		public byte R
		{
			get { return (byte)((_instance.Base >> 16) & 0xFF); }
		}

		/// <summary>
		/// Gets the green component value of this <see cref="CefColor"/> structure.
		/// </summary>
		public byte G
		{
			get { return (byte)((_instance.Base >> 8) & 0xFF); }
		}

		/// <summary>
		/// Gets the blue component value of this <see cref="CefColor"/> structure.
		/// </summary>
		public byte B
		{
			get { return (byte)(_instance.Base & 0xFF); }
		}

		/// <summary>
		/// Gets the 32-bit ARGB value of this <see cref="CefColor"/> structure.
		/// </summary>
		/// <returns>The 32-bit ARGB value of this <see cref="CefColor"/>.</returns>
		public int ToArgb()
		{
			return (int)_instance.Base;
		}

		/// <summary>
		/// Creates a <see cref="CefColor"/> structure from the four 8-bit ARGB
		/// components (alpha, red, green, and blue) values.
		/// </summary>
		/// <param name="argb">A value specifying the 32-bit ARGB value.</param>
		/// <returns>The <see cref="CefColor"/> structure that this method creates.</returns>
		public static CefColor FromArgb(int argb)
		{
			return new CefColor { _instance = { Base = (uint)argb } };
		}

		/// <summary>
		/// Converts a <see cref="CefColor"/> into a <see cref="cef_color_t"/>.
		/// </summary>
		/// <param name="instance">The value to convert.</param>
		/// <returns>The <see cref="cef_color_t"/> structure that this method creates.</returns>
		public static implicit operator cef_color_t(CefColor instance)
		{
			return instance._instance;
		}

		/// <summary>
		/// Converts a <see cref="cef_color_t"/> into a <see cref="CefColor"/>.
		/// </summary>
		/// <param name="instance">The value to convert.</param>
		/// <returns>The <see cref="CefColor"/> structure that this method creates.</returns>
		public static implicit operator CefColor(cef_color_t instance)
		{
			return new CefColor(instance);
		}

		/// <summary>
		/// Converts a <see cref="CefColor"/> into a 32-bit ARGB value.
		/// </summary>
		/// <param name="color">The value to convert.</param>
		/// <returns>The 32-bit ARGB value of the specified <see cref="CefColor"/>.</returns>
		public static implicit operator int(CefColor color)
		{
			return (int)color._instance.Base;
		}

		/// <summary>
		/// Converts a 32-bit ARGB value into a <see cref="CefColor"/>.
		/// </summary>
		/// <param name="argb">A value specifying the 32-bit ARGB value.</param>
		/// <returns>The <see cref="CefColor"/> structure that this method creates.</returns>
		public static implicit operator CefColor(int argb)
		{
			return new CefColor { _instance = { Base = (uint)argb } };
		}

	}
}	

