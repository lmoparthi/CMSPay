
<Flags()>
Public Enum UIStates As Int32
    None = 0
    AsIs = 1
    Viewing = 2
    Adding = 4
    Added = 8
    Rejecting = 16
    Rejected = 32
    Saving = 64
    Saved = 128
    Canceling = 256
    Canceled = 512
    Approving = 1024
    Approved = 2048
    NotModifiable = 4096
    Modifiable = 8192
    Modifying = 16384
    Modified = 32768
    Deleting = 65536
    Deleted = 131072
    Closing = 262144
    Disposing = 524288
    Disposed = 1048576
    Archiving = 2097152
    Archived = 4194304
    [ReadOnly] = None Or NotModifiable Or Modifiable Or Viewing Or Canceling
    Editing = Modifying Or Adding Or Deleting
    Edited = Modified Or Added Or Deleted
    'Example of use if (_UIState And UIState.[ReadOnly]) = _UIState then true
End Enum

Enum HookType As Integer
    WH_JOURNALRECORD = 0
    WH_JOURNALPLAYBACK = 1
    WH_KEYBOARD = 2
    WH_GETMESSAGE = 3
    WH_CALLWNDPROC = 4
    WH_CBT = 5
    WH_SYSMSGFILTER = 6
    WH_MOUSE = 7
    WH_HARDWARE = 8
    WH_DEBUG = 9
    WH_SHELL = 10
    WH_FOREGROUNDIDLE = 11
    WH_CALLWNDPROCRET = 12
    WH_KEYBOARD_LL = 13
    WH_MOUSE_LL = 14
End Enum

Public Enum ProcessAccessFlag As Integer
    All = &H1F0FFF
    Terminate = &H1
    CreateThread = &H2
    VMOperation = &H8
    VMRead = &H10
    VMWrite = &H20
    DupHandle = &H40
    SetInformation = &H200
    QueryInformation = &H400
    Synchronize = &H100000
End Enum

<Flags>
Public Enum MouseEventDataXButtons As UInteger
    [Nothing] = &H0
    XBUTTON1 = &H1
    XBUTTON2 = &H2
End Enum

<Flags>
Public Enum MOUSEEVENTF As UInteger
    ABSOLUTE = &H8000
    HWHEEL = &H1000
    MOVE = &H1
    MOVE_NOCOALESCE = &H2000
    LEFTDOWN = &H2
    LEFTUP = &H4
    RIGHTDOWN = &H8
    RIGHTUP = &H10
    MIDDLEDOWN = &H20
    MIDDLEUP = &H40
    VIRTUALDESK = &H4000
    WHEEL = &H800
    XDOWN = &H80
    XUP = &H100
End Enum

