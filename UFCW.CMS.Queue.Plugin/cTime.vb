''' -----------------------------------------------------------------------------
''' Project	 : Queue
''' Class	 : Claims.UI.cTime
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' this class returns duration
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[Nick Snyder]	3/1/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Class cTime
    Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Public Shared Function ReturnDuration(ByVal dteStartTime As Date) As String
        Dim dteEndTime As Date = UFCWGeneral.NowDate
        Dim strDays As String
        Dim strHours As String
        Dim strMinutes As String
        Dim strSeconds As String
        Dim Tmp As String
        Dim RunLength As System.TimeSpan = dteEndTime.Subtract(dteStartTime)

        '' Format the duration
        Select Case RunLength.Days
            Case Is < 1
                strDays = ""
            Case Is = 1
                strDays = RunLength.Days & " Day "
            Case Is > 1
                strDays = RunLength.Days & " Days "
        End Select

        Select Case RunLength.Hours
            Case Is < 1
                strHours = ""
            Case Is = 1
                strHours = RunLength.Hours & " Hr "
            Case Is > 1
                strHours = RunLength.Hours & " Hrs "
        End Select

        Select Case RunLength.Minutes
            Case Is < 1
                strMinutes = ""
            Case Is = 1
                strMinutes = RunLength.Minutes & " Min "
            Case Is > 1
                strMinutes = RunLength.Minutes & " Mins "
        End Select

        Select Case RunLength.Seconds
            Case Is < 1
                strSeconds = ""
            Case 1
                strSeconds = RunLength.Seconds & " Sec "
            Case Is > 1
                strSeconds = RunLength.Seconds & " Secs "
        End Select

        '' Display the duration to the user
        Tmp = strDays & strHours & strMinutes & strSeconds
        If Tmp = "" Then Tmp = "0 Secs"

        Return Tmp
    End Function

    Public Shared Function ReturnDuration(ByVal dteStartTime As DateTime, ByVal dteEndTime As DateTime) As String
        Dim strDays As String
        Dim strHours As String
        Dim strMinutes As String
        Dim strSeconds As String
        Dim Tmp As String
        Dim RunLength As System.TimeSpan = dteEndTime.Subtract(dteStartTime)

        '' Format the duration
        Select Case RunLength.Days
            Case Is < 1
                strDays = ""
            Case Is = 1
                strDays = RunLength.Days & " Day "
            Case Is > 1
                strDays = RunLength.Days & " Days "
        End Select

        Select Case RunLength.Hours
            Case Is < 1
                strHours = ""
            Case Is = 1
                strHours = RunLength.Hours & " Hr "
            Case Is > 1
                strHours = RunLength.Hours & " Hrs "
        End Select

        Select Case RunLength.Minutes
            Case Is < 1
                strMinutes = ""
            Case Is = 1
                strMinutes = RunLength.Minutes & " Min "
            Case Is > 1
                strMinutes = RunLength.Minutes & " Mins "
        End Select

        Select Case RunLength.Seconds
            Case Is < 1
                strSeconds = ""
            Case 1
                strSeconds = RunLength.Seconds & " Sec "
            Case Is > 1
                strSeconds = RunLength.Seconds & " Secs "
        End Select

        '' Display the duration to the user
        Tmp = strDays & strHours & strMinutes & strSeconds
        If Tmp = "" Then Tmp = "0 Secs"

        Return Tmp
    End Function
End Class