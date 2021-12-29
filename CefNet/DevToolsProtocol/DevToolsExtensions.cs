using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CefNet.Internal;

namespace CefNet
{
	/// <summary>
	/// Provides methods for interaction over the DevTools protocol.
	/// </summary>
	public static class DevToolsExtensions
	{
		private static readonly Dictionary<long, DevToolsProtocolClient> _Clients = new Dictionary<long, DevToolsProtocolClient>();

		/// <summary>
		/// Returns a number that uniquely identifies the DevTools Protocol message. 
		/// </summary>
		/// <param name="browserHost">The browser instance.</param>
		/// <returns>A number that uniquely identifies the protocol message.</returns>
		public static int GetNextDevToolsMessageId(this CefBrowserHost browserHost)
		{
			return GetProtocolClient(browserHost).IncrementMessageId();
		}

		private static DevToolsProtocolClient GetProtocolClient(CefBrowserHost browserHost)
		{
			if (browserHost is null)
				throw new ArgumentNullException(nameof(browserHost));

			DevToolsProtocolClient client;
			long browserId = browserHost.Browser.Identifier;
			lock (_Clients)
			{
				if (!_Clients.TryGetValue(browserId, out client))
				{
					var webview = browserHost.Client.GetWebView() as IChromiumWebViewPrivate;
					if (webview is null)
						throw new InvalidOperationException("This browser is not associated with a WebView control.");
					client = new DevToolsProtocolClient(webview);
					_Clients.Add(browserId, client);
				}
			}
			return client;
		}

		internal static void ReleaseProtocolClient(long browserId)
		{
			DevToolsProtocolClient protocolClient;
			lock (_Clients)
			{
				_Clients.Remove(browserId, out protocolClient);
			}
			if (protocolClient is null)
				return;
			protocolClient.Close();
		}

		private static async Task<string> ExecuteDevToolsMethodInternalAsync(IChromiumWebView webview, string method, CefDictionaryValue parameters, CancellationToken cancellationToken)
		{
			CefBrowser browser = webview.BrowserObject;
			if (browser is null)
				throw new InvalidOperationException();

			cancellationToken.ThrowIfCancellationRequested();

			CefBrowserHost browserHost = browser.Host;
			DevToolsProtocolClient protocolClient = GetProtocolClient(browserHost);

			await CefNetSynchronizationContextAwaiter.GetForThread(CefThreadId.UI);
			cancellationToken.ThrowIfCancellationRequested();

			int messageId = browserHost.ExecuteDevToolsMethod(protocolClient.IncrementMessageId(), method, parameters);
			protocolClient.UpdateLastMessageId(messageId);
			DevToolsMethodResult r;
			if (cancellationToken.CanBeCanceled)
			{
				Task<DevToolsMethodResult> waitTask = protocolClient.WaitForMessageAsync(messageId, DevToolsCallCompletionSource.ConvertUtf8BufferToJsonString);
				await Task.WhenAny(waitTask, Task.Delay(Timeout.Infinite, cancellationToken)).ConfigureAwait(false);
				cancellationToken.ThrowIfCancellationRequested();
				r = waitTask.Result;
			}
			else
			{
				r = await protocolClient.WaitForMessageAsync(messageId, DevToolsCallCompletionSource.ConvertUtf8BufferToJsonString).ConfigureAwait(false);
			}
			if (r.Success)
			{
				if (r.Result is byte[] buff)
					return Encoding.UTF8.GetString(buff);
				return r.Result as string;
			}

			CefValue errorValue;
			if (r.Result is byte[] buffer)
				errorValue = CefApi.CefParseJSONBuffer(buffer, CefJsonParserOptions.AllowTrailingCommas);
			else
				errorValue = CefApi.CefParseJSON(r.Result as string, CefJsonParserOptions.AllowTrailingCommas);
			if (errorValue is null)
				throw new DevToolsProtocolException($"An unknown error occurred while trying to execute the '{method}' method.");
			throw new DevToolsProtocolException(errorValue.GetDictionary().GetString("message"));
		}

