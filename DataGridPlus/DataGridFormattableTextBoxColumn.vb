Option Strict On

Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Text.RegularExpressions


Public Delegate Sub FormatCellEventHandler(ByVal sender As Object, ByVal e As DataGridFormatCellEventArgs)
Public Delegate Sub FormatRowEventHandler(ByVal sender As Object, ByVal e As DataGridFormatRowEventArgs)

<System.Diagnostics.DebuggerStepThrough()>
Public Class DataGridFormattableMultiLineTextBoxColumn
    Inherits DataGridHighlightTextBoxColumn

    Private _CM As CurrencyManager
    Private _RowNumber As Integer
    Private _IsEditing As Boolean = False
    Private _ReadOnly As Boolean = True

    Dim _Disposed As Boolean = False
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)

        If _Disposed Then Return

        If disposing Then
            RemoveHandler MyBase.TextBox.TextChanged, AddressOf TextBoxStartEditing
            RemoveHandler MyBase.TextBox.Leave, AddressOf LeaveTextBox
        End If

        _Disposed = True

        MyBase.Dispose(disposing)
    End Sub

    Public Sub New()
        MyBase.New()

        Me.TextBox.ScrollBars = ScrollBars.Vertical

        RemoveHandler MyBase.TextBox.TextChanged, AddressOf TextBoxStartEditing
        RemoveHandler MyBase.TextBox.Leave, AddressOf LeaveTextBox

        AddHandler MyBase.TextBox.TextChanged, AddressOf TextBoxStartEditing
        AddHandler MyBase.TextBox.Leave, AddressOf LeaveTextBox

    End Sub
    Private Sub TextBoxStartEditing(ByVal sender As Object, ByVal e As EventArgs)
        If Not _ReadOnly Then
            'Debug.Print("TextBoxStartEditing")
            _IsEditing = True
            MyBase.ColumnStartedEditing(CType(sender, Control))
        End If
    End Sub
    Private Sub LeaveTextBox(ByVal sender As Object, ByVal e As EventArgs)
        If _IsEditing Then
            SetColumnValueAtRow(_CM, _RowNumber, MyBase.TextBox.Text)
            _IsEditing = False
        End If
        Invalidate()
    End Sub
    Protected Overloads Overrides Sub Edit(ByVal cm As CurrencyManager, ByVal rowNum As Integer, ByVal bounds As Rectangle, ByVal [readOnly] As Boolean, ByVal instantText As String, ByVal cellIsVisible As Boolean)
        _ReadOnly = [readOnly]

        MyBase.Edit(cm, rowNum, bounds, [readOnly], instantText, cellIsVisible)

        _RowNumber = rowNum
        _CM = cm

    End Sub
    Protected Overloads Overrides Function Commit(ByVal cm As CurrencyManager, ByVal rowNum As Integer) As Boolean

        If _IsEditing Then
            _IsEditing = False

            SetColumnValueAtRow(cm, rowNum, MyBase.TextBox.Text)
        End If

        Me.TextBox.Visible = False
        Return True

    End Function
    Protected Overloads Overrides Sub Paint(ByVal g As Graphics, ByVal bounds As Rectangle, ByVal cm As CurrencyManager, ByVal rowNum As Integer, ByVal backBrush As Brush, ByVal foreBrush As Brush, ByVal alignToRight As Boolean)
        Try

            MyBase.Paint(g, bounds, cm, rowNum, backBrush, foreBrush, alignToRight)

            g.FillRectangle(backBrush, bounds)
            Dim vText As String = CType(MyBase.GetColumnValueAtRow(cm, rowNum), String)
            g.DrawString(vText, Me.TextBox.Font, foreBrush, New RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height))

        Catch ex As Exception
            Throw
        End Try
    End Sub
End Class

<System.Diagnostics.DebuggerStepThrough()>
Public Class DataGridFormattableTextBoxColumn
    Inherits DataGridHighlightTextBoxColumn

    Private _Col As Integer

    Public Sub New(ByVal col As Integer)
        MyBase.New()
        _Col = col
    End Sub

    Public Event SetCellFormat As FormatCellEventHandler

    Public Shadows Function Position() As Integer?
        Return MyBase.Position
    End Function

    ''<System.Diagnostics.DebuggerStepThrough()>
    Protected Overloads Overrides Sub Paint(ByVal g As Graphics, ByVal bounds As Rectangle, ByVal source As CurrencyManager, ByVal rowNum As Integer, ByVal backBrush As Brush, ByVal foreBrush As Brush, ByVal alignToRight As Boolean)

        Dim DGCellArgs As DataGridFormatCellEventArgs
        Dim ColumnValue As Object

        Try

            DGCellArgs = New DataGridFormatCellEventArgs(rowNum, _Col, Me.Format, Me.DataGridTableStyle.DataGrid.Font, backBrush, foreBrush)

            ColumnValue = Me.GetColumnValueAtRow(source, rowNum)

            RaiseEvent SetCellFormat(Me, DGCellArgs)

            If DGCellArgs.UseBaseClassDrawing Then
                MyBase.Paint(g, bounds, source, rowNum, backBrush, foreBrush, alignToRight)
            Else
                g.FillRectangle(DGCellArgs.BackBrush, bounds)

                If Me.FormatIsRegEx Then
                    'not implemented yet
                End If

                If Me.Format.ToString.Trim.Length > 0 AndAlso Not IsDBNull(ColumnValue) Then 'default format handler

                    Select Case Me.Format.ToString.Trim
                        Case "000-00-0000" 'this is necessary due to the uncertainty that the query will return a numeric datatype
                            g.DrawString(String.Format("{0:" & Me.Format.ToString.Trim & "}", CDec(If(Not IsNumeric(ColumnValue), 0, ColumnValue))), DGCellArgs.TextFont, DGCellArgs.ForeBrush, bounds.X, bounds.Y)
                        Case Else
                            g.DrawString(String.Format("{0:" & Me.Format.ToString.Trim & "}", ColumnValue), DGCellArgs.TextFont, DGCellArgs.ForeBrush, bounds.X, bounds.Y)
                    End Select

                Else
                    'If Me.NullText.Length > 0 Then
                    '    If IsDBNull(ColumnValue) OrElse CStr(ColumnValue).Trim = "" Then
                    '        ColumnValue = Me.NullText
                    '    End If
                    'End If
                    g.DrawString(If(ColumnValue, "").ToString, DGCellArgs.TextFont, DGCellArgs.ForeBrush, bounds.X, bounds.Y)
                End If

            End If

            If (DGCellArgs.TextFont Is Me.DataGridTableStyle.DataGrid.Font) = False Then
                DGCellArgs.TextFont.Dispose()
            End If

        Catch ex As Exception 'try repaint using standard method
            MyBase.Paint(g, bounds, source, rowNum, backBrush, foreBrush, alignToRight)
        Finally

            DGCellArgs = Nothing
            ColumnValue = Nothing

        End Try

    End Sub

    Protected Overloads Overrides Sub Edit(ByVal source As CurrencyManager, ByVal rowNum As Integer, ByVal bounds As Rectangle, ByVal [readOnly] As Boolean, ByVal instantText As String, ByVal cellIsVisible As Boolean)

        'comment to make cells unable to become editable
        MyBase.Edit(source, rowNum, bounds, [readOnly], instantText, cellIsVisible)

    End Sub
