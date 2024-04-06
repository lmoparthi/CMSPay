'<System.Diagnostics.DebuggerStepThrough()>
Public Class DataGridHighlightBoolColumn
    Inherits System.Windows.Forms.DataGridBoolColumn

    Public Delegate Sub BoolValueChangedEventHandler(ByVal sender As Object, ByVal e As BoolValueChangedEventArgs)

    Private _HighlightedCell As New DataGridCell(-1, -1)
    Private _HighlightedRow As Integer?
    Private _HighlightedBackColor As Color = Color.DarkBlue
    Private _HighlightedForeColor As Color = Color.White

    Private _LastCell As New DataGridCell(-1, -1)

    Private _Init As Boolean = True

    Public Event OnPaint(ByRef Highlight As Boolean, ByVal Cell As DataGridCell)
    Public Event BoolValueChanged As BoolValueChangedEventHandler

    Private _SaveValue As Object
    Private _SaveRow As Integer?
    Private _LockValue As Boolean
    Private _Editing As Boolean

    Public Const VK_SPACE As Integer = 32

    <System.ComponentModel.Description("Gets or Sets the Column Position")>
    Public Overridable Property Position() As Integer
        Get
            If Me.DataGridTableStyle IsNot Nothing Then
                Return Me.DataGridTableStyle.GridColumnStyles.IndexOf(Me)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As Integer)
            If Me.DataGridTableStyle Is Nothing Then Exit Property
            If Me.DataGridTableStyle.DataGrid Is Nothing Then Exit Property

            CType(Me.DataGridTableStyle.DataGrid, DataGridCustom).MoveColumn(Me.DataGridTableStyle.GridColumnStyles.IndexOf(Me), Value)
        End Set
    End Property

    <System.ComponentModel.Description("Gets or Sets the Highlighted Cell")>
    Public Property HighlightCell() As DataGridCell
        Get
            Return _HighlightedCell
        End Get
        Set(ByVal Value As DataGridCell)
            _Init = False
            _HighlightedCell = Value
        End Set
    End Property

    <System.ComponentModel.Description("Gets or Sets the Highlighted Row")>
    Public Property HighlightRow() As Integer?
        Get
            Return _HighlightedRow
        End Get
        Set(ByVal Value As Integer?)
            _HighlightedRow = Value
        End Set
    End Property

    <System.ComponentModel.Description("Gets or Sets the Highlight Backcolor")>
    Public Property HighlightBackColor() As Color
        Get
            Return _HighlightedBackColor
        End Get
        Set(ByVal Value As Color)
            _HighlightedBackColor = Value
        End Set
    End Property

    <System.ComponentModel.Description("Gets or Sets the Highlight Forecolor")>
    Public Property HighlightForeColor() As Color
        Get
            Return _HighlightedForeColor
        End Get
        Set(ByVal Value As Color)
            _HighlightedForeColor = Value
        End Set
    End Property

    Public Sub New()
        MyBase.New()
        _SaveRow = Nothing
        _LockValue = False
        _Editing = False

    End Sub

    Protected Overloads Overrides Sub Paint(ByVal g As Graphics, ByVal bounds As Rectangle, ByVal source As CurrencyManager, ByVal rowNum As Integer, ByVal backBrush As Brush, ByVal foreBrush As Brush, ByVal alignToRight As Boolean)

        Dim Changing As Boolean
        Dim MousePos As Point
        Dim DG As DataGrid
        Dim IsClickInCell As Boolean

        Dim DGTS As DataGridTableStyle
        Dim Cell As DataGridCell
        Dim EArgs As BoolValueChangedEventArgs
        Dim Highlight As Boolean = False

        Try

            MousePos = Me.DataGridTableStyle.DataGrid.PointToClient(Control.MousePosition)
            DG = Me.DataGridTableStyle.DataGrid
            IsClickInCell = DG.CurrentCell.RowNumber > -1 AndAlso ((Control.MouseButtons = System.Windows.Forms.MouseButtons.Left) AndAlso DG.GetCellBounds(DG.CurrentCell).Contains(MousePos))

            DGTS = Me.DataGridTableStyle
            Cell = New DataGridCell(rowNum, DGTS.GridColumnStyles.IndexOf(DGTS.GridColumnStyles(MyBase.MappingName)))

            If HighlightTest(Cell) Then
                Highlight = True
            Else
                If Cell.Equals(CType(Me.DataGridTableStyle.DataGrid, DataGridCustom).HighlightedCell) Then
                    Highlight = True
                ElseIf rowNum = HighlightRow Then
                    Highlight = True
                End If
            End If

            Changing = DG.Focused AndAlso (IsClickInCell OrElse NativeMethods.GetKeyState(VK_SPACE) < 0)   ' or spacebar

            If ((Not _LockValue) AndAlso _Editing AndAlso Changing AndAlso _SaveRow = rowNum) Then
                _SaveValue = MyBase.GetColumnValueAtRow(source, rowNum)
                _LockValue = False
                EArgs = New BoolValueChangedEventArgs(rowNum, Me.DataGridTableStyle.GridColumnStyles.IndexOf(Me), _SaveValue)
                RaiseEvent BoolValueChanged(Me, EArgs)
            End If

            If _SaveRow = rowNum Then _LockValue = False

            MyBase.Paint(g, bounds, source, rowNum, If(Highlight, Brushes.DarkBlue, backBrush), If(Highlight, Brushes.White, foreBrush), alignToRight)

        Catch IgnoreException As Exception
            'Debug.Print(IgnoreException.ToString)
        Finally

            Cell = Nothing

            If DGTS IsNot Nothing Then DGTS.Dispose()
            DGTS = Nothing

        End Try
    End Sub

    Public Function HighlightTest(ByVal Cell As DataGridCell) As Boolean
        Dim Highlight As Boolean = False

        RaiseEvent OnPaint(Highlight, Cell)
        Return Highlight

    End Function

    Protected Overloads Overrides Sub Edit(ByVal cm As CurrencyManager, ByVal rowNum As Integer, ByVal bounds As Rectangle, ByVal [readOnly] As Boolean, ByVal instantText As String, ByVal cellIsVisible As Boolean)

        Dim DGTS As DataGridTableStyle
        Dim Cell As DataGridCell

        Try

            DGTS = Me.DataGridTableStyle()
            Cell = New DataGridCell(rowNum, DGTS.GridColumnStyles.IndexOf(DGTS.GridColumnStyles(MyBase.MappingName)))

            If Cell.Equals(CType(Me.DataGridTableStyle.DataGrid, DataGridCustom).HighlightedCell) AndAlso _Init = False Then
                If _LastCell.Equals(Cell) = False Then
                    _LastCell = Cell
                Else
                    _LockValue = True
                    _Editing = True
                    _SaveRow = rowNum
                    _SaveValue = MyBase.GetColumnValueAtRow(cm, rowNum)

                    MyBase.Edit(cm, rowNum, bounds, [readOnly], instantText, cellIsVisible)
                End If

            Else
                _LockValue = True
                _Editing = True
                _SaveRow = rowNum
                _SaveValue = MyBase.GetColumnValueAtRow(cm, rowNum)

                MyBase.Edit(cm, rowNum, bounds, [readOnly], instantText, cellIsVisible)

            End If
        Catch IgnoreException As Exception
            'Debug.Print(IgnoreException.ToString)
        Finally
            Cell = Nothing
            If DGTS IsNot Nothing Then DGTS.Dispose()
            DGTS = Nothing
        End Try
    End Sub

    Protected Overloads Overrides Function Commit(ByVal cm As CurrencyManager, ByVal rowNum As Integer) As Boolean
        _LockValue = True
        _Editing = False
        Return MyBase.Commit(cm, rowNum)
    End Function
End Class

