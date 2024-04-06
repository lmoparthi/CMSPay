Imports System.Runtime.InteropServices

Public Class DataGridBoolColumn
    Inherits System.Windows.Forms.DataGridBoolColumn
    Enum KeyState As Integer
        VK_LBUTTON = &H1
        VK_RBUTTON = &H2
        VK_CANCEL = &H3
        VK_MBUTTON = &H4
        '
        VK_XBUTTON1 = &H5
        VK_XBUTTON2 = &H6
        '
        VK_BACK = &H8
        VK_TAB = &H9
        '
        VK_CLEAR = &HC
        VK_RETURN = &HD
        '
        VK_SHIFT = &H10
        VK_CONTROL = &H11
        VK_MENU = &H12
        VK_PAUSE = &H13
        VK_CAPITAL = &H14
        '
        VK_KANA = &H15
        VK_HANGEUL = &H15
        ' old name - should be here for compatibility 
        VK_HANGUL = &H15
        VK_JUNJA = &H17
        VK_FINAL = &H18
        VK_HANJA = &H19
        VK_KANJI = &H19
        '
        VK_ESCAPE = &H1B
        '
        VK_CONVERT = &H1C
        VK_NONCONVERT = &H1D
        VK_ACCEPT = &H1E
        VK_MODECHANGE = &H1F
        '
        VK_SPACE = &H20
        VK_PRIOR = &H21
        VK_NEXT = &H22
        VK_END = &H23
        VK_HOME = &H24
        VK_LEFT = &H25
        VK_UP = &H26
        VK_RIGHT = &H27
        VK_DOWN = &H28
        VK_SELECT = &H29
        VK_PRINT = &H2A
        VK_EXECUTE = &H2B
        VK_SNAPSHOT = &H2C
        VK_INSERT = &H2D
        VK_DELETE = &H2E
        VK_HELP = &H2F
        '
        VK_LWIN = &H5B
        VK_RWIN = &H5C
        VK_APPS = &H5D
        '
        VK_SLEEP = &H5F
        '
        VK_NUMPAD0 = &H60
        VK_NUMPAD1 = &H61
        VK_NUMPAD2 = &H62
        VK_NUMPAD3 = &H63
        VK_NUMPAD4 = &H64
        VK_NUMPAD5 = &H65
        VK_NUMPAD6 = &H66
        VK_NUMPAD7 = &H67
        VK_NUMPAD8 = &H68
        VK_NUMPAD9 = &H69
        VK_MULTIPLY = &H6A
        VK_ADD = &H6B
        VK_SEPARATOR = &H6C
        VK_SUBTRACT = &H6D
        VK_DECIMAL = &H6E
        VK_DIVIDE = &H6F
        VK_F1 = &H70
        VK_F2 = &H71
        VK_F3 = &H72
        VK_F4 = &H73
        VK_F5 = &H74
        VK_F6 = &H75
        VK_F7 = &H76
        VK_F8 = &H77
        VK_F9 = &H78
        VK_F10 = &H79
        VK_F11 = &H7A
        VK_F12 = &H7B
        VK_F13 = &H7C
        VK_F14 = &H7D
        VK_F15 = &H7E
        VK_F16 = &H7F
        VK_F17 = &H80
        VK_F18 = &H81
        VK_F19 = &H82
        VK_F20 = &H83
        VK_F21 = &H84
        VK_F22 = &H85
        VK_F23 = &H86
        VK_F24 = &H87
        '
        VK_NUMLOCK = &H90
        VK_SCROLL = &H91
        '
        VK_OEM_NEC_EQUAL = &H92
        ' '=' key on numpad
        '
        VK_OEM_FJ_JISHO = &H92
        ' 'Dictionary' key
        VK_OEM_FJ_MASSHOU = &H93
        ' 'Unregister word' key
        VK_OEM_FJ_TOUROKU = &H94
        ' 'Register word' key
        VK_OEM_FJ_LOYA = &H95
        ' 'Left OYAYUBI' key
        VK_OEM_FJ_ROYA = &H96
        ' 'Right OYAYUBI' key
        '
        VK_LSHIFT = &HA0
        VK_RSHIFT = &HA1
        VK_LCONTROL = &HA2
        VK_RCONTROL = &HA3
        VK_LMENU = &HA4
        VK_RMENU = &HA5
        '
        VK_BROWSER_BACK = &HA6
        VK_BROWSER_FORWARD = &HA7
        VK_BROWSER_REFRESH = &HA8
        VK_BROWSER_STOP = &HA9
        VK_BROWSER_SEARCH = &HAA
        VK_BROWSER_FAVORITES = &HAB
        VK_BROWSER_HOME = &HAC
        '
        VK_VOLUME_MUTE = &HAD
        VK_VOLUME_DOWN = &HAE
        VK_VOLUME_UP = &HAF
        VK_MEDIA_NEXT_TRACK = &HB0
        VK_MEDIA_PREV_TRACK = &HB1
        VK_MEDIA_STOP = &HB2
        VK_MEDIA_PLAY_PAUSE = &HB3
        VK_LAUNCH_MAIL = &HB4
        VK_LAUNCH_MEDIA_SELECT = &HB5
        VK_LAUNCH_APP1 = &HB6
        VK_LAUNCH_APP2 = &HB7
        '
        VK_OEM_1 = &HBA
        ' ';:' for US
        VK_OEM_PLUS = &HBB
        ' '+' any country
        VK_OEM_COMMA = &HBC
        ' ',' any country
        VK_OEM_MINUS = &HBD
        ' '-' any country
        VK_OEM_PERIOD = &HBE
        ' '.' any country
        VK_OEM_2 = &HBF
        ' '/?' for US
        VK_OEM_3 = &HC0
        ' '`~' for US
        '
        VK_OEM_4 = &HDB
        '  '[{' for US
        VK_OEM_5 = &HDC
        '  '\|' for US
        VK_OEM_6 = &HDD
        '  ']}' for US
        VK_OEM_7 = &HDE
        '  ''"' for US
        VK_OEM_8 = &HDF
        '
        VK_OEM_AX = &HE1
        '  'AX' key on Japanese AX kbd
        VK_OEM_102 = &HE2
        '  "<>" or "\|" on RT 102-key kbd.
        VK_ICO_HELP = &HE3
        '  Help key on ICO
        VK_ICO_00 = &HE4
        '  00 key on ICO
        '
        VK_PROCESSKEY = &HE5
        '
        VK_ICO_CLEAR = &HE6
        '
        VK_PACKET = &HE7
        '
        VK_OEM_RESET = &HE9
        VK_OEM_JUMP = &HEA
        VK_OEM_PA1 = &HEB
        VK_OEM_PA2 = &HEC
        VK_OEM_PA3 = &HED
        VK_OEM_WSCTRL = &HEE
        VK_OEM_CUSEL = &HEF
        VK_OEM_ATTN = &HF0
        VK_OEM_FINISH = &HF1
        VK_OEM_COPY = &HF2
        VK_OEM_AUTO = &HF3
        VK_OEM_ENLW = &HF4
        VK_OEM_BACKTAB = &HF5
        '
        VK_ATTN = &HF6
        VK_CRSEL = &HF7
        VK_EXSEL = &HF8
        VK_EREOF = &HF9
        VK_PLAY = &HFA
        VK_ZOOM = &HFB
        VK_NONAME = &HFC
        VK_PA1 = &HFD
        VK_OEM_CLEAR = &HFE
    End Enum

    Public Delegate Sub BoolValueChangedEventHandler(ByVal sender As Object, ByVal e As BoolValueChangedEventArgs)

    Private _SaveValue As Object
    Private _SaveRow As Integer
    Private _LockValue As Boolean
    Private _IsBeingEdited As Boolean

    Public Const VK_SPACE As Integer = 32

    Public Event BoolValueChanged As BoolValueChangedEventHandler

    <System.ComponentModel.Description("Gets or Sets the Column Position")>
    Public Overridable Property Position() As Integer
        Get
            If Me.DataGridTableStyle IsNot Nothing Then
                Return Me.DataGridTableStyle.GridColumnStyles.IndexOf(Me)
            Else
                Return -1
            End If
        End Get
        Set(ByVal Value As Integer)
            If Me.DataGridTableStyle Is Nothing Then Exit Property
            If Me.DataGridTableStyle.DataGrid Is Nothing Then Exit Property

            CType(Me.DataGridTableStyle.DataGrid, DataGridCustom).MoveColumn(Me.DataGridTableStyle.GridColumnStyles.IndexOf(Me), Value)
        End Set
    End Property

    Public Sub New()
        MyBase.New()
        _SaveRow = -(1)
        _LockValue = False
        _IsBeingEdited = False
    End Sub

    Protected Overloads Overrides Sub Paint(ByVal g As Graphics, ByVal bounds As Rectangle, ByVal source As CurrencyManager, ByVal rowNum As Integer, ByVal backBrush As Brush, ByVal foreBrush As Brush, ByVal alignToRight As Boolean)

        Dim Changing As Boolean
        Dim MousePos As Point
        Dim DG As DataGrid
        Dim IsClickInCell As Boolean

        Try

            MousePos = Me.DataGridTableStyle.DataGrid.PointToClient(Control.MousePosition)
            DG = Me.DataGridTableStyle.DataGrid
            IsClickInCell = ((Control.MouseButtons = System.Windows.Forms.MouseButtons.Left) AndAlso DG.GetCellBounds(DG.CurrentCell).Contains(MousePos))

            Changing = DG.Focused AndAlso (IsClickInCell OrElse NativeMethods.GetKeyState(VK_SPACE) < 0)   ' or spacebar

            If ((Not _LockValue) AndAlso _IsBeingEdited AndAlso Changing AndAlso _SaveRow = rowNum) Then
                _SaveValue = MyBase.GetColumnValueAtRow(source, rowNum)
                _LockValue = False
                Dim e As New BoolValueChangedEventArgs(rowNum, Me.DataGridTableStyle.GridColumnStyles.IndexOf(Me), _SaveValue)
                RaiseEvent BoolValueChanged(Me, e)
            End If

            If (_SaveRow = rowNum) Then
                _LockValue = False
            End If

            MyBase.Paint(g, bounds, source, rowNum, backBrush, foreBrush, alignToRight)

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Protected Overloads Overrides Sub Edit(ByVal source As CurrencyManager, ByVal rowNum As Integer, ByVal bounds As Rectangle, ByVal [readOnly] As Boolean, ByVal instantText As String, ByVal cellIsVisible As Boolean)
        _LockValue = True
        _IsBeingEdited = True
        _SaveRow = rowNum
        _SaveValue = MyBase.GetColumnValueAtRow(source, rowNum)

        MyBase.Edit(source, rowNum, bounds, [readOnly], instantText, cellIsVisible)
    End Sub
    Protected Overloads Overrides Function Commit(ByVal dataSource As CurrencyManager, ByVal rowNum As Integer) As Boolean
        _LockValue = True
        _IsBeingEdited = False
        Return MyBase.Commit(dataSource, rowNum)
    End Function

End Class
