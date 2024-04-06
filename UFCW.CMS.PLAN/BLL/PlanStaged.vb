Option Strict On
Option Explicit On
Imports System
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.PlanStaged
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class represents and handles a Staged Plan
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	1/25/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Class PlanStaged
    Inherits Plan

#Region "Properties and Variables"
    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _Status As Int32
    Private _StagingMasterID As Integer
    Private _Publisher As Publisher
    Private _FromDate As Date
    Private _ToDate As Date
    Private _TotalPlanProcedures As Integer
    Private _TotalEligibleProcedures As Integer
    Private _NewProceduresWithRuleSet As Integer
    Private _NewProceduresWithoutRuleSet As Integer
    Private _ChangedProceduresWithNewRuleSet As Integer
    Private _ChangedProceduresWithoutNewRuleSet As Integer
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets FromDate for Staged Plan
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property FromDate() As Date
        Get
            Return _FromDate
        End Get
        Set(ByVal value As Date)
            If Value = #12:00:00 AM# Then Throw New ArgumentException("From Date requires a valid date")
            _FromDate = Value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets To date of staged plan
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property ToDate() As Date
        Get
            Return _ToDate
        End Get
        Set(ByVal value As Date)
            If Value = #12:00:00 AM# Then Throw New ArgumentException("To Date requires a valid date")
            _ToDate = Value
        End Set
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets Meta Data representing the New Procedure that have a ruleset for this publisher
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    ReadOnly Property NewProceduresWithRuleset() As Integer
        Get
            Return _NewProceduresWithRuleSet
        End Get
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets Meta Data representing the New Procedure that dont have a ruleset for this publisher
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    ReadOnly Property NewProceduresWithoutRuleset() As Integer
        Get
            Return _NewProceduresWithoutRuleSet
        End Get
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets Meta Data representing the Changed Procedure that have a new ruleset for this publisher
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    ReadOnly Property ChangedProceduresWithNewRuleset() As Integer
        Get
            Return _ChangedProceduresWithNewRuleSet
        End Get
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets Meta Data representing the New Procedure that dont have a new ruleset for this publisher
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    ReadOnly Property ChangedProceduresWithoutNewRuleset() As Integer
        Get
            Return Me._changedProceduresWithoutNewRuleSet
        End Get
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets Meta Data representing the Total Procedures availble for publishing
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    ReadOnly Property TotalPlanProcedures() As Integer
        Get
            Return _TotalPlanProcedures
        End Get
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets Meta Data representing the Total amount of procedures that are eligable for publish
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    ReadOnly Property TotalEligibleProcedures() As Integer
        Get
            Return _TotalEligibleProcedures
        End Get
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets the Status of the Plan
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public ReadOnly Property Status() As Int32
        Get
            Return _Status
        End Get
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets the Staging Master id for this plan
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public ReadOnly Property StagingMasterId() As Int32
        Get
            Return _StagingMasterID
        End Get
    End Property
#End Region

#Region "Constructors"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Constructor
    ' </summary>
    ' <param name="planType"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Sub New(ByVal planType As String, ByVal planDescription As String, ByVal planStatus As Integer, ByVal planStagingMasterId As Integer, ByVal constructAsLightweight As Boolean)
        MyBase.New(planDescription, planType, New Procedures)
        _status = planStatus

        If Not constructAsLightweight Then
            _Procedures = PlanStagingDAL.GetStagedProcedures(planType, constructAsLightweight)
        End If
        _StagingMasterID = planStagingMasterId
        _publisher = New Publisher(Me)
    End Sub
#End Region

