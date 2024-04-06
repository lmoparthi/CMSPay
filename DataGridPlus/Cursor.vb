Option Strict On

Public Class WaitCursor
    Implements IDisposable

    Private ReadOnly _PreviousCursor As Cursor
    Private _Disposing As Boolean = False

    ' To detect redundant call 
    ''' <summary>
    ''' .cteur
    ''' </summary>
    Public Sub New()
        _PreviousCursor = Cursor.Current
        Cursor.Current = Cursors.WaitCursor
    End Sub

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not _Disposing Then
            If disposing Then
                Cursor.Current = _PreviousCursor
            End If

            _Disposing = True
        End If
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub
End Class

Public Class HourGlass
    Implements IDisposable

    Private _Disposing As Boolean = False ' To detect redundant calls

    Public Sub New()
        Enabled = True
    End Sub

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not _Disposing Then
            If disposing Then
                Enabled = False
            End If

            _Disposing = True
        End If
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub

    Public Shared Property Enabled() As Boolean
        Get
            Return Application.UseWaitCursor
        End Get
        Set(ByVal value As Boolean)

            If value = Application.UseWaitCursor Then
                Exit Property
            End If

            Application.UseWaitCursor = value
            Application.UseWaitCursor = Not value
            Application.UseWaitCursor = value
            Dim f As Form = Form.ActiveForm
            If f IsNot Nothing Then
                ' Send WM_SETCURSOR
                NativeMethods.SendMessage(f.Handle, CType(&H20, UInteger), f.Handle, CType(1, IntPtr))
            End If
        End Set
    End Property

End Class

