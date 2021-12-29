using CefNet.CApi;
using System;
using System.Collections.Generic;

namespace CefNet
{
	public unsafe partial class CefCommandLine
	{
		private static CefCommandLineGlobal _GlobalInstance;
		private static readonly object SyncRoot = new object();

		/// <summary>
		/// Returns the singleton global CefCommandLine object. The returned object will be read-only.
		/// </summary>
		public static CefCommandLine Global
		{
			get
			{
				if (_GlobalInstance == null)
				{
					lock (SyncRoot)
					{
						if (_GlobalInstance == null)
						{
							_GlobalInstance = new CefCommandLineGlobal();
						}
					}
				}
				return _GlobalInstance;
			}
		}

		/// <summary>
		/// Create a new CefCommandLine instance.
		/// </summary>
		public CefCommandLine()
			: this(CefNativeApi.cef_command_line_create())
		{

		}

		/// <summary>
		/// Gets an IEnumerable&lt;T&gt; view of switch names and values.
		/// </summary>
		/// <returns>An enumerable view of switches.</returns>
		public IEnumerable<KeyValuePair<string, string>> GetSwitches()
		{
			List<KeyValuePair<string, string>> items;
			using (var map = new CefStringMap())
			{
				this.GetSwitches(map);
				int count = map.Count;
				items = new List<KeyValuePair<string, string>>(count);
				for (int i = 0; i < count; i++)
				{
					items.Add(map.Get(i));
				}
			}
			return items;
		}

	}
}

