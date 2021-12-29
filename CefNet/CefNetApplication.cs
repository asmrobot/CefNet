using CefNet.Internal;
using CefNet.JSInterop;
using CefNet.WinApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace CefNet
{
	/// <summary>
	/// Provides methods and properties to manage an application, such as methods to start and stop
	/// an application, to process messages, and properties to get information about an application.
	/// </summary>
	public class CefNetApplication : CefApp
	{
		internal const string XrayRequestKey = "xray-request";
		internal const string XrayResponseKey = "xray-response";
		internal const string XrayReleaseKey = "xray-release";

		/// <summary>
		/// Occurs when a new message is received from a different process. Do not keep a
		/// reference to or attempt to access the message outside of an event handler.
		/// </summary>
		public event EventHandler<CefProcessMessageReceivedEventArgs> CefProcessMessageReceived;

		/// <summary>
		/// Occurs when an exception in a frame is not caught. This event is disabled by default.
		/// To enable set CefSettings.UncaughtExceptionStackSize &gt; 0.
		/// </summary>
		public event EventHandler<CefUncaughtExceptionEventArgs> CefUncaughtException;

		/// <summary>
		/// Occurs after WebKit has been initialized.
		/// </summary>
		public event EventHandler WebKitInitialized;

		/// <summary>
		/// Occurs immediately after the CEF context has been initialized.
		/// </summary>
		public event EventHandler CefContextInitialized;

		/// <summary>
		/// Occurs before a child process is launched.
		/// </summary>
		public event EventHandler<EventArgs> BeforeChildProcessLaunch;

		/// <summary>
		/// Occurs after loading the CEF library, but before the CEF process is initialized.
		/// </summary>
		public event EventHandler BeforeInitialize;

		private static IntPtr _cefLibHandle;
		private static ProcessType? _ProcessType;
		private int _initThreadId;
		private int _browsersCount;
		private TaskCompletionSource<bool> _shutdownSignalTaskSource;

		static CefNetApplication()
		{
			using (Process process = Process.GetCurrentProcess())
			{
				AllowDotnetProcess = "dotnet".Equals(process.ProcessName, StringComparison.Ordinal);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CefNetApplication"/> class.
		/// </summary>
		public CefNetApplication()
		{
			AppGlue = new CefAppGlue(this);
		}

		private CefAppGlue AppGlue { get; }

		/// <summary>
		/// Gets the current <see cref="CefNetApplication"/>.
		/// </summary>
		public static CefNetApplication Instance { get; private set; }

		/// <summary>
		/// Gets a value that indicates whether the <see cref="CefNetApplication"/> is initialized.
		/// </summary>
		public static bool IsInitialized
		{
			get { return Instance != null; }
		}

		/// <summary>
		/// Gets and sets a value indicating that the dotnet can be used to run this application,
		/// by specifying an application DLL, such as &apos;dotnet myapp.dll&apos;.
		/// </summary>
		public static bool AllowDotnetProcess { get; set; }

		/// <summary>
		/// Gets a value indicating that the application is using an external message loop.
		/// </summary>
		public bool UsesExternalMessageLoop { get; private set; }

		private static void AssertApiVersion()
		{
			string hash = CefApi.CefApiHash(CefApiHashType.Universal);
			if (CefApi.ApiHash.Equals(hash, StringComparison.OrdinalIgnoreCase))
				return;

			Version assemblyVersion = typeof(CefApi).Assembly.GetName().Version;
			throw new CefVersionMismatchException(string.Format(
				"CEF runtime version mismatch. Loaded version API hash: '{0}', expected: '{1}' (CEF {2}.{3}).",
				hash,
				CefApi.ApiHash,
				assemblyVersion.Major,
				assemblyVersion.Minor
			));
		}

		private static string InitializeDllPath(string cefPath)
		{
			if (!string.IsNullOrWhiteSpace(cefPath))
			{
				cefPath = cefPath.Trim();
				if (!Directory.Exists(cefPath))
					throw new DirectoryNotFoundException(string.Format("The CEF runtime can't be initialized from '{0}'.", cefPath));

				string path = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
				if (PlatformInfo.IsWindows)
				{
					if (!path.StartsWith(cefPath, StringComparison.CurrentCultureIgnoreCase)
						|| (path.Length > cefPath.Length && path[cefPath.Length] != ';'))
					{
						Environment.SetEnvironmentVariable("PATH", cefPath + ";" + path);
					}
					return Path.Combine(cefPath, "libcef.dll");
				}
				else if (PlatformInfo.IsLinux)
				{
					if (!path.StartsWith(cefPath, StringComparison.CurrentCulture)
						|| (path.Length > cefPath.Length && path[cefPath.Length] != ':'))
					{
						Environment.SetEnvironmentVariable("PATH", cefPath + ":" + path);
					}
					path = Environment.GetEnvironmentVariable("LD_LIBRARY_PATH") ?? string.Empty;
					if (!path.StartsWith(cefPath, StringComparison.CurrentCulture)
						|| (path.Length > cefPath.Length && path[cefPath.Length] != ';'))
					{
						Environment.SetEnvironmentVariable("LD_LIBRARY_PATH", cefPath + ":" + path);
					}
					return Path.Combine(cefPath, "libcef.so");
				}
				else if (PlatformInfo.IsMacOS)
				{
					if (!path.StartsWith(cefPath, StringComparison.CurrentCulture)
						|| (path.Length > cefPath.Length && path[cefPath.Length] != ':'))
					{
						Environment.SetEnvironmentVariable("PATH", cefPath + ":" + path);
					}
					path = Environment.GetEnvironmentVariable("DYLD_LIBRARY_PATH") ?? string.Empty;
					if (!path.StartsWith(cefPath, StringComparison.CurrentCulture)
						|| (path.Length > cefPath.Length && path[cefPath.Length] != ';'))
					{
						Environment.SetEnvironmentVariable("DYLD_LIBRARY_PATH", cefPath + ":" + Path.Combine(cefPath, "Libraries") + ":" + path);
					}
					return Path.Combine(cefPath, "Chromium Embedded Framework");
				}
			}

			if (PlatformInfo.IsWindows)
				return "libcef.dll";
			if (PlatformInfo.IsLinux)
				return "libcef.so";
			if (PlatformInfo.IsMacOS)
				return "Chromium Embedded Framework";

			throw new PlatformNotSupportedException();
		}

		/// <summary>
		/// Initializes CEF from specified path with user-provided settings. This
		/// function should be called on the main application thread to initialize
		/// CEF processes.
		/// </summary>
		/// <param name="path"></param>
		/// <param name="settings"></param>
		/// <exception cref="DllNotFoundException"></exception>
		/// <exception cref="DirectoryNotFoundException"></exception>
		/// <exception cref="CefVersionMismatchException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		public void Initialize(string path, CefSettings settings)
		{
			if (PlatformInfo.IsWindows)
			{
				if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
					throw new InvalidOperationException("The calling thread must be STA");
			}

			if (IsInitialized)
				throw new InvalidOperationException("CEF already initialized. You must call Initialize once per application process.");

			path = InitializeDllPath(path);

			if (PlatformInfo.IsWindows)
			{
				const int LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008;
				_cefLibHandle = NativeMethods.LoadLibraryEx(path, IntPtr.Zero, LOAD_WITH_ALTERED_SEARCH_PATH);
				if (IntPtr.Zero == _cefLibHandle)
					throw new DllNotFoundException(string.Format("Can't load '{0}' (error: {1}).", path, Marshal.GetLastWin32Error()));
			}
			else if (PlatformInfo.IsLinux || PlatformInfo.IsMacOS)
			{
				const int RTLD_NOW = 2;
				_cefLibHandle = NativeMethods.dlopen(path, RTLD_NOW);
				if (IntPtr.Zero == _cefLibHandle)
					throw new DllNotFoundException(string.Format("Can't load '{0}'.", path));
			}
			else
			{
				throw new PlatformNotSupportedException();
			}

			if (!TryInitializeDllImportResolver(_cefLibHandle) && PlatformInfo.IsMacOS)
				throw new NotSupportedException("Requires .NET Core 3.0 or later.");

			AssertApiVersion();

			OnBeforeInitalize(EventArgs.Empty);

			Interlocked.Exchange(ref _initThreadId, Thread.CurrentThread.ManagedThreadId);
			Instance = this;

			// Main args
			CefMainArgs main_args = CefMainArgs.CreateDefault();
			int retval = CefApi.ExecuteProcess(main_args, this, IntPtr.Zero);
			if (retval != -1)
				Environment.Exit(retval);

			UsesExternalMessageLoop = settings.ExternalMessagePump;
			if (!CefApi.Initialize(main_args, settings, this, IntPtr.Zero))
				throw new CefRuntimeException("Failed to initialize the CEF browser process.");

			GC.KeepAlive(settings);
		}

		/// <summary>
		/// Initializes a callback for resolving native library imports from an assembly.
		/// </summary>
		/// <param name="libcefHandle">The Chromium Embedded Framework library handle.</param>
		protected virtual bool TryInitializeDllImportResolver(IntPtr libcefHandle)
		{
#if NET
			NativeLibrary.SetDllImportResolver(typeof(CefApi).Assembly, ResolveNativeLibrary);
			return true;
#else
			Type nativeLibraryType = Type.GetType("System.Runtime.InteropServices.NativeLibrary");
			if (nativeLibraryType is null)
				return false;

			Type dllImportResolverDelegateType = Type.GetType("System.Runtime.InteropServices.DllImportResolver");
			if (dllImportResolverDelegateType is null)
				return false;

			MethodInfo setDllImportResolver = nativeLibraryType.GetMethod("SetDllImportResolver", new[] { typeof(Assembly), dllImportResolverDelegateType });
			if (setDllImportResolver is null)
				return false;

			setDllImportResolver.Invoke(null, new object[] {
				typeof(CefApi).Assembly,
				Delegate.CreateDelegate(dllImportResolverDelegateType, this, new Func<string, Assembly, DllImportSearchPath?, IntPtr>(ResolveNativeLibrary).Method)
			});

			return true;
#endif
		}

		/// <summary>
		/// Resolves native library loads initiated by this assembly.
		/// </summary>
		/// <param name="libname">The native library to resolve.</param>
		/// <param name="assembly">The assembly requesting the resolution.</param>
		/// <param name="paths">
		/// The <see cref="DefaultDllImportSearchPathsAttribute"/> on the PInvoke, if any.
		/// Otherwise, the <see cref="DefaultDllImportSearchPathsAttribute"/> on the assembly, if any.
		/// Otherwise null.
		/// </param>
		/// <returns>
		/// The handle for the loaded native library on success, or <see cref="IntPtr.Zero"/> on failure.
		/// </returns>
		protected virtual IntPtr ResolveNativeLibrary(string libname, Assembly assembly, DllImportSearchPath? paths)
		{
			if ("libcef".Equals(libname, StringComparison.Ordinal))
				return _cefLibHandle;
			return IntPtr.Zero;
		}

		/// <summary>
		/// Checks that the current thread is the main application thread.
		/// </summary>
		/// <returns>
		/// true if the calling thread is the main application thread; otherwise, false.
		/// </returns>
		public bool CheckAccess()
		{
			return _initThreadId == Thread.CurrentThread.ManagedThreadId;
		}

		/// <summary>
		/// Checks that the current thread is the main application thread and throws if not.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// The current thread is not the UI thread.
		/// </exception>
		public void AssertAccess()
		{
			if (!CheckAccess())
				throw new InvalidOperationException("Cross-thread operation not valid.");
		}

		/// <summary>
		/// Begins running a standard application message loop on the current thread.<para/>
		/// This function should only be called on the main application thread and only if
		/// <see cref="Initialize"/> is called with a <see cref="CefSettings.MultiThreadedMessageLoop"/>
		/// value of false. This function will block until a quit message is received by the system.
		/// </summary>
		/// <remarks>
		/// Use this function instead of an application-provided message loop to get the best
		/// balance between performance and CPU usage. 
		/// </remarks>
		public static void Run()
		{
			if (Instance is null || !Instance.CheckAccess())
				throw new InvalidOperationException("Cross-thread operation not valid.");
			CefApi.RunMessageLoop();
		}

		/// <summary>
		/// Quit the CEF message loop that was started by calling <see cref="Run"/>.
		/// </summary>
		/// <remarks>
		/// This function should only be called if <see cref="Run"/> was used.
		/// </remarks>
		public static void Exit()
		{
			if (CefApi.CurrentlyOn(CefThreadId.UI))
				CefApi.QuitMessageLoop();
			else
				CefNetApi.Post(CefThreadId.UI, CefApi.QuitMessageLoop);
		}

		/// <summary>
		/// Calls a <paramref name="callback"/> when the <see cref="CefNetApplication"/> is ready to shut down.
		/// </summary>
		/// <param name="callback">An action to run when the <see cref="CefNetApplication"/> is ready to shut down. </param>
		public async void SignalForShutdown(Action callback)
		{
			var shutdownTaskSource = new TaskCompletionSource<bool>();
			shutdownTaskSource = Interlocked.CompareExchange(ref _shutdownSignalTaskSource, shutdownTaskSource, null) ?? shutdownTaskSource;
			if (Volatile.Read(ref _browsersCount) == 0)
				shutdownTaskSource.TrySetResult(false);
			await shutdownTaskSource.Task.ConfigureAwait(false);
			GC.Collect();
			GC.WaitForPendingFinalizers();
			callback();
		}

		/// <summary>
		/// Shuts down a CEF application.
		/// </summary>
		public void Shutdown()
		{
			AssertAccess();
			GC.Collect();
			GC.WaitForPendingFinalizers();
			CefApi.Shutdown();
		}

		/// <summary>
		/// Gets the type of the current process.
		/// </summary>
		public static ProcessType ProcessType
		{
			get
			{
				if (_ProcessType != null)
					return _ProcessType.Value;

				string type = Environment.GetCommandLineArgs().FirstOrDefault(arg => arg.StartsWith("--type="));
				if (type is null)
				{
					_ProcessType = ProcessType.Main;
					return ProcessType.Main;
				}

				switch (type.Substring(7))
				{
					case "renderer":
						_ProcessType = ProcessType.Renderer;
						break;
					case "zygote":
						_ProcessType = ProcessType.Zygote;
						break;
					case "gpu-process":
						_ProcessType = ProcessType.Gpu;
						break;
					case "utility":
						_ProcessType = ProcessType.Utility;
						break;
					case "ppapi":
						_ProcessType = ProcessType.PPApi;
						break;
					case "ppapi-broker":
						_ProcessType = ProcessType.PPApiBroker;
						break;
					case "nacl-loader":
						_ProcessType = ProcessType.NaClLoader;
						break;
					default:
						_ProcessType = ProcessType.Other;
						break;
				}
				return _ProcessType.Value;
			}
		}

		/// <summary>
		/// Raises the <see cref="BeforeInitialize"/> event.
		/// </summary>
		/// <param name="e">
		/// The <see cref="EventArgs"/> object that contains the event data.
		/// </param>
		protected virtual void OnBeforeInitalize(EventArgs e)
		{
			BeforeInitialize?.Invoke(this, e);
		}

		/// <summary>
		/// Returns a handler for functionality specific to the render process.
		/// </summary>
		/// <returns>A handler for functionality specific to the render process.</returns>
		protected internal override CefRenderProcessHandler GetRenderProcessHandler()
		{
			return AppGlue.RenderProcessGlue;
		}

		/// <summary>
		/// Returns the handler for functionality specific to the browser process.<para/>
		/// This function is called on multiple threads in the browser process.
		/// </summary>
		/// <returns>A handler for functionality specific to the browser process.</returns>
		protected internal override CefBrowserProcessHandler GetBrowserProcessHandler()
		{
			return AppGlue.BrowserProcessGlue;
		}

		private static bool ProcessXrayMessage(CefProcessMessage msg)
		{
			CefListValue args = msg.ArgumentList;
			var sqi = ScriptableRequestInfo.Get(args.GetInt(0));
			if (sqi is null)
				return true;

			if (args.GetSize() == 2)
			{
				sqi.Complete(args.GetValue(1));
			}
			else
			{
				sqi.Complete(new CefNetRemoteException(args.GetString(1), args.GetString(2), args.GetString(3)));
			}
			return true;
		}

		private static void ProcessXrayRelease(CefProcessMessageReceivedEventArgs e)
		{
			using (var args = e.Message.ArgumentList)
			{
				if (args.GetType(0) != CefValueType.Binary)
					return;
				XrayHandle handle = XrayHandle.FromCfxBinaryValue(args.GetBinary(0));
				if ((int)(handle.frame >> 32) != (int)(e.Frame.Identifier >> 32))
					return; // Mismatch process IDs
				handle.Release();
			}
		}

		private static void ProcessXrayRequest(CefProcessMessageReceivedEventArgs e)
		{
			CefListValue args = e.Message.ArgumentList;

			CefProcessMessage message = new CefProcessMessage(XrayResponseKey);
			CefListValue retArgs = message.ArgumentList;
			retArgs.SetSize(2);
			retArgs.SetValue(0, args.GetValue(0));

			CefValue retval = null;
			XrayAction queryAction = (XrayAction)args.GetInt(1);

			try
			{
				CefV8Context v8Context;
				XrayObject target = null;

				if (queryAction == XrayAction.GetGlobal)
				{
					v8Context = e.Frame.V8Context;
				}
				else
				{
					target = XrayHandle.FromCfxBinaryValue(args.GetBinary(2)).GetTarget(e.Frame);
					v8Context = target?.Context ?? e.Frame.V8Context;
				}

				if (!v8Context.IsValid)
					throw new ObjectDeadException();
				if (!v8Context.Enter())
					throw new ObjectDeadException();
				try
				{
					CefV8Value rv = null;
					switch (queryAction)
					{
						case XrayAction.Get:
							long corsRid = ScriptableObjectProvider.Get(v8Context, target, args, out rv);
							if (corsRid != 0)
							{
								var xray = new XrayHandle();
								xray.dataType = XrayDataType.CorsRedirect;
								xray.iRaw = corsRid;
								retval = new CefValue();
								retval.SetBinary(xray.ToCfxBinaryValue());
								retArgs.SetValue(1, retval);
							}
							break;
						case XrayAction.Set:
							ScriptableObjectProvider.Set(v8Context, target, args);
							break;
						case XrayAction.InvokeMember:
							rv = ScriptableObjectProvider.InvokeMember(v8Context, target, args);
							break;
						case XrayAction.Invoke:
							rv = ScriptableObjectProvider.Invoke(v8Context, target, args);
							break;
						case XrayAction.GetGlobal:
							rv = v8Context.GetGlobal();
							break;
						default:
							throw new NotSupportedException();
					}
					if (rv != null)
					{
						retval = ScriptableObjectProvider.CastCefV8ValueToCefValue(v8Context, rv, out bool isXray);
						if (!isXray) rv.Dispose();
						retArgs.SetValue(1, retval);
					}
				}
				finally
				{
					v8Context.Exit();
				}
			}
			catch (AccessViolationException) { throw; }
			catch (OutOfMemoryException) { throw; }
			catch (Exception ex)
			{
				//File.AppendAllText("G:\\outlog.txt", ex.GetType().Name + ": " + ex.Message + "\r\n" + ex.StackTrace + "\r\n");
				retArgs.SetSize(4);
				retArgs.SetString(1, ex.Message);
				retArgs.SetString(2, ex.GetType().FullName);
				retArgs.SetString(3, ex.StackTrace);
			}



			//CfxValueType t = e.Message.ArgumentList.GetType(0);



			e.Frame.SendProcessMessage(CefProcessId.Browser, message);
			message.Dispose();
			retval?.Dispose();

			e.Handled = true;
		}


#region CefRenderProcessHandler

		/// <summary>
		/// Raises the <see cref="WebKitInitialized"/> event.
		/// </summary>
		protected internal virtual void OnWebKitInitialized()
		{
			WebKitInitialized?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Called after a browser has been created.
		/// </summary>
		/// <param name="browser">The browser instance.</param>
		/// <param name="extraInfo">A read-only value originating from the browser creator or null.</param>
		protected internal virtual void OnBrowserCreated(CefBrowser browser, CefDictionaryValue extraInfo)
		{
			Interlocked.Increment(ref _browsersCount);
		}

		/// <summary>
		/// Called before a browser is destroyed. Release all references to the browser object
		/// and do not attempt to execute any methods on the browser object after this method returns.
		/// </summary>
		/// <param name="browser">The browser instance.</param>
		protected internal virtual void OnBrowserDestroyed(CefBrowser browser)
		{
			if (Interlocked.Decrement(ref _browsersCount) == 0)
				Volatile.Read(ref _shutdownSignalTaskSource)?.TrySetResult(true);
		}

		/// <summary>
		/// Return a handler for browser load status events.
		/// </summary>
		/// <returns>The handler for browser load status events.</returns>
		protected internal virtual CefLoadHandler GetLoadHandler()
		{
			return null;
		}

		protected internal virtual void OnContextCreated(CefBrowser browser, CefFrame frame, CefV8Context context)
		{
			XrayObject.OnContextCreated(context);
		}

		protected internal virtual void OnContextReleased(CefBrowser browser, CefFrame frame, CefV8Context context)
		{
			XrayObject.OnContextReleased(context);
		}

		/// <summary>
		/// Called for global uncaught exceptions in a frame. Execution of this callback is disabled by default.
		/// To enable set CefSettings.UncaughtExceptionStackSize &gt; 0.
		/// </summary>
		/// <param name="e"></param>
		protected internal virtual void OnUncaughtException(CefUncaughtExceptionEventArgs e)
		{
			CefUncaughtException?.Invoke(this, e);
		}

		protected internal virtual void OnFocusedNodeChanged(CefBrowser browser, CefFrame frame, CefDOMNode node)
		{

		}

		/// <summary>
		/// Called when a new message is received from a different process.
		/// </summary>
		protected internal virtual void OnCefProcessMessageReceived(CefProcessMessageReceivedEventArgs e)
		{
			if (e.SourceProcess == CefProcessId.Renderer)
			{
				if (e.Name == XrayResponseKey)
				{
					e.Handled = ProcessXrayMessage(e.Message);
				}
			}
			else if (e.SourceProcess == CefProcessId.Browser)
			{
				switch (e.Name)
				{
					case XrayReleaseKey:
						ProcessXrayRelease(e);
						return;
					case XrayRequestKey:
						ProcessXrayRequest(e);
						return;
				}
			}
			if (!e.Handled)
			{
				CefProcessMessageReceived?.Invoke(this, e);
			}
		}

#endregion

		/// <summary>
		/// Raises the <see cref="CefContextInitialized"/> event.<para/>
		/// Called on the browser process UI thread immediately after the CEF context has been initialized.
		/// </summary>
		protected internal virtual void OnContextInitialized()
		{
			CefContextInitialized?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raises the <see cref="BeforeChildProcessLaunch"/> event.<para/>
		/// Will be called on the browser process UI thread when launching a render process
		/// and on the browser process IO thread when launching a GPU or plugin process.
		/// </summary>
		/// <param name="e">A <see cref="BeforeChildProcessLaunchEventArgs"/> that contains the event data.</param>
		protected internal virtual void OnBeforeChildProcessLaunch(BeforeChildProcessLaunchEventArgs e)
		{
			if (AllowDotnetProcess)
			{
				e.CommandLine.Program = Environment.GetCommandLineArgs()[0];
				e.CommandLine.PrependWrapper("dotnet");
			}
			BeforeChildProcessLaunch?.Invoke(this, e);
		}

		/// <summary>
		/// Called from any thread when work has been scheduled for the browser process main (UI) thread.<para/>
		/// This callback should schedule a <see cref="CefApi.DoMessageLoopWork"/> call to happen on the main (UI) thread. 
		/// </summary>
		/// <param name="delayMs">
		/// The requested delay in milliseconds.<para/>If <paramref name="delayMs"/> is &lt;= 0
		/// then the call should happen reasonably soon; otherwise the call should be scheduled
		/// to happen after the specified delay and any currently pending scheduled call should
		/// be cancelled.
		/// </param>
		/// <remarks>
		/// This callback is used in combination with <see cref="CefSettings.ExternalMessagePump"/>
		/// and <see cref="CefApi.DoMessageLoopWork"/> in cases where the CEF message loop must be
		/// integrated into an existing application message loop.
		/// </remarks>
		protected internal virtual void OnScheduleMessagePumpWork(long delayMs)
		{

		}

		/// <summary>
		/// Provides the default client for use with a newly created browser window.
		/// </summary>
		/// <returns>
		/// Returns the default client for use with a newly created browser window. If
		/// null is returned the browser will be unmanaged (no callbacks will be
		/// executed for that browser) and application shutdown will be blocked until
		/// the browser window is closed manually. 
		/// </returns>
		/// <remarks>This function is currently only used with the chrome runtime.</remarks>
		protected internal virtual CefClient GetDefaultClient()
		{
			return null;
		}
	}
}
