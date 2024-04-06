Option Strict On

Imports System.Threading
Imports System.Threading.Tasks

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.ProcessorEngine
''' Class	 : CMS.ProcessorEngine.Actions
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class represents a collection of Action objects
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/12/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public Class Actions
    Inherits CollectionBase
    Implements ICloneable

    Private Shared _TraceCloning As New TraceSwitch("TraceCloning", "Trace Switch in App.Config", "0")

    Default Public Property Item(ByVal index As Integer) As Action
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' This property is used for 'looping' purposes (ex. action(i) )
        ' </summary>
        ' <param name="index"></param>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/14/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return DirectCast(Me.List(index), Action)
        End Get
        Set(ByVal value As Action)
            Me.List(index) = value
        End Set
    End Property

    Public Sub New()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' default constructor
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/12/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        MyBase.New()
    End Sub

    Public Sub Add(ByVal action As IAction)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Adds an action to this list of actions
        ' </summary>
        ' <param name="act"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/12/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Me.List.Add(action)
    End Sub

#Region "Clone"
    Public Function DeepCopy() As Actions

        Dim BeginTime As Date = UFCWGeneral.NowDate
        Dim ActionsClone As Actions

        Try

            ActionsClone = Me.ShallowCopy

            Parallel.ForEach(Me.Cast(Of Object), Sub(item)
                                                     ActionsClone.Add(CType(item, Action).DeepCopy)
                                                 End Sub)

            Return ActionsClone

        Catch ex As Exception
            Throw
        Finally
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
        End Try
    End Function

    Public Function ShallowCopy() As Actions
        Dim BeginTime As Date = UFCWGeneral.NowDate

        Try

            Return New Actions

        Catch
            Throw
        Finally
        End Try
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone

        Try

            Return DirectCast(CloneHelper.Clone(Me), ICloneable)

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Function

#End Region

End Class