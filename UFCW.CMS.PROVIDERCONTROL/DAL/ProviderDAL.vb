Imports Microsoft.Practices.EnterpriseLibrary.Data

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
Friend Class ProviderDAL

    Public Shared DomainUser As String = SystemInformation.UserName
    Private Sub New()

    End Sub

    Public Shared Function GetNPIRegistryByNPI(ByVal NPI As Decimal, Optional ByVal DS As DataSet = Nothing) As DataSet

        Dim DB As Database
        Dim dbCommandWrapper As Data.Common.DbCommand
        Dim SQLCall As String

        Try

            DB = CMSDALCommon.CreateDatabase()
            SQLCall = "FDBMD.RETRIEVE_NPI_REGISTRY_BY_NPI"
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

    Public Shared Function GetProviderInformationByProviderID(ByVal ProviderId As Integer, Optional ByVal DS As DataSet = Nothing) As DataSet

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim dbCommandWrapper As Data.Common.DbCommand
        Dim SQLCall As String

        Try

            SQLCall = "FDBMD.RETRIEVE_PROVIDERMAILADDRESS_BY_PROVIDERID"
            dbCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(dbCommandWrapper, "@PROVIDER_ID", DbType.Int32, ProviderId)

            If DS Is Nothing Then
                DS = DB.ExecuteDataSet(dbCommandWrapper)
            Else
                DB.LoadDataSet(dbCommandWrapper, DS, "PROVIDER_ADDRESS")
            End If

            Return DS

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetProviderInformationByTaxID(ByVal TaxID As Integer, Optional ByVal DS As DataSet = Nothing) As DataSet

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim dbCommandWrapper As Data.Common.DbCommand
        Dim SQLCall As String

        Try

            SQLCall = "FDBMD.RETRIEVE_PROVIDERMAILADDRESSES_BY_TAXID"
            dbCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(dbCommandWrapper, "@TAXID", DbType.Int32, TaxID)

            If DS Is Nothing Then
                DS = DB.ExecuteDataSet(dbCommandWrapper)
            Else
                DB.LoadDataSet(dbCommandWrapper, DS, "PROVIDER_ADDRESS")
            End If

            Return DS

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetProviderInformationByNPI(ByVal NPI As Decimal, Optional ByVal DS As DataSet = Nothing) As DataSet

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim dbCommandWrapper As Data.Common.DbCommand
        Dim SQLCall As String

        Try

            SQLCall = "FDBMD.RETRIEVE_PROVIDERMAILADDRESSES_BY_NPI"
            dbCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(dbCommandWrapper, "@NPI", DbType.Decimal, NPI)

            If DS Is Nothing Then
                DS = DB.ExecuteDataSet(dbCommandWrapper)
            Else
                DB.LoadDataSet(dbCommandWrapper, DS, "PROVIDER_ADDRESS")
            End If

            Return DS

        Catch ex As Exception
            Throw
        End Try
    End Function
End Class