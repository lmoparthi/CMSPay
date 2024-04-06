Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data.Common


''' -----------------------------------------------------------------------------
''' Project	 : Accumulator
''' Class	 : CMS.AccumulatorMemberAccumulatorEntryDAL
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' Manges MemberAccumulatorEntrys
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	12/15/2005	Created
''' </history>
''' -----------------------------------------------------------------------------
Friend NotInheritable Class MemberAccumulatorEntryDAL
#Region "CRUD"
    Public Shared Function GetHighestEntryIdForFamily(ByVal familyId As Integer) As Integer

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCommand As String = "FDBMD.RETRIEVE_MAX_MEM_ACCUM_ENTRY_ID_FOR_FAMILY"

        Try
            dbCommandWrapper = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(dbCommandWrapper, "@FAMILY_ID", DbType.Int32, familyId)
            db.AddOutParameter(dbCommandWrapper, "@ENTRY_ID", DbType.Int32, 8)

            db.ExecuteScalar(dbCommandWrapper)

            If db.GetParameterValue(dbCommandWrapper, "@ENTRY_ID") Is System.DBNull.Value Then
                Return 0
            Else
                Return Integer.Parse(CStr(db.GetParameterValue(dbCommandWrapper, "@ENTRY_ID")))
            End If
        Catch ex As Exception
            Throw
        Finally
            db = Nothing
        End Try
    End Function

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Commits a MemberAccumulatorEntry and corresponding summary table to the database
    ' </summary>
    ' <param name="MemberAccumulatorEntrys"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	12/15/2005	Created
    '     [paulw] 1/18/2006   Removed "CREATE_DATE" per Ron Lewis.  He is handling
    '                         this on his side
    '     [paulw] 1/18/2006   Refactored to accomodate transactions
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Sub CommitAll(ByVal memberAccumulatorEntries As DataTable, ByVal proposedSummary As DataTable)
        If memberAccumulatorEntries Is Nothing Then Return
        If proposedSummary Is Nothing Then Return

        Dim DB As Database
        Dim connection As DbConnection
        Dim transaction As DbTransaction

        Try
            db = CMSDALCommon.CreateDatabase()
            connection = db.CreateConnection()

            connection.Open()
            transaction = connection.BeginTransaction()
            CommitAll(memberAccumulatorEntries, proposedSummary, transaction)
            transaction.Commit()

        Catch ex As Exception
            Try

                If transaction IsNot Nothing AndAlso transaction.Connection IsNot Nothing Then
                    transaction.Rollback()
                End If

            Finally
            End Try

            Throw
        Finally
            If Not connection Is Nothing Then
                connection.Close()
            End If
        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Commit the entries within the context of a transaction
    ' </summary>
    ' <param name="memberAccumulatorEntries"></param>
    ' <param name="proposedSummary"></param>
    ' <param name="transaction"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	5/24/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Sub CommitAll(ByVal memberAccumulatorEntries As DataTable, ByVal proposedSummary As DataTable, ByVal transaction As DbTransaction)
        If memberAccumulatorEntries Is Nothing Then Return
        If proposedSummary Is Nothing Then Return
        Try
            CommitEntries(memberAccumulatorEntries, transaction)
            CommitSummaries(proposedSummary, transaction)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Commits a MemberAccumulatorEntries to the database
    ' </summary>
    ' <param name="memberAccumulatorEntries"></param>
    ' <param name="dalDataBase"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    '     [paulw]	12/15/2005	Created
    ' 	[paulw]	1/18/2006	Refactored from CommitAll
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Shared Sub CommitEntries(ByVal memberAccumulatorEntries As DataTable, ByVal transaction As DbTransaction) ', ByVal connection As IDbConnection)
        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Try
            For i As Integer = 0 To memberAccumulatorEntries.Rows.Count - 1
                'we dont want to commit entries of 0.  this is basically superfluous information
                '8/15/2007 - per gary mitchem, we DO need to post to accident accumulators, even if 0 value.
                If Decimal.Parse(memberAccumulatorEntries.Rows(i)("ENTRY_VALUE")) <> 0 Or AccumulatorController.GetAccumulatorName(Integer.Parse(CStr(memberAccumulatorEntries.Rows(i)("ACCUM_ID")))).Trim.StartsWith("AC") Then
                    SQLCommand = "FDBMD.CREATE_MEM_ACCUM_ENTRY"
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)

                    DB.AddInParameter(DBCommandWrapper, "@ACCUM_ID", DbType.Int32, memberAccumulatorEntries.Rows(i)("ACCUM_ID"))
                    If memberAccumulatorEntries.Rows(i)("CLAIM_ID") Is System.DBNull.Value Then
                        DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, -1)
                    Else
                        DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, memberAccumulatorEntries.Rows(i)("CLAIM_ID"))
                    End If
                    If memberAccumulatorEntries.Rows(i)("LINE_NBR") Is System.DBNull.Value Then
                        DB.AddInParameter(DBCommandWrapper, "@LINE_NBR", DbType.Int16, -1)
                    Else
                        DB.AddInParameter(DBCommandWrapper, "@LINE_NBR", DbType.Int16, memberAccumulatorEntries.Rows(i)("LINE_NBR"))
                    End If
                    DB.AddInParameter(DBCommandWrapper, "@ORG_ACCIDENT_CLAIM_ID", DbType.Int32, memberAccumulatorEntries.Rows(i)("ORG_ACCIDENT_CLAIM_ID"))
                    'is it family proposedSummary, pass 0 to memberid
                    If AccumulatorController.GetAccumulatorIsFamily(CInt(memberAccumulatorEntries.Rows(i)("ACCUM_ID"))) Then
                        DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, 0)
                    Else
                        DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, memberAccumulatorEntries.Rows(i)("RELATION_ID"))
                    End If
                    DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, memberAccumulatorEntries.Rows(i)("FAMILY_ID"))
                    DB.AddInParameter(DBCommandWrapper, "@APPLY_DATE", DbType.Date, memberAccumulatorEntries.Rows(i)("APPLY_DATE"))
                    DB.AddInParameter(DBCommandWrapper, "@ENTRY_VALUE", DbType.Decimal, memberAccumulatorEntries.Rows(i)("ENTRY_VALUE"))
                    DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String, CStr(memberAccumulatorEntries.Rows(i)("CREATE_USERID")).ToUpper)
                    DB.AddInParameter(DBCommandWrapper, "@OVERRIDE_SW", DbType.Decimal, memberAccumulatorEntries.Rows(i)("OVERRIDE_SW"))
                    DB.ExecuteNonQuery(DBCommandWrapper, transaction)
                End If
            Next

        Catch ex As Exception
            Throw
        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Commits a summary to the database
    ' </summary>
    ' <param name="proposedSummary"></param>
    ' <param name="dalDataBase"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    '     [paulw]	12/15/2005	Created
    ' 	[paulw]	1/18/2006	Moved over from MemberAccumulatorDAL
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Shared Sub CommitSummaries(ByVal proposedSummary As DataTable, ByVal transaction As DbTransaction) ', ByVal connection As IDbConnection)
        If proposedSummary Is Nothing Then Return
        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Try

            For i As Integer = 0 To proposedSummary.Rows.Count - 1
                sqlCommand = "FDBMD.UPDATE_ACCUM_SUM"
                dbCommandWrapper = db.GetStoredProcCommand(sqlCommand)

                db.AddInParameter(dbCommandWrapper, "@ACCUM_ID", DbType.Int32, proposedSummary.Rows(i)("ACCUM_ID"))
                db.AddInParameter(dbCommandWrapper, "@ACCUM_YEAR", DbType.Int32, proposedSummary.Rows(i)("ACCUM_YEAR"))
                db.AddInParameter(dbCommandWrapper, "@FAMILY_ID", DbType.Int32, proposedSummary.Rows(i)("FAMILY_ID"))
                'is it family proposedSummary, pass 0 to memberid
                If AccumulatorController.GetAccumulatorIsFamily(CInt(proposedSummary.Rows(i)("ACCUM_ID"))) Then
                    db.AddInParameter(dbCommandWrapper, "@RELATION_ID", DbType.Int16, 0)
                Else
                    db.AddInParameter(dbCommandWrapper, "@RELATION_ID", DbType.Int16, proposedSummary.Rows(i)("RELATION_ID"))
                End If

                db.AddInParameter(dbCommandWrapper, "@M1", DbType.Decimal, proposedSummary.Rows(i)("M1"))
                db.AddInParameter(dbCommandWrapper, "@M2", DbType.Decimal, proposedSummary.Rows(i)("M2"))
                db.AddInParameter(dbCommandWrapper, "@M3", DbType.Decimal, proposedSummary.Rows(i)("M3"))
                db.AddInParameter(dbCommandWrapper, "@M4", DbType.Decimal, proposedSummary.Rows(i)("M4"))
                db.AddInParameter(dbCommandWrapper, "@M5", DbType.Decimal, proposedSummary.Rows(i)("M5"))
                db.AddInParameter(dbCommandWrapper, "@M6", DbType.Decimal, proposedSummary.Rows(i)("M6"))
                db.AddInParameter(dbCommandWrapper, "@M7", DbType.Decimal, proposedSummary.Rows(i)("M7"))
                db.AddInParameter(dbCommandWrapper, "@M8", DbType.Decimal, proposedSummary.Rows(i)("M8"))
                db.AddInParameter(dbCommandWrapper, "@M9", DbType.Decimal, proposedSummary.Rows(i)("M9"))
                db.AddInParameter(dbCommandWrapper, "@M10", DbType.Decimal, proposedSummary.Rows(i)("M10"))
                db.AddInParameter(dbCommandWrapper, "@M11", DbType.Decimal, proposedSummary.Rows(i)("M11"))
                db.AddInParameter(dbCommandWrapper, "@M12", DbType.Decimal, proposedSummary.Rows(i)("M12"))
                db.AddInParameter(dbCommandWrapper, "@DAYS_VALUE", DbType.Binary, BuildDaysByteArray(proposedSummary.Rows(i)))

                db.ExecuteNonQuery(dbCommandWrapper, transaction)
            Next
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Shared Sub CommitSummaries(ByVal proposedSummary As DataTable)
        If proposedSummary Is Nothing Then Return
        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Try

            For i As Integer = 0 To proposedSummary.Rows.Count - 1
                sqlCommand = "FDBMD.UPDATE_ACCUM_SUM"
                dbCommandWrapper = db.GetStoredProcCommand(sqlCommand)

                db.AddInParameter(dbCommandWrapper, "@ACCUM_ID", DbType.Int32, proposedSummary.Rows(i)("ACCUM_ID"))
                db.AddInParameter(dbCommandWrapper, "@ACCUM_YEAR", DbType.Int32, proposedSummary.Rows(i)("ACCUM_YEAR"))
                db.AddInParameter(dbCommandWrapper, "@FAMILY_ID", DbType.Int32, proposedSummary.Rows(i)("FAMILY_ID"))
                'is it family proposedSummary, pass 0 to memberid
                If AccumulatorController.GetAccumulatorIsFamily(CInt(proposedSummary.Rows(i)("ACCUM_ID"))) Then
                    db.AddInParameter(dbCommandWrapper, "@RELATION_ID", DbType.Int16, 0)
                Else
                    db.AddInParameter(dbCommandWrapper, "@RELATION_ID", DbType.Int16, proposedSummary.Rows(i)("RELATION_ID"))
                End If

                db.AddInParameter(dbCommandWrapper, "@M1", DbType.Decimal, proposedSummary.Rows(i)("M1"))
                db.AddInParameter(dbCommandWrapper, "@M2", DbType.Decimal, proposedSummary.Rows(i)("M2"))
                db.AddInParameter(dbCommandWrapper, "@M3", DbType.Decimal, proposedSummary.Rows(i)("M3"))
                db.AddInParameter(dbCommandWrapper, "@M4", DbType.Decimal, proposedSummary.Rows(i)("M4"))
                db.AddInParameter(dbCommandWrapper, "@M5", DbType.Decimal, proposedSummary.Rows(i)("M5"))
                db.AddInParameter(dbCommandWrapper, "@M6", DbType.Decimal, proposedSummary.Rows(i)("M6"))
                db.AddInParameter(dbCommandWrapper, "@M7", DbType.Decimal, proposedSummary.Rows(i)("M7"))
                db.AddInParameter(dbCommandWrapper, "@M8", DbType.Decimal, proposedSummary.Rows(i)("M8"))
                db.AddInParameter(dbCommandWrapper, "@M9", DbType.Decimal, proposedSummary.Rows(i)("M9"))
                db.AddInParameter(dbCommandWrapper, "@M10", DbType.Decimal, proposedSummary.Rows(i)("M10"))
                db.AddInParameter(dbCommandWrapper, "@M11", DbType.Decimal, proposedSummary.Rows(i)("M11"))
                db.AddInParameter(dbCommandWrapper, "@M12", DbType.Decimal, proposedSummary.Rows(i)("M12"))
                db.AddInParameter(dbCommandWrapper, "@DAYS_VALUE", DbType.Binary, BuildDaysByteArray(proposedSummary.Rows(i)))

                db.ExecuteNonQuery(dbCommandWrapper)
            Next
        Catch ex As Exception
            Throw
        End Try
    End Sub
    ' -----------------------------------------------------------------------------
    ' <summary>
    '
    ' </summary>
    ' <param name="relevantDate"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	5/3/2007	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function GetAccidentAccumulatorEntries(relationID As Integer, familyID As Integer) As DataTable
        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand
        Dim DS As DataSet

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Try
            SQLCommand = "FDBMD.RETRIEVE_MEM_ACCUM_ACCIDENT_ENTRIES"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)

            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            Return DS.Tables(0)

        Catch ex As Exception
            Throw
        End Try
    End Function
    ' -----------------------------------------------------------------------------
    ' <summary>
    '
    ' </summary>
    ' <param name="claimId"></param>
    ' <param name="relationId"></param>
    ' <param name="familyId"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/31/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function GetCommittedEntries(ByVal claimID As Integer, ByVal relationID As Integer, ByVal familyID As Integer) As DataTable
        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand
        Dim DS As DataSet
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Try
            SQLCommand = "FDBMD.RETRIEVE_MEM_ACCUM_ENTRIES_BY_CLAIM"
            dbCommandWrapper = db.GetStoredProcCommand(SQLCommand)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)

            DS = db.ExecuteDataSet(dbCommandWrapper)

            Return DS.Tables(0)

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetCommittedEntries(ByVal claimID As Integer, ByVal lineID As Integer, ByVal relationID As Integer, ByVal familyID As Integer) As DataTable
        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand
        Dim DS As DataSet
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Try
            SQLCommand = "FDBMD.RETRIEVE_MEM_ACCUM_ENTRIES_BY_LINE"
            dbCommandWrapper = db.GetStoredProcCommand(SQLCommand)

            db.AddInParameter(dbCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            db.AddInParameter(dbCommandWrapper, "@LINE_ID", DbType.Int16, lineID)
            db.AddInParameter(dbCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            db.AddInParameter(dbCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)

            DS = db.ExecuteDataSet(dbCommandWrapper)

            Return DS.Tables(0)

        Catch ex As Exception
            Throw
        End Try
    End Function
    ' -----------------------------------------------------------------------------
    ' <summary>
    '
    ' </summary>
    ' <param name="relationId"></param>
    ' <param name="familyId"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/31/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function GetCommittedEntries(ByVal relationID As Integer, ByVal familyID As Integer) As DataTable

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLsqlCommand As String = "FDBMD.RETRIEVE_MEM_ACCUM_ENTRIES"

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLsqlCommand)

            db.AddInParameter(dbCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            db.AddInParameter(dbCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)

            Dim ds As DataSet = db.ExecuteDataSet(dbCommandWrapper)

            Return ds.Tables(0)

        Catch ex As Exception
            Throw
        Finally
            db = Nothing
        End Try
    End Function

    ' -----------------------------------------------------------------------------
    ' <summary>
    '
    ' </summary>
    ' <param name="familyId"></param>
    ' <param name="relationId"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	3/29/2007	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function GetOverrideHistory(ByVal familyID As Integer, ByVal relationID As Integer) As DataTable
        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand
        Dim DS As DataSet
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Try
            SQLCommand = "FDBMD.RETRIEVE_MEM_ACCUM_OVERRIDES"
            dbCommandWrapper = db.GetStoredProcCommand(SQLCommand)

            db.AddInParameter(dbCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            db.AddInParameter(dbCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)

            DS = db.ExecuteDataSet(dbCommandWrapper)

            Return DS.Tables(0)

        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

#Region "Binary Helpers"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Builds the Binary data from the datarow
    ' </summary>
    ' <param name="row"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	12/15/2005	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Shared Function BuildDaysByteArray(ByVal summaryRow As DataRow) As Byte()
        Dim MS As New IO.MemoryStream
        Dim SR As New IO.BinaryWriter(MS)

        Try
            MS = New IO.MemoryStream
            SR = New IO.BinaryWriter(MS)

            'loop through all 366 (incase of leap year) days and add any data that is present
            For I As Integer = 1 To 366
                If summaryRow("D" & I) IsNot System.DBNull.Value Then
                    If Not CDbl(summaryRow("D" & I)) = 0D Then
                        SR.Write(I)
                        SR.Write(CDbl(summaryRow("D" & I)))
                    End If
                End If
            Next

            Return MS.ToArray()

        Catch ex As Exception
            Throw
        Finally

            SR.Close()
            MS.Close()

        End Try
    End Function
#End Region

#Region "Constructors"
    Private Sub New()

    End Sub
#End Region

End Class