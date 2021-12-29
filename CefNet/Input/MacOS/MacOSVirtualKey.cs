// Source: /Library/Developer/CommandLineTools/SDKs/MacOSX.sdk/System/Library/Frameworks/Carbon.framework/Versions/A/Frameworks/HIToolbox.framework/Versions/A/Headers/Events.h

namespace CefNet.Input.MacOS
{
	/// <summary>
	/// Specifies macOS virtual keycodes.
	/// </summary>
	public enum MacOSVirtualKey
	{
		/// <summary>
		/// Invalid key code.
		/// </summary>
		Invalid = -1,

		/// <summary>The A key.</summary>
		A = 0x00,
		/// <summary>The S key.</summary>
		S = 0x01,
		/// <summary>The D key.</summary>
		D = 0x02,
		/// <summary>The F key.</summary>
		F = 0x03,
		/// <summary>The H key.</summary>
		H = 0x04,
		/// <summary>The G key.</summary>
		G = 0x05,
		/// <summary>The Z key.</summary>
		Z = 0x06,
		/// <summary>The X key.</summary>
		X = 0x07,
		/// <summary>The C key.</summary>
		C = 0x08,
		/// <summary>The V key.</summary>
		V = 0x09,
		/// <summary>The B key.</summary>
		B = 0x0B,
		/// <summary>The Q key.</summary>
		Q = 0x0C,
		/// <summary>The W key.</summary>
		W = 0x0D,
		/// <summary>The E key.</summary>
		E = 0x0E,
		/// <summary>The R key.</summary>
		R = 0x0F,
		/// <summary>The Y key.</summary>
		Y = 0x10,
		/// <summary>The T key.</summary>
		T = 0x11,
		/// <summary>The 1 key.</summary>
		D1 = 0x12,
		/// <summary>The 2 key.</summary>
		D2 = 0x13,
		/// <summary>The 3 key.</summary>
		D3 = 0x14,
		/// <summary>The 4 key.</summary>
		D4 = 0x15,
		/// <summary>The 6 key.</summary>
		D6 = 0x16,
		/// <summary>The 5 key.</summary>
		D5 = 0x17,
		/// <summary>The equal key.</summary>
		Equal = 0x18,
		/// <summary>The 9 key.</summary>
		D9 = 0x19,
		/// <summary>The 7 key.</summary>
		D7 = 0x1A,
		/// <summary>The minus key.</summary>
		Minus = 0x1B,
		/// <summary>The 8 key.</summary>
		D8 = 0x1C,
		/// <summary>The 0 key.</summary>
		D0 = 0x1D,
		/// <summary>The right bracket key.</summary>
		RightBracket = 0x1E,
		/// <summary>The O key.</summary>
		O = 0x1F,
		/// <summary>The U key.</summary>
		U = 0x20,
		/// <summary>The left bracket key.</summary>
		LeftBracket = 0x21,
		/// <summary>The I key.</summary>
		I = 0x22,
		/// <summary>The P key.</summary>
		P = 0x23,
		/// <summary>The L key.</summary>
		L = 0x25,
		/// <summary>The J key.</summary>
		J = 0x26,
		/// <summary>The quote key.</summary>
		Quote = 0x27,
		/// <summary>The K key.</summary>
		K = 0x28,
		/// <summary>The semicolon key.</summary>
		Semicolon = 0x29,
		/// <summary>The backslash key.</summary>
		Backslash = 0x2A,
		/// <summary>The comma key.</summary>
		Comma = 0x2B,
		/// <summary>The slash key.</summary>
		Slash = 0x2C,
		/// <summary>The N key.</summary>
		N = 0x2D,
		/// <summary>The M key.</summary>
		M = 0x2E,
		/// <summary>The period key.</summary>
		Period = 0x2F,
		/// <summary>The grave key.</summary>
		Grave = 0x32,
		/// <summary>The decimal key on the numeric keypad.</summary>
		KeypadDecimal = 0x41,
		/// <summary>The multiple key on the numeric keypad.</summary>
		KeypadMultiply = 0x43,
		/// <summary>The plus key on the numeric keypad.</summary>
		KeypadPlus = 0x45,
		/// <summary>The clear key on the numeric keypad.</summary>
		KeypadClear = 0x47,
		/// <summary>The devide key on the numeric keypad.</summary>
		KeypadDivide = 0x4B,
		/// <summary>The enter key on the numeric keypad.</summary>
		KeypadEnter = 0x4C,
		/// <summary>The minus key on the numeric keypad.</summary>
		KeypadMinus = 0x4E,
		/// <summary>The equal key on the numeric keypad.</summary>
		KeypadEquals = 0x51,
		/// <summary>The 0 key on the numeric keypad.</summary>
		Keypad0 = 0x52,
		/// <summary>The 1 key on the numeric keypad.</summary>
		Keypad1 = 0x53,
		/// <summary>The 2 key on the numeric keypad.</summary>
		Keypad2 = 0x54,
		/// <summary>The 3 key on the numeric keypad.</summary>
		Keypad3 = 0x55,
		/// <summary>The 4 key on the numeric keypad.</summary>
		Keypad4 = 0x56,
		/// <summary>The 5 key on the numeric keypad.</summary>
		Keypad5 = 0x57,
		/// <summary>The 6 key on the numeric keypad.</summary>
		Keypad6 = 0x58,
		/// <summary>The 7 key on the numeric keypad.</summary>
		Keypad7 = 0x59,
		/// <summary>The 8 key on the numeric keypad.</summary>
		Keypad8 = 0x5B,
		/// <summary>The 9 key on the numeric keypad.</summary>
		Keypad9 = 0x5C,

