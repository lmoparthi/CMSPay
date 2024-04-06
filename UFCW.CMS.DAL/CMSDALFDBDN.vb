Option Infer On
Option Strict On

Imports DDTek.DB2
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Configuration
Imports System.Data.Common


Public NotInheritable Class CMSDALFDBDN

    ''Private Shared _TraceCaching As New BooleanSwitch("TraceCaching", "Trace Switch in App.Config")

    Public Shared Event RecentClaimRefreshAvailable(e As RecentClaimEventArgs)

    Public Const _DB2SchemaName As String = "FDBDN"

    Private Shared _ComputerName As String = UFCWGeneral.ComputerName
    Private Shared _LastSQL As String

    Private Shared _RetrieveChemoProcedureValuesSyncLock As New Object
    Private Shared _RetrieveAllAuditRulesSyncLock As New Object
    Private Shared _GetLettersSyncLock As New Object
    Private Shared _RetrieveClaimStatusValuesSyncLock As New Object
    Private Shared _RetrievePayersSyncLock As New Object
    Private Shared _RetrieveUserDocTypesSyncLock As New Object
    Private Shared _RetrieveBillTypeValuesSyncLock As New Object
    Private Shared _RetrievePreventativeProcedureValuesSyncLock As New Object
    Private Shared _RetrievePWOExclusionProcedureValuesSyncLock As New Object
    Private Shared _RetrieveChiroProcedureValuesSyncLock As New Object
    Private Shared _LoadPlaceOfServiceValuesSyncLock As New Object
    Private Shared _DocTypelockObj As New Object
    Private Shared _ParValueslockObj As New Object
    Private Shared _DiagnosisValueslockObj As New Object
    Private Shared _DiagnosisPreventativeStatuslockObj As New Object
    Private Shared _DiagnosisValuesXreflockObj As New Object
    Private Shared _LoadNDCValuesSyncLock As New Object
    Private Shared _LoadProcedureValuesSyncLock As New Object
    Private Shared _LoadModifierValuesSyncLock As New Object
    Private Shared _RuleSetTypeslockObj As New Object
    Private Shared _COBValueslockObj As New Object
    Private Shared _PPOValueslockObj As New Object
    Private Shared _GetActiveAccumulatorsSyncLock As New Object
    Private Shared _GetRoleSettingsSyncLock As New Object

    Private Shared _RuleSetTypesTotalDS As DataSet
    Private Shared _NDCDS As DataSet
    Private Shared _DiagnosisDS As DataSet
    Private Shared _DiagnosisXRefDS As DataSet
    Private Shared _ProcedureTotalDS As DataSet
    Private Shared _ReasonDS As DataSet
    Private Shared _ReasonHIPAADS As DataSet
    Private Shared _BillTypeDS As DataSet
    Private Shared _ModifierDS As DataSet
    Private Shared _PlaceOfServiceDS As DataSet
    Private Shared _AccumulatorsDS As DataSet
    Private Shared _DocTypeDS As DataSet
    Private Shared _AuditRulesDS As DataSet
    Private Shared _UserDocTypeDS As DataSet
    Private Shared _PayeeValuesDS As DataSet
    Private Shared _PayerValuesDS As DataSet
    Private Shared _PARValuesDS As DataSet
    Private Shared _BCDetailReasonCodesDS As DataSet
    Private Shared _BCSuspendDenyCodesDS As DataSet
    Private Shared _BCDetailErrorCodesDS As DataSet
    Private Shared _BCReasonCodesDS As DataSet
    Private Shared _PPOValuesDS As DataSet
    Private Shared _COBValuesDS As DataSet
    Private Shared _PlansDS As DataSet
    Private Shared _RelationshipValuesDS As DataSet
    Private Shared _ChiroProcedureValuesDS As DataSet
    Private Shared _ChemoProcedureValuesDS As DataSet
    Private Shared _PWOExclusionProcedureValuesDS As DataSet
    Private Shared _PreventativeProcedureValuesDS As DataSet
    Private Shared _StatusValuesDS As DataSet
    Private Shared _ClaimsHistoryDT As DataTable
    Private Shared _LetterValuesDS As DataSet
    Private Shared _RoleSettingsKP As New Dictionary(Of String, String)
    Private Shared _NDCKP As New Dictionary(Of String, String)
    Public Shared _DomainUser As String = UFCWGeneral.WindowsUserID.Name

