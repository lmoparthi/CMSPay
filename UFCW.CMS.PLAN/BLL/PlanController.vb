Option Explicit On
Option Infer On
Option Strict On

Imports System.Configuration
Imports System.Collections.Specialized
Imports Microsoft.Office.Interop

Imports System.Threading.Tasks
Imports System.Threading


''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.PlanController
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This Class Manages Plans
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	1/25/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()>
Public NotInheritable Class PlanController

#Region "Variables"

    Private Shared _TraceParallel As New TraceSwitch("TraceParallel", "Parallel Trace Switch in App.Config", "0")
    Private Shared _TraceCloning As New TraceSwitch("TraceCloning", "Trace Switch in App.Config", "0")
    Private Shared _TraceCaching As New TraceSwitch("TraceCaching", "Trace Switch in App.Config", "0")

    Private Shared _WildCardProcedure As String = ""
    Private Shared _ValidProceduresDT As New DataTable
    Private Shared _ValidModifiersSC As New StringCollection
    Private Shared _ValidPlaceOfServicesSC As New StringCollection
    Private Shared _ValidDiagnosisSC As New StringCollection
    Private Shared _ValidProvidersSC As New StringCollection
    Private Shared _ValidBillTypesSC As New StringCollection
    Private Shared _WildCardProcedures As Procedures
    Private Shared _CachedProcedures As Hashtable
    Private Shared _CachedRuleSets As New Hashtable
    Private Shared _SequenceNumbersDT As DataTable
    Private Shared _GroupingNumbersDT As DataTable
    Private Shared _RuleSetTypesCols As Collection
    Private Shared _GetWildCardProcedureCollectionSyncLock As New Object
    Private Shared _GetActiveProceduresByProcedureCode As New Object
    Private Shared _GetSequenceNumberSyncLock As New Object
#End Region

#Region "Constructor"
    Private Sub New()

    End Sub
#End Region

#Region "Properties"

    Public Shared ReadOnly Property ValidProcedures() As DataTable
        Get
            Try

                Return _ValidProceduresDT

            Catch ex As Exception
                Throw
            End Try
        End Get

    End Property

    Public Shared ReadOnly Property WildCardProcedureCollection() As Procedures
        Get
            Try

                Return _WildCardProcedures

            Catch ex As Exception
                Throw
            End Try
        End Get

    End Property

    Public Shared ReadOnly Property CachedProcedures() As Hashtable
        Get
            Try

                Return _CachedProcedures

            Catch ex As Exception
                Throw
            End Try
        End Get

    End Property

    Public Shared ReadOnly Property CachedRuleSets() As Hashtable
        Get
            Try

                Return _CachedRuleSets

            Catch ex As Exception
                Throw
            End Try
        End Get

    End Property

    Public Shared ReadOnly Property SequenceNumbers() As DataTable
        Get
            Try

                Return _SequenceNumbersDT

            Catch ex As Exception
                Throw
            End Try
        End Get

    End Property

    Public Shared ReadOnly Property GroupingNumbers() As DataTable
        Get
            Try

                Return _GroupingNumbersDT

            Catch ex As Exception
                Throw
            End Try
        End Get

    End Property

    Public Shared ReadOnly Property RuleSetTypes() As Collection
        Get
            Try

                Return _RuleSetTypesCols

            Catch ex As Exception
                Throw
            End Try
        End Get

    End Property

    Public Shared ReadOnly Property WildCardProcedure() As String
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Returns the Wild Card Procedure code
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	9/29/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Try

                If _WildCardProcedure.Length < 1 Then
                    _WildCardProcedure = Convert.ToString(ConfigurationManager.AppSettings("WILD_CARD_PROCEDURE_CODE"))
                End If

                Return _WildCardProcedure

            Catch ex As Exception
                Throw
            End Try
        End Get

    End Property

#End Region

#Region "Public Shared Functions"

    Public Shared Function GetRuleSetPublishDateForPlanTypeSeqNbr(ByVal plan_Type As String, ByVal sequenceNumber As Integer) As Date

        Try

            Return PlanActiveDAL.GetRuleSetPublishDateForPlanTypeSeqNbr(plan_Type, sequenceNumber)

        Catch ex As Exception

            Throw

        End Try

    End Function

    Public Shared Function GetProcedurePropertiesForProcedureID(plan_Type As String, procedureID As Integer) As ProcedureActive

        Dim result As ProcedureActive

        Try
            ' result = PlanActiveDAL.GetProcedurePropertiesForProcedureID(plan_Type, iClaimDerived_ProcID)

            result = PlanActiveDAL.GetActiveProcedure(procedureID)

        Catch ex As Exception

            Throw

        End Try

        Return result

    End Function

    Public Shared Function GetRuleSetIDForProcedureID(ByVal procedureID As Integer?, ByVal ruleSetType As Integer?) As Integer

        Try

            Return PlanActiveDAL.GetRuleSetIDForProcedureID(procedureID, ruleSetType)

        Catch ex As Exception

            Throw

        End Try

    End Function

    Public Shared Function GetRuleSetNameByProcedureID(ByVal procedureID As Integer?, Optional ByVal ruleSetType As Integer? = Nothing) As String

        Try

            Return PlanActiveDAL.GetRuleSetNameForProcedureID(procedureID, ruleSetType)

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function GetRuleSetNameByRulesetID(ByVal ruleSetId As Integer) As String

        Try

            Return PlanActiveDAL.GetRuleSetNameForRulesetID(ruleSetId)

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function GetProcedureActive(ByVal procedureID As Integer?) As ProcedureActive
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="procId"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/21/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim ProcedureActive As ProcedureActive

        Try

