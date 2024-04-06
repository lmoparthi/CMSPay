Option Strict On

Imports System.Security.Cryptography
Imports System.IO
Imports System.Text
Imports System.Security

Public Class UFCWCryptor

    Shared ReadOnly _Entropy As Byte() = System.Text.Encoding.Unicode.GetBytes("Salt Is Not A Password")

    Public Shared ReadOnly Property Password(ByVal databasename As String) As String
        Get
            Dim EncryptedPassword As String = System.Configuration.ConfigurationManager.AppSettings(databasename & "PW")

            Return SimpleCrypt(EncryptedPassword)

        End Get
    End Property

    Public Shared Function Encrypt256BitString(ByVal input As System.Security.SecureString, Optional ByRef tableName As String = Nothing) As String
        Return EncryptString(input.ToString, If(tableName, "") & " CMSDAL")
    End Function

    Public Shared Function EncryptString(ByVal input As System.Security.SecureString) As String
        Dim EncryptedData As Byte() = Cryptography.ProtectedData.Protect(System.Text.Encoding.Unicode.GetBytes(ToInsecureString(input)), _Entropy, System.Security.Cryptography.DataProtectionScope.CurrentUser)
        Return Convert.ToBase64String(EncryptedData)
    End Function

    Public Shared Function EncryptString(ByVal EncryptionValue As String, ByVal EncryptionKey As String) As String
        Dim bytValue() As Byte
        Dim bytKey() As Byte
        Dim bytEncoded() As Byte
        Dim bytIV() As Byte = {121, 241, 10, 1, 132, 74, 11, 39, 255, 91, 45, 78, 14, 211, 22, 62}
        Dim intLength As Integer
        Dim intRemaining As Integer
        Dim objRijndaelManaged As RijndaelManaged

        Try

            bytValue = Encoding.ASCII.GetBytes(EncryptionValue.ToCharArray)

            intLength = Len(EncryptionKey)

            '   ********************************************************************
            '   ******   Encryption Key must be 256 bits long (32 bytes)      ******
            '   ******   If it is longer than 32 bytes it will be truncated.  ******
            '   ******   If it is shorter than 32 bytes it will be padded     ******
            '   ******   with upper-case Xs.                                  ****** 
            '   ********************************************************************

            If intLength >= 32 Then
                EncryptionKey = Strings.Left(EncryptionKey, 32)
            Else
                intLength = Len(EncryptionKey)
                intRemaining = 32 - intLength
                EncryptionKey &= Strings.StrDup(intRemaining, "X")
            End If

            bytKey = Encoding.ASCII.GetBytes(EncryptionKey.ToCharArray)

            objRijndaelManaged = New RijndaelManaged

            Using objMemoryStream As New MemoryStream

                Using objCryptoStream As New CryptoStream(objMemoryStream, objRijndaelManaged.CreateEncryptor(bytKey, bytIV), CryptoStreamMode.Write)

                    objCryptoStream.Write(bytValue, 0, bytValue.Length)

                    If Not objCryptoStream.HasFlushedFinalBlock Then objCryptoStream.FlushFinalBlock()

                    bytEncoded = objMemoryStream.ToArray

                    Return Convert.ToBase64String(bytEncoded)

                End Using
            End Using

        Catch
            Throw
        Finally

        End Try

    End Function

    Public Shared Function DecryptString(ByVal encryptedData As String) As SecureString
        Try
            Dim DecryptedData As Byte() = System.Security.Cryptography.ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), _Entropy, System.Security.Cryptography.DataProtectionScope.CurrentUser)
            Return ToSecureString(System.Text.Encoding.Unicode.GetString(DecryptedData))
        Catch
            Return New SecureString()
        End Try
    End Function

    Public Shared Function Decrypt256BitString(ByVal encryptedData As String, Optional ByRef tableName As String = Nothing) As SecureString
        Try
            Return ToSecureString(DecryptString(encryptedData, If(tableName, "") & " CMSDAL"))
        Catch
            Return New SecureString()
        End Try
    End Function

    Public Shared Function ToSecureString(ByVal input As String) As SecureString

        Try
            Using Secure As New SecureString()

                For Each C As Char In input
                    Secure.AppendChar(C)
                Next
                Secure.MakeReadOnly()

                Return Secure

            End Using

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Function

    Public Shared Function ToInsecureString(ByVal input As SecureString) As String
        Dim ReturnValue As String = String.Empty
        Dim Ptr As IntPtr
        Try
            Ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input)
            ReturnValue = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(Ptr)
        Finally
            System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(Ptr)
        End Try
        Return ReturnValue
    End Function

    Public Shared Function SimpleCrypt(ByVal text As String) As String
        ' Encrypts/decrypts the passed string using 
        ' a simple ASCII value-swapping algorithm
        Dim StrTempChar As String, i As Integer
        For i = 1 To Len(text)
            If Asc(Mid$(text, i, 1)) < 128 Then
                StrTempChar =
          CType(Asc(Mid$(text, i, 1)) + 128, String)
            ElseIf Asc(Mid$(text, i, 1)) > 128 Then
                StrTempChar =
          CType(Asc(Mid$(text, i, 1)) - 128, String)
            End If
            Mid$(text, i, 1) = Chr(CType(StrTempChar, Integer))
        Next i
        Return text
    End Function

    Public Shared Function DecryptString(ByVal DecryptionValue As String, ByVal DecryptionKey As String) As String

        Dim bytDataToBeDecrypted() As Byte
        Dim bytTemp() As Byte
        Dim bytIV() As Byte = {121, 241, 10, 1, 132, 74, 11, 39, 255, 91, 45, 78, 14, 211, 22, 62}
        Dim objRijndaelManaged As New RijndaelManaged
        Dim bytDecryptionKey() As Byte
        Dim intLength As Integer
        Dim intRemaining As Integer
        Dim strReturnString As String = String.Empty

        Try

            bytDataToBeDecrypted = Convert.FromBase64String(DecryptionValue)

            intLength = Len(DecryptionKey)

            If intLength >= 32 Then
                DecryptionKey = Strings.Left(DecryptionKey, 32)
            Else
                intLength = Len(DecryptionKey)
                intRemaining = 32 - intLength
                DecryptionKey &= Strings.StrDup(intRemaining, "X")
            End If

            bytDecryptionKey = Encoding.ASCII.GetBytes(DecryptionKey.ToCharArray)

            ReDim bytTemp(bytDataToBeDecrypted.Length)

            Using objMemoryStream As New MemoryStream(bytDataToBeDecrypted)
                Using objCryptoStream As New CryptoStream(objMemoryStream, objRijndaelManaged.CreateDecryptor(bytDecryptionKey, bytIV), CryptoStreamMode.Read)
                    objCryptoStream.Read(bytTemp, 0, bytTemp.Length)

                    If Not objCryptoStream.HasFlushedFinalBlock Then objCryptoStream.FlushFinalBlock()

                End Using
            End Using

            Return Encoding.ASCII.GetString(bytTemp).Trim(Chr(0))

        Catch ex As Exception
            Throw
        Finally
            If objRijndaelManaged IsNot Nothing Then objRijndaelManaged.Dispose()
        End Try

    End Function






    'Public Shared Function DecryptBasic(ByVal StringToDecrypt As String) As String

    '    Dim dblCountLength As Double
    '    Dim intLengthChar As Short
    '    Dim strCurrentChar As String
    '    Dim dblCurrentChar As Double
    '    Dim intCountChar As Integer
    '    Dim intRandomSeed As Short
    '    Dim intBeforeMulti As Short
    '    Dim intAfterMulti As Short
    '    Dim intSubNinetyNine As Short
    '    Dim intInverseAsc As Short

    '    Dim Decryption As New StringBuilder

    '    Try

    '        For dblCountLength = 1 To Len(StringToDecrypt)
    '            '   Place the character at 'dblCountLength' into the variable
    '            '   'intLengthChar'
    '            intLengthChar = CShort(Mid(StringToDecrypt, CInt(dblCountLength), 1))
    '            '   Place the string 'intLengthChar' long, directly following
    '            '   'dblCountLength' into the variable 'strCurrentChar'
    '            strCurrentChar = Mid(StringToDecrypt, CInt(dblCountLength + 1), intLengthChar)
    '            '   Let the variable 'dblCurrentChar' be equal to 0
    '            dblCurrentChar = 0
    '            '   Start a For...Next loop that counts through the length of the
    '            '   variable 'strCurrentChar'
    '            For intCountChar = 1 To strCurrentChar.Length
    '                '   Convert the variable 'strCurrent' from base 98 to base 10 and
    '                '   place the value into the variable 'dblCurrentChar'
    '                dblCurrentChar = dblCurrentChar + (Asc(Mid(strCurrentChar, intCountChar, 1)) - 33) * (93 ^ (Len(strCurrentChar) - intCountChar))
    '                '   Go to the next character in the variable 'strCurrentChar'
    '            Next intCountChar
    '            '   Determine the random number that was used in the 'Encrypt' function
    '            intRandomSeed = CShort(Mid(CStr(dblCurrentChar), 3, 2))
    '            '   Determine the number that represents the character without the random
    '            '   seed
    '            intBeforeMulti = CShort(Mid(CStr(dblCurrentChar), 1, 2) & Mid(CStr(dblCurrentChar), 5, 2))
    '            '   Divide the number that represents the character by the random seed
    '            '   and place that value into the variable 'intAfterMulti'
    '            intAfterMulti = CShort(intBeforeMulti / intRandomSeed)
    '            '   Subtract 99 from the variable 'intAfterMulti' and place that value
    '            '   into the variable 'intSubNinetyNine'
    '            intSubNinetyNine = CShort(intAfterMulti - 99)
    '            '   Subtract the variable 'intSubNinetyNine' from 256 and place that
    '            '   value into the variable 'intInverseAsc'
    '            intInverseAsc = CShort(256 - intSubNinetyNine)
    '            '   Place the character equivalent of the variable 'intInverseAsc' at the
    '            '   end of the function 'Decrypt'
    '            Decryption.Append(Chr(intInverseAsc))

    '            '   Add the variable 'intLengthChar' to 'dblCountLength' to ensure that
    '            '   the next character is being analyzed
    '            dblCountLength = dblCountLength + intLengthChar
    '            '   Go to the next character in the variable 'StringToEncrypt'
    '        Next dblCountLength

    '        Return Decryption.ToString

    '    Catch ex As Exception

    '        Throw
    '    End Try

    'End Function

    'Public Shared Function EncryptString(ByVal EncryptionValue As String, ByVal EncryptionKey As String) As String
    '    Dim bytValue() As Byte
    '    Dim bytKey() As Byte
    '    Dim bytEncoded() As Byte
    '    Dim bytIV() As Byte = {121, 241, 10, 1, 132, 74, 11, 39, 255, 91, 45, 78, 14, 211, 22, 62}
    '    Dim intLength As Integer
    '    Dim intRemaining As Integer
    '    Dim objMemoryStream As New MemoryStream
    '    Dim objCryptoStream As CryptoStream
    '    Dim objRijndaelManaged As RijndaelManaged

    '    bytValue = Encoding.ASCII.GetBytes(EncryptionValue.ToCharArray)

    '    intLength = Len(EncryptionKey)

    '    '   ********************************************************************
    '    '   ******   Encryption Key must be 256 bits long (32 bytes)      ******
    '    '   ******   If it is longer than 32 bytes it will be truncated.  ******
    '    '   ******   If it is shorter than 32 bytes it will be padded     ******
    '    '   ******   with upper-case Xs.                                  ****** 
    '    '   ********************************************************************

    '    If intLength >= 32 Then
    '        EncryptionKey = Strings.Left(EncryptionKey, 32)
    '    Else
    '        intLength = Len(EncryptionKey)
    '        intRemaining = 32 - intLength
    '        EncryptionKey = EncryptionKey & Strings.StrDup(intRemaining, "X")
    '    End If

    '    bytKey = Encoding.ASCII.GetBytes(EncryptionKey.ToCharArray)

    '    objRijndaelManaged = New RijndaelManaged

    '    Try
    '        objCryptoStream = New CryptoStream(objMemoryStream, objRijndaelManaged.CreateEncryptor(bytKey, bytIV), CryptoStreamMode.Write)
    '        objCryptoStream.Write(bytValue, 0, bytValue.Length)

    '        If Not objCryptoStream.HasFlushedFinalBlock Then objCryptoStream.FlushFinalBlock()

    '        bytEncoded = objMemoryStream.ToArray

    '        Return Convert.ToBase64String(bytEncoded)

    '    Catch
    '    Finally

    '        If objMemoryStream IsNot Nothing Then objMemoryStream.Close()
    '        If objCryptoStream IsNot Nothing Then objCryptoStream.Close()

    '        objMemoryStream = Nothing
    '        objCryptoStream = Nothing

    '    End Try

    'End Function

    'Public Shared Function DecryptString(ByVal DecryptionValue As String, ByVal DecryptionKey As String) As String
    '    Dim bytDataToBeDecrypted() As Byte
    '    Dim bytTemp() As Byte
    '    Dim bytIV() As Byte = {121, 241, 10, 1, 132, 74, 11, 39, 255, 91, 45, 78, 14, 211, 22, 62}
    '    Dim objRijndaelManaged As New RijndaelManaged
    '    Dim objMemoryStream As MemoryStream
    '    Dim objCryptoStream As CryptoStream
    '    Dim bytDecryptionKey() As Byte
    '    Dim intLength As Integer
    '    Dim intRemaining As Integer
    '    Dim strReturnString As String = String.Empty

    '    bytDataToBeDecrypted = Convert.FromBase64String(DecryptionValue)

    '    intLength = Len(DecryptionKey)

    '    If intLength >= 32 Then
    '        DecryptionKey = Strings.Left(DecryptionKey, 32)
    '    Else
    '        intLength = Len(DecryptionKey)
    '        intRemaining = 32 - intLength
    '        DecryptionKey = DecryptionKey & Strings.StrDup(intRemaining, "X")
    '    End If

    '    bytDecryptionKey = Encoding.ASCII.GetBytes(DecryptionKey.ToCharArray)

    '    ReDim bytTemp(bytDataToBeDecrypted.Length)

    '    objMemoryStream = New MemoryStream(bytDataToBeDecrypted)

    '    Try
    '        objCryptoStream = New CryptoStream(objMemoryStream, objRijndaelManaged.CreateDecryptor(bytDecryptionKey, bytIV), CryptoStreamMode.Read)

    '        objCryptoStream.Read(bytTemp, 0, bytTemp.Length)

    '        If Not objCryptoStream.HasFlushedFinalBlock Then objCryptoStream.FlushFinalBlock()
    '    Catch
    '    Finally

    '        If objMemoryStream IsNot Nothing Then objMemoryStream.Close()
    '        If objCryptoStream IsNot Nothing Then objCryptoStream.Close()

    '        objMemoryStream = Nothing
    '        objCryptoStream = Nothing
    '    End Try

    '    Return Encoding.ASCII.GetString(bytTemp).Trim(Chr(0))
    'End Function

    ''Public Function StripNullCharacters(ByVal vstrStringWithNulls As String) As String
    ''    Dim intPosition As Integer
    ''    Dim strStringWithOutNulls As String

    ''    intPosition = 1
    ''    strStringWithOutNulls = vstrStringWithNulls

    ''    Do While intPosition > 0
    ''        intPosition = InStr(intPosition, vstrStringWithNulls, vbNullChar)

    ''        If intPosition > 0 Then
    ''            strStringWithOutNulls = Microsoft.VisualBasic.Left(strStringWithOutNulls, intPosition - 1) & _
    ''                                    Microsoft.VisualBasic.Right(strStringWithOutNulls, Len(strStringWithOutNulls) - intPosition)
    ''        End If

    ''        If intPosition > strStringWithOutNulls.Length Then
    ''            Exit Do
    ''        End If
    ''    Loop

    ''    Return strStringWithOutNulls
    ''End Function
End Class
