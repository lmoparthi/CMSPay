Option Explicit On
'Option Strict On
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling


''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.RangeManager
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' A Class that handles and manages ranges
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	1/25/2006	Created
'''     [paulw] 9/29/2006   Added Wild Card Procedure Support
''' </history>
''' -----------------------------------------------------------------------------
Public Class RangeManager

#Region "Variables"
    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
    Private Shared _RuleSetTypes As Collection

#End Region

#Region "Functions"

    Public Shared Function GetProcedureRanges(ByVal procedures As Procedures) As DataSet
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets a strongly typed dataset of all the procedure ranges
        ' </summary>
        ' <param name="procedures">The ProcedureCollection object</param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	7/18/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim RangeDS As Range
        Dim ProcedureRangeDT As Range.ProcedureRangeDataTable
        Dim ProcedureRangeDR As Range.ProcedureRangeRow
        Dim ProcedureAttributes As Procedures
        Dim Procedure As Procedure
        Dim ActiveProcedure As ProcedureActive
        Dim TempDT As Range.ProcedureRangeDataTable

        Try

            RangeDS = New Range
            ProcedureRangeDT = New Range.ProcedureRangeDataTable
            ProcedureAttributes = New Procedures

            procedures.Sort()
            ProcedureRangeDR = CType(ProcedureRangeDT.NewRow, Range.ProcedureRangeRow)

            'try: get all combinations of modifiers, diagnosis, etc.
            For I As Integer = 0 To procedures.Count - 1
                Procedure = DirectCast(procedures(I), Procedure)

                ActiveProcedure = New ProcedureActive(-1)
                ActiveProcedure.BillType = Procedure.BillType
                ActiveProcedure.Diagnosis = Procedure.Diagnosis
                ActiveProcedure.Modifier = Procedure.Modifier
                ActiveProcedure.PlaceOfService = Procedure.PlaceOfService
                ActiveProcedure.Provider = Procedure.Provider
                Procedure.PlanType = Procedure.PlanType

                If Not ProcedureAttributes.Contains(ActiveProcedure) Then
                    ProcedureAttributes.Add(ActiveProcedure)
                End If
            Next

            RangeDS.ProcedureRange.BeginLoadData()
            For I As Integer = 0 To ProcedureAttributes.Count - 1
                Procedure = DirectCast(ProcedureAttributes(I), Procedure)
                TempDT = GetProcedureRangesByAttributes(Procedure, procedures)

                For J As Integer = 0 To TempDT.Rows.Count - 1
                    RangeDS.ProcedureRange.ImportRow(TempDT.Rows(J))
                Next
            Next
            RangeDS.ProcedureRange.EndLoadData()

            Return RangeDS

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Shared Function GetProcedureRangesByAttributes(ByVal procCompare As Procedure, ByVal procedures As Procedures) As Range.ProcedureRangeDataTable
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Goes through each procedure in the collection and sees if the
        '  attrivutes match the procedure passed in
        ' </summary>
        ' <param name="procCompare"></param>
        ' <param name="procedures"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	7/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim ProcedureRangeDT As Range.ProcedureRangeDataTable
        Dim ProcedureRangeDR As Range.ProcedureRangeRow
        Dim JustAddedProcedureRangeDR As Range.ProcedureRangeRow
        Dim RuleSetType As RuleSetType
        Dim ProcedureCollection As Procedures
        Dim Procedure As Procedure
        Dim BeginProcedure As String
        Dim BeginProcedureRuleSetName As String
        Dim BeginProcedureRuleSetType As String
        Dim PrevProcedure As Procedure
        Dim RuleSets As Rulesets

        Try
            ProcedureRangeDT = New Range.ProcedureRangeDataTable
            ProcedureRangeDR = CType(ProcedureRangeDT.NewRow, Range.ProcedureRangeRow)

            ProcedureCollection = New Procedures

            For I As Integer = 0 To procedures.Count - 1
                Procedure = DirectCast(procedures(I), Procedure)
                If Procedure.BillType = procCompare.BillType AndAlso Procedure.Diagnosis(0) = procCompare.Diagnosis(0) AndAlso _
                    Procedure.Modifier = procCompare.Modifier AndAlso Procedure.PlaceOfService = procCompare.PlaceOfService AndAlso _
                    Procedure.Provider = procCompare.Provider Then
                    ProcedureCollection.Add(Procedure)
                End If
            Next

            BeginProcedure = ProcedureCollection(0).ProcedureCode

            If ProcedureCollection(0).Rulesets.Count < 1 Then
                BeginProcedureRuleSetName = ""
                BeginProcedureRuleSetType = ""
            Else
                BeginProcedureRuleSetName = ProcedureCollection(0).Rulesets(0).RuleSetName
                If BeginProcedureRuleSetName.Length = 0 Then
                    BeginProcedureRuleSetType = "NOT ASSIGNED"
                Else

                    For I As Integer = 1 To _RuleSetTypes.Count
                        RuleSetType = CType(_RuleSetTypes(I), RuleSetType)
                        If RuleSetType.Id = ProcedureCollection(0).Rulesets(0).RulesetType Then
                            BeginProcedureRuleSetType = RuleSetType.Name
                        End If
                    Next
                    '    Case Ruleset.RulesetTypes.Accident
                    'beginProcRSType = "ACCIDENT"
                    '    Case Ruleset.RulesetTypes.Chiro
                    'beginProcRSType = "CHIRO"
                    '    Case Ruleset.RulesetTypes.Standard
                    'beginProcRSType = "GENERAL"
                    'End Select
                End If
            End If

            ProcedureCollection.Sort()

            For I As Integer = 0 To ProcedureCollection.Count - 1
                Procedure = DirectCast(ProcedureCollection(I), Procedure)
                If Procedure.RuleSets.Count < 1 Then
                    RuleSets = DirectCast(Procedure, ProcedureStaged).NewRuleSets
                Else
                    RuleSets = Procedure.RuleSets
                End If

                If RuleSets.Count > 0 Then
                    For Each rlS As RuleSet In RuleSets
                        If rlS.RuleSetName <> BeginProcedureRuleSetName Then
                            JustAddedProcedureRangeDR = ProcedureRangeDT.NewProcedureRangeRow
                            ProcedureRangeDR = ProcedureRangeDT.NewProcedureRangeRow
                            ProcedureRangeDR.BeginProcedure = BeginProcedure : JustAddedProcedureRangeDR.BeginProcedure = BeginProcedure
                            ProcedureRangeDR.EndProcedure = PrevProcedure.ProcedureCode : JustAddedProcedureRangeDR.EndProcedure = PrevProcedure.ProcedureCode
                            ProcedureRangeDR.BillType = Procedure.BillType : JustAddedProcedureRangeDR.BillType = Procedure.BillType
                            ProcedureRangeDR.Diagnosis = Procedure.Diagnosis : JustAddedProcedureRangeDR.Diagnosis = Procedure.Diagnosis
                            ProcedureRangeDR.Modifier = Procedure.Modifier : JustAddedProcedureRangeDR.Modifier = Procedure.Modifier
                            ProcedureRangeDR.PlaceOfService = Procedure.PlaceOfService : JustAddedProcedureRangeDR.PlaceOfService = Procedure.PlaceOfService
                            ProcedureRangeDR.Provider = Procedure.Provider : JustAddedProcedureRangeDR.Provider = Procedure.Provider
                            ProcedureRangeDR.PlanType = Procedure.PlanType : JustAddedProcedureRangeDR.PlanType = Procedure.PlanType
                            ProcedureRangeDR.RulesetName = BeginProcedureRuleSetName : JustAddedProcedureRangeDR.RulesetName = BeginProcedureRuleSetName
                            If ProcedureRangeDR.RulesetName.ToString.Length < 1 Then
                                ProcedureRangeDR.RulesetName = "NOT ASSIGNED"
                            End If
                            BeginProcedureRuleSetName = rlS.RuleSetName
                            ProcedureRangeDR.RulesetType = BeginProcedureRuleSetType : JustAddedProcedureRangeDR.RulesetType = BeginProcedureRuleSetType

                            For j As Integer = 1 To _RuleSetTypes.Count
                                RuleSetType = CType(_RuleSetTypes(j), RuleSetType)
                                If RuleSetType.Id = rlS.RulesetType Then
                                    BeginProcedureRuleSetType = RuleSetType.Name
                                End If
                            Next
                            BeginProcedure = Procedure.ProcedureCode
                            ProcedureRangeDT.Rows.Add(ProcedureRangeDR)
                        End If
                        PrevProcedure = Procedure
                    Next
                Else
                    If BeginProcedureRuleSetName <> "" Then
                        JustAddedProcedureRangeDR = ProcedureRangeDT.NewProcedureRangeRow
                        ProcedureRangeDR = ProcedureRangeDT.NewProcedureRangeRow
                        ProcedureRangeDR.BeginProcedure = BeginProcedure : JustAddedProcedureRangeDR.BeginProcedure = BeginProcedure
                        ProcedureRangeDR.EndProcedure = PrevProcedure.ProcedureCode : JustAddedProcedureRangeDR.EndProcedure = PrevProcedure.ProcedureCode
                        ProcedureRangeDR.BillType = Procedure.BillType : JustAddedProcedureRangeDR.BillType = Procedure.BillType
                        ProcedureRangeDR.Diagnosis = Procedure.Diagnosis : JustAddedProcedureRangeDR.Diagnosis = Procedure.Diagnosis
                        ProcedureRangeDR.Modifier = Procedure.Modifier : JustAddedProcedureRangeDR.Modifier = Procedure.Modifier
                        ProcedureRangeDR.PlaceOfService = Procedure.PlaceOfService : JustAddedProcedureRangeDR.PlaceOfService = Procedure.PlaceOfService
                        ProcedureRangeDR.Provider = Procedure.Provider : JustAddedProcedureRangeDR.Provider = Procedure.Provider
                        ProcedureRangeDR.PlanType = Procedure.PlanType : JustAddedProcedureRangeDR.PlanType = Procedure.PlanType
                        ProcedureRangeDR.RulesetName = BeginProcedureRuleSetName : JustAddedProcedureRangeDR.RulesetName = "NOT ASSIGNED"
                        ProcedureRangeDR.RulesetType = BeginProcedureRuleSetType
                        BeginProcedureRuleSetName = ""
                        BeginProcedureRuleSetType = ""
                        BeginProcedure = Procedure.ProcedureCode
                        ProcedureRangeDT.Rows.Add(ProcedureRangeDR)
                    End If
                End If
                PrevProcedure = Procedure
            Next

            ProcedureRangeDR = ProcedureRangeDT.NewProcedureRangeRow
            ProcedureRangeDR.BeginProcedure = BeginProcedure
            ProcedureRangeDR.EndProcedure = PrevProcedure.ProcedureCode
            ProcedureRangeDR.BillType = PrevProcedure.BillType
            ProcedureRangeDR.Diagnosis = PrevProcedure.Diagnosis
            ProcedureRangeDR.Modifier = PrevProcedure.Modifier
            ProcedureRangeDR.PlaceOfService = PrevProcedure.PlaceOfService
            ProcedureRangeDR.Provider = PrevProcedure.Provider
            ProcedureRangeDR.PlanType = PrevProcedure.PlanType
            ProcedureRangeDR.RulesetName = If(BeginProcedureRuleSetName.Length < 1, "NOT ASSIGNED", BeginProcedureRuleSetName)
            ProcedureRangeDR.RulesetType = BeginProcedureRuleSetType

            If Not JustAddedProcedureRangeDR Is ProcedureRangeDR Then
                ProcedureRangeDT.Rows.Add(ProcedureRangeDR)
            End If

            Return ProcedureRangeDT

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function GetProcedureRanges(ByVal staged As Boolean, ByVal ParamArray planTypes() As String) As DataSet
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets the procedures ranges in a strongly typed dataset
        ' </summary>
        ' <param name="planType">The plan type</param>
        ' <param name="staged">True if staged procedures are needed. False if active procedures are needed.</param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	7/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim RangeDS As Range
        Dim ReturnRangeDs As Range
        Dim RangeDT As Range.ProcedureRangeDataTable
        Dim Range2DT As Range.ProcedureRangeDataTable

        Try

            ReturnRangeDs = New Range

            _RuleSetTypes = GeneralDataDAL.GetRulesetTypes

            ReturnRangeDs.Tables.Clear()

            RangeDT = New Range.ProcedureRangeDataTable

            For Each plantype As String In planTypes
                If staged Then
                    RangeDS = CType(GetProcedureRanges(PlanStagingDAL.GetStagedProcedures(plantype.Trim, False)), Range)
                Else
                    RangeDS = CType(GetProcedureRanges(PlanActiveDAL.GetActiveProcedures(plantype.Trim, False)), Range)
                End If

                If RangeDS.Tables.Count > 0 Then
                    RangeDT.BeginLoadData()
                    Range2DT = CType(RangeDS.Tables(0), Range.ProcedureRangeDataTable)

                    For I As Integer = 0 To Range2DT.Rows.Count - 1
                        Range2DT.Rows(I).EndEdit()
                        RangeDT.ImportRow(Range2DT.Rows(I))
                    Next
                    RangeDT.EndLoadData()
                End If
            Next

            ReturnRangeDs.Tables.Add(RangeDT)
            Return ReturnRangeDs

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function IsRangeValid(ByVal procedureFrom As String, ByVal procedureTo As String) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' determines if the procedure range is valid and returns that range
        ' </summary>
        ' <param name="procedureFrom"></param>
        ' <param name="procedureTo"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	11/18/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim ToChrBegin As String, ToChrEnd As String, FromChrBegin As String, FromChrEnd As String

        Try
            If procedureTo.Trim.Length < 1 Then Return False
            If procedureFrom.Trim.Length < 1 Then Return False

            ToChrEnd = procedureTo.Trim.Substring(procedureTo.Trim.Length - 1, 1)
            ToChrBegin = procedureTo.Trim.Substring(0, 1)
            FromChrEnd = procedureFrom.Trim.Substring(procedureFrom.Trim.Length - 1, 1)
            FromChrBegin = procedureFrom.Trim.Substring(0, 1)

            If procedureTo.Trim = procedureFrom.Trim Then Return True

            'FROM CHECK
            If procedureFrom.Length = 1 Then
                If Not IsNumeric(FromChrBegin) Then
                    Return False
                End If
            ElseIf IsNumeric(FromChrBegin) Then  'the first character is a number
                If procedureFrom.Trim.Length = 2 Then
                    If Not IsNumeric(FromChrEnd) Then
                        Return False
                    End If
                Else
                    'all chars, except the last one, numeric
                    If Not IsNumeric(procedureFrom.Trim.Substring(0, procedureFrom.Trim.Length - 2)) Then
                        Return False
                    End If
                    'the last one is an alpha
                    If Not IsNumeric(FromChrEnd) Then
                        'make sure both have the same end char
                        If FromChrEnd <> ToChrEnd Then
                            Return False
                        End If
                    End If
                End If
            Else 'the first character is not a number..it is an alpha
                'if the rest of it is not numeric
                If Not IsNumeric(procedureFrom.Trim.Substring(1)) Then
                    Return False
                End If
                'make sure both have the same begin char
                If FromChrBegin <> ToChrBegin Then
                    Return False
                End If
            End If

            'TOCHECK
            If procedureTo.Length = 1 Then
                If Not IsNumeric(ToChrBegin) Then
                    Return False
                End If
            ElseIf IsNumeric(ToChrBegin) Then 'the first character is a number
                If procedureTo.Trim.Length = 2 Then
                    If Not IsNumeric(ToChrEnd) Then
                        Return False
                    End If
                Else
                    'all chars, except the last one, numeric
                    If Not IsNumeric(procedureTo.Trim.Substring(0, procedureTo.Trim.Length - 2)) Then
                        Return False
                    End If
                    'the last one is an alpha
                    If Not IsNumeric(ToChrEnd) Then
                        'make sure both have the same end char
                        If ToChrEnd <> FromChrEnd Then
                            Return False
                        End If
                    End If
                End If
            Else 'the first character is not a number..it is an alpha
                'if the rest of it is not numeric
                If Not IsNumeric(procedureTo.Trim.Substring(1)) Then
                    Return False
                End If
                'make sure both have the same begin char
                If FromChrBegin <> ToChrBegin Then
                    Return False
                End If
            End If

            'make sure that if one is numeric, both are numeric or vis a versa.
            If (IsNumeric(procedureFrom) AndAlso Not IsNumeric(procedureTo)) OrElse (IsNumeric(procedureTo) AndAlso Not IsNumeric(procedureFrom)) Then
                Return False
            End If

            'make sure that if they are numbers, that from is less than to
            If IsNumeric(procedureFrom) AndAlso IsNumeric(procedureTo) Then
                If procedureFrom > procedureTo Then
                    Return False
                End If
            End If
            If procedureFrom.Trim.Length <> procedureTo.Trim.Length Then
                Return False
            End If
            'if it makes it here then we know it is valid
            Return True
        Catch ex As Exception

            Throw New ProcedureRangeException("Cannot Create Procedure Range", ex)

        End Try
    End Function

    Public Shared Function CreateRange(ByVal beginProcedureCode As String, ByVal endProcedureCode As String) As ArrayList
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Creates a range of procdure Codes and puts them in an arraylist
        ' </summary>
        ' <param name="beginProcedureCode"></param>
        ' <param name="endProcedureCode"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim Range As New ArrayList

        Try
            'create procedure range - all numeric codes
            If beginProcedureCode = endProcedureCode Then
                Range.Add(beginProcedureCode)
                Return Range
            End If
            If IsNumeric(beginProcedureCode) Then
                Dim procCodeCount As Integer = CType(endProcedureCode, Integer) - CType(beginProcedureCode, Integer) + 1
                Dim procCodeIncr As Integer = CType(beginProcedureCode, Integer)
                Dim prefix As String = ""
                Dim Counter As Integer = 0

                While beginProcedureCode.Substring(Counter, 1) = "0"
                    Counter += 1
                    prefix += "0"
                End While

                For I As Integer = 1 To procCodeCount - 1
                    If PlanController.IsValidProcedureCode(prefix + CType(procCodeIncr, String), UFCWGeneral.NowDate) Then
                        Range.Add(prefix + CType(procCodeIncr, String))
                    End If
                    If (procCodeIncr + 1).ToString.Length > procCodeIncr.ToString.Length Then
                        If PlanController.IsValidProcedureCode(CType(procCodeIncr + 1, String), UFCWGeneral.NowDate) Then
                            Range.Add(CType(procCodeIncr + 1, String))
                        End If
                        Exit For
                    End If
                    procCodeIncr += 1
                Next I
            Else
                'create procedure range with alpha suffix
                If IsNumeric(beginProcedureCode.Substring(0, 1)) Then
                    Dim procCodeIncrString As String = ""
                    Dim suffix As String = beginProcedureCode.Substring(beginProcedureCode.Length - 1)
                    Dim procCodeCount As Integer = CType(endProcedureCode.Substring(0, endProcedureCode.Length - 1), Integer) - CType(beginProcedureCode.Substring(0, beginProcedureCode.Length - 1), Integer) + 1
                    Dim procCodeIncr As Integer = CType(beginProcedureCode.Substring(0, beginProcedureCode.Length - 1), Integer)

                    For I As Integer = 1 To procCodeCount
                        procCodeIncrString = procCodeIncr.ToString
                        For J As Integer = 0 To ((beginProcedureCode.Length - 1) - CType(procCodeIncr, String).Length) - 1
                            procCodeIncrString = "0" & CType(procCodeIncrString, String)
                        Next

                        Range.Add((CType(procCodeIncrString, String) & suffix.Trim()).Trim())
                        procCodeIncr += 1
                    Next
                Else 'create procedure range with alpha prefix
                    Dim prefix2 As String = ""
                    Dim prefix As String = beginProcedureCode.Substring(0, 1)
                    Dim procCodeCount As Integer = CType(endProcedureCode.Substring(1, endProcedureCode.Length - 1), Integer) - CType(beginProcedureCode.Substring(1, beginProcedureCode.Length - 1), Integer) + 1
                    Dim procCodeIncr As Integer = CType(beginProcedureCode.Substring(1, beginProcedureCode.Length - 1), Integer)

                    For I As Integer = 1 To procCodeCount
                        prefix2 = prefix.Trim
                        For J As Integer = 0 To ((beginProcedureCode.Length - 1) - CType(procCodeIncr, String).Length) - 1
                            prefix2 = prefix2 & "0"
                        Next
                        Range.Add(prefix2.Trim() + CType(procCodeIncr, String))
                        procCodeIncr += 1
                    Next
                End If
            End If

            Return Range

        Catch ex As Exception

            Throw New ProcedureRangeException("Cannot Create Procedure Range", ex)
        End Try

    End Function

#End Region

#Region "Constructors"
    Private Sub New()

    End Sub
#End Region
End Class