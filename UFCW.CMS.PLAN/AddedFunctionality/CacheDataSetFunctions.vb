Imports System
Imports System.Collections.Generic
Imports System.Data

Public Module CacheDataSetFunctions

    Public Function MergeWildCardSequenceDataSets(newDataSet As DataSet, masterDataSet As DataSet) As DataSet
        Dim mergedDataSet As New DataSet()

        If masterDataSet Is Nothing Then
            masterDataSet = New DataSet
        End If

        If newDataSet.Tables.Count > 0 AndAlso masterDataSet.Tables.Count = 0 Then
            ' Copy schema from DataSet_A to DataSet_B
            For Each sourceTable As DataTable In newDataSet.Tables
                Dim newTable As New DataTable(sourceTable.TableName)
                For Each column As DataColumn In sourceTable.Columns
                    newTable.Columns.Add(column.ColumnName, column.DataType)
                Next
                masterDataSet.Tables.Add(newTable)
            Next
        End If

        If newDataSet.Tables.Count <> masterDataSet.Tables.Count Then
            Throw New ArgumentException("Both data sets must have the same number of tables.")
        End If

        For tableIndex As Integer = 0 To newDataSet.Tables.Count - 1
            Dim newTable As DataTable = newDataSet.Tables(tableIndex)
            Dim masterTable As DataTable = masterDataSet.Tables(tableIndex)

            ' Set primary key columns for both tables
            If tableIndex = 0 Then
                SetTablePrimaryKeyColumnNames(newTable, {"PROC_ID"}) ' Adjust column names
                SetTablePrimaryKeyColumnNames(masterTable, {"PROC_ID"}) ' Adjust column names

            Else
                SetPrimaryKeyColumns(newTable)
                SetPrimaryKeyColumns(masterTable)

            End If

            ' Merge the contents of newTable into masterTable using the .Merge method
            masterTable.Merge(newTable, False, MissingSchemaAction.Ignore)

            ' Copy the merged masterTable to the mergedDataSet
            mergedDataSet.Tables.Add(masterTable.Copy())
        Next

        Return mergedDataSet
    End Function

    Private Sub SetTablePrimaryKeyColumnNames(myDataTable As DataTable, columnNames() As String)
        Dim primaryKeyColumns As New List(Of DataColumn)

        For Each columnName As String In columnNames
            If myDataTable.Columns.Contains(columnName) Then
                primaryKeyColumns.Add(myDataTable.Columns(columnName))
            End If
        Next

        If primaryKeyColumns.Count > 0 Then
            myDataTable.PrimaryKey = primaryKeyColumns.ToArray()
        End If
    End Sub
    Private Sub SetPrimaryKeyColumns(table As DataTable)
        If table.PrimaryKey Is Nothing OrElse table.PrimaryKey.Length = 0 Then
            Dim primaryKeyColumns(table.Columns.Count - 1) As DataColumn
            For i As Integer = 0 To primaryKeyColumns.Length - 1
                primaryKeyColumns(i) = table.Columns(i)
            Next
            table.PrimaryKey = primaryKeyColumns
        End If
    End Sub

 Private Function BuildDataTable_MasterPlanTypeSequenceNumber(ByVal dataTableName As String) As DataTable

        Dim result As DataTable = Nothing

        Try

            result = New DataTable(dataTableName)

            result.Columns.Add("PLAN_TYPE", System.Type.GetType("System.String"))
            result.Columns.Add("SEQ_NBR", System.Type.GetType("System.Int32"))
            result.Columns.Add("OCC_FROM_DATE", System.Type.GetType("System.Date"))
            result.Columns.Add("OCC_TO_DATE", System.Type.GetType("System.Date"))
            result.Columns.Add("PUBLISH_DATE", System.Type.GetType("System.Date"))

            result.PrimaryKey = New DataColumn() {result.Columns("PLAN_TYPE"), result.Columns("SEQ_NBR"), result.Columns("OCC_FROM_DATE"), result.Columns("OCC_TO_DATE")}

        Catch ex As Exception
            Debug.Print("Exception occurred: '{0}'", ex.Message)

        End Try

        Return result

    End Function

End Module




