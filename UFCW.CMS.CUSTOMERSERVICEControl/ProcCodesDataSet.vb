﻿'------------------------------------------------------------------------------
' <autogenerated>
'     This code was generated by a tool.
'     Runtime Version: 1.1.4322.2032
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </autogenerated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.Data
Imports System.Runtime.Serialization
Imports System.Xml

<Serializable(), _
 System.ComponentModel.DesignerCategoryAttribute("code"), _
 System.Diagnostics.DebuggerStepThrough(), _
 System.ComponentModel.ToolboxItem(True)> _
Public Class ProcCodesDataSet
    Inherits DataSet

    Private tablePROCEDURE_VALUES As PROCEDURE_VALUESDataTable

    Public Sub New()
        MyBase.New()
        Me.InitClass()
        Dim schemaChangedHandler As System.ComponentModel.CollectionChangeEventHandler = AddressOf Me.SchemaChanged
        AddHandler Me.Tables.CollectionChanged, schemaChangedHandler
        AddHandler Me.Relations.CollectionChanged, schemaChangedHandler
    End Sub

    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.New()
        Dim strSchema As String = CType(info.GetValue("XmlSchema", GetType(System.String)), String)
        If (Not (strSchema) Is Nothing) Then
            Dim ds As DataSet = New DataSet
            ds.ReadXmlSchema(New XmlTextReader(New System.IO.StringReader(strSchema)))
            If (Not (ds.Tables("PROCEDURE_VALUES")) Is Nothing) Then
                Me.Tables.Add(New PROCEDURE_VALUESDataTable(ds.Tables("PROCEDURE_VALUES")))
            End If
            Me.DataSetName = ds.DataSetName
            Me.Prefix = ds.Prefix
            Me.Namespace = ds.Namespace
            Me.Locale = ds.Locale
            Me.CaseSensitive = ds.CaseSensitive
            Me.EnforceConstraints = ds.EnforceConstraints
            Me.Merge(ds, False, System.Data.MissingSchemaAction.Add)
            Me.InitVars()
        Else
            Me.InitClass()
        End If
        Me.GetSerializationData(info, context)
        Dim schemaChangedHandler As System.ComponentModel.CollectionChangeEventHandler = AddressOf Me.SchemaChanged
        AddHandler Me.Tables.CollectionChanged, schemaChangedHandler
        AddHandler Me.Relations.CollectionChanged, schemaChangedHandler
    End Sub

    <System.ComponentModel.Browsable(False), _
     System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)> _
    Public ReadOnly Property PROCEDURE_VALUES As PROCEDURE_VALUESDataTable
        Get
            Return Me.tablePROCEDURE_VALUES
        End Get
    End Property

    Public Overrides Function Clone() As DataSet
        Dim cln As ProcCodesDataSet = CType(MyBase.Clone, ProcCodesDataSet)
        cln.InitVars()
        Return cln
    End Function

    Protected Overrides Function ShouldSerializeTables() As Boolean
        Return False
    End Function

    Protected Overrides Function ShouldSerializeRelations() As Boolean
        Return False
    End Function

    Protected Overrides Sub ReadXmlSerializable(ByVal reader As XmlReader)
        Me.Reset()
        Dim ds As DataSet = New DataSet
        ds.ReadXml(reader)
        If (Not (ds.Tables("PROCEDURE_VALUES")) Is Nothing) Then
            Me.Tables.Add(New PROCEDURE_VALUESDataTable(ds.Tables("PROCEDURE_VALUES")))
        End If
        Me.DataSetName = ds.DataSetName
        Me.Prefix = ds.Prefix
        Me.Namespace = ds.Namespace
        Me.Locale = ds.Locale
        Me.CaseSensitive = ds.CaseSensitive
        Me.EnforceConstraints = ds.EnforceConstraints
        Me.Merge(ds, False, System.Data.MissingSchemaAction.Add)
        Me.InitVars()
    End Sub

    Protected Overrides Function GetSchemaSerializable() As System.Xml.Schema.XmlSchema
        Dim stream As System.IO.MemoryStream = New System.IO.MemoryStream
        Me.WriteXmlSchema(New XmlTextWriter(stream, Nothing))
        stream.Position = 0
        Return System.Xml.Schema.XmlSchema.Read(New XmlTextReader(stream), Nothing)
    End Function

    Friend Sub InitVars()
        Me.tablePROCEDURE_VALUES = CType(Me.Tables("PROCEDURE_VALUES"), PROCEDURE_VALUESDataTable)
        If (Not (Me.tablePROCEDURE_VALUES) Is Nothing) Then
            Me.tablePROCEDURE_VALUES.InitVars()
        End If
    End Sub

    Private Sub InitClass()
        Me.DataSetName = "ProcCodesDataSet"
        Me.Prefix = ""
        Me.Namespace = "http://www.tempuri.org/ProcCodesDataSet.xsd"
        Me.Locale = New System.Globalization.CultureInfo("en-US")
        Me.CaseSensitive = False
        Me.EnforceConstraints = True
        Me.tablePROCEDURE_VALUES = New PROCEDURE_VALUESDataTable
        Me.Tables.Add(Me.tablePROCEDURE_VALUES)
    End Sub

    Private Function ShouldSerializePROCEDURE_VALUES() As Boolean
        Return False
    End Function

    Private Sub SchemaChanged(ByVal sender As Object, ByVal e As System.ComponentModel.CollectionChangeEventArgs)
        If (e.Action = System.ComponentModel.CollectionChangeAction.Remove) Then
            Me.InitVars()
        End If
    End Sub

    Public Delegate Sub PROCEDURE_VALUESRowChangeEventHandler(ByVal sender As Object, ByVal e As PROCEDURE_VALUESRowChangeEvent)

    <System.Diagnostics.DebuggerStepThrough()> _
    Public Class PROCEDURE_VALUESDataTable
        Inherits DataTable
        Implements System.Collections.IEnumerable

        Private columnPROC_VALUE As DataColumn

        Private columnFROM_DATE As DataColumn

        Private columnTHRU_DATE As DataColumn

        Private columnSHORT_DESC As DataColumn

        Private columnFULL_DESC As DataColumn

        Private columnLASTUPDT As DataColumn

        Friend Sub New()
            MyBase.New("PROCEDURE_VALUES")
            Me.InitClass()
        End Sub

        Friend Sub New(ByVal table As DataTable)
            MyBase.New(table.TableName)
            If (table.CaseSensitive <> table.DataSet.CaseSensitive) Then
                Me.CaseSensitive = table.CaseSensitive
            End If
            If (table.Locale.ToString <> table.DataSet.Locale.ToString) Then
                Me.Locale = table.Locale
            End If
            If (table.Namespace <> table.DataSet.Namespace) Then
                Me.Namespace = table.Namespace
            End If
            Me.Prefix = table.Prefix
            Me.MinimumCapacity = table.MinimumCapacity
            Me.DisplayExpression = table.DisplayExpression
        End Sub

        <System.ComponentModel.Browsable(False)> _
        Public ReadOnly Property Count As Integer
            Get
                Return Me.Rows.Count
            End Get
        End Property

        Friend ReadOnly Property PROC_VALUEColumn As DataColumn
            Get
                Return Me.columnPROC_VALUE
            End Get
        End Property

        Friend ReadOnly Property FROM_DATEColumn As DataColumn
            Get
                Return Me.columnFROM_DATE
            End Get
        End Property

        Friend ReadOnly Property THRU_DATEColumn As DataColumn
            Get
                Return Me.columnTHRU_DATE
            End Get
        End Property

        Friend ReadOnly Property SHORT_DESCColumn As DataColumn
            Get
                Return Me.columnSHORT_DESC
            End Get
        End Property

        Friend ReadOnly Property FULL_DESCColumn As DataColumn
            Get
                Return Me.columnFULL_DESC
            End Get
        End Property

        Friend ReadOnly Property LASTUPDTColumn As DataColumn
            Get
                Return Me.columnLASTUPDT
            End Get
        End Property

        Default Public ReadOnly Property Item(ByVal index As Integer) As PROCEDURE_VALUESRow
            Get
                Return CType(Me.Rows(index), PROCEDURE_VALUESRow)
            End Get
        End Property

        Public Event PROCEDURE_VALUESRowChanged As PROCEDURE_VALUESRowChangeEventHandler

        Public Event PROCEDURE_VALUESRowChanging As PROCEDURE_VALUESRowChangeEventHandler

        Public Event PROCEDURE_VALUESRowDeleted As PROCEDURE_VALUESRowChangeEventHandler

        Public Event PROCEDURE_VALUESRowDeleting As PROCEDURE_VALUESRowChangeEventHandler

        Public Overloads Sub AddPROCEDURE_VALUESRow(ByVal row As PROCEDURE_VALUESRow)
            Me.Rows.Add(row)
        End Sub

        Public Overloads Function AddPROCEDURE_VALUESRow(ByVal PROC_VALUE As String, ByVal FROM_DATE As Date, ByVal THRU_DATE As Date, ByVal SHORT_DESC As String, ByVal FULL_DESC As String, ByVal LASTUPDT As Date) As PROCEDURE_VALUESRow
            Dim RowPROCEDURE_VALUESRow As PROCEDURE_VALUESRow = CType(Me.NewRow, PROCEDURE_VALUESRow)
            rowPROCEDURE_VALUESRow.ItemArray = New Object() {PROC_VALUE, FROM_DATE, THRU_DATE, SHORT_DESC, FULL_DESC, LASTUPDT}
            Me.Rows.Add(rowPROCEDURE_VALUESRow)
            Return rowPROCEDURE_VALUESRow
        End Function

        Public Function FindByPROC_VALUEFROM_DATE(ByVal PROC_VALUE As String, ByVal FROM_DATE As Date) As PROCEDURE_VALUESRow
            Return CType(Me.Rows.Find(New Object() {PROC_VALUE, FROM_DATE}), PROCEDURE_VALUESRow)
        End Function

        Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
            Return Me.Rows.GetEnumerator
        End Function

        Public Overrides Function Clone() As DataTable
            Dim cln As PROCEDURE_VALUESDataTable = CType(MyBase.Clone, PROCEDURE_VALUESDataTable)
            cln.InitVars()
            Return cln
        End Function

        Protected Overrides Function CreateInstance() As DataTable
            Return New PROCEDURE_VALUESDataTable
        End Function

        Friend Sub InitVars()
            Me.columnPROC_VALUE = Me.Columns("PROC_VALUE")
            Me.columnFROM_DATE = Me.Columns("FROM_DATE")
            Me.columnTHRU_DATE = Me.Columns("THRU_DATE")
            Me.columnSHORT_DESC = Me.Columns("SHORT_DESC")
            Me.columnFULL_DESC = Me.Columns("FULL_DESC")
            Me.columnLASTUPDT = Me.Columns("LASTUPDT")
        End Sub

        Private Sub InitClass()
            Me.columnPROC_VALUE = New DataColumn("PROC_VALUE", GetType(System.String), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnPROC_VALUE)
            Me.columnFROM_DATE = New DataColumn("FROM_DATE", GetType(System.DateTime), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnFROM_DATE)
            Me.columnTHRU_DATE = New DataColumn("THRU_DATE", GetType(System.DateTime), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnTHRU_DATE)
            Me.columnSHORT_DESC = New DataColumn("SHORT_DESC", GetType(System.String), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnSHORT_DESC)
            Me.columnFULL_DESC = New DataColumn("FULL_DESC", GetType(System.String), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnFULL_DESC)
            Me.columnLASTUPDT = New DataColumn("LASTUPDT", GetType(System.DateTime), Nothing, System.Data.MappingType.Element)
            Me.Columns.Add(Me.columnLASTUPDT)
            Me.Constraints.Add(New UniqueConstraint("Constraint1", New DataColumn() {Me.columnPROC_VALUE, Me.columnFROM_DATE}, True))
            Me.columnPROC_VALUE.AllowDBNull = False
            Me.columnFROM_DATE.AllowDBNull = False
            Me.columnTHRU_DATE.AllowDBNull = False
            Me.columnSHORT_DESC.AllowDBNull = False
            Me.columnFULL_DESC.AllowDBNull = False
            Me.columnLASTUPDT.AllowDBNull = False
        End Sub

        Public Function NewPROCEDURE_VALUESRow() As PROCEDURE_VALUESRow
            Return CType(Me.NewRow, PROCEDURE_VALUESRow)
        End Function

        Protected Overrides Function NewRowFromBuilder(ByVal builder As DataRowBuilder) As DataRow
            Return New PROCEDURE_VALUESRow(builder)
        End Function

        Protected Overrides Function GetRowType() As System.Type
            Return GetType(PROCEDURE_VALUESRow)
        End Function

        Protected Overrides Sub OnRowChanged(ByVal e As DataRowChangeEventArgs)
            MyBase.OnRowChanged(e)
            If (Not (Me.PROCEDURE_VALUESRowChangedEvent) Is Nothing) Then
                RaiseEvent PROCEDURE_VALUESRowChanged(Me, New PROCEDURE_VALUESRowChangeEvent(CType(e.Row, PROCEDURE_VALUESRow), e.Action))
            End If
        End Sub

        Protected Overrides Sub OnRowChanging(ByVal e As DataRowChangeEventArgs)
            MyBase.OnRowChanging(e)
            If (Not (Me.PROCEDURE_VALUESRowChangingEvent) Is Nothing) Then
                RaiseEvent PROCEDURE_VALUESRowChanging(Me, New PROCEDURE_VALUESRowChangeEvent(CType(e.Row, PROCEDURE_VALUESRow), e.Action))
            End If
        End Sub

        Protected Overrides Sub OnRowDeleted(ByVal e As DataRowChangeEventArgs)
            MyBase.OnRowDeleted(e)
            If (Not (Me.PROCEDURE_VALUESRowDeletedEvent) Is Nothing) Then
                RaiseEvent PROCEDURE_VALUESRowDeleted(Me, New PROCEDURE_VALUESRowChangeEvent(CType(e.Row, PROCEDURE_VALUESRow), e.Action))
            End If
        End Sub

        Protected Overrides Sub OnRowDeleting(ByVal e As DataRowChangeEventArgs)
            MyBase.OnRowDeleting(e)
            If (Not (Me.PROCEDURE_VALUESRowDeletingEvent) Is Nothing) Then
                RaiseEvent PROCEDURE_VALUESRowDeleting(Me, New PROCEDURE_VALUESRowChangeEvent(CType(e.Row, PROCEDURE_VALUESRow), e.Action))
            End If
        End Sub

        Public Sub RemovePROCEDURE_VALUESRow(ByVal row As PROCEDURE_VALUESRow)
            Me.Rows.Remove(row)
        End Sub
    End Class

    <System.Diagnostics.DebuggerStepThrough()> _
    Public Class PROCEDURE_VALUESRow
        Inherits DataRow

        Private tablePROCEDURE_VALUES As PROCEDURE_VALUESDataTable

        Friend Sub New(ByVal rb As DataRowBuilder)
            MyBase.New(rb)
            Me.tablePROCEDURE_VALUES = CType(Me.Table, PROCEDURE_VALUESDataTable)
        End Sub

        Public Property PROC_VALUE As String
            Get
                Return CType(Me(Me.tablePROCEDURE_VALUES.PROC_VALUEColumn), String)
            End Get
            Set(value As String)
                Me(Me.tablePROCEDURE_VALUES.PROC_VALUEColumn) = Value
            End Set
        End Property

        Public Property FROM_DATE As Date
            Get
                Return CType(Me(Me.tablePROCEDURE_VALUES.FROM_DATEColumn), Date)
            End Get
            Set(value As Date)
                Me(Me.tablePROCEDURE_VALUES.FROM_DATEColumn) = Value
            End Set
        End Property

        Public Property THRU_DATE As Date
            Get
                Return CType(Me(Me.tablePROCEDURE_VALUES.THRU_DATEColumn), Date)
            End Get
            Set(value As Date)
                Me(Me.tablePROCEDURE_VALUES.THRU_DATEColumn) = Value
            End Set
        End Property

        Public Property SHORT_DESC As String
            Get
                Return CType(Me(Me.tablePROCEDURE_VALUES.SHORT_DESCColumn), String)
            End Get
            Set(value As String)
                Me(Me.tablePROCEDURE_VALUES.SHORT_DESCColumn) = Value
            End Set
        End Property

        Public Property FULL_DESC As String
            Get
                Return CType(Me(Me.tablePROCEDURE_VALUES.FULL_DESCColumn), String)
            End Get
            Set(value As String)
                Me(Me.tablePROCEDURE_VALUES.FULL_DESCColumn) = Value
            End Set
        End Property

        Public Property LASTUPDT As Date
            Get
                Return CType(Me(Me.tablePROCEDURE_VALUES.LASTUPDTColumn), Date)
            End Get
            Set(value As Date)
                Me(Me.tablePROCEDURE_VALUES.LASTUPDTColumn) = Value
            End Set
        End Property
    End Class

    <System.Diagnostics.DebuggerStepThrough()> _
    Public Class PROCEDURE_VALUESRowChangeEvent
        Inherits EventArgs

        Private eventRow As PROCEDURE_VALUESRow

        Private eventAction As DataRowAction

        Public Sub New(ByVal row As PROCEDURE_VALUESRow, ByVal action As DataRowAction)
            MyBase.New()
            Me.eventRow = row
            Me.eventAction = action
        End Sub

        Public ReadOnly Property Row As PROCEDURE_VALUESRow
            Get
                Return Me.eventRow
            End Get
        End Property

        Public ReadOnly Property Action As DataRowAction
            Get
                Return Me.eventAction
            End Get
        End Property
    End Class
End Class