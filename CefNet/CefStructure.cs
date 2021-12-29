// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace CefNet
{
	internal static class CefStructure
	{
		private static readonly HashSet<IntPtr> MemorySet = new HashSet<IntPtr>();

		[StructLayout(LayoutKind.Sequential)]
		private struct cef_base_sized_t
		{
			public UIntPtr size;
		}

		public static bool IsAllocated(IntPtr ptr)
		{
			lock (MemorySet)
			{
				return MemorySet.Contains(ptr);
			}
		}

		public unsafe static IntPtr Allocate(int size)
		{
			unchecked
			{
				IntPtr mem = Marshal.AllocHGlobal(size);
				mem.InitBlock(0, size);
				((cef_base_sized_t*)mem)->size = (UIntPtr)size;
				lock (MemorySet)
				{
					MemorySet.Add(mem);
				}
				return mem;
			}
		}

		public static bool Free(IntPtr mem)
		{
			bool found;
			lock (MemorySet)
			{
				found = MemorySet.Remove(mem);
			}
			if (found)
			{
				Marshal.FreeHGlobal(mem);
				return true;
			}
			return false;
		}

	}
}
