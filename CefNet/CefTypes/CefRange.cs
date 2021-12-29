using CefNet.CApi;

namespace CefNet
{
	partial struct CefRange
	{
		/// <summary>
		/// Instantiates a new <see cref="CefRange"/> instance with the specified starting and ending indexes.
		/// </summary>
		/// <param name="from">The inclusive start index of the range.</param>
		/// <param name="to">The exclusive end index of the range.</param>
		public CefRange(int from, int to)
		{
			_instance = new cef_range_t { from = from, to = to };
		}
	}
}
