Imports System.IO
Imports System.Xml.Serialization

Public Class TestForm

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        OpenFileDialog1.ShowDialog()

    End Sub

    Private Sub OpenFileDialog1_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.SafeFileName.ToString()
        TestDataGrid.Name = Replace(TextBox1.Text.ToString, ".XML", "", , , CompareMethod.Text)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadStyleButton.Click

        TestDataGrid.SetTableStyle(Replace(TextBox1.Text.ToString, ".XML", "", , , CompareMethod.Text))
    End Sub

    Private Sub DefaultButton_Click(sender As Object, e As EventArgs) Handles DefaultButton.Click

        TestDataGrid.Name = "ProcedureCodesDataGrid"
        TestDataGrid.AllowMultiSelect = CheckBox1.Checked
        TestDataGrid.DataSource = LoadProcedureValues()
        TestDataGrid.DataMember = "PROCEDURE_VALUES"
        TestDataGrid.SetTableStyle("ProcedureCodesDataGrid")

    End Sub
    Private Shared Function LoadProcedureValues() As DataSet

        Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & "FDBMD.RETRIEVE_ALL_PROCEDURE_VALUES" & ".xml"
        Dim FStream As FileStream

        Try


            Dim XMLDS As DataSet = XMLHandler.ToandFromDataset(XMLFilename)

            Return XMLDS

        Catch ex As Exception
            Throw
        Finally

            If FStream IsNot Nothing Then
                FStream.Close()
                FStream.Dispose()
            End If

            FStream = Nothing

        End Try
    End Function

End Class
Public Class XMLHandler

    Public Shared Function ToandFromDataset(ByVal xmlFilename As String) As DataSet


        Dim Serializer As XmlSerializer
        Dim FileStream As FileStream
        Dim ResultDS As DataSet

        Try
            ResultDS = New DataSet

            If File.Exists(xmlFilename) Then

                Serializer = New XmlSerializer(ResultDS.GetType)

                For x As Integer = 1 To 10

                    Try

                        FileStream = New FileStream(xmlFilename, FileMode.Open, FileAccess.Read, FileShare.None)

                        Exit For

                    Catch ex As Exception

                        System.Threading.Thread.Sleep(1000)

                    End Try

                Next
                '' To read the file

                Try
                    '' Create the object from the xml file
                    ResultDS = CType(Serializer.Deserialize(FileStream), DataSet)
                    FileStream.Close()
                    File.SetAttributes(xmlFilename, FileAttributes.ReadOnly)

                Catch ex As Exception ' invalid xml file

                    FileStream.Close()

                    If File.Exists(xmlFilename) AndAlso (File.GetAttributes(xmlFilename) And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
                        File.SetAttributes(xmlFilename, FileAttributes.Normal)
                    End If

                    File.Delete(xmlFilename)

                    ResultDS = New DataSet
                Finally

                    FileStream = Nothing
                    Serializer = Nothing

                End Try

            End If

            Return ResultDS

        Catch ex As Exception

            Throw

        Finally

            If ResultDS IsNot Nothing Then ResultDS.Dispose()
            ResultDS = Nothing

            If FileStream IsNot Nothing Then FileStream.Close()
            FileStream = Nothing

            Serializer = Nothing

        End Try

    End Function

End Class
