Public Class ConfirmDeleteDataGrid
    Inherits DataGridCustom

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _LastKey As System.Windows.Forms.Keys

    Public Event DeletedRow As EventHandler(Of EventArgs)

    Private Const WM_KEYDOWN As Integer = &H100

    Public ReadOnly Property LastKey() As System.Windows.Forms.Keys
        Get
            Return _LastKey
        End Get
    End Property

    Public Sub ScrollToRow(ByVal row As Integer)
        If Me.DataSource IsNot Nothing Then
            GridVScrolled(Me, New ScrollEventArgs(ScrollEventType.LargeIncrement, row))
        End If
    End Sub

    Public Overrides Function PreProcessMessage(ByRef msg As System.Windows.Forms.Message) As Boolean

        Dim CM As CurrencyManager

        Dim DV As DataView
        Dim KeyCode As Keys

        Try

            KeyCode = CType((msg.WParam.ToInt32 And Keys.KeyCode), Keys)

            If msg.Msg = WM_KEYDOWN AndAlso KeyCode = Keys.Delete Then

                CM = CType(Me.BindingContext(Me.DataSource, Me.DataMember), CurrencyManager)

                DV = CType(CM.List, System.Data.DataView)

                For I As Integer = 0 To DV.Count - 1

                    If Me.IsSelected(I) Then
                        If DV.Item(I).Row.RowState <> DataRowState.Added Then
                            MessageBox.Show("To Delete an Accumulator set it's Dollar Amount to 0", "Delete unavailable", MessageBoxButtons.OK)

                            Return True

                        End If
                    End If

                Next

            End If

            Return MyBase.PreProcessMessage(msg)

        Catch ex As Exception
            Throw
        End Try

    End Function

    Protected Overrides Function ProcessDialogKey(ByVal keyData As System.Windows.Forms.Keys) As Boolean

        Dim Pt As Point

        Dim HTI As DataGrid.HitTestInfo
        Dim CM As CurrencyManager

        Dim DV As DataView

        Try

            _LastKey = CType(keyData - Control.ModifierKeys, Keys)

            Pt = Me.PointToClient(Cursor.Position)

            HTI = Me.HitTest(Pt)

            If keyData = Keys.Delete Then

                If HTI.Type = ConfirmDeleteDataGrid.HitTestType.RowHeader Then

                    CM = CType(Me.BindingContext(Me.DataSource, Me.DataMember), CurrencyManager)

                    DV = CType(CM.List, System.Data.DataView)

                    For i As Integer = 0 To DV.Count - 1

                        If Me.IsSelected(i) Then
                            If DV.Item(i).Row.RowState <> DataRowState.Added Then
                                MessageBox.Show("To Delete an Accumulator set it's Dollar Amount to 0", "Delete unavailable", MessageBoxButtons.OK)

                                Return True

                            End If
                        End If

                    Next

                End If
            ElseIf keyData = Keys.Tab AndAlso CurrentCell.ColumnNumber = 1 Then
            End If

            Return MyBase.ProcessDialogKey(keyData)

        Catch ex As Exception
            Throw
        End Try

    End Function

End Class