#If TRACE Then
            If CInt(_TraceCaching.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If

            If _CachedProcedures Is Nothing Then
                _CachedProcedures = New Hashtable
            End If

            'check if Procedure has been retreived from db
            ProcedureActive = CType(_CachedProcedures(procedureID.ToString()), ProcedureActive)
            If ProcedureActive Is Nothing Then
                ProcedureActive = PlanActiveDAL.GetActiveProcedure(procedureID)
                If ProcedureActive IsNot Nothing Then
                    _CachedProcedures.Add(procedureID.ToString, ProcedureActive)
                End If
            End If

            ' Return CType(ProcedureActive.DeepCopy, ProcedureActive) ' Edit made on 2023-01-09 'return clone of cached data 
            If ProcedureActive IsNot Nothing Then
                Return CType(ProcedureActive.DeepCopy, ProcedureActive) 'return clone of cached data
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceCaching.Level) > 0 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If
            ProcedureActive = Nothing
        End Try

    End Function
    Public Shared Function GetRuleSetEffectiveDatesForPlanRuleSet(ByVal planType As String, ruleSetID As Integer, ByVal ruleSetType As Integer?) As DataTable

        Dim result As DataTable = Nothing

        Try

            result = PlanActiveDAL.GetRuleSetEffectiveDatesForPlanRuleSet(planType, ruleSetID, ruleSetType)

        Catch ex As Exception

            Throw

        End Try

        Return result

    End Function

    Public Shared Function GetProcedureEffectiveDatesForPlanProcedureID(ByVal planType As String, procedureID As Integer, ByVal ruleSetType As Integer?) As DataTable

        Dim result As DataTable = Nothing

        Try

            result = PlanActiveDAL.GetProcedureEffectiveDatesForPlanProcedureID(planType, procedureID, ruleSetType)

        Catch ex As Exception

            Throw

        End Try

        Return result

    End Function


    Public Shared Function GetRuleSetPublishedInformationForPlanSequenceNumber(ByVal planType As String, sequenceNumber As Integer) As DataTable

        Dim result As DataTable = Nothing

        Try

            result = PlanActiveDAL.GetRuleSetPublishedInformationForPlanSequenceNumber(planType, sequenceNumber)

        Catch ex As Exception

            Throw

        End Try

        Return result

    End Function

    Public Shared Sub StageAndPublishFromSpreadsheet(ByRef excelApplication As Excel.Application, ByVal planType As String, ByVal fromDate As Date, ByVal toDate As Date, ByVal userID As String)
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="oXL"></param>
        ' <param name="planType"></param>
        ' <param name="fromDate"></param>
        ' <param name="toDate"></param>
        ' <param name="userId"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/11/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            PlanStagingDAL.StageAndPublishSpreadsheet(excelApplication, planType.ToUpper.Replace("PLAN", "").Trim, fromDate, toDate, userID)
        Catch ex As Exception

            Throw New Exception("Cannot Stage and Publish", ex)
        End Try
    End Sub

    Public Shared Function GetPlanConditionsForFamily(ByVal familyID As Integer, ByVal relevantDate As Date) As Conditions
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="familyId"></param>
        ' <param name="relevantDate"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/31/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

#If TRACE Then
            If CInt(_TraceCaching.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If

            Return PlanActiveDAL.GetConditionsForFamily(familyID, relevantDate)

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceCaching.Level) > 0 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If
        End Try

    End Function

    Public Shared Function GetDistinctConditionsForDOS(ByVal dateOfService As Date) As Conditions

        Try

#If TRACE Then
            If CInt(_TraceCaching.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If

            Return PlanActiveDAL.GetDistinctConditionsForDOS(dateOfService)

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceCaching.Level) > 0 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If
        End Try

    End Function

    Public Shared Function GetDistinctAccumulatorsForDOS(ByVal dateOfService As Date) As DataTable

        Try

#If TRACE Then
            If CInt(_TraceCaching.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If

            Return PlanActiveDAL.GetDistinctAccumulatorsForDOS(dateOfService)

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceCaching.Level) > 0 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If
        End Try

    End Function

    Public Shared Function GetPlanConditionsForDOS(ByVal planType As String, ByVal dateOfService As Date) As Conditions

        Try

#If TRACE Then
            If CInt(_TraceCaching.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If

            Return PlanActiveDAL.GetPlanConditionsForDOS(planType, dateOfService)

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceCaching.Level) > 0 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If
        End Try

    End Function

    Public Shared Function GetPlanConditionsForClaim(ByVal ruleSetID As Integer) As Conditions
        Try

#If TRACE Then
            If CInt(_TraceCaching.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If

            Return PlanActiveDAL.GetConditionsForClaim(ruleSetID)

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceCaching.Level) > 0 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If
        End Try

    End Function

    Public Shared Function GetAllPlanAccumulators() As DataTable
        Try

#If TRACE Then
            If CInt(_TraceCaching.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If

            Return PlanActiveDAL.PlanAccumulators

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceCaching.Level) > 0 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If
        End Try

    End Function

    Public Shared Function GetDistinctConditions() As Conditions
        Try