		/// <summary>The RETURN key.</summary>
		Return = 0x24,
		/// <summary>The TAB key.</summary>
		Tab = 0x30,
		/// <summary>The SPACE key.</summary>
		Space = 0x31,
		/// <summary>The DELETE key.</summary>
		Delete = 0x33,
		/// <summary>The ESC key.</summary>
		Escape = 0x35,
		/// <summary>The COMMAND key.</summary>
		Command = 0x37,
		/// <summary>The SHIFT key.</summary>
		Shift = 0x38,
		/// <summary>The CAPS LOCK key.</summary>
		CapsLock = 0x39,
		/// <summary>The OPTION key.</summary>
		Option = 0x3A,
		/// <summary>The CTRL key.</summary>
		Control = 0x3B,
		/// <summary>The RIGHT SHIFT key.</summary>
		RightShift = 0x3C,
		/// <summary>The RIGHT OPTION key.</summary>
		RightOption = 0x3D,
		/// <summary>The RIGTH CTRL key.</summary>
		RightControl = 0x3E,
		/// <summary>The FUNCTION key.</summary>
		Function = 0x3F,
		/// <summary>The F17 key.</summary>
		F17 = 0x40,
		/// <summary>The volume up key.</summary>
		VolumeUp = 0x48,
		/// <summary>The volume down key.</summary>
		VolumeDown = 0x49,
		/// <summary>The volume mute key.</summary>
		Mute = 0x4A,
		/// <summary>The F18 key.</summary>
		F18 = 0x4F,
		/// <summary>The F19 key.</summary>
		F19 = 0x50,
		/// <summary>The F20 key.</summary>
		F20 = 0x5A,
		/// <summary>The F5 key.</summary>
		F5 = 0x60,
		/// <summary>The F6 key.</summary>
		F6 = 0x61,
		/// <summary>The F7 key.</summary>
		F7 = 0x62,
		/// <summary>The F3 key.</summary>
		F3 = 0x63,
		/// <summary>The F8 key.</summary>
		F8 = 0x64,
		/// <summary>The F9 key.</summary>
		F9 = 0x65,
		/// <summary>The F11 key.</summary>
		F11 = 0x67,
		/// <summary>The F13 key.</summary>
		F13 = 0x69,
		/// <summary>The F16 key.</summary>
		F16 = 0x6A,
		/// <summary>The F14 key.</summary>
		F14 = 0x6B,
		/// <summary>The F10 key.</summary>
		F10 = 0x6D,
		/// <summary>The F12 key.</summary>
		F12 = 0x6F,
		/// <summary>The F15 key.</summary>
		F15 = 0x71,
		/// <summary>The HELP key.</summary>
		Help = 0x72,
		/// <summary>The HOME key.</summary>
		Home = 0x73,
		/// <summary>The PAGE UP key.</summary>
		PageUp = 0x74,
		/// <summary>The DELETE key (below the HELP Key).</summary>
		ForwardDelete = 0x75,
		/// <summary>The F4 key.</summary>
		F4 = 0x76,
		/// <summary>The END key.</summary>
		End = 0x77,
		/// <summary>The F2 key.</summary>
		F2 = 0x78,
		/// <summary>The PAGE DOWN key.</summary>
		PageDown = 0x79,
		/// <summary>The F1 key.</summary>
		F1 = 0x7A,
		/// <summary>The LEFT ARROW key.</summary>
		LeftArrow = 0x7B,
		/// <summary>The RIGHT ARROW key.</summary>
		RightArrow = 0x7C,
		/// <summary>The DOWN ARROW key.</summary>
		DownArrow = 0x7D,
		/// <summary>The UP ARRUW key.</summary>
		UpArrow = 0x7E,

		// ISO keyboards only

		/// <summary>The ISO key.</summary>
		ISOSection = 0x0A,

		// JIS keyboards only

		/// <summary>The YEN key.</summary>
		JISYen = 0x5D,
		/// <summary>The UNDERSCORE key.</summary>
		JISUnderscore = 0x5E,
		/// <summary>The COMMA key on the keypad.</summary>
		JISKeypadComma = 0x5F,
		/// <summary>The COMMA key on the keypad.</summary>
		JISEisu = 0x66,
		/// <summary>The KANA key.</summary>
		JISKana = 0x68,
	}
}
