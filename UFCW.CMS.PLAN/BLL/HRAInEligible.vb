Option Strict On

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
Public Class HRAInEligibleRule
    Inherits CoResponsibilityRule

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

#Region "Constructors"
    Public Sub New(ByVal RuleActions As Actions, ByVal RuleConditions As Conditions)
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
        MyBase.New(RuleActions, RuleConditions)
    End Sub

    Public Sub New(ByVal RuleConditions As Conditions)
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
        MyBase.New(RuleConditions)
    End Sub
#End Region

#Region "Methods"
    Public Overrides Function Eval(ByVal disbursement As Disbursement, ByVal memberAccumulatorManager As MemberAccumulatorManager, ByVal dateOfService As Date?, ByVal actionDelegate As IAction.ExecuteAction, ByVal alertDelegate As Alert.AddAlert) As Disbursement
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

        Dim TmpAmt As Decimal
        Dim FundReductionAction As FundReductionAction
        Dim ActionArgs As ActionArgs

        Try

            TmpAmt = Disbursement.FundPayment

            disbursement.FundPayment *= Math.Round((_Conditions(0).Operand / 100), 2)
            'here just for purposes of showing that both are 'looked' at
            disbursement.MemberPayment = TmpAmt - disbursement.FundPayment

            FundReductionAction = New FundReductionAction
            ActionArgs = New ActionArgs
            FundReductionAction.ActionValueType = ActionValueTypes.ActualValue
            FundReductionAction.Name = (100 - _Conditions(0).Operand).ToString & "%"
            FundReductionAction.ActionValue = TmpAmt

            _Actions.Add(FundReductionAction)
            ActionArgs.Action = FundReductionAction

            ActionDelegate.Invoke(ActionArgs)

            Return Disbursement

        Catch ex As Exception
            Throw
        End Try
    End Function
#End Region
End Class