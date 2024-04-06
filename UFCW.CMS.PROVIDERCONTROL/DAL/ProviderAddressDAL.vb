Imports System.Text
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Provider
''' Class	 : CMS.Provider.ProviderAddressDAL
''' 
''' -----------------------------------------------------------------------------
''' <summary>
''' 
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[MAS]	1/17/2007	Created
''' </history>
''' -----------------------------------------------------------------------------
Friend Class ProviderAddressDAL
    Public Const _DB2databaseName As String = "FDBMD"
    Public Const _SQLdatabaseName As String = "dbo"

    Public Shared DomainUser As String = SystemInformation.UserName
    Private Sub New()

    End Sub
    Public Shared ReadOnly Property GetDatabaseName(ByVal Instance As String) As String
        Get
            Dim db As Database
            If Instance = Nothing Then
                db = DatabaseFactory.CreateDatabase()
            Else
                db = DatabaseFactory.CreateDatabase(Instance)
            End If

            If LCase(db.ConfigurationName) = "ddtek database instance" Then
                Return _DB2databaseName
            Else
                Return db.GetConnection.Database & ".dbo" '_SQLdatabaseName
            End If

            db = Nothing
        End Get
    End Property

    Public Shared Function GetProviderInformationByProviderID(ByVal ProviderId As Integer, Optional ByVal DS As DataSet = Nothing) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommandWrapper As DBCommandWrapper
        Dim SQLCall As String

        Try

            SQLCall = GetDatabaseName(Nothing) & "." & "RETRIEVE_PROVIDERMAILADDRESS_BY_PROVIDERID"
            dbCommandWrapper = db.GetStoredProcCommandWrapper(SQLCall)

            dbCommandWrapper.AddInParameter("@PROVIDER_ID", DbType.Int32, ProviderId)

            If DS Is Nothing Then
                DS = db.ExecuteDataSet(dbCommandWrapper)
            Else
                db.LoadDataSet(dbCommandWrapper, DS, "PROVIDER_ADDRESS")
            End If

            Return DS


        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw New Exception("Cannot Retrieve Provider Information", ex)
            End If
        End Try
    End Function
    Public Shared Function GetProviderInformationByTaxID(ByVal TaxID As Integer, Optional ByVal DS As DataSet = Nothing) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommandWrapper As DBCommandWrapper
        Dim SQLCall As String

        Try

            SQLCall = GetDatabaseName(Nothing) & "." & "RETRIEVE_PROVIDERMAILADDRESSES_BY_TAXID"
            dbCommandWrapper = db.GetStoredProcCommandWrapper(SQLCall)

            dbCommandWrapper.AddInParameter("@TAXID", DbType.Int32, TaxID)

            If DS Is Nothing Then
                DS = db.ExecuteDataSet(dbCommandWrapper)
            Else
                db.LoadDataSet(dbCommandWrapper, DS, "PROVIDER_ADDRESS")
            End If

            Return DS

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw New Exception("Cannot Retrieve Provider Information", ex)
            End If
        End Try
    End Function

End Class