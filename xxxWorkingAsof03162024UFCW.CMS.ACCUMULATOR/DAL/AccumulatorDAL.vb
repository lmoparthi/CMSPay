Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.IO
Imports System.Xml.Serialization
Imports System.Data.Common
Imports System.Threading


''' -----------------------------------------------------------------------------
''' Project	 : Accumulator
''' Class	 : CMS.AccumulatorDAL
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' The Data Access Layer for the Accumulators
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	12/15/2005	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Class AccumulatorDAL
    Shared _DB2databaseName As String = "FDBMD"
    Shared _SQLdatabaseName As String = "dbo"

    Private Shared _AccumulatorsDS As DataSet
    Private Shared _TraceParallel As New TraceSwitch("TraceParallel", "Parallel Trace Switch in App.Config", "0")

#Region "CRUD"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets all Accumulators from the database
    ' </summary>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	12/15/2005	Created
    '     [paulw]	1/13/2006	Chnaged logic to allow for better binding of a datagrid
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function GetAccumulators() As DataTable
        Dim DS As DataSet
        Dim DT As DataTable
        Dim DR As DataRow
        Dim NewDR As DataRow

        Try

#If TRACE Then
            If CInt(_TraceParallel.Level) > 1 Then _
                Trace.WriteLine(
                    UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " &
                    Thread.CurrentThread.ManagedThreadId.ToString & vbTab &
                    New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString &
                    vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " &
                    New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" &
                    New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " &
                    New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" &
                    New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")" & " < " &
                    New System.Diagnostics.StackTrace(True).GetFrame(2).GetMethod.ToString & "  (" &
                    New System.Diagnostics.StackTrace(True).GetFrame(2).GetFileLineNumber.ToString & ")",
                    "TraceParallel" & vbTab)
#End If

            DS = GetActiveAccumulators()
            DT = BuildAccumulatorColumns()

            'I Have to do the following in order to have the datatable bind correctly.  ExecuteDataSet does not set up the columns properly
            For I As Integer = 0 To DS.Tables(0).Rows.Count - 1

                DR = DS.Tables(0).Rows(I)
                NewDR = DT.NewRow

                For J As Integer = 0 To DS.Tables(0).Columns.Count - 1
                    NewDR(DS.Tables(0).Columns(J).ColumnName) = DR(DS.Tables(0).Columns(J).ColumnName)
                    If (DS.Tables(0).Columns(J).ColumnName = "VALUE_TYPE") Then
                        NewDR("VALUE_TYPE_NAME") =
                            If(CInt(DR(DS.Tables(0).Columns(J).ColumnName)) = 0, "UNITS", "CURRENCY")
                    ElseIf DS.Tables(0).Columns(J).ColumnName = "ACCIDENT_SW" Then
                        NewDR("ACCIDENT_SW") =
                            CBool(
                                If _
                                    (DR(DS.Tables(0).Columns(J).ColumnName) Is System.DBNull.Value, False,
                                     DR(DS.Tables(0).Columns(J).ColumnName)))
                    End If
                Next
                DT.Rows.Add(NewDR)
            Next

            DT.DefaultView.Sort = "DISPLAY_ORDER ASC"

            Return DT

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceParallel.Level) > 0 Then _
                Trace.WriteLine(
                    UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " &
                    Thread.CurrentThread.ManagedThreadId.ToString & vbTab &
                    New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString &
                    vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " &
                    New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" &
                    New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " &
                    New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" &
                    New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")" & " < " &
                    New System.Diagnostics.StackTrace(True).GetFrame(2).GetMethod.ToString & "  (" &
                    New System.Diagnostics.StackTrace(True).GetFrame(2).GetFileLineNumber.ToString & ")",
                    "TraceParallel" & vbTab)
#End If
        End Try
    End Function

    Public Shared Function GetActiveAccumulators(Optional ByRef transaction As DbTransaction = Nothing) As DataSet

        Dim XMLFilename As String
        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String
        Dim FStream As FileStream
        Dim XMLSerial As XmlSerializer

        Try
#If TRACE Then
            If CInt(_TraceParallel.Level) > 1 Then _
                Trace.WriteLine(
                    UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " &
                    Thread.CurrentThread.ManagedThreadId.ToString & vbTab &
                    New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString &
                    vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " &
                    New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" &
                    New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " &
                    New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" &
                    New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")" & " < " &
                    New System.Diagnostics.StackTrace(True).GetFrame(2).GetMethod.ToString & "  (" &
                    New System.Diagnostics.StackTrace(True).GetFrame(2).GetFileLineNumber.ToString & ")",
                    "TraceParallel" & vbTab)
#End If

            If _AccumulatorsDS Is Nothing Then
                XMLFilename = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." &
                              "FDBMD.RETRIEVE_ACCUMULATORS" & ".xml"

                DS = CMSXMLHandler.ToAndFromDataset(UFCWGeneral.GetUniqueKey(), "FDBMD.ACCUMULATOR", "LASTUPDT",
                                                 "FDBMD.RETRIEVE_ACCUMULATORS")
                If DS.Tables.Count = 0 Then
                    DB = CMSDALCommon.CreateDatabase()
                    SQLCall = "FDBMD.RETRIEVE_ACCUMULATORS"
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                    If DS Is Nothing Then
                        If transaction Is Nothing Then
                            DS = DB.ExecuteDataSet(DBCommandWrapper)
                        Else
                            DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                        End If
                    Else
                        If transaction Is Nothing Then
                            DB.LoadDataSet(DBCommandWrapper, DS, "ACCUMULATORS")
                        Else
                            DB.LoadDataSet(DBCommandWrapper, DS, "ACCUMULATORS", transaction)
                        End If
                    End If

                    _AccumulatorsDS = DS
                    FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                                             FileShare.ReadWrite)
                    '' Dim sWriter As New System.IO.StreamWriter(xmlFilename)
                    XMLSerial = New XmlSerializer(DS.GetType())
                    XMLSerial.Serialize(FStream, DS)
                    FStream.Close()
                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _AccumulatorsDS = DS
                End If
            Else
                DS = _AccumulatorsDS
            End If

            Return DS

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceParallel.Level) > 0 Then _
                Trace.WriteLine(
                    UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " &
                    Thread.CurrentThread.ManagedThreadId.ToString & vbTab &
                    New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString &
                    vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " &
                    New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" &
                    New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " &
                    New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" &
                    New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")" & " < " &
                    New System.Diagnostics.StackTrace(True).GetFrame(2).GetMethod.ToString & "  (" &
                    New System.Diagnostics.StackTrace(True).GetFrame(2).GetFileLineNumber.ToString & ")",
                    "TraceParallel" & vbTab)
#End If
        End Try
    End Function

    Public Shared Function GetNextAccidentAccumulator(ByVal accumulatorName As String) As Integer _
        ', ByVal valueType As MemberAccumulator.MemberAccumulatorValueTypes, ByVal isActive As Boolean, ByVal isFamily As Boolean, ByVal description As String, ByVal displayOrder As Integer, ByVal userId As String)
        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand
        Dim DB As Database
        Dim Connection As DbConnection

        Try
            DB = CMSDALCommon.CreateDatabase()
            Connection = DB.CreateConnection()

            Connection.Open()
            SQLCommand = "FDBMD.RETRIEVE_NEXT_ACCIDENT_ACCUMULATOR"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)
            DB.AddInParameter(DBCommandWrapper, "@ACCUM_NAME", DbType.String, accumulatorName)
            DB.AddOutParameter(DBCommandWrapper, "@ACCUMID", DbType.Int32, 4)
            DB.ExecuteNonQuery(DBCommandWrapper)

            Return CInt(DB.GetParameterValue(DBCommandWrapper, "@ACCUMID"))

        Catch ex As Exception
            Throw

        Finally
            If Connection IsNot Nothing Then Connection.Close()
            Connection = Nothing

        End Try
    End Function
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Saves all accumulators to the database, and handles deleted accumulators
    ' </summary>
    ' <param name="accumulators"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	12/16/2005	Created
    '     [paulw] 1/18/2006   Removed CREATE_DATE and LASTUPDT per Ron Lewis.
    '                         He is handeling this in the database now
    '     [paulw] 10/9/2006   Added Display Order Per ACR MED-0048
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Sub CommitAccumulators(ByVal accumulatorsDT As DataTable, ByVal deleteList As ArrayList)
        If accumulatorsDT Is Nothing Then Return

        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand
        Dim DB As Database
        Dim Connection As DbConnection
        Dim Transaction As DbTransaction

        Try
            DB = CMSDALCommon.CreateDatabase()
            Connection = DB.CreateConnection()

            Connection.Open()
            Transaction = Connection.BeginTransaction()

            For I As Integer = 0 To accumulatorsDT.Rows.Count - 1
                'if the id is null, then it has not been added to the database yet
                If accumulatorsDT.Rows(I)("ACCUM_ID") Is System.DBNull.Value Then
                    SQLCommand = "FDBMD.CREATE_ACCUMULATOR"
                ElseIf CInt(accumulatorsDT.Rows(I)("ACCUM_ID")) = -1 Then
                    SQLCommand = "FDBMD.CREATE_ACCUMULATOR"
                Else
                    SQLCommand = "FDBMD.UPDATE_ACCUMULATOR"
                End If
                DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)

                If accumulatorsDT.Rows(I)("ACCUM_ID") IsNot System.DBNull.Value Then
                    If Not CInt(accumulatorsDT.Rows(I)("ACCUM_ID")) = -1 Then
                        DB.AddInParameter(DBCommandWrapper, "@ACCUM_ID", DbType.Int32,
                                          CInt(accumulatorsDT.Rows(I)("ACCUM_ID")))
                    End If
                End If

                DB.AddInParameter(DBCommandWrapper, "@ACCUM_NAME", DbType.String,
                                  CStr(accumulatorsDT.Rows(I)("ACCUM_NAME")))
                DB.AddInParameter(DBCommandWrapper, "@VALUE_TYPE", DbType.Int32,
                                  CInt(accumulatorsDT.Rows(I)("VALUE_TYPE")))
                DB.AddInParameter(DBCommandWrapper, "@ACTIVE_SW", DbType.Decimal,
                                  CBool(accumulatorsDT.Rows(I)("ACTIVE_SW")))
                DB.AddInParameter(DBCommandWrapper, "@FAMILY_SW", DbType.Decimal,
                                  CBool(accumulatorsDT.Rows(I)("FAMILY_SW")))
                DB.AddInParameter(DBCommandWrapper, "@ACCUM_DESC", DbType.String,
                                  CStr(accumulatorsDT.Rows(I)("ACCUM_DESC")))
                DB.AddInParameter(DBCommandWrapper, "@DISPLAY_ORDER", DbType.Int32,
                                  accumulatorsDT.Rows(I)("DISPLAY_ORDER"))
                DB.AddInParameter(DBCommandWrapper, "@ACCIDENT_SW", DbType.Decimal,
                                  CBool(accumulatorsDT.Rows(I)("ACCIDENT_SW")))
                If accumulatorsDT.Rows(I)("ACCUM_ID") Is System.DBNull.Value Then
                    DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String,
                                      CStr(accumulatorsDT.Rows(I)("CREATE_USERID")))
                ElseIf CInt(accumulatorsDT.Rows(I)("ACCUM_ID")) = -1 Then
                    DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String,
                                      CStr(accumulatorsDT.Rows(I)("CREATE_USERID")))
                Else
                    DB.AddInParameter(DBCommandWrapper, "@USERID", DbType.String, CStr(accumulatorsDT.Rows(I)("UserId")))
                End If
                DB.ExecuteNonQuery(DBCommandWrapper)
            Next

            'handle the deletion of any Accumulators that have been selected to be deleted
            If deleteList IsNot Nothing Then
                If deleteList.Count > 0 Then DeleteAccumulators(deleteList)
            End If
            Transaction.Commit()

        Catch ex As Exception
            Try

                If Transaction IsNot Nothing AndAlso Transaction.Connection IsNot Nothing Then
                    Transaction.Rollback()
                End If

            Finally
            End Try

            Dim RethrowMessage As String = String.Empty
            If ex.ToString.IndexOf("DELETE statement conflicted with COLUMN REFERENCE") > -1 Then
                RethrowMessage = "An Accumulator you tried to delete is in use.  " & vbCrLf &
                                 " A person already has money applied to this accumulator."
            Else
                RethrowMessage = "There was a problem committing the changes.  A possible problem " & vbCrLf &
                                 " is that there may be an Accumulator with this name already."
            End If
            Throw New ConstraintException(RethrowMessage)
        Finally
            Connection.Close()
        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Delte the Accumulator from the db
    ' </summary>
    ' <param name="deleteList"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	12/16/2005	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Shared Sub DeleteAccumulators(ByVal deleteList As ArrayList)
        Dim DBCommandWrapper As DbCommand
        Dim DB As Database

        Try
            DB = CMSDALCommon.CreateDatabase()
            If deleteList IsNot Nothing Then
                If deleteList.Count > 0 Then
                    For I As Integer = 0 To deleteList.Count - 1
                        DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.DELETE_ACCUMULATOR")
                        DB.AddInParameter(DBCommandWrapper, "@ACCUM_ID", DbType.Int32, CInt(deleteList(I)))
                        DB.ExecuteNonQuery(DBCommandWrapper)
                    Next
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

#Region "Column Builder"
    Public Shared Function BuildAccumulatorColumns() As DataTable
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Builds Columns for the Accumulator
        ' </summary>
        ' <param name="accumulatorTable"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/15/2005	Created
        '     [paulw] 10/9/2006   Added Display Order Per ACR MED-0048
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim AccumulatorDT As DataTable

        Try
            AccumulatorDT = New DataTable("Accumulators")

            AccumulatorDT.Columns.Add("ACCUM_ID", System.Type.GetType("System.Int32"))
            AccumulatorDT.Columns.Add("ACCUM_NAME", System.Type.GetType("System.String"))
            AccumulatorDT.Columns.Add("VALUE_TYPE", System.Type.GetType("System.Int32"))
            AccumulatorDT.Columns.Add("VALUE_TYPE_NAME", System.Type.GetType("System.String"))
            AccumulatorDT.Columns.Add("FAMILY_SW", System.Type.GetType("System.Decimal"))
            AccumulatorDT.Columns.Add("DISPLAY_ORDER", System.Type.GetType("System.Int32"))
            AccumulatorDT.Columns.Add("ACTIVE_SW", System.Type.GetType("System.Decimal"))
            AccumulatorDT.Columns.Add("ACCUM_DESC", System.Type.GetType("System.String"))
            AccumulatorDT.Columns.Add("ACCIDENT_SW", System.Type.GetType("System.Decimal"))
            AccumulatorDT.Columns.Add("LASTUPDT", System.Type.GetType("System.DateTime"))
            AccumulatorDT.Columns.Add("USERID", System.Type.GetType("System.String"))
            AccumulatorDT.Columns.Add("CREATE_DATE", System.Type.GetType("System.DateTime"))
            AccumulatorDT.Columns.Add("CREATE_USERID", System.Type.GetType("System.String"))
            AccumulatorDT.Columns.Add("MANUAL_SW", System.Type.GetType("System.Decimal"))
            AccumulatorDT.Columns.Add("BATCH_SW", System.Type.GetType("System.Decimal"))
            AccumulatorDT.Columns.Add("PREVENTIVE_SW", System.Type.GetType("System.Decimal"))

            AccumulatorDT.PrimaryKey = New DataColumn() {AccumulatorDT.Columns("ACCUM_ID")}

            Return AccumulatorDT.Clone

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function BuildSummaryColumns(ByVal summaryDT As DataTable) As DataTable
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="dt"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	2/27/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        If summaryDT Is Nothing Then
            summaryDT = New DataTable("AccumulatorSummary")
        End If

        summaryDT.Columns.Add("ACCUM_ID", System.Type.GetType("System.Int32"))
        summaryDT.Columns.Add("ACCUM_YEAR", System.Type.GetType("System.Int32"))
        summaryDT.Columns.Add("RELATION_ID", System.Type.GetType("System.Int16"))
        summaryDT.Columns.Add("FAMILY_ID", System.Type.GetType("System.Int32"))

        For I As Integer = 1 To 12
            summaryDT.Columns.Add("M" & I.ToString(), System.Type.GetType("System.Decimal"))
        Next

        For I As Integer = 1 To 366
            summaryDT.Columns.Add("D" & I.ToString(), System.Type.GetType("System.Decimal"))
        Next

        For I As Integer = 4 To (12 + 366) ' + 52
            summaryDT.Columns(I).DefaultValue = 0
        Next

        'summary columns
        summaryDT.Columns.Add("Q1", System.Type.GetType("System.Decimal"), "M1 + M2 + M3")
        summaryDT.Columns.Add("Q2", System.Type.GetType("System.Decimal"), "M4 + M5 + M6")
        summaryDT.Columns.Add("Q3", System.Type.GetType("System.Decimal"), "M7 + M8 + M9")
        summaryDT.Columns.Add("Q4", System.Type.GetType("System.Decimal"), "M10 + M11 + M12")
        summaryDT.Columns.Add("TOTAL_VALUE", System.Type.GetType("System.Decimal"), "Q1 + Q2 + Q3 + Q4")
        summaryDT.Columns.Add("DAYS_INFORMATION", System.Type.GetType("System.String"))
        summaryDT.Columns.Add("ACTIVE_SW", System.Type.GetType("System.Decimal"))
        summaryDT.Columns.Add("MANUAL_SW", System.Type.GetType("System.Decimal"))
        summaryDT.Columns.Add("BATCH_SW", System.Type.GetType("System.Decimal"))

        Return summaryDT
    End Function

#End Region

#Region "Constructor"

    Private Sub New()
    End Sub

#End Region
End Class