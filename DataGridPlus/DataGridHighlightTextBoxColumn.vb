<System.Diagnostics.DebuggerStepThrough()>
Public Class DataGridHighlightTextBoxColumn
    Inherits System.Windows.Forms.DataGridTextBoxColumn

    Private _HighlightedCell As New DataGridCell(-1, -1)
    Private _HighlightedRow As Integer?
    Private _MinimumCharWidth As Integer?
    Private _MaximumCharWidth As Integer?
    Private _HighlightedBackColor As Color = Color.DarkBlue
    Private _HighlightedForeColor As Color = Color.White
    Private _FormatUsingRegEx As Boolean = False
    Private _CanFilterBy As Boolean = False

    Private _LastCell As New DataGridCell(-1, -1)

    Private _Init As Boolean = True

    Public Event OnPaint(ByRef Highlight As Boolean, ByVal Cell As DataGridCell)
    Public Event Formatting(ByRef Value As Object, ByVal RowIndex As Integer)
    Public Event NullTextSubstitution(ByRef Value As Object, ByVal NullText As String)
    Public Event UnFormatting(ByRef Value As Object, ByVal RowIndex As Integer)
    Public Event SetRowFormat As FormatRowEventHandler

    Public Sub New()
        MyBase.New()
    End Sub

#Region "Public Properties"
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

            CType(Me.DataGridTableStyle.DataGrid, DataGridCustom).MoveColumn(Me.DataGridTableStyle.GridColumnStyles.IndexOf(Me), CInt(Value))
        End Set
    End Property

    Public Property MinimumCharWidth() As Integer?
        Get
            Return _MinimumCharWidth
        End Get
        Set(ByVal Value As Integer?)
            _MinimumCharWidth = Value
        End Set
    End Property

    Public Property MaximumCharWidth() As Integer?
        Get
            Return _MaximumCharWidth
        End Get
        Set(ByVal Value As Integer?)
            _MaximumCharWidth = Value
        End Set
    End Property

    Public Property HighlightCell() As DataGridCell
        Get
            Return _HighlightedCell
        End Get
        Set(ByVal Value As DataGridCell)
            _Init = False
            _HighlightedCell = Value
        End Set
    End Property

    Public Property HighlightRow() As Integer?
        Get
            Return _HighlightedRow
        End Get
        Set(ByVal Value As Integer?)
            _HighlightedRow = Value
        End Set
    End Property

    Public Property HighlightBackColor() As Color
        Get
            Return _HighlightedBackColor
        End Get
        Set(ByVal Value As Color)
            _HighlightedBackColor = Value
        End Set
    End Property

    Public Property HighlightForeColor() As Color
        Get
            Return _HighlightedForeColor
        End Get
        Set(ByVal Value As Color)
            _HighlightedForeColor = Value
        End Set
    End Property

    Public Property FormatIsRegEx() As Boolean
        Get
            Return _FormatUsingRegEx
        End Get
        Set(ByVal Value As Boolean)
            _FormatUsingRegEx = Value
        End Set
    End Property

    Public Property CanFilterByColumn() As Boolean
        Get
            Return _CanFilterBy
        End Get
        Set(ByVal Value As Boolean)
            _CanFilterBy = Value
        End Set
    End Property

#End Region

    Public Function HighlightTest(ByVal Cell As DataGridCell) As Boolean
        Dim Highlight As Boolean = False
        RaiseEvent OnPaint(Highlight, Cell)
        Return Highlight
    End Function

    Protected Overloads Overrides Sub Edit(ByVal source As CurrencyManager, ByVal rowNum As Integer, ByVal bounds As Rectangle, ByVal [readOnly] As Boolean, ByVal instantText As String, ByVal cellIsVisible As Boolean)
        Dim Style As DataGridTableStyle
        Dim Cell As DataGridCell

        Try
            Style = Me.DataGridTableStyle()
            Cell = New DataGridCell(rowNum, Style.GridColumnStyles.IndexOf(Style.GridColumnStyles(MyBase.MappingName)))

            If Cell.Equals(CType(Me.DataGridTableStyle.DataGrid, DataGridCustom).HighlightedCell) AndAlso _Init = False Then
                If _LastCell.Equals(Cell) = False Then
                    _LastCell = Cell
                Else
                    MyBase.Edit(source, rowNum, bounds, [readOnly], instantText, cellIsVisible)
                End If

            Else
                MyBase.Edit(source, rowNum, bounds, [readOnly], instantText, cellIsVisible)
            End If

        Catch Exception As Exception
            Throw
        Finally
        End Try
    End Sub

    Protected Overrides Function GetColumnValueAtRow(ByVal cm As CurrencyManager, ByVal rowNum As Integer) As Object

        Dim DV As DataView
        Dim DRV As DataRowView
        Dim DR As DataRow
        Dim DT As DataTable
        Dim Obj As Object

        'source.Current represent the selected row only and not the row being processed by this code.
        'rowNum represents DataView position

        Try
            If cm IsNot Nothing AndAlso cm.Position > -1 AndAlso cm.Count > cm.Position AndAlso cm.Current IsNot Nothing AndAlso CType(cm.Current, DataRowView).Row.Table.Columns.Contains(MyBase.MappingName) Then

                If TypeOf cm.List Is BindingSource Then
                    If DirectCast(cm.List, BindingSource) IsNot Nothing AndAlso DirectCast(cm.List, BindingSource).DataSource IsNot Nothing Then

                        DV = DirectCast(DirectCast(cm.List, BindingSource).List, DataView)
                        If DirectCast(cm.List, BindingSource).Position > -1 Then
                            DRV = DirectCast(DirectCast(cm.List, BindingSource).Current, DataRowView)
                        Else
                            Return Nothing
                        End If
                    Else
                        Throw New ApplicationException(" RowNum: " & rowNum.ToString)
                    End If
                Else
                    DV = DirectCast(cm.List, DataView)
                    DRV = DirectCast(cm.Current, DataRowView)
                End If

                DR = DRV.Row 'obtain row before data commits and is subject to archive filter
                DT = DR.Table

                If rowNum > -1 AndAlso DT.Columns.Contains(MyBase.MappingName) AndAlso rowNum < DV.Count Then 'if rowNum is > available rows a row add was most likely reversed.
                    Obj = DV(rowNum)(MyBase.MappingName)
                    RaiseEvent Formatting(Obj, rowNum)
                    RaiseEvent NullTextSubstitution(Obj, NullText)
                End If

                Return If(IsDBNull(Obj), "", Obj)

            End If

            Return Nothing

        Catch ex As Exception

            Throw New ApplicationException(" RowNum: " & rowNum.ToString)

        End Try

    End Function

    Protected Overrides Function Commit(ByVal dataSource As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer) As Boolean
        Dim TBox As DataGridTextBox
        Dim Obj As Object

        Try
            TBox = CType(Me.TextBox, DataGridTextBox)
            Obj = TBox.Text

            Me.HideEditBox()

            If TBox.IsInEditOrNavigateMode Then Return True

            'RaiseEvent UnFormating(obj, rowNum)

            SetColumnValueAtRow(dataSource, rowNum, Obj)   ' Write new value.

            Return True

        Catch ex As Exception
            'Debug.Print(ex.ToString)
            Throw
        Finally
            Me.EndEdit()   ' Let the DataGrid know that processing is completed.
        End Try
    End Function

    Protected Overrides Sub SetColumnValueAtRow(ByVal source As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer, ByVal value As Object)
        Dim Obj As Object

        Try
            Obj = value

            RaiseEvent UnFormatting(Obj, rowNum)

            'MyBase.SetColumnValueAtRow(source, rowNum, obj) ' Write new value.

            If source.Position = rowNum Then MyBase.SetColumnValueAtRow(source, rowNum, Obj) ' Write new value.

        Catch ex As Exception
            'Debug.Print(ex.ToString)
            Throw
        End Try
    End Sub
End Class