#If TRACE Then
            If CInt(_TraceCaching.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If

            Return PlanActiveDAL.GetDistinctConditions()

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceCaching.Level) > 0 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""), "TraceCaching" & vbTab)
#End If
        End Try

    End Function

    Public Shared Function ReplaceAccidentGenericWithActualAccidents(ByVal conditions As Conditions, ByVal yearOfAccident As Integer, ByVal accidentAccumulatorEntriesDT As DataTable) As Conditions
        Dim Operand As Decimal = 0D

        Try

            Dim QueryAccidents =
                From Accidents In accidentAccumulatorEntriesDT.AsEnumerable()
                Where Accidents.RowState <> DataRowState.Deleted _
                AndAlso (yearOfAccident >= Year(Accidents.Field(Of Date)("APPLY_DATE")) AndAlso yearOfAccident <= Year(Accidents.Field(Of Date)("APPLY_DATE")))

            conditions = RemoveGenericAccumulator(conditions, Operand)
            conditions = AddNonGenericAccidentAccumulators(QueryAccidents.AsDataView, conditions, yearOfAccident, Operand)

            Return conditions

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetClaimProcedures(ByVal meddtlDT As DataTable, ByVal claimType As String, ByVal medhdrDT As DataTable) As Hashtable
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="claimDetailsTable"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/26/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------


        Dim RuleSetType As Integer?
        Dim GrpCol As Hashtable
        Dim AllCPTIndicator As Hashtable

        Dim ProviderNetwork As String
        Dim PlanType As String

        Dim DR As DataRow
        Dim ProcessedDatesAL As ArrayList
        Dim AllCPTCodes As Boolean = True
        Dim DateToWorkWith As Date
        Dim DateToUse As Date

        Dim GroupNumbersAL As ArrayList

        Try

            GrpCol = New Hashtable
            AllCPTIndicator = New Hashtable

            'get the provider class.
            ProviderNetwork = GetProviderNetwork(meddtlDT)

            RuleSetType = GetRuleSetType(claimType, medhdrDT)

            'make sure it is a valid ruleset type
            If RuleSetType Is Nothing Then
                Throw New ArgumentException("Claim Type not found.  Claim Type must be a valid Ruleset Type.", claimType)
            End If

            PlanType = GetPlanType(meddtlDT)

            ProcessedDatesAL = New ArrayList

            '---------------------------------------------------------------
            'Get the groups by date and get the indicator if they are all cpt codes by date
            'loop through all rows
            For X As Integer = 0 To meddtlDT.Rows.Count - 1

                GroupNumbersAL = New ArrayList

                'find the date
                If meddtlDT.Rows(X)("INCIDENT_DATE") IsNot System.DBNull.Value Then
                    DateToWorkWith = CDate(meddtlDT.Rows(X)("INCIDENT_DATE"))
                ElseIf meddtlDT.Rows(X)("OCC_FROM_DATE") IsNot System.DBNull.Value Then
                    DateToWorkWith = CDate(meddtlDT.Rows(X)("OCC_FROM_DATE"))
                End If

                'if this date has NOT already been looked at
                If Not ProcessedDatesAL.Contains(DateToWorkWith) Then
                    ProcessedDatesAL.Add(DateToWorkWith)

                    For I As Integer = 0 To meddtlDT.Rows.Count - 1
                        'get the date
                        If meddtlDT.Rows(I)("INCIDENT_DATE") IsNot System.DBNull.Value Then
                            DateToUse = CDate(meddtlDT.Rows(I)("INCIDENT_DATE"))
                        ElseIf meddtlDT.Rows(I)("OCC_FROM_DATE") IsNot System.DBNull.Value Then
                            DateToUse = CDate(meddtlDT.Rows(I)("OCC_FROM_DATE"))
                        End If

                        'if the date is the date we are currently working with
                        If DateToWorkWith = DateToUse Then
                            AllCPTCodes = True
                            DR = meddtlDT.Rows(I)
                            If PlanController.IsValidProcedureCode(CStr(DR("PROC_CODE")), DateToUse) Then
                                GroupNumbersAL.Add(GetGroupNumber(PlanType, CStr(DR("PROC_CODE"))))
                                'check to see if this is a CPT code
                                If CStr(DR("PROC_CODE")).Trim.Length < 5 Then
                                    AllCPTCodes = False
                                End If
                            End If
                        End If
                    Next

                    AllCPTIndicator.Add(DateToWorkWith.ToShortDateString, AllCPTCodes)
                    GrpCol.Add(DateToWorkWith.ToShortDateString, GroupNumbersAL)
                End If
            Next

            Return GetProcedureDateHashtable(GrpCol, PlanType, AllCPTIndicator, RuleSetType, ProviderNetwork)

        Catch ex As Exception

            Throw

        Finally

            GrpCol = Nothing
            AllCPTIndicator = Nothing
            DR = Nothing
            ProcessedDatesAL = Nothing
            GroupNumbersAL = Nothing

        End Try
    End Function

    Public Shared Sub AddRuleSetType(ByVal typeName As String)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' This sub adds a ruleset type to the types
        ' </summary>
        ' <param name="typeName"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            GeneralDataDAL.AddRulesetType(typeName)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub AddNewRuleSet(ByVal procedures As Procedures, ByVal userID As String)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Adds a new ruleset to the Staged Procedures
        ' </summary>
        ' <param name="proceduresCollection">The ProcedureCollection object that contains the procedures that have rulesets that need to be added</param>
        ' <param name="userId">The id of the user that is making the change</param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	6/14/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            PlanStagingDAL.AddNewRuleSet(procedures, userID)

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Shared Function GetRulesetTypes() As Collection
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets the ruleset types
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/5/2006	Created
        '     [paulw] 6/11/2007   Created cacheing for rule set types
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            If _RuleSetTypesCols Is Nothing OrElse _RuleSetTypesCols.Count = 0 Then
                _RuleSetTypesCols = GeneralDataDAL.GetRulesetTypes
            End If

            Return _RuleSetTypesCols

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetRulesetTypeID(ByVal ruleSetTypeName As String) As Integer?
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="ruleSetTypeName"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/10/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim RulesetTypesCol As Collection

        Try
            RulesetTypesCol = PlanController.GetRulesetTypes

            For Each RuleSetTypeItem As RuleSetType In RulesetTypesCol
                If RuleSetTypeItem.Name.ToUpper = ruleSetTypeName.Trim.ToUpper Then
                    Return RuleSetTypeItem.Id
                End If
            Next

            Return Nothing

        Catch ex As Exception
            Throw
        Finally

            RulesetTypesCol = Nothing

        End Try

    End Function

    Public Shared Function GetValidProcedures() As DataTable
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets valid procedures
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	6/13/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim DS As DataSet

        Try
            DS = CMSDALFDBMD.RetrieveProcedureValues

            DS.Tables(0).PrimaryKey = New DataColumn() {DS.Tables(0).Columns("PROC_VALUE"), DS.Tables(0).Columns("FROM_DATE"), DS.Tables(0).Columns("THRU_DATE")}

            Return DS.Tables(0)

        Catch ex As Exception
            Throw
        Finally

            DS = Nothing

        End Try

    End Function

    Public Shared Function GetValidModifiers() As StringCollection
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets valid modifiers
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	6/13/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            Return GeneralDataDAL.GetValidModifiers

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetValidPlaceOfServices() As StringCollection
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' gets valid place of services
        ' </summary>
        '     ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	6/13/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            Return GeneralDataDAL.GetValidPlaceOfServices

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function GetValidDiagnosis() As StringCollection
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' gets valid diagnosis codes
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	6/13/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            Return GeneralDataDAL.GetValidDiagnosis

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetValidProviders() As StringCollection
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' gets valid providers
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	6/13/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            Return GeneralDataDAL.GetValidProviders

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function GetValidBillTypes() As StringCollection
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' gets valid bill types
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	6/13/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            Return GeneralDataDAL.GetValidBillTypes

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function GetStagedPlans(ByVal GetAsLightweight As Boolean) As ArrayList
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets all the Staged Plans
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try
            Return PlanStagingDAL.GetStagedPlans(GetAsLightweight)

        Catch ex As Exception

            Throw New PlanException("Cannot Retrieve Staged Plans", ex)

        End Try
    End Function

    Public Shared Function GetNonStagedPlans(ByVal getAsLightweight As Boolean) As ArrayList
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets all NonStaged Plans
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            Return PlanActiveDAL.GetNonStagedPlans(getAsLightweight)
        Catch ex As Exception

            Throw New PlanException("Cannot Retrieve NonStaged Plans", ex)

        End Try
    End Function

    Public Shared Sub StagePlan(ByVal planType As String, ByVal userID As String)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Stages a Plan
        ' </summary>
        ' <param name="planType"></param>
        ' <param name="userId"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim Status As Integer = 0

        Try
            'create Staging Master record
            PlanStagingDAL.StagePlan(planType, userID, Status)
        Catch ex As Exception

            Throw New PlanException("Cannot Retrieve NonStaged Plans", ex)

        End Try
    End Sub

    Public Shared Sub ClearStagedPlan(ByVal stagedPlan As PlanStaged)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Clears the staged plan that is specified
        ' </summary>
        ' <param name="sPlan">The PlanStaged object</param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            If stagedPlan Is Nothing Then Throw New ArgumentNullException("stagedPlan")

            PlanStagingDAL.ClearStagedPlan(stagedPlan.PlanType)

        Catch ex As Exception

            Throw New PlanException("Cannot Retrieve Staged Plans", ex)

        End Try
    End Sub

    Public Shared Function GetStagedPlan(ByVal planType As String, ByVal getAsLightweight As Boolean) As PlanStaged
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Get a Staged Plan
        ' </summary>
        ' <param name="planType"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="planType">The plan type</param>
        ' <param name="getAsLightweight">Send this parameter as true if the desired out come if a light weight version of this plan. A light weight version
        '     only builds the top most layer of the object heirarchy
        ' </param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            Return PlanStagingDAL.GetStagingMaster(planType, getAsLightweight)

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function GetStagedProcedures(ByVal planType As String, ByVal filterParameters As String) As Procedures
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' This function exposes the Query function in the FDBMD.
        ' </summary>
        ' <param name="planType"></param>
        ' <param name="filterParameters"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	2/14/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            Return PlanStagingDAL.GetProcedures(planType, filterParameters)

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function GetStagedRulesets(ByVal planType As String, ByVal getAsLightWeight As Boolean) As RuleSet()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets staged rulesets
        ' </summary>
        ' <param name="planType"></param>
        ' <param name="getAsLightWeight"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	9/15/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            Return PlanStagingDAL.GetStagedRuleSets(planType, getAsLightWeight)

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Sub RemoveRulesetFromStaging(ByVal ruleSetID As Integer)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Removes the ruleset, with the id, from staging, completly
        ' </summary>
        ' <param name="rlSetId">The id of the ruleset to remove</param>
        ' <remarks>
        ' Added in regards to ACR MED-0011
        ' </remarks>
        ' <history>
        ' 	[paulw]	9/15/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            PlanStagingDAL.RemoveRulesetFromStaging(ruleSetID)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub RemoveRulesetFromStaging(ByVal procedureCollection As Procedures, ByVal ruleSetType As Integer)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Removes a ruleset type from the specified procedures in staging
        ' </summary>
        ' <param name="procCollection"></param>
        ' <param name="rlSetType"></param>
        ' <remarks>
        ' Added in regards to ACR MED-0011
        ' </remarks>
        ' <history>
        ' 	[paulw]	9/18/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            PlanStagingDAL.RemoveRulesetFromStaging(procedureCollection, ruleSetType)
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Shared Function GetActiveProceduresByProcedureCode(ByVal planType As String, ByVal procedureCode As String, ByVal dateOfProcedureValidity As Date, ByVal getAsLightWeight As Boolean) As Procedures
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets all Active Procedures by a given procedure Code
        ' </summary>
        ' <param name="planType">The plan type</param>
        ' <param name="procedureCode">The procedure code</param>
        ' <param name="dateOfProcedureValidity">The date that the procedure is valid for in the published (active) table</param>
        ' <param name="getAsLightWeight">True will build the procedure(s) returned as only the top layer of the object heirarchy</param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/7/2006	Created
        '     [paulw] 7/5/2006    Added Date Of Claim parameter
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim sequenceNumber As Integer

        SyncLock _GetWildCardProcedureCollectionSyncLock

            Dim WildProcedures As Procedures

            Try

