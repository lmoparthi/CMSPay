<System.Diagnostics.DebuggerStepThrough()>
Public Class DataGridButtonColumnExtn
    Inherits DataGridTextBoxColumn

    Public WithEvents _ColumnButton As ButtonValue
    Public Property ColumnButton As ButtonValue
        Get
            Return _ColumnButton
        End Get
        Set(value As ButtonValue)
            _ColumnButton = value
        End Set
    End Property

    Private WithEvents _CM As CurrencyManager

    Private _RowNum As Integer
    Private _IsEditing As Boolean
    Private _MyGrid As DataGrid
    Private _TTip As ToolTip

    Dim _Disposed As Boolean = False
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)

        If _Disposed Then Return

        If disposing Then

            _CM = Nothing
            _MyGrid = Nothing

            'If _TTip IsNot Nothing Then _TTip.Dispose()
            '_TTip = Nothing

            '_ColumnButton = Nothing

        End If

        _Disposed = True

        MyBase.Dispose(disposing)
    End Sub
    Public Sub New(ByVal dg As DataGridCustom, Optional ByVal buttonPic As System.Drawing.Image = Nothing)
        MyBase.New()

        _TTip = New ToolTip

        _CM = Nothing
        _IsEditing = False
        _MyGrid = dg

        _ColumnButton = New ButtonValue With {
            .Size = New Size(25, 20),
            .Text = "...",
            .Font = New Font("Arial", 9, FontStyle.Bold),
            .Anchor = System.Windows.Forms.AnchorStyles.Right,
            .TabStop = False
        }

        If buttonPic IsNot Nothing Then _ColumnButton.Image = buttonPic : _ColumnButton.ImageAlign = ContentAlignment.MiddleLeft : _ColumnButton.Text = ""

        AddHandler Me.TextBox.Leave, New EventHandler(AddressOf LeaveTextBox)
        AddHandler Me.TextBox.Enter, New EventHandler(AddressOf EnterTextBox)
        AddHandler Me.TextBox.KeyUp, New KeyEventHandler(AddressOf TextBoxKeyUp)
        AddHandler Me.TextBox.TextChanged, New EventHandler(AddressOf TextBoxStartEditing)
        AddHandler _ColumnButton.Leave, New EventHandler(AddressOf LeaveButton)
        AddHandler _ColumnButton.Click, New EventHandler(AddressOf ButtonStartEditing)
        AddHandler _ColumnButton.MouseUp, New MouseEventHandler(AddressOf Button_AfterClick)
        AddHandler _ColumnButton.TagChanged, AddressOf ButtonTagChanged

        AddHandler _MyGrid.MouseUp, New MouseEventHandler(AddressOf Grid_MouseUp)

        AddHandler _MyGrid.Scroll, New EventHandler(AddressOf Grid_Scroll)
        AddHandler _MyGrid.Resize, New EventHandler(AddressOf Grid_Resize)
        AddHandler _MyGrid.Leave, New EventHandler(AddressOf Grid_Leave)

        AddHandler CType(_MyGrid, DataGridCustom).EnterPressed, New DataGridCustom.EnterPressedEventHandler(AddressOf Grid_EnterPressed)
    End Sub

    Private Sub ButtonTagChanged(ByVal sender As Object, ByVal value As Object)
        If _ColumnButton.ChangeValueWithTag Then _ColumnButton.Value = _ColumnButton.Tag
    End Sub

    Protected Overloads Overrides Sub Edit(ByVal cm As CurrencyManager, ByVal rowNum As Integer, ByVal bounds As Rectangle, ByVal [readOnly] As Boolean, ByVal instantText As String, ByVal cellIsVisible As Boolean)
        Try

            MyBase.Edit(cm, rowNum, bounds, [readOnly], instantText, cellIsVisible)
            _RowNum = rowNum
            _CM = cm

            If _ColumnButton IsNot Nothing Then
                _ColumnButton.Parent = Me.TextBox.Parent
                _ColumnButton.Location = Me.TextBox.Location
                _ColumnButton.Top = Me.TextBox.Top - 1
                _ColumnButton.Left = Me.TextBox.Left + Me.TextBox.Width - _ColumnButton.Width
                _ColumnButton.Value = Me.TextBox.Text
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Protected Overloads Overrides Function Commit(ByVal cm As CurrencyManager, ByVal rowNum As Integer) As Boolean
        Try
            If _IsEditing Then
                _IsEditing = False

                If _ColumnButton Is Nothing OrElse _ColumnButton.Value Is Nothing Then Return False

                Me.TextBox.Text = CStr(_ColumnButton.Value)
                If cm.Position <> rowNum Then rowNum = cm.Position
                SetColumnValueAtRow(cm, rowNum, _ColumnButton.Value)
            End If

            Return True

        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Sub ButtonStartEditing(ByVal sender As Object, ByVal e As EventArgs)
        Try
            _IsEditing = True
            MyBase.ColumnStartedEditing(CType(sender, Control))
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub Button_AfterClick(ByVal sender As Object, ByVal e As MouseEventArgs)
        Try
            _ColumnButton.Value = _ColumnButton.Tag

            If _IsEditing Then
                _IsEditing = False
                If IsDBNull(_ColumnButton.Value) = True Then
                    Me.TextBox.Text = ""
                Else
                    Me.TextBox.Text = CStr(_ColumnButton.Value)
                End If
                If _CM.Position <> _RowNum Then _RowNum = _CM.Position
                SetColumnValueAtRow(_CM, _RowNum, _ColumnButton.Value)
                Invalidate()
            End If
            If _ColumnButton IsNot Nothing Then
                _ColumnButton.Focus()
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub TextBoxStartEditing(ByVal sender As Object, ByVal e As EventArgs)
        Try
            _IsEditing = True
            _ColumnButton.Value = Me.TextBox.Text
            MyBase.ColumnStartedEditing(CType(sender, Control))
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub LeaveButton(ByVal sender As Object, ByVal e As EventArgs)
        Try

            _ColumnButton.Hide()

        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub LeaveTextBox(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If _CM Is Nothing Then
                Select Case DirectCast(DirectCast(sender, System.Windows.Forms.Control).Parent, System.Windows.Forms.DataGrid).DataSource.GetType
                    Case GetType(BindingSource)
                        _CM = DirectCast(DirectCast(DirectCast(sender, System.Windows.Forms.Control).Parent, System.Windows.Forms.DataGrid).DataSource, System.Windows.Forms.BindingSource).CurrencyManager
                End Select
            End If

            If _IsEditing Then
                Me.TextBox.Text = If(_ColumnButton Is Nothing, "", CStr(_ColumnButton.Value))
                SetColumnValueAtRow(_CM, _RowNum, Me.TextBox.Text)
                _IsEditing = False
                Invalidate()
            End If
            If DirectCast(DirectCast(sender, System.Windows.Forms.Control).Parent, System.Windows.Forms.DataGrid).CurrentCell.ColumnNumber <> Me.Position Then
                _ColumnButton.Hide()
            End If

            Me.TextBox.Hide()

        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub EnterTextBox(ByVal sender As Object, ByVal e As EventArgs)
        Try
            _ColumnButton.Parent = Me.TextBox.Parent
            _ColumnButton.Location = Me.TextBox.Location
            _ColumnButton.Top = Me.TextBox.Top - 1
            _ColumnButton.Left = Me.TextBox.Left + Me.TextBox.Width - _ColumnButton.Width

            _ColumnButton.Value = Me.TextBox.Text
            _ColumnButton.Visible = True
            _ColumnButton.BackColor = System.Drawing.Color.LightGray
            _ColumnButton.BringToFront()
        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub TextBoxKeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Try
            _ColumnButton.Value = Me.TextBox.Text
        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub TextBoxKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Try
            _ColumnButton.Value = Me.TextBox.Text
        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub EnterButton(ByVal sender As Object, ByVal e As EventArgs)
        Try
            _ColumnButton.Value = Me.TextBox.Text
            If _MyGrid.CurrentRowIndex <> _RowNum Then
                _ColumnButton.Hide()
            End If
        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                ' If _ColumnButton IsNot Nothing Then
                _ColumnButton.Hide()
                'End If

            End If
            'If DirectCast(DirectCast(sender, System.Windows.Forms.Control).Parent, System.Windows.Forms.DataGrid).CurrentCell.ColumnNumber <> Me.Position Then
            '    _ColumnButton.Hide()
            'End If
        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            MyBase.EndEdit()
        End Try
    End Sub
    Private Sub Grid_Scroll(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Me.TextBox.Hide()
            _ColumnButton.Hide()
        Catch ex As Exception
            Throw
        Finally
            'MyBase.EndEdit()
        End Try
    End Sub
    Private Sub Grid_Resize(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Me.TextBox.Hide()
            _ColumnButton.Hide()
        Catch ex As Exception
            Throw
        Finally
            ' MyBase.EndEdit()
        End Try
    End Sub
    Private Sub Grid_Leave(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Me.TextBox.Hide()
            _ColumnButton.Hide()
        Catch ex As Exception
            Throw
        Finally
            ' MyBase.EndEdit()
        End Try
    End Sub
    Private Sub Grid_EnterPressed(ByVal cell As System.Windows.Forms.DataGridCell)
        Try
            If _MyGrid.CurrentCell.ColumnNumber = Me.Position Then
                _ColumnButton.PerformClick()

                If _ColumnButton.ChangeValueWithTag Then _ColumnButton.Value = _ColumnButton.Tag
                If _IsEditing Then
                    _IsEditing = False
                    Me.TextBox.Text = CStr(_ColumnButton.Value)
                    If _CM.Position <> _RowNum Then _RowNum = _CM.Position
                    SetColumnValueAtRow(_CM, _RowNum, _ColumnButton.Value)
                    Invalidate()
                End If

                _ColumnButton.Focus()
            End If
        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class