		private static async Task<object> ExecuteDevToolsMethodInternalAsync(IChromiumWebView webview, string method, CefDictionaryValue parameters, ConvertUtf8BufferToObjectDelegate convert, CancellationToken cancellationToken)
		{
			CefBrowser browser = webview.BrowserObject;
			if (browser is null)
				throw new InvalidOperationException();

			cancellationToken.ThrowIfCancellationRequested();

			CefBrowserHost browserHost = browser.Host;
			DevToolsProtocolClient protocolClient = GetProtocolClient(browserHost);

			await CefNetSynchronizationContextAwaiter.GetForThread(CefThreadId.UI);
			cancellationToken.ThrowIfCancellationRequested();

			int messageId = browserHost.ExecuteDevToolsMethod(protocolClient.IncrementMessageId(), method, parameters);
			protocolClient.UpdateLastMessageId(messageId);
			DevToolsMethodResult r;
			if (cancellationToken.CanBeCanceled)
			{
				Task<DevToolsMethodResult> waitTask = protocolClient.WaitForMessageAsync(messageId, convert);
				await Task.WhenAny(waitTask, Task.Delay(Timeout.Infinite, cancellationToken)).ConfigureAwait(false);
				cancellationToken.ThrowIfCancellationRequested();
				r = waitTask.Result;
			}
			else
			{
				r = await protocolClient.WaitForMessageAsync(messageId, convert).ConfigureAwait(false);
			}
			if (r.Success)
				return r.Result;

			CefValue errorValue;
			if (r.Result is byte[] buffer)
				errorValue = CefApi.CefParseJSONBuffer(buffer, CefJsonParserOptions.AllowTrailingCommas);
			else
				errorValue = CefApi.CefParseJSON(r.Result as string, CefJsonParserOptions.AllowTrailingCommas);
			if (errorValue is null)
				throw new DevToolsProtocolException($"An unknown error occurred while trying to execute the '{method}' method.");
			throw new DevToolsProtocolException(errorValue.GetDictionary().GetString("message"));
		}

		/// <summary>
		/// Executes a method call over the DevTools protocol.
		/// </summary>
		/// <param name="webview">The WebView control.</param>
		/// <param name="method">The method name.</param>
		/// <param name="parameters">
		/// The dictionaly with method parameters. May be null.
		/// See the <see href="https://chromedevtools.github.io/devtools-protocol/">
		/// DevTools Protocol documentation</see> for details of supported methods
		/// and the expected parameters.
		/// </param>
		/// <returns>
		/// The JSON string with the response. Structure of the response varies depending
		/// on the method name and is defined by the &apos;RETURN OBJECT&apos; section of
		/// the Chrome DevTools Protocol command description.
		/// </returns>
		/// <remarks>
		/// Usage of the ExecuteDevToolsMethodAsync function does not require an active
		/// DevTools front-end or remote-debugging session. Other active DevTools sessions
		/// will continue to function independently. However, any modification of global
		/// browser state by one session may not be reflected in the UI of other sessions.
		/// <para/>
		/// Communication with the DevTools front-end (when displayed) can be logged
		/// for development purposes by passing the `--devtools-protocol-log-
		/// file=&lt;path&gt;` command-line flag.
		/// </remarks>
		public static Task<string> ExecuteDevToolsMethodAsync(this IChromiumWebView webview, string method, CefDictionaryValue parameters)
		{
			return ExecuteDevToolsMethodAsync(webview, method, parameters, CancellationToken.None);
		}


		/// <summary>
		/// Executes a method call over the DevTools protocol.
		/// </summary>
		/// <param name="webview">The WebView control.</param>
		/// <param name="method">The method name.</param>
		/// <param name="parameters">
		/// The dictionaly with method parameters. May be null.
		/// See the <see href="https://chromedevtools.github.io/devtools-protocol/">
		/// DevTools Protocol documentation</see> for details of supported methods
		/// and the expected parameters.
		/// </param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		/// <returns>
		/// The JSON string with the response. Structure of the response varies depending
		/// on the method name and is defined by the &apos;RETURN OBJECT&apos; section of
		/// the Chrome DevTools Protocol command description.
		/// </returns>
		/// <remarks>
		/// Usage of the ExecuteDevToolsMethodAsync function does not require an active
		/// DevTools front-end or remote-debugging session. Other active DevTools sessions
		/// will continue to function independently. However, any modification of global
		/// browser state by one session may not be reflected in the UI of other sessions.
		/// <para/>
		/// Communication with the DevTools front-end (when displayed) can be logged
		/// for development purposes by passing the `--devtools-protocol-log-
		/// file=&lt;path&gt;` command-line flag.
		/// </remarks>
		public static Task<string> ExecuteDevToolsMethodAsync(this IChromiumWebView webview, string method, CefDictionaryValue parameters, CancellationToken cancellationToken)
		{
			if (webview is null)
				throw new ArgumentNullException(nameof(webview));

			if (method is null)
				throw new ArgumentNullException(nameof(method));

			method = method.Trim();
			if (method.Length == 0)
				throw new ArgumentOutOfRangeException(nameof(method));

			return ExecuteDevToolsMethodInternalAsync(webview, method, parameters, cancellationToken);
		}

