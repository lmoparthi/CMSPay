Option Explicit On
Option Strict On

Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data.Common


''' -----------------------------------------------------------------------------
''' Project	 : Accumulator
''' Class	 : CMS.MemberAccumulatorDAL
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' Manages Member Accumulators
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
'''     [paulw] 1/18/2006   Refactored code to move Commits to MemberAccumulatorEntryDAL
'''                         to accomidate transactions
''' </history>
''' -----------------------------------------------------------------------------
Friend NotInheritable Class MemberAccumulatorDAL

#Region "Binary Helper"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Moves Binary data from the db into a datarow format
    ' </summary>
    ' <param name="dataRow"></param>
    ' <param name="byteArray"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	12/15/2005	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Shared Sub MoveBinaryDayDataToDataTable(ByRef summaryRow As DataRow, ByVal byteArray() As Byte)
        Dim MS As New IO.MemoryStream(byteArray)
        Dim Sr As New IO.BinaryReader(MS)
        Dim Day As Integer
        Dim DayValue As Decimal
        Dim DaysInformationString As String

        Try

            'loop through the Binary data and massage it into the proper column of the datarow
            While (Sr.BaseStream.Position < Sr.BaseStream.Length)
                Day = Sr.ReadInt32()
                DayValue = CDec(Sr.ReadDouble)
                DaysInformationString += Day & "?" & DayValue & ","
                summaryRow("D" & CInt(Day)) = DayValue
            End While

            summaryRow("DAYS_INFORMATION") = DaysInformationString

        Catch ex As Exception
            Throw
        Finally
            If Sr IsNot Nothing Then
                Sr.Dispose()
            End If
            Sr = Nothing
            If MS IsNot Nothing Then
                MS.Dispose()
            End If
            MS = Nothing

        End Try
    End Sub
    Public Shared Function CVDMBF(ByVal strNumber As String) As Double

        Dim DblResult As Double

        Dim intExponent As Integer
        Dim intMantissa As Integer
        Dim intSign As Integer
        Dim intByte As Integer

        On Error GoTo CVDMBF_Error

        dblResult = 0
        If Len(strNumber) = 8 Then
            intExponent = Asc(Right$(strNumber, 1)) - 128
            intMantissa = Asc(Mid$(strNumber, 7, 1))
            intSign = intMantissa \ 128
            intMantissa = 128 + intMantissa Mod 128
            dblResult = intMantissa / 256
            For intByte = 6 To 1 Step -1
                dblResult = dblResult + Asc(Mid$(strNumber, intByte, 1)) / 256 ^ (8 - intByte)
            Next intByte
            dblResult = dblResult * 2 ^ intExponent
            If CBool(intSign) Then
                dblResult = -dblResult
            End If
        End If

        CVDMBF = dblResult
        On Error GoTo 0
        Exit Function

CVDMBF_Error:

        MsgBox("Error " & Err.Number & " (" & Err.Description & ")")

    End Function
#End Region

#Region "CRUD"

    Public Shared Function GetAccumulatorOOPSummaries(ByVal familyID As Integer, ByVal relationID As Short, ByVal actionYear As Integer) As DataSet
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets all OOP Accumulator Summaries from the db for the specified member.
        ' This function
        ' 1)  Retrieves the data from the db
        ' 2)  Builds the code schema for the datatables
        ' 3)  Loops through the returned data and massages it into
        '     the newly created datatables (referenced in 2 above) for
        '     both the Summary data and the Accumulator
        ' 4)  Accept all changes and add tables to the dataset that is to be
        '     returned.
        ' 5)  Return the dataset if it has any data, else return nothing
        ' </summary>
        ' <param name="memberId"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/15/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCommand As String
        Dim MemberAccumulatorDS As DataSet
        Dim DS As DataSet
        Dim Tablenames() As String = {"AccumulatorOOPSummary", "AccumulatorOOPDetail"}

        Try
            DB = CMSDALCommon.CreateDatabase()

            ' 1) - as referenced in  Function Comments
            'set up the local variables and stored procedure
            MemberAccumulatorDS = New DataSet

            SQLCommand = "FDBMD.RETRIEVE_ACCUM_OOP_SUM"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@ACTION_YEAR", DbType.Int32, actionYear)

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            ' 2) - as referenced in  Function Comments
            MemberAccumulatorDS = ExtractSummaryData(DS.Tables(0), MemberAccumulatorDS)
            MemberAccumulatorDS.Tables(0).TableName = "AccumulatorOOPSummary"

            If DS.Tables(1).Rows.Count > 0 Then
                MemberAccumulatorDS.Tables.Add("AccumulatorOOPDetail").Load(DS.Tables(1).CreateDataReader)
            End If

            ' 5) - as referenced in  Function Comments

            If MemberAccumulatorDS.Tables("AccumulatorOOPSummary").Rows.Count > 0 Then
                Return MemberAccumulatorDS
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        Finally
            DB = Nothing
        End Try
    End Function

    Private Shared Function ExtractSummaryData(dt As DataTable, memberAccumulatorDS As DataSet) As DataSet

        Dim AccumulatorDT As DataTable
        Dim SummaryDT As DataTable
        Dim NewDR As DataRow

        AccumulatorDT = AccumulatorDAL.BuildAccumulatorColumns()
        SummaryDT = AccumulatorDAL.BuildSummaryColumns(SummaryDT)

        AccumulatorDT.BeginLoadData()
        SummaryDT.BeginLoadData()

        ' 3) - as referenced in  Function Comments
        For Each DR As DataRow In dt.Rows
            NewDR = SummaryDT.NewRow
            'add all column data

            For Each DC As DataColumn In dt.Columns

                If DC.ColumnName.StartsWith("Q") OrElse DC.ColumnName.StartsWith("M") OrElse DC.ColumnName.ToUpper.StartsWith("ACCUM_YEAR") _
                   OrElse DC.ColumnName.ToUpper.StartsWith("FAMILY_ID") OrElse DC.ColumnName.ToUpper.StartsWith("RELATION_ID") OrElse DC.ColumnName.ToUpper.StartsWith("ACCUM_ID") Then
                    NewDR(DC.ColumnName) = DR(DC.ColumnName)
                ElseIf DC.ColumnName.ToUpper = "DAYS_VALUE" Then
                    'handle binary data
                    If DR("DAYS_VALUE") IsNot System.DBNull.Value Then MoveBinaryDayDataToDataTable(NewDR, CType(DR("DAYS_VALUE"), Byte()))
                End If

            Next

            'move the accumulator into the datatable
            If Not AccumulatorDT.Rows.Contains(CInt(DR("ACCUM_ID"))) Then
                AccumulatorDT.ImportRow(AccumulatorController.GetAccumulator(CInt(DR("ACCUM_ID"))).Rows(0))
            End If

            'move the newly created row into the new Datatable
            SummaryDT.Rows.Add(NewDR)
        Next

        AccumulatorDT.AcceptChanges()
        AccumulatorDT.EndLoadData()

        ' 4) - as referenced in  Function Comments
        'accept all changes so that the table is seen as unmodified
        SummaryDT.AcceptChanges()
        SummaryDT.EndLoadData()

        ' add Summary datatable and Accumulator datatable
        memberAccumulatorDS.Tables.Add(SummaryDT)
        memberAccumulatorDS.Tables.Add(AccumulatorDT)

        Return memberAccumulatorDS

    End Function

    Public Shared Function GetAccumulatorSummaries(ByVal memberID As Int16, ByVal familyID As Integer, Optional ByVal manualOnly As Boolean = False) As DataSet
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets alll Accumulator Summaires from the db for the specified member.
        ' This function
        ' 1)  Retrieves the data from the db
        ' 2)  Builds the code schema for the datatables
        ' 3)  Loops through the returned data and massages it into
        '     the newly created datatables (referenced in 2 above) for
        '     both the Summary data and the Accumulator
        ' 4)  Accept all changes and add tables to the dataset that is to be
        '     returned.
        ' 5)  Return the dataset if it has any data, else return nothing
        ' </summary>
        ' <param name="memberId"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/15/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCommand As String
        Dim MemberAccumulatorDS As DataSet
        Dim DS As DataSet

        Try
            DB = CMSDALCommon.CreateDatabase()

            ' 1) - as referenced in  Function Comments
            'set up the local variables and stored procedure
            MemberAccumulatorDS = New DataSet

            If manualOnly Then
                SQLCommand = "FDBMD.RETRIEVE_MANUAL_ACCUM_SUM"
            Else
                SQLCommand = "FDBMD.RETRIEVE_ACCUM_SUM"
            End If

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, memberID)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            MemberAccumulatorDS = ExtractSummaryData(DS.Tables(0), MemberAccumulatorDS)

            If MemberAccumulatorDS.Tables(0).Rows.Count > 0 Then
                MemberAccumulatorDS.Tables(0).TableName = "AccumulatorSummary"
                Return MemberAccumulatorDS
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        Finally
            DB = Nothing
        End Try
    End Function

#End Region
End Class