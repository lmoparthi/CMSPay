''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.SpecialRuleGroup
'''
''' -----------------------------------------------------------------------------
''' <summary>
'''
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	5/4/2007	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public Class SpecialRuleGroup
    Inherits RuleGroup
    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    'Private Shared _multilineValue As Double

    'Public Shared Property MultiLineCoPayValue() As Double
    '    Get
    '        Return _multilineValue
    '    End Get
    '    Set(ByVal value As Double)
    '        _multilineValue = Value
    '    End Set
    'End Property
#Region "Methods"
    ' -----------------------------------------------------------------------------
    ' <summary>
    '
    ' </summary>
    ' <param name="dsbursement"></param>
    ' <param name="accumManager"></param>
    ' <param name="dateOfService"></param>
    ' <param name="dlgate"></param>
    ' <param name="alrtDelagate"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	5/4/2007	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Overrides Function Eval(ByVal disbursement As Disbursement, ByVal memberAccumulatorManager As MemberAccumulatorManager, ByVal dateOfService As Date?, ByVal actionDelegate As IAction.ExecuteAction, ByVal alertDelegate As Alert.AddAlert) As Disbursement

        Dim DisbursementClone As Disbursement = CType(disbursement.Clone, Disbursement)
        For Each Rule As Rule In List
            If TypeOf (Rule) Is AccidentRule Then
                DisbursementClone = DirectCast(Rule, AccidentRule).Eval(disbursement, DisbursementClone, memberAccumulatorManager, dateOfService, actionDelegate, alertDelegate)
            End If
        Next

        Return DisbursementClone
    End Function
#End Region

End Class
Public Enum RulesSetStatus
    Published = 1
    NotPublished = 0
End Enum

