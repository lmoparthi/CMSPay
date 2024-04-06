'<System.Diagnostics.DebuggerStepThrough()>
Public Class DataGridIconColumn
    Inherits System.Windows.Forms.DataGridTextBoxColumn

    Private WithEvents _Icons As ImageList

    Private _CurrentImage As Image
    Private _IconOnly As Boolean = False
    Private _StretchIcon As Boolean = False

    Public Event PaintCellPicture(ByRef Pic As Image, ByVal Cell As DataGridCell)
    Public Event Formatting(ByRef Value As Object, ByVal RowIndex As Integer)
    Public Event UnFormatting(ByRef Value As Object, ByVal RowIndex As Integer)

    Dim _Disposed As Boolean = False
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)

        If _Disposed Then Return

        If disposing Then

            _Icons = Nothing
            If _CurrentImage IsNot Nothing Then _CurrentImage.Dispose()
            _CurrentImage = Nothing

        End If

        _Disposed = True

        MyBase.Dispose(disposing)
    End Sub
    Public Overridable Property IconOnly() As Boolean
        Get
            Return _IconOnly
        End Get
        Set(ByVal Value As Boolean)
            _IconOnly = Value
        End Set
    End Property

    Public Overridable Property StretchToCell() As Boolean
        Get
            Return _StretchIcon
        End Get
        Set(ByVal Value As Boolean)
            _StretchIcon = Value
        End Set
    End Property

    Public Overridable Property Position() As Integer?
        Get
            If Not Me.DataGridTableStyle Is Nothing Then
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

    Public Sub New(ByVal dg As DataGrid, Optional ByVal icons As ImageList = Nothing)
        MyBase.New()

        If icons IsNot Nothing Then _Icons = icons
    End Sub

    Private Function GetPic(ByVal cell As DataGridCell) As Image
        Try

            RaiseEvent PaintCellPicture(_CurrentImage, cell)
            Return _CurrentImage

        Catch ex As Exception
            Throw
        End Try
    End Function

    Protected Overloads Overrides Sub Paint(ByVal g As Graphics, ByVal bounds As Rectangle, ByVal cm As CurrencyManager, ByVal rowNum As Integer, ByVal backBrush As Brush, ByVal foreBrush As Brush, ByVal alignToRight As Boolean)
        Dim DGTS As DataGridTableStyle
        Dim Cell As DataGridCell
        Dim Rect As Rectangle
        Dim S As Size

        Try

            DGTS = New DataGridTableStyle(cm)
            Cell = New DataGridCell(rowNum, DGTS.GridColumnStyles.IndexOf(DGTS.GridColumnStyles(MyBase.MappingName)))

            _CurrentImage = GetPic(Cell)

            If _CurrentImage IsNot Nothing Then
                If _StretchIcon = False Then
                    S = New Size(_CurrentImage.Size.Width, _CurrentImage.Size.Height)
                Else
                    If _IconOnly = True Then
                        S = New Size(bounds.Width, bounds.Height)
                    Else
                        S = New Size(_CurrentImage.Size.Width, bounds.Height)
                    End If
                End If
                Rect = New Rectangle(bounds.X, bounds.Y, S.Width, S.Height)
                g.FillRectangle(backBrush, bounds)
                g.FillRectangle(backBrush, Rect)
                g.DrawImage(_CurrentImage, Rect)
                bounds.X = (bounds.X + Rect.Width)
                bounds.Width = (bounds.Width - Rect.Width)
            Else
                'erase background
                g.FillRectangle(backBrush, bounds)
            End If

            MyBase.Paint(g, bounds, cm, rowNum, backBrush, foreBrush, alignToRight)

        Catch IgnoreException As Exception
            'Debug.Print(IgnoreException.ToString)
        Finally
            Cell = Nothing

            If DGTS IsNot Nothing Then DGTS.Dispose()
            DGTS = Nothing
        End Try
    End Sub

    Protected Overloads Overrides Sub Edit(ByVal cm As CurrencyManager, ByVal rowNum As Integer, ByVal bounds As Rectangle, ByVal [readOnly] As Boolean, ByVal instantText As String, ByVal cellIsVisible As Boolean)
        Dim DGTS As DataGridTableStyle
        Dim Cell As DataGridCell

        Try
            DGTS = New DataGridTableStyle(cm)
            Cell = New DataGridCell(rowNum, DGTS.GridColumnStyles.IndexOf(DGTS.GridColumnStyles(MyBase.MappingName)))

            If _IconOnly = False Then

                _CurrentImage = GetPic(Cell)

                If _CurrentImage IsNot Nothing Then
                    bounds.X += _CurrentImage.Size.Width

                    bounds.Width += -_CurrentImage.Size.Width
                Else
                    bounds.Y -= 2
                    bounds.X -= 2

                    bounds.Height += 2
                    bounds.Width += 2
                End If

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
        Dim Box As DataGridTextBox
        Dim Obj As Object

        Try
            Box = CType(Me.TextBox, DataGridTextBox)
            Obj = Box.Text

            Me.HideEditBox()

            If Box.IsInEditOrNavigateMode Then Return True

            RaiseEvent UnFormatting(Obj, rowNum)

            SetColumnValueAtRow(cm, rowNum, Obj)   ' Write new value.

            Return True
        Catch ex As Exception
            Throw
        Finally

            Me.EndEdit()

            Obj = Nothing
            Box = Nothing
        End Try
    End Function

End Class

