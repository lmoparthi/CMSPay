Option Explicit On
Option Strict On
Imports System
Imports System.Runtime.Serialization
Imports System.Threading
Imports System.Threading.Tasks

''' -----------------------------------------------------------------------------
''' Project	 : Accumulator
''' Class	 : CMS.MemberAccumulatorCollection
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' Stores all the Accumulators for a member
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	12/9/2005	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()>
Public Class MemberAccumulatorCollection
    Inherits Hashtable

    Private Shared _TraceCloning As New TraceSwitch("TraceCloning", "Trace Switch in App.Config", "0")

    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.New(info, context)
    End Sub

    Protected Overrides Sub Finalize()
        Me.Clear()
        MyBase.Finalize()
    End Sub

    Sub New()

    End Sub
    Public Function ShallowCopy() As MemberAccumulatorCollection

        Return New MemberAccumulatorCollection

    End Function

    Public Function DeepCopy() As MemberAccumulatorCollection
        Dim MemberAccumulatorCollectionClone As MemberAccumulatorCollection
        Dim BeginTime As Date = UFCWGeneral.NowDate

        Try

            MemberAccumulatorCollectionClone = Me.ShallowCopy()

            Parallel.ForEach(Me.Cast(Of DictionaryEntry), Sub(theItem)
                                                              MemberAccumulatorCollectionClone.Add(theItem.Key, CType(theItem.Value, MemberAccumulator).DeepCopy)
                                                          End Sub)

            'For Each Item As DictionaryEntry In Me

            '    MemberAccumulatorCollectionClone.Add(Item.Key, CType(Item.Value, MemberAccumulator).DeepCopy)

            'Next

            Return MemberAccumulatorCollectionClone

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
#End If
        End Try

    End Function

End Class