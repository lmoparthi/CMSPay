''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.ProcessorEngine
''' Class	 : CMS.ProcessorEngine.RuleGroup
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' Represents a group of rules
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/12/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public MustInherit Class RuleGroup
    Inherits CollectionBase


#Region "Constructors"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Costructor
    ' </summary>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	4/12/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Sub New()
        MyBase.New()
    End Sub
#End Region

#Region "Other Methods"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Adds a rule to this group
    ' </summary>
    ' <param name="item">a Rule object</param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	4/12/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Sub AddRule(ByVal rule As Rule)
        List.Add(rule)
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Evaluates the entire collection
    ' </summary>
    ' <param name="dsbursement">The disbursment object</param>
    ' <param name="accumManager">The MemberAccumlator object</param>
    ' <param name="dateOfService">The date of the claim</param>
    ' <param name="dlgate">The delegate that handles the execution of actions that come from the processor</param>
    ' <param name="alrtDelagate">The delegate that handles alerts</param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	4/12/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public MustOverride Function Eval(ByVal disbursement As Disbursement, ByVal memberAccumulatorManager As MemberAccumulatorManager, ByVal dateOfService As Date?, ByVal actionDelegate As IAction.ExecuteAction, ByVal alertDelegate As Alert.AddAlert) As Disbursement

#End Region

#Region "Shared Methods"
    '' -----------------------------------------------------------------------------
    '' <summary>
    '' goes through all the rules and finds the rules that meet the PreSubTotalGroup
    ''  criteria; those rules are added to the collection
    '' </summary>
    '' <param name="rules"></param>
    '' <returns></returns>
    '' <remarks>
    '' </remarks>
    '' <history>
    '' 	[paulw]	4/12/2006	Created
    '' </history>
    '' -----------------------------------------------------------------------------
    'Public Shared Function GetPreSubTotalRuleGroup(ByVal rules As Rule()) As PreSubTotalRuleGroup
    '    Dim PreSubTotalRuleGroup As New PreSubTotalRuleGroup
    '    For Each Rule As Rule In rules
    '        If TypeOf Rule Is CoResponsibilityRule OrElse TypeOf Rule Is DeductibleRule OrElse TypeOf Rule Is FundNonPaymentRule Then
    '            PreSubTotalRuleGroup.AddRule(Rule)
    '        End If
    '    Next
    '    Return PreSubTotalRuleGroup
    '    'Dim a As MedicalProcessor.ExecuteAction
    'End Function


    Public Shared Function GetPreSubTotalRuleGroup(ByVal rules As ArrayList) As PreSubTotalRuleGroup
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' goes through all the rules and finds the rules that meet the PreSubTotalGroup
        '  criteria; those rules are added to the collection
        ' </summary>
        ' <param name="rules"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/12/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim PreSubTotalRuleGroup As New PreSubTotalRuleGroup
        For Each TestRule As Rule In rules
            If TypeOf TestRule Is CoResponsibilityRule OrElse TypeOf TestRule Is DeductibleRule OrElse TypeOf TestRule Is FundNonPaymentRule Then
                PreSubTotalRuleGroup.AddRule(TestRule)
            End If
        Next
        Return PreSubTotalRuleGroup
    End Function

    '' -----------------------------------------------------------------------------
    '' <summary>
    '' goes through all the rules and finds the rules that meet the PreSubTotalGroup
    ''  criteria; those rules are added to the collection
    '' </summary>
    '' <param name="rules"></param>
    '' <returns></returns>
    '' <remarks>
    '' </remarks>
    '' <history>
    '' 	[paulw]	4/12/2006	Created
    '' </history>
    '' -----------------------------------------------------------------------------
    'Public Shared Function GetPreSubTotalRuleGroup(ByVal ruleGroup As RuleGroup) As PreSubTotalRuleGroup
    '    Dim PreSubTotalRuleGroup As New PreSubTotalRuleGroup
    '    For Each TestRule As Rule In ruleGroup
    '        If TypeOf TestRule Is CoResponsibilityRule OrElse TypeOf TestRule Is DeductibleRule OrElse TypeOf TestRule Is FundNonPaymentRule Then
    '            PreSubTotalRuleGroup.AddRule(TestRule)
    '        End If
    '    Next
    '    Return PreSubTotalRuleGroup
    'End Function

    '' -----------------------------------------------------------------------------
    '' <summary>
    '' goes through all the rules and finds the rules that meet the FundIncreasing
    ''  criteria; those rules are added to the collection
    '' </summary>
    '' <param name="rules"></param>
    '' <returns></returns>
    '' <remarks>
    '' </remarks>
    '' <history>
    '' 	[paulw]	4/12/2006	Created
    '' </history>
    '' -----------------------------------------------------------------------------
    'Public Shared Function GetFundIncreasingRuleGroup(ByVal ruleGroup As RuleGroup) As FundIncreasingRuleGroup
    '    Dim FundIncreasingRuleGroup As New FundIncreasingRuleGroup
    '    For Each TestRule As Rule In ruleGroup
    '        If TypeOf TestRule Is OutOfPocketRule Then
    '            FundIncreasingRuleGroup.AddRule(TestRule)
    '        End If
    '    Next
    '    Return FundIncreasingRuleGroup
    'End Function
    ' -----------------------------------------------------------------------------
    ' <summary>
    '
    ' </summary>
    ' <param name="rules"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	4/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function GetFundIncreasingRuleGroup(ByVal rules As ArrayList) As FundIncreasingRuleGroup
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' goes through all the rules and finds the rules that meet the FundIncreasing
        '  criteria; those rules are added to the collection
        ' </summary>
        ' <param name="rules"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/12/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------        
        Dim FundIncreasingRuleGroup As New FundIncreasingRuleGroup
        For Each TestRule As Rule In rules
            If TypeOf TestRule Is OutOfPocketRule Then
                FundIncreasingRuleGroup.AddRule(TestRule)
            End If
        Next
        Return FundIncreasingRuleGroup
    End Function

    '' -----------------------------------------------------------------------------
    '' <summary>
    '' goes through all the rules and finds the rules that meet the FundIncreasing
    ''  criteria; those rules are added to the collection
    '' </summary>
    '' <param name="rules"></param>
    '' <returns></returns>
    '' <remarks>
    '' </remarks>
    '' <history>
    '' 	[paulw]	4/12/2006	Created
    '' </history>
    '' -----------------------------------------------------------------------------
    'Public Shared Function GetFundIncreasingRuleGroup(ByVal rules As Rule()) As FundIncreasingRuleGroup
    '    Dim FundIncreasingRuleGroup As New FundIncreasingRuleGroup
    '    For Each TestRule As Rule In rules
    '        If TypeOf TestRule Is OutOfPocketRule Then
    '            FundIncreasingRuleGroup.AddRule(TestRule)
    '        End If
    '    Next
    '    Return FundIncreasingRuleGroup
    'End Function

    '' -----------------------------------------------------------------------------
    '' <summary>
    '' goes through all the rules and finds the rules that meet the FundDecreasing
    ''  criteria; those rules are added to the collection
    '' </summary>
    '' <param name="rules"></param>
    '' <returns></returns>
    '' <remarks>
    '' </remarks>
    '' <history>
    '' 	[paulw]	4/12/2006	Created
    '' </history>
    '' -----------------------------------------------------------------------------
    'Public Shared Function GetFundDecreasingRuleGroup(ByVal rules As Rule()) As FundDecreasingRuleGroup
    '    Dim FundDecreasingRuleGroup As New FundDecreasingRuleGroup
    '    For Each Rule As Rule In rules
    '        If TypeOf Rule Is StandardAccumulatorRule Then
    '            FundDecreasingRuleGroup.AddRule(Rule)
    '        End If
    '    Next
    '    Return FundDecreasingRuleGroup
    'End Function


    Public Shared Function GetFundDecreasingRuleGroup(ByVal rules As ArrayList) As FundDecreasingRuleGroup
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' goes through all the rules and finds the rules that meet the FundDecreasing
        '  criteria; those rules are added to the collection
        ' </summary>
        ' <param name="rules"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/12/2006	Created
        ' </history>
        ' ----------------------------------------------------------------------------- 
        Dim FundDecreasingRuleGroup As New FundDecreasingRuleGroup
        For Each TestRule As Rule In rules
            If TypeOf TestRule Is StandardAccumulatorRule Then
                FundDecreasingRuleGroup.AddRule(TestRule)
            End If
        Next
        Return FundDecreasingRuleGroup
    End Function

    '' -----------------------------------------------------------------------------
    '' <summary>
    '' goes through all the rules and finds the rules that meet the FundDecreasing
    ''  criteria; those rules are added to the collection
    '' </summary>
    '' <param name="rules"></param>
    '' <returns></returns>
    '' <remarks>
    '' </remarks>
    '' <history>
    '' 	[paulw]	4/12/2006	Created
    '' </history>
    '' -----------------------------------------------------------------------------
    'Public Shared Function GetFundDecreasingRuleGroup(ByVal ruleGroup As RuleGroup) As FundDecreasingRuleGroup
    '    Dim FundDecreasingRuleGroup As New FundDecreasingRuleGroup
    '    For Each Rule As Rule In ruleGroup
    '        If TypeOf Rule Is StandardAccumulatorRule Then
    '            FundDecreasingRuleGroup.AddRule(Rule)
    '        End If
    '    Next
    '    Return FundDecreasingRuleGroup
    'End Function

    'Public Shared Function GetSpecialRuleGroup(ByVal ruleGroup As RuleGroup) As SpecialRuleGroup
    '    Dim SpecialRuleGroup As New SpecialRuleGroup
    '    For Each Rule As Rule In ruleGroup
    '        If TypeOf Rule Is AccidentRule Then
    '            SpecialRuleGroup.AddRule(DirectCast(Rule, AccidentRule))
    '        End If
    '    Next
    '    Return SpecialRuleGroup
    'End Function

    Public Shared Function GetSpecialRuleGroup(ByVal rules As ArrayList) As SpecialRuleGroup
        Dim SpecialRuleGroup As New SpecialRuleGroup
        For Each TestRule As Rule In rules
            If TypeOf TestRule Is AccidentRule Then
                SpecialRuleGroup.AddRule(DirectCast(TestRule, AccidentRule))
            End If
        Next
        Return SpecialRuleGroup
    End Function

#End Region
    Public Function GetRuleAtIndex(ByVal index As Integer) As Rule
        Return CType(List.Item(index), Rule)
    End Function
End Class
#Region "Enums"
Public Enum RuleGroupTypes
    PreSubTotal = 0
    OutOfPocket = 1
    Standard = 2
End Enum
#End Region
