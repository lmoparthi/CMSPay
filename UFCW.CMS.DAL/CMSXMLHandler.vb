Imports System.IO
Imports System.Text
Imports System.Xml.Serialization

Public Module CMSXMLHandler

    Private _ToAndFromDSSyncLock As New Object
    Private _ToAndFromDBSyncLock As New Object

    ''Private Shared _TraceCaching As New BooleanSwitch("TraceCaching", "Trace Switch in App.Config")

    Public Function ToAndFromDataset(ByVal uniqueThreadIdentifier As String, ByVal tablename As String, ByVal columnname As String, ByVal spname As String, Optional whereClause As String = "") As DataSet

        Dim DT As DataTable
        Dim LastUpDateTime As Date
        Dim SQLCall As New StringBuilder

        SyncLock _ToAndFromDBSyncLock

            Try

                SQLCall.Append("SELECT MAX( " & columnname & " )" & " AS MAXDATE,COUNT(*) AS NUMBEROFROWS FROM " & tablename & " " & whereClause & " FOR READ ONLY WITH UR")

                DT = CMSDALFDBMD.RUNIMMEDIATESELECT(SQLCall.ToString)

#If TRACE Then
                If CInt(_TraceCaching.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & spname & vbTab & "SQL Finished: " & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & Generals.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If

                If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
                    LastUpDateTime = CType(DT.Rows(0)("MAXDATE"), Date)
                    If LastUpDateTime.ToString = "1/1/0001 12:00:00 AM" Then
                        LastUpDateTime = CDate("2006/01/01 12:00:00 AM")
                    End If
                End If

                Return ToAndFromDataset(uniqueThreadIdentifier, tablename, LastUpDateTime, CType(DT.Rows(0)("NUMBEROFROWS"), Integer), spname, False, Nothing)

            Catch ex As Exception
                Throw
            End Try

        End SyncLock


    End Function

    Public Function ToAndFromDataset(ByVal uniqueThreadIdentifier As String, ByVal tablename As String, ByVal columnname As String, ByVal spname As String, dateOnly As Boolean, Optional whereClause As String = "") As DataSet
        Dim DT As DataTable
        Dim LastUpDateTime As Date
        Dim SQLCall As New StringBuilder

        SyncLock _ToAndFromDBSyncLock

            Try

                SQLCall.Append("SELECT MAX( " & columnname & " )" & " AS MAXDATE,COUNT(*) AS NUMBEROFROWS FROM " & tablename & " " & whereClause & " FOR READ ONLY WITH UR")

                DT = CMSDALFDBMD.RUNIMMEDIATESELECT(SQLCall.ToString)

#If TRACE Then
                If CInt(_TraceCaching.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & spname & vbTab & "SQL Finished: " & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & Generals.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If

                If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
                    LastUpDateTime = CType(DT.Rows(0)("MAXDATE"), Date)
                    If LastUpDateTime.ToString = "1/1/0001 12:00:00 AM" Then
                        LastUpDateTime = CDate("2006/01/01 12:00:00 AM")
                    End If
                End If

                Return ToAndFromDataset(uniqueThreadIdentifier, tablename, LastUpDateTime, CType(DT.Rows(0)("NUMBEROFROWS"), Integer), spname, dateOnly, Nothing)

            Catch ex As Exception
                Throw
            End Try
        End SyncLock


    End Function

    Public Function ToAndFromDataset(ByVal uniqueThreadIdentifier As String, ByVal tablename As String, ByVal columnname As String, ByVal spname As String, dateOnly As Boolean, whereClause As String, Optional forceCreate As Boolean = False) As DataSet

        Dim DT As DataTable
        Dim LastUpDateTime As Date
        Dim SQLCall As New StringBuilder

        Try

            SQLCall.Append("SELECT MAX( " & columnname & " )" & " AS MAXDATE,COUNT(*) AS NUMBEROFROWS FROM " & tablename & " " & whereClause & " FOR READ ONLY WITH UR")

            DT = CMSDALFDBMD.RUNIMMEDIATESELECT(SQLCall.ToString)

#If TRACE Then
                If CInt(_TraceCaching.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & spname & vbTab & "SQL Finished: " & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & Generals.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If

            If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
                LastUpDateTime = CType(DT.Rows(0)("MAXDATE"), Date)
                If LastUpDateTime.ToString = "1/1/0001 12:00:00 AM" Then
                    LastUpDateTime = CDate("2006/01/01 12:00:00 AM")
                End If
            End If

            Return ToAndFromDataset(uniqueThreadIdentifier, tablename, LastUpDateTime, CType(DT.Rows(0)("NUMBEROFROWS"), Integer), spname, dateOnly, forceCreate)

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Function ToAndFromDataset(ByVal uniqueThreadIdentifier As String, ByVal tablename As String, ByVal lastUpDateTime As Date, ByVal rowCount As Integer, ByVal spname As String, dateOnly As Boolean, Optional ForceCreate As Boolean = False) As DataSet

        'if the contents of PUBLISH_HISTORY are used as the XML Creation trigger, the test is limited to only ensuring the Caching XML was created on or after the specified publish date by providing dateOnly = true

        SyncLock _ToAndFromDSSyncLock

            Dim XMLFilename As String

            XMLFilename = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & spname & ".xml"

#If TRACE Then
            If CInt(_TraceCaching.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & spname & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & Generals.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If

            Dim XMLSerializer As XmlSerializer
            Dim FileStream As FileStream

            Dim ResultDataSet As New DataSet

            Try

                'Prepare existing xml file for modification
                If File.Exists(XMLFilename) AndAlso (File.GetAttributes(XMLFilename) And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
                    File.SetAttributes(XMLFilename, FileAttributes.Normal)
                End If

                'Check if data has been modified since offline xml datastore was created, if database has newer, recreate xml file
                ' Less than zero - t1 is earlier than t2. (File does need to be recreated
                ' Zero -  t1 is the same as t2. (File does not need to be recreated
                ' Greater than zero -  t1 is later than t2. (File does not need to be recreated

                If File.Exists(XMLFilename) AndAlso (DateTime.Compare(File.GetLastWriteTime(XMLFilename), lastUpDateTime) > 0) Then

                    XMLSerializer = New XmlSerializer(ResultDataSet.GetType)

                    For X As Integer = 1 To 10

                        Try

                            FileStream = New FileStream(XMLFilename, FileMode.Open, FileAccess.Read, FileShare.None)

                            Exit For

                        Catch ex As Exception

#If TRACE Then
                            If CInt(_TraceCaching.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & spname & vbTab & "XML Busy: " & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & Generals.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If
                            System.Threading.Thread.Sleep(1000)

                        End Try

                    Next
                    '' To read the file

                    Try
                        If ForceCreate Then Throw New ApplicationException

                        '' Create the object from the xml file
#If TRACE Then
                        If CInt(_TraceCaching.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & spname & vbTab & "XML Load Start: " & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & Generals.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If

                        ResultDataSet = CType(XMLSerializer.Deserialize(FileStream), DataSet)

#If TRACE Then
                        If CInt(_TraceCaching.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & spname & vbTab & "XML Load End: " & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & Generals.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If

                        File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)

                    Catch ex As Exception ' invalid xml file

#If TRACE Then
                        If CInt(_TraceCaching.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & spname & vbTab & "XML Corrupt: " & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & Generals.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If

                        If FileStream IsNot Nothing Then FileStream.Close()

                        If File.Exists(XMLFilename) AndAlso (File.GetAttributes(XMLFilename) And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
                            File.SetAttributes(XMLFilename, FileAttributes.Normal)
                        End If

                        File.Delete(XMLFilename)

                        ResultDataSet = New DataSet
                    Finally

                    End Try

                End If

                'check number of rows stored in xml matches table
                If ResultDataSet.Tables.Count = 0 OrElse (ResultDataSet.Tables(0).Rows.Count <> rowCount AndAlso Not dateOnly) Then
                    If File.Exists(XMLFilename) Then File.SetAttributes(XMLFilename, FileAttributes.Normal)

                    If ResultDataSet.Tables.Count = 0 Then
#If TRACE Then
                        If CInt(_TraceCaching.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & spname & vbTab & "XML Build: " & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & Generals.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If
                    ElseIf ResultDataSet.Tables(0).Rows.Count <> rowCount Then
#If TRACE Then
                        If CInt(_TraceCaching.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & spname & vbTab & "XML Rebuild: XML (" & ResultDataSet.Tables(0).Rows.Count.ToString & ") <> SQL (" & DT.Rows(0)("NUMBEROFROWS").ToString & ") " & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & Generals.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If
                    End If

                    Return New DataSet
                Else
#If TRACE Then
                    If CInt(_TraceCaching.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & spname & vbTab & "XML Valid: " & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & Generals.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If
                    Return ResultDataSet
                End If

            Catch ex As Exception

                Throw

            Finally

                If ResultDataSet IsNot Nothing Then ResultDataSet.Dispose()
                ResultDataSet = Nothing

                If FileStream IsNot Nothing Then FileStream.Close()
                FileStream = Nothing

                XMLSerializer = Nothing

#If TRACE Then
                If CInt(_TraceCaching.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & spname & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & Generals.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If
            End Try

        End SyncLock

    End Function

    Public Function CreateDataSetFromXML(ByVal xmlFile As String) As DataSet

        'load xml into a dataset to use here

        'open the xml file so we can use it to fill the dataset
        Try
            Using FileStream As New FileStream(System.Windows.Forms.Application.StartupPath & "\" & xmlFile, FileMode.Open, FileAccess.Read)

                Using DS As New DataSet
                    'fill the dataset
                    DS.ReadXml(FileStream)

                    If Not DS.Tables("Column").Columns.Contains("Visible") Then DS.Tables("Column").Columns.Add("Visible")
                    If Not DS.Tables("Column").Columns.Contains("FormatIsRegEx") Then DS.Tables("Column").Columns.Add("FormatIsRegEx")
                    If Not DS.Tables("Column").Columns.Contains("WordWrap") Then DS.Tables("Column").Columns.Add("WordWrap")
                    If Not DS.Tables("Column").Columns.Contains("ReadOnly") Then DS.Tables("Column").Columns.Add("ReadOnly", System.Type.GetType("System.Boolean"))
                    If Not DS.Tables("Column").Columns.Contains("MinimumCharWidth") Then DS.Tables("Column").Columns.Add("MinimumCharWidth")
                    If Not DS.Tables("Column").Columns.Contains("MaximumCharWidth") Then DS.Tables("Column").Columns.Add("MaximumCharWidth")

                    Return DS

                End Using
            End Using

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Function

End Module
