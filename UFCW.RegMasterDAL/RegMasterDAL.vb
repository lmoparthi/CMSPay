Option Strict On

Imports System.Configuration
Imports System.Data.Common
Imports System.IO
Imports System.Security
Imports System.Security.Principal
Imports System.Text
Imports System.Windows.Forms
Imports System.Xml
Imports System.Xml.Serialization
Imports DDTek.DB2
Imports Microsoft.Practices.EnterpriseLibrary.Common.Configuration
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Configuration
Imports Microsoft.Practices.EnterpriseLibrary.Logging

''' -----------------------------------------------------------------------------
''' Project	 : RegMasterDAL
''' Class	 : RegMasterDAL
''' 
''' -----------------------------------------------------------------------------
''' <summary>
''' Data Access Layer for the RegMaster UI
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[Sri Bandi]	3/24/2010	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Class RegMasterDAL

    Private Shared _RelationshipValueslockObj As Object = New Object
    Private Shared _RiskValueslockObj As Object = New Object
    Private Shared _AddressTypeslockObj As Object = New Object
    Private Shared _RemarkslockObj As Object = New Object
    Private Shared _LifeEventValueslockObj As Object = New Object
    Private Shared _MemplanlockObj As Object = New Object
    Private Shared _RegAlertTermValueslockObj As Object = New Object
    Private Shared _RegAlertReasonValueslockObj As Object = New Object
    Private Shared _A2countoverridereasonslockObj As Object = New Object
    Private Shared _EligspecialAcctValueslockObj As Object = New Object
    Private Shared _MedicalCoverageValueslockObj As Object = New Object
    Private Shared _DentalCoverageValueslockObj As Object = New Object
    Private Shared _LocalValueslockObj As Object = New Object
    Private Shared _RetireePlanlockObj As Object = New Object
    Private Shared _InvalidMemtypeMedlockObj As Object = New Object
    Private Shared _IneligibleforSpecialAcctslockObj As Object = New Object
    Private Shared _EligMetalDesclockObj As Object = New Object
    Private Shared _UHCHMONetworklockObj As Object = New Object
    Private Shared _CobraQElockObj As Object = New Object
    Private Shared _CobraRateslockObj As Object = New Object
    Private Shared _PhoneTypeslockObj As Object = New Object
    Private Shared _StateslockObj As Object = New Object

    Private Shared _RelationshipValuesDS As DataSet
    Private Shared _RiskValuesDS As DataSet
    Private Shared _AddressTypesDS As DataSet
    Private Shared _PhoneTypesDS As DataSet
    Private Shared _StatesDS As DataSet
    Private Shared _RemarksDS As DataSet
    Private Shared _LifeEventsDS As DataSet
    Private Shared _LifeEventsAppliesDS As DataSet
    Private Shared _MemPlansDS As DataSet
    Private Shared _RegAlertTermValuesDS As DataSet
    Private Shared _RegAlertReasonValuesDS As DataSet
    Private Shared _A2CountOverrideReasonsDS As DataSet
    Private Shared _EligSpecialAcctValuesDS As DataSet
    Private Shared _MedicalCoverageValuesDS As DataSet
    Private Shared _DentalCoverageValuesDS As DataSet
    Private Shared _LocalValuesDS As DataSet
    Private Shared _RetireePlanDS As DataSet
    Private Shared _InvalidMemtypeMedplanDS As DataSet
    Private Shared _IneligibleforSpecialAcctsDS As DataSet
    Private Shared _EligMetalDescDS As DataSet
    Private Shared _HMONetworksDS As DataSet
    Private Shared _CobraQEDS As DataSet
    Private Shared _CobraRatesDS As DataSet

    Private Shared _LogEntry As New LogEntry

    Private Shared _WindowsUserID As WindowsIdentity = UFCWGeneral.WindowsUserID
    Private Shared _WindowsPrincipalForID As WindowsPrincipal = UFCWGeneral.WindowsPrincipalForID
    Private Shared _ComputerName As String = SystemInformation.ComputerName
    Private Shared _DomainUser As String = SystemInformation.UserName

    Public Const _DB2databaseName As String = "FDBEL"
    Public Const _SQLdatabaseName As String = "dbo"

    Private Shared _DefaultDB2Instance As String

    Private _DS As DataSet
    Private _ProviderID As Integer
    Private Shared _DBChanges As Boolean = False
    Private Shared _EligChanges As Boolean = False

    '*****************************************************************************************************************************
    '********************************************** PHI Authorization Record Processing ******************************************
    Private Shared _PermissionTypesDS As DataSet
    Private Shared _PermissionTypeslockObj As Object = New Object
    Private Shared _FamilyMembersDS As DataSet
    Private Shared _FamilyMemberslockObj As Object = New Object
    Private Shared _PHIPermissionsDS As DataSet
    Private Shared _PermissionsGridlockObj As Object = New Object
    '*****************************************************************************************************************************

    Private Shared _GlobalEligPeriod As Date?

    Public Shared Property DefaultDB2Instance() As String
        Get
            Return _DefaultDB2Instance
        End Get
        Set(ByVal value As String)
            _DefaultDB2Instance = value
        End Set
    End Property

    Public Shared ReadOnly Property NowDate As Date
        Get
            Return Now.AddDays(CInt(If(System.Configuration.ConfigurationManager.AppSettings("AddDaysToCurrentDate") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("AddDaysToCurrentDate")))))
        End Get
    End Property

    Public WriteOnly Property Dataset() As DataSet
        Set(ByVal value As DataSet)
            _DS = value
        End Set
    End Property

    Public Shared Property MadeDBChanges() As Boolean
        Get
            Return _DBChanges
        End Get
        Set(ByVal Value As Boolean)
            _DBChanges = Value
        End Set
    End Property
    Public Shared Property MadeEligibilityChanges() As Boolean
        Get
            Return _EligChanges
        End Get
        Set(ByVal Value As Boolean)
            _EligChanges = Value
        End Set
    End Property

#Region "Constructor"
    Shared Sub New()

        Try

            If ConfigurationManager.AppSettings("EnableDDTEKLogging") IsNot Nothing AndAlso CDbl(System.Configuration.ConfigurationManager.AppSettings("EnableDDTEKLogging")) = 1 Then
                DB2Trace.TraceFile = CMSDALLog.LogDirectory(New String() {"#"}, New String() {If(CType(ConfigurationManager.GetSection("dataConfiguration"), Microsoft.Practices.EnterpriseLibrary.Data.Configuration.DatabaseSettings).DefaultDatabase.Contains(" P "), "Prod", "Test")}) & "DDTEK" & My.Application.Info.ProductName & UFCWGeneral.WindowsUserID.Name.Replace("\", "_").ToString & String.Format("{0000}", NowDate.Year) & String.Format("{00}", NowDate.Month) & String.Format("{0:00}", NowDate.Day) & If(CBool(If(System.Configuration.ConfigurationManager.AppSettings("IncludeTimeinTraceName") Is Nothing, False, CBool(System.Configuration.ConfigurationManager.AppSettings("IncludeTimeinTraceName")))), String.Format("{0:00}", NowDate.Hour), "").ToString & ".txt"
                DB2Trace.RecreateTrace = CInt(If(System.Configuration.ConfigurationManager.AppSettings("RecreateTrace") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("RecreateTrace"))))
                DB2Trace.EnableTrace = CInt(If(System.Configuration.ConfigurationManager.AppSettings("StartTraceImmediately") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("StartTraceImmediately"))))
            Else
                DB2Trace.EnableTrace = 0
            End If

        Catch ex As Exception

            Throw

        End Try

    End Sub
    Private Sub New()

        Try

            Dim currentDomain As AppDomain = AppDomain.CurrentDomain

            If System.Configuration.ConfigurationManager.AppSettings("EnableDDTEKLogging") IsNot Nothing AndAlso CDbl(System.Configuration.ConfigurationManager.AppSettings("EnableDDTEKLogging")) = 1 Then
                If System.Configuration.ConfigurationManager.AppSettings("LogDirectory").Length > 0 Then
                    Dim directoryInfo As New DirectoryInfo(Path.GetDirectoryName(System.Configuration.ConfigurationManager.AppSettings("LogDirectory")))

                    If directoryInfo.Exists = False Then
                        directoryInfo.Create()
                    End If
                End If

                DB2Trace.TraceFile = System.Configuration.ConfigurationManager.AppSettings("LogDirectory") & "\DDTEK" & My.Application.Info.ProductName & _WindowsUserID.Name.Replace("\", "_").ToString & String.Format("{0000}", NowDate.Year) & String.Format("{00}", NowDate.Month) & String.Format("{0:00}", NowDate.Day) & If(CBool(If(System.Configuration.ConfigurationManager.AppSettings("IncludeTimeinTraceName") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("IncludeTimeinTraceName")))), String.Format("{0:00}", NowDate.Hour), "").ToString & ".txt"
                DB2Trace.RecreateTrace = CInt(If(IsNothing(System.Configuration.ConfigurationManager.AppSettings("RecreateTrace")), 0, CInt(System.Configuration.ConfigurationManager.AppSettings("RecreateTrace"))))
                DB2Trace.EnableTrace = CInt(If(IsNothing(System.Configuration.ConfigurationManager.AppSettings("StartTraceImmediately")), 0, CInt(System.Configuration.ConfigurationManager.AppSettings("StartTraceImmediately"))))
            Else
                DB2Trace.EnableTrace = 0
            End If

        Catch ex As Exception

            Throw

        End Try

    End Sub
#End Region

#Region "Properties"

    'Public Shared Property UserName() As String
    '    Get
    '        Dim DB  As Database
    '        DB = CMSDALCommon.CreateDatabase()
    '        Return DB.ConnectionString.Substring(DB.ConnectionString.ToLower.IndexOf("user id") + 8, DB.ConnectionString.IndexOf(";"c, DB.ConnectionString.ToLower.IndexOf("user id")) - DB.ConnectionString.ToLower.IndexOf("user id") - 8)
    '    End Get
    '    Set(ByVal value As String)
    '        ''Dim dbSettings As DatabaseSettings = ConfigurationManager.GetConfiguration("dataConfiguration")
    '        ''dbSettings.ConnectionStrings("DDTek Connection String").Parameters("user id").Value = Value.ToUpper
    '    End Set
    'End Property

    'Public Shared ReadOnly Property GetDatabaseName(Optional ByVal Instance As String = Nothing) As String
    '    Get

    '        Dim settings As DatabaseSettings
    '        Dim ConnectionStringName As String
    '        Dim CSS As ConnectionStringSettings
    '        Dim cn() As String
    '        Dim ConnString As String

    '        Try

    '            settings = DatabaseSettings.GetDatabaseSettings(New SystemConfigurationSource())
    '            ConnectionStringName = settings.DefaultDatabase

    '            If Instance Is Nothing OrElse Instance.Trim.Length = 0 Then
    '                Instance = ConnectionStringName
    '            End If

    '            CSS = ConfigurationManager.ConnectionStrings(Instance)

    '            ConnString = CSS.ConnectionString.ToString

    '            cn = ConnString.Split(New Char() {CChar(";")})

    '            If ConnString.ToUpper.Contains("DB2") Then
    '                For x As Integer = 0 To cn.Length - 1
    '                    If cn(x).ToLower.Contains("database=") Then
    '                        Return cn(x).ToLower.Replace("database=", "").ToString.ToUpper.Trim
    '                    End If
    '                Next
    '            Else  'use sql database referenced in connection string
    '                For x As Integer = 0 To cn.Length - 1
    '                    If cn(x).Contains("Initial Catalog=") Then
    '                        Return cn(x).Replace("Initial Catalog=", "").ToString.Trim & ".dbo"
    '                    End If
    '                Next
    '            End If

    '            Return ""

    '        Catch ex As Exception

    '            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '            If (Rethrow) Then
    '                Throw
    '            End If
    '            Return ""
    '        End Try

    '    End Get

    'End Property

    'Public Shared ReadOnly Property GetServerName() As String
    '    Get
    '        If _DefaultDB2Instance IsNot Nothing AndAlso _DefaultDB2Instance.Trim.Length > 0 Then
    '            Return GetServerName(_DefaultDB2Instance)
    '        Else
    '            Return GetServerName(Nothing)
    '        End If
    '    End Get
    'End Property

    'Public Shared ReadOnly Property GetServerName(ByVal Instance As String) As String
    '    Get
    '        Dim DB  As Database
    '        Dim ServerInfo() As String
    '        Dim Server As String = ""

    '        Try

    '            If Instance Is Nothing Then
    '                DB = CMSDALCommon.CreateDatabase()
    '            Else
    '                DB = CMSDALCommon.CreateDatabase(Instance)
    '            End If

    '            ServerInfo = DB.ConnectionString.Split(CChar(";"))

    '            For cnt As Integer = 0 To UBound(ServerInfo, 1)
    '                If ServerInfo(cnt).ToLower.StartsWith("server=") OrElse
    '                        ServerInfo(cnt).ToLower.StartsWith("host=") Then
    '                    Server = Right(ServerInfo(cnt), ServerInfo(cnt).Length - InStr(ServerInfo(cnt), "="))
    '                    Exit For
    '                End If
    '            Next

    '            For cnt As Integer = 0 To UBound(ServerInfo, 1)
    '                If ServerInfo(cnt).ToLower.StartsWith("port=") Then
    '                    Server += ";" & ServerInfo(cnt)
    '                    Exit For
    '                End If
    '            Next

    '            Return Server

    '        Catch ex As Exception

    '            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '            If (Rethrow) Then
    '                Throw
    '            End If
    '            Return ""
    '        Finally

    '            DB = Nothing
    '        End Try

    '    End Get
    'End Property
    'Public Shared ReadOnly Property GetServerAlias(Optional ByVal Instance As String = Nothing) As String
    '    Get
    '        Dim ServerInfo() As String

    '        Dim settings As DatabaseSettings = DatabaseSettings.GetDatabaseSettings(New SystemConfigurationSource())
    '        Dim ConnectionStringName As String = settings.DefaultDatabase
    '        Try
    '            If Instance Is Nothing OrElse Instance.Trim.Length = 0 Then
    '                Instance = ConnectionStringName
    '            End If

    '            Dim CSS As ConnectionStringSettings = ConfigurationManager.ConnectionStrings(Instance)

    '            Dim ConnString As String = CSS.ConnectionString.ToString

    '            ServerInfo = ConnString.Split(";"c)

    '            For cnt As Integer = 0 To ServerInfo.Length - 1
    '                If ServerInfo(cnt).ToString.ToLower.Contains("database=") = True Then
    '                    Return ServerInfo(cnt).ToString.ToLower.Replace("database=", "").ToUpper
    '                    Exit For
    '                End If
    '            Next
    '        Catch ex As Exception

    '            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '            If (rethrow) Then
    '                Throw
    '            End If
    '            Return ""
    '        Finally
    '        End Try
    '    End Get
    'End Property
    'Public Shared ReadOnly Property GetDisplayConnectionString(ByVal Instance As String) As String
    '    Get
    '        Dim DB  As Database
    '        Dim ServerInfo() As String
    '        Dim Server As String = ""

    '        If Instance = Nothing Then
    '            DB = CMSDALCommon.CreateDatabase()
    '        Else
    '            DB = CMSDALCommon.CreateDatabase(Instance)
    '        End If

    '        ServerInfo = DB.ConnectionString.Split(CChar(";"))

    '        For cnt As Integer = 0 To UBound(ServerInfo, 1)
    '            If ServerInfo(cnt).ToLower.StartsWith("database=") OrElse
    '                    ServerInfo(cnt).ToLower.StartsWith("host=") Then
    '                Server = ServerInfo(cnt)
    '                Exit For
    '            End If
    '        Next
    '        For cnt As Integer = 0 To UBound(ServerInfo, 1)
    '            If ServerInfo(cnt).ToLower.StartsWith("port=") Then
    '                Server += ";" & ServerInfo(cnt)
    '                Exit For
    '            End If
    '        Next
    '        Return Server

    '        DB = Nothing
    '    End Get
    'End Property

    Public Shared ReadOnly Property GlobalEligPeriod As Date
        Get
            If _GlobalEligPeriod Is Nothing Then
                RetrieveEligibilityPeriod()
            End If
            Return CDate(_GlobalEligPeriod)
        End Get
    End Property

#End Region

#Region "Cached SQL"

    '******************************************************************************************************************************************************************************
    'FOR: PHI Authorization Control
    '******************************************************************************************************************************************************************************
    'BOOKMARK: 005b - RetrievePermissionsGrid
    Public Shared Function RetreivePHIPermissions(ByVal familyID As Integer, ByVal relationID As Short, ByRef firstTime As Boolean) As DataTable

        If _PHIPermissionsDS IsNot Nothing Then
            firstTime = True
        End If

        _PHIPermissionsDS = RetreivePHIPermissions(familyID, relationID)
        If _PHIPermissionsDS Is Nothing OrElse _PHIPermissionsDS.Tables.Count < 1 Then
            Return Nothing
        Else
            _PHIPermissionsDS.Tables(0).TableName = "REG_PHIAUTH"
        End If

        Return _PHIPermissionsDS.Tables("REG_PHIAUTH")

    End Function


    'BOOKMARK: 005c - LoadPermissionsGrid - RegMasterDAL.vb
    Public Shared Function RetreivePHIPermissions(ByVal familyID As Integer, ByVal relationID As Short) As DataSet
        SyncLock (_PermissionTypeslockObj)
            Dim UniqueThreadIdentifier As String = RegMasterDAL.GetUniqueKey()
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_REG_PERMISSIONS"

            Try
                'Dim WhereClause As String = " Where (FAMILY_ID=" & FamilyID.ToString & ") AND (RELATION_ID = " & RelationID.ToString & ")"
                DB = CMSDALCommon.CreateDatabase()
                '
                DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
                DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
                DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)

                Return DB.ExecuteDataSet(DBCommandWrapper)

                Return Nothing

            Catch ex As Exception
                Throw
            Finally

            End Try
        End SyncLock

    End Function

    'BOOKMARK: 005h - RetrievePermissionsGridGrantee - RegMasterDAL.vb
    Public Shared Function RetrievePHIPermissionsGrantee(ByVal familyID As Integer, ByVal relationID As Short, ByRef firstTime As Boolean) As DataTable

        If _PHIPermissionsDS IsNot Nothing Then
            firstTime = True
        End If

        _PHIPermissionsDS = RetrievePHIPermissionsGrantee(familyID, relationID)
        If _PHIPermissionsDS Is Nothing OrElse _PHIPermissionsDS.Tables.Count < 1 Then
            Return Nothing
        Else
            _PHIPermissionsDS.Tables(0).TableName = "REG_PHIAUTH"
            Return _PHIPermissionsDS.Tables("REG_PHIAUTH")
        End If

    End Function


    'BOOKMARK: 005d - LoadPermissionsGridGrantee - RegMasterDAL.vb
    Public Shared Function RetrievePHIPermissionsGrantee(ByVal familyID As Integer, ByVal relationID As Short) As DataSet
        SyncLock (_PermissionTypeslockObj)
            Dim UniqueThreadIdentifier As String = RegMasterDAL.GetUniqueKey()
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_REG_PERMISSIONS_GRANTEE"

            Try
                'Dim WhereClause As String = " Where (FAMILY_ID=" & FamilyID.ToString & ") AND (RELATION_ID = " & RelationID.ToString & ")"
                DB = CMSDALCommon.CreateDatabase()
                '
                DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
                DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
                DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)

                Return DB.ExecuteDataSet(DBCommandWrapper)

            Catch ex As Exception
                Throw
            Finally

            End Try
        End SyncLock

    End Function

    'BOOKMARK: 002e - RetrievePermissionTypes
    Public Shared Function RetrievePermissionTypes() As DataTable
        Dim SQLCall As String
        Dim PermissionTypeDT As DataTable

        SyncLock (_PermissionTypeslockObj)
            Try

                SQLCall = "SELECT APPLICATION_TYPE, NAME FROM FDBEL.REG_APPLICATION_TYPE "
                PermissionTypeDT = CMSDALFDBMD.RUNIMMEDIATESELECT(SQLCall)

                Return PermissionTypeDT

            Catch ex As Exception
                Throw

            End Try
        End SyncLock

    End Function

    Public Shared Function RetrieveRelationshipValues() As DataTable

        SyncLock (_RelationshipValueslockObj)

            If _RelationshipValuesDS Is Nothing Then
                _RelationshipValuesDS = LoadRelationshipValues()
            End If

            Dim DT As New DataTable("RELATIONSHIP_VALUES")
            If _RelationshipValuesDS.Tables.Count > 0 Then
                DT = _RelationshipValuesDS.Tables(0)
            End If

            Return DT
        End SyncLock

    End Function
    Private Shared Function LoadRelationshipValues(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBEL.RETRIEVE_RELATIONSHIP_VALUES" & ".xml"
        Dim UniqueThreadIdentifier As String = GetUniqueKey()
        Dim FStream As FileStream
        Dim XMLSerial As XmlSerializer

        Try

            Dim XMLDS As DataSet = XMLHandler.ToandFromDataset(UniqueThreadIdentifier, "FDBEL.RELATIONSHIP_VALUES", "LASTUPDT", "FDBEL.RETRIEVE_RELATIONSHIP_VALUES")
            If XMLDS.Tables.Count = 0 Then
                Dim DB As Database = CMSDALCommon.CreateDatabase()
                Dim DBCommandWrapper As DbCommand
                Dim SQLCall As String = "FDBEL.RETRIEVE_RELATIONSHIP_VALUES"
                DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                If XMLDS Is Nothing Then
                    If transaction Is Nothing Then
                        XMLDS = DB.ExecuteDataSet(DBCommandWrapper)
                    Else
                        XMLDS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                    End If
                Else
                    If transaction Is Nothing Then
                        DB.LoadDataSet(DBCommandWrapper, XMLDS, "RELATIONSHIP_VALUES")
                    Else
                        DB.LoadDataSet(DBCommandWrapper, XMLDS, "RELATIONSHIP_VALUES", transaction)
                    End If
                End If
                _RelationshipValuesDS = XMLDS

                FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)
                XMLSerial = New XmlSerializer(XMLDS.GetType())
                XMLSerial.Serialize(FStream, XMLDS)
                FStream.Close()
                File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
            Else
                _RelationshipValuesDS = XMLDS
            End If

            If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                ds.Merge(_RelationshipValuesDS)
                Return ds
            Else
                Return _RelationshipValuesDS
            End If

        Catch ex As Exception
            Throw
        Finally

            If FStream IsNot Nothing Then
                FStream.Close()
                FStream.Dispose()
            End If

            FStream = Nothing

        End Try

    End Function

    Public Shared Function RetrieveRiskValues() As DataTable
        Try

            SyncLock (_RiskValueslockObj)
                If _RiskValuesDS Is Nothing Then
                    _RiskValuesDS = LoadRiskValues()
                End If

                Dim DT As New DataTable("RISK_VALUES")
                If _RiskValuesDS.Tables.Count > 0 Then
                    DT = _RiskValuesDS.Tables(0)
                End If

                Return DT

            End SyncLock

        Catch ex As Exception
            Throw
        End Try

    End Function
    Private Shared Function LoadRiskValues(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBEL.RETRIEVE_RISK_VALUES" & ".xml"
        Dim UniqueThreadIdentifier As String = GetUniqueKey()
        Dim XMLDS As DataSet
        Dim FStream As FileStream
        Dim XMLSerial As XmlSerializer

        Try

            XMLDS = XMLHandler.ToandFromDataset(UniqueThreadIdentifier, "FDBEL.RISK_VALUES", "LASTUPDT", "FDBEL.RETRIEVE_RISK_VALUES")
            If XMLDS.Tables.Count = 0 Then
                Dim DB As Database = CMSDALCommon.CreateDatabase()
                Dim DBCommandWrapper As DbCommand
                Dim SQLCall As String = "FDBEL.RETRIEVE_RISK_VALUES"
                DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
                If XMLDS Is Nothing Then
                    If transaction Is Nothing Then
                        XMLDS = DB.ExecuteDataSet(DBCommandWrapper)
                    Else
                        XMLDS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                    End If
                Else
                    If transaction Is Nothing Then
                        DB.LoadDataSet(DBCommandWrapper, XMLDS, "RISK_VALUES")
                    Else
                        DB.LoadDataSet(DBCommandWrapper, XMLDS, "RISK_VALUES", transaction)
                    End If
                End If

                _RiskValuesDS = XMLDS

                FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)
                XMLSerial = New XmlSerializer(XMLDS.GetType())
                XMLSerial.Serialize(FStream, XMLDS)
                FStream.Close()
                File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
            Else
                _RiskValuesDS = XMLDS
            End If

            If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                ds.Merge(_RiskValuesDS)
                Return ds
            Else
                Return _RiskValuesDS
            End If

        Catch ex As Exception
            Throw
        Finally

            If FStream IsNot Nothing Then
                FStream.Close()
                FStream.Dispose()
            End If

            FStream = Nothing


        End Try

    End Function

    Public Shared Function RetrieveAddressTypes() As DataTable

        Dim DT As New DataTable

        Try

            If _AddressTypesDS Is Nothing OrElse _AddressTypesDS.Tables.Count < 1 OrElse _AddressTypesDS.Tables(0).Rows.Count < 1 Then
                _AddressTypesDS = LoadAddressTypes()
            End If

            DT = New DataTable("REG_ADDRESS_TYPE")

            If _AddressTypesDS.Tables.Count > 0 Then
                DT = _AddressTypesDS.Tables(0)
            End If

            Return DT

        Catch ex As Exception
            Throw
        End Try

    End Function
    Private Shared Function LoadAddressTypes(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        SyncLock _AddressTypeslockObj

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBEL.RETRIEVE_ALL_ADDRESS_TYPES" & ".xml"
            Dim UniqueThreadIdentifier As String = GetUniqueKey()
            Dim XMLDS As DataSet
            Dim FStream As FileStream
            Dim XMLSerial As XmlSerializer
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_ALL_ADDRESS_TYPES"

            Try


                XMLDS = XMLHandler.ToandFromDataset(UniqueThreadIdentifier, "FDBEL.REG_ADDRESS_TYPE", "CREATE_DATE", "FDBEL.RETRIEVE_ALL_ADDRESS_TYPES")
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
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "REG_ADDRESS_TYPE")
                        Else
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "REG_ADDRESS_TYPE", transaction)
                        End If
                    End If

                    _AddressTypesDS = XMLDS

                    XMLSerial = New XmlSerializer(XMLDS.GetType())

                    FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)
                    XMLSerial.Serialize(FStream, XMLDS)

                    FStream.Close()

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _AddressTypesDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_AddressTypesDS)
                    Return ds
                Else
                    Return _AddressTypesDS
                End If

            Catch ex As Exception
                Throw
            Finally

                If FStream IsNot Nothing Then
                    FStream.Close()
                    FStream.Dispose()
                End If

                FStream = Nothing

            End Try

        End SyncLock

    End Function

    Public Shared Function RetrieveRemarkDefinitions() As DataTable

        If _RemarksDS Is Nothing Then
            _RemarksDS = LoadRemarkDefinitionValues()
        End If

        Dim DT As New DataTable("REMARKS_DEFINITION")
        If _RemarksDS.Tables.Count > 0 Then
            DT = _RemarksDS.Tables(0)
        End If

        Return DT
    End Function

    Private Shared Function LoadRemarkDefinitionValues(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        SyncLock (_RemarkslockObj)

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBEL.RETRIEVE_REMARKS_DEFINITION" & ".xml"
            Dim UniqueThreadIdentifier As String = GetUniqueKey()
            Dim FStream As FileStream
            Dim XMLSerial As XmlSerializer

            Try

                Dim XMLDS As DataSet = XMLHandler.ToandFromDataset(UniqueThreadIdentifier, "FDBEL.REMARKS_DEFINITION", "CREATE_DATE", "FDBEL.RETRIEVE_REMARKS_DEFINITION")
                If XMLDS.Tables.Count = 0 Then
                    Dim DB As Database = CMSDALCommon.CreateDatabase()
                    Dim DBCommandWrapper As DbCommand
                    Dim SQLCall As String = "FDBEL.RETRIEVE_REMARKS_DEFINITION"
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        If transaction Is Nothing Then
                            XMLDS = DB.ExecuteDataSet(DBCommandWrapper)
                        Else
                            XMLDS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                        End If
                    Else
                        If transaction Is Nothing Then
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "REMARKS_DEFINITION")
                        Else
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "REMARKS_DEFINITION", transaction)
                        End If
                    End If
                    _RemarksDS = XMLDS

                    FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)
                    XMLSerial = New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(FStream, XMLDS)
                    FStream.Close()
                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _RemarksDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_RemarksDS)
                    Return ds
                Else
                    Return _RemarksDS
                End If

            Catch ex As Exception
                Throw
            Finally

                If FStream IsNot Nothing Then
                    FStream.Close()
                    FStream.Dispose()
                End If

                FStream = Nothing
            End Try
        End SyncLock


    End Function

    Public Shared Function RetrieveLifeEventValues() As DataTable
        If _LifeEventsDS Is Nothing Then
            _LifeEventsDS = LoadLifeEventValues()
        End If

        Dim DT As New DataTable("LIFE_EVENT_VALUES")
        If _LifeEventsDS.Tables.Count > 0 Then
            DT = _LifeEventsDS.Tables(0)
        End If

        Return DT
    End Function
    Private Shared Function LoadLifeEventValues(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        SyncLock (_LifeEventValueslockObj)
            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBEL.RETRIEVE_LIFE_EVENT_VALUES" & ".xml"
            Dim UniqueThreadIdentifier As String = GetUniqueKey()
            Dim FStream As FileStream
            Dim XMLSerial As XmlSerializer


            Try

                Dim XMLDS As DataSet = XMLHandler.ToandFromDataset(UniqueThreadIdentifier, "FDBEL.REG_LIFE_EVENT_VALUES", "CREATE_DATE", "FDBEL.RETRIEVE_LIFE_EVENT_VALUES")
                If XMLDS.Tables.Count = 0 Then
                    Dim DB As Database = CMSDALCommon.CreateDatabase()
                    Dim DBCommandWrapper As DbCommand
                    Dim SQLCall As String = "FDBEL.RETRIEVE_LIFE_EVENT_VALUES"
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        If transaction Is Nothing Then
                            XMLDS = DB.ExecuteDataSet(DBCommandWrapper)
                        Else
                            XMLDS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                        End If
                    Else
                        If transaction Is Nothing Then
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "LIFE_EVENT_VALUES")
                        Else
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "LIFE_EVENT_VALUES", transaction)
                        End If
                    End If
                    _LifeEventsDS = XMLDS

                    FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)
                    XMLSerial = New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(FStream, XMLDS)
                    FStream.Close()
                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _LifeEventsDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_LifeEventsDS)
                    Return ds
                Else
                    Return _LifeEventsDS
                End If

            Catch ex As Exception
                Throw
            Finally

                If FStream IsNot Nothing Then
                    FStream.Close()
                    FStream.Dispose()
                End If

                FStream = Nothing

            End Try

        End SyncLock

    End Function

    Public Shared Function RetrieveLifeEventValuesApplies() As DataTable
        If _LifeEventsAppliesDS Is Nothing Then
            _LifeEventsAppliesDS = LoadLifeEventValuesApplies()
        End If

        Dim DT As New DataTable("LIFE_EVENT_VALUES_APPLIES")
        If _LifeEventsAppliesDS.Tables.Count > 0 Then
            DT = _LifeEventsAppliesDS.Tables(0)
        End If

        Return DT
    End Function
    Private Shared Function LoadLifeEventValuesApplies(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        SyncLock (_LifeEventValueslockObj)

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBEL.RETRIEVE_LIFE_EVENT_VALUES_APPLIES" & ".xml"
            Dim UniqueThreadIdentifier As String = GetUniqueKey()
            Dim FStream As FileStream
            Dim XMLSerial As XmlSerializer


            Try

                Dim XMLDS As DataSet = XMLHandler.ToandFromDataset(UniqueThreadIdentifier, "FDBEL.REG_LIFE_EVENT_VALUES_APPLIES", "ONLINE_DATE", "FDBEL.RETRIEVE_LIFE_EVENT_VALUES_APPLIES")
                If XMLDS.Tables.Count = 0 Then
                    Dim DB As Database = CMSDALCommon.CreateDatabase()
                    Dim DBCommandWrapper As DbCommand
                    Dim SQLCall As String = "FDBEL.REG_LIFE_EVENT_VALUES_APPLIES"
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        If transaction Is Nothing Then
                            XMLDS = DB.ExecuteDataSet(DBCommandWrapper)
                        Else
                            XMLDS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                        End If
                    Else
                        If transaction Is Nothing Then
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "REG_LIFE_EVENT_VALUES_APPLIES")
                        Else
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "REG_LIFE_EVENT_VALUES_APPLIES", transaction)
                        End If
                    End If
                    _LifeEventsAppliesDS = XMLDS

                    FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)
                    XMLSerial = New XmlSerializer(XMLDS.GetType())

                    XMLSerial.Serialize(FStream, XMLDS)
                    FStream.Close()

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _LifeEventsAppliesDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_LifeEventsAppliesDS)
                    Return ds
                Else
                    Return _LifeEventsAppliesDS
                End If

            Catch ex As Exception
                Throw
            Finally

                If FStream IsNot Nothing Then
                    FStream.Close()
                    FStream.Dispose()
                End If

                FStream = Nothing

            End Try
        End SyncLock

    End Function

    Public Shared Function RetrieveMemplans() As DataTable
        If _MemPlansDS Is Nothing Then
            _MemPlansDS = LoadMemplans()
        End If

        Dim DT As New DataTable("MEMPLANS")
        If _MemPlansDS.Tables.Count > 0 Then
            DT = _MemPlansDS.Tables(0)
        End If

        Return DT
    End Function
    Private Shared Function LoadMemplans(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        SyncLock (_MemplanlockObj)
            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBWRK.RETRIEVE_MEMPLAN" & ".xml"
            Dim UniqueThreadIdentifier As String = GetUniqueKey()
            Dim FStream As FileStream
            Dim XMLSerial As XmlSerializer

            Try

                Dim XMLDS As DataSet = XMLHandler.ToandFromDataset(UniqueThreadIdentifier, "FDBWRK.MEMPLAN", "LASTUPDT", "FDBWRK.RETRIEVE_MEMPLAN")
                If XMLDS.Tables.Count = 0 Then
                    Dim DB As Database = CMSDALCommon.CreateDatabase()
                    Dim DBCommandWrapper As DbCommand
                    Dim SQLCall As String = "FDBWRK.RETRIEVE_MEMPLAN"
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        If transaction Is Nothing Then
                            XMLDS = DB.ExecuteDataSet(DBCommandWrapper)
                        Else
                            XMLDS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                        End If
                    Else
                        If transaction Is Nothing Then
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "RETRIEVE_MEMPLAN")
                        Else
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "RETRIEVE_MEMPLAN", transaction)
                        End If
                    End If
                    _MemPlansDS = XMLDS

                    FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)
                    XMLSerial = New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(FStream, XMLDS)
                    FStream.Close()

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _MemPlansDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_MemPlansDS)
                    Return ds
                Else
                    Return _MemPlansDS
                End If


            Catch ex As Exception
                Throw
            Finally

                If FStream IsNot Nothing Then
                    FStream.Close()
                    FStream.Dispose()
                End If

                FStream = Nothing

            End Try
        End SyncLock

    End Function

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
            DT = Nothing
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
            If FStream IsNot Nothing Then
                FStream.Close()
            End If

            FStream = Nothing

        End Try
    End Function

    Public Shared Function RetrieveRegAlertTerminationValues() As DataTable

        If _RegAlertTermValuesDS Is Nothing Then
            _RegAlertTermValuesDS = LoadRegAlertTerminationValues()
        End If

        Dim DT As New DataTable("REG_ALERT_TERMINATION_VALUES")
        If _RegAlertTermValuesDS.Tables.Count > 0 Then
            DT = _RegAlertTermValuesDS.Tables(0)
        End If

        Return DT

    End Function
    Private Shared Function LoadRegAlertTerminationValues(Optional ByVal ds As DataSet = Nothing) As DataSet
        SyncLock (_RegAlertTermValueslockObj)

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBEL.RETRIEVE_REG_ALERT_TERMINATION_VALUES" & ".xml"
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim FStream As FileStream
            Dim XMLSerial As XmlSerializer
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_REG_ALERT_TERMINATION_VALUES"
            Dim XMLDS As DataSet

            Try

                XMLDS = XMLHandler.ToandFromDataset(UniqueThreadIdentifier, "FDBEL.REG_ALERT_TERMINATION_VALUES", "LASTUPDT", "FDBEL.RETRIEVE_REG_ALERT_TERMINATION_VALUES")
                If XMLDS.Tables.Count = 0 Then
                    DB = CMSDALCommon.CreateDatabase()
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        XMLDS = DB.ExecuteDataSet(DBCommandWrapper)
                    Else
                        DB.LoadDataSet(DBCommandWrapper, XMLDS, "REG_ALERT_TERMINATION_VALUES")
                    End If

                    _RegAlertTermValuesDS = XMLDS

                    FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                    XMLSerial = New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(FStream, XMLDS)

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _RegAlertTermValuesDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_RegAlertTermValuesDS)
                    Return ds
                Else
                    Return _RegAlertTermValuesDS
                End If

            Catch ex As Exception
                Throw
            Finally
                If FStream IsNot Nothing Then
                    FStream.Close()
                    FStream.Dispose()
                End If

                FStream = Nothing

            End Try
        End SyncLock

    End Function

    Public Shared Function RetrieveEligibilityPeriod() As DataTable

        Dim DB As Database
        Dim DBCommand As DbCommand
        Dim SQLCall As String = "FDBEL.RETRIEVE_ELIG_PERIOD"
        Dim DS As DataSet
        Dim DT As DataTable

        Try
            DB = CMSDALCommon.CreateDatabase()

            dbCommand = DB.GetStoredProcCommand(SQLCall)

            DS = DB.ExecuteDataSet(dbCommand)
            DT = New DataTable

            If DS.Tables.Count > 0 Then
                DT = DS.Tables(0)
                DT.TableName = "ELIG_PERIOD"
                _GlobalEligPeriod = CDate(DS.Tables(0).Rows(0)("ELIG_PER_DATE"))
            End If

            Return DT

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function RetrieveA2countOverrideReasons() As DataTable

        If _A2CountOverrideReasonsDS Is Nothing Then
            _A2CountOverrideReasonsDS = LoadA2countOverrideReasons()
        End If

        Dim DT As New DataTable("A2COUNT_OVERRIDE_REASONS")
        If _A2CountOverrideReasonsDS.Tables.Count > 0 Then
            DT = _A2CountOverrideReasonsDS.Tables(0)
        End If

        Return DT

    End Function
    Private Shared Function LoadA2countOverrideReasons(Optional ByVal ds As DataSet = Nothing) As DataSet
        SyncLock (_A2countoverridereasonslockObj)
            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBEL.RETRIEVE_A2COUNT_OVERRIDE_REASONS" & ".xml"
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim FStream As FileStream = Nothing

            Try

                Dim XMLDS As DataSet = XMLHandler.ToandFromDataset(UniqueThreadIdentifier, "FDBWRK.A2COUNT_OVERRIDE_REASONS", "CREATE_DATE", "FDBEL.RETRIEVE_A2COUNT_OVERRIDE_REASONS")
                If XMLDS.Tables.Count = 0 Then
                    Dim DB As Database = CMSDALCommon.CreateDatabase()
                    Dim DBCommandWrapper As DbCommand
                    Dim SQLCall As String = "FDBEL.RETRIEVE_A2COUNT_OVERRIDE_REASONS"
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        XMLDS = DB.ExecuteDataSet(DBCommandWrapper)
                    Else
                        DB.LoadDataSet(DBCommandWrapper, XMLDS, "A2COUNT_OVERRIDE_REASONS")
                    End If

                    _A2CountOverrideReasonsDS = XMLDS

                    FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                    Dim XMLSerial As New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(FStream, XMLDS)

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _A2CountOverrideReasonsDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_A2CountOverrideReasonsDS)
                    Return ds
                Else
                    Return _A2CountOverrideReasonsDS
                End If

            Catch ex As Exception
                Throw
            Finally
                If FStream IsNot Nothing Then
                    FStream.Close()
                    FStream.Dispose()
                End If

                FStream = Nothing

            End Try
        End SyncLock

    End Function

    Public Shared Function RetrieveEligSpecialAcctValues() As DataTable

        If _EligSpecialAcctValuesDS Is Nothing Then
            _EligSpecialAcctValuesDS = LoadEligSpecialAcctValues()
        End If

        Dim DT As New DataTable("ELIG_SPECIAL_ACCT_VALUES")
        If _EligSpecialAcctValuesDS.Tables.Count > 0 Then
            DT = _EligSpecialAcctValuesDS.Tables(0)
        End If

        Return DT

    End Function
    Private Shared Function LoadEligSpecialAcctValues(Optional ByVal ds As DataSet = Nothing) As DataSet
        SyncLock (_EligspecialAcctValueslockObj)
            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBEL.RETRIEVE_ELIG_SPECIAL_ACCT_VALUES" & ".xml"
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim FStream As FileStream = Nothing

            Try

                Dim XMLDS As DataSet = XMLHandler.ToandFromDataset(UniqueThreadIdentifier, "FDBEL.ELIG_SPECIAL_ACCT_VALUES", "ONLINE_DATE", "FDBEL.RETRIEVE_ELIG_SPECIAL_ACCT_VALUES")
                If XMLDS.Tables.Count = 0 Then
                    Dim DB As Database = CMSDALCommon.CreateDatabase()
                    Dim DBCommandWrapper As DbCommand
                    Dim SQLCall As String = "FDBEL.RETRIEVE_ELIG_SPECIAL_ACCT_VALUES"
                    dbCommandWrapper = db.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        XMLDS = db.ExecuteDataSet(dbCommandWrapper)
                    Else
                        db.LoadDataSet(dbCommandWrapper, XMLDS, "ELIG_SPECIAL_ACCT_VALUES")
                    End If

                    _EligSpecialAcctValuesDS = XMLDS

                    FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                    Dim XMLSerial As New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(FStream, XMLDS)

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _EligSpecialAcctValuesDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_EligSpecialAcctValuesDS)
                    Return ds
                Else
                    Return _EligSpecialAcctValuesDS
                End If

            Catch ex As Exception
                Throw

            Finally
                If FStream IsNot Nothing Then
                    FStream.Close()
                    FStream.Dispose()
                End If

                FStream = Nothing
            End Try
        End SyncLock

    End Function

    Public Shared Function RetrieveMedicalCoverageValues() As DataTable

        If _MedicalCoverageValuesDS Is Nothing Then
            _MedicalCoverageValuesDS = LoadMedicalCoverageValues()
        End If

        Dim DT As New DataTable("COVERAGE_VALUES")
        If _MedicalCoverageValuesDS.Tables.Count > 0 Then
            DT = _MedicalCoverageValuesDS.Tables(0)
        End If

        Return DT

    End Function
    Private Shared Function LoadMedicalCoverageValues(Optional ByVal ds As DataSet = Nothing) As DataSet
        SyncLock (_MedicalCoverageValueslockObj)
            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBEL.RETRIEVE_MEDICAL_COVERAGE_VALUES" & ".xml"
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim FStream As FileStream = Nothing

            Try

                Dim XMLDS As DataSet = XMLHandler.ToandFromDataset(UniqueThreadIdentifier, "FDBEL.MEDICAL_COVERAGE_VALUES", "LASTUPDT", "FDBEL.RETRIEVE_MEDICAL_COVERAGE_VALUES")
                If XMLDS.Tables.Count = 0 Then
                    Dim DB As Database = CMSDALCommon.CreateDatabase()
                    Dim DBCommandWrapper As DbCommand
                    Dim SQLCall As String = "FDBEL.RETRIEVE_MEDICAL_COVERAGE_VALUES"
                    dbCommandWrapper = db.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        XMLDS = db.ExecuteDataSet(dbCommandWrapper)
                    Else
                        db.LoadDataSet(dbCommandWrapper, XMLDS, "COVERAGE_VALUES")
                    End If

                    _MedicalCoverageValuesDS = XMLDS

                    FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                    Dim XMLSerial As New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(FStream, XMLDS)

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _MedicalCoverageValuesDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_MedicalCoverageValuesDS)
                    Return ds
                Else
                    Return _MedicalCoverageValuesDS
                End If

            Catch ex As Exception
                Throw
            Finally
                If FStream IsNot Nothing Then
                    FStream.Close()
                    FStream.Dispose()
                End If

                FStream = Nothing
            End Try
        End SyncLock

    End Function

    Public Shared Function RetrieveDentalCoverageValues() As DataTable

        If _DentalCoverageValuesDS Is Nothing Then
            _DentalCoverageValuesDS = LoadDentalCoverageValues()
        End If

        Dim DT As New DataTable("COVERAGE_VALUES")
        If _DentalCoverageValuesDS.Tables.Count > 0 Then
            DT = _DentalCoverageValuesDS.Tables(0)
        End If

        Return DT

    End Function
    Private Shared Function LoadDentalCoverageValues(Optional ByVal ds As DataSet = Nothing) As DataSet
        SyncLock (_DentalCoverageValueslockObj)
            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBEL.RETRIEVE_DENTAL_COVERAGE_VALUES" & ".xml"
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim FStream As FileStream = Nothing

            Try

                Dim XMLDS As DataSet = XMLHandler.ToandFromDataset(UniqueThreadIdentifier, "FDBEL.DENTAL_COVERAGE_VALUES", "LASTUPDT", "FDBEL.RETRIEVE_DENTAL_COVERAGE_VALUES")
                If XMLDS.Tables.Count = 0 Then
                    Dim DB As Database = CMSDALCommon.CreateDatabase()
                    Dim DBCommandWrapper As DbCommand
                    Dim SQLCall As String = "FDBEL.RETRIEVE_DENTAL_COVERAGE_VALUES"
                    dbCommandWrapper = db.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        XMLDS = db.ExecuteDataSet(dbCommandWrapper)
                    Else
                        db.LoadDataSet(dbCommandWrapper, XMLDS, "COVERAGE_VALUES")
                    End If

                    _DentalCoverageValuesDS = XMLDS

                    FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                    Dim XMLSerial As New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(FStream, XMLDS)

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _DentalCoverageValuesDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_DentalCoverageValuesDS)
                    Return ds
                Else
                    Return _DentalCoverageValuesDS
                End If

            Catch ex As Exception
                Throw

            Finally
                If FStream IsNot Nothing Then
                    FStream.Close()
                    FStream.Dispose()
                End If

                FStream = Nothing
            End Try
        End SyncLock

    End Function

    Public Shared Function RetrieveLocals() As DataTable

        If _LocalValuesDS Is Nothing Then
            _LocalValuesDS = LoadLocalValues()
        End If

        Dim DT As New DataTable("LOOKUP_LOCAL")
        If _LocalValuesDS.Tables.Count > 0 Then
            DT = _LocalValuesDS.Tables(0)
        End If

        Return DT

    End Function
    Private Shared Function LoadLocalValues(Optional ByVal ds As DataSet = Nothing) As DataSet
        SyncLock (_LocalValueslockObj)
            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBEL.RETRIEVE_LOCALS" & ".xml"
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim FStream As FileStream = Nothing

            Try

                Dim XMLDS As DataSet = XMLHandler.ToandFromDataset(UniqueThreadIdentifier, "HOURS.LOOKUP_LOCAL", "LASTUPDT", "FDBEL.RETRIEVE_LOCALS")
                If XMLDS.Tables.Count = 0 Then
                    Dim DB As Database = CMSDALCommon.CreateDatabase()
                    Dim DBCommandWrapper As DbCommand
                    Dim SQLCall As String = "FDBEL.RETRIEVE_LOCALS"
                    dbCommandWrapper = db.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        XMLDS = db.ExecuteDataSet(dbCommandWrapper)
                    Else
                        db.LoadDataSet(dbCommandWrapper, XMLDS, "LOOKUP_LOCAL")
                    End If

                    _LocalValuesDS = XMLDS

                    FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                    Dim XMLSerial As New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(FStream, XMLDS)

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _LocalValuesDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_LocalValuesDS)
                    Return ds
                Else
                    Return _LocalValuesDS
                End If

            Catch ex As Exception
                Throw

            Finally
                If FStream IsNot Nothing Then
                    FStream.Close()
                    FStream.Dispose()
                End If

                FStream = Nothing
            End Try
        End SyncLock

    End Function

    Public Shared Function RetrieveRetireePercentValues() As DataTable

        If _RetireePlanDS Is Nothing Then
            _RetireePlanDS = LoadRetireePercentValues()
        End If

        Dim DT As New DataTable("ELIG_RETIREE_PLAN_PERCENT_LOOKUP")
        If _RetireePlanDS.Tables.Count > 0 Then
            DT = _RetireePlanDS.Tables(0)
        End If

        Return DT

    End Function
    Private Shared Function LoadRetireePercentValues(Optional ByVal ds As DataSet = Nothing) As DataSet
        SyncLock (_RetireePlanlockObj)

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBEL.RETRIEVE_RETIREE_PLAN_PERCENT_VALUES" & ".xml"
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim fStream As FileStream = Nothing

            Try

                Dim XMLDS As DataSet = XMLHandler.ToandFromDataset(UniqueThreadIdentifier, "FDBEL.ELIG_RETIREE_PLAN_PERCENT_LOOKUP", "CREATE_DATE", "FDBEL.RETRIEVE_RETIREE_PLAN_PERCENT_VALUES")
                If XMLDS.Tables.Count = 0 Then
                    Dim DB As Database = CMSDALCommon.CreateDatabase()
                    Dim DBCommandWrapper As DbCommand
                    Dim SQLCall As String = "FDBEL.RETRIEVE_RETIREE_PLAN_PERCENT_VALUES"
                    dbCommandWrapper = db.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        XMLDS = db.ExecuteDataSet(dbCommandWrapper)
                    Else
                        db.LoadDataSet(dbCommandWrapper, XMLDS, "ELIG_RETIREE_PLAN_PERCENT_LOOKUP")
                    End If

                    _RetireePlanDS = XMLDS

                    fStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                    Dim XMLSerial As New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(fStream, XMLDS)

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _RetireePlanDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_RetireePlanDS)
                    Return ds
                Else
                    Return _RetireePlanDS
                End If

            Catch ex As Exception
                Throw
                Return Nothing
            Finally
                If fStream IsNot Nothing Then
                    fStream.Close()
                    fStream.Dispose()
                End If

                fStream = Nothing
            End Try
        End SyncLock

    End Function

    Public Shared Function RetrieveInvalidMemtypeMedplans() As DataTable

        If _InvalidMemtypeMedplanDS Is Nothing Then
            _InvalidMemtypeMedplanDS = LoadInvalidMemtypeMedplanValues()
        End If

        Dim DT As New DataTable("INVALID_MEMTYPE_MEDPLAN_LOOKUP")
        If _InvalidMemtypeMedplanDS.Tables.Count > 0 Then
            DT = _InvalidMemtypeMedplanDS.Tables(0)
        End If

        Return DT

    End Function
    Private Shared Function LoadInvalidMemtypeMedplanValues(Optional ByVal ds As DataSet = Nothing) As DataSet
        SyncLock (_InvalidMemtypeMedlockObj)

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBEL.RETRIEVE_INVALID_MEMTYPE_MEDPLAN_LOOKUP" & ".xml"
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim fStream As FileStream = Nothing

            Try

                Dim XMLDS As DataSet = XMLHandler.ToandFromDataset(UniqueThreadIdentifier, "FDBEL.INVALID_MEMTYPE_MEDPLAN_LOOKUP", "FROM_DATE", "FDBEL.RETRIEVE_INVALID_MEMTYPE_MEDPLAN_LOOKUP")
                If XMLDS.Tables.Count = 0 Then
                    Dim DB As Database = CMSDALCommon.CreateDatabase()
                    Dim DBCommandWrapper As DbCommand
                    Dim SQLCall As String = "FDBEL.RETRIEVE_INVALID_MEMTYPE_MEDPLAN_LOOKUP"
                    dbCommandWrapper = db.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        XMLDS = db.ExecuteDataSet(dbCommandWrapper)
                    Else
                        db.LoadDataSet(dbCommandWrapper, XMLDS, "INVALID_MEMTYPE_MEDPLAN_LOOKUP")
                    End If

                    _InvalidMemtypeMedplanDS = XMLDS

                    fStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                    Dim XMLSerial As New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(fStream, XMLDS)

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _InvalidMemtypeMedplanDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_InvalidMemtypeMedplanDS)
                    Return ds
                Else
                    Return _InvalidMemtypeMedplanDS
                End If

            Catch ex As Exception
                Throw
                Return Nothing
            Finally
                If fStream IsNot Nothing Then
                    fStream.Close()
                    fStream.Dispose()
                End If

                fStream = Nothing
            End Try
        End SyncLock

    End Function

    Public Shared Function RetrieveIneligibleforSpecialAccounts() As DataTable

        If _IneligibleforSpecialAcctsDS Is Nothing Then
            _IneligibleforSpecialAcctsDS = LoadIneligibleforSpecialAccounts()
        End If

        Dim _dataTable As New DataTable("INELIGIBLE_FOR_SPECIAL_ACCTS")
        If _IneligibleforSpecialAcctsDS.Tables.Count > 0 Then
            _dataTable = _IneligibleforSpecialAcctsDS.Tables(0)
        End If

        Return _dataTable

    End Function
    Private Shared Function LoadIneligibleforSpecialAccounts(Optional ByVal ds As DataSet = Nothing) As DataSet
        Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBEL.RETRIEVE_INELIGIBLE_FOR_SPECIAL_ACCTS" & ".xml"
        Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
        Dim fStream As FileStream = Nothing

        Try

            SyncLock (_IneligibleforSpecialAcctslockObj)
                Dim XMLDS As DataSet = XMLHandler.ToandFromDataset(UniqueThreadIdentifier, "FDBEL.INELIGIBLE_FOR_SPECIAL_ACCTS", "ONLINE_DATE", "FDBEL.RETRIEVE_INELIGIBLE_FOR_SPECIAL_ACCTS")
                If XMLDS.Tables.Count = 0 Then
                    Dim DB As Database = CMSDALCommon.CreateDatabase()
                    Dim DBCommandWrapper As DbCommand
                    Dim SQLCall As String = "FDBEL.RETRIEVE_INELIGIBLE_FOR_SPECIAL_ACCTS"
                    dbCommandWrapper = db.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        XMLDS = db.ExecuteDataSet(dbCommandWrapper)
                    Else
                        db.LoadDataSet(dbCommandWrapper, XMLDS, "INELIGIBLE_FOR_SPECIAL_ACCTS")
                    End If

                    _IneligibleforSpecialAcctsDS = XMLDS

                    fStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                    Dim XMLSerial As New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(fStream, XMLDS)

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _IneligibleforSpecialAcctsDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_IneligibleforSpecialAcctsDS)
                    Return ds
                Else
                    Return _IneligibleforSpecialAcctsDS
                End If

            End SyncLock

        Catch ex As Exception
            Throw
            Return Nothing
        Finally
            If fStream IsNot Nothing Then
                fStream.Close()
                fStream.Dispose()
            End If

            fStream = Nothing
        End Try
    End Function

    Public Shared Function RetrieveEligMetalDescriptions() As DataTable

        If _EligMetalDescDS Is Nothing Then
            _EligMetalDescDS = LoadEligMetalDescriptions()
        End If

        Dim _dataTable As New DataTable("ELIG_METAL_DESCRIPTIONS")
        If _EligMetalDescDS.Tables.Count > 0 Then
            _dataTable = _EligMetalDescDS.Tables(0)
        End If

        Return _dataTable

    End Function
    Private Shared Function LoadEligMetalDescriptions(Optional ByVal ds As DataSet = Nothing) As DataSet
        Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBEL.RETRIEV_ELIG_METAL_DESCRIPTIONS" & ".xml"
        Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
        Dim fStream As FileStream = Nothing

        Try

            SyncLock (_EligMetalDesclockObj)
                Dim XMLDS As DataSet = XMLHandler.ToandFromDataset(UniqueThreadIdentifier, "FDBEL.ELIG_METAL_DESCRIPTIONS", "CREATE_DATE", "FDBEL.RETRIEV_ELIG_METAL_DESCRIPTIONS")
                If XMLDS.Tables.Count = 0 Then
                    Dim DB As Database = CMSDALCommon.CreateDatabase()
                    Dim DBCommandWrapper As DbCommand
                    Dim SQLCall As String = "FDBEL.RETRIEV_ELIG_METAL_DESCRIPTIONS"
                    dbCommandWrapper = db.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        XMLDS = db.ExecuteDataSet(dbCommandWrapper)
                    Else
                        db.LoadDataSet(dbCommandWrapper, XMLDS, "ELIG_METAL_DESCRIPTIONS")
                    End If

                    _EligMetalDescDS = XMLDS

                    fStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                    Dim XMLSerial As New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(fStream, XMLDS)

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _EligMetalDescDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_EligMetalDescDS)
                    Return ds
                Else
                    Return _EligMetalDescDS
                End If

            End SyncLock

        Catch ex As Exception
            Throw
            Return Nothing
        Finally
            If fStream IsNot Nothing Then
                fStream.Close()
                fStream.Dispose()
            End If

            fStream = Nothing
        End Try
    End Function

    Public Shared Function RetrieveHMONetworks() As DataTable

        If _HMONetworksDS Is Nothing Then
            _HMONetworksDS = LoadHMONetworks()
        End If

        Dim DT As New DataTable("HMO_NETWORK_LOOKUP")
        If _HMONetworksDS.Tables.Count > 0 Then
            DT = _HMONetworksDS.Tables(0)
        End If

        Return DT

    End Function
    Private Shared Function LoadHMONetworks(Optional ByVal ds As DataSet = Nothing) As DataSet
        Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBEL.RETRIEVE_HMO_NETWORK_LOOKUP" & ".xml"
        Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
        Dim FS As FileStream = Nothing

        Try

            SyncLock (_UHCHMONetworklockObj)
                Dim XMLDS As DataSet = XMLHandler.ToandFromDataset(UniqueThreadIdentifier, "FDBEL.HMO_NETWORK_LOOKUP", "ONLINE_DATE", "FDBEL.RETRIEVE_HMO_NETWORK_LOOKUP")
                If XMLDS.Tables.Count = 0 Then
                    Dim DB As Database = CMSDALCommon.CreateDatabase()
                    Dim DBCommandWrapper As DbCommand
                    Dim SQLCall As String = "FDBEL.RETRIEVE_HMO_NETWORK_LOOKUP"
                    dbCommandWrapper = db.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        XMLDS = db.ExecuteDataSet(dbCommandWrapper)
                    Else
                        db.LoadDataSet(dbCommandWrapper, XMLDS, "HMO_NETWORK_LOOKUP")
                    End If

                    _HMONetworksDS = XMLDS

                    FS = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                    Dim XMLSerial As New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(FS, XMLDS)

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _HMONetworksDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_HMONetworksDS)
                    Return ds
                Else
                    Return _HMONetworksDS
                End If

            End SyncLock

        Catch ex As Exception
            Throw
            Return Nothing
        Finally
            If FS IsNot Nothing Then
                FS.Close()
                FS.Dispose()
            End If

            FS = Nothing
        End Try
    End Function

    Public Shared Function RetrieveCOBRAqeValues() As DataTable

        If _CobraQEDS Is Nothing Then
            _CobraQEDS = LoadCOBRAQEValues()
        End If

        Dim _dataTable As New DataTable("COBRA_QE_VALUES")
        If _CobraQEDS.Tables.Count > 0 Then
            _dataTable = _CobraQEDS.Tables(0)
        End If

        Return _dataTable

    End Function
    Private Shared Function LoadCOBRAQEValues(Optional ByVal ds As DataSet = Nothing) As DataSet
        Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBEL.RETRIEVE_COBRA_QE_VALUES" & ".xml"
        Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
        Dim fStream As FileStream = Nothing

        Try

            SyncLock (_CobraQElockObj)
                Dim XMLDS As DataSet = XMLHandler.ToandFromDataset(UniqueThreadIdentifier, "FDBEL.COBRA_QE_VALUES", "CREATE_DATE", "FDBEL.RETRIEVE_COBRA_QE_VALUES")
                If XMLDS.Tables.Count = 0 Then
                    Dim DB As Database = CMSDALCommon.CreateDatabase()
                    Dim DBCommandWrapper As DbCommand
                    Dim SQLCall As String = "FDBEL.RETRIEVE_COBRA_QE_VALUES"
                    dbCommandWrapper = db.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        XMLDS = db.ExecuteDataSet(dbCommandWrapper)
                    Else
                        db.LoadDataSet(dbCommandWrapper, XMLDS, "COBRA_QE_VALUES")
                    End If

                    _CobraQEDS = XMLDS

                    fStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                    Dim XMLSerial As New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(fStream, XMLDS)

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _CobraQEDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_CobraQEDS)
                    Return ds
                Else
                    Return _CobraQEDS
                End If

            End SyncLock

        Catch ex As Exception
            Throw
            Return Nothing
        Finally
            If fStream IsNot Nothing Then
                fStream.Close()
                fStream.Dispose()
            End If

            fStream = Nothing
        End Try
    End Function

    Public Shared Function RetrieveCOBRARates() As DataTable

        If _CobraRatesDS Is Nothing Then
            _CobraRatesDS = LoadCOBRARates()
        End If

        Dim _dataTable As New DataTable("COBRA_RATES")
        If _CobraRatesDS.Tables.Count > 0 Then
            _dataTable = _CobraRatesDS.Tables(0)
        End If

        Return _dataTable

    End Function
    Private Shared Function LoadCOBRARates(Optional ByVal ds As DataSet = Nothing) As DataSet
        Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBEL.RETRIEVE_COBRA_RATES" & ".xml"
        Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
        Dim fStream As FileStream = Nothing

        Try

            SyncLock (_CobraRateslockObj)
                Dim XMLDS As DataSet = XMLHandler.ToandFromDataset(UniqueThreadIdentifier, "FDBEL.COBRA_RATES", "CREATE_DATE", "FDBEL.RETRIEVE_COBRA_RATES")
                If XMLDS.Tables.Count = 0 Then
                    Dim DB As Database = CMSDALCommon.CreateDatabase()
                    Dim DBCommandWrapper As DbCommand
                    Dim SQLCall As String = "FDBEL.RETRIEVE_COBRA_RATES"
                    dbCommandWrapper = db.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        XMLDS = db.ExecuteDataSet(dbCommandWrapper)
                    Else
                        db.LoadDataSet(dbCommandWrapper, XMLDS, "COBRA_RATES")
                    End If

                    _CobraRatesDS = XMLDS

                    fStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                    Dim XMLSerial As New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(fStream, XMLDS)

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _CobraRatesDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_CobraRatesDS)
                    Return ds
                Else
                    Return _CobraRatesDS
                End If

            End SyncLock

        Catch ex As Exception
            Throw
            Return Nothing
        Finally
            If fStream IsNot Nothing Then
                fStream.Close()
                fStream.Dispose()
            End If

            fStream = Nothing
        End Try
    End Function



#End Region

#Region "CRUD"

    '*****************************************************************************************************************************
    '********************************************** PHI Authorization Record Processing ******************************************
    'BOOKMARK: 002b - PHIAuthAdd 
    Public Shared Function PHIAuthAdd(ByVal familyID As Integer,
                                  ByVal relationID As Short,
                                  ByVal fromDate As Date,
                                  ByVal thruDate As Date,
                                  ByVal GranteeFamilyID As Integer,
                                  ByVal GranteeRelationID As Short,
                                  ByVal GranteeApplication As Short,
                                  ByVal Comments As String,
                                  ByVal UserRights As String,
                                  Optional ByVal transaction As DbTransaction = Nothing) As Boolean

        Dim ResultOfSP As Boolean = True

        If thruDate = CDate("12/31/9998") Then
            thruDate = CDate("12/31/9999")
        End If

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.ADD_REG_PHIAUTH"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@vFAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@vRELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@vFROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@vTHRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@vGRANTEE_FAMILY_ID", DbType.Int32, GranteeFamilyID)
            DB.AddInParameter(DBCommandWrapper, "@vGRANTEE_RELATION_ID", DbType.Int16, GranteeRelationID)
            DB.AddInParameter(DBCommandWrapper, "@vGRANTEE_APPLICATION", DbType.Int16, GranteeApplication)
            DB.AddInParameter(DBCommandWrapper, "@vCOMMENTS", DbType.String, Comments)
            DB.AddInParameter(DBCommandWrapper, "@vUSER_RIGHTS", DbType.String, UserRights)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            ResultOfSP = False
            Throw
        Catch ex As SqlClient.SqlException
            ResultOfSP = False
            Stop
        Catch ex As Exception
            ResultOfSP = False
            Throw
        End Try

        Return ResultOfSP
    End Function

    'BOOKMARK: 002c - PHIAuthorizationTrim
    Public Shared Function PHIAuthorizationTrim(ByVal familyID As Integer,
                                  ByVal relationID As Short,
                                  ByVal thruDate As Date,
                                  ByVal GranteeFamilyID As Integer,
                                  ByVal GranteeRelationID As Short,
                                  ByVal GranteeApplication As Short,
                                  ByVal UserRights As String,
                                  ByVal lastOnlineDate As String,
                                  Optional ByVal transaction As DbTransaction = Nothing) As Boolean

        Dim ResultOfSP As Boolean = True

        If thruDate = CDate("12/31/9998") Then
            thruDate = CDate("12/31/9999")
        End If

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.TRIM_REG_PHIAUTH"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@vFAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@vRELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@vTHRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@vGRANTEE_FAMILY_ID", DbType.Int32, GranteeFamilyID)
            DB.AddInParameter(DBCommandWrapper, "@vGRANTEE_RELATION_ID", DbType.Int16, GranteeRelationID)
            DB.AddInParameter(DBCommandWrapper, "@vGRANTEE_APPLICATION", DbType.Int16, GranteeApplication)

            'ByVal Comments As String,
            'DB.AddInParameter(DBCommandWrapper, "@vCOMMENTS", DbType.String, Comments)

            DB.AddInParameter(DBCommandWrapper, "@vUSER_RIGHTS", DbType.String, UserRights)
            DB.AddInParameter(DBCommandWrapper, "@vLAST_ONLINE_DATE", DbType.String, lastOnlineDate)  'Timestamp

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            ResultOfSP = False
            Throw
        Catch ex As SqlClient.SqlException
            ResultOfSP = False
            Stop
        Catch ex As Exception
            ResultOfSP = False
            Throw
        End Try

        Return ResultOfSP
    End Function

    'BOOKMARK: 002d - PHIAuthExtend
    Public Shared Function PHIAuthExtend(ByVal fAMILYID As Integer,
                                  ByVal relationID As Short,
                                  ByVal fromDate As Date,
                                  ByVal thruDate As Date,
                                  ByVal GranteeFamilyID As Integer,
                                  ByVal GranteeRelationID As Short,
                                  ByVal GranteeApplication As Short,
                                  ByVal Comments As String,
                                  ByVal UserRights As String,
                                  ByVal lastOnlineDate As String,
                                  Optional ByVal transaction As DbTransaction = Nothing) As Boolean

        Dim ResultOfSP As Boolean = True

        If thruDate = CDate("12/31/9998") Then
            thruDate = CDate("12/31/9999")
        End If

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.EXTEND_EXISTING_REG_PHIAUTH"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@vFAMILY_ID", DbType.Int32, fAMILYID)
            DB.AddInParameter(DBCommandWrapper, "@vRELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@vFROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@vTHRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@vGRANTEE_FAMILY_ID", DbType.Int32, GranteeFamilyID)
            DB.AddInParameter(DBCommandWrapper, "@vGRANTEE_RELATION_ID", DbType.Int16, GranteeRelationID)
            DB.AddInParameter(DBCommandWrapper, "@vGRANTEE_APPLICATION", DbType.Int16, GranteeApplication)
            DB.AddInParameter(DBCommandWrapper, "@vCOMMENTS", DbType.String, Comments)
            DB.AddInParameter(DBCommandWrapper, "@vUSER_RIGHTS", DbType.String, UserRights)
            DB.AddInParameter(DBCommandWrapper, "@vLAST_ONLINE_DATE", DbType.String, lastOnlineDate)  'for Timestamp

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            ResultOfSP = False
            Throw
        Catch ex As SqlClient.SqlException
            ResultOfSP = False
            Stop
        Catch ex As Exception
            ResultOfSP = False
            Throw
        End Try

        Return ResultOfSP
    End Function

    'BOOKMARK: 002c - PHIAuthTrim
    Public Shared Function PHIAuthTrim(ByVal fAMILYID As Integer,
                                  ByVal relationID As Short,
                                  ByVal fromDate As Date,
                                  ByVal thruDate As Date,
                                  ByVal GranteeFamilyID As Integer,
                                  ByVal GranteeRelationID As Short,
                                  ByVal GranteeApplication As Short,
                                  ByVal Comments As String,
                                  ByVal UserRights As String,
                                  ByVal lastOnlineDate As String,
                                  Optional ByVal transaction As DbTransaction = Nothing) As Boolean

        Dim ResultOfSP As Boolean = True

        If thruDate = CDate("12/31/9998") Then
            thruDate = CDate("12/31/9999")
        End If

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.TRIM_REG_PHIAUTH"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@vFAMILY_ID", DbType.Int32, fAMILYID)
            DB.AddInParameter(DBCommandWrapper, "@vRELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@vTHRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@vGRANTEE_FAMILY_ID", DbType.Int32, GranteeFamilyID)
            DB.AddInParameter(DBCommandWrapper, "@vGRANTEE_RELATION_ID", DbType.Int16, GranteeRelationID)
            DB.AddInParameter(DBCommandWrapper, "@vGRANTEE_APPLICATION", DbType.Int16, GranteeApplication)
            DB.AddInParameter(DBCommandWrapper, "@vCOMMENTS", DbType.String, Comments)
            DB.AddInParameter(DBCommandWrapper, "@vUSER_RIGHTS", DbType.String, UserRights)
            DB.AddInParameter(DBCommandWrapper, "@vLAST_ONLINE_DATE", DbType.String, lastOnlineDate)  'for Timestamp

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            ResultOfSP = False
            Throw
        Catch ex As SqlClient.SqlException
            ResultOfSP = False
            Stop
        Catch ex As Exception
            ResultOfSP = False
            Throw
        End Try

        Return ResultOfSP
    End Function

    '*****************************************************************************************************************************


    Public Shared Function RetrieveRegMasterbySSN(ByVal SSN As Integer) As DataTable
        Try
            Dim DS As DataSet
            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommand As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_REG_MASTER_BY_SSNO"
            dbCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(dbCommand, "@SSNO", DbType.Int32, SSN)

            dbCommand.CommandTimeout = 180

            DS = DB.ExecuteDataSet(dbCommand)
            Dim _dataTable As New DataTable("REG_MASTER")

            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                _dataTable = DS.Tables(0)
                Return _dataTable
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw
            Return Nothing
        End Try
    End Function
    Public Shared Function RetrieveRegMaster(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Try

            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommand As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_REG_MASTER"

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
                    DB.LoadDataSet(DBCommand, ds, "REG_MASTER")
                Else
                    DB.LoadDataSet(DBCommand, ds, "REG_MASTER", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception
            Throw
            Return Nothing
        End Try
    End Function
    Public Shared Function RetrieveRegMaster(ByVal xmlNodeLst As Xml.XmlNodeList, Optional ByVal employeeAccess As Boolean = False, Optional ByVal localAccess As Boolean = False) As DataTable
        Dim DS As DataSet
        Dim SQLSelect As String
        Dim SQLSecuritySelect As String
        Dim SQLFrom As String = ""
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand = Nothing
        Dim SQLSecurity As String = ""
        Dim LastSQL As String = ""

        Try
            Dim StrWhere As String = BuildWhere(xmlNodeLst)
            Dim SQLWhere As String = StrWhere.ToString.Remove(StrWhere.ToString.LastIndexOf("AND"), 3)
            Dim SQLThruWhere As String = StrWhere.ToString.Remove(StrWhere.ToString.LastIndexOf("AND"), 3).Replace("A.", "REG_MAST.")

            If SQLWhere.ToString.Length = 0 Then Return Nothing

            If employeeAccess = False Then
                SQLSecurity += " AND (A.TRUST_SW IS NULL or A.TRUST_SW = 0)"
            End If

            If localAccess = False And employeeAccess = False Then
                SQLSecurity += " AND (A.LOCAL_SW IS NULL or A.LOCAL_SW = 0)"
            End If


            SQLSelect = " SELECT  A.FAMILY_ID, A.RELATION_ID, IFNULL(A.SSNO, 0) AS SSNO, A.PART_SSNO, "
            SQLSelect += "A.FIRST_NAME, A.MIDDLE_INITIAL, A.LAST_NAME, A.GENDER, A.BIRTH_DATE, "
            SQLSelect += " (CAST(YEAR(CURRENT DATE - A.BIRTH_DATE) AS VARCHAR(3)) || ' / ' || CAST(MONTH(CURRENT DATE - A.BIRTH_DATE) AS VARCHAR(2))) AS AGE_AS_Y_M, "
            SQLSelect += "A.TRUST_SW, A.SURVIVING_SPOUSE_SW, A.STEP_SW, A.FOSTER_SW, A.DISABLE_SW, "
            SQLSelect += "A.STUDENT_SW, A.FROM_DATE, A.THRU_DATE, A.MARRIED_DATE, A.MARRIAGE_OVERRIDE_SW, A.DIVORCE_DATE, "
            SQLSelect += "A.DEATH_DATE, A.RELATION, A.LOCAL_SW, "
            SQLSelect += "A.REMARK,A.RISK,A.ADDRESS_SW,A.COBRA_SW, "
            SQLSelect += "A.CREATE_USERID,A.CREATE_DATE,A.BATCH_USERID,A.BATCH_DATE,A.ONLINE_USERID,A.ONLINE_DATE , '' AS STATUS, 0 AS ELIGIBILITY_GAP_COUNT, 0 AS ELIGIBILITY_MAX_GAP_COUNT "

            SQLFrom += "FROM  FDBEL.REG_MASTER AS A "
            SQLFrom += " WHERE ( "
            SQLFrom += SQLWhere ''"(REG_MASTER.FAMILY_ID = vFAMILY_ID)"
            SQLFrom += ") AND "
            SQLFrom += "A.THRU_DATE = (SELECT MAX(REG_MAST.THRU_DATE) FROM FDBEL.REG_MASTER AS REG_MAST"
            SQLFrom += " WHERE ( "
            SQLFrom += SQLThruWhere ''"(REG_MAST.FAMILY_ID = vFAMILY_ID)"
            SQLFrom += ") AND IFNULL(REG_MAST.SSNO, 0) = IFNULL(A.SSNO, 0) AND REG_MAST.RELATION_ID =  A.RELATION_ID)"



            SQLSecuritySelect = " UNION ALL SELECT DISTINCT B.FAMILY_ID,B.RELATION_ID,  B.SSNO, B.PART_SSNO, "
            SQLSecuritySelect += "B.FIRST_NAME, B.MIDDLE_INITIAL,B.LAST_NAME, B.GENDER, B.BIRTH_DATE, "
            SQLSecuritySelect += " '' as AGE_AS_Y_M, "
            SQLSecuritySelect += "B.TRUST_SW,  B.SURVIVING_SPOUSE_SW, B.STEP_SW,  B.FOSTER_SW, B.DISABLE_SW,  "
            SQLSecuritySelect += "B.STUDENT_SW, B.FROM_DATE,  B.THRU_DATE, B.MARRIED_DATE,  B.MARRIAGE_OVERRIDE_SW,  B.DIVORCE_DATE, "
            SQLSecuritySelect += " B.DEATH_DATE, B.RELATION, B.LOCAL_SW, "
            SQLSecuritySelect += "B.REMARK,B.RISK,B.ADDRESS_SW,B.COBRA_SW, "
            SQLSecuritySelect += "B.CREATE_USERID,B.CREATE_DATE, B.BATCH_USERID, B.BATCH_DATE, B.ONLINE_USERID, B.ONLINE_DATE, "
            SQLSecuritySelect += " 'RESTRICTED' as STATUS, 0 AS ELIGIBILITY_GAP_COUNT, 0 AS ELIGIBILITY_MAX_GAP_COUNT  FROM FDBEL.REG_MASTER A "
            SQLSecuritySelect += "LEFT OUTER JOIN"
            SQLSecuritySelect += "(SELECT  0 as FAMILY_ID,0 as RELATION_ID,  0 as SSNO, 0 as PART_SSNO, "
            SQLSecuritySelect += " '' as FIRST_NAME,  '' as MIDDLE_INITIAL,  '' as LAST_NAME,  '' as GENDER,  CAST(NULL AS DATE)  as BIRTH_DATE, "
            SQLSecuritySelect += " 0 as TRUST_SW,  0 as SURVIVING_SPOUSE_SW,  0 as STEP_SW,  0 as FOSTER_SW,  0 as DISABLE_SW,  "
            SQLSecuritySelect += " 0 as STUDENT_SW,  CAST(NULL AS DATE)  as FROM_DATE,  CAST(NULL AS DATE) AS THRU_DATE,  CAST(NULL AS DATE)  as MARRIED_DATE,  0 as  MARRIAGE_OVERRIDE_SW,  CAST(NULL AS DATE) as DIVORCE_DATE,  CAST(NULL AS DATE)  as DEATH_DATE,  '' as RELATION,  0 as LOCAL_SW, "
            SQLSecuritySelect += " '' as REMARK, 0 as RISK, 0 as ADDRESS_SW, 0 as COBRA_SW, "
            SQLSecuritySelect += " '' as CREATE_USERID, CAST(NULL AS TIMESTAMP)  as CREATE_DATE, '' as BATCH_USERID, CAST(NULL AS TIMESTAMP)  as BATCH_DATE, '' as ONLINE_USERID, CAST(NULL AS TIMESTAMP)  as ONLINE_DATE, "
            SQLSecuritySelect += "'RESTRICTED' as STATUS FROM SYSIBM.SYSDUMMY1)B  ON A.FAMILY_ID = B.FAMILY_ID "
            SQLSecuritySelect += " WHERE "
            SQLSecuritySelect += SQLWhere
            SQLSecuritySelect += " FOR READ ONLY "

            LastSQL = SQLSelect + SQLFrom + SQLSecurity + SQLSecuritySelect

            If System.Configuration.ConfigurationManager.AppSettings("LogSQLFileName") IsNot Nothing AndAlso System.Configuration.ConfigurationManager.AppSettings("LogSQLFileName").ToString.Trim.Length > 0 Then
                CMSDALLog.Log("Query Timestamp: " & UFCWGeneral.NowDate & " --Query: " & LastSQL, CMSDALLog.LogDirectory & "\" & String.Format("{0000}", UFCWGeneral.NowDate.Year) & String.Format("{00}", UFCWGeneral.NowDate.Month) & System.Configuration.ConfigurationManager.AppSettings("LogSQLFileName"))
            End If

            DBCommandWrapper = DB.GetStoredProcCommand("FDBEL.RUNIMMEDIATESELECT")
            DB.AddInParameter(DBCommandWrapper, "SQLInput", DbType.String, LastSQL)

            DBCommandWrapper.CommandTimeout = 300

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            If DS.Tables.Count > 0 Then
                Return DS.Tables(0)
            Else
                Return Nothing
            End If
        Catch ex As System.IO.IOException

            MsgBox("Search Request is taking too long and was cancelled. " & Environment.NewLine() & " Try limiting your search by including other search criteria such as a date range.", MsgBoxStyle.Exclamation, "Search TimeOut occurred.")
            Return Nothing

        Catch ex As DB2Exception


            Throw


        Catch ex As Exception


            Throw


        Finally
            If DBCommandWrapper IsNot Nothing Then
                DBCommandWrapper.Dispose()

            End If
            DBCommandWrapper = Nothing

        End Try
    End Function

    Public Shared Function RetrieveSecureRegMasterByFamilyid(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Try

            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommand As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_SECURE_REG_MASTER_BY_FAMILYID"

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommand, "@EMPLOYEE_ACCESS", DbType.Int32, If(UFCWGeneralAD.REGMEmployeeAccess, 1, 0))
            DB.AddOutParameter(DBCommand, "@DUAL_COVERAGE", DbType.Int32, 1)
            DB.AddOutParameter(DBCommand, "@DUAL_COVERAGE_FAMILY_ID", DbType.Int32, 8)
            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If
                ds.Tables(0).TableName = "REG_MASTER"
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, "REG_MASTER")
                Else
                    DB.LoadDataSet(DBCommand, ds, "REG_MASTER", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception
            Throw
            Return Nothing
        End Try
    End Function
    Public Shared Function RetrieveSecureRegMasterByPartSSNO(ByVal partSSNO As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Try

            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommand As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_SECURE_REG_MASTER_BY_PART_SSNO"

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@PART_SSN", DbType.Int64, partSSNO)
            DB.AddInParameter(DBCommand, "@EMPLOYEE_ACCESS", DbType.Int32, If(UFCWGeneralAD.REGMEmployeeAccess, 1, 0))
            DB.AddOutParameter(DBCommand, "@DUAL_COVERAGE", DbType.Int32, 1)
            DB.AddOutParameter(DBCommand, "@DUAL_COVERAGE_FAMILY_ID", DbType.Int32, 8)
            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If
                ds.Tables(0).TableName = "REG_MASTER"
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, "REG_MASTER")
                Else
                    DB.LoadDataSet(DBCommand, ds, "REG_MASTER", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception
            Throw
            Return Nothing
        End Try
    End Function
    Public Shared Function RetrieveSecureRegMasterByPatSSNO(ByVal patSSNO As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Try

            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommand As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_SECURE_REG_MASTER_BY_PAT_SSNO"

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@PAT_SSN", DbType.Int64, patSSNO)
            DB.AddInParameter(DBCommand, "@EMPLOYEE_ACCESS", DbType.Int32, If(UFCWGeneralAD.REGMEmployeeAccess, 1, 0))
            DB.AddOutParameter(DBCommand, "@DUAL_COVERAGE", DbType.Int32, 1)
            DB.AddOutParameter(DBCommand, "@DUAL_COVERAGE_FAMILY_ID", DbType.Int32, 8)
            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If
                ds.Tables(0).TableName = "REG_MASTER"
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, "REG_MASTER")
                Else
                    DB.LoadDataSet(DBCommand, ds, "REG_MASTER", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception
            Throw
            Return Nothing
        End Try
    End Function
    Public Shared Function RetrieveAllControlInfoByFamilyID(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim tablenames() As String = {"REG_ADDRESS", "REG_EMAIL", "REG_PHONE", "REG_REMARKS", "REG_HISTORY", "REG_RISK_EVENTS", "COVERAGE"}

        Dim DB As Database
        Dim DBCommand As DbCommand
        Dim SQLCall As String = "FDBEL.RETRIEVE_ADDRESS_EMAIL_PHONE_REMARK_HISTORY"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int32, familyID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If

                ds.Tables(0).TableName = "REG_ADDRESS"
                ds.Tables(1).TableName = "REG_EMAIL"
                ds.Tables(2).TableName = "REG_PHONE"
                ds.Tables(3).TableName = "REG_REMARKS"
                ds.Tables(4).TableName = "REG_HISTORY"
                ds.Tables(5).TableName = "REG_RISK_EVENTS"
                ds.Tables(6).TableName = "COVERAGE"

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

    Public Shared Function BuildWhere(ByVal xmlNodeLst As Xml.XmlNodeList) As String
        Try

            Dim StrWhere As New StringBuilder
            Dim SQLWhere As String

            For Each nd As XmlNode In xmlNodeLst
                Dim XMLNds As XmlNodeList = nd.SelectNodes("Criterion")
                For Each xmlNd As Xml.XmlNode In XMLNds
                    StrWhere.Append(xmlNd.SelectSingleNode("@Name").Value & " = " & xmlNd.SelectSingleNode("@Value").Value & " AND ")
                Next
            Next

            SQLWhere = StrWhere.ToString
            SQLWhere = SQLWhere.ToUpper.Replace("FAMILYID", "A.FAMILY_ID")
            SQLWhere = SQLWhere.ToUpper.Replace("PARTICIPANTSSN", "A.PART_SSNO")
            SQLWhere = SQLWhere.ToUpper.Replace("LASTNAME", "A.LAST_NAME")
            SQLWhere = SQLWhere.ToUpper.Replace("FIRSTNAME", "A.FIRST_NAME")
            SQLWhere = SQLWhere.ToUpper.Replace("DEPENDENTSSN", "A.SSNO")
            BuildWhere = SQLWhere

        Catch ex As Exception


            Throw

            Return Nothing

        End Try

    End Function
    Public Shared Function InsertRegMaster(ByVal fromDate As Date,
                                      ByVal thruDate As Date, ByVal SSNO As Integer, ByVal partSSNO As Integer, ByVal lastName As String, ByVal firstName As String,
                                      ByVal middleinitial As String, ByVal relation As String, ByVal Gender As String, ByVal BirthDate As Date?,
                                      ByVal DeathDate As Date?, ByVal userRights As String, ByVal remark As String, ByVal risk As Decimal,
                                      ByVal addressSW As Decimal, ByVal trustSW As Decimal, ByVal SurvivingSW As Decimal, ByVal StepSW As Decimal,
                                      ByVal fosterSW As Decimal, ByVal DisableSW As Decimal, ByVal StudentSW As Decimal, ByVal marriedDate As Date?,
                                      ByVal DivorceDate As Date?, ByVal marriageOverrideSW As Decimal, ByVal localSW As Decimal,
                                      ByVal createDate As Date, ByVal transaction As DbTransaction) As Integer

        If CDate(thruDate) = CDate("12/31/9998") Then
            thruDate = CDate("12/31/9999")
        End If

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.INSERT_REG_MASTER_TS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@SSNO", DbType.Int64, SSNO)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSNO", DbType.Int64, partSSNO)
            DB.AddInParameter(DBCommandWrapper, "@LAST_NAME", DbType.String, lastName)
            DB.AddInParameter(DBCommandWrapper, "@FIRST_NAME", DbType.String, firstName)
            DB.AddInParameter(DBCommandWrapper, "@MIDDLE_INITIAL", DbType.String, middleinitial)
            DB.AddInParameter(DBCommandWrapper, "@RELATION", DbType.String, relation)
            DB.AddInParameter(DBCommandWrapper, "@GENDER", DbType.String, Gender)
            DB.AddInParameter(DBCommandWrapper, "@BIRTH_DATE", DbType.Date, BirthDate)
            DB.AddInParameter(DBCommandWrapper, "@DEATH_DATE", DbType.Date, DeathDate)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@REMARK", DbType.String, remark)
            DB.AddInParameter(DBCommandWrapper, "@RISK", DbType.Decimal, risk)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_SW", DbType.Decimal, addressSW)
            DB.AddInParameter(DBCommandWrapper, "@TRUST_SW", DbType.Decimal, trustSW)
            DB.AddInParameter(DBCommandWrapper, "@SURVIVING_SPOUSE_SW", DbType.Decimal, SurvivingSW)
            DB.AddInParameter(DBCommandWrapper, "@STEP_SW", DbType.Decimal, StepSW)
            DB.AddInParameter(DBCommandWrapper, "@FOSTER_SW", DbType.Decimal, fosterSW)
            DB.AddInParameter(DBCommandWrapper, "@DISABLE_SW", DbType.Decimal, DisableSW)
            DB.AddInParameter(DBCommandWrapper, "@STUDENT_SW", DbType.Decimal, StudentSW)
            DB.AddInParameter(DBCommandWrapper, "@MARRIED_DATE", DbType.Date, marriedDate)
            DB.AddInParameter(DBCommandWrapper, "@DIVORCE_DATE", DbType.Date, DivorceDate)
            DB.AddInParameter(DBCommandWrapper, "@MARRIAGE_OVERRIDE_SW", DbType.Decimal, marriageOverrideSW)
            DB.AddInParameter(DBCommandWrapper, "@LOCAL_SW", DbType.Decimal, localSW)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_DATE", DbType.DateTime, createDate)

            DB.AddOutParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, 10)

            If transaction IsNot Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper)
            End If

            Return CInt(DB.GetParameterValue(DBCommandWrapper, "@FAMILY_ID"))

        Catch ex As Exception

            Throw

        End Try
    End Function
    Public Shared Sub UpdateRegMaster(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date,
                                      ByVal thruDate As Date, ByVal ssno As Integer?, ByVal lastName As String, ByVal firstName As String,
                                      ByVal middleinitial As String, ByVal relation As String, ByVal Gender As String, ByVal birthDate As Date?,
                                      ByVal deathDate As Date?, ByVal userRights As String, ByVal remark As String, ByVal risk As Decimal,
                                      ByVal addressSW As Decimal, ByVal trustSW As Decimal, ByVal survivingSW As Decimal, ByVal StepSW As Decimal,
                                      ByVal fosterSW As Decimal, ByVal disableSW As Decimal, ByVal studentSW As Decimal, ByVal marriedDate As Date?,
                                      ByVal divorceDate As Date?, ByVal marriageOverrideSW As Decimal, ByVal localSW As Decimal,
                                      ByVal onlineDate As Date, ByVal originalOnlineDate As Date, ByVal transaction As DbTransaction)

        If thruDate = CDate("12/31/9998") Then
            thruDate = CDate("12/31/9999")
        End If

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_REG_MASTER_TS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@SSNO", DbType.Int64, ssno)
            DB.AddInParameter(DBCommandWrapper, "@LAST_NAME", DbType.String, lastName)
            DB.AddInParameter(DBCommandWrapper, "@FIRST_NAME", DbType.String, firstName)
            DB.AddInParameter(DBCommandWrapper, "@MIDDLE_INITIAL", DbType.String, middleinitial)
            DB.AddInParameter(DBCommandWrapper, "@RELATION", DbType.String, relation)
            DB.AddInParameter(DBCommandWrapper, "@GENDER", DbType.String, Gender)
            DB.AddInParameter(DBCommandWrapper, "@BIRTH_DATE", DbType.Date, birthDate)
            DB.AddInParameter(DBCommandWrapper, "@DEATH_DATE", DbType.Date, deathDate)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@REMARK", DbType.String, remark)
            DB.AddInParameter(DBCommandWrapper, "@RISK", DbType.Decimal, risk)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_SW", DbType.Decimal, addressSW)
            DB.AddInParameter(DBCommandWrapper, "@TRUST_SW", DbType.Decimal, trustSW)
            DB.AddInParameter(DBCommandWrapper, "@SURVIVING_SPOUSE_SW", DbType.Decimal, survivingSW)
            DB.AddInParameter(DBCommandWrapper, "@STEP_SW", DbType.Decimal, StepSW)
            DB.AddInParameter(DBCommandWrapper, "@FOSTER_SW", DbType.Decimal, fosterSW)
            DB.AddInParameter(DBCommandWrapper, "@DISABLE_SW", DbType.Decimal, disableSW)
            DB.AddInParameter(DBCommandWrapper, "@STUDENT_SW", DbType.Decimal, studentSW)
            DB.AddInParameter(DBCommandWrapper, "@MARRIED_DATE", DbType.Date, marriedDate)
            DB.AddInParameter(DBCommandWrapper, "@DIVORCE_DATE", DbType.Date, divorceDate)
            DB.AddInParameter(DBCommandWrapper, "@MARRIAGE_OVERRIDE_SW", DbType.Decimal, marriageOverrideSW)
            DB.AddInParameter(DBCommandWrapper, "@LOCAL_SW", DbType.Decimal, localSW)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_DATE", DbType.DateTime, onlineDate)
            DB.AddInParameter(DBCommandWrapper, "@ORIGINAL_ONLINE_DATE", DbType.DateTime, originalOnlineDate)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

        Catch ex As Exception

            Throw

        End Try
    End Sub
    Public Shared Sub DeleteRegMaster(ByVal familyID As Integer, ByVal relationID As Integer, ByVal transaction As DbTransaction)
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.DELETE_REGMASTER_BY_FAMILYID_RELATIONID"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)
        Catch ex As Exception

            Throw

        End Try
    End Sub
    Public Shared Function AddRowToRegMaster(ByVal familyID As Integer, ByVal fromDate As Date,
                                      ByVal thruDate As Date, ByVal SSNO As Integer?, ByVal partSSNO As Integer, ByVal lastName As String, ByVal firstName As String,
                                      ByVal middleinitial As String, ByVal relation As String, ByVal Gender As String, ByVal BirthDate As Date?,
                                      ByVal DeathDate As Date?, ByVal userRights As String, ByVal remark As String, ByVal risk As Decimal,
                                      ByVal addressSW As Decimal, ByVal trustSW As Decimal, ByVal SurvivingSW As Decimal, ByVal StepSW As Decimal,
                                      ByVal fosterSW As Decimal, ByVal DisableSW As Decimal, ByVal StudentSW As Decimal, ByVal marriedDate As Date?,
                                      ByVal DivorceDate As Date?, ByVal marriageOverrideSW As Decimal, ByVal localSW As Decimal,
                                      ByVal createDate As Date, ByVal transaction As DbTransaction) As Short

        If thruDate = CDate("12/31/9998") Then
            thruDate = CDate("12/31/9999")
        End If

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.ADD_REG_MASTER_TS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@SSNO", DbType.Int64, SSNO)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSNO", DbType.Int64, partSSNO)
            DB.AddInParameter(DBCommandWrapper, "@LAST_NAME", DbType.String, lastName)
            DB.AddInParameter(DBCommandWrapper, "@FIRST_NAME", DbType.String, firstName)
            DB.AddInParameter(DBCommandWrapper, "@MIDDLE_INITIAL", DbType.String, middleinitial)
            DB.AddInParameter(DBCommandWrapper, "@RELATION", DbType.String, relation)
            DB.AddInParameter(DBCommandWrapper, "@GENDER", DbType.String, Gender)
            DB.AddInParameter(DBCommandWrapper, "@BIRTH_DATE", DbType.Date, BirthDate)
            DB.AddInParameter(DBCommandWrapper, "@DEATH_DATE", DbType.Date, DeathDate)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@REMARK", DbType.String, remark)
            DB.AddInParameter(DBCommandWrapper, "@RISK", DbType.Decimal, risk)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_SW", DbType.Decimal, addressSW)
            DB.AddInParameter(DBCommandWrapper, "@TRUST_SW", DbType.Decimal, trustSW)
            DB.AddInParameter(DBCommandWrapper, "@SURVIVING_SPOUSE_SW", DbType.Decimal, SurvivingSW)
            DB.AddInParameter(DBCommandWrapper, "@STEP_SW", DbType.Decimal, StepSW)
            DB.AddInParameter(DBCommandWrapper, "@FOSTER_SW", DbType.Decimal, fosterSW)
            DB.AddInParameter(DBCommandWrapper, "@DISABLE_SW", DbType.Decimal, DisableSW)
            DB.AddInParameter(DBCommandWrapper, "@STUDENT_SW", DbType.Decimal, StudentSW)
            DB.AddInParameter(DBCommandWrapper, "@MARRIED_DATE", DbType.Date, marriedDate)
            DB.AddInParameter(DBCommandWrapper, "@DIVORCE_DATE", DbType.Date, DivorceDate)
            DB.AddInParameter(DBCommandWrapper, "@MARRIAGE_OVERRIDE_SW", DbType.Decimal, marriageOverrideSW)
            DB.AddInParameter(DBCommandWrapper, "@LOCAL_SW", DbType.Decimal, localSW)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_DATE", DbType.DateTime, createDate)

            DB.AddOutParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, 10)

            If Not transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper)
            End If

            Return CShort(DB.GetParameterValue(DBCommandWrapper, "@RELATION_ID"))

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

    Public Shared Function RetrieveRegAddress(ByVal familyID As Integer?) As DataRow
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
            DT = Nothing

        End Try
    End Function

    Public Shared Function RetrieveRegaddressByFamilyID(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Try

            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommand As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_REGADDRESS_BY_FAMILYID"

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
                    DB.LoadDataSet(DBCommand, ds, "REG_ADDRESS")
                Else
                    DB.LoadDataSet(DBCommand, ds, "REG_ADDRESS", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception
            Throw
        End Try
    End Function
    'new address control
    Public Shared Sub INSERTRegAddress(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date, ByVal thruDate As Date,
                                                ByVal partSSNO As Integer,
                                                ByVal foreignSW As Decimal, ByVal country As String, ByVal address1 As String, ByVal address2 As String,
                                                ByVal city As String, ByVal State As String, ByVal zip1 As Integer?, ByVal zip2 As Integer?,
                                                ByVal userRights As String, ByVal addressType As Integer, ByVal priority As Integer,
                                                ByVal accountid As String, ByVal entityname As String, ByVal addressorigin As String, ByVal additionaldeliveryinfo As String, ByVal foreignProvince As String, ByVal foreignPostCode As String, Optional ByVal transaction As DbTransaction = Nothing)

        If thruDate = CDate("12/31/9998") Then
            thruDate = CDate("12/31/9999")
        End If

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.INSERT_REG_ADDRESS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSNO", DbType.Int32, partSSNO)

            DB.AddInParameter(DBCommandWrapper, "@FOREIGN_SW", DbType.Decimal, foreignSW)
            DB.AddInParameter(DBCommandWrapper, "@COUNTRY", DbType.String, UFCWGeneral.ToNullStringHandler(country))
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS1", DbType.String, address1)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS2", DbType.String, UFCWGeneral.ToNullStringHandler(address2))
            DB.AddInParameter(DBCommandWrapper, "@CITY", DbType.String, city)
            DB.AddInParameter(DBCommandWrapper, "@STATE", DbType.String, UFCWGeneral.ToNullStringHandler(State))
            DB.AddInParameter(DBCommandWrapper, "@ZIP1", DbType.Int32, UFCWGeneral.ToNullIntegerHandler(zip1))
            DB.AddInParameter(DBCommandWrapper, "@ZIP2", DbType.Int32, UFCWGeneral.ToNullIntegerHandler(zip2))
            DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_TYPE", DbType.Int64, addressType)
            ' DB.AddInParameter(DBCommandWrapper, "@PRIORITY", DbType.Int64, priority)
            DB.AddInParameter(DBCommandWrapper, "@ACCOUNT_ID", DbType.String, UFCWGeneral.ToNullStringHandler(accountid))
            DB.AddInParameter(DBCommandWrapper, "@ENTITY_NAME", DbType.String, UFCWGeneral.ToNullStringHandler(entityname))
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_ORIGIN", DbType.String, addressorigin)
            DB.AddInParameter(DBCommandWrapper, "@ADDITIONAL_DELIVERY_INFO", DbType.String, UFCWGeneral.ToNullStringHandler(additionaldeliveryinfo))
            DB.AddInParameter(DBCommandWrapper, "@FOREIGN_PROVINCE", DbType.String, UFCWGeneral.ToNullStringHandler(foreignProvince))
            DB.AddInParameter(DBCommandWrapper, "@FOREIGN_POSTCODE", DbType.String, UFCWGeneral.ToNullStringHandler(foreignPostCode))

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub INSERTRegAddressWithTS(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date, ByVal thruDate As Date,
                                                ByVal partSSNO As Integer,
                                                ByVal foreignSW As Decimal, ByVal country As String, ByVal address1 As String, ByVal address2 As String,
                                                ByVal city As String, ByVal State As String, ByVal zip1 As Integer?, ByVal zip2 As Integer?,
                                                ByVal userRights As String, ByVal addressType As Integer, ByVal priority As Integer,
                                                ByVal accountid As String, ByVal entityname As String, ByVal addressorigin As String, ByVal additionaldeliveryinfo As String, ByVal foreignProvince As String,
                                                ByVal foreignPostCode As String, ByVal createDate As Date, Optional ByVal transaction As DbTransaction = Nothing)

        If thruDate = CDate("12/31/9998") Then
            thruDate = CDate("12/31/9999")
        End If

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.INSERT_REG_ADDRESS_WITH_TS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSNO", DbType.Int32, partSSNO)

            DB.AddInParameter(DBCommandWrapper, "@FOREIGN_SW", DbType.Decimal, foreignSW)
            DB.AddInParameter(DBCommandWrapper, "@COUNTRY", DbType.String, UFCWGeneral.ToNullStringHandler(country))
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS1", DbType.String, address1)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS2", DbType.String, UFCWGeneral.ToNullStringHandler(address2))
            DB.AddInParameter(DBCommandWrapper, "@CITY", DbType.String, city)
            DB.AddInParameter(DBCommandWrapper, "@STATE", DbType.String, UFCWGeneral.ToNullStringHandler(State))
            DB.AddInParameter(DBCommandWrapper, "@ZIP1", DbType.Int32, UFCWGeneral.ToNullIntegerHandler(zip1))
            DB.AddInParameter(DBCommandWrapper, "@ZIP2", DbType.Int32, UFCWGeneral.ToNullIntegerHandler(zip2))
            DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_TYPE", DbType.Int64, addressType)
            ' DB.AddInParameter(DBCommandWrapper, "@PRIORITY", DbType.Int64, priority)
            DB.AddInParameter(DBCommandWrapper, "@ACCOUNT_ID", DbType.String, UFCWGeneral.ToNullStringHandler(accountid))
            DB.AddInParameter(DBCommandWrapper, "@ENTITY_NAME", DbType.String, UFCWGeneral.ToNullStringHandler(entityname))
            'DB.AddInParameter(DBCommandWrapper, "@ADDRESS_ORIGIN", DbType.String, addressorigin)
            DB.AddInParameter(DBCommandWrapper, "@ADDITIONAL_DELIVERY_INFO", DbType.String, UFCWGeneral.ToNullStringHandler(additionaldeliveryinfo))
            DB.AddInParameter(DBCommandWrapper, "@FOREIGN_PROVINCE", DbType.String, UFCWGeneral.ToNullStringHandler(foreignProvince))
            DB.AddInParameter(DBCommandWrapper, "@FOREIGN_POSTCODE", DbType.String, UFCWGeneral.ToNullStringHandler(foreignPostCode))
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_DATE", DbType.DateTime, createDate)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub UpdateRegAddress(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date, ByVal thruDate As Date, ByVal partSSNO As Integer, ByVal phone_Country1 As Decimal, ByVal phone_Area1 As Decimal, ByVal phone_Number1 As Decimal,
                                        ByVal phone_Country2 As Decimal, ByVal phone_Area2 As Decimal, ByVal phone_Number2 As Decimal, ByVal foreignSW As Decimal, ByVal country As String, ByVal address1 As String, ByVal address2 As String,
                                        ByVal city As String, ByVal State As String, ByVal zip1 As Integer?, ByVal zip2 As Integer?,
                                        ByVal approveSW As Decimal?, ByVal checkCountry As String, ByVal checkAddress1 As String, ByVal checkAddress2 As String,
                                        ByVal checkCity As String, ByVal checkState As String, ByVal checkZIP1 As Integer?, ByVal checkZIP2 As Integer?,
                                        ByVal priority As Integer, ByVal accountID As String, ByVal entityName As String, ByVal addressOrigin As String,
                                        ByVal userRights As String, ByVal addressType As Integer, ByVal archive As Integer, ByVal additionalDeliveryInfo As String,
                                        ByVal onlineDate As Date, ByVal foreignProvince As String, ByVal foreignPostCode As String, ByVal addstatus As String, Optional ByVal transaction As DbTransaction = Nothing)


        If thruDate = CDate("12/31/9998") Then
            thruDate = CDate("12/31/9999")
        End If

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_REG_ADDRESS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSNO", DbType.Int32, partSSNO)

            DB.AddInParameter(DBCommandWrapper, "@PHONE_COUNTRY1", DbType.Decimal, DBNull.Value)
            DB.AddInParameter(DBCommandWrapper, "@PHONE_AREA1", DbType.Decimal, DBNull.Value)
            DB.AddInParameter(DBCommandWrapper, "@PHONE_NUMBER1", DbType.Decimal, DBNull.Value)
            DB.AddInParameter(DBCommandWrapper, "@PHONE_COUNTRY2", DbType.Decimal, DBNull.Value)
            DB.AddInParameter(DBCommandWrapper, "@PHONE_AREA2", DbType.Decimal, DBNull.Value)
            DB.AddInParameter(DBCommandWrapper, "@PHONE_NUMBER2", DbType.Decimal, DBNull.Value)

            DB.AddInParameter(DBCommandWrapper, "@FOREIGN_SW", DbType.Decimal, foreignSW)
            DB.AddInParameter(DBCommandWrapper, "@COUNTRY", DbType.String, UFCWGeneral.ToNullStringHandler(country))
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS1", DbType.String, address1)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS2", DbType.String, UFCWGeneral.ToNullStringHandler(address2))
            DB.AddInParameter(DBCommandWrapper, "@CITY", DbType.String, city)
            DB.AddInParameter(DBCommandWrapper, "@STATE", DbType.String, UFCWGeneral.ToNullStringHandler(State))
            DB.AddInParameter(DBCommandWrapper, "@ZIP1", DbType.Int32, UFCWGeneral.ToNullIntegerHandler(zip1))
            DB.AddInParameter(DBCommandWrapper, "@ZIP2", DbType.Int32, UFCWGeneral.ToNullIntegerHandler(zip2))

            DB.AddInParameter(DBCommandWrapper, "@APPROVAL_REQUIRED_SW", DbType.Decimal, approveSW)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_COUNTRY", DbType.String, DBNull.Value)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_ADDRESS1", DbType.String, DBNull.Value)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_ADDRESS2", DbType.String, DBNull.Value)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_CITY", DbType.String, DBNull.Value)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_STATE", DbType.String, DBNull.Value)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_ZIP1", DbType.Int64, DBNull.Value)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_ZIP2", DbType.Int64, DBNull.Value)

            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_TYPE", DbType.Int64, addressType)
            DB.AddInParameter(DBCommandWrapper, "@ACCOUNT_ID", DbType.String, UFCWGeneral.ToNullStringHandler(accountID))
            DB.AddInParameter(DBCommandWrapper, "@ENTITY_NAME", DbType.String, UFCWGeneral.ToNullStringHandler(entityName))
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_ORIGIN", DbType.String, UFCWGeneral.ToNullStringHandler(addressOrigin))
            DB.AddInParameter(DBCommandWrapper, "@PRIORITY", DbType.Int32, priority)
            DB.AddInParameter(DBCommandWrapper, "@ARCHIVE_SW", DbType.Int16, archive)
            DB.AddInParameter(DBCommandWrapper, "@ADDITIONAL_DELIVERY_INFO", DbType.String, UFCWGeneral.ToNullStringHandler(additionalDeliveryInfo))
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_DATE", DbType.DateTime, onlineDate)
            DB.AddInParameter(DBCommandWrapper, "@FOREIGN_PROVINCE", DbType.String, UFCWGeneral.ToNullStringHandler(foreignProvince))
            DB.AddInParameter(DBCommandWrapper, "@FOREIGN_POSTCODE", DbType.String, UFCWGeneral.ToNullStringHandler(foreignPostCode))
            DB.AddInParameter(DBCommandWrapper, "@STATUS", DbType.String, addstatus)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub UpdateRegAddressByStatus(ByVal familyID As Integer, ByVal relationID As Integer, ByVal origFromDate As Date, ByVal origThruDate As Date, ByVal fromDate As Date, ByVal thruDate As Date, ByVal partSSNO As Integer, ByVal phone_Country1 As Decimal, ByVal phone_Area1 As Decimal, ByVal phone_Number1 As Decimal,
                                                ByVal phone_Country2 As Decimal, ByVal phone_Area2 As Decimal, ByVal phone_Number2 As Decimal, ByVal foreignSW As Decimal, ByVal country As String, ByVal address1 As String, ByVal address2 As String,
                                                ByVal city As String, ByVal State As String, ByVal zip1 As Integer?, ByVal zip2 As Integer?,
                                                ByVal approveSW As Decimal?, ByVal checkCountry As String, ByVal checkAddress1 As String, ByVal checkAddress2 As String,
                                                ByVal checkCity As String, ByVal checkState As String, ByVal checkZIP1 As Integer?, ByVal checkZIP2 As Integer?,
                                                ByVal priority As Integer, ByVal accountID As String, ByVal entityName As String, ByVal addressOrigin As String,
                                                ByVal userRights As String, ByVal addressType As Integer, ByVal archive As Integer, ByVal suppressMail As Integer, ByVal additionalDeliveryInfo As String,
                                                ByVal originalOnlineDate As Date, ByVal foreignProvince As String, ByVal foreignPostCode As String, ByVal addstatus As String, ByVal onlineDate As Date,
                                                Optional ByVal transaction As DbTransaction = Nothing)


        If thruDate = CDate("12/31/9998") Then
            thruDate = CDate("12/31/9999")
        End If

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_REG_ADDRESS_BY_STATUS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)
            DB.AddInParameter(DBCommandWrapper, "@ORIG_FROM_DATE", DbType.Date, origFromDate)
            DB.AddInParameter(DBCommandWrapper, "@ORIG_THRU_DATE", DbType.Date, origThruDate)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSNO", DbType.Int32, partSSNO)

            DB.AddInParameter(DBCommandWrapper, "@PHONE_COUNTRY1", DbType.Decimal, DBNull.Value)
            DB.AddInParameter(DBCommandWrapper, "@PHONE_AREA1", DbType.Decimal, DBNull.Value)
            DB.AddInParameter(DBCommandWrapper, "@PHONE_NUMBER1", DbType.Decimal, DBNull.Value)
            DB.AddInParameter(DBCommandWrapper, "@PHONE_COUNTRY2", DbType.Decimal, DBNull.Value)
            DB.AddInParameter(DBCommandWrapper, "@PHONE_AREA2", DbType.Decimal, DBNull.Value)
            DB.AddInParameter(DBCommandWrapper, "@PHONE_NUMBER2", DbType.Decimal, DBNull.Value)

            DB.AddInParameter(DBCommandWrapper, "@FOREIGN_SW", DbType.Decimal, foreignSW)
            DB.AddInParameter(DBCommandWrapper, "@COUNTRY", DbType.String, UFCWGeneral.ToNullStringHandler(country))
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS1", DbType.String, address1)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS2", DbType.String, UFCWGeneral.ToNullStringHandler(address2))
            DB.AddInParameter(DBCommandWrapper, "@CITY", DbType.String, city)
            DB.AddInParameter(DBCommandWrapper, "@STATE", DbType.String, UFCWGeneral.ToNullStringHandler(State))
            DB.AddInParameter(DBCommandWrapper, "@ZIP1", DbType.Int32, UFCWGeneral.ToNullIntegerHandler(zip1))
            DB.AddInParameter(DBCommandWrapper, "@ZIP2", DbType.Int32, UFCWGeneral.ToNullIntegerHandler(zip2))

            DB.AddInParameter(DBCommandWrapper, "@APPROVAL_REQUIRED_SW", DbType.Decimal, approveSW)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_COUNTRY", DbType.String, DBNull.Value)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_ADDRESS1", DbType.String, DBNull.Value)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_ADDRESS2", DbType.String, DBNull.Value)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_CITY", DbType.String, DBNull.Value)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_STATE", DbType.String, DBNull.Value)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_ZIP1", DbType.Int64, DBNull.Value)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_ZIP2", DbType.Int64, DBNull.Value)

            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_TYPE", DbType.Int64, addressType)
            DB.AddInParameter(DBCommandWrapper, "@ACCOUNT_ID", DbType.String, UFCWGeneral.ToNullStringHandler(accountID))
            DB.AddInParameter(DBCommandWrapper, "@ENTITY_NAME", DbType.String, UFCWGeneral.ToNullStringHandler(entityName))
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_ORIGIN", DbType.String, UFCWGeneral.ToNullStringHandler(addressOrigin))
            DB.AddInParameter(DBCommandWrapper, "@PRIORITY", DbType.Int32, priority)
            DB.AddInParameter(DBCommandWrapper, "@ARCHIVE_SW", DbType.Int16, archive)
            DB.AddInParameter(DBCommandWrapper, "@SUPPRESS_MAIL_SW", DbType.Int16, suppressMail)
            DB.AddInParameter(DBCommandWrapper, "@ADDITIONAL_DELIVERY_INFO", DbType.String, UFCWGeneral.ToNullStringHandler(additionalDeliveryInfo))
            DB.AddInParameter(DBCommandWrapper, "@ORIGINAL_ONLINE_DATE", DbType.DateTime, originalOnlineDate)
            DB.AddInParameter(DBCommandWrapper, "@FOREIGN_PROVINCE", DbType.String, UFCWGeneral.ToNullStringHandler(foreignProvince))
            DB.AddInParameter(DBCommandWrapper, "@FOREIGN_POSTCODE", DbType.String, UFCWGeneral.ToNullStringHandler(foreignPostCode))
            DB.AddInParameter(DBCommandWrapper, "@STATUS", DbType.String, addstatus)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_DATE", DbType.DateTime, onlineDate)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub UpdateRegAddress(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date, ByVal thruDate As Date,
                                              ByVal partSSNO As Integer,
                                              ByVal foreignSW As Decimal, ByVal country As String, ByVal address1 As String, ByVal address2 As String,
                                              ByVal city As String, ByVal State As String, ByVal zip1 As Integer?, ByVal zip2 As Integer?,
                                              ByVal checkForeignSW As Decimal, ByVal checkCountry As String, ByVal checkAddress1 As String, ByVal checkAddress2 As String,
                                              ByVal checkCity As String, ByVal checkState As String, ByVal checkZIP1 As Integer?, ByVal checkZIP2 As Integer?,
                                              ByVal userRights As String, ByVal addressType As Integer, ByVal originalFromDate As Date, ByVal originalAddressType As Integer, Optional ByVal transaction As DbTransaction = Nothing)

        If thruDate = CDate("12/31/9998") Then
            thruDate = CDate("12/31/9999")
        End If

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_REG_ADDRESS_NOPHONEPARAMS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSNO", DbType.Int64, partSSNO)

            DB.AddInParameter(DBCommandWrapper, "@FOREIGN_SW", DbType.Decimal, foreignSW)
            DB.AddInParameter(DBCommandWrapper, "@COUNTRY", DbType.String, country)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS1", DbType.String, address1)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS2", DbType.String, address2)
            DB.AddInParameter(DBCommandWrapper, "@CITY", DbType.String, city)
            DB.AddInParameter(DBCommandWrapper, "@STATE", DbType.String, State)
            DB.AddInParameter(DBCommandWrapper, "@ZIP1", DbType.Int64, UFCWGeneral.ToNullIntegerHandler(zip1))
            DB.AddInParameter(DBCommandWrapper, "@ZIP2", DbType.Int64, UFCWGeneral.ToNullIntegerHandler(zip2))

            DB.AddInParameter(DBCommandWrapper, "@CHECK_FOREIGN_SW", DbType.Decimal, checkForeignSW)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_COUNTRY", DbType.String, checkCountry)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_ADDRESS1", DbType.String, checkAddress1)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_ADDRESS2", DbType.String, checkAddress2)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_CITY", DbType.String, checkCity)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_STATE", DbType.String, checkState)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_ZIP1", DbType.Int64, checkZIP1)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_ZIP2", DbType.Int64, checkZIP2)

            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_TYPE", DbType.Int64, addressType)
            DB.AddInParameter(DBCommandWrapper, "@ORIFROM_DATE", DbType.Date, originalFromDate)
            DB.AddInParameter(DBCommandWrapper, "@ORIADDRESS_TYPE", DbType.Int64, originalAddressType)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub

    'Public Shared Sub DeleteRegAddress(ByVal familyID As Integer, ByVal relationID As Integer, ByVal from_Date As Date, ByVal address_Type As Integer, ByVal transaction As DbTransaction)
    '    Dim DB  As Database = CMSDALCommon.CreateDatabase()
    '    Dim DBCommandWrapper As DbCommand
    '    Dim SQLCall As String

    '    Try
    '        SQLCall = CMSDALCommon.GetDatabaseName(Nothing) & "." & "DELETE_REGADDRESS_BY_FAMID_RELID_FRDT_ADDRTYPE"

    '        DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

    '        DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
    '        DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
    '        DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, from_Date)
    '        DB.AddInParameter(DBCommandWrapper, "@ADDRESS_TYPE", DbType.Int64, address_Type)

    '        If transaction Is Nothing Then
    '            DB.ExecuteNonQuery(DBCommandWrapper)
    '        Else
    '            DB.ExecuteNonQuery(DBCommandWrapper, transaction)
    '        End If
    '    Catch ex As Exception
    '        Dim Rethrow As Boolean = True
    '        If (Rethrow) Then
    '            Throw
    '        End If
    '    End Try
    'End Sub
    Public Shared Sub DeleteRegAddress(ByVal familyID As Integer, ByVal relationID As Integer, ByVal from_Date As Date, ByVal address_Type As Integer, ByVal originalOnlineDate As Date, ByVal transaction As DbTransaction)
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.DELETE_REGADDRESS_BY_FAMILYID_RELATIONID_LASTUPDT"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, from_Date)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_TYPE", DbType.Int32, address_Type)
            DB.AddInParameter(DBCommandWrapper, "@ORIGINAL_ONLINE_DATE", DbType.DateTime, originalOnlineDate)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub DeleteRegAddress(ByVal familyID As Integer, ByVal relationID As Integer, ByVal transaction As DbTransaction)
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.DELETE_REGADDRESS_BY_FAMILYID_RELATIONID"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    'new address control
    Public Shared Sub AddNewRegAddress(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date, ByVal thruDate As Date,
                                                ByVal partSSNO As Integer, ByVal foreignSW As Decimal, ByVal country As String, ByVal address1 As String, ByVal address2 As String,
                                                ByVal city As String, ByVal State As String, ByVal zip1 As Integer?, ByVal zip2 As Integer?,
                                                ByVal userRights As String, ByVal addressType As Integer, ByVal priority As Integer, ByVal accountID As String, ByVal entityName As String,
                                                ByVal addressOrigin As String, ByVal additionalDeliveryInfo As String, ByVal foreignProvince As String, ByVal foreignPostCode As String,
                                                Optional ByVal transaction As DbTransaction = Nothing)

        If thruDate = CDate("12/31/9998") Then
            thruDate = CDate("12/31/9999")
        End If

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.ADD_NEW_REG_ADDRESS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSNO", DbType.Int64, partSSNO)
            DB.AddInParameter(DBCommandWrapper, "@FOREIGN_SW", DbType.Decimal, foreignSW)

            DB.AddInParameter(DBCommandWrapper, "@COUNTRY", DbType.String, UFCWGeneral.ToNullStringHandler(country))
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS1", DbType.String, address1)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS2", DbType.String, UFCWGeneral.ToNullStringHandler(address2))
            DB.AddInParameter(DBCommandWrapper, "@CITY", DbType.String, city)
            DB.AddInParameter(DBCommandWrapper, "@STATE", DbType.String, UFCWGeneral.ToNullStringHandler(State))
            DB.AddInParameter(DBCommandWrapper, "@ZIP1", DbType.Int32, UFCWGeneral.ToNullIntegerHandler(zip1))
            DB.AddInParameter(DBCommandWrapper, "@ZIP2", DbType.Int32, UFCWGeneral.ToNullIntegerHandler(zip2))
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_TYPE", DbType.Int64, addressType)

            DB.AddInParameter(DBCommandWrapper, "@PRIORITY", DbType.Int32, priority)
            DB.AddInParameter(DBCommandWrapper, "@ACCOUNT_ID", DbType.String, UFCWGeneral.ToNullStringHandler(accountID))
            DB.AddInParameter(DBCommandWrapper, "@ENTITY_NAME", DbType.String, UFCWGeneral.ToNullStringHandler(entityName))
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_ORIGIN", DbType.String, UFCWGeneral.ToNullStringHandler(addressOrigin))
            DB.AddInParameter(DBCommandWrapper, "@ADDITIONAL_DELIVERY_INFO", DbType.String, UFCWGeneral.ToNullStringHandler(additionalDeliveryInfo))
            DB.AddInParameter(DBCommandWrapper, "@FOREIGN_PROVINCE", DbType.String, UFCWGeneral.ToNullStringHandler(foreignProvince))
            DB.AddInParameter(DBCommandWrapper, "@FOREIGN_POSTCODE", DbType.String, UFCWGeneral.ToNullStringHandler(foreignPostCode))

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As SqlClient.SqlException
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub AddNewRegAddressWithTS(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date, ByVal thruDate As Date,
                                                ByVal partSSNO As Integer, ByVal foreignSW As Decimal, ByVal country As String, ByVal address1 As String, ByVal address2 As String,
                                                ByVal city As String, ByVal State As String, ByVal zip1 As Integer?, ByVal zip2 As Integer?,
                                                ByVal userRights As String, ByVal addressType As Integer, ByVal priority As Integer, ByVal accountID As String, ByVal entityName As String,
                                                ByVal addressOrigin As String, ByVal additionalDeliveryInfo As String, ByVal foreignProvince As String, ByVal foreignPostCode As String,
                                                ByVal createDate As Date, Optional ByVal transaction As DbTransaction = Nothing)

        If thruDate = CDate("12/31/9998") Then
            thruDate = CDate("12/31/9999")
        End If

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.ADD_NEW_REG_ADDRESS_WITH_TS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSNO", DbType.Int64, partSSNO)
            DB.AddInParameter(DBCommandWrapper, "@FOREIGN_SW", DbType.Decimal, foreignSW)

            DB.AddInParameter(DBCommandWrapper, "@COUNTRY", DbType.String, UFCWGeneral.ToNullStringHandler(country))
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS1", DbType.String, address1)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS2", DbType.String, UFCWGeneral.ToNullStringHandler(address2))
            DB.AddInParameter(DBCommandWrapper, "@CITY", DbType.String, city)
            DB.AddInParameter(DBCommandWrapper, "@STATE", DbType.String, UFCWGeneral.ToNullStringHandler(State))
            DB.AddInParameter(DBCommandWrapper, "@ZIP1", DbType.Int32, UFCWGeneral.ToNullIntegerHandler(zip1))
            DB.AddInParameter(DBCommandWrapper, "@ZIP2", DbType.Int32, UFCWGeneral.ToNullIntegerHandler(zip2))
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_TYPE", DbType.Int64, addressType)

            DB.AddInParameter(DBCommandWrapper, "@PRIORITY", DbType.Int32, priority)
            DB.AddInParameter(DBCommandWrapper, "@ACCOUNT_ID", DbType.String, UFCWGeneral.ToNullStringHandler(accountID))
            DB.AddInParameter(DBCommandWrapper, "@ENTITY_NAME", DbType.String, UFCWGeneral.ToNullStringHandler(entityName))
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_ORIGIN", DbType.String, UFCWGeneral.ToNullStringHandler(addressOrigin))
            DB.AddInParameter(DBCommandWrapper, "@ADDITIONAL_DELIVERY_INFO", DbType.String, UFCWGeneral.ToNullStringHandler(additionalDeliveryInfo))
            DB.AddInParameter(DBCommandWrapper, "@FOREIGN_PROVINCE", DbType.String, UFCWGeneral.ToNullStringHandler(foreignProvince))
            DB.AddInParameter(DBCommandWrapper, "@FOREIGN_POSTCODE", DbType.String, UFCWGeneral.ToNullStringHandler(foreignPostCode))
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_DATE", DbType.DateTime, createDate)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As SqlClient.SqlException
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub UpdateRegMasterFromRegAddress(ByVal familyID As Integer, ByVal relationID As Integer, ByVal userRights As String, ByVal transaction As DbTransaction)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_REG_MASTER_FROM_REG_ADDRESS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)
        Catch ex As Exception

            Throw

        End Try
    End Sub

    Public Shared Sub INSERTWEBRegistration(ByVal familyID As Integer, ByVal relationID As Integer,
                                            ByVal userName As String, ByVal pwHash As String, ByVal email As String, ByVal recoveryEmail As String,
                                            ByVal recoveryPhone As String, ByVal phoneType As Decimal?, ByVal smsOk As Decimal?, ByVal autodialOK As Decimal?,
                                            ByVal securityQuestions As SecureString,
                                            ByVal deactivated As Decimal?,
                                            ByVal registrationCompleteddt As Date?,
                                            ByVal source As String, ByVal userRights As String, ByVal lastUpdt As Date?,
                                            Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase()

            SQLCall = "FDBEL.CREATE_REGISTRATION_INFO"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)

            DB.AddInParameter(DBCommandWrapper, "@USERNAME", DbType.String, UFCWGeneral.ToNullStringHandler(userName.Trim))
            DB.AddInParameter(DBCommandWrapper, "@PWHASH", DbType.String, UFCWGeneral.ToNullStringHandler(pwHash))
            DB.AddInParameter(DBCommandWrapper, "@EMAIL", DbType.String, UFCWGeneral.ToNullStringHandler(email))
            DB.AddInParameter(DBCommandWrapper, "@RECOVERY_EMAIL", DbType.String, UFCWGeneral.ToNullStringHandler(recoveryEmail))
            DB.AddInParameter(DBCommandWrapper, "@RECOVERY_PHONE", DbType.String, UFCWGeneral.ToNullStringHandler(recoveryPhone))
            DB.AddInParameter(DBCommandWrapper, "@PHONE_TYPE", DbType.Decimal, UFCWGeneral.ToNullDecimalHandler(phoneType))
            DB.AddInParameter(DBCommandWrapper, "@SMS_OK_SW", DbType.Decimal, UFCWGeneral.ToNullDecimalHandler(smsOk))
            DB.AddInParameter(DBCommandWrapper, "@AUTODIAL_OK_SW", DbType.Decimal, UFCWGeneral.ToNullDecimalHandler(autodialOK))

            DB.AddInParameter(DBCommandWrapper, "@SECURITY_QUESTIONS", DbType.String, UFCWGeneral.ToNullStringHandler(UFCWCryptor.Encrypt256BitString(securityQuestions, "REG_REGISTRATION_INFO")))
            DB.AddInParameter(DBCommandWrapper, "@DEACTIVATED", DbType.Decimal, UFCWGeneral.ToNullDecimalHandler(deactivated))

            DB.AddInParameter(DBCommandWrapper, "@REGISTRATION_COMPLETED_DATE", DbType.DateTime, UFCWGeneral.ToNullDateHandler(registrationCompleteddt))
            DB.AddInParameter(DBCommandWrapper, "@SOURCE", DbType.String, source)
            DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@LASTUPDT", DbType.DateTime, UFCWGeneral.ToNullDateHandler(lastUpdt))

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Function RetrieveWEBRegistrationByFamilyID(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Try

            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommand As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_REG_REGISTRATION_INFO_BY_FAMILY"
            Dim tablenames() As String = {"REG_REGISTRATION_INFO", "REG_MASTER"}

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
                    DB.LoadDataSet(DBCommand, ds, tablenames)
                Else
                    DB.LoadDataSet(DBCommand, ds, tablenames, transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveRegEmailByFamilyID(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Try

            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommand As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_REGEMAIL_BY_FAMILYID"
            Dim tablenames() As String = {"REG_EMAIL", "REG_MASTER"}

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int64, familyID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If

                ds.Tables(0).TableName = "REG_EMAIL"
                ds.Tables(1).TableName = "REG_MASTER"

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
    Public Shared Sub INSERTRegEmail(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date, ByVal thruDate As Date,
                                                ByVal primaryEmail As String, ByVal secondaryEmail As String, ByVal archive As Decimal,
                                                ByVal approvalRequired As Decimal, ByVal massMailOptin As Decimal?, ByVal electronicOptin As Decimal?,
                                                ByVal userRights As String, ByVal source As String, ByVal lastUpdt As Date?,
                                                Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase()

            SQLCall = "FDBEL.INSERT_REG_EMAIL"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)

            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)

            DB.AddInParameter(DBCommandWrapper, "@EMAIL1", DbType.String, primaryEmail.Trim.ToUpper)
            DB.AddInParameter(DBCommandWrapper, "@EMAIL2", DbType.String, secondaryEmail.Trim.ToUpper)

            DB.AddInParameter(DBCommandWrapper, "@ARCHIVE", DbType.Decimal, archive)
            DB.AddInParameter(DBCommandWrapper, "@APPROVAL_REQUIRED_SW", DbType.Decimal, approvalRequired)

            DB.AddInParameter(DBCommandWrapper, "@MASS_MAIL_OPTIN_SW", DbType.Decimal, UFCWGeneral.ToNullDecimalHandler(massMailOptin))
            DB.AddInParameter(DBCommandWrapper, "@ELECTRONIC_OPTIN_SW", DbType.Decimal, UFCWGeneral.ToNullDecimalHandler(electronicOptin))

            DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@SOURCE", DbType.String, source)

            DB.AddInParameter(DBCommandWrapper, "@LASTUPDT", DbType.DateTime, UFCWGeneral.ToNullDateHandler(lastUpdt))

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub UpdateRegeMAIL(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date, ByVal thruDate As Date,
                                                ByVal primaryEmail As String, ByVal secondaryEmail As String, ByVal archive As Decimal?,
                                                ByVal approvalRequired As Decimal?, ByVal massMailOptin As Decimal?, ByVal electronicOptin As Decimal?,
                                                ByVal userRights As String, ByVal onlineDate As Date, ByVal source As String, ByVal status As String,
                                                ByVal lastUpdt As Date?, Optional ByVal transaction As DbTransaction = Nothing)

        If thruDate = CDate("12/31/9998") Then
            thruDate = CDate("12/31/9999")
        End If

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase()

            SQLCall = "FDBEL.UPDATE_REG_EMAIL"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)

            DB.AddInParameter(DBCommandWrapper, "@EMAIL1", DbType.String, primaryEmail)
            DB.AddInParameter(DBCommandWrapper, "@EMAIL2", DbType.String, secondaryEmail)

            DB.AddInParameter(DBCommandWrapper, "@ARCHIVE", DbType.Decimal, UFCWGeneral.ToNullDecimalHandler(archive))
            DB.AddInParameter(DBCommandWrapper, "@APPROVAL_REQUIRED_SW", DbType.Decimal, UFCWGeneral.ToNullDecimalHandler(approvalRequired))

            DB.AddInParameter(DBCommandWrapper, "@MASS_MAIL_OPTIN_SW", DbType.Decimal, UFCWGeneral.ToNullDecimalHandler(massMailOptin))
            DB.AddInParameter(DBCommandWrapper, "@ELECTRONIC_OPTIN_SW", DbType.Decimal, UFCWGeneral.ToNullDecimalHandler(electronicOptin))

            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@ONLINEDATE", DbType.DateTime, onlineDate)
            DB.AddInParameter(DBCommandWrapper, "@SOURCE", DbType.String, source)

            DB.AddInParameter(DBCommandWrapper, "@STATUS", DbType.String, status)

            DB.AddInParameter(DBCommandWrapper, "@LASTUPDT", DbType.DateTime, UFCWGeneral.ToNullDateHandler(lastUpdt))

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub UpdateRegEMAILByStatus(ByVal familyID As Integer, ByVal relationID As Integer,
                                                ByVal origFromDate As Date, ByVal origThruDate As Date, ByVal fromDate As Date, ByVal thruDate As Date,
                                                ByVal primaryEmail As String, ByVal secondaryEmail As String, ByVal archive As Decimal?,
                                                ByVal approvalRequired As Decimal?, ByVal massMailOptin As Decimal?, ByVal electronicOptin As Decimal?,
                                                ByVal userRights As String, ByVal origOnlineDate As Date, ByVal source As String, ByVal status As String,
                                                ByVal lastUpdt As Date?, Optional ByVal transaction As DbTransaction = Nothing)

        If thruDate = CDate("12/31/9998") Then
            thruDate = CDate("12/31/9999")
        End If

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase()

            SQLCall = "FDBEL.UPDATE_REG_EMAIL_BY_STATUS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@ORIG_FROM_DATE", DbType.Date, origFromDate)
            DB.AddInParameter(DBCommandWrapper, "@ORIG_THRU_DATE", DbType.Date, origThruDate)

            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)

            DB.AddInParameter(DBCommandWrapper, "@EMAIL1", DbType.String, primaryEmail)
            DB.AddInParameter(DBCommandWrapper, "@EMAIL2", DbType.String, secondaryEmail)

            DB.AddInParameter(DBCommandWrapper, "@ARCHIVE", DbType.Decimal, UFCWGeneral.ToNullDecimalHandler(archive))
            DB.AddInParameter(DBCommandWrapper, "@APPROVAL_REQUIRED_SW", DbType.Decimal, UFCWGeneral.ToNullDecimalHandler(approvalRequired))

            DB.AddInParameter(DBCommandWrapper, "@MASS_MAIL_OPTIN_SW", DbType.Decimal, UFCWGeneral.ToNullDecimalHandler(massMailOptin))
            DB.AddInParameter(DBCommandWrapper, "@ELECTRONIC_OPTIN_SW", DbType.Decimal, UFCWGeneral.ToNullDecimalHandler(electronicOptin))

            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@ORIGINAL_ONLINE_DATE", DbType.DateTime, origOnlineDate)
            DB.AddInParameter(DBCommandWrapper, "@SOURCE", DbType.String, source)

            DB.AddInParameter(DBCommandWrapper, "@STATUS", DbType.String, status)

            DB.AddInParameter(DBCommandWrapper, "@LASTUPDT", DbType.DateTime, UFCWGeneral.ToNullDateHandler(lastUpdt))

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub UpdateRegeMAILForMaintenance(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date, ByVal thruDate As Date,
                                               ByVal email1 As String, ByVal email2 As String, ByVal massMailOptin As Decimal?, ByVal electronicOptin As Decimal?,
                                               ByVal userRights As String, ByVal originalFMDate As Date, Optional ByVal transaction As DbTransaction = Nothing)

        If thruDate = CDate("12/31/9998") Then
            thruDate = CDate("12/31/9999")
        End If

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_REG_EMAIL_MAINTENANCE"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)

            DB.AddInParameter(DBCommandWrapper, "@EMAIL1", DbType.String, email1)
            DB.AddInParameter(DBCommandWrapper, "@EMAIL2", DbType.String, email2)

            DB.AddInParameter(DBCommandWrapper, "@MASS_MAIL_OPTIN_SW", DbType.Decimal, UFCWGeneral.ToNullDecimalHandler(massMailOptin))
            DB.AddInParameter(DBCommandWrapper, "@ELECTRONIC_OPTIN_SW", DbType.Decimal, UFCWGeneral.ToNullDecimalHandler(electronicOptin))

            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@ORIGFROM_DATE", DbType.Date, originalFMDate)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub DeleteRegEmail(ByVal familyID As Integer, ByVal relationID As Integer, ByVal from_Date As Date, ByVal originalOnlineDate As Date, ByVal transaction As DbTransaction)
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.DELETE_REGEMAIL_BY_FAMILYID_RELATIONID_LASTUPDT"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, from_Date)
            DB.AddInParameter(DBCommandWrapper, "@ORIGINAL_ONLINE_DATE", DbType.DateTime, originalOnlineDate)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub DeleteRegEmail(ByVal familyID As Integer, ByVal relationID As Integer, ByVal transaction As DbTransaction)
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.DELETE_REGEMAIL_BY_FAMILYID_RELATIONID"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    'Public Shared Function RetrieveAnnotationsByFamilyID(ByVal familyId As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
    '    Try

    '        Dim DB  As Database = CMSDALCommon.CreateDatabase()
    '        Dim DBCommand As DbCommand
    '        Dim SQLCall As String = GetDatabaseName(Nothing) & "." & "RETRIEVE_ANNOTATIONS_BY_FAMILYID"

    '        dbCommand = DB.GetStoredProcCommand(SQLCall)
    '        DB.AddInParameter(dbCommand, "@FAMILY_ID", DbType.Int64, familyId)

    '        If ds Is Nothing Then
    '            If transaction Is Nothing Then
    '                ds = DB.ExecuteDataSet(dbCommand)
    '            Else
    '                ds = DB.ExecuteDataSet(dbCommand, transaction)
    '            End If
    '        Else
    '            If transaction Is Nothing Then
    '                DB.LoadDataSet(dbCommand, ds, "ANNOTATIONS")
    '            Else
    '                DB.LoadDataSet(dbCommand, ds, "ANNOTATIONS", transaction)
    '            End If
    '        End If
    '        Return ds
    '    Catch ex As Exception
    '        Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '        If (Rethrow) Then
    '            Throw
    '        End If
    '        Return Nothing
    '    End Try
    'End Function
    'Public Shared Sub CreateAnnotation(ByVal familyID As Integer, ByVal relationID As Integer, ByVal partSSN As Integer, ByVal patSSN As Integer, ByVal partFName As String, ByVal partLName As String, ByVal patFName As String, ByVal patLName As String, ByVal annotation As String, ByVal Icon As Integer, ByVal userRights As String, ByVal transaction As DbTransaction)
    '    Dim DB  As Database = CMSDALCommon.CreateDatabase()
    '    Dim DBCommandWrapper As DbCommand
    '    Dim SQLCall As String

    '    Try
    '        SQLCall = GetDatabaseName(Nothing) & "." & "CREATE_ANNOTATION"

    '        DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

    '        DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
    '        DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)
    '        DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, partSSN)
    '        DB.AddInParameter(DBCommandWrapper, "@PAT_SSN", DbType.Int32, patSSN)
    '        DB.AddInParameter(DBCommandWrapper, "@PART_FNAME", DbType.String, partFName)
    '        DB.AddInParameter(DBCommandWrapper, "@PART_LNAME", DbType.String, partLName)
    '        DB.AddInParameter(DBCommandWrapper, "@PAT_FNAME", DbType.String, patFName)
    '        DB.AddInParameter(DBCommandWrapper, "@PAT_LNAME", DbType.String, patLName)
    '        DB.AddInParameter(DBCommandWrapper, "@ANNOTATION", DbType.String, Replace(CStr(annotation), vbCrLf, "\N"))
    '        DB.AddInParameter(DBCommandWrapper, "@ICON", DbType.Int32, Icon)
    '        DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)


    '        If transaction Is Nothing Then
    '            DB.ExecuteNonQuery(DBCommandWrapper)
    '        Else
    '            DB.ExecuteNonQuery(DBCommandWrapper, transaction)
    '        End If

    '    Catch ex As DB2Exception
    '        Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '        If (Rethrow) Then
    '            Throw
    '        End If
    '    Catch ex As SqlClient.SqlException
    '        Stop
    '    Catch ex As Exception
    '        Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '        If (Rethrow) Then
    '            Throw
    '        End If
    '    End Try
    'End Sub
    'Public Shared Sub UpdateAnnotationsByFamilyIDRelationID(ByVal annotationID As Integer, ByVal familyID As Integer, ByVal relationID As Integer,
    '                                            ByVal annotation As String, ByVal Icon As Integer,
    '                                            ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)

    '    Dim DB  As Database = CMSDALCommon.CreateDatabase()
    '    Dim DBCommandWrapper As DbCommand
    '    Dim SQLCall As String

    '    Try
    '        SQLCall = GetDatabaseName(Nothing) & "." & "UPDATE_ANNOTATIONS_BY_FAMILYID_RELATIONID"

    '        DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
    '        DB.AddInParameter(DBCommandWrapper, "@ANNOTATION_ID", DbType.Int64, annotationID)
    '        DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
    '        DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)

    '        DB.AddInParameter(DBCommandWrapper, "@ANNOTATION", DbType.String, annotation)
    '        DB.AddInParameter(DBCommandWrapper, "@ICON", DbType.Int64, Icon)

    '        DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String, userRights)


    '        If transaction Is Nothing Then
    '            DB.ExecuteNonQuery(DBCommandWrapper)
    '        Else
    '            DB.ExecuteNonQuery(DBCommandWrapper, transaction)
    '        End If

    '    Catch ex As DB2Exception
    '        Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '        If (Rethrow) Then
    '            Throw
    '        End If
    '    Catch ex As SqlClient.SqlException
    '        Stop
    '    Catch ex As Exception
    '        Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '        If (Rethrow) Then
    '            Throw
    '        End If
    '    End Try
    'End Sub
    Public Shared Function RetrieveRegRemarksByFamilyID(ByVal familyId As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Try

            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommand As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_REG_REMARKS_BY_FAMILYID"

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int64, familyId)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, "REG_REMARKS")
                Else
                    DB.LoadDataSet(DBCommand, ds, "REG_REMARKS", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Sub CreateRegRemarks(ByVal familyID As Integer, ByVal relationID As Integer, ByVal postingDate As Date?, ByVal remarks As String, ByVal userRights As String, ByVal deleteFlag As Integer, ByVal transaction As DbTransaction)
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.CREATE_REG_REMARKS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)
            DB.AddInParameter(DBCommandWrapper, "@POSTING_DATE", DbType.Date, postingDate)
            DB.AddInParameter(DBCommandWrapper, "@REMARKS", DbType.String, Replace(CStr(remarks), vbCrLf, "\N"))
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@DELETE_FLAG", DbType.Decimal, deleteFlag)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As SqlClient.SqlException
            Throw

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub UpdateRegRemarksByFamilyIDRelationID(ByVal remarkID As Integer, ByVal familyID As Integer, ByVal relationID As Integer,
                                                ByVal remarks As String,
                                                ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_REG_REMARKS_BY_FAMILYID_RELATIONID"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@REMARK_ID", DbType.Int64, remarkID)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@REMARKS", DbType.String, remarks)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As SqlClient.SqlException
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub CreateRegHistory(ByVal familyID As Integer, ByVal relationID As Integer, ByVal partSSN As Integer?, ByVal patSSN As Integer?, ByVal transactionType As String,
                                       ByVal docClass As String, ByVal docType As String, ByVal docID As Integer, ByVal summary As String,
                                       ByVal detail As String, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.CREATE_REG_HISTORY"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, partSSN)
            DB.AddInParameter(DBCommandWrapper, "@PAT_SSN", DbType.Int32, patSSN)
            DB.AddInParameter(DBCommandWrapper, "@TRANSACTION_TYPE", DbType.String, transactionType)
            DB.AddInParameter(DBCommandWrapper, "@DOC_CLASS", DbType.String, docClass)
            DB.AddInParameter(DBCommandWrapper, "@DOC_TYPE", DbType.String, docType)
            DB.AddInParameter(DBCommandWrapper, "@DOCID", DbType.Int32, docID)
            DB.AddInParameter(DBCommandWrapper, "@SUMMARY", DbType.String, Replace(CStr(summary), vbCrLf, "\N"))
            DB.AddInParameter(DBCommandWrapper, "@DETAIL", DbType.String, Replace(CStr(detail), vbCrLf, "\N"))
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
    Public Shared Function RetrieveRegHistoryByFamily(ByVal familyID As Integer) As DataTable
        Try
            Dim DS As DataSet
            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_REG_HISTORY_BY_FAMILYID"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)

            DBCommandWrapper.CommandTimeout = 180

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            Dim _dataTable As New DataTable("HISTORY")
            If DS.Tables.Count > 0 Then
                _dataTable = DS.Tables(0)
            End If

            Return _dataTable
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveCoverageHistory(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommand As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.RETRIEVE_COVERAGE_HISTORY_BY_FAMILYID_RELATIONID"

            DBCommand = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int32, familyID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, "COVERAGE_HISTORY")
                Else
                    DB.LoadDataSet(DBCommand, ds, "COVERAGE_HISTORY", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception

            Throw

            Return Nothing
        End Try
    End Function
    Public Shared Function RetrieveCoverageHistoryAndNetworks(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommand As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase()

            SQLCall = "FDBEL.RETRIEVE_COVERAGE_HISTORY_HMO_NETWORKS_BY_FAMILYID_RELATIONID"
            Dim tablenames() As String = {"COVERAGE_HISTORY", "HMO_NETWORK"}
            DBCommand = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int32, familyID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If

                ds.Tables(0).TableName = "COVERAGE_HISTORY"
                ds.Tables(1).TableName = "HMO_NETWORK"

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

    Public Shared Function RetrieveCoverageHistoryAndNetworksNoOverlap(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommand As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase()

            SQLCall = "FDBEL.RETRIEVE_COVERAGE_HISTORY_HMO_NETWORKS_BY_FID_RID_NOOVERLAP"
            Dim tablenames() As String = {"COVERAGE_HISTORY", "HMO_NETWORK"}
            DBCommand = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int32, familyID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If

                ds.Tables(0).TableName = "COVERAGE_HISTORY"
                ds.Tables(1).TableName = "HMO_NETWORK"

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

    Public Shared Function RetrieveCoverageHistoryAndNetwork(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommand As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase()

            SQLCall = "FDBEL.RETRIEVE_COVERAGE_HISTORY_HMO_NETWORK_BY_FAMILYID_RELATIONID"
            Dim tablenames() As String = {"COVERAGE_HISTORY", "HMO_NETWORK"}
            DBCommand = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int32, familyID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If

                ds.Tables(0).TableName = "COVERAGE_HISTORY"
                ds.Tables(1).TableName = "HMO_NETWORK"

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
    Public Shared Sub CreatePHIAlert(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date, ByVal alertReason As Integer, ByVal passPhrase As String, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.CREATE_PHI_ALERT"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, CDate("12/31/9999"))
            DB.AddInParameter(DBCommandWrapper, "@ALERT_REASON", DbType.Int32, alertReason)
            DB.AddInParameter(DBCommandWrapper, "@PASSPHRASE", DbType.String, passPhrase)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ALERT", DbType.Decimal, 1)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub DeletePHIAlert(ByVal familyID As Integer, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.DELETE_PHI_ALERT"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub


    Public Shared Function RetrieveRegLifeEventsByFamilyID(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommand As DbCommand
        Dim SQLCall As String = "FDBEL.RETRIEVE_ALL_REG_LIFE_EVENTS_BY_FAMILYID"

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
                    DB.LoadDataSet(DBCommand, ds, "REG_LIFE_EVENTS")
                Else
                    DB.LoadDataSet(DBCommand, ds, "REG_LIFE_EVENTS", transaction)
                End If
            End If

            If ds.Tables.Count > 0 Then
                ds.Tables(0).TableName = "REG_LIFE_EVENTS"
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Sub AddLifeEvent(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date, ByVal thruDate As Date,
                                    ByVal eventCode As Integer, ByVal eventDate As Date,
                                    ByVal termCode As Integer, ByVal termDate As Date?,
                                    ByVal userRights As String, ByVal DeleteFlag As Decimal, ByVal csoIndicator As String, ByVal parentRelationID As Integer,
                                    ByVal createDate As Date, Optional ByVal transaction As DbTransaction = Nothing)

        If thruDate = CDate("12/31/9998") Then
            thruDate = CDate("12/31/9999")
        End If

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.ADD_LIFE_EVENT_TS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)

            DB.AddInParameter(DBCommandWrapper, "@EVENT_CODE", DbType.Int16, eventCode)
            DB.AddInParameter(DBCommandWrapper, "@EVENT_DATE", DbType.Date, eventDate)
            DB.AddInParameter(DBCommandWrapper, "@TERM_CODE", DbType.Int16, termCode)
            DB.AddInParameter(DBCommandWrapper, "@TERM_DATE", DbType.Date, termDate)


            DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@DELETE_FLAG", DbType.Decimal, DeleteFlag)
            DB.AddInParameter(DBCommandWrapper, "@SPCL_IND", DbType.String, csoIndicator)
            DB.AddInParameter(DBCommandWrapper, "@SPARENT_RELID", DbType.Int16, parentRelationID)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_DATE", DbType.DateTime, createDate)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub DeleteLifeEvent(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date,
                                        ByVal eventCode As Integer, ByVal userRights As String,
                                        ByVal onlineDate As Date, ByVal originalOnlineDate As Date, Optional ByVal transaction As DbTransaction = Nothing)


        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.DELETE_LIFE_EVENT_TS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)

            DB.AddInParameter(DBCommandWrapper, "@EVENT_CODE", DbType.Int16, eventCode)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_DATE", DbType.DateTime, onlineDate)
            DB.AddInParameter(DBCommandWrapper, "@ORIG_ONLINE_DATE", DbType.DateTime, originalOnlineDate)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As SqlClient.SqlException
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Function GetEligibility(ByVal familyId As Integer, ByVal relationId As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand = DB.GetStoredProcCommand("FDBEL.RETRIEVE_ELIGIBILITY_BY_FAMILYID_RELATIONID")
        Try
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyId)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationId)
            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "ELIGIBILITY")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "ELIGIBILITY", transaction)
                End If
            End If
            '' Return DS
        Catch ex As Exception
            Throw
        End Try
        Return ds
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
            DT = Nothing
            If DS IsNot Nothing Then DS.Dispose()
            DS = Nothing

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

    Public Shared Function RetrievePatientsCoverableGapInfo(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date, ByVal thruDate As Date, Optional ByVal transaction As DbTransaction = Nothing) As DataTable
        Try
            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommandWrapper As DbCommand
            Dim DS As DataSet = Nothing

            Dim SQLCall As String = "FDBEL.RETRIEVE_MONTH_YEAR_NOT_COVERABLE_BY_FAMILYID_RELATIONID"
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
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Sub UpdateRegLifeEventTS(ByVal familyID As Integer, ByVal relationID As Integer,
                                            ByVal thruDate As Date, ByVal userRights As String,
                                            ByVal eventCode As Integer?, ByVal eventDate As Date?,
                                            ByVal termCode As Integer?, ByVal termDate As Date?,
                                            ByVal csoIndicator As String, ByVal parentRelationID As Integer?,
                                            ByVal onlineDate As Date,
                                            ByVal originalFromDate As Date, ByVal originalEventCode As Integer, ByVal originalOnlineDate As Date,
                                            ByVal transaction As DbTransaction)

        If thruDate = CDate("12/31/9998") Then
            thruDate = CDate("12/31/9999")
        End If

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String
        Dim UpdateCount As Integer
        Try
            SQLCall = "FDBEL.UPDATE_REG_LIFE_EVENT_TS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)

            DB.AddInParameter(DBCommandWrapper, "@EVENT_CODE", DbType.Int16, eventCode)
            DB.AddInParameter(DBCommandWrapper, "@EVENT_DATE", DbType.Date, eventDate)
            DB.AddInParameter(DBCommandWrapper, "@TERM_CODE", DbType.Int16, termCode)
            DB.AddInParameter(DBCommandWrapper, "@TERM_DATE", DbType.Date, termDate)

            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@SPCL_IND", DbType.String, csoIndicator)
            DB.AddInParameter(DBCommandWrapper, "@SPARENT_RELID", DbType.Int16, parentRelationID)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_DATE", DbType.DateTime, onlineDate)
            DB.AddInParameter(DBCommandWrapper, "@ORIGINAL_FROM_DATE", DbType.Date, originalFromDate)
            DB.AddInParameter(DBCommandWrapper, "@ORIGINAL_EVENT_CODE", DbType.Int16, originalEventCode)
            DB.AddInParameter(DBCommandWrapper, "@ORIGINAL_ONLINE_DATE", DbType.DateTime, originalOnlineDate)

            UpdateCount = DB.ExecuteNonQuery(DBCommandWrapper, transaction)

        Catch ex As Exception

            Throw

        End Try
    End Sub

    Public Shared Sub UpdateRegLifeEvent(ByVal familyID As Integer, ByVal relationID As Integer,
                                     ByVal thruDate As Date, ByVal userRights As String,
                                     ByVal transaction As DbTransaction, Optional ByVal EventCode As Integer? = Nothing, Optional ByVal EventDate As Date? = Nothing,
                                     Optional ByVal termCode As Integer? = Nothing, Optional ByVal termDate As Date? = Nothing,
                                     Optional ByVal csoIndicator As String = Nothing, Optional ByVal parentRelationID As Integer? = Nothing)

        If thruDate = CDate("12/31/9998") Then
            thruDate = CDate("12/31/9999")
        End If

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase()

            SQLCall = "FDBEL.UPDATE_REG_LIFE_EVENT"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)

            DB.AddInParameter(DBCommandWrapper, "@EVENT_CODE", DbType.Int16, EventCode)
            DB.AddInParameter(DBCommandWrapper, "@EVENT_DATE", DbType.Date, EventDate)
            DB.AddInParameter(DBCommandWrapper, "@TERM_CODE", DbType.Int16, termCode)
            DB.AddInParameter(DBCommandWrapper, "@TERM_DATE", DbType.Date, termDate)

            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@SPCL_IND", DbType.String, csoIndicator)
            DB.AddInParameter(DBCommandWrapper, "@SPARENT_RELID", DbType.Int16, parentRelationID)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)
        Catch ex As Exception

            Throw

        End Try
    End Sub

    Public Shared Sub UpdateRegLifeEventforMaintenance(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date,
                                                        ByVal thruDate As Date, ByVal userRights As String, ByVal originalFromDate As Date, ByVal originalThruDate As Date,
                                                        ByVal eventCode As Integer?, ByVal eventDate As Date?,
                                                        ByVal termCode As Integer?, ByVal termDate As Date?,
                                                        ByVal cSOindicator As String, ByVal parentRelationID As Integer?,
                                                        ByVal onlineDate As Date, ByVal originalOnlineDate As Date, ByVal transaction As DbTransaction)

        If thruDate = CDate("12/31/9998") Then
            thruDate = CDate("12/31/9999")
        End If

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_LIFE_EVENT_FOR_MAINTENANCE_TS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)

            DB.AddInParameter(DBCommandWrapper, "@ORGINAL_FROM_DATE", DbType.Date, originalFromDate)
            DB.AddInParameter(DBCommandWrapper, "@ORGINAL_THRU_DATE", DbType.Date, originalThruDate)

            DB.AddInParameter(DBCommandWrapper, "@EVENT_CODE", DbType.Int16, eventCode)
            DB.AddInParameter(DBCommandWrapper, "@EVENT_DATE", DbType.Date, eventDate)
            DB.AddInParameter(DBCommandWrapper, "@TERM_CODE", DbType.Int16, termCode)
            DB.AddInParameter(DBCommandWrapper, "@TERM_DATE", DbType.Date, termDate)

            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@SPCL_IND", DbType.String, cSOindicator)
            DB.AddInParameter(DBCommandWrapper, "@SPARENT_RELID", DbType.Int16, parentRelationID)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_DATE", DbType.DateTime, onlineDate)
            DB.AddInParameter(DBCommandWrapper, "@ORIGINAL_ONLINE_DATE", DbType.DateTime, originalOnlineDate)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

        Catch ex As Exception

            Throw

        End Try
    End Sub
    Public Shared Sub UpdateRegMasterFromLifeEvents(ByVal familyID As Integer, ByVal relationID As Integer,
                                                    ByVal thruDate As Date, ByVal userRights As String,
                                                    ByVal onlineDate As Date, ByVal originalOnlineDate As Date, ByVal transaction As DbTransaction)

        If thruDate = CDate("12/31/9998") Then
            thruDate = CDate("12/31/9999")
        End If

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_REG_MASTER_FROM_LIFE_EVENTS_TS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_DATE", DbType.DateTime, onlineDate)
            DB.AddInParameter(DBCommandWrapper, "@ORIGINAL_ONLINE_DATE", DbType.DateTime, originalOnlineDate)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)
        Catch ex As Exception

            Throw

        End Try
    End Sub
    Public Shared Sub UpdateAllRegMasterAndLifeEventsDependentsByFamilyID(ByVal familyID As Integer,
                                                                            ByVal thruDate As Date, ByVal userRights As String, ByVal termDate As Date,
                                                                            ByVal onlineDate As Date, ByVal transaction As DbTransaction)


        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_RECORDS_AFTER_MEMBER_DEATH_TS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@DEATH_DATE", DbType.Date, termDate)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_DATE", DbType.DateTime, onlineDate)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

        Catch ex As Exception

            Throw

        End Try
    End Sub
    Public Shared Sub UpdateAllRegMasterandAllLifeEventsByFamilyID(ByVal familyID As Integer,
                                                                    ByVal thruDate As Date, ByVal userRights As String, ByVal termDate As Date, ByVal termCode As Integer,
                                                                    ByVal onlineDate As Date, ByVal originalOnlineDate As Date, ByVal transaction As DbTransaction)


        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_RECORDS_AFTER_MEM_SSN_CHANGE"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@TERM_DATE", DbType.Date, termDate)
            DB.AddInParameter(DBCommandWrapper, "@TERM_DATE", DbType.Int64, termCode)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_DATE", DbType.DateTime, onlineDate)
            DB.AddInParameter(DBCommandWrapper, "@ORIGINAL_ONLINE_DATE", DbType.DateTime, originalOnlineDate)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

        Catch ex As Exception

            Throw

        End Try
    End Sub
    Public Shared Sub DeleteLifeEventbyFamilyIDRelationID(ByVal familyID As Integer, ByVal relationID As Integer,
                                                            ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)

        ''  This is useful when we r deleting the dependent 
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.DELETE_LIFE_EVENTS_BY_FAMILYID_RELATIONID"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)

            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub UpdateMonthlyDetail(ByVal familyID As Integer, ByVal eligPeriod As Date,
                                          ByVal userRights As String,
                                          ByVal onlineDate As Date, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String
        Dim UpdateCount As Integer

        Try
            SQLCall = "FDBEL.UPDATE_MONTHLY_DETAIL_BY_FAMILYID_TS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@ELIG_PERIOD", DbType.Date, eligPeriod)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_DATE", DbType.DateTime, onlineDate)

            If transaction Is Nothing Then
                UpdateCount = DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                UpdateCount = DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Function RetrieveRegLifeEventsForAuditByFamilyIDRelationID(ByVal familyID As Integer, ByVal relationID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataTable
        Try

            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommand As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_AUDIT_LIFEEVENTS_BY_FAMILYID_RELATIONID"

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommand, "@RELATION_ID", DbType.Int32, relationID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, "REG_LIFE_EVENTS")
                Else
                    DB.LoadDataSet(DBCommand, ds, "REG_LIFE_EVENTS", transaction)
                End If
            End If
            If ds.Tables.Count > 0 Then
                ds.Tables(0).TableName = "REG_LIFE_EVENTS"
            End If
            If ds.Tables.Count > 0 Then
                Return ds.Tables(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Sub UpdateDependentLifeEvents(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date,
                                    ByVal thruDate As Date, ByVal eventCode As Integer, ByVal termCode As Integer, ByVal termDate As Date, ByVal userRights As String,
                                    ByVal onlineDate As Date, ByVal originalOnlineDate As Date, ByVal transaction As DbTransaction)

        If thruDate = CDate("12/31/9998") Then
            thruDate = CDate("12/31/9999")
        End If

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_DEPENDENT_LIFE_EVENTS_TS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@EVENT_CODE", DbType.Int16, eventCode)
            DB.AddInParameter(DBCommandWrapper, "@TERM_CODE", DbType.Int16, termCode)
            DB.AddInParameter(DBCommandWrapper, "@TERM_DATE", DbType.Date, termDate)

            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_DATE", DbType.DateTime, onlineDate)
            DB.AddInParameter(DBCommandWrapper, "@ORIGINAL_ONLINE_DATE", DbType.DateTime, originalOnlineDate)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub UpdateRegMasterMarriageSW(ByVal familyID As Integer, ByVal relationID As Integer, ByVal thruDate As Date,
                                                ByVal userRights As String, ByVal transaction As DbTransaction)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_REGMASTER_MARRIAGE_OVER_SW"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@MARRIAGE_OVERRIDE_SW", DbType.Decimal, 0)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Function RetrieveRegPhoneByFamilyID(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Try

            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommand As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_PHONE_NUMBERS_BY_FAMILYID"
            Dim tablenames() As String = {"REG_PHONE", "REG_MASTER"}

            DBCommand = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int32, familyID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If

                ds.Tables(0).TableName = "REG_PHONE"
                ds.Tables(1).TableName = "REG_MASTER"

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
        End Try
    End Function

    Public Shared Function RetrieveStates() As DataTable
        Try

            If _StatesDS Is Nothing Then
                _StatesDS = LoadStates()
            End If

            Dim DT As New DataTable("LOOKUP_STATES")
            If _StatesDS.Tables.Count > 0 Then
                DT = _StatesDS.Tables(0)
            End If

            Return DT

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Shared Function LoadStates(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        SyncLock (_StateslockObj)

            Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBEL.RETRIEVE_STATES" & ".xml"
            Dim UniqueThreadIdentifier As String = GetUniqueKey()
            Dim XMLDS As DataSet
            Dim FStream As FileStream
            Dim XMLSerial As XmlSerializer

            Try

                XMLDS = XMLHandler.ToandFromDataset(UniqueThreadIdentifier, "HOURS.LOOKUP_STATES", "LASTUPDT", "FDBEL.RETRIEVE_STATES")
                If XMLDS.Tables.Count = 0 Then
                    Dim DB As Database = CMSDALCommon.CreateDatabase()
                    Dim DBCommandWrapper As DbCommand
                    Dim SQLCall As String = "FDBEL.RETRIEVE_STATES"
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
                    If XMLDS Is Nothing Then
                        If transaction Is Nothing Then
                            XMLDS = DB.ExecuteDataSet(DBCommandWrapper)
                        Else
                            XMLDS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                        End If
                    Else
                        If transaction Is Nothing Then
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "LOOKUP_STATES")
                        Else
                            DB.LoadDataSet(DBCommandWrapper, XMLDS, "LOOKUP_STATES", transaction)
                        End If
                    End If

                    _StatesDS = XMLDS

                    FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)
                    XMLSerial = New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(FStream, XMLDS)
                    FStream.Close()
                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _StatesDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_StatesDS)
                    Return ds
                Else
                    Return _StatesDS
                End If

            Catch ex As Exception
                Throw
            Finally

                If FStream IsNot Nothing Then
                    FStream.Close()
                    FStream.Dispose()
                End If

                FStream = Nothing

            End Try
        End SyncLock

    End Function

    Public Shared Function RetrievePhoneTypes() As DataTable

        SyncLock (_PhoneTypeslockObj)
            Try

                If _PhoneTypesDS Is Nothing Then
                    _PhoneTypesDS = LoadPhoneTypes()
                End If

                Dim DT As New DataTable("REG_PHONE_TYPE")
                If _PhoneTypesDS.Tables.Count > 0 Then
                    DT = _PhoneTypesDS.Tables(0)
                End If

                Return DT

            Catch ex As Exception
                Throw
            End Try
        End SyncLock

    End Function

    Private Shared Function LoadPhoneTypes(Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBEL.RETRIEVE_REG_PHONE_TYPES" & ".xml"
        Dim UniqueThreadIdentifier As String = GetUniqueKey()
        Dim XMLDS As DataSet
        Dim FStream As FileStream
        Dim XMLSerial As XmlSerializer

        Try

            XMLDS = XMLHandler.ToandFromDataset(UniqueThreadIdentifier, "FDBEL.REG_PHONE_TYPE", "ONLINE_DATE", "FDBEL.RETRIEVE_REG_PHONE_TYPES")
            If XMLDS.Tables.Count = 0 Then
                Dim DB As Database = CMSDALCommon.CreateDatabase()
                Dim DBCommandWrapper As DbCommand
                Dim SQLCall As String = "FDBEL.RETRIEVE_REG_PHONE_TYPES"
                DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
                If XMLDS Is Nothing Then
                    If transaction Is Nothing Then
                        XMLDS = DB.ExecuteDataSet(DBCommandWrapper)
                    Else
                        XMLDS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                    End If
                Else
                    If transaction Is Nothing Then
                        DB.LoadDataSet(DBCommandWrapper, XMLDS, "REG_PHONE_TYPE")
                    Else
                        DB.LoadDataSet(DBCommandWrapper, XMLDS, "REG_PHONE_TYPE", transaction)
                    End If
                End If

                _PhoneTypesDS = XMLDS

                FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)
                XMLSerial = New XmlSerializer(XMLDS.GetType())
                XMLSerial.Serialize(FStream, XMLDS)
                FStream.Close()
                File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
            Else
                _PhoneTypesDS = XMLDS
            End If

            If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                ds.Merge(_PhoneTypesDS)
                Return ds
            Else
                Return _PhoneTypesDS
            End If

        Catch ex As Exception
            Throw
        Finally

            If FStream IsNot Nothing Then
                FStream.Close()
                FStream.Dispose()
            End If

            FStream = Nothing

        End Try

    End Function

    Public Shared Sub INSERTRegPhone(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date, ByVal thruDate As Date,
                                                ByVal phone As String, ByVal source As String, ByVal preferred As Integer,
                                                ByVal type As String, ByVal archive As Decimal, ByVal approvalRequired As Decimal,
                                                ByVal smsOK As Integer, ByVal autoDialOK As Integer, ByVal userRights As String, ByVal createDate As Date?, ByVal phoneType As Integer, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.INSERT_REG_PHONE_ENHANCED"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)

            DB.AddInParameter(DBCommandWrapper, "@PHONE", DbType.String, phone)
            DB.AddInParameter(DBCommandWrapper, "@SOURCE", DbType.String, source)
            DB.AddInParameter(DBCommandWrapper, "@PREFFERED", DbType.Int16, preferred)
            DB.AddInParameter(DBCommandWrapper, "@TYPE", DbType.String, type)
            DB.AddInParameter(DBCommandWrapper, "@ARCHIVE", DbType.Decimal, archive)
            DB.AddInParameter(DBCommandWrapper, "@APPROVAL_REQUIRED_SW", DbType.Decimal, approvalRequired)
            DB.AddInParameter(DBCommandWrapper, "@SMSOK", DbType.Int16, smsOK)
            DB.AddInParameter(DBCommandWrapper, "@AUTODIAL_OK_SW", DbType.Int16, autoDialOK)
            DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@CREATE_DATE", DbType.DateTime, createDate)
            DB.AddInParameter(DBCommandWrapper, "@PHONE_TYPE", DbType.Int16, phoneType)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub DeleteRegPhone(ByVal familyID As Integer, ByVal relationID As Integer, ByVal from_Date As Date, ByVal originalOnlineDate As Date, ByVal transaction As DbTransaction)
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.DELETE_REGPHONE_BY_FAMILYID_RELATIONID_LASTUPDT"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, from_Date)
            DB.AddInParameter(DBCommandWrapper, "@ORIGINAL_ONLINE_DATE", DbType.DateTime, originalOnlineDate)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub UpdateRegPhone(ByVal familyID As Integer, ByVal relationID As Integer,
                                        ByVal origFromDate As Date, ByVal origThruDate As Date, ByVal fromDate As Date, ByVal thruDate As Date,
                                        ByVal phone As String, ByVal preferred As Integer?,
                                        ByVal type As String, ByVal archive As Decimal?, ByVal approvalRequired As Decimal?,
                                        ByVal smsok As Integer?, ByVal autoDialOK As Integer?, ByVal userRights As String, ByVal phoneType As Integer?,
                                        ByVal origPhoneType As Integer?, ByVal origOnlineDate As Date,
                                        ByVal source As String, ByVal status As String, ByVal lastUpDt As Date?,
                                        Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_REG_PHONE_BY_STATUS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)

            DB.AddInParameter(DBCommandWrapper, "@ORIG_FROM_DATE", DbType.Date, origFromDate)
            DB.AddInParameter(DBCommandWrapper, "@ORIG_THRU_DATE", DbType.Date, origThruDate)

            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)

            DB.AddInParameter(DBCommandWrapper, "@PHONE", DbType.String, phone)

            DB.AddInParameter(DBCommandWrapper, "@PREFERRED", DbType.Int16, preferred)
            DB.AddInParameter(DBCommandWrapper, "@TYPE", DbType.String, type)
            DB.AddInParameter(DBCommandWrapper, "@PHONE_TYPE", DbType.Int16, phoneType)
            DB.AddInParameter(DBCommandWrapper, "@ORIG_PHONE_TYPE", DbType.Int16, origPhoneType)
            DB.AddInParameter(DBCommandWrapper, "@SMSALLOWED", DbType.Int16, smsok)
            DB.AddInParameter(DBCommandWrapper, "@AUTODIAL_OK_SW", DbType.Int16, autoDialOK)

            DB.AddInParameter(DBCommandWrapper, "@ARCHIVE", DbType.Decimal, archive)
            DB.AddInParameter(DBCommandWrapper, "@APPROVAL_REQUIRED_SW", DbType.Decimal, approvalRequired)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_DATE", DbType.DateTime, origOnlineDate)
            DB.AddInParameter(DBCommandWrapper, "@SOURCE", DbType.String, source)

            DB.AddInParameter(DBCommandWrapper, "@STATUS", DbType.String, status)
            DB.AddInParameter(DBCommandWrapper, "@LASTUPDT", DbType.DateTime, lastUpDt)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub UpdateRegPhoneAutoDialAllowed(ByVal phoneID As Integer, ByVal smsOK As Integer?, ByVal autoDialOK As Integer?, ByVal userRights As String,
                                                Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String
        Dim UpdateCount As Integer

        Try
            SQLCall = "FDBEL.UPDATE_REG_PHONE_AUTODIAL"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@PHONE_ID", DbType.Int32, phoneID)
            DB.AddInParameter(DBCommandWrapper, "@SMS_OK_SW", DbType.Int16, smsOK)
            DB.AddInParameter(DBCommandWrapper, "@AUTODIAL_OK_SW", DbType.Int16, autoDialOK)

            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            If transaction Is Nothing Then
                UpdateCount = DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                UpdateCount = DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub SaveFamilyIDHistoryXML(familyHistoryDT As DataTable)

        Dim XMLFilename As String
        Dim RententionDays As Integer
        Dim SaveDT As DataTable

        If System.Configuration.ConfigurationManager.AppSettings("TransactionHistory") IsNot Nothing Then
            XMLFilename = Path.Combine(Application.StartupPath, System.Configuration.ConfigurationManager.AppSettings("TransactionHistory"))
        Else
            XMLFilename = Path.Combine(Application.StartupPath, "FamilyHistory.xml")
        End If

        If System.Configuration.ConfigurationManager.AppSettings("TransactionHistoryRetentionInDays") IsNot Nothing Then
            RententionDays = CInt(System.Configuration.ConfigurationManager.AppSettings("TransactionHistoryRetentionInDays"))
        Else
            RententionDays = 5
        End If

        Try

            If familyHistoryDT IsNot Nothing Then

                Try
                    File.Delete(XMLFilename)
                Catch ex As Exception
                    'if file doesn't exist ignore error
                End Try

                Using sWriter As System.IO.StreamWriter = New System.IO.StreamWriter(XMLFilename)
                    Dim XMLSerial As New XmlSerializer(familyHistoryDT.GetType())

                    Dim DV As DataView = New DataView(familyHistoryDT, "LAST_ACCESSED_DATE > #" & UFCWGeneral.NowDate.AddDays(CInt(RententionDays.ToString) * -1) & "#", "", DataViewRowState.CurrentRows)
                    SaveDT = DV.ToTable

                    XMLSerial.Serialize(sWriter, SaveDT)
                    sWriter.Close()
                End Using

            End If

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Sub

    Public Shared Function GetFamilyIDHistoryXML() As DataTable

        Dim mySerializer As XmlSerializer
        Dim NewCol As DataColumn
        Dim FamilyHistoryDT As DataTable
        Dim XMLFilename As String

        Try

            If System.Configuration.ConfigurationManager.AppSettings("TransactionHistory") IsNot Nothing Then
                XMLFilename = Path.Combine(Application.StartupPath, System.Configuration.ConfigurationManager.AppSettings("TransactionHistory"))
            Else
                XMLFilename = Path.Combine(Application.StartupPath, "FamilyHistory.xml")
            End If

            Try

                If File.Exists(XMLFilename) Then

                    FamilyHistoryDT = New DataTable

                    mySerializer = New XmlSerializer(FamilyHistoryDT.GetType)

                    Using FStream As FileStream = New FileStream(XMLFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                        FamilyHistoryDT = CType(mySerializer.Deserialize(FStream), DataTable)
                    End Using
                    '' Create the object from the xml file

                End If

            Catch ex As Exception

                If File.Exists(XMLFilename) Then 'assume corrupted 
                    File.Delete(XMLFilename)
                End If
                FamilyHistoryDT = Nothing
            End Try

            If FamilyHistoryDT Is Nothing Then

                FamilyHistoryDT = New DataTable("FamilyHistory")

                ' Create Claims History Table structure

                NewCol = New DataColumn("FAMILY_ID")
                NewCol.DataType = System.Type.GetType("System.String")
                FamilyHistoryDT.Columns.Add(NewCol)

                NewCol = New DataColumn("PART_SSN")
                NewCol.DataType = System.Type.GetType("System.String")
                FamilyHistoryDT.Columns.Add(NewCol)

                NewCol = New DataColumn("NAME")
                NewCol.DataType = System.Type.GetType("System.String")
                FamilyHistoryDT.Columns.Add(NewCol)

                NewCol = New DataColumn("LAST_ACCESSED_DATE")
                NewCol.DataType = System.Type.GetType("System.DateTime")
                FamilyHistoryDT.Columns.Add(NewCol)

                NewCol = New DataColumn("STATUS")
                NewCol.DataType = System.Type.GetType("System.String")
                FamilyHistoryDT.Columns.Add(NewCol)

            End If

            Return FamilyHistoryDT

        Catch ex As InvalidOperationException
        Catch ex As Exception

            Throw

        Finally

            If mySerializer IsNot Nothing Then mySerializer = Nothing

        End Try

    End Function
    Public Shared Sub WriteToFamilyIDHistoryXML(ByVal familyID As String, ByVal SSN As String, ByVal Name As String, ByVal accessed As DateTime, ByVal status As String)

        Dim NewRow As DataRow
        Try

            Using FamilyHistoryDT As DataTable = GetFamilyIDHistoryXML()
                NewRow = FamilyHistoryDT.NewRow()

                NewRow("FAMILY_ID") = familyID
                NewRow("PART_SSN") = SSN
                NewRow("NAME") = Name
                NewRow("LAST_ACCESSED_DATE") = accessed
                NewRow("STATUS") = status

                FamilyHistoryDT.Rows.Add(NewRow)
                FamilyHistoryDT.AcceptChanges()

                RegMasterDAL.SaveFamilyIDHistoryXML(FamilyHistoryDT)

            End Using

        Catch ex As Exception

            Throw

        End Try

    End Sub

    Public Shared Function RetrieveEligCalcElementsByFamilyID(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Try

            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommand As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_ELGCALC_ELEMENTS_BY_FAMILYID"

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int64, familyID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If

                If ds IsNot Nothing Then
                    ds.Tables(0).TableName = "ELGCALC_ELEMENTS"
                End If

            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, "ELGCALC_ELEMENTS")
                Else
                    DB.LoadDataSet(DBCommand, ds, "ELGCALC_ELEMENTS", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrievePatients(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal lifeDS As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim PatientsDS As DataSet
        Dim PatientDR As DataRow

        Try

            For Each DR As DataRow In ds.Tables("REG_MASTER").Rows
                If lifeDS IsNot Nothing Then
                    Dim DRRelation() As DataRow = lifeDS.Tables(0).Select("RELATION_ID= " & DR("RELATION_ID").ToString)
                    If DRRelation IsNot Nothing AndAlso DRRelation.Length = 0 Then
                        DR("ELIGIBILITY_GAP_COUNT") = 0
                        DR("ELIGIBILITY_MAX_GAP_COUNT") = 0
                    Else
                        PatientDR = CMSDALFDBEL.RetrievePatientsDaysNotCoverable(familyID, CInt(DR("RELATION_ID")), CDate(DR("FROM_DATE")), CDate(DR("THRU_DATE")), PatientsDS, transaction)
                        DR("ELIGIBILITY_GAP_COUNT") = PatientDR("ELIGIBILITY_GAP_COUNT")
                        DR("ELIGIBILITY_MAX_GAP_COUNT") = PatientDR("ELIGIBILITY_MAX_GAP_COUNT")
                    End If
                Else
                    Select Case DR("RELATION").ToString
                        Case "S", "D", "B", "G", "H", "W", "P", "M"
                            PatientDR = CMSDALFDBEL.RetrievePatientsDaysNotCoverable(familyID, CInt(DR("RELATION_ID")), CDate(DR("FROM_DATE")), CDate(DR("THRU_DATE")), PatientsDS, transaction)
                            DR("ELIGIBILITY_GAP_COUNT") = PatientDR("ELIGIBILITY_GAP_COUNT")
                            DR("ELIGIBILITY_MAX_GAP_COUNT") = PatientDR("ELIGIBILITY_MAX_GAP_COUNT")
                        Case Else
                            DR("ELIGIBILITY_GAP_COUNT") = 0
                            DR("ELIGIBILITY_MAX_GAP_COUNT") = 0
                    End Select
                End If
            Next

            Return ds

        Catch ex As Exception

            Throw
        End Try

    End Function

    Public Shared Sub RetrievePatients(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date, ByVal thruDate As Date, ByRef eligibilityGapCount As Integer, eligibilityMaxGapCount As Integer)

        Dim DR As DataRow

        Try
            eligibilityGapCount = 0
            eligibilityMaxGapCount = 0

            DR = CMSDALFDBEL.RetrievePatientsDaysNotCoverable(familyID, relationID, fromDate, thruDate)
            If DR IsNot Nothing Then
                eligibilityGapCount = CInt(DR("ELIGIBILITY_GAP_COUNT"))
                eligibilityMaxGapCount = CInt(DR("ELIGIBILITY_MAX_GAP_COUNT"))
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub InsertFamilyLock(ByVal familyID As Integer, ByVal relationID As Integer, ByVal userRights As String, ByVal workStation As String, Optional ByVal transaction As DbTransaction = Nothing)
        Try
            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBEL.INSERT_FAMILY_LOCKS"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)
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
        Try
            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBEL.RELEASE_FAMILY_LOCKS"
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
    Public Shared Function RetrieveFamilyLock(ByVal familyID As Integer, Optional ByVal transaction As DbTransaction = Nothing) As DataTable
        Try
            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_FAMILY_LOCKS_BY_FAMILYID"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            Dim DS As DataSet

            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
            End If
            Dim DT As New DataTable("FAMILY_LOCKS")
            If DS.Tables.Count > 0 Then
                DT = DS.Tables(0)
            End If

            Return DT

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveHMONetworkInfo(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Try
            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommand As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_HMO_NETWORK_FAMILYID"
            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int32, familyID)

            DBCommand.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If

                If ds IsNot Nothing Then
                    ds.Tables(0).TableName = "HMO_NETWORK"
                End If

            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, "HMO_NETWORK")
                Else
                    DB.LoadDataSet(DBCommand, ds, "HMO_NETWORK", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Sub AddHMONetworkInfo(ByVal activityTimestamp As DateTime, ByVal hmoID As Integer?, ByVal fromDate As Date, ByVal thruDate As Date,
                                        ByVal familyID As Integer, ByVal relationID As Integer,
                                        ByVal hmoNetwork As String,
                                        ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try

            SQLCall = "FDBEL.ADD_HMO_NETWORK_ASSIGNMENT"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@HMO_ID", DbType.Int32, UFCWGeneral.ToNullIntegerHandler(hmoID))
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@HMO_NETWORK", DbType.String, hmoNetwork)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@CREATE_DATE", DbType.DateTime, activityTimestamp)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Function RetrieveZipHMONetworkInfo(ByVal hmoNetwork As Integer, ByVal zip As Integer, ByVal fromdate As Date, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataRow
        Try
            Dim DR As DataRow = Nothing
            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommand As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_ZIP_BY_HMO_NETWORK"
            DBCommand = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommand, "@HMO_NETWORK", DbType.Int32, hmoNetwork)
            DB.AddInParameter(DBCommand, "@ZIP", DbType.Int32, zip)
            DB.AddInParameter(DBCommand, "@FROM_DATE", DbType.Date, fromdate)

            DBCommand.CommandTimeout = 180


            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If

                If ds IsNot Nothing Then
                    ds.Tables(0).TableName = "HMO_NETWORK_ZIP"
                End If
            Else

                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, "HMO_NETWORK_ZIP")
                Else
                    DB.LoadDataSet(DBCommand, ds, "HMO_NETWORK_ZIP", transaction)
                End If
            End If

            If ds.Tables(0).Rows.Count > 0 Then
                Return ds.Tables(0).Rows(0)
            Else
                Return Nothing
            End If

            Return DR

        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Shared Sub UpdateHMONetworkInfo(ByVal activityTimestamp As DateTime, ByVal hmoID As Integer?, ByVal fromDate As Date, ByVal thruDate As Date, ByVal familyID As Integer, ByVal relationID As Integer,
                                           ByVal hmoNetwork As String, ByVal originalOnlineDate As DateTime, ByVal userRights As String, ByVal originalFromDate As Date, ByVal originalThruDate As Date,
                                           Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBEL.UPDATE_HMO_NETWORK_ASSIGNMENT"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            'if HMO_ID has a value the destination table is the deprecated PACCARE table
            DB.AddInParameter(DBCommandWrapper, "@HMO_ID", DbType.Int32, UFCWGeneral.ToNullIntegerHandler(hmoID))
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@HMO_NETWORK", DbType.String, hmoNetwork)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@ORIGFROM_DATE", DbType.Date, originalFromDate)
            DB.AddInParameter(DBCommandWrapper, "@ORIGTHRU_DATE", DbType.Date, originalThruDate)
            DB.AddInParameter(DBCommandWrapper, "@ORIGONLINE_DATE", DbType.DateTime, originalOnlineDate)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_DATE", DbType.DateTime, activityTimestamp)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub DeleteHMONetworkInfo(ByVal hmoID As Integer?, ByVal familyID As Integer, ByVal relationID As Integer,
                                           ByVal originalOnlineDate As Date, ByVal originalFromDate As Date, ByVal originalThruDate As Date,
                                           Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBEL.DELETE_HMO_NETWORK_ASSIGNMENT"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            'if HMO_ID has a value the destination table is the deprecated PACCARE table
            DB.AddInParameter(DBCommandWrapper, "@HMO_ID", DbType.Int32, UFCWGeneral.ToNullIntegerHandler(hmoID))
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@ORIGFROM_DATE", DbType.Date, originalFromDate)
            DB.AddInParameter(DBCommandWrapper, "@ORIGTHRU_DATE", DbType.Date, originalThruDate)
            DB.AddInParameter(DBCommandWrapper, "@ORIGONLINE_DATE", DbType.DateTime, originalOnlineDate)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub AddHRATransientData(ByVal familyID As Integer, ByVal relationID As Integer, ByVal effectiveDate As Date,
                                                ByVal eventDate As Date, ByVal transCode As String,
                                                ByVal memType As String, ByVal processStatus As String,
                                                ByVal userRights As String, Optional ByVal comments As String = "",
                                                Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.ADD_HRA_TRANSIENT_DATA_AND_COMMENTS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@EFFECTIVE_DATE", DbType.Date, effectiveDate)
            DB.AddInParameter(DBCommandWrapper, "@EVENT_DATE", DbType.Date, eventDate)


            DB.AddInParameter(DBCommandWrapper, "@TRANS_CODE", DbType.String, transCode)
            DB.AddInParameter(DBCommandWrapper, "@MEMTYPE", DbType.String, memType)
            DB.AddInParameter(DBCommandWrapper, "@PROCESS_STATUS", DbType.String, processStatus)


            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@COMMENTS", DbType.String, comments)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub AddHRATransientData(ByVal familyID As Integer, ByVal relationID As Integer, ByVal effectiveDate As Date,
                                                ByVal eventDate As Date, ByVal transCode As String,
                                                ByVal memType As String, ByVal processStatus As String,
                                                ByVal userRights As String,
                                                ByVal createDate As Date,
                                                Optional ByVal comments As String = "",
                                                Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.ADD_HRA_TRANSIENT_DATA_AND_COMMENTS_TS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@EFFECTIVE_DATE", DbType.Date, effectiveDate)
            DB.AddInParameter(DBCommandWrapper, "@EVENT_DATE", DbType.Date, eventDate)


            DB.AddInParameter(DBCommandWrapper, "@TRANS_CODE", DbType.String, transCode)
            DB.AddInParameter(DBCommandWrapper, "@MEMTYPE", DbType.String, memType)
            DB.AddInParameter(DBCommandWrapper, "@PROCESS_STATUS", DbType.String, processStatus)


            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@COMMENTS", DbType.String, comments)

            DB.AddInParameter(DBCommandWrapper, "@ONLINE_DATE", DbType.DateTime, createDate)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Function RetrieveRegAlertsByFamilyID(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Try

            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommand As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_REG_ALERTS_BY_FID"

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
    Public Shared Sub CreateRegAlert(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date, ByVal thruDate As Date, ByVal alertReason As Integer, ByVal passPhrase As String, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.CREATE_REG_ALERT"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@ALERT_REASON", DbType.Int32, alertReason)
            DB.AddInParameter(DBCommandWrapper, "@PASSPHRASE", DbType.String, passPhrase)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ALERT", DbType.Decimal, 1)
            ''db.AddInParameter(dbCommandWrapper, "@COMMENTS", DbType.String, Comments)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub UpdateRegAlert(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date, ByVal thruDate As Date, ByVal alertReason As Integer, ByVal passPhrase As String, ByVal termReason As Integer, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_REG_ALERT"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@ALERT_REASON", DbType.Int32, alertReason)
            DB.AddInParameter(DBCommandWrapper, "@PASSPHRASE", DbType.String, passPhrase)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)
            DB.AddInParameter(DBCommandWrapper, "@TERMINATION_REASON", DbType.Int32, termReason)
            '' db.AddInParameter(dbCommandWrapper, "@COMMENTS", DbType.String, Comments)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub UpdateRegAlerts(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date, ByVal thruDate As Date, ByVal alertReason As Integer, ByVal passPhrase As String, ByVal comments As String, ByVal termReason As Integer?, ByVal createDate As Date, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String
        Dim UpdateCount As Integer

        Try
            SQLCall = "FDBEL.UPDATE_REG_ALERTS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@ALERT_REASON", DbType.Int32, alertReason)
            DB.AddInParameter(DBCommandWrapper, "@PASSPHRASE", DbType.String, passPhrase)
            DB.AddInParameter(DBCommandWrapper, "@COMMENTS", DbType.String, comments)
            DB.AddInParameter(DBCommandWrapper, "@TERMINATION_REASON", DbType.Int32, termReason)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_DATE", DbType.DateTime, createDate)

            If transaction Is Nothing Then
                UpdateCount = DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                UpdateCount = DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub CreateRegAlertComments(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date, ByVal thruDate As Date, ByVal alertReason As Integer, ByVal passPhrase As String, ByVal userRights As String, ByVal comments As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.CREATE_REG_ALERT_COMMENTS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@ALERT_REASON", DbType.Int32, alertReason)
            DB.AddInParameter(DBCommandWrapper, "@PASSPHRASE", DbType.String, passPhrase)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ALERT", DbType.Decimal, 1)
            DB.AddInParameter(DBCommandWrapper, "@COMMENTS", DbType.String, comments)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub UpdateRegAlertComments(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date, ByVal thruDate As Date, ByVal alertReason As Integer, ByVal passPhrase As String, ByVal termReason As Integer?, ByVal comments As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_REG_ALERT_COMMENTS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@ALERT_REASON", DbType.Int32, alertReason)
            DB.AddInParameter(DBCommandWrapper, "@PASSPHRASE", DbType.String, passPhrase)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)
            DB.AddInParameter(DBCommandWrapper, "@TERMINATION_REASON", DbType.Int32, termReason)
            DB.AddInParameter(DBCommandWrapper, "@COMMENTS", DbType.String, comments)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub UpdateRegAlertCommentsandThrudate(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date, ByVal thruDate As Date, ByVal alertReason As Integer, ByVal passPhrase As String, ByVal termReason As Integer?, ByVal comments As String, ByVal originalFMDate As Date, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_REG_ALERTS_COMMENTS_AND_THRUDATE"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@ALERT_REASON", DbType.Int32, alertReason)
            DB.AddInParameter(DBCommandWrapper, "@PASSPHRASE", DbType.String, passPhrase)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)
            DB.AddInParameter(DBCommandWrapper, "@TERMINATION_REASON", DbType.Int32, termReason)
            DB.AddInParameter(DBCommandWrapper, "@COMMENTS", DbType.String, comments)
            DB.AddInParameter(DBCommandWrapper, "@ORIGFROM_DATE", DbType.Date, originalFMDate)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Function RUNIMMEDIATESELECT(ByVal SQLQuery As String) As DataTable
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim DS As DataSet

        Try

            DB = CMSDALCommon.CreateDatabase()

            If CBool(If(System.Configuration.ConfigurationManager.AppSettings("UseRUNIMMEDIATE") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("UseRUNIMMEDIATE")))) Then
                DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RUNIMMEDIATESELECTLOG")
                DB.AddInParameter(DBCommandWrapper, "SQLInput", DbType.String, SQLQuery)
            Else
                DBCommandWrapper = DB.GetSqlStringCommand(CStr(SQLQuery))
            End If

            DBCommandWrapper.CommandTimeout = CInt(If(System.Configuration.ConfigurationManager.AppSettings("RUNIMMEDIATETimeOut") Is Nothing, 120, CInt(System.Configuration.ConfigurationManager.AppSettings("RUNIMMEDIATETimeOut"))))
            DS = DB.ExecuteDataSet(DBCommandWrapper)

            If DS.Tables.Count > 0 Then
                Return DS.Tables(0)
            End If

            Return Nothing

        Catch ex As DB2Exception

            Throw

        Catch ex As Exception

            Throw
        End Try


    End Function

    Public Shared Sub AddRegRiskEvent(ByVal familyID As Integer, ByVal relationID As Integer,
                                      ByVal fromDate As Date, ByVal thruDate As Date,
                                      ByVal risk As Integer,
                                      ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.CREATE_REG_RISK_EVENT"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@RISK", DbType.Int32, risk)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub UpdateRegMasterRiskValue(ByVal familyID As Integer, ByVal relationID As Integer, ByVal risk As Integer, ByVal userRights As String, ByVal transaction As DbTransaction)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_REG_MASTER_RISK"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@RISK", DbType.Int32, risk)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)
        Catch ex As Exception

            Throw

        End Try
    End Sub
    Public Shared Sub UpdateRegRiskEvent(ByVal familyID As Integer, ByVal relationID As Integer,
                                      ByVal fromDate As Date, ByVal origFromDate As Date,
                                      ByVal risk As Integer,
                                      ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_REG_RISK_EVENT"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@ORIGFROM_DATE", DbType.Date, origFromDate)
            DB.AddInParameter(DBCommandWrapper, "@RISK", DbType.Int32, risk)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Function RetrieveRegRiskEvents(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Try
            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommand As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_REG_RISK_EVENTS_BY_FAMILYID"
            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int32, familyID)

            DBCommand.CommandTimeout = 180


            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If

                If ds IsNot Nothing Then
                    ds.Tables(0).TableName = "REG_RISK_EVENTS"
                End If

            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, "REG_RISK_EVENTS")
                Else
                    DB.LoadDataSet(DBCommand, ds, "REG_RISK_EVENTS", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveWaitingPeriodByFamilyid(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Try
            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommand As DbCommand

            Dim tablenames() As String = {"WAITING_PERIOD", "MEMBER_WAIT"}

            Dim SQLCall As String = "FDBEL.RETRIEVE_WAITPERIOD_BY_FAMILYID"
            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int32, familyID)

            DBCommand.CommandTimeout = 180


            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If

                If ds IsNot Nothing Then
                    ds.Tables(0).TableName = "WAITING_PERIOD"
                End If

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
        End Try
    End Function
    Public Shared Sub AddWaitingPeriod(ByVal ssn As Integer, ByVal familyID As Integer, ByVal relationID As Integer,
                                    ByVal waitDate As Date, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.ADD_WAITING_PERIOD"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@SSN", DbType.Int64, ssn)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@WAITPER_DATE", DbType.Date, waitDate)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub UpdateWaitingPeriod(ByVal familyID As Integer, ByVal waitDate As Date, ByVal userRights As String, ByVal transaction As DbTransaction)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_WAITING_PERIOD"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@WAITPER_DATE", DbType.Date, waitDate)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

        Catch ex As Exception

            Throw

        End Try
    End Sub

    Public Shared Function RetrieveA2CountByFamilyid(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommand As DbCommand
        Dim Tablenames() As String = {"A2BKCNT", "MEMBER_A2BKCNT"}

        Dim SQLCall As String = "FDBEL.RETRIEVE_A2COUNT_BY_FAMILYID"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int32, familyID)

            DBCommand.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If

                If ds IsNot Nothing Then
                    ds.Tables(0).TableName = "A2BKCNT"
                    ds.Tables(1).TableName = "MEMBER_A2BKCNT"
                End If

            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, Tablenames)
                Else
                    DB.LoadDataSet(DBCommand, ds, Tablenames, transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Sub UpdateA2Count(ByVal familyID As Integer, ByVal oldestHrs As Date, ByVal a2Count As Integer, ByVal reasonCode As String, ByVal userRights As String, ByVal acaEligDate As Date, ByVal transaction As DbTransaction)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_A2COUNT"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@OLDESTHRS", DbType.Date, oldestHrs)
            DB.AddInParameter(DBCommandWrapper, "@A2COUNT", DbType.Int64, a2Count)
            DB.AddInParameter(DBCommandWrapper, "@OVRIDE_RSNCD", DbType.String, reasonCode)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@ACA_HLPR_ELIG_DATE", DbType.Date, acaEligDate)
            DB.ExecuteNonQuery(DBCommandWrapper, transaction)
        Catch ex As Exception

            Throw

        End Try
    End Sub
    Public Shared Sub AddA2count(ByVal ssn As Integer, ByVal familyID As Integer, ByVal relationID As Integer, ByVal oldestHrs As Date, ByVal a2Count As Integer, ByVal reasonCode As String, ByVal memType As String, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.ADD_A2COUNT"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@SSN", DbType.Int64, ssn)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@OLDESTHRS", DbType.Date, oldestHrs)
            DB.AddInParameter(DBCommandWrapper, "@A2COUNT", DbType.Int64, a2Count)
            DB.AddInParameter(DBCommandWrapper, "@OVRIDE_RSNCD", DbType.String, reasonCode)
            DB.AddInParameter(DBCommandWrapper, "@MEMTYPE", DbType.String, memType)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Function RetrieveTerminationsBySSN(ByVal ssn As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommand As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.RETRIEVE_TERMINATIONS_BY_SSN"

            DBCommand = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommand, "@SSN", DbType.Int64, ssn)


            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If

                If ds IsNot Nothing Then
                    ds.Tables(0).TableName = "TERMS"
                End If

            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, "TERMS")
                Else
                    DB.LoadDataSet(DBCommand, ds, "TERMS", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function TestMedicalView(ByVal familyID As Integer, ByVal relationID As Integer, ByVal eligPeriod As Date) As DataSet

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String
        Dim DS As DataSet

        Try
            SQLCall = "FDBEL.TEST_VIEW_MEDICAL_ELIGIBILE"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@ELIG_PERIOD", DbType.Date, eligPeriod)

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            Return DS

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function AddEligSpecialAccountHours(ByVal familyID As Integer, ByVal relationID As Integer, ByVal pstperiod As Date,
                                                           ByVal acctNo As Integer, ByVal eligPeriod As Date, ByVal hrs As Decimal, ByVal hoursCode As String, ByVal userRights As String, ByVal consecServiceSW As Decimal,
                                                           ByVal weightedHrs As Decimal, ByVal remarks As String, Optional ByVal transaction As DbTransaction = Nothing) As Integer

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.ADD_ELIGIBILITY_SPECIAL_ACCOUNT_HOURS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)


            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@PSTPERIOD", DbType.Date, pstperiod)
            DB.AddInParameter(DBCommandWrapper, "@ACCTNO", DbType.Int64, acctNo)
            DB.AddInParameter(DBCommandWrapper, "@ELIG_PERIOD", DbType.Date, eligPeriod)
            DB.AddInParameter(DBCommandWrapper, "@HOURS", DbType.Decimal, hrs)
            DB.AddInParameter(DBCommandWrapper, "@HOURS_CODE", DbType.String, hoursCode)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@CONSEC_SERVICE_SW", DbType.Decimal, consecServiceSW)
            DB.AddInParameter(DBCommandWrapper, "@PROCESS_STATUS", DbType.String, "N")
            DB.AddInParameter(DBCommandWrapper, "@WEIGHTED_HOURS", DbType.Decimal, weightedHrs)
            DB.AddInParameter(DBCommandWrapper, "@REMARKS", DbType.String, remarks)
            DB.AddOutParameter(DBCommandWrapper, "@RECCOUNT", DbType.Int16, 2)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

            Return CInt(DB.GetParameterValue(DBCommandWrapper, "@RECCOUNT"))

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Sub DeleteEligSpecialAccountHours(ByVal familyID As Integer, ByVal relationID As Integer, ByVal acctNo As Integer,
                                                ByVal eligPeriod As Date, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing)


        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.DELETE_ELIGIBILITY_SPECIAL_ACCOUNT_HOURS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@ACCTNO", DbType.Int32, acctNo)

            DB.AddInParameter(DBCommandWrapper, "@ELIG_PERIOD", DbType.Date, eligPeriod)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub UpdateEligSpecialAccountHours(ByVal familyID As Integer, ByVal relationID As Integer, ByVal pstperiod As Date,
                                                           ByVal acctNo As Integer, ByVal eligPeriod As Date, ByVal hrs As Decimal, ByVal hoursCode As String, ByVal userRights As String, ByVal consecserviceSW As Decimal,
                                                           ByVal weightedhrs As Decimal, ByVal remarks As String, ByVal originalAcctNo As Integer, Optional ByVal transaction As DbTransaction = Nothing)


        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_ELIGIBILITY_SPECIAL_ACCOUNT_HOURS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@PSTPERIOD", DbType.Date, pstperiod)
            DB.AddInParameter(DBCommandWrapper, "@ACCTNO", DbType.Int64, acctNo)
            DB.AddInParameter(DBCommandWrapper, "@ELIG_PERIOD", DbType.Date, eligPeriod)
            DB.AddInParameter(DBCommandWrapper, "@HOURS", DbType.Decimal, hrs)
            DB.AddInParameter(DBCommandWrapper, "@HOURS_CODE", DbType.String, hoursCode)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@CONSEC_SERVICE_SW", DbType.Decimal, consecserviceSW)
            DB.AddInParameter(DBCommandWrapper, "@PROCESS_STATUS", DbType.String, "N")
            DB.AddInParameter(DBCommandWrapper, "@WEIGHTED_HOURS", DbType.Decimal, weightedhrs)
            DB.AddInParameter(DBCommandWrapper, "@REMARKS", DbType.String, remarks)
            DB.AddInParameter(DBCommandWrapper, "@OriginalACCTNO", DbType.Int64, originalAcctNo)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Function RetrieveMainframeviewByFamilyID(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommand As DbCommand

        Dim SQLCall As String = "FDBEL.RETRIEVE_ELIG_HOURS_FOR_MAINFRAME_VIEW"

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int32, familyID)

            DBCommand.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If

                If ds IsNot Nothing Then
                    ds.Tables(0).TableName = "ELIG_ACCT_HOURS"
                End If

            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, "ELIG_ACCT_HOURS")
                Else
                    DB.LoadDataSet(DBCommand, ds, "ELIG_ACCT_HOURS", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function RetrieveEligCoverageWithSplitByFamilyID(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommand As DbCommand

        Dim Tablenames() As String = {"ELIG_COVERAGE", "SPLIT_COVERAGE"}

        Dim SQLCall As String = "FDBEL.RETRIEVE_ELIG_COVERAGE_WITH_SPLITS_BY_FAMILY_ID"

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int32, familyID)

            DBCommand.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If

                If ds IsNot Nothing Then
                    ds.Tables(0).TableName = "ELIG_COVERAGE"
                    ds.Tables(1).TableName = "SPLIT_COVERAGE"
                End If

            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, Tablenames)
                Else
                    DB.LoadDataSet(DBCommand, ds, Tablenames, transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function RetrieveEligCoverageWithSplitByFamilyIDNoOverlap(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommand As DbCommand

        Dim Tablenames() As String = {"ELIG_COVERAGE", "SPLIT_COVERAGE"}

        Dim SQLCall As String = "FDBEL.RETRIEVE_ELIG_COVERAGE_WITH_SPLITS_BY_FID_NOOVERLAP"

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int32, familyID)

            DBCommand.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If

                If ds IsNot Nothing Then
                    ds.Tables(0).TableName = "ELIG_COVERAGE"
                    ds.Tables(1).TableName = "SPLIT_COVERAGE"
                End If

            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, Tablenames)
                Else
                    DB.LoadDataSet(DBCommand, ds, Tablenames, transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function RetrieveEligCoverageByFamilyID(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommand As DbCommand

        Dim tablenames() As String = {"ELIG_COVERAGE", "SPLIT_COVERAGE"}

        Dim SQLCall As String = "FDBEL.RETRIEVE_ELIG_COVERAGE_BY_FAMILY_ID"

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int32, familyID)

            DBCommand.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If

                If ds IsNot Nothing Then
                    ds.Tables(0).TableName = "ELIG_COVERAGE"
                    ds.Tables(1).TableName = "SPLIT_COVERAGE"
                End If

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
        End Try
    End Function
    Public Shared Function AddEligCoverage(ByVal activityTimestamp As DateTime, ByVal familyID As Integer, ByVal relationID As Integer, ByVal coveragetype As String,
                                                           ByVal fromdate As Date, ByVal thrudate As Date, ByVal coveragecode As Integer,
                                                           Optional ByVal generateletter As String = "Y", Optional ByVal transaction As DbTransaction = Nothing) As Integer

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.ADD_ELIG_COVERAGE_AND_PROCESS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@COVERAGE_TYPE", DbType.String, coveragetype)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromdate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thrudate)
            DB.AddInParameter(DBCommandWrapper, "@COVERAGE_CODE", DbType.Int64, coveragecode)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)
            DB.AddInParameter(DBCommandWrapper, "@GENERATE_LETTER", DbType.String, generateletter.ToUpper)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_DATE", DbType.DateTime, activityTimestamp)

            DB.AddOutParameter(DBCommandWrapper, "@COVERAGE_ID", DbType.Int32, 8)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

            Return CInt(DB.GetParameterValue(DBCommandWrapper, "@COVERAGE_ID"))

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Sub UpdateEligCoverage(ByVal activityTimestamp As Date, ByVal originalOnlineDate As Date, ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromdate As Date, ByVal thrudate As Date,
                                             ByVal coveragecode As Integer, ByVal coverageID As Integer, Optional ByVal generateletter As String = "Y",
                                             Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_ELIG_COVERAGE_ASSIGNMENT"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromdate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thrudate)
            DB.AddInParameter(DBCommandWrapper, "@COVERAGE_CODE", DbType.Int32, coveragecode)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)
            DB.AddInParameter(DBCommandWrapper, "@COVERAGE_ID", DbType.Int32, coverageID)
            DB.AddInParameter(DBCommandWrapper, "@GENERATE_LETTER", DbType.String, generateletter.ToUpper)
            DB.AddInParameter(DBCommandWrapper, "@ORIGONLINE_DATE", DbType.DateTime, originalOnlineDate)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_DATE", DbType.DateTime, activityTimestamp)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub DeleteEligCoverage(ByVal originalOnlineDate As DateTime, ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromdate As Date, ByVal thrudate As Date,
                                             ByVal coveragecode As Integer, ByVal coveragetype As String,
                                             Optional ByVal transaction As DbTransaction = Nothing)


        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.DELETE_ELIG_COVERAGE_ASSIGNMENT"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromdate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thrudate)
            DB.AddInParameter(DBCommandWrapper, "@COVERAGE_CODE", DbType.Int16, coveragecode)
            DB.AddInParameter(DBCommandWrapper, "@COVERAGE_TYPE", DbType.String, coveragetype)
            DB.AddInParameter(DBCommandWrapper, "@ORIGONLINE_DATE", DbType.DateTime, originalOnlineDate)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As SqlClient.SqlException
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub AddTerminations(ByVal ssn As Integer, ByVal termDate As Date, ByVal status As String, ByVal userRights As String, ByVal familyID As Integer,
                                      ByVal relationID As Integer, ByVal cobraLetter As String, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.ADD_TERMINATIONS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@SSN", DbType.Int64, ssn)
            DB.AddInParameter(DBCommandWrapper, "@TERM_DATE", DbType.Date, termDate)
            DB.AddInParameter(DBCommandWrapper, "@STATUS", DbType.String, status)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@COBRA_LETTER", DbType.String, cobraLetter)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub RemoveTerminations(ByVal ssn As Integer, ByVal termDate As Date, ByVal userRights As String, ByVal familyID As Integer,
                                    ByVal relationID As Integer, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.REMOVE_TERMINATIONS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@SSN", DbType.Int64, ssn)
            DB.AddInParameter(DBCommandWrapper, "@TERM_DATE", DbType.Date, termDate)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Function RetrieveRowStatusbeforeEligcalculation(ByVal familyID As Integer, ByVal eligPeriod As Date) As DataSet

        Dim DB As Database
        Dim DBCommand As DbCommand
        Dim SQLCall As String
        Dim DS As DataSet = Nothing

        Try
            DB = CMSDALCommon.CreateDatabase()
            SQLCall = "FDBEL.RETRIEVE_ROW_SATUSES_BFR_ELIG_CALCULATION"

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommand, "@ELIG_PERIOD", DbType.Date, eligPeriod)

            DBCommand.CommandTimeout = 180
            If DS Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommand)
            End If

            Return DS

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Shared Function GetWeightedhoursandStatus(ByVal familyID As Integer, ByVal eligPeriod As Date, ByVal mpplan As String, mpstatus As String, ByVal ecmemtype As String) As DataTable

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommand As DbCommand
        Dim SQLCall As String
        Dim DS As DataSet = Nothing

        Try
            SQLCall = "FDBEL.RETRIEVE_WEIGHTED_HOURS_AND_STATUS"

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommand, "@ELIG_PERIOD", DbType.Date, eligPeriod)
            DB.AddInParameter(DBCommand, "@PLANTYPE", DbType.String, mpplan.Trim.ToUpper)
            DB.AddInParameter(DBCommand, "@STATUS", DbType.String, mpstatus.Trim.ToUpper)
            DB.AddInParameter(DBCommand, "@LAST_MEMTYPE", DbType.String, ecmemtype.Trim.ToUpper)

            DS = DB.ExecuteDataSet(DBCommand)

            If DS.Tables.Count > 0 Then
                DS.Tables(0).TableName = "WEIGHTD_HOURS"
            End If

            If DS.Tables.Count > 0 Then
                Return DS.Tables(0)
            End If

            Return Nothing

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function RetrieveCommonDataforEligcalculation(ByVal familyID As Integer, ByVal eligPeriod As Date) As DataSet

        Dim DB As Database
        Dim DBCommand As DbCommand
        Dim SQLCall As String
        Dim DS As DataSet = Nothing

        Try
            DB = CMSDALCommon.CreateDatabase()
            SQLCall = "FDBEL.RETRIEVE_COMMON_DATA_FOR_ELIG_CALCULATION"

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommand, "@ELIG_PERIOD", DbType.Date, eligPeriod)

            DBCommand.CommandTimeout = 180
            If DS Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommand)
            End If

            Return DS

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function RetrieveDataforEligcalculation(ByVal familyID As Integer, ByVal eligPeriod As Date) As DataSet

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommand As DbCommand
        Dim SQLCall As String
        Dim DS As DataSet = Nothing

        Try
            SQLCall = "FDBEL.RETRIEVE_DATA_FOR_ELIG_CALCULATION"

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommand, "@ELIG_PERIOD", DbType.Date, eligPeriod)

            DS = DB.ExecuteDataSet(DBCommand)

            Return DS

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function UpdateA2KKCNTfromEligCalculation(ByVal familyID As Integer, ByVal memtype As String, Optional ByVal transaction As DbTransaction = Nothing) As Integer
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_A2BKCNT_FROM_ELIG_CALCULATION"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@MEMTYPE", DbType.String, memtype.ToUpper)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)
            DB.AddOutParameter(DBCommandWrapper, "@RECORDCOUNT", DbType.Int16, 1)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If
            Return CInt(DB.GetParameterValue(DBCommandWrapper, "@RECORDCOUNT"))

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function UpdateA2KKCNTfromEligCalculation(ByVal familyID As Integer, ByVal eligPeriod As Date, Optional ByVal transaction As DbTransaction = Nothing) As Integer
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_A2BKCNT_FROM_ELIG_CALCULATION1"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@ELIG_PERIOD", DbType.Date, eligPeriod)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)
            DB.AddOutParameter(DBCommandWrapper, "@RECORDCOUNT", DbType.Int16, 1)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If
            Return CInt(DB.GetParameterValue(DBCommandWrapper, "@RECORDCOUNT"))

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function UpdateEligCalelementsfromEligCalculation(ByVal familyID As Integer, ByVal memtype As String, Optional ByVal transaction As DbTransaction = Nothing) As Integer
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_ELG_CALC_ELMENTS_FM_ELIGCALC"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@MEMTYPE", DbType.String, memtype.ToUpper)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)
            DB.AddOutParameter(DBCommandWrapper, "@RECORDCOUNT", DbType.Int16, 1)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If
            Return CInt(DB.GetParameterValue(DBCommandWrapper, "@RECORDCOUNT"))

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function InsertMTHDTL(ByVal familyID As Integer, ByVal eligperiod As Date, ByVal status As String,
                                      ByVal plantype As String, ByVal memtype As String, ByVal localno As Integer,
                                      ByVal medplan As Integer, ByVal dentalplan As Integer,
                                      ByVal medSW As Decimal, ByVal dentSW As Decimal, ByVal premiumSW As Decimal, ByVal familySW As Decimal,
                                      ByVal retplan As String, ByVal a2count As Int16, ByVal ab1stelig As Date, ByVal breakSW As Decimal, ByVal transaction As DbTransaction) As Integer

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.INSERT_MTHDTL"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, 0)
            DB.AddInParameter(DBCommandWrapper, "@ELIG_PERIOD", DbType.Date, eligperiod)
            DB.AddInParameter(DBCommandWrapper, "@STATUS", DbType.String, status)

            DB.AddInParameter(DBCommandWrapper, "@PLANTYPE", DbType.String, plantype)
            DB.AddInParameter(DBCommandWrapper, "@MEMTYPE", DbType.String, memtype)
            DB.AddInParameter(DBCommandWrapper, "@LOCALNO", DbType.Int64, localno)
            DB.AddInParameter(DBCommandWrapper, "@MEDICAL_PLAN", DbType.Int64, medplan)
            DB.AddInParameter(DBCommandWrapper, "@DENTAL_PLAN", DbType.Int64, dentalplan)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)


            DB.AddInParameter(DBCommandWrapper, "@MED_ELIG_SW", DbType.Decimal, medSW)
            DB.AddInParameter(DBCommandWrapper, "@DEN_ELIG_SW", DbType.Decimal, dentSW)
            DB.AddInParameter(DBCommandWrapper, "@PREMIUM_SW", DbType.Decimal, premiumSW)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_SW", DbType.Decimal, familySW)

            DB.AddInParameter(DBCommandWrapper, "@RET_PLAN", DbType.String, retplan)
            DB.AddInParameter(DBCommandWrapper, "@A2COUNT", DbType.Int16, a2count)
            DB.AddInParameter(DBCommandWrapper, "@PLAN_AB_1ST_ELGDATE", DbType.Date, ab1stelig)
            DB.AddInParameter(DBCommandWrapper, "@BREAK_IN_SERVICE_SW", DbType.Decimal, breakSW)

            If Not transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper)
            End If

        Catch ex As Exception

            Throw

        End Try
    End Function
    Public Shared Sub UpdateEligCalelementsASStatusChange(ByVal familyID As Integer, ByRef memtype As String, ByRef recordcount As Short, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_ELG_CALC_ELMENTS_BCOZ_OF_STATUS_CHANGE"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)
            DB.AddOutParameter(DBCommandWrapper, "@RECORDCOUNT", DbType.Int16, 1)
            DB.AddOutParameter(DBCommandWrapper, "@MEMTYPE", DbType.String, 5)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

            memtype = CStr(DB.GetParameterValue(DBCommandWrapper, "@MEMTYPE"))
            recordcount = CShort(DB.GetParameterValue(DBCommandWrapper, "@RECORDCOUNT"))

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Function DeleteMTHDTLfromEligCalculation(ByVal familyID As Integer, ByVal eligPeriod As Date, Optional ByVal transaction As DbTransaction = Nothing) As Integer
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.DELETE_MTHDTL_FM_ELIG_CALCULATION"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@ELIG_PERIOD", DbType.Date, eligPeriod)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)
            DB.AddOutParameter(DBCommandWrapper, "@RECORDCOUNT", DbType.Int16, 1)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If
            Return CInt(DB.GetParameterValue(DBCommandWrapper, "@RECORDCOUNT"))

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Sub UpdateSwitchesAfterEligCalculation(ByVal familyID As Integer, ByVal eligPeriod As Date, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_SWITCHES_AFTER_ELIG_CALCULATION"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@ELIG_PERIOD", DbType.Date, eligPeriod)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Function CalculateEligibility(ByVal familyID As Integer, ByVal lastmemtype As String, ByVal lastlocal As Short, Optional ByVal transaction As DbTransaction = Nothing) As Integer
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.CALCULATE_ELIGIBILITY_BY_FID"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@ELGCALC_FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@ELGCALC_LAST_MEMTYPE", DbType.String, lastmemtype)
            DB.AddInParameter(DBCommandWrapper, "@ELGCALC_LAST_LOCAL", DbType.Int16, lastlocal)
            DB.AddInParameter(DBCommandWrapper, "@BATCH_OR_ONLINE_PROCESSING", DbType.String, "ONLINE")
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_USERID", DbType.String, _DomainUser.ToUpper)

            DB.AddOutParameter(DBCommandWrapper, "@CCSQLMSG_OPERATION", DbType.String, 9)
            DB.AddOutParameter(DBCommandWrapper, "@CCSQLMSG_TABLE", DbType.String, 30)
            DB.AddOutParameter(DBCommandWrapper, "@CCSQLMSG_SQLCODE", DbType.Int32, 10)
            DB.AddOutParameter(DBCommandWrapper, "@CCSQLMSG_SQLSTATE", DbType.String, 5)
            DB.AddOutParameter(DBCommandWrapper, "@CCSQLMSG_SQLERRMC", DbType.String, 13)


            DB.AddOutParameter(DBCommandWrapper, "@WS_PARAGRAPH_ID", DbType.String, 30)
            DB.AddOutParameter(DBCommandWrapper, "@MONTHLY_DETAIL_UPDATES", DbType.Int32, 10)
            DB.AddOutParameter(DBCommandWrapper, "@MONTHLY_DETAIL_INSERTS", DbType.Int32, 10)
            DB.AddOutParameter(DBCommandWrapper, "@ELGCALC-UPDATES", DbType.Int32, 10)
            DB.AddOutParameter(DBCommandWrapper, "@ECE-MEMTYPE-UPDATES", DbType.Int32, 10)
            DB.AddOutParameter(DBCommandWrapper, "@A2C-MEMTYPE-UPDATES", DbType.Int32, 10)
            DB.AddOutParameter(DBCommandWrapper, "@TERMS-FOUND", DbType.Int32, 10)
            DB.AddOutParameter(DBCommandWrapper, "@E01_INSERTS", DbType.Int32, 10)
            DB.AddOutParameter(DBCommandWrapper, "@HOURS_ROWS_FETCHED", DbType.Int32, 10)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

            Return CInt(DB.GetParameterValue(DBCommandWrapper, "@CCSQLMSG_SQLCODE"))

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Sub ResetswitchesforEligcalculation(ByVal familyid As Integer, ByVal eligPeriod As Date)
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommand As DbCommand
        Dim SQLCall As String
        Dim DS As DataSet = Nothing

        Try
            SQLCall = "FDBEL.RESET_SWITCHES_FOR_ELIG_CALCULATION"

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int64, familyid)
            DB.AddInParameter(DBCommand, "@ELIG_PERIOD", DbType.Date, eligPeriod)
            DB.AddInParameter(DBCommand, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)

            DB.ExecuteDataSet(DBCommand)

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub AddEligCalcElements(ByVal familyID As Integer,
                                   ByVal entryDate As Date, ByVal memtype As String, ByVal lastlocal As Integer, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.ADD_ELIG_CALC_ELMENTS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@ENTRY_DATE", DbType.Date, entryDate)
            DB.AddInParameter(DBCommandWrapper, "@MEMTYPE", DbType.String, memtype)
            DB.AddInParameter(DBCommandWrapper, "@LAST_LOCAL", DbType.Int64, lastlocal)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub UpdateEligCalcElements(ByVal familyID As Integer, ByVal entryDate As Date,
                                             ByVal memtype As String, ByVal lastlocal As Integer, ByVal termDate As Date, ByVal transaction As DbTransaction)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_ELIG_CALC_ELMENTS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@ENTRY_DATE", DbType.Date, entryDate)
            DB.AddInParameter(DBCommandWrapper, "@MEMTYPE", DbType.String, memtype)
            DB.AddInParameter(DBCommandWrapper, "@LAST_LOCAL", DbType.Int64, lastlocal)
            DB.AddInParameter(DBCommandWrapper, "@TERM_DATE", DbType.Date, termDate)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

        Catch ex As Exception

            Throw

        End Try
    End Sub

    Public Shared Function RetrieveEligRetireeElementsByFamilyID(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Try

            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommand As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_ELGRETIRE_ELEMENTS_BY_FAMILYID"

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int64, familyID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If

                If ds IsNot Nothing Then
                    ds.Tables(0).TableName = "ELGRETIREE_ELEMENTS"
                End If

            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, "ELGRETIREE_ELEMENTS")
                Else
                    DB.LoadDataSet(DBCommand, ds, "ELGRETIREE_ELEMENTS", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Sub AddEligRetireElements(ByVal familyID As Integer, ByVal fromDate As Date, ByVal thruDate As Date,
                                            ByVal retireeplan As String, ByVal retireepercent As Decimal, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.ADD_ELIG_RETIRE_ELEMENTS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@RETIREE_PLAN", DbType.String, retireeplan)
            DB.AddInParameter(DBCommandWrapper, "@RETIREE_PERCENT", DbType.Decimal, retireepercent)
            DB.AddInParameter(DBCommandWrapper, "@MTHDTL_PLAN", DbType.String, retireeplan & CStr(retireepercent))
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub AddEligRetireeElements(ByVal familyID As Integer, ByVal fromDate As Date, ByVal thruDate As Date,
                                             ByVal retireeplan As String, ByVal retireepercent As Decimal,
                                             ByVal onlineDate As Date, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.ADD_ELIG_RETIREE_ELEMENTS_TS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@RETIREE_PLAN", DbType.String, retireeplan)
            DB.AddInParameter(DBCommandWrapper, "@RETIREE_PERCENT", DbType.Decimal, retireepercent)
            DB.AddInParameter(DBCommandWrapper, "@MTHDTL_PLAN", DbType.String, retireeplan & CStr(retireepercent))
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_DATE", DbType.DateTime, onlineDate)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub UpdateEligRetireElements(ByVal familyID As Integer, ByVal fromDate As Date, ByVal thruDate As Date,
                                               ByVal retireeplan As String, ByVal retireepercent As Decimal, ByVal originalfromDate As Date, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_ELIG_RETIRE_ELEMENTS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@RETIREE_PLAN", DbType.String, retireeplan)
            DB.AddInParameter(DBCommandWrapper, "@RETIREE_PERCENT", DbType.Decimal, retireepercent)
            DB.AddInParameter(DBCommandWrapper, "@MTHDTL_PLAN", DbType.String, retireeplan & CStr(retireepercent))
            DB.AddInParameter(DBCommandWrapper, "@ORGFROM_DATE", DbType.Date, originalfromDate)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub UpdateEligRetireeElements(ByVal familyID As Integer, ByVal fromDate As Date, ByVal thruDate As Date,
                                                ByVal retireeplan As String, ByVal retireepercent As Decimal,
                                                ByVal originalfromDate As Date, ByVal onlineDate As Date, ByVal originalOnlineDate As Date, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_ELIG_RETIREE_ELEMENTS_TS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@RETIREE_PLAN", DbType.String, retireeplan)
            DB.AddInParameter(DBCommandWrapper, "@RETIREE_PERCENT", DbType.Decimal, retireepercent)
            DB.AddInParameter(DBCommandWrapper, "@RET_PLAN", DbType.String, retireeplan & CStr(retireepercent))
            DB.AddInParameter(DBCommandWrapper, "@ORGFROM_DATE", DbType.Date, originalfromDate)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_DATE", DbType.DateTime, onlineDate)
            DB.AddInParameter(DBCommandWrapper, "@ORIGINAL_ONLINE_DATE", DbType.DateTime, originalOnlineDate)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub DeleteEligRetireeElements(ByVal familyID As Integer, ByVal fromDate As Date, ByVal thruDate As Date,
                                                ByVal onlineDate As Date, ByVal originalOnlineDate As Date, Optional ByVal transaction As DbTransaction = Nothing)


        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.DELETE_ELIG_RETIREE_ELEMENTS_TS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@RET_PLAN", DbType.String, DBNull.Value) 'This remains for backwards compatibility only
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_DATE", DbType.DateTime, onlineDate)
            DB.AddInParameter(DBCommandWrapper, "@ORIGINAL_ONLINE_DATE", DbType.DateTime, originalOnlineDate)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub DeleteEligRetireElements(ByVal familyID As Integer, ByVal fromDate As Date, ByVal thruDate As Date,
                                                Optional ByVal retireeplan As String = "", Optional ByVal retireepercent As String = "", Optional ByVal transaction As DbTransaction = Nothing)


        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try

            DB = CMSDALCommon.CreateDatabase()

            SQLCall = "FDBEL.DELETE_ELIG_RETIRE_ELEMENTS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@MTHDTL_PLAN", DbType.String, retireeplan & retireepercent)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Function RetrieveEligMTHDTLByFamilyID(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommand As DbCommand

        Dim SQLCall As String = "FDBEL.RETRIEVE_ELIG_MTHDTL_FOR_SSN_CHANGE"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int32, familyID)

            DBCommand.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If

                If ds IsNot Nothing Then
                    ds.Tables(0).TableName = "ELIG_MTHDTL"
                End If

            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, "ELIG_MTHDTL")
                Else
                    DB.LoadDataSet(DBCommand, ds, "ELIG_MTHDTL", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function RetrieveEligHoursforSSNchange(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommand As DbCommand

        Dim SQLCall As String = "FDBEL.RETRIEVE_ELIG_ACCT_HOURS_FOR_SSN_CHANGE"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int32, familyID)

            DBCommand.CommandTimeout = 180


            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If

                If ds IsNot Nothing Then
                    ds.Tables(0).TableName = "ELIG_ACCT_HOURS"
                End If

            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, "ELIG_ACCT_HOURS")
                Else
                    DB.LoadDataSet(DBCommand, ds, "ELIG_ACCT_HOURS", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Sub InsertMTHDTLfromSSNchanges(ByVal oldSSN As Integer, ByVal oldFamilyID As Integer, ByVal newSSN As Integer,
                                            ByVal newFamilyID As Integer, ByVal remarks As String, ByVal fromEligperiod As Date, ByVal toEligPeriod As Date, ByVal transaction As DbTransaction)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase()

            SQLCall = "FDBEL.INSERT_MTHDTL_FROM_SSN_CHANGES"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@OLDSSN", DbType.Int64, oldSSN)
            DB.AddInParameter(DBCommandWrapper, "@OLDFAMILY_ID", DbType.Int32, oldFamilyID)

            DB.AddInParameter(DBCommandWrapper, "@NEWSSN", DbType.Int64, newSSN)
            DB.AddInParameter(DBCommandWrapper, "@NEWFAMILY_ID", DbType.Int32, newFamilyID)

            DB.AddInParameter(DBCommandWrapper, "@FROM_ELIG_PERIOD", DbType.Date, fromEligperiod)
            DB.AddInParameter(DBCommandWrapper, "@THRU_ELIG_PERIOD", DbType.Date, toEligPeriod)

            DB.AddInParameter(DBCommandWrapper, "@REMARKS", DbType.String, remarks)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)

            If transaction IsNot Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper)
            End If

        Catch ex As Exception

            Throw

        End Try
    End Sub
    Public Shared Function InsertACCTHOURSfromSSNchanges(ByVal oldSSN As Integer, ByVal oldfamilyID As Integer, ByVal newSSN As Integer,
                                         ByVal newfamilyID As Integer, ByVal remarks As String, ByVal fromeligperiod As Date, ByVal toeligperiod As Date, ByVal transaction As DbTransaction) As Decimal

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase()
            SQLCall = "FDBEL.INSERT_ELG_ACCT_HRS_FROM_SSN_CHANGES"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@OLDSSN", DbType.Int64, oldSSN)
            DB.AddInParameter(DBCommandWrapper, "@OLDFAMILY_ID", DbType.Int32, oldfamilyID)

            DB.AddInParameter(DBCommandWrapper, "@NEWSSN", DbType.Int64, newSSN)
            DB.AddInParameter(DBCommandWrapper, "@NEWFAMILY_ID", DbType.Int32, newfamilyID)

            DB.AddInParameter(DBCommandWrapper, "@FROM_ELIG_PERIOD", DbType.Date, fromeligperiod)
            DB.AddInParameter(DBCommandWrapper, "@THRU_ELIG_PERIOD", DbType.Date, toeligperiod)

            DB.AddInParameter(DBCommandWrapper, "@REMARKS", DbType.String, remarks)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)

            DB.AddOutParameter(DBCommandWrapper, "@MOVED_HOURS", DbType.Decimal, 7)

            If Not transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper)
            End If

            Return CDec(DB.GetParameterValue(DBCommandWrapper, "@MOVED_HOURS"))

        Catch ex As Exception

            Throw

        End Try
    End Function
    Public Shared Function RetrieveSSNxRef(ByVal ssn As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommand As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase()
            SQLCall = "FDBEL.RETRIEVE_SSN_XREF_VALUES"

            DBCommand = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommand, "@SSN", DbType.Int64, ssn)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If

                If ds IsNot Nothing Then
                    ds.Tables(0).TableName = "SSNO_XREF_DATA"
                End If

            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, "SSNO_XREF_DATA")
                Else
                    DB.LoadDataSet(DBCommand, ds, "SSNO_XREF_DATA", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Shared Function UpdateMemberSocial(ByVal familyID As Integer, ByVal newSSN As Integer, Optional ByVal transaction As DbTransaction = Nothing) As Boolean

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase()
            SQLCall = "FDBEL.UPDATE_MEMBER_SOCIAL"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@NEWSSN", DbType.Int64, newSSN)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

            Return True

        Catch ex As DB2Exception
            Throw
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

    Public Shared Function RetrieveHOURSbySSN(ByVal ssn As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommand As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase()
            SQLCall = "FDBEL.RETRIEVE_HOURS_BY_SSN"

            DBCommand = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommand, "@SSN", DbType.Int64, ssn)


            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If

                If ds IsNot Nothing Then
                    ds.Tables(0).TableName = "HOURS"
                End If

            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, "HOURS")
                Else
                    DB.LoadDataSet(DBCommand, ds, "HOURS", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Sub UpdateEligMTHDTLMemtype(ByVal familyID As Integer, ByVal eligPeriod As Date,
                                                ByVal memtype As String, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase()
            SQLCall = "FDBEL.UPDATE_ELIG_MTHDTL_MEMTYPE"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, 0)
            DB.AddInParameter(DBCommandWrapper, "@ELIG_PERIOD", DbType.Date, eligPeriod)
            DB.AddInParameter(DBCommandWrapper, "@MEMTYPE", DbType.String, memtype)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Function RetrieveSummaryByFamilyID(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommand As DbCommand
        Dim SQLCall As String = "FDBEL.RETRIEVE_SUMMARY_DATA"

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

            End If

            Return ds
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Sub INSERTRegAddressForBatch(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date, ByVal thruDate As Date,
                                                ByVal partSSNO As Integer,
                                                ByVal foreignSW As Decimal, ByVal country As String, ByVal address1 As String, ByVal address2 As String,
                                                ByVal city As String, ByVal State As String, ByVal zip1 As Integer?, ByVal zip2 As Integer?,
                                                ByVal userRights As String, ByVal addressType As Integer,
                                                ByVal accountid As String, ByVal entityname As String, ByVal addressorigin As String, ByVal additionaldeliveryinfo As String, ByVal foreignProvince As String, ByVal foreignPostCode As String, Optional ByVal transaction As DbTransaction = Nothing)

        If thruDate = CDate("12/31/9998") Then
            thruDate = CDate("12/31/9999")
        End If

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.INSERT_REG_ADDRESS_FOR_BATCH"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int64, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSNO", DbType.Int32, partSSNO)

            DB.AddInParameter(DBCommandWrapper, "@FOREIGN_SW", DbType.Decimal, foreignSW)
            DB.AddInParameter(DBCommandWrapper, "@COUNTRY", DbType.String, UFCWGeneral.ToNullStringHandler(country))
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS1", DbType.String, address1)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS2", DbType.String, UFCWGeneral.ToNullStringHandler(address2))
            DB.AddInParameter(DBCommandWrapper, "@CITY", DbType.String, city)
            DB.AddInParameter(DBCommandWrapper, "@STATE", DbType.String, UFCWGeneral.ToNullStringHandler(State))
            DB.AddInParameter(DBCommandWrapper, "@ZIP1", DbType.Int32, UFCWGeneral.ToNullIntegerHandler(zip1))
            DB.AddInParameter(DBCommandWrapper, "@ZIP2", DbType.Int32, UFCWGeneral.ToNullIntegerHandler(zip2))
            DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String, userRights)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_TYPE", DbType.Int64, addressType)

            DB.AddInParameter(DBCommandWrapper, "@ACCOUNT_ID", DbType.String, UFCWGeneral.ToNullStringHandler(accountid))
            DB.AddInParameter(DBCommandWrapper, "@ENTITY_NAME", DbType.String, UFCWGeneral.ToNullStringHandler(entityname))
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_ORIGIN", DbType.String, addressorigin)
            DB.AddInParameter(DBCommandWrapper, "@ADDITIONAL_DELIVERY_INFO", DbType.String, UFCWGeneral.ToNullStringHandler(additionaldeliveryinfo))
            DB.AddInParameter(DBCommandWrapper, "@FOREIGN_PROVINCE", DbType.String, UFCWGeneral.ToNullStringHandler(foreignProvince))
            DB.AddInParameter(DBCommandWrapper, "@FOREIGN_POSTCODE", DbType.String, UFCWGeneral.ToNullStringHandler(foreignPostCode))

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
#Region "Cobra"

    Public Shared Sub AddCOBRAQELifeEvent(ByVal familyID As Integer, ByVal relationID As Integer, ByVal fromDate As Date, ByVal thruDate As Date,
                                                ByVal eventCode As Integer, ByVal eventDate As Date,
                                                ByVal termCode As Integer, ByVal termDate As Date?,
                                                ByVal userRights As String, ByVal DeleteFlag As Decimal, ByVal csoIndicator As String,
                                                ByVal parentRelationID As Integer, ByRef recordcount As Short, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.ADD_COBRA_QE_LIFE_EVENT"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FROM_DATE", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@THRU_DATE", DbType.Date, thruDate)

            DB.AddInParameter(DBCommandWrapper, "@EVENT_CODE", DbType.Int16, eventCode)
            DB.AddInParameter(DBCommandWrapper, "@EVENT_DATE", DbType.Date, eventDate)
            DB.AddInParameter(DBCommandWrapper, "@TERM_CODE", DbType.Int16, termCode)
            DB.AddInParameter(DBCommandWrapper, "@TERM_DATE", DbType.Date, termDate)

            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)
            DB.AddInParameter(DBCommandWrapper, "@DELETE_FLAG", DbType.Decimal, DeleteFlag)
            DB.AddInParameter(DBCommandWrapper, "@SPCL_IND", DbType.String, csoIndicator)
            DB.AddInParameter(DBCommandWrapper, "@SPARENT_RELID", DbType.Int16, parentRelationID)
            DB.AddOutParameter(DBCommandWrapper, "@RECORDCOUNT", DbType.Int16, 1)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

            recordcount = CShort(DB.GetParameterValue(DBCommandWrapper, "@RECORDCOUNT"))

        Catch ex As DB2Exception
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Function RetrieveCOBRAQEEventsByFamilyID(ByVal familyID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Try

            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommand As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_COBRA_QE_EVENTS_BY_FAMILYID"

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
                    DB.LoadDataSet(DBCommand, ds, "REG_LIFE_EVENTS")
                Else
                    DB.LoadDataSet(DBCommand, ds, "REG_LIFE_EVENTS", transaction)
                End If
            End If
            If ds.Tables.Count > 0 Then
                ds.Tables(0).TableName = "REG_LIFE_EVENTS"
            End If
            Return ds
        Catch ex As Exception
            Throw
            Return Nothing
        End Try
    End Function
    Public Shared Function RetrieveQEEnrollmentsByFamilyID(ByVal familyID As Integer, ByVal QETYPE As String, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Try

            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommand As DbCommand
            Dim SQLCall As String = "FDBEL.RETRIEVE_QE_ENROLLMENTS_BY_FAMILYID"

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommand, "@FAMILY_ID", DbType.Int64, familyID)
            DB.AddInParameter(DBCommand, "@QE_TYPE", DbType.String, QETYPE)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommand)
                Else
                    ds = DB.ExecuteDataSet(DBCommand, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommand, ds, "COBRA_ENROLLED")
                Else
                    DB.LoadDataSet(DBCommand, ds, "COBRA_ENROLLED", transaction)
                End If
            End If
            If ds.Tables.Count > 0 Then
                ds.Tables(0).TableName = "COBRA_ENROLLED"
            End If
            Return ds
        Catch ex As Exception
            Throw
            Return Nothing
        End Try
    End Function
    Public Shared Sub AddCOBRAEnrolled(ByVal familyID As Integer, ByVal relationID As Integer,
                                      ByVal SSNO As Integer?, ByVal eventDate As Date?, ByVal qetype As String, ByVal rowstatus As String,
                                      ByVal corememtype As String, ByVal corememstat As String, ByVal corelevel As String, ByVal corenetwork As String,
                                      ByVal typecore As String, ByVal typecoveage As String, ByVal typefamily As String, ByVal subsidyflag As String,
                                      ByVal subsidycntr As Decimal, ByVal startingcobrarate As Decimal, ByVal qereceiveddate As Date?,
                                      ByVal qeexpirydate As Date?, ByVal qeenrolldate As Date?, ByVal coupnstoprint As Decimal,
                                      ByVal lostcoveragedate As Date?, ByVal medicalplan As Decimal, ByVal firstduedate As Date?,
                                      ByVal lastpayment As Decimal, ByVal monthseligible As Decimal, ByVal userRights As String, ByVal transaction As DbTransaction)



        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.ADD_COBRA_ENROLLED"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)
            DB.AddInParameter(DBCommandWrapper, "@SSNO", DbType.Int64, SSNO)
            DB.AddInParameter(DBCommandWrapper, "@EVENT_DATE", DbType.Date, eventDate)
            DB.AddInParameter(DBCommandWrapper, "@QE_TYPE", DbType.String, qetype)

            DB.AddInParameter(DBCommandWrapper, "@ROW_STATUS", DbType.String, rowstatus)
            DB.AddInParameter(DBCommandWrapper, "@CORE_MEMTYPE", DbType.String, corememtype)
            DB.AddInParameter(DBCommandWrapper, "@CORE_MEMSTAT ", DbType.String, corememstat)
            DB.AddInParameter(DBCommandWrapper, "@CORE_LEVEL", DbType.String, corelevel)
            DB.AddInParameter(DBCommandWrapper, "@CORE_NETWORK", DbType.String, corenetwork)

            DB.AddInParameter(DBCommandWrapper, "@TYPE_CORE", DbType.String, typecore)
            DB.AddInParameter(DBCommandWrapper, "@TYPE_COVERAGE", DbType.String, typecoveage)
            DB.AddInParameter(DBCommandWrapper, "@TYPE_FAMILY", DbType.String, typefamily)
            DB.AddInParameter(DBCommandWrapper, "@SUBSIDY_FLAG", DbType.String, subsidyflag)
            DB.AddInParameter(DBCommandWrapper, "@SUBSIDY_CNTR", DbType.Decimal, subsidycntr)

            DB.AddInParameter(DBCommandWrapper, "@STARTING_COBRATE", DbType.Decimal, startingcobrarate)
            DB.AddInParameter(DBCommandWrapper, "@QE_RECEIVED_DATE", DbType.Date, qereceiveddate)
            DB.AddInParameter(DBCommandWrapper, "@QE_EXPIRE_DATE", DbType.Date, qeexpirydate)
            DB.AddInParameter(DBCommandWrapper, "@QE_ENROLL_DATE", DbType.Date, qeenrolldate)
            DB.AddInParameter(DBCommandWrapper, "@COUPONS_TO_PRINT", DbType.Decimal, coupnstoprint)

            DB.AddInParameter(DBCommandWrapper, "@LOST_COVERAGE_DATE", DbType.Date, lostcoveragedate)
            DB.AddInParameter(DBCommandWrapper, "@MEDICAL_PLAN", DbType.Decimal, medicalplan)
            DB.AddInParameter(DBCommandWrapper, "@FIRST_DUE_DATE", DbType.Date, firstduedate)
            DB.AddInParameter(DBCommandWrapper, "@LAST_PAYMENT", DbType.Decimal, lastpayment)
            DB.AddInParameter(DBCommandWrapper, "@MONTHS_ELIGIBLE", DbType.Decimal, monthseligible)

            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            If Not transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper)
            End If



        Catch ex As Exception

            Throw

        End Try
    End Sub
    Public Shared Sub UpdateCOBRAEnrolled(ByVal CobraID As Integer, ByVal familyID As Integer, ByVal relationID As Integer,
                                      ByVal eventDate As Date?, ByVal corelevel As String, ByVal corenetwork As String,
                                      ByVal typecore As String, ByVal typecoveage As String, ByVal typefamily As String, ByVal subsidyflag As String,
                                      ByVal subsidycntr As Decimal, ByVal coupnstoprint As Decimal,
                                      ByVal lostcoveragedate As Date?, ByVal medicalplan As Decimal, ByVal firstduedate As Date?,
                                      ByVal lastpayment As Decimal, ByVal monthseligible As Decimal, ByVal userRights As String, ByVal transaction As DbTransaction)



        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.UPDATE_COBRA_ENROLLED"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@COBRA_ID", DbType.Int32, CobraID)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)
            DB.AddInParameter(DBCommandWrapper, "@EVENT_DATE", DbType.Date, eventDate)

            DB.AddInParameter(DBCommandWrapper, "@CORE_LEVEL", DbType.String, corelevel)
            DB.AddInParameter(DBCommandWrapper, "@CORE_NETWORK", DbType.String, corenetwork)
            DB.AddInParameter(DBCommandWrapper, "@TYPE_CORE", DbType.String, typecore)
            DB.AddInParameter(DBCommandWrapper, "@TYPE_COVERAGE", DbType.String, typecoveage)

            DB.AddInParameter(DBCommandWrapper, "@TYPE_FAMILY", DbType.String, typefamily)
            DB.AddInParameter(DBCommandWrapper, "@SUBSIDY_FLAG", DbType.String, subsidyflag)
            DB.AddInParameter(DBCommandWrapper, "@SUBSIDY_CNTR", DbType.Decimal, subsidycntr)
            DB.AddInParameter(DBCommandWrapper, "@COUPONS_TO_PRINT", DbType.Decimal, coupnstoprint)

            DB.AddInParameter(DBCommandWrapper, "@LOST_COVERAGE_DATE", DbType.Date, lostcoveragedate)
            DB.AddInParameter(DBCommandWrapper, "@MEDICAL_PLAN", DbType.Decimal, medicalplan)
            DB.AddInParameter(DBCommandWrapper, "@FIRST_DUE_DATE", DbType.Date, firstduedate)
            DB.AddInParameter(DBCommandWrapper, "@LAST_PAYMENT", DbType.Decimal, lastpayment)

            DB.AddInParameter(DBCommandWrapper, "@MONTHS_ELIGIBLE", DbType.Decimal, monthseligible)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            If Not transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper)
            End If



        Catch ex As Exception

            Throw

        End Try
    End Sub
    Public Shared Sub AddUFCWLETTER(ByVal familyID As Integer, ByVal relationID As Integer,
                                       ByVal qetype As String, ByVal eventDate As Date?, ByVal userRights As String, ByRef sqlcode As Integer, ByRef sqlstate As String, ByRef sqlmessage As String, ByVal transaction As DbTransaction)



        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.ADD_UFCWLETTER"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)
            DB.AddInParameter(DBCommandWrapper, "@QETYPE", DbType.String, qetype)
            DB.AddInParameter(DBCommandWrapper, "@EVENT_DATE", DbType.Date, eventDate)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)

            DB.AddOutParameter(DBCommandWrapper, "@PSQLCODE", DbType.Int16, 10)
            DB.AddOutParameter(DBCommandWrapper, "@PSQLSTATE", DbType.String, 5)
            DB.AddOutParameter(DBCommandWrapper, "@PSQLERRMC", DbType.String, 250)

            If Not transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper)
            End If

            sqlcode = CInt(DB.GetParameterValue(DBCommandWrapper, "@PSQLCODE"))
            sqlstate = CStr(DB.GetParameterValue(DBCommandWrapper, "@PSQLSTATE"))
            sqlmessage = CStr(DB.GetParameterValue(DBCommandWrapper, "@PSQLERRMC"))


        Catch ex As Exception

            Throw

        End Try
    End Sub
    Public Shared Function RetrieveShortHoursLifeEvents() As DataTable
        Dim ds As New DataSet
        Dim DB As Database
        Dim DBCommand As DbCommand
        Dim SQLCall As String = "FDBEL.RETRIEVE_SHORT_HOURS_LIFE_EVENTS"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommand = DB.GetStoredProcCommand(SQLCall)
            DB.LoadDataSet(DBCommand, ds, "SHORT_HOURS_LE")

            If ds.Tables.Count > 0 Then
                ds.Tables(0).TableName = "SHORT_HOURS_LE"
            End If

            Return ds.Tables(0)

        Catch ex As Exception
            Throw
        End Try
    End Function
#End Region

#End Region

#Region "Procedures/Functions"

    Public Shared Function CreateDataSetFromXML(ByVal XMLFile As String) As DataSet

        Dim Fs As FileStream

        'open the xml file so we can use it to fill the dataset
        Try

            'load xml into a dataset to use here
            Using DSet As New DataSet

                Fs = New FileStream(XMLFile, FileMode.Open, FileAccess.Read)

                DSet.ReadXml(Fs)

                Return DSet

            End Using

        Catch ex As Exception
            Throw
            Return Nothing
        Finally
            If Fs IsNot Nothing Then
                Fs.Close()
                Fs.Dispose()
            End If
            Fs = Nothing
        End Try

    End Function

    Public Shared Function GetUniqueKey() As String

        Dim maxSize As Integer = 8
        Dim minSize As Integer = 5
        Dim chars(62) As Char
        Dim size As Integer = maxSize
        Dim dataarray(1) As Byte

        Dim a As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"

        chars = a.ToCharArray()

        Using crypto As New Security.Cryptography.RNGCryptoServiceProvider()
            crypto.GetNonZeroBytes(dataarray)
            size = maxSize

            dataarray = New Byte(size - 1) {}

            crypto.GetNonZeroBytes(dataarray)

        End Using

        Dim Result As New Text.StringBuilder(size)
        For Each b As Byte In dataarray
            Result.Append(b)
        Next
        Return Result.ToString

    End Function

    Public Shared Function BeginTransaction() As DbTransaction
        Dim DB As Database
        Dim connection As DbConnection

        Try
            DB = CMSDALCommon.CreateDatabase()

            connection = DB.CreateConnection
            connection.Open()

            Return connection.BeginTransaction()

        Catch ex As Exception

            Throw

            Return Nothing
        End Try
    End Function

    Public Shared Sub CommitTransaction(ByRef Transaction As DbTransaction)
        Try
            If Transaction IsNot Nothing AndAlso Transaction.Connection IsNot Nothing Then
                Transaction.Commit()
            End If
            If Transaction IsNot Nothing AndAlso Transaction.Connection IsNot Nothing Then
                If Transaction.Connection.State = ConnectionState.Open Then Transaction.Connection.Close()
            End If
        Catch ex As Exception

            Throw

        Finally
            Transaction = Nothing
        End Try
    End Sub

    Public Shared Sub RollbackTransaction(ByRef Transaction As DbTransaction)
        Try
            If Transaction IsNot Nothing AndAlso Transaction.Connection IsNot Nothing Then
                Transaction.Rollback()
            End If
            If Transaction IsNot Nothing AndAlso Transaction.Connection IsNot Nothing Then
                If Transaction.Connection.State = ConnectionState.Open Then Transaction.Connection.Close()
            End If
        Catch ex As Exception

            Throw

        Finally
            Transaction = Nothing
        End Try
    End Sub

    Public Shared Function ReturnDuration(ByVal dteStartTime As Date) As String
        Dim dteEndTime As Date = UFCWGeneral.NowDate
        Dim StrDays As String = ""
        Dim StrHours As String = ""
        Dim StrMinutes As String = ""
        Dim StrSeconds As String = ""
        Dim StrMiliSeconds As String = ""
        Dim Tmp As String = ""
        Dim RunLength As System.TimeSpan = dteEndTime.Subtract(dteStartTime)

        '' Format the duration
        Select Case RunLength.Days
            Case Is < 1
                StrDays = ""
            Case Is = 1
                StrDays = RunLength.Days & " Day "
            Case Is > 1
                StrDays = RunLength.Days & " Days "
        End Select

        Select Case RunLength.Hours
            Case Is < 1
                StrHours = ""
            Case Is = 1
                StrHours = RunLength.Hours & " Hr "
            Case Is > 1
                StrHours = RunLength.Hours & " Hrs "
        End Select

        Select Case RunLength.Minutes
            Case Is < 1
                StrMinutes = ""
            Case Is = 1
                StrMinutes = RunLength.Minutes & " Min "
            Case Is > 1
                StrMinutes = RunLength.Minutes & " Mins "
        End Select

        Select Case RunLength.Seconds
            Case Is < 1
                StrSeconds = ""
            Case 1
                StrSeconds = RunLength.Seconds & " Sec "
            Case Is > 1
                StrSeconds = RunLength.Seconds & " Secs "
        End Select

        Select Case RunLength.Milliseconds
            Case Is < 1
                StrMiliSeconds = ""
            Case 1
                StrMiliSeconds = RunLength.Milliseconds & " MilliSec "
            Case Is > 1
                StrMiliSeconds = RunLength.Milliseconds & " MilliSecs "
        End Select

        '' Display the duration to the user
        Tmp = StrDays & StrHours & StrMinutes & StrSeconds & StrMiliSeconds
        If Tmp = "" Then Tmp = "0 Secs"

        Return Tmp
    End Function

#End Region

End Class

''Public Class CMSDALLog

''    Public Shared Sub Log(ByVal message As String, ByVal Destination As String)

''        Try

''            Dim LogEntry As LogEntry = New LogEntry
''            LogEntry.Message = Message
''            LogEntry.ExtendedProperties.Add("filelocation", Destination)
''            LogEntry.Categories.Add("CMSDALLog")

''            Logger.Write(LogEntry)

''        Catch ex As Exception

''            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
''            If (rethrow) Then
''                Throw
''            End If

''        End Try

''    End Sub

''End Class

Public Class XMLHandler
    'Private Shared _TraceCaching As New BooleanSwitch("TraceCaching", "Trace Switch in App.Config")
    Private _ToandFromlockObj As Object = New Object

    Public Shared Function ToandFromDataset(ByVal uniqueThreadIdentifier As String, ByVal tablename As String, ByVal columnname As String, ByVal spname As String) As DataSet


        'If _TraceCaching.Enabled Then CMSDALLog.Log(Generals.NowDate.ToString("HH:mm:ss.fff") & ControlChars.Tab & uniqueThreadIdentifier & ControlChars.Tab & "Entering: " & xmlFilename, CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & "Caching.txt")

        Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & spname & ".xml"

        Dim mySerializer As XmlSerializer
        Dim FStream As FileStream

        Dim ResultDataSet As New DataSet
        Dim DT As DataTable
        Dim columndate As DateTime
        Dim SQLCall As String

        Try

            'obtain last change and rows count of target datasource
            'If _TraceCaching.Enabled Then CMSDALLog.Log(Generals.NowDate.ToString("HH:mm:ss.fff") & ControlChars.Tab & uniqueThreadIdentifier & ControlChars.Tab & "Start Query:" & xmlFilename, CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & "Caching.txt")

            SQLCall = "SELECT MAX( " & columnname & " )" & " AS MAXDATE,COUNT(*) AS NUMBEROFROWS FROM " & tablename & " FOR READ ONLY WITH UR"

            DT = CMSDALFDBMD.RUNIMMEDIATESELECT(SQLCall)

            If DT IsNot Nothing And DT.Rows.Count > 0 Then
                columndate = CType(DT.Rows(0)("MAXDATE"), DateTime)
                If columndate.ToString = "1/1/0001 12:00:00 AM" Then
                    columndate = CDate("2006/01/01 12:00:00 AM")
                End If
                'If _TraceCaching.Enabled Then CMSDALLog.Log(Generals.NowDate.ToString("HH:mm:ss.fff") & ControlChars.Tab & uniqueThreadIdentifier & ControlChars.Tab & "End Query: " & xmlFilename & ControlChars.Tab & " Returned Date: " & columndate & ControlChars.Tab & "Row Count: " & DT.Rows(0)("NUMBEROFROWS").ToString & ControlChars.Tab & ControlChars.Tab & "SQL: " & SQLCall, CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & "Caching.txt")
            End If

            'Prepare existing xml file for modification
            If File.Exists(XMLFilename) AndAlso (File.GetAttributes(XMLFilename) And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
                File.SetAttributes(XMLFilename, FileAttributes.Normal)
            End If

            'Check if data has been modified since offline xml datastore was created, if database has newer, recreate xml file
            ' Less than zero - t1 is earlier than t2. (File does need to be recreated
            ' Zero -  t1 is the same as t2. (File does not need to be recreated
            ' Greater than zero -  t1 is later than t2. (File does not need to be recreated

            If File.Exists(XMLFilename) AndAlso (DateTime.Compare(File.GetLastWriteTime(XMLFilename), columndate) > 0) Then
                'If _TraceCaching.Enabled Then CMSDALLog.Log(Generals.NowDate.ToString("HH:mm:ss.fff") & ControlChars.Tab & uniqueThreadIdentifier & ControlChars.Tab & "XML Start: " & xmlFilename, CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & "Caching.txt")

                mySerializer = New XmlSerializer(ResultDataSet.GetType)

                For x As Integer = 1 To 10

                    Try

                        FStream = New FileStream(XMLFilename, FileMode.Open, FileAccess.Read, FileShare.None)

                        Exit For

                    Catch ex As Exception

                        'If _TraceCaching.Enabled Then CMSDALLog.Log(Generals.NowDate.ToString("HH:mm:ss.fff") & ControlChars.Tab & uniqueThreadIdentifier & ControlChars.Tab & "Wait: " & xmlFilename & ControlChars.Tab & " Wait# (" & x & ")", CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & "Caching.txt")
                        System.Threading.Thread.Sleep(1000)

                    End Try

                Next
                '' To read the file

                Try
                    '' Create the object from the xml file
                    ResultDataSet = CType(mySerializer.Deserialize(FStream), DataSet)
                    FStream.Close()
                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)

                Catch ex As Exception ' invalid xml file

                    CMSDALLog.Log(UFCWGeneral.NowDate.ToString("HH:mm:ss.fff") & ControlChars.Tab & uniqueThreadIdentifier & ControlChars.Tab & "XML Corrupt: " & XMLFilename, CMSDALLog.LogDirectory & "\" & String.Format("{0000}", UFCWGeneral.NowDate.Year) & String.Format("{00}", UFCWGeneral.NowDate.Month) & String.Format("{0:00}", UFCWGeneral.NowDate.Day) & "Exception.txt")

                    FStream.Close()

                    If File.Exists(XMLFilename) AndAlso (File.GetAttributes(XMLFilename) And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
                        File.SetAttributes(XMLFilename, FileAttributes.Normal)
                    End If

                    File.Delete(XMLFilename)

                    ResultDataSet = New DataSet
                Finally

                    FStream = Nothing
                    mySerializer = Nothing

                End Try

                'If _TraceCaching.Enabled Then CMSDALLog.Log(Generals.NowDate.ToString("HH:mm:ss.fff") & ControlChars.Tab & uniqueThreadIdentifier & ControlChars.Tab & "XML End: " & xmlFilename, CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & "Caching.txt")
            End If

            'check number of rows stored in xml matches table
            If ResultDataSet.Tables.Count = 0 OrElse ResultDataSet.Tables(0).Rows.Count <> CType(DT.Rows(0)("NUMBEROFROWS"), Integer) Then
                If File.Exists(XMLFilename) Then File.SetAttributes(XMLFilename, FileAttributes.Normal)

                If ResultDataSet.Tables.Count = 0 Then
                    'If _TraceCaching.Enabled Then CMSDALLog.Log(Generals.NowDate.ToString("HH:mm:ss.fff") & ControlChars.Tab & uniqueThreadIdentifier & ControlChars.Tab & "XML Build : " & xmlFilename, CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & "Caching.txt")
                ElseIf ResultDataSet.Tables(0).Rows.Count <> CType(DT.Rows(0)("NUMBEROFROWS"), Integer) Then
                    'If _TraceCaching.Enabled Then CMSDALLog.Log(Generals.NowDate.ToString("HH:mm:ss.fff") & ControlChars.Tab & uniqueThreadIdentifier & ControlChars.Tab & "XML ReBuild :" & xmlFilename & " " & ResultDataSet.Tables(0).Rows.Count & " <> " & CType(DT.Rows(0)("NUMBEROFROWS"), Integer), CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & "Caching.txt")
                End If

                Return New DataSet
            Else
                'If _TraceCaching.Enabled Then CMSDALLog.Log(Generals.NowDate.ToString("HH:mm:ss.fff") & ControlChars.Tab & uniqueThreadIdentifier & ControlChars.Tab & "XML Valid: " & xmlFilename, CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & "Caching.txt")
                Return ResultDataSet
            End If

        Catch ex As Exception

            'If _TraceCaching.Enabled Then CMSDALLog.Log(ex.ToString, CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & String.Format("{0:00}", Generals.NowDate.Day) & "Exception.txt")
            'If _TraceCaching.Enabled Then CMSDALLog.Log(Generals.NowDate.ToString("HH:mm:ss.fff") & ControlChars.Tab & uniqueThreadIdentifier & ControlChars.Tab & "Conflict: " & xmlFilename, CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & "Caching.txt")

            Throw
        Finally

            ResultDataSet = Nothing
            DT = Nothing

            If FStream IsNot Nothing Then
                FStream.Close()
                FStream.Dispose()
            End If
            FStream = Nothing

            mySerializer = Nothing

            'If _TraceCaching.Enabled Then CMSDALLog.Log(Generals.NowDate.ToString("HH:mm:ss.fff") & ControlChars.Tab & uniqueThreadIdentifier & ControlChars.Tab & "Exiting Class: " & xmlFilename, CMSDALLog.LogDirectory & "\" & String.Format("{0000}", Generals.NowDate.Year) & String.Format("{00}", Generals.NowDate.Month) & "Caching.txt")

        End Try

    End Function

    Public Shared Function FileInUse(ByVal sFile As String) As Boolean

        Try

            If System.IO.File.Exists(sFile) Then
                Using FStream As FileStream = New FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.None)

                End Using
            End If

        Catch
            Return True
        Finally
        End Try

    End Function

    Public Shared Function CreateDataSetFromXML(ByVal XMLFile As String) As DataSet

        'load xml into a dataset to use here
        Dim DSet As New DataSet
        Dim FStream As FileStream


        'open the xml file so we can use it to fill the dataset
        Try
            FStream = New FileStream(System.Windows.Forms.Application.StartupPath & "\" & XMLFile, FileMode.Open, FileAccess.Read)

        Catch ex As Exception
            Dim Rethrow As Boolean = True
            If (Rethrow) Then
                Throw
            Else
                Return Nothing
            End If
        End Try

        'fill the dataset
        Try
            DSet.ReadXml(FStream)

            If Not DSet.Tables("Column").Columns.Contains("Visible") Then DSet.Tables("Column").Columns.Add("Visible")
            If Not DSet.Tables("Column").Columns.Contains("FormatIsRegEx") Then DSet.Tables("Column").Columns.Add("FormatIsRegEx")
            If Not DSet.Tables("Column").Columns.Contains("WordWrap") Then DSet.Tables("Column").Columns.Add("WordWrap")
            If Not DSet.Tables("Column").Columns.Contains("ReadOnly") Then DSet.Tables("Column").Columns.Add("ReadOnly")
            If Not DSet.Tables("Column").Columns.Contains("MinimumCharWidth") Then DSet.Tables("Column").Columns.Add("MinimumCharWidth")
            If Not DSet.Tables("Column").Columns.Contains("MaximumCharWidth") Then DSet.Tables("Column").Columns.Add("MaximumCharWidth")

            '' Return dSet

        Catch ex As Exception
            Throw
        Finally
            If FStream IsNot Nothing Then
                FStream.Close()
                FStream.Dispose()
            End If

            FStream = Nothing

        End Try
        Return DSet
    End Function

End Class



