// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

using CefNet.CApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CefNet
{
	/// <summary>
	/// Base class for all wrapper classes for scoped CEF structs.
	/// </summary>
	/// <typeparam name="T">A scoped CEF struct.</typeparam>
	public abstract class CefBaseScoped<T> : Internal.CefBaseScopedImpl
		where T : unmanaged
	{
		/// <summary>
		/// Initializes a new instance of <see cref="CefBaseScoped{T}"/>.
		/// </summary>
		public unsafe CefBaseScoped()
			: base(Allocate(sizeof(T)))
		{
			lock (Scope)
			{
				Scope.Add((IntPtr)_instance, this);
			}
		}

		/// <summary>
		/// Initializes a new instance of <see cref="CefBaseScoped{T}"/> using
		/// the specified pointer to a specified CEF struct.
		/// </summary>
		/// <param name="instance">The pointer to a specified CEF struct.</param>
		public unsafe CefBaseScoped(cef_base_scoped_t* instance)
			: base(instance)
		{

		}

		/// <summary>
		/// Returns a wrapper for the specified pointer.
		/// </summary>
		/// <typeparam name="TClass">The type of wrapper.</typeparam>
		/// <param name="create">Represents a method that create a new wrapper.</param>
		/// <param name="instance">The pointer to scoped CEF struct.</param>
		/// <returns>An existing or new wrapper for the specified pointer.</returns>
		public unsafe static TClass Wrap<TClass>(Func<IntPtr, TClass> create, T* instance)
		{
			return create(unchecked((IntPtr)instance));
		}

		/// <summary>
		/// Gets an unsafe pointer to a specified CEF struct.
		/// </summary>
		public new unsafe T* NativeInstance
		{
			get
			{
				return (T*)_instance;
			}
		}

		/// <summary>
		/// Returns an unsafe pointer to a specified CEF struct.
		/// </summary>
		/// <returns>
		/// An unsafe pointer to a specified CEF struct.
		/// </returns>
		public new unsafe T* GetNativeInstance()
		{
			return (T*)_instance;
		}

		/// <summary>
		/// Returns an instance of a type that represents a <see cref="CefBaseScoped"/>
		/// object by an unspecified pointer.
		/// </summary>
		/// <param name="ptr">A pointer to a scoped struct.</param>
		/// <returns>
		/// A <see cref="CefBaseScoped"/>-based object that corresponds to the pointer
		/// or null if wrapper not found.
		/// </returns>
		public static CefBaseScoped GetInstance(IntPtr ptr)
		{
			lock (Scope)
			{
				if (Scope.TryGetValue(ptr, out CefBaseScoped instance))
					return instance;
			}
			return null;
		}

#pragma warning disable CS1591
		protected unsafe override void Dispose(bool disposing)
		{
			IntPtr mem = (IntPtr)_instance;
			bool alive;
			lock (Scope)
			{
				alive = Scope.Remove(mem);
			}
			if (alive)
			{
				CefStructure.Free(mem);
				_instance = null;
			}
		}
#pragma warning restore CS1591

	}

}

namespace CefNet.Internal
{
	public unsafe abstract class CefBaseScopedImpl : CefBaseScoped
	{
#if NET_LESS_5_0
		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		private unsafe delegate void CefActionDelegate(cef_base_scoped_t* self);

		private static readonly unsafe CefActionDelegate fnDel = DelImpl;
#endif
		private protected static readonly Dictionary<IntPtr, CefBaseScoped> Scope = new Dictionary<IntPtr, CefBaseScoped>();

		internal unsafe CefBaseScopedImpl(cef_base_scoped_t* instance)
			: base(instance)
		{

		}

		private protected unsafe static cef_base_scoped_t* Allocate(int size)
		{
			cef_base_scoped_t* instance = (cef_base_scoped_t*)CefStructure.Allocate(size);
#if NET_LESS_5_0
			instance->del = (void*)Marshal.GetFunctionPointerForDelegate(fnDel);
#else
			instance->del = (delegate* unmanaged[Stdcall]<cef_base_scoped_t*, void>)&DelImpl;
#endif
			return instance;
		}

#if !NET_LESS_5_0
		[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
#endif
		private unsafe static void DelImpl(cef_base_scoped_t* self)
		{
			CefBaseScoped instance;
			lock (Scope)
			{
				Scope.TryGetValue((IntPtr)self, out instance);
			}
			((IDisposable)instance)?.Dispose();
		}

	}
}

namespace CefNet
{ 

	/// <summary>
	/// Base class for all wrapper classes for scoped CEF structs.
	/// </summary>
	public unsafe abstract class CefBaseScoped : IDisposable
	{
		private protected cef_base_scoped_t* _instance;

		/// <summary>
		/// Initializes a new instance of <see cref="CefBaseScoped"/> using
		/// the pointer to scaped CEF struct.
		/// </summary>
		/// <param name="instance">The pointer to a scoped CEF struct.</param>
		/// <exception cref="NullReferenceException"><paramref name="instance"/> is null.</exception>
		public CefBaseScoped(cef_base_scoped_t* instance)
		{
			if (instance == null)
				throw new ArgumentNullException(nameof(instance));
			_instance = instance;
		}

#pragma warning disable CS1591 // Missing comments
		~CefBaseScoped()
		{
			Dispose(false);
		}

		protected abstract void Dispose(bool disposing);

		void IDisposable.Dispose()
		{
			Dispose(true);
		}
#pragma warning restore CS1591 // Missing comments

		/// <summary>
		/// Gets an unsafe pointer to a scoped struct.
		/// </summary>
		public unsafe cef_base_scoped_t* NativeInstance
		{
			get
			{
				return _instance;
			}
		}

		/// <summary>
		/// Returns an unsafe pointer to a scoped struct.
		/// </summary>
		/// <returns>
		/// An unsafe pointer to a scoped struct.
		/// </returns>
		public unsafe cef_base_scoped_t* GetNativeInstance()
		{
			return _instance;
		}

		/// <summary>
		/// Deletes this object.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void Del()
		{
			if (_instance->del != null)
				_instance->Del();
		}

		/// <summary>
		/// Makes himself ineligible for garbage collection from the start of the current routine
		/// to the point where this method is called (like &apos;GC.KeepAlive(this)&apos;) and
		/// returns passed <paramref name="result"/>.
		/// </summary>
		/// <typeparam name="T">Any type.</typeparam>
		/// <param name="result">Any value of <typeparamref name="T"/> type.</param>
		/// <returns>Returns the passed parameter of <typeparamref name="T"/> type.</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public T SafeCall<T>(T result)
		{
			return result;
		}
	}
}
