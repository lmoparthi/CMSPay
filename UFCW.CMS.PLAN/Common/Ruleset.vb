Option Explicit On
Option Strict On

Imports System.Threading

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.Ruleset
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class represents a base Ruleset.  This class must be inherited.
'''   This class is a collection of rules
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	1/25/2006	Created
'''     [paulw] 10/5/2006   Removed the Enum for ruleset types to accomodate a more
'''                         flexible solution
''' </history>
''' -----------------------------------------------------------------------------

<Serializable()> _
Public MustInherit Class RuleSet
    Inherits CollectionBase
    Implements IRuleset, ICloneable

#Region "Properties and Local Variables"

    Private Shared _TraceCloning As New TraceSwitch("TraceCloning", "Trace Switch in App.Config", "0")

    Private _RuleSetID As Integer
    Private _RuleSetName As String = ""
    Private _Hidden As Boolean
    Private _RuleSetType As Integer
    Private _MaxUnits As Decimal = 9999998

    Public Property MaxUnits() As Decimal
        Get
            Return _MaxUnits
        End Get
        Set(ByVal value As Decimal)
            _MaxUnits = value
        End Set
    End Property

    Public Property RulesetType() As Integer
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the ruleset type
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	6/15/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _RuleSetType
        End Get
        Set(ByVal value As Integer)
            _RuleSetType = value
        End Set
    End Property

    Public Property RulesetID() As Integer Implements IRuleset.RulesetID
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets and Sets the Ruleset id
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _RuleSetID
        End Get
        Set(ByVal value As Integer)
            _RuleSetID = value
        End Set
    End Property

    Public Property RuleSetName() As String Implements IRuleset.RuleSetName
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets and Sets the Ruleset name
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _RuleSetName
        End Get
        Set(ByVal value As String)
            _RuleSetName = value
        End Set
    End Property

    Public Property Hidden() As Boolean Implements IRuleset.Hidden
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets and Sets if the Ruleset is hidden or not
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Hidden
        End Get
        Set(ByVal value As Boolean)
            _Hidden = value
        End Set
    End Property

    Default ReadOnly Property Item(ByVal index As Integer) As Rule
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets the rule at the given index
        ' </summary>
        ' <param name="index"></param>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return DirectCast(Me.List(index), Rule)
        End Get

    End Property

#End Region

#Region "Constructors"
    Protected Sub New()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Default Constructor
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
    End Sub
#End Region

#Region "Functions"
    Public Sub Insert(ByVal ruleItem As Rule)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Adds the Rule to this Ruleset
        ' </summary>
        ' <param name="item"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            Me.List.Add(ruleItem)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub Add(ByVal ruleItem As Rule) Implements IRuleset.Add
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Adds the Rule to this Ruleset
        ' </summary>
        ' <param name="item"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            Me.List.Add(ruleItem)
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Sub Remove(ByVal ruleItem As Rule)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Removed the rule from the ruleset
        ' </summary>
        ' <param name="item"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            Me.List.Remove(ruleItem)

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Function Contains(ByVal ruleItem As Rule) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Determines if the Ruleset already contains the rule
        ' </summary>
        ' <param name="item">a Rule object</param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        If ruleItem Is Nothing Then Throw New ArgumentNullException("item")

        Dim Rule As Rule

        Try

            For I As Integer = 0 To Me.List.Count - 1
                Rule = DirectCast(Me.List(I), Rule)
                If ruleItem.RuleID = Rule.RuleID Then
                    Return True
                End If
            Next

        Catch ex As Exception
            Throw
        Finally
            Rule = Nothing
        End Try
    End Function

    Public Function ContainsRuleType(ByVal ruleTypeItem As Type) As Integer
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Determines if the Ruleset already contains the rule
        ' </summary>
        ' <param name="item">a Ruleset type</param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[behzadk]	6/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        If ruleTypeItem Is Nothing Then Throw New ArgumentNullException("item")

        Dim Rule As Rule

        Try

            For I As Integer = 0 To Me.List.Count - 1
                Rule = DirectCast(Me.List(I), Rule)
                If ruleTypeItem Is Rule.GetType Then
                    Return I
                End If
            Next

            Return -1

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Sub RemoveConditionLessRules()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Removes the rules with no conditions
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[behzadk]	6/19/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim Rule As Rule

        Try

            For I As Integer = 0 To Me.List.Count - 1
                Rule = DirectCast(Me.List(I), Rule)
                If Rule.Conditions.Count = 0 Then
                    Remove(Rule)
                    I = I - 1
                End If

                If I = List.Count - 1 Then
                    Exit For
                End If
            Next
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Function RuleTypeIndex(ByVal ruleType As RuleTypes) As Integer

        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Returns the index where of the rule of that rule type
        ' </summary>
        ' <param name="item"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[behzadk]	6/16/2006	Created
        '     [paulw]     9/27/2006   Per ACR MED-0029, added MultiLineCoPay type support
        '     [paulw]	    10/3/2006	Per ACR MED-0023, added support for deny type
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim Rule As Rule

        Try

            For I As Integer = 0 To Me.List.Count - 1
                Rule = DirectCast(Me.List(I), Rule)
                Select Case ruleType
                    Case RuleTypes.CoInsurance
                        If TypeOf (Rule) Is CoInsuranceRule Then
                            Return I
                        End If
                    Case RuleTypes.CoPay
                        If TypeOf (Rule) Is CoPayRule Then
                            Return I
                        End If
                    Case RuleTypes.MultiLineCoPay
                        If TypeOf (Rule) Is MultilineCoPayRule Then
                            Return I
                        End If
                    Case RuleTypes.ProceduralAllowance
                        If TypeOf (Rule) Is ProceduralAllowanceRule Then
                            Return I
                        End If
                    Case RuleTypes.Deductible
                        If TypeOf (Rule) Is DeductibleRule Then
                            Return I
                        End If
                    Case RuleTypes.OutOfPocket
                        If TypeOf (Rule) Is OutOfPocketRule Then
                            Return I
                        End If
                    Case RuleTypes.Standard
                        If TypeOf (Rule) Is StandardAccumulatorRule Then
                            Return I
                        End If
                    Case RuleTypes.Accident
                        If TypeOf (Rule) Is AccidentRule Then
                            Return I
                        End If
                    Case RuleTypes.Deny
                        If TypeOf (Rule) Is DenyRule Then
                            Return I
                        End If
                    Case RuleTypes.ProviderWriteOff
                        If TypeOf (Rule) Is ProviderWriteOffRule Then
                            Return I
                        End If
                    Case RuleTypes.Original
                        If TypeOf (Rule) Is OriginalRule Then
                            Return I
                        End If
                End Select
            Next

            Return -1

        Catch ex As Exception
            Throw
        End Try
    End Function

    'Public Function IndexOf(ByVal ruleItem As Rule) As Int32
    '    ' -----------------------------------------------------------------------------
    '    ' <summary>
    '    ' Gets the index of the rule
    '    ' </summary>
    '    ' <param name="item"></param>
    '    ' <returns></returns>
    '    ' <remarks>
    '    ' </remarks>
    '    ' <history>
    '    ' 	[paulw]	1/25/2006	Created
    '    ' </history>
    '    ' -----------------------------------------------------------------------------

    '    If ruleItem Is Nothing Then Throw New ArgumentNullException("item")

    '    Dim Rule As Rule

    '    Try

    '        For I As Integer = 0 To List.Count - 1
    '            Rule = DirectCast(List(I), Rule)
    '            If ruleItem.RuleID = Rule.RuleID Then
    '                Return I
    '            End If
    '        Next
    '    Catch ex As Exception
    '        Throw
    '    End Try

    'End Function

    Protected Overrides Sub OnValidate(ByVal value As Object)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' this even happens on validation of the item
        ' </summary>
        ' <param name="item"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        MyBase.OnValidate(value)
        If Not (TypeOf (value) Is Rule) Then
            Throw New ArgumentException("Collection only supports objects implementing Rule.")
        End If
    End Sub

    Protected Sub ClearRules() Implements IRuleset.ClearRules
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Clears all the rules from this ruleset Collection
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	3/21/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Clear()
    End Sub

#End Region

#Region "Clone"

    Public Function DeepCopy() As RuleSet

        Dim RulesetClone As RuleSet
        Dim BeginTime As Date = UFCWGeneral.NowDate

        Try

            ' This copies references types by just copying the pointer, so to break any connection back to those object the objects need to be recreated.
            RulesetClone = Me.ShallowCopy
            For Each Rule As Rule In Me
                RulesetClone.Add(Rule.DeepCopy)
            Next

            Return RulesetClone

        Catch ex As Exception
            Throw
        Finally
            RulesetClone = Nothing
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)

        End Try

    End Function

    Public Function ShallowCopy() As RuleSet
        Dim RulesetClone As RuleSet
        Dim BeginTime As Date = UFCWGeneral.NowDate

        Try

            Select Case True
                Case TypeOf (Me) Is RuleSetActive
                    RulesetClone = New RuleSetActive
                    RulesetClone._RuleSetID = Me.RulesetID
                    RulesetClone._RuleSetName = Me.RuleSetName
                    RulesetClone._RuleSetType = Me.RulesetType

                    DirectCast(RulesetClone, RuleSetActive).PublishBatchNumber = DirectCast(Me, RuleSetActive).PublishBatchNumber
                    DirectCast(RulesetClone, RuleSetActive).PublishDate = DirectCast(Me, RuleSetActive).PublishDate

                Case TypeOf (Me) Is RuleSetStaged
                    RulesetClone = New RuleSetStaged
                    RulesetClone.RulesetID = Me.RulesetID
                Case Else
                    Stop
            End Select

            Return RulesetClone

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