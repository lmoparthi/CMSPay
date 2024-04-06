
Option Infer On
Option Strict On

Imports DDTek.DB2
Imports System.Configuration
Imports System.Data.Common
Imports System.Diagnostics
Imports System.IO
Imports Microsoft.Practices.EnterpriseLibrary.Common.Configuration
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Configuration
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Microsoft.Practices.EnterpriseLibrary.Logging


Public NotInheritable Class CMSDALCommon

#Region "Shared Properties"
    Private Shared ReadOnly _TraceBinding As New TraceSwitch("TraceBinding", "Trace Switch in App.Config", "0")
    Private Shared ReadOnly _TraceGeneral As New TraceSwitch("TraceGeneral", "Trace Switch in App.Config", "0")
    Private Shared _TracePerformance As New TraceSwitch("TracePerformance", "Trace Switch in App.Config", "0")
    Private Shared _DB2TraceFile As String

    Private Shared _DBlock As New Object()

    Private Shared _DefaultSQLInstance As String = Nothing
    Private Shared _DefaultDB2Instance As String = Nothing
    Private Shared _EnvironmentOverride As String = Nothing

    Private Shared _Env As String

    Public Shared Property EnvironmentOverride As String
        Get
            Return _EnvironmentOverride
        End Get
        Set
            Debug.Print($"CMSDALCommon.Set.EnvironmentOverride")
            _EnvironmentOverride = Value
        End Set
    End Property

    Public Shared ReadOnly Property DB2TraceFile As String
        Get
            Return _DB2TraceFile
        End Get
    End Property

    Private Shared _Batchconnection As DbConnection

    Public Shared Sub SetupDDTEKLogging(Optional logDirectory As String = Nothing)

        Try

        If System.Configuration.ConfigurationManager.AppSettings("EnableDDTEKLogging") IsNot Nothing AndAlso CDbl(System.Configuration.ConfigurationManager.AppSettings("EnableDDTEKLogging")) = 1 Then
            Select Case logDirectory
                Case Nothing
                    _DB2TraceFile = CMSDALLog.LogDirectory((New String() {"#"}), (New String() {If(CType(ConfigurationManager.GetSection("dataConfiguration"), Microsoft.Practices.EnterpriseLibrary.Data.Configuration.DatabaseSettings).DefaultDatabase.Contains(" P "), "Prod", "Test")})) & "DDTEK" & My.Application.Info.ProductName & UFCWGeneral.WindowsUserID.Name.Replace("\", "_").ToString & String.Format("{0000}", UFCWGeneral.NowDate.Year) & String.Format("{00}", UFCWGeneral.NowDate.Month) & String.Format("{0:00}", UFCWGeneral.NowDate.Day) & If(System.Configuration.ConfigurationManager.AppSettings("IncludeTimeinTraceName") IsNot Nothing AndAlso CBool(System.Configuration.ConfigurationManager.AppSettings("IncludeTimeinTraceName")), String.Format("{0:00}", UFCWGeneral.NowDate.Hour), "").ToString & ".txt"
                Case Else
                    _DB2TraceFile = logDirectory & "DDTEK" & My.Application.Info.ProductName & UFCWGeneral.WindowsUserID.Name.Replace("\", "_").ToString & String.Format("{0000}", UFCWGeneral.NowDate.Year) & String.Format("{00}", UFCWGeneral.NowDate.Month) & String.Format("{0:00}", UFCWGeneral.NowDate.Day) & If(CBool(System.Configuration.ConfigurationManager.AppSettings("IncludeTimeinTraceName") IsNot Nothing AndAlso CBool(System.Configuration.ConfigurationManager.AppSettings("IncludeTimeinTraceName"))), String.Format("{0:00}", UFCWGeneral.NowDate.Hour), "").ToString & ".txt"
            End Select

                If Not Directory.Exists(Path.GetDirectoryName(_DB2TraceFile)) Then
                    Directory.CreateDirectory(Path.GetDirectoryName(_DB2TraceFile))
                End If

            DB2Trace.TraceFile = _DB2TraceFile
            DB2Trace.RecreateTrace = CInt(If(System.Configuration.ConfigurationManager.AppSettings("RecreateTrace") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("RecreateTrace"))))
            DB2Trace.EnableTrace = CInt(If(System.Configuration.ConfigurationManager.AppSettings("StartTraceImmediately") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("StartTraceImmediately"))))
        Else
            _DB2TraceFile = ""
            DB2Trace.EnableTrace = 0
        End If

        Catch ex As Exception

        End Try
    End Sub

    Private Shared _ConfigSource As Microsoft.Practices.EnterpriseLibrary.Common.Configuration.IConfigurationSource
    Private Shared _ELFactory As Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyFactory
    Private Shared _ELExManager As Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionManager

    Public Sub New()

    End Sub

    Shared Sub New()

    End Sub

    Public Shared Sub InitializeEL()

        Try

            If My.Computer.Keyboard.ShiftKeyDown AndAlso My.Computer.Keyboard.CtrlKeyDown Then
                Debug.Print($"CtrlShiftKeyDown")
                CMSDALCommon.EnvironmentOverride = "Q"
            End If

            Logger.SetLogWriter(New LogWriterFactory().Create(), False)

            _ConfigSource = New SystemConfigurationSource()
            _ELFactory = New ExceptionPolicyFactory(_ConfigSource)
            _ELExManager = _ELFactory.CreateManager()

            If _ELExManager.Policies IsNot Nothing Then
                ExceptionPolicy.SetExceptionManager(_ELExManager, False)
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Public Shared ReadOnly Property GetTimeOut() As Integer
        Get

            Dim QueryTimeOut As Integer
            Dim ExtendedSearchMorningStart As Date
            Dim ExtendedSearchMorningEnd As Date
            Dim ExtendedSearchAfternoonStart As Date
            Dim ExtendedSearchAfternoonEnd As Date
            Dim ExtendedSearchEveningStart As Date
            Dim ExtendedSearchEveningEnd As Date

            Try

                ExtendedSearchMorningStart = CDate(CType(ConfigurationManager.GetSection("ExtendedSearchMorningExclusion"), IDictionary)("Start"))
                ExtendedSearchMorningEnd = CDate(CType(ConfigurationManager.GetSection("ExtendedSearchMorningExclusion"), IDictionary)("End"))
                ExtendedSearchAfternoonStart = CDate(CType(ConfigurationManager.GetSection("ExtendedSearchAfternoonExclusion"), IDictionary)("Start"))
                ExtendedSearchAfternoonEnd = CDate(CType(ConfigurationManager.GetSection("ExtendedSearchAfternoonExclusion"), IDictionary)("End"))
                ExtendedSearchEveningStart = CDate(CType(ConfigurationManager.GetSection("ExtendedSearchEveningExclusion"), IDictionary)("Start"))
                ExtendedSearchEveningEnd = CDate(CType(ConfigurationManager.GetSection("ExtendedSearchEveningExclusion"), IDictionary)("End"))


                If (TimeOfDay >= ExtendedSearchMorningStart AndAlso TimeOfDay <= ExtendedSearchMorningEnd) OrElse (TimeOfDay >= ExtendedSearchAfternoonStart AndAlso TimeOfDay <= ExtendedSearchAfternoonEnd) OrElse (TimeOfDay >= ExtendedSearchEveningStart AndAlso TimeOfDay <= ExtendedSearchEveningEnd) Then
                    QueryTimeOut = 600 ' allow 10 mins during non peak else use config default
                Else
                    QueryTimeOut = CInt(If(System.Configuration.ConfigurationManager.AppSettings("RUNIMMEDIATETimeOut") Is Nothing, 120, CInt(System.Configuration.ConfigurationManager.AppSettings("RUNIMMEDIATETimeOut"))))
                End If

                If UFCWGeneralAD.CMSCanRunReports() OrElse UFCWGeneralAD.CMSCanAudit() Then
                    QueryTimeOut = 0
                End If

                Return QueryTimeOut

            Catch ex As Exception
                Throw
            End Try

        End Get
    End Property

    Public Shared ReadOnly Property GetDatabaseName() As String
        Get
            If String.IsNullOrWhiteSpace(_DefaultDB2Instance) Then
                Return GetDatabaseName(Nothing)
            Else
                Return GetDatabaseName(_DefaultDB2Instance)
            End If
        End Get
    End Property

    Public Shared ReadOnly Property GetDatabaseName(ByVal dbConnection As String) As String
        Get

            Dim ConnectionStringName As String
            Dim CSS As ConnectionStringSettings
            Dim ConnString As String
            Dim ConnectionStringParamsA() As String
            Dim DBConnectionA() As String

            Try

                ConnectionStringName = CMSDALCommon.DefaultDatabase

                If String.IsNullOrWhiteSpace(dbConnection) Then ' Is Nothing OrElse dbConnection.Trim.Length = 0 Then
                    dbConnection = ConnectionStringName.Trim
                Else
                    DBConnectionA = dbConnection.Split(New Char() {CChar(";")}, StringSplitOptions.RemoveEmptyEntries)
                    dbConnection = DBConnectionA(0)
                End If

                'Override only works to switch production to QA.
                dbConnection = dbConnection.Replace(" P ", $" {If(_EnvironmentOverride?.Trim.Length > 0, _EnvironmentOverride, "P")} ").Replace(" SQL ", If(_EnvironmentOverride?.Trim.Length > 0, " SQ1 ", " SQL "))

                CSS = ConfigurationManager.ConnectionStrings(dbConnection)
                If CSS Is Nothing Then
                    CSS = ConfigurationManager.ConnectionStrings($"{dbConnection} Database Instance")

                    If CSS Is Nothing Then
                        Throw New ApplicationException($"No connection string found for {dbConnection} in Config file")
                    Else
                        dbConnection = $"{dbConnection} Database Instance"
                    End If
                End If

                ConnString = CSS.ConnectionString.ToString

                ConnectionStringParamsA = ConnString.Split(New Char() {CChar(";")}, StringSplitOptions.RemoveEmptyEntries)

                If ConnString.ToUpper.Contains("DB2") Then
                    For X As Integer = 0 To ConnectionStringParamsA.Length - 1
                        If ConnectionStringParamsA(X).ToUpper.Contains("DATABASE=") Then 'because the Connection string may not identify the database, the database identifier is also modified within the connection string
                            Return ConnectionStringParamsA(X).ToUpper.Replace("DATABASE=", "").ToString.ToUpper.Trim.Replace("DB2P", $"DB2{If(_EnvironmentOverride?.Trim.Length > 0, _EnvironmentOverride, "P")}")
                        End If
                    Next
                Else  'use sql database referenced in connection string, try datasource first
                    'For X As Integer = 0 To ConnectionStringParamsA.Length - 1
                    '    If ConnectionStringParamsA(X).ToUpper.Contains("DATA SOURCE=") Then 'because the Connection string may not identify the database, the database identifier is also modified within the connection string
                    '        Return ConnectionStringParamsA(X).ToUpper.Replace("DATA SOURCE=", "").ToString.Trim.Replace("SQL", $"SQ{If(_EnvironmentOverride?.Trim.Length > 0, _EnvironmentOverride, "L")}")
                    '    End If
                    'Next
                    For X As Integer = 0 To ConnectionStringParamsA.Length - 1
                        If ConnectionStringParamsA(X).ToUpper.Contains("INITIAL CATALOG=") Then 'because the Connection string may not identify the database, the database identifier is also modified within the connection string
                            Return ConnectionStringParamsA(X).ToUpper.Replace("INITIAL CATALOG=", "").ToString.Trim.Replace("SQL", $"SQ{If(_EnvironmentOverride?.Trim.Length > 0, _EnvironmentOverride, "L")}") & ".dbo"
                        End If
                    Next
                End If

                Return "" 'Fallout

            Catch ex As Exception
                Throw
            Finally

            End Try
        End Get

    End Property

    Public Shared ReadOnly Property GetServerName() As String
        Get
            If String.IsNullOrWhiteSpace(_DefaultDB2Instance) Then
                Return GetServerName(Nothing)
            Else
                Return GetServerName(_DefaultDB2Instance)
            End If
        End Get
    End Property

    Public Shared ReadOnly Property GetServerName(ByVal dbinstance As String) As String
        Get

            Dim DB As Database
            Dim ServerInfo() As String
            Dim Server As String = ""

            Try

                If String.IsNullOrWhiteSpace(dbinstance) Then
                    DB = CreateDatabase()
                Else
                    DB = CreateDatabase(dbinstance)
                End If

                ServerInfo = DB.ConnectionString.Split(New Char() {CChar(";")}, StringSplitOptions.RemoveEmptyEntries)

                For Cnt As Integer = 0 To UBound(ServerInfo, 1)
                    Select Case True
                        Case ServerInfo(Cnt).ToLower.Contains("server"), ServerInfo(Cnt).ToLower.Contains("host"), ServerInfo(Cnt).ToLower.Contains("data source")
                            Server = Right(ServerInfo(Cnt), ServerInfo(Cnt).Length - InStr(ServerInfo(Cnt), "="))
                            Exit For
                    End Select
                Next

                For Cnt As Integer = 0 To UBound(ServerInfo, 1)
                    If ServerInfo(Cnt).ToLower.StartsWith("port=") = True Then
                        Server += ";" & ServerInfo(Cnt)
                        Exit For
                    End If
                Next

                Return Server

            Catch ex As Exception
                Throw
            Finally

                DB = Nothing
            End Try

        End Get
    End Property

    Public Shared ReadOnly Property GetDisplayConnectionString(ByVal dbConnection As String) As String
        Get
            Dim DB As Database
            Dim ServerInfo() As String
            Dim Server As String = ""

            Try

                If dbConnection Is Nothing Then
                    DB = CreateDatabase()
                Else
                    DB = CreateDatabase(dbConnection)
                End If

                ServerInfo = DB.ConnectionString.Split(New Char() {CChar(";")}, StringSplitOptions.RemoveEmptyEntries)

                For Cnt As Integer = 0 To UBound(ServerInfo, 1)
                    If ServerInfo(Cnt).ToLower.StartsWith("database=") OrElse ServerInfo(Cnt).ToLower.StartsWith("host=") Then
                        Server = ServerInfo(Cnt)
                        Exit For
                    End If
                Next

                For Cnt As Integer = 0 To UBound(ServerInfo, 1)
                    If ServerInfo(Cnt).ToLower.StartsWith("port=") Then
                        Server += ";" & ServerInfo(Cnt)
                        Exit For
                    End If
                Next

                Return Server

            Catch ex As Exception
                Throw
            Finally
                DB = Nothing
            End Try

        End Get
    End Property

    Public Shared ReadOnly Property DefaultDatabase() As String
        'Returns dataConfiguration defaultDatabase setting modified to include Override when appropriate
        Get
            Dim Settings As DatabaseSettings

            Try

                Using ConfigSource As IConfigurationSource = New SystemConfigurationSource()

                    Settings = DatabaseSettings.GetDatabaseSettings(ConfigSource)

                    Return Settings.DefaultDatabase.Replace(" P ", $" {If(_EnvironmentOverride?.Trim.Length > 0, _EnvironmentOverride, "P")} ").Replace(" SQL ", If(_EnvironmentOverride?.Trim.Length > 0, " SQ1 ", " SQL "))

                End Using

            Catch ex As Exception
                Throw
            End Try

        End Get

    End Property

    Public Shared ReadOnly Property DefaultEnvironment() As String
        'navigate via dataConfiguration defaultDatabase setting to connectionstring and extract DB2 environment P/Q/T/D
        Get
            Dim DefaultDB As String = GetDatabaseName?.Replace("DB2", "")

            Return DefaultDB.First().ToString.ToUpper
        End Get
    End Property

    Public Shared ReadOnly Property DefaultDB2Database() As String
        Get
            Return GetDatabaseName(_DefaultDB2Instance)
        End Get
    End Property

    Public Shared ReadOnly Property DefaultSQLDatabase() As String
        'SQL & DB2 dont use the term database or Instance in the same way, so ServerName is returned for SQL Inquiries
        Get
            Return GetServerName(If(_DefaultSQLInstance, "ImgWorkflow")) 'SQL/SQ1
        End Get
    End Property

    Public Shared Property DefaultDB2Instance() As String
        'if set will override dataConfiguration defaultDatabase for DB2 database calls
        Get
            Return _DefaultDB2Instance
        End Get
        Set(ByVal value As String)
            _DefaultDB2Instance = value
        End Set
    End Property

    Public Shared Property DefaultSQLInstance() As String
        'if set will override dataConfiguration defaultDatabase for SQL database calls
        Get
            Return _DefaultSQLInstance
        End Get
        Set(ByVal value As String)
            _DefaultSQLInstance = value
        End Set
    End Property

    Public Shared Property DBlock As Object
        Get
            Return _DBlock
        End Get
        Set(value As Object)
            _DBlock = value
        End Set
    End Property

    Public Shared Property Batchconnection As DbConnection
        Get
            Return _Batchconnection
        End Get
        Set(value As DbConnection)
            _Batchconnection = value
        End Set
    End Property

