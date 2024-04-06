Option Explicit On 
Option Strict On

Namespace SharedInterfaces

    Public Interface IMessage
        Sub StatusMessage(ByVal msg As String)
    End Interface

End Namespace