Option Explicit On 
Option Strict On

Namespace PlugInSharedInterfaces
    ' -----------------------------------------------------------------------------
    ' Project	 : PlugIn
    ' Interface	 : IMessage
    ' 
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' This is the Plugin interface to talk to the main form the plugin was launched from
    ' </summary>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	3/1/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Interface IMessage
        Sub StatusMessage(ByVal msg As String)
    End Interface
End Namespace