#End Region

#Region "Shared Methods"

    Public Shared Function InitializeUserInfo(ByVal dbInstance As String, ByVal userName As String) As String

        Dim EncryptedPW As String
        Try
            EncryptedPW = CType(CMSDALDBO.RetrieveRoleSettings(dbInstance, userName & " Password"), String)
            If EncryptedPW IsNot Nothing AndAlso Not IsDBNull(EncryptedPW) Then
                Return UFCWCryptor.DecryptString(EncryptedPW, userName.Trim & " ACCESS")
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function SelectDistinctAndSortedWithFormat(ByVal tableName As String, ByVal sourceTable As DataTable, ByVal fieldName As String, ByVal addAll As Boolean, ByVal formatMask As String) As DataTable

        Return SelectDistinctAndSorted(tableName, sourceTable, fieldName, addAll, formatMask)

    End Function

    Public Shared Function SelectDistinctAndSorted(ByVal tableName As String, ByVal sourceTable As DataTable, ByVal fieldName As String, ByVal addAll As Boolean) As DataTable

        Return SelectDistinctAndSorted(tableName, sourceTable, fieldName, addAll, "")

    End Function

    Public Shared Function SelectDistinctAndSorted(ByVal tableName As String, ByVal sourceTable As DataTable, ByVal fieldName As String) As DataTable

        Return SelectDistinctAndSorted(tableName, sourceTable, fieldName, False, "")

    End Function

    Public Shared Function SelectDistinctAndSorted(ByVal tableName As String, ByVal sourceTable As DataTable, ByVal fieldName As String, ByVal addAll As Boolean, ByVal formatMask As String) As DataTable
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets Distinct values
        ' </summary>
        ' <param name="tableName"></param>
        ' <param name="sourceTable"></param>
        ' <param name="fieldName"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/18/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DT As DataTable
        Dim DescendingSortOrder As Boolean = False

        Try
            DT = New DataTable(tableName)

            If fieldName.EndsWith(" DESC") Then
                DescendingSortOrder = True
                fieldName = fieldName.Replace(" DESC", "")
            End If

            If sourceTable.Columns.Contains(fieldName) Then

                DT.Columns.Add(fieldName, CType(IIf(addAll OrElse formatMask.Trim.Length > 0, System.Type.GetType("System.String"), sourceTable.Columns(fieldName).DataType), Type))

                Dim SortedValuesQuery =
                    From SortedDistinctList In sourceTable.AsEnumerable()
                    Where SortedDistinctList.Field(Of Object)(fieldName) IsNot Nothing
                    Order By SortedDistinctList.Field(Of Object)(fieldName)
                    Select IIf(formatMask.Trim.Length > 0, Format(SortedDistinctList.Field(Of Object)(fieldName), formatMask).ToString, SortedDistinctList.Field(Of Object)(fieldName))
                    Distinct

                If addAll Then
                    If fieldName.ToUpper <> "DOC_CLASS" Then
                        DT.Rows.Add(New String() {"(all)"})
                    End If
                End If

                If DescendingSortOrder Then
                    For Each SortedValueRow In SortedValuesQuery.Reverse
                        If SortedValueRow IsNot Nothing Then
                            DT.Rows.Add(IIf(addAll OrElse formatMask.Trim.Length > 0, SortedValueRow.ToString, SortedValueRow))
                        End If
                    Next
                Else
                    For Each SortedValueRow In SortedValuesQuery
                        If SortedValueRow IsNot Nothing Then
                            DT.Rows.Add(IIf(addAll OrElse formatMask.Trim.Length > 0, SortedValueRow.ToString, SortedValueRow))
                        End If
                    Next
                End If
            End If

            Return DT

        Catch ex As Exception

            Throw

        Finally
            DT = Nothing
        End Try
    End Function

    <System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions>
    Public Shared Function CanFileBeOpened(db As Database) As Boolean

        Dim Connection As DbConnection

        Try

            Connection = db.CreateConnection()

            Connection.Open()

            Return True

        Catch ex As Exception

            Return False

        Finally

            If Connection IsNot Nothing Then Connection.Close()
            Connection = Nothing

        End Try

    End Function

    Public Shared Function IdentifyPatient(ByRef familyID As Integer?, ByRef relationID As Short?, ByRef trustSW As Decimal?, ByRef partSSN As Integer?, ByRef patSSN As Integer?, ByRef firstName As String, ByRef lastName As String, ByRef patientDOB As Date?, ByRef patientDR As DataRow, ByRef participantDR As DataRow, ByRef FamilyDT As DataTable) As Boolean
        'using what is past the routine attempts to make a definitive identification

        Dim BeginTime As Date = UFCWGeneral.NowDate

        Dim CriteriaUsed As String
        Dim IdentifiedFamilyID As Integer?
        Dim IdentifiedRelationID As Short?

        Try

            FamilyDT = CMSDALFDBMD.IdentifyPatient(partSSN, patSSN, firstName, lastName, patientDOB, CriteriaUsed, IdentifiedFamilyID, IdentifiedRelationID)

            If FamilyDT IsNot Nothing AndAlso FamilyDT.Rows.Count > 0 Then

                Dim QueryParticipant =
                    From Family In FamilyDT.AsEnumerable()
                    Where Family.Field(Of Integer)("RELATION_ID") = 0
                    Select Family

                If QueryParticipant.Count = 1 Then
                    participantDR = QueryParticipant.First
                    trustSW = DirectCast(participantDR("TRUST_SW"), Decimal)
                End If

                Dim QueryPatient =
                    From Family In FamilyDT.AsEnumerable()
                    Where Family.Field(Of Integer)("RELATION_ID") = CInt(IdentifiedRelationID)
                    Select Family

                If QueryPatient.Count = 1 Then
                    patientDR = QueryPatient.First
                End If

                If familyID IsNot Nothing OrElse relationID IsNot Nothing Then
                    If UFCWGeneral.IsNullIntegerHandler(patientDR("FAMILY_ID")) <> familyID OrElse UFCWGeneral.IsNullShortHandler(patientDR("RELATION_ID")) <> relationID Then
                        Throw New ArgumentException("Unexpected Family/RelationID returned", "Family -> " & familyID.ToString & " Relation -> " & relationID.ToString)
                    End If
                End If
            End If

            Return True

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TracePerformance.Level) > 0 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Lookup via " & CriteriaUsed & " duration: " & Generals.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", MethodBase.GetCurrentMethod().DeclaringType.ToString)
#End If
        End Try

    End Function

    Public Shared Function CreateAccumulatorTypeDT(Optional tableName As String = Nothing) As DataTable

        Using DT As New DataTable("Accumulators")

            DT.Columns.Add("ACCUM_ID", System.Type.GetType("System.Int32"))
            DT.Columns.Add("ACCUM_NAME", System.Type.GetType("System.String"))
            DT.Columns.Add("ACCUM_VALUE", System.Type.GetType("System.Decimal"))
            DT.Columns.Add("ORIGINAL_ACCUM_VALUE", System.Type.GetType("System.Decimal"))
            DT.Columns.Add("DISPLAY_ORDER", System.Type.GetType("System.Int32"))
            DT.Columns.Add("ACTIVE_SW", System.Type.GetType("System.Decimal"))
            DT.Columns.Add("MANUAL_SW", System.Type.GetType("System.Decimal"))
            DT.Columns.Add("BATCH_SW", System.Type.GetType("System.Decimal"))
            DT.Columns.Add("DURATION_TYPE", System.Type.GetType("System.Int32"))
            DT.Columns.Add("YEARS", System.Type.GetType("System.Int32"))

            If tableName IsNot Nothing Then
                DT.TableName = tableName
            End If

            Return DT.Clone()

        End Using

    End Function

    Public Shared Function CreateAccumulatorValuesDT(Optional tableName As String = Nothing) As DataTable

        Using DT As New DataTable("Accumulators")
            DT.Columns.Add("ACCUM_ID", System.Type.GetType("System.Int32"))
            DT.Columns.Add("CLAIM_ID", System.Type.GetType("System.Int32"))
            DT.Columns.Add("LINE_NBR", System.Type.GetType("System.Int16"))
            DT.Columns.Add("ORG_ACCIDENT_CLAIM_ID", System.Type.GetType("System.Int32"))
            DT.Columns.Add("FAMILY_ID", System.Type.GetType("System.Int32"))
            DT.Columns.Add("RELATION_ID", System.Type.GetType("System.Int16"))
            DT.Columns.Add("ACCUM_NAME", System.Type.GetType("System.String"))
            DT.Columns.Add("APPLY_DATE", System.Type.GetType("System.DateTime"))
            DT.Columns.Add("ENTRY_VALUE", System.Type.GetType("System.Decimal"))
            DT.Columns.Add("OVERRIDE_SW", System.Type.GetType("System.Decimal"))
            DT.Columns.Add("DISPLAY_ORDER", System.Type.GetType("System.Int32"))
            DT.Columns.Add("CREATE_DATE", System.Type.GetType("System.DateTime"))
            DT.Columns.Add("CREATE_USERID", System.Type.GetType("System.String"))

            If tableName IsNot Nothing Then
                DT.TableName = tableName
            End If

            Return DT.Clone()

        End Using

    End Function

    Public Shared Function CreateAccessDatabase() As Database
        Return CreateAccessConnection(Nothing)
    End Function

    Public Shared Function CreateAccessConnection(ByVal dbConnection As String) As Database
        'allows for use of password protected MDB files
        Dim DB As Database

        Try

            If dbConnection?.Contains("ACCDB") OrElse dbConnection?.Contains("MDB") Then
                Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & " Wait -> " & dbConnection.ToString)
            End If

            SyncLock _DBlock
                If dbConnection?.Contains("ACCDB") OrElse dbConnection?.Contains("MDB") Then
                    Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & " In -> " & dbConnection.ToString)
                End If

                While True

                    'assume format extension represents content and is not secured
                    DB = CMSDALCommon.CreateDatabase(dbConnection)
                    If CanFileBeOpened(DB) Then
                        Return DB
                    End If

                    If dbConnection?.Contains("XLS") Then 'Attempt to use Excel 8.0 driver 
                        DB = CMSDALCommon.CreateDatabase(dbConnection.Replace("XLSX", "XLS"))
                        If CanFileBeOpened(DB) Then
                            Return DB
                        End If
                    End If

                    If dbConnection?.Contains("(Secure)") Then Throw New ApplicationException("Unable to connect to " & dbConnection.Replace(" (Secure)", ""))

                    dbConnection = dbConnection?.Replace("Instance;", "Instance (Secure);")

                End While

            End SyncLock


        Catch ex As ApplicationException
            Throw
        Catch ex As Exception
            Throw
        Finally

            If dbConnection?.Contains("ACCDB") OrElse dbConnection?.Contains("MDB") Then
                Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & " Out -> " & dbConnection.ToString)
            End If

        End Try

    End Function

    Public Shared Function CreateDatabase() As Database

        If String.IsNullOrWhiteSpace(_DefaultDB2Instance) Then
            Return CreateDatabase(Nothing)
        Else
            Return CreateDatabase(_DefaultDB2Instance)
        End If

    End Function

    Public Shared Function CreateDatabase(ByVal dbConnection As String) As Database

        Dim ReplaceUserID As Boolean = True
        Dim DBConnectionA As String()
        Dim ConnectionStringName As String
        Dim CSS As ConnectionStringSettings
        Dim ConnString As String

        Dim ConnectionStringParamsA() As String
        Dim UserName As String = Nothing
        Dim Password As String = Nothing

        Dim DBProviderFactory As DbProviderFactory
        Dim Database As Database

        Try

            ConnectionStringName = CMSDALCommon.DefaultDatabase

            If String.IsNullOrWhiteSpace(dbConnection) Then
                dbConnection = ConnectionStringName.Trim
            End If

            DBConnectionA = dbConnection.Split(New Char() {CChar(";")}, StringSplitOptions.RemoveEmptyEntries)

            If DBConnectionA.Length > 1 Then
                dbConnection = DBConnectionA(0)
            End If
            'Override only works to switch production to QA.
            dbConnection = dbConnection.Replace(" P ", $" {If(_EnvironmentOverride?.Trim.Length > 0, _EnvironmentOverride, "P")} ").Replace(" SQL ", If(_EnvironmentOverride?.Trim.Length > 0, " SQ1 ", " SQL "))

            Debug.Print($"CMSDALCommon.CreateDatabase({dbConnection})")

            CSS = ConfigurationManager.ConnectionStrings(dbConnection)

            If CSS Is Nothing Then
                CSS = ConfigurationManager.ConnectionStrings($"{dbConnection} Database Instance")

                If CSS Is Nothing Then
                    Throw New ApplicationException($"No connection string found for {dbConnection} in Config file")
                Else
                    dbConnection = $"{dbConnection} Database Instance"
                End If
            End If

            ConnString = CSS.ConnectionString.ToString.Replace("DB2PL", $"DB2{If(_EnvironmentOverride?.Trim.Length > 0, _EnvironmentOverride, "P")}L").Replace("SQL", $"SQ{If(_EnvironmentOverride?.Trim.Length > 0, "1", "L")}")

            If DBConnectionA.Length > 1 Then ConnString = ConnString.Replace("$mdb$", DBConnectionA(1)).Replace("$MDB$", DBConnectionA(1))

            If Not CSS.ConnectionString.ToLower.Contains("kerberos") AndAlso (dbConnection.ToLower.Contains("ddtek") OrElse dbConnection.ToLower.Contains("accdb") OrElse dbConnection.ToLower.Contains("mdb") OrElse dbConnection.ToLower.Contains("oledb")) Then 'DB2
                ConnectionStringParamsA = ConnString.Split(New Char() {CChar(";")}, StringSplitOptions.RemoveEmptyEntries)

                For x As Integer = 0 To ConnectionStringParamsA.Length - 1

                    If ConnectionStringParamsA(x).Contains("user id=") Then
                        UserName = ConnectionStringParamsA(x)
                    End If
                    If ConnectionStringParamsA(x).ToLower.Contains("password=") OrElse ConnectionStringParamsA(x).Contains("Database Password=") Then
                        Password = ConnectionStringParamsA(x)
                    End If

                    If Not String.IsNullOrWhiteSpace(UserName) AndAlso Not String.IsNullOrWhiteSpace(Password) Then Exit For

                Next

                If ConfigurationManager.AppSettings("ReplaceUserID") IsNot Nothing Then
                    ReplaceUserID = CBool(ConfigurationManager.AppSettings("ReplaceUserID"))
                End If

                If ReplaceUserID Then
                    If UserName.Trim.Length < 1 Then
                        ConnString &= ";user id=" & UFCWGeneral.ComputerName
                    Else
                        ConnString = ConnString.Replace(UserName, "user id=" & UFCWGeneral.ComputerName)
                    End If
                End If

                If Password.Trim.Length < 1 AndAlso Not dbConnection.ToUpper.Contains("ACCDB") AndAlso Not dbConnection.ToUpper.Contains("MDB") AndAlso Not dbConnection.ToUpper.Contains("OLEDB") Then
                    ConnString &= ";password=" & UFCWCryptor.Password("DB2")
                Else
                    Select Case True
                        Case ((dbConnection.ToUpper.Contains("MDB") OrElse dbConnection.ToUpper.Contains("ACCDB")) AndAlso Password.ToUpper.Contains("MDB")) OrElse (dbConnection.ToUpper.Contains("OLEDB") AndAlso Password.ToUpper.Contains("OLEDB"))
                            ConnString = ConnString.Replace(Password, "Jet OLEDB:Database Password=" & UFCWCryptor.Password("MDB"))
                        Case dbConnection.ToUpper.Contains("ACCDB"), dbConnection.ToUpper.Contains("MDB"), dbConnection.ToUpper.Contains("OLEDB")
                            'do nothing
                        Case Else
                            ConnString = ConnString.Replace(Password, "password=" & UFCWCryptor.Password("DB2"))
                    End Select
                End If

            End If

            DBProviderFactory = DbProviderFactories.GetFactory(CSS.ProviderName)
            Database = New GenericDatabase(ConnString, DBProviderFactory)

            Return Database

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Function

    Public Shared Function ConvertSchemaToDataTable(ByRef tableSchema As DataTable) As DataTable

        Dim ClonedDT As DataTable
        Dim SchemaDT As DataTable

        Dim Column As DataColumn
        Dim SchemaDR As DataRow
        Dim ColumnName As String
        '        Dim Columns As List(Of DataColumn) = New List(Of DataColumn)()

        Try

            If tableSchema IsNot Nothing Then

                ClonedDT = New DataTable
                SchemaDT = tableSchema.Clone

                For Each DR As DataRow In tableSchema.Rows
                    Dim Suffix As String = ""

                    SchemaDR = SchemaDT.NewRow
                    SchemaDR.ItemArray = DR.ItemArray


                    Do While True


                        'SchemaDR("ColumnName") = Regex.Replace(SchemaDR("ColumnName").ToString, "[^0-9a-zA-Z]+", "") ' remove characters that cannot be used in column names
                        SchemaDR("ColumnName") = SchemaDR("ColumnName").ToString.Replace(".", "").Replace("#", "No")

                        ColumnName = SchemaDR("ColumnName").ToString
                        Column = New DataColumn(ColumnName, CType(DR("DataType"), Type)) With {
                            .AllowDBNull = CBool(DR("AllowDBNull")),
                            .AutoIncrement = CBool(DR("IsAutoIncrement"))
                        }
                        '                        Columns.Add(Column)
                        If CInt(SchemaDR("ColumnSize")) > 255 Then SchemaDR("ColumnSize") = 255

                        Try
                            ClonedDT.Columns.Add(Column)
                            Exit Do
                        Catch ex As Exception
                            Suffix = If(Suffix = "", 2, CInt(Suffix) + 1).ToString
                            SchemaDR("ColumnName") = DR("ColumnName").ToString & Suffix
                        End Try

                    Loop

                    SchemaDT.Rows.Add(SchemaDR)

                Next DR

                ClonedDT.TableName = "Data"
                tableSchema = SchemaDT 'replace with modified schema

                Return ClonedDT
            End If

            Return Nothing

        Catch ex As ApplicationException
            Throw
        Catch ex As Exception
            Throw
        Finally
        End Try

    End Function

    Public Shared Function BeginTransaction(ByVal oleProvider As String) As DbTransaction

        Dim DB As Database
        Dim Connection As DbConnection

        Try
            DB = CMSDALCommon.CreateDatabase(oleProvider)
            Connection = DB.CreateConnection()

            Connection.Open()

            Return Connection.BeginTransaction()

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function BeginTransaction() As DbTransaction
        Dim DB As Database
        Dim Connection As DbConnection
        Dim Transaction As DbTransaction

        Try
            DB = CMSDALCommon.CreateDatabase()
            Connection = DB.CreateConnection()

            Connection.Open()
            Transaction = Connection.BeginTransaction()

            Return Transaction

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function BeginTransactionForBatch() As DbTransaction
        Dim DB As Database
        Dim Transaction As DbTransaction

        Try
            DB = CMSDALCommon.CreateDatabase()

            If Batchconnection Is Nothing Then
                Batchconnection = DB.CreateConnection()
                Batchconnection.Open()
            ElseIf Batchconnection.State = ConnectionState.Closed Then
                Batchconnection.Open()
            End If

            Transaction = Batchconnection.BeginTransaction()

            Return Transaction
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Sub CommitTransaction(ByRef transaction As DbTransaction)

        Dim Conn As SqlClient.SqlConnection

        Try

            If transaction IsNot Nothing AndAlso transaction.Connection IsNot Nothing AndAlso transaction.Connection.State <> ConnectionState.Closed Then
                transaction.Commit()
            End If

            Conn = TryCast(transaction.Connection, System.Data.SqlClient.SqlConnection)
            ' in the event of an error the following code is not performed in the finally block because the transaction ticket could be used in a rollback process.
            If Conn IsNot Nothing AndAlso Conn.State <> ConnectionState.Closed Then
                Conn.Close()
            End If

        Catch ex As Exception
            Throw
        Finally
            If transaction IsNot Nothing Then
                transaction.Dispose()
            End If
            transaction = Nothing
        End Try

    End Sub

    Public Shared Sub RollbackTransaction(ByVal transaction As DbTransaction)
        Dim Conn As SqlClient.SqlConnection

        Try

            If transaction IsNot Nothing AndAlso transaction.Connection IsNot Nothing Then

                transaction.Rollback()

            End If

            If transaction IsNot Nothing Then
                Conn = TryCast(transaction.Connection, System.Data.SqlClient.SqlConnection)
                ' in the event of an error the following code is not performed in the finally block because the transaction ticket could be used in a rollback process.
                If Conn IsNot Nothing AndAlso Conn.State <> ConnectionState.Closed Then
                    Conn.Close()
                End If

            End If

        Catch ex As Exception
            Throw
        Finally

            If transaction IsNot Nothing Then
                transaction.Dispose()
            End If
            transaction = Nothing
        End Try
    End Sub

    Public Shared Function ExpandProcedureValues(proceduresToExclude As String) As String()

        Dim ProcedureRanges() As String
        Dim ProcedureRange() As String
        Dim Procedures As New List(Of String)

        Try
            If proceduresToExclude Is Nothing OrElse proceduresToExclude.Trim.Length < 1 Then Return Procedures.ToArray

            ProcedureRanges = proceduresToExclude.Split(","c).[Select](Function(p) p.Trim()).ToArray()

            For x As Integer = 0 To ProcedureRanges.Length - 1
                ProcedureRange = ProcedureRanges(x).Split("-"c)

                Dim ProcedureRangeLow As Integer = CInt(ProcedureRange(0))
                Dim ProcedureRangeHigh As Integer = CInt(If(ProcedureRange.Length = 1, ProcedureRange(0), ProcedureRange(1)))

                For y As Integer = ProcedureRangeLow To ProcedureRangeHigh
                    Procedures.Add(CStr(y))
                Next

            Next

            Return Procedures.ToArray

        Catch ex As Exception
            Throw
        End Try

    End Function

