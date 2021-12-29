using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using CefNet.CApi;

namespace CefNet
{
	/// <summary>
	/// Represents a collection of string keys and values.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public sealed unsafe class CefStringMap : IDisposable, IDictionary<string, string>
	{
		private cef_string_map_t _instance;
#if DEBUG
		private readonly bool _finalizable;
#endif

		/// <summary>
		/// Initializes a new instance of the <see cref="CefStringMap"/> class.
		/// </summary>
		public CefStringMap()
		{
			_instance = CefNativeApi.cef_string_map_alloc();
#if DEBUG
			_finalizable = true;
#endif
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CefStringMap"/> class.
		/// </summary>
		/// <param name="instance">The native string map instance.</param>
		public CefStringMap(cef_string_map_t instance)
		{
			_instance = instance;
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Allows an object to try to free resources and perform other cleanup operations
		/// before it is reclaimed by garbage collection.
		/// </summary>
		~CefStringMap()
		{
			Dispose(false);
		}

		private void Dispose(bool disposing)
		{
			if (_instance.Base == null)
				return;

			CefNativeApi.cef_string_map_clear(_instance);
			CefNativeApi.cef_string_map_free(_instance);
			_instance = default;
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private cef_string_map_t Instance
		{
			get
			{
				if (_instance.Base == null)
					throw new ObjectDisposedException(this.GetType().Name);
				return _instance;
			}
		}

		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key of the value to get.</param>
		/// <exception cref="ArgumentNullException">key is null.</exception>
		/// <exception cref="KeyNotFoundException">
		/// The property is retrieved and key does not exist in the collection.
		/// </exception>
		public string this[string key]
		{
			get
			{
				if (key is null)
					throw new ArgumentNullException(nameof(key));

				fixed (char* k = key)
				{
					cef_string_t s0 = new cef_string_t { Str = k, Length = key.Length };
					cef_string_t s1 = new cef_string_t();
					int rv = CefNativeApi.cef_string_map_find(Instance, &s0, &s1);
					string value = CefString.ReadAndFree(&s1);
					if (rv == 0)
						throw new KeyNotFoundException();
					return value;
				}
			}

			set
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Gets the value of the entry at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the element to get.</param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="index"/> is less than 0;
		/// or, <paramref name="index"/> is equal to or greater than <see cref="Count"/>.
		/// </exception>
		public string this[int index]
		{
			get
			{
				if (index < 0)
					throw new ArgumentOutOfRangeException(nameof(index));

				cef_string_t s0 = new cef_string_t();
				int rv = CefNativeApi.cef_string_map_value(Instance, new UIntPtr((uint)index), &s0);
				string value = CefString.ReadAndFree(&s0);
				if (rv == 0)
					throw new ArgumentOutOfRangeException(nameof(index));
				return value;
			}
		}

		/// <summary>
		/// Gets a collection containing the keys in the string map.
		/// </summary>
		public ICollection<string> Keys
		{
			get
			{
				var keys = new string[Count];
				for (uint i = 0; i < keys.Length; i++)
				{
					var s = new cef_string_t();
					CefNativeApi.cef_string_map_key(_instance, new UIntPtr(i), &s);
					keys[i] = CefString.ReadAndFree(&s);
				}
				return keys;
			}
		}

		/// <summary>
		/// Gets a collection containing the values in the string map.
		/// </summary>
		public ICollection<string> Values
		{
			get
			{
				var values = new string[Count];
				for (uint i = 0; i < values.Length; i++)
				{
					var s = new cef_string_t();
					CefNativeApi.cef_string_map_value(_instance, new UIntPtr(i), &s);
					values[i] = CefString.ReadAndFree(&s);
				}
				return values;
			}
		}

		/// <summary>
		/// Gets the number of elements in the string map.
		/// </summary>
		public int Count
		{
			get
			{
				return (int)CefNativeApi.cef_string_map_size(Instance);
			}
		}

		/// <inheritdoc/>
		public bool IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Appends a new key/value pair at the end of the string map.
		/// </summary>
		/// <param name="key">The <see cref="string"/> key of the entry to add.</param>
		/// <param name="value">The <see cref="string"/> value of the entry to add.</param>
		/// <exception cref="ArgumentNullException">The key or the value is null.</exception>
		public void Add(string key, string value)
		{
			if (key is null)
				throw new ArgumentNullException(nameof(key));
			if (value is null)
				throw new ArgumentNullException(nameof(value));

			fixed (char* k = key)
			fixed (char* v = value)
			{
				cef_string_t s0 = new cef_string_t { Str = k, Length = key.Length };
				cef_string_t s1 = new cef_string_t { Str = v, Length = value.Length };
				CefNativeApi.cef_string_map_append(Instance, &s0, &s1);
			}
		}

		/// <summary>
		/// Appends a new key/value pair at the end of the string map.
		/// </summary>
		/// <param name="item">The entry to add.</param>
		/// <exception cref="ArgumentOutOfRangeException">The key or the value is null.</exception>
		public void Add(KeyValuePair<string, string> item)
		{
			if (item.Key is null || item.Value is null)
				throw new ArgumentOutOfRangeException(nameof(item));

			fixed (char* k = item.Key)
			fixed (char* v = item.Value)
			{
				cef_string_t s0 = new cef_string_t { Str = k, Length = item.Key.Length };
				cef_string_t s1 = new cef_string_t { Str = v, Length = item.Value.Length };
				CefNativeApi.cef_string_map_append(Instance, &s0, &s1);
			}
		}

		/// <summary>
		/// Clears the string map.
		/// </summary>
		public void Clear()
		{
			CefNativeApi.cef_string_map_clear(Instance);
		}

		/// <summary>
		/// Determines whether the string map contains the specified entry.
		/// </summary>
		/// <param name="item">The entry to locate in the string map.</param>
		/// <returns>
		/// true if the string map contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public bool Contains(KeyValuePair<string, string> item)
		{
			if (item.Key is null)
				throw new ArgumentOutOfRangeException(nameof(item));

			fixed (char* k = item.Key)
			{
				cef_string_t s0 = new cef_string_t { Str = k, Length = item.Key.Length };
				cef_string_t s1 = new cef_string_t();
				int rv = CefNativeApi.cef_string_map_find(Instance, &s0, &s1);
				string value = CefString.ReadAndFree(&s1);
				return rv != 0 && string.Equals(value, item.Value, StringComparison.CurrentCulture);
			}
		}

		/// <summary>
		/// Determines whether the string map contains the specified key.
		/// </summary>
		/// <param name="key">The key to locate in the string map.</param>
		/// <returns>
		/// true if the string map contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public bool ContainsKey(string key)
		{
			if (key is null)
				throw new ArgumentNullException(nameof(key));

			fixed (char* k = key)
			{
				cef_string_t s0 = new cef_string_t { Str = k, Length = key.Length };
				cef_string_t s1 = new cef_string_t();
				int rv = CefNativeApi.cef_string_map_find(Instance, &s0, &s1);
				CefString.Free(&s1);
				return rv != 0;
			}
		}

		/// <inheritdoc/>
		public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
		{
			if (array is null)
				throw new ArgumentNullException(nameof(array));
			if (arrayIndex < 0)
				throw new ArgumentOutOfRangeException(nameof(arrayIndex));

			int count = this.Count;
			if (array.Length < arrayIndex + count)
				throw new ArgumentException("The number of elements in this map is greater than the available space from arrayIndex to the end of the destination array.");

			for (int i = 0; i < count; i++)
			{
				array[arrayIndex + i] = GetInternal((uint)i);
			}
		}

		/// <summary>
		/// Returns the key/value pair at the specified index.
		/// </summary>
		/// <returns>The string key/value pair at the specified index.</returns>
		public KeyValuePair<string, string> Get(int index)
		{
			if (index < 0 || index >= this.Count)
				throw new ArgumentOutOfRangeException(nameof(index));
			return GetInternal((uint)index);
		}

		private KeyValuePair<string, string> GetInternal(uint index)
		{
			var key = new cef_string_t();
			CefNativeApi.cef_string_map_key(_instance, new UIntPtr(index), &key);
			var value = new cef_string_t();
			CefNativeApi.cef_string_map_value(_instance, new UIntPtr(index), &value);
			return new KeyValuePair<string, string>(CefString.ReadAndFree(&key), CefString.ReadAndFree(&value));
		}

		/// <inheritdoc/>
		public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			uint count = (uint)CefNativeApi.cef_string_map_size(Instance);
			for (uint i = 0; i < count; i++)
			{
				yield return GetInternal(i);
			}
		}

		/// <inheritdoc/>
		public bool Remove(string key)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc/>
		public bool Remove(KeyValuePair<string, string> item)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc/>
		public bool TryGetValue(string key, out string value)
		{
			if (key is null)
				throw new ArgumentNullException(nameof(key));

			fixed (char* k = key)
			{
				cef_string_t s0 = new cef_string_t { Str = k, Length = key.Length };
				cef_string_t s1 = new cef_string_t();
				int rv = CefNativeApi.cef_string_map_find(Instance, &s0, &s1);
				value = CefString.ReadAndFree(&s1);
				return rv != 0;
			}
		}

		/// <inheritdoc/>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Converts the supplied <see cref="CefStringMap"/> to the <see cref="cef_string_map_t"/>.
		/// </summary>
		/// <param name="instance">The <see cref="CefStringMap"/> object to be converted.</param>
		/// <returns>A <see cref="cef_string_map_t"/> that represented by the <see cref="CefStringMap"/> parameter.</returns>
		public static implicit operator cef_string_map_t(CefStringMap instance)
		{
			return instance is null ? default : instance._instance;
		}

		/// <summary>
		/// Creates a new <see cref="CefStringMap"/> object initialized to a specified string map instance.
		/// </summary>
		/// <param name="instance">A string map instance.</param>
		public static implicit operator CefStringMap(cef_string_map_t instance)
		{
			return instance.Base == null ? null : new CefStringMap(instance);
		}
	}
}
