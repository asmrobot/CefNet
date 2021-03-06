namespace CefNet.Input.MacOS
{
	/// <summary>
	/// Provides static methods to convert keys.
	/// </summary>
	public static class KeyInterop
	{
		/// <summary>
		/// Converts a macOS virtual keycode into a Windows virtual Key.
		/// </summary>
		/// <param name="key">The <see cref="MacOSVirtualKey"/> to convert.</param>
		/// <returns>The virtual key.</returns>
		public static VirtualKeys MacOSKeyToWindowsKey(MacOSVirtualKey key)
		{
			switch (key)
			{
				case MacOSVirtualKey.A:
					return VirtualKeys.A;
				case MacOSVirtualKey.S:
					return VirtualKeys.S;
				case MacOSVirtualKey.D:
					return VirtualKeys.D;
				case MacOSVirtualKey.F:
					return VirtualKeys.F;
				case MacOSVirtualKey.H:
					return VirtualKeys.H;
				case MacOSVirtualKey.G:
					return VirtualKeys.G;
				case MacOSVirtualKey.Z:
					return VirtualKeys.Z;
				case MacOSVirtualKey.X:
					return VirtualKeys.X;
				case MacOSVirtualKey.C:
					return VirtualKeys.C;
				case MacOSVirtualKey.V:
					return VirtualKeys.V;
				case MacOSVirtualKey.B:
					return VirtualKeys.B;
				case MacOSVirtualKey.Q:
					return VirtualKeys.Q;
				case MacOSVirtualKey.W:
					return VirtualKeys.W;
				case MacOSVirtualKey.E:
					return VirtualKeys.E;
				case MacOSVirtualKey.R:
					return VirtualKeys.R;
				case MacOSVirtualKey.Y:
					return VirtualKeys.Y;
				case MacOSVirtualKey.T:
					return VirtualKeys.T;
				case MacOSVirtualKey.D1:
					return VirtualKeys.D1;
				case MacOSVirtualKey.D2:
					return VirtualKeys.D2;
				case MacOSVirtualKey.D3:
					return VirtualKeys.D3;
				case MacOSVirtualKey.D4:
					return VirtualKeys.D4;
				case MacOSVirtualKey.D6:
					return VirtualKeys.D6;
				case MacOSVirtualKey.D5:
					return VirtualKeys.D5;
				case MacOSVirtualKey.Equal:
					return VirtualKeys.OemPlus;
				case MacOSVirtualKey.D9:
					return VirtualKeys.D9;
				case MacOSVirtualKey.D7:
					return VirtualKeys.D7;
				case MacOSVirtualKey.Minus:
					return VirtualKeys.OemMinus;
				case MacOSVirtualKey.D8:
					return VirtualKeys.D8;
				case MacOSVirtualKey.D0:
					return VirtualKeys.D0;
				case MacOSVirtualKey.RightBracket:
					return VirtualKeys.OemCloseBrackets;
				case MacOSVirtualKey.O:
					return VirtualKeys.O;
				case MacOSVirtualKey.U:
					return VirtualKeys.U;
				case MacOSVirtualKey.LeftBracket:
					return VirtualKeys.OemOpenBrackets;
				case MacOSVirtualKey.I:
					return VirtualKeys.I;
				case MacOSVirtualKey.P:
					return VirtualKeys.P;
				case MacOSVirtualKey.L:
					return VirtualKeys.L;
				case MacOSVirtualKey.J:
					return VirtualKeys.J;
				case MacOSVirtualKey.Quote:
					return VirtualKeys.OemQuotes;
				case MacOSVirtualKey.K:
					return VirtualKeys.K;
				case MacOSVirtualKey.Semicolon:
					return VirtualKeys.OemSemicolon;
				case MacOSVirtualKey.Backslash:
					return VirtualKeys.OemBackslash;
				case MacOSVirtualKey.Comma:
					return VirtualKeys.OemComma;
				case MacOSVirtualKey.Slash:
					return VirtualKeys.Oem2;
				case MacOSVirtualKey.N:
					return VirtualKeys.N;
				case MacOSVirtualKey.M:
					return VirtualKeys.M;
				case MacOSVirtualKey.Period:
					return VirtualKeys.OemPeriod;
				case MacOSVirtualKey.Grave:
					return VirtualKeys.OemTilde;
				case MacOSVirtualKey.KeypadDecimal:
					return VirtualKeys.OemComma;
				case MacOSVirtualKey.KeypadMultiply:
					return VirtualKeys.Multiply;
				case MacOSVirtualKey.KeypadPlus:
					return VirtualKeys.OemPlus;
				case MacOSVirtualKey.KeypadClear:
					return VirtualKeys.Clear;
				case MacOSVirtualKey.KeypadDivide:
					return VirtualKeys.Divide;
				case MacOSVirtualKey.KeypadEnter:
					return VirtualKeys.Enter;
				case MacOSVirtualKey.KeypadMinus:
					return VirtualKeys.OemMinus;
				case MacOSVirtualKey.KeypadEquals:
					return VirtualKeys.Add;
				case MacOSVirtualKey.Keypad0:
					return VirtualKeys.NumPad0;
				case MacOSVirtualKey.Keypad1:
					return VirtualKeys.NumPad1;
				case MacOSVirtualKey.Keypad2:
					return VirtualKeys.NumPad2;
				case MacOSVirtualKey.Keypad3:
					return VirtualKeys.NumPad3;
				case MacOSVirtualKey.Keypad4:
					return VirtualKeys.NumPad4;
				case MacOSVirtualKey.Keypad5:
					return VirtualKeys.NumPad5;
				case MacOSVirtualKey.Keypad6:
					return VirtualKeys.NumPad6;
				case MacOSVirtualKey.Keypad7:
					return VirtualKeys.NumPad7;
				case MacOSVirtualKey.Keypad8:
					return VirtualKeys.NumPad8;
				case MacOSVirtualKey.Keypad9:
					return VirtualKeys.NumPad9;
				case MacOSVirtualKey.Return:
					return VirtualKeys.Return;
				case MacOSVirtualKey.Tab:
					return VirtualKeys.Tab;
				case MacOSVirtualKey.Space:
					return VirtualKeys.Space;
				case MacOSVirtualKey.Delete:
					return VirtualKeys.Back;
				case MacOSVirtualKey.Escape:
					return VirtualKeys.Escape;
				case MacOSVirtualKey.Command:
					return VirtualKeys.LWin;
				case MacOSVirtualKey.Shift:
					return VirtualKeys.LShiftKey;
				case MacOSVirtualKey.CapsLock:
					return VirtualKeys.CapsLock;
				case MacOSVirtualKey.Option:
					return VirtualKeys.LMenu;
				case MacOSVirtualKey.Control:
					return VirtualKeys.LControlKey;
				case MacOSVirtualKey.RightShift:
					return VirtualKeys.RShiftKey;
				case MacOSVirtualKey.RightOption:
					return VirtualKeys.RMenu;
				case MacOSVirtualKey.RightControl:
					return VirtualKeys.RControlKey;
				case MacOSVirtualKey.F17:
					return VirtualKeys.F17;
				case MacOSVirtualKey.VolumeUp:
					return VirtualKeys.VolumeUp;
				case MacOSVirtualKey.VolumeDown:
					return VirtualKeys.VolumeDown;
				case MacOSVirtualKey.Mute:
					return VirtualKeys.VolumeMute;
				case MacOSVirtualKey.F18:
					return VirtualKeys.F18;
				case MacOSVirtualKey.F19:
					return VirtualKeys.F19;
				case MacOSVirtualKey.F20:
					return VirtualKeys.F20;
				case MacOSVirtualKey.F5:
					return VirtualKeys.F5;
				case MacOSVirtualKey.F6:
					return VirtualKeys.F6;
				case MacOSVirtualKey.F7:
					return VirtualKeys.F7;
				case MacOSVirtualKey.F3:
					return VirtualKeys.F3;
				case MacOSVirtualKey.F8:
					return VirtualKeys.F8;
				case MacOSVirtualKey.F9:
					return VirtualKeys.F9;
				case MacOSVirtualKey.F11:
					return VirtualKeys.F11;
				case MacOSVirtualKey.F13:
					return VirtualKeys.F13;
				case MacOSVirtualKey.F16:
					return VirtualKeys.F16;
				case MacOSVirtualKey.F14:
					return VirtualKeys.F14;
				case MacOSVirtualKey.F10:
					return VirtualKeys.F10;
				case MacOSVirtualKey.F12:
					return VirtualKeys.F12;
				case MacOSVirtualKey.F15:
					return VirtualKeys.F15;
				case MacOSVirtualKey.Help:
					return VirtualKeys.Help;
				case MacOSVirtualKey.Home:
					return VirtualKeys.Home;
				case MacOSVirtualKey.PageUp:
					return VirtualKeys.PageUp;
				case MacOSVirtualKey.ForwardDelete:
					return VirtualKeys.Delete;
				case MacOSVirtualKey.F4:
					return VirtualKeys.F4;
				case MacOSVirtualKey.End:
					return VirtualKeys.End;
				case MacOSVirtualKey.F2:
					return VirtualKeys.F2;
				case MacOSVirtualKey.PageDown:
					return VirtualKeys.PageDown;
				case MacOSVirtualKey.F1:
					return VirtualKeys.F1;
				case MacOSVirtualKey.LeftArrow:
					return VirtualKeys.Left;
				case MacOSVirtualKey.RightArrow:
					return VirtualKeys.Right;
				case MacOSVirtualKey.DownArrow:
					return VirtualKeys.Down;
				case MacOSVirtualKey.UpArrow:
					return VirtualKeys.Up;
				case MacOSVirtualKey.JISKana:
					return VirtualKeys.KanaMode;
			}

			return VirtualKeys.None;
		}

		/// <summary>
		/// Converts a Windows virtual key into a macOS virtual keycode.
		/// </summary>
		/// <param name="key">The <see cref="VirtualKeys"/> to convert.</param>
		/// <param name="extended">
		/// A value indicating that the key is pressed on the numeric keypad.
		/// </param>
		/// <returns>The virtual key.</returns>
		public static MacOSVirtualKey WindowsKeyToMacOSKey(VirtualKeys key, bool extended)
		{
			switch (key)
			{
				case VirtualKeys.A:
					return MacOSVirtualKey.A;
				case VirtualKeys.S:
					return MacOSVirtualKey.S;
				case VirtualKeys.D:
					return MacOSVirtualKey.D;
				case VirtualKeys.F:
					return MacOSVirtualKey.F;
				case VirtualKeys.H:
					return MacOSVirtualKey.H;
				case VirtualKeys.G:
					return MacOSVirtualKey.G;
				case VirtualKeys.Z:
					return MacOSVirtualKey.Z;
				case VirtualKeys.X:
					return MacOSVirtualKey.X;
				case VirtualKeys.C:
					return MacOSVirtualKey.C;
				case VirtualKeys.V:
					return MacOSVirtualKey.V;
				case VirtualKeys.B:
					return MacOSVirtualKey.B;
				case VirtualKeys.Q:
					return MacOSVirtualKey.Q;
				case VirtualKeys.W:
					return MacOSVirtualKey.W;
				case VirtualKeys.E:
					return MacOSVirtualKey.E;
				case VirtualKeys.R:
					return MacOSVirtualKey.R;
				case VirtualKeys.Y:
					return MacOSVirtualKey.Y;
				case VirtualKeys.T:
					return MacOSVirtualKey.T;
				case VirtualKeys.D1:
					return MacOSVirtualKey.D1;
				case VirtualKeys.D2:
					return MacOSVirtualKey.D2;
				case VirtualKeys.D3:
					return MacOSVirtualKey.D3;
				case VirtualKeys.D4:
					return MacOSVirtualKey.D4;
				case VirtualKeys.D6:
					return MacOSVirtualKey.D6;
				case VirtualKeys.D5:
					return MacOSVirtualKey.D5;
				case VirtualKeys.OemPlus:
					return extended ? MacOSVirtualKey.KeypadEquals : MacOSVirtualKey.Equal;
				case VirtualKeys.D9:
					return MacOSVirtualKey.D9;
				case VirtualKeys.D7:
					return MacOSVirtualKey.D7;
				case VirtualKeys.OemMinus:
					return extended ? MacOSVirtualKey.KeypadMinus : MacOSVirtualKey.Minus;
				case VirtualKeys.D8:
					return MacOSVirtualKey.D8;
				case VirtualKeys.D0:
					return MacOSVirtualKey.D0;
				case VirtualKeys.OemCloseBrackets:
					return MacOSVirtualKey.RightBracket;
				case VirtualKeys.O:
					return MacOSVirtualKey.O;
				case VirtualKeys.U:
					return MacOSVirtualKey.U;
				case VirtualKeys.OemOpenBrackets:
					return MacOSVirtualKey.LeftBracket;
				case VirtualKeys.I:
					return MacOSVirtualKey.I;
				case VirtualKeys.P:
					return MacOSVirtualKey.P;
				case VirtualKeys.L:
					return MacOSVirtualKey.L;
				case VirtualKeys.J:
					return MacOSVirtualKey.J;
				case VirtualKeys.OemQuotes:
					return MacOSVirtualKey.Quote;
				case VirtualKeys.K:
					return MacOSVirtualKey.K;
				case VirtualKeys.OemSemicolon:
					return MacOSVirtualKey.Semicolon;
				case VirtualKeys.OemBackslash:
					return MacOSVirtualKey.Backslash;
				case VirtualKeys.OemComma:
					return extended ? MacOSVirtualKey.KeypadDecimal : MacOSVirtualKey.Comma;
				case VirtualKeys.Divide:
					return MacOSVirtualKey.KeypadDivide;
				case VirtualKeys.Oem2:
					return MacOSVirtualKey.Slash;
				case VirtualKeys.N:
					return MacOSVirtualKey.N;
				case VirtualKeys.M:
					return MacOSVirtualKey.M;
				case VirtualKeys.OemPeriod:
					return MacOSVirtualKey.Period;
				case VirtualKeys.OemTilde:
					return MacOSVirtualKey.Grave;
				case VirtualKeys.Multiply:
					return MacOSVirtualKey.KeypadMultiply;
				case VirtualKeys.Clear:
					return MacOSVirtualKey.KeypadClear;
				case VirtualKeys.Enter:
					return extended ? MacOSVirtualKey.KeypadEnter : MacOSVirtualKey.Return;
				case VirtualKeys.Add:
					return MacOSVirtualKey.KeypadEquals;
				case VirtualKeys.NumPad0:
					return MacOSVirtualKey.Keypad0;
				case VirtualKeys.NumPad1:
					return MacOSVirtualKey.Keypad1;
				case VirtualKeys.NumPad2:
					return MacOSVirtualKey.Keypad2;
				case VirtualKeys.NumPad3:
					return MacOSVirtualKey.Keypad3;
				case VirtualKeys.NumPad4:
					return MacOSVirtualKey.Keypad4;
				case VirtualKeys.NumPad5:
					return MacOSVirtualKey.Keypad5;
				case VirtualKeys.NumPad6:
					return MacOSVirtualKey.Keypad6;
				case VirtualKeys.NumPad7:
					return MacOSVirtualKey.Keypad7;
				case VirtualKeys.NumPad8:
					return MacOSVirtualKey.Keypad8;
				case VirtualKeys.NumPad9:
					return MacOSVirtualKey.Keypad9;
				case VirtualKeys.Tab:
					return MacOSVirtualKey.Tab;
				case VirtualKeys.Space:
					return MacOSVirtualKey.Space;
				case VirtualKeys.Delete:
					return MacOSVirtualKey.ForwardDelete;
				case VirtualKeys.Back:
					return MacOSVirtualKey.Delete;
				case VirtualKeys.Escape:
					return MacOSVirtualKey.Escape;
				case VirtualKeys.LWin:
					return MacOSVirtualKey.Command;
				case VirtualKeys.ShiftKey:
					return MacOSVirtualKey.Shift;
				case VirtualKeys.LShiftKey:
					return MacOSVirtualKey.Shift;
				case VirtualKeys.CapsLock:
					return MacOSVirtualKey.CapsLock;
				case VirtualKeys.Menu:
					return MacOSVirtualKey.Option;
				case VirtualKeys.LMenu:
					return MacOSVirtualKey.Option;
				case VirtualKeys.LControlKey:
					return MacOSVirtualKey.Control;
				case VirtualKeys.RShiftKey:
					return MacOSVirtualKey.RightShift;
				case VirtualKeys.RMenu:
					return MacOSVirtualKey.RightOption;
				case VirtualKeys.RControlKey:
					return MacOSVirtualKey.RightControl;
				case VirtualKeys.F17:
					return MacOSVirtualKey.F17;
				case VirtualKeys.VolumeUp:
					return MacOSVirtualKey.VolumeUp;
				case VirtualKeys.VolumeDown:
					return MacOSVirtualKey.VolumeDown;
				case VirtualKeys.VolumeMute:
					return MacOSVirtualKey.Mute;
				case VirtualKeys.F18:
					return MacOSVirtualKey.F18;
				case VirtualKeys.F19:
					return MacOSVirtualKey.F19;
				case VirtualKeys.F20:
					return MacOSVirtualKey.F20;
				case VirtualKeys.F5:
					return MacOSVirtualKey.F5;
				case VirtualKeys.F6:
					return MacOSVirtualKey.F6;
				case VirtualKeys.F7:
					return MacOSVirtualKey.F7;
				case VirtualKeys.F3:
					return MacOSVirtualKey.F3;
				case VirtualKeys.F8:
					return MacOSVirtualKey.F8;
				case VirtualKeys.F9:
					return MacOSVirtualKey.F9;
				case VirtualKeys.F11:
					return MacOSVirtualKey.F11;
				case VirtualKeys.F13:
					return MacOSVirtualKey.F13;
				case VirtualKeys.F16:
					return MacOSVirtualKey.F16;
				case VirtualKeys.F14:
					return MacOSVirtualKey.F14;
				case VirtualKeys.F10:
					return MacOSVirtualKey.F10;
				case VirtualKeys.F12:
					return MacOSVirtualKey.F12;
				case VirtualKeys.F15:
					return MacOSVirtualKey.F15;
				case VirtualKeys.Help:
					return MacOSVirtualKey.Help;
				case VirtualKeys.Home:
					return MacOSVirtualKey.Home;
				case VirtualKeys.PageUp:
					return MacOSVirtualKey.PageUp;
				case VirtualKeys.F4:
					return MacOSVirtualKey.F4;
				case VirtualKeys.End:
					return MacOSVirtualKey.End;
				case VirtualKeys.F2:
					return MacOSVirtualKey.F2;
				case VirtualKeys.PageDown:
					return MacOSVirtualKey.PageDown;
				case VirtualKeys.F1:
					return MacOSVirtualKey.F1;
				case VirtualKeys.Left:
					return MacOSVirtualKey.LeftArrow;
				case VirtualKeys.Right:
					return MacOSVirtualKey.RightArrow;
				case VirtualKeys.Down:
					return MacOSVirtualKey.DownArrow;
				case VirtualKeys.Up:
					return MacOSVirtualKey.UpArrow;
				case VirtualKeys.KanaMode:
					return MacOSVirtualKey.JISKana;

			}

			return MacOSVirtualKey.Invalid;
		}

	}
}