		/// <summary>
		/// Executes a method call over the DevTools protocol.
		/// </summary>
		/// <typeparam name="T">The type of object to return.</typeparam>
		/// <param name="webview">The WebView control.</param>
		/// <param name="method">The method name.</param>
		/// <param name="parameters">
		/// The dictionaly with method parameters. May be null.
		/// See the <see href="https://chromedevtools.github.io/devtools-protocol/">
		/// DevTools Protocol documentation</see> for details of supported methods
		/// and the expected parameters.
		/// </param>
		/// <param name="convert">
		/// A callback function that converts the result content into an object of type <see cref="T"/>.
		/// If null, a byte array containing a UTF-8-encoded copy of the content is returned.
		/// </param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		/// <returns>
		/// The JSON string with the response. Structure of the response varies depending
		/// on the method name and is defined by the &apos;RETURN OBJECT&apos; section of
		/// the Chrome DevTools Protocol command description.
		/// </returns>
		/// <remarks>
		/// Usage of the ExecuteDevToolsMethodAsync function does not require an active
		/// DevTools front-end or remote-debugging session. Other active DevTools sessions
		/// will continue to function independently. However, any modification of global
		/// browser state by one session may not be reflected in the UI of other sessions.
		/// <para/>
		/// Communication with the DevTools front-end (when displayed) can be logged
		/// for development purposes by passing the `--devtools-protocol-log-
		/// file=&lt;path&gt;` command-line flag.
		/// </remarks>
		public static async Task<T> ExecuteDevToolsMethodAsync<T>(this IChromiumWebView webview, string method, CefDictionaryValue parameters, ConvertUtf8BufferToTypeDelegate<T> convert, CancellationToken cancellationToken)
			where T : class
		{
			if (webview is null)
				throw new ArgumentNullException(nameof(webview));

			if (method is null)
				throw new ArgumentNullException(nameof(method));

			method = method.Trim();
			if (method.Length == 0)
				throw new ArgumentOutOfRangeException(nameof(method));

			object result = await ExecuteDevToolsMethodInternalAsync(webview, method, parameters, (buffer, size) => convert(buffer, size), cancellationToken).ConfigureAwait(false);
			return (T)result;
		}

		/// <summary>
		/// Executes a method call over the DevTools protocol.
		/// </summary>
		/// <param name="webview">The WebView control.</param>
		/// <param name="method">The method name.</param>
		/// <param name="parameters">
		/// The JSON string with method parameters. May be null.
		/// See the <see href="https://chromedevtools.github.io/devtools-protocol/">
		/// DevTools Protocol documentation</see> for details of supported methods
		/// and the expected parameters.
		/// </param>
		/// <returns>
		/// The JSON string with the response. Structure of the response varies depending
		/// on the method name and is defined by the &apos;RETURN OBJECT&apos; section of
		/// the Chrome DevTools Protocol command description.
		/// </returns>
		/// <remarks>
		/// Usage of the ExecuteDevToolsMethodAsync function does not require an active
		/// DevTools front-end or remote-debugging session. Other active DevTools sessions
		/// will continue to function independently. However, any modification of global
		/// browser state by one session may not be reflected in the UI of other sessions.
		/// <para/>
		/// Communication with the DevTools front-end (when displayed) can be logged
		/// for development purposes by passing the `--devtools-protocol-log-
		/// file=&lt;path&gt;` command-line flag.
		/// </remarks>
		public static Task<string> ExecuteDevToolsMethodAsync(this IChromiumWebView webview, string method, string parameters)
		{
			return ExecuteDevToolsMethodAsync(webview, method, parameters, CancellationToken.None);
		}

