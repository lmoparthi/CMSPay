Option Strict On

Imports System.Threading

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.ProcessorEngine
''' Class	 : CMS.ProcessorEngine.Rule
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class represents a base rule
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/12/2006	Created
'''     [paulw] 9/26/2006   Per ACR MED-0029, added MultiLineCoPay type to enum
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public MustInherit Class Rule
    Implements ICloneable

#Region "Properties and Local Variables"
    Private Shared _TraceCloning As New TraceSwitch("TraceCloning", "Trace Switch in App.Config", "0")

    Protected _Disbursement As Disbursement
    Protected _Actions As Actions
    Protected _Conditions As Conditions
    Protected _RuleName As String
    Protected _RuleID As Integer

    Public Property RuleName() As String
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the rule name
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' as of 9/20/2006, still not in use
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/12/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _RuleName
        End Get
        Set(ByVal value As String)
            _RuleName = value
        End Set
    End Property

    Public Property RuleID() As Integer
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the ruleid
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/12/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _RuleID
        End Get
        Set(ByVal value As Integer)
            _RuleID = value
        End Set
    End Property

    Public Property Disbursement() As Disbursement
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the rule Disbursement
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/12/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Disbursement
        End Get
        Set(value As Disbursement)
            _Disbursement = value
        End Set
    End Property

    Public Property Actions() As Actions
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the rule actions
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/12/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Actions
        End Get
        Set(value As Actions)
            _Actions = value
        End Set
    End Property

    Public Property Conditions() As Conditions
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the rule conditions
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/12/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Conditions
        End Get
        Set(value As Conditions)
            _Conditions = value
        End Set
    End Property

#End Region

#Region "Constructors"
    Private Sub New()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Hide the default constructor
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/12/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
    End Sub

    Public Sub New(ByVal ruleActions As Actions, ByVal ruleConditions As Conditions)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Constructor
        ' </summary>
        ' <param name="ruleActions"></param>
        ' <param name="ruleConditions"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/19/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            _Actions = New Actions
            _Conditions = ruleConditions

            For Each Action As IAction In ruleActions
                _Actions.Add(Action)
            Next

            _RuleName = ""

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub New(ByVal ruleConditions As Conditions)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Constructor
        ' </summary>
        ' <param name="ruleConditions"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/19/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        _Conditions = ruleConditions
        _Actions = New Actions
        _RuleName = ""
    End Sub
#End Region

