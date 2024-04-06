Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Reflection
Imports System.Runtime.CompilerServices

Module CollectionExtensions

    <System.Runtime.CompilerServices.Extension()>
    Public Function ToDataTable(Of T)(ByVal collection As IEnumerable(Of T)) As DataTable
        Dim dt As DataTable = New DataTable("DataTable")
        Dim type As Type = GetType(T)
        Dim pia() As PropertyInfo = type.GetProperties()

        ' For a collection of primitive types create a 1 column DataTable
        If type.IsPrimitive OrElse type.Equals(GetType(String)) Then
            dt.Columns.Add("Column", type)
        Else
            ' Inspect the properties and create the column in the DataTable
            For Each pi As PropertyInfo In pia
                Dim ColumnType As Type = pi.PropertyType
                If ColumnType.IsGenericType Then
                    ColumnType = ColumnType.GetGenericArguments()(0)
                End If
                dt.Columns.Add(pi.Name, ColumnType)
            Next

        End If

        ' Populate the data table
        For Each item As T In collection
            Dim dr As DataRow = dt.NewRow()
            dr.BeginEdit()
            ' Set item as the value for the lone column on each row
            If type.IsPrimitive OrElse type.Equals(GetType(String)) Then
                dr("Column") = item
            Else
                For Each pi As PropertyInfo In pia
                    If pi.GetValue(item, Nothing) IsNot Nothing Then
                        dr(pi.Name) = pi.GetValue(item, Nothing)
                    End If
                Next
            End If
            dr.EndEdit()
            dt.Rows.Add(dr)
        Next
        Return dt
    End Function

    <System.Runtime.CompilerServices.Extension>
    Public Function TwoDataTable(Of T)(ByVal data As IList(Of T)) As DataTable
        Dim properties As PropertyDescriptorCollection = TypeDescriptor.GetProperties(GetType(T))
        Dim table As New DataTable()
        For Each prop As PropertyDescriptor In properties
            table.Columns.Add(prop.Name, If(Nullable.GetUnderlyingType(prop.PropertyType), prop.PropertyType))
        Next prop
        For Each item As T In data
            Dim row As DataRow = table.NewRow()
            For Each prop As PropertyDescriptor In properties
                row(prop.Name) = If(prop.GetValue(item), DBNull.Value)
            Next prop
            table.Rows.Add(row)
        Next item
        Return table
    End Function

    'Generic function to convert Linq query to DataTable.
    Friend Function LinqToDataTable(Of T)(ByVal items As IEnumerable(Of T)) As DataTable
        'Createa DataTable with the Name of the Class i.e. Customer class.
        Dim dt As DataTable = New DataTable(GetType(T).Name)

        'Read all the properties of the Class i.e. Customer class.
        Dim propInfos As PropertyInfo() = GetType(T).GetProperties(BindingFlags.Public Or BindingFlags.Instance)

        'Loop through each property of the Class i.e. Customer class.
        For Each propInfo As PropertyInfo In propInfos
            'Add Columns in DataTable based on Property Name and Type.
            dt.Columns.Add(New DataColumn(propInfo.Name, propInfo.PropertyType))
        Next

        'Loop through the items of the Collection.
        For Each item As T In items
            'Add a new Row to DataTable.
            Dim dr As DataRow = dt.Rows.Add()

            'Loop through each property of the Class i.e. Customer class.
            For Each propInfo As PropertyInfo In propInfos
                'Add value Column to the DataRow.
                dr(propInfo.Name) = propInfo.GetValue(item, Nothing)
            Next
        Next

        Return dt
    End Function
End Module
