using CefNet.CApi;

namespace CefNet
{
	public unsafe partial struct CefCompositionUnderline
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CefCompositionUnderline"/> struct.
		/// </summary>
		/// <param name="range">The underline character range.</param>
		/// <param name="color">The text color.</param>
		/// <param name="backgroundColor">The background color.</param>
		/// <param name="thick">The thick underline.</param>
		public CefCompositionUnderline(CefRange range, CefColor color, CefColor backgroundColor, bool thick)
		{
			_instance = new cef_composition_underline_t
			{
				range = range,
				color = color,
				background_color = backgroundColor,
				thick = thick ? 1 : 0,
			};
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CefCompositionUnderline"/> struct.
		/// </summary>
		/// <param name="range">The underline character range.</param>
		/// <param name="color">The text color.</param>
		/// <param name="backgroundColor">The background color.</param>
		/// <param name="thick">The thick underline.</param>
		/// <param name="style">The style.</param>
		public CefCompositionUnderline(CefRange range, CefColor color, CefColor backgroundColor, bool thick, CefCompositionUnderlineStyle style)
		{
			_instance = new cef_composition_underline_t
			{
				range = range,
				color = color,
				background_color = backgroundColor,
				thick = thick ? 1 : 0,
				style = style,
			};
		}

	}
}
