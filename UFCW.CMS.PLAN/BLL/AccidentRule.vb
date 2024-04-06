Option Strict On

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.AccidentRule
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class represents an Accident Rule
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	5/11/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public Class AccidentRule
    Inherits AccumulatorRule

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
    Private _Enabled As Boolean = False

    Public Property Enabled() As Boolean
        Get
            Return _Enabled
        End Get
        Set(ByVal value As Boolean)
            _Enabled = Value
        End Set
    End Property

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
        ' 	[paulw]	5/11/2006	Created
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
        ' 	[paulw]	5/11/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        MyBase.New(ruleConditions)
    End Sub
#End Region

#Region "Methods"

    Public Overloads Function Eval(ByVal originalDisbursement As Disbursement, ByVal processedDisbursement As Disbursement, ByVal memberAccumulatorManager As MemberAccumulatorManager, ByVal dateOfService As Date?, ByVal actionDelegate As IAction.ExecuteAction, ByVal alertDelegate As Alert.AddAlert) As Disbursement
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
        ' 	[paulw] 	5/11/2006	Created
        '     [paulw]     9/19/2006   Added in logic for 'head room check' in regards to ACR MED-0018
        '     [paulw]     11/2/2006   I fixed the way that Alerts are thrown.  The implementation looks something
        '                             like this:
        '                             1.  The conditions are sorted before they get here
        '                             2.  Processing happens normally
        '                             3.  If an alert is thrown, we find all conditions within this rule
        '                                 that have the same operand and throw alerts for all of the same operands/accumulators
        ' </history>
        ' -----------------------------------------------------------------------------

        If Not Enabled Then Return processedDisbursement

        Dim Condition As Condition
        Dim Disbursement As Disbursement
        Dim Value As Decimal
        Dim CurrentValue As Decimal
        Dim HeadRoom As Decimal
        Dim ActionArg As ActionArgs
        Dim Actions As Actions
        Dim AlterAccumulatorAction As AlterAccumulatorAction
        Dim Alert As Alert

        Try

            Disbursement = CType(processedDisbursement.Clone, Disbursement)
            Actions = New Actions

            For I As Integer = 0 To _Conditions.Count - 1
                Condition = DirectCast(_Conditions(I), Condition)

                CurrentValue = memberAccumulatorManager.GetProposedValue(CInt(AccumulatorController.GetAccumulatorID(Condition.AccumulatorName)), Condition.DurationType, Condition.Duration, dateOfService, Condition.Direction)
                HeadRoom = Condition.Operand - CurrentValue

                ' i think instead of the next line we actually need something like
                ' val = processedDis.PrepocessPym + MemPymt

                'basically we need to first eliminate ded then oop
                'unless there is not enough room in a FundDecreasing Accum
                ' or not enough in this accum (ACCxx).

                'basically we dont want
                ' to givem them back more than what they gave towards their
                ' ded and oop combined.

                'In addition, we whatever we apply towards any FundDec accum, we apply towards MM3.
                Value = processedDisbursement.PreSubTotalMemberPayment + processedDisbursement.MemberPayment
                If HeadRoom >= Value Then
                    processedDisbursement.FundPayment += processedDisbursement.PreSubTotalMemberPayment
                    processedDisbursement.FundPayment += processedDisbursement.MemberPayment
                    processedDisbursement.MemberPayment -= processedDisbursement.MemberPayment
                    processedDisbursement.NonParMemberPaymentValue -= processedDisbursement.NonParMemberPaymentValue
                    processedDisbursement.ParMemberPaymentValue -= processedDisbursement.ParMemberPaymentValue
                Else
                    If HeadRoom > 0 Then
                        Alert = New Alert
                        Alert.AlertMessage = "AAEB Maximum Met"
                        Alert.SetSeverity(1)
                        Alert.Category = CategoryTypes.Accumulator
                        alertDelegate.Invoke(Alert)
                    End If

                    If HeadRoom >= processedDisbursement.PreSubTotalMemberPayment Then
                        Value = processedDisbursement.PreSubTotalMemberPayment
                        HeadRoom -= processedDisbursement.PreSubTotalMemberPayment
                        processedDisbursement.FundPayment += processedDisbursement.PreSubTotalMemberPayment

                        If HeadRoom >= processedDisbursement.MemberPayment Then
                            'should never hit this logic
                            MsgBox("If you see this message please notify the IS department.")
                            'Stop
                            Value += processedDisbursement.MemberPayment
                            processedDisbursement.FundPayment += processedDisbursement.MemberPayment
                            processedDisbursement.MemberPayment -= processedDisbursement.MemberPayment
                            processedDisbursement.NonParMemberPaymentValue -= processedDisbursement.NonParMemberPaymentValue
                            processedDisbursement.ParMemberPaymentValue -= processedDisbursement.ParMemberPaymentValue
                        Else
                            Value += HeadRoom
                            processedDisbursement.FundPayment += HeadRoom
                            processedDisbursement.MemberPayment -= HeadRoom
                            processedDisbursement.NonParMemberPaymentValue -= HeadRoom
                            processedDisbursement.ParMemberPaymentValue -= HeadRoom
                        End If
                    Else
                        Value = HeadRoom
                        processedDisbursement.FundPayment += HeadRoom
                    End If

                End If

                AlterAccumulatorAction = New AlterAccumulatorAction(Me.AccumulatorManager)
                AlterAccumulatorAction.Name = Condition.AccumulatorName
                AlterAccumulatorAction.ApplyDate = dateOfService
                AlterAccumulatorAction.ActionValueType = ActionValueTypes.ActualValue
                AlterAccumulatorAction.ActionValue = Value
                Actions.Add(AlterAccumulatorAction)
            Next

            For Each Action As IAction In Actions
                ActionArg = New ActionArgs
                ActionArg.Action = Action
                actionDelegate.Invoke(ActionArg)
            Next

            Return CType(processedDisbursement.Clone, Disbursement)

        Catch ex As Exception
            Throw
        End Try

    End Function

#End Region
End Class