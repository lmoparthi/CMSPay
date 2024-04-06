'<System.Diagnostics.DebuggerStepThrough()>
Public Class DataGridButtonColumn
    Inherits System.Windows.Forms.DataGridTextBoxColumn

    Private WithEvents _ColumnButton As ButtonValue

    Private WithEvents _CM As CurrencyManager
    Private _RowNum As Integer
    Private _IsEditing As Boolean
    Private _MyGrid As DataGrid
    Private _LastHitTestInfo As System.Windows.Forms.DataGrid.HitTestInfo

    Private _TTip As ToolTip

    Public Event Formatting(ByRef Value As Object, ByVal RowIndex As Integer)
    Public Event UnFormatting(ByRef Value As Object, ByVal RowIndex As Integer)

    Private _Disposed As Boolean = False

    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.
            If _TTip IsNot Nothing Then _TTip.Dispose()
            _TTip = Nothing

            _MyGrid = Nothing
            _CM = Nothing

        End If

        ' Free any unmanaged objects here.
        '
        _Disposed = True

        ' Call base class implementation.
        MyBase.Dispose(disposing)
    End Sub

    Public Overridable Property Position() As Integer?
        Get
            If Me.DataGridTableStyle IsNot Nothing Then
                Return Me.DataGridTableStyle.GridColumnStyles.IndexOf(Me)
            End If

            Return Nothing
        End Get
        Set(ByVal Value As Integer?)
            If Me.DataGridTableStyle Is Nothing Then Exit Property
            If Me.DataGridTableStyle.DataGrid Is Nothing Then Exit Property

            Try
                CType(Me.DataGridTableStyle.DataGrid, DataGridCustom).MoveColumn(Me.DataGridTableStyle.GridColumnStyles.IndexOf(Me), CInt(Value))
            Catch ex As Exception
                Throw
            End Try
        End Set
    End Property

    Public Sub New(ByVal dg As DataGrid, Optional ByVal buttonPic As System.Drawing.Image = Nothing)
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
            _ColumnButton.Parent = Me.TextBox.Parent
            _ColumnButton.Location = Me.TextBox.Location
            _ColumnButton.Top = Me.TextBox.Top - 1
            _ColumnButton.Left = Me.TextBox.Left + Me.TextBox.Width - _ColumnButton.Width

            _ColumnButton.Value = Me.TextBox.Text

        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ButtonStartEditing(ByVal sender As Object, ByVal e As EventArgs)
        Try
            _IsEditing = True
            MyBase.ColumnStartedEditing(CType(sender, Control))
        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub Button_AfterClick(ByVal sender As Object, ByVal e As MouseEventArgs)
        Try
            If _ColumnButton.ChangeValueWithTag Then _ColumnButton.Value = _ColumnButton.Tag
            If _IsEditing Then
                _IsEditing = False
                Me.TextBox.Text = CStr(_ColumnButton.Value)
                If _CM.Position <> _RowNum Then _RowNum = _CM.Position
                SetColumnValueAtRow(_CM, _RowNum, _ColumnButton.Value)
                Invalidate()
            End If

            _ColumnButton.Focus()

        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub TextBoxStartEditing(ByVal sender As Object, ByVal e As EventArgs)
        Try
            _IsEditing = True
            MyBase.ColumnStartedEditing(CType(sender, Control))
        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
            If _IsEditing Then
                Me.TextBox.Text = CStr(_ColumnButton.Value)
                SetColumnValueAtRow(_CM, _RowNum, Me.TextBox.Text)
                _IsEditing = False
                Invalidate()
            End If

            If _MyGrid.CurrentCell.ColumnNumber <> Me.Position Then
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

            If _MyGrid.CurrentCell.ColumnNumber <> Me.Position Then
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
                _ColumnButton.Hide()
                MyBase.EndEdit()
            End If

            If DG.CurrentCell.ColumnNumber <> Me.Position Then
                _ColumnButton.Hide()
            End If

        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
        End Try
    End Sub

    Private Sub Grid_Scroll(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Me.TextBox.Hide()
            _ColumnButton.Hide()
        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            MyBase.EndEdit()
        End Try
    End Sub
    Private Sub Grid_Resize(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Me.TextBox.Hide()
            _ColumnButton.Hide()
        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            MyBase.EndEdit()
        End Try
    End Sub
    Private Sub Grid_Leave(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Me.TextBox.Hide()
            _ColumnButton.Hide()

            If _MyGrid.BindingContext Is Nothing Then Exit Sub
        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            MyBase.EndEdit()
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

    Protected Overrides Function GetColumnValueAtRow(ByVal cm As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer) As Object

        Dim Obj As Object

        Try

            Obj = MyBase.GetColumnValueAtRow(cm, rowNum)

            RaiseEvent Formatting(Obj, rowNum)

            Return Obj
        Catch ex As Exception
            Throw

        Finally
            Obj = Nothing
        End Try

    End Function

    Protected Overrides Function Commit(ByVal cm As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer) As Boolean
        Dim Obj As Object

        Try

            If _IsEditing Then
                _IsEditing = False

                If __ColumnButton Is Nothing OrElse __ColumnButton.Value Is Nothing Then Return False

                SetColumnValueAtRow(cm, rowNum, _ColumnButton.Value)
            End If

            If _IsEditing Then
                _IsEditing = False

                If __ColumnButton Is Nothing OrElse __ColumnButton.Value Is Nothing Then Return False

                Me.TextBox.Text = CStr(_ColumnButton.Value)
                If cm.Position <> rowNum Then rowNum = cm.Position
                Obj = _ColumnButton.Value

                RaiseEvent UnFormatting(Obj, rowNum)

                SetColumnValueAtRow(cm, rowNum, Obj)   ' Write new value.

            End If

            Return True

        Catch ex As Exception
            Throw
        Finally
            Me.EndEdit()   ' Let the DataGrid know that processing is completed.
        End Try
    End Function
End Class

