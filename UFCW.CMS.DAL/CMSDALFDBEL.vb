
Imports DDTek.DB2

Imports Microsoft.Practices.EnterpriseLibrary.Data

Imports System.IO
Imports System.Data.Common
Imports System.Xml.Serialization


Public Class CMSDALFDBEL
    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Public Const _DB2databaseName As String = "FDBEL"

    Private Shared _RegAlertReasonValueslockObj As New Object
    Private Shared _RegAlertReasonValuesDS As DataSet
    Private Shared _GlobalEligPeriod As Date?

#Region "Constructor"

    Shared Sub New()

        'Try

        '    If System.Configuration.ConfigurationManager.AppSettings("EnableDDTEKLogging") IsNot Nothing AndAlso CDbl(System.Configuration.ConfigurationManager.AppSettings("EnableDDTEKLogging")) = 1 Then
        '        If Replace(System.Configuration.ConfigurationManager.AppSettings("LogDirectory"), "..", System.Windows.Forms.Application.StartupPath.ToString).Length > 0 Then
        '            Dim directoryInfo As New DirectoryInfo(Path.GetDirectoryName(Replace(System.Configuration.ConfigurationManager.AppSettings("LogDirectory"), "..", System.Windows.Forms.Application.StartupPath.ToString)))

        '            If directoryInfo.Exists = False Then
        '                directoryInfo.Create()
        '            End If
        '        End If

        '        DB2Trace.TraceFile = Replace(System.Configuration.ConfigurationManager.AppSettings("LogDirectory"), "..", System.Windows.Forms.Application.StartupPath.ToString) & "DDTEK" & My.Application.Info.ProductName & UFCWGeneral.WindowsUserID.Name.Replace("\", "_").ToString & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & String.Format("{0:00}", Generals.NowDate.Day) & If(CBool(If(System.Configuration.ConfigurationManager.AppSettings("IncludeTimeinTraceName") Is Nothing, False, CBool(System.Configuration.ConfigurationManager.AppSettings("IncludeTimeinTraceName")))), String.Format("{0:00}", Generals.NowDate.Hour), "").ToString & ".txt"
        '        DB2Trace.RecreateTrace = CInt(If(System.Configuration.ConfigurationManager.AppSettings("RecreateTrace") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("RecreateTrace"))))
        '        DB2Trace.EnableTrace = CInt(If(System.Configuration.ConfigurationManager.AppSettings("StartTraceImmediately") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("StartTraceImmediately"))))
        '    Else
        '        DB2Trace.EnableTrace = 0
        '    End If

        'Catch ex As Exception

        '    Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
        '    If (rethrow) Then
        '        Throw
        '    End If

        'End Try

    End Sub
    Private Sub New()

        'Try

        'If System.Configuration.ConfigurationManager.AppSettings("EnableDDTEKLogging") IsNot Nothing AndAlso CDbl(System.Configuration.ConfigurationManager.AppSettings("EnableDDTEKLogging")) = 1 Then
        '    If Replace(System.Configuration.ConfigurationManager.AppSettings("LogDirectory"), "..", System.Windows.Forms.Application.StartupPath.ToString).Length > 0 Then
        '        Dim directoryInfo As New DirectoryInfo(Path.GetDirectoryName(Replace(System.Configuration.ConfigurationManager.AppSettings("LogDirectory"), "..", System.Windows.Forms.Application.StartupPath.ToString)))

        '        If directoryInfo.Exists = False Then
        '            directoryInfo.Create()
        '        End If
        '    End If

        '    DB2Trace.TraceFile = Replace(System.Configuration.ConfigurationManager.AppSettings("LogDirectory"), "..", System.Windows.Forms.Application.StartupPath.ToString) & "DDTEK" & My.Application.Info.ProductName & UFCWGeneral.WindowsUserID.Name.Replace("\", "_").ToString & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & String.Format("{0:00}", Generals.NowDate.Day) & If(CBool(If(System.Configuration.ConfigurationManager.AppSettings("IncludeTimeinTraceName") Is Nothing, False, CBool(System.Configuration.ConfigurationManager.AppSettings("IncludeTimeinTraceName")))), String.Format("{0:00}", Generals.NowDate.Hour), "").ToString & ".txt"
        '    DB2Trace.RecreateTrace = CInt(If(System.Configuration.ConfigurationManager.AppSettings("RecreateTrace") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("RecreateTrace"))))
        '    DB2Trace.EnableTrace = CInt(If(System.Configuration.ConfigurationManager.AppSettings("StartTraceImmediately") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("StartTraceImmediately"))))
        'Else
        '    DB2Trace.EnableTrace = 0
        'End If

        'Catch ex As Exception

        '    Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
        '    If (rethrow) Then
        '        Throw
        '    End If

        'End Try

    End Sub
