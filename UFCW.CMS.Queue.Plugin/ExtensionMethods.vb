Module ExtensionMethods

    <System.Runtime.CompilerServices.Extension()> _
    Public Function IsInteger(ByVal value As String) As Boolean
        If String.IsNullOrEmpty(value) Then
            Return False
        End If
        Dim tempNo As Integer
        Return Integer.TryParse(value, tempNo)
    End Function

    <System.Runtime.CompilerServices.Extension()> _
    Public Function IsCharacterAlphaNumeric(ByVal value As Char) As Boolean

        If String.IsNullOrEmpty(value) Then
            Return False
        End If

        Dim lFnd As Long
        Const sAlphaNum As String = "0123456789abcdefghijklmnopqrstuvwxyz"
        lFnd = InStr(1, sAlphaNum, value.ToString.ToLower, vbTextCompare)

        Return If(lFnd > 0, True, False)

    End Function

End Module