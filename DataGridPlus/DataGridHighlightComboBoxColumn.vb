'<System.Diagnostics.DebuggerStepThrough()>
Public Class DataGridHighlightComboBoxColumn
    Inherits DataGridHighlightTextBoxColumn

    Public WithEvents _ColumnComboBox As ComboBoxNoKeyUp

    Private WithEvents _SourceCM As CurrencyManager
    Private _LastHitTestInfo As System.Windows.Forms.DataGrid.HitTestInfo
    Private _RowNum As Integer
    Private _IsEditing As Boolean = False
    Private _MyGrid As DataGrid

    Public Sub New(ByVal DataGrid As DataGrid)
        MyBase.New()

        _MyGrid = DataGrid

        _SourceCM = Nothing

        _ColumnComboBox = New ComboBoxNoKeyUp

        AddHandler _ColumnComboBox.Leave, AddressOf LeaveComboBox
        AddHandler _ColumnComboBox.SelectionChangeCommitted, AddressOf ComboStartEditing
        AddHandler _ColumnComboBox.KeyDown, AddressOf ComboKeyDown

        AddHandler _MyGrid.MouseUp, AddressOf Grid_MouseUp
        AddHandler _MyGrid.Scroll, AddressOf Grid_Scroll
        AddHandler _MyGrid.Resize, AddressOf Grid_Resize
        AddHandler _MyGrid.Leave, AddressOf Grid_Leave
    End Sub
    Protected Overloads Overrides Sub Edit(ByVal source As CurrencyManager, ByVal rowNum As Integer, ByVal bounds As Rectangle, ByVal [readOnly] As Boolean, ByVal instantText As String, ByVal cellIsVisible As Boolean)
        MyBase.Edit(source, rowNum, bounds, [readOnly], instantText, cellIsVisible)
        _RowNum = rowNum
        _SourceCM = source

        If Me.ReadOnly = False Then
            _ColumnComboBox.Parent = Me.TextBox.Parent
            _ColumnComboBox.Location = Me.TextBox.Location
            _ColumnComboBox.Size = New Size(Me.TextBox.Size.Width, _ColumnComboBox.Size.Height)
            _ColumnComboBox.Text = Me.TextBox.Text
            Me.TextBox.Visible = False
            _ColumnComboBox.Visible = True
            _ColumnComboBox.BringToFront()
            _ColumnComboBox.Focus()
        End If

    End Sub
    Protected Overloads Overrides Function Commit(ByVal dataSource As CurrencyManager, ByVal rowNum As Integer) As Boolean

        Try

            If _IsEditing Then
                _IsEditing = False

                If _ColumnComboBox Is Nothing OrElse _ColumnComboBox.Text Is Nothing Then Return False

                SetColumnValueAtRow(dataSource, rowNum, _ColumnComboBox.Text)
            End If

            Return True

        Catch ex As Exception
            'Debug.Print(ex.ToString)
            Throw
        End Try

    End Function
    Private Sub ComboStartEditing(ByVal sender As Object, ByVal e As EventArgs)

        _IsEditing = True
        MyBase.ColumnStartedEditing(CType(sender, Control))

    End Sub
    Private Sub LeaveComboBox(ByVal sender As Object, ByVal e As EventArgs)

        If _IsEditing Then
            SetColumnValueAtRow(_SourceCM, _RowNum, _ColumnComboBox.Text)
            _IsEditing = False
            Invalidate()
        End If

        _ColumnComboBox.Hide()

    End Sub
    Private Sub ComboKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim Pos As Integer = -1
        Dim Chg As Boolean = False

        If e.KeyCode = Keys.Enter Then
            _IsEditing = True

            If _SourceCM.Position <> _RowNum Then
                Chg = True
                Pos = _SourceCM.Position
                _SourceCM.Position = _RowNum
            End If

            SetColumnValueAtRow(_SourceCM, _RowNum, _ColumnComboBox.Text)

            If Chg Then
                _SourceCM.Position = Pos
            End If

            _IsEditing = False

            Invalidate()

            MyBase.EndEdit()
            _ColumnComboBox.Hide()

        ElseIf e.KeyCode = Keys.Delete Then
            _IsEditing = True

            _ColumnComboBox.SelectedItem = Nothing

            If _SourceCM.Position <> _RowNum Then
                Chg = True
                Pos = _SourceCM.Position
                _SourceCM.Position = _RowNum
            End If

            SetColumnValueAtRow(_SourceCM, _RowNum, "")

            If Chg Then
                _SourceCM.Position = Pos
            End If

            _IsEditing = False

            Invalidate()

        ElseIf e.KeyCode = Keys.Left OrElse e.KeyCode = Keys.Right OrElse e.KeyCode = Keys.Up OrElse e.KeyCode = Keys.Down Then
            e.Handled = True
        End If
    End Sub

    Private Sub Grid_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Dim DG As DataGrid

        Dim HTI As DataGrid.HitTestInfo

        Try
            DG = CType(sender, DataGridCustom)
            HTI = DG.HitTest(e.X, e.Y)

            If HTI.Type <> DataGrid.HitTestType.Cell Then
                _ColumnComboBox.Hide()
                MyBase.EndEdit()
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub Grid_Scroll(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            _ColumnComboBox.Hide()
        Catch ex As Exception
            Throw

        Finally
            MyBase.EndEdit()
        End Try
    End Sub
    Private Sub Grid_Resize(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            _ColumnComboBox.Hide()
        Catch ex As Exception
            Throw
        Finally
            MyBase.EndEdit()
        End Try
    End Sub
    Private Sub Grid_Leave(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            _ColumnComboBox.Hide()
        Catch ex As Exception
            Throw
        Finally
            MyBase.EndEdit()
        End Try
    End Sub

    Public Sub New()

    End Sub
End Class


