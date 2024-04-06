Option Strict On
Option Infer On

Imports System.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling


''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.ProcessingEngine
''' Class	 : Processor
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' Represents a processor for medical claims
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/19/2006	Created
'''     [paulw] 9/27/2006   Per ACR MED-0029, added MultiLineCoPay type support
''' </history>
''' -----------------------------------------------------------------------------
Public NotInheritable Class MedicalProcessor
    Implements IProcessor, IDisposable

#Region "Events"
    Public Event OnProcessorCompleteEvent(ByVal actionDescriptions As Collection) Implements IProcessor.OnProcessorComplete
#End Region

#Region "Private Variables"

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _ActionQueue As ActionQueue
    Private _CurrentBinder As Binder
    Private _CurrentLineNumber As Short
    Private _Disposed As Boolean = False
    Private _MedicalProcessorGuid As System.Guid = Guid.NewGuid()
#End Region

    Protected Overrides Sub Finalize()
        ' Simply call Dispose(False).
        Dispose(False)
    End Sub

    Public Shared Function GetEntryValuesWithCap(ByVal meddtlDT As DataTable, ByVal lineNumber As Short, ByVal accumulatorName As String, ByVal memberAccumManager As MemberAccumulatorManager) As DataView

        Dim DV As DataView

        Try

            DV = New DataView(GetEntryValuesWithCap(meddtlDT, memberAccumManager))

            DV.RowFilter = "LINE_NBR = " & lineNumber & " AND ACCUM_NAME = '" & accumulatorName & "'"

            Return DV

        Catch ex As Exception

            Throw
        Finally

            If DV IsNot Nothing Then DV.Dispose()
            DV = Nothing

        End Try
    End Function

    Public Shared Function GetEntryValuesWithCap(ByVal meddtlDT As DataTable, ByVal lineNumber As Short, ByVal memberAccumManager As MemberAccumulatorManager) As DataView

        Dim DV As DataView

        Try

            DV = New DataView(GetEntryValuesWithCap(meddtlDT, memberAccumManager))

            DV.RowFilter = "LINE_NBR = " & lineNumber
            Return DV

        Catch ex As Exception
            Throw
        Finally

            If DV IsNot Nothing Then DV.Dispose()
            DV = Nothing

        End Try

    End Function

    Public Shared Function GetEntryValuesWithCap(ByVal meddtlDT As DataTable, ByVal memberAccumManager As MemberAccumulatorManager) As DataTable

        Dim ReturnEntriesDT As DataTable
        Dim EntriesDT As DataTable
        Dim ClaimDates As Date()
        Dim Integers As Integer()
        Dim RuleSetHashTable As Hashtable
        Dim TheRuleSet As RuleSet
        Dim RuleSetArrayList As ArrayList

        Dim Conditions As Conditions

        Try

            ReturnEntriesDT = CMSDALCommon.CreateAccumulatorValuesDT

            ClaimDates = GetDistinctDateValuesFromDataTable("OCC_FROM_DATE", meddtlDT)
            Integers = GetDistinctRulesetIDValuesFromDataTable("RULE_SET_ID", meddtlDT)

            RuleSetHashTable = New Hashtable
            RuleSetArrayList = New ArrayList

            For I As Integer = 0 To Integers.Length - 1
                TheRuleSet = PlanController.GetRuleset(Integers(I))
                RuleSetArrayList.Add(TheRuleSet)
                For X As Integer = 0 To meddtlDT.Rows.Count - 1
                    If CInt(meddtlDT.Rows(X)("RULE_SET_ID")) = TheRuleSet.RulesetID Then
                        RuleSetHashTable.Add(CInt(meddtlDT.Rows(X)("LINE_NBR")), TheRuleSet)
                    End If
                Next
            Next

            Conditions = GetClaimConditions(RuleSetArrayList)

            Array.Sort(ClaimDates)

            For Each ClaimDate As Date In ClaimDates  'go through all the dates of the claim
                If Not ClaimDate = New Date Then
                    For I As Integer = 0 To meddtlDT.Rows.Count - 1   'loop through all rows in details table
                        If CDate(meddtlDT.Rows(I)("OCC_FROM_DATE")) = CDate(ClaimDate) Then    'if the row was on the date
                            'get the entries for that line
                            EntriesDT = memberAccumManager.GetAccumulatorEntryValues(True, CInt(meddtlDT.Rows(0)("CLAIM_ID")), CShort(meddtlDT.Rows(I)("LINE_NBR")))
                            ReturnEntriesDT = AccumulateAndCapEntries(EntriesDT, ReturnEntriesDT, Conditions, memberAccumManager, RuleSetHashTable)
                        End If
                    Next
                End If
            Next

            Return ReturnEntriesDT

        Catch ex As Exception

            Throw

        Finally

            ReturnEntriesDT = Nothing
            EntriesDT = Nothing
            ClaimDates = Nothing
            Integers = Nothing
            RuleSetHashTable = Nothing
            TheRuleSet = Nothing
            RuleSetArrayList = Nothing

            Conditions = Nothing

        End Try

    End Function

    ' Implement IDisposable.
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Private Shared Function AccumulateAndCapEntries(ByVal entriesDT As DataTable, ByVal returnEntriesDT As DataTable, ByVal theConditions As Conditions, ByVal accumManager As MemberAccumulatorManager, ByVal rulesHashTable As Hashtable) As DataTable

        Dim DV As DataView
        Dim DR As DataRow
        Dim OriginalValue As Decimal
        Dim TheCondition As Condition
        Dim TheRule As Rule
        Dim DisplayZeroForOOPs As Boolean
        Dim TheRuleSet As RuleSet

        Try

            DV = New DataView(returnEntriesDT)

            For I As Integer = 0 To entriesDT.Rows.Count - 1

                TheRule = Nothing

                DV.RowFilter = "ACCUM_NAME = '" & entriesDT.Rows(I)("ACCUM_NAME").ToString & "' and LINE_NBR = " & entriesDT.Rows(I)("LINE_NBR").ToString
                If DV.Count = 0 Then
                    DR = returnEntriesDT.NewRow
                    DR("CLAIM_ID") = entriesDT.Rows(I)("CLAIM_ID")
                    DR("ACCUM_NAME") = entriesDT.Rows(I)("ACCUM_NAME")
                    DR("OVERRIDE_SW") = entriesDT.Rows(I)("OVERRIDE_SW")
                    DR("LINE_NBR") = entriesDT.Rows(I)("LINE_NBR")
                    DR("DISPLAY_ORDER") = entriesDT.Rows(I)("DISPLAY_ORDER")
                    DR("APPLY_DATE") = entriesDT.Rows(I)("APPLY_DATE")
                End If

                DR("ENTRY_VALUE") = CDec(DR("ENTRY_VALUE")) + CDec(entriesDT.Rows(I)("ENTRY_VALUE"))

                For Z As Integer = 0 To theConditions.Count - 1
                    TheRuleSet = CType(rulesHashTable(CInt(entriesDT.Rows(I)("LINE_NBR"))), RuleSet)
                    For Y As Integer = 0 To TheRuleSet.Count
                        For Each DE As DictionaryEntry In TheRuleSet(Y).Conditions
                            If DirectCast(DE.Value, Condition).AccumulatorName.ToUpper = CStr(DR("ACCUM_NAME")).ToUpper.Trim Then
                                TheRule = TheRuleSet(Y)
                                Exit For
                            End If
                        Next
                        If TheRule IsNot Nothing Then Exit For
                    Next

                    If TheRule IsNot Nothing Then
                        TheCondition = theConditions(Z)
                        If TypeOf (TheRule) Is StandardAccumulatorRule Then
                            If TheRule.Conditions.IsAccumulatorInConditions(TheCondition.AccumulatorName) Then
                                OriginalValue = accumManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID(TheCondition.AccumulatorName)), TheCondition.DurationType, TheCondition.Duration, CDate(entriesDT.Rows(I)("APPLY_DATE")), TheCondition.Direction)
                                If TheCondition.Operand <= OriginalValue Then
                                    DisplayZeroForOOPs = True
                                End If
                            End If
                        End If

                        If TheCondition.AccumulatorName.ToUpper.Trim = CStr(DR("ACCUM_NAME")).ToUpper.Trim Then
                            If theConditions.IsConfiguredForNonPar Then
                                If TheCondition.UseInHeadroomCheck Then 'non-par
                                    If TheRule.GetNonParHeadroom(accumManager, True, CDate(entriesDT.Rows(I)("APPLY_DATE"))) <> 0 Then
                                        DR("ENTRY_VALUE") = TheRule.GetNonParHeadroom(accumManager, True, CDate(entriesDT.Rows(I)("APPLY_DATE")))
                                    Else
                                        If CDec(DR("ENTRY_VALUE")) > TheCondition.Operand Then DR("ENTRY_VALUE") = TheCondition.Operand
                                    End If
                                Else 'par
                                    If TheRule.GetParHeadroomForNonParClaim(accumManager, True, CDate(entriesDT.Rows(I)("APPLY_DATE"))) <> 0 Then
                                        DR("ENTRY_VALUE") = TheRule.GetParHeadroomForNonParClaim(accumManager, True, CDate(entriesDT.Rows(I)("APPLY_DATE")))
                                    Else
                                        If CDec(DR("ENTRY_VALUE")) > TheCondition.Operand Then DR("ENTRY_VALUE") = CDec(DR("ENTRY_VALUE")) + TheRule.GetParHeadroomForNonParClaim(accumManager, True, CDate(entriesDT.Rows(I)("APPLY_DATE")))
                                    End If
                                End If
                            Else
                                If CDec(DR("ENTRY_VALUE")) > TheCondition.Operand Then DR("ENTRY_VALUE") = CDec(DR("ENTRY_VALUE")) + TheRule.GetNonParHeadroom(accumManager, False, CDate(entriesDT.Rows(I)("APPLY_DATE"))) 'rc.Operand
                            End If

                        End If
                    End If
                Next

                If DV.Count = 0 Then returnEntriesDT.Rows.Add(DR)

            Next

            Return returnEntriesDT

        Catch ex As Exception
            Throw
        Finally

            If DV IsNot Nothing Then DV.Dispose()
            DV = Nothing
        End Try

    End Function

    Private Shared Function GetClaimConditions(ByVal ruleSets As ArrayList) As Conditions

        Dim Enabled As Boolean
        Dim TheConditions As Conditions

        Try
            TheConditions = New Conditions

            For Each TheRuleSet As RuleSet In ruleSets
                For Each TheRule As Rule In TheRuleSet
                    If TypeOf (TheRule) Is AccumulatorRule Then
                        If TypeOf (TheRule) Is AccidentRule Then
                            Enabled = DirectCast(TheRule, AccidentRule).Enabled
                        Else
                            Enabled = True
                        End If

                        If Enabled Then
                            For Each TheCondition As DictionaryEntry In TheRule.Conditions
                                If TheConditions.IsAccumulatorInConditions(DirectCast(TheCondition.Value, Condition).AccumulatorName) Then
                                    For I As Integer = 0 To TheConditions.Count - 1
                                        If TheConditions(I).AccumulatorName = DirectCast(TheCondition.Value, Condition).AccumulatorName Then
                                            If TheConditions(I).Operand < DirectCast(TheCondition.Value, Condition).Operand Then
                                                TheConditions(I).Operand = DirectCast(TheCondition.Value, Condition).Operand
                                            End If
                                        End If
                                    Next
                                Else
                                    TheConditions.Add(DirectCast(TheCondition.Value, Condition))
                                End If
                            Next 'rc
                        End If
                    End If
                Next
            Next

            Return TheConditions

        Catch ex As Exception
            Throw
        Finally
            TheConditions = Nothing
        End Try

    End Function

    Private Shared Function GetDistinctDateValuesFromDataTable(ByVal dateColumnName As String, ByVal dt As DataTable) As Date()

        Dim AL As ArrayList
        Dim Dates() As Date

        Try

            AL = New ArrayList

            For I As Integer = 0 To dt.Rows.Count - 1
                If Not AL.Contains(CDate(dt.Rows(I)(dateColumnName))) Then
                    AL.Add(CDate(dt.Rows(I)(dateColumnName)))
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
            AL = Nothing
            Dates = Nothing
        End Try

    End Function

    Private Shared Function GetDistinctRulesetIDValuesFromDataTable(ByVal dateColumnName As String, ByVal dt As DataTable) As Integer()

        Dim AL As ArrayList
        Dim Integers() As Integer

        Try

            AL = New ArrayList

            For I As Integer = 0 To dt.Rows.Count - 1
                If Not AL.Contains(CInt(dt.Rows(I)(dateColumnName))) Then
                    AL.Add(CInt(dt.Rows(I)(dateColumnName)))
                End If
            Next

            ReDim Integers(AL.Count - 1)

            For I As Integer = 0 To AL.Count - 1
                Integers(I) = DirectCast(AL(I), Integer)
            Next

            Return Integers

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Overloads Sub Dispose(disposing As Boolean)
        If _Disposed = False Then
            If disposing Then
                ' Free other state (managed objects).
                _Disposed = True

                _CurrentBinder = Nothing
                _ActionQueue = Nothing
            End If
            ' Free your own state (unmanaged objects).
            ' Set large fields to null.
        End If
    End Sub
