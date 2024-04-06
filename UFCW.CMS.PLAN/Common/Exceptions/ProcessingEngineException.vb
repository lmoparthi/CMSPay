''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.ProcessingEngine
''' Class	 : CMS.ProcessorEngine.ProcessingEngineException
'''
''' -----------------------------------------------------------------------------
''' <summary>
'''
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/12/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<SerializableAttribute()> _
Public Class ProcessingEngineException
    Inherits Exception

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal inner As Exception)
        MyBase.New(message, inner)
    End Sub

    Protected Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub
End Class