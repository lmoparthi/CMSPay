Option Infer On
Option Strict On

Imports System.Configuration
Imports System.Data
Imports System.Collections.Generic


Public Delegate Sub AccumulatorOverrideUIDelegate(claimAccumulatorManager As MemberAccumulatorManager, claimDS As DataSet, ByRef claimBinder As MedicalBinder)

Public Delegate Sub AddAlertsDelegate(ByVal alertManagerRowFields As Object())
Public Delegate Sub SelectAccidentUIDelegate(claimAccumulatorManager As MemberAccumulatorManager, claimDS As DataSet, meddtlCurrentRowsDT As DataTable, ByRef accumIDForAccident As Integer?, ByRef accumNameForAccident As String, ByRef claimIDForAccident As Integer?, ByRef dateForAccident As Date?)
Public Delegate Sub SharedInterfacesMessageDelegate(msg As String)

Public NotInheritable Class ClaimProcessor


    Private Shared _PossibleAccident As String = ConfigurationManager.AppSettings("PossibleAccident")
    Private Shared _MaternityRelated As String = ConfigurationManager.AppSettings("MaternityRelated")
    Private Shared _PreventativeReview As String = ConfigurationManager.AppSettings("PreventativeReview")
    Private Shared _CaseManagement As String = ConfigurationManager.AppSettings("CaseManagement")
    Private Shared _OtherInsurance As String = ConfigurationManager.AppSettings("OtherInsurance")

    Private Shared _ChemoExclusionProcedures As String()
    Private Shared _PTSTExclusionProcedures As String()
    Private Shared _PWOExclusionProcedures As List(Of String)
    Private Shared _RuleSetType As Integer? = PlanController.GetRulesetTypeID("General")
    Private Shared _TraceParallel As New TraceSwitch("TraceParallel", "Parallel Trace Switch in App.Config", "0")
    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
    Private Sub New()

    End Sub

    Public Shared Sub LoadClaimAlerts(ByRef claimDS As DataSet, ByRef claimAlertManager As AlertManagerCollection)

        Dim COBDT As DataTable
        Dim FamilyID As Integer
        Dim ClaimMasterDR As DataRow
        Dim ProviderDR As DataRow

        Try

            ClaimMasterDR = claimDS.Tables("CLAIM_MASTER").Rows(0)

            DetermineElectronicAlert(claimDS, claimAlertManager)

            'Inhouse created JAA Claim
            If ClaimMasterDR("EDI_SOURCE").ToString.Trim = "INHOUSE" AndAlso ClaimMasterDR("EDI_STATUS").ToString.Trim = "RECEIVED" Then
                claimAlertManager.AddAlertRow(New Object() {"Inhouse created JAA Claim", 0, "Header", 20})
            End If

            'Invalid Provider
            If Not IsDBNull(ClaimMasterDR("PROV_NAME")) AndAlso ClaimMasterDR("PROV_NAME").ToString.Trim = "***INVALID PROVIDER***" Then
                claimAlertManager.AddAlertRow(New Object() {"Invalid Provider", 0, "Header", 30})
            End If

            'Provider suspended
            If CBool(If(IsDBNull(ClaimMasterDR("SUSPEND_SW")), False, ClaimMasterDR("SUSPEND_SW"))) Then
                claimAlertManager.AddAlertRow(New Object() {"Provider is Suspended", 0, "Header", 30})
            End If

            'Provider suspended Address
            If CBool(ClaimMasterDR("PROV_ADDRESS_SUSPEND_SW")) Then
                claimAlertManager.AddAlertRow(New Object() {"Provider has Suspended Address", 0, "Header", 30})
            End If

            'DocType contradicts Employee security
            If ClaimMasterDR("DOC_TYPE").ToString.ToUpper.Contains("EMPLOYEE") AndAlso Not CBool(ClaimMasterDR("TRUST_SW")) Then
                claimAlertManager.AddAlertRow(New Object() {"Employee Document Type is not valid for NON UFCW Employee", 0, "Header", 30})
            End If

            'Provider Last Name = Patient's Last Name
            If Not IsDBNull(ClaimMasterDR("PROV_NAME")) AndAlso Not IsDBNull(ClaimMasterDR("PAT_LNAME")) AndAlso CStr(ClaimMasterDR("PROV_NAME")).StartsWith(CStr(ClaimMasterDR("PAT_LNAME"))) Then
                claimAlertManager.AddAlertRow(New Object() {"Provider's Name Matches Patient's Name", 0, "Header", 20})
            End If

            'Provider Alerts
            If Not IsDBNull(ClaimMasterDR("PROV_ID")) AndAlso Not IsDBNull(ClaimMasterDR("PROV_NAME")) AndAlso ClaimMasterDR("PROV_NAME").ToString.Trim <> "***INVALID PROVIDER***" Then

                If claimDS.Tables("PROVIDER").Rows.Count < 1 Then
                    claimAlertManager.AddAlertRow(New Object() {"Provider Not Found (Prov Alert)", 0, "ProvAlert", 30})
                Else
                    ProviderDR = claimDS.Tables("PROVIDER").Rows(0)

                    If ProviderDR IsNot Nothing AndAlso Not IsDBNull(ProviderDR("ALERT")) AndAlso CStr(ProviderDR("ALERT")).Trim <> "" Then
                        claimAlertManager.AddAlertRow(New Object() {ProviderDR("DESCRIPTION").ToString & " (Prov Alert)", 0, "ProvAlert", 30})
                    End If
                End If
            End If

            'COB Alerts

            'If UFCWGeneral.IsNullDateHandler(ClaimMasterDR("DATE_OF_SERVICE")).HasValue Then
            '    Dim MatchingRows = From row As DataRow In claimDS.Tables("ELIGIBILITY").AsEnumerable()
            '                       Where row.Field(Of Date)("ELIG_PERIOD").Month = UFCWGeneral.IsNullDateHandler(ClaimMasterDR("DATE_OF_SERVICE")).Value.Month And
            '                         row.Field(Of Date)("ELIG_PERIOD").Year = UFCWGeneral.IsNullDateHandler(ClaimMasterDR("DATE_OF_SERVICE")).Value.Year And
            '                        row.Field(Of Decimal)("DUAL_COVERAGE_SW") = 1
            '    If MatchingRows IsNot Nothing AndAlso MatchingRows.Any() Then
            '        COBDT = MatchingRows.CopyToDataTable
            '    End If
            'End If
            'If COBDT Is Nothing  Then
            '         COBDT = claimDS.Tables("FUNDDUALCOVERAGE")
            '     End If
            COBDT = claimDS.Tables("FUNDDUALCOVERAGE")
            If COBDT IsNot Nothing AndAlso COBDT.Rows.Count > 1 Then
                claimAlertManager.AddAlertRow(New Object() {"Possible UFCW Dual Coverage (Family ID: " & COBDT.Rows(0)("FAMILY_ID").ToString & If(COBDT.Rows(0)("MEDICAL_PLAN").ToString.Trim = "0", " - Ineligible)", " Plan: " & COBDT.Rows(0)("PLAN_TYPE").ToString & " )"), 0, "Header", 20})

                If UFCWGeneral.IsNullShortHandler(ClaimMasterDR("RELATION_ID")) > 0 Then
                    Select Case CType(COBDT.Rows(0)("Relation"), String).ToUpper
                        Case "W", "S", "P"
                        Case Else
                            FamilyID = CInt(CMSDALFDBMD.RetrieveCOBPrime(CInt(ClaimMasterDR("FAMILY_ID")), CInt(COBDT.Rows(0)("FAMILY_ID"))))
                            If Not IsDBNull(FamilyID) AndAlso CInt(ClaimMasterDR("FAMILY_ID")) <> FamilyID Then
                                claimAlertManager.AddAlertRow(New Object() {"Current Participant is identified as COB(Secondary). DOB (Family ID: " & COBDT.Rows(0)("FAMILY_ID").ToString & ") is earlier than current Participant.", 0, "Header", 20})
                            End If
                    End Select
                End If
            End If

        Catch ex As Exception

            Throw
        End Try
    End Sub

    Public Shared Sub LoadDetailLineAccumulators(ByRef sharedInterfacesMessage As SharedInterfacesMessageDelegate, ByRef selectAccidentUI As SelectAccidentUIDelegate, ByVal accumulatorCheckIfOverrideNeededUI As AccumulatorOverrideUIDelegate, ByRef highestAccumulatorEntryIdForFamily As Integer, ByRef claimBinder As MedicalBinder, ByRef claimAccumulatorManager As MemberAccumulatorManager, ByRef claimDS As DataSet, ByRef detailAccumulatorsDT As DataTable, ByRef accumulatorsDT As DataTable, ByRef claimAlertManager As AlertManagerCollection)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' performs the main processing using the processing engine, builds accumulators,
        ' and reports accumulator related alerts
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim ProcedureCollection As Procedures
        Dim Procedure As ProcedureActive
        Dim Procedures As Hashtable

        Dim ProceduresQ As System.Collections.Generic.Queue(Of ProcedureActive)

        Dim PercentMatch As Integer

        Dim ClaimBinderItem As MedicalBinderItem
        Dim BinderItemCount As Integer? = 0

        Dim DocClass As String

        Dim RuleSetTypeName As String
        Dim MeddtlCurrentRowsDT As DataTable
        Dim MeddtlNonMergedRowsDT As DataTable

        Dim OrigTotalPricedAmt As Decimal = 0

        Try

            If claimAlertManager Is Nothing Then claimAlertManager = New AlertManagerCollection

            DocClass = CStr(claimDS.Tables("CLAIM_MASTER").Rows(0)("DOC_CLASS"))

            If detailAccumulatorsDT IsNot Nothing Then
                detailAccumulatorsDT.Rows.Clear()
            End If

            If accumulatorsDT IsNot Nothing Then
                accumulatorsDT.Rows.Clear()
            End If

            'attempt to find specific
            RuleSetTypeName = PlanController.GetRuleSetTypeName(claimDS.Tables("CLAIM_MASTER").Rows(0)("DOC_TYPE").ToString, claimDS.Tables("MEDHDR"))
            If RuleSetTypeName <> "" Then
                _RuleSetType = PlanController.GetRulesetTypeID(RuleSetTypeName)
            Else
                RuleSetTypeName = "General"
                _RuleSetType = PlanController.GetRulesetTypeID(RuleSetTypeName)
            End If

            MeddtlCurrentRowsDT = claimDS.Tables("MEDDTL").Clone
            MeddtlNonMergedRowsDT = claimDS.Tables("MEDDTL").Clone

            MeddtlNonMergedRowsDT.BeginLoadData()
            MeddtlCurrentRowsDT.BeginLoadData()

            For Each DR As DataRow In claimDS.Tables("MEDDTL").Rows
                If DR.RowState <> DataRowState.Deleted Then
                    DR.EndEdit() 'required to clone changed pricing info changed during preprocessing
                    If DR("STATUS").ToString.Trim <> "MERGED" Then
                        MeddtlNonMergedRowsDT.ImportRow(DR)
                    End If
                    MeddtlCurrentRowsDT.ImportRow(DR)
                End If
            Next

            MeddtlNonMergedRowsDT.EndLoadData()
            MeddtlCurrentRowsDT.EndLoadData()

            If claimDS.Tables("MEDDTL").Rows.Count > 0 Then

                If claimAccumulatorManager Is Nothing Then 'retrieve accumulators for patient
                    claimAccumulatorManager = New MemberAccumulatorManager(CShort(claimDS.Tables("CLAIM_MASTER").Rows(0)("RELATION_ID")), CInt(claimDS.Tables("CLAIM_MASTER").Rows(0)("FAMILY_ID")))
                End If

                claimAccumulatorManager.RefreshAccumulatorSummariesForMember()

                highestAccumulatorEntryIdForFamily = claimAccumulatorManager.GetHighestEntryIdForFamily

                claimBinder = CType(BinderFactory.CreateBinder(CInt(claimDS.Tables("CLAIM_MASTER").Rows(0)("CLAIM_ID")), DocClass, _RuleSetType), MedicalBinder)
                claimBinder.BinderAccumulatorManager = claimAccumulatorManager
                claimBinder.BinderAlertManager = claimAlertManager

                If accumulatorCheckIfOverrideNeededUI IsNot Nothing Then
                    accumulatorCheckIfOverrideNeededUI(claimAccumulatorManager, claimDS, claimBinder)
                End If

                Dim QueryMEDDTL =
                    From MEDDTL In claimDS.Tables("MEDDTL").AsEnumerable
                    Where MEDDTL.RowState <> DataRowState.Deleted
                    Order By MEDDTL.Field(Of Short)("LINE_NBR")
                    Select MEDDTL

                For Each DR As DataRow In QueryMEDDTL.AsEnumerable

                    If sharedInterfacesMessage IsNot Nothing Then sharedInterfacesMessage.Invoke("Line " & DR("LINE_NBR").ToString & " of " & MeddtlCurrentRowsDT.Rows.Count.ToString & " is being compared against payment rules.")

                    ProcedureCollection = New Procedures

                    If CStr(DR("STATUS")).ToUpper <> "DENY" AndAlso CStr(DR("STATUS")).ToUpper <> "MERGED" Then 'Don't process Denies or Merged items

                        If Not IsDBNull(DR("MED_PLAN")) AndAlso DR("MED_PLAN").ToString.Trim.Length > 0 AndAlso Not IsDBNull(DR("PROC_CODE")) AndAlso DR("PROC_CODE").ToString.Trim.Length > 0 Then
                            Try

                                BinderItemCount = ManageProcedures(claimAlertManager, DR, BinderItemCount, PercentMatch, Procedure, Procedures, ProceduresQ, ProcedureCollection, claimBinder, ClaimBinderItem, claimDS, MeddtlNonMergedRowsDT)

                            Catch ex As ArgumentException

                                claimAlertManager.AddAlertRow(New Object() {"Line " & DR("LINE_NBR").ToString & ": " & Replace(ex.Message, vbCrLf, " ").Trim.ToString, DR("LINE_NBR").ToString, "Detail", 30})

                            Catch ex As Exception
                                Throw
                            End Try

                        Else
                            If IsDBNull(DR("MED_PLAN")) OrElse DR("MED_PLAN").ToString.Trim.Length = 0 Then
                                claimAlertManager.AddAlertRow(New Object() {"Line " & DR("LINE_NBR").ToString & ": Missing Plan - Unable to Process Rules", DR("LINE_NBR").ToString, "Detail", 30})
                            Else
                                claimAlertManager.AddAlertRow(New Object() {"Line " & DR("LINE_NBR").ToString & ": Missing Procedure - Unable to Process Rules", DR("LINE_NBR").ToString, "Detail", 30})
                            End If

                        End If
                    ElseIf CStr(DR("STATUS")).ToUpper = "MERGED" Then
                        claimAlertManager.AddAlertRow(New Object() {"Line " & DR("LINE_NBR").ToString & ": Consolidated Lines are marked as MERGED", DR("LINE_NBR").ToString, "Detail", 5})
                    ElseIf CStr(DR("STATUS")).ToUpper = "DENY" Then
                        ProcessDenyStatus(claimAlertManager, DR)
                    End If

                Next

                If BinderItemCount IsNot Nothing AndAlso BinderItemCount > 0 AndAlso claimBinder IsNot Nothing Then

                    If claimDS.Tables("MEDHDR").Rows.Count > 0 Then
                        If Not IsDBNull(claimDS.Tables("MEDHDR").Rows(0)("TOT_PRICED_AMT")) Then
                            OrigTotalPricedAmt = CDec(claimDS.Tables("MEDHDR").Rows(0)("TOT_PRICED_AMT"))
                        End If

                    End If

                    'claims priced as Summary "(S)" require pricing dispersal, this is no longer limited to Hospital
                    If OrigTotalPricedAmt <> 0 AndAlso (claimDS.Tables("MEDHDR").Rows(0)("PRICED_BY").ToString.Contains(" (S)")) Then
                        'disperse pricing across line items if it was received as summary
                        DisperseSummaryPricing(claimBinder, claimAccumulatorManager, _RuleSetType, OrigTotalPricedAmt, MeddtlNonMergedRowsDT, claimDS)
                    End If

                    Try

                        ProcessBinder(claimBinder, highestAccumulatorEntryIdForFamily, claimAccumulatorManager, claimDS, MeddtlCurrentRowsDT, DocClass, Procedure, detailAccumulatorsDT, accumulatorsDT, claimAlertManager, selectAccidentUI)

                    Catch ex As BinderException

                        claimAlertManager.AddAlertRow(New Object() {"Unable to Process, validate claim data before trying again.", 0, "Header", 30})
                        claimAlertManager.AddAlertRow(New Object() {ex.InnerException.Message.Substring(0, ex.InnerException.Message.IndexOf(Environment.NewLine)), 0, "Header", 30})

                    Catch ex As Exception
                        Throw
                    End Try

                Else 'bin count <=0
                    'no binder items
                    'load accum tables
                    claimAccumulatorManager.RefreshAccumulatorSummariesForMember()

                    If detailAccumulatorsDT IsNot Nothing Then
                        detailAccumulatorsDT.Dispose()
                        detailAccumulatorsDT = Nothing
                    End If
                End If
            Else

                If claimAccumulatorManager Is Nothing Then
                    claimAccumulatorManager = New MemberAccumulatorManager(CShort(claimDS.Tables("CLAIM_MASTER").Rows(0)("RELATION_ID")), CInt(claimDS.Tables("CLAIM_MASTER").Rows(0)("FAMILY_ID")))
                End If

                claimBinder = CType(BinderFactory.CreateBinder(CInt(claimDS.Tables("CLAIM_MASTER").Rows(0)("CLAIM_ID")), DocClass, _RuleSetType), MedicalBinder)
                claimBinder.BinderAccumulatorManager = claimAccumulatorManager
                claimBinder.BinderAlertManager = claimAlertManager

                'no binder items
                'load accum tables
                detailAccumulatorsDT = claimBinder.BinderAccumulatorManager.GetAccumulatorEntryValues(True)
                accumulatorsDT = claimBinder.GetAccumulatorSummary

            End If

        Catch ex As Exception

            Throw
        Finally

            If sharedInterfacesMessage IsNot Nothing Then sharedInterfacesMessage.Invoke("")

            ProceduresQ = Nothing
            ClaimBinderItem = Nothing
            ProcedureCollection = Nothing
            Procedure = Nothing
            Procedures = Nothing

        End Try
    End Sub

    Public Shared Sub LoadDetailLineAlerts(ByRef claimDS As DataSet, ByRef claimAlertManager As AlertManagerCollection)

        Dim MeddtlDV As DataView
        Dim DetailDV As DataView
        Dim MeddtlCurrentRowsDT As DataTable

        Dim ChrgDT As DataTable
        Dim CoPay25DV As DataView
        Dim CoPay30DV As DataView
        Dim AlertDV As DataView

        Dim DTLDV As DataView
        Dim DiagDV As DataView


        Try

            If claimDS.Tables("MEDDTL").Rows.Count < 1 Then
                claimAlertManager.AddAlertRow(New Object() {"No Detail Lines", 0, "Detail", 20})
            End If

            ''TOT_PAID_AMT > 0 and with Line(s) identified as PAY, that a Payee has been assigned
            If Not IsDBNull(claimDS.Tables("MEDHDR").Rows(0)("TOT_PAID_AMT")) AndAlso CDec(claimDS.Tables("MEDHDR").Rows(0)("TOT_PAID_AMT")) > 0 AndAlso IsDBNull(claimDS.Tables("MEDHDR").Rows(0)("PAYEE")) Then
                MeddtlDV = New DataView(MeddtlCurrentRowsDT, "STATUS = 'PAY'", "LINE_NBR", DataViewRowState.CurrentRows)
                If MeddtlDV.Count > 0 Then
                    claimAlertManager.AddAlertRow(New Object() {"Invalid Payee", 0, "Detail", 20})
                End If
            End If

            If _PTSTExclusionProcedures Is Nothing Then _PTSTExclusionProcedures = CMSDALCommon.ExpandProcedureValues(ConfigurationManager.AppSettings("PTSTExclusionProcedures"))
            If _ChemoExclusionProcedures Is Nothing Then _ChemoExclusionProcedures = CMSDALFDBMD.RetrieveChemoProcedureValues
            If _PWOExclusionProcedures Is Nothing Then _PWOExclusionProcedures = CMSDALFDBMD.RetrievePWOExclusionProcedureValues

            MeddtlCurrentRowsDT = claimDS.Tables("MEDDTL").Clone

            MeddtlCurrentRowsDT.BeginLoadData()
            For Each DRV As DataRowView In claimDS.Tables("MEDDTL").DefaultView
                MeddtlCurrentRowsDT.ImportRow(DRV.Row)
            Next
            MeddtlCurrentRowsDT.EndLoadData()

            'ProcCode
            MeddtlDV = New DataView(MeddtlCurrentRowsDT, "STATUS <> 'MERGED' AND PROC_CODE_DESC = '***INVALID PROCEDURE CODE***'", "PROC_CODE_DESC", DataViewRowState.CurrentRows)
            If MeddtlDV.Count > 0 Then
                For Cnt As Integer = 0 To MeddtlDV.Count - 1
                    claimAlertManager.DeleteAlertRowsLikeMessageAndLine("determine how to process line", CInt(MeddtlDV(Cnt)("LINE_NBR")))
                    claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Invalid Procedure Code", MeddtlDV(Cnt)("LINE_NBR").ToString, "Detail", 30, Nothing})
                Next
            End If

            'PT/ST/Accident
            MeddtlDV = New DataView(MeddtlCurrentRowsDT, "STATUS <> 'MERGED'", "LINE_NBR", DataViewRowState.CurrentRows)
            For Each DRV As DataRowView In MeddtlDV
                If Array.IndexOf(_PTSTExclusionProcedures, DRV("PROC_CODE")) >= 0 Then

                    claimAlertManager.AddAlertRow(New Object() {"Line " & DRV("LINE_NBR").ToString & ": " & "Possible PT/ST", DRV("LINE_NBR"), "Detail", 20})
                End If
                If Array.IndexOf(_ChemoExclusionProcedures, DRV("PROC_CODE")) >= 0 AndAlso IsRetiree(claimDS, CDate(DRV("OCC_FROM_DATE"))) Then

                    claimAlertManager.AddAlertRow(New Object() {"Line " & DRV("LINE_NBR").ToString & ": " & "Possible Chemo", DRV("LINE_NBR"), "Detail", 20})
                End If
                'If AccidentDiagnosisPresent(claimDS, CInt(meddtlDataRowView("LINE_NBR"))) Then

                '    ClaimAlertManager.AddAlertRow(New Object() {"Line " & meddtlDataRowView("LINE_NBR").ToString & ": " & "Possible Accident", meddtlDataRowView("LINE_NBR"), "Detail", 20})
                'End If
            Next

            'PlaceOfService
            MeddtlDV = New DataView(MeddtlCurrentRowsDT, "STATUS <> 'MERGED' AND PLACE_OF_SERV_DESC = '***INVALID PLACE OF SERVICE***'", "PLACE_OF_SERV_DESC", DataViewRowState.CurrentRows)
            If MeddtlDV.Count > 0 Then
                For Cnt As Integer = 0 To MeddtlDV.Count - 1
                    claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Invalid Place of Service", MeddtlDV(Cnt)("LINE_NBR").ToString, "Detail", 30})
                Next
            End If

            'Case Mgmt
            MeddtlDV = New DataView(MeddtlCurrentRowsDT, "STATUS <> 'MERGED' AND PLACE_OF_SERV = '12'", "PLACE_OF_SERV", DataViewRowState.CurrentRows)
            If MeddtlDV.Count > 0 Then
                For Cnt As Integer = 0 To MeddtlDV.Count - 1
                    claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Possible Case Management", MeddtlDV(Cnt)("LINE_NBR").ToString, "Detail", _CaseManagement})
                Next
            End If

            'Pay Billed
            MeddtlDV = New DataView(MeddtlCurrentRowsDT, "STATUS <> 'MERGED' AND NON_PAR_SW = 1 AND (PLACE_OF_SERV = '23' OR PLACE_OF_SERV = '41' OR PLACE_OF_SERV = '42')", "PLACE_OF_SERV", DataViewRowState.CurrentRows)
            If MeddtlDV.Count > 0 Then
                For Cnt As Integer = 0 To MeddtlDV.Count - 1
                    claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Pay as Billed", MeddtlDV(Cnt)("LINE_NBR").ToString, "Detail", 20})
                Next
            End If

            'Rendering Provider Review
            MeddtlDV = New DataView(MeddtlCurrentRowsDT, "STATUS <> 'MERGED' AND NON_PAR_SW = 1 AND PLACE_OF_SERV = '21'", "PLACE_OF_SERV", DataViewRowState.CurrentRows)
            If MeddtlDV.Count > 0 Then
                For Cnt As Integer = 0 To MeddtlDV.Count - 1
                    claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Rendering Facility Review required", MeddtlDV(Cnt)("LINE_NBR").ToString, "Detail", 20})
                Next
            End If

            'BillType
            MeddtlDV = New DataView(MeddtlCurrentRowsDT, "STATUS <> 'MERGED' AND BILL_TYPE_DESC = '***INVALID BILL TYPE***'", "BILL_TYPE_DESC", DataViewRowState.CurrentRows)
            If MeddtlDV.Count > 0 Then
                For Each DRV As DataRowView In MeddtlDV
                    ValidateBillType(DRV, CStr(DRV("BILL_TYPE")), UFCWGeneral.IsNullDateHandler(DRV("OCC_FROM_DATE")), claimAlertManager)
                Next
            End If

            'Detail Lines with a suspect unit amount (most likely missing decimal)
            If claimDS.Tables("CLAIM_MASTER").Rows(0)("EDI_SOURCE").ToString.Trim.Length = 0 Then
                MeddtlDV = New DataView(MeddtlCurrentRowsDT, "STATUS <> 'MERGED' AND DAYS_UNITS > 9 ", "DAYS_UNITS", DataViewRowState.CurrentRows)
                If MeddtlDV.Count > 0 Then
                    For Cnt As Integer = 0 To MeddtlDV.Count - 1
                        claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Check Units against Image ", MeddtlDV(Cnt)("LINE_NBR").ToString, "Detail", 15})
                    Next
                End If
            End If

            'Modifiers
            MeddtlDV = New DataView(claimDS.Tables("MEDMOD"), "FULL_DESC = '***INVALID MODIFIER***'", "FULL_DESC", DataViewRowState.CurrentRows)
            If MeddtlDV.Count > 0 Then
                For Cnt As Integer = 0 To MeddtlDV.Count - 1
                    DetailDV = New DataView(MeddtlCurrentRowsDT, "STATUS <> 'MERGED' AND LINE_NBR = " & MeddtlDV(Cnt)("LINE_NBR").ToString, "LINE_NBR", DataViewRowState.CurrentRows)

                    If DetailDV.Count > 0 Then
                        claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Invalid Modifier", MeddtlDV(Cnt)("LINE_NBR").ToString, "Detail", 30})
                    End If

                Next
            End If

            'Missing Diagnosis
            MeddtlDV = New DataView(MeddtlCurrentRowsDT, "STATUS <> 'MERGED' AND STATUS <> 'DENY' AND DIAGNOSIS = '*** MISSING DIAGNOSIS ***'", "", DataViewRowState.CurrentRows)
            If MeddtlDV.Count > 0 Then
                For Cnt As Integer = 0 To MeddtlDV.Count - 1
                    claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Invalid Diagnosis (Missing)", MeddtlDV(Cnt)("LINE_NBR"), "Detail", 20})
                Next
            End If

            'Diagnosis
            MeddtlDV = New DataView(claimDS.Tables("MEDDIAG"), "SHORT_DESC = '***INVALID DIAGNOSIS***'", "SHORT_DESC", DataViewRowState.CurrentRows)
            If MeddtlDV.Count > 0 Then
                For Cnt As Integer = 0 To MeddtlDV.Count - 1
                    DetailDV = New DataView(MeddtlCurrentRowsDT, "STATUS <> 'MERGED' AND LINE_NBR = " & MeddtlDV(Cnt)("LINE_NBR").ToString, "LINE_NBR", DataViewRowState.CurrentRows)

                    If DetailDV.Count > 0 Then
                        claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Invalid Diagnosis", MeddtlDV(Cnt)("LINE_NBR").ToString, "Detail", 30})
                    End If

                Next
            End If

            ChrgDT = claimDS.Tables("MEDDIAG").GetChanges(DataRowState.Deleted)
            'If claimDS.Tables("MEDDIAG").Rows.Count = 0 OrElse (ChrgDT IsNot Nothing AndAlso claimDS.Tables("MEDDIAG").Rows.Count = ChrgDT.Rows.Count) Then
            '    For Cnt As Integer = 0 To MeddtlCurrentRowsDT.Rows.Count - 1
            '        claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlCurrentRowsDT.Rows(Cnt)("LINE_NBR").ToString & ": Invalid Changed Diagnosis", MeddtlCurrentRowsDT.Rows(Cnt)("LINE_NBR").ToString, "Detail", 30})
            '    Next
            'Else
            MeddtlDV = New DataView(MeddtlCurrentRowsDT, "STATUS <> 'MERGED'", "LINE_NBR", DataViewRowState.CurrentRows)
            DiagDV = New DataView(claimDS.Tables("MEDDIAG"), "", "LINE_NBR", DataViewRowState.CurrentRows)

            For Cnt As Integer = 0 To MeddtlDV.Count - 1
                DiagDV.RowFilter = "LINE_NBR = " & MeddtlDV(Cnt)("LINE_NBR").ToString

                If DiagDV.Count = 0 Then
                    claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Invalid Diagnosis (Missing)", MeddtlDV(Cnt)("LINE_NBR").ToString, "Detail", 30})
                End If
            Next

            'Pricing Errors
            MeddtlDV = New DataView(MeddtlCurrentRowsDT, "STATUS <> 'MERGED' AND PRICING_ERROR_DESC IS NOT NULL ", "LINE_NBR, PRICING_ERROR_DESC, PRICING_SD_ERROR_DESC", DataViewRowState.CurrentRows)
            If MeddtlDV.Count > 0 Then
                For Cnt As Integer = 0 To MeddtlDV.Count - 1
                    If Not IsDBNull(MeddtlDV(Cnt)("PRICING_ERROR_DISP_SW")) AndAlso CBool(MeddtlDV(Cnt)("PRICING_ERROR_DISP_SW")) AndAlso CInt(MeddtlDV(Cnt)("PRICING_ERROR_SEVERITY")) > 0 Then
                        claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Error BC(" & MeddtlDV(Cnt)("PRICING_ERROR").ToString & ") - " & MeddtlDV(Cnt)("PRICING_ERROR_DESC").ToString & " (Pricing)", MeddtlDV(Cnt)("LINE_NBR"), "Detail", MeddtlDV(Cnt)("PRICING_ERROR_SEVERITY")})
                    End If

                    If IsDBNull(MeddtlDV(Cnt)("PRICING_ERROR_MRU_SW")) = False AndAlso CBool(MeddtlDV(Cnt)("PRICING_ERROR_MRU_SW")) Then
                        claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Error BC(" & MeddtlDV(Cnt)("PRICING_ERROR").ToString & ") - " & "Possible Send To MRU (Pricing)", MeddtlDV(Cnt)("LINE_NBR"), "Detail", CInt(MeddtlDV(Cnt)("PRICING_ERROR_SEVERITY")) + 25})
                    End If
                Next
            End If

            'Pricing Suspend/Deny Errors
            MeddtlDV = New DataView(MeddtlCurrentRowsDT, "STATUS <> 'MERGED' AND PRICING_SD_ERROR_DESC IS NOT NULL", "LINE_NBR, PRICING_SD_ERROR_DESC", DataViewRowState.CurrentRows)
            If MeddtlDV.Count > 0 Then
                For Cnt As Integer = 0 To MeddtlDV.Count - 1
                    If Not IsDBNull(MeddtlDV(Cnt)("PRICING_SD_ERROR_DISP_SW")) AndAlso CBool(MeddtlDV(Cnt)("PRICING_SD_ERROR_DISP_SW")) Then
                        Select Case MeddtlDV(Cnt)("PRICING_SD_ERROR_DESC").ToString
                            Case "Procedure not on fee Schedule"
                                claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": SD Error BC(" & MeddtlDV(Cnt)("PRICING_SD_ERROR").ToString & ") - " & MeddtlDV(Cnt)("PRICING_SD_ERROR_DESC").ToString & " (Pricing)", MeddtlDV(Cnt)("LINE_NBR"), "Detail", If(MeddtlDV(Cnt)("PROC_CODE").ToString.EndsWith("99"), CInt(MeddtlDV(Cnt)("PRICING_SD_ERROR_SEVERITY")) + 25, MeddtlDV(Cnt)("PRICING_SD_ERROR_SEVERITY"))})
                            Case Else
                                claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": SD Error BC(" & MeddtlDV(Cnt)("PRICING_SD_ERROR").ToString & ") - " & MeddtlDV(Cnt)("PRICING_SD_ERROR_DESC").ToString & " (Pricing)", MeddtlDV(Cnt)("LINE_NBR"), "Detail", MeddtlDV(Cnt)("PRICING_SD_ERROR_SEVERITY")})
                        End Select

                    End If

                    If IsDBNull(MeddtlDV(Cnt)("PRICING_SD_ERROR_MRU_SW")) = False AndAlso CBool(MeddtlDV(Cnt)("PRICING_SD_ERROR_MRU_SW")) Then
                        claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": SD Error BC(" & MeddtlDV(Cnt)("PRICING_SD_ERROR").ToString & ") - " & "Possible Send To MRU (Pricing)", MeddtlDV(Cnt)("LINE_NBR"), "Detail", CInt(MeddtlDV(Cnt)("PRICING_SD_ERROR_SEVERITY")) + 25})
                    End If

                Next
            End If

            'Pricing Reasons
            MeddtlDV = New DataView(MeddtlCurrentRowsDT, "STATUS <> 'MERGED' AND PRICING_REASON_DESC IS NOT NULL", "LINE_NBR, PRICING_REASON", DataViewRowState.CurrentRows)
            If MeddtlDV.Count > 0 Then
                For Cnt As Integer = 0 To MeddtlDV.Count - 1

                    'Note the specific alerts are due to N/O/P matching for a specific reason code
                    If Not IsDBNull(MeddtlDV(Cnt)("PNO_PRIC_RSN_DISP_SW")) AndAlso CBool(MeddtlDV(Cnt)("PNO_PRIC_RSN_DISP_SW")) Then
                        claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Pricing Reason (" & MeddtlDV(Cnt)("PRICING_REASON").ToString & ") - " & MeddtlDV(Cnt)("PRICING_REASON_DESC").ToString & " (Pricing)", MeddtlDV(Cnt)("LINE_NBR"), "Detail", MeddtlDV(Cnt)("PRICING_REASON_SEVERITY")})
                    ElseIf Not IsDBNull(MeddtlDV(Cnt)("PRICING_REASON_DISP_SW")) AndAlso CBool(MeddtlDV(Cnt)("PRICING_REASON_DISP_SW")) Then
                        claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Pricing Reason (" & MeddtlDV(Cnt)("PRICING_REASON").ToString & ") - " & MeddtlDV(Cnt)("PRICING_REASON_DESC").ToString & " (Pricing)", MeddtlDV(Cnt)("LINE_NBR"), "Detail", MeddtlDV(Cnt)("PRICING_REASON_SEVERITY")})
                    End If

                    If IsDBNull(MeddtlDV(Cnt)("PNO_PRIC_RSN_MRU_SW")) = False AndAlso CBool(MeddtlDV(Cnt)("PNO_PRIC_RSN_MRU_SW")) Then
                        claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Pricing Reason (" & MeddtlDV(Cnt)("PRICING_REASON").ToString & ") - " & "Possible Send To MRU (Pricing)", MeddtlDV(Cnt)("LINE_NBR"), "Detail", CInt(MeddtlDV(Cnt)("PRICING_REASON_SEVERITY")) + 25})
                    ElseIf IsDBNull(MeddtlDV(Cnt)("PRICING_REASON_MRU_SW")) = False AndAlso CBool(MeddtlDV(Cnt)("PRICING_REASON_MRU_SW")) Then
                        claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Pricing Reason (" & MeddtlDV(Cnt)("PRICING_REASON").ToString & ") - " & "Possible Send To MRU (Pricing)", MeddtlDV(Cnt)("LINE_NBR"), "Detail", CInt(MeddtlDV(Cnt)("PRICING_REASON_SEVERITY")) + 25})
                    End If

                    If Not IsDBNull(MeddtlDV(Cnt)("PNO_PRIC_RSN_DISP_SW_0_PAY")) AndAlso CBool(MeddtlDV(Cnt)("PNO_PRIC_RSN_DISP_SW_0_PAY")) AndAlso Not IsDBNull(MeddtlDV(Cnt)("PNO_PRIC_RSN_UFCW_CD_0_PAY")) AndAlso MeddtlDV(Cnt)("PNO_PRIC_RSN_UFCW_CD_0_PAY").ToString = If(IsJAA(claimDS.Tables("MEDHDR").Rows(0)), "FFW", "PWO") AndAlso Not IsDBNull(MeddtlDV(Cnt)("ORIG_PRICED_AMT")) AndAlso CDec(MeddtlDV(Cnt)("ORIG_PRICED_AMT")) = 0D Then
                        claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Provider Write Off (BC)(" & MeddtlDV(Cnt)("PRICING_REASON").ToString & ") - " & MeddtlDV(Cnt)("PRICING_REASON_DESC").ToString & " (Pricing)", MeddtlDV(Cnt)("LINE_NBR"), "Detail", CInt(MeddtlDV(Cnt)("PRICING_REASON_SEVERITY")) + If(PWOExclusionProcedurePresent(MeddtlDV(Cnt)), 25, 0)})
                    ElseIf Not IsDBNull(MeddtlDV(Cnt)("PRICING_REASON_DISP_SW_0_PAY")) AndAlso CBool(MeddtlDV(Cnt)("PRICING_REASON_DISP_SW_0_PAY")) AndAlso Not IsDBNull(MeddtlDV(Cnt)("UFCW_REASON_CODE_WHEN_ZERO_PAY")) AndAlso MeddtlDV(Cnt)("UFCW_REASON_CODE_WHEN_ZERO_PAY").ToString = If(IsJAA(claimDS.Tables("MEDHDR").Rows(0)), "FFW", "PWO") AndAlso Not IsDBNull(MeddtlDV(Cnt)("ORIG_PRICED_AMT")) AndAlso CDec(MeddtlDV(Cnt)("ORIG_PRICED_AMT")) = 0D Then
                        claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Provider Write Off (BC)(" & MeddtlDV(Cnt)("PRICING_REASON").ToString & ") - " & MeddtlDV(Cnt)("PRICING_REASON_DESC").ToString & " (Pricing)", MeddtlDV(Cnt)("LINE_NBR"), "Detail", CInt(MeddtlDV(Cnt)("PRICING_REASON_SEVERITY")) + If(PWOExclusionProcedurePresent(MeddtlDV(Cnt)), 25, 0)})
                    End If

                    If IsDBNull(MeddtlDV(Cnt)("PNO_PRIC_RSN_MRU_SW_0_PAY")) = False AndAlso CBool(MeddtlDV(Cnt)("PNO_PRIC_RSN_MRU_SW_0_PAY")) Then
                        claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Pricing Reason (" & MeddtlDV(Cnt)("PRICING_REASON").ToString & ") - " & "Possible Send To MRU (Pricing)", MeddtlDV(Cnt)("LINE_NBR"), "Detail", CInt(MeddtlDV(Cnt)("PRICING_REASON_SEVERITY")) + 25})
                    ElseIf IsDBNull(MeddtlDV(Cnt)("PRICING_REASON_MRU_SW_0_PAY")) = False AndAlso CBool(MeddtlDV(Cnt)("PRICING_REASON_MRU_SW_0_PAY")) Then
                        claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Pricing Reason (" & MeddtlDV(Cnt)("PRICING_REASON").ToString & ") - " & "Possible Send To MRU (Pricing)", MeddtlDV(Cnt)("LINE_NBR"), "Detail", CInt(MeddtlDV(Cnt)("PRICING_REASON_SEVERITY")) + 25})
                    End If

                Next
            End If

            'Detail Lines Received by the fund a year later
            If Not IsDBNull(claimDS.Tables("CLAIM_MASTER").Rows(0)("REC_DATE")) Then
                MeddtlDV = New DataView(MeddtlCurrentRowsDT, "STATUS <> 'MERGED' AND OCC_FROM_DATE < '" & DateAdd(DateInterval.Year, -1, CDate(claimDS.Tables("CLAIM_MASTER").Rows(0)("REC_DATE"))) & "'", "OCC_FROM_DATE", DataViewRowState.CurrentRows)
                If MeddtlDV.Count > 0 Then
                    For Cnt As Integer = 0 To MeddtlDV.Count - 1
                        claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Was Received 1 Year After DOS", MeddtlDV(Cnt)("LINE_NBR").ToString, "Detail", 20})
                    Next
                End If
            End If

            'Detail Lines missing OCC_FROM_DATE
            MeddtlDV = New DataView(MeddtlCurrentRowsDT, "STATUS <> 'MERGED' AND ISNULL(OCC_FROM_DATE,'1-1-1600') =  '1-1-1600'", "OCC_FROM_DATE", DataViewRowState.CurrentRows)
            If MeddtlDV.Count > 0 Then
                For Cnt As Integer = 0 To MeddtlDV.Count - 1
                    claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Missing From Date", MeddtlDV(Cnt)("LINE_NBR").ToString, "Detail", 20})
                Next
            End If

            ''Cannot complete with negative paid amount.
            MeddtlDV = New DataView(MeddtlCurrentRowsDT, "STATUS = 'PAY' AND ISNULL(PAID_AMT, 0) < 0", "LINE_NBR", DataViewRowState.CurrentRows)
            If MeddtlDV.Count > 0 Then
                For Cnt As Integer = 0 To MeddtlDV.Count - 1
                    claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Paid should not be Negative", MeddtlDV(Cnt)("LINE_NBR"), "Detail", 20})
                Next
            End If

            '' For DENY status Paid Amount should be Zero
            MeddtlDV = New DataView(MeddtlCurrentRowsDT, "STATUS = 'DENY' AND ISNULL(PAID_AMT, 0) > 0", "LINE_NBR", DataViewRowState.CurrentRows)
            If MeddtlDV.Count > 0 Then

                For Cnt As Integer = 0 To MeddtlDV.Count - 1
                    claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": PAY AMT Should be 0 for Status DENY", MeddtlDV(Cnt)("LINE_NBR"), "Detail", 20})
                Next
            End If

            'Paid > Priced or Charged
            MeddtlDV = New DataView(MeddtlCurrentRowsDT, "STATUS <> 'MERGED' AND PAID_AMT > PRICED_AMT", "PAID_AMT, PRICED_AMT", DataViewRowState.CurrentRows)
            If MeddtlDV.Count > 0 Then
                For Cnt As Integer = 0 To MeddtlDV.Count - 1
                    claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Paid Is More Than Priced", MeddtlDV(Cnt)("LINE_NBR").ToString, "Detail", 20})
                Next
            End If

            'Paid = 0 without a reason code
            MeddtlDV = New DataView(MeddtlCurrentRowsDT, "STATUS <> 'MERGED' AND ISNULL(PAID_AMT, 0) = 0 AND REASON_SW = 0", "PAID_AMT, REASON_SW", DataViewRowState.CurrentRows)
            If MeddtlDV.Count > 0 Then
                For Cnt As Integer = 0 To MeddtlDV.Count - 1
                    claimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Paid Is 0 and a Reason is Required", MeddtlDV(Cnt)("LINE_NBR").ToString, "Detail", If(IsDBNull(MeddtlDV(Cnt)("MED_PLAN")) OrElse IsPlanPPO(CStr(MeddtlDV(Cnt)("MED_PLAN"))), 15, 20)})
                Next
            End If

            'Paid < Allowed without a reason code
            MeddtlDV = New DataView(MeddtlCurrentRowsDT, "STATUS <> 'MERGED' AND (ISNULL(PAID_AMT, 0) < ISNULL(ALLOWED_AMT, 0) AND ISNULL(ALLOWED_AMT, 0) > 0)", "", DataViewRowState.CurrentRows)
            If MeddtlDV.Count > 0 Then
                For Cnt As Integer = 0 To MeddtlDV.Count - 1
                    AlertDV = New DataView(claimAlertManager.AlertManagerDataTable, "LineNumber = " & MeddtlDV(Cnt)("LINE_NBR").ToString & " And Category = 'Detail' And Message = 'Line " & MeddtlDV(Cnt)("LINE_NBR").ToString & ": Paid Is 0 and a Reason is Required'", "LineNumber, Category, Message", DataViewRowState.CurrentRows)
                    CoPay25DV = New DataView(claimAlertManager.AlertManagerDataTable, "LineNumber = " & MeddtlDV(Cnt)("LINE_NBR").ToString & " And Category = 'ReasonCode' And Message like '%$25%'", "LineNumber, Category, Message", DataViewRowState.CurrentRows)
                    CoPay30DV = New DataView(claimAlertManager.AlertManagerDataTable, "LineNumber = " & MeddtlDV(Cnt)("LINE_NBR").ToString & " And Category = 'ReasonCode' And Message like '%$30%'", "LineNumber, Category, Message", DataViewRowState.CurrentRows)

                    If (CoPay25DV.Count > 0 And (Not IsDBNull(MeddtlDV(Cnt)("PAID_AMT")) And Not IsDBNull(MeddtlDV(Cnt)("ALLOWED_AMT")))) AndAlso (CDec(MeddtlDV(Cnt)("ALLOWED_AMT").ToString) - 25) = CDec(MeddtlDV(Cnt)("PAID_AMT").ToString) Then
                    ElseIf (CoPay30DV.Count > 0 And (Not IsDBNull(MeddtlDV(Cnt)("PAID_AMT")) And Not IsDBNull(MeddtlDV(Cnt)("ALLOWED_AMT")))) AndAlso (CDec(MeddtlDV(Cnt)("ALLOWED_AMT").ToString) - 30) = CDec(MeddtlDV(Cnt)("PAID_AMT").ToString) Then

                    ElseIf AlertDV.Count = 0 Then
                        'for now only ppo claims and ineligibile will be autoadjudicated when this alert is raised (CoPay issues arise for HMO)
                        'ClaimAlertManager.AddAlertRow(New Object() {"Line " & meddtlDV(cnt)("LINE_NBR").ToString & ": Paid Is < Allowed and a Reason is Required", meddtlDV(cnt)("LINE_NBR"), "Detail", If(IsDBNull(meddtlDV(cnt)("MED_PLAN")) OrElse IsPlanPPO(CStr(meddtlDV(cnt)("MED_PLAN"))), 15, 20)})
                    End If
                Next
            End If

        Catch ex As Exception

            Throw
        Finally
            If MeddtlDV IsNot Nothing Then MeddtlDV.Dispose()
            MeddtlDV = Nothing

            If DetailDV IsNot Nothing Then DetailDV.Dispose()
            DetailDV = Nothing

            If DTLDV IsNot Nothing Then DTLDV.Dispose()
            DTLDV = Nothing

            If DiagDV IsNot Nothing Then DiagDV.Dispose()
            DiagDV = Nothing

            If AlertDV IsNot Nothing Then AlertDV.Dispose()
            AlertDV = Nothing

            If CoPay25DV IsNot Nothing Then CoPay25DV.Dispose()
            CoPay25DV = Nothing

            If CoPay30DV IsNot Nothing Then CoPay30DV.Dispose()
            CoPay30DV = Nothing

        End Try
    End Sub

    Public Shared Sub LoadDiagnosisAlerts(ByRef claimDS As DataSet, ByRef claimAlertManager As AlertManagerCollection, ByVal plansIncludingPreventativeRules As String())
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Load Alerts For invalid Data in the Header or Pricing Errors
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	9/15/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim ClaimMasterDR As DataRow

        Dim MeddiagDV As DataView

        Dim MeddtlDV As DataView
        Dim MeddtlCurrentRowsDT As DataTable

        Dim AlertExpr As String
        Dim AlertDRArray() As DataRow

        Dim Diagnoses() As String = {"", "", "", ""}
        Dim IdenticalDiagnosis As Boolean = True

        Try

            MeddtlCurrentRowsDT = claimDS.Tables("MEDDTL").Clone

            MeddtlCurrentRowsDT.BeginLoadData()
            For Each DRV As DataRowView In claimDS.Tables("MEDDTL").DefaultView
                MeddtlCurrentRowsDT.ImportRow(DRV.Row)
            Next
            MeddtlCurrentRowsDT.EndLoadData()

            ClaimMasterDR = claimDS.Tables("CLAIM_MASTER").Rows(0)

            MeddtlDV = New DataView(MeddtlCurrentRowsDT, "STATUS <> 'MERGED'", "", DataViewRowState.CurrentRows)

            For Each DRV As DataRowView In MeddtlDV

                If DRV("DIAGNOSES").ToString.Trim.Length = 0 Then
                ElseIf Diagnoses.SequenceEqual(New String() {"", "", "", ""}) Then
                    Diagnoses = Replace(DRV("DIAGNOSES").ToString, " ", "").ToString.Split(New Char() {CChar(",")})
                ElseIf Diagnoses.SequenceEqual(Replace(DRV("DIAGNOSES").ToString, " ", "").ToString.Split(New Char() {CChar(",")})) Then
                Else
                    IdenticalDiagnosis = False
                End If

                If Array.IndexOf(plansIncludingPreventativeRules, DRV("MED_PLAN")) >= 0 Then

                    MeddiagDV = New DataView(claimDS.Tables("MEDDIAG"), "LINE_NBR = " & DRV("LINE_NBR").ToString, "", DataViewRowState.CurrentRows)

                    For Each MedDiagDRV As DataRowView In MeddiagDV
                        If CBool(MedDiagDRV("PREVENTATIVE_USE_SW")) Then
                            AlertExpr = "LineNumber = " & MedDiagDRV("LINE_NBR").ToString & " AND MESSAGE LIKE '%processed as Preventative%'"
                            AlertDRArray = claimAlertManager.AlertManagerDataTable.Select(AlertExpr)

                            If AlertDRArray.Length < 1 Then claimAlertManager.AddAlertRow(New Object() {"Line " & MedDiagDRV("LINE_NBR").ToString & ": " & "Possible Preventative Diagnosis(" & MedDiagDRV("DIAGNOSIS").ToString & ") Priority(" & MedDiagDRV("PRIORITY").ToString & ")", MedDiagDRV("LINE_NBR").ToString, "Detail", 15})
                        End If
                    Next
                End If
            Next

            If MeddtlCurrentRowsDT.Rows.Count > 1 AndAlso ClaimMasterDR("DOCID").ToString.Trim.Length > 0 AndAlso Not ClaimMasterDR("MAXID").ToString.ToUpper.StartsWith("E") AndAlso Not ClaimMasterDR("MAXID").ToString.ToUpper.StartsWith("U") AndAlso Not ClaimMasterDR("MAXID").ToString.ToUpper.StartsWith("H") AndAlso Diagnoses.Length > 1 AndAlso IdenticalDiagnosis Then
                claimAlertManager.AddAlertRow(New Object() {"Possible miss-coded Diagnosis Pointers. Confirm associated Diagnosis values against image.", 0, "Header", 30})
            End If

        Catch ex As Exception

            Throw
        Finally
            MeddtlDV = Nothing
            MeddiagDV = Nothing

        End Try
    End Sub

    Public Shared Sub LoadHeaderAlerts(ByRef claimDS As DataSet, ByRef claimAlertManager As AlertManagerCollection)

        Dim MedHdrDR As DataRow

        Try

            If claimDS.Tables("MEDHDR").Rows.Count > 0 Then MedHdrDR = claimDS.Tables("MEDHDR").Rows(0)

            If MedHdrDR IsNot Nothing Then

                If MedHdrDR("FAMILY_ID").ToString.Trim.Length = 0 OrElse CInt(MedHdrDR("FAMILY_ID").ToString) = -1 Then
                    'add an alert
                    claimAlertManager.AddAlertRow(New Object() {"Family ID is invalid ", 0, "Header", 20})
                End If

                If MedHdrDR("PART_SSN").ToString.Trim.Length = 0 Then
                    'add an alert
                    claimAlertManager.AddAlertRow(New Object() {"Participant SSN is invalid ", 0, "Header", 20})
                End If

                If MedHdrDR("RELATION_ID").ToString.Trim.Length = 0 OrElse CInt(MedHdrDR("RELATION_ID").ToString) = -1 Then
                    'add an alert
                    claimAlertManager.AddAlertRow(New Object() {"RELATION ID is invalid ", 0, "Header", 20})
                End If

                If MedHdrDR("PAT_SSN").ToString.Trim.Length = 0 Then
                    'add an alert
                    claimAlertManager.AddAlertRow(New Object() {"Patient SSN is invalid ", 0, "Header", 20})
                End If

                'valid COB
                If IsDBNull(MedHdrDR("COB")) OrElse MedHdrDR("COB").ToString.Trim.Length = 0 Then
                    claimAlertManager.AddAlertRow(New Object() {"COB is invalid ", 0, "Header", 20})
                End If

                'COB indicates insurance overlap
                If Not IsDBNull(MedHdrDR("COB")) AndAlso CInt(MedHdrDR("COB").ToString) > 0 Then
                    claimAlertManager.AddAlertRow(New Object() {"COB indicates possible other insurance ", 0, "Header", 20})
                End If

                'COB indicates insurance overlap
                If Not IsDBNull(MedHdrDR("OTH_INS_SW")) AndAlso CBool(MedHdrDR("OTH_INS_SW")) Then
                    claimAlertManager.AddAlertRow(New Object() {"Other Insurance Switch is active ", 0, "Header", _OtherInsurance})
                End If

                'Valid PPO
                If (IsDBNull(MedHdrDR("PPO")) OrElse MedHdrDR("PPO").ToString.Trim.Length = 0) AndAlso Not (Not IsDBNull(MedHdrDR("PRICED_BY")) AndAlso MedHdrDR("PRICED_BY").ToString.Trim = "N/A") Then
                    claimAlertManager.AddAlertRow(New Object() {"PPO is invalid ", 0, "Header", 20})
                End If

                'valid Payee
                If IsDBNull(MedHdrDR("PAYEE")) OrElse MedHdrDR("PAYEE").ToString.Trim.Length = 0 Then
                    claimAlertManager.AddAlertRow(New Object() {"PAYEE is invalid ", 0, "Header", 20})
                End If

                'validate charge amounts for all lines with the exception of lines added by BC
                If IsDBNull(MedHdrDR("TOT_CHRG_AMT")) OrElse IsDBNull(claimDS.Tables("MEDDTL").Compute("Sum(ORIG_CHRG_AMT)", "PRICING_ERROR IS NULL or PRICING_ERROR Not Like 'M%'")) OrElse CDec(MedHdrDR("TOT_CHRG_AMT").ToString) <> CDec(claimDS.Tables("MEDDTL").Compute("Sum(ORIG_CHRG_AMT)", "PRICING_ERROR IS NULL or PRICING_ERROR Not Like 'M%'").ToString) Then
                    claimAlertManager.AddAlertRow(New Object() {"Charge Amount does not match line items ", 0, "Header", 20})
                End If

                If (MedHdrDR("PPO").ToString.Trim = "BC" OrElse MedHdrDR("PPO").ToString.Trim = "BCFFE") AndAlso (CBool(MedHdrDR("NON_PAR_SW").ToString) OrElse CBool(MedHdrDR("OUT_OF_AREA_SW").ToString)) Then
                    claimAlertManager.AddAlertRow(New Object() {"PPO is invalid for Non Par item ", 0, "Header", 20})
                End If

                If MedHdrDR("PPO").ToString.Trim = "BCJAA" AndAlso CBool(MedHdrDR("OUT_OF_AREA_SW")) AndAlso Not IsDBNull(MedHdrDR("OCC_FROM_DATE")) AndAlso CDate(MedHdrDR("OCC_FROM_DATE")) <= New Date(2012, 12, 31) Then
                    claimAlertManager.AddAlertRow(New Object() {"PPO is invalid for OOS JAA Claim Prior to 1/1/2013 ", 0, "Header", 20})
                End If

                If MedHdrDR("RENDERING_NPI").ToString.Trim.Length > 0 AndAlso MedHdrDR("RENDERING_NPI").ToString.Trim.Length < 10 Then
                    'add an alert
                    claimAlertManager.AddAlertRow(New Object() {"RENDERING_NPI is invalid ", 0, "Header", 20})
                End If

                If MedHdrDR("PROV_ZIP").ToString.Trim.Length > 0 AndAlso MedHdrDR("PROV_ZIP").ToString.Trim.Length < 5 Then
                    'add an alert
                    claimAlertManager.AddAlertRow(New Object() {"PROV_ZIP is invalid ", 0, "Header", 20})
                End If

                'Pricing Alerts
                If Not IsDBNull(MedHdrDR("PRICING_ERROR_DESC")) AndAlso IsDBNull(MedHdrDR("PRICING_ERROR_DISP_SW")) = False AndAlso CBool(MedHdrDR("PRICING_ERROR_DISP_SW")) Then
                    Dim Severity As Integer = CInt(MedHdrDR("PRICING_ERROR_SEVERITY"))
                    claimAlertManager.AddAlertRow(New Object() {MedHdrDR("PRICING_ERROR_DESC").ToString & " (Pricing)", 0, "Header", Severity})
                End If

                If Not IsDBNull(MedHdrDR("PRICING_ERROR_MRU_SW")) AndAlso CBool(MedHdrDR("PRICING_ERROR_MRU_SW")) Then
                    claimAlertManager.AddAlertRow(New Object() {"Possible Send To MRU (Pricing)", 0, "Header", 20})
                End If

                If IsDBNull(MedHdrDR("MRU_DECISION_CD")) AndAlso MedHdrDR("MRU_DECISION_CD").ToString.Trim.Length > 0 Then
                    Dim BCSuspendDenyCodeRow As DataRow = CMSDALFDBMD.RetrieveBCSuspendDenyCodeByCode(MedHdrDR("MRU_DECISION_CD").ToString)

                    claimAlertManager.AddAlertRow(New Object() {"Anthem Reported MRU Code (" & MedHdrDR("MRU_DECISION_CD").ToString & If(BCSuspendDenyCodeRow Is Nothing, ")", ") - " & BCSuspendDenyCodeRow("DESCRIPTION").ToString), 0, "Header", 20})
                End If

                'Paying Member
                If Not IsDBNull(MedHdrDR("PAYEE")) AndAlso MedHdrDR("PAYEE").ToString = "3" Then

                    claimAlertManager.AddAlertRow(New Object() {"Paying Member", 0, "Header", 20})
                End If

                'missing dob
                If IsDBNull(MedHdrDR("PAT_DOB")) Then
                    claimAlertManager.AddAlertRow(New Object() {"Incomplete Member Info, reselect Patient", 0, "Header", 30})
                End If

                If Not IsDBNull(MedHdrDR("INCIDENT_DATE")) Then
                    claimAlertManager.AddAlertRow(New Object() {"Incident Date Reported", 0, "Header", _OtherInsurance})
                End If

            End If
        Catch ex As Exception

            Throw
        End Try
    End Sub

    Public Shared Sub LoadPatientAlerts(ByRef claimDS As DataSet, ByRef claimAlertManager As AlertManagerCollection)

        Dim PartDR As DataRow
        Dim PatDR As DataRow

        Try

            If claimDS.Tables("PARTICIPANT").Rows.Count > 0 Then PartDR = claimDS.Tables("PARTICIPANT").Rows(0)

            If PartDR Is Nothing Then
                claimAlertManager.AddAlertRow(New Object() {"Invalid Participant", 0, "Header", 30})
            End If

            If claimDS.Tables("PATIENT").Rows.Count > 0 Then PatDR = claimDS.Tables("PATIENT").Rows(0)

            If PatDR Is Nothing OrElse Not IsDate(claimDS.Tables("MEDHDR").Rows(0)("PAT_DOB")) Then
                claimAlertManager.AddAlertRow(New Object() {"Invalid Patient", 0, "Header", 30})
            End If

            If PatDR IsNot Nothing AndAlso PatDR("BIRTH_DATE").ToString <> claimDS.Tables("MEDHDR").Rows(0)("PAT_DOB").ToString Then
                claimAlertManager.AddAlertRow(New Object() {"Patient DOB Mismatch", 0, "Header", 30})
            End If

        Catch ex As Exception

            Throw
        End Try
    End Sub

    Public Shared Sub LoadReasonsAlerts(ByRef claimDS As DataSet, ByRef claimAlertManager As AlertManagerCollection)

        Dim DV As DataView

        Try

            'Load Reason Alerts
            DV = New DataView(claimDS.Tables("REASON"), "PRINT_SW = 0", "PRINT_SW", DataViewRowState.CurrentRows)
            If DV.Count > 0 Then
                For Cnt As Integer = 0 To DV.Count - 1
                    claimAlertManager.AddAlertRow(New Object() {"Line " & DV(Cnt)("LINE_NBR").ToString & ": " & DV(Cnt)("DESCRIPTION").ToString, DV(Cnt)("LINE_NBR").ToString, "Reasons", 10})
                Next
            End If

        Catch ex As Exception

            Throw
        Finally
            DV = Nothing
        End Try
    End Sub
    Public Shared Function PWOExclusionProcedurePresent(ByVal meddtlCurrentDRV As DataRowView) As Boolean

        Try

            Return _PWOExclusionProcedures.Contains(meddtlCurrentDRV("PROC_CODE").ToString.Trim)

        Catch ex As Exception

            Throw
        End Try

    End Function

    Public Shared Function PWOExclusionProcedurePresent(ByVal meddtlCurrentDR As DataRow) As Boolean

        Try

            Return _PWOExclusionProcedures.Contains(meddtlCurrentDR("PROC_CODE").ToString.Trim)

        Catch ex As Exception

            Throw
        End Try

    End Function

    Private Shared Function AccidentDiagnosisPresent(ByRef claimDS As DataSet, ByVal lineNumber As Short) As Boolean
        Dim DV As DataView
        Try
            DV = New DataView(claimDS.Tables("MEDDIAG"), "LINE_NBR = " & lineNumber.ToString, "", DataViewRowState.CurrentRows)

            Return DV.Cast(Of DataRowView)().Any(Function(DRV) CBool(DRV("ACCIDENT_RELATED_SW")))

        Catch ex As Exception

            Throw
        Finally
            If DV IsNot Nothing Then DV.Dispose()
            DV = Nothing
        End Try

    End Function

    Private Shared Sub AddReason(ByRef claimDS As DataSet, ByRef meddtlDR As DataRow, meddtlCurrentRowsDT As DataTable, factToApply As Fact, ByRef claimAlertManager As AlertManagerCollection)

        Dim AsOfDate As Date?
        Dim DateOfServiceDV As DataView
        Dim ReasonValueDR As DataRow

        Try
            meddtlDR("REASON_SW") = 1

            Dim DR As DataRow = claimDS.Tables("REASON").NewRow

            If Not IsDBNull(meddtlDR("OCC_FROM_DATE")) Then
                AsOfDate = UFCWGeneral.IsNullDateHandler(meddtlDR("OCC_FROM_DATE"))
            Else

                DateOfServiceDV = New DataView(meddtlCurrentRowsDT, "OCC_FROM_DATE = MIN(OCC_FROM_DATE)", "OCC_FROM_DATE", DataViewRowState.CurrentRows)

                If DateOfServiceDV.Count > 0 AndAlso Not IsDBNull(DateOfServiceDV(0)("OCC_FROM_DATE")) Then
                    AsOfDate = UFCWGeneral.IsNullDateHandler(DateOfServiceDV(0)("OCC_FROM_DATE"))
                ElseIf Not IsDBNull(claimDS.Tables("CLAIM_MASTER").Rows(0)("DATE_OF_SERVICE")) Then
                    AsOfDate = UFCWGeneral.IsNullDateHandler(claimDS.Tables("CLAIM_MASTER").Rows(0)("DATE_OF_SERVICE"))
                End If
            End If

            ReasonValueDR = CMSDALFDBMD.RetrieveReasonValuesInformation(If(IsJAA(claimDS.Tables("MEDHDR").Rows(0)), "FFW", "PWO"), AsOfDate)

            DR("CLAIM_ID") = claimDS.Tables("CLAIM_MASTER").Rows(0)("CLAIM_ID")
            DR("LINE_NBR") = factToApply.LineNumber
            DR("REASON") = If(IsJAA(claimDS.Tables("MEDHDR").Rows(0)), "FFW", "PWO")
            DR("DESCRIPTION") = ReasonValueDR("DESCRIPTION")
            DR("PRINT_SW") = ReasonValueDR("PRINT_SW")
            DR("APPLY_STATUS") = ReasonValueDR("APPLY_STATUS")
            DR("PRIORITY") = 0

            claimDS.Tables("REASON").Rows.Add(DR)

            If Not CBool(ReasonValueDR("PRINT_SW")) Then
                claimAlertManager.AddAlertRow(New Object() {"Line " & DR("LINE_NBR").ToString & ": " & DR("DESCRIPTION").ToString, DR("LINE_NBR").ToString, "Reasons", 10})
            End If

            claimAlertManager.DeleteAlertRowsByMessageAndLine("'Line " & factToApply.LineNumber & ": Paid Is 0 and a Reason is Required'", factToApply.LineNumber)

        Catch ex As Exception
            Throw

        End Try
    End Sub

    Private Shared Function BinderPayAmount(ByVal binder As Binder, ByVal docClass As String, ByVal lineNumber As Short) As Decimal?

        Dim MaxPayment As Decimal?
        Dim Processor As IProcessor

        Try

            Processor = ProcessorFactory.CreateProcessor(docClass)
            Processor.Process(binder)

            Dim QueryFactInLineNumberOrder = From Facts In binder.Facts.OfType(Of Fact)() Where Facts.LineNumber = lineNumber

            For Each Fact In QueryFactInLineNumberOrder.AsEnumerable

                If MaxPayment Is Nothing OrElse Fact.PaymentAmount > MaxPayment Then
                    MaxPayment = Fact.PaymentAmount
                End If

            Next 'f

            Return MaxPayment

        Catch ex As Exception

            Throw
        Finally
            Processor = Nothing
        End Try

    End Function

    Private Shared Sub DetermineElectronicAlert(ByRef claimDS As DataSet, ByRef claimAlertManager As AlertManagerCollection)

        Dim ClaimMasterDR As DataRow

        Try

            ClaimMasterDR = claimDS.Tables("CLAIM_MASTER").Rows(0)

            If IsDBNull(ClaimMasterDR("MAXID")) = False Then
                Select Case True
                    Case ClaimMasterDR("MAXID").ToString.ToUpper.StartsWith("E")
                        claimAlertManager.AddAlertRow(New Object() {"Received as Electronic Submission via File Import", 0, "Header", 15})
                    Case ClaimMasterDR("MAXID").ToString.ToUpper.StartsWith("U"), ClaimMasterDR("MAXID").ToString.ToUpper.StartsWith("H")
                        Select Case ClaimMasterDR("EDI_SOURCE").ToString
                            Case "PSBCCA", "PRBCCA", "PHBCCA"
                                Select Case ClaimMasterDR("REFERENCE_ID").ToString.Substring(7, 2)
                                    Case "12", "13", "16", "24", "25", "36", "37", "39", "41", "42", "43", "44", "45", "46", "51", "56", "57", "67", "69", "73", "74", "75", "80", "89", "95", "97", "98"
                                        claimAlertManager.AddAlertRow(New Object() {If(ClaimMasterDR("REFERENCE_ID").ToString.EndsWith("84"), "Adjustment - ", "") & "Received as " & If(ClaimMasterDR("EDI_SOURCE").ToString = "PSBCCA", "FFE", "JAA") & " Electronic Submission" & If(claimDS.Tables("MEDHDR").Rows(0)("PRICED_BY").ToString.Contains(" (S)"), " (Summary Priced) ", "") & ", Original Paper Document via Anthem - DCN " & ClaimMasterDR("REFERENCE_ID").ToString, 0, "Header", 15})
                                    Case "LB" To "LZ"
                                        claimAlertManager.AddAlertRow(New Object() {If(ClaimMasterDR("REFERENCE_ID").ToString.EndsWith("84"), "Adjustment - ", "") & "Received as " & If(ClaimMasterDR("EDI_SOURCE").ToString = "PSBCCA", "FFE", "JAA") & " Electronic Submission" & If(claimDS.Tables("MEDHDR").Rows(0)("PRICED_BY").ToString.Contains(" (S)"), " (Summary Priced) ", "") & ", Original Paper Document via Anthem - DCN " & ClaimMasterDR("REFERENCE_ID").ToString, 0, "Header", 15})
                                    Case "MA" To "ZZ", "M0" To "M9", "47", "48", "49", "87"
                                        claimAlertManager.AddAlertRow(New Object() {If(ClaimMasterDR("REFERENCE_ID").ToString.EndsWith("84"), "Adjustment - ", "") & "Received as " & If(ClaimMasterDR("EDI_SOURCE").ToString = "PSBCCA", "FFE", "JAA") & " Electronic Submission" & If(claimDS.Tables("MEDHDR").Rows(0)("PRICED_BY").ToString.Contains(" (S)"), " (Summary Priced) ", "") & ", BlueCard via Anthem - DCN " & ClaimMasterDR("REFERENCE_ID").ToString, 0, "Header", 15})
                                    Case Else
                                        claimAlertManager.AddAlertRow(New Object() {If(ClaimMasterDR("REFERENCE_ID").ToString.EndsWith("84"), "Adjustment - ", "") & "Received as " & If(ClaimMasterDR("EDI_SOURCE").ToString = "PSBCCA", "FFE", "JAA") & " Electronic Submission" & If(claimDS.Tables("MEDHDR").Rows(0)("PRICED_BY").ToString.Contains(" (S)"), " (Summary Priced) ", "") & "", 0, "Header", 15})
                                End Select

                            Case Else
                                claimAlertManager.AddAlertRow(New Object() {"Received as Electronic Submission" & If(claimDS.Tables("MEDHDR").Rows(0)("PRICED_BY").ToString.Contains(" (S)"), " (Summary Priced) ", "") & " from " & ClaimMasterDR("EDI_SOURCE").ToString, 0, "Header", 15})
                        End Select
                End Select
            End If
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Shared Sub DisperseSummaryPricing(ByVal claimBinder As IBinder, ByVal claimAccumulatorManager As MemberAccumulatorManager, ByVal ruleSetType As Integer?, ByVal origTotalPricedAmt As Decimal, ByVal meddtlNonMergedRowsDT As DataTable, ByRef claimDS As DataSet)

        Dim TotalPermittedChrgAmt As Decimal = 0
        Dim TotalDeniedChrgAmt As Decimal = 0

        Dim LineChrgAmt As Decimal
        Dim LinePricedAmt As Decimal

        Dim TotalPricedAmt As Decimal = 0

        Dim RoundedAmt As Decimal = 0

        Dim ClaimBinderItem As BinderItem

        Try

            'check that accumulators have been validated since system conversion
            If CInt(claimAccumulatorManager.GetOriginalLifetimeValue(CInt(AccumulatorController.GetAccumulatorID("FIXAC")))) <= 0 Then Return

            Dim QueryMEDDTLBinder =
                From MEDDTL In meddtlNonMergedRowsDT.AsEnumerable
                Where MEDDTL.RowState <> DataRowState.Deleted
                Order By MEDDTL.Field(Of Short)("LINE_NBR")
                Select MEDDTL

            For Each DR As DataRow In QueryMEDDTLBinder.AsEnumerable
                ClaimBinderItem = claimBinder.GetBinderItem(CShort(DR("LINE_NBR")))

                If ClaimBinderItem IsNot Nothing Then
                    If Not ClaimBinderItem.Procedure.IsDenyRule(ruleSetType) AndAlso Not DR("STATUS").ToString = "DENY" Then
                        If Not IsDBNull(DR("CHRG_AMT")) Then
                            TotalPermittedChrgAmt += CDec(DR("CHRG_AMT"))
                        End If
                    Else
                        If Not IsDBNull(DR("CHRG_AMT")) Then
                            TotalDeniedChrgAmt += CDec(DR("CHRG_AMT"))
                        End If
                    End If
                ElseIf DR("STATUS").ToString = "DENY" Then
                    If Not IsDBNull(DR("CHRG_AMT")) Then
                        TotalDeniedChrgAmt += CDec(DR("CHRG_AMT"))
                    End If
                End If

            Next

            If TotalPermittedChrgAmt = 0 Then Exit Try

            Dim QueryMEDDTL =
                From MEDDTL In claimDS.Tables("MEDDTL").AsEnumerable
                Where MEDDTL.RowState <> DataRowState.Deleted _
                AndAlso MEDDTL.Field(Of String)("STATUS") <> "MERGE"
                Order By MEDDTL.Field(Of Short)("LINE_NBR")
                Select MEDDTL

            'Apply Amounts, Note: Deny lines are assumed to be priced at zero by Anthem
            For Each DR As DataRow In QueryMEDDTL
                ClaimBinderItem = claimBinder.GetBinderItem(CShort(DR("LINE_NBR")))

                If ClaimBinderItem IsNot Nothing Then
                    If Not ClaimBinderItem.Procedure.IsDenyRule(ruleSetType) AndAlso Not DR("STATUS").ToString = "DENY" Then

                        If Not IsDBNull(DR("CHRG_AMT")) Then
                            LineChrgAmt = CDec(DR("CHRG_AMT"))
                        Else
                            LineChrgAmt = 0
                        End If

                        If LineChrgAmt <> 0 Then
                            LinePricedAmt = ((LineChrgAmt / TotalPermittedChrgAmt) * origTotalPricedAmt) + RoundedAmt
                        Else
                            LinePricedAmt = 0
                        End If

                        RoundedAmt = CDec(CDbl(Format(LinePricedAmt, "0.0000"))) - LinePricedAmt

                        TotalPricedAmt += CDec(Format(LinePricedAmt, "0.00"))

                        If Not IsDBNull(DR("PRICED_AMT")) Then
                            If LinePricedAmt <> CDec(DR("PRICED_AMT")) Then
                                DR("PRICED_AMT") = Format(LinePricedAmt, "0.00")
                            End If
                        Else
                            DR("PRICED_AMT") = LinePricedAmt
                        End If

                        If Not IsDBNull(DR("ALLOWED_AMT")) Then
                            If CDec(DR("ALLOWED_AMT")) > CDec(DR("PRICED_AMT")) Then
                                DR("ALLOWED_AMT") = DR("PRICED_AMT")
                            End If
                        Else
                            If Not DR("ALLOWED_AMT").Equals(DR("PRICED_AMT")) Then DR("ALLOWED_AMT") = DR("PRICED_AMT")
                        End If

                        'If IsDBNull(DRV("ORIG_PRICED_AMT")) Then
                        '    DRV("ORIG_PRICED_AMT") = Format(LinePricedAmt, "0.00")
                        'End If

                    Else 'deny rule

                        DR("PRICED_AMT") = 0D

                        If Not IsDBNull(DR("ALLOWED_AMT")) Then
                            If CDec(DR("ALLOWED_AMT")) > CDec(DR("PRICED_AMT")) Then
                                DR("ALLOWED_AMT") = DR("PRICED_AMT")
                            End If
                        Else
                            DR("ALLOWED_AMT") = DR("PRICED_AMT")
                        End If

                        'If IsDBNull(DRV("ORIG_PRICED_AMT")) Then
                        '    DRV("ORIG_PRICED_AMT") = Format(0, "0.00")
                        'End If

                    End If

                ElseIf DR("STATUS").ToString = "DENY" Then

                    DR("PRICED_AMT") = 0D

                    If Not IsDBNull(DR("ALLOWED_AMT")) Then
                        If CDec(DR("ALLOWED_AMT")) > CDec(DR("PRICED_AMT")) Then
                            DR("ALLOWED_AMT") = DR("PRICED_AMT")
                        End If
                    Else
                        DR("ALLOWED_AMT") = DR("PRICED_AMT")
                    End If

                End If

            Next

            If TotalPricedAmt <> origTotalPricedAmt Then
                RoundedAmt = origTotalPricedAmt - TotalPricedAmt

                Dim QueryMEDDTLDesc =
                    From MEDDTL In claimDS.Tables("MEDDTL").AsEnumerable
                    Where MEDDTL.RowState <> DataRowState.Deleted _
                    AndAlso MEDDTL.Field(Of String)("STATUS") <> "MERGE"
                    Order By MEDDTL.Field(Of Short)("LINE_NBR") Descending
                    Select MEDDTL

                For Each DR As DataRow In QueryMEDDTLDesc

                    ClaimBinderItem = claimBinder.GetBinderItem(CShort(DR("LINE_NBR")))

                    If ClaimBinderItem IsNot Nothing Then
                        If Not ClaimBinderItem.Procedure.IsDenyRule(ruleSetType) AndAlso Not DR("STATUS").ToString = "DENY" Then

                            LinePricedAmt = CDec(DR("PRICED_AMT"))

                            If LinePricedAmt <> 0 Then
                                LinePricedAmt += RoundedAmt

                                If Not IsDBNull(DR("PRICED_AMT")) Then
                                    If LinePricedAmt <> CDec(DR("PRICED_AMT")) Then
                                        DR("PRICED_AMT") = LinePricedAmt
                                    End If
                                Else
                                    DR("PRICED_AMT") = LinePricedAmt
                                End If

                                If Not IsDBNull(DR("ALLOWED_AMT")) Then
                                    If CDec(DR("ALLOWED_AMT")) > CDec(DR("PRICED_AMT")) Then
                                        DR("ALLOWED_AMT") = DR("PRICED_AMT")
                                    End If
                                Else
                                    DR("ALLOWED_AMT") = DR("PRICED_AMT")
                                End If

                                Exit For
                            End If
                        End If
                    End If

                Next

            End If

        Catch ex As Exception
            Throw
        Finally
            claimDS.Tables("MEDDTL").DefaultView.RowFilter = ""
            claimDS.Tables("MEDDTL").DefaultView.Sort = ""
        End Try

    End Sub

    Private Shared Function GetBestPaySchedule(ByVal claimBinder As Binder, ByVal claimBinderItem As BinderItem, ByVal procedures As System.Collections.Generic.Queue(Of ProcedureActive)) As ProcedureActive
        'BinderItem parameter is item in progress and has not yet been added to the Binder

        Dim MaxPayment As Decimal?
        Dim PayAmount As Decimal?
        Dim ClaimBinderClone As Binder
        Dim ClaimBinderItemClone As BinderItem
        Dim BestProcedure As ProcedureActive
        Dim BeginTime As Date = UFCWGeneral.NowDate

        Try
            ClaimBinderClone = claimBinder.ShallowCopy

            ClaimBinderClone.Facts = claimBinder.Facts?.DeepCopy

            For Each Procedure As ProcedureActive In procedures

                ClaimBinderClone.BinderAccumulatorManager = claimBinder.BinderAccumulatorManager.DeepCopy
                ClaimBinderClone.BinderAlertManager = New AlertManagerCollection

                ClaimBinderClone.BinderItems = New Hashtable
                ClaimBinderItemClone = claimBinderItem.DeepCopy
                ClaimBinderItemClone.Procedure = DirectCast(Procedure.DeepCopy, ProcedureActive)
                ClaimBinderClone.AddBinderItem(ClaimBinderItemClone)

                PayAmount = BinderPayAmount(ClaimBinderClone, ClaimBinderClone.DocumentClass, ClaimBinderItemClone.LineNumber)

                If PayAmount IsNot Nothing AndAlso (MaxPayment Is Nothing OrElse PayAmount > MaxPayment) Then
                    MaxPayment = CDec(PayAmount)
                    BestProcedure = Procedure
                End If

            Next

            Return BestProcedure

        Catch ex As Exception

            Throw
        Finally


            ClaimBinderItemClone = Nothing
            BestProcedure = Nothing
        End Try

    End Function

    Private Shared Function IsAccident(ByVal medhdrDR As DataRow) As Boolean
        Try
            If Not IsDBNull(medhdrDR("INCIDENT_DATE")) Then
                If Not IsDBNull(medhdrDR("WORKERS_COMP_SW")) AndAlso (CBool(medhdrDR("WORKERS_COMP_SW")) OrElse CBool(medhdrDR("WORKERS_COMP_SW"))) Then
                    Return True
                ElseIf Not IsDBNull(medhdrDR("AUTO_ACCIDENT_SW")) AndAlso (CBool(medhdrDR("AUTO_ACCIDENT_SW")) OrElse CBool(medhdrDR("AUTO_ACCIDENT_SW"))) Then
                    Return True
                ElseIf Not IsDBNull(medhdrDR("OTH_ACCIDENT_SW")) AndAlso (CBool(medhdrDR("OTH_ACCIDENT_SW")) OrElse CBool(medhdrDR("OTH_ACCIDENT_SW"))) Then
                    Return True
                End If
            End If

            Return False

        Catch ex As Exception

            Throw
        End Try

    End Function

    Private Shared Function IsJAA(medhdrDR As DataRow) As Boolean
        Try
            If medhdrDR IsNot Nothing Then

                If Not IsDBNull(medhdrDR("PRICED_BY")) Then
                    Select Case medhdrDR("PRICED_BY").ToString.ToUpper
                        Case "BLUE CROSS JAA", "BLUE CROSS JAA (S)"
                            Return True
                    End Select
                End If

            End If

            Return False

        Catch ex As Exception

            Throw
        End Try
    End Function

    Private Shared Function IsPlanPPO(ByVal medPlan As String) As Boolean

        Dim PlansDV As DataView

        Try

            PlansDV = New DataView(CMSDALFDBMD.RetrievePlans, "PLAN_TYPE = '" & medPlan & "' And PPO = 1", "", DataViewRowState.CurrentRows)
            If PlansDV.Count > 0 Then
                Return True
            End If

            Return False

        Catch ex As Exception
            Throw
        Finally
            If PlansDV IsNot Nothing Then PlansDV.Dispose()
            PlansDV = Nothing
        End Try

    End Function

    Private Shared Function IsRetiree(ByRef claimDS As DataSet, dateOfService As Date) As Boolean

        Dim DV As DataView

        Try

            DV = New DataView(claimDS.Tables("ELIGIBILITY"), "ELIG_PERIOD = '" & Format(dateOfService, "MM-01-yyyy") & "'", "ELIG_PERIOD", DataViewRowState.CurrentRows)

            If DV.Count > 0 AndAlso DV(0)("STATUS").ToString.ToUpper = "RETIREE" Then Return True

            Return False

        Catch ex As Exception

            Throw

        Finally
            If DV IsNot Nothing Then DV.Dispose()
            DV = Nothing
        End Try

    End Function

    Private Shared Function ManageProcedures(ByRef claimAlertManager As AlertManagerCollection, ByVal meddtlDR As DataRow, ByVal binderItemCount As Integer?, ByVal percentMatch As Integer, ByRef procedure As ProcedureActive, ByRef procedures As Hashtable, ByRef proceduresQ As Generic.Queue(Of ProcedureActive), ByRef procedureCollection As Procedures, ByRef claimBinder As MedicalBinder, ByRef claimBinderItem As MedicalBinderItem, ByRef claimDS As DataSet, meddtlNonMergedRowsDT As DataTable) As Integer?

        Dim ProceduresEnum As IDictionaryEnumerator

        Dim PlaceOfServ As String = ""
        Dim Prov As String = ""
        Dim BillType As String = ""
        Dim Modifier As String = ""
        Dim Gender As String = ""
        Dim DateOfService As Date?
        Dim DateOfBirth As Date?

        Dim Diagnoses() As String = {"", "", "", ""}

        Dim ProcedureHash As Hashtable
        Dim IgnorePriced As Boolean = False

        Try

            'test for any null plan
            Dim QueryNoPlan =
                From NoPlan In meddtlNonMergedRowsDT.AsEnumerable
                Where NoPlan.Field(Of String)("MED_PLAN") Is Nothing
                Select NoPlan

            If CStr(claimDS.Tables("CLAIM_MASTER").Rows(0)("DOC_TYPE")).ToUpper.Contains("HOSPITAL") AndAlso Not QueryNoPlan.Any Then
                ProcedureHash = PlanController.GetClaimProcedures(meddtlNonMergedRowsDT, CStr(claimDS.Tables("CLAIM_MASTER").Rows(0)("DOC_TYPE")).ToUpper, claimDS.Tables("MEDHDR"))
            End If

            If claimDS.Tables("MEDHDR").Rows(0)("PAT_SEX").ToString.Trim.Length > 0 Then
                Gender = claimDS.Tables("MEDHDR").Rows(0)("PAT_SEX").ToString
            Else
                Gender = ""
            End If

            If claimDS.Tables("MEDHDR").Rows(0)("PAT_DOB").ToString.Trim.Length > 0 AndAlso IsDate(claimDS.Tables("MEDHDR").Rows(0)("PAT_DOB")) Then
                DateOfBirth = CDate(claimDS.Tables("MEDHDR").Rows(0)("PAT_DOB"))
            Else
                DateOfBirth = Nothing
            End If

            If Not IsDBNull(meddtlDR("OCC_FROM_DATE")) Then
                DateOfService = Date.Parse(CStr(meddtlDR("OCC_FROM_DATE")))
            Else
                If Not IsDBNull(claimDS.Tables("CLAIM_MASTER").Rows(0)("DATE_OF_SERVICE")) Then
                    DateOfService = CType(If(IsDBNull(claimDS.Tables("CLAIM_MASTER").Rows(0)("DATE_OF_SERVICE")), Nothing, claimDS.Tables("CLAIM_MASTER").Rows(0)("DATE_OF_SERVICE")), Date?)
                Else
                    'What should be put if no DateOfService
                    Exit Try
                End If
            End If

            If Not IsDBNull(meddtlDR("PLACE_OF_SERV")) Then
                PlaceOfServ = CStr(meddtlDR("PLACE_OF_SERV"))
            Else
                PlaceOfServ = ""
            End If

            If CStr(claimDS.Tables("CLAIM_MASTER").Rows(0)("DOC_TYPE")).ToUpper.Contains("MENTAL HEALTH") Then
                If IsDBNull(claimDS.Tables("MEDHDR").Rows(0)("ADMITTANCE")) = False AndAlso CStr(claimDS.Tables("MEDHDR").Rows(0)("ADMITTANCE")).ToUpper = "I" Then
                    'inpatient
                    PlaceOfServ = "51"
                Else
                    'outpatient
                    PlaceOfServ = "99"
                End If
            End If

            If CBool(claimDS.Tables("MEDHDR").Rows(0)("OUT_OF_AREA_SW")) Then
                Prov = "O"
            ElseIf CBool(claimDS.Tables("MEDHDR").Rows(0)("NON_PAR_SW")) Then
                Prov = "N"
            Else
                Prov = "P"
            End If

            If IsDBNull(meddtlDR("DIAGNOSES")) = False Then
                Diagnoses = Replace(meddtlDR("DIAGNOSES").ToString, " ", "").ToString.Split(New Char() {CChar(",")})
            End If

            If IsDBNull(meddtlDR("BILL_TYPE")) = False Then
                BillType = CStr(meddtlDR("BILL_TYPE"))
            Else
                BillType = ""
            End If

            If IsDBNull(meddtlDR("MODIFIER")) = False Then
                Modifier = CStr(meddtlDR("MODIFIER"))
            Else
                Modifier = ""
            End If

            procedure = Nothing
            procedures = New Hashtable
            proceduresQ = New System.Collections.Generic.Queue(Of ProcedureActive)

            If claimDS.Tables("CLAIM_MASTER").Rows(0)("DOC_TYPE").ToString.ToUpper.Contains("HOSPITAL") AndAlso ProcedureHash IsNot Nothing Then
                procedure = CType(ProcedureHash(CDate(DateOfService).ToShortDateString), ProcedureActive)
            End If

            If procedure Is Nothing Then

                If procedureCollection IsNot Nothing Then procedureCollection.Clear()
                procedureCollection = Nothing

                If PlanController.IsValidProcedureCode(CStr(meddtlDR("PROC_CODE")), CDate(DateOfService)) Then 'get rules that use procedure code
                    procedureCollection = PlanController.GetActiveProceduresByProcedureCode(CStr(meddtlDR("MED_PLAN")), CStr(meddtlDR("PROC_CODE")).ToUpper, CDate(DateOfService), True)
                End If

                If procedureCollection Is Nothing OrElse procedureCollection.Count = 0 Then 'Procedure Family Not in System Error
                    If claimAlertManager IsNot Nothing Then claimAlertManager.AddAlertRow(New Object() {"Line " & meddtlDR("LINE_NBR").ToString & ": Procedure Code not found, or not valid for Plan or DOS", meddtlDR("LINE_NBR").ToString, "Binder", 30})
                    procedureCollection = PlanController.GetActiveProceduresByProcedureCode(CStr(meddtlDR("MED_PLAN")), PlanController.WildCardProcedure, CDate(DateOfService), True)
                End If

                If procedureCollection.Count > 0 Then
                    For I As Integer = 0 To Diagnoses.Length - 1 'collect the rules for each diagnosis
                        procedure = CType(procedureCollection.GetBestMatch(procedureCollection(0).ProcedureCode, PlaceOfServ, Prov, Diagnoses(I), Diagnoses, BillType, Modifier, Gender, CDate(DateOfBirth), CDate(DateOfService), _RuleSetType, percentMatch), ProcedureActive)
                        If procedure IsNot Nothing AndAlso Not procedures.ContainsKey(procedure.ToString) Then
                            procedures.Add(procedure.ToString, procedure)
                            proceduresQ.Enqueue(procedure)
                        End If
                    Next
                End If

                If procedures.Count = 0 Then
                    'Procedure/Rule Not in System Error
                    If claimAlertManager IsNot Nothing Then claimAlertManager.AddAlertRow(New Object() {"Line " & meddtlDR("LINE_NBR").ToString & ": Can't determine how to process line", meddtlDR("LINE_NBR").ToString, "Binder", 30})

                    Return Nothing

                End If
            Else
                procedures.Add(procedure.ToString, procedure)
                proceduresQ.Enqueue(procedure)
            End If

            CType(claimBinder, IBinder).DateOfClaim = CDate(DateOfService)

            claimBinderItem = CType(claimBinder.NewBinderItem(), MedicalBinderItem)
            claimBinderItem.LineNumber = CShort(meddtlDR("LINE_NBR"))

            claimBinderItem.DateOfService = CDate(DateOfService)
            claimBinderItem.DateOfBirth = CDate(DateOfBirth)
            claimBinderItem.Gender = Gender
            claimBinderItem.PrimaryDiagnosis = Diagnoses(0)

            If Not IsDBNull(meddtlDR("DAYS_UNITS")) Then
                claimBinderItem.UnitAmount = CDec(meddtlDR("DAYS_UNITS"))
            Else
                claimBinderItem.UnitAmount = 1D
            End If

            If Not IsDBNull(claimDS.Tables("MEDHDR").Rows(0)("INCIDENT_DATE")) Then
                claimBinderItem.IncidentDate = Date.Parse(CStr(claimDS.Tables("MEDHDR").Rows(0)("INCIDENT_DATE")))
            End If

            If CBool(claimDS.Tables("MEDHDR").Rows(0)("CHIRO_SW")) AndAlso CBool(claimDS.Tables("MEDHDR").Rows(0)("NON_PAR_SW")) Then
                IgnorePriced = True
            End If

            If Not IsDBNull(meddtlDR("CHRG_AMT")) Then
                claimBinderItem.ValuedAmount = CDec(meddtlDR("CHRG_AMT"))
            End If

            If Not IgnorePriced Then
                If Not IsDBNull(meddtlDR("PRICED_AMT")) Then
                    If Not IsDBNull(meddtlDR("CHRG_AMT")) Then
                        If CDec(meddtlDR("CHRG_AMT")) > CDec(meddtlDR("PRICED_AMT")) Then
                            claimBinderItem.ValuedAmount = CDec(meddtlDR("PRICED_AMT"))
                        End If
                    Else
                        claimBinderItem.ValuedAmount = CDec(meddtlDR("PRICED_AMT"))
                    End If
                End If
            End If

            If procedures.Count > 1 Then

                claimBinderItem.Procedure = GetBestPaySchedule(claimBinder, claimBinderItem, proceduresQ)

            Else
                ProceduresEnum = procedures.GetEnumerator()
                ProceduresEnum.MoveNext()
                claimBinderItem.Procedure = CType(ProceduresEnum.Value, ProcedureActive)
            End If

            claimBinderItem.RuleSetNameUsed = PlanController.GetRuleSetNameByProcedureID(claimBinderItem.Procedure.ProcedureID) 'expensive ?

            claimBinder.AddBinderItem(claimBinderItem)
            binderItemCount += 1

            Return binderItemCount

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Function

    Private Shared Sub ProcessBinder(ByRef claimBinder As MedicalBinder, ByRef highestAccumulatorEntryIdForFamily As Integer, ByRef claimAccumulatorManager As MemberAccumulatorManager, ByRef claimDS As DataSet, ByVal meddtlCurrentRowsDT As DataTable, ByVal docClass As String, ByVal procedure As ProcedureActive, ByRef detailAccumulatorsDT As DataTable, ByRef accumulatorsDT As DataTable, ByRef claimAlertManager As AlertManagerCollection, ByRef selectAccidentUI As SelectAccidentUIDelegate)

        Dim AccumIDForAccident As Integer?
        Dim AccumNameForAccident As String = ""
        Dim ClaimIDForAccident As Integer?
        Dim DateForAccident As Date?

        Dim ClaimBinderItem As MedicalBinderItem
        Dim Processor As IProcessor

        Dim HasValidAccident As Boolean

        Try

            claimAlertManager.DeleteAlertRowsByCategory("Reprice")

            highestAccumulatorEntryIdForFamily = claimAccumulatorManager.GetHighestEntryIdForFamily

            'This codepath is not currently valid for AA as it should have resulted in an exclusion Alert during the Validate process.
            If IsAccident(claimDS.Tables("MEDHDR").Rows(0)) Then

                For Each DR As DataRow In meddtlCurrentRowsDT.Rows
                    If Not CDate(DR("OCC_FROM_DATE")) < CDate(claimDS.Tables("MEDHDR").Rows(0)("INCIDENT_DATE")) Then
                        If CDate(DR("OCC_FROM_DATE")).AddDays(-90) <= CDate(claimDS.Tables("MEDHDR").Rows(0)("INCIDENT_DATE")) Then
                            HasValidAccident = True
                            Exit For
                        End If
                    End If
                Next

                If HasValidAccident Then

                    If claimBinder.HasAccidentRule Then ' check if selected rule has an accident component
                        If selectAccidentUI IsNot Nothing Then
                            selectAccidentUI(claimAccumulatorManager, claimDS, meddtlCurrentRowsDT, AccumIDForAccident, AccumNameForAccident, ClaimIDForAccident, DateForAccident)
                        End If
                    End If

                End If

                If AccumIDForAccident IsNot Nothing Then
                    claimBinder.ReplaceAccidentAccumulator(AccumNameForAccident)
                    claimBinder.OriginalClaimIDForAccident = CInt(If(ClaimIDForAccident Is Nothing, 0, ClaimIDForAccident))
                Else
                    claimBinder.RemoveAccidentAccumulators()
                End If

            End If

            Processor = ProcessorFactory.CreateProcessor(docClass)
            Processor.Process(claimBinder)

            detailAccumulatorsDT = claimBinder.BinderAccumulatorManager.GetAccumulatorEntryValues(True)
            accumulatorsDT = claimBinder.GetAccumulatorSummary

            '#If TRACE Then
            '            If CInt(_TraceParallel.Level) > 0 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : Facts Count : " & claimBinder.Facts.Count.ToString & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
            '#End If

            Dim QueryFactInLineNumberOrder = From Facts In claimBinder.Facts.OfType(Of Fact)() Order By Facts.LineNumber

            For Each FactToApply As Fact In QueryFactInLineNumberOrder.AsEnumerable
                Dim LineNbr As Short = FactToApply.LineNumber

                Dim QueryMEDDTL =
                    From MEDDTL In claimDS.Tables("MEDDTL").AsEnumerable
                    Where MEDDTL.RowState <> DataRowState.Deleted _
                    AndAlso MEDDTL.Field(Of Short)("LINE_NBR") = LineNbr
                    Select MEDDTL

                Dim MeddtlDR As DataRow = QueryMEDDTL.FirstOrDefault

                'this needs to be utimately resolved as a possible threading issue 
                If MeddtlDR Is Nothing Then
                    claimAlertManager.AddAlertRow(New Object() {"Line " & FactToApply.LineNumber & ": Meddtl row was not available, Line skipped", FactToApply.LineNumber, "Binder", 30})
                    Continue For
                End If

                ClaimBinderItem = CType(claimBinder.GetBinderItem(FactToApply.LineNumber), MedicalBinderItem)

                If CBool(CType(ConfigurationManager.GetSection("AlertManagerConfig"), IDictionary)("ShowExtendedAlerts")) Then
                    'claimAlertManager.AddAlertRow(New Object() {"Line " & FactToApply.LineNumber & ": Rule used: " & FactToApply.RuleSetName & " (" & FactToApply.RuleSetIdUsed.ToString & ")", FactToApply.LineNumber, "Detail", 10})
                    claimAlertManager.AddAlertRow(New Object() {"Line " & FactToApply.LineNumber & ": Rule Set Used: " & String.Format("{0}({1})", PlanController.GetRuleSetNameByRulesetID(FactToApply.RuleSetIdUsed), FactToApply.RuleSetIdUsed), FactToApply.LineNumber, "Detail", 10})
                End If

                If FactToApply.IsPreventative Then 'preventative rules was used
                    Dim DenyRuleExists As Boolean = False

                    For Each FactRule As Rule In FactToApply.Rules
                        If TypeOf FactRule Is DenyRule Then
                            DenyRuleExists = True
                            Exit For
                        End If
                    Next

                    If procedure.Diagnosis.Trim.Length = 0 Then ' rule had no diagnosis component
                        claimAlertManager.AddAlertRow(New Object() {"Line " & FactToApply.LineNumber & ": Line was processed as Preventative", FactToApply.LineNumber, "Preventative", _PreventativeReview})
                    ElseIf procedure.Diagnosis = ClaimBinderItem.PrimaryDiagnosis Then
                        claimAlertManager.AddAlertRow(New Object() {"Line " & FactToApply.LineNumber & ": Line was processed as Preventative using Primary Diagnosis (" & ClaimBinderItem.Procedure.Diagnosis & ")", FactToApply.LineNumber, "Preventative", _PreventativeReview})
                    ElseIf DenyRuleExists Then
                        claimAlertManager.AddAlertRow(New Object() {"Line " & FactToApply.LineNumber & ": Line was denied but may be Preventative using Diagnosis (" & ClaimBinderItem.Procedure.Diagnosis & ")", FactToApply.LineNumber, "Preventative", _PreventativeReview})
                    Else
                        claimAlertManager.AddAlertRow(New Object() {"Line " & FactToApply.LineNumber & ": Line was processed as Preventative using Diagnosis (" & ClaimBinderItem.Procedure.Diagnosis & ")", FactToApply.LineNumber, "Preventative", _PreventativeReview})
                    End If

                End If

                MeddtlDR.BeginEdit() 'prevent rowchanged from firing multiple times

                If FactToApply.Status = BinderItemStatus.InvalidAccumulator Then
                    claimAlertManager.AddAlertRow(New Object() {"Line " & FactToApply.LineNumber & ": Binder Processing Failed due to Invalid Accumulator", FactToApply.LineNumber, "Binder", 30})

                ElseIf FactToApply.Status = BinderItemStatus.Failed Then
                    If UFCWGeneral.NowDate < CDate(MeddtlDR("OCC_FROM_DATE")) Then
                        claimAlertManager.AddAlertRow(New Object() {"Line " & FactToApply.LineNumber & ": Invalid Date Of Service", FactToApply.LineNumber, "Binder", 30})
                    Else
                        claimAlertManager.AddAlertRow(New Object() {"Line " & FactToApply.LineNumber & ": Binder Processing Failed", FactToApply.LineNumber, "Binder", 30})
                    End If

                ElseIf FactToApply.Status = BinderItemStatus.NeedsReprice Then
                    claimAlertManager.AddAlertRow(New Object() {"Line " & FactToApply.LineNumber & ": Needs to be Re-Priced", FactToApply.LineNumber, "Reprice", 30})

                ElseIf FactToApply.Status = BinderItemStatus.Denied OrElse ClaimBinderItem.Procedure.IsProviderWriteOffRule(_RuleSetType) Then

                    If IsDBNull(MeddtlDR("PROC_ID")) OrElse CInt(MeddtlDR("PROC_ID")) <> ClaimBinderItem.Procedure.ProcedureID Then
                        MeddtlDR("PROC_ID") = ClaimBinderItem.Procedure.ProcedureID
                    End If

                    If MeddtlDR("STATUS").ToString.Trim <> "DENY" Then
                        MeddtlDR("STATUS") = "DENY"
                    End If

                    If Not IsDBNull(MeddtlDR("PAID_AMT")) Then
                        MeddtlDR("PAID_AMT") = DBNull.Value
                    End If

                    If Not IsDBNull(MeddtlDR("PROCESSED_AMT")) Then
                        MeddtlDR("PROCESSED_AMT") = DBNull.Value
                    End If

                    'Reason Code
                    If ClaimBinderItem.Procedure.IsProviderWriteOffRule(_RuleSetType) Then

                        Dim ReasonFilter As String = If(IsJAA(claimDS.Tables("MEDHDR").Rows(0)), "FFW", "PWO")
                        Dim QueryREASON =
                            From REASON In claimDS.Tables("MEDDTL").AsEnumerable
                            Where REASON.RowState <> DataRowState.Deleted _
                            AndAlso REASON.Field(Of Short)("LINE_NBR") = LineNbr _
                            AndAlso REASON.Field(Of String)("REASON") = ReasonFilter
                            Select REASON

                        Dim REASONDR As DataRow = QueryREASON.FirstOrDefault

                        If REASONDR IsNot Nothing Then

                            AddReason(claimDS, MeddtlDR, meddtlCurrentRowsDT, FactToApply, claimAlertManager)

                        End If
                    End If
                Else

                    If IsDBNull((MeddtlDR("HRA_EXCLUDE"))) OrElse (CBool(MeddtlDR("HRA_EXCLUDE"))) <> FactToApply.HRAInEligible Then MeddtlDR("HRA_EXCLUDE") = FactToApply.HRAInEligible

                    If (procedure.ProcedureID Is Nothing OrElse IsDBNull(MeddtlDR("PROC_ID"))) OrElse CInt(MeddtlDR("PROC_ID")) <> procedure.ProcedureID Then
                        MeddtlDR("PROC_ID") = procedure.ProcedureID
                    End If

                    If Not IsDBNull(MeddtlDR("PAID_AMT")) Then
                        If CDec(FactToApply.PaymentAmount) <> CDec(MeddtlDR("PAID_AMT")) Then
                            MeddtlDR("PAID_AMT") = FactToApply.PaymentAmount
                        End If
                    Else
                        MeddtlDR("PAID_AMT") = FactToApply.PaymentAmount
                    End If

                    If Not IsDBNull(MeddtlDR("PROCESSED_AMT")) Then
                        If CDec(FactToApply.PaymentAmount) <> CDec(MeddtlDR("PROCESSED_AMT")) Then
                            MeddtlDR("PROCESSED_AMT") = FactToApply.PaymentAmount
                        End If
                    Else
                        MeddtlDR("PROCESSED_AMT") = FactToApply.PaymentAmount
                    End If

                    If MeddtlDR("STATUS").ToString.Trim <> "PAY" Then
                        MeddtlDR("STATUS") = "PAY"
                    End If

                End If

                ClaimBinderItem = CType(claimBinder.GetBinderItem(FactToApply.LineNumber), MedicalBinderItem)

                If Not IsDBNull(MeddtlDR("RULE_SET_ID")) Then
                    If CInt(MeddtlDR("RULE_SET_ID")) <> ClaimBinderItem.RuleSetIDUsed Then
                        MeddtlDR("RULE_SET_ID") = ClaimBinderItem.RuleSetIDUsed
                    End If
                Else
                    MeddtlDR("RULE_SET_ID") = ClaimBinderItem.RuleSetIDUsed
                End If

                MeddtlDR.EndEdit()

            Next 'f

            If claimBinder.BinderAlertManager.Count > 0 Then
                For Each BinderAlert As Alert In claimBinder.BinderAlertManager
                    claimAlertManager.AddAlertRow(New Object() {"Line " & BinderAlert.LineNumber & ": " & BinderAlert.AlertMessage, BinderAlert.LineNumber, BinderAlert.Category.ToString, CInt(BinderAlert.Severity)})
                Next 'alrt
            End If

        Catch ex As Exception
            Throw
        Finally

            '#If TRACE Then
            '            If CInt(_TraceParallel.Level) > 0 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : BinderAlertManager Count : " & claimBinder.BinderAlertManager.Count.ToString & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
            '#End If

        End Try

    End Sub

    Private Shared Sub ProcessDenyStatus(claimAlertManager As AlertManagerCollection, meddtlDR As DataRow)

        Try

            claimAlertManager.DeleteAlertRowsByMessageAndLine("'Line " & meddtlDR("LINE_NBR").ToString & ": Paid Is More Than Priced'", CInt(meddtlDR("LINE_NBR")))
            claimAlertManager.DeleteAlertRowsByMessageAndLine("'Line " & meddtlDR("LINE_NBR").ToString & ": Paid Is 0 and a Reason is Required'", CInt(meddtlDR("LINE_NBR")))

            If IsDBNull(meddtlDR("PAID_AMT")) OrElse CDec(meddtlDR("PAID_AMT")) <> 0 Then meddtlDR("PAID_AMT") = 0D
            If IsDBNull(meddtlDR("PROCESSED_AMT")) OrElse CDec(meddtlDR("PROCESSED_AMT")) <> 0 Then meddtlDR("PROCESSED_AMT") = 0D

            If Not IsDBNull(meddtlDR("PRICED_AMT")) AndAlso CDec(meddtlDR("PAID_AMT")) > CDec(Format(meddtlDR("PRICED_AMT"), "0.00")) Then
                claimAlertManager.AddAlertRow(New Object() {"Line " & meddtlDR("LINE_NBR").ToString & ": Paid Is More Than Priced", meddtlDR("LINE_NBR").ToString, "Detail", 20})
            End If

            If Not CBool(meddtlDR("REASON_SW")) AndAlso CDec(meddtlDR("PAID_AMT")) = 0 Then
                claimAlertManager.AddAlertRow(New Object() {"Line " & meddtlDR("LINE_NBR").ToString & ": Paid Is 0 and a Reason is Required", meddtlDR("LINE_NBR").ToString, "Detail", 30})
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    'Private Shared Sub ProcessHospitalBinder(ByVal claimBinder As Binder, ByVal claimAccumulatorManager As MemberAccumulatorManager, ByVal ruleSetType As Integer?, ByVal origTotalPricedAmt As Decimal, ByVal meddtlNonMergedRowsDT As DataTable, ByRef claimDS As DataSet)

    '    Dim TotalPermittedChrgAmt As Decimal = 0
    '    Dim TotalDeniedChrgAmt As Decimal = 0

    '    Dim LineChrgAmt As Decimal
    '    Dim LinePricedAmt As Decimal

    '    Dim TotalPricedAmt As Decimal = 0

    '    Dim RoundedAmt As Decimal = 0

    '    Dim ClaimBinderItem As BinderItem
    '    Dim MedDTLDV As DataView

    '    Try

    '        'check that accumulators have been validated since system conversion
    '        If CInt(claimAccumulatorManager.GetOriginalLifetimeValue(CInt(AccumulatorController.GetAccumulatorID("FIXAC")))) <= 0 Then Return

    '        For Each DR As DataRow In meddtlNonMergedRowsDT.Rows
    '            ClaimBinderItem = claimBinder.GetBinderItem(CInt(DR("LINE_NBR")))

    '            If ClaimBinderItem IsNot Nothing Then
    '                If ClaimBinderItem.Procedure.IsDenyRule(ruleSetType) = False Then
    '                    If IsDBNull(DR("CHRG_AMT")) = False Then
    '                        TotalPermittedChrgAmt += CDec(DR("CHRG_AMT"))
    '                    End If
    '                Else
    '                    If IsDBNull(DR("CHRG_AMT")) = False Then
    '                        TotalDeniedChrgAmt += CDec(DR("CHRG_AMT"))
    '                    End If
    '                End If
    '            End If

    '        Next

    '        If TotalPermittedChrgAmt = 0 Then Exit Try

    '        MedDTLDV = claimDS.Tables("MEDDTL").DefaultView

    '        MedDTLDV.RowFilter = "STATUS <> 'MERGE'"
    '        MedDTLDV.Sort = "LINE_NBR"

    '        MedDTLDV.AllowEdit = True

    '        'Apply Amounts
    '        For Each DRV As DataRowView In MedDTLDV
    '            ClaimBinderItem = claimBinder.GetBinderItem(CInt(DRV("LINE_NBR")))

    '            If ClaimBinderItem IsNot Nothing Then
    '                If Not ClaimBinderItem.Procedure.IsDenyRule(ruleSetType) Then

    '                    If IsDBNull(DRV("CHRG_AMT")) = False Then
    '                        LineChrgAmt = CDec(DRV("CHRG_AMT"))
    '                    Else
    '                        LineChrgAmt = 0
    '                    End If

    '                    If LineChrgAmt <> 0 Then
    '                        LinePricedAmt = ((LineChrgAmt / TotalPermittedChrgAmt) * origTotalPricedAmt) + RoundedAmt
    '                    Else
    '                        LinePricedAmt = 0
    '                    End If

    '                    RoundedAmt = CDec(CDbl(Format(LinePricedAmt, "0.0000")) - LinePricedAmt)

    '                    TotalPricedAmt += CDec(Format(LinePricedAmt, "0.00"))

    '                    If IsDBNull(DRV("PRICED_AMT")) = False Then
    '                        If LinePricedAmt <> CDec(DRV("PRICED_AMT")) Then
    '                            DRV("PRICED_AMT") = Format(LinePricedAmt, "0.00")
    '                        End If
    '                    Else
    '                        DRV("PRICED_AMT") = Format(LinePricedAmt, "0.00")
    '                    End If

    '                    If IsDBNull(DRV("ALLOWED_AMT")) = False Then
    '                        If CDec(DRV("ALLOWED_AMT")) > CDec(DRV("PRICED_AMT")) Then
    '                            DRV("ALLOWED_AMT") = DRV("PRICED_AMT")
    '                        End If
    '                    Else
    '                        If Not DRV("ALLOWED_AMT").Equals(DRV("PRICED_AMT")) Then DRV("ALLOWED_AMT") = DRV("PRICED_AMT")
    '                    End If

    '                    If IsDBNull(DRV("ORIG_PRICED_AMT")) = True Then
    '                        DRV("ORIG_PRICED_AMT") = Format(LinePricedAmt, "0.00")
    '                    End If
    '                Else 'deny rule
    '                    If IsDBNull(DRV("PRICED_AMT")) = False Then
    '                        If LinePricedAmt <> 0 Then
    '                            DRV("PRICED_AMT") = Format(0, "0.00")
    '                        End If
    '                    Else
    '                        DRV("PRICED_AMT") = Format(0, "0.00")
    '                    End If

    '                    If IsDBNull(DRV("ALLOWED_AMT")) = False Then
    '                        If CDec(DRV("ALLOWED_AMT")) > CDec(DRV("PRICED_AMT")) Then
    '                            DRV("ALLOWED_AMT") = DRV("PRICED_AMT")
    '                        End If
    '                    Else
    '                        DRV("ALLOWED_AMT") = DRV("PRICED_AMT")
    '                    End If

    '                    If IsDBNull(DRV("ORIG_PRICED_AMT")) = True Then
    '                        DRV("ORIG_PRICED_AMT") = Format(0, "0.00")
    '                    End If

    '                End If
    '            End If

    '        Next

    '        If TotalPricedAmt <> origTotalPricedAmt Then
    '            RoundedAmt = origTotalPricedAmt - TotalPricedAmt

    '            MedDTLDV.Sort = "LINE_NBR DESC"

    '            For Each DRV As DataRowView In MedDTLDV

    '                ClaimBinderItem = claimBinder.GetBinderItem(CInt(DRV("LINE_NBR")))

    '                If ClaimBinderItem IsNot Nothing Then
    '                    If Not ClaimBinderItem.Procedure.IsDenyRule(ruleSetType) Then

    '                        LinePricedAmt = CDec(DRV("PRICED_AMT"))

    '                        If LinePricedAmt <> 0 Then
    '                            LinePricedAmt += RoundedAmt

    '                            If Not IsDBNull(DRV("PRICED_AMT")) Then
    '                                If LinePricedAmt <> CDec(DRV("PRICED_AMT")) Then
    '                                    DRV("PRICED_AMT") = Format(LinePricedAmt, "0.00")
    '                                End If
    '                            Else
    '                                DRV("PRICED_AMT") = Format(LinePricedAmt, "0.00")
    '                            End If

    '                            If Not IsDBNull(DRV("ALLOWED_AMT")) Then
    '                                If CDec(DRV("ALLOWED_AMT")) > CDec(DRV("PRICED_AMT")) Then
    '                                    DRV("ALLOWED_AMT") = DRV("PRICED_AMT")
    '                                End If
    '                            Else
    '                                DRV("ALLOWED_AMT") = DRV("PRICED_AMT")
    '                            End If

    '                            Exit For
    '                        End If
    '                    End If
    '                End If

    '            Next

    '        End If

    '    Catch ex As Exception
    '        Throw
    '    Finally
    '        claimDS.Tables("MEDDTL").DefaultView.RowFilter = ""
    '        claimDS.Tables("MEDDTL").DefaultView.Sort = ""
    '    End Try

    'End Sub
    'Public Shared Sub PreProcessHospital(ByRef claimAccumulatorManager As MemberAccumulatorManager, ByRef claimDS As DataSet)
    '    'same as loadDetailLineAccumlators but no Accumulator manipulation

    '    Dim ProcedureCollection As Procedures
    '    Dim Procedure As ProcedureActive
    '    Dim Procedures As Hashtable
    '    Dim ProceduresQ As System.Collections.Generic.Queue(Of ProcedureActive)

    '    Dim PercentMatch As Integer

    '    Dim ClaimBinder As MedicalBinder
    '    Dim ClaimBinderItem As MedicalBinderItem
    '    Dim BinderItemCount As Integer? = 0

    '    Dim ClaimAlertManager As AlertManager

    '    Dim DocClass As String
    '    Dim PlaceOfServ As String = ""
    '    Dim Prov As String = ""
    '    Dim Diagnoses() As String = {"", "", "", ""}
    '    Dim BillType As String = ""
    '    Dim Modifier As String = ""
    '    Dim Gender As String = ""
    '    Dim AgeMin As String = ""
    '    Dim AgeMax As String = ""

    '    Dim RuleSetTypeName As String

    '    Dim MeddtlNonMergedRowsDT As DataTable

    '    Dim OrigTotalPricedAmt As Decimal = 0

    '    Try

    '        If (claimDS.Tables("MEDHDR").Rows(0)("PRICED_BY").ToString.Contains("JAA") OrElse claimDS.Tables("MEDHDR").Rows(0)("PRICED_BY").ToString.Contains("835")) Then Return 'Pricing was already received at the line, therefore distributing summary pricing is not allowed

    '        ClaimAlertManager = New AlertManager

    '        DocClass = CStr(claimDS.Tables("CLAIM_MASTER").Rows(0)("DOC_CLASS"))

    '        RuleSetTypeName = "Hospital"
    '        _RuleSetType = PlanController.GetRulesetTypeID(RuleSetTypeName)

    '        MeddtlNonMergedRowsDT = claimDS.Tables("MEDDTL").Clone

    '        claimDS.Tables("MEDDTL").Select("", "", DataViewRowState.CurrentRows) 'remove deleted rows and Merged items from plan processing
    '        For Each MeddtlCurrentDataRowView As DataRowView In claimDS.Tables("MEDDTL").DefaultView
    '            If MeddtlCurrentDataRowView("STATUS").ToString.Trim <> "MERGED" Then
    '                MeddtlCurrentDataRowView.Row.EndEdit()
    '                MeddtlNonMergedRowsDT.ImportRow(MeddtlCurrentDataRowView.Row)
    '            End If
    '        Next

    '        If claimDS.Tables("MEDDTL").Rows.Count > 0 Then

    '            If claimDS.Tables("MEDHDR").Rows.Count > 0 Then
    '                If IsDBNull(claimDS.Tables("MEDHDR").Rows(0)("TOT_PRICED_AMT")) = False Then
    '                    OrigTotalPricedAmt = CDec(claimDS.Tables("MEDHDR").Rows(0)("TOT_PRICED_AMT"))
    '                End If

    '            End If

    '            If OrigTotalPricedAmt = 0 Then Return

    '            'build Binder to establish Pay/Deny lines for pricing distribution
    '            ClaimBinder = CType(BinderFactory.CreateBinder(CInt(claimDS.Tables("CLAIM_MASTER").Rows(0)("CLAIM_ID")), DocClass, _RuleSetType), MedicalBinder)

    '            For Each DR As DataRow In MeddtlNonMergedRowsDT.Rows 'process each non merged line item

    '                If IsDBNull(DR("MED_PLAN")) = False AndAlso DR("MED_PLAN").ToString.Trim.Length > 0 AndAlso Not IsDBNull(DR("PROC_CODE")) Then

    '                    BinderItemCount = ManageProcedures(Nothing, DR, BinderItemCount, PercentMatch, Procedure, Procedures, ProceduresQ, ProcedureCollection, ClaimBinder, ClaimBinderItem, claimDS, MeddtlNonMergedRowsDT)

    '                    If Procedure Is Nothing OrElse BinderItemCount Is Nothing Then Return

    '                End If

    '            Next

    '            If BinderItemCount > 0 AndAlso ClaimBinder IsNot Nothing Then

    '                ProcessHospitalBinder(ClaimBinder, claimAccumulatorManager, _RuleSetType, OrigTotalPricedAmt, MeddtlNonMergedRowsDT, claimDS)

    '            End If

    '        End If

    '    Catch ex As Exception
    '        Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '        If (rethrow) Then
    '            Throw
    '        End If
    '    Finally
    '        If ClaimAlertManager IsNot Nothing Then ClaimAlertManager.Dispose()
    '        ClaimAlertManager = Nothing
    '    End Try
    'End Sub
    Private Shared Sub ValidateBillType(meddtlDataRowView As DataRowView, ByVal billType As String, ByVal dateOfService As Date?, ByRef claimAlertManager As AlertManagerCollection)

        Dim BillTypeValueDR As DataRow

        Try
            BillTypeValueDR = CMSDALFDBMD.RetrieveBillTypeValuesInformation(billType, dateOfService)

            If BillTypeValueDR Is Nothing Then

                If IsDBNull(meddtlDataRowView("BILL_TYPE_DESC")) OrElse meddtlDataRowView("BILL_TYPE_DESC").ToString.Trim <> "***INVALID BILL TYPE***" Then
                    meddtlDataRowView.Row("BILL_TYPE_DESC") = "***INVALID BILL TYPE***" 'DBNull.Value
                End If

                claimAlertManager.AddAlertRow(New Object() {"Line " & meddtlDataRowView("LINE_NBR").ToString & ": Invalid Bill Type", meddtlDataRowView("LINE_NBR").ToString, "Detail", 30})

            Else
                'Valid BILLTYPE
                If IsDBNull(meddtlDataRowView("BILL_TYPE_DESC")) OrElse meddtlDataRowView("BILL_TYPE_DESC").ToString.Trim <> BillTypeValueDR("FULL_DESC").ToString Then
                    meddtlDataRowView.Row("BILL_TYPE_DESC") = BillTypeValueDR("FULL_DESC")
                End If

                claimAlertManager.DeleteAlertRowsByMessageAndLine("Line " & meddtlDataRowView("LINE_NBR").ToString & ": Invalid Bill Type", CInt(meddtlDataRowView("LINE_NBR")))

            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
End Class