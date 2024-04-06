Option Strict On

Imports DDTek.DB2

Imports System.IO
Imports System.Configuration
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data



Public Class CMSDALFDBHRA
    Public Const _SQLdatabaseName As String = "dbo"

    Public Const _DB2SchemaName As String = "FDBHRA"

    Private Shared _ComputerName As String = UFCWGeneral.ComputerName
    Private Shared _LastSQL As String

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

    Public Shared Function RetrieveMemType(ByVal familyID As Integer, ByVal effectiveDate As Date?, Optional ByVal transaction As DbTransaction = Nothing) As DataTable
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_ELIG_FAMILY_BY_ELIG_PERIOD"
        Dim DS As DataSet

        Try
            DB = CMSDALCommon.CreateDatabase

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@ELIGIBILITY_PERIOD", DbType.Date, effectiveDate) 'Format(EffectiveDate, "yyyy-MM-dd"))
            DS = DB.ExecuteDataSet(DBCommandWrapper)

            If DS.Tables.Count > 0 Then
                DS.Tables(0).TableName = "ELIG_MTHDTL"
                Return DS.Tables(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function RetrieveHRAEventList(ByVal familyID As Integer, ByVal relationID As Short?) As DataTable

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_HRA_REVERSABLE_EVENTS"

        Dim DS As DataSet

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)

            DS = DB.ExecuteDataSet(DBCommandWrapper)
            If DS.Tables.Count > 0 Then
                DS.Tables(0).TableName = "HRA_SCHEDULE_TYPES"
                Return DS.Tables(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveHRAScheduleTypes() As DataTable
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_HRA_SCHEDULE_TYPES"

        Dim DS As DataSet

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DS = DB.ExecuteDataSet(DBCommandWrapper)
            If DS.Tables.Count > 0 Then
                DS.Tables(0).TableName = "HRA_TRIGGER_EVENT_XREF"
                Return DS.Tables(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function RetrieveHRATransactionTypes() As DataTable
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_HRA_TRANSACTION_TYPES"

        Dim DS As DataSet

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DS = DB.ExecuteDataSet(DBCommandWrapper)

            If DS.Tables.Count > 0 Then
                DS.Tables(0).TableName = "HRA_TRANSACTION_TYPES"
                Return DS.Tables(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Sub InsertHRATransaction(ByVal transactionType As Integer, ByVal familyID As Integer, ByVal relationID As Short?, ByVal transactionAmt As Decimal?,
                                            ByVal effectiveDate As Date?, ByVal expirationDate As Date?,
                                            ByVal comments As String, ByVal UserRights As String,
                                             Optional ByVal transaction As DbTransaction = Nothing)

        InsertHRATransaction(transactionType, familyID, relationID, Nothing, Nothing, Nothing, transactionAmt, effectiveDate, expirationDate, comments, UserRights, Nothing, transaction)

    End Sub

    Public Shared Sub InsertHRATransaction(ByVal transactionType As Integer, ByVal familyID As Integer, ByVal relationID As Short?, ByVal transactionAmt As Decimal?,
                                            ByVal effectiveDate As Date?, ByVal expirationDate As Date?,
                                            ByVal comments As String, ByVal UserRights As String, ByVal hraTransientID As Integer?,
                                            Optional ByVal transaction As DbTransaction = Nothing)

        InsertHRATransaction(transactionType, familyID, relationID, Nothing, Nothing, Nothing, transactionAmt, effectiveDate, expirationDate, comments, UserRights, hraTransientID, transaction)

    End Sub

    Public Shared Sub InsertHRATransaction(ByVal transactionType As Integer, ByVal familyID As Integer, ByVal relationID As Short?, ByVal transactionAmt As Decimal?,
                                            ByVal effectiveDate As Date?,
                                            ByVal comments As String, ByVal UserRights As String, ByVal hraTransientID As Integer?,
                                            Optional ByVal transaction As DbTransaction = Nothing)

        InsertHRATransaction(transactionType, familyID, relationID, Nothing, Nothing, Nothing, transactionAmt, effectiveDate, Nothing, comments, UserRights, hraTransientID, transaction)

    End Sub

    Public Shared Sub InsertHRATransaction(ByVal transactionType As Integer, ByVal familyID As Integer, ByVal relationID As Short?, ByVal claimID As Integer?,
                                            ByVal LineNbr As Short?, ByVal checkNbr As Integer?, ByVal transactionAmt As Decimal?,
                                            ByVal effectiveDate As Date?, ByVal expirationDate As Date?,
                                            ByVal comments As String, ByVal UserRights As String, ByVal hraTransientID As Integer?,
                                            Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_HRA_TRANSACTION_POST_TRANSIENT"

        Try

            If IsDBNull(expirationDate) OrElse expirationDate Is Nothing OrElse CDate(expirationDate) = CDate("12/31/9998") Then 'Necessary due to control's maxdate limitation
                expirationDate = CDate("12/31/9999")
            End If

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@CLAIMD_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@LINE_NBR", DbType.Int16, LineNbr)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_NBR", DbType.Int32, checkNbr)
            DB.AddInParameter(DBCommandWrapper, "@EFFECTIVE_DATE", DbType.Date, effectiveDate)
            DB.AddInParameter(DBCommandWrapper, "@TRANSACTION_AMT", DbType.Decimal, transactionAmt)
            DB.AddInParameter(DBCommandWrapper, "@TRANSACTION_TYPE", DbType.Int32, transactionType)
            DB.AddInParameter(DBCommandWrapper, "@COMMENTS", DbType.String, comments)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, UserRights)
            DB.AddInParameter(DBCommandWrapper, "@EXPIRATION_DATE", DbType.Date, expirationDate)
            DB.AddInParameter(DBCommandWrapper, "@HRA_TRANSIENT_ID", DbType.Int32, hraTransientID)

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

    Public Shared Sub InsertHRAAction(ByVal familyID As Integer, ByVal relationID As Short?,
                                            ByVal MAXIMID As String, ByVal STATUSCODE As String, ByVal STATUSCODEDATE As Date?,
                                            ByVal LETTERNAME As String, ByVal INDEXDATE As String, ByVal PROCESSDATE As String,
                                            ByVal RECEIVEDATE As String, ByVal PARTSSNO As Integer, ByVal PATSSNO As Integer?,
                                            ByVal PARTFULLNAME As String, ByVal PATIENTSW As Decimal?, ByVal PATLASTNAME As String,
                                            ByVal PATFIRSTNAME As String, ByVal PATMIDDLEINT As String, ByVal ADDRESS As String,
                                            ByVal CITY As String, ByVal STATE As String, ByVal ZIP As Integer?,
                                            ByVal HOMEPHONE As String, ByVal CELLPHONE As String, ByVal BIRTHDATE As Date?,
                                            ByVal EMAILADDRESS As String, ByVal SIGNATURESW As Decimal?, ByVal SIGNATUREDATE As Date?,
                                            ByVal PHYSICALDATE As Date?, ByVal BIOMETRICDATE As Date?, ByVal HEIGHTFEET As String,
                                            ByVal HEIGHTINCHES As String, ByVal WEIGHT As String, ByVal BODYMASS As String,
                                            ByVal PATIENTFASTED As Decimal?, ByVal PATIENTGLUCOSE As String, ByVal BPSYSTOLIC As String,
                                            ByVal BPDIASTOLIC As String, ByVal PULSE As String, ByVal TRIGLYCERIDES As String,
                                            ByVal HDL As String, ByVal TOTALCHOLESTEROL As String, ByVal LDL As String,
                                            ByVal TABACCOSW As Decimal?, ByVal PHYSIGSW As Decimal?, ByVal PHYSIGDATE As Date?,
                                            ByVal PHYSICIANFULLNAME As String, ByVal UPINNPI As Decimal?, ByVal DRPHONE As String,
                                            ByVal WEIGHTSW As Decimal?, ByVal WEIGHTDOCSW As Decimal?, ByVal WEIGHTPGMNAME As String,
                                            ByVal WEIGHTDATE1 As Date?, ByVal WEIGHTDATE2 As Date?, ByVal TABACCOPGMSW As Decimal?,
                                            ByVal TABACCODOCSW As Decimal?, ByVal TABACCOPGMNAME As String, ByVal TABACCODATE1 As Date?, ByVal TABACCODATE2 As Date?,
                                            ByVal GYMSW As Decimal?, ByVal GYMDOCSW As Decimal?, ByVal GYMPGMNAME As String,
                                            ByVal GYMDATE1 As Date?, ByVal GYMDATE2 As Date?, ByVal RUNSW As Decimal?,
                                            ByVal RUNDOCSW As Decimal?, ByVal RUNPGMNAME As String, ByVal RUNDATE1 As Date?,
                                            ByVal RUNDATE2 As Date?, ByVal PDCIFULLNAME As String, ByVal PDCIPHONE As String,
                                            ByVal PDCIADDRESS As String, ByVal PDCICITY As String, ByVal PDCISTATE As String,
                                            ByVal PDCIZIP As Integer?, ByVal CREATEUSERID As String, ByVal CREATETIMESTAMP As Date?,
                                            ByVal BATCHUSERID As String, ByVal BATCHTIMESTAMP As Date?, ByVal ONLINEUSERID As String,
                                            ByVal ONLINETIMESTAMP As Date?, ByVal FLUSW As Decimal?, ByVal FLUDOCSW As Decimal?,
                                            ByVal COLONSW As Decimal?, ByVal COLONDOCSW As Decimal?, ByVal MAMOSW As Decimal?,
                                            ByVal MAMODOCSW As Decimal?, ByVal PAPSW As Decimal?, ByVal PAPDOCSW As Decimal?,
                                            ByVal PSASW As Decimal?, ByVal PSADOCSW As Decimal?, ByVal WAISTMEASURE As String, ByVal PHYSICALSW As Decimal?,
                                            ByVal PHYSICALDOCSW As Decimal?, ByVal PRESENT1SW As Decimal?, ByVal PRESENT2SW As Decimal?,
                                            ByVal PRESENT3SW As Decimal?, ByVal PRESENT4SW As Decimal?, ByVal PRESENT5SW As Decimal?,
                                            ByVal PRESENT6SW As Decimal?, ByVal DOCID As Long?,
                                            Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBHRA.CREATE_HRA_ACTION"

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@MAXIM_ID", DbType.String, MAXIMID)
            DB.AddInParameter(DBCommandWrapper, "@STATUS_CODE", DbType.String, STATUSCODE)
            DB.AddInParameter(DBCommandWrapper, "@STATUS_CODE_DATE", DbType.DateTime, STATUSCODEDATE)
            DB.AddInParameter(DBCommandWrapper, "@LETTERNAME", DbType.String, LETTERNAME)
            DB.AddInParameter(DBCommandWrapper, "@INDEX_DATE", DbType.String, INDEXDATE)
            DB.AddInParameter(DBCommandWrapper, "@PROCESS_DATE", DbType.String, PROCESSDATE)
            DB.AddInParameter(DBCommandWrapper, "@RECEIVE_DATE", DbType.String, RECEIVEDATE)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSNO", DbType.Int32, PARTSSNO)
            DB.AddInParameter(DBCommandWrapper, "@PAT_SSNO", DbType.Int32, PATSSNO)
            DB.AddInParameter(DBCommandWrapper, "@PART_FULL_NAME", DbType.String, PARTFULLNAME)
            DB.AddInParameter(DBCommandWrapper, "@PATIENT_SW", DbType.Decimal, PATIENTSW)
            DB.AddInParameter(DBCommandWrapper, "@PAT_LAST_NAME", DbType.String, PATLASTNAME)
            DB.AddInParameter(DBCommandWrapper, "@PAT_FIRST_NAME", DbType.String, PATFIRSTNAME)
            DB.AddInParameter(DBCommandWrapper, "@PAT_MIDDLE_INT", DbType.String, PATMIDDLEINT)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS", DbType.String, ADDRESS)
            DB.AddInParameter(DBCommandWrapper, "@CITY", DbType.String, CITY)
            DB.AddInParameter(DBCommandWrapper, "@STATE", DbType.String, STATE)
            DB.AddInParameter(DBCommandWrapper, "@ZIP", DbType.Int32, ZIP)
            DB.AddInParameter(DBCommandWrapper, "@HOME_PHONE", DbType.String, HOMEPHONE)
            DB.AddInParameter(DBCommandWrapper, "@CELL_PHONE", DbType.String, CELLPHONE)
            DB.AddInParameter(DBCommandWrapper, "@BIRTH_DATE", DbType.Date, BIRTHDATE)
            DB.AddInParameter(DBCommandWrapper, "@EMAIL_ADDRESS", DbType.String, EMAILADDRESS)
            DB.AddInParameter(DBCommandWrapper, "@SIGNATURE_SW", DbType.Decimal, SIGNATURESW)
            DB.AddInParameter(DBCommandWrapper, "@SIGNATURE_DATE", DbType.Date, SIGNATUREDATE)
            DB.AddInParameter(DBCommandWrapper, "@PHYSICAL_DATE", DbType.Date, PHYSICALDATE)
            DB.AddInParameter(DBCommandWrapper, "@BIOMETRIC_DATE", DbType.Date, BIOMETRICDATE)
            DB.AddInParameter(DBCommandWrapper, "@HEIGHT_FEET", DbType.String, HEIGHTFEET)
            DB.AddInParameter(DBCommandWrapper, "@HEIGHT_INCHES", DbType.String, HEIGHTINCHES)
            DB.AddInParameter(DBCommandWrapper, "@WEIGHT", DbType.String, WEIGHT)
            DB.AddInParameter(DBCommandWrapper, "@BODY_MASS", DbType.String, BODYMASS)
            DB.AddInParameter(DBCommandWrapper, "@PATIENT_FASTED", DbType.Decimal, PATIENTFASTED)
            DB.AddInParameter(DBCommandWrapper, "@PATIENT_GLUCOSE", DbType.String, PATIENTGLUCOSE)
            DB.AddInParameter(DBCommandWrapper, "@BP_SYSTOLIC", DbType.String, BPSYSTOLIC)
            DB.AddInParameter(DBCommandWrapper, "@BP_DIASTOLIC", DbType.String, BPDIASTOLIC)
            DB.AddInParameter(DBCommandWrapper, "@PULSE", DbType.String, PULSE)
            DB.AddInParameter(DBCommandWrapper, "@TRIGLYCERIDES", DbType.String, TRIGLYCERIDES)
            DB.AddInParameter(DBCommandWrapper, "@HDL", DbType.String, HDL)
            DB.AddInParameter(DBCommandWrapper, "@TOTAL_CHOLESTEROL", DbType.String, TOTALCHOLESTEROL)
            DB.AddInParameter(DBCommandWrapper, "@LDL", DbType.String, LDL)
            DB.AddInParameter(DBCommandWrapper, "@TABACCO_SW", DbType.Decimal, TABACCOSW)
            DB.AddInParameter(DBCommandWrapper, "@PHY_SIG_SW", DbType.Decimal, PHYSIGSW)
            DB.AddInParameter(DBCommandWrapper, "@PHY_SIG_DATE", DbType.Date, PHYSIGDATE)
            DB.AddInParameter(DBCommandWrapper, "@PHYSICIAN_FULL_NAME", DbType.String, PHYSICIANFULLNAME)
            DB.AddInParameter(DBCommandWrapper, "@UPIN_NPI", DbType.Decimal, UPINNPI)
            DB.AddInParameter(DBCommandWrapper, "@DR_PHONE", DbType.String, DRPHONE)
            DB.AddInParameter(DBCommandWrapper, "@WEIGHT_SW", DbType.Decimal, WEIGHTSW)
            DB.AddInParameter(DBCommandWrapper, "@WEIGHT_DOC_SW", DbType.Decimal, WEIGHTDOCSW)
            DB.AddInParameter(DBCommandWrapper, "@WEIGHT_PGM_NAME", DbType.String, WEIGHTPGMNAME)
            DB.AddInParameter(DBCommandWrapper, "@WEIGHT_DATE1", DbType.Date, WEIGHTDATE1)
            DB.AddInParameter(DBCommandWrapper, "@WEIGHT_DATE2", DbType.Date, WEIGHTDATE2)
            DB.AddInParameter(DBCommandWrapper, "@TABACCO_PGM_SW", DbType.Decimal, TABACCOPGMSW)
            DB.AddInParameter(DBCommandWrapper, "@TABACCO_DOC_SW", DbType.Decimal, TABACCODOCSW)
            DB.AddInParameter(DBCommandWrapper, "@TABACCO_PGM_NAME", DbType.String, TABACCOPGMNAME)
            DB.AddInParameter(DBCommandWrapper, "@TABACCO_DATE1", DbType.Date, TABACCODATE1)
            DB.AddInParameter(DBCommandWrapper, "@TABACCO_DATE2", DbType.Date, TABACCODATE2)
            DB.AddInParameter(DBCommandWrapper, "@GYM_SW", DbType.Decimal, GYMSW)
            DB.AddInParameter(DBCommandWrapper, "@GYM_DOC_SW", DbType.Decimal, GYMDOCSW)
            DB.AddInParameter(DBCommandWrapper, "@GYM_PGM_NAME", DbType.String, GYMPGMNAME)
            DB.AddInParameter(DBCommandWrapper, "@GYM_DATE1", DbType.Date, GYMDATE1)
            DB.AddInParameter(DBCommandWrapper, "@GYM_DATE2", DbType.Date, GYMDATE2)
            DB.AddInParameter(DBCommandWrapper, "@RUN_SW", DbType.Decimal, RUNSW)
            DB.AddInParameter(DBCommandWrapper, "@RUN_DOC_SW", DbType.Decimal, RUNDOCSW)
            DB.AddInParameter(DBCommandWrapper, "@RUN_PGM_NAME", DbType.String, RUNPGMNAME)
            DB.AddInParameter(DBCommandWrapper, "@RUN_DATE1", DbType.Date, RUNDATE1)
            DB.AddInParameter(DBCommandWrapper, "@RUN_DATE2", DbType.Date, RUNDATE2)
            DB.AddInParameter(DBCommandWrapper, "@PDCI_FULL_NAME", DbType.String, PDCIFULLNAME)
            DB.AddInParameter(DBCommandWrapper, "@PDCI_PHONE", DbType.String, PDCIPHONE)
            DB.AddInParameter(DBCommandWrapper, "@PDCI_ADDRESS", DbType.String, PDCIADDRESS)
            DB.AddInParameter(DBCommandWrapper, "@PDCI_CITY", DbType.String, PDCICITY)
            DB.AddInParameter(DBCommandWrapper, "@PDCI_STATE", DbType.String, PDCISTATE)
            DB.AddInParameter(DBCommandWrapper, "@PDCI_ZIP", DbType.Int32, PDCIZIP)
            DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String, CREATEUSERID)
            DB.AddInParameter(DBCommandWrapper, "@CREATE_TIMESTAMP", DbType.DateTime, CREATETIMESTAMP)
            DB.AddInParameter(DBCommandWrapper, "@BATCH_USERID", DbType.String, BATCHUSERID)
            DB.AddInParameter(DBCommandWrapper, "@BATCH_TIMESTAMP", DbType.DateTime, BATCHTIMESTAMP)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_USERID", DbType.String, ONLINEUSERID)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_TIMESTAMP", DbType.DateTime, ONLINETIMESTAMP)
            DB.AddInParameter(DBCommandWrapper, "@FLU_SW", DbType.Decimal, FLUSW)
            DB.AddInParameter(DBCommandWrapper, "@FLU_DOC_SW", DbType.Decimal, FLUDOCSW)
            DB.AddInParameter(DBCommandWrapper, "@COLON_SW", DbType.Decimal, COLONSW)
            DB.AddInParameter(DBCommandWrapper, "@COLON_DOC_SW", DbType.Decimal, COLONDOCSW)
            DB.AddInParameter(DBCommandWrapper, "@MAMO_SW", DbType.Decimal, MAMOSW)
            DB.AddInParameter(DBCommandWrapper, "@MAMO_DOC_SW", DbType.Decimal, MAMODOCSW)
            DB.AddInParameter(DBCommandWrapper, "@PAP_SW", DbType.Decimal, PAPSW)
            DB.AddInParameter(DBCommandWrapper, "@PAP_DOC_SW", DbType.Decimal, PAPDOCSW)
            DB.AddInParameter(DBCommandWrapper, "@PSA_SW", DbType.Decimal, PSASW)
            DB.AddInParameter(DBCommandWrapper, "@PSA_DOC_SW", DbType.Decimal, PSADOCSW)
            DB.AddInParameter(DBCommandWrapper, "@WAIST_MEASURE", DbType.String, WAISTMEASURE)
            DB.AddInParameter(DBCommandWrapper, "@PHYSICAL_SW", DbType.Decimal, PHYSICALSW)
            DB.AddInParameter(DBCommandWrapper, "@PHYSICAL_DOC_SW", DbType.Decimal, PHYSICALDOCSW)
            DB.AddInParameter(DBCommandWrapper, "@PRESENT1_SW", DbType.Decimal, PRESENT1SW)
            DB.AddInParameter(DBCommandWrapper, "@PRESENT2_SW", DbType.Decimal, PRESENT2SW)
            DB.AddInParameter(DBCommandWrapper, "@PRESENT3_SW", DbType.Decimal, PRESENT3SW)
            DB.AddInParameter(DBCommandWrapper, "@PRESENT4_SW", DbType.Decimal, PRESENT4SW)
            DB.AddInParameter(DBCommandWrapper, "@PRESENT5_SW", DbType.Decimal, PRESENT5SW)
            DB.AddInParameter(DBCommandWrapper, "@PRESENT6_SW", DbType.Decimal, PRESENT6SW)
            DB.AddInParameter(DBCommandWrapper, "@DOCID", DbType.Decimal, DOCID)

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

    Public Shared Sub UpdateHRAAction(ByVal familyID As Integer, ByVal relationID As Short?,
                                            ByVal MAXIMID As String, ByVal STATUS As String, ByVal STATUSDATE As Date?,
                                            ByVal LETTERNAME As String, ByVal INDEXDATE As String, ByVal PROCESSDATE As String,
                                            ByVal RECEIVEDATE As String, ByVal PARTSSNO As Integer, ByVal PATSSNO As Integer?,
                                            ByVal PARTFULLNAME As String, ByVal PATIENTSW As Decimal?, ByVal PATLASTNAME As String,
                                            ByVal PATFIRSTNAME As String, ByVal PATMIDDLEINT As String, ByVal ADDRESS As String,
                                            ByVal CITY As String, ByVal STATE As String, ByVal ZIP As Integer?,
                                            ByVal HOMEPHONE As String, ByVal CELLPHONE As String, ByVal BIRTHDATE As Date?,
                                            ByVal EMAILADDRESS As String, ByVal SIGNATURESW As Decimal?, ByVal SIGNATUREDATE As Date?,
                                            ByVal PHYSICALDATE As Date?, ByVal BIOMETRICDATE As Date?, ByVal HEIGHTFEET As String,
                                            ByVal HEIGHTINCHES As String, ByVal WEIGHT As String, ByVal BODYMASS As String,
                                            ByVal PATIENTFASTED As Decimal?, ByVal PATIENTGLUCOSE As String, ByVal BPSYSTOLIC As String,
                                            ByVal BPDIASTOLIC As String, ByVal PULSE As String, ByVal TRIGLYCERIDES As String,
                                            ByVal HDL As String, ByVal TOTALCHOLESTEROL As String, ByVal LDL As String,
                                            ByVal TABACCOSW As Decimal?, ByVal PHYSIGSW As Decimal?, ByVal PHYSIGDATE As Date?,
                                            ByVal PHYSICIANFULLNAME As String, ByVal UPINNPI As Decimal?, ByVal DRPHONE As String,
                                            ByVal WEIGHTSW As Decimal?, ByVal WEIGHTDOCSW As Decimal?, ByVal WEIGHTPGMNAME As String,
                                            ByVal WEIGHTDATE1 As Date?, ByVal WEIGHTDATE2 As Date?, ByVal TABACCOPGMSW As Decimal?,
                                            ByVal TABACCODOCSW As Decimal?, ByVal TABACCOPGMNAME As String, ByVal TABACCODATE1 As Date?, ByVal TABACCODATE2 As Date?,
                                            ByVal GYMSW As Decimal?, ByVal GYMDOCSW As Decimal?, ByVal GYMPGMNAME As String,
                                            ByVal GYMDATE1 As Date?, ByVal GYMDATE2 As Date?, ByVal RUNSW As Decimal?,
                                            ByVal RUNDOCSW As Decimal?, ByVal RUNPGMNAME As String, ByVal RUNDATE1 As Date?,
                                            ByVal RUNDATE2 As Date?, ByVal PDCIFULLNAME As String, ByVal PDCIPHONE As String,
                                            ByVal PDCIADDRESS As String, ByVal PDCICITY As String, ByVal PDCISTATE As String,
                                            ByVal PDCIZIP As Integer?, ByVal CREATEUSERID As String, ByVal CREATETIMESTAMP As Date?,
                                            ByVal BATCHUSERID As String, ByVal BATCHTIMESTAMP As Date?, ByVal ONLINEUSERID As String,
                                            ByVal ONLINETIMESTAMP As Date?, ByVal FLUSW As Decimal?, ByVal FLUDOCSW As Decimal?,
                                            ByVal COLONSW As Decimal?, ByVal COLONDOCSW As Decimal?, ByVal MAMOSW As Decimal?,
                                            ByVal MAMODOCSW As Decimal?, ByVal PAPSW As Decimal?, ByVal PAPDOCSW As Decimal?,
                                            ByVal PSASW As Decimal?, ByVal PSADOCSW As Decimal?, ByVal WAISTMEASURE As String, ByVal PHYSICALSW As Decimal?,
                                            ByVal PHYSICALDOCSW As Decimal?, ByVal PRESENT1SW As Decimal?, ByVal PRESENT2SW As Decimal?,
                                            ByVal PRESENT3SW As Decimal?, ByVal PRESENT4SW As Decimal?, ByVal PRESENT5SW As Decimal?,
                                            ByVal PRESENT6SW As Decimal?, ByVal DOCID As Long?,
                                            Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBHRA.UPDATE_HRA_ACTION"

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@MAXIM_ID", DbType.String, MAXIMID)
            DB.AddInParameter(DBCommandWrapper, "@STATUS_CODE", DbType.String, STATUS)
            DB.AddInParameter(DBCommandWrapper, "@STATUS_CODE_DATE", DbType.DateTime, STATUSDATE)
            DB.AddInParameter(DBCommandWrapper, "@LETTERNAME", DbType.String, LETTERNAME)
            DB.AddInParameter(DBCommandWrapper, "@INDEX_DATE", DbType.String, INDEXDATE)
            DB.AddInParameter(DBCommandWrapper, "@PROCESS_DATE", DbType.String, PROCESSDATE)
            DB.AddInParameter(DBCommandWrapper, "@RECEIVE_DATE", DbType.String, RECEIVEDATE)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSNO", DbType.Int32, PARTSSNO)
            DB.AddInParameter(DBCommandWrapper, "@PAT_SSNO", DbType.Int32, PATSSNO)
            DB.AddInParameter(DBCommandWrapper, "@PART_FULL_NAME", DbType.String, PARTFULLNAME)
            DB.AddInParameter(DBCommandWrapper, "@PATIENT_SW", DbType.Decimal, PATIENTSW)
            DB.AddInParameter(DBCommandWrapper, "@PAT_LAST_NAME", DbType.String, PATLASTNAME)
            DB.AddInParameter(DBCommandWrapper, "@PAT_FIRST_NAME", DbType.String, PATFIRSTNAME)
            DB.AddInParameter(DBCommandWrapper, "@PAT_MIDDLE_INT", DbType.String, PATMIDDLEINT)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS", DbType.String, ADDRESS)
            DB.AddInParameter(DBCommandWrapper, "@CITY", DbType.String, CITY)
            DB.AddInParameter(DBCommandWrapper, "@STATE", DbType.String, STATE)
            DB.AddInParameter(DBCommandWrapper, "@ZIP", DbType.Int32, ZIP)
            DB.AddInParameter(DBCommandWrapper, "@HOME_PHONE", DbType.String, HOMEPHONE)
            DB.AddInParameter(DBCommandWrapper, "@CELL_PHONE", DbType.String, CELLPHONE)
            DB.AddInParameter(DBCommandWrapper, "@BIRTH_DATE", DbType.Date, BIRTHDATE)
            DB.AddInParameter(DBCommandWrapper, "@EMAIL_ADDRESS", DbType.String, EMAILADDRESS)
            DB.AddInParameter(DBCommandWrapper, "@SIGNATURE_SW", DbType.Decimal, SIGNATURESW)
            DB.AddInParameter(DBCommandWrapper, "@SIGNATURE_DATE", DbType.Date, SIGNATUREDATE)
            DB.AddInParameter(DBCommandWrapper, "@PHYSICAL_DATE", DbType.Date, PHYSICALDATE)
            DB.AddInParameter(DBCommandWrapper, "@BIOMETRIC_DATE", DbType.Date, BIOMETRICDATE)
            DB.AddInParameter(DBCommandWrapper, "@HEIGHT_FEET", DbType.String, HEIGHTFEET)
            DB.AddInParameter(DBCommandWrapper, "@HEIGHT_INCHES", DbType.String, HEIGHTINCHES)
            DB.AddInParameter(DBCommandWrapper, "@WEIGHT", DbType.String, WEIGHT)
            DB.AddInParameter(DBCommandWrapper, "@BODY_MASS", DbType.String, BODYMASS)
            DB.AddInParameter(DBCommandWrapper, "@PATIENT_FASTED", DbType.Decimal, PATIENTFASTED)
            DB.AddInParameter(DBCommandWrapper, "@PATIENT_GLUCOSE", DbType.String, PATIENTGLUCOSE)
            DB.AddInParameter(DBCommandWrapper, "@BP_SYSTOLIC", DbType.String, BPSYSTOLIC)
            DB.AddInParameter(DBCommandWrapper, "@BP_DIASTOLIC", DbType.String, BPDIASTOLIC)
            DB.AddInParameter(DBCommandWrapper, "@PULSE", DbType.String, PULSE)
            DB.AddInParameter(DBCommandWrapper, "@TRIGLYCERIDES", DbType.String, TRIGLYCERIDES)
            DB.AddInParameter(DBCommandWrapper, "@HDL", DbType.String, HDL)
            DB.AddInParameter(DBCommandWrapper, "@TOTAL_CHOLESTEROL", DbType.String, TOTALCHOLESTEROL)
            DB.AddInParameter(DBCommandWrapper, "@LDL", DbType.String, LDL)
            DB.AddInParameter(DBCommandWrapper, "@TABACCO_SW", DbType.Decimal, TABACCOSW)
            DB.AddInParameter(DBCommandWrapper, "@PHY_SIG_SW", DbType.Decimal, PHYSIGSW)
            DB.AddInParameter(DBCommandWrapper, "@PHY_SIG_DATE", DbType.Date, PHYSIGDATE)
            DB.AddInParameter(DBCommandWrapper, "@PHYSICIAN_FULL_NAME", DbType.String, PHYSICIANFULLNAME)
            DB.AddInParameter(DBCommandWrapper, "@UPIN_NPI", DbType.Decimal, UPINNPI)
            DB.AddInParameter(DBCommandWrapper, "@DR_PHONE", DbType.String, DRPHONE)
            DB.AddInParameter(DBCommandWrapper, "@WEIGHT_SW", DbType.Decimal, WEIGHTSW)
            DB.AddInParameter(DBCommandWrapper, "@WEIGHT_DOC_SW", DbType.Decimal, WEIGHTDOCSW)
            DB.AddInParameter(DBCommandWrapper, "@WEIGHT_PGM_NAME", DbType.String, WEIGHTPGMNAME)
            DB.AddInParameter(DBCommandWrapper, "@WEIGHT_DATE1", DbType.Date, WEIGHTDATE1)
            DB.AddInParameter(DBCommandWrapper, "@WEIGHT_DATE2", DbType.Date, WEIGHTDATE2)
            DB.AddInParameter(DBCommandWrapper, "@TABACCO_PGM_SW", DbType.Decimal, TABACCOPGMSW)
            DB.AddInParameter(DBCommandWrapper, "@TABACCO_DOC_SW", DbType.Decimal, TABACCODOCSW)
            DB.AddInParameter(DBCommandWrapper, "@TABACCO_PGM_NAME", DbType.String, TABACCOPGMNAME)
            DB.AddInParameter(DBCommandWrapper, "@TABACCO_DATE1", DbType.Date, TABACCODATE1)
            DB.AddInParameter(DBCommandWrapper, "@TABACCO_DATE2", DbType.Date, TABACCODATE2)
            DB.AddInParameter(DBCommandWrapper, "@GYM_SW", DbType.Decimal, GYMSW)
            DB.AddInParameter(DBCommandWrapper, "@GYM_DOC_SW", DbType.Decimal, GYMDOCSW)
            DB.AddInParameter(DBCommandWrapper, "@GYM_PGM_NAME", DbType.String, GYMPGMNAME)
            DB.AddInParameter(DBCommandWrapper, "@GYM_DATE1", DbType.Date, GYMDATE1)
            DB.AddInParameter(DBCommandWrapper, "@GYM_DATE2", DbType.Date, GYMDATE2)
            DB.AddInParameter(DBCommandWrapper, "@RUN_SW", DbType.Decimal, RUNSW)
            DB.AddInParameter(DBCommandWrapper, "@RUN_DOC_SW", DbType.Decimal, RUNDOCSW)
            DB.AddInParameter(DBCommandWrapper, "@RUN_PGM_NAME", DbType.String, RUNPGMNAME)
            DB.AddInParameter(DBCommandWrapper, "@RUN_DATE1", DbType.Date, RUNDATE1)
            DB.AddInParameter(DBCommandWrapper, "@RUN_DATE2", DbType.Date, RUNDATE2)
            DB.AddInParameter(DBCommandWrapper, "@PDCI_FULL_NAME", DbType.String, PDCIFULLNAME)
            DB.AddInParameter(DBCommandWrapper, "@PDCI_PHONE", DbType.String, PDCIPHONE)
            DB.AddInParameter(DBCommandWrapper, "@PDCI_ADDRESS", DbType.String, PDCIADDRESS)
            DB.AddInParameter(DBCommandWrapper, "@PDCI_CITY", DbType.String, PDCICITY)
            DB.AddInParameter(DBCommandWrapper, "@PDCI_STATE", DbType.String, PDCISTATE)
            DB.AddInParameter(DBCommandWrapper, "@PDCI_ZIP", DbType.Int32, PDCIZIP)
            DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String, CREATEUSERID)
            DB.AddInParameter(DBCommandWrapper, "@CREATE_TIMESTAMP", DbType.DateTime, CREATETIMESTAMP)
            DB.AddInParameter(DBCommandWrapper, "@BATCH_USERID", DbType.String, BATCHUSERID)
            DB.AddInParameter(DBCommandWrapper, "@BATCH_TIMESTAMP", DbType.DateTime, BATCHTIMESTAMP)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_USERID", DbType.String, ONLINEUSERID)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_TIMESTAMP", DbType.DateTime, ONLINETIMESTAMP)
            DB.AddInParameter(DBCommandWrapper, "@FLU_SW", DbType.Decimal, FLUSW)
            DB.AddInParameter(DBCommandWrapper, "@FLU_DOC_SW", DbType.Decimal, FLUDOCSW)
            DB.AddInParameter(DBCommandWrapper, "@COLON_SW", DbType.Decimal, COLONSW)
            DB.AddInParameter(DBCommandWrapper, "@COLON_DOC_SW", DbType.Decimal, COLONDOCSW)
            DB.AddInParameter(DBCommandWrapper, "@MAMO_SW", DbType.Decimal, MAMOSW)
            DB.AddInParameter(DBCommandWrapper, "@MAMO_DOC_SW", DbType.Decimal, MAMODOCSW)
            DB.AddInParameter(DBCommandWrapper, "@PAP_SW", DbType.Decimal, PAPSW)
            DB.AddInParameter(DBCommandWrapper, "@PAP_DOC_SW", DbType.Decimal, PAPDOCSW)
            DB.AddInParameter(DBCommandWrapper, "@PSA_SW", DbType.Decimal, PSASW)
            DB.AddInParameter(DBCommandWrapper, "@PSA_DOC_SW", DbType.Decimal, PSADOCSW)
            DB.AddInParameter(DBCommandWrapper, "@WAIST_MEASURE", DbType.String, WAISTMEASURE)
            DB.AddInParameter(DBCommandWrapper, "@PHYSICAL_SW", DbType.Decimal, PHYSICALSW)
            DB.AddInParameter(DBCommandWrapper, "@PHYSICAL_DOC_SW", DbType.Decimal, PHYSICALDOCSW)
            DB.AddInParameter(DBCommandWrapper, "@PRESENT1_SW", DbType.Decimal, PRESENT1SW)
            DB.AddInParameter(DBCommandWrapper, "@PRESENT2_SW", DbType.Decimal, PRESENT2SW)
            DB.AddInParameter(DBCommandWrapper, "@PRESENT3_SW", DbType.Decimal, PRESENT3SW)
            DB.AddInParameter(DBCommandWrapper, "@PRESENT4_SW", DbType.Decimal, PRESENT4SW)
            DB.AddInParameter(DBCommandWrapper, "@PRESENT5_SW", DbType.Decimal, PRESENT5SW)
            DB.AddInParameter(DBCommandWrapper, "@PRESENT6_SW", DbType.Decimal, PRESENT6SW)
            DB.AddInParameter(DBCommandWrapper, "@DOCID", DbType.Decimal, DOCID)

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

    Public Shared Sub InsertDMCandidate(ByVal familyID As Integer, ByVal relationID As Short,
                                            ByVal firstName As String, ByVal lastName As String,
                                            ByVal birthDate As Date,
                                            ByVal accuityLevel As String, ByVal condition As String,
                                            ByVal fileDate As Date,
                                            Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBWRK.INSERT_DM_CANDIDATE"

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FIRST_NAME", DbType.String, firstName)
            DB.AddInParameter(DBCommandWrapper, "@LAST_NAME", DbType.String, lastName)
            DB.AddInParameter(DBCommandWrapper, "@BIRTH_DATE", DbType.Date, birthDate)
            DB.AddInParameter(DBCommandWrapper, "@ACCUITY_LEVEL", DbType.String, accuityLevel)
            DB.AddInParameter(DBCommandWrapper, "@CONDITION", DbType.String, condition)
            DB.AddInParameter(DBCommandWrapper, "@FILE_DATE", DbType.Date, fileDate)

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

    Public Shared Sub InsertDMIncentive(
                                            ByVal runDate As Date,
                                            ByVal carrierID As String, ByVal familyRelID As String,
                                            ByVal effYear As Short,
                                            ByVal firstName As String, ByVal lastName As String,
                                            ByVal birthDate As Date,
                                            ByVal address1 As String, ByVal city As String, ByVal state As String,
                                            ByVal zip As Integer?,
                                            ByVal homePhone As Decimal?,
                                            ByVal workPhone As Decimal?,
                                            ByVal gender As String, ByVal emailAdd As String, ByVal primaryCond As String, ByVal activityID As String,
                                            ByVal asthmaOpenDate As Date?,
                                            ByVal asthmaActDate As Date?,
                                            ByVal asthmaLastActDate As Date?,
                                            ByVal asthmaMthInProg As Decimal?,
                                            ByVal asthmaProgName As String, ByVal asthmaEngLevel As String, ByVal asthmaRskLevel As String,
                                            ByVal asthmaCloseDate As Date?,
                                            ByVal asthmaCloseRsn As String,
                                            ByVal cadOpenDate As Date?,
                                            ByVal cadActDate As Date?,
                                            ByVal cadLastActDate As Date?,
                                            ByVal cadMthInProg As Decimal?,
                                            ByVal cadProgName As String, ByVal cadEngLevel As String, ByVal cadRskLevel As String,
                                            ByVal cadCloseDate As Date?,
                                            ByVal cadCloseRsn As String,
                                            ByVal diabetOpenDate As Date?,
                                            ByVal diabetActDate As Date?,
                                            ByVal diabetLastActDate As Date?,
                                            ByVal diabetMthInProg As Decimal?,
                                            ByVal diabetProgName As String, ByVal diabetEngLevel As String, ByVal diabetRskLevel As String,
                                            ByVal diabetCloseDate As Date?,
                                            ByVal diabetCloseRsn As String,
                                            ByVal familyID As Integer, ByVal relationID As Short,
                                            Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBWRK.INSERT_DM_INCENTIVE"

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@RUN_DATE", DbType.Date, runDate)
            DB.AddInParameter(DBCommandWrapper, "@CARRIER_ID", DbType.String, carrierID)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID_CHAR", DbType.String, familyRelID)
            DB.AddInParameter(DBCommandWrapper, "@EFF_YEAR", DbType.Int16, effYear)
            DB.AddInParameter(DBCommandWrapper, "@FIRST_NAME", DbType.String, firstName)
            DB.AddInParameter(DBCommandWrapper, "@LAST_NAME", DbType.String, lastName)
            DB.AddInParameter(DBCommandWrapper, "@BIRTH_DATE", DbType.Date, birthDate)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS1", DbType.String, address1)
            DB.AddInParameter(DBCommandWrapper, "@CITY", DbType.String, city)
            DB.AddInParameter(DBCommandWrapper, "@STATE", DbType.String, state)
            DB.AddInParameter(DBCommandWrapper, "@ZIP1", DbType.Int32, zip)
            DB.AddInParameter(DBCommandWrapper, "@HOME_PHONE", DbType.Decimal, homePhone)
            DB.AddInParameter(DBCommandWrapper, "@WORK_PHONE", DbType.Decimal, workPhone)
            DB.AddInParameter(DBCommandWrapper, "@GENDER", DbType.String, gender)
            DB.AddInParameter(DBCommandWrapper, "@EMAIL_ADD", DbType.String, emailAdd)
            DB.AddInParameter(DBCommandWrapper, "@PRIMARY_COND", DbType.String, primaryCond)
            DB.AddInParameter(DBCommandWrapper, "@ACTIVITY_ID", DbType.String, activityID)

            DB.AddInParameter(DBCommandWrapper, "@ASTHMA_OPEN_DATE", DbType.Date, asthmaOpenDate)
            DB.AddInParameter(DBCommandWrapper, "@ASTHMA_ACT_DATE", DbType.Date, asthmaActDate)
            DB.AddInParameter(DBCommandWrapper, "@ASTHMA_LAST_ACT_DATE", DbType.Date, asthmaLastActDate)
            DB.AddInParameter(DBCommandWrapper, "@ASTHMA_MTH_IN_PROG", DbType.Decimal, asthmaMthInProg)
            DB.AddInParameter(DBCommandWrapper, "@ASTHMA_PROG_NAME", DbType.String, asthmaProgName)
            DB.AddInParameter(DBCommandWrapper, "@ASTHMA_ENG_LEVEL", DbType.String, asthmaEngLevel)
            DB.AddInParameter(DBCommandWrapper, "@ASTHMA_RSK_LEVEL", DbType.String, asthmaRskLevel)
            DB.AddInParameter(DBCommandWrapper, "@ASTHMA_CLOSE_DATE", DbType.Date, asthmaCloseDate)
            DB.AddInParameter(DBCommandWrapper, "@ASTHMA_CLOSE_RSN", DbType.String, asthmaCloseRsn)

            DB.AddInParameter(DBCommandWrapper, "@CAD_OPEN_DATE", DbType.Date, cadOpenDate)
            DB.AddInParameter(DBCommandWrapper, "@CAD_ACT_DATE", DbType.Date, cadActDate)
            DB.AddInParameter(DBCommandWrapper, "@CAD_LAST_ACT_DATE", DbType.Date, cadLastActDate)
            DB.AddInParameter(DBCommandWrapper, "@CAD_MTH_IN_PROG", DbType.Decimal, cadMthInProg)
            DB.AddInParameter(DBCommandWrapper, "@CAD_PROG_NAME", DbType.String, cadProgName)
            DB.AddInParameter(DBCommandWrapper, "@CAD_ENG_LEVEL", DbType.String, cadEngLevel)
            DB.AddInParameter(DBCommandWrapper, "@CAD_RSK_LEVEL", DbType.String, cadRskLevel)
            DB.AddInParameter(DBCommandWrapper, "@CAD_CLOSE_DATE", DbType.Date, cadCloseDate)
            DB.AddInParameter(DBCommandWrapper, "@CAD_CLOSE_RSN", DbType.String, cadCloseRsn)

            DB.AddInParameter(DBCommandWrapper, "@DIABET_OPEN_DATE", DbType.Date, diabetOpenDate)
            DB.AddInParameter(DBCommandWrapper, "@DIABET_ACT_DATE", DbType.Date, diabetActDate)
            DB.AddInParameter(DBCommandWrapper, "@DIABET_LAST_ACT_DATE", DbType.Date, diabetLastActDate)
            DB.AddInParameter(DBCommandWrapper, "@DIABET_MTH_IN_PROG", DbType.Decimal, diabetMthInProg)
            DB.AddInParameter(DBCommandWrapper, "@DIABET_PROG_NAME", DbType.String, diabetProgName)
            DB.AddInParameter(DBCommandWrapper, "@DIABET_ENG_LEVEL", DbType.String, diabetEngLevel)
            DB.AddInParameter(DBCommandWrapper, "@DIABET_RSK_LEVEL", DbType.String, diabetRskLevel)
            DB.AddInParameter(DBCommandWrapper, "@DIABET_CLOSE_DATE", DbType.Date, diabetCloseDate)
            DB.AddInParameter(DBCommandWrapper, "@DIABET_CLOSE_RSN", DbType.String, diabetCloseRsn)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)

            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DbException

            Throw New ApplicationException(ex.Message, ex)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub InsertDMDisincentive(
                                            ByVal familyRelID As String,
                                            ByVal firstName As String, ByVal lastName As String,
                                            ByVal birthDate As Date,
                                            ByVal caseID As Integer?,
                                            ByVal address1 As String, ByVal city As String, ByVal state As String,
                                            ByVal zip As Integer?,
                                            ByVal homePhone As Decimal?,
                                            ByVal gender As String,
                                            ByVal programName As String,
                                            ByVal engagementLevel As String,
                                            ByVal caseClosedDate As Date?,
                                            ByVal closedReason As String,
                                            ByVal reEngagementDate As Date?,
                                            ByVal familyID As Integer, ByVal relationID As Short,
                                            Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBWRK.INSERT_DM_DISINCENTIVE"

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID_CHAR", DbType.String, familyRelID)
            DB.AddInParameter(DBCommandWrapper, "@FIRST_NAME", DbType.String, firstName)
            DB.AddInParameter(DBCommandWrapper, "@LAST_NAME", DbType.String, lastName)
            DB.AddInParameter(DBCommandWrapper, "@BIRTH_DATE", DbType.Date, birthDate)
            DB.AddInParameter(DBCommandWrapper, "@CASE_ID", DbType.Int32, caseID)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS1", DbType.String, address1)
            DB.AddInParameter(DBCommandWrapper, "@CITY", DbType.String, city)
            DB.AddInParameter(DBCommandWrapper, "@STATE", DbType.String, state)
            DB.AddInParameter(DBCommandWrapper, "@ZIP1", DbType.Int32, zip)
            DB.AddInParameter(DBCommandWrapper, "@HOME_PHONE", DbType.Decimal, homePhone)
            DB.AddInParameter(DBCommandWrapper, "@GENDER", DbType.String, gender)
            DB.AddInParameter(DBCommandWrapper, "@PROGRAM_NAME", DbType.String, programName)
            DB.AddInParameter(DBCommandWrapper, "@ENGAGEMENT_LEVEL", DbType.String, engagementLevel)
            DB.AddInParameter(DBCommandWrapper, "@CASE_CLOSED_DATE", DbType.Date, caseClosedDate)
            DB.AddInParameter(DBCommandWrapper, "@CLOSED_REASON", DbType.String, closedReason)
            DB.AddInParameter(DBCommandWrapper, "@REENGAGE_DATE", DbType.Date, reEngagementDate)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)

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
    Public Shared Sub InsertHRARxReIssueCheck(ByVal familyID As Integer, ByVal relationID As Short?, ByVal claimID As Integer?, ByVal checkNbr As Integer?, ByVal effectiveDate As Date?, ByVal transactionType As Integer, ByVal comments As String, ByVal UserRights As String, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_HRA_RX_REISSUE_TRANSACTION"

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@CLAIMD_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_NBR", DbType.Int32, checkNbr)
            DB.AddInParameter(DBCommandWrapper, "@EFFECTIVE_DATE", DbType.Date, effectiveDate)
            DB.AddInParameter(DBCommandWrapper, "@TRANSACTION_TYPE", DbType.Int32, transactionType)
            DB.AddInParameter(DBCommandWrapper, "@COMMENTS", DbType.String, comments)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, UserRights)

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
    Public Shared Function InsertHRATransientTransaction(ByVal scheduleEvent As String, ByVal familyID As Integer, ByVal relationID As Short?, ByVal memType As String, ByVal effectiveDate As Date, ByVal eventDate As Date, ByVal comments As String, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing) As Integer

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_HRA_TRANSIENT_DATA_RETURN_IDENT"

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@EFFECTIVE_DATE", DbType.Date, effectiveDate)
            DB.AddInParameter(DBCommandWrapper, "@EVENT_DATE", DbType.Date, eventDate)
            DB.AddInParameter(DBCommandWrapper, "@TRANS_CODE", DbType.String, scheduleEvent)
            DB.AddInParameter(DBCommandWrapper, "@MEMTYPE", DbType.String, memType)
            DB.AddInParameter(DBCommandWrapper, "@COMMENTS", DbType.String, comments)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddOutParameter(DBCommandWrapper, "@TRANS_ID", DbType.Int32, 4)

            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

            Return CType(DB.GetParameterValue(DBCommandWrapper, "@TRANS_ID"), Int32)

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function InsertHRATransientTransaction(ByVal familyID As Integer, ByVal relationID As Short, ByVal receiveDate As Date, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing) As Integer

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBHRA.CREATE_HRA_TRANSIENT_REGISTRATION_DATA"

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@RECEIVE_DATE", DbType.Date, receiveDate)
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
    End Function

    Public Shared Function CancelHRATransaction(ByVal transactionType As Integer, ByVal familyID As Integer, ByVal relationID As Short?, ByVal effectiveDate As Date, ByVal comments As String, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing) As Integer?

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CANCEL_HRA_TRANSACTION"
        Dim UpdatedRows As Decimal?

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@EFFECTIVE_DATE", DbType.Date, effectiveDate)
            DB.AddInParameter(DBCommandWrapper, "@TRANSACTION_TYPE", DbType.Int32, transactionType)
            DB.AddInParameter(DBCommandWrapper, "@COMMENTS", DbType.String, comments)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddOutParameter(DBCommandWrapper, "@ROWCOUNT", DbType.Decimal, 31)

            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

            UpdatedRows = CType(DB.GetParameterValue(DBCommandWrapper, "@ROWCOUNT"), Decimal?)

            Return CType(UpdatedRows, Integer?)

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function CancelHRATransient(ByVal transCode As String, ByVal familyID As Integer, ByVal relationID As Short?, ByVal effectiveDate As Date, ByVal comments As String, ByVal userRights As String, Optional ByVal transaction As DbTransaction = Nothing) As Integer?

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CANCEL_HRA_TRANSIENT"
        Dim UpdatedRows As Decimal?

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@EFFECTIVE_DATE", DbType.Date, effectiveDate)
            DB.AddInParameter(DBCommandWrapper, "@TRANS_CODE", DbType.String, transCode)
            DB.AddInParameter(DBCommandWrapper, "@COMMENTS", DbType.String, comments)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, userRights)
            DB.AddOutParameter(DBCommandWrapper, "@ROWCOUNT", DbType.Decimal, 31)

            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

            UpdatedRows = CType(DB.GetParameterValue(DBCommandWrapper, "@ROWCOUNT"), Decimal?)

            Return CType(UpdatedRows, Integer?)

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function CreateDataSetFromXML(ByVal xmlFile As String) As DataSet

        'load xml into a dataset to use here
        Dim DS As New DataSet
        Dim FS As FileStream

        'open the xml file so we can use it to fill the dataset
        Try
            FS = New FileStream(System.Windows.Forms.Application.StartupPath & "\" & xmlFile, FileMode.Open, FileAccess.Read)

            DS.ReadXml(FS)

            If Not DS.Tables("Column").Columns.Contains("Visible") Then DS.Tables("Column").Columns.Add("Visible")
            If Not DS.Tables("Column").Columns.Contains("FormatIsRegEx") Then DS.Tables("Column").Columns.Add("FormatIsRegEx")
            If Not DS.Tables("Column").Columns.Contains("WordWrap") Then DS.Tables("Column").Columns.Add("WordWrap")
            If Not DS.Tables("Column").Columns.Contains("ReadOnly") Then DS.Tables("Column").Columns.Add("ReadOnly")
            If Not DS.Tables("Column").Columns.Contains("MinimumCharWidth") Then DS.Tables("Column").Columns.Add("MinimumCharWidth")
            If Not DS.Tables("Column").Columns.Contains("MaximumCharWidth") Then DS.Tables("Column").Columns.Add("MaximumCharWidth")

            Return DS

        Catch ex As Exception
            Throw
        Finally

            If FS IsNot Nothing Then
                FS.Close()
                FS.Dispose()
            End If
            FS = Nothing

            If DS IsNot Nothing Then DS.Dispose()

        End Try

    End Function
    Public Shared Function GetHRAAvailableFundsByEffectiveDate(ByVal familyID As Integer, ByVal relationID As Short?, ByVal effectiveDate As Date?, Optional ByVal transaction As DbTransaction = Nothing) As DataTable

        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RETRIEVE_HRA_AVAILABLE_FUNDS_FAMILYID_BY_EFFECTIVE_DATE")

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@EFFECTIVE_DATE", DbType.Date, effectiveDate)
            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
            End If

            Return DS.Tables(0)

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetHRAAvailableFundsByDateRange(ByVal familyID As Integer, ByVal relationID As Short?, ByVal effectiveDate As Date?, ByVal expirationDate As Date?, Optional ByVal transaction As DbTransaction = Nothing) As DataTable

        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RETRIEVE_HRA_AVAILABLE_FUNDS_FAMILYID_BY_DATE_RANGE")

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@EFFECTIVE_DATE", DbType.Date, effectiveDate)
            DB.AddInParameter(DBCommandWrapper, "@EXPIRATION_DATE", DbType.Date, expirationDate)
            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
            End If

            Return DS.Tables(0)

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function RetrieveHRAClaimHistory(ByVal familyID As Integer, ByVal claimID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RETRIEVE_HRA_HISTORY_BY_FAMILYID_CLAIMID")

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
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
                    DB.LoadDataSet(DBCommandWrapper, ds, "HRA")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "HRA", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function RetrieveHRATransientEventHistory(ByVal familyID As Integer, ByVal relationID As Short?, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RETRIEVE_HRA_TRANSIENT_EVENTHISTORY_BY_FAMILYID_RELATIONID")

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "HRA")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "HRA", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveHRATransactionEventHistory(ByVal familyID As Integer, ByVal relationID As Short?, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RETRIEVE_HRA_TRANSACTION_EVENTHISTORY_BY_FAMILYID_RELATIONID")

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "HRA")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "HRA", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function RetrieveHRACheckHistory(ByVal familyID As Integer, ByVal checkNumber As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RETRIEVE_HRA_HISTORY_BY_FAMILYID_CHECKNBR")

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@CHECKNUMBER", DbType.Int32, checkNumber)
            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "HRA")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "HRA", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GetHRAControlInformation(ByVal familyID As Integer, ByVal relationID As Short?, Optional ByVal DS As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBHRA.RETRIEVE_HRA_BY_FAMILYID_RELATIONID")

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DBCommandWrapper.CommandTimeout = 180

            If DS Is Nothing Then
                If transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "HRA")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "HRA", transaction)
                End If
            End If
            Return DS
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GetHRABalanceByClaimID(ByVal claimID As Integer, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RETRIEVE_HRA_BALANCE_CLAIMID")

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DBCommandWrapper.CommandTimeout = 180

            If DS Is Nothing Then
                If transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "HRABALANCE")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "HRABALANCE", transaction)
                End If
            End If
            Return DS
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetHRABalanceByFamilyIDRelationID(ByVal familyID As Integer, ByVal relationID As Short?, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RETRIEVE_HRA_BALANCE_FAMILYID")

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "HRABALANCE")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "HRABALANCE", transaction)
                End If
            End If

            If ds.Tables(0).TableName = "Table" Then ds.Tables(0).TableName = "HRABALANCE"

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetHRQInformation(ByVal familyID As Integer, ByVal relationID As Short?, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RETRIEVE_HRQ")

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "HRQ")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "HRQ", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveHRAAllEventHistory(ByVal familyID As Integer, ByVal relationID As Short?, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RETRIEVE_HRA_ALL_EVENTHISTORY_BY_FAMILYID_RELATIONID")

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, UFCWGeneral.ToNullShortHandler(relationID))
            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "HRA")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "HRA", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveHRAAllHistory(ByVal familyID As Integer, ByVal relationID As Short?, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(If(UFCWGeneralAD.CMSLocals, "FDBMD.RETRIEVE_HRA_BY_FAMILYID_RELATIONID", "FDBMD.RETRIEVE_HRA_ALL_BY_FAMILYID_RELATIONID"))

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, UFCWGeneral.ToNullShortHandler(relationID))
            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "HRA")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "HRA", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveCVSTransientDetails(Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim ds As DataSet
        Dim SQLCall As String = "FDBHRA.RETRIEVE_CVS_TRANSIENT_DETAILS"
        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                ds = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Sub InsertTransientTables(ByVal familyId As Integer, ByVal relationId As Integer, ByVal chFirstName As String, ByVal chLastName As String,
                                           ByVal productId As String, ByVal totAmtBilled As Decimal?, ByVal dispenseDate As Date?, ByVal pharmacyName As String, ByVal pharmacyNbr As String,
                                            Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBHRA.INSERT_CVS_TRANSIENT_TABLES"

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyId)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationId)
            DB.AddInParameter(DBCommandWrapper, "@CH_FIRST_NAME", DbType.String, chFirstName)
            DB.AddInParameter(DBCommandWrapper, "@CH_LAST_NAME", DbType.String, chLastName)
            DB.AddInParameter(DBCommandWrapper, "@PRODUCT_ID", DbType.String, productId)
            DB.AddInParameter(DBCommandWrapper, "@TOT_AMT_BILLED", DbType.Decimal, totAmtBilled)
            DB.AddInParameter(DBCommandWrapper, "@DISPENSE_DATE", DbType.Date, dispenseDate)
            DB.AddInParameter(DBCommandWrapper, "@PHARMACY_NAME", DbType.String, pharmacyName)
            DB.AddInParameter(DBCommandWrapper, "@PHARMACY_NBR", DbType.String, pharmacyNbr)

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

    Public Shared Sub InsertPrescriptionTransaction(
                                              ByVal procBatchNbr As Integer,
                                              ByVal prescriptionNbr As String,
                                              ByVal prescriptionSeq As Decimal?,
                                              ByVal referenceNbrQual As String,
                                              ByVal dispenseDate As Date?,
                                              ByVal paymentType As String,
                                              ByVal patientPayAmt As Decimal?,
                                              ByVal otherPayerAmt As Decimal?,
                                              ByVal payAmtToDeduct As Decimal?,
                                              ByVal patFirstName As String,
                                              ByVal patLastName As String,
                                              ByVal patBirthDate As Date?,
                                              ByVal patGender As String,
                                              ByVal patId As Integer,
                                              ByVal personCode As String,
                                              ByVal relationCode As String,
                                              ByVal cardholderId As String,
                                              ByVal chLastName As String,
                                              ByVal chFirstName As String,
                                              ByVal customerLocation As String,
                                              ByVal formularyStatus As String,
                                              ByVal planDrugStatus As String,
                                              ByVal groupId As String,
                                              ByVal ssno As Integer?,
                                              ByVal rxSubmitDate As Date?,
                                              ByVal rxClaimNbr As Decimal?,
                                              ByVal rxClaimNbrSeq As Decimal?,
                                              ByVal claimStatus As String,
                                              ByVal psTransCode As String,
                                              ByVal familyId As Integer,
                                              ByVal relationId As Short?,
                                              ByVal processId As Decimal?,
                                              ByVal pharmacyNbr As String,
                                              ByVal pharmacyQual As String,
                                              ByVal clmOrigFlag As String,
                                              ByVal pharmNetwork As String,
                                              ByVal pharmAffiliate As String,
                                              ByVal pharmDispClass As String,
                                              ByVal pharmDispType As String,
                                              ByVal productId As String,
                                              ByVal productIdQual As String,
                                              ByVal labelName As String,
                                              ByVal dosageForm As String,
                                              ByVal strengthUnits As String,
                                              ByVal prodManufacturer As String,
                                              ByVal metDecQuantity As Decimal?,
                                              ByVal daysSupply As Decimal?,
                                              ByVal drugRdOtcInd As String,
                                              ByVal drugStrength As Decimal?,
                                              ByVal unitOfMeasure As String,
                                              ByVal routeOfAdmin As String,
                                              ByVal dosageFormMediSpan As String,
                                              ByVal basisOfReimburs As String,
                                              ByVal costCodePricing As String,
                                              ByVal awpUnitCost As Decimal?,
                                              ByVal ingredCostBilled As Decimal?,
                                              ByVal dispenseFeeBilled As Decimal?,
                                              ByVal salesTax As Decimal?,
                                              ByVal proServiceFee As Decimal?,
                                              ByVal incentAmtBilled As Decimal?,
                                              ByVal claimAdjust As Decimal?,
                                              ByVal totAmtBilled As Decimal?,
                                              ByVal othAmtBilled As Decimal?,
                                              ByVal usualCustBillAmt As Decimal?,
                                              ByVal prescriberId As String,
                                              ByVal prescriberIdQual As String,
                                              ByVal prescriberLastName As String,
                                              ByVal prescriberFirstName As String,
                                              ByVal prescriberSpecCode As String,
                                              ByVal prescriberNetworkId As String,
                                              ByVal diagCode As String,
                                              ByVal diagCodeQual As String,
                                              ByVal priorAuthQual As String,
                                              ByVal priorAuthNbr As String,
                                              ByVal priorReasonCode As String,
                                              ByVal prescWrittenDate As Date?,
                                              ByVal prodSelCode As String,
                                              ByVal othCoverageCode As String,
                                              ByVal thirdPartyRetrictCode As String,
                                              ByVal compoundCode As String,
                                              ByVal nbrOfRefillsAuth As Decimal?,
                                              ByVal multiSourceCode As String,
                                              ByVal brandNameCode As String,
                                              ByVal deaClassCode As String,
                                              ByVal primaryProvId As String,
                                              ByVal carefacilityId As String,
                                              ByVal careQualifierCode As String,
                                              ByVal carenetworkId As String,
                                              ByVal unitDoseId As String,
                                              ByVal maintDrugCode As String,
                                              ByVal ahfsCode As String,
                                              ByVal genericProdIndex As String,
                                              ByVal genericName As String,
                                              ByVal genericCodeNbr As Integer,
                                              ByVal genericCodeSeqNbr As Integer,
                                              ByVal carrier As String,
                                              ByVal account As String,
                                              ByVal brandGenericId As String,
                                              ByVal claimMessage As String,
                                              ByVal benMaxFlag As String,
                                              ByVal clientFlag As String,
                                              ByVal periodicBenMaxApplied As Decimal?,
                                              ByVal periodEndDate As Date?,
                                              ByVal theraGroupId As String,
                                              ByVal formularyClaimFlag As String,
                                              ByVal medicalClaimFlag As String,
                                              ByVal priorAuthClmFlag As String,
                                              ByVal transPlantFlag As String,
                                              ByVal injectProdFlag As String,
                                              ByVal rxsolFormuFlag As String,
                                              ByVal checkDate As Date?,
                                              ByVal checkNbr As String,
                                              ByVal hcpcsCode As String,
                                              ByVal clientCode As String,
                                              ByVal clientRiderCode As String,
                                              ByVal clientBenCode As String,
                                              ByVal planQualCode As String,
                                              ByVal adminFee As Integer,
                                              ByVal clientDefined As String,
                                              ByVal reserved As String,
                                              ByVal pharmacyName As String,
                                              ByVal basisOfCostDeterm As String,
                                              ByVal ingredCostSubmit As Decimal?,
                                              ByVal ingredCostCalc As Decimal?,
                                              ByVal outOfNetPenalty As Decimal?,
                                              ByVal clientCopay As Decimal?,
                                              ByVal netPatientPayAmt As Decimal?,
                                              ByVal planCopay As Decimal?,
                                              ByVal prodSelcDiff As Decimal?,
                                              ByVal patMidInt As String,
                                              ByVal partZip As String,
                                              ByVal prescriberDeaNbr As String,
                                              ByVal chMidInt As String,
                                              ByVal chDob As Date?,
                                              ByVal chAdd1 As String,
                                              ByVal chAdd2 As String,
                                              ByVal chCity As String,
                                              ByVal chState As String,
                                              ByVal chZip As String,
                                              ByVal rxOrgin As String,
                                              ByVal patResidence As String,
                                              ByVal specialtyFlag As String,
                                              ByVal planCode As String,
                                              ByVal submitTimeofclaim As Date?,
                                              ByVal oopMaxApplied As Decimal?,
                                              ByVal halfTabFlag As String,
                                              ByVal remitType As String,
                                              ByVal planTypeCode As String,
                                              ByVal tierValue As String,
                                              ByVal phHraAmt As Decimal?,
                                              ByVal altInsFlag As String,
                                              ByVal adminFeeDec As Decimal?,
                                              ByVal regionDisasterOverride As String,
                                              ByVal claimAdj As Decimal?,
                                              ByVal maxId As String,
                                              ByVal benMaxApplied As Decimal?,
                                              ByVal awpTypeInd As String,
                                              ByVal macReducedInd As String,
                                              ByVal medBDInd As String,
                                              ByVal accDedAmount As Decimal?,
                                              ByVal cobPrimaryPayerAmountPaid As Decimal?,
                                              ByVal orgQuantity As Decimal?,
                                              ByVal fullAwpUnitPrice As Decimal?,
                                              ByVal patAge As Integer?,
                                              ByVal orgDaySupply As Integer?,
                                              ByVal clientPricingbasisOfCost As String,
                                              ByVal totalAmountPaidByAllSources As Decimal?,
                                            Optional ByVal transaction As DbTransaction = Nothing)



        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBHRA.INSERT_CVS_PRESCRIPTION_TRANSACTIONS"

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@PROC_BATCH_NBR", DbType.Int32, procBatchNbr)
            DB.AddInParameter(DBCommandWrapper, "@PRESCRIPTION_NBR", DbType.String, prescriptionNbr)
            DB.AddInParameter(DBCommandWrapper, "@PRESCRIPTION_SEQ", DbType.Decimal, prescriptionSeq)
            DB.AddInParameter(DBCommandWrapper, "@REFERENCE_NBR_QUAL", DbType.String, referenceNbrQual)
            DB.AddInParameter(DBCommandWrapper, "@DISPENSE_DATE", DbType.Date, dispenseDate)
            DB.AddInParameter(DBCommandWrapper, "@PAYMENT_TYPE", DbType.String, paymentType)
            DB.AddInParameter(DBCommandWrapper, "@PATIENT_PAY_AMT", DbType.Decimal, patientPayAmt)
            DB.AddInParameter(DBCommandWrapper, "@OTHER_PAYER_AMT", DbType.Decimal, otherPayerAmt)
            DB.AddInParameter(DBCommandWrapper, "@PAY_AMT_TO_DEDUCT", DbType.Decimal, payAmtToDeduct)
            DB.AddInParameter(DBCommandWrapper, "@PAT_FIRST_NAME", DbType.String, patFirstName)
            DB.AddInParameter(DBCommandWrapper, "@PAT_LAST_NAME", DbType.String, patLastName)
            DB.AddInParameter(DBCommandWrapper, "@PAT_BIRTH_DATE", DbType.Date, patBirthDate)
            DB.AddInParameter(DBCommandWrapper, "@PAT_GENDER", DbType.String, patGender)
            DB.AddInParameter(DBCommandWrapper, "@PAT_ID", DbType.Int32, patId)
            DB.AddInParameter(DBCommandWrapper, "@PERSON_CODE", DbType.String, personCode)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_CODE", DbType.String, relationCode)
            DB.AddInParameter(DBCommandWrapper, "@CARD_HOLDER_ID", DbType.String, cardholderId)
            DB.AddInParameter(DBCommandWrapper, "@CH_LAST_NAME", DbType.String, chLastName)
            DB.AddInParameter(DBCommandWrapper, "@CH_FIRST_NAME", DbType.String, chFirstName)
            DB.AddInParameter(DBCommandWrapper, "@CUSTOMER_LOCATION", DbType.String, customerLocation)
            DB.AddInParameter(DBCommandWrapper, "@FORMULARY_STATUS", DbType.String, formularyStatus)
            DB.AddInParameter(DBCommandWrapper, "@PLAN_DRUG_STATUS", DbType.String, planDrugStatus)
            DB.AddInParameter(DBCommandWrapper, "@GROUP_ID", DbType.String, groupId)
            DB.AddInParameter(DBCommandWrapper, "@SSNO", DbType.Int32, ssno)
            DB.AddInParameter(DBCommandWrapper, "@RX_SUBMIT_DATE", DbType.Date, rxSubmitDate)
            DB.AddInParameter(DBCommandWrapper, "@RX_CLAIM_NBR", DbType.Decimal, rxClaimNbr)
            DB.AddInParameter(DBCommandWrapper, "@RX_CLAIM_NBR_SEQ", DbType.Decimal, rxClaimNbrSeq)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_STATUS", DbType.String, claimStatus)
            DB.AddInParameter(DBCommandWrapper, "@PS_TRANS_CODE", DbType.String, psTransCode)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyId)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationId)
            DB.AddInParameter(DBCommandWrapper, "@PROCESS_ID", DbType.Decimal, processId)
            DB.AddInParameter(DBCommandWrapper, "@PHARMACY_NBR", DbType.String, pharmacyNbr)
            DB.AddInParameter(DBCommandWrapper, "@PHARMACY_QUAL", DbType.String, pharmacyQual)
            DB.AddInParameter(DBCommandWrapper, "@CLM_ORIG_FLAG", DbType.String, clmOrigFlag)
            DB.AddInParameter(DBCommandWrapper, "@PHARM_NETWORK", DbType.String, pharmNetwork)
            DB.AddInParameter(DBCommandWrapper, "@PHARM_AFFILIATE", DbType.String, pharmAffiliate)
            DB.AddInParameter(DBCommandWrapper, "@PHARM_DISP_CLASS", DbType.String, pharmDispClass)
            DB.AddInParameter(DBCommandWrapper, "@PHARM_DISP_TYPE", DbType.String, pharmDispType)
            DB.AddInParameter(DBCommandWrapper, "@PRODUCT_ID", DbType.String, productId)
            DB.AddInParameter(DBCommandWrapper, "@PRODUCT_ID_QUAL", DbType.String, productIdQual)
            DB.AddInParameter(DBCommandWrapper, "@LABEL_NAME", DbType.String, labelName.Trim)
            DB.AddInParameter(DBCommandWrapper, "@DOSAGE_FORM", DbType.String, dosageForm)
            DB.AddInParameter(DBCommandWrapper, "@STRENGTH_UNITS", DbType.String, strengthUnits)
            DB.AddInParameter(DBCommandWrapper, "@PROD_MANUFACTURER", DbType.String, prodManufacturer)
            DB.AddInParameter(DBCommandWrapper, "@MET_DEC_QUANTITY", DbType.Decimal, metDecQuantity)
            DB.AddInParameter(DBCommandWrapper, "@DAYS_SUPPLY", DbType.Decimal, daysSupply)
            DB.AddInParameter(DBCommandWrapper, "@DRUG_RD_OTC_IND", DbType.String, drugRdOtcInd)
            DB.AddInParameter(DBCommandWrapper, "@DRUG_STRENGTH", DbType.String, drugStrength)
            DB.AddInParameter(DBCommandWrapper, "@UNIT_OF_MEASURE", DbType.String, unitOfMeasure)
            DB.AddInParameter(DBCommandWrapper, "@ROUTE_OF_ADMIN", DbType.String, routeOfAdmin)
            DB.AddInParameter(DBCommandWrapper, "@DOSAGE_FORM_MEDI_SPAN", DbType.String, dosageFormMediSpan)
            DB.AddInParameter(DBCommandWrapper, "@BASIS_OF_REIMBURS", DbType.String, basisOfReimburs)
            DB.AddInParameter(DBCommandWrapper, "@COST_CODE_PRICING", DbType.String, costCodePricing)
            DB.AddInParameter(DBCommandWrapper, "@AWP_UNIT_COST", DbType.Decimal, awpUnitCost)
            DB.AddInParameter(DBCommandWrapper, "@INGRED_COST_BILLED", DbType.Decimal, ingredCostBilled)
            DB.AddInParameter(DBCommandWrapper, "@DISPENSE_FEE_BILLED", DbType.Decimal, dispenseFeeBilled)
            DB.AddInParameter(DBCommandWrapper, "@SALES_TAX", DbType.Decimal, salesTax)
            DB.AddInParameter(DBCommandWrapper, "@PRO_SERVICE_FEE", DbType.Decimal, proServiceFee)
            DB.AddInParameter(DBCommandWrapper, "@INCENT_AMT_BILLED", DbType.Decimal, incentAmtBilled)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ADJUST", DbType.Decimal, claimAdjust)
            DB.AddInParameter(DBCommandWrapper, "@TOT_AMT_BILLED", DbType.Decimal, totAmtBilled)
            DB.AddInParameter(DBCommandWrapper, "@OTH_AMT_BILLED", DbType.Decimal, othAmtBilled)
            DB.AddInParameter(DBCommandWrapper, "@USUAL_CUST_BILL_AMT", DbType.Decimal, usualCustBillAmt)
            DB.AddInParameter(DBCommandWrapper, "@PRESCRIBER_ID", DbType.String, prescriberId)
            DB.AddInParameter(DBCommandWrapper, "@PRESCRIBER_ID_QUAL", DbType.String, prescriberIdQual)
            DB.AddInParameter(DBCommandWrapper, "@PRESCRIBER_LAST_NAME", DbType.String, prescriberLastName)
            DB.AddInParameter(DBCommandWrapper, "@PRESCRIBER_FIRST_NAME", DbType.String, prescriberFirstName)
            DB.AddInParameter(DBCommandWrapper, "@PRESCRIBER_SPEC_CODE", DbType.String, prescriberSpecCode)
            DB.AddInParameter(DBCommandWrapper, "@PRESCRIBER_NETWORK_ID", DbType.String, prescriberNetworkId)
            DB.AddInParameter(DBCommandWrapper, "@DIAG_CODE", DbType.String, diagCode)
            DB.AddInParameter(DBCommandWrapper, "@DIAG_CODE_QUAL", DbType.String, diagCodeQual)
            DB.AddInParameter(DBCommandWrapper, "@PRIOR_AUTH_QUAL", DbType.String, priorAuthQual)
            DB.AddInParameter(DBCommandWrapper, "@PRIOR_AUTH_NBR", DbType.String, priorAuthNbr)
            DB.AddInParameter(DBCommandWrapper, "@PRIOR_REASON_CODE", DbType.String, priorReasonCode)
            DB.AddInParameter(DBCommandWrapper, "@PRESC_WRITTEN_DATE", DbType.Date, prescWrittenDate)
            DB.AddInParameter(DBCommandWrapper, "@PROD_SEL_CODE", DbType.String, prodSelCode)
            DB.AddInParameter(DBCommandWrapper, "@OTH_COVERAGE_CODE", DbType.String, othCoverageCode)
            DB.AddInParameter(DBCommandWrapper, "@THIRD_PARTY_RETRICT_CODE", DbType.String, thirdPartyRetrictCode)
            DB.AddInParameter(DBCommandWrapper, "@COMPOUND_CODE", DbType.String, compoundCode)
            DB.AddInParameter(DBCommandWrapper, "@NBR_OF_REFILLS_AUTH", DbType.Decimal, nbrOfRefillsAuth)
            DB.AddInParameter(DBCommandWrapper, "@MULTI_SOURCE_CODE", DbType.String, multiSourceCode)
            DB.AddInParameter(DBCommandWrapper, "@BRAND_NAME_CODE", DbType.String, brandNameCode)
            DB.AddInParameter(DBCommandWrapper, "@DEA_CLASS_CODE", DbType.String, deaClassCode)
            DB.AddInParameter(DBCommandWrapper, "@PRIMARY_PROV_ID", DbType.String, primaryProvId)
            DB.AddInParameter(DBCommandWrapper, "@CARE_FACILITY_ID", DbType.String, carefacilityId)
            DB.AddInParameter(DBCommandWrapper, "@CARE_QUALIFIER_CODE", DbType.String, careQualifierCode)
            DB.AddInParameter(DBCommandWrapper, "@CARE_NETWORK_ID", DbType.String, carenetworkId)
            DB.AddInParameter(DBCommandWrapper, "@UNIT_DOSE_ID", DbType.String, unitDoseId)
            DB.AddInParameter(DBCommandWrapper, "@MAINT_DRUG_CODE", DbType.String, maintDrugCode)
            DB.AddInParameter(DBCommandWrapper, "@AHFS_CODE", DbType.String, ahfsCode)
            DB.AddInParameter(DBCommandWrapper, "@GENERIC_PROD_INDEX", DbType.String, genericProdIndex)
            DB.AddInParameter(DBCommandWrapper, "@GENERIC_NAME", DbType.String, genericName)
            DB.AddInParameter(DBCommandWrapper, "@GENERIC_CODE_NBR", DbType.String, genericCodeNbr)
            DB.AddInParameter(DBCommandWrapper, "@GENERIC_CODE_SEQ_NBR", DbType.String, genericCodeSeqNbr)
            DB.AddInParameter(DBCommandWrapper, "@CARRIER", DbType.String, carrier)
            DB.AddInParameter(DBCommandWrapper, "@ACCOUNT", DbType.String, account)
            DB.AddInParameter(DBCommandWrapper, "@BRAND_GENERIC_ID", DbType.String, brandGenericId)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_MESSAGE", DbType.String, claimMessage)
            DB.AddInParameter(DBCommandWrapper, "@BEN_MAX_FLAG", DbType.String, benMaxFlag)
            DB.AddInParameter(DBCommandWrapper, "@CLIENT_FLAG", DbType.String, clientFlag)
            DB.AddInParameter(DBCommandWrapper, "@BEN_MAX_APPLIED", DbType.Decimal, periodicBenMaxApplied)
            DB.AddInParameter(DBCommandWrapper, "@PERIOD_END_DATE", DbType.Date, periodEndDate)
            DB.AddInParameter(DBCommandWrapper, "@THERA_GROUP_ID", DbType.String, theraGroupId)
            DB.AddInParameter(DBCommandWrapper, "@FORMULARY_CLAIM_FLAG", DbType.String, formularyClaimFlag)
            DB.AddInParameter(DBCommandWrapper, "@MEDICAL_CLAIM_FLAG", DbType.String, medicalClaimFlag)
            DB.AddInParameter(DBCommandWrapper, "@PRIOR_AUTH_CLM_FLAG", DbType.String, priorAuthClmFlag)
            DB.AddInParameter(DBCommandWrapper, "@TRANS_PLANT_FLAG", DbType.String, transPlantFlag)
            DB.AddInParameter(DBCommandWrapper, "@INJECT_PROD_FLAG", DbType.String, injectProdFlag)
            DB.AddInParameter(DBCommandWrapper, "@RXSOL_FORMU_FLAG", DbType.String, rxsolFormuFlag)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_DATE", DbType.Date, checkDate)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_NBR", DbType.String, checkNbr)
            DB.AddInParameter(DBCommandWrapper, "@HCPCS_CODE", DbType.String, hcpcsCode)
            DB.AddInParameter(DBCommandWrapper, "@CLIENT_CODE", DbType.String, clientCode)
            DB.AddInParameter(DBCommandWrapper, "@CLIENT_RIDER_CODE", DbType.String, clientRiderCode)
            DB.AddInParameter(DBCommandWrapper, "@CLIENT_BEN_CODE", DbType.String, clientBenCode)
            DB.AddInParameter(DBCommandWrapper, "@PLAN_QUAL_CODE", DbType.String, planQualCode)
            DB.AddInParameter(DBCommandWrapper, "@ADMIN_FEE", DbType.Int32, adminFee)
            DB.AddInParameter(DBCommandWrapper, "@CLIENT_DEFINED", DbType.String, clientDefined)
            DB.AddInParameter(DBCommandWrapper, "@RESERVED", DbType.String, reserved)
            DB.AddInParameter(DBCommandWrapper, "@PHARMACY_NAME", DbType.String, pharmacyName)
            DB.AddInParameter(DBCommandWrapper, "@BASIS_OF_COST_DETERM", DbType.String, basisOfCostDeterm)
            DB.AddInParameter(DBCommandWrapper, "@INGRED_COST_SUBMIT", DbType.Decimal, ingredCostSubmit)
            DB.AddInParameter(DBCommandWrapper, "@INGRED_COST_CALC", DbType.Decimal, ingredCostCalc)
            DB.AddInParameter(DBCommandWrapper, "@OUT_OF_NET_PENALTY", DbType.Decimal, outOfNetPenalty)
            DB.AddInParameter(DBCommandWrapper, "@CLIENT_COPAY", DbType.Decimal, clientCopay)
            DB.AddInParameter(DBCommandWrapper, "@NET_PATIENT_PAY_AMT", DbType.Decimal, netPatientPayAmt)
            DB.AddInParameter(DBCommandWrapper, "@PLAN_COPAY", DbType.Decimal, planCopay)
            DB.AddInParameter(DBCommandWrapper, "@PROD_SELC_DIFF", DbType.Decimal, prodSelcDiff)
            DB.AddInParameter(DBCommandWrapper, "@PAT_MID_INT", DbType.String, patMidInt)
            DB.AddInParameter(DBCommandWrapper, "@PART_ZIP", DbType.String, partZip)
            DB.AddInParameter(DBCommandWrapper, "@PRESCRIBER_DEA_NBR", DbType.String, prescriberDeaNbr)
            DB.AddInParameter(DBCommandWrapper, "@CH_MID_INT", DbType.String, chMidInt)
            DB.AddInParameter(DBCommandWrapper, "@CH_DOB", DbType.Date, IIf(chDob.Value.ToShortDateString.Equals("1/1/0001"), Nothing, chDob))
            DB.AddInParameter(DBCommandWrapper, "@CH_ADD1", DbType.String, chAdd1)
            DB.AddInParameter(DBCommandWrapper, "@CH_ADD2", DbType.String, chAdd2)
            DB.AddInParameter(DBCommandWrapper, "@CH_CITY", DbType.String, chCity)
            DB.AddInParameter(DBCommandWrapper, "@CH_STATE", DbType.String, chState)
            DB.AddInParameter(DBCommandWrapper, "@CH_ZIP", DbType.String, chZip)
            DB.AddInParameter(DBCommandWrapper, "@RX_ORGIN", DbType.String, rxOrgin)
            DB.AddInParameter(DBCommandWrapper, "@PAT_RESIDENCE", DbType.String, patResidence)
            DB.AddInParameter(DBCommandWrapper, "@SPECIALTY_FLAG", DbType.String, specialtyFlag)
            DB.AddInParameter(DBCommandWrapper, "@PLAN_CODE", DbType.String, planCode)
            DB.AddInParameter(DBCommandWrapper, "@SUBMIT_TIME_OF_CLAIM", DbType.Time, submitTimeofclaim)
            DB.AddInParameter(DBCommandWrapper, "@OOP_MAX_APPLIED", DbType.Decimal, oopMaxApplied)
            DB.AddInParameter(DBCommandWrapper, "@HALF_TAB_FLAG", DbType.String, halfTabFlag)
            DB.AddInParameter(DBCommandWrapper, "@REMIT_TYPE", DbType.String, remitType)
            DB.AddInParameter(DBCommandWrapper, "@PLAN_TYPE_CODE", DbType.String, planTypeCode)
            DB.AddInParameter(DBCommandWrapper, "@TIER_VALUE", DbType.String, tierValue)
            DB.AddInParameter(DBCommandWrapper, "@PH_HRA_AMT", DbType.Decimal, phHraAmt)
            DB.AddInParameter(DBCommandWrapper, "@ALT_INS_FLAG", DbType.String, altInsFlag)
            DB.AddInParameter(DBCommandWrapper, "@ADMIN_FEE_DEC", DbType.Decimal, adminFeeDec)
            DB.AddInParameter(DBCommandWrapper, "@REGION_DISASTER_OVERRIDE", DbType.String, regionDisasterOverride)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ADJ", DbType.Decimal, claimAdj)
            'DB.AddInParameter(DBCommandWrapper, "@MAXID", DbType.String, maxId)
            DB.AddInParameter(DBCommandWrapper, "@AMT_EXCEED_BEN_MAX", DbType.Decimal, benMaxApplied)
            DB.AddInParameter(DBCommandWrapper, "@AWP_TYPE_INDICATOR", DbType.String, awpTypeInd)
            DB.AddInParameter(DBCommandWrapper, "@MAC_REDUCED_IND", DbType.String, macReducedInd)
            DB.AddInParameter(DBCommandWrapper, "@MED_B_D_IND", DbType.String, medBDInd)
            DB.AddInParameter(DBCommandWrapper, "@ACCUM_DED_AMOUNT", DbType.Decimal, accDedAmount)
            DB.AddInParameter(DBCommandWrapper, "@COB_PRIMARY_PAYER_AMOUNT_PAID", DbType.Decimal, cobPrimaryPayerAmountPaid)
            DB.AddInParameter(DBCommandWrapper, "@ORIGINAL_QUANTITY", DbType.Decimal, orgQuantity)
            DB.AddInParameter(DBCommandWrapper, "@FULL_AWP_UNIT_PRICE", DbType.Decimal, fullAwpUnitPrice)
            DB.AddInParameter(DBCommandWrapper, "@PATIENT_AGE", DbType.Int32, patAge)
            DB.AddInParameter(DBCommandWrapper, "@ORIGINAL_DAY_SUPPLY", DbType.Int32, orgDaySupply)
            DB.AddInParameter(DBCommandWrapper, "@CLIENT_PRICING_BASIS_OF_COST", DbType.String, clientPricingbasisOfCost)
            DB.AddInParameter(DBCommandWrapper, "@GROSS_AMT_PAID", DbType.Decimal, totalAmountPaidByAllSources)


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

    Public Shared Function RetrieveCVSDMEligibleExtract(Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim ds As DataSet
        Dim SQLCall As String = "FDBHRA.RETRIEVE_CVS_DM_ELIG_EXTRACT"
        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DBCommandWrapper.CommandTimeout = 180

            If transaction Is Nothing Then
                ds = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function
End Class