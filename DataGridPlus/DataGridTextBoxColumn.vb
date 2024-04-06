'<System.Diagnostics.DebuggerStepThrough()>
Public Class DataGridTextBoxColumn
    Inherits System.Windows.Forms.DataGridTextBoxColumn

    Public Event Formatting(ByRef Value As Object, ByVal RowIndex As Integer)
    Public Event UnFormatting(ByRef Value As String, ByVal RowIndex As Integer)


    Public Sub New()
        MyBase.New()

    End Sub

    Public Overridable Property Position() As Integer?
        Get
            If Not Me.DataGridTableStyle Is Nothing Then
                Return Me.DataGridTableStyle.GridColumnStyles.IndexOf(Me)
            End If
            Return Nothing

        End Get
        Set(ByVal Value As Integer?)
            If Me.DataGridTableStyle Is Nothing Then Exit Property
            If Me.DataGridTableStyle.DataGrid Is Nothing Then Exit Property

            CType(Me.DataGridTableStyle.DataGrid, DataGridCustom).MoveColumn(Me.DataGridTableStyle.GridColumnStyles.IndexOf(Me), CInt(Value))
        End Set
    End Property

    Protected Overrides Function GetColumnValueAtRow(ByVal cm As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer) As Object
        Try

            RaiseEvent Formatting(MyBase.GetColumnValueAtRow(cm, rowNum), rowNum)

            Return MyBase.GetColumnValueAtRow(cm, rowNum)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Protected Overrides Function Commit(ByVal dataSource As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer) As Boolean
        Dim Box As DataGridTextBox

        Try
            Box = CType(Me.TextBox, DataGridTextBox)

            Me.HideEditBox()
            If Box.IsInEditOrNavigateMode Then Return True

            RaiseEvent UnFormatting(Box.Text, rowNum)

            SetColumnValueAtRow(dataSource, rowNum, Box.Text)   ' Write new value.

            Return True
        Catch ex As Exception
            Throw
        Finally
            Me.EndEdit()
        End Try
    End Function
End Class

