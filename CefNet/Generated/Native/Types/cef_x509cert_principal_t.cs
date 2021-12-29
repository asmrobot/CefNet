﻿// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------
// Generated by CefGen
// Source: include/capi/cef_x509_certificate_capi.h
// --------------------------------------------------------------------------------------------﻿
// DO NOT MODIFY! THIS IS AUTOGENERATED FILE!
// --------------------------------------------------------------------------------------------

#pragma warning disable 0169, 1591, 1573

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using CefNet.WinApi;

namespace CefNet.CApi
{
	/// <summary>
	/// Structure representing the issuer or subject field of an X.509 certificate.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct cef_x509cert_principal_t
	{
		/// <summary>
		/// Base structure.
		/// </summary>
		public cef_base_ref_counted_t @base;

		/// <summary>
		/// cef_string_userfree_t (*)(_cef_x509cert_principal_t* self)*
		/// </summary>
		public void* get_display_name;

		/// <summary>
		/// Returns a name that can be used to represent the issuer. It tries in this
		/// order: Common Name (CN), Organization Name (O) and Organizational Unit Name
		/// (OU) and returns the first non-NULL one found.
		/// The resulting string must be freed by calling cef_string_userfree_free().
		/// </summary>
		[NativeName("get_display_name")]
		public unsafe cef_string_userfree_t GetDisplayName()
		{
			fixed (cef_x509cert_principal_t* self = &this)
			{
				return ((delegate* unmanaged[Stdcall]<cef_x509cert_principal_t*, cef_string_userfree_t>)get_display_name)(self);
			}
		}

		/// <summary>
		/// cef_string_userfree_t (*)(_cef_x509cert_principal_t* self)*
		/// </summary>
		public void* get_common_name;

		/// <summary>
		/// Returns the common name.
		/// The resulting string must be freed by calling cef_string_userfree_free().
		/// </summary>
		[NativeName("get_common_name")]
		public unsafe cef_string_userfree_t GetCommonName()
		{
			fixed (cef_x509cert_principal_t* self = &this)
			{
				return ((delegate* unmanaged[Stdcall]<cef_x509cert_principal_t*, cef_string_userfree_t>)get_common_name)(self);
			}
		}

		/// <summary>
		/// cef_string_userfree_t (*)(_cef_x509cert_principal_t* self)*
		/// </summary>
		public void* get_locality_name;

		/// <summary>
		/// Returns the locality name.
		/// The resulting string must be freed by calling cef_string_userfree_free().
		/// </summary>
		[NativeName("get_locality_name")]
		public unsafe cef_string_userfree_t GetLocalityName()
		{
			fixed (cef_x509cert_principal_t* self = &this)
			{
				return ((delegate* unmanaged[Stdcall]<cef_x509cert_principal_t*, cef_string_userfree_t>)get_locality_name)(self);
			}
		}

		/// <summary>
		/// cef_string_userfree_t (*)(_cef_x509cert_principal_t* self)*
		/// </summary>
		public void* get_state_or_province_name;

		/// <summary>
		/// Returns the state or province name.
		/// The resulting string must be freed by calling cef_string_userfree_free().
		/// </summary>
		[NativeName("get_state_or_province_name")]
		public unsafe cef_string_userfree_t GetStateOrProvinceName()
		{
			fixed (cef_x509cert_principal_t* self = &this)
			{
				return ((delegate* unmanaged[Stdcall]<cef_x509cert_principal_t*, cef_string_userfree_t>)get_state_or_province_name)(self);
			}
		}

		/// <summary>
		/// cef_string_userfree_t (*)(_cef_x509cert_principal_t* self)*
		/// </summary>
		public void* get_country_name;

		/// <summary>
		/// Returns the country name.
		/// The resulting string must be freed by calling cef_string_userfree_free().
		/// </summary>
		[NativeName("get_country_name")]
		public unsafe cef_string_userfree_t GetCountryName()
		{
			fixed (cef_x509cert_principal_t* self = &this)
			{
				return ((delegate* unmanaged[Stdcall]<cef_x509cert_principal_t*, cef_string_userfree_t>)get_country_name)(self);
			}
		}

		/// <summary>
		/// void (*)(_cef_x509cert_principal_t* self, cef_string_list_t addresses)*
		/// </summary>
		public void* get_street_addresses;

		/// <summary>
		/// Retrieve the list of street addresses.
		/// </summary>
		[NativeName("get_street_addresses")]
		public unsafe void GetStreetAddresses(cef_string_list_t addresses)
		{
			fixed (cef_x509cert_principal_t* self = &this)
			{
				((delegate* unmanaged[Stdcall]<cef_x509cert_principal_t*, cef_string_list_t, void>)get_street_addresses)(self, addresses);
			}
		}

		/// <summary>
		/// void (*)(_cef_x509cert_principal_t* self, cef_string_list_t names)*
		/// </summary>
		public void* get_organization_names;

		/// <summary>
		/// Retrieve the list of organization names.
		/// </summary>
		[NativeName("get_organization_names")]
		public unsafe void GetOrganizationNames(cef_string_list_t names)
		{
			fixed (cef_x509cert_principal_t* self = &this)
			{
				((delegate* unmanaged[Stdcall]<cef_x509cert_principal_t*, cef_string_list_t, void>)get_organization_names)(self, names);
			}
		}

		/// <summary>
		/// void (*)(_cef_x509cert_principal_t* self, cef_string_list_t names)*
		/// </summary>
		public void* get_organization_unit_names;

		/// <summary>
		/// Retrieve the list of organization unit names.
		/// </summary>
		[NativeName("get_organization_unit_names")]
		public unsafe void GetOrganizationUnitNames(cef_string_list_t names)
		{
			fixed (cef_x509cert_principal_t* self = &this)
			{
				((delegate* unmanaged[Stdcall]<cef_x509cert_principal_t*, cef_string_list_t, void>)get_organization_unit_names)(self, names);
			}
		}

		/// <summary>
		/// void (*)(_cef_x509cert_principal_t* self, cef_string_list_t components)*
		/// </summary>
		public void* get_domain_components;

		/// <summary>
		/// Retrieve the list of domain components.
		/// </summary>
		[NativeName("get_domain_components")]
		public unsafe void GetDomainComponents(cef_string_list_t components)
		{
			fixed (cef_x509cert_principal_t* self = &this)
			{
				((delegate* unmanaged[Stdcall]<cef_x509cert_principal_t*, cef_string_list_t, void>)get_domain_components)(self, components);
			}
		}
	}
}
