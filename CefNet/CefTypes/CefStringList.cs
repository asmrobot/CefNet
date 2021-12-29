using CefNet.CApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CefNet
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe class CefStringList : ICollection<string>, ICloneable, IDisposable
	{
		private struct CefStringListEnumerator : IEnumerator<string>
		{
			private CefStringList _list;
			private int _index;
			private int _count;
			private cef_string_t _cstr0;

			public CefStringListEnumerator(CefStringList list)
			{
				_index = 0;
				_list = list;
				_cstr0 = new cef_string_t();
				_count = list.Count;
			}

			object IEnumerator.Current => Current;

			public string Current
			{
				get
				{
					fixed (cef_string_t* s = &_cstr0)
					{
						if (CefNativeApi.cef_string_list_value(_list.GetNativeInstance(), unchecked((UIntPtr)_index), s) == 0)
							throw new InvalidOperationException();
						return CefString.ReadAndFree(s);
					}
				}
			}

			

			public void Dispose() { }

			public bool MoveNext()
			{
				return ++_index < _count;
			}

			public void Reset()
			{
				_index = 0;
			}
		}

		private cef_string_list_t _instance;

		public static CefStringList Wrap(cef_string_list_t instance)
		{
			return new CefStringList(instance);
		}

		public CefStringList()
		{
			_instance = CefNativeApi.cef_string_list_alloc();
		}

		private CefStringList(cef_string_list_t instance)
		{
			_instance = instance;
			IsNative = true;
		}

		~CefStringList()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!IsNative)
			{
				CefNativeApi.cef_string_list_free(_instance);
			}
			_instance.Base = null;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public cef_string_list_t NativeInstance
		{
			get
			{
				if (_instance.Base == null)
					throw new ObjectDisposedException(nameof(CefStringList));
				return _instance;
			}
		}

		public cef_string_list_t GetNativeInstance()
		{
			return NativeInstance;
		}

		public bool IsNative { get; private set; }

		public int Count
		{
			get { return (int)CefNativeApi.cef_string_list_size(GetNativeInstance()); }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public string this[int index]
		{
			get
			{
				var cstr0 = new cef_string_t();
				if (CefNativeApi.cef_string_list_value(GetNativeInstance(), unchecked((UIntPtr)index), &cstr0) != 0)
				{
					return CefString.ReadAndFree(&cstr0);
				}
				throw new ArgumentOutOfRangeException(nameof(index));
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public void Add(string item)
		{
			fixed (char* s0 = item)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = (item != null ? item.Length : 0) };
				CefNativeApi.cef_string_list_append(GetNativeInstance(), &cstr0);
			}
		} 

		public void Clear()
		{
			CefNativeApi.cef_string_list_clear(GetNativeInstance());
		}

		public IEnumerator<string> GetEnumerator()
		{
			return new CefStringListEnumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public object Clone()
		{
			return new CefStringList { _instance = CefNativeApi.cef_string_list_copy(GetNativeInstance()) };
		}

		public bool Contains(string item)
		{
			foreach (string s in this)
			{
				if (string.Equals(item, s, StringComparison.InvariantCultureIgnoreCase))
					return true;
			}
			return false;
		}

		public void CopyTo(string[] array, int arrayIndex)
		{
			cef_string_list_t instance = GetNativeInstance();
			var cstr0 = new cef_string_t();
			for (int i = 0; i < array.Length; i++)
			{
				if (CefNativeApi.cef_string_list_value(instance, unchecked((UIntPtr)i), &cstr0) == 0)
					throw new InvalidOperationException();

				array[i + arrayIndex] = CefString.ReadAndFree(&cstr0);
			}
		}

		bool ICollection<string>.Remove(string item)
		{
			throw new NotSupportedException();
		}

		public string[] ToArray()
		{
			cef_string_list_t instance = GetNativeInstance();
			var cstr0 = new cef_string_t();

			var array = new string[this.Count];
			for (int i = 0; i < array.Length; i++)
			{
				if (CefNativeApi.cef_string_list_value(instance, unchecked((UIntPtr)i), &cstr0) == 0)
					throw new InvalidOperationException();

				array[i] = CefString.ReadAndFree(&cstr0);
			}
			return array;
		}

	}
}
