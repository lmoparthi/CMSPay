''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.ConvertToStagedException
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' this class represents an exception that is thrown when active
'''  data is converted to staged data/
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	1/29/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<SerializableAttribute()> _
Public Class ConvertToStagedException
    Inherits BusinessLayerException
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