#If TRACE Then

                If CInt(_TraceParallel.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(2).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(2).GetFileLineNumber.ToString & ")", "TraceCaching" & vbTab)

#End If

                WildProcedures = PlanActiveDAL.GetProcedures(planType, procedureCode, getAsLightWeight, dateOfProcedureValidity)

                If WildProcedures.Count < 1 Then
                    ' A0001
                    ' _SequenceNumbers = Nothing

                    sequenceNumber = GetSequenceNumber(planType, dateOfProcedureValidity)
                    If sequenceNumber = -1 Then
                        If _WildCardProcedures IsNot Nothing Then
                            _WildCardProcedures = Nothing
                        End If
                        If _SequenceNumbersDT IsNot Nothing Then
                            _SequenceNumbersDT = Nothing
                        End If
                    End If

                    WildProcedures.WildCardProceduresAdded = False

                    WildProcedures = PlanController.GetWildCardProcedureCollection(planType, sequenceNumber, dateOfProcedureValidity)

                    If WildProcedures.Count > 0 Then
                        WildProcedures.WildCardProceduresAdded = True
                    End If
                End If

                Return WildProcedures

            Catch ex As Exception
                Throw
            Finally
#If TRACE Then
                If CInt(_TraceParallel.Level) > 0 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(2).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(2).GetFileLineNumber.ToString & ")", "TraceCaching" & vbTab)
