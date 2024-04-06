Option Strict On

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.ProcessorEngine
''' Class	 : CMS.ProcessorEngine.OutOfPocketRule
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class represents an Out of Pocket rule
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/12/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public Class OutOfPocketRule
    Inherits AccumulatorRule

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
        MyBase.New(RuleConditions)
    End Sub
#End Region

#Region "Methods"
    Public Overrides Function Eval(ByVal disbursement As Disbursement, ByVal memberAccumulatorManager As MemberAccumulatorManager, ByVal dateOfService As Date?, ByVal actionDelegate As IAction.ExecuteAction, ByVal alertDelegate As Alert.AddAlert) As Disbursement
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Out Of Pocket Implementation of Eval
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
        ' 	[paulw]	    4/18/2006	Created
        '     [paulw]     9/19/2006   Added in logic for 'head room check' in regards to ACR MED-0018
        '     [paulw]     11/2/2006   I fixed the way that Alerts are thrown.  The implementation looks something
        '                             like this:
        '                             1.  The conditions are sorted before they get here
        '                             2.  Processing happens normally
        '                             3.  If an alert is thrown, we find all conditions within this rule
        '                                 that have the same operand and throw alerts for all of the same operands/accumulators
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim Condition As Condition
        Dim Dsb As Disbursement
        Dim CurrentAmt As Decimal
        Dim HeadRoomAmt As Decimal
        Dim ActionArgs As ActionArgs
        Dim AlterAccumulatorAction As AlterAccumulatorAction
        Dim Alert As Alert
        Dim PARConditions As Conditions
        Dim NonPARConditions As Conditions

        Try

            PARConditions = New Conditions
            NonPARConditions = New Conditions

            Dsb = CType(Disbursement.Clone, Disbursement)

            Rule.DetermineParAndNonParConditions(_Conditions, PARConditions, NonPARConditions)

            For I As Integer = 0 To _Conditions.Count - 1
                Condition = DirectCast(_Conditions(I), Condition)
                CurrentAmt = memberAccumulatorManager.GetProposedValue(CInt(AccumulatorController.GetAccumulatorID(Condition.AccumulatorName)), Condition.DurationType, Condition.Duration, dateOfService, Condition.Direction)
                HeadRoomAmt = Condition.Operand - CurrentAmt
                'if this condition is not supposed to check for head room, then ignore the headroom.  the
                ' logic will still make sure that accumulators are posted too, but we don't want to
                ' adjust the disbusrement object if there is no headroom check.  this is the solution
                ' for ACR MED-0011
                If Condition.UseInHeadroomCheck Then
                    If HeadRoomAmt >= Dsb.MemberPayment Then 'is there enough room to add this amount
                        'there is enough room so dont change dsb
                    Else    'there is not enough head room
                        If HeadRoomAmt > 0 AndAlso Condition.Operand > 0 Then
                            'find all conditions with that same operand so they can
                            ' be in the alert
                            For X As Integer = 0 To _Conditions.Count - 1
                                If _Conditions(X).Operand = Condition.Operand Then
                                    Alert = New Alert
                                    Alert.AlertMessage = _Conditions(X).AccumulatorName + " Maximum Met"
                                    Alert.SetSeverity(1)
                                    Alert.Category = CategoryTypes.Accumulator
                                    alertDelegate.Invoke(Alert)
                                End If
                            Next
                        End If
                        If HeadRoomAmt >= 0 Then
                            Dsb.FundPayment += (Dsb.MemberPayment - HeadRoomAmt)
                            Dsb.MemberPayment -= (Dsb.MemberPayment - HeadRoomAmt)
                        Else
                            Dsb.FundPayment += Dsb.MemberPayment
                            Dsb.MemberPayment -= Dsb.MemberPayment
                        End If
                    End If
                End If

                ActionArgs = New ActionArgs

                AlterAccumulatorAction = New AlterAccumulatorAction(Me.AccumulatorManager)
                AlterAccumulatorAction.Name = Condition.AccumulatorName
                AlterAccumulatorAction.ApplyDate = dateOfService

                If PARConditions.IsAccumulatorInConditions(AlterAccumulatorAction.Name) Then
                    AlterAccumulatorAction.ActionValueType = ActionValueTypes.ParMemberPaymentValue
                Else
                    AlterAccumulatorAction.ActionValueType = ActionValueTypes.NonParMemberPaymentValue
                End If

                ActionArgs.Action = AlterAccumulatorAction
                actionDelegate.Invoke(ActionArgs)
            Next

            If _Conditions.IsConfiguredForNonPar Then
                Dsb.NonParMemberPaymentValue = Dsb.MemberPayment
                Dsb.ParMemberPaymentValue = If(Rule.GetSmallestHeadroom(PARConditions, memberAccumulatorManager, DateOfService) < Dsb.MemberPayment, Rule.GetSmallestHeadroom(PARConditions, memberAccumulatorManager, DateOfService), Dsb.MemberPayment)
            Else
                Dsb.NonParMemberPaymentValue = Dsb.MemberPayment
                Dsb.ParMemberPaymentValue = Dsb.MemberPayment
            End If
            Return Dsb

        Catch ex As Exception
            Throw
        End Try
    End Function
#End Region
End Class