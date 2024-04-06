Option Strict On

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.FundDecreasingRuleGroup
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class represent a group of Rules that decrease the funds responsibilty.
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/18/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public Class FundDecreasingRuleGroup
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
        Dim ActionArgs As ActionArgs

        Try

            Dsb = CType(Disbursement.Clone, Disbursement)
            ActionArgs = New ActionArgs

            For Each Rule As Rule In Me.List
                Dsb = Rule.Eval(disbursement, memberAccumulatorManager, dateOfService, actionDelegate, alertDelegate)
            Next

            Return Dsb

        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Function GetSmallestHeadroom(ByVal accumManager As MemberAccumulatorManager, ByVal dateOfService As Date, ByRef accumulatorName As String) As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="accumManager"></param>
        ' <param name="dateOfService"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/12/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim HeadRoom As Decimal
        Dim ReturnHeadroom As Decimal
        Dim AccumName As String

        Try

            ReturnHeadroom = Decimal.MaxValue

            For Each Rule As Rule In Me.List
                HeadRoom = Rule.GetHeadroom(accumManager, dateOfService, AccumName)
                If HeadRoom < ReturnHeadroom Then
                    ReturnHeadroom = HeadRoom
                    accumulatorName = AccumName
                End If
            Next
            Return ReturnHeadroom

        Catch ex As Exception
            Throw
        End Try
    End Function
#End Region
End Class