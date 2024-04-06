Option Explicit On
Option Strict On

Imports System
Imports System.Data
Imports System.Text
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Microsoft.Office.Interop
Imports System.Data.Common

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.PlanStagingDAL
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class handles the C.R.U.D. for all staged plans
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	1/25/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Friend NotInheritable Class PlanStagingDAL

#Region "Shared Properties"
    Shared _DB2databaseName As String = "FDBMD"
    Shared _SQLdatabaseName As String = "dbo"
    Shared _WorkBook As Excel.Workbook
    Shared _WorkSheet As Excel.Worksheet

#End Region

#Region "Batch Number"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' updates the batch number for the next publish
    ' </summary>
    ' <param name="db"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Shared Function GetNextBatchNumber() As Integer
        Dim DB As Database = CMSDALCommon.CreateDatabase("Rules Database Instance")
        Try
            Dim SQLCommand As String = CMSDALCommon.GetDatabaseName & "." & "INCREMENT_AND_RETRIEVE_BATCH_NBR"
            Dim DBCommandWrapper As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddOutParameter(dbCommandWrapper, "@NEXT_BATCH_NBR", DbType.Int32, 4)
            db.ExecuteNonQuery(dbCommandWrapper)
            db.CreateConnection.Open()
            Return CType(db.GetParameterValue(dbCommandWrapper, "@NEXT_BATCH_NBR"), Int32)
        Catch ex As Exception

            Throw New PublishException("Cannot Retrieve Next Batch Number")

        Finally
            If DB.CreateConnection IsNot Nothing Then
                DB.CreateConnection.Close()
            End If
        End Try
    End Function
#End Region