#Region "Constructor"
    Shared Sub New()

        Try

            If System.Configuration.ConfigurationManager.AppSettings("EnableDDTEKLogging") IsNot Nothing AndAlso CDbl(System.Configuration.ConfigurationManager.AppSettings("EnableDDTEKLogging")) = 1 Then
                DB2Trace.TraceFile = CMSDALLog.LogDirectory((New String() {"#"}), (New String() {If(CType(ConfigurationManager.GetSection("dataConfiguration"), Microsoft.Practices.EnterpriseLibrary.Data.Configuration.DatabaseSettings).DefaultDatabase.Contains(" P "), "Prod", "Test")})) & "DDTEK" & My.Application.Info.ProductName & UFCWGeneral.WindowsUserID.Name.Replace("\", "_").ToString & String.Format("{0000}", UFCWGeneral.NowDate.Year) & String.Format("{00}", UFCWGeneral.NowDate.Month) & String.Format("{0:00}", UFCWGeneral.NowDate.Day) & If(CBool(If(System.Configuration.ConfigurationManager.AppSettings("IncludeTimeinTraceName") Is Nothing, False, CBool(System.Configuration.ConfigurationManager.AppSettings("IncludeTimeinTraceName")))), String.Format("{0:00}", UFCWGeneral.NowDate.Hour), "").ToString & ".txt"
                DB2Trace.RecreateTrace = CInt(If(System.Configuration.ConfigurationManager.AppSettings("RecreateTrace") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("RecreateTrace"))))
                DB2Trace.EnableTrace = CInt(If(System.Configuration.ConfigurationManager.AppSettings("StartTraceImmediately") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("StartTraceImmediately"))))
            Else
                DB2Trace.EnableTrace = 0
            End If

        Catch ex As Exception

            Throw
        Finally
        End Try

    End Sub
    Private Sub New()

        Try

            If System.Configuration.ConfigurationManager.AppSettings("EnableDDTEKLogging") IsNot Nothing AndAlso CDbl(System.Configuration.ConfigurationManager.AppSettings("EnableDDTEKLogging")) = 1 Then
                DB2Trace.TraceFile = CMSDALLog.LogDirectory((New String() {"#"}), (New String() {If(CType(ConfigurationManager.GetSection("dataConfiguration"), Microsoft.Practices.EnterpriseLibrary.Data.Configuration.DatabaseSettings).DefaultDatabase.Contains(" P "), "Prod", "Test")})) & "DDTEK" & My.Application.Info.ProductName & UFCWGeneral.WindowsUserID.Name.Replace("\", "_").ToString & String.Format("{0000}", UFCWGeneral.NowDate.Year) & String.Format("{00}", UFCWGeneral.NowDate.Month) & String.Format("{0:00}", UFCWGeneral.NowDate.Day) & If(CBool(If(System.Configuration.ConfigurationManager.AppSettings("IncludeTimeinTraceName") Is Nothing, False, CBool(System.Configuration.ConfigurationManager.AppSettings("IncludeTimeinTraceName")))), String.Format("{0:00}", UFCWGeneral.NowDate.Hour), "").ToString & ".txt"
                DB2Trace.RecreateTrace = CInt(If(System.Configuration.ConfigurationManager.AppSettings("RecreateTrace") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("RecreateTrace"))))
                DB2Trace.EnableTrace = CInt(If(System.Configuration.ConfigurationManager.AppSettings("StartTraceImmediately") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("StartTraceImmediately"))))
            Else
                DB2Trace.EnableTrace = 0
            End If

        Catch ex As Exception

            Throw

        End Try

    End Sub
#End Region

#Region "Public Properties"
    Public Shared WriteOnly Property TraceEnabled() As Integer
        Set(ByVal value As Integer)
            DB2Trace.EnableTrace = value
        End Set
    End Property

    Public Shared ReadOnly Property LastSQL() As String
        Get
            Return _LastSQL
        End Get
    End Property

    Public Shared ReadOnly Property ComputerName() As String
        Get
            Return _ComputerName
        End Get
    End Property
