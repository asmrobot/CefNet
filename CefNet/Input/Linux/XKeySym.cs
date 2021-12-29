
namespace CefNet.Input.Linux
{
	/// <summary>
	/// Specifies the values for X KeySyms.
	/// </summary>
	public enum XKeySym
	{
		None = 0,

		/*** Defined in /usr/include/X11/keysymdef.h ***/

		/// <summary>Void symbol</summary>
		VoidSymbol = 0xffffff,

		// #ifdef XK_MISCELLANY
		/*
		* TTY function keys, cleverly chosen to map to ASCII, for convenience of
		* programming, but could have been arbitrary (at the cost of lookup
		* tables in client code).
		*/

		/// <summary>Back space, back char</summary>
		BackSpace = 0xff08,
		/// <summary></summary>
		Tab = 0xff09,
		/// <summary>Linefeed, LF</summary>
		Linefeed = 0xff0a,
		/// <summary></summary>
		Clear = 0xff0b,
		/// <summary>Return, enter</summary>
		Return = 0xff0d,
		/// <summary>Pause, hold</summary>
		Pause = 0xff13,
		/// <summary></summary>
		ScrollLock = 0xff14,
		/// <summary></summary>
		SysReq = 0xff15,
		/// <summary></summary>
		Escape = 0xff1b,
		/// <summary>Delete, rubout</summary>
		Delete = 0xffff,



		/* International & multi-key character composition */

		/// <summary>Multi-key character compose</summary>
		MultiKey = 0xff20,
		/// <summary></summary>
		Codeinput = 0xff37,
		/// <summary></summary>
		SingleCandidate = 0xff3c,
		/// <summary></summary>
		MultipleCandidate = 0xff3d,
		/// <summary></summary>
		PreviousCandidate = 0xff3e,

		/* Japanese keyboard support */

		/// <summary>Kanji, Kanji convert</summary>
		Kanji = 0xff21,
		/// <summary>Cancel Conversion</summary>
		Muhenkan = 0xff22,
		/// <summary>Start/Stop Conversion</summary>
		HenkanMode = 0xff23,
		/// <summary>Alias for Henkan_Mode</summary>
		Henkan = 0xff23,
		/// <summary>to Romaji</summary>
		Romaji = 0xff24,
		/// <summary>to Hiragana</summary>
		Hiragana = 0xff25,
		/// <summary>to Katakana</summary>
		Katakana = 0xff26,
		/// <summary>Hiragana/Katakana toggle</summary>
		HiraganaKatakana = 0xff27,
		/// <summary>to Zenkaku</summary>
		Zenkaku = 0xff28,
		/// <summary>to Hankaku</summary>
		Hankaku = 0xff29,
		/// <summary>Zenkaku/Hankaku toggle</summary>
		ZenkakuHankaku = 0xff2a,
		/// <summary>Add to Dictionary</summary>
		Touroku = 0xff2b,
		/// <summary>Delete from Dictionary</summary>
		Massyo = 0xff2c,
		/// <summary>Kana Lock</summary>
		KanaLock = 0xff2d,
		/// <summary>Kana Shift</summary>
		KanaShift = 0xff2e,
		/// <summary>Alphanumeric Shift</summary>
		EisuShift = 0xff2f,
		/// <summary>Alphanumeric toggle</summary>
		EisuToggle = 0xff30,
		/// <summary>Codeinput</summary>
		KanjiBangou = 0xff37,
		/// <summary>Multiple/All Candidate(s)</summary>
		ZenKoho = 0xff3d,
		/// <summary>Previous Candidate</summary>
		MaeKoho = 0xff3e,

		/* 0xff31 thru 0xff3f are under XK_KOREAN */

		/* Cursor control & motion */

		/// <summary></summary>
		Home = 0xff50,
		/// <summary>Move left, left arrow</summary>
		Left = 0xff51,
		/// <summary>Move up, up arrow</summary>
		Up = 0xff52,
		/// <summary>Move right, right arrow</summary>
		Right = 0xff53,
		/// <summary>Move down, down arrow</summary>
		Down = 0xff54,
		/// <summary>Prior, previous</summary>
		Prior = 0xff55,
		/// <summary></summary>
		PageUp = 0xff55,
		/// <summary>Next</summary>
		Next = 0xff56,
		/// <summary></summary>
		PageDown = 0xff56,
		/// <summary>EOL</summary>
		End = 0xff57,
		/// <summary>BOL</summary>
		Begin = 0xff58,


		/* Misc functions */

		/// <summary>Select, mark</summary>
		Select = 0xff60,
		/// <summary></summary>
		Print = 0xff61,
		/// <summary>Execute, run, do</summary>
		Execute = 0xff62,
		/// <summary>Insert, insert here</summary>
		Insert = 0xff63,
		/// <summary></summary>
		Undo = 0xff65,
		/// <summary>Redo, again</summary>
		Redo = 0xff66,
		/// <summary></summary>
		Menu = 0xff67,
		/// <summary>Find, search</summary>
		Find = 0xff68,
		/// <summary>Cancel, stop, abort, exit</summary>
		Cancel = 0xff69,
		/// <summary>Help</summary>
		Help = 0xff6a,
		/// <summary></summary>
		Break = 0xff6b,
		/// <summary>Character set switch</summary>
		ModeSwitch = 0xff7e,
		/// <summary>Alias for mode_switch</summary>
		ScriptSwitch = 0xff7e,
		/// <summary></summary>
		NumLock = 0xff7f,

		/* Keypad functions, keypad numbers cleverly chosen to map to ASCII */

		/// <summary>Space</summary>
		KPSpace = 0xff80,
		/// <summary></summary>
		KPTab = 0xff89,
		/// <summary>Enter</summary>
		KPEnter = 0xff8d,
		/// <summary>PF1, KP_A, ...</summary>
		KPF1 = 0xff91,
		/// <summary></summary>
		KPF2 = 0xff92,
		/// <summary></summary>
		KPF3 = 0xff93,
		/// <summary></summary>
		KPF4 = 0xff94,
		/// <summary></summary>
		KPHome = 0xff95,
		/// <summary></summary>
		KPLeft = 0xff96,
		/// <summary></summary>
		KPUp = 0xff97,
		/// <summary></summary>
		KPRight = 0xff98,
		/// <summary></summary>
		KPDown = 0xff99,
		/// <summary></summary>
		KPPrior = 0xff9a,
		/// <summary></summary>
		KPPageUp = 0xff9a,
		/// <summary></summary>
		KPNext = 0xff9b,
		/// <summary></summary>
		KPPageDown = 0xff9b,
		/// <summary></summary>
		KPEnd = 0xff9c,
		/// <summary></summary>
		KPBegin = 0xff9d,
		/// <summary></summary>
		KPInsert = 0xff9e,
		/// <summary></summary>
		KPDelete = 0xff9f,
		/// <summary>Equals</summary>
		KPEqual = 0xffbd,
		/// <summary></summary>
		KPMultiply = 0xffaa,
		/// <summary></summary>
		KPAdd = 0xffab,
		/// <summary>Separator, often comma</summary>
		KPSeparator = 0xffac,
		/// <summary></summary>
		KPSubtract = 0xffad,
		/// <summary></summary>
		KPDecimal = 0xffae,
		/// <summary></summary>
		KPDivide = 0xffaf,

		/// <summary></summary>
		KP0 = 0xffb0,
		/// <summary></summary>
		KP1 = 0xffb1,
		/// <summary></summary>
		KP2 = 0xffb2,
		/// <summary></summary>
		KP3 = 0xffb3,
		/// <summary></summary>
		KP4 = 0xffb4,
		/// <summary></summary>
		KP5 = 0xffb5,
		/// <summary></summary>
		KP6 = 0xffb6,
		/// <summary></summary>
		KP7 = 0xffb7,
		/// <summary></summary>
		KP8 = 0xffb8,
		/// <summary></summary>
		KP9 = 0xffb9,



		/*
		* Auxiliary functions; note the duplicate definitions for left and right
		* function keys;  Sun keyboards and a few other manufacturers have such
		* function key groups on the left and/or right sides of the keyboard.
		* We've not found a keyboard with more than 35 function keys total.
		*/

		/// <summary></summary>
		F1 = 0xffbe,
		/// <summary></summary>
		F2 = 0xffbf,
		/// <summary></summary>
		F3 = 0xffc0,
		/// <summary></summary>
		F4 = 0xffc1,
		/// <summary></summary>
		F5 = 0xffc2,
		/// <summary></summary>
		F6 = 0xffc3,
		/// <summary></summary>
		F7 = 0xffc4,
		/// <summary></summary>
		F8 = 0xffc5,
		/// <summary></summary>
		F9 = 0xffc6,
		/// <summary></summary>
		F10 = 0xffc7,
		/// <summary></summary>
		F11 = 0xffc8,
		/// <summary></summary>
		L1 = 0xffc8,
		/// <summary></summary>
		F12 = 0xffc9,
		/// <summary></summary>
		L2 = 0xffc9,
		/// <summary></summary>
		F13 = 0xffca,
		/// <summary></summary>
		L3 = 0xffca,
		/// <summary></summary>
		F14 = 0xffcb,
		/// <summary></summary>
		L4 = 0xffcb,
		/// <summary></summary>
		F15 = 0xffcc,
		/// <summary></summary>
		L5 = 0xffcc,
		/// <summary></summary>
		F16 = 0xffcd,
		/// <summary></summary>
		L6 = 0xffcd,
		/// <summary></summary>
		F17 = 0xffce,
		/// <summary></summary>
		L7 = 0xffce,
		/// <summary></summary>
		F18 = 0xffcf,
		/// <summary></summary>
		L8 = 0xffcf,
		/// <summary></summary>
		F19 = 0xffd0,
		/// <summary></summary>
		L9 = 0xffd0,
		/// <summary></summary>
		F20 = 0xffd1,
		/// <summary></summary>
		L10 = 0xffd1,
		/// <summary></summary>
		F21 = 0xffd2,
		/// <summary></summary>
		R1 = 0xffd2,
		/// <summary></summary>
		F22 = 0xffd3,
		/// <summary></summary>
		R2 = 0xffd3,
		/// <summary></summary>
		F23 = 0xffd4,
		/// <summary></summary>
		R3 = 0xffd4,
		/// <summary></summary>
		F24 = 0xffd5,
		/// <summary></summary>
		R4 = 0xffd5,
		/// <summary></summary>
		F25 = 0xffd6,
		/// <summary></summary>
		R5 = 0xffd6,
		/// <summary></summary>
		F26 = 0xffd7,
		/// <summary></summary>
		R6 = 0xffd7,
		/// <summary></summary>
		F27 = 0xffd8,
		/// <summary></summary>
		R7 = 0xffd8,
		/// <summary></summary>
		F28 = 0xffd9,
		/// <summary></summary>
		R8 = 0xffd9,
		/// <summary></summary>
		F29 = 0xffda,
		/// <summary></summary>
		R9 = 0xffda,
		/// <summary></summary>
		F30 = 0xffdb,
		/// <summary></summary>
		R10 = 0xffdb,
		/// <summary></summary>
		F31 = 0xffdc,
		/// <summary></summary>
		R11 = 0xffdc,
		/// <summary></summary>
		F32 = 0xffdd,
		/// <summary></summary>
		R12 = 0xffdd,
		/// <summary></summary>
		F33 = 0xffde,
		/// <summary></summary>
		R13 = 0xffde,
		/// <summary></summary>
		F34 = 0xffdf,
		/// <summary></summary>
		R14 = 0xffdf,
		/// <summary></summary>
		F35 = 0xffe0,
		/// <summary></summary>
		R15 = 0xffe0,

		/* Modifiers */

		/// <summary>Left shift</summary>
		ShiftL = 0xffe1,
		/// <summary>Right shift</summary>
		ShiftR = 0xffe2,
		/// <summary>Left control</summary>
		ControlL = 0xffe3,
		/// <summary>Right control</summary>
		ControlR = 0xffe4,
		/// <summary>Caps lock</summary>
		CapsLock = 0xffe5,
		/// <summary>Shift lock</summary>
		ShiftLock = 0xffe6,

		/// <summary>Left meta</summary>
		MetaL = 0xffe7,
		/// <summary>Right meta</summary>
		MetaR = 0xffe8,
		/// <summary>Left alt</summary>
		AltL = 0xffe9,
		/// <summary>Right alt</summary>
		AltR = 0xffea,
		/// <summary>Left super</summary>
		SuperL = 0xffeb,
		/// <summary>Right super</summary>
		SuperR = 0xffec,
		/// <summary>Left hyper</summary>
		HyperL = 0xffed,
		/// <summary>Right hyper</summary>
		HyperR = 0xffee,
		// #endif /* XK_MISCELLANY */

		/*
		* Keyboard (XKB) Extension function and modifier keys
		* (from Appendix C of "The X Keyboard Extension: Protocol Specification")
		* Byte 3 = 0xfe
		*/

		// #ifdef XK_XKB_KEYS
		/// <summary></summary>
		ISOLock = 0xfe01,
		/// <summary></summary>
		ISOLevel2Latch = 0xfe02,
		/// <summary></summary>
		ISOLevel3Shift = 0xfe03,
		/// <summary></summary>
		ISOLevel3Latch = 0xfe04,
		/// <summary></summary>
		ISOLevel3Lock = 0xfe05,
		/// <summary></summary>
		ISOLevel5Shift = 0xfe11,
		/// <summary></summary>
		ISOLevel5Latch = 0xfe12,
		/// <summary></summary>
		ISOLevel5Lock = 0xfe13,
		/// <summary>Alias for mode_switch</summary>
		ISOGroupShift = 0xff7e,
		/// <summary></summary>
		ISOGroupLatch = 0xfe06,
		/// <summary></summary>
		ISOGroupLock = 0xfe07,
		/// <summary></summary>
		ISONextGroup = 0xfe08,
		/// <summary></summary>
		ISONextGroupLock = 0xfe09,
		/// <summary></summary>
		ISOPrevGroup = 0xfe0a,
		/// <summary></summary>
		ISOPrevGroupLock = 0xfe0b,
		/// <summary></summary>
		ISOFirstGroup = 0xfe0c,
		/// <summary></summary>
		ISOFirstGroupLock = 0xfe0d,
		/// <summary></summary>
		ISOLastGroup = 0xfe0e,
		/// <summary></summary>
		ISOLastGroupLock = 0xfe0f,

		/// <summary></summary>
		ISOLeftTab = 0xfe20,
		/// <summary></summary>
		ISOMoveLineUp = 0xfe21,
		/// <summary></summary>
		ISOMoveLineDown = 0xfe22,
		/// <summary></summary>
		ISOPartialLineUp = 0xfe23,
		/// <summary></summary>
		ISOPartialLineDown = 0xfe24,
		/// <summary></summary>
		ISOPartialSpaceLeft = 0xfe25,
		/// <summary></summary>
		ISOPartialSpaceRight = 0xfe26,
		/// <summary></summary>
		ISOSetMarginLeft = 0xfe27,
		/// <summary></summary>
		ISOSetMarginRight = 0xfe28,
		/// <summary></summary>
		ISOReleaseMarginLeft = 0xfe29,
		/// <summary></summary>
		ISOReleaseMarginRight = 0xfe2a,
		/// <summary></summary>
		ISOReleaseBothMargins = 0xfe2b,
		/// <summary></summary>
		ISOFastCursorLeft = 0xfe2c,
		/// <summary></summary>
		ISOFastCursorRight = 0xfe2d,
		/// <summary></summary>
		ISOFastCursorUp = 0xfe2e,
		/// <summary></summary>
		ISOFastCursorDown = 0xfe2f,
		/// <summary></summary>
		ISOContinuousUnderline = 0xfe30,
		/// <summary></summary>
		ISODiscontinuousUnderline = 0xfe31,
		/// <summary></summary>
		ISOEmphasize = 0xfe32,
		/// <summary></summary>
		ISOCenterObject = 0xfe33,
		/// <summary></summary>
		ISOEnter = 0xfe34,

		/// <summary></summary>
		DeadGrave = 0xfe50,
		/// <summary></summary>
		DeadAcute = 0xfe51,
		/// <summary></summary>
		DeadCircumflex = 0xfe52,
		/// <summary></summary>
		DeadTilde = 0xfe53,
		/// <summary>alias for dead_tilde</summary>
		DeadPerispomeni = 0xfe53,
		/// <summary></summary>
		DeadMacron = 0xfe54,
		/// <summary></summary>
		DeadBreve = 0xfe55,
		/// <summary></summary>
		DeadAbovedot = 0xfe56,
		/// <summary></summary>
		DeadDiaeresis = 0xfe57,
		/// <summary></summary>
		DeadAbovering = 0xfe58,
		/// <summary></summary>
		DeadDoubleacute = 0xfe59,
		/// <summary></summary>
		DeadCaron = 0xfe5a,
		/// <summary></summary>
		DeadCedilla = 0xfe5b,
		/// <summary></summary>
		DeadOgonek = 0xfe5c,
		/// <summary></summary>
		DeadIota = 0xfe5d,
		/// <summary></summary>
		DeadVoicedSound = 0xfe5e,
		/// <summary></summary>
		DeadSemivoicedSound = 0xfe5f,
		/// <summary></summary>
		DeadBelowdot = 0xfe60,
		/// <summary></summary>
		DeadHook = 0xfe61,
		/// <summary></summary>
		DeadHorn = 0xfe62,
		/// <summary></summary>
		DeadStroke = 0xfe63,
		/// <summary></summary>
		DeadAbovecomma = 0xfe64,
		/// <summary>alias for dead_abovecomma</summary>
		DeadPsili = 0xfe64,
		/// <summary></summary>
		DeadAbovereversedcomma = 0xfe65,
		/// <summary>alias for dead_abovereversedcomma</summary>
		DeadDasia = 0xfe65,
		/// <summary></summary>
		DeadDoublegrave = 0xfe66,
		/// <summary></summary>
		DeadBelowring = 0xfe67,
		/// <summary></summary>
		DeadBelowmacron = 0xfe68,
		/// <summary></summary>
		DeadBelowcircumflex = 0xfe69,
		/// <summary></summary>
		DeadBelowtilde = 0xfe6a,
		/// <summary></summary>
		DeadBelowbreve = 0xfe6b,
		/// <summary></summary>
		DeadBelowdiaeresis = 0xfe6c,
		/// <summary></summary>
		DeadInvertedbreve = 0xfe6d,
		/// <summary></summary>
		DeadBelowcomma = 0xfe6e,
		/// <summary></summary>
		DeadCurrency = 0xfe6f,

		/* extra dead elements for German T3 layout */
		/// <summary></summary>
		DeadLowline = 0xfe90,
		/// <summary></summary>
		DeadAboveverticalline = 0xfe91,
		/// <summary></summary>
		DeadBelowverticalline = 0xfe92,
		/// <summary></summary>
		DeadLongsolidusoverlay = 0xfe93,

		/* dead vowels for universal syllable entry */
		/// <summary>SMALL DEAD A</summary>
		deadA = 0xfe80,
		/// <summary>CAPITAL DEAD A</summary>
		DeadA = 0xfe81,
		/// <summary>SMALL DEAD E</summary>
		deadE = 0xfe82,
		/// <summary>CAPITAL DEAD E</summary>
		DeadE = 0xfe83,
		/// <summary>SMALL DEAD I</summary>
		deadI = 0xfe84,
		/// <summary>CAPITAL DEAD I</summary>
		DeadI = 0xfe85,
		/// <summary>SMALL DEAD O</summary>
		deadO = 0xfe86,
		/// <summary>CAPITAL DEAD O</summary>
		DeadO = 0xfe87,
		/// <summary>SMALL DEAD U</summary>
		deadU = 0xfe88,
		/// <summary>CAPITAL DEAD U</summary>
		DeadU = 0xfe89,
		/// <summary></summary>
		DeadSmallSchwa = 0xfe8a,
		/// <summary></summary>
		DeadCapitalSchwa = 0xfe8b,

		/// <summary></summary>
		DeadGreek = 0xfe8c,

		/// <summary></summary>
		FirstVirtualScreen = 0xfed0,
		/// <summary></summary>
		PrevVirtualScreen = 0xfed1,
		/// <summary></summary>
		NextVirtualScreen = 0xfed2,
		/// <summary></summary>
		LastVirtualScreen = 0xfed4,
		/// <summary></summary>
		TerminateServer = 0xfed5,

		/// <summary></summary>
		AccessXEnable = 0xfe70,
		/// <summary></summary>
		AccessXFeedbackEnable = 0xfe71,
		/// <summary></summary>
		RepeatKeysEnable = 0xfe72,
		/// <summary></summary>
		SlowKeysEnable = 0xfe73,
		/// <summary></summary>
		BounceKeysEnable = 0xfe74,
		/// <summary></summary>
		StickyKeysEnable = 0xfe75,
		/// <summary></summary>
		MouseKeysEnable = 0xfe76,
		/// <summary></summary>
		MouseKeysAccelEnable = 0xfe77,
		/// <summary></summary>
		Overlay1Enable = 0xfe78,
		/// <summary></summary>
		Overlay2Enable = 0xfe79,
		/// <summary></summary>
		AudibleBellEnable = 0xfe7a,

		/// <summary></summary>
		PointerLeft = 0xfee0,
		/// <summary></summary>
		PointerRight = 0xfee1,
		/// <summary></summary>
		PointerUp = 0xfee2,
		/// <summary></summary>
		PointerDown = 0xfee3,
		/// <summary></summary>
		PointerUpLeft = 0xfee4,
		/// <summary></summary>
		PointerUpRight = 0xfee5,
		/// <summary></summary>
		PointerDownLeft = 0xfee6,
		/// <summary></summary>
		PointerDownRight = 0xfee7,
		/// <summary></summary>
		PointerButtonDflt = 0xfee8,
		/// <summary></summary>
		PointerButton1 = 0xfee9,
		/// <summary></summary>
		PointerButton2 = 0xfeea,
		/// <summary></summary>
		PointerButton3 = 0xfeeb,
		/// <summary></summary>
		PointerButton4 = 0xfeec,
		/// <summary></summary>
		PointerButton5 = 0xfeed,
		/// <summary></summary>
		PointerDblClickDflt = 0xfeee,
		/// <summary></summary>
		PointerDblClick1 = 0xfeef,
		/// <summary></summary>
		PointerDblClick2 = 0xfef0,
		/// <summary></summary>
		PointerDblClick3 = 0xfef1,
		/// <summary></summary>
		PointerDblClick4 = 0xfef2,
		/// <summary></summary>
		PointerDblClick5 = 0xfef3,
		/// <summary></summary>
		PointerDragDflt = 0xfef4,
		/// <summary></summary>
		PointerDrag1 = 0xfef5,
		/// <summary></summary>
		PointerDrag2 = 0xfef6,
		/// <summary></summary>
		PointerDrag3 = 0xfef7,
		/// <summary></summary>
		PointerDrag4 = 0xfef8,
		/// <summary></summary>
		PointerDrag5 = 0xfefd,

		/// <summary></summary>
		PointerEnableKeys = 0xfef9,
		/// <summary></summary>
		PointerAccelerate = 0xfefa,
		/// <summary></summary>
		PointerDfltBtnNext = 0xfefb,
		/// <summary></summary>
		PointerDfltBtnPrev = 0xfefc,

		/* Single-Stroke Multiple-Character N-Graph Keysyms For The X Input Method */

		/// <summary></summary>
		ch = 0xfea0,
		/// <summary></summary>
		Ch = 0xfea1,
		/// <summary></summary>
		CH = 0xfea2,
		/// <summary></summary>
		c_h = 0xfea3,
		/// <summary></summary>
		C_h = 0xfea4,
		/// <summary></summary>
		C_H = 0xfea5,

		// #endif /* XK_XKB_KEYS */

		/*
		* 3270 Terminal Keys
		* Byte 3 = 0xfd
		*/

		// #ifdef XK_3270
		/// <summary></summary>
		XK3270Duplicate = 0xfd01,
		/// <summary></summary>
		XK3270FieldMark = 0xfd02,
		/// <summary></summary>
		XK3270Right2 = 0xfd03,
		/// <summary></summary>
		XK3270Left2 = 0xfd04,
		/// <summary></summary>
		XK3270BackTab = 0xfd05,
		/// <summary></summary>
		XK3270EraseEOF = 0xfd06,
		/// <summary></summary>
		XK3270EraseInput = 0xfd07,
		/// <summary></summary>
		XK3270Reset = 0xfd08,
		/// <summary></summary>
		XK3270Quit = 0xfd09,
		/// <summary></summary>
		XK3270PA1 = 0xfd0a,
		/// <summary></summary>
		XK3270PA2 = 0xfd0b,
		/// <summary></summary>
		XK3270PA3 = 0xfd0c,
		/// <summary></summary>
		XK3270Test = 0xfd0d,
		/// <summary></summary>
		XK3270Attn = 0xfd0e,
		/// <summary></summary>
		XK3270CursorBlink = 0xfd0f,
		/// <summary></summary>
		XK3270AltCursor = 0xfd10,
		/// <summary></summary>
		XK3270KeyClick = 0xfd11,
		/// <summary></summary>
		XK3270Jump = 0xfd12,
		/// <summary></summary>
		XK3270Ident = 0xfd13,
		/// <summary></summary>
		XK3270Rule = 0xfd14,
		/// <summary></summary>
		XK3270Copy = 0xfd15,
		/// <summary></summary>
		XK3270Play = 0xfd16,
		/// <summary></summary>
		XK3270Setup = 0xfd17,
		/// <summary></summary>
		XK3270Record = 0xfd18,
		/// <summary></summary>
		XK3270ChangeScreen = 0xfd19,
		/// <summary></summary>
		XK3270DeleteWord = 0xfd1a,
		/// <summary></summary>
		XK3270ExSelect = 0xfd1b,
		/// <summary></summary>
		XK3270CursorSelect = 0xfd1c,
		/// <summary></summary>
		XK3270PrintScreen = 0xfd1d,
		/// <summary></summary>
		XK3270Enter = 0xfd1e,
		// #endif /* XK_3270 */

