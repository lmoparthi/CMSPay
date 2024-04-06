Option Strict On

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.ProviderWriteOffRule
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class reprsents a fund denail of payment
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	10/3/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public Class ProviderWriteOffRule
    Inherits FundNonPaymentRule

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
        ' 	[paulw]	3/8/2007	Created
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
        ' 	[paulw]	10/3/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        MyBase.New(RuleConditions)
    End Sub
#End Region

#Region "Methods"
    Public Overloads Overrides Function Eval(ByVal disbursement As Disbursement, ByVal memberAccumulatorManager As MemberAccumulatorManager, ByVal dateOfService As Date?, ByVal actiondelegate As IAction.ExecuteAction, ByVal alertDelegate As Alert.AddAlert) As Disbursement
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Evaluate the Disbursement in light of the disbursement passed in
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
        ' 	[paulw]	3/8/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim Dsb As Disbursement
        Dim ActionArgs As ActionArgs
        Dim Action As Action
        Dim Actions As Actions
        Dim Alert As Alert

        Try
            Dsb = New Disbursement
            Actions = New Actions

            Dsb.FundPayment = Disbursement.FundPayment
            Dsb.MemberPayment = Disbursement.MemberPayment
            Dsb.Units = Disbursement.Units

            Alert = New Alert
            Alert.AlertMessage = "PROVIDER WRITE OFF"
            Alert.Category = CategoryTypes.ProviderWriteOff
            Alert.SetSeverity(20)
            AlertDelegate.Invoke(Alert)

            Action = New ProviderWriteOffAction
            Action.Name = ""
            Action.ActionValueType = ActionValueTypes.Other
            Action.ApplyDate = dateOfService
            Action.ActionValue = 0
            ActionArgs = New ActionArgs
            ActionArgs.Action = Action
            Actiondelegate.Invoke(ActionArgs)
            Dsb.FundPayment = 0

            Return Dsb

        Catch ex As Exception
            Throw
        End Try

    End Function
#End Region
End Class