Public Enum VirtualKeyShort As Short
    '''<summary>
    '''Left mouse button
    '''</summary>
    LBUTTON = &H1
    '''<summary>
    '''Right mouse button
    '''</summary>
    RBUTTON = &H2
    '''<summary>
    '''Control-break processing
    '''</summary>
    CANCEL = &H3
    '''<summary>
    '''Middle mouse button (three-button mouse)
    '''</summary>
    MBUTTON = &H4
    '''<summary>
    '''Windows 2000/XP: X1 mouse button
    '''</summary>
    XBUTTON1 = &H5
    '''<summary>
    '''Windows 2000/XP: X2 mouse button
    '''</summary>
    XBUTTON2 = &H6
    '''<summary>
    '''BACKSPACE key
    '''</summary>
    BACK = &H8
    '''<summary>
    '''TAB key
    '''</summary>
    TAB = &H9
    '''<summary>
    '''CLEAR key
    '''</summary>
    CLEAR = &HC
    '''<summary>
    '''ENTER key
    '''</summary>
    [RETURN] = &HD
    '''<summary>
    '''SHIFT key
    '''</summary>
    SHIFT = &H10
    '''<summary>
    '''CTRL key
    '''</summary>
    CONTROL = &H11
    '''<summary>
    '''ALT key
    '''</summary>
    MENU = &H12
    '''<summary>
    '''PAUSE key
    '''</summary>
    PAUSE = &H13
    '''<summary>
    '''CAPS LOCK key
    '''</summary>
    CAPITAL = &H14
    '''<summary>
    '''Input Method Editor (IME) Kana mode
    '''</summary>
    KANA = &H15
    '''<summary>
    '''IME Hangul mode
    '''</summary>
    HANGUL = &H15
    '''<summary>
    '''IME Junja mode
    '''</summary>
    JUNJA = &H17
    '''<summary>
    '''IME final mode
    '''</summary>
    FINAL = &H18
    '''<summary>
    '''IME Hanja mode
    '''</summary>
    HANJA = &H19
    '''<summary>
    '''IME Kanji mode
    '''</summary>
    KANJI = &H19
    '''<summary>
    '''ESC key
    '''</summary>
    ESCAPE = &H1B
    '''<summary>
    '''IME convert
    '''</summary>
    CONVERT = &H1C
    '''<summary>
    '''IME nonconvert
    '''</summary>
    NONCONVERT = &H1D
    '''<summary>
    '''IME accept
    '''</summary>
    ACCEPT = &H1E
    '''<summary>
    '''IME mode change request
    '''</summary>
    MODECHANGE = &H1F
    '''<summary>
    '''SPACEBAR
    '''</summary>
    SPACE = &H20
    '''<summary>
    '''PAGE UP key
    '''</summary>
    PRIOR = &H21
    '''<summary>
    '''PAGE DOWN key
    '''</summary>
    [NEXT] = &H22
    '''<summary>
    '''END key
    '''</summary>
    [END] = &H23
    '''<summary>
    '''HOME key
    '''</summary>
    HOME = &H24
    '''<summary>
    '''LEFT ARROW key
    '''</summary>
    LEFT = &H25
    '''<summary>
    '''UP ARROW key
    '''</summary>
    UP = &H26
    '''<summary>
    '''RIGHT ARROW key
    '''</summary>
    RIGHT = &H27
    '''<summary>
    '''DOWN ARROW key
    '''</summary>
    DOWN = &H28
    '''<summary>
    '''SELECT key
    '''</summary>
    [SELECT] = &H29
    '''<summary>
    '''PRINT key
    '''</summary>
    PRINT = &H2A
    '''<summary>
    '''EXECUTE key
    '''</summary>
    EXECUTE = &H2B
    '''<summary>
    '''PRINT SCREEN key
    '''</summary>
    SNAPSHOT = &H2C
    '''<summary>
    '''INS key
    '''</summary>
    INSERT = &H2D
    '''<summary>
    '''DEL key
    '''</summary>
    DELETE = &H2E
    '''<summary>
    '''HELP key
    '''</summary>
    HELP = &H2F
    '''<summary>
    '''0 key
    '''</summary>
    KEY_0 = &H30
    '''<summary>
    '''1 key
    '''</summary>
    KEY_1 = &H31
    '''<summary>
    '''2 key
    '''</summary>
    KEY_2 = &H32
    '''<summary>
    '''3 key
    '''</summary>
    KEY_3 = &H33
    '''<summary>
    '''4 key
    '''</summary>
    KEY_4 = &H34
    '''<summary>
    '''5 key
    '''</summary>
    KEY_5 = &H35
    '''<summary>
    '''6 key
    '''</summary>
    KEY_6 = &H36
    '''<summary>
    '''7 key
    '''</summary>
    KEY_7 = &H37
    '''<summary>
    '''8 key
    '''</summary>
    KEY_8 = &H38
    '''<summary>
    '''9 key
    '''</summary>
    KEY_9 = &H39
    '''<summary>
    '''A key
    '''</summary>
    KEY_A = &H41
    '''<summary>
    '''B key
    '''</summary>
    KEY_B = &H42
    '''<summary>
    '''C key
    '''</summary>
    KEY_C = &H43
    '''<summary>
    '''D key
    '''</summary>
    KEY_D = &H44
    '''<summary>
    '''E key
    '''</summary>
    KEY_E = &H45
    '''<summary>
    '''F key
    '''</summary>
    KEY_F = &H46
    '''<summary>
    '''G key
    '''</summary>
    KEY_G = &H47
    '''<summary>
    '''H key
    '''</summary>
    KEY_H = &H48
    '''<summary>
    '''I key
    '''</summary>
    KEY_I = &H49
    '''<summary>
    '''J key
    '''</summary>
    KEY_J = &H4A
    '''<summary>
    '''K key
    '''</summary>
    KEY_K = &H4B
    '''<summary>
    '''L key
    '''</summary>
    KEY_L = &H4C
    '''<summary>
    '''M key
    '''</summary>
    KEY_M = &H4D
    '''<summary>
    '''N key
    '''</summary>
    KEY_N = &H4E
    '''<summary>
    '''O key
    '''</summary>
    KEY_O = &H4F
    '''<summary>
    '''P key
    '''</summary>
    KEY_P = &H50
    '''<summary>
    '''Q key
    '''</summary>
    KEY_Q = &H51
    '''<summary>
    '''R key
    '''</summary>
    KEY_R = &H52
    '''<summary>
    '''S key
    '''</summary>
    KEY_S = &H53
    '''<summary>
    '''T key
    '''</summary>
    KEY_T = &H54
    '''<summary>
    '''U key
    '''</summary>
    KEY_U = &H55
    '''<summary>
    '''V key
    '''</summary>
    KEY_V = &H56
    '''<summary>
    '''W key
    '''</summary>
    KEY_W = &H57
    '''<summary>
    '''X key
    '''</summary>
    KEY_X = &H58
    '''<summary>
    '''Y key
    '''</summary>
    KEY_Y = &H59
    '''<summary>
    '''Z key
    '''</summary>
    KEY_Z = &H5A
    '''<summary>
    '''Left Windows key (Microsoft Natural keyboard) 
    '''</summary>
    LWIN = &H5B
    '''<summary>
    '''Right Windows key (Natural keyboard)
    '''</summary>
    RWIN = &H5C
    '''<summary>
    '''Applications key (Natural keyboard)
    '''</summary>
    APPS = &H5D
    '''<summary>
    '''Computer Sleep key
    '''</summary>
    SLEEP = &H5F
    '''<summary>
    '''Numeric keypad 0 key
    '''</summary>
    NUMPAD0 = &H60
    '''<summary>
    '''Numeric keypad 1 key
    '''</summary>
    NUMPAD1 = &H61
    '''<summary>
    '''Numeric keypad 2 key
    '''</summary>
    NUMPAD2 = &H62
    '''<summary>
    '''Numeric keypad 3 key
    '''</summary>
    NUMPAD3 = &H63
    '''<summary>
    '''Numeric keypad 4 key
    '''</summary>
    NUMPAD4 = &H64
    '''<summary>
    '''Numeric keypad 5 key
    '''</summary>
    NUMPAD5 = &H65
    '''<summary>
    '''Numeric keypad 6 key
    '''</summary>
    NUMPAD6 = &H66
    '''<summary>
    '''Numeric keypad 7 key
    '''</summary>
    NUMPAD7 = &H67
    '''<summary>
    '''Numeric keypad 8 key
    '''</summary>
    NUMPAD8 = &H68
    '''<summary>
    '''Numeric keypad 9 key
    '''</summary>
    NUMPAD9 = &H69
    '''<summary>
    '''Multiply key
    '''</summary>
    MULTIPLY = &H6A
    '''<summary>
    '''Add key
    '''</summary>
    ADD = &H6B
    '''<summary>
    '''Separator key
    '''</summary>
    SEPARATOR = &H6C
    '''<summary>
    '''Subtract key
    '''</summary>
    SUBTRACT = &H6D
    '''<summary>
    '''Decimal key
    '''</summary>
    [DECIMAL] = &H6E
    '''<summary>
    '''Divide key
    '''</summary>
    DIVIDE = &H6F
    '''<summary>
    '''F1 key
    '''</summary>
    F1 = &H70
    '''<summary>
    '''F2 key
    '''</summary>
    F2 = &H71
    '''<summary>
    '''F3 key
    '''</summary>
    F3 = &H72
    '''<summary>
    '''F4 key
    '''</summary>
    F4 = &H73
    '''<summary>
    '''F5 key
    '''</summary>
    F5 = &H74
    '''<summary>
    '''F6 key
    '''</summary>
    F6 = &H75
    '''<summary>
    '''F7 key
    '''</summary>
    F7 = &H76
    '''<summary>
    '''F8 key
    '''</summary>
    F8 = &H77
    '''<summary>
    '''F9 key
    '''</summary>
    F9 = &H78
    '''<summary>
    '''F10 key
    '''</summary>
    F10 = &H79
    '''<summary>
    '''F11 key
    '''</summary>
    F11 = &H7A
    '''<summary>
    '''F12 key
    '''</summary>
    F12 = &H7B
    '''<summary>
    '''F13 key
    '''</summary>
    F13 = &H7C
    '''<summary>
    '''F14 key
    '''</summary>
    F14 = &H7D
    '''<summary>
    '''F15 key
    '''</summary>
    F15 = &H7E
    '''<summary>
    '''F16 key
    '''</summary>
    F16 = &H7F
    '''<summary>
    '''F17 key  
    '''</summary>
    F17 = &H80
    '''<summary>
    '''F18 key  
    '''</summary>
    F18 = &H81
    '''<summary>
    '''F19 key  
    '''</summary>
    F19 = &H82
    '''<summary>
    '''F20 key  
    '''</summary>
    F20 = &H83
    '''<summary>
    '''F21 key  
    '''</summary>
    F21 = &H84
    '''<summary>
    '''F22 key, (PPC only) Key used to lock device.
    '''</summary>
    F22 = &H85
    '''<summary>
    '''F23 key  
    '''</summary>
    F23 = &H86
    '''<summary>
    '''F24 key  
    '''</summary>
    F24 = &H87
    '''<summary>
    '''NUM LOCK key
    '''</summary>
    NUMLOCK = &H90
    '''<summary>
    '''SCROLL LOCK key
    '''</summary>
    SCROLL = &H91
    '''<summary>
    '''Left SHIFT key
    '''</summary>
    LSHIFT = &HA0
    '''<summary>
    '''Right SHIFT key
    '''</summary>
    RSHIFT = &HA1
    '''<summary>
    '''Left CONTROL key
    '''</summary>
    LCONTROL = &HA2
    '''<summary>
    '''Right CONTROL key
    '''</summary>
    RCONTROL = &HA3
    '''<summary>
    '''Left MENU key
    '''</summary>
    LMENU = &HA4
    '''<summary>
    '''Right MENU key
    '''</summary>
    RMENU = &HA5
    '''<summary>
    '''Windows 2000/XP: Browser Back key
    '''</summary>
    BROWSER_BACK = &HA6
    '''<summary>
    '''Windows 2000/XP: Browser Forward key
    '''</summary>
    BROWSER_FORWARD = &HA7
    '''<summary>
    '''Windows 2000/XP: Browser Refresh key
    '''</summary>
    BROWSER_REFRESH = &HA8
    '''<summary>
    '''Windows 2000/XP: Browser Stop key
    '''</summary>
    BROWSER_STOP = &HA9
    '''<summary>
    '''Windows 2000/XP: Browser Search key 
    '''</summary>
    BROWSER_SEARCH = &HAA
    '''<summary>
    '''Windows 2000/XP: Browser Favorites key
    '''</summary>
    BROWSER_FAVORITES = &HAB
    '''<summary>
    '''Windows 2000/XP: Browser Start and Home key
    '''</summary>
    BROWSER_HOME = &HAC
    '''<summary>
    '''Windows 2000/XP: Volume Mute key
    '''</summary>
    VOLUME_MUTE = &HAD
    '''<summary>
    '''Windows 2000/XP: Volume Down key
    '''</summary>
    VOLUME_DOWN = &HAE
    '''<summary>
    '''Windows 2000/XP: Volume Up key
    '''</summary>
    VOLUME_UP = &HAF
    '''<summary>
    '''Windows 2000/XP: Next Track key
    '''</summary>
    MEDIA_NEXT_TRACK = &HB0
    '''<summary>
    '''Windows 2000/XP: Previous Track key
    '''</summary>
    MEDIA_PREV_TRACK = &HB1
    '''<summary>
    '''Windows 2000/XP: Stop Media key
    '''</summary>
    MEDIA_STOP = &HB2
    '''<summary>
    '''Windows 2000/XP: Play/Pause Media key
    '''</summary>
    MEDIA_PLAY_PAUSE = &HB3
    '''<summary>
    '''Windows 2000/XP: Start Mail key
    '''</summary>
    LAUNCH_MAIL = &HB4
    '''<summary>
    '''Windows 2000/XP: Select Media key
    '''</summary>
    LAUNCH_MEDIA_SELECT = &HB5
    '''<summary>
    '''Windows 2000/XP: Start Application 1 key
    '''</summary>
    LAUNCH_APP1 = &HB6
    '''<summary>
    '''Windows 2000/XP: Start Application 2 key
    '''</summary>
    LAUNCH_APP2 = &HB7
    '''<summary>
    '''Used for miscellaneous characters; it can vary by keyboard.
    '''</summary>
    OEM_1 = &HBA
    '''<summary>
    '''Windows 2000/XP: For any country/region, the '+' key
    '''</summary>
    OEM_PLUS = &HBB
    '''<summary>
    '''Windows 2000/XP: For any country/region, the ',' key
    '''</summary>
    OEM_COMMA = &HBC
    '''<summary>
    '''Windows 2000/XP: For any country/region, the '-' key
    '''</summary>
    OEM_MINUS = &HBD
    '''<summary>
    '''Windows 2000/XP: For any country/region, the '.' key
    '''</summary>
    OEM_PERIOD = &HBE
    '''<summary>
    '''Used for miscellaneous characters; it can vary by keyboard.
    '''</summary>
    OEM_2 = &HBF
    '''<summary>
    '''Used for miscellaneous characters; it can vary by keyboard. 
    '''</summary>
    OEM_3 = &HC0
    '''<summary>
    '''Used for miscellaneous characters; it can vary by keyboard. 
    '''</summary>
    OEM_4 = &HDB
    '''<summary>
    '''Used for miscellaneous characters; it can vary by keyboard. 
    '''</summary>
    OEM_5 = &HDC
    '''<summary>
    '''Used for miscellaneous characters; it can vary by keyboard. 
    '''</summary>
    OEM_6 = &HDD
    '''<summary>
    '''Used for miscellaneous characters; it can vary by keyboard. 
    '''</summary>
    OEM_7 = &HDE
    '''<summary>
    '''Used for miscellaneous characters; it can vary by keyboard.
    '''</summary>
    OEM_8 = &HDF
    '''<summary>
    '''Windows 2000/XP: Either the angle bracket key or the backslash key on the RT 102-key keyboard
    '''</summary>
    OEM_102 = &HE2
    '''<summary>
    '''Windows 95/98/Me, Windows NT 4.0, Windows 2000/XP: IME PROCESS key
    '''</summary>
    PROCESSKEY = &HE5
    '''<summary>
    '''Windows 2000/XP: Used to pass Unicode characters as if they were keystrokes.
    '''The VK_PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods. For more information,
    '''see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN, and WM_KEYUP
    '''</summary>
    PACKET = &HE7
    '''<summary>
    '''Attn key
    '''</summary>
    ATTN = &HF6
    '''<summary>
    '''CrSel key
    '''</summary>
    CRSEL = &HF7
    '''<summary>
    '''ExSel key
    '''</summary>
    EXSEL = &HF8
    '''<summary>
    '''Erase EOF key
    '''</summary>
    EREOF = &HF9
    '''<summary>
    '''Play key
    '''</summary>
    PLAY = &HFA
    '''<summary>
    '''Zoom key
    '''</summary>
    ZOOM = &HFB
    '''<summary>
    '''Reserved 
    '''</summary>
    NONAME = &HFC
    '''<summary>
    '''PA1 key
    '''</summary>
    PA1 = &HFD
    '''<summary>
    '''Clear key
    '''</summary>
    OEM_CLEAR = &HFE