#Region "Adds"

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Adds data to the history table based off of a publish
    ' </summary>
    ' <param name="transaction"></param>
    ' <param name="batchNumber"></param>
    ' <param name="publishDate"></param>
    ' <param name="planType"></param>
    ' <param name="userID"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Sub AddPublishHistory(ByVal transaction As DbTransaction, ByVal batchNumber As Integer, ByVal publishDate As Date, ByVal planType As String, ByVal userID As String)
        Try
            Dim SQLCommand As String = CMSDALCommon.GetDatabaseName & "." & "CREATE_PUBLISH_HISTORY"
            Dim DB As Database = CMSDALCommon.CreateDatabase("Rules Database Instance")
            Dim DBCommandWrapper As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(dbCommandWrapper, "@BATCH_NBR", DbType.Int32, batchNumber)
            db.AddInParameter(dbCommandWrapper, "@PUBLISH_DATE", DbType.Time, publishDate)
            db.AddInParameter(dbCommandWrapper, "@PLAN_TYPE", DbType.String, planType)
            db.AddInParameter(dbCommandWrapper, "@CREATE_USERID", DbType.String, userID)

            db.ExecuteNonQuery(dbCommandWrapper, transaction)
        Catch ex As Exception

            Throw New PublishDataException("Cannot Publish History", ex)

        End Try
    End Sub
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Commits an Staged Plan
    ' </summary>
    ' <param name="stagedProcs"></param>
    ' <param name="publishdate"></param>
    ' <param name="planType"></param>
    ' <param name="userid"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Sub PublishStagedPlan(ByVal stagedProcs As Procedures, ByVal publishdate As Date, ByVal planType As String, ByVal userid As String)
        If stagedProcs Is Nothing Then
            Throw New ArgumentNullException("activeProcs")
            Return
        End If

        Dim DB As Database
        Dim DBConnection As DbConnection
        Dim Transaction As DbTransaction
        Dim BatchNumber As Integer

        Try
            DB = CMSDALCommon.CreateDatabase("Rules Database Instance")
            DBConnection = DB.CreateConnection()

            DBConnection.Open()
            Transaction = DBConnection.BeginTransaction()
            BatchNumber = GetNextBatchNumber()
            AddPublishHistory(Transaction, BatchNumber, publishdate, planType, userid)
            CommitProcedures(Transaction, stagedProcs, BatchNumber, userid)
            PlanStagingDAL.ClearStagedPlan(Transaction, planType)
            Transaction.Commit()
        Catch ex As Exception
            Try

                Transaction.Rollback()
            Catch ex2 As Exception

            End Try

            Throw New PublishDataException("Cannot Commit Plan As Active Plan", ex)

        Finally
            If DBConnection IsNot Nothing Then
                DBConnection.Close()
            End If
        End Try
    End Sub
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Commits active procedures
    ' </summary>
    ' <param name="transaction"></param>
    ' <param name="stagedProcs"></param>
    ' <param name="batchNumber"></param>
    ' <param name="userid"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Shared Sub CommitProcedures(ByVal transaction As DbTransaction, ByVal stagedProcs As Procedures, ByVal batchNumber As Integer, ByVal userid As String)
        Dim SQLCommand As String
        Dim DB As Database = CMSDALCommon.CreateDatabase("Rules Database Instance")
        Dim DBCommandWrapper As DbCommand
        Dim ProcID As Integer, ruleSetID As Integer, RuleID As Integer, TempruleSetID As Integer
        Dim RuleSetsPublished As New Hashtable

        RuleSetsPublished.Add("", "")

        Dim IDArray As New Collection
        Dim FileName As String

        FileName = "PublishLog_" & UFCWGeneral.NowDate.ToString & ".csv"
        FileName = FileName.Replace("/", "_")
        FileName = FileName.Replace(" ", "_")
        FileName = "C:\" & FileName.Replace(":", "_")

        Try
            For Each activeProc As ProcedureActive In stagedProcs
                SQLCommand = CMSDALCommon.GetDatabaseName & "." & "CREATE_PROCEDURE_ACTIVE"
                DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)
                DB.AddOutParameter(DBCommandWrapper, "@PROC_ID", DbType.Int32, 4)
                DB.AddInParameter(DBCommandWrapper, "@BILL_TYPE", DbType.String, activeProc.BillType)
                DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String, userid)
                DB.AddInParameter(DBCommandWrapper, "@DIAGNOSIS", DbType.String, activeProc.Diagnosis)
                DB.AddInParameter(DBCommandWrapper, "@PROVIDER", DbType.String, activeProc.Provider)
                DB.AddInParameter(DBCommandWrapper, "@MODIFIER", DbType.String, activeProc.Modifier)
                DB.AddInParameter(DBCommandWrapper, "@GENDER", DbType.String, activeProc.Gender)
                DB.AddInParameter(DBCommandWrapper, "@MONTHS_MIN", DbType.String, activeProc.MonthsMin)
                DB.AddInParameter(DBCommandWrapper, "@MONTHS_MAX", DbType.String, activeProc.MonthsMax)
                DB.AddInParameter(DBCommandWrapper, "@OCC_FROM_DATE", DbType.Date, activeProc.FromDate)
                DB.AddInParameter(DBCommandWrapper, "@OCC_TO_DATE", DbType.Date, activeProc.ToDate)
                DB.AddInParameter(DBCommandWrapper, "@PLACE_OF_SERV", DbType.String, activeProc.PlaceOfService)
                DB.AddInParameter(DBCommandWrapper, "@PLAN_TYPE", DbType.String, activeProc.PlanType)
                DB.AddInParameter(DBCommandWrapper, "@PROC_CODE", DbType.String, activeProc.ProcedureCode)
                DB.AddInParameter(DBCommandWrapper, "@PUBLISH_BATCH_NBR", DbType.Int32, batchNumber)
                DB.AddInParameter(DBCommandWrapper, "@SEQ_NBR", DbType.Int32, activeProc.SequenceNumber)
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
                ProcID = Convert.ToInt32(DB.GetParameterValue(DBCommandWrapper, "@PROC_ID"))

                PlanTraceLog.WriteToFile(FileName, "procedure published: " & ProcID & " under batch number: " & batchNumber)

                ruleSetID = -1

                For Each ActiveProcRuleSet As RuleSet In activeProc.RuleSets
                    If RuleSetsPublished(ActiveProcRuleSet.RuleSetName) IsNot Nothing Then
                        ruleSetID = CInt(RuleSetsPublished(ActiveProcRuleSet.RuleSetName))
                    End If

                    TempruleSetID = CommitRuleset(transaction, DirectCast(ActiveProcRuleSet, RuleSetActive), batchNumber, userid, RuleSetsPublished(ActiveProcRuleSet.RuleSetName) Is Nothing, ProcID, ruleSetID)
                    If TempruleSetID > 0 Then
                        ruleSetID = TempruleSetID
                    End If

                    If RuleSetsPublished(ActiveProcRuleSet.RuleSetName) Is Nothing Then
                        RuleSetsPublished.Add(ActiveProcRuleSet.RuleSetName, ruleSetID)

                        For Each RuleSetRule As Rule In ActiveProcRuleSet
                            RuleID = CommitRule(transaction, RuleSetRule, userid, ruleSetID)
                            For Each DE As DictionaryEntry In RuleSetRule.Conditions
                                CommitConditions(transaction, DirectCast(DE.Value, Condition), userid, RuleID)
                            Next
                        Next
                    End If
                Next
            Next
        Catch ex As Exception

            Throw New CommitDataException("Cannot Commit Procedures", ex)

        End Try
    End Sub
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Commits an active ruleset
    ' </summary>
    ' <param name="transaction"></param>
    ' <param name="activeRuleSet"></param>
    ' <param name="batchNumber"></param>
    ' <param name="userId"></param>
    ' <param name="isNew"></param>
    ' <param name="procedureId"></param>
    ' <param name="ruleSetID"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Shared Function CommitRuleset(ByVal transaction As DbTransaction, ByVal activeRuleSet As RuleSetActive, ByVal batchNumber As Integer, ByVal userId As String, ByVal isNew As Boolean, ByVal procedureId As Integer, ByVal ruleSetID As Integer) As Integer
        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand
        Dim DB As Database = CMSDALCommon.CreateDatabase("Rules Database Instance")
        Dim RulesPublished As New ArrayList
        Dim ReturnVal As Integer
        Dim FileName As String

        FileName = "PublishLog_" & UFCWGeneral.NowDate.ToString & ".csv"
        FileName = FileName.Replace("/", "_")
        FileName = FileName.Replace(" ", "_")
        FileName = "C:\" & FileName.Replace(":", "_")

        Try
            'loop through all procedure and find each ones ruleset
            If isNew Then
                SQLCommand = CMSDALCommon.GetDatabaseName & "." & "CREATE_RULE_SET_ACTIVE"
                DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)
                'db.ClearParameterCache()
                DB.AddInParameter(DBCommandWrapper, "@PROC_ID", DbType.Int32, procedureId)
                DB.AddOutParameter(DBCommandWrapper, "@RULE_SET_ID", DbType.Int32, 4)
                DB.AddInParameter(DBCommandWrapper, "@RULE_SET_TYPE", DbType.Int32, activeRuleSet.RulesetType)
                DB.AddInParameter(DBCommandWrapper, "@RULE_SET_NAME", DbType.String, activeRuleSet.RuleSetName)
                DB.AddInParameter(DBCommandWrapper, "@HIDDEN_SW", DbType.Int32, activeRuleSet.Hidden)
                DB.AddInParameter(DBCommandWrapper, "@PUBLISH_BATCH_NBR", DbType.Int32, batchNumber)
                DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String, userId)
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
                ReturnVal = Convert.ToInt32(DB.GetParameterValue(DBCommandWrapper, "@RULE_SET_ID"))
                Return ReturnVal
            Else
                SQLCommand = CMSDALCommon.GetDatabaseName & "." & "CREATE_PROC_TO_RULE_SET_ACTIVE"
                DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)
                'db.ClearParameterCache()
                DB.AddInParameter(DBCommandWrapper, "@PROC_ID", DbType.Int32, procedureId)
                DB.AddInParameter(DBCommandWrapper, "@RULE_SET_ID", DbType.Int32, ruleSetID)
                DB.AddInParameter(DBCommandWrapper, "@RULE_SET_TYPE", DbType.Int32, activeRuleSet.RulesetType)
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

            PlanTraceLog.WriteToFile(FileName, "ruleset published: name - " & activeRuleSet.RuleSetName & " type - " & activeRuleSet.RulesetType & " id - " & CStr(If(ReturnVal > 0, ReturnVal, activeRuleSet.RulesetID)))

        Catch ex As Exception

            Throw New CommitDataException("Cannot Commit Ruleset", ex)

        End Try

    End Function
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Commits active rules
    ' </summary>
    ' <param name="transaction"></param>
    ' <param name="rl"></param>
    ' <param name="userId"></param>
    ' <param name="ruleSetID"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    '     [paulw] 9/27/2006   Per ACR MED-0029, added MultiLineCoPay type support
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Shared Function CommitRule(ByVal transaction As DbTransaction, ByVal rl As Rule, ByVal userId As String, ByVal ruleSetID As Integer) As Integer
        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand
        Dim DB As Database = CMSDALCommon.CreateDatabase("Rules Database Instance")
        Dim ReturnVal As Integer
        Dim FileName As String

        FileName = "PublishLog_" & UFCWGeneral.NowDate.ToString & ".csv"
        FileName = FileName.Replace("/", "_")
        FileName = FileName.Replace(" ", "_")
        FileName = "C:\" & FileName.Replace(":", "_")

        Try
            SQLCommand = CMSDALCommon.GetDatabaseName & "." & "CREATE_RULE_ACTIVE"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)
            DB.AddInParameter(DBCommandWrapper, "@RULE_SET_ID", DbType.Int32, ruleSetID)
            DB.AddInParameter(DBCommandWrapper, "@RULE_NAME", DbType.String, rl.RuleName)

            If TypeOf (rl) Is CoPayRule Then
                DB.AddInParameter(DBCommandWrapper, "@RULE_TYPE", DbType.Int32, RuleTypes.CoPay)
            ElseIf TypeOf (rl) Is MultilineCoPayRule Then
                DB.AddInParameter(DBCommandWrapper, "@RULE_TYPE", DbType.Int32, RuleTypes.MultiLineCoPay)
            ElseIf TypeOf (rl) Is CoInsuranceRule Then
                DB.AddInParameter(DBCommandWrapper, "@RULE_TYPE", DbType.Int32, RuleTypes.CoInsurance)
            ElseIf TypeOf (rl) Is DeductibleRule Then
                DB.AddInParameter(DBCommandWrapper, "@RULE_TYPE", DbType.Int32, RuleTypes.Deductible)
            ElseIf TypeOf (rl) Is AccidentRule Then
                DB.AddInParameter(DBCommandWrapper, "@RULE_TYPE", DbType.Int32, RuleTypes.Accident)
            ElseIf TypeOf (rl) Is OutOfPocketRule Then
                DB.AddInParameter(DBCommandWrapper, "@RULE_TYPE", DbType.Int32, RuleTypes.OutOfPocket)
            ElseIf TypeOf (rl) Is StandardAccumulatorRule Then
                DB.AddInParameter(DBCommandWrapper, "@RULE_TYPE", DbType.Int32, RuleTypes.Standard)
            ElseIf TypeOf (rl) Is ProceduralAllowanceRule Then
                DB.AddInParameter(DBCommandWrapper, "@RULE_TYPE", DbType.Int32, RuleTypes.ProceduralAllowance)
            ElseIf TypeOf (rl) Is DenyRule Then
                DB.AddInParameter(DBCommandWrapper, "@RULE_TYPE", DbType.Int32, RuleTypes.Deny)
            ElseIf TypeOf (rl) Is ProviderWriteOffRule Then
                DB.AddInParameter(DBCommandWrapper, "@RULE_TYPE", DbType.Int32, RuleTypes.ProviderWriteOff)
            ElseIf TypeOf (rl) Is OriginalRule Then
                DB.AddInParameter(DBCommandWrapper, "@RULE_TYPE", DbType.Int32, RuleTypes.Original)
            End If

            DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String, userId)
            DB.AddOutParameter(DBCommandWrapper, "@RULE_ID", DbType.Int32, 4)
            DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            ReturnVal = Convert.ToInt32(DB.GetParameterValue(DBCommandWrapper, "@RULE_ID"))

            PlanTraceLog.WriteToFile(FileName, "rule_id - " & ReturnVal & " ruletype - " & rl.GetType.ToString)

            Return ReturnVal

        Catch ex As Exception

            Throw New CommitDataException("Cannot Commit Rules", ex)

        End Try

    End Function

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Pushes the conditions to the database
    ' </summary>
    ' <param name="transaction"></param>
    ' <param name="cnd"></param>
    ' <param name="userId"></param>
    ' <param name="ruleid"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	6/26/2006	Created
    '     [paulw] 9/19/2006   Added in paramter for condition that corresponds to ACR MED-0018
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Sub CommitConditions(ByVal transaction As DbTransaction, ByVal cnd As Condition, ByVal userId As String, ByVal ruleid As Integer)
        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand
        Dim DB As Database = CMSDALCommon.CreateDatabase("Rules Database Instance")
        Dim FileName As String

        FileName = "PublishLog_" & UFCWGeneral.NowDate.ToString & ".csv"
        FileName = FileName.Replace("/", "_")
        FileName = FileName.Replace(" ", "_")
        FileName = "C:\" & FileName.Replace(":", "_")

        Try
            SQLCommand = CMSDALCommon.GetDatabaseName & "." & "CREATE_RULE_ACCUM_COND_ACTIVE"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)
            DB.AddInParameter(DBCommandWrapper, "@RULE_ACCUM_COND_ID", DbType.Int32, cnd.ConditionID)
            DB.AddInParameter(DBCommandWrapper, "@ACCUM_NAME", DbType.String, cnd.AccumulatorName)
            DB.AddInParameter(DBCommandWrapper, "@OPERAND", DbType.Decimal, cnd.Operand)
            DB.AddInParameter(DBCommandWrapper, "@DIRECTION", DbType.Int32, cnd.Direction)
            DB.AddInParameter(DBCommandWrapper, "@DURATION", DbType.Int32, cnd.Duration)
            DB.AddInParameter(DBCommandWrapper, "@DURATION_TYPE", DbType.Int32, cnd.DurationType)
            DB.AddInParameter(DBCommandWrapper, "@RULE_ID", DbType.Int32, ruleid)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_FOR_HEADROOM", DbType.Decimal, cnd.UseInHeadroomCheck)
            DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String, userId)

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            PlanTraceLog.WriteToFile(FileName, "condition written: type: " & cnd.ToString())

        Catch ex As Exception

            Throw New CommitDataException("Cannot Commit Condition", ex)

        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Puts a procedure range into the ProcedureStaged table
    ' </summary>
    ' <param name="proceduresCollection"></param>
    ' <param name="stagingMasterID"></param>
    ' <param name="modifiedUserID"></param>
    ' <param name="status"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Sub AddProcedures(ByVal proceduresCollection As Procedures, ByVal stagingMasterID As Int32, ByVal modifiedUserID As String, ByVal status As Int32)
        Dim DB As Database
        Dim DBConnection As DbConnection
        Dim Transaction As DbTransaction
        Dim FileName As String
        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand

        FileName = "AddLog_" & UFCWGeneral.NowDate.ToString & ".csv"
        FileName = FileName.Replace("/", "_")
        FileName = FileName.Replace(" ", "_")
        FileName = "C:\" & FileName.Replace(":", "_")
        Try

            DB = CMSDALCommon.CreateDatabase("Rules Database Instance")
            SQLCommand = CMSDALCommon.GetDatabaseName & "." & "CREATE_PROCEDURE_RANGE"
            DBConnection = DB.CreateConnection
            DBConnection.Open()
            Transaction = DBConnection.BeginTransaction

            For I As Integer = 0 To proceduresCollection.Count - 1
                DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)
                DB.AddInParameter(DBCommandWrapper, "@PLAN_TYPE", DbType.String, proceduresCollection(I).PlanType)
                DB.AddInParameter(DBCommandWrapper, "@PROC_CODE", DbType.String, proceduresCollection(I).ProcedureCode)
                DB.AddInParameter(DBCommandWrapper, "@MODIFIER", DbType.String, proceduresCollection(I).Modifier)
                DB.AddInParameter(DBCommandWrapper, "@GENDER", DbType.String, proceduresCollection(I).Gender)
                DB.AddInParameter(DBCommandWrapper, "@MONTHS_MIN", DbType.String, proceduresCollection(I).MonthsMin)
                DB.AddInParameter(DBCommandWrapper, "@MONTHS_MAX", DbType.String, proceduresCollection(I).MonthsMax)
                DB.AddInParameter(DBCommandWrapper, "@PLACE_OF_SERV", DbType.String, proceduresCollection(I).PlaceOfService)
                DB.AddInParameter(DBCommandWrapper, "@DIAGNOSIS", DbType.String, proceduresCollection(I).Diagnosis)
                DB.AddInParameter(DBCommandWrapper, "@BILL_TYPE", DbType.String, proceduresCollection(I).BillType)
                DB.AddInParameter(DBCommandWrapper, "@PROVIDER", DbType.String, proceduresCollection(I).Provider)
                DB.AddInParameter(DBCommandWrapper, "@STATUS", DbType.Int32, status)
                DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String, modifiedUserID)
                DB.AddInParameter(DBCommandWrapper, "@STAGING_MASTER_ID", DbType.Int32, stagingMasterID)
                DB.ExecuteNonQuery(DBCommandWrapper)

                PlanTraceLog.WriteToFile(FileName, CType(proceduresCollection(I), ProcedureStaged).ToString)

            Next

            Transaction.Commit()

        Catch ex As Exception
            Transaction.Rollback()

            Throw New StagingDataException("Cannot Add Procedures to Staged Plan", ex)

        Finally
            If DBConnection IsNot Nothing Then
                DBConnection.Close()
            End If
        End Try
    End Sub
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
    Public Shared Sub AddNewRuleSet(ByVal proceduresCollection As Procedures, ByVal userId As String)
        Dim DB As Database
        Dim DBConnection As DbConnection
        Dim Transaction As DbTransaction
        Dim RuleSets As New ArrayList
        Dim procIds As New ArrayList
        Dim RlSetCol As New Rulesets
        Dim SaveProcIDs() As Integer
        Dim IsHere As Boolean = False
        'Get all the unique rulesets
        Dim Procedure As Procedure

        Try

            DB = CMSDALCommon.CreateDatabase("Rules Database Instance")
            DBConnection = DB.CreateConnection

            DBConnection.Open()
            Transaction = DBConnection.BeginTransaction

            For I As Integer = 0 To proceduresCollection.Count - 1
                Procedure = DirectCast(proceduresCollection(I), Procedure)
                For Each ProcedureRuleSet As RuleSet In Procedure.RuleSets
                    For Each RuleSet As RuleSet In RuleSets
                        If RuleSet.RuleSetName = ProcedureRuleSet.RuleSetName Then
                            IsHere = True
                        End If
                    Next
                    If Not IsHere Then
                        RuleSets.Add(ProcedureRuleSet)
                    End If
                    IsHere = False
                Next
            Next

            'go through each ruleset and find the procedures that have that ruleset
            For Each Ruleset As RuleSet In RuleSets
                procIds.Clear()
                For I As Integer = 0 To proceduresCollection.Count - 1
                    For Each ProceduresCollectionRuleset As RuleSet In proceduresCollection(I).Rulesets
                        If ProceduresCollectionRuleset.RuleSetName = Ruleset.RuleSetName Then
                            procIds.Add(proceduresCollection(I).ProcedureID)
                        End If
                    Next
                Next
                ReDim SaveProcIDs(procIds.Count - 1)
                For I As Integer = 0 To procIds.Count - 1
                    SaveProcIDs(I) = Convert.ToInt32(procIds(I))
                Next

                AddAndAssignRuleSet(SaveProcIDs, Ruleset, userId, Transaction)
            Next
            Transaction.Commit()
        Catch ex As Exception
            Transaction.Rollback()

            Throw New PlanException("There was a Problem Adding the Ruleset to this Plan", ex)

        Finally
            If DBConnection IsNot Nothing Then
                DBConnection.Close()
            End If
        End Try
    End Sub
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Adds and assigns a ruleset into the db
    ' </summary>
    ' <param name="proceduresCollection"></param>
    ' <param name="stagingMasterID"></param>
    ' <param name="modifiedUserID"></param>
    ' <param name="status"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	6/14/2006	Created
    '     [paulw] 9/19/2006   Added in paramter for condition that corresponds to ACR MED-0018
    '     [paulw] 9/27/2006   Per ACR MED-0029, added MultiLineCoPay type support
    '     [paulw]	10/3/2006	Per ACR MED-0023, added support for deny type
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Shared Sub AddAndAssignRuleSet(ByVal procedureIds() As Integer, ByVal rlSet As RuleSet, ByVal modifiedUserID As String, ByVal transaction As DbTransaction)
        Try
            Dim DB As Database = CMSDALCommon.CreateDatabase("Rules Database Instance")
            Dim DBCommandWrapper As DbCommand

            DBCommandWrapper = DB.GetStoredProcCommand(CMSDALCommon.GetDatabaseName & "." & "CREATE_RULE_SET_STAGED")
            DB.AddInParameter(DBCommandWrapper, "@RULE_SET_NAME", DbType.String, rlSet.RuleSetName)
            DB.AddInParameter(DBCommandWrapper, "@STATUS", DbType.Int32, DirectCast(rlSet, RuleSetStaged).Status)
            DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String, modifiedUserID)
            DB.AddInParameter(DBCommandWrapper, "@RULE_SET_TYPE", DbType.Int32, rlSet.RulesetType)
            DB.AddOutParameter(DBCommandWrapper, "@RULE_SET_ID", DbType.Int32, 4)
            DBCommandWrapper.CommandTimeout = 1200
            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

            rlSet.RulesetID = CType(DB.GetParameterValue(DBCommandWrapper, "@RULE_SET_ID"), Int32)
            Dim procBuilder As New StringBuilder

            For i As Integer = 0 To procedureIds.Length - 1
                procBuilder.Append(procedureIds(i).ToString())
                procBuilder.Append(",")
                If procBuilder.ToString.Length > 7990 Then
                    DBCommandWrapper = DB.GetStoredProcCommand(CMSDALCommon.GetDatabaseName & "." & "UPDATE_PROC_TO_RULE_SET_STAGING")
                    DB.AddInParameter(DBCommandWrapper, "@PROC_ID", DbType.AnsiString, procBuilder.ToString.Substring(0, procBuilder.ToString.Length - 1))
                    DB.AddInParameter(DBCommandWrapper, "@RULE_SET_ID", DbType.Int32, rlSet.RulesetID)
                    DB.AddInParameter(DBCommandWrapper, "@RULE_SET_TYPE", DbType.Int32, rlSet.RulesetType)
                    DBCommandWrapper.CommandTimeout = 120
                    DB.ExecuteNonQuery(DBCommandWrapper, transaction)
                    procBuilder = New StringBuilder
                End If

            Next
            DBCommandWrapper = DB.GetStoredProcCommand(CMSDALCommon.GetDatabaseName & "." & "UPDATE_PROC_TO_RULE_SET_STAGING")
            Dim prcString As String = procBuilder.ToString.Substring(0, procBuilder.ToString.Length - 1)
            If prcString.EndsWith(",0") Then
                prcString = prcString.Substring(0, prcString.Length - 2)
            End If
            DB.AddInParameter(DBCommandWrapper, "@PROC_ID", DbType.AnsiString, prcString)
            DB.AddInParameter(DBCommandWrapper, "@RULE_SET_ID", DbType.Int32, rlSet.RulesetID)
            DB.AddInParameter(DBCommandWrapper, "@RULE_SET_TYPE", DbType.Int32, rlSet.RulesetType)
            DBCommandWrapper.CommandTimeout = 1200
            DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Dim RuleID As Integer

            For Each Rule As Rule In rlSet
                DBCommandWrapper = DB.GetStoredProcCommand(CMSDALCommon.GetDatabaseName & "." & "CREATE_RULE_STAGED")
                DB.AddInParameter(DBCommandWrapper, "@RULE_SET_ID", DbType.Int32, rlSet.RulesetID)
                DB.AddInParameter(DBCommandWrapper, "@RULE_NAME", DbType.String, "")
                If TypeOf (Rule) Is AccidentRule Then
                    DB.AddInParameter(DBCommandWrapper, "@RULE_TYPE", DbType.Int32, CType(RuleTypes.Accident, Int32))
                ElseIf TypeOf (Rule) Is CoInsuranceRule Then
                    DB.AddInParameter(DBCommandWrapper, "@RULE_TYPE", DbType.Int32, CType(RuleTypes.CoInsurance, Int32))
                ElseIf TypeOf (Rule) Is CoPayRule Then
                    DB.AddInParameter(DBCommandWrapper, "@RULE_TYPE", DbType.Int32, CType(RuleTypes.CoPay, Int32))
                ElseIf TypeOf (Rule) Is DeductibleRule Then
                    DB.AddInParameter(DBCommandWrapper, "@RULE_TYPE", DbType.Int32, CType(RuleTypes.Deductible, Int32))
                ElseIf TypeOf (Rule) Is OutOfPocketRule Then
                    DB.AddInParameter(DBCommandWrapper, "@RULE_TYPE", DbType.Int32, CType(RuleTypes.OutOfPocket, Int32))
                ElseIf TypeOf (Rule) Is StandardAccumulatorRule Then
                    DB.AddInParameter(DBCommandWrapper, "@RULE_TYPE", DbType.Int32, CType(RuleTypes.Standard, Int32))
                ElseIf TypeOf (Rule) Is ProceduralAllowanceRule Then
                    DB.AddInParameter(DBCommandWrapper, "@RULE_TYPE", DbType.Int32, CType(RuleTypes.ProceduralAllowance, Int32))
                ElseIf TypeOf (Rule) Is MultilineCoPayRule Then
                    DB.AddInParameter(DBCommandWrapper, "@RULE_TYPE", DbType.Int32, CType(RuleTypes.MultiLineCoPay, Int32))
                ElseIf TypeOf (Rule) Is DenyRule Then
                    DB.AddInParameter(DBCommandWrapper, "@RULE_TYPE", DbType.Int32, CType(RuleTypes.Deny, Int32))
                ElseIf TypeOf (Rule) Is ProviderWriteOffRule Then
                    DB.AddInParameter(DBCommandWrapper, "@RULE_TYPE", DbType.Int32, CType(RuleTypes.ProviderWriteOff, Int32))
                ElseIf TypeOf (Rule) Is OriginalRule Then
                    DB.AddInParameter(DBCommandWrapper, "@RULE_TYPE", DbType.Int32, CType(RuleTypes.Original, Int32))
                End If

                DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String, modifiedUserID)
                DB.AddOutParameter(DBCommandWrapper, "@RULE_ID", DbType.Int32, 4)
                DBCommandWrapper.CommandTimeout = 1200
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)

                RuleID = CType(DB.GetParameterValue(DBCommandWrapper, "@RULE_ID"), Int32)
                For Each DE As DictionaryEntry In Rule.Conditions
                    DBCommandWrapper = DB.GetStoredProcCommand(CMSDALCommon.GetDatabaseName & "." & "CREATE_RULE_ACCUM_COND_STAGING")

                    If DirectCast(DE.Value, Condition).AccumulatorName Is Nothing Then DirectCast(DE.Value, Condition).AccumulatorName = ""

                    DB.AddInParameter(DBCommandWrapper, "@ACCUM_NAME", DbType.String, DirectCast(DE.Value, Condition).AccumulatorName)
                    DB.AddInParameter(DBCommandWrapper, "@OPERAND", DbType.Decimal, DirectCast(DE.Value, Condition).Operand)
                    DB.AddInParameter(DBCommandWrapper, "@DIRECTION", DbType.Decimal, DirectCast(DE.Value, Condition).Direction)
                    DB.AddInParameter(DBCommandWrapper, "@DURATION", DbType.Int32, DirectCast(DE.Value, Condition).Duration)
                    DB.AddInParameter(DBCommandWrapper, "@DURATION_TYPE", DbType.Int32, CType(DirectCast(DE.Value, Condition).DurationType, Int32))
                    DB.AddInParameter(DBCommandWrapper, "@RULE_ID", DbType.Int32, RuleID)
                    DB.AddInParameter(DBCommandWrapper, "@CHECK_FOR_HEADROOM", DbType.Decimal, DirectCast(DE.Value, Condition).UseInHeadroomCheck)
                    DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String, modifiedUserID)
                    DB.ExecuteNonQuery(DBCommandWrapper, transaction)
                Next
            Next
            DBCommandWrapper = DB.GetStoredProcCommand(CMSDALCommon.GetDatabaseName & "." & "DELETE_ORPHAN_RULE_SETS")
            DBCommandWrapper.CommandTimeout = 1200
            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

        Catch ex As Exception

            Throw New StagingDataException("Cannot Add Ruleset to Staged Plan", ex)

        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Updates the status of a group of procedures
    ' </summary>
    ' <param name="procedureIds">A comma delimited string of procedure ids</param>
    ' <param name="status"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	6/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Sub UpdateProcedureStatus(ByVal procedureIds As String, ByVal status As Integer)
        Dim DB As Database = CMSDALCommon.CreateDatabase("Rules Database Instance")
        Try

            Dim DBCommandWrapper As DbCommand

            DBCommandWrapper = DB.GetStoredProcCommand(CMSDALCommon.GetDatabaseName & "." & "UPDATE_PROCEDURE_STAGED_STATUS")
            DB.AddInParameter(DBCommandWrapper, "@PROC_IDS", DbType.AnsiString, procedureIds)
            DB.AddInParameter(DBCommandWrapper, "@STATUS", DbType.Int32, status)
            DBCommandWrapper.CommandTimeout = 1200
            DB.CreateConnection.Open()
            DB.ExecuteNonQuery(DBCommandWrapper)
        Catch ex As Exception

            Throw New StagingDataException("Cannot Update Procedure Status", ex)

        Finally
            If Not (DB.CreateConnection Is Nothing) Then
                DB.CreateConnection.Close()
            End If
        End Try

    End Sub