		/// <summary>
		/// Executes a method call over the DevTools protocol.
		/// </summary>
		/// <param name="webview">The WebView control.</param>
		/// <param name="method">The method name.</param>
		/// <param name="parameters">
		/// The JSON string with method parameters. May be null.
		/// See the <see href="https://chromedevtools.github.io/devtools-protocol/">
		/// DevTools Protocol documentation</see> for details of supported methods
		/// and the expected parameters.
		/// </param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		/// <returns>
		/// The JSON string with the response. Structure of the response varies depending
		/// on the method name and is defined by the &apos;RETURN OBJECT&apos; section of
		/// the Chrome DevTools Protocol command description.
		/// </returns>
		/// <remarks>
		/// Usage of the ExecuteDevToolsMethodAsync function does not require an active
		/// DevTools front-end or remote-debugging session. Other active DevTools sessions
		/// will continue to function independently. However, any modification of global
		/// browser state by one session may not be reflected in the UI of other sessions.
		/// <para/>
		/// Communication with the DevTools front-end (when displayed) can be logged
		/// for development purposes by passing the `--devtools-protocol-log-
		/// file=&lt;path&gt;` command-line flag.
		/// </remarks>
		public static Task<string> ExecuteDevToolsMethodAsync(this IChromiumWebView webview, string method, string parameters, CancellationToken cancellationToken)
		{
			CefValue args = null;
			if (parameters != null)
			{
				args = CefApi.CefParseJSON(parameters, CefJsonParserOptions.AllowTrailingCommas, out string errorMessage);
				if (args is null)
					throw new ArgumentOutOfRangeException(nameof(parameters), errorMessage is null ? "An error occurred during JSON parsing." : errorMessage);
			}
			return ExecuteDevToolsMethodAsync(webview, method, args is null ? default(CefDictionaryValue) : args.GetDictionary(), cancellationToken);
		}

		/// <summary>
		/// Executes a method call over the DevTools protocol.
		/// </summary>
		/// <typeparam name="T">The type of object to return.</typeparam>
		/// <param name="webview">The WebView control.</param>
		/// <param name="method">The method name.</param>
		/// <param name="parameters">
		/// The JSON string with method parameters. May be null.
		/// See the <see href="https://chromedevtools.github.io/devtools-protocol/">
		/// DevTools Protocol documentation</see> for details of supported methods
		/// and the expected parameters.
		/// </param>
		/// <param name="convert">
		/// A callback function that converts the result content into an object of type <see cref="T"/>.
		/// If null, a byte array containing a UTF-8-encoded copy of the content is returned.
		/// </param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		/// <returns>
		/// The JSON string with the response. Structure of the response varies depending
		/// on the method name and is defined by the &apos;RETURN OBJECT&apos; section of
		/// the Chrome DevTools Protocol command description.
		/// </returns>
		/// <remarks>
		/// Usage of the ExecuteDevToolsMethodAsync function does not require an active
		/// DevTools front-end or remote-debugging session. Other active DevTools sessions
		/// will continue to function independently. However, any modification of global
		/// browser state by one session may not be reflected in the UI of other sessions.
		/// <para/>
		/// Communication with the DevTools front-end (when displayed) can be logged
		/// for development purposes by passing the `--devtools-protocol-log-
		/// file=&lt;path&gt;` command-line flag.
		/// </remarks>
		public static Task<T> ExecuteDevToolsMethodAsync<T>(this IChromiumWebView webview, string method, string parameters, ConvertUtf8BufferToTypeDelegate<T> convert, CancellationToken cancellationToken)
			where T : class
		{
			CefValue args = null;
			if (parameters != null)
			{
				args = CefApi.CefParseJSON(parameters, CefJsonParserOptions.AllowTrailingCommas, out string errorMessage);
				if (args is null)
					throw new ArgumentOutOfRangeException(nameof(parameters), errorMessage is null ? "An error occurred during JSON parsing." : errorMessage);
			}
			return ExecuteDevToolsMethodAsync(webview, method, args is null ? default(CefDictionaryValue) : args.GetDictionary(), convert, cancellationToken);
		}

		/// <summary>
		/// Executes a method call over the DevTools protocol without any optional parameters.
		/// </summary>
		/// <param name="webview">The WebView control.</param>
		/// <param name="method">The method name.</param>
		/// <returns>
		/// The JSON string with the response. Structure of the response varies depending
		/// on the method name and is defined by the &apos;RETURN OBJECT&apos; section of
		/// the Chrome DevTools Protocol command description.
		/// </returns>
		/// <remarks>
		/// Usage of the ExecuteDevToolsMethodAsync function does not require an active
		/// DevTools front-end or remote-debugging session. Other active DevTools sessions
		/// will continue to function independently. However, any modification of global
		/// browser state by one session may not be reflected in the UI of other sessions.
		/// <para/>
		/// Communication with the DevTools front-end (when displayed) can be logged
		/// for development purposes by passing the `--devtools-protocol-log-
		/// file=&lt;path&gt;` command-line flag.
		/// </remarks>
		public static Task<string> ExecuteDevToolsMethodAsync(this IChromiumWebView webview, string method)
		{
			return ExecuteDevToolsMethodAsync(webview, method, default(CefDictionaryValue), CancellationToken.None);
		}

