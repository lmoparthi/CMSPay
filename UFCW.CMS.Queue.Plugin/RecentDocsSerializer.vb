Imports System.IO
Imports System.Xml.Serialization
Imports System.Runtime.CompilerServices

Public Class RecentDocsSerializer
    <MethodImplAttribute(MethodImplOptions.Synchronized)> _
    Public Shared Function GetRecentDocs(ByVal XMLFile As String) As DataSet
        Dim DS As DataSet
        Dim FI As New FileInfo(XMLFile)
        Dim XmlSer As XmlSerializer
        Dim Stream As FileStream

        Try
            If FI.Exists Then

                XmlSer = New XmlSerializer(GetType(DataSet))
                Stream = New FileStream(XMLFile, FileMode.Open)

                DS = CType(XmlSer.Deserialize(Stream), DataSet)

                Stream.Close()

            End If

        Catch ex As Exception

            DS = Nothing

            Stream.Close()

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

            If FI.Exists = False Then
                DS = New DataSet("RecentDocs")
                Dim dt As DataTable = DS.Tables.Add("RecentDocs")
                'Dim Key(0) As DataColumn

                'dt.Columns.Add("Position", System.Type.GetType("System.Int32"))
                dt.Columns.Add("DocID", System.Type.GetType("System.Int32"))
                dt.Columns.Add("LastAccessed", System.Type.GetType("System.DateTime"))
                'Key(0) = dt.Columns.Add("LastAccessed", System.Type.GetType("System.DateTime"))
                'dt.PrimaryKey = Key
            Else
                XmlSer = New XmlSerializer(GetType(DataSet))
                Stream = New FileStream(XMLFile, FileMode.Open)

                DS = CType(XmlSer.Deserialize(Stream), DataSet)

                Stream.Close()

                Stream = Nothing
                XmlSer = Nothing
            End If

            Return DS
        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Sub SaveRecentDocs(ByVal DS As DataSet, ByVal XMLFile As String)
        Dim XmlSer As XmlSerializer = New XmlSerializer(GetType(DataSet))
        Dim writer As TextWriter = New StreamWriter(XMLFile)

        XmlSer.Serialize(writer, DS)
        writer.Close()

        'Cleanup
        XmlSer = Nothing
        writer = Nothing
    End Sub

End Class