#End Region

#Region "Gets"
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
    Public Shared Function DoesRulesetExist(ByVal ruleSetName As String, ByVal planType As String) As Boolean
        Dim DB As Database
        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand
        Dim MatchFound As Boolean = False
        Dim Cnt As Integer

        Try
            DB = CMSDALCommon.CreateDatabase("Rules Database Instance")
            SQLCommand = CMSDALCommon.GetDatabaseName & "." & "RETRIEVE_NUMBER_OF_MATCHING_RULE_SETS"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)
            DB.AddInParameter(DBCommandWrapper, "@PLAN_TYPE", DbType.String, planType)
            DB.AddInParameter(DBCommandWrapper, "@RULE_SET_NAME", DbType.String, ruleSetName)
            DBCommandWrapper.CommandTimeout = 1200
            DB.CreateConnection.Open()

            Cnt = Convert.ToInt32(DB.ExecuteScalar(DBCommandWrapper))

            Return (Cnt > 0)

        Catch ex As Exception

            Throw New StagingDataException("Cannot Determine if ruleset exists", ex)

        Finally
            If (DB.CreateConnection IsNot Nothing) Then
                DB.CreateConnection.Close()
            End If
        End Try
    End Function
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets the data, for the given plantype, of the staging master
    ' </summary>
    ' <param name="planType"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function GetStagingMaster(ByVal planType As String, ByVal getAsLightweight As Boolean) As PlanStaged
        Dim DB As Database = CMSDALCommon.CreateDatabase("Rules Database Instance")
        Try
            Dim planStg As PlanStaged
            Dim SQLCommand As String = CMSDALCommon.GetDatabaseName & "." & "RETRIEVE_STAGING_MASTER"
            Dim DBCommandWrapper As DbCommand = DB.GetStoredProcCommand(SQLCommand)
            DB.AddInParameter(DBCommandWrapper, "@PLAN_TYPE", DbType.String, planType)
            DBCommandWrapper.CommandTimeout = 1200
            DB.CreateConnection.Open()
            Dim dataReader As IDataReader = DB.ExecuteReader(DBCommandWrapper)

            If (dataReader.Read()) Then
                planStg = New PlanStaged(planType, CStr(dataReader("PLAN_DESCRIPTION")), CInt(dataReader("STATUS")), CInt(dataReader("STAGING_MASTER_ID")), getAsLightweight)
            End If
            If Not dataReader.IsClosed Then
                dataReader.Close()
            End If

            Return planStg

        Catch ex As Exception

            Throw New StagingDataException("Cannot Get Staging Master", ex)

        Finally
            If Not (DB.CreateConnection Is Nothing) Then
                DB.CreateConnection.Close()
            End If
        End Try
    End Function

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets all Staged plans
    ' </summary>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function GetStagedPlans(ByVal getAsLightweight As Boolean) As ArrayList
        Dim DB As Database = CMSDALCommon.CreateDatabase("Rules Database Instance")
        Try
            Dim SQLCommand As String = CMSDALCommon.GetDatabaseName & "." & "RETRIEVE_STAGED_PLANS"
            Dim DBCommandWrapper As DbCommand = DB.GetStoredProcCommand(SQLCommand)
            DBCommandWrapper.CommandTimeout = 1200
            DB.CreateConnection.Open()
            Dim dataReader As IDataReader = DB.ExecuteReader(DBCommandWrapper)

            Dim plan As PlanStaged
            Dim plans As New ArrayList
            While (dataReader.Read())
                plan = New PlanStaged(CStr(dataReader("PLAN_TYPE")), CStr(dataReader("PLAN_DESCRIPTION")), CInt(dataReader("STATUS")), CInt(dataReader("STAGING_MASTER_ID")), getAsLightweight)
                plans.Add(plan)
            End While

            If Not dataReader.IsClosed Then
                dataReader.Close()
            End If

            Return plans
        Catch ex As Exception

            Throw New StagingDataException("Cannot Get Staged Plans", ex)

        Finally
            If (DB.CreateConnection IsNot Nothing) Then
                DB.CreateConnection.Close()
            End If
        End Try
    End Function

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets conditions for the given ruleid
    ' </summary>
    ' <param name="ruleId"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	6/6/2006	Created
    '     [paulw] 9/19/2006   Added in paramter for condition that corresponds to ACR MED-0018
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Shared Function GetConditions(ByVal ruleId As Integer) As Conditions
        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand
        Dim DB As Database
        Dim DBDataReader As DbDataReader
        Dim FirstLoop As Boolean = True
        Dim RuleSetActive As New RuleSetActive
        Dim Condition As Condition
        Dim Conditions As New Conditions

        Try
            DB = CMSDALCommon.CreateDatabase("Rules Database Instance")

            SQLCommand = CMSDALCommon.GetDatabaseName & "." & "RETRIEVE_CONDITION_STAGING"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)
            DBCommandWrapper.CommandTimeout = 1200
            DB.CreateConnection.Open()
            DB.AddInParameter(DBCommandWrapper, "@RULE_ID", DbType.Int32, ruleId)

            DBDataReader = CType(DB.ExecuteReader(DBCommandWrapper), DbDataReader)

            While (DBDataReader.Read)
                Condition = New Condition
                If DBDataReader("ACCUM_NAME") IsNot DBNull.Value Then
                    Condition.AccumulatorName = DBDataReader("ACCUM_NAME").ToString
                End If
                If DBDataReader("DIRECTION") IsNot DBNull.Value Then
                    Condition.Direction = CType(Convert.ToInt32(DBDataReader("DIRECTION").ToString), DateDirection)
                End If
                If DBDataReader("DURATION") IsNot DBNull.Value Then
                    Condition.Duration = Convert.ToInt32(DBDataReader("DURATION"))
                End If
                If DBDataReader("DURATION_TYPE") IsNot DBNull.Value Then
                    Condition.DurationType = CType(Convert.ToInt32(DBDataReader("DURATION_TYPE")), DateType)
                End If
                If DBDataReader("OPERAND") IsNot DBNull.Value Then
                    Condition.Operand = CDec(DBDataReader("OPERAND"))
                End If
                If DBDataReader("CHECK_FOR_HEADROOM") IsNot DBNull.Value Then
                    Condition.UseInHeadroomCheck = Convert.ToBoolean(DBDataReader("CHECK_FOR_HEADROOM"))
                End If
                Conditions.Add(Condition)
            End While

            Return Conditions

        Catch ex As Exception

            Throw New ActiveDataException("Cannot Get Conditions", ex)

        Finally
            If DBDataReader IsNot Nothing AndAlso Not DBDataReader.IsClosed Then
                DBDataReader.Close()
            End If
            If (DB.CreateConnection IsNot Nothing) Then
                DB.CreateConnection.Close()
            End If
        End Try
    End Function
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets all staged procedures for the given plan
    ' </summary>
    ' <param name="stagedPlanType"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function GetStagedProcedures(ByVal stagedPlanType As String, ByVal getAsLightweight As Boolean) As Procedures
        Dim DB As Database
        Dim Procedures As Procedures
        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand

        Dim PlanType As String

        Try
            DB = CMSDALCommon.CreateDatabase("Rules Database Instance")
            SQLCommand = CMSDALCommon.GetDatabaseName & "." & CStr(If(getAsLightweight, "RETRIEVE_PROCEDURES_STAGED_LIGHT", "RETRIEVE_PROCEDURES_STAGED"))
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)

            PlanType = stagedPlanType
            DB.AddInParameter(DBCommandWrapper, "@PLAN_TYPE", DbType.String, PlanType)
            DBCommandWrapper.CommandTimeout = 1200
            DB.CreateConnection.Open()
            Procedures = GetProcedureCollectionFromDataReader(DB.ExecuteReader(DBCommandWrapper))

            Return Procedures
        Catch ex As Exception

            Throw New StagingDataException("Cannot Get Staged Procedures", ex)

        Finally
            If (DB.CreateConnection IsNot Nothing) Then
                DB.CreateConnection.Close()
            End If
        End Try
    End Function

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets staged rulesets
    ' </summary>
    ' <param name="stagedPlanType"></param>
    ' <param name="getAsLightWeight"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	9/15/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function GetStagedRuleSets(ByVal stagedPlanType As String, ByVal getAsLightWeight As Boolean) As RuleSet()
        Dim DB As Database = CMSDALCommon.CreateDatabase("Rules Database Instance")
        Try
            Dim RuleSetArray As RuleSet()
            Dim SQLCommand As String = CMSDALCommon.GetDatabaseName & "." & CStr(If(getAsLightWeight, "RETRIEVE_RULE_SETS_STAGED_LIGHT", "RETRIEVE_RULE_SETS_STAGED"))
            Dim DBCommandWrapper As DbCommand = DB.GetStoredProcCommand(SQLCommand)

            Dim planType As String = stagedPlanType
            DB.AddInParameter(DBCommandWrapper, "@PLAN_TYPE", DbType.String, planType)
            DBCommandWrapper.CommandTimeout = 1200
            DB.CreateConnection.Open()
            RuleSetArray = GetRulesetsFromDataReader(DB.ExecuteReader(DBCommandWrapper))

            Return RuleSetArray

        Catch ex As Exception

            Throw New StagingDataException("Cannot Get Staged Rulesets", ex)

        Finally
            If Not (DB.CreateConnection Is Nothing) Then
                DB.CreateConnection.Close()
            End If
        End Try
    End Function
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets Staged Procedures based off the dynamic parameters passed in via an Xml doc
    ' </summary>
    ' <param name="xmlParameterList"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/10/2006	Created
    '     [paulw] 9/12/2006   removed 2nd part of query (which resulted in removal of ability to
    '                         search by ruleset name) per ACR MED-00??
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function GetProcedures(ByVal planType As String, ByVal filterParameters As String) As Procedures
        'ByVal planType As String,
        Dim DB As Database = CMSDALCommon.CreateDatabase("Rules Database Instance")
        Try
            Dim procCollection As Procedures
            Dim SQLCommand As New StringBuilder
            Dim SQLString As String = ""
            SQLCommand.Append("SELECT ")
            SQLCommand.Append(CMSDALCommon.GetDatabaseName & "." & "PROCEDURE_STAGING.PROC_ID, ")
            SQLCommand.Append(CMSDALCommon.GetDatabaseName & "." & "PROCEDURE_STAGING.PROC_CODE, ")
            SQLCommand.Append(CMSDALCommon.GetDatabaseName & "." & "PROCEDURE_STAGING.PLAN_TYPE, ")
            SQLCommand.Append(CMSDALCommon.GetDatabaseName & "." & "PROCEDURE_STAGING.PROVIDER, ")
            SQLCommand.Append(CMSDALCommon.GetDatabaseName & "." & "PROCEDURE_STAGING.MODIFIER, ")
            SQLCommand.Append(CMSDALCommon.GetDatabaseName & "." & "PROCEDURE_STAGING.GENDER, ")
            SQLCommand.Append(CMSDALCommon.GetDatabaseName & "." & "PROCEDURE_STAGING.MONTHS_MIN, ")
            SQLCommand.Append(CMSDALCommon.GetDatabaseName & "." & "PROCEDURE_STAGING.MONTHS_MAX, ")
            SQLCommand.Append(CMSDALCommon.GetDatabaseName & "." & "PROCEDURE_STAGING.PLACE_OF_SERV, ")
            SQLCommand.Append(CMSDALCommon.GetDatabaseName & "." & "PROCEDURE_STAGING.BILL_TYPE, ")
            SQLCommand.Append(CMSDALCommon.GetDatabaseName & "." & "PROCEDURE_STAGING.DIAGNOSIS, ")
            SQLCommand.Append(CMSDALCommon.GetDatabaseName & "." & "PROCEDURE_STAGING.STATUS AS PROCEDURE_STAGING_STATUS, ")
            SQLCommand.Append(CMSDALCommon.GetDatabaseName & "." & "PROCEDURE_STAGING.STAGING_MASTER_ID ")
            SQLCommand.Append("FROM ")
            SQLCommand.Append(CMSDALCommon.GetDatabaseName & "." & "PROCEDURE_STAGING ")
            SQLCommand.Append("WHERE " & CMSDALCommon.GetDatabaseName & "." & "PROCEDURE_STAGING.PLAN_TYPE = '" & planType & "' AND ")
            SQLCommand.Append(filterParameters)

            If filterParameters.Length < 1 Then
                SQLString = SQLCommand.ToString().Substring(0, SQLCommand.ToString().Length - 4)
            Else
                SQLString = SQLCommand.ToString()
            End If
            SQLString += " ORDER BY " & CMSDALCommon.GetDatabaseName & "." & "PROCEDURE_STAGING.PROC_ID;"
            SQLCommand = New StringBuilder
            SQLCommand.Append(SQLString)

            SQLString = SQLCommand.ToString
            Dim Cmd As DbCommand = DB.GetSqlStringCommand(SQLString)
            Cmd.CommandTimeout = 1200
            DB.CreateConnection.Open()
            procCollection = GetProcedureCollectionFromDataReader(DB.ExecuteReader(Cmd))

            Return procCollection

        Catch ex As Exception

            Throw New StagingDataException("Cannot Get Staged Data Because There is a Problem With The Query", ex)

        Finally
            If (DB.CreateConnection IsNot Nothing) Then
                DB.CreateConnection.Close()
            End If
        End Try
    End Function