#End Region

End Class

'<Serializable>
'Public NotInheritable Class CloneHelper

'    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

'    Private Sub New()

'    End Sub

'    Public Shared Function DeepCopy(ByVal hashtableToClone As Hashtable, hashtableClone As Hashtable) As Hashtable
'        Dim BeginTime As Date = UFCWGeneral.NowDate

'        Try

'            Parallel.ForEach(hashtableToClone.Cast(Of Object), Sub(item)
'                                                               End Sub)

'            Return hashtableClone

'        Catch ex As Exception
'            Throw
'        Finally
'            'If _TraceSwitch.Enabled Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & Generals.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", MethodBase.GetCurrentMethod().DeclaringType.ToString)
'        End Try
'    End Function

'    Public Shared Function DeepCopy(ByVal tableToClone As DataTable) As DataTable
'        Return DeepCopy(tableToClone, Nothing)
'    End Function

'    Public Shared Function DeepCopy(ByVal cloneDT As DataTable, memoryStreamSize As Integer?) As DataTable

'        If cloneDT Is Nothing Then Return Nothing

'        Dim ClonedTable As DataTable
'        Dim BeginTime As Date = UFCWGeneral.NowDate

'        Try

'            ClonedTable = cloneDT.Clone
'            For Each DC As DataColumn In ClonedTable.Columns
'                If DC.Expression.Length > 0 Then DC.Expression = ""
'            Next

