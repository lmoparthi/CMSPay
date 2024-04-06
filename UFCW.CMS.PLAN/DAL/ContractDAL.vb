Option Explicit On
Option Strict On

Imports System
Imports System.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Data.Common

Friend NotInheritable Class ContractDAL
    ''' -----------------------------------------------------------------------------
    ''' Project	 : UFCW.CMS.Plan
    ''' Class	 : CMS.Plan.ContractDAL
    '''
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' This class handles database interaction for Contracts
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[paulw]	1/26/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------

    Private Shared _databaseName As String = CMSDALCommon.GetDatabaseName
#Region "Constructors"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' private constructor to avoid instantiation
    ' </summary>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/26/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub New()

    End Sub

#End Region

#Region "C.R.U.D"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Will publish a valid Contract
    ' </summary>
    ' <param name="beginDate"></param>
    ' <param name="endDate"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/26/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Sub PublishContract(ByVal newContract As Contract)
        Try
            If Not newContract.IsValidForPublication Then
                Throw New InvalidContractDataException("Contract Must Be Valid")
                Return
            End If

            Dim SQLCommand As String = _databaseName & "." & "CREATE_CONTRACT"
            Dim DB As Database = CMSDALCommon.CreateDatabase()

            Dim DBCommandWrapper As DbCommand = DB.GetStoredProcCommand(SQLCommand)

            DB.AddInParameter(DBCommandWrapper, "@BEGIN_DATE", DbType.Date, newContract.BeginDate)
            DB.AddInParameter(DBCommandWrapper, "@END_DATE", DbType.Date, newContract.EndDate)
            DB.ExecuteNonQuery(DBCommandWrapper)
        Catch ex As Exception

            Throw New InvalidContractDataException("Cannot Create Contract in Database", ex)

        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets beginning and end dates for a contract that falls within the request date
    '  parameter
    ' </summary>
    ' <param name="requestDate"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function GetContract(ByVal requestDate As Date) As Contract
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Try
            Dim SQLCommand As String = _databaseName & "." & "RETRIEVE_CONTRACT"
            Dim DBCommandWrapper As DbCommand = DB.GetStoredProcCommand(SQLCommand)

            DB.AddInParameter(DBCommandWrapper, "@REQUEST_DATE", DbType.Date, requestDate)
            DB.AddOutParameter(DBCommandWrapper, "@BEGIN_DATE", DbType.Date, 8)
            DB.AddOutParameter(DBCommandWrapper, "@END_DATE", DbType.Date, 8)
            DBCommandWrapper.CommandTimeout = 1200

            DB.CreateConnection.Open()
            DB.ExecuteNonQuery(DBCommandWrapper)
            If DB.GetParameterValue(DBCommandWrapper, "@BEGIN_DATE") Is System.DBNull.Value Then
                Return Nothing
            Else
                Return New Contract(CDate(DB.GetParameterValue(DBCommandWrapper, "@BEGIN_DATE")), CDate(DB.GetParameterValue(DBCommandWrapper, "@END_DATE")))
            End If
        Catch ex As Exception

            Throw New ContractDataException("Cannot Get Contract from the database", ex)

        Finally
            If DB.CreateConnection IsNot Nothing Then
                DB.CreateConnection.Close()
            End If
        End Try

    End Function
#End Region
End Class