#End If
                WildProcedures = Nothing
            End Try

        End SyncLock

    End Function

    Public Shared Function RuleSetExists(ByVal ruleSetName As String, ByVal planType As String) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Will return true if the ruleset name already exists for the same plan
        ' </summary>
        ' <param name="ruleSetName"></param>
        ' <param name="planType"></param>
        ' <returns></returns>
        ' <remarks>
        ' Added for ACR 0025
        ' </remarks>
        ' <history>
        ' 	[paulw]	9/13/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            Return PlanStagingDAL.DoesRulesetExist(ruleSetName, planType)

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function IsValidModifier(ByVal modifier As String) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Determines if the modifier is valid
        ' </summary>
        ' <param name="modifier"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	6/13/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            InitializeValidAttributes()

            Return _ValidModifiersSC.Contains(modifier.Trim)

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function IsValidDiagnosis(ByVal diagnosis As String) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Determines if the diagnosis code is valid
        ' </summary>
        ' <param name="diagnosis"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	6/13/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            InitializeValidAttributes()
            Return _ValidDiagnosisSC.Contains(diagnosis.Trim)

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function IsValidPlaceOfService(ByVal placeOfService As String) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' determines of the place of service is valid
        ' </summary>
        ' <param name="placeOfService"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	6/13/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            InitializeValidAttributes()
            Return _ValidPlaceOfServicesSC.Contains(placeOfService.Trim)

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function IsValidProvider(ByVal provider As String) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' determines if the provider is valid
        ' </summary>
        ' <param name="provider"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	6/13/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            InitializeValidAttributes()
            Return _ValidProvidersSC.Contains(provider.Trim)

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function IsValidProcedureCode(ByVal procedureCode As String, ByVal validDate As Date) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' determines if the procedure is valid
        ' </summary>
        ' <param name="procedureCode"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	9/29/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            InitializeValidAttributes()
            Return _ValidProceduresDT.Select("PROC_VALUE = '" & procedureCode & "' AND '" & validDate & "' >= FROM_DATE AND '" & validDate & "' <= THRU_DATE").Length > 0

        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Shared Function IsValidBillType(ByVal billType As String) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' determines if the billtype is valid
        ' </summary>
        ' <param name="billType"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	6/13/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            InitializeValidAttributes()
            Return _ValidBillTypesSC.Contains(billType.Trim)

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function GetWildCardProcedureCollection(ByVal planType As String, ByVal provider As String, ByVal sequenceNumber As Integer, dateOfService As Date) As Procedures

        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="planType"></param>
        ' <param name="sequenceNumber"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	3/7/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        SyncLock (_GetWildCardProcedureCollectionSyncLock)

            Dim WildCardProcedures As Procedures
            '            Dim ProcedureActive As ProcedureActive
            Dim DC As DataColumn

            Try