'            ClonedTable.BeginLoadData()
'            ClonedTable.Load(cloneDT.CreateDataReader)
'            ClonedTable.EndLoadData()

'            For Each DC As DataColumn In cloneDT.Columns
'                If DC.Expression.Length > 0 Then ClonedTable.Columns(DC.ColumnName).Expression = DC.Expression
'            Next

'            Return ClonedTable

'        Catch ex As Exception
'            Throw
'        Finally

'            'If _TraceSwitch.Enabled Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & Generals.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", MethodBase.GetCurrentMethod().DeclaringType.ToString)

'            ClonedTable = Nothing

'        End Try
'    End Function

'    Public Shared Function Clone(ByVal objectToClone As Object) As Object

'        Try
'            Return Clone(objectToClone, Nothing)
'        Catch ex As Exception
'            Throw
'        End Try

'    End Function

'    Public Shared Function Clone(ByVal objectToClone As Object, memoryStreamSize As Integer?) As Object

'        Dim BinFormatter As Runtime.Serialization.Formatters.Binary.BinaryFormatter
'        Dim BeginTime As Date = UFCWGeneral.NowDate

'        Try

'            BinFormatter = New Runtime.Serialization.Formatters.Binary.BinaryFormatter(Nothing, New Runtime.Serialization.StreamingContext(Runtime.Serialization.StreamingContextStates.Clone))

