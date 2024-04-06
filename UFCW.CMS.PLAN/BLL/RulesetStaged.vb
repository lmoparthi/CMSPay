Option Explicit On
Option Strict On
''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.RulesetStaged
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class represents a Staged Ruleset
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	1/25/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Class RuleSetStaged
    Inherits RuleSet

#Region "Enums, Properties and Local Variables"

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
    Private _Status As RulesSetStatus
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets and Sets the Status of the Ruleset
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property Status() As RulesSetStatus
        Get
            Return _Status
        End Get
        Set(ByVal value As RulesSetStatus)
            _Status = Value
        End Set
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
        MyBase.New()
        _Status = RulesSetStatus.Published
    End Sub
#End Region

#Region "Functions"
    'Public Overrides Function Clone() As Ruleset
    '    Dim tmp As New RulesetStaged
    '    For Each rl As Rule In Me
    '        tmp.Add(rl)
    '    Next

    '    tmp.RulesetId = Me.RulesetId
    '    tmp.RulesetName = Me.RulesetName
    '    tmp.RulesetType = Me.RulesetType
    '    Return tmp
    'End Function
#End Region
End Class
