
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data.Common

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.PARTICIPANT
''' Class	 : CMS.PARTICIPANT.RegMastDAL
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
Friend Class RegMastDAL
    Public Const _DB2databaseName As String = "FDBMD"
    Public Const _SQLdatabaseName As String = "dbo"

    Public Shared DomainUser As String = SystemInformation.UserName
    Private Sub New()

    End Sub

    Public Shared Function GetPARTICIPANTInformation(ByVal familyId As Integer, ByVal relationId As Integer, Optional ByVal DS As DataSet = Nothing) As DataSet

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBMD.RETRIEVE_PARTICIPANT_ADDRESS_BY_FAMILY_ID"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyId)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationId)

            If DS Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DB.LoadDataSet(DBCommandWrapper, DS, "REG_ADDRESS")
            End If

            Return DS

        Catch ex As Exception
            Throw
        End Try
    End Function

End Class