End Class

<System.Diagnostics.DebuggerStepThrough()>
Public Class DataGridFormatCellEventArgs
    Inherits EventArgs

    Private _column As Integer
    Private _rowNumber As Integer
    Private _font As Font
    Private _backBrush As Brush
    Private _foreBrush As Brush
    Private _useBaseClassDrawing As Boolean
    Private _format As String
    'Fields
    'Constructors
    'Methods
    Public Sub New(ByVal rowNumber As Integer, ByVal col As Integer, ByVal Format As String, ByVal font1 As Font, ByVal backBrush As Brush, ByVal foreBrush As Brush)
        MyBase.New()
        _rowNumber = rowNumber
        _column = col
        _font = font1
        _backBrush = backBrush
        _foreBrush = foreBrush
        _useBaseClassDrawing = False
        _format = Format
    End Sub
    Public Property Column() As Integer
        Get

            Return _column

        End Get
        Set(ByVal Value As Integer)

            _column = Value

        End Set
    End Property
    Public Property RowNumber() As Integer
        Get

            Return _rowNumber

        End Get
        Set(ByVal Value As Integer)

            _rowNumber = Value

        End Set
    End Property
    Public Property TextFont() As Font
        Get

            Return _font

        End Get
        Set(ByVal Value As Font)

            _font = Value

        End Set
    End Property
    Public Property BackBrush() As Brush
        Get

            Return _backBrush

        End Get
        Set(ByVal Value As Brush)

            _backBrush = Value

        End Set
    End Property
    Public Property ForeBrush() As Brush
        Get

            Return _foreBrush

        End Get
        Set(ByVal Value As Brush)

            _foreBrush = Value

        End Set
    End Property
    Public Property UseBaseClassDrawing() As Boolean
        Get

            Return _useBaseClassDrawing

        End Get
        Set(ByVal Value As Boolean)

            _useBaseClassDrawing = Value

        End Set
    End Property
    Public Property Format() As String
        Get

            Return _format

        End Get
        Set(ByVal Value As String)

            _format = Value

        End Set
    End Property
End Class

<System.Diagnostics.DebuggerStepThrough()>
Public Class DataGridFormatRowEventArgs
    Inherits EventArgs
    Private _column As Integer
    Private _rowNumber As Integer
    Private _backBrush As Brush
    Private _foreBrush As Brush
    Private _useBaseClassDrawing As Boolean
    Private _format As String
    Private _font As Font
    'Fields
    'Constructors
    'Methods
    Public Sub New(ByVal rowNumber As Integer, ByVal col As Integer, ByVal Format As String, ByVal font1 As Font, ByVal backBrush As Brush, ByVal foreBrush As Brush)
        MyBase.New()
        _rowNumber = rowNumber
        _column = col
        _font = font1
        _backBrush = backBrush
        _foreBrush = foreBrush
        _useBaseClassDrawing = False
        _format = Format
    End Sub
    Public Property Column() As Integer
        Get

            Return _column

        End Get
        Set(ByVal Value As Integer)

            _column = Value

        End Set
    End Property
    Public Property RowNumber() As Integer
        Get

            Return _rowNumber

        End Get
        Set(ByVal Value As Integer)

            _rowNumber = Value

        End Set
    End Property
    Public Property TextFont() As Font
        Get

            Return _font

        End Get
        Set(ByVal Value As Font)

            _font = Value

        End Set
    End Property
    Public Property BackBrush() As Brush
        Get

            Return _backBrush

        End Get
        Set(ByVal Value As Brush)

            _backBrush = Value

        End Set
    End Property
    Public Property ForeBrush() As Brush
        Get

            Return _foreBrush

        End Get
        Set(ByVal Value As Brush)

            _foreBrush = Value

        End Set
    End Property
    Public Property UseBaseClassDrawing() As Boolean
        Get

            Return _useBaseClassDrawing

        End Get
        Set(ByVal Value As Boolean)

            _useBaseClassDrawing = Value

        End Set
    End Property
    Public Property Format() As String
        Get

            Return _format

        End Get
        Set(ByVal Value As String)

            _format = Value

        End Set
    End Property
End Class



