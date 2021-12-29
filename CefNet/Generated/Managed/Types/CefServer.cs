﻿// --------------------------------------------------------------------------------------------
// Copyright (c) 2019 The CefNet Authors. All rights reserved.
// Licensed under the MIT license.
// See the licence file in the project root for full license information.
// --------------------------------------------------------------------------------------------
// Generated by CefGen
// Source: Generated/Native/Types/cef_server_t.cs
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
	/// Structure representing a server that supports HTTP and WebSocket requests.
	/// Server capacity is limited and is intended to handle only a small number of
	/// simultaneous connections (e.g. for communicating between applications on
	/// localhost). The functions of this structure are safe to call from any thread
	/// in the brower process unless otherwise indicated.
	/// </summary>
	/// <remarks>
	/// Role: Proxy
	/// </remarks>
	public unsafe partial class CefServer : CefBaseRefCounted<cef_server_t>
	{
		internal static unsafe CefServer Create(IntPtr instance)
		{
			return new CefServer((cef_server_t*)instance);
		}

		public CefServer(cef_server_t* instance)
			: base((cef_base_ref_counted_t*)instance)
		{
		}

		/// <summary>
		/// Gets the task runner for the dedicated server thread.
		/// </summary>
		public unsafe virtual CefTaskRunner TaskRunner
		{
			get
			{
				return SafeCall(CefTaskRunner.Wrap(CefTaskRunner.Create, NativeInstance->GetTaskRunner()));
			}
		}

		/// <summary>
		/// Gets a value indicating whether the server is currently running and accepting incoming
		/// connections. See cef_server_handler_t::OnServerCreated documentation for a
		/// description of server lifespan. This property must be called on the
		/// dedicated server thread.
		/// </summary>
		public unsafe virtual bool IsRunning
		{
			get
			{
				return SafeCall(NativeInstance->IsRunning() != 0);
			}
		}

		/// <summary>
		/// Gets the server address including the port number.
		/// The resulting string must be freed by calling cef_string_userfree_free().
		/// </summary>
		public unsafe virtual string Address
		{
			get
			{
				return SafeCall(CefString.ReadAndFree(NativeInstance->GetAddress()));
			}
		}

		/// <summary>
		/// Gets a value indicating whether the server currently has a connection. This property
		/// must be called on the dedicated server thread.
		/// </summary>
		public unsafe virtual bool HasConnection
		{
			get
			{
				return SafeCall(NativeInstance->HasConnection() != 0);
			}
		}

		/// <summary>
		/// Stop the server and shut down the dedicated server thread. See
		/// cef_server_handler_t::OnServerCreated documentation for a description of
		/// server lifespan.
		/// </summary>
		public unsafe virtual void Shutdown()
		{
			NativeInstance->Shutdown();
			GC.KeepAlive(this);
		}

		/// <summary>
		/// Returns true (1) if |connection_id| represents a valid connection. This
		/// function must be called on the dedicated server thread.
		/// </summary>
		public unsafe virtual bool IsValidConnection(int connectionId)
		{
			return SafeCall(NativeInstance->IsValidConnection(connectionId) != 0);
		}

		/// <summary>
		/// Send an HTTP 200 &quot;OK&quot; response to the connection identified by
		/// |connection_id|. |content_type| is the response content type (e.g.
		/// &quot;text/html&quot;), |data| is the response content, and |data_size| is the size
		/// of |data| in bytes. The contents of |data| will be copied. The connection
		/// will be closed automatically after the response is sent.
		/// </summary>
		public unsafe virtual void SendHttp200response(int connectionId, string contentType, IntPtr data, long dataSize)
		{
			fixed (char* s1 = contentType)
			{
				var cstr1 = new cef_string_t { Str = s1, Length = contentType != null ? contentType.Length : 0 };
				NativeInstance->SendHttp200response(connectionId, &cstr1, (void*)data, new UIntPtr((ulong)dataSize));
			}
			GC.KeepAlive(this);
		}

		/// <summary>
		/// Send an HTTP 404 &quot;Not Found&quot; response to the connection identified by
		/// |connection_id|. The connection will be closed automatically after the
		/// response is sent.
		/// </summary>
		public unsafe virtual void SendHttp404response(int connectionId)
		{
			NativeInstance->SendHttp404response(connectionId);
			GC.KeepAlive(this);
		}

		/// <summary>
		/// Send an HTTP 500 &quot;Internal Server Error&quot; response to the connection
		/// identified by |connection_id|. |error_message| is the associated error
		/// message. The connection will be closed automatically after the response is
		/// sent.
		/// </summary>
		public unsafe virtual void SendHttp500response(int connectionId, string errorMessage)
		{
			fixed (char* s1 = errorMessage)
			{
				var cstr1 = new cef_string_t { Str = s1, Length = errorMessage != null ? errorMessage.Length : 0 };
				NativeInstance->SendHttp500response(connectionId, &cstr1);
			}
			GC.KeepAlive(this);
		}

		/// <summary>
		/// Send a custom HTTP response to the connection identified by
		/// |connection_id|. |response_code| is the HTTP response code sent in the
		/// status line (e.g. 200), |content_type| is the response content type sent as
		/// the &quot;Content-Type&quot; header (e.g. &quot;text/html&quot;), |content_length| is the
		/// expected content length, and |extra_headers| is the map of extra response
		/// headers. If |content_length| is &gt;= 0 then the &quot;Content-Length&quot; header will
		/// be sent. If |content_length| is 0 then no content is expected and the
		/// connection will be closed automatically after the response is sent. If
		/// |content_length| is
		/// &lt;
		/// 0 then no &quot;Content-Length&quot; header will be sent and
		/// the client will continue reading until the connection is closed. Use the
		/// SendRawData function to send the content, if applicable, and call
		/// CloseConnection after all content has been sent.
		/// </summary>
		public unsafe virtual void SendHttpResponse(int connectionId, int responseCode, string contentType, long contentLength, CefStringMultimap extraHeaders)
		{
			fixed (char* s2 = contentType)
			{
				var cstr2 = new cef_string_t { Str = s2, Length = contentType != null ? contentType.Length : 0 };
				NativeInstance->SendHttpResponse(connectionId, responseCode, &cstr2, contentLength, extraHeaders);
			}
			GC.KeepAlive(this);
		}

		/// <summary>
		/// Send raw data directly to the connection identified by |connection_id|.
		/// |data| is the raw data and |data_size| is the size of |data| in bytes. The
		/// contents of |data| will be copied. No validation of |data| is performed
		/// internally so the client should be careful to send the amount indicated by
		/// the &quot;Content-Length&quot; header, if specified. See SendHttpResponse
		/// documentation for intended usage.
		/// </summary>
		public unsafe virtual void SendRawData(int connectionId, IntPtr data, long dataSize)
		{
			NativeInstance->SendRawData(connectionId, (void*)data, new UIntPtr((ulong)dataSize));
			GC.KeepAlive(this);
		}

		/// <summary>
		/// Close the connection identified by |connection_id|. See SendHttpResponse
		/// documentation for intended usage.
		/// </summary>
		public unsafe virtual void CloseConnection(int connectionId)
		{
			NativeInstance->CloseConnection(connectionId);
			GC.KeepAlive(this);
		}

		/// <summary>
		/// Send a WebSocket message to the connection identified by |connection_id|.
		/// |data| is the response content and |data_size| is the size of |data| in
		/// bytes. The contents of |data| will be copied. See
		/// cef_server_handler_t::OnWebSocketRequest documentation for intended usage.
		/// </summary>
		public unsafe virtual void SendWebSocketMessage(int connectionId, IntPtr data, long dataSize)
		{
			NativeInstance->SendWebSocketMessage(connectionId, (void*)data, new UIntPtr((ulong)dataSize));
			GC.KeepAlive(this);
		}
	}
}
