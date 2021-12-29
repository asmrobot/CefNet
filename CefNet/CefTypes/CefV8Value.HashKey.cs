using System;
using System.Collections.Generic;
using System.Text;
using CefNet.CApi;

namespace CefNet
{
	unsafe partial class CefV8Value
	{
		private struct HashKey
		{
			public int hashcode;
			public cef_v8value_t* raw;
			public WeakReference<CefV8Value> wrapped;

			public HashKey(int hashcode, cef_v8value_t* raw)
			{
				this.wrapped = null;
				this.hashcode = hashcode;
				this.raw = raw;
			}

			public HashKey(int hashcode, WeakReference<CefV8Value> weakRef)
			{
				this.wrapped = weakRef;
				this.hashcode = hashcode;
				this.raw = null;
			}

			public override int GetHashCode()
			{
				return hashcode;
			}
		}

		private sealed class HashKeyComparer : IEqualityComparer<HashKey>
		{
			public bool Equals(HashKey x, HashKey y)
			{
				if (x.hashcode == y.hashcode)
				{
					CefV8Value value;
					if (x.wrapped != null)
					{
						if (y.wrapped != null)
							return ReferenceEquals(x.wrapped, y.wrapped);

						if (y.raw != null && x.wrapped.TryGetTarget(out value))
						{
							return y.raw->IsSame(value.GetNativeInstance()) != 0;
						}
					}
					else if (x.raw != null)
					{
						if (y.wrapped != null && y.wrapped.TryGetTarget(out value))
						{
							return x.raw->IsSame(value.GetNativeInstance()) != 0;
						}
					}
				}
				else if(x.wrapped != null)
				{
					return ReferenceEquals(x.wrapped, y.wrapped);
				}
				return false;
			}

			public int GetHashCode(HashKey obj)
			{
				return obj.hashcode;
			}
		}
	}
}
