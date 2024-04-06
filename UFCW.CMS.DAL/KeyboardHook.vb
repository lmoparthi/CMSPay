'Option Strict On

'Imports System.Diagnostics
'Imports System.Runtime.InteropServices
'Imports System.Windows.Forms

'Public Class KeyboardHook

'    Private Const HC_ACTION As Integer = 0
'    Private Const WM_KEYDOWN As Integer = &H100
'    Private Const WM_KEYUP As Integer = &H101
'    Private Const WM_SYSKEYDOWN As Integer = &H104
'    Private Const WM_SYSKEYUP As Integer = &H105

'    <DllImport("user32.dll", SetLastError:=True)>
'    Private Shared Function SetWindowsHookEx(ByVal hookType As HookType, ByVal lpfn As KeyboardProcDelegate, ByVal hMod As IntPtr, ByVal dwThreadId As UInteger) As IntPtr
'    End Function

'    <DllImport("user32.dll", SetLastError:=True)>
'    Private Shared Function CallNextHookEx(ByVal hhk As IntPtr, ByVal nCode As Integer, ByVal wParam As UInteger, <[In]()> ByRef lParam As KBDLLHOOKSTRUCT) As IntPtr
'    End Function

'    <DllImport("user32.dll", SetLastError:=True)>
'    Private Shared Function UnhookWindowsHookEx(ByVal hhk As IntPtr) As Boolean
'    End Function

'    Private Delegate Function KeyboardProcDelegate(ByVal nCode As Integer, ByVal wParam As UInteger, ByRef lParam As KBDLLHOOKSTRUCT) As IntPtr

'    Private Shared KeyHook As IntPtr
'    Private Shared KeyHookDelegate As KeyboardProcDelegate

'    Public Shared Event KeyDown(ByVal Key As Keys)
'    Public Shared Event KeyUp(ByVal Key As Keys)

'    Shared Sub New() ' Installs The Hook

'        KeyHookDelegate = New KeyboardProcDelegate(AddressOf KeyboardProc)
'        KeyHook = SetWindowsHookEx(HookType.WH_KEYBOARD_LL, KeyHookDelegate, IntPtr.Zero, 0)

'    End Sub

'    Private Shared Function KeyboardProc(ByVal nCode As Integer, ByVal wParam As UInteger, ByRef lParam As KBDLLHOOKSTRUCT) As IntPtr

'        If (nCode = HC_ACTION) Then
'            Select Case wParam
'                Case WM_KEYDOWN, WM_SYSKEYDOWN
'                    Debug.Print($"KeyCode {lParam.vkCode}")

'                    If CMSDALCommon.EnvironmentOverride Is Nothing Then

'                        If (My.Computer.Keyboard.ShiftKeyDown AndAlso My.Computer.Keyboard.CtrlKeyDown) Then

'                            Debug.Print($"ShiftCtrlKeyDown")

'                            Select Case lParam.vkCode
'                                Case Keys.Q
'                                    CMSDALCommon.EnvironmentOverride = "Q"
'                                Case Keys.D
'                                    CMSDALCommon.EnvironmentOverride = "D"
'                                Case Keys.T
'                                    CMSDALCommon.EnvironmentOverride = "T"
'                            End Select

'                        ElseIf My.Computer.Keyboard.CtrlKeyDown Then

'                            Debug.Print($"CtrlKeyDown")

'                            '    If Key = Keys.Q Then

'                            '        CMSDALCommon.EnvironmentOverride = "Q"
'                            '        Debug.Print($"KeyCode {Key}")
'                            '    End If

'                        ElseIf My.Computer.Keyboard.ShiftKeyDown Then

'                            Debug.Print($"ShiftKeyDown")

'                            '    If Key = Keys.Q Then

'                            '        CMSDALCommon.EnvironmentOverride = "Q"
'                            '        Debug.Print($"KeyCode {Key}")
'                            '    End If


'                        ElseIf My.Computer.Keyboard.AltKeyDown Then
'                            Debug.Print($"AltKeyDown")

'                            '    If Key = Keys.Q Then

'                            '        CMSDALCommon.EnvironmentOverride = "Q"
'                            '        Debug.Print($"KeyCode {Key}")
'                            '    End If

'                        End If
'                    End If

'                    If CMSDALCommon.EnvironmentOverride IsNot Nothing Then
'                        UnhookWindowsHookEx(KeyHook)   'Un-Hooks When Program Closes
'                    End If

'                Case WM_KEYUP, WM_SYSKEYUP
'            End Select
'        End If

'        Return CallNextHookEx(KeyHook, nCode, wParam, lParam)
'    End Function
'    Protected Overrides Sub Finalize()

'        UnhookWindowsHookEx(KeyHook)   'Un-Hooks When Program Closes
'        MyBase.Finalize()
'    End Sub
'End Class

'Enum HookType As Integer
'    WH_JOURNALRECORD = 0
'    WH_JOURNALPLAYBACK = 1
'    WH_KEYBOARD = 2
'    WH_GETMESSAGE = 3
'    WH_CALLWNDPROC = 4
'    WH_CBT = 5
'    WH_SYSMSGFILTER = 6
'    WH_MOUSE = 7
'    WH_HARDWARE = 8
'    WH_DEBUG = 9
'    WH_SHELL = 10
'    WH_FOREGROUNDIDLE = 11
'    WH_CALLWNDPROCRET = 12
'    WH_KEYBOARD_LL = 13
'    WH_MOUSE_LL = 14
'End Enum

'Public Structure KBDLLHOOKSTRUCT
'    Public vkCode As Integer
'    Public scancode As Integer
'    Public flags As Integer
'    Public time As Integer
'    Public dwExtraInfo As Integer
'End Structure
