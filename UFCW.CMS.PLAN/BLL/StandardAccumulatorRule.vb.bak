Option Strict On

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.ProcessorEngine
''' Class	 : CMS.ProcessorEngine.StandardAccumulatorRule
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class represents a Standard Accumulator rule
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/12/2006	Created
''' </history>
''' -----------------------------------------------------------------------------

<Serializable()> _
Public Class StandardAccumulatorRule
    Inherits AccumulatorRule

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

#Region "Constructors"
    Public Sub New(ByVal ruleActions As Actions, ByVal ruleConditions As Conditions)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Default Constructor
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
        ' 	[paulw]	4/21/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        MyBase.New(RuleConditions)
    End Sub
#End Region

#Region "Methods"
    Public Overrides Function Eval(ByVal disbursement As Disbursement, ByVal memberAccumulatorManager As MemberAccumulatorManager, ByVal dateOfService As Date?, ByVal actionDelegate As IAction.ExecuteAction, ByVal alertDelegate As Alert.AddAlert) As Disbursement
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Standard Accumulator's implementation of Eval
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
        Dim CurrentVal As Decimal
        Dim HeadRoomAmt As Decimal
        Dim ActionArg As ActionArgs
        Dim Actions As Actions
        Dim AlterAccumulatorAction As AlterAccumulatorAction
        Dim IsCurrency As Boolean
        Dim Pass As Boolean = True
        Dim Alert As Alert

        Try
            Dsb = CType(disbursement.Clone, Disbursement)
            Actions = New Actions

            For I As Integer = 0 To _Conditions.Count - 1

                Condition = DirectCast(_Conditions(I), Condition)
                IsCurrency = (AccumulatorController.GetAccumulatorValueType(CInt(AccumulatorController.GetAccumulatorID(Condition.AccumulatorName))) = MemberAccumulatorValueType.Currency)
                CurrentVal = memberAccumulatorManager.GetProposedValue(CInt(AccumulatorController.GetAccumulatorID(Condition.AccumulatorName)), Condition.DurationType, Condition.Duration, dateOfService, Condition.Direction)

                If Condition.Duration > 0 AndAlso Condition.Duration < 1000 AndAlso Condition.DurationType = DateType.Years Then
                    CurrentVal += memberAccumulatorManager.GetProposedValue(CInt(AccumulatorController.GetAccumulatorID(Condition.AccumulatorName)), DateType.Days, CInt(DateDiff(DateInterval.Day, CDate(dateOfService).AddDays(1), UFCWGeneral.NowDate)), CDate(dateOfService).AddDays(1), DateDirection.Forward)
                End If

                HeadRoomAmt = Condition.Operand - CurrentVal

                If IsCurrency Then
                    If HeadRoomAmt >= Dsb.FundPayment Then
                        'dont do anything with dsb because there is enough room, so it is fine
                    Else
                        'if this condition is not supposed to check for head room, then ignore the headroom.  the
                        ' logic will still make sure that accumulators are posted too, but we don't want to
                        ' adjust the disbursement object if there is no headroom check.  this is the solution
                        ' for ACR MED-0011
                        If Condition.UseInHeadroomCheck Then
                            If HeadRoomAmt > 0 AndAlso Condition.Operand > 0 Then
                                'find all conditions with that same operand so they can
                                ' be in the alert
                                For X As Integer = 0 To _Conditions.Count - 1
                                    If _Conditions(X).Operand = Condition.Operand Then
                                        Alert = New Alert
                                        Alert.AlertMessage = _Conditions(X).AccumulatorName + " Maximum Exceeded"
                                        Alert.SetSeverity(1)
                                        Alert.Category = CategoryTypes.Accumulator
                                        alertDelegate.Invoke(Alert)
                                    End If
                                Next
                            End If

                            If HeadRoomAmt >= 0 Then
                                Dsb.ParMemberPaymentValue += (Dsb.FundPayment - HeadRoomAmt)
                                Dsb.NonParMemberPaymentValue += (Dsb.FundPayment - HeadRoomAmt)
                                Dsb.FundPayment -= (Dsb.FundPayment - HeadRoomAmt)
                            Else
                                Dsb.ParMemberPaymentValue += Dsb.FundPayment
                                Dsb.NonParMemberPaymentValue += Dsb.FundPayment
                                Dsb.FundPayment -= Dsb.FundPayment
                            End If
                        End If
                    End If
                Else
                    If HeadRoomAmt >= Dsb.Units Then
                        'dont do anything with dsb because there is enough room, so it is fine
                    Else
                        'if this condition is not supposed to check for head room, then ignore the headroom.  the
                        ' logic will still make sure that accumulators are posted too, but we don't want to
                        ' adjust the disbusrement object if there is no headroom check.  this is the solution
                        ' for ACR MED-0011
                        If Condition.UseInHeadroomCheck Then
                            Alert = New Alert
                            Alert.AlertMessage = Condition.AccumulatorName + " Maximum Exceeded"
                            Alert.SetSeverity(25)
                            Alert.Category = CategoryTypes.RepriceNeeded

                            alertDelegate.Invoke(Alert)

                            If HeadRoomAmt > 0 Then
                                If Condition.RepriceIfExceeded Then
                                    Dsb.Units = HeadRoomAmt
                                    Throw New NeedsRepricingException("Not enough Unit Head Room in " & Condition.AccumulatorName)
                                Else
                                    Dsb.FundPayment -= ((Dsb.OriginalAmount / Dsb.Units) * (Dsb.Units - HeadRoomAmt)) '/ dsb.Units
                                    Dsb.Units = HeadRoomAmt
                                End If
                            Else
                                Dsb.Units = 0
                                Dsb.FundPayment = 0
                                If Condition.RepriceIfExceeded Then
                                    Throw New NeedsRepricingException("Not enough Unit Head Room in " & Condition.AccumulatorName)
                                End If
                            End If
                        End If
                    End If
                End If

                AlterAccumulatorAction = New AlterAccumulatorAction(Me.AccumulatorManager)
                AlterAccumulatorAction.Name = Condition.AccumulatorName
                AlterAccumulatorAction.ApplyDate = dateOfService

                If IsCurrency Then
                    AlterAccumulatorAction.ActionValueType = ActionValueTypes.FundPaymentValue
                Else
                    AlterAccumulatorAction.ActionValueType = ActionValueTypes.UnitsValue
                    AlterAccumulatorAction.ActionValue = Dsb.Units
                End If
                Actions.Add(AlterAccumulatorAction)

            Next

            For Each Action As IAction In Actions
                ActionArg = New ActionArgs
                ActionArg.Action = Action
                actionDelegate.Invoke(ActionArg)
            Next

            Return Dsb

        Catch ex As Exception
            Throw
        End Try

    End Function
#End Region

End Class