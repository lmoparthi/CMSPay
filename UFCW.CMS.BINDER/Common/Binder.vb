Option Strict On

Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Threading

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Binder
''' Class	 : CMS.Binder.Binder
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' Represents a binder
''' </summary>
''' <remarks>
''' _binderItems is purposefully not exposed.  This allows greater control from Binder class.
''' </remarks>
''' <history>
''' 	[paulw]	4/4/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public MustInherit Class Binder
    Implements IBinder, ICloneable

#Region "Properties and Local Variables"

    Private Shared _TraceCloning As New TraceSwitch("TraceCloning", "Trace Switch in App.Config", "0")

    Protected _BinderGuid As System.Guid = Guid.NewGuid()

    Protected _Disposed As Boolean = False
    Protected _OriginalClaimIDForAccident As Integer
    Protected _ClaimID As Integer
    Protected _DateOfClaim As Date?
    Protected _ClaimType As ClaimTypes
    Protected _TypeOfBinder As Integer
    Protected _DocumentClass As String
    Protected _HasAccidentRule As Boolean?
    Protected _IsHRAIneligible As Boolean?
    Protected _IsPreventative As Boolean?

    Protected _BinderAlertManager As AlertManagerCollection
    Protected _BinderItems As Hashtable
    Protected _BinderAccumulatorManager As MemberAccumulatorManager
    Protected _Facts As Facts

    Protected Property ClaimType() As ClaimTypes
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the type of claim
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	8/18/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _ClaimType
        End Get
        Set(ByVal value As ClaimTypes)
            _ClaimType = value
        End Set

    End Property

    Property DateOfClaim() As Date? Implements IBinder.DateOfClaim
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the date of the claim
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _DateOfClaim
        End Get
        Set(ByVal value As Date?)
            _DateOfClaim = value
        End Set
    End Property

    Property Facts() As Facts Implements IBinder.Facts
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the facts
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Facts
        End Get
        Set(ByVal value As Facts)
            _Facts = value
        End Set
    End Property

    Property ClaimNumber() As Integer Implements IBinder.ClaimNumber
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the claimId
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _ClaimID
        End Get
        Set(ByVal value As Integer)
            _ClaimID = value
        End Set
    End Property

    Property BinderAccumulatorManager() As MemberAccumulatorManager Implements IBinder.BinderAccumulatorManager
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the Binder Accumulator Manager
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/22/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _BinderAccumulatorManager
        End Get
        Set(ByVal value As MemberAccumulatorManager)
            _BinderAccumulatorManager = value
        End Set
    End Property

    Property BinderItems() As Hashtable Implements IBinder.BinderItems
        Get
            Return _BinderItems
        End Get
        Set(ByVal value As Hashtable)
            _BinderItems = value
        End Set
    End Property

    Property BinderAlertManager() As AlertManagerCollection Implements IBinder.BinderAlertManager
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the binder alert manager
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/31/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _BinderAlertManager
        End Get
        Set(ByVal value As AlertManagerCollection)
            _BinderAlertManager = value
        End Set
    End Property

    Public ReadOnly Property TypeOfBinder() As Integer Implements IBinder.TypeOfBinder
        Get
            Return _TypeOfBinder
        End Get
    End Property

    Public ReadOnly Property DocumentClass() As String Implements IBinder.DocumentClass
        Get
            Return _DocumentClass
        End Get
    End Property

    Public ReadOnly Property HasAccidentRule() As Boolean Implements IBinder.HasAccidentRule
        Get
            Dim BinderItem As BinderItem
            Dim BinderItemEnum As IDictionaryEnumerator

            Try
                If _HasAccidentRule Is Nothing Then
                    _HasAccidentRule = False

                    If _BinderItems IsNot Nothing Then
                        BinderItemEnum = _BinderItems.GetEnumerator()

                        While BinderItemEnum.MoveNext()
                            BinderItem = CType(BinderItemEnum.Value, BinderItem)
                            If BinderItem.HasAccidentRule(_TypeOfBinder) Then
                                _HasAccidentRule = True
                                Exit While
                            End If
                        End While
                    End If

                End If

                Return CBool(_HasAccidentRule)

            Catch ex As Exception
                Throw
            Finally
                BinderItemEnum = Nothing
            End Try

        End Get
    End Property

    Public ReadOnly Property IsHRAIneligible() As Boolean Implements IBinder.IsHRAInEligible
        Get
            Dim BinderItem As BinderItem
            Dim BinderItemEnum As IDictionaryEnumerator

            Try
                If _IsHRAIneligible Is Nothing Then
                    _IsHRAIneligible = False

                    If _BinderItems IsNot Nothing Then
                        BinderItemEnum = _BinderItems.GetEnumerator()
                        While BinderItemEnum.MoveNext()
                            BinderItem = CType(BinderItemEnum.Value, BinderItem)
                            If BinderItem.IsHRAInEligible(_TypeOfBinder) Then
                                _IsHRAIneligible = True
                                Exit While
                            End If
                        End While
                    End If
                End If

                Return CBool(_IsHRAIneligible)

            Catch ex As Exception
                Throw
            Finally
                BinderItemEnum = Nothing
            End Try
        End Get

    End Property

    Public ReadOnly Property IsPreventative() As Boolean Implements IBinder.IsPreventative
        Get
            Dim BinderItem As BinderItem
            Dim BinderItemEnum As IDictionaryEnumerator

            Try
                If _IsPreventative Is Nothing Then
                    _IsPreventative = False

                    If _BinderItems IsNot Nothing Then
                        BinderItemEnum = _BinderItems.GetEnumerator()
                        While BinderItemEnum.MoveNext()
                            BinderItem = CType(BinderItemEnum.Value, BinderItem)
                            If BinderItem.IsPreventative(_TypeOfBinder) Then
                                _IsPreventative = True
                                Exit While
                            End If
                        End While
                    End If

                End If

                Return CBool(_IsPreventative)

            Catch ex As Exception
                Throw
            Finally
                BinderItemEnum = Nothing
            End Try
        End Get
    End Property

    Property OriginalClaimIDForAccident() As Integer Implements IBinder.OriginalClaimIDForAccident
        Get
            Return _OriginalClaimIDForAccident
        End Get
        Set(ByVal value As Integer)
            _OriginalClaimIDForAccident = value
        End Set
    End Property

#End Region

#Region "Constructors"
    Protected Sub New()
#If debug Then
        Debug.Print(TypeName(Me) & " New: " & _BinderGuid.ToString)
#End If

        _BinderItems = New Hashtable
    End Sub
#End Region

#Region "Methods"

    Public MustOverride Function GetAccumulatorSummary() As DataTable Implements IBinder.GetAccumulatorSummary
    Public MustOverride Function GetAccumulatorSummaryCommitted(ByVal conditionsDate As Date) As DataTable Implements IBinder.GetAccumulatorSummaryCommitted
    Public MustOverride Function GetOriginalAccumulatorSummary() As DataTable Implements IBinder.GetOriginalAccumulatorSummary
    Public MustOverride Function NewBinderItem() As BinderItem Implements IBinder.NewBinderItem
    Public MustOverride Sub AddBinderItem(ByVal lineNumber As Short, ByVal procedure As ProcedureActive, ByVal valuedAmount As Decimal, ByVal incidentDate As Date) Implements IBinder.AddBinderItem
    Public MustOverride Sub AddBinderItem(ByVal lineNumber As Short) Implements IBinder.AddBinderItem
    Public MustOverride Sub AddBinderItem(ByVal bndrItem As BinderItem) Implements IBinder.AddBinderItem
    Public MustOverride Sub BuildFacts() Implements IBinder.BuildFacts
    Public MustOverride Sub BuildFacts(ByVal lineNumbers As Short()) Implements IBinder.BuildFacts
    Public MustOverride Sub ReplaceAccidentAccumulator(ByVal accumName As String) Implements IBinder.ReplaceAccidentAccumulator
    Public MustOverride Sub RemoveAccidentAccumulators() Implements IBinder.RemoveAccidentAccumulators
    Public Sub RemoveBinderItem(ByVal lineNumber As Short) Implements IBinder.RemoveBinderItem
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' removes a binder item
        ' </summary>
        ' <param name="lineNumber"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            _BinderItems.Remove(lineNumber)
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw New BinderItemDoesNotExistException("The specified line id does not have a corresponding binder item", ex)
            End If
        End Try
    End Sub
    Public Function GetBinderItem(ByVal lineNumber As Short) As BinderItem Implements IBinder.GetBinderItem
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' gets a binder item
        ' </summary>
        ' <param name="lineNumber"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        If lineNumber < 0 Then Throw New ArgumentOutOfRangeException("lineNumber")

        Try
            Return CType(_BinderItems(lineNumber), BinderItem)
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw New BinderItemDoesNotExistException("The specified line id does not have a corresponding binder item", ex)
            End If
        End Try
    End Function

#End Region

#Region "Clone"

    Public Function ShallowCopy() As Binder
        Dim BeginTime As Date = UFCWGeneral.NowDate
        Dim BinderClone As Binder

        Try
            BinderClone = DirectCast(Me.MemberwiseClone(), Binder)
            BinderClone.BinderItems = Nothing
            BinderClone.BinderAlertManager = Nothing
            BinderClone.BinderAccumulatorManager = Nothing
            BinderClone.Facts = Nothing

            Return BinderClone

        Catch ex As Exception
            Throw
        Finally
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
        End Try
    End Function

    Public Function DeepCopy() As Binder
        Dim BeginTime As Date = UFCWGeneral.NowDate
        Dim BinderClone As Binder
        Dim BinderItemsClone As Hashtable

        Try
            BinderClone = Me.ShallowCopy

            BinderItemsClone = CloneHelper.DeepCopy(Me.BinderItems, BinderItemsClone)
            BinderClone.BinderItems = BinderItemsClone

            BinderClone.BinderAlertManager = Me.BinderAlertManager.DeepCopy
            BinderClone.BinderAccumulatorManager = Me.BinderAccumulatorManager.DeepCopy

            BinderClone.Facts = Me.Facts.DeepCopy

            Return BinderClone

        Catch ex As Exception
            Throw
        Finally
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
        End Try
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone

        Dim BeginTime As Date = UFCWGeneral.NowDate

        Try

            Return CloneHelper.Clone(Me)

        Catch ex As Exception
            Throw
        Finally
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
        End Try

    End Function

#End Region

#Region "Clean Up"

    'Public Overloads Sub Dispose() Implements IDisposable.Dispose

    '    Dispose(True)
    '    GC.SuppressFinalize(Me)
    'End Sub

    'Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
    '    If _disposed = False Then
    '        If disposing Then

    '            'Debug.Print(TypeName(Me) & " Dispose: " & _ClassGuid.ToString)

    '            ' Free other state (managed objects).

    '            If Not Me.Facts Is Nothing Then
    '                For Each fct As Fact In Me.Facts
    '                    fct = Nothing
    '                Next
    '            End If

    '            _binderItems.Clear()
    '            _binderItems = Nothing

    '            _disposed = True
    '        End If
    '        ' Free your own state (unmanaged objects).
    '        ' Set large fields to null.
    '    End If
    'End Sub

    'Protected Overrides Sub Finalize()
    '    'Debug.Print(TypeName(Me) & " Finalize: " & _ClassGuid.ToString)
    '    MyBase.Finalize()
    'End Sub

#End Region

End Class

Public Enum ClaimTypes
    Standard = 0
    Accident = 1
End Enum