		/// <summary>
		/// Executes a method call over the DevTools protocol without any optional parameters.
		/// </summary>
		/// <param name="webview">The WebView control.</param>
		/// <param name="method">The method name.</param>
		/// <returns>
		/// The JSON string with the response. Structure of the response varies depending
		/// on the method name and is defined by the &apos;RETURN OBJECT&apos; section of
		/// the Chrome DevTools Protocol command description.
		/// </returns>
		/// <remarks>
		/// Usage of the ExecuteDevToolsMethodAsync function does not require an active
		/// DevTools front-end or remote-debugging session. Other active DevTools sessions
		/// will continue to function independently. However, any modification of global
		/// browser state by one session may not be reflected in the UI of other sessions.
		/// <para/>
		/// Communication with the DevTools front-end (when displayed) can be logged
		/// for development purposes by passing the `--devtools-protocol-log-
		/// file=&lt;path&gt;` command-line flag.
		/// </remarks>
		public static Task<string> ExecuteDevToolsMethodAsync(this IChromiumWebView webview, string method, CancellationToken cancellationToken)
		{
			return ExecuteDevToolsMethodAsync(webview, method, default(CefDictionaryValue), cancellationToken);
		}

		/// <summary>
		/// Captures page screenshot.
		/// </summary>
		/// <param name="webview">The WebView control.</param>
		/// <param name="settings">The capture settings or null.</param>
		/// <param name="targetStream">A stream to save the captured image to.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="webview"/> or <paramref name="targetStream"/> is null.
		/// </exception>
		/// <exception cref="DevToolsProtocolException">
		/// An error occurred while trying to execute a DevTools Protocol method.
		/// </exception>
		/// <exception cref="InvalidOperationException">Other error occurred.</exception>
		public static async Task CaptureScreenshotAsync(this IChromiumWebView webview, PageCaptureSettings settings, Stream targetStream, CancellationToken cancellationToken)
		{
			if (webview is null)
				throw new ArgumentNullException(nameof(webview));

			if (targetStream is null)
				throw new ArgumentNullException(nameof(targetStream));

			CefDictionaryValue args;
			if (settings is null)
			{
				args = null;
			}
			else
			{
				args = new CefDictionaryValue();
				if (settings.Format == ImageCompressionFormat.Jpeg)
				{
					args.SetString("format", "jpeg");
					if (settings.Quality.HasValue)
						args.SetInt("quality", settings.Quality.Value);
				}
				if (!settings.FromSurface)
					args.SetBool("fromSurface", false);
				if (settings.Viewport != null)
				{
					PageViewport viewport = settings.Viewport;
					var viewportDict = new CefDictionaryValue();
					viewportDict.SetDouble("x", viewport.X);
					viewportDict.SetDouble("y", viewport.Y);
					viewportDict.SetDouble("width", viewport.Width);
					viewportDict.SetDouble("height", viewport.Height);
					viewportDict.SetDouble("scale", viewport.Scale);
					args.SetDictionary("clip", viewportDict);
				}
				if (settings.CaptureBeyondViewport)
					args.SetBool("captureBeyondViewport", true);
			}

			byte[] rv = (byte[])await ExecuteDevToolsMethodInternalAsync(webview, "Page.captureScreenshot", args, null, cancellationToken).ConfigureAwait(false);

			if (rv != null && rv.Length > 11 && rv[rv.Length - 1] == '}' && rv[rv.Length - 2] == '"'
				&& "{\"data\":\"".Equals(Encoding.ASCII.GetString(rv, 0, 9), StringComparison.Ordinal))
			{
				using (var input = new MemoryStream(rv, 9, rv.Length - 11))
				using (var base64Transform = new FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces))
				using (var cryptoStream = new CryptoStream(input, base64Transform, CryptoStreamMode.Read))
				{
					await cryptoStream.CopyToAsync(targetStream, 4096, cancellationToken).ConfigureAwait(false);
					await targetStream.FlushAsync(cancellationToken).ConfigureAwait(false);
				}
			}
			else
			{
				throw new InvalidOperationException();
			}
		}