		/*
		* Latin 1
		* (ISO/IEC 8859-1 = Unicode U+0020..U+00FF)
		* Byte 3 = 0
		*/
		// #ifdef XK_LATIN1
		/// <summary>U+0020 SPACE</summary>
		Space = 0x0020,
		/// <summary>U+0021 EXCLAMATION MARK</summary>
		Exclam = 0x0021,
		/// <summary>U+0022 QUOTATION MARK</summary>
		Quotedbl = 0x0022,
		/// <summary>U+0023 NUMBER SIGN</summary>
		Numbersign = 0x0023,
		/// <summary>U+0024 DOLLAR SIGN</summary>
		Dollar = 0x0024,
		/// <summary>U+0025 PERCENT SIGN</summary>
		Percent = 0x0025,
		/// <summary>U+0026 AMPERSAND</summary>
		Ampersand = 0x0026,
		/// <summary>U+0027 APOSTROPHE</summary>
		Apostrophe = 0x0027,
		/// <summary>deprecated</summary>
		Quoteright = 0x0027,
		/// <summary>U+0028 LEFT PARENTHESIS</summary>
		Parenleft = 0x0028,
		/// <summary>U+0029 RIGHT PARENTHESIS</summary>
		Parenright = 0x0029,
		/// <summary>U+002A ASTERISK</summary>
		Asterisk = 0x002a,
		/// <summary>U+002B PLUS SIGN</summary>
		Plus = 0x002b,
		/// <summary>U+002C COMMA</summary>
		Comma = 0x002c,
		/// <summary>U+002D HYPHEN-MINUS</summary>
		Minus = 0x002d,
		/// <summary>U+002E FULL STOP</summary>
		Period = 0x002e,
		/// <summary>U+002F SOLIDUS</summary>
		Slash = 0x002f,
		/// <summary>U+0030 DIGIT ZERO</summary>
		D0 = 0x0030,
		/// <summary>U+0031 DIGIT ONE</summary>
		D1 = 0x0031,
		/// <summary>U+0032 DIGIT TWO</summary>
		D2 = 0x0032,
		/// <summary>U+0033 DIGIT THREE</summary>
		D3 = 0x0033,
		/// <summary>U+0034 DIGIT FOUR</summary>
		D4 = 0x0034,
		/// <summary>U+0035 DIGIT FIVE</summary>
		D5 = 0x0035,
		/// <summary>U+0036 DIGIT SIX</summary>
		D6 = 0x0036,
		/// <summary>U+0037 DIGIT SEVEN</summary>
		D7 = 0x0037,
		/// <summary>U+0038 DIGIT EIGHT</summary>
		D8 = 0x0038,
		/// <summary>U+0039 DIGIT NINE</summary>
		D9 = 0x0039,
		/// <summary>U+003A COLON</summary>
		Colon = 0x003a,
		/// <summary>U+003B SEMICOLON</summary>
		Semicolon = 0x003b,
		/// <summary>U+003C LESS-THAN SIGN</summary>
		Less = 0x003c,
		/// <summary>U+003D EQUALS SIGN</summary>
		Equal = 0x003d,
		/// <summary>U+003E GREATER-THAN SIGN</summary>
		Greater = 0x003e,
		/// <summary>U+003F QUESTION MARK</summary>
		Question = 0x003f,
		/// <summary>U+0040 COMMERCIAL AT</summary>
		At = 0x0040,
		/// <summary>U+0041 LATIN CAPITAL LETTER A</summary>
		A = 0x0041,
		/// <summary>U+0042 LATIN CAPITAL LETTER B</summary>
		B = 0x0042,
		/// <summary>U+0043 LATIN CAPITAL LETTER C</summary>
		C = 0x0043,
		/// <summary>U+0044 LATIN CAPITAL LETTER D</summary>
		D = 0x0044,
		/// <summary>U+0045 LATIN CAPITAL LETTER E</summary>
		E = 0x0045,
		/// <summary>U+0046 LATIN CAPITAL LETTER F</summary>
		F = 0x0046,
		/// <summary>U+0047 LATIN CAPITAL LETTER G</summary>
		G = 0x0047,
		/// <summary>U+0048 LATIN CAPITAL LETTER H</summary>
		H = 0x0048,
		/// <summary>U+0049 LATIN CAPITAL LETTER I</summary>
		I = 0x0049,
		/// <summary>U+004A LATIN CAPITAL LETTER J</summary>
		J = 0x004a,
		/// <summary>U+004B LATIN CAPITAL LETTER K</summary>
		K = 0x004b,
		/// <summary>U+004C LATIN CAPITAL LETTER L</summary>
		L = 0x004c,
		/// <summary>U+004D LATIN CAPITAL LETTER M</summary>
		M = 0x004d,
		/// <summary>U+004E LATIN CAPITAL LETTER N</summary>
		N = 0x004e,
		/// <summary>U+004F LATIN CAPITAL LETTER O</summary>
		O = 0x004f,
		/// <summary>U+0050 LATIN CAPITAL LETTER P</summary>
		P = 0x0050,
		/// <summary>U+0051 LATIN CAPITAL LETTER Q</summary>
		Q = 0x0051,
		/// <summary>U+0052 LATIN CAPITAL LETTER R</summary>
		R = 0x0052,
		/// <summary>U+0053 LATIN CAPITAL LETTER S</summary>
		S = 0x0053,
		/// <summary>U+0054 LATIN CAPITAL LETTER T</summary>
		T = 0x0054,
		/// <summary>U+0055 LATIN CAPITAL LETTER U</summary>
		U = 0x0055,
		/// <summary>U+0056 LATIN CAPITAL LETTER V</summary>
		V = 0x0056,
		/// <summary>U+0057 LATIN CAPITAL LETTER W</summary>
		W = 0x0057,
		/// <summary>U+0058 LATIN CAPITAL LETTER X</summary>
		X = 0x0058,
		/// <summary>U+0059 LATIN CAPITAL LETTER Y</summary>
		Y = 0x0059,
		/// <summary>U+005A LATIN CAPITAL LETTER Z</summary>
		Z = 0x005a,
		/// <summary>U+005B LEFT SQUARE BRACKET</summary>
		Bracketleft = 0x005b,
		/// <summary>U+005C REVERSE SOLIDUS</summary>
		Backslash = 0x005c,
		/// <summary>U+005D RIGHT SQUARE BRACKET</summary>
		Bracketright = 0x005d,
		/// <summary>U+005E CIRCUMFLEX ACCENT</summary>
		Asciicircum = 0x005e,
		/// <summary>U+005F LOW LINE</summary>
		Underscore = 0x005f,
		/// <summary>U+0060 GRAVE ACCENT</summary>
		Grave = 0x0060,
		/// <summary>deprecated</summary>
		Quoteleft = 0x0060,
		/// <summary>U+0061 LATIN SMALL LETTER A</summary>
		a = 0x0061,
		/// <summary>U+0062 LATIN SMALL LETTER B</summary>
		b = 0x0062,
		/// <summary>U+0063 LATIN SMALL LETTER C</summary>
		c = 0x0063,
		/// <summary>U+0064 LATIN SMALL LETTER D</summary>
		d = 0x0064,
		/// <summary>U+0065 LATIN SMALL LETTER E</summary>
		e = 0x0065,
		/// <summary>U+0066 LATIN SMALL LETTER F</summary>
		f = 0x0066,
		/// <summary>U+0067 LATIN SMALL LETTER G</summary>
		g = 0x0067,
		/// <summary>U+0068 LATIN SMALL LETTER H</summary>
		h = 0x0068,
		/// <summary>U+0069 LATIN SMALL LETTER I</summary>
		i = 0x0069,
		/// <summary>U+006A LATIN SMALL LETTER J</summary>
		j = 0x006a,
		/// <summary>U+006B LATIN SMALL LETTER K</summary>
		k = 0x006b,
		/// <summary>U+006C LATIN SMALL LETTER L</summary>
		l = 0x006c,
		/// <summary>U+006D LATIN SMALL LETTER M</summary>
		m = 0x006d,
		/// <summary>U+006E LATIN SMALL LETTER N</summary>
		n = 0x006e,
		/// <summary>U+006F LATIN SMALL LETTER O</summary>
		o = 0x006f,
		/// <summary>U+0070 LATIN SMALL LETTER P</summary>
		p = 0x0070,
		/// <summary>U+0071 LATIN SMALL LETTER Q</summary>
		q = 0x0071,
		/// <summary>U+0072 LATIN SMALL LETTER R</summary>
		r = 0x0072,
		/// <summary>U+0073 LATIN SMALL LETTER S</summary>
		s = 0x0073,
		/// <summary>U+0074 LATIN SMALL LETTER T</summary>
		t = 0x0074,
		/// <summary>U+0075 LATIN SMALL LETTER U</summary>
		u = 0x0075,
		/// <summary>U+0076 LATIN SMALL LETTER V</summary>
		v = 0x0076,
		/// <summary>U+0077 LATIN SMALL LETTER W</summary>
		w = 0x0077,
		/// <summary>U+0078 LATIN SMALL LETTER X</summary>
		x = 0x0078,
		/// <summary>U+0079 LATIN SMALL LETTER Y</summary>
		y = 0x0079,
		/// <summary>U+007A LATIN SMALL LETTER Z</summary>
		z = 0x007a,
		/// <summary>U+007B LEFT CURLY BRACKET</summary>
		Braceleft = 0x007b,
		/// <summary>U+007C VERTICAL LINE</summary>
		Bar = 0x007c,
		/// <summary>U+007D RIGHT CURLY BRACKET</summary>
		Braceright = 0x007d,
		/// <summary>U+007E TILDE</summary>
		Asciitilde = 0x007e,

		/// <summary>U+00A0 NO-BREAK SPACE</summary>
		Nobreakspace = 0x00a0,
		/// <summary>U+00A1 INVERTED EXCLAMATION MARK</summary>
		Exclamdown = 0x00a1,
		/// <summary>U+00A2 CENT SIGN</summary>
		Cent = 0x00a2,
		/// <summary>U+00A3 POUND SIGN</summary>
		Sterling = 0x00a3,
		/// <summary>U+00A4 CURRENCY SIGN</summary>
		Currency = 0x00a4,
		/// <summary>U+00A5 YEN SIGN</summary>
		Yen = 0x00a5,
		/// <summary>U+00A6 BROKEN BAR</summary>
		Brokenbar = 0x00a6,
		/// <summary>U+00A7 SECTION SIGN</summary>
		Section = 0x00a7,
		/// <summary>U+00A8 DIAERESIS</summary>
		Diaeresis = 0x00a8,
		/// <summary>U+00A9 COPYRIGHT SIGN</summary>
		Copyright = 0x00a9,
		/// <summary>U+00AA FEMININE ORDINAL INDICATOR</summary>
		Ordfeminine = 0x00aa,
		/// <summary>U+00AB LEFT-POINTING DOUBLE ANGLE QUOTATION MARK</summary>
		Guillemotleft = 0x00ab,
		/// <summary>U+00AC NOT SIGN</summary>
		Notsign = 0x00ac,
		/// <summary>U+00AD SOFT HYPHEN</summary>
		Hyphen = 0x00ad,
		/// <summary>U+00AE REGISTERED SIGN</summary>
		Registered = 0x00ae,
		/// <summary>U+00AF MACRON</summary>
		Macron = 0x00af,
		/// <summary>U+00B0 DEGREE SIGN</summary>
		Degree = 0x00b0,
		/// <summary>U+00B1 PLUS-MINUS SIGN</summary>
		Plusminus = 0x00b1,
		/// <summary>U+00B2 SUPERSCRIPT TWO</summary>
		Twosuperior = 0x00b2,
		/// <summary>U+00B3 SUPERSCRIPT THREE</summary>
		Threesuperior = 0x00b3,
		/// <summary>U+00B4 ACUTE ACCENT</summary>
		Acute = 0x00b4,
		/// <summary>U+00B5 MICRO SIGN</summary>
		Mu = 0x00b5,
		/// <summary>U+00B6 PILCROW SIGN</summary>
		Paragraph = 0x00b6,
		/// <summary>U+00B7 MIDDLE DOT</summary>
		Periodcentered = 0x00b7,
		/// <summary>U+00B8 CEDILLA</summary>
		Cedilla = 0x00b8,
		/// <summary>U+00B9 SUPERSCRIPT ONE</summary>
		Onesuperior = 0x00b9,
		/// <summary>U+00BA MASCULINE ORDINAL INDICATOR</summary>
		Masculine = 0x00ba,
		/// <summary>U+00BB RIGHT-POINTING DOUBLE ANGLE QUOTATION MARK</summary>
		Guillemotright = 0x00bb,
		/// <summary>U+00BC VULGAR FRACTION ONE QUARTER</summary>
		Onequarter = 0x00bc,
		/// <summary>U+00BD VULGAR FRACTION ONE HALF</summary>
		Onehalf = 0x00bd,
		/// <summary>U+00BE VULGAR FRACTION THREE QUARTERS</summary>
		Threequarters = 0x00be,
		/// <summary>U+00BF INVERTED QUESTION MARK</summary>
		Questiondown = 0x00bf,
		/// <summary>U+00C0 LATIN CAPITAL LETTER A WITH GRAVE</summary>
		Agrave = 0x00c0,
		/// <summary>U+00C1 LATIN CAPITAL LETTER A WITH ACUTE</summary>
		Aacute = 0x00c1,
		/// <summary>U+00C2 LATIN CAPITAL LETTER A WITH CIRCUMFLEX</summary>
		Acircumflex = 0x00c2,
		/// <summary>U+00C3 LATIN CAPITAL LETTER A WITH TILDE</summary>
		Atilde = 0x00c3,
		/// <summary>U+00C4 LATIN CAPITAL LETTER A WITH DIAERESIS</summary>
		Adiaeresis = 0x00c4,
		/// <summary>U+00C5 LATIN CAPITAL LETTER A WITH RING ABOVE</summary>
		Aring = 0x00c5,
		/// <summary>U+00C6 LATIN CAPITAL LETTER AE</summary>
		AE = 0x00c6,
		/// <summary>U+00C7 LATIN CAPITAL LETTER C WITH CEDILLA</summary>
		Ccedilla = 0x00c7,
		/// <summary>U+00C8 LATIN CAPITAL LETTER E WITH GRAVE</summary>
		Egrave = 0x00c8,
		/// <summary>U+00C9 LATIN CAPITAL LETTER E WITH ACUTE</summary>
		Eacute = 0x00c9,
		/// <summary>U+00CA LATIN CAPITAL LETTER E WITH CIRCUMFLEX</summary>
		Ecircumflex = 0x00ca,
		/// <summary>U+00CB LATIN CAPITAL LETTER E WITH DIAERESIS</summary>
		Ediaeresis = 0x00cb,
		/// <summary>U+00CC LATIN CAPITAL LETTER I WITH GRAVE</summary>
		Igrave = 0x00cc,
		/// <summary>U+00CD LATIN CAPITAL LETTER I WITH ACUTE</summary>
		Iacute = 0x00cd,
		/// <summary>U+00CE LATIN CAPITAL LETTER I WITH CIRCUMFLEX</summary>
		Icircumflex = 0x00ce,
		/// <summary>U+00CF LATIN CAPITAL LETTER I WITH DIAERESIS</summary>
		Idiaeresis = 0x00cf,
		/// <summary>U+00D0 LATIN CAPITAL LETTER ETH</summary>
		ETH = 0x00d0,
		/// <summary>deprecated</summary>
		Eth = 0x00d0,
		/// <summary>U+00D1 LATIN CAPITAL LETTER N WITH TILDE</summary>
		Ntilde = 0x00d1,
		/// <summary>U+00D2 LATIN CAPITAL LETTER O WITH GRAVE</summary>
		Ograve = 0x00d2,
		/// <summary>U+00D3 LATIN CAPITAL LETTER O WITH ACUTE</summary>
		Oacute = 0x00d3,
		/// <summary>U+00D4 LATIN CAPITAL LETTER O WITH CIRCUMFLEX</summary>
		Ocircumflex = 0x00d4,
		/// <summary>U+00D5 LATIN CAPITAL LETTER O WITH TILDE</summary>
		Otilde = 0x00d5,
		/// <summary>U+00D6 LATIN CAPITAL LETTER O WITH DIAERESIS</summary>
		Odiaeresis = 0x00d6,
		/// <summary>U+00D7 MULTIPLICATION SIGN</summary>
		Multiply = 0x00d7,
		/// <summary>U+00D8 LATIN CAPITAL LETTER O WITH STROKE</summary>
		Oslash = 0x00d8,
		/// <summary>U+00D8 LATIN CAPITAL LETTER O WITH STROKE</summary>
		Ooblique = 0x00d8,
		/// <summary>U+00D9 LATIN CAPITAL LETTER U WITH GRAVE</summary>
		Ugrave = 0x00d9,
		/// <summary>U+00DA LATIN CAPITAL LETTER U WITH ACUTE</summary>
		Uacute = 0x00da,
		/// <summary>U+00DB LATIN CAPITAL LETTER U WITH CIRCUMFLEX</summary>
		Ucircumflex = 0x00db,
		/// <summary>U+00DC LATIN CAPITAL LETTER U WITH DIAERESIS</summary>
		Udiaeresis = 0x00dc,
		/// <summary>U+00DD LATIN CAPITAL LETTER Y WITH ACUTE</summary>
		Yacute = 0x00dd,
		/// <summary>U+00DE LATIN CAPITAL LETTER THORN</summary>
		THORN = 0x00de,
		/// <summary>deprecated</summary>
		Thorn = 0x00de,
		/// <summary>U+00DF LATIN SMALL LETTER SHARP S</summary>
		ssharp = 0x00df,
		/// <summary>U+00E0 LATIN SMALL LETTER A WITH GRAVE</summary>
		agrave = 0x00e0,
		/// <summary>U+00E1 LATIN SMALL LETTER A WITH ACUTE</summary>
		aacute = 0x00e1,
		/// <summary>U+00E2 LATIN SMALL LETTER A WITH CIRCUMFLEX</summary>
		acircumflex = 0x00e2,
		/// <summary>U+00E3 LATIN SMALL LETTER A WITH TILDE</summary>
		atilde = 0x00e3,
		/// <summary>U+00E4 LATIN SMALL LETTER A WITH DIAERESIS</summary>
		adiaeresis = 0x00e4,
		/// <summary>U+00E5 LATIN SMALL LETTER A WITH RING ABOVE</summary>
		aring = 0x00e5,
		/// <summary>U+00E6 LATIN SMALL LETTER AE</summary>
		ae = 0x00e6,
		/// <summary>U+00E7 LATIN SMALL LETTER C WITH CEDILLA</summary>
		ccedilla = 0x00e7,
		/// <summary>U+00E8 LATIN SMALL LETTER E WITH GRAVE</summary>
		egrave = 0x00e8,
		/// <summary>U+00E9 LATIN SMALL LETTER E WITH ACUTE</summary>
		eacute = 0x00e9,
		/// <summary>U+00EA LATIN SMALL LETTER E WITH CIRCUMFLEX</summary>
		ecircumflex = 0x00ea,
		/// <summary>U+00EB LATIN SMALL LETTER E WITH DIAERESIS</summary>
		ediaeresis = 0x00eb,
		/// <summary>U+00EC LATIN SMALL LETTER I WITH GRAVE</summary>
		igrave = 0x00ec,
		/// <summary>U+00ED LATIN SMALL LETTER I WITH ACUTE</summary>
		iacute = 0x00ed,
		/// <summary>U+00EE LATIN SMALL LETTER I WITH CIRCUMFLEX</summary>
		icircumflex = 0x00ee,
		/// <summary>U+00EF LATIN SMALL LETTER I WITH DIAERESIS</summary>
		idiaeresis = 0x00ef,
		/// <summary>U+00F0 LATIN SMALL LETTER ETH</summary>
		eth = 0x00f0,
		/// <summary>U+00F1 LATIN SMALL LETTER N WITH TILDE</summary>
		ntilde = 0x00f1,
		/// <summary>U+00F2 LATIN SMALL LETTER O WITH GRAVE</summary>
		ograve = 0x00f2,
		/// <summary>U+00F3 LATIN SMALL LETTER O WITH ACUTE</summary>
		oacute = 0x00f3,
		/// <summary>U+00F4 LATIN SMALL LETTER O WITH CIRCUMFLEX</summary>
		ocircumflex = 0x00f4,
		/// <summary>U+00F5 LATIN SMALL LETTER O WITH TILDE</summary>
		otilde = 0x00f5,
		/// <summary>U+00F6 LATIN SMALL LETTER O WITH DIAERESIS</summary>
		odiaeresis = 0x00f6,
		/// <summary>U+00F7 DIVISION SIGN</summary>
		Division = 0x00f7,
		/// <summary>U+00F8 LATIN SMALL LETTER O WITH STROKE</summary>
		oslash = 0x00f8,
		/// <summary>U+00F8 LATIN SMALL LETTER O WITH STROKE</summary>
		ooblique = 0x00f8,
		/// <summary>U+00F9 LATIN SMALL LETTER U WITH GRAVE</summary>
		ugrave = 0x00f9,
		/// <summary>U+00FA LATIN SMALL LETTER U WITH ACUTE</summary>
		uacute = 0x00fa,
		/// <summary>U+00FB LATIN SMALL LETTER U WITH CIRCUMFLEX</summary>
		ucircumflex = 0x00fb,
		/// <summary>U+00FC LATIN SMALL LETTER U WITH DIAERESIS</summary>
		udiaeresis = 0x00fc,
		/// <summary>U+00FD LATIN SMALL LETTER Y WITH ACUTE</summary>
		yacute = 0x00fd,
		/// <summary>U+00FE LATIN SMALL LETTER THORN</summary>
		thorn = 0x00fe,
		/// <summary>U+00FF LATIN SMALL LETTER Y WITH DIAERESIS</summary>
		ydiaeresis = 0x00ff,
		// #endif /* XK_LATIN1 */

		/*
		* Latin 2
		* Byte 3 = 1
		*/

		// #ifdef XK_LATIN2
		/// <summary>U+0104 LATIN CAPITAL LETTER A WITH OGONEK</summary>
		Aogonek = 0x01a1,
		/// <summary>U+02D8 BREVE</summary>
		Breve = 0x01a2,
		/// <summary>U+0141 LATIN CAPITAL LETTER L WITH STROKE</summary>
		Lstroke = 0x01a3,
		/// <summary>U+013D LATIN CAPITAL LETTER L WITH CARON</summary>
		Lcaron = 0x01a5,
		/// <summary>U+015A LATIN CAPITAL LETTER S WITH ACUTE</summary>
		Sacute = 0x01a6,
		/// <summary>U+0160 LATIN CAPITAL LETTER S WITH CARON</summary>
		Scaron = 0x01a9,
		/// <summary>U+015E LATIN CAPITAL LETTER S WITH CEDILLA</summary>
		Scedilla = 0x01aa,
		/// <summary>U+0164 LATIN CAPITAL LETTER T WITH CARON</summary>
		Tcaron = 0x01ab,
		/// <summary>U+0179 LATIN CAPITAL LETTER Z WITH ACUTE</summary>
		Zacute = 0x01ac,
		/// <summary>U+017D LATIN CAPITAL LETTER Z WITH CARON</summary>
		Zcaron = 0x01ae,
		/// <summary>U+017B LATIN CAPITAL LETTER Z WITH DOT ABOVE</summary>
		Zabovedot = 0x01af,
		/// <summary>U+0105 LATIN SMALL LETTER A WITH OGONEK</summary>
		aogonek = 0x01b1,
		/// <summary>U+02DB OGONEK</summary>
		Ogonek = 0x01b2,
		/// <summary>U+0142 LATIN SMALL LETTER L WITH STROKE</summary>
		lstroke = 0x01b3,
		/// <summary>U+013E LATIN SMALL LETTER L WITH CARON</summary>
		lcaron = 0x01b5,
		/// <summary>U+015B LATIN SMALL LETTER S WITH ACUTE</summary>
		sacute = 0x01b6,
		/// <summary>U+02C7 CARON</summary>
		Caron = 0x01b7,
		/// <summary>U+0161 LATIN SMALL LETTER S WITH CARON</summary>
		scaron = 0x01b9,
		/// <summary>U+015F LATIN SMALL LETTER S WITH CEDILLA</summary>
		scedilla = 0x01ba,
		/// <summary>U+0165 LATIN SMALL LETTER T WITH CARON</summary>
		tcaron = 0x01bb,
		/// <summary>U+017A LATIN SMALL LETTER Z WITH ACUTE</summary>
		zacute = 0x01bc,
		/// <summary>U+02DD DOUBLE ACUTE ACCENT</summary>
		Doubleacute = 0x01bd,
		/// <summary>U+017E LATIN SMALL LETTER Z WITH CARON</summary>
		zcaron = 0x01be,
		/// <summary>U+017C LATIN SMALL LETTER Z WITH DOT ABOVE</summary>
		zabovedot = 0x01bf,
		/// <summary>U+0154 LATIN CAPITAL LETTER R WITH ACUTE</summary>
		Racute = 0x01c0,
		/// <summary>U+0102 LATIN CAPITAL LETTER A WITH BREVE</summary>
		Abreve = 0x01c3,
		/// <summary>U+0139 LATIN CAPITAL LETTER L WITH ACUTE</summary>
		Lacute = 0x01c5,
		/// <summary>U+0106 LATIN CAPITAL LETTER C WITH ACUTE</summary>
		Cacute = 0x01c6,
		/// <summary>U+010C LATIN CAPITAL LETTER C WITH CARON</summary>
		Ccaron = 0x01c8,
		/// <summary>U+0118 LATIN CAPITAL LETTER E WITH OGONEK</summary>
		Eogonek = 0x01ca,
		/// <summary>U+011A LATIN CAPITAL LETTER E WITH CARON</summary>
		Ecaron = 0x01cc,
		/// <summary>U+010E LATIN CAPITAL LETTER D WITH CARON</summary>
		Dcaron = 0x01cf,
		/// <summary>U+0110 LATIN CAPITAL LETTER D WITH STROKE</summary>
		Dstroke = 0x01d0,
		/// <summary>U+0143 LATIN CAPITAL LETTER N WITH ACUTE</summary>
		Nacute = 0x01d1,
		/// <summary>U+0147 LATIN CAPITAL LETTER N WITH CARON</summary>
		Ncaron = 0x01d2,
		/// <summary>U+0150 LATIN CAPITAL LETTER O WITH DOUBLE ACUTE</summary>
		Odoubleacute = 0x01d5,
		/// <summary>U+0158 LATIN CAPITAL LETTER R WITH CARON</summary>
		Rcaron = 0x01d8,
		/// <summary>U+016E LATIN CAPITAL LETTER U WITH RING ABOVE</summary>
		Uring = 0x01d9,
		/// <summary>U+0170 LATIN CAPITAL LETTER U WITH DOUBLE ACUTE</summary>
		Udoubleacute = 0x01db,
		/// <summary>U+0162 LATIN CAPITAL LETTER T WITH CEDILLA</summary>
		Tcedilla = 0x01de,
		/// <summary>U+0155 LATIN SMALL LETTER R WITH ACUTE</summary>
		racute = 0x01e0,
		/// <summary>U+0103 LATIN SMALL LETTER A WITH BREVE</summary>
		abreve = 0x01e3,
		/// <summary>U+013A LATIN SMALL LETTER L WITH ACUTE</summary>
		lacute = 0x01e5,
		/// <summary>U+0107 LATIN SMALL LETTER C WITH ACUTE</summary>
		cacute = 0x01e6,
		/// <summary>U+010D LATIN SMALL LETTER C WITH CARON</summary>
		ccaron = 0x01e8,
		/// <summary>U+0119 LATIN SMALL LETTER E WITH OGONEK</summary>
		eogonek = 0x01ea,
		/// <summary>U+011B LATIN SMALL LETTER E WITH CARON</summary>
		ecaron = 0x01ec,
		/// <summary>U+010F LATIN SMALL LETTER D WITH CARON</summary>
		dcaron = 0x01ef,
		/// <summary>U+0111 LATIN SMALL LETTER D WITH STROKE</summary>
		dstroke = 0x01f0,
		/// <summary>U+0144 LATIN SMALL LETTER N WITH ACUTE</summary>
		nacute = 0x01f1,
		/// <summary>U+0148 LATIN SMALL LETTER N WITH CARON</summary>
		ncaron = 0x01f2,
		/// <summary>U+0151 LATIN SMALL LETTER O WITH DOUBLE ACUTE</summary>
		odoubleacute = 0x01f5,
		/// <summary>U+0159 LATIN SMALL LETTER R WITH CARON</summary>
		rcaron = 0x01f8,
		/// <summary>U+016F LATIN SMALL LETTER U WITH RING ABOVE</summary>
		uring = 0x01f9,
		/// <summary>U+0171 LATIN SMALL LETTER U WITH DOUBLE ACUTE</summary>
		udoubleacute = 0x01fb,
		/// <summary>U+0163 LATIN SMALL LETTER T WITH CEDILLA</summary>
		tcedilla = 0x01fe,
		/// <summary>U+02D9 DOT ABOVE</summary>
		Abovedot = 0x01ff,
		// #endif /* XK_LATIN2 */

		/*
		* Latin 3
		* Byte 3 = 2
		*/

		// #ifdef XK_LATIN3
		/// <summary>U+0126 LATIN CAPITAL LETTER H WITH STROKE</summary>
		Hstroke = 0x02a1,
		/// <summary>U+0124 LATIN CAPITAL LETTER H WITH CIRCUMFLEX</summary>
		Hcircumflex = 0x02a6,
		/// <summary>U+0130 LATIN CAPITAL LETTER I WITH DOT ABOVE</summary>
		Iabovedot = 0x02a9,
		/// <summary>U+011E LATIN CAPITAL LETTER G WITH BREVE</summary>
		Gbreve = 0x02ab,
		/// <summary>U+0134 LATIN CAPITAL LETTER J WITH CIRCUMFLEX</summary>
		Jcircumflex = 0x02ac,
		/// <summary>U+0127 LATIN SMALL LETTER H WITH STROKE</summary>
		hstroke = 0x02b1,
		/// <summary>U+0125 LATIN SMALL LETTER H WITH CIRCUMFLEX</summary>
		hcircumflex = 0x02b6,
		/// <summary>U+0131 LATIN SMALL LETTER DOTLESS I</summary>
		idotless = 0x02b9,
		/// <summary>U+011F LATIN SMALL LETTER G WITH BREVE</summary>
		gbreve = 0x02bb,
		/// <summary>U+0135 LATIN SMALL LETTER J WITH CIRCUMFLEX</summary>
		jcircumflex = 0x02bc,
		/// <summary>U+010A LATIN CAPITAL LETTER C WITH DOT ABOVE</summary>
		Cabovedot = 0x02c5,
		/// <summary>U+0108 LATIN CAPITAL LETTER C WITH CIRCUMFLEX</summary>
		Ccircumflex = 0x02c6,
		/// <summary>U+0120 LATIN CAPITAL LETTER G WITH DOT ABOVE</summary>
		Gabovedot = 0x02d5,
		/// <summary>U+011C LATIN CAPITAL LETTER G WITH CIRCUMFLEX</summary>
		Gcircumflex = 0x02d8,
		/// <summary>U+016C LATIN CAPITAL LETTER U WITH BREVE</summary>
		Ubreve = 0x02dd,
		/// <summary>U+015C LATIN CAPITAL LETTER S WITH CIRCUMFLEX</summary>
		Scircumflex = 0x02de,
		/// <summary>U+010B LATIN SMALL LETTER C WITH DOT ABOVE</summary>
		cabovedot = 0x02e5,
		/// <summary>U+0109 LATIN SMALL LETTER C WITH CIRCUMFLEX</summary>
		ccircumflex = 0x02e6,
		/// <summary>U+0121 LATIN SMALL LETTER G WITH DOT ABOVE</summary>
		gabovedot = 0x02f5,
		/// <summary>U+011D LATIN SMALL LETTER G WITH CIRCUMFLEX</summary>
		gcircumflex = 0x02f8,
		/// <summary>U+016D LATIN SMALL LETTER U WITH BREVE</summary>
		ubreve = 0x02fd,
		/// <summary>U+015D LATIN SMALL LETTER S WITH CIRCUMFLEX</summary>
		scircumflex = 0x02fe,
		// #endif /* XK_LATIN3 */


		/*
		* Latin 4
		* Byte 3 = 3
		*/

		// #ifdef XK_LATIN4
		/// <summary>U+0138 LATIN SMALL LETTER KRA</summary>
		kra = 0x03a2,
		/// <summary>deprecated</summary>
		Kappa = 0x03a2,
		/// <summary>U+0156 LATIN CAPITAL LETTER R WITH CEDILLA</summary>
		Rcedilla = 0x03a3,
		/// <summary>U+0128 LATIN CAPITAL LETTER I WITH TILDE</summary>
		Itilde = 0x03a5,
		/// <summary>U+013B LATIN CAPITAL LETTER L WITH CEDILLA</summary>
		Lcedilla = 0x03a6,
		/// <summary>U+0112 LATIN CAPITAL LETTER E WITH MACRON</summary>
		Emacron = 0x03aa,
		/// <summary>U+0122 LATIN CAPITAL LETTER G WITH CEDILLA</summary>
		Gcedilla = 0x03ab,
		/// <summary>U+0166 LATIN CAPITAL LETTER T WITH STROKE</summary>
		Tslash = 0x03ac,
		/// <summary>U+0157 LATIN SMALL LETTER R WITH CEDILLA</summary>
		rcedilla = 0x03b3,
		/// <summary>U+0129 LATIN SMALL LETTER I WITH TILDE</summary>
		itilde = 0x03b5,
		/// <summary>U+013C LATIN SMALL LETTER L WITH CEDILLA</summary>
		lcedilla = 0x03b6,
		/// <summary>U+0113 LATIN SMALL LETTER E WITH MACRON</summary>
		emacron = 0x03ba,
		/// <summary>U+0123 LATIN SMALL LETTER G WITH CEDILLA</summary>
		gcedilla = 0x03bb,
		/// <summary>U+0167 LATIN SMALL LETTER T WITH STROKE</summary>
		tslash = 0x03bc,
		/// <summary>U+014A LATIN CAPITAL LETTER ENG</summary>
		ENG = 0x03bd,
		/// <summary>U+014B LATIN SMALL LETTER ENG</summary>
		eng = 0x03bf,
		/// <summary>U+0100 LATIN CAPITAL LETTER A WITH MACRON</summary>
		Amacron = 0x03c0,
		/// <summary>U+012E LATIN CAPITAL LETTER I WITH OGONEK</summary>
		Iogonek = 0x03c7,
		/// <summary>U+0116 LATIN CAPITAL LETTER E WITH DOT ABOVE</summary>
		Eabovedot = 0x03cc,
		/// <summary>U+012A LATIN CAPITAL LETTER I WITH MACRON</summary>
		Imacron = 0x03cf,
		/// <summary>U+0145 LATIN CAPITAL LETTER N WITH CEDILLA</summary>
		Ncedilla = 0x03d1,
		/// <summary>U+014C LATIN CAPITAL LETTER O WITH MACRON</summary>
		Omacron = 0x03d2,
		/// <summary>U+0136 LATIN CAPITAL LETTER K WITH CEDILLA</summary>
		Kcedilla = 0x03d3,
		/// <summary>U+0172 LATIN CAPITAL LETTER U WITH OGONEK</summary>
		Uogonek = 0x03d9,
		/// <summary>U+0168 LATIN CAPITAL LETTER U WITH TILDE</summary>
		Utilde = 0x03dd,
		/// <summary>U+016A LATIN CAPITAL LETTER U WITH MACRON</summary>
		Umacron = 0x03de,
		/// <summary>U+0101 LATIN SMALL LETTER A WITH MACRON</summary>
		amacron = 0x03e0,
		/// <summary>U+012F LATIN SMALL LETTER I WITH OGONEK</summary>
		iogonek = 0x03e7,
		/// <summary>U+0117 LATIN SMALL LETTER E WITH DOT ABOVE</summary>
		eabovedot = 0x03ec,
		/// <summary>U+012B LATIN SMALL LETTER I WITH MACRON</summary>
		imacron = 0x03ef,
		/// <summary>U+0146 LATIN SMALL LETTER N WITH CEDILLA</summary>
		ncedilla = 0x03f1,
		/// <summary>U+014D LATIN SMALL LETTER O WITH MACRON</summary>
		omacron = 0x03f2,
		/// <summary>U+0137 LATIN SMALL LETTER K WITH CEDILLA</summary>
		kcedilla = 0x03f3,
		/// <summary>U+0173 LATIN SMALL LETTER U WITH OGONEK</summary>
		uogonek = 0x03f9,
		/// <summary>U+0169 LATIN SMALL LETTER U WITH TILDE</summary>
		utilde = 0x03fd,
		/// <summary>U+016B LATIN SMALL LETTER U WITH MACRON</summary>
		umacron = 0x03fe,
		// #endif /* XK_LATIN4 */

