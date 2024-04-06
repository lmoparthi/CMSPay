Imports System
Imports System.Drawing
Imports System.Windows.Forms

Public Class DataGridFormattableTextBoxColumn
    Inherits DataGridHighlightTextBoxColumn
    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _Column As Integer
    Public Sub New(ByVal column As Integer)
        MyBase.New()
        _Column = column
    End Sub
    Public Event SetCellFormat As FormatCellEventHandler
    Public Shadows Event SetRowFormat As FormatRowEventHandler

    Protected Overloads Overrides Sub Paint(ByVal g As Graphics, ByVal bounds As Rectangle, ByVal source As CurrencyManager, ByVal rowNum As Integer, ByVal backBrush As Brush, ByVal foreBrush As Brush, ByVal alignToRight As Boolean)
        Dim E As DataGridFormatRowEventArgs
        E = New DataGridFormatRowEventArgs(rowNum, Me._Column, Me.DataGridTableStyle.DataGrid.Font, backBrush, foreBrush)
        RaiseEvent SetRowFormat(Me, e)
        MyBase.Paint(g, bounds, source, rowNum, e.BackBrush, e.ForeBrush, alignToRight)
    End Sub

    Protected Overloads Overrides Sub Edit(ByVal source As CurrencyManager, ByVal rowNum As Integer, ByVal bounds As Rectangle, ByVal [readOnly] As Boolean, ByVal instantText As String, ByVal cellIsVisible As Boolean)

        'comment to make cells unable to become editable
        MyBase.Edit(source, rowNum, bounds, [readOnly], instantText, cellIsVisible)

    End Sub
End Class

Public Class DataGridFormatCellEventArgs
    Inherits EventArgs

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _Column As Integer
    Private _Row As Integer
    Private _BackBrush As Brush
    Private _ForeBrush As Brush
    'Private _useBaseClassDrawing As Boolean
    Private _Format As String
    'Fields
    'Constructors
    'Methods
    Public Sub New(ByVal row As Integer, ByVal col As Integer, ByVal font1 As Font, ByVal backBrush As Brush, ByVal foreBrush As Brush)
        MyBase.New()
        _row = row
        _column = col
        _backBrush = backBrush
        _foreBrush = foreBrush
        '_useBaseClassDrawing = False
        _format = Format
    End Sub
    Public Property Column() As Integer
        Get

            Return _Column

        End Get
        Set(ByVal value As Integer)

            _Column = Value

        End Set
    End Property
    Public Property Row() As Integer
        Get

            Return _row

        End Get
        Set(ByVal value As Integer)

            _Row = Value

        End Set
    End Property
    Public Property BackBrush() As Brush
        Get

            Return _backBrush

        End Get
        Set(ByVal value As Brush)

            _BackBrush = Value

        End Set
    End Property
    Public Property ForeBrush() As Brush
        Get

            Return _foreBrush

        End Get
        Set(ByVal value As Brush)

            _ForeBrush = Value

        End Set
    End Property
    'Public Property UseBaseClassDrawing() As Boolean
    '    Get

    '        Return _useBaseClassDrawing

    '    End Get
    '    Set(ByVal value As Boolean)

    '        _useBaseClassDrawing = Value

    '    End Set
    'End Property
    Public Property Format() As String
        Get

            Return _format

        End Get
        Set(ByVal value As String)

            _Format = Value

        End Set
    End Property
End Class

Public Class DataGridFormatRowEventArgs
    Inherits EventArgs
    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _Column As Integer
    Private _Row As Integer
    Private _BackBrush As Brush
    Private _ForeBrush As Brush
    Private _Format As String

    Public Sub New(ByVal row As Integer, ByVal col As Integer, ByVal font1 As Font, ByVal backBrush As Brush, ByVal foreBrush As Brush)
        MyBase.New()
        _Row = row
        _Column = col
        '_font = font1
        _BackBrush = backBrush
        _ForeBrush = foreBrush
        '_useBaseClassDrawing = False
        '_format = format
    End Sub
    Public Property Column() As Integer
        Get

            Return _Column

        End Get
        Set(ByVal value As Integer)

            _Column = Value

        End Set
    End Property
    Public Property Row() As Integer
        Get

            Return _Row

        End Get
        Set(ByVal value As Integer)

            _Row = Value

        End Set
    End Property
    'Public Property TextFont() As Font
    '    Get

    '        Return _font

    '    End Get
    '    Set(ByVal value As Font)

    '        _font = Value

    '    End Set
    'End Property
    Public Property BackBrush() As Brush
        Get

            Return _BackBrush

        End Get
        Set(ByVal value As Brush)

            _BackBrush = Value

        End Set
    End Property
    Public Property ForeBrush() As Brush
        Get

            Return _ForeBrush

        End Get
        Set(ByVal value As Brush)

            _ForeBrush = Value

        End Set
    End Property
    'Public Property UseBaseClassDrawing() As Boolean
    '    Get

    '        Return _useBaseClassDrawing

    '    End Get
    '    Set(ByVal value As Boolean)

    '        _useBaseClassDrawing = Value

    '    End Set
    'End Property
    Public Property Format() As String
        Get

            Return _Format

        End Get
        Set(ByVal value As String)

            _Format = Value

        End Set
    End Property
End Class

Public Delegate Sub FormatCellEventHandler(ByVal sender As Object, ByVal e As DataGridFormatCellEventArgs)
Public Delegate Sub FormatRowEventHandler(ByVal sender As Object, ByVal e As DataGridFormatRowEventArgs)

Public Class DataGridColorBoolColumn
    Inherits DataGridHighlightBoolColumn

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _Column As Integer
    Public Sub New(ByVal column As Integer)
        MyBase.New()
        _Column = column
    End Sub
    Public Event SetCellFormat As FormatCellEventHandler
    Public Event SetRowFormat As FormatRowEventHandler
    Protected Overloads Overrides Sub Paint(ByVal g As Graphics, ByVal bounds As Rectangle, ByVal source As CurrencyManager, ByVal rowNum As Integer, ByVal backBrush As Brush, ByVal foreBrush As Brush, ByVal alignToRight As Boolean)
        Dim E As DataGridFormatCellEventArgs
        E = New DataGridFormatCellEventArgs(rowNum, Me._Column, Me.DataGridTableStyle.DataGrid.Font, backBrush, foreBrush)
        RaiseEvent SetCellFormat(Me, E)
        MyBase.Paint(g, bounds, source, rowNum, E.BackBrush, E.ForeBrush, alignToRight)
    End Sub

    Protected Overloads Overrides Sub Edit(ByVal source As CurrencyManager, ByVal rowNum As Integer, ByVal bounds As Rectangle, ByVal [readOnly] As Boolean, ByVal instantText As String, ByVal cellIsVisible As Boolean)

        'comment to make cells unable to become editable
        MyBase.Edit(source, rowNum, bounds, [readOnly], instantText, cellIsVisible)

    End Sub
End Class