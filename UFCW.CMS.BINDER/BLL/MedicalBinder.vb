Option Strict On

Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Binder
''' Class	 : CMS.Binder.MedicalBinder
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' A Medical Binder
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/4/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> Public Class MedicalBinder
    Inherits Binder

#Region "Private Variables"

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _MedicalBinderGuid As System.Guid = Guid.NewGuid()
    Private _IsAccident As Boolean

#End Region

#Region "Constructors"
    Private Sub New()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Hide the default constructor
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        MyBase.New()
    End Sub

    Public Sub New(ByVal claimId As Integer, ByVal typeOfBinder As Integer)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Default Constructor
        ' </summary>
        ' <param name="claimId"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        MyBase.New()

#If debug Then
        Debug.Print("MedicalBinder New: " & _MedicalBinderGuid.ToString)
#End If

        _ClaimID = claimId
        _TypeOfBinder = typeOfBinder
        _DocumentClass = "MEDICAL"

    End Sub

    Public Sub New(ByVal claimID As Integer, ByVal typeOfBinder As Integer, ByVal isAccident As Boolean)

        MyBase.New()
        _ClaimID = claimID
        _TypeOfBinder = typeOfBinder
        _DocumentClass = "MEDICAL"
        _IsAccident = isAccident

    End Sub
#End Region

#Region "Methods"
    Public Overrides Sub RemoveAccidentAccumulators()

        Dim BinderItem As BinderItem
        Dim BinderItemEnum As IDictionaryEnumerator

        Try

            BinderItemEnum = _BinderItems.GetEnumerator()

            While BinderItemEnum.MoveNext()
                BinderItem = CType(BinderItemEnum.Value, BinderItem)
                BinderItem.RemoveAccidentAccumulators()
            End While

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Overrides Sub ReplaceAccidentAccumulator(ByVal accumName As String)

        Dim BinderItem As BinderItem
        Dim BinderItemEnum As IDictionaryEnumerator

        Try

            BinderItemEnum = _BinderItems.GetEnumerator()

            While BinderItemEnum.MoveNext()
                BinderItem = CType(BinderItemEnum.Value, BinderItem)
                BinderItem.ReplaceAccidentAccumulator(accumName)
            End While

        Catch ex As Exception
            Throw
        Finally
            BinderItemEnum = Nothing

        End Try
    End Sub

    Public Overrides Function NewBinderItem() As BinderItem
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' returns a new medical binder item
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/28/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Return New MedicalBinderItem

    End Function

    Public Overloads Overrides Function GetAccumulatorSummary() As DataTable
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets the Accumulator Summary Datatable
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/31/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim AccumulatorsDT As DataTable
        Dim DR As DataRow
        Dim AccumDV As DataView
        Dim AccumOrderDV As DataView
        Dim LineDate As Date?
        Dim TheAccumulators As Conditions

        Dim Enabled As Boolean = True

        Try

            AccumulatorsDT = BuildAccumulators()

            AccumDV = New DataView(AccumulatorsDT, "", "ACCUM_NAME", DataViewRowState.CurrentRows)
            AccumOrderDV = New DataView(AccumulatorController.GetAccumulators, "", "ACCUM_NAME", DataViewRowState.CurrentRows)

            TheAccumulators = GetClaimConditions()

            If _Facts IsNot Nothing Then
                For Each TheFact As Fact In _Facts
                    For Each DE As DictionaryEntry In TheAccumulators
                        AccumOrderDV.RowFilter = "ACCUM_NAME = '" & DirectCast(DE.Value, Condition).AccumulatorName & "'"
                        If DirectCast(DE.Value, Condition).DurationType = DateType.Rollover AndAlso DirectCast(DE.Value, Condition).Duration = 0 Then
                            LineDate = CDate(Format(TheFact.LineDate, "12-31-yyyy"))
                            AccumDV.RowFilter = "ACCUM_NAME = '" & DirectCast(DE.Value, Condition).AccumulatorName & "' AND YEAR = '" & LineDate.ToString & "'"
                        ElseIf DirectCast(DE.Value, Condition).DurationType = DateType.Years AndAlso DirectCast(DE.Value, Condition).Duration = 0 Then
                            LineDate = CDate(Format(TheFact.LineDate, "12-31-yyyy"))
                            AccumDV.RowFilter = "ACCUM_NAME = '" & DirectCast(DE.Value, Condition).AccumulatorName & "' AND YEAR = '" & LineDate.ToString & "'"
                        ElseIf DirectCast(DE.Value, Condition).DurationType = DateType.Years AndAlso DirectCast(DE.Value, Condition).Duration > 50 Then
                            LineDate = Nothing ' CDate(Format(f.LineDate, "12-31-yyyy"))
                            AccumDV.RowFilter = "ACCUM_NAME = '" & DirectCast(DE.Value, Condition).AccumulatorName & "'" ' &  AND YEAR = '" & lineDate & "'"
                        ElseIf DirectCast(DE.Value, Condition).DurationType = DateType.Years AndAlso (DirectCast(DE.Value, Condition).Duration = 1 OrElse DirectCast(DE.Value, Condition).Duration = 2 OrElse DirectCast(DE.Value, Condition).Duration = 4) Then
                            LineDate = CDate(Format(TheFact.LineDate, "12-31-yyyy"))
                            AccumDV.RowFilter = "ACCUM_NAME = '" & DirectCast(DE.Value, Condition).AccumulatorName & "'" ' AND YEAR = '" & lineDate & "'"
                        ElseIf DirectCast(DE.Value, Condition).DurationType = DateType.Days Then
                            LineDate = Nothing
                            AccumDV.RowFilter = "ACCUM_NAME = '" & DirectCast(DE.Value, Condition).AccumulatorName & "'"
                        Else
                            LineDate = Nothing
                            AccumDV.RowFilter = "ACCUM_NAME = '" & DirectCast(DE.Value, Condition).AccumulatorName & "'"
                        End If

                        If AccumDV.Count = 0 Then
                            DR = AccumulatorsDT.NewRow
                            DR("ACCUM_NAME") = DirectCast(DE.Value, Condition).AccumulatorName
                            If AccumOrderDV.Count = 0 Then
                                DR("PRIORITY") = 9999999
                            Else
                                DR("PRIORITY") = AccumOrderDV(0)("DISPLAY_ORDER")
                            End If
                            If LineDate Is Nothing Then
                                DR("YEAR") = System.DBNull.Value
                            Else
                                DR("YEAR") = LineDate
                            End If

                            If DirectCast(DE.Value, Condition).DurationType = DateType.Rollover AndAlso DirectCast(DE.Value, Condition).Duration = 0 Then
                                DR("ORIGINAL_VALUE") = Me.BinderAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName)), DirectCast(DE.Value, Condition).DurationType, DirectCast(DE.Value, Condition).Duration, LineDate, DirectCast(DE.Value, Condition).Direction)
                                DR("PROPOSED_VALUE") = Me.BinderAccumulatorManager.GetProposedValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName)), DirectCast(DE.Value, Condition).DurationType, DirectCast(DE.Value, Condition).Duration, LineDate, DirectCast(DE.Value, Condition).Direction)
                            ElseIf DirectCast(DE.Value, Condition).DurationType = DateType.Years AndAlso DirectCast(DE.Value, Condition).Duration = 0 Then
                                DR("ORIGINAL_VALUE") = Me.BinderAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName)), DirectCast(DE.Value, Condition).DurationType, DirectCast(DE.Value, Condition).Duration, LineDate, DirectCast(DE.Value, Condition).Direction)
                                DR("PROPOSED_VALUE") = Me.BinderAccumulatorManager.GetProposedValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName)), DirectCast(DE.Value, Condition).DurationType, DirectCast(DE.Value, Condition).Duration, LineDate, DirectCast(DE.Value, Condition).Direction)
                            ElseIf DirectCast(DE.Value, Condition).DurationType = DateType.Years AndAlso DirectCast(DE.Value, Condition).Duration > 50 Then
                                DR("ORIGINAL_VALUE") = Me.BinderAccumulatorManager.GetOriginalLifetimeValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName))) ', rc.DurationType, rc.Duration, lineDate, rc.Direction)
                                DR("PROPOSED_VALUE") = Me.BinderAccumulatorManager.GetProposedLifetimeValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName))) ', rc.DurationType, rc.Duration, lineDate, rc.Direction)
                            ElseIf DirectCast(DE.Value, Condition).DurationType = DateType.Years AndAlso DirectCast(DE.Value, Condition).Duration > 3 AndAlso DirectCast(DE.Value, Condition).Duration < 1000 Then
                                Dim Val As Decimal
                                'get the value from 5 year ago to the date of service
                                Val = BinderAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName)), DirectCast(DE.Value, Condition).DurationType, DirectCast(DE.Value, Condition).Duration, LineDate, DirectCast(DE.Value, Condition).Direction)
                                'get the value from the date of service to today
                                Val += BinderAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName)), DateType.Days, CInt(DateDiff(DateInterval.Day, CDate(TheFact.LineDate).AddDays(1), UFCWGeneral.NowDate)), CDate(LineDate).AddDays(1), DateDirection.Forward)
                                DR("ORIGINAL_VALUE") = Val
                                'get the value from 3 year ago to the date of service
                                Val = BinderAccumulatorManager.GetProposedValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName)), DirectCast(DE.Value, Condition).DurationType, DirectCast(DE.Value, Condition).Duration, LineDate, DirectCast(DE.Value, Condition).Direction)
                                'get the value from the date of service to today
                                Val += BinderAccumulatorManager.GetProposedValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName)), DateType.Days, CInt(DateDiff(DateInterval.Day, CDate(TheFact.LineDate).AddDays(1), UFCWGeneral.NowDate)), CDate(LineDate).AddDays(1), DateDirection.Forward)
                                DR("PROPOSED_VALUE") = Val
                            ElseIf DirectCast(DE.Value, Condition).DurationType = DateType.Years AndAlso DirectCast(DE.Value, Condition).Duration > 0 AndAlso DirectCast(DE.Value, Condition).Duration < 1000 Then
                                Dim Val As Decimal
                                'get the value from 3 year ago to the date of service
                                Val = BinderAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName)), DirectCast(DE.Value, Condition).DurationType, DirectCast(DE.Value, Condition).Duration, LineDate, DirectCast(DE.Value, Condition).Direction)
                                'get the value from the date of service to today
                                Val += BinderAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName)), DateType.Days, CInt(DateDiff(DateInterval.Day, CDate(TheFact.LineDate).AddDays(1), UFCWGeneral.NowDate)), CDate(LineDate).AddDays(1), DateDirection.Forward)
                                DR("ORIGINAL_VALUE") = Val
                                'get the value from 3 year ago to the date of service
                                Val = BinderAccumulatorManager.GetProposedValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName)), DirectCast(DE.Value, Condition).DurationType, DirectCast(DE.Value, Condition).Duration, LineDate, DirectCast(DE.Value, Condition).Direction)
                                'get the value from the date of service to today
                                Val += BinderAccumulatorManager.GetProposedValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName)), DateType.Days, CInt(DateDiff(DateInterval.Day, CDate(TheFact.LineDate).AddDays(1), UFCWGeneral.NowDate)), CDate(LineDate).AddDays(1), DateDirection.Forward)
                                DR("PROPOSED_VALUE") = Val
                            ElseIf CStr(DR("ACCUM_NAME")).Trim.StartsWith("AC") Then
                                DR("ORIGINAL_VALUE") = Me.BinderAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName)), DirectCast(DE.Value, Condition).DurationType, DirectCast(DE.Value, Condition).Duration, TheFact.IncidentDate, DirectCast(DE.Value, Condition).Direction)
                                DR("PROPOSED_VALUE") = Me.BinderAccumulatorManager.GetProposedValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName)), DirectCast(DE.Value, Condition).DurationType, DirectCast(DE.Value, Condition).Duration, TheFact.IncidentDate, DirectCast(DE.Value, Condition).Direction)
                            End If

                            AccumulatorsDT.Rows.Add(DR)
                        End If
                        'End If
                    Next 'rc
                Next 'f
            End If

            AccumulatorsDT = GetAdditionalAccumulatorData(AccumulatorsDT)

            Return AccumulatorsDT

        Catch ex As Exception
            Throw
        Finally

            If AccumDV IsNot Nothing Then
                AccumDV.Dispose()
            End If
            AccumDV = Nothing

            If AccumOrderDV IsNot Nothing Then
                AccumOrderDV.Dispose()
            End If
            AccumOrderDV = Nothing

            DR = Nothing
            TheAccumulators = Nothing

        End Try

    End Function

    Public Overloads Overrides Function GetAccumulatorSummaryCommitted(ByVal relevantDate As Date) As DataTable
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' gets committed accumulator summary for a claim
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	2/12/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim AccumulatorsDT As DataTable
        Dim ClaimAccumulatorEntriesDT As DataTable
        Dim ClaimAccumulatorEntriesDV As DataView
        Dim ClaimLineAccumulatorTotalDV As DataView
        Dim SumDV As DataView
        Dim LineDate As Date?
        Dim PlanAccumulatorsDT As DataTable
        Dim PlanAccumulatorsDV As DataView
        Dim DR As DataRow

        Dim AccumulatorName As String = ""
        Dim TotalAccum As Decimal
        Dim OriginalAccum As Decimal?

        Try

            AccumulatorsDT = BuildAccumulators()
            SumDV = New DataView(AccumulatorsDT, "", "ACCUM_NAME", DataViewRowState.CurrentRows)

            'AccumConditions = PlanController.GetConditionsOfPlanForDOS(relevantDate)

            PlanAccumulatorsDT = PlanController.GetAllPlanAccumulators
            PlanAccumulatorsDV = New DataView(PlanAccumulatorsDT, "", "ACCUM_NAME", DataViewRowState.CurrentRows)

            _BinderAccumulatorManager.RefreshAccumulatorSummariesForMember()

            ClaimAccumulatorEntriesDT = _BinderAccumulatorManager.GetAccumulatorEntryValues(False, _ClaimID)

            ClaimAccumulatorEntriesDV = New DataView(ClaimAccumulatorEntriesDT, "", "LINE_NBR", DataViewRowState.CurrentRows)
            ClaimLineAccumulatorTotalDV = New DataView(ClaimAccumulatorEntriesDT, "", "LINE_NBR", DataViewRowState.CurrentRows)

            For Cnt As Integer = 0 To ClaimAccumulatorEntriesDT.Rows.Count - 1
                AccumulatorName = ClaimAccumulatorEntriesDT.Rows(Cnt)("ACCUM_NAME").ToString

                ClaimAccumulatorEntriesDV.RowFilter = "ACCUM_NAME = '" & AccumulatorName & "'"
                PlanAccumulatorsDV.RowFilter = "ACCUM_NAME = '" & AccumulatorName & "'"

                If PlanAccumulatorsDV.Count > 0 Then
                    If CType(PlanAccumulatorsDV(0)("DURATION_TYPE"), DateType) = DateType.Rollover AndAlso CInt(PlanAccumulatorsDV(0)("DURATION")) = 0 Then
                        LineDate = CDate(Format(CDate(ClaimAccumulatorEntriesDT.Rows(Cnt)("APPLY_DATE")), "12-31-yyyy"))
                        OriginalAccum = DirectCast(_BinderAccumulatorManager.AccumulatorSummaries(AccumulatorController.GetAccumulatorID(AccumulatorName)), MemberAccumulator).OriginalRolloverValue(CDate(LineDate).Year)
                        SumDV.RowFilter = "ACCUM_NAME = '" & AccumulatorName & "' AND YEAR = '" & LineDate.ToString & "'"
                    ElseIf CType(PlanAccumulatorsDV(0)("DURATION_TYPE"), DateType) = DateType.Years AndAlso CInt(PlanAccumulatorsDV(0)("DURATION")) = 0 Then
                        LineDate = CDate(Format(CDate(ClaimAccumulatorEntriesDT.Rows(Cnt)("APPLY_DATE")), "12-31-yyyy"))
                        OriginalAccum = DirectCast(_BinderAccumulatorManager.AccumulatorSummaries(AccumulatorController.GetAccumulatorID(AccumulatorName)), MemberAccumulator).OriginalYearValue(CDate(LineDate).Year)
                        SumDV.RowFilter = "ACCUM_NAME = '" & AccumulatorName & "' AND YEAR = '" & LineDate.ToString & "'"
                    ElseIf CType(PlanAccumulatorsDV(0)("DURATION_TYPE"), DateType) = DateType.Days Then
                        LineDate = CDate(Format(CDate(ClaimAccumulatorEntriesDT.Rows(Cnt)("APPLY_DATE")), "12-31-yyyy"))
                        OriginalAccum = DirectCast(_BinderAccumulatorManager.AccumulatorSummaries(AccumulatorController.GetAccumulatorID(AccumulatorName)), MemberAccumulator).OriginalLifetimeValue
                        SumDV.RowFilter = "ACCUM_NAME = '" & AccumulatorName & "'"
                    Else
                        LineDate = Nothing
                        OriginalAccum = DirectCast(_BinderAccumulatorManager.AccumulatorSummaries(AccumulatorController.GetAccumulatorID(AccumulatorName)), MemberAccumulator).OriginalLifetimeValue
                        SumDV.RowFilter = "ACCUM_NAME = '" & AccumulatorName & "'"
                    End If
                ElseIf AccumulatorName.StartsWith("AC") Then
                    LineDate = Nothing
                    OriginalAccum = DirectCast(_BinderAccumulatorManager.AccumulatorSummaries(AccumulatorController.GetAccumulatorID(AccumulatorName)), MemberAccumulator).OriginalLifetimeValue
                    SumDV.RowFilter = "ACCUM_NAME = '" & AccumulatorName & "'"
                Else
                    LineDate = Nothing
                    SumDV.RowFilter = "ACCUM_NAME = '" & AccumulatorName & "'"
                End If

                If SumDV.Count = 0 Then
                    If LineDate IsNot Nothing Then
                        ClaimLineAccumulatorTotalDV.RowFilter = "ACCUM_NAME = '" & AccumulatorName & "' AND APPLY_DATE >= '" & Format(CDate(LineDate), "1/1/yyyy") & "' AND APPLY_DATE <= '" & LineDate & "'"
                    Else
                        ClaimLineAccumulatorTotalDV.RowFilter = "ACCUM_NAME = '" & AccumulatorName & "'"
                    End If

                    TotalAccum = 0
                    For SubCnt As Integer = 0 To ClaimLineAccumulatorTotalDV.Count - 1
                        TotalAccum += CDec(ClaimLineAccumulatorTotalDV(SubCnt)("ENTRY_VALUE"))
                    Next

                    DR = AccumulatorsDT.NewRow
                    DR("ACCUM_NAME") = AccumulatorName
                    DR("PRIORITY") = ClaimAccumulatorEntriesDT.Rows(Cnt)("DISPLAY_ORDER")
                    DR("YEAR") = IIf(LineDate IsNot Nothing, LineDate, System.DBNull.Value)
                    DR("ORIGINAL_VALUE") = OriginalAccum - TotalAccum
                    DR("PROPOSED_VALUE") = OriginalAccum
                    AccumulatorsDT.Rows.Add(DR)
                End If
            Next

            Return AccumulatorsDT

        Catch ex As Exception
            Throw
        Finally
            If ClaimAccumulatorEntriesDT IsNot Nothing Then
                ClaimAccumulatorEntriesDT.Dispose()
            End If
            ClaimAccumulatorEntriesDT = Nothing

            If ClaimAccumulatorEntriesDV IsNot Nothing Then
                ClaimAccumulatorEntriesDV.Dispose()
            End If
            ClaimAccumulatorEntriesDV = Nothing

            If ClaimLineAccumulatorTotalDV IsNot Nothing Then
                ClaimLineAccumulatorTotalDV.Dispose()
            End If
            ClaimLineAccumulatorTotalDV = Nothing

            If SumDV IsNot Nothing Then
                SumDV.Dispose()
            End If
            SumDV = Nothing

            If PlanAccumulatorsDT IsNot Nothing Then
                PlanAccumulatorsDT.Dispose()
            End If
            PlanAccumulatorsDT = Nothing

            If PlanAccumulatorsDV IsNot Nothing Then
                PlanAccumulatorsDV.Dispose()
            End If
            PlanAccumulatorsDV = Nothing

            DR = Nothing

        End Try

    End Function

    Public Overloads Overrides Function GetOriginalAccumulatorSummary() As DataTable
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets the original accumulator values
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/31/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim OriginalAccumulatorsDT As DataTable
        Dim DR As DataRow
        Dim BinItem As BinderItem
        Dim RSType As Integer
        Dim AccumOrderDV As DataView

        Try

            OriginalAccumulatorsDT = BuildOriginalAccumulators()
            AccumOrderDV = New DataView(AccumulatorController.GetAccumulators, "", "ACCUM_NAME", DataViewRowState.CurrentRows)

            If _Facts IsNot Nothing Then
                For Each Fact As Fact In _Facts

                    BinItem = GetBinderItem(Fact.LineNumber)

                    For Each RuleSet As RuleSet In BinItem.Procedure.RuleSets

                        If RuleSet.RulesetType = RSType Then

                            For Each Rule As Rule In RuleSet
                                If TypeOf (Rule) Is AccumulatorRule Then
                                    If TypeOf (Rule) Is AccidentRule Then
                                        If DirectCast(Rule, AccidentRule).Enabled Then

                                            For Each DE As DictionaryEntry In Rule.Conditions
                                                AccumOrderDV.RowFilter = "ACCUM_NAME = '" & DirectCast(DE.Value, Condition).AccumulatorName & "'"
                                                DR = OriginalAccumulatorsDT.NewRow
                                                DR("LINE_NBR") = Fact.LineNumber
                                                DR("ACCUM_NAME") = DirectCast(DE.Value, Condition).AccumulatorName
                                                DR("ACCUM_VALUE") = Me.BinderAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName)), DirectCast(DE.Value, Condition).DurationType, DirectCast(DE.Value, Condition).Duration, Me.DateOfClaim, DirectCast(DE.Value, Condition).Direction)
                                                DR("PRIORITY") = AccumOrderDV(0)("DISPLAY_ORDER")
                                                OriginalAccumulatorsDT.Rows.Add(DR)
                                                If CDec(DR("ACCUM_VALUE")) > DirectCast(DE.Value, Condition).Operand Then DR("ACCUM_VALUE") = DirectCast(DE.Value, Condition).Operand
                                            Next 'rc
                                        End If
                                    Else
                                        For Each DE As DictionaryEntry In Rule.Conditions
                                            AccumOrderDV.RowFilter = "ACCUM_NAME = '" & DirectCast(DE.Value, Condition).AccumulatorName & "'"
                                            DR = OriginalAccumulatorsDT.NewRow
                                            DR("LINE_NBR") = Fact.LineNumber
                                            DR("ACCUM_NAME") = DirectCast(DE.Value, Condition).AccumulatorName
                                            DR("ACCUM_VALUE") = Me.BinderAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName)), DirectCast(DE.Value, Condition).DurationType, DirectCast(DE.Value, Condition).Duration, Me.DateOfClaim, DirectCast(DE.Value, Condition).Direction)
                                            DR("PRIORITY") = AccumOrderDV(0)("DISPLAY_ORDER")
                                            OriginalAccumulatorsDT.Rows.Add(DR)
                                            If CDec(DR("ACCUM_VALUE")) > DirectCast(DE.Value, Condition).Operand Then DR("ACCUM_VALUE") = DirectCast(DE.Value, Condition).Operand
                                        Next 'rc
                                    End If

                                End If
                            Next 'rl
                        End If
                    Next 'rs
                Next 'f
            End If

            Return OriginalAccumulatorsDT

        Catch ex As Exception
            Throw
        Finally

            DR = Nothing
            If AccumOrderDV IsNot Nothing Then
                AccumOrderDV.Dispose()
            End If
            AccumOrderDV = Nothing

            If BinItem IsNot Nothing Then
                BinItem.Dispose()
            End If
            BinItem = Nothing

        End Try

    End Function

    Public Overloads Overrides Sub AddBinderItem(ByVal lineNumber As Short, ByVal procedureActive As ProcedureActive, ByVal valuedAmount As Decimal, ByVal incidentDate As Date)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Add a Binder Item
        ' </summary>
        ' <param name="lineItem"></param>
        ' <param name="procedure"></param>
        ' <param name="valuedAmount"></param>
        ' <param name="incidentDate"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/7/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim BinderItem As MedicalBinderItem

        Try
            'error checking
            If lineNumber = -1 Then
                Throw New ArgumentException("lineNumber must be >= 0", NameOf(lineNumber))
            End If
            If procedureActive Is Nothing Then
                Throw New ArgumentNullException("procedure")
            End If
            If procedureActive.RuleSets.Count < 1 Then
                Throw New ArgumentException("procedure must have a valid RuleSet", "procedure")
            End If

            If _BinderItems(lineNumber) IsNot Nothing Then
                Throw New BinderItemAlreadyExistsException("There is already a BinderItem using this lineNumber in this Binder")
            End If

            BinderItem = New MedicalBinderItem
            BinderItem.LineNumber = lineNumber
            BinderItem.Procedure = procedureActive
            BinderItem.ValuedAmount = valuedAmount
            BinderItem.MemberAmount = 0

            BinderItem.IncidentDate = incidentDate

            _BinderItems.Add(lineNumber, BinderItem)

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw New BinderItemException("There was a problem adding the item to the binder", ex)
            End If
        Finally

            BinderItem = Nothing

        End Try
    End Sub

    Public Overloads Overrides Sub AddBinderItem(lineNumber As Short)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' add a generic binderitem
        ' </summary>
        ' <param name="lineNumber"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/7/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim BinderItem As MedicalBinderItem

        Try
            If _BinderItems(lineNumber) IsNot Nothing Then
                Throw New BinderItemAlreadyExistsException("There is already a BinderItem using this lineNumber in this Binder")
            End If

            BinderItem = New MedicalBinderItem
            _BinderItems.Add(lineNumber, BinderItem)

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw New BinderItemException("There was a problem adding the item to the binder", ex)
            End If
        Finally
            BinderItem = Nothing

        End Try

    End Sub

    Public Overloads Overrides Sub AddBinderItem(ByVal binderItem As BinderItem)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Add a binder item
        ' </summary>
        ' <param name="bnderItem"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/21/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        If binderItem Is Nothing Then
            Throw New ArgumentNullException("bnderItem")
        End If

        Try
            If binderItem.IncidentDate <> New Date Then
                _DateOfClaim = binderItem.IncidentDate
            End If

            _BinderItems.Add(binderItem.LineNumber, binderItem)

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw New BinderItemException("There was a problem adding the item to the binder", ex)
            End If
        End Try

    End Sub

    Public Overloads Overrides Sub BuildFacts(ByVal lineNumbers() As Short)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Build the facts for each line item that is in the array, for this binder
        ' </summary>
        ' <param name="lineNumbers"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/21/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim TheFacts As Facts
        Dim TheFact As Fact

        Try
            TheFacts = New Facts

            For Each LineNumber As Short In lineNumbers
                TheFact = Fact.GetFact(GetBinderItem(LineNumber), _TypeOfBinder)
                For Each TheRule As Rule In TheFact.Rules
                    If TypeOf (TheRule) Is AccumulatorRule Then
                        CType(TheRule, AccumulatorRule).AccumulatorManager = _BinderAccumulatorManager
                    ElseIf TypeOf (TheRule) Is CoResponsibilityRule Then
                        'nothing to do right UFCWGeneral.NowDate.
                    End If
                Next
                TheFacts.Add(TheFact)
            Next

            _Facts = TheFacts

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw New BinderException("There was a problem getting the Facts for this binder", ex)
            End If
        Finally
            TheFacts = Nothing
        End Try

    End Sub

    Public Overloads Overrides Sub BuildFacts()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Builds the facts for this Binder
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/21/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        If _BinderAccumulatorManager Is Nothing Then
            Throw New BinderException("Must set BinderAccumulatorManager to BuildFacts")
        End If

        Dim TheFacts As Facts
        Dim BinderItemsEnum As IDictionaryEnumerator
        Dim TheFact As Fact
        Dim TheBinderItems() As BinderItem
        Dim BinderItemCount As Integer = 0
        Dim Dte As Date

        Try

            BinderItemsEnum = _BinderItems.GetEnumerator()
            BinderItemsEnum.Reset()

            TheFacts = New Facts

            ReDim TheBinderItems(_BinderItems.Count - 1)

            While BinderItemsEnum.MoveNext()
                TheBinderItems(BinderItemCount) = CType(BinderItemsEnum.Value, BinderItem)
                BinderItemCount += 1
            End While

            For I As Integer = TheBinderItems.Length - 1 To 0 Step -1

                TheFact = Fact.GetFact(TheBinderItems(I), _TypeOfBinder)

                For Each TheRule As Rule In TheFact.Rules
                    If TypeOf (TheRule) Is AccumulatorRule Then
                        CType(TheRule, AccumulatorRule).AccumulatorManager = _BinderAccumulatorManager
                    ElseIf TypeOf (TheRule) Is CoResponsibilityRule Then
                        'nothing to do right UFCWGeneral.NowDate.
                    End If
                Next
                TheFacts.Add(TheFact)
            Next

            _Facts = TheFacts

            'This is need because of ACRs 0297 and 0299
            'essentially we need to process earliest line first
            For Each TheFact In _Facts
                Dte = GetLineDate(TheFact)
                If Dte <> TheFact.LineDate Then
                    TheFact.LineDateOverride = Dte
                End If
            Next

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw New BinderException("There was a problem getting the Facts for this binder", ex)
            End If
        Finally

            TheFacts = Nothing
            BinderItemsEnum = Nothing
            TheFact = Nothing
            TheBinderItems = Nothing
        End Try
    End Sub

    Private Function BuildAccumulators() As DataTable
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Builds a Datatable
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' Taken from original code by Nick Snyder
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/31/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim AccumulatorsDT As DataTable

        Try

            AccumulatorsDT = New DataTable("Accumulators")
            AccumulatorsDT.Columns.Add("ACCUM_NAME")
            AccumulatorsDT.Columns.Add("ORIGINAL_VALUE", System.Type.GetType("System.Decimal"))
            AccumulatorsDT.Columns.Add("PROPOSED_VALUE", System.Type.GetType("System.Decimal"))
            AccumulatorsDT.Columns.Add("INCREASE_AMT", System.Type.GetType("System.Decimal"))
            AccumulatorsDT.Columns.Add("YEAR", System.Type.GetType("System.DateTime"))
            AccumulatorsDT.Columns.Add("PRIORITY", System.Type.GetType("System.Int32"))

            Return AccumulatorsDT

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Throw
            End If
        Finally
            AccumulatorsDT = Nothing
        End Try

    End Function

    Private Function BuildOriginalAccumulators() As DataTable
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Builds a datatable
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' Taken from original code by Nick Snyder
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/31/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim OriginalAccumulatorsDT As DataTable

        Try
            OriginalAccumulatorsDT = New DataTable("OriginalAccumulators")
            OriginalAccumulatorsDT.Columns.Add("LINE_NBR", System.Type.GetType("System.Int32"))
            OriginalAccumulatorsDT.Columns.Add("ACCUM_NAME")
            OriginalAccumulatorsDT.Columns.Add("ACCUM_VALUE")
            OriginalAccumulatorsDT.Columns.Add("PRIORITY", System.Type.GetType("System.Int32"))

            Return OriginalAccumulatorsDT

        Catch ex As Exception

                Throw
        Finally
            OriginalAccumulatorsDT = Nothing
        End Try
    End Function

    Private Function GetClaimConditions() As Conditions

        Dim RuleSetType As Integer
        Dim Enabled As Boolean

        Dim TheBinderItem As BinderItem
        Dim TheConditions As Conditions

        Try
            TheConditions = New Conditions

            If _Facts IsNot Nothing Then

                For Each Fact As Fact In _Facts

                    TheBinderItem = GetBinderItem(Fact.LineNumber)

                    For Each TheRuleSet As RuleSet In TheBinderItem.Procedure.RuleSets

                        If TheRuleSet.RulesetType = RuleSetType Then

                            For Each TheRule As Rule In TheRuleSet
                                If TypeOf (TheRule) Is AccumulatorRule Then

                                    If TypeOf (TheRule) Is AccidentRule Then
                                        Enabled = DirectCast(TheRule, AccidentRule).Enabled
                                    Else
                                        Enabled = True
                                    End If

                                    If Enabled Then
                                        For Each DE As DictionaryEntry In TheRule.Conditions
                                            If TheConditions.IsAccumulatorInConditions(DirectCast(DE.Value, Condition).AccumulatorName) Then
                                                For I As Integer = 0 To TheConditions.Count - 1
                                                    If TheConditions(I).AccumulatorName = DirectCast(DE.Value, Condition).AccumulatorName Then
                                                        If TheConditions(I).Operand < DirectCast(DE.Value, Condition).Operand Then
                                                            TheConditions(I).Operand = DirectCast(DE.Value, Condition).Operand
                                                        End If
                                                    End If
                                                Next
                                            Else
                                                TheConditions.Add(DirectCast(DE.Value, Condition).DeepCopy)
                                            End If
                                        Next 'rc
                                    End If
                                End If
                            Next 'rl
                        End If
                    Next 'rs
                Next 'f
            End If

            Return TheConditions

        Catch ex As Exception
            Throw
        Finally

            TheBinderItem = Nothing
            TheConditions = Nothing

        End Try

    End Function

    Private Function GetAdditionalAccumulatorData(ByVal accumulatorsDT As DataTable) As DataTable
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' This gets data for any additional accumulators that were added to the claim
        '   that were not a part of the rules
        ' </summary>
        ' <param name="accumulatorsDataTable"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/31/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim DT As DataTable
        Dim AccumulatorNames As ArrayList
        Dim AccumulatorName As String
        Dim AlreadyInList As Boolean = False

        Try

            DT = _BinderAccumulatorManager.GetAccumulatorEntryValues(True)
            AccumulatorNames = New ArrayList

            For I As Integer = 0 To DT.Rows.Count - 1
                AlreadyInList = False
                AccumulatorName = DT.Rows(I)("ACCUM_NAME").ToString

                For X As Integer = 0 To accumulatorsDT.Rows.Count - 1
                    If accumulatorsDT.Rows(X)("ACCUM_NAME").ToString.Trim = AccumulatorName.Trim Then
                        AlreadyInList = True
                    End If
                Next

                Dim AddIt As Boolean = True

                If Not AlreadyInList Then
                    AddIt = True
                    For Z As Integer = 0 To AccumulatorNames.Count - 1
                        If AccumulatorNames(Z).ToString = AccumulatorName Then
                            AddIt = False
                        End If
                    Next
                    If AddIt Then AccumulatorNames.Add(AccumulatorName)
                End If
            Next

            If AccumulatorNames.Count > 0 Then
                Return GetAdditionalAccumulatorValues(AccumulatorNames, accumulatorsDT)
            End If

            Return accumulatorsDT

        Catch ex As Exception
            Throw
        Finally
            DT = Nothing
            AccumulatorNames = Nothing

        End Try

    End Function

    Private Function GetAdditionalAccumulatorValues(ByVal accumList As ArrayList, ByVal accumulatorsDT As DataTable) As DataTable
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' This augments the datatable with any new accumulators that were added to the
        '   claim that were not a part of the rules
        ' </summary>
        ' <param name="accumList"></param>
        ' <param name="accumulatorsDataTable"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/31/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim AccumulatorDV As DataView
        Dim AccumulatorOrderDV As DataView
        Dim Conditions As Conditions

        Dim Amt As Decimal
        Dim ServiceDates As Date()
        Dim Found As Boolean
        Dim DR As DataRow
        Dim LineDate As Date?

        Try

            AccumulatorDV = New DataView(accumulatorsDT, "", "ACCUM_NAME", DataViewRowState.CurrentRows)
            AccumulatorOrderDV = New DataView(AccumulatorController.GetAccumulators, "", "ACCUM_NAME", DataViewRowState.CurrentRows)

            ServiceDates = GetDistinctDatesFromEntries()
            Conditions = New Conditions

            For Each DateOfService As Date In ServiceDates
                Dim UniqueConditionsForYear As Conditions = PlanController.GetDistinctConditionsForDOS(DateOfService)
                Found = False

                For Each DE As DictionaryEntry In UniqueConditionsForYear
                    If accumList.Contains(DirectCast(DE.Value, Condition).AccumulatorName) Then
                        Conditions.Add(DirectCast(DE.Value, Condition))
                    End If
                Next
            Next

            For Each DE As DictionaryEntry In Conditions
                For Each DateOfService As Date In ServiceDates
                    AccumulatorOrderDV.RowFilter = "ACCUM_NAME = '" & DirectCast(DE.Value, Condition).AccumulatorName & "'"
                    If DirectCast(DE.Value, Condition).DurationType = DateType.Rollover AndAlso DirectCast(DE.Value, Condition).Duration = 0 Then
                        LineDate = CDate(Format(DateOfService, "12-31-yyyy"))
                        AccumulatorDV.RowFilter = "ACCUM_NAME = '" & DirectCast(DE.Value, Condition).AccumulatorName & "' AND YEAR = '" & LineDate & "'"
                    ElseIf DirectCast(DE.Value, Condition).DurationType = DateType.Years AndAlso DirectCast(DE.Value, Condition).Duration = 0 Then
                        LineDate = CDate(Format(DateOfService, "12-31-yyyy"))
                        AccumulatorDV.RowFilter = "ACCUM_NAME = '" & DirectCast(DE.Value, Condition).AccumulatorName & "' AND YEAR = '" & LineDate & "'"
                    ElseIf DirectCast(DE.Value, Condition).DurationType = DateType.Years AndAlso DirectCast(DE.Value, Condition).Duration > 50 Then
                        LineDate = Nothing
                        AccumulatorDV.RowFilter = "ACCUM_NAME = '" & DirectCast(DE.Value, Condition).AccumulatorName & "' " 'AND YEAR = '" & lineDate & "'"
                    ElseIf DirectCast(DE.Value, Condition).DurationType = DateType.Days Then
                        LineDate = Nothing 'CDate(Format(dt, "12-31-yyyy"))
                        AccumulatorDV.RowFilter = "ACCUM_NAME = '" & DirectCast(DE.Value, Condition).AccumulatorName & "'"
                    Else
                        LineDate = Nothing
                        AccumulatorDV.RowFilter = "ACCUM_NAME = '" & DirectCast(DE.Value, Condition).AccumulatorName & "'"
                    End If

                    If AccumulatorDV.Count < 1 Then
                        DR = accumulatorsDT.NewRow
                        DR("ACCUM_NAME") = DirectCast(DE.Value, Condition).AccumulatorName
                        DR("PRIORITY") = AccumulatorController.GetAccumulatorDisplayOrder(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName)))

                        If LineDate Is Nothing Then
                            DR("YEAR") = System.DBNull.Value
                        Else
                            DR("YEAR") = LineDate
                        End If

                        If DirectCast(DE.Value, Condition).DurationType = DateType.Rollover AndAlso DirectCast(DE.Value, Condition).Duration = 0 Then
                            DR("ORIGINAL_VALUE") = Me.BinderAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName)), DirectCast(DE.Value, Condition).DurationType, DirectCast(DE.Value, Condition).Duration, LineDate, DirectCast(DE.Value, Condition).Direction)
                            DR("PROPOSED_VALUE") = Me.BinderAccumulatorManager.GetProposedValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName)), DirectCast(DE.Value, Condition).DurationType, DirectCast(DE.Value, Condition).Duration, LineDate, DirectCast(DE.Value, Condition).Direction)
                        ElseIf DirectCast(DE.Value, Condition).DurationType = DateType.Years AndAlso DirectCast(DE.Value, Condition).Duration = 0 Then
                            DR("ORIGINAL_VALUE") = Me.BinderAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName)), DirectCast(DE.Value, Condition).DurationType, DirectCast(DE.Value, Condition).Duration, LineDate, DirectCast(DE.Value, Condition).Direction)
                            DR("PROPOSED_VALUE") = Me.BinderAccumulatorManager.GetProposedValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName)), DirectCast(DE.Value, Condition).DurationType, DirectCast(DE.Value, Condition).Duration, LineDate, DirectCast(DE.Value, Condition).Direction)
                        ElseIf DirectCast(DE.Value, Condition).DurationType = DateType.Years AndAlso DirectCast(DE.Value, Condition).Duration > 50 Then
                            DR("ORIGINAL_VALUE") = Me.BinderAccumulatorManager.GetOriginalLifetimeValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName))) ', rc.DurationType, rc.Duration, lineDate, rc.Direction)
                            DR("PROPOSED_VALUE") = Me.BinderAccumulatorManager.GetProposedLifetimeValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName))) ', rc.DurationType, rc.Duration, lineDate, rc.Direction)
                        ElseIf CStr(DR("ACCUM_NAME")).Trim.StartsWith("AC") Then
                            DR("ORIGINAL_VALUE") = Me.BinderAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName)), DirectCast(DE.Value, Condition).DurationType, DirectCast(DE.Value, Condition).Duration, DateOfService, DirectCast(DE.Value, Condition).Direction)
                            DR("PROPOSED_VALUE") = Me.BinderAccumulatorManager.GetProposedValue(CInt(AccumulatorController.GetAccumulatorID(DirectCast(DE.Value, Condition).AccumulatorName)), DirectCast(DE.Value, Condition).DurationType, DirectCast(DE.Value, Condition).Duration, DateOfService, DirectCast(DE.Value, Condition).Direction)
                        End If
                        accumulatorsDT.Rows.Add(DR)
                    End If
                Next
            Next

            If Conditions.Count <> accumList.Count Then
                For Each Dt As Date In ServiceDates

                    For I As Integer = 0 To accumList.Count - 1
                        Found = False

                        For X As Integer = 0 To Conditions.Count - 1
                            If Conditions(X).AccumulatorName.ToUpper = CStr(accumList(I)).Trim.ToUpper Then
                                Found = True
                                Amt = Conditions(X).Operand 'Amt is not used ?
                            End If
                        Next

                        If Not Found Then
                            AccumulatorDV.RowFilter = "ACCUM_NAME = '" & accumList(I).ToString & "' AND YEAR = '" & CDate(Format(Dt, "12-31-yyyy")) & "'"

                            If AccumulatorDV.Count < 1 Then
                                DR = accumulatorsDT.NewRow
                                DR("ACCUM_NAME") = accumList(I)
                                DR("PRIORITY") = AccumulatorController.GetAccumulatorDisplayOrder(CInt(AccumulatorController.GetAccumulatorID(CStr(accumList(I)))))
                                DR("YEAR") = Format(Dt, "12/31/yyyy")
                                DR("ORIGINAL_VALUE") = Me.BinderAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID(CStr(accumList(I)))), DateType.Days, 0, Dt, DateDirection.Reverse) 'System.DBNull.Value

                                DR("PROPOSED_VALUE") = Me.BinderAccumulatorManager.GetProposedValue(CInt(AccumulatorController.GetAccumulatorID(CStr(accumList(I)))), DateType.Days, 0, Dt, DateDirection.Reverse)

                                accumulatorsDT.Rows.Add(DR)
                            End If
                        End If
                    Next
                Next
            End If

            Return accumulatorsDT

        Catch ex As Exception
            Throw
        Finally
            If AccumulatorDV IsNot Nothing Then
                AccumulatorDV.Dispose()
            End If
            AccumulatorDV = Nothing
            If AccumulatorOrderDV IsNot Nothing Then
                AccumulatorOrderDV.Dispose()
            End If
            AccumulatorOrderDV = Nothing

            Conditions = Nothing

        End Try

    End Function

    Private Function GetDistinctDatesFromEntries() As Date()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' This gets distinct date entries
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/31/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim DT As DataTable
        Dim AL As ArrayList
        Dim Dte As Date
        Dim Dates() As Date

        Try

            DT = _BinderAccumulatorManager.GetAccumulatorEntryValues(True)
            AL = New ArrayList

            For I As Integer = 0 To DT.Rows.Count - 1
                Dte = CDate(DT.Rows(I)("APPLY_DATE"))
                If Not AL.Contains(Dte) Then
                    AL.Add(Dte)
                End If
            Next

            ReDim Dates(AL.Count - 1)
            For I As Integer = 0 To AL.Count - 1
                Dates(I) = DirectCast(AL(I), Date)
            Next

            Return Dates

        Catch ex As Exception
            Throw
        Finally
            DT = Nothing
        End Try
    End Function

    Private Function GetLineDate(ByVal fact As Fact) As Date

        Try

            For Each Rule As Rule In fact.Rules
                For Each DE As DictionaryEntry In Rule.Conditions
                    If DirectCast(DE.Value, Condition).DurationType = DateType.Years AndAlso DirectCast(DE.Value, Condition).Duration > 0 AndAlso DirectCast(DE.Value, Condition).Duration < 20 Then
                        Return GetEarliestForMultiYear()
                    End If
                Next
            Next

            Return fact.LineDate

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Function GetLatestDateForMultiYear() As Date
        Dim Dte As Date

        Try

            Dte = New Date(1970, 1, 1)

            For Each Fact As Fact In Me.Facts
                For Each Rule As Rule In Fact.Rules
                    For Each DE As DictionaryEntry In Rule.Conditions
                        If DirectCast(DE.Value, Condition).DurationType = DateType.Years AndAlso DirectCast(DE.Value, Condition).Duration > 1 Then
                            If Fact.LineDate > Dte Then
                                Dte = Fact.LineDate
                            End If
                        End If
                    Next
                Next
            Next

            Return Dte

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function GetEarliestForMultiYear() As Date

        Dim Dte As Date

        Try
            Dte = New Date(2970, 1, 1)

            For Each Fact As Fact In Me.Facts
                For Each Rule As Rule In Fact.Rules
                    For Each DE As DictionaryEntry In Rule.Conditions
                        If DirectCast(DE.Value, Condition).DurationType = DateType.Years AndAlso DirectCast(DE.Value, Condition).Duration > 1 Then
                            If Fact.LineDate < Dte Then
                                Dte = Fact.LineDate
                            End If
                        End If
                    Next
                Next
            Next

            Return Dte

        Catch ex As Exception
            Throw
        End Try

    End Function

#End Region

End Class