#End Region

#Region "Conversion"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets a staged Rulset
    ' </summary>
    ' <param name="stagedProcedureDataReader"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	7/10/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Shared Function GetRuleSetStaged(ByVal stagedProcedureDataReader As IDataReader, ByVal procStaged As Procedure) As RuleSetStaged
        Dim RlSetStaged As RuleSetStaged
        Dim RlSetIsSet As Boolean = False
        For Each rlset As RuleSet In procStaged.RuleSets
            If rlset.RulesetType = Convert.ToInt32(stagedProcedureDataReader("RULE_SET_TYPE")) Then
                RlSetStaged = CType(rlset, RuleSetStaged)
                RlSetIsSet = True
            End If
        Next
        If Not RlSetIsSet Then
            RlSetStaged = New RuleSetStaged
            'rlSetActive.Hidden = CType(stagedProcedureDataReader("HIDDEN_SW"), Boolean)
            RlSetStaged.RulesetType = Convert.ToInt32(stagedProcedureDataReader("RULE_SET_TYPE"))
            RlSetStaged.RuleSetName = stagedProcedureDataReader("RULE_SET_NAME").ToString
            RlSetStaged.RulesetID = CInt(stagedProcedureDataReader("RULE_SET_ID"))
            RlSetStaged.Status = RulesSetStatus.NotPublished
        End If
        Return RlSetStaged
    End Function
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets a staged ruleset from the datareader
    ' </summary>
    ' <param name="stagedProcedureDataReader"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	7/10/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Shared Function GetRuleSetStaged(ByVal stagedProcedureDataReader As IDataReader) As RuleSetStaged
        Dim RuleSetStaged As RuleSetStaged

        RuleSetStaged = New RuleSetStaged
        'rlSetActive.Hidden = CType(stagedProcedureDataReader("HIDDEN_SW"), Boolean)
        If stagedProcedureDataReader.FieldCount > 2 Then
            RuleSetStaged.RulesetType = Convert.ToInt32(stagedProcedureDataReader("RULE_SET_TYPE"))
        End If
        RuleSetStaged.RuleSetName = stagedProcedureDataReader("RULE_SET_NAME").ToString
        RuleSetStaged.RulesetID = CInt(stagedProcedureDataReader("RULE_SET_ID"))
        RuleSetStaged.Status = RulesSetStatus.NotPublished
        Return RuleSetStaged
    End Function

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets a rule
    ' </summary>
    ' <param name="stagedProcedureDataReader"></param>
    ' <param name="rlSetStaged"></param>
    ' <param name="rlSetActive"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	7/10/2006	Created
    '     [paulw] 9/27/2006   Per ACR MED-0029, added MultiLineCoPay type support
    '     [paulw]	10/3/2006	Per ACR MED-0023, added support for deny type
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Shared Function GetRule(ByVal stagedProcedureDataReader As IDataReader, ByRef rlSetStaged As RuleSetStaged, ByRef rlSetActive As RuleSetActive) As Rule
        Dim ReturnRule As Rule

        For Each Rule As Rule In rlSetStaged
            If TypeOf (Rule) Is AccidentRule Then
                If CType(Convert.ToInt32(stagedProcedureDataReader("RULE_TYPE")), RuleTypes) = RuleTypes.Accident Then
                    ReturnRule = Rule
                    Exit For
                End If
            ElseIf TypeOf (Rule) Is CoInsuranceRule Then
                If CType(Convert.ToInt32(stagedProcedureDataReader("RULE_TYPE")), RuleTypes) = RuleTypes.CoInsurance Then
                    ReturnRule = Rule
                    Exit For
                End If
            ElseIf TypeOf (Rule) Is CoPayRule Then
                If CType(Convert.ToInt32(stagedProcedureDataReader("RULE_TYPE")), RuleTypes) = RuleTypes.CoPay Then
                    ReturnRule = Rule
                    Exit For
                End If
            ElseIf TypeOf (Rule) Is MultilineCoPayRule Then
                If CType(Convert.ToInt32(stagedProcedureDataReader("RULE_TYPE")), RuleTypes) = RuleTypes.MultiLineCoPay Then
                    ReturnRule = Rule
                    Exit For
                End If
            ElseIf TypeOf (Rule) Is DeductibleRule Then
                If CType(Convert.ToInt32(stagedProcedureDataReader("RULE_TYPE")), RuleTypes) = RuleTypes.Deductible Then
                    ReturnRule = Rule
                    Exit For
                End If
            ElseIf TypeOf (Rule) Is OutOfPocketRule Then
                If CType(Convert.ToInt32(stagedProcedureDataReader("RULE_TYPE")), RuleTypes) = RuleTypes.OutOfPocket Then
                    ReturnRule = Rule
                    Exit For
                End If
            ElseIf TypeOf (Rule) Is StandardAccumulatorRule Then
                If CType(Convert.ToInt32(stagedProcedureDataReader("RULE_TYPE")), RuleTypes) = RuleTypes.Standard Then
                    ReturnRule = Rule
                    Exit For
                End If
            ElseIf TypeOf (Rule) Is ProceduralAllowanceRule Then
                If CType(Convert.ToInt32(stagedProcedureDataReader("RULE_TYPE")), RuleTypes) = RuleTypes.ProceduralAllowance Then
                    ReturnRule = Rule
                    Exit For
                End If
            ElseIf TypeOf (Rule) Is DenyRule Then
                If CType(Convert.ToInt32(stagedProcedureDataReader("RULE_TYPE")), RuleTypes) = RuleTypes.Deny Then
                    ReturnRule = Rule
                    Exit For
                End If
            ElseIf TypeOf (Rule) Is ProviderWriteOffRule Then
                If CType(Convert.ToInt32(stagedProcedureDataReader("RULE_TYPE")), RuleTypes) = RuleTypes.ProviderWriteOff Then
                    ReturnRule = Rule
                    Exit For
                End If
            ElseIf TypeOf (Rule) Is OriginalRule Then
                If CType(Convert.ToInt32(stagedProcedureDataReader("RULE_TYPE")), RuleTypes) = RuleTypes.Original Then
                    ReturnRule = Rule
                    Exit For
                End If
            End If
        Next

        If ReturnRule Is Nothing Then
            Select Case CType(Convert.ToInt32(stagedProcedureDataReader("RULE_TYPE")), RuleTypes)
                Case RuleTypes.CoInsurance
                    ReturnRule = New CoInsuranceRule(New Conditions)
                Case RuleTypes.CoPay
                    ReturnRule = New CoPayRule(New Conditions)
                Case RuleTypes.MultiLineCoPay
                    ReturnRule = New MultilineCoPayRule(New Conditions)
                Case RuleTypes.Deductible
                    ReturnRule = New DeductibleRule(New Conditions)
                Case RuleTypes.OutOfPocket
                    ReturnRule = New OutOfPocketRule(New Conditions)
                Case RuleTypes.Standard
                    ReturnRule = New StandardAccumulatorRule(New Conditions)
                Case RuleTypes.Accident
                    ReturnRule = New AccidentRule(New Conditions)
                Case RuleTypes.ProceduralAllowance
                    ReturnRule = New ProceduralAllowanceRule(New Conditions)
                Case RuleTypes.Deny
                    ReturnRule = New DenyRule(New Conditions)
                Case RuleTypes.ProviderWriteOff
                    ReturnRule = New ProviderWriteOffRule(New Conditions)
                Case RuleTypes.Original
                    ReturnRule = New OriginalRule(New Conditions)
            End Select
            rlSetStaged.Add(ReturnRule)
            rlSetActive.Add(ReturnRule)
        End If
        Return ReturnRule
    End Function

    'Private Shared Function GetRule(ByVal stagedProcedureDataReader As IDataReader, ByRef rlSetStaged As RulesetStaged) As Rule
    '    Dim Rl As Rule
    '    For Each rul As Rule In rlSetStaged
    '        If TypeOf (rul) Is AccidentRule Then
    '            If CType(Convert.ToInt32(stagedProcedureDataReader("RULE_TYPE")), Rule.RuleTypes) = Rule.RuleTypes.Accident Then
    '                rl = rul
    '                Exit For
    '            End If
    '        ElseIf TypeOf (rul) Is CoInsuranceRule Then
    '            If CType(Convert.ToInt32(stagedProcedureDataReader("RULE_TYPE")), Rule.RuleTypes) = Rule.RuleTypes.CoInsurance Then
    '                rl = rul
    '                Exit For
    '            End If
    '        ElseIf TypeOf (rul) Is CoPayRule Then
    '            If CType(Convert.ToInt32(stagedProcedureDataReader("RULE_TYPE")), Rule.RuleTypes) = Rule.RuleTypes.CoPay Then
    '                rl = rul
    '                Exit For
    '            End If
    '        ElseIf TypeOf (rul) Is DeductibleRule Then
    '            If CType(Convert.ToInt32(stagedProcedureDataReader("RULE_TYPE")), Rule.RuleTypes) = Rule.RuleTypes.Deductible Then
    '                rl = rul
    '                Exit For
    '            End If
    '        ElseIf TypeOf (rul) Is OutOfPocketRule Then
    '            If CType(Convert.ToInt32(stagedProcedureDataReader("RULE_TYPE")), Rule.RuleTypes) = Rule.RuleTypes.OutOfPocket Then
    '                rl = rul
    '                Exit For
    '            End If
    '        ElseIf TypeOf (rul) Is StandardAccumulatorRule Then
    '            If CType(Convert.ToInt32(stagedProcedureDataReader("RULE_TYPE")), Rule.RuleTypes) = Rule.RuleTypes.Standard Then
    '                rl = rul
    '                Exit For
    '            End If
    '        ElseIf TypeOf (rul) Is ProceduralAllowanceRule Then
    '            If CType(Convert.ToInt32(stagedProcedureDataReader("RULE_TYPE")), Rule.RuleTypes) = Rule.RuleTypes.ProceduralAllowance Then
    '                rl = rul
    '                Exit For
    '            End If
    '        End If
    '    Next

    '    If rl Is Nothing Then
    '        Select Case CType(Convert.ToInt32(stagedProcedureDataReader("RULE_TYPE")), Rule.RuleTypes)
    '            Case Rule.RuleTypes.CoInsurance
    '                rl = New CoInsuranceRule(New Conditions)
    '            Case Rule.RuleTypes.CoPay
    '                rl = New CoPayRule(New Conditions)
    '            Case Rule.RuleTypes.Deductible
    '                rl = New DeductibleRule(New Conditions)
    '            Case Rule.RuleTypes.OutOfPocket
    '                rl = New OutOfPocketRule(New Conditions)
    '            Case Rule.RuleTypes.Standard
    '                rl = New StandardAccumulatorRule(New Conditions)
    '            Case Rule.RuleTypes.Accident
    '                rl = New AccidentRule(New Conditions)
    '            Case Rule.RuleTypes.ProceduralAllowance
    '                rl = New ProceduralAllowanceRule(New Conditions)
    '        End Select
    '        rlSetStaged.Add(rl)
    '    End If
    '    Return rl
    'End Function

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets a procedure collection from a datareader's database data
    ' </summary>
    ' <param name="stagedProcedureDataReader"></param>
    ' <param name="getAsLightweight"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	7/6/2006	Created
    '     [paulw] 9/19/2006   Added in paramter for condition that corresponds to ACR MED-0018
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Shared Function GetProcedureCollectionFromDataReader(ByVal stagedProcedureDataReader As IDataReader) As Procedures
        If stagedProcedureDataReader Is Nothing Then Throw New ArgumentNullException("stagedProcedureDataReader")
        Dim PROC_ID As Integer?
        Dim BILL_TYPE As String
        Dim Diagnosis As String
        Dim PlaceOfService As String
        Dim PlanType As String
        Dim PROC_CODE As String
        Dim Provider As String
        Dim STATUS As Integer
        Dim Modifier As String = ""
        Dim Gender As String = ""
        Dim MONTHS_MIN As Integer?
        Dim MONTHS_MAX As Integer?
        Dim Procedures As New Hashtable
        Dim Rule As Rule
        Dim Found As Boolean = False
        Dim ProcedureStaged As ProcedureStaged
        Dim RuleSetStaged As RuleSetStaged
        Dim RuleSetActive As RuleSetActive
        Dim Condition As Condition
        Dim ProcCollection As New Procedures

        Try

            While (stagedProcedureDataReader.Read)
                ProcedureStaged = DirectCast(Procedures(CInt(stagedProcedureDataReader("PROC_ID"))), ProcedureStaged)
                If ProcedureStaged Is Nothing Then
                    PROC_ID = CInt(stagedProcedureDataReader("PROC_ID"))
                    BILL_TYPE = CStr(stagedProcedureDataReader("BILL_TYPE")).Trim
                    Diagnosis = CStr(stagedProcedureDataReader("DIAGNOSIS")).Trim
                    Gender = CStr(stagedProcedureDataReader("GENDER")).Trim
                    MONTHS_MIN = CType(stagedProcedureDataReader("MONTHS_MIN"), Integer?)
                    MONTHS_MAX = CType(stagedProcedureDataReader("MONTHS_MAX"), Integer?)
                    PlaceOfService = CStr(stagedProcedureDataReader("PLACE_OF_SERV")).Trim
                    PlanType = CStr(stagedProcedureDataReader("PLAN_TYPE")).Trim
                    PROC_CODE = CStr(stagedProcedureDataReader("PROC_CODE")).Trim
                    Provider = CStr(stagedProcedureDataReader("PROVIDER")).Trim
                    STATUS = CInt(stagedProcedureDataReader("PROCEDURE_STAGING_STATUS"))
                    If (stagedProcedureDataReader("MODIFIER") Is System.DBNull.Value) Then
                        Modifier = "-1"
                    Else
                        Modifier = CStr(stagedProcedureDataReader("MODIFIER"))
                    End If
                    ProcedureStaged = New ProcedureStaged(PROC_ID, BILL_TYPE, Diagnosis, Modifier, Gender, MONTHS_MIN, MONTHS_MAX, PlaceOfService, PlanType, PROC_CODE, Provider, Nothing, Nothing, STATUS)
                    Procedures.Add(ProcedureStaged.ProcedureID, ProcedureStaged)
                End If
            End While

            Dim i As Integer = 0

            If stagedProcedureDataReader.NextResult Then
                While (stagedProcedureDataReader.Read)
                    If stagedProcedureDataReader("RULE_SET_ID") IsNot DBNull.Value Then
                        If Procedures(Convert.ToInt32(stagedProcedureDataReader("PROC_ID"))) IsNot Nothing Then
                            Found = False
                            For Each rlSet As RuleSetStaged In DirectCast(Procedures(Convert.ToInt32(stagedProcedureDataReader("PROC_ID"))), ProcedureStaged).NewRuleSets
                                If rlSet.RulesetType = Convert.ToInt32(stagedProcedureDataReader("RULE_SET_TYPE")) Then
                                    RuleSetStaged = rlSet : Found = True
                                End If
                            Next
                            If Not Found Then
                                RuleSetStaged = Nothing
                            End If
                            Found = False
                            For Each rlSet As RuleSetActive In DirectCast(Procedures(Convert.ToInt32(stagedProcedureDataReader("PROC_ID"))), ProcedureStaged).RuleSets
                                If rlSet.RulesetType = Convert.ToInt32(stagedProcedureDataReader("RULE_SET_TYPE")) Then
                                    RuleSetActive = rlSet : Found = True
                                End If
                            Next
                            If Not Found Then
                                RuleSetActive = Nothing
                            End If

                            If RuleSetStaged Is Nothing Then
                                RuleSetActive = PlanActiveDAL.GetRuleSetActive(stagedProcedureDataReader, DirectCast(Procedures(Convert.ToInt32(stagedProcedureDataReader("PROC_ID"))), ProcedureStaged))
                                RuleSetStaged = GetRuleSetStaged(stagedProcedureDataReader, DirectCast(Procedures(Convert.ToInt32(stagedProcedureDataReader("PROC_ID"))), ProcedureStaged))
                            End If

                            Rule = GetRule(stagedProcedureDataReader, RuleSetStaged, RuleSetActive)
                            Condition = New Condition
                            If stagedProcedureDataReader("ACCUM_NAME") IsNot DBNull.Value Then
                                If stagedProcedureDataReader("ACCUM_NAME").ToString.Trim.Length > 0 Then
                                    Condition.ConditionID = Convert.ToInt32(stagedProcedureDataReader("RULE_ACCUM_COND_ID").ToString())
                                    Condition.AccumulatorName = stagedProcedureDataReader("ACCUM_NAME").ToString()
                                    Condition.Direction = CType(stagedProcedureDataReader("DIRECTION").ToString(), DateDirection)
                                    Condition.Duration = Convert.ToInt32(stagedProcedureDataReader("DURATION").ToString())
                                    Condition.DurationType = CType(stagedProcedureDataReader("DURATION_TYPE"), DateType)
                                    Condition.UseInHeadroomCheck = CType(stagedProcedureDataReader("CHECK_FOR_HEADROOM"), Boolean)
                                End If
                            End If
                            Condition.Operand = CDec(stagedProcedureDataReader("OPERAND").ToString())
                            Rule.Conditions.Add(Condition)
                            CType(Procedures(Convert.ToInt32(stagedProcedureDataReader("PROC_ID"))), ProcedureStaged).NewRuleSets.Add(RuleSetStaged)
                            CType(Procedures(Convert.ToInt32(stagedProcedureDataReader("PROC_ID"))), ProcedureStaged).RuleSets.Add(RuleSetActive)
                        End If
                    End If
                End While

                If Not stagedProcedureDataReader.IsClosed Then
                    stagedProcedureDataReader.Close()
                End If

            End If

            Dim ProceduresEnum As IDictionaryEnumerator = Procedures.GetEnumerator()
            While ProceduresEnum.MoveNext
                ProcCollection.Add(DirectCast(ProceduresEnum.Value, ProcedureStaged))
            End While

            Return ProcCollection

        Catch ex As Exception

            Throw New ConvertDataException("Cannot Convert Data to ProcedureCollection", ex)

        End Try
    End Function

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets a ruleset from a datareader
    ' </summary>
    ' <param name="stagedProcedureDataReader"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	9/12/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Shared Function GetRulesetsFromDataReader(ByVal StagedProcedureDataReader As IDataReader) As RuleSet()
        If StagedProcedureDataReader Is Nothing Then Throw New ArgumentNullException("stagedProcedureDataReader")
        Dim Found As Boolean = False
        Dim RuleSetStaged As RuleSetStaged
        Dim RlSetCol As New Hashtable

        Try
            Dim i As Integer = 0
            'rlSetCol.Add("", "-1")
            'If stagedProcedureDataReader.NextResult Then
            While (StagedProcedureDataReader.Read)
                If StagedProcedureDataReader("RULE_SET_ID") IsNot DBNull.Value Then
                    If RlSetCol.Count = 0 Then
                        RuleSetStaged = GetRuleSetStaged(StagedProcedureDataReader)
                    Else
                        RuleSetStaged = DirectCast(RlSetCol(Convert.ToInt32(StagedProcedureDataReader("RULE_SET_ID"))), RuleSetStaged)
                        If RuleSetStaged Is Nothing Then
                            RuleSetStaged = GetRuleSetStaged(StagedProcedureDataReader)
                        End If
                    End If

                    '    rl = GetRule(stagedProcedureDataReader, rlSetStaged)
                    '    cnd = New Condition
                    '    If Not stagedProcedureDataReader("ACCUM_NAME") Is DBNull.Value Then
                    '        If stagedProcedureDataReader("ACCUM_NAME").ToString.Trim.Length > 0 Then
                    '            cnd.ConditionId = Convert.ToInt32(stagedProcedureDataReader("RULE_ACCUM_COND_ID").ToString())
                    '            cnd.AccumulatorName = stagedProcedureDataReader("ACCUM_NAME").ToString()
                    '            cnd.Direction = CType(stagedProcedureDataReader("DIRECTION").ToString(), UFCW.CMS.Accumulator.MemberAccumulator.DateDirection)
                    '            cnd.Duration = Convert.ToInt32(stagedProcedureDataReader("DURATION").ToString())
                    '            cnd.DurationType = CType(stagedProcedureDataReader("DURATION_TYPE"), MemberAccumulator.DateTypes)
                    '        End If
                    '    End If
                    '    cnd.Operand = cdec(stagedProcedureDataReader("OPERAND").ToString())
                    '    rl.Conditions.AddCondition(cnd)

                End If
                If RlSetCol.Count > 0 Then
                    If RlSetCol(Convert.ToInt32(StagedProcedureDataReader("RULE_SET_ID"))) Is Nothing Then
                        RlSetCol.Add(CStr(StagedProcedureDataReader("RULE_SET_ID")), RuleSetStaged)
                    End If
                Else
                    RlSetCol.Add(CStr(StagedProcedureDataReader("RULE_SET_ID")), RuleSetStaged)
                End If

            End While
            If Not StagedProcedureDataReader.IsClosed Then
                StagedProcedureDataReader.Close()
            End If
            'End If
            Dim RuleSets As RuleSet()
            ReDim RuleSets(RlSetCol.Count - 1)
            Dim Rlsets As IDictionaryEnumerator = RlSetCol.GetEnumerator()
            Dim z As Integer = 0
            While Rlsets.MoveNext
                RuleSets(z) = DirectCast(Rlsets.Value, RuleSet)
                z += 1
            End While

            Return RuleSets

        Catch ex As Exception

            Throw New ConvertDataException("Cannot Convert Data to ProcedureCollection", ex)

        End Try
    End Function
