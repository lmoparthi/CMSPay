Imports System.Configuration
''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.ProcessorEngine
''' Class	 : CMS.ProcessorEngine.AccumulatorRule
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class represnts a base class for rules that use accumulators
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/12/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public MustInherit Class AccumulatorRule
    Inherits Rule

#Region "Private Variables"
    Private _AccumulatorManager As MemberAccumulatorManager
#End Region

#Region "Properties"
    Public Property AccumulatorManager() As MemberAccumulatorManager
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' gets/sets MemberAccumulatorManager
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/21/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _AccumulatorManager
        End Get
        Set(ByVal value As MemberAccumulatorManager)
            _AccumulatorManager = Value
        End Set
    End Property
#End Region

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
        ' 	[paulw]	4/19/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        MyBase.New(ruleConditions)
    End Sub
#End Region

#Region "Methods"
    Public Overrides Function Eval(ByVal disbursement As Disbursement, ByVal memberAccumulatorManager As MemberAccumulatorManager, ByVal dateOfService As Date?, ByVal actionDelegate As IAction.ExecuteAction, ByVal alertDelegate As Alert.AddAlert) As Disbursement
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

        'Throw New Exception("not implemented")
    End Function

    Private Shared _Retrieved As Boolean = False
    Private Shared _Log As Boolean

    Public Shared Function LogAllAccumulatorActions() As Boolean
        Try

            If Not _Retrieved Then
                _Log = CBool(CType(ConfigurationManager.GetSection("AlertManagerConfig"), IDictionary)("ShowAllAccumulatorAlerts"))
                _Retrieved = True
            End If

            Return _Log

        Catch ex As Exception

            Return False

        End Try

    End Function

#End Region
End Class