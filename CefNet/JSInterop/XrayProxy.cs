using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CefNet.JSInterop
{
	internal sealed class XrayProxy : IDisposable
	{
		private GCHandle _providerHandle;
		private XrayHandle _instance;

		internal XrayProxy(XrayHandle instance, ScriptableObjectProvider provider)
		{
			_instance = instance;
			_providerHandle = GCHandle.Alloc(provider, GCHandleType.Normal);
		}

		~XrayProxy()
		{
			Dispose(false);
		}

		private void Dispose(bool disposing)
		{
			if (_providerHandle.IsAllocated)
			{
				if (!Environment.HasShutdownStarted)
				{
					Provider?.ReleaseObject(_instance);
				}
				_providerHandle.Free();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		internal bool TryGetHandle(out XrayHandle handle)
		{
			handle = _instance;
			return _providerHandle.IsAllocated;
		}

		internal ScriptableObjectProvider Provider
		{
			get { return _providerHandle.Target as ScriptableObjectProvider; }
		}

	}
}
