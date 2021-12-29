using Modern.Forms;

namespace CefNet.Modern.Forms
{
	public sealed class CustomCursor
	{
		public unsafe static Cursor Create(ref CefCursorInfo cursorInfo)
		{
			return Cursor.Default;
		}
	}
}
