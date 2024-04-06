''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.ActiveDataException
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class represents and Exception that is thrown when there is a problem
'''  reading or writing data active plan data in the database
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	1/29/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<SerializableAttribute()> _
Public Class ActiveDataException
    Inherits DataLayerException

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