#Region "Functions"

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets a collection of procedures for the range, modifiers, etc.
    ' </summary>
    ' <param name="planType"></param>
    ' <param name="beginProcedureCode"></param>
    ' <param name="endProcedureCode"></param>
    ' <param name="modifier"></param>
    ' <param name="location"></param>
    ' <param name="diagnosis"></param>
    ' <param name="billType"></param>
    ' <param name="providerID"></param>
    ' <param name="status"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Function GetProcedureRange(ByVal planType As String, ByVal beginProcedureCode As String, ByVal endProcedureCode As String, ByVal modifier As String, ByVal location As String, ByVal diagnosis As String, ByVal billType As String, ByVal provider As String, ByVal status As Integer) As Procedures

        Dim ProceduresCollection As Procedures
        Dim Range As ArrayList = New ArrayList

        Try
            ProceduresCollection = New Procedures

            Range = RangeManager.CreateRange(beginProcedureCode, endProcedureCode)

            '[PW - 8/23/2018]:  Changes the code below to improve performance.   I believe it went from 6 seconds for 1,000 records to less than 1 second for the same records. 
            For I As Integer = 0 To range.Count - 1
                ProceduresCollection.Add(New ProcedureStaged() With {
                .PlanType = planType,
                .ProcedureCode = CType(Range(I), String),
                .Modifier = modifier,
                .PlaceOfService = location,
                .Provider = provider,
                .Diagnosis = diagnosis,
                .BillType = billType
                })
            Next I

            Return proceduresCollection

        Catch ex As Exception

            Throw New ProcedureRangeException("There was a Problem Getting the requested Procedure Range", ex)

        End Try
    End Function

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Indicates if the procedure range is valid
    ' </summary>
    ' <param name="planType"></param>
    ' <param name="beginProcedureCode"></param>
    ' <param name="endProcedureCode"></param>
    ' <param name="procedureCode"></param>
    ' <param name="modifier"></param>
    ' <param name="location"></param>
    ' <param name="diagnosis"></param>
    ' <param name="billType"></param>
    ' <param name="provider"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Function IsProcedureRangeValid(ByVal planType As String, ByVal beginProcedureCode As String, ByVal endProcedureCode As String, ByVal modifier As String, ByVal location As String, ByVal diagnosis As String, ByVal billType As String, ByVal provider As String) As Boolean
        Dim matchingProceduresFound As Boolean

        Try
            matchingProceduresFound = PlanStagingDAL.IsProcedureRangeValid(planType, beginProcedureCode, endProcedureCode, modifier, location, diagnosis, billType, provider)
            Return matchingProceduresFound

        Catch ex As Exception

            Throw New ProcedureRangeException("There was a Problem Validating the Procedure Range", ex)

        End Try
    End Function

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Adds Procedures to the Staging table for the current plan
    ' </summary>
    ' <param name="proceduresCollection"></param>
    ' <param name="userId"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Sub AddNewProcedures(ByVal proceduresCollection As Procedures, ByVal userId As String)
        Dim Status As Integer = 0
        Dim StagingMasterID As Integer
        Dim StagedProc As ProcedureStaged

        Try
            StagingMasterID = PlanStagingDAL.UpdateStagingMaster(PlanType, userId, Status)
            Status = 2

            PlanStagingDAL.AddProcedures(proceduresCollection, StagingMasterID, userId, Status)
            For I As Integer = 0 To proceduresCollection.Count - 1
                StagedProc = DirectCast(proceduresCollection(I), ProcedureStaged)
                _Procedures.Add(StagedProc)
            Next

        Catch ex As Exception

            Throw New PlanException("There was a Problem Adding Procedures to this Plan", ex)

        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' This Sub populates all of the metadata fields
    ' </summary>
    ' <param name="StagedPlanType"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Sub GenerateProcedureMetaData()
        Dim StagedProcedure As ProcedureStaged

        Try
            _newProceduresWithRuleSet = 0
            _totalEligibleProcedures = 0
            _newProceduresWithoutRuleSet = 0
            _changedProceduresWithNewRuleSet = 0
            _totalEligibleProcedures = 0
            _changedProceduresWithoutNewRuleSet = 0

            If _Procedures.Count > 0 Then
                _totalPlanProcedures = _Procedures.Count
                For I As Integer = 0 To _Procedures.Count - 1
                    StagedProcedure = DirectCast(_Procedures(I), ProcedureStaged)
                    If StagedProcedure.NewRuleSets IsNot Nothing Then
                        'check for NEW procedures with and without rulesets applied
                        If StagedProcedure.Status = StatusCodes.[New] AndAlso Not StagedProcedure.NewRuleSets.Count < 1 Then
                            _newProceduresWithRuleSet += 1
                            _totalEligibleProcedures += 1
                        End If
                        If StagedProcedure.Status = StatusCodes.[New] AndAlso StagedProcedure.NewRuleSets.Count < 1 Then
                            _newProceduresWithoutRuleSet += 1
                        End If

                        'check for CHANGED procedures (staged) that have a NEW ruleset
                        If StagedProcedure.Status = StatusCodes.Changed AndAlso Not StagedProcedure.NewRuleSets.Count < 1 Then
                            _changedProceduresWithNewRuleSet += 1
                            _totalEligibleProcedures += 1
                        End If
                        If StagedProcedure.Status = StatusCodes.Changed AndAlso StagedProcedure.NewRuleSets.Count < 1 Then
                            _changedProceduresWithoutNewRuleSet += 1
                        End If
                    End If
                Next
            End If

        Catch ex As Exception

            Throw New BusinessLayerException("There was a Problem Generating MetaData about a Staged Plan", ex)

        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Publish this plan
    ' </summary>
    ' <param name="userId"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Sub Publish(ByVal userId As String)
        Try
            If _FromDate = #12:00:00 AM# Then
                Throw New ConstraintException("Must Set FromDate and ToDate Before Publishing")
            End If

            If _ToDate = #12:00:00 AM# Then
                Throw New ConstraintException("Must Set FromDate and ToDate Before Publishing")
            End If
            _publisher.PublishPlan(Me._fromDate, Me._toDate, Me.Procedures, userId)
        Catch ex As Exception

            Throw New PublishException("There was a Problem Publishing the Staged Plan", ex)

        End Try

    End Sub

#End Region

End Class