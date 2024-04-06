Imports System
Imports System.Runtime.InteropServices

Public NotInheritable Class NativeMethods
    Private Sub New()
    End Sub

    <DllImport("user32.dll", SetLastError:=True)>
    Public Shared Function GetKeyState(ByVal nVirtKey As Integer) As Short
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)>
    Public Shared Function OpenProcess(ByVal dwDesiredAccess As ProcessAccessFlag, <MarshalAs(UnmanagedType.Bool)> ByVal bInheritHandle As Boolean, ByVal dwProcessId As Integer) As IntPtr
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)>
    Public Shared Function CloseHandle(ByVal hObject As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)>
    Public Shared Function GetExitCodeProcess(ByVal hProcess As IntPtr, ByRef lpExitCode As UInteger) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

#Disable Warning BC40027 ' Return type of function is not CLS-compliant
    <DllImport("user32.dll", SetLastError:=True)>
    Public Shared Function GetGuiResources(hProcess As IntPtr, uiFlags As UInteger) As UInteger
#Enable Warning BC40027 ' Return type of function is not CLS-compliant
    End Function

    <DllImport("user32.dll")>
    Public Shared Function GetAsyncKeyState(vKey As System.Windows.Forms.Keys) As Short
    End Function

    <DllImport("user32.dll")>
    Public Shared Function GetWindowLong(hWnd As IntPtr, <MarshalAs(UnmanagedType.I4)> nIndex As WindowLongFlag) As Integer
    End Function

    <DllImport("user32.dll", EntryPoint:="GetWindowLongPtr")>
    Public Shared Function GetWindowLongPtr(ByVal hWnd As IntPtr, <MarshalAs(UnmanagedType.I4)> nIndex As WindowLongFlag) As IntPtr
    End Function

    <DllImport("user32.dll", EntryPoint:="SetWindowLongPtr")>
    Public Shared Function SetWindowLongPtr(ByVal hWnd As IntPtr, <MarshalAs(UnmanagedType.I4)> nIndex As WindowLongFlag, ByVal dwNewLong As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll")>
    Public Shared Function SetWindowLong(hWnd As IntPtr, <MarshalAs(UnmanagedType.I4)> nIndex As WindowLongFlag, dwNewLong As IntPtr) As Integer
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Public Shared Function SetParent(ByVal hWndChild As IntPtr, ByVal hWndNewParent As IntPtr) As Integer
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Public Shared Function SetWindowsHookEx(ByVal idHook As Integer, ByVal lpfn As KeyboardProcDelegate, ByVal hmod As Integer, ByVal dwThreadId As Integer) As Integer
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Public Shared Function CallNextHookEx(ByVal hHook As Integer, ByVal nCode As Integer, ByVal wParam As Integer, ByVal lParam As KEYBDINPUT) As Integer
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Public Shared Function UnhookWindowsHookEx(ByVal hHook As Integer) As Integer
    End Function

    Public Delegate Function KeyboardProcDelegate(ByVal nCode As Integer, ByVal wParam As Integer, ByRef lParam As KEYBDINPUT) As Integer

    ''' <summary>
    ''' Declaration of external SendInput method
    ''' </summary>
    <DllImport("user32.dll")>
    Public Shared Function SendInput(nInputs As UInteger, <MarshalAs(UnmanagedType.LPArray), [In]> pInputs As INPUT(), cbSize As Integer) As UInteger
    End Function


    Public Const WH_JOURNALPLAYBACK As Integer = 1, WH_GETMESSAGE As Integer = 3, WH_MOUSE As Integer = 7, WSF_VISIBLE As Integer = &H1, WM_NULL As Integer = &H0, WM_CREATE As Integer = &H1,
    WM_DELETEITEM As Integer = &H2D, WM_DESTROY As Integer = &H2, WM_MOVE As Integer = &H3, WM_SIZE As Integer = &H5, WM_ACTIVATE As Integer = &H6, WA_INACTIVE As Integer = 0,
    WA_ACTIVE As Integer = 1, WA_CLICKACTIVE As Integer = 2, WM_SETFOCUS As Integer = &H7, WM_KILLFOCUS As Integer = &H8, WM_ENABLE As Integer = &HA, WM_SETREDRAW As Integer = &HB,
    WM_SETTEXT As Integer = &HC, WM_GETTEXT As Integer = &HD, WM_GETTEXTLENGTH As Integer = &HE, WM_PAINT As Integer = &HF, WM_CLOSE As Integer = &H10, WM_QUERYENDSESSION As Integer = &H11,
    WM_QUIT As Integer = &H12, WM_QUERYOPEN As Integer = &H13, WM_ERASEBKGND As Integer = &H14, WM_SYSCOLORCHANGE As Integer = &H15, WM_ENDSESSION As Integer = &H16, WM_SHOWWINDOW As Integer = &H18,
    WM_WININICHANGE As Integer = &H1A, WM_SETTINGCHANGE As Integer = &H1A, WM_DEVMODECHANGE As Integer = &H1B, WM_ACTIVATEAPP As Integer = &H1C, WM_FONTCHANGE As Integer = &H1D, WM_TIMECHANGE As Integer = &H1E,
    WM_CANCELMODE As Integer = &H1F, WM_SETCURSOR As Integer = &H20, WM_MOUSEACTIVATE As Integer = &H21, WM_CHILDACTIVATE As Integer = &H22, WM_QUEUESYNC As Integer = &H23, WM_GETMINMAXINFO As Integer = &H24,
    WM_PAINTICON As Integer = &H26, WM_ICONERASEBKGND As Integer = &H27, WM_NEXTDLGCTL As Integer = &H28, WM_SPOOLERSTATUS As Integer = &H2A, WM_DRAWITEM As Integer = &H2B, WM_MEASUREITEM As Integer = &H2C,
    WM_VKEYTOITEM As Integer = &H2E, WM_CHARTOITEM As Integer = &H2F, WM_SETFONT As Integer = &H30, WM_GETFONT As Integer = &H31, WM_SETHOTKEY As Integer = &H32, WM_GETHOTKEY As Integer = &H33,
    WM_QUERYDRAGICON As Integer = &H37, WM_COMPAREITEM As Integer = &H39, WM_GETOBJECT As Integer = &H3D, WM_COMPACTING As Integer = &H41, WM_COMMNOTIFY As Integer = &H44, WM_WINDOWPOSCHANGING As Integer = &H46,
    WM_WINDOWPOSCHANGED As Integer = &H47, WM_POWER As Integer = &H48, WM_COPYDATA As Integer = &H4A, WM_CANCELJOURNAL As Integer = &H4B, WM_NOTIFY As Integer = &H4E, WM_INPUTLANGCHANGEREQUEST As Integer = &H50,
    WM_INPUTLANGCHANGE As Integer = &H51, WM_TCARD As Integer = &H52, WM_HELP As Integer = &H53, WM_USERCHANGED As Integer = &H54, WM_NOTIFYFORMAT As Integer = &H55, WM_CONTEXTMENU As Integer = &H7B,
    WM_STYLECHANGING As Integer = &H7C, WM_STYLECHANGED As Integer = &H7D, WM_DISPLAYCHANGE As Integer = &H7E, WM_GETICON As Integer = &H7F, WM_SETICON As Integer = &H80, WM_NCCREATE As Integer = &H81,
    WM_NCDESTROY As Integer = &H82, WM_NCCALCSIZE As Integer = &H83, WM_NCHITTEST As Integer = &H84, WM_NCPAINT As Integer = &H85, WM_NCACTIVATE As Integer = &H86, WM_GETDLGCODE As Integer = &H87,
    WM_NCMOUSEMOVE As Integer = &HA0, WM_NCMOUSELEAVE As Integer = &H2A2, WM_NCLBUTTONDOWN As Integer = &HA1, WM_NCLBUTTONUP As Integer = &HA2, WM_NCLBUTTONDBLCLK As Integer = &HA3, WM_NCRBUTTONDOWN As Integer = &HA4,
    WM_NCRBUTTONUP As Integer = &HA5, WM_NCRBUTTONDBLCLK As Integer = &HA6, WM_NCMBUTTONDOWN As Integer = &HA7, WM_NCMBUTTONUP As Integer = &HA8, WM_NCMBUTTONDBLCLK As Integer = &HA9, WM_NCXBUTTONDOWN As Integer = &HAB,
    WM_NCXBUTTONUP As Integer = &HAC, WM_NCXBUTTONDBLCLK As Integer = &HAD, WM_KEYFIRST As Integer = &H100, WM_KEYDOWN As Integer = &H100, WM_KEYUP As Integer = &H101, WM_CHAR As Integer = &H102,
    WM_DEADCHAR As Integer = &H103, WM_CTLCOLOR As Integer = &H19, WM_SYSKEYDOWN As Integer = &H104, WM_SYSKEYUP As Integer = &H105, WM_SYSCHAR As Integer = &H106, WM_SYSDEADCHAR As Integer = &H107,
    WM_KEYLAST As Integer = &H108, WM_IME_STARTCOMPOSITION As Integer = &H10D, WM_IME_ENDCOMPOSITION As Integer = &H10E, WM_IME_COMPOSITION As Integer = &H10F, WM_IME_KEYLAST As Integer = &H10F, WM_INITDIALOG As Integer = &H110,
    WM_COMMAND As Integer = &H111, WM_SYSCOMMAND As Integer = &H112, WM_TIMER As Integer = &H113, WM_HSCROLL As Integer = &H114, WM_VSCROLL As Integer = &H115, WM_INITMENU As Integer = &H116,
    WM_INITMENUPOPUP As Integer = &H117, WM_MENUSELECT As Integer = &H11F, WM_MENUCHAR As Integer = &H120, WM_ENTERIDLE As Integer = &H121, WM_UNINITMENUPOPUP As Integer = &H125, WM_CHANGEUISTATE As Integer = &H127,
    WM_UPDATEUISTATE As Integer = &H128, WM_QUERYUISTATE As Integer = &H129, WM_CTLCOLORMSGBOX As Integer = &H132, WM_CTLCOLOREDIT As Integer = &H133, WM_CTLCOLORLISTBOX As Integer = &H134, WM_CTLCOLORBTN As Integer = &H135,
    WM_CTLCOLORDLG As Integer = &H136, WM_CTLCOLORSCROLLBAR As Integer = &H137, WM_CTLCOLORSTATIC As Integer = &H138, WM_MOUSEFIRST As Integer = &H200, WM_MOUSEMOVE As Integer = &H200, WM_LBUTTONDOWN As Integer = &H201,
    WM_LBUTTONUP As Integer = &H202, WM_LBUTTONDBLCLK As Integer = &H203, WM_RBUTTONDOWN As Integer = &H204, WM_RBUTTONUP As Integer = &H205, WM_RBUTTONDBLCLK As Integer = &H206, WM_MBUTTONDOWN As Integer = &H207,
    WM_MBUTTONUP As Integer = &H208, WM_MBUTTONDBLCLK As Integer = &H209, WM_XBUTTONDOWN As Integer = &H20B, WM_XBUTTONUP As Integer = &H20C, WM_XBUTTONDBLCLK As Integer = &H20D, WM_MOUSEWHEEL As Integer = &H20A,
    WM_MOUSELAST As Integer = &H20A

    Public Const WHEEL_DELTA As Integer = 120, WM_PARENTNOTIFY As Integer = &H210, WM_ENTERMENULOOP As Integer = &H211, WM_EXITMENULOOP As Integer = &H212, WM_NEXTMENU As Integer = &H213, WM_SIZING As Integer = &H214,
            WM_CAPTURECHANGED As Integer = &H215, WM_MOVING As Integer = &H216, WM_POWERBROADCAST As Integer = &H218, WM_DEVICECHANGE As Integer = &H219, WM_IME_SETCONTEXT As Integer = &H281, WM_IME_NOTIFY As Integer = &H282,
            WM_IME_CONTROL As Integer = &H283, WM_IME_COMPOSITIONFULL As Integer = &H284, WM_IME_SELECT As Integer = &H285, WM_IME_CHAR As Integer = &H286, WM_IME_KEYDOWN As Integer = &H290, WM_IME_KEYUP As Integer = &H291,
            WM_MDICREATE As Integer = &H220, WM_MDIDESTROY As Integer = &H221, WM_MDIACTIVATE As Integer = &H222, WM_MDIRESTORE As Integer = &H223, WM_MDINEXT As Integer = &H224, WM_MDIMAXIMIZE As Integer = &H225,
            WM_MDITILE As Integer = &H226, WM_MDICASCADE As Integer = &H227, WM_MDIICONARRANGE As Integer = &H228, WM_MDIGETACTIVE As Integer = &H229, WM_MDISETMENU As Integer = &H230, WM_ENTERSIZEMOVE As Integer = &H231,
            WM_EXITSIZEMOVE As Integer = &H232, WM_DROPFILES As Integer = &H233, WM_MDIREFRESHMENU As Integer = &H234, WM_MOUSEHOVER As Integer = &H2A1, WM_MOUSELEAVE As Integer = &H2A3, WM_CUT As Integer = &H300,
            WM_COPY As Integer = &H301, WM_PASTE As Integer = &H302, WM_CLEAR As Integer = &H303, WM_UNDO As Integer = &H304, WM_RENDERFORMAT As Integer = &H305, WM_RENDERALLFORMATS As Integer = &H306,
            WM_DESTROYCLIPBOARD As Integer = &H307, WM_DRAWCLIPBOARD As Integer = &H308, WM_PAINTCLIPBOARD As Integer = &H309, WM_VSCROLLCLIPBOARD As Integer = &H30A, WM_SIZECLIPBOARD As Integer = &H30B, WM_ASKCBFORMATNAME As Integer = &H30C,
            WM_CHANGECBCHAIN As Integer = &H30D, WM_HSCROLLCLIPBOARD As Integer = &H30E, WM_QUERYNEWPALETTE As Integer = &H30F, WM_PALETTEISCHANGING As Integer = &H310, WM_PALETTECHANGED As Integer = &H311, WM_HOTKEY As Integer = &H312,
            WM_PRINT As Integer = &H317, WM_PRINTCLIENT As Integer = &H318, WM_THEMECHANGED As Integer = &H31A, WM_HANDHELDFIRST As Integer = &H358, WM_HANDHELDLAST As Integer = &H35F, WM_AFXFIRST As Integer = &H360,
            WM_AFXLAST As Integer = &H37F, WM_PENWINFIRST As Integer = &H380, WM_PENWINLAST As Integer = &H38F, WM_APP As Integer = CInt(&H8000), WM_USER As Integer = &H400, WM_REFLECT As Integer = NativeMethods.WM_USER + &H1C00,
            WS_OVERLAPPED As Integer = &H0, WS_POPUP As Integer = &H80000000, WS_CHILD As Integer = &H40000000, WS_MINIMIZE As Integer = &H20000000, WS_VISIBLE As Integer = &H10000000, WS_DISABLED As Integer = &H8000000,
            WS_CLIPSIBLINGS As Integer = &H4000000, WS_CLIPCHILDREN As Integer = &H2000000, WS_MAXIMIZE As Integer = &H1000000, WS_CAPTION As Integer = &HC00000, WS_BORDER As Integer = &H800000, WS_DLGFRAME As Integer = &H400000,
            WS_VSCROLL As Integer = &H200000, WS_HSCROLL As Integer = &H100000, WS_SYSMENU As Integer = &H80000, WS_THICKFRAME As Integer = &H40000, WS_TABSTOP As Integer = &H10000, WS_MINIMIZEBOX As Integer = &H20000,
            WS_MAXIMIZEBOX As Integer = &H10000, WS_EX_DLGMODALFRAME As Integer = &H1, WS_EX_MDICHILD As Integer = &H40, WS_EX_TOOLWINDOW As Integer = &H80, WS_EX_CLIENTEDGE As Integer = &H200, WS_EX_CONTEXTHELP As Integer = &H400,
            WS_EX_RIGHT As Integer = &H1000, WS_EX_LEFT As Integer = &H0, WS_EX_RTLREADING As Integer = &H2000, WS_EX_LEFTSCROLLBAR As Integer = &H4000, WS_EX_CONTROLPARENT As Integer = &H10000, WS_EX_STATICEDGE As Integer = &H20000,
            WS_EX_APPWINDOW As Integer = &H40000, WS_EX_LAYERED As Integer = &H80000, WS_EX_TOPMOST As Integer = &H8, WS_EX_LAYOUTRTL As Integer = &H400000, WS_EX_NOINHERITLAYOUT As Integer = &H100000, WPF_SETMINPOSITION As Integer = &H1,
            WM_CHOOSEFONT_GETLOGFONT As Integer = (&H400 + 1)

    Public Const TRANSPARENT As Integer = 1, OPAQUE As Integer = 2, TME_HOVER As Integer = &H1, TME_LEAVE As Integer = &H2, TPM_LEFTBUTTON As Integer = &H0, TPM_RIGHTBUTTON As Integer = &H2,
        TPM_LEFTALIGN As Integer = &H0, TPM_RIGHTALIGN As Integer = &H8, TPM_VERTICAL As Integer = &H40, TV_FIRST As Integer = &H1100, TBSTATE_CHECKED As Integer = &H1, TBSTATE_ENABLED As Integer = &H4,
        TBSTATE_HIDDEN As Integer = &H8, TBSTATE_INDETERMINATE As Integer = &H10, TBSTYLE_BUTTON As Integer = &H0, TBSTYLE_SEP As Integer = &H1, TBSTYLE_CHECK As Integer = &H2, TBSTYLE_DROPDOWN As Integer = &H8,
        TBSTYLE_TOOLTIPS As Integer = &H100, TBSTYLE_FLAT As Integer = &H800, TBSTYLE_LIST As Integer = &H1000, TBSTYLE_EX_DRAWDDARROWS As Integer = &H1, TB_ENABLEBUTTON As Integer = (&H400 + 1), TB_ISBUTTONCHECKED As Integer = (&H400 + 10),
        TB_ISBUTTONINDETERMINATE As Integer = (&H400 + 13), TB_ADDBUTTONSA As Integer = (&H400 + 20), TB_ADDBUTTONSW As Integer = (&H400 + 68), TB_INSERTBUTTONA As Integer = (&H400 + 21), TB_INSERTBUTTONW As Integer = (&H400 + 67), TB_DELETEBUTTON As Integer = (&H400 + 22),
        TB_GETBUTTON As Integer = (&H400 + 23), TB_SAVERESTOREA As Integer = (&H400 + 26), TB_SAVERESTOREW As Integer = (&H400 + 76), TB_ADDSTRINGA As Integer = (&H400 + 28), TB_ADDSTRINGW As Integer = (&H400 + 77), TB_BUTTONSTRUCTSIZE As Integer = (&H400 + 30),
        TB_SETBUTTONSIZE As Integer = (&H400 + 31), TB_AUTOSIZE As Integer = (&H400 + 33), TB_GETROWS As Integer = (&H400 + 40), TB_GETBUTTONTEXTA As Integer = (&H400 + 45), TB_GETBUTTONTEXTW As Integer = (&H400 + 75), TB_SETIMAGELIST As Integer = (&H400 + 48),
        TB_GETRECT As Integer = (&H400 + 51), TB_GETBUTTONSIZE As Integer = (&H400 + 58), TB_GETBUTTONINFOW As Integer = (&H400 + 63), TB_SETBUTTONINFOW As Integer = (&H400 + 64), TB_GETBUTTONINFOA As Integer = (&H400 + 65), TB_SETBUTTONINFOA As Integer = (&H400 + 66),
        TB_MAPACCELERATORA As Integer = (&H400 + 78), TB_SETEXTENDEDSTYLE As Integer = (&H400 + 84), TB_MAPACCELERATORW As Integer = (&H400 + 90), TB_GETTOOLTIPS As Integer = (&H400 + 35), TB_SETTOOLTIPS As Integer = (&H400 + 36), TBIF_IMAGE As Integer = &H1,
        TBIF_TEXT As Integer = &H2, TBIF_STATE As Integer = &H4, TBIF_STYLE As Integer = &H8, TBIF_COMMAND As Integer = &H20, TBIF_SIZE As Integer = &H40, TBN_GETBUTTONINFOA As Integer = ((0 - 700) - 0),
        TBN_GETBUTTONINFOW As Integer = ((0 - 700) - 20), TBN_QUERYINSERT As Integer = ((0 - 700) - 6), TBN_DROPDOWN As Integer = ((0 - 700) - 10), TBN_HOTITEMCHANGE As Integer = ((0 - 700) - 13), TBN_GETDISPINFOA As Integer = ((0 - 700) - 16), TBN_GETDISPINFOW As Integer = ((0 - 700) - 17),
        TBN_GETINFOTIPA As Integer = ((0 - 700) - 18), TBN_GETINFOTIPW As Integer = ((0 - 700) - 19), TTS_ALWAYSTIP As Integer = &H1, TTS_NOPREFIX As Integer = &H2, TTS_NOANIMATE As Integer = &H10, TTS_NOFADE As Integer = &H20,
        TTS_BALLOON As Integer = &H40, TTI_WARNING As Integer = 2, TTF_IDISHWND As Integer = &H1, TTF_RTLREADING As Integer = &H4, TTF_TRACK As Integer = &H20, TTF_CENTERTIP As Integer = &H2,
        TTF_SUBCLASS As Integer = &H10, TTF_TRANSPARENT As Integer = &H100, TTF_ABSOLUTE As Integer = &H80, TTDT_AUTOMATIC As Integer = 0, TTDT_RESHOW As Integer = 1, TTDT_AUTOPOP As Integer = 2,
        TTDT_INITIAL As Integer = 3, TTM_TRACKACTIVATE As Integer = (&H400 + 17), TTM_TRACKPOSITION As Integer = (&H400 + 18), TTM_ACTIVATE As Integer = (&H400 + 1), TTM_POP As Integer = (&H400 + 28), TTM_ADJUSTRECT As Integer = (&H400 + 31),
        TTM_SETDELAYTIME As Integer = (&H400 + 3), TTM_SETTITLEA As Integer = (WM_USER + 32), TTM_SETTITLEW As Integer = (WM_USER + 33), TTM_ADDTOOLA As Integer = (&H400 + 4), TTM_ADDTOOLW As Integer = (&H400 + 50), TTM_DELTOOLA As Integer = (&H400 + 5),
        TTM_DELTOOLW As Integer = (&H400 + 51), TTM_NEWTOOLRECTA As Integer = (&H400 + 6), TTM_NEWTOOLRECTW As Integer = (&H400 + 52), TTM_RELAYEVENT As Integer = (&H400 + 7), TTM_GETTIPBKCOLOR As Integer = (&H400 + 22), TTM_SETTIPBKCOLOR As Integer = (&H400 + 19),
        TTM_SETTIPTEXTCOLOR As Integer = (&H400 + 20), TTM_GETTIPTEXTCOLOR As Integer = (&H400 + 23), TTM_GETTOOLINFOA As Integer = (&H400 + 8), TTM_GETTOOLINFOW As Integer = (&H400 + 53), TTM_SETTOOLINFOA As Integer = (&H400 + 9), TTM_SETTOOLINFOW As Integer = (&H400 + 54),
        TTM_HITTESTA As Integer = (&H400 + 10), TTM_HITTESTW As Integer = (&H400 + 55), TTM_GETTEXTA As Integer = (&H400 + 11), TTM_GETTEXTW As Integer = (&H400 + 56), TTM_UPDATE As Integer = (&H400 + 29), TTM_UPDATETIPTEXTA As Integer = (&H400 + 12),
        TTM_UPDATETIPTEXTW As Integer = (&H400 + 57), TTM_ENUMTOOLSA As Integer = (&H400 + 14), TTM_ENUMTOOLSW As Integer = (&H400 + 58), TTM_GETCURRENTTOOLA As Integer = (&H400 + 15), TTM_GETCURRENTTOOLW As Integer = (&H400 + 59), TTM_WINDOWFROMPOINT As Integer = (&H400 + 16),
        TTM_GETDELAYTIME As Integer = (&H400 + 21), TTM_SETMAXTIPWIDTH As Integer = (&H400 + 24), TTN_GETDISPINFOA As Integer = ((0 - 520) - 0), TTN_GETDISPINFOW As Integer = ((0 - 520) - 10), TTN_SHOW As Integer = ((0 - 520) - 1), TTN_POP As Integer = ((0 - 520) - 2),
        TTN_NEEDTEXTA As Integer = ((0 - 520) - 0), TTN_NEEDTEXTW As Integer = ((0 - 520) - 10), TBS_AUTOTICKS As Integer = &H1, TBS_VERT As Integer = &H2, TBS_TOP As Integer = &H4, TBS_BOTTOM As Integer = &H0,
        TBS_BOTH As Integer = &H8, TBS_NOTICKS As Integer = &H10, TBM_GETPOS As Integer = (&H400), TBM_SETTIC As Integer = (&H400 + 4), TBM_SETPOS As Integer = (&H400 + 5), TBM_SETRANGE As Integer = (&H400 + 6),
        TBM_SETRANGEMIN As Integer = (&H400 + 7), TBM_SETRANGEMAX As Integer = (&H400 + 8), TBM_SETTICFREQ As Integer = (&H400 + 20), TBM_SETPAGESIZE As Integer = (&H400 + 21), TBM_SETLINESIZE As Integer = (&H400 + 23), TB_LINEUP As Integer = 0,
        TB_LINEDOWN As Integer = 1, TB_PAGEUP As Integer = 2, TB_PAGEDOWN As Integer = 3, TB_THUMBPOSITION As Integer = 4, TB_THUMBTRACK As Integer = 5, TB_TOP As Integer = 6,
        TB_BOTTOM As Integer = 7, TB_ENDTRACK As Integer = 8, TVS_HASBUTTONS As Integer = &H1, TVS_HASLINES As Integer = &H2, TVS_LINESATROOT As Integer = &H4, TVS_EDITLABELS As Integer = &H8,
        TVS_SHOWSELALWAYS As Integer = &H20, TVS_RTLREADING As Integer = &H40, TVS_CHECKBOXES As Integer = &H100, TVS_TRACKSELECT As Integer = &H200, TVS_FULLROWSELECT As Integer = &H1000, TVS_NONEVENHEIGHT As Integer = &H4000,
        TVS_INFOTIP As Integer = &H800, TVS_NOTOOLTIPS As Integer = &H80, TVIF_TEXT As Integer = &H1, TVIF_IMAGE As Integer = &H2, TVIF_PARAM As Integer = &H4, TVIF_STATE As Integer = &H8,
        TVIF_HANDLE As Integer = &H10, TVIF_SELECTEDIMAGE As Integer = &H20, TVIS_SELECTED As Integer = &H2, TVIS_EXPANDED As Integer = &H20, TVIS_EXPANDEDONCE As Integer = &H40, TVIS_STATEIMAGEMASK As Integer = &HF000,
        TVI_ROOT As Integer = &HFFFF0000, TVI_FIRST As Integer = &HFFFF0001, TVM_INSERTITEMA As Integer = (&H1100 + 0), TVM_INSERTITEMW As Integer = (&H1100 + 50), TVM_DELETEITEM As Integer = (&H1100 + 1), TVM_EXPAND As Integer = (&H1100 + 2),
        TVE_COLLAPSE As Integer = &H1, TVE_EXPAND As Integer = &H2, TVM_GETITEMRECT As Integer = (&H1100 + 4), TVM_GETINDENT As Integer = (&H1100 + 6), TVM_SETINDENT As Integer = (&H1100 + 7), TVM_GETIMAGELIST As Integer = (&H1100 + 8),
        TVM_SETIMAGELIST As Integer = (&H1100 + 9), TVM_GETNEXTITEM As Integer = (&H1100 + 10), TVGN_NEXT As Integer = &H1, TVGN_PREVIOUS As Integer = &H2, TVGN_FIRSTVISIBLE As Integer = &H5, TVGN_NEXTVISIBLE As Integer = &H6,
        TVGN_PREVIOUSVISIBLE As Integer = &H7, TVGN_DROPHILITE As Integer = &H8, TVGN_CARET As Integer = &H9, TVM_SELECTITEM As Integer = (&H1100 + 11), TVM_GETITEMA As Integer = (&H1100 + 12), TVM_GETITEMW As Integer = (&H1100 + 62),
        TVM_SETITEMA As Integer = (&H1100 + 13), TVM_SETITEMW As Integer = (&H1100 + 63), TVM_EDITLABELA As Integer = (&H1100 + 14), TVM_EDITLABELW As Integer = (&H1100 + 65), TVM_GETEDITCONTROL As Integer = (&H1100 + 15), TVM_GETVISIBLECOUNT As Integer = (&H1100 + 16),
        TVM_HITTEST As Integer = (&H1100 + 17), TVM_ENSUREVISIBLE As Integer = (&H1100 + 20), TVM_ENDEDITLABELNOW As Integer = (&H1100 + 22), TVM_GETISEARCHSTRINGA As Integer = (&H1100 + 23), TVM_GETISEARCHSTRINGW As Integer = (&H1100 + 64), TVM_SETITEMHEIGHT As Integer = (&H1100 + 27),
        TVM_GETITEMHEIGHT As Integer = (&H1100 + 28), TVN_SELCHANGINGA As Integer = ((0 - 400) - 1), TVN_SELCHANGINGW As Integer = ((0 - 400) - 50), TVN_GETINFOTIPA As Integer = ((0 - 400) - 13), TVN_GETINFOTIPW As Integer = ((0 - 400) - 14), TVN_SELCHANGEDA As Integer = ((0 - 400) - 2),
        TVN_SELCHANGEDW As Integer = ((0 - 400) - 51), TVC_UNKNOWN As Integer = &H0, TVC_BYMOUSE As Integer = &H1, TVC_BYKEYBOARD As Integer = &H2, TVN_GETDISPINFOA As Integer = ((0 - 400) - 3), TVN_GETDISPINFOW As Integer = ((0 - 400) - 52),
        TVN_SETDISPINFOA As Integer = ((0 - 400) - 4), TVN_SETDISPINFOW As Integer = ((0 - 400) - 53), TVN_ITEMEXPANDINGA As Integer = ((0 - 400) - 5), TVN_ITEMEXPANDINGW As Integer = ((0 - 400) - 54), TVN_ITEMEXPANDEDA As Integer = ((0 - 400) - 6), TVN_ITEMEXPANDEDW As Integer = ((0 - 400) - 55),
        TVN_BEGINDRAGA As Integer = ((0 - 400) - 7), TVN_BEGINDRAGW As Integer = ((0 - 400) - 56), TVN_BEGINRDRAGA As Integer = ((0 - 400) - 8), TVN_BEGINRDRAGW As Integer = ((0 - 400) - 57), TVN_BEGINLABELEDITA As Integer = ((0 - 400) - 10), TVN_BEGINLABELEDITW As Integer = ((0 - 400) - 59),
        TVN_ENDLABELEDITA As Integer = ((0 - 400) - 11), TVN_ENDLABELEDITW As Integer = ((0 - 400) - 60), TCS_BOTTOM As Integer = &H2, TCS_RIGHT As Integer = &H2, TCS_FLATBUTTONS As Integer = &H8, TCS_HOTTRACK As Integer = &H40,
        TCS_VERTICAL As Integer = &H80, TCS_TABS As Integer = &H0, TCS_BUTTONS As Integer = &H100, TCS_MULTILINE As Integer = &H200, TCS_RIGHTJUSTIFY As Integer = &H0, TCS_FIXEDWIDTH As Integer = &H400,
        TCS_RAGGEDRIGHT As Integer = &H800, TCS_OWNERDRAWFIXED As Integer = &H2000, TCS_TOOLTIPS As Integer = &H4000, TCM_SETIMAGELIST As Integer = (&H1300 + 3), TCIF_TEXT As Integer = &H1, TCIF_IMAGE As Integer = &H2,
        TCM_GETITEMA As Integer = (&H1300 + 5), TCM_GETITEMW As Integer = (&H1300 + 60), TCM_SETITEMA As Integer = (&H1300 + 6), TCM_SETITEMW As Integer = (&H1300 + 61), TCM_INSERTITEMA As Integer = (&H1300 + 7), TCM_INSERTITEMW As Integer = (&H1300 + 62),
        TCM_DELETEITEM As Integer = (&H1300 + 8), TCM_DELETEALLITEMS As Integer = (&H1300 + 9), TCM_GETITEMRECT As Integer = (&H1300 + 10), TCM_GETCURSEL As Integer = (&H1300 + 11), TCM_SETCURSEL As Integer = (&H1300 + 12), TCM_ADJUSTRECT As Integer = (&H1300 + 40),
        TCM_SETITEMSIZE As Integer = (&H1300 + 41), TCM_SETPADDING As Integer = (&H1300 + 43), TCM_GETROWCOUNT As Integer = (&H1300 + 44), TCM_GETTOOLTIPS As Integer = (&H1300 + 45), TCM_SETTOOLTIPS As Integer = (&H1300 + 46), TCN_SELCHANGE As Integer = ((0 - 550) - 1),
        TCN_SELCHANGING As Integer = ((0 - 550) - 2), TBSTYLE_WRAPPABLE As Integer = &H200, TVM_SETBKCOLOR As Integer = (TV_FIRST + 29), TVM_SETTEXTCOLOR As Integer = (TV_FIRST + 30), TYMED_NULL As Integer = 0, TVM_GETLINECOLOR As Integer = (TV_FIRST + 41),
        TVM_SETLINECOLOR As Integer = (TV_FIRST + 40), TVM_SETTOOLTIPS As Integer = (TV_FIRST + 24), TVSIL_STATE As Integer = 2, TVM_SORTCHILDRENCB As Integer = (TV_FIRST + 21), TMPF_FIXED_PITCH As Integer = &H1

    Public Shared Sub SetNativeEnabled(hWnd As IntPtr, enabled As Boolean)
        NativeMethods.SetWindowLongPtr(hWnd, WindowLongFlag.GWL_STYLE, CType(NativeMethods.GetWindowLongPtr(hWnd, WindowLongFlag.GWL_STYLE).ToInt64 And Not NativeMethods.WS_DISABLED Or (If(enabled, 0, NativeMethods.WS_DISABLED)), IntPtr))
    End Sub

End Class
