Imports System.Configuration
Public Class TraceLog

    Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
    Private Shared _Log As Boolean
    Private Shared _Retrieved As Boolean = False
    Private Shared Function GetLogSetting() As Boolean
        Try

            If Not _Retrieved Then
                _Log = CBool(CType(ConfigurationManager.GetSection("ShowLoadTime"), IDictionary)("LogBackEnd"))
                _Retrieved = True
            End If

            Return _Log

        Catch ex As Exception

            Return False

        End Try
    End Function

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Used for logging purposes
    ' </summary>
    ' <param name="fileName"></param>
    ' <param name="contents"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	9/12/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Sub WriteToFile(ByVal fileName As String, ByVal contents As String)
        Dim StreamWriter As IO.StreamWriter
        Try
            _Log = GetLogSetting()
            If _Log Then
                StreamWriter = New IO.StreamWriter(fileName, True)
                StreamWriter.WriteLine(contents)
            End If
        Catch ex As Exception

        Finally
            If StreamWriter IsNot Nothing Then StreamWriter.Close()
            StreamWriter = Nothing
        End Try
    End Sub
End Class