Option Infer On
Option Strict On

Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Configuration
Imports DDTek.DB2
Imports System.Data.Common


Public NotInheritable Class CMSDALFDBIS

    ''Private Shared _TraceCaching As New BooleanSwitch("TraceCaching", "Trace Switch in App.Config")

    Public Shared Event RecentClaimRefreshAvailable(e As RecentClaimEventArgs)

    Public Const _DB2SchemaName As String = "FDBMD"

    Private Shared _ComputerName As String = UFCWGeneral.ComputerName
    Private Shared _LastSQL As String

    Private Shared _GetLettersSyncLock As New Object


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

#Region "CRUD"

    Public Shared Function RetrieveAdjusters(Optional ByVal docClass As String = "", Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBIS.RETRIEVE_ADJUSTERS_WITH_OPTIONAL_DOCCLASS"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@DOCCLASS", DbType.String, docClass)

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

    Public Shared Sub UpdateLetterStatusOrIMB(ByVal schema As String, ByVal ident As Integer?, ByVal lastUPDT As Date, ByVal mailStatus As String, ByVal imb As String, ByVal user As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBIS.UPDATE_LETTER_IMB"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@SCHEMA", DbType.String, schema)
            DB.AddInParameter(DBCommandWrapper, "@IDENT", DbType.Decimal, ident)
            DB.AddInParameter(DBCommandWrapper, "@LASTUPDT", DbType.DateTime, UFCWGeneral.ToNullDateHandler(lastUPDT))
            DB.AddInParameter(DBCommandWrapper, "@MAILSTATUS", DbType.String, mailStatus)
            DB.AddInParameter(DBCommandWrapper, "@IMB", DbType.String, imb)
            DB.AddInParameter(DBCommandWrapper, "@USERID", DbType.String, user)

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


End Class