End Enum

Public Enum ScanCodeShort As Short
    LBUTTON = 0
    RBUTTON = 0
    CANCEL = 70
    MBUTTON = 0
    XBUTTON1 = 0
    XBUTTON2 = 0
    BACK = 14
    TAB = 15
    CLEAR = 76
    [RETURN] = 28
    SHIFT = 42
    CONTROL = 29
    MENU = 56
    PAUSE = 0
    CAPITAL = 58
    KANA = 0
    HANGUL = 0
    JUNJA = 0
    FINAL = 0
    HANJA = 0
    KANJI = 0
    ESCAPE = 1
    CONVERT = 0
    NONCONVERT = 0
    ACCEPT = 0
    MODECHANGE = 0
    SPACE = 57
    PRIOR = 73
    [NEXT] = 81
    [END] = 79
    HOME = 71
    LEFT = 75
    UP = 72
    RIGHT = 77
    DOWN = 80
    [SELECT] = 0
    PRINT = 0
    EXECUTE = 0
    SNAPSHOT = 84
    INSERT = 82
    DELETE = 83
    HELP = 99
    KEY_0 = 11
    KEY_1 = 2
    KEY_2 = 3
    KEY_3 = 4
    KEY_4 = 5
    KEY_5 = 6
    KEY_6 = 7
    KEY_7 = 8
    KEY_8 = 9
    KEY_9 = 10
    KEY_A = 30
    KEY_B = 48
    KEY_C = 46
    KEY_D = 32
    KEY_E = 18
    KEY_F = 33
    KEY_G = 34
    KEY_H = 35
    KEY_I = 23
    KEY_J = 36
    KEY_K = 37
    KEY_L = 38
    KEY_M = 50
    KEY_N = 49
    KEY_O = 24
    KEY_P = 25
    KEY_Q = 16
    KEY_R = 19
    KEY_S = 31
    KEY_T = 20
    KEY_U = 22
    KEY_V = 47
    KEY_W = 17
    KEY_X = 45
    KEY_Y = 21
    KEY_Z = 44
    LWIN = 91
    RWIN = 92
    APPS = 93
    SLEEP = 95
    NUMPAD0 = 82
    NUMPAD1 = 79
    NUMPAD2 = 80
    NUMPAD3 = 81
    NUMPAD4 = 75
    NUMPAD5 = 76
    NUMPAD6 = 77
    NUMPAD7 = 71
    NUMPAD8 = 72
    NUMPAD9 = 73
    MULTIPLY = 55
    ADD = 78
    SEPARATOR = 0
    SUBTRACT = 74
    [DECIMAL] = 83
    DIVIDE = 53
    F1 = 59
    F2 = 60
    F3 = 61
    F4 = 62
    F5 = 63
    F6 = 64
    F7 = 65
    F8 = 66
    F9 = 67
    F10 = 68
    F11 = 87
    F12 = 88
    F13 = 100
    F14 = 101
    F15 = 102
    F16 = 103
    F17 = 104
    F18 = 105
    F19 = 106
    F20 = 107
    F21 = 108
    F22 = 109
    F23 = 110
    F24 = 118
    NUMLOCK = 69
    SCROLL = 70
    LSHIFT = 42
    RSHIFT = 54
    LCONTROL = 29
    RCONTROL = 29
    LMENU = 56
    RMENU = 56
    BROWSER_BACK = 106
    BROWSER_FORWARD = 105
    BROWSER_REFRESH = 103
    BROWSER_STOP = 104
    BROWSER_SEARCH = 101
    BROWSER_FAVORITES = 102
    BROWSER_HOME = 50
    VOLUME_MUTE = 32
    VOLUME_DOWN = 46
    VOLUME_UP = 48
    MEDIA_NEXT_TRACK = 25
    MEDIA_PREV_TRACK = 16
    MEDIA_STOP = 36
    MEDIA_PLAY_PAUSE = 34
    LAUNCH_MAIL = 108
    LAUNCH_MEDIA_SELECT = 109
    LAUNCH_APP1 = 107
    LAUNCH_APP2 = 33
    OEM_1 = 39
    OEM_PLUS = 13
    OEM_COMMA = 51
    OEM_MINUS = 12
    OEM_PERIOD = 52
    OEM_2 = 53
    OEM_3 = 41
    OEM_4 = 26
    OEM_5 = 43
    OEM_6 = 27
    OEM_7 = 40
    OEM_8 = 0
    OEM_102 = 86
    PROCESSKEY = 0
    PACKET = 0
    ATTN = 0
    CRSEL = 0
    EXSEL = 0
    EREOF = 93
    PLAY = 0
    ZOOM = 98
    NONAME = 0
    PA1 = 0
    OEM_CLEAR = 0