#Region "Methods"

    Public MustOverride Function Eval(ByVal disbursement As Disbursement, ByVal memberAccumulatorManager As MemberAccumulatorManager, ByVal dateOfService As Date?, ByVal actionDelegate As IAction.ExecuteAction, ByVal alertDelegate As Alert.AddAlert) As Disbursement

    Public Shared Function GetSmallestHeadroom(ByVal conditions As Conditions, ByVal memberAccumulatorManager As MemberAccumulatorManager, ByVal dateOfService As Date?) As Decimal
        Dim SmallestAmt As Decimal = Decimal.MaxValue - 1
        Dim Condition As Condition
        Dim Amt As Decimal
        Dim SetToZero As Boolean = False

        Try

            For I As Integer = 0 To conditions.Count - 1
                Condition = DirectCast(conditions(I), Condition)
                Amt = memberAccumulatorManager.GetProposedValue(CInt(AccumulatorController.GetAccumulatorID(Condition.AccumulatorName)), Condition.DurationType, Condition.Duration, dateOfService, Condition.Direction)

                If Condition.Operand - Amt < 0 Then SetToZero = True
                If ((Condition.Operand - Amt) < SmallestAmt) AndAlso (Condition.Operand - Amt) >= 0 Then SmallestAmt = Condition.Operand - Amt
            Next

            Return If(SetToZero, 0, SmallestAmt)

        Catch ex As Exception
            Throw
        Finally
            Condition = Nothing
        End Try

    End Function

    Public Shared Function GetSmallestHeadroom(ByVal rule As Rule, ByVal memberAccumulatorManager As MemberAccumulatorManager, ByVal dateOfService As Date?, ByVal originalAmt As Boolean) As Decimal
        Dim SmallestAmt As Decimal = Decimal.MaxValue - 1
        Dim Condition As Condition
        Dim Amt As Decimal

        Try

            For Each DE As DictionaryEntry In rule.Conditions

                Condition = DirectCast(DE.Value, Condition)

                If originalAmt Then
                    Amt = memberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID(Condition.AccumulatorName)), Condition.DurationType, Condition.Duration, dateOfService, Condition.Direction)
                Else
                    Amt = memberAccumulatorManager.GetProposedValue(CInt(AccumulatorController.GetAccumulatorID(Condition.AccumulatorName)), Condition.DurationType, Condition.Duration, dateOfService, Condition.Direction)
                End If

                If ((Condition.Operand - Amt) < SmallestAmt) AndAlso (Condition.Operand - Amt) >= 0 Then SmallestAmt = Condition.Operand - Amt
            Next

            Return SmallestAmt

        Catch ex As Exception
            Throw
        Finally
            Condition = Nothing
        End Try

    End Function

    Public Function GetParHeadroomForNonParClaim(ByVal accumManager As MemberAccumulatorManager, ByVal originalValue As Boolean, ByVal dateOfService As Date) As Decimal
        Dim Amt As Decimal
        Dim Smallest As Decimal = Decimal.MaxValue - 1

        Try

            For Each Condition As Condition In _Conditions
                If Not Condition.UseInHeadroomCheck Then
                    If originalValue Then
                        Amt = accumManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID(Condition.AccumulatorName)), Condition.DurationType, Condition.Duration, dateOfService, Condition.Direction)
                    Else
                        Amt = accumManager.GetProposedValue(CInt(AccumulatorController.GetAccumulatorID(Condition.AccumulatorName)), Condition.DurationType, Condition.Duration, dateOfService, Condition.Direction)
                    End If
                End If
                If ((Condition.Operand - Amt) < Smallest) Then Smallest = Condition.Operand - Amt
            Next

            If Smallest < 0 Then
                Return Smallest
            Else
                Return 0
            End If

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Function GetNonParHeadroom(ByVal accumManager As MemberAccumulatorManager, ByVal originalValue As Boolean, ByVal dateOfService As Date) As Decimal
        Dim Amt As Decimal
        Dim Smallest As Decimal = Decimal.MaxValue - 1

        Try

            For Each Condition As Condition In _Conditions
                If Condition.UseInHeadroomCheck Then
                    If originalValue Then
                        Amt = accumManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID(Condition.AccumulatorName)), Condition.DurationType, Condition.Duration, dateOfService, Condition.Direction)
                    Else
                        Amt = accumManager.GetProposedValue(CInt(AccumulatorController.GetAccumulatorID(Condition.AccumulatorName)), Condition.DurationType, Condition.Duration, dateOfService, Condition.Direction)
                    End If
                End If
                If ((Condition.Operand - Amt) < Smallest) Then Smallest = Condition.Operand - Amt
            Next

            If Smallest < 0 Then
                Return Smallest
            Else
                Return 0
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function

    Friend Shared Sub DetermineParAndNonParConditions(ByVal conditions As Conditions, ByRef parConditions As Conditions, ByRef nonParConditions As Conditions)
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="cnds"></param>
        ' <param name="parConditions"></param>
        ' <param name="nonParConditions"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	9/5/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim ParConditionMatch As Condition

        Try

            If parConditions Is Nothing Then parConditions = New Conditions
            If nonParConditions Is Nothing Then nonParConditions = New Conditions

            'non-family
            For I As Integer = 0 To conditions.Count - 1
                If AccumulatorController.GetAccumulatorIsFamily(CInt(AccumulatorController.GetAccumulatorID(conditions(I).AccumulatorName))) = False Then
                    If ParConditionMatch Is Nothing Then
                        ParConditionMatch = conditions(I)
                    Else
                        If conditions(I).Operand < ParConditionMatch.Operand Then
                            ParConditionMatch = conditions(I)
                        End If
                    End If
                End If
            Next

            For I As Integer = 0 To conditions.Count - 1
                If AccumulatorController.GetAccumulatorIsFamily(CInt(AccumulatorController.GetAccumulatorID(conditions(I).AccumulatorName))) = False Then
                    If conditions(I).Operand = ParConditionMatch.Operand Then
                        parConditions.Add(conditions(I))
                    Else
                        nonParConditions.Add(conditions(I))
                    End If
                End If
            Next

            'family
            parConditionMatch = Nothing
            For I As Integer = 0 To conditions.Count - 1
                If AccumulatorController.GetAccumulatorIsFamily(CInt(AccumulatorController.GetAccumulatorID(conditions(I).AccumulatorName))) Then
                    If ParConditionMatch Is Nothing Then
                        ParConditionMatch = conditions(I)
                    Else
                        If conditions(I).Operand < ParConditionMatch.Operand Then
                            ParConditionMatch = conditions(I)
                        End If
                    End If
                End If
            Next

            For I As Integer = 0 To conditions.Count - 1
                If AccumulatorController.GetAccumulatorIsFamily(CInt(AccumulatorController.GetAccumulatorID(conditions(I).AccumulatorName))) Then
                    If conditions(I).Operand = ParConditionMatch.Operand Then
                        parConditions.Add(conditions(I))
                    Else
                        nonParConditions.Add(conditions(I))
                    End If
                End If
            Next

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function GetHeadroom(ByVal accumManager As MemberAccumulatorManager, ByVal dateOfService As Date, ByRef accumName As String) As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="orginalDisbursement"></param>
        ' <param name="processedDisbursement"></param>
        ' <param name="accumManager"></param>
        ' <param name="dateOfService"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/12/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim Condition As Condition
        Dim Disbursement As Disbursement
        Dim CurrentAmt As Decimal
        Dim HeadRoom As Decimal
        Dim ReturnHeadRoom As Decimal = Decimal.MaxValue

        Try
            Disbursement = New Disbursement

            For I As Integer = 0 To _Conditions.Count - 1
                Condition = DirectCast(_Conditions(I), Condition)

                CurrentAmt = accumManager.GetProposedValue(CInt(AccumulatorController.GetAccumulatorID(Condition.AccumulatorName)), Condition.DurationType, Condition.Duration, dateOfService, Condition.Direction)

                HeadRoom = Condition.Operand - CurrentAmt

                If HeadRoom < ReturnHeadRoom Then
                    ReturnHeadRoom = HeadRoom
                    accumName = Condition.AccumulatorName
                End If
            Next

            Return ReturnHeadRoom

        Catch ex As Exception
            Throw
        Finally
            Disbursement = Nothing
        End Try
    End Function
#End Region

#Region "Clone"

    Public Function DeepCopy() As Rule

        Dim RuleClone As Rule
        Dim BeginTime As Date = UFCWGeneral.NowDate

        Try

            RuleClone = Me.ShallowCopy

            RuleClone.Disbursement = If(Me.Disbursement Is Nothing, Nothing, Me.Disbursement.DeepCopy)
            RuleClone.Actions = Me.Actions.DeepCopy
            RuleClone.Conditions = Me.Conditions.DeepCopy

            Return RuleClone

        Catch ex As Exception
            Throw
        Finally
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)

            RuleClone = Nothing
        End Try

    End Function

    Public Function ShallowCopy() As Rule
        Dim BeginTime As Date = UFCWGeneral.NowDate
        Dim RuleClone As Rule

        Try

            RuleClone = DirectCast(Me.MemberwiseClone(), Rule)
            RuleClone._Disbursement = Nothing
            RuleClone._Actions = Nothing
            RuleClone._Conditions = Nothing

            Return RuleClone

        Catch
            Throw
        Finally
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
        End Try

    End Function

    Public Function Clone() As Object Implements ICloneable.Clone

        Try

            Return DirectCast(CloneHelper.Clone(Me), Rulesets)

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Function

#End Region

End Class
