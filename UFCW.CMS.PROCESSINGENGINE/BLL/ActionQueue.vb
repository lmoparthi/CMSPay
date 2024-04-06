''' -----------------------------------------------------------------------------
''' Project	 : ProcessingEngine
''' Class	 : ActionQueue
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class represents a Queue of actions
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/14/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Class ActionQueue
    Inherits Queue

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

#Region "Constructors"

    Public Sub New()
        MyBase.New()
    End Sub
#End Region

#Region "Methods"
    Public Sub AddAction(ByVal act As IAction)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Adds an Action to the Action Queue for processing
        ' </summary>
        ' <param name="act"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/19/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        If act Is Nothing Then
            Throw New ArgumentNullException("act")
        End If
        MyBase.Enqueue(act)
    End Sub

    Public Sub AddActions(ByVal acts As Actions)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Adds actions to processing queue
        ' </summary>
        ' <param name="acts"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/19/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        If acts Is Nothing Then
            Throw New ArgumentNullException("acts")
        End If
        If acts.Count = 0 Then
            Throw New ArgumentException("Must have more than 1 action in the Actions collection", "acts")
        End If
        For Each act As IAction In acts
            MyBase.Enqueue(act)
        Next
    End Sub

    '' -----------------------------------------------------------------------------
    '' <summary>
    '' Gets and removes an action from the queue
    '' </summary>
    '' <returns></returns>
    '' <remarks>
    '' </remarks>
    '' <history>
    '' 	[paulw]	4/20/2006	Created
    '' </history>
    '' -----------------------------------------------------------------------------
    'Public Overrides Function Dequeue() As IAction
    '    Return MyBase.Dequeue
    'End Function

    'Default Public Property Item(ByVal index As Integer) As Action
    '    Get
    '        Return CType(me.(index), Action)
    '    End Get
    '    Set(ByVal value As IProcedure)
    '        list(index) = Value
    '    End Set
    'End Property
#End Region
End Class