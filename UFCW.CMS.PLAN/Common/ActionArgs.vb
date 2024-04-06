''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.ActionArgs
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class represents an ActionArgument used for events
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/22/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Class ActionArgs
    Inherits EventArgs

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
    Private _Action As IAction

    Public Property Action() As IAction
        Get
            Return _Action
        End Get
        Set(ByVal value As IAction)
            _Action = Value
        End Set
    End Property
End Class