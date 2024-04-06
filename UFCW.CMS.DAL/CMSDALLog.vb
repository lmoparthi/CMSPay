Option Strict On

Imports Microsoft.Practices.EnterpriseLibrary.Common.Configuration
Imports Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
Imports Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports System.IO
Imports System.Text
Imports System.Configuration
Imports System.Diagnostics
Imports System.Threading

<ConfigurationElementType(GetType(CustomTraceListenerData))>
Public Class CMSDALCustomLog
    Inherits CustomTraceListener

    Shared CMSDALCustomLogLock As New Object

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
    ''' <summary>
    ''' Write message to named file
    ''' </summary>
    Private Shared Sub Log(ByVal logMessage As String, ByVal w As TextWriter)

        SyncLock CMSDALCustomLogLock
        w.WriteLine("{0}", logMessage)
        End SyncLock

    End Sub

    Public Overrides Sub TraceData(ByVal eventCache As TraceEventCache, ByVal source As String, ByVal eventType As TraceEventType, ByVal id As Integer, ByVal data As Object)

        Try
        If (TypeOf data Is LogEntry) AndAlso Me.Formatter IsNot Nothing Then
            WriteOutToLog(Me.Formatter.Format(DirectCast(data, LogEntry)), DirectCast(data, LogEntry))
        Else
            WriteOutToLog(data.ToString(), DirectCast(data, LogEntry))
        End If

        Catch ex As Exception

        End Try

    End Sub

    Public Overrides Sub Write(ByVal message As String)
        Debug.Print(message.ToString)
    End Sub

    Public Overrides Sub WriteLine(ByVal message As String)
        Debug.Print(message.ToString)
    End Sub

    Private Shared Sub WriteOutToLog(ByVal bodyText As String, ByVal logentry As LogEntry)

        SyncLock CMSDALCustomLogLock

        Try


                'Debug.Print("FileLocation: " & logentry.ExtendedProperties("filelocation").ToString())
                'Get the filelocation from the extended properties
                Dim FullPath As String = Path.GetFullPath(logentry.ExtendedProperties("filelocation").ToString())

                'Create the directory where the log file is written to if it does not exist.
                Dim directoryInfo As New DirectoryInfo(Path.GetDirectoryName(FullPath))

                If directoryInfo.Exists = False Then
                    directoryInfo.Create()
                End If

                Using writer As StreamWriter = File.AppendText(FullPath)
                    writer.WriteLine($"{bodyText} - {Thread.CurrentThread.ManagedThreadId}")
                End Using

                'Using fs As New FileStream(FullPath, FileMode.Append, FileAccess.Write, FileShare.Write, 4096, True)
                '    Using sw As New StreamWriter(fs, Encoding.UTF8)
                '        Log(bodyText, sw)
                '    End Using
                'End Using

        Catch ex As Exception
            Throw New LoggingException(ex.Message, ex)
        Finally
        End Try
        End SyncLock


    End Sub
End Class

Public Delegate Sub LoggedTextDelegate(Of T)(ByVal item As T)

Public Class CMSDALLog

    Private Shared _LogFileMask As String = If(ConfigurationManager.AppSettings("LogFileMask") Is Nothing, "yyyyMMddhhmmss", ConfigurationManager.AppSettings("LogFileMask").ToString.Trim)
    Private Shared _LogFilePathMask As String = If(ConfigurationManager.AppSettings("LogFilePathMask") Is Nothing, "yyyyMM", ConfigurationManager.AppSettings("LogFilePathMask").ToString.Trim)
    Private Shared _LogPath As String
    Private Shared _LogPathFileName As String
    Private Shared _StreamWriter As StreamWriter
    Private Shared _StreamWriterSL As New Object
    Private Shared _SymbolicReplacementCode As String() = Nothing
    Private Shared _SymbolicReplacementValue As String() = Nothing
    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
    Private Shared _TranLogOpen As Boolean = False

    Public Shared ReadOnly Property CurrentLog As String
        Get
            Return _LogPathFileName
        End Get
    End Property


    Public Shared Sub Log(ByVal message As String, ByVal destination As String)

        Try

            Dim LogEntry As New LogEntry With {
                .Message = message
            }
            LogEntry.ExtendedProperties.Add("filelocation", destination)
            LogEntry.Categories.Add("Log2File")

            Logger.Write(LogEntry)

        Catch ex As Exception

            Throw

        End Try

    End Sub

    Public Shared Function LogDirectory(Optional symbolicReplacementCode As String() = Nothing, Optional symbolicReplacementValue As String() = Nothing) As String

        Dim LogPath As String = ""

        If symbolicReplacementCode IsNot Nothing Then
            _SymbolicReplacementCode = symbolicReplacementCode
        End If

        If symbolicReplacementValue IsNot Nothing Then
            _SymbolicReplacementValue = symbolicReplacementValue
        End If

        _LogPath = ConfigurationManager.AppSettings("LogDirectory").ToString

        If _LogPath IsNot Nothing AndAlso _LogPath.Length > 0 Then
            '..\    go back 1 folder
            '.\     current folder
            Select Case True
                Case _LogPath.StartsWith("..\")
                    LogPath = _LogPath.Replace("..\", UFCWGeneral.StartupPath.Substring(0, UFCWGeneral.StartupPath.LastIndexOf("\")) & "\")

                Case _LogPath.StartsWith(".\")
                    LogPath = _LogPath.Replace(".\", UFCWGeneral.StartupPath & "\")
                Case _LogPath.StartsWith("\\")
                    LogPath = _LogPath & If(_LogPath.EndsWith("\"), "", "\")
            End Select
        Else
            LogPath = UFCWGeneral.StartupPath & "\"
        End If

        If _SymbolicReplacementCode IsNot Nothing Then
            For X As Integer = 0 To _SymbolicReplacementCode.Length - 1
                LogPath = LogPath.Replace(_SymbolicReplacementCode(X), _SymbolicReplacementValue(X))
            Next
        End If

        If _LogFilePathMask IsNot Nothing Then
            _LogFilePathMask = If(_LogFilePathMask.Trim.Length = 0, "yyyyMMdd", _LogFilePathMask)
        End If

        LogPath &= If(_LogFilePathMask.Trim.Length = 0, "", Format$(Now(), _LogFilePathMask) & "\")

        If Not Directory.Exists(LogPath.Trim) Then
            Directory.CreateDirectory(LogPath.Trim)
        End If

        Return LogPath.Trim

    End Function

    Public Shared Sub TranLog_Close()

        If _StreamWriter IsNot Nothing Then _StreamWriter.Close()
        _TranLogOpen = False

    End Sub

    Public Shared Function TranLog_Init(Optional symbolicReplacementCode As String() = Nothing, Optional symbolicReplacementValue As String() = Nothing) As String

        Try

            If symbolicReplacementCode IsNot Nothing Then
                _SymbolicReplacementCode = symbolicReplacementCode
            End If

            If symbolicReplacementValue IsNot Nothing Then
                _SymbolicReplacementValue = symbolicReplacementValue
            End If

            _LogPathFileName = LogDirectory(_SymbolicReplacementCode, _SymbolicReplacementValue)

            _LogPathFileName &= Format$(UFCWGeneral.NowDate, _LogFileMask) & ".Log"
            _StreamWriter = File.AppendText(_LogPathFileName)

            _TranLogOpen = True

            UFCWLastKeyData.Log = CMSDALLog.CurrentLog

            Return _LogPathFileName

        Catch Exp As FileNotFoundException

            _StreamWriter = File.CreateText(_LogPathFileName)
            _TranLogOpen = True

        Catch Ex As Exception

            Throw
        Finally

            If _StreamWriter IsNot Nothing Then
                TranLog_Write("Log Location -> " & _LogPathFileName)
                TranLog_Write("Machine --> " & UFCWGeneral.ComputerName & " Logon --> " & UFCWGeneral.WindowsUserID.Name)
                _StreamWriter.AutoFlush = True
            End If

        End Try

    End Function

    Public Shared Sub TranLog_Write(ByRef responseDelegate As LoggedTextDelegate(Of String), ByVal psText As String, Optional ByVal pbNoTime As Boolean = False)

        Dim bNoTime As Boolean
        Dim LogText As New StringBuilder

        If Not _TranLogOpen Then Exit Sub

        Try

            If Not bNoTime Then
                LogText.Append(UFCWGeneral.NowDate.ToShortDateString & " " & UFCWGeneral.NowDate.ToLongTimeString & Space(4) & psText)
            Else
                LogText.Append(psText)
            End If

            SyncLock _StreamWriterSL
                _StreamWriter.WriteLine(LogText.ToString)
            End SyncLock

        Catch Exp As Exception

            _StreamWriter.WriteLine(Exp.Message)

            Throw
        Finally

            If LogText.ToString.Trim.Length > 0 Then responseDelegate.Invoke(LogText.ToString)

            LogText = Nothing
        End Try


    End Sub

    Public Shared Sub TranLog_Write(ByVal psText As String, Optional ByVal pbNoTime As Boolean = False)

        Dim bNoTime As Boolean
        Dim LogText As New StringBuilder

        If Not _TranLogOpen Then Exit Sub

        Try
            If Not bNoTime Then
                LogText.Append(UFCWGeneral.NowDate.ToShortDateString & " " & UFCWGeneral.NowDate.ToLongTimeString & Space(4) & psText)
            Else
                LogText.Append(psText)
            End If

            _StreamWriter.WriteLine(LogText.ToString)

        Catch Exp As Exception

            _StreamWriter.WriteLine(Exp.Message)

            Throw
        Finally
            LogText = Nothing
        End Try

    End Sub
End Class
