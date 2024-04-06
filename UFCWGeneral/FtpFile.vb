Imports System.Collections.Generic
Imports System.Net
Imports System.IO
Imports System.Text.RegularExpressions



#Region "FTP file info class"
''' <summary>
''' Represents a file or directory entry from an FTP listing
''' </summary>
''' <remarks>
''' This class is used to parse the results from a detailed
''' directory list from FTP. It supports most formats of
''' 
''' v1.1 fixed bug in Fullname/path
''' 
''' v1.2 fixed bug to handle blank size field
''' </remarks>
Public Class FTPfileInfo
    'Stores extended info about FTP file

#Region "Properties"
    Public ReadOnly Property FullName() As String
        Get
            Return Path & Filename
        End Get
    End Property
    Public ReadOnly Property Filename() As String
        Get
            Return _Filename
        End Get
    End Property
    ''' <summary>
    ''' Path of the file (always ends in /)
    ''' </summary>
    ''' <remarks>
    ''' 1.1: Modifed to ensure always ends in / - with thanks to jfransella for pointing this out
    ''' </remarks>
    Public ReadOnly Property Path() As String
        Get
            Return _Path & If(_Path.EndsWith("/"), "", "/")
        End Get
    End Property
    Public ReadOnly Property FileType() As DirectoryEntryType
        Get
            Return _FileType
        End Get
    End Property
    Public ReadOnly Property Size() As Long
        Get
            Return _Size
        End Get
    End Property
    Public Property FileDateTime() As Date
        Get
            Return _FileDateTime
        End Get
        Set(ByVal value As Date)
            _FileDateTime = value
        End Set
    End Property
    Public ReadOnly Property Permission() As String
        Get
            Return _Permission
        End Get
    End Property
    Public ReadOnly Property Extension() As String
        Get
            Dim I As Integer = Me.Filename.LastIndexOf(".")
            If I >= 0 And I < (Me.Filename.Length - 1) Then
                Return Me.Filename.Substring(I + 1)
            Else
                Return ""
            End If
        End Get
    End Property
    Public ReadOnly Property NameOnly() As String
        Get
            Dim I As Integer = Me.Filename.LastIndexOf(".")
            If I > 0 Then
                Return Me.Filename.Substring(0, I)
            Else
                Return Me.Filename
            End If
        End Get
    End Property

    Private _Filename As String
    Private _Path As String
    Private _FileType As DirectoryEntryType
    Private _Size As Long
    Private _FileDateTime As Date
    Private _Permission As String

#End Region

    ''' <summary>
    ''' Constructor taking a directory listing line and path
    ''' </summary>
    ''' <param name="line">The line returned from the detailed directory list</param>
    ''' <param name="path">Path of the directory</param>
    ''' <remarks></remarks>
    Sub New(ByVal line As String, ByVal path As String)
        'parse line
        Dim M As Match = GetMatchingRegex(line)

        If M Is Nothing Then
            'failed
            Throw New ApplicationException("Unable to parse line: " & line)
        Else
            _Filename = M.Groups("name").Value
            _Path = path

            'v1.2 - fix to handle null size fields (copied from C# version)
            Int64.TryParse(M.Groups("size").Value, _Size)

            _Permission = M.Groups("permission").Value
            Dim Dir As String = M.Groups("dir").Value
            If (Dir <> "" AndAlso Dir <> "-") Then
                _FileType = DirectoryEntryType.Directory
            Else
                _FileType = DirectoryEntryType.File
            End If

            _FileDateTime = Nothing

            Dim Testtimestamp As String

            Try

                Testtimestamp = M.Groups("timestamp").ToString.Replace("  ", " ")

                If Testtimestamp.Length = 12 AndAlso Testtimestamp.Contains(":") Then
                    _FileDateTime = DateTime.ParseExact(Testtimestamp, "MMM dd HH:mm", System.Globalization.CultureInfo.CurrentCulture)
                ElseIf Testtimestamp.Length = 11 AndAlso _FileType = DirectoryEntryType.Directory Then
                    _FileDateTime = DateTime.ParseExact(Testtimestamp, "MMM dd yyyy", System.Globalization.CultureInfo.CurrentCulture)
                Else
                    _FileDateTime = Date.Parse(Testtimestamp)
                End If
            Catch ex As Exception
                Try
                    _FileDateTime = DateTime.ParseExact(Testtimestamp, "MMM dd HH:mm", System.Globalization.CultureInfo.CurrentCulture)
                Catch ex2 As Exception
                End Try
            End Try
        End If

    End Sub

    Private Function GetMatchingRegex(ByVal line As String) As Match
        Dim Rx As Regex, M As Match

        Try

            For I As Integer = 0 To _ParseFormats.Length - 1
                Rx = New Regex(_ParseFormats(I))
                M = Rx.Match(line)
                If M.Success Then Return M
            Next

            Return Nothing

        Catch ex As Exception
            Throw
        End Try

    End Function

#Region "Regular expressions for parsing LIST results"
    ''' <summary>
    ''' List of REGEX formats for different FTP server listing formats
    ''' </summary>
    ''' <remarks>
    ''' The first three are various UNIX/LINUX formats, fourth is for MS FTP
    ''' in detailed mode and the last for MS FTP in 'DOS' mode.
    ''' I wish VB.NET had support for Const arrays like C# but there you go
    ''' </remarks>
    Private Shared _ParseFormats As String() = {
                                                "(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})(\s+)(?<size>(\d+))(?<owner>(\s+\-*\w*\-*){1})(?<group>(\s+\-*\w*\-*){1})(\s+)(?<size2>(\d+))(\s+)(?<timestamp>\d{1,2}\/\d{1,2}\/\d{2,4}\s\d{2}\:\d{2}\:\d{2}\sAM|PM)\s+(?<name>.+)",
                                                "(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})(\s+)(?<size>(\d+))(?<owner>(\s+\-*\w*\-*){1})(?<group>(\s+\-*\w*\-*){1})(\s+)(?<size2>(\d+))(\s+)(?<timestamp>\d{1,2}\/\d{1,2}\/\d{2,4}\s\d{1,2}\:\d{2}\:\d{2}\s(AM|PM))\s+(?<name>.+)",
                                                "(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})(\s+)(?<size>(\d+))(\s+)(?<owner>(\-*\w+\-*\s+\-*\w+\-*))(\s+)(?<size2>(\d+))\s+(?<timestamp>\w+\s+\d+\s+\d{2}:\d{2})\s+(?<name>.+)",
                                                "(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})\s+\d+\s+\w+\s+\w+\s+(?<size>\d+)\s+(?<timestamp>\w+\s+\d+\s+\d{4}\s+\d{1,2}:\d{2})\s+(?<name>.+)",
                                                "(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})\s+\d+\s+\w+\s+\w+\s+(?<size>\d+)\s+(?<timestamp>\w+\s+\d+\s+\d{4})\s+(?<name>.+)",
                                                "(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})\s+\d+\s+\d+\s+(?<size>\d+)\s+(?<timestamp>\w+\s+\d+\s+\d{4}\s+\d{1,2}:\d{2})\s+(?<name>.+)",
                                                "(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})\s+\d+\s+\d+\s+(?<size>\d+)\s+(?<timestamp>\w+\s+\d+\s+\d{4})\s+(?<name>.+)",
                                                "(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})\s+\d+\s+\w+\s+\w+\s+(?<size>\d+)\s+(?<timestamp>\w+\s+\d+\s+\d{1,2}:\d{2})\s+(?<name>.+)",
                                                "(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})\s+\d+\s+\d+\s+(?<size>\d+)\s+(?<timestamp>\w+\s+\d+\s+\d{1,2}:\d{2})\s+(?<name>.+)",
                                                "(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})(\s+)(?<size>(\d+))(\s+)(?<owner>(\w+\s\w+))(\s+)(?<size2>(\d+))\s+(?<timestamp>\w+\s+\d+\s+\d{2}:\d{2})\s+(?<name>.+)",
                                                "(?<timestamp>\d{2}\-\d{2}\-\d{2}\s+\d{2}:\d{2}[Aa|Pp][mM])\s+(?<dir>\<\w+\>){0,1}(?<size>\d+){0,1}\s+(?<name>.+)",
                                                "(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})(\s+)(?<size>(\d+))(?<owner>(\s+\-*\w*\-*){2})(\s+)(?<size2>(\d+))(\s+)(?<timestamp>\w+\s+\d+\s+\d{4})\s+(?<name>.+)",
                                                "(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})(\s+)(?<size>(\d+))(?<owner>(\s+\-*\w*\-*){2})(\s+)(?<size2>(\d+))(\s+)(?<timestamp>\d{1,2}\/\d{1,2}\/\d{2,4}\s\d{1,2}\:\d{2}\:\d{2}\s(AM|PM))"
                                                } ',
    '"(?<Volume>.{7})(?<Unit>.{4})(?<Referred>.{13})(?<Ext>.{3})(?<Used>.{5})(?<Recfm>.{4})(?<Lrecl>.{8})(?<BlkSz>.{6})(?<Dsorg>.{6})(?<Dname>.+)"}
#End Region
End Class
''' <summary>
''' Identifies entry as either File or Directory
''' </summary>
Public Enum DirectoryEntryType
    File
    Directory
End Enum

#End Region