End Enum

<Flags>
Public Enum KEYEVENTF As UInteger
    EXTENDEDKEY = &H1
    KEYUP = &H2
    SCANCODE = &H8
    UNICODE = &H4
End Enum    ' Declare the InputUnion struct

#Region "WM - Window Messages"
Public Enum WM As UInteger
    WM_NULL = &H0
    WM_CREATE = &H1
    WM_DESTROY = &H2
    WM_MOVE = &H3
    WM_SIZE = &H5
    WM_ACTIVATE = &H6
    WM_SETFOCUS = &H7
    WM_KILLFOCUS = &H8
    WM_ENABLE = &HA
    WM_SETREDRAW = &HB
    WM_SETTEXT = &HC
    WM_GETTEXT = &HD
    WM_GETTEXTLENGTH = &HE
    WM_PAINT = &HF
    WM_CLOSE = &H10
    WM_QUERYENDSESSION = &H11
    WM_QUIT = &H12
    WM_QUERYOPEN = &H13
    WM_ERASEBKGND = &H14
    WM_KEYDOWN = &H100
    WM_KEYUP = &H101
    WM_SYSKEYDOWN = &H104
    WM_SYSKEYUP = &H105

End Enum
Public Enum WindowLongFlag As Integer
    GWL_EXSTYLE = -20
    GWLP_HINSTANCE = -6
    GWLP_HWNDPARENT = -8
    GWL_ID = -12
    GWL_STYLE = -16
    GWL_USERDATA = -21
    GWL_WNDPROC = -4
    DWLP_USER = &H8
    DWLP_MSGRESULT = &H0
    DWLP_DLGPROC = &H4
