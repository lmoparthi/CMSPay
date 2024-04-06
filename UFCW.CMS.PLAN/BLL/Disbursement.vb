Option Strict On

Imports System.Threading

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.ProcessorEngine
''' Class	 : CMS.ProcessorEngine.Disbursement
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class represents the disbursemnt of funds and units
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/12/2006	Created
'''     [paulw] 9/26/2006   Per ACR MED-0029, added MultilineFundPayment to handle MultiLineCoPay type
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public NotInheritable Class Disbursement
    Implements ICloneable

#Region "Private Variables"
    Private Shared _TraceCloning As New TraceSwitch("TraceCloning", "Trace Switch in App.Config", "0")

    Private _FundPayment As Decimal
    Private _MemberPayment As Decimal
    Private _Units As Decimal
    Private _MultiLineFundPayment As Decimal
    Private _CheckMultiLine As Boolean
    Private _OriginalAmt As Decimal
    Private _PreSubTotalMemberPayment As Decimal
    Private _PreSubTotalMemberPaymentAmt As Decimal
    Private _PreSubTotalNonParMemberPaymentAmt As Decimal
    Private _PreSubTotalParMemberPaymentAmt As Decimal
    Private _ParMemberPaymentAmt As Decimal
    Private _NonParMemberPaymentAmt As Decimal
#End Region

#Region "Properties"

    Public Property PreSubTotalNonParMemberPaymentValue() As Decimal
        Get
            Return _PreSubTotalNonParMemberPaymentAmt
        End Get
        Set(ByVal value As Decimal)
            _PreSubTotalNonParMemberPaymentAmt = value
        End Set
    End Property

    Public Property PreSubTotalParMemberPaymentValue() As Decimal
        Get
            Return _PreSubTotalParMemberPaymentAmt
        End Get
        Set(ByVal value As Decimal)
            _PreSubTotalParMemberPaymentAmt = value
        End Set
    End Property

    Public Property ParMemberPaymentValue() As Decimal
        Get
            Return _ParMemberPaymentAmt
        End Get
        Set(ByVal value As Decimal)
            _ParMemberPaymentAmt = value
        End Set
    End Property

    Public Property NonParMemberPaymentValue() As Decimal
        Get
            Return _NonParMemberPaymentAmt
        End Get
        Set(ByVal value As Decimal)
            _NonParMemberPaymentAmt = value
        End Set
    End Property

    Public Property PreSubTotalMemberPayment() As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/4/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _PreSubTotalMemberPayment
        End Get
        Set(ByVal value As Decimal)
            _PreSubTotalMemberPayment = value
        End Set
    End Property

    Public Property OriginalAmount() As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' Added to satisify the pricing of units
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/12/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _OriginalAmt
        End Get
        Set(ByVal value As Decimal)
            _OriginalAmt = value
        End Set
    End Property

    Public Property CheckMultiLineValue() As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/4/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _CheckMultiLine
        End Get
        Set(ByVal value As Boolean)
            _CheckMultiLine = Value
        End Set
    End Property

    Public Property MultiLineFundPayment() As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets fund responsibilty for a multi-line CoPay claim
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	9/26/2006	Created as functionality support for ACR MED-0029
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _MultiLineFundPayment
        End Get
        Set(ByVal value As Decimal)
            _MultiLineFundPayment = value
        End Set
    End Property

    Public Property FundPayment() As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets fund responsibilty
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/12/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _FundPayment
        End Get
        Set(ByVal value As Decimal)
            _FundPayment = value
        End Set
    End Property

    Public Property MemberPayment() As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets member responsibility
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/12/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _MemberPayment
        End Get
        Set(ByVal value As Decimal)
            _MemberPayment = value
        End Set
    End Property

    Public Property Units() As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets units
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/12/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Units
        End Get
        Set(ByVal value As Decimal)
            _Units = value
        End Set
    End Property

    Public ReadOnly Property Total() As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets the total for Member and Fund responsibility
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/12/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _FundPayment + _MemberPayment
        End Get
    End Property
#End Region

#Region "Clone"
    Public Function DeepCopy() As Disbursement

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

    Public Function ShallowCopy() As Disbursement

        Dim BeginTime As Date = UFCWGeneral.NowDate

        Try

            Return DirectCast(Me.MemberwiseClone(), Disbursement)

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

    'Public Function Clone() As Disbursement
    '    Dim dsb As New Disbursement
    '    dsb.CheckMultiLineValue = Me.CheckMultiLineValue
    '    dsb.FundPayment = Me.FundPayment
    '    dsb.MemberPayment = Me.MemberPayment
    '    dsb.MultiLineFundPayment = Me.MultiLineFundPayment
    '    dsb.OriginalAmount = Me.OriginalAmount
    '    dsb.PreSubTotalMemberPayment = Me.PreSubTotalMemberPayment
    '    dsb.Units = Me.Units
    '    dsb.PreSubTotalNonParMemberPaymentValue = Me.PreSubTotalNonParMemberPaymentValue
    '    dsb.PreSubTotalParMemberPaymentValue = Me.PreSubTotalParMemberPaymentValue
    '    dsb.ParMemberPaymentValue = Me.ParMemberPaymentValue
    '    dsb.NonParMemberPaymentValue = Me.NonParMemberPaymentValue
    '    Return dsb
    'End Function

End Class