'            If memoryStreamSize Is Nothing Then
'                Using MemoryStream As New IO.MemoryStream
'                    BinFormatter.Serialize(MemoryStream, objectToClone)
'                    MemoryStream.Seek(0, SeekOrigin.Begin)

'                    Return BinFormatter.Deserialize(MemoryStream) ', Nothing) unsafedeserialize is slower
'                End Using
'            Else
'                Using MemoryStream As New IO.MemoryStream(CInt(memoryStreamSize))
'                    BinFormatter.Serialize(MemoryStream, objectToClone)
'                    MemoryStream.Seek(0, SeekOrigin.Begin)

'                    Return BinFormatter.Deserialize(MemoryStream) ', Nothing) unsafedeserialize is slower
'                End Using
'            End If

'        Catch ex As Exception
'            Throw
'        Finally

'            'If _TraceSwitch.Enabled Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & Generals.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", MethodBase.GetCurrentMethod().DeclaringType.ToString)

'            BinFormatter = Nothing

'        End Try
'    End Function
'    Public Shared Function DeepCopy(ByVal collectionToClone As CollectionBase) As CollectionBase
'        ' This copies references types by just copying the pointer, so to break any connection back to those object the objects need to be recreated.

'        Dim BeginTime As Date = UFCWGeneral.NowDate
'        Dim ParallelCollectionClone As New BaseCloneHelperCollection

