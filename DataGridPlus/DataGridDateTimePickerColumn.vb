'<System.Diagnostics.DebuggerStepThrough()>
Public Class DataGridDateTimePickerColumn
    Inherits System.Windows.Forms.DataGridTextBoxColumn

    Public WithEvents _ColumnDateTimePicker As DateTimePickerNoKeyUp

    Private WithEvents _CM As CurrencyManager
    Private _RowNum As Integer
    Private _IsEditing As Boolean
    Private _MyGrid As DataGrid
    Private _LastHitTestInfo As DataGrid.HitTestInfo

    Private ReadOnly DateLimit As DateTime = CDate("12/31/9998 12:00:00 AM")
    Private ReadOnly DateMinimum As DateTime = CDate("1/1/1753 00:00:00")

    Public Event Formatting(ByRef Value As Object, ByVal RowIndex As Integer)
    Public Event UnFormatting(ByRef Value As Object, ByVal RowIndex As Integer)

    Private _Disposed As Boolean = False
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)

        If _Disposed Then Return

        If disposing Then

            _CM = Nothing
            _MyGrid = Nothing

        End If

        _Disposed = True

        MyBase.Dispose(disposing)
    End Sub

    Public Overridable Property Position() As Integer
        Get
            If Me.DataGridTableStyle IsNot Nothing Then
                Return Me.DataGridTableStyle.GridColumnStyles.IndexOf(Me)
            Else
                Return -1
            End If
        End Get
        Set(ByVal Value As Integer)
            If Me.DataGridTableStyle Is Nothing Then Exit Property
            If Me.DataGridTableStyle.DataGrid Is Nothing Then Exit Property

            CType(Me.DataGridTableStyle.DataGrid, DataGridCustom).MoveColumn(Me.DataGridTableStyle.GridColumnStyles.IndexOf(Me), Value)
        End Set
    End Property

    Public Sub New(ByVal dg As DataGrid, Optional ByVal defaultValue As Object = Nothing)
        MyBase.New()

        _CM = Nothing
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
        'AddHandler ColumnDateTimePicker.Enter, New EventHandler(AddressOf EnterComboBox)
        AddHandler _ColumnDateTimePicker.TextChanged, New EventHandler(AddressOf ComboStartEditing)
        AddHandler _ColumnDateTimePicker.KeyDown, New KeyEventHandler(AddressOf ComboKeyDown)

        AddHandler _MyGrid.MouseUp, New MouseEventHandler(AddressOf Grid_MouseUp)
        AddHandler _MyGrid.Scroll, New EventHandler(AddressOf Grid_Scroll)
        AddHandler _MyGrid.Resize, New EventHandler(AddressOf Grid_Resize)
        AddHandler _MyGrid.Leave, New EventHandler(AddressOf Grid_Leave)
    End Sub

    Protected Overloads Overrides Sub Edit(ByVal cm As CurrencyManager, ByVal rowNum As Integer, ByVal bounds As Rectangle, ByVal [readOnly] As Boolean, ByVal instantText As String, ByVal cellIsVisible As Boolean)
        Try

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

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ComboStartEditing(ByVal sender As Object, ByVal e As EventArgs)

        Try

            _IsEditing = True
            MyBase.ColumnStartedEditing(CType(sender, Control))

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub LeaveComboBox(ByVal sender As Object, ByVal e As EventArgs)

        Try
            If _IsEditing Then
                SetColumnValueAtRow(_CM, _RowNum, _ColumnDateTimePicker.Text)
                _IsEditing = False
                Invalidate()
            End If
            _ColumnDateTimePicker.Hide()
        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub ComboKeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Try

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

        Catch ex As Exception
            Throw
        End Try
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
            End If

        Catch ex As Exception
            Throw
        Finally
            MyBase.EndEdit()
        End Try

    End Sub
    Private Sub Grid_Scroll(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Me.TextBox.Hide()
            _ColumnDateTimePicker.Hide()
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
            _ColumnDateTimePicker.Hide()
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
            _ColumnDateTimePicker.Hide()
        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            MyBase.EndEdit()
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

                If _ColumnDateTimePicker Is Nothing OrElse _ColumnDateTimePicker.Text Is Nothing Then Return False

                Obj = _ColumnDateTimePicker.Text

                RaiseEvent UnFormatting(Obj, rowNum)

                SetColumnValueAtRow(cm, rowNum, Obj)   ' Write new value.
            End If

            Return True

        Catch ex As Exception
            Throw
        Finally
            Obj = Nothing
            Me.EndEdit()   ' Let the DataGrid know that processing is completed.
        End Try
    End Function
End Class

