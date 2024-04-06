'<System.Diagnostics.DebuggerStepThrough()>
Public Class DataGridComboBoxColumnExtn
    Inherits System.Windows.Forms.DataGridTextBoxColumn

    'Public WithEvents _ColumnComboBox As ComboBoxNoKeyUp
    Private WithEvents _ColumnComboBox As NoKeyUpCombo
    Private WithEvents _CM As CurrencyManager
    Private _RowNum As Integer
    Private _Col As Integer
    Private _IsEditing As Boolean
    Private _MyGrid As DataGrid
    Private _LastHitTestInfo As System.Windows.Forms.DataGrid.HitTestInfo

    Public Event CheckCellEnabled As EnableCellEventHandler

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
    Public ReadOnly Property ColumnComboBox As NoKeyUpCombo
        Get
            ColumnComboBox = _ColumnComboBox
        End Get
    End Property
    Public Sub New(ByVal DataGrid As DataGrid)
        MyBase.New()

        _MyGrid = DataGrid

        _CM = Nothing
        _IsEditing = False

        ' _ColumnComboBox = New ComboBoxNoKeyUp
        _ColumnComboBox = New NoKeyUpCombo
        AddHandler _ColumnComboBox.Leave, AddressOf LeaveComboBox
        AddHandler _ColumnComboBox.SelectionChangeCommitted, AddressOf ComboStartEditing
        AddHandler _ColumnComboBox.KeyDown, AddressOf ComboKeyDown

        AddHandler _MyGrid.MouseUp, AddressOf Grid_MouseUp
        AddHandler _MyGrid.Scroll, AddressOf Grid_Scroll
        AddHandler _MyGrid.Resize, AddressOf Grid_Resize
        AddHandler _MyGrid.Leave, AddressOf Grid_Leave
    End Sub

    Protected Overloads Overrides Sub Edit(ByVal source As CurrencyManager, ByVal rowNum As Integer, ByVal bounds As Rectangle, ByVal [readOnly] As Boolean, ByVal instantText As String, ByVal cellIsVisible As Boolean)
        Dim enabled As Boolean = True
        Dim e As DataGridEnableEventArgs

        e = New DataGridEnableEventArgs(rowNum, _Col, enabled)

        RaiseEvent CheckCellEnabled(Me, e)
        If e.EnableValue Then 'provide check to approve edit of cell
            MyBase.Edit(source, rowNum, bounds, [readOnly], instantText, cellIsVisible)
            _RowNum = rowNum
            _CM = source

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
        End If
    End Sub

    Private Sub ComboStartEditing(ByVal sender As Object, ByVal e As EventArgs)

        _IsEditing = True
        MyBase.ColumnStartedEditing(CType(sender, Control))

    End Sub
    Private Sub LeaveComboBox(ByVal sender As Object, ByVal e As EventArgs)
        ' _CM = DirectCast(DirectCast(sender, System.Windows.Forms.ComboBox).DataSource, System.Windows.Forms.BindingSource).CurrencyManager
        If _CM Is Nothing Then

            Select Case DirectCast(DirectCast(sender, System.Windows.Forms.Control).Parent, System.Windows.Forms.DataGrid).DataSource.GetType
                Case GetType(BindingSource)
                    _CM = DirectCast(DirectCast(DirectCast(sender, System.Windows.Forms.Control).Parent, System.Windows.Forms.DataGrid).DataSource, System.Windows.Forms.BindingSource).CurrencyManager
            End Select

            If _CM Is Nothing Then
                _CM = DirectCast(DirectCast(sender, System.Windows.Forms.ComboBox).DataSource, System.Windows.Forms.BindingSource).CurrencyManager
            End If
        End If


        If _IsEditing Then
            SetColumnValueAtRow(_CM, _RowNum, _ColumnComboBox.Text)
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
            If _CM.Position <> _RowNum Then
                Chg = True
                Pos = _CM.Position
                _CM.Position = _RowNum
            End If
            SetColumnValueAtRow(_CM, _RowNum, _ColumnComboBox.Text)
            If Chg = True Then
                _CM.Position = Pos
            End If
            _IsEditing = False
            Invalidate()

            MyBase.EndEdit()
            _ColumnComboBox.Hide()
        ElseIf e.KeyCode = Keys.Delete Then
            _IsEditing = True
            _ColumnComboBox.SelectedItem = Nothing
            If _CM.Position <> _RowNum Then
                Chg = True
                Pos = _CM.Position
                _CM.Position = _RowNum
            End If
            SetColumnValueAtRow(_CM, _RowNum, "")
            If Chg = True Then
                _CM.Position = Pos
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
            End If
        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            MyBase.EndEdit()
        End Try
    End Sub
    Private Sub Grid_Scroll(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            _ColumnComboBox.Hide()
        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            MyBase.EndEdit()
        End Try
    End Sub
    Private Sub Grid_Resize(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            _ColumnComboBox.Hide()
        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            MyBase.EndEdit()
        End Try
    End Sub
    Private Sub Grid_Leave(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            _ColumnComboBox.Hide()
        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            MyBase.EndEdit()
        End Try
    End Sub

    Protected Overrides Function GetColumnValueAtRow(ByVal cm As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer) As Object
        Dim Obj As Object = MyBase.GetColumnValueAtRow(cm, rowNum)

        Try

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

            If _ColumnComboBox Is Nothing OrElse _ColumnComboBox.Text Is Nothing Then Return False

            Obj = _ColumnComboBox.Text

            If _IsEditing Then
                _IsEditing = False

                RaiseEvent UnFormatting(Obj, rowNum)

                SetColumnValueAtRow(cm, rowNum, Obj)   ' Write new value.
            End If

            Return True

        Catch ex As Exception
            Throw
        Finally
            Obj = Nothing
            Me.EndEdit()
        End Try

    End Function
End Class