#End Region

#Region "Public Properties"
    Public Shared WriteOnly Property TraceEnabled() As Integer
        Set(ByVal value As Integer)
            DB2Trace.EnableTrace = value
        End Set
    End Property

    Public Shared ReadOnly Property GlobalEligPeriod As Date
        Get
            If _GlobalEligPeriod Is Nothing Then
                RetrieveEligibilityPeriod()
            End If
            Return CDate(_GlobalEligPeriod)
        End Get
    End Property

#End Region

#Region "CRUD"

    Public Shared Function RetrieveContactInfoByFamilyID(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        ds = RetrieveRegEmailByFamilyID(familyID, ds)
        ds = RetrieveRegPhoneByFamilyID(familyID, ds)

        Return ds

    End Function

    Public Shared Function RetrieveContactInfoByFamilyIDRelationID(ByVal familyID As Integer, ByVal relationID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        ds = RetrieveAllContactInfoByFamilyIDRelationID(familyID, relationID, ds)

        Return ds

    End Function
    Public Shared Function RetrieveCoverageHistoryAndNetworks(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DbCommand As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase()

            SQLCall = "FDBEL.RETRIEVE_COVERAGE_HISTORY_HMO_NETWORKS_BY_FAMILYID_RELATIONID"
            Dim tablenames() As String = {"COVERAGE_HISTORY", "HMO_NETWORK"}
            DbCommand = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DbCommand, "@FAMILY_ID", DbType.Int32, familyID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DbCommand)
                Else
                    ds = DB.ExecuteDataSet(DbCommand, transaction)
                End If

                ds.Tables(0).TableName = "COVERAGE_HISTORY"
                ds.Tables(1).TableName = "HMO_NETWORK"

            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DbCommand, ds, tablenames)
                Else
                    DB.LoadDataSet(DbCommand, ds, tablenames, transaction)
                End If
            End If
            Return ds
        Catch ex As Exception
            Dim Rethrow As Boolean = True
            If (Rethrow) Then
                Throw
            End If
            Return Nothing
        End Try
    End Function

    Public Shared Function RetrieveCoverageHistoryAndNetwork(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DbCommand As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase()

            SQLCall = "FDBEL.RETRIEVE_COVERAGE_HISTORY_HMO_NETWORK_BY_FAMILYID_RELATIONID"
            Dim tablenames() As String = {"COVERAGE_HISTORY", "HMO_NETWORK"}
            DbCommand = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DbCommand, "@FAMILY_ID", DbType.Int32, familyID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DbCommand)
                Else
                    ds = DB.ExecuteDataSet(DbCommand, transaction)
                End If

                ds.Tables(0).TableName = "COVERAGE_HISTORY"
                ds.Tables(1).TableName = "HMO_NETWORK"

            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DbCommand, ds, tablenames)
                Else
                    DB.LoadDataSet(DbCommand, ds, tablenames, transaction)
                End If
            End If
            Return ds
        Catch ex As Exception
            Dim Rethrow As Boolean = True
            If (Rethrow) Then
                Throw
            End If
            Return Nothing
        End Try
    End Function

    Public Shared Function RetrieveRegEmailByFamilyID(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Try

            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommand As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_REGEMAIL_BY_FAMILYID"

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int64, familyID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If
                ds.Tables(0).TableName = "REG_EMAIL"
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, "REG_EMAIL")
                Else
                    DB.LoadDataSet(DBCommand, ds, "REG_EMAIL", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception

	Throw
            Return Nothing
        End Try
    End Function

    Public Shared Function RetrieveAllContactInfoByFamilyIDRelationID(ByVal familyID As Integer, ByVal relationID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommand As DbCommand
        Dim SQLCall As String = "FDBEL.RETRIEVE_REGDEMOGRAPHICS_BY_FAMILYIDRELATIONID"
        Dim tablenames() As String = {"REG_EMAIL", "REG_PHONE", "REG_ADDRESS", "REG_REGISTRATION_INFO"}

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommand, "@RELATION_ID", DbType.Int16, relationID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If
                ds.Tables(0).TableName = "REG_EMAIL"
                ds.Tables(1).TableName = "REG_PHONE"
                ds.Tables(2).TableName = "REG_ADDRESS"
                ds.Tables(3).TableName = "REG_REGISTRATION_INFO"
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, tablenames)
                Else
                    DB.LoadDataSet(DBCommand, ds, tablenames, transaction)
                End If
            End If
            Return ds
        Catch ex As Exception

	Throw
            Return Nothing
        End Try
    End Function

    Public Shared Function RetrieveRegEmailByFamilyIDRelationID(ByVal familyID As Integer, ByVal relationID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Try

            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommand As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_REGEMAIL_BY_FAMILYID_RELATIONID"

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommand, "@RELATION_ID", DbType.Int32, relationID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If
                ds.Tables(0).TableName = "REG_EMAIL"
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, "REG_EMAIL")
                Else
                    DB.LoadDataSet(DBCommand, ds, "REG_EMAIL", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception

	Throw
            Return Nothing
        End Try
    End Function

    Public Shared Function RetrieveRegPhoneByFamilyID(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Try

            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommand As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_PHONE_NUMBERS_BY_FAMILYID"

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int64, familyID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If
                ds.Tables(0).TableName = "REG_PHONE"
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, "REG_PHONE")
                Else
                    DB.LoadDataSet(DBCommand, ds, "REG_PHONE", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception

	Throw
            Return Nothing
        End Try
    End Function

    Public Shared Function RetrieveRegPhoneByFamilyIDRelationID(ByVal familyID As Integer, ByVal relationID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Try

            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommand As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_PHONE_NUMBERS_BY_FAMILYID_RELATIONID"

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommand, "@RELATION_ID", DbType.Int32, relationID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If
                ds.Tables(0).TableName = "REG_PHONE"
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, "REG_PHONE")
                Else
                    DB.LoadDataSet(DBCommand, ds, "REG_PHONE", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception

	Throw
            Return Nothing
        End Try
    End Function

    Public Shared Function RetrievePatientsLifeEvents(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Try
            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_REG_LIFE_EVENTS_BY_FAMILYID"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
                ds.Tables(0).TableName = "REG_LIFE_EVENTS"
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "REG_LIFE_EVENTS")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "REG_LIFE_EVENTS", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception

	Throw
        End Try
    End Function

    Public Shared Function RetrievePatientsSummarizedCoverableGapInfo(ByVal familyID As Integer, ByVal relationID As Short, ByVal fromDate As Date?, ByVal thruDate As Date?, Optional ByVal transaction As DbTransaction = Nothing) As DataTable

        Dim DS As DataSet
        Dim DT As DataTable

        Try

            DS = RetrievePatientsCoverableGapInfo(familyID, relationID, fromDate, thruDate, transaction)

            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                DT = New DataTable
                DT.Columns.Add("BeginDate", System.Type.GetType("System.DateTime"))
                DT.Columns.Add("EndDate", System.Type.GetType("System.DateTime"))

                Dim BeginDate As Date? = Nothing
                Dim EndDate As Date? = Nothing

                For Each DR As DataRow In DS.Tables(0).Rows

                    If BeginDate Is Nothing Then
                        BeginDate = CDate(DR("DIA"))
                        EndDate = CDate(DR("DIA"))
                    ElseIf CDate(DR("DIA")) = CDate(EndDate).AddDays(1) Then
                        EndDate = CDate(DR("DIA"))
                    Else
                        DT.Rows.Add(New Object() {BeginDate, EndDate})
                        BeginDate = CDate(DR("DIA"))
                        EndDate = CDate(DR("DIA"))
                    End If

                Next

                'if the reg mast thru date has the same day as the end of the gap range, the gap is not truly a coverage gap, but rather an eligibility gap and should not be reported 
                If thruDate <> EndDate Then DT.Rows.Add(New Object() {BeginDate, EndDate})

                DT.TableName = "REG_LIFE_EVENT_GAPS"

                Return DT

            End If

            Return Nothing

        Catch ex As Exception
            Throw
        Finally
            If DT IsNot Nothing Then DT.Dispose()
            If DS IsNot Nothing Then DS.Dispose()

        End Try
    End Function

    Public Shared Function RetrievePatientsCoverableGapInfo(ByVal familyID As Integer, ByVal relationID As Short, ByVal fromDate As Date?, ByVal thruDate As Date?, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim DS As DataSet

        Dim SQLCall As String = "FDBEL.RETRIEVE_DAYS_NOT_COVERABLE_BY_FAMILYID_RELATIONID"

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, {thruDate, Now}.Max())

            DBCommandWrapper.CommandTimeout = 180

            If DS Is Nothing Then
                If transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "DAYS_NOT_ELIGIBLE")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "DAYS_NOT_ELIGIBLE", transaction)
                End If
            End If

            Return DS

        Catch ex As Exception

	Throw
        End Try
    End Function

    Public Shared Function GetEligibilityHours(ByVal familyId As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBEL.RETRIEVE_ELIGIBILITY_HOURS_BY_FAMILYID")

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyId)
            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
                ds.Tables(0).TableName = "ELIGIBILITY_HOURS"

            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "ELIGIBILITY_HOURS")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "ELIGIBILITY_HOURS", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception

	Throw
        End Try
    End Function

    Public Shared Function RetrievePatientsDaysNotCoverable(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date, ByVal thruDate As Date, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataRow
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBEL.RETRIEVE_COUNT_OF_DAYS_NOT_COVERABLE_BY_FAMILYID_RELATIONID"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "DAYS_NOT_ELIGIBLE")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "DAYS_NOT_ELIGIBLE", transaction)
                End If
            End If

            If ds.Tables(0).Rows.Count > 0 Then
                Return ds.Tables(0).Rows(0)
            End If

            Return Nothing

        Catch ex As Exception

	Throw
        End Try
    End Function

    Public Shared Function RetrievePatientsMonthsCoverableGapInfo(ByVal familyID As Integer, ByVal relationID As Short, ByVal fromDate As Date, ByVal thruDate As Date, Optional ByVal transaction As DbTransaction = Nothing) As DataTable
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim DS As DataSet = Nothing

        Dim SQLCall As String = "FDBEL.RETRIEVE_MONTH_YEAR_NOT_COVERABLE_BY_FAMILYID_RELATIONID"

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)

            DBCommandWrapper.CommandTimeout = 180

            If DS Is Nothing Then
                If transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "MONTH_YEAR_NOT_ELIGIBLE")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "MONTH_YEAR_NOT_ELIGIBLE", transaction)
                End If
            End If

            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0)
            Else
                Return Nothing
            End If

        Catch ex As Exception

	Throw
        End Try
    End Function

    Public Shared Function RetrieveRegLifeEventsForAuditByFamilyIDRelationID(ByVal familyID As Integer, ByVal relationID As Short?, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataTable
        Dim DB As Database
        Dim DBCommand As DbCommand
        Dim SQLCall As String = "FDBEL.RETRIEVE_AUDIT_LIFEEVENTS_BY_FAMILYID_RELATIONID"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(dbCommand, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(dbCommand, "@RELATION_ID", DbType.Int32, relationID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(dbCommand)
                Else
                    ds = DB.ExecuteDataSet(dbCommand, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(dbCommand, ds, "REG_LIFE_EVENTS")
                Else
                    DB.LoadDataSet(dbCommand, ds, "REG_LIFE_EVENTS", transaction)
                End If
            End If

            If ds.Tables.Count > 0 Then
                ds.Tables(0).TableName = "REG_LIFE_EVENTS"
                Return ds.Tables(0)
            End If

            Return Nothing

        Catch ex As Exception

	Throw
        End Try
    End Function

    Public Shared Function RetrieveRegAlertsByFamilyID(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommand As DbCommand
        Dim SQLCall As String = "FDBEL.RETRIEVE_REG_ALERTS_BY_FAMILYID"


        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int64, familyID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, "REG_ALERTS")
                Else
                    DB.LoadDataSet(DBCommand, ds, "REG_ALERTS", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception

	Throw
        End Try
    End Function

    Public Shared Function RetrieveEligibilityPeriod() As DataTable
        Dim DB As Database
        Dim DBCommand As DbCommand
        Dim SQLCall As String = "FDBEL.RETRIEVE_ELIG_PERIOD"
        Try

            DB = CMSDALCommon.CreateDatabase()
            dbCommand = DB.GetStoredProcCommand(SQLCall)

            Dim DS As DataSet = DB.ExecuteDataSet(dbCommand)
            Dim DT As New DataTable("ELIG_PERIOD")

            If DS.Tables.Count > 0 Then
                DT = DS.Tables(0)
                _GlobalEligPeriod = CDate(DS.Tables(0).Rows(0)("ELIG_PER_DATE"))
            End If

            Return DT
        Catch ex As Exception

	Throw
            Return Nothing
        End Try

    End Function
#End Region

#Region "Cache"

    Public Shared Function RetrieveRegAlertReasonValues() As DataTable

        Dim DT As DataTable

        Try

            If _RegAlertReasonValuesDS Is Nothing Then
                _RegAlertReasonValuesDS = LoadRegAlertReasonValues()
            End If

            DT = New DataTable("REG_ALERT_REASON_VALUES")
            If _RegAlertReasonValuesDS.Tables.Count > 0 Then
                DT = _RegAlertReasonValuesDS.Tables(0)
            End If

            Return DT

        Catch ex As Exception
            Throw
        Finally
            If DT IsNot Nothing Then DT.Dispose()
        End Try

    End Function

    Private Shared Function LoadRegAlertReasonValues(Optional ByVal ds As DataSet = Nothing) As DataSet
        Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBEL.RETRIEVE_REG_ALERT_REASON_VALUES" & ".xml"
        Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
        Dim FStream As FileStream

        Try

            SyncLock (_RegAlertReasonValueslockObj)
                Dim XMLDS As DataSet = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBEL.REG_ALERT_REASON_VALUES", "LASTUPDT", "FDBEL.RETRIEVE_REG_ALERT_REASON_VALUES")
                If XMLDS.Tables.Count = 0 Then
                    Dim DB As Database = CMSDALCommon.CreateDatabase()
                    Dim DBCommandWrapper As DbCommand
                    Dim SQLCall As String = "FDBEL.RETRIEVE_REG_ALERT_REASON_VALUES"
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        XMLDS = DB.ExecuteDataSet(DBCommandWrapper)
                    Else
                        DB.LoadDataSet(DBCommandWrapper, XMLDS, "REG_ALERT_REASON_VALUES")
                    End If

                    _RegAlertReasonValuesDS = XMLDS

                    FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                    Dim XMLSerial As New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(FStream, XMLDS)

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _RegAlertReasonValuesDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_RegAlertReasonValuesDS)
                    Return ds
                Else
                    Return _RegAlertReasonValuesDS
                End If

            End SyncLock

        Catch ex As Exception

            Throw
        Finally
            If FStream IsNot Nothing Then FStream.Close()

        End Try
    End Function

#End Region

End Class
