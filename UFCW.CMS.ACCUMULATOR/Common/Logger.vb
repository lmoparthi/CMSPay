Imports System.Configuration
Public Class TraceLog

    Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
    Private Shared _Log As Boolean
    Private Shared _Retrieved As Boolean = False

    Private Shared Function GetLogSetting() As Boolean

        If Not _Retrieved Then
            Try
                _Log = CType(ConfigurationManager.GetSection("ShowLoadTime"), IDictionary)("LogBackEnd")
            Catch ex As Exception
                _Log = False
            End Try
            _Retrieved = True
        End If

        Return _Log
    End Function

    Public Shared Sub WriteToFile(ByVal fileName As String, ByVal contents As String)
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
        Dim StreamWriter As IO.StreamWriter

        Try

            _Log = GetLogSetting()

            If _Log Then
                StreamWriter = New IO.StreamWriter(fileName, True)
                StreamWriter.WriteLine(contents)
            End If

        Catch ex As Exception
            Throw
        Finally
            If StreamWriter IsNot Nothing Then
                StreamWriter.Close()
                StreamWriter = Nothing
            End If
        End Try
    End Sub

    Public Shared Sub WriteToFile(ByVal fileName As String, ByVal contents As String, ByVal ignoreConfigSettings As Boolean)

        Dim StreamWriter As IO.StreamWriter

        Try

            _Log = GetLogSetting()

            If IgnoreConfigSettings Then _Log = True

            If _Log Then
                StreamWriter = New IO.StreamWriter(FileName, True)
                StreamWriter.WriteLine(Contents)
            End If

        Catch ex As Exception
            Throw
        Finally
            If StreamWriter IsNot Nothing Then
                StreamWriter.Close()
                StreamWriter = Nothing
            End If
        End Try

    End Sub
End Class