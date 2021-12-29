using CefNet.CApi;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Collections;

namespace CefNet
{
	/// <summary>
	/// Represents a string multimap.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe sealed class CefStringMultimap : IDisposable, IEnumerable<string>
	{
		private cef_string_multimap_t _instance;

		/// <summary>
		/// Initializes a new instance of the <see cref="CefStringMultimap"/> class.
		/// </summary>
		public CefStringMultimap()
		{
			_instance = CefNativeApi.cef_string_multimap_alloc();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CefStringMultimap"/> class.
		/// </summary>
		/// <param name="instance">The native string multimap instance.</param>
		public CefStringMultimap(cef_string_multimap_t instance)
		{
			_instance = instance;
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Allows an object to try to free resources and perform other cleanup operations
		/// before it is reclaimed by garbage collection.
		/// </summary>
		~CefStringMultimap()
		{
			Dispose(false);
		}

#pragma warning disable CS1591
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (_instance.Base == null)
				return;

			CefNativeApi.cef_string_multimap_clear(_instance);
			CefNativeApi.cef_string_multimap_free(_instance);
			_instance = default;
		}
#pragma warning restore CS1591

		private cef_string_multimap_t Instance
		{
			get
			{
				if (_instance.Base == null)
					throw new ObjectDisposedException(this.GetType().Name);
				return _instance;
			}
		}

		/// <summary>
		/// Gets the value of the entry at the specified index of the <see cref="CefStringMultimap"/>.
		/// </summary>
		/// <param name="index">The zero-based index of the entry to locate in the <see cref="CefStringMultimap"/>.</param>
		/// <returns>
		/// A <see cref="String"/> that contains the value of the entry at the specified index of the collection.
		/// </returns>
		public string this[int index]
		{
			get
			{
				return Get(index);
			}
		}

		/// <summary>
		/// Gets the entry with the specified key in the <see cref="CefStringMultimap"/>.
		/// </summary>
		/// <param name="key">The <see cref="String"/> key of the entry to locate. The <paramref name="key"/> can be null.</param>
		/// <returns>
		/// A <see cref="String"/> that contains the comma-separated list of values associated
		/// with the specified key, if found; otherwise, null.
		/// </returns>
		public string this[string key]
		{
			get
			{
				return Get(key);
			}
		}

		/// <summary>
		/// Gets all the keys in the <see cref="CefStringMultimap"/>.
		/// </summary>
		public string[] AllKeys
		{
			get
			{
				var keys = new string[this.Count];
				for (uint i = 0; i < keys.Length; i++)
				{
					var cstr = new cef_string_t();
					if (CefNativeApi.cef_string_multimap_key(Instance, new UIntPtr(i), &cstr) == 0)
					{
						Array.Resize(ref keys, Math.Max((int)i - 1, 0));
						return keys;
					}
					keys[i] = CefString.ReadAndFree(&cstr);
				}
				return keys;
			}
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="CefStringMultimap"/> instance.
		/// </summary>
		public int Count
		{
			get
			{
				return (int)CefNativeApi.cef_string_multimap_size(Instance);
			}
		}

		/// <summary>
		/// Adds an entry with the specified name and value to the <see cref="CefStringMultimap"/>.
		/// </summary>
		/// <param name="name">The <see cref="String"/> key of the entry to add. The <paramref name="key"/> can be null.</param>
		/// <param name="value">The <see cref="String"/> value of the entry to add. The <paramref name="value"/> can be null.</param>
		public void Add(string name, string value)
		{
			fixed (char* s0 = name)
			fixed (char* s1 = value)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = name is null ? 0 : name.Length };
				var cstr1 = new cef_string_t { Str = s1, Length = value is null ? 0 : value.Length };
				if (CefNativeApi.cef_string_multimap_append(Instance, &cstr0, &cstr1) != 1)
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// Adds the elements of the specified collection to the end of the <see cref="CefStringMultimap"/>.
		/// </summary>
		/// <param name="collection">
		/// The collection whose elements should be added to the end of the <see cref="CefStringMultimap"/>.
		/// The collection itself cannot be null, but it can contain key/value pairs that the key or the value are null.
		/// </param>
		public void Add(IEnumerable<KeyValuePair<string, string>> collection)
		{
			if (collection is null)
				throw new ArgumentNullException(nameof(collection));

			foreach (KeyValuePair<string, string> kvp in collection)
			{
				fixed (char* s0 = kvp.Key)
				fixed (char* s1 = kvp.Value)
				{
					var cstr0 = new cef_string_t { Str = s0, Length = s0 == null ? 0 : kvp.Key.Length };
					var cstr1 = new cef_string_t { Str = s1, Length = s1 == null ? 0 : kvp.Value.Length };
					CefNativeApi.cef_string_multimap_append(Instance, &cstr0, &cstr1);
				}
			}
		}

		/// <summary>
		/// Copies the entries in the specified <see cref="NameValueCollection"/> to the <see cref="CefStringMultimap"/>.
		/// </summary>
		/// <param name="collection">The <see cref="NameValueCollection"/> to copy to the <see cref="CefStringMultimap"/>.</param>
		public void Add(NameValueCollection collection)
		{
			if (collection is null)
				throw new ArgumentNullException(nameof(collection));

			for (int i = 0; i < collection.Count; i++)
			{
				string key = collection.GetKey(i);
				string value = collection[i];
				fixed (char* s0 = key)
				fixed (char* s1 = value)
				{
					var cstr0 = new cef_string_t { Str = s0, Length = key is null ? 0 : key.Length };
					var cstr1 = new cef_string_t { Str = s1, Length = value is null ? 0 : value.Length };
					CefNativeApi.cef_string_multimap_append(Instance, &cstr0, &cstr1);
				}
			}
		}

		/// <summary>
		/// Gets the key at the specified index of the <see cref="CefStringMultimap"/>.
		/// </summary>
		/// <param name="index">The zero-based index of the key to get from the collection.</param>
		/// <returns>
		/// A <see cref="String"/> that contains the key at the specified index of
		/// the <see cref="CefStringMultimap"/>, if found; otherwise, null.
		/// </returns>
		public string GetKey(int index)
		{
			if (index < 0)
				throw new IndexOutOfRangeException();

			var cstr = new cef_string_t();
			if (CefNativeApi.cef_string_multimap_key(Instance, new UIntPtr((uint)index), &cstr) == 0)
				throw new IndexOutOfRangeException();
			return CefString.ReadAndFree(&cstr);
		}

		/// <summary>
		/// Returns the value of the entry at the specified index of the <see cref="CefStringMultimap"/>.
		/// </summary>
		/// <param name="index">The zero-based index of the entry to locate in the <see cref="CefStringMultimap"/>.</param>
		/// <returns>
		/// A <see cref="String"/> that contains the value of the entry at the specified index of the collection.
		/// </returns>
		public string Get(int index)
		{
			if (index < 0)
				throw new IndexOutOfRangeException();
			var cstr = new cef_string_t();
			if (CefNativeApi.cef_string_multimap_value(Instance, new UIntPtr((uint)index), &cstr) == 0)
				throw new IndexOutOfRangeException();
			return CefString.ReadAndFree(&cstr);
		}

		/// <summary>
		/// Returns the entry with the specified key in the <see cref="CefStringMultimap"/>.
		/// </summary>
		/// <param name="key">The <see cref="String"/> key of the entry to locate. The <paramref name="key"/> can be null.</param>
		/// <returns>
		/// A <see cref="String"/> that contains the comma-separated list of values associated
		/// with the specified key, if found; otherwise, null.
		/// </returns>
		public string Get(string key)
		{
			string[] values = GetValues(key);
			if (values is null)
				return null;
			return string.Join(",", values.Where(v => v != null));
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="CefStringMultimap"/> contains keys that are not null.
		/// </summary>
		/// <returns>true if the <see cref="CefStringMultimap"/> contains keys that are not null; otherwise, false.</returns>
		public bool HasKeys()
		{
			uint count = (uint)this.Count;
			for (uint i = 0; i < count; i++)
			{
				var cstr = new cef_string_t();
				if (CefNativeApi.cef_string_multimap_key(Instance, new UIntPtr(i), &cstr) == 0)
					return false;
				if (CefString.ReadAndFree(&cstr) != null)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Removes all entries from the <see cref="CefStringMultimap"/>.
		/// </summary>
		public void Clear()
		{
			CefNativeApi.cef_string_multimap_clear(Instance);
		}

		/// <summary>
		/// Returns the number of values with the specified key.
		/// </summary>
		/// <param name="key">The specified key.</param>
		/// <returns>The number of values with the specified key.</returns>
		public int CountWith(string key)
		{
			fixed (char* s0 = key)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = key is null ? 0 : key.Length };
				return (int)CefNativeApi.cef_string_multimap_find_count(Instance, &cstr0);
			}
		}

		/// <summary>
		/// Gets the values associated with the specified key from the <see cref="CefStringMultimap"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="String"/> array that contains the values associated with
		/// the specified key from the <see cref="CefStringMultimap"/>, if found; otherwise, null.
		/// </returns>
		public string[] GetValues(string key)
		{
			fixed (char* s0 = key)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = key is null ? 0 : key.Length };
				uint count = (uint)CefNativeApi.cef_string_multimap_find_count(Instance, &cstr0);
				if (count == 0)
					return null;

				var cstr1 = new cef_string_t();
				var values = new string[count];
				for (uint i = 0; i < count; i++)
				{
					if (CefNativeApi.cef_string_multimap_enumerate(Instance, &cstr0, new UIntPtr(i), &cstr1) == 0)
					{
						if (i == 0)
							return null;
						Array.Resize(ref values, Math.Max((int)i, 0));
						return values;
					}
					values[i] = CefString.ReadAndFree(&cstr1);
				}
				return values;
			}
		}

#pragma warning disable CS1591
		public IEnumerator<string> GetEnumerator()
		{
			return ((IEnumerable<string>)AllKeys).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)AllKeys).GetEnumerator();
		}

		public static implicit operator cef_string_multimap_t(CefStringMultimap instance)
		{
			if (instance is null)
				throw new ArgumentNullException(nameof(instance));
			return instance.Instance;
		}
#pragma warning restore CS1591

	}
}
