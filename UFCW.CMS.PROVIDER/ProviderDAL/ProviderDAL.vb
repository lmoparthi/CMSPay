Option Strict On

Imports System.Windows.Forms
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports DDTek.DB2
Imports System.Security.Principal
Imports System.Data.Common

''' -----------------------------------------------------------------------------
''' Project	 : ProviderDAL
''' Class	 : ProviderDAL
''' 
''' -----------------------------------------------------------------------------
''' <summary>
''' Data Access Layer for the Provider UI
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[Mark Stone]	9/28/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Class ProviderDAL

    Public Shared _DomainUser As String = SystemInformation.UserName
    Public Shared _WindowsUserID As Security.Principal.WindowsIdentity = WindowsIdentity.GetCurrent()
    Public Shared _ComputerName As String = SystemInformation.ComputerName

    Private _DS As DataSet
    Private _ProviderID As Integer

    Public Property ProviderID() As Integer
        Set(ByVal Value As Integer)
            _ProviderID = Value
        End Set
        Get

        End Get
    End Property
    Public WriteOnly Property Dataset() As DataSet
        Set(ByVal Value As DataSet)
            _DS = Value
        End Set
    End Property

#Region "Constructor"
    Private Sub New()

    End Sub
#End Region

#Region "CRUD"

    Public Shared Function RetrieveProviderAddressSummaryWithComments(ByVal providerID As Integer, Optional ByVal mailOnly As Boolean = False, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_PROVIDERADDRESSSUMMARY_BY_PROVIDERID"

        Try

            db = CMSDALCommon.CreateDatabase()

            dbCommandWrapper = db.GetStoredProcCommand(SQLCall)
            db.AddInParameter(dbCommandWrapper, "@PROVIDER_ID", DbType.Int32, providerID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = db.ExecuteDataSet(dbCommandWrapper)
                Else
                    ds = db.ExecuteDataSet(dbCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    db.LoadDataSet(dbCommandWrapper, ds, "ProviderAddressSummaryWithComments")
                Else
                    db.LoadDataSet(dbCommandWrapper, ds, "ProviderAddressSummaryWithComments", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
            Return Nothing
        End Try
    End Function

    Public Shared Function RetrieveAssociatedProviders(ByVal providerID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_PROVIDER_BY_ASSOCIATED_PROVIDERID"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PROVIDER_ID", DbType.Int32, providerID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "ProviderAssociatedSummary")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "ProviderAssociatedSummary", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
            Return Nothing
        End Try
    End Function
    Public Shared Function RetrieveProviderHistory(ByVal providerID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_PROVIDER_LOG_BY_PROVIDERID"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PROVIDER_ID", DbType.Int32, providerID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "ProviderHistory")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "ProviderHistory", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
            Return Nothing
        End Try
    End Function
    Public Shared Function RetrieveProvidersByName(ByVal providerName As String, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_PROVIDERS_BY_NAME"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PROVIDER_NAME", DbType.String, providerName)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "PROVIDERS")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "PROVIDERS", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
            Return Nothing
        End Try
    End Function
    Public Shared Function RetrieveNPIAndUFCWProvidersByName(ByVal providerName As String, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_UFCW_NPI_PROVIDERS_BY_NAME"
        Dim Tablenames() As String = {"PROVIDER", "NPI_REGISTRY"}

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PROVIDER_NAME", DbType.String, providerName)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, tablenames)
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, tablenames, transaction)
                End If
            End If
            Return ds
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
            Return Nothing
        End Try
    End Function

    Public Shared Function RetrieveNPIAndUFCWProvidersByNPI(ByVal npi As Decimal, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_UFCW_NPI_PROVIDERS_BY_NPI"
        Dim Tablenames() As String = {"PROVIDER", "NPI_REGISTRY"}

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@NPI", DbType.Decimal, NPI)

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
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
            Return Nothing
        End Try
    End Function
    Public Shared Function RetrieveProvidersByNPI(ByVal npi As Decimal, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_PROVIDERS_BY_NPI"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@NPI", DbType.Decimal, NPI)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "PROVIDERS")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "PROVIDERS", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
            Return Nothing
        End Try
    End Function
    Public Shared Function RetrieveProvidersByTAXID(ByVal taxID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_PROVIDERS_BY_TAXID"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PROVIDER_TAXID", DbType.Int32, taxID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "PROVIDERS")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "PROVIDERS", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
            Return Nothing
        End Try
    End Function
    Public Shared Function RetrieveNPIAndUFCWProvidersByTAXID(ByVal taxID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_UFCW_NPI_PROVIDERS_BY_TAXID"
        Dim Tablenames() As String = {"PROVIDER", "NPI_REGISTRY"}

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PROVIDER_TAXID", DbType.Int32, taxID)

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
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
            Return Nothing
        End Try
    End Function
    Public Shared Function RetrieveProvidersByPROVIDERID(ByVal providerID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_PROVIDERS_BY_PROVIDERID"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PROVIDER_ID", DbType.Int32, providerID)

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
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
            Return Nothing
        End Try
    End Function

    Public Shared Function RetrieveProviderAddressTypes() As DataTable
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_PROVIDER_ADDRESS_TYPES"
        Dim DS As DataSet
        Dim DT As DataTable

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DS = DB.ExecuteDataSet(DBCommandWrapper)
            DT = New DataTable("PROVIDER_ADDRESS_TYPES")

            If DS.Tables.Count > 0 Then
                DT = DS.Tables(0)
            End If
            Return DT
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
            Return Nothing
        End Try
    End Function

    Public Shared Function RetrieveStates() As DataTable
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_STATES"
        Dim DS As DataSet
        Dim DT As DataTable

        Try
            DB = CMSDALCommon.CreateDatabase()

            dbCommandWrapper = db.GetStoredProcCommand(SQLCall)

            DS = DB.ExecuteDataSet(DBCommandWrapper)
            DT = New DataTable("LOOKUP_STATES")

            If ds.Tables.Count > 0 Then
                DT = DS.Tables(0)
            End If

            Return DT

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
            Return Nothing
        End Try
    End Function

    Public Shared Function RetrieveProviderLicenseTypes() As DataTable
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_PROVIDER_LICENSE_TYPES"
        Dim DS As DataSet
        Dim DT As DataTable

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DS = DB.ExecuteDataSet(DBCommandWrapper)
            DT = New DataTable("PROVIDER_LICENSE_TYPE")

            If ds.Tables.Count > 0 Then
                DT = DS.Tables(0)
            End If

            Return DT

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
            Return Nothing
        End Try
    End Function
    Public Shared Function RetrieveProviderAlertTypes() As DataTable
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_PROVIDER_ALERT_TYPES"
        Dim DS As DataSet
        Dim DT As DataTable

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DS = DB.ExecuteDataSet(DBCommandWrapper)
            DT = New DataTable("ALERT_TYPES")

            If DS.Tables.Count > 0 Then
                DT = DS.Tables(0)
            End If
            Return DT
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
            Return Nothing
        End Try
    End Function
    Public Shared Function RetrieveProviderCount(Optional ByVal providerID As Integer? = Nothing, Optional ByVal taxID As Integer? = Nothing, Optional ByVal npi As Decimal? = Nothing) As Integer
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.COUNT_PROVIDER_MATCH"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PROVIDER_ID", DbType.Int32, providerID)
            DB.AddInParameter(DBCommandWrapper, "@TAXID", DbType.Int32, taxID)
            DB.AddInParameter(DBCommandWrapper, "@NPI", DbType.Decimal, NPI)
            DB.AddOutParameter(DBCommandWrapper, "@MATCH_COUNT", DbType.Int32, 5)


            DB.ExecuteDataSet(DBCommandWrapper)

            Try
                Return CInt(DB.GetParameterValue(DBCommandWrapper, "@MATCH_COUNT"))
            Catch
                Return 0
            End Try

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
            Return Nothing
        End Try
    End Function

    Public Shared Function RetrieveProviderLicenses(ByVal providerID As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_PROVIDERLICENSES_BY_PROVIDERID"

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PROVIDER_ID", DbType.Int32, providerID)

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "ProviderLicenses")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "ProviderLicenses", transaction)
                End If
            End If
            Return ds
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
            Return Nothing
        End Try
    End Function

    Public Shared Function RetrieveProvider(ByVal providerID As Integer, Optional ByVal mailOnly As Boolean = False, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_PROVIDER_BY_PROVIDERID"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PROVIDER_ID", DbType.Int32, providerID)
            If mailOnly Then
                DB.AddInParameter(DBCommandWrapper, "@MAILONLY", DbType.Int32, 1)
            Else
                DB.AddInParameter(DBCommandWrapper, "@MAILONLY", DbType.Int32, 0)
            End If

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
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
            Return Nothing
        End Try
    End Function
    Public Shared Sub ModifyProviderAddress(ByVal providerID As Integer, ByVal addressTS As Date?, ByVal addressType As Integer, ByVal suspendSW As Integer, _
                                            ByVal addressLine1 As String, ByVal addressLine2 As String, ByVal city As String, ByVal country As String, _
                                            ByVal state As String, ByVal zip As Decimal, ByVal zip_4 As Decimal?, ByVal email1 As String, _
                                            ByVal phone1 As Decimal?, ByVal extension1 As Integer?, ByVal contact1 As String, _
                                            ByVal email2 As String, ByVal phone2 As Decimal?, ByVal extension2 As Integer?, ByVal contact2 As String, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase

            SQLCall = "FDBMD." & IIf(IsDate(addressTS), "UPDATE", "CREATE").ToString & "_PROVIDER_ADDRESS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PROVIDER_ID", DbType.Int32, providerID)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_TYPE", DbType.Int32, addressType)
            DB.AddInParameter(DBCommandWrapper, "@SUSPEND_SW", DbType.Decimal, suspendSW)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_LINE1", DbType.String, addressLine1)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_LINE2", DbType.String, addressLine2)
            DB.AddInParameter(DBCommandWrapper, "@CITY", DbType.String, city)
            DB.AddInParameter(DBCommandWrapper, "@COUNTRY", DbType.String, IIf(IsDBNull(country), "UNITED STATES", country))
            DB.AddInParameter(DBCommandWrapper, "@STATE", DbType.String, state)
            DB.AddInParameter(DBCommandWrapper, "@ZIP", DbType.Decimal, zip)
            DB.AddInParameter(DBCommandWrapper, "@ZIP_4", DbType.Decimal, zip_4)
            DB.AddInParameter(DBCommandWrapper, "@CONTACT1", DbType.String, contact1)
            DB.AddInParameter(DBCommandWrapper, "@EXTENSION1", DbType.Int32, extension1)
            DB.AddInParameter(DBCommandWrapper, "@PHONE1", DbType.Decimal, phone1)
            DB.AddInParameter(DBCommandWrapper, "@EMAIL1", DbType.String, email1)
            DB.AddInParameter(DBCommandWrapper, "@CONTACT2", DbType.String, contact2)
            DB.AddInParameter(DBCommandWrapper, "@EXTENSION2", DbType.Int32, extension2)
            DB.AddInParameter(DBCommandWrapper, "@PHONE2", DbType.Decimal, phone2)
            DB.AddInParameter(DBCommandWrapper, "@EMAIL2", DbType.String, email2)
            DB.AddInParameter(DBCommandWrapper, "@USERID", DbType.String, _DomainUser.ToUpper)

            If IsDate(addressTS) Then
                DB.AddInParameter(DBCommandWrapper, "@ORIGINAL_ONLINE_DATE", DbType.DateTime, addressTS)
            End If

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        Catch ex As SqlClient.SqlException
            Stop
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub

    Public Shared Sub ModifyProvider(ByRef providerID As Integer, ByVal providerTS As Date?, ByVal taxID As Integer, ByVal taxIDType As String, _
                                     ByVal suspendSW As Integer, ByVal ppocEligibleSW As Integer, ByVal hraInEligibleSW As Integer, ByVal electronicOnlySW As Integer, ByVal npi As Decimal?, ByVal commentID As Integer?, ByVal alert As String, _
                                     ByVal name1 As String, ByVal name2 As String, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try

            DB = CMSDALCommon.CreateDatabase

            SQLCall = "FDBMD." & IIf(IsDate(providerTS), "UPDATE", "CREATE").ToString & "_PROVIDER_ESO"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            If IsDate(providerTS) Then
                DB.AddInParameter(DBCommandWrapper, "@PROVIDER_ID", DbType.Int32, providerID)
            End If

            DB.AddInParameter(DBCommandWrapper, "@TAXID", DbType.Int32, taxID)
            DB.AddInParameter(DBCommandWrapper, "@TAXID_TYPE", DbType.String, taxIDType)
            DB.AddInParameter(DBCommandWrapper, "@ALERT", DbType.String, alert)
            DB.AddInParameter(DBCommandWrapper, "@NAME1", DbType.String, name1)
            DB.AddInParameter(DBCommandWrapper, "@NAME2", DbType.String, name2)
            DB.AddInParameter(DBCommandWrapper, "@SUSPEND_SW", DbType.Decimal, suspendSW)
            DB.AddInParameter(DBCommandWrapper, "@PPOC_ELIGIBLE_SW", DbType.Decimal, ppocEligibleSW)
            DB.AddInParameter(DBCommandWrapper, "@HRA_INELIGIBLE_SW", DbType.Decimal, hraInEligibleSW)
            DB.AddInParameter(DBCommandWrapper, "@ELECTRONIC_USE_ONLY_SW", DbType.Decimal, electronicOnlySW)
            DB.AddInParameter(DBCommandWrapper, "@NPI", DbType.Decimal, npi)
            DB.AddInParameter(DBCommandWrapper, "@COMMENT_ID", DbType.Int32, commentID)
            DB.AddInParameter(DBCommandWrapper, "@USERID", DbType.String, _DomainUser.ToUpper)

            If IsDate(providerTS) = False Then
                DB.AddOutParameter(DBCommandWrapper, "@PROVIDERID", DbType.Int32, 10)
            Else
                DB.AddInParameter(DBCommandWrapper, "@ORIGINAL_ONLINE_DATE", DbType.DateTime, providerTS)
            End If

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

            If IsDate(providerTS) = False Then
                providerID = CInt(DB.GetParameterValue(DBCommandWrapper, "@PROVIDERID"))
            End If

        Catch ex As DB2Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub
    Public Shared Function ModifyProviderComments(ByVal comments As String, Optional ByVal transaction As DbTransaction = Nothing) As Integer

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_PROVIDER_COMMENT"

        Try

            DB = CMSDALCommon.CreateDatabase

            'Note, any changes to comments result in the original comments being archived and a new comments record created.

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@COMMENTS_TEXT", DbType.String, comments)
            DB.AddInParameter(DBCommandWrapper, "@USERID", DbType.String, _DomainUser.ToUpper)

            DB.AddOutParameter(DBCommandWrapper, "@COMMENTID", DbType.Int32, 10)

            Try

                If transaction Is Nothing Then
                    DB.ExecuteNonQuery(DBCommandWrapper)
                Else
                    DB.ExecuteNonQuery(DBCommandWrapper, transaction)
                End If

                Return CInt(DB.GetParameterValue(DBCommandWrapper, "@COMMENTID"))

            Catch ex As DB2Exception
                Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
                If (rethrow) Then
                    Throw
                End If
            Catch ex As Exception
                Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
                If (rethrow) Then
                    Throw
                End If
            End Try

        Catch ex As DB2Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Shared Sub ModifyProviderLicense(ByVal providerID As Integer, ByVal licenseTS As Date?, ByVal license As String, ByVal licenseDescription As String, _
                                            ByVal licenseID As String, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase

            SQLCall = "FDBMD." & IIf(IsDate(licenseTS) = True, "UPDATE", "CREATE").ToString & "_PROVIDER_LICENSE"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            If IsDate(licenseTS) = True Then
                DB.AddInParameter(DBCommandWrapper, "@LICENSE_ID", DbType.Int32, licenseID)
            End If

            DB.AddInParameter(DBCommandWrapper, "@PROVIDER_ID", DbType.Int32, providerID)
            DB.AddInParameter(DBCommandWrapper, "@LICENSE", DbType.String, license)
            DB.AddInParameter(DBCommandWrapper, "@LICENSE_DESCRIPTION", DbType.String, licenseDescription)
            DB.AddInParameter(DBCommandWrapper, "@USERID", DbType.String, _DomainUser.ToUpper)

            If IsDate(licenseTS) = True Then
                DB.AddInParameter(DBCommandWrapper, "@ORIGINAL_ONLINE_DATE", DbType.DateTime, licenseTS)
            End If

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception When ex.Number = -803

            ex.HelpLink = "Duplicate LICENSE(" & license & ")"

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub
    Public Shared Sub ModifyProviderHistory(ByVal providerID As Integer, ByVal transactionType As Integer, ByVal transactionComment As String, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_PROVIDER_LOG"

        Try
            DB = CMSDALCommon.CreateDatabase

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@PROVIDER_ID", DbType.Int32, providerID)
            DB.AddInParameter(DBCommandWrapper, "@TRANSACTION_TYPE", DbType.Int32, transactionType)
            DB.AddInParameter(DBCommandWrapper, "@TRANSACTION_COMMENT", DbType.String, transactionComment)
            DB.AddInParameter(DBCommandWrapper, "@USERID", DbType.String, _DomainUser.ToUpper)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As DB2Exception When ex.Number = -803

            ex.HelpLink = "Duplicate Log Entry"

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub
    Public Shared Sub DeleteProviderAddress(ByVal providerID As Integer, ByVal addressType As Integer, ByVal addressTS As Date?, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.DELETE_PROVIDER_ADDRESS"

        Try
            DB = CMSDALCommon.CreateDatabase

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@PROVIDER_ID", DbType.Int32, providerID)
            DB.AddInParameter(DBCommandWrapper, "@ADDRESS_TYPE", DbType.Int32, addressType)
            DB.AddInParameter(DBCommandWrapper, "@ORIGINAL_ONLINE_DATE", DbType.DateTime, addressTS)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub
    Public Shared Sub DeleteProviderLicense(ByVal licenseID As Integer, ByVal licenseTS As Date?, Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.DELETE_PROVIDER_LICENSE"

        Try
            DB = CMSDALCommon.CreateDatabase

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@LICENSE_ID", DbType.Int32, licenseID)
            DB.AddInParameter(DBCommandWrapper, "@ORIGINAL_ONLINE_DATE", DbType.DateTime, licenseTS)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub

#End Region
End Class
'Public Class EncryptionHelper
'    Shared entropy As Byte() = System.Text.Encoding.Unicode.GetBytes("Salt Is Not A Password")

'    Public Shared Function EncryptString(ByVal input As System.Security.SecureString) As String
'        Dim encryptedData As Byte() = System.Security.Cryptography.ProtectedData.Protect(System.Text.Encoding.Unicode.GetBytes(ToInsecureString(input)), entropy, System.Security.Cryptography.DataProtectionScope.CurrentUser)
'        Return Convert.ToBase64String(encryptedData)
'    End Function

'    Public Shared Function DecryptString(ByVal encryptedData As String) As SecureString
'        Try
'            Dim decryptedData As Byte() = System.Security.Cryptography.ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), entropy, System.Security.Cryptography.DataProtectionScope.CurrentUser)
'            Return ToSecureString(System.Text.Encoding.Unicode.GetString(decryptedData))
'        Catch
'            Return New SecureString()
'        End Try
'    End Function

'    Public Shared Function ToSecureString(ByVal input As String) As SecureString
'        Dim secure As New SecureString()
'        For Each c As Char In input
'            secure.AppendChar(c)
'        Next
'        secure.MakeReadOnly()
'        Return secure
'    End Function

'    Public Shared Function ToInsecureString(ByVal input As SecureString) As String
'        Dim returnValue As String = String.Empty
'        Dim ptr As IntPtr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input)
'        Try
'            returnValue = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr)
'        Finally
'            System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr)
'        End Try
'        Return returnValue
'    End Function


'    Public Shared Function SimpleCrypt(ByVal text As String) As String
'        ' Encrypts/decrypts the passed string using 
'        ' a simple ASCII value-swapping algorithm
'        Dim strTempChar As String = "", i As Integer
'        For i = 1 To Len(Text)
'            If Asc(Mid$(Text, i, 1)) < 128 Then
'                strTempChar = CType(Asc(Mid$(Text, i, 1)) + 128, String)
'            ElseIf Asc(Mid$(Text, i, 1)) > 128 Then
'                strTempChar = CType(Asc(Mid$(Text, i, 1)) - 128, String)
'            End If
'            Mid$(Text, i, 1) = Chr(CType(strTempChar, Integer))
'        Next i
'        Return Text
'    End Function

'End Class