Option Infer On
Option Strict Off

Imports System.Reflection
Imports System.Linq.Expressions
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports System.Diagnostics.Contracts

Public Module ExtensionMethods

    <System.Runtime.CompilerServices.Extension>
    Public Function IsDerivedFromOpenGenericType(ByVal type As Type, ByVal openGenericType As Type) As Boolean
        Contract.Requires(type IsNot Nothing)
        Contract.Requires(openGenericType IsNot Nothing)
        Contract.Requires(openGenericType.IsGenericTypeDefinition)
        Return type.GetTypeHierarchy().Where(Function(t) t.IsGenericType).Select(Function(t) t.GetGenericTypeDefinition()).Any(Function(t) openGenericType.Equals(t))
    End Function

    <System.Runtime.CompilerServices.Extension>
    Public Iterator Function GetTypeHierarchy(ByVal type As Type) As IEnumerable(Of Type)
        Contract.Requires(type IsNot Nothing)
        Dim currentType As Type = type
        Do While currentType IsNot Nothing
            Yield currentType
            currentType = currentType.BaseType
        Loop
    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function HasMethod(ByVal objectToCheck As Object, ByVal methodName As String) As Boolean
        Dim type = objectToCheck.GetType()
        Return type.GetMethod(methodName) IsNot Nothing
    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function HasProperty(ByVal objectToCheck As Object, ByVal propertyName As String) As Boolean
        Dim type = objectToCheck.GetType()
        Return type.GetProperty(propertyName) IsNot Nothing
    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function IsInteger(ByVal value As String) As Boolean
        'Note this will test for whole numbers and not fractions
        If String.IsNullOrEmpty(value) Then
            Return False
        End If
        Return Integer.TryParse(value, 0)
    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function IsNumber(ByVal value As String) As Boolean
        'Note this will test for whole numbers and not fractions
        If String.IsNullOrEmpty(value) Then
            Return False
        End If
        Return IsNumeric(value)
    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function IsShort(ByVal value As String) As Boolean
        If String.IsNullOrEmpty(value) Then
            Return False
        End If
        Return Short.TryParse(value, 0S)
    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function IsDecimal(ByVal value As String) As Boolean
        If String.IsNullOrEmpty(value) Then
            Return False
        End If
        Return Decimal.TryParse(value, 0D)
    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function IsLong(ByVal value As String) As Boolean
        If String.IsNullOrEmpty(value) Then
            Return False
        End If
        Return Long.TryParse(value, 0L)
    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function IsDouble(ByVal value As String) As Boolean
        If String.IsNullOrEmpty(value) Then
            Return False
        End If
        Dim tempNo As Double
        Return Double.TryParse(value, tempNo)
    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function IsRealDate(ByVal value As Object) As Boolean
        'disallows 12:00:00 representation of timestamp
        If String.IsNullOrEmpty(value.ToString) OrElse value.ToString.Trim.Length < 6 Then
            Return False
        End If

        Return UFCWGeneral.ValidateDate(value) IsNot Nothing

    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function IsCharacterAlphaNumeric(ByVal value As Char) As Boolean

        If String.IsNullOrEmpty(value) Then
            Return False
        End If

        Dim Found As Long
        Const AlphaNum As String = "0123456789abcdefghijklmnopqrstuvwxyz"
        Found = InStr(1, AlphaNum, value.ToString.ToLower, vbTextCompare)

        Return If(Found > 0, True, False)

    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function IsCharacterAlphaZeroOrWildcard(ByVal value As Char) As Boolean

        If String.IsNullOrEmpty(value) Then
            Return False
        End If

        Dim Found As Long
        Const AlphaNum As String = "0abcdefghijklmnopqrstuvwxyz*"
        Found = InStr(1, AlphaNum, value.ToString.ToLower, vbTextCompare)

        Return If(Found > 0, True, False)

    End Function


    <System.Runtime.CompilerServices.Extension()>
    Public Function IsCharacterAlphaOrWildcard(ByVal value As Char) As Boolean

        If String.IsNullOrEmpty(value) Then
            Return False
        End If

        Dim Found As Long
        Const AlphaNum As String = "abcdefghijklmnopqrstuvwxyz*"
        Found = InStr(1, AlphaNum, value.ToString.ToLower, vbTextCompare)

        Return If(Found > 0, True, False)

    End Function

    <System.Runtime.CompilerServices.Extension>
    Public Sub Add(Of T)(ByRef arr As T(), item As T)

        If arr IsNot Nothing Then
            Array.Resize(arr, arr.Length + 1)
            arr(arr.Length - 1) = item
        Else
            ReDim arr(0)
            arr(0) = item
        End If
    End Sub

    <System.Runtime.CompilerServices.Extension>
    Public Function ToDataTable(Of T)(collection As IEnumerable(Of T), tableName As String) As DataTable

        Dim DT As DataTable = ToDataTable(collection)

        DT.TableName = tableName

        Return DT

    End Function

    <System.Runtime.CompilerServices.Extension>
    Public Function ToDataTableviaReflected(Of T)(ByVal VarList As IEnumerable(Of T)) As DataTable

        Dim DT As New DataTable()

        ' column names 
        Dim PIs() As PropertyInfo = Nothing

        If VarList Is Nothing Then
            Return DT
        End If

        For Each Var As T In VarList
            ' Use reflection to get property names, to create table, Only first time, others will follow 
            If PIs Is Nothing Then

                PIs = CType(Var.GetType(), Type).GetProperties()

                For Each PI As PropertyInfo In PIs
                    Dim ColPropertyType As Type = PI.ReflectedType

                    If (ColPropertyType.IsGenericType) AndAlso (ColPropertyType.GetGenericTypeDefinition() Is GetType(Nullable(Of ))) Then
                        ColPropertyType = ColPropertyType.GetGenericArguments()(0)
                    End If

                    DT.Columns.Add(New DataColumn(PI.Name, ColPropertyType))
                Next PI
            End If

            Dim DR As DataRow = DT.NewRow()

            For Each PI As PropertyInfo In PIs
                If PI.ReflectedType = GetType(System.String) Then
                    DR(PI.Name) = Var
                Else
                    DR(PI.Name) = If(PI.GetValue(Var, Nothing) Is Nothing, DBNull.Value, PI.GetValue(Var, Nothing))
                End If

            Next PI

            DT.Rows.Add(DR)
        Next Var

        Return DT

    End Function

    <System.Runtime.CompilerServices.Extension>
    Public Function ToDataTable(Of T)(collection As IEnumerable(Of T)) As DataTable


        Dim DT As New DataTable()

        Dim GenericType As Type = GetType(T)

        Dim PIA As PropertyInfo() = GenericType.GetProperties()

        'Create the columns in the DataTable

        For Each PI As PropertyInfo In PIA
            DT.Columns.Add(PI.Name, PI.PropertyType)
        Next

        'Populate the table

        For Each ColItem As T In collection

            Dim DR As DataRow = DT.NewRow()

            DR.BeginEdit()

            For Each PI As PropertyInfo In PIA
                DR(PI.Name) = PI.GetValue(ColItem, Nothing)
            Next

            DR.EndEdit()

            DT.Rows.Add(DR)
        Next

        Return DT

    End Function

    Public Function [True](Of T)() As Expression(Of Func(Of T, Boolean))
        Return Function(f) True
    End Function
    Public Function [False](Of T)() As Expression(Of Func(Of T, Boolean))
        Return Function(f) False
    End Function

    <System.Runtime.CompilerServices.Extension>
    Public Function [Or](Of T)(expr1 As Expression(Of Func(Of T, Boolean)), expr2 As Expression(Of Func(Of T, Boolean))) As Expression(Of Func(Of T, Boolean))
        Dim invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast(Of Expression)())
        Return Expression.Lambda(Of Func(Of T, Boolean))(Expression.[OrElse](expr1.Body, invokedExpr), expr1.Parameters)
    End Function

    <System.Runtime.CompilerServices.Extension>
    Public Function [And](Of T)(expr1 As Expression(Of Func(Of T, Boolean)), expr2 As Expression(Of Func(Of T, Boolean))) As Expression(Of Func(Of T, Boolean))
        Dim invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast(Of Expression)())
        Return Expression.Lambda(Of Func(Of T, Boolean))(Expression.[AndAlso](expr1.Body, invokedExpr), expr1.Parameters)
    End Function

End Module

Public Enum ListViewExtendedStyle
    ''' <summary>
    ''' LVS_EX_GRIDLINES
    ''' </summary>
    GridLines = &H1
    ''' <summary>
    ''' LVS_EX_SUBITEMIMAGES
    ''' </summary>
    SubItemImages = &H2
    ''' <summary>
    ''' LVS_EX_CHECKBOXES
    ''' </summary>
    CheckBoxes = &H4
    ''' <summary>
    ''' LVS_EX_TRACKSELECT
    ''' </summary>
    TrackSelect = &H8
    ''' <summary>
    ''' LVS_EX_HEADERDRAGDROP
    ''' </summary>
    HeaderDragDrop = &H10
    ''' <summary>
    ''' LVS_EX_FULLROWSELECT
    ''' </summary>
    FullRowSelect = &H20
    ''' <summary>
    ''' LVS_EX_ONECLICKACTIVATE
    ''' </summary>
    OneClickActivate = &H40
    ''' <summary>
    ''' LVS_EX_TWOCLICKACTIVATE
    ''' </summary>
    TwoClickActivate = &H80
    ''' <summary>
    ''' LVS_EX_FLATSB
    ''' </summary>
    FlatsB = &H100
    ''' <summary>
    ''' LVS_EX_REGIONAL
    ''' </summary>
    Regional = &H200
    ''' <summary>
    ''' LVS_EX_INFOTIP
    ''' </summary>
    InfoTip = &H400
    ''' <summary>
    ''' LVS_EX_UNDERLINEHOT
    ''' </summary>
    UnderlineHot = &H800
    ''' <summary>
    ''' LVS_EX_UNDERLINECOLD
    ''' </summary>
    UnderlineCold = &H1000
    ''' <summary>
    ''' LVS_EX_MULTIWORKAREAS
    ''' </summary>
    MultilWorkAreas = &H2000
    ''' <summary>
    ''' LVS_EX_LABELTIP
    ''' </summary>
    LabelTip = &H4000
    ''' <summary>
    ''' LVS_EX_BORDERSELECT
    ''' </summary>
    BorderSelect = &H8000
    ''' <summary>
    ''' LVS_EX_DOUBLEBUFFER
    ''' </summary>
    DoubleBuffer = &H10000
    ''' <summary>
    ''' LVS_EX_HIDELABELS
    ''' </summary>
    HideLabels = &H20000
    ''' <summary>
    ''' LVS_EX_SINGLEROW
    ''' </summary>
    SingleRow = &H40000
    ''' <summary>
    ''' LVS_EX_SNAPTOGRID
    ''' </summary>
    SnapToGrid = &H80000
    ''' <summary>
    ''' LVS_EX_SIMPLESELECT
    ''' </summary>
    SimpleSelect = &H100000
End Enum

Public Enum ListViewMessage
    First = &H1000
    SetExtendedStyle = (First + 54)
    GetExtendedStyle = (First + 55)
End Enum

''' <summary>
''' Contains helper methods to change extended styles on ListView, including enabling double buffering.
''' </summary>
Public Class ListViewHelper
    Private Sub New()
    End Sub

    Public Shared Sub SetExtendedStyle(ByVal control As Control, ByVal exStyle As ListViewExtendedStyle)
        Dim styles As ListViewExtendedStyle
        styles = CType(NativeMethods.SendMessage(control.Handle, CInt(ListViewMessage.GetExtendedStyle), 0, 0), ListViewExtendedStyle)
        styles = styles Or exStyle
        NativeMethods.SendMessage(control.Handle, CInt(ListViewMessage.SetExtendedStyle), 0, CInt(styles))
    End Sub

    Public Shared Sub EnableDoubleBuffer(ByVal control As Control)
        Dim styles As ListViewExtendedStyle
        ' read current style
        styles = CType(NativeMethods.SendMessage(control.Handle, CInt(ListViewMessage.GetExtendedStyle), 0, 0), ListViewExtendedStyle)
        ' enable double buffer and border select
        styles = styles Or ListViewExtendedStyle.DoubleBuffer Or ListViewExtendedStyle.BorderSelect
        ' write new style
        NativeMethods.SendMessage(control.Handle, CInt(ListViewMessage.SetExtendedStyle), 0, CInt(styles))
    End Sub
    Public Shared Sub DisableDoubleBuffer(ByVal control As Control)
        Dim styles As ListViewExtendedStyle
        ' read current style
        styles = CType(NativeMethods.SendMessage(control.Handle, CInt(ListViewMessage.GetExtendedStyle), 0, 0), ListViewExtendedStyle)
        ' disable double buffer and border select
        styles -= styles And ListViewExtendedStyle.DoubleBuffer
        styles -= styles And ListViewExtendedStyle.BorderSelect
        ' write new style
        NativeMethods.SendMessage(control.Handle, CInt(ListViewMessage.SetExtendedStyle), 0, CInt(styles))
    End Sub
End Class
