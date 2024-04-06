Option Explicit On
Option Strict On

Imports System.Data.Common
Imports System.Threading

''' -----------------------------------------------------------------------------
''' Project	 : Accumulator
''' Class	 : CMS.MemberAccumulator
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' An class representing a member's Accumulator
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	12/9/2005	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public Class MemberAccumulator
    Implements ICloneable, IDisposable


#Region "Variables and Enums"
    Private Shared _TraceCloning As New TraceSwitch("TraceCloning", "Trace Switch in App.Config", "0")

    Private _RelationID As Int16
    Private _FamilyID As Integer

    Private _Disposed As Boolean = False
    Private _MemberAccumulatorType As MemberAccumulatorType
    Private _AccumulatorID As Integer = -1
    Private _AccumulatorName As String = ""
    Private _ValueType As MemberAccumulatorValueType = MemberAccumulatorValueType.NotSet
    Private _IsActive As Boolean = False

    Private _MemberAccumulatorEntriesDT As DataTable
    Private _CurrentSummaryDT As DataTable
    Private _ProposedSummaryDT As DataTable
    Private _MemberCommittedAccumulatorEntriesDT As DataTable
    Private _ClaimCommittedAccumulatorEntriesDT As DataTable
    Private _ClaimLineCommittedAccumulatorEntriesDT As DataTable

#End Region

#Region "Constructors"

    Public Sub New(ByVal accumulatorID As Integer, ByVal year As Integer, ByVal memberID As Int16, ByVal familyID As Integer)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Constructor
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/9/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        _RelationID = memberID
        _FamilyID = familyID
        _AccumulatorID = accumulatorID

        _CurrentSummaryDT = AccumulatorDAL.BuildSummaryColumns(_CurrentSummaryDT)
        _ProposedSummaryDT = AccumulatorDAL.BuildSummaryColumns(_ProposedSummaryDT)

        _ProposedSummaryDT.Rows.Add(_ProposedSummaryDT.NewRow)
        _CurrentSummaryDT.Rows.Add(_CurrentSummaryDT.NewRow)
        _CurrentSummaryDT.Rows(0)("ACCUM_ID") = accumulatorID
        _ProposedSummaryDT.Rows(0)("ACCUM_ID") = accumulatorID
        _CurrentSummaryDT.Rows(0)("RELATION_ID") = memberID
        _ProposedSummaryDT.Rows(0)("RELATION_ID") = memberID
        _CurrentSummaryDT.Rows(0)("FAMILY_ID") = familyID
        _ProposedSummaryDT.Rows(0)("FAMILY_ID") = familyID
        _CurrentSummaryDT.Rows(0)("ACCUM_YEAR") = year
        _ProposedSummaryDT.Rows(0)("ACCUM_YEAR") = year

        _MemberAccumulatorEntriesDT = CMSDALCommon.CreateAccumulatorValuesDT("MemberAccumulatorEntries")

        '        _MemberAccumulatorEntriesDT.Columns.Add("ACCUM_ID", System.Type.GetType("System.Int32"))
        '        _MemberAccumulatorEntriesDT.Columns.Add("CLAIM_ID", System.Type.GetType("System.Int32"))
        '        _MemberAccumulatorEntriesDT.Columns.Add("LINE_NBR", System.Type.GetType("System.Int16"))
        '        _MemberAccumulatorEntriesDT.Columns.Add("ORG_ACCIDENT_CLAIM_ID", System.Type.GetType("System.Int32"))
        '        _MemberAccumulatorEntriesDT.Columns.Add("RELATION_ID", System.Type.GetType("System.Int16"))
        '        _MemberAccumulatorEntriesDT.Columns.Add("FAMILY_ID", System.Type.GetType("System.Int32"))
        '        _MemberAccumulatorEntriesDT.Columns.Add("APPLY_DATE", System.Type.GetType("System.DateTime"))
        '        _MemberAccumulatorEntriesDT.Columns.Add("ENTRY_VALUE", System.Type.GetType("System.Decimal"))
        '        _MemberAccumulatorEntriesDT.Columns.Add("OVERRIDE_SW", System.Type.GetType("System.Decimal"))
        '        _MemberAccumulatorEntriesDT.Columns.Add("CREATE_DATE", System.Type.GetType("System.DateTime"))
        '        _MemberAccumulatorEntriesDT.Columns.Add("CREATE_USERID", System.Type.GetType("System.String"))

    End Sub

    Public Sub New(ByVal accumulatorID As Integer, ByVal relationID As Int16, ByVal familyID As Integer)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Constructor
        ' </summary>
        ' <param name="memberId"></param>
        ' <param name="familyId"></param>
        ' <param name="accumulatorId"></param>
        ' <param name="accumulatorSummary"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/28/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        _AccumulatorID = accumulatorID
        _CurrentSummaryDT = AccumulatorDAL.BuildSummaryColumns(_CurrentSummaryDT)
        _ProposedSummaryDT = AccumulatorDAL.BuildSummaryColumns(_ProposedSummaryDT)

        _MemberAccumulatorEntriesDT = CMSDALCommon.CreateAccumulatorValuesDT("MemberAccumulatorEntries")

        '        _MemberAccumulatorEntriesDT = New DataTable("MemberAccumulatorEntries")
        '        _MemberAccumulatorEntriesDT.Columns.Add("ACCUM_ID", System.Type.GetType("System.Int32"))
        '        _MemberAccumulatorEntriesDT.Columns.Add("CLAIM_ID", System.Type.GetType("System.Int32"))
        '        _MemberAccumulatorEntriesDT.Columns.Add("LINE_NBR", System.Type.GetType("System.Int16"))
        '        _MemberAccumulatorEntriesDT.Columns.Add("ORG_ACCIDENT_CLAIM_ID", System.Type.GetType("System.Int32"))
        '        _MemberAccumulatorEntriesDT.Columns.Add("RELATION_ID", System.Type.GetType("System.Int16"))
        '        _MemberAccumulatorEntriesDT.Columns.Add("FAMILY_ID", System.Type.GetType("System.Int32"))
        '        _MemberAccumulatorEntriesDT.Columns.Add("APPLY_DATE", System.Type.GetType("System.DateTime"))
        '        _MemberAccumulatorEntriesDT.Columns.Add("ENTRY_VALUE", System.Type.GetType("System.Decimal"))
        '        _MemberAccumulatorEntriesDT.Columns.Add("OVERRIDE_SW", System.Type.GetType("System.Boolean"))
        '        _MemberAccumulatorEntriesDT.Columns.Add("CREATE_DATE", System.Type.GetType("System.DateTime"))
        '        _MemberAccumulatorEntriesDT.Columns.Add("CREATE_USERID", System.Type.GetType("System.String"))

        _FamilyID = familyID
        _RelationID = relationID

    End Sub
#End Region

#Region "Functions"

    Public Sub Commit(Optional forceUpdate As Boolean = False)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Commits the Accumulator to the Database
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/15/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        ' if anything is needing to be commited
        Try

            If IsDirty OrElse forceUpdate Then
                MemberAccumulatorEntryDAL.CommitAll(_MemberAccumulatorEntriesDT, _ProposedSummaryDT)
                _MemberAccumulatorEntriesDT.Rows.Clear()
                'set the proposed to be the current and make not-dirty
                _CurrentSummaryDT = _ProposedSummaryDT
                _CurrentSummaryDT.AcceptChanges()
                _ProposedSummaryDT.AcceptChanges()
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub Commit(ByVal transaction As DbTransaction)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Commits all entries within a transaction
        ' </summary>
        ' <param name="transaction"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        ' if anything is needing to be commited
        Try
            If IsDirty Then
                MemberAccumulatorEntryDAL.CommitAll(_MemberAccumulatorEntriesDT, _ProposedSummaryDT, transaction)

                _MemberAccumulatorEntriesDT.Rows.Clear()
                'set the proposed to be the current and make not-dirty
                _CurrentSummaryDT = _ProposedSummaryDT

                _CurrentSummaryDT.AcceptChanges()
                _ProposedSummaryDT.AcceptChanges()
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
#End Region

#Region "Gets and Sets"
    Public Overridable Sub SetValue(ByVal dateOfValue As Date, ByVal amt As Decimal)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Sets the value of the accumulator for the given date
        ' </summary>
        ' <param name="dateOfValue"></param>
        ' <param name="value"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/15/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim NewDR As DataRow

        Try
            _ProposedSummaryDT.DefaultView.RowFilter = "ACCUM_YEAR = " & CInt(dateOfValue.Year)

            'if there is no data for that year, we need to create a row for it
            If _ProposedSummaryDT.DefaultView.Count = 0 Then
                NewDR = _ProposedSummaryDT.NewRow
                NewDR("ACCUM_YEAR") = dateOfValue.Year
                NewDR("ACCUM_ID") = _AccumulatorID
                NewDR("RELATION_ID") = _RelationID
                NewDR("FAMILY_ID") = _FamilyID
                _ProposedSummaryDT.Rows.Add(NewDR)
                _ProposedSummaryDT.DefaultView.RowFilter = "ACCUM_YEAR = " & CInt(dateOfValue.Year)
            End If
            'month
            If _ProposedSummaryDT.DefaultView(0)("M" & dateOfValue.Month.ToString) Is System.DBNull.Value Then
                _ProposedSummaryDT.DefaultView(0).Row("M" & dateOfValue.Month.ToString) = amt
            Else
                _ProposedSummaryDT.DefaultView(0).Row("M" & dateOfValue.Month.ToString) = amt + CDec(_ProposedSummaryDT.DefaultView(0)("M" & dateOfValue.Month.ToString))
            End If
            'day
            If _ProposedSummaryDT.DefaultView(0)("D" & dateOfValue.DayOfYear.ToString) Is System.DBNull.Value Then
                _ProposedSummaryDT.DefaultView(0).Row("D" & dateOfValue.DayOfYear.ToString) = amt
            Else
                _ProposedSummaryDT.DefaultView(0).Row("D" & dateOfValue.DayOfYear.ToString) = amt + CDec(_ProposedSummaryDT.DefaultView(0)("D" & dateOfValue.DayOfYear.ToString))
            End If
            _ProposedSummaryDT.DefaultView.RowFilter = ""
            _ProposedSummaryDT.AcceptChanges()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Friend Sub SetCurrentValue(ByVal dateOfValue As Date, ByVal amt As Decimal)

        Dim NewDR As DataRow

        Try
            _CurrentSummaryDT.DefaultView.RowFilter = "ACCUM_YEAR = " & CInt(dateOfValue.Year)
            'if there is no data for that year, we need to create a row for it
            If _CurrentSummaryDT.DefaultView.Count = 0 Then
                NewDR = _CurrentSummaryDT.NewRow
                NewDR("ACCUM_YEAR") = dateOfValue.Year
                NewDR("ACCUM_ID") = _AccumulatorID
                NewDR("RELATION_ID") = _RelationID
                NewDR("FAMILY_ID") = _FamilyID
                _CurrentSummaryDT.Rows.Add(NewDR)
                _CurrentSummaryDT.DefaultView.RowFilter = "ACCUM_YEAR = " & CInt(dateOfValue.Year)
            End If
            'month
            If _CurrentSummaryDT.DefaultView(0)("M" & dateOfValue.Month.ToString) Is System.DBNull.Value Then
                _CurrentSummaryDT.DefaultView(0).Row("M" & dateOfValue.Month.ToString) = amt
            Else
                _CurrentSummaryDT.DefaultView(0).Row("M" & dateOfValue.Month.ToString) = amt + CDec(_CurrentSummaryDT.DefaultView(0)("M" & dateOfValue.Month.ToString))
            End If
            'day
            If _CurrentSummaryDT.DefaultView(0)("D" & dateOfValue.DayOfYear.ToString) Is System.DBNull.Value Then
                _CurrentSummaryDT.DefaultView(0).Row("D" & dateOfValue.DayOfYear.ToString) = amt
            Else
                _CurrentSummaryDT.DefaultView(0).Row("D" & dateOfValue.DayOfYear.ToString) = amt + CDec(_CurrentSummaryDT.DefaultView(0)("D" & dateOfValue.DayOfYear.ToString))
            End If
            _CurrentSummaryDT.DefaultView.RowFilter = ""

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function GetProposedValue(ByVal dateType As DateType, ByVal dateTypeQuantity As Integer, ByVal dateTypeStart As Date?, ByVal dateDirection As DateDirection) As Decimal

        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets the proposed value
        ' </summary>
        ' <param name="dateType"></param>
        ' <param name="dateTypeQuantity"></param>
        ' <param name="dateTypeStart"></param>
        ' <param name="dateDirection"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/21/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            Return GetValue(_ProposedSummaryDT, dateType, dateTypeQuantity, dateTypeStart, dateDirection)

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Overridable Function GetOriginalValue(ByVal dateType As DateType, ByVal dateTypeQuantity As Integer, ByVal dateTypeStart As Date?, ByVal dateDirection As DateDirection) As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets the value that is currently in the database
        ' </summary>
        ' <param name="dateType"></param>
        ' <param name="dateTypeQuantity"></param>
        ' <param name="dateTypeStart"></param>
        ' <param name="dateDirection"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/21/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            Return GetValue(_CurrentSummaryDT, dateType, dateTypeQuantity, dateTypeStart, dateDirection)

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Function GetValue(ByVal summaryDT As DataTable, ByVal dateType As DateType, ByVal dateTypeQuantity As Integer, ByVal dateTypeStart As Date?, ByVal dateDirection As DateDirection) As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets value for time period specified and from datatable specified
        ' </summary>
        ' <param name="tbl"></param>
        ' <param name="dateType"></param>
        ' <param name="dateTypeQuantity"></param>
        ' <param name="dateTypeStart"></param>
        ' <param name="dateDirection"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/21/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim TmpArray As New ArrayList(dateTypeQuantity)
        Dim Val As Decimal

        Try
            Select Case dateType
                Case DateType.Days
                    TmpArray = GetDaysWithAmt(summaryDT, dateType, dateTypeQuantity, dateTypeStart, dateDirection)
                Case DateType.Months
                    TmpArray = GetMonthsValue(summaryDT, dateType, dateTypeQuantity, dateTypeStart, dateDirection)
                Case DateType.Quarters
                    TmpArray = GetQuartersValue(summaryDT, dateType, dateTypeQuantity, dateTypeStart, dateDirection)
                Case DateType.Weeks
                    TmpArray = GetWeeksValue(summaryDT, dateType, dateTypeQuantity, dateTypeStart, dateDirection)
                Case DateType.Years
                    If dateTypeQuantity > 0 AndAlso dateTypeQuantity < 1000 Then
                        'multi-year values are a sliding scale.
                        TmpArray = GetDaysWithAmt(summaryDT, DateType.Days, GetDaysFromYears(dateTypeQuantity, dateTypeStart, dateDirection), dateTypeStart, dateDirection)
                    ElseIf dateTypeQuantity = 1000 Then '1000 is the lifetime.
                        TmpArray = GetYearsValue(summaryDT, dateType, dateTypeQuantity, UFCWGeneral.NowDate, dateDirection) 'Note LifeTime values are always determined from todays date and not the DOS
                    Else
                        TmpArray = GetYearsValue(summaryDT, dateType, dateTypeQuantity, dateTypeStart, dateDirection)
                    End If
                Case DateType.Rollover
                    TmpArray = GetRolloverValue(summaryDT, dateType, dateTypeStart, dateDirection)
            End Select

            'tally up all the values
            Val += TmpArray.Cast(Of Decimal)().Sum()

            Return Val

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function GetDaysFromYears(ByVal years As Integer, ByVal dateStart As Date?, ByVal dateDirection As DateDirection) As Integer
        Dim Val As Integer
        Dim TmpDate As Date

        Try
            TmpDate = CDate(dateStart)

            For I As Integer = 0 To years
                If Date.IsLeapYear(TmpDate.Year) Then
                    Val += 366
                Else
                    Val += 365
                End If

                If dateDirection = DateDirection.Forward Then
                    TmpDate = TmpDate.AddYears(1)
                Else
                    TmpDate = TmpDate.AddYears(-1)
                End If
            Next

            Return Val

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function GetDaysWithAmt(ByVal summaryDT As DataTable, ByVal dateType As DateType, ByVal daysToSearch As Integer, ByVal zeroDate As Date?, ByVal dateDirection As DateDirection) As ArrayList
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets the Days' value
        ' </summary>
        ' <param name="summaryTable"></param>
        ' <param name="dateType"></param>
        ' <param name="dateTypeQuantity"></param>
        ' <param name="dateTypeStart"></param>
        ' <param name="dateDirection"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	2/14/2006	Created - Refactored from GetValue
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim TmpAL As New ArrayList(daysToSearch)
        Dim TmpDate As Date
        Dim JulianDay As String = ""
        Dim SearchYear As String = ""
        Dim DRs() As DataRow

        Try
            TmpDate = CDate(zeroDate)

            If daysToSearch = 0 Then
                SearchYear = "ACCUM_YEAR = " & TmpDate.Year.ToString
                JulianDay = "D" & TmpDate.DayOfYear.ToString
                DRs = summaryDT.Select(SearchYear)
                'apply the filter to the rows that meet the criteria
                For Each DR As DataRow In DRs
                    If DR(JulianDay) IsNot System.DBNull.Value Then TmpAL.Add(CDec(DR(JulianDay)))
                Next
            Else
                For I As Integer = 0 To daysToSearch

                    If dateDirection = DateDirection.Forward Then
                        TmpDate = TmpDate.AddDays(1)
                        'we need to cover the day they selected also
                        If I = 0 Then TmpDate = TmpDate.AddDays(-1)
                    Else
                        TmpDate = TmpDate.AddDays(-1)
                        'we need to cover the day they selected also
                        If I = 0 Then TmpDate = TmpDate.AddDays(1)
                    End If

                    SearchYear = "ACCUM_YEAR = " & TmpDate.Year.ToString
                    JulianDay = "D" & TmpDate.DayOfYear.ToString

                    DRs = summaryDT.Select(SearchYear)

                    'apply the filter to the rows that meet the criteria
                    For Each DR As DataRow In DRs
                        If DR(JulianDay) IsNot System.DBNull.Value Then TmpAL.Add(CDec(DR(JulianDay))) 'why get zeroes ?
                    Next

                Next
            End If

            Return TmpAL

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function GetMonthsValue(ByVal summaryDT As DataTable, ByVal dateType As DateType, ByVal dateTypeQuantity As Integer, ByVal dateTypeStart As Date?, ByVal dateDirection As DateDirection) As ArrayList
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets the Months' value
        ' </summary>
        ' <param name="summaryTable"></param>
        ' <param name="dateType"></param>
        ' <param name="dateTypeQuantity"></param>
        ' <param name="dateTypeStart"></param>
        ' <param name="dateDirection"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	2/14/2006	Created - Refactored from GetValue
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim TmpAL As New ArrayList(dateTypeQuantity)
        Dim TmpDate As Date
        Dim Filter As String = ""
        Dim Criteria As String = ""
        Dim DRs() As DataRow

        Try
            TmpDate = CDate(dateTypeStart)

            If dateTypeQuantity = 0 Then
                Criteria = "ACCUM_YEAR = " & TmpDate.Year.ToString
                Filter = "M" & TmpDate.Month.ToString
                DRs = summaryDT.Select(Criteria)
                'apply the filter to the rows that meet the criteria
                For Each DR As DataRow In DRs
                    If DR(Filter) IsNot System.DBNull.Value Then TmpAL.Add(CDec(DR(Filter)))
                Next
            Else
                For I As Integer = 0 To dateTypeQuantity
                    If dateDirection = DateDirection.Forward Then
                        TmpDate = TmpDate.AddMonths(1)
                        'we need to cover the month they selected also
                        If I = 0 Then TmpDate = TmpDate.AddMonths(-1)
                    Else
                        TmpDate = TmpDate.AddMonths(-1)
                        'we need to cover the month they selected also
                        If I = 0 Then TmpDate = TmpDate.AddMonths(1)
                    End If
                    Criteria = "ACCUM_YEAR = " & TmpDate.Year.ToString
                    Filter = "M" & TmpDate.Month.ToString
                    DRs = summaryDT.Select(Criteria)
                    'apply the filter to the rows that meet the criteria
                    For Each DR As DataRow In DRs
                        If DR(Filter) IsNot System.DBNull.Value Then TmpAL.Add(CDec(DR(Filter)))
                    Next
                Next
            End If

            Return TmpAL

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function GetQuartersValue(ByVal summaryDT As DataTable, ByVal dateType As DateType, ByVal dateTypeQuantity As Integer, ByVal dateTypeStart As Date?, ByVal dateDirection As DateDirection) As ArrayList
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets the Quarters' Value
        ' </summary>
        ' <param name="summaryTable"></param>
        ' <param name="dateType"></param>
        ' <param name="dateTypeQuantity"></param>
        ' <param name="dateTypeStart"></param>
        ' <param name="dateDirection"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	2/14/2006	Created - Refactored from GetValue
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim TmpAL As New ArrayList(dateTypeQuantity)
        Dim TmpDate As Date
        Dim Filter As String = ""
        Dim Criteria As String = ""
        Dim DRs() As DataRow

        Try
            TmpDate = CDate(dateTypeStart)

            If dateTypeQuantity = 0 Then
                Criteria = "ACCUM_YEAR = " & TmpDate.Year.ToString
                Select Case TmpDate.Month
                    Case 1, 2, 3 : Filter = "Q1"
                    Case 4, 5, 6 : Filter = "Q2"
                    Case 7, 8, 9 : Filter = "Q3"
                    Case 10, 11, 12 : Filter = "Q4"
                End Select
                DRs = summaryDT.Select(Criteria)
                'apply the filter to the rows that meet the criteria
                For Each DR As DataRow In DRs
                    If DR(Filter) IsNot System.DBNull.Value Then TmpAL.Add(CDec(DR(Filter)))
                Next
            Else
                For I As Integer = 0 To dateTypeQuantity
                    If dateDirection = DateDirection.Forward Then
                        TmpDate = TmpDate.AddMonths(3)
                        'we need to cover the month they selected also
                        If I = 0 Then TmpDate = TmpDate.AddMonths(-3)
                    Else
                        TmpDate = TmpDate.AddMonths(-3)
                        'we need to cover the month they selected also
                        If I = 0 Then TmpDate = TmpDate.AddMonths(3)
                    End If
                    Criteria = "ACCUM_YEAR = " & TmpDate.Year.ToString
                    Select Case TmpDate.Month
                        Case 1, 2, 3 : Filter = "Q1"
                        Case 4, 5, 6 : Filter = "Q2"
                        Case 7, 8, 9 : Filter = "Q3"
                        Case 10, 11, 12 : Filter = "Q4"
                    End Select

                    DRs = summaryDT.Select(Criteria)
                    'apply the filter to the rows that meet the criteria
                    For Each DR As DataRow In DRs
                        If DR(Filter) IsNot System.DBNull.Value Then TmpAL.Add(CDec(DR(Filter)))
                    Next
                Next
            End If

            Return TmpAL

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function GetRolloverValue(ByVal summaryDT As DataTable, ByVal dateType As DateType, ByVal dateTypeStart As Date?, ByVal dateDirection As DateDirection) As ArrayList

        Dim TmpArray As ArrayList
        Dim TmpDate As Date
        Dim Amt As Decimal = 0D

        Try
            TmpDate = CDate(dateTypeStart)
            TmpArray = GetQuartersValue(summaryDT, DateType.Quarters, 0, New Date(TmpDate.Year - 1, 12, 1), dateDirection)

            For Each TmpAmt As Decimal In TmpArray
                Amt += TmpAmt
            Next

            TmpArray = GetYearsValue(summaryDT, dateType, 0, dateTypeStart, dateDirection)

            For Each TmpAmt As Decimal In TmpArray
                Amt += TmpAmt
            Next

            TmpArray.Clear()
            TmpArray.Add(Amt)

            Return TmpArray

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Function GetYearsValue(ByVal summaryDT As DataTable, ByVal dateType As DateType, ByVal dateTypeQuantity As Integer, ByVal dateTypeStart As Date?, ByVal dateDirection As DateDirection) As ArrayList
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Get the Years Values.
        ' </summary>
        ' <param name="summaryTable"></param>
        ' <param name="dateType"></param>
        ' <param name="dateTypeQuantity"></param>
        ' <param name="dateTypeStart"></param>
        ' <param name="dateDirection"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	2/14/2006	Created -  Refactored from GetValue
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim TmpAL As New ArrayList(dateTypeQuantity)
        Dim TmpDate As Date
        Dim Filter As String = ""
        Dim Criteria As String = ""
        Dim DRs() As DataRow

        Try
            TmpDate = CDate(dateTypeStart)

            If summaryDT Is Nothing Then Return TmpAL

            If dateTypeQuantity = 0 Then
                Criteria = "ACCUM_YEAR = " & TmpDate.Year.ToString
                Filter = "TOTAL_VALUE"
                DRs = summaryDT.Select(Criteria)
                'apply the filter to the rows that meet the criteria
                For Each DR As DataRow In DRs
                    If DR(Filter) IsNot System.DBNull.Value Then TmpAL.Add(CDec(DR(Filter)))
                Next
            Else
                For I As Integer = 0 To dateTypeQuantity
                    If dateDirection = DateDirection.Forward Then
                        TmpDate = TmpDate.AddYears(1)
                        'we need to cover the year they selected also
                        If I = 0 Then TmpDate = TmpDate.AddYears(-1)
                    Else
                        TmpDate = TmpDate.AddYears(-1)
                        'we need to cover the year they selected also
                        If I = 0 Then TmpDate = TmpDate.AddYears(1)
                    End If

                    Criteria = "ACCUM_YEAR = " & TmpDate.Year.ToString
                    Filter = "TOTAL_VALUE"
                    DRs = summaryDT.Select(Criteria)

                    'apply the filter to the rows that meet the criteria
                    For Each DR As DataRow In DRs
                        If DR(Filter) IsNot System.DBNull.Value Then TmpAL.Add(CDec(DR(Filter)))
                    Next
                Next
            End If

            Return TmpAL

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function GetWeeksValue(ByVal summaryTable As DataTable, ByVal dateType As DateType, ByVal dateTypeQuantity As Integer, ByVal dateTypeStart As Date?, ByVal dateDirection As DateDirection) As ArrayList
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets the Weeks Values
        ' </summary>
        ' <param name="summaryTable"></param>
        ' <param name="dateType"></param>
        ' <param name="dateTypeQuantity"></param>
        ' <param name="dateTypeStart"></param>
        ' <param name="dateDirection"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	2/14/2006	Created - Refactored from GetValue
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim TmpAL As New ArrayList(dateTypeQuantity)
        Dim TmpDate As Date
        Dim Filter As String = ""
        Dim Criteria As String = ""
        Dim DRs() As DataRow

        Try
            TmpDate = CDate(dateTypeStart)

            If dateTypeQuantity = 0 Then
                For I As Integer = 0 To 7
                    If dateDirection = DateDirection.Forward Then
                        TmpDate = TmpDate.AddDays(1)
                        'we need to cover the day they selected also
                        If I = 0 Then TmpDate = TmpDate.AddDays(-1)
                    Else
                        TmpDate = TmpDate.AddDays(-1)
                        'we need to cover the day they selected also
                        If I = 0 Then TmpDate = TmpDate.AddDays(1)
                    End If
                    Criteria = "ACCUM_YEAR = " & TmpDate.Year.ToString
                    Filter = "D" & TmpDate.DayOfYear.ToString
                    DRs = summaryTable.Select(Criteria)
                    'apply the filter to the rows that meet the criteria
                    For Each DR As DataRow In DRs
                        If DR(Filter) IsNot System.DBNull.Value Then TmpAL.Add(CDec(DR(Filter)))
                    Next
                Next
            Else
                For I As Integer = 0 To (dateTypeQuantity + 1) * 7
                    If dateDirection = DateDirection.Forward Then
                        TmpDate = TmpDate.AddDays(1)
                        'we need to cover the day they selected also
                        If I = 0 Then TmpDate = TmpDate.AddDays(-1)
                    Else
                        TmpDate = TmpDate.AddDays(-1)
                        'we need to cover the day they selected also
                        If I = 0 Then TmpDate = TmpDate.AddDays(1)
                    End If

                    Criteria = "ACCUM_YEAR = " & TmpDate.Year.ToString
                    Filter = "D" & TmpDate.DayOfYear.ToString
                    DRs = summaryTable.Select(Criteria)

                    'apply the filter to the rows that meet the criteria
                    For Each DR As DataRow In DRs
                        If DR(Filter) IsNot System.DBNull.Value Then TmpAL.Add(CDec(DR(Filter)))
                    Next
                Next
            End If

            Return TmpAL

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Sub RemoveEntry(ByVal lineNumber As Short)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Removes an Entry from the Entries table and updates the summary
        ' </summary>
        ' <param name="lineNumber"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/17/2006	Created at Request of Nick.  VL Does not agree with this decision
        '                         but is implementing it at the request of UFCW management.
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim ApplyDate As Date
        Dim Val As Decimal
        Dim DRs() As DataRow

        Try

            DRs = _MemberAccumulatorEntriesDT.Select("LINE_NBR = " & lineNumber)

            For J As Integer = 0 To DRs.Length - 1
                ApplyDate = CDate(DRs(J)("APPLY_DATE"))
                Val = CDec(DRs(J)("ENTRY_VALUE"))
                _ProposedSummaryDT.DefaultView.RowFilter = "ACCUM_YEAR = " & ApplyDate.Year.ToString

                For I As Integer = 0 To _ProposedSummaryDT.DefaultView.Count - 1
                    If _ProposedSummaryDT.DefaultView(I)("D" & ApplyDate.DayOfYear.ToString) Is System.DBNull.Value Then
                        _ProposedSummaryDT.DefaultView(I).Row("D" & ApplyDate.DayOfYear.ToString) = 0
                    Else
                        _ProposedSummaryDT.DefaultView(I).Row("D" & ApplyDate.DayOfYear.ToString) = CDec(_ProposedSummaryDT.DefaultView(I)("D" & ApplyDate.DayOfYear.ToString)) - Val
                    End If
                    If _ProposedSummaryDT.DefaultView(I).Row("M" & ApplyDate.Month.ToString) Is System.DBNull.Value Then
                        _ProposedSummaryDT.DefaultView(I).Row("M" & ApplyDate.Month.ToString) = 0
                    Else
                        _ProposedSummaryDT.DefaultView(I).Row("M" & ApplyDate.Month.ToString) = CDec(_ProposedSummaryDT.DefaultView(I)("M" & ApplyDate.Month.ToString)) - Val
                    End If
                Next
                _ProposedSummaryDT.DefaultView.RowFilter = ""
                _MemberAccumulatorEntriesDT.Rows.Remove(DRs(J))
                _ProposedSummaryDT.DefaultView.RowFilter = ""
            Next
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Sub OverrideEntry(ByVal claimID As Integer, ByVal lineNumber As Short, ByVal applyDate As Date, ByVal val As Decimal, ByVal isOverride As Boolean, ByVal userID As String)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Overrides and Entry for the Accumulator
        ' </summary>
        ' <param name="claimId"></param>
        ' <param name="lineNumber"></param>
        ' <param name="applyDate"></param>
        ' <param name="value"></param>
        ' <param name="isOverride"></param>
        ' <param name="userId"></param>
        ' <remarks>
        ' This sub will remove all entries (including previous overrides that use the other method (insert entry)) and
        '  then it will insert the new value.  This is a complete override.  there is a loss of grnaual auditing
        '  using this method.  For example, you dont know what steps led them the place you end up after this point, from the
        '  accumulators standpoint.
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/17/2006	Created at Request of Nick.  VL Does not agree with this decision
        '                         but is implementing it at the request of UFCW management.
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            RemoveEntry(lineNumber)
            InsertEntry(claimID, lineNumber, applyDate, val, True, userID)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub InsertEntry(ByVal claimID As Integer, ByVal lineNumber As Integer, ByVal applyDate As Date, ByVal val As Decimal, ByVal isOverride As Boolean, ByVal userID As String)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Inserts and entry in the transaction datatable - _memberAccumulatorEntries
        ' </summary>
        ' <param name="claimId"></param>
        ' <param name="lineNumber"></param>
        ' <param name="applyDate"></param>
        ' <param name="value"></param>
        ' <param name="isOverride"></param>
        ' <param name="userId"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/21/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        'always remove any previous matching entries from the list
        Dim OverWrite As Boolean = False
        Dim ValueToOverWrite As Decimal
        Dim YearToOverWrite As Integer
        Dim MemberAccumDR As DataRow
        Dim NewDR As DataRow

        Try

            OverWrite = OverWriteEntry(claimID, lineNumber, applyDate, val, isOverride)
            If OverWrite Then

                For I As Integer = 0 To _MemberAccumulatorEntriesDT.Rows.Count - 1

                    If CInt(_MemberAccumulatorEntriesDT.Rows(I)("CLAIM_ID")) = claimID AndAlso CInt(_MemberAccumulatorEntriesDT.Rows(I)("LINE_NBR")) = lineNumber Then
                        ValueToOverWrite = CDec(_MemberAccumulatorEntriesDT.Rows(I)("ENTRY_VALUE"))
                        YearToOverWrite = CDate(_MemberAccumulatorEntriesDT.Rows(I)("APPLY_DATE")).Year
                        _MemberAccumulatorEntriesDT.Rows.RemoveAt(I)
                        _ProposedSummaryDT.DefaultView.RowFilter = "ACCUM_YEAR = " & YearToOverWrite.ToString

                        For j As Integer = 0 To _ProposedSummaryDT.DefaultView.Count - 1
                            _ProposedSummaryDT.DefaultView(j).Row("D" & applyDate.DayOfYear.ToString) = CDec(_ProposedSummaryDT.DefaultView(j)("D" & applyDate.DayOfYear.ToString)) - ValueToOverWrite
                            _ProposedSummaryDT.DefaultView(j).Row("M" & applyDate.Month.ToString) = CDec(_ProposedSummaryDT.DefaultView(j)("M" & applyDate.Month.ToString)) - ValueToOverWrite
                        Next

                        _ProposedSummaryDT.DefaultView.RowFilter = ""
                        Exit For
                    End If
                Next
            End If

            MemberAccumDR = _MemberAccumulatorEntriesDT.NewRow : MemberAccumDR("ACCUM_ID") = AccumulatorID : MemberAccumDR("CLAIM_ID") = claimID : MemberAccumDR("LINE_NBR") = lineNumber : MemberAccumDR("RELATION_ID") = _RelationID : MemberAccumDR("FAMILY_ID") = _FamilyID : MemberAccumDR("APPLY_DATE") = applyDate : MemberAccumDR("ENTRY_VALUE") = val : MemberAccumDR("OVERRIDE_SW") = isOverride : MemberAccumDR("CREATE_DATE") = UFCWGeneral.NowDate : MemberAccumDR("CREATE_USERID") = userID
            _MemberAccumulatorEntriesDT.Rows.Add(MemberAccumDR)

            _ProposedSummaryDT.DefaultView.RowFilter = "ACCUM_YEAR = " & applyDate.Year.ToString
            'if there is no data for that year, we need to create a new row
            If _ProposedSummaryDT.DefaultView.Count = 0 Then
                NewDR = _ProposedSummaryDT.NewRow
                NewDR("ACCUM_YEAR") = applyDate.Year
                NewDR("RELATION_ID") = _RelationID
                NewDR("FAMILY_ID") = _FamilyID
                NewDR("ACCUM_ID") = _AccumulatorID
                _ProposedSummaryDT.Rows.Add(NewDR)
                _ProposedSummaryDT.DefaultView.RowFilter = "ACCUM_YEAR = " & applyDate.Year.ToString
            End If

            For I As Integer = 0 To _ProposedSummaryDT.DefaultView.Count - 1
                If _ProposedSummaryDT.DefaultView(I)("D" & applyDate.DayOfYear.ToString) Is System.DBNull.Value Then
                    _ProposedSummaryDT.DefaultView(I).Row("D" & applyDate.DayOfYear.ToString) = val
                Else
                    _ProposedSummaryDT.DefaultView(I).Row("D" & applyDate.DayOfYear.ToString) = val + CDec(_ProposedSummaryDT.DefaultView(I)("D" & applyDate.DayOfYear.ToString))
                End If
                If _ProposedSummaryDT.DefaultView(I).Row("M" & applyDate.Month.ToString) Is System.DBNull.Value Then
                    _ProposedSummaryDT.DefaultView(I).Row("M" & applyDate.Month.ToString) = val
                Else
                    _ProposedSummaryDT.DefaultView(I).Row("M" & applyDate.Month.ToString) = val + CDec(_ProposedSummaryDT.DefaultView(I)("M" & applyDate.Month.ToString))
                End If
            Next
            _ProposedSummaryDT.DefaultView.RowFilter = ""

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Sub InsertEntry(ByVal claimID As Integer, ByVal lineNumber As Integer, ByVal originalClaimIdForAccident As Integer, ByVal applyDate As Date, ByVal value As Decimal, ByVal isOverride As Boolean, ByVal userID As String)
        'always remove any previous matching entries from the list
        Dim OverWrite As Boolean = False
        Dim ValueToOverWrite As Decimal
        Dim YearToOverWrite As Integer
        Dim MemberAccumDR As DataRow
        Dim NewDR As DataRow

        Try

            OverWrite = OverWriteEntry(claimID, lineNumber, applyDate, value, isOverride)
            If OverWrite Then
                For I As Integer = 0 To _MemberAccumulatorEntriesDT.Rows.Count - 1
                    If CInt(_MemberAccumulatorEntriesDT.Rows(I)("CLAIM_ID")) = claimID AndAlso CInt(_MemberAccumulatorEntriesDT.Rows(I)("LINE_NBR")) = lineNumber Then
                        ValueToOverWrite = CDec(_MemberAccumulatorEntriesDT.Rows(I)("ENTRY_VALUE"))
                        YearToOverWrite = CDate(_MemberAccumulatorEntriesDT.Rows(I)("APPLY_DATE")).Year
                        _MemberAccumulatorEntriesDT.Rows.RemoveAt(I)
                        _ProposedSummaryDT.DefaultView.RowFilter = "ACCUM_YEAR = " & YearToOverWrite.ToString

                        For J As Integer = 0 To _ProposedSummaryDT.DefaultView.Count - 1
                            _ProposedSummaryDT.DefaultView(J).Row("D" & applyDate.DayOfYear.ToString) = CDec(_ProposedSummaryDT.DefaultView(J)("D" & applyDate.DayOfYear.ToString)) - ValueToOverWrite
                            _ProposedSummaryDT.DefaultView(J).Row("M" & applyDate.Month.ToString) = CDec(_ProposedSummaryDT.DefaultView(J)("M" & applyDate.Month.ToString)) - ValueToOverWrite
                        Next

                        _ProposedSummaryDT.DefaultView.RowFilter = ""
                        Exit For
                    End If
                Next
            End If

            MemberAccumDR = _MemberAccumulatorEntriesDT.NewRow : MemberAccumDR("ACCUM_ID") = AccumulatorID : MemberAccumDR("CLAIM_ID") = claimID : MemberAccumDR("LINE_NBR") = lineNumber : MemberAccumDR("RELATION_ID") = _RelationID : MemberAccumDR("FAMILY_ID") = _FamilyID : MemberAccumDR("APPLY_DATE") = applyDate : MemberAccumDR("ENTRY_VALUE") = value : MemberAccumDR("OVERRIDE_SW") = isOverride : MemberAccumDR("CREATE_DATE") = UFCWGeneral.NowDate : MemberAccumDR("CREATE_USERID") = userID : MemberAccumDR("ORG_ACCIDENT_CLAIM_ID") = originalClaimIdForAccident
            _MemberAccumulatorEntriesDT.Rows.Add(MemberAccumDR)

            _ProposedSummaryDT.DefaultView.RowFilter = "ACCUM_YEAR = " & applyDate.Year.ToString
            'if there is no data for that year, we need to create a new row
            If _ProposedSummaryDT.DefaultView.Count = 0 Then
                NewDR = _ProposedSummaryDT.NewRow
                NewDR("ACCUM_YEAR") = applyDate.Year
                NewDR("RELATION_ID") = _RelationID
                NewDR("FAMILY_ID") = _FamilyID
                NewDR("ACCUM_ID") = _AccumulatorID
                _ProposedSummaryDT.Rows.Add(NewDR)
                _ProposedSummaryDT.DefaultView.RowFilter = "ACCUM_YEAR = " & applyDate.Year.ToString
            End If

            For I As Integer = 0 To _ProposedSummaryDT.DefaultView.Count - 1
                If _ProposedSummaryDT.DefaultView(I)("D" & applyDate.DayOfYear.ToString) Is System.DBNull.Value Then
                    _ProposedSummaryDT.DefaultView(I).Row("D" & applyDate.DayOfYear.ToString) = value
                Else
                    _ProposedSummaryDT.DefaultView(I).Row("D" & applyDate.DayOfYear.ToString) = value + CDec(_ProposedSummaryDT.DefaultView(I)("D" & applyDate.DayOfYear.ToString))
                End If
                If _ProposedSummaryDT.DefaultView(I)("M" & applyDate.Month.ToString) Is System.DBNull.Value Then
                    _ProposedSummaryDT.DefaultView(I).Row("M" & applyDate.Month.ToString) = value
                Else
                    _ProposedSummaryDT.DefaultView(I).Row("M" & applyDate.Month.ToString) = value + CDec(_ProposedSummaryDT.DefaultView(I)("M" & applyDate.Month.ToString))
                End If
            Next
            _ProposedSummaryDT.DefaultView.RowFilter = ""

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub InsertEntry(ByVal applyDate As Date, ByVal value As Decimal, ByVal isOverride As Boolean, ByVal userID As String)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Inserts an entry in the transaction datatable - _memberAccumulatorEntries
        ' </summary>
        ' <param name="applyDate"></param>
        ' <param name="value"></param>
        ' <param name="isOverride"></param>
        ' <param name="userId"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/6/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim MemberAccumDR As DataRow
        Dim NewDR As DataRow

        Try

            _ProposedSummaryDT.DefaultView.RowFilter = "ACCUM_YEAR = " & applyDate.Year.ToString
            'if there is no data for that year, we need to create a new row
            If _ProposedSummaryDT.DefaultView.Count = 0 Then
                NewDR = _ProposedSummaryDT.NewRow
                NewDR("ACCUM_YEAR") = applyDate.Year
                NewDR("RELATION_ID") = _RelationID
                NewDR("FAMILY_ID") = _FamilyID
                NewDR("ACCUM_ID") = _AccumulatorID
                _ProposedSummaryDT.Rows.Add(NewDR)
                _ProposedSummaryDT.DefaultView.RowFilter = "ACCUM_YEAR = " & applyDate.Year.ToString
            End If

            For I As Integer = 0 To _ProposedSummaryDT.DefaultView.Count - 1
                If _ProposedSummaryDT.DefaultView(I)("D" & applyDate.DayOfYear.ToString) Is System.DBNull.Value Then
                    _ProposedSummaryDT.DefaultView(I).Row("D" & applyDate.DayOfYear.ToString) = value
                Else
                    _ProposedSummaryDT.DefaultView(I).Row("D" & applyDate.DayOfYear.ToString) = value + CDec(_ProposedSummaryDT.DefaultView(I)("D" & applyDate.DayOfYear.ToString))
                End If
                If _ProposedSummaryDT.DefaultView(I)("M" & applyDate.Month.ToString) Is System.DBNull.Value Then
                    _ProposedSummaryDT.DefaultView(I).Row("M" & applyDate.Month.ToString) = value
                Else
                    _ProposedSummaryDT.DefaultView(I).Row("M" & applyDate.Month.ToString) = value + CDec(_ProposedSummaryDT.DefaultView(I)("M" & applyDate.Month.ToString))
                End If
            Next
            _ProposedSummaryDT.DefaultView.RowFilter = ""

            MemberAccumDR = _MemberAccumulatorEntriesDT.NewRow : MemberAccumDR("ACCUM_ID") = AccumulatorID : MemberAccumDR("CLAIM_ID") = System.DBNull.Value : MemberAccumDR("LINE_NBR") = System.DBNull.Value : MemberAccumDR("RELATION_ID") = _RelationID : MemberAccumDR("FAMILY_ID") = _FamilyID : MemberAccumDR("APPLY_DATE") = applyDate : MemberAccumDR("ENTRY_VALUE") = value : MemberAccumDR("OVERRIDE_SW") = isOverride : MemberAccumDR("CREATE_DATE") = UFCWGeneral.NowDate : MemberAccumDR("CREATE_USERID") = userID
            _MemberAccumulatorEntriesDT.Rows.Add(MemberAccumDR)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function OverWriteEntry(ByVal claimID As Integer, ByVal lineNumber As Integer, ByVal applyDate As Date, ByVal value As Decimal, ByVal isOverride As Boolean) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' overwrites entries from the transaction datatable - _memberAccumulatorEntries
        ' </summary>
        ' <param name="claimId"></param>
        ' <param name="lineNumber"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/21/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            If isOverride Then Return False

            For I As Integer = 0 To _MemberAccumulatorEntriesDT.Rows.Count - 1
                If CInt(_MemberAccumulatorEntriesDT.Rows(I)("CLAIM_ID")) = claimID AndAlso CInt(_MemberAccumulatorEntriesDT.Rows(I)("LINE_NBR")) = lineNumber Then
                    Return True
                End If
            Next

            _ProposedSummaryDT.DefaultView.RowFilter = ""

        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region

#Region "Special Gets"
    Public ReadOnly Property ProposedLifetimeValue() As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets the lifetime value for this accumualtor
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Dim Val As Decimal = 0
            Try
                'add up all the Total values from all rows
                For Each DR As DataRow In _ProposedSummaryDT.Rows
                    Val += CDec(DR("TOTAL_VALUE"))
                Next

                Return Val

            Catch ex As Exception
                Throw
            End Try
        End Get
    End Property

    Public ReadOnly Property OriginalLifetimeValue() As Decimal
        Get
            Dim Amt As Decimal = 0D
            Try
                'add up all the Total values from all rows
                For Each DR As DataRow In _CurrentSummaryDT.Rows
                    Amt += CDec(DR("TOTAL_VALUE"))
                Next

                Return Amt

            Catch ex As Exception
                Throw
            End Try
        End Get
    End Property

    Public Function OriginalCurrentValueForCurrentYear() As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' gets the original current value for this accumulator
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/5/2006	Created
        '     [paulw] 11/21/2006  Altered to accomidate ACR MED-0083
        ' </history>
        ' -----------------------------------------------------------------------------
        Return OriginalYearValue(UFCWGeneral.NowDate.Year)
    End Function

    Public Function OriginalCurrentValueForCurrentYear(ByVal showOnlyMaxIfExceeded As Boolean) As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="showOnlyMaxIfExceeded"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	11/21/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim val As Decimal = OriginalYearValue(UFCWGeneral.NowDate.Year)
    End Function

    Public ReadOnly Property ProposedCurrentValueForCurrentYear() As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' gets the proposed current value for this accumulator
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/1/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return ProposedYearValue(UFCWGeneral.NowDate.Year)
        End Get
    End Property

    Public ReadOnly Property ProposedYearValue(ByVal year As Integer) As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets the proposed value for the specified year for this accumulator
        ' </summary>
        ' <param name="year"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Dim Val As Decimal = 0D
            'get all the rows from this year
            'should only be one row
            Try
                Dim Rows() As DataRow = _ProposedSummaryDT.Select("ACCUM_YEAR = " & year.ToString())
                'add up all the Total values from those rows
                For Each DR As DataRow In Rows
                    Val += CDec(DR("TOTAL_VALUE"))
                Next

                Return Val

            Catch ex As Exception
                Throw
            End Try
        End Get
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets the original value for the specified year for this accumulator
    ' </summary>
    ' <param name="year"></param>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	5/1/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public ReadOnly Property OriginalYearValue(ByVal year As Integer) As Decimal
        Get
            Dim Val As Decimal = 0D
            'get all the rows from this year
            'should only be one row
            Try
                Dim Rows() As DataRow = _CurrentSummaryDT.Select("ACCUM_YEAR = " & year.ToString())
                'add up all the Total values from those rows
                For Each DR As DataRow In Rows
                    Val += CDec(DR("TOTAL_VALUE"))
                Next

                Return Val

            Catch ex As Exception
                Throw
            End Try
        End Get
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' gets the proposed rollover value for this accumulator
    ' </summary>
    ' <param name="year"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/5/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public ReadOnly Property ProposedRolloverValue(ByVal year As Integer) As Decimal
        Get
            Return ProposedYearValue(year) + ProposedFourthQuarterValue(year - 1)
        End Get
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' gets the original rollover value for this accumulator
    ' </summary>
    ' <param name="year"></param>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	5/1/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public ReadOnly Property OriginalRolloverValue(ByVal year As Integer) As Decimal
        Get
            Return OriginalYearValue(year) + OriginalFourthQuarterValue(year - 1)
        End Get
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' gets the proposed 4th quarter value for this accumulator for the year specified
    ' </summary>
    ' <param name="year"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/5/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public ReadOnly Property ProposedFourthQuarterValue(ByVal year As Integer) As Decimal
        Get
            Dim val As Decimal = 0D
            'get all the rows from this year
            'should only be one row
            Try
                Dim Rows() As DataRow = _ProposedSummaryDT.Select("ACCUM_YEAR = " & year.ToString())
                'add up all the Total values from those rows
                For Each DR As DataRow In Rows
                    val += CDec(DR("Q4"))
                Next

                Return val

            Catch ex As Exception
                Throw
            End Try

        End Get
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets the original value for the 4th quarter
    ' </summary>
    ' <param name="year"></param>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	5/1/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public ReadOnly Property OriginalFourthQuarterValue(ByVal year As Integer) As Decimal
        Get
            Dim Val As Decimal = 0D
            'get all the rows from this year
            'should only be one row
            Try
                Dim Rows() As DataRow = _CurrentSummaryDT.Select("ACCUM_YEAR = " & year.ToString())
                'add up all the Total values from those rows
                For Each DR As DataRow In Rows
                    Val += CDec(DR("Q4"))
                Next

                Return Val

            Catch ex As Exception
                Throw
            End Try
        End Get
    End Property

    Public Function GetIncurredDate() As Date
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' gets the first date that any data exists for this accumulator
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Return GetIncurredDate(New Date(1900, 1, 1))
    End Function
    Public Function GetIncurredDate(ByVal fromDate As Date) As Date

        ' -----------------------------------------------------------------------------
        ' <summary>
        ' gets the first date that any data exists, from the date specified, for this accumulator
        ' </summary>
        ' <param name="fromDate"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim TmpDate As Date

        Try

            _ProposedSummaryDT.DefaultView.RowFilter = "ACCUM_YEAR >= " & fromDate.Year.ToString()
            _ProposedSummaryDT.DefaultView.Sort = "ACCUM_YEAR ASC"

            'add up all the Total values from those rows
            For I As Integer = 0 To _ProposedSummaryDT.DefaultView.Count - 1
                TmpDate = New Date(CInt(_ProposedSummaryDT.DefaultView(I)("ACCUM_YEAR")), 1, 1)
                For J As Integer = 1 To 366
                    If Not CDec(_ProposedSummaryDT.DefaultView(I)("D" & J)) = 0 Then
                        If fromDate <= TmpDate.AddDays(J) Then
                            Return TmpDate.AddDays(J)
                        End If
                    End If
                Next
            Next

        Catch ex As Exception
            Throw
        End Try

    End Function
#End Region

#Region "Public Properties"
    Public Property CommittedEntries(ByVal claimID As Integer, ByVal lineNumber As Integer) As DataTable
        Get
            Try

                _ClaimLineCommittedAccumulatorEntriesDT = MemberAccumulatorEntryDAL.GetCommittedEntries(claimID, lineNumber, _RelationID, _FamilyID)
                Return _ClaimLineCommittedAccumulatorEntriesDT

            Catch ex As Exception
                Throw
            End Try
        End Get
        Set(value As DataTable)
            _ClaimLineCommittedAccumulatorEntriesDT = value
        End Set
    End Property

    Public Property CommittedEntries(ByVal claimID As Integer) As DataTable
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/31/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Try
                _ClaimCommittedAccumulatorEntriesDT = MemberAccumulatorEntryDAL.GetCommittedEntries(claimID, _RelationID, _FamilyID)
                Return _ClaimCommittedAccumulatorEntriesDT

            Catch ex As Exception
                Throw
            End Try
        End Get
        Set(value As DataTable)
            _ClaimCommittedAccumulatorEntriesDT = value
        End Set
    End Property

    Public Property CommittedEntries() As DataTable
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/31/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Try
                _MemberCommittedAccumulatorEntriesDT = MemberAccumulatorEntryDAL.GetCommittedEntries(_RelationID, _FamilyID)
                Return _MemberCommittedAccumulatorEntriesDT

            Catch ex As Exception
                Throw
            End Try
        End Get

        Set(value As DataTable)
            _MemberCommittedAccumulatorEntriesDT = value
        End Set
    End Property

    Public ReadOnly Property IsDirty() As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' If there has been any changes to the proposed summary, this returns true
        ' else it returns false
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/14/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Try

                Return _MemberAccumulatorEntriesDT.Rows.Count > 0

            Catch ex As Exception
                Throw
            End Try
        End Get
    End Property

    Public Property Name() As String
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' The name of the Accumualtor
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/12/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _AccumulatorName
        End Get
        Set(ByVal value As String)
            _AccumulatorName = value
        End Set
    End Property

    Public Property ValueType() As MemberAccumulatorValueType
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' The Value Type of the Accumulator
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/12/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _ValueType
        End Get
        Set(ByVal value As MemberAccumulatorValueType)
            _ValueType = value
        End Set
    End Property

    Public Property Type() As MemberAccumulatorType
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' The type of Accumulator
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/15/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _MemberAccumulatorType
        End Get
        Set(ByVal value As MemberAccumulatorType)
            _MemberAccumulatorType = value
        End Set
    End Property

    Public Property IsActive() As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Is the Accumulator active
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/12/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _IsActive
        End Get
        Set(ByVal value As Boolean)
            _IsActive = value
        End Set
    End Property

    Public Property AccumulatorID() As Integer
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' The database id of the Accumulator
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/12/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _AccumulatorID
        End Get
        Set(ByVal value As Integer)
            _AccumulatorID = value
        End Set
    End Property

    Public Property MemberAccumulatorEntries() As DataTable

        Get
            Return _MemberAccumulatorEntriesDT
        End Get
        Set(value As DataTable)
            _MemberAccumulatorEntriesDT = value
        End Set
    End Property

    Public Property CurrentSummary() As DataTable

        Get
            Return _CurrentSummaryDT
        End Get
        Set(value As DataTable)
            _CurrentSummaryDT = value
        End Set
    End Property

    Public Property ProposedSummary() As DataTable
        Get
            Return _ProposedSummaryDT
        End Get
        Set(value As DataTable)
            _ProposedSummaryDT = value
        End Set
    End Property

#End Region

#Region "Clone"
    Public Function DeepCopy() As MemberAccumulator

        Dim MemberAccumulatorClone As MemberAccumulator
        Dim BeginTime As Date = UFCWGeneral.NowDate

        Try

            MemberAccumulatorClone = Me.ShallowCopy()

            MemberAccumulatorClone._MemberAccumulatorEntriesDT = CloneHelper.DeepCopy(_MemberAccumulatorEntriesDT)
            MemberAccumulatorClone._CurrentSummaryDT = CloneHelper.DeepCopy(_CurrentSummaryDT)
            MemberAccumulatorClone._ProposedSummaryDT = CloneHelper.DeepCopy(_ProposedSummaryDT)
            MemberAccumulatorClone._MemberCommittedAccumulatorEntriesDT = CloneHelper.DeepCopy(_MemberCommittedAccumulatorEntriesDT)
            MemberAccumulatorClone._ClaimCommittedAccumulatorEntriesDT = CloneHelper.DeepCopy(_ClaimCommittedAccumulatorEntriesDT)
            MemberAccumulatorClone._ClaimLineCommittedAccumulatorEntriesDT = CloneHelper.DeepCopy(_ClaimLineCommittedAccumulatorEntriesDT)

            Return MemberAccumulatorClone

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
#End If
        End Try
    End Function

    Public Function ShallowCopy() As MemberAccumulator
        Dim MemberAccumulatorClone As MemberAccumulator
        Dim BeginTime As Date = UFCWGeneral.NowDate

        Try

            MemberAccumulatorClone = DirectCast(Me.MemberwiseClone(), MemberAccumulator)
            MemberAccumulatorClone._MemberAccumulatorEntriesDT = Nothing
            MemberAccumulatorClone._CurrentSummaryDT = Nothing
            MemberAccumulatorClone._ProposedSummaryDT = Nothing
            MemberAccumulatorClone._MemberCommittedAccumulatorEntriesDT = Nothing
            MemberAccumulatorClone._ClaimCommittedAccumulatorEntriesDT = Nothing
            MemberAccumulatorClone._ClaimLineCommittedAccumulatorEntriesDT = Nothing

            Return MemberAccumulatorClone

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
#End If
        End Try

    End Function

    Public Function Clone() As Object Implements ICloneable.Clone

        Try

            Return DirectCast(CloneHelper.Clone(Me), ICloneable)

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Function

#End Region

#Region "Clean Up"
    ' Instantiate a SafeHandle instance.

    ' Public implementation of Dispose pattern callable by consumers.
    Public Sub Dispose() _
               Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    ' Protected implementation of Dispose pattern.
    Protected Overridable Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.
            '
            If _MemberAccumulatorEntriesDT IsNot Nothing Then
                _MemberAccumulatorEntriesDT.Dispose()
            End If
            _MemberAccumulatorEntriesDT = Nothing

            If _CurrentSummaryDT IsNot Nothing Then
                _CurrentSummaryDT.Dispose()
            End If
            _CurrentSummaryDT = Nothing

            If _ProposedSummaryDT IsNot Nothing Then
                _ProposedSummaryDT.Dispose()
            End If
            _ProposedSummaryDT = Nothing

            If _MemberCommittedAccumulatorEntriesDT IsNot Nothing Then
                _MemberCommittedAccumulatorEntriesDT.Dispose()
            End If
            _MemberCommittedAccumulatorEntriesDT = Nothing

            If _ClaimCommittedAccumulatorEntriesDT IsNot Nothing Then
                _ClaimCommittedAccumulatorEntriesDT.Dispose()
            End If
            _ClaimCommittedAccumulatorEntriesDT = Nothing

            If _ClaimLineCommittedAccumulatorEntriesDT IsNot Nothing Then
                _ClaimLineCommittedAccumulatorEntriesDT.Dispose()
            End If
            _ClaimLineCommittedAccumulatorEntriesDT = Nothing

        End If

        _Disposed = True
    End Sub
#End Region

End Class
Public Enum DateType
    Days = 0
    Weeks = 1
    Months = 2
    Quarters = 3
    Years = 4
    Rollover = 5
End Enum

Public Enum DateDirection
    Forward = 0
    Reverse = 1
End Enum
Public Enum MemberAccumulatorValueType
    NotSet = -1
    Units = 0
    Currency = 1
End Enum
Public Enum MemberAccumulatorType
    NotSet = -1
    Annual = 0
    Lifetime = 1
    Rollover = 2
End Enum
