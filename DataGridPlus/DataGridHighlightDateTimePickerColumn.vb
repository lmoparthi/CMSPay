'<System.Diagnostics.DebuggerStepThrough()>
Public Class DataGridHighlightDateTimePickerColumn
    Inherits DataGridHighlightTextBoxColumn

    Private WithEvents _ColumnDateTimePicker As DateTimePickerNoKeyUp

    Private WithEvents _CM As CurrencyManager
    Private _LastHitTestInfo As System.Windows.Forms.DataGrid.HitTestInfo
    Private _RowNum As Integer
    Private _IsEditing As Boolean
    Private _MyGrid As DataGrid

    Private ReadOnly DateLimit As DateTime = CDate("12/31/9998 12:00:00 AM")
    Private ReadOnly DateMinimum As DateTime = CDate("1/1/1753 00:00:00")

    Private _Disposed As Boolean = False
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)

        If _Disposed Then Return

        If disposing Then
            RemoveHandler _ColumnDateTimePicker.Leave, New EventHandler(AddressOf LeaveComboBox)
            RemoveHandler _ColumnDateTimePicker.Enter, New EventHandler(AddressOf EnterComboBox)
            RemoveHandler _ColumnDateTimePicker.TextChanged, New EventHandler(AddressOf ComboStartEditing)
            RemoveHandler _ColumnDateTimePicker.KeyDown, New KeyEventHandler(AddressOf ComboKeyDown)

            RemoveHandler _MyGrid.MouseUp, New MouseEventHandler(AddressOf Grid_MouseUp)
            RemoveHandler _MyGrid.Scroll, New EventHandler(AddressOf Grid_Scroll)
            RemoveHandler _MyGrid.Resize, New EventHandler(AddressOf Grid_Resize)
            RemoveHandler _MyGrid.Leave, New EventHandler(AddressOf Grid_Leave)

            _CM = Nothing
            _MyGrid = Nothing

        End If

        _Disposed = True

        MyBase.Dispose(disposing)
    End Sub

    Public Sub New(ByVal dg As DataGrid, Optional ByVal defaultValue As Object = Nothing)
        MyBase.New()

        _IsEditing = False
        _MyGrid = dg

        _ColumnDateTimePicker = New DateTimePickerNoKeyUp With {
            .Size = Me.TextBox.Size, 'New Size(25, 20)
            .CustomFormat = Me.Format,
            .Format = DateTimePickerFormat.Custom
        }

        If defaultValue IsNot Nothing Then
            If IsDate(defaultValue) Then
                _ColumnDateTimePicker.Value = CDate(defaultValue)
            Else
                _ColumnDateTimePicker.Value = Date.Now
            End If
        Else
            _ColumnDateTimePicker.Value = Date.Now
        End If

        AddHandler _ColumnDateTimePicker.Leave, New EventHandler(AddressOf LeaveComboBox)
        AddHandler _ColumnDateTimePicker.Enter, New EventHandler(AddressOf EnterComboBox)
        AddHandler _ColumnDateTimePicker.TextChanged, New EventHandler(AddressOf ComboStartEditing)
        AddHandler _ColumnDateTimePicker.KeyDown, New KeyEventHandler(AddressOf ComboKeyDown)

        AddHandler _MyGrid.MouseUp, New MouseEventHandler(AddressOf Grid_MouseUp)
        AddHandler _MyGrid.Scroll, New EventHandler(AddressOf Grid_Scroll)
        AddHandler _MyGrid.Resize, New EventHandler(AddressOf Grid_Resize)
        AddHandler _MyGrid.Leave, New EventHandler(AddressOf Grid_Leave)
    End Sub

    Protected Overloads Overrides Sub Edit(ByVal cm As CurrencyManager, ByVal rowNum As Integer, ByVal bounds As Rectangle, ByVal [readOnly] As Boolean, ByVal instantText As String, ByVal cellIsVisible As Boolean)

        MyBase.Edit(cm, rowNum, bounds, [readOnly], instantText, cellIsVisible)

        _RowNum = rowNum
        _CM = cm

        _ColumnDateTimePicker.Parent = Me.TextBox.Parent
        _ColumnDateTimePicker.Location = Me.TextBox.Location
        _ColumnDateTimePicker.Size = New Size(Me.TextBox.Size.Width, _ColumnDateTimePicker.Size.Height)

        If IsDate(Me.TextBox.Text) Then
            If CDate(Me.TextBox.Text) > DateLimit Then Me.TextBox.Text = CStr(DateLimit)
            If CDate(Me.TextBox.Text) < DateMinimum Then Me.TextBox.Text = CStr(DateMinimum)

            If CDate(Me.TextBox.Text) > _ColumnDateTimePicker.MaxDate Then _ColumnDateTimePicker.MaxDate = CDate(Me.TextBox.Text)
            If CDate(Me.TextBox.Text) < _ColumnDateTimePicker.MinDate Then _ColumnDateTimePicker.MinDate = CDate(Me.TextBox.Text)
        End If

        _ColumnDateTimePicker.Value = CDate(Me.TextBox.Text)
        Me.TextBox.Visible = False
        _ColumnDateTimePicker.Visible = True
        _ColumnDateTimePicker.BringToFront()
        _ColumnDateTimePicker.Focus()
    End Sub
    Protected Overloads Overrides Function Commit(ByVal cm As CurrencyManager, ByVal rowNum As Integer) As Boolean

        Try

            If _IsEditing Then
                _IsEditing = False

                If _ColumnDateTimePicker Is Nothing OrElse _ColumnDateTimePicker.Text Is Nothing Then Return False

                SetColumnValueAtRow(cm, rowNum, _ColumnDateTimePicker.Text)
            End If

            Return True

        Catch ex As Exception
            Throw
        End Try

    End Function
    Private Sub ComboStartEditing(ByVal sender As Object, ByVal e As EventArgs)

        _IsEditing = True
        MyBase.ColumnStartedEditing(CType(sender, Control))

    End Sub

    Private Sub EnterComboBox(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Private Sub LeaveComboBox(ByVal sender As Object, ByVal e As EventArgs)

        If _IsEditing Then
            SetColumnValueAtRow(_CM, _RowNum, _ColumnDateTimePicker.Text)
            _IsEditing = False
            Invalidate()
        End If
        _ColumnDateTimePicker.Hide()

    End Sub
    Private Sub ComboKeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)

        If e.KeyCode = Keys.Enter Then
            _IsEditing = True
            SetColumnValueAtRow(_CM, _RowNum, _ColumnDateTimePicker.Text)
            _IsEditing = False
            Invalidate()
        ElseIf e.KeyCode = Keys.Tab Then

        Else
            If _IsEditing = False Then
                _IsEditing = True
                MyBase.ColumnStartedEditing(CType(sender, Control))
            End If
        End If
    End Sub
    Private Sub ComboKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim Pos As Integer = -1
        Dim Chg As Boolean = False
        Try

            If e.KeyCode = Keys.Enter Then
                _IsEditing = True
                If _CM.Position <> _RowNum Then
                    Chg = True
                    Pos = _CM.Position
                    _CM.Position = _RowNum
                End If
                SetColumnValueAtRow(_CM, _RowNum, _ColumnDateTimePicker.Text)

                If Chg = True Then
                    _CM.Position = Pos
                End If

                _IsEditing = False
                Invalidate()
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub Grid_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Dim DG As DataGrid

        Dim HTI As DataGrid.HitTestInfo

        Try
            DG = CType(sender, DataGridCustom)
            HTI = DG.HitTest(e.X, e.Y)

            If HTI.Type <> DataGrid.HitTestType.Cell Then
                Me.TextBox.Hide()
                _ColumnDateTimePicker.Hide()
                MyBase.EndEdit()
            End If
        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub Grid_Scroll(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Me.TextBox.Hide()
            _ColumnDateTimePicker.Hide()
        Catch ex As Exception
            Throw
        Finally
            MyBase.EndEdit()
        End Try
    End Sub
    Private Sub Grid_Resize(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Me.TextBox.Hide()
            _ColumnDateTimePicker.Hide()
        Catch ex As Exception
            Throw
        Finally
            MyBase.EndEdit()
        End Try
    End Sub
    Private Sub Grid_Leave(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Me.TextBox.Hide()
            _ColumnDateTimePicker.Hide()
        Catch ex As Exception
            Throw
        Finally
            MyBase.EndEdit()
        End Try
    End Sub
End Class
