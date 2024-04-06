Imports System.Windows.Forms

Public NotInheritable Class GlobalCursor
    Implements IDisposable

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCursor", "Trace Switch in App.Config")

    'Do not use this in background worker threads as the cursor could be unset unexpectedly when the thread finishes.
    ' '' ''Using WC As New WaitCursor
    ' '' ''    ' do your long operation
    ' '' ''    ' optionally you can change the cursor at any time with e.g.:
    ' '' ''    WC.SetCursorAllForms(Cursors.Help)
    ' '' ''    ' complete your long operation
    ' '' ''End Using
    Sub New()

        SetCursorAllForms(Cursors.WaitCursor)

    End Sub

    Public Shared Sub SetCursorAllForms(ByVal cursor As Cursor)

        For Each Form As Form In Application.OpenForms.OfType(Of Form)().ToList()
            If Form.Cursor <> cursor Then

                If Form.InvokeRequired Then
                    Form.SetCursor(cursor)
                Else
                    Form.Cursor = cursor
                End If

            End If
        Next
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Class must implement Idisposable.Dispose() so that
        ' Using construct may be employed

        SetCursorAllForms(Cursors.Default)

    End Sub

End Class
