Option Infer On
Option Strict On

Imports DDTek.DB2
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Configuration
Imports System.Text
Imports System.Xml
Imports System.IO
Imports System.Xml.Serialization
Imports System.Text.RegularExpressions
Imports System.Data.Common
Imports System.Threading.Tasks


Public NotInheritable Class CMSDALFDBMD

    ''Private Shared _TraceCaching As New BooleanSwitch("TraceCaching", "Trace Switch in App.Config")

    Public Shared Event RecentClaimRefreshAvailable(e As RecentClaimEventArgs)

    Public Const _DB2SchemaName As String = "FDBMD"

    Private Shared ReadOnly _ComputerName As String = UFCWGeneral.ComputerName
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

#Region "Constructor"
    Shared Sub New()

        Try

            If System.Configuration.ConfigurationManager.AppSettings("EnableDDTEKLogging") IsNot Nothing AndAlso CDbl(System.Configuration.ConfigurationManager.AppSettings("EnableDDTEKLogging")) = 1 Then
                DB2Trace.TraceFile = CMSDALLog.LogDirectory((New String() {"#"}), (New String() {If(CType(ConfigurationManager.GetSection("dataConfiguration"), Microsoft.Practices.EnterpriseLibrary.Data.Configuration.DatabaseSettings).DefaultDatabase.Contains(" P "), "Prod", "Test")})) & "DDTEK" & My.Application.Info.ProductName & UFCWGeneral.WindowsUserID.Name.Replace("\", "_").ToString & String.Format("{0000}", UFCWGeneral.NowDate.Year) & String.Format("{00}", UFCWGeneral.NowDate.Month) & String.Format("{0:00}", UFCWGeneral.NowDate.Day) & If(CBool(System.Configuration.ConfigurationManager.AppSettings("IncludeTimeinTraceName") IsNot Nothing AndAlso CBool(System.Configuration.ConfigurationManager.AppSettings("IncludeTimeinTraceName"))), String.Format("{0:00}", UFCWGeneral.NowDate.Hour), "").ToString & ".txt"
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

#Region "Cache"
    Public Shared Function RetrieveRoleSettings(ByVal settingName As String) As Object

        SyncLock _GetRoleSettingsSyncLock
            Try

                If _RoleSettingsKP Is Nothing OrElse Not _RoleSettingsKP.ContainsKey(settingName) Then

                    Dim SETTING_VALUE As String = LoadRoleSettings(settingName).ToString

                    If SETTING_VALUE Is Nothing Then Return Nothing

                    _RoleSettingsKP.Add(settingName, SETTING_VALUE)

                End If

                Return _RoleSettingsKP(settingName)

            Catch ex As Exception
                Throw
            End Try
        End SyncLock

    End Function

    Public Shared Function LoadRoleSettings(ByVal settingName As String) As Object

        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_ROLE_SETTINGS_BY_NAME"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@SETTING_NAME", DbType.String, settingName)

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0).Rows(0)("SETTING_VALUE")
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw

        End Try
    End Function

    Public Shared Function RetrieveNDCDesc(ByVal ndc As String) As String


        Try

            If _NDCKP Is Nothing OrElse Not _NDCKP.ContainsKey(ndc) Then

                Dim DR As DataRow = LoadNDC(ndc)

                If DR Is Nothing Then Return Nothing

                _NDCKP.Add(ndc, If(IsDBNull(DR("NDC_DESC")), "", DR("NDC_DESC").ToString))

            End If

            Return _NDCKP(ndc)

        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Shared Function LoadNDC(ByVal ndc As String) As DataRow

        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RX_RETRIEVE_NDC_DATA"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@NDC", DbType.String, ndc)

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0).Rows(0)
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function RetrievePlaceOfServiceValueInformation(ByVal placeOfServiceCode As String, Optional ByVal effectiveDate As Date? = Nothing) As DataRow

        Dim SelectFragment As String
        Dim SelectedRows As DataRow()

        Try
            If _PlaceOfServiceDS Is Nothing Then
                _PlaceOfServiceDS = RetrievePlaceOfServiceValues(Nothing, _PlaceOfServiceDS)
            End If

            If IsDBNull(effectiveDate) OrElse effectiveDate Is Nothing Then
            Else
                SelectFragment = " AND #" & Format(effectiveDate, "yyyy-MM-dd").ToString & "# >= FROM_DATE AND #" & Format(effectiveDate, "yyyy-MM-dd").ToString & "# <= THRU_DATE "
            End If

            SelectedRows = _PlaceOfServiceDS.Tables(0).Select("PLACE_OF_SERV_VALUE = '" & placeOfServiceCode.ToString.Trim & "'" & SelectFragment)

            If SelectedRows.Length > 0 Then
                Return SelectedRows(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function RetrievePlaceOfServiceValues(ByVal effectiveDate As Date?, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim CloneDS As DataSet

        Try

            If _PlaceOfServiceDS Is Nothing Then
                _PlaceOfServiceDS = RetrievePlaceOfServiceValues(ds, transaction)
            End If

            CloneDS = _PlaceOfServiceDS.Clone

            If effectiveDate Is Nothing OrElse IsDBNull(effectiveDate) Then
                effectiveDate = UFCWGeneral.NowDate
            End If

            _PlaceOfServiceDS.Tables(0).DefaultView.Sort = "PLACE_OF_SERV_VALUE"
            _PlaceOfServiceDS.Tables(0).DefaultView.RowFilter = "#" & Format(effectiveDate, "yyyy-MM-dd").ToString & "# >= FROM_DATE AND #" & Format(effectiveDate, "yyyy-MM-dd").ToString & "# <= THRU_DATE "

            For Each PlaceOfServiceRow As DataRowView In _PlaceOfServiceDS.Tables(0).DefaultView
                CloneDS.Tables(0).LoadDataRow(PlaceOfServiceRow.Row.ItemArray, True)
            Next

            If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                ds.Merge(CloneDS)
                Return ds
            End If

            Return CloneDS

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function RetrievePlaceOfServiceValues(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        If _PlaceOfServiceDS Is Nothing Then
            _PlaceOfServiceDS = LoadPlaceOfServiceValues(ds, transaction)
        End If

        Return _PlaceOfServiceDS

    End Function

    'Note this is marked private to force all other access to data to come through cached references
    Private Shared Function LoadPlaceOfServiceValues(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        SyncLock _LoadPlaceOfServiceValuesSyncLock

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_ALL_PLACE_OF_SERV_VALUES" & ".xml"
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim XMLDS As DataSet
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBMD.RETRIEVE_ALL_PLACE_OF_SERV_VALUES"
            Dim SWriter As System.IO.StreamWriter
            Dim XMLSerial As XmlSerializer

            Try

                XMLDS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.PLACE_OF_SERVE_VALUES", "LASTUPDT", "FDBMD.RETRIEVE_ALL_PLACE_OF_SERV_VALUES")
                If XMLDS.Tables.Count = 0 Then

                    DB = CMSDALCommon.CreateDatabase()
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        If transaction Is Nothing Then
                            XMLDS = DB.ExecuteDataSet(DBCommandWrapper)
                        Else
                            XMLDS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                        End If
                    Else
                        If transaction Is Nothing Then
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "PLACE_OF_SERV_VALUES")
                        Else
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "PLACE_OF_SERV_VALUES", transaction)
                        End If
                    End If

                    _PlaceOfServiceDS = XMLDS

                    SWriter = New System.IO.StreamWriter(XMLFilename)
                    XMLSerial = New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(SWriter, XMLDS)
                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)

                Else
                    _PlaceOfServiceDS = XMLDS
                End If

                If ds IsNot Nothing Then
                    ds.Merge(_PlaceOfServiceDS)
                    Return ds
                Else
                    Return _PlaceOfServiceDS
                End If

            Catch ex As Exception
                Throw
            Finally

                If SWriter IsNot Nothing Then SWriter.Close()
                SWriter = Nothing

            End Try
        End SyncLock


    End Function

    Public Shared Function RetrieveChiroProcedureValues() As String()

        If _ChiroProcedureValuesDS Is Nothing Then
            _ChiroProcedureValuesDS = RetrieveChiroProcedureValues(_ChiroProcedureValuesDS)
        End If

        Dim ChiroList As New ArrayList(_ChiroProcedureValuesDS.Tables("CHIRO_PROCEDURE_CODES").Rows.Count)

        For Each ChirowRow As DataRowView In _ChiroProcedureValuesDS.Tables("CHIRO_PROCEDURE_CODES").DefaultView
            ChiroList.Add(ChirowRow("PROC_CODE"))
        Next

        Return CType(ChiroList.ToArray(GetType(String)), String())

    End Function

    Private Shared Function RetrieveChiroProcedureValues(ByVal ds As DataSet, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        SyncLock _RetrieveChiroProcedureValuesSyncLock

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_ALL_CHIRO_PROCEDURE_CODES" & ".xml"
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim StreamWriter As System.IO.StreamWriter
            Dim XMLSerial As XmlSerializer
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBMD.RETRIEVE_ALL_CHIRO_PROCEDURE_CODES"

            Try

                If ds Is Nothing Then
                    DB = CMSDALCommon.CreateDatabase()
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
                    If ds Is Nothing Then
                        If transaction Is Nothing Then
                            ds = DB.ExecuteDataSet(DBCommandWrapper)
                        Else
                            ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                        End If
                        ds.Tables(0).TableName = "CHIRO_PROCEDURE_CODES"
                    Else
                        If transaction Is Nothing Then
                            DB.LoadDataSet(DBCommandWrapper, ds, "CHIRO_PROCEDURE_CODES")
                        Else
                            DB.LoadDataSet(DBCommandWrapper, ds, "CHIRO_PROCEDURE_CODES", transaction)
                        End If
                    End If

                    If File.Exists(XMLFilename) Then File.SetAttributes(XMLFilename, FileAttributes.Normal)

                    StreamWriter = New System.IO.StreamWriter(XMLFilename)
                    XMLSerial = New XmlSerializer(ds.GetType())
                    XMLSerial.Serialize(StreamWriter, ds)
                    StreamWriter.Close()

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)

                    Return ds

                Else
                    Return ds
                End If

            Catch ex As Exception
                Throw

            Finally

                If StreamWriter IsNot Nothing Then StreamWriter.Close()
                StreamWriter = Nothing
                'mutexFile.ReleaseMutex()

            End Try
        End SyncLock

    End Function

    Public Shared Function RetrievePWOExclusionProcedureValues() As List(Of String)

        If _PWOExclusionProcedureValuesDS Is Nothing Then
            _PWOExclusionProcedureValuesDS = RetrievePWOExclusionProcedureValues(_PWOExclusionProcedureValuesDS)
        End If

        Dim PWOExclusionList As New List(Of String)

        For Each PWOExclusionRow As DataRowView In _PWOExclusionProcedureValuesDS.Tables("PWOEXCLUSION_PROCEDURE_CODES").DefaultView
            PWOExclusionList.Add(PWOExclusionRow("PROC_CODE").ToString)
        Next

        Return PWOExclusionList

    End Function

    Private Shared Function RetrievePWOExclusionProcedureValues(ByVal ds As DataSet, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        SyncLock _RetrievePWOExclusionProcedureValuesSyncLock

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_ALL_PWO_EXCLUSION_PROCEDURE_CODES" & ".xml"
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim StreamWriter As System.IO.StreamWriter
            Dim Serializer As XmlSerializer
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBMD.RETRIEVE_ALL_PWO_EXCLUSION_PROCEDURE_CODES"

            Try
                'The Count is as expensive as the retreive so it was removed to eliminate the double dipping
                If ds Is Nothing Then
                    DB = CMSDALCommon.CreateDatabase()
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
                    If ds Is Nothing Then
                        If transaction Is Nothing Then
                            ds = DB.ExecuteDataSet(DBCommandWrapper)
                        Else
                            ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                        End If
                        ds.Tables(0).TableName = "PWOEXCLUSION_PROCEDURE_CODES"
                    Else
                        If transaction Is Nothing Then
                            DB.LoadDataSet(DBCommandWrapper, ds, "PWOEXCLUSION_PROCEDURE_CODES")
                        Else
                            DB.LoadDataSet(DBCommandWrapper, ds, "PWOEXCLUSION_PROCEDURE_CODES", transaction)
                        End If
                    End If

                    If File.Exists(XMLFilename) Then File.SetAttributes(XMLFilename, FileAttributes.Normal)

                    StreamWriter = New System.IO.StreamWriter(XMLFilename)
                    Serializer = New XmlSerializer(ds.GetType())
                    Serializer.Serialize(StreamWriter, ds)
                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                    Return ds
                Else
                    Return ds
                End If

            Catch ex As Exception
                Throw

            Finally
                If StreamWriter IsNot Nothing Then StreamWriter.Close()
                StreamWriter = Nothing

            End Try
        End SyncLock

    End Function
    Public Shared Function RetrievePreventativeProcedureValues() As String()

        If _PreventativeProcedureValuesDS Is Nothing Then
            _PreventativeProcedureValuesDS = RetrievePreventativeProcedureValues(_PreventativeProcedureValuesDS)
        End If

        Dim PreventativeList As New ArrayList(_PreventativeProcedureValuesDS.Tables("PREVENTATIVE_PROCEDURE_CODES").Rows.Count)

        For Each PreventativeRow As DataRowView In _PreventativeProcedureValuesDS.Tables("PREVENTATIVE_PROCEDURE_CODES").DefaultView
            PreventativeList.Add(PreventativeRow("PROC_CODE"))
        Next

        Return CType(PreventativeList.ToArray(GetType(String)), String())

    End Function

    Private Shared Function RetrievePreventativeProcedureValues(ByVal ds As DataSet, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        SyncLock _RetrievePreventativeProcedureValuesSyncLock

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_ALL_PREVENTATIVE_PROCEDURE_CODES" & ".xml"
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBMD.RETRIEVE_ALL_PREVENTATIVE_PROCEDURE_CODES"
            Dim SWriter As System.IO.StreamWriter
            Dim XMLSerial As XmlSerializer

            Try
                If ds Is Nothing Then
                    ds = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.PUBLISH_HISTORY", "PUBLISH_DATE", "FDBMD.RETRIEVE_ALL_PREVENTATIVE_PROCEDURE_CODES", True)
                    If ds.Tables.Count = 0 Then
                        DB = CMSDALCommon.CreateDatabase()
                        DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
                        If ds Is Nothing Then
                            If transaction Is Nothing Then
                                ds = DB.ExecuteDataSet(DBCommandWrapper)
                            Else
                                ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                            End If
                        Else
                            If transaction Is Nothing Then
                                DB.LoadDataSet(DBCommandWrapper, ds, "PREVENTATIVE_PROCEDURE_CODES")
                            Else
                                DB.LoadDataSet(DBCommandWrapper, ds, "PREVENTATIVE_PROCEDURE_CODES", transaction)
                            End If
                        End If
                        SWriter = New System.IO.StreamWriter(XMLFilename)
                        XMLSerial = New XmlSerializer(ds.GetType())
                        XMLSerial.Serialize(SWriter, ds)
                        File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                    End If
                    Return ds
                Else
                    Return ds
                End If

            Catch ex As Exception
                Throw
            Finally

                If SWriter IsNot Nothing Then SWriter.Close()
                SWriter = Nothing

            End Try
        End SyncLock

    End Function
    Public Shared Function RetrieveBillTypeValues(ByVal effectiveDate As Date?, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim CloneDS As DataSet

        Try

            If _BillTypeDS Is Nothing Then
                _BillTypeDS = RetrieveBillTypeValues(ds, transaction)
            End If

            CloneDS = _BillTypeDS.Clone

            If effectiveDate Is Nothing OrElse IsDBNull(effectiveDate) Then
                effectiveDate = UFCWGeneral.NowDate
            End If

            _BillTypeDS.Tables(0).DefaultView.Sort = "BILL_TYPE_VALUE"
            _BillTypeDS.Tables(0).DefaultView.RowFilter = "#" & Format(effectiveDate, "yyyy-MM-dd").ToString & "# >= FROM_DATE AND #" & Format(effectiveDate, "yyyy-MM-dd").ToString & "# <= THRU_DATE "

            For Each BillTypeRow As DataRowView In _BillTypeDS.Tables(0).DefaultView
                CloneDS.Tables(0).LoadDataRow(BillTypeRow.Row.ItemArray, True)
            Next

            If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                ds.Merge(CloneDS)
                Return ds
            Else
                Return CloneDS
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Shared Function RetrieveBillTypeValues(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        SyncLock _RetrieveBillTypeValuesSyncLock

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_ALL_BILL_TYPE_VALUES" & ".xml"
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim FStream As FileStream
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBMD.RETRIEVE_ALL_BILL_TYPE_VALUES"
            Dim XMLSerial As XmlSerializer

            Try

                If _BillTypeDS Is Nothing Then
                    ds = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.BILL_TYPE_VALUES", "LASTUPDT", "FDBMD.RETRIEVE_ALL_BILL_TYPE_VALUES")
                    If ds.Tables.Count = 0 Then

                        DB = CMSDALCommon.CreateDatabase()
                        DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                        If ds Is Nothing Then
                            If transaction Is Nothing Then
                                ds = DB.ExecuteDataSet(DBCommandWrapper)
                            Else
                                ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                            End If
                        Else
                            If transaction Is Nothing Then
                                DB.LoadDataSet(DBCommandWrapper, ds, "BILL_TYPE_VALUES")
                            Else
                                DB.LoadDataSet(DBCommandWrapper, ds, "BILL_TYPE_VALUES", transaction)
                            End If
                        End If

                        _BillTypeDS = ds

                        FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                        XMLSerial = New XmlSerializer(ds.GetType())
                        XMLSerial.Serialize(FStream, ds)
                        File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                    Else
                        _BillTypeDS = ds
                    End If
                Else
                    ds = _BillTypeDS
                End If

                Return ds

            Catch ex As Exception
                Throw
            Finally

                If FStream IsNot Nothing Then FStream.Close()
                FStream = Nothing

            End Try
        End SyncLock

    End Function


    Public Shared Function RetrieveRuleSetTypesValues(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        If _RuleSetTypesTotalDS Is Nothing Then
            _RuleSetTypesTotalDS = LoadRuleSetTypesValues(ds, transaction)
        End If

        Return _RuleSetTypesTotalDS

    End Function

    Private Shared Function LoadRuleSetTypesValues(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        SyncLock _RuleSetTypeslockObj

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_RULE_SET_TYPES" & ".xml"
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim FStream As FileStream
            Dim XMLDS As DataSet
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBMD.RETRIEVE_RULE_SET_TYPES"
            Dim XMLSerial As XmlSerializer

            Try
                XMLDS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.RULE_SET_TYPES", "LASTUPDT", "FDBMD.RETRIEVE_RULE_SET_TYPES")
                If XMLDS.Tables.Count = 0 Then
                    DB = CMSDALCommon.CreateDatabase()
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
                    DBCommandWrapper.CommandTimeout = 120

                    If XMLDS Is Nothing Then
                        If transaction Is Nothing Then
                            XMLDS = DB.ExecuteDataSet(DBCommandWrapper)
                        Else
                            XMLDS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                        End If
                    Else
                        If transaction Is Nothing Then
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "RULE_SET_TYPES")
                        Else
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "RULE_SET_TYPES", transaction)
                        End If
                    End If

                    _RuleSetTypesTotalDS = XMLDS

                    FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)

                    XMLSerial = New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(FStream, XMLDS)


                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _RuleSetTypesTotalDS = XMLDS
                End If

                If ds IsNot Nothing Then
                    ds.Merge(_RuleSetTypesTotalDS)
                    Return ds
                Else
                    Return _RuleSetTypesTotalDS
                End If

            Catch ex As Exception
                Throw
            Finally
                If FStream IsNot Nothing Then FStream.Close()
                FStream = Nothing

            End Try
        End SyncLock

    End Function

    Public Shared Function RetrieveDiseaseManagement(ByVal familyID As Integer, ByRef ds As DataSet, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_DM_BY_FAMILYID"

        Dim Tablenames() As String = {"DM_CANDIDATES", "ALL_DM_INCENTIVE", "DM_DISINCENTIVE_DNP", "UFCWLETTER", "REG_ALERTS"}

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, Tablenames)
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, Tablenames, transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Function

    Public Shared Function RetrieveDocType(ByVal docClass As String, ByVal docType As String) As DataRow
        Dim DocTypesDT As DataTable
        Dim CloneDT As DataTable

        Try

            DocTypesDT = LoadDocTypes()

            CloneDT = DocTypesDT.Clone

            DocTypesDT.DefaultView.Sort = ""
            DocTypesDT.DefaultView.RowFilter = "DOC_CLASS = '" & docClass.ToString.Trim & "' AND DOC_TYPE = '" & docType.ToString.Trim & "'"

            For Each DocTypeRow As DataRowView In DocTypesDT.DefaultView
                CloneDT.LoadDataRow(DocTypeRow.Row.ItemArray, True)
            Next

            Return CloneDT.Rows(0)

        Catch ex As Exception

            Throw

        End Try

    End Function

    Public Shared Function RetrieveActiveDocTypes() As DataTable

        Dim DocTypesDT As DataTable
        Dim CloneDT As DataTable

        Try

            DocTypesDT = LoadDocTypes()

            CloneDT = DocTypesDT.Clone

            DocTypesDT.DefaultView.Sort = "DOC_TYPE"
            DocTypesDT.DefaultView.RowFilter = "ACTIVE_SW = 1"

            For Each DocTypeRow As DataRowView In DocTypesDT.DefaultView
                CloneDT.LoadDataRow(DocTypeRow.Row.ItemArray, True)
            Next

            Return CloneDT

        Catch ex As Exception

            Throw

        End Try

    End Function

    Private Shared Function LoadDocTypes() As DataTable

        SyncLock _DocTypelockObj

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_ALL_DOC_TYPES" & ".xml"
            Dim DT As DataTable
            Dim DS As DataSet
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim FileStream As FileStream
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBMD.RETRIEVE_ALL_DOC_TYPES"

            Try

                DT = New DataTable("DOC_TYPE")

                If _DocTypeDS Is Nothing Then
                    DS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.DOC_TYPE", "LASTUPDT", "FDBMD.RETRIEVE_ALL_DOC_TYPES")
                    If DS.Tables.Count = 0 Then
                        DB = CMSDALCommon.CreateDatabase()
                        DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                        If DS Is Nothing Then
                            DS = DB.ExecuteDataSet(DBCommandWrapper)
                        Else
                            DB.LoadDataSet(DBCommandWrapper, DS, "DOC_TYPE")
                        End If

                        _DocTypeDS = DS

                        FileStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                        Dim XMLSerial As New XmlSerializer(DS.GetType())
                        XMLSerial.Serialize(FileStream, DS)

                        File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                    Else
                        _DocTypeDS = DS
                    End If
                Else
                    DS = _DocTypeDS
                End If

                If DS.Tables.Count > 0 Then
                    DT = DS.Tables(0)
                End If

                Return DT

            Catch ex As Exception

                Throw

            Finally

                If FileStream IsNot Nothing Then FileStream.Close()
                FileStream = Nothing

                If DT IsNot Nothing Then DT.Dispose()
                DT = Nothing
            End Try
        End SyncLock


    End Function

    Public Shared Function RetrieveUserDocTypes(ByVal userName As String) As DataTable

        Return RetrieveUserDocTypes(userName, "")

    End Function

    Public Shared Function ValidateUserDocTypes(ByVal userName As String, ByVal docType As String) As Boolean

        Dim DV As DataView

        Try

            DV = New DataView(RetrieveUserDocTypes(userName, ""), "DOC_TYPE = '" & docType & "'", "", DataViewRowState.CurrentRows)

            If DV.Count > 0 Then Return True

            Return False

        Catch ex As Exception
            Throw
        Finally
            If DV IsNot Nothing Then DV.Dispose()

        End Try

    End Function

    Public Shared Function RetrieveUserDocTypes(ByVal userName As String, ByVal mode As String) As DataTable

        Dim UsersDS As DataSet
        Dim CloneDT As DataTable

        UsersDS = RetrieveUserDocTypes(UsersDS)

        CloneDT = UsersDS.Tables("USER_DOC_TYPES").Clone

        UsersDS.Tables("USER_DOC_TYPES").DefaultView.Sort = "DOC_TYPE"

        If mode IsNot Nothing Then
            Select Case mode
                Case "AUDIT"
                    UsersDS.Tables("USER_DOC_TYPES").DefaultView.RowFilter = "USERID = '" & userName & "' AND AUDIT_SW = 1 AND ACTIVE_SW = 1"
                Case "BOTH"
                    UsersDS.Tables("USER_DOC_TYPES").DefaultView.RowFilter = "USERID = '" & userName & "' AND (REGULAR_SW = 1 OR AUDIT_SW = 1) AND ACTIVE_SW = 1"
                Case Else ' REGULAR
                    UsersDS.Tables("USER_DOC_TYPES").DefaultView.RowFilter = "USERID = '" & userName & "' AND REGULAR_SW = 1 AND ACTIVE_SW = 1"
            End Select
        End If

        For Each DocTypeRow As DataRowView In UsersDS.Tables("USER_DOC_TYPES").DefaultView
            CloneDT.LoadDataRow(DocTypeRow.Row.ItemArray, True)
        Next

        Return CloneDT

    End Function
    Public Shared Function RetrieveDistinctUsers(Optional ByVal addAll As Boolean = True) As DataTable

        Return RetrieveDistinctUsers(Nothing, addAll)

    End Function

    Public Shared Function RetrieveDistinctUsers(ByVal active As Object, ByVal addAll As Boolean) As DataTable

        Dim UsersDS As DataSet
        Dim CloneDT As DataTable

        Try

            UsersDS = RetrieveUserDocTypes(UsersDS)

            CloneDT = UsersDS.Tables("USER_DOC_TYPES").Clone

            UsersDS.Tables("USER_DOC_TYPES").DefaultView.Sort = ""

            If active IsNot Nothing Then
                UsersDS.Tables("USER_DOC_TYPES").DefaultView.RowFilter = "ACTIVE_SW = " & active.ToString
            Else
                UsersDS.Tables("USER_DOC_TYPES").DefaultView.RowFilter = "" ' note this is necessary to cancel any prior uses of the underlying dataset
            End If

            For Each DocTypeRow As DataRowView In UsersDS.Tables("USER_DOC_TYPES").DefaultView
                CloneDT.LoadDataRow(DocTypeRow.Row.ItemArray, True)
            Next

            If UFCWGeneralAD.CMSCanAudit Then
                CloneDT.LoadDataRow(New Object() {"INSTANTIATOR%", "MEDICAL", "", 1, 1, 1}, True)
                CloneDT.LoadDataRow(New Object() {"AUTO ADJUDICATOR", "MEDICAL", "", 1, 1, 1}, True)
            End If

            Return CMSDALCommon.SelectDistinctAndSorted("", CloneDT, "USERID", addAll)

        Catch ex As Exception

            Throw

        End Try


    End Function

    Private Shared Function RetrieveUserDocTypes(Optional ByVal ds As DataSet = Nothing) As DataSet

        SyncLock _RetrieveUserDocTypesSyncLock

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_ALL_USER_DOC_TYPES_NOUSERFILTER" & ".xml"
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim FStream As FileStream
            Dim XMLSerial As XmlSerializer
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBMD.RETRIEVE_ALL_USER_DOC_TYPES_NOUSERFILTER"
            Dim XMLDS As DataSet

            Try

                If _UserDocTypeDS Is Nothing Then
                    XMLDS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.USER_DOC_TYPES", "LASTUPDT", "FDBMD.RETRIEVE_ALL_USER_DOC_TYPES_NOUSERFILTER", True)
                    If XMLDS.Tables.Count = 0 Then
                        DB = CMSDALCommon.CreateDatabase()
                        DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                        DB.LoadDataSet(DBCommandWrapper, XMLDS, "USER_DOC_TYPES")

                        _UserDocTypeDS = XMLDS
                        FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)


                        Try

                            XMLSerial = New XmlSerializer(XMLDS.GetType())
                            XMLSerial.Serialize(FStream, XMLDS)
                        Catch ex As Exception
                            Throw
                        End Try

                        File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                    Else
                        _UserDocTypeDS = XMLDS
                    End If

                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_UserDocTypeDS)
                    Return ds
                Else
                    Return _UserDocTypeDS
                End If

            Catch ex As Exception
                Throw

            Finally

                XMLSerial = Nothing
                If FStream IsNot Nothing Then
                    FStream.Close()
                End If

                FStream = Nothing

            End Try
        End SyncLock


    End Function

    Public Shared Function RetrievePlans() As DataTable
        Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
        Dim SWriter As System.IO.StreamWriter
        Dim XMLSerial As XmlSerializer
        Dim XMLFilename As String
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_PLANS"

        Try
            If _PlansDS Is Nothing Then

                XMLFilename = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_PLANS" & ".xml"
                _PlansDS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.PLANS", "LASTUPDT", "FDBMD.RETRIEVE_PLANS")

                If _PlansDS.Tables.Count = 0 Then
                    DB = CMSDALCommon.CreateDatabase()
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                    _PlansDS = DB.ExecuteDataSet(DBCommandWrapper)

                    SWriter = New System.IO.StreamWriter(XMLFilename)
                    XMLSerial = New XmlSerializer(_PlansDS.GetType())
                    XMLSerial.Serialize(SWriter, _PlansDS)
                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                End If
            End If

            If _PlansDS.Tables.Count > 0 Then
                _PlansDS.Tables(0).Rows.Add(_PlansDS.Tables(0).NewRow)
                Return _PlansDS.Tables(0)
            End If

        Catch ex As Exception
            Throw
        Finally
            If SWriter IsNot Nothing Then SWriter.Close()
            SWriter = Nothing

        End Try
    End Function

    Public Shared Function RetrieveRelationShipValues() As DataTable
        Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
        Dim XMLSerial As XmlSerializer
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBEL.RETRIEVE_ALL_RELATIONSHIP_VALUES"
        Dim XMLFilename As String

        Try
            If _RelationshipValuesDS Is Nothing Then

                XMLFilename = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBEL.RETRIEVE_ALL_RELATIONSHIP_VALUES" & ".xml"
                _RelationshipValuesDS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBEL.RELATIONSHIP_VALUES", "LASTUPDT", "FDBEL.RETRIEVE_ALL_RELATIONSHIP_VALUES")

                If _RelationshipValuesDS.Tables.Count = 0 Then
                    DB = CMSDALCommon.CreateDatabase()
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                    _RelationshipValuesDS = DB.ExecuteDataSet(DBCommandWrapper)

                    Using StreamWriter As New System.IO.StreamWriter(XMLFilename)

                        XMLSerial = New XmlSerializer(_RelationshipValuesDS.GetType())
                        XMLSerial.Serialize(StreamWriter, _RelationshipValuesDS)
                        StreamWriter.Close()

                    End Using
                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                End If
            End If

            If _RelationshipValuesDS.Tables.Count > 0 Then
                Return _RelationshipValuesDS.Tables(0)
            End If

        Catch ex As Exception
            Throw
        Finally

        End Try
    End Function

    Public Shared Function RetrieveCOBs() As DataTable

        Dim DT As DataTable

        Try

            If _COBValuesDS Is Nothing Then
                _COBValuesDS = LoadCOBs()
                _COBValuesDS.Tables(0).Rows.Add(_COBValuesDS.Tables(0).NewRow)
            End If

            If _COBValuesDS.Tables.Count > 0 Then
                _COBValuesDS.Tables(0).TableName = "COB_VALUES"
                DT = _COBValuesDS.Tables(0)
            End If

            Return DT
        Catch ex As Exception
            Throw
        Finally
            If DT IsNot Nothing Then DT.Dispose()

        End Try

    End Function


    Private Shared Function LoadCOBs(Optional ByVal ds As DataSet = Nothing) As DataSet

        SyncLock _COBValueslockObj

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_COB_VALUES" & ".xml"
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim FileStream As FileStream
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBMD.RETRIEVE_COB_VALUES"
            Dim XMLDS As DataSet

            Try

                XMLDS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.COB_VALUES", "LASTUPDT", "FDBMD.RETRIEVE_COB_VALUES")
                If XMLDS.Tables.Count = 0 Then
                    DB = CMSDALCommon.CreateDatabase()
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        XMLDS = DB.ExecuteDataSet(DBCommandWrapper)
                    Else
                        DB.LoadDataSet(DBCommandWrapper, XMLDS, "COB_VALUES")
                    End If

                    _COBValuesDS = XMLDS

                    FileStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                    Dim XMLSerial As New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(FileStream, XMLDS)

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _COBValuesDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_COBValuesDS)
                    Return ds
                Else
                    Return _COBValuesDS
                End If

            Catch ex As Exception
                Throw
            Finally

                If FileStream IsNot Nothing Then FileStream.Close()
                FileStream = Nothing

            End Try
        End SyncLock

    End Function

    Public Shared Function RetrievePPOs() As DataTable

        Dim DT As DataTable

        Try

            If _PPOValuesDS Is Nothing Then
                _PPOValuesDS = LoadPPOs()
                _PPOValuesDS.Tables(0).Rows.Add(_PPOValuesDS.Tables(0).NewRow)
            End If

            If _PPOValuesDS.Tables.Count > 0 Then
                _PPOValuesDS.Tables(0).TableName = "PPO_VALUES"
                DT = _PPOValuesDS.Tables(0)
            End If

            Return DT

        Catch ex As Exception
            Throw
        Finally
            If DT IsNot Nothing Then DT.Dispose()
        End Try


    End Function

    Private Shared Function LoadPPOs(Optional ByVal ds As DataSet = Nothing) As DataSet

        SyncLock _PPOValueslockObj

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_PPO_VALUES" & ".xml"
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim FStream As FileStream
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBMD.RETRIEVE_PPO_VALUES"
            Dim XMLDS As DataSet
            Dim XMLSerial As XmlSerializer

            Try

                XMLDS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.PPO_VALUES", "LASTUPDT", "FDBMD.RETRIEVE_PPO_VALUES")
                If XMLDS.Tables.Count = 0 Then

                    DB = CMSDALCommon.CreateDatabase()
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        XMLDS = DB.ExecuteDataSet(DBCommandWrapper)
                    Else
                        DB.LoadDataSet(DBCommandWrapper, XMLDS, "PPO_VALUES")
                    End If

                    _PPOValuesDS = XMLDS

                    FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                    XMLSerial = New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(FStream, XMLDS)
                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _PPOValuesDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_PPOValuesDS)
                    Return ds
                End If

                Return _PPOValuesDS

            Catch ex As Exception
                Throw
            Finally
                If FStream IsNot Nothing Then
                    FStream.Close()
                End If

                FStream = Nothing

            End Try
        End SyncLock

    End Function

    Public Shared Function RetrievePARs() As DataTable

        Dim DT As DataTable

        Try

            If _PARValuesDS Is Nothing Then
                _PARValuesDS = LoadPARs(_PARValuesDS)
            End If

            DT = New DataTable("PAR_VALUES")
            If _PARValuesDS.Tables.Count > 0 Then
                DT = _PARValuesDS.Tables(0)
            End If

            Return DT
        Catch ex As Exception
            Throw
        Finally
            If DT IsNot Nothing Then DT.Dispose()

        End Try

    End Function

    Private Shared Function LoadPARs(Optional ByVal ds As DataSet = Nothing) As DataSet

        SyncLock _ParValueslockObj

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_PAR_VALUES" & ".xml"
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim FStream As FileStream
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBMD.RETRIEVE_PAR_VALUES"
            Dim XMLDS As DataSet

            Try

                XMLDS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.PAR_VALUES", "LASTUPDT", "FDBMD.RETRIEVE_PAR_VALUES")
                If XMLDS.Tables.Count = 0 Then

                    DB = CMSDALCommon.CreateDatabase()
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        XMLDS = DB.ExecuteDataSet(DBCommandWrapper)
                    Else
                        DB.LoadDataSet(DBCommandWrapper, XMLDS, "PAR_VALUES")
                    End If

                    _PARValuesDS = XMLDS

                    FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                    Dim XMLSerial As New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(FStream, XMLDS)

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _PARValuesDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_PARValuesDS)
                    Return ds
                Else
                    Return _PARValuesDS
                End If

            Catch ex As Exception
                Throw
            Finally

                If FStream IsNot Nothing Then
                    FStream.Close()
                End If

                FStream = Nothing
            End Try
        End SyncLock

    End Function

    Public Shared Function RetrievePayees() As DataTable

        Dim DT As DataTable

        Try

            If _PayeeValuesDS Is Nothing Then
                _PayeeValuesDS = LoadPayees(_PayeeValuesDS)
                _PayeeValuesDS.Tables(0).Rows.Add(_PayeeValuesDS.Tables(0).NewRow) 'add blank line 
            End If

            If _PayeeValuesDS.Tables.Count > 0 Then
                _PayeeValuesDS.Tables(0).TableName = "PAR_VALUES"
                DT = _PayeeValuesDS.Tables(0)
            End If

            Return DT

        Catch ex As Exception
            Throw
        Finally
            If DT IsNot Nothing Then DT.Dispose()
        End Try

    End Function

    Private Shared Function LoadPayees(Optional ByVal ds As DataSet = Nothing) As DataSet

        SyncLock _ParValueslockObj

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_PAYEE_VALUES" & ".xml"
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim FStream As FileStream
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBMD.RETRIEVE_PAYEE_VALUES"
            Dim XMLDS As DataSet
            Dim XMLSerial As XmlSerializer

            Try

                XMLDS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.PAYEE_VALUES", "LASTUPDT", "FDBMD.RETRIEVE_PAYEE_VALUES")
                If XMLDS.Tables.Count = 0 Then
                    DB = CMSDALCommon.CreateDatabase()
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        XMLDS = DB.ExecuteDataSet(DBCommandWrapper)
                    Else
                        DB.LoadDataSet(DBCommandWrapper, XMLDS, "PAYEE_VALUES")
                    End If

                    _PayeeValuesDS = XMLDS

                    FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                    XMLSerial = New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(FStream, XMLDS)

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _PayeeValuesDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_PayeeValuesDS)
                    Return ds
                End If

                Return _PayeeValuesDS

            Catch ex As Exception
                Throw
            Finally
                If FStream IsNot Nothing Then
                    FStream.Close()
                End If

                FStream = Nothing

            End Try
        End SyncLock
    End Function

    Public Shared Function RetrievePayers(ByVal ds As DataSet) As DataSet

        Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_ALL_PAYERS" & ".xml"
        Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()

        Dim XMLDS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_ALL_PAYERS"
        Dim XMLSerial As XmlSerializer
        Dim PopulateXMLTask As Task

        Try

            If _PayerValuesDS Is Nothing Then
                XMLDS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.PAYER_VALUES", "LASTUPDT", "FDBMD.RETRIEVE_ALL_PAYERS")
                If XMLDS.Tables.Count = 0 Then
                    DB = CMSDALCommon.CreateDatabase()

                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        XMLDS = DB.ExecuteDataSet(DBCommandWrapper)
                    Else
                        DB.LoadDataSet(DBCommandWrapper, XMLDS, "PAYER_VALUES")
                    End If

                    _PayerValuesDS = XMLDS

                    PopulateXMLTask = Task.Factory.StartNew(Sub()
                                                                Using FStream As New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)
                                                                    XMLSerial = New XmlSerializer(XMLDS.GetType())
                                                                    XMLSerial.Serialize(FStream, XMLDS)
                                                                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                                                                    FStream.Close()
                                                                End Using
                                                            End Sub)

                Else
                    _PayerValuesDS = XMLDS
                End If
            End If

            Return _PayerValuesDS

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Function

    Public Shared Function RetrievePayers() As DataSet

        SyncLock _RetrievePayersSyncLock

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_ALL_PAYERS" & ".xml"
            Dim DS As DataSet
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim FStream As FileStream
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBMD.RETRIEVE_ALL_PAYERS"
            Dim XMLSerial As XmlSerializer

            Try

                If _PayerValuesDS Is Nothing Then
                    DS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.PAYER_VALUES", "LASTUPDT", "FDBMD.RETRIEVE_ALL_PAYERS")
                    If DS.Tables.Count = 0 Then
                        DB = CMSDALCommon.CreateDatabase()
                        DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                        If DS Is Nothing Then
                            DS = DB.ExecuteDataSet(DBCommandWrapper)
                        Else
                            DB.LoadDataSet(DBCommandWrapper, DS, "PAYER_VALUES")
                        End If

                        _PayerValuesDS = DS

                        FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                        XMLSerial = New XmlSerializer(DS.GetType())
                        XMLSerial.Serialize(FStream, DS)

                        File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                    Else
                        _PayerValuesDS = DS
                    End If
                Else
                    DS = _PayerValuesDS
                End If

                Return DS

            Catch ex As Exception
                Throw
            Finally

                If FStream IsNot Nothing Then
                    FStream.Close()

                End If

                FStream = Nothing

            End Try
        End SyncLock
    End Function

    Public Shared Function RetrieveReasonHIPAAValuesInformation(ByVal reasonCode As String, nonPAR As Boolean, ooa As Boolean, cxt As Boolean) As DataRow
        Dim SelectedDRs As DataRow()

        Try

            If _ReasonHIPAADS Is Nothing Then
                _ReasonHIPAADS = RetrieveReasonHIPAAValues(_ReasonHIPAADS)
            End If

            SelectedDRs = _ReasonHIPAADS.Tables(0).Select("REASON = '" & reasonCode.ToString.Trim & "'" & If(cxt, " AND CXT_SW = 1", " AND CXT_SW = 0") & If(nonPAR, " AND PAR_SW = 0", " AND PAR_SW = 1") & If(nonPAR, " AND NON_PAR_SW = 1", " AND NON_PAR_SW = 0") & If(ooa, " AND OUT_OF_AREA_SW = 1", " AND OUT_OF_AREA_SW = 0"), "")

            If SelectedDRs.Length > 0 Then
                Return SelectedDRs(0)
            Else 'default item for UFCW Reason Code
                SelectedDRs = _ReasonHIPAADS.Tables(0).Select("REASON = '" & reasonCode.ToString.Trim & "'" & " AND CXT_SW = 0" & " AND PAR_SW = 0" & " AND NON_PAR_SW = 0" & " AND OUT_OF_AREA_SW =  0")
                If SelectedDRs.Length > 0 Then
                    Return SelectedDRs(0)
                End If
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveReasonHIPAAValues(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_ALL_REASON_VALUES_XREF_HIPAA" & ".xml"
        Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
        Dim StreamWriter As System.IO.StreamWriter
        Dim Serializer As XmlSerializer
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_ALL_REASON_VALUES_XREF_HIPAA"

        Try
            DB = CMSDALCommon.CreateDatabase()

            ds = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.REASON_VALUES_XREF_HIPAA", "LASTUPDT", "FDBMD.RETRIEVE_ALL_REASON_VALUES_XREF_HIPAA")
            If ds.Tables.Count = 0 Then
                DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
                If ds Is Nothing Then
                    If transaction Is Nothing Then
                        ds = DB.ExecuteDataSet(DBCommandWrapper)
                    Else
                        ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                    End If
                    ds.Tables(0).TableName = "RETRIEVE_ALL_REASON_VALUES_XREF_HIPAA"
                Else
                    If transaction Is Nothing Then
                        DB.LoadDataSet(DBCommandWrapper, ds, "RETRIEVE_ALL_REASON_VALUES_XREF_HIPAA")
                    Else
                        DB.LoadDataSet(DBCommandWrapper, ds, "RETRIEVE_ALL_REASON_VALUES_XREF_HIPAA", transaction)
                    End If
                End If

                StreamWriter = New System.IO.StreamWriter(XMLFilename)
                Serializer = New XmlSerializer(ds.GetType())
                Serializer.Serialize(StreamWriter, ds)
                File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
            End If
            Return ds
        Catch ex As Exception
            Throw
        Finally
            If StreamWriter IsNot Nothing Then StreamWriter.Close()
            StreamWriter = Nothing
        End Try
    End Function

    Public Shared Function RetrieveReasonValuesInformation(ByVal reasonCode As String, Optional ByVal effectiveDate As Date? = Nothing) As DataRow
        Dim SelectedDRs As DataRow()

        Try

            If _ReasonDS Is Nothing Then
                _ReasonDS = RetrieveReasonValues(Nothing, _ReasonDS)
            End If

            SelectedDRs = _ReasonDS.Tables(0).Select("REASON = '" & reasonCode.ToString.Trim & "'" & If(effectiveDate IsNot Nothing, " AND #" & Format(effectiveDate, "yyyy-MM-dd").ToString & "# >= FROM_DATE AND #" & Format(effectiveDate, "yyyy-MM-dd").ToString & "# <= THRU_DATE ", ""))

            If SelectedDRs.Length > 0 Then
                Return SelectedDRs(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveReasonValues(ByVal effectiveDate As Date?, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim CloneDS As DataSet

        Try

            If _ReasonDS Is Nothing Then
                _ReasonDS = RetrieveReasonValues(ds, transaction)
            End If

            CloneDS = _ReasonDS.Clone

            If effectiveDate Is Nothing OrElse IsDBNull(effectiveDate) Then
                effectiveDate = UFCWGeneral.NowDate
            End If

            _ReasonDS.Tables(0).DefaultView.Sort = "REASON"
            _ReasonDS.Tables(0).DefaultView.RowFilter = "#" & Format(effectiveDate, "yyyy-MM-dd").ToString & "# >= FROM_DATE AND #" & Format(effectiveDate, "yyyy-MM-dd").ToString & "# <= THRU_DATE "

            For Each ReasonRow As DataRowView In _ReasonDS.Tables(0).DefaultView
                CloneDS.Tables(0).LoadDataRow(ReasonRow.Row.ItemArray, True)
            Next

            If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                ds.Merge(CloneDS)
                Return ds
            Else
                Return CloneDS
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveReasonValues(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_ALL_REASON_VALUES" & ".xml"
        Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
        Dim SWriter As System.IO.StreamWriter
        Dim XMLSerial As XmlSerializer
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_ALL_REASON_VALUES"

        Try
            ds = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.REASON_VALUES", "LASTUPDT", "FDBMD.RETRIEVE_ALL_REASON_VALUES")
            If ds.Tables.Count = 0 Then
                DB = CMSDALCommon.CreateDatabase()
                DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                If ds Is Nothing Then
                    If transaction Is Nothing Then
                        ds = DB.ExecuteDataSet(DBCommandWrapper)
                    Else
                        ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                    End If
                Else
                    If transaction Is Nothing Then
                        DB.LoadDataSet(DBCommandWrapper, ds, "REASON_VALUES")
                    Else
                        DB.LoadDataSet(DBCommandWrapper, ds, "REASON_VALUES", transaction)
                    End If
                End If

                SWriter = New System.IO.StreamWriter(XMLFilename)
                XMLSerial = New XmlSerializer(ds.GetType())
                XMLSerial.Serialize(SWriter, ds)
                File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
            End If
            Return ds

        Catch ex As Exception
            Throw
        Finally
            If SWriter IsNot Nothing Then SWriter.Close()
            SWriter = Nothing

        End Try
    End Function

    Public Shared Function RetrieveBCSuspendDenyCodeByCode(ByVal code As String, Optional ByVal transaction As DbTransaction = Nothing) As DataRow

        Dim SelectedDRs As DataRow()

        Try

            If _BCSuspendDenyCodesDS Is Nothing Then
                _BCSuspendDenyCodesDS = RetrieveBCSuspendDenyCodes(_BCSuspendDenyCodesDS, transaction)
            End If

            SelectedDRs = _BCSuspendDenyCodesDS.Tables(0).Select("CODE = '" & code.ToString.Trim & "'")

            If SelectedDRs.Length > 0 Then
                Return SelectedDRs(0)
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveBCReason(ByVal errorCode As String, Optional ByVal transaction As DbTransaction = Nothing) As DataRow

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_BC_REASON_CODES"
        Dim SelectedDRs As DataRow()


        Try

            If _BCReasonCodesDS Is Nothing Then
                DB = CMSDALCommon.CreateDatabase()
                DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                If transaction Is Nothing Then
                    _BCReasonCodesDS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    _BCReasonCodesDS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If

            End If

            SelectedDRs = _BCReasonCodesDS.Tables(0).Select(" Code = '" & errorCode.ToString & "' ")

            Return SelectedDRs(0)

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveBCReasonByParNonParIndicator(ByVal errorCode As String, ByVal parNonParIndicator As String, Optional ByVal transaction As DbTransaction = Nothing) As DataRow

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_BC_REASON_CODES"
        Dim SelectedDRs As DataRow()


        Try

            If _BCReasonCodesDS Is Nothing Then
                DB = CMSDALCommon.CreateDatabase()
                DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                If transaction Is Nothing Then
                    _BCReasonCodesDS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    _BCReasonCodesDS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If

            End If

            SelectedDRs = _BCReasonCodesDS.Tables(0).Select(" Code = '" & errorCode.ToString & "' AND (PNO_OUT_OF_AREA_SW IS NULL OR (PNO_OUT_OF_AREA_SW = " & If(parNonParIndicator = "O", 1, 0) & " AND PNO_NON_PAR_SW = " & If(parNonParIndicator = "N", 1, 0) & "))")

            Return SelectedDRs(0)

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveBCDetailReasonCodeByCodeAndParNonParIndicator(ByVal reasonCode As String, ByVal parNonParIndicator As String, Optional ByVal transaction As DbTransaction = Nothing) As DataRow

        Dim SelectedDRs As DataRow()

        Try

            If _BCDetailReasonCodesDS Is Nothing Then
                _BCDetailReasonCodesDS = RetrieveBCDetailReasonCodes(_BCDetailReasonCodesDS, transaction)
            End If

            SelectedDRs = _BCDetailReasonCodesDS.Tables(0).Select("CODE = '" & reasonCode.ToString.Trim & "' AND (ISNULL(APPLY_TO,'') = '' OR " & If(parNonParIndicator = "P", " APPLY_TO = 'P'", " APPLY_TO <> 'P'") & ")")

            If SelectedDRs.Length > 0 Then
                Return SelectedDRs(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveBCDetailReasonCodeByCode(ByVal reasonCode As String, Optional ByVal transaction As DbTransaction = Nothing) As DataRow

        Dim SelectedDRs As DataRow()

        Try

            If _BCDetailReasonCodesDS Is Nothing Then
                _BCDetailReasonCodesDS = RetrieveBCDetailReasonCodes(_BCDetailReasonCodesDS, transaction)
            End If

            SelectedDRs = _BCDetailReasonCodesDS.Tables(0).Select("CODE = '" & reasonCode.ToString.Trim & "'")

            If SelectedDRs.Length > 0 Then
                Return SelectedDRs(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveBCDetailReasonCodeByDescription(ByVal reasonCodeDescription As String, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim CloneDS As DataSet

        Try

            If _BCDetailReasonCodesDS Is Nothing Then
                _BCDetailReasonCodesDS = RetrieveBCDetailReasonCodes(ds, transaction)
            End If

            CloneDS = _BCDetailReasonCodesDS.Clone

            _BCDetailReasonCodesDS.Tables(0).DefaultView.RowFilter = "DESCRIPTION = '" & reasonCodeDescription.ToString.Trim & "'"

            For Each ReasonRow As DataRowView In _BCDetailReasonCodesDS.Tables(0).DefaultView
                CloneDS.Tables(0).LoadDataRow(ReasonRow.Row.ItemArray, True)
            Next

            If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                ds.Merge(CloneDS)
                Return ds
            Else
                Return CloneDS
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Shared Function RetrieveBCDetailReasonCodes(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_ALL_BC_REASON_CODES" & ".xml"
        Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
        Dim StreamWriter As System.IO.StreamWriter
        Dim XMLSerial As XmlSerializer
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_ALL_BC_REASON_CODES"

        Try
            ds = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.BC_DETAIL_REASON_CODES", "LASTUPDT", "FDBMD.RETRIEVE_ALL_BC_REASON_CODES")
            If ds.Tables.Count = 0 Then
                DB = CMSDALCommon.CreateDatabase()
                DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
                If ds Is Nothing Then
                    If transaction Is Nothing Then
                        ds = DB.ExecuteDataSet(DBCommandWrapper)
                    Else
                        ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                    End If
                Else
                    If transaction Is Nothing Then
                        DB.LoadDataSet(DBCommandWrapper, ds, "BC_DETAIL_REASON_CODES")
                    Else
                        DB.LoadDataSet(DBCommandWrapper, ds, "BC_DETAIL_REASON_CODES", transaction)
                    End If
                End If
                StreamWriter = New System.IO.StreamWriter(XMLFilename)
                XMLSerial = New XmlSerializer(ds.GetType())
                XMLSerial.Serialize(StreamWriter, ds)
                File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
            End If
            Return ds
        Catch ex As Exception
            Throw
        Finally
            If StreamWriter IsNot Nothing Then StreamWriter.Close()
            StreamWriter = Nothing

        End Try
    End Function

    Private Shared Function RetrieveBCSuspendDenyCodes(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_ALL_BC_SUSPEND_DENY_CODES" & ".xml"
        Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_ALL_BC_SUSPEND_DENY_CODES"
        Dim SWriter As System.IO.StreamWriter
        Dim XMLSerial As XmlSerializer

        Try
            ds = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.BC_SUSPEND_DENY_CODES", "LASTUPDT", "FDBMD.RETRIEVE_ALL_BC_SUSPEND_DENY_CODES")
            If ds.Tables.Count = 0 Then
                DB = CMSDALCommon.CreateDatabase()
                DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
                If ds Is Nothing Then
                    If transaction Is Nothing Then
                        ds = DB.ExecuteDataSet(DBCommandWrapper)
                    Else
                        ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                    End If
                Else
                    If transaction Is Nothing Then
                        DB.LoadDataSet(DBCommandWrapper, ds, "BC_SUSPEND_DENY_CODES")
                    Else
                        DB.LoadDataSet(DBCommandWrapper, ds, "BC_SUSPEND_DENY_CODES", transaction)
                    End If
                End If
                SWriter = New System.IO.StreamWriter(XMLFilename)
                XMLSerial = New XmlSerializer(ds.GetType())
                XMLSerial.Serialize(SWriter, ds)
                File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
            End If
            Return ds
        Catch ex As Exception
            Throw
        Finally
            If SWriter IsNot Nothing Then SWriter.Close()
            SWriter = Nothing
        End Try
    End Function

    Public Shared Function RetrieveBCDetailErrorByErrorCode(ByVal errorCode As String, Optional ByVal transaction As DbTransaction = Nothing) As DataRow

        Dim SelectedDRs() As DataRow

        Try

            If _BCDetailErrorCodesDS Is Nothing Then
                _BCDetailErrorCodesDS = RetrieveBCDetailErrorCodes(_BCDetailErrorCodesDS, transaction)
            End If

            SelectedDRs = _BCDetailErrorCodesDS.Tables(0).Select("CODE = '" & errorCode.ToString.Trim & "'")

            If SelectedDRs.Length > 0 Then
                Return SelectedDRs(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Shared Function RetrieveBCDetailErrorCodes(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_ALL_BC_DETAIL_ERROR_CODES" & ".xml"
        Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
        Dim SWriter As System.IO.StreamWriter
        Dim XMLSerial As XmlSerializer
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_ALL_BC_DETAIL_ERROR_CODES"

        Try
            ds = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.BC_DETAIL_ERROR_CODES", "LASTUPDT", "FDBMD.RETRIEVE_ALL_BC_DETAIL_ERROR_CODES")
            If ds.Tables.Count = 0 Then
                DB = CMSDALCommon.CreateDatabase()
                DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
                If ds Is Nothing Then
                    If transaction Is Nothing Then
                        ds = DB.ExecuteDataSet(DBCommandWrapper)
                    Else
                        ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                    End If
                Else
                    If transaction Is Nothing Then
                        DB.LoadDataSet(DBCommandWrapper, ds, "BC_DETAIL_ERROR_CODES")
                    Else
                        DB.LoadDataSet(DBCommandWrapper, ds, "BC_DETAIL_ERROR_CODES", transaction)
                    End If
                End If
                SWriter = New System.IO.StreamWriter(XMLFilename)
                XMLSerial = New XmlSerializer(ds.GetType())
                XMLSerial.Serialize(SWriter, ds)
                File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
            End If

            Return ds

        Catch ex As Exception
            Throw
        Finally
            If SWriter IsNot Nothing Then SWriter.Close()
            SWriter = Nothing
        End Try
    End Function

    Public Shared Function RetrieveDiagnosisXRef(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        If _DiagnosisXRefDS Is Nothing Then
            _DiagnosisXRefDS = LoadDiagnosisXRef(ds, transaction)
        End If

        Return _DiagnosisDS

    End Function
    Private Shared Function LoadDiagnosisXRef(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        SyncLock _DiagnosisValuesXreflockObj

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_ALL_DIAGNOSIS_XREF" & ".xml"
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim FStream As FileStream
            Dim XMLSerial As XmlSerializer
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBMD.RETRIEVE_ALL_DIAGNOSIS_XREF"
            Dim XMLDS As DataSet

            Try

                XMLDS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.DIAGNOSIS_XREF", "LASTUPDT", "FDBMD.RETRIEVE_ALL_DIAGNOSIS_XREF")
                If XMLDS.Tables.Count = 0 Then
                    DB = CMSDALCommon.CreateDatabase()
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
                    DBCommandWrapper.CommandTimeout = 120

                    If XMLDS Is Nothing Then
                        If transaction Is Nothing Then
                            XMLDS = DB.ExecuteDataSet(DBCommandWrapper)
                        Else
                            XMLDS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                        End If
                    Else
                        If transaction Is Nothing Then
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "DIAGNOSIS_XREF")
                        Else
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "DIAGNOSIS_XREF", transaction)
                        End If
                    End If

                    _DiagnosisXRefDS = XMLDS

                    For X As Integer = 1 To 100
                        'If _TraceCaching.Enabled Then CMSDALLog.Log(Generals.NowDate.ToString("HH:mm:ss.fff") & ControlChars.Tab & UniqueThreadIdentifier & ControlChars.Tab & "Checking(2): " & XMLFilename, CMSDALLog.LogDirectory  & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & "Caching.txt")

                        Try
                            FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)
                            Exit For

                        Catch ex As Exception

                            'If _TraceCaching.Enabled Then CMSDALLog.Log(Generals.NowDate.ToString("HH:mm:ss.fff") & ControlChars.Tab & UniqueThreadIdentifier & ControlChars.Tab & "Wait(2): " & XMLFilename & ControlChars.Tab & " Wait# (" & X & ")", CMSDALLog.LogDirectory  & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & "Caching.txt")
                            System.Threading.Thread.Sleep(100 * X)

                        End Try

                    Next

                    '' Dim SWriter As New System.IO.StreamWriter(xmlFilename)
                    XMLSerial = New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(FStream, XMLDS)

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)

                    'If _TraceCaching.Enabled Then CMSDALLog.Log(Generals.NowDate.ToString("HH:mm:ss.fff") & ControlChars.Tab & UniqueThreadIdentifier & ControlChars.Tab & "End Create XML: " & XMLFilename, CMSDALLog.LogDirectory  & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & "Caching.txt")
                Else
                    _DiagnosisXRefDS = XMLDS
                End If

                If _DiagnosisXRefDS IsNot Nothing Then
                    If _DiagnosisXRefDS.Tables(0).PrimaryKey Is Nothing OrElse _DiagnosisXRefDS.Tables(0).PrimaryKey.Length = 0 Then
                        _DiagnosisXRefDS.Tables(0).PrimaryKey = New DataColumn() {_DiagnosisXRefDS.Tables(0).Columns("DIAG_VALUE_ICD10"), _DiagnosisXRefDS.Tables(0).Columns("FROM_DATE"), _DiagnosisXRefDS.Tables(0).Columns("THRU_DATE")}
                    End If
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_DiagnosisXRefDS)
                    Return ds
                Else
                    Return _DiagnosisXRefDS
                End If

            Catch ex As Exception
                Throw
            Finally

                If FStream IsNot Nothing Then FStream.Close()
                FStream = Nothing

            End Try
        End SyncLock

    End Function

    Public Shared Function RetrieveDiagnosisXRefEffectiveAsOf(Optional ByVal effectiveDate As Date? = Nothing) As DataSet

        Dim SelectedRows As DataRow()
        Dim Filter As String
        Dim DS As DataSet
        Dim DT As DataTable

        Try
            DS = New DataSet

            If _DiagnosisXRefDS Is Nothing Then
                Call RetrieveDiagnosisXRef()
            End If

            If effectiveDate Is Nothing Then
                effectiveDate = UFCWGeneral.NowDate
            End If

            Filter = "#" & Format(effectiveDate, "yyyy-MM-dd").ToString & "# >= FROM_DATE AND #" & Format(effectiveDate, "yyyy-MM-dd").ToString & "# <= THRU_DATE "

            _DiagnosisXRefDS.Tables(0).DefaultView.RowFilter = Filter

            SelectedRows = _DiagnosisXRefDS.Tables(0).Select(Filter)

            'DT = _DiagnosisXRefDS.Tables(0).Clone

            DT = SelectedRows.CopyToDataTable
            DT.TableName = _DiagnosisXRefDS.Tables(0).TableName
            DS.Tables.Add(DT)

            Return DS

        Catch ex As Exception
            Throw
        Finally
            If DS IsNot Nothing Then DS.Dispose()
        End Try
    End Function

    Public Shared Function RetrieveDiagnosisValues(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        If _DiagnosisDS Is Nothing Then
            _DiagnosisDS = LoadDiagnosisValues(ds, transaction)
        End If

        Return _DiagnosisDS

    End Function

    Private Shared Function LoadDiagnosisValues(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        SyncLock _DiagnosisValueslockObj

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_ALL_DIAGNOSIS_VALUES" & ".xml"
            Dim UniqueThreadIdentifier As String
            Dim FStream As FileStream
            Dim XMLSerial As XmlSerializer
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBMD.RETRIEVE_ALL_DIAGNOSIS_VALUES"
            Dim XMLDS As DataSet

            Try

                UniqueThreadIdentifier = UFCWGeneral.GetUniqueKey()

                XMLDS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.DIAGNOSIS_VALUES", "LASTUPDT", "FDBMD.RETRIEVE_ALL_DIAGNOSIS_VALUES")
                If XMLDS.Tables.Count = 0 Then
                    DB = CMSDALCommon.CreateDatabase()
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
                    DBCommandWrapper.CommandTimeout = 120

                    If XMLDS Is Nothing Then
                        If transaction Is Nothing Then
                            XMLDS = DB.ExecuteDataSet(DBCommandWrapper)
                        Else
                            XMLDS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                        End If
                    Else
                        If transaction Is Nothing Then
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "DIAGNOSIS_VALUES")
                        Else
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "DIAGNOSIS_VALUES", transaction)
                        End If
                    End If

                    _DiagnosisDS = XMLDS

                    For x As Integer = 1 To 100
                        'If _TraceCaching.Enabled Then CMSDALLog.Log(Generals.NowDate.ToString("HH:mm:ss.fff") & ControlChars.Tab & UniqueThreadIdentifier & ControlChars.Tab & "Checking(2): " & XMLFilename, CMSDALLog.LogDirectory  & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & "Caching.txt")

                        Try
                            FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)
                            Exit For

                        Catch ex As Exception

                            'If _TraceCaching.Enabled Then CMSDALLog.Log(Generals.NowDate.ToString("HH:mm:ss.fff") & ControlChars.Tab & UniqueThreadIdentifier & ControlChars.Tab & "Wait(2): " & XMLFilename & ControlChars.Tab & " Wait# (" & x & ")", CMSDALLog.LogDirectory  & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & "Caching.txt")
                            System.Threading.Thread.Sleep(100 * x)

                        End Try

                    Next

                    '' Dim SWriter As New System.IO.StreamWriter(xmlFilename)
                    XMLSerial = New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(FStream, XMLDS)

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)

                    'If _TraceCaching.Enabled Then CMSDALLog.Log(Generals.NowDate.ToString("HH:mm:ss.fff") & ControlChars.Tab & UniqueThreadIdentifier & ControlChars.Tab & "End Create XML: " & XMLFilename, CMSDALLog.LogDirectory  & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & "Caching.txt")
                Else
                    _DiagnosisDS = XMLDS
                End If

                If _DiagnosisDS IsNot Nothing Then
                    If _DiagnosisDS.Tables(0).PrimaryKey Is Nothing OrElse _DiagnosisDS.Tables(0).PrimaryKey.Length = 0 Then
                        _DiagnosisDS.Tables(0).PrimaryKey = New DataColumn() {_DiagnosisDS.Tables(0).Columns("DIAG_VALUE"), _DiagnosisDS.Tables(0).Columns("FROM_DATE"), _DiagnosisDS.Tables(0).Columns("THRU_DATE")}
                    End If
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_DiagnosisDS)
                    Return ds
                Else
                    Return _DiagnosisDS
                End If

            Catch ex As Exception
                Throw
            Finally

                If FStream IsNot Nothing Then FStream.Close()
                FStream = Nothing

            End Try
        End SyncLock

    End Function

    Public Shared Function RetrieveDiagnosisValuesEffectiveAsOfOld(Optional ByVal effectiveDate As Date? = Nothing) As DataSet

        Dim DS As DataSet

        Try
            If _DiagnosisDS Is Nothing Then
                Call RetrieveDiagnosisValues()
            End If

            DS = _DiagnosisDS.Clone

            If effectiveDate Is Nothing Then
                effectiveDate = UFCWGeneral.NowDate
            End If

            _DiagnosisDS.Tables(0).DefaultView.Sort = "DIAG_VALUE"
            _DiagnosisDS.Tables(0).DefaultView.RowFilter = "#" & Format(effectiveDate, "yyyy-MM-dd") & "# >= FROM_DATE AND #" & Format(effectiveDate, "yyyy-MM-dd") & "# <= THRU_DATE "

            For Each DiagnosisRow As DataRowView In _DiagnosisDS.Tables(0).DefaultView
                DS.Tables(0).LoadDataRow(DiagnosisRow.Row.ItemArray, True)
            Next

            Return DS

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveDiagnosisValuesEffectiveAsOf(Optional ByVal effectiveDate As Date? = Nothing) As DataSet

        Dim SelectedRows As DataRow()
        Dim Filter As String
        Dim DS As DataSet
        Dim DT As DataTable

        Try
            DS = New DataSet

            If _DiagnosisDS Is Nothing Then
                Call RetrieveDiagnosisValues()
            End If

            If effectiveDate Is Nothing Then
                effectiveDate = UFCWGeneral.NowDate
            End If

            Filter = "#" & Format(effectiveDate, "yyyy-MM-dd").ToString & "# >= FROM_DATE AND #" & Format(effectiveDate, "yyyy-MM-dd").ToString & "# <= THRU_DATE "

            _DiagnosisDS.Tables(0).DefaultView.RowFilter = Filter

            SelectedRows = _DiagnosisDS.Tables(0).Select(Filter)

            'DT = _DiagnosisDS.Tables(0).Clone

            DT = SelectedRows.CopyToDataTable
            DT.TableName = _DiagnosisDS.Tables(0).TableName
            DS.Tables.Add(DT)

            Return DS

        Catch ex As Exception
            Throw
        Finally
            If DS IsNot Nothing Then DS.Dispose()
        End Try
    End Function

    Public Shared Function RetrieveDiagnosisPreventativeStatusEffectiveAsOf(ByVal diagnosisCode As String, ByVal effectiveDate As Date?) As DataRow

        SyncLock _DiagnosisPreventativeStatuslockObj

            Dim SelectedDRs As DataRow()

            Try
                If _DiagnosisDS Is Nothing Then
                    Call RetrieveDiagnosisValues()
                End If

                SelectedDRs = _DiagnosisDS.Tables(0).Select("DIAG_VALUE = '" & diagnosisCode.ToString.Trim & "' AND #" & Format(effectiveDate, "yyyy-MM-dd") & "# >= FROM_DATE AND #" & Format(effectiveDate, "yyyy-MM-dd") & "# <= THRU_DATE ")

                If SelectedDRs.Length > 0 Then
                    Return SelectedDRs(0)
                End If

                Return Nothing

            Catch ex As Exception
                Throw
            End Try

        End SyncLock


    End Function

    Public Shared Function RetrieveDiagnosisValuesInformation(ByVal diagnosisCode As String, Optional ByVal effectiveDate As Date? = Nothing) As DataRow

        Dim SelectedDRs As DataRow()

        Try
            If _DiagnosisDS Is Nothing Then
                _DiagnosisDS = RetrieveDiagnosisValues()
            End If

            SelectedDRs = _DiagnosisDS.Tables(0).Select("DIAG_VALUE = '" & diagnosisCode.ToString.Trim & "'" & If(effectiveDate IsNot Nothing, " AND #" & Format(effectiveDate, "yyyy-MM-dd") & "# >= FROM_DATE AND #" & Format(effectiveDate, "yyyy-MM-dd") & "# <= THRU_DATE ", ""))

            If SelectedDRs.Length > 0 Then
                Return SelectedDRs(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveNDCValues(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        If _NDCDS Is Nothing Then
            _NDCDS = LoadNDCValues(ds, transaction)
        End If

        Return _NDCDS

    End Function

    Private Shared Function LoadNDCValues(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        SyncLock _LoadNDCValuesSyncLock

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_ALL_NDC_VALUES" & ".xml"
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim FStream As FileStream
            Dim XMLDS As DataSet
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBMD.RETRIEVE_ALL_NDC_VALUES"
            Dim XMLSerial As XmlSerializer

            Try

                XMLDS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.REDBOOK_NDC", "LASTUPDT", "FDBMD.RETRIEVE_ALL_NDC_VALUES")
                If XMLDS.Tables.Count = 0 Then
                    DB = CMSDALCommon.CreateDatabase()
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
                    DBCommandWrapper.CommandTimeout = 120

                    If XMLDS Is Nothing Then
                        If transaction Is Nothing Then
                            XMLDS = DB.ExecuteDataSet(DBCommandWrapper)
                        Else
                            XMLDS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                        End If
                    Else
                        If transaction Is Nothing Then
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "NDC_VALUES")
                        Else
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "NDC_VALUES", transaction)
                        End If
                    End If

                    _NDCDS = XMLDS

                    For X As Integer = 1 To 100
                        'If _TraceCaching.Enabled Then CMSDALLog.Log(Generals.NowDate.ToString("HH:mm:ss.fff") & ControlChars.Tab & UniqueThreadIdentifier & ControlChars.Tab & "Checking(2): " & XMLFilename, CMSDALLog.LogDirectory  & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & "Caching.txt")

                        Try
                            FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)
                            Exit For

                        Catch ex As Exception

                            'If _TraceCaching.Enabled Then CMSDALLog.Log(Generals.NowDate.ToString("HH:mm:ss.fff") & ControlChars.Tab & UniqueThreadIdentifier & ControlChars.Tab & "Wait(2): " & XMLFilename & ControlChars.Tab & " Wait# (" & X & ")", CMSDALLog.LogDirectory  & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & "Caching.txt")
                            System.Threading.Thread.Sleep(100 * X)

                        End Try

                    Next

                    '' Dim SWriter As New System.IO.StreamWriter(xmlFilename)
                    XMLSerial = New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(FStream, XMLDS)

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)

                    'If _TraceCaching.Enabled Then CMSDALLog.Log(Generals.NowDate.ToString("HH:mm:ss.fff") & ControlChars.Tab & UniqueThreadIdentifier & ControlChars.Tab & "End Create XML: " & XMLFilename, CMSDALLog.LogDirectory  & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & "Caching.txt")
                Else
                    _NDCDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_NDCDS)
                    Return ds
                Else
                    Return _NDCDS
                End If

            Catch ex As Exception
                Throw
            Finally

                If FStream IsNot Nothing Then
                    FStream.Close()

                End If

                FStream = Nothing

            End Try
        End SyncLock

    End Function


    Public Shared Function RetrieveModifierValues(ByVal effectiveDate As Date?, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim CloneDS As DataSet

        Try

            If _ModifierDS Is Nothing Then
                _ModifierDS = LoadModifierValues(ds, transaction)
            End If

            CloneDS = _ModifierDS.Clone

            If effectiveDate Is Nothing OrElse IsDBNull(effectiveDate) Then
                effectiveDate = UFCWGeneral.NowDate
            End If

            _ModifierDS.Tables(0).DefaultView.Sort = "MODIFIER_VALUE"
            _ModifierDS.Tables(0).DefaultView.RowFilter = "#" & Format(effectiveDate, "yyyy-MM-dd") & "# >= FROM_DATE AND #" & Format(effectiveDate, "yyyy-MM-dd") & "# <= THRU_DATE "

            For Each ReasonRow As DataRowView In _ModifierDS.Tables(0).DefaultView
                CloneDS.Tables(0).LoadDataRow(ReasonRow.Row.ItemArray, True)
            Next

            If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                ds.Merge(CloneDS)
                Return ds
            Else
                Return CloneDS
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Shared Function LoadModifierValues(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        SyncLock _LoadModifierValuesSyncLock

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_ALL_MODIFIER_VALUES" & ".xml"
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim FStream As FileStream
            Dim XMLDS As DataSet
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBMD.RETRIEVE_ALL_MODIFIER_VALUES"
            Dim XMLSerial As XmlSerializer

            Try

                XMLDS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.MODIFIER_VALUES", "LASTUPDT", "FDBMD.RETRIEVE_ALL_MODIFIER_VALUES")
                If XMLDS.Tables.Count = 0 Then
                    DB = CMSDALCommon.CreateDatabase()
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        If transaction Is Nothing Then
                            XMLDS = DB.ExecuteDataSet(DBCommandWrapper)
                        Else
                            XMLDS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                        End If
                    Else
                        If transaction Is Nothing Then
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "MODIFIER_VALUES")
                        Else
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "MODIFIER_VALUES", transaction)
                        End If
                    End If

                    _ModifierDS = XMLDS

                    FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)
                    XMLSerial = New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(FStream, XMLDS)

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _ModifierDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_ModifierDS)
                    Return ds
                Else
                    Return _ModifierDS
                End If

            Catch ex As Exception
                Throw
            Finally

                If FStream IsNot Nothing Then
                    FStream.Close()
                End If

                FStream = Nothing

            End Try
        End SyncLock

    End Function

    Public Shared Function RetrieveModifierValuesInformation(ByVal modifierCode As String, Optional ByVal effectiveDate As Date? = Nothing) As DataRow
        Dim SelectedDRs As DataRow()

        Try
            If _ModifierDS Is Nothing Then
                _ModifierDS = RetrieveModifierValues(Nothing, _ModifierDS)
            End If

            SelectedDRs = _ModifierDS.Tables(0).Select("MODIFIER_VALUE = '" & modifierCode.ToString.Trim & "'" & If(effectiveDate IsNot Nothing, " AND #" & Format(effectiveDate, "yyyy-MM-dd") & "# >= FROM_DATE AND #" & Format(effectiveDate, "yyyy-MM-dd") & "# <= THRU_DATE ", ""))

            If SelectedDRs.Length > 0 Then
                Return SelectedDRs(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Shared Function LoadProcedureValues(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        SyncLock _LoadProcedureValuesSyncLock

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_ALL_PROCEDURE_VALUES" & ".xml"
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim FStream As FileStream
            Dim XMLDS As DataSet
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBMD.RETRIEVE_ALL_PROCEDURE_VALUES"
            Dim XMLSerial As XmlSerializer

            Try


                XMLDS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.PROCEDURE_VALUES", "LASTUPDT", "FDBMD.RETRIEVE_ALL_PROCEDURE_VALUES")
                If XMLDS.Tables.Count = 0 Then
                    DB = CMSDALCommon.CreateDatabase()
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        If transaction Is Nothing Then
                            XMLDS = DB.ExecuteDataSet(DBCommandWrapper)
                        Else
                            XMLDS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                        End If
                    Else
                        If transaction Is Nothing Then
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "PROCEDURE_VALUES")
                        Else
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "PROCEDURE_VALUES", transaction)
                        End If
                    End If

                    _ProcedureTotalDS = XMLDS

                    For x As Integer = 1 To 100

                        'If _TraceCaching.Enabled Then CMSDALLog.Log(Generals.NowDate.ToString("HH:mm:ss.fff") & ControlChars.Tab & UniqueThreadIdentifier & ControlChars.Tab & "Checking(2): " & XMLFilename, CMSDALLog.LogDirectory  & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & "Caching.txt")

                        Try
                            FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)
                            Exit For
                        Catch ex As Exception

                            'If _TraceCaching.Enabled Then CMSDALLog.Log(Generals.NowDate.ToString("HH:mm:ss.fff") & ControlChars.Tab & UniqueThreadIdentifier & ControlChars.Tab & "Wait(2): " & XMLFilename & ControlChars.Tab & " Wait# (" & x & ")", CMSDALLog.LogDirectory  & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & "Caching.txt")

                            System.Threading.Thread.Sleep(100 * x)

                        End Try

                    Next

                    If FStream IsNot Nothing Then 'it's possible that it timed out and the stream is not established.
                        XMLSerial = New XmlSerializer(XMLDS.GetType())

                        XMLSerial.Serialize(FStream, XMLDS)

                        'If _TraceCaching.Enabled Then CMSDALLog.Log(Generals.NowDate.ToString("HH:mm:ss.fff") & ControlChars.Tab & UniqueThreadIdentifier & ControlChars.Tab & "Wait: " & XMLFilename, CMSDALLog.LogDirectory  & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & "Caching.txt")

                        File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)

                        'If _TraceCaching.Enabled Then CMSDALLog.Log(Generals.NowDate.ToString("HH:mm:ss.fff") & ControlChars.Tab & UniqueThreadIdentifier & ControlChars.Tab & "End Create XML: " & XMLFilename, CMSDALLog.LogDirectory  & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & "Caching.txt")

                    End If

                Else
                    _ProcedureTotalDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_ProcedureTotalDS)
                    Return ds
                Else
                    Return _ProcedureTotalDS
                End If

            Catch ex As Exception
                Throw
            Finally

                If FStream IsNot Nothing Then
                    FStream.Close()
                End If

                FStream = Nothing

            End Try

        End SyncLock

    End Function

    Public Shared Function RetrieveProcedureValuesAsOfEffectiveDate(ByVal effectiveDate As Date?, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim ProcDV As DataView
        Dim Maxdate As Date = CType("9999-12-31", Date)
        Dim ProcedureClaimDS As DataSet

        Try

            ProcedureClaimDS = New DataSet

            If _ProcedureTotalDS Is Nothing Then
                _ProcedureTotalDS = LoadProcedureValues(ds, transaction)
            End If

            ProcDV = New DataView(_ProcedureTotalDS.Tables("PROCEDURE_VALUES"), "'" & Format(If(effectiveDate, UFCWGeneral.NowDate), "yyyy-MM-dd") & "'" & " >= FROM_DATE AND '" & Format(If(effectiveDate, UFCWGeneral.NowDate), "yyyy-MM-dd") & "'" & "<= THRU_DATE", "", DataViewRowState.CurrentRows)
            ProcedureClaimDS.Tables.Add((_ProcedureTotalDS.Tables("PROCEDURE_VALUES").Clone))

            ProcedureClaimDS.Tables(0).BeginLoadData()
            For I As Integer = 0 To ProcDV.Count - 1
                ProcedureClaimDS.Tables(0).ImportRow(ProcDV(I).Row)
            Next
            ProcedureClaimDS.Tables(0).EndLoadData()

            ds = ProcedureClaimDS

            Return ds

        Catch ex As Exception
            Throw
        Finally
            If ProcDV IsNot Nothing Then ProcDV.Dispose()
            ProcDV = Nothing
            If ProcedureClaimDS IsNot Nothing Then ProcedureClaimDS.Dispose()
            ProcedureClaimDS = Nothing
        End Try
    End Function

    Public Shared Function RetrieveProcedureValues(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        If _ProcedureTotalDS Is Nothing Then
            _ProcedureTotalDS = LoadProcedureValues(ds, transaction)
        End If

        Return _ProcedureTotalDS

    End Function

    Public Shared Function RetrieveProcedureValueInformation(ByVal procedureCode As String, Optional ByVal effectiveDate As Date? = Nothing) As DataRow

        Dim SelectedDRs As DataRow()

        Try

            If _ProcedureTotalDS Is Nothing Then
                _ProcedureTotalDS = LoadProcedureValues()
            End If

            If effectiveDate Is Nothing OrElse IsDBNull(effectiveDate) Then effectiveDate = UFCWGeneral.NowDate

            SelectedDRs = _ProcedureTotalDS.Tables(0).Select("PROC_VALUE = '" & procedureCode.ToString.Trim & "'" & If(effectiveDate IsNot Nothing, " AND #" & Format(effectiveDate, "yyyy-MM-dd") & "# >= FROM_DATE AND #" & Format(effectiveDate, "yyyy-MM-dd") & "# <= THRU_DATE ", ""))

            If SelectedDRs.Length > 0 Then
                Return SelectedDRs(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveBillTypeValuesInformation(ByVal billTypeCode As String, Optional ByVal effectiveDate As Date? = Nothing) As DataRow
        Dim SelectedDRs As DataRow()

        Try
            If _BillTypeDS Is Nothing Then
                _BillTypeDS = RetrieveBillTypeValues(Nothing, _BillTypeDS)
            End If

            SelectedDRs = _BillTypeDS.Tables(0).Select("BILL_TYPE_VALUE = '" & billTypeCode.ToString.Trim & "'" & If(effectiveDate IsNot Nothing, " AND #" & Format(effectiveDate, "yyyy-MM-dd") & "# >= FROM_DATE AND #" & Format(effectiveDate, "yyyy-MM-dd") & "# <= THRU_DATE ", ""))

            If SelectedDRs.Length > 0 Then
                Return SelectedDRs(0)
            Else
                Return ValidateBillTypeByPosition(billTypeCode, _BillTypeDS)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveAccumulatorInformation(ByVal accumulator As String) As DataRow
        Dim DT As DataTable
        Dim SelectedDRs As DataRow()

        Try
            DT = New DataTable("ACCUMULATORS")

            If _AccumulatorsDS Is Nothing Then
                _AccumulatorsDS = GetActiveAccumulators()
            End If

            DT = _AccumulatorsDS.Tables("ACCUMULATORS")

            SelectedDRs = DT.Select("ACCUM_NAME = '" & accumulator.ToString.Trim & "'", "")

            If SelectedDRs.Length > 0 Then
                Return SelectedDRs(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        Finally
            If DT IsNot Nothing Then DT.Dispose()
        End Try
    End Function
    Public Shared Function GetPremiumInformation(ByVal FamilyId As Integer, Optional ByVal DS As DataSet = Nothing, Optional ByVal Transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand = DB.GetStoredProcCommand("FDBMD.RETRIEVE_PREMSUM_BY_FAMILYID")
        Try
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, FamilyId)
            DB.AddInParameter(DBCommandWrapper, "@EMPLOYEE_ACCESS", DbType.Int32, If(UFCWGeneralAD.CMSCanAdjudicateEmployee() Or UFCWGeneralAD.CMSEligibilityEmployee(), 1, 0))
            DB.AddInParameter(DBCommandWrapper, "@LOCAL_ACCESS", DbType.Int32, If(UFCWGeneralAD.CMSLocalsEmployee() Or UFCWGeneralAD.CMSEligibility(), 1, 0))
            DBCommandWrapper.CommandTimeout = 180

            If DS Is Nothing Then
                If Transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, Transaction)
                End If
                DS.Tables(0).TableName = "PREMSUM"
            Else
                If Transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "PREMSUM")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "PREMSUM", Transaction)
                End If
            End If
            Return DS
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GetPrescriptionsInformation(ByVal fromDate As Date, ByVal toDate As Date, ByVal familyID As Integer?, Optional ByVal relationID As Short? = Nothing, Optional ByVal DS As DataSet = Nothing, Optional ByVal Transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RETRIEVE_PRESCRIPTIONS_DATA_BY_FAMILYID_RELATIONID_DOS")

            DB.AddInParameter(DBCommandWrapper, "@FAMILYID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATIONID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@PRESCRIPTIONS_FROMDATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@PRESCRIPTIONS_TODATE", DbType.Date, toDate)
            DBCommandWrapper.CommandTimeout = 180

            If DS Is Nothing Then
                If Transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, Transaction)
                End If
                DS.Tables(0).TableName = "Prescriptions"
            Else
                If Transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "Prescriptions")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "Prescriptions", Transaction)
                End If
            End If

            Return DS

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GetPrescriptionsInformation(ByVal familyID As Integer?, Optional ByVal relationID As Short? = Nothing, Optional ByVal DS As DataSet = Nothing, Optional ByVal Transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RETRIEVE_PRESCRIPTIONS_BY_FID_RID")

            DB.AddInParameter(DBCommandWrapper, "@FAMILYID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATIONID", DbType.Int16, relationID)
            DBCommandWrapper.CommandTimeout = 180

            If DS Is Nothing Then
                If Transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, Transaction)
                End If
                DS.Tables(0).TableName = "Prescriptions"
            Else
                If Transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "Prescriptions")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "Prescriptions", Transaction)
                End If
            End If

            Return DS

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GetPrescriptionsInformation(ByVal ssn As Integer?, ByVal presFromdate As Date, ByVal presTodate As Date, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RETRIEVE_PRESCRIPTIONS_DATA_BY_SSN")

            DB.AddInParameter(DBCommandWrapper, "@SSN", DbType.Int32, If(ssn Is Nothing, Nothing, CType(ssn, Integer?)))
            DB.AddInParameter(DBCommandWrapper, "@PRESCRIPTIONS_FROMDATE", DbType.Date, presFromdate)
            DB.AddInParameter(DBCommandWrapper, "@PRESCRIPTIONS_TODATE", DbType.Date, presTodate)
            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
                ds.Tables(0).TableName = "Prescriptions"
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "Prescriptions")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "Prescriptions", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GetActiveAccumulators(Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        SyncLock _GetActiveAccumulatorsSyncLock

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_ACTIVE_ACCUMULATORS" & ".xml"
            Dim DS As DataSet
            Dim UniqueThreadIdentifier As String
            Dim FStream As FileStream
            Dim XMLSerial As XmlSerializer
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBMD.RETRIEVE_ACTIVE_ACCUMULATORS"

            Try

                UniqueThreadIdentifier = UFCWGeneral.GetUniqueKey()

                If _AccumulatorsDS Is Nothing Then
                    DS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.ACCUMULATOR", "LASTUPDT", "FDBMD.RETRIEVE_ACTIVE_ACCUMULATORS", False, "WHERE ACTIVE_SW = 1")
                    If DS.Tables.Count = 0 Then
                        DB = CMSDALCommon.CreateDatabase()
                        DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                        If DS Is Nothing Then
                            If transaction Is Nothing Then
                                DS = DB.ExecuteDataSet(DBCommandWrapper)
                            Else
                                DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                            End If
                            DS.Tables(0).TableName = "ACCUMULATORS"
                        Else
                            If transaction Is Nothing Then
                                DB.LoadDataSet(DBCommandWrapper, DS, "ACCUMULATORS")
                            Else
                                DB.LoadDataSet(DBCommandWrapper, DS, "ACCUMULATORS", transaction)
                            End If
                        End If

                        FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                        XMLSerial = New XmlSerializer(DS.GetType())
                        XMLSerial.Serialize(FStream, DS)

                        File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                    End If
                End If

                Return DS

            Catch ex As Exception
                Throw
            Finally

                If FStream IsNot Nothing Then
                    FStream.Close()

                End If

                FStream = Nothing

            End Try
        End SyncLock

    End Function

    Public Shared Function RetrieveClaimStatusValues() As DataTable

        SyncLock _RetrieveClaimStatusValuesSyncLock

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_CLAIM_STATUS_VALUES" & ".xml"
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim DS As DataSet
            Dim FileStream As FileStream
            Dim DT As DataTable
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBMD.RETRIEVE_CLAIM_STATUS_VALUES"
            Dim XMLSerial As XmlSerializer

            Try

                If _StatusValuesDS Is Nothing Then
                    DS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.CLAIM_STATUS_VALUES", "LASTUPDT", "FDBMD.RETRIEVE_CLAIM_STATUS_VALUES")
                    If DS.Tables.Count = 0 Then

                        DB = CMSDALCommon.CreateDatabase()
                        DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                        If DS Is Nothing Then
                            DS = DB.ExecuteDataSet(DBCommandWrapper)
                        Else
                            DB.LoadDataSet(DBCommandWrapper, DS, "CLAIM_STATUS_VALUES")
                        End If

                        _StatusValuesDS = DS

                        FileStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                        XMLSerial = New XmlSerializer(DS.GetType())
                        XMLSerial.Serialize(FileStream, DS)

                        File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                    Else
                        _StatusValuesDS = DS
                    End If
                Else
                    DS = _StatusValuesDS
                End If

                DT = New DataTable("CLAIM_STATUS_VALUES")
                If DS.Tables.Count > 0 Then
                    DT = DS.Tables(0)
                End If
                Return DT

            Catch ex As Exception
                Throw
            Finally
                If FileStream IsNot Nothing Then FileStream.Close()
                FileStream = Nothing
                If DT IsNot Nothing Then DT.Dispose()
                DT = Nothing

            End Try
        End SyncLock

    End Function
    Public Shared Function GetLetters(ByVal documentClass As String, Optional ByVal ds As DataSet = Nothing) As DataSet

        SyncLock _GetLettersSyncLock

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_LETTERS" & ".xml"
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String
            Dim LetterDS As DataSet
            Dim LetterDV As DataView
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim FileStream As FileStream
            Dim LetterDT As DataTable
            Dim XMLSerial As XmlSerializer

            Try
                DB = CMSDALCommon.CreateDatabase()

                LetterDS = New DataSet

                If _LetterValuesDS Is Nothing Then
                    ds = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.LETTER_MASTER", "LASTUPDT", "FDBMD.RETRIEVE_LETTERS")
                    If ds.Tables.Count = 0 Then
                        SQLCall = "FDBMD.RETRIEVE_LETTERS"
                        DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                        If ds Is Nothing Then
                            ds = DB.ExecuteDataSet(DBCommandWrapper)
                        Else
                            DB.LoadDataSet(DBCommandWrapper, ds, "LETTER_MASTER")
                        End If

                        _LetterValuesDS = ds

                        FileStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)

                        XMLSerial = New XmlSerializer(ds.GetType())
                        XMLSerial.Serialize(FileStream, ds)

                        File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                    Else
                        _LetterValuesDS = ds
                    End If
                Else
                    ds = _LetterValuesDS
                End If

                LetterDT = _LetterValuesDS.Tables(0).Clone
                LetterDV = New DataView(_LetterValuesDS.Tables("LETTER_MASTER"), "DOC_CLASS='" & documentClass & "'", "LETTER_NAME ASC", DataViewRowState.CurrentRows)

                If LetterDV.Count > 0 Then
                    For Each drv As DataRowView In LetterDV
                        LetterDT.LoadDataRow(drv.Row.ItemArray, True)
                    Next
                End If
                LetterDS.Tables.Add(LetterDT)
                Return LetterDS
            Catch ex As Exception
                Throw
            Finally

                If FileStream IsNot Nothing Then FileStream.Close()
                FileStream = Nothing
                If LetterDS IsNot Nothing Then LetterDS.Dispose()
                LetterDS = Nothing
                If LetterDV IsNot Nothing Then LetterDV.Dispose()
                LetterDV = Nothing
            End Try
        End SyncLock

    End Function
#End Region
#Region "CRUD"
    Public Shared Sub CreateClaimHistory(ByVal CLAIM_ID As Integer, ByVal TRANSACTION_TYPE As String, ByVal FAMILY_ID As Integer,
                                        ByVal RELATION_ID As Short, ByVal PART_SSN As Integer, ByVal PAT_SSN As Integer, ByVal DOC_CLASS As String,
                                        ByVal DOC_TYPE As String, ByVal SUMMARY As String, ByVal DETAIL As String, Optional ByVal DOCID As Decimal = Nothing,
                                        Optional ByVal transaction As DbTransaction = Nothing)


        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_DOC_HISTORY"


        Try

            DB = CMSDALCommon.CreateDatabase()


            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, CLAIM_ID)
            DB.AddInParameter(DBCommandWrapper, "@DOCID", DbType.Decimal, DOCID)
            DB.AddInParameter(DBCommandWrapper, "@TRANSACTION_TYPE", DbType.String, TRANSACTION_TYPE)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, FAMILY_ID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, RELATION_ID)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, PART_SSN)
            DB.AddInParameter(DBCommandWrapper, "@PAT_SSN", DbType.Int32, PAT_SSN)
            DB.AddInParameter(DBCommandWrapper, "@DOC_CLASS", DbType.String, DOC_CLASS)
            DB.AddInParameter(DBCommandWrapper, "@DOC_TYPE", DbType.String, DOC_TYPE)
            DB.AddInParameter(DBCommandWrapper, "@SUMMARY", DbType.String, SUMMARY)
            DB.AddInParameter(DBCommandWrapper, "@DETAIL", DbType.String, DETAIL)
            DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String, "MAILMERGE")

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        Finally

        End Try
    End Sub
    Public Shared Function CreateClaimMasterLetterUsingOriginalClaimAsTemplate(ByVal ClaimID As Integer, ByVal DocumentID As Decimal, ByVal MAXID As String,
                            ByVal PAGE_COUNT As Integer,
                            ByVal PARTFNAME As String,
                            ByVal PARTINT As String,
                            ByVal PARTLNAME As String,
                            ByVal PRIORITY As Integer,
                            ByVal BUSYSW As Integer,
                            ByVal ARCHIVESW As Integer,
                            ByVal DOC_CLASS As String,
                            ByVal DOC_TYPE As String,
                            ByVal PATINT As String,
                            ByVal REQUESTDATE As Date,
                            Optional ByVal Transaction As DbTransaction = Nothing) As Int32

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_CLAIM_MASTER_LETTER_USING_MEDHDR"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@DOCID", DbType.Decimal, DocumentID)
            DB.AddInParameter(DBCommandWrapper, "@MAXID", DbType.String, MAXID)
            DB.AddInParameter(DBCommandWrapper, "@PAGE_COUNT", DbType.Int32, PAGE_COUNT)
            DB.AddInParameter(DBCommandWrapper, "@REFRENCE_CLAIM", DbType.Int32, ClaimID)
            DB.AddInParameter(DBCommandWrapper, "@PART_FNAME", DbType.String, PARTFNAME)
            DB.AddInParameter(DBCommandWrapper, "@PART_INT", DbType.String, PARTINT)
            DB.AddInParameter(DBCommandWrapper, "@PART_LNAME", DbType.String, PARTLNAME)
            DB.AddInParameter(DBCommandWrapper, "@PRIORITY", DbType.Int32, PRIORITY)
            DB.AddInParameter(DBCommandWrapper, "@BUSYSW", DbType.Int32, BUSYSW)
            DB.AddInParameter(DBCommandWrapper, "@ARCHIVESW", DbType.Int32, ARCHIVESW)
            DB.AddInParameter(DBCommandWrapper, "@DOCCLASS", DbType.String, "MEDICAL")
            DB.AddInParameter(DBCommandWrapper, "@DOCTYPE", DbType.String, DOC_TYPE)
            DB.AddInParameter(DBCommandWrapper, "@PAT_INT", DbType.String, PATINT)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, "MAILMERGE")
            DB.AddInParameter(DBCommandWrapper, "@REQUESTDATE", DbType.DateTime, UFCWGeneral.ToNullDateHandler(REQUESTDATE))

            DB.AddOutParameter(DBCommandWrapper, "@CLAIMID", DbType.Int32, 10)

            If Transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, Transaction)
            End If

            Return CInt(DB.GetParameterValue(DBCommandWrapper, "@CLAIMID"))

        Catch ex As DB2Exception
            If ex.Number = -407 Then
                Throw New System.Exception("No matching Claim Master or MedHDR record found for Claim (" & ClaimID & ")!")
            Else
                Throw
            End If
        Catch ex As Exception
            Throw
        Finally
        End Try
    End Function
    Public Shared Function CreateAssociatedLetter(ByVal claimID As Integer, ByVal documentID As Long, ByVal maxID As String,
                                                  ByVal pageCount As Integer,
                                                  ByVal letterID As Long,
                                                  ByVal familyID As Integer,
                                                  ByVal relationID As Integer,
                                                  ByVal partSSN As Integer,
                                                  ByVal patSSN As Integer,
                                                  ByVal docClass As String,
                                                  ByVal docType As String,
                                                  ByVal userRights As String,
                                                  ByVal summary As String,
                                                  ByVal detail As String,
                                                  Optional ByVal transaction As DbTransaction = Nothing) As Int32

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_ASSOCIATED_CLAIM_LETTER"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@REFRENCE_CLAIM", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@DOCID", DbType.Decimal, documentID)
            DB.AddInParameter(DBCommandWrapper, "@MAXID", DbType.String, maxID)
            DB.AddInParameter(DBCommandWrapper, "@PAGE_COUNT", DbType.Int32, pageCount)
            DB.AddInParameter(DBCommandWrapper, "@LETTERID", DbType.Decimal, letterID)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, partSSN)
            DB.AddInParameter(DBCommandWrapper, "@PAT_SSN", DbType.Int32, patSSN)
            DB.AddInParameter(DBCommandWrapper, "@DOCCLASS", DbType.String, docClass)
            DB.AddInParameter(DBCommandWrapper, "@DOCTYPE", DbType.String, docType)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@SUMMARY", DbType.String, summary)
            DB.AddInParameter(DBCommandWrapper, "@DETAIL", DbType.String, detail)

            DB.AddOutParameter(DBCommandWrapper, "@CLAIMID", DbType.Int32, 10)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

            Dim OutputParm As Object = DB.GetParameterValue(DBCommandWrapper, "@CLAIMID")

            If IsNumeric(OutputParm) Then Return CInt(OutputParm)

            Throw New ApplicationException("Failed to associate Letter with Claim ")

        Catch ex As DB2Exception
            If ex.Number = -407 Then
                Throw New ApplicationException("No matching Claim Master or MedHDR record found for Claim (" & claimID & ")!")
            Else
                Throw New ApplicationException("DB2 Error: " & ex.Message)
            End If
        Catch ex As Exception
            Throw
        Finally
        End Try
    End Function
    Shared Function IdentifyPatient(partSSN As Integer?, patSSN As Integer?, firstName As String, lastName As String, patientDOB As Date?, ByRef cursorUsed As String, ByRef familyID As Integer?, ByRef relationID As Short?) As DataTable

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.IDENTIFY_PATIENT"
        Dim DS As DataSet

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, partSSN)
            DB.AddInParameter(DBCommandWrapper, "@PAT_SSN", DbType.Int32, patSSN)
            DB.AddInParameter(DBCommandWrapper, "@FIRST_NAME", DbType.String, firstName.Trim)
            DB.AddInParameter(DBCommandWrapper, "@LAST_NAME", DbType.String, lastName.Trim)
            DB.AddInParameter(DBCommandWrapper, "@DOB", DbType.Date, UFCWGeneral.ToNullDateHandler(patientDOB))
            DB.AddOutParameter(DBCommandWrapper, "@CURSORNAME", DbType.String, 40)
            DB.AddOutParameter(DBCommandWrapper, "@ROWCOUNT", DbType.Int32, 8)
            DB.AddOutParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, 8)
            DB.AddOutParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, 4)

            DBCommandWrapper.CommandTimeout = 180

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            cursorUsed = DB.GetParameterValue(DBCommandWrapper, "@CURSORNAME").ToString
            familyID = CType(DB.GetParameterValue(DBCommandWrapper, "@FAMILY_ID"), Integer?)
            relationID = CType(DB.GetParameterValue(DBCommandWrapper, "@RELATION_ID"), Short?)

            If DS.Tables.Count > 0 Then
                Return DS.Tables(0)
            End If

            Return Nothing

        Catch ex As DB2Exception

            Throw

        Catch ex As Exception

            Throw
        Finally

            DB = Nothing

        End Try
    End Function

    Public Shared Sub IdentifyDuplicates(ByVal claimID As Integer, ByVal familyID As Integer, ByVal relationID As Short, ByVal provTIN As Integer, ByVal occFromDate As Date, ByVal totChrgAmt As Decimal, ByVal numberOfDetailLines As Integer, ByRef probableDuplicate As Decimal, ByRef definiteDuplicate As Decimal, ByRef deniableDuplicate As Decimal, Optional SaveData As Integer = 0)
        'Note: This routine will update the claim_master record directly if the item is determined to be a duplicate
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.DETERMINE_IF_DUPLICATE"

        Try

            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.FamilyID = familyID.ToString.Trim
            UFCWLastKeyData.RelationID = relationID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@PROVTIN", DbType.Int32, provTIN)
            DB.AddInParameter(DBCommandWrapper, "@OCCFROMDATE", DbType.Date, UFCWGeneral.ToNullDateHandler(occFromDate))
            DB.AddInParameter(DBCommandWrapper, "@TOTCHRGAMT", DbType.Decimal, totChrgAmt)
            DB.AddInParameter(DBCommandWrapper, "@DETAILINES", DbType.Int32, numberOfDetailLines)
            DB.AddInParameter(DBCommandWrapper, "@SAVEDATA", DbType.Int32, SaveData)
            DB.AddOutParameter(DBCommandWrapper, "@PROBABLEDUPLICATE", DbType.Int32, 8)
            DB.AddOutParameter(DBCommandWrapper, "@DEFINITEDUPLICATE", DbType.Int32, 8)
            DB.AddOutParameter(DBCommandWrapper, "@DENIABLEDUPLICATE", DbType.Int32, 8)

            DBCommandWrapper.CommandTimeout = 180

            DB.ExecuteNonQuery(DBCommandWrapper)

            probableDuplicate = CDec(DB.GetParameterValue(DBCommandWrapper, "@PROBABLEDUPLICATE"))
            definiteDuplicate = CDec(DB.GetParameterValue(DBCommandWrapper, "@DEFINITEDUPLICATE"))
            deniableDuplicate = CDec(DB.GetParameterValue(DBCommandWrapper, "@DENIABLEDUPLICATE"))

        Catch ex As DB2Exception

            Throw
        Catch ex As Exception

            Throw
        Finally

            DB = Nothing

        End Try
    End Sub

    Public Shared Sub IdentifyHospitalDuplicates(ByVal claimID As Integer, ByVal familyID As Integer, ByVal relationID As Short, ByVal provTIN As Integer, ByVal occFromDate As Date, ByVal totChrgAmt As Decimal, ByVal numberOfDetailLines As Integer, ByRef probableDuplicate As Decimal, ByRef definiteDuplicate As Decimal, ByRef deniableDuplicate As Decimal, Optional SaveData As Integer = 0)

        'Note: This routine will update the claim_master record directly if the item is determined to be a duplicate
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.DETERMINE_IF_DUPLICATE_INCLUDE_PRICED_AMT"

        Try

            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.FamilyID = familyID.ToString.Trim
            UFCWLastKeyData.RelationID = relationID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@PROVTIN", DbType.Int32, provTIN)
            DB.AddInParameter(DBCommandWrapper, "@OCCFROMDATE", DbType.Date, UFCWGeneral.ToNullDateHandler(occFromDate))
            DB.AddInParameter(DBCommandWrapper, "@TOTCHRGAMT", DbType.Decimal, totChrgAmt)
            DB.AddInParameter(DBCommandWrapper, "@DETAILINES", DbType.Int32, numberOfDetailLines)
            DB.AddInParameter(DBCommandWrapper, "@SAVEDATA", DbType.Int32, SaveData)
            DB.AddOutParameter(DBCommandWrapper, "@PROBABLEDUPLICATE", DbType.Int32, 8)
            DB.AddOutParameter(DBCommandWrapper, "@DEFINITEDUPLICATE", DbType.Int32, 8)
            DB.AddOutParameter(DBCommandWrapper, "@DENIABLEDUPLICATE", DbType.Int32, 8)

            DBCommandWrapper.CommandTimeout = 180

            DB.ExecuteNonQuery(DBCommandWrapper)

            probableDuplicate = CDec(DB.GetParameterValue(DBCommandWrapper, "@PROBABLEDUPLICATE"))
            definiteDuplicate = CDec(DB.GetParameterValue(DBCommandWrapper, "@DEFINITEDUPLICATE"))
            deniableDuplicate = CDec(DB.GetParameterValue(DBCommandWrapper, "@DENIABLEDUPLICATE"))

        Catch ex As DB2Exception

            Throw

        Catch ex As Exception

            Throw

        Finally

            DB = Nothing

        End Try
    End Sub

    Public Shared Function RetrieveQueue(ByVal employeeSecurity As Boolean, ByVal userRights As String, ByVal docClass As String, Optional ByVal docType As String = Nothing, Optional ByVal myAssignmentsUserName As String = Nothing, Optional ByVal maxItems As Integer? = Nothing, Optional ByVal identifier As String = Nothing, Optional ByVal familyID As Integer? = Nothing, Optional ByVal docID As Decimal? = Nothing, Optional ByVal batch As String = Nothing, Optional ByVal partSSN As Integer? = Nothing, Optional ByVal tin As Integer? = Nothing) As DataTable
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_CLAIM_MASTER_BY_FILTER"
        Dim DS As DataSet
        Dim DT As DataTable

        Try

            UFCWLastKeyData.ClaimID = If(identifier Is Nothing, "", identifier.ToString).Trim
            UFCWLastKeyData.FamilyID = familyID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@SECURITY_SW", DbType.Decimal, If(employeeSecurity = True, 1, 0))
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@DOC_CLASS", DbType.String, docClass)
            DB.AddInParameter(DBCommandWrapper, "@DOC_TYPE", DbType.String, docType)
            DB.AddInParameter(DBCommandWrapper, "@PENDED_TO", DbType.String, myAssignmentsUserName)
            DB.AddInParameter(DBCommandWrapper, "@MAX_ITEMS", DbType.Int32, maxItems)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.String, identifier)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@DOCID", DbType.Decimal, docID)
            DB.AddInParameter(DBCommandWrapper, "@BATCH", DbType.String, batch)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, partSSN)
            DB.AddInParameter(DBCommandWrapper, "@PROV_TIN", DbType.Int32, tin)

            DB.AddOutParameter(DBCommandWrapper, "@SQLOUT", DbType.String, 8000)

            DBCommandWrapper.CommandTimeout = 300

            DS = DB.ExecuteDataSet(DBCommandWrapper)
            DT = New DataTable("CLAIM_MASTER")
            If DS.Tables.Count > 0 Then
                DT = DS.Tables(0)
            End If

            If System.Configuration.ConfigurationManager.AppSettings("LogSQLFileName") IsNot Nothing AndAlso System.Configuration.ConfigurationManager.AppSettings("LogSQLFileName").ToString.Trim.Length > 0 Then
                CMSDALLog.Log("Query Timestamp: " & UFCWGeneral.NowDate & " --Query: " & CStr(DB.GetParameterValue(DBCommandWrapper, "@SQLOUT")), CMSDALLog.LogDirectory & String.Format("{0000}", UFCWGeneral.NowDate.Year) & String.Format("{00}", UFCWGeneral.NowDate.Month) & System.Configuration.ConfigurationManager.AppSettings("LogSQLFileName"))
            End If

            Return DT

        Catch ex As Exception
            Throw
        Finally
            If DS IsNot Nothing Then DS.Dispose()
            If DT IsNot Nothing Then DT.Dispose()


        End Try
    End Function

    Public Shared Function RetrieveAuditQueue(ByVal employeeSecurity As Boolean, ByVal userRights As String, ByVal docClass As String, Optional ByVal docType As String = Nothing, Optional ByVal myAssignmentsUserName As String = Nothing, Optional ByVal maxItems As Integer? = Nothing, Optional ByVal identifier As String = Nothing, Optional ByVal familyID As Integer? = Nothing, Optional ByVal docID As Decimal? = Nothing, Optional ByVal batch As String = Nothing, Optional ByVal partSSN As Integer? = Nothing, Optional ByVal tin As Integer? = Nothing) As DataTable
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_CLAIM_MASTER_AUDIT_BY_FILTER"
        Dim DS As DataSet

        Dim DT As DataTable

        Try
            UFCWLastKeyData.ClaimID = If(identifier Is Nothing, "", identifier.ToString).Trim
            UFCWLastKeyData.FamilyID = familyID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@SECURITY_SW", DbType.Decimal, If(employeeSecurity = True, 1, 0))
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@DOC_CLASS", DbType.String, docClass)
            DB.AddInParameter(DBCommandWrapper, "@DOC_TYPE", DbType.String, docType)
            DB.AddInParameter(DBCommandWrapper, "@PENDED_TO", DbType.String, myAssignmentsUserName)
            DB.AddInParameter(DBCommandWrapper, "@MAX_ITEMS", DbType.Int32, maxItems)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.String, identifier)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@DOCID", DbType.Decimal, docID)
            DB.AddInParameter(DBCommandWrapper, "@BATCH", DbType.String, batch)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, partSSN)
            DB.AddInParameter(DBCommandWrapper, "@PROV_TIN", DbType.Int32, tin)
            DB.AddOutParameter(DBCommandWrapper, "@SQLOUT", DbType.String, 8000)
            DBCommandWrapper.CommandTimeout = 300

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            DT = New DataTable("CLAIM_MASTER")
            If DS.Tables.Count > 0 Then
                DT = DS.Tables(0)
            End If

            If System.Configuration.ConfigurationManager.AppSettings("LogSQLFileName") IsNot Nothing AndAlso System.Configuration.ConfigurationManager.AppSettings("LogSQLFileName").ToString.Trim.Length > 0 Then
                CMSDALLog.Log("Query Timestamp: " & UFCWGeneral.NowDate & " --Query: " & CStr(DB.GetParameterValue(DBCommandWrapper, "@SQLOUT")), CMSDALLog.LogDirectory & String.Format("{0000}", UFCWGeneral.NowDate.Year) & String.Format("{00}", UFCWGeneral.NowDate.Month) & System.Configuration.ConfigurationManager.AppSettings("LogSQLFileName"))
            End If

            Return DT
        Catch ex As Exception
            Throw
        Finally
            If DS IsNot Nothing Then DS.Dispose()
            If DT IsNot Nothing Then DT.Dispose()
        End Try
    End Function

    Public Shared Function GetNext(ByRef transaction As DbTransaction, ByVal userRights As String, ByVal employeeSecurity As Boolean, ByVal docClass As String, ByVal docType As String, ByVal workStation As String) As Integer
        Try

            Return UpdateGetNextItem(transaction, userRights, employeeSecurity, docClass, docType, workStation)

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetNext(ByVal userRights As String, ByVal employeeSecurity As Boolean, ByVal docClass As String, ByVal docType As String, ByVal workStation As String) As Integer
        Try

            Return UpdateGetNextItem(userRights, employeeSecurity, docClass, docType, workStation)

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function UpdateGetNextItem(ByRef transaction As DbTransaction, ByVal userRights As String, ByVal employeeSecurity As Boolean, ByVal docClass As String, ByVal docType As String, ByVal workStation As String) As Integer

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CLAIM_MASTER_GETNEXT"

        Try

            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@SECURITY_SW", DbType.Decimal, If(employeeSecurity = True, 1, 0))
            DB.AddInParameter(DBCommandWrapper, "@DOC_CLASS", DbType.String, docClass)
            DB.AddInParameter(DBCommandWrapper, "@DOC_TYPE", DbType.String, docType)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@BUSY", DbType.Decimal, 1)
            DB.AddInParameter(DBCommandWrapper, "@WORKSTATION", DbType.String, workStation)
            DB.AddOutParameter(DBCommandWrapper, "@CLAIMID", DbType.Int32, 4)

            'dbCommandWrapper.CommandTimeout = 20

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

            If DB.GetParameterValue(DBCommandWrapper, "@CLAIMID") Is System.DBNull.Value Then
                Return Nothing
            Else
                Return CInt(DB.GetParameterValue(DBCommandWrapper, "@CLAIMID"))
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function UpdateGetNextItem(ByVal userRights As String, ByVal employeeSecurity As Boolean, ByVal docClass As String, ByVal docType As String, ByVal workStation As String) As Integer
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase()

            If docType.Trim.ToUpper.StartsWith("UTL") Then
                SQLCall = "FDBMD.UPDATE_CLAIM_MASTER_GETNEXT_UTL" 'update_CLAIM_MASTER_GETNEXT_UTL
            Else
                SQLCall = "FDBMD.CLAIM_MASTER_GETNEXT"
            End If

            UFCWLastKeyData.SQL = SQLCall

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@SECURITY_SW", DbType.Decimal, If(employeeSecurity = True, 1, 0))
            DB.AddInParameter(DBCommandWrapper, "@DOC_CLASS", DbType.String, docClass)
            DB.AddInParameter(DBCommandWrapper, "@DOC_TYPE", DbType.String, docType)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@BUSY", DbType.Decimal, 1)
            DB.AddInParameter(DBCommandWrapper, "@WORKSTATION", DbType.String, workStation)
            DB.AddOutParameter(DBCommandWrapper, "@CLAIMID", DbType.Int32, 4)

            'dbCommandWrapper.CommandTimeout = 20

            DB.ExecuteNonQuery(DBCommandWrapper)

            If DB.GetParameterValue(DBCommandWrapper, "@CLAIMID") Is System.DBNull.Value Then
                Return Nothing
            Else
                Return CInt(DB.GetParameterValue(DBCommandWrapper, "@CLAIMID"))
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Sub ClearUserMachineClaimLocks(ByVal userName As String, ByVal machineName As String)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String
        Dim DBConnection As DbConnection
        Dim Transaction As DbTransaction

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBConnection = DB.CreateConnection

            DBConnection.Open()
            Transaction = CMSDALCommon.BeginTransaction

            SQLCall = "FDBMD.UPDATE_BUSY_ITEMS_FOR_USER_MACHINE"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@USERID", DbType.String, userName)
            DB.AddInParameter(DBCommandWrapper, "@WORKSTATION", DbType.String, machineName)
            DBCommandWrapper.CommandTimeout = 180
            DB.ExecuteNonQuery(DBCommandWrapper, Transaction)


            SQLCall = "FDBMD.DELETE_CLAIM_LOCKS_FOR_USER_MACHINE"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@USERID", DbType.String, userName)
            DB.AddInParameter(DBCommandWrapper, "@WORKSTATION", DbType.String, machineName)
            DBCommandWrapper.CommandTimeout = 180
            DB.ExecuteNonQuery(DBCommandWrapper, Transaction)

            CMSDALCommon.CommitTransaction(Transaction)

        Catch ex As Exception

            Try

                If Transaction IsNot Nothing AndAlso Transaction.Connection IsNot Nothing AndAlso Transaction.Connection.State <> ConnectionState.Closed Then
                    CMSDALCommon.RollbackTransaction(Transaction)
                End If

            Finally
            End Try

            Throw
        Finally
            If DBConnection IsNot Nothing Then
                DBConnection.Close()
            End If

            If Transaction IsNot Nothing Then Transaction.Dispose()

        End Try
    End Sub

    Public Shared Function GetNextAudit(ByRef transaction As DbTransaction, ByVal userRights As String, ByVal employeeSecurity As Boolean, ByVal docClass As String, ByVal docType As String) As Integer
        Try

            Return UpdateGetNextItemAudit(transaction, userRights, employeeSecurity, docClass, docType)

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function ValidateLocalUser(ByVal sSN As Integer, Optional ByVal transaction As DbTransaction = Nothing) As Integer
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.VALIDATE_USER_LOCAL"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@SSNO", DbType.Int32, sSN)
            DB.AddOutParameter(DBCommandWrapper, "@MATCHCOUNT", DbType.Int32, 1)

            If transaction Is Nothing Then
                DB.ExecuteReader(DBCommandWrapper)
            Else
                DB.ExecuteReader(DBCommandWrapper, transaction)
            End If

            Try
                Return CInt(DB.GetParameterValue(DBCommandWrapper, "@MATCHCOUNT"))
            Catch
                Return 0
            End Try

        Catch ex As Exception
            Throw
        Finally

            If DBCommandWrapper IsNot Nothing Then DBCommandWrapper.Dispose()
            DBCommandWrapper = Nothing

        End Try
    End Function

    Public Shared Function ValidatePriorClaim(ByVal claimID As Integer?, ByVal dcn As String, Optional ByVal transaction As DbTransaction = Nothing) As Integer
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.VALIDATE_DCN_AND_CLAIMID"

        Try

            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIMID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@DCN", DbType.String, dcn)
            DB.AddOutParameter(DBCommandWrapper, "@CLAIMIDCOUNT", DbType.Int32, 1)
            DB.AddOutParameter(DBCommandWrapper, "@DCNCOUNT", DbType.Int32, 1)

            If transaction Is Nothing Then
                DB.ExecuteReader(DBCommandWrapper)
            Else
                DB.ExecuteReader(DBCommandWrapper, transaction)
            End If

            Try

                Dim ClaimsCount As Integer = CInt(DB.GetParameterValue(DBCommandWrapper, "@CLAIMIDCOUNT"))
                Dim DCNCount As Integer = CInt(DB.GetParameterValue(DBCommandWrapper, "@DCNCOUNT"))

                ' Count value of -1 = no search criteria used
                If ClaimsCount < 0 AndAlso DCNCount = 0 Then 'no claim search, no DCN match
                    Return 6
                ElseIf ClaimsCount > 0 AndAlso DCNCount > 0 Then 'Match on both
                    Return 5
                ElseIf ClaimsCount < 0 AndAlso DCNCount > 0 Then 'no claim search
                    Return 4
                ElseIf ClaimsCount > 0 AndAlso DCNCount < 0 Then ' no dcn search
                    Return 3
                ElseIf ClaimsCount = 0 AndAlso DCNCount > 0 Then 'claims match
                    Return 2
                ElseIf ClaimsCount > 0 AndAlso DCNCount = 0 Then 'no dcn match
                    Return 1
                ElseIf ClaimsCount = 0 AndAlso DCNCount = 0 Then 'no match on either DCN or Claim#
                    Return 0
                End If

            Catch
                Return 0
            End Try

        Catch ex As Exception
            Throw
        Finally
            If DBCommandWrapper IsNot Nothing Then DBCommandWrapper.Dispose()
            DBCommandWrapper = Nothing

        End Try
    End Function

    Public Shared Function GetNextAudit(ByVal userRights As String, ByVal employeeSecurity As Boolean, ByVal docClass As String, ByVal docType As String) As Integer
        Try
            Return UpdateGetNextItemAudit(userRights, employeeSecurity, docClass, docType)

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function UpdateGetNextItemAudit(ByRef transaction As DbTransaction, ByVal userRights As String, ByVal employeeSecurity As Boolean, ByVal docClass As String, ByVal docType As String) As Integer
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.UPDATE_CLAIM_MASTER_GETNEXT_AUDIT"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@SECURITY_SW", DbType.Decimal, If(employeeSecurity = True, 1, 0))
            DB.AddInParameter(DBCommandWrapper, "@DOC_CLASS", DbType.String, docClass)
            DB.AddInParameter(DBCommandWrapper, "@DOC_TYPE", DbType.String, docType)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@BUSY", DbType.Decimal, 1)
            DB.AddOutParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, 11)

            'dbCommandWrapper.CommandTimeout = 20

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

            If Not IsDBNull(DBCommandWrapper.Parameters.Item("@CLAIM_ID").Value) Then
                Return CInt(DBCommandWrapper.Parameters.Item("@CLAIM_ID").Value)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function UpdateGetNextItemAudit(ByVal userRights As String, ByVal employeeSecurity As Boolean, ByVal docClass As String, ByVal docType As String) As Integer
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.UPDATE_CLAIM_MASTER_GETNEXT_AUDIT"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@SECURITY_SW", DbType.Decimal, If(employeeSecurity = True, 1, 0))
            DB.AddInParameter(DBCommandWrapper, "@DOC_CLASS", DbType.String, docClass)
            DB.AddInParameter(DBCommandWrapper, "@DOC_TYPE", DbType.String, docType)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@BUSY", DbType.Decimal, 1)
            DB.AddOutParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, 11)

            'dbCommandWrapper.CommandTimeout = 20

            DB.ExecuteNonQuery(DBCommandWrapper)

            If Not IsDBNull(DBCommandWrapper.Parameters.Item("@CLAIM_ID").Value) Then
                Return CInt(DBCommandWrapper.Parameters.Item("@CLAIM_ID").Value)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveAssociatedClaims(ByVal claimID As Integer, ByVal auditMode As Boolean) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim DS As DataSet

        Dim Tablenames() As String = {"RESULTS", "MEDDTL", "MEDDIAG", "REASON", "MEDMOD"}
        Dim SQLCall As String = "FDBMD.RETRIEVE_ASSOCIATED" & If(auditMode, "_AUDIT", "")

        Try

            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)

            DBCommandWrapper.CommandTimeout = 0

            DS = New DataSet
            DB.LoadDataSet(DBCommandWrapper, DS, Tablenames)

            Return DS

        Catch ex As Exception
            Throw
        Finally
            If DS IsNot Nothing Then DS.Dispose()
        End Try
    End Function

    Public Shared Function RetrieveCompleteClaim(ByVal claimID As Integer, ByRef ClaimCount As Integer, ByVal tableNames() As String, Optional ByVal ds As DataSet = Nothing, Optional ByVal userRights As String = "", Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_COMPLETE_CLAIM_RX"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddOutParameter(DBCommandWrapper, "@CLAIM_COUNT", DbType.Int32, 11)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, tableNames)
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, tableNames, transaction)
                End If
            End If

            ClaimCount = CInt(DBCommandWrapper.Parameters.Item("@CLAIM_COUNT").Value)

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveCompleteClaimBatch(ByVal claimID As Integer, ByRef ClaimCount As Integer, ByVal tableNames() As String, Optional ByVal ds As DataSet = Nothing, Optional ByVal userRights As String = "", Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_COMPLETE_CLAIM_BATCH"

        Try

            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddOutParameter(DBCommandWrapper, "@CLAIM_COUNT", DbType.Int32, 11)
            DBCommandWrapper.CommandTimeout = 0

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, tableNames)
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, tableNames, transaction)
                End If
            End If

            ClaimCount = CInt(DBCommandWrapper.Parameters.Item("@CLAIM_COUNT").Value)

            Return ds

        Catch ex As Exception
            Throw
        Finally

            DB = Nothing

        End Try
    End Function

    Public Shared Function RetrieveClaimByPricingID(ByVal pricingID As Double, ByVal tableNames() As String, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_BC_COMPLETE_CLAIM"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PRICING_ID", DbType.Double, pricingID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, tableNames)
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, tableNames, transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveRemitCandidates(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_HIPAA_REMIT_CANDIDATES"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DBCommandWrapper.CommandTimeout = 0

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
                ds.Tables(0).TableName = "CLAIM_MASTER"
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "CLAIM_MASTER")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "CLAIM_MASTER", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveRemitCandidatesByBatch(batch As String, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_HIPAA_REMIT_CANDIDATES_BY_SOURCE_837"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DBCommandWrapper.CommandTimeout = 0
            DB.AddInParameter(DBCommandWrapper, "@BATCH", DbType.String, batch)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
                ds.Tables(0).TableName = "CLAIM_MASTER"
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "CLAIM_MASTER")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "CLAIM_MASTER", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveRemitCandidate(ByVal claimID As Integer, ByVal lastUPDT As DateTime, ByVal tableNames() As String, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_HIPAA_REMIT_CANDIDATE_BY_CLAIMID"

        Try

            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@LASTUPDT", DbType.DateTime, UFCWGeneral.ToNullDateHandler(lastUPDT))

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If

                Dim tableNumber As Integer = 0
                For Each DT As DataTable In ds.Tables
                    DT.TableName = tableNames(tableNumber)
                    tableNumber += 1
                Next
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, tableNames)
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, tableNames, transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveClaimWithUpdate(ByVal claimID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal userRights As String = "", Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim Tablenames() As String = {"CLAIM_MASTER", "MEDHDR", "ANNOTATIONS"}

        Dim SQLCall As String = "FDBMD.RETRIEVE_COMPLETE_CLAIM_BY_CLAIMID_WITH_UPDATE"

        Try

            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, Tablenames)
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, Tablenames, transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveClaim(ByVal claimID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal userRights As String = "", Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_CLAIM_MASTER_BY_CLAIMID_WITH_UPDATE"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "CLAIM_MASTER")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "CLAIM_MASTER", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Sub ReopenClaim(ByVal claimID As Integer, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.REOPEN_CLAIM"

        Try

            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub PendToUser(ByVal claimID As Integer, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.UPDATE_CLAIM_MASTER_PENDEDTO"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub PendToUser(ByVal claimID As Integer, ByVal userRights As String, ByVal lastUpdatedt As Date, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.UPDATE_CLAIM_MASTER_FOR_ROUTE"

        Try

            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@LASTUPDT", DbType.DateTime, UFCWGeneral.ToNullDateHandler(lastUpdatedt))

            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception When ex.Number = -438

            ex.HelpLink = "Row Not Found"
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub PendToAuditor(ByVal claimID As Integer, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.UPDATE_CLAIM_MASTER_AUDITEDBY"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub UnPendUser(ByVal claimID As Integer, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.UPDATE_CLAIM_MASTER_UNPEND"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub BusyItem(ByVal claimID As Integer, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.UPDATE_CLAIM_MASTER_BUSYITEM"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@BUSY", DbType.Decimal, 1)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Function BulkUpdateClaimToTaxID(ByVal claimID As Integer, ByVal claimLastUpdated As Date?, ByVal updatedBy As String, ByVal docType As String, ByVal taxID As String, ByVal updatedTaxID As String, ByVal provID As String, ByVal updatedProvID As String, ByVal transactionType As String, Optional ByVal transaction As DbTransaction = Nothing) As Boolean
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Dim SQLCall As String = "FDBMD.BULK_UPDATE_ARCHIVE"
        Dim HistSum As String = "CLAIM# " & Format(claimID, "00000000") & " UPDATED TaxID From: " & updatedTaxID & " To: " & taxID & " ;ProvID From: " & updatedProvID & " To: " & provID
        Dim bulkFrom, bulkTo As String
        bulkFrom = "TaxID= " & updatedTaxID & " ;ProvID= " & updatedProvID
        bulkTo = "TaxID= " & taxID & " ;ProvID= " & provID
        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)


            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@USERID", DbType.String, updatedBy)
            DB.AddInParameter(DBCommandWrapper, "@BULK_FROM", DbType.String, bulkFrom)
            DB.AddInParameter(DBCommandWrapper, "@BULK_TO", DbType.String, bulkTo)
            DB.AddInParameter(DBCommandWrapper, "@PROV_TIN", DbType.Int32, CInt(taxID))
            DB.AddInParameter(DBCommandWrapper, "@PROV_ID", DbType.Int32, CInt(provID))
            DB.AddInParameter(DBCommandWrapper, "@DOC_TYPE", DbType.String, docType)
            DB.AddInParameter(DBCommandWrapper, "@LASTUPDT", DbType.DateTime, UFCWGeneral.ToNullDateHandler(claimLastUpdated))
            DB.AddInParameter(DBCommandWrapper, "@TRANSACTION_TYPE", DbType.String, transactionType)
            DB.AddInParameter(DBCommandWrapper, "@SUMMARY", DbType.String, Replace(HistSum, vbCrLf, "\N"))
            DB.AddInParameter(DBCommandWrapper, "@DETAIL", DbType.String, Replace("TaxID= " & taxID & ",ProvID= " & provID, vbCrLf, "\N"))
            DB.AddInParameter(DBCommandWrapper, "@UPDATED_DETAIL", DbType.String, docType)
            DB.AddInParameter(DBCommandWrapper, "@BULKUPDATE_TYPE", DbType.String, "TAXID")

            If transaction IsNot Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper)
            End If

            Return True

        Catch ex As Exception
            Throw
        End Try


    End Function
    Public Shared Function BulkUpdateClaimToDocType(ByVal claimID As Integer, ByVal claimLastUpdated As Date?, ByVal updatedBy As String, ByVal bulkFrom As String, ByVal bulkTo As String, ByVal docType As String, ByVal updatedDocType As String, ByVal transactionType As String, Optional ByVal transaction As DbTransaction = Nothing) As Boolean   'ByVal pendedTo As String, ByVal routingComment As String,
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand


        Dim SQLCall As String = "FDBMD.BULK_UPDATE_ARCHIVE "
        Dim HistSum As String = "CLAIM# " & Format(claimID, "00000000") & " UPDATED Doc Type From: " & docType & " To: " & updatedDocType
        Try


            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@USERID", DbType.String, updatedBy)
            DB.AddInParameter(DBCommandWrapper, "@BULK_FROM", DbType.String, "DOCTYPE From: " & bulkFrom)
            DB.AddInParameter(DBCommandWrapper, "@BULK_TO", DbType.String, "DOCTYPE To: " & bulkTo)
            DB.AddInParameter(DBCommandWrapper, "@PROV_TIN", DbType.Int32, 0)
            DB.AddInParameter(DBCommandWrapper, "@PROV_ID", DbType.Int32, 0)
            DB.AddInParameter(DBCommandWrapper, "@DOC_TYPE", DbType.String, docType)
            DB.AddInParameter(DBCommandWrapper, "@LASTUPDT", DbType.DateTime, UFCWGeneral.ToNullDateHandler(claimLastUpdated))
            DB.AddInParameter(DBCommandWrapper, "@TRANSACTION_TYPE", DbType.String, transactionType)
            DB.AddInParameter(DBCommandWrapper, "@SUMMARY", DbType.String, Replace(HistSum, vbCrLf, "\N"))
            DB.AddInParameter(DBCommandWrapper, "@DETAIL", DbType.String, Replace(docType, vbCrLf, "\N"))
            DB.AddInParameter(DBCommandWrapper, "@UPDATED_DETAIL", DbType.String, Replace(updatedDocType, vbCrLf, "\N"))
            DB.AddInParameter(DBCommandWrapper, "@BULKUPDATE_TYPE", DbType.String, "DOCTYPE")


            If transaction IsNot Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper)
            End If

            Return True

        Catch ex As Exception
            Throw
        End Try


    End Function
    Public Shared Function BulkArchive(ByVal claimID As Integer, ByVal claimLastUpdated As Date?, ByVal updatedBy As String, ByVal bulkFrom As String, ByVal bulkTo As String, ByVal docType As String, ByVal transactionType As String, Optional ByVal transaction As DbTransaction = Nothing) As Boolean
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Dim SQLCall As String = "FDBMD.BULK_UPDATE_ARCHIVE "
        Dim HistSum As String = "CLAIM ID " & Format(claimID, "00000000") & " HAS BEEN ARCHIVED OUT OF THE CURRENT INVENTORY"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@USERID", DbType.String, updatedBy)
            DB.AddInParameter(DBCommandWrapper, "@BULK_FROM", DbType.String, bulkFrom)
            DB.AddInParameter(DBCommandWrapper, "@BULK_TO", DbType.String, bulkTo)
            DB.AddInParameter(DBCommandWrapper, "@PROV_TIN", DbType.Int32, 0)
            DB.AddInParameter(DBCommandWrapper, "@PROV_ID", DbType.Int32, 0)
            DB.AddInParameter(DBCommandWrapper, "@DOC_TYPE", DbType.String, docType)
            DB.AddInParameter(DBCommandWrapper, "@LASTUPDT", DbType.DateTime, UFCWGeneral.ToNullDateHandler(claimLastUpdated))
            DB.AddInParameter(DBCommandWrapper, "@TRANSACTION_TYPE", DbType.String, transactionType)
            DB.AddInParameter(DBCommandWrapper, "@SUMMARY", DbType.String, Replace(HistSum, vbCrLf, "\N"))
            DB.AddInParameter(DBCommandWrapper, "@DETAIL", DbType.String, updatedBy & " ARCHIVE THIS ITEMS.")
            DB.AddInParameter(DBCommandWrapper, "@UPDATED_DETAIL", DbType.String, docType)
            DB.AddInParameter(DBCommandWrapper, "@BULKUPDATE_TYPE", DbType.String, "ARCHIVE")

            If transaction IsNot Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper)
            End If

            Return True

        Catch ex As Exception
            Throw
        End Try


    End Function
    Public Shared Sub UpdateOptumRxOOPAccum(ByVal entryID As Integer, flagAsError As Decimal?, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.UPDATE_OPTUMRX_OOP_ACCUM_ENTRY"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@ENTRY_ID", DbType.Int32, entryID)
            DB.AddInParameter(DBCommandWrapper, "@REPORT_AS_ERROR_SW", DbType.Decimal, flagAsError)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub CloseUHCOOPAccum(ByVal maxCREATE_DATE As Date?, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CLOSE_UHC_DAILY_OOP_ENTRIES"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DBCommandWrapper.CommandTimeout = 0
            DB.AddInParameter(DBCommandWrapper, "@CREATE_DATE", DbType.Date, maxCREATE_DATE)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub UpdateUHCOOPAccum(ByVal entryID As Integer, flagAsError As Decimal?, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.UPDATE_UHC_OOP_ACCUM_ENTRY"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@ENTRY_ID", DbType.Int32, entryID)
            DB.AddInParameter(DBCommandWrapper, "@REPORT_AS_ERROR_SW", DbType.Decimal, flagAsError)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub UnBusyItem(ByVal claimID As Integer, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.UPDATE_CLAIM_MASTER_BUSYITEM"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@BUSY", DbType.Decimal, 0)

            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Function RetrieveFamilyLock(ByVal familyID As Integer, Optional ByVal transaction As DbTransaction = Nothing) As DataTable

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_CLAIM_LOCKS_BY_FAMILYID"
        Dim DS As DataSet

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)

            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
            End If

            If DS.Tables.Count > 0 Then
                DS.Tables(0).TableName = "CLAIM_LOCKS"
                Return DS.Tables(0)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Sub InsertFamilyLock(ByVal familyID As Integer, ByVal relationID As Short?, ByVal claimID As Integer, ByVal userRights As String, ByVal workStation As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.INSERT_CLAIM_LOCKS"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.FamilyID = familyID.ToString.Trim
            UFCWLastKeyData.RelationID = relationID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, If(relationID, 0S))
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@WORKSTATION", DbType.String, workStation)

            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub ReleaseFamilyLock(ByVal familyID As Integer, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RELEASE_CLAIM_LOCKS"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)

            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Function RetrieveHeader(ByVal claimID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_MEDHDR_BY_CLAIMID"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "MEDHDR")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "MEDHDR", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveDetail(ByVal claimID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_MEDDTL_BY_CLAIMID"

        Try

            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "MEDDTL")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "MEDDTL", transaction)
                End If
            End If

            Return ds
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveDetailWithRules(ByVal claimID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_MEDDTL_AND_RULES_BY_CLAIMID"

        Try

            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "MEDDTL")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "MEDDTL", transaction)
                End If
            End If

            Return ds
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveAccidents() As DataTable
        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_ACCIDENT_VALUES"
        Dim DT As DataTable

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DS = DB.ExecuteDataSet(DBCommandWrapper)
            DT = New DataTable("ACCIDENT_VALUES")

            If DS.Tables.Count > 0 Then
                DT = DS.Tables(0)
            End If

            Return DT

        Catch ex As Exception
            Throw
        Finally
            If DS IsNot Nothing Then DS.Dispose()
            If DT IsNot Nothing Then DT.Dispose()
        End Try
    End Function

    Public Shared Sub UpdateClaimLocks(ByVal claimID As Integer, ByVal familyID As Integer, ByVal relationID As Short, ByVal userRights As String, ByVal workStation As String, ByRef transaction As DbTransaction)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.UPDATE_CLAIM_LOCKS"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.FamilyID = familyID.ToString.Trim
            UFCWLastKeyData.RelationID = relationID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@WORKSTATION", DbType.String, workStation)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub UpdateClaimMasterAndComplete(ByVal claimID As Integer, ByVal partSSN As Integer?, ByVal patSSN As Integer?, ByVal familyID As Integer, ByVal relationID As Short, ByVal priority As Short,
                                                   ByVal securitySW As Decimal, ByVal attachSW As Decimal, ByVal duplicateSW As Decimal, ByVal busySW As Decimal, ByVal status As String, ByVal statusDate As Date?,
                                                   ByVal docClass As String, ByVal docType As String, ByVal dateOfService As Date?, ByVal patFName As String, ByVal patInt As String, ByVal patLName As String,
                                                   ByVal partFName As String, ByVal partInt As String, ByVal partLName As String, ByVal provTIN As Integer?, ByVal provID As Integer?, ByVal referenceClaim As Integer?,
                                                   ByVal auditBy As String, ByVal userRights As String, ByRef transaction As DbTransaction)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.UPDATE_CLAIM_MASTER_AND_COMPLETE"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.FamilyID = familyID.ToString.Trim
            UFCWLastKeyData.RelationID = relationID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, partSSN)
            DB.AddInParameter(DBCommandWrapper, "@PAT_SSN", DbType.Int32, patSSN)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@PRIORITY", DbType.Int32, priority)
            DB.AddInParameter(DBCommandWrapper, "@SECURITY_SW", DbType.Decimal, securitySW)
            DB.AddInParameter(DBCommandWrapper, "@ATTACH_SW", DbType.Decimal, attachSW)
            DB.AddInParameter(DBCommandWrapper, "@DUPLICATE_SW", DbType.Decimal, duplicateSW)
            DB.AddInParameter(DBCommandWrapper, "@BUSY_SW", DbType.Decimal, busySW)
            DB.AddInParameter(DBCommandWrapper, "@STATUS", DbType.String, status)
            DB.AddInParameter(DBCommandWrapper, "@STATUS_DATE", DbType.DateTime, UFCWGeneral.ToNullDateHandler(statusDate))
            DB.AddInParameter(DBCommandWrapper, "@DOC_CLASS", DbType.String, docClass)
            DB.AddInParameter(DBCommandWrapper, "@DOC_TYPE", DbType.String, docType)
            DB.AddInParameter(DBCommandWrapper, "@DATE_OF_SERVICE", DbType.Date, UFCWGeneral.ToNullDateHandler(dateOfService))
            DB.AddInParameter(DBCommandWrapper, "@PAT_FNAME", DbType.String, patFName)
            DB.AddInParameter(DBCommandWrapper, "@PAT_INT", DbType.String, patInt)
            DB.AddInParameter(DBCommandWrapper, "@PAT_LNAME", DbType.String, patLName)
            DB.AddInParameter(DBCommandWrapper, "@PART_FNAME", DbType.String, partFName)
            DB.AddInParameter(DBCommandWrapper, "@PART_INT", DbType.String, partInt)
            DB.AddInParameter(DBCommandWrapper, "@PART_LNAME", DbType.String, partLName)
            DB.AddInParameter(DBCommandWrapper, "@PROV_TIN", DbType.Int32, provTIN)
            DB.AddInParameter(DBCommandWrapper, "@PROV_ID", DbType.Int32, provID)
            DB.AddInParameter(DBCommandWrapper, "@REFRENCE_CLAIM", DbType.Int32, referenceClaim)
            DB.AddInParameter(DBCommandWrapper, "@PENDED_TO", DbType.String, auditBy)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub UpdateClaimMaster(ByVal claimID As Integer, ByVal partSSN As Integer?, ByVal patSSN As Integer?, ByVal familyID As Integer, ByVal relationID As Short, ByVal priority As Short,
                                        ByVal securitySW As Decimal, ByVal attachSW As Decimal, ByVal duplicateSW As Decimal, ByVal busySW As Decimal, ByVal status As String, ByVal statusDate As Date?,
                                        ByVal docClass As String, ByVal docType As String, ByVal dateOfService As Date?, ByVal patFName As String, ByVal patInt As String, ByVal patLName As String,
                                        ByVal partFName As String, ByVal partInt As String, ByVal partLName As String, ByVal provTIN As Integer?, ByVal provID As Integer?, ByVal referenceClaim As Integer?,
                                        ByVal userRights As String, ByRef transaction As DbTransaction)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.UPDATE_CLAIM_MASTER"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.FamilyID = familyID.ToString.Trim
            UFCWLastKeyData.RelationID = relationID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, partSSN)
            DB.AddInParameter(DBCommandWrapper, "@PAT_SSN", DbType.Int32, patSSN)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@PRIORITY", DbType.Int16, priority)
            DB.AddInParameter(DBCommandWrapper, "@SECURITY_SW", DbType.Decimal, securitySW)
            DB.AddInParameter(DBCommandWrapper, "@ATTACH_SW", DbType.Decimal, attachSW)
            DB.AddInParameter(DBCommandWrapper, "@DUPLICATE_SW", DbType.Decimal, duplicateSW)
            DB.AddInParameter(DBCommandWrapper, "@BUSY_SW", DbType.Decimal, busySW)
            DB.AddInParameter(DBCommandWrapper, "@STATUS", DbType.String, status)
            DB.AddInParameter(DBCommandWrapper, "@STATUS_DATE", DbType.DateTime, UFCWGeneral.ToNullDateHandler(statusDate))
            DB.AddInParameter(DBCommandWrapper, "@DOC_CLASS", DbType.String, docClass)
            DB.AddInParameter(DBCommandWrapper, "@DOC_TYPE", DbType.String, docType)
            DB.AddInParameter(DBCommandWrapper, "@DATE_OF_SERVICE", DbType.Date, UFCWGeneral.ToNullDateHandler(dateOfService))
            DB.AddInParameter(DBCommandWrapper, "@PAT_FNAME", DbType.String, patFName)
            DB.AddInParameter(DBCommandWrapper, "@PAT_INT", DbType.String, patInt)
            DB.AddInParameter(DBCommandWrapper, "@PAT_LNAME", DbType.String, patLName)
            DB.AddInParameter(DBCommandWrapper, "@PART_FNAME", DbType.String, partFName)
            DB.AddInParameter(DBCommandWrapper, "@PART_INT", DbType.String, partInt)
            DB.AddInParameter(DBCommandWrapper, "@PART_LNAME", DbType.String, partLName)
            DB.AddInParameter(DBCommandWrapper, "@PROV_TIN", DbType.Int64, provTIN)
            DB.AddInParameter(DBCommandWrapper, "@PROV_ID", DbType.Int32, provID)
            DB.AddInParameter(DBCommandWrapper, "@REFRENCE_CLAIM", DbType.Int32, referenceClaim)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Function CreateMEDOTHER_INS_Return_InsId(ByVal medother_INS_ID As Integer, ByVal familyID As Integer, ByVal relationID As Short,
                                        ByVal occ_From_Date As Date?, ByVal occ_To_Date As Date?, ByVal working_SPOUSE_SW As Decimal,
                                        ByVal oth_INS_REFUSAL_SW As Decimal, ByVal oth_PAT_ACCT_NBR As String, ByVal hicn As String, ByVal oth_POLICY As String,
                                        ByVal oth_TAXID As Integer?, ByVal oth_PAYER As String, ByVal oth_SSN As Integer?,
                                        ByVal oth_FName As String, ByVal oth_LName As String, ByVal oth_SEX As String,
                                        ByVal oth_RELATION As String, ByVal oth_DOB As Date?, ByVal oth_PAYER_ID As Integer?,
                                        ByVal oth_COMMENTS As String, ByVal update_REASON As String, ByVal documentID As Long?,
                                        ByVal oth_SUB_ACCT_NBR As String, ByVal address_LINE1 As String, ByVal address_LINE2 As String,
                                        ByVal city As String, ByVal sTATE As String, ByVal zip As Decimal?, ByVal zip_4 As Decimal?,
                                        ByVal country As String, ByVal eMAIL As String, ByVal phone As Decimal?, ByVal extension1 As Integer?,
                                        ByVal contact1 As String, ByVal userRights As String, ByRef transaction As DbTransaction) As Integer

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        'Dim SQLCall As String = "FDBMD.CREATE_MEDOTHER_INSURANCE"
        Dim SQLCall As String = "FDBMD.CREATE_MEDOTHER_INSURANCE_RETURN_INS_ID"
        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@OCC_FROM_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(occ_From_Date))
            DB.AddInParameter(DBCommandWrapper, "@OCC_TO_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(occ_To_Date))
            DB.AddInParameter(DBCommandWrapper, "@WORKING_SPOUSE_SW", DbType.Decimal, working_SPOUSE_SW)
            DB.AddInParameter(DBCommandWrapper, "@OTH_INS_REFUSAL_SW", DbType.Decimal, oth_INS_REFUSAL_SW)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PAT_ACCT_NBR", DbType.String, oth_PAT_ACCT_NBR)
            DB.AddInParameter(DBCommandWrapper, "@HICN", DbType.String, hicn)
            DB.AddInParameter(DBCommandWrapper, "@OTH_POLICY", DbType.String, oth_POLICY)
            DB.AddInParameter(DBCommandWrapper, "@OTH_TAXID", DbType.Int32, oth_TAXID)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PAYER_ID", DbType.Int32, oth_PAYER_ID)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PAYER", DbType.String, oth_PAYER)
            DB.AddInParameter(DBCommandWrapper, "@OTH_SSN", DbType.Int32, oth_SSN)
            DB.AddInParameter(DBCommandWrapper, "@OTH_FNAME", DbType.String, oth_FName)
            DB.AddInParameter(DBCommandWrapper, "@OTH_LNAME", DbType.String, oth_LName)
            DB.AddInParameter(DBCommandWrapper, "@OTH_SEX", DbType.String, oth_SEX)
            DB.AddInParameter(DBCommandWrapper, "@OTH_RELATION", DbType.String, oth_RELATION)
            DB.AddInParameter(DBCommandWrapper, "@OTH_DOB", DbType.Date, UFCWGeneral.ToNullDateHandler(oth_DOB))
            DB.AddInParameter(DBCommandWrapper, "@OTH_COMMENTS", DbType.String, oth_COMMENTS)
            DB.AddInParameter(DBCommandWrapper, "@UPDATE_REASON", DbType.String, update_REASON)
            DB.AddInParameter(DBCommandWrapper, "@DOCID", DbType.Decimal, documentID)
            DB.AddInParameter(DBCommandWrapper, "@OTH_SUB_ACCT_NBR", DbType.String, oth_SUB_ACCT_NBR)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_LINE1", DbType.String, address_LINE1)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_LINE2", DbType.String, address_LINE2)
            DB.AddInParameter(DBCommandWrapper, "@CITY", DbType.String, city)
            DB.AddInParameter(DBCommandWrapper, "@STATE", DbType.String, sTATE)
            DB.AddInParameter(DBCommandWrapper, "@ZIP", DbType.Decimal, zip)
            DB.AddInParameter(DBCommandWrapper, "@ZIP_4", DbType.Decimal, zip_4)
            DB.AddInParameter(DBCommandWrapper, "@COUNTRY", DbType.String, country)
            DB.AddInParameter(DBCommandWrapper, "@EMAIL", DbType.String, eMAIL)
            DB.AddInParameter(DBCommandWrapper, "@PHONE", DbType.Decimal, phone)
            DB.AddInParameter(DBCommandWrapper, "@EXTENSION1", DbType.Int32, extension1)
            DB.AddInParameter(DBCommandWrapper, "@CONTACT1", DbType.String, contact1)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddOutParameter(DBCommandWrapper, "@MEDOTHER_INS_ID", DbType.Int32, 11)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

            Return CInt(DB.GetParameterValue(DBCommandWrapper, "@MEDOTHER_INS_ID"))
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Sub CreateMEDOTHER_INS(ByVal medother_INS_ID As Integer, ByVal familyID As Integer, ByVal relationID As Short,
                                        ByVal occ_From_Date As Date?, ByVal occ_To_Date As Date?, ByVal working_SPOUSE_SW As Decimal,
                                        ByVal oth_INS_REFUSAL_SW As Decimal, ByVal oth_PAT_ACCT_NBR As String, ByVal hicn As String, ByVal oth_POLICY As String,
                                        ByVal oth_TAXID As Integer?, ByVal oth_PAYER As String, ByVal oth_SSN As Integer?,
                                        ByVal oth_FName As String, ByVal oth_LName As String, ByVal oth_SEX As String,
                                        ByVal oth_RELATION As String, ByVal oth_DOB As Date?, ByVal oth_PAYER_ID As Integer?,
                                        ByVal oth_COMMENTS As String, ByVal update_REASON As String, ByVal documentID As Long?,
                                        ByVal oth_SUB_ACCT_NBR As String, ByVal address_LINE1 As String, ByVal address_LINE2 As String,
                                        ByVal city As String, ByVal sTATE As String, ByVal zip As Decimal?, ByVal zip_4 As Decimal?,
                                        ByVal country As String, ByVal eMAIL As String, ByVal phone As Decimal?, ByVal extension1 As Integer?,
                                        ByVal contact1 As String, ByVal userRights As String, ByRef transaction As DbTransaction)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_MEDOTHER_INSURANCE"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@OCC_FROM_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(occ_From_Date))
            DB.AddInParameter(DBCommandWrapper, "@OCC_TO_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(occ_To_Date))
            DB.AddInParameter(DBCommandWrapper, "@WORKING_SPOUSE_SW", DbType.Decimal, working_SPOUSE_SW)
            DB.AddInParameter(DBCommandWrapper, "@OTH_INS_REFUSAL_SW", DbType.Decimal, oth_INS_REFUSAL_SW)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PAT_ACCT_NBR", DbType.String, oth_PAT_ACCT_NBR)
            DB.AddInParameter(DBCommandWrapper, "@HICN", DbType.String, hicn)
            DB.AddInParameter(DBCommandWrapper, "@OTH_POLICY", DbType.String, oth_POLICY)
            DB.AddInParameter(DBCommandWrapper, "@OTH_TAXID", DbType.Int32, oth_TAXID)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PAYER_ID", DbType.Int32, oth_PAYER_ID)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PAYER", DbType.String, oth_PAYER)
            DB.AddInParameter(DBCommandWrapper, "@OTH_SSN", DbType.Int32, oth_SSN)
            DB.AddInParameter(DBCommandWrapper, "@OTH_FNAME", DbType.String, oth_FName)
            DB.AddInParameter(DBCommandWrapper, "@OTH_LNAME", DbType.String, oth_LName)
            DB.AddInParameter(DBCommandWrapper, "@OTH_SEX", DbType.String, oth_SEX)
            DB.AddInParameter(DBCommandWrapper, "@OTH_RELATION", DbType.String, oth_RELATION)
            DB.AddInParameter(DBCommandWrapper, "@OTH_DOB", DbType.Date, UFCWGeneral.ToNullDateHandler(oth_DOB))
            DB.AddInParameter(DBCommandWrapper, "@OTH_COMMENTS", DbType.String, oth_COMMENTS)
            DB.AddInParameter(DBCommandWrapper, "@UPDATE_REASON", DbType.String, update_REASON)
            DB.AddInParameter(DBCommandWrapper, "@DOCID", DbType.Decimal, documentID)
            DB.AddInParameter(DBCommandWrapper, "@OTH_SUB_ACCT_NBR", DbType.String, oth_SUB_ACCT_NBR)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_LINE1", DbType.String, address_LINE1)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_LINE2", DbType.String, address_LINE2)
            DB.AddInParameter(DBCommandWrapper, "@CITY", DbType.String, city)
            DB.AddInParameter(DBCommandWrapper, "@STATE", DbType.String, sTATE)
            DB.AddInParameter(DBCommandWrapper, "@ZIP", DbType.Decimal, zip)
            DB.AddInParameter(DBCommandWrapper, "@ZIP_4", DbType.Decimal, zip_4)
            DB.AddInParameter(DBCommandWrapper, "@COUNTRY", DbType.String, country)
            DB.AddInParameter(DBCommandWrapper, "@EMAIL", DbType.String, eMAIL)
            DB.AddInParameter(DBCommandWrapper, "@PHONE", DbType.Decimal, phone)
            DB.AddInParameter(DBCommandWrapper, "@EXTENSION1", DbType.Int32, extension1)
            DB.AddInParameter(DBCommandWrapper, "@CONTACT1", DbType.String, contact1)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Function UpdateMEDOTHER_INS(ByVal medother_INS_ID As Integer, ByVal familyID As Integer, ByVal relationID As Short,
                                                ByVal occ_FROM_Date As Date?, ByVal occ_TO_Date As Date?, ByVal working_SPOUSE_SW As Decimal,
                                                ByVal oth_INS_REFUSAL_SW As Decimal, ByVal oth_PAT_ACCT_NBR As String, ByVal hicn As String, ByVal oth_Policy As String,
                                                ByVal oth_TAXID As Integer?, ByVal oth_PAYER As String, ByVal oth_SSN As Integer?,
                                                ByVal oth_FName As String, ByVal oth_LName As String, ByVal oth_SEX As String,
                                                ByVal oth_RELATION As String, ByVal oth_DOB As Date?, ByVal oth_PAYER_ID As Integer?,
                                                ByVal oth_COMMENTS As String, ByVal update_REASON As String, ByVal documentID As Decimal?,
                                                ByVal oth_SUB_ACCT_NBR As String, ByVal address_LINE1 As String, ByVal address_LINE2 As String,
                                                ByVal city As String, ByVal sTATE As String, ByVal zip As Decimal?, ByVal zip_4 As Decimal?,
                                                ByVal country As String, ByVal eMAIL As String, ByVal phone As Decimal?, ByVal extension1 As Integer?,
                                                ByVal contact1 As String, ByVal userRights As String, ByVal lastUPDT As Date?, ByRef transaction As DbTransaction) As DateTime

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.UPDATE_MEDOTHER_INSURANCE_RETURN_LASTUPDT"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@MEDOTHER_INS_ID", DbType.Int32, medother_INS_ID)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@OCC_FROM_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(occ_FROM_Date))
            DB.AddInParameter(DBCommandWrapper, "@OCC_TO_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(occ_TO_Date))
            DB.AddInParameter(DBCommandWrapper, "@WORKING_SPOUSE_SW", DbType.Decimal, working_SPOUSE_SW)
            DB.AddInParameter(DBCommandWrapper, "@OTH_INS_REFUSAL_SW", DbType.Decimal, oth_INS_REFUSAL_SW)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PAT_ACCT_NBR", DbType.String, oth_PAT_ACCT_NBR)
            DB.AddInParameter(DBCommandWrapper, "@HICN", DbType.String, hicn)
            DB.AddInParameter(DBCommandWrapper, "@OTH_POLICY", DbType.String, oth_Policy)
            DB.AddInParameter(DBCommandWrapper, "@OTH_TAXID", DbType.Int32, oth_TAXID)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PAYER_ID", DbType.Int32, oth_PAYER_ID)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PAYER", DbType.String, oth_PAYER)
            DB.AddInParameter(DBCommandWrapper, "@OTH_SSN", DbType.Int32, oth_SSN)
            DB.AddInParameter(DBCommandWrapper, "@OTH_FNAME", DbType.String, oth_FName)
            DB.AddInParameter(DBCommandWrapper, "@OTH_LNAME", DbType.String, oth_LName)
            DB.AddInParameter(DBCommandWrapper, "@OTH_SEX", DbType.String, oth_SEX)
            DB.AddInParameter(DBCommandWrapper, "@OTH_RELATION", DbType.String, oth_RELATION)
            DB.AddInParameter(DBCommandWrapper, "@OTH_DOB", DbType.Date, UFCWGeneral.ToNullDateHandler(oth_DOB))
            DB.AddInParameter(DBCommandWrapper, "@OTH_COMMENTS", DbType.String, oth_COMMENTS)
            DB.AddInParameter(DBCommandWrapper, "@UPDATE_REASON", DbType.String, update_REASON)
            DB.AddInParameter(DBCommandWrapper, "@DOCID", DbType.Decimal, documentID)
            DB.AddInParameter(DBCommandWrapper, "@OTH_SUB_ACCT_NBR", DbType.String, oth_SUB_ACCT_NBR)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_LINE1", DbType.String, address_LINE1)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_LINE2", DbType.String, address_LINE2)
            DB.AddInParameter(DBCommandWrapper, "@CITY", DbType.String, city)
            DB.AddInParameter(DBCommandWrapper, "@STATE", DbType.String, sTATE)
            DB.AddInParameter(DBCommandWrapper, "@ZIP", DbType.Decimal, zip)
            DB.AddInParameter(DBCommandWrapper, "@ZIP_4", DbType.Decimal, zip_4)
            DB.AddInParameter(DBCommandWrapper, "@COUNTRY", DbType.String, country)
            DB.AddInParameter(DBCommandWrapper, "@EMAIL", DbType.String, eMAIL)
            DB.AddInParameter(DBCommandWrapper, "@PHONE", DbType.Decimal, phone)
            DB.AddInParameter(DBCommandWrapper, "@EXTENSION1", DbType.Int32, extension1)
            DB.AddInParameter(DBCommandWrapper, "@CONTACT1", DbType.String, contact1)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@LASTUPDT", DbType.DateTime, lastUPDT)
            DB.AddOutParameter(DBCommandWrapper, "@NEWLASTUPDT", DbType.DateTime, 8)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

            Return CDate(DB.GetParameterValue(DBCommandWrapper, "@NEWLASTUPDT"))

        Catch ex As DB2Exception When ex.Number = -438

            Throw New Global.System.Data.DataException("Row is no longer available for update")

        Catch ex As DB2Exception

            Throw

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Sub UpdateClaimMasterWithRoute(ByVal claimID As Integer, ByVal partSSN As Integer?, ByVal patSSN As Integer?, ByVal familyID As Integer, ByVal relationID As Short,
                                                 ByVal priority As Short, ByVal securitySW As Decimal, ByVal attachSW As Decimal, ByVal duplicateSW As Decimal, ByVal busySW As Decimal,
                                                 ByVal status As String, ByVal statusDate As Date?, ByVal docClass As String, ByVal docType As String, ByVal dateOfService As Date?,
                                                 ByVal patFName As String, ByVal patInt As String, ByVal patLName As String, ByVal partFName As String, ByVal partInt As String,
                                                 ByVal partLName As String, ByVal provTIN As Integer?, ByVal provID As Integer?, ByVal refrenceClaim As Integer?, ByVal userRights As String,
                                                 ByRef transaction As DbTransaction)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.UPDATE_CLAIM_MASTER_WITH_ROUTE"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.FamilyID = familyID.ToString.Trim
            UFCWLastKeyData.RelationID = relationID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, partSSN)
            DB.AddInParameter(DBCommandWrapper, "@PAT_SSN", DbType.Int32, patSSN)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@PRIORITY", DbType.Int32, priority)
            DB.AddInParameter(DBCommandWrapper, "@SECURITY_SW", DbType.Decimal, securitySW)
            DB.AddInParameter(DBCommandWrapper, "@ATTACH_SW", DbType.Decimal, attachSW)
            DB.AddInParameter(DBCommandWrapper, "@DUPLICATE_SW", DbType.Decimal, duplicateSW)
            DB.AddInParameter(DBCommandWrapper, "@BUSY_SW", DbType.Decimal, busySW)
            DB.AddInParameter(DBCommandWrapper, "@STATUS", DbType.String, status)
            DB.AddInParameter(DBCommandWrapper, "@STATUS_DATE", DbType.DateTime, UFCWGeneral.ToNullDateHandler(statusDate))
            DB.AddInParameter(DBCommandWrapper, "@DOC_CLASS", DbType.String, docClass)
            DB.AddInParameter(DBCommandWrapper, "@DOC_TYPE", DbType.String, docType)
            DB.AddInParameter(DBCommandWrapper, "@DATE_OF_SERVICE", DbType.Date, UFCWGeneral.ToNullDateHandler(dateOfService))
            DB.AddInParameter(DBCommandWrapper, "@PAT_FNAME", DbType.String, patFName)
            DB.AddInParameter(DBCommandWrapper, "@PAT_INT", DbType.String, patInt)
            DB.AddInParameter(DBCommandWrapper, "@PAT_LNAME", DbType.String, patLName)
            DB.AddInParameter(DBCommandWrapper, "@PART_FNAME", DbType.String, partFName)
            DB.AddInParameter(DBCommandWrapper, "@PART_INT", DbType.String, partInt)
            DB.AddInParameter(DBCommandWrapper, "@PART_LNAME", DbType.String, partLName)
            DB.AddInParameter(DBCommandWrapper, "@PROV_TIN", DbType.Int64, provTIN)
            DB.AddInParameter(DBCommandWrapper, "@PROV_ID", DbType.Int32, provID)
            DB.AddInParameter(DBCommandWrapper, "@REFRENCE_CLAIM", DbType.Int32, refrenceClaim)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub UpdateMEDHDR(ByVal claimID As Integer, ByVal securitySW As Decimal, ByVal familyID As Integer, ByVal relationID As Short, ByVal partSSN As Integer?, ByVal patSSN As Integer?,
                                   ByVal systemCode As String, ByVal status As String, ByVal statusDate As Date?, ByVal claimType As String, ByVal admittance As String, ByVal recDate As Date?,
                                   ByVal netWrkRecDate As Date?, ByVal occFromDate As Date?, ByVal occToDate As Date?, ByVal incidentDate As Date?, ByVal pPO As String, ByVal cOB As String,
                                   ByVal payee As String, ByVal pricedBy As String, ByVal pricingError As String, ByVal attachSW As Decimal, ByVal duplicateSW As Decimal, ByVal nonParSW As Decimal,
                                   ByVal outOfareaSW As Decimal, ByVal autoAccidentSW As Decimal, ByVal workersCompSW As Decimal, ByVal preventativeSW As Decimal,
                                   ByVal othAccidentSW As Decimal, ByVal chiroSW As Decimal, ByVal paritySW As Decimal, ByVal sEDSW As Decimal, ByVal authorizedSW As Decimal,
                                   ByVal assignOfBenSW As Decimal, ByVal adjustmentSW As Decimal, ByVal othInsSW As Decimal, ByVal othInsRefusalSW As Decimal, ByVal othInsID As Integer?,
                                   ByVal othInsPolicyNBR As String, ByVal patFName As String, ByVal patLName As String, ByVal patSex As String, ByVal patDOB As Date?, ByVal patAcctNBR As String,
                                   ByVal patZip As Integer?, ByVal patZip2 As Integer?, ByVal provTIN As Integer?, ByVal provID As Integer?, ByVal provZip As Integer?, ByVal provZip2 As Integer?,
                                   ByVal provLicense As String, ByVal renderingNPI As Decimal?, ByVal billTaxID As Integer?, ByVal billName As String, ByVal billAddr1 As String, ByVal billAddr2 As String,
                                   ByVal billcity As String, ByVal billState As String, ByVal billZip As Integer?, ByVal billZip2 As Integer?, ByVal totChrgAmt As Decimal?, ByVal totPricedAmt As Decimal?,
                                   ByVal totAllowedAmt As Decimal?, ByVal totOthInsAmt As Decimal?, ByVal totPaidAmt As Decimal?, ByVal totProcessedAmt As Decimal?, ByVal holdDays As Integer?,
                                   ByVal holdDate As Date?, ByVal holdTime As String, ByVal userRights As String, ByRef transaction As DbTransaction)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.UPDATE_MEDHDR"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.FamilyID = familyID.ToString.Trim
            UFCWLastKeyData.RelationID = relationID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@SECURITY_SW", DbType.Decimal, securitySW)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, partSSN)
            DB.AddInParameter(DBCommandWrapper, "@PAT_SSN", DbType.Int32, patSSN)
            DB.AddInParameter(DBCommandWrapper, "@SYSTEM_CODE", DbType.String, systemCode)
            DB.AddInParameter(DBCommandWrapper, "@STATUS", DbType.String, status)
            DB.AddInParameter(DBCommandWrapper, "@STATUS_DATE", DbType.DateTime, UFCWGeneral.ToNullDateHandler(statusDate))
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_TYPE", DbType.String, claimType)
            DB.AddInParameter(DBCommandWrapper, "@ADMITTANCE", DbType.String, admittance)
            DB.AddInParameter(DBCommandWrapper, "@REC_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(recDate))
            DB.AddInParameter(DBCommandWrapper, "@NETWRK_REC_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(netWrkRecDate))
            DB.AddInParameter(DBCommandWrapper, "@OCC_FROM_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(occFromDate))
            DB.AddInParameter(DBCommandWrapper, "@OCC_TO_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(occToDate))
            DB.AddInParameter(DBCommandWrapper, "@INCIDENT_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(incidentDate))
            DB.AddInParameter(DBCommandWrapper, "@PPO", DbType.String, pPO)
            DB.AddInParameter(DBCommandWrapper, "@COB", DbType.String, cOB)
            DB.AddInParameter(DBCommandWrapper, "@PAYEE", DbType.String, payee)
            DB.AddInParameter(DBCommandWrapper, "@PRICED_BY", DbType.String, pricedBy)
            DB.AddInParameter(DBCommandWrapper, "@PRICING_ERROR", DbType.String, pricingError)
            DB.AddInParameter(DBCommandWrapper, "@ATTACH_SW", DbType.Decimal, attachSW)
            DB.AddInParameter(DBCommandWrapper, "@DUPLICATE_SW", DbType.Decimal, duplicateSW)
            DB.AddInParameter(DBCommandWrapper, "@NON_PAR_SW", DbType.Decimal, nonParSW)
            DB.AddInParameter(DBCommandWrapper, "@OUT_OF_AREA_SW", DbType.Decimal, outOfareaSW)
            DB.AddInParameter(DBCommandWrapper, "@AUTO_ACCIDENT_SW", DbType.Decimal, autoAccidentSW)
            DB.AddInParameter(DBCommandWrapper, "@WORKERS_COMP_SW", DbType.Decimal, workersCompSW)
            DB.AddInParameter(DBCommandWrapper, "@PREVENTATIVE_SW", DbType.Decimal, preventativeSW)
            DB.AddInParameter(DBCommandWrapper, "@OTH_ACCIDENT_SW", DbType.Decimal, othAccidentSW)
            DB.AddInParameter(DBCommandWrapper, "@OTH_INS_SW", DbType.Decimal, othInsSW)
            DB.AddInParameter(DBCommandWrapper, "@CHIRO_SW", DbType.Decimal, chiroSW)
            DB.AddInParameter(DBCommandWrapper, "@PARITY_SW", DbType.Decimal, paritySW)
            DB.AddInParameter(DBCommandWrapper, "@SED_SW", DbType.Decimal, sEDSW)
            DB.AddInParameter(DBCommandWrapper, "@AUTHORIZED_SW", DbType.Decimal, authorizedSW)
            DB.AddInParameter(DBCommandWrapper, "@ASSIGN_OF_BEN_SW", DbType.Decimal, assignOfBenSW)
            DB.AddInParameter(DBCommandWrapper, "@ADJUSTMENT_SW", DbType.Decimal, adjustmentSW)
            DB.AddInParameter(DBCommandWrapper, "@OTH_INS_REFUSAL_SW", DbType.Decimal, othInsRefusalSW)
            DB.AddInParameter(DBCommandWrapper, "@OTH_INS_ID", DbType.Int32, othInsID)
            DB.AddInParameter(DBCommandWrapper, "@OTH_INS_POLICY_NBR", DbType.String, othInsPolicyNBR)
            DB.AddInParameter(DBCommandWrapper, "@PAT_FNAME", DbType.String, patFName)
            DB.AddInParameter(DBCommandWrapper, "@PAT_LNAME", DbType.String, patLName)
            DB.AddInParameter(DBCommandWrapper, "@PAT_SEX", DbType.String, patSex)
            DB.AddInParameter(DBCommandWrapper, "@PAT_DOB", DbType.Date, UFCWGeneral.ToNullDateHandler(patDOB))
            DB.AddInParameter(DBCommandWrapper, "@PAT_ACCT_NBR", DbType.String, patAcctNBR)
            DB.AddInParameter(DBCommandWrapper, "@PAT_ZIP", DbType.Int32, patZip)
            DB.AddInParameter(DBCommandWrapper, "@PAT_ZIP2", DbType.Int32, patZip2)
            DB.AddInParameter(DBCommandWrapper, "@PROV_TIN", DbType.Int32, provTIN)
            DB.AddInParameter(DBCommandWrapper, "@PROV_ID", DbType.Int32, provID)
            DB.AddInParameter(DBCommandWrapper, "@PROV_ZIP", DbType.Int32, provZip)
            DB.AddInParameter(DBCommandWrapper, "@PROV_ZIP2", DbType.Int32, provZip2)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LICENSE", DbType.String, provLicense)
            DB.AddInParameter(DBCommandWrapper, "@RENDERING_NPI", DbType.Decimal, renderingNPI)
            DB.AddInParameter(DBCommandWrapper, "@BILL_TAXID", DbType.Int32, billTaxID)
            DB.AddInParameter(DBCommandWrapper, "@BILL_NAME", DbType.String, billName)
            DB.AddInParameter(DBCommandWrapper, "@BILL_ADDR1", DbType.String, billAddr1)
            DB.AddInParameter(DBCommandWrapper, "@BILL_ADDR2", DbType.String, billAddr2)
            DB.AddInParameter(DBCommandWrapper, "@BILL_city", DbType.String, billcity)
            DB.AddInParameter(DBCommandWrapper, "@BILL_STATE", DbType.String, billState)
            DB.AddInParameter(DBCommandWrapper, "@BILL_ZIP", DbType.Int32, billZip)
            DB.AddInParameter(DBCommandWrapper, "@BILL_ZIP2", DbType.Int32, billZip2)
            DB.AddInParameter(DBCommandWrapper, "@TOT_CHRG_AMT", DbType.Decimal, totChrgAmt)
            DB.AddInParameter(DBCommandWrapper, "@TOT_PRICED_AMT", DbType.Decimal, totPricedAmt)
            DB.AddInParameter(DBCommandWrapper, "@TOT_ALLOWED_AMT", DbType.Decimal, totAllowedAmt)
            DB.AddInParameter(DBCommandWrapper, "@TOT_oth_INS_AMT", DbType.Decimal, totOthInsAmt)
            DB.AddInParameter(DBCommandWrapper, "@TOT_PAID_AMT", DbType.Decimal, totPaidAmt)
            DB.AddInParameter(DBCommandWrapper, "@TOT_PROCESSED_AMT", DbType.Decimal, totProcessedAmt)
            DB.AddInParameter(DBCommandWrapper, "@HOLD_DAYS", DbType.Int32, holdDays)
            DB.AddInParameter(DBCommandWrapper, "@HOLD_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(holdDate))
            DB.AddInParameter(DBCommandWrapper, "@HOLD_TIME", DbType.String, holdTime)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)
        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub CreateMEDHDR(ByVal claimID As Integer, ByVal securitySW As Decimal, ByVal familyID As Integer, ByVal relationID As Short, ByVal partSSN As Integer?, ByVal patSSN As Integer?,
                                   ByVal docID As Decimal?, ByVal maxID As String, ByVal systemCode As String, ByVal status As String, ByVal statusDate As Date?, ByVal claimType As String,
                                   ByVal admittance As String, ByVal recDate As Date?, ByVal netWrkRecDate As Date?, ByVal occFromDate As Date?, ByVal occToDate As Date?, ByVal incidentDate As Date?,
                                   ByVal ppo As String, ByVal cob As String, ByVal payee As String, ByVal pricedBy As String, ByVal pricingError As String, ByVal attachSW As Decimal, ByVal duplicateSW As Decimal,
                                   ByVal nonParSW As Decimal, ByVal outOfAreaSW As Decimal, ByVal autoAccidentSW As Decimal, ByVal workersCompSW As Decimal, ByVal preventativeSW As Decimal,
                                   ByVal othAccidentSW As Decimal, ByVal chiroSW As Decimal, ByVal othInsSW As Decimal, ByVal paritySW As Decimal, ByVal sEDSW As Decimal, ByVal authorizedSW As Decimal,
                                   ByVal assignOfBenSW As Decimal, ByVal adjustmentSW As Decimal, ByVal othInsRefusalSW As Decimal, ByVal othInsID As Integer?, ByVal othInsPolicyNBR As String,
                                   ByVal patFName As String, ByVal patLName As String, ByVal patSex As String, ByVal patDOB As Date?, ByVal patAcctNBR As String, ByVal patZip As Integer?, ByVal patZip2 As Integer?,
                                   ByVal provTIN As Integer?, ByVal provID As Integer?, ByVal provZip As Integer?, ByVal provZip2 As Integer?, ByVal provLicense As String, ByVal renderingNPI As Decimal?,
                                   ByVal billTaxID As Integer?, ByVal billName As String, ByVal billAddr1 As String, ByVal billAddr2 As String, ByVal billcity As String, ByVal billState As String,
                                   ByVal billZip As Integer?, ByVal billZip2 As Integer?, ByVal totChrgAmt As Decimal?, ByVal totPricedAmt As Decimal?, ByVal totAllowedAmt As Decimal?, ByVal totOthInsAmt As Decimal?,
                                   ByVal totPaidAmt As Decimal?, ByVal totProcessedAmt As Decimal?, ByVal holdDays As Integer?, ByVal holdDate As Date?, ByVal holdTime As String, ByVal userRights As String,
                                   ByRef transaction As DbTransaction)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_MEDHDR"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.FamilyID = familyID.ToString.Trim
            UFCWLastKeyData.RelationID = relationID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@SECURITY_SW", DbType.Decimal, securitySW)
            DB.AddInParameter(DBCommandWrapper, "@LOCAL_SW", DbType.Decimal, ValidateLocalUser(CInt(partSSN), transaction))
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, partSSN)
            DB.AddInParameter(DBCommandWrapper, "@PAT_SSN", DbType.Int32, patSSN)
            DB.AddInParameter(DBCommandWrapper, "@DOCID", DbType.Decimal, docID)
            DB.AddInParameter(DBCommandWrapper, "@MAXID", DbType.String, maxID)
            DB.AddInParameter(DBCommandWrapper, "@SYSTEM_CODE", DbType.String, systemCode)
            DB.AddInParameter(DBCommandWrapper, "@STATUS", DbType.String, status)
            DB.AddInParameter(DBCommandWrapper, "@STATUS_DATE", DbType.DateTime, UFCWGeneral.ToNullDateHandler(statusDate))
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_TYPE", DbType.String, claimType)
            DB.AddInParameter(DBCommandWrapper, "@ADMITTANCE", DbType.String, admittance)
            DB.AddInParameter(DBCommandWrapper, "@REC_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(recDate))
            DB.AddInParameter(DBCommandWrapper, "@NETWRK_REC_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(netWrkRecDate))
            DB.AddInParameter(DBCommandWrapper, "@OCC_FROM_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(occFromDate))
            DB.AddInParameter(DBCommandWrapper, "@OCC_TO_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(occToDate))
            DB.AddInParameter(DBCommandWrapper, "@INCIDENT_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(incidentDate))
            DB.AddInParameter(DBCommandWrapper, "@PPO", DbType.String, ppo)
            DB.AddInParameter(DBCommandWrapper, "@COB", DbType.String, cob)
            DB.AddInParameter(DBCommandWrapper, "@PAYEE", DbType.String, payee)
            DB.AddInParameter(DBCommandWrapper, "@PRICED_BY", DbType.String, pricedBy)
            DB.AddInParameter(DBCommandWrapper, "@PRICING_ERROR", DbType.String, pricingError)
            DB.AddInParameter(DBCommandWrapper, "@ATTACH_SW", DbType.Decimal, attachSW)
            DB.AddInParameter(DBCommandWrapper, "@DUPLICATE_SW", DbType.Decimal, duplicateSW)
            DB.AddInParameter(DBCommandWrapper, "@NON_PAR_SW", DbType.Decimal, nonParSW)
            DB.AddInParameter(DBCommandWrapper, "@OUT_OF_AREA_SW", DbType.Decimal, outOfAreaSW)
            DB.AddInParameter(DBCommandWrapper, "@AUTO_ACCIDENT_SW", DbType.Decimal, autoAccidentSW)
            DB.AddInParameter(DBCommandWrapper, "@WORKERS_COMP_SW", DbType.Decimal, workersCompSW)
            DB.AddInParameter(DBCommandWrapper, "@PREVENTATIVE_SW", DbType.Decimal, preventativeSW)
            DB.AddInParameter(DBCommandWrapper, "@OTH_ACCIDENT_SW", DbType.Decimal, othAccidentSW)
            DB.AddInParameter(DBCommandWrapper, "@CHIRO_SW", DbType.Decimal, chiroSW)
            DB.AddInParameter(DBCommandWrapper, "@OTH_INS_SW", DbType.Decimal, othInsSW)
            DB.AddInParameter(DBCommandWrapper, "@PARITY_SW", DbType.Decimal, paritySW)
            DB.AddInParameter(DBCommandWrapper, "@SED_SW", DbType.Decimal, sEDSW)
            DB.AddInParameter(DBCommandWrapper, "@AUTHORIZED_SW", DbType.Decimal, authorizedSW)
            DB.AddInParameter(DBCommandWrapper, "@ASSIGN_OF_BEN_SW", DbType.Decimal, assignOfBenSW)
            DB.AddInParameter(DBCommandWrapper, "@ADJUSTMENT_SW", DbType.Decimal, adjustmentSW)
            DB.AddInParameter(DBCommandWrapper, "@OTH_INS_REFUSAL_SW", DbType.Decimal, othInsRefusalSW)
            DB.AddInParameter(DBCommandWrapper, "@OTH_INS_ID", DbType.Int32, othInsID)
            DB.AddInParameter(DBCommandWrapper, "@OTH_INS_POLICY_NBR", DbType.String, othInsPolicyNBR)
            DB.AddInParameter(DBCommandWrapper, "@PAT_FNAME", DbType.String, patFName)
            DB.AddInParameter(DBCommandWrapper, "@PAT_LNAME", DbType.String, patLName)
            DB.AddInParameter(DBCommandWrapper, "@PAT_SEX", DbType.String, patSex)
            DB.AddInParameter(DBCommandWrapper, "@PAT_DOB", DbType.Date, UFCWGeneral.ToNullDateHandler(patDOB))
            DB.AddInParameter(DBCommandWrapper, "@PAT_ACCT_NBR", DbType.String, patAcctNBR)
            DB.AddInParameter(DBCommandWrapper, "@PAT_ZIP", DbType.Int32, patZip)
            DB.AddInParameter(DBCommandWrapper, "@PAT_ZIP2", DbType.Int32, patZip2)
            DB.AddInParameter(DBCommandWrapper, "@PROV_TIN", DbType.Int32, provTIN)
            DB.AddInParameter(DBCommandWrapper, "@PROV_ID", DbType.Int32, provID)
            DB.AddInParameter(DBCommandWrapper, "@PROV_ZIP", DbType.Int32, provZip)
            DB.AddInParameter(DBCommandWrapper, "@PROV_ZIP2", DbType.Int32, provZip2)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LICENSE", DbType.String, provLicense)
            DB.AddInParameter(DBCommandWrapper, "@RENDERING_NPI", DbType.Int32, renderingNPI)
            DB.AddInParameter(DBCommandWrapper, "@BILL_TAXID", DbType.Int32, billTaxID)
            DB.AddInParameter(DBCommandWrapper, "@BILL_NAME", DbType.String, billName)
            DB.AddInParameter(DBCommandWrapper, "@BILL_ADDR1", DbType.String, billAddr1)
            DB.AddInParameter(DBCommandWrapper, "@BILL_ADDR2", DbType.String, billAddr2)
            DB.AddInParameter(DBCommandWrapper, "@BILL_city", DbType.String, billcity)
            DB.AddInParameter(DBCommandWrapper, "@BILL_STATE", DbType.String, billState)
            DB.AddInParameter(DBCommandWrapper, "@BILL_ZIP", DbType.Int32, billZip)
            DB.AddInParameter(DBCommandWrapper, "@BILL_ZIP2", DbType.Int32, billZip2)
            DB.AddInParameter(DBCommandWrapper, "@TOT_CHRG_AMT", DbType.Decimal, totChrgAmt)
            DB.AddInParameter(DBCommandWrapper, "@TOT_PRICED_AMT", DbType.Decimal, totPricedAmt)
            DB.AddInParameter(DBCommandWrapper, "@TOT_ALLOWED_AMT", DbType.Decimal, totAllowedAmt)
            DB.AddInParameter(DBCommandWrapper, "@TOT_oth_INS_AMT", DbType.Decimal, totOthInsAmt)
            DB.AddInParameter(DBCommandWrapper, "@TOT_PAID_AMT", DbType.Decimal, totPaidAmt)
            DB.AddInParameter(DBCommandWrapper, "@TOT_PROCESSED_AMT", DbType.Decimal, totProcessedAmt)
            DB.AddInParameter(DBCommandWrapper, "@HOLD_DAYS", DbType.Int32, holdDays)
            DB.AddInParameter(DBCommandWrapper, "@HOLD_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(holdDate))
            DB.AddInParameter(DBCommandWrapper, "@HOLD_TIME", DbType.String, holdTime)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub UpdateMEDDTL(ByVal claimID As Integer, ByVal lineNbr As Short, ByVal securitySW As Decimal, ByVal familyID As Integer, ByVal relationID As Short, ByVal partSSN As Integer?, ByVal patSSN As Integer?,
                                   ByVal maxIDLineNbr As Short, ByVal systemCode As String, ByVal status As String, ByVal statusDate As Date?, ByVal pricingError As String, ByVal pricingSDError As String,
                                   ByVal pricingReason As String, ByVal occFromDate As Date?, ByVal occToDate As Date?, ByVal incidentDate As Date?, ByVal placeOfServ As String, ByVal billType As String,
                                   ByVal procCode As String, ByVal modifierSW As Decimal, ByVal reasonSW As Decimal, ByVal diagSW As Decimal, ByVal localUse As String, ByVal daysUnits As Decimal?,
                                   ByVal medPlan As String, ByVal memType As String, ByVal eligStatus As String, ByVal duplicateSW As Decimal, ByVal nonParSW As Decimal, ByVal outOfAreaSW As Decimal,
                                   ByVal autoAccidentSW As Decimal, ByVal workersCompSW As Decimal, ByVal othAccidentSW As Decimal, ByVal othInsSW As Decimal, ByVal chrgAmt As Decimal?, ByVal pricedAmt As Decimal?,
                                   ByVal allowedAmt As Decimal?, ByVal othInsAmt As Decimal?, ByVal paidAmt As Decimal?, ByVal processedAmt As Decimal?, ByVal overrideSW As Decimal, ByVal procID As Integer?,
                                   ByVal ruleSetID As Integer?, ByVal checkSW As Decimal, ByVal closedDate As Date?, ByVal adjuster As String, ByVal userRights As String, ByVal hRAExclude As Decimal,
                                   ByVal ndc As String, ByVal rxUnits As String, ByVal rxQty As Decimal?, ByVal rxPrescriptionNum As String,
                                   ByRef transaction As DbTransaction)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.UPDATE_MEDDTL_RX"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.FamilyID = familyID.ToString.Trim
            UFCWLastKeyData.RelationID = relationID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@LINE_NBR", DbType.Int32, lineNbr)
            DB.AddInParameter(DBCommandWrapper, "@SECURITY_SW", DbType.Decimal, securitySW)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, partSSN)
            DB.AddInParameter(DBCommandWrapper, "@PAT_SSN", DbType.Int32, patSSN)
            DB.AddInParameter(DBCommandWrapper, "@MAXID_LINE_NBR", DbType.Int32, maxIDLineNbr)
            DB.AddInParameter(DBCommandWrapper, "@SYSTEM_CODE", DbType.String, systemCode)
            DB.AddInParameter(DBCommandWrapper, "@STATUS", DbType.String, status)
            DB.AddInParameter(DBCommandWrapper, "@STATUS_DATE", DbType.DateTime, UFCWGeneral.ToNullDateHandler(statusDate))
            DB.AddInParameter(DBCommandWrapper, "@PRICING_ERROR", DbType.String, pricingError)
            DB.AddInParameter(DBCommandWrapper, "@PRICING_SD_ERROR", DbType.String, pricingSDError)
            DB.AddInParameter(DBCommandWrapper, "@PRICING_REASON", DbType.String, pricingReason)
            DB.AddInParameter(DBCommandWrapper, "@OCC_FROM_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(occFromDate))
            DB.AddInParameter(DBCommandWrapper, "@OCC_TO_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(occToDate))
            DB.AddInParameter(DBCommandWrapper, "@INCIDENT_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(incidentDate))
            DB.AddInParameter(DBCommandWrapper, "@PLACE_OF_SERV", DbType.String, placeOfServ)
            DB.AddInParameter(DBCommandWrapper, "@BILL_TYPE", DbType.String, billType)
            DB.AddInParameter(DBCommandWrapper, "@PROC_CODE", DbType.String, procCode)
            DB.AddInParameter(DBCommandWrapper, "@MODIFIER_SW", DbType.Decimal, modifierSW)
            DB.AddInParameter(DBCommandWrapper, "@REASON_SW", DbType.Decimal, reasonSW)
            DB.AddInParameter(DBCommandWrapper, "@DIAG_SW", DbType.Decimal, diagSW)
            DB.AddInParameter(DBCommandWrapper, "@LOCAL_USE", DbType.String, localUse)
            DB.AddInParameter(DBCommandWrapper, "@DAYS_UNITS", DbType.Decimal, daysUnits)
            DB.AddInParameter(DBCommandWrapper, "@MED_PLAN", DbType.String, medPlan)
            DB.AddInParameter(DBCommandWrapper, "@MEMTYPE", DbType.String, memType)
            DB.AddInParameter(DBCommandWrapper, "@ELIG_STATUS", DbType.String, eligStatus)
            DB.AddInParameter(DBCommandWrapper, "@DUPLICATE_SW", DbType.Decimal, duplicateSW)
            DB.AddInParameter(DBCommandWrapper, "@NON_PAR_SW", DbType.Decimal, nonParSW)
            DB.AddInParameter(DBCommandWrapper, "@OUT_OF_AREA_SW", DbType.Decimal, outOfAreaSW)
            DB.AddInParameter(DBCommandWrapper, "@AUTO_ACCIDENT_SW", DbType.Decimal, autoAccidentSW)
            DB.AddInParameter(DBCommandWrapper, "@WORKERS_COMP_SW", DbType.Decimal, workersCompSW)
            DB.AddInParameter(DBCommandWrapper, "@OTH_ACCIDENT_SW", DbType.Decimal, othAccidentSW)
            DB.AddInParameter(DBCommandWrapper, "@OTH_INS_SW", DbType.Decimal, othInsSW)
            DB.AddInParameter(DBCommandWrapper, "@CHRG_AMT", DbType.Decimal, chrgAmt)
            DB.AddInParameter(DBCommandWrapper, "@PRICED_AMT", DbType.Decimal, pricedAmt)
            DB.AddInParameter(DBCommandWrapper, "@ALLOWED_AMT", DbType.Decimal, allowedAmt)
            DB.AddInParameter(DBCommandWrapper, "@OTH_INS_AMT", DbType.Decimal, othInsAmt)
            DB.AddInParameter(DBCommandWrapper, "@PAID_AMT", DbType.Decimal, paidAmt)
            DB.AddInParameter(DBCommandWrapper, "@PROCESSED_AMT", DbType.Decimal, processedAmt)
            DB.AddInParameter(DBCommandWrapper, "@OVERRIDE_SW", DbType.Decimal, overrideSW)
            DB.AddInParameter(DBCommandWrapper, "@PROC_ID", DbType.Int32, procID)
            DB.AddInParameter(DBCommandWrapper, "@RULE_SET_ID", DbType.Int32, ruleSetID)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_SW", DbType.Decimal, checkSW)
            DB.AddInParameter(DBCommandWrapper, "@CLOSED_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(closedDate))
            DB.AddInParameter(DBCommandWrapper, "@ADJUSTER", DbType.String, adjuster)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@HRA_EXCLUDE", DbType.Decimal, hRAExclude)
            DB.AddInParameter(DBCommandWrapper, "@NDC", DbType.String, ndc)
            DB.AddInParameter(DBCommandWrapper, "@RX_UNITS", DbType.String, rxUnits)
            DB.AddInParameter(DBCommandWrapper, "@RX_QTY", DbType.Decimal, rxQty)
            DB.AddInParameter(DBCommandWrapper, "@RX_PRESCRIPTION_NUM", DbType.String, rxPrescriptionNum)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub CreateMEDDTL(ByVal claimID As Integer, ByVal lineNbr As Integer, ByVal securitySW As Decimal, ByVal familyID As Integer, ByVal relationID As Short,
                                   ByVal partSSN As Integer?, ByVal patSSN As Integer?, ByVal maxID As String, ByVal maxIDLineNbr As Short, ByVal systemCode As String,
                                   ByVal status As String, ByVal statusDate As Date?, ByVal pricingError As String, ByVal pricingSDError As String, ByVal pricingReason As String,
                                   ByVal occFromDate As Date?, ByVal occToDate As Date?, ByVal incidentDate As Date?, ByVal placeOfServ As String, ByVal billType As String,
                                   ByVal procCode As String, ByVal modifierSW As Decimal, ByVal reasonSW As Decimal, ByVal diagSW As Decimal, ByVal localUse As String,
                                   ByVal daysUnits As Decimal?, ByVal medPlan As String, ByVal memType As String, ByVal eligStatus As String, ByVal duplicateSW As Decimal,
                                   ByVal nonParSW As Decimal, ByVal outOfAreaSW As Decimal, ByVal autoAccidentSW As Decimal, ByVal workersCompSW As Decimal, ByVal othAccidentSW As Decimal,
                                   ByVal othInsSW As Decimal, ByVal chrgAmt As Decimal?, ByVal pricedAmt As Decimal?, ByVal allowedAmt As Decimal?, ByVal othInsAmt As Decimal?, ByVal paidAmt As Decimal?,
                                   ByVal processedAmt As Decimal?, ByVal overrideSW As Decimal, ByVal procID As Integer?, ByVal ruleSetID As Integer?, ByVal checkSW As Decimal, ByVal closedDate As Date?,
                                   ByVal adjuster As String, ByVal userRights As String, ByVal hraExclude As Decimal,
                                   ByVal ndc As String, ByVal rxUnits As String, ByVal rxQty As Decimal?, ByVal rxPrescriptionNum As String,
                                   ByRef transaction As DbTransaction)


        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_MEDDTL_RX"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.FamilyID = familyID.ToString.Trim
            UFCWLastKeyData.RelationID = relationID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@LINE_NBR", DbType.Int32, lineNbr)
            DB.AddInParameter(DBCommandWrapper, "@SECURITY_SW", DbType.Decimal, securitySW)
            DB.AddInParameter(DBCommandWrapper, "@LOCAL_SW", DbType.Decimal, ValidateLocalUser(CInt(partSSN), transaction))
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, partSSN)
            DB.AddInParameter(DBCommandWrapper, "@PAT_SSN", DbType.Int32, patSSN)
            DB.AddInParameter(DBCommandWrapper, "@MAXID", DbType.String, maxID)
            DB.AddInParameter(DBCommandWrapper, "@MAXID_LINE_NBR", DbType.Int32, maxIDLineNbr)
            DB.AddInParameter(DBCommandWrapper, "@SYSTEM_CODE", DbType.String, systemCode)
            DB.AddInParameter(DBCommandWrapper, "@STATUS", DbType.String, status)
            DB.AddInParameter(DBCommandWrapper, "@STATUS_DATE", DbType.DateTime, UFCWGeneral.ToNullDateHandler(statusDate))
            DB.AddInParameter(DBCommandWrapper, "@PRICING_ERROR", DbType.String, pricingError)
            DB.AddInParameter(DBCommandWrapper, "@PRICING_SD_ERROR", DbType.String, pricingSDError)
            DB.AddInParameter(DBCommandWrapper, "@PRICING_REASON", DbType.String, pricingReason)
            DB.AddInParameter(DBCommandWrapper, "@OCC_FROM_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(occFromDate))
            DB.AddInParameter(DBCommandWrapper, "@OCC_TO_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(occToDate))
            DB.AddInParameter(DBCommandWrapper, "@INCIDENT_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(incidentDate))
            DB.AddInParameter(DBCommandWrapper, "@PLACE_OF_SERV", DbType.String, placeOfServ)
            DB.AddInParameter(DBCommandWrapper, "@BILL_TYPE", DbType.String, billType)
            DB.AddInParameter(DBCommandWrapper, "@PROC_CODE", DbType.String, UFCWGeneral.IsNullStringHandler(procCode, ""))
            DB.AddInParameter(DBCommandWrapper, "@MODIFIER_SW", DbType.Decimal, modifierSW)
            DB.AddInParameter(DBCommandWrapper, "@REASON_SW", DbType.Decimal, reasonSW)
            DB.AddInParameter(DBCommandWrapper, "@DIAG_SW", DbType.Decimal, diagSW)
            DB.AddInParameter(DBCommandWrapper, "@LOCAL_USE", DbType.String, localUse)
            DB.AddInParameter(DBCommandWrapper, "@DAYS_UNITS", DbType.Decimal, daysUnits)
            DB.AddInParameter(DBCommandWrapper, "@MED_PLAN", DbType.String, medPlan)
            DB.AddInParameter(DBCommandWrapper, "@MEMTYPE", DbType.String, memType)
            DB.AddInParameter(DBCommandWrapper, "@ELIG_STATUS", DbType.String, eligStatus)
            DB.AddInParameter(DBCommandWrapper, "@DUPLICATE_SW", DbType.Decimal, duplicateSW)
            DB.AddInParameter(DBCommandWrapper, "@NON_PAR_SW", DbType.Decimal, nonParSW)
            DB.AddInParameter(DBCommandWrapper, "@OUT_OF_AREA_SW", DbType.Decimal, outOfAreaSW)
            DB.AddInParameter(DBCommandWrapper, "@AUTO_ACCIDENT_SW", DbType.Decimal, autoAccidentSW)
            DB.AddInParameter(DBCommandWrapper, "@WORKERS_COMP_SW", DbType.Decimal, workersCompSW)
            DB.AddInParameter(DBCommandWrapper, "@OTH_ACCIDENT_SW", DbType.Decimal, othAccidentSW)
            DB.AddInParameter(DBCommandWrapper, "@OTH_INS_SW", DbType.Decimal, othInsSW)
            DB.AddInParameter(DBCommandWrapper, "@CHRG_AMT", DbType.Decimal, chrgAmt)
            DB.AddInParameter(DBCommandWrapper, "@PRICED_AMT", DbType.Decimal, pricedAmt)
            DB.AddInParameter(DBCommandWrapper, "@ALLOWED_AMT", DbType.Decimal, allowedAmt)
            DB.AddInParameter(DBCommandWrapper, "@OTH_INS_AMT", DbType.Decimal, othInsAmt)
            DB.AddInParameter(DBCommandWrapper, "@PAID_AMT", DbType.Decimal, paidAmt)
            DB.AddInParameter(DBCommandWrapper, "@PROCESSED_AMT", DbType.Decimal, processedAmt)
            DB.AddInParameter(DBCommandWrapper, "@OVERRIDE_SW", DbType.Decimal, overrideSW)
            DB.AddInParameter(DBCommandWrapper, "@PROC_ID", DbType.Int32, procID)
            DB.AddInParameter(DBCommandWrapper, "@RULE_SET_ID", DbType.Int32, ruleSetID)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_SW", DbType.Decimal, checkSW)
            DB.AddInParameter(DBCommandWrapper, "@CLOSED_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(closedDate))
            DB.AddInParameter(DBCommandWrapper, "@ADJUSTER", DbType.String, adjuster)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@HRA_EXCLUDE", DbType.Decimal, hraExclude)

            DB.AddInParameter(DBCommandWrapper, "@NDC", DbType.String, ndc)
            DB.AddInParameter(DBCommandWrapper, "@RX_UNITS", DbType.String, rxUnits)
            DB.AddInParameter(DBCommandWrapper, "@RX_QTY", DbType.Decimal, rxQty)
            DB.AddInParameter(DBCommandWrapper, "@RX_PRESCRIPTION_NUM", DbType.String, rxPrescriptionNum)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub ArchiveClaimMaster(ByVal claimID As Integer, ByVal status As String, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.ARCHIVE_CLAIM_MASTER"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@STATUS", DbType.String, status)
            DB.AddInParameter(DBCommandWrapper, "@ARCHIVE_SW", DbType.Int32, 1)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub RouteToPricing(ByVal claimID As Integer, ByVal userRights As String, ClaimLastupdated As Date?, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.ROUTE_CLAIM_PRICING"

        Try

            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@LASTUPDT", DbType.DateTime, UFCWGeneral.ToNullDateHandler(ClaimLastupdated))

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub UpdateForRePrice(ByVal claimID As Integer, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RESET_PRICING_STATUS"

        Try

            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub UpdateForRePrice(ByVal status As String, ByVal claimID As Integer, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)

        Try

            UpdateClaimMasterStatus(claimID, status, userRights, transaction)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub UpdateClaimMasterStatus(ByVal claimID As Integer, ByVal status As String, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.UPDATE_CLAIM_MASTER_STATUS"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@STATUS", DbType.String, status)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub UpdateClaimMasterEDIStatus(ByVal claimID As Integer, ByVal status As String, ByVal lastUPDT As DateTime, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.UPDATE_HIPAA_REMIT_CANDIDATE_BY_CLAIMID"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@STATUS", DbType.String, status)
            DB.AddInParameter(DBCommandWrapper, "@LASTUPDT", DbType.DateTime, UFCWGeneral.ToNullDateHandler(lastUPDT))

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub UpdateClaimMasterProcessed(ByVal claimID As Integer, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.UPDATE_CLAIM_MASTER_PROCESSED"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Function RetrieveDetailLineReasons(ByVal claimID As Integer, ByVal lineNumber As Short, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_REASON_BY_CLAIMID_LINENUMBER"

        Try

            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@LINE_NBR", DbType.Int32, lineNumber)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "REASON")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "REASON", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Sub DeleteReason(ByVal claimID As Integer, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.DELETE_REASON_BY_CLAIMID"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)

            If transaction IsNot Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub CreateReason(ByVal claimID As Integer, ByVal lineNbr As Short, ByVal reason As String, ByVal priority As Integer, ByVal description As String, ByVal printSW As Decimal, ByVal userRights As String, ByRef transaction As DbTransaction)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_REASON"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@LINE_NBR", DbType.Int16, lineNbr)
            DB.AddInParameter(DBCommandWrapper, "@REASON", DbType.String, reason)
            DB.AddInParameter(DBCommandWrapper, "@PRIORITY", DbType.Int32, priority)
            DB.AddInParameter(DBCommandWrapper, "@DESCRIPTION", DbType.String, description)
            DB.AddInParameter(DBCommandWrapper, "@PRINT_SW", DbType.Decimal, printSW)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Function RetrieveReportColumns(ByVal className As String, ByVal displayArea As String, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_COLUMNS_BY_CLASSNAME_DISPLAYAREA"

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLASSNAME", DbType.String, className)
            DB.AddInParameter(DBCommandWrapper, "@DISPLAYAREA", DbType.String, displayArea)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "REPORTCOLUMNS")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "REPORTCOLUMNS", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveProviderFromNPIRegistry(ByVal npi As Decimal, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_NPI"

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@NPI", DbType.Decimal, npi)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "NPI_REGISTRY")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "NPI_REGISTRY", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveClaimReasons(ByVal claimID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_REASON_BY_CLAIMID"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "REASON")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "REASON", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveDocHistoryStatusValues(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_DOC_HISTORY_STATUS_VALUES"

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "DOC_HISTORY_STATUS_VALUES")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "DOC_HISTORY_STATUS_VALUES", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveClaimDiagnosis(ByVal claimID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_MEDDIAG_BY_CLAIMID"

        Try

            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "MEDDIAG")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "MEDDIAG", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Sub DeleteMEDOTHER_INS(ByVal mEDOTHER_INS_ID As Integer, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.DELETE_MEDOTHER_INS"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@MEDOTHER_INS_ID", DbType.Int32, mEDOTHER_INS_ID)

            If transaction IsNot Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub DeleteMEDDTLAndCascade(ByVal claimID As Integer, ByVal lineNbr As Short, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.DELETE_MEDDTL_AND_CHILDREN_BY_LINENBR"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIMID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@LINENBR", DbType.Int16, lineNbr)

            If transaction IsNot Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub DeleteDiagnosis(ByVal claimID As Integer, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.DELETE_MEDDIAG_BY_CLAIMID"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)

            If transaction IsNot Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub CreateDiagnosis(ByVal claimID As Integer, ByVal lineNbr As Short, ByVal diagnosis As String, ByVal priority As Short, ByVal userRights As String, ByRef transaction As DbTransaction)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_MEDDIAG"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@LINE_NBR", DbType.Int16, lineNbr)
            DB.AddInParameter(DBCommandWrapper, "@DIAGNOSIS", DbType.String, diagnosis)
            DB.AddInParameter(DBCommandWrapper, "@PRIORITY", DbType.Int16, priority)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Function RetrieveClaimModifier(ByVal claimID As Integer, ByVal effectiveDate As Date?, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_MEDMOD_BY_CLAIMID_EFFECTIVE_DATE"

        Try

            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@EFFECTIVE_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(effectiveDate))

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "MEDMOD")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "MEDMOD", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Sub DeleteModifier(ByVal claimID As Integer, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.DELETE_MEDMOD_BY_CLAIMID"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)

            If transaction IsNot Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub CreateModifier(ByVal claimID As Integer, ByVal lineNbr As Short, ByVal modifier As String, ByVal priority As Short, ByVal userRights As String, ByRef transaction As DbTransaction)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_MEDMOD"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@LINE_NBR", DbType.Int16, lineNbr)
            DB.AddInParameter(DBCommandWrapper, "@MODIFIER", DbType.String, modifier)
            DB.AddInParameter(DBCommandWrapper, "@PRIORITY", DbType.Int16, priority)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Function RetrieveClaimAnnotations(ByVal claimID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_ANNOTATIONS_BY_CLAIMID"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "ANNOTATIONS")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "ANNOTATIONS", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Sub CreateAnnotation(ByVal claimID As Integer, ByVal familyID As Integer, ByVal relationID As Short, ByVal partSSN As Integer?, ByVal patSSN As Integer?, ByVal partFName As String, ByVal partLName As String, ByVal patFName As String, ByVal patLName As String, ByVal annotation As String, ByVal flag As Object, ByVal userRights As String, ByRef transaction As DbTransaction)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_ANNOTATION"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.FamilyID = familyID.ToString.Trim
            UFCWLastKeyData.RelationID = relationID.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, partSSN)
            DB.AddInParameter(DBCommandWrapper, "@PAT_SSN", DbType.Int32, patSSN)
            DB.AddInParameter(DBCommandWrapper, "@PART_FNAME", DbType.String, partFName)
            DB.AddInParameter(DBCommandWrapper, "@PART_LNAME", DbType.String, partLName)
            DB.AddInParameter(DBCommandWrapper, "@PAT_FNAME", DbType.String, patFName)
            DB.AddInParameter(DBCommandWrapper, "@PAT_LNAME", DbType.String, patLName)
            DB.AddInParameter(DBCommandWrapper, "@ANNOTATION", DbType.String, Replace(annotation.ToString, vbCrLf, "\N"))
            DB.AddParameter(DBCommandWrapper, "@FLAG", DbType.Binary, 300, ParameterDirection.Input, True, 38, 0, "@FLAG", DataRowVersion.Current, flag)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

        Catch ex As DB2Exception When ex.Number <> -301
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Function RetrieveCOBPrime(ByVal familyID As Integer, ByVal familyIDSpouse As Integer) As Integer?
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_FUND_COB_PRIME"

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILYID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@FAMILYIDSPOUSE", DbType.Int32, familyIDSpouse)
            DB.AddOutParameter(DBCommandWrapper, "@PRIMEFAMILYID", DbType.Int32, 11)

            DB.ExecuteNonQuery(DBCommandWrapper)

            Return UFCWGeneral.IsNullIntegerHandler(DB.GetParameterValue(DBCommandWrapper, "@PRIMEFAMILYID"), "@PRIMEFAMILYID")

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function IndentifyAsFundCOB(ByVal familyID As Integer, ByVal occFromDate As Date) As DataTable
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_FUND_COB_STATUS"
        Dim DS As DataSet
        Dim DT As DataTable

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@OCC_FROM_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(occFromDate))

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            DT = New DataTable("COB")

            If DS.Tables.Count > 0 Then
                DT = DS.Tables(0)
            End If

            Return DT

        Catch ex As Exception
            Throw
        Finally
            If DS IsNot Nothing Then DS.Dispose()
            If DT IsNot Nothing Then DT.Dispose()
        End Try
    End Function

    Public Shared Function RetrieveGFCandLBLByNDC(ByVal nDC11 As String, Optional ByVal ds As DataSet = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_REDBOOK_GFC_AND_LBL_BY_NDC"
        Dim Tablenames() As String = {"REDBOOK_GFC", "REDBOOK_LBL_Y2K"}

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@NDC11", DbType.String, nDC11)

            If ds Is Nothing Then
                ds = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DB.LoadDataSet(DBCommandWrapper, ds, Tablenames)
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function


    Public Shared Function RetrieveFreeText(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_FREE_TEXT_BY_FAMILYID"
        Dim Tablenames() As String = {"FREE_TEXT"}

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, Tablenames)
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, Tablenames, transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function RetrieveFreeTextAndAlerts(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_FREE_TEXT_AND_ALERTS_BY_FAMILYID"
        Dim Tablenames() As String = {"FREE_TEXT", "REG_ALERTS"}

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, Tablenames)
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, Tablenames, transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Sub CreateFreeText(ByVal familyID As Integer, ByVal relationID As Short, ByVal partSSN As Integer, ByVal patSSN As Integer, ByVal partFName As String, ByVal partLName As String, ByVal patFName As String, ByVal patLName As String,
                                     ByVal freeText As String, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_FREE_TEXT"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, partSSN)
            DB.AddInParameter(DBCommandWrapper, "@PAT_SSN", DbType.Int32, patSSN)
            DB.AddInParameter(DBCommandWrapper, "@PART_FNAME", DbType.String, If(partFName IsNot Nothing, partFName.ToUpper, ""))
            DB.AddInParameter(DBCommandWrapper, "@PART_LNAME", DbType.String, If(partLName IsNot Nothing, partLName.ToUpper, ""))
            DB.AddInParameter(DBCommandWrapper, "@PAT_FNAME", DbType.String, If(patFName IsNot Nothing, patFName.ToUpper, ""))
            DB.AddInParameter(DBCommandWrapper, "@PAT_LNAME", DbType.String, If(patLName IsNot Nothing, patLName.ToUpper, ""))
            DB.AddInParameter(DBCommandWrapper, "@FREE_TEXT", DbType.String, Replace(freeText.ToString, vbCrLf, "\N"))
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Function CreateNewClaim(ByVal familyId As Integer, ByVal relationId As Integer, ByVal docClass As String, ByVal docType As String, ByVal provTIN As Integer, ByVal claimID As Integer?, ByVal dcn As String, ByVal JAA As Boolean, ByVal userId As String, Optional ByVal transaction As DbTransaction = Nothing) As Integer
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_NEW_CLAIM"

        Try
            UFCWLastKeyData.ClaimID = claimID.ToString.Trim
            UFCWLastKeyData.FamilyID = familyId.ToString.Trim
            UFCWLastKeyData.RelationID = relationId.ToString.Trim
            UFCWLastKeyData.SQL = SQLCall

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILYID", DbType.Int32, familyId)
            DB.AddInParameter(DBCommandWrapper, "@RELATIONID", DbType.Int16, relationId)
            DB.AddInParameter(DBCommandWrapper, "@DOCCLASS", DbType.String, docClass)
            DB.AddInParameter(DBCommandWrapper, "@DOCTYPE", DbType.String, docType)
            DB.AddInParameter(DBCommandWrapper, "@PROVID", DbType.Int32, provTIN)
            DB.AddInParameter(DBCommandWrapper, "@REFERENCE_CLAIM", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@DCN", DbType.String, dcn)
            DB.AddInParameter(DBCommandWrapper, "@JAA", DbType.Decimal, If(JAA, 1, 0))
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userId)
            DB.AddOutParameter(DBCommandWrapper, "@CLAIMID", DbType.Int32, 11)

            If transaction IsNot Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper)
            End If

            Return CInt(DB.GetParameterValue(DBCommandWrapper, "@CLAIMID"))

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Sub CreateBCPricing(ByVal claimID As Integer, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_BC_PRICING"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            DBCommandWrapper.CommandTimeout = 240

            If transaction IsNot Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub CreateBCPricingWithReturn(ByVal claimID As Integer, ByVal returnSW As Boolean, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_BC_PRICING_FOR_RETURN"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@RETURN_SW", DbType.Decimal, returnSW)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            DBCommandWrapper.CommandTimeout = 240

            If transaction IsNot Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub CreateDocHistory(ByVal claimID As Integer, ByVal documentID As Decimal?, ByVal transactionType As String, ByVal familyID As Integer?, ByVal relationID As Short?, ByVal partSSN As Integer?, ByVal patSSN As Integer?, ByVal docClass As String, ByVal docType As String, ByVal summary As String, ByVal detail As String, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_DOC_HISTORY"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@DOCID", DbType.Decimal, UFCWGeneral.IsNullDecimalHandler(documentID))
            DB.AddInParameter(DBCommandWrapper, "@TRANSACTION_TYPE", DbType.String, "(V1.1)" & transactionType)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, UFCWGeneral.IsNullIntegerHandler(familyID))
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, UFCWGeneral.IsNullShortHandler(relationID))
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, UFCWGeneral.IsNullIntegerHandler(partSSN))
            DB.AddInParameter(DBCommandWrapper, "@PAT_SSN", DbType.Int32, UFCWGeneral.IsNullIntegerHandler(patSSN))
            DB.AddInParameter(DBCommandWrapper, "@DOC_CLASS", DbType.String, docClass)
            DB.AddInParameter(DBCommandWrapper, "@DOC_TYPE", DbType.String, docType)
            DB.AddInParameter(DBCommandWrapper, "@SUMMARY", DbType.String, Replace(summary.ToString, vbCrLf, "\N"))
            DB.AddInParameter(DBCommandWrapper, "@DETAIL", DbType.String, Replace(detail.ToString, vbCrLf, "\N"))
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            If transaction IsNot Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub CreateHistory(ByVal transactionType As Integer, ByVal familyID As Integer, ByVal detail As String, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_HISTORY"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, Nothing)
            DB.AddInParameter(DBCommandWrapper, "@TRANSACTION_TYPE", DbType.Int32, transactionType)
            DB.AddInParameter(DBCommandWrapper, "@TRANSACTION_COMMENT", DbType.String, Replace(detail.ToString, vbCrLf, "\N"))
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            If transaction IsNot Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub CreateHistory(ByVal transactionType As Integer, ByVal familyID As Integer, ByVal relationID As Short, ByVal detail As String, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_PATIENT_HISTORY"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@TRANSACTION_TYPE", DbType.Int32, transactionType)
            DB.AddInParameter(DBCommandWrapper, "@TRANSACTION_COMMENT", DbType.String, Replace(detail.ToString, vbCrLf, "\N"))
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            If transaction IsNot Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Function RetrieveDocHistory(ByVal claimID As Integer) As DataTable
        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_DOC_HISTORY_BY_CLAIMID"
        Dim DT As DataTable

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)

            DBCommandWrapper.CommandTimeout = 180

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            DT = New DataTable("DOC_HISTORY")
            If DS.Tables.Count > 0 Then
                DT = DS.Tables(0)
            End If

            Return DT

        Catch ex As Exception
            Throw
        Finally
            If DS IsNot Nothing Then DS.Dispose()
            If DT IsNot Nothing Then DT.Dispose()
        End Try
    End Function

    Public Shared Function RetrieveClaimMasterDocsBySSN(ByVal ssn As Integer, ByVal beginDate As Nullable(Of Date), ByVal endDate As Nullable(Of Date), Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_CLAIM_MASTER_DOCUMENTS_BY_SSN"

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@SSN", DbType.Int32, ssn)
            DB.AddInParameter(DBCommandWrapper, "@BEGINDATE", DbType.Date, beginDate)
            DB.AddInParameter(DBCommandWrapper, "@ENDDATE", DbType.Date, endDate)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
                ds.Tables(0).TableName = "CLAIM_MASTER"
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "CLAIM_MASTER")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "CLAIM_MASTER", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Shared Function RetrieveClaimMasterDocsByTaxID(ByVal taxID As Integer, ByVal beginDate As Nullable(Of Date), ByVal endDate As Nullable(Of Date), Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_CLAIM_MASTER_DOCUMENTS_BY_TAXID"

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@TAXID", DbType.Int32, taxID)
            DB.AddInParameter(DBCommandWrapper, "@BEGINDATE", DbType.Date, If(beginDate Is Nothing, UFCWGeneral.ToNullDateHandler(beginDate), beginDate.Value.ToShortDateString))
            DB.AddInParameter(DBCommandWrapper, "@ENDDATE", DbType.Date, If(endDate Is Nothing, UFCWGeneral.ToNullDateHandler(endDate), endDate.Value.ToShortDateString))

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
                ds.Tables(0).TableName = "CLAIM_MASTER"
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "CLAIM_MASTER")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "CLAIM_MASTER", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Shared Function RetrieveClaimMasterDocsBySSNTaxID(ByVal ssn As Integer, ByVal taxID As Integer, ByVal beginDate As Nullable(Of Date), ByVal endDate As Nullable(Of Date), Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_CLAIM_MASTER_DOCUMENTS_BY_SSN_TAXID"

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@SSN", DbType.Int32, ssn)
            DB.AddInParameter(DBCommandWrapper, "@TAXID", DbType.Int32, taxID)
            DB.AddInParameter(DBCommandWrapper, "@BEGINDATE", DbType.Date, If(beginDate Is Nothing, UFCWGeneral.ToNullDateHandler(beginDate), beginDate.Value.ToShortDateString))
            DB.AddInParameter(DBCommandWrapper, "@ENDDATE", DbType.Date, If(endDate Is Nothing, UFCWGeneral.ToNullDateHandler(endDate), endDate.Value.ToShortDateString))

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
                ds.Tables(0).TableName = "CLAIM_MASTER"
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "CLAIM_MASTER")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "CLAIM_MASTER", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function RetrieveClaimMasterDocsByBatch(ByVal batch As String, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_CLAIM_MASTER_DOCUMENTS_BY_BATCH"

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@BATCH", DbType.String, batch)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
                ds.Tables(0).TableName = "CLAIM_MASTER"
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "CLAIM_MASTER")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "CLAIM_MASTER", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Shared Function RetrieveClaimMasterDocsByReceivedDate(ByVal beginDate As Nullable(Of Date), ByVal endDate As Nullable(Of Date), Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_CLAIM_MASTER_DOCUMENTS_BY_RECEIVE_DATE"

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@BEGINDATE", DbType.Date, If(beginDate Is Nothing, UFCWGeneral.ToNullDateHandler(beginDate), beginDate.Value.ToShortDateString))
            DB.AddInParameter(DBCommandWrapper, "@ENDDATE", DbType.Date, If(endDate Is Nothing, UFCWGeneral.ToNullDateHandler(endDate), endDate.Value.ToShortDateString))

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
                ds.Tables(0).TableName = "CLAIM_MASTER"
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "CLAIM_MASTER")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "CLAIM_MASTER", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function RetrieveClaimMasterDocsByMaxID(ByVal maxID As String, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_CLAIM_MASTER_DOCUMENTS_BY_MAXID"

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@MAXID", DbType.String, maxID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
                ds.Tables(0).TableName = "CLAIM_MASTER"
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "CLAIM_MASTER")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "CLAIM_MASTER", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Shared Function RetrieveClaimMasterDocsByDocID(ByVal docID As Long, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_CLAIM_MASTER_DOCUMENTS_BY_DOCID"

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@DOCID", DbType.Decimal, docID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
                ds.Tables(0).TableName = "CLAIM_MASTER"
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "CLAIM_MASTER")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "CLAIM_MASTER", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function RetrieveDocHistoryBydocID(ByVal docID As Integer) As DataTable
        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_DOC_HISTORY_BY_docID"
        Dim DT As DataTable

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@DOC_ID", DbType.Int64, docID)

            DBCommandWrapper.CommandTimeout = 180

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            DT = New DataTable("DOC_HISTORY")
            If DS.Tables.Count > 0 Then
                DT = DS.Tables(0)
            End If

            Return DT

        Catch ex As Exception
            Throw
        Finally
            If DS IsNot Nothing Then DS.Dispose()
            If DT IsNot Nothing Then DT.Dispose()
        End Try
    End Function

    Public Shared Function RetrieveHistory(ByVal familyID As Integer, ByVal relationID As Short, ByVal transactionType As String) As DataTable
        Dim DS As New DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_HISTORY"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@TRANSACTION_TYPE", DbType.String, transactionType)

            DBCommandWrapper.CommandTimeout = 180

            DB.LoadDataSet(DBCommandWrapper, DS, "HISTORY")

            Return DS.Tables("HISTORY")

        Catch ex As Exception
            Throw
        Finally
            If DS IsNot Nothing Then DS.Dispose()
        End Try
    End Function

    Public Shared Function RetrieveDocHistoryByFamily(ByVal familyID As Integer) As DataTable
        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_DOC_HISTORY_BY_FAMILYID"
        Dim DT As DataTable

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)

            DBCommandWrapper.CommandTimeout = 180

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            DT = New DataTable("DOC_HISTORY")
            If DS.Tables.Count > 0 Then
                DT = DS.Tables(0)
            End If

            Return DT

        Catch ex As Exception
            Throw
        Finally
            If DT IsNot Nothing Then DT.Dispose()
        End Try
    End Function

    Public Shared Function RetrieveDocHistoryByPatient(ByVal familyID As Integer, ByVal relationID As Integer) As DataTable
        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_DOC_HISTORY_BY_FAMILYID_RELATIONID"
        Dim DT As DataTable

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)

            DBCommandWrapper.CommandTimeout = 180

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            DT = New DataTable("DOC_HISTORY")
            If DS.Tables.Count > 0 Then
                DT = DS.Tables(0)
            End If

            Return DT

        Catch ex As Exception
            Throw
        Finally
            If DT IsNot Nothing Then DT.Dispose()
        End Try
    End Function

    Public Shared Function RetrieveProviderByID(ByVal provID As Integer, Optional ByVal transaction As DbTransaction = Nothing) As DataRow
        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_PROVIDER_BY_PROVIDERID"

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PROVIDER_ID", DbType.Int32, provID)

            If transaction Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
            End If

            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0).Rows(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveProvidersByTIN(ByVal tIN As Integer, Optional ByVal transaction As DbTransaction = Nothing) As DataTable

        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_PROVIDER_BY_PROVTIN"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PROV_TIN", DbType.Int64, tIN)

            If transaction Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
            End If

            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveProviderByTIN(ByVal tin As Integer, Optional ByVal transaction As DbTransaction = Nothing) As DataRow
        Dim DT As DataTable

        Try

            DT = CMSDALFDBMD.RetrieveProvidersByTIN(tin, transaction)

            If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
                Return DT.Rows(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function RetrieveProviderByTIN_NPI(ByVal tIN As Integer, ByVal nPI As Decimal, Optional ByVal transaction As DbTransaction = Nothing) As DataTable
        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_PROVIDER_BY_TIN_NPI"

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PROV_TIN", DbType.Int64, tIN)
            DB.AddInParameter(DBCommandWrapper, "@PROV_NPI", DbType.Decimal, nPI)

            If transaction Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
            End If

            If DS.Tables.Count > 0 Then
                DS.Tables(0).TableName = "PROVIDER"
                Return DS.Tables(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveCompleteProviderInfoByTIN(ByVal tIN As Integer, Optional ByVal transaction As DbTransaction = Nothing) As DataRow
        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_PROVIDER_BY_TAXID"

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PROV_TIN", DbType.Int64, tIN)

            If transaction Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
            End If

            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0).Rows(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveRegMasterByFamilyID(ByVal fAMILYID As Integer?, Optional ByVal transaction As DbTransaction = Nothing) As DataTable
        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_REG_MASTER_BY_FAMILYID"

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, fAMILYID)

            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
            End If

            If DS.Tables.Count > 0 Then
                DS.Tables(0).TableName = "REG_MASTER"
                Return DS.Tables(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveRegMaster(ByVal partSSN As Integer, ByVal patSSN As Integer, Optional ByVal transaction As DbTransaction = Nothing) As DataRow
        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_REG_MASTER_BY_PARTSSN_DEPSSN"

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, partSSN)
            DB.AddInParameter(DBCommandWrapper, "@DEP_SSN", DbType.Int32, patSSN)

            If transaction Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
            End If

            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0).Rows(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Sub CreateCurrentDetails(ByVal claimID As Integer, ByVal lineNbr As Short, ByVal occFromDate As Date?, ByVal occToDate As Date?, ByVal procCode As String, ByVal chrgAmt As Decimal?, ByVal modifier As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_DUPLICATE_DETAILS"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@LINE_NBR", DbType.Int16, lineNbr)
            DB.AddInParameter(DBCommandWrapper, "@OCC_FROM_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(occFromDate))
            DB.AddInParameter(DBCommandWrapper, "@OCC_TO_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(occToDate))
            DB.AddInParameter(DBCommandWrapper, "@PROC_CODE", DbType.String, procCode)
            DB.AddInParameter(DBCommandWrapper, "@CHRG_AMT", DbType.Decimal, chrgAmt)
            DB.AddInParameter(DBCommandWrapper, "@MODIFIER", DbType.String, modifier)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Function RetrieveAssociated(ByVal claimID As Integer, ByVal auditMode As Boolean) As DataSet

        Dim DB As Database
        Dim DBConnection As DbConnection
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String
        Dim DS As DataSet
        Dim Tablenames() As String = {"HEADER", "DETAIL", "REASON", "DIAGNOSIS", "MEDMOD"}

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBConnection = DB.CreateConnection()
            DS = New DataSet

            SQLCall = "FDBMD.RETRIEVE_ASSOCIATED" & If(auditMode, "_AUDIT", "")

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)

            DBCommandWrapper.CommandTimeout = 0

            DB.LoadDataSet(DBCommandWrapper, DS, Tablenames)

            Return DS

        Catch ex As DB2Exception

            Throw
        Catch ex As Exception

            Throw
        Finally

            If DS IsNot Nothing Then DS.Dispose()

            If DBConnection IsNot Nothing Then
                DBConnection.Close()

                DBConnection = Nothing
                If DB IsNot Nothing Then
                    DB = Nothing
                End If
            End If

        End Try
    End Function

    Public Shared Function RetrieveDuplicates(ByVal claimID As Integer, ByVal familyID As Integer, ByVal relationID As Short, ByVal provTIN As Integer?, ByVal fromDate As Date?, ByVal retrieveClaim As Decimal) As DataSet

        Dim DB As Database
        Dim DBConnection As DbConnection
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String
        Dim DS As New DataSet
        Dim Tablenames() As String = {"EXACT", "PARTIAL", "HEADER", "DETAIL", "REASON", "DIAGNOSIS"}

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBConnection = DB.CreateConnection()

            Try

                SQLCall = "FDBMD.RUNIMMEDIATEOTHER"
                DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
                DB.AddInParameter(DBCommandWrapper, "@SQLInput", DbType.String, "COMMIT;")
                DB.ExecuteNonQuery(DBCommandWrapper)

            Catch ex As Exception

            End Try

            SQLCall = "FDBMD.RETRIEVE_DUPLICATES_UI"

#If Fixed = True Then
            SQLCall += "_FIXED"
#End If
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@PROV_TIN", DbType.Int64, provTIN)
            DB.AddInParameter(DBCommandWrapper, "@OCC_FROM_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(fromDate))
            DB.AddInParameter(DBCommandWrapper, "@DONT_RETRIEVE_CLAIM", DbType.Decimal, retrieveClaim)

            DBCommandWrapper.CommandTimeout = 0

            DB.LoadDataSet(DBCommandWrapper, DS, Tablenames)

            Return DS

        Catch ex As DB2Exception

            Throw
        Catch ex As Exception

            Throw
        Finally

            If DS IsNot Nothing Then DS.Dispose()

            If DBConnection IsNot Nothing Then
                DBConnection.Close()

                DBConnection = Nothing
                If DB IsNot Nothing Then
                    DB = Nothing
                End If
            End If

        End Try
    End Function

    Public Shared Function RetrieveRefrenceDocs(ByVal referenceID As Integer) As DataTable
        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_CLAIM_MASTER_BY_REFRENCE_CLAIM"
        Dim DT As DataTable

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@REFRENCE_CLAIM", DbType.Int32, referenceID)

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            DT = New DataTable("CLAIM_MASTER")
            If DS.Tables.Count > 0 Then
                DT = DS.Tables(0)
            End If

            Return DT

        Catch ex As Exception
            Throw
        Finally

            If DT IsNot Nothing Then DT.Dispose()

        End Try
    End Function

    Public Shared Function RouteClaimToUser(ByVal claimID As Integer, ByVal claimLastUpdated As Date, ByVal routedBy As String, ByVal pendedTo As String, ByVal routingComment As String, ByVal transactionType As String, ByVal newLastUpdt As Date, Optional ByVal transaction As DbTransaction = Nothing) As Boolean
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.ROUTE_CLAIM_TO_USER_TS"
        Dim HistSum As String = "CLAIM ID " & Format(claimID, "00000000") & " REASSIGNED TO " & pendedTo

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@ROUTED_BY", DbType.String, routedBy)
            DB.AddInParameter(DBCommandWrapper, "@PENDED_TO", DbType.String, pendedTo)
            DB.AddInParameter(DBCommandWrapper, "@LASTUPDT", DbType.DateTime, claimLastUpdated)
            DB.AddInParameter(DBCommandWrapper, "@TRANSACTION_TYPE", DbType.String, transactionType)
            DB.AddInParameter(DBCommandWrapper, "@SUMMARY", DbType.String, Replace(HistSum, vbCrLf, "\N"))
            DB.AddInParameter(DBCommandWrapper, "@DETAIL", DbType.String, Replace(routingComment, vbCrLf, "\N"))
            DB.AddInParameter(DBCommandWrapper, "@ROUTING_COMMENT", DbType.String, Replace(routingComment, vbCrLf, "\N"))
            DB.AddInParameter(DBCommandWrapper, "@NEWLASTUPDT", DbType.DateTime, newLastUpdt)

            If transaction IsNot Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper)
            End If

            Return True

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Sub CreateRoutingHistory(ByVal claimID As Integer, ByVal routedBy As String, ByVal routedTo As String, ByVal routingComment As String, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_ROUTING_HISTORY"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@ROUTED_BY", DbType.String, routedBy)
            DB.AddInParameter(DBCommandWrapper, "@ROUTED_TO", DbType.String, routedTo)
            DB.AddInParameter(DBCommandWrapper, "@ROUTING_COMMENT", DbType.String, Replace(routingComment, vbCrLf, "\N"))
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            If transaction IsNot Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Function RetrieveRoutingHistory(ByVal claimID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_ROUTING_HISTORY_BY_CLAIMID"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "ROUTING_HISTORY")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "ROUTING_HISTORY", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveRoutingUsers(ByVal docType As String, ByVal userRights As String, ByVal mode As String) As DataTable
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_USER_DOC_TYPES_BY_DOCTYPE"
        Dim DS As DataSet
        Dim DT As DataTable

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@DOC_TYPE", DbType.String, docType)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@MODE", DbType.String, mode)

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            DT = New DataTable("USER_DOC_TYPES")

            If DS.Tables.Count > 0 Then
                DT = DS.Tables(0)
            End If

            Return DT

        Catch ex As Exception
            Throw
        Finally

            If DT IsNot Nothing Then DT.Dispose()

        End Try
    End Function

    Public Shared Function RetrieveUsersWithOptionalDoctype(Optional ByVal docType As String = "", Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_USERS_WITH_OPTIONAL_DOCTYPE"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@DOC_TYPE", DbType.String, docType)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "VALID_USERS")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "VALID_USERS", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Sub UpdateClaimMasterRouting(ByVal claimID As Integer, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.UPDATE_CLAIM_MASTER_ROUTING_BY_CLAIMID"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Function RetrievePatientParticipantAddresses(ByVal familyId As Integer, ByVal relationId As Short, Optional ByVal ds As DataSet = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_PARTICIPANT_ADDRESSES"
        Dim Tablenames() As String = {"PARTICIPANT_ADDRESS", "PATIENT_ADDRESS"}

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyId)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationId)

            If ds Is Nothing Then
                ds = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DB.LoadDataSet(DBCommandWrapper, ds, Tablenames)
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveProviderAndNPIInfo(ByVal providerID As Integer?, ByVal nPI As Decimal?, Optional ByVal ds As DataSet = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_PROVIDER_AND_NPI_REGISTRY"
        Dim Tablenames() As String = {"PROVIDER_ADDRESS", "NPI_REGISTRY"}

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@NPI", DbType.Decimal, nPI)
            DB.AddInParameter(DBCommandWrapper, "@PROVIDER_ID", DbType.Int32, providerID)

            If ds Is Nothing Then
                ds = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DB.LoadDataSet(DBCommandWrapper, ds, Tablenames)
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveProviderInfo(ByVal tin As Integer, Optional ByVal transaction As DbTransaction = Nothing) As DataRow
        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_PROVIDER_BY_PROVTIN"

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PROV_TIN", DbType.Int32, tin)

            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
            End If

            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0).Rows(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveProviders(ByVal provTIN As Integer?, ByVal provName As String, ByVal npi As Decimal?, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_PROVIDERS_BY_PROVTINNPI_NAME"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PROV_TIN", DbType.Int32, provTIN)
            DB.AddInParameter(DBCommandWrapper, "@PROV_NAME", DbType.String, Replace(provName, "'", "''"))
            DB.AddInParameter(DBCommandWrapper, "@NPI", DbType.Decimal, npi)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "PROVIDER")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "PROVIDER", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveProvideraddressInfo(ByVal provID As Integer, Optional ByVal transaction As DbTransaction = Nothing) As DataRow
        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_PROVIDERMAILaddress_BY_PROVIDERID"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PROVIDER_ID", DbType.Int32, provID)

            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
            End If

            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0).Rows(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Sub SaveClaimsHistoryXML()

        Dim XMLFilename As String
        Dim RententionDays As Integer
        Dim SaveDT As DataTable
        Dim StreamWriter As System.IO.StreamWriter
        Dim Serializer As XmlSerializer
        Dim DV As DataView

        Try

            If System.Configuration.ConfigurationManager.AppSettings("TransactionHistory") IsNot Nothing Then
                XMLFilename = Path.Combine(UFCWGeneral.StartupPath, System.Configuration.ConfigurationManager.AppSettings("TransactionHistory"))
            Else
                XMLFilename = Path.Combine(UFCWGeneral.StartupPath, "ClaimsHistory.xml")
            End If

            If System.Configuration.ConfigurationManager.AppSettings("TransactionHistoryRetentionInDays") IsNot Nothing Then
                RententionDays = CInt(System.Configuration.ConfigurationManager.AppSettings("TransactionHistoryRetentionInDays"))
            Else
                RententionDays = 30
            End If

            Try
                File.Delete(XMLFilename)
            Catch ex As Exception
                'if file doesn't exist ignore error
            End Try

            If _ClaimsHistoryDT IsNot Nothing Then

                StreamWriter = New System.IO.StreamWriter(XMLFilename)
                Serializer = New XmlSerializer(_ClaimsHistoryDT.GetType())

                DV = New DataView(_ClaimsHistoryDT, "LAST_ACCESSED_DATE > #" & UFCWGeneral.NowDate.AddDays(CInt(RententionDays.ToString) * -1) & "#", "", DataViewRowState.CurrentRows)
                SaveDT = DV.ToTable

                Serializer.Serialize(StreamWriter, SaveDT)

            End If

        Catch ex As Exception
            Throw
        Finally
            If StreamWriter IsNot Nothing Then StreamWriter.Close()
            StreamWriter = Nothing

            If DV IsNot Nothing Then DV.Dispose()

        End Try

    End Sub

    Public Shared Function LoadClaimsHistoryXML() As DataTable

        Dim Serializer As XmlSerializer
        Dim FileStream As FileStream
        Dim ResultDataSet As DataSet
        Dim XMLFilename As String

        Try

            If _ClaimsHistoryDT Is Nothing Then 'Use InMemory Cache not loaded check if the XML file already exists, and load

                If System.Configuration.ConfigurationManager.AppSettings("TransactionHistory") IsNot Nothing Then
                    XMLFilename = Path.Combine(UFCWGeneral.StartupPath, System.Configuration.ConfigurationManager.AppSettings("TransactionHistory"))
                Else
                    XMLFilename = Path.Combine(UFCWGeneral.StartupPath, "ClaimsHistory.xml")
                End If

                If File.Exists(XMLFilename) Then

                    _ClaimsHistoryDT = New DataTable

                    Serializer = New XmlSerializer(_ClaimsHistoryDT.GetType)
                    FileStream = New FileStream(XMLFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                    '' Create the object from the xml file
                    _ClaimsHistoryDT = CType(Serializer.Deserialize(FileStream), DataTable)

                End If
            End If

            Return _ClaimsHistoryDT

        Catch ex As Exception

            Throw

        Finally

            If FileStream IsNot Nothing Then FileStream.Close()
            FileStream = Nothing

            Serializer = Nothing

        End Try

    End Function

    Public Shared Sub WriteToClaimsHistoryXML(ByVal claimID As Integer, ByVal ssn As Integer?, ByVal docID As Long?, ByVal familyID As Integer?, ByVal accessed As DateTime, ByVal action As String)
        Dim NewDR As DataRow
        Dim DC As DataColumn
        Dim XMLFilename As String

        Try

            If _ClaimsHistoryDT Is Nothing Then 'Use InMemory Cache not loaded check if the XML file already exists, and load

                If System.Configuration.ConfigurationManager.AppSettings("TransactionHistory") IsNot Nothing Then
                    XMLFilename = Path.Combine(UFCWGeneral.StartupPath, System.Configuration.ConfigurationManager.AppSettings("TransactionHistory"))
                Else
                    XMLFilename = Path.Combine(UFCWGeneral.StartupPath, "ClaimsHistory.xml")
                End If

                If File.Exists(XMLFilename) Then
                    'Load existing XML
                    _ClaimsHistoryDT = LoadClaimsHistoryXML()
                Else

                    _ClaimsHistoryDT = New DataTable("ClaimsHistory")

                    ' Create Claims History Table structure
                    DC = New DataColumn("CLAIM_ID") With {
                        .DataType = System.Type.GetType("System.String")
                    }
                    _ClaimsHistoryDT.Columns.Add(DC)

                    DC = New DataColumn("docID") With {
                        .DataType = System.Type.GetType("System.String")
                    }
                    _ClaimsHistoryDT.Columns.Add(DC)

                    DC = New DataColumn("FAMILY_ID") With {
                        .DataType = System.Type.GetType("System.String")
                    }
                    _ClaimsHistoryDT.Columns.Add(DC)

                    DC = New DataColumn("PART_SSN") With {
                        .DataType = System.Type.GetType("System.String")
                    }
                    _ClaimsHistoryDT.Columns.Add(DC)

                    DC = New DataColumn("LAST_ACCESSED_DATE") With {
                        .DataType = System.Type.GetType("System.DateTime")
                    }
                    _ClaimsHistoryDT.Columns.Add(DC)

                    DC = New DataColumn("STATUS") With {
                        .DataType = System.Type.GetType("System.String")
                    }
                    _ClaimsHistoryDT.Columns.Add(DC)
                End If

            End If

            NewDR = _ClaimsHistoryDT.NewRow()

            NewDR("CLAIM_ID") = claimID.ToString
            NewDR("docID") = docID.ToString
            NewDR("FAMILY_ID") = familyID.ToString
            NewDR("PART_SSN") = ssn.ToString
            NewDR("LAST_ACCESSED_DATE") = accessed
            NewDR("STATUS") = action

            _ClaimsHistoryDT.Rows.Add(NewDR)

            RaiseEvent RecentClaimRefreshAvailable(New RecentClaimEventArgs(_ClaimsHistoryDT))

        Catch ex As Exception

            Throw
        Finally

            If DC IsNot Nothing Then DC.Dispose()
            DC = Nothing

        End Try

    End Sub

    Public Shared Function RetrieveDisabledUsersForRoute() As DataTable

        Try
            Dim SQLCall As String = "SELECT DISTINCT USER_DOC_TYPES.USERID FROM FDBMD.USER_DOC_TYPES AS USER_DOC_TYPES WHERE " &
                                    " DOC_TYPE='' ORDER BY USER_DOC_TYPES.USERID ASC " &
                                    " FOR READ ONLY"

            Return RUNIMMEDIATESELECT(SQLCall)

        Catch ex As Exception
            Throw
        End Try

    End Function

#End Region
#Region "Eligibility"
    Public Shared Function GetEligibilityInformation(ByVal familyId As Integer, ByVal relationId As Short, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBEL.RETRIEVE_ELIGIBILITY_BY_FAMILYID_RELATIONID")

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyId)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationId)
            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "REG_MASTER")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "REG_MASTER", transaction)
                End If
            End If

            ds.Tables(0).TableName = "REG_MASTER"
            ds.Tables(1).TableName = "ISELIGIBLE"

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetHours(ByVal familyId As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RETRIEVE_HOURS_BY_FAMILYID")

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyId)
            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "HOURS")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "HOURS", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GetPremLetterHistory(ByVal familyId As Integer, Optional ByVal DS As DataSet = Nothing, Optional ByVal Transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand = DB.GetStoredProcCommand("FDBMD.RETRIEVE_PREMLETTERHISTORY_BY_FAMILYID")
        Try
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyId)
            DB.AddInParameter(DBCommandWrapper, "@EMPLOYEE_ACCESS", DbType.Int32, If(UFCWGeneralAD.CMSCanAdjudicateEmployee() Or UFCWGeneralAD.CMSEligibilityEmployee(), 1, 0))
            DB.AddInParameter(DBCommandWrapper, "@LOCAL_ACCESS", DbType.Int32, If(UFCWGeneralAD.CMSLocalsEmployee() Or UFCWGeneralAD.CMSEligibility(), 1, 0))
            DBCommandWrapper.CommandTimeout = 180

            If DS Is Nothing Then
                If Transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, Transaction)
                End If
                DS.Tables(0).TableName = "PREMLETTER"
            Else
                If Transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "PREMLETTER")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "PREMLETTER", Transaction)
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
            DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RETRIEVE_ELIGIBILITY_HOURS_BY_FAMILYID")

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

    Public Shared Function GetCOBHistory(ByVal familyId As Integer, ByVal relationid As Integer, ByVal claimid As Integer) As Integer

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RETRIEVE_LASTCOB_BY_FAMILYID")

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyId)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationid)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimid)
            DB.AddOutParameter(DBCommandWrapper, "@COB", DbType.String, 5)
            DBCommandWrapper.CommandTimeout = 180

            DB.ExecuteNonQuery(DBCommandWrapper)

            If IsDBNull(DB.GetParameterValue(DBCommandWrapper, "@COB")) Then
                Return 0
            Else
                Return CInt(DB.GetParameterValue(DBCommandWrapper, "@COB"))
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function RetrieveCOBInfo(ByVal familyID As Integer, ByVal relationID As Short, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_MEDOTHER_INS_BY_FAMILYID_RELATIONID"
        Dim tablenames() As String = {"MEDOTHER_INS", "MEDOTHER_INS_COUNT"}

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If

                Dim tableNumber As Integer = 0
                For Each DT As DataTable In ds.Tables
                    DT.TableName = tablenames(tableNumber)
                    tableNumber += 1
                Next

            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, tablenames)
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, tablenames, transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveParticipantInfo(ByVal partSSN As Integer, Optional ByVal transaction As DbTransaction = Nothing) As DataRow
        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_REG_MASTER_BY_PARTSSN"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, partSSN)

            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
            End If

            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0).Rows(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrievePatientInfo(ByVal patSSN As Integer, Optional ByVal transaction As DbTransaction = Nothing) As DataRow

        Dim DT As DataTable

        Try

            DT = RetrievePatientsInfo(patSSN, transaction)
            If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
                Return DT.Rows(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function RetrievePatientInfo(ByVal familyID As Integer, ByVal patSSN As Integer, Optional ByVal transaction As DbTransaction = Nothing) As DataRow
        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_REG_MASTER_BY_FAMILYID_PATSSN"

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@PAT_SSN", DbType.Int32, patSSN)

            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
            End If

            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0).Rows(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function RetrievePatientInfo(ByVal familyID As Integer, ByVal relationID As Short, Optional ByVal transaction As DbTransaction = Nothing) As DataRow
        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_REG_MASTER_BY_FAMILYID_RELATIONID"

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)

            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
            End If

            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0).Rows(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Function

    Public Shared Function RetrieveParticipantsByFirstNameLastName(ByVal firstName As String, ByVal lastName As String) As DataTable

        Dim DS As DataSet

        Try

            DS = RetrieveParticipantsByFirstNameLastName(firstName, lastName, Nothing, Nothing)

            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function RetrieveParticipantsByFirstNameLastName(ByVal firstName As String, ByVal lastName As String, ByVal ds As DataSet, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_REG_MASTER_PART_BY_FIRSTNAME_LASTNAME"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FIRST_NAME", DbType.String, If(firstName Is Nothing OrElse firstName.Trim = "%", "", firstName))
            DB.AddInParameter(DBCommandWrapper, "@LAST_NAME", DbType.String, If(lastName Is Nothing OrElse lastName.ToString.Trim.Length = 0, Nothing, lastName))
            DB.AddInParameter(DBCommandWrapper, "@LOCALS_ACCESS", DbType.Decimal, If(UFCWGeneralAD.CMSLocals(), 1, 0))
            DB.AddOutParameter(DBCommandWrapper, "@SQLOUT", DbType.String, 8000)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "REG_MASTER")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "REG_MASTER", transaction)
                End If
            End If

            Dim SQLOut As String = CStr(DB.GetParameterValue(DBCommandWrapper, "@SQLOUT"))

            Return ds

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Function

    Public Shared Function RetrievePatientsByLastNameDOB(ByVal lastName As String, ByVal dOB As Date) As DataTable

        Dim DS As DataSet

        Try

            DS = RetrievePatientsByLastNameDOB(lastName, dOB, Nothing, Nothing)

            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function RetrievePatientsByLastNameDOB(ByVal lastName As String, ByVal dOB As Date, ByVal ds As DataSet, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_REG_MASTER_PAT_BY_LASTNAME_DOB"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@LAST_NAME", DbType.String, lastName & "%")
            DB.AddInParameter(DBCommandWrapper, "@DOB", DbType.Date, UFCWGeneral.ToNullDateHandler(dOB))

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "REG_MASTER")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "REG_MASTER", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        Finally

        End Try
    End Function


    Public Shared Function RetrievePatientsByFirstNameLastName(ByVal firstName As String, ByVal lastName As String) As DataTable
        Dim DS As DataSet

        Try

            DS = RetrievePatientsByFirstNameLastName(firstName, lastName, Nothing, Nothing)

            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function RetrievePatientsByFirstNameLastName(ByVal firstName As String, ByVal lastName As String, ByVal ds As DataSet, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_REG_MASTER_PAT_BY_FIRSTNAME_LASTNAME"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FIRST_NAME", DbType.String, firstName.Trim & "%")
            DB.AddInParameter(DBCommandWrapper, "@LAST_NAME", DbType.String, lastName & "%")

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "REG_MASTER")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "REG_MASTER", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Function

    Public Shared Function RetrievePeople(ByVal firstName As String, ByVal lastName As String, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_REG_MASTER_BY_LASTNAME_FIRSTNAME_SECURE"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FIRST_NAME", DbType.String, firstName)
            DB.AddInParameter(DBCommandWrapper, "@LAST_NAME", DbType.String, lastName)
            DB.AddInParameter(DBCommandWrapper, "@EMPLOYEE_ACCESS", DbType.Int32, If(UFCWGeneralAD.CMSCanAdjudicateEmployee() OrElse UFCWGeneralAD.CMSEligibilityEmployee() OrElse UFCWGeneralAD.REGMEmployeeAccess, 1, 0))
            DB.AddInParameter(DBCommandWrapper, "@LOCAL_ACCESS", DbType.Int32, If(UFCWGeneralAD.CMSUsers() OrElse UFCWGeneralAD.CMSDental() OrElse UFCWGeneralAD.CMSLocalsEmployee() OrElse UFCWGeneralAD.CMSEligibility(), 1, 0))
            'db.AddOutParameter(dbCommandWrapper, "@SQLOUT", DbType.String, 8000)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "REG_MASTER")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "REG_MASTER", transaction)
                End If
            End If

            'Dim SQLOut As String = db.GetParameterValue(dbCommandWrapper, "@SQLOUT")

            Return ds
        Catch ex As Exception
            Throw
        Finally

        End Try

    End Function
    Public Shared Function RetrievePeopleWithLast4SSN(ByVal last4SSN As String, ByVal firstName As String, ByVal lastName As String, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_REG_MASTER_BY_LASTNAME_FIRSTNAME_4SSN_SECURE"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@LAST4SSN", DbType.String, last4SSN)
            DB.AddInParameter(DBCommandWrapper, "@FIRST_NAME", DbType.String, If(firstName Is Nothing OrElse firstName.Trim = "%", "", firstName))
            DB.AddInParameter(DBCommandWrapper, "@LAST_NAME", DbType.String, lastName)
            DB.AddInParameter(DBCommandWrapper, "@EMPLOYEE_ACCESS", DbType.Int32, If(UFCWGeneralAD.CMSCanAdjudicateEmployee() Or UFCWGeneralAD.CMSEligibilityEmployee() Or UFCWGeneralAD.REGMEmployeeAccess, 1, 0))
            DB.AddInParameter(DBCommandWrapper, "@LOCAL_ACCESS", DbType.Int32, If(UFCWGeneralAD.CMSUsers() Or UFCWGeneralAD.CMSDental() Or UFCWGeneralAD.CMSLocalsEmployee() Or UFCWGeneralAD.CMSEligibility(), 1, 0))
            'db.AddOutParameter(dbCommandWrapper, "@SQLOUT", DbType.String, 8000)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "REG_MASTER")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "REG_MASTER", transaction)
                End If
            End If

            'Dim SQLOut As String = db.GetParameterValue(dbCommandWrapper, "@SQLOUT")

            Return ds

        Catch ex As Exception
            Throw
        Finally


        End Try
    End Function

    Public Shared Function RetrievePeopleBySoundex(ByVal firstName As String, ByVal lastName As String, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_REG_MASTER_BY_LAST_FIRST_SOUNDEX_SECURE"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FIRST_NAME", DbType.String, If(firstName Is Nothing OrElse firstName.Trim = "%", "", firstName))
            DB.AddInParameter(DBCommandWrapper, "@LAST_NAME", DbType.String, lastName)
            DB.AddInParameter(DBCommandWrapper, "@EMPLOYEE_ACCESS", DbType.Int32, If(UFCWGeneralAD.CMSCanAdjudicateEmployee() Or UFCWGeneralAD.CMSEligibilityEmployee() Or UFCWGeneralAD.REGMEmployeeAccess, 1, 0))
            DB.AddInParameter(DBCommandWrapper, "@LOCAL_ACCESS", DbType.Int32, If(UFCWGeneralAD.CMSUsers() Or UFCWGeneralAD.CMSDental() Or UFCWGeneralAD.CMSLocalsEmployee() Or UFCWGeneralAD.CMSEligibility(), 1, 0))
            'db.AddOutParameter(dbCommandWrapper, "@SQLOUT", DbType.String, 8000)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "REG_MASTER")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "REG_MASTER", transaction)
                End If
            End If

            'Dim SQLOut As String = db.GetParameterValue(dbCommandWrapper, "@SQLOUT")

            Return ds

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Function
    Public Shared Function RetrievePeopleBySoundexWithLast4SSN(ByVal last4SSN As String, ByVal firstName As String, ByVal lastName As String, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_REG_MASTER_BY_LAST_FIRST_4SSN_SOUNDEX_SECURE"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@LAST4SSN", DbType.String, last4SSN)
            DB.AddInParameter(DBCommandWrapper, "@FIRST_NAME", DbType.String, If(firstName Is Nothing OrElse firstName.Trim = "%", "", firstName))
            DB.AddInParameter(DBCommandWrapper, "@LAST_NAME", DbType.String, lastName)
            DB.AddInParameter(DBCommandWrapper, "@EMPLOYEE_ACCESS", DbType.Int32, If(UFCWGeneralAD.CMSCanAdjudicateEmployee() Or UFCWGeneralAD.CMSEligibilityEmployee() Or UFCWGeneralAD.REGMEmployeeAccess, 1, 0))
            DB.AddInParameter(DBCommandWrapper, "@LOCAL_ACCESS", DbType.Int32, If(UFCWGeneralAD.CMSUsers() Or UFCWGeneralAD.CMSDental() Or UFCWGeneralAD.CMSLocalsEmployee() Or UFCWGeneralAD.CMSEligibility(), 1, 0))
            'db.AddOutParameter(dbCommandWrapper, "@SQLOUT", DbType.String, 8000)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "REG_MASTER")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "REG_MASTER", transaction)
                End If
            End If

            'Dim SQLOut As String = db.GetParameterValue(dbCommandWrapper, "@SQLOUT")

            Return ds

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Function

    Public Shared Function RetrievePeopleByLast4SSN(ByVal last4SSN As String, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_REG_MASTER_BY_LAST_4SSN_SECURE"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@LAST4SSN", DbType.String, last4SSN)
            DB.AddInParameter(DBCommandWrapper, "@EMPLOYEE_ACCESS", DbType.Int32, If(UFCWGeneralAD.CMSCanAdjudicateEmployee() Or UFCWGeneralAD.CMSEligibilityEmployee() Or UFCWGeneralAD.REGMEmployeeAccess, 1, 0))
            DB.AddInParameter(DBCommandWrapper, "@LOCAL_ACCESS", DbType.Int32, If(UFCWGeneralAD.CMSUsers() Or UFCWGeneralAD.CMSDental() Or UFCWGeneralAD.CMSLocalsEmployee() Or UFCWGeneralAD.CMSEligibility(), 1, 0))
            'db.AddOutParameter(dbCommandWrapper, "@SQLOUT", DbType.String, 8000)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "REG_MASTER")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "REG_MASTER", transaction)
                End If
            End If

            'Dim SQLOut As String = db.GetParameterValue(dbCommandWrapper, "@SQLOUT")

            Return ds

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Function

    Public Shared Function RetrievePatients(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim PatientsDS As DataSet = RetrievePatientsDemographics(familyID, ds, transaction)
        Dim PatientDR As DataRow

        For x As Integer = 0 To PatientsDS.Tables.Count - 1
            For Each DR As DataRow In PatientsDS.Tables(x).Rows
                Select Case DR("RELATION").ToString
                    Case "S", "D", "B", "G", "H", "W", "P", "M"
                        PatientDR = CMSDALFDBEL.RetrievePatientsDaysNotCoverable(CInt(DR("FAMILY_ID")), CInt(DR("RELATION_ID")), CDate(DR("FROM_DATE")), CDate(DR("THRU_DATE")), ds, transaction)
                        DR("ELIGIBILITY_GAP_COUNT") = PatientDR("ELIGIBILITY_GAP_COUNT")
                        DR("ELIGIBILITY_MAX_GAP_COUNT") = PatientDR("ELIGIBILITY_MAX_GAP_COUNT")
                    Case Else
                        DR("ELIGIBILITY_GAP_COUNT") = 0
                        DR("ELIGIBILITY_MAX_GAP_COUNT") = 0
                End Select
            Next

        Next

        Return PatientsDS

    End Function


    Public Shared Function RetrievePatientsDemographics(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_REG_MASTER_BY_FAMILYID_SECURE"

        Try


            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@EMPLOYEE_ACCESS", DbType.Int32, If(UFCWGeneralAD.CMSCanAdjudicateEmployee() Or UFCWGeneralAD.CMSEligibilityEmployee(), 1, 0))
            DB.AddInParameter(DBCommandWrapper, "@LOCAL_ACCESS", DbType.Int32, If(UFCWGeneralAD.CMSUsers() Or UFCWGeneralAD.CMSDental() Or UFCWGeneralAD.CMSLocalsEmployee() Or UFCWGeneralAD.CMSEligibility(), 1, 0))
            DB.AddOutParameter(DBCommandWrapper, "@DUAL_COVERAGE", DbType.Int32, 1)
            DB.AddOutParameter(DBCommandWrapper, "@DUAL_COVERAGE_FAMILY_ID", DbType.Int32, 8)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
                ds.Tables(0).TableName = "REG_MASTER"
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "REG_MASTER")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "REG_MASTER", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveRedbookLBLByMFGCODE(ByVal mfgCode As String, Optional ByVal ds As DataSet = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_REDBOOK_LBL_BY_MFGCODE"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@MFGCODE", DbType.String, mfgCode)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                ds = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DB.LoadDataSet(DBCommandWrapper, ds, "REDBOOK_LBL_Y2K")
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveRedbookGFCByGCRCODE(ByVal GCRCode As Integer, Optional ByVal ds As DataSet = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_REDBOOK_GFC_BY_GCRCODE"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@GCRCODE", DbType.Int32, GCRCode)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                ds = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DB.LoadDataSet(DBCommandWrapper, ds, "REDBOOK_GFC")
            End If

            Return ds
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrievePatientsInfo(ByVal patSSN As Integer, Optional ByVal transaction As DbTransaction = Nothing) As DataTable
        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_REG_MASTER_BY_PATSSN"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PAT_SSN", DbType.Int32, patSSN)

            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
            End If

            If DS.Tables.Count > 0 Then
                DS.Tables(0).TableName = "REG_MASTER"
                Return DS.Tables(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function RetrieveParticipantInfofromPartSSN(ByVal partSSN As Integer, Optional ByVal transaction As DbTransaction = Nothing) As DataRow

        Dim DT As DataTable
        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_REG_MASTER_BY_PARTSSN"

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, partSSN)

            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
            End If

            DT = New DataTable("REG_MASTER")
            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0).Rows(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        Finally

            If DT IsNot Nothing Then DT.Dispose()
        End Try
    End Function

    Public Shared Function RetrieveFamilyInfoByPartSSN(ByVal partSSN As Integer, Optional ByVal transaction As DbTransaction = Nothing) As DataTable
        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_REG_MASTER_ALL_BY_PARTSSN"
        Dim DT As DataTable

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, partSSN)

            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
            End If

            DT = New DataTable("REG_MASTER")
            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        Finally
            If DT IsNot Nothing Then DT.Dispose()

        End Try
    End Function

    Public Shared Function RetrieveFamilyIDRelationIDByMaxIDOrReferenceID(ByVal indentifier As String, ByVal ds As DataSet, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Dim SQLCall As String

        Try

            If Not IsNumeric(indentifier.Substring(0, 1)) Then
                SQLCall = "FDBMD.RETRIEVE_CLAIM_MASTER_FAMILYID_RELATIONID_BY_MAXID"
            Else
                SQLCall = "FDBMD.RETRIEVE_CLAIM_MASTER_FAMILYID_RELATIONID_BY_REFERENCEID"
            End If

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIMID", DbType.String, indentifier)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
                ds.Tables(0).TableName = "CLAIM_MASTER"
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "CLAIM_MASTER")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "CLAIM_MASTER", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveFamilyIDRelationIDByMaxIDOrReferenceID(ByVal indentifier As String) As DataTable

        Dim DS As DataSet

        Try

            DS = RetrieveFamilyIDRelationIDByMaxIDOrReferenceID(indentifier, Nothing)

            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function RetrieveFamilyIDRelationIDByClaimID(ByVal claimID As Integer, ByVal ds As DataSet, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_CLAIM_MASTER_FAMILYID_RELATIONID_BY_CLAIMID"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIMID", DbType.Int32, claimID)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
                ds.Tables(0).TableName = "CLAIM_MASTER"
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "CLAIM_MASTER")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "CLAIM_MASTER", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveFamilyIDRelationIDByClaimID(ByVal claimID As Integer) As DataTable
        Dim DS As DataSet

        Try

            DS = RetrieveFamilyIDRelationIDByClaimID(claimID, Nothing)

            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Shared Function RetrievePatientsBySSN(ByVal ssn As Integer, ByVal ds As DataSet, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_REG_MASTER_BY_SNNO_SECURE"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@SSNO", DbType.Int32, ssn)
            DB.AddInParameter(DBCommandWrapper, "@EMPLOYEE_ACCESS", DbType.Int32, If(UFCWGeneralAD.CMSCanAdjudicateEmployee() Or UFCWGeneralAD.CMSEligibilityEmployee(), 1, 0))
            DB.AddInParameter(DBCommandWrapper, "@LOCAL_ACCESS", DbType.Int32, If(UFCWGeneralAD.CMSUsers() Or UFCWGeneralAD.CMSDental() Or UFCWGeneralAD.CMSLocalsEmployee() Or UFCWGeneralAD.CMSEligibility(), 1, 0))

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
                ds.Tables(0).TableName = "REG_MASTER"
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "REG_MASTER")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "REG_MASTER", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrievePatientsBySSN(ByVal ssn As Integer) As DataTable
        Dim DS As DataSet = RetrievePatientsBySSN(ssn, Nothing)

        If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
            Return DS.Tables(0)
        Else
            Return Nothing
        End If

    End Function
    Public Shared Function RetrieveDemographicsByFamilyID(ByVal familyID As Integer?) As DataRow
        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_REGaddress_BY_FAMILYID"
        Dim DT As DataTable

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)

            DBCommandWrapper.CommandTimeout = 180

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            DT = New DataTable("REG_MASTER")
            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0).Rows(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        Finally
            If DT IsNot Nothing Then DT.Dispose()

        End Try
    End Function

    Public Shared Function RetrieveRegaddressAndAlerts(ByVal familyID As Integer?, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Dim Tablenames() As String = {"PARTICIPANT_ADDRESS", "PARTICIPANT_ALERTS", "PATIENTS_ELIGIBILITY"}

        Dim SQLCall As String = "FDBMD.RETRIEVE_DEMOGRAPHICS_AND_ALERTS_BY_FAMILYID"

        Try


            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@EMPLOYEE_ACCESS", DbType.Int32, If(UFCWGeneralAD.CMSCanAdjudicateEmployee() Or UFCWGeneralAD.CMSEligibilityEmployee(), 1, 0))
            DB.AddInParameter(DBCommandWrapper, "@LOCAL_ACCESS", DbType.Int32, If(UFCWGeneralAD.CMSUsers() Or UFCWGeneralAD.CMSDental() Or UFCWGeneralAD.CMSLocalsEmployee() Or UFCWGeneralAD.CMSEligibility(), 1, 0))

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                ds = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DB.LoadDataSet(DBCommandWrapper, ds, Tablenames)
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveRegaddress(ByVal familyID As Integer?) As DataRow
        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim DT As DataTable
        Dim SQLCall As String = "FDBMD.RETRIEVE_DEMOGRAPHICS_BY_FAMILYID"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@EMPLOYEE_ACCESS", DbType.Int32, If(UFCWGeneralAD.CMSCanAdjudicateEmployee() Or UFCWGeneralAD.CMSEligibilityEmployee(), 1, 0))
            DB.AddInParameter(DBCommandWrapper, "@LOCAL_ACCESS", DbType.Int32, If(UFCWGeneralAD.CMSUsers() Or UFCWGeneralAD.CMSDental() Or UFCWGeneralAD.CMSLocalsEmployee() Or UFCWGeneralAD.CMSEligibility(), 1, 0))

            DBCommandWrapper.CommandTimeout = 180

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            DT = New DataTable("REG_MASTER")
            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0).Rows(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        Finally
            If DT IsNot Nothing Then DT.Dispose()

        End Try
    End Function

    Public Shared Function EligibilityCalcElements(ByVal familyID As Integer?) As DataRow
        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_ELGCALC_ELEMENTS_BY_FAMILYID"
        Dim DT As DataTable

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)

            DBCommandWrapper.CommandTimeout = 180

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            DT = New DataTable("ELGCALC_ELEMENTS")
            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0).Rows(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        Finally
            If DT IsNot Nothing Then DT.Dispose()
        End Try
    End Function

    Public Shared Function RetrieveLastEligibleDayBeforeDOS(ByVal familyID As Integer, ByVal relationID As Int16, ByVal dateOfService As Date) As Date?
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_LAST_ELIGIBLE_DAY_BEFORE_DOS"

        Try


            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILYID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATIONID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@DOS", DbType.Date, UFCWGeneral.ToNullDateHandler(dateOfService))
            DB.AddOutParameter(DBCommandWrapper, "@LASTELIGIBLEDAY", DbType.Date, 8)

            DB.ExecuteNonQuery(DBCommandWrapper)

            Return UFCWGeneral.IsNullDateHandler(DB.GetParameterValue(DBCommandWrapper, "@LASTELIGIBLEDAY"), "@LASTELIGIBLEDAY")

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveMonthsGapInfoinHours(ByVal familyID As Integer, ByVal fromDate As Date, ByVal thruDate As Date, Optional ByVal transaction As DbTransaction = Nothing) As DataTable
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim DS As DataSet
        Dim SQLCall As String = "FDBMD.RETRIEVE_MONTH_YEAR_GAP_HOURS_BY_FAMILYID"

        Try


            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(fromDate))
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(thruDate))
            DBCommandWrapper.CommandTimeout = 180

            If DS Is Nothing Then
                If transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "HOURS_GAP")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "HOURS_GAP", transaction)
                End If
            End If

            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveMonthsGapInfoinEligHours(ByVal familyID As Integer, ByVal fromDate As Date, ByVal thruDate As Date, Optional ByVal transaction As DbTransaction = Nothing) As DataTable
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim DS As DataSet = Nothing
        Dim SQLCall As String = "FDBMD.RETRIEVE_MONTH_YEAR_ELIG_GAP_HOURS_BY_FAMILYID"

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(fromDate))
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(thruDate))
            DBCommandWrapper.CommandTimeout = 180
            If DS Is Nothing Then
                If transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "HOURS_GAP")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "HOURS_GAP", transaction)
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

#End Region

#Region "Audit Functions"
    Public Shared Function RetrieveAllAuditRules() As DataTable

        SyncLock _RetrieveAllAuditRulesSyncLock

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_AUDIT_RULES" & ".xml"
            Dim DT As DataTable
            Dim DS As DataSet
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim FileStream As FileStream
            Dim DC As DataColumn
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBMD.RETRIEVE_AUDIT_RULES"
            Dim XMLSerial As XmlSerializer

            Try

                DB = CMSDALCommon.CreateDatabase()
                DT = New DataTable("AUDIT_RULE")

                If _AuditRulesDS Is Nothing Then
                    DS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.AUDIT_RULE", "LASTUPDT", "FDBMD.RETRIEVE_AUDIT_RULES")
                    If DS.Tables.Count = 0 Then
                        DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                        If DS Is Nothing Then
                            DS = DB.ExecuteDataSet(DBCommandWrapper)
                        Else
                            DB.LoadDataSet(DBCommandWrapper, DS, "AUDIT_RULE")
                        End If

                        _AuditRulesDS = DS

                        FileStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                        XMLSerial = New XmlSerializer(DS.GetType())
                        XMLSerial.Serialize(FileStream, DS)

                        File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                    Else
                        _AuditRulesDS = DS
                    End If
                Else
                    DS = _AuditRulesDS
                End If

                If DS.Tables.Count > 0 Then

                    DT = DS.Tables(0)

                    If Not DT.Columns.Contains("RULE") Then
                        DC = New DataColumn("RULE") With {
                            .Expression = "AUDIT_COLUMN_ALIAS + ' ' + AUDIT_OPERATOR + ' ' + AUDIT_OPERAND"
                        }
                        DT.Columns.Add(DC)
                    End If

                End If

                Return DT

            Catch ex As Exception
                Throw

            Finally

                If FileStream IsNot Nothing Then FileStream.Close()
                FileStream = Nothing

                If DT IsNot Nothing Then DT.Dispose()
                DT = Nothing

                If DC IsNot Nothing Then DC.Dispose()
                DC = Nothing

            End Try
        End SyncLock


    End Function
    Public Shared Function RetrieveAllAuditRulesExtn() As DataTable

        SyncLock _RetrieveAllAuditRulesSyncLock

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_AUDIT_RULES_EXTN" & ".xml"
            Dim DT As DataTable
            Dim DS As DataSet
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim FileStream As FileStream
            Dim DC As DataColumn
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBMD.RETRIEVE_AUDIT_RULES_EXTN"
            Dim XMLSerial As XmlSerializer

            Try

                DB = CMSDALCommon.CreateDatabase()
                DT = New DataTable("AUDIT_RULE")

                If _AuditRulesDS Is Nothing Then
                    DS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.AUDIT_RULE", "LASTUPDT", "FDBMD.RETRIEVE_AUDIT_RULES_EXTN")
                    If DS.Tables.Count = 0 Then
                        DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                        If DS Is Nothing Then
                            DS = DB.ExecuteDataSet(DBCommandWrapper)
                        Else
                            DB.LoadDataSet(DBCommandWrapper, DS, "AUDIT_RULE")
                        End If

                        _AuditRulesDS = DS

                        FileStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                        XMLSerial = New XmlSerializer(DS.GetType())
                        XMLSerial.Serialize(FileStream, DS)

                        File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                    Else
                        _AuditRulesDS = DS
                    End If
                Else
                    DS = _AuditRulesDS
                End If

                If DS.Tables.Count > 0 Then

                    DT = DS.Tables(0)

                    If Not DT.Columns.Contains("RULE") Then
                        DC = New DataColumn("RULE") With {
                            .Expression = "AUDIT_COLUMN_ALIAS + ' ' + AUDIT_OPERATOR + ' ' + AUDIT_OPERAND"
                        }
                        DT.Columns.Add(DC)
                    End If

                End If

                Return DT

            Catch ex As Exception
                Throw

            Finally

                If FileStream IsNot Nothing Then FileStream.Close()
                FileStream = Nothing

                If DT IsNot Nothing Then DT.Dispose()
                DT = Nothing

                If DC IsNot Nothing Then DC.Dispose()
                DC = Nothing

            End Try
        End SyncLock


    End Function
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Retrieves columns that we can audit
    ' </summary>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    '     [paulw]     11/16/2006  Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function RetrieveAuditableColumns() As DataTable
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_AUDITABLE_COLUMNS"
        Dim DS As DataSet
        Dim DT As DataTable

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DS = DB.ExecuteDataSet(DBCommandWrapper)
            DT = New DataTable("AUDITABLE_COLUMNS")

            If DS.Tables.Count > 0 Then
                DT = DS.Tables(0)
            End If

            DT.TableName = "AUDITABLE_COLUMNS"

            Return DT

        Catch ex As Exception
            Throw
        Finally

            If DT IsNot Nothing Then DT.Dispose()

        End Try
    End Function

    Public Shared Sub CreateAuditableColumn(ByVal tableName As String, ByVal columnName As String, ByVal aliasName As String, ByVal userId As String)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_AUDIT_COLUMN"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "AUDIT_COLUMN_NAME", DbType.String, columnName)
            DB.AddInParameter(DBCommandWrapper, "AUDIT_TABLE_NAME", DbType.String, tableName)
            DB.AddInParameter(DBCommandWrapper, "AUDIT_COLUMN_ALIAS", DbType.String, aliasName)
            DB.AddInParameter(DBCommandWrapper, "CREATE_USERID", DbType.String, userId)
            DB.CreateConnection.Open()
            DB.ExecuteNonQuery(DBCommandWrapper)

        Catch ex As Exception
            Throw
        Finally
            If DB.CreateConnection IsNot Nothing Then
                DB.CreateConnection.Close()
            End If
        End Try
    End Sub
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Create an audit rule
    ' </summary>
    ' <param name="aliasName"></param>
    ' <param name="operator"></param>
    ' <param name="dataType"></param>
    ' <param name="val"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    '     [paulw]     11/15/2006  Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Sub CreateAuditRule(ByVal aliasName As String, ByVal [operator] As String, ByVal dataType As String, ByVal val As String, ByVal userId As String)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_AUDIT_RULE"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "AUDIT_COLUMN_ALIAS", DbType.String, aliasName)
            DB.AddInParameter(DBCommandWrapper, "AUDIT_OPERATOR", DbType.String, [operator])
            DB.AddInParameter(DBCommandWrapper, "AUDIT_OPERAND", DbType.String, val)
            DB.AddInParameter(DBCommandWrapper, "AUDIT_OPERAND_DATA_TYPE", DbType.String, dataType)
            DB.AddInParameter(DBCommandWrapper, "CREATE_USERID", DbType.String, userId)
            DB.CreateConnection.Open()
            DB.ExecuteNonQuery(DBCommandWrapper)

        Catch ex As Exception
            Throw
        Finally
            If DB.CreateConnection IsNot Nothing Then
                DB.CreateConnection.Close()
            End If
        End Try
    End Sub
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Deletes the rule
    ' </summary>
    ' <param name="ruleId"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    '     [paulw]     11/15/2006  Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Sub DeleteAuditRule(ByVal ruleId As Integer)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.DELETE_AUDIT_RULE"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "AUDIT_RULE_ID", DbType.Int32, ruleId)
            DB.CreateConnection.Open()
            DB.ExecuteNonQuery(DBCommandWrapper)

        Catch ex As Exception
            Throw
        Finally
            If DB.CreateConnection IsNot Nothing Then
                DB.CreateConnection.Close()
            End If
        End Try
    End Sub
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Saves the Audit information
    ' </summary>
    ' <param name="meataDataXmlNodeLst"></param>
    ' <param name="xmlNodeLst"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    '     [paulw]     11/14/2006  Created
    '     [NSNYDER]   2/15/2007   REVISED
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function SaveAuditInformation(ByVal meataDataXmlNodeLst As Xml.XmlNodeList, ByVal xmlNodeLst As Xml.XmlNodeList, ByRef transaction As DbTransaction) As DataTable

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim ClaimID As Integer, Adjuster As String, Auditor As String
        Dim SQLCall As String = "FDBMD.CREATE_AUDIT_LOG"
        Dim XMLNds As XmlNodeList

        Try
            DB = CMSDALCommon.CreateDatabase()

            ClaimID = CInt(meataDataXmlNodeLst.Item(0).SelectSingleNode("CLAIM_ID").SelectNodes("@Value").Item(0).Value)
            Adjuster = CStr(meataDataXmlNodeLst.Item(0).SelectSingleNode("ADJUSTER").SelectNodes("@Value").Item(0).Value)
            Auditor = CStr(meataDataXmlNodeLst.Item(0).SelectSingleNode("AUDITOR").SelectNodes("@Value").Item(0).Value)

            For Each nd As XmlNode In xmlNodeLst
                XMLNds = nd.SelectNodes("AuditInfo")
                For Each xmlNd As Xml.XmlNode In XMLNds
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
                    DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, ClaimID)
                    DB.AddInParameter(DBCommandWrapper, "@AUDITOR", DbType.String, Auditor)
                    DB.AddInParameter(DBCommandWrapper, "@ADJUSTER", DbType.String, Adjuster)
                    DB.AddInParameter(DBCommandWrapper, "@AUDIT_TEXT", DbType.String, xmlNd.SelectSingleNode("@Value").Value)
                    DB.AddInParameter(DBCommandWrapper, "@NON_ISSUE", DbType.Decimal, 0)
                    DB.AddInParameter(DBCommandWrapper, "@AUDIT_CATEGORY_NAME", DbType.String, xmlNd.SelectSingleNode("@Name").Value)

                    DB.ExecuteNonQuery(DBCommandWrapper, transaction)
                Next
            Next

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Function

    Public Shared Function LimitJoin(ByVal xmlNodeLst As Xml.XmlNodeList) As String
        Dim SQLJoinFilter As StringBuilder
        Dim XMLNds As XmlNodeList

        Try

            SQLJoinFilter = New StringBuilder

            For Each nd As XmlNode In xmlNodeLst
                XMLNds = nd.SelectNodes("Criterion")
                For Each xmlNd As Xml.XmlNode In XMLNds
                    If xmlNd.SelectSingleNode("@Name").Value.ToUpper = "BATCHNUMBER" Then
                        SQLJoinFilter.Append(" AND UPPER(C.BATCH) = " & xmlNd.SelectSingleNode("@Value").Value.Trim & " ")
                        Exit For
                    End If
                Next
            Next

            Return SQLJoinFilter.ToString

        Catch ex As Exception

            Throw

        End Try

    End Function

    Public Shared Function BuildWhere(ByVal xmlNodeLst As Xml.XmlNodeList) As String
        Dim StrWhere As StringBuilder
        Dim SQLWhere As String
        Dim XMLNds As XmlNodeList
        Dim NoCommaText As String
        Dim NoSpaceText As String
        Dim DiagnosisAL As String()
        Dim ProcedureAL As String()
        Dim ReasonsAL As String()

        Try

            StrWhere = New StringBuilder

            For Each nd As XmlNode In xmlNodeLst
                XMLNds = nd.SelectNodes("Criterion")
                For Each xmlNd As Xml.XmlNode In XMLNds
                    If xmlNd.SelectSingleNode("@Name").Value.ToUpper = "DATEOFSERVICE" OrElse xmlNd.SelectSingleNode("@Name").Value.ToUpper = "STATUSDATE" OrElse xmlNd.SelectSingleNode("@Name").Value.ToUpper = "PROCESSEDDATE" OrElse xmlNd.SelectSingleNode("@Name").Value.ToUpper = "RECEIVEDDATE" Then
                        StrWhere.Append(xmlNd.SelectSingleNode("@Name").Value & " BETWEEN " & xmlNd.SelectSingleNode("@Value").Value & " AND ")
                    ElseIf xmlNd.SelectSingleNode("@Name").Value.ToUpper = "CHECKAMOUNT" Then
                        StrWhere.Append(xmlNd.SelectSingleNode("@Name").Value & " > " & xmlNd.SelectSingleNode("@Value").Value & " AND ")
                    ElseIf xmlNd.SelectSingleNode("@Name").Value.ToUpper = "PROCEDURECODE" Then

                        If xmlNd.SelectSingleNode("@Value").Value.Contains("-") Then
                            NoSpaceText = xmlNd.SelectSingleNode("@Value").Value.ToUpper.Replace(" ", "").Replace("'", "")
                            ProcedureAL = NoSpaceText.Split(New Char() {CChar("-")}, StringSplitOptions.RemoveEmptyEntries)

                            StrWhere.Append(xmlNd.SelectSingleNode("@Name").Value & " BETWEEN '" & ProcedureAL(0) & "' AND '" & ProcedureAL(1) & "' AND ")

                        Else
                            NoCommaText = xmlNd.SelectSingleNode("@Value").Value.ToUpper.Replace(",", " ").Replace("'", "")
                            ProcedureAL = NoCommaText.Split(New Char() {CChar(" ")}, StringSplitOptions.RemoveEmptyEntries)

                            If ProcedureAL.Length > 1 Then
                                StrWhere.Append(xmlNd.SelectSingleNode("@Name").Value & " IN ('" & String.Join("','", ProcedureAL.Select(Function(p) p.ToString())) & "') AND ")
                            Else
                                StrWhere.Append(xmlNd.SelectSingleNode("@Name").Value & " = " & xmlNd.SelectSingleNode("@Value").Value & " AND ")
                            End If

                        End If

                    ElseIf xmlNd.SelectSingleNode("@Name").Value.ToUpper = "DIAGNOSIS" Then

                        If xmlNd.SelectSingleNode("@Value").Value.Contains("-") Then
                            NoSpaceText = xmlNd.SelectSingleNode("@Value").Value.ToUpper.Replace(" ", "").Replace("'", "")
                            DiagnosisAL = NoSpaceText.Split(New Char() {CChar("-")}, StringSplitOptions.RemoveEmptyEntries)

                            StrWhere.Append(xmlNd.SelectSingleNode("@Name").Value & " BETWEEN '" & DiagnosisAL(0) & "' AND '" & DiagnosisAL(1) & "' AND ")

                        Else
                            NoCommaText = xmlNd.SelectSingleNode("@Value").Value.ToUpper.Replace(",", " ").Replace("'", "")
                            DiagnosisAL = NoCommaText.Split(New Char() {CChar(" ")}, StringSplitOptions.RemoveEmptyEntries)

                            If DiagnosisAL.Length > 1 Then
                                StrWhere.Append(xmlNd.SelectSingleNode("@Name").Value & " IN ('" & String.Join("','", DiagnosisAL.Select(Function(p) p.ToString())) & "') AND ")
                            Else
                                StrWhere.Append(xmlNd.SelectSingleNode("@Name").Value & " = " & xmlNd.SelectSingleNode("@Value").Value & " AND ")
                            End If

                        End If

                    ElseIf xmlNd.SelectSingleNode("@Name").Value.ToUpper = "REASON" Then

                        NoCommaText = xmlNd.SelectSingleNode("@Value").Value.ToUpper.Replace(",", " ").Replace("'", "")
                        ReasonsAL = NoCommaText.Split(New Char() {CChar(" ")}, StringSplitOptions.RemoveEmptyEntries)

                        If ReasonsAL.Length > 1 Then
                            StrWhere.Append(xmlNd.SelectSingleNode("@Name").Value & " IN ('" & String.Join("','", ReasonsAL.Select(Function(p) p.ToString())) & "') AND ")
                        Else
                            StrWhere.Append(xmlNd.SelectSingleNode("@Name").Value & " = " & xmlNd.SelectSingleNode("@Value").Value & " AND ")
                        End If

                    Else
                        If xmlNd.SelectSingleNode("@Name").Value.ToUpper = "CLAIMID" Then

                            UFCWLastKeyData.ClaimID = xmlNd.SelectSingleNode("@Value").Value

                            StrWhere.Append(" ( " & xmlNd.SelectSingleNode("@Name").Value & " = " & xmlNd.SelectSingleNode("@Value").Value & " OR ")
                            StrWhere.Append(" C.REFRENCE_CLAIM = " & xmlNd.SelectSingleNode("@Value").Value & ") AND ")

                        ElseIf xmlNd.SelectSingleNode("@Name").Value.ToUpper = "REFERENCEID" Then

                            UFCWLastKeyData.ClaimID = xmlNd.SelectSingleNode("@Value").Value

                            StrWhere.Append(xmlNd.SelectSingleNode("@Name").Value & " = " & xmlNd.SelectSingleNode("@Value").Value & " AND ")

                        ElseIf xmlNd.SelectSingleNode("@Name").Value.ToUpper = "PROVIDERNAME" Then
                            StrWhere.Append(xmlNd.SelectSingleNode("@Name").Value & " LIKE " & xmlNd.SelectSingleNode("@Value").Value & " AND ")
                        ElseIf xmlNd.SelectSingleNode("@Name").Value.ToUpper = "CLAIMSTAT" Then
                            StrWhere.Append(xmlNd.SelectSingleNode("@Name").Value & " LIKE " & xmlNd.SelectSingleNode("@Value").Value & " AND ")
                        Else

                            Select Case xmlNd.SelectSingleNode("@Name").Value.ToUpper
                                Case "FAMILYID"
                                    UFCWLastKeyData.FamilyID = xmlNd.SelectSingleNode("@Value").Value
                                Case "RELATIONID"
                                    UFCWLastKeyData.RelationID = xmlNd.SelectSingleNode("@Value").Value
                            End Select

                            StrWhere.Append(xmlNd.SelectSingleNode("@Name").Value & " = " & xmlNd.SelectSingleNode("@Value").Value & " AND ")

                        End If
                    End If
                Next
            Next

            'A = REG_MASTER
            'C = CLAIM_MASTER
            'H = MEDHDR
            'D = MEDDTL
            'P = PROVIDER
            'V = PAYEE_VALUES
            'O = MEDMOD
            'I = MEDDIAG
            'R = REASON

            SQLWhere = StrWhere.ToString
            SQLWhere = SQLWhere.ToUpper.Replace("PATIENTSSN", "C.PAT_SSN")
            SQLWhere = SQLWhere.ToUpper.Replace("BATCHNUMBER", "UPPER(C.BATCH)")
            SQLWhere = SQLWhere.ToUpper.Replace("PARTICIPANTSSN", "C.PART_SSN")
            SQLWhere = SQLWhere.ToUpper.Replace("FAMILYID", "C.FAMILY_ID")
            SQLWhere = SQLWhere.ToUpper.Replace("RELATIONID", "C.RELATION_ID")
            SQLWhere = SQLWhere.ToUpper.Replace("CLAIMID", "C.CLAIM_ID")
            SQLWhere = SQLWhere.ToUpper.Replace("REFERENCEID", "C.REFERENCE_ID")
            SQLWhere = SQLWhere.ToUpper.Replace("REFERENCEID", "C.REFERENCE_ID")
            SQLWhere = SQLWhere.ToUpper.Replace("MAXID", "C.MAXID")
            SQLWhere = SQLWhere.ToUpper.Replace("DOCID", "C.DOCID")
            SQLWhere = SQLWhere.ToUpper.Replace("DOCTYPE", "C.DOC_TYPE")
            SQLWhere = SQLWhere.ToUpper.Replace("ARCHIVE", "C.ARCHIVE_SW")
            SQLWhere = SQLWhere.ToUpper.Replace("STATUSDATE", "C.STATUS_DATE")
            SQLWhere = SQLWhere.ToUpper.Replace("PROCESSEDDATE", "C.PROCESSED_DATE")
            SQLWhere = SQLWhere.ToUpper.Replace("RECEIVEDDATE", "C.REC_DATE")
            SQLWhere = SQLWhere.ToUpper.Replace("PROCESSEDBY", "C.PROCESSED_BY")
            SQLWhere = SQLWhere.ToUpper.Replace("DATEOFSERVICE", "C.DATE_OF_SERVICE")
            SQLWhere = SQLWhere.ToUpper.Replace("ADJUSTER", "C.PROCESSED_BY")
            SQLWhere = SQLWhere.ToUpper.Replace("REASON", "R.REASON")

            '            sqlWhere = sqlWhere.ToUpper.Replace("DATEOFSERVICE", "D.OCC_FROM_DATE") 'placed at this level to ensure accuracy (roll up issues ?)
            SQLWhere = SQLWhere.ToUpper.Replace("CHECKNUMBER", "D.CHK_NBR")
            SQLWhere = SQLWhere.ToUpper.Replace("BILLTYPE", "D.BILL_TYPE")
            SQLWhere = SQLWhere.ToUpper.Replace("PROCEDURECODE", "D.PROC_CODE")
            SQLWhere = SQLWhere.ToUpper.Replace("PLACEOFSERVICE", "D.PLACE_OF_SERV")
            SQLWhere = SQLWhere.ToUpper.Replace("STATUS", "D.STATUS")

            SQLWhere = SQLWhere.ToUpper.Replace("CLAIMSTAT", "C.STATUS")

            SQLWhere = SQLWhere.ToUpper.Replace("MODIFIER", "O.MODIFIER")

            SQLWhere = SQLWhere.ToUpper.Replace("DIAGNOSIS", "I.DIAGNOSIS")

            SQLWhere = SQLWhere.ToUpper.Replace("OUTOFAREA", "H.OUT_OF_AREA_SW")
            SQLWhere = SQLWhere.ToUpper.Replace("CHIRO", "H.CHIRO_SW")
            SQLWhere = SQLWhere.ToUpper.Replace("NONPARTICIPATING", "H.NON_PAR_SW")
            SQLWhere = SQLWhere.ToUpper.Replace("INCIDENTDATE", "H.INCIDENT_DATE")
            SQLWhere = SQLWhere.ToUpper.Replace("CHECKAMOUNT", "H.TOT_PAID_AMT")

            SQLWhere = SQLWhere.ToUpper.Replace("PROVIDERNAME", "P.NAME1")
            SQLWhere = SQLWhere.ToUpper.Replace("PROVIDERTIN", "P.TAXID")
            SQLWhere = SQLWhere.ToUpper.Replace("PROVIDERID", "P.PROVIDER_ID")
            'sqlWhere = sqlWhere.ToUpper.Replace("NPI", "P.NPI")
            SQLWhere = SQLWhere.ToUpper.Replace("NPI", "H.RENDERING_NPI")
            'sqlWhere = sqlWhere.ToUpper.Replace("BILLINGNPI", "H.BILL_NPI")

            'these values are not dynamic because of the nature
            ' of the table design.  there is no related table
            ' and each type of accident has a switch in the table
            ' so we must set them explicitly since there is no
            ' way to set them dynamically.
            SQLWhere = SQLWhere.ToUpper.Replace("AUTOACCIDENT", "H.AUTO_ACCIDENT_SW")
            SQLWhere = SQLWhere.ToUpper.Replace("WORKERSCOMP", "H.WORKERS_COMP_SW")
            SQLWhere = SQLWhere.ToUpper.Replace("OTHERACCIDENT", "H.OTH_ACCIDENT_SW")

            SQLWhere = SQLWhere.ToUpper.Replace("TRUE", "1")
            SQLWhere = SQLWhere.ToUpper.Replace("FALSE", "0")

            SQLWhere = SQLWhere.ToUpper.Replace("PATIENTLASTNAME", "C.PAT_LNAME")
            SQLWhere = SQLWhere.ToUpper.Replace("PATIENTFIRSTNAME", "C.PAT_FNAME")

            Dim exp As New Regex("=", RegexOptions.IgnoreCase)

            If exp.Matches(SQLWhere).Count = 1 AndAlso SQLWhere.Contains("C.PAT_LNAME") Then
                SQLWhere = SQLWhere.ToUpper.Replace("C.PAT_LNAME", "A.LAST_NAME")
            ElseIf exp.Matches(SQLWhere).Count = 2 AndAlso SQLWhere.Contains("C.PAT_FNAME") AndAlso SQLWhere.Contains("C.PAT_LNAME") Then
                SQLWhere = SQLWhere.ToUpper.Replace("C.PAT_LNAME", "A.LAST_NAME")
                SQLWhere = SQLWhere.ToUpper.Replace("C.PAT_FNAME", "A.FIRST_NAME")
            End If

            Return SQLWhere

        Catch ex As Exception

            Throw

        End Try

    End Function

    Public Shared Function BuildProviderJoin(ByVal xmlNodeLst As Xml.XmlNodeList) As String
        Dim StrWhere As StringBuilder
        Dim XMLNds As XmlNodeList

        Try

            StrWhere = New StringBuilder

            For Each ND As XmlNode In xmlNodeLst
                XMLNds = ND.SelectNodes("Criterion")
                For Each xmlNd As Xml.XmlNode In XMLNds
                    If xmlNd.SelectSingleNode("@Name").Value.ToUpper = "PROVIDERNAME" Then
                        StrWhere.Append(" INNER JOIN FDBMD.PROVIDER PP ON Y.PROV_ID = PP.PROVIDER_ID AND PP.NAME1 LIKE " & xmlNd.SelectSingleNode("@Value").Value)
                        Exit For
                    End If
                Next
            Next

            Return StrWhere.ToString

        Catch ex As Exception

            Throw

        End Try

    End Function
    Public Shared Function BuildJoin(ByVal xmlNodeLst As Xml.XmlNodeList) As String
        Dim StrWhere As StringBuilder
        Dim SQLWhere As String
        Dim XMLNds As XmlNodeList

        Try

            StrWhere = New StringBuilder

            For Each nd As XmlNode In xmlNodeLst
                XMLNds = nd.SelectNodes("Criterion")
                For Each xmlNd As Xml.XmlNode In XMLNds
                    If xmlNd.SelectSingleNode("@Name").Value.ToUpper = "DATEOFSERVICE" OrElse xmlNd.SelectSingleNode("@Name").Value.ToUpper = "STATUSDATE" OrElse xmlNd.SelectSingleNode("@Name").Value.ToUpper = "PROCESSEDDATE" OrElse xmlNd.SelectSingleNode("@Name").Value.ToUpper = "RECEIVEDDATE" Then
                        StrWhere.Append(xmlNd.SelectSingleNode("@Name").Value & " BETWEEN " & xmlNd.SelectSingleNode("@Value").Value & " AND ")
                    ElseIf xmlNd.SelectSingleNode("@Name").Value.ToUpper = "CHECKAMOUNT" Then
                    ElseIf xmlNd.SelectSingleNode("@Name").Value.ToUpper = "PROCEDURECODE" Then
                    ElseIf xmlNd.SelectSingleNode("@Name").Value.ToUpper = "DIAGNOSIS" Then
                    ElseIf xmlNd.SelectSingleNode("@Name").Value.ToUpper = "OUTOFAREA" Then
                    ElseIf xmlNd.SelectSingleNode("@Name").Value.ToUpper = "CHIRO" Then
                    ElseIf xmlNd.SelectSingleNode("@Name").Value.ToUpper = "NONPARTICIPATING" Then
                    ElseIf xmlNd.SelectSingleNode("@Name").Value.ToUpper = "INCIDENTDATE" Then
                    ElseIf xmlNd.SelectSingleNode("@Name").Value.ToUpper = "AUTOACCIDENT" Then
                    ElseIf xmlNd.SelectSingleNode("@Name").Value.ToUpper = "OTHERACCIDENT" Then
                    ElseIf xmlNd.SelectSingleNode("@Name").Value.ToUpper = "WORKERSCOMP" Then
                    ElseIf xmlNd.SelectSingleNode("@Name").Value.ToUpper = "DATEOFSERVICE" Then
                    ElseIf xmlNd.SelectSingleNode("@Name").Value.ToUpper = "CHECKNUMBER" Then
                    ElseIf xmlNd.SelectSingleNode("@Name").Value.ToUpper = "BILLTYPE" Then
                    ElseIf xmlNd.SelectSingleNode("@Name").Value.ToUpper = "PLACEOFSERVICE" Then
                    ElseIf xmlNd.SelectSingleNode("@Name").Value.ToUpper = "STATUS" Then
                    ElseIf xmlNd.SelectSingleNode("@Name").Value.ToUpper = "MODIFIER" Then
                    ElseIf xmlNd.SelectSingleNode("@Name").Value.ToUpper = "PROVIDERNAME" Then

                    Else
                        StrWhere.Append(xmlNd.SelectSingleNode("@Name").Value & " = " & xmlNd.SelectSingleNode("@Value").Value & " AND ")

                    End If
                Next
            Next

            'Y = CLAIM_MASTER
            'R = REASON

            SQLWhere = StrWhere.ToString
            SQLWhere = SQLWhere.ToUpper.Replace("PATIENTSSN", "Y.PAT_SSN")
            SQLWhere = SQLWhere.ToUpper.Replace("PROVIDERTIN", "Y.PROV_TIN")
            SQLWhere = SQLWhere.ToUpper.Replace("PROVIDERID", "Y.PROV_ID")
            SQLWhere = SQLWhere.ToUpper.Replace("BATCHNUMBER", "Y.BATCH")
            SQLWhere = SQLWhere.ToUpper.Replace("PARTICIPANTSSN", "Y.PART_SSN")
            SQLWhere = SQLWhere.ToUpper.Replace("FAMILYID", "Y.FAMILY_ID")
            SQLWhere = SQLWhere.ToUpper.Replace("RELATIONID", "Y.RELATION_ID")
            SQLWhere = SQLWhere.ToUpper.Replace("CLAIMID", "Y.CLAIM_ID")
            SQLWhere = SQLWhere.ToUpper.Replace("DOCID", "Y.DOCID")
            SQLWhere = SQLWhere.ToUpper.Replace("DOCTYPE", "Y.DOC_TYPE")
            SQLWhere = SQLWhere.ToUpper.Replace("ARCHIVE", "Y.ARCHIVE_SW")
            SQLWhere = SQLWhere.ToUpper.Replace("STATUSDATE", "Y.STATUS_DATE")
            SQLWhere = SQLWhere.ToUpper.Replace("PROCESSEDDATE", "Y.PROCESSED_DATE")
            SQLWhere = SQLWhere.ToUpper.Replace("PROCESSEDBY", "Y.PROCESSED_BY")
            SQLWhere = SQLWhere.ToUpper.Replace("PATIENTLASTNAME", "Y.PAT_LNAME")
            SQLWhere = SQLWhere.ToUpper.Replace("PATIENTFIRSTNAME", "Y.PAT_FNAME")
            SQLWhere = SQLWhere.ToUpper.Replace("DATEOFSERVICE", "C.DATE_OF_SERVICE")
            SQLWhere = SQLWhere.ToUpper.Replace("REASON", "R.REASON")

            BuildJoin = SQLWhere

        Catch ex As Exception

            Throw

        End Try

    End Function

    Public Shared Function RetrieveAssociatedClaims(ByVal claimID As Integer, Optional ByVal employeeAccess As Boolean = False, Optional ByVal localAccess As Boolean = False) As DataTable

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim StrUnion As StringBuilder
        Dim DS As DataSet
        Dim SQLSelect As String
        Dim SQLFrom As String
        Dim SQLWhere As String


        Try

            DB = CMSDALCommon.CreateDatabase()

            StrUnion = New StringBuilder

            'A = REG_MASTER
            'C = CLAIM_MASTER
            'H = MEDHDR
            'D = MEDDTL
            'P = PROVIDER
            'V = PAYEE_VALUES
            'O = MEDMOD
            'I = MEDDIAG


            ' C - Claim Master
            ' H - MEDHDR
            ' D - MEDDTL
            ' R - REASON
            ' A - REGMASTER
            SQLSelect = "SELECT DISTINCT C.docID, C.CLAIM_ID, C.PART_SSN, C.PAT_SSN, A.FAMILY_ID, A.RELATION_ID, "
            SQLSelect += "CASE WHEN C.ARCHIVE_SW = 1 THEN 'ARCHIVED' ELSE C.STATUS END AS STATUS,"
            SQLSelect += "C.STATUS_DATE, C.DOC_CLASS, C.DOC_TYPE, C.PAGE_COUNT, "
            SQLSelect += "C.BATCH, C.REC_DATE, C.OPEN_DATE, COALESCE(H.occ_FROM_DATE,C.DATE_OF_SERVICE) AS occ_FROM_DATE, H.occ_TO_DATE, H.NETWRK_REC_DATE,"
            SQLSelect += "C.CREATE_DATE AS SCAN_DATE, C.PAT_FNAME, C.PAT_LNAME, C.PAT_INT, C.PART_FNAME, C.PART_LNAME, C.PART_INT, H.PAYEE, "
            SQLSelect += "CASE WHEN H.PAYEE = '1' THEN 'Pay Provider' WHEN H.PAYEE = '3' THEN 'Pay Participant\Member' ELSE Null END AS DESCRIPTION, "
            SQLSelect += "CASE WHEN A.TRUST_SW = 1 THEN 'TRUST EMPL.' WHEN H.LOCAL_SW = 1 THEN 'LOCAL EMPL.' ELSE 'No'  END AS EMPLOYEE, "
            SQLSelect += "H.LOCAL_SW, A.TRUST_SW, H.AUTHORIZED_SW, H.CHIRO_SW, COALESCE(C.PROV_TIN,H.PROV_TIN) AS PROV_TIN, COALESCE(C.PROV_ID,H.PROV_ID) AS PROV_ID, H.TOT_HRA_AMT, P.NAME1, "
            SQLSelect += "CASE WHEN C.ARCHIVE_SW = 1 THEN 0 ELSE H.TOT_PAID_AMT END AS TOT_PAID_AMT, H.TOT_CHRG_AMT, H.TOT_PRICED_AMT, H.TOT_oth_INS_AMT, H.NON_PAR_SW, H.OUT_OF_AREA_SW, H.RENDERING_NPI,"
            SQLSelect += "H.INCIDENT_DATE, C.PENDED_TO, COALESCE(C.PROCESSED_DATE,C.STATUS_DATE) AS PROCESSED_DATE, "
            SQLSelect += "COALESCE(CASE WHEN LENGTH(RTRIM(C.PROCESSED_BY)) = 0 THEN NULL ELSE RTRIM(C.PROCESSED_BY) END, C.USERID) AS PROCESSED_BY, "
            SQLSelect += "C.ARCHIVE_SW, C.REFRENCE_CLAIM, C.REFERENCE_ID, C.MAXID, C.USERID, "

            SQLSelect += "( SELECT REASON"
            SQLSelect += "  FROM FDBMD.REASON AS X"
            SQLSelect += "  WHERE LINE_NBR = (SELECT MIN(Z.LINE_NBR)"
            SQLSelect += "  FROM"
            SQLSelect += "  FDBMD.REASON AS Z INNER JOIN FDBMD.MEDDTL AS MD ON Z.CLAIM_ID = MD.CLAIM_ID AND MD.STATUS <> 'MERGED' AND Z.LINE_NBR = MD.LINE_NBR"
            SQLSelect += "  WHERE Z.CLAIM_ID = C.CLAIM_ID)"
            SQLSelect += "  AND PRIORITY = (SELECT MIN(ZZ.PRIORITY)"
            SQLSelect += "                  FROM"
            SQLSelect += "                  FDBMD.REASON AS ZZ INNER JOIN FDBMD.MEDDTL AS MD ON ZZ.CLAIM_ID = MD.CLAIM_ID AND MD.STATUS <> 'MERGED' AND ZZ.LINE_NBR = MD.LINE_NBR"
            SQLSelect += "                  WHERE ZZ.CLAIM_ID = C.CLAIM_ID"
            SQLSelect += "                  AND ZZ.LINE_NBR = ( SELECT MIN(WW.LINE_NBR)"
            SQLSelect += "                                      FROM"
            SQLSelect += "                                      FDBMD.REASON AS WW INNER JOIN FDBMD.MEDDTL AS MD ON WW.CLAIM_ID = MD.CLAIM_ID AND MD.STATUS <> 'MERGED' AND WW.LINE_NBR = MD.LINE_NBR"
            SQLSelect += "                                      WHERE WW.CLAIM_ID = C.CLAIM_ID))"
            SQLSelect += "  AND X.CLAIM_ID = C.CLAIM_ID) AS REASON,"
            SQLSelect += "( SELECT CASE WHEN (( SELECT COUNT(*)"
            SQLSelect += "                      FROM"
            SQLSelect += "                      FDBMD.REASON AS X1 INNER JOIN FDBMD.MEDDTL AS MD ON X1.CLAIM_ID = MD.CLAIM_ID AND MD.STATUS <> 'MERGED' AND X1.LINE_NBR = MD.LINE_NBR"
            SQLSelect += "                      WHERE X1.CLAIM_ID = C.CLAIM_ID) = ( SELECT COUNT(*)"
            SQLSelect += "                                                          FROM"
            SQLSelect += "                                                          FDBMD.REASON AS X2 INNER JOIN FDBMD.MEDDTL AS MD ON X2.CLAIM_ID = MD.CLAIM_ID AND MD.STATUS <> 'MERGED' AND X2.LINE_NBR = MD.LINE_NBR INNER JOIN FDBMD.REASON_VALUES AS V ON X2.REASON = V.REASON AND V.PENDING_USE_SW = 1 AND MD.OCC_FROM_DATE BETWEEN V.FROM_DATE AND V.THRU_DATE"
            SQLSelect += "                                                          WHERE X2.CLAIM_ID = C.CLAIM_ID))"
            SQLSelect += "                                                          AND (   SELECT COUNT(*)"
            SQLSelect += "                                                                  FROM"
            SQLSelect += "                                                                  FDBMD.MEDDTL AS Y1 LEFT OUTER JOIN FDBMD.REASON AS X3 ON X3.CLAIM_ID = Y1.CLAIM_ID AND X3.LINE_NBR = Y1.LINE_NBR"
            SQLSelect += "                                                                  WHERE Y1.CLAIM_ID = C.CLAIM_ID AND X3.REASON IS NULL AND Y1.STATUS <> 'MERGED') = 0"
            SQLSelect += "                                                          AND (   SELECT COUNT(*)"
            SQLSelect += "                                                                  FROM"
            SQLSelect += "                                                                  FDBMD.REASON AS X4 INNER JOIN FDBMD.MEDDTL AS MD ON X4.CLAIM_ID = MD.CLAIM_ID AND MD.STATUS <> 'MERGED' AND X4.LINE_NBR = MD.LINE_NBR"
            SQLSelect += "                                                                  WHERE X4.CLAIM_ID = C.CLAIM_ID) > 0"
            SQLSelect += "                                                          AND (   SELECT COUNT(*)"
            SQLSelect += "                                                                  FROM FDBMD.MEDDTL AS Z1"
            SQLSelect += "                                                                  WHERE Z1.CLAIM_ID = C.CLAIM_ID AND CHECK_SW = 1 AND Z1.STATUS <> 'MERGED') = 0 THEN 'Yes' ELSE 'No' END AS REOPENABLE"
            SQLSelect += "  FROM SYSIBM.SYSDUMMY1) AS REOPENABLE"

            SQLFrom += "FROM "
            SQLFrom += "FDBEL.REG_MASTER A INNER JOIN "
            SQLFrom += "FDBMD.CLAIM_MASTER C ON C.FAMILY_ID = A.FAMILY_ID AND C.RELATION_ID = A.RELATION_ID AND C.STATUS <> 'HIDE' LEFT OUTER JOIN FDBMD.MEDHDR AS H ON C.CLAIM_ID = H.CLAIM_ID LEFT OUTER JOIN FDBMD.PROVIDER AS P ON C.PROV_TIN = P.TAXID "

            SQLWhere += "WHERE C.CLAIM_ID IN (  SELECT M.CLAIM_ID"
            SQLWhere += "                       FROM FDBMD.MEDDTL AS M INNER JOIN "
            SQLWhere += "                           (   SELECT MIN(occ_FROM_DATE) AS occ_MIN, MAX(occ_TO_DATE) AS occ_MAX "
            SQLWhere += "                               FROM FDBMD.MEDDTL "
            SQLWhere += "                               WHERE CLAIM_ID = " & CStr(claimID) & ") AS O ON (   O.occ_MIN >= M.occ_FROM_DATE AND O.occ_MIN <= M.occ_TO_DATE OR "
            SQLWhere += "                                                                                   O.occ_MAX >= M.occ_FROM_DATE AND O.occ_MAX <= M.occ_TO_DATE) "
            SQLWhere += "                       WHERE M.CLAIM_ID <> " & CStr(claimID) & ") "

            '' Build Union portion of query
            '' This  UnionsqlCall is for Checking restricted access, the goal is to bring back a row marked as 'Restricted' if the search would have brought backrows in the absence of testing the trust/local switches
            '' To do this the returned rows family information is compared against reg master
            Dim SQLSelectUnion As String
            Dim SQLFromUnion As String

            SQLSelectUnion = "SELECT DISTINCT B.docID, B.CLAIM_ID, B.PART_SSN, B.PAT_SSN, B.FAMILY_ID, B.RELATION_ID, "
            SQLSelectUnion += "'RESTRICTED' AS STATUS,"
            SQLSelectUnion += "B.STATUS_DATE, B.DOC_CLASS, B.DOC_TYPE, B.PAGE_COUNT, "
            SQLSelectUnion += "B.BATCH, B.REC_DATE, B.OPEN_DATE, B.occ_FROM_DATE, B.occ_TO_DATE, B.NETWRK_REC_DATE,"
            SQLSelectUnion += "B.SCAN_DATE, B.PAT_FNAME, B.PAT_LNAME, B.PAT_INT, B.PART_FNAME, B.PART_LNAME, B.PART_INT, B.PAYEE, B.DESCRIPTION, "
            SQLSelectUnion += "B.EMPLOYEE, "
            SQLSelectUnion += "B.LOCAL_SW, B.TRUST_SW, B.AUTHORIZED_SW, B.CHIRO_SW, B.PROV_TIN, B.PROV_ID, B.TOT_HRA_AMT, B.NAME1, "
            SQLSelectUnion += "B.TOT_PAID_AMT, B.TOT_CHRG_AMT, B.TOT_PRICED_AMT, B.TOT_oth_INS_AMT, B.NON_PAR_SW, B.OUT_OF_AREA_SW, B.RENDERING_NPI, "
            SQLSelectUnion += "B.INCIDENT_DATE, B.PENDED_TO, COALESCE(B.PROCESSED_DATE,B.STATUS_DATE) AS PROCESSED_DATE , "
            SQLSelectUnion += "COALESCE(CASE WHEN LENGTH(RTRIM(B.PROCESSED_BY)) = 0 THEN NULL ELSE RTRIM(B.PROCESSED_BY) END, B.USERID) AS PROCESSED_BY, "
            SQLSelectUnion += "B.ARCHIVE_SW, B.REFRENCE_CLAIM, B.REFERENCE_ID,  B.MAXID, B.USERID, B.REASON, B.REOPENABLE "

            SQLFromUnion += " FROM FDBEL.REG_MASTER A INNER JOIN "
            SQLFromUnion += "      FDBMD.CLAIM_MASTER C ON C.FAMILY_ID = A.FAMILY_ID AND C.RELATION_ID = A.RELATION_ID "

            SQLFromUnion += " LEFT OUTER JOIN "
            SQLFromUnion += " ( SELECT 0 AS docID, 0 AS CLAIM_ID, 0 AS PART_SSN,0 AS PAT_SSN, 0 AS FAMILY_ID,0 AS RELATION_ID, 'RESTRICTED' as STATUS,CAST(NULL AS TIMESTAMP) AS STATUS_DATE, " &
                            "'' AS DOC_CLASS, '' AS DOC_TYPE , 0 AS PAGE_COUNT, '' AS BATCH, CAST(NULL AS DATE) AS REC_DATE,  CAST(NULL AS TIMESTAMP) AS OPEN_DATE, " &
                            "CAST(NULL AS DATE) AS occ_FROM_DATE,CAST(NULL AS DATE) AS occ_TO_DATE,CAST(NULL AS DATE) AS NETWRK_REC_DATE, CAST(NULL AS TIMESTAMP)  AS SCAN_DATE,'' AS PAT_FNAME, '' AS PAT_LNAME, '' AS PAT_INT , '' AS PART_FNAME, " &
                            "'' AS PART_LNAME, '' AS PART_INT,  '' AS PAYEE, '' AS DESCRIPTION,'' AS EMPLOYEE, 0 AS TRUST_SW, 0 AS LOCAL_SW, 0 AS AUTHORIZED_SW, 0 AS CHIRO_SW , 0 AS PROV_TIN, " &
                            "0 AS PROV_ID, 0 AS TOT_HRA_AMT, '' AS NAME1,0 AS TOT_PAID_AMT, 0 AS TOT_CHRG_AMT, 0 AS TOT_PRICED_AMT, 0 AS TOT_oth_INS_AMT, 0 AS NON_PAR_SW, 0 AS OUT_OF_AREA_SW, 0 AS RENDERING_NPI, CAST(NULL AS DATE) AS INCIDENT_DATE, '' AS PENDED_TO, " &
                            "CAST(NULL AS TIMESTAMP) AS  PROCESSED_DATE ,'' AS PROCESSED_BY, 0 AS ARCHIVE_SW, CAST(NULL AS INTEGER)  AS REFRENCE_CLAIM, CAST(NULL AS VARCHAR(30)) as REFERENCE_ID, '' AS MAXID, '' AS USERID, '' AS REASON, '' AS REOPENABLE " &
                            " FROM SYSIBM.SYSDUMMY1 ) B ON A.FAMILY_ID = B.FAMILY_ID "


            Dim UnionSQL As String = SQLSelectUnion & SQLFromUnion & " WHERE " & SQLWhere

            Dim RegularSQL As String = SQLSelect & SQLFrom & " WHERE " & SQLWhere

            _LastSQL = RegularSQL & " UNION ALL " & UnionSQL & " FOR READ ONLY"
            _LastSQL = _LastSQL.Replace(vbTab, " ")
            _LastSQL = _LastSQL.Replace(vbCrLf, " ")

            Do While (_LastSQL.Contains("  "))
                ' if true, the string still contains double spaces,
                ' replace with single space
                _LastSQL = _LastSQL.Replace("  ", " ")
            Loop

            If System.Configuration.ConfigurationManager.AppSettings("LogSQLFileName") IsNot Nothing AndAlso System.Configuration.ConfigurationManager.AppSettings("LogSQLFileName").ToString.Trim.Length > 0 Then
                CMSDALLog.Log("Query Timestamp: " & UFCWGeneral.NowDate & " --Query: " & _LastSQL, CMSDALLog.LogDirectory & String.Format("{0000}", UFCWGeneral.NowDate.Year) & String.Format("{00}", UFCWGeneral.NowDate.Month) & System.Configuration.ConfigurationManager.AppSettings("LogSQLFileName"))
            End If

            If If(System.Configuration.ConfigurationManager.AppSettings("UseRUNIMMEDIATEROWSET") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("UseRUNIMMEDIATEROWSET"))) = 1 Then

                DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RUNIMMEDIATEROWSETSELECT")
                DB.AddInParameter(DBCommandWrapper, "SQLInput", DbType.String, _LastSQL)

            ElseIf If(System.Configuration.ConfigurationManager.AppSettings("UseRUNIMMEDIATELOG") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("UseRUNIMMEDIATELOG"))) = 1 Then

                DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RUNIMMEDIATESELECTLOG")
                DB.AddInParameter(DBCommandWrapper, "SQLInput", DbType.String, _LastSQL)

            ElseIf If(System.Configuration.ConfigurationManager.AppSettings("UseRUNIMMEDIATE") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("UseRUNIMMEDIATE"))) = 1 Then

                DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RUNIMMEDIATESELECT")
                DB.AddInParameter(DBCommandWrapper, "SQLInput", DbType.String, _LastSQL)

            Else
                DBCommandWrapper = DB.GetSqlStringCommand(_LastSQL)
            End If

            DBCommandWrapper.CommandTimeout = CMSDALCommon.GetTimeOut

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            If DS.Tables.Count > 0 Then
                DS.Tables(0).TableName = "AssociatedClaimsResults"

                Return DS.Tables(0)
            End If

            Return Nothing

        Catch ex As DB2Exception

            Throw

        Catch ex As Exception

            Throw

        Finally

            DBCommandWrapper.Dispose()

        End Try


    End Function

    Public Shared Function RetrieveSelection(ByVal xmlNodeLst As Xml.XmlNodeList, Optional ByVal employeeAccess As Boolean = False, Optional ByVal localAccess As Boolean = False) As DataTable

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim IncludeHdr As Boolean = False
        Dim IncludeDtl As Boolean = False
        Dim IncludePCs As Boolean = False
        Dim IncludeDiags As Boolean = False
        Dim IncludeMod As Boolean = False
        Dim IncludeDiag As Boolean = False
        Dim IncludeReopenableReason As Boolean = False
        Dim IncludeReason As Boolean = False
        Dim StrWhere As String
        Dim SQLWhere As String
        Dim StrJoin As String
        Dim SQLJoinWhere As String = ""
        Dim SQLWhereUnion As String
        Dim SQLMEDHDRInnerJoin As Boolean
        Dim DS As DataSet
        Dim SQLSelect As String
        Dim SQLFrom As String
        Dim SQLFlattenReasonSelect As String
        Dim SQLFlattenProceduresSelect As String
        Dim SQLFlattenDiagnosisSelect As String
        Dim SQLSelectUnion As String
        Dim SQLFromUnion As String
        Dim UnionSQL As String
        Dim RegularSQL As String


        Try

            DB = CMSDALCommon.CreateDatabase()

            StrWhere = BuildWhere(xmlNodeLst)
            SQLWhere = StrWhere.ToString.Remove(StrWhere.LastIndexOf("AND"), 3)

            If SQLWhere.ToString.Length = 0 Then Exit Function

            StrJoin = BuildJoin(xmlNodeLst)

            If StrJoin.ToString.Length > 0 Then
                SQLJoinWhere = StrJoin.ToString.Remove(StrJoin.LastIndexOf("AND"), 3)
            End If


            If SQLWhere.ToString.Length = 0 Then Exit Function

            'A = REG_MASTER
            Dim WhereIncludesREG_MASTER As Boolean = SQLWhere.Contains("A.")
            'C = CLAIM_MASTER
            Dim WhereIncludesCLAIM_MASTER As Boolean = SQLWhere.Contains("C.")
            'H = MEDHDR
            Dim WhereIncludesMEDHDR As Boolean = SQLWhere.Contains("H.")
            'D = MEDDTL
            Dim WhereIncludesMEDDTL As Boolean = SQLWhere.Contains("D.")
            'P = PROVIDER
            Dim WhereIncludesPROVIDER As Boolean = SQLWhere.Contains("P.")
            'V = PAYEE_VALUES
            Dim WhereIncludesPAYEE_VALUES As Boolean = SQLWhere.Contains("V.")
            'O = MEDMOD
            Dim WhereIncludesMEDMOD As Boolean = SQLWhere.Contains("O.")
            'I = MEDDIAG
            Dim WhereIncludesMEDDIAG As Boolean = SQLWhere.Contains("I.")
            'R = REASON
            Dim WhereIncludesREASON As Boolean = SQLWhere.Contains("R.")

            'Copy Where block before Security criteria is added
            SQLWhereUnion = SQLWhere

            'note: this is done before security "WHERE" additions to exclude security switches from join determination
            SQLMEDHDRInnerJoin = SQLWhere.Contains("H.")

            If DB.ConnectionString.ToLower.IndexOf("ddtek") = -1 Then
                SQLWhere = SQLWhere.Replace("-00.00.00", " 00:00:00")
                SQLWhere = SQLWhere.Replace("-23.59.59", " 23:59:59")

                If employeeAccess = False Then
                    SQLWhere += " AND (A.TRUST_SW IS NULL or A.TRUST_SW = 0)"
                End If

                If localAccess = False And employeeAccess = False Then
                    SQLWhere += " AND (H.LOCAL_SW IS NULL OR H.LOCAL_SW = 0)"
                End If

            Else

                If employeeAccess = False Then
                    SQLWhere += " AND (A.TRUST_SW IS NULL or A.TRUST_SW = 0)"
                End If

                If localAccess = False And employeeAccess = False Then
                    SQLWhere += " AND ((H.LOCAL_SW IS NULL OR H.LOCAL_SW = 0) AND (C.LOCAL_SW IS NULL OR C.LOCAL_SW = 0))"
                End If

            End If

            If SQLWhere.IndexOf("H.") >= 0 Then IncludeHdr = True
            If SQLWhere.IndexOf("D.") >= 0 Then IncludeDtl = True
            If SQLWhere.IndexOf("D.PROC_CODE") >= 0 Then IncludePCs = True

            If SQLJoinWhere.Trim.Length > 0 AndAlso SQLJoinWhere.IndexOf("Y.") >= 0 Then IncludeReopenableReason = True

            If SQLJoinWhere.Trim.Length > 0 AndAlso SQLJoinWhere.IndexOf("R.") >= 0 Then IncludeReason = True

            If SQLWhere.IndexOf("O.") >= 0 Then
                IncludeMod = True
                IncludeDtl = True
            End If

            If SQLWhere.IndexOf("I.") >= 0 Then
                IncludeDiag = True
                IncludeDtl = True
            End If


            ' C - Claim Master
            ' H - MEDHDR
            ' D - MEDDTL
            ' R - REASON
            ' A - REGMASTER
            SQLSelect = "SELECT DISTINCT C.DOCID, C.CLAIM_ID, C.PART_SSN, C.PAT_SSN, C.FAMILY_ID, C.RELATION_ID, "
            SQLSelect += "CASE WHEN C.ARCHIVE_SW = 1 THEN 'ARCHIVED' ELSE C.STATUS END AS STATUS,"
            SQLSelect += "C.STATUS_DATE, C.DOC_CLASS, C.DOC_TYPE, C.PAGE_COUNT, "
            SQLSelect += "C.BATCH, C.REC_DATE, C.OPEN_DATE, COALESCE(H.OCC_FROM_DATE,C.DATE_OF_SERVICE) AS OCC_FROM_DATE, H.OCC_TO_DATE, H.NETWRK_REC_DATE,"
            SQLSelect += "C.CREATE_DATE AS SCAN_DATE, C.PAT_FNAME, C.PAT_LNAME, C.PAT_INT, C.PART_FNAME, C.PART_LNAME, C.PART_INT, H.PAYEE, "
            SQLSelect += "CASE WHEN H.PAYEE = '1' THEN 'Pay Provider' WHEN H.PAYEE = '3' THEN 'Pay Participant\Member' ELSE Null END AS DESCRIPTION, "
            SQLSelect += "CASE WHEN A.TRUST_SW = 1 THEN 'TRUST EMPL.' WHEN H.LOCAL_SW = 1 THEN 'LOCAL EMPL.' ELSE 'No'  END AS EMPLOYEE, "
            SQLSelect += "H.LOCAL_SW, A.TRUST_SW, H.AUTHORIZED_SW, H.CHIRO_SW, COALESCE(C.PROV_TIN,H.PROV_TIN) AS PROV_TIN, COALESCE(C.PROV_ID,H.PROV_ID) AS PROV_ID, H.TOT_HRA_AMT, P.NAME1, "
            SQLSelect += "CASE WHEN C.ARCHIVE_SW = 1 THEN 0 ELSE H.TOT_PAID_AMT END AS TOT_PAID_AMT, H.TOT_CHRG_AMT, H.TOT_PRICED_AMT, H.TOT_OTH_INS_AMT, H.NON_PAR_SW, H.OUT_OF_AREA_SW, H.RENDERING_NPI, H.PRICED_BY, H.PPO, H.SURPRISE_BILLING_INDICATOR, "
            SQLSelect += "H.INCIDENT_DATE, C.PENDED_TO, COALESCE(C.PROCESSED_DATE,C.STATUS_DATE) AS PROCESSED_DATE, "
            SQLSelect += "COALESCE(CASE WHEN LENGTH(RTRIM(C.PROCESSED_BY)) = 0 THEN NULL ELSE RTRIM(C.PROCESSED_BY) END, C.USERID) AS PROCESSED_BY, "
            SQLSelect += "C.ARCHIVE_SW, C.REFRENCE_CLAIM, C.REFERENCE_ID, C.MAXID, C.USERID " & If(IncludePCs, " ,PC.PROCEDURES ", "") & If(IncludeReopenableReason, " , R.REASON, RO.REOPENABLE ", "")

            SQLFrom += "FROM "

            'Search can return without the need for a REG hit
            If WhereIncludesCLAIM_MASTER And Not (WhereIncludesREG_MASTER OrElse WhereIncludesMEDHDR OrElse WhereIncludesMEDDTL OrElse WhereIncludesPROVIDER OrElse WhereIncludesPAYEE_VALUES OrElse WhereIncludesMEDMOD OrElse WhereIncludesMEDDIAG OrElse WhereIncludesREASON) Then
                SQLFrom += "FDBMD.CLAIM_MASTER C LEFT JOIN "
                SQLFrom += "FDBEL.REG_MASTER A ON C.FAMILY_ID = A.FAMILY_ID AND ((C.RELATION_ID = A.RELATION_ID) OR (C.RELATION_ID = -1 AND A.RELATION_ID = 0)) AND C.STATUS <> 'HIDE'" & LimitJoin(xmlNodeLst)
            Else
                SQLFrom += "FDBEL.REG_MASTER A INNER JOIN "
                SQLFrom += "FDBMD.CLAIM_MASTER C ON C.FAMILY_ID = A.FAMILY_ID AND C.RELATION_ID = A.RELATION_ID AND C.STATUS <> 'HIDE'" & LimitJoin(xmlNodeLst)
            End If

            If IncludeReason Then
                SQLFrom += " INNER JOIN FDBMD.REASON R ON C.CLAIM_ID = R.CLAIM_ID "
            End If

            SQLFrom += If(SQLMEDHDRInnerJoin OrElse IncludeDtl, " INNER JOIN ", " LEFT JOIN ") & "FDBMD.MEDHDR H ON C.CLAIM_ID = H.CLAIM_ID "

            SQLFlattenProceduresSelect += " ( "
            SQLFlattenProceduresSelect += " SELECT 	STRIP(CAST(XMLSERIALIZE(XMLAGG(XMLTEXT(STRIP(LINE_NBR || ':' || PC.PROC_CODE,B,' ') || ' ') ORDER BY PC.LINE_NBR) AS CLOB (1K) ) AS VARCHAR(500)),B,' ') AS PROC_CODES "
            SQLFlattenProceduresSelect += " FROM    ("
            SQLFlattenProceduresSelect += "           SELECT MD.LINE_NBR, MD.PROC_CODE"
            SQLFlattenProceduresSelect += "           FROM FDBMD.MEDDTL MD"
            SQLFlattenProceduresSelect += "           WHERE MD.CLAIM_ID = C.CLAIM_ID"
            SQLFlattenProceduresSelect += "         ) AS PC"
            SQLFlattenProceduresSelect += ") AS PROCEDURES"

            SQLFlattenReasonSelect += " ( "
            SQLFlattenReasonSelect += " SELECT "
            SQLFlattenReasonSelect += " CASE    WHEN X.REASON = 'LTR' "
            SQLFlattenReasonSelect += "         THEN COALESCE(LM.LETTER_NAMES,'') "
            SQLFlattenReasonSelect += "         ELSE X.REASON "
            SQLFlattenReasonSelect += " END AS REASON "
            SQLFlattenReasonSelect += " FROM FDBMD.REASON X CROSS JOIN"
            SQLFlattenReasonSelect += "      ("

            SQLFlattenReasonSelect += "         SELECT 'LTR: ' || STRIP(CAST(XMLSERIALIZE(XMLAGG(XMLTEXT(STRIP(ILM.LETTER_NAME,B,' ') || ' ') ORDER BY ILM.LETTER_NAME) AS CLOB (1K) ) AS VARCHAR(50)),B,' ') AS LETTER_NAMES "
            SQLFlattenReasonSelect += "         FROM    ("
            SQLFlattenReasonSelect += "                     SELECT DISTINCT LETTER_NAME"
            SQLFlattenReasonSelect += "                     FROM FDBMD.LETTERS_MEDICAL"
            SQLFlattenReasonSelect += "                     WHERE CLAIM_ID = C.CLAIM_ID"
            SQLFlattenReasonSelect += "                 ) AS ILM"

            SQLFlattenReasonSelect += "      ) AS LM "

            SQLFlattenReasonSelect += " WHERE   X.LINE_NBR = (  SELECT MIN(Z.LINE_NBR) "
            SQLFlattenReasonSelect += "                         FROM FDBMD.REASON Z INNER JOIN FDBMD.MEDDTL MD ON Z.CLAIM_ID = MD.CLAIM_ID AND MD.STATUS <> 'MERGED'  AND Z.LINE_NBR = MD.LINE_NBR"
            SQLFlattenReasonSelect += "                         WHERE Z.CLAIM_ID = C.CLAIM_ID) "
            SQLFlattenReasonSelect += " AND     X.PRIORITY = (  SELECT  MIN(ZZ.PRIORITY) "
            SQLFlattenReasonSelect += "                         FROM    FDBMD.REASON ZZ INNER JOIN FDBMD.MEDDTL MD ON ZZ.CLAIM_ID = MD.CLAIM_ID AND MD.STATUS <> 'MERGED'  AND ZZ.LINE_NBR = MD.LINE_NBR"
            SQLFlattenReasonSelect += "                         WHERE   ZZ.CLAIM_ID = C.CLAIM_ID "
            SQLFlattenReasonSelect += "                         AND     ZZ.LINE_NBR = ( SELECT MIN (WW.LINE_NBR) "
            SQLFlattenReasonSelect += "                                                 FROM FDBMD.REASON WW INNER JOIN FDBMD.MEDDTL MD ON WW.CLAIM_ID = MD.CLAIM_ID AND MD.STATUS <> 'MERGED'  AND WW.LINE_NBR = MD.LINE_NBR"
            SQLFlattenReasonSelect += "                                                 WHERE WW.CLAIM_ID = C.CLAIM_ID)) "
            SQLFlattenReasonSelect += " AND X.CLAIM_ID = C.CLAIM_ID) AS REASON, "

            SQLFlattenReasonSelect += " ( "
            SQLFlattenReasonSelect += " SELECT "
            SQLFlattenReasonSelect += " CASE WHEN    ((  SELECT  COUNT ( * ) "
            SQLFlattenReasonSelect += "                  FROM    FDBMD.REASON X1 INNER JOIN FDBMD.MEDDTL MD ON X1.CLAIM_ID = MD.CLAIM_ID AND MD.STATUS <> 'MERGED'  AND X1.LINE_NBR = MD.LINE_NBR"
            SQLFlattenReasonSelect += "                  WHERE   X1.CLAIM_ID = C.CLAIM_ID) = (  SELECT  COUNT (*)"
            SQLFlattenReasonSelect += "                                                         FROM    FDBMD.REASON X2 INNER JOIN FDBMD.MEDDTL MD ON X2.CLAIM_ID = MD.CLAIM_ID AND MD.STATUS <> 'MERGED'  AND X2.LINE_NBR = MD.LINE_NBR INNER JOIN "
            SQLFlattenReasonSelect += "                                                                 FDBMD.REASON_VALUES V ON X2.REASON = V.REASON AND V.PENDING_USE_SW = 1 AND MD.OCC_FROM_DATE BETWEEN V.FROM_DATE AND V.THRU_DATE "
            SQLFlattenReasonSelect += "                                                         WHERE   X2.CLAIM_ID = C.CLAIM_ID)) "
            SQLFlattenReasonSelect += "         AND  (  SELECT  COUNT(*) "
            SQLFlattenReasonSelect += "                 FROM    FDBMD.MEDDTL Y1 LEFT JOIN"
            SQLFlattenReasonSelect += "                         FDBMD.REASON X3  ON X3.CLAIM_ID = Y1.CLAIM_ID AND X3.LINE_NBR = Y1.LINE_NBR"
            SQLFlattenReasonSelect += "                 WHERE   Y1.CLAIM_ID = C.CLAIM_ID AND X3.REASON IS NULL AND Y1.STATUS <> 'MERGED') = 0 AND (     SELECT  COUNT(*) "
            SQLFlattenReasonSelect += "                                                                                                                 FROM    FDBMD.REASON X4 INNER JOIN FDBMD.MEDDTL MD ON X4.CLAIM_ID = MD.CLAIM_ID AND MD.STATUS <> 'MERGED'  AND X4.LINE_NBR = MD.LINE_NBR"
            SQLFlattenReasonSelect += "                                                                                                                 WHERE   X4.CLAIM_ID = C.CLAIM_ID ) > 0 "
            SQLFlattenReasonSelect += "         AND  (  SELECT  COUNT(*) "
            SQLFlattenReasonSelect += "                 FROM    FDBMD.MEDDTL Z1 "
            SQLFlattenReasonSelect += "                 WHERE   Z1.CLAIM_ID = C.CLAIM_ID AND Z1.CHECK_SW = 1 AND Z1.STATUS <> 'MERGED') = 0 "
            SQLFlattenReasonSelect += "     THEN 'Yes' "
            SQLFlattenReasonSelect += "     ELSE 'No' "
            SQLFlattenReasonSelect += " END AS REOPENABLE "

            SQLFlattenReasonSelect += " FROM SYSIBM.SYSDUMMY1) AS REOPENABLE "

            If SQLWhere.IndexOf("H.OCC_FROM_DATE") > 0 OrElse IncludeDtl Then
                SQLFrom += " INNER JOIN FDBMD.MEDDTL D ON C.CLAIM_ID = D.CLAIM_ID "
            ElseIf IncludeDtl Then
                SQLFrom += " LEFT OUTER JOIN FDBMD.MEDDTL D ON C.CLAIM_ID = D.CLAIM_ID "
            End If

            If SQLWhere.IndexOf("NAME1") >= 0 OrElse SQLWhere.IndexOf("NPI") >= 0 OrElse SQLWhere.IndexOf("TAXID") >= 0 OrElse SQLWhere.IndexOf("PROV_ID") >= 0 Then ' Provider specific spearch, so assume provider must exist
                SQLFrom += " INNER JOIN FDBMD.PROVIDER P ON C.PROV_ID = P.PROVIDER_ID "
            Else
                SQLFrom += " LEFT OUTER JOIN FDBMD.PROVIDER P ON C.PROV_ID = P.PROVIDER_ID "
            End If

            If IncludeMod Then
                SQLFrom += " LEFT OUTER JOIN FDBMD.MEDMOD O ON C.CLAIM_ID = O.CLAIM_ID "
            End If

            If IncludeDiag Then
                SQLFrom += " INNER JOIN FDBMD.MEDDIAG I ON C.CLAIM_ID = I.CLAIM_ID "
            End If

            '' Build Union portion of query
            '' This  UnionSQLCall is for Checking restricted access, the goal is to bring back a row marked as 'Restricted' if the search would have brought backrows in the absence of testing the trust/local switches
            '' To do this the returned rows family information is compared against reg master

            SQLSelectUnion = "SELECT DISTINCT B.DOCID, B.CLAIM_ID, B.PART_SSN, B.PAT_SSN, B.FAMILY_ID, B.RELATION_ID, "
            SQLSelectUnion += "'RESTRICTED' AS STATUS,"
            SQLSelectUnion += "B.STATUS_DATE, B.DOC_CLASS, B.DOC_TYPE, B.PAGE_COUNT, "
            SQLSelectUnion += "B.BATCH, B.REC_DATE, B.OPEN_DATE, B.OCC_FROM_DATE, B.OCC_TO_DATE, B.NETWRK_REC_DATE,"
            SQLSelectUnion += "B.SCAN_DATE, B.PAT_FNAME, B.PAT_LNAME, B.PAT_INT, B.PART_FNAME, B.PART_LNAME, B.PART_INT, B.PAYEE, B.DESCRIPTION, "
            SQLSelectUnion += "B.EMPLOYEE, "
            SQLSelectUnion += "B.LOCAL_SW, B.TRUST_SW, B.AUTHORIZED_SW, B.CHIRO_SW, B.PROV_TIN, B.PROV_ID, B.TOT_HRA_AMT, B.NAME1, "
            SQLSelectUnion += "B.TOT_PAID_AMT, B.TOT_CHRG_AMT, B.TOT_PRICED_AMT, B.TOT_oth_INS_AMT, B.NON_PAR_SW, B.OUT_OF_AREA_SW, B.RENDERING_NPI,  B.PRICED_BY, B.PPO, B.SURPRISE_BILLING_INDICATOR, "
            SQLSelectUnion += "B.INCIDENT_DATE, B.PENDED_TO, COALESCE(B.PROCESSED_DATE,B.STATUS_DATE) AS PROCESSED_DATE , "
            SQLSelectUnion += "COALESCE(CASE WHEN LENGTH(RTRIM(B.PROCESSED_BY)) = 0 THEN NULL ELSE RTRIM(B.PROCESSED_BY) END, B.USERID) AS PROCESSED_BY, "
            SQLSelectUnion += "B.ARCHIVE_SW, B.REFRENCE_CLAIM, B.REFERENCE_ID,  B.MAXID, B.USERID " & If(IncludePCs, " , B.PROCEDURES", "") & If(IncludeReopenableReason, " , B.REASON, B.REOPENABLE ", "")
            SQLFromUnion += " FROM FDBEL.REG_MASTER A INNER JOIN "
            SQLFromUnion += "      FDBMD.CLAIM_MASTER C ON C.FAMILY_ID = A.FAMILY_ID AND C.RELATION_ID = A.RELATION_ID " & LimitJoin(xmlNodeLst)

            If IncludeReason Then
                SQLFromUnion += " INNER JOIN FDBMD.REASON R ON C.CLAIM_ID = R.CLAIM_ID "
            End If

            If IncludeDtl Then
                SQLFromUnion += " INNER JOIN FDBMD.MEDHDR H ON C.CLAIM_ID = H.CLAIM_ID "
                SQLFromUnion += " INNER JOIN FDBMD.MEDDTL D ON C.CLAIM_ID = D.CLAIM_ID "
            ElseIf IncludeHdr Then
                SQLFromUnion += " INNER JOIN FDBMD.MEDHDR H ON C.CLAIM_ID = H.CLAIM_ID "
            End If

            If IncludeDiag Then
                SQLFromUnion += " INNER JOIN FDBMD.MEDDIAG I ON C.CLAIM_ID = I.CLAIM_ID "
            End If

            If IncludeReopenableReason AndAlso Not IncludeReason Then
                SQLFromUnion += " LEFT OUTER JOIN FDBMD.REASON R ON C.CLAIM_ID = R.CLAIM_ID "
            End If

            If IncludeMod Then
                SQLFromUnion += " LEFT OUTER JOIN FDBMD.MEDMOD O ON C.CLAIM_ID = O.CLAIM_ID "
            End If

            If SQLWhere.Trim.Length > 0 AndAlso SQLWhere.IndexOf("P.") >= 0 Then
                If SQLWhere.IndexOf("NAME1") >= 0 OrElse SQLWhere.IndexOf("NPI") >= 0 OrElse SQLWhere.IndexOf("TAXID") >= 0 OrElse SQLWhere.IndexOf("PROV_ID") >= 0 Then ' Provider specific spearch, so assume provider must exist
                    SQLFromUnion += " INNER JOIN FDBMD.PROVIDER P ON C.PROV_ID = P.PROVIDER_ID "
                Else
                    SQLFromUnion += " LEFT OUTER JOIN FDBMD.PROVIDER P ON C.PROV_ID = P.PROVIDER_ID "
                End If

            End If

            SQLFromUnion += " LEFT OUTER JOIN "
            SQLFromUnion += " ( SELECT 0 AS DOCID, 0 AS CLAIM_ID, 0 AS PART_SSN,0 AS PAT_SSN, 0 AS FAMILY_ID,0 AS RELATION_ID, 'RESTRICTED' as STATUS,CAST(NULL AS TIMESTAMP) AS STATUS_DATE, " &
                            "'' AS DOC_CLASS, '' AS DOC_TYPE , 0 AS PAGE_COUNT, '' AS BATCH, CAST(NULL AS DATE) AS REC_DATE,  CAST(NULL AS TIMESTAMP) AS OPEN_DATE, " &
                            "CAST(NULL AS DATE) AS OCC_FROM_DATE,CAST(NULL AS DATE) AS OCC_TO_DATE,CAST(NULL AS DATE) AS NETWRK_REC_DATE, CAST(NULL AS TIMESTAMP)  AS SCAN_DATE,'' AS PAT_FNAME, '' AS PAT_LNAME, '' AS PAT_INT , '' AS PART_FNAME, " &
                            "'' AS PART_LNAME, '' AS PART_INT,  '' AS PAYEE, '' AS DESCRIPTION,'' AS EMPLOYEE, 0 AS TRUST_SW, 0 AS LOCAL_SW, 0 AS AUTHORIZED_SW, 0 AS CHIRO_SW , 0 AS PROV_TIN, " &
                            "0 AS PROV_ID, 0 AS TOT_HRA_AMT, '' AS NAME1,0 AS TOT_PAID_AMT, 0 AS TOT_CHRG_AMT, 0 AS TOT_PRICED_AMT, 0 AS TOT_oth_INS_AMT, 0 AS NON_PAR_SW, 0 AS OUT_OF_AREA_SW, 0 AS RENDERING_NPI,  '' AS PRICED_BY, '' AS PPO, '' AS SURPRISE_BILLING_INDICATOR, CAST(NULL AS DATE) AS INCIDENT_DATE, '' AS PENDED_TO, " &
                            "CAST(NULL AS TIMESTAMP) AS  PROCESSED_DATE ,'' AS PROCESSED_BY, 0 AS ARCHIVE_SW, CAST(NULL AS INTEGER)  AS REFRENCE_CLAIM, CAST(NULL AS VARCHAR(30)) as REFERENCE_ID, '' AS MAXID, '' AS USERID " & If(IncludePCs, " , '' AS PROCEDURES ", "") & If(IncludeReopenableReason, " , '' AS REASON, '' AS REOPENABLE ", "") &
                            " FROM SYSIBM.SYSDUMMY1 ) B ON A.FAMILY_ID = B.FAMILY_ID "

            If IncludeReopenableReason Then 'non union from block should include reason code SQL
                SQLSelect = SQLSelect.Replace("R.REASON, RO.REOPENABLE", SQLFlattenReasonSelect)
            End If

            If IncludePCs Then 'non union from block should include reason code SQL
                SQLSelect = SQLSelect.Replace("PC.PROCEDURES", SQLFlattenProceduresSelect)
            End If

            UnionSQL = SQLSelectUnion & SQLFromUnion & " WHERE " & SQLWhereUnion

            RegularSQL = SQLSelect & SQLFrom & " WHERE " & SQLWhere

            _LastSQL = RegularSQL & " UNION ALL " & UnionSQL & " FOR READ ONLY WITH UR"
            _LastSQL = _LastSQL.Replace(vbTab, " ")
            _LastSQL = _LastSQL.Replace(vbCrLf, " ")

            Do While (_LastSQL.Contains("  "))
                ' if true, the string still contains double spaces,
                ' replace with single space
                _LastSQL = _LastSQL.Replace("  ", " ")
            Loop

            If System.Configuration.ConfigurationManager.AppSettings("LogSQLFileName") IsNot Nothing AndAlso System.Configuration.ConfigurationManager.AppSettings("LogSQLFileName").ToString.Trim.Length > 0 Then
                CMSDALLog.Log("Query Timestamp: " & UFCWGeneral.NowDate & " --Query: " & _LastSQL, CMSDALLog.LogDirectory & String.Format("{0000}", UFCWGeneral.NowDate.Year) & String.Format("{00}", UFCWGeneral.NowDate.Month) & System.Configuration.ConfigurationManager.AppSettings("LogSQLFileName"))
            End If

            UFCWLastKeyData.SQL = _LastSQL

            If _LastSQL.Length < 7000 AndAlso If(System.Configuration.ConfigurationManager.AppSettings("UseRUNIMMEDIATEROWSET") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("UseRUNIMMEDIATEROWSET"))) = 1 Then

                DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RUNIMMEDIATEROWSETSELECT")
                DB.AddInParameter(DBCommandWrapper, "SQLInput", DbType.String, _LastSQL)

            ElseIf _LastSQL.Length < 7000 AndAlso If(System.Configuration.ConfigurationManager.AppSettings("UseRUNIMMEDIATELOG") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("UseRUNIMMEDIATELOG"))) = 1 Then

                DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RUNIMMEDIATESELECTLOG")
                DB.AddInParameter(DBCommandWrapper, "SQLInput", DbType.String, _LastSQL)

            ElseIf _LastSQL.Length < 7000 AndAlso If(System.Configuration.ConfigurationManager.AppSettings("UseRUNIMMEDIATE") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("UseRUNIMMEDIATE"))) = 1 Then

                DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RUNIMMEDIATESELECT")
                DB.AddInParameter(DBCommandWrapper, "SQLInput", DbType.String, _LastSQL)

            Else
                DBCommandWrapper = DB.GetSqlStringCommand(_LastSQL)
            End If

            DBCommandWrapper.CommandTimeout = CMSDALCommon.GetTimeOut

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            If DS.Tables.Count > 0 Then
                DS.Tables(0).TableName = "CustomerServiceResults"

                Return DS.Tables(0)
            End If

            Return Nothing

        Catch ex As System.IO.IOException

            MsgBox("Search Request is taking too long and was cancelled. " & Environment.NewLine() & " Try limiting your search by including other search criteria such as a date range.", MsgBoxStyle.Exclamation, "Search TimeOut occurred.")
            Return Nothing

        Catch ex As DB2Exception

            UFCWLastKeyData.SQL = _LastSQL

            Throw

        Catch ex As Exception

            Throw

        Finally

            If DBCommandWrapper IsNot Nothing Then
                DBCommandWrapper.Cancel()
                DBCommandWrapper.Dispose()
            End If
            DBCommandWrapper = Nothing

        End Try


    End Function

    Public Shared Function RUNIMMEDIATESELECT(ByVal sqlQuery As String) As DataTable

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim DS As DataSet

        Try

            UFCWLastKeyData.SQL = sqlQuery

            DB = CMSDALCommon.CreateDatabase()

            If CBool(If(System.Configuration.ConfigurationManager.AppSettings("UseRUNIMMEDIATE") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("UseRUNIMMEDIATE")))) Then
                DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RUNIMMEDIATESELECTLOG")
                DB.AddInParameter(DBCommandWrapper, "SQLInput", DbType.String, sqlQuery)
            Else
                DBCommandWrapper = DB.GetSqlStringCommand(sqlQuery.ToString)
            End If

            DBCommandWrapper.CommandTimeout = If(System.Configuration.ConfigurationManager.AppSettings("RUNIMMEDIATETimeOut") Is Nothing, 120, CInt(System.Configuration.ConfigurationManager.AppSettings("RUNIMMEDIATETimeOut")))
            DS = DB.ExecuteDataSet(DBCommandWrapper)

            If DS.Tables.Count > 0 Then
                Return DS.Tables(0)
            End If

            Return Nothing

        Catch ex As DB2Exception

            Throw

        Catch ex As Exception

            Throw
        Finally

            DBCommandWrapper = Nothing
            DB = Nothing

        End Try

    End Function


    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Retrieves Audit Information
    ' </summary>
    ' <param name="claimId"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    '     [paulw]     11/14/2006  Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function RetrieveAuditInformation(ByVal claimId As Integer) As DataTable
        Dim DS As DataSet
        'DS.Locale = System.Globalization.CultureInfo.InvariantCulture
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_AUDIT_INFORMATION"
        Dim DT As DataTable

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimId)
            DS = DB.ExecuteDataSet(DBCommandWrapper)
            DT = New DataTable("AUDIT_INFORMATION")

            If DS.Tables.Count > 0 Then
                DT = DS.Tables(0)
            End If

            Return DT

        Catch ex As Exception
            Throw
        Finally
            If DT IsNot Nothing Then DT.Dispose()
        End Try
    End Function
#End Region

#Region "Procedures/Functions"

    Public Shared Sub LockClaimMaster(ByRef transaction As DbTransaction)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.LOCK_CLAIM_MASTER"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)
        Catch ex As Exception
            Throw
        End Try
    End Sub


#End Region

    Private Shared Function ValidateBillTypeByPosition(billTypeCode As String, billTypeDS As DataSet) As DataRow
        Dim DR As DataRow
        Dim BillTypeDesc As String

        Try

            If billTypeCode.Trim.Length <> 3 Then Return Nothing

            Select Case billTypeCode.Trim.Substring(0, 1)
                Case "1", "2", "3", "4", "5", "6", "7"
                Case Else
                    Return Nothing
            End Select

            Select Case billTypeCode.Trim.Substring(1, 1)
                Case "1", "2", "3", "4", "5", "6", "7", "8", "9"
                Case Else
                    Return Nothing
            End Select

            Select Case billTypeCode.Trim.Substring(2, 1)
                Case "1", "2", "3", "4", "5", "6", "7", "8"
                Case Else
                    Return Nothing
            End Select

            DR = billTypeDS.Tables(0).NewRow
            DR("BILL_TYPE_VALUE") = billTypeCode
            DR("FROM_DATE") = "1985-02-01" 'current default in table"
            DR("THRU_DATE") = "12/31/9999"

            Select Case billTypeCode.Trim.Substring(0, 1)
                Case "1"
                    BillTypeDesc &= "Hospital"
                Case "2"
                    BillTypeDesc &= "Skilled Nursing Facility"
                Case "3"
                    BillTypeDesc &= "Home Health"
                Case "4"
                    BillTypeDesc &= "Christian Science (Hospital)"
                Case "5"
                    BillTypeDesc &= "Christian Science (Extended Care)"
                Case "6"
                    BillTypeDesc &= "Intermediate Care"
                Case "7"
                    BillTypeDesc &= "Clinic"
            End Select

            Select Case billTypeCode.Trim.Substring(0, 1)
                Case "2"
                    Select Case billTypeCode.Trim.Substring(1, 1)
                        Case "1"
                            BillTypeDesc &= " - " & "Hospice (Non-Hospital Based)"
                        Case "2"
                            BillTypeDesc &= " - " & "Hospice (Hospital Based)"
                        Case "3"
                            BillTypeDesc &= " - " & "Ambulatory Surgery Center (ASC)"
                        Case "4"
                            BillTypeDesc &= " - " & "Freestanding Birthing Center"
                        Case "5"
                            BillTypeDesc &= " - " & "Intermediate Care, Level I"
                        Case "6"
                            BillTypeDesc &= " - " & "Intermediate Care, Level II"
                        Case "7"
                            BillTypeDesc &= " - " & "Intermediate Care, Level III"
                        Case "8"
                            BillTypeDesc &= " - " & "Swing Beds"
                        Case "9"
                            Return Nothing
                    End Select
                Case "7"
                    Select Case billTypeCode.Trim.Substring(1, 1)
                        Case "1"
                            BillTypeDesc &= " - " & "Rural Health"
                        Case "2"
                            BillTypeDesc &= " - " & "Hospital Based or Independent Renal Dialysis Center"
                        Case "3"
                            BillTypeDesc &= " - " & "Free Standing"
                        Case "4"
                            BillTypeDesc &= " - " & "Other Rehabilitation Facility (ORF)"
                        Case "5"
                            BillTypeDesc &= " - " & "Intermediate Care, Level I"
                        Case "6"
                            BillTypeDesc &= " - " & "Intermediate Care, Level II"
                        Case "7"
                            BillTypeDesc &= " - " & "Intermediate Care, Level III"
                        Case "8"
                            BillTypeDesc &= " - " & "Swing Beds"
                        Case "9"
                            BillTypeDesc &= " - " & "Other"
                    End Select
                Case Else
                    Select Case billTypeCode.Trim.Substring(1, 1)
                        Case "1"
                            BillTypeDesc &= " - " & "Inpatient "
                        Case "2"
                            Return Nothing
                        Case "3"
                            BillTypeDesc &= " - " & "Outpatient"
                        Case "4"
                            BillTypeDesc &= " - " & "Other (For Hospital Referenced Diagnostic Services, or Home Health Not Under a Plan of Treatment)"
                        Case "5"
                            BillTypeDesc &= " - " & "Intermediate Care, Level I"
                        Case "6"
                            BillTypeDesc &= " - " & "Intermediate Care, Level II"
                        Case "7"
                            BillTypeDesc &= " - " & "Intermediate Care, Level III"
                        Case "8"
                            BillTypeDesc &= " - " & "Swing Beds"
                        Case "9"
                            Return Nothing
                    End Select
            End Select

            Select Case billTypeCode.Trim.Substring(2, 1)
                Case "1"
                    BillTypeDesc &= " - " & "Admit through Discharge Claim"
                Case "2"
                    BillTypeDesc &= " - " & "Interim – First Claim"
                Case "3"
                    BillTypeDesc &= " - " & "Interim – Continuing Claims"
                Case "4"
                    BillTypeDesc &= " - " & "Interim – Last Claim"
                Case "5"
                    BillTypeDesc &= " - " & "Late Charge only"
                Case "6"
                    BillTypeDesc &= " - " & "Adjustment of Prior Claim"
                Case "7"
                    BillTypeDesc &= " - " & "Replacement of Prior Claim"
                Case "8"
                    BillTypeDesc &= " - " & "Void/Cancel of Prior Claim"
            End Select

            DR("FULL_DESC") = BillTypeDesc
            DR("DESC_1") = BillTypeDesc
            DR("DESC_2") = BillTypeDesc
            DR("DESC_3") = BillTypeDesc

            billTypeDS.Tables(0).Rows.Add(DR)

            Return DR

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function RetrieveUHCOOPAccums(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_UHC_OOP_ACCUM_ENTRIES"

        Dim Tablenames() As String = {"UHC", "MAX_DATE"}


        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DBCommandWrapper.CommandTimeout = 0

            DB.AddInParameter(DBCommandWrapper, "@OOP_YEAR", DbType.String, Now.Year.ToString)

            ds = New DataSet
            DB.LoadDataSet(DBCommandWrapper, ds, Tablenames)

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveOptumOOPAccums(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_OPTUMRX_OOP_ACCUM_ENTRIES"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DBCommandWrapper.CommandTimeout = 0

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
                ds.Tables(0).TableName = "OPTUMRX"
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "OPTUMRX")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "OPTUMRX", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveChemoProcedureValues() As String()

        If _ChemoProcedureValuesDS Is Nothing Then
            _ChemoProcedureValuesDS = RetrieveChemoProcedureValues(_ChemoProcedureValuesDS)
        End If

        Dim ChemoList As New ArrayList(_ChemoProcedureValuesDS.Tables("CHEMO_PROCEDURE_CODES").Rows.Count)

        For Each ChemoRow As DataRowView In _ChemoProcedureValuesDS.Tables("CHEMO_PROCEDURE_CODES").DefaultView
            ChemoList.Add(ChemoRow("PROC_CODE"))
        Next

        Return CType(ChemoList.ToArray(GetType(String)), String())

    End Function

    Private Shared Function RetrieveChemoProcedureValues(ByVal ds As DataSet, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        SyncLock _RetrieveChemoProcedureValuesSyncLock

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_ALL_CHEMO_PROCEDURE_CODES" & ".xml"
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim SWriter As System.IO.StreamWriter
            Dim XMLSerial As XmlSerializer
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBMD.RETRIEVE_ALL_CHEMO_PROCEDURE_CODES"

            Try

                If ds Is Nothing Then
                    DB = CMSDALCommon.CreateDatabase()
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
                    If ds Is Nothing Then
                        If transaction Is Nothing Then
                            ds = DB.ExecuteDataSet(DBCommandWrapper)
                        Else
                            ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                        End If
                        ds.Tables(0).TableName = "CHEMO_PROCEDURE_CODES"
                    Else
                        If transaction Is Nothing Then
                            DB.LoadDataSet(DBCommandWrapper, ds, "CHEMO_PROCEDURE_CODES")
                        Else
                            DB.LoadDataSet(DBCommandWrapper, ds, "CHEMO_PROCEDURE_CODES", transaction)
                        End If
                    End If

                    If File.Exists(XMLFilename) Then File.SetAttributes(XMLFilename, FileAttributes.Normal)

                    SWriter = New System.IO.StreamWriter(XMLFilename)
                    XMLSerial = New XmlSerializer(ds.GetType())
                    XMLSerial.Serialize(SWriter, ds)
                    SWriter.Close()
                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)

                End If

                Return ds

            Catch ex As Exception
                Throw

            Finally
                If SWriter IsNot Nothing Then SWriter.Close()
                SWriter = Nothing

                'mutexFile.ReleaseMutex()

            End Try
        End SyncLock

    End Function

#Region "NPI/BATCH"

    Public Shared Function GetNPIRegistryByNPIForBatch(ByVal NPI As Decimal, Optional ByVal DS As DataSet = Nothing) As DataSet

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim dbCommandWrapper As Data.Common.DbCommand
        Dim SQLCall As String

        Try

            SQLCall = "FDBMD.RETRIEVE_NPI_REGISTRY_BY_NPI_FOR_BATCH"
            dbCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(dbCommandWrapper, "@NPI", DbType.Decimal, NPI)

            If DS Is Nothing Then
                DS = DB.ExecuteDataSet(dbCommandWrapper)
            Else
                DB.LoadDataSet(dbCommandWrapper, DS, "NPI_REGISTRY")
            End If

            Return DS

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Sub InsertNPIforBATCH(
              ByVal NPI As Decimal,
            ByVal EntityTypeCode As Object,
            ByVal ReplacementNPI As Object,
            ByVal EmployerIdentificationNumber As String,
            ByVal ProviderOrganizationName As String,
            ByVal ProviderLastName As String,
            ByVal ProviderFirstName As String,
            ByVal ProviderMiddleName As String,
            ByVal ProviderNamePrefixText As String,
            ByVal ProviderNameSuffixText As String,
            ByVal ProviderCredentialText As String,
            ByVal ProviderOtherOrganizationName As String,
            ByVal ProviderOtherOrganizationNameTypeCode As String,
            ByVal ProviderOtherLastName As String,
            ByVal ProviderOtherFirstName As String,
            ByVal ProviderOtherMiddleName As String,
            ByVal ProviderOtherNamePrefixText As String,
            ByVal ProviderOtherNameSuffixText As String,
            ByVal ProviderOtherCredentialText As String,
            ByVal ProviderOtherLastNameTypeCode As String,
            ByVal ProviderFirstLineBusinessMailingAddress As String,
            ByVal ProviderSecondLineBusinessMailingAddress As String,
            ByVal ProviderBusinessMailingAddressCityName As String,
            ByVal ProviderBusinessMailingAddressStateName As String,
            ByVal ProviderBusinessMailingAddressPostalCode As String,
            ByVal ProviderBusinessMailingAddressCountryCode As String,
            ByVal ProviderBusinessMailingAddressTelephoneNumber As String,
            ByVal ProviderBusinessMailingAddressFaxNumber As String,
            ByVal ProviderFirstLineBusinessPracticeLocationAddress As String,
            ByVal ProviderSecondLineBusinessPracticeLocationAddress As String,
            ByVal ProviderBusinessPracticeLocationAddressCityName As String,
            ByVal ProviderBusinessPracticeLocationAddressStateName As String,
            ByVal ProviderBusinessPracticeLocationAddressPostalCode As String,
            ByVal ProviderBusinessPracticeLocationAddressCountryCode As String,
            ByVal ProviderBusinessPracticeLocationAddressTelephoneNumber As String,
            ByVal ProviderBusinessPracticeLocationAddressFaxNumber As String,
            ByVal ProviderEnumerationDate As Object,
            ByVal LastUpdateDate As Object,
            ByVal NPIDeactivationReasonCode As String,
            ByVal NPIDeactivationDate As Object,
            ByVal NPIReactivationDate As Object,
            ByVal ProviderGenderCode As String,
            ByVal AuthorizedOfficialLastName As String,
            ByVal AuthorizedOfficialFirstName As String,
            ByVal AuthorizedOfficialMiddleName As String,
            ByVal AuthorizedOfficialTitleorPosition As String,
            ByVal AuthorizedOfficialTelephoneNumber As String,
            ByVal HealthcareProviderTaxonomyCode1 As String,
            ByVal ProviderLicenseNumber1 As String,
            ByVal ProviderLicenseNumberStateCode1 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch1 As String,
            ByVal HealthcareProviderTaxonomyCode2 As String,
            ByVal ProviderLicenseNumber2 As String,
            ByVal ProviderLicenseNumberStateCode2 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch2 As String,
            ByVal HealthcareProviderTaxonomyCode3 As String,
            ByVal ProviderLicenseNumber3 As String,
            ByVal ProviderLicenseNumberStateCode3 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch3 As String,
            ByVal HealthcareProviderTaxonomyCode4 As String,
            ByVal ProviderLicenseNumber4 As String,
            ByVal ProviderLicenseNumberStateCode4 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch4 As String,
            ByVal HealthcareProviderTaxonomyCode5 As String,
            ByVal ProviderLicenseNumber5 As String,
            ByVal ProviderLicenseNumberStateCode5 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch5 As String,
            ByVal HealthcareProviderTaxonomyCode6 As String,
            ByVal ProviderLicenseNumber6 As String,
            ByVal ProviderLicenseNumberStateCode6 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch6 As String,
            ByVal HealthcareProviderTaxonomyCode7 As String,
            ByVal ProviderLicenseNumber7 As String,
            ByVal ProviderLicenseNumberStateCode7 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch7 As String,
            ByVal HealthcareProviderTaxonomyCode8 As String,
            ByVal ProviderLicenseNumber8 As String,
            ByVal ProviderLicenseNumberStateCode8 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch8 As String,
            ByVal HealthcareProviderTaxonomyCode9 As String,
            ByVal ProviderLicenseNumber9 As String,
            ByVal ProviderLicenseNumberStateCode9 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch9 As String,
            ByVal HealthcareProviderTaxonomyCode10 As String,
            ByVal ProviderLicenseNumber10 As String,
            ByVal ProviderLicenseNumberStateCode10 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch10 As String,
            ByVal HealthcareProviderTaxonomyCode11 As String,
            ByVal ProviderLicenseNumber11 As String,
            ByVal ProviderLicenseNumberStateCode11 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch11 As String,
            ByVal HealthcareProviderTaxonomyCode12 As String,
            ByVal ProviderLicenseNumber12 As String,
            ByVal ProviderLicenseNumberStateCode12 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch12 As String,
            ByVal HealthcareProviderTaxonomyCode13 As String,
            ByVal ProviderLicenseNumber13 As String,
            ByVal ProviderLicenseNumberStateCode13 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch13 As String,
            ByVal HealthcareProviderTaxonomyCode14 As String,
            ByVal ProviderLicenseNumber14 As String,
            ByVal ProviderLicenseNumberStateCode14 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch14 As String,
            ByVal HealthcareProviderTaxonomyCode15 As String,
            ByVal ProviderLicenseNumber15 As String,
            ByVal ProviderLicenseNumberStateCode15 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch15 As String,
            ByVal OtherProviderIdentifier1 As String,
            ByVal OtherProviderIdentifierTypeCode1 As String,
            ByVal OtherProviderIdentifierState1 As String,
            ByVal OtherProviderIdentifierIssuer1 As String,
            ByVal OtherProviderIdentifier2 As String,
            ByVal OtherProviderIdentifierTypeCode2 As String,
            ByVal OtherProviderIdentifierState2 As String,
            ByVal OtherProviderIdentifierIssuer2 As String,
            ByVal OtherProviderIdentifier3 As String,
            ByVal OtherProviderIdentifierTypeCode3 As String,
            ByVal OtherProviderIdentifierState3 As String,
            ByVal OtherProviderIdentifierIssuer3 As String,
            ByVal OtherProviderIdentifier4 As String,
            ByVal OtherProviderIdentifierTypeCode4 As String,
            ByVal OtherProviderIdentifierState4 As String,
            ByVal OtherProviderIdentifierIssuer4 As String,
            ByVal OtherProviderIdentifier5 As String,
            ByVal OtherProviderIdentifierTypeCode5 As String,
            ByVal OtherProviderIdentifierState5 As String,
            ByVal OtherProviderIdentifierIssuer5 As String,
            ByVal OtherProviderIdentifier6 As String,
            ByVal OtherProviderIdentifierTypeCode6 As String,
            ByVal OtherProviderIdentifierState6 As String,
            ByVal OtherProviderIdentifierIssuer6 As String,
            ByVal OtherProviderIdentifier7 As String,
            ByVal OtherProviderIdentifierTypeCode7 As String,
            ByVal OtherProviderIdentifierState7 As String,
            ByVal OtherProviderIdentifierIssuer7 As String,
            ByVal OtherProviderIdentifier8 As String,
            ByVal OtherProviderIdentifierTypeCode8 As String,
            ByVal OtherProviderIdentifierState8 As String,
            ByVal OtherProviderIdentifierIssuer8 As String,
            ByVal OtherProviderIdentifier9 As String,
            ByVal OtherProviderIdentifierTypeCode9 As String,
            ByVal OtherProviderIdentifierState9 As String,
            ByVal OtherProviderIdentifierIssuer9 As String,
            ByVal OtherProviderIdentifier10 As String,
            ByVal OtherProviderIdentifierTypeCode10 As String,
            ByVal OtherProviderIdentifierState10 As String,
            ByVal OtherProviderIdentifierIssuer10 As String,
            ByVal OtherProviderIdentifier11 As String,
            ByVal OtherProviderIdentifierTypeCode11 As String,
            ByVal OtherProviderIdentifierState11 As String,
            ByVal OtherProviderIdentifierIssuer11 As String,
            ByVal OtherProviderIdentifier12 As String,
            ByVal OtherProviderIdentifierTypeCode12 As String,
            ByVal OtherProviderIdentifierState12 As String,
            ByVal OtherProviderIdentifierIssuer12 As String,
            ByVal OtherProviderIdentifier13 As String,
            ByVal OtherProviderIdentifierTypeCode13 As String,
            ByVal OtherProviderIdentifierState13 As String,
            ByVal OtherProviderIdentifierIssuer13 As String,
            ByVal OtherProviderIdentifier14 As String,
            ByVal OtherProviderIdentifierTypeCode14 As String,
            ByVal OtherProviderIdentifierState14 As String,
            ByVal OtherProviderIdentifierIssuer14 As String,
            ByVal OtherProviderIdentifier15 As String,
            ByVal OtherProviderIdentifierTypeCode15 As String,
            ByVal OtherProviderIdentifierState15 As String,
            ByVal OtherProviderIdentifierIssuer15 As String,
            ByVal OtherProviderIdentifier16 As String,
            ByVal OtherProviderIdentifierTypeCode16 As String,
            ByVal OtherProviderIdentifierState16 As String,
            ByVal OtherProviderIdentifierIssuer16 As String,
            ByVal OtherProviderIdentifier17 As String,
            ByVal OtherProviderIdentifierTypeCode17 As String,
            ByVal OtherProviderIdentifierState17 As String,
            ByVal OtherProviderIdentifierIssuer17 As String,
            ByVal OtherProviderIdentifier18 As String,
            ByVal OtherProviderIdentifierTypeCode18 As String,
            ByVal OtherProviderIdentifierState18 As String,
            ByVal OtherProviderIdentifierIssuer18 As String,
            ByVal OtherProviderIdentifier19 As String,
            ByVal OtherProviderIdentifierTypeCode19 As String,
            ByVal OtherProviderIdentifierState19 As String,
            ByVal OtherProviderIdentifierIssuer19 As String,
            ByVal OtherProviderIdentifier20 As String,
            ByVal OtherProviderIdentifierTypeCode20 As String,
            ByVal OtherProviderIdentifierState20 As String,
            ByVal OtherProviderIdentifierIssuer20 As String,
            ByVal OtherProviderIdentifier21 As String,
            ByVal OtherProviderIdentifierTypeCode21 As String,
            ByVal OtherProviderIdentifierState21 As String,
            ByVal OtherProviderIdentifierIssuer21 As String,
            ByVal OtherProviderIdentifier22 As String,
            ByVal OtherProviderIdentifierTypeCode22 As String,
            ByVal OtherProviderIdentifierState22 As String,
            ByVal OtherProviderIdentifierIssuer22 As String,
            ByVal OtherProviderIdentifier23 As String,
            ByVal OtherProviderIdentifierTypeCode23 As String,
            ByVal OtherProviderIdentifierState23 As String,
            ByVal OtherProviderIdentifierIssuer23 As String,
            ByVal OtherProviderIdentifier24 As String,
            ByVal OtherProviderIdentifierTypeCode24 As String,
            ByVal OtherProviderIdentifierState24 As String,
            ByVal OtherProviderIdentifierIssuer24 As String,
            ByVal OtherProviderIdentifier25 As String,
            ByVal OtherProviderIdentifierTypeCode25 As String,
            ByVal OtherProviderIdentifierState25 As String,
            ByVal OtherProviderIdentifierIssuer25 As String,
            ByVal OtherProviderIdentifier26 As String,
            ByVal OtherProviderIdentifierTypeCode26 As String,
            ByVal OtherProviderIdentifierState26 As String,
            ByVal OtherProviderIdentifierIssuer26 As String,
            ByVal OtherProviderIdentifier27 As String,
            ByVal OtherProviderIdentifierTypeCode27 As String,
            ByVal OtherProviderIdentifierState27 As String,
            ByVal OtherProviderIdentifierIssuer27 As String,
            ByVal OtherProviderIdentifier28 As String,
            ByVal OtherProviderIdentifierTypeCode28 As String,
            ByVal OtherProviderIdentifierState28 As String,
            ByVal OtherProviderIdentifierIssuer28 As String,
            ByVal OtherProviderIdentifier29 As String,
            ByVal OtherProviderIdentifierTypeCode29 As String,
            ByVal OtherProviderIdentifierState29 As String,
            ByVal OtherProviderIdentifierIssuer29 As String,
            ByVal OtherProviderIdentifier30 As String,
            ByVal OtherProviderIdentifierTypeCode30 As String,
            ByVal OtherProviderIdentifierState30 As String,
            ByVal OtherProviderIdentifierIssuer30 As String,
            ByVal OtherProviderIdentifier31 As String,
            ByVal OtherProviderIdentifierTypeCode31 As String,
            ByVal OtherProviderIdentifierState31 As String,
            ByVal OtherProviderIdentifierIssuer31 As String,
            ByVal OtherProviderIdentifier32 As String,
            ByVal OtherProviderIdentifierTypeCode32 As String,
            ByVal OtherProviderIdentifierState32 As String,
            ByVal OtherProviderIdentifierIssuer32 As String,
            ByVal OtherProviderIdentifier33 As String,
            ByVal OtherProviderIdentifierTypeCode33 As String,
            ByVal OtherProviderIdentifierState33 As String,
            ByVal OtherProviderIdentifierIssuer33 As String,
            ByVal OtherProviderIdentifier34 As String,
            ByVal OtherProviderIdentifierTypeCode34 As String,
            ByVal OtherProviderIdentifierState34 As String,
            ByVal OtherProviderIdentifierIssuer34 As String,
            ByVal OtherProviderIdentifier35 As String,
            ByVal OtherProviderIdentifierTypeCode35 As String,
            ByVal OtherProviderIdentifierState35 As String,
            ByVal OtherProviderIdentifierIssuer35 As String,
            ByVal OtherProviderIdentifier36 As String,
            ByVal OtherProviderIdentifierTypeCode36 As String,
            ByVal OtherProviderIdentifierState36 As String,
            ByVal OtherProviderIdentifierIssuer36 As String,
            ByVal OtherProviderIdentifier37 As String,
            ByVal OtherProviderIdentifierTypeCode37 As String,
            ByVal OtherProviderIdentifierState37 As String,
            ByVal OtherProviderIdentifierIssuer37 As String,
            ByVal OtherProviderIdentifier38 As String,
            ByVal OtherProviderIdentifierTypeCode38 As String,
            ByVal OtherProviderIdentifierState38 As String,
            ByVal OtherProviderIdentifierIssuer38 As String,
            ByVal OtherProviderIdentifier39 As String,
            ByVal OtherProviderIdentifierTypeCode39 As String,
            ByVal OtherProviderIdentifierState39 As String,
            ByVal OtherProviderIdentifierIssuer39 As String,
            ByVal OtherProviderIdentifier40 As String,
            ByVal OtherProviderIdentifierTypeCode40 As String,
            ByVal OtherProviderIdentifierState40 As String,
            ByVal OtherProviderIdentifierIssuer40 As String,
            ByVal OtherProviderIdentifier41 As String,
            ByVal OtherProviderIdentifierTypeCode41 As String,
            ByVal OtherProviderIdentifierState41 As String,
            ByVal OtherProviderIdentifierIssuer41 As String,
            ByVal OtherProviderIdentifier42 As String,
            ByVal OtherProviderIdentifierTypeCode42 As String,
            ByVal OtherProviderIdentifierState42 As String,
            ByVal OtherProviderIdentifierIssuer42 As String,
            ByVal OtherProviderIdentifier43 As String,
            ByVal OtherProviderIdentifierTypeCode43 As String,
            ByVal OtherProviderIdentifierState43 As String,
            ByVal OtherProviderIdentifierIssuer43 As String,
            ByVal OtherProviderIdentifier44 As String,
            ByVal OtherProviderIdentifierTypeCode44 As String,
            ByVal OtherProviderIdentifierState44 As String,
            ByVal OtherProviderIdentifierIssuer44 As String,
            ByVal OtherProviderIdentifier45 As String,
            ByVal OtherProviderIdentifierTypeCode45 As String,
            ByVal OtherProviderIdentifierState45 As String,
            ByVal OtherProviderIdentifierIssuer45 As String,
            ByVal OtherProviderIdentifier46 As String,
            ByVal OtherProviderIdentifierTypeCode46 As String,
            ByVal OtherProviderIdentifierState46 As String,
            ByVal OtherProviderIdentifierIssuer46 As String,
            ByVal OtherProviderIdentifier47 As String,
            ByVal OtherProviderIdentifierTypeCode47 As String,
            ByVal OtherProviderIdentifierState47 As String,
            ByVal OtherProviderIdentifierIssuer47 As String,
            ByVal OtherProviderIdentifier48 As String,
            ByVal OtherProviderIdentifierTypeCode48 As String,
            ByVal OtherProviderIdentifierState48 As String,
            ByVal OtherProviderIdentifierIssuer48 As String,
            ByVal OtherProviderIdentifier49 As String,
            ByVal OtherProviderIdentifierTypeCode49 As String,
            ByVal OtherProviderIdentifierState49 As String,
            ByVal OtherProviderIdentifierIssuer49 As String,
            ByVal OtherProviderIdentifier50 As String,
            ByVal OtherProviderIdentifierTypeCode50 As String,
            ByVal OtherProviderIdentifierState50 As String,
            ByVal OtherProviderIdentifierIssuer50 As String,
            ByVal IsSoleProprietor As String,
            ByVal IsOrganizationSubpart As String,
            ByVal ParentOrganizationLBN As String,
            ByVal ParentOrganizationTIN As String,
            ByVal AuthorizedOfficialNamePrefixText As String,
            ByVal AuthorizedOfficialNameSuffixText As String,
            ByVal AuthorizedOfficialCredentialText As String,
            ByVal HealthcareProviderTaxonomyGroup1 As String,
            ByVal HealthcareProviderTaxonomyGroup2 As String,
            ByVal HealthcareProviderTaxonomyGroup3 As String,
            ByVal HealthcareProviderTaxonomyGroup4 As String,
            ByVal HealthcareProviderTaxonomyGroup5 As String,
            ByVal HealthcareProviderTaxonomyGroup6 As String,
            ByVal HealthcareProviderTaxonomyGroup7 As String,
            ByVal HealthcareProviderTaxonomyGroup8 As String,
            ByVal HealthcareProviderTaxonomyGroup9 As String,
            ByVal HealthcareProviderTaxonomyGroup10 As String,
            ByVal HealthcareProviderTaxonomyGroup11 As String,
            ByVal HealthcareProviderTaxonomyGroup12 As String,
            ByVal HealthcareProviderTaxonomyGroup13 As String,
            ByVal HealthcareProviderTaxonomyGroup14 As String,
            ByVal HealthcareProviderTaxonomyGroup15 As String,
            ByVal CertificationDate As Object,
            Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.INSERT_NPI_REGISTRY_FOR_BATCH"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)


#Region "Parameters"



            DB.AddInParameter(DBCommandWrapper, "@NPI", DbType.Decimal, NPI)
            DB.AddInParameter(DBCommandWrapper, "@ENTITY_TYPE_CD", DbType.Int16, UFCWGeneral.ToNullShortHandler(EntityTypeCode))
            DB.AddInParameter(DBCommandWrapper, "@REPLACEMENT_NPI", DbType.Decimal, UFCWGeneral.ToNullDecimalHandler(ReplacementNPI))
            DB.AddInParameter(DBCommandWrapper, "@EMP_IDENTIFICATION_NUM_EIN", DbType.String, EmployerIdentificationNumber)

            DB.AddInParameter(DBCommandWrapper, "@PROV_ORG_NAME_LEGAL_BUS_NAME", DbType.String, ProviderOrganizationName)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LAST_NAME_LEGAL_NAME", DbType.String, ProviderLastName)
            DB.AddInParameter(DBCommandWrapper, "@PROV_FIRST_NAME", DbType.String, ProviderFirstName)
            DB.AddInParameter(DBCommandWrapper, "@PROV_MIDDLE_NAME", DbType.String, ProviderMiddleName)

            DB.AddInParameter(DBCommandWrapper, "@PROV_NAME_PRE_TXT", DbType.String, ProviderNamePrefixText)
            DB.AddInParameter(DBCommandWrapper, "@PROV_NAME_SUF_TXT", DbType.String, ProviderNameSuffixText)
            DB.AddInParameter(DBCommandWrapper, "@PROV_CREDENTIAL_TXT", DbType.String, ProviderCredentialText)
            DB.AddInParameter(DBCommandWrapper, "@PROV_OTH_ORG_NAME", DbType.String, ProviderOtherOrganizationName)

            DB.AddInParameter(DBCommandWrapper, "@PROV_OTH_ORG_NAME_TYPE_CD", DbType.String, ProviderOtherOrganizationNameTypeCode)
            DB.AddInParameter(DBCommandWrapper, "@PROV_OTH_LAST_NAME", DbType.String, ProviderOtherLastName)
            DB.AddInParameter(DBCommandWrapper, "@PROV_OTH_FIRST_NAME", DbType.String, ProviderOtherFirstName)
            DB.AddInParameter(DBCommandWrapper, "@PROV_OTH_MIDDLE_NAME", DbType.String, ProviderOtherMiddleName)

            DB.AddInParameter(DBCommandWrapper, "@PROV_OTH_NAME_PRE_TXT", DbType.String, ProviderOtherNamePrefixText)
            DB.AddInParameter(DBCommandWrapper, "@PROV_OTH_NAME_SUF_TXT", DbType.String, ProviderOtherNameSuffixText)
            DB.AddInParameter(DBCommandWrapper, "@PROV_OTH_CREDENTIAL_TXT", DbType.String, ProviderOtherCredentialText)
            DB.AddInParameter(DBCommandWrapper, "@PROV_OTH_LAST_NAME_TYPE_CD", DbType.String, ProviderOtherLastNameTypeCode)

            DB.AddInParameter(DBCommandWrapper, "@PROV_FST_LN_BUS_MAIL_ADD", DbType.String, ProviderFirstLineBusinessMailingAddress)
            DB.AddInParameter(DBCommandWrapper, "@PROV_SEC_LN_BUS_MAIL_ADD", DbType.String, ProviderSecondLineBusinessMailingAddress)
            DB.AddInParameter(DBCommandWrapper, "@PROV_BUS_MAIL_ADD_CITY_NAME", DbType.String, ProviderBusinessMailingAddressCityName)
            DB.AddInParameter(DBCommandWrapper, "@PROV_BUS_MAIL_ADD_STATE_NAME", DbType.String, ProviderBusinessMailingAddressStateName)


            DB.AddInParameter(DBCommandWrapper, "@PROV_BUS_MAIL_ADD_POSTAL_CD", DbType.String, ProviderBusinessMailingAddressPostalCode)
            DB.AddInParameter(DBCommandWrapper, "@PROV_BUS_MAIL_ADD_CTRY_NON_US", DbType.String, ProviderBusinessMailingAddressCountryCode)
            DB.AddInParameter(DBCommandWrapper, "@PROV_BUS_MAIL_ADD_PHONE_NUM", DbType.String, ProviderBusinessMailingAddressTelephoneNumber)
            DB.AddInParameter(DBCommandWrapper, "@PROV_BUS_MAIL_ADD_FAX_NUM", DbType.String, ProviderBusinessMailingAddressFaxNumber)

            DB.AddInParameter(DBCommandWrapper, "@PROV_FST_LN_BUS_PRAC_ADD", DbType.String, ProviderFirstLineBusinessPracticeLocationAddress)
            DB.AddInParameter(DBCommandWrapper, "@PROV_SEC_LN_BUS_PRAC_ADD", DbType.String, ProviderSecondLineBusinessPracticeLocationAddress)
            DB.AddInParameter(DBCommandWrapper, "@PROV_BUS_PRAC_ADD_CITY_NAME", DbType.String, ProviderBusinessPracticeLocationAddressCityName)
            DB.AddInParameter(DBCommandWrapper, "@PROV_BUS_PRAC_ADD_STATE_NAME", DbType.String, ProviderBusinessPracticeLocationAddressStateName)

            DB.AddInParameter(DBCommandWrapper, "@PROV_BUS_PRAC_ADD_POSTAL_CD", DbType.String, ProviderBusinessPracticeLocationAddressPostalCode)
            DB.AddInParameter(DBCommandWrapper, "@PROV_BUS_PRAC_ADD_CTRY_NON_US", DbType.String, ProviderBusinessPracticeLocationAddressCountryCode)
            DB.AddInParameter(DBCommandWrapper, "@PROV_BUS_PRAC_ADD_PHONE_NUM", DbType.String, ProviderBusinessPracticeLocationAddressTelephoneNumber)
            DB.AddInParameter(DBCommandWrapper, "@PROV_BUS_PRAC_ADD_FAX_NUM", DbType.String, ProviderBusinessPracticeLocationAddressFaxNumber)

            '' DB.AddInParameter(DBCommandWrapper, "@PROV_ENUMERATION_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(Format(ProviderEnumerationDate, "yyyy-MM-dd")))
            ''DB.AddInParameter(DBCommandWrapper, "@CMS_LAST_UPDATE_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(Format(LastUpdateDate, "yyyy-MM-dd")))
            '' DB.AddInParameter(DBCommandWrapper, "@NPI_DEACT_REASON_CD", DbType.String, NPIDeactivationReasonCode)
            '' DB.AddInParameter(DBCommandWrapper, "@NPI_DEACT_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(Format(CDate(NPIDeactivationDate), "yyyy-MM-dd")))

            ''DB.AddInParameter(DBCommandWrapper, "@NPI_REACTIVATION_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(Format(CDate(NPIReactivationDate), "yyyy-MM-dd")))

            '' DB.AddInParameter(DBCommandWrapper, "@PROV_ENUMERATION_DATE", DbType.Date, ProviderEnumerationDate)
            '' DB.AddInParameter(DBCommandWrapper, "@CMS_LAST_UPDATE_DATE", DbType.Date, LastUpdateDate)
            '' DB.AddInParameter(DBCommandWrapper, "@NPI_DEACT_DATE", DbType.Date, NPIDeactivationDate)
            ''DB.AddInParameter(DBCommandWrapper, "@NPI_REACTIVATION_DATE", DbType.Date, NPIReactivationDate)

            DB.AddInParameter(DBCommandWrapper, "@PROV_ENUMERATION_DATE", DbType.Date, If(IsDBNull(ProviderEnumerationDate), UFCWGeneral.ToNullDateHandler(ProviderEnumerationDate), CType(ProviderEnumerationDate, Date)))

            DB.AddInParameter(DBCommandWrapper, "@CMS_LAST_UPDATE_DATE", DbType.Date, If(IsDBNull(LastUpdateDate), UFCWGeneral.ToNullDateHandler(LastUpdateDate), CType(LastUpdateDate, Date)))
            DB.AddInParameter(DBCommandWrapper, "@NPI_DEACT_REASON_CD", DbType.String, NPIDeactivationReasonCode)
            DB.AddInParameter(DBCommandWrapper, "@NPI_DEACT_DATE", DbType.Date, If(IsDBNull(NPIDeactivationDate), UFCWGeneral.ToNullDateHandler(NPIDeactivationDate), CType(NPIDeactivationDate, Date)))

            DB.AddInParameter(DBCommandWrapper, "@NPI_REACTIVATION_DATE", DbType.Date, If(IsDBNull(NPIReactivationDate), UFCWGeneral.ToNullDateHandler(NPIReactivationDate), CType(NPIReactivationDate, Date)))


            DB.AddInParameter(DBCommandWrapper, "@PROV_GENDER_CD", DbType.String, ProviderGenderCode)
            DB.AddInParameter(DBCommandWrapper, "@AUTH_OFF_FIRST_NAME", DbType.String, AuthorizedOfficialFirstName)
            DB.AddInParameter(DBCommandWrapper, "@AUTH_OFF_LAST_NAME", DbType.String, AuthorizedOfficialLastName)
            DB.AddInParameter(DBCommandWrapper, "@AUTH_OFF_MIDDLE_NAME", DbType.String, AuthorizedOfficialMiddleName)
            DB.AddInParameter(DBCommandWrapper, "@AUTH_OFF_TITLE_OR_POSITION", DbType.String, AuthorizedOfficialTitleorPosition)
            DB.AddInParameter(DBCommandWrapper, "@AUTH_OFF_PHONE_NUM", DbType.String, AuthorizedOfficialTelephoneNumber)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_1", DbType.String, HealthcareProviderTaxonomyCode1)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_1", DbType.String, ProviderLicenseNumber1)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_1", DbType.String, ProviderLicenseNumberStateCode1)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_1", DbType.String, HealthcareProviderPrimaryTaxonomySwitch1)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_2", DbType.String, HealthcareProviderTaxonomyCode2)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_2", DbType.String, ProviderLicenseNumber2)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_2", DbType.String, ProviderLicenseNumberStateCode2)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_2", DbType.String, HealthcareProviderPrimaryTaxonomySwitch2)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_3", DbType.String, HealthcareProviderTaxonomyCode3)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_3", DbType.String, ProviderLicenseNumber3)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_3", DbType.String, ProviderLicenseNumberStateCode3)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_3", DbType.String, HealthcareProviderPrimaryTaxonomySwitch3)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_4", DbType.String, HealthcareProviderTaxonomyCode4)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_4", DbType.String, ProviderLicenseNumber4)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_4", DbType.String, ProviderLicenseNumberStateCode4)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_4", DbType.String, HealthcareProviderPrimaryTaxonomySwitch4)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_5", DbType.String, HealthcareProviderTaxonomyCode5)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_5", DbType.String, ProviderLicenseNumber5)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_5", DbType.String, ProviderLicenseNumberStateCode5)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_5", DbType.String, HealthcareProviderPrimaryTaxonomySwitch5)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_6", DbType.String, HealthcareProviderTaxonomyCode6)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_6", DbType.String, ProviderLicenseNumber6)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_6", DbType.String, ProviderLicenseNumberStateCode6)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_6", DbType.String, HealthcareProviderPrimaryTaxonomySwitch6)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_7", DbType.String, HealthcareProviderTaxonomyCode7)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_7", DbType.String, ProviderLicenseNumber7)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_7", DbType.String, ProviderLicenseNumberStateCode7)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_7", DbType.String, HealthcareProviderPrimaryTaxonomySwitch7)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_8", DbType.String, HealthcareProviderTaxonomyCode8)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_8", DbType.String, ProviderLicenseNumber8)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_8", DbType.String, ProviderLicenseNumberStateCode8)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_8", DbType.String, HealthcareProviderPrimaryTaxonomySwitch8)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_9", DbType.String, HealthcareProviderTaxonomyCode9)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_9", DbType.String, ProviderLicenseNumber9)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_9", DbType.String, ProviderLicenseNumberStateCode9)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_9", DbType.String, HealthcareProviderPrimaryTaxonomySwitch9)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_10", DbType.String, HealthcareProviderTaxonomyCode10)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_10", DbType.String, ProviderLicenseNumber10)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_10", DbType.String, ProviderLicenseNumberStateCode10)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_10", DbType.String, HealthcareProviderPrimaryTaxonomySwitch10)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_11", DbType.String, HealthcareProviderTaxonomyCode11)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_11", DbType.String, ProviderLicenseNumber11)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_11", DbType.String, ProviderLicenseNumberStateCode11)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_11", DbType.String, HealthcareProviderPrimaryTaxonomySwitch11)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_12", DbType.String, HealthcareProviderTaxonomyCode12)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_12", DbType.String, ProviderLicenseNumber12)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_12", DbType.String, ProviderLicenseNumberStateCode12)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_12", DbType.String, HealthcareProviderPrimaryTaxonomySwitch12)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_13", DbType.String, HealthcareProviderTaxonomyCode13)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_13", DbType.String, ProviderLicenseNumber13)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_13", DbType.String, ProviderLicenseNumberStateCode13)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_13", DbType.String, HealthcareProviderPrimaryTaxonomySwitch13)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_14", DbType.String, HealthcareProviderTaxonomyCode14)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_14", DbType.String, ProviderLicenseNumber14)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_14", DbType.String, ProviderLicenseNumberStateCode14)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_14", DbType.String, HealthcareProviderPrimaryTaxonomySwitch14)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_15", DbType.String, HealthcareProviderTaxonomyCode15)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_15", DbType.String, ProviderLicenseNumber15)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_15", DbType.String, ProviderLicenseNumberStateCode15)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_15", DbType.String, HealthcareProviderPrimaryTaxonomySwitch15)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_1", DbType.String, OtherProviderIdentifier1)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_1", DbType.String, OtherProviderIdentifierTypeCode1)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_1", DbType.String, OtherProviderIdentifierState1)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_1", DbType.String, OtherProviderIdentifierIssuer1)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_2", DbType.String, OtherProviderIdentifier2)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_2", DbType.String, OtherProviderIdentifierTypeCode2)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_2", DbType.String, OtherProviderIdentifierState2)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_2", DbType.String, OtherProviderIdentifierIssuer2)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_3", DbType.String, OtherProviderIdentifier3)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_3", DbType.String, OtherProviderIdentifierTypeCode3)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_3", DbType.String, OtherProviderIdentifierState3)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_3", DbType.String, OtherProviderIdentifierIssuer3)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_4", DbType.String, OtherProviderIdentifier4)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_4", DbType.String, OtherProviderIdentifierTypeCode4)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_4", DbType.String, OtherProviderIdentifierState4)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_4", DbType.String, OtherProviderIdentifierIssuer4)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_5", DbType.String, OtherProviderIdentifier5)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_5", DbType.String, OtherProviderIdentifierTypeCode5)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_5", DbType.String, OtherProviderIdentifierState5)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_5", DbType.String, OtherProviderIdentifierIssuer5)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_6", DbType.String, OtherProviderIdentifier6)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_6", DbType.String, OtherProviderIdentifierTypeCode6)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_6", DbType.String, OtherProviderIdentifierState6)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_6", DbType.String, OtherProviderIdentifierIssuer6)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_7", DbType.String, OtherProviderIdentifier7)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_7", DbType.String, OtherProviderIdentifierTypeCode7)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_7", DbType.String, OtherProviderIdentifierState7)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_7", DbType.String, OtherProviderIdentifierIssuer7)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_8", DbType.String, OtherProviderIdentifier8)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_8", DbType.String, OtherProviderIdentifierTypeCode8)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_8", DbType.String, OtherProviderIdentifierState8)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_8", DbType.String, OtherProviderIdentifierIssuer8)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_9", DbType.String, OtherProviderIdentifier9)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_9", DbType.String, OtherProviderIdentifierTypeCode9)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_9", DbType.String, OtherProviderIdentifierState9)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_9", DbType.String, OtherProviderIdentifierIssuer9)



            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_10", DbType.String, OtherProviderIdentifier10)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_10", DbType.String, OtherProviderIdentifierTypeCode10)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_10", DbType.String, OtherProviderIdentifierState10)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_10", DbType.String, OtherProviderIdentifierIssuer10)

            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_11", DbType.String, OtherProviderIdentifier11)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_11", DbType.String, OtherProviderIdentifierTypeCode11)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_11", DbType.String, OtherProviderIdentifierState11)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_11", DbType.String, OtherProviderIdentifierIssuer11)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_12", DbType.String, OtherProviderIdentifier12)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_12", DbType.String, OtherProviderIdentifierTypeCode12)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_12", DbType.String, OtherProviderIdentifierState12)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_12", DbType.String, OtherProviderIdentifierIssuer12)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_13", DbType.String, OtherProviderIdentifier13)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_13", DbType.String, OtherProviderIdentifierTypeCode13)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_13", DbType.String, OtherProviderIdentifierState13)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_13", DbType.String, OtherProviderIdentifierIssuer13)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_14", DbType.String, OtherProviderIdentifier14)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_14", DbType.String, OtherProviderIdentifierTypeCode14)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_14", DbType.String, OtherProviderIdentifierState14)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_14", DbType.String, OtherProviderIdentifierIssuer14)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_15", DbType.String, OtherProviderIdentifier15)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_15", DbType.String, OtherProviderIdentifierTypeCode15)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_15", DbType.String, OtherProviderIdentifierState15)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_15", DbType.String, OtherProviderIdentifierIssuer15)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_16", DbType.String, OtherProviderIdentifier16)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_16", DbType.String, OtherProviderIdentifierTypeCode16)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_16", DbType.String, OtherProviderIdentifierState16)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_16", DbType.String, OtherProviderIdentifierIssuer16)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_17", DbType.String, OtherProviderIdentifier17)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_17", DbType.String, OtherProviderIdentifierTypeCode17)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_17", DbType.String, OtherProviderIdentifierState17)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_17", DbType.String, OtherProviderIdentifierIssuer17)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_18", DbType.String, OtherProviderIdentifier18)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_18", DbType.String, OtherProviderIdentifierTypeCode18)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_18", DbType.String, OtherProviderIdentifierState18)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_18", DbType.String, OtherProviderIdentifierIssuer18)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_19", DbType.String, OtherProviderIdentifier19)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_19", DbType.String, OtherProviderIdentifierTypeCode19)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_19", DbType.String, OtherProviderIdentifierState19)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_19", DbType.String, OtherProviderIdentifierIssuer19)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_20", DbType.String, OtherProviderIdentifier20)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_20", DbType.String, OtherProviderIdentifierTypeCode20)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_20", DbType.String, OtherProviderIdentifierState20)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_20", DbType.String, OtherProviderIdentifierIssuer20)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_21", DbType.String, OtherProviderIdentifier21)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_21", DbType.String, OtherProviderIdentifierTypeCode21)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_21", DbType.String, OtherProviderIdentifierState21)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_21", DbType.String, OtherProviderIdentifierIssuer21)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_22", DbType.String, OtherProviderIdentifier22)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_22", DbType.String, OtherProviderIdentifierTypeCode22)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_22", DbType.String, OtherProviderIdentifierState22)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_22", DbType.String, OtherProviderIdentifierIssuer22)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_23", DbType.String, OtherProviderIdentifier23)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_23", DbType.String, OtherProviderIdentifierTypeCode23)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_23", DbType.String, OtherProviderIdentifierState23)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_23", DbType.String, OtherProviderIdentifierIssuer23)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_24", DbType.String, OtherProviderIdentifier24)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_24", DbType.String, OtherProviderIdentifierTypeCode24)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_24", DbType.String, OtherProviderIdentifierState24)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_24", DbType.String, OtherProviderIdentifierIssuer24)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_25", DbType.String, OtherProviderIdentifier25)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_25", DbType.String, OtherProviderIdentifierTypeCode25)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_25", DbType.String, OtherProviderIdentifierState25)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_25", DbType.String, OtherProviderIdentifierIssuer25)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_26", DbType.String, OtherProviderIdentifier26)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_26", DbType.String, OtherProviderIdentifierTypeCode26)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_26", DbType.String, OtherProviderIdentifierState26)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_26", DbType.String, OtherProviderIdentifierIssuer26)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_27", DbType.String, OtherProviderIdentifier27)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_27", DbType.String, OtherProviderIdentifierTypeCode27)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_27", DbType.String, OtherProviderIdentifierState27)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_27", DbType.String, OtherProviderIdentifierIssuer27)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_28", DbType.String, OtherProviderIdentifier28)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_28", DbType.String, OtherProviderIdentifierTypeCode28)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_28", DbType.String, OtherProviderIdentifierState28)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_28", DbType.String, OtherProviderIdentifierIssuer28)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_29", DbType.String, OtherProviderIdentifier29)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_29", DbType.String, OtherProviderIdentifierTypeCode29)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_29", DbType.String, OtherProviderIdentifierState29)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_29", DbType.String, OtherProviderIdentifierIssuer29)



            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_30", DbType.String, OtherProviderIdentifier30)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_30", DbType.String, OtherProviderIdentifierTypeCode30)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_30", DbType.String, OtherProviderIdentifierState30)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_30", DbType.String, OtherProviderIdentifierIssuer30)

            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_31", DbType.String, OtherProviderIdentifier31)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_31", DbType.String, OtherProviderIdentifierTypeCode31)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_31", DbType.String, OtherProviderIdentifierState31)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_31", DbType.String, OtherProviderIdentifierIssuer31)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_32", DbType.String, OtherProviderIdentifier32)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_32", DbType.String, OtherProviderIdentifierTypeCode32)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_32", DbType.String, OtherProviderIdentifierState32)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_32", DbType.String, OtherProviderIdentifierIssuer32)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_33", DbType.String, OtherProviderIdentifier33)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_33", DbType.String, OtherProviderIdentifierTypeCode33)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_33", DbType.String, OtherProviderIdentifierState33)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_33", DbType.String, OtherProviderIdentifierIssuer33)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_34", DbType.String, OtherProviderIdentifier34)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_34", DbType.String, OtherProviderIdentifierTypeCode34)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_34", DbType.String, OtherProviderIdentifierState34)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_34", DbType.String, OtherProviderIdentifierIssuer34)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_35", DbType.String, OtherProviderIdentifier35)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_35", DbType.String, OtherProviderIdentifierTypeCode35)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_35", DbType.String, OtherProviderIdentifierState35)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_35", DbType.String, OtherProviderIdentifierIssuer35)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_36", DbType.String, OtherProviderIdentifier36)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_36", DbType.String, OtherProviderIdentifierTypeCode36)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_36", DbType.String, OtherProviderIdentifierState36)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_36", DbType.String, OtherProviderIdentifierIssuer36)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_37", DbType.String, OtherProviderIdentifier37)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_37", DbType.String, OtherProviderIdentifierTypeCode37)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_37", DbType.String, OtherProviderIdentifierState37)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_37", DbType.String, OtherProviderIdentifierIssuer37)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_38", DbType.String, OtherProviderIdentifier38)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_38", DbType.String, OtherProviderIdentifierTypeCode38)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_38", DbType.String, OtherProviderIdentifierState38)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_38", DbType.String, OtherProviderIdentifierIssuer38)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_39", DbType.String, OtherProviderIdentifier39)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_39", DbType.String, OtherProviderIdentifierTypeCode39)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_39", DbType.String, OtherProviderIdentifierState39)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_39", DbType.String, OtherProviderIdentifierIssuer39)



            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_40", DbType.String, OtherProviderIdentifier40)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_40", DbType.String, OtherProviderIdentifierTypeCode40)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_40", DbType.String, OtherProviderIdentifierState40)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_40", DbType.String, OtherProviderIdentifierIssuer40)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_41", DbType.String, OtherProviderIdentifier41)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_41", DbType.String, OtherProviderIdentifierTypeCode41)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_41", DbType.String, OtherProviderIdentifierState41)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_41", DbType.String, OtherProviderIdentifierIssuer41)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_42", DbType.String, OtherProviderIdentifier42)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_42", DbType.String, OtherProviderIdentifierTypeCode42)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_42", DbType.String, OtherProviderIdentifierState42)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_42", DbType.String, OtherProviderIdentifierIssuer42)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_43", DbType.String, OtherProviderIdentifier43)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_43", DbType.String, OtherProviderIdentifierTypeCode43)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_43", DbType.String, OtherProviderIdentifierState43)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_43", DbType.String, OtherProviderIdentifierIssuer43)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_44", DbType.String, OtherProviderIdentifier44)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_44", DbType.String, OtherProviderIdentifierTypeCode44)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_44", DbType.String, OtherProviderIdentifierState44)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_44", DbType.String, OtherProviderIdentifierIssuer44)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_45", DbType.String, OtherProviderIdentifier45)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_45", DbType.String, OtherProviderIdentifierTypeCode45)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_45", DbType.String, OtherProviderIdentifierState45)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_45", DbType.String, OtherProviderIdentifierIssuer45)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_46", DbType.String, OtherProviderIdentifier46)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_46", DbType.String, OtherProviderIdentifierTypeCode46)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_46", DbType.String, OtherProviderIdentifierState46)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_46", DbType.String, OtherProviderIdentifierIssuer46)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_47", DbType.String, OtherProviderIdentifier47)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_47", DbType.String, OtherProviderIdentifierTypeCode47)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_47", DbType.String, OtherProviderIdentifierState47)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_47", DbType.String, OtherProviderIdentifierIssuer47)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_48", DbType.String, OtherProviderIdentifier48)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_48", DbType.String, OtherProviderIdentifierTypeCode48)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_48", DbType.String, OtherProviderIdentifierState48)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_48", DbType.String, OtherProviderIdentifierIssuer48)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_49", DbType.String, OtherProviderIdentifier49)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_49", DbType.String, OtherProviderIdentifierTypeCode49)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_49", DbType.String, OtherProviderIdentifierState49)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_49", DbType.String, OtherProviderIdentifierIssuer49)



            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_50", DbType.String, OtherProviderIdentifier50)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_50", DbType.String, OtherProviderIdentifierTypeCode50)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_50", DbType.String, OtherProviderIdentifierState50)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_50", DbType.String, OtherProviderIdentifierIssuer50)

            DB.AddInParameter(DBCommandWrapper, "@S_SOLE_PROPRIETOR", DbType.String, IsSoleProprietor)
            DB.AddInParameter(DBCommandWrapper, "@IS_ORG_SUBPART", DbType.String, IsOrganizationSubpart)
            DB.AddInParameter(DBCommandWrapper, "@PARENT_ORG_LBN", DbType.String, ParentOrganizationLBN)
            DB.AddInParameter(DBCommandWrapper, "@PARENT_ORG_TIN", DbType.String, ParentOrganizationTIN)

            DB.AddInParameter(DBCommandWrapper, "@AUTH_OFFICIAL_NAME_PRE_TXT", DbType.String, AuthorizedOfficialNamePrefixText)
            DB.AddInParameter(DBCommandWrapper, "@AUTH_OFFICIAL_NAME_SUF_TXT", DbType.String, AuthorizedOfficialNameSuffixText)
            DB.AddInParameter(DBCommandWrapper, "@AUTH_OFF_CREDENTIAL_TXT", DbType.String, AuthorizedOfficialCredentialText)
            '' DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String, "IMPORTNPI")
            ''DB.AddInParameter(DBCommandWrapper, "@LASTUPDT", DbType.DateTime, curr)


            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_1", DbType.String, If(HealthcareProviderTaxonomyGroup1.Length > 10, HealthcareProviderTaxonomyGroup1.Substring(0, 10), HealthcareProviderTaxonomyGroup1))
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_2", DbType.String, If(HealthcareProviderTaxonomyGroup2.Length > 10, HealthcareProviderTaxonomyGroup2.Substring(0, 10), HealthcareProviderTaxonomyGroup2))
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_3", DbType.String, If(HealthcareProviderTaxonomyGroup3.Length > 10, HealthcareProviderTaxonomyGroup3.Substring(0, 10), HealthcareProviderTaxonomyGroup3))
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_4", DbType.String, If(HealthcareProviderTaxonomyGroup4.Length > 10, HealthcareProviderTaxonomyGroup4.Substring(0, 10), HealthcareProviderTaxonomyGroup4))
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_5", DbType.String, If(HealthcareProviderTaxonomyGroup5.Length > 10, HealthcareProviderTaxonomyGroup5.Substring(0, 10), HealthcareProviderTaxonomyGroup5))
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_6", DbType.String, If(HealthcareProviderTaxonomyGroup6.Length > 10, HealthcareProviderTaxonomyGroup6.Substring(0, 10), HealthcareProviderTaxonomyGroup6))
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_7", DbType.String, If(HealthcareProviderTaxonomyGroup7.Length > 10, HealthcareProviderTaxonomyGroup7.Substring(0, 10), HealthcareProviderTaxonomyGroup7))
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_8", DbType.String, If(HealthcareProviderTaxonomyGroup8.Length > 10, HealthcareProviderTaxonomyGroup8.Substring(0, 10), HealthcareProviderTaxonomyGroup8))
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_9", DbType.String, If(HealthcareProviderTaxonomyGroup9.Length > 10, HealthcareProviderTaxonomyGroup9.Substring(0, 10), HealthcareProviderTaxonomyGroup9))
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_10", DbType.String, If(HealthcareProviderTaxonomyGroup10.Length > 10, HealthcareProviderTaxonomyGroup10.Substring(0, 10), HealthcareProviderTaxonomyGroup10))
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_11", DbType.String, If(HealthcareProviderTaxonomyGroup11.Length > 10, HealthcareProviderTaxonomyGroup11.Substring(0, 10), HealthcareProviderTaxonomyGroup11))
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_12", DbType.String, If(HealthcareProviderTaxonomyGroup12.Length > 10, HealthcareProviderTaxonomyGroup12.Substring(0, 10), HealthcareProviderTaxonomyGroup12))
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_13", DbType.String, If(HealthcareProviderTaxonomyGroup13.Length > 10, HealthcareProviderTaxonomyGroup13.Substring(0, 10), HealthcareProviderTaxonomyGroup13))
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_14", DbType.String, If(HealthcareProviderTaxonomyGroup14.Length > 10, HealthcareProviderTaxonomyGroup14.Substring(0, 10), HealthcareProviderTaxonomyGroup14))
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_15", DbType.String, If(HealthcareProviderTaxonomyGroup15.Length > 10, HealthcareProviderTaxonomyGroup15.Substring(0, 10), HealthcareProviderTaxonomyGroup15))

            ''DB.AddInParameter(DBCommandWrapper, "@CERTIFICATION_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(Format(CertificationDate, "yyyy-MM-dd")))
            '' DB.AddInParameter(DBCommandWrapper, "@CERTIFICATION_DATE", DbType.Date, If(CertificationDate Is Nothing, UFCWGeneral.ToNullDateHandler(CertificationDate), CType(CertificationDate, Date).ToShortDateString))
            DB.AddInParameter(DBCommandWrapper, "@CERTIFICATION_DATE", DbType.Date, If(IsDBNull(CertificationDate), UFCWGeneral.ToNullDateHandler(CertificationDate), CType(CertificationDate, Date)))

#End Region



            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Shared Sub UpdateNPIforBATCH(
            ByVal NPI As Decimal,
            ByVal EntityTypeCode As Short,
            ByVal ReplacementNPI As Object,
            ByVal EmployerIdentificationNumber As String,
            ByVal ProviderOrganizationName As String,
            ByVal ProviderLastName As String,
            ByVal ProviderFirstName As String,
            ByVal ProviderMiddleName As String,
            ByVal ProviderNamePrefixText As String,
            ByVal ProviderNameSuffixText As String,
            ByVal ProviderCredentialText As String,
            ByVal ProviderOtherOrganizationName As String,
            ByVal ProviderOtherOrganizationNameTypeCode As String,
            ByVal ProviderOtherLastName As String,
            ByVal ProviderOtherFirstName As String,
            ByVal ProviderOtherMiddleName As String,
            ByVal ProviderOtherNamePrefixText As String,
            ByVal ProviderOtherNameSuffixText As String,
            ByVal ProviderOtherCredentialText As String,
            ByVal ProviderOtherLastNameTypeCode As String,
            ByVal ProviderFirstLineBusinessMailingAddress As String,
            ByVal ProviderSecondLineBusinessMailingAddress As String,
            ByVal ProviderBusinessMailingAddressCityName As String,
            ByVal ProviderBusinessMailingAddressStateName As String,
            ByVal ProviderBusinessMailingAddressPostalCode As String,
            ByVal ProviderBusinessMailingAddressCountryCode As String,
            ByVal ProviderBusinessMailingAddressTelephoneNumber As String,
            ByVal ProviderBusinessMailingAddressFaxNumber As String,
            ByVal ProviderFirstLineBusinessPracticeLocationAddress As String,
            ByVal ProviderSecondLineBusinessPracticeLocationAddress As String,
            ByVal ProviderBusinessPracticeLocationAddressCityName As String,
            ByVal ProviderBusinessPracticeLocationAddressStateName As String,
            ByVal ProviderBusinessPracticeLocationAddressPostalCode As String,
            ByVal ProviderBusinessPracticeLocationAddressCountryCode As String,
            ByVal ProviderBusinessPracticeLocationAddressTelephoneNumber As String,
            ByVal ProviderBusinessPracticeLocationAddressFaxNumber As String,
            ByVal ProviderEnumerationDate As Object,
            ByVal LastUpdateDate As Object,
            ByVal NPIDeactivationReasonCode As String,
            ByVal NPIDeactivationDate As Object,
            ByVal NPIReactivationDate As Object,
            ByVal ProviderGenderCode As String,
            ByVal AuthorizedOfficialLastName As String,
            ByVal AuthorizedOfficialFirstName As String,
            ByVal AuthorizedOfficialMiddleName As String,
            ByVal AuthorizedOfficialTitleorPosition As String,
            ByVal AuthorizedOfficialTelephoneNumber As String,
            ByVal HealthcareProviderTaxonomyCode1 As String,
            ByVal ProviderLicenseNumber1 As String,
            ByVal ProviderLicenseNumberStateCode1 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch1 As String,
            ByVal HealthcareProviderTaxonomyCode2 As String,
            ByVal ProviderLicenseNumber2 As String,
            ByVal ProviderLicenseNumberStateCode2 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch2 As String,
            ByVal HealthcareProviderTaxonomyCode3 As String,
            ByVal ProviderLicenseNumber3 As String,
            ByVal ProviderLicenseNumberStateCode3 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch3 As String,
            ByVal HealthcareProviderTaxonomyCode4 As String,
            ByVal ProviderLicenseNumber4 As String,
            ByVal ProviderLicenseNumberStateCode4 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch4 As String,
            ByVal HealthcareProviderTaxonomyCode5 As String,
            ByVal ProviderLicenseNumber5 As String,
            ByVal ProviderLicenseNumberStateCode5 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch5 As String,
            ByVal HealthcareProviderTaxonomyCode6 As String,
            ByVal ProviderLicenseNumber6 As String,
            ByVal ProviderLicenseNumberStateCode6 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch6 As String,
            ByVal HealthcareProviderTaxonomyCode7 As String,
            ByVal ProviderLicenseNumber7 As String,
            ByVal ProviderLicenseNumberStateCode7 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch7 As String,
            ByVal HealthcareProviderTaxonomyCode8 As String,
            ByVal ProviderLicenseNumber8 As String,
            ByVal ProviderLicenseNumberStateCode8 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch8 As String,
            ByVal HealthcareProviderTaxonomyCode9 As String,
            ByVal ProviderLicenseNumber9 As String,
            ByVal ProviderLicenseNumberStateCode9 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch9 As String,
            ByVal HealthcareProviderTaxonomyCode10 As String,
            ByVal ProviderLicenseNumber10 As String,
            ByVal ProviderLicenseNumberStateCode10 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch10 As String,
            ByVal HealthcareProviderTaxonomyCode11 As String,
            ByVal ProviderLicenseNumber11 As String,
            ByVal ProviderLicenseNumberStateCode11 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch11 As String,
            ByVal HealthcareProviderTaxonomyCode12 As String,
            ByVal ProviderLicenseNumber12 As String,
            ByVal ProviderLicenseNumberStateCode12 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch12 As String,
            ByVal HealthcareProviderTaxonomyCode13 As String,
            ByVal ProviderLicenseNumber13 As String,
            ByVal ProviderLicenseNumberStateCode13 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch13 As String,
            ByVal HealthcareProviderTaxonomyCode14 As String,
            ByVal ProviderLicenseNumber14 As String,
            ByVal ProviderLicenseNumberStateCode14 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch14 As String,
            ByVal HealthcareProviderTaxonomyCode15 As String,
            ByVal ProviderLicenseNumber15 As String,
            ByVal ProviderLicenseNumberStateCode15 As String,
            ByVal HealthcareProviderPrimaryTaxonomySwitch15 As String,
            ByVal OtherProviderIdentifier1 As String,
            ByVal OtherProviderIdentifierTypeCode1 As String,
            ByVal OtherProviderIdentifierState1 As String,
            ByVal OtherProviderIdentifierIssuer1 As String,
            ByVal OtherProviderIdentifier2 As String,
            ByVal OtherProviderIdentifierTypeCode2 As String,
            ByVal OtherProviderIdentifierState2 As String,
            ByVal OtherProviderIdentifierIssuer2 As String,
            ByVal OtherProviderIdentifier3 As String,
            ByVal OtherProviderIdentifierTypeCode3 As String,
            ByVal OtherProviderIdentifierState3 As String,
            ByVal OtherProviderIdentifierIssuer3 As String,
            ByVal OtherProviderIdentifier4 As String,
            ByVal OtherProviderIdentifierTypeCode4 As String,
            ByVal OtherProviderIdentifierState4 As String,
            ByVal OtherProviderIdentifierIssuer4 As String,
            ByVal OtherProviderIdentifier5 As String,
            ByVal OtherProviderIdentifierTypeCode5 As String,
            ByVal OtherProviderIdentifierState5 As String,
            ByVal OtherProviderIdentifierIssuer5 As String,
            ByVal OtherProviderIdentifier6 As String,
            ByVal OtherProviderIdentifierTypeCode6 As String,
            ByVal OtherProviderIdentifierState6 As String,
            ByVal OtherProviderIdentifierIssuer6 As String,
            ByVal OtherProviderIdentifier7 As String,
            ByVal OtherProviderIdentifierTypeCode7 As String,
            ByVal OtherProviderIdentifierState7 As String,
            ByVal OtherProviderIdentifierIssuer7 As String,
            ByVal OtherProviderIdentifier8 As String,
            ByVal OtherProviderIdentifierTypeCode8 As String,
            ByVal OtherProviderIdentifierState8 As String,
            ByVal OtherProviderIdentifierIssuer8 As String,
            ByVal OtherProviderIdentifier9 As String,
            ByVal OtherProviderIdentifierTypeCode9 As String,
            ByVal OtherProviderIdentifierState9 As String,
            ByVal OtherProviderIdentifierIssuer9 As String,
            ByVal OtherProviderIdentifier10 As String,
            ByVal OtherProviderIdentifierTypeCode10 As String,
            ByVal OtherProviderIdentifierState10 As String,
            ByVal OtherProviderIdentifierIssuer10 As String,
            ByVal OtherProviderIdentifier11 As String,
            ByVal OtherProviderIdentifierTypeCode11 As String,
            ByVal OtherProviderIdentifierState11 As String,
            ByVal OtherProviderIdentifierIssuer11 As String,
            ByVal OtherProviderIdentifier12 As String,
            ByVal OtherProviderIdentifierTypeCode12 As String,
            ByVal OtherProviderIdentifierState12 As String,
            ByVal OtherProviderIdentifierIssuer12 As String,
            ByVal OtherProviderIdentifier13 As String,
            ByVal OtherProviderIdentifierTypeCode13 As String,
            ByVal OtherProviderIdentifierState13 As String,
            ByVal OtherProviderIdentifierIssuer13 As String,
            ByVal OtherProviderIdentifier14 As String,
            ByVal OtherProviderIdentifierTypeCode14 As String,
            ByVal OtherProviderIdentifierState14 As String,
            ByVal OtherProviderIdentifierIssuer14 As String,
            ByVal OtherProviderIdentifier15 As String,
            ByVal OtherProviderIdentifierTypeCode15 As String,
            ByVal OtherProviderIdentifierState15 As String,
            ByVal OtherProviderIdentifierIssuer15 As String,
            ByVal OtherProviderIdentifier16 As String,
            ByVal OtherProviderIdentifierTypeCode16 As String,
            ByVal OtherProviderIdentifierState16 As String,
            ByVal OtherProviderIdentifierIssuer16 As String,
            ByVal OtherProviderIdentifier17 As String,
            ByVal OtherProviderIdentifierTypeCode17 As String,
            ByVal OtherProviderIdentifierState17 As String,
            ByVal OtherProviderIdentifierIssuer17 As String,
            ByVal OtherProviderIdentifier18 As String,
            ByVal OtherProviderIdentifierTypeCode18 As String,
            ByVal OtherProviderIdentifierState18 As String,
            ByVal OtherProviderIdentifierIssuer18 As String,
            ByVal OtherProviderIdentifier19 As String,
            ByVal OtherProviderIdentifierTypeCode19 As String,
            ByVal OtherProviderIdentifierState19 As String,
            ByVal OtherProviderIdentifierIssuer19 As String,
            ByVal OtherProviderIdentifier20 As String,
            ByVal OtherProviderIdentifierTypeCode20 As String,
            ByVal OtherProviderIdentifierState20 As String,
            ByVal OtherProviderIdentifierIssuer20 As String,
            ByVal OtherProviderIdentifier21 As String,
            ByVal OtherProviderIdentifierTypeCode21 As String,
            ByVal OtherProviderIdentifierState21 As String,
            ByVal OtherProviderIdentifierIssuer21 As String,
            ByVal OtherProviderIdentifier22 As String,
            ByVal OtherProviderIdentifierTypeCode22 As String,
            ByVal OtherProviderIdentifierState22 As String,
            ByVal OtherProviderIdentifierIssuer22 As String,
            ByVal OtherProviderIdentifier23 As String,
            ByVal OtherProviderIdentifierTypeCode23 As String,
            ByVal OtherProviderIdentifierState23 As String,
            ByVal OtherProviderIdentifierIssuer23 As String,
            ByVal OtherProviderIdentifier24 As String,
            ByVal OtherProviderIdentifierTypeCode24 As String,
            ByVal OtherProviderIdentifierState24 As String,
            ByVal OtherProviderIdentifierIssuer24 As String,
            ByVal OtherProviderIdentifier25 As String,
            ByVal OtherProviderIdentifierTypeCode25 As String,
            ByVal OtherProviderIdentifierState25 As String,
            ByVal OtherProviderIdentifierIssuer25 As String,
            ByVal OtherProviderIdentifier26 As String,
            ByVal OtherProviderIdentifierTypeCode26 As String,
            ByVal OtherProviderIdentifierState26 As String,
            ByVal OtherProviderIdentifierIssuer26 As String,
            ByVal OtherProviderIdentifier27 As String,
            ByVal OtherProviderIdentifierTypeCode27 As String,
            ByVal OtherProviderIdentifierState27 As String,
            ByVal OtherProviderIdentifierIssuer27 As String,
            ByVal OtherProviderIdentifier28 As String,
            ByVal OtherProviderIdentifierTypeCode28 As String,
            ByVal OtherProviderIdentifierState28 As String,
            ByVal OtherProviderIdentifierIssuer28 As String,
            ByVal OtherProviderIdentifier29 As String,
            ByVal OtherProviderIdentifierTypeCode29 As String,
            ByVal OtherProviderIdentifierState29 As String,
            ByVal OtherProviderIdentifierIssuer29 As String,
            ByVal OtherProviderIdentifier30 As String,
            ByVal OtherProviderIdentifierTypeCode30 As String,
            ByVal OtherProviderIdentifierState30 As String,
            ByVal OtherProviderIdentifierIssuer30 As String,
            ByVal OtherProviderIdentifier31 As String,
            ByVal OtherProviderIdentifierTypeCode31 As String,
            ByVal OtherProviderIdentifierState31 As String,
            ByVal OtherProviderIdentifierIssuer31 As String,
            ByVal OtherProviderIdentifier32 As String,
            ByVal OtherProviderIdentifierTypeCode32 As String,
            ByVal OtherProviderIdentifierState32 As String,
            ByVal OtherProviderIdentifierIssuer32 As String,
            ByVal OtherProviderIdentifier33 As String,
            ByVal OtherProviderIdentifierTypeCode33 As String,
            ByVal OtherProviderIdentifierState33 As String,
            ByVal OtherProviderIdentifierIssuer33 As String,
            ByVal OtherProviderIdentifier34 As String,
            ByVal OtherProviderIdentifierTypeCode34 As String,
            ByVal OtherProviderIdentifierState34 As String,
            ByVal OtherProviderIdentifierIssuer34 As String,
            ByVal OtherProviderIdentifier35 As String,
            ByVal OtherProviderIdentifierTypeCode35 As String,
            ByVal OtherProviderIdentifierState35 As String,
            ByVal OtherProviderIdentifierIssuer35 As String,
            ByVal OtherProviderIdentifier36 As String,
            ByVal OtherProviderIdentifierTypeCode36 As String,
            ByVal OtherProviderIdentifierState36 As String,
            ByVal OtherProviderIdentifierIssuer36 As String,
            ByVal OtherProviderIdentifier37 As String,
            ByVal OtherProviderIdentifierTypeCode37 As String,
            ByVal OtherProviderIdentifierState37 As String,
            ByVal OtherProviderIdentifierIssuer37 As String,
            ByVal OtherProviderIdentifier38 As String,
            ByVal OtherProviderIdentifierTypeCode38 As String,
            ByVal OtherProviderIdentifierState38 As String,
            ByVal OtherProviderIdentifierIssuer38 As String,
            ByVal OtherProviderIdentifier39 As String,
            ByVal OtherProviderIdentifierTypeCode39 As String,
            ByVal OtherProviderIdentifierState39 As String,
            ByVal OtherProviderIdentifierIssuer39 As String,
            ByVal OtherProviderIdentifier40 As String,
            ByVal OtherProviderIdentifierTypeCode40 As String,
            ByVal OtherProviderIdentifierState40 As String,
            ByVal OtherProviderIdentifierIssuer40 As String,
            ByVal OtherProviderIdentifier41 As String,
            ByVal OtherProviderIdentifierTypeCode41 As String,
            ByVal OtherProviderIdentifierState41 As String,
            ByVal OtherProviderIdentifierIssuer41 As String,
            ByVal OtherProviderIdentifier42 As String,
            ByVal OtherProviderIdentifierTypeCode42 As String,
            ByVal OtherProviderIdentifierState42 As String,
            ByVal OtherProviderIdentifierIssuer42 As String,
            ByVal OtherProviderIdentifier43 As String,
            ByVal OtherProviderIdentifierTypeCode43 As String,
            ByVal OtherProviderIdentifierState43 As String,
            ByVal OtherProviderIdentifierIssuer43 As String,
            ByVal OtherProviderIdentifier44 As String,
            ByVal OtherProviderIdentifierTypeCode44 As String,
            ByVal OtherProviderIdentifierState44 As String,
            ByVal OtherProviderIdentifierIssuer44 As String,
            ByVal OtherProviderIdentifier45 As String,
            ByVal OtherProviderIdentifierTypeCode45 As String,
            ByVal OtherProviderIdentifierState45 As String,
            ByVal OtherProviderIdentifierIssuer45 As String,
            ByVal OtherProviderIdentifier46 As String,
            ByVal OtherProviderIdentifierTypeCode46 As String,
            ByVal OtherProviderIdentifierState46 As String,
            ByVal OtherProviderIdentifierIssuer46 As String,
            ByVal OtherProviderIdentifier47 As String,
            ByVal OtherProviderIdentifierTypeCode47 As String,
            ByVal OtherProviderIdentifierState47 As String,
            ByVal OtherProviderIdentifierIssuer47 As String,
            ByVal OtherProviderIdentifier48 As String,
            ByVal OtherProviderIdentifierTypeCode48 As String,
            ByVal OtherProviderIdentifierState48 As String,
            ByVal OtherProviderIdentifierIssuer48 As String,
            ByVal OtherProviderIdentifier49 As String,
            ByVal OtherProviderIdentifierTypeCode49 As String,
            ByVal OtherProviderIdentifierState49 As String,
            ByVal OtherProviderIdentifierIssuer49 As String,
            ByVal OtherProviderIdentifier50 As String,
            ByVal OtherProviderIdentifierTypeCode50 As String,
            ByVal OtherProviderIdentifierState50 As String,
            ByVal OtherProviderIdentifierIssuer50 As String,
            ByVal IsSoleProprietor As String,
            ByVal IsOrganizationSubpart As String,
            ByVal ParentOrganizationLBN As String,
            ByVal ParentOrganizationTIN As String,
            ByVal AuthorizedOfficialNamePrefixText As String,
            ByVal AuthorizedOfficialNameSuffixText As String,
            ByVal AuthorizedOfficialCredentialText As String,
            ByVal HealthcareProviderTaxonomyGroup1 As String,
            ByVal HealthcareProviderTaxonomyGroup2 As String,
            ByVal HealthcareProviderTaxonomyGroup3 As String,
            ByVal HealthcareProviderTaxonomyGroup4 As String,
            ByVal HealthcareProviderTaxonomyGroup5 As String,
            ByVal HealthcareProviderTaxonomyGroup6 As String,
            ByVal HealthcareProviderTaxonomyGroup7 As String,
            ByVal HealthcareProviderTaxonomyGroup8 As String,
            ByVal HealthcareProviderTaxonomyGroup9 As String,
            ByVal HealthcareProviderTaxonomyGroup10 As String,
            ByVal HealthcareProviderTaxonomyGroup11 As String,
            ByVal HealthcareProviderTaxonomyGroup12 As String,
            ByVal HealthcareProviderTaxonomyGroup13 As String,
            ByVal HealthcareProviderTaxonomyGroup14 As String,
            ByVal HealthcareProviderTaxonomyGroup15 As String,
            ByVal CertificationDate As Object,
            Optional ByVal transaction As DbTransaction = Nothing)


        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.UPDATE_NPI_REGISTRY_FOR_BATCH"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

#Region "Parameters"



            DB.AddInParameter(DBCommandWrapper, "@NPI", DbType.Decimal, NPI)
            DB.AddInParameter(DBCommandWrapper, "@ENTITY_TYPE_CD", DbType.Int16, UFCWGeneral.ToNullShortHandler(EntityTypeCode))
            DB.AddInParameter(DBCommandWrapper, "@REPLACEMENT_NPI", DbType.Decimal, UFCWGeneral.ToNullDecimalHandler(ReplacementNPI))
            DB.AddInParameter(DBCommandWrapper, "@EMP_IDENTIFICATION_NUM_EIN", DbType.String, EmployerIdentificationNumber)

            DB.AddInParameter(DBCommandWrapper, "@PROV_ORG_NAME_LEGAL_BUS_NAME", DbType.String, ProviderOrganizationName)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LAST_NAME_LEGAL_NAME", DbType.String, ProviderLastName)
            DB.AddInParameter(DBCommandWrapper, "@PROV_FIRST_NAME", DbType.String, ProviderFirstName)
            DB.AddInParameter(DBCommandWrapper, "@PROV_MIDDLE_NAME", DbType.String, ProviderMiddleName)

            DB.AddInParameter(DBCommandWrapper, "@PROV_NAME_PRE_TXT", DbType.String, ProviderNamePrefixText)
            DB.AddInParameter(DBCommandWrapper, "@PROV_NAME_SUF_TXT", DbType.String, ProviderNameSuffixText)
            DB.AddInParameter(DBCommandWrapper, "@PROV_CREDENTIAL_TXT", DbType.String, ProviderCredentialText)
            DB.AddInParameter(DBCommandWrapper, "@PROV_OTH_ORG_NAME", DbType.String, ProviderOtherOrganizationName)

            DB.AddInParameter(DBCommandWrapper, "@PROV_OTH_ORG_NAME_TYPE_CD", DbType.String, ProviderOtherOrganizationNameTypeCode)
            DB.AddInParameter(DBCommandWrapper, "@PROV_OTH_LAST_NAME", DbType.String, ProviderOtherLastName)
            DB.AddInParameter(DBCommandWrapper, "@PROV_OTH_FIRST_NAME", DbType.String, ProviderOtherFirstName)
            DB.AddInParameter(DBCommandWrapper, "@PROV_OTH_MIDDLE_NAME", DbType.String, ProviderOtherMiddleName)

            DB.AddInParameter(DBCommandWrapper, "@PROV_OTH_NAME_PRE_TXT", DbType.String, ProviderOtherNamePrefixText)
            DB.AddInParameter(DBCommandWrapper, "@PROV_OTH_NAME_SUF_TXT", DbType.String, ProviderOtherNameSuffixText)
            DB.AddInParameter(DBCommandWrapper, "@PROV_OTH_CREDENTIAL_TXT", DbType.String, ProviderOtherCredentialText)
            DB.AddInParameter(DBCommandWrapper, "@PROV_OTH_LAST_NAME_TYPE_CD", DbType.String, ProviderOtherLastNameTypeCode)

            DB.AddInParameter(DBCommandWrapper, "@PROV_FST_LN_BUS_MAIL_ADD", DbType.String, ProviderFirstLineBusinessMailingAddress)
            DB.AddInParameter(DBCommandWrapper, "@PROV_SEC_LN_BUS_MAIL_ADD", DbType.String, ProviderSecondLineBusinessMailingAddress)
            DB.AddInParameter(DBCommandWrapper, "@PROV_BUS_MAIL_ADD_CITY_NAME", DbType.String, ProviderBusinessMailingAddressCityName)
            DB.AddInParameter(DBCommandWrapper, "@PROV_BUS_MAIL_ADD_STATE_NAME", DbType.String, ProviderBusinessMailingAddressStateName)


            DB.AddInParameter(DBCommandWrapper, "@PROV_BUS_MAIL_ADD_POSTAL_CD", DbType.String, ProviderBusinessMailingAddressPostalCode)
            DB.AddInParameter(DBCommandWrapper, "@PROV_BUS_MAIL_ADD_CTRY_NON_US", DbType.String, ProviderBusinessMailingAddressCountryCode)
            DB.AddInParameter(DBCommandWrapper, "@PROV_BUS_MAIL_ADD_PHONE_NUM", DbType.String, ProviderBusinessMailingAddressTelephoneNumber)
            DB.AddInParameter(DBCommandWrapper, "@PROV_BUS_MAIL_ADD_FAX_NUM", DbType.String, ProviderBusinessMailingAddressFaxNumber)

            DB.AddInParameter(DBCommandWrapper, "@PROV_FST_LN_BUS_PRAC_ADD", DbType.String, ProviderFirstLineBusinessPracticeLocationAddress)
            DB.AddInParameter(DBCommandWrapper, "@PROV_SEC_LN_BUS_PRAC_ADD", DbType.String, ProviderSecondLineBusinessPracticeLocationAddress)
            DB.AddInParameter(DBCommandWrapper, "@PROV_BUS_PRAC_ADD_CITY_NAME", DbType.String, ProviderBusinessPracticeLocationAddressCityName)
            DB.AddInParameter(DBCommandWrapper, "@PROV_BUS_PRAC_ADD_STATE_NAME", DbType.String, ProviderBusinessPracticeLocationAddressStateName)

            DB.AddInParameter(DBCommandWrapper, "@PROV_BUS_PRAC_ADD_POSTAL_CD", DbType.String, ProviderBusinessPracticeLocationAddressPostalCode)
            DB.AddInParameter(DBCommandWrapper, "@PROV_BUS_PRAC_ADD_CTRY_NON_US", DbType.String, ProviderBusinessPracticeLocationAddressCountryCode)
            DB.AddInParameter(DBCommandWrapper, "@PROV_BUS_PRAC_ADD_PHONE_NUM", DbType.String, ProviderBusinessPracticeLocationAddressTelephoneNumber)
            DB.AddInParameter(DBCommandWrapper, "@PROV_BUS_PRAC_ADD_FAX_NUM", DbType.String, ProviderBusinessPracticeLocationAddressFaxNumber)

            '' DB.AddInParameter(DBCommandWrapper, "@PROV_ENUMERATION_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(Format(ProviderEnumerationDate, "yyyy-MM-dd")))
            ''DB.AddInParameter(DBCommandWrapper, "@CMS_LAST_UPDATE_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(Format(LastUpdateDate, "yyyy-MM-dd")))
            '' DB.AddInParameter(DBCommandWrapper, "@NPI_DEACT_REASON_CD", DbType.String, NPIDeactivationReasonCode)
            '' DB.AddInParameter(DBCommandWrapper, "@NPI_DEACT_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(Format(CDate(NPIDeactivationDate), "yyyy-MM-dd")))

            ''DB.AddInParameter(DBCommandWrapper, "@NPI_REACTIVATION_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(Format(CDate(NPIReactivationDate), "yyyy-MM-dd")))

            '' DB.AddInParameter(DBCommandWrapper, "@PROV_ENUMERATION_DATE", DbType.Date, ProviderEnumerationDate)
            '' DB.AddInParameter(DBCommandWrapper, "@CMS_LAST_UPDATE_DATE", DbType.Date, LastUpdateDate)
            '' DB.AddInParameter(DBCommandWrapper, "@NPI_DEACT_DATE", DbType.Date, NPIDeactivationDate)
            ''DB.AddInParameter(DBCommandWrapper, "@NPI_REACTIVATION_DATE", DbType.Date, NPIReactivationDate)

            DB.AddInParameter(DBCommandWrapper, "@PROV_ENUMERATION_DATE", DbType.Date, If(IsDBNull(ProviderEnumerationDate), UFCWGeneral.ToNullDateHandler(ProviderEnumerationDate), CType(ProviderEnumerationDate, Date)))
            DB.AddInParameter(DBCommandWrapper, "@CMS_LAST_UPDATE_DATE", DbType.Date, If(IsDBNull(LastUpdateDate), UFCWGeneral.ToNullDateHandler(LastUpdateDate), CType(LastUpdateDate, Date)))
            DB.AddInParameter(DBCommandWrapper, "@NPI_DEACT_REASON_CD", DbType.String, NPIDeactivationReasonCode)
            DB.AddInParameter(DBCommandWrapper, "@NPI_DEACT_DATE", DbType.Date, If(IsDBNull(NPIDeactivationDate), UFCWGeneral.ToNullDateHandler(NPIDeactivationDate), CType(NPIDeactivationDate, Date)))

            DB.AddInParameter(DBCommandWrapper, "@NPI_REACTIVATION_DATE", DbType.Date, If(IsDBNull(NPIReactivationDate), UFCWGeneral.ToNullDateHandler(NPIReactivationDate), CType(NPIReactivationDate, Date)))


            DB.AddInParameter(DBCommandWrapper, "@PROV_GENDER_CD", DbType.String, ProviderGenderCode)
            DB.AddInParameter(DBCommandWrapper, "@AUTH_OFF_FIRST_NAME", DbType.String, AuthorizedOfficialFirstName)
            DB.AddInParameter(DBCommandWrapper, "@AUTH_OFF_LAST_NAME", DbType.String, AuthorizedOfficialLastName)
            DB.AddInParameter(DBCommandWrapper, "@AUTH_OFF_MIDDLE_NAME", DbType.String, AuthorizedOfficialMiddleName)
            DB.AddInParameter(DBCommandWrapper, "@AUTH_OFF_TITLE_OR_POSITION", DbType.String, AuthorizedOfficialTitleorPosition)
            DB.AddInParameter(DBCommandWrapper, "@AUTH_OFF_PHONE_NUM", DbType.String, AuthorizedOfficialTelephoneNumber)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_1", DbType.String, HealthcareProviderTaxonomyCode1)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_1", DbType.String, ProviderLicenseNumber1)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_1", DbType.String, ProviderLicenseNumberStateCode1)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_1", DbType.String, HealthcareProviderPrimaryTaxonomySwitch1)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_2", DbType.String, HealthcareProviderTaxonomyCode2)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_2", DbType.String, ProviderLicenseNumber2)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_2", DbType.String, ProviderLicenseNumberStateCode2)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_2", DbType.String, HealthcareProviderPrimaryTaxonomySwitch2)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_3", DbType.String, HealthcareProviderTaxonomyCode3)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_3", DbType.String, ProviderLicenseNumber3)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_3", DbType.String, ProviderLicenseNumberStateCode3)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_3", DbType.String, HealthcareProviderPrimaryTaxonomySwitch3)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_4", DbType.String, HealthcareProviderTaxonomyCode4)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_4", DbType.String, ProviderLicenseNumber4)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_4", DbType.String, ProviderLicenseNumberStateCode4)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_4", DbType.String, HealthcareProviderPrimaryTaxonomySwitch4)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_5", DbType.String, HealthcareProviderTaxonomyCode5)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_5", DbType.String, ProviderLicenseNumber5)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_5", DbType.String, ProviderLicenseNumberStateCode5)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_5", DbType.String, HealthcareProviderPrimaryTaxonomySwitch5)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_6", DbType.String, HealthcareProviderTaxonomyCode6)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_6", DbType.String, ProviderLicenseNumber6)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_6", DbType.String, ProviderLicenseNumberStateCode6)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_6", DbType.String, HealthcareProviderPrimaryTaxonomySwitch6)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_7", DbType.String, HealthcareProviderTaxonomyCode7)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_7", DbType.String, ProviderLicenseNumber7)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_7", DbType.String, ProviderLicenseNumberStateCode7)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_7", DbType.String, HealthcareProviderPrimaryTaxonomySwitch7)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_8", DbType.String, HealthcareProviderTaxonomyCode8)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_8", DbType.String, ProviderLicenseNumber8)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_8", DbType.String, ProviderLicenseNumberStateCode8)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_8", DbType.String, HealthcareProviderPrimaryTaxonomySwitch8)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_9", DbType.String, HealthcareProviderTaxonomyCode9)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_9", DbType.String, ProviderLicenseNumber9)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_9", DbType.String, ProviderLicenseNumberStateCode9)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_9", DbType.String, HealthcareProviderPrimaryTaxonomySwitch9)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_10", DbType.String, HealthcareProviderTaxonomyCode10)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_10", DbType.String, ProviderLicenseNumber10)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_10", DbType.String, ProviderLicenseNumberStateCode10)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_10", DbType.String, HealthcareProviderPrimaryTaxonomySwitch10)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_11", DbType.String, HealthcareProviderTaxonomyCode11)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_11", DbType.String, ProviderLicenseNumber11)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_11", DbType.String, ProviderLicenseNumberStateCode11)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_11", DbType.String, HealthcareProviderPrimaryTaxonomySwitch11)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_12", DbType.String, HealthcareProviderTaxonomyCode12)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_12", DbType.String, ProviderLicenseNumber12)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_12", DbType.String, ProviderLicenseNumberStateCode12)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_12", DbType.String, HealthcareProviderPrimaryTaxonomySwitch12)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_13", DbType.String, HealthcareProviderTaxonomyCode13)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_13", DbType.String, ProviderLicenseNumber13)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_13", DbType.String, ProviderLicenseNumberStateCode13)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_13", DbType.String, HealthcareProviderPrimaryTaxonomySwitch13)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_14", DbType.String, HealthcareProviderTaxonomyCode14)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_14", DbType.String, ProviderLicenseNumber14)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_14", DbType.String, ProviderLicenseNumberStateCode14)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_14", DbType.String, HealthcareProviderPrimaryTaxonomySwitch14)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_CD_15", DbType.String, HealthcareProviderTaxonomyCode15)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_15", DbType.String, ProviderLicenseNumber15)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LIC_NUM_STATE_CD_15", DbType.String, ProviderLicenseNumberStateCode15)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_PRIM_TXNMY_SW_15", DbType.String, HealthcareProviderPrimaryTaxonomySwitch15)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_1", DbType.String, OtherProviderIdentifier1)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_1", DbType.String, OtherProviderIdentifierTypeCode1)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_1", DbType.String, OtherProviderIdentifierState1)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_1", DbType.String, OtherProviderIdentifierIssuer1)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_2", DbType.String, OtherProviderIdentifier2)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_2", DbType.String, OtherProviderIdentifierTypeCode2)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_2", DbType.String, OtherProviderIdentifierState2)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_2", DbType.String, OtherProviderIdentifierIssuer2)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_3", DbType.String, OtherProviderIdentifier3)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_3", DbType.String, OtherProviderIdentifierTypeCode3)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_3", DbType.String, OtherProviderIdentifierState3)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_3", DbType.String, OtherProviderIdentifierIssuer3)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_4", DbType.String, OtherProviderIdentifier4)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_4", DbType.String, OtherProviderIdentifierTypeCode4)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_4", DbType.String, OtherProviderIdentifierState4)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_4", DbType.String, OtherProviderIdentifierIssuer4)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_5", DbType.String, OtherProviderIdentifier5)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_5", DbType.String, OtherProviderIdentifierTypeCode5)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_5", DbType.String, OtherProviderIdentifierState5)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_5", DbType.String, OtherProviderIdentifierIssuer5)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_6", DbType.String, OtherProviderIdentifier6)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_6", DbType.String, OtherProviderIdentifierTypeCode6)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_6", DbType.String, OtherProviderIdentifierState6)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_6", DbType.String, OtherProviderIdentifierIssuer6)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_7", DbType.String, OtherProviderIdentifier7)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_7", DbType.String, OtherProviderIdentifierTypeCode7)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_7", DbType.String, OtherProviderIdentifierState7)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_7", DbType.String, OtherProviderIdentifierIssuer7)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_8", DbType.String, OtherProviderIdentifier8)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_8", DbType.String, OtherProviderIdentifierTypeCode8)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_8", DbType.String, OtherProviderIdentifierState8)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_8", DbType.String, OtherProviderIdentifierIssuer8)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_9", DbType.String, OtherProviderIdentifier9)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_9", DbType.String, OtherProviderIdentifierTypeCode9)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_9", DbType.String, OtherProviderIdentifierState9)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_9", DbType.String, OtherProviderIdentifierIssuer9)



            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_10", DbType.String, OtherProviderIdentifier10)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_10", DbType.String, OtherProviderIdentifierTypeCode10)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_10", DbType.String, OtherProviderIdentifierState10)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_10", DbType.String, OtherProviderIdentifierIssuer10)

            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_11", DbType.String, OtherProviderIdentifier11)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_11", DbType.String, OtherProviderIdentifierTypeCode11)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_11", DbType.String, OtherProviderIdentifierState11)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_11", DbType.String, OtherProviderIdentifierIssuer11)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_12", DbType.String, OtherProviderIdentifier12)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_12", DbType.String, OtherProviderIdentifierTypeCode12)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_12", DbType.String, OtherProviderIdentifierState12)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_12", DbType.String, OtherProviderIdentifierIssuer12)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_13", DbType.String, OtherProviderIdentifier13)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_13", DbType.String, OtherProviderIdentifierTypeCode13)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_13", DbType.String, OtherProviderIdentifierState13)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_13", DbType.String, OtherProviderIdentifierIssuer13)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_14", DbType.String, OtherProviderIdentifier14)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_14", DbType.String, OtherProviderIdentifierTypeCode14)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_14", DbType.String, OtherProviderIdentifierState14)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_14", DbType.String, OtherProviderIdentifierIssuer14)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_15", DbType.String, OtherProviderIdentifier15)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_15", DbType.String, OtherProviderIdentifierTypeCode15)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_15", DbType.String, OtherProviderIdentifierState15)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_15", DbType.String, OtherProviderIdentifierIssuer15)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_16", DbType.String, OtherProviderIdentifier16)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_16", DbType.String, OtherProviderIdentifierTypeCode16)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_16", DbType.String, OtherProviderIdentifierState16)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_16", DbType.String, OtherProviderIdentifierIssuer16)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_17", DbType.String, OtherProviderIdentifier17)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_17", DbType.String, OtherProviderIdentifierTypeCode17)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_17", DbType.String, OtherProviderIdentifierState17)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_17", DbType.String, OtherProviderIdentifierIssuer17)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_18", DbType.String, OtherProviderIdentifier18)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_18", DbType.String, OtherProviderIdentifierTypeCode18)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_18", DbType.String, OtherProviderIdentifierState18)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_18", DbType.String, OtherProviderIdentifierIssuer18)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_19", DbType.String, OtherProviderIdentifier19)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_19", DbType.String, OtherProviderIdentifierTypeCode19)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_19", DbType.String, OtherProviderIdentifierState19)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_19", DbType.String, OtherProviderIdentifierIssuer19)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_20", DbType.String, OtherProviderIdentifier20)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_20", DbType.String, OtherProviderIdentifierTypeCode20)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_20", DbType.String, OtherProviderIdentifierState20)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_20", DbType.String, OtherProviderIdentifierIssuer20)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_21", DbType.String, OtherProviderIdentifier21)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_21", DbType.String, OtherProviderIdentifierTypeCode21)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_21", DbType.String, OtherProviderIdentifierState21)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_21", DbType.String, OtherProviderIdentifierIssuer21)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_22", DbType.String, OtherProviderIdentifier22)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_22", DbType.String, OtherProviderIdentifierTypeCode22)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_22", DbType.String, OtherProviderIdentifierState22)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_22", DbType.String, OtherProviderIdentifierIssuer22)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_23", DbType.String, OtherProviderIdentifier23)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_23", DbType.String, OtherProviderIdentifierTypeCode23)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_23", DbType.String, OtherProviderIdentifierState23)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_23", DbType.String, OtherProviderIdentifierIssuer23)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_24", DbType.String, OtherProviderIdentifier24)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_24", DbType.String, OtherProviderIdentifierTypeCode24)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_24", DbType.String, OtherProviderIdentifierState24)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_24", DbType.String, OtherProviderIdentifierIssuer24)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_25", DbType.String, OtherProviderIdentifier25)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_25", DbType.String, OtherProviderIdentifierTypeCode25)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_25", DbType.String, OtherProviderIdentifierState25)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_25", DbType.String, OtherProviderIdentifierIssuer25)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_26", DbType.String, OtherProviderIdentifier26)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_26", DbType.String, OtherProviderIdentifierTypeCode26)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_26", DbType.String, OtherProviderIdentifierState26)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_26", DbType.String, OtherProviderIdentifierIssuer26)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_27", DbType.String, OtherProviderIdentifier27)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_27", DbType.String, OtherProviderIdentifierTypeCode27)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_27", DbType.String, OtherProviderIdentifierState27)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_27", DbType.String, OtherProviderIdentifierIssuer27)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_28", DbType.String, OtherProviderIdentifier28)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_28", DbType.String, OtherProviderIdentifierTypeCode28)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_28", DbType.String, OtherProviderIdentifierState28)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_28", DbType.String, OtherProviderIdentifierIssuer28)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_29", DbType.String, OtherProviderIdentifier29)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_29", DbType.String, OtherProviderIdentifierTypeCode29)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_29", DbType.String, OtherProviderIdentifierState29)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_29", DbType.String, OtherProviderIdentifierIssuer29)



            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_30", DbType.String, OtherProviderIdentifier30)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_30", DbType.String, OtherProviderIdentifierTypeCode30)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_30", DbType.String, OtherProviderIdentifierState30)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_30", DbType.String, OtherProviderIdentifierIssuer30)

            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_31", DbType.String, OtherProviderIdentifier31)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_31", DbType.String, OtherProviderIdentifierTypeCode31)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_31", DbType.String, OtherProviderIdentifierState31)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_31", DbType.String, OtherProviderIdentifierIssuer31)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_32", DbType.String, OtherProviderIdentifier32)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_32", DbType.String, OtherProviderIdentifierTypeCode32)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_32", DbType.String, OtherProviderIdentifierState32)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_32", DbType.String, OtherProviderIdentifierIssuer32)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_33", DbType.String, OtherProviderIdentifier33)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_33", DbType.String, OtherProviderIdentifierTypeCode33)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_33", DbType.String, OtherProviderIdentifierState33)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_33", DbType.String, OtherProviderIdentifierIssuer33)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_34", DbType.String, OtherProviderIdentifier34)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_34", DbType.String, OtherProviderIdentifierTypeCode34)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_34", DbType.String, OtherProviderIdentifierState34)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_34", DbType.String, OtherProviderIdentifierIssuer34)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_35", DbType.String, OtherProviderIdentifier35)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_35", DbType.String, OtherProviderIdentifierTypeCode35)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_35", DbType.String, OtherProviderIdentifierState35)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_35", DbType.String, OtherProviderIdentifierIssuer35)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_36", DbType.String, OtherProviderIdentifier36)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_36", DbType.String, OtherProviderIdentifierTypeCode36)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_36", DbType.String, OtherProviderIdentifierState36)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_36", DbType.String, OtherProviderIdentifierIssuer36)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_37", DbType.String, OtherProviderIdentifier37)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_37", DbType.String, OtherProviderIdentifierTypeCode37)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_37", DbType.String, OtherProviderIdentifierState37)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_37", DbType.String, OtherProviderIdentifierIssuer37)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_38", DbType.String, OtherProviderIdentifier38)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_38", DbType.String, OtherProviderIdentifierTypeCode38)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_38", DbType.String, OtherProviderIdentifierState38)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_38", DbType.String, OtherProviderIdentifierIssuer38)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_39", DbType.String, OtherProviderIdentifier39)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_39", DbType.String, OtherProviderIdentifierTypeCode39)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_39", DbType.String, OtherProviderIdentifierState39)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_39", DbType.String, OtherProviderIdentifierIssuer39)



            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_40", DbType.String, OtherProviderIdentifier40)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_40", DbType.String, OtherProviderIdentifierTypeCode40)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_40", DbType.String, OtherProviderIdentifierState40)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_40", DbType.String, OtherProviderIdentifierIssuer40)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_41", DbType.String, OtherProviderIdentifier41)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_41", DbType.String, OtherProviderIdentifierTypeCode41)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_41", DbType.String, OtherProviderIdentifierState41)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_41", DbType.String, OtherProviderIdentifierIssuer41)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_42", DbType.String, OtherProviderIdentifier42)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_42", DbType.String, OtherProviderIdentifierTypeCode42)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_42", DbType.String, OtherProviderIdentifierState42)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_42", DbType.String, OtherProviderIdentifierIssuer42)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_43", DbType.String, OtherProviderIdentifier43)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_43", DbType.String, OtherProviderIdentifierTypeCode43)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_43", DbType.String, OtherProviderIdentifierState43)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_43", DbType.String, OtherProviderIdentifierIssuer43)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_44", DbType.String, OtherProviderIdentifier44)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_44", DbType.String, OtherProviderIdentifierTypeCode44)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_44", DbType.String, OtherProviderIdentifierState44)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_44", DbType.String, OtherProviderIdentifierIssuer44)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_45", DbType.String, OtherProviderIdentifier45)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_45", DbType.String, OtherProviderIdentifierTypeCode45)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_45", DbType.String, OtherProviderIdentifierState45)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_45", DbType.String, OtherProviderIdentifierIssuer45)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_46", DbType.String, OtherProviderIdentifier46)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_46", DbType.String, OtherProviderIdentifierTypeCode46)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_46", DbType.String, OtherProviderIdentifierState46)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_46", DbType.String, OtherProviderIdentifierIssuer46)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_47", DbType.String, OtherProviderIdentifier47)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_47", DbType.String, OtherProviderIdentifierTypeCode47)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_47", DbType.String, OtherProviderIdentifierState47)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_47", DbType.String, OtherProviderIdentifierIssuer47)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_48", DbType.String, OtherProviderIdentifier48)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_48", DbType.String, OtherProviderIdentifierTypeCode48)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_48", DbType.String, OtherProviderIdentifierState48)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_48", DbType.String, OtherProviderIdentifierIssuer48)


            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_49", DbType.String, OtherProviderIdentifier49)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_49", DbType.String, OtherProviderIdentifierTypeCode49)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_49", DbType.String, OtherProviderIdentifierState49)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_49", DbType.String, OtherProviderIdentifierIssuer49)



            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_50", DbType.String, OtherProviderIdentifier50)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_TYPE_CD_50", DbType.String, OtherProviderIdentifierTypeCode50)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_STATE_50", DbType.String, OtherProviderIdentifierState50)
            DB.AddInParameter(DBCommandWrapper, "@OTH_PROV_IDENT_ISSUER_50", DbType.String, OtherProviderIdentifierIssuer50)

            DB.AddInParameter(DBCommandWrapper, "@S_SOLE_PROPRIETOR", DbType.String, IsSoleProprietor)
            DB.AddInParameter(DBCommandWrapper, "@IS_ORG_SUBPART", DbType.String, IsOrganizationSubpart)
            DB.AddInParameter(DBCommandWrapper, "@PARENT_ORG_LBN", DbType.String, ParentOrganizationLBN)
            DB.AddInParameter(DBCommandWrapper, "@PARENT_ORG_TIN", DbType.String, ParentOrganizationTIN)

            DB.AddInParameter(DBCommandWrapper, "@AUTH_OFFICIAL_NAME_PRE_TXT", DbType.String, AuthorizedOfficialNamePrefixText)
            DB.AddInParameter(DBCommandWrapper, "@AUTH_OFFICIAL_NAME_SUF_TXT", DbType.String, AuthorizedOfficialNameSuffixText)
            DB.AddInParameter(DBCommandWrapper, "@AUTH_OFF_CREDENTIAL_TXT", DbType.String, AuthorizedOfficialCredentialText)
            '' DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String, "IMPORTNPI")
            ''DB.AddInParameter(DBCommandWrapper, "@LASTUPDT", DbType.DateTime, curr)

            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_1", DbType.String, HealthcareProviderTaxonomyGroup1)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_2", DbType.String, HealthcareProviderTaxonomyGroup2)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_3", DbType.String, HealthcareProviderTaxonomyGroup3)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_4", DbType.String, HealthcareProviderTaxonomyGroup4)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_5", DbType.String, HealthcareProviderTaxonomyGroup5)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_6", DbType.String, HealthcareProviderTaxonomyGroup6)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_7", DbType.String, HealthcareProviderTaxonomyGroup7)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_8", DbType.String, HealthcareProviderTaxonomyGroup8)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_9", DbType.String, HealthcareProviderTaxonomyGroup9)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_10", DbType.String, HealthcareProviderTaxonomyGroup10)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_11", DbType.String, HealthcareProviderTaxonomyGroup11)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_12", DbType.String, HealthcareProviderTaxonomyGroup12)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_13", DbType.String, HealthcareProviderTaxonomyGroup13)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_14", DbType.String, HealthcareProviderTaxonomyGroup14)
            DB.AddInParameter(DBCommandWrapper, "@HLTHCARE_PROV_TXNMY_GROUP_15", DbType.String, HealthcareProviderTaxonomyGroup15)

            ''DB.AddInParameter(DBCommandWrapper, "@CERTIFICATION_DATE", DbType.Date, UFCWGeneral.ToNullDateHandler(Format(CertificationDate, "yyyy-MM-dd")))
            ''DB.AddInParameter(DBCommandWrapper, "@CERTIFICATION_DATE", DbType.Date, If(CertificationDate Is Nothing, UFCWGeneral.ToNullDateHandler(CertificationDate), CType(CertificationDate, Date).ToShortDateString))
            DB.AddInParameter(DBCommandWrapper, "@CERTIFICATION_DATE", DbType.Date, If(IsDBNull(CertificationDate), UFCWGeneral.ToNullDateHandler(CertificationDate), CType(CertificationDate, Date)))

#End Region

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub



#End Region

    Public Shared Function UpdateDiagnosisValuesPreventativeSW(ByVal dt As DataTable, HasPreventativeRule As Boolean, Optional ByRef transaction As DbTransaction = Nothing) As DataSet
        Dim SQLCall As String = String.Empty
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        'DB = CMSDALCommon.CreateDatabase()
        Try
            Dim deletesDT As DataTable = dt.GetChanges(DataRowState.Deleted)
            Dim addsDT As DataTable = dt.GetChanges(DataRowState.Added)
            DB = CMSDALCommon.CreateDatabase()

            If addsDT IsNot Nothing And HasPreventativeRule Then
                SQLCall = "FDBMD.UPDATE_DIAGNOSIS_VALUE_ADD_PREVENTATIVE_SW"
                For Each dr As DataRow In addsDT.Rows
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                    DB.AddInParameter(DBCommandWrapper, "@vDIAG_VALUE", DbType.String, dr("DIAGNOSIS").ToString)
                    DB.AddInParameter(DBCommandWrapper, "@vUSERID", DbType.String, Environment.UserName)
                    DB.ExecuteNonQuery(DBCommandWrapper, transaction)
                Next
            End If
        Catch ex As Exception
            Throw

        Finally

        End Try
    End Function

End Class

Public Class RecentClaimEventArgs
    Inherits Global.System.EventArgs
    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _RecentClaimsDT As DataTable
    Public ReadOnly Property RecentClaimsDT As DataTable
        Get
            Return _RecentClaimsDT
        End Get
    End Property
    Public Sub New(ByVal recentClaimsDt As DataTable)
        MyBase.New()
        _RecentClaimsDT = recentClaimsDt
    End Sub

End Class