'        Try

'            Parallel.ForEach(DirectCast(collectionToClone, CollectionBase).Cast(Of Object), Sub(item)
'                                                                                                ParallelCollectionClone.Add(item)
'                                                                                            End Sub)

'            Return ParallelCollectionClone

'        Catch ex As Exception
'            Throw
'        Finally
'            'If _TraceSwitch.Enabled Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & Generals.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", MethodBase.GetCurrentMethod().DeclaringType.ToString)
'        End Try
'    End Function

'    Public Shared Function ShallowCopy(ByVal collectionToClone As CollectionBase) As CollectionBase
'        ' This copies references types by just copying the pointer, so to break any connection back to those object the objects need to be recreated.

'        Dim BeginTime As Date = UFCWGeneral.NowDate
'        Dim ParallelCollectionClone As New BaseCloneHelperCollection

'        Try

'            Return ParallelCollectionClone

'        Catch ex As Exception
'            Throw
'        Finally
'            'If _TraceSwitch.Enabled Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & Generals.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", MethodBase.GetCurrentMethod().DeclaringType.ToString)
'        End Try
'    End Function

'End Class

'Public NotInheritable Class BaseCloneHelperCollection
'    Inherits CollectionBase

'    Public Sub New()
'        MyBase.New()
'    End Sub

