''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.ProceduralAllowanceRule
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class rerpresents a procedural allowance rule
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	8/17/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public Class ProceduralAllowanceRule
    Inherits CoResponsibilityRule

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

#Region "Constructors"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Default Constructor
    ' </summary>
    ' <param name="ruleActions"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	8/17/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Sub New(ByVal ruleActions As Actions, ByVal ruleConditions As Conditions)
        MyBase.New(ruleActions, ruleConditions)
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Constructor
    ' </summary>
    ' <param name="ruleConditions"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	8/17/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Sub New(ByVal ruleConditions As Conditions)
        MyBase.New(ruleConditions)
    End Sub
#End Region

#Region "Methods"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' The eval of this rule
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
    ' 	[paulw]	4/14/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Overrides Function Eval(ByVal disbursement As Disbursement, ByVal memberAccumulatorManager As MemberAccumulatorManager, ByVal dateOfService As Date?, ByVal actionDelegate As IAction.ExecuteAction, ByVal alertDelegate As Alert.AddAlert) As Disbursement
        Dim FundAllowanceAction As New FundAllowanceAction
        Dim ActiontArgs As New ActionArgs

        FundAllowanceAction.ActionValueType = ActionValueTypes.ActualValue
        FundAllowanceAction.Name = "$" & Me._Conditions(0).Operand.ToString & " * " & Convert.ToInt32(If(disbursement.Units = 0, 1, disbursement.Units))
        FundAllowanceAction.ActionValue = disbursement.FundPayment

        _Actions.Add(FundAllowanceAction)

        'set the fund payment to the operand because it is the allowance
        If _Conditions(0).Operand * Convert.ToInt32(If(disbursement.Units = 0, 1, disbursement.Units)) < disbursement.FundPayment Then
            disbursement.FundPayment = _Conditions(0).Operand * Convert.ToInt32(If(disbursement.Units = 0, 1, disbursement.Units))

            If disbursement.FundPayment < 0 Then
                disbursement.FundPayment = 0
            End If
            _Actions.Add(FundAllowanceAction)

            ActiontArgs.Action = FundAllowanceAction
            actionDelegate.Invoke(ActiontArgs)
        End If
        Return disbursement
    End Function
#End Region
End Class