#End Region

#Region "Validation"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Determines if a procedure range is valid
    ' </summary>
    ' <param name="planType"></param>
    ' <param name="beginProcedureCode"></param>
    ' <param name="endProcedureCode"></param>
    ' <param name="procedureCode"></param>
    ' <param name="modifier"></param>
    ' <param name="PLACE_OF_SERV"></param>
    ' <param name="diagnosis"></param>
    ' <param name="billType"></param>
    ' <param name="providerID"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function IsProcedureRangeValid(ByVal planType As String, ByVal beginProcedureCode As String, ByVal endProcedureCode As String, ByVal modifier As String, ByVal location As String, ByVal diagnosis As String, ByVal billType As String, ByVal providerID As String) As Boolean
        Dim DB As Database = CMSDALCommon.CreateDatabase("Rules Database Instance")
        Try
            Dim SQLCommand As String = CMSDALCommon.GetDatabaseName & "." & "VALIDATE_PROCEDURE_RANGE"
            Dim DBCommandWrapper As DbCommand = DB.GetStoredProcCommand(SQLCommand)
            Dim ReturnVal As Boolean
            DB.AddInParameter(DBCommandWrapper, "@PLAN_TYPE", DbType.String, planType)
            DB.AddInParameter(DBCommandWrapper, "@BEGIN_PROC_CODE", DbType.String, beginProcedureCode)
            DB.AddInParameter(DBCommandWrapper, "@END_PROC_CODE", DbType.String, endProcedureCode)
            DB.AddInParameter(DBCommandWrapper, "@MODIFIER", DbType.String, modifier)
            DB.AddInParameter(DBCommandWrapper, "@PLACE_OF_SERV", DbType.String, location)
            DB.AddInParameter(DBCommandWrapper, "@DIAGNOSIS", DbType.String, diagnosis)
            DB.AddInParameter(DBCommandWrapper, "@BILL_TYPE", DbType.String, billType)
            DB.AddInParameter(DBCommandWrapper, "@PROVIDER", DbType.String, providerID)
            DB.AddOutParameter(DBCommandWrapper, "@MATCHES_FOUND", DbType.Int32, 4)
            DBCommandWrapper.CommandTimeout = 1200

            DB.CreateConnection.Open()
            DB.ExecuteNonQuery(DBCommandWrapper)
            ReturnVal = CType(DB.GetParameterValue(DBCommandWrapper, "@MATCHES_FOUND"), Boolean)

            Return ReturnVal

        Catch ex As Exception

            Throw New StagingDataException("Cannot Determine if Staged Procedure Range is Valid", ex)

        Finally
            If (DB.CreateConnection IsNot Nothing) Then
                DB.CreateConnection.Close()
            End If
        End Try
    End Function
