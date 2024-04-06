''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.PlanException
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class represents and exception that is thrown when there is a problem
'''  with the plan
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	1/29/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<SerializableAttribute()> _
Public Class PlanException
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