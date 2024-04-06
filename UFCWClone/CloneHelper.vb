
Option Infer On
Option Strict On

Imports System.IO
Imports System.Threading.Tasks

<Serializable>
Public NotInheritable Class CloneHelper

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private Sub New()

    End Sub

    Public Shared Function DeepCopy(ByVal hashtableToClone As Hashtable, hashtableClone As Hashtable) As Hashtable
        Dim BeginTime As Date = UFCWGeneral.NowDate

        Try

            Parallel.ForEach(hashtableToClone.Cast(Of Object), Sub(item)
                                                               End Sub)

            Return hashtableClone

        Catch ex As Exception
            Throw
        Finally
            'If _TraceSwitch.Enabled Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & Generals.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", MethodBase.GetCurrentMethod().DeclaringType.ToString)
        End Try
    End Function

    Public Shared Function DeepCopy(ByVal tableToClone As DataTable) As DataTable
        Return DeepCopy(tableToClone, Nothing)
    End Function

    Public Shared Function DeepCopy(ByVal cloneDT As DataTable, memoryStreamSize As Integer?) As DataTable

        If cloneDT Is Nothing Then Return Nothing

        Dim ClonedTable As DataTable
        Dim BeginTime As Date = UFCWGeneral.NowDate

        Try

            ClonedTable = cloneDT.Clone
            For Each DC As DataColumn In ClonedTable.Columns
                If DC.Expression.Length > 0 Then DC.Expression = ""
            Next

            ClonedTable.BeginLoadData()
            ClonedTable.Load(cloneDT.CreateDataReader)
            ClonedTable.EndLoadData()

            For Each DC As DataColumn In cloneDT.Columns
                If DC.Expression.Length > 0 Then ClonedTable.Columns(DC.ColumnName).Expression = DC.Expression
            Next

            Return ClonedTable

        Catch ex As Exception
            Throw
        Finally

            'If _TraceSwitch.Enabled Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & Generals.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", MethodBase.GetCurrentMethod().DeclaringType.ToString)

            ClonedTable = Nothing

        End Try
    End Function

    Public Shared Function Clone(ByVal objectToClone As Object) As Object

        Try
            Return Clone(objectToClone, Nothing)
        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function Clone(ByVal objectToClone As Object, memoryStreamSize As Integer?) As Object

        Dim BinFormatter As Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim BeginTime As Date = UFCWGeneral.NowDate

        Try

            BinFormatter = New Runtime.Serialization.Formatters.Binary.BinaryFormatter(Nothing, New Runtime.Serialization.StreamingContext(Runtime.Serialization.StreamingContextStates.Clone))

            If memoryStreamSize Is Nothing Then
                Using MemoryStream As New IO.MemoryStream
                    BinFormatter.Serialize(MemoryStream, objectToClone)
                    MemoryStream.Seek(0, SeekOrigin.Begin)

                    Return BinFormatter.Deserialize(MemoryStream) ', Nothing) unsafedeserialize is slower
                End Using
            Else
                Using MemoryStream As New IO.MemoryStream(CInt(memoryStreamSize))
                    BinFormatter.Serialize(MemoryStream, objectToClone)
                    MemoryStream.Seek(0, SeekOrigin.Begin)

                    Return BinFormatter.Deserialize(MemoryStream) ', Nothing) unsafedeserialize is slower
                End Using
            End If

        Catch ex As Exception
            Throw
        Finally

            'If _TraceSwitch.Enabled Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & Generals.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", MethodBase.GetCurrentMethod().DeclaringType.ToString)

            BinFormatter = Nothing

        End Try
    End Function
    Public Shared Function DeepCopy(ByVal collectionToClone As CollectionBase) As CollectionBase
        ' This copies references types by just copying the pointer, so to break any connection back to those object the objects need to be recreated.

        Dim BeginTime As Date = UFCWGeneral.NowDate
        Dim ParallelCollectionClone As New BaseCloneHelperCollection

        Try

            Parallel.ForEach(DirectCast(collectionToClone, CollectionBase).Cast(Of Object), Sub(item)
                                                                                                ParallelCollectionClone.Add(item)
                                                                                            End Sub)

            Return ParallelCollectionClone

        Catch ex As Exception
            Throw
        Finally
            'If _TraceSwitch.Enabled Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & Generals.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", MethodBase.GetCurrentMethod().DeclaringType.ToString)
        End Try
    End Function

    Public Shared Function ShallowCopy(ByVal collectionToClone As CollectionBase) As CollectionBase
        ' This copies references types by just copying the pointer, so to break any connection back to those object the objects need to be recreated.

        Dim BeginTime As Date = UFCWGeneral.NowDate
        Dim ParallelCollectionClone As New BaseCloneHelperCollection

        Try

            Return ParallelCollectionClone

        Catch ex As Exception
            Throw
        Finally
            'If _TraceSwitch.Enabled Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & Generals.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", MethodBase.GetCurrentMethod().DeclaringType.ToString)
        End Try
    End Function

End Class

Public NotInheritable Class BaseCloneHelperCollection
    Inherits CollectionBase

    Public Sub New()
        MyBase.New()
    End Sub

    Default Public Property Item(ByVal index As Integer) As Object
        Get
            Return Me.List(index)
        End Get
        Set(ByVal value As Object)
            Me.List(index) = value
        End Set
    End Property

    Public Sub Add(ByVal item As Object)
        Me.List.Add(CloneHelper.Clone(item))
    End Sub
End Class


