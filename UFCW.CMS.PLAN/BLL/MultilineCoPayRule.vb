Option Strict On

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.ProcessorEngine
''' Class	 : CMS.ProcessorEngine.MultilineCoPayRule
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class represents a multi-line CoPay rule
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	9/26/2006	Created Per ACR MED-0029
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public Class MultilineCoPayRule
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
        ' 	[paulw]	9/26/2006	Created
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
        ' 	[paulw]	9/26/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        MyBase.New(RuleConditions)
    End Sub
#End Region

#Region "Methods"

    Public Overrides Function Eval(ByVal disbursement As Disbursement, ByVal memberAccumulatorManager As MemberAccumulatorManager, ByVal dateOfService As Date?, ByVal actionDelegate As IAction.ExecuteAction, ByVal alertDelegate As Alert.AddAlert) As Disbursement
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
        ' 	[paulw]	9/26/2006	Created Per ACR MED-0029, added MultiLineCoPay type support
        '     [paulw] 10/6/2006   Per ACR MED-0090 included Units in calculation
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim FundReductionAction As FundReductionAction
        Dim OriginalAmt As Decimal
        Dim Alert As Alert
        Dim ActionArgs As New ActionArgs

        Try
            OriginalAmt = Disbursement.FundPayment

            FundReductionAction = New FundReductionAction
            FundReductionAction.ActionValue = Disbursement.FundPayment
            FundReductionAction.ActionValueType = ActionValueTypes.ActualValue

            Alert = New Alert

            If Disbursement.Units = 0 Then Disbursement.Units = 1

            Alert.Category = CategoryTypes.ReasonCode
            Alert.SetSeverity(1)

            If Disbursement.CheckMultiLineValue Then 'if this is not the 1st line with a co-pay
                If Disbursement.MultiLineFundPayment > 0 Then    'if there is an amount left to use for co-pays
                    '//Commented out because Jill informed me that we dont need to do this
                    'dsbursement.FundPayment *= dsbursement.Units
                    Disbursement.FundPayment -= Disbursement.MultiLineFundPayment

                    If Disbursement.FundPayment >= 0 Then
                        FundReductionAction.Name = "$" & Format((_Conditions(0).Operand * disbursement.Units), "0.00;-0.00") & " DISTRIBUTED CoPay IS: $" & disbursement.MultiLineFundPayment
                        Alert.AlertMessage = "Applied $" & Format(disbursement.MultiLineFundPayment, "0.00;-0.00") & " CoPay"
                        Disbursement.MultiLineFundPayment = 0
                    Else
                        FundReductionAction.Name = "$" & Format(OriginalAmt, "0.00;-0.00") & " DISTRIBUTED CoPay IS: $" & disbursement.MultiLineFundPayment
                        Alert.AlertMessage = "Applied $" & Format(OriginalAmt, "0.00;-0.00") & " CoPay"
                        Disbursement.FundPayment = 0
                        Disbursement.MultiLineFundPayment -= OriginalAmt * Disbursement.Units
                    End If

                    _Actions.Add(FundReductionAction)
                    _Actions.Add(FundReductionAction)
                    ActionArgs.Action = FundReductionAction
                    ActionDelegate.Invoke(ActionArgs)
                End If
            Else    'if this is the first time through
                disbursement.MultiLineFundPayment = _Conditions(0).Operand '* dsbursement.Units)
                '//Commented out because Jill informed me that we dont need to do this
                'dsbursement.FundPayment *= dsbursement.Units
                Disbursement.FundPayment -= Disbursement.MultiLineFundPayment '(Me._conditions(0).Operand * dsbursement.Units)
                If Disbursement.FundPayment >= 0 Then 'if all of the CoPay from the previous co-pay line was taken out, set the multiline to 0
                    Disbursement.MultiLineFundPayment = 0
                Else    'set the amount of be used for next time through
                    Disbursement.MultiLineFundPayment -= OriginalAmt * Disbursement.Units 'Math.Abs(dsbursement.FundPayment)  '(Me._conditions(0).Operand * dsbursement.Units)
                    Disbursement.FundPayment = 0
                End If
                '
                FundReductionAction.Name = "$" & (Math.Round(_Conditions(0).Operand * disbursement.Units, 2)) - disbursement.MultiLineFundPayment
                Alert.AlertMessage = "Applied $" & Format((_Conditions(0).Operand * disbursement.Units) - disbursement.MultiLineFundPayment, "0.00;-0.00") & " CoPay"
                If Disbursement.Units > 1 Then
                    'alrt.AlertMessage += " $(" & Math.Round(Me._conditions(0).Operand, 2).ToString & " * " & dsbursement.Units & ")"  ' - " & dsbursement.MultiLineFundPayment & " (Left over from previous line)"
                End If

                _Actions.Add(FundReductionAction)
                _Actions.Add(FundReductionAction)
                ActionArgs.Action = FundReductionAction
                actionDelegate.Invoke(ActionArgs)
            End If

            Disbursement.CheckMultiLineValue = True

            If Alert IsNot Nothing Then
                If Alert.AlertMessage IsNot Nothing Then
                    alertDelegate.Invoke(Alert)
                End If
            End If

            Return Disbursement

        Catch ex As Exception
            Throw
        End Try

    End Function
#End Region
End Class