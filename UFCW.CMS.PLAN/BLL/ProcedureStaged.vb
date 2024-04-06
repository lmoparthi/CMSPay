Option Explicit On
Option Strict On
''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.ProcedureStaged
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class represents and handles a Staged Procedure
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	1/25/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Class ProcedureStaged
    Inherits Procedure

#Region "Enums, Properties and Local Variables"
    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _Status As StatusCodes
    Private _NewRuleSets As Rulesets

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets and Sets the status
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/18/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property Status() As StatusCodes
        Get
            Return _Status
        End Get
        Set(ByVal value As StatusCodes)
            _Status = Value
        End Set
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the NewRuleSet
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property NewRuleSets() As Rulesets
        Get
            If _NewRuleSets Is Nothing Then
                _NewRuleSets = New Rulesets
            End If
            Return _NewRuleSets
        End Get
        Set(ByVal value As Rulesets)
            _NewRuleSets = Value
        End Set
    End Property

#End Region

#Region "Constructors"

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Default Constructor
    ' </summary>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Sub New()
        MyBase.New()
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Constructor with procedure id
    ' </summary>
    ' <param name="procedureId"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Sub New(ByVal procedureID As Int32)
        MyBase.New(procedureID)
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Constructor that sets all default variables and the ruleset
    ' </summary>
    ' <param name="procedureID"></param>
    ' <param name="billType"></param>
    ' <param name="diagnosis"></param>
    ' <param name="modifier"></param>
    ' <param name="placeOfService"></param>
    ' <param name="planType"></param>
    ' <param name="procedureCode"></param>
    ' <param name="providerID"></param>
    ' <param name="ruleSet"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Sub New(ByVal procedureID As Integer?, ByVal billType As String, ByVal diagnosis As String, ByVal modifier As String, ByVal gender As String, ByVal monthsMin As Integer?, ByVal monthsMax As Integer?, ByVal placeOfService As String, ByVal planType As String, ByVal procedureCode As String, ByVal provider As String, ByVal ruleSets As Rulesets)
        MyBase.New(procedureID, billType, diagnosis, modifier, gender, monthsMin, monthsMax, placeOfService, planType, procedureCode, provider)
        _RuleSets = ruleSets
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Constructor that sets all default variables and the ruleset, plus all staged related variables
    ' </summary>
    ' <param name="procedureID"></param>
    ' <param name="billType"></param>
    ' <param name="diagnosis"></param>
    ' <param name="modifier"></param>
    ' <param name="placeOfService"></param>
    ' <param name="planType"></param>
    ' <param name="procedureCode"></param>
    ' <param name="providerID"></param>
    ' <param name="ruleSet"></param>
    ' <param name="newRuleSet"></param>
    ' <param name="status"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Sub New(ByVal procedureID As Integer?, ByVal billType As String, ByVal diagnosis As String, ByVal modifier As String, ByVal gender As String, ByVal ageMin As Integer?, ByVal ageMax As Integer?, ByVal placeOfService As String, ByVal planType As String, ByVal procedureCode As String, ByVal provider As String, ByVal ruleSets As Rulesets, ByVal newRuleSets As Rulesets, ByVal status As Integer)
        MyBase.New(procedureID, billType, diagnosis, modifier, gender, ageMin, ageMax, placeOfService, planType, procedureCode, provider)
        _RuleSets = ruleSets
        _Status = CType(status, StatusCodes)
        _NewRuleSets = newRuleSets
    End Sub
#End Region

#Region "Functions"
    Public Overrides Function ToString() As String
        Return MyBase.ToString()
    End Function
#End Region

End Class
Public Enum StatusCodes
    Unchanged = 0
    Changed = 1
    [New] = 2
End Enum

