Option Explicit On
Option Strict On

Imports System.Collections.Generic
Imports System.Threading

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.Procedure
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' The Parent Procdure Class.  This class represents and handles base procedure
'''  functionality
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	1/25/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public MustInherit Class Procedure
    Implements IProcedure, ICloneable

#Region "Properties and Local Variables"

    Private Shared _TraceCloning As New TraceSwitch("TraceCloning", "Trace Switch in App.Config", "0")

    Private Shared _TraceParallel As New TraceSwitch("TraceParallel", "Parallel Trace Switch in App.Config", "0")
    Private Shared _op_EqualsSyncLock As New Object

    Protected _ProcedureID As Integer?
    Protected _BillType As String = ""
    Protected _Diagnosis As String = ""
    Protected _Modifier As String = ""
    Protected _Gender As String = ""
    Protected _MonthsMin As Integer?
    Protected _MonthsMax As Integer?
    Protected _PlaceOfService As String = ""
    Protected _PlanType As String = ""
    Protected _ProcedureCode As String = ""
    Protected _Provider As String = ""
    Protected _Weight As Integer?

    Protected _RuleSets As Rulesets

    Public ReadOnly Property ProcedureID() As Integer? Implements IProcedure.ProcedureID
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets the ProcedureId
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _ProcedureID
        End Get
    End Property

    Public Property BillType() As String Implements IProcedure.BillType
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets and Sets the Bill type
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _BillType
        End Get
        Set(ByVal value As String)
            _BillType = value
        End Set
    End Property

    Public Property Diagnosis() As String Implements IProcedure.Diagnosis
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets and Sets the Diagnosis
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Diagnosis
        End Get
        Set(ByVal value As String)
            _Diagnosis = value
        End Set
    End Property

    Public Property Modifier() As String Implements IProcedure.Modifier
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets and Sets the Modifier
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Modifier
        End Get
        Set(ByVal value As String)
            _Modifier = value
        End Set
    End Property

    Public Property Gender() As String Implements IProcedure.Gender
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets and Sets the Modifier
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Gender
        End Get
        Set(ByVal value As String)
            _Gender = value
        End Set
    End Property

    Public Property MonthsMin() As Integer? Implements IProcedure.MonthsMin
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets and Sets the Modifier
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _MonthsMin
        End Get
        Set(ByVal value As Integer?)
            _MonthsMin = value
        End Set
    End Property

    Public Property MonthsMax() As Integer? Implements IProcedure.MonthsMax
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets and Sets the Modifier
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _MonthsMax
        End Get
        Set(ByVal value As Integer?)
            _MonthsMax = value
        End Set
    End Property

    Public Property PlaceOfService() As String Implements IProcedure.PlaceOfService
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets and Sets the Place of Service
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _PlaceOfService
        End Get
        Set(ByVal value As String)
            _PlaceOfService = value
        End Set
    End Property

    Public Property PlanType() As String Implements IProcedure.PlanType
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets and Sets the Plan type
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _PlanType.Trim
        End Get
        Set(ByVal value As String)
            If value.Length > 5 Then
                Throw New ArgumentException("PlanType argument exceeds acceptable length.", value)
            Else
                _PlanType = value
            End If
        End Set
    End Property

    Public Property ProcedureCode() As String Implements IProcedure.ProcedureCode
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets and Sets the Procedure Code
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _ProcedureCode.Trim
        End Get
        Set(ByVal value As String)
            _ProcedureCode = value
        End Set
    End Property

    Public Property Provider() As String Implements IProcedure.Provider
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets and Sets the Provider Id
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Provider
        End Get
        Set(ByVal value As String)
            _Provider = value
        End Set
    End Property

    Public Property RuleSets() As Rulesets Implements IProcedure.Rulesets
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets and Sets the Ruleset object
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            If _RuleSets Is Nothing Then
                _RuleSets = New Rulesets
            End If

            Return _RuleSets
        End Get
        Set(value As Rulesets)
            _RuleSets = value
        End Set
    End Property

    Public Property Weight() As Integer? Implements IProcedure.Weight
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets and Sets the Weight
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _Weight
        End Get
        Set(ByVal value As Integer?)
            _Weight = value
        End Set
    End Property
#End Region

#Region "Misc Functions"
    Public Function IsOriginalRule(ByVal ruleSetType As Integer?) As Boolean Implements IProcedure.IsOriginalRule
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="ruleSetType"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	2/28/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            For Each RuleSet As RuleSet In Me.RuleSets
                If RuleSet.RulesetType = ruleSetType Then
                    For Each Rule As Rule In RuleSet
                        If TypeOf (Rule) Is OriginalRule Then
                            Return True
                        End If
                    Next
                End If
            Next

            Return False
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function IsReviewRule(ByVal ruleSetType As Integer?) As Boolean Implements IProcedure.IsReviewRule
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="ruleSetType"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	2/28/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            For Each RuleSet As RuleSet In Me.RuleSets
                If RuleSet.RulesetType = ruleSetType Then
                    For Each Rule As Rule In RuleSet
                        If TypeOf (Rule) Is ReviewRule Then
                            Return True
                        End If
                    Next
                End If
            Next

            Return False

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function IsDenyRule(ByVal ruleSetType As Integer?) As Boolean Implements IProcedure.IsDenyRule
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="ruleSetType"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	2/28/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            For Each RuleSet As RuleSet In Me.RuleSets
                If RuleSet.RulesetType = ruleSetType Then
                    For Each Rule As Rule In RuleSet
                        If TypeOf (Rule) Is DenyRule Then
                            Return True
                        End If
                    Next
                End If
            Next
            Return False

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function IsProviderWriteOffRule(ByVal ruleSetType As Integer?) As Boolean Implements IProcedure.IsProviderWriteOffRule
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="ruleSetType"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	3/8/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            For Each RuleSet As RuleSet In Me.RuleSets
                If RuleSet.RulesetType = ruleSetType Then
                    For Each Rule As Rule In RuleSet
                        If TypeOf (Rule) Is ProviderWriteOffRule Then
                            Return True
                        End If
                    Next
                End If
            Next

            Return False

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function IsDenyOrProviderWriteOffRule(ByVal ruleSetType As Integer?) As Boolean Implements IProcedure.IsDenyOrProviderWriteOffRule
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="ruleSetType"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	3/8/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            For Each RuleSet As RuleSet In Me.RuleSets
                If RuleSet.RulesetType = ruleSetType Then
                    For Each Rule As Rule In RuleSet
                        If TypeOf (Rule) Is ProviderWriteOffRule OrElse TypeOf (Rule) Is DenyRule Then
                            Return True
                        End If
                    Next
                End If
            Next
            Return False

        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Overrides Function ToString() As String
        Return $"Procedure ID: {_ProcedureID} Procedure Code: {_ProcedureCode} Modifier: {_Modifier.ToString} Gender: {_Gender.ToString} Min Age: {_MonthsMin.ToString} Max Age: {_MonthsMax.ToString} Diagnosis: {_Diagnosis.ToString()} Bill Type: {_BillType} Place of Service: {_PlaceOfService} Provider: {_Provider} Weight: {_Weight} Gender: {_Gender} Min Age: {_MonthsMin} Max Age: {_MonthsMax}"
    End Function

    Public Overrides Function GetHashCode() As Integer Implements IProcedure.GetHashCode
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets the custom Hashcode for the Procedure
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	2/6/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Return CStr(_ProcedureID.ToString & _BillType.ToString & _Diagnosis.ToString & _PlaceOfService.ToString & _PlanType.ToString & _ProcedureCode.ToString & _Provider.ToString).GetHashCode
    End Function

    Public Shared Function op_Equals(ByVal claimProcedure As Procedure, ByVal ruleProcedure As Procedure) As Boolean
        'each data element is compared, True is returned if the two procedures match perfectly
        SyncLock (_op_EqualsSyncLock)

            Try

                If claimProcedure Is Nothing Then Throw New ArgumentNullException("claimProcedureIsNull")

                'add better exception description (2nd procedure has not been initialized... Why?)
                If ruleProcedure Is Nothing Then Throw New ArgumentNullException("ruleProcedureIsNull")

                'Note: When a rule has no value specified for a filter it should be included as a positive result. The rule with the higest weight will be used in a later process
                If ruleProcedure.BillType.Trim.Length > 0 AndAlso Not claimProcedure.BillType = ruleProcedure.BillType Then Return False
                If ruleProcedure.Diagnosis.Trim.Length > 0 AndAlso Not claimProcedure.Diagnosis = ruleProcedure.Diagnosis Then Return False
                If ruleProcedure.Modifier.Trim.Length > 0 AndAlso Not claimProcedure.Modifier = ruleProcedure.Modifier Then Return False
                If ruleProcedure.Gender.Trim.Length > 0 AndAlso Not claimProcedure.Gender = ruleProcedure.Gender Then Return False
                If ruleProcedure.PlaceOfService.Trim.Length > 0 AndAlso Not claimProcedure.PlaceOfService = ruleProcedure.PlaceOfService Then Return False

                If Not claimProcedure.PlanType = ruleProcedure.PlanType Then Return False
                If Not claimProcedure.Provider = ruleProcedure.Provider Then Return False

                ' if rule date is in use check if claim date falls between rule range, which is considered a positive match
                If ruleProcedure.MonthsMin > 0 OrElse ruleProcedure.MonthsMax > 0 Then
                    If (claimProcedure.MonthsMin >= ruleProcedure.MonthsMin AndAlso (claimProcedure.MonthsMax <= ruleProcedure.MonthsMax OrElse ruleProcedure.MonthsMax = 0)) = False OrElse (claimProcedure.MonthsMin = 0 AndAlso claimProcedure.MonthsMax = 0) Then
                        Return False
                    End If
                End If

                If Not claimProcedure.ProcedureCode = PlanController.WildCardProcedure AndAlso Not ruleProcedure.ProcedureCode = PlanController.WildCardProcedure Then
                    If Not claimProcedure.ProcedureCode = ruleProcedure.ProcedureCode Then Return False
                End If

                Return True

            Catch ex As Exception
                Throw
            End Try

        End SyncLock
    End Function
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
        _ProcedureID = Nothing
    End Sub

    Protected Sub New(ByVal procedureID As Integer?)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Constructor that only takes the procedure id
        ' </summary>
        ' <param name="procedureID"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        _ProcedureID = procedureID
    End Sub

    Protected Sub New(ByVal procedureID As Integer?, ByVal billType As String, ByVal diagnosis As String, ByVal modifier As String, ByVal gender As String, ByVal monthsMin As Integer?, ByVal monthsMax As Integer?, ByVal placeOfService As String, ByVal planType As String, ByVal procedureCode As String, ByVal provider As String)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Constructor that sets all class variables
        ' </summary>
        ' <param name="procedureID"></param>
        ' <param name="billType"></param>
        ' <param name="diagnosis"></param>
        ' <param name="modifier"></param>
        ' <param name="gender"></param>
        ' <param name="monthsMin"></param>
        ' <param name="monthsMax"></param>
        ' <param name="placeOfService"></param>
        ' <param name="planType"></param>
        ' <param name="procedureCode"></param>
        ' <param name="providerID"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        _ProcedureID = procedureID
        _BillType = billType
        _Diagnosis = diagnosis
        _Modifier = modifier
        _MonthsMin = monthsMin
        _MonthsMax = monthsMax
        _Gender = gender
        _PlaceOfService = placeOfService
        _PlanType = planType
        _ProcedureCode = procedureCode
        _Provider = provider
        '_ruleSet = ruleSet
    End Sub
#End Region

#Region "Clone"
    Public Function DeepCopy() As Procedure

        Dim BeginTime As Date = UFCWGeneral.NowDate
        Dim ProcedureClone As Procedure
        Dim RuleSetsClone As New Rulesets

        Try

            ProcedureClone = Me.ShallowCopy()

            'For Each RuleSet As RuleSet In CloneHelper.DeepCopy(Me.RuleSets)
            '    RuleSetsClone.Add(RuleSet, False)
            'Next

            If _RuleSets IsNot Nothing AndAlso _RuleSets.Count > 0 Then
                For Each RuleSet As RuleSet In _RuleSets
                    RuleSetsClone.Add(RuleSet.DeepCopy, False)
                Next
            End If

            ProcedureClone.RuleSets = RuleSetsClone

            Return ProcedureClone

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
#End If
        End Try
    End Function

    Public Function ShallowCopy() As Procedure
        Dim BeginTime As Date = UFCWGeneral.NowDate
        Dim ProcedureClone As Procedure

        Try

            ProcedureClone = DirectCast(Me.MemberwiseClone(), Procedure)
            ProcedureClone._RuleSets = Nothing

            Return ProcedureClone

        Catch
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
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




End Class

Public Class ProcedureComparer
    Implements IEqualityComparer(Of Procedure)

    Public Function Equals1(
        ByVal x As Procedure,
        ByVal y As Procedure
        ) As Boolean Implements IEqualityComparer(Of Procedure).Equals

        ' Check whether the compared objects reference the same data.
        If x Is y Then Return True

        'Check whether any of the compared objects is null.
        If x Is Nothing OrElse y Is Nothing Then Return False

        If x Is Nothing Then Throw New ArgumentNullException("xIsNull")

        'add better exception description (2nd procedure has not been initialized... Why?)
        If y Is Nothing Then Throw New ArgumentNullException("yIsNull")

        'Note: When a rule has no value specified for a filter it should be included as a positive result. The rule with the higest weight will be used in a later process
        If y.BillType.Trim.Length > 0 AndAlso Not x.BillType = y.BillType Then Return False
        If y.Diagnosis.Trim.Length > 0 AndAlso Not x.Diagnosis = y.Diagnosis Then Return False
        If y.Modifier.Trim.Length > 0 AndAlso Not x.Modifier = y.Modifier Then Return False
        If y.Gender.Trim.Length > 0 AndAlso Not x.Gender = y.Gender Then Return False
        If y.PlaceOfService.Trim.Length > 0 AndAlso Not x.PlaceOfService = y.PlaceOfService Then Return False

        If Not x.PlanType = y.PlanType Then Return False
        If Not x.Provider = y.Provider Then Return False

        ' if rule date is in use check if claim date falls between rule range, which is considered a positive match
        If y.MonthsMin > 0 OrElse y.MonthsMax > 0 Then
            If (x.MonthsMin >= y.MonthsMin AndAlso (x.MonthsMax <= y.MonthsMax OrElse y.MonthsMax = 0)) = False OrElse (x.MonthsMin = 0 AndAlso x.MonthsMax = 0) Then
                Return False
            End If
        End If

        If Not x.ProcedureCode = PlanController.WildCardProcedure AndAlso Not y.ProcedureCode = PlanController.WildCardProcedure Then
            If Not x.ProcedureCode = y.ProcedureCode Then Return False
        End If

        Return True
    End Function

    Public Function GetHashCode1(
        ByVal proc As Procedure
        ) As Integer Implements IEqualityComparer(Of Procedure).GetHashCode

        ' Check whether the object is null.
        If proc Is Nothing Then Return 0

        ' Get hash code for the ProcedureCode field if it is not null.
        Dim hashProcedureCode As Integer =
            If(proc.ProcedureCode Is Nothing, 0, proc.ProcedureCode.GetHashCode())

        Dim hashBillType As Integer =
            If(proc.BillType Is Nothing, 0, proc.BillType.GetHashCode())

        Dim hashModifier As Integer =
            If(proc.BillType Is Nothing, 0, proc.Modifier.GetHashCode())

        Dim hashGender As Integer =
            If(proc.Gender Is Nothing, 0, proc.Gender.GetHashCode())

        Dim hahsDiagnosis As Integer =
            If(proc.Diagnosis Is Nothing, 0, proc.Diagnosis.GetHashCode())

        Dim hashPlaceOfService As Integer =
            If(proc.PlaceOfService Is Nothing, 0, proc.PlaceOfService.GetHashCode())

        Dim hashPlanType As Integer =
            If(proc.PlanType Is Nothing, 0, proc.PlanType.GetHashCode())

        Dim hashProvider As Integer =
            If(proc.Provider Is Nothing, 0, proc.Provider.GetHashCode())


        ' Calculate the hash code for the product.
        Return hashProcedureCode Xor hashBillType Xor hashModifier Xor hashGender Xor hahsDiagnosis Xor hashPlaceOfService Xor hashPlanType Xor hashProvider
    End Function
End Class