End Enum

#End Region

Public Enum SC As Int32

    '''<summary>Sizes the window.</summary>
    SC_SIZE = &HF000

    '''<summary>Moves the window.</summary>
    SC_MOVE = &HF010

    '''<summary>Minimizes the window.</summary>
    SC_MINIMIZE = &HF020

    '''<summary>Maximizes the window.</summary>
    SC_MAXIMIZE = &HF030

    '''<summary>Moves to the next window.</summary>
    SC_NEXTWINDOW = &HF040

    '''<summary>Moves to the previous window.</summary>
    SC_PREVWINDOW = &HF050

    '''<summary>Closes the window.</summary>
    SC_CLOSE = &HF060

    '''<summary>Scrolls vertically.</summary>
    SC_VSCROLL = &HF070

    '''<summary>Scrolls horizontally.</summary>
    SC_HSCROLL = &HF080

    '''<summary>Retrieves the window menu as a result of a mouse click.</summary>
    SC_MOUSEMENU = &HF090

    '''<summary>
    '''Retrieves the window menu as a result of a keystroke.
    '''For more information, see the Remarks section.
    '''</summary>
    SC_KEYMENU = &HF100

    '''<summary>TODO</summary>
    SC_ARRANGE = &HF110

    '''<summary>Restores the window to its normal position and size.</summary>
    SC_RESTORE = &HF120

    '''<summary>Activates the Start menu.</summary>
    SC_TASKLIST = &HF130

    '''<summary>Executes the screen saver application specified in the [boot] section of the System.ini file.</summary>
    SC_SCREENSAVE = &HF140

    '''<summary>
    '''Activates the window associated with the application-specified hot key.
    '''The lParam parameter identifies the window to activate.
    '''</summary>
    SC_HOTKEY = &HF150

    '#if(WINVER >= &h0400) 'Win95

    '''<summary>Selects the default item; the user double-clicked the window menu.</summary>
    SC_DEFAULT = &HF160

    '''<summary>
    '''Sets the state of the display. This command supports devices that
    '''have power-saving features, such as a battery-powered personal computer.
    '''The lParam parameter can have the following values: -1 = the display is powering on,
    '''1 = the display is going to low power, 2 = the display is being shut off
    '''</summary>
    SC_MONITORPOWER = &HF170

    '''<summary>
    '''Changes the cursor to a question mark with a pointer. If the user
    '''then clicks a control in the dialog box, the control receives a WM_HELP message.
    '''</summary>
    SC_CONTEXTHELP = &HF180

    '''<summary>TODO</summary>
    SC_SEPARATOR = &HF00F
    '#endif 'WINVER >= &h0400

    '#if(WINVER >= &h0600) 'Vista

    '''<summary>Indicates whether the screen saver is secure.</summary>
    SCF_ISSECURE = &H1

    '#endif 'WINVER >= &h0600

    <
    Obsolete("Use SC_MINIMIZE instead."),
    System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)
      >
    SC_ICON = SC_MINIMIZE

    <
    Obsolete("Use SC_MAXIMIZE instead."),
    System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)
      >
    SC_ZOOM = SC_MAXIMIZE

End Enum