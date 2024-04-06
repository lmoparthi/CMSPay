Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports System.Threading
Imports System.Reflection

Public Module FormExtensions

    Private _TraceSwitch As New BooleanSwitch("TraceCursor", "Trace Switch in App.Config")

    Sub New()
    End Sub

    Private Delegate Sub SetCursorDelegate(control As Control, cursor As Cursor)

    <System.Runtime.CompilerServices.Extension> _
    Public Sub SetCursor(control As Control, cursor As Cursor)

        If control.Cursor <> cursor Then

            If control.InvokeRequired Then

                'If _TraceSwitch.Enabled Then Trace.WriteLine(Generals.NowDate.ToString("HH:mm:ss.fffffff") & " I: Thread " & Thread.CurrentThread.ManagedThreadId.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", MethodBase.GetCurrentMethod().DeclaringType.ToString)
                control.Invoke(New SetCursorDelegate(AddressOf SetCursor), New Object() {control, cursor})
            Else
                'If _TraceSwitch.Enabled Then Trace.WriteLine(Generals.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", MethodBase.GetCurrentMethod().DeclaringType.ToString)

                control.Cursor = cursor
            End If

        End If

    End Sub

End Module