#End Region

#Region "Staging"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Stages an Active Plan as a Staged Plan
    ' </summary>
    ' <param name="planType"></param>
    ' <param name="userID"></param>
    ' <param name="status"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Sub StagePlan(ByVal planType As String, ByVal userID As String, ByVal status As Int32)
        Dim DB As Database
        Dim DBConnection As DbConnection
        Dim Transaction As DbTransaction
        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase("Rules Database Instance")
            DBConnection = DB.CreateConnection

            DBConnection.Open()
            Transaction = DBConnection.BeginTransaction
            SQLCommand = CMSDALCommon.GetDatabaseName & "." & "CREATE_STAGING_MASTER"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)
            DB.AddInParameter(DBCommandWrapper, "@PLAN_TYPE", DbType.String, planType)
            DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String, userID)
            DB.AddInParameter(DBCommandWrapper, "@STATUS", DbType.Int32, status)
            DB.AddOutParameter(DBCommandWrapper, "@STAGING_MASTER_ID", DbType.Int32, 4)
            DBCommandWrapper.CommandTimeout = 120
            DB.ExecuteNonQuery(DBCommandWrapper, Transaction)
            Dim stagingMasterID As Int32 = CInt(DB.GetParameterValue(DBCommandWrapper, "@STAGING_MASTER_ID"))

            SQLCommand = CMSDALCommon.GetDatabaseName & "." & "STAGE_PLAN"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)
            DB.AddInParameter(DBCommandWrapper, "@PLAN_TYPE", DbType.String, planType.Trim)
            DB.AddInParameter(DBCommandWrapper, "@STAGING_MASTER_ID", DbType.Int32, stagingMasterID)
            DB.AddInParameter(DBCommandWrapper, "@USERID", DbType.String, userID)
            DBCommandWrapper.CommandTimeout = 120
            DB.ExecuteNonQuery(DBCommandWrapper, Transaction)

            Transaction.Commit()
        Catch ex As Exception
            Transaction.Rollback()

            Throw New StagingDataException("Cannot Stage the Plan", ex)

        Finally
            If (DBConnection IsNot Nothing) Then
                DBConnection.Close()
            End If
        End Try
    End Sub
#End Region

#Region "Clearing"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Removed the rulesets from staging
    ' </summary>
    ' <param name="rlSetId"></param>
    ' <remarks>
    ' Implemented per ACR MED-0011
    ' </remarks>
    ' <history>
    ' 	[paulw]	9/15/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Sub RemoveRulesetFromStaging(ByVal rlSetId As Integer)
        Dim DB As Database = CMSDALCommon.CreateDatabase("Rules Database Instance")
        Dim FileName As String
        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand

        Try
            SQLCommand = CMSDALCommon.GetDatabaseName & "." & "DELETE_RULE_SET_FROM_STAGING"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)
            DB.AddInParameter(DBCommandWrapper, "@RULE_SET_ID", DbType.Int32, rlSetId)
            DBCommandWrapper.CommandTimeout = 1200
            DB.CreateConnection.Open()
            DB.ExecuteNonQuery(DBCommandWrapper)

            FileName = "RSLog_" & UFCWGeneral.NowDate.ToString & ".csv"
            FileName = FileName.Replace("/", "_")
            FileName = FileName.Replace(" ", "_")
            FileName = "C:\" & FileName.Replace(":", "_")

            PlanTraceLog.WriteToFile(FileName, "delete rsid: " & rlSetId)

        Catch ex As Exception

            Throw New StagingDataException("Cannot Remove Ruleset From Staging", ex)

        Finally
            If DB.CreateConnection IsNot Nothing Then
                DB.CreateConnection.Close()
            End If
        End Try
    End Sub
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Removed a ruleset from the staged procedures passed in
    ' </summary>
    ' <param name="procCollection"></param>
    ' <param name="rlSetType"></param>
    ' <remarks>
    ' Implemented per ACR MED-0011
    ' </remarks>
    ' <history>
    ' 	[paulw]	9/15/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Sub RemoveRulesetFromStaging(ByVal procedureCollection As Procedures, ByVal ruleSetType As Integer)
        Dim SQLCommand As String
        Dim DB As Database
        Dim Transaction As DbTransaction
        Dim DBCommandWrapper As DbCommand
        Dim ProcedureBuilder As New StringBuilder
        Dim DBConnection As DbConnection

        Try
            SQLCommand = CMSDALCommon.GetDatabaseName & "." & "DELETE_RULE_SET_FROM_PROCEDURES_STAGING"
            DB = CMSDALCommon.CreateDatabase("Rules Database Instance")
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)
            DBConnection = DB.CreateConnection
            DBConnection.Open()
            Transaction = DBConnection.BeginTransaction

            For I As Integer = 0 To procedureCollection.Count - 1
                ProcedureBuilder.Append(procedureCollection(I).ProcedureID)
                ProcedureBuilder.Append(",")
                If ProcedureBuilder.ToString.Length > 7990 Then
                    DBCommandWrapper = DB.GetStoredProcCommand(CMSDALCommon.GetDatabaseName & "." & "DELETE_RULE_SET_FROM_PROCEDURES_STAGING")
                    DB.AddInParameter(DBCommandWrapper, "@PROC_IDS", DbType.AnsiString, ProcedureBuilder.ToString.Substring(0, ProcedureBuilder.ToString.Length - 1))
                    DB.AddInParameter(DBCommandWrapper, "@RULE_SET_TYPE", DbType.Int32, ruleSetType)
                    DBCommandWrapper.CommandTimeout = 120
                    DB.ExecuteNonQuery(DBCommandWrapper, Transaction)
                    ProcedureBuilder = New StringBuilder
                End If

            Next
            DBCommandWrapper = DB.GetStoredProcCommand(CMSDALCommon.GetDatabaseName & "." & "DELETE_RULE_SET_FROM_PROCEDURES_STAGING")

            Dim ProcedureString As String = ProcedureBuilder.ToString.Substring(0, ProcedureBuilder.ToString.Length - 1)
            If ProcedureString.EndsWith(",0") Then
                ProcedureString = ProcedureString.Substring(0, ProcedureString.Length - 2)
            End If

            DB.AddInParameter(DBCommandWrapper, "@PROC_IDS", DbType.AnsiString, ProcedureString)
            DB.AddInParameter(DBCommandWrapper, "@RULE_SET_TYPE", DbType.Int32, ruleSetType)
            DBCommandWrapper.CommandTimeout = 1200
            DB.ExecuteNonQuery(DBCommandWrapper, Transaction)

            Transaction.Commit()
        Catch ex As Exception
            Transaction.Rollback()

            Throw New StagingDataException("Connot remove the ruleset from staging", ex)

        Finally
            If DBConnection IsNot Nothing Then
                DBConnection.Close()
            End If
        End Try
    End Sub
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Clears a staged plan
    ' </summary>
    ' <param name="planType"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Sub ClearStagedPlan(ByVal planType As String)
        Dim DB As Database
        Dim DBConnection As DbConnection
        Dim Transaction As DbTransaction
        Try
            DB = CMSDALCommon.CreateDatabase("Rules Database Instance")
            DBConnection = DB.CreateConnection()
            DBConnection.Open()
            Transaction = DBConnection.BeginTransaction
            ClearStagedPlan(Transaction, planType)
            Transaction.Commit()
        Catch ex As Exception
            Transaction.Rollback()

            Throw New StagingDataException("Cannot Clear the Staged Plan", ex)

        Finally
            If DBConnection IsNot Nothing Then
                DBConnection.Close()
            End If
        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Clears a staged plan
    ' </summary>
    ' <param name="activedb"></param>
    ' <param name="planType"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Sub ClearStagedPlan(ByVal transaction As DbTransaction, ByVal planType As String)
        Try
            Dim SQLCommand As String = CMSDALCommon.GetDatabaseName & "." & "CLEAR_STAGED_PLAN"
            Dim DB As Database = CMSDALCommon.CreateDatabase("Rules Database Instance")
            Dim DBCommandWrapper As DbCommand = DB.GetStoredProcCommand(SQLCommand)
            DB.AddInParameter(DBCommandWrapper, "@PLAN_TYPE", DbType.String, planType)
            DBCommandWrapper.CommandTimeout = 1200
            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

        Catch ex As Exception
            Throw New StagingDataException("Cannot Clear the Staged Plan", ex)

        End Try
    End Sub

