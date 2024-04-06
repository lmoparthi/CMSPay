Imports System.IO
Imports System.Xml.Serialization
Imports System.Runtime.CompilerServices

Public NotInheritable Class RecentDocsSerializer

    Public Shared Event RecentDocsRefreshAvailable(e As RecentDocsEvent)

    Private Sub New()

    End Sub

    <MethodImplAttribute(MethodImplOptions.Synchronized)> _
    Public Shared Function GetRecentDocs(ByVal xmlFile As String) As DataSet

        Dim DS As DataSet
        Dim FI As New FileInfo(xmlFile)
        Dim XmlSer As XmlSerializer
        Dim Stream As FileStream
        Dim DT As DataTable

        Try
            If FI.Exists Then

                XmlSer = New XmlSerializer(GetType(DataSet))
                Stream = New FileStream(xmlFile, FileMode.Open)

                DS = CType(XmlSer.Deserialize(Stream), DataSet)

                Stream.Close()

            End If

        Catch ex As Exception

            DS = Nothing

            If Stream IsNot Nothing Then Stream.Close()

            Stream = Nothing
            XmlSer = Nothing

            Try
                If FI.Exists Then 'this assumes a corrupted file.
                    FI.Delete()
                End If
            Catch ex2 As Exception

            End Try
        Finally
        End Try


        Try

            If FI.Exists Then
                XmlSer = New XmlSerializer(GetType(DataSet))
                Stream = New FileStream(xmlFile, FileMode.Open)

                DS = CType(XmlSer.Deserialize(Stream), DataSet)

            End If

            If FI.Exists = False OrElse DS.Tables("RecentDocs").Columns.Count < 3 Then
                DS = New DataSet("RecentDocs")
                DT = DS.Tables.Add("RecentDocs")
                DT.Columns.Add("DocID", System.Type.GetType("System.Int32"))
                DT.Columns.Add("PART_SSN", System.Type.GetType("System.Int32"))
                DT.Columns.Add("LastAccessed", System.Type.GetType("System.DateTime"))
            End If


            DS.Tables("RecentDocs").PrimaryKey = New DataColumn() {DS.Tables("RecentDocs").Columns("DocID")}

            Return DS

        Catch ex As Exception
            Throw
        Finally

            If Stream IsNot Nothing Then Stream.Close()
            Stream = Nothing

            XmlSer = Nothing

        End Try

    End Function

    Public Shared Sub SaveRecentDocs(ByVal recentDocsDS As DataSet, ByVal xmlFile As String)
        Dim XmlSer As XmlSerializer
        Dim Writer As TextWriter

        Try
            XmlSer = New XmlSerializer(GetType(DataSet))
            Writer = New StreamWriter(xmlFile)

            XmlSer.Serialize(Writer, recentDocsDS)

        Catch ex As Exception
            Throw
        Finally
            If Writer IsNot Nothing Then Writer.Close()

            RaiseEvent RecentDocsRefreshAvailable(New RecentDocsEvent(recentDocsDS))

            'Cleanup
            XmlSer = Nothing
            Writer = Nothing
        End Try


    End Sub

End Class
Public Class RecentDocsEvent
    Inherits Global.System.EventArgs

    Private Shared _TraceWCF As New BooleanSwitch("TraceWCF", "Trace Switch in App.Config")

    Private _RecentDocsDS As DataSet
    Public ReadOnly Property RecentDocsDS As DataSet
        Get
            Return _RecentDocsDS
        End Get
    End Property
    Public Sub New(ByVal recentDocsDS As DataSet)
        MyBase.New()
        _RecentDocsDS = recentDocsDS
    End Sub

End Class
