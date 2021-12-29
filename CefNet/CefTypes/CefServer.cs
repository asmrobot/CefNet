using CefNet.CApi;
using System;
using System.Net;
using System.Net.Sockets;

namespace CefNet
{
	public unsafe partial class CefServer
	{
		/// <summary>
		/// Create a new server that binds to |ipString| and |port|. A new thread will
		/// be created for each CreateServer call (the &quot;dedicated server thread&quot;).
		/// See CefServerHandler::OnServerCreated documentation for a description of
		/// server lifespan.
		/// </summary>
		/// <param name="ipString">
		/// A valid IPv4 or IPv6 address (e.g. 127.0.0.1 or ::1).
		/// </param>
		/// <param name="port">
		/// A port number outside of the reserved range (e.g. between 1025 and 65535 on most platforms).
		/// </param>
		/// <param name="backlog">
		/// The maximum number of pending connections.
		/// </param>
		/// <param name="handler">
		/// It is therefore recommended to use a different CefServerHandler instance for
		/// each CreateServer call to avoid thread safety issues in the CefServerHandler
		/// implementation. The CefServerHandler::OnServerCreated function will be called
		/// on the dedicated server thread to report success or failure.
		/// </param>
		public static void Create(string ipString, int port, int backlog, CefServerHandler handler)
		{
			IPAddress address = IPAddress.Parse(ipString);
			if (address.AddressFamily != AddressFamily.InterNetwork
				&& address.AddressFamily != AddressFamily.InterNetworkV6)
			{
				throw new ArgumentOutOfRangeException(nameof(ipString));
			}

			if (port <= 0 || port > ushort.MaxValue)
				throw new ArgumentOutOfRangeException(nameof(port));

			if (handler == null)
				throw new ArgumentNullException(nameof(handler));

			fixed (char* s0 = ipString)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = ipString.Length };
				CefNativeApi.cef_server_create(&cstr0, unchecked((ushort)port), backlog, handler.GetNativeInstance());
			}
		}

		/// <summary>
		/// Create a new server that binds to |ip| and |port|. A new thread will
		/// be created for each CreateServer call (the &quot;dedicated server thread&quot;).
		/// See CefServerHandler::OnServerCreated documentation for a description of
		/// server lifespan.
		/// </summary>
		/// <param name="ipString">
		/// A valid IPv4 or IPv6 address (e.g. 127.0.0.1 or ::1).
		/// </param>
		/// <param name="port">
		/// A port number outside of the reserved range (e.g. between 1025 and 65535 on most platforms).
		/// </param>
		/// <param name="backlog">
		/// The maximum number of pending connections.
		/// </param>
		/// <param name="handler">
		/// It is therefore recommended to use a different CefServerHandler instance for
		/// each CreateServer call to avoid thread safety issues in the CefServerHandler
		/// implementation. The CefServerHandler::OnServerCreated function will be called
		/// on the dedicated server thread to report success or failure.
		/// </param>
		public static void Create(IPAddress ip, int port, int backlog, CefServerHandler handler)
		{
			if (ip == null)
				throw new ArgumentNullException(nameof(ip));

			if (ip.AddressFamily != AddressFamily.InterNetwork
				&& ip.AddressFamily != AddressFamily.InterNetworkV6)
			{
				throw new ArgumentOutOfRangeException(nameof(ip));
			}

			if (port <= 0 || port > ushort.MaxValue)
				throw new ArgumentOutOfRangeException(nameof(port));

			if (handler == null)
				throw new ArgumentNullException(nameof(handler));

			string ipString = ip.ToString();
			fixed (char* s0 = ipString)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = ipString.Length };
				CefNativeApi.cef_server_create(&cstr0, unchecked((ushort)port), backlog, handler.GetNativeInstance());
			}
		}

		/// <summary>
		/// Create a new server that binds to |endpoint|. A new thread will
		/// be created for each CreateServer call (the &quot;dedicated server thread&quot;).
		/// See CefServerHandler::OnServerCreated documentation for a description of
		/// server lifespan.
		/// </summary>
		/// <param name="endpoint">
		/// A valid IPv4 or IPv6 address (e.g. 127.0.0.1 or ::1).
		/// </param>
		/// <param name="backlog">
		/// The maximum number of pending connections.
		/// </param>
		/// <param name="handler">
		/// It is therefore recommended to use a different CefServerHandler instance for
		/// each CreateServer call to avoid thread safety issues in the CefServerHandler
		/// implementation. The CefServerHandler::OnServerCreated function will be called
		/// on the dedicated server thread to report success or failure.
		/// </param>
		public static void Create(IPEndPoint endpoint, int backlog, CefServerHandler handler)
		{
			if (endpoint == null)
				throw new ArgumentNullException(nameof(endpoint));

			if (endpoint.AddressFamily == AddressFamily.InterNetwork
				&& endpoint.AddressFamily != AddressFamily.InterNetworkV6)
			{
				throw new ArgumentOutOfRangeException(nameof(endpoint));
			}

			if (handler == null)
				throw new ArgumentNullException(nameof(handler));

			string address = endpoint.Address.ToString();
			fixed (char* s0 = address)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = address.Length };
				CefNativeApi.cef_server_create(&cstr0, unchecked((ushort)endpoint.Port), backlog, handler.GetNativeInstance());
			}
		}

	}
}
