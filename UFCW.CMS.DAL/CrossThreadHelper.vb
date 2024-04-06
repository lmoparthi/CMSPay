Imports System.ComponentModel

Public Module InvokeRequiredHandler
    ''' <summary>
    ''' Provides a thread-safe call to form elements/controls. [Templated Method]
    ''' </summary>
    ''' <param name="controlToInvoke">(As T) Name of the form element/control to invoke.</param>
    ''' <param name="actionToPerform">(As Action(Of T)) Action to perform on the form element.</param>
    ''' <remarks>Example Usage:
    ''' <c>Public SomeElement As SomeControl</c>
    ''' <c>Private SomeElement2 As SomeControl</c>
    ''' <c>SomeElement.HandleInvokeRequired(Sub(SomeElement As SomeControl) SomeElement.Text = "Text Here")</c>
    ''' <c>SomeElement2.HandleInvokeRequired(Sub(SomeElement2 As SomeControl) SomeElement2.Enabled = False)</c>
    ''' </remarks>
    <System.Runtime.CompilerServices.Extension()> _
    Public Sub HandleInvokeRequired(Of T As ISynchronizeInvoke)(ByVal controlToInvoke As T, ByVal actionToPerform As Action(Of T))

        'Check to see if the control's InvokeRequired property is true
        If controlToInvoke.InvokeRequired Then
            'Use Invoke() to invoke your action
            controlToInvoke.Invoke(actionToPerform, New Object() {controlToInvoke})
        Else
            'Perform the action
            actionToPerform(controlToInvoke)
        End If
    End Sub

End Module