#End Region

    Public Shared Function GetDentalInformation(ByVal fromDate As Date, ByVal toDate As Date, ByVal ssn As Integer?, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBDN.RETRIEVE_DENTAL_SUMMARY_BY_SSN_DOS")

            DB.AddInParameter(DBCommandWrapper, "@SSN", DbType.Int32, If(ssn Is Nothing, Nothing, CType(ssn, Integer?)))
            DB.AddInParameter(DBCommandWrapper, "@vFDOS_FROM", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@vFDOS_THRU", DbType.Date, toDate)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If

                ds.Tables(0).TableName = "Dental"
                'ds.Tables(0).DefaultView.RowFilter = "FDOS >= #" & fromDate & "# AND LDOS <= #" & toDate & "#"

            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "Dental")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "Dental", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function
    'Public Shared Function GetDentalPreAUTHAndPending(ByVal familyID As Integer?, ByVal relationID As Short?, ByVal gridType As String) As DataTable

    '    Dim DS As DataSet = GetDentalPreAUTHAndPending(familyID, relationID, reportType, gridType, Nothing, Nothing)

    '    If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
    '        Return DS.Tables(0)
    '    Else
    '        Return Nothing
    '    End If

    'End Function
    'Public Shared Function GetDentalPreAUTHAndPending(ByVal familyID As Integer?, ByVal relationID As Short?, ByVal reportType As String, ByVal gridType As String, Optional ByVal DS As DataSet = Nothing, Optional ByVal Transaction As DbTransaction = Nothing) As DataSet

    '    Dim DB As Database
    '    Dim DBCommandWrapper As DbCommand
    '    Try

    '        DB = CMSDALCommon.CreateDatabase()
    '        DBCommandWrapper = DB.GetStoredProcCommand("FDBDN.RETRIEVE_DENTAL_PREAUTH_PEND")

    '        DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
    '        DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
    '        DB.AddInParameter(DBCommandWrapper, "@REPORT_TYPE", DbType.String, reportType)
    '        DB.AddInParameter(DBCommandWrapper, "@GRID_TYPE", DbType.String, gridType)

    '        DBCommandWrapper.CommandTimeout = 180

    '        If DS Is Nothing Then
    '            If Transaction Is Nothing Then
    '                DS = DB.ExecuteDataSet(DBCommandWrapper)
    '            Else
    '                DS = DB.ExecuteDataSet(DBCommandWrapper, Transaction)
    '            End If

    '            DS.Tables(0).TableName = "Dental"

    '        Else
    '            If Transaction Is Nothing Then
    '                DB.LoadDataSet(DBCommandWrapper, DS, "Dental")
    '            Else
    '                DB.LoadDataSet(DBCommandWrapper, DS, "Dental", Transaction)
    '            End If
    '        End If

    '        Return DS

    '    Catch ex As Exception
    '        Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '        If (Rethrow) Then
    '            Throw
    '        End If
    '    End Try
    'End Function

    Public Shared Function GetDentalPendOrPreAuthInformation(ByVal familyID As Integer, ByVal relationID As Short?, ByVal gridType As String, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBDN.RETRIEVE_DENTAL_SUMMARY_PREAUTH_PEND")

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@GRID_TYPE", DbType.String, gridType)
            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else

                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "DentalDentalPREAuth")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "DentalDentalPREAuth", transaction)

                End If
            End If

            ds.Tables(0).TableName = "DentalPREAuth"


            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GetDentalAccumulators(ByVal familyID As Integer?, ByVal relationID As Short?, ByVal yearType As String, ByVal firstDay As Date, ByVal lastDay As Date, ByVal gridType As String) As DataTable

        Dim DS As DataSet = GetDentalAccumulators(familyID, relationID, yearType, firstDay, lastDay, gridType, Nothing, Nothing)

        If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
            Return DS.Tables(0)
        Else
            Return Nothing
        End If

    End Function
    Public Shared Function GetDentalAccumulators(ByVal familyID As Integer?, ByVal relationID As Short?, ByVal yearType As String, ByVal firstDay As Date, ByVal lastDay As Date, ByVal gridType As String, Optional ByVal DS As DataSet = Nothing, Optional ByVal Transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBDN.RETRIEVE_DENTAL_ACCUMULATORS")

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@YEAR_TYPE", DbType.String, yearType)
            DB.AddInParameter(DBCommandWrapper, "@vFIRST_DAY", DbType.Date, firstDay)
            DB.AddInParameter(DBCommandWrapper, "@vLAST_DAY", DbType.Date, lastDay)
            DB.AddInParameter(DBCommandWrapper, "@GRID_TYPE", DbType.String, gridType)

            DBCommandWrapper.CommandTimeout = 180

            If DS Is Nothing Then
                If Transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, Transaction)
                End If

                DS.Tables(0).TableName = "Dental"

            Else
                If Transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "Dental")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "Dental", Transaction)
                End If
            End If

            Return DS

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetDentalInformation(ByVal fromDate As Date, ByVal toDate As Date, ByVal familyID As Integer, Optional ByVal relationID As Short? = Nothing, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBDN.RETRIEVE_DENTAL_SUMMARY_BY_FAMILYID_RELATIONID_DOS")

            DB.AddInParameter(DBCommandWrapper, "@FAMILYID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATIONID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@vFDOS_FROM", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@vFDOS_THRU", DbType.Date, toDate)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else

                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "Dental")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "Dental", transaction)

                End If
            End If

            ds.Tables(0).TableName = "Dental"

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function UpdateDentalHistoryStatus(ByVal familyID As Integer?, ByVal relationID As Short?, ByVal claimID As Integer?, ByVal status As String, Optional ByVal transaction As DbTransaction = Nothing) As Boolean

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            status = "REPRINT_EOB "
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBDN.UPDATE_DENTAL_HISTORY_STATUS_BY_CLAIM")

            DB.AddInParameter(DBCommandWrapper, "@FAMILYID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATIONID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@PROCESS_STATUS", DbType.String, status)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_USERID", DbType.String, _DomainUser.ToUpper)
            DBCommandWrapper.CommandTimeout = 180


            DB.ExecuteDataSet(DBCommandWrapper)

            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GetDentalInformation(ByVal familyID As Integer, Optional ByVal relationID As Short? = Nothing, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBDN.RETRIEVE_DENTAL_SUMMARY_BY_FAMILYID_RELATIONID")

            DB.AddInParameter(DBCommandWrapper, "@FAMILYID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATIONID", DbType.Int16, relationID)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else

                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "Dental")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "Dental", transaction)

                End If
            End If

            ds.Tables(0).TableName = "Dental"

            'ds.Tables(0).DefaultView.RowFilter = "FDOS >= #" & fromDate & "# AND LDOS <= #" & toDate & "#"

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    'Public Shared Sub UpdateDentalHistoryStatus(ByVal familyID As Integer?, ByVal relationID As Short?, ByVal claimID As Integer?, ByVal status As String, Optional ByVal transaction As DbTransaction = Nothing)

    '    Dim DB As Database
    '    Dim DBCommandWrapper As DbCommand

    '    Try

    '        DB = CMSDALCommon.CreateDatabase()
    '        DBCommandWrapper = DB.GetStoredProcCommand("FDBDN.UPDATE_DENTAL_HISTORY_STATUS_BY_CLAIM")

    '        DB.AddInParameter(DBCommandWrapper, "@FAMILYID", DbType.Int32, familyID)
    '        DB.AddInParameter(DBCommandWrapper, "@RELATIONID", DbType.Int16, relationID)
    '        DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
    '        DB.AddInParameter(DBCommandWrapper, "@PROCESS_STATUS", DbType.String, status)
    '        DB.AddInParameter(DBCommandWrapper, "@ONLINE_USERID", DbType.String, _DomainUser.ToUpper)
    '        DBCommandWrapper.CommandTimeout = 180


    '        DB.ExecuteDataSet(DBCommandWrapper)


    '    Catch ex As Exception
    '        Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '        If (Rethrow) Then
    '            Throw
    '        End If
    '    End Try
    'End Sub

    Public Shared Function GetDentalClaimDetail(ByVal familyID As Integer?, ByVal relationID As Short?, ByVal claimID As Integer?) As DataTable

        Dim DS As DataSet = GetDentalClaimDetail(familyID, relationID, claimID, Nothing, Nothing)

        If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
            Return DS.Tables(0)
        Else
            Return Nothing
        End If

    End Function
    Public Shared Function GetDentalClaimDetail(ByVal familyID As Integer?, ByVal relationID As Short?, ByVal claimID As Integer?, Optional ByVal DS As DataSet = Nothing, Optional ByVal Transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBDN.RETRIEVE_DENTAL_CLAIM_BY_FAMILYID_RELATIONID")

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM", DbType.Int32, claimID)


            DBCommandWrapper.CommandTimeout = 180

            If DS Is Nothing Then
                If Transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, Transaction)
                End If

                DS.Tables(0).TableName = "Dental"

            Else
                If Transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "Dental")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "Dental", Transaction)
                End If
            End If

            Return DS

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetDentalLineDetail(ByVal familyID As Integer?, ByVal relationID As Short?, ByVal claimID As Integer?, ByVal procedure As String) As DataTable

        Dim DS As DataSet = GetDentalLineDetail(familyID, relationID, claimID, procedure, Nothing, Nothing)

        If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
            Return DS.Tables(0)
        Else
            Return Nothing
        End If

    End Function

    Public Shared Function GetDentalLineDetail(ByVal familyID As Integer?, ByVal relationID As Short?, ByVal claimID As Integer?, ByVal procedure As String, Optional ByVal DS As DataSet = Nothing, Optional ByVal Transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBDN.RETRIEVE_DENTAL_LINE_DETAIL")

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@PROCEDURE", DbType.String, procedure)

            DBCommandWrapper.CommandTimeout = 180

            If DS Is Nothing Then
                If Transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, Transaction)
                End If

                DS.Tables(0).TableName = "Dental"

            Else
                If Transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "Dental")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "Dental", Transaction)
                End If
            End If

            Return DS

        Catch ex As Exception
            Throw
        End Try
    End Function

End Class

