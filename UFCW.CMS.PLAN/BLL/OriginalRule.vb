Option Strict On

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.ProcessorEngine
''' Class	 : CMS.ProcessorEngine.OriginalRule
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class represents a rule that expects the the associated accumulator to be increased by the original reported amt units/amt to be Original
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/12/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public Class OriginalRule
    Inherits AccumulatorRule

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
        ' Original's implementation of Eval
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
        Dim SmallestAmt As Decimal
        Dim HeadRoomAmt As Decimal
        Dim ActionArgs As ActionArgs
        Dim AlterAccumulatorAction As AlterAccumulatorAction
        Dim Actions As Actions
        Dim Amt As Decimal = Decimal.MaxValue - 1
        Dim Alert As Alert

        Dim PARConditions As Conditions
        Dim NonPARConditions As Conditions

        Try

            Actions = New Actions
            PARConditions = New Conditions
            NonPARConditions = New Conditions

            Dsb = CType(Disbursement.Clone, Disbursement)

            Rule.DetermineParAndNonParConditions(_Conditions, PARConditions, NonPARConditions)

            For I As Integer = 0 To _Conditions.Count - 1
                AlterAccumulatorAction = New AlterAccumulatorAction(Me.AccumulatorManager)
                Condition = DirectCast(_Conditions(I), Condition)
                CurrentAmt = memberAccumulatorManager.GetProposedValue(CInt(AccumulatorController.GetAccumulatorID(Condition.AccumulatorName)), Condition.DurationType, Condition.Duration, dateOfService, Condition.Direction)

                HeadRoomAmt = Condition.Operand - CurrentAmt

                If HeadRoomAmt > 0 AndAlso Condition.Operand > 0 Then
                    'if this condition is not supposed to check for head room, then ignore the headroom.  the
                    ' logic will still make sure that accumulators are posted too, but we don't want to
                    ' adjust the disbusrement object if there is no headroom check.  this is the solution
                    ' for ACR MED-0011
                    If Condition.UseInHeadroomCheck Then
                        If HeadRoomAmt < Amt Then
                            If HeadRoomAmt >= 0 Then
                                Amt = HeadRoomAmt
                                If Amt < Dsb.FundPayment Then
                                    If Condition.Operand > 0 Then
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
                                End If
                            Else
                                Amt = 0
                            End If
                        End If
                    End If
                End If

                AlterAccumulatorAction.Name = Condition.AccumulatorName
                If PARConditions.IsAccumulatorInConditions(AlterAccumulatorAction.Name) Then
                    AlterAccumulatorAction.ActionValueType = ActionValueTypes.PreSubTotalParMemberPaymentValue
                Else
                    AlterAccumulatorAction.ActionValueType = ActionValueTypes.PreSubTotalNonParMemberPaymentValue
                End If

                AlterAccumulatorAction.ApplyDate = dateOfService
                Actions.Add(AlterAccumulatorAction)

                If HeadRoomAmt <= 0 AndAlso Condition.Operand > 0 Then
                    'find all conditions with that same operand so they can
                    ' be in the alert
                    'add actions for every condition
                    If HeadRoomAmt <> 0 Then

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

                    If Condition.UseInHeadroomCheck Then
                        Amt = 0
                        Exit For
                    End If
                End If
            Next

            If Amt = Decimal.MaxValue - 1 Then
                Amt = 0
            ElseIf Amt > Dsb.FundPayment Then
                Amt = Dsb.FundPayment
            End If

            For Each Action As IAction In Actions
                ActionArgs = New ActionArgs
                ActionArgs.Action = Action
                ActionDelegate.Invoke(ActionArgs)
            Next

            ' Originals not yet covered will be deducted from fund payment
            Dsb.FundPayment -= Amt
            Dsb.PreSubTotalMemberPayment += Amt
            If _Conditions.IsConfiguredForNonPar Then
                Dsb.PreSubTotalNonParMemberPaymentValue = Dsb.PreSubTotalMemberPayment
                SmallestAmt = Rule.GetSmallestHeadroom(_Conditions, memberAccumulatorManager, dateOfService)
                If SmallestAmt > Amt Then SmallestAmt = Amt
                Dsb.PreSubTotalParMemberPaymentValue = SmallestAmt
            Else
                Dsb.PreSubTotalNonParMemberPaymentValue = Dsb.PreSubTotalMemberPayment
                Dsb.PreSubTotalParMemberPaymentValue = Dsb.PreSubTotalMemberPayment
            End If

            Return Dsb

        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region
End Class