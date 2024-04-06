''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.ProcessorEngine
''' Class	 : CMS.ProcessorEngine.CoInsuranceRule
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' Represents a CoInsurance (a.k.a 'Percent Fund Pays') rule
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/12/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public Class PreventativeRule
    Inherits CoResponsibilityRule

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

#Region "Constructors"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Constructor
    ' </summary>
    ' <param name="ruleActions"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	4/12/2006	Created
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
    ' 	[paulw]	4/19/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Sub New(ByVal ruleConditions As Conditions)
        MyBase.New(ruleConditions)
    End Sub
#End Region

#Region "Methods"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' CoInsurance Implementation of Eval
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
    ' 	[paulw]	4/18/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Overrides Function Eval(ByVal disbursement As Disbursement, ByVal memberAccumulatorManager As MemberAccumulatorManager, ByVal dateOfService As Date?, ByVal actionDelegate As IAction.ExecuteAction, ByVal alertDelegate As Alert.AddAlert) As Disbursement
        Dim TmpAmt As Decimal = disbursement.FundPayment
        Dim FundReductionAction As New FundReductionAction
        Dim ActionArgs As New ActionArgs

        disbursement.FundPayment *= Math.Round((Me._Conditions(0).Operand / 100D), 2)
        'here just for purposes of showing that both are 'looked' at
        disbursement.MemberPayment = TmpAmt - disbursement.FundPayment

        FundReductionAction.ActionValueType = ActionValueTypes.ActualValue
        FundReductionAction.Name = (100 - Me._Conditions(0).Operand).ToString & "%"
        FundReductionAction.ActionValue = TmpAmt
        _Actions.Add(FundReductionAction)
        ActionArgs.Action = FundReductionAction
        actionDelegate.Invoke(ActionArgs)

        Return disbursement
    End Function
#End Region
End Class