		/// <summary>
		/// Clears browser cache.
		/// </summary>
		/// <param name="webview">The WebView control.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public static async Task ClearBrowserCacheAsync(this IChromiumWebView webview, CancellationToken cancellationToken)
		{
			if (webview is null)
				throw new ArgumentNullException(nameof(webview));

			ThrowIfNotEmptyResponse(await ExecuteDevToolsMethodInternalAsync(webview, "Network.clearBrowserCache", null, cancellationToken).ConfigureAwait(false));
		}

		/// <summary>
		/// Clears browser cookies.
		/// </summary>
		/// <param name="webview">The WebView control.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public static async Task ClearBrowserCookiesAsync(this IChromiumWebView webview, CancellationToken cancellationToken)
		{
			if (webview is null)
				throw new ArgumentNullException(nameof(webview));

			ThrowIfNotEmptyResponse(await ExecuteDevToolsMethodInternalAsync(webview, "Network.clearBrowserCookies", null, cancellationToken).ConfigureAwait(false));
		}

		#region Emulation

		/// <summary>
		/// Overrides user agent with the given string.
		/// </summary>
		/// <param name="webview">The WebView control.</param>
		/// <param name="userAgent">User agent to use.</param>
		/// <param name="acceptLanguage">Browser langugage to emulate.</param>
		/// <param name="platform">The platform navigator.platform should return.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public static async Task SetUserAgentOverrideAsync(this IChromiumWebView webview, string userAgent, string acceptLanguage, string platform, CancellationToken cancellationToken)
		{
			if (webview is null)
				throw new ArgumentNullException(nameof(webview));

			var args = new CefDictionaryValue();
			args.SetString("userAgent", userAgent);
			if (acceptLanguage is not null)
				args.SetString("acceptLanguage", acceptLanguage);
			if (platform is not null)
				args.SetString("platform", platform);
			ThrowIfNotEmptyResponse(await ExecuteDevToolsMethodInternalAsync(webview, "Emulation.setUserAgentOverride", args, cancellationToken).ConfigureAwait(false));
		}

		/// <summary>
		/// Enables touch on platforms which do not support them.
		/// </summary>
		/// <param name="webview">The WebView control.</param>
		/// <param name="enabled">True whether the touch event emulation should be enabled.</param>
		/// <param name="maxTouchPoints">Maximum touch points supported. Defaults to one.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public static async Task SetTouchEmulationEnabledAsync(this IChromiumWebView webview, bool enabled, int? maxTouchPoints, CancellationToken cancellationToken)
		{
			if (webview is null)
				throw new ArgumentNullException(nameof(webview));

			var args = new CefDictionaryValue();
			args.SetBool("enabled", enabled);
			if (maxTouchPoints is not null)
			{
				if (maxTouchPoints.Value < 1 || maxTouchPoints.Value > 16)
					throw new ArgumentOutOfRangeException(nameof(maxTouchPoints), "Touch points must be between 1 and 16.");
				args.SetInt("maxTouchPoints", maxTouchPoints.Value);
			}
			ThrowIfNotEmptyResponse(await ExecuteDevToolsMethodInternalAsync(webview, "Emulation.setTouchEmulationEnabled", args, cancellationToken).ConfigureAwait(false));
		}

		/// <summary>
		/// Emulates the given media type or media feature for CSS media queries.
		/// </summary>
		/// <param name="webview">The WebView control.</param>
		/// <param name="media">Media type to emulate. Empty string disables the override.</param>
		/// <param name="features">Media features to emulate.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public static async Task SetEmulatedMediaAsync(this IChromiumWebView webview, string media, IDictionary<string, string> features, CancellationToken cancellationToken)
		{
			if (webview is null)
				throw new ArgumentNullException(nameof(webview));

			var args = new CefDictionaryValue();
			args.SetString("media", media);
			if (features is not null)
			{
				foreach (KeyValuePair<string, string> feature in features)
				{
					if (string.IsNullOrWhiteSpace(feature.Key))
						throw new ArgumentOutOfRangeException(nameof(features), "Key may not be empty.");
					args.SetString(feature.Key, feature.Value);
				}
			}
			ThrowIfNotEmptyResponse(await ExecuteDevToolsMethodInternalAsync(webview, "Emulation.setEmulatedMedia", args, cancellationToken).ConfigureAwait(false));
		}

		#endregion

		private static void ThrowIfNotEmptyResponse(string response)
		{
			if (response is null || !"{}".Equals(response, StringComparison.Ordinal))
				throw new InvalidOperationException();
		}
	}
}