#End Region

#Region "Updates"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Updates the staging master table
    ' </summary>
    ' <param name="planType"></param>
    ' <param name="modifiedDate"></param>
    ' <param name="modifiedUserID"></param>
    ' <param name="status"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function UpdateStagingMaster(ByVal planType As String, ByVal modifiedUserID As String, ByVal status As Int32) As Integer
        Dim DB As Database = CMSDALCommon.CreateDatabase("Rules Database Instance")
        Try
            Dim SQLCommand As String = CMSDALCommon.GetDatabaseName & "." & "UPDATE_STAGING_MASTER"
            Dim DBCommandWrapper As DbCommand = DB.GetStoredProcCommand(SQLCommand)
            Dim ReturnVal As Integer

            DB.AddInParameter(DBCommandWrapper, "@PLAN_TYPE", DbType.String, planType)
            DB.AddInParameter(DBCommandWrapper, "@USERID", DbType.String, modifiedUserID)
            DB.AddInParameter(DBCommandWrapper, "@STATUS", DbType.Int32, status)
            DB.AddOutParameter(DBCommandWrapper, "@STAGING_MASTER_ID", DbType.Int32, 4)
            DBCommandWrapper.CommandTimeout = 1200

            DB.CreateConnection.Open()
            DB.ExecuteNonQuery(DBCommandWrapper)
            ReturnVal = CInt(DB.GetParameterValue(DBCommandWrapper, "@STAGING_MASTER_ID"))

            Return ReturnVal

        Catch ex As Exception

            Throw New StagingDataException("Cannot Update the Staging Master table", ex)

        Finally
            If (DB.CreateConnection IsNot Nothing) Then
                DB.CreateConnection.Close()
            End If
        End Try
    End Function

#End Region

#Region "Constructors"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Constructor to avoid this class being instantiated
    ' </summary>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub New()
    End Sub
#End Region

#Region "Batch Processing"
    Public Shared Sub StageAndPublishSpreadsheet(ByVal excelApp As Excel.Application, ByVal planType As String, ByVal fromDate As Date, ByVal toDate As Date, ByVal userId As String)
        Try
            Debug.WriteLine("Begin Batch Publish: " & UFCWGeneral.NowDate)
            Dim bgTime As DateTime = UFCWGeneral.NowDate
            'NOTE:  The next to Subs use the SqlClient library
            '        because performance was suffering when using
            '        the Enterprise Library.  This decision was made
            '        because we were so close to deadline and because
            '        initially we are not using these two subs to hit
            '        the DB2 database.
            StageFromSpreadsheet(excelApp, planType) ', transaction)
            PublishStaged(fromDate, toDate, userId) ', transaction)
            Dim s As String = "End Batch Publish: " & UFCWGeneral.NowDate & " difference of: " & DateDiff(DateInterval.Second, bgTime, UFCWGeneral.NowDate)
            Debug.WriteLine(s)
        Catch ex As Exception

            Throw New Exception("Cannot Stage and Publish", ex)

        End Try

    End Sub
    Private Shared Sub PublishStaged(ByVal fromDate As Date, ByVal toDate As Date, ByVal userId As String) ', ByVal transaction As DbTransaction)
        Dim DB As Database = CMSDALCommon.CreateDatabase("Rules Database Instance")
        Dim Conn As SqlClient.SqlConnection
        Dim Cmd As SqlClient.SqlCommand

        Try
            Conn = New SqlClient.SqlConnection(DB.CreateConnection.ConnectionString)
            Conn.Open()
            Cmd = New SqlClient.SqlCommand("PUBLISH_BATCHPUB_TABLES", Conn)
            Cmd.CommandType = CommandType.StoredProcedure
            Cmd.CommandTimeout = 300000
            Cmd.Parameters.Add(New SqlClient.SqlParameter("@USERID", SqlDbType.VarChar, 40, ParameterDirection.Input, False, 0, 0, "@USERID", DataRowVersion.Current, userId))
            Cmd.Parameters.Add(New SqlClient.SqlParameter("@OCC_FROM_DATE", SqlDbType.DateTime, 8, ParameterDirection.Input, False, 0, 0, "@OCC_FROM_DATE", DataRowVersion.Current, fromDate))
            Cmd.Parameters.Add(New SqlClient.SqlParameter("@OCC_TO_DATE", SqlDbType.DateTime, 8, ParameterDirection.Input, False, 0, 0, "@OCC_TO_DATE", DataRowVersion.Current, toDate))
            Cmd.ExecuteNonQuery()
        Catch ex As SqlClient.SqlException
            Dim sErr As String = "SQL Error" + vbCrLf
            Dim SQLErr As SqlClient.SqlError

            For Each SQLErr In ex.Errors
                Dim sMsg As String = ""
                If SQLErr.Procedure.Length > 0 Then sMsg += "Procedure:" + SQLErr.Procedure
                If SQLErr.Number > 0 Then sMsg += If(sMsg.Length > 0, ", ", String.Empty) + "Code: " + CType(SQLErr.Number, String)
                If SQLErr.LineNumber > 0 Then sMsg += If(sMsg.Length > 0, ", ", String.Empty) + "Line Number: " + CType(SQLErr.LineNumber, String)
                If SQLErr.Message.Length > 0 Then sMsg += If(sMsg.Length > 0, ", ", String.Empty) + "Message: " + SQLErr.Message
                sMsg += vbCrLf
                sErr += sMsg
            Next

            Throw New Exception(sErr, ex)

        Catch ex As Exception

            Throw New Exception("Cannot Publish", ex)

        Finally
            If Conn IsNot Nothing Then Conn.Close()
        End Try
    End Sub
    Private Shared Sub StageFromSpreadsheet(ByVal excelApp As Excel.Application, ByVal planType As String) ', ByVal transaction As DbTransaction)
        Dim DB As Database = CMSDALCommon.CreateDatabase("Rules Database Instance")
        Dim SQLConn As SqlClient.SqlConnection
        Dim SQLCmd As SqlClient.SqlCommand

        Dim LineNo As Integer

        Dim XLSLines As System.Collections.ArrayList

        Try

            If excelApp Is Nothing Then excelApp = CType(CreateObject("Excel.Application"), Excel.Application)

            Using WC As New GlobalCursor

                For Each XLSSheet As Excel.Worksheet In excelApp.ActiveWorkbook.Worksheets
                    XLSLines = New ArrayList(100)

                    If XLSSheet.Name.ToUpper.Trim.Replace("PLAN", "").Trim = planType Then
                        For LineNo = 2 To XLSSheet.UsedRange.Rows.Count

                            If (CType(XLSSheet.Cells(LineNo, XLCOL.RuleSetName), Excel.Range).Value IsNot Nothing AndAlso CStr(CType(XLSSheet.Cells(LineNo, XLCOL.RuleSetName), Excel.Range).Value).Trim.Length > 0) OrElse
                               (CType(XLSSheet.Cells(LineNo, XLCOL.ProcedureCode), Excel.Range).Value IsNot Nothing AndAlso CStr(CType(XLSSheet.Cells(LineNo, XLCOL.ProcedureCode), Excel.Range).Value).Trim.Length > 0) OrElse
                               (CType(XLSSheet.Cells(LineNo, XLCOL.RuleType), Excel.Range).Value IsNot Nothing AndAlso CStr(CType(XLSSheet.Cells(LineNo, XLCOL.RuleType), Excel.Range).Value).Trim.Length > 0) Then

                                Try

                                    XLSLines.Add(New XLSBatchLine(LineNo,
                                                                 planType,
                                                                 CStr(CType(XLSSheet.Cells(LineNo, XLCOL.RuleSetName), Excel.Range).Value),
                                                                 CStr(CType(XLSSheet.Cells(LineNo, XLCOL.RuleSetType), Excel.Range).Value),
                                                                 CStr(CType(XLSSheet.Cells(LineNo, XLCOL.ProcedureCode), Excel.Range).Value),
                                                                 CStr(CType(XLSSheet.Cells(LineNo, XLCOL.ProviderClass), Excel.Range).Value),
                                                                 CStr(CType(XLSSheet.Cells(LineNo, XLCOL.PlaceOfService), Excel.Range).Value),
                                                                 CStr(CType(XLSSheet.Cells(LineNo, XLCOL.Diagnosis), Excel.Range).Value),
                                                                 CStr(CType(XLSSheet.Cells(LineNo, XLCOL.BillType), Excel.Range).Value),
                                                                 CStr(CType(XLSSheet.Cells(LineNo, XLCOL.Modifier), Excel.Range).Value),
                                                                 CStr(CType(XLSSheet.Cells(LineNo, XLCOL.Gender), Excel.Range).Value),
                                                                 CStr(CType(XLSSheet.Cells(LineNo, XLCOL.MonthsMin), Excel.Range).Value),
                                                                 CStr(CType(XLSSheet.Cells(LineNo, XLCOL.MonthsMax), Excel.Range).Value),
                                                                 CStr(CType(XLSSheet.Cells(LineNo, XLCOL.RuleType), Excel.Range).Value),
                                                                 CStr(CType(XLSSheet.Cells(LineNo, XLCOL.AccumulatorName), Excel.Range).Value),
                                                                 CStr(CType(XLSSheet.Cells(LineNo, XLCOL.Amount), Excel.Range).Value),
                                                                 CStr(CType(XLSSheet.Cells(LineNo, XLCOL.DurationType), Excel.Range).Value),
                                                                 CStr(CType(XLSSheet.Cells(LineNo, XLCOL.CheckForMaxHeadroom), Excel.Range).Value),
                                                                 CStr(CType(XLSSheet.Cells(LineNo, XLCOL.RepriceIfExceeded), Excel.Range).Value)))
                                Catch ex As Exception

                                    Throw New Exception("Error processing spreadsheet Line: " & LineNo, ex)

                                End Try
                            End If
                        Next
                        Exit For
                    End If
                Next

                If XLSLines.Count = 0 Then
                    Exit Sub
                End If

            End Using

            SQLConn = New SqlClient.SqlConnection(DB.CreateConnection.ConnectionString)
            SQLConn.Open()

            SQLCmd = New SqlClient.SqlCommand("RESET_BATCHPUB", SQLConn)
            SQLCmd.CommandType = CommandType.StoredProcedure
            SQLCmd.ExecuteNonQuery()


            Using WC As New GlobalCursor

                For Each XLSLine As XLSBatchLine In XLSLines

                    SQLCmd = New SqlClient.SqlCommand("INSERT INTO dbo.BATCHPUB(PLAN_TYPE, RULE_SET_NAME, RULE_SET_TYPE, PROC_CODES, PROVIDERS, PLACE_OF_SERVS, DIAGNOSIS, BILL_TYPES, MODIFIERS, GENDER, MONTHS_MIN, MONTHS_MAX, RULE_TYPE, ACCUM_NAME, OPERAND, DURATION_TYPE_DESCRIPTION, CHECK_FOR_HEADROOM, REPRICE_IF_EXCEEDED) " +
                                                   "VALUES (@PlanName, @RuleSetName, @RuleSetType, @ProcedureCode, @ProviderClass, @PlaceOfService, @Diagnosis, @BillType, @Modifier, @Gender, @MonthsMin, @MonthsMax, @RuleType, @AccumulatorName, @Amount, @DurationType, @CheckForMaxHeadroom, @RepriceIfExceeded)", SQLConn)

                    SQLCmd.CommandType = CommandType.Text

                    SQLCmd.Parameters.Add(New SqlClient.SqlParameter("@PlanName", SqlDbType.VarChar, 40, ParameterDirection.Input, True, 0, 0, "PLAN_TYPE", DataRowVersion.Current, UFCWGeneral.ToNullStringHandler(XLSLine._Plan)))
                    SQLCmd.Parameters.Add(New SqlClient.SqlParameter("@RuleSetName", SqlDbType.VarChar, 100, ParameterDirection.Input, True, 0, 0, "RULE_SET_NAME", DataRowVersion.Current, UFCWGeneral.ToNullStringHandler(XLSLine._RuleSetName)))
                    SQLCmd.Parameters.Add(New SqlClient.SqlParameter("@RuleSetType", SqlDbType.VarChar, 40, ParameterDirection.Input, True, 0, 0, "RULE_SET_TYPE", DataRowVersion.Current, UFCWGeneral.ToNullStringHandler(XLSLine._RuleSetType)))
                    SQLCmd.Parameters.Add(New SqlClient.SqlParameter("@ProcedureCode", SqlDbType.VarChar, 30000, ParameterDirection.Input, True, 0, 0, "PROC_CODES", DataRowVersion.Current, UFCWGeneral.ToNullStringHandler(XLSLine._ProcedureCode)))
                    SQLCmd.Parameters.Add(New SqlClient.SqlParameter("@ProviderClass", SqlDbType.VarChar, 30000, ParameterDirection.Input, True, 0, 0, "PROVIDERS", DataRowVersion.Current, UFCWGeneral.ToNullStringHandler(XLSLine._ProviderClass)))
                    SQLCmd.Parameters.Add(New SqlClient.SqlParameter("@PlaceOfService", SqlDbType.VarChar, 30000, ParameterDirection.Input, True, 0, 0, "PLACE_OF_SERVS", DataRowVersion.Current, UFCWGeneral.ToNullStringHandler(XLSLine._PlaceOfService)))
                    SQLCmd.Parameters.Add(New SqlClient.SqlParameter("@Diagnosis", SqlDbType.VarChar, 30000, ParameterDirection.Input, True, 0, 0, "DIAGNOSIS", DataRowVersion.Current, UFCWGeneral.ToNullStringHandler(XLSLine._Diagnosis)))
                    SQLCmd.Parameters.Add(New SqlClient.SqlParameter("@BillType", SqlDbType.VarChar, 30000, ParameterDirection.Input, True, 0, 0, "BILL_TYPES", DataRowVersion.Current, UFCWGeneral.ToNullStringHandler(XLSLine._BillType)))
                    SQLCmd.Parameters.Add(New SqlClient.SqlParameter("@Modifier", SqlDbType.VarChar, 30000, ParameterDirection.Input, True, 0, 0, "MODIFIERS", DataRowVersion.Current, UFCWGeneral.ToNullStringHandler(XLSLine._Modifier)))
                    SQLCmd.Parameters.Add(New SqlClient.SqlParameter("@Gender", SqlDbType.VarChar, 30000, ParameterDirection.Input, True, 0, 0, "GENDER", DataRowVersion.Current, UFCWGeneral.ToNullStringHandler(XLSLine._Gender)))
                    SQLCmd.Parameters.Add(New SqlClient.SqlParameter("@MonthsMin", SqlDbType.VarChar, 30000, ParameterDirection.Input, True, 0, 0, "MONTHS_MIN", DataRowVersion.Current, UFCWGeneral.ToNullDecimalHandler(XLSLine._MonthsMin)))
                    SQLCmd.Parameters.Add(New SqlClient.SqlParameter("@MonthsMax", SqlDbType.VarChar, 30000, ParameterDirection.Input, True, 0, 0, "MONTHS_MAX", DataRowVersion.Current, UFCWGeneral.ToNullDecimalHandler(XLSLine._MonthsMax)))
                    SQLCmd.Parameters.Add(New SqlClient.SqlParameter("@RuleType", SqlDbType.VarChar, 40, ParameterDirection.Input, True, 0, 0, "RULE_TYPE", DataRowVersion.Current, UFCWGeneral.ToNullStringHandler(XLSLine._RuleType)))
                    SQLCmd.Parameters.Add(New SqlClient.SqlParameter("@AccumulatorName", SqlDbType.Char, 5, ParameterDirection.Input, True, 0, 0, "ACCUM_NAME", DataRowVersion.Current, UFCWGeneral.ToNullStringHandler(XLSLine._AccumulatorName)))
                    SQLCmd.Parameters.Add(New SqlClient.SqlParameter("@Amount", SqlDbType.Decimal, 40, ParameterDirection.Input, True, 13, 4, "OPERAND", DataRowVersion.Current, UFCWGeneral.ToNullDecimalHandler(XLSLine._Amount)))
                    SQLCmd.Parameters.Add(New SqlClient.SqlParameter("@DurationType", SqlDbType.VarChar, 20, ParameterDirection.Input, True, 0, 0, "DURATION_TYPE_DESCRIPTION", DataRowVersion.Current, UFCWGeneral.ToNullStringHandler(XLSLine._DurationType)))
                    SQLCmd.Parameters.Add(New SqlClient.SqlParameter("@CheckForMaxHeadroom", SqlDbType.VarChar, 1, ParameterDirection.Input, True, 0, 0, "CHECK_FOR_HEADROOM", DataRowVersion.Current, UFCWGeneral.ToNullStringHandler(XLSLine._CheckForMaxHeadroom)))
                    SQLCmd.Parameters.Add(New SqlClient.SqlParameter("@RepriceIfExceeded", SqlDbType.VarChar, 1, ParameterDirection.Input, True, 0, 0, "REPRICE_IF_EXCEEDED", DataRowVersion.Current, UFCWGeneral.ToNullStringHandler(XLSLine._RepriceIfExceeded)))
                    SQLCmd.ExecuteNonQuery()
                Next

                SQLCmd = New SqlClient.SqlCommand("RELOAD_BATCHPUB_TABLES", SQLConn)
                SQLCmd.CommandType = CommandType.StoredProcedure
                SQLCmd.CommandTimeout = 300000
                SQLCmd.ExecuteNonQuery()

            End Using

        Catch ex As SqlClient.SqlException

            Dim sErr As String = "SQL Error" + vbCrLf
            Dim SQLErr As SqlClient.SqlError

            For Each SQLErr In ex.Errors
                Dim sMsg As String = ""
                If ex.Procedure.Length > 0 Then sMsg += "Procedure:" + ex.Procedure
                If ex.Number > 0 Then sErr += If(sMsg.Length > 0, ", ", String.Empty) + "Code: " + CType(ex.Number, String)
                If ex.LineNumber > 0 Then sErr += If(sMsg.Length > 0, ", ", String.Empty) + "Line Number: " + CType(ex.LineNumber, String)
                If ex.Message.Length > 0 Then sErr += If(sMsg.Length > 0, ", ", String.Empty) + "Message: " + ex.Message
                sMsg += vbCrLf
                sErr += sMsg
            Next

            Throw New Exception(sErr, ex)

        Catch ex As Exception

            Throw New Exception("Cannot get conditions", ex)

        Finally
            If SQLConn IsNot Nothing Then SQLConn.Close()
        End Try
    End Sub
    Private Shared Function SafeStr(ByVal obj As Object) As String
        If obj Is Nothing Then Return ""
        Try
            Return CType(obj, String)
        Catch ex As Exception
            Throw
        End Try
    End Function
