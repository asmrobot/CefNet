using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using CefNet.CApi;
using CefNet.WinApi;

namespace CefNet
{
	/// <summary>
	/// Class representing window information.
	/// </summary>
	public unsafe sealed class CefWindowInfo : IDisposable
	{
		private static readonly int CW_USEDEFAULT = PlatformInfo.IsWindows ? unchecked((int)0x80000000) : 0;

		private cef_window_info_t* _instance;

		public static CefWindowInfo Wrap(cef_window_info_t* instance)
		{
			if (instance == null)
				return null;
			return new CefWindowInfo(instance);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CefWindowInfo"/>.
		/// </summary>
		public CefWindowInfo()
		{
			int size;
			if (PlatformInfo.IsWindows)
				size = sizeof(cef_window_info_windows_t);
			else if (PlatformInfo.IsLinux)
				size = sizeof(cef_window_info_linux_t);
			else if (PlatformInfo.IsMacOS)
				size = sizeof(cef_window_info_mac_t);
			else
				throw new PlatformNotSupportedException();

			IntPtr mem = Marshal.AllocHGlobal(size);
			mem.InitBlock(0, size);
			_instance = (cef_window_info_t*)mem;
		}

		private CefWindowInfo(cef_window_info_t* instance)
		{
			_instance = instance;
			GC.SuppressFinalize(this);
		}

#pragma warning disable CS1591 // Missing comments
		~CefWindowInfo()
		{
			Dispose();
		}

		public void Dispose()
		{
			if (_instance != null)
			{
				WindowName = null;
				Marshal.FreeHGlobal(new IntPtr(_instance));
				_instance = null;
				GC.SuppressFinalize(this);
			}
		}
#pragma warning restore CS1591 // Missing comments

		private cef_window_info_windows_t* WindowsInstance
		{
			get
			{
				if (!PlatformInfo.IsWindows)
					throw new PlatformNotSupportedException();
				return (cef_window_info_windows_t*)GetNativeInstance();
			}
		}

		private cef_window_info_linux_t* LinuxInstance
		{
			get
			{
				if (!PlatformInfo.IsLinux)
					throw new PlatformNotSupportedException();
				return (cef_window_info_linux_t*)GetNativeInstance();
			}
		}

		private cef_window_info_mac_t* MacInstance
		{
			get
			{
				if (!PlatformInfo.IsMacOS)
					throw new PlatformNotSupportedException();
				return (cef_window_info_mac_t*)GetNativeInstance();
			}
		}

		/// <summary>
		/// Returns an unsafe pointer to the <see cref="cef_window_info_t"/> struct.
		/// </summary>
		/// <returns>A pointer to the <see cref="cef_window_info_t"/> struct.</returns>
		public cef_window_info_t* GetNativeInstance()
		{
			if (_instance == null)
				throw new ObjectDisposedException(nameof(CefWindowInfo));
			return _instance;
		}

		/// <summary>
		/// Gets or sets the extended window style of the window being created (Windows only).
		/// See CreateWindowEx() for more information.
		/// </summary>
		public uint ExStyle
		{
			get { return WindowsInstance->ex_style; }
			set { WindowsInstance->ex_style = value; }
		}

		/// <summary>
		/// The window name.
		/// </summary>
		public string WindowName
		{
			get
			{
				if (PlatformInfo.IsWindows)
					return CefString.Read(&WindowsInstance->window_name);
				if (PlatformInfo.IsLinux)
					return CefString.Read(&LinuxInstance->window_name);
				return CefString.Read(&MacInstance->window_name);
			}
			set
			{
				if (PlatformInfo.IsWindows)
					CefString.Replace(&WindowsInstance->window_name, value);
				else if (PlatformInfo.IsLinux)
					CefString.Replace(&LinuxInstance->window_name, value);
				else
					CefString.Replace(&MacInstance->window_name, value);
			}
		}

		/// <summary>
		/// The style of the window being created. See CreateWindowEx() for more
		/// information.
		/// </summary>
		public WindowStyle Style
		{
			get { return (WindowStyle)WindowsInstance->style; }
			set { WindowsInstance->style = (uint)value; }
		}

		/// <summary>
		/// The initial horizontal position of the window.
		/// </summary>
		public int X
		{
			get
			{
				if (PlatformInfo.IsWindows)
					return WindowsInstance->x;
				if (PlatformInfo.IsLinux)
					return LinuxInstance->x;
				return MacInstance->x;
			}
			set
			{
				if (PlatformInfo.IsWindows)
					WindowsInstance->x = value;
				else if (PlatformInfo.IsLinux)
					LinuxInstance->x = value;
				else
					MacInstance->x = value;
			}
		}

		/// <summary>
		/// The initial vertical position of the window.
		/// </summary>
		public int Y
		{
			get
			{
				if (PlatformInfo.IsWindows)
					return WindowsInstance->y;
				if (PlatformInfo.IsLinux)
					return LinuxInstance->y;
				return MacInstance->y;
			}
			set
			{
				if (PlatformInfo.IsWindows)
					WindowsInstance->y = value;
				else if (PlatformInfo.IsLinux)
					LinuxInstance->y = value;
				else
					MacInstance->y = value;
			}
		}

		/// <summary>
		/// The width of the window.
		/// </summary>
		public int Width
		{
			get
			{
				if (PlatformInfo.IsWindows)
					return WindowsInstance->width;
				if (PlatformInfo.IsLinux)
					return LinuxInstance->width;
				return MacInstance->width;
			}
			set
			{
				if (PlatformInfo.IsWindows)
					WindowsInstance->width = value;
				else if (PlatformInfo.IsLinux)
					LinuxInstance->width = value;
				else
					MacInstance->width = value;
			}
		}

		/// <summary>
		/// The height of the window.
		/// </summary>
		public int Height
		{
			get
			{
				if (PlatformInfo.IsWindows)
					return WindowsInstance->height;
				if (PlatformInfo.IsLinux)
					return LinuxInstance->height;
				return MacInstance->height;
			}
			set
			{
				if (PlatformInfo.IsWindows)
					WindowsInstance->height = value;
				else if (PlatformInfo.IsLinux)
					LinuxInstance->height = value;
				else
					MacInstance->height = value;
			}
		}

		/// <summary>
		/// Gets or sets the pointer for the parent window/view.
		/// </summary>
		public IntPtr ParentWindow
		{
			get
			{
				if (PlatformInfo.IsWindows)
					return WindowsInstance->parent_window;
				if (PlatformInfo.IsLinux)
					return LinuxInstance->parent_window;
				return MacInstance->parent_view;
			}
			set
			{
				if (PlatformInfo.IsWindows)
					WindowsInstance->parent_window = value;
				else if (PlatformInfo.IsLinux)
					LinuxInstance->parent_window = value;
				else
					MacInstance->parent_view = value;
			}
		}

		/// <summary>
		/// Gets or sets the pointer for the new browser window/view. Only used with windowed rendering.
		/// </summary>
		public IntPtr Window
		{
			get
			{
				if (PlatformInfo.IsWindows)
					return WindowsInstance->window;
				if (PlatformInfo.IsLinux)
					return LinuxInstance->window;
				return MacInstance->view;
			}
			set
			{
				if (PlatformInfo.IsWindows)
					WindowsInstance->window = value;
				else if (PlatformInfo.IsLinux)
					LinuxInstance->window = value;
				else
					MacInstance->view = value;
			}
		}

		/// <summary>
		/// Gets or sets a handle to a menu, or specifies a child-window identifier, depending on the
		/// window style (Windows only). See CreateWindowEx() for more information.
		/// </summary>
		public IntPtr Menu
		{
			get { return WindowsInstance->menu; }
			set { WindowsInstance->menu = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the browser using windowless (off-screen)
		/// rendering. No window will be created for the browser and all rendering will
		/// occur via the CefRenderHandler interface. The |ParentWindow| value will be
		/// used to identify monitor info and to act as the parent window for dialogs,
		/// context menus, etc. If |ParentWindow| is not provided then the main screen
		/// monitor will be used and some functionality that requires a parent window
		/// may not function correctly. In order to create windowless browsers the
		/// CefSettings.WindowlessRenderingEnabled value must be set to true.
		/// Transparent painting is enabled by default but can be disabled by setting
		/// CefBrowserSettings.BackgroundColor to an opaque value.
		/// </summary>
		public bool WindowlessRenderingEnabled
		{
			get
			{
				if (PlatformInfo.IsWindows)
					return WindowsInstance->windowless_rendering_enabled != 0;
				if (PlatformInfo.IsLinux)
					return LinuxInstance->windowless_rendering_enabled != 0;
				return MacInstance->windowless_rendering_enabled != 0;
			}
			set
			{
				if (PlatformInfo.IsWindows)
					WindowsInstance->windowless_rendering_enabled = value ? 1 : 0;
				else if (PlatformInfo.IsLinux)
					LinuxInstance->windowless_rendering_enabled = value ? 1 : 0;
				else
					MacInstance->windowless_rendering_enabled = value ? 1 : 0;
			}
		}

		/// <summary>
		/// Set to true to enable shared textures for windowless rendering. Only
		/// valid if WindowlessRenderingEnabled above is also set to true. Currently
		/// only supported on Windows (D3D11).
		/// </summary>
		public bool SharedTextureEnabled
		{
			get
			{
				if (PlatformInfo.IsWindows)
					return WindowsInstance->shared_texture_enabled != 0;
				if (PlatformInfo.IsLinux)
					return LinuxInstance->shared_texture_enabled != 0;
				return MacInstance->shared_texture_enabled != 0;
			}
			set
			{
				if (PlatformInfo.IsWindows)
					WindowsInstance->shared_texture_enabled = value ? 1 : 0;
				else if (PlatformInfo.IsLinux)
					LinuxInstance->shared_texture_enabled = value ? 1 : 0;
				else
					MacInstance->shared_texture_enabled = value ? 1 : 0;
			}
		}

		/// <summary>
		/// Set to true to enable the ability to issue BeginFrame requests from the
		/// client application by calling CefBrowserHost::SendExternalBeginFrame.
		/// </summary>
		public bool ExternalBeginFrameEnabled
		{
			get
			{
				if (PlatformInfo.IsWindows)
					return WindowsInstance->external_begin_frame_enabled != 0;
				if (PlatformInfo.IsLinux)
					return LinuxInstance->external_begin_frame_enabled != 0;
				return MacInstance->external_begin_frame_enabled != 0;
			}
			set
			{
				if (PlatformInfo.IsWindows)
					WindowsInstance->external_begin_frame_enabled = value ? 1 : 0;
				else if (PlatformInfo.IsLinux)
					LinuxInstance->external_begin_frame_enabled = value ? 1 : 0;
				else
					MacInstance->external_begin_frame_enabled = value ? 1 : 0;
			}
		}

		/// <summary>
		/// Create the browser as a child window.
		/// </summary>
		public void SetAsChild(IntPtr parentWindow, int left, int top, int width, int height)
		{
			if (PlatformInfo.IsWindows)
				Style = WindowStyle.WS_CHILD | WindowStyle.WS_CLIPCHILDREN | WindowStyle.WS_CLIPSIBLINGS | WindowStyle.WS_TABSTOP | WindowStyle.WS_VISIBLE;

			ParentWindow = parentWindow;
			X = left;
			Y = top;
			Width = width;
			Height = height;
		}

		/// <summary>
		/// Create the browser as a disabled child window.
		/// </summary>
		public void SetAsDisabledChild(IntPtr parentWindow)
		{
			if (PlatformInfo.IsWindows)
				Style = WindowStyle.WS_CHILD | WindowStyle.WS_CLIPCHILDREN | WindowStyle.WS_CLIPSIBLINGS | WindowStyle.WS_TABSTOP | WindowStyle.WS_DISABLED;

			ParentWindow = parentWindow;
			X = CW_USEDEFAULT;
			Y = CW_USEDEFAULT;
			Width = CW_USEDEFAULT;
			Height = CW_USEDEFAULT;
		}

		/// <summary>
		/// Create the browser as a popup window (Windows only).
		/// </summary>
		public void SetAsPopup(IntPtr parentWindow, string windowName)
		{
			if (PlatformInfo.IsWindows)
				Style = WindowStyle.WS_OVERLAPPEDWINDOW | WindowStyle.WS_CLIPCHILDREN | WindowStyle.WS_CLIPSIBLINGS | WindowStyle.WS_VISIBLE;
			
			ParentWindow = parentWindow;
			X = CW_USEDEFAULT;
			Y = CW_USEDEFAULT;
			Width = CW_USEDEFAULT;
			Height = CW_USEDEFAULT;
			WindowName = windowName;
		}

		/// <summary>
		/// Create the browser using windowless (off-screen) rendering. No window
		/// will be created for the browser and all rendering will occur via the
		/// CefRenderHandler interface. The |parent| value will be used to identify
		/// monitor info and to act as the parent window for dialogs, context menus,
		/// etc. If |parent| is not provided then the main screen monitor will be used
		/// and some functionality that requires a parent window may not function
		/// correctly. In order to create windowless browsers the
		/// CefSettings.WindowlessRenderingEnabled value must be set to true.
		/// Transparent painting is enabled by default but can be disabled by setting
		/// CefBrowserSettings.BackgroundColor to an opaque value.
		/// </summary>
		public void SetAsWindowless(IntPtr parentWindow)
		{
			ParentWindow = parentWindow;
			WindowlessRenderingEnabled = true;
		}

		/// <summary>
		/// Compares two <see cref="CefWindowInfo"/> instances for reference equality.
		/// </summary>
		/// <param name="obj">
		/// The <see cref="CefWindowInfo"/> instance to compare with the current instance.
		/// </param>
		/// <returns>
		/// A <see cref="Boolean"/> value that is true if the two instances are equal;
		/// otherwise, false.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (obj is CefWindowInfo windowInfo)
				return _instance == windowInfo._instance;
			return false;
		}

		/// <summary>
		/// Gets the hash code for the <see cref="CefWindowInfo"/>.
		/// </summary>
		/// <returns>An <see cref="Int32"/> containing the hash value.</returns>
		public override int GetHashCode()
		{
			return new IntPtr(_instance).GetHashCode();
		}

	}
}
