Option Strict On

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.ProcessorEngine
''' Class	 : CMS.ProcessorEngine.CoPayRule
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class represents a CoPay rule
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/12/2006	Created
''' </history>
''' -----------------------------------------------------------------------------

<Serializable()> _
Public Class CoPayRule
    Inherits CoResponsibilityRule

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
        ' 	[paulw]	4/12/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        MyBase.New(RuleActions, RuleConditions)
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
        ' 	[paulw]	4/19/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        MyBase.New(ruleConditions)
    End Sub
#End Region

#Region "Methods"
    Public Overrides Function Eval(ByVal disbursement As Disbursement, ByVal memberAccumulatorManager As MemberAccumulatorManager, ByVal dateOfService As Date?, ByVal actiondelegate As IAction.ExecuteAction, ByVal alertDelegate As Alert.AddAlert) As Disbursement
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' The logic for this rule
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
        '     [paulw] 10/6/2006   Per ACR MED-0090 included Units in calculation
        '     [paulw] 1/16/2007   Added logic to Satisfy ACR MED-195
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim Alert As Alert
        Dim Action As FundReductionAction
        Dim ActionArgs As ActionArgs

        Try

            If Disbursement.Units = 0 Then Disbursement.Units = 1

            Alert = New Alert
            Alert.AlertMessage = "Applied $" & (_Conditions(0).Operand * Disbursement.Units) & " CoPay"

            If Disbursement.Units > 1 Then
                Alert.AlertMessage += " (" & _Conditions(0).Operand.ToString & " * " & Disbursement.Units & ")"
            End If

            Alert.Category = CategoryTypes.ReasonCode
            Alert.SetSeverity(1)
            alertDelegate.Invoke(Alert)

            Alert = New Alert
            Alert.AlertMessage = "a 110 Plan. Check # of visits paid." 'CoPay Present on a 110 Plan"
            Alert.Category = CategoryTypes.Other
            Alert.SetSeverity(20)
            AlertDelegate.Invoke(Alert)

            Action = New FundReductionAction
            Action.ActionValueType = ActionValueTypes.ActualValue
            Action.Name = "$" & (Me._Conditions(0).Operand * Disbursement.Units) & " CoPay is: " & _Conditions(0).Operand.ToString & " * " & Disbursement.Units
            Action.ActionValue = Disbursement.FundPayment
            _Actions.Add(Action)

            Disbursement.FundPayment -= _Conditions(0).Operand * Disbursement.Units

            If Disbursement.FundPayment < 0 Then
                Disbursement.FundPayment = 0
            End If

            ActionArgs = New ActionArgs

            _Actions.Add(Action)
            ActionArgs.Action = Action
            Actiondelegate.Invoke(ActionArgs)

            Return Disbursement

        Catch ex As Exception
            Throw
        End Try
    End Function
#End Region
End Class