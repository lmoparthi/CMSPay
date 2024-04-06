Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.IO
Imports System.Data.Common

Friend Class PremiumsDAL
    Public Const _DB2databaseName As String = "HOURS"
    Public Const _SQLdatabaseName As String = "dbo"
    Public Shared DomainUser As String = SystemInformation.UserName

#Region "Procedures/Functions"

    Public Shared Function CreateDataSetFromXML(XMLFile As String) As DataSet

        'load xml into a dataset to use here
        Dim DS As New DataSet
        Dim FS As FileStream = Nothing

        'open the xml file so we can use it to fill the dataset
        Try
            FS = New FileStream(System.Windows.Forms.Application.StartupPath & "\" & XMLFile, FileMode.Open, FileAccess.Read)

            DS.ReadXml(FS)

            Return DS

        Catch ex As Exception

                Throw

        Finally
            If DS IsNot Nothing Then DS.Dispose()
            DS = Nothing

            If FS IsNot Nothing Then
                FS.Close()
                FS.Dispose()
            End If
            FS = Nothing
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

    Public Shared Function GetPremiumPaymentsInformation(ByVal familyId As Integer, Optional ByVal DS As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RETRIEVE_PREMAUD_BY_FAMILYID")

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyId)
            DB.AddInParameter(DBCommandWrapper, "@EMPLOYEE_ACCESS", DbType.Int32, If(UFCWGeneralAD.CMSCanAdjudicateEmployee() OrElse UFCWGeneralAD.CMSEligibilityEmployee(), 1, 0))
            DB.AddInParameter(DBCommandWrapper, "@LOCAL_ACCESS", DbType.Int32, If(UFCWGeneralAD.CMSLocalsEmployee() OrElse UFCWGeneralAD.CMSEligibility(), 1, 0))
            DBCommandWrapper.CommandTimeout = 180

            If DS Is Nothing Then
                If transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "PREMAUD")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "PREMAUD", transaction)
                End If
            End If
            Return DS
        Catch ex As Exception

                Throw
        End Try
    End Function

#End Region
End Class