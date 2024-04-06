''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.ProcessorEngine
''' Class	 : CMS.ProcessorEngine.CoResponsibilityRule
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class represents a CoResponsiblityRule class used as a base class.
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/12/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public MustInherit Class CoResponsibilityRule
    Inherits Rule

#Region "Private Members"
    Private _Operand As Char
    Private _Value As Decimal
#End Region

#Region "Constructors"
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
    Public Sub New(ByVal ruleActions As Actions, ByVal ruleConditions As Conditions)
        MyBase.New(ruleActions, ruleConditions)
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
        MyBase.New(ruleConditions)
    End Sub
#End Region

#Region "Methods"
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
    ' 	[paulw]	4/12/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Overrides Function Eval(ByVal disbursement As Disbursement, ByVal memberAccumulatorManager As MemberAccumulatorManager, ByVal dateOfService As Date?, ByVal actionDelegate As IAction.ExecuteAction, ByVal alertDelegate As Alert.AddAlert) As Disbursement

    End Function
#End Region
End Class