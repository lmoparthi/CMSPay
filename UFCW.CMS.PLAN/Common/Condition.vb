
Imports System.Threading

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.ProcessingEngine
''' Class	 : CMS.ProcessorEngine.Condition
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' Represents a rule condition
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/14/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public NotInheritable Class Condition
    Implements ICloneable

#Region "Private Variables"
    Private Shared ReadOnly _TraceCloning As New TraceSwitch("TraceCloning", "Trace Switch in App.Config", "0")

    Private _AccumulatorName As String = ""
    Private _Operand As Decimal
    Private _Direction As DateDirection
    Private _Duration As Integer
    Private _DurationType As DateType
    Private _ConditionID As Integer
    Private _UseInHeadroomCheck As Boolean = False
    Private _RepriceIfExceeded As Boolean = True
    Private _Active As Integer = 1
    Private _Manual As Integer = 0
    Private _Preventive As Integer = 0
    Private _Batch As Integer = 0
    Private _AccumulatorYear As Integer
    Private _AccumulatorStartDate As Date?
    Private _AccumulatorEndDate As Date?
    Private _PublishBatchNbr As Integer
    Private _PlanType As String

#End Region

#Region "Constructors"
    Public Sub New()
        _AccumulatorName = ""
    End Sub
#End Region

#Region "Properties"

    Public Property PlanType() As String
        Get
            Return _PlanType
        End Get
        Set(ByVal value As String)
            _PlanType = value
        End Set
    End Property

    Public Property PublishBatchNbr() As Integer
        Get
            Return _PublishBatchNbr
        End Get
        Set(ByVal value As Integer)
            _PublishBatchNbr = Value
        End Set
    End Property

    Public Property AccumulatorEndDate() As Date?
        Get
            Return _AccumulatorEndDate
        End Get
        Set(ByVal value As Date?)
            _AccumulatorEndDate = value
        End Set
    End Property

    Public Property AccumulatorStartDate() As Date?
        Get
            Return _AccumulatorStartDate
        End Get
        Set(ByVal value As Date?)
            _AccumulatorStartDate = value
        End Set
    End Property

    Public Property RepriceIfExceeded() As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/11/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _RepriceIfExceeded
        End Get
        Set(ByVal value As Boolean)
            _RepriceIfExceeded = value
        End Set
    End Property

    Public Property Active() As Integer
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/11/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Active
        End Get
        Set(ByVal value As Integer)
            _Active = value
        End Set
    End Property
    Public Property Preventive() As Integer
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/11/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Preventive
        End Get
        Set(ByVal value As Integer)
            _Preventive = value
        End Set
    End Property
    Public Property Manual() As Integer
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/11/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Manual
        End Get
        Set(ByVal value As Integer)
            _Manual = value
        End Set
    End Property
    Public Property Batch() As Integer
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/11/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Batch
        End Get
        Set(ByVal value As Integer)
            _Batch = value
        End Set
    End Property
    Public Property ConditionID() As Integer
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the Condition Id
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	6/26/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _ConditionID
        End Get
        Set(ByVal value As Integer)
            _ConditionID = value
        End Set
    End Property
    Public Property AccumulatorName() As String
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the accumulator name
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/14/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _AccumulatorName.Trim
        End Get
        Set(ByVal value As String)
            _AccumulatorName = value
        End Set
    End Property

    Public ReadOnly Property SortKey() As String

        Get
            Return _Operand.ToString.Trim & " " & _AccumulatorName.Trim & " " & _Duration.ToString & " " & _DurationType.ToString & " " & _Direction.ToString & " " & _UseInHeadroomCheck.ToString
        End Get

    End Property

    Public Property Operand() As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets operand
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/14/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Operand
        End Get
        Set(ByVal value As Decimal)
            _Operand = value
        End Set
    End Property

    Public Property Direction() As DateDirection
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the date direction
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/14/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Direction
        End Get
        Set(ByVal value As DateDirection)
            _Direction = value
        End Set
    End Property

    Public Property Duration() As Integer
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets duration
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/14/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Duration
        End Get
        Set(ByVal value As Integer)
            _Duration = value
        End Set
    End Property

    Public Property AccumulatorYear() As Integer
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the Year the Accumulator belongs too
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/14/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _AccumulatorYear
        End Get
        Set(ByVal value As Integer)
            _AccumulatorYear = value
        End Set
    End Property

    Public Property DurationType() As DateType
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets durationtype
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/14/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _DurationType
        End Get
        Set(ByVal value As DateType)
            _DurationType = value
        End Set
    End Property
    Public Property UseInHeadroomCheck() As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' This property is used to determine if the value of the accumulator (if
        '  one is used on this condition) is checked for headroom for the purposes
        '  of adjusting the member or fund responsibility.
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	9/19/2006	Created - this is added per ACR MED-0018.
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _UseInHeadroomCheck
        End Get
        Set(ByVal value As Boolean)
            _UseInHeadroomCheck = value
        End Set
    End Property
#End Region

#Region "Methods"
    Public Overrides Function ToString() As String
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' An override of the ToString function.  This is used typically for logging.
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/1/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim Operand As String

        If _AccumulatorName Is Nothing Then
            Operand = _Operand.ToString
        ElseIf _AccumulatorName.Trim.Length < 1 Then
            Operand = _Operand.ToString
        Else
            Operand = "ACCUM: " & _AccumulatorName.Trim & " LESS THAN " & Math.Round(_Operand, 2).ToString & " FOR "
            Select Case _Duration
                Case 0
                    Operand += "CURRENT "
                Case 1
                    Operand += If(_Direction = DateDirection.Forward, "CURRENT & NEXT ", "LAST AND CURRENT ")
                Case Is > 1
                    Operand += If(_Direction = DateDirection.Forward, "FUTURE ", "PAST ") & _Duration.ToString & " "
            End Select

            Select Case _DurationType
                Case DateType.Days
                    Operand += "DAY(S)"
                Case DateType.Months
                    Operand += "MONTH(S)"
                Case DateType.Quarters
                    Operand += "QUARTER(S)"
                Case DateType.Weeks
                    Operand += "WEEK(S)"
                Case DateType.Years
                    Operand += "YEAR(S)"
                Case DateType.Rollover
                    Operand += "ROLLOVER(S)"
            End Select

            If _UseInHeadroomCheck Then
                Operand += " - $/UNITS NOT APPLIED AFTER MAX"
            Else
                Operand += " - $/UNITS APPLIED AFTER MAX"
            End If
        End If

        Return Operand
    End Function

    'Public Function Copy() As Condition

    '    Dim Condition As Condition

    '    Try

    '        Condition = New Condition

    '        Condition.AccumulatorName = _AccumulatorName
    '        Condition.ConditionId = _ConditionID
    '        Condition.Direction = _Direction
    '        Condition.Duration = _Duration
    '        Condition.DurationType = _DurationType
    '        Condition.UseInHeadroomCheck = _UseInHeadroomCheck
    '        Condition.RepriceIfExceeded = _RepriceIfExceeded
    '        Condition.Active = _Active
    '        Condition.Manual = _Manual
    '        Condition.Operand = _Operand
    '        Condition.AccumulatorStartDate = _AccumulatorStartDate
    '        Condition.AccumulatorEndDate = _AccumulatorEndDate
    '        Condition.PublishBatchNbr = _PublishBatchNbr
    '        Condition.PlanType = _PlanType

    '        Return Condition

    '    Catch ex As Exception
    '        Throw
    '    Finally
    '        Condition = Nothing
    '    End Try

    'End Function
#End Region

#Region "Clone"
    Public Function DeepCopy() As Condition

        Dim BeginTime As Date = UFCWGeneral.NowDate

        Try

            Return Me.ShallowCopy()

        Catch ex As Exception
            Throw
        Finally
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
        End Try
    End Function

    Public Function ShallowCopy() As Condition
        Dim BeginTime As Date = UFCWGeneral.NowDate

        Try

            Return DirectCast(Me.MemberwiseClone(), Condition)
        Catch
            Throw
        Finally
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
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