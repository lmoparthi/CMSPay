

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Binder
''' Class	 : CMS.Binder.Fact
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class represents a 'Fact'
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/5/2006	Created
'''     [paulw] 9/27/2006   Per ACR MED-0029, added MultiLineCoPay type support
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public Class Fact

#Region "Private Variables"

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _ValuedAmount As Decimal
    Private _PaymentAmount As Decimal
    Private _MemberAmount As Decimal
    Private _Rules As New ArrayList
    Private _LineNumber As Short
    Private _ClaimID As Integer
    Private _UnitAmount As Decimal
    Private _HRAInEligible As Boolean
    Private _IsPreventative As Boolean
    Private _MultiLineFundPayment As Decimal
    Private _PreSubTotalMemberPayment As Decimal
    Private _Status As BinderItemStatus
    Private _CheckMultiLine As Boolean
    Private _LineDate As Date
    Private _LineDateOverride As Date
    Private _IncidentDate As Date
    Private _RuleSetIDUsed As Integer
    Private _RuleSetName As String
    Private _PreSubTotalNonParMemberPaymentValue As Decimal
    Private _PreSubTotalParMemberPaymentValue As Decimal
    Private _ParMemberPaymentValue As Decimal
    Private _NonParMemberPaymentValue As Decimal
    Private _ProcedureCode As String
    Private _ClaimLevelUnitAmount As New Hashtable
    Private _ClaimLevelUnitAmountSet As New Hashtable

    Friend Property ClaimLevelUnitAmount(ByVal procedureCode As String) As Decimal
        Get
            If _ClaimLevelUnitAmount.ContainsKey(procedureCode) Then
                Return _ClaimLevelUnitAmount(procedureCode)
            Else
                Return -1
            End If
        End Get
        Set(ByVal value As Decimal)
            If _ClaimLevelUnitAmount.ContainsKey(procedureCode) Then
                _ClaimLevelUnitAmount(procedureCode) = value
            Else
                _ClaimLevelUnitAmount.Add(procedureCode, value)
            End If
        End Set
    End Property

    Friend Property ClaimLevelUnitAmountSet(ByVal procedureCode As String) As Boolean
        Get
            If _claimLevelUnitAmountSet.ContainsKey(procedureCode) Then
                Return _claimLevelUnitAmountSet(procedureCode)
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            If _ClaimLevelUnitAmountSet.ContainsKey(procedureCode) Then
                _ClaimLevelUnitAmountSet(procedureCode) = Value
            Else
                _ClaimLevelUnitAmountSet.Add(procedureCode, Value)
            End If
        End Set
    End Property
#End Region

#Region "Properties"
    Public Property PreSubTotalNonParMemberPaymentValue() As Decimal
        Get
            Return _PreSubTotalNonParMemberPaymentValue
        End Get
        Set(ByVal value As Decimal)
            _PreSubTotalNonParMemberPaymentValue = value
        End Set
    End Property
    Public Property PreSubTotalParMemberPaymentValue() As Decimal
        Get
            Return _PreSubTotalParMemberPaymentValue
        End Get
        Set(ByVal value As Decimal)
            _PreSubTotalParMemberPaymentValue = value
        End Set
    End Property

    Public Property ParMemberPaymentValue() As Decimal
        Get
            Return _ParMemberPaymentValue
        End Get
        Set(ByVal value As Decimal)
            _ParMemberPaymentValue = value
        End Set
    End Property

    Public Property NonParMemberPaymentValue() As Decimal
        Get
            Return _NonParMemberPaymentValue
        End Get
        Set(ByVal value As Decimal)
            _NonParMemberPaymentValue = value
        End Set
    End Property

    Public Property RuleSetIdUsed() As Integer
        Get
            Return _RuleSetIDUsed
        End Get
        Set(ByVal value As Integer)
            _RuleSetIDUsed = Value
        End Set
    End Property

    Public Property RuleSetName() As String
        Get
            Return _RuleSetName
        End Get
        Set(ByVal value As String)
            _RuleSetName = value
        End Set
    End Property

    Public Property ProcedureCodeUsed() As String
        Get
            Return _ProcedureCode
        End Get
        Set(ByVal value As String)
            _ProcedureCode = value
        End Set
    End Property

    Public Property LineDate() As Date
        Get
            Return _lineDate
        End Get
        Set(ByVal value As Date)
            _LineDate = Value
        End Set
    End Property

    Public Property IncidentDate() As Date
        Get
            Return _incidentDate
        End Get
        Set(ByVal value As Date)
            _IncidentDate = Value
        End Set
    End Property

    Public Property LineDateOverride() As Date
        Get
            Return _lineDateOverride
        End Get
        Set(ByVal value As Date)
            _LineDateOverride = Value
        End Set
    End Property

    Public Property CheckMultiLineValue() As Boolean
        Get
            Return _checkMultiLine
        End Get
        Set(ByVal value As Boolean)
            _CheckMultiLine = Value
        End Set
    End Property

    Public Property Status() As BinderItemStatus
        Get
            Return _Status
        End Get
        Set(ByVal value As BinderItemStatus)
            _Status = Value
        End Set
    End Property

    Public Property ValuedAmount() As Decimal
        Get
            Return _ValuedAmount
        End Get
        Set(ByVal value As Decimal)
            _ValuedAmount = value
        End Set
    End Property

    Public Property PaymentAmount() As Decimal
        Get
            Return _PaymentAmount
        End Get
        Set(ByVal value As Decimal)
            _PaymentAmount = Math.Round(value, 2)
        End Set
    End Property

    Public Property PreSubTotalMemberPayment() As Decimal
        Get
            Return _PreSubTotalMemberPayment
        End Get
        Set(ByVal value As Decimal)
            _PreSubTotalMemberPayment = Math.Round(value, 2)
        End Set
    End Property

    Public Property MultiLineFundPaymentAmount() As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets multiline amount
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	9/27/2006	Created Per ACR MED-0029, added MultiLineCoPay type support
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _MultiLineFundPayment
        End Get
        Set(ByVal value As Decimal)
            _MultiLineFundPayment = Math.Round(value, 2)
        End Set
    End Property

    Public Property MemberAmount() As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets member amount
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/7/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _MemberAmount
        End Get
        Set(ByVal value As Decimal)
            _MemberAmount = Math.Round(value, 2)
        End Set
    End Property

    Public ReadOnly Property Rules() As ArrayList
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' gets the rules for the processor
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/21/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Rules
        End Get
    End Property

    Public Property ClaimID() As Integer
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' gets/sets claim number
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/22/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _ClaimID
        End Get
        Set(ByVal value As Integer)
            _ClaimID = Value
        End Set
    End Property

    Public Property LineNumber() As Short
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' gets/sets line number
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/22/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _LineNumber
        End Get
        Set(ByVal value As Short)
            _LineNumber = value
        End Set
    End Property

    Public Property UnitAmount() As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the unit amount
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/3/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _UnitAmount
        End Get
        Set(ByVal value As Decimal)
            _UnitAmount = value
        End Set
    End Property

    Public Property HRAInEligible() As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the HRA Eligibility
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[MarkS]	11/20/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _HRAInEligible
        End Get
        Set(ByVal value As Boolean)
            _HRAInEligible = value
        End Set
    End Property

    Public Property IsPreventative() As Boolean
        Get
            Return _IsPreventative
        End Get
        Set(ByVal value As Boolean)
            _IsPreventative = value
        End Set
    End Property

