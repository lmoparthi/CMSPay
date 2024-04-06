Imports System.Linq.Expressions
Imports System.Text.RegularExpressions
Imports System.Windows.Forms

Public NotInheritable Class PatternMatcher
    Private Shared ReadOnly _TraceGeneral As New TraceSwitch("TraceGeneral", "Trace Switch in App.Config", "0")

    Private Sub New()
    End Sub

    ' Fields
    Private Const ANSI_DOS_QM As Char = "<"c
    Private Const ANSI_DOS_STAR As Char = ">"c
    Private Const DOS_DOT As Char = """"c
    Private Const MATCHES_ARRAY_SIZE As Integer = 16
    Public Shared Function ClipBoardCleaner(sender As Object, e As KeyEventArgs, regExpressions() As String) As Boolean

        Try

            If (e.KeyCode = Keys.V AndAlso e.Modifiers = Keys.Control) OrElse (e.KeyCode = Keys.Insert AndAlso e.Modifiers = Keys.Shift) Then

                Dim ClipText As String = Clipboard.GetText
                Dim ReturnText As String = ClipText
                Dim Reg As Regex

                For Each regExpression As String In regExpressions
                    Reg = New Regex(regExpression)
                    ReturnText = Reg.Replace(ReturnText, String.Empty)
                Next

                If ClipText <> ReturnText Then
                    If ReturnText.Length > 0 Then
                        Clipboard.SetText(ReturnText)
                    Else
                        Clipboard.Clear()
                    End If

                End If

            End If

        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Shared Function ClipBoardCleaner(regExpressions() As String, Optional options As RegexOptions = RegexOptions.None) As Boolean

        Try
            'if activated the assumption is this will clean up the clipboard to ensure the field being populated will not have invalid data deposited, only characters within the regex mask will be allowed. 
            If Clipboard.ContainsText Then

                Dim ClipText As String = Clipboard.GetText
                Dim ReturnText As String = ClipText
                '                Dim Reg As Regex

                For Each regExpression As String In regExpressions
                    'Reg = New Regex(regExpression)
                    ReturnText = Regex.Replace(ReturnText, regExpression, String.Empty, options)
                Next

                If ClipText = ReturnText Then
                    Return True
                ElseIf ClipText <> ReturnText Then
                    If ReturnText.Length > 0 Then
                        Clipboard.SetText(ReturnText)
                        Return True
                    End If
                End If

            End If

            Clipboard.Clear()
            Return False

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function FitsMaskUsingRegEX(fileName As String, fileMask As String) As Boolean

        fileMask = fileMask.Trim

        If fileMask.EndsWith("*.*") Then fileMask = fileMask.Replace("*.*", "__EIGHTdotTHREE__") 'filenames ending with any suffix, and an unknown prefix end

        If fileMask.EndsWith(".*") Then 'filenames ending with any suffix
            fileMask = fileMask.Substring(0, fileMask.Length - 2) & "__ANYSUFFIX__"
            If Not fileName.Contains(".") Then fileName &= "." 'to allow mask of .* to still match
        End If

        If fileMask.StartsWith("*.") Then 'filenames starting with any prefix
            fileMask = "__ANYPREFIX__" & fileMask.Substring(2)
            If Not fileName.Contains(".") Then fileName &= "." 'to allow mask of .* to still match
        End If

        If fileMask.EndsWith("*") Then fileMask = fileMask.Substring(0, fileMask.Length - 1) & "__ANYENDING__" 'filenames ending with anything

        fileMask = fileMask.Replace("*", "__ANYSTRING__") 'Any period in the file that was not part of the suffix
        fileMask = fileMask.Replace(".", "__APERIOD__") 'Any period in the file that was not part of the suffix
        fileMask = fileMask.Replace("?", "__ANYCHARACTER__") 'Any single character
        fileMask = fileMask.Replace(" ", "__BLANK__") 'Any single character

        Dim pattern As String = Regex.Escape(fileMask).Replace("__ANYPREFIX__", "^.*\.").Replace("__ANYSUFFIX__", "\..*$").Replace("__APERIOD__", "\.").Replace("__ANYCHARACTER__", ".").Replace("__ANYSTRING__", ".*").Replace("__BLANK__", " ").Replace("__ANYENDING__", ".*$").Replace("__EIGHTdotTHREE__", ".*\..*$")
        pattern = If(pattern.StartsWith("^"c), "", "^"c) & pattern
        pattern &= If(pattern.EndsWith("$"c), "", "$"c)

        Return New Regex(pattern, RegexOptions.IgnoreCase).IsMatch(fileName)

    End Function

    ' Methods
    Public Shared Function FitsMask(name As String, expression As String) As Boolean

        expression = expression.ToUpperInvariant()
        name = name.ToUpperInvariant()

        Dim num9 As Integer
        Dim ch As Char = ControlChars.NullChar
        Dim ch2 As Char = ControlChars.NullChar
        Dim sourceArray As Integer() = New Integer(15) {}
        Dim numArray2 As Integer() = New Integer(15) {}
        Dim Flag As Boolean = False
        If ((name Is Nothing) OrElse (name.Length = 0)) OrElse ((expression Is Nothing) OrElse (expression.Length = 0)) Then
            Return False
        End If
        If expression.Equals("*") OrElse expression.Equals("*.*") Then
            Return True
        End If
        If (expression(0) = "*"c) AndAlso (expression.IndexOf("*"c, 1) = -1) Then
            Dim length As Integer = expression.Length - 1
            If (name.Length >= length) AndAlso (String.Compare(expression, 1, name, name.Length - length, length, StringComparison.OrdinalIgnoreCase) = 0) Then
                Return True
            End If
        End If
        sourceArray(0) = 0
        Dim num7 As Integer = 1
        Dim num As Integer = 0
        Dim num8 As Integer = expression.Length * 2
        While Not Flag
            Dim num3 As Integer
            If num < name.Length Then
                ch = name(num)
                num3 = 1
                num += 1
            Else
                Flag = True
                If sourceArray(num7 - 1) = num8 Then
                    Exit While
                End If
            End If
            Dim index As Integer = 0
            Dim num5 As Integer = 0
            Dim num6 As Integer = 0
            While index < num7
                Dim num2 As Integer = (sourceArray(System.Math.Max(System.Threading.Interlocked.Increment(index), index - 1)) + 1) \ 2
                num3 = 0
Label_00F2:
                If num2 <> expression.Length Then
                    num2 += num3
                    num9 = num2 * 2
                    If num2 = expression.Length Then
                        numArray2(System.Math.Max(System.Threading.Interlocked.Increment(num5), num5 - 1)) = num8
                    Else
                        ch2 = expression(num2)
                        num3 = 1
                        If num5 >= 14 Then
                            Dim num11 As Integer = numArray2.Length * 2
                            Dim destinationArray As Integer() = New Integer(num11 - 1) {}
                            Array.Copy(numArray2, destinationArray, numArray2.Length)
                            numArray2 = destinationArray
                            destinationArray = New Integer(num11 - 1) {}
                            Array.Copy(sourceArray, destinationArray, sourceArray.Length)
                            sourceArray = destinationArray
                        End If
                        If ch2 = "*"c Then
                            numArray2(System.Math.Max(System.Threading.Interlocked.Increment(num5), num5 - 1)) = num9
                            numArray2(System.Math.Max(System.Threading.Interlocked.Increment(num5), num5 - 1)) = num9 + 1
                            GoTo Label_00F2
                        End If
                        If ch2 = ">"c Then
                            Dim Flag2 As Boolean = False
                            If Not Flag AndAlso (ch = "."c) Then
                                Dim num13 As Integer = name.Length
                                For i As Integer = num To num13 - 1
                                    Dim ch3 As Char = name(i)
                                    num3 = 1
                                    If ch3 = "."c Then
                                        Flag2 = True
                                        Exit For
                                    End If
                                Next
                            End If
                            If (Flag OrElse (ch <> "."c)) OrElse Flag2 Then
                                numArray2(System.Math.Max(System.Threading.Interlocked.Increment(num5), num5 - 1)) = num9
                                numArray2(System.Math.Max(System.Threading.Interlocked.Increment(num5), num5 - 1)) = num9 + 1
                            Else
                                numArray2(System.Math.Max(System.Threading.Interlocked.Increment(num5), num5 - 1)) = num9 + 1
                            End If
                            GoTo Label_00F2
                        End If
                        num9 += num3 * 2
                        Select Case ch2
                            Case "<"c
                                If Flag OrElse (ch = "."c) Then
                                    GoTo Label_00F2
                                End If
                                numArray2(System.Math.Max(System.Threading.Interlocked.Increment(num5), num5 - 1)) = num9
                                GoTo Label_028D

                            Case """"c
                                If Flag Then
                                    GoTo Label_00F2
                                End If
                                If ch = "."c Then
                                    numArray2(System.Math.Max(System.Threading.Interlocked.Increment(num5), num5 - 1)) = num9
                                    GoTo Label_028D
                                End If
                                Exit Select
                        End Select
                        If Not Flag Then
                            If ch2 = "?"c Then
                                numArray2(System.Math.Max(System.Threading.Interlocked.Increment(num5), num5 - 1)) = num9
                            ElseIf ch2 = ch Then
                                numArray2(System.Math.Max(System.Threading.Interlocked.Increment(num5), num5 - 1)) = num9
                            End If
                        End If
                    End If
                End If
Label_028D:
                If (index < num7) AndAlso (num6 < num5) Then
                    While num6 < num5
                        Dim num14 As Integer = sourceArray.Length
                        While (index < num14) AndAlso (sourceArray(index) < numArray2(num6))
                            index += 1
                        End While
                        num6 += 1
                    End While
                End If
            End While
            If num5 = 0 Then
                Return False
            End If
            Dim numArray4 As Integer() = sourceArray
            sourceArray = numArray2
            numArray2 = numArray4
            num7 = num5
        End While
        num9 = sourceArray(num7 - 1)
        Return (num9 = num8)
    End Function
End Class
