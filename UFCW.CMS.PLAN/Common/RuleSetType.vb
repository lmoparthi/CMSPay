''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.RuleSetType
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' Used for storing rule set types
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	10/5/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public NotInheritable Class RuleSetType

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _RuleSetName As String
    Private _RuleSetID As Integer
    Public Sub New(ByVal name As String, ByVal id As Integer)
        _RuleSetName = name
        _RuleSetID = id
    End Sub
    Public Property Name() As String
        Get
            Return _RuleSetName
        End Get
        Set(ByVal value As String)
            _RuleSetName = Value
        End Set
    End Property

    Public Property Id() As Integer
        Get
            Return _RuleSetID
        End Get
        Set(ByVal value As Integer)
            _RuleSetID = Value
        End Set
    End Property
End Class