#End Region

#Region "Methods"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Get a fact from a BinderItem
    ' </summary>
    ' <param name="item"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	4/5/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function GetFact(ByVal binderItem As BinderItem, ByVal ruleSetType As Integer) As Fact

        'check for errors
        If binderItem Is Nothing Then
            Throw New ArgumentNullException("BinderItem")
        End If

        If binderItem.Procedure Is Nothing Then
            Throw New ArgumentException("item must have a valid procedure", "Procedure")
        End If

        If binderItem.Procedure.RuleSets.Count < 1 Then
            Throw New ArgumentException("item must have a valid ruleset", "RuleSet")
        End If

        If binderItem.LineNumber = -1 Then
            Throw New ArgumentException("item must have a valid line item", "LineNumber")
        End If

        If binderItem.ValuedAmount < 0 Then
            Throw New ArgumentException("Amount is negative", "ValuedAmount")
        End If

        If binderItem.MemberAmount < 0 Then
            Throw New ArgumentException("item must have a valid MemberAmount ", "MemberAmount")
        End If

        Dim Found As Boolean = False
        Dim TheFact As Fact

        Dim DurationType As DateType
        Dim Duration As Integer

        Try

            For Each ProcedureRuleSet As RuleSet In binderItem.Procedure.RuleSets
                If ProcedureRuleSet.RulesetType = ruleSetType Then
                    Found = True
                End If
            Next

            If Not Found Then
                Throw New ArgumentException("item's procedure does not contain that type of ruleset", "RulesetType")
            End If

            TheFact = New Fact

            For Each ProcedureRuleSet As RuleSet In binderItem.Procedure.RuleSets
                If ProcedureRuleSet.RulesetType = ruleSetType Then
                    If ProcedureRuleSet.MaxUnits <= binderItem.UnitAmount Then
                        binderItem.UnitAmount = ProcedureRuleSet.MaxUnits
                        TheFact.ClaimLevelUnitAmountSet(binderItem.Procedure.ProcedureCode) = True
                    End If

                    TheFact.RuleSetIdUsed = ProcedureRuleSet.RulesetID
                    TheFact.RuleSetName = ProcedureRuleSet.RuleSetName

                    For Each ProcedureRule As Rule In ProcedureRuleSet
                        TheFact.Rules.Add(ProcedureRule)
                    Next
                End If
            Next

            TheFact.ProcedureCodeUsed = binderItem.Procedure.ProcedureCode

            TheFact.LineNumber = binderItem.LineNumber
            TheFact.HRAInEligible = binderItem.IsHRAInEligible(10)
            TheFact.IsPreventative = binderItem.IsPreventative(11)

            TheFact.ValuedAmount = binderItem.ValuedAmount
            TheFact.MemberAmount = binderItem.MemberAmount
            TheFact.PaymentAmount = binderItem.PaymentAmount
            TheFact.UnitAmount = binderItem.UnitAmount

            If TheFact.ClaimLevelUnitAmountSet(TheFact.ProcedureCodeUsed) Then TheFact.ClaimLevelUnitAmount(TheFact.ProcedureCodeUsed) = binderItem.UnitAmount

            TheFact.LineDate = binderItem.DateOfService
            TheFact.IncidentDate = binderItem.IncidentDate

            If binderItem.IncidentDate <> New Date Then

                DurationType = binderItem.AccidentDurationType(ruleSetType)
                Duration = binderItem.AccidentDuration(ruleSetType)

                Select Case DurationType
                    Case DateType.Days
                        If binderItem.DateOfService >= binderItem.IncidentDate Then
                            If binderItem.DateOfService > binderItem.IncidentDate.AddDays(Duration + 1) Then
                                binderItem.RemoveAccidentAccumulators()
                            End If
                        End If
                    Case DateType.Months
                        If binderItem.DateOfService >= binderItem.IncidentDate Then
                            If binderItem.DateOfService > binderItem.IncidentDate.AddMonths(Duration + 1) Then
                                binderItem.RemoveAccidentAccumulators()
                            End If
                        End If
                    Case DateType.Quarters
                        If binderItem.DateOfService >= binderItem.IncidentDate Then
                            If binderItem.DateOfService > binderItem.IncidentDate.AddMonths((Duration + 1) * 3) Then
                                binderItem.RemoveAccidentAccumulators()
                            End If
                        End If
                    Case DateType.Rollover
                        If binderItem.DateOfService >= binderItem.IncidentDate Then
                            If binderItem.DateOfService > binderItem.IncidentDate.AddMonths((Duration + 1) * 15) Then
                                binderItem.RemoveAccidentAccumulators()
                            End If
                        End If
                    Case DateType.Years
                        If binderItem.DateOfService >= binderItem.IncidentDate Then
                            If binderItem.DateOfService > binderItem.IncidentDate.AddYears((Duration + 1)) Then
                                binderItem.RemoveAccidentAccumulators()
                            End If
                        End If
                End Select
            End If

            Return TheFact

        Catch ex As Exception
            Throw
        Finally
            TheFact = Nothing
            DurationType = Nothing

        End Try
    End Function

    Public Function GetRuleByType(ByVal ruleType As RuleTypes) As Rule
        Try

            For Each TheRule As Rule In Me.Rules
                If TypeOf TheRule Is DeductibleRule AndAlso ruleType = RuleTypes.Deductible Then
                    Return TheRule
                ElseIf ruleType = RuleTypes.Accident AndAlso TypeOf TheRule Is AccidentRule Then
                    Return TheRule
                ElseIf ruleType = RuleTypes.CoInsurance AndAlso TypeOf TheRule Is CoInsuranceRule Then
                    Return TheRule
                ElseIf ruleType = RuleTypes.CoPay AndAlso TypeOf TheRule Is CoPayRule Then
                    Return TheRule
                ElseIf ruleType = RuleTypes.Deny AndAlso TypeOf TheRule Is DenyRule Then
                    Return TheRule
                ElseIf ruleType = RuleTypes.MultiLineCoPay AndAlso TypeOf TheRule Is MultilineCoPayRule Then
                    Return TheRule
                ElseIf ruleType = RuleTypes.OutOfPocket AndAlso TypeOf TheRule Is OutOfPocketRule Then
                    Return TheRule
                ElseIf ruleType = RuleTypes.ProceduralAllowance AndAlso TypeOf TheRule Is ProceduralAllowanceRule Then
                    Return TheRule
                ElseIf ruleType = RuleTypes.ProviderWriteOff AndAlso TypeOf TheRule Is ProviderWriteOffRule Then
                    Return TheRule
                ElseIf ruleType = RuleTypes.Standard AndAlso TypeOf TheRule Is StandardAccumulatorRule Then
                    Return TheRule
                ElseIf ruleType = RuleTypes.Original AndAlso TypeOf TheRule Is OriginalRule Then
                    Return TheRule
                End If
            Next

            Return Nothing

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Function GetRuleByType(ByVal comparisonRule As Rule) As Rule

        Try

            For Each TheRule As Rule In _Rules
                If TheRule.GetType Is comparisonRule.GetType() Then
                    Return TheRule
                End If
            Next

            Return Nothing

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Function GetRuleByType(ByVal accumulatorName As String) As Rule
        Try
            For Each TheRule As Rule In Me.Rules
                For Each TheCondition As DictionaryEntry In TheRule.Conditions
                    If DirectCast(TheCondition.Value, Condition).AccumulatorName IsNot Nothing Then
                        If DirectCast(TheCondition.Value, Condition).AccumulatorName.ToUpper = accumulatorName.Trim.ToUpper Then
                            Return TheRule
                        End If
                    End If
                Next
            Next

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function
#End Region

End Class