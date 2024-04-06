
Imports System.Threading

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Binder
''' Class	 : CMS.Binder.BinderItem
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' Represents a binder item
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/4/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public MustInherit Class BinderItem
    Implements IBinderItem, IComparable, IDisposable, ICloneable

#Region "Private Members"
    Private Shared _TraceCloning As New TraceSwitch("TraceCloning", "Trace Switch in App.Config", "0")

    Private _ClassGuid As System.Guid = Guid.NewGuid()

    Private _Disposed As Boolean = False
    Private _LineNumber As Short
    Private _ValuedAmount As Decimal
    Private _PaymentAmount As Decimal
    Private _MemberAmount As Decimal
    Private _Status As BinderItemStatus
    Private _IncidentDate As Date
    Private _UnitAmount As Decimal
    Private _StatusDescription As String
    Private _Gender As String
    Private _PrimaryDiagnosis As String
    Private _DateOfService As Date
    Private _DateOfBirth As Date
    Private _RuleSetIDUsed As Integer
    Private _RuleSetNameUsed As String

    Private _Procedure As ProcedureActive

#End Region

#Region "Events"
    <NonSerialized()> Public Event OnBinderItemUpdate(ByVal binderItemArgs As BinderItemEventArgs)
    <NonSerialized()> Public Event OnValuedAmountUpdate(ByVal binderItemArgs As BinderItemEventArgs)
    <NonSerialized()> Public Event OnPaymentAmountUpdate(ByVal binderItemArgs As BinderItemEventArgs)
    <NonSerialized()> Public Event OnStatusUpdate(ByVal binderItemArgs As BinderItemEventArgs)
#End Region

#Region "Constructors"
    Protected Sub New()
#If debug Then
        Debug.Print(TypeName(Me) & " New: " & _ClassGuid.ToString)
#End If
        _LineNumber = -1
    End Sub
#End Region

#Region "Properties"
    Public Property RuleSetNameUsed() As String Implements IBinderItem.RuleSetNameUsed
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
            Return _RuleSetNameUsed
        End Get
        Set(ByVal value As String)
            _RuleSetNameUsed = value
        End Set
    End Property
    Public Property RuleSetIDUsed() As Integer Implements IBinderItem.RuleSetIDUsed
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
            Return _RuleSetIDUsed
        End Get
        Set(ByVal value As Integer)
            _RuleSetIDUsed = value
        End Set
    End Property
    Public Property DateOfService() As Date Implements IBinderItem.DateOfService
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _DateOfService
        End Get
        Set(ByVal value As Date)
            _DateOfService = value
        End Set
    End Property
    Public Property DateOfBirth() As Date Implements IBinderItem.DateOfBirth
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _DateOfBirth
        End Get
        Set(ByVal value As Date)
            _DateOfBirth = value
        End Set
    End Property
    Public Property LineNumber() As Short Implements IBinderItem.LineNumber
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the LineItem
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _LineNumber
        End Get
        Set(ByVal value As Short)
            _LineNumber = value
        End Set
    End Property
    Public Property Procedure() As ProcedureActive Implements IBinderItem.Procedure
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the Procedure
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Procedure
        End Get
        Set(ByVal value As ProcedureActive)
            _Procedure = value
        End Set
    End Property
    Public Property ValuedAmount() As Decimal Implements IBinderItem.ValuedAmount
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the ValuedAmount
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _ValuedAmount
        End Get
        Set(ByVal value As Decimal)
            Dim CurAmt As Decimal = _ValuedAmount
            _ValuedAmount = Math.Round(value, 2)
            RaiseEvent OnBinderItemUpdate(New BinderItemEventArgs(CurAmt, _ValuedAmount, "ValuedAmount"))
            RaiseEvent OnValuedAmountUpdate(New BinderItemEventArgs(CurAmt, _ValuedAmount, "ValuedAmount"))
        End Set
    End Property

    Public Property MemberAmount() As Decimal Implements IBinderItem.MemberAmount
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the MemberAmount
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/1/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _MemberAmount
        End Get
        Set(ByVal value As Decimal)
            Dim CurAmt As Decimal = _MemberAmount
            _MemberAmount = Math.Round(value, 2)
            RaiseEvent OnBinderItemUpdate(New BinderItemEventArgs(CurAmt, _MemberAmount, "MemberAmount"))
        End Set
    End Property
    Public Property PaymentAmount() As Decimal Implements IBinderItem.PaymentAmount
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the PaymentAmount
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _PaymentAmount
        End Get
        Set(ByVal value As Decimal)
            Dim CurAmt As Decimal = _PaymentAmount
            _PaymentAmount = Math.Round(value, 2)

            RaiseEvent OnBinderItemUpdate(New BinderItemEventArgs(CurAmt, _PaymentAmount, "PaymentAmount"))
            RaiseEvent OnPaymentAmountUpdate(New BinderItemEventArgs(CurAmt, _PaymentAmount, "PaymentAmount"))
        End Set
    End Property
    Public Property Status() As BinderItemStatus Implements IBinderItem.Status
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the Status
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Status
        End Get
        Set(ByVal value As BinderItemStatus)
            Dim CurVal As BinderItemStatus = _Status
            _Status = value
            RaiseEvent OnBinderItemUpdate(New BinderItemEventArgs(CurVal, _Status, "Status"))
            RaiseEvent OnStatusUpdate(New BinderItemEventArgs(CurVal, _Status, "Status"))
        End Set
    End Property

    Public Property IncidentDate() As Date Implements IBinderItem.IncidentDate
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the Incident Date
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/7/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _IncidentDate
        End Get
        Set(ByVal value As Date)
            _IncidentDate = value
        End Set
    End Property
    Public Property UnitAmount() As Decimal Implements IBinderItem.UnitAmount
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the unit amount for this binder item
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/3/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _UnitAmount
        End Get
        Set(ByVal value As Decimal)
            _UnitAmount = value
        End Set
    End Property

    Public Property StatusDescription() As String Implements IBinderItem.StatusDescription
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the status description of this binder item
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/3/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _StatusDescription
        End Get
        Set(ByVal value As String)
            _StatusDescription = value
        End Set
    End Property
    Public Property Gender() As String Implements IBinderItem.Gender
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the gender of this binder item
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/3/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Gender
        End Get
        Set(ByVal value As String)
            _Gender = value
        End Set
    End Property

    Public Property PrimaryDiagnosis() As String Implements IBinderItem.PrimaryDiagnosis
        Get
            Return _PrimaryDiagnosis
        End Get
        Set(ByVal value As String)
            _PrimaryDiagnosis = value
        End Set
    End Property

    Public ReadOnly Property HasAccidentRule(ByVal ruleSetType As Integer) As Boolean Implements IBinderItem.HasAccidentRule
        Get
            For Each RuleSet As RuleSet In _Procedure.RuleSets
                If ruleSetType = RuleSet.RulesetType Then
                    For Each Rule As Rule In RuleSet
                        If TypeOf (Rule) Is AccidentRule Then
                            Return True
                        End If
                    Next
                End If
            Next
        End Get
    End Property
    Public ReadOnly Property IsPreventative(ByVal ruleSetType As Integer) As Boolean Implements IBinderItem.IsPreventative
        Get
            For Each RuleSet As RuleSet In _Procedure.RuleSets
                For Each Rule As Rule In RuleSet
                    If TypeOf (Rule) Is PreventativeRule Then
                        Return True
                    End If
                Next
            Next
        End Get
    End Property
    Public ReadOnly Property IsHRAInEligible(ByVal rlSetType As Integer) As Boolean Implements IBinderItem.IsHRAInEligible
        Get
            For Each RuleSet As RuleSet In _Procedure.RuleSets
                For Each Rule As Rule In RuleSet
                    If TypeOf (Rule) Is HRAInEligibleRule Then
                        Return True
                    End If
                Next
            Next
        End Get
    End Property
    Public ReadOnly Property AccidentDuration(ByVal rlSetType As Integer) As Integer Implements IBinderItem.AccidentDuration
        Get
            For Each RuleSet As RuleSet In _Procedure.RuleSets
                If rlSetType = RuleSet.RulesetType Then
                    For Each Rule As Rule In RuleSet
                        If TypeOf (Rule) Is AccidentRule Then
                            If Rule.Conditions(0) IsNot Nothing Then
                                Return Rule.Conditions(0).Duration()
                            End If
                        End If
                    Next
                End If
            Next
        End Get
    End Property

    Public ReadOnly Property AccidentDurationType(ByVal rlSetType As Integer) As DateType Implements IBinderItem.AccidentDurationType
        Get
            For Each RuleSet As RuleSet In _Procedure.RuleSets
                If rlSetType = RuleSet.RulesetType Then
                    For Each Rule As Rule In RuleSet
                        If TypeOf (Rule) Is AccidentRule Then
                            If Rule.Conditions(0) IsNot Nothing Then
                                Return Rule.Conditions(0).DurationType
                            End If
                        End If
                    Next
                End If
            Next
        End Get
    End Property
#End Region

#Region "Methods"

    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        Dim BinderItem As BinderItem = DirectCast(obj, BinderItem)
        Return Me.DateOfService.CompareTo(BinderItem.DateOfService) * -1
    End Function

    Public Sub ReplaceAccidentAccumulator(ByVal accumName As String) Implements IBinderItem.ReplaceAccidentAccumulator

        For Each RuleSet As RuleSet In Me.Procedure.RuleSets
            For Each Rule As Rule In RuleSet
                If TypeOf (Rule) Is AccidentRule Then
                    DirectCast(Rule, AccidentRule).Enabled = True
                    For Each DE As DictionaryEntry In Rule.Conditions
                        DirectCast(DE.Value, Condition).AccumulatorName = accumName
                    Next
                End If
            Next
        Next

    End Sub

    Public Sub RemoveAccidentAccumulators() Implements IBinderItem.RemoveAccidentAccumulators
        For Each RuleSet As RuleSet In Me.Procedure.RuleSets
            For Each Rule As Rule In RuleSet
                If TypeOf (Rule) Is AccidentRule Then
                    DirectCast(Rule, AccidentRule).Enabled = False
                    Exit For
                End If
            Next
        Next
    End Sub
#End Region

#Region "Clone"

    Public Function ShallowCopy() As BinderItem
        Dim BeginTime As Date = UFCWGeneral.NowDate
        Dim BinderItemClone As BinderItem

        Try
            BinderItemClone = DirectCast(Me.MemberwiseClone(), BinderItem)
            BinderItemClone._Procedure = Nothing

            Return BinderItemClone

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
#End If
        End Try
    End Function

    Public Function DeepCopy() As BinderItem
        Dim BeginTime As Date = UFCWGeneral.NowDate
        Dim BinderItemClone As BinderItem

        Try
            BinderItemClone = Me.ShallowCopy
            BinderItemClone.Procedure = If(Me.Procedure Is Nothing, Nothing, Me.Procedure.DeepCopy)

            Return BinderItemClone

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
#End If
        End Try
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone

        Try

            Return CloneHelper.Clone(Me)

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Function

#End Region
#Region "Clean Up"

    ' Public implementation of Dispose pattern callable by consumers.
    Public Sub Dispose() _
               Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    ' Protected implementation of Dispose pattern.
    Protected Overridable Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.
            '
        End If

        _Disposed = True
    End Sub
#End Region

End Class