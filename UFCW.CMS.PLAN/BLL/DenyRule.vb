Option Strict On

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.DenyRule
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
Public Class DenyRule
    Inherits FundNonPaymentRule
    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

#Region "Constructors"
    Public Sub New(ByVal ruleActions As Actions, ByVal ruleConditions As Conditions)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Constructor
        ' </summary>
        ' <param name="ruleActions"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/3/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        MyBase.New(ruleActions, ruleConditions)
    End Sub

    Public Sub New(ByVal ruleConditions As Conditions)
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
        MyBase.New(ruleConditions)
    End Sub
#End Region

#Region "Methods"

    Public Overloads Overrides Function Eval(ByVal disbursement As Disbursement, ByVal memberAccumulatorManager As MemberAccumulatorManager, ByVal dateOfService As Date?, ByVal actionDelegate As IAction.ExecuteAction, ByVal alertDelegate As Alert.AddAlert) As Disbursement
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
        ' 	[paulw]	10/3/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim Dsb As Disbursement
        Dim ActionArgs As ActionArgs
        Dim Action As Action
        Dim Alert As New Alert

        Try

            Dsb = New Disbursement

            Dsb.FundPayment = Disbursement.FundPayment
            Dsb.MemberPayment = Disbursement.MemberPayment
            Dsb.Units = Disbursement.Units

            Alert = New Alert
            Alert.AlertMessage = "PROCEDURE NOT COVERED"
            Alert.Category = CategoryTypes.Denied
            Alert.SetSeverity(20)
            AlertDelegate.Invoke(Alert)

            Action = New FundDenialAction
            Action.Name = ""
            Action.ActionValueType = ActionValueTypes.Other
            Action.ApplyDate = DateOfService
            Action.ActionValue = 0

            ActionArgs = New ActionArgs
            ActionArgs.Action = Action
            ActionDelegate.Invoke(ActionArgs)
            Dsb.FundPayment = 0

            Return Dsb

        Catch ex As Exception
            Throw
        End Try

    End Function
#End Region
End Class