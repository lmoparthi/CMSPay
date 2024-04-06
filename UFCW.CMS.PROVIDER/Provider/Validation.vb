
Public Class RegularExpression

    Public Const PhoneNumber As String = "^\s*([\(]?)\[?\s*\d{3}\s*\]?[\)]?\s*[\-]?[\.]?\s*\d{3}\s*[\-]?[\.]?\s*\d{4}$"
    Public Const ZIPCode As String = "^\d{5}$"
    Public Const AlphaOnly As String = "^[a-zA-Z]+$"
    Public Const NumberOnly As String = "^[0-9]*$"
    Public Const NoNumbers As String = "^[a-zA-Z\040]+$"
    Public Const Email As String = "^((?>[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+\x20*|""((?=[\x01-\x7f])[^""\\]|\\[\x01-\x7f])*" & _
"""\x20*)*(?<angle><))?((?!\.)(?>\.?[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+)+|""((?=[\x01-\" & _
"x7f])[^""\\]|\\[\x01-\x7f])*"")@(((?!-)[a-zA-Z\d\-]+(?<!-)\.)+[a-zA-Z]{2,}|\[(((?(" & _
"?<!\[)\.)(25[0-5]|2[0-4]\d|[01]?\d?\d)){4}|[a-zA-Z\d\-]*[a-zA-Z\d]:((?=[\x01-\x7" & _
"f])[^\\\[\]]|\\[\x01-\x7f])+)\])(?(angle)>)$" & Microsoft.VisualBasic.ChrW(9)

    Public Shared Function IsEmail(ByVal ComparisonText As String) As Boolean

        Dim options As System.Text.RegularExpressions.RegexOptions = ((System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace Or System.Text.RegularExpressions.RegexOptions.Multiline) _
                    Or System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim reg As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex(Email, options)

        Return reg.IsMatch(ComparisonText)

    End Function
    Public Shared Function IsPhoneNumber(ByVal ComparisonText As String) As Boolean

        Dim options As System.Text.RegularExpressions.RegexOptions = ((System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace Or System.Text.RegularExpressions.RegexOptions.Multiline) _
                    Or System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim reg As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex(PhoneNumber, options)

        Return reg.IsMatch(ComparisonText)

    End Function
    Public Shared Function IsZIPCode(ByVal ComparisonText As String) As Boolean

        Dim options As System.Text.RegularExpressions.RegexOptions = ((System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace Or System.Text.RegularExpressions.RegexOptions.Multiline) _
                    Or System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim reg As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex(ZipCode, options)

        Return reg.IsMatch(ComparisonText)

    End Function
    Public Shared Function RestrictToNumeric(ByVal ComparisonText As String) As Boolean

        Dim options As System.Text.RegularExpressions.RegexOptions = ((System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace Or System.Text.RegularExpressions.RegexOptions.Multiline) _
                    Or System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim reg As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex(NumberOnly, options)

        Return reg.IsMatch(ComparisonText)

    End Function
    Public Shared Function RestrictToAlphaWithSpaces(ByVal ComparisonText As String) As Boolean

        Dim options As System.Text.RegularExpressions.RegexOptions = ((System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace Or System.Text.RegularExpressions.RegexOptions.Multiline) _
                    Or System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim reg As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex(NoNumbers, options)

        Return reg.IsMatch(ComparisonText)

    End Function
    Public Shared Function RestrictToAlphaNoSpaces(ByVal ComparisonText As String) As Boolean

        Dim options As System.Text.RegularExpressions.RegexOptions = ((System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace Or System.Text.RegularExpressions.RegexOptions.Multiline) _
                    Or System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim reg As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex(AlphaOnly, options)

        Return reg.IsMatch(ComparisonText)

    End Function
    Public Shared Function RestrictToAlphaWithPunctuation(ByVal ComparisonText As String) As Boolean

        Dim regex As String = "[a-zA-Z]"

        Dim options As System.Text.RegularExpressions.RegexOptions = ((System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace Or System.Text.RegularExpressions.RegexOptions.Multiline) _
                    Or System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim reg As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex(regex, options)

        Return reg.IsMatch(ComparisonText)

    End Function
    Public Shared Function RestrictToAlphaNumericNoSpaces(ByVal ComparisonText As String) As Boolean

        Dim regex As String = "^[a-zA-Z0-9]+$"

        Dim options As System.Text.RegularExpressions.RegexOptions = ((System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace Or System.Text.RegularExpressions.RegexOptions.Multiline) _
                    Or System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim reg As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex(regex, options)

        Return reg.IsMatch(ComparisonText)

    End Function
End Class