#If TRACE Then
                If CInt(_TraceParallel.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(2).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(2).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
#End If
                If DoesPlanSequenceExist(_SequenceNumbersDT, planType, sequenceNumber) = False Then

                    _WildCardProcedures = Nothing
                    _SequenceNumbersDT = Nothing

                Else
                    Debug.Print("Cached data already exists for PlanType: {0} and SequenceNumber: {1}", planType, sequenceNumber)
                End If

                If _WildCardProcedures Is Nothing Then
                    _WildCardProcedures = New Procedures
                End If

                If _WildCardProcedures.Count < 1 Then
                    If _SequenceNumbersDT Is Nothing Then
                        _SequenceNumbersDT = New DataTable

                        DC = New DataColumn("SEQ_NBR", Type.GetType("System.Int32"))
                        _SequenceNumbersDT.Columns.Add(DC)

                        DC = New DataColumn("OCC_FROM_DATE", Type.GetType("System.DateTime"))
                        _SequenceNumbersDT.Columns.Add(DC)

                        DC = New DataColumn("OCC_TO_DATE", Type.GetType("System.DateTime"))
                        _SequenceNumbersDT.Columns.Add(DC)

                        DC = New DataColumn("PLAN_TYPE", Type.GetType("System.String"))
                        _SequenceNumbersDT.Columns.Add(DC)
                    End If

                    ' A0001

                    ' PlanActiveDAL.GetWildCardProceduresAndSequenceNumbers(_WildCardProcedureCollection, _SequenceNumbers)
                    PlanActiveDAL.GetWildCardProceduresAndSequenceNumbersFor_PlanType_And_DateOfService(_WildCardProcedures, _SequenceNumbersDT, planType, dateOfService)

                    If _WildCardProcedures IsNot Nothing Then
                        Debug.Print("Collection: _WildCardProcedureCollection contains: {0} items.", _WildCardProcedures.Count)
                    End If

                    If _SequenceNumbersDT IsNot Nothing AndAlso _SequenceNumbersDT.Rows.Count > 0 Then
                        Debug.Print("DataTable: _SequenceNumbers contains: {0} rows.", _SequenceNumbersDT.Rows.Count)
                    End If
                End If

                'Debug.Print(UFCWGeneral.NowDate.Ticks) LINQ is slower than for loop

                'Dim query = _
                '    From pc In _wildCardProcedureCollection _
                '    Where pc.SequenceNumber = sequenceNumber AndAlso _
                '            pc.PlanType = planType _
                '    Select pc

                'If query.Count > 0 Then
                '    For Each r In query
                '        procCollection.Add(r)
                '    Next
                'End If

                WildCardProcedures = New Procedures

                Parallel.ForEach(_WildCardProcedures.Cast(Of ProcedureActive), Sub(Procedure)
                                                                                   If provider IsNot Nothing Then
                                                                                       If Procedure.SequenceNumber = sequenceNumber AndAlso Procedure.PlanType = planType AndAlso Procedure.Provider = provider Then
                                                                                           WildCardProcedures.Add(Procedure)
                                                                                       End If
                                                                                   Else
                                                                                       If Procedure.SequenceNumber = sequenceNumber AndAlso Procedure.PlanType = planType Then
                                                                                           WildCardProcedures.Add(Procedure)
                                                                                       End If
                                                                                   End If
                                                                               End Sub)


                'restrict procedure collection to those specific to the current plan
                'For I As Integer = 0 To _WildCardProcedureCollection.Count - 1
                '    ProcedureActive = CType(_WildCardProcedureCollection(I), ProcedureActive)
                '    If ProcedureActive.SequenceNumber = sequenceNumber AndAlso ProcedureActive.PlanType = planType Then
                '        Procedures.Add(ProcedureActive)
                '    End If
                'Next

                Return WildCardProcedures

            Catch ex As Exception
                Throw New ConvertDataException("Cannot Get WildCards" & ex.ToString, ex)
            Finally
#If TRACE Then
                If CInt(_TraceParallel.Level) > 0 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(2).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(2).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
#End If

                WildCardProcedures = Nothing
                'ProcedureActive = Nothing

            End Try

        End SyncLock

    End Function

    Public Shared Function DoesPlanSequenceExist(dt As DataTable, planType As String, sequenceNumber As Integer) As Boolean
        If dt Is Nothing Then
            Return False
        End If

        If dt.Rows.Count = 0 Then
            Return False
        End If

        Dim matchingRows() As DataRow = dt.Select($"SEQ_NBR = {sequenceNumber} AND PLAN_TYPE = '{planType}'")
        Return matchingRows.Length > 0
    End Function

    Public Shared Function GetWildCardProcedureCollection(ByVal planType As String, ByVal sequenceNumber As Integer, ByVal dateOfService As Date) As Procedures

        ' A0001

        Return GetWildCardProcedureCollection(planType, Nothing, sequenceNumber, dateOfService)

    End Function

    Public Shared Function GetRuleset(ByVal ruleSetID As Integer) As RuleSet
        Dim ProceduresEnum As IDictionaryEnumerator
        Dim ProcedureRule As Procedure

        Try
            ProceduresEnum = _CachedProcedures.GetEnumerator

            While ProceduresEnum.MoveNext
                ProcedureRule = CType(ProceduresEnum.Value, Procedure)
                For Each ProcedureRuleSet As RuleSet In ProcedureRule.RuleSets
                    If ProcedureRuleSet.RulesetID = ruleSetID Then Return ProcedureRuleSet
                Next
            End While

            For I As Integer = 0 To _CachedRuleSets.Count - 1
                If CType(_CachedRuleSets(I), RuleSet).RulesetID = ruleSetID Then
                    Return CType(_CachedRuleSets(I), RuleSet)
                End If
            Next

            If _CachedRuleSets(ruleSetID) IsNot Nothing Then
                Return CType(_CachedRuleSets(ruleSetID), RuleSet)
            End If

            _CachedRuleSets.Add(ruleSetID, PlanActiveDAL.GetRuleset(ruleSetID))
            Return CType(_CachedRuleSets(ruleSetID), RuleSet)

        Catch ex As Exception
            Throw
        Finally
            ProceduresEnum = Nothing
            ProcedureRule = Nothing

        End Try

    End Function

#End Region

#Region "Private Shared Functions"

    Public Shared Property BEST_MATCH_RESULT_CLAIM_ID As Integer

    'Public Shared Event OnBestMatchProcedureFound(sender As Object, e As ProcedureMatchedEventArgs)
    'Private Shared Sub FireOnBestMatchProcedureFound(e As ProcedureMatchedEventArgs)
    '    RaiseEvent OnBestMatchProcedureFound(GetType(PlanController), e)
    'End Sub

    Private Shared Function GetProcedureDateHashtable(ByVal groupCol As Hashtable, ByVal planType As String, ByVal allCPTIndicator As Hashtable, ByVal ruleSetType As Integer?, ByVal provider As String) As Hashtable
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="grpCol"></param>
        ' <param name="planType"></param>
        ' <param name="allCptIndicator"></param>
        ' <param name="rlSetType"></param>
        ' <param name="provider"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	11/7/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim ProcedureActive As ProcedureActive
        Dim ProcedureDates As Hashtable
        Dim Enumerator As IDictionaryEnumerator
        Dim GrpNumbersAL As ArrayList

        Try
            ProcedureDates = New Hashtable
            Enumerator = groupCol.GetEnumerator

            While Enumerator.MoveNext

                GrpNumbersAL = CType(Enumerator.Value, ArrayList)
                ProcedureActive = Nothing

                If GetNumberOfInstancesInArray(GrpNumbersAL, 2) > 0 AndAlso GetNumberOfInstancesInArray(GrpNumbersAL, 1) < 1 AndAlso GetNumberOfInstancesInArray(GrpNumbersAL, 3) < 1 Then 'if there are any from group 1
                    ProcedureActive = PlanActiveDAL.GetActiveProcedure(planType, "HOSP1", provider, CDate(Enumerator.Key), CInt(ruleSetType))

                ElseIf GetNumberOfInstancesInArray(GrpNumbersAL, 4) > 0 AndAlso CBool(allCPTIndicator(Enumerator.Key)) Then 'if all groups are group 2
                    ProcedureActive = PlanActiveDAL.GetActiveProcedure(planType, "HOSP1", provider, CDate(Enumerator.Key), CInt(ruleSetType))
                End If

                ProcedureDates.Add(Enumerator.Key, ProcedureActive)

            End While

            Return ProcedureDates

        Catch ex As Exception
            Throw
        Finally

            ProcedureActive = Nothing
            ProcedureDates = Nothing
            Enumerator = Nothing
            GrpNumbersAL = Nothing

        End Try

    End Function

    Private Shared Function GetGroupNumber(ByVal planType As String, ByVal procedureCode As String) As Integer
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="planType"></param>
        ' <param name="procedureCode"></param>
        ' <param name="billType"></param>
        ' <returns></returns>
        ' <remarks>
        ' This function may not belong in this class but it can be refactored into
        '  another class at a later date.  this class just makes the most sense to me right now
        '   -pw
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/26/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DRs() As DataRow

        Try

            If _GroupingNumbersDT Is Nothing Then
                _GroupingNumbersDT = PlanActiveDAL.GetActiveProcedureGroupingNumbers
            End If

            DRs = _GroupingNumbersDT.Select("PLAN_TYPE = '" & planType & "' AND PROC_CODE = '" & procedureCode & "'")

            If DRs.Length < 1 Then
                Return 0
            Else
                Return CInt(DRs(0)("GROUP_NUMBER"))
            End If

        Catch ex As Exception
            Throw
        Finally

            DRs = Nothing

        End Try

    End Function

    Private Shared Function GetProviderNetwork(ByVal claimDetailsDT As DataTable) As String
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="claimDetailsTable"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	11/7/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            If CBool(claimDetailsDT.Rows(0)("NON_PAR_SW")) Then
                Return "N"
            ElseIf CBool(claimDetailsDT.Rows(0)("OUT_OF_AREA_SW")) Then
                Return "O"
            Else
                Return "P"
            End If

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function GetRuleSetTypeName(ByVal claimType As String, ByVal medhdr As DataTable) As String
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' gets the friendly name of a column from the config file
        ' </summary>
        ' <param name="claimType"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	10/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim RuleSetTypeName As String

        Try

            RuleSetTypeName = claimType.Replace("+", "Plus").Replace("EMPLOYEE CLAIMS - ", "").ToString.ToUpper.Replace(" ", "_").Trim

            If RuleSetTypeName.ToUpper.Contains("VISION") Then
                Return "Vision"
            End If

            If medhdr IsNot Nothing AndAlso medhdr.Rows.Count > 0 Then
                If CBool(medhdr.Rows(0)("CHIRO_SW")) Then
                    Return "Chiropractic"
                End If
            End If

            Return If(CType(ConfigurationManager.GetSection("RuleSetTypes"), IDictionary)(RuleSetTypeName) Is Nothing, Nothing, CStr(CType(ConfigurationManager.GetSection("RuleSetTypes"), IDictionary)(RuleSetTypeName)))

        Catch ex As Exception

            Throw

        End Try
    End Function

    Public Shared Function GetRuleSetType(ByVal claimType As String, ByVal medhdrDT As DataTable) As Integer?
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="claimDetailsTable"></param>
        ' <param name="claimType"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	11/7/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        'get the ruleset type based off the claim/doc type

        Dim RuleSetTypesCol As Collection
        Dim RuleSetTypeName As String

        Try
            RuleSetTypesCol = PlanController.GetRulesetTypes()
            RuleSetTypeName = GetRuleSetTypeName(claimType, medhdrDT)
            If RuleSetTypeName IsNot Nothing Then
                RuleSetTypeName = RuleSetTypeName.ToUpper
            End If

            For Each RuleSetTypeItem As RuleSetType In RuleSetTypesCol
                If RuleSetTypeItem.Name.ToUpper.Trim = RuleSetTypeName Then
                    Return RuleSetTypeItem.Id
                End If
            Next

            Return Nothing

        Catch ex As Exception
            Throw
        Finally
            RuleSetTypesCol = Nothing
        End Try
    End Function



    Public Shared Function GetPlanType(ByVal claimDetailsDT As DataTable) As String

        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="claimDetailsTable"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	11/7/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            If claimDetailsDT.Rows.Count > 0 Then
                Return CStr(claimDetailsDT.Rows(0)("MED_PLAN"))
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Shared Function GetNumberOfInstancesInArray(ByVal instancesArray As ArrayList, ByVal comparisonObject As Object) As Integer
        'returns the number of instances matching the obj in the supplied Array
        Dim Cnt As Integer = 0

        Try

            For I As Integer = 0 To instancesArray.Count - 1
                If instancesArray(I).Equals(comparisonObject) Then
                    Cnt += 1
                End If
            Next

            Return Cnt

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Shared Sub InitializeValidAttributes()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets all valid attributes from the DB
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	7/18/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            'see if it has been initialized yet.  if it has not, then get them all from the db
            If _ValidModifiersSC.Count < 1 Then
                _ValidModifiersSC = PlanController.GetValidModifiers
                _ValidPlaceOfServicesSC = PlanController.GetValidPlaceOfServices
                _ValidDiagnosisSC = PlanController.GetValidDiagnosis
                _ValidProvidersSC = PlanController.GetValidProviders
                _ValidBillTypesSC = PlanController.GetValidBillTypes
            End If

            If _ValidProceduresDT.Rows.Count < 1 Then
                _ValidProceduresDT = PlanController.GetValidProcedures()
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Friend Shared Function GetWildCardSequenceNumberSetForPlanTypeAndDateOfService(planType As String, dateOfService As Date) As DataTable

        Dim Result As DataTable

        ' A0001

        Try

            Result = PlanActiveDAL.GetWildCardSequenceNumberSetForPlanTypeAndDateOfService(planType, dateOfService)

            Return Result

        Catch ex As Exception

            Throw

        End Try

    End Function
    Public Shared Function GetMaxSequenceNumberDAL(ByVal planType As String, ByVal dateOfService As Date) As Integer

        Dim Result As Integer

        ' A0001

        Try

            Result = PlanActiveDAL.GetMaxSequenceNumber(planType, dateOfService)

            Return Result

        Catch ex As Exception

            Throw

        End Try

    End Function

    Public Shared Function GetSequenceNumber(ByVal planType As String, ByVal relevantDate As Date) As Integer


        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="planType"></param>
        ' <param name="relevantDate"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	3/7/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DRs() As DataRow
        Dim DC As DataColumn


        Try
            If _SequenceNumbersDT Is Nothing Then
                _SequenceNumbersDT = New DataTable
                DC = New DataColumn("SEQ_NBR", GetType(System.Int32))
                _SequenceNumbersDT.Columns.Add(DC)

                DC = New DataColumn("OCC_FROM_DATE", GetType(System.DateTime))
                _SequenceNumbersDT.Columns.Add(DC)

                DC = New DataColumn("OCC_TO_DATE", GetType(System.DateTime))
                _SequenceNumbersDT.Columns.Add(DC)

                DC = New DataColumn("PLAN_TYPE", GetType(System.String))
                _SequenceNumbersDT.Columns.Add(DC)
            End If

            SyncLock _GetSequenceNumberSyncLock
                ' PlanActiveDAL.GetWildCardProceduresAndSequenceNumbers(_WildCardProcedureCollection, _SequenceNumbers)
                PlanActiveDAL.GetWildCardProceduresAndSequenceNumbersFor_PlanType_And_DateOfService(_WildCardProcedures, _SequenceNumbersDT, planType, relevantDate)

            End SyncLock

            DRs = _SequenceNumbersDT.Select("PLAN_TYPE = '" & planType & "' AND '" & relevantDate & "' >= OCC_FROM_DATE AND '" & relevantDate & "' <= OCC_TO_DATE", "SEQ_NBR DESC")

            Return If(DRs.Length < 1, -1, CInt(DRs(0)("SEQ_NBR")))

        Catch ex As Exception

            Throw New ConvertDataException("Cannot Get Sequence Number Structure", ex)

        Finally
            DRs = Nothing
        End Try
    End Function

    Private Shared Function RemoveGenericAccumulator(ByVal conditions As Conditions, ByRef operand As Decimal) As Conditions
        Dim AccumCondition As Condition

        Try

            For I As Integer = conditions.Count - 1 To 0 Step -1
                AccumCondition = conditions(I)
                If AccumCondition.AccumulatorName.StartsWith("AC") Then
                    If Not operand = 0 Then operand = AccumCondition.Operand
                    conditions.RemoveAt(I)
                End If
            Next

            Return conditions

        Catch ex As Exception
            Throw
        Finally
            AccumCondition = Nothing
        End Try

    End Function

    Private Shared Function AddNonGenericAccidentAccumulators(ByVal dv As DataView, ByVal conditions As Conditions, ByVal year As Integer, ByVal operand As Decimal) As Conditions
        Dim AccumCondition As Condition

        Try

            For I As Integer = 0 To dv.Count - 1
                AccumCondition = New Condition
                AccumCondition.AccumulatorName = CStr(dv(I)("ACCUM_NAME"))
                AccumCondition.Direction = DateDirection.Reverse
                AccumCondition.Duration = 1
                AccumCondition.DurationType = DateType.Years
                AccumCondition.Operand = operand
                conditions.Add(AccumCondition)
            Next

            Return conditions

        Catch ex As Exception
            Throw
        Finally
            AccumCondition = Nothing
        End Try

    End Function
#End Region

End Class