
''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Binder
''' Class	 : CMS.Binder.BinderFactory
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' A BinderFactory
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/5/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public NotInheritable Class BinderFactory

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private Sub New()

    End Sub
    Public Shared Function CreateBinder(ByVal claimID As Integer, ByVal docClass As String, ByVal typeOfClaim As Integer?) As Binder
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Creates a binder
        ' </summary>
        ' <param name="claimId"></param>
        ' <param name="docClass"></param>
        ' <param name="typeOfClaim"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        If docClass Is Nothing OrElse docClass.Trim.Length = 0 Then
            Throw New ArgumentException("DocClass is required.", "docClass")
        End If

        Select Case docClass.ToUpper
            Case "MEDICAL"
                Return New MedicalBinder(claimID, typeOfClaim)
            Case "DENTAL"
                Throw New Exception("Not Implemented")
        End Select
    End Function
End Class