#Region "Constructors"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Default Constructor
    ' </summary>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	4/20/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Sub New()
        _ActionQueue = New ActionQueue
    End Sub
#End Region

#Region "Methods"

    Public Shared Sub CheckBinder(ByVal theBinder As Binder)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Makes sure the binder is valid
        ' </summary>
        ' <param name="bnder"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/3/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            If theBinder Is Nothing Then
                Throw New ArgumentNullException("binder")
            End If

            If theBinder.BinderAccumulatorManager Is Nothing Then
                Throw New ArgumentException("Binder must have valid AccumulatorManager", "binder")
            End If

            If theBinder.BinderAlertManager Is Nothing Then
                Throw New ArgumentException("Binder must have valid AlertManager", "binder")
            End If

            If theBinder.DateOfClaim = New Date Then
                Throw New ArgumentException("Binder must have valid DateOfClaim", "binder")
            End If

            If theBinder.ClaimNumber = 0 Then
                Throw New ArgumentException("Binder must have valid claim number", "binder")
            End If

            'build the facts to make sure we have valid facts
            theBinder.BuildFacts()

            If theBinder.Facts Is Nothing Then
                Throw New ArgumentException("Binder must have valid facts object", "binder")
            End If

            If theBinder.Facts.Count < 1 Then
                Throw New ArgumentException("Binder must have more than one fact. Try calling BuildFacts", "binder")
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Sub Process(ByVal theBinder As Binder) Implements IProcessor.Process
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Process the binder
        ' </summary>
        ' <param name="IBinder"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/21/2006	Created
        '     [paulw] 9/27/2006   Per ACR MED-0029, added MultiLineCoPay type support
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim ActionDetails As Collection
        Dim DatesProcessed As ArrayList
        Dim ProcessDate As Date?
        Dim ItemFound As Boolean

        Try
            ActionDetails = New Collection
            DatesProcessed = New ArrayList

            CheckBinder(theBinder)

            _CurrentBinder = theBinder

            theBinder.BinderAccumulatorManager.RefreshAccumulatorSummariesForMember()
            theBinder.BuildFacts()

            ' get first date
            'go through all that have that date
            'add date to processed dates
            'get next date that does not exist in processed date
            'go through all that have that date
            '1.  go through all that have that date
            '2.  add that date to processed dates
            '3.  go to next date

            Dim QueryFactInDateOfServiceOrder = From TheFacts In theBinder.Facts.OfType(Of Fact)() Order By TheFacts.LineDate

            For Each FactByDateToProcess As Fact In QueryFactInDateOfServiceOrder.AsEnumerable

                ItemFound = False

                For Z As Integer = 0 To DatesProcessed.Count - 1
                    If CDate(DatesProcessed(Z)).Date = CDate(FactByDateToProcess.LineDate).Date Then ItemFound = True
                Next

                ProcessDate = Nothing

                If Not ItemFound Then
                    ProcessDate = CDate(FactByDateToProcess.LineDate)
                End If

                If ProcessDate IsNot Nothing Then
                    Dim CheckMultiLine As Boolean = False
                    Dim MultiLineAmt As Decimal = 0D

                    'go through all the facts and process each fact

                    Dim QueryFactInLineNumberOrder = From TheFacts In theBinder.Facts.OfType(Of Fact)() Order By TheFacts.LineNumber

                    For Each FactByLineToProcess As Fact In QueryFactInLineNumberOrder.AsEnumerable

                        Try
                            If FactByLineToProcess.LineDate.Date = ProcessDate Then
                                FactByLineToProcess.MultiLineFundPaymentAmount = MultiLineAmt
                                FactByLineToProcess.CheckMultiLineValue = CheckMultiLine

                                If theBinder.Facts.ClaimLevelUnitAmount(FactByLineToProcess.ProcedureCodeUsed) < 1 AndAlso theBinder.Facts.ClaimLevelUnitAmountSet(FactByLineToProcess.ProcedureCodeUsed) Then

                                Else
                                    ProcessFact(FactByLineToProcess)
                                End If

                                If theBinder.Facts.ClaimLevelUnitAmountSet(FactByLineToProcess.ProcedureCodeUsed) Then
                                    theBinder.Facts.ClaimLevelUnitAmount(FactByLineToProcess.ProcedureCodeUsed) -= FactByLineToProcess.UnitAmount
                                End If

                                MultiLineAmt = FactByLineToProcess.MultiLineFundPaymentAmount
                                CheckMultiLine = FactByLineToProcess.CheckMultiLineValue

                                FactByLineToProcess.Status = 0

                                theBinder.GetBinderItem(FactByLineToProcess.LineNumber).RuleSetIDUsed = FactByLineToProcess.RuleSetIdUsed
                                theBinder.GetBinderItem(FactByLineToProcess.LineNumber).Status = BinderItemStatus.Processed
                            End If

                        Catch ex As Exception
                            If TypeOf (ex) Is NeedsRepricingException Then
                                FactByLineToProcess.Status = BinderItemStatus.NeedsReprice
                                theBinder.GetBinderItem(FactByLineToProcess.LineNumber).Status = BinderItemStatus.NeedsReprice
                            ElseIf TypeOf ex Is FundDenialException Then
                                FactByLineToProcess.Status = BinderItemStatus.Denied
                                theBinder.GetBinderItem(FactByLineToProcess.LineNumber).Status = BinderItemStatus.Denied
                            ElseIf TypeOf ex Is ProviderWriteOffException Then
                                FactByLineToProcess.Status = BinderItemStatus.Denied
                                theBinder.GetBinderItem(FactByLineToProcess.LineNumber).Status = BinderItemStatus.ProviderWriteOff
                            ElseIf TypeOf ex Is ConstraintException Then
                                FactByLineToProcess.Status = BinderItemStatus.InvalidAccumulator
                                theBinder.GetBinderItem(FactByLineToProcess.LineNumber).Status = BinderItemStatus.InvalidAccumulator
                            Else
                                FactByLineToProcess.Status = BinderItemStatus.Failed
                                theBinder.GetBinderItem(FactByLineToProcess.LineNumber).Status = BinderItemStatus.Failed
                            End If
                        End Try

                    Next

                    DatesProcessed.Add(ProcessDate)

                End If
            Next

            RaiseEvent OnProcessorCompleteEvent(ActionDetails)

        Catch ex As Exception

                Throw
        Finally
            ActionDetails = Nothing
            DatesProcessed = Nothing
        End Try
    End Sub

    Private Shared Function ExecuteAction(ByVal theAction As IAction, ByVal theCondition As Condition, ByVal theFact As Fact) As String
        Try

            Select Case theAction.ActionValueType
                Case ActionValueTypes.ActualValue

                    If TypeOf (theAction) Is AlterAccumulatorAction Then
                        theAction.ActionValue = theAction.Execute(theAction.ActionValue, theCondition)
                    Else
                        theAction.ActionValue = theAction.Execute(theAction.ActionValue)
                    End If

                    Return Format(Math.Round(theAction.ActionValue, 2), "0.00")

                Case ActionValueTypes.FundPaymentValue

                    If TypeOf (theAction) Is AlterAccumulatorAction Then
                        theFact.PaymentAmount = theAction.Execute(theFact.PaymentAmount, theCondition)
                    Else
                        theFact.PaymentAmount = theAction.Execute(theFact.PaymentAmount)
                    End If

                    Return Format(Math.Round(theFact.PaymentAmount, 2), "0.00")

                Case ActionValueTypes.MemberPaymentValue

                    If TypeOf (theAction) Is AlterAccumulatorAction Then
                        theFact.MemberAmount = theAction.Execute(theFact.MemberAmount, theCondition)
                    Else
                        theFact.MemberAmount = theAction.Execute(theFact.MemberAmount)
                    End If

                    Return Format(Math.Round(theFact.MemberAmount, 2), "0.00")

                Case ActionValueTypes.UnitsValue

                    If TypeOf (theAction) Is AlterAccumulatorAction Then
                        theFact.UnitAmount = theAction.Execute(theFact.UnitAmount, theCondition)
                    Else
                        theFact.UnitAmount = theAction.Execute(theFact.UnitAmount)
                    End If

                    Return theFact.UnitAmount.ToString

                Case ActionValueTypes.Other

                    If TypeOf (theAction) Is AlterAccumulatorAction Then
                        theFact.PaymentAmount = theAction.Execute(theFact.PaymentAmount, theCondition)
                    Else
                        theFact.PaymentAmount = theAction.Execute(theFact.PaymentAmount)
                    End If

                    Return Format(Math.Round(theFact.PaymentAmount, 2), "0.00")

                Case ActionValueTypes.PreSubTotalMemberPaymentValue

                    If TypeOf (theAction) Is AlterAccumulatorAction Then
                        theFact.PreSubTotalMemberPayment = theAction.Execute(theFact.PreSubTotalMemberPayment, theCondition)
                    Else
                        theFact.PreSubTotalMemberPayment = theAction.Execute(theFact.PreSubTotalMemberPayment)
                    End If

                    Return Format(Math.Round(theFact.PreSubTotalMemberPayment, 2), "0.00")

                Case ActionValueTypes.PreSubTotalParMemberPaymentValue

                    If TypeOf (theAction) Is AlterAccumulatorAction Then
                        theFact.PreSubTotalParMemberPaymentValue = theAction.Execute(theFact.PreSubTotalParMemberPaymentValue, theCondition)
                    Else
                        theFact.PreSubTotalParMemberPaymentValue = theAction.Execute(theFact.PreSubTotalParMemberPaymentValue)
                    End If

                    Return Format(Math.Round(theFact.PreSubTotalParMemberPaymentValue, 2), "0.00")

                Case ActionValueTypes.PreSubTotalNonParMemberPaymentValue

                    If TypeOf (theAction) Is AlterAccumulatorAction Then
                        theFact.PreSubTotalNonParMemberPaymentValue = theAction.Execute(theFact.PreSubTotalNonParMemberPaymentValue, theCondition)
                    Else
                        theFact.PreSubTotalNonParMemberPaymentValue = theAction.Execute(theFact.PreSubTotalNonParMemberPaymentValue)
                    End If

                    Return Format(Math.Round(theFact.PreSubTotalNonParMemberPaymentValue, 2), "0.00")

                Case ActionValueTypes.ParMemberPaymentValue

                    If TypeOf (theAction) Is AlterAccumulatorAction Then
                        theFact.ParMemberPaymentValue = theAction.Execute(theFact.ParMemberPaymentValue, theCondition)
                    Else
                        theFact.ParMemberPaymentValue = theAction.Execute(theFact.ParMemberPaymentValue)
                    End If

                    For Each FactsRule As Rule In theFact.Rules
                        If FactsRule.Conditions.IsAccumulatorInConditions(theCondition.AccumulatorName) Then
                            If Not FactsRule.Conditions.IsConfiguredForNonPar Then
                                theFact.NonParMemberPaymentValue = theFact.ParMemberPaymentValue
                            End If
                        End If
                    Next

                    Return Format(Math.Round(theFact.ParMemberPaymentValue, 2), "0.00")

                Case ActionValueTypes.NonParMemberPaymentValue

                    If TypeOf (theAction) Is AlterAccumulatorAction Then
                        theFact.NonParMemberPaymentValue = theAction.Execute(theFact.NonParMemberPaymentValue, theCondition)
                    Else
                        theFact.NonParMemberPaymentValue = theAction.Execute(theFact.NonParMemberPaymentValue)
                    End If

                    Return Format(Math.Round(theFact.NonParMemberPaymentValue, 2), "0.00")

            End Select

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Shared Function GetDisbursement(ByVal theFact As Fact) As Disbursement
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets a disbursement for this processor.
        ' </summary>
        ' <param name="fct"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/3/2006	Created
        '     [paulw] 9/27/2006   Per ACR MED-0029, added MultiLineCoPay type support
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim LineDisbursement As Disbursement

        Try
            LineDisbursement = New Disbursement

            LineDisbursement.FundPayment = theFact.ValuedAmount
            LineDisbursement.OriginalAmount = theFact.ValuedAmount
            LineDisbursement.MemberPayment = 0
            LineDisbursement.MultiLineFundPayment = theFact.MultiLineFundPaymentAmount
            LineDisbursement.Units = theFact.UnitAmount
            LineDisbursement.CheckMultiLineValue = theFact.CheckMultiLineValue
            LineDisbursement.PreSubTotalMemberPayment = theFact.PreSubTotalMemberPayment
            LineDisbursement.PreSubTotalParMemberPaymentValue = theFact.PreSubTotalParMemberPaymentValue
            LineDisbursement.PreSubTotalNonParMemberPaymentValue = theFact.PreSubTotalNonParMemberPaymentValue
            LineDisbursement.NonParMemberPaymentValue = theFact.NonParMemberPaymentValue
            LineDisbursement.ParMemberPaymentValue = theFact.ParMemberPaymentValue

            Return LineDisbursement

        Catch ex As Exception
            Throw
        Finally
            LineDisbursement = Nothing
        End Try

    End Function

    Private Shared Sub JoinDisbursementIntoFact(ByRef fact As Fact, ByVal disbursement As Disbursement, ByVal multiLineAmt As Decimal, ByVal checkMultiLine As Boolean)

        Try

            fact.PaymentAmount = disbursement.FundPayment
            fact.MemberAmount = disbursement.MemberPayment
            fact.MultiLineFundPaymentAmount = multiLineAmt
            fact.UnitAmount = disbursement.Units
            fact.CheckMultiLineValue = checkMultiLine
            fact.PreSubTotalMemberPayment = disbursement.PreSubTotalMemberPayment

            fact.PreSubTotalParMemberPaymentValue = disbursement.PreSubTotalParMemberPaymentValue
            fact.PreSubTotalNonParMemberPaymentValue = disbursement.PreSubTotalNonParMemberPaymentValue
            fact.ParMemberPaymentValue = disbursement.ParMemberPaymentValue
            fact.NonParMemberPaymentValue = disbursement.NonParMemberPaymentValue

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub ExecuteActions(ByVal theFact As Fact, ByVal theConditions As Conditions, ByVal alertDelegate As Alert.AddAlert)

        Dim TheAction As IAction
        Dim Processed As Integer
        Dim ActionValue As String
        Dim TheAlert As Alert
        Dim TestCondition As Condition
        Dim TheAlterAccumulatorAction As AlterAccumulatorAction

        Try

            'loop through all actions and execute them
            While _ActionQueue.Count > 0

                TestCondition = Nothing
                TheAction = Nothing

                TheAction = CType(_ActionQueue.Dequeue, IAction)

                If TypeOf (TheAction) Is AlterAccumulatorAction Then

                    TheAlterAccumulatorAction = CType(TheAction, AlterAccumulatorAction)
                    If _CurrentBinder.OriginalClaimIDForAccident > -1 Then
                        TheAlterAccumulatorAction.OriginalClaimIDForAccident = _CurrentBinder.OriginalClaimIDForAccident
                    End If

                    If Not TheAlterAccumulatorAction.Name.Trim.StartsWith("AC") Then
                        TheAlterAccumulatorAction.ApplyDate = theFact.LineDate
                    End If

                    For Each DE As DictionaryEntry In theConditions
                        If DirectCast(DE.Value, Condition).AccumulatorName IsNot Nothing AndAlso DirectCast(DE.Value, Condition).AccumulatorName.Length > 0 Then
                            If DirectCast(DE.Value, Condition).AccumulatorName = TheAlterAccumulatorAction.Name.Trim Then
                                TestCondition = DirectCast(DE.Value, Condition)
                                Exit For
                            End If
                        End If
                    Next
                End If

                ActionValue = ExecuteAction(TheAction, TestCondition, theFact)

                If TypeOf (TheAction) Is AlterAccumulatorAction Then
                    If TheAction.ActionValueType = ActionValueTypes.PreSubTotalMemberPaymentValue Then
                        If CDec(ActionValue) <> 0 Then
                            TheAlert = New Alert
                            TheAlert.AlertMessage = "Applied $" & ActionValue & " to " & If(TheAction.Name.Trim.StartsWith("AC"), "AAEB Accumulator", TheAction.Name.Trim)
                            TheAlert.SetSeverity(1)
                            TheAlert.Category = CategoryTypes.Accumulator
                            alertDelegate.Invoke(TheAlert)
                        End If
                    Else
                        If CDec(ActionValue) <> 0 Then
                            TheAlert = New Alert
                            TheAlert.AlertMessage = "Applied $" & ActionValue & " to " & TheAction.Name.Trim
                            TheAlert.LineNumber = theFact.LineNumber
                            TheAlert.SetSeverity(1)
                            TheAlert.Category = CategoryTypes.Accumulator
                            alertDelegate.Invoke(TheAlert)
                        End If
                    End If
                End If

                Processed += 1

            End While

        Catch ex As Exception
            Throw
        Finally
            TheAction = Nothing
            TheAlert = Nothing
            TestCondition = Nothing

        End Try

    End Sub

    Private Sub ExecuteActions(ByVal theFact As Fact, ByVal theConditions As Conditions)

        Dim Processed As Integer
        Dim TestCondition As Condition
        Dim TheAction As IAction
        Dim TheAlterAccumulatorAction As AlterAccumulatorAction

        Try

            'loop through all actions and execute them
            While _ActionQueue.Count > 0
                TestCondition = Nothing
                TheAction = Nothing

                TheAction = CType(_ActionQueue.Dequeue, IAction)

                If TypeOf (TheAction) Is AlterAccumulatorAction Then
                    TheAlterAccumulatorAction = CType(TheAction, AlterAccumulatorAction)
                    If _CurrentBinder.OriginalClaimIDForAccident > -1 Then
                        TheAlterAccumulatorAction.OriginalClaimIDForAccident = _CurrentBinder.OriginalClaimIDForAccident
                    End If

                    If Not TheAlterAccumulatorAction.Name.Trim.StartsWith("AC") Then
                        TheAlterAccumulatorAction.ApplyDate = theFact.LineDate
                    End If

                    For Each DE As DictionaryEntry In theConditions
                        If DirectCast(DE.Value, Condition).AccumulatorName = TheAlterAccumulatorAction.Name.Trim Then
                            TestCondition = DirectCast(DE.Value, Condition)
                            Exit For
                        End If
                    Next
                End If

                ExecuteAction(TheAction, TestCondition, theFact)

                Processed += 1
            End While

        Catch ex As Exception
            Throw
        Finally
            TestCondition = Nothing
            TheAction = Nothing

        End Try

    End Sub

    Private Sub ProcessFact(ByVal theFact As Fact)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Process a fact
        ' </summary>
        ' <param name="fct"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/3/2006	Created
        '     [paulw] 1/12/2006   Added 'GetSmallestHeadroom' functionality
        '                         to satisfy new requirement learned today.
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim ActionDelegate As IAction.ExecuteAction
        Dim AlertDelegate As Alert.AddAlert
        Dim TheAlert As Alert
        Dim TheConditions As Conditions
        Dim TheDisbursement As Disbursement
        Dim ProcessingArray As ArrayList
        Dim PreSubTotal As PreSubTotalRuleGroup
        Dim FundIncrease As FundIncreasingRuleGroup
        Dim FundDecrease As FundDecreasingRuleGroup
        Dim SpecialRule As SpecialRuleGroup

        Dim MultiLineAmnt As Decimal
        Dim ChkMultiLine As Boolean
        Dim DateToUse As Date
        Dim Amount As Decimal
        Dim AccumulatorName As String

        Try

            If theFact.ValuedAmount = 0 Then Return

            ProcessingArray = New ArrayList

            ActionDelegate = New IAction.ExecuteAction(AddressOf OnRuleEval)
            AlertDelegate = New Alert.AddAlert(AddressOf OnAlertRaised)

            _CurrentLineNumber = theFact.LineNumber

            If theFact.LineDateOverride = New Date Then
                DateToUse = theFact.LineDate
            Else
                DateToUse = theFact.LineDateOverride
            End If

            For Each TestRule As Rule In theFact.Rules
                ProcessingArray.Add(TestRule)
            Next

            TheDisbursement = GetDisbursement(theFact)

            '******** CHECK TO MAKE SURE $ IS LEFT IN THE FUND DECREASING GROUP *********************************************************
            FundDecrease = RuleGroup.GetFundDecreasingRuleGroup(ProcessingArray)

            Amount = FundDecrease.GetSmallestHeadroom(_CurrentBinder.BinderAccumulatorManager, DateToUse, AccumulatorName)

            If Amount <= 0 Then
                TheAlert = New Alert
                TheAlert.Category = CategoryTypes.Accumulator
                TheAlert.AlertMessage = AccumulatorName & " Maximum Met"
                TheAlert.LineNumber = _CurrentLineNumber
                TheAlert.SetSeverity(1)

                AlertDelegate.Invoke(TheAlert)

                JoinDisbursementIntoFact(theFact, TheDisbursement, MultiLineAmnt, ChkMultiLine)

                theFact.PaymentAmount = 0

                SetOutOfPocketActions(ActionDelegate, RuleGroup.GetFundIncreasingRuleGroup(ProcessingArray), DateToUse, theFact, AlertDelegate)

                Return
            End If

            '******** PROCESS THE PRE-SUBTOTAL GROUP **************************************************************************************
            PreSubTotal = RuleGroup.GetPreSubTotalRuleGroup(ProcessingArray)

            'change the disbursement
            TheDisbursement = PreSubTotal.Eval(TheDisbursement, _CurrentBinder.BinderAccumulatorManager, DateToUse, ActionDelegate, AlertDelegate)
            MultiLineAmnt = TheDisbursement.MultiLineFundPayment
            ChkMultiLine = TheDisbursement.CheckMultiLineValue

            If TheDisbursement.FundPayment > 0 Then
                '********* PROCESS THE FUND-INCREASING GROUP *************************************************************************************
                FundIncrease = RuleGroup.GetFundIncreasingRuleGroup(ProcessingArray)
                'change the disbursement
                TheDisbursement = FundIncrease.Eval(TheDisbursement, _CurrentBinder.BinderAccumulatorManager, DateToUse, ActionDelegate, AlertDelegate)

                '*********** PROCESS THE FUND-DECREASING GROUP ***********************************************************************************
                FundDecrease = RuleGroup.GetFundDecreasingRuleGroup(ProcessingArray)
                'change the disbursement
                TheDisbursement = FundDecrease.Eval(TheDisbursement, _CurrentBinder.BinderAccumulatorManager, DateToUse, ActionDelegate, AlertDelegate)

                '**********************************************************************************************
            Else 'the fund pays $0 so we want to make sure that the fund does not pay any units either
                theFact.UnitAmount = 0
            End If

            '*********** PROCESS THE SPECIAL GROUP ***********************************************************************************
            'currently this only includes accident 'stuff'
            SpecialRule = RuleGroup.GetSpecialRuleGroup(ProcessingArray)
            TheDisbursement = SpecialRule.Eval(TheDisbursement, _CurrentBinder.BinderAccumulatorManager, _CurrentBinder.GetBinderItem(_CurrentLineNumber).IncidentDate, ActionDelegate, AlertDelegate)
            '**********************************************************************************************

            JoinDisbursementIntoFact(theFact, TheDisbursement, MultiLineAmnt, ChkMultiLine)

            TheConditions = New Conditions
            For Each TestRule As Rule In ProcessingArray
                For Each DE As DictionaryEntry In TestRule.Conditions
                    TheConditions.Add(DirectCast(DE.Value, Condition))
                Next
            Next

            If AccumulatorRule.LogAllAccumulatorActions Then
                ExecuteActions(theFact, TheConditions, AlertDelegate)
            Else
                ExecuteActions(theFact, TheConditions)
            End If

        Catch ex As Exception
            Throw
        Finally

            ActionDelegate = Nothing
            AlertDelegate = Nothing
            TheAlert = Nothing
            TheConditions = Nothing
            TheDisbursement = Nothing
            ProcessingArray = Nothing
            PreSubTotal = Nothing
            FundIncrease = Nothing
            FundDecrease = Nothing
            SpecialRule = Nothing

        End Try

    End Sub
    Private Sub SetOutOfPocketActions(ByVal actionDelegate As IAction.ExecuteAction, ByVal fundIncrease As FundIncreasingRuleGroup, ByVal dateOfService As Date?, ByVal theFact As Fact, ByVal alertDelegate As Alert.AddAlert)

        Dim TheAlterAccumulatorAction As AlterAccumulatorAction
        Dim TheActionArg As ActionArgs

        Try
            For I As Integer = 0 To fundIncrease.Count - 1
                For Each DE As DictionaryEntry In fundIncrease.GetRuleAtIndex(I).Conditions
                    TheActionArg = New ActionArgs
                    TheAlterAccumulatorAction = New AlterAccumulatorAction(_CurrentBinder.BinderAccumulatorManager)
                    TheAlterAccumulatorAction.Name = DirectCast(DE.Value, Condition).AccumulatorName
                    TheAlterAccumulatorAction.ApplyDate = dateOfService
                    TheAlterAccumulatorAction.ActionValueType = ActionValueTypes.MemberPaymentValue
                    TheActionArg.Action = TheAlterAccumulatorAction
                    actionDelegate.Invoke(TheActionArg)
                Next

                If AccumulatorRule.LogAllAccumulatorActions Then
                    ExecuteActions(theFact, fundIncrease.GetRuleAtIndex(I).Conditions, alertDelegate)
                Else
                    ExecuteActions(theFact, fundIncrease.GetRuleAtIndex(I).Conditions)
                End If
            Next

        Catch ex As Exception
            Throw
        Finally
            TheAlterAccumulatorAction = Nothing
            TheActionArg = Nothing
        End Try

    End Sub
