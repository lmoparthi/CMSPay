Imports System.Threading


''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.AlertManager
''' Class	 : CMS.AlertManager.IAlert
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This calls represents an alert
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	5/31/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public NotInheritable Class Alert
    Implements ICloneable

    Private Shared _TraceCloning As New TraceSwitch("TraceCloning", "Trace Switch in App.Config", "0")

    Delegate Sub AddAlert(ByVal alrt As Alert)
    Delegate Sub AddAndBuildAlert(ByVal severity As SeverityTypes, lineNumber As Short, alertMessage As String, category As CategoryTypes, tag As Object)

    Private _Severity As SeverityTypes
    Private _LineNumber As Short
    Private _AlertMessage As String
    Private _CategoryType As CategoryTypes
    Private _Tag As Object

    Public ReadOnly Property Severity() As SeverityTypes
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets the severity
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/31/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Severity
        End Get
    End Property

    Public Property LineNumber() As Short
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the linenumber
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/31/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _LineNumber
        End Get
        Set(ByVal value As Short)
            _LineNumber = value
        End Set
    End Property

    Property AlertMessage() As String
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the Alert Message
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/31/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _AlertMessage
        End Get
        Set(ByVal value As String)
            _AlertMessage = Value
        End Set
    End Property

    Public Property Tag() As Object
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the Tag of the alert
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/31/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Tag
        End Get
        Set(ByVal value As Object)
            _Tag = Value
        End Set
    End Property

    Public Property Category() As CategoryTypes
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the category of the alert
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/31/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _CategoryType
        End Get
        Set(ByVal value As CategoryTypes)
            _CategoryType = Value
        End Set
    End Property

    Public Sub SetSeverity(ByVal severityLevel As Integer)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Set the severity
        ' </summary>
        ' <param name="severityLevel"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/31/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        If severityLevel > 30 Then
            Throw New ArgumentOutOfRangeException("severityLevel", "Severity Level Must be less than or equal to 30")
        End If
        If severityLevel < 10 Then
            _Severity = SeverityTypes.Information
        ElseIf severityLevel <= 20 Then
            _Severity = SeverityTypes.Warning
        ElseIf severityLevel <= 30 Then
            _Severity = SeverityTypes.Critical
        End If
    End Sub

#Region "Clone"

    Public Function DeepCopy() As Alert
        Dim BeginTime As Date = UFCWGeneral.NowDate

        Try
            Return DirectCast(Me.Clone, Alert)

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
#End If
        End Try
    End Function

    Public Function ShallowCopy() As Alert
        Dim BeginTime As Date = UFCWGeneral.NowDate

        Try

            Return DirectCast(Me.MemberwiseClone(), Alert)
        Catch
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
#End If
        End Try

    End Function

    Public Function Clone() As Object Implements ICloneable.Clone

        Dim MS As IO.MemoryStream
        Dim BF As Runtime.Serialization.Formatters.Binary.BinaryFormatter

        Dim BeginTime As Date = UFCWGeneral.NowDate

        Try

            'ms = New IO.MemoryStream()
            'bf = New Runtime.Serialization.Formatters.Binary.BinaryFormatter(Nothing, New Runtime.Serialization.StreamingContext(Runtime.Serialization.StreamingContextStates.Clone))

            'bf.Serialize(ms, Me)

            'ms.Seek(0, SeekOrigin.Begin)

            'Return DirectCast(bf.Deserialize(ms), Alert)

            Return DirectCast(CloneHelper.Clone(Me), Alert)

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
#End If

            If MS IsNot Nothing Then MS.Close()
            MS = Nothing
            BF = Nothing
        End Try

    End Function

#End Region
End Class
Public Enum SeverityTypes
    Information = 10
    Warning = 20
    Critical = 30
End Enum

Public Enum CategoryTypes
    Accumulator
    SystemException
    RepriceNeeded
    Denied
    ProviderWriteOff
    ReasonCode
    Other
    NoAllowanceRemaining
End Enum
