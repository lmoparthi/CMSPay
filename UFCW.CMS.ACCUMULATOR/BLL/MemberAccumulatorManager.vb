Option Explicit On
Option Strict On
Option Infer On

Imports System.Data.Common
Imports System.Data.DataTableExtensions

''' -----------------------------------------------------------------------------
''' Project	 : Accumulator
''' Class	 : CMS.MemberAccumulatorManager
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' Manages all Member Accumulators
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	12/9/2005	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public NotInheritable Class MemberAccumulatorManager
    Implements ICloneable

    '    Implements IDisposable, ICloneable

#Region "Private members and Enums"

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _MemberAccumulatorManagerGuid As System.Guid = Guid.NewGuid()
    Private _RelationID As Short
    Private _FamilyID As Integer
    Private _NeedsRefresh As Boolean = True

    Private _AccumulatorCollection As MemberAccumulatorCollection
    Private _Disposed As Boolean

#End Region

#Region "Properties"

    Private Property AccumulatorCollection() As MemberAccumulatorCollection
        Get
            Return _AccumulatorCollection
        End Get
        Set(value As MemberAccumulatorCollection)
            _AccumulatorCollection = value
        End Set
    End Property

    Public ReadOnly Property needsRefresh() As Boolean
        Get
            Return _NeedsRefresh
        End Get
    End Property

    Public ReadOnly Property AccumulatorSummaries() As MemberAccumulatorCollection
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets all the Accumultors for a member
        ' </summary>
        ' <param name="memberId">Member Id</param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/9/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Try
                'populate collection if there is nothing in it
                If _AccumulatorCollection.Count < 1 Then
                    PopulateAccumulatorSummariesForMember()
                End If

                Return _AccumulatorCollection

            Catch ex As Exception
                Throw
            End Try
        End Get
    End Property

    Public ReadOnly Property RelationID() As Int16
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' returns the memberid
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/6/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _RelationID
        End Get
    End Property

    Public ReadOnly Property FamilyID() As Integer
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' returns the familyid
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/6/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _FamilyID
        End Get
    End Property

    Private ReadOnly Property CommittedEntries(ByVal claimID As Integer, ByVal lineID As Integer) As DataTable
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="claimId"></param>
        ' <param name="lineId"></param>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/31/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Try

                Return MemberAccumulatorEntryDAL.GetCommittedEntries(claimID, lineID, _RelationID, _FamilyID)

            Catch ex As Exception
                Throw
            End Try
        End Get
    End Property

    Private ReadOnly Property CommittedEntries(ByVal claimID As Integer) As DataTable
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

                Return MemberAccumulatorEntryDAL.GetCommittedEntries(claimID, _RelationID, _FamilyID)

            Catch ex As Exception
                Throw
            End Try

        End Get
    End Property

    Private ReadOnly Property CommittedEntries() As DataTable
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

                Return MemberAccumulatorEntryDAL.GetCommittedEntries(_RelationID, _FamilyID)

            Catch ex As Exception
                Throw
            End Try
        End Get
    End Property

#End Region

#Region "Constructors"
    Public Sub New(ByVal relationID As Int16, ByVal familyID As Integer)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Constructor must have member and family ids for other functions
        '  and subs to work properly
        ' </summary>
        ' <param name="memberId"></param>
        ' <param name="familyId"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/15/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        MyBase.New()

        _RelationID = relationID
        _FamilyID = familyID
        _NeedsRefresh = True

        _AccumulatorCollection = New MemberAccumulatorCollection

    End Sub

    Public Sub New(ByVal relationID As Int16, ByVal familyID As Integer, ByVal needsRefresh As Boolean)

        MyBase.New()

        _RelationID = relationID
        _FamilyID = familyID
        _NeedsRefresh = needsRefresh

        _AccumulatorCollection = New MemberAccumulatorCollection

    End Sub

#End Region

#Region "Private Functions"

    Private Function IsDirty() As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	3/19/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim MemberAcculator As MemberAccumulator
        Dim AccumulatorCollectionEnum As IDictionaryEnumerator

        Try

            AccumulatorCollectionEnum = _AccumulatorCollection.GetEnumerator()
            While AccumulatorCollectionEnum.MoveNext()
                MemberAcculator = CType(AccumulatorCollectionEnum.Value, MemberAccumulator)
                If MemberAcculator.IsDirty Then
                    Return True
                End If
            End While

            Return False

        Catch ex As Exception

            Throw
        Finally
            MemberAcculator = Nothing
            AccumulatorCollectionEnum = Nothing
        End Try
    End Function

#End Region

#Region "Public"

    Public Function MembersFirstAccumulatorActivity() As Boolean

        Try

            Return Not CInt(GetOriginalLifetimeValue(CInt(AccumulatorController.GetAccumulatorID("FIXAC")))) <= 0

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Sub PopulateAccumulatorSummariesForMember()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Populate the Collection of Member Accumulator Summaries
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/15/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            RefreshAccumulatorSummariesForMember()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub RefreshAccumulatorSummariesForMember(ByVal manualOnly As Boolean, ByVal forceRefresh As Boolean)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Refresh the Accumulator Summaries
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/15/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            If forceRefresh Then _NeedsRefresh = True

            RefreshAccumulatorSummariesForMember(manualOnly)

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Sub RefreshAccumulatorSummariesForMember()

        Try

            RefreshAccumulatorSummariesForMember(False)

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Sub RefreshAccumulatorOOPSummariesForMemberYear(ByVal actionYear As Integer)

        Dim AccumulatorsDS As DataSet
        Dim Amt As Decimal = 0
        Dim MemberAccumulator As MemberAccumulator
        Dim AddDate As Date

        Try

            'clear out the hashtable

            _AccumulatorCollection.Clear()

            'get all the raw data
            AccumulatorsDS = MemberAccumulatorDAL.GetAccumulatorOOPSummaries(_FamilyID, _RelationID, actionYear)
            If AccumulatorsDS Is Nothing OrElse AccumulatorsDS.Tables("AccumulatorOOPSummary") Is Nothing OrElse AccumulatorsDS.Tables.Count <> 3 Then Return

            'loop through all summary rows


            'go through all rows brought back, 1 for each accumulator
            For Each DR As DataRow In AccumulatorsDS.Tables("AccumulatorOOPSummary").Rows

                'create a new memberaccumulator
                MemberAccumulator = New MemberAccumulator(CInt(DR("ACCUM_ID")), CShort(DR("RELATION_ID")), CInt(DR("FAMILY_ID")))
                If DR("DAYS_INFORMATION") IsNot System.DBNull.Value Then
                    Dim DaysA() As String = CStr(DR("DAYS_INFORMATION")).Split(CChar(","))
                    Dim S2() As String
                    Dim Day As String

                    For Each Str As String In DaysA
                        If Str IsNot Nothing Then
                            If Str.Length > 0 Then
                                S2 = Str.Split("?"c)
                                Day = S2(0)
                                Amt = CDec(S2(1))

                                AddDate = DateHelper.GetDateFromDayOfYear(CInt(DR("ACCUM_YEAR")), CInt(Day))
                                Dim AccumMatches As Integer = CInt(If(AccumulatorsDS.Tables.Contains("AccumulatorOOPDetail") AndAlso AccumulatorsDS.Tables("AccumulatorOOPDetail") IsNot Nothing AndAlso AccumulatorsDS.Tables("AccumulatorOOPDetail").Rows.Count > 0, AccumulatorsDS.Tables("AccumulatorOOPDetail").AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso r.Field(Of Integer)("ACCUM_ID") = CInt(DR("ACCUM_ID")) AndAlso r.Field(Of Integer)("CLAIM_ID") = -1 AndAlso r.Field(Of Int16)("LINE_NBR") = -1 AndAlso r.Field(Of Date)("APPLY_DATE").ToShortDateString = AddDate.ToShortDateString).Count, 0))
                                If AccumMatches > 0 Then
                                    Dim RecalculatedValue As Decimal = AccumulatorsDS.Tables("AccumulatorOOPDetail").AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso r.Field(Of Integer)("ACCUM_ID") = CInt(DR("ACCUM_ID")) AndAlso r.Field(Of Integer)("CLAIM_ID") = -1 AndAlso r.Field(Of Int16)("LINE_NBR") = -1 AndAlso r.Field(Of Date)("APPLY_DATE") = AddDate).Sum(Function(r) DirectCast(r("ENTRY_VALUE"), [Decimal]))

                                    MemberAccumulator.SetCurrentValue(AddDate, RecalculatedValue)
                                    MemberAccumulator.SetValue(AddDate, RecalculatedValue)
                                End If

                            End If
                        End If
                    Next

                    _AccumulatorCollection.Add(MemberAccumulator.AccumulatorID, MemberAccumulator)

                End If

            Next

            _NeedsRefresh = False

        Catch ex As Exception

            Throw

        Finally
            If AccumulatorsDS IsNot Nothing Then
                AccumulatorsDS.Dispose()
            End If
            AccumulatorsDS = Nothing
            'If MemberAccumulator IsNot Nothing Then
            '    MemberAccumulator.Dispose()
            'End If
            MemberAccumulator = Nothing

        End Try

    End Sub

    Public Sub RefreshAccumulatorSummariesForMember(ByVal manualOnly As Boolean)

        If Not _NeedsRefresh Then Return

        Dim DR As DataRow
        Dim Amt As Decimal
        Dim AddDate As Date

#If DEBUG Then
        Debug.Print(UFCWGeneral.NowDate.TimeOfDay.ToString & " RefreshAccumulatorSummariesForMember")
#End If

        Try

            'clear out the hashtable

            _AccumulatorCollection.Clear()

            'get all the raw data
            Using AccumulatorsDS As DataSet = MemberAccumulatorDAL.GetAccumulatorSummaries(_RelationID, _FamilyID, manualOnly)

                If AccumulatorsDS Is Nothing Then Return

                'loop through all summary rows


                'go through all rows brought back
                For I As Integer = 0 To AccumulatorsDS.Tables("AccumulatorSummary").Rows.Count - 1

                    DR = AccumulatorsDS.Tables("AccumulatorSummary").Rows(I)

                    'see if an accumulator already exists with that id
                    If _AccumulatorCollection.Contains(DR("ACCUM_ID")) Then
                        'copy all values

                        If DR("DAYS_INFORMATION") IsNot System.DBNull.Value Then
                            Dim S() As String = CStr(DR("DAYS_INFORMATION")).Split(CChar(","))
                            Dim S2() As String
                            Dim Day As String

                            For Each Str As String In S
                                If Str IsNot Nothing Then
                                    If Str.Length > 0 Then
                                        S2 = Str.Split("?"c)
                                        Day = S2(0)
                                        Amt = CDec(S2(1))

                                        AddDate = DateHelper.GetDateFromDayOfYear(CInt(DR("ACCUM_YEAR")), CInt(Day))

                                        CType(_AccumulatorCollection(DR("ACCUM_ID")), MemberAccumulator).SetCurrentValue(AddDate, Amt)
                                        CType(_AccumulatorCollection(DR("ACCUM_ID")), MemberAccumulator).SetValue(AddDate, Amt)
                                    End If
                                End If
                            Next
                        End If

                    Else
                        'create a new memberaccumulator
                        Dim TheMemberAccumulator As New MemberAccumulator(CInt(DR("ACCUM_ID")), _RelationID, _FamilyID)

                        If DR("DAYS_INFORMATION") IsNot System.DBNull.Value Then
                            Dim S() As String = CStr(DR("DAYS_INFORMATION")).Split(CChar(","))
                            Dim S2() As String
                            Dim Day As String

                            For Each Str As String In S
                                If Str IsNot Nothing Then
                                    If Str.Length > 0 Then
                                        S2 = Str.Split("?"c)
                                        Day = S2(0)
                                        Amt = CDec(S2(1))

                                        AddDate = DateHelper.GetDateFromDayOfYear(CInt(DR("ACCUM_YEAR")), CInt(Day))

                                        TheMemberAccumulator.SetCurrentValue(AddDate, Amt)
                                        TheMemberAccumulator.SetValue(AddDate, Amt)
                                    End If
                                End If
                            Next

                            _AccumulatorCollection.Add(TheMemberAccumulator.AccumulatorID, TheMemberAccumulator)

                        End If

                    End If
                Next

                _NeedsRefresh = False
            End Using

        Catch ex As Exception

            Throw

        Finally

            DR = Nothing

        End Try

    End Sub

    Public Function GetAccidentAccumulatorEntries() As DataTable

        Try
            Return MemberAccumulatorEntryDAL.GetAccidentAccumulatorEntries(_RelationID, _FamilyID)

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Function GetMemberAccumulator(ByVal accumulatorID As Integer) As MemberAccumulator
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets a MemberAccumulator with that Id
        ' </summary>
        ' <param name="accumulatorId"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            If _AccumulatorCollection(accumulatorID) Is Nothing Then
                Return New MemberAccumulator(accumulatorID, UFCWGeneral.NowDate.Year, _RelationID, _FamilyID)
            Else
                Return CType(_AccumulatorCollection(accumulatorID), MemberAccumulator)
            End If

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Function GetProposedValue(ByVal accumulatorID As Integer, ByVal dateType As DateType, ByVal dateTypeQuantity As Integer, ByVal dateTypeStart As Date?, ByVal dateDirection As DateDirection) As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Get the proposed value for the Accumulator for given time period
        ' </summary>
        ' <param name="accumulatorId"></param>
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
        Dim Amt As Decimal = 0
        Dim MemberAccumulator As MemberAccumulator

        Try
            MemberAccumulator = CType(_AccumulatorCollection(accumulatorID), MemberAccumulator)
            If MemberAccumulator IsNot Nothing Then
                Amt = GetProposedValue(MemberAccumulator, dateType, dateTypeQuantity, dateTypeStart, dateDirection)
            End If

            Return Math.Round(Amt, 2)

        Catch ex As Exception
            Throw
            'Finally
            '    MemberAccumulator = Nothing
        End Try
    End Function

    Public Function GetOriginalValue(ByVal accumulatorID As Integer, ByVal dateType As DateType, ByVal dateTypeQuantity As Integer, ByVal dateTypeStart As Date?, ByVal dateDirection As DateDirection) As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Get original value for the accumulator for the date period
        ' </summary>
        ' <param name="accumulatorId"></param>
        ' <param name="dateType"></param>
        ' <param name="dateTypeQuantity"></param>
        ' <param name="dateTypeStart"></param>
        ' <param name="dateDirection"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/1/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim Amt As Decimal = 0
        Dim MemberAccumulator As MemberAccumulator

        Try
            MemberAccumulator = CType(_AccumulatorCollection(accumulatorID), MemberAccumulator)
            If MemberAccumulator IsNot Nothing Then
                Amt = GetOriginalValue(MemberAccumulator, dateType, dateTypeQuantity, dateTypeStart, dateDirection)
            End If

            Return Math.Round(Amt, 2)

        Catch ex As Exception
            Throw
        Finally
            MemberAccumulator = Nothing
        End Try

    End Function

    Private Shared Function GetOriginalValue(ByVal accumulator As MemberAccumulator, ByVal dateType As DateType, ByVal dateTypeQuantity As Integer, ByVal dateTypeStart As Date?, ByVal dateDirection As DateDirection) As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Get original value for the accumulator for the date period
        ' </summary>
        ' <param name="accumulator"></param>
        ' <param name="dateType"></param>
        ' <param name="dateTypeQuantity"></param>
        ' <param name="dateTypeStart"></param>
        ' <param name="dateDirection"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/1/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            Return accumulator.GetOriginalValue(dateType, dateTypeQuantity, dateTypeStart, dateDirection)

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Shared Function GetProposedValue(ByVal accumulator As MemberAccumulator, ByVal dateType As DateType, ByVal dateTypeQuantity As Integer, ByVal dateTypeStart As Date?, ByVal dateDirection As DateDirection) As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' This function is made private because it cant really be used outside of this class
        '  right UFCWGeneral.NowDate.  So I have left it private and am using it within this class right UFCWGeneral.NowDate.
        ' </summary>
        ' <param name="accumulator"></param>
        ' <param name="dateType"></param>
        ' <param name="dateTypeQuantity"></param>
        ' <param name="dateTypeStart"></param>
        ' <param name="dateDirection"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/15/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim AccumValue As Decimal

        Try

            AccumValue = Math.Round(accumulator.GetProposedValue(dateType, dateTypeQuantity, dateTypeStart, dateDirection), 2)

            Return AccumValue

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Function

    Public Function GetProposedYearValue(ByVal accumulatorID As Integer, ByVal year As Integer) As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets the value for that year for that accumulator
        ' </summary>
        ' <param name="accumualtorId"></param>
        ' <param name="year"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim AccumValue As Decimal
        Dim MemberAccum As MemberAccumulator

        Try
            MemberAccum = GetMemberAccumulator(accumulatorID)

            AccumValue = Math.Round(MemberAccum.ProposedYearValue(year), 2)

            Return AccumValue

        Catch ex As Exception
            Throw
        Finally
            'If MemberAccum IsNot Nothing Then
            '    MemberAccum.Dispose()
            'End If
            MemberAccum = Nothing
        End Try

    End Function

    Public Function GetProposedLifetimeValue(ByVal accumulatorID As Integer) As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets the proposed lifetime value for the accumulator with the accumulatorid
        ' </summary>
        ' <param name="accumualtorId"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim AccumValue As Decimal
        Dim MemberAccum As MemberAccumulator

        Try

            MemberAccum = GetMemberAccumulator(accumulatorID)

            AccumValue = Math.Round(MemberAccum.ProposedLifetimeValue, 2)

            Return AccumValue

        Catch ex As Exception
            Throw
        Finally
            'If MemberAccum IsNot Nothing Then
            '    MemberAccum.Dispose()
            'End If
            MemberAccum = Nothing

        End Try

    End Function

    Public Function GetOriginalLifetimeValue(ByVal accumulatorID As Integer) As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets the original lifetime value for the accumulator with the accumulatorid
        ' </summary>
        ' <param name="accumualtorId"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim AccumValue As Decimal
        Dim MemberAccum As MemberAccumulator

        Try

            MemberAccum = GetMemberAccumulator(accumulatorID)

            AccumValue = Math.Round(MemberAccum.OriginalLifetimeValue, 2)

            Return AccumValue

        Catch ex As Exception
            Throw
        Finally
            'If MemberAccum IsNot Nothing Then
            '    MemberAccum.Dispose()
            'End If
            MemberAccum = Nothing

        End Try

    End Function

    Public Function GetProposedRolloverValue(ByVal accumulatorID As Integer, ByVal year As Integer) As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' gets the proposed rollover value for that accumulator
        ' </summary>
        ' <param name="accumualtorId"></param>
        ' <param name="year"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim AccumValue As Decimal
        Dim MemberAccum As MemberAccumulator

        Try

            MemberAccum = GetMemberAccumulator(accumulatorID)

            AccumValue = Math.Round(MemberAccum.ProposedRolloverValue(year), 2)

            Return AccumValue

        Catch ex As Exception
            Throw
        Finally
            'If MemberAccum IsNot Nothing Then
            '    MemberAccum.Dispose()
            'End If
            MemberAccum = Nothing

        End Try

    End Function

    Public Function GetProposedCurrentValueForCurrentYear(ByVal accumulatorID As Integer) As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets the proposed current value for the current year
        ' </summary>
        ' <param name="accumualtorId"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim AccumValue As Decimal
        Dim MemberAccum As MemberAccumulator

        Try

            MemberAccum = GetMemberAccumulator(accumulatorID)

            AccumValue = Math.Round(MemberAccum.ProposedCurrentValueForCurrentYear, 2)

            Return AccumValue

        Catch ex As Exception
            Throw
        Finally
            'If MemberAccum IsNot Nothing Then
            '    MemberAccum.Dispose()
            'End If
            MemberAccum = Nothing

        End Try

    End Function

    Public Sub InsertEntry(ByVal accumulatorID As Integer, ByVal claimID As Integer, ByVal lineNumber As Int16, ByVal applyDate As Date, ByVal value As Decimal, ByVal isOverride As Boolean, ByVal userID As String)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Inserts an entry into the MemberAccumulatorEntries table
        ' </summary>
        ' <param name="accumulatorId"></param>
        ' <param name="dateOfValue"></param>
        ' <param name="value"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/19/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim MemberAccumulator As MemberAccumulator

        Try
            'TODO:  Uncomment this code...this is commented out to accomodate vics methodology of flagging suspect information

            If applyDate.AddDays(-1) > UFCWGeneral.NowDate.Date Then
                Throw New ArgumentException("Date cannot be in future.  Entry not added")
            End If

            'look for the accumulator
            MemberAccumulator = CType(_AccumulatorCollection(accumulatorID), MemberAccumulator)

            'if it exists then insert the entry to that accumulator
            If MemberAccumulator Is Nothing Then
                MemberAccumulator = New MemberAccumulator(accumulatorID, applyDate.Year, _RelationID, _FamilyID)
            End If

            'insert a new entry
            MemberAccumulator.InsertEntry(claimID, lineNumber, applyDate, value, isOverride, userID)
            _AccumulatorCollection.Remove(accumulatorID)
            _AccumulatorCollection.Add(accumulatorID, MemberAccumulator)

            _NeedsRefresh = True

        Catch ex As Exception
            Throw
        Finally
            MemberAccumulator = Nothing
        End Try
    End Sub

    Public Sub InsertEntry(ByVal accumulatorID As Integer, ByVal claimID As Integer, ByVal lineNumber As Int16, ByVal originalClaimIdForAccident As Integer, ByVal applyDate As Date, ByVal value As Decimal, ByVal isOverride As Boolean, ByVal userID As String)

        SyncLock _AccumulatorCollection.SyncRoot
            Dim MemberAccumulator As MemberAccumulator

            Try
                If applyDate.AddDays(-1) > UFCWGeneral.NowDate.Date Then
                    Throw New ArgumentException("Date cannot be in future.  Entry not added")
                End If

                'look for the accumulator
                MemberAccumulator = CType(_AccumulatorCollection(accumulatorID), MemberAccumulator)

                'if it exists then insert the entry to that accumulator
                If MemberAccumulator Is Nothing Then
                    MemberAccumulator = New MemberAccumulator(accumulatorID, applyDate.Year, _RelationID, _FamilyID)
                End If

                'insert a new entry
                MemberAccumulator.InsertEntry(claimID, lineNumber, originalClaimIdForAccident, applyDate, value, isOverride, userID)

                If _AccumulatorCollection.ContainsKey(accumulatorID) Then
                    _AccumulatorCollection.Item(accumulatorID) = MemberAccumulator
                Else
                    _AccumulatorCollection.Add(accumulatorID, MemberAccumulator)
                End If

                _NeedsRefresh = True

            Catch ex As Exception
                Throw
            Finally
                MemberAccumulator = Nothing
            End Try
        End SyncLock

    End Sub

    Public Sub InsertEntry(ByVal accumulatorID As Integer, ByVal applyDate As Date, ByVal value As Decimal, ByVal isOverride As Boolean, ByVal userID As String)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Inserts an entry into the MemberAccumulatorEntries table
        ' </summary>
        ' <param name="accumulatorId"></param>
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

        Dim MemberAccumulator As MemberAccumulator

        Try
            If applyDate.AddDays(-1) > UFCWGeneral.NowDate.Date Then
                Throw New ArgumentException("Date cannot be in future.  Entry not added")
            End If

            'look for the accumulator
            MemberAccumulator = CType(_AccumulatorCollection(accumulatorID), MemberAccumulator)

            'if it exists then insert the entry to that accumulator
            If MemberAccumulator Is Nothing Then
                MemberAccumulator = New MemberAccumulator(accumulatorID, applyDate.Year, _RelationID, _FamilyID)
            End If

            'insert a new entry
            MemberAccumulator.InsertEntry(applyDate, value, isOverride, userID)

            _AccumulatorCollection.Remove(accumulatorID)
            _AccumulatorCollection.Add(accumulatorID, MemberAccumulator)

            _NeedsRefresh = True

        Catch ex As Exception
            Throw
        Finally
            MemberAccumulator = Nothing
        End Try
    End Sub

    Public Sub RemoveEntry(ByVal accumulatorID As Integer, ByVal lineNumber As Int16)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Removes the line from the Entries table and updates the summary table too
        ' </summary>
        ' <param name="accumulatorId"></param>
        ' <param name="lineNumber"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/17/2006	Created at Request of Nick.  VL Does not agree with this decision
        '                         but is implementing it at the request of UFCW management.
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim MemberAccumulator As MemberAccumulator

        Try
            MemberAccumulator = CType(_AccumulatorCollection(accumulatorID), MemberAccumulator)
            MemberAccumulator.RemoveEntry(lineNumber)

            _NeedsRefresh = True

        Catch ex As Exception
            Throw
        Finally
            MemberAccumulator = Nothing
        End Try

    End Sub

    Public Sub OverrideEntry(ByVal accumulatorID As Integer, ByVal claimID As Integer, ByVal lineNumber As Int16, ByVal applyDate As Date, ByVal value As Decimal, ByVal userID As String)

        Dim MemberAccumulator As MemberAccumulator

        Try
            MemberAccumulator = CType(_AccumulatorCollection(accumulatorID), MemberAccumulator)
            MemberAccumulator.OverrideEntry(claimID, lineNumber, applyDate, value, True, userID)

            _NeedsRefresh = True

        Catch ex As Exception
            Throw
        Finally
            MemberAccumulator = Nothing
        End Try

    End Sub

    Public Sub CommitAll(Optional forceUpdate As Boolean = False)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Commits all Entries to the database
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/19/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim MemberAccumulator As MemberAccumulator
        Dim Enumerator As IDictionaryEnumerator

        Try

            Enumerator = _AccumulatorCollection.GetEnumerator()
            While Enumerator.MoveNext()
                MemberAccumulator = CType(Enumerator.Value, MemberAccumulator)
                If MemberAccumulator.IsDirty OrElse forceUpdate Then
                    MemberAccumulator.Commit(forceUpdate)
                End If
            End While

            _NeedsRefresh = False

        Catch ex As Exception
            Throw
        Finally
            MemberAccumulator = Nothing
            Enumerator = Nothing
        End Try

    End Sub

    Public Sub CommitAll(ByVal transaction As DbTransaction)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Commit entries within context of a transaction
        ' </summary>
        ' <param name="transaction"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim MemberAccumulator As MemberAccumulator
        Dim Enumerator As IDictionaryEnumerator

        Try

            Enumerator = _AccumulatorCollection.GetEnumerator()
            While Enumerator.MoveNext()
                MemberAccumulator = CType(Enumerator.Value, MemberAccumulator)
                If MemberAccumulator.IsDirty Then
                    MemberAccumulator.Commit(transaction)
                End If
            End While

            _NeedsRefresh = False

        Catch ex As Exception
            Throw
        Finally
            MemberAccumulator = Nothing
            Enumerator = Nothing
        End Try

    End Sub

    Public Sub RollbackAll(ByVal claimID As Integer, ByVal userAccount As String)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' rollsback all of a claims committed accumulators
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	2/13/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            RollbackAll(claimID, userAccount, Nothing)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub RollbackAll(ByVal claimID As Integer, ByVal userAccount As String, ByVal transaction As DbTransaction)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' rollsback all of a claims committed accumulators
        ' </summary>
        ' <param name="ClaimID"></param>
        ' <param name="Transaction"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	2/13/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim EntriesDT As DataTable

        Try

            RefreshAccumulatorSummariesForMember()

            EntriesDT = GetAccumulatorEntryValues(False, claimID)

            If EntriesDT.Rows.Count > 0 Then

                For Cnt As Integer = 0 To EntriesDT.Rows.Count - 1
                    If CDec(EntriesDT.Rows(Cnt)("ENTRY_VALUE")) <> 0D Then
                        InsertEntry(CInt(AccumulatorController.GetAccumulatorID(CStr(EntriesDT.Rows(Cnt)("ACCUM_NAME")))), claimID, CShort(EntriesDT.Rows(Cnt)("LINE_NBR")), CDate(EntriesDT.Rows(Cnt)("APPLY_DATE")), CDec(EntriesDT.Rows(Cnt)("ENTRY_VALUE")) * -1, True, userAccount.ToUpper)
                    End If
                Next

                If transaction IsNot Nothing Then
                    CommitAll(transaction)
                    _NeedsRefresh = False
                Else
                    CommitAll()
                    _NeedsRefresh = False
                End If
            End If

        Catch ex As Exception

            Throw

        Finally
            If EntriesDT IsNot Nothing Then
                EntriesDT.Dispose()
            End If
            EntriesDT = Nothing

        End Try

    End Sub

    Public Function GetHighestEntryIdForFamily() As Integer
        Try

            Return MemberAccumulatorEntryDAL.GetHighestEntryIdForFamily(_FamilyID)

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetAccumulatorEntryValues(ByVal proposed As Boolean) As DataTable
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets a usable list of entries
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/16/2006	Created
        '     [paulw] 10/17/2006  Augmented 'Display Order' at Request of Nick.  VL Does not agree with this decision
        '                         but is implementing it at the request of UFCW management.
        '     [nicks] 2/14/2007   Revised
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim NewDR As DataRow
        Dim AccumEntriesDT As DataTable
        Dim DTLAccumDT As DataTable
        Dim AccumsDT As DataTable

        Try

            DTLAccumDT = CMSDALCommon.CreateAccumulatorValuesDT("DTL_ACCUMS")

            If proposed Then
                AccumsDT = AccumulatorController.GetAccumulators()

                For Each AccumDR As DataRow In AccumsDT.Rows
                    If _AccumulatorCollection(AccumDR("ACCUM_ID")) IsNot Nothing Then

                        AccumEntriesDT = CType(_AccumulatorCollection(AccumDR("ACCUM_ID")), MemberAccumulator).MemberAccumulatorEntries

                        For Each AccumEntryDR As DataRow In AccumEntriesDT.Rows
                            NewDR = DTLAccumDT.NewRow
                            NewDR("CLAIM_ID") = AccumEntryDR("CLAIM_ID")
                            NewDR("ACCUM_NAME") = AccumulatorController.GetAccumulatorName(CInt(AccumEntryDR("ACCUM_ID")))
                            NewDR("ENTRY_VALUE") = AccumEntryDR("ENTRY_VALUE")
                            NewDR("OVERRIDE_SW") = AccumEntryDR("OVERRIDE_SW")
                            NewDR("LINE_NBR") = AccumEntryDR("LINE_NBR")
                            NewDR("APPLY_DATE") = AccumEntryDR("APPLY_DATE")
                            NewDR("DISPLAY_ORDER") = AccumulatorController.GetAccumulatorDisplayOrder(CInt(AccumEntryDR("ACCUM_ID")))
                            DTLAccumDT.Rows.Add(NewDR)
                        Next
                    End If
                Next
            Else
                AccumEntriesDT = CommittedEntries()
                AccumEntriesDT.Columns.Add("ACCUM_NAME")
                AccumEntriesDT.Columns.Add("DISPLAY_ORDER", Type.GetType("System.Int32"))

                For Each AccumEntryDR As DataRow In AccumEntriesDT.Rows
                    AccumEntryDR("ACCUM_NAME") = AccumulatorController.GetAccumulatorName(CInt(AccumEntryDR("ACCUM_ID")))
                    AccumEntryDR("DISPLAY_ORDER") = AccumulatorController.GetAccumulatorDisplayOrder(CInt(AccumEntryDR("ACCUM_ID")))
                    DTLAccumDT.ImportRow(AccumEntryDR)
                Next

            End If

            DTLAccumDT.DefaultView.Sort = "DISPLAY_ORDER"

            Return DTLAccumDT

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetAccumulatorEntryValues(ByVal proposed As Boolean, ByVal claimID As Integer) As DataTable
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="proposed"></param>
        ' <param name="claimId"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/31/2006	Created
        '     [nicks] 2/14/2007   Revised
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim NewDR As DataRow
        Dim AccumEntriesDT As DataTable
        Dim AccumSummaryDT As DataTable

        Try

            AccumSummaryDT = CMSDALCommon.CreateAccumulatorValuesDT("DTL_ACCUMS")

            If proposed Then
                AccumEntriesDT = AccumulatorController.GetAccumulators()
            Else
                AccumEntriesDT = CommittedEntries(claimID)
            End If

            AccumEntriesDT.DefaultView.Sort = "CLAIM_ID, LINE_NBR, ACCUM_ID"

            Dim QuerySumByLineAccum =
                (
                    From Accum As DataRow In AccumEntriesDT.AsEnumerable
                    Where Accum.RowState <> DataRowState.Deleted
                    Group By CLAIM_ID = Accum.Field(Of Integer)("CLAIM_ID"),
                                LINE_NBR = Accum.Field(Of Short)("LINE_NBR"),
                                ACCUM_ID = Accum.Field(Of Integer)("ACCUM_ID"),
                                APPLY_DATE = Accum.Field(Of Date)("APPLY_DATE")
                   Into Group
                    Order By CLAIM_ID, LINE_NBR, ACCUM_ID
                    Where Group.Sum(Function(S) S.Field(Of Decimal)("ENTRY_VALUE")) <> 0
                    Select CLAIM_ID, LINE_NBR, ACCUM_ID, APPLY_DATE, SUM_ENTRY_VALUE = Group.Sum(Function(S) S.Field(Of Decimal)("ENTRY_VALUE"))
                )

            For Each AccumEntrySum In QuerySumByLineAccum.AsEnumerable

                NewDR = AccumSummaryDT.NewRow
                NewDR("CLAIM_ID") = AccumEntrySum.CLAIM_ID
                NewDR("ACCUM_NAME") = AccumulatorController.GetAccumulatorName(CInt(AccumEntrySum.ACCUM_ID))
                NewDR("ENTRY_VALUE") = AccumEntrySum.SUM_ENTRY_VALUE
                NewDR("LINE_NBR") = AccumEntrySum.LINE_NBR
                NewDR("APPLY_DATE") = AccumEntrySum.APPLY_DATE
                NewDR("DISPLAY_ORDER") = AccumulatorController.GetAccumulatorDisplayOrder(CInt(AccumEntrySum.ACCUM_ID))

                AccumSummaryDT.Rows.Add(NewDR)

            Next

            AccumSummaryDT.DefaultView.Sort = "CLAIM_ID, LINE_NBR, DISPLAY_ORDER, ACCUM_NAME"

            Return AccumSummaryDT

        Catch ex As Exception
            Throw
        Finally

            If AccumEntriesDT IsNot Nothing Then AccumEntriesDT.Dispose()

            NewDR = Nothing
            AccumEntriesDT = Nothing

        End Try
    End Function

    Public Function GetAccumulatorEntryValues(ByVal proposed As Boolean, ByVal claimID As Integer, ByVal lineNumber As Short) As DataTable
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets a usable list of entries per line
        ' </summary>
        ' <param name="lineNumber"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/16/2006	Created
        '     [paulw] 10/17/2006  Augmented 'Display Order' at Request of Nick.  VL Does not agree with this decision
        '                         but is implementing it at the request of UFCW management.
        '     [nicks] 2/14/2007   Revised
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim DR As DataRow '= dt.NewRow
        Dim EntriesDT As DataTable
        Dim DTLAccumDT As DataTable
        Dim AccumDV As DataView
        Dim AccumName As String = ""

        Try

            DTLAccumDT = CMSDALCommon.CreateAccumulatorValuesDT("DTL_ACCUMS")

            AccumDV = New DataView(DTLAccumDT, "", "ACCUM_NAME", DataViewRowState.CurrentRows)

            If proposed Then
                Dim AccumsDT As DataTable = AccumulatorController.GetAccumulators()

                For I As Integer = 0 To AccumsDT.Rows.Count - 1
                    If _AccumulatorCollection(AccumsDT.Rows(I)("ACCUM_ID")) IsNot Nothing Then
                        EntriesDT = CType(_AccumulatorCollection(AccumsDT.Rows(I)("ACCUM_ID")), DataTable)

                        For J As Integer = 0 To EntriesDT.Rows.Count - 1
                            If CInt(EntriesDT.Rows(J)("LINE_NBR")) = lineNumber Then
                                DR = DTLAccumDT.NewRow
                                DR("CLAIM_ID") = EntriesDT.Rows(J)("CLAIM_ID")
                                DR("ACCUM_NAME") = AccumulatorController.GetAccumulatorName(CInt(EntriesDT.Rows(J)("ACCUM_ID")))
                                DR("ENTRY_VALUE") = EntriesDT.Rows(J)("ENTRY_VALUE")
                                DR("OVERRIDE_SW") = EntriesDT.Rows(J)("OVERRIDE_SW")
                                DR("LINE_NBR") = EntriesDT.Rows(J)("LINE_NBR")
                                DR("APPLY_DATE") = EntriesDT.Rows(J)("APPLY_DATE")
                                DR("DISPLAY_ORDER") = AccumulatorController.GetAccumulatorDisplayOrder(CInt(EntriesDT.Rows(J)("ACCUM_ID")))
                                DTLAccumDT.Rows.Add(DR)
                            End If
                        Next
                    End If
                Next
            Else
                EntriesDT = CommittedEntries(claimID, lineNumber)

                For j As Integer = 0 To EntriesDT.Rows.Count - 1
                    AccumName = AccumulatorController.GetAccumulatorName(CInt(EntriesDT.Rows(j)("ACCUM_ID")))
                    AccumDV.RowFilter = "CLAIM_ID = " & EntriesDT.Rows(j)("CLAIM_ID").ToString & " AND ACCUM_NAME = '" & AccumName & "' AND LINE_NBR = " & EntriesDT.Rows(j)("LINE_NBR").ToString

                    If AccumDV.Count > 0 Then
                        AccumDV(0)("ENTRY_VALUE") = CDec(EntriesDT.Rows(j)("ENTRY_VALUE")) + CDec(AccumDV(0)("ENTRY_VALUE"))
                    Else
                        DR = DTLAccumDT.NewRow
                        DR("CLAIM_ID") = EntriesDT.Rows(j)("CLAIM_ID")
                        DR("ACCUM_NAME") = AccumName
                        DR("ENTRY_VALUE") = EntriesDT.Rows(j)("ENTRY_VALUE")
                        DR("OVERRIDE_SW") = EntriesDT.Rows(j)("OVERRIDE_SW")
                        DR("LINE_NBR") = EntriesDT.Rows(j)("LINE_NBR")
                        DR("APPLY_DATE") = EntriesDT.Rows(j)("APPLY_DATE")
                        DR("DISPLAY_ORDER") = AccumulatorController.GetAccumulatorDisplayOrder(CInt(EntriesDT.Rows(j)("ACCUM_ID")))
                        DTLAccumDT.Rows.Add(DR)
                    End If
                Next
            End If

            DTLAccumDT.DefaultView.Sort = "DISPLAY_ORDER"

            Return DTLAccumDT

        Catch ex As Exception
            Throw
        Finally

            If EntriesDT IsNot Nothing Then EntriesDT.Dispose()
            If AccumDV IsNot Nothing Then AccumDV.Dispose()

            DR = Nothing
            EntriesDT = Nothing
            AccumDV = Nothing

        End Try

    End Function

    Public Function GetOverrideHistory() As DataTable
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
        Try

            Return MemberAccumulatorEntryDAL.GetOverrideHistory(_FamilyID, _RelationID)

        Catch ex As Exception
            Throw
        End Try

    End Function
#End Region

#Region "Clean Up"

    Public Sub Clear()
        Try

            If _AccumulatorCollection IsNot Nothing Then _AccumulatorCollection.Clear()
            _AccumulatorCollection = New MemberAccumulatorCollection 'reset for next use

        Catch ex As Exception
            Throw
        End Try
    End Sub

    'Public Overloads Sub Dispose() Implements IDisposable.Dispose
    '    Dispose(True)
    '    GC.SuppressFinalize(Me)
    'End Sub

    'Protected Overloads Sub Dispose(ByVal disposing As Boolean)
    '    If _disposed = False Then
    '        If disposing Then

    '            'Debug.Print(TypeName(Me) & " Dispose: " & _ClassGuid.ToString)

    '            ' Free other state (managed objects).
    '            If Not IsNothing(_accumulatorCollection) Then
    '                _accumulatorCollection.Clear()
    '            End If

    '            _accumulatorCollection = Nothing

    '            _disposed = True
    '        End If
    '        ' Free your own state (unmanaged objects).
    '        ' Set large fields to null.
    '    End If
    'End Sub

    'Protected Overrides Sub Finalize()
    '    MyBase.Finalize()
    'End Sub

#End Region

#Region "Clone"

    Public Function DeepCopy() As MemberAccumulatorManager

        Dim MemberAccumulatorManagerClone As MemberAccumulatorManager
        Dim BeginTime As Date = UFCWGeneral.NowDate

        Try

            ' This copies references types by just copying the pointer, so to break any connection back to those object the objects need to be recreated.
            MemberAccumulatorManagerClone = Me.ShallowCopy
            MemberAccumulatorManagerClone._AccumulatorCollection = Me.AccumulatorCollection.DeepCopy

            Return MemberAccumulatorManagerClone

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            'If _TraceSwitch.Enabled Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceSwitch" & vbTab)
#End If
            MemberAccumulatorManagerClone = Nothing
        End Try
    End Function

    Public Function ShallowCopy() As MemberAccumulatorManager
        Dim BeginTime As Date = UFCWGeneral.NowDate
        Dim MemberAccumulatorManagerClone As MemberAccumulatorManager

        Try

            MemberAccumulatorManagerClone = DirectCast(Me.MemberwiseClone(), MemberAccumulatorManager)
            MemberAccumulatorManagerClone._AccumulatorCollection = Nothing

            Return MemberAccumulatorManagerClone

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            'If _TraceSwitch.Enabled Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceSwitch" & vbTab)
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

    'Private Sub Dispose(disposing As Boolean)

    '    If Not _Disposed Then
    '        If disposing Then
    '            If _AccumulatorCollection IsNot Nothing Then _AccumulatorCollection.Clear()
    '        End If

    '        ' TODO: free unmanaged resources (unmanaged objects) and override finalizer
    '        ' TODO: set large fields to null
    '        _Disposed = True
    '    End If
    'End Sub

    '' ' TODO: override finalizer only if 'Dispose(disposing As Boolean)' has code to free unmanaged resources
    '' Protected Overrides Sub Finalize()
    ''     ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
    ''     Dispose(disposing:=False)
    ''     MyBase.Finalize()
    '' End Sub

    'Public Sub Dispose() Implements IDisposable.Dispose
    '    ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
    '    Dispose(disposing:=True)
    '    GC.SuppressFinalize(Me)
    'End Sub

#End Region

End Class
Public Enum QueryType
    CurrentValue = 0
    LifetimeValue = 1
    CurrentYearValue = 2
    LastYearValue = 3
    CurrentYearFirstThreeQuarters = 4
    CurrentYearLastQuarter = 5
    LastYearFirstThreeQuarters = 6
    LastYearLastQuarter = 7
    RollOver = 8
    Annual = 9
    Days = 10
    Weeks = 11
    Months = 12
    Quarters = 13
    Years = 14
End Enum
