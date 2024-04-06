Option Explicit On
Option Strict On
''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.RulesetActive
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class represents and handles an Active Ruleset
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	1/25/2006	Created
''' </history>
''' -----------------------------------------------------------------------------

<Serializable()> _
Public Class RuleSetActive
    Inherits RuleSet

#Region "Properties and Local Variables"
    'Private Shared _TraceSwitch As New BooleanSwitch("TraceGeneral", "Trace Switch in App.Config")

    Private _PublishDate As Date
    Private _PublishBatchNum As Integer
    Private _Status As RuleSetStatus

    Public Property PublishDate() As Date
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets and Sets the Publish Date for this Ruleset
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _PublishDate
        End Get
        Set(ByVal value As Date)
            _publishDate = Value
        End Set
    End Property

    Public Property PublishBatchNumber() As Integer
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets and Sets the Publish Batch Number for this Ruleset
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _PublishBatchNum
        End Get
        Set(ByVal value As Integer)
            _PublishBatchNum = value
        End Set
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
        MyBase.New()
    End Sub
#End Region

End Class
Public Enum RuleSetStatus
    [New]
    Changed
    UnChanged
End Enum