#End Region

#Region "Internal Class"
    Private Class XLSBatchLine
        Public _LineNo As Integer?
        Public _Plan As String
        Public _RuleSetName As String
        Public _RuleSetType As String
        Public _ProcedureCode As String
        Public _ProviderClass As String
        Public _PlaceOfService As String
        Public _Diagnosis As String
        Public _BillType As String
        Public _Modifier As String
        Public _Gender As String
        Public _MonthsMin As Decimal?
        Public _MonthsMax As Decimal?
        Public _RuleType As String
        Public _AccumulatorName As String
        Public _Amount As Decimal?
        Public _DurationType As String
        Public _CheckForMaxHeadroom As String
        Public _RepriceIfExceeded As String

        Protected Sub New()

        End Sub

        Public Sub New(ByVal lineNo As Integer, ByVal plan As String, ByVal ruleSetName As String, ByVal ruleSetType As String, ByVal procedureCode As String, ByVal providerClass As String, ByVal placeOfService As String, ByVal diagnosis As String, ByVal billType As String, ByVal modifier As String, ByVal gender As String, ByVal monthsMin As String, ByVal monthsMax As String, ByVal ruleType As String, ByVal accumulatorName As String, ByVal amount As String, ByVal durationType As String, ByVal checkForMaxHeadroom As String, ByVal repriceIfExceeded As String)

            _LineNo = lineNo

            If plan IsNot Nothing AndAlso plan.Trim.Length > 0 Then
                _Plan = plan.Trim
            Else
                _Plan = Nothing
            End If

            If ruleSetName IsNot Nothing AndAlso ruleSetName.Trim.Length > 0 Then
                _RuleSetName = ruleSetName.Trim
            Else
                _RuleSetName = Nothing
            End If

            If ruleSetType IsNot Nothing AndAlso ruleSetType.Trim.Length > 0 Then
                Dim result As RuleSetTypes
                If [Enum].TryParse(ruleSetType.Trim.Replace(" ", ""), True, result) Then
                    _RuleSetType = ruleSetType.Trim
                Else
                    Throw New ArgumentException("Invalid ->" & CStr(ruleSetType), "RuleSetType")
                End If
            Else
                _RuleSetType = Nothing
            End If

            If procedureCode IsNot Nothing AndAlso procedureCode.Trim.Length > 0 Then
                _ProcedureCode = procedureCode.Trim
            Else
                _ProcedureCode = Nothing
            End If

            If providerClass IsNot Nothing AndAlso providerClass.Trim.Length > 0 Then
                Dim providerClassA As String() = providerClass.Split(CChar(","))
                For Each provider As String In providerClassA.ToList
                    Select Case provider.Trim
                        Case "N", "O", "P"
                        Case Else
                            Throw New ArgumentException("Invalid ->" & CStr(provider), "ProviderClass")
                    End Select
                Next
                _ProviderClass = providerClass.Trim
            Else
                _ProviderClass = Nothing
            End If

            If placeOfService IsNot Nothing AndAlso placeOfService.Trim.Length > 0 Then
                _PlaceOfService = placeOfService.Trim
            Else
                _PlaceOfService = Nothing
            End If

            If diagnosis IsNot Nothing AndAlso diagnosis.Trim.Length > 0 Then
                _Diagnosis = diagnosis.Trim
            Else
                _Diagnosis = Nothing
            End If

            If billType IsNot Nothing AndAlso billType.Trim.Length > 0 Then
                _BillType = billType.Trim
            Else
                _BillType = Nothing
            End If

            If modifier IsNot Nothing AndAlso modifier.Trim.Length > 0 Then
                _Modifier = modifier.Trim
            Else
                _Modifier = Nothing
            End If

            If gender IsNot Nothing AndAlso gender.Trim.Length > 0 Then
                Select Case gender.ToUpper
                    Case "M", "F"
                        _Gender = gender.Trim.ToUpper
                    Case Else
                        Throw New ArgumentException("Invalid ->" & CStr(gender), "Gender")
                End Select
            Else
                _Gender = Nothing
            End If

            If IsDecimal(monthsMin) Then
                _MonthsMin = CType(monthsMin, Decimal)
            ElseIf monthsMin IsNot Nothing AndAlso monthsMin.Trim.Length > 0 Then
                Throw New ArgumentException("Invalid ->" & CStr(monthsMin), "MonthsMin")
            Else
                _MonthsMin = Nothing
            End If

            If IsDecimal(monthsMax) Then
                _MonthsMax = CType(monthsMax, Decimal)
            ElseIf monthsMax IsNot Nothing AndAlso monthsMax.Trim.Length > 0 Then
                Throw New ArgumentException("Invalid ->" & CStr(monthsMax), "MonthsMax")
            Else
                _MonthsMax = Nothing
            End If

            If ruleType IsNot Nothing AndAlso ruleType.Trim.Length > 0 Then
                Dim result As RuleTypes
                If [Enum].TryParse(ruleType.Trim.Replace(" ", "").Replace("-", ""), True, result) Then
                    _RuleType = ruleType.Trim
                Else
                    Throw New ArgumentException("Invalid ->" & CStr(ruleType), "RuleType")
                End If
            Else
                _RuleType = Nothing
            End If

                If accumulatorName IsNot Nothing AndAlso accumulatorName.Trim.Length > 0 Then
                    _AccumulatorName = accumulatorName.Trim
                Else
                    _AccumulatorName = Nothing
                End If

                If IsDecimal(amount) Then
                    _Amount = CType(amount, Decimal)
                ElseIf amount IsNot Nothing AndAlso amount.Trim.Length > 0 Then
                    Throw New ArgumentException("Invalid ->" & CStr(amount), "Amount")
                Else
                    _Amount = Nothing
                End If

                If durationType IsNot Nothing AndAlso durationType.Trim.Length > 0 Then
                    Select Case durationType.Trim.ToLower
                        Case "90days", "year", "2year", "3year", "5year", "rollover", "lifetime"
                            _DurationType = durationType.Trim
                        Case Else
                            Throw New ArgumentException("Invalid ->" & CStr(durationType), "DurationType")
                    End Select
                Else
                    _DurationType = Nothing
                End If

                If checkForMaxHeadroom IsNot Nothing AndAlso checkForMaxHeadroom.Trim.Length > 0 Then
                    Select Case checkForMaxHeadroom.ToUpper
                        Case "Y", "N"
                            _CheckForMaxHeadroom = checkForMaxHeadroom.Trim
                        Case Else
                            Throw New ArgumentException("Invalid ->" & CStr(checkForMaxHeadroom), "CheckForMaxHeadroom")
                    End Select
                Else
                    _CheckForMaxHeadroom = Nothing
                End If

                If repriceIfExceeded IsNot Nothing AndAlso repriceIfExceeded.Trim.Length > 0 Then
                    Select Case repriceIfExceeded.ToUpper
                        Case "Y", "N"
                            _RepriceIfExceeded = repriceIfExceeded.Trim
                        Case Else
                            Throw New ArgumentException("Invalid ->" & CStr(repriceIfExceeded), "RepriceIfExceeded")
                    End Select
                Else
                    _RepriceIfExceeded = Nothing
                End If

        End Sub

    End Class
#End Region
End Class
Friend Enum XLCOL
    Plan = 1
    RuleSetName
    RuleSetType
    ProcedureCode
    ProviderClass
    PlaceOfService
    Diagnosis
    BillType
    Modifier
    Gender
    MonthsMin
    MonthsMax
    RuleType
    AccumulatorName
    Amount
    DurationType
    CheckForMaxHeadroom
    RepriceIfExceeded
End Enum
