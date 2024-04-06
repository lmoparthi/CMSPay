Option Explicit On
Option Strict On

''' -----------------------------------------------------------------------------
''' Project	 : Accumulator
''' Class	 : CMS.DateHelper
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' this class has functions that are used for date handling
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	12/27/2005	Created
''' </history>
''' -----------------------------------------------------------------------------
Friend NotInheritable Class DateHelper
    Private Sub New()
    End Sub
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets the date from the day of year
    ' </summary>
    ' <param name="year"></param>
    ' <param name="dayOfYear"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	12/27/2005	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function GetDateFromDayOfYear(ByVal year As Integer, ByVal dayOfYear As Integer) As Date
        Dim dt As New Date(year, 1, 1)
        dt = dt.AddDays(dayOfYear - 1)
        Return dt
    End Function

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Get Week number from date
    ' </summary>
    ' <param name="inDate"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	12/15/2005	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    'Public Shared Function GetWeekNumber(ByVal inDate As Date) As Integer
    '    Try
    '        Dim dayOfYear As Integer, wkNumber As Integer
    '        Dim compensation As Integer = 0
    '        Dim oneDate As String
    '        Dim FirstDayDate As Date
    '        dayOfYear = inDate.DayOfYear
    '        oneDate = "1/1/" & inDate.Year.ToString
    '        firstDayDate = DateAndTime.DateValue(oneDate)
    '        Select Case firstDayDate.DayOfWeek
    '            Case DayOfWeek.Sunday
    '                compensation = 0
    '            Case DayOfWeek.Monday
    '                compensation = 6
    '            Case DayOfWeek.Tuesday
    '                compensation = 5
    '            Case DayOfWeek.Wednesday
    '                compensation = 4
    '            Case DayOfWeek.Thursday
    '                compensation = 3
    '            Case DayOfWeek.Friday
    '                compensation = 2
    '            Case DayOfWeek.Saturday
    '                compensation = 1
    '        End Select
    '        dayOfYear = dayOfYear - compensation
    '        If dayOfYear Mod 7 = 0 Then
    '            wkNumber = CInt(dayOfYear / 7)
    '        Else
    '            wkNumber = (dayOfYear \ 7) + 1 'WATCH THE OPERATOR \ !!! IT RETURNS THE INTEGER RESULT
    '        End If
    '        'TODO: SEEMS TO HAVE PROBLEMS IF IT IS IN THE FIRST WEEK OR SOMETHING.  IF THIS FUNCTION
    '        'IS GOING TO BE USED, THIS PART MUST BE FIGURED OUT - paulw
    '        If wkNumber = 0 Then wkNumber = 1
    '        Return wkNumber
    '    Catch ex As Exception
    '        Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '        If (rethrow) Then
    '            Throw
    '        End If
    '    End Try
    'End Function
End Class