'    Default Public Property Item(ByVal index As Integer) As Object
'        Get
'            Return Me.List(index)
'        End Get
'        Set(ByVal value As Object)
'            Me.List(index) = value
'        End Set
'    End Property

'    Public Sub Add(ByVal item As Object)
'        Me.List.Add(CloneHelper.Clone(item))
'    End Sub
'End Class

Friend Class FieldInfo
    Public RelationName As String
    Public FieldName As String      ' source table field name
    Public FieldAlias As String     ' destination table field name
    Public Aggregate As String
End Class

Public Class DataSetHelper

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _DS As DataSet
    Private _FieldInfo As ArrayList, _FieldList As String
    Private _GroupByFieldInfo As ArrayList, _GroupByFieldList As String

    Public Sub New(ByVal ds As DataSet)
        _DS = ds
    End Sub

    Public Sub New()
        _DS = Nothing
    End Sub

    Private Sub ParseGroupByFieldList(ByVal fieldList As String)
        '
        ' Parses FieldList into FieldInfo objects and then adds them to the GroupByFieldInfo private member
        '
        ' FieldList syntax: fieldname[ alias]|operatorname(fieldname)[ alias],...
        '
        ' Supported Operators: count,sum,max,min,first,last
        '
        If _GroupByFieldList = fieldList Then Exit Sub
        _GroupByFieldInfo = New ArrayList

        Dim Field As FieldInfo, FieldParts() As String, Fields() As String = fieldList.Split(CChar(","))
        Dim I As Integer

        For I = 0 To Fields.Length - 1
            Field = New FieldInfo
            '
            ' Parse FieldAlias
            '
            FieldParts = Fields(I).Trim().Split(CChar(" "))
            Select Case FieldParts.Length
                Case 1
                    ' To be set at the end of the loop
                Case 2
                    Field.FieldAlias = FieldParts(1)
                Case Else
                    Throw New ArgumentException("Too many spaces in field definition: '" & Fields(I) & "'.")
            End Select
            '
            ' Parse FieldName and Aggregate
            '
            FieldParts = FieldParts(0).Split(CChar("("))
            Select Case FieldParts.Length
                Case 1
                    Field.FieldName = FieldParts(0)
                Case 2
                    Field.Aggregate = FieldParts(0).Trim().ToLower() ' You will do a case-sensitive comparison later.
                    Field.FieldName = FieldParts(1).Trim(" "c, ")"c)
                Case Else
                    Throw New ArgumentException("Invalid field definition: '" & Fields(I) & "'.")
            End Select
            If Field.FieldAlias = "" Then
                If Field.Aggregate = "" Then
                    Field.FieldAlias = Field.FieldName
                Else
                    Field.FieldAlias = Field.Aggregate & "Of" & Field.FieldName
                End If
            End If
            _GroupByFieldInfo.Add(Field)
        Next
        _GroupByFieldList = fieldList
    End Sub

    Private Sub ParseFieldList(ByVal fieldList As String, Optional ByVal allowRelation As Boolean = False)
        '
        ' Parses FieldList into FieldInfo objects and then adds them to the m_FieldInfo private member
        '
        ' FieldList syntax: [relationname.]fieldname[ alias],...
        '
        If _FieldList = fieldList Then Exit Sub

        _FieldInfo = New ArrayList
        _FieldList = fieldList

        Dim Field As FieldInfo, FieldParts() As String, Fields() As String = fieldList.Split(CChar(","))
        Dim I As Integer

        For I = 0 To Fields.Length - 1
            Field = New FieldInfo
            '
            ' Parse FieldAlias
            '
            FieldParts = Fields(I).Trim().Split(CChar(" "))
            Select Case FieldParts.Length
                Case 1
                    ' To be set at the end of the loop
                Case 2
                    Field.FieldAlias = FieldParts(1)
                Case Else
                    Throw New ArgumentException("Too many spaces in field definition: '" & Fields(I) & "'.")
            End Select
            '
            ' Parse FieldName and RelationName
            '
            FieldParts = FieldParts(0).Split(CChar("."))
            Select Case FieldParts.Length
                Case 1
                    Field.FieldName = FieldParts(0)
                Case 2
                    If Not allowRelation Then _
                        Throw New ArgumentException("Relation specifiers not allowed in field list: '" & Fields(I) & "'.")
                    Field.RelationName = FieldParts(0).Trim()
                    Field.FieldName = FieldParts(1).Trim()
                Case Else
                    Throw New ArgumentException("Invalid field definition: '" & Fields(I) & "'.")
            End Select
            If Field.FieldAlias = "" Then Field.FieldAlias = Field.FieldName
            _FieldInfo.Add(Field)
        Next
    End Sub

    Public Sub InsertGroupByInto(ByVal destDT As DataTable, ByVal sourceDT As DataTable, ByVal fieldList As String)
        InsertGroupByInto(destDT, sourceDT, fieldList, "", "", False)
    End Sub

    Public Sub InsertGroupByInto(ByVal destDT As DataTable, ByVal sourceDT As DataTable, ByVal fieldList As String, ByVal rowFilter As String)
        InsertGroupByInto(destDT, sourceDT, fieldList, rowFilter, "", False)
    End Sub

    Public Sub InsertGroupByInto(ByVal destDT As DataTable, ByVal sourceDT As DataTable, ByVal fieldList As String, ByVal rowFilter As String, ByVal groupBy As String)
        InsertGroupByInto(destDT, sourceDT, fieldList, rowFilter, groupBy, False)
    End Sub

    Public Sub InsertGroupByInto(ByVal destDT As DataTable, ByVal sourceDT As DataTable, ByVal fieldList As String, ByVal rowFilter As String, ByVal groupBy As String, ByVal rollup As Boolean)
        '
        ' Copies the selected rows and columns from SourceTable and inserts them into DestTable
        ' FieldList has same format as CreateGroupByTable
        '

        Dim Field As FieldInfo
        Dim Rows() As DataRow
        Dim SourceRow, LastSourceRow As DataRow, SameRow As Boolean, I As Integer, J As Integer, K As Integer
        Dim DestRows(_FieldInfo.Count) As DataRow, RowCount(_FieldInfo.Count) As Integer

        Try
            ParseGroupByFieldList(fieldList)  ' parse field list
            ParseFieldList(groupBy)           ' parse field names to Group By into an arraylist

            Rows = sourceDT.Select(rowFilter, groupBy)
            '
            ' Initialize Grand total row
            '
            DestRows(0) = destDT.NewRow()
            '
            ' Process source table rows
            '
            For Each SourceRow In Rows
                '
                ' Determine whether we've hit a control break
                '
                SameRow = False
                If LastSourceRow IsNot Nothing Then
                    SameRow = True
                    For I = 0 To _FieldInfo.Count - 1 ' fields to Group By
                        Field = CType(_FieldInfo(I), FieldInfo)
                        If UFCWGeneral.ColumnEqual(LastSourceRow(Field.FieldName), SourceRow(Field.FieldName)) = False Then
                            SameRow = False
                            Exit For
                        End If
                    Next I
                    '
                    ' Add previous totals to the destination table
                    '
                    If Not SameRow Then
                        For J = _FieldInfo.Count To I + 1 Step -1
                            '
                            ' Make NULL the key values for levels that have been rolled up
                            '
                            For K = _FieldInfo.Count - 1 To J Step -1
                                Field = LocateFieldInfoByName(_GroupByFieldInfo, CType(_FieldInfo(K), FieldInfo).FieldName)
                                If Field IsNot Nothing Then   ' Group By field does not have to be in field list
                                    DestRows(J)(Field.FieldAlias) = DBNull.Value
                                End If
                            Next K
                            '
                            ' Make NULL any non-aggregate, non-group-by fields in anything other than
                            ' the lowest level.
                            '
                            If J <> _FieldInfo.Count Then
                                For Each Field In _GroupByFieldInfo
                                    If Field.Aggregate <> "" Then Exit For
                                    If LocateFieldInfoByName(_FieldInfo, Field.FieldName) Is Nothing Then
                                        DestRows(J)(Field.FieldAlias) = DBNull.Value
                                    End If
                                Next
                            End If
                            '
                            ' Add row
                            '
                            destDT.Rows.Add(DestRows(J))
                            If rollup = False Then Exit For ' only add most child row if not doing a roll-up
                        Next J
                    End If
                End If
                '
                ' create new destination rows
                ' Value of I comes from previous If block
                '
                If Not SameRow Then
                    For J = _FieldInfo.Count To I + 1 Step -1
                        DestRows(J) = destDT.NewRow()
                        RowCount(J) = 0
                        If rollup = False Then Exit For
                    Next J
                End If
                For J = 0 To _FieldInfo.Count
                    RowCount(J) += 1
                    For Each Field In _GroupByFieldInfo
                        Select Case Field.Aggregate  ' this test is case-sensitive - made lower-case by Build_GroupByFiledInfo
                            Case ""    ' implicit Last
                                DestRows(J)(Field.FieldAlias) = SourceRow(Field.FieldName)
                            Case "last"
                                DestRows(J)(Field.FieldAlias) = SourceRow(Field.FieldName)
                            Case "first"
                                If RowCount(J) = 1 Then DestRows(J)(Field.FieldAlias) = SourceRow(Field.FieldName)
                            Case "count"
                                DestRows(J)(Field.FieldAlias) = RowCount(J)
                            Case "sum"
                                DestRows(J)(Field.FieldAlias) = UFCWGeneral.Add(DestRows(J)(Field.FieldAlias), SourceRow(Field.FieldName))
                            Case "max"
                                DestRows(J)(Field.FieldAlias) = UFCWGeneral.Max(DestRows(J)(Field.FieldAlias), SourceRow(Field.FieldName))
                            Case "min"
                                If RowCount(J) = 1 Then
                                    DestRows(J)(Field.FieldAlias) = SourceRow(Field.FieldName)  ' so we get by initial NULL
                                Else
                                    DestRows(J)(Field.FieldAlias) = UFCWGeneral.Min(DestRows(J)(Field.FieldAlias), SourceRow(Field.FieldName))
                                End If
                        End Select
                    Next
                Next J
                LastSourceRow = SourceRow
            Next

            If Rows.Length > 0 Then
                '
                ' Make NULL the key values for levels that have been rolled up
                '
                For J = _FieldInfo.Count To 0 Step -1
                    For K = _FieldInfo.Count - 1 To J Step -1
                        Field = LocateFieldInfoByName(_GroupByFieldInfo, CType(_FieldInfo(K), FieldInfo).FieldName)
                        If Not (Field Is Nothing) Then  ' Group By field does not have to be in field list
                            DestRows(J)(Field.FieldAlias) = DBNull.Value
                        End If
                    Next K
                    '
                    ' Make NULL any non-aggregate, non-group-by fields in anything other than
                    ' the lowest level.
                    '
                    If J <> _FieldInfo.Count Then
                        For Each Field In _GroupByFieldInfo
                            If Field.Aggregate <> "" Then Exit For
                            If LocateFieldInfoByName(_FieldInfo, Field.FieldName) Is Nothing Then
                                DestRows(J)(Field.FieldAlias) = DBNull.Value
                            End If
                        Next
                    End If
                    '
                    ' Add row
                    '
                    destDT.Rows.Add(DestRows(J))
                    If rollup = False Then Exit For
                Next J
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Shared Function LocateFieldInfoByName(ByVal fieldList As ArrayList, ByVal name As String) As FieldInfo
        '
        ' Looks up a FieldInfo record based on FieldName
        '
        Dim Field As FieldInfo
        For Each Field In fieldList
            If Field.FieldName = name Then Return Field
        Next
    End Function

    Public Function CreateGroupByTable(ByVal tableName As String, ByVal sourceDT As DataTable, ByVal fieldList As String) As DataTable
        '
        ' Creates a table based on aggregates of fields of another table
        '
        ' RowFilter affects rows before the GroupBy operation. No HAVING-type support
        ' although this can be emulated by later filtering of the resultant table.
        '
        ' FieldList syntax: fieldname[ alias]|aggregatefunction(fieldname)[ alias], ...
        '
        Dim DT As DataTable
        Dim Field As FieldInfo, DC As DataColumn

        Try

            If fieldList = "" Then
                Throw New ArgumentException("You must specify at least one field in the field list.")
                ' Return CreateTable(TableName, SourceTable)
            Else
                DT = New DataTable(tableName)

                ParseGroupByFieldList(fieldList)

                For Each Field In _GroupByFieldInfo
                    DC = sourceDT.Columns(Field.FieldName)
                    If Field.Aggregate = "" Then
                        DT.Columns.Add(Field.FieldAlias, DC.DataType, DC.Expression)
                    Else
                        DT.Columns.Add(Field.FieldAlias, DC.DataType)
                    End If
                Next

                If _DS IsNot Nothing Then _DS.Tables.Add(DT)

                Return DT

            End If

        Catch ex As Exception
            Throw
        Finally
            DT = Nothing
        End Try
    End Function

    Public Function SelectGroupByInto(ByVal tableName As String, ByVal sourceTable As DataTable, ByVal fieldList As String) As DataTable
        Return SelectGroupByInto(tableName, sourceTable, fieldList, "", "", False)

    End Function

    Public Function SelectGroupByInto(ByVal tableName As String, ByVal sourceTable As DataTable, ByVal fieldList As String, ByVal rowFilter As String, ByVal groupBy As String, ByVal rollup As Boolean) As DataTable
        '
        ' Selects data from one DataTable to another and performs various aggregate functions
        ' along the way. See InsertGroupByInto and ParseGroupByFieldList for supported aggregate functions.
        '
        Dim DT As DataTable
        Try

            DT = CreateGroupByTable(tableName, sourceTable, fieldList)
            InsertGroupByInto(DT, sourceTable, fieldList, rowFilter, groupBy, rollup)

            Return DT

        Catch ex As Exception
            Throw
        Finally
            If DT IsNot Nothing Then DT.Dispose()

        End Try

    End Function

End Class
