﻿// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------
// Generated by CefGen
// Source: Generated/Native/Types/cef_print_settings_t.cs
// --------------------------------------------------------------------------------------------﻿
// DO NOT MODIFY! THIS IS AUTOGENERATED FILE!
// --------------------------------------------------------------------------------------------

#pragma warning disable 0169, 1591, 1573

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using CefNet.WinApi;
using CefNet.CApi;
using CefNet.Internal;

namespace CefNet
{
	/// <summary>
	/// Structure representing print settings.
	/// </summary>
	/// <remarks>
	/// Role: Proxy
	/// </remarks>
	public unsafe partial class CefPrintSettings : CefBaseRefCounted<cef_print_settings_t>
	{
		internal static unsafe CefPrintSettings Create(IntPtr instance)
		{
			return new CefPrintSettings((cef_print_settings_t*)instance);
		}

		public CefPrintSettings(cef_print_settings_t* instance)
			: base((cef_base_ref_counted_t*)instance)
		{
		}

		/// <summary>
		/// Gets a value indicating whether this object is valid. Do not call any other functions
		/// if this property returns false.
		/// </summary>
		public unsafe virtual bool IsValid
		{
			get
			{
				return SafeCall(NativeInstance->IsValid() != 0);
			}
		}

		/// <summary>
		/// Gets a value indicating whether the values of this object are read-only. Some APIs may
		/// expose read-only objects.
		/// </summary>
		public unsafe virtual bool IsReadOnly
		{
			get
			{
				return SafeCall(NativeInstance->IsReadOnly() != 0);
			}
		}

		/// <summary>
		/// Gets a value indicating whether the orientation is landscape.
		/// </summary>
		public unsafe virtual bool IsLandscape
		{
			get
			{
				return SafeCall(NativeInstance->IsLandscape() != 0);
			}
		}

		/// <summary>
		/// Gets and sets the device name.
		/// The resulting string must be freed by calling cef_string_userfree_free().
		/// </summary>
		public unsafe virtual string DeviceName
		{
			get
			{
				return SafeCall(CefString.ReadAndFree(NativeInstance->GetDeviceName()));
			}
			set
			{
				fixed (char* s0 = value)
				{
					var cstr0 = new cef_string_t { Str = s0, Length = value != null ? value.Length : 0 };
					NativeInstance->SetDeviceName(&cstr0);
				}
				GC.KeepAlive(this);
			}
		}

		/// <summary>
		/// Gets and sets the DPI (dots per inch).
		/// </summary>
		public unsafe virtual int Dpi
		{
			get
			{
				return SafeCall(NativeInstance->GetDpi());
			}
			set
			{
				NativeInstance->SetDpi(value);
				GC.KeepAlive(this);
			}
		}

		/// <summary>
		/// Gets the number of page ranges that currently exist.
		/// </summary>
		public unsafe virtual long PageRangesCount
		{
			get
			{
				return SafeCall((long)NativeInstance->GetPageRangesCount());
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether only the selection will be printed.
		/// </summary>
		public unsafe virtual bool SelectionOnly
		{
			get
			{
				return SafeCall(NativeInstance->IsSelectionOnly() != 0);
			}
			set
			{
				NativeInstance->SetSelectionOnly(value ? 1 : 0);
				GC.KeepAlive(this);
			}
		}

		/// <summary>
		/// Gets and sets the color model.
		/// </summary>
		public unsafe virtual CefColorModel ColorModel
		{
			get
			{
				return SafeCall(NativeInstance->GetColorModel());
			}
			set
			{
				NativeInstance->SetColorModel(value);
				GC.KeepAlive(this);
			}
		}

		/// <summary>
		/// Gets and sets the number of copies.
		/// </summary>
		public unsafe virtual int Copies
		{
			get
			{
				return SafeCall(NativeInstance->GetCopies());
			}
			set
			{
				NativeInstance->SetCopies(value);
				GC.KeepAlive(this);
			}
		}

		/// <summary>
		/// Gets and sets the duplex mode.
		/// </summary>
		public unsafe virtual CefDuplexMode DuplexMode
		{
			get
			{
				return SafeCall(NativeInstance->GetDuplexMode());
			}
			set
			{
				NativeInstance->SetDuplexMode(value);
				GC.KeepAlive(this);
			}
		}

		/// <summary>
		/// Set the printer printable area in device units. Some platforms already
		/// provide flipped area. Set |landscape_needs_flip| to false (0) on those
		/// platforms to avoid double flipping.
		/// </summary>
		public unsafe virtual void SetPrinterPrintableArea(CefSize physicalSizeDeviceUnits, CefRect printableAreaDeviceUnits, bool landscapeNeedsFlip)
		{
			NativeInstance->SetPrinterPrintableArea((cef_size_t*)&physicalSizeDeviceUnits, (cef_rect_t*)&printableAreaDeviceUnits, landscapeNeedsFlip ? 1 : 0);
			GC.KeepAlive(this);
		}

		/// <summary>
		/// Set the page ranges.
		/// </summary>
		public unsafe virtual void SetPageRanges(CefRange[] ranges)
		{
			fixed (CefRange* p1 = ranges)
			{
				NativeInstance->SetPageRanges(new UIntPtr((uint)ranges.Length), (cef_range_t*)p1);
			}
			GC.KeepAlive(this);
		}

		/// <summary>
		/// Retrieve the page ranges.
		/// </summary>
		public unsafe virtual void GetPageRanges(ref long rangesCount, ref CefRange[] ranges)
		{
			fixed (CefRange* p1 = ranges)
			{
				UIntPtr c1 = new UIntPtr((uint)ranges.Length);
				NativeInstance->GetPageRanges(&c1, (cef_range_t*)p1);
				rangesCount = (long)c1;
				Array.Resize(ref ranges, (int)c1);
			}
			GC.KeepAlive(this);
		}

		/// <summary>
		/// Returns true (1) if pages will be collated.
		/// </summary>
		public unsafe virtual bool WillCollate()
		{
			return SafeCall(NativeInstance->WillCollate() != 0);
		}

		/// <summary>
		/// Set the page orientation.
		/// </summary>
		public unsafe virtual void SetOrientation(bool landscape)
		{
			NativeInstance->SetOrientation(landscape ? 1 : 0);
			GC.KeepAlive(this);
		}

		/// <summary>
		/// Set whether pages will be collated.
		/// </summary>
		public unsafe virtual void SetCollate(bool collate)
		{
			NativeInstance->SetCollate(collate ? 1 : 0);
			GC.KeepAlive(this);
		}
	}
}