#End Region
#Region "Delegate Methods"

    Private Sub OnAlertRaised(ByVal theAlert As Alert)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' When an alert is raised this delegate is called in order to add alerts.  this
        '   was done to avoid passing the alert manager too deep.
        ' </summary>
        ' <param name="actArgs"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/21/2006	Created
        '     [paulw] 1/16/2007   Added logic to Satisfy ACR MED-195
        '     [pauw]  3/6/2007    Added logic to Satisfy ACR MED-242
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            If theAlert.AlertMessage.ToUpper.IndexOf("ACCUMULATOR IS PRESENT") > -1 OrElse theAlert.AlertMessage.ToUpper.IndexOf("A 110") > -1 Then
                If _CurrentBinder.GetBinderItem(_CurrentBinder.Facts(0).LineNumber).Procedure.PlanType.IndexOf("110") > 0 Then
                    theAlert.LineNumber = _CurrentLineNumber
                    If theAlert.Category = CategoryTypes.Accumulator Then
                        If Not _CurrentBinder.BinderAlertManager.IsAlertAlreadyPresent(theAlert.AlertMessage, _CurrentLineNumber) Then
                            _CurrentBinder.BinderAlertManager.AddAlert(theAlert)
                        End If
                    Else
                        _CurrentBinder.BinderAlertManager.AddAlert(theAlert)
                    End If
                End If
            Else
                theAlert.LineNumber = _CurrentLineNumber
                'Is Hospital?
                If _CurrentBinder.TypeOfBinder = 4 Then
                    If theAlert.AlertMessage.ToUpper.IndexOf("COPAY") > -1 Then
                        theAlert.AlertMessage = "Applied Hospital CoPay.  ADD REASON CODE"
                        theAlert.SetSeverity(15)
                    End If
                End If

                If theAlert.Category = CategoryTypes.Accumulator Then
                    If Not _CurrentBinder.BinderAlertManager.IsAlertAlreadyPresent(theAlert.AlertMessage, _CurrentLineNumber) Then
                        _CurrentBinder.BinderAlertManager.AddAlert(theAlert)
                    End If
                Else
                    _CurrentBinder.BinderAlertManager.AddAlert(theAlert)
                End If
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub OnRuleEval(ByVal actionArguments As ActionArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' When a rule has been evaluated this delegate is called.
        '   This delegate is here to make changes to the acccumulator manager
        '   without having to pass the accumulator manager too deep.  We also
        '   add actions to the queue here.
        ' </summary>
        ' <param name="actArgs"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/21/2006	Created
        '     [paulw] 5/15/2007   See notes in regards to Accidents below
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim TheAction As IAction
        Dim NewActionQueue As ActionQueue
        Dim LifetimeInRules As Boolean = False
        Dim LifetimeInActions As Boolean = False
        Dim TheFacts As Facts
        Dim TheAlterAccumulatorAction As AlterAccumulatorAction

        Try

            TheAction = actionArguments.Action
            TheAction.ClaimID = _CurrentBinder.ClaimNumber
            TheAction.LineNumber = _CurrentLineNumber

            If TypeOf (TheAction) Is AlterAccumulatorAction Then
                TheAlterAccumulatorAction = CType(TheAction, AlterAccumulatorAction)
                TheAlterAccumulatorAction.AccumulatorManager = _CurrentBinder.BinderAccumulatorManager
                If TheAlterAccumulatorAction.Name.Trim.StartsWith("AC") Then
                    TheAlterAccumulatorAction.ApplyDate = _CurrentBinder.GetBinderItem(_CurrentLineNumber).IncidentDate
                End If
            End If

            _ActionQueue.AddAction(TheAction)

            If TypeOf (TheAction) Is AlterAccumulatorAction Then
                '1.  if this is an accident accumulator alteration
                '2.  go through all the facts and get all Standard Accumulator Rules with
                '     MM3 as its name
                '3.  see if that Standard Accumulator Rule (MM3) exist in the action queue
                '4.  if they dont exist in the action queue, add an action for them.
                'Note:  This can be done on the ACC accumulator because it would be the last
                '       accumulator added to the action queue.
                'Note:  I am trying to make this solution as non-hard-coded as possible.
                '       However, some of the hard coding is inevitable given that we dont
                '       want to alter the architecture very much, if any at all.

                TheAlterAccumulatorAction = CType(TheAction, AlterAccumulatorAction)

                If TheAlterAccumulatorAction.Name.Trim.StartsWith("AC") Then

                    TheAlterAccumulatorAction.ApplyDate = _CurrentBinder.GetBinderItem(_CurrentLineNumber).IncidentDate

                    NewActionQueue = New ActionQueue

                    TheFacts = _CurrentBinder.Facts

                    For Each TestFact As Fact In TheFacts
                        For Each TestRule As Rule In TestFact.Rules
                            If TypeOf (TestRule) Is StandardAccumulatorRule Then
                                For Each DE As DictionaryEntry In TestRule.Conditions
                                    'Have to hardcode this because we dont know (here) if it is a lifetime or not
                                    ' given our current architecture
                                    If DirectCast(DE.Value, Condition).AccumulatorName = "MM3" Then
                                        LifetimeInRules = True
                                    End If
                                Next
                            End If
                        Next
                    Next

                    If LifetimeInRules Then

                        While _ActionQueue.Count > 0
                            TheAction = CType(_ActionQueue.Dequeue, IAction)
                            'Have to hardcode this because we dont know (here) if it is a lifetime or not
                            ' given our current architecture
                            If TheAction.Name = "MM3" Then
                                LifetimeInActions = True
                            End If
                            NewActionQueue.AddAction(TheAction)
                        End While

                        While NewActionQueue.Count > 0
                            TheAction = CType(NewActionQueue.Dequeue(), IAction)
                            _ActionQueue.AddAction(TheAction)
                        End While

                        If Not LifetimeInActions Then
                            TheAction = New AlterAccumulatorAction(_CurrentBinder.BinderAccumulatorManager)
                            TheAction.ActionValueType = ActionValueTypes.FundPaymentValue
                            TheAction.LineNumber = _CurrentLineNumber

                            DirectCast(TheAction, AlterAccumulatorAction).ApplyDate = _CurrentBinder.GetBinderItem(_CurrentLineNumber).DateOfService

                            TheAction.ClaimID = _CurrentBinder.ClaimNumber
                            TheAction.Name = "MM3"

                            _ActionQueue.AddAction(TheAction)

                        End If
                    End If
                End If 'ACC accumulator alteration
            End If 'alter accumulator action

        Catch ex As Exception

            Throw

        Finally

            TheAction = Nothing
            NewActionQueue = Nothing
            TheFacts = Nothing

        End Try

    End Sub
#End Region

#Region "Clean Up"
    'Public Overloads Sub Dispose() Implements IDisposable.Dispose
    '    Dispose(True)
    '    GC.SuppressFinalize(Me)
    'End Sub

    'Protected Overloads Sub Dispose(ByVal disposing As Boolean)
    '    If _disposed = False Then
    '        If disposing Then

    '            'Debug.Print(TypeName(Me) & " Dispose: " & ClassGuid.ToString)

    '            _currentBinder = Nothing
    '            ' Free other state (managed objects).

    '            _disposed = True
    '        End If
    '        ' Free your own state (unmanaged objects).
    '        ' Set large fields to null.
    '    End If
    'End Sub

    'Protected Overrides Sub Finalize()
    '    'Debug.Print(TypeName(Me) & " Finalize: " & ClassGuid.ToString)
    '    MyBase.Finalize()
    'End Sub

#End Region

End Class