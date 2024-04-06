'<System.Diagnostics.DebuggerStepThrough()>
Public Class DataGridColorBoolColumn
    Inherits DataGridHighlightBoolColumn
    Private _Col As Integer

    Public Sub New(ByVal col As Integer)
        MyBase.New()
        _Col = col
    End Sub

    Public Event SetCellFormat As FormatCellEventHandler

    Protected Overrides Function GetColumnValueAtRow(ByVal cm As System.Windows.Forms.CurrencyManager, ByVal row As Integer) As Object

        Dim ObjNull As Object = Convert.DBNull

        Try
            If cm IsNot Nothing AndAlso cm.Position > -1 AndAlso cm.Count > cm.Position AndAlso cm.Current IsNot Nothing AndAlso CType(cm.Current, DataRowView).Row.Table.Columns.Contains(MyBase.MappingName) Then

                If ObjNull.Equals(MyBase.GetColumnValueAtRow(cm, row)) Then

                Else

                    Return MyBase.GetColumnValueAtRow(cm, row)

                End If
            End If

            Return False

        Catch ex As Exception

            Return False

        End Try

    End Function

    Protected Overloads Overrides Sub Paint(ByVal g As Graphics, ByVal bounds As Rectangle, ByVal source As CurrencyManager, ByVal rowNum As Integer, ByVal backBrush As Brush, ByVal foreBrush As Brush, ByVal alignToRight As Boolean)
        Dim eCell As DataGridFormatCellEventArgs

        Try
            eCell = New DataGridFormatCellEventArgs(rowNum, Me._Col, "", Me.DataGridTableStyle.DataGrid.Font, backBrush, foreBrush)

            RaiseEvent SetCellFormat(Me, eCell)

            MyBase.Paint(g, bounds, source, rowNum, eCell.BackBrush, eCell.ForeBrush, alignToRight)

        Catch IgnoreException As Exception
            'Debug.Print(IgnoreException.ToString)
        Finally
            eCell = Nothing
        End Try

    End Sub

    Protected Overloads Overrides Sub Edit(ByVal cm As CurrencyManager, ByVal rowNum As Integer, ByVal bounds As Rectangle, ByVal [readOnly] As Boolean, ByVal instantText As String, ByVal cellIsVisible As Boolean)

        'comment to make cells unable to become editable
        MyBase.Edit(cm, rowNum, bounds, [readOnly], instantText, cellIsVisible)

    End Sub
End Class

