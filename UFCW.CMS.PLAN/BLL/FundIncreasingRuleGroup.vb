Option Strict On

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.FundIncreasingRuleGroup
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This groups eval method.  This method goes through each rule
'''  and calls it's eval method
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/18/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public Class FundIncreasingRuleGroup
    Inherits RuleGroup

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

#Region "Methods"

    Public Overrides Function Eval(ByVal disbursement As Disbursement, ByVal memberAccumulatorManager As MemberAccumulatorManager, ByVal dateOfService As Date?, ByVal actionDelegate As IAction.ExecuteAction, ByVal alertDelegate As Alert.AddAlert) As Disbursement
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' This groups eval method.  This method goes through each rule
        '  and calls it's eval method
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

        Dim Dsb As Disbursement

        Try

            Dsb = CType(disbursement.DeepCopy, Disbursement)
            For Each Rule As Rule In List
                Dsb = Rule.Eval(disbursement, memberAccumulatorManager, dateOfService, actionDelegate, alertDelegate)
            Next

            Return Dsb

        Catch ex As Exception
            Throw
        End Try
    End Function
#End Region
End Class