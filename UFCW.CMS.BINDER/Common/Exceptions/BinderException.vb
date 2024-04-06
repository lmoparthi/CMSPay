''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Binder
''' Class	 : CMS.Binder.BinderException
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' A generic Binder Exception
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/7/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable>
Public Class BinderException
    Inherits BusinessLayerException

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

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