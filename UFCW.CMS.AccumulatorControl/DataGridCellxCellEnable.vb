Option Strict Off
Option Explicit On

Imports Microsoft.VisualBasic
Imports System

Public Class DataGridEnableEventArgs
    Inherits EventArgs

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _Column As Integer
    Private _Row As Integer
    Private _Enablevalue As Boolean
    'Fields
    'Constructors
    'Methods
    Public Sub New(ByVal row As Integer, ByVal col As Integer, ByVal value As Boolean)
        MyBase.New()
        _row = row
        _column = col
        _enablevalue = value

    End Sub
    Public Property Column() As Integer
        Get

            Return _column

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
    Public Property EnableValue() As Boolean
        Get

            Return _enablevalue

        End Get
        Set(ByVal value As Boolean)

            _Enablevalue = Value

        End Set
    End Property
End Class