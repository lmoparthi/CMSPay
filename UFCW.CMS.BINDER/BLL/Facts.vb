Imports System.Threading


''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Binder
''' Class	 : CMS.Binder.Facts
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' Represents the 'Facts' of a Binder
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/5/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> Public Class Facts
    Inherits CollectionBase
    Implements ICloneable

    Private Shared _TraceCloning As New TraceSwitch("TraceCloning", "Trace Switch in App.Config", "0")

    Private _ClaimLevelUnitAmount As New Hashtable
    Private _ClaimLevelUnitAmountSet As New Hashtable

#Region "Constructors"

    Public Sub New()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Default Constructor
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        MyBase.New()
    End Sub

    Public Sub New(ByVal binderFacts As Facts)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Constructor to add premade facts to this collection
        ' </summary>
        ' <param name="binderFacts"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Me.InnerList().AddRange(binderFacts)
    End Sub
#End Region

#Region "Misc"
    Default Public Property Item(ByVal index As Integer) As Fact
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets and Stes Fact at the specified index
        ' </summary>
        ' <param name="index"></param>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return CType(Me.List(index), Fact)
        End Get
        Set(ByVal value As Fact)
            Me.List(index) = value
        End Set
    End Property

    Public Property ClaimLevelUnitAmount(ByVal procedureCode As String) As Decimal
        Get
            If _ClaimLevelUnitAmount.ContainsKey(procedureCode) Then
                Return _ClaimLevelUnitAmount(procedureCode)
            Else
                Return -1
            End If
        End Get
        Set(ByVal value As Decimal)
            If _ClaimLevelUnitAmount.ContainsKey(procedureCode) Then
                _ClaimLevelUnitAmount(procedureCode) = value
            Else
                _ClaimLevelUnitAmount.Add(procedureCode, value)
            End If
        End Set
    End Property

    Public Property ClaimLevelUnitAmountSet(ByVal procedureCode As String) As Boolean
        Get
            If _ClaimLevelUnitAmountSet.ContainsKey(procedureCode) Then
                Return _ClaimLevelUnitAmountSet(procedureCode)
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            If _ClaimLevelUnitAmountSet.ContainsKey(procedureCode) Then
                _ClaimLevelUnitAmountSet(procedureCode) = value
            Else
                _ClaimLevelUnitAmountSet.Add(procedureCode, value)
            End If
        End Set
    End Property
#End Region

#Region "Methods"

    Public Sub Add(ByVal Fact As Fact)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Adds a Fact
        ' </summary>
        ' <param name="item"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            Me.List.Add(Fact)

            ClaimLevelUnitAmount(Fact.ProcedureCodeUsed) = Fact.ClaimLevelUnitAmount(Fact.ProcedureCodeUsed)
            ClaimLevelUnitAmountSet(Fact.ProcedureCodeUsed) = Fact.ClaimLevelUnitAmountSet(Fact.ProcedureCodeUsed)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function Contains(ByVal theItem As Fact) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' See if this Fact already exists
        ' </summary>
        ' <param name="item"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        If theItem Is Nothing Then Throw New ArgumentNullException("item")

        Dim Fact As Fact

        Try

            For I As Integer = 0 To Me.List.Count - 1
                Fact = CType(Me.List(I), Fact)
            Next

        Catch ex As Exception
            Throw
        End Try

    End Function

    'Public Function IndexOf(ByVal item As Fact) As Int32
    '    ' -----------------------------------------------------------------------------
    '    ' <summary>
    '    ' Get the index of the Fact
    '    ' </summary>
    '    ' <param name="item"></param>
    '    ' <returns></returns>
    '    ' <remarks>
    '    ' </remarks>
    '    ' <history>
    '    ' 	[paulw]	4/5/2006	Created
    '    ' </history>
    '    ' -----------------------------------------------------------------------------

    '    If item Is Nothing Then Throw New ArgumentNullException("item")

    '    Dim Fact As Fact

    '    Try

    '        For I As Integer = 0 To Me.List.Count - 1
    '            Fact = CType(Me.List(I), Fact)
    '        Next

    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Function

    Protected Overrides Sub OnValidate(ByVal value As Object)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Overriding the OnValidate allows for the confirmation of a strongly typed
        ' 'item' as a more generic IList object.
        ' </summary>
        ' <param name="item"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        MyBase.OnValidate(value)
        If Not (TypeOf (value) Is Fact) Then
            Throw New ArgumentException("Collection only supports objects implementing Fact.", value.ToString)
        End If
    End Sub

#End Region

#Region "Clone"
    Public Function DeepCopy() As Facts

        Dim FactsClone As Facts
        Dim BeginTime As Date = UFCWGeneral.NowDate

        Try

            FactsClone = ShallowCopy()

            For Each Fact As Fact In DirectCast(CloneHelper.DeepCopy(Me), Facts)
                FactsClone.Add(Fact)
            Next

            FactsClone._ClaimLevelUnitAmount = CloneHelper.DeepCopy(Me._ClaimLevelUnitAmount, _ClaimLevelUnitAmount)
            FactsClone._ClaimLevelUnitAmountSet = CloneHelper.DeepCopy(Me._ClaimLevelUnitAmountSet, _ClaimLevelUnitAmountSet)

            Return FactsClone

        Catch ex As Exception
            Throw
        Finally
            FactsClone = Nothing
#If TRACE Then
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
#End If
        End Try

    End Function

    Public Function ShallowCopy() As Facts
        Dim BeginTime As Date = UFCWGeneral.NowDate
        Dim FactsClone As Facts

        Try

            FactsClone = DirectCast(Me.MemberwiseClone(), Facts)
            FactsClone._ClaimLevelUnitAmount = Nothing
            FactsClone._ClaimLevelUnitAmountSet = Nothing

            Return FactsClone

        Catch
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
#End If
        End Try

    End Function

    Public Function Clone() As Object Implements ICloneable.Clone

        Dim BeginTime As Date = UFCWGeneral.NowDate

        Try

            Return DirectCast(CloneHelper.Clone(Me), Facts)

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
#End If
        End Try
    End Function

#End Region

End Class