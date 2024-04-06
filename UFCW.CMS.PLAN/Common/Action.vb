
Imports System.Threading

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.Action
'''
''' -----------------------------------------------------------------------------
''' <summary>
'''
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/26/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public MustInherit Class Action
    Implements IAction, ICloneable

    Protected Shared _TraceCloning As New TraceSwitch("TraceCloning", "Trace Switch in App.Config", "0")

#Region "Protected Variables"
    Protected _Description As String
    Protected _ValueType As ActionValueTypes
    Protected _Value As Decimal
    Protected _ClaimID As Integer
    Protected _LineNumber As Short
    Protected _Name As String
    Protected _ApplyDate As Date?
#End Region

#Region "Methods"
    Protected MustOverride Function Execute(ByVal actionValue As Decimal) As Decimal Implements IAction.Execute
    Protected MustOverride Function Execute(ByVal actionValue As Decimal, ByVal condition As Condition) As Decimal Implements IAction.Execute

    Public Overrides Function ToString() As String Implements IAction.ToString
        Return "Description: " & _Description & " Value Type: " & _ValueType.ToString & " Value: " & _Value.ToString & " ClaimId: " & _ClaimID.ToString & " Line Number: " & _LineNumber.ToString & " Name: " & _Name & " Apply Date: " & _ApplyDate.ToString
    End Function
#End Region

#Region "Properties"
    Public MustOverride ReadOnly Property Description() As String Implements IAction.Description

    Public Property ActionValueType() As ActionValueTypes Implements IAction.ActionValueType
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Set the action value type
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/20/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _ValueType
        End Get
        Set(ByVal value As ActionValueTypes)
            _ValueType = value
        End Set
    End Property

    Public Property ActionValue() As Decimal Implements IAction.ActionValue
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the action value
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/20/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Value
        End Get
        Set(ByVal value As Decimal)
            _Value = value
        End Set
    End Property

    Public Property ClaimID() As Integer Implements IAction.ClaimID
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the claimid for the action to use
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/20/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _ClaimID
        End Get
        Set(ByVal value As Integer)
            _ClaimID = value
        End Set
    End Property

    Public Property LineNumber() As Short Implements IAction.LineNumber
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the line number that this action will correspond to
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/20/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _LineNumber
        End Get
        Set(ByVal value As Short)
            _LineNumber = value
        End Set
    End Property

    Public Property Name() As String Implements IAction.Name
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the name of this action
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/20/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Public Property ApplyDate() As Date?
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the apply date for this action
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/20/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _ApplyDate
        End Get
        Set(ByVal value As Date?)
            _ApplyDate = value
        End Set
    End Property
#End Region

#Region "Clone"
    Public Function DeepCopy() As Action

        Dim BeginTime As Date = UFCWGeneral.NowDate

        Try

            Return Me.ShallowCopy()

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
#End If
        End Try
    End Function

    Public Function ShallowCopy() As Action

        Dim BeginTime As Date = UFCWGeneral.NowDate

        Try

            Return DirectCast(Me.MemberwiseClone(), Action)

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
#End If
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

#Region "Dispose"
    'Protected Overridable Sub Dispose(ByVal disposing As Boolean) Implements IAction.Dispose
    '    If disposing Then
    '        ' Call dispose on any objects referenced by this object
    '    End If
    '    ' Release unmanaged resources
    'End Sub

    'Protected Overridable Sub Dispose() Implements IDisposable.Dispose, IAction.Dispose
    '    Dispose(True)
    '    _disposed = True
    '    ' Take off finalization queue
    '    GC.SuppressFinalize(Me)
    'End Sub

    'Protected Overrides Sub Finalize() Implements IAction.Finalize
    '    Me.Dispose(False)
    'End Sub
#End Region
End Class