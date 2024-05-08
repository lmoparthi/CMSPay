Option Explicit On
Option Strict On
Option Infer On

Imports System.Collections
Imports System.Configuration
Imports System.Threading.Tasks
Imports System.Threading

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.Procedures
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class represents a Collection of Procedures.  This will generally be
'''  used to store all staged or active procedures for a given plan
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	1/25/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Class Procedures
    Inherits CollectionBase
    'KeyedCollection(Of Tuple, Procedure)
    'Implements IDisposable

#Region "Variables"
    Private Shared _TraceCloning As New TraceSwitch("TraceCloning", "Trace Switch in App.Config", "0")
    Private Shared _TraceParallel As New TraceSwitch("TraceParallel", "Parallel Trace Switch in App.Config", "0")

    Private Shared _AddSyncLock As New Object
    Private Shared _GetBestMatchSyncLock As New Object

    Private _ProcedureWeight As Integer = 1
    Private _ModifierWeight As Integer
    Private _ProviderWeight As Integer
    Private _DiagnosisWeight As Integer
    Private _BillTypeWeight As Integer
    Private _GenderWeight As Integer
    Private _DateOfBirthWeight As Integer
    Private _PlaceOfServiceWeight As Integer
    Private _WildCardProceduresAdded As Boolean

    Private _IndexKeyCrossRefDT As DataTable

#End Region

#Region "Constructors"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Default Constructor
    ' </summary>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Sub New()
        MyBase.New()
        _ModifierWeight = Convert.ToInt32(ConfigurationManager.AppSettings("MODIFIER_WEIGHT"))
        _ProviderWeight = Convert.ToInt32(ConfigurationManager.AppSettings("PROVIDER_WEIGHT"))
        _DiagnosisWeight = Convert.ToInt32(ConfigurationManager.AppSettings("DIAGNOSIS_WEIGHT"))
        _BillTypeWeight = Convert.ToInt32(ConfigurationManager.AppSettings("BILL_TYPE_WEIGHT"))
        _PlaceOfServiceWeight = Convert.ToInt32(ConfigurationManager.AppSettings("PLACE_OF_SERVICE_WEIGHT"))
        _GenderWeight = Convert.ToInt32(ConfigurationManager.AppSettings("GENDER_WEIGHT"))
        _DateOfBirthWeight = Convert.ToInt32(ConfigurationManager.AppSettings("DOB_WEIGHT"))

        _IndexKeyCrossRefDT = New DataTable
        _IndexKeyCrossRefDT.Columns.Add("ListIndex", System.Type.GetType("System.Int32"))
        _IndexKeyCrossRefDT.Columns.Add("SequenceNumber", System.Type.GetType("System.Int32"))
        _IndexKeyCrossRefDT.Columns.Add("PlanType", System.Type.GetType("System.String"))

    End Sub

    Public Sub New(ByVal theProcedures As Procedures)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Constructor that will allow for copying of a procedure collection
        ' </summary>
        ' <param name="procedures"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        MyClass.New()

        Me.InnerList().AddRange(theProcedures)

    End Sub
#End Region

#Region "Properties"
    Public Property WildCardProceduresAdded() As Boolean
        Get
            Return _WildCardProceduresAdded
        End Get
        Set(ByVal value As Boolean)
            _WildCardProceduresAdded = value
        End Set
    End Property

    Default Public Property Item(ByVal index As Integer) As IProcedure
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' implements the collection indexer.
        ' </summary>
        ' <param name="index"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[GregS]	1/8/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Try

                Return DirectCast(Me.List(index), IProcedure)

            Catch ex As Exception
                Throw
            Finally
#If TRACE Then
                If CInt(_TraceParallel.Level) > 3 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
#End If
            End Try

        End Get
        Set(ByVal value As IProcedure)
            Me.List(index) = value
        End Set
    End Property
#End Region

#Region "Functions"
    Public Sub Sort()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Sorts the collection
        ' </summary>
        ' <remarks>
        ' With the addition of procedure code whose length is less than 5 we
        '  may have issues with this sorting function.
        ' </remarks>
        ' <history>
        ' 	[paulw]	7/12/2006	Created
        '     [paulw] 9/29/2006   Modified to handle Int64.  Ran into problems when
        '                         I was using the wild card procedure (at the time of
        '                         writing this the wild card procedure is 'UFCW').
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim SortedProcedures As SortedList
        Dim Procedure As Procedure
        Dim Key As Int64

        Try
            SortedProcedures = New SortedList(Count)

            For I As Integer = 0 To Count - 1
                Procedure = DirectCast(Me(I), Procedure)
                Key = GetNumericValueOfProcedureCode(Procedure.ProcedureCode) * 100

                While SortedProcedures.ContainsKey(Key)
                    Key += 1
                End While

                SortedProcedures.Add(Key, Procedure)
            Next

            Clear()

            For I As Integer = 0 To SortedProcedures.Count - 1
                Add(DirectCast(SortedProcedures.GetByIndex(I), Procedure))
            Next

        Catch ex As Exception
            Throw
        Finally
            SortedProcedures = Nothing
            Procedure = Nothing
        End Try

    End Sub

    Public Function GetBestMatch(ByVal procedureCode As String, ByVal placeOfService As String, ByVal provider As String, ByVal diagnosis As String, ByVal diagnoses() As String, ByVal billType As String, ByVal modifier As String, ByVal gender As String, ByVal dateOfBirth As Date, ByVal dateOfService As Date, ByVal ruleSetType As Integer?, ByRef percent As Integer) As Procedure
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Finds the Active Procedure that most closely matches the criteria passed in.
        ' The closest matching procedure is returned and the percentMatch variable is
        '  populated with the percent that the procedure actually matched the paramterts,
        '  accorinding to the algorithm in this methos
        ' </summary>
        ' <param name="procedureCode"></param>
        ' <param name="placeOfService"></param>
        ' <param name="provider"></param>
        ' <param name="diagnosis"></param>
        ' <param name="billType"></param>
        ' <param name="modifier"></param>
        ' <param name="percentMatch"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/5/2006	Created
        '     [paulw] 7/12/2006   Modified for new Best match algorithm that was decided upon
        '                         in a meeting with Greg, George, Nick, Gary and myself.
        '     [paulw] 10/4/2006   Removed the throwing of the exception when the procedure code is not
        '                         valid per Nick Snyder.
        '     ' </history>
        ' -----------------------------------------------------------------------------

        SyncLock _GetBestMatchSyncLock

            Dim WildCardProcedures As Procedures
            Dim Procedure As Procedure
            Dim AgeInMonths As Integer
            Dim AgeInDays As Integer

            Dim Cntr As Integer = 0

            Dim chkPlanType As String

            Try

#If TRACE Then
                If CInt(_TraceParallel.Level) > 0 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : Procedure Count : " & Me.Count.ToString & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
#End If

                If Me.Count > 0 Then

                    chkPlanType = Me(0).PlanType

                    If Not WildCardProceduresAdded Then 'add wildcard procedures for plan

                        ' A0001

                        WildCardProcedures = PlanController.GetWildCardProcedureCollection(Me(0).PlanType, provider, CType(Me(0), ProcedureActive).SequenceNumber, dateOfService) 'Could this be improved by only bring back wilcard with s appropriate ruleset ?

                        If WildCardProcedures Is Nothing Then
                            Throw New ArgumentNullException("WildCardProcedureNotMatched")
                        End If

                        Parallel.ForEach(WildCardProcedures.Cast(Of ProcedureActive), Sub(ProcedureActive)
                                                                                          Add(ProcedureActive)
                                                                                          '_WildCardProceduresAdded = True
                                                                                          WildCardProceduresAdded = True
                                                                                      End Sub)

                        'For Each ProcedureActive As ProcedureActive In WildCardProcedures
                        '    Add(ProcedureActive)
                        '    _WildCardProceduresAdded = True
                        'Next

                    End If

                    percent = 0

                    If PlanController.IsValidProcedureCode(procedureCode, dateOfService) Then 'check is Procedure Code is in Lookup Table

                        Dim ClaimLineProcedureToCompare As ProcedureActive
                        Dim FullyMatchingProceduresAL As New ArrayList
                        Dim PartiallyMatchingProceduresAL As New ArrayList

                        AgeInMonths = CInt(DateDiff(DateInterval.Month, dateOfBirth, dateOfService))
                        AgeInDays = CInt(DateDiff(DateInterval.Day, dateOfBirth, dateOfService))

                        If AgeInMonths = 0 AndAlso AgeInDays > 0 Then AgeInMonths = 1

                        'create Procedure Object using current line item
                        ClaimLineProcedureToCompare = New ProcedureActive(-1, billType, diagnosisByDOS(diagnosis, dateOfService), modifier, gender, AgeInMonths, AgeInMonths, placeOfService, Me(0).PlanType, procedureCode, provider, New Rulesets)

                        'fastest
                        Parallel.ForEach(Me.Cast(Of ProcedureActive), Sub(ProcedureToCompareToRule)
                                                                          If Procedure.op_Equals(ClaimLineProcedureToCompare, ProcedureToCompareToRule) Then 'perfect match on all data elements
                                                                              If ProcedureToCompareToRule.FromDate <= dateOfService AndAlso ProcedureToCompareToRule.ToDate >= dateOfService Then
                                                                                  FullyMatchingProceduresAL.Add(ProcedureToCompareToRule) 'exact match on all line elements
                                                                              End If
                                                                          ElseIf IsPropertyWildCardMatch(ClaimLineProcedureToCompare, ProcedureToCompareToRule) Then
                                                                              If ProcedureToCompareToRule.FromDate <= dateOfService AndAlso ProcedureToCompareToRule.ToDate >= dateOfService Then
                                                                                  PartiallyMatchingProceduresAL.Add(ProcedureToCompareToRule) 'partial match on line elements
                                                                              End If
                                                                          End If
                                                                      End Sub)


                        'faster
                        'For Each ProcedureToCompareToRule As ProcedureActive In Me

                        '    If Procedure.op_Equals(ClaimLineProcedureToCompare, ProcedureToCompareToRule) Then 'perfect match on all data elements
                        '        If ProcedureToCompareToRule.FromDate <= dateOfService AndAlso ProcedureToCompareToRule.ToDate >= dateOfService Then
                        '            FullyMatchingProceduresAL.Add(ProcedureToCompareToRule) 'exact match on all line elements
                        '        End If
                        '    ElseIf IsPropertyWildCardMatch(ClaimLineProcedureToCompare, ProcedureToCompareToRule) Then
                        '        If ProcedureToCompareToRule.FromDate <= dateOfService AndAlso ProcedureToCompareToRule.ToDate >= dateOfService Then
                        '            PartiallyMatchingProceduresAL.Add(ProcedureToCompareToRule) 'partial match on line elements
                        '        End If
                        '    End If

                        'Next

                        'slowest
                        'Dim ProcedureToCompareToRule As ProcedureActive
                        'For I As Integer = 0 To Me.Count - 1 'loop through procedure code + wildcard combinations to identify rules that are relevant

                        '    ProcedureToCompareToRule = DirectCast(Me(I), ProcedureActive)

                        '    If Procedure.op_Equals(ClaimLineProcedureToCompare, ProcedureToCompareToRule) Then 'perfect match on all data elements
                        '        If ProcedureToCompareToRule.FromDate <= dateOfService AndAlso ProcedureToCompareToRule.ToDate >= dateOfService Then
                        '            FullyMatchingProceduresAL.Add(ProcedureToCompareToRule) 'exact match on all line elements
                        '        End If
                        '    ElseIf IsPropertyWildCardMatch(ClaimLineProcedureToCompare, ProcedureToCompareToRule) Then
                        '        If ProcedureToCompareToRule.FromDate <= dateOfService AndAlso ProcedureToCompareToRule.ToDate >= dateOfService Then
                        '            PartiallyMatchingProceduresAL.Add(ProcedureToCompareToRule) 'partial match on line elements
                        '        End If
                        '    End If
                        'Next

                        'establish best item
                        If FullyMatchingProceduresAL.Count > 0 Then 'exact match on all line elements
                            Procedure = GetBestWeight(ClaimLineProcedureToCompare, FullyMatchingProceduresAL, percent)
                        Else
                            Procedure = GetBestWeight(ClaimLineProcedureToCompare, PartiallyMatchingProceduresAL, percent)
                        End If

                        If Procedure Is Nothing Then
                            Throw New ArgumentNullException("claimProcedureNotMatched")
                        End If

                        Procedure = PlanController.GetProcedureActive(Procedure.ProcedureID) 'get complete rule 1st from cache and then from db
                        If Procedure Is Nothing Then
                            Throw New ArgumentNullException("claimProcedureNotFound")
                        End If

                        'loop through matches until a procedure is found with a matching procedure
                        While Not ProcedureHasRuleSet(Procedure, ruleSetType) 'search for procedure which matches the current rulesettype

                            If Cntr = Count Then Exit While 'if no rules are found could only happen if partial and full included every matching possibility

                            UFCWLastKeyData.TEXT = Procedure.ToString

                            If FullyMatchingProceduresAL.Count > 0 Then

                                For I As Integer = 0 To FullyMatchingProceduresAL.Count - 1 'search through full matches, find the item that failed the test and remove it in preperation for the next test
                                    If CType(FullyMatchingProceduresAL(I), Procedure).BillType = Procedure.BillType AndAlso
                                    CType(FullyMatchingProceduresAL(I), Procedure).Diagnosis = Procedure.Diagnosis AndAlso
                                    CType(FullyMatchingProceduresAL(I), Procedure).Modifier = Procedure.Modifier AndAlso
                                    CType(FullyMatchingProceduresAL(I), Procedure).PlaceOfService = Procedure.PlaceOfService AndAlso
                                    CType(FullyMatchingProceduresAL(I), Procedure).PlanType = Procedure.PlanType AndAlso
                                    CType(FullyMatchingProceduresAL(I), Procedure).ProcedureCode = Procedure.ProcedureCode AndAlso
                                    CType(FullyMatchingProceduresAL(I), Procedure).Gender = Procedure.Gender Then

                                        ' if rule date is in use check if claim date falls between rule range, which is considered a positive match
                                        If CType(FullyMatchingProceduresAL(I), Procedure).MonthsMin > 0 OrElse CType(FullyMatchingProceduresAL(I), Procedure).MonthsMax > 0 Then
                                            If (Procedure.MonthsMin >= CType(FullyMatchingProceduresAL(I), Procedure).MonthsMin AndAlso Procedure.MonthsMax <= CType(FullyMatchingProceduresAL(I), Procedure).MonthsMax) OrElse (Procedure.MonthsMin = 0 AndAlso Procedure.MonthsMax = 0) Then
                                                FullyMatchingProceduresAL.RemoveAt(I)
                                                Exit For
                                            End If
                                        Else
                                            FullyMatchingProceduresAL.RemoveAt(I)
                                            Exit For
                                        End If

                                    End If
                                Next

                                Procedure = GetBestWeight(ClaimLineProcedureToCompare, FullyMatchingProceduresAL, percent) ' with non matching ruleset item removed repeat ruleset test

                            Else
                                For I As Integer = 0 To PartiallyMatchingProceduresAL.Count - 1
                                    If CType(PartiallyMatchingProceduresAL(I), Procedure).BillType = Procedure.BillType AndAlso
                                    CType(PartiallyMatchingProceduresAL(I), Procedure).Diagnosis = Procedure.Diagnosis AndAlso
                                    CType(PartiallyMatchingProceduresAL(I), Procedure).Modifier = Procedure.Modifier AndAlso
                                    CType(PartiallyMatchingProceduresAL(I), Procedure).PlaceOfService = Procedure.PlaceOfService AndAlso
                                    CType(PartiallyMatchingProceduresAL(I), Procedure).PlanType = Procedure.PlanType AndAlso
                                    CType(PartiallyMatchingProceduresAL(I), Procedure).ProcedureCode = Procedure.ProcedureCode AndAlso
                                    CType(PartiallyMatchingProceduresAL(I), Procedure).Gender = Procedure.Gender Then

                                        ' if rule date is in use check if claim date falls between rule range, which is considered a positive match
                                        If CType(PartiallyMatchingProceduresAL(I), Procedure).MonthsMin > 0 OrElse CType(PartiallyMatchingProceduresAL(I), Procedure).MonthsMax > 0 Then
                                            If (Procedure.MonthsMin >= CType(PartiallyMatchingProceduresAL(I), Procedure).MonthsMin AndAlso Procedure.MonthsMax <= CType(PartiallyMatchingProceduresAL(I), Procedure).MonthsMax) OrElse (Procedure.MonthsMin = 0 AndAlso Procedure.MonthsMax = 0) Then
                                                PartiallyMatchingProceduresAL.RemoveAt(I)
                                                Exit For
                                            End If
                                        Else

                                            PartiallyMatchingProceduresAL.RemoveAt(I)
                                            Exit For
                                        End If

                                    End If
                                Next

                                Procedure = GetBestWeight(ClaimLineProcedureToCompare, PartiallyMatchingProceduresAL, percent)
                            End If

                            If Procedure Is Nothing Then
                                Return Nothing
                            End If

                            Procedure = PlanController.GetProcedureActive(Procedure.ProcedureID)

                        End While
                    End If

                    If ProcedureHasRuleSet(Procedure, ruleSetType) Then
                        Return Procedure
                    End If

                    Return Nothing

                End If

            Catch ex As Exception
                Throw
            Finally

#If TRACE Then
                If CInt(_TraceParallel.Level) > 0 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(2).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(2).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
#End If

                Procedure = Nothing
                WildCardProcedures = Nothing

            End Try

        End SyncLock

    End Function

    Public Sub Add(ByVal theItem As IProcedure)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Adds a procedure to the collection
        ' </summary>
        ' <param name="item"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        SyncLock _AddSyncLock
            Try

                If theItem Is Nothing Then Throw New ArgumentNullException("item")

                Me.List.Add(theItem)


                'Dim DR As DataRow = _IndexKeyCrossRefDT.NewRow
                'DR("ListIndex") = Me.List.Add(item)
                'DR("PlanType") = item.PlanType
                'DR("SequenceNumber") = CType(item, ProcedureActive).SequenceNumber

                '    _IndexKeyCrossRefDT.Rows.Add(DR)

            Catch ex As Exception
                Throw
            Finally
#If TRACE Then
                If CInt(_TraceParallel.Level) > 3 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
#End If
            End Try
        End SyncLock


    End Sub

    Public Sub Remove(ByVal theItem As IProcedure)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' removes a procedure from the collection
        ' </summary>
        ' <param name="item"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Me.List.Remove(theItem)

    End Sub

    Public Function Contains(ByVal theItem As IProcedure) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Determines if a procedure already exists in this collection, based off of
        '  what we know to be a matching procedure.
        ' </summary>
        ' <param name="item"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            If theItem Is Nothing Then Throw New ArgumentNullException("item")

            For Each TestProcedure As Procedure In Me
                If theItem.BillType = TestProcedure.BillType AndAlso theItem.Diagnosis(0) = TestProcedure.Diagnosis(0) AndAlso theItem.Modifier = TestProcedure.Modifier AndAlso
                    theItem.PlaceOfService = TestProcedure.PlaceOfService AndAlso theItem.PlanType = TestProcedure.PlanType AndAlso theItem.ProcedureCode = TestProcedure.ProcedureCode AndAlso
                    theItem.Provider = TestProcedure.Provider AndAlso theItem.Gender = TestProcedure.Gender Then 'todo add age logic
                    Return True
                End If

            Next


            'Dim Procedure As Procedure
            'For I As Integer = 0 To Me.List.Count - 1

            '    Procedure = DirectCast(Me.List(I), Procedure)

            '    If item.BillType = Procedure.BillType AndAlso item.Diagnosis(0) = Procedure.Diagnosis(0) AndAlso item.Modifier = Procedure.Modifier AndAlso _
            '        item.PlaceOfService = Procedure.PlaceOfService AndAlso item.PlanType = Procedure.PlanType AndAlso item.ProcedureCode = Procedure.ProcedureCode AndAlso _
            '        item.Provider = Procedure.Provider AndAlso item.Gender = Procedure.Gender Then 'todo add age logic
            '        Return True
            '    End If

            'Next

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Function

    Private Function GetNumericValueOfProcedureCode(ByVal procCode As String) As Int64
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Converts the procedure code into a numeric
        ' </summary>
        ' <param name="procCode"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	7/12/2006	Created
        '     [paulw] 9/29/2006   Modified to handle Int64.  Ran into problems when
        '                         I was using the wild card procedure (at the time of
        '                         writing this the wild card procedure is 'UFCW').
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim IsCharAnAlpha As Boolean
        Dim IntString As String = ""
        Dim EndPosition As Integer = procCode.Length
        Dim Position As Integer = 0

        Try

            While Position < EndPosition
                If IsNumeric(procCode) Then
                    Return Convert.ToInt64(procCode)
                Else
                    IsCharAnAlpha = IsNumeric(procCode.Substring(Position, 1))
                    If IsCharAnAlpha Then
                        IntString += procCode.Substring(Position, 1).ToString
                    Else
                        IntString += Asc(procCode.Substring(Position, 1)).ToString()
                    End If
                End If
                Position += 1
            End While

            Return Convert.ToInt64(IntString)

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Shared Function ProcedureHasRuleSet(ByVal theProcedure As IProcedure, ByVal theRuleSetType As Integer?) As Boolean

        Try

            If theProcedure Is Nothing Then Return False
            If theProcedure.RuleSets Is Nothing Then Return False
            If theProcedure.RuleSets.Count = 0 Then Return False

            For Each Ruleset As RuleSet In theProcedure.RuleSets
                If Ruleset.RulesetType = theRuleSetType Then
                    Return True
                End If
            Next

            Return False

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Function GetBestWeight(ByVal procedureClaimLineItem As Procedure, ByVal proceduresToWeight As ArrayList, ByRef percent As Integer) As Procedure
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' based off the provided procedure, this function gets
        '  the best wighted match for that procedure.  the
        '  weight for each property comes from a config file.
        ' </summary>
        ' <param name="procCompare"></param>
        ' <param name="matchingProcs"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	7/13/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim Matches As New ArrayList
        Dim MatchesWeight As New ArrayList
        Dim Weight As Integer
        Dim MatchingProcRule As Procedure

        Try

            'loop through all the matching procedures

            For I As Integer = 0 To proceduresToWeight.Count - 1

                If proceduresToWeight(I) Is Nothing Then Continue For

                MatchingProcRule = DirectCast(proceduresToWeight(I), Procedure)

                Weight = 0
                If MatchingProcRule.BillType = procedureClaimLineItem.BillType Then
                    Weight += _BillTypeWeight
                    'Debug.Print("+BillType")
                End If
                If MatchingProcRule.Diagnosis = procedureClaimLineItem.Diagnosis Then
                    Weight += _DiagnosisWeight
                    'Debug.Print("+Diagnosis")
                End If
                If MatchingProcRule.Modifier = procedureClaimLineItem.Modifier Then
                    Weight += _ModifierWeight
                    'Debug.Print("+Modifier")
                End If
                If MatchingProcRule.PlaceOfService = procedureClaimLineItem.PlaceOfService Then
                    Weight += _PlaceOfServiceWeight
                    'Debug.Print("+PlaceOfService")
                End If
                If MatchingProcRule.Provider = procedureClaimLineItem.Provider Then
                    Weight += _ProviderWeight
                    'Debug.Print("+Provider")
                End If
                If MatchingProcRule.Gender = procedureClaimLineItem.Gender Then
                    Weight += _GenderWeight
                    'Debug.Print("+Gender")
                End If

                'if rule has no date component increase the weighting of the procedure to correctly prioritize the rule against others
                'if rule has no upper limit test if min age has been met
                'if age falls between rule
                If (MatchingProcRule.MonthsMin = 0 AndAlso MatchingProcRule.MonthsMax = 0) OrElse
                    (MatchingProcRule.MonthsMax = 0 AndAlso procedureClaimLineItem.MonthsMin >= MatchingProcRule.MonthsMin) OrElse
                    (procedureClaimLineItem.MonthsMin >= MatchingProcRule.MonthsMin AndAlso procedureClaimLineItem.MonthsMax <= MatchingProcRule.MonthsMax) Then
                    Weight += _DateOfBirthWeight

                    If (MatchingProcRule.MonthsMin > 0 OrElse MatchingProcRule.MonthsMax > 0) Then 'give a rule with a dob component a slight edge
                        Weight += 1
                    End If
#If DEBUG Then
                    Debug.Print("+DOB Line Info: " & procedureClaimLineItem.MonthsMin.ToString & " - " & procedureClaimLineItem.MonthsMax.ToString & " Rule Info: " & MatchingProcRule.MonthsMin.ToString & " - " & MatchingProcRule.MonthsMax)
#End If
                End If

                If MatchingProcRule.ProcedureCode <> PlanController.WildCardProcedure Then Weight += _ProcedureWeight 'give real rule the edge over WildCard

                'we want to add it, and its weight to a collections
                'If matches.Count = 0 Then
                MatchingProcRule.Weight = Weight
                Matches.Add(MatchingProcRule)
                MatchesWeight.Add(Weight.ToString)
#If debug Then
                Debug.Print("Tested against: " & MatchingProcRule.ProcedureID & " - TotalWeight " & Weight.ToString & " - " & procedureClaimLineItem.BillType.ToString & " - " & procedureClaimLineItem.Diagnosis.ToString & " - " & procedureClaimLineItem.Modifier.ToString & " - " & procedureClaimLineItem.PlaceOfService.ToString & " - " & procedureClaimLineItem.Provider.ToString & " - " & procedureClaimLineItem.Gender.ToString & " - " & procedureClaimLineItem.MonthsMin.ToString & " - " & procedureClaimLineItem.MonthsMax.ToString)
#End If

            Next

            'get the max weight of the procedures.
            Dim MaxWeight As Integer = _BillTypeWeight + _DiagnosisWeight + _ModifierWeight + _PlaceOfServiceWeight + _ProviderWeight + _DateOfBirthWeight + _GenderWeight + _ProcedureWeight + 1
            Dim MatchFound As Boolean = False

            Dim QuerySortedRules = From rules In Matches.OfType(Of Procedure)()
                                   Order By rules.Weight Descending

            Dim QueryNonUFCWRules = From rules In QuerySortedRules.OfType(Of Procedure)()
                                    Where rules.ProcedureCode <> "UFCW"
                                    Order By rules.Weight Descending

            Dim QueryUFCWRules = From rules In QuerySortedRules.OfType(Of Procedure)()
                                 Where rules.ProcedureCode = "UFCW"
                                 Order By rules.Weight Descending

            ''start counting down and resequence matching rules highest to lowest match weight
            'For i As Integer = max To 0 Step -1
            '    For j As Integer = 0 To matchesWeight.Count - 1
            '        If Convert.ToInt32(matchesWeight(j)) = i Then
            '            percent = Convert.ToInt32((i / max) * 100)
            '            SortedProcedures.Add(DirectCast(matches(j), Procedure))
            '            matchFound = True
            '        End If
            '    Next
            'Next

            'Dim ReturnProc As Procedure
            'Dim FirstWildCardMatchLocation As Integer = -1
            'Dim FirstRealMatchLocation As Integer = -1

            'If matchFound AndAlso SortedProcedures.Count > 0 Then

            '    For i As Integer = SortedProcedures.Count - 1 To 0 Step -1
            '        If FirstWildCardMatchLocation = -1 AndAlso SortedProcedures(i).ProcedureCode = PlanController.WildCardProcedure Then
            '            FirstWildCardMatchLocation = i
            '        ElseIf FirstRealMatchLocation = -1 Then
            '            FirstRealMatchLocation = i
            '        End If
            '        If FirstRealMatchLocation > -1 AndAlso FirstWildCardMatchLocation > -1 Then Exit For
            '    Next

            '    If FirstRealMatchLocation > -1 Then
            '        returnProc = CType(SortedProcedures(FirstRealMatchLocation), Procedure)
            '        'Debug.Print("Best Weighted (Real):" & returnProc.ToString)
            '    Else
            '        returnProc = CType(SortedProcedures(FirstWildCardMatchLocation), Procedure)
            '        'Debug.Print("Best Weighted (Wild):" & returnProc.ToString)
            '    End If

            'End If

            If QueryNonUFCWRules.Any Then
                Return CType(QueryNonUFCWRules(0), Procedure)
            ElseIf QueryUFCWRules.any Then
                Return CType(QueryUFCWRules(0), Procedure)
            End If

        Catch ex As Exception

            Throw

        Finally
            Matches = Nothing
            MatchesWeight = Nothing
            MatchingProcRule = Nothing

        End Try
    End Function

    Private Function GetHighestSequenceNumber(ByVal procedureCollection As Procedures) As Procedure
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets the highest sequence number procedure.
        ' </summary>
        ' <param name="procCollection"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim ProcedureToReturn As ProcedureActive
        Dim Procedure As ProcedureActive

        Try

            For I As Integer = 0 To procedureCollection.Count - 1
                Procedure = DirectCast(procedureCollection(I), ProcedureActive)
                If ProcedureToReturn Is Nothing Then
                    ProcedureToReturn = Procedure
                Else
                    If Procedure.SequenceNumber > ProcedureToReturn.SequenceNumber Then
                        ProcedureToReturn = Procedure
                    End If
                End If
            Next

            Return ProcedureToReturn
        Catch ex As Exception
            Throw
        Finally
            ProcedureToReturn = Nothing
            Procedure = Nothing
        End Try

    End Function

    Private Function IsPropertyWildCardMatch(ByVal claimProcedure As Procedure, ByVal ruleProcedure As ProcedureActive) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' determines if this procCompare matches an existing procedure based off
        '  any wildcard (property = "") matches.  In other words, if the rule
        '  procedure is "" for the property (other than procedure code), then the
        '  claim procedure can be anything. if the ruleProcedure is not ""
        '  then it must match exactly.
        ' </summary>
        ' <param name="procCompare"></param>
        ' <param name="procExisting"></param>
        ' <returns></returns>
        ' <remarks>
        ' the logic in the code basically says this: go through all propertys
        ' if it is not a blank property
        '     if it is not the same value for the property
        '         return false
        '     keep going
        ' once all properties have been checked and it makes it
        '  to then end without having already returned false
        ' then return true
        ' </remarks>
        ' <history>
        ' 	[paulw]	7/12/2006	Created
        '     [paulw] 10/3/2006   Changed the logic so that Provider is not necessary.
        '                         This was due to the changes that the wild card procedure
        '                         brought about.
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            If ruleProcedure.ProcedureCode = claimProcedure.ProcedureCode OrElse ruleProcedure.ProcedureCode = PlanController.WildCardProcedure Then

                If ruleProcedure.BillType <> "" Then
                    If ruleProcedure.BillType <> claimProcedure.BillType Then
                        Return False
                    End If
                End If

                If ruleProcedure.Diagnosis <> "" Then
                    If claimProcedure.Diagnosis.Length = 0 OrElse ruleProcedure.Diagnosis <> claimProcedure.Diagnosis(0) Then
                        Return False
                    End If
                End If

                If ruleProcedure.Modifier <> "" Then
                    If ruleProcedure.Modifier <> claimProcedure.Modifier Then
                        Return False
                    End If
                End If

                If ruleProcedure.PlaceOfService <> "" Then
                    If ruleProcedure.PlaceOfService <> claimProcedure.PlaceOfService Then
                        Return False
                    End If
                End If

                If ruleProcedure.Provider <> "" Then
                    If ruleProcedure.Provider <> claimProcedure.Provider Then
                        Return False
                    End If
                End If

                If ruleProcedure.Gender <> "" Then
                    If ruleProcedure.Gender <> claimProcedure.Gender Then
                        Return False
                    End If
                End If

                If ruleProcedure.MonthsMin > 0 Then
                    If claimProcedure.MonthsMin < ruleProcedure.MonthsMin Then
                        Return False
                    End If
                End If

                If ruleProcedure.MonthsMax > 0 Then
                    If claimProcedure.MonthsMax > ruleProcedure.MonthsMax Then
                        Return False
                    End If
                End If

                Return True
            End If

            Return False

        Catch ex As Exception
            Throw
        End Try
    End Function


    'Public Function IndexOf(ByVal item As IProcedure) As Int32
    '    ' -----------------------------------------------------------------------------
    '    ' <summary>
    '    ' Gets the index of a particular procedure
    '    ' </summary>
    '    ' <param name="item"></param>
    '    ' <returns></returns>
    '    ' <remarks>
    '    ' </remarks>
    '    ' <history>
    '    ' 	[paulw]	1/25/2006	Created
    '    ' </history>
    '    ' -----------------------------------------------------------------------------

    '    Dim Procedure As Procedure

    '    Try

    '        If Item Is Nothing Then Throw New ArgumentNullException("item")

    '        For I As Integer = 0 To Me.List.Count - 1
    '            Procedure = DirectCast(Me.List(I), Procedure)
    '            If item.BillType = Procedure.BillType AndAlso item.Diagnosis(0) = Procedure.Diagnosis(0) AndAlso item.Modifier = Procedure.Modifier AndAlso _
    '                item.PlaceOfService = Procedure.PlaceOfService AndAlso item.PlanType = Procedure.PlanType AndAlso item.ProcedureCode = Procedure.ProcedureCode AndAlso _
    '                item.Provider = Procedure.Provider AndAlso item.Gender = Procedure.Gender Then 'todo add age logic
    '                Return I
    '            End If
    '        Next

    '    Catch ex As Exception
    '        Throw
    '    Finally
    '        Procedure = Nothing
    '    End Try
    'End Function

    Protected Overrides Sub OnValidate(ByVal value As Object)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Overriding the OnValidate allows for the confirmation of a strongly typed
        ' 'item' as a more generic IList object.
        ' </summary>
        ' <param name="item"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[GregS]	1/8/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            MyBase.OnValidate(value)
            If Not (TypeOf (value) Is IProcedure) Then
                Throw New ArgumentException("Collection only supports objects implementing IProcedure.")
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Shared Function diagnosisByDOS(diagnosis As String, dateOfService As Date) As String

        If dateOfService < CDate("10/01/2015") Then
            Return diagnosis
        End If

        Dim QueryDiagXRef = CMSDALFDBMD.RetrieveDiagnosisXRefEffectiveAsOf(dateOfService).Tables(0).AsEnumerable().Where(Function(r) r.Field(Of String)("DIAG_VALUE_ICD10") = diagnosis.ToUpper)

        Dim DR As DataRow = QueryDiagXRef.FirstOrDefault()

        If DR IsNot Nothing Then
            Return DR("DIAG_VALUE_ICD9").ToString
        Else
            Return diagnosis
        End If

    End Function

#End Region

#Region "Required Overrides"
    'Protected Overrides Function GetKeyForItem( _
    '        ByVal item As Procedure) As Tuple

    '    ' In this example, the key is the part number. 
    '    Return item.PartNumber
    'End Function

#End Region


End Class