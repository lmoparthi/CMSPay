''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.ProcessingEngine
''' Class	 : ProcessorFactory
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' A Factory pattern class from creating processors
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/21/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public NotInheritable Class ProcessorFactory

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

#Region "Constructors"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' So no one can instantiate
    ' </summary>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	4/26/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub New()
    End Sub
#End Region

#Region "Shared Methods"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Creates a processor
    ' </summary>
    ' <param name="docClass"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	4/26/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function CreateProcessor(ByVal docClass As String) As IProcessor
        'check for errors
        If docClass Is Nothing Then
            Throw New ArgumentNullException("docClass")
        End If
        If docClass.Length = 0 Then
            Throw New ArgumentException("DocClass must be of length greater than 0", "docClass")
        End If
        If docClass.ToUpper = "MEDICAL" Then
            Return New MedicalProcessor
        ElseIf docClass.ToUpper = "DENTAL" Then
            Throw New Exception("Not Yet Implemented")
        End If
    End Function
#End Region
End Class