		/*
		* Latin 8
		*/
		// #ifdef XK_LATIN8
		/// <summary>U+0174 LATIN CAPITAL LETTER W WITH CIRCUMFLEX</summary>
		Wcircumflex = 0x1000174,
		/// <summary>U+0175 LATIN SMALL LETTER W WITH CIRCUMFLEX</summary>
		wcircumflex = 0x1000175,
		/// <summary>U+0176 LATIN CAPITAL LETTER Y WITH CIRCUMFLEX</summary>
		Ycircumflex = 0x1000176,
		/// <summary>U+0177 LATIN SMALL LETTER Y WITH CIRCUMFLEX</summary>
		ycircumflex = 0x1000177,
		/// <summary>U+1E02 LATIN CAPITAL LETTER B WITH DOT ABOVE</summary>
		Babovedot = 0x1001e02,
		/// <summary>U+1E03 LATIN SMALL LETTER B WITH DOT ABOVE</summary>
		babovedot = 0x1001e03,
		/// <summary>U+1E0A LATIN CAPITAL LETTER D WITH DOT ABOVE</summary>
		Dabovedot = 0x1001e0a,
		/// <summary>U+1E0B LATIN SMALL LETTER D WITH DOT ABOVE</summary>
		dabovedot = 0x1001e0b,
		/// <summary>U+1E1E LATIN CAPITAL LETTER F WITH DOT ABOVE</summary>
		Fabovedot = 0x1001e1e,
		/// <summary>U+1E1F LATIN SMALL LETTER F WITH DOT ABOVE</summary>
		fabovedot = 0x1001e1f,
		/// <summary>U+1E40 LATIN CAPITAL LETTER M WITH DOT ABOVE</summary>
		Mabovedot = 0x1001e40,
		/// <summary>U+1E41 LATIN SMALL LETTER M WITH DOT ABOVE</summary>
		mabovedot = 0x1001e41,
		/// <summary>U+1E56 LATIN CAPITAL LETTER P WITH DOT ABOVE</summary>
		Pabovedot = 0x1001e56,
		/// <summary>U+1E57 LATIN SMALL LETTER P WITH DOT ABOVE</summary>
		pabovedot = 0x1001e57,
		/// <summary>U+1E60 LATIN CAPITAL LETTER S WITH DOT ABOVE</summary>
		Sabovedot = 0x1001e60,
		/// <summary>U+1E61 LATIN SMALL LETTER S WITH DOT ABOVE</summary>
		sabovedot = 0x1001e61,
		/// <summary>U+1E6A LATIN CAPITAL LETTER T WITH DOT ABOVE</summary>
		Tabovedot = 0x1001e6a,
		/// <summary>U+1E6B LATIN SMALL LETTER T WITH DOT ABOVE</summary>
		tabovedot = 0x1001e6b,
		/// <summary>U+1E80 LATIN CAPITAL LETTER W WITH GRAVE</summary>
		Wgrave = 0x1001e80,
		/// <summary>U+1E81 LATIN SMALL LETTER W WITH GRAVE</summary>
		wgrave = 0x1001e81,
		/// <summary>U+1E82 LATIN CAPITAL LETTER W WITH ACUTE</summary>
		Wacute = 0x1001e82,
		/// <summary>U+1E83 LATIN SMALL LETTER W WITH ACUTE</summary>
		wacute = 0x1001e83,
		/// <summary>U+1E84 LATIN CAPITAL LETTER W WITH DIAERESIS</summary>
		Wdiaeresis = 0x1001e84,
		/// <summary>U+1E85 LATIN SMALL LETTER W WITH DIAERESIS</summary>
		wdiaeresis = 0x1001e85,
		/// <summary>U+1EF2 LATIN CAPITAL LETTER Y WITH GRAVE</summary>
		Ygrave = 0x1001ef2,
		/// <summary>U+1EF3 LATIN SMALL LETTER Y WITH GRAVE</summary>
		ygrave = 0x1001ef3,
		// #endif /* XK_LATIN8 */

		/*
		* Latin 9
		* Byte 3 = 0x13
		*/

		// #ifdef XK_LATIN9
		/// <summary>U+0152 LATIN CAPITAL LIGATURE OE</summary>
		OE = 0x13bc,
		/// <summary>U+0153 LATIN SMALL LIGATURE OE</summary>
		oe = 0x13bd,
		/// <summary>U+0178 LATIN CAPITAL LETTER Y WITH DIAERESIS</summary>
		Ydiaeresis = 0x13be,
		// #endif /* XK_LATIN9 */

		/*
		* Katakana
		* Byte 3 = 4
		*/

		// #ifdef XK_KATAKANA
		/// <summary>U+203E OVERLINE</summary>
		Overline = 0x047e,
		/// <summary>U+3002 IDEOGRAPHIC FULL STOP</summary>
		KanaFullstop = 0x04a1,
		/// <summary>U+300C LEFT CORNER BRACKET</summary>
		KanaOpeningbracket = 0x04a2,
		/// <summary>U+300D RIGHT CORNER BRACKET</summary>
		KanaClosingbracket = 0x04a3,
		/// <summary>U+3001 IDEOGRAPHIC COMMA</summary>
		KanaComma = 0x04a4,
		/// <summary>U+30FB KATAKANA MIDDLE DOT</summary>
		KanaConjunctive = 0x04a5,
		/// <summary>deprecated</summary>
		KanaMiddledot = 0x04a5,
		/// <summary>U+30F2 KATAKANA LETTER WO</summary>
		KanaWO = 0x04a6,
		/// <summary>U+30A1 KATAKANA LETTER SMALL A</summary>
		kanaA = 0x04a7,
		/// <summary>U+30A3 KATAKANA LETTER SMALL I</summary>
		kanaI = 0x04a8,
		/// <summary>U+30A5 KATAKANA LETTER SMALL U</summary>
		kanaU = 0x04a9,
		/// <summary>U+30A7 KATAKANA LETTER SMALL E</summary>
		kanaE = 0x04aa,
		/// <summary>U+30A9 KATAKANA LETTER SMALL O</summary>
		kanaO = 0x04ab,
		/// <summary>U+30E3 KATAKANA LETTER SMALL YA</summary>
		kanaYa = 0x04ac,
		/// <summary>U+30E5 KATAKANA LETTER SMALL YU</summary>
		kanaYu = 0x04ad,
		/// <summary>U+30E7 KATAKANA LETTER SMALL YO</summary>
		kanaYo = 0x04ae,
		/// <summary>U+30C3 KATAKANA LETTER SMALL TU</summary>
		kanaTsu = 0x04af,
		/// <summary>deprecated</summary>
		KanaTu = 0x04af,
		/// <summary>U+30FC KATAKANA-HIRAGANA PROLONGED SOUND MARK</summary>
		Prolongedsound = 0x04b0,
		/// <summary>U+30A2 KATAKANA LETTER A</summary>
		KanaA = 0x04b1,
		/// <summary>U+30A4 KATAKANA LETTER I</summary>
		KanaI = 0x04b2,
		/// <summary>U+30A6 KATAKANA LETTER U</summary>
		KanaU = 0x04b3,
		/// <summary>U+30A8 KATAKANA LETTER E</summary>
		KanaE = 0x04b4,
		/// <summary>U+30AA KATAKANA LETTER O</summary>
		KanaO = 0x04b5,
		/// <summary>U+30AB KATAKANA LETTER KA</summary>
		KanaKA = 0x04b6,
		/// <summary>U+30AD KATAKANA LETTER KI</summary>
		KanaKI = 0x04b7,
		/// <summary>U+30AF KATAKANA LETTER KU</summary>
		KanaKU = 0x04b8,
		/// <summary>U+30B1 KATAKANA LETTER KE</summary>
		KanaKE = 0x04b9,
		/// <summary>U+30B3 KATAKANA LETTER KO</summary>
		KanaKO = 0x04ba,
		/// <summary>U+30B5 KATAKANA LETTER SA</summary>
		KanaSA = 0x04bb,
		/// <summary>U+30B7 KATAKANA LETTER SI</summary>
		KanaSHI = 0x04bc,
		/// <summary>U+30B9 KATAKANA LETTER SU</summary>
		KanaSU = 0x04bd,
		/// <summary>U+30BB KATAKANA LETTER SE</summary>
		KanaSE = 0x04be,
		/// <summary>U+30BD KATAKANA LETTER SO</summary>
		KanaSO = 0x04bf,
		/// <summary>U+30BF KATAKANA LETTER TA</summary>
		KanaTA = 0x04c0,
		/// <summary>U+30C1 KATAKANA LETTER TI</summary>
		KanaCHI = 0x04c1,
		/// <summary>deprecated</summary>
		KanaTI = 0x04c1,
		/// <summary>U+30C4 KATAKANA LETTER TU</summary>
		KanaTSU = 0x04c2,
		/// <summary>deprecated</summary>
		KanaTU = 0x04c2,
		/// <summary>U+30C6 KATAKANA LETTER TE</summary>
		KanaTE = 0x04c3,
		/// <summary>U+30C8 KATAKANA LETTER TO</summary>
		KanaTO = 0x04c4,
		/// <summary>U+30CA KATAKANA LETTER NA</summary>
		KanaNA = 0x04c5,
		/// <summary>U+30CB KATAKANA LETTER NI</summary>
		KanaNI = 0x04c6,
		/// <summary>U+30CC KATAKANA LETTER NU</summary>
		KanaNU = 0x04c7,
		/// <summary>U+30CD KATAKANA LETTER NE</summary>
		KanaNE = 0x04c8,
		/// <summary>U+30CE KATAKANA LETTER NO</summary>
		KanaNO = 0x04c9,
		/// <summary>U+30CF KATAKANA LETTER HA</summary>
		KanaHA = 0x04ca,
		/// <summary>U+30D2 KATAKANA LETTER HI</summary>
		KanaHI = 0x04cb,
		/// <summary>U+30D5 KATAKANA LETTER HU</summary>
		KanaFU = 0x04cc,
		/// <summary>deprecated</summary>
		KanaHU = 0x04cc,
		/// <summary>U+30D8 KATAKANA LETTER HE</summary>
		KanaHE = 0x04cd,
		/// <summary>U+30DB KATAKANA LETTER HO</summary>
		KanaHO = 0x04ce,
		/// <summary>U+30DE KATAKANA LETTER MA</summary>
		KanaMA = 0x04cf,
		/// <summary>U+30DF KATAKANA LETTER MI</summary>
		KanaMI = 0x04d0,
		/// <summary>U+30E0 KATAKANA LETTER MU</summary>
		KanaMU = 0x04d1,
		/// <summary>U+30E1 KATAKANA LETTER ME</summary>
		KanaME = 0x04d2,
		/// <summary>U+30E2 KATAKANA LETTER MO</summary>
		KanaMO = 0x04d3,
		/// <summary>U+30E4 KATAKANA LETTER YA</summary>
		KanaYA = 0x04d4,
		/// <summary>U+30E6 KATAKANA LETTER YU</summary>
		KanaYU = 0x04d5,
		/// <summary>U+30E8 KATAKANA LETTER YO</summary>
		KanaYO = 0x04d6,
		/// <summary>U+30E9 KATAKANA LETTER RA</summary>
		KanaRA = 0x04d7,
		/// <summary>U+30EA KATAKANA LETTER RI</summary>
		KanaRI = 0x04d8,
		/// <summary>U+30EB KATAKANA LETTER RU</summary>
		KanaRU = 0x04d9,
		/// <summary>U+30EC KATAKANA LETTER RE</summary>
		KanaRE = 0x04da,
		/// <summary>U+30ED KATAKANA LETTER RO</summary>
		KanaRO = 0x04db,
		/// <summary>U+30EF KATAKANA LETTER WA</summary>
		KanaWA = 0x04dc,
		/// <summary>U+30F3 KATAKANA LETTER N</summary>
		KanaN = 0x04dd,
		/// <summary>U+309B KATAKANA-HIRAGANA VOICED SOUND MARK</summary>
		Voicedsound = 0x04de,
		/// <summary>U+309C KATAKANA-HIRAGANA SEMI-VOICED SOUND MARK</summary>
		Semivoicedsound = 0x04df,
		/// <summary>Alias for mode_switch</summary>
		KanaSwitch = 0xff7e,
		// #endif /* XK_KATAKANA */

		/*
		* Arabic
		* Byte 3 = 5
		*/

		// #ifdef XK_ARABIC
		/// <summary>U+06F0 EXTENDED ARABIC-INDIC DIGIT ZERO</summary>
		Farsi0 = 0x10006f0,
		/// <summary>U+06F1 EXTENDED ARABIC-INDIC DIGIT ONE</summary>
		Farsi1 = 0x10006f1,
		/// <summary>U+06F2 EXTENDED ARABIC-INDIC DIGIT TWO</summary>
		Farsi2 = 0x10006f2,
		/// <summary>U+06F3 EXTENDED ARABIC-INDIC DIGIT THREE</summary>
		Farsi3 = 0x10006f3,
		/// <summary>U+06F4 EXTENDED ARABIC-INDIC DIGIT FOUR</summary>
		Farsi4 = 0x10006f4,
		/// <summary>U+06F5 EXTENDED ARABIC-INDIC DIGIT FIVE</summary>
		Farsi5 = 0x10006f5,
		/// <summary>U+06F6 EXTENDED ARABIC-INDIC DIGIT SIX</summary>
		Farsi6 = 0x10006f6,
		/// <summary>U+06F7 EXTENDED ARABIC-INDIC DIGIT SEVEN</summary>
		Farsi7 = 0x10006f7,
		/// <summary>U+06F8 EXTENDED ARABIC-INDIC DIGIT EIGHT</summary>
		Farsi8 = 0x10006f8,
		/// <summary>U+06F9 EXTENDED ARABIC-INDIC DIGIT NINE</summary>
		Farsi9 = 0x10006f9,
		/// <summary>U+066A ARABIC PERCENT SIGN</summary>
		ArabicPercent = 0x100066a,
		/// <summary>U+0670 ARABIC LETTER SUPERSCRIPT ALEF</summary>
		ArabicSuperscriptAlef = 0x1000670,
		/// <summary>U+0679 ARABIC LETTER TTEH</summary>
		ArabicTteh = 0x1000679,
		/// <summary>U+067E ARABIC LETTER PEH</summary>
		ArabicPeh = 0x100067e,
		/// <summary>U+0686 ARABIC LETTER TCHEH</summary>
		ArabicTcheh = 0x1000686,
		/// <summary>U+0688 ARABIC LETTER DDAL</summary>
		ArabicDdal = 0x1000688,
		/// <summary>U+0691 ARABIC LETTER RREH</summary>
		ArabicRreh = 0x1000691,
		/// <summary>U+060C ARABIC COMMA</summary>
		ArabicComma = 0x05ac,
		/// <summary>U+06D4 ARABIC FULL STOP</summary>
		ArabicFullstop = 0x10006d4,
		/// <summary>U+0660 ARABIC-INDIC DIGIT ZERO</summary>
		Arabic0 = 0x1000660,
		/// <summary>U+0661 ARABIC-INDIC DIGIT ONE</summary>
		Arabic1 = 0x1000661,
		/// <summary>U+0662 ARABIC-INDIC DIGIT TWO</summary>
		Arabic2 = 0x1000662,
		/// <summary>U+0663 ARABIC-INDIC DIGIT THREE</summary>
		Arabic3 = 0x1000663,
		/// <summary>U+0664 ARABIC-INDIC DIGIT FOUR</summary>
		Arabic4 = 0x1000664,
		/// <summary>U+0665 ARABIC-INDIC DIGIT FIVE</summary>
		Arabic5 = 0x1000665,
		/// <summary>U+0666 ARABIC-INDIC DIGIT SIX</summary>
		Arabic6 = 0x1000666,
		/// <summary>U+0667 ARABIC-INDIC DIGIT SEVEN</summary>
		Arabic7 = 0x1000667,
		/// <summary>U+0668 ARABIC-INDIC DIGIT EIGHT</summary>
		Arabic8 = 0x1000668,
		/// <summary>U+0669 ARABIC-INDIC DIGIT NINE</summary>
		Arabic9 = 0x1000669,
		/// <summary>U+061B ARABIC SEMICOLON</summary>
		ArabicSemicolon = 0x05bb,
		/// <summary>U+061F ARABIC QUESTION MARK</summary>
		ArabicQuestionMark = 0x05bf,
		/// <summary>U+0621 ARABIC LETTER HAMZA</summary>
		ArabicHamza = 0x05c1,
		/// <summary>U+0622 ARABIC LETTER ALEF WITH MADDA ABOVE</summary>
		ArabicMaddaonalef = 0x05c2,
		/// <summary>U+0623 ARABIC LETTER ALEF WITH HAMZA ABOVE</summary>
		ArabicHamzaonalef = 0x05c3,
		/// <summary>U+0624 ARABIC LETTER WAW WITH HAMZA ABOVE</summary>
		ArabicHamzaonwaw = 0x05c4,
		/// <summary>U+0625 ARABIC LETTER ALEF WITH HAMZA BELOW</summary>
		ArabicHamzaunderalef = 0x05c5,
		/// <summary>U+0626 ARABIC LETTER YEH WITH HAMZA ABOVE</summary>
		ArabicHamzaonyeh = 0x05c6,
		/// <summary>U+0627 ARABIC LETTER ALEF</summary>
		ArabicAlef = 0x05c7,
		/// <summary>U+0628 ARABIC LETTER BEH</summary>
		ArabicBeh = 0x05c8,
		/// <summary>U+0629 ARABIC LETTER TEH MARBUTA</summary>
		ArabicTehmarbuta = 0x05c9,
		/// <summary>U+062A ARABIC LETTER TEH</summary>
		ArabicTeh = 0x05ca,
		/// <summary>U+062B ARABIC LETTER THEH</summary>
		ArabicTheh = 0x05cb,
		/// <summary>U+062C ARABIC LETTER JEEM</summary>
		ArabicJeem = 0x05cc,
		/// <summary>U+062D ARABIC LETTER HAH</summary>
		ArabicHah = 0x05cd,
		/// <summary>U+062E ARABIC LETTER KHAH</summary>
		ArabicKhah = 0x05ce,
		/// <summary>U+062F ARABIC LETTER DAL</summary>
		ArabicDal = 0x05cf,
		/// <summary>U+0630 ARABIC LETTER THAL</summary>
		ArabicThal = 0x05d0,
		/// <summary>U+0631 ARABIC LETTER REH</summary>
		ArabicRa = 0x05d1,
		/// <summary>U+0632 ARABIC LETTER ZAIN</summary>
		ArabicZain = 0x05d2,
		/// <summary>U+0633 ARABIC LETTER SEEN</summary>
		ArabicSeen = 0x05d3,
		/// <summary>U+0634 ARABIC LETTER SHEEN</summary>
		ArabicSheen = 0x05d4,
		/// <summary>U+0635 ARABIC LETTER SAD</summary>
		ArabicSad = 0x05d5,
		/// <summary>U+0636 ARABIC LETTER DAD</summary>
		ArabicDad = 0x05d6,
		/// <summary>U+0637 ARABIC LETTER TAH</summary>
		ArabicTah = 0x05d7,
		/// <summary>U+0638 ARABIC LETTER ZAH</summary>
		ArabicZah = 0x05d8,
		/// <summary>U+0639 ARABIC LETTER AIN</summary>
		ArabicAin = 0x05d9,
		/// <summary>U+063A ARABIC LETTER GHAIN</summary>
		ArabicGhain = 0x05da,
		/// <summary>U+0640 ARABIC TATWEEL</summary>
		ArabicTatweel = 0x05e0,
		/// <summary>U+0641 ARABIC LETTER FEH</summary>
		ArabicFeh = 0x05e1,
		/// <summary>U+0642 ARABIC LETTER QAF</summary>
		ArabicQaf = 0x05e2,
		/// <summary>U+0643 ARABIC LETTER KAF</summary>
		ArabicKaf = 0x05e3,
		/// <summary>U+0644 ARABIC LETTER LAM</summary>
		ArabicLam = 0x05e4,
		/// <summary>U+0645 ARABIC LETTER MEEM</summary>
		ArabicMeem = 0x05e5,
		/// <summary>U+0646 ARABIC LETTER NOON</summary>
		ArabicNoon = 0x05e6,
		/// <summary>U+0647 ARABIC LETTER HEH</summary>
		ArabicHa = 0x05e7,
		/// <summary>deprecated</summary>
		ArabicHeh = 0x05e7,
		/// <summary>U+0648 ARABIC LETTER WAW</summary>
		ArabicWaw = 0x05e8,
		/// <summary>U+0649 ARABIC LETTER ALEF MAKSURA</summary>
		ArabicAlefmaksura = 0x05e9,
		/// <summary>U+064A ARABIC LETTER YEH</summary>
		ArabicYeh = 0x05ea,
		/// <summary>U+064B ARABIC FATHATAN</summary>
		ArabicFathatan = 0x05eb,
		/// <summary>U+064C ARABIC DAMMATAN</summary>
		ArabicDammatan = 0x05ec,
		/// <summary>U+064D ARABIC KASRATAN</summary>
		ArabicKasratan = 0x05ed,
		/// <summary>U+064E ARABIC FATHA</summary>
		ArabicFatha = 0x05ee,
		/// <summary>U+064F ARABIC DAMMA</summary>
		ArabicDamma = 0x05ef,
		/// <summary>U+0650 ARABIC KASRA</summary>
		ArabicKasra = 0x05f0,
		/// <summary>U+0651 ARABIC SHADDA</summary>
		ArabicShadda = 0x05f1,
		/// <summary>U+0652 ARABIC SUKUN</summary>
		ArabicSukun = 0x05f2,
		/// <summary>U+0653 ARABIC MADDAH ABOVE</summary>
		ArabicMaddaAbove = 0x1000653,
		/// <summary>U+0654 ARABIC HAMZA ABOVE</summary>
		ArabicHamzaAbove = 0x1000654,
		/// <summary>U+0655 ARABIC HAMZA BELOW</summary>
		ArabicHamzaBelow = 0x1000655,
		/// <summary>U+0698 ARABIC LETTER JEH</summary>
		ArabicJeh = 0x1000698,
		/// <summary>U+06A4 ARABIC LETTER VEH</summary>
		ArabicVeh = 0x10006a4,
		/// <summary>U+06A9 ARABIC LETTER KEHEH</summary>
		ArabicKeheh = 0x10006a9,
		/// <summary>U+06AF ARABIC LETTER GAF</summary>
		ArabicGaf = 0x10006af,
		/// <summary>U+06BA ARABIC LETTER NOON GHUNNA</summary>
		ArabicNoonGhunna = 0x10006ba,
		/// <summary>U+06BE ARABIC LETTER HEH DOACHASHMEE</summary>
		ArabicHehDoachashmee = 0x10006be,
		/// <summary>U+06CC ARABIC LETTER FARSI YEH</summary>
		FarsiYeh = 0x10006cc,
		/// <summary>U+06CC ARABIC LETTER FARSI YEH</summary>
		ArabicFarsiYeh = 0x10006cc,
		/// <summary>U+06D2 ARABIC LETTER YEH BARREE</summary>
		ArabicYehBaree = 0x10006d2,
		/// <summary>U+06C1 ARABIC LETTER HEH GOAL</summary>
		ArabicHehGoal = 0x10006c1,
		/// <summary>Alias for mode_switch</summary>
		ArabicSwitch = 0xff7e,
		// #endif /* XK_ARABIC */

		/*
		* Cyrillic
		* Byte 3 = 6
		*/
		// #ifdef XK_CYRILLIC
		/// <summary>U+0492 CYRILLIC CAPITAL LETTER GHE WITH STROKE</summary>
		CyrillicGHEBar = 0x1000492,
		/// <summary>U+0493 CYRILLIC SMALL LETTER GHE WITH STROKE</summary>
		cyrillicGheBar = 0x1000493,
		/// <summary>U+0496 CYRILLIC CAPITAL LETTER ZHE WITH DESCENDER</summary>
		CyrillicZHEDescender = 0x1000496,
		/// <summary>U+0497 CYRILLIC SMALL LETTER ZHE WITH DESCENDER</summary>
		cyrillicZheDescender = 0x1000497,
		/// <summary>U+049A CYRILLIC CAPITAL LETTER KA WITH DESCENDER</summary>
		CyrillicKADescender = 0x100049a,
		/// <summary>U+049B CYRILLIC SMALL LETTER KA WITH DESCENDER</summary>
		cyrillicKaDescender = 0x100049b,
		/// <summary>U+049C CYRILLIC CAPITAL LETTER KA WITH VERTICAL STROKE</summary>
		CyrillicKAVertstroke = 0x100049c,
		/// <summary>U+049D CYRILLIC SMALL LETTER KA WITH VERTICAL STROKE</summary>
		cyrillicKaVertstroke = 0x100049d,
		/// <summary>U+04A2 CYRILLIC CAPITAL LETTER EN WITH DESCENDER</summary>
		CyrillicENDescender = 0x10004a2,
		/// <summary>U+04A3 CYRILLIC SMALL LETTER EN WITH DESCENDER</summary>
		cyrillicEnDescender = 0x10004a3,
		/// <summary>U+04AE CYRILLIC CAPITAL LETTER STRAIGHT U</summary>
		CyrillicUStraight = 0x10004ae,
		/// <summary>U+04AF CYRILLIC SMALL LETTER STRAIGHT U</summary>
		cyrillicUStraight = 0x10004af,
		/// <summary>U+04B0 CYRILLIC CAPITAL LETTER STRAIGHT U WITH STROKE</summary>
		CyrillicUStraightBar = 0x10004b0,
		/// <summary>U+04B1 CYRILLIC SMALL LETTER STRAIGHT U WITH STROKE</summary>
		cyrillicUStraightBar = 0x10004b1,
		/// <summary>U+04B2 CYRILLIC CAPITAL LETTER HA WITH DESCENDER</summary>
		CyrillicHADescender = 0x10004b2,
		/// <summary>U+04B3 CYRILLIC SMALL LETTER HA WITH DESCENDER</summary>
		cyrillicHaDescender = 0x10004b3,
		/// <summary>U+04B6 CYRILLIC CAPITAL LETTER CHE WITH DESCENDER</summary>
		CyrillicCHEDescender = 0x10004b6,
		/// <summary>U+04B7 CYRILLIC SMALL LETTER CHE WITH DESCENDER</summary>
		cyrillicCheDescender = 0x10004b7,
		/// <summary>U+04B8 CYRILLIC CAPITAL LETTER CHE WITH VERTICAL STROKE</summary>
		CyrillicCHEVertstroke = 0x10004b8,
		/// <summary>U+04B9 CYRILLIC SMALL LETTER CHE WITH VERTICAL STROKE</summary>
		cyrillicCheVertstroke = 0x10004b9,
		/// <summary>U+04BA CYRILLIC CAPITAL LETTER SHHA</summary>
		CyrillicSHHA = 0x10004ba,
		/// <summary>U+04BB CYRILLIC SMALL LETTER SHHA</summary>
		cyrillicShha = 0x10004bb,

		/// <summary>U+04D8 CYRILLIC CAPITAL LETTER SCHWA</summary>
		CyrillicSCHWA = 0x10004d8,
		/// <summary>U+04D9 CYRILLIC SMALL LETTER SCHWA</summary>
		cyrillicSchwa = 0x10004d9,
		/// <summary>U+04E2 CYRILLIC CAPITAL LETTER I WITH MACRON</summary>
		CyrillicIMacron = 0x10004e2,
		/// <summary>U+04E3 CYRILLIC SMALL LETTER I WITH MACRON</summary>
		cyrillicIMacron = 0x10004e3,
		/// <summary>U+04E8 CYRILLIC CAPITAL LETTER BARRED O</summary>
		CyrillicOBar = 0x10004e8,
		/// <summary>U+04E9 CYRILLIC SMALL LETTER BARRED O</summary>
		cyrillicOBar = 0x10004e9,
		/// <summary>U+04EE CYRILLIC CAPITAL LETTER U WITH MACRON</summary>
		CyrillicUMacron = 0x10004ee,
		/// <summary>U+04EF CYRILLIC SMALL LETTER U WITH MACRON</summary>
		cyrillicUMacron = 0x10004ef,

		/// <summary>U+0452 CYRILLIC SMALL LETTER DJE</summary>
		serbianDje = 0x06a1,
		/// <summary>U+0453 CYRILLIC SMALL LETTER GJE</summary>
		macedoniaGje = 0x06a2,
		/// <summary>U+0451 CYRILLIC SMALL LETTER IO</summary>
		cyrillicIo = 0x06a3,
		/// <summary>U+0454 CYRILLIC SMALL LETTER UKRAINIAN IE</summary>
		ukrainianIe = 0x06a4,
		/// <summary>deprecated</summary>
		UkranianJe = 0x06a4,
		/// <summary>U+0455 CYRILLIC SMALL LETTER DZE</summary>
		macedoniaDse = 0x06a5,
		/// <summary>U+0456 CYRILLIC SMALL LETTER BYELORUSSIAN-UKRAINIAN I</summary>
		ukrainianI = 0x06a6,
		/// <summary>deprecated</summary>
		ukranianI = 0x06a6,
		/// <summary>U+0457 CYRILLIC SMALL LETTER YI</summary>
		ukrainianYi = 0x06a7,
		/// <summary>deprecated</summary>
		UkranianYi = 0x06a7,
		/// <summary>U+0458 CYRILLIC SMALL LETTER JE</summary>
		cyrillicJe = 0x06a8,
		/// <summary>deprecated</summary>
		SerbianJe = 0x06a8,
		/// <summary>U+0459 CYRILLIC SMALL LETTER LJE</summary>
		cyrillicLje = 0x06a9,
		/// <summary>deprecated</summary>
		SerbianLje = 0x06a9,
		/// <summary>U+045A CYRILLIC SMALL LETTER NJE</summary>
		cyrillicNje = 0x06aa,
		/// <summary>deprecated</summary>
		SerbianNje = 0x06aa,
		/// <summary>U+045B CYRILLIC SMALL LETTER TSHE</summary>
		serbianTshe = 0x06ab,
		/// <summary>U+045C CYRILLIC SMALL LETTER KJE</summary>
		macedoniaKje = 0x06ac,
		/// <summary>U+0491 CYRILLIC SMALL LETTER GHE WITH UPTURN</summary>
		ukrainianGheWithUpturn = 0x06ad,
		/// <summary>U+045E CYRILLIC SMALL LETTER SHORT U</summary>
		byelorussianShortu = 0x06ae,
		/// <summary>U+045F CYRILLIC SMALL LETTER DZHE</summary>
		cyrillicDzhe = 0x06af,
		/// <summary>deprecated</summary>
		SerbianDze = 0x06af,
		/// <summary>U+2116 NUMERO SIGN</summary>
		Numerosign = 0x06b0,
		/// <summary>U+0402 CYRILLIC CAPITAL LETTER DJE</summary>
		SerbianDJE = 0x06b1,
		/// <summary>U+0403 CYRILLIC CAPITAL LETTER GJE</summary>
		MacedoniaGJE = 0x06b2,
		/// <summary>U+0401 CYRILLIC CAPITAL LETTER IO</summary>
		CyrillicIO = 0x06b3,
		/// <summary>U+0404 CYRILLIC CAPITAL LETTER UKRAINIAN IE</summary>
		UkrainianIE = 0x06b4,
		/// <summary>deprecated</summary>
		UkranianJE = 0x06b4,
		/// <summary>U+0405 CYRILLIC CAPITAL LETTER DZE</summary>
		MacedoniaDSE = 0x06b5,
		/// <summary>U+0406 CYRILLIC CAPITAL LETTER BYELORUSSIAN-UKRAINIAN I</summary>
		UkrainianI = 0x06b6,
		/// <summary>deprecated</summary>
		UkranianI = 0x06b6,
		/// <summary>U+0407 CYRILLIC CAPITAL LETTER YI</summary>
		UkrainianYI = 0x06b7,
		/// <summary>deprecated</summary>
		UkranianYI = 0x06b7,
		/// <summary>U+0408 CYRILLIC CAPITAL LETTER JE</summary>
		CyrillicJE = 0x06b8,
		/// <summary>deprecated</summary>
		SerbianJE = 0x06b8,
		/// <summary>U+0409 CYRILLIC CAPITAL LETTER LJE</summary>
		CyrillicLJE = 0x06b9,
		/// <summary>deprecated</summary>
		SerbianLJE = 0x06b9,
		/// <summary>U+040A CYRILLIC CAPITAL LETTER NJE</summary>
		CyrillicNJE = 0x06ba,
		/// <summary>deprecated</summary>
		SerbianNJE = 0x06ba,
		/// <summary>U+040B CYRILLIC CAPITAL LETTER TSHE</summary>
		SerbianTSHE = 0x06bb,
		/// <summary>U+040C CYRILLIC CAPITAL LETTER KJE</summary>
		MacedoniaKJE = 0x06bc,
		/// <summary>U+0490 CYRILLIC CAPITAL LETTER GHE WITH UPTURN</summary>
		UkrainianGHEWITHUPTURN = 0x06bd,
		/// <summary>U+040E CYRILLIC CAPITAL LETTER SHORT U</summary>
		ByelorussianSHORTU = 0x06be,
		/// <summary>U+040F CYRILLIC CAPITAL LETTER DZHE</summary>
		CyrillicDZHE = 0x06bf,
		/// <summary>deprecated</summary>
		SerbianDZE = 0x06bf,
		/// <summary>U+044E CYRILLIC SMALL LETTER YU</summary>
		cyrillicYu = 0x06c0,
		/// <summary>U+0430 CYRILLIC SMALL LETTER A</summary>
		cyrillicA = 0x06c1,
		/// <summary>U+0431 CYRILLIC SMALL LETTER BE</summary>
		cyrillicBe = 0x06c2,
		/// <summary>U+0446 CYRILLIC SMALL LETTER TSE</summary>
		cyrillicTse = 0x06c3,
		/// <summary>U+0434 CYRILLIC SMALL LETTER DE</summary>
		cyrillicDe = 0x06c4,
		/// <summary>U+0435 CYRILLIC SMALL LETTER IE</summary>
		cyrillicIe = 0x06c5,
		/// <summary>U+0444 CYRILLIC SMALL LETTER EF</summary>
		cyrillicEf = 0x06c6,
		/// <summary>U+0433 CYRILLIC SMALL LETTER GHE</summary>
		cyrillicGhe = 0x06c7,
		/// <summary>U+0445 CYRILLIC SMALL LETTER HA</summary>
		cyrillicHa = 0x06c8,
		/// <summary>U+0438 CYRILLIC SMALL LETTER I</summary>
		cyrillicI = 0x06c9,
		/// <summary>U+0439 CYRILLIC SMALL LETTER SHORT I</summary>
		cyrillicShorti = 0x06ca,
		/// <summary>U+043A CYRILLIC SMALL LETTER KA</summary>
		cyrillicKa = 0x06cb,
		/// <summary>U+043B CYRILLIC SMALL LETTER EL</summary>
		cyrillicEl = 0x06cc,
		/// <summary>U+043C CYRILLIC SMALL LETTER EM</summary>
		cyrillicEm = 0x06cd,
		/// <summary>U+043D CYRILLIC SMALL LETTER EN</summary>
		cyrillicEn = 0x06ce,
		/// <summary>U+043E CYRILLIC SMALL LETTER O</summary>
		cyrillicO = 0x06cf,
		/// <summary>U+043F CYRILLIC SMALL LETTER PE</summary>
		cyrillicPe = 0x06d0,
		/// <summary>U+044F CYRILLIC SMALL LETTER YA</summary>
		cyrillicYa = 0x06d1,
		/// <summary>U+0440 CYRILLIC SMALL LETTER ER</summary>
		cyrillicEr = 0x06d2,
		/// <summary>U+0441 CYRILLIC SMALL LETTER ES</summary>
		cyrillicEs = 0x06d3,
		/// <summary>U+0442 CYRILLIC SMALL LETTER TE</summary>
		cyrillicTe = 0x06d4,
		/// <summary>U+0443 CYRILLIC SMALL LETTER U</summary>
		cyrillicU = 0x06d5,
		/// <summary>U+0436 CYRILLIC SMALL LETTER ZHE</summary>
		cyrillicZhe = 0x06d6,
		/// <summary>U+0432 CYRILLIC SMALL LETTER VE</summary>
		cyrillicVe = 0x06d7,
		/// <summary>U+044C CYRILLIC SMALL LETTER SOFT SIGN</summary>
		cyrillicSoftsign = 0x06d8,
		/// <summary>U+044B CYRILLIC SMALL LETTER YERU</summary>
		cyrillicYeru = 0x06d9,
		/// <summary>U+0437 CYRILLIC SMALL LETTER ZE</summary>
		cyrillicZe = 0x06da,
		/// <summary>U+0448 CYRILLIC SMALL LETTER SHA</summary>
		cyrillicSha = 0x06db,
		/// <summary>U+044D CYRILLIC SMALL LETTER E</summary>
		cyrillicE = 0x06dc,
		/// <summary>U+0449 CYRILLIC SMALL LETTER SHCHA</summary>
		cyrillicShcha = 0x06dd,
		/// <summary>U+0447 CYRILLIC SMALL LETTER CHE</summary>
		cyrillicChe = 0x06de,
		/// <summary>U+044A CYRILLIC SMALL LETTER HARD SIGN</summary>
		cyrillicHardsign = 0x06df,
		/// <summary>U+042E CYRILLIC CAPITAL LETTER YU</summary>
		CyrillicYU = 0x06e0,
		/// <summary>U+0410 CYRILLIC CAPITAL LETTER A</summary>
		CyrillicA = 0x06e1,
		/// <summary>U+0411 CYRILLIC CAPITAL LETTER BE</summary>
		CyrillicBE = 0x06e2,
		/// <summary>U+0426 CYRILLIC CAPITAL LETTER TSE</summary>
		CyrillicTSE = 0x06e3,
		/// <summary>U+0414 CYRILLIC CAPITAL LETTER DE</summary>
		CyrillicDE = 0x06e4,
		/// <summary>U+0415 CYRILLIC CAPITAL LETTER IE</summary>
		CyrillicIE = 0x06e5,
		/// <summary>U+0424 CYRILLIC CAPITAL LETTER EF</summary>
		CyrillicEF = 0x06e6,
		/// <summary>U+0413 CYRILLIC CAPITAL LETTER GHE</summary>
		CyrillicGHE = 0x06e7,
		/// <summary>U+0425 CYRILLIC CAPITAL LETTER HA</summary>
		CyrillicHA = 0x06e8,
		/// <summary>U+0418 CYRILLIC CAPITAL LETTER I</summary>
		CyrillicI = 0x06e9,
		/// <summary>U+0419 CYRILLIC CAPITAL LETTER SHORT I</summary>
		CyrillicSHORTI = 0x06ea,
		/// <summary>U+041A CYRILLIC CAPITAL LETTER KA</summary>
		CyrillicKA = 0x06eb,
		/// <summary>U+041B CYRILLIC CAPITAL LETTER EL</summary>
		CyrillicEL = 0x06ec,
		/// <summary>U+041C CYRILLIC CAPITAL LETTER EM</summary>
		CyrillicEM = 0x06ed,
		/// <summary>U+041D CYRILLIC CAPITAL LETTER EN</summary>
		CyrillicEN = 0x06ee,
		/// <summary>U+041E CYRILLIC CAPITAL LETTER O</summary>
		CyrillicO = 0x06ef,
		/// <summary>U+041F CYRILLIC CAPITAL LETTER PE</summary>
		CyrillicPE = 0x06f0,
		/// <summary>U+042F CYRILLIC CAPITAL LETTER YA</summary>
		CyrillicYA = 0x06f1,
		/// <summary>U+0420 CYRILLIC CAPITAL LETTER ER</summary>
		CyrillicER = 0x06f2,
		/// <summary>U+0421 CYRILLIC CAPITAL LETTER ES</summary>
		CyrillicES = 0x06f3,
		/// <summary>U+0422 CYRILLIC CAPITAL LETTER TE</summary>
		CyrillicTE = 0x06f4,
		/// <summary>U+0423 CYRILLIC CAPITAL LETTER U</summary>
		CyrillicU = 0x06f5,
		/// <summary>U+0416 CYRILLIC CAPITAL LETTER ZHE</summary>
		CyrillicZHE = 0x06f6,
		/// <summary>U+0412 CYRILLIC CAPITAL LETTER VE</summary>
		CyrillicVE = 0x06f7,
		/// <summary>U+042C CYRILLIC CAPITAL LETTER SOFT SIGN</summary>
		CyrillicSOFTSIGN = 0x06f8,
		/// <summary>U+042B CYRILLIC CAPITAL LETTER YERU</summary>
		CyrillicYERU = 0x06f9,
		/// <summary>U+0417 CYRILLIC CAPITAL LETTER ZE</summary>
		CyrillicZE = 0x06fa,
		/// <summary>U+0428 CYRILLIC CAPITAL LETTER SHA</summary>
		CyrillicSHA = 0x06fb,
		/// <summary>U+042D CYRILLIC CAPITAL LETTER E</summary>
		CyrillicE = 0x06fc,
		/// <summary>U+0429 CYRILLIC CAPITAL LETTER SHCHA</summary>
		CyrillicSHCHA = 0x06fd,
		/// <summary>U+0427 CYRILLIC CAPITAL LETTER CHE</summary>
		CyrillicCHE = 0x06fe,
		/// <summary>U+042A CYRILLIC CAPITAL LETTER HARD SIGN</summary>
		CyrillicHARDSIGN = 0x06ff,
		// #endif /* XK_CYRILLIC */

		/*
		* Greek
		* (based on an early draft of, and not quite identical to, ISO/IEC 8859-7)
		* Byte 3 = 7
		*/

		// #ifdef XK_GREEK
		/// <summary>U+0386 GREEK CAPITAL LETTER ALPHA WITH TONOS</summary>
		GreekALPHAaccent = 0x07a1,
		/// <summary>U+0388 GREEK CAPITAL LETTER EPSILON WITH TONOS</summary>
		GreekEPSILONaccent = 0x07a2,
		/// <summary>U+0389 GREEK CAPITAL LETTER ETA WITH TONOS</summary>
		GreekETAaccent = 0x07a3,
		/// <summary>U+038A GREEK CAPITAL LETTER IOTA WITH TONOS</summary>
		GreekIOTAaccent = 0x07a4,
		/// <summary>U+03AA GREEK CAPITAL LETTER IOTA WITH DIALYTIKA</summary>
		GreekIOTAdieresis = 0x07a5,
		/// <summary>old typo</summary>
		GreekIOTAdiaeresis = 0x07a5,
		/// <summary>U+038C GREEK CAPITAL LETTER OMICRON WITH TONOS</summary>
		GreekOMICRONaccent = 0x07a7,
		/// <summary>U+038E GREEK CAPITAL LETTER UPSILON WITH TONOS</summary>
		GreekUPSILONaccent = 0x07a8,
		/// <summary>U+03AB GREEK CAPITAL LETTER UPSILON WITH DIALYTIKA</summary>
		GreekUPSILONdieresis = 0x07a9,
		/// <summary>U+038F GREEK CAPITAL LETTER OMEGA WITH TONOS</summary>
		GreekOMEGAaccent = 0x07ab,
		/// <summary>U+0385 GREEK DIALYTIKA TONOS</summary>
		GreekAccentdieresis = 0x07ae,
		/// <summary>U+2015 HORIZONTAL BAR</summary>
		GreekHorizbar = 0x07af,
		/// <summary>U+03AC GREEK SMALL LETTER ALPHA WITH TONOS</summary>
		greekAlphaaccent = 0x07b1,
		/// <summary>U+03AD GREEK SMALL LETTER EPSILON WITH TONOS</summary>
		greekEpsilonaccent = 0x07b2,
		/// <summary>U+03AE GREEK SMALL LETTER ETA WITH TONOS</summary>
		greekEtaaccent = 0x07b3,
		/// <summary>U+03AF GREEK SMALL LETTER IOTA WITH TONOS</summary>
		greekIotaaccent = 0x07b4,
		/// <summary>U+03CA GREEK SMALL LETTER IOTA WITH DIALYTIKA</summary>
		greekIotadieresis = 0x07b5,
		/// <summary>U+0390 GREEK SMALL LETTER IOTA WITH DIALYTIKA AND TONOS</summary>
		greekIotaaccentdieresis = 0x07b6,
		/// <summary>U+03CC GREEK SMALL LETTER OMICRON WITH TONOS</summary>
		greekOmicronaccent = 0x07b7,
		/// <summary>U+03CD GREEK SMALL LETTER UPSILON WITH TONOS</summary>
		greekUpsilonaccent = 0x07b8,
		/// <summary>U+03CB GREEK SMALL LETTER UPSILON WITH DIALYTIKA</summary>
		greekUpsilondieresis = 0x07b9,
		/// <summary>U+03B0 GREEK SMALL LETTER UPSILON WITH DIALYTIKA AND TONOS</summary>
		greekUpsilonaccentdieresis = 0x07ba,
		/// <summary>U+03CE GREEK SMALL LETTER OMEGA WITH TONOS</summary>
		greekOmegaaccent = 0x07bb,
		/// <summary>U+0391 GREEK CAPITAL LETTER ALPHA</summary>
		GreekALPHA = 0x07c1,
		/// <summary>U+0392 GREEK CAPITAL LETTER BETA</summary>
		GreekBETA = 0x07c2,
		/// <summary>U+0393 GREEK CAPITAL LETTER GAMMA</summary>
		GreekGAMMA = 0x07c3,
		/// <summary>U+0394 GREEK CAPITAL LETTER DELTA</summary>
		GreekDELTA = 0x07c4,
		/// <summary>U+0395 GREEK CAPITAL LETTER EPSILON</summary>
		GreekEPSILON = 0x07c5,
		/// <summary>U+0396 GREEK CAPITAL LETTER ZETA</summary>
		GreekZETA = 0x07c6,
		/// <summary>U+0397 GREEK CAPITAL LETTER ETA</summary>
		GreekETA = 0x07c7,
		/// <summary>U+0398 GREEK CAPITAL LETTER THETA</summary>
		GreekTHETA = 0x07c8,
		/// <summary>U+0399 GREEK CAPITAL LETTER IOTA</summary>
		GreekIOTA = 0x07c9,
		/// <summary>U+039A GREEK CAPITAL LETTER KAPPA</summary>
		GreekKAPPA = 0x07ca,
		/// <summary>U+039B GREEK CAPITAL LETTER LAMDA</summary>
		GreekLAMDA = 0x07cb,
		/// <summary>U+039B GREEK CAPITAL LETTER LAMDA</summary>
		GreekLAMBDA = 0x07cb,
		/// <summary>U+039C GREEK CAPITAL LETTER MU</summary>
		GreekMU = 0x07cc,
		/// <summary>U+039D GREEK CAPITAL LETTER NU</summary>
		GreekNU = 0x07cd,
		/// <summary>U+039E GREEK CAPITAL LETTER XI</summary>
		GreekXI = 0x07ce,
		/// <summary>U+039F GREEK CAPITAL LETTER OMICRON</summary>
		GreekOMICRON = 0x07cf,
		/// <summary>U+03A0 GREEK CAPITAL LETTER PI</summary>
		GreekPI = 0x07d0,
		/// <summary>U+03A1 GREEK CAPITAL LETTER RHO</summary>
		GreekRHO = 0x07d1,
		/// <summary>U+03A3 GREEK CAPITAL LETTER SIGMA</summary>
		GreekSIGMA = 0x07d2,
		/// <summary>U+03A4 GREEK CAPITAL LETTER TAU</summary>
		GreekTAU = 0x07d4,
		/// <summary>U+03A5 GREEK CAPITAL LETTER UPSILON</summary>
		GreekUPSILON = 0x07d5,
		/// <summary>U+03A6 GREEK CAPITAL LETTER PHI</summary>
		GreekPHI = 0x07d6,
		/// <summary>U+03A7 GREEK CAPITAL LETTER CHI</summary>
		GreekCHI = 0x07d7,
		/// <summary>U+03A8 GREEK CAPITAL LETTER PSI</summary>
		GreekPSI = 0x07d8,
		/// <summary>U+03A9 GREEK CAPITAL LETTER OMEGA</summary>
		GreekOMEGA = 0x07d9,
		/// <summary>U+03B1 GREEK SMALL LETTER ALPHA</summary>
		greekAlpha = 0x07e1,
		/// <summary>U+03B2 GREEK SMALL LETTER BETA</summary>
		greekBeta = 0x07e2,
		/// <summary>U+03B3 GREEK SMALL LETTER GAMMA</summary>
		greekGamma = 0x07e3,
		/// <summary>U+03B4 GREEK SMALL LETTER DELTA</summary>
		greekDelta = 0x07e4,
		/// <summary>U+03B5 GREEK SMALL LETTER EPSILON</summary>
		greekEpsilon = 0x07e5,
		/// <summary>U+03B6 GREEK SMALL LETTER ZETA</summary>
		greekZeta = 0x07e6,
		/// <summary>U+03B7 GREEK SMALL LETTER ETA</summary>
		greekEta = 0x07e7,
		/// <summary>U+03B8 GREEK SMALL LETTER THETA</summary>
		greekTheta = 0x07e8,
		/// <summary>U+03B9 GREEK SMALL LETTER IOTA</summary>
		greekIota = 0x07e9,
		/// <summary>U+03BA GREEK SMALL LETTER KAPPA</summary>
		greekKappa = 0x07ea,
		/// <summary>U+03BB GREEK SMALL LETTER LAMDA</summary>
		greekLamda = 0x07eb,
		/// <summary>U+03BB GREEK SMALL LETTER LAMDA</summary>
		greekLambda = 0x07eb,
		/// <summary>U+03BC GREEK SMALL LETTER MU</summary>
		greekMu = 0x07ec,
		/// <summary>U+03BD GREEK SMALL LETTER NU</summary>
		greekNu = 0x07ed,
		/// <summary>U+03BE GREEK SMALL LETTER XI</summary>
		greekXi = 0x07ee,
		/// <summary>U+03BF GREEK SMALL LETTER OMICRON</summary>
		greekOmicron = 0x07ef,
		/// <summary>U+03C0 GREEK SMALL LETTER PI</summary>
		greekPi = 0x07f0,
		/// <summary>U+03C1 GREEK SMALL LETTER RHO</summary>
		greekRho = 0x07f1,
		/// <summary>U+03C3 GREEK SMALL LETTER SIGMA</summary>
		greekSigma = 0x07f2,
		/// <summary>U+03C2 GREEK SMALL LETTER FINAL SIGMA</summary>
		greekFinalsmallsigma = 0x07f3,
		/// <summary>U+03C4 GREEK SMALL LETTER TAU</summary>
		greekTau = 0x07f4,
		/// <summary>U+03C5 GREEK SMALL LETTER UPSILON</summary>
		greekUpsilon = 0x07f5,
		/// <summary>U+03C6 GREEK SMALL LETTER PHI</summary>
		greekPhi = 0x07f6,
		/// <summary>U+03C7 GREEK SMALL LETTER CHI</summary>
		greekChi = 0x07f7,
		/// <summary>U+03C8 GREEK SMALL LETTER PSI</summary>
		greekPsi = 0x07f8,
		/// <summary>U+03C9 GREEK SMALL LETTER OMEGA</summary>
		greekOmega = 0x07f9,
		/// <summary>Alias for mode_switch</summary>
		GreekSwitch = 0xff7e,
		// #endif /* XK_GREEK */

		/*
		* Technical
		* (from the DEC VT330/VT420 Technical Character Set, http://vt100.net/charsets/technical.html)
		* Byte 3 = 8
		*/

		// #ifdef XK_TECHNICAL
		/// <summary>U+23B7 RADICAL SYMBOL BOTTOM</summary>
		Leftradical = 0x08a1,
		/// <summary>(U+250C BOX DRAWINGS LIGHT DOWN AND RIGHT)</summary>
		Topleftradical = 0x08a2,
		/// <summary>(U+2500 BOX DRAWINGS LIGHT HORIZONTAL)</summary>
		Horizconnector = 0x08a3,
		/// <summary>U+2320 TOP HALF INTEGRAL</summary>
		Topintegral = 0x08a4,
		/// <summary>U+2321 BOTTOM HALF INTEGRAL</summary>
		Botintegral = 0x08a5,
		/// <summary>(U+2502 BOX DRAWINGS LIGHT VERTICAL)</summary>
		Vertconnector = 0x08a6,
		/// <summary>U+23A1 LEFT SQUARE BRACKET UPPER CORNER</summary>
		Topleftsqbracket = 0x08a7,
		/// <summary>U+23A3 LEFT SQUARE BRACKET LOWER CORNER</summary>
		botleftsqbracket = 0x08a8,
		/// <summary>U+23A4 RIGHT SQUARE BRACKET UPPER CORNER</summary>
		Toprightsqbracket = 0x08a9,
		/// <summary>U+23A6 RIGHT SQUARE BRACKET LOWER CORNER</summary>
		botrightsqbracket = 0x08aa,
		/// <summary>U+239B LEFT PARENTHESIS UPPER HOOK</summary>
		Topleftparens = 0x08ab,
		/// <summary>U+239D LEFT PARENTHESIS LOWER HOOK</summary>
		botleftparens = 0x08ac,
		/// <summary>U+239E RIGHT PARENTHESIS UPPER HOOK</summary>
		Toprightparens = 0x08ad,
		/// <summary>U+23A0 RIGHT PARENTHESIS LOWER HOOK</summary>
		botrightparens = 0x08ae,
		/// <summary>U+23A8 LEFT CURLY BRACKET MIDDLE PIECE</summary>
		Leftmiddlecurlybrace = 0x08af,
		/// <summary>U+23AC RIGHT CURLY BRACKET MIDDLE PIECE</summary>
		Rightmiddlecurlybrace = 0x08b0,
		/// <summary></summary>
		Topleftsummation = 0x08b1,
		/// <summary></summary>
		Botleftsummation = 0x08b2,
		/// <summary></summary>
		Topvertsummationconnector = 0x08b3,
		/// <summary></summary>
		Botvertsummationconnector = 0x08b4,
		/// <summary></summary>
		Toprightsummation = 0x08b5,
		/// <summary></summary>
		Botrightsummation = 0x08b6,
		/// <summary></summary>
		Rightmiddlesummation = 0x08b7,
		/// <summary>U+2264 LESS-THAN OR EQUAL TO</summary>
		Lessthanequal = 0x08bc,
		/// <summary>U+2260 NOT EQUAL TO</summary>
		Notequal = 0x08bd,
		/// <summary>U+2265 GREATER-THAN OR EQUAL TO</summary>
		Greaterthanequal = 0x08be,
		/// <summary>U+222B INTEGRAL</summary>
		Integral = 0x08bf,
		/// <summary>U+2234 THEREFORE</summary>
		Therefore = 0x08c0,
		/// <summary>U+221D PROPORTIONAL TO</summary>
		Variation = 0x08c1,
		/// <summary>U+221E INFINITY</summary>
		Infinity = 0x08c2,
		/// <summary>U+2207 NABLA</summary>
		Nabla = 0x08c5,
		/// <summary>U+223C TILDE OPERATOR</summary>
		Approximate = 0x08c8,
		/// <summary>U+2243 ASYMPTOTICALLY EQUAL TO</summary>
		Similarequal = 0x08c9,
		/// <summary>U+21D4 LEFT RIGHT DOUBLE ARROW</summary>
		Ifonlyif = 0x08cd,
		/// <summary>U+21D2 RIGHTWARDS DOUBLE ARROW</summary>
		Implies = 0x08ce,
		/// <summary>U+2261 IDENTICAL TO</summary>
		Identical = 0x08cf,
		/// <summary>U+221A SQUARE ROOT</summary>
		Radical = 0x08d6,
		/// <summary>U+2282 SUBSET OF</summary>
		Includedin = 0x08da,
		/// <summary>U+2283 SUPERSET OF</summary>
		Includes = 0x08db,
		/// <summary>U+2229 INTERSECTION</summary>
		Intersection = 0x08dc,
		/// <summary>U+222A UNION</summary>
		Union = 0x08dd,
		/// <summary>U+2227 LOGICAL AND</summary>
		Logicaland = 0x08de,
		/// <summary>U+2228 LOGICAL OR</summary>
		Logicalor = 0x08df,
		/// <summary>U+2202 PARTIAL DIFFERENTIAL</summary>
		Partialderivative = 0x08ef,
		/// <summary>U+0192 LATIN SMALL LETTER F WITH HOOK</summary>
		function = 0x08f6,
		/// <summary>U+2190 LEFTWARDS ARROW</summary>
		Leftarrow = 0x08fb,
		/// <summary>U+2191 UPWARDS ARROW</summary>
		Uparrow = 0x08fc,
		/// <summary>U+2192 RIGHTWARDS ARROW</summary>
		Rightarrow = 0x08fd,
		/// <summary>U+2193 DOWNWARDS ARROW</summary>
		Downarrow = 0x08fe,
		// #endif /* XK_TECHNICAL */

		/*
		* Special
		* (from the DEC VT100 Special Graphics Character Set)
		* Byte 3 = 9
		*/

		// #ifdef XK_SPECIAL
		/// <summary></summary>
		Blank = 0x09df,
		/// <summary>U+25C6 BLACK DIAMOND</summary>
		Soliddiamond = 0x09e0,
		/// <summary>U+2592 MEDIUM SHADE</summary>
		Checkerboard = 0x09e1,
		/// <summary>U+2409 SYMBOL FOR HORIZONTAL TABULATION</summary>
		Ht = 0x09e2,
		/// <summary>U+240C SYMBOL FOR FORM FEED</summary>
		Ff = 0x09e3,
		/// <summary>U+240D SYMBOL FOR CARRIAGE RETURN</summary>
		Cr = 0x09e4,
		/// <summary>U+240A SYMBOL FOR LINE FEED</summary>
		Lf = 0x09e5,
		/// <summary>U+2424 SYMBOL FOR NEWLINE</summary>
		Nl = 0x09e8,
		/// <summary>U+240B SYMBOL FOR VERTICAL TABULATION</summary>
		Vt = 0x09e9,
		/// <summary>U+2518 BOX DRAWINGS LIGHT UP AND LEFT</summary>
		Lowrightcorner = 0x09ea,
		/// <summary>U+2510 BOX DRAWINGS LIGHT DOWN AND LEFT</summary>
		Uprightcorner = 0x09eb,
		/// <summary>U+250C BOX DRAWINGS LIGHT DOWN AND RIGHT</summary>
		Upleftcorner = 0x09ec,
		/// <summary>U+2514 BOX DRAWINGS LIGHT UP AND RIGHT</summary>
		Lowleftcorner = 0x09ed,
		/// <summary>U+253C BOX DRAWINGS LIGHT VERTICAL AND HORIZONTAL</summary>
		Crossinglines = 0x09ee,
		/// <summary>U+23BA HORIZONTAL SCAN LINE-1</summary>
		Horizlinescan1 = 0x09ef,
		/// <summary>U+23BB HORIZONTAL SCAN LINE-3</summary>
		Horizlinescan3 = 0x09f0,
		/// <summary>U+2500 BOX DRAWINGS LIGHT HORIZONTAL</summary>
		Horizlinescan5 = 0x09f1,
		/// <summary>U+23BC HORIZONTAL SCAN LINE-7</summary>
		Horizlinescan7 = 0x09f2,
		/// <summary>U+23BD HORIZONTAL SCAN LINE-9</summary>
		Horizlinescan9 = 0x09f3,
		/// <summary>U+251C BOX DRAWINGS LIGHT VERTICAL AND RIGHT</summary>
		Leftt = 0x09f4,
		/// <summary>U+2524 BOX DRAWINGS LIGHT VERTICAL AND LEFT</summary>
		Rightt = 0x09f5,
		/// <summary>U+2534 BOX DRAWINGS LIGHT UP AND HORIZONTAL</summary>
		Bott = 0x09f6,
		/// <summary>U+252C BOX DRAWINGS LIGHT DOWN AND HORIZONTAL</summary>
		Topt = 0x09f7,
		/// <summary>U+2502 BOX DRAWINGS LIGHT VERTICAL</summary>
		Vertbar = 0x09f8,
		// #endif /* XK_SPECIAL */

		/*
		* Publishing
		* (these are probably from a long forgotten DEC Publishing
		* font that once shipped with DECwrite)
		* Byte 3 = 0x0a
		*/

		// #ifdef XK_PUBLISHING
		/// <summary>U+2003 EM SPACE</summary>
		Emspace = 0x0aa1,
		/// <summary>U+2002 EN SPACE</summary>
		Enspace = 0x0aa2,
		/// <summary>U+2004 THREE-PER-EM SPACE</summary>
		Em3space = 0x0aa3,
		/// <summary>U+2005 FOUR-PER-EM SPACE</summary>
		Em4space = 0x0aa4,
		/// <summary>U+2007 FIGURE SPACE</summary>
		Digitspace = 0x0aa5,
		/// <summary>U+2008 PUNCTUATION SPACE</summary>
		Punctspace = 0x0aa6,
		/// <summary>U+2009 THIN SPACE</summary>
		Thinspace = 0x0aa7,
		/// <summary>U+200A HAIR SPACE</summary>
		Hairspace = 0x0aa8,
		/// <summary>U+2014 EM DASH</summary>
		Emdash = 0x0aa9,
		/// <summary>U+2013 EN DASH</summary>
		Endash = 0x0aaa,
		/// <summary>(U+2423 OPEN BOX)</summary>
		Signifblank = 0x0aac,
		/// <summary>U+2026 HORIZONTAL ELLIPSIS</summary>
		Ellipsis = 0x0aae,
		/// <summary>U+2025 TWO DOT LEADER</summary>
		Doubbaselinedot = 0x0aaf,
		/// <summary>U+2153 VULGAR FRACTION ONE THIRD</summary>
		Onethird = 0x0ab0,
		/// <summary>U+2154 VULGAR FRACTION TWO THIRDS</summary>
		Twothirds = 0x0ab1,
		/// <summary>U+2155 VULGAR FRACTION ONE FIFTH</summary>
		Onefifth = 0x0ab2,
		/// <summary>U+2156 VULGAR FRACTION TWO FIFTHS</summary>
		Twofifths = 0x0ab3,
		/// <summary>U+2157 VULGAR FRACTION THREE FIFTHS</summary>
		Threefifths = 0x0ab4,
		/// <summary>U+2158 VULGAR FRACTION FOUR FIFTHS</summary>
		Fourfifths = 0x0ab5,
		/// <summary>U+2159 VULGAR FRACTION ONE SIXTH</summary>
		Onesixth = 0x0ab6,
		/// <summary>U+215A VULGAR FRACTION FIVE SIXTHS</summary>
		Fivesixths = 0x0ab7,
		/// <summary>U+2105 CARE OF</summary>
		Careof = 0x0ab8,
		/// <summary>U+2012 FIGURE DASH</summary>
		Figdash = 0x0abb,
		/// <summary>(U+27E8 MATHEMATICAL LEFT ANGLE BRACKET)</summary>
		Leftanglebracket = 0x0abc,
		/// <summary>(U+002E FULL STOP)</summary>
		Decimalpoint = 0x0abd,
		/// <summary>(U+27E9 MATHEMATICAL RIGHT ANGLE BRACKET)</summary>
		Rightanglebracket = 0x0abe,
		/// <summary></summary>
		Marker = 0x0abf,
		/// <summary>U+215B VULGAR FRACTION ONE EIGHTH</summary>
		Oneeighth = 0x0ac3,
		/// <summary>U+215C VULGAR FRACTION THREE EIGHTHS</summary>
		Threeeighths = 0x0ac4,
		/// <summary>U+215D VULGAR FRACTION FIVE EIGHTHS</summary>
		Fiveeighths = 0x0ac5,
		/// <summary>U+215E VULGAR FRACTION SEVEN EIGHTHS</summary>
		Seveneighths = 0x0ac6,
		/// <summary>U+2122 TRADE MARK SIGN</summary>
		Trademark = 0x0ac9,
		/// <summary>(U+2613 SALTIRE)</summary>
		Signaturemark = 0x0aca,
		/// <summary></summary>
		Trademarkincircle = 0x0acb,
		/// <summary>(U+25C1 WHITE LEFT-POINTING TRIANGLE)</summary>
		Leftopentriangle = 0x0acc,
		/// <summary>(U+25B7 WHITE RIGHT-POINTING TRIANGLE)</summary>
		Rightopentriangle = 0x0acd,
		/// <summary>(U+25CB WHITE CIRCLE)</summary>
		Emopencircle = 0x0ace,
		/// <summary>(U+25AF WHITE VERTICAL RECTANGLE)</summary>
		Emopenrectangle = 0x0acf,
		/// <summary>U+2018 LEFT SINGLE QUOTATION MARK</summary>
		Leftsinglequotemark = 0x0ad0,
		/// <summary>U+2019 RIGHT SINGLE QUOTATION MARK</summary>
		Rightsinglequotemark = 0x0ad1,
		/// <summary>U+201C LEFT DOUBLE QUOTATION MARK</summary>
		Leftdoublequotemark = 0x0ad2,
		/// <summary>U+201D RIGHT DOUBLE QUOTATION MARK</summary>
		Rightdoublequotemark = 0x0ad3,
		/// <summary>U+211E PRESCRIPTION TAKE</summary>
		Prescription = 0x0ad4,
		/// <summary>U+2030 PER MILLE SIGN</summary>
		Permille = 0x0ad5,
		/// <summary>U+2032 PRIME</summary>
		Minutes = 0x0ad6,
		/// <summary>U+2033 DOUBLE PRIME</summary>
		Seconds = 0x0ad7,
		/// <summary>U+271D LATIN CROSS</summary>
		Latincross = 0x0ad9,
		/// <summary></summary>
		Hexagram = 0x0ada,
		/// <summary>(U+25AC BLACK RECTANGLE)</summary>
		Filledrectbullet = 0x0adb,
		/// <summary>(U+25C0 BLACK LEFT-POINTING TRIANGLE)</summary>
		Filledlefttribullet = 0x0adc,
		/// <summary>(U+25B6 BLACK RIGHT-POINTING TRIANGLE)</summary>
		Filledrighttribullet = 0x0add,
		/// <summary>(U+25CF BLACK CIRCLE)</summary>
		Emfilledcircle = 0x0ade,
		/// <summary>(U+25AE BLACK VERTICAL RECTANGLE)</summary>
		Emfilledrect = 0x0adf,
		/// <summary>(U+25E6 WHITE BULLET)</summary>
		Enopencircbullet = 0x0ae0,
		/// <summary>(U+25AB WHITE SMALL SQUARE)</summary>
		enopensquarebullet = 0x0ae1,
		/// <summary>(U+25AD WHITE RECTANGLE)</summary>
		Openrectbullet = 0x0ae2,
		/// <summary>(U+25B3 WHITE UP-POINTING TRIANGLE)</summary>
		Opentribulletup = 0x0ae3,
		/// <summary>(U+25BD WHITE DOWN-POINTING TRIANGLE)</summary>
		Opentribulletdown = 0x0ae4,
		/// <summary>(U+2606 WHITE STAR)</summary>
		Openstar = 0x0ae5,
		/// <summary>(U+2022 BULLET)</summary>
		Enfilledcircbullet = 0x0ae6,
		/// <summary>(U+25AA BLACK SMALL SQUARE)</summary>
		enfilledsqbullet = 0x0ae7,
		/// <summary>(U+25B2 BLACK UP-POINTING TRIANGLE)</summary>
		Filledtribulletup = 0x0ae8,
		/// <summary>(U+25BC BLACK DOWN-POINTING TRIANGLE)</summary>
		Filledtribulletdown = 0x0ae9,
		/// <summary>(U+261C WHITE LEFT POINTING INDEX)</summary>
		Leftpointer = 0x0aea,
		/// <summary>(U+261E WHITE RIGHT POINTING INDEX)</summary>
		Rightpointer = 0x0aeb,
		/// <summary>U+2663 BLACK CLUB SUIT</summary>
		Club = 0x0aec,
		/// <summary>U+2666 BLACK DIAMOND SUIT</summary>
		Diamond = 0x0aed,
		/// <summary>U+2665 BLACK HEART SUIT</summary>
		Heart = 0x0aee,
		/// <summary>U+2720 MALTESE CROSS</summary>
		Maltesecross = 0x0af0,
		/// <summary>U+2020 DAGGER</summary>
		Dagger = 0x0af1,
		/// <summary>U+2021 DOUBLE DAGGER</summary>
		Doubledagger = 0x0af2,
		/// <summary>U+2713 CHECK MARK</summary>
		Checkmark = 0x0af3,
		/// <summary>U+2717 BALLOT X</summary>
		Ballotcross = 0x0af4,
		/// <summary>U+266F MUSIC SHARP SIGN</summary>
		Musicalsharp = 0x0af5,
		/// <summary>U+266D MUSIC FLAT SIGN</summary>
		Musicalflat = 0x0af6,
		/// <summary>U+2642 MALE SIGN</summary>
		Malesymbol = 0x0af7,
		/// <summary>U+2640 FEMALE SIGN</summary>
		Femalesymbol = 0x0af8,
		/// <summary>U+260E BLACK TELEPHONE</summary>
		Telephone = 0x0af9,
		/// <summary>U+2315 TELEPHONE RECORDER</summary>
		Telephonerecorder = 0x0afa,
		/// <summary>U+2117 SOUND RECORDING COPYRIGHT</summary>
		Phonographcopyright = 0x0afb,
		/// <summary>U+2038 CARET</summary>
		Caret = 0x0afc,
		/// <summary>U+201A SINGLE LOW-9 QUOTATION MARK</summary>
		Singlelowquotemark = 0x0afd,
		/// <summary>U+201E DOUBLE LOW-9 QUOTATION MARK</summary>
		Doublelowquotemark = 0x0afe,
		/// <summary></summary>
		Cursor = 0x0aff,
		// #endif /* XK_PUBLISHING */

		/*
		* APL
		* Byte 3 = 0x0b
		*/

		// #ifdef XK_APL
		/// <summary>(U+003C LESS-THAN SIGN)</summary>
		Leftcaret = 0x0ba3,
		/// <summary>(U+003E GREATER-THAN SIGN)</summary>
		Rightcaret = 0x0ba6,
		/// <summary>(U+2228 LOGICAL OR)</summary>
		Downcaret = 0x0ba8,
		/// <summary>(U+2227 LOGICAL AND)</summary>
		Upcaret = 0x0ba9,
		/// <summary>(U+00AF MACRON)</summary>
		Overbar = 0x0bc0,
		/// <summary>U+22A4 DOWN TACK</summary>
		Downtack = 0x0bc2,
		/// <summary>(U+2229 INTERSECTION)</summary>
		Upshoe = 0x0bc3,
		/// <summary>U+230A LEFT FLOOR</summary>
		Downstile = 0x0bc4,
		/// <summary>(U+005F LOW LINE)</summary>
		Underbar = 0x0bc6,
		/// <summary>U+2218 RING OPERATOR</summary>
		Jot = 0x0bca,
		/// <summary>U+2395 APL FUNCTIONAL SYMBOL QUAD</summary>
		Quad = 0x0bcc,
		/// <summary>U+22A5 UP TACK</summary>
		Uptack = 0x0bce,
		/// <summary>U+25CB WHITE CIRCLE</summary>
		Circle = 0x0bcf,
		/// <summary>U+2308 LEFT CEILING</summary>
		Upstile = 0x0bd3,
		/// <summary>(U+222A UNION)</summary>
		Downshoe = 0x0bd6,
		/// <summary>(U+2283 SUPERSET OF)</summary>
		Rightshoe = 0x0bd8,
		/// <summary>(U+2282 SUBSET OF)</summary>
		Leftshoe = 0x0bda,
		/// <summary>U+22A3 LEFT TACK</summary>
		Lefttack = 0x0bdc,
		/// <summary>U+22A2 RIGHT TACK</summary>
		Righttack = 0x0bfc,
		// #endif /* XK_APL */

		/*
		* Hebrew
		* Byte 3 = 0x0c
		*/

		// #ifdef XK_HEBREW
		/// <summary>U+2017 DOUBLE LOW LINE</summary>
		HebrewDoublelowline = 0x0cdf,
		/// <summary>U+05D0 HEBREW LETTER ALEF</summary>
		HebrewAleph = 0x0ce0,
		/// <summary>U+05D1 HEBREW LETTER BET</summary>
		HebrewBet = 0x0ce1,
		/// <summary>deprecated</summary>
		HebrewBeth = 0x0ce1,
		/// <summary>U+05D2 HEBREW LETTER GIMEL</summary>
		HebrewGimel = 0x0ce2,
		/// <summary>deprecated</summary>
		HebrewGimmel = 0x0ce2,
		/// <summary>U+05D3 HEBREW LETTER DALET</summary>
		HebrewDalet = 0x0ce3,
		/// <summary>deprecated</summary>
		HebrewDaleth = 0x0ce3,
		/// <summary>U+05D4 HEBREW LETTER HE</summary>
		HebrewHe = 0x0ce4,
		/// <summary>U+05D5 HEBREW LETTER VAV</summary>
		HebrewWaw = 0x0ce5,
		/// <summary>U+05D6 HEBREW LETTER ZAYIN</summary>
		HebrewZain = 0x0ce6,
		/// <summary>deprecated</summary>
		HebrewZayin = 0x0ce6,
		/// <summary>U+05D7 HEBREW LETTER HET</summary>
		HebrewChet = 0x0ce7,
		/// <summary>deprecated</summary>
		HebrewHet = 0x0ce7,
		/// <summary>U+05D8 HEBREW LETTER TET</summary>
		HebrewTet = 0x0ce8,
		/// <summary>deprecated</summary>
		HebrewTeth = 0x0ce8,
		/// <summary>U+05D9 HEBREW LETTER YOD</summary>
		HebrewYod = 0x0ce9,
		/// <summary>U+05DA HEBREW LETTER FINAL KAF</summary>
		HebrewFinalkaph = 0x0cea,
		/// <summary>U+05DB HEBREW LETTER KAF</summary>
		HebrewKaph = 0x0ceb,
		/// <summary>U+05DC HEBREW LETTER LAMED</summary>
		HebrewLamed = 0x0cec,
		/// <summary>U+05DD HEBREW LETTER FINAL MEM</summary>
		HebrewFinalmem = 0x0ced,
		/// <summary>U+05DE HEBREW LETTER MEM</summary>
		HebrewMem = 0x0cee,
		/// <summary>U+05DF HEBREW LETTER FINAL NUN</summary>
		HebrewFinalnun = 0x0cef,
		/// <summary>U+05E0 HEBREW LETTER NUN</summary>
		HebrewNun = 0x0cf0,
		/// <summary>U+05E1 HEBREW LETTER SAMEKH</summary>
		HebrewSamech = 0x0cf1,
		/// <summary>deprecated</summary>
		HebrewSamekh = 0x0cf1,
		/// <summary>U+05E2 HEBREW LETTER AYIN</summary>
		HebrewAyin = 0x0cf2,
		/// <summary>U+05E3 HEBREW LETTER FINAL PE</summary>
		HebrewFinalpe = 0x0cf3,
		/// <summary>U+05E4 HEBREW LETTER PE</summary>
		HebrewPe = 0x0cf4,
		/// <summary>U+05E5 HEBREW LETTER FINAL TSADI</summary>
		HebrewFinalzade = 0x0cf5,
		/// <summary>deprecated</summary>
		HebrewFinalzadi = 0x0cf5,
		/// <summary>U+05E6 HEBREW LETTER TSADI</summary>
		HebrewZade = 0x0cf6,
		/// <summary>deprecated</summary>
		HebrewZadi = 0x0cf6,
		/// <summary>U+05E7 HEBREW LETTER QOF</summary>
		HebrewQoph = 0x0cf7,
		/// <summary>deprecated</summary>
		HebrewKuf = 0x0cf7,
		/// <summary>U+05E8 HEBREW LETTER RESH</summary>
		HebrewResh = 0x0cf8,
		/// <summary>U+05E9 HEBREW LETTER SHIN</summary>
		HebrewShin = 0x0cf9,
		/// <summary>U+05EA HEBREW LETTER TAV</summary>
		HebrewTaw = 0x0cfa,
		/// <summary>deprecated</summary>
		HebrewTaf = 0x0cfa,
		/// <summary>Alias for mode_switch</summary>
		HebrewSwitch = 0xff7e,
		// #endif /* XK_HEBREW */

		/*
		* Thai
		* Byte 3 = 0x0d
		*/

		// #ifdef XK_THAI
		/// <summary>U+0E01 THAI CHARACTER KO KAI</summary>
		ThaiKokai = 0x0da1,
		/// <summary>U+0E02 THAI CHARACTER KHO KHAI</summary>
		ThaiKhokhai = 0x0da2,
		/// <summary>U+0E03 THAI CHARACTER KHO KHUAT</summary>
		ThaiKhokhuat = 0x0da3,
		/// <summary>U+0E04 THAI CHARACTER KHO KHWAI</summary>
		ThaiKhokhwai = 0x0da4,
		/// <summary>U+0E05 THAI CHARACTER KHO KHON</summary>
		ThaiKhokhon = 0x0da5,
		/// <summary>U+0E06 THAI CHARACTER KHO RAKHANG</summary>
		ThaiKhorakhang = 0x0da6,
		/// <summary>U+0E07 THAI CHARACTER NGO NGU</summary>
		ThaiNgongu = 0x0da7,
		/// <summary>U+0E08 THAI CHARACTER CHO CHAN</summary>
		ThaiChochan = 0x0da8,
		/// <summary>U+0E09 THAI CHARACTER CHO CHING</summary>
		ThaiChoching = 0x0da9,
		/// <summary>U+0E0A THAI CHARACTER CHO CHANG</summary>
		ThaiChochang = 0x0daa,
		/// <summary>U+0E0B THAI CHARACTER SO SO</summary>
		ThaiSoso = 0x0dab,
		/// <summary>U+0E0C THAI CHARACTER CHO CHOE</summary>
		ThaiChochoe = 0x0dac,
		/// <summary>U+0E0D THAI CHARACTER YO YING</summary>
		ThaiYoying = 0x0dad,
		/// <summary>U+0E0E THAI CHARACTER DO CHADA</summary>
		ThaiDochada = 0x0dae,
		/// <summary>U+0E0F THAI CHARACTER TO PATAK</summary>
		ThaiTopatak = 0x0daf,
		/// <summary>U+0E10 THAI CHARACTER THO THAN</summary>
		ThaiThothan = 0x0db0,
		/// <summary>U+0E11 THAI CHARACTER THO NANGMONTHO</summary>
		ThaiThonangmontho = 0x0db1,
		/// <summary>U+0E12 THAI CHARACTER THO PHUTHAO</summary>
		ThaiThophuthao = 0x0db2,
		/// <summary>U+0E13 THAI CHARACTER NO NEN</summary>
		ThaiNonen = 0x0db3,
		/// <summary>U+0E14 THAI CHARACTER DO DEK</summary>
		ThaiDodek = 0x0db4,
		/// <summary>U+0E15 THAI CHARACTER TO TAO</summary>
		ThaiTotao = 0x0db5,
		/// <summary>U+0E16 THAI CHARACTER THO THUNG</summary>
		ThaiThothung = 0x0db6,
		/// <summary>U+0E17 THAI CHARACTER THO THAHAN</summary>
		ThaiThothahan = 0x0db7,
		/// <summary>U+0E18 THAI CHARACTER THO THONG</summary>
		ThaiThothong = 0x0db8,
		/// <summary>U+0E19 THAI CHARACTER NO NU</summary>
		ThaiNonu = 0x0db9,
		/// <summary>U+0E1A THAI CHARACTER BO BAIMAI</summary>
		ThaiBobaimai = 0x0dba,
		/// <summary>U+0E1B THAI CHARACTER PO PLA</summary>
		ThaiPopla = 0x0dbb,
		/// <summary>U+0E1C THAI CHARACTER PHO PHUNG</summary>
		ThaiPhophung = 0x0dbc,
		/// <summary>U+0E1D THAI CHARACTER FO FA</summary>
		ThaiFofa = 0x0dbd,
		/// <summary>U+0E1E THAI CHARACTER PHO PHAN</summary>
		ThaiPhophan = 0x0dbe,
		/// <summary>U+0E1F THAI CHARACTER FO FAN</summary>
		ThaiFofan = 0x0dbf,
		/// <summary>U+0E20 THAI CHARACTER PHO SAMPHAO</summary>
		ThaiPhosamphao = 0x0dc0,
		/// <summary>U+0E21 THAI CHARACTER MO MA</summary>
		ThaiMoma = 0x0dc1,
		/// <summary>U+0E22 THAI CHARACTER YO YAK</summary>
		ThaiYoyak = 0x0dc2,
		/// <summary>U+0E23 THAI CHARACTER RO RUA</summary>
		ThaiRorua = 0x0dc3,
		/// <summary>U+0E24 THAI CHARACTER RU</summary>
		ThaiRu = 0x0dc4,
		/// <summary>U+0E25 THAI CHARACTER LO LING</summary>
		ThaiLoling = 0x0dc5,
		/// <summary>U+0E26 THAI CHARACTER LU</summary>
		ThaiLu = 0x0dc6,
		/// <summary>U+0E27 THAI CHARACTER WO WAEN</summary>
		ThaiWowaen = 0x0dc7,
		/// <summary>U+0E28 THAI CHARACTER SO SALA</summary>
		ThaiSosala = 0x0dc8,
		/// <summary>U+0E29 THAI CHARACTER SO RUSI</summary>
		ThaiSorusi = 0x0dc9,
		/// <summary>U+0E2A THAI CHARACTER SO SUA</summary>
		ThaiSosua = 0x0dca,
		/// <summary>U+0E2B THAI CHARACTER HO HIP</summary>
		ThaiHohip = 0x0dcb,
		/// <summary>U+0E2C THAI CHARACTER LO CHULA</summary>
		ThaiLochula = 0x0dcc,
		/// <summary>U+0E2D THAI CHARACTER O ANG</summary>
		ThaiOang = 0x0dcd,
		/// <summary>U+0E2E THAI CHARACTER HO NOKHUK</summary>
		ThaiHonokhuk = 0x0dce,
		/// <summary>U+0E2F THAI CHARACTER PAIYANNOI</summary>
		ThaiPaiyannoi = 0x0dcf,
		/// <summary>U+0E30 THAI CHARACTER SARA A</summary>
		ThaiSaraa = 0x0dd0,
		/// <summary>U+0E31 THAI CHARACTER MAI HAN-AKAT</summary>
		ThaiMaihanakat = 0x0dd1,
		/// <summary>U+0E32 THAI CHARACTER SARA AA</summary>
		ThaiSaraaa = 0x0dd2,
		/// <summary>U+0E33 THAI CHARACTER SARA AM</summary>
		ThaiSaraam = 0x0dd3,
		/// <summary>U+0E34 THAI CHARACTER SARA I</summary>
		ThaiSarai = 0x0dd4,
		/// <summary>U+0E35 THAI CHARACTER SARA II</summary>
		ThaiSaraii = 0x0dd5,
		/// <summary>U+0E36 THAI CHARACTER SARA UE</summary>
		ThaiSaraue = 0x0dd6,
		/// <summary>U+0E37 THAI CHARACTER SARA UEE</summary>
		ThaiSarauee = 0x0dd7,
		/// <summary>U+0E38 THAI CHARACTER SARA U</summary>
		ThaiSarau = 0x0dd8,
		/// <summary>U+0E39 THAI CHARACTER SARA UU</summary>
		ThaiSarauu = 0x0dd9,
		/// <summary>U+0E3A THAI CHARACTER PHINTHU</summary>
		ThaiPhinthu = 0x0dda,
		/// <summary></summary>
		ThaiMaihanakatMaitho = 0x0dde,
		/// <summary>U+0E3F THAI CURRENCY SYMBOL BAHT</summary>
		ThaiBaht = 0x0ddf,
		/// <summary>U+0E40 THAI CHARACTER SARA E</summary>
		ThaiSarae = 0x0de0,
		/// <summary>U+0E41 THAI CHARACTER SARA AE</summary>
		ThaiSaraae = 0x0de1,
		/// <summary>U+0E42 THAI CHARACTER SARA O</summary>
		ThaiSarao = 0x0de2,
		/// <summary>U+0E43 THAI CHARACTER SARA AI MAIMUAN</summary>
		ThaiSaraaimaimuan = 0x0de3,
		/// <summary>U+0E44 THAI CHARACTER SARA AI MAIMALAI</summary>
		ThaiSaraaimaimalai = 0x0de4,
		/// <summary>U+0E45 THAI CHARACTER LAKKHANGYAO</summary>
		ThaiLakkhangyao = 0x0de5,
		/// <summary>U+0E46 THAI CHARACTER MAIYAMOK</summary>
		ThaiMaiyamok = 0x0de6,
		/// <summary>U+0E47 THAI CHARACTER MAITAIKHU</summary>
		ThaiMaitaikhu = 0x0de7,
		/// <summary>U+0E48 THAI CHARACTER MAI EK</summary>
		ThaiMaiek = 0x0de8,
		/// <summary>U+0E49 THAI CHARACTER MAI THO</summary>
		ThaiMaitho = 0x0de9,
		/// <summary>U+0E4A THAI CHARACTER MAI TRI</summary>
		ThaiMaitri = 0x0dea,
		/// <summary>U+0E4B THAI CHARACTER MAI CHATTAWA</summary>
		ThaiMaichattawa = 0x0deb,
		/// <summary>U+0E4C THAI CHARACTER THANTHAKHAT</summary>
		ThaiThanthakhat = 0x0dec,
		/// <summary>U+0E4D THAI CHARACTER NIKHAHIT</summary>
		ThaiNikhahit = 0x0ded,
		/// <summary>U+0E50 THAI DIGIT ZERO</summary>
		ThaiLeksun = 0x0df0,
		/// <summary>U+0E51 THAI DIGIT ONE</summary>
		ThaiLeknung = 0x0df1,
		/// <summary>U+0E52 THAI DIGIT TWO</summary>
		ThaiLeksong = 0x0df2,
		/// <summary>U+0E53 THAI DIGIT THREE</summary>
		ThaiLeksam = 0x0df3,
		/// <summary>U+0E54 THAI DIGIT FOUR</summary>
		ThaiLeksi = 0x0df4,
		/// <summary>U+0E55 THAI DIGIT FIVE</summary>
		ThaiLekha = 0x0df5,
		/// <summary>U+0E56 THAI DIGIT SIX</summary>
		ThaiLekhok = 0x0df6,
		/// <summary>U+0E57 THAI DIGIT SEVEN</summary>
		ThaiLekchet = 0x0df7,
		/// <summary>U+0E58 THAI DIGIT EIGHT</summary>
		ThaiLekpaet = 0x0df8,
		/// <summary>U+0E59 THAI DIGIT NINE</summary>
		ThaiLekkao = 0x0df9,
		// #endif /* XK_THAI */

		/*
		* Korean
		* Byte 3 = 0x0e
		*/

		// #ifdef XK_KOREAN

		/// <summary>Hangul start/stop(toggle)</summary>
		Hangul = 0xff31,
		/// <summary>Hangul start</summary>
		HangulStart = 0xff32,
		/// <summary>Hangul end, English start</summary>
		HangulEnd = 0xff33,
		/// <summary>Start Hangul->Hanja Conversion</summary>
		HangulHanja = 0xff34,
		/// <summary>Hangul Jamo mode</summary>
		HangulJamo = 0xff35,
		/// <summary>Hangul Romaja mode</summary>
		HangulRomaja = 0xff36,
		/// <summary>Hangul code input mode</summary>
		HangulCodeinput = 0xff37,
		/// <summary>Jeonja mode</summary>
		HangulJeonja = 0xff38,
		/// <summary>Banja mode</summary>
		HangulBanja = 0xff39,
		/// <summary>Pre Hanja conversion</summary>
		HangulPreHanja = 0xff3a,
		/// <summary>Post Hanja conversion</summary>
		HangulPostHanja = 0xff3b,
		/// <summary>Single candidate</summary>
		HangulSingleCandidate = 0xff3c,
		/// <summary>Multiple candidate</summary>
		HangulMultipleCandidate = 0xff3d,
		/// <summary>Previous candidate</summary>
		HangulPreviousCandidate = 0xff3e,
		/// <summary>Special symbols</summary>
		HangulSpecial = 0xff3f,
		/// <summary>Alias for mode_switch</summary>
		HangulSwitch = 0xff7e,

		/* Hangul Consonant Characters */
		/// <summary></summary>
		HangulKiyeog = 0x0ea1,
		/// <summary></summary>
		HangulSsangKiyeog = 0x0ea2,
		/// <summary></summary>
		HangulKiyeogSios = 0x0ea3,
		/// <summary></summary>
		HangulNieun = 0x0ea4,
		/// <summary></summary>
		HangulNieunJieuj = 0x0ea5,
		/// <summary></summary>
		HangulNieunHieuh = 0x0ea6,
		/// <summary></summary>
		HangulDikeud = 0x0ea7,
		/// <summary></summary>
		HangulSsangDikeud = 0x0ea8,
		/// <summary></summary>
		HangulRieul = 0x0ea9,
		/// <summary></summary>
		HangulRieulKiyeog = 0x0eaa,
		/// <summary></summary>
		HangulRieulMieum = 0x0eab,
		/// <summary></summary>
		HangulRieulPieub = 0x0eac,
		/// <summary></summary>
		HangulRieulSios = 0x0ead,
		/// <summary></summary>
		HangulRieulTieut = 0x0eae,
		/// <summary></summary>
		HangulRieulPhieuf = 0x0eaf,
		/// <summary></summary>
		HangulRieulHieuh = 0x0eb0,
		/// <summary></summary>
		HangulMieum = 0x0eb1,
		/// <summary></summary>
		HangulPieub = 0x0eb2,
		/// <summary></summary>
		HangulSsangPieub = 0x0eb3,
		/// <summary></summary>
		HangulPieubSios = 0x0eb4,
		/// <summary></summary>
		HangulSios = 0x0eb5,
		/// <summary></summary>
		HangulSsangSios = 0x0eb6,
		/// <summary></summary>
		HangulIeung = 0x0eb7,
		/// <summary></summary>
		HangulJieuj = 0x0eb8,
		/// <summary></summary>
		HangulSsangJieuj = 0x0eb9,
		/// <summary></summary>
		HangulCieuc = 0x0eba,
		/// <summary></summary>
		HangulKhieuq = 0x0ebb,
		/// <summary></summary>
		HangulTieut = 0x0ebc,
		/// <summary></summary>
		HangulPhieuf = 0x0ebd,
		/// <summary></summary>
		HangulHieuh = 0x0ebe,

		/* Hangul Vowel Characters */
		/// <summary></summary>
		HangulA = 0x0ebf,
		/// <summary></summary>
		HangulAE = 0x0ec0,
		/// <summary></summary>
		HangulYA = 0x0ec1,
		/// <summary></summary>
		HangulYAE = 0x0ec2,
		/// <summary></summary>
		HangulEO = 0x0ec3,
		/// <summary></summary>
		HangulE = 0x0ec4,
		/// <summary></summary>
		HangulYEO = 0x0ec5,
		/// <summary></summary>
		HangulYE = 0x0ec6,
		/// <summary></summary>
		HangulO = 0x0ec7,
		/// <summary></summary>
		HangulWA = 0x0ec8,
		/// <summary></summary>
		HangulWAE = 0x0ec9,
		/// <summary></summary>
		HangulOE = 0x0eca,
		/// <summary></summary>
		HangulYO = 0x0ecb,
		/// <summary></summary>
		HangulU = 0x0ecc,
		/// <summary></summary>
		HangulWEO = 0x0ecd,
		/// <summary></summary>
		HangulWE = 0x0ece,
		/// <summary></summary>
		HangulWI = 0x0ecf,
		/// <summary></summary>
		HangulYU = 0x0ed0,
		/// <summary></summary>
		HangulEU = 0x0ed1,
		/// <summary></summary>
		HangulYI = 0x0ed2,
		/// <summary></summary>
		HangulI = 0x0ed3,

		/* Hangul syllable-final (JongSeong) Characters */
		/// <summary></summary>
		HangulJKiyeog = 0x0ed4,
		/// <summary></summary>
		HangulJSsangKiyeog = 0x0ed5,
		/// <summary></summary>
		HangulJKiyeogSios = 0x0ed6,
		/// <summary></summary>
		HangulJNieun = 0x0ed7,
		/// <summary></summary>
		HangulJNieunJieuj = 0x0ed8,
		/// <summary></summary>
		HangulJNieunHieuh = 0x0ed9,
		/// <summary></summary>
		HangulJDikeud = 0x0eda,
		/// <summary></summary>
		HangulJRieul = 0x0edb,
		/// <summary></summary>
		HangulJRieulKiyeog = 0x0edc,
		/// <summary></summary>
		HangulJRieulMieum = 0x0edd,
		/// <summary></summary>
		HangulJRieulPieub = 0x0ede,
		/// <summary></summary>
		HangulJRieulSios = 0x0edf,
		/// <summary></summary>
		HangulJRieulTieut = 0x0ee0,
		/// <summary></summary>
		HangulJRieulPhieuf = 0x0ee1,
		/// <summary></summary>
		HangulJRieulHieuh = 0x0ee2,
		/// <summary></summary>
		HangulJMieum = 0x0ee3,
		/// <summary></summary>
		HangulJPieub = 0x0ee4,
		/// <summary></summary>
		HangulJPieubSios = 0x0ee5,
		/// <summary></summary>
		HangulJSios = 0x0ee6,
		/// <summary></summary>
		HangulJSsangSios = 0x0ee7,
		/// <summary></summary>
		HangulJIeung = 0x0ee8,
		/// <summary></summary>
		HangulJJieuj = 0x0ee9,
		/// <summary></summary>
		HangulJCieuc = 0x0eea,
		/// <summary></summary>
		HangulJKhieuq = 0x0eeb,
		/// <summary></summary>
		HangulJTieut = 0x0eec,
		/// <summary></summary>
		HangulJPhieuf = 0x0eed,
		/// <summary></summary>
		HangulJHieuh = 0x0eee,

		/* Ancient Hangul Consonant Characters */
		/// <summary></summary>
		HangulRieulYeorinHieuh = 0x0eef,
		/// <summary></summary>
		HangulSunkyeongeumMieum = 0x0ef0,
		/// <summary></summary>
		HangulSunkyeongeumPieub = 0x0ef1,
		/// <summary></summary>
		HangulPanSios = 0x0ef2,
		/// <summary></summary>
		HangulKkogjiDalrinIeung = 0x0ef3,
		/// <summary></summary>
		HangulSunkyeongeumPhieuf = 0x0ef4,
		/// <summary></summary>
		HangulYeorinHieuh = 0x0ef5,

		/* Ancient Hangul Vowel Characters */
		/// <summary></summary>
		HangulAraeA = 0x0ef6,
		/// <summary></summary>
		HangulAraeAE = 0x0ef7,

		/* Ancient Hangul syllable-final (JongSeong) Characters */
		/// <summary></summary>
		HangulJPanSios = 0x0ef8,
		/// <summary></summary>
		HangulJKkogjiDalrinIeung = 0x0ef9,
		/// <summary></summary>
		HangulJYeorinHieuh = 0x0efa,

		/* Korean currency symbol */
		/// <summary>(U+20A9 WON SIGN)</summary>
		KoreanWon = 0x0eff,

		// #endif /* XK_KOREAN */

		/*
		* Armenian
		*/

		// #ifdef XK_ARMENIAN
		/// <summary>U+0587 ARMENIAN SMALL LIGATURE ECH YIWN</summary>
		armenianLigatureEw = 0x1000587,
		/// <summary>U+0589 ARMENIAN FULL STOP</summary>
		ArmenianFullStop = 0x1000589,
		/// <summary>U+0589 ARMENIAN FULL STOP</summary>
		ArmenianVerjaket = 0x1000589,
		/// <summary>U+055D ARMENIAN COMMA</summary>
		ArmenianSeparationMark = 0x100055d,
		/// <summary>U+055D ARMENIAN COMMA</summary>
		ArmenianBut = 0x100055d,
		/// <summary>U+058A ARMENIAN HYPHEN</summary>
		ArmenianHyphen = 0x100058a,
		/// <summary>U+058A ARMENIAN HYPHEN</summary>
		ArmenianYentamna = 0x100058a,
		/// <summary>U+055C ARMENIAN EXCLAMATION MARK</summary>
		ArmenianExclam = 0x100055c,
		/// <summary>U+055C ARMENIAN EXCLAMATION MARK</summary>
		ArmenianAmanak = 0x100055c,
		/// <summary>U+055B ARMENIAN EMPHASIS MARK</summary>
		ArmenianAccent = 0x100055b,
		/// <summary>U+055B ARMENIAN EMPHASIS MARK</summary>
		ArmenianShesht = 0x100055b,
		/// <summary>U+055E ARMENIAN QUESTION MARK</summary>
		ArmenianQuestion = 0x100055e,
		/// <summary>U+055E ARMENIAN QUESTION MARK</summary>
		ArmenianParuyk = 0x100055e,
		/// <summary>U+0531 ARMENIAN CAPITAL LETTER AYB</summary>
		ArmenianAYB = 0x1000531,
		/// <summary>U+0561 ARMENIAN SMALL LETTER AYB</summary>
		armenianAyb = 0x1000561,
		/// <summary>U+0532 ARMENIAN CAPITAL LETTER BEN</summary>
		ArmenianBEN = 0x1000532,
		/// <summary>U+0562 ARMENIAN SMALL LETTER BEN</summary>
		armenianBen = 0x1000562,
		/// <summary>U+0533 ARMENIAN CAPITAL LETTER GIM</summary>
		ArmenianGIM = 0x1000533,
		/// <summary>U+0563 ARMENIAN SMALL LETTER GIM</summary>
		armenianGim = 0x1000563,
		/// <summary>U+0534 ARMENIAN CAPITAL LETTER DA</summary>
		ArmenianDA = 0x1000534,
		/// <summary>U+0564 ARMENIAN SMALL LETTER DA</summary>
		armenianDa = 0x1000564,
		/// <summary>U+0535 ARMENIAN CAPITAL LETTER ECH</summary>
		ArmenianYECH = 0x1000535,
		/// <summary>U+0565 ARMENIAN SMALL LETTER ECH</summary>
		armenianYech = 0x1000565,
		/// <summary>U+0536 ARMENIAN CAPITAL LETTER ZA</summary>
		ArmenianZA = 0x1000536,
		/// <summary>U+0566 ARMENIAN SMALL LETTER ZA</summary>
		armenianZa = 0x1000566,
		/// <summary>U+0537 ARMENIAN CAPITAL LETTER EH</summary>
		ArmenianE = 0x1000537,
		/// <summary>U+0567 ARMENIAN SMALL LETTER EH</summary>
		armenianE = 0x1000567,
		/// <summary>U+0538 ARMENIAN CAPITAL LETTER ET</summary>
		ArmenianAT = 0x1000538,
		/// <summary>U+0568 ARMENIAN SMALL LETTER ET</summary>
		armenianAt = 0x1000568,
		/// <summary>U+0539 ARMENIAN CAPITAL LETTER TO</summary>
		ArmenianTO = 0x1000539,
		/// <summary>U+0569 ARMENIAN SMALL LETTER TO</summary>
		armenianTo = 0x1000569,
		/// <summary>U+053A ARMENIAN CAPITAL LETTER ZHE</summary>
		ArmenianZHE = 0x100053a,
		/// <summary>U+056A ARMENIAN SMALL LETTER ZHE</summary>
		armenianZhe = 0x100056a,
		/// <summary>U+053B ARMENIAN CAPITAL LETTER INI</summary>
		ArmenianINI = 0x100053b,
		/// <summary>U+056B ARMENIAN SMALL LETTER INI</summary>
		armenianIni = 0x100056b,
		/// <summary>U+053C ARMENIAN CAPITAL LETTER LIWN</summary>
		ArmenianLYUN = 0x100053c,
		/// <summary>U+056C ARMENIAN SMALL LETTER LIWN</summary>
		armenianLyun = 0x100056c,
		/// <summary>U+053D ARMENIAN CAPITAL LETTER XEH</summary>
		ArmenianKHE = 0x100053d,
		/// <summary>U+056D ARMENIAN SMALL LETTER XEH</summary>
		armenianKhe = 0x100056d,
		/// <summary>U+053E ARMENIAN CAPITAL LETTER CA</summary>
		ArmenianTSA = 0x100053e,
		/// <summary>U+056E ARMENIAN SMALL LETTER CA</summary>
		armenianTsa = 0x100056e,
		/// <summary>U+053F ARMENIAN CAPITAL LETTER KEN</summary>
		ArmenianKEN = 0x100053f,
		/// <summary>U+056F ARMENIAN SMALL LETTER KEN</summary>
		armenianKen = 0x100056f,
		/// <summary>U+0540 ARMENIAN CAPITAL LETTER HO</summary>
		ArmenianHO = 0x1000540,
		/// <summary>U+0570 ARMENIAN SMALL LETTER HO</summary>
		armenianHo = 0x1000570,
		/// <summary>U+0541 ARMENIAN CAPITAL LETTER JA</summary>
		ArmenianDZA = 0x1000541,
		/// <summary>U+0571 ARMENIAN SMALL LETTER JA</summary>
		armenianDza = 0x1000571,
		/// <summary>U+0542 ARMENIAN CAPITAL LETTER GHAD</summary>
		ArmenianGHAT = 0x1000542,
		/// <summary>U+0572 ARMENIAN SMALL LETTER GHAD</summary>
		armenianGhat = 0x1000572,
		/// <summary>U+0543 ARMENIAN CAPITAL LETTER CHEH</summary>
		ArmenianTCHE = 0x1000543,
		/// <summary>U+0573 ARMENIAN SMALL LETTER CHEH</summary>
		armenianTche = 0x1000573,
		/// <summary>U+0544 ARMENIAN CAPITAL LETTER MEN</summary>
		ArmenianMEN = 0x1000544,
		/// <summary>U+0574 ARMENIAN SMALL LETTER MEN</summary>
		armenianMen = 0x1000574,
		/// <summary>U+0545 ARMENIAN CAPITAL LETTER YI</summary>
		ArmenianHI = 0x1000545,
		/// <summary>U+0575 ARMENIAN SMALL LETTER YI</summary>
		armenianHi = 0x1000575,
		/// <summary>U+0546 ARMENIAN CAPITAL LETTER NOW</summary>
		ArmenianNU = 0x1000546,
		/// <summary>U+0576 ARMENIAN SMALL LETTER NOW</summary>
		armenianNu = 0x1000576,
		/// <summary>U+0547 ARMENIAN CAPITAL LETTER SHA</summary>
		ArmenianSHA = 0x1000547,
		/// <summary>U+0577 ARMENIAN SMALL LETTER SHA</summary>
		armenianSha = 0x1000577,
		/// <summary>U+0548 ARMENIAN CAPITAL LETTER VO</summary>
		ArmenianVO = 0x1000548,
		/// <summary>U+0578 ARMENIAN SMALL LETTER VO</summary>
		armenianVo = 0x1000578,
		/// <summary>U+0549 ARMENIAN CAPITAL LETTER CHA</summary>
		ArmenianCHA = 0x1000549,
		/// <summary>U+0579 ARMENIAN SMALL LETTER CHA</summary>
		armenianCha = 0x1000579,
		/// <summary>U+054A ARMENIAN CAPITAL LETTER PEH</summary>
		ArmenianPE = 0x100054a,
		/// <summary>U+057A ARMENIAN SMALL LETTER PEH</summary>
		armenianPe = 0x100057a,
		/// <summary>U+054B ARMENIAN CAPITAL LETTER JHEH</summary>
		ArmenianJE = 0x100054b,
		/// <summary>U+057B ARMENIAN SMALL LETTER JHEH</summary>
		armenianJe = 0x100057b,
		/// <summary>U+054C ARMENIAN CAPITAL LETTER RA</summary>
		ArmenianRA = 0x100054c,
		/// <summary>U+057C ARMENIAN SMALL LETTER RA</summary>
		armenianRa = 0x100057c,
		/// <summary>U+054D ARMENIAN CAPITAL LETTER SEH</summary>
		ArmenianSE = 0x100054d,
		/// <summary>U+057D ARMENIAN SMALL LETTER SEH</summary>
		armenianSe = 0x100057d,
		/// <summary>U+054E ARMENIAN CAPITAL LETTER VEW</summary>
		ArmenianVEV = 0x100054e,
		/// <summary>U+057E ARMENIAN SMALL LETTER VEW</summary>
		armenianVev = 0x100057e,
		/// <summary>U+054F ARMENIAN CAPITAL LETTER TIWN</summary>
		ArmenianTYUN = 0x100054f,
		/// <summary>U+057F ARMENIAN SMALL LETTER TIWN</summary>
		armenianTyun = 0x100057f,
		/// <summary>U+0550 ARMENIAN CAPITAL LETTER REH</summary>
		ArmenianRE = 0x1000550,
		/// <summary>U+0580 ARMENIAN SMALL LETTER REH</summary>
		armenianRe = 0x1000580,
		/// <summary>U+0551 ARMENIAN CAPITAL LETTER CO</summary>
		ArmenianTSO = 0x1000551,
		/// <summary>U+0581 ARMENIAN SMALL LETTER CO</summary>
		armenianTso = 0x1000581,
		/// <summary>U+0552 ARMENIAN CAPITAL LETTER YIWN</summary>
		ArmenianVYUN = 0x1000552,
		/// <summary>U+0582 ARMENIAN SMALL LETTER YIWN</summary>
		armenianVyun = 0x1000582,
		/// <summary>U+0553 ARMENIAN CAPITAL LETTER PIWR</summary>
		ArmenianPYUR = 0x1000553,
		/// <summary>U+0583 ARMENIAN SMALL LETTER PIWR</summary>
		armenianPyur = 0x1000583,
		/// <summary>U+0554 ARMENIAN CAPITAL LETTER KEH</summary>
		ArmenianKE = 0x1000554,
		/// <summary>U+0584 ARMENIAN SMALL LETTER KEH</summary>
		armenianKe = 0x1000584,
		/// <summary>U+0555 ARMENIAN CAPITAL LETTER OH</summary>
		ArmenianO = 0x1000555,
		/// <summary>U+0585 ARMENIAN SMALL LETTER OH</summary>
		armenianO = 0x1000585,
		/// <summary>U+0556 ARMENIAN CAPITAL LETTER FEH</summary>
		ArmenianFE = 0x1000556,
		/// <summary>U+0586 ARMENIAN SMALL LETTER FEH</summary>
		armenianFe = 0x1000586,
		/// <summary>U+055A ARMENIAN APOSTROPHE</summary>
		ArmenianApostrophe = 0x100055a,
		// #endif /* XK_ARMENIAN */

		/*
		* Georgian
		*/

		// #ifdef XK_GEORGIAN
		/// <summary>U+10D0 GEORGIAN LETTER AN</summary>
		GeorgianAn = 0x10010d0,
		/// <summary>U+10D1 GEORGIAN LETTER BAN</summary>
		GeorgianBan = 0x10010d1,
		/// <summary>U+10D2 GEORGIAN LETTER GAN</summary>
		GeorgianGan = 0x10010d2,
		/// <summary>U+10D3 GEORGIAN LETTER DON</summary>
		GeorgianDon = 0x10010d3,
		/// <summary>U+10D4 GEORGIAN LETTER EN</summary>
		GeorgianEn = 0x10010d4,
		/// <summary>U+10D5 GEORGIAN LETTER VIN</summary>
		GeorgianVin = 0x10010d5,
		/// <summary>U+10D6 GEORGIAN LETTER ZEN</summary>
		GeorgianZen = 0x10010d6,
		/// <summary>U+10D7 GEORGIAN LETTER TAN</summary>
		GeorgianTan = 0x10010d7,
		/// <summary>U+10D8 GEORGIAN LETTER IN</summary>
		GeorgianIn = 0x10010d8,
		/// <summary>U+10D9 GEORGIAN LETTER KAN</summary>
		GeorgianKan = 0x10010d9,
		/// <summary>U+10DA GEORGIAN LETTER LAS</summary>
		GeorgianLas = 0x10010da,
		/// <summary>U+10DB GEORGIAN LETTER MAN</summary>
		GeorgianMan = 0x10010db,
		/// <summary>U+10DC GEORGIAN LETTER NAR</summary>
		GeorgianNar = 0x10010dc,
		/// <summary>U+10DD GEORGIAN LETTER ON</summary>
		GeorgianOn = 0x10010dd,
		/// <summary>U+10DE GEORGIAN LETTER PAR</summary>
		GeorgianPar = 0x10010de,
		/// <summary>U+10DF GEORGIAN LETTER ZHAR</summary>
		GeorgianZhar = 0x10010df,
		/// <summary>U+10E0 GEORGIAN LETTER RAE</summary>
		GeorgianRae = 0x10010e0,
		/// <summary>U+10E1 GEORGIAN LETTER SAN</summary>
		GeorgianSan = 0x10010e1,
		/// <summary>U+10E2 GEORGIAN LETTER TAR</summary>
		GeorgianTar = 0x10010e2,
		/// <summary>U+10E3 GEORGIAN LETTER UN</summary>
		GeorgianUn = 0x10010e3,
		/// <summary>U+10E4 GEORGIAN LETTER PHAR</summary>
		GeorgianPhar = 0x10010e4,
		/// <summary>U+10E5 GEORGIAN LETTER KHAR</summary>
		GeorgianKhar = 0x10010e5,
		/// <summary>U+10E6 GEORGIAN LETTER GHAN</summary>
		GeorgianGhan = 0x10010e6,
		/// <summary>U+10E7 GEORGIAN LETTER QAR</summary>
		GeorgianQar = 0x10010e7,
		/// <summary>U+10E8 GEORGIAN LETTER SHIN</summary>
		GeorgianShin = 0x10010e8,
		/// <summary>U+10E9 GEORGIAN LETTER CHIN</summary>
		GeorgianChin = 0x10010e9,
		/// <summary>U+10EA GEORGIAN LETTER CAN</summary>
		GeorgianCan = 0x10010ea,
		/// <summary>U+10EB GEORGIAN LETTER JIL</summary>
		GeorgianJil = 0x10010eb,
		/// <summary>U+10EC GEORGIAN LETTER CIL</summary>
		GeorgianCil = 0x10010ec,
		/// <summary>U+10ED GEORGIAN LETTER CHAR</summary>
		GeorgianChar = 0x10010ed,
		/// <summary>U+10EE GEORGIAN LETTER XAN</summary>
		GeorgianXan = 0x10010ee,
		/// <summary>U+10EF GEORGIAN LETTER JHAN</summary>
		GeorgianJhan = 0x10010ef,
		/// <summary>U+10F0 GEORGIAN LETTER HAE</summary>
		GeorgianHae = 0x10010f0,
		/// <summary>U+10F1 GEORGIAN LETTER HE</summary>
		GeorgianHe = 0x10010f1,
		/// <summary>U+10F2 GEORGIAN LETTER HIE</summary>
		GeorgianHie = 0x10010f2,
		/// <summary>U+10F3 GEORGIAN LETTER WE</summary>
		GeorgianWe = 0x10010f3,
		/// <summary>U+10F4 GEORGIAN LETTER HAR</summary>
		GeorgianHar = 0x10010f4,
		/// <summary>U+10F5 GEORGIAN LETTER HOE</summary>
		GeorgianHoe = 0x10010f5,
		/// <summary>U+10F6 GEORGIAN LETTER FI</summary>
		GeorgianFi = 0x10010f6,
		// #endif /* XK_GEORGIAN */

		/*
		* Azeri (and other Turkic or Caucasian languages)
		*/

		// #ifdef XK_CAUCASUS
		/* latin */
		/// <summary>U+1E8A LATIN CAPITAL LETTER X WITH DOT ABOVE</summary>
		Xabovedot = 0x1001e8a,
		/// <summary>U+012C LATIN CAPITAL LETTER I WITH BREVE</summary>
		Ibreve = 0x100012c,
		/// <summary>U+01B5 LATIN CAPITAL LETTER Z WITH STROKE</summary>
		Zstroke = 0x10001b5,
		/// <summary>U+01E6 LATIN CAPITAL LETTER G WITH CARON</summary>
		Gcaron = 0x10001e6,
		/// <summary>U+01D2 LATIN CAPITAL LETTER O WITH CARON</summary>
		Ocaron = 0x10001d1,
		/// <summary>U+019F LATIN CAPITAL LETTER O WITH MIDDLE TILDE</summary>
		Obarred = 0x100019f,
		/// <summary>U+1E8B LATIN SMALL LETTER X WITH DOT ABOVE</summary>
		xabovedot = 0x1001e8b,
		/// <summary>U+012D LATIN SMALL LETTER I WITH BREVE</summary>
		ibreve = 0x100012d,
		/// <summary>U+01B6 LATIN SMALL LETTER Z WITH STROKE</summary>
		zstroke = 0x10001b6,
		/// <summary>U+01E7 LATIN SMALL LETTER G WITH CARON</summary>
		gcaron = 0x10001e7,
		/// <summary>U+01D2 LATIN SMALL LETTER O WITH CARON</summary>
		ocaron = 0x10001d2,
		/// <summary>U+0275 LATIN SMALL LETTER BARRED O</summary>
		obarred = 0x1000275,
		/// <summary>U+018F LATIN CAPITAL LETTER SCHWA</summary>
		SCHWA = 0x100018f,
		/// <summary>U+0259 LATIN SMALL LETTER SCHWA</summary>
		schwa = 0x1000259,
		/// <summary>U+01B7 LATIN CAPITAL LETTER EZH</summary>
		EZH = 0x10001b7,
		/// <summary>U+0292 LATIN SMALL LETTER EZH</summary>
		ezh = 0x1000292,
		/* those are not really Caucasus */
		/* For Inupiak */
		/// <summary>U+1E36 LATIN CAPITAL LETTER L WITH DOT BELOW</summary>
		Lbelowdot = 0x1001e36,
		/// <summary>U+1E37 LATIN SMALL LETTER L WITH DOT BELOW</summary>
		lbelowdot = 0x1001e37,
		// #endif /* XK_CAUCASUS */

		/*
		* Vietnamese
		*/

		// #ifdef XK_VIETNAMESE
		/// <summary>U+1EA0 LATIN CAPITAL LETTER A WITH DOT BELOW</summary>
		Abelowdot = 0x1001ea0,
		/// <summary>U+1EA1 LATIN SMALL LETTER A WITH DOT BELOW</summary>
		abelowdot = 0x1001ea1,
		/// <summary>U+1EA2 LATIN CAPITAL LETTER A WITH HOOK ABOVE</summary>
		Ahook = 0x1001ea2,
		/// <summary>U+1EA3 LATIN SMALL LETTER A WITH HOOK ABOVE</summary>
		ahook = 0x1001ea3,
		/// <summary>U+1EA4 LATIN CAPITAL LETTER A WITH CIRCUMFLEX AND ACUTE</summary>
		Acircumflexacute = 0x1001ea4,
		/// <summary>U+1EA5 LATIN SMALL LETTER A WITH CIRCUMFLEX AND ACUTE</summary>
		acircumflexacute = 0x1001ea5,
		/// <summary>U+1EA6 LATIN CAPITAL LETTER A WITH CIRCUMFLEX AND GRAVE</summary>
		Acircumflexgrave = 0x1001ea6,
		/// <summary>U+1EA7 LATIN SMALL LETTER A WITH CIRCUMFLEX AND GRAVE</summary>
		acircumflexgrave = 0x1001ea7,
		/// <summary>U+1EA8 LATIN CAPITAL LETTER A WITH CIRCUMFLEX AND HOOK ABOVE</summary>
		Acircumflexhook = 0x1001ea8,
		/// <summary>U+1EA9 LATIN SMALL LETTER A WITH CIRCUMFLEX AND HOOK ABOVE</summary>
		acircumflexhook = 0x1001ea9,
		/// <summary>U+1EAA LATIN CAPITAL LETTER A WITH CIRCUMFLEX AND TILDE</summary>
		Acircumflextilde = 0x1001eaa,
		/// <summary>U+1EAB LATIN SMALL LETTER A WITH CIRCUMFLEX AND TILDE</summary>
		acircumflextilde = 0x1001eab,
		/// <summary>U+1EAC LATIN CAPITAL LETTER A WITH CIRCUMFLEX AND DOT BELOW</summary>
		Acircumflexbelowdot = 0x1001eac,
		/// <summary>U+1EAD LATIN SMALL LETTER A WITH CIRCUMFLEX AND DOT BELOW</summary>
		acircumflexbelowdot = 0x1001ead,
		/// <summary>U+1EAE LATIN CAPITAL LETTER A WITH BREVE AND ACUTE</summary>
		Abreveacute = 0x1001eae,
		/// <summary>U+1EAF LATIN SMALL LETTER A WITH BREVE AND ACUTE</summary>
		abreveacute = 0x1001eaf,
		/// <summary>U+1EB0 LATIN CAPITAL LETTER A WITH BREVE AND GRAVE</summary>
		Abrevegrave = 0x1001eb0,
		/// <summary>U+1EB1 LATIN SMALL LETTER A WITH BREVE AND GRAVE</summary>
		abrevegrave = 0x1001eb1,
		/// <summary>U+1EB2 LATIN CAPITAL LETTER A WITH BREVE AND HOOK ABOVE</summary>
		Abrevehook = 0x1001eb2,
		/// <summary>U+1EB3 LATIN SMALL LETTER A WITH BREVE AND HOOK ABOVE</summary>
		abrevehook = 0x1001eb3,
		/// <summary>U+1EB4 LATIN CAPITAL LETTER A WITH BREVE AND TILDE</summary>
		Abrevetilde = 0x1001eb4,
		/// <summary>U+1EB5 LATIN SMALL LETTER A WITH BREVE AND TILDE</summary>
		abrevetilde = 0x1001eb5,
		/// <summary>U+1EB6 LATIN CAPITAL LETTER A WITH BREVE AND DOT BELOW</summary>
		Abrevebelowdot = 0x1001eb6,
		/// <summary>U+1EB7 LATIN SMALL LETTER A WITH BREVE AND DOT BELOW</summary>
		abrevebelowdot = 0x1001eb7,
		/// <summary>U+1EB8 LATIN CAPITAL LETTER E WITH DOT BELOW</summary>
		Ebelowdot = 0x1001eb8,
		/// <summary>U+1EB9 LATIN SMALL LETTER E WITH DOT BELOW</summary>
		ebelowdot = 0x1001eb9,
		/// <summary>U+1EBA LATIN CAPITAL LETTER E WITH HOOK ABOVE</summary>
		Ehook = 0x1001eba,
		/// <summary>U+1EBB LATIN SMALL LETTER E WITH HOOK ABOVE</summary>
		ehook = 0x1001ebb,
		/// <summary>U+1EBC LATIN CAPITAL LETTER E WITH TILDE</summary>
		Etilde = 0x1001ebc,
		/// <summary>U+1EBD LATIN SMALL LETTER E WITH TILDE</summary>
		etilde = 0x1001ebd,
		/// <summary>U+1EBE LATIN CAPITAL LETTER E WITH CIRCUMFLEX AND ACUTE</summary>
		Ecircumflexacute = 0x1001ebe,
		/// <summary>U+1EBF LATIN SMALL LETTER E WITH CIRCUMFLEX AND ACUTE</summary>
		ecircumflexacute = 0x1001ebf,
		/// <summary>U+1EC0 LATIN CAPITAL LETTER E WITH CIRCUMFLEX AND GRAVE</summary>
		Ecircumflexgrave = 0x1001ec0,
		/// <summary>U+1EC1 LATIN SMALL LETTER E WITH CIRCUMFLEX AND GRAVE</summary>
		ecircumflexgrave = 0x1001ec1,
		/// <summary>U+1EC2 LATIN CAPITAL LETTER E WITH CIRCUMFLEX AND HOOK ABOVE</summary>
		Ecircumflexhook = 0x1001ec2,
		/// <summary>U+1EC3 LATIN SMALL LETTER E WITH CIRCUMFLEX AND HOOK ABOVE</summary>
		ecircumflexhook = 0x1001ec3,
		/// <summary>U+1EC4 LATIN CAPITAL LETTER E WITH CIRCUMFLEX AND TILDE</summary>
		Ecircumflextilde = 0x1001ec4,
		/// <summary>U+1EC5 LATIN SMALL LETTER E WITH CIRCUMFLEX AND TILDE</summary>
		ecircumflextilde = 0x1001ec5,
		/// <summary>U+1EC6 LATIN CAPITAL LETTER E WITH CIRCUMFLEX AND DOT BELOW</summary>
		Ecircumflexbelowdot = 0x1001ec6,
		/// <summary>U+1EC7 LATIN SMALL LETTER E WITH CIRCUMFLEX AND DOT BELOW</summary>
		ecircumflexbelowdot = 0x1001ec7,
		/// <summary>U+1EC8 LATIN CAPITAL LETTER I WITH HOOK ABOVE</summary>
		Ihook = 0x1001ec8,
		/// <summary>U+1EC9 LATIN SMALL LETTER I WITH HOOK ABOVE</summary>
		ihook = 0x1001ec9,
		/// <summary>U+1ECA LATIN CAPITAL LETTER I WITH DOT BELOW</summary>
		Ibelowdot = 0x1001eca,
		/// <summary>U+1ECB LATIN SMALL LETTER I WITH DOT BELOW</summary>
		ibelowdot = 0x1001ecb,
		/// <summary>U+1ECC LATIN CAPITAL LETTER O WITH DOT BELOW</summary>
		Obelowdot = 0x1001ecc,
		/// <summary>U+1ECD LATIN SMALL LETTER O WITH DOT BELOW</summary>
		obelowdot = 0x1001ecd,
		/// <summary>U+1ECE LATIN CAPITAL LETTER O WITH HOOK ABOVE</summary>
		Ohook = 0x1001ece,
		/// <summary>U+1ECF LATIN SMALL LETTER O WITH HOOK ABOVE</summary>
		ohook = 0x1001ecf,
		/// <summary>U+1ED0 LATIN CAPITAL LETTER O WITH CIRCUMFLEX AND ACUTE</summary>
		Ocircumflexacute = 0x1001ed0,
		/// <summary>U+1ED1 LATIN SMALL LETTER O WITH CIRCUMFLEX AND ACUTE</summary>
		ocircumflexacute = 0x1001ed1,
		/// <summary>U+1ED2 LATIN CAPITAL LETTER O WITH CIRCUMFLEX AND GRAVE</summary>
		Ocircumflexgrave = 0x1001ed2,
		/// <summary>U+1ED3 LATIN SMALL LETTER O WITH CIRCUMFLEX AND GRAVE</summary>
		ocircumflexgrave = 0x1001ed3,
		/// <summary>U+1ED4 LATIN CAPITAL LETTER O WITH CIRCUMFLEX AND HOOK ABOVE</summary>
		Ocircumflexhook = 0x1001ed4,
		/// <summary>U+1ED5 LATIN SMALL LETTER O WITH CIRCUMFLEX AND HOOK ABOVE</summary>
		ocircumflexhook = 0x1001ed5,
		/// <summary>U+1ED6 LATIN CAPITAL LETTER O WITH CIRCUMFLEX AND TILDE</summary>
		Ocircumflextilde = 0x1001ed6,
		/// <summary>U+1ED7 LATIN SMALL LETTER O WITH CIRCUMFLEX AND TILDE</summary>
		ocircumflextilde = 0x1001ed7,
		/// <summary>U+1ED8 LATIN CAPITAL LETTER O WITH CIRCUMFLEX AND DOT BELOW</summary>
		Ocircumflexbelowdot = 0x1001ed8,
		/// <summary>U+1ED9 LATIN SMALL LETTER O WITH CIRCUMFLEX AND DOT BELOW</summary>
		ocircumflexbelowdot = 0x1001ed9,
		/// <summary>U+1EDA LATIN CAPITAL LETTER O WITH HORN AND ACUTE</summary>
		Ohornacute = 0x1001eda,
		/// <summary>U+1EDB LATIN SMALL LETTER O WITH HORN AND ACUTE</summary>
		ohornacute = 0x1001edb,
		/// <summary>U+1EDC LATIN CAPITAL LETTER O WITH HORN AND GRAVE</summary>
		Ohorngrave = 0x1001edc,
		/// <summary>U+1EDD LATIN SMALL LETTER O WITH HORN AND GRAVE</summary>
		ohorngrave = 0x1001edd,
		/// <summary>U+1EDE LATIN CAPITAL LETTER O WITH HORN AND HOOK ABOVE</summary>
		Ohornhook = 0x1001ede,
		/// <summary>U+1EDF LATIN SMALL LETTER O WITH HORN AND HOOK ABOVE</summary>
		ohornhook = 0x1001edf,
		/// <summary>U+1EE0 LATIN CAPITAL LETTER O WITH HORN AND TILDE</summary>
		Ohorntilde = 0x1001ee0,
		/// <summary>U+1EE1 LATIN SMALL LETTER O WITH HORN AND TILDE</summary>
		ohorntilde = 0x1001ee1,
		/// <summary>U+1EE2 LATIN CAPITAL LETTER O WITH HORN AND DOT BELOW</summary>
		Ohornbelowdot = 0x1001ee2,
		/// <summary>U+1EE3 LATIN SMALL LETTER O WITH HORN AND DOT BELOW</summary>
		ohornbelowdot = 0x1001ee3,
		/// <summary>U+1EE4 LATIN CAPITAL LETTER U WITH DOT BELOW</summary>
		Ubelowdot = 0x1001ee4,
		/// <summary>U+1EE5 LATIN SMALL LETTER U WITH DOT BELOW</summary>
		ubelowdot = 0x1001ee5,
		/// <summary>U+1EE6 LATIN CAPITAL LETTER U WITH HOOK ABOVE</summary>
		Uhook = 0x1001ee6,
		/// <summary>U+1EE7 LATIN SMALL LETTER U WITH HOOK ABOVE</summary>
		uhook = 0x1001ee7,
		/// <summary>U+1EE8 LATIN CAPITAL LETTER U WITH HORN AND ACUTE</summary>
		Uhornacute = 0x1001ee8,
		/// <summary>U+1EE9 LATIN SMALL LETTER U WITH HORN AND ACUTE</summary>
		uhornacute = 0x1001ee9,
		/// <summary>U+1EEA LATIN CAPITAL LETTER U WITH HORN AND GRAVE</summary>
		Uhorngrave = 0x1001eea,
		/// <summary>U+1EEB LATIN SMALL LETTER U WITH HORN AND GRAVE</summary>
		uhorngrave = 0x1001eeb,
		/// <summary>U+1EEC LATIN CAPITAL LETTER U WITH HORN AND HOOK ABOVE</summary>
		Uhornhook = 0x1001eec,
		/// <summary>U+1EED LATIN SMALL LETTER U WITH HORN AND HOOK ABOVE</summary>
		uhornhook = 0x1001eed,
		/// <summary>U+1EEE LATIN CAPITAL LETTER U WITH HORN AND TILDE</summary>
		Uhorntilde = 0x1001eee,
		/// <summary>U+1EEF LATIN SMALL LETTER U WITH HORN AND TILDE</summary>
		uhorntilde = 0x1001eef,
		/// <summary>U+1EF0 LATIN CAPITAL LETTER U WITH HORN AND DOT BELOW</summary>
		Uhornbelowdot = 0x1001ef0,
		/// <summary>U+1EF1 LATIN SMALL LETTER U WITH HORN AND DOT BELOW</summary>
		uhornbelowdot = 0x1001ef1,
		/// <summary>U+1EF4 LATIN CAPITAL LETTER Y WITH DOT BELOW</summary>
		Ybelowdot = 0x1001ef4,
		/// <summary>U+1EF5 LATIN SMALL LETTER Y WITH DOT BELOW</summary>
		ybelowdot = 0x1001ef5,
		/// <summary>U+1EF6 LATIN CAPITAL LETTER Y WITH HOOK ABOVE</summary>
		Yhook = 0x1001ef6,
		/// <summary>U+1EF7 LATIN SMALL LETTER Y WITH HOOK ABOVE</summary>
		yhook = 0x1001ef7,
		/// <summary>U+1EF8 LATIN CAPITAL LETTER Y WITH TILDE</summary>
		Ytilde = 0x1001ef8,
		/// <summary>U+1EF9 LATIN SMALL LETTER Y WITH TILDE</summary>
		ytilde = 0x1001ef9,
		/// <summary>U+01A0 LATIN CAPITAL LETTER O WITH HORN</summary>
		Ohorn = 0x10001a0,
		/// <summary>U+01A1 LATIN SMALL LETTER O WITH HORN</summary>
		ohorn = 0x10001a1,
		/// <summary>U+01AF LATIN CAPITAL LETTER U WITH HORN</summary>
		Uhorn = 0x10001af,
		/// <summary>U+01B0 LATIN SMALL LETTER U WITH HORN</summary>
		uhorn = 0x10001b0,

		// #endif /* XK_VIETNAMESE */

		// #ifdef XK_CURRENCY
		/// <summary>U+20A0 EURO-CURRENCY SIGN</summary>
		EcuSign = 0x10020a0,
		/// <summary>U+20A1 COLON SIGN</summary>
		ColonSign = 0x10020a1,
		/// <summary>U+20A2 CRUZEIRO SIGN</summary>
		CruzeiroSign = 0x10020a2,
		/// <summary>U+20A3 FRENCH FRANC SIGN</summary>
		FFrancSign = 0x10020a3,
		/// <summary>U+20A4 LIRA SIGN</summary>
		LiraSign = 0x10020a4,
		/// <summary>U+20A5 MILL SIGN</summary>
		MillSign = 0x10020a5,
		/// <summary>U+20A6 NAIRA SIGN</summary>
		NairaSign = 0x10020a6,
		/// <summary>U+20A7 PESETA SIGN</summary>
		PesetaSign = 0x10020a7,
		/// <summary>U+20A8 RUPEE SIGN</summary>
		RupeeSign = 0x10020a8,
		/// <summary>U+20A9 WON SIGN</summary>
		WonSign = 0x10020a9,
		/// <summary>U+20AA NEW SHEQEL SIGN</summary>
		NewSheqelSign = 0x10020aa,
		/// <summary>U+20AB DONG SIGN</summary>
		DongSign = 0x10020ab,
		/// <summary>U+20AC EURO SIGN</summary>
		EuroSign = 0x20ac,
		// #endif /* XK_CURRENCY */

		// #ifdef XK_MATHEMATICAL
		/* one, two and three are defined above. */
		/// <summary>U+2070 SUPERSCRIPT ZERO</summary>
		Zerosuperior = 0x1002070,
		/// <summary>U+2074 SUPERSCRIPT FOUR</summary>
		Foursuperior = 0x1002074,
		/// <summary>U+2075 SUPERSCRIPT FIVE</summary>
		Fivesuperior = 0x1002075,
		/// <summary>U+2076 SUPERSCRIPT SIX</summary>
		Sixsuperior = 0x1002076,
		/// <summary>U+2077 SUPERSCRIPT SEVEN</summary>
		Sevensuperior = 0x1002077,
		/// <summary>U+2078 SUPERSCRIPT EIGHT</summary>
		Eightsuperior = 0x1002078,
		/// <summary>U+2079 SUPERSCRIPT NINE</summary>
		Ninesuperior = 0x1002079,
		/// <summary>U+2080 SUBSCRIPT ZERO</summary>
		Zerosubscript = 0x1002080,
		/// <summary>U+2081 SUBSCRIPT ONE</summary>
		Onesubscript = 0x1002081,
		/// <summary>U+2082 SUBSCRIPT TWO</summary>
		Twosubscript = 0x1002082,
		/// <summary>U+2083 SUBSCRIPT THREE</summary>
		Threesubscript = 0x1002083,
		/// <summary>U+2084 SUBSCRIPT FOUR</summary>
		Foursubscript = 0x1002084,
		/// <summary>U+2085 SUBSCRIPT FIVE</summary>
		Fivesubscript = 0x1002085,
		/// <summary>U+2086 SUBSCRIPT SIX</summary>
		Sixsubscript = 0x1002086,
		/// <summary>U+2087 SUBSCRIPT SEVEN</summary>
		Sevensubscript = 0x1002087,
		/// <summary>U+2088 SUBSCRIPT EIGHT</summary>
		Eightsubscript = 0x1002088,
		/// <summary>U+2089 SUBSCRIPT NINE</summary>
		Ninesubscript = 0x1002089,
		/// <summary>U+2202 PARTIAL DIFFERENTIAL</summary>
		Partdifferential = 0x1002202,
		/// <summary>U+2205 NULL SET</summary>
		Emptyset = 0x1002205,
		/// <summary>U+2208 ELEMENT OF</summary>
		Elementof = 0x1002208,
		/// <summary>U+2209 NOT AN ELEMENT OF</summary>
		Notelementof = 0x1002209,
		/// <summary>U+220B CONTAINS AS MEMBER</summary>
		Containsas = 0x100220B,
		/// <summary>U+221A SQUARE ROOT</summary>
		Squareroot = 0x100221A,
		/// <summary>U+221B CUBE ROOT</summary>
		Cuberoot = 0x100221B,
		/// <summary>U+221C FOURTH ROOT</summary>
		Fourthroot = 0x100221C,
		/// <summary>U+222C DOUBLE INTEGRAL</summary>
		Dintegral = 0x100222C,
		/// <summary>U+222D TRIPLE INTEGRAL</summary>
		Tintegral = 0x100222D,
		/// <summary>U+2235 BECAUSE</summary>
		Because = 0x1002235,
		/// <summary>U+2245 ALMOST EQUAL TO</summary>
		Approxeq = 0x1002248,
		/// <summary>U+2247 NOT ALMOST EQUAL TO</summary>
		Notapproxeq = 0x1002247,
		/// <summary>U+2262 NOT IDENTICAL TO</summary>
		Notidentical = 0x1002262,
		/// <summary>U+2263 STRICTLY EQUIVALENT TO */</summary>
		Stricteq = 0x1002263,
		// #endif /* XK_MATHEMATICAL */

		// #ifdef XK_BRAILLE
		/// <summary></summary>
		BrailleDot1 = 0xfff1,
		/// <summary></summary>
		BrailleDot2 = 0xfff2,
		/// <summary></summary>
		BrailleDot3 = 0xfff3,
		/// <summary></summary>
		BrailleDot4 = 0xfff4,
		/// <summary></summary>
		BrailleDot5 = 0xfff5,
		/// <summary></summary>
		BrailleDot6 = 0xfff6,
		/// <summary></summary>
		BrailleDot7 = 0xfff7,
		/// <summary></summary>
		BrailleDot8 = 0xfff8,
		/// <summary></summary>
		BrailleDot9 = 0xfff9,
		/// <summary></summary>
		BrailleDot10 = 0xfffa,
		/// <summary>U+2800 BRAILLE PATTERN BLANK</summary>
		BrailleBlank = 0x1002800,
		/// <summary>U+2801 BRAILLE PATTERN DOTS-1</summary>
		BrailleDots1 = 0x1002801,
		/// <summary>U+2802 BRAILLE PATTERN DOTS-2</summary>
		BrailleDots2 = 0x1002802,
		/// <summary>U+2803 BRAILLE PATTERN DOTS-12</summary>
		BrailleDots12 = 0x1002803,
		/// <summary>U+2804 BRAILLE PATTERN DOTS-3</summary>
		BrailleDots3 = 0x1002804,
		/// <summary>U+2805 BRAILLE PATTERN DOTS-13</summary>
		BrailleDots13 = 0x1002805,
		/// <summary>U+2806 BRAILLE PATTERN DOTS-23</summary>
		BrailleDots23 = 0x1002806,
		/// <summary>U+2807 BRAILLE PATTERN DOTS-123</summary>
		BrailleDots123 = 0x1002807,
		/// <summary>U+2808 BRAILLE PATTERN DOTS-4</summary>
		BrailleDots4 = 0x1002808,
		/// <summary>U+2809 BRAILLE PATTERN DOTS-14</summary>
		BrailleDots14 = 0x1002809,
		/// <summary>U+280a BRAILLE PATTERN DOTS-24</summary>
		BrailleDots24 = 0x100280a,
		/// <summary>U+280b BRAILLE PATTERN DOTS-124</summary>
		BrailleDots124 = 0x100280b,
		/// <summary>U+280c BRAILLE PATTERN DOTS-34</summary>
		BrailleDots34 = 0x100280c,
		/// <summary>U+280d BRAILLE PATTERN DOTS-134</summary>
		BrailleDots134 = 0x100280d,
		/// <summary>U+280e BRAILLE PATTERN DOTS-234</summary>
		BrailleDots234 = 0x100280e,
		/// <summary>U+280f BRAILLE PATTERN DOTS-1234</summary>
		BrailleDots1234 = 0x100280f,
		/// <summary>U+2810 BRAILLE PATTERN DOTS-5</summary>
		BrailleDots5 = 0x1002810,
		/// <summary>U+2811 BRAILLE PATTERN DOTS-15</summary>
		BrailleDots15 = 0x1002811,
		/// <summary>U+2812 BRAILLE PATTERN DOTS-25</summary>
		BrailleDots25 = 0x1002812,
		/// <summary>U+2813 BRAILLE PATTERN DOTS-125</summary>
		BrailleDots125 = 0x1002813,
		/// <summary>U+2814 BRAILLE PATTERN DOTS-35</summary>
		BrailleDots35 = 0x1002814,
		/// <summary>U+2815 BRAILLE PATTERN DOTS-135</summary>
		BrailleDots135 = 0x1002815,
		/// <summary>U+2816 BRAILLE PATTERN DOTS-235</summary>
		BrailleDots235 = 0x1002816,
		/// <summary>U+2817 BRAILLE PATTERN DOTS-1235</summary>
		BrailleDots1235 = 0x1002817,
		/// <summary>U+2818 BRAILLE PATTERN DOTS-45</summary>
		BrailleDots45 = 0x1002818,
		/// <summary>U+2819 BRAILLE PATTERN DOTS-145</summary>
		BrailleDots145 = 0x1002819,
		/// <summary>U+281a BRAILLE PATTERN DOTS-245</summary>
		BrailleDots245 = 0x100281a,
		/// <summary>U+281b BRAILLE PATTERN DOTS-1245</summary>
		BrailleDots1245 = 0x100281b,
		/// <summary>U+281c BRAILLE PATTERN DOTS-345</summary>
		BrailleDots345 = 0x100281c,
		/// <summary>U+281d BRAILLE PATTERN DOTS-1345</summary>
		BrailleDots1345 = 0x100281d,
		/// <summary>U+281e BRAILLE PATTERN DOTS-2345</summary>
		BrailleDots2345 = 0x100281e,
		/// <summary>U+281f BRAILLE PATTERN DOTS-12345</summary>
		BrailleDots12345 = 0x100281f,
		/// <summary>U+2820 BRAILLE PATTERN DOTS-6</summary>
		BrailleDots6 = 0x1002820,
		/// <summary>U+2821 BRAILLE PATTERN DOTS-16</summary>
		BrailleDots16 = 0x1002821,
		/// <summary>U+2822 BRAILLE PATTERN DOTS-26</summary>
		BrailleDots26 = 0x1002822,
		/// <summary>U+2823 BRAILLE PATTERN DOTS-126</summary>
		BrailleDots126 = 0x1002823,
		/// <summary>U+2824 BRAILLE PATTERN DOTS-36</summary>
		BrailleDots36 = 0x1002824,
		/// <summary>U+2825 BRAILLE PATTERN DOTS-136</summary>
		BrailleDots136 = 0x1002825,
		/// <summary>U+2826 BRAILLE PATTERN DOTS-236</summary>
		BrailleDots236 = 0x1002826,
		/// <summary>U+2827 BRAILLE PATTERN DOTS-1236</summary>
		BrailleDots1236 = 0x1002827,
		/// <summary>U+2828 BRAILLE PATTERN DOTS-46</summary>
		BrailleDots46 = 0x1002828,
		/// <summary>U+2829 BRAILLE PATTERN DOTS-146</summary>
		BrailleDots146 = 0x1002829,
		/// <summary>U+282a BRAILLE PATTERN DOTS-246</summary>
		BrailleDots246 = 0x100282a,
		/// <summary>U+282b BRAILLE PATTERN DOTS-1246</summary>
		BrailleDots1246 = 0x100282b,
		/// <summary>U+282c BRAILLE PATTERN DOTS-346</summary>
		BrailleDots346 = 0x100282c,
		/// <summary>U+282d BRAILLE PATTERN DOTS-1346</summary>
		BrailleDots1346 = 0x100282d,
		/// <summary>U+282e BRAILLE PATTERN DOTS-2346</summary>
		BrailleDots2346 = 0x100282e,
		/// <summary>U+282f BRAILLE PATTERN DOTS-12346</summary>
		BrailleDots12346 = 0x100282f,
		/// <summary>U+2830 BRAILLE PATTERN DOTS-56</summary>
		BrailleDots56 = 0x1002830,
		/// <summary>U+2831 BRAILLE PATTERN DOTS-156</summary>
		BrailleDots156 = 0x1002831,
		/// <summary>U+2832 BRAILLE PATTERN DOTS-256</summary>
		BrailleDots256 = 0x1002832,
		/// <summary>U+2833 BRAILLE PATTERN DOTS-1256</summary>
		BrailleDots1256 = 0x1002833,
		/// <summary>U+2834 BRAILLE PATTERN DOTS-356</summary>
		BrailleDots356 = 0x1002834,
		/// <summary>U+2835 BRAILLE PATTERN DOTS-1356</summary>
		BrailleDots1356 = 0x1002835,
		/// <summary>U+2836 BRAILLE PATTERN DOTS-2356</summary>
		BrailleDots2356 = 0x1002836,
		/// <summary>U+2837 BRAILLE PATTERN DOTS-12356</summary>
		BrailleDots12356 = 0x1002837,
		/// <summary>U+2838 BRAILLE PATTERN DOTS-456</summary>
		BrailleDots456 = 0x1002838,
		/// <summary>U+2839 BRAILLE PATTERN DOTS-1456</summary>
		BrailleDots1456 = 0x1002839,
		/// <summary>U+283a BRAILLE PATTERN DOTS-2456</summary>
		BrailleDots2456 = 0x100283a,
		/// <summary>U+283b BRAILLE PATTERN DOTS-12456</summary>
		BrailleDots12456 = 0x100283b,
		/// <summary>U+283c BRAILLE PATTERN DOTS-3456</summary>
		BrailleDots3456 = 0x100283c,
		/// <summary>U+283d BRAILLE PATTERN DOTS-13456</summary>
		BrailleDots13456 = 0x100283d,
		/// <summary>U+283e BRAILLE PATTERN DOTS-23456</summary>
		BrailleDots23456 = 0x100283e,
		/// <summary>U+283f BRAILLE PATTERN DOTS-123456</summary>
		BrailleDots123456 = 0x100283f,
		/// <summary>U+2840 BRAILLE PATTERN DOTS-7</summary>
		BrailleDots7 = 0x1002840,
		/// <summary>U+2841 BRAILLE PATTERN DOTS-17</summary>
		BrailleDots17 = 0x1002841,
		/// <summary>U+2842 BRAILLE PATTERN DOTS-27</summary>
		BrailleDots27 = 0x1002842,
		/// <summary>U+2843 BRAILLE PATTERN DOTS-127</summary>
		BrailleDots127 = 0x1002843,
		/// <summary>U+2844 BRAILLE PATTERN DOTS-37</summary>
		BrailleDots37 = 0x1002844,
		/// <summary>U+2845 BRAILLE PATTERN DOTS-137</summary>
		BrailleDots137 = 0x1002845,
		/// <summary>U+2846 BRAILLE PATTERN DOTS-237</summary>
		BrailleDots237 = 0x1002846,
		/// <summary>U+2847 BRAILLE PATTERN DOTS-1237</summary>
		BrailleDots1237 = 0x1002847,
		/// <summary>U+2848 BRAILLE PATTERN DOTS-47</summary>
		BrailleDots47 = 0x1002848,
		/// <summary>U+2849 BRAILLE PATTERN DOTS-147</summary>
		BrailleDots147 = 0x1002849,
		/// <summary>U+284a BRAILLE PATTERN DOTS-247</summary>
		BrailleDots247 = 0x100284a,
		/// <summary>U+284b BRAILLE PATTERN DOTS-1247</summary>
		BrailleDots1247 = 0x100284b,
		/// <summary>U+284c BRAILLE PATTERN DOTS-347</summary>
		BrailleDots347 = 0x100284c,
		/// <summary>U+284d BRAILLE PATTERN DOTS-1347</summary>
		BrailleDots1347 = 0x100284d,
		/// <summary>U+284e BRAILLE PATTERN DOTS-2347</summary>
		BrailleDots2347 = 0x100284e,
		/// <summary>U+284f BRAILLE PATTERN DOTS-12347</summary>
		BrailleDots12347 = 0x100284f,
		/// <summary>U+2850 BRAILLE PATTERN DOTS-57</summary>
		BrailleDots57 = 0x1002850,
		/// <summary>U+2851 BRAILLE PATTERN DOTS-157</summary>
		BrailleDots157 = 0x1002851,
		/// <summary>U+2852 BRAILLE PATTERN DOTS-257</summary>
		BrailleDots257 = 0x1002852,
		/// <summary>U+2853 BRAILLE PATTERN DOTS-1257</summary>
		BrailleDots1257 = 0x1002853,
		/// <summary>U+2854 BRAILLE PATTERN DOTS-357</summary>
		BrailleDots357 = 0x1002854,
		/// <summary>U+2855 BRAILLE PATTERN DOTS-1357</summary>
		BrailleDots1357 = 0x1002855,
		/// <summary>U+2856 BRAILLE PATTERN DOTS-2357</summary>
		BrailleDots2357 = 0x1002856,
		/// <summary>U+2857 BRAILLE PATTERN DOTS-12357</summary>
		BrailleDots12357 = 0x1002857,
		/// <summary>U+2858 BRAILLE PATTERN DOTS-457</summary>
		BrailleDots457 = 0x1002858,
		/// <summary>U+2859 BRAILLE PATTERN DOTS-1457</summary>
		BrailleDots1457 = 0x1002859,
		/// <summary>U+285a BRAILLE PATTERN DOTS-2457</summary>
		BrailleDots2457 = 0x100285a,
		/// <summary>U+285b BRAILLE PATTERN DOTS-12457</summary>
		BrailleDots12457 = 0x100285b,
		/// <summary>U+285c BRAILLE PATTERN DOTS-3457</summary>
		BrailleDots3457 = 0x100285c,
		/// <summary>U+285d BRAILLE PATTERN DOTS-13457</summary>
		BrailleDots13457 = 0x100285d,
		/// <summary>U+285e BRAILLE PATTERN DOTS-23457</summary>
		BrailleDots23457 = 0x100285e,
		/// <summary>U+285f BRAILLE PATTERN DOTS-123457</summary>
		BrailleDots123457 = 0x100285f,
		/// <summary>U+2860 BRAILLE PATTERN DOTS-67</summary>
		BrailleDots67 = 0x1002860,
		/// <summary>U+2861 BRAILLE PATTERN DOTS-167</summary>
		BrailleDots167 = 0x1002861,
		/// <summary>U+2862 BRAILLE PATTERN DOTS-267</summary>
		BrailleDots267 = 0x1002862,
		/// <summary>U+2863 BRAILLE PATTERN DOTS-1267</summary>
		BrailleDots1267 = 0x1002863,
		/// <summary>U+2864 BRAILLE PATTERN DOTS-367</summary>
		BrailleDots367 = 0x1002864,
		/// <summary>U+2865 BRAILLE PATTERN DOTS-1367</summary>
		BrailleDots1367 = 0x1002865,
		/// <summary>U+2866 BRAILLE PATTERN DOTS-2367</summary>
		BrailleDots2367 = 0x1002866,
		/// <summary>U+2867 BRAILLE PATTERN DOTS-12367</summary>
		BrailleDots12367 = 0x1002867,
		/// <summary>U+2868 BRAILLE PATTERN DOTS-467</summary>
		BrailleDots467 = 0x1002868,
		/// <summary>U+2869 BRAILLE PATTERN DOTS-1467</summary>
		BrailleDots1467 = 0x1002869,
		/// <summary>U+286a BRAILLE PATTERN DOTS-2467</summary>
		BrailleDots2467 = 0x100286a,
		/// <summary>U+286b BRAILLE PATTERN DOTS-12467</summary>
		BrailleDots12467 = 0x100286b,
		/// <summary>U+286c BRAILLE PATTERN DOTS-3467</summary>
		BrailleDots3467 = 0x100286c,
		/// <summary>U+286d BRAILLE PATTERN DOTS-13467</summary>
		BrailleDots13467 = 0x100286d,
		/// <summary>U+286e BRAILLE PATTERN DOTS-23467</summary>
		BrailleDots23467 = 0x100286e,
		/// <summary>U+286f BRAILLE PATTERN DOTS-123467</summary>
		BrailleDots123467 = 0x100286f,
		/// <summary>U+2870 BRAILLE PATTERN DOTS-567</summary>
		BrailleDots567 = 0x1002870,
		/// <summary>U+2871 BRAILLE PATTERN DOTS-1567</summary>
		BrailleDots1567 = 0x1002871,
		/// <summary>U+2872 BRAILLE PATTERN DOTS-2567</summary>
		BrailleDots2567 = 0x1002872,
		/// <summary>U+2873 BRAILLE PATTERN DOTS-12567</summary>
		BrailleDots12567 = 0x1002873,
		/// <summary>U+2874 BRAILLE PATTERN DOTS-3567</summary>
		BrailleDots3567 = 0x1002874,
		/// <summary>U+2875 BRAILLE PATTERN DOTS-13567</summary>
		BrailleDots13567 = 0x1002875,
		/// <summary>U+2876 BRAILLE PATTERN DOTS-23567</summary>
		BrailleDots23567 = 0x1002876,
		/// <summary>U+2877 BRAILLE PATTERN DOTS-123567</summary>
		BrailleDots123567 = 0x1002877,
		/// <summary>U+2878 BRAILLE PATTERN DOTS-4567</summary>
		BrailleDots4567 = 0x1002878,
		/// <summary>U+2879 BRAILLE PATTERN DOTS-14567</summary>
		BrailleDots14567 = 0x1002879,
		/// <summary>U+287a BRAILLE PATTERN DOTS-24567</summary>
		BrailleDots24567 = 0x100287a,
		/// <summary>U+287b BRAILLE PATTERN DOTS-124567</summary>
		BrailleDots124567 = 0x100287b,
		/// <summary>U+287c BRAILLE PATTERN DOTS-34567</summary>
		BrailleDots34567 = 0x100287c,
		/// <summary>U+287d BRAILLE PATTERN DOTS-134567</summary>
		BrailleDots134567 = 0x100287d,
		/// <summary>U+287e BRAILLE PATTERN DOTS-234567</summary>
		BrailleDots234567 = 0x100287e,
		/// <summary>U+287f BRAILLE PATTERN DOTS-1234567</summary>
		BrailleDots1234567 = 0x100287f,
		/// <summary>U+2880 BRAILLE PATTERN DOTS-8</summary>
		BrailleDots8 = 0x1002880,
		/// <summary>U+2881 BRAILLE PATTERN DOTS-18</summary>
		BrailleDots18 = 0x1002881,
		/// <summary>U+2882 BRAILLE PATTERN DOTS-28</summary>
		BrailleDots28 = 0x1002882,
		/// <summary>U+2883 BRAILLE PATTERN DOTS-128</summary>
		BrailleDots128 = 0x1002883,
		/// <summary>U+2884 BRAILLE PATTERN DOTS-38</summary>
		BrailleDots38 = 0x1002884,
		/// <summary>U+2885 BRAILLE PATTERN DOTS-138</summary>
		BrailleDots138 = 0x1002885,
		/// <summary>U+2886 BRAILLE PATTERN DOTS-238</summary>
		BrailleDots238 = 0x1002886,
		/// <summary>U+2887 BRAILLE PATTERN DOTS-1238</summary>
		BrailleDots1238 = 0x1002887,
		/// <summary>U+2888 BRAILLE PATTERN DOTS-48</summary>
		BrailleDots48 = 0x1002888,
		/// <summary>U+2889 BRAILLE PATTERN DOTS-148</summary>
		BrailleDots148 = 0x1002889,
		/// <summary>U+288a BRAILLE PATTERN DOTS-248</summary>
		BrailleDots248 = 0x100288a,
		/// <summary>U+288b BRAILLE PATTERN DOTS-1248</summary>
		BrailleDots1248 = 0x100288b,
		/// <summary>U+288c BRAILLE PATTERN DOTS-348</summary>
		BrailleDots348 = 0x100288c,
		/// <summary>U+288d BRAILLE PATTERN DOTS-1348</summary>
		BrailleDots1348 = 0x100288d,
		/// <summary>U+288e BRAILLE PATTERN DOTS-2348</summary>
		BrailleDots2348 = 0x100288e,
		/// <summary>U+288f BRAILLE PATTERN DOTS-12348</summary>
		BrailleDots12348 = 0x100288f,
		/// <summary>U+2890 BRAILLE PATTERN DOTS-58</summary>
		BrailleDots58 = 0x1002890,
		/// <summary>U+2891 BRAILLE PATTERN DOTS-158</summary>
		BrailleDots158 = 0x1002891,
		/// <summary>U+2892 BRAILLE PATTERN DOTS-258</summary>
		BrailleDots258 = 0x1002892,
		/// <summary>U+2893 BRAILLE PATTERN DOTS-1258</summary>
		BrailleDots1258 = 0x1002893,
		/// <summary>U+2894 BRAILLE PATTERN DOTS-358</summary>
		BrailleDots358 = 0x1002894,
		/// <summary>U+2895 BRAILLE PATTERN DOTS-1358</summary>
		BrailleDots1358 = 0x1002895,
		/// <summary>U+2896 BRAILLE PATTERN DOTS-2358</summary>
		BrailleDots2358 = 0x1002896,
		/// <summary>U+2897 BRAILLE PATTERN DOTS-12358</summary>
		BrailleDots12358 = 0x1002897,
		/// <summary>U+2898 BRAILLE PATTERN DOTS-458</summary>
		BrailleDots458 = 0x1002898,
		/// <summary>U+2899 BRAILLE PATTERN DOTS-1458</summary>
		BrailleDots1458 = 0x1002899,
		/// <summary>U+289a BRAILLE PATTERN DOTS-2458</summary>
		BrailleDots2458 = 0x100289a,
		/// <summary>U+289b BRAILLE PATTERN DOTS-12458</summary>
		BrailleDots12458 = 0x100289b,
		/// <summary>U+289c BRAILLE PATTERN DOTS-3458</summary>
		BrailleDots3458 = 0x100289c,
		/// <summary>U+289d BRAILLE PATTERN DOTS-13458</summary>
		BrailleDots13458 = 0x100289d,
		/// <summary>U+289e BRAILLE PATTERN DOTS-23458</summary>
		BrailleDots23458 = 0x100289e,
		/// <summary>U+289f BRAILLE PATTERN DOTS-123458</summary>
		BrailleDots123458 = 0x100289f,
		/// <summary>U+28a0 BRAILLE PATTERN DOTS-68</summary>
		BrailleDots68 = 0x10028a0,
		/// <summary>U+28a1 BRAILLE PATTERN DOTS-168</summary>
		BrailleDots168 = 0x10028a1,
		/// <summary>U+28a2 BRAILLE PATTERN DOTS-268</summary>
		BrailleDots268 = 0x10028a2,
		/// <summary>U+28a3 BRAILLE PATTERN DOTS-1268</summary>
		BrailleDots1268 = 0x10028a3,
		/// <summary>U+28a4 BRAILLE PATTERN DOTS-368</summary>
		BrailleDots368 = 0x10028a4,
		/// <summary>U+28a5 BRAILLE PATTERN DOTS-1368</summary>
		BrailleDots1368 = 0x10028a5,
		/// <summary>U+28a6 BRAILLE PATTERN DOTS-2368</summary>
		BrailleDots2368 = 0x10028a6,
		/// <summary>U+28a7 BRAILLE PATTERN DOTS-12368</summary>
		BrailleDots12368 = 0x10028a7,
		/// <summary>U+28a8 BRAILLE PATTERN DOTS-468</summary>
		BrailleDots468 = 0x10028a8,
		/// <summary>U+28a9 BRAILLE PATTERN DOTS-1468</summary>
		BrailleDots1468 = 0x10028a9,
		/// <summary>U+28aa BRAILLE PATTERN DOTS-2468</summary>
		BrailleDots2468 = 0x10028aa,
		/// <summary>U+28ab BRAILLE PATTERN DOTS-12468</summary>
		BrailleDots12468 = 0x10028ab,
		/// <summary>U+28ac BRAILLE PATTERN DOTS-3468</summary>
		BrailleDots3468 = 0x10028ac,
		/// <summary>U+28ad BRAILLE PATTERN DOTS-13468</summary>
		BrailleDots13468 = 0x10028ad,
		/// <summary>U+28ae BRAILLE PATTERN DOTS-23468</summary>
		BrailleDots23468 = 0x10028ae,
		/// <summary>U+28af BRAILLE PATTERN DOTS-123468</summary>
		BrailleDots123468 = 0x10028af,
		/// <summary>U+28b0 BRAILLE PATTERN DOTS-568</summary>
		BrailleDots568 = 0x10028b0,
		/// <summary>U+28b1 BRAILLE PATTERN DOTS-1568</summary>
		BrailleDots1568 = 0x10028b1,
		/// <summary>U+28b2 BRAILLE PATTERN DOTS-2568</summary>
		BrailleDots2568 = 0x10028b2,
		/// <summary>U+28b3 BRAILLE PATTERN DOTS-12568</summary>
		BrailleDots12568 = 0x10028b3,
		/// <summary>U+28b4 BRAILLE PATTERN DOTS-3568</summary>
		BrailleDots3568 = 0x10028b4,
		/// <summary>U+28b5 BRAILLE PATTERN DOTS-13568</summary>
		BrailleDots13568 = 0x10028b5,
		/// <summary>U+28b6 BRAILLE PATTERN DOTS-23568</summary>
		BrailleDots23568 = 0x10028b6,
		/// <summary>U+28b7 BRAILLE PATTERN DOTS-123568</summary>
		BrailleDots123568 = 0x10028b7,
		/// <summary>U+28b8 BRAILLE PATTERN DOTS-4568</summary>
		BrailleDots4568 = 0x10028b8,
		/// <summary>U+28b9 BRAILLE PATTERN DOTS-14568</summary>
		BrailleDots14568 = 0x10028b9,
		/// <summary>U+28ba BRAILLE PATTERN DOTS-24568</summary>
		BrailleDots24568 = 0x10028ba,
		/// <summary>U+28bb BRAILLE PATTERN DOTS-124568</summary>
		BrailleDots124568 = 0x10028bb,
		/// <summary>U+28bc BRAILLE PATTERN DOTS-34568</summary>
		BrailleDots34568 = 0x10028bc,
		/// <summary>U+28bd BRAILLE PATTERN DOTS-134568</summary>
		BrailleDots134568 = 0x10028bd,
		/// <summary>U+28be BRAILLE PATTERN DOTS-234568</summary>
		BrailleDots234568 = 0x10028be,
		/// <summary>U+28bf BRAILLE PATTERN DOTS-1234568</summary>
		BrailleDots1234568 = 0x10028bf,
		/// <summary>U+28c0 BRAILLE PATTERN DOTS-78</summary>
		BrailleDots78 = 0x10028c0,
		/// <summary>U+28c1 BRAILLE PATTERN DOTS-178</summary>
		BrailleDots178 = 0x10028c1,
		/// <summary>U+28c2 BRAILLE PATTERN DOTS-278</summary>
		BrailleDots278 = 0x10028c2,
		/// <summary>U+28c3 BRAILLE PATTERN DOTS-1278</summary>
		BrailleDots1278 = 0x10028c3,
		/// <summary>U+28c4 BRAILLE PATTERN DOTS-378</summary>
		BrailleDots378 = 0x10028c4,
		/// <summary>U+28c5 BRAILLE PATTERN DOTS-1378</summary>
		BrailleDots1378 = 0x10028c5,
		/// <summary>U+28c6 BRAILLE PATTERN DOTS-2378</summary>
		BrailleDots2378 = 0x10028c6,
		/// <summary>U+28c7 BRAILLE PATTERN DOTS-12378</summary>
		BrailleDots12378 = 0x10028c7,
		/// <summary>U+28c8 BRAILLE PATTERN DOTS-478</summary>
		BrailleDots478 = 0x10028c8,
		/// <summary>U+28c9 BRAILLE PATTERN DOTS-1478</summary>
		BrailleDots1478 = 0x10028c9,
		/// <summary>U+28ca BRAILLE PATTERN DOTS-2478</summary>
		BrailleDots2478 = 0x10028ca,
		/// <summary>U+28cb BRAILLE PATTERN DOTS-12478</summary>
		BrailleDots12478 = 0x10028cb,
		/// <summary>U+28cc BRAILLE PATTERN DOTS-3478</summary>
		BrailleDots3478 = 0x10028cc,
		/// <summary>U+28cd BRAILLE PATTERN DOTS-13478</summary>
		BrailleDots13478 = 0x10028cd,
		/// <summary>U+28ce BRAILLE PATTERN DOTS-23478</summary>
		BrailleDots23478 = 0x10028ce,
		/// <summary>U+28cf BRAILLE PATTERN DOTS-123478</summary>
		BrailleDots123478 = 0x10028cf,
		/// <summary>U+28d0 BRAILLE PATTERN DOTS-578</summary>
		BrailleDots578 = 0x10028d0,
		/// <summary>U+28d1 BRAILLE PATTERN DOTS-1578</summary>
		BrailleDots1578 = 0x10028d1,
		/// <summary>U+28d2 BRAILLE PATTERN DOTS-2578</summary>
		BrailleDots2578 = 0x10028d2,
		/// <summary>U+28d3 BRAILLE PATTERN DOTS-12578</summary>
		BrailleDots12578 = 0x10028d3,
		/// <summary>U+28d4 BRAILLE PATTERN DOTS-3578</summary>
		BrailleDots3578 = 0x10028d4,
		/// <summary>U+28d5 BRAILLE PATTERN DOTS-13578</summary>
		BrailleDots13578 = 0x10028d5,
		/// <summary>U+28d6 BRAILLE PATTERN DOTS-23578</summary>
		BrailleDots23578 = 0x10028d6,
		/// <summary>U+28d7 BRAILLE PATTERN DOTS-123578</summary>
		BrailleDots123578 = 0x10028d7,
		/// <summary>U+28d8 BRAILLE PATTERN DOTS-4578</summary>
		BrailleDots4578 = 0x10028d8,
		/// <summary>U+28d9 BRAILLE PATTERN DOTS-14578</summary>
		BrailleDots14578 = 0x10028d9,
		/// <summary>U+28da BRAILLE PATTERN DOTS-24578</summary>
		BrailleDots24578 = 0x10028da,
		/// <summary>U+28db BRAILLE PATTERN DOTS-124578</summary>
		BrailleDots124578 = 0x10028db,
		/// <summary>U+28dc BRAILLE PATTERN DOTS-34578</summary>
		BrailleDots34578 = 0x10028dc,
		/// <summary>U+28dd BRAILLE PATTERN DOTS-134578</summary>
		BrailleDots134578 = 0x10028dd,
		/// <summary>U+28de BRAILLE PATTERN DOTS-234578</summary>
		BrailleDots234578 = 0x10028de,
		/// <summary>U+28df BRAILLE PATTERN DOTS-1234578</summary>
		BrailleDots1234578 = 0x10028df,
		/// <summary>U+28e0 BRAILLE PATTERN DOTS-678</summary>
		BrailleDots678 = 0x10028e0,
		/// <summary>U+28e1 BRAILLE PATTERN DOTS-1678</summary>
		BrailleDots1678 = 0x10028e1,
		/// <summary>U+28e2 BRAILLE PATTERN DOTS-2678</summary>
		BrailleDots2678 = 0x10028e2,
		/// <summary>U+28e3 BRAILLE PATTERN DOTS-12678</summary>
		BrailleDots12678 = 0x10028e3,
		/// <summary>U+28e4 BRAILLE PATTERN DOTS-3678</summary>
		BrailleDots3678 = 0x10028e4,
		/// <summary>U+28e5 BRAILLE PATTERN DOTS-13678</summary>
		BrailleDots13678 = 0x10028e5,
		/// <summary>U+28e6 BRAILLE PATTERN DOTS-23678</summary>
		BrailleDots23678 = 0x10028e6,
		/// <summary>U+28e7 BRAILLE PATTERN DOTS-123678</summary>
		BrailleDots123678 = 0x10028e7,
		/// <summary>U+28e8 BRAILLE PATTERN DOTS-4678</summary>
		BrailleDots4678 = 0x10028e8,
		/// <summary>U+28e9 BRAILLE PATTERN DOTS-14678</summary>
		BrailleDots14678 = 0x10028e9,
		/// <summary>U+28ea BRAILLE PATTERN DOTS-24678</summary>
		BrailleDots24678 = 0x10028ea,
		/// <summary>U+28eb BRAILLE PATTERN DOTS-124678</summary>
		BrailleDots124678 = 0x10028eb,
		/// <summary>U+28ec BRAILLE PATTERN DOTS-34678</summary>
		BrailleDots34678 = 0x10028ec,
		/// <summary>U+28ed BRAILLE PATTERN DOTS-134678</summary>
		BrailleDots134678 = 0x10028ed,
		/// <summary>U+28ee BRAILLE PATTERN DOTS-234678</summary>
		BrailleDots234678 = 0x10028ee,
		/// <summary>U+28ef BRAILLE PATTERN DOTS-1234678</summary>
		BrailleDots1234678 = 0x10028ef,
		/// <summary>U+28f0 BRAILLE PATTERN DOTS-5678</summary>
		BrailleDots5678 = 0x10028f0,
		/// <summary>U+28f1 BRAILLE PATTERN DOTS-15678</summary>
		BrailleDots15678 = 0x10028f1,
		/// <summary>U+28f2 BRAILLE PATTERN DOTS-25678</summary>
		BrailleDots25678 = 0x10028f2,
		/// <summary>U+28f3 BRAILLE PATTERN DOTS-125678</summary>
		BrailleDots125678 = 0x10028f3,
		/// <summary>U+28f4 BRAILLE PATTERN DOTS-35678</summary>
		BrailleDots35678 = 0x10028f4,
		/// <summary>U+28f5 BRAILLE PATTERN DOTS-135678</summary>
		BrailleDots135678 = 0x10028f5,
		/// <summary>U+28f6 BRAILLE PATTERN DOTS-235678</summary>
		BrailleDots235678 = 0x10028f6,
		/// <summary>U+28f7 BRAILLE PATTERN DOTS-1235678</summary>
		BrailleDots1235678 = 0x10028f7,
		/// <summary>U+28f8 BRAILLE PATTERN DOTS-45678</summary>
		BrailleDots45678 = 0x10028f8,
		/// <summary>U+28f9 BRAILLE PATTERN DOTS-145678</summary>
		BrailleDots145678 = 0x10028f9,
		/// <summary>U+28fa BRAILLE PATTERN DOTS-245678</summary>
		BrailleDots245678 = 0x10028fa,
		/// <summary>U+28fb BRAILLE PATTERN DOTS-1245678</summary>
		BrailleDots1245678 = 0x10028fb,
		/// <summary>U+28fc BRAILLE PATTERN DOTS-345678</summary>
		BrailleDots345678 = 0x10028fc,
		/// <summary>U+28fd BRAILLE PATTERN DOTS-1345678</summary>
		BrailleDots1345678 = 0x10028fd,
		/// <summary>U+28fe BRAILLE PATTERN DOTS-2345678</summary>
		BrailleDots2345678 = 0x10028fe,
		/// <summary>U+28ff BRAILLE PATTERN DOTS-12345678</summary>
		BrailleDots12345678 = 0x10028ff,
		// #endif /* XK_BRAILLE */

		/*
		* Sinhala (http://unicode.org/charts/PDF/U0D80.pdf)
		* http://www.nongnu.org/sinhala/doc/transliteration/sinhala-transliteration_6.html
		*/

		// #ifdef XK_SINHALA
		/// <summary>U+0D82 SINHALA ANUSVARAYA</summary>
		SinhNg = 0x1000d82,
		/// <summary>U+0D83 SINHALA VISARGAYA</summary>
		SinhH2 = 0x1000d83,
		/// <summary>U+0D85 SINHALA AYANNA</summary>
		SinhA = 0x1000d85,
		/// <summary>U+0D86 SINHALA AAYANNA</summary>
		SinhAa = 0x1000d86,
		/// <summary>U+0D87 SINHALA AEYANNA</summary>
		SinhAe = 0x1000d87,
		/// <summary>U+0D88 SINHALA AEEYANNA</summary>
		SinhAee = 0x1000d88,
		/// <summary>U+0D89 SINHALA IYANNA</summary>
		SinhI = 0x1000d89,
		/// <summary>U+0D8A SINHALA IIYANNA</summary>
		SinhIi = 0x1000d8a,
		/// <summary>U+0D8B SINHALA UYANNA</summary>
		SinhU = 0x1000d8b,
		/// <summary>U+0D8C SINHALA UUYANNA</summary>
		SinhUu = 0x1000d8c,
		/// <summary>U+0D8D SINHALA IRUYANNA</summary>
		SinhRi = 0x1000d8d,
		/// <summary>U+0D8E SINHALA IRUUYANNA</summary>
		SinhRii = 0x1000d8e,
		/// <summary>U+0D8F SINHALA ILUYANNA</summary>
		SinhLu = 0x1000d8f,
		/// <summary>U+0D90 SINHALA ILUUYANNA</summary>
		SinhLuu = 0x1000d90,
		/// <summary>U+0D91 SINHALA EYANNA</summary>
		SinhE = 0x1000d91,
		/// <summary>U+0D92 SINHALA EEYANNA</summary>
		SinhEe = 0x1000d92,
		/// <summary>U+0D93 SINHALA AIYANNA</summary>
		SinhAi = 0x1000d93,
		/// <summary>U+0D94 SINHALA OYANNA</summary>
		SinhO = 0x1000d94,
		/// <summary>U+0D95 SINHALA OOYANNA</summary>
		SinhOo = 0x1000d95,
		/// <summary>U+0D96 SINHALA AUYANNA</summary>
		SinhAu = 0x1000d96,
		/// <summary>U+0D9A SINHALA KAYANNA</summary>
		SinhKa = 0x1000d9a,
		/// <summary>U+0D9B SINHALA MAHA. KAYANNA</summary>
		SinhKha = 0x1000d9b,
		/// <summary>U+0D9C SINHALA GAYANNA</summary>
		SinhGa = 0x1000d9c,
		/// <summary>U+0D9D SINHALA MAHA. GAYANNA</summary>
		SinhGha = 0x1000d9d,
		/// <summary>U+0D9E SINHALA KANTAJA NAASIKYAYA</summary>
		SinhNg2 = 0x1000d9e,
		/// <summary>U+0D9F SINHALA SANYAKA GAYANNA</summary>
		SinhNga = 0x1000d9f,
		/// <summary>U+0DA0 SINHALA CAYANNA</summary>
		SinhCa = 0x1000da0,
		/// <summary>U+0DA1 SINHALA MAHA. CAYANNA</summary>
		SinhCha = 0x1000da1,
		/// <summary>U+0DA2 SINHALA JAYANNA</summary>
		SinhJa = 0x1000da2,
		/// <summary>U+0DA3 SINHALA MAHA. JAYANNA</summary>
		SinhJha = 0x1000da3,
		/// <summary>U+0DA4 SINHALA TAALUJA NAASIKYAYA</summary>
		SinhNya = 0x1000da4,
		/// <summary>U+0DA5 SINHALA TAALUJA SANYOOGA NAASIKYAYA</summary>
		SinhJnya = 0x1000da5,
		/// <summary>U+0DA6 SINHALA SANYAKA JAYANNA</summary>
		SinhNja = 0x1000da6,
		/// <summary>U+0DA7 SINHALA TTAYANNA</summary>
		SinhTta = 0x1000da7,
		/// <summary>U+0DA8 SINHALA MAHA. TTAYANNA</summary>
		SinhTtha = 0x1000da8,
		/// <summary>U+0DA9 SINHALA DDAYANNA</summary>
		SinhDda = 0x1000da9,
		/// <summary>U+0DAA SINHALA MAHA. DDAYANNA</summary>
		SinhDdha = 0x1000daa,
		/// <summary>U+0DAB SINHALA MUURDHAJA NAYANNA</summary>
		SinhNna = 0x1000dab,
		/// <summary>U+0DAC SINHALA SANYAKA DDAYANNA</summary>
		SinhNdda = 0x1000dac,
		/// <summary>U+0DAD SINHALA TAYANNA</summary>
		SinhTha = 0x1000dad,
		/// <summary>U+0DAE SINHALA MAHA. TAYANNA</summary>
		SinhThha = 0x1000dae,
		/// <summary>U+0DAF SINHALA DAYANNA</summary>
		SinhDha = 0x1000daf,
		/// <summary>U+0DB0 SINHALA MAHA. DAYANNA</summary>
		SinhDhha = 0x1000db0,
		/// <summary>U+0DB1 SINHALA DANTAJA NAYANNA</summary>
		SinhNa = 0x1000db1,
		/// <summary>U+0DB3 SINHALA SANYAKA DAYANNA</summary>
		SinhNdha = 0x1000db3,
		/// <summary>U+0DB4 SINHALA PAYANNA</summary>
		SinhPa = 0x1000db4,
		/// <summary>U+0DB5 SINHALA MAHA. PAYANNA</summary>
		SinhPha = 0x1000db5,
		/// <summary>U+0DB6 SINHALA BAYANNA</summary>
		SinhBa = 0x1000db6,
		/// <summary>U+0DB7 SINHALA MAHA. BAYANNA</summary>
		SinhBha = 0x1000db7,
		/// <summary>U+0DB8 SINHALA MAYANNA</summary>
		SinhMa = 0x1000db8,
		/// <summary>U+0DB9 SINHALA AMBA BAYANNA</summary>
		SinhMba = 0x1000db9,
		/// <summary>U+0DBA SINHALA YAYANNA</summary>
		SinhYa = 0x1000dba,
		/// <summary>U+0DBB SINHALA RAYANNA</summary>
		SinhRa = 0x1000dbb,
		/// <summary>U+0DBD SINHALA DANTAJA LAYANNA</summary>
		SinhLa = 0x1000dbd,
		/// <summary>U+0DC0 SINHALA VAYANNA</summary>
		SinhVa = 0x1000dc0,
		/// <summary>U+0DC1 SINHALA TAALUJA SAYANNA</summary>
		SinhSha = 0x1000dc1,
		/// <summary>U+0DC2 SINHALA MUURDHAJA SAYANNA</summary>
		SinhSsha = 0x1000dc2,
		/// <summary>U+0DC3 SINHALA DANTAJA SAYANNA</summary>
		SinhSa = 0x1000dc3,
		/// <summary>U+0DC4 SINHALA HAYANNA</summary>
		SinhHa = 0x1000dc4,
		/// <summary>U+0DC5 SINHALA MUURDHAJA LAYANNA</summary>
		SinhLla = 0x1000dc5,
		/// <summary>U+0DC6 SINHALA FAYANNA</summary>
		SinhFa = 0x1000dc6,
		/// <summary>U+0DCA SINHALA AL-LAKUNA</summary>
		SinhAl = 0x1000dca,
		/// <summary>U+0DCF SINHALA AELA-PILLA</summary>
		SinhAa2 = 0x1000dcf,
		/// <summary>U+0DD0 SINHALA AEDA-PILLA</summary>
		SinhAe2 = 0x1000dd0,
		/// <summary>U+0DD1 SINHALA DIGA AEDA-PILLA</summary>
		SinhAee2 = 0x1000dd1,
		/// <summary>U+0DD2 SINHALA IS-PILLA</summary>
		SinhI2 = 0x1000dd2,
		/// <summary>U+0DD3 SINHALA DIGA IS-PILLA</summary>
		SinhIi2 = 0x1000dd3,
		/// <summary>U+0DD4 SINHALA PAA-PILLA</summary>
		SinhU2 = 0x1000dd4,
		/// <summary>U+0DD6 SINHALA DIGA PAA-PILLA</summary>
		SinhUu2 = 0x1000dd6,
		/// <summary>U+0DD8 SINHALA GAETTA-PILLA</summary>
		SinhRu2 = 0x1000dd8,
		/// <summary>U+0DD9 SINHALA KOMBUVA</summary>
		SinhE2 = 0x1000dd9,
		/// <summary>U+0DDA SINHALA DIGA KOMBUVA</summary>
		SinhEe2 = 0x1000dda,
		/// <summary>U+0DDB SINHALA KOMBU DEKA</summary>
		SinhAi2 = 0x1000ddb,
		/// <summary>U+0DDC SINHALA KOMBUVA HAA AELA-PILLA</summary>
		SinhO2 = 0x1000ddc,
		/// <summary>U+0DDD SINHALA KOMBUVA HAA DIGA AELA-PILLA</summary>
		SinhOo2 = 0x1000ddd,
		/// <summary>U+0DDE SINHALA KOMBUVA HAA GAYANUKITTA</summary>
		SinhAu2 = 0x1000dde,
		/// <summary>U+0DDF SINHALA GAYANUKITTA</summary>
		SinhLu2 = 0x1000ddf,
		/// <summary>U+0DF2 SINHALA DIGA GAETTA-PILLA</summary>
		SinhRuu2 = 0x1000df2,
		/// <summary>U+0DF3 SINHALA DIGA GAYANUKITTA</summary>
		SinhLuu2 = 0x1000df3,
		/// <summary>U+0DF4 SINHALA KUNDDALIYA</summary>
		SinhKunddaliya = 0x1000df4,
		// #endif /* XK_SINHALA */


		/*** Some keys from XF86keysym.h ***/
		
		/// <summary>Monitor/panel brightness.</summary>
		MonBrightnessUp = 0x1008FF02,
		/// <summary>Monitor/panel brightness.</summary>
		MonBrightnessDown = 0x1008FF03,
		/// <summary>Keyboards may be lit.</summary>
		KbdLightOnOff = 0x1008FF04,
		/// <summary>Keyboards may be lit.</summary>
		KbdBrightnessUp = 0x1008FF05,
		/// <summary>Keyboards may be lit.</summary>
		KbdBrightnessDown = 0x1008FF06,
		/// <summary>System into standby mode.</summary>
		Standby = 0x1008FF10,
		/// <summary>Volume control down.</summary>
		AudioLowerVolume = 0x1008FF11,
		/// <summary>Mute sound from the system.</summary>
		AudioMute = 0x1008FF12,
		/// <summary>Volume control up</summary>
		AudioRaiseVolume = 0x1008FF13,
		/// <summary>Start playing of audio</summary>
		AudioPlay = 0x1008FF14,
		/// <summary>Stop playing audio</summary>
		AudioStop = 0x1008FF15,
		/// <summary>Previous track</summary>
		AudioPrev = 0x1008FF16,
		/// <summary>Next track</summary>
		AudioNext = 0x1008FF17,
		/// <summary>Invoke calculator program</summary>
		Calculator = 0x1008FF1D,
		/// <summary>Display user's home page</summary>
		HomePage = 0x1008FF18,
		/// <summary>Invoke user's mail program</summary>
		Mail = 0x1008FF19,
		/// <summary>Start application</summary>
		Start = 0x1008FF1A,
		/// <summary>Search</summary>
		Search = 0x1008FF1B,
		/// <summary>Record audio application</summary>
		AudioRecord = 0x1008FF1C,
		/// <summary>Like back on a browser</summary>
		Back = 0x1008FF26,
		/// <summary>Like forward on a browser</summary>
		Forward = 0x1008FF27,
		/// <summary>Stop current operation</summary>
		Stop = 0x1008FF28,
		/// <summary>Refresh the page</summary>
		Refresh = 0x1008FF29,
		/// <summary>Reload web page, file, etc.</summary>
		Reload = 0x1008FF73,
		/// <summary>Show favorite locations</summary>
		Favorites = 0x1008FF30,
		/// <summary>Power off system entirely</summary>
		PowerOff = 0x1008FF2A,
		/// <summary>Put system to sleep</summary>
		Sleep = 0x1008FF2F,
		/// <summary>Launch Application</summary>
		Launch5 = 0x1008FF45,
		/// <summary>Launch Application</summary>
		Launch6 = 0x1008FF46,
		/// <summary>Launch Application</summary>
		Launch7 = 0x1008FF47,
		/// <summary>Launch Application</summary>
		Launch8 = 0x1008FF48,
		/// <summary>Launch Application</summary>
		Launch9 = 0x1008FF49,
		/// <summary>Launch Application</summary>
		LaunchA = 0x1008FF4A,
		/// <summary>Launch Application</summary>
		LaunchB = 0x1008FF4B,
		/// <summary>Enable/disable WLAN</summary>
		WLAN = 0x1008FF95,
		/// <summary>Toolbox of desktop/app</summary>
		Tools = 0x1008FF81,

	}
}