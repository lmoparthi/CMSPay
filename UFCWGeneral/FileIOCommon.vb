Imports System.IO
Imports System.Security.Permissions
Imports System.Collections.Generic
Imports System.Threading

Public Class FileIOCommon

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _FullName As String
    Private _FileName As String
    Private _FilePath As String
    Private _Extension As String
    Private _Size As Long

    Public ReadOnly Property FileName() As String
        Get
            Return _FileName
        End Get
    End Property

    Public ReadOnly Property FilePath() As String
        Get
            Return _FilePath
        End Get
    End Property

    Public ReadOnly Property FullName() As String
        Get
            Return _FullName
        End Get
    End Property

    Public ReadOnly Property Extension() As String
        Get
            Return If(Trim(_Extension).StartsWith("."), Trim(_Extension).Substring(1), Trim(_Extension))
        End Get
    End Property

    Public ReadOnly Property Size() As Long
        Get
            Return _Size
        End Get
    End Property

    Public Sub New(f As FileInfo)
        _FileName = f.Name
        _FilePath = f.DirectoryName
        _FullName = f.FullName
        _Extension = f.Extension
        _Size = f.Length
    End Sub

    Public Sub New(ftpfile As FTPfileInfo)

        _FileName = ftpfile.Filename
        _FilePath = ftpfile.Path
        _FullName = ftpfile.FullName
        _Extension = ftpfile.Extension
        _Size = ftpfile.Size
    End Sub

    Public Sub New(fullName As String, fileName As String, filePath As String, extension As String, size As Long)

        _FileName = fileName
        _FilePath = filePath
        _FullName = fullName
        _Extension = extension
        _Size = size
    End Sub

    Public Shared Function WaitForFile(ByVal fullPath As String, ByVal mode As FileMode, ByVal access As FileAccess, ByVal share As FileShare) As Boolean

        For numTries As Integer = 0 To 9

            Try

                Using fs As FileStream = New FileStream(fullPath, mode, access, share)
                    Thread.Sleep(500)
                    Return True
                End Using

            Catch ex As IOException
                Debug.Print("WaitForFile: IOException " & ex.Message)

                Thread.Sleep(500)
            End Try

        Next numTries

        Return False
    End Function

    Public Shared Sub DeleteTemporaryFiles(filesLocation As String)

        Dim DI As New DirectoryInfo(filesLocation)

        Try

            If DI.Exists Then

                System.GC.Collect()
                System.GC.WaitForPendingFinalizers()

                DeleteDirectoryContents(filesLocation)
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Shared Sub DeleteDirectoryContents(path As String)

        Dim DI As New DirectoryInfo(path)

        DI.GetDirectories().ToList().ForEach(Sub(d) DeleteDirectoryContents(d.FullName))
        DI.GetFiles().ToList().ForEach(Sub(f)
                                           f.Attributes = FileAttributes.Normal
                                           f.Delete()
                                       End Sub)

        System.GC.Collect()
        System.GC.WaitForPendingFinalizers()

        DI.Delete(True)

        Thread.Sleep(50)

    End Sub

    Public Shared Function LineCount(dataSetName As String) As Integer

        Dim Rows As Integer

        Try
            Rows = File.ReadAllLines(dataSetName).Length

            Return Rows

        Catch ex As Exception

            Throw
        End Try

    End Function
End Class

Public Class Watcher
    Private _PathName As String
    Private _Filter As String
    Private _IncludeSubFolders As Boolean
    Private _FileSystemWatcher As New FileSystemWatcher
    Private _Files As List(Of FileIOCommon)
    Private _FileCount As Long

    Public ReadOnly Property FileCount As Long
        Get
            Return _FileCount
        End Get
    End Property

    Public ReadOnly Property PathName As String
        Get
            Return _PathName
        End Get
    End Property

    Public ReadOnly Property Filter As String
        Get
            Return _Filter
        End Get
    End Property

    Public ReadOnly Property Files As List(Of FileIOCommon)
        Get
            Return _Files
        End Get
    End Property

    <PermissionSet(SecurityAction.Demand, Name:="FullTrust")> _
    Public Sub WatchForNewFiles(pathName As String, filter As String, includeSubFolders As Boolean)

        _PathName = pathName
        _Filter = filter
        _IncludeSubFolders = includeSubFolders

        ' Create a new FileSystemWatcher and set its properties.
        _FileSystemWatcher.Path = _PathName
        _FileSystemWatcher.Filter = _Filter
        _FileSystemWatcher.IncludeSubdirectories = _IncludeSubFolders

        ' Watch for changes in LastAccess and LastWrite times, and
        ' the renaming of files or directories. 
        _FileSystemWatcher.NotifyFilter = (NotifyFilters.LastAccess Or NotifyFilters.LastWrite Or NotifyFilters.FileName Or NotifyFilters.DirectoryName)

        ' Add event handlers.
        AddHandler _FileSystemWatcher.Created, AddressOf OnChanged
        AddHandler _FileSystemWatcher.Changed, AddressOf OnChanged

        ' Begin watching.
        _FileSystemWatcher.EnableRaisingEvents = True

        Debug.Print("watcher enabled: " & _FileSystemWatcher.Path & _FileSystemWatcher.Filter)

    End Sub

    ' Define the event handlers.
    Private Sub OnChanged(source As Object, e As FileSystemEventArgs)

        _FileCount += 1
        ' Specify what is done when a file is changed, created, or deleted.
        Debug.Print("File: " & e.FullPath & " " & e.ChangeType)
    End Sub

End Class