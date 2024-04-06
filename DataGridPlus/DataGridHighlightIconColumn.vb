'<System.Diagnostics.DebuggerStepThrough()>
Public Class DataGridHighlightIconColumn
    Inherits DataGridHighlightTextBoxColumn

    Private WithEvents _Icons As ImageList

    Private _CurrentImage As Image
    Private _IconOnly As Boolean = False
    Private _StretchIcon As Boolean = False

    Public Event PaintCellPicture(ByRef Pic As Image, ByVal Cell As DataGridCell)

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

    Dim _Disposed As Boolean = False
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)

        If _Disposed Then Return

        If disposing Then
            If _CurrentImage IsNot Nothing Then _CurrentImage.Dispose()
            _CurrentImage = Nothing

            _Icons = Nothing

        End If

        _Disposed = True

        MyBase.Dispose(disposing)
    End Sub

    Public Sub New(ByVal dg As DataGrid, Optional ByVal icons As ImageList = Nothing)
        MyBase.New()

        If icons IsNot Nothing Then _Icons = icons
    End Sub

    Private Function GetPic(ByVal cell As DataGridCell) As Image
        RaiseEvent PaintCellPicture(_CurrentImage, cell)
        Return _CurrentImage
    End Function

    Protected Overloads Overrides Sub Paint(ByVal g As Graphics, ByVal bounds As Rectangle, ByVal source As CurrencyManager, ByVal rowNum As Integer, ByVal backBrush As Brush, ByVal foreBrush As Brush, ByVal alignToRight As Boolean)

        Dim DGTS As DataGridTableStyle
        Dim Cell As DataGridCell
        Dim Rect As Rectangle
        Dim S As Size

        Try

            DGTS = New DataGridTableStyle(source)
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

            MyBase.Paint(g, bounds, source, rowNum, backBrush, foreBrush, alignToRight)

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

                _CurrentImage = Nothing
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
End Class
