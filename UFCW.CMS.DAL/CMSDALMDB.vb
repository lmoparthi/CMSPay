Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.IO
Imports System.Data.Common

Public Class CMSDALMDB
    'Shared Sub New()

    '    AddHandler Application.ThreadException, AddressOf UFCW.CMS.CMSDALThreadExceptionHandler.Application_ThreadException
    '    AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf UFCW.CMS.CMSDALThreadExceptionHandler.CurrentDomain_UnhandledException

    'End Sub

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
    Private Shared _DBlock As Object = New Object()

    Public Shared Sub UpdateMDBStatus(mdbDataSetName As String, ByVal jobName As String, ByVal active As Boolean)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try

            SyncLock (_DBlock)

                DB = CMSDALCommon.CreateAccessConnection(If(mdbDataSetName.ToUpper.Contains("ACCDB"), "ACCDB", "MDB") & " Database Instance" & ";" & mdbDataSetName)

                Dim SQLQuery As String = "UPDATE [Job Profiles] SET Active = " & If(active, 0, 1) & "  WHERE (Name= '" & jobName & "') "

                DBCommandWrapper = DB.GetSqlStringCommand(SQLQuery)
                DB.ExecuteNonQuery(DBCommandWrapper)

            End SyncLock

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Sub

    Public Shared Sub UpdateMDBLastRun(mdbDataSetName As String, ByVal jobName As String, ByVal lastRunStart As Date?, ByVal lastRunComplete As Date?, ByVal lastMessage As String)

        Dim SQLQuery As String

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try

            SyncLock (_DBlock)

                DB = CMSDALCommon.CreateAccessConnection(If(mdbDataSetName.ToUpper.Contains("ACCDB"), "ACCDB", "MDB") & " Database Instance" & ";" & mdbDataSetName)

                lastMessage = Replace(lastMessage, "'", "''")
                SQLQuery = "UPDATE [Job Profiles] SET " & If(lastRunStart IsNot Nothing, "LastRunStart = #" & lastRunStart & "#,", "") & If(lastRunComplete IsNot Nothing, "LastRunComplete = #" & lastRunComplete & "#,", "") & " LastMessage = '" & lastMessage & "'  WHERE (Name= '" & jobName & "') "

                DBCommandWrapper = DB.GetSqlStringCommand(SQLQuery)
                DB.ExecuteNonQuery(DBCommandWrapper)

            End SyncLock

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Sub

    Public Shared Function LoadTaskDS(tasksDS As DataSet, mdbDataSetName As String) As DataSet

        Dim DB As Database = CMSDALCommon.CreateAccessConnection(If(mdbDataSetName.ToUpper.Contains("ACCDB"), "ACCDB", "MDB") & " Database Instance" & ";" & mdbDataSetName)
        Dim DBCommandWrapper As DbCommand

        Try

            Dim SQLQuery As String = "SELECT [Job Steps].* " &
                                     "FROM [Job Steps]"
            DBCommandWrapper = DB.GetSqlStringCommand(SQLQuery)
            DB.LoadDataSet(DBCommandWrapper, tasksDS, "Job Steps")

            SQLQuery = "SELECT * " &
                       "FROM [Job Schedules] "
            DBCommandWrapper = DB.GetSqlStringCommand(SQLQuery)
            DB.LoadDataSet(DBCommandWrapper, tasksDS, "Job Schedules")

            SQLQuery = "SELECT JP.*, JS.StartTime, JS.ContinualExecution " &
                       "FROM [Job Profiles] AS JP LEFT JOIN " &
                       "     [Job Schedules] AS JS ON JS.NAME = JP.NAME " &
                       "WHERE (NOW() BETWEEN JP.FromDate AND JP.ToDate) " &
                       "ORDER BY JP.[ExecutionOrder], JS.[StartTime]"
            DBCommandWrapper = DB.GetSqlStringCommand(SQLQuery)
            DB.LoadDataSet(DBCommandWrapper, tasksDS, "Job Profiles")

            SQLQuery = " SELECT [Job FileWatcher].FromDate, [Job FileWatcher].ToDate, [Job FileWatcher].Name, " &
                       " [Job FileWatcher].Priority, [Job FileWatcher].FileMask, [Job FileWatcher].MinimumSize " &
                       " FROM [Job FileWatcher] "
            DBCommandWrapper = DB.GetSqlStringCommand(SQLQuery)
            DB.LoadDataSet(DBCommandWrapper, tasksDS, "Job FileWatcher")

            SQLQuery = " SELECT [Job Profiles].FromDate, [Job Profiles].ToDate, [Job Profiles].Name, [Job Profiles].Active, [Job Profiles].Desc " &
                       " FROM [Job Profiles] WHERE ((([Job Profiles].Active)=False)) "
            DBCommandWrapper = DB.GetSqlStringCommand(SQLQuery)
            DB.LoadDataSet(DBCommandWrapper, tasksDS, "InactiveJobs")

            Return tasksDS

        Catch ex As Exception


	Throw

        Finally

        End Try

    End Function

    Public Shared Function OLEDBRowCount(dataSetName As String) As Integer?

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand


        Dim ExcelSchemaDT As New DataTable
        Dim ExcelConn As DbConnection
        Dim Rows As Integer

        Try

            Try
                DB = CMSDALCommon.CreateAccessConnection("OLEDB " & UFCWGeneral.GetSuffix(dataSetName).ToUpper & " Instance" & ";" & dataSetName)

            Catch ex As ApplicationException
                'allow flow to process as text file
            Catch ex As Exception

                Throw

            End Try

            If DB IsNot Nothing Then
                ExcelConn = DB.CreateConnection
                ExcelConn.Open()

                ExcelSchemaDT = ExcelConn.GetSchema("Tables")

                If ExcelSchemaDT.Rows.Count > 0 Then 'excel
                    For Each DR As DataRow In ExcelSchemaDT.Rows
                        Dim SQLQuery As String = "SELECT COUNT(*) from [" & DR("TABLE_NAME").ToString & "]"
                        DBCommandWrapper = DB.GetSqlStringCommand(SQLQuery)

                        Rows += CInt(DB.ExecuteScalar(DBCommandWrapper))

                    Next

                Else 'assume txt file
                    Rows = File.ReadAllLines(dataSetName).Length
                End If
            Else 'assume txt file
                Rows = File.ReadAllLines(dataSetName).Length
            End If

            Return Rows

        Catch ex As Exception

            Return Nothing

        Finally

            If ExcelSchemaDT IsNot Nothing Then ExcelSchemaDT.Dispose()
            ExcelSchemaDT = Nothing

            If ExcelConn IsNot Nothing Then
                ExcelConn.Close()
                ExcelConn.Dispose()
            End If
            ExcelConn = Nothing

            DB = Nothing

        End Try

    End Function

End Class
