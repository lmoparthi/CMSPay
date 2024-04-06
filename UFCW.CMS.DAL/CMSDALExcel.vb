Imports System.Data.Common
Imports Microsoft.Office.Interop
Imports System.Data.OleDb
Imports System.Threading
Imports ClosedXML.Excel
Imports System.Runtime.InteropServices
Imports System.Diagnostics

Public Class CMSDALExcel

    Private Const ERROR_ACCESS_DENIED As Integer = 5
    Private Shared _OldCI As System.Globalization.CultureInfo
    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
    Public Shared ReadOnly Property ConnectionString(fullFileName As String) As String
        Get

            Dim ProviderName As String
            Dim ExtendedProperties As String

            ProviderName = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="
            ExtendedProperties = If(UFCWGeneral.GetSuffix(fullFileName) = "XLS", ";Extended Properties=" & """" & "Excel 8.0;HDR=Yes" & """", ";Extended Properties=" & """" & "Excel 12.0 Xml;HDR=Yes" & """")

            Return ProviderName & """" & fullFileName & """" & ExtendedProperties

        End Get
    End Property

    Private Shared ReadOnly Property InsertInto(dr As DataRow) As String
        Get

            Dim IntoDefinitions As String = ""

            For ColNo As Integer = 0 To dr.Table.Columns.Count - 1

                IntoDefinitions &= If(IntoDefinitions.Length > 0, ", [", "[") & dr.Table.Columns(ColNo).ColumnName & "]"

            Next

            Return "INSERT INTO [Sheet1] (" & IntoDefinitions & ")"

        End Get
    End Property

    Private Shared ReadOnly Property InsertValues(dr As DataRow) As String
        Get

            Dim ValueDefinitions As String = ""

            For ColNo As Integer = 0 To dr.Table.Columns.Count - 1
                If IsDBNull(dr(ColNo)) Then
                    ValueDefinitions &= If(ValueDefinitions.Length > 0, ", ", "") & " NULL"
                Else
                    Select Case dr.Table.Columns(ColNo).DataType.Name
                        Case "Int32", "Int16", "Long", "Decimal"
                            ValueDefinitions &= If(ValueDefinitions.Length > 0, ", ", "") & dr(ColNo).ToString
                        Case Else
                            ValueDefinitions &= If(ValueDefinitions.Length > 0, ", ", "") & "'" & dr(ColNo).ToString.Replace("'", "''") & "'"
                    End Select
                End If
            Next

            Return " VALUES (" & ValueDefinitions & ")"

        End Get
    End Property

    Private Shared ReadOnly Property WorkBookDefinition(dt As DataTable) As String
        Get

            Dim ColumnDefinitions As String = ""
            Dim DefaultColumnName As Integer = 1
            Dim ColumnName As String = ""

            For Each DR As DataRow In dt.Rows

                If DR("ColumnName").ToString.Length < 1 Then
                    ColumnName = "Column" & DefaultColumnName.ToString
                    DefaultColumnName += 1
                Else
                    ColumnName = DR("ColumnName").ToString
                End If

                Select Case True
                    Case DR("DataType").ToString.Contains("Int32"), DR("DataType").ToString.Contains("Int16"), DR("DataType").ToString.Contains("Long")
                        ColumnDefinitions &= If(ColumnDefinitions.Length > 0, ", [", "[") & ColumnName & "]  Long "
                    Case DR("DataType").ToString.Contains("Decimal")
                        ColumnDefinitions &= If(ColumnDefinitions.Length > 0, ", [", "[") & ColumnName & "]  Decimal "
                    Case DR("DataType").ToString.Contains("DateTime")
                        ColumnDefinitions &= If(ColumnDefinitions.Length > 0, ", [", "[") & ColumnName & "]  Date "
                    Case Else
                        If CInt(DR("ColumnSize")) > 254 Then
                            ColumnDefinitions &= If(ColumnDefinitions.Length > 0, ", [", "[") & ColumnName & "]  MEMO "
                        Else
                            ColumnDefinitions &= If(ColumnDefinitions.Length > 0, ", [", "[") & ColumnName & "]  CHAR(" & DR("ColumnSize").ToString & ") "
                        End If
                End Select
            Next

            Return "CREATE TABLE [Sheet1] (" & ColumnDefinitions & ")"

        End Get
    End Property
    Public Shared Sub ApplyExcelFormatting(ByVal jobName As String, ByVal drvStep As DataRowView, ByVal dS As DataSet, ByVal fileName As String, JoblogFileNameAndDestination As String)

        Dim FileInfo As System.IO.FileInfo

        Dim XLApp As Excel.Application
        Dim XLBook As Excel.Workbook
        Dim XLBooks As Excel.Workbooks
        Dim XLSheet As Excel.Worksheet
        Dim XLRange As Excel.Range
        Dim XLRow As DataRow
        Dim ExcelSchemaDS As New DataSet
        Dim DT As DataTable
        Dim DBDataReader As DbDataReader

        Try

            Using XLConnection As New System.Data.OleDb.OleDbConnection(ConnectionString(fileName)) 'establish OLEDB datatypes

                XLConnection.Open()
                ExcelSchemaDS.EnforceConstraints = False
                ExcelSchemaDS.Tables.Add(XLConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing))
                ExcelSchemaDS.Tables("Tables").TableName = "WorkSheetTables"

                For X As Integer = 0 To ExcelSchemaDS.Tables.Count - 1

                    For Each DR As DataRow In ExcelSchemaDS.Tables(X).Rows
                        ExcelSchemaDS.Tables.Add(XLConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, New Object() {Nothing, Nothing, DR("TABLE_NAME").ToString, Nothing}))
                        ExcelSchemaDS.Tables("Columns").TableName = DR("TABLE_NAME").ToString
                    Next

                Next

                Using XLCommand As New System.Data.OleDb.OleDbCommand() ' retreive 1 line to examine data (possibly flawed logic if 1st line is incomplete)

                    XLCommand.Connection = XLConnection

                    XLCommand.CommandText = "SELECT TOP 100 * FROM [Sheet1]"
                    DBDataReader = XLCommand.ExecuteReader()

                    DT = New DataTable

                    DT.Load(DBDataReader)

                    If DT IsNot Nothing Then
                        XLRow = DT.Rows(0)
                    End If

                End Using

                If XLConnection IsNot Nothing Then XLConnection.Close()
            End Using

            XLApp = New Excel.Application With {
                .Visible = False,
                .UserControl = False,
                .DisplayAlerts = False,
                .ScreenUpdating = False,
                .EnableEvents = False,
                .Interactive = False
            }

            XLBooks = XLApp.Workbooks
            XLBook = XLBooks.Open(fileName)
            XLSheet = CType(XLApp.Sheets("Sheet1"), Excel.Worksheet)
            XLApp.Calculation = Excel.XlCalculation.xlCalculationManual

            For Each DR As DataRow In ExcelSchemaDS.Tables("Sheet1").Rows

                If IsDBNull(XLRow(CInt(DR("ORDINAL_POSITION")) - 1)) Then
                    Continue For
                End If

                Select Case DR("DATA_TYPE").ToString
                    Case "5" ' Double
                        XLRange = CType(XLSheet.Columns(DR("ORDINAL_POSITION")), Global.Microsoft.Office.Interop.Excel.Range).EntireColumn
                        XLRange.NumberFormat = If(Not IsDBNull(XLRow(CInt(DR("ORDINAL_POSITION")) - 1)) AndAlso XLRow(CInt(DR("ORDINAL_POSITION")) - 1).ToString.Contains("."), "0.00", "0")
                    Case "6" ' Currency
                        XLRange = CType(XLSheet.Columns(DR("ORDINAL_POSITION")), Global.Microsoft.Office.Interop.Excel.Range).EntireColumn
                        XLRange.NumberFormat = "0.00"
                    Case "7" ' date
                        XLRange = CType(XLSheet.Columns(DR("ORDINAL_POSITION")), Global.Microsoft.Office.Interop.Excel.Range).EntireColumn
                        XLRange.NumberFormat = If(Not IsDBNull(XLRow(CInt(DR("ORDINAL_POSITION")) - 1)) AndAlso CType(XLRow(CInt(DR("ORDINAL_POSITION")) - 1), DateTime).ToString.Contains("12:00:00"), "mm/dd/yyyy", "mm/dd/yyyy hh:mm:ss AM/PM")
                    Case "11" ' boolean
                    Case "130"
                        If DR("CHARACTER_MAXIMUM_LENGTH").ToString = "0" Then 'Memo
                            XLRange = CType(XLSheet.Columns(DR("ORDINAL_POSITION")), Global.Microsoft.Office.Interop.Excel.Range).EntireColumn
                            XLRange.AutoFit() 'this helps with line breaks
                            XLRange.WrapText = True
                        Else ' String
                        End If
                End Select

            Next

            XLSheet.Columns.AutoFit()
            XLBook.Save()

            XLBook.Close()
            XLBooks.Close()
            XLApp.Quit()

        Catch ex As Exception


            Throw

        Finally

            If XLSheet IsNot Nothing Then Marshal.FinalReleaseComObject(XLSheet)
            If XLBook IsNot Nothing Then Marshal.FinalReleaseComObject(XLBook)
            If XLBooks IsNot Nothing Then Marshal.FinalReleaseComObject(XLBooks)
            If XLApp IsNot Nothing Then Marshal.FinalReleaseComObject(XLApp)
            XLApp = Nothing

            If ExcelSchemaDS IsNot Nothing Then ExcelSchemaDS.Dispose()
            ExcelSchemaDS = Nothing

            If DBDataReader IsNot Nothing Then DBDataReader.Close()

            If DT IsNot Nothing Then DT.Dispose()

            FileInfo = Nothing

        End Try
    End Sub
    Public Shared Sub ApplyExcelFormatting(ByVal fileName As String)

        Dim FileInfo As System.IO.FileInfo

        Dim XLApp As Excel.Application
        Dim XLBook As Excel.Workbook
        Dim XLBooks As Excel.Workbooks
        Dim XLSheet As Excel.Worksheet
        Dim XLRange As Excel.Range
        Dim XLRow As DataRow
        Dim ExcelSchemaDS As New DataSet
        Dim XLDT As DataTable
        Dim DBDataReader As DbDataReader

        Try

            Using XLConnection As New System.Data.OleDb.OleDbConnection(ConnectionString(fileName)) 'establish OLEDB datatypes

                XLConnection.Open()
                ExcelSchemaDS.EnforceConstraints = False
                ExcelSchemaDS.Tables.Add(XLConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing))
                ExcelSchemaDS.Tables("Tables").TableName = "WorkSheetTables"

                For X As Integer = 0 To ExcelSchemaDS.Tables.Count - 1

                    For Each DR As DataRow In ExcelSchemaDS.Tables(X).Rows
                        ExcelSchemaDS.Tables.Add(XLConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, New Object() {Nothing, Nothing, DR("TABLE_NAME").ToString, Nothing}))
                        ExcelSchemaDS.Tables("Columns").TableName = DR("TABLE_NAME").ToString
                    Next

                Next

                Using XLCommand As New System.Data.OleDb.OleDbCommand() ' retreive 1 line to examine data (possibly flawed logic if 1st line is incomplete)

                    XLCommand.Connection = XLConnection

                    XLCommand.CommandText = "SELECT TOP 100 * FROM [Sheet1]"
                    DBDataReader = XLCommand.ExecuteReader()

                    XLDT = New DataTable

                    XLDT.Load(DBDataReader)

                    If XLDT IsNot Nothing Then
                        XLRow = XLDT.Rows(0)
                    End If

                End Using

                If XLConnection IsNot Nothing Then XLConnection.Close()
            End Using

            Dim XLapplication As New Excel.Application
            XLApp = XLapplication

            XLApp.Visible = False
            XLApp.UserControl = False
            XLApp.DisplayAlerts = False
            XLApp.ScreenUpdating = False
            XLApp.EnableEvents = False
            XLApp.Interactive = False

            XLBooks = XLApp.Workbooks
            XLBook = XLBooks.Open(fileName)
            XLSheet = CType(XLApp.Sheets("Sheet1"), Excel.Worksheet)
            XLApp.Calculation = Excel.XlCalculation.xlCalculationManual

            For Each DR As DataRow In ExcelSchemaDS.Tables("Sheet1").Rows

                If IsDBNull(XLRow(CInt(DR("ORDINAL_POSITION")) - 1)) Then
                    Continue For
                End If

                Select Case DR("DATA_TYPE").ToString
                    Case "5" ' Double
                        XLRange = CType(XLSheet.Columns(DR("ORDINAL_POSITION")), Global.Microsoft.Office.Interop.Excel.Range).EntireColumn
                        XLRange.NumberFormat = If(Not IsDBNull(XLRow(CInt(DR("ORDINAL_POSITION")) - 1)) AndAlso XLRow(CInt(DR("ORDINAL_POSITION")) - 1).ToString.Contains("."), "0.00", "0")
                    Case "6" ' Currency
                        XLRange = CType(XLSheet.Columns(DR("ORDINAL_POSITION")), Global.Microsoft.Office.Interop.Excel.Range).EntireColumn
                        XLRange.NumberFormat = "0.00"
                    Case "7" ' date
                        XLRange = CType(XLSheet.Columns(DR("ORDINAL_POSITION")), Global.Microsoft.Office.Interop.Excel.Range).EntireColumn
                        XLRange.NumberFormat = If(Not IsDBNull(XLRow(CInt(DR("ORDINAL_POSITION")) - 1)) AndAlso CType(XLRow(CInt(DR("ORDINAL_POSITION")) - 1), DateTime).ToString.Contains("12:00:00"), "mm/dd/yyyy", "mm/dd/yyyy hh:mm:ss AM/PM")
                    Case "11" ' boolean
                    Case "130"
                        If DR("CHARACTER_MAXIMUM_LENGTH").ToString = "0" Then 'Memo
                            XLRange = CType(XLSheet.Columns(DR("ORDINAL_POSITION")), Global.Microsoft.Office.Interop.Excel.Range).EntireColumn
                            XLRange.AutoFit() 'this helps with line breaks
                            XLRange.WrapText = True
                        Else ' String
                        End If
                End Select

            Next

            XLSheet.Columns.AutoFit()
            XLBook.Save()

            XLBook.Close()
            XLBooks.Close()
            XLApp.Quit()

        Catch ex As Exception


            Throw

        Finally

            If XLSheet IsNot Nothing Then Marshal.FinalReleaseComObject(XLSheet)
            If XLBook IsNot Nothing Then Marshal.FinalReleaseComObject(XLBook)
            If XLBooks IsNot Nothing Then Marshal.FinalReleaseComObject(XLBooks)
            If XLApp IsNot Nothing Then Marshal.FinalReleaseComObject(XLApp)
            XLApp = Nothing

            If ExcelSchemaDS IsNot Nothing Then ExcelSchemaDS.Dispose()
            ExcelSchemaDS = Nothing

            If DBDataReader IsNot Nothing Then DBDataReader.Close()

            If XLDT IsNot Nothing Then XLDT.Dispose()
            XLDT = Nothing

            FileInfo = Nothing

        End Try
    End Sub

    <System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions>
    Public Shared Sub ModifyExcelFileType(ByVal inFileName As String, ByVal outFileName As String, ByVal outFileFormat As Excel.XlFileFormat)

        Dim XLApp As Excel.Application
        Dim XLBook As Excel.Workbook
        Dim XLBooks As Excel.Workbooks
        Dim XLSheet As Excel.Worksheet

        Try

            XLApp = New Excel.Application

            With XLApp
                .Visible = False
                .UserControl = False
                .DisplayAlerts = False
                .ScreenUpdating = False
                .EnableEvents = False
                .Interactive = False
            End With

            XLBooks = XLApp.Workbooks
            XLBook = XLBooks.Open(inFileName)
            XLSheet = CType(XLApp.Sheets(0), Excel.Worksheet)
            XLApp.Calculation = Excel.XlCalculation.xlCalculationManual

            XLBook.SaveAs(Filename:=outFileName, FileFormat:=outFileFormat)

            XLBook.Close()
            XLBooks.Close()
            XLApp.Quit()

        Catch ex As Exception


            Throw

        Finally

            If XLSheet IsNot Nothing Then Marshal.FinalReleaseComObject(XLSheet)
            If XLBook IsNot Nothing Then Marshal.FinalReleaseComObject(XLBook)
            If XLBooks IsNot Nothing Then Marshal.FinalReleaseComObject(XLBooks)
            If XLApp IsNot Nothing Then Marshal.FinalReleaseComObject(XLApp)
            XLApp = Nothing

        End Try
    End Sub

    Public Shared Sub CreateDataInExcelCellByCell(ByVal jobName As String, ByVal drvStep As DataRowView, ByVal dS As DataSet, ByVal fullFileName As String, ByRef rowCount As Long, JoblogFileNameAndDestination As String, Optional ByVal excludeHeader As Boolean = False)

        Dim XLApp As Excel.Application

        Dim XLbook As Excel.Workbook
        Dim XLSheet As Excel.Worksheet
        Dim XLBooks As Excel.Workbooks
        Dim XLRange As Excel.Range

        'Log.TranLog_Write("Job -> " & jobName & " step -> " & drvStep("Step").ToString & " Entered Excel: Thread (" & System.Threading.Environment.CurrentManagedThreadId & ") -> " & " " & fullFileName, JoblogFileNameAndDestination)

        'SyncLock _XLSyncLock

        Try
            'Log.TranLog_Write("Job -> " & jobName & " step -> " & drvStep("Step").ToString & " Creating Excel Instance: Thread (" & System.Threading.Environment.CurrentManagedThreadId & ") -> " & " " & fullFileName, JoblogFileNameAndDestination)
            'Log.TranLog_Write("Job -> " & jobName & " step -> " & drvStep("Step").ToString & " Prepare Excel: Thread (" & System.Threading.Environment.CurrentManagedThreadId & ") -> " & " " & fullFileName, JoblogFileNameAndDestination)
            XLApp = New Excel.Application With {
                .Visible = False,
                .UserControl = False,
                .DisplayAlerts = False,
                .ScreenUpdating = False,
                .EnableEvents = False,
                .Interactive = False
            }

            SetUSCurrentCulture()

            XLBooks = XLApp.Workbooks
            XLbook = XLBooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet)
            XLSheet = CType(XLbook.Worksheets(1), Global.Microsoft.Office.Interop.Excel.Worksheet)
            XLRange = CType(XLSheet.Rows(1), Global.Microsoft.Office.Interop.Excel.Range)

            XLApp.Calculation = Excel.XlCalculation.xlCalculationManual

            Dim BaseRow As Integer = 2

            If dS.Tables("Data").Rows.Count > 0 Then

                'Log.TranLog_Write("Job -> " & jobName & " step -> " & drvStep("Step").ToString & " Create Excel file: Thread (" & System.Threading.Environment.CurrentManagedThreadId & ") -> " & " " & fullFileName, JoblogFileNameAndDestination)

                XLbook.SaveAs(fullFileName, If(UFCWGeneral.GetSuffix(fullFileName) = "XLS", Excel.XlFileFormat.xlExcel8, Excel.XlFileFormat.xlOpenXMLWorkbook), Nothing, Nothing, False, False, Excel.XlSaveAsAccessMode.xlNoChange, Excel.XlSaveConflictResolution.xlLocalSessionChanges, False) 'save to avoid temp file issues ?

                If Not excludeHeader Then ''writing the column names

                    'Log.TranLog_Write("Job -> " & jobName & " step -> " & drvStep("Step").ToString & " Insert Header: Thread (" & System.Threading.Environment.CurrentManagedThreadId & ") -> " & " " & fullFileName, JoblogFileNameAndDestination)

                    For J As Integer = 0 To dS.Tables("Data").Columns.Count - 1
                        CType(XLSheet.Cells(1, J + 1), Microsoft.Office.Interop.Excel.Range).NumberFormat = "@" 'text
                        CType(XLSheet.Cells(1, J + 1), Microsoft.Office.Interop.Excel.Range).Value2 = dS.Tables("Data").Columns(J).ColumnName
                    Next
                End If

                '' For each row, print the values of each column.

                Dim RowNo As Integer = 0
                Dim ReportingInterval As Integer = 0

                'Log.TranLog_Write("Job -> " & jobName & " step -> " & drvStep("Step").ToString & " Insert " & dt.Rows.Count.ToString & " Rows: Thread (" & System.Threading.Environment.CurrentManagedThreadId & ") -> " & " " & fullFileName, JoblogFileNameAndDestination)

                If dS.Tables("Data").Rows.Count > 100 Then
                    ReportingInterval = CInt(dS.Tables("Data").Rows.Count / 10)
                End If

                For Each DR As DataRow In dS.Tables("Data").Rows

                    For ColNo As Integer = 0 To dS.Tables("Data").Columns.Count - 1
                        Select Case dS.Tables("Data").Columns(ColNo).DataType.Name
                            Case "Int32", "Int16", "Long"
                                CType(XLSheet.Cells(BaseRow + RowNo, ColNo + 1), Microsoft.Office.Interop.Excel.Range).NumberFormat = "0"
                            Case "Decimal"
                                CType(XLSheet.Cells(BaseRow + RowNo, ColNo + 1), Microsoft.Office.Interop.Excel.Range).NumberFormat = "0.00"
                            Case "DateTime"
                                CType(XLSheet.Cells(BaseRow + RowNo, ColNo + 1), Microsoft.Office.Interop.Excel.Range).NumberFormat = If(dS.Tables("Data").Rows(RowNo)(ColNo).ToString.Contains("12:00:00"), "mm/dd/yyyy", "mm/dd/yyyy hh:mm:ss AM/PM")
                            Case Else
                                CType(XLSheet.Cells(BaseRow + RowNo, ColNo + 1), Microsoft.Office.Interop.Excel.Range).NumberFormat = "@" 'text
                        End Select
                        CType(XLSheet.Cells(BaseRow + RowNo, ColNo + 1), Microsoft.Office.Interop.Excel.Range).Value2 = dS.Tables("Data").Rows(RowNo)(ColNo).ToString
                    Next

                    RowNo += 1
                    If ReportingInterval <> 0 AndAlso RowNo Mod ReportingInterval = 0 Then
                        'Log.TranLog_Write("Job -> " & jobName & " step -> " & drvStep("Step").ToString & " Inserted " & RowNo.ToString & " Rows: Thread (" & System.Threading.Environment.CurrentManagedThreadId & ") -> " & " " & fullFileName, JoblogFileNameAndDestination)
                        XLbook.SaveAs(fullFileName, If(UFCWGeneral.GetSuffix(fullFileName) = "XLS", Excel.XlFileFormat.xlExcel8, Excel.XlFileFormat.xlOpenXMLWorkbook), Nothing, Nothing, False, False, Excel.XlSaveAsAccessMode.xlNoChange, Excel.XlSaveConflictResolution.xlLocalSessionChanges, False)
                    End If
                Next

                If dS.Tables("Data").Rows.Count < 101 Then
                    XLSheet.Columns.AutoFit() 'Too expensive for large files
                End If

            End If

            XLbook.SaveAs(fullFileName, If(UFCWGeneral.GetSuffix(fullFileName) = "XLS", Excel.XlFileFormat.xlExcel8, Excel.XlFileFormat.xlOpenXMLWorkbook), Nothing, Nothing, False, False, Excel.XlSaveAsAccessMode.xlNoChange, Excel.XlSaveConflictResolution.xlLocalSessionChanges, False)

            '' Need all following code to clean up and remove all references!!!

            'Log.TranLog_Write("Job -> " & jobName & " step -> " & drvStep("Step").ToString & " Close Excel Workbook: Thread (" & System.Threading.Environment.CurrentManagedThreadId & ") -> " & " " & fullFileName, JoblogFileNameAndDestination)

            XLbook.Close()
            XLBooks.Close()
            XLApp.Quit()

            rowCount += dS.Tables("Data").Rows.Count

        Catch ex As ApplicationException
            Throw
        Catch ex As Exception


            Throw

        Finally

            If XLRange IsNot Nothing Then ReleaseObject(XLRange)
            If XLSheet IsNot Nothing Then ReleaseObject(XLSheet)
            If XLbook IsNot Nothing Then Marshal.FinalReleaseComObject(XLbook)
            If XLBooks IsNot Nothing Then Marshal.FinalReleaseComObject(XLBooks)

            If XLApp IsNot Nothing Then Marshal.FinalReleaseComObject(XLApp)
            XLApp = Nothing

            ResetCurrentCulture()

            'Log.TranLog_Write("Job -> " & jobName & " step -> " & drvStep("Step").ToString & " Quit Excel: Thread (" & System.Threading.Environment.CurrentManagedThreadId & ") -> " & " " & fullFileName, JoblogFileNameAndDestination)

        End Try

        'End SyncLock

    End Sub

    Public Shared Function KillUnusedExcelProcess() As Integer
        Dim XlProcesses As Process()

        Try

            XlProcesses = Process.GetProcessesByName("EXCEL")
            For Each XLProcess As Process In XlProcesses
                If Len(XLProcess.MainWindowTitle) = 0 Then
                    Try

                        XLProcess.Kill()

                        KillUnusedExcelProcess += 1

                        Thread.Sleep(5000)

                    Catch e As System.ComponentModel.Win32Exception When e.NativeErrorCode = ERROR_ACCESS_DENIED
                        'do nothing
                    Catch ex As Exception
                        Throw
                    End Try
                End If
            Next

        Catch ex As Exception

            Throw
        End Try
    End Function
    Public Shared Function RowCountUsingExcel12(ByVal filename As String, Optional excludeHeader As Boolean = True) As Integer?

        Try
            Using Workbook As New XLWorkbook(filename)
                Return Workbook.Worksheets(0).Rows.Count + CInt(excludeHeader)
            End Using

        Catch ex As Exception


            Throw

        End Try

    End Function

    Public Shared Function RowCountUsingExcel8(ByVal filename As String, Optional excludeHeader As Boolean = True) As Integer?

        Dim RowsCount As Integer?
        Dim WorksheetDT As New DataTable
        Dim SchemaDT As New DataTable

        Try

            Using ExcelConnCSV As New OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0; data source='" & filename & " '; " & "Extended Properties=Excel 12.0;")
                Try
                    ExcelConnCSV.Open()

                    SchemaDT = ExcelConnCSV.GetOleDbSchemaTable(OleDb.OleDbSchemaGuid.Tables, Nothing)
                    For Each DR As DataRow In SchemaDT.Rows

                        If Not DR("TABLE_NAME").ToString().EndsWith("$") AndAlso Not DR("TABLE_NAME").ToString().EndsWith("$'") Then
                            Continue For
                        End If

                        Using ExcelCmdSelect As New OleDbCommand("SELECT * FROM [" & DR.Field(Of String)("TABLE_NAME").ToString & "]", ExcelConnCSV)
                            Using ExcelDACSV As New OleDbDataAdapter()
                                ExcelDACSV.SelectCommand = ExcelCmdSelect
                                ExcelDACSV.Fill(WorksheetDT)
                                If RowsCount Is Nothing Then
                                    RowsCount = WorksheetDT.Rows.Count
                                Else
                                    RowsCount += CType(WorksheetDT.Rows.Count, Integer?)
                                End If
                            End Using
                        End Using

                    Next DR

                Catch ex As Exception When ex.HResult = -2147467259 Or ex.Message = "External table is not in the expected format."
                    Return Nothing
                Finally
                    ExcelConnCSV.Close()
                End Try
            End Using

            Return RowsCount + CInt(excludeHeader) 'because this is a nullable integer the excludeheader math does not take affect if RowsCount is still null

        Catch ex As Exception


            Throw

        Finally

            If WorksheetDT IsNot Nothing Then WorksheetDT.Dispose()
            WorksheetDT = Nothing

            If SchemaDT IsNot Nothing Then SchemaDT.Dispose()
            SchemaDT = Nothing

        End Try
    End Function
    Public Shared Sub WriteToExcel(ByVal ds As DataSet, ByVal fullFileName As String)

        Dim InsertPrefix As String = ""

        Try
            Using XLConnection As New System.Data.OleDb.OleDbConnection(ConnectionString(fullFileName))

                XLConnection.Open()

                Using XLCommand As New System.Data.OleDb.OleDbCommand()

                    XLCommand.Connection = XLConnection

                    XLCommand.CommandText = WorkBookDefinition(ds.Tables("Schema1"))
                    XLCommand.ExecuteNonQuery()

                    If ds.Tables("Data").Rows.Count > 0 Then

                        For Each DR As DataRow In ds.Tables("Data").Rows

                            If InsertPrefix.Length = 0 Then
                                InsertPrefix = InsertInto(DR)
                            End If

                            XLCommand.CommandText = InsertPrefix & InsertValues(DR)
                            XLCommand.ExecuteNonQuery()

                        Next

                    End If

                    'Close the connection.
                    XLCommand.Connection.Close()

                End Using

                If XLConnection IsNot Nothing Then XLConnection.Close()
            End Using

        Catch ex As ApplicationException
            Throw
        Catch ex As Exception


            Throw

        Finally

        End Try

    End Sub

    '<System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions>
    Public Shared Sub WriteToExcel(ByVal jobName As String, ByVal drvStep As DataRowView, ByVal ds As DataSet, ByVal fullFileName As String, ByRef rowCount As Long, JoblogFileNameAndDestination As String, Optional ByVal excludeHeader As Boolean = False)

        Dim InsertPrefix As String = ""
        Dim WorkBookFileName As String
        Dim TotalRowsRemaining As Integer = 0
        Dim StartingRow As Integer = 0
        Dim EndingRow As Integer = 0
        Dim PartNumber As Integer = 0
        Const MaxRowsInASheet As Integer = 1000000 '1048575

        Try

            rowCount += ds.Tables("Data").Rows.Count

            TotalRowsRemaining = ds.Tables("Data").Rows.Count

            Dim FileNameSuffix As String = UFCWGeneral.GetSuffix(fullFileName)
            Dim FileNamePrefix As String = fullFileName.Replace("." & FileNameSuffix, "")

            Do While True

                PartNumber += 1

                If TotalRowsRemaining > MaxRowsInASheet Then
                    EndingRow += MaxRowsInASheet
                    TotalRowsRemaining -= MaxRowsInASheet
                Else
                    EndingRow += TotalRowsRemaining
                    TotalRowsRemaining = 0
                End If

                If rowCount > MaxRowsInASheet Then
                    WorkBookFileName = FileNamePrefix & " (Part " & PartNumber.ToString & ")." & FileNameSuffix
                Else
                    WorkBookFileName = fullFileName
                End If

                Using Workbook As New XLWorkbook()
                    Workbook.Worksheets.Add(ds.Tables("Data").AsEnumerable().Skip(StartingRow).Take(EndingRow).CopyToDataTable, "Sheet1")
                    Workbook.Worksheet("Sheet1").Table(0).ShowAutoFilter = False
                    Workbook.Worksheet("Sheet1").Table(0).SetShowHeaderRow(Not excludeHeader)
                    Workbook.SaveAs(WorkBookFileName)
                End Using

                If TotalRowsRemaining = 0 Then Exit Do

                StartingRow = EndingRow

            Loop

        Catch ex As ApplicationException
            Throw
        Catch ex As Exception

            Throw

        Finally

        End Try

    End Sub
    Private Shared Sub ReleaseObject(ByVal obj As Object)
        Try
            If obj IsNot Nothing Then System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)

        Catch IgnoreException As Exception
        Finally
            obj = Nothing
        End Try
    End Sub

    'reset Current Culture back to the originale
    Private Shared Sub ResetCurrentCulture()
        System.Threading.Thread.CurrentThread.CurrentCulture = _OldCI
    End Sub

    'get the old CurrenCulture and set the new, en-US
    Private Shared Sub SetUSCurrentCulture()
        _OldCI = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
    End Sub
End Class