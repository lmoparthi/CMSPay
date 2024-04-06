Option Strict on

Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.IO
Imports System.Data.Common

Friend Class HRADAL
    Public Const _SQLdatabaseName As String = "dbo"
    Public Shared _DomainUser As String = SystemInformation.UserName

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
            Return Nothing
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

    Public Shared Sub InsertHRATransaction(ByVal transactionType As Integer, ByVal familyID As Integer, ByVal relationID As Short?, ByVal transactionAmt As Decimal?, _
                                            ByVal effectiveDate As Date?, ByVal expirationDate As Date?, _
                                            ByVal comments As String, ByVal UserRights As String, _
                                             Optional ByVal transaction As DbTransaction = Nothing)

        InsertHRATransaction(transactionType, familyID, relationID, Nothing, Nothing, Nothing, transactionAmt, effectiveDate, expirationDate, comments, UserRights, Nothing, transaction)

    End Sub

    Public Shared Sub InsertHRATransaction(ByVal transactionType As Integer, ByVal familyID As Integer, ByVal relationID As Short?, ByVal transactionAmt As Decimal?, _
                                            ByVal effectiveDate As Date?, ByVal expirationDate As Date?, _
                                            ByVal comments As String, ByVal UserRights As String, ByVal hraTransientID As Integer?, _
                                            Optional ByVal transaction As DbTransaction = Nothing)

        InsertHRATransaction(transactionType, familyID, relationID, Nothing, Nothing, Nothing, transactionAmt, effectiveDate, expirationDate, comments, UserRights, hraTransientID, transaction)

    End Sub

    Public Shared Sub InsertHRATransaction(ByVal transactionType As Integer, ByVal familyID As Integer, ByVal relationID As Short?, ByVal transactionAmt As Decimal?, _
                                            ByVal effectiveDate As Date?, _
                                            ByVal comments As String, ByVal UserRights As String, ByVal hraTransientID As Integer?, _
                                            Optional ByVal transaction As DbTransaction = Nothing)

        InsertHRATransaction(transactionType, familyID, relationID, Nothing, Nothing, Nothing, transactionAmt, effectiveDate, Nothing, comments, UserRights, hraTransientID, transaction)

    End Sub

    Public Shared Sub InsertHRATransaction(ByVal transactionType As Integer, ByVal familyID As Integer, ByVal relationID As Short?, ByVal claimID As Integer?, _
                                            ByVal LineNbr As Short?, ByVal checkNbr As Integer?, ByVal transactionAmt As Decimal?, _
                                            ByVal effectiveDate As Date?, ByVal expirationDate As Date?, _
                                            ByVal comments As String, ByVal UserRights As String, ByVal hraTransientID As Integer?, _
                                            Optional ByVal transaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.CREATE_HRA_TRANSACTION_POST_TRANSIENT"

        Try

            If IsDBNull(expirationDate) OrElse IsNothing(expirationDate) OrElse CDate(expirationDate) = CDate("12/31/9998") Then 'Necassary due to control's maxdate limitation
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
        Catch ex As Exception

	Throw
        End Try

        'fill the dataset
        Try
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
            Return Nothing
        Finally
            FS.Close()
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
End Class