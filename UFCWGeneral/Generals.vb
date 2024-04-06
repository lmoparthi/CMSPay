Imports System.IO
Imports System.Windows.Forms

<System.Diagnostics.DebuggerStepThrough()>
Public Class UFCWGeneral

    Public Const DB2TimeFormat As String = "yyyy-MM-dd-hh.mm.ss.000000"
    Public Const DB2DateFormat As String = "yyyy-MM-dd"

    Public Shared AppInfo As New FileInfo(Application.ExecutablePath)

    Public Const increone As Integer = 1
    Public Shared ReadOnly DomainUser As String = SystemInformation.UserName
    Public Shared ReadOnly PCName As String = SystemInformation.ComputerName

    Public Shared Function FormatSSN(ByRef strSSN As String) As String
        Dim strTemp As String

        strTemp = UnFormatSSN(strSSN)
        If strTemp.Trim <> "" Then
            Return Microsoft.VisualBasic.Left(strTemp, 3) & "-" & Microsoft.VisualBasic.Mid(strTemp, 4, 2) & "-" & Microsoft.VisualBasic.Right(strTemp, 4)
        Else
            Return ""
        End If
    End Function

    Public Shared Function DecryptSSN(ByVal ssn As String) As Integer
        Return CInt(ssn.ToUpper.Replace("N"c, "0"c).
            Replace("E"c, "1"c).Replace("D"c, "2"c).Replace("T"c, "3"c).
            Replace("F"c, "4"c).Replace("C"c, "5"c).Replace("A"c, "6"c).
            Replace("P"c, "7"c).Replace("W"c, "8"c).Replace("S"c, "9"c).
            Replace("-"c, ""))
    End Function

    Public Shared Function UnFormatSSN(ByRef strSSN As String) As String
        If Replace(Replace(Replace(strSSN, " ", ""), "-", ""), "/", "") <> "" Then
            Return Format(CLng(Replace(Replace(Replace(strSSN, " ", ""), "-", ""), "/", "")), "0########")
        Else
            Return ""
        End If
    End Function

    Public Shared Function FormatTIN(ByVal tin As String) As String

        tin = UnFormatTIN(tin)

        If IsNumeric(tin) = True Then
            tin = Format(CLng(tin), "00-0000000")
        End If

        Return tin

    End Function

    Public Shared Function UnFormatTIN(ByVal tin As String) As String

        tin = Replace(Replace(Replace(tin.ToUpper.Trim, "/", ""), "-", ""), " ", "")
        tin = UnFormatSSN(tin)

        Return tin

    End Function

End Class