#if True
using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using CefNet.WinApi;
using CefNet.CApi;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace CefNet
{
	/// <summary>
	/// Provides global methods.
	/// </summary>
	public static unsafe class CefApi
	{
		internal static bool UseUnsafeImplementation = true;

		/// <summary>
		/// Gets compatible CEF API hash.
		/// </summary>
		public static string ApiHash
		{
			get
			{
				return CefNativeApi.ApiHash;
			}
		}

		/// <summary>
		/// Create a new browser using the window parameters specified by
		/// <paramref name="windowInfo"/>.<para/>
		/// All values will be copied internally and the actual window (if any) will be
		/// created on the UI thread. This function can be called on any browser process
		/// thread and will not block.
		/// </summary>
		/// <param name="windowInfo">The CefWindowInfo instance.</param>
		/// <param name="client">The CefClient instance.</param>
		/// <param name="url">The initial url.</param>
		/// <param name="settings">Provides browser initialization settings.</param>
		/// <param name="requestContext">
		/// If <paramref name="requestContext"/> is null the global request context will be used.
		/// </param>
		/// <param name="extraInfo">
		/// Provides an opportunity to specify extra information specific to the created browser
		/// that will be passed to <see cref="CefRenderProcessHandler.OnBrowserCreated"/>
		/// in the render process.
		/// </param>
		/// <returns>Returns true on success.</returns>
		public static bool CreateBrowser(CefWindowInfo windowInfo, CefClient client, string url, CefBrowserSettings settings, CefDictionaryValue extraInfo, CefRequestContext requestContext)
		{
			if (windowInfo == null)
				throw new ArgumentNullException(nameof(windowInfo));
			if (settings == null)
				throw new ArgumentNullException(nameof(settings));

			int rv;
			fixed (char* s = url)
			{
				var aUrl = new cef_string_t() { Str = s, Length = (url != null ? url.Length : 0) };
				rv = CefNativeApi.cef_browser_host_create_browser(
					windowInfo.GetNativeInstance(),
					client != null ? client.GetNativeInstance() : null,
					&aUrl,
					settings.GetNativeInstance(),
					extraInfo != null ? extraInfo.GetNativeInstance() : null,
					requestContext != null ? requestContext.GetNativeInstance() : null
				);
			}
			GC.KeepAlive(windowInfo);
			GC.KeepAlive(settings);
			return rv != 0;
		}

		/// <summary>
		/// Create a new browser using the window parameters specified by
		/// <paramref name="windowInfo"/>.<para/>
		/// This function can only be called on the browser process UI thread.
		/// </summary>
		/// <param name="windowInfo">The CefWindowInfo instance.</param>
		/// <param name="client">The CefClient instance.</param>
		/// <param name="url">The initial url.</param>
		/// <param name="settings">Provides browser initialization settings.</param>
		/// <param name="requestContext">
		/// If <paramref name="requestContext"/> is null the global request context will be used.
		/// </param>
		/// <param name="extraInfo">
		/// Provides an opportunity to specify extra information specific to the created browser
		/// that will be passed to <see cref="CefRenderProcessHandler.OnBrowserCreated"/>
		/// in the render process.
		/// </param>
		/// <returns>A new CefBrowser object.</returns>
		public static CefBrowser CreateBrowserSync(CefWindowInfo windowInfo, CefClient client, string url, CefBrowserSettings settings, CefDictionaryValue extraInfo, CefRequestContext requestContext)
		{
			if (windowInfo == null)
				throw new ArgumentNullException(nameof(windowInfo));
			if (settings == null)
				throw new ArgumentNullException(nameof(settings));

			cef_browser_t* rv;
			fixed (char* s = url)
			{
				var aUrl = new cef_string_t() { Str = s, Length = (url != null ? url.Length : 0) };
				rv = CefNativeApi.cef_browser_host_create_browser_sync(
					windowInfo.GetNativeInstance(),
					client != null ? client.GetNativeInstance() : null,
					&aUrl,
					settings.GetNativeInstance(),
					extraInfo != null ? extraInfo.GetNativeInstance() : null,
					requestContext != null ? requestContext.GetNativeInstance() : null
				);
			}
			GC.KeepAlive(settings);
			return CefBrowser.Wrap(CefBrowser.Create, rv);
		}

		/// <summary>
		/// Returns True if called on the specified thread.
		/// </summary>
		/// <param name="threadId">The specified thread id.</param>
		/// <returns>Returns true if called on the specified thread.</returns>
		public static bool CurrentlyOn(CefThreadId threadId)
		{
			return CefNativeApi.cef_currently_on(threadId) != 0;
		}

		/// <summary>
		/// Post a task for execution on the specified thread.<para/>
		/// This function may be called on any thread.
		/// </summary>
		/// <param name="threadId">The specified thread id.</param>
		/// <param name="task">
		/// A <see cref="CefTask"/> that contains a method to be called on the thread.
		/// </param>
		/// <returns>Returns true on success.</returns>
		/// <remarks>It is an error to request a thread from the wrong process.</remarks>
		public static bool PostTask(CefThreadId threadId, CefTask task)
		{
			if (task == null)
				throw new ArgumentNullException(nameof(task));
			return CefNativeApi.cef_post_task(threadId, task.GetNativeInstance()) != 0;
		}

		/// <summary>
		/// Post a task for delayed execution on the specified thread.<para/>
		/// This function may be called on any thread. 
		/// </summary>
		/// <param name="threadId">The specified thread id.</param>
		/// <param name="millisecondsDelay">
		/// The number of milliseconds to wait before executing the posted task.
		/// </param>
		/// <param name="task">A <see cref="CefTask"/> instance.</param>
		/// <returns>Returns true on success.</returns>
		/// <remarks>
		/// It is an error to request a thread from the wrong
		/// process. Execution will occur asynchronously. Delayed tasks are not
		/// supported on V8 WebWorker threads and will be executed without the
		/// specified delay.
		/// </remarks>
		public static bool PostTask(CefThreadId threadId, CefTask task, long millisecondsDelay)
		{
			if (task == null)
				throw new ArgumentNullException(nameof(task));
			return CefNativeApi.cef_post_delayed_task(threadId, task.GetNativeInstance(), millisecondsDelay) != 0;
		}

		/// <summary>
		/// Register a new V8 extension with the specified JavaScript extension code and handler.<para/>
		/// This function may only be called on the render process main thread.
		/// </summary>
		/// <param name="name">An extension name.</param>
		/// <param name="jscode">
		/// An extension source code. Functions implemented by the <paramref name="handler"/>
		/// are prototyped using the keyword &apos;native&apos;.
		/// </param>
		/// <param name="handler">A <see cref="CefV8Handler"/> instance.</param>
		/// <returns>Returns true on success.</returns>
		/// <remarks>
		/// The calling of a native function is restricted to the scope
		/// in which the prototype of the native function is defined.
		/// </remarks>
		public static bool RegisterExtension(string name, string jscode, CefV8Handler handler)
		{
			if (name == null)
				throw new ArgumentNullException(nameof(name));
			if (jscode == null)
				throw new ArgumentNullException(nameof(jscode));

			fixed (char* sName = name)
			fixed (char* sCode = jscode)
			{
				var aName = new cef_string_t { Str = sName, Length = name.Length };
				var aCode = new cef_string_t { Str = sCode, Length = jscode.Length };
				return CefNativeApi.cef_register_extension(&aName, &aCode, handler != null ? handler.GetNativeInstance() : null) != 0;
			}

		}

		/// <summary>
		/// Register a scheme handler factory with the global request context. This
		/// function may be called multiple times to change or remove the factory that
		/// matches the specified <paramref name="schemeName"/> and optional
		/// <paramref name="domainName"/>.<para/>
		/// This function may be called on any thread in the browser process.
		/// </summary>
		/// <param name="schemeName">
		/// If <paramref name="schemeName"/> is a built-in scheme and no handler is returned by
		/// <paramref name="factory"/> then the built-in scheme handler factory will be called. If
		/// <paramref name="schemeName"/> is a custom scheme then you must also implement the
		/// <see cref="CefApp.OnRegisterCustomSchemes"/> function in all processes.
		/// </param>
		/// <param name="domainName">
		/// A null value for a standard scheme will cause the factory to match all domain names.
		/// The <paramref name="domainName"/> value will be ignored for non-standard schemes.
		/// </param>
		/// <param name="factory">A <see cref="CefSchemeHandlerFactory"/> instance.</param>
		/// <returns>Returns false if an error occurs.</returns>
		public static bool RegisterSchemeHandlerFactory(string schemeName, string domainName, CefSchemeHandlerFactory factory)
		{
			if (string.IsNullOrWhiteSpace(schemeName))
				throw new ArgumentOutOfRangeException(nameof(schemeName));
			if (factory == null)
				throw new ArgumentNullException(nameof(factory));

			fixed (char* s0 = schemeName)
			fixed (char* s1 = domainName)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = schemeName.Length };
				var cstr1 = new cef_string_t { Str = s1, Length = domainName.Length };
				return CefNativeApi.cef_register_scheme_handler_factory(&cstr0, &cstr1, factory.GetNativeInstance()) != 0;
			}
		}

		/// <summary>
		/// Clear all scheme handler factories registered with the global request context.<para/>
		/// This function may be called on any thread in the browser process.
		/// </summary>
		/// <returns>Returns false on error.</returns>
		public static bool ClearSchemeHandlerFactories()
		{
			return CefNativeApi.cef_clear_scheme_handler_factories() != 0;
		}

		/// <summary>
		/// This function should be called from the application entry point function to
		/// execute a secondary process. It can be used to run secondary processes from
		/// the browser client executable (default behavior) or from a separate
		/// executable specified by the <see cref="CefSettings.BrowserSubprocessPath"/> value. 
		/// </summary>
		/// <param name="args">Command-line arguments.</param>
		/// <param name="application">
		/// The <paramref name="application"/> parameter may be null. 
		/// </param>
		/// <param name="windowsSandboxInfo">
		/// This parameter is only used on Windows and may be null (see cef_sandbox_win.h for details).
		/// </param>
		/// <returns>
		/// If called for the browser process (identified by no &quot;type&quot; command-line value)
		/// it will return immediately with a value of -1. If called for a recognized secondary process
		/// it will block until the process should exit and then return the process exit code.
		/// </returns>
		public static int ExecuteProcess(CefMainArgs args, CefApp application, IntPtr windowsSandboxInfo)
		{
			return CefNativeApi.cef_execute_process((cef_main_args_t*)&args, application != null ? application.GetNativeInstance() : null, (void*)windowsSandboxInfo);
		}

		/// <summary>
		/// This function should be called on the main application thread to initialize
		/// the CEF browser process. 
		/// </summary>
		/// <param name="args">Command-line arguments.</param>
		/// <param name="settings">CEF configuration settings.</param>
		/// <param name="application">
		/// The <paramref name="application"/> parameter may be null.
		/// </param>
		/// <param name="windowsSandboxInfo">
		/// This parameter is only used on Windows and may be null (see cef_sandbox_win.h for details).
		/// </param>
		/// <returns>
		/// A return value of true indicates that it succeeded and false indicates that it failed.
		/// </returns>
		public static bool Initialize(CefMainArgs args, CefSettings settings, CefApp application, IntPtr windowsSandboxInfo)
		{
			if (settings == null)
				throw new ArgumentNullException(nameof(settings));
			bool rv = CefNativeApi.cef_initialize((cef_main_args_t*)&args, settings.GetNativeInstance(), application != null ? application.GetNativeInstance() : null, (void*)windowsSandboxInfo) != 0;
			GC.KeepAlive(settings);
			GC.KeepAlive(application);
			return rv;
		}

		/// <summary>
		/// This function should be called on the main application thread to shut down
		/// the CEF browser process before the application exits.
		/// </summary>
		public static void Shutdown()
		{
			CefNativeApi.cef_shutdown();
		}

		/// <summary>
		/// Perform a single iteration of CEF message loop processing.<para/>
		/// This function should only be called on the main application thread and only if
		/// <see cref="Initialize"/> is called with a <see cref="CefSettings.MultiThreadedMessageLoop"/>
		/// value of false. This function will not block.
		/// </summary>
		/// <remarks>
		/// This function is provided for cases where the CEF message loop must be integrated
		/// into an existing application message loop. Use of this function is not recommended
		/// for most users; use either the <see cref="RunMessageLoop"/> function or
		/// <see cref="CefSettings.MultiThreadedMessageLoop"/> if possible. When using this function
		/// care must be taken to balance performance against excessive CPU usage. It is
		/// recommended to enable the <see cref="CefSettings.ExternalMessagePump"/> option when using
		/// this function so that <see cref="CefBrowserProcessHandler.OnScheduleMessagePumpWork"/> 
		/// callbacks can facilitate the scheduling process.
		/// </remarks>
		public static void DoMessageLoopWork()
		{
			CefNativeApi.cef_do_message_loop_work();
		}

		/// <summary>
		/// Run the CEF message loop.<para/>
		/// This function should only be called on the main application thread and only if
		/// <see cref="Initialize"/> is called with a <see cref="CefSettings.MultiThreadedMessageLoop"/>
		/// value of false. This function will block until a quit message is received by the system.
		/// </summary>
		/// <remarks>
		/// Use this function instead of an application-provided message loop to get the best
		/// balance between performance and CPU usage. 
		/// </remarks>
		public static void RunMessageLoop()
		{
			CefNativeApi.cef_run_message_loop();
		}

		/// <summary>
		/// Quit the CEF message loop that was started by calling <see cref="RunMessageLoop"/>.<para/>
		/// This function should only be called on the main application thread and only
		/// if <see cref="RunMessageLoop"/> was used.
		/// </summary>
		public static void QuitMessageLoop()
		{
			CefNativeApi.cef_quit_message_loop();
		}

		/// <summary>
		/// Notifies CEF that a modal loop has been entered/exited.
		/// </summary>
		/// <param name="osModalLoop">
		/// Set to true before calling Windows APIs like TrackPopupMenu that enter a
		/// modal message loop. Set to false after exiting the modal message loop.
		/// </param>
		public static void SetOSModalLoop(bool osModalLoop)
		{
			CefNativeApi.cef_set_osmodal_loop(osModalLoop ? 1 : 0);
		}

		/// <summary>
		/// Call during process startup to enable High-DPI support on Windows 7 or newer.
		/// Older versions of Windows should be left DPI-unaware because they do not
		/// support DirectWrite and GDI fonts are kerned very badly.
		/// </summary>
		public static void EnableHighDPISupport()
		{
			CefNativeApi.cef_enable_highdpi_support();
		}

		/// <summary>
		/// Returns true if the certificate status has any error, major or minor.
		/// </summary>
		/// <param name="status">A status code.</param>
		/// <returns>
		/// Returns true if the certificate status has any error.
		/// </returns>
		public static bool IsCertStatusError(CefCertStatus status)
		{
			return CefNativeApi.cef_is_cert_status_error(status) != 0;
		}

		/// <summary>
		/// Get a value which indicates that crash reporting is configured using an
		/// INI-style config file named &quot;crash_reporter.cfg&quot;.
		/// </summary>
		/// <remarks>
		/// On Windows and Linux .cfg file must be placed next to the main application executable.
		/// On macOS this file must be placed in the top-level app bundle Resources directory
		/// (e.g. &quot;&lt;appname&gt;.app/Contents/Resources&quot;).<para/>
		/// See <see href="https://bitbucket.org/chromiumembedded/cef/wiki/CrashReporting"/> for details.
		/// </remarks>
		public static bool CrashReportingEnabled
		{
			get { return CefNativeApi.cef_crash_reporting_enabled() != 0; }
		}

		/// <summary>
		/// Sets or clears a specific key-value pair from the crash metadata.
		/// </summary>
		/// <param name="key">The key of the element to add.</param>
		/// <param name="value">The value of the element to add. The value can be null.</param>
		public static void SetCrashKeyValue(string key, string value)
		{
			if (string.IsNullOrWhiteSpace(key))
				throw new ArgumentOutOfRangeException(nameof(key));

			fixed (char* s0 = key)
			fixed (char* s1 = value)
			{
				var aKey = new cef_string_t { Str = s0, Length = value.Length };
				var aValue = new cef_string_t { Str = s1, Length = (value != null ? value.Length : 0) };
				CefNativeApi.cef_set_crash_key_value(&aKey, &aValue);
			}
		}

		/// <summary>
		/// Creates a directory and all parent directories if they don&apos;t already exist.<para/>
		/// Calling this function on the browser process UI or IO threads is not allowed.
		/// </summary>
		/// <param name="fullPath">The directory to create.</param>
		/// <returns>Returns true on successful creation or if the directory already exists.</returns>
		/// <remarks>The directory is only readable by the current user.</remarks>
		public static bool CreateDirectory(string fullPath)
		{
			if (string.IsNullOrWhiteSpace(fullPath))
				throw new ArgumentOutOfRangeException(nameof(fullPath));

			fixed (char* s0 = fullPath)
			{
				var path = new cef_string_t { Str = s0, Length = fullPath.Length };
				return CefNativeApi.cef_create_directory(&path) != 0;
			}
		}

		/// <summary>
		/// Get the temporary directory provided by the system.<para/>
		/// WARNING: In general, you should use the temp directory variants below instead
		/// of this function. Those variants will ensure that the proper permissions are
		/// set so that other users on the system can&apos;t edit them while they&apos;re open
		/// (which could lead to security issues). 
		/// </summary>
		/// <returns>Returns null if an error occurs.</returns>
		public static string GetTempDirectory()
		{
			var path = new cef_string_t();
			if (CefNativeApi.cef_get_temp_directory(&path) != 0)
			{
				return CefString.ReadAndFree(&path);
			}
			return null;
		}

		/// <summary>
		/// Creates a new directory.<para/>
		/// Calling this function on the browser process UI or IO threads is not allowed.
		/// </summary>
		/// <param name="prefix">
		/// On Windows if this value is provided the new directory name is in the format
		/// of &quot;prefixyyyy&quot;.
		/// </param>
		/// <returns>
		/// Returns the full path of the directory that was created or null if an error occurs.
		/// </returns>
		/// <remarks>
		/// The directory is only readable by the current user.
		/// </remarks>
		public static string CreateNewTempDirectory(string prefix)
		{
			fixed (char* s0 = prefix)
			{
				var path = new cef_string_t();
				var cstr = new cef_string_t { Str = s0, Length = (prefix != null ? prefix.Length : 0) };
				if (CefNativeApi.cef_create_new_temp_directory(&cstr, &path) != 0)
				{
					return CefString.ReadAndFree(&path);
				}
			}
			return null;
		}

		/// <summary>
		/// Creates a directory within another directory.<para/>
		/// Calling this function on the browser process UI or IO threads is not allowed.
		/// </summary>
		/// <param name="basePath">The path to base directory.</param>
		/// <param name="prefix">
		/// Extra characters will be appended to <paramref name="prefix"/> to ensure that
		/// the new directory does not have the same name as an existing directory.
		/// </param>
		/// <returns>
		/// Returns the full path of the directory that was created or null if an error occurs.
		/// </returns>
		/// <remarks>
		/// The directory is only readable by the current user.
		/// </remarks>
		public static string CreateTempDirectoryInDirectory(string basePath, string prefix)
		{
			if (string.IsNullOrWhiteSpace(basePath))
				throw new ArgumentOutOfRangeException(nameof(basePath));

			fixed (char* s0 = basePath)
			fixed (char* s1 = prefix)
			{
				var path = new cef_string_t();
				var cstr0 = new cef_string_t { Str = s0, Length = basePath.Length };
				var cstr1 = new cef_string_t { Str = s1, Length = (prefix != null ? prefix.Length : 0) };
				if (CefNativeApi.cef_create_temp_directory_in_directory(&cstr0, &cstr1, &path) != 0)
				{
					return CefString.ReadAndFree(&path);
				}
			}
			return null;
		}

		/// <summary>
		/// Determines whether the given path refers to an existing directory on disk.<para/>
		/// Calling this function on the browser process UI or IO threads is not allowed.
		/// </summary>
		/// <param name="path">The path to test.</param>
		/// <returns>
		/// Returns true if the given path exists and is a directory.
		/// </returns>
		public static bool DirectoryExists(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
				throw new ArgumentOutOfRangeException(nameof(path));
			fixed (char* s0 = path)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = path.Length };
				return CefNativeApi.cef_directory_exists(&cstr0) != 0;
			}
		}

		/// <summary>
		/// Deletes the given path whether it&apos;s a file or a directory.<para/>
		/// Calling this function on the browser process UI or IO threads is not allowed.
		/// </summary>
		/// <param name="path">
		/// If <paramref name="path"/> is a directory all contents will be deleted.
		/// On POSIX environments if <paramref name="path"/> is a symbolic link then
		/// only the symlink will be deleted.
		/// </param>
		/// <param name="recursive">
		/// If true any sub directories and their contents will also be deleted (equivalent to executing
		/// &quot;rm -rf&quot;, so use with caution).
		/// </param>
		/// <returns>
		/// Returns true on successful deletion or if <paramref name="path"/> does not exist. 
		/// </returns>
		public static bool DeleteFile(string path, bool recursive)
		{
			if (path == null)
				throw new ArgumentNullException(nameof(path));
			fixed (char* s0 = path)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = path.Length };
				return CefNativeApi.cef_delete_file(&cstr0, recursive ? 1 : 0) != 0;
			}
		}

		/// <summary>
		/// Writes the contents of <paramref name="sourceDirectory"/> into a zip archive at
		/// <paramref name="destinationFile"/>.<para/>
		/// Calling this function on the browser process UI or IO threads is not allowed.
		/// </summary>
		/// <param name="sourceDirectory">The source directory.</param>
		/// <param name="destinationFile">The file to write to.</param>
		/// <param name="includeHiddenFiles">
		/// If true then files starting with &quot;.&quot; will be included.
		/// </param>
		/// <returns>
		/// Returns true on success.
		/// </returns>
		public static bool ZipDirectory(string sourceDirectory, string destinationFile, bool includeHiddenFiles)
		{
			if (string.IsNullOrWhiteSpace(sourceDirectory))
				throw new ArgumentOutOfRangeException(nameof(sourceDirectory));
			if (string.IsNullOrWhiteSpace(destinationFile))
				throw new ArgumentOutOfRangeException(nameof(destinationFile));

			fixed (char* s0 = sourceDirectory)
			fixed (char* s1 = destinationFile)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = sourceDirectory.Length };
				var cstr1 = new cef_string_t { Str = s1, Length = destinationFile.Length };
				return CefNativeApi.cef_zip_directory(&cstr0, &cstr1, includeHiddenFiles ? 1 : 0) != 0;
			}
		}

		/// <summary>
		/// Loads the existing &quot;Certificate Revocation Lists&quot; file that is managed
		/// by Google Chrome.<para/>
		/// Should be called in the browser process after the context has been initialized.
		/// </summary>
		/// <param name="path">The path to file.</param>
		/// <remarks>
		/// The &quot;Certificate Revocation Lists&quot; file can generally be found in Chrome&apos;s
		/// User Data directory (e.g. &quot;%LOCALAPPDATA%\Google\Chrome\User Data&quot; on Windows)
		/// and is updated periodically by Chrome&apos;s component updater service.<para/>
		/// See <see href="https://dev.chromium.org/Home/chromium-security/crlsets"/> for background.
		/// </remarks>
		public static void LoadCrlSetsFile(string path)
		{
			if (!File.Exists(path))
				throw new FileNotFoundException(null, path);

			fixed (char* s0 = path)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = path.Length };
				CefNativeApi.cef_load_crlsets_file(&cstr0);
			}
		}

		/// <summary>
		/// Add an entry to the cross-origin access whitelist.<para/>
		/// This function may be called on any thread in the browser process. 
		/// </summary>
		/// <param name="sourceOrigin">
		/// The origin allowed to be accessed by the target protocol/domain.
		/// </param>
		/// <param name="targetProtocol">
		/// The target protocol allowed to access the source origin.
		/// </param>
		/// <param name="targetDomain">
		/// The optional target domain allowed to access the source origin.
		/// </param>
		/// <param name="allowTargetSubdomains">
		/// If <paramref name="targetDomain"/> is non-null and this value is false then
		/// only exact domain matches will be allowed. If <paramref name="targetDomain"/>
		/// contains a top-level domain component (like &quot;example.com&quot;) and
		/// this value is true sub-domain matches will be allowed.
		/// If <paramref name="targetDomain"/> is null and <paramref name="allowTargetSubdomains"/>
		/// is true all domains and IP addresses will be allowed.
		/// </param>
		/// <returns>
		/// Returns false if <paramref name="sourceOrigin"/> is invalid or the whitelist
		/// cannot be accessed.
		/// </returns>
		/// <remarks>
		/// The same-origin policy restricts how scripts hosted from different origins
		/// (scheme + domain + port) can communicate. By default, scripts can only access
		/// resources with the same origin. Scripts hosted on the HTTP and HTTPS schemes
		/// (but no other schemes) can use the &quot;Access-Control-Allow-Origin&quot; header to
		/// allow cross-origin requests.<para/>For example, https://source.example.com can make
		/// XMLHttpRequest requests on http://target.example.com if the
		/// http://target.example.com request returns an &quot;Access-Control-Allow-Origin:
		/// https://source.example.com&quot; response header.<para/>
		/// Scripts in separate frames or iframes and hosted from the same protocol and
		/// domain suffix can execute cross-origin JavaScript if both pages set the
		/// document.domain value to the same domain suffix.<para/>For example,
		/// scheme://foo.example.com and scheme://bar.example.com can communicate using
		/// JavaScript if both domains set document.domain=&quot;example.com&quot;.<para/>
		/// This function is used to allow access to origins that would otherwise violate
		/// the same-origin policy. Scripts hosted underneath the fully qualified
		/// <paramref name="sourceOrigin"/> URL (like http://www.example.com) will be allowed
		/// access to all resources hosted on the specified <paramref name="targetProtocol"/>
		/// and <paramref name="targetDomain"/>.
		/// This function cannot be used to bypass the restrictions on local or display
		/// isolated schemes.
		/// </remarks>
		public static bool AddCrossOriginWhitelistEntry(string sourceOrigin, string targetProtocol, string targetDomain, bool allowTargetSubdomains)
		{
			if (sourceOrigin == null)
				throw new ArgumentNullException(nameof(sourceOrigin));
			if (targetProtocol == null)
				throw new ArgumentNullException(nameof(targetProtocol));

			fixed (char* s0 = sourceOrigin)
			fixed (char* s1 = targetProtocol)
			fixed (char* s2 = targetDomain)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = sourceOrigin.Length };
				var cstr1 = new cef_string_t { Str = s1, Length = targetProtocol.Length };
				var cstr2 = new cef_string_t { Str = s2, Length = (targetDomain != null ? targetDomain.Length : 0) };
				return CefNativeApi.cef_add_cross_origin_whitelist_entry(&cstr0, &cstr1, &cstr2, allowTargetSubdomains ? 1 : 0) != 0;
			}
		}

		/// <summary>
		/// Remove an entry from the cross-origin access whitelist.<para/>
		/// This function may be called on any thread in the browser process.
		/// </summary>
		/// <param name="sourceOrigin">The origin allowed to be accessed by the target protocol/domain.</param>
		/// <param name="targetProtocol">The target protocol allowed to access the source origin.</param>
		/// <param name="targetDomain">The optional target domain allowed to access the source origin.</param>
		/// <param name="allowTargetSubdomains">Indicates that target subdomains allowed to access the source origin.</param>
		/// <returns>
		/// Returns false if <paramref name="sourceOrigin"/> is invalid or the whitelist cannot be accessed.
		/// </returns>
		public static bool RemoveCrossOriginWhitelistEntry(string sourceOrigin, string targetProtocol, string targetDomain, bool allowTargetSubdomains)
		{
			if (sourceOrigin == null)
				throw new ArgumentNullException(nameof(sourceOrigin));
			if (targetProtocol == null)
				throw new ArgumentNullException(nameof(targetProtocol));

			fixed (char* s0 = sourceOrigin)
			fixed (char* s1 = targetProtocol)
			fixed (char* s2 = targetDomain)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = sourceOrigin.Length };
				var cstr1 = new cef_string_t { Str = s1, Length = targetProtocol.Length };
				var cstr2 = new cef_string_t { Str = s2, Length = (targetDomain != null ? targetDomain.Length : 0) };
				return CefNativeApi.cef_remove_cross_origin_whitelist_entry(&cstr0, &cstr1, &cstr2, allowTargetSubdomains ? 1 : 0) != 0;
			}
		}

		/// <summary>
		/// Remove all entries from the cross-origin access whitelist.<para/>
		/// This function may be called on any thread in the browser process.
		/// </summary>
		/// <returns>Returns flase if the whitelist cannot be accessed.</returns>
		public static bool ClearCrossOriginWhitelist()
		{
			return CefNativeApi.cef_clear_cross_origin_whitelist() != 0;
		}

		/// <summary>
		/// Parse the specified <paramref name="url"/> into its component parts.
		/// </summary>
		/// <param name="url">The string representing the url.</param>
		/// <returns>The <see cref="CefUrlParts"/> instance.</returns>
		public static CefUrlParts ParseUrl(string url)
		{
			if (url == null)
				throw new ArgumentNullException(nameof(url));

			var url_parts = new CefUrlParts();
			fixed (char* s0 = url)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = url.Length };
				if (CefNativeApi.cef_parse_url(&cstr0, (cef_urlparts_t*)&url_parts) != 0)
				{
					return url_parts;
				}
			}
			throw new UriFormatException();
		}

		/// <summary>
		/// Creates a URL from the specified <paramref name="parts"/>. 
		/// </summary>
		/// <param name="parts">
		/// The <see cref="CefUrlParts"/> instance which must contain a non-null spec
		/// or a non-null host and path (at a minimum), but not both.
		/// </param>
		/// <returns>
		/// Returns null if <paramref name="parts"/> isn&apos;t initialized as described.
		/// </returns>
		public static string CreateUrl(CefUrlParts parts)
		{
			var s = new cef_string_t();
			CefNativeApi.cef_create_url((cef_urlparts_t*)&parts, &s);
			return CefString.ReadAndFree(&s);
		}

		/// <summary>
		/// This is a convenience function for formatting a URL in a concise and
		/// human-friendly way to help users make security-related decisions (or in other
		/// circumstances when people need to distinguish sites, origins, or
		/// otherwise-simplified URLs from each other). Internationalized domain names (IDN) may be
		/// presented in Unicode if the conversion is considered safe. Do not use this
		/// for URLs which will be parsed or sent to other applications.
		/// </summary>
		/// <param name="originUrl">An origin url.</param>
		/// <returns>
		/// The returned value will (a) omit the path for standard schemes, excepting file
		/// and filesystem, and (b) omit the port if it is the default for the scheme.
		/// </returns>
		public static string FormatUrlForSecurityDisplay(string originUrl)
		{
			fixed (char* s0 = originUrl)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = (originUrl != null ? originUrl.Length : 0) };
				return CefString.ReadAndFree(CefNativeApi.cef_format_url_for_security_display(&cstr0));
			}
		}

		/// <summary>
		/// Returns the mime type for the specified file extension.
		/// </summary>
		/// <param name="extension">The file extension.</param>
		/// <returns>
		/// Returns the mime type for the specified file extension or null if unknown.
		/// </returns>
		public static string GetMimeType(string extension)
		{
			fixed (char* s0 = extension)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = (extension != null ? extension.Length : 0) };
				return CefString.ReadAndFree(CefNativeApi.cef_get_mime_type(&cstr0));
			}
		}

		/// <summary>
		/// Get the file extensions associated with the given mime type.
		/// </summary>
		/// <param name="mimeType">The MIME type. This should be passed in lower case.</param>
		/// <returns>
		/// Returns an array that contains the file extensions associated with the given mime type.
		/// </returns>
		public static string[] GetExtensionsForMimeType(string mimeType)
		{
			using (var list = new CefStringList())
			{
				fixed (char* s0 = mimeType)
				{
					var cstr0 = new cef_string_t { Str = s0, Length = (mimeType != null ? mimeType.Length : 0) };
					CefNativeApi.cef_get_extensions_for_mime_type(&cstr0, list.GetNativeInstance());
				}
				return list.ToArray();
			}
		}

		/// <summary>
		/// Escapes characters in <paramref name="text"/> which are unsuitable for use as a query
		/// parameter value. Everything except alphanumerics and -_.!~*&apos;()
		/// will be converted to &quot;%XX&quot;. The result is basically the same
		/// as encodeURIComponent in Javacript.
		/// </summary>
		/// <param name="text">The string to be encoded.</param>
		/// <param name="usePlus">
		/// If <paramref name="usePlus"/> is true spaces will change to &quot;+&quot;.
		/// </param>
		/// <returns>A new string representing the provided string encoded as a URI component.</returns>
		public static string UriEncode(string text, bool usePlus)
		{
			if (text == null)
				return null;

			fixed (char* s0 = text)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = text.Length };
				return CefString.ReadAndFree(CefNativeApi.cef_uriencode(&cstr0, usePlus ? 1 : 0));
			}
		}

		/// <summary>
		/// Decodes any &quot;%XX&quot; encoding in the given string. Plus symbols ('+')
		/// are decoded to a space character.
		/// </summary>
		/// <param name="text">The string to be decoded.</param>
		/// <param name="convertToUtf8">
		/// If <paramref name="convertToUtf8"/> is true this function will attempt to interpret the initial
		/// decoded result as UTF-8. If the result is convertable into UTF-8 it will be
		/// returned as converted. Otherwise the initial decoded result will be returned.
		/// </param>
		/// <param name="rule">
		/// Supports further customization the decoding process.
		/// </param>
		/// <returns>Returns the decoded string.</returns>
		public static string UriDecode(string text, bool convertToUtf8, CefUriUnescapeRule rule)
		{
			if (text == null)
				return null;

			fixed (char* s0 = text)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = text.Length };
				return CefString.ReadAndFree(CefNativeApi.cef_uridecode(&cstr0, convertToUtf8 ? 1 : 0, rule));
			}
		}

		/// <summary>
		/// Parses the specified <paramref name="json"/> string.
		/// </summary>
		/// <param name="json">The JSON string to parse.</param>
		/// <param name="options">Options to control the behavior during parsing.</param>
		/// <returns>
		/// Returns a dictionary or list representation. If JSON parsing fails this function returns NULL.
		/// </returns>
		public static CefValue CefParseJSON(string json, CefJsonParserOptions options)
		{
			if (json == null)
				return null;

			fixed (char* s0 = json)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = json.Length };
				return CefValue.Wrap(CefValue.Create, CefNativeApi.cef_parse_json(&cstr0, options));
			}
		}

		/// <summary>
		/// Parses the specified UTF8-encoded JSON buffer.
		/// </summary>
		/// <param name="json">The UTF8-encoded JSON buffer to parse.</param>
		/// <param name="options">Options to control the behavior during parsing.</param>
		/// <returns>On success, a dictionary or list representation. If JSON parsing fails this function returns null.</returns>
		public static CefValue CefParseJSONBuffer(byte[] json, CefJsonParserOptions options)
		{
			if (json == null)
				return null;

			fixed (byte* buf = json)
			{
				return CefValue.Wrap(CefValue.Create, CefNativeApi.cef_parse_json_buffer(buf, new UIntPtr((uint)json.Length), options));
			}
		}

		/// <summary>
		/// Parses the specified <paramref name="json"/> string.
		/// </summary>
		/// <param name="json">The JSON string to parse.</param>
		/// <param name="options">Options to control the behavior during parsing.</param>
		/// <param name="errorMessage">The error message.</param>
		/// <returns>
		/// Returns a dictionary or list representation of the JSON string. If JSON parsing fails
		/// returns null and populates <paramref name="errorMessage"/> with a formatted error message.
		/// </returns>
		public static CefValue CefParseJSON(string json, CefJsonParserOptions options, out string errorMessage)
		{
			if (json == null)
			{
				errorMessage = "Line: 1, column: 1, Unexpected token.";
				return null;
			}

			fixed (char* s0 = json)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = json.Length };
				var cstr1 = new cef_string_t();
				CefValue rv = CefValue.Wrap(CefValue.Create, CefNativeApi.cef_parse_jsonand_return_error(&cstr0, options, &cstr1));
				errorMessage = CefString.ReadAndFree(&cstr1);
				return rv;
			}
		}

		/// <summary>
		/// Generates a JSON string from the specified root <paramref name="node"/>.
		/// </summary>
		/// <param name="node">A dictionary or list value.</param>
		/// <param name="options">Options to control serialization behavior.</param>
		/// <returns>
		/// Returns a JSON encoded string on success or null on failure.
		/// </returns>
		/// <remarks>
		/// This function requires exclusive access to <paramref name="node"/> including any underlying data.
		/// </remarks>
		public static string CefWriteJSON(CefValue node, CefJsonWriterOptions options)
		{
			if (node == null)
				return null;

			return CefString.ReadAndFree(CefNativeApi.cef_write_json(node.GetNativeInstance(), options));
		}

		/// <summary>
		/// Retrieve the path associated with the specified <paramref name="key"/>.<para/>
		/// Can be called on any thread in the browser process.
		/// </summary>
		/// <param name="key">The path key value.</param>
		/// <returns>
		/// Returns the path associated with the specified <paramref name="key"/> or null on failure.
		/// </returns>
		public static string GetPath(CefPathKey key)
		{
			var path = new cef_string_t();
			if (CefNativeApi.cef_get_path(key, &path) != 0)
			{
				return CefString.ReadAndFree(&path);
			}
			return null;
		}

		/// <summary>
		/// Launches the process specified via <paramref name="commandLine"/>.<para/>
		/// Must be called on the browser process TID_PROCESS_LAUNCHER thread.
		/// </summary>
		/// <param name="commandLine">The command line arguments.</param>
		/// <returns>Returns true upon success.</returns>
		/// <remarks>
		/// Unix-specific notes:
		/// <list type="bullet">
		///		<item>
		///			<description>
		/// All file descriptors open in the parent process will be closed in the
		/// child process except for stdin, stdout, and stderr.
		///			</description>
		///		</item>
		///		<item>
		///			<description>
		/// If the first argument on the command line does not contain a slash,
		/// PATH will be searched. (See man execvp.)
		///			</description>
		///		</item>
		/// </list>
		/// </remarks>
		public static bool CefLaunchProcess(CefCommandLine commandLine)
		{
			if (commandLine == null)
				throw new ArgumentNullException(nameof(commandLine));

			return CefNativeApi.cef_launch_process(commandLine.GetNativeInstance()) != 0;
		}

		/// <summary>
		/// Visit web plugin information.<para/>
		/// Can be called on any thread in the browser process.
		/// </summary>
		/// <param name="visitor">A <see cref="	CefWebPluginInfoVisitor"/> instance.</param>
		public static void VisitWebPluginInfo(CefWebPluginInfoVisitor visitor)
		{
			if (visitor == null)
				throw new ArgumentNullException(nameof(visitor));

			CefNativeApi.cef_visit_web_plugin_info(visitor.GetNativeInstance());
		}

		/// <summary>
		/// Cause the plugin list to refresh the next time it is accessed regardless of
		/// whether it has already been loaded.<para/>
		/// Can be called on any thread in the browser process.
		/// </summary>
		public static void RefreshWebPlugins()
		{
			CefNativeApi.cef_refresh_web_plugins();
		}

		/// <summary>
		/// Unregister an internal plugin. This may be undone the next time 
		/// <see cref="RefreshWebPlugins"/> is called.<para/>
		/// Can be called on any thread in the browser process.
		/// </summary>
		/// <param name="path">The plugin file path.</param>
		public static void UnregisterInternalWebPlugin(string path)
		{
			if (path == null)
				throw new ArgumentNullException(nameof(path));

			fixed (char* s0 = path)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = path.Length };
				CefNativeApi.cef_unregister_internal_web_plugin(&cstr0);
			}
		}

		/// <summary>
		/// Register a plugin crash.<para/>
		/// Can be called on any thread in the browser process but will be executed on the IO thread.
		/// </summary>
		/// <param name="path">The plugin file path.</param>
		public static void RegisterWebPluginCrash(string path)
		{
			if (path == null)
				throw new ArgumentNullException(nameof(path));

			fixed (char* s0 = path)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = path.Length };
				CefNativeApi.cef_register_web_plugin_crash(&cstr0);
			}
		}

		/// <summary>
		/// Query if a plugin is unstable.<para/>
		/// Can be called on any thread in the browser process.
		/// </summary>
		/// <param name="path">The plugin file path.</param>
		/// <param name="callback">
		/// A <see cref="CefWebPluginUnstableCallback"/> instance to receiving unstable
		/// plugin information.
		/// </param>
		public static void IsWebPluginUnstable(string path, CefWebPluginUnstableCallback callback)
		{
			if (path == null)
				throw new ArgumentNullException(nameof(path));
			if (callback == null)
				throw new ArgumentNullException(nameof(callback));

			fixed (char* s0 = path)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = path.Length };
				CefNativeApi.cef_is_web_plugin_unstable(&cstr0, callback.GetNativeInstance());
			}
		}

		/// <summary>
		/// Returns the current platform thread ID.
		/// </summary>
		/// <returns>Returns the current platform thread ID.</returns>
		public static uint GetCurrentPlatformThreadId()
		{
			return CefNativeApi.cef_get_current_platform_thread_id();
		}

		/// <summary>
		/// Returns the current platform thread handle.
		/// </summary>
		/// <returns>Returns the current platform thread handle.</returns>
		public static IntPtr GetCurrentPlatformThreadHandle()
		{
			if (PlatformInfo.IsWindows)
				return new IntPtr((int)CefNativeApi.cef_get_current_platform_thread_handle_windows());
			if (PlatformInfo.IsLinux)
				return CefNativeApi.cef_get_current_platform_thread_handle_linux();
			throw new NotImplementedException();
		}

		/// <summary>
		/// Start tracing events on all processes.<para/>
		/// This function must be called on the browser process UI thread.
		/// </summary>
		/// <param name="categories">
		/// A comma-delimited list of category wildcards. A category can
		/// have an optional &apos;-&apos; prefix to make it an excluded category. Having both
		/// included and excluded categories in the same list is not supported.<para/>
		/// Examples:
		/// <list type="bullet">
		/// <item><description>&quot;test_MyTest*,test_OtherStuff&quot;</description></item>
		/// <item><description>&quot;-excluded_category1,-excluded_category2&quot;</description></item>
		/// </list>
		/// </param>
		/// <param name="callback">
		/// Tracing is initialized asynchronously and <paramref name="callback"/> will be executed
		/// on the UI thread after initialization is complete.
		/// </param>
		/// <returns>
		/// If <see cref="CefBeginTracing"/> was called previously, or if a CefEndTracingAsync call is
		/// pending, <see cref="CefBeginTracing"/> will fail and return false.
		/// </returns>
		public static bool CefBeginTracing(string categories, CefCompletionCallback callback)
		{
			if (categories == null)
				throw new ArgumentOutOfRangeException(nameof(categories));
			if (callback == null)
				throw new ArgumentNullException(nameof(callback));

			fixed (char* s0 = categories)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = categories.Length };
				return CefNativeApi.cef_begin_tracing(&cstr0, callback.GetNativeInstance()) != 0;
			}
		}

		/// <summary>
		/// Stop tracing events on all processes.<para/>
		/// This function must be called on the browser process UI thread.
		/// </summary>
		/// <param name="path">
		/// The path at which tracing data will be written. If <paramref name="path"/> is null
		/// a new temporary file path will be used.
		/// </param>
		/// <param name="callback">
		/// The callback that will be executed once all processes have sent their trace data.
		/// If <paramref name="callback"/> is no trace data will be written.
		/// </param>
		/// <returns>
		/// This function will fail and return false if a previous call to
		/// CefEndTracingAsync is already pending or if <see cref="CefBeginTracing"/>
		/// was not called.
		/// </returns>
		public static bool CefEndTracing(string path, CefEndTracingCallback callback)
		{
			if (callback == null)
				throw new ArgumentNullException(nameof(callback));

			fixed (char* s0 = path)
			{
				var cstr0 = new cef_string_t { Str = s0, Length = path != null ? path.Length : 0 };
				return CefNativeApi.cef_end_tracing(&cstr0, callback.GetNativeInstance()) != 0;
			}
		}

		/// <summary>
		/// Returns the current system trace time or, if none is defined, the current
		/// high-res time. Can be used by clients to synchronize with the time
		/// information in trace events.
		/// </summary>
		/// <returns>
		/// Returns the current system trace time or, if none is defined, the current
		/// high-res time.
		/// </returns>
		public static long CefNowFromSystemTraceTime()
		{
			return CefNativeApi.cef_now_from_system_trace_time();
		}

		/// <summary>
		/// Returns CEF version information for the libcef library.
		/// </summary>
		/// <param name="component">
		/// Describes which version component will be returned.
		/// </param>
		/// <returns>Returns CEF version component.</returns>
		public static int CefVersionInfo(CefVersionComponent component)
		{
			return CefNativeApi.cef_version_info((int)component);
		}

		/// <summary>
		/// Returns CEF API hashes for the libcef library.
		/// </summary>
		/// <param name="type">
		/// Describes which hash value will be returned.
		/// </param>
		/// <returns>Returns CEF API hash.</returns>
		public static string CefApiHash(CefApiHashType type)
		{
			return Marshal.PtrToStringAnsi(CefNativeApi.cef_api_hash((int)type));
		}

	}
}

#endif