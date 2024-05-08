Option Explicit On
Option Strict On
Option Infer On

Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.IO
Imports System.Xml.Serialization
Imports System.Data.Common
Imports System.Configuration

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.PlanActiveDAL
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class handles the C.R.U.D. for all active plans
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	1/25/2006	Created
''' </history>
''' -----------------------------------------------------------------------------

Friend NotInheritable Class PlanActiveDAL

    '    Private Shared _AccumulatorConditionsForYearDS As DataSet
    Private Shared _AccumulatorsOfAllPlansDS As DataSet
    Private Shared ReadOnly _TraceCaching As New TraceSwitch("TraceCaching", "Trace Switch in App.Config", "0")
    Private Shared ReadOnly _TraceParallel As New TraceSwitch("TraceParallel", "Parallel Trace Switch in App.Config", "0")
    Private Shared ReadOnly _RulesV2 As Boolean = CBool(((ConfigurationManager.AppSettings("EnableRulesV2") IsNot Nothing) AndAlso CBool(ConfigurationManager.AppSettings("EnableRulesV2"))))


#Region "Gets"

    Public Shared Function PlanAccumulators() As DataTable

        If _AccumulatorsOfAllPlansDS Is Nothing OrElse _AccumulatorsOfAllPlansDS.Tables.Count < 1 Then
            _AccumulatorsOfAllPlansDS = GetAccumulators()
        End If

        Return _AccumulatorsOfAllPlansDS.Tables(0)

    End Function

    Public Shared Function GetDistinctConditions() As Conditions
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' This routine gets all the accumulators currently available (irrelevant of year), and filters out the duplicates (aka returns only distinct)
        ' Note: An accumulator is considered distinct if any of it's columns vary between an accumulator of the same name !!!
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	2/5/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim Condition As Condition
        Dim Conditions As Conditions
        Dim MatchFound As Boolean = False
        Dim GroupNumbers As ArrayList
        Dim UniqueThreadIdentifier As String

        Try

            Conditions = New Conditions
            GroupNumbers = New ArrayList

            UniqueThreadIdentifier = UFCWGeneral.GetUniqueKey()

            For Each DR As DataRow In PlanAccumulators.Rows
                Condition = New Condition
                If DR("ACCUM_NAME") IsNot DBNull.Value Then
                    Condition.AccumulatorName = DR("ACCUM_NAME").ToString
                End If
                If DR("ACTIVE_SW") IsNot DBNull.Value Then
                    Condition.Active = CInt(DR("ACTIVE_SW"))
                End If
                If DR("PREVENTIVE_SW") IsNot DBNull.Value Then
                    Condition.Preventive = CInt(DR("PREVENTIVE_SW"))
                End If
                If DR("MANUAL_SW") IsNot DBNull.Value Then
                    Condition.Manual = CInt(DR("MANUAL_SW"))
                End If
                If DR.Table.Columns.Contains("BATCH_SW") AndAlso DR("BATCH_SW") IsNot DBNull.Value Then
                    Condition.Batch = CInt(DR("BATCH_SW"))
                End If
                If DR("DIRECTION") IsNot DBNull.Value Then
                    Condition.Direction = CType(Convert.ToInt32(DR("DIRECTION").ToString), DateDirection)
                End If
                If DR("DURATION") IsNot DBNull.Value Then
                    Condition.Duration = CInt(DR("DURATION"))
                End If
                If DR("DURATION_TYPE") IsNot DBNull.Value Then
                    Condition.DurationType = CType(Convert.ToInt32(DR("DURATION_TYPE")), DateType)
                End If
                If DR.Table.Columns.Contains("OPERAND") AndAlso DR("OPERAND") IsNot DBNull.Value Then
                    Condition.Operand = CDec(DR("OPERAND"))
                End If
                If DR.Table.Columns.Contains("CHECK_FOR_HEADROOM") AndAlso DR("CHECK_FOR_HEADROOM") IsNot DBNull.Value Then
                    Condition.UseInHeadroomCheck = CBool(DR("CHECK_FOR_HEADROOM"))
                End If
                If DR.Table.Columns.Contains("REPRICE_EXCEEDED_SW") AndAlso DR("REPRICE_EXCEEDED_SW") IsNot DBNull.Value Then
                    Condition.RepriceIfExceeded = CBool(DR("REPRICE_EXCEEDED_SW"))
                End If

                Conditions.Add(Condition)
            Next

            Return Conditions

        Catch ex As Exception

            Throw New Exception("Cannot get conditions: " & ex.ToString, ex)

        Finally
        End Try
    End Function

    'Public Shared Function GetDistinctAccumulatorsForQueryDate(ByVal dateOfQuery As Date, Optional ByVal planType As String = Nothing) As Conditions

    '    Dim AccumulatorDistinctConditionsForYearDS As New DataSet

    '    Dim Condition As Condition
    '    Dim Conditions As New Conditions
    '    Dim DRs() As DataRow

    '    Try

    '        GetPlanAccumulatorConditionsForDOS(dateOfQuery)

    '        AccumulatorDistinctConditionsForYearDS = _AccumulatorConditionsForYearDS.Clone

    '        Dim Query = _
    '            From accum In AccumulatorDistinctConditionsForYearDS.Tables(0) _
    '            Group accum By accum!ACCUM_NAME _
    '            Into Max(accum!PUBLISH_BATCH_NBR)

    '        Select Case planType Is Nothing
    '            Case False
    '                Query = _
    '                    From accum In AccumulatorDistinctConditionsForYearDS.Tables(0) _
    '                    Where (accum.Field(Of String)("PLAN_TYPE") = planType.ToString)
    '                    Group accum By accum!ACCUM_NAME _
    '                    Into Max(accum!PUBLISH_BATCH_NBR)
    '            Case True

    '        End Select

    '        For Each r In Query
    '            Condition = New Condition

    '            Dim SQLSelect As String = "ACCUM_NAME = '" & r.ACCUM_NAME.ToString & "' AND PUBLISH_BATCH_NBR = " & r.Max.ToString
    '            If planType IsNot Nothing Then
    '                SQLSelect &= " AND PLAN_TYPE = '" & planType.ToString & "'"
    '            End If
    '            DRs = AccumulatorDistinctConditionsForYearDS.Tables(0).Select(SQLSelect)

    '            If DRs(0)("ACCUM_NAME") IsNot DBNull.Value Then
    '                Condition.AccumulatorName = DRs(0)("ACCUM_NAME").ToString
    '            End If
    '            If DRs(0)("ACCUMULATOR_YEAR") IsNot DBNull.Value Then
    '                Condition.AccumulatorYear = Convert.ToInt32(DRs(0)("ACCUMULATOR_YEAR"))
    '            End If
    '            Conditions.Add(Condition)
    '        Next

    '        Return Conditions

    '    Catch ex As Exception
    '        Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '        If (Rethrow) Then
    '            Throw New Exception("Cannot get conditions: " & ex.ToString, ex)
    '        End If
    '    Finally
    '    End Try

    'End Function

    Public Shared Function GetDistinctConditionsForDOS(dateOfService As Date) As Conditions

        Dim Condition As Condition
        Dim Conditions As New Conditions

        Try

            Dim QueryAccum =
                (
                    From Accum In PlanAccumulators.AsEnumerable
                    Where dateOfService >= Accum.Field(Of Date)("OCC_FROM_DATE") AndAlso dateOfService <= Accum.Field(Of Date)("OCC_TO_DATE")
                    Group Accum By ACCUM_NAME = Accum.Field(Of String)("ACCUM_NAME"),
                                OCC_FROM_DATE = Accum.Field(Of Date)("OCC_FROM_DATE"),
                                OCC_TO_DATE = Accum.Field(Of Date)("OCC_TO_DATE"),
                                ACTIVE_SW = Accum.Field(Of Decimal)("ACTIVE_SW"),
                                MANUAL_SW = Accum.Field(Of Decimal)("MANUAL_SW"),
                                PREVENTIVE_SW = Accum.Field(Of Decimal)("PREVENTIVE_SW"),
                                BATCH_SW = Accum.Field(Of Decimal)("BATCH_SW"),
                                DIRECTION = Accum.Field(Of Decimal)("DIRECTION"),
                                DURATION = Accum.Field(Of Integer)("DURATION"),
                                DURATION_TYPE = Accum.Field(Of Integer)("DURATION_TYPE")
                    Into AccumGroup = Group
                    Select ACCUM_NAME, OCC_FROM_DATE, OCC_TO_DATE, ACTIVE_SW, MANUAL_SW, PREVENTIVE_SW, BATCH_SW, DIRECTION, DURATION, DURATION_TYPE, MAX_PUBLISH_BATCH_NBR = AccumGroup.Max(Function(Accum) Accum!MAX_PUBLISH_BATCH_NBR)
                )

            For Each R In QueryAccum.AsEnumerable
                Condition = New Condition

                Condition.AccumulatorName = R.ACCUM_NAME.Trim

                Condition.Active = CInt(R.ACTIVE_SW)
                Condition.Manual = CInt(R.MANUAL_SW)
                Condition.Batch = CInt(R.BATCH_SW)

                Condition.Direction = CType(R.DIRECTION, DateDirection)
                Condition.Duration = R.DURATION
                Condition.DurationType = CType(R.DURATION_TYPE, DateType)

                Condition.AccumulatorYear = CDate(dateOfService).Year

                Condition.PublishBatchNbr = Convert.ToInt32(R.MAX_PUBLISH_BATCH_NBR)

                Condition.PlanType = Nothing

                Condition.AccumulatorStartDate = R.OCC_FROM_DATE

                Condition.AccumulatorEndDate = R.OCC_TO_DATE

                Conditions.Add(Condition)
            Next

            Return Conditions

        Catch ex As Exception

            Throw New Exception("Cannot get conditions: " & ex.ToString, ex)

        End Try

    End Function

    Public Shared Function GetDistinctAccumulatorsForDOS(dateOfService As Date) As DataTable

        Dim AccumulatorsForDOSDT As DataTable
        Dim DR As DataRow

        Try

            AccumulatorsForDOSDT = _AccumulatorsOfAllPlansDS.Tables(0).Clone

            Dim QueryAccum =
                (
                    From Accum As DataRow In PlanAccumulators.AsEnumerable
                    Where dateOfService >= Accum.Field(Of Date)("OCC_FROM_DATE") AndAlso dateOfService <= Accum.Field(Of Date)("OCC_TO_DATE")
                    Group By ACCUM_NAME = Accum.Field(Of String)("ACCUM_NAME"),
                                OCC_FROM_DATE = Accum.Field(Of Date)("OCC_FROM_DATE"),
                                OCC_TO_DATE = Accum.Field(Of Date)("OCC_TO_DATE"),
                                ACTIVE_SW = Accum.Field(Of Decimal)("ACTIVE_SW"),
                                MANUAL_SW = Accum.Field(Of Decimal)("MANUAL_SW"),
                                PREVENTIVE_SW = Accum.Field(Of Decimal)("PREVENTIVE_SW"),
                                BATCH_SW = Accum.Field(Of Decimal)("BATCH_SW"),
                                DIRECTION = Accum.Field(Of Decimal)("DIRECTION"),
                                DURATION = Accum.Field(Of Integer)("DURATION"),
                                DURATION_TYPE = Accum.Field(Of Integer)("DURATION_TYPE")
                    Into AccumGroup = Group, MAX_PUBLISH_BATCH_NBR = Max(Function() Accum.Field(Of Integer)("MAX_PUBLISH_BATCH_NBR"))
                )

            AccumulatorsForDOSDT.BeginLoadData()

            For Each R In QueryAccum.AsEnumerable
                DR = AccumulatorsForDOSDT.NewRow

                DR("ACCUM_NAME") = R.ACCUM_NAME
                DR("PUBLISH_BATCH_NBR") = R.MAX_PUBLISH_BATCH_NBR

                DR("ACTIVE_SW") = R.ACTIVE_SW
                DR("MANUAL_SW") = R.MANUAL_SW
                DR("BATCH_SW") = R.BATCH_SW

                DR("DIRECTION") = R.DIRECTION
                DR("DURATION") = R.DURATION
                DR("DURATION_TYPE") = R.DURATION_TYPE

                DR("PLAN_TYPE") = DBNull.Value

                DR("OCC_FROM_DATE") = R.OCC_FROM_DATE

                DR("OCC_TO_DATE") = R.OCC_TO_DATE

                AccumulatorsForDOSDT.ImportRow(DR)
            Next
            AccumulatorsForDOSDT.EndLoadData()

            Return AccumulatorsForDOSDT

        Catch ex As Exception

            Throw New Exception("Cannot get conditions: " & ex.ToString, ex)

        Finally
        End Try
    End Function

    Public Shared Function GetPlanByAccumulatorConditionsForDOS(dateOfService As Date) As Conditions

        ' -----------------------------------------------------------------------------
        ' <summary>
        ' This routine gets all the accumulators currently available for the date of service
        ' Note: An accumulator is considered distinct if any of it's columns vary between an accumulator of the same name !!!
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	2/5/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim Condition As Condition
        Dim Conditions As New Conditions

        Try

            Dim QueryAccum =
                From Accum In PlanAccumulators.AsEnumerable()
                Where dateOfService >= Accum.Field(Of Date)("OCC_FROM_DATE") AndAlso dateOfService <= Accum.Field(Of Date)("OCC_TO_DATE")
                Group Accum By Accum!PLAN_TYPE, Accum!ACCUM_NAME, Accum!OCC_FROM_DATE, Accum!OCC_TO_DATE, Accum!ACTIVE_SW, Accum!MANUAL_SW, Accum!PREVENTIVE_SW, Accum!BATCH_SW, Accum!DIRECTION, Accum!DURATION, Accum!DURATION_TYPE Into AccumGroup = Group
                Select ACCUM_NAME, PLAN_TYPE, OCC_FROM_DATE, OCC_TO_DATE, ACTIVE_SW, MANUAL_SW, PREVENTIVE_SW, BATCH_SW, DIRECTION, DURATION, DURATION_TYPE, MAX_PUBLISH_BATCH_NBR = AccumGroup.Max(Function(Accum) Accum!MAX_PUBLISH_BATCH_NBR)

            For Each R In QueryAccum.AsEnumerable
                Condition = New Condition
                If R.ACCUM_NAME IsNot DBNull.Value Then
                    Condition.AccumulatorName = R.ACCUM_NAME.ToString
                End If
                If R.ACTIVE_SW IsNot DBNull.Value Then
                    Condition.Active = CInt(R.ACTIVE_SW)
                End If
                If R.MANUAL_SW IsNot DBNull.Value Then
                    Condition.Manual = CInt(R.MANUAL_SW)
                End If
                If R.BATCH_SW IsNot DBNull.Value Then
                    Condition.Batch = CInt(R.BATCH_SW)
                End If
                If R.ACCUM_NAME IsNot DBNull.Value Then
                    Condition.AccumulatorName = R.ACCUM_NAME.ToString
                End If
                If R.DIRECTION IsNot DBNull.Value Then
                    Condition.Direction = CType(Convert.ToInt32(R.DIRECTION.ToString), DateDirection)
                End If
                If R.DURATION IsNot DBNull.Value Then
                    Condition.Duration = Convert.ToInt32(R.DURATION)
                End If
                If R.DURATION_TYPE IsNot DBNull.Value Then
                    Condition.DurationType = CType(Convert.ToInt32(R.DURATION_TYPE), DateType)
                End If

                Condition.AccumulatorYear = CDate(dateOfService).Year

                If R.MAX_PUBLISH_BATCH_NBR IsNot DBNull.Value Then
                    Condition.PublishBatchNbr = Convert.ToInt32(R.MAX_PUBLISH_BATCH_NBR)
                End If

                If R.PLAN_TYPE IsNot DBNull.Value Then
                    Condition.PlanType = R.PLAN_TYPE.ToString
                End If

                If R.OCC_FROM_DATE IsNot DBNull.Value Then
                    Condition.AccumulatorStartDate = CDate(R.OCC_FROM_DATE)
                End If

                If R.OCC_TO_DATE IsNot DBNull.Value Then
                    Condition.AccumulatorEndDate = CDate(R.OCC_TO_DATE)
                End If

                Conditions.Add(Condition)
            Next

            Return Conditions

        Catch ex As Exception

            Throw New Exception("Cannot get conditions: " & ex.ToString, ex)

        Finally
        End Try

    End Function

    Public Shared Function GetPlanConditionsForDOS(planType As String, dateOfService As Date) As Conditions
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' This routine gets all the accumulators currently available for the date of service
        ' Note: An accumulator is considered distinct if any of it's columns vary between an accumulator of the same name !!!
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	2/5/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim TheCondition As Condition
        Dim TheConditions As New Conditions

        Try

            Dim QueryAccum =
                From Accum In PlanAccumulators.AsEnumerable()
                Where dateOfService >= Accum.Field(Of Date)("OCC_FROM_DATE") AndAlso dateOfService <= Accum.Field(Of Date)("OCC_TO_DATE") _
                AndAlso Accum.Field(Of String)("PLAN_TYPE").Trim = planType
                Group Accum By Accum!PLAN_TYPE, Accum!ACCUM_NAME, Accum!OCC_FROM_DATE, Accum!OCC_TO_DATE, Accum!ACTIVE_SW, Accum!MANUAL_SW, Accum!PREVENTIVE_SW, Accum!BATCH_SW, Accum!DIRECTION, Accum!DURATION, Accum!DURATION_TYPE Into AccumGroup = Group
                Select ACCUM_NAME, PLAN_TYPE, OCC_FROM_DATE, OCC_TO_DATE, ACTIVE_SW, MANUAL_SW, PREVENTIVE_SW, BATCH_SW, DIRECTION, DURATION, DURATION_TYPE, MAX_PUBLISH_BATCH_NBR = AccumGroup.Max(Function(Accum) Accum!MAX_PUBLISH_BATCH_NBR)

            For Each R In QueryAccum.AsEnumerable
                TheCondition = New Condition
                If R.ACCUM_NAME IsNot DBNull.Value Then
                    TheCondition.AccumulatorName = R.ACCUM_NAME.ToString
                End If
                If R.ACTIVE_SW IsNot DBNull.Value Then
                    TheCondition.Active = CInt(R.ACTIVE_SW)
                End If
                If R.MANUAL_SW IsNot DBNull.Value Then
                    TheCondition.Manual = CInt(R.MANUAL_SW)
                End If
                If R.BATCH_SW IsNot DBNull.Value Then
                    TheCondition.Batch = CInt(R.BATCH_SW)
                End If
                If R.ACCUM_NAME IsNot DBNull.Value Then
                    TheCondition.AccumulatorName = R.ACCUM_NAME.ToString
                End If
                If R.DIRECTION IsNot DBNull.Value Then
                    TheCondition.Direction = CType(Convert.ToInt32(R.DIRECTION.ToString), DateDirection)
                End If
                If R.DURATION IsNot DBNull.Value Then
                    TheCondition.Duration = Convert.ToInt32(R.DURATION)
                End If
                If R.DURATION_TYPE IsNot DBNull.Value Then
                    TheCondition.DurationType = CType(Convert.ToInt32(R.DURATION_TYPE), DateType)
                End If

                TheCondition.AccumulatorYear = CDate(dateOfService).Year

                If R.MAX_PUBLISH_BATCH_NBR IsNot DBNull.Value Then
                    TheCondition.PublishBatchNbr = Convert.ToInt32(R.MAX_PUBLISH_BATCH_NBR)
                End If

                If R.PLAN_TYPE IsNot DBNull.Value Then
                    TheCondition.PlanType = R.PLAN_TYPE.ToString
                End If

                If R.OCC_FROM_DATE IsNot DBNull.Value Then
                    TheCondition.AccumulatorStartDate = CDate(R.OCC_FROM_DATE)
                End If

                If R.OCC_TO_DATE IsNot DBNull.Value Then
                    TheCondition.AccumulatorEndDate = CDate(R.OCC_TO_DATE)
                End If

                TheConditions.Add(TheCondition)
            Next

            Return TheConditions

        Catch ex As Exception

            Throw New Exception("Cannot get conditions: " & ex.ToString, ex)

        Finally
        End Try
    End Function

    Public Shared Function GetConditionsForFamily(familyId As Integer, dateOfQuery As Date) As Conditions
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="familyId"></param>
        ' <param name="dateOfQuery"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/31/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim SQLCommand As String = "FDBMD.RETRIEVE_DISTINCT_CONDITIONS_FOR_FAMILY_BY_DATE"
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim DBDataReader As DbDataReader

        Dim MatchFound As Boolean = False
        Dim GroupNumbers As New ArrayList

        Dim TheCondition As Condition
        Dim TheConditions As New Conditions

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyId)
            DB.AddInParameter(DBCommandWrapper, "@ELIGIBILITY_PERIOD", DbType.Date, Format(dateOfQuery, "yyyy-MM-dd"))
            DBCommandWrapper.CommandTimeout = 1200

            DBDataReader = CType(DB.ExecuteReader(DBCommandWrapper), Common.DbDataReader)

            While DBDataReader.Read
                TheCondition = New Condition
                If DBDataReader("ACCUM_NAME") IsNot DBNull.Value Then
                    TheCondition.AccumulatorName = DBDataReader("ACCUM_NAME").ToString
                End If
                If DBDataReader("DIRECTION") IsNot DBNull.Value Then
                    TheCondition.Direction = CType(Convert.ToInt32(DBDataReader("DIRECTION").ToString), DateDirection)
                End If
                If DBDataReader("DURATION") IsNot DBNull.Value Then
                    TheCondition.Duration = Convert.ToInt32(DBDataReader("DURATION"))
                End If
                If DBDataReader("DURATION_TYPE") IsNot DBNull.Value Then
                    TheCondition.DurationType = CType(Convert.ToInt32(DBDataReader("DURATION_TYPE")), DateType)
                End If
                If DBDataReader("OPERAND") IsNot DBNull.Value Then
                    TheCondition.Operand = CDec(DBDataReader("OPERAND"))
                End If

                If DBDataReader.FieldCount > 5 Then
                    If DBDataReader("CHECK_FOR_HEADROOM") IsNot DBNull.Value Then
                        TheCondition.UseInHeadroomCheck = Convert.ToBoolean(DBDataReader("CHECK_FOR_HEADROOM"))
                    End If
                    If DBDataReader("REPRICE_EXCEEDED_SW") IsNot DBNull.Value Then
                        TheCondition.RepriceIfExceeded = Convert.ToBoolean(DBDataReader("REPRICE_EXCEEDED_SW"))
                    End If
                End If
                TheConditions.Add(TheCondition)
            End While

            Return TheConditions

        Catch ex As Exception

            Throw New Exception("Cannot get conditions: " & ex.ToString, ex)

        Finally
            DBDataReader.Close()
            DBDataReader = Nothing
        End Try
    End Function

    'Public Shared Function GetConditionsForPlan(ByVal planType As String, ByVal eligibilityPeriod As Date) As Conditions
    '    ' -----------------------------------------------------------------------------
    '    ' <summary>
    '    '
    '    ' </summary>
    '    ' <param name="familyId"></param>
    '    ' <param name="dateOfQuery"></param>
    '    ' <returns></returns>
    '    ' <remarks>
    '    ' </remarks>
    '    ' <history>
    '    ' 	[paulw]	10/31/2006	Created
    '    ' </history>
    '    ' -----------------------------------------------------------------------------

    '    Dim SQL As String = "FDBMD.RETRIEVE_DISTINCT_CONDITIONS_FOR_PLAN_BY_DATE"
    '    Dim DB As Database
    '    Dim DBCommandWrapper As DbCommand
    '    Dim DBDataReader As DbDataReader

    '    Dim MatchFound As Boolean = False
    '    Dim GroupNumbers As New ArrayList

    '    Dim Condition As Condition
    '    Dim Conditions As New Conditions

    '    Try
    '        DB = CMSDALCommon.CreateDatabase()

    '        DBCommandWrapper = DB.GetStoredProcCommand(SQL)
    '        DB.AddInParameter(DBCommandWrapper, "@PLAN_TYPE", DbType.String, planType)
    '        DB.AddInParameter(DBCommandWrapper, "@ELIGIBILITY_PERIOD", DbType.Date, Format(eligibilityPeriod, "yyyy-MM-dd"))
    '        DBCommandWrapper.CommandTimeout = 1200

    '        DBDataReader = CType(DB.ExecuteReader(DBCommandWrapper), Common.DbDataReader)

    '        While DBDataReader.Read
    '            Condition = New Condition
    '            If DBDataReader("ACCUM_NAME") IsNot DBNull.Value Then
    '                Condition.AccumulatorName = DBDataReader("ACCUM_NAME").ToString
    '            End If
    '            If DBDataReader("DIRECTION") IsNot DBNull.Value Then
    '                Condition.Direction = CType(Convert.ToInt32(DBDataReader("DIRECTION").ToString), DateDirection)
    '            End If
    '            If DBDataReader("DURATION") IsNot DBNull.Value Then
    '                Condition.Duration = Convert.ToInt32(DBDataReader("DURATION"))
    '            End If
    '            If DBDataReader("DURATION_TYPE") IsNot DBNull.Value Then
    '                Condition.DurationType = CType(Convert.ToInt32(DBDataReader("DURATION_TYPE")), DateTypes)
    '            End If
    '            If DBDataReader("OPERAND") IsNot DBNull.Value Then
    '                Condition.Operand = CDec(DBDataReader("OPERAND"))
    '            End If
    '            If DBDataReader.FieldCount > 5 Then
    '                If DBDataReader("CHECK_FOR_HEADROOM") IsNot DBNull.Value Then
    '                    Condition.UseInHeadroomCheck = CBool(DBDataReader("CHECK_FOR_HEADROOM"))
    '                End If
    '                If DBDataReader("REPRICE_EXCEEDED_SW") IsNot DBNull.Value Then
    '                    Condition.RepriceIfExceeded = CBool(DBDataReader("REPRICE_EXCEEDED_SW"))
    '                End If
    '            End If
    '            Conditions.Add(Condition)
    '        End While
    '        Return Conditions
    '    Catch ex As Exception
    '        Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '        If (Rethrow) Then
    '            Throw New Exception("Cannot get conditions: " & ex.ToString, ex)
    '        End If
    '    Finally
    '        DBDataReader.Close()
    '        DBDataReader = Nothing
    '    End Try
    'End Function

    Public Shared Function GetConditionsForClaim(ruleSetID As Integer) As Conditions
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="RULE_SET_ID"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/31/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim SQLCommand As String = "FDBMD.RETRIEVE_DISTINCT_CONDITIONS_FOR_CLAIM"
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim MatchFound As Boolean = False
        Dim GroupNumbers As New ArrayList
        Dim DBDataReader As DbDataReader

        Dim TheCondition As Condition
        Dim TheConditions As New Conditions

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)

            DB.AddInParameter(DBCommandWrapper, "@RULE_SET_ID", DbType.Int32, ruleSetID)
            DBCommandWrapper.CommandTimeout = 1200

            DBDataReader = CType(DB.ExecuteReader(DBCommandWrapper), Common.DbDataReader)

            While DBDataReader.Read
                TheCondition = New Condition
                If DBDataReader("ACCUM_NAME") IsNot DBNull.Value Then
                    TheCondition.AccumulatorName = DBDataReader("ACCUM_NAME").ToString
                End If
                If DBDataReader("ACTIVE_SW") IsNot DBNull.Value Then
                    TheCondition.Active = CInt(DBDataReader("ACTIVE_SW"))
                End If
                If DBDataReader("MANUAL_SW") IsNot DBNull.Value Then
                    TheCondition.Manual = CInt(DBDataReader("MANUAL_SW"))
                End If
                If DBDataReader("BATCH_SW") IsNot DBNull.Value Then
                    TheCondition.Batch = CInt(DBDataReader("BATCH_SW"))
                End If
                If DBDataReader("ACCUM_NAME") IsNot DBNull.Value Then
                    TheCondition.AccumulatorName = DBDataReader("ACCUM_NAME").ToString
                End If
                If DBDataReader("DIRECTION") IsNot DBNull.Value Then
                    TheCondition.Direction = CType(Convert.ToInt32(DBDataReader("DIRECTION").ToString), DateDirection)
                End If
                If DBDataReader("DURATION") IsNot DBNull.Value Then
                    TheCondition.Duration = Convert.ToInt32(DBDataReader("DURATION"))
                End If
                If DBDataReader("DURATION_TYPE") IsNot DBNull.Value Then
                    TheCondition.DurationType = CType(Convert.ToInt32(DBDataReader("DURATION_TYPE")), DateType)
                End If

                If DBDataReader.FieldCount > 7 Then
                    If DBDataReader("OPERAND") IsNot DBNull.Value Then
                        TheCondition.Operand = CDec(DBDataReader("OPERAND"))
                    End If
                    If DBDataReader.FieldCount > 7 Then
                        If DBDataReader("CHECK_FOR_HEADROOM") IsNot DBNull.Value Then
                            TheCondition.UseInHeadroomCheck = Convert.ToBoolean(DBDataReader("CHECK_FOR_HEADROOM"))
                        End If

                        If DBDataReader("REPRICE_EXCEEDED_SW") IsNot DBNull.Value Then
                            TheCondition.RepriceIfExceeded = Convert.ToBoolean(DBDataReader("REPRICE_EXCEEDED_SW"))
                        End If
                    End If
                End If
                TheConditions.Add(TheCondition)
            End While

            Return TheConditions

        Catch ex As Exception

            Throw New Exception("Cannot get conditions: " & ex.ToString, ex)

        Finally

            DBDataReader.Close()
            DBDataReader = Nothing
        End Try

    End Function

    Public Shared Function GetActiveProcedureGroupingNumber(ByVal planType As String, ByVal procedureCode As String) As Integer
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="planType"></param>
        ' <param name="procedureCode"></param>
        ' <param name="billType"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/26/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim SQLCommand As String = "FDBMD.RETRIEVE_PROCEDURE_GROUP_NUMBER"
        Dim DBCommandWrapper As DbCommand
        Dim DB As Database

        Dim MatchFound As Boolean = False
        Dim GroupNumbers As New ArrayList

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)
            DB.AddInParameter(DBCommandWrapper, "@PLAN_TYPE", DbType.String, planType)
            DB.AddInParameter(DBCommandWrapper, "@PROC_CODE", DbType.String, procedureCode)
            DBCommandWrapper.CommandTimeout = 1200

            Return Convert.ToInt32(DB.ExecuteScalar(DBCommandWrapper))

        Catch ex As Exception

            Throw New StagingDataException("Cannot Determine if ruleset exists", ex)

        Finally
        End Try
    End Function

    Public Shared Function GetActiveProcedureGroupingNumbers() As DataTable

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim MatchFound As Boolean = False
        Dim GroupNumbers As New ArrayList
        Dim DS As DataSet

        Dim SQLCommand As String = "FDBMD.RETRIEVE_PROCEDURE_GROUP_NUMBERS"

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)

            DBCommandWrapper.CommandTimeout = 1200

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            Return DS.Tables(0)

        Catch ex As Exception

            Throw New StagingDataException("Cannot Determine if ruleset exists", ex)

        Finally
        End Try
    End Function

    Public Shared Function GetActiveProcedure(ByVal procedureID As Integer?) As ProcedureActive
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

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCommand As String = "FDBMD.RETRIEVE_PROCEDURE_ACTIVE_BY_PROC_ID"

        Dim ProcedureCollection As Procedures

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)
            DB.AddInParameter(DBCommandWrapper, "@PROC_ID", DbType.Int32, procedureID)

            DBCommandWrapper.CommandTimeout = 0

            Using DBReader As RefCountingDataReader = CType(DB.ExecuteReader(DBCommandWrapper), RefCountingDataReader)

                ProcedureCollection = GetProcedureCollectionFromDataReader(CType(DBReader.InnerReader, DbDataReader), False)
                If ProcedureCollection.Count > 0 Then
                    Return DirectCast(ProcedureCollection(0), ProcedureActive)
                End If

            End Using

            Return Nothing

        Catch ex As Exception

            Throw New StagingDataException("Cannot return procedure", ex)

        Finally

        End Try

    End Function

    Friend Shared Function GetRuleSetPublishDateForPlanTypeSeqNbr(planType As String, sequenceNumber As Integer) As Date

        Dim result As Date

        Dim DB As Database
        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand


        Try
            DB = CMSDALCommon.CreateDatabase()

            SQLCommand =
                <SQL>
                    SELECT A.PUBLISH_DATE
                    FROM FDBMD.PLANS_ACTIVE A
                    WHERE A.PLAN_TYPE = '<%= planType %>'
                      AND A.SEQ_NBR = <%= sequenceNumber %>
                </SQL>.Value

            DBCommandWrapper = DB.GetSqlStringCommand(SQLCommand)
            DBCommandWrapper.CommandTimeout = 10

            result = CDate(DB.ExecuteScalar(DBCommandWrapper))

            Return result

        Catch ex As Exception

            Throw New StagingDataException("Cannot Get RuleSetID", ex)

        End Try

    End Function

    Friend Shared Function GetRuleSetPublishedInformationForPlanSequenceNumber(planType As String, ByVal sequenceNumber As Integer) As DataTable

        Dim DB As Database
        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand
        Dim DS As DataSet
        Dim result As DataTable = Nothing

        Try

            DB = CMSDALCommon.CreateDatabase()

            SQLCommand =
            <SQL>
SELECT
    A.PLAN_TYPE,
    A.SEQ_NBR,
    A.PUBLISH_BATCH_NBR,
    A.OCC_FROM_DATE,
    A.OCC_TO_DATE,
    A.PUBLISH_DATE,
    A."USERID" As PUBLISHED_BY
FROM
    FDBMD.PLANS_ACTIVE_HISTORY A
WHERE
    A.PLAN_TYPE = '<%= planType %>' AND
    A.SEQ_NBR = <%= sequenceNumber %>
            </SQL>.Value

            DBCommandWrapper = DB.GetSqlStringCommand(SQLCommand)
            DBCommandWrapper.CommandTimeout = 10

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            If DS.Tables.Count > 0 Then
                DS.Tables(0).TableName = "RuleSetPublishedInformation"

                result = DS.Tables(0)
            End If

        Catch ex As Exception

            Throw

        End Try

        Return result

    End Function

    Friend Shared Function GetRuleSetEffectiveDatesForPlanRuleSet(planType As String, ruleSetID As Integer, ruleSetType As Integer?) As DataTable

        Dim DB As Database
        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand
        Dim DS As DataSet
        Dim result As DataTable = Nothing

        Try

            DB = CMSDALCommon.CreateDatabase()

            SQLCommand =
                <SQL>
SELECT
    A.PROC_ID,
    B.RULE_SET_ID,
    B.RULE_SET_TYPE,
    C.RULE_SET_TYPE AS RULE_SET_TYPE_DESCRIPTION,
    D.RULE_SET_NAME,
    UPPER(D.RULE_SET_NAME) AS RULE_SET_NAME_UCASE,
    A.OCC_FROM_DATE,
    A.OCC_TO_DATE,
    A.PLAN_TYPE,
    A.PROC_CODE,
    A.SEQ_NBR,
    A.PUBLISH_BATCH_NBR,
    A.PROVIDER,
    A.MODIFIER,
    A.PLACE_OF_SERV,
    A.BILL_TYPE,
    A.DIAGNOSIS,
    A.CREATE_USERID,
    A.GENDER,
    A.MONTHS_MIN,
    A.MONTHS_MAX
FROM
    FDBMD.PROCEDURE_ACTIVE A
    INNER JOIN FDBMD.PROC_TO_RULE_SET_ACTIVE B
        ON B.PROC_ID = A.PROC_ID
    INNER JOIN FDBMD.RULE_SET_TYPES C
        ON C.RULE_SET_TYPE_ID = B.RULE_SET_TYPE
    INNER JOIN FDBMD.RULE_SET_ACTIVE D
        ON D.RULE_SET_ID = B.RULE_SET_ID
WHERE
    A.PLAN_TYPE = '<%= planType %>' AND
    B.RULE_SET_ID = <%= ruleSetID %> AND
    B.RULE_SET_TYPE = <%= ruleSetType %>
-- ORDER BY A.PUBLISH_BATCH_NBR DESC, A.SEQ_NBR DESC, A.OCC_FROM_DATE, A.OCC_TO_DATE, A.PROC_ID
FETCH FIRST 1 ROWS ONLY
        </SQL>.Value

            DBCommandWrapper = DB.GetSqlStringCommand(SQLCommand)
            DBCommandWrapper.CommandTimeout = 10

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            If DS.Tables.Count > 0 Then
                DS.Tables(0).TableName = "RuleSetEffectiveDates"

                result = DS.Tables(0)
            End If

        Catch ex As Exception

            Throw

        End Try

        Return result

    End Function

    Friend Shared Function GetProcedureEffectiveDatesForPlanProcedureID(planType As String, procedureID As Integer, ruleSetType As Integer?) As DataTable

        Dim DB As Database
        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand
        Dim DS As DataSet
        Dim result As DataTable = Nothing

        Try

            DB = CMSDALCommon.CreateDatabase()

            SQLCommand =
                <SQL>
SELECT
    A.PROC_ID,
    B.RULE_SET_ID,
    B.RULE_SET_TYPE,
    C.RULE_SET_TYPE AS RULE_SET_TYPE_DESCRIPTION,
    D.RULE_SET_NAME,
    UPPER(D.RULE_SET_NAME) AS RULE_SET_NAME_UCASE,
    A.OCC_FROM_DATE,
    A.OCC_TO_DATE,
    A.PLAN_TYPE,
    A.PROC_CODE,
    A.SEQ_NBR,
    A.PUBLISH_BATCH_NBR,
    A.PROVIDER,
    A.MODIFIER,
    A.PLACE_OF_SERV,
    A.BILL_TYPE,
    A.DIAGNOSIS,
    A.CREATE_USERID,
    A.GENDER,
    A.MONTHS_MIN,
    A.MONTHS_MAX
FROM
    FDBMD.PROCEDURE_ACTIVE A
    INNER JOIN FDBMD.PROC_TO_RULE_SET_ACTIVE B
        ON B.PROC_ID = A.PROC_ID
    INNER JOIN FDBMD.RULE_SET_TYPES C
        ON C.RULE_SET_TYPE_ID = B.RULE_SET_TYPE
    INNER JOIN FDBMD.RULE_SET_ACTIVE D
        ON D.RULE_SET_ID = B.RULE_SET_ID
WHERE
    A.PLAN_TYPE = '<%= planType %>' AND
    A.PROC_ID = <%= procedureID %>
                    <%= GetOptionalRuleSetTypePredicate(ruleSetType) %>
-- ORDER BY B.RULE_SET_TYPE, A.PUBLISH_BATCH_NBR DESC, A.SEQ_NBR DESC, A.OCC_FROM_DATE, A.OCC_TO_DATE
FETCH FIRST 1 ROWS ONLY
        </SQL>.Value

            DBCommandWrapper = DB.GetSqlStringCommand(SQLCommand)
            DBCommandWrapper.CommandTimeout = 10

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            If DS.Tables.Count > 0 Then
                DS.Tables(0).TableName = "RuleSetEffectiveDates"

                result = DS.Tables(0)
            End If

        Catch ex As Exception

            Throw

        End Try

        Return result

    End Function

    Private Shared Function GetOptionalRuleSetTypePredicate(ruleSetType As Integer?) As String
        Dim result As String = String.Empty

        If ruleSetType IsNot Nothing Then
            result = String.Format(" AND B.RULE_SET_TYPE = {0}", ruleSetType)
        End If

        Return result

    End Function
    Public Shared Function GetRuleSetIDForProcedureID(ByVal procedureID As Integer?, ByVal ruleSetType As Integer?) As Integer

        Dim DB As Database
        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand

        Try

            DB = CMSDALCommon.CreateDatabase()

            SQLCommand =
                <SQL>
                    SELECT A.RULE_SET_ID
                    FROM FDBMD.PROC_TO_RULE_SET_ACTIVE A
                    WHERE A.PROC_ID = <%= procedureID %>
                      AND A.RULE_SET_TYPE = <%= ruleSetType %>
                </SQL>.Value

            DBCommandWrapper = DB.GetSqlStringCommand(SQLCommand)
            DBCommandWrapper.CommandTimeout = 10

            Return Convert.ToInt32(DB.ExecuteScalar(DBCommandWrapper))

        Catch ex As Exception

            Throw New StagingDataException("Cannot Get RuleSetID", ex)

        End Try

    End Function

    Public Shared Function GetRuleSetNameForProcedureID(ByVal procedureID As Integer?, Optional ByVal ruleSetType As Integer? = Nothing) As String
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

        Dim SQLCommand As String = "FDBMD.RETRIEVE_RULE_SET_NAME_BY_PROC_ID"
        Dim DBCommandWrapper As DbCommand
        Dim DB As Database

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)

            DB.AddInParameter(DBCommandWrapper, "@PROC_ID", DbType.Int32, procedureID)

            Using DBReader As RefCountingDataReader = CType(DB.ExecuteReader(DBCommandWrapper), RefCountingDataReader)

                While (CType(DBReader.InnerReader, DbDataReader).Read)
                    If CType(DBReader.InnerReader, DbDataReader)("RULE_SET_NAME") IsNot System.DBNull.Value Then
                        If ruleSetType Is Nothing OrElse CType(DBReader.InnerReader, DbDataReader)("RULE_SET_TYPE").ToString = ruleSetType.ToString Then
                            Return CType(DBReader.InnerReader, DbDataReader)("RULE_SET_NAME").ToString
                        End If
                    End If

                End While

            End Using

            Return ""

        Catch ex As Exception

            Throw New StagingDataException("Cannot return rule set name", ex)

        Finally
        End Try
    End Function
    Public Shared Function GetRuleSetNameForRulesetID(ByVal rulesetID As Integer) As String
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="procId"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	11/12/2018	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        'Return String.Empty

        Dim SQLCommand As String = "FDBMD.RETRIEVE_RULE_SET_NAME_BY_RULE_SET_ID"
        Dim DBCommandWrapper As DbCommand
        Dim DB As Database

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)

            DB.AddInParameter(DBCommandWrapper, "@vRULE_SET_ID", DbType.Int32, rulesetID)


            Return Convert.ToString(DB.ExecuteScalar(DBCommandWrapper))

            'Can never get here???

            Using DBReader As RefCountingDataReader = CType(DB.ExecuteReader(DBCommandWrapper), RefCountingDataReader)

                While (CType(DBReader.InnerReader, DbDataReader).Read)
                    If CType(DBReader.InnerReader, DbDataReader)("RULE_SET_NAME") IsNot System.DBNull.Value Then
                        Return CType(DBReader.InnerReader, DbDataReader)("RULE_SET_NAME").ToString
                    End If
                End While

            End Using

            Return ""

        Catch ex As Exception

            Throw New StagingDataException("Cannot return rule set name", ex)

        Finally
        End Try
    End Function
    Public Shared Function GetWildCardProcedures() As Procedures

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCommand As String = "FDBMD.RETRIEVE_WILDCARD_PROCEDURES"

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)
            DBCommandWrapper.CommandTimeout = 30000

            Using DBReader As RefCountingDataReader = CType(DB.ExecuteReader(DBCommandWrapper), RefCountingDataReader)
                Return GetProcedureCollectionFromDataReader(CType(DBReader.InnerReader, DbDataReader), True)
            End Using

        Catch ex As Exception

            Throw New StagingDataException("Cannot Determine if ruleset exists", ex)

        Finally

        End Try

    End Function

    Public Shared Function GetActiveProcedure(ByVal planType As String, ByVal procedureCode As String, ByVal provider As String, ByVal relevantDate As Date, ByVal ruleSetType As Integer?) As ProcedureActive
        Dim DB As Database
        Dim SQLCommand As String = "FDBMD.RETRIEVE_PROCEDURE_ACTIVE"
        Dim DBCommandWrapper As DbCommand
        Dim Pct As Integer
        Dim ProcCollection As Procedures

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)

            DB.AddInParameter(DBCommandWrapper, "@PLAN_TYPE", DbType.String, planType)
            DB.AddInParameter(DBCommandWrapper, "@PROC_CODE", DbType.String, procedureCode)
            DB.AddInParameter(DBCommandWrapper, "@RETRIEVE_DATE", DbType.Date, relevantDate)
            DBCommandWrapper.CommandTimeout = 30000

            Using DBReader As RefCountingDataReader = CType(DB.ExecuteReader(DBCommandWrapper), RefCountingDataReader)
                ProcCollection = GetProcedureCollectionFromDataReader(CType(DBReader.InnerReader, DbDataReader), False)
            End Using

            Return CType(ProcCollection.GetBestMatch(procedureCode, "", provider, "", Nothing, "", "", "", Nothing, relevantDate, ruleSetType, Pct), ProcedureActive)

        Catch ex As Exception

            Throw New StagingDataException("Cannot Determine if ruleset exists", ex)

        Finally
        End Try

    End Function

    Public Shared Function GetNonStagedPlans(ByVal getAsLightweight As Boolean) As ArrayList
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets all plans that are not staged
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim SQLCommand As String = CMSDALCommon.GetDatabaseName & "." & "RETRIEVE_NONSTAGED_PLANS"
        Dim DBCommandWrapper As DbCommand
        Dim DB As Database
        Dim PlanActive As PlanActive
        Dim Plans As New ArrayList

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)
            DBCommandWrapper.CommandTimeout = 1200

            Using DBReader As RefCountingDataReader = CType(DB.ExecuteReader(DBCommandWrapper), RefCountingDataReader)

                'build table
                While (DBReader.InnerReader.Read())
                    PlanActive = New PlanActive(CStr(DBReader.InnerReader("PLAN_TYPE")), CStr(DBReader.InnerReader("PLAN_DESCRIPTION")), getAsLightweight)
                    Plans.Add(PlanActive)
                End While

            End Using

            Return Plans

        Catch ex As Exception

            Throw New StagingDataException("Cannot Get Non Staged Plans", ex)

        Finally

        End Try

    End Function

    Public Shared Function GetWildCardSequenceNumberSetForPlanTypeAndDateOfService(planType As String, ByVal dateOfService As Date) As DataTable

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim DS As DataSet
        Dim ResultDT As DataTable

        Dim SQLCall As String = "FDBMD.RETRIEVE_WILDCARD_SEQUENCE_SET_FOR_PLAN_AND_DATE_OF_SERVICE"

        ' A0001

        Try

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@PLAN_TYPE", DbType.String, planType)
            DB.AddInParameter(DBCommandWrapper, "@RETRIEVE_DATE", DbType.Date, dateOfService)

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            If DS IsNot Nothing AndAlso DS.Tables.Count = 1 Then
                ResultDT = DS.Tables(0)
            Else
                ResultDT = Nothing
            End If

        Catch ex As Exception

            Throw New ActiveDataException(String.Format("Cannot get SequenceNumber Set for PlanType: {0} and DateOfService: {1}", planType, dateOfService), ex)

        End Try

        Return ResultDT

    End Function

    Public Shared Function GetMaxSequenceNumber(ByVal planType As String, ByVal dateOfService As Date) As Integer

        Dim DB As Database
        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand

        Dim DBProvider As String
        Dim FormattedDateOfService As String

        ' A0001

        Try

            FormattedDateOfService = Format(dateOfService, "yyyy-MM-dd")

            DB = CMSDALCommon.CreateDatabase()

            DBProvider = DB.DbProviderFactory.ToString()

            ' If DB.ConnectionString.ToLower.Contains("ddtek") Then
            If DBProvider.ToLower.Contains("ddtek") Then
                SQLCommand = "SELECT MAX(SEQ_NBR) " &
                            " FROM	FDBMD.PLANS_ACTIVE AS PLANS_ACTIVE " &
                            " WHERE(PLANS_ACTIVE.PLAN_TYPE = '" & planType & "') " &
                            " AND '" & FormattedDateOfService & "' BETWEEN OCC_FROM_DATE AND OCC_TO_DATE;"
            Else
                SQLCommand = ("DECLARE @MAXSEQNBR AS INT; ")
                SQLCommand += ("SET @MaxSeqNbr = dbo.SELECT_SEQ_NBR('" & planType & "', '" & dateOfService & "'); ")
                SQLCommand += ("SELECT @MAXSEQNBR")
            End If

            DBCommandWrapper = DB.GetSqlStringCommand(SQLCommand)
            DBCommandWrapper.CommandTimeout = 1200

            Return Convert.ToInt32(DB.ExecuteScalar(DBCommandWrapper))

        Catch ex As Exception

            Throw New StagingDataException("Cannot Get Non Staged Plans", ex)

        Finally
        End Try

    End Function

    Public Shared Function GetProcedures(ByVal planType As String, ByVal procedureCode As String, ByVal getAsLightWeight As Boolean, ByVal dateOfService As Date) As Procedures
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets Valid procedures that match the filterparamters passed in
        ' </summary>
        ' <param name="planType"></param>
        ' <param name="filterParameters"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/6/2006	Created
        '     [paulw] 9/19/2006   Added in paramter for condition that corresponds to ACR MED-0018
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RETRIEVE_PROCEDURES_ACTIVE_WITHOUT_WILDCARD" & If(_RulesV2, "_FOR_PROC_CODE_IN_DATE_RANGE", ""))

            DB.AddInParameter(DBCommandWrapper, "@PLAN_TYPE", DbType.String, planType)
            DB.AddInParameter(DBCommandWrapper, "@PROC_CODE", DbType.String, procedureCode)
            DB.AddInParameter(DBCommandWrapper, "@RETRIEVE_DATE", DbType.Date, dateOfService)
            DBCommandWrapper.CommandTimeout = 0

            Using DBReader As RefCountingDataReader = CType(DB.ExecuteReader(DBCommandWrapper), RefCountingDataReader)

                Return GetProcedureCollectionFromDataReader(CType(DBReader.InnerReader, DbDataReader), getAsLightWeight)

            End Using

        Catch ex As Exception

            Throw New ActiveDataException("Cannot Get Active Data Because There is a Problem With The Query", ex)

        Finally

        End Try

    End Function

    Public Shared Function GetRuleset(ByVal ruleSetId As Integer) As RuleSetActive
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets a active ruleset with the given id
        ' </summary>
        ' <param name="rulesetId"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        '     [paulw] 9/27/2006   Per ACR MED-0029, added MultiLineCoPay type support
        '     [paulw]	10/3/2006	Per ACR MED-0023, added support for deny type
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim SQLCommand As String = "FDBMD.RETRIEVE_RULE_SET_ACTIVE"
        Dim DBCommandWrapper As DbCommand
        Dim DB As Database
        Dim FirstLoop As Boolean = True
        Dim RuleSet As New RuleSetActive
        Dim Rule As Rule

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)

            DB.AddInParameter(DBCommandWrapper, "@RULE_SET_ID", DbType.Int32, ruleSetId)
            DBCommandWrapper.CommandTimeout = 1200

            Using DBReader As RefCountingDataReader = CType(DB.ExecuteReader(DBCommandWrapper), RefCountingDataReader)

                While (DBReader.InnerReader.Read)

                    RuleSet.RuleSetName = DBReader.InnerReader("RULE_SET_NAME").ToString
                    RuleSet.RulesetID = Convert.ToInt32(DBReader.InnerReader("RULE_SET_ID"))

                    Select Case CType(Convert.ToInt32(DBReader.InnerReader("RULE_TYPE")), RuleTypes)
                        Case RuleTypes.CoInsurance
                            Rule = New CoInsuranceRule(GetConditionsByRule(Convert.ToInt32(DBReader.InnerReader("RULE_ID"))))
                        Case RuleTypes.CoPay
                            Rule = New CoPayRule(GetConditionsByRule(Convert.ToInt32(DBReader.InnerReader("RULE_ID"))))
                        Case RuleTypes.MultiLineCoPay
                            Rule = New MultilineCoPayRule(GetConditionsByRule(Convert.ToInt32(DBReader.InnerReader("RULE_ID"))))
                        Case RuleTypes.Deductible
                            Rule = New DeductibleRule(GetConditionsByRule(Convert.ToInt32(DBReader.InnerReader("RULE_ID"))))
                        Case RuleTypes.OutOfPocket
                            Rule = New OutOfPocketRule(GetConditionsByRule(Convert.ToInt32(DBReader.InnerReader("RULE_ID"))))
                        Case RuleTypes.Standard
                            Rule = New StandardAccumulatorRule(GetConditionsByRule(Convert.ToInt32(DBReader.InnerReader("RULE_ID"))))
                        Case RuleTypes.Accident
                            Rule = New AccidentRule(GetConditionsByRule(Convert.ToInt32(DBReader.InnerReader("RULE_ID"))))
                        Case RuleTypes.Deny
                            Rule = New DenyRule(GetConditionsByRule(Convert.ToInt32(DBReader.InnerReader("RULE_ID"))))
                        Case RuleTypes.ProviderWriteOff
                            Rule = New ProviderWriteOffRule(GetConditionsByRule(Convert.ToInt32(DBReader.InnerReader("RULE_ID"))))
                        Case RuleTypes.HRAInEligible
                            Rule = New HRAInEligibleRule(GetConditionsByRule(Convert.ToInt32(DBReader.InnerReader("RULE_ID"))))
                        Case RuleTypes.Preventative
                            Rule = New PreventativeRule(GetConditionsByRule(Convert.ToInt32(DBReader.InnerReader("RULE_ID"))))
                        Case RuleTypes.Original
                            Rule = New OriginalRule(GetConditionsByRule(Convert.ToInt32(DBReader.InnerReader("RULE_ID"))))
                    End Select

                    RuleSet.Add(Rule)

                End While
            End Using

            Return RuleSet

        Catch ex As Exception

            Throw New ActiveDataException("Cannot Get Active Ruleset", ex)

        Finally
        End Try

    End Function

    Public Shared Function GetActiveProcedures(ByVal planType As String, ByVal getAsLightWeight As Boolean) As Procedures
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets all active procedures for the given plan
        ' </summary>
        ' <param name="planType"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim SQLCommand As String = "FDBMD.RETRIEVE_PROCEDURES_ACTIVE"
        Dim DBCommandWrapper As DbCommand
        Dim DB As Database

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)

            DBCommandWrapper.CommandTimeout = 1200
            DB.AddInParameter(DBCommandWrapper, "@PLAN_TYPE", DbType.String, planType)

            Using DBReader As RefCountingDataReader = CType(DB.ExecuteReader(DBCommandWrapper), RefCountingDataReader)
                Return GetProcedureCollectionFromDataReader(CType(DBReader.InnerReader, DbDataReader), getAsLightWeight)
            End Using

        Catch ex As Exception

            Throw New ActiveDataException("Cannot Get Active Procedures", ex)

        Finally
        End Try

    End Function

    Public Shared Function GetSequenceNumbers() As DataTable

        Dim SQLCommand As String = "FDBMD.RETRIEVE_PROCEDURES_ACTIVE"
        Dim DBCommandWrapper As DbCommand
        Dim DB As Database

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)
            DBCommandWrapper.CommandTimeout = 1200

            Dim ds As DataSet = DB.ExecuteDataSet(DBCommandWrapper)

            Return ds.Tables(0)

        Catch ex As Exception

            Throw New ActiveDataException("Cannot Get Sequence Numbers", ex)

        Finally
        End Try
    End Function

    Public Shared Sub GetWildCardProceduresAndSequenceNumbers(ByRef procedureCol As Procedures, ByRef dt As DataTable)

        Dim DS As DataSet
        Dim UniqueThreadIdentifier As String
        Dim FStream As FileStream
        Dim XMLSerial As XmlSerializer
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_WILDCARD_PROCEDURES_AND_SEQ_NBRS"
        Dim XMLFilename As String

        Try

#If TRACE Then
            If CInt(_TraceParallel.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(2).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(2).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
#End If

            XMLFilename = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & SQLCall & ".xml"

            UniqueThreadIdentifier = UFCWGeneral.GetUniqueKey()

            ' A0001

            ' DS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.PUBLISH_HISTORY", "PUBLISH_DATE", "FDBMD.RETRIEVE_WILDCARD_PROCEDURES_AND_SEQ_NBRS", True, "")
            DS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.PLANS_ACTIVE_HISTORY", "PUBLISH_DATE", "FDBMD.RETRIEVE_WILDCARD_PROCEDURES_AND_SEQ_NBRS", True, "")
            '
            If DS.Tables.Count = 0 Then
                DB = CMSDALCommon.CreateDatabase()
                DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                DS = DB.ExecuteDataSet(DBCommandWrapper)
                DS.Tables(0).TableName = "WILDCARD"

                FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                XMLSerial = New XmlSerializer(DS.GetType())
                XMLSerial.Serialize(FStream, DS)

                File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
            End If

            procedureCol = GetProcedureCollectionFromDataReader(DS)

            dt = DS.Tables(1)

        Catch ex As Exception

            Throw New ActiveDataException("Cannot Get Wildcard Procedures and SequenceNumbers", ex)

        Finally
            If FStream IsNot Nothing Then FStream.Close()
            FStream = Nothing

#If TRACE Then
            If CInt(_TraceParallel.Level) > 0 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(2).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(2).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
#End If
        End Try

    End Sub

    Private Shared _CombinedWildCardSequenceDS As DataSet
    Public Shared Property CombinedWildCardSequenceDS As DataSet
        Get
            If _CombinedWildCardSequenceDS Is Nothing Then
                _CombinedWildCardSequenceDS = New DataSet
            End If
            Return _CombinedWildCardSequenceDS
        End Get
        Set
            _CombinedWildCardSequenceDS = Value
        End Set
    End Property

    Public Shared Property RefreshWildCardSequenceCacheFile As Boolean

    Public Shared Sub GetWildCardProceduresAndSequenceNumbersFor_PlanType_And_DateOfService(ByRef procedureCol As Procedures, ByRef dt As DataTable, planType As String, dateOfService As Date)

        Dim sMsg As String = String.Empty

        Dim CacheDS As DataSet
        Dim DatabaseDS As DataSet

        Dim UniqueThreadIdentifier As String
        Dim FStream As FileStream

        ' Dim SQLCall As String = "FDBMD.RETRIEVE_WILDCARD_PROCEDURES_AND_SEQ_NBRS"
        Dim SQLCall As String = "FDBMD.RETRIEVE_WILDCARD_PROCEDURES_AND_SEQ_NBRS_FOR_PLAN_AND_DATE_OF_SERVICE"
        Dim XMLFilename As String
        Dim dateOnly As Boolean = True

        Dim bXmlCacheFileExists As Boolean = False

        Dim changeTrigger_DB As ChangeTriggerInfo
        Dim changeTrigger_XML As ChangeTriggerInfo
        Dim maxSequenceNumberForPlanTypeDAL As Integer

        Dim dtDBSequenceSet As DataTable

        RefreshWildCardSequenceCacheFile = False

        Try

            If procedureCol IsNot Nothing Then
                procedureCol = Nothing
            End If
            'MH 5/3/2024
            'If dt IsNot Nothing Then
            '    dt = Nothing
            'End If

#If TRACE Then
            If CInt(_TraceParallel.Level) > 1 Then
                Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(2).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(2).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
            End If
#End If

            XMLFilename = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & SQLCall & ".xml"
            Debug.Print("XMLFilename: {0}", XMLFilename)

            bXmlCacheFileExists = System.IO.File.Exists(XMLFilename)

            UniqueThreadIdentifier = UFCWGeneral.GetUniqueKey()

            ' A0001

            ' DS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.PUBLISH_HISTORY", "PUBLISH_DATE", "FDBMD.RETRIEVE_WILDCARD_PROCEDURES_AND_SEQ_NBRS", True, "")
            CacheDS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.PLANS_ACTIVE_HISTORY", "PUBLISH_DATE", SQLCall, dateOnly, "")
            '
            If CacheDS.Tables.Count = 2 Then

                changeTrigger_XML = CacheFunctions.GetChangeTriggersForPlanType(CacheDS.Tables(1), planType)

                dtDBSequenceSet = PlanController.GetWildCardSequenceNumberSetForPlanTypeAndDateOfService(planType, dateOfService)

                If dtDBSequenceSet IsNot Nothing AndAlso dtDBSequenceSet.Rows.Count > 0 Then
                    changeTrigger_DB = CacheFunctions.GetChangeTriggersForPlanType(dtDBSequenceSet, planType)
                    maxSequenceNumberForPlanTypeDAL = PlanController.GetMaxSequenceNumberDAL(planType, dateOfService)
                End If

            End If

            If changeTrigger_XML IsNot Nothing AndAlso changeTrigger_DB IsNot Nothing Then

                If changeTrigger_XML.SequenceNumberHash = changeTrigger_DB.SequenceNumberHash Then
                    Debug.Print("XML SequenceNumberHash: {0} for PlanType: {1} and DateOfService: {2} MATCHES DB SequenceNumberHash: {3}", changeTrigger_XML.SequenceNumberHash, planType, dateOfService, maxSequenceNumberForPlanTypeDAL, changeTrigger_DB.SequenceNumberHash)
                    RefreshWildCardSequenceCacheFile = False
                Else
                    Debug.Print("XML SequenceNumberHash: {0} for PlanType: {1} and DateOfService: {2} DOES NOT MATCH DB SequenceNumberHash: {3}", changeTrigger_XML.SequenceNumberHash, planType, dateOfService, maxSequenceNumberForPlanTypeDAL, changeTrigger_DB.SequenceNumberHash)
                    RefreshWildCardSequenceCacheFile = True
                End If

                If (maxSequenceNumberForPlanTypeDAL > changeTrigger_XML.MaximumSequenceNumber) Then
                    Debug.Print("Database MaxSequenceNumber for PlanType: {0} and DateOfService: {1} is: {2} which is GREATER than the XML's ChangeTrigger.MaximumSequenceNumber: {3}", planType, dateOfService, maxSequenceNumberForPlanTypeDAL, changeTrigger_XML)
                    RefreshWildCardSequenceCacheFile = True
                End If
            End If


            If CacheDS.Tables.Count = 0 Or RefreshWildCardSequenceCacheFile = True Then

                'A0001

                DatabaseDS = GetWildCardsAndSequenceNumbersDataSetForPlanTypeAndDateOfService(planType, dateOfService, changeTrigger_DB)

            End If

            If CacheDS.Tables.Count = 2 Then
                CombinedWildCardSequenceDS = CacheDataSetFunctions.MergeWildCardSequenceDataSets(CacheDS, CombinedWildCardSequenceDS)
            End If

            If DatabaseDS IsNot Nothing AndAlso DatabaseDS.Tables.Count = 2 Then
                CombinedWildCardSequenceDS = CacheDataSetFunctions.MergeWildCardSequenceDataSets(DatabaseDS, CombinedWildCardSequenceDS)
            End If

            If CombinedWildCardSequenceDS.Tables.Count = 2 Then
                procedureCol = GetProcedureCollectionFromDataReader(CombinedWildCardSequenceDS)

                dt = CombinedWildCardSequenceDS.Tables(1)

                Call WriteWildCardsAndSequenceNumberDataSetToCacheFile(CombinedWildCardSequenceDS, XMLFilename)
            End If

        Catch ex As Exception

            sMsg = String.Format("Cannot Get Wildcard Procedures and SequenceNumbers for PlanType: {0} and DateOfService: {1}", planType, dateOfService)

            Throw New ActiveDataException(sMsg, ex)

        Finally
            If FStream IsNot Nothing Then
                FStream.Close()
            End If
            FStream = Nothing

#If TRACE Then
            If CInt(_TraceParallel.Level) > 0 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(2).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(2).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
#End If
        End Try
    End Sub

    Private Shared Function WriteWildCardsAndSequenceNumberDataSetToCacheFile(dataSetToSave As DataSet, pathToXmlCacheFile As String) As String

        Dim sMsg As String

        Dim result As String

        Dim FStream As FileStream
        Dim XMLSerial As XmlSerializer

        Try

            If System.IO.File.Exists(pathToXmlCacheFile) = True Then
                File.SetAttributes(pathToXmlCacheFile, FileAttributes.Normal)
                File.Delete(pathToXmlCacheFile)
            End If

            FStream = New FileStream(pathToXmlCacheFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

            XMLSerial = New XmlSerializer(dataSetToSave.GetType())
            XMLSerial.Serialize(FStream, dataSetToSave)

            File.SetAttributes(pathToXmlCacheFile, FileAttributes.ReadOnly)

        Catch ex As Exception

            sMsg = String.Format("Unable to save DataSet to Xml cache file: '{0}'", pathToXmlCacheFile)

            Throw New ActiveDataException(sMsg, ex)

        Finally

            If FStream IsNot Nothing Then
                FStream.Close()
            End If

        End Try

        Return result

    End Function
    Private Shared Function GetWildCardsAndSequenceNumbersDataSetForPlanTypeAndDateOfService(planType As String, dateOfService As Date, ByRef changeTrigger_DB As ChangeTriggerInfo) As DataSet

        Dim result As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_WILDCARD_PROCEDURES_AND_SEQ_NBRS_FOR_PLAN_AND_DATE_OF_SERVICE"

        Try

            If changeTrigger_DB IsNot Nothing Then
                changeTrigger_DB = Nothing
            End If

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@PLAN_TYPE", DbType.String, planType)
            DB.AddInParameter(DBCommandWrapper, "@RETRIEVE_DATE", DbType.Date, dateOfService)

            result = DB.ExecuteDataSet(DBCommandWrapper)

            If result IsNot Nothing AndAlso result.Tables.Count = 2 Then
                result.Tables(0).TableName = "WILDCARD"
                changeTrigger_DB = CacheFunctions.GetChangeTriggersForPlanType(result.Tables(1), planType)
            End If

        Catch ex As Exception

            Throw New ActiveDataException("Cannot get DataSet for Wildcard Procedures and SequenceNumbers", ex)

        End Try

        Return result

    End Function

    Private Shared Function ToAndFromDataset(ByVal tablename As String, ByVal columnname As String, ByVal spname As String) As DataSet
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' For storing latest data in disc in xml format and reading it from there for subsequent requests
        ' To reduce the overhead to system
        ' </summary>
        ' <param name="tablename">Complete tablename which includes the database name</param>
        ' <param name="columnname">Columnname</param>
        ' <param name="spname">Complete spname which includes database name</param>
        ' <returns>DataSet</returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[sbandi]	12/3/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand
        Dim DB As Database
        Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName & "." & spname & ".xml"
        Dim ResultDS As New DataSet
        Dim DS As DataSet
        Dim ColumnDate As Date
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase()

            SQLCall = "SELECT MAX( " & columnname & " )" & "AS MAXDATE FROM " & tablename & " FOR READ ONLY WITH UR"
            DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RUNIMMEDIATESELECT")
            DB.AddInParameter(DBCommandWrapper, "SQLInput", DbType.String, SQLCall)
            DBCommandWrapper.CommandTimeout = 120

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            If DS.Tables.Count > 0 Then
                ColumnDate = CType(DS.Tables(0).Rows(0)(0), Date)
            End If

            If File.Exists(XMLFilename) AndAlso (File.GetAttributes(XMLFilename) And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
                File.SetAttributes(XMLFilename, FileAttributes.Normal)
            End If

            If File.Exists(XMLFilename) AndAlso (File.GetLastWriteTime(XMLFilename).Date >= ColumnDate) Then
                Dim XMLSerializer As XmlSerializer = New XmlSerializer(ResultDS.GetType)
                ' To read the file
                Dim FileStream As FileStream = New FileStream(XMLFilename, FileMode.Open, FileAccess.Read, FileShare.Read)
                ' Create the object from the xml file
                ResultDS = CType(XMLSerializer.Deserialize(FileStream), DataSet)
                FileStream.Close()
            Else
                SQLCommand = spname
                DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)
                DBCommandWrapper.CommandTimeout = 1200
                ResultDS = DB.ExecuteDataSet(DBCommandWrapper)
                Dim sWriter As New System.IO.StreamWriter(XMLFilename)
                Dim xmlSerial As New XmlSerializer(ResultDS.GetType())
                xmlSerial.Serialize(sWriter, ResultDS)
                sWriter.Close()
            End If
            File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)

            Return ResultDS

        Catch ex As Exception

            Throw

        Finally
        End Try
    End Function

    Private Shared Function ToAndFromDataset(ByVal tableName As String, ByVal columnName As String, ByVal spName As String, ByVal relatedDate As Date) As DataSet
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' For storing latest data in disc in xml format and reading it from there for subsequent requests
        ' To reduce the overhead to system
        ' </summary>
        ' <param name="tablename">Complete tablename which includes the database name</param>
        ' <param name="columnname">Columnname</param>
        ' <param name="spname">Complete spname which includes database name</param>
        ' <returns>DataSet</returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[sbandi]	12/3/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim SQLCommand As String
        Dim DBCommandWrapper As DbCommand
        Dim DB As Database
        Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName & "." & spName & "_" & Format(relatedDate, "yyyy") & ".xml"
        Dim ResultDataSet As New DataSet
        Dim DS As DataSet
        Dim ColumnDate As Date
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase()

            SQLCall = "SELECT MAX( " & columnName & " )" & "AS MAXDATE FROM " & tableName & " FOR READ ONLY WITH UR"
            DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RUNIMMEDIATESELECT")
            DB.AddInParameter(DBCommandWrapper, "SQLInput", DbType.String, SQLCall)
            DBCommandWrapper.CommandTimeout = 120

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            If DS.Tables.Count > 0 Then
                ColumnDate = CType(DS.Tables(0).Rows(0)(0), Date)
            End If

            If File.Exists(XMLFilename) AndAlso (File.GetAttributes(XMLFilename) And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
                File.SetAttributes(XMLFilename, FileAttributes.Normal)
            End If

            If File.Exists(XMLFilename) AndAlso (File.GetLastWriteTime(XMLFilename).Date >= ColumnDate) Then
                Dim mySerializer As XmlSerializer = New XmlSerializer(ResultDataSet.GetType)
                ' To read the file
                Dim FStream As FileStream = New FileStream(XMLFilename, FileMode.Open, FileAccess.Read, FileShare.Read)
                ' Create the object from the xml file
                ResultDataSet = CType(mySerializer.Deserialize(FStream), DataSet)
                FStream.Close()
            Else
                SQLCommand = spName
                DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)

                DB.AddInParameter(DBCommandWrapper, "SQLInput", DbType.String, Format(relatedDate, "yyyy-MM-dd"))

                DBCommandWrapper.CommandTimeout = 1200
                ResultDataSet = DB.ExecuteDataSet(DBCommandWrapper)
                Dim sWriter As New System.IO.StreamWriter(XMLFilename)
                Dim xmlSerial As New XmlSerializer(ResultDataSet.GetType())
                xmlSerial.Serialize(sWriter, ResultDataSet)
                sWriter.Close()
            End If

            File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
            Return ResultDataSet

        Catch ex As Exception

            Throw

        Finally
        End Try
    End Function

#End Region

#Region "Private"
    Private Shared Function GetCompleteProcedureFromDataReader(ByRef activeProcedureDataReader As DbDataReader, ByRef procedureActive As ProcedureActive) As ProcedureActive

        Dim Condition As Condition
        Dim RuleSetActive As RuleSetActive
        Dim Rule As Rule

        Try

            If activeProcedureDataReader("RULE_SET_ID") IsNot DBNull.Value Then

                RuleSetActive = GetRuleSetActive(activeProcedureDataReader, procedureActive)

                procedureActive.RuleSets.Add(RuleSetActive)

                Rule = GetRule(activeProcedureDataReader, RuleSetActive)

                Condition = New Condition

                If activeProcedureDataReader("ACCUM_NAME") IsNot DBNull.Value Then
                    If activeProcedureDataReader("ACCUM_NAME").ToString.Trim.Length > 0 Then
                        If (activeProcedureDataReader("RULE_ACCUM_COND_ID") IsNot System.DBNull.Value) Then
                            Condition.ConditionID = Convert.ToInt32(activeProcedureDataReader("RULE_ACCUM_COND_ID").ToString())
                        Else
                            Condition.ConditionID = -1
                        End If
                        If (activeProcedureDataReader("ACCUM_NAME") IsNot System.DBNull.Value) Then
                            Condition.AccumulatorName = activeProcedureDataReader("ACCUM_NAME").ToString()
                        Else
                            Condition.AccumulatorName = ""
                        End If
                        If (activeProcedureDataReader("DIRECTION") IsNot System.DBNull.Value) Then
                            Condition.Direction = CType(activeProcedureDataReader("DIRECTION").ToString(), DateDirection)
                        Else
                            Condition.Direction = DateDirection.Forward
                        End If
                        If (activeProcedureDataReader("DURATION") IsNot System.DBNull.Value) Then
                            Condition.Duration = Convert.ToInt32(activeProcedureDataReader("DURATION").ToString())
                        Else
                            Condition.Duration = -1
                        End If
                        If (activeProcedureDataReader("DURATION_TYPE") IsNot System.DBNull.Value) Then
                            Condition.DurationType = CType(activeProcedureDataReader("DURATION_TYPE"), DateType)
                        Else
                            Condition.DurationType = DateType.Days
                        End If
                        If (activeProcedureDataReader("CHECK_FOR_HEADROOM") IsNot System.DBNull.Value) Then
                            Condition.UseInHeadroomCheck = CType(activeProcedureDataReader("CHECK_FOR_HEADROOM"), Boolean)
                        Else
                            Condition.UseInHeadroomCheck = True
                        End If
                        If (activeProcedureDataReader("ACTIVE_SW") IsNot System.DBNull.Value) Then
                            Condition.Active = CType(activeProcedureDataReader("ACTIVE_SW"), Integer)
                        Else
                            Condition.Active = 1
                        End If

                        If (activeProcedureDataReader("MANUAL_SW") IsNot System.DBNull.Value) Then
                            Condition.Manual = CType(activeProcedureDataReader("MANUAL_SW"), Integer)
                        Else
                            Condition.Manual = 0
                        End If

                        If activeProcedureDataReader.GetSchemaTable.Columns.Contains("BATCH_SW") AndAlso (activeProcedureDataReader("BATCH_SW") IsNot System.DBNull.Value) Then
                            Condition.Batch = CType(activeProcedureDataReader("BATCH_SW"), Integer)
                        Else
                            Condition.Batch = 0
                        End If

                        If (activeProcedureDataReader("REPRICE_EXCEEDED_SW") IsNot System.DBNull.Value) Then
                            Condition.RepriceIfExceeded = CType(activeProcedureDataReader("REPRICE_EXCEEDED_SW"), Boolean)
                        Else
                            Condition.RepriceIfExceeded = True
                        End If

                    End If
                End If

                If (activeProcedureDataReader("OPERAND") IsNot System.DBNull.Value) Then
                    Condition.Operand = CDec(activeProcedureDataReader("OPERAND").ToString())
                Else
                    Condition.Operand = -1
                End If

                Rule.Conditions.Add(Condition)
            End If

            Return procedureActive

        Catch ex As Exception
            Throw

        Finally
            Rule = Nothing
            RuleSetActive = Nothing
            Condition = Nothing
        End Try

    End Function

    Private Shared Function GetProcedureCollectionFromDataReader(ByRef activeProcedureDataReader As DbDataReader, ByVal getAsLightweight As Boolean) As Procedures
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Converts an DataReader to a ProcedureCollection
        ' </summary>
        ' <param name="activeProcedureDatReader"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/25/2006	Created
        '     [paulw] 7/1/2006    Altered to have better performance
        '     [paulw] 9/19/2006   Added in paramter for condition that corresponds to ACR MED-0018
        ' </history>

        If activeProcedureDataReader Is Nothing Then Throw New ArgumentNullException(NameOf(activeProcedureDataReader))

        Dim FROM_DATE As Date, TO_DATE As Date, SEQ_NBR As Integer, PROC_ID As Integer = -1 : Dim BILL_TYPE As String : Dim DIAGNOSIS As String : Dim PLACE_OF_SERV As String : Dim PLAN_TYPE As String : Dim PROC_CODE As String : Dim Provider As String : Dim MODIFIER As String = "" : Dim GENDER As String = "" : Dim MONTHSMIN As Integer = -1 : Dim MONTHSMAX As Integer = -1
        Dim PUBLISH_BATCH_NBR As Integer?
        Dim PUBLISH_DATE As Date

        Dim Procedures As Hashtable
        Dim ProcedureActive As ProcedureActive
        Dim ProcedureColl As Procedures

        Try

            Procedures = New Hashtable
            ProcedureColl = New Procedures

            While (activeProcedureDataReader.Read)

                'check if procedure is already accounted for in collection
                ProcedureActive = DirectCast(Procedures(CInt(activeProcedureDataReader("PROC_ID"))), ProcedureActive)

                If ProcedureActive Is Nothing Then
                    If activeProcedureDataReader("PROC_ID") IsNot System.DBNull.Value Then
                        PROC_ID = CInt(activeProcedureDataReader("PROC_ID"))
                    Else
                        PROC_ID = -1
                    End If
                    If activeProcedureDataReader("BILL_TYPE") IsNot System.DBNull.Value Then
                        BILL_TYPE = CStr(activeProcedureDataReader("BILL_TYPE")).Trim
                    Else
                        BILL_TYPE = ""
                    End If
                    If activeProcedureDataReader("DIAGNOSIS") IsNot System.DBNull.Value Then
                        DIAGNOSIS = CStr(activeProcedureDataReader("DIAGNOSIS")).Trim
                    Else
                        DIAGNOSIS = ""
                    End If
                    If activeProcedureDataReader("PLACE_OF_SERV") IsNot System.DBNull.Value Then
                        PLACE_OF_SERV = CStr(activeProcedureDataReader("PLACE_OF_SERV")).Trim
                    Else
                        PLACE_OF_SERV = ""
                    End If
                    If activeProcedureDataReader("PLAN_TYPE") IsNot System.DBNull.Value Then
                        PLAN_TYPE = CStr(activeProcedureDataReader("PLAN_TYPE")).Trim
                    Else
                        PLAN_TYPE = ""
                    End If
                    If activeProcedureDataReader("OCC_FROM_DATE") IsNot System.DBNull.Value Then
                        FROM_DATE = CDate(activeProcedureDataReader("OCC_FROM_DATE"))
                    Else
                        FROM_DATE = New Date(1970, 1, 1)
                    End If
                    If activeProcedureDataReader("OCC_TO_DATE") IsNot System.DBNull.Value Then
                        TO_DATE = CDate(activeProcedureDataReader("OCC_TO_DATE"))
                    Else
                        TO_DATE = New Date(1970, 1, 1)
                    End If
                    If activeProcedureDataReader("SEQ_NBR") IsNot System.DBNull.Value Then
                        SEQ_NBR = CInt(activeProcedureDataReader("SEQ_NBR"))
                    Else
                        SEQ_NBR = -1
                    End If
                    If activeProcedureDataReader("PROC_CODE") IsNot System.DBNull.Value Then
                        PROC_CODE = CStr(activeProcedureDataReader("PROC_CODE")).Trim
                    Else
                        PROC_CODE = ""
                    End If
                    If activeProcedureDataReader("PROVIDER") IsNot System.DBNull.Value Then
                        Provider = CStr(activeProcedureDataReader("PROVIDER")).Trim
                    Else
                        Provider = ""
                    End If
                    If (activeProcedureDataReader("MODIFIER") IsNot System.DBNull.Value) Then
                        MODIFIER = CStr(activeProcedureDataReader("MODIFIER"))
                    Else
                        MODIFIER = ""
                    End If
                    If (activeProcedureDataReader("GENDER") IsNot System.DBNull.Value) Then
                        GENDER = CStr(activeProcedureDataReader("GENDER"))
                    Else
                        GENDER = ""
                    End If
                    If (activeProcedureDataReader("MONTHS_MIN") IsNot System.DBNull.Value) Then
                        MONTHSMIN = CInt(activeProcedureDataReader("MONTHS_MIN"))
                    Else
                        MONTHSMIN = -1
                    End If
                    If (activeProcedureDataReader("MONTHS_MAX") IsNot System.DBNull.Value) Then
                        MONTHSMAX = CInt(activeProcedureDataReader("MONTHS_MAX"))
                    Else
                        MONTHSMAX = -1
                    End If

                    PUBLISH_BATCH_NBR = Nothing

                    If DbDataReaderColumnExists(activeProcedureDataReader, "PUBLISH_BATCH_NBR") = True Then
                        If activeProcedureDataReader("PUBLISH_BATCH_NBR") IsNot System.DBNull.Value Then
                            PUBLISH_BATCH_NBR = CInt(activeProcedureDataReader("PUBLISH_BATCH_NBR"))
                        End If
                    End If

                    If DbDataReaderColumnExists(activeProcedureDataReader, "PUBLISH_DATE") = True Then
                        If activeProcedureDataReader("PUBLISH_DATE") IsNot System.DBNull.Value Then
                            PUBLISH_DATE = CDate(activeProcedureDataReader("PUBLISH_DATE"))
                        End If
                    End If

                    ProcedureActive = New ProcedureActive(PROC_ID, BILL_TYPE, DIAGNOSIS, MODIFIER, GENDER, MONTHSMIN, MONTHSMAX, PLACE_OF_SERV, PLAN_TYPE, PROC_CODE, Provider, Nothing)
                    ProcedureActive.FromDate = FROM_DATE
                    ProcedureActive.ToDate = TO_DATE
                    ProcedureActive.SequenceNumber = SEQ_NBR

                    ProcedureActive.PublishDate = PUBLISH_DATE
                    Procedures.Add(ProcedureActive.ProcedureID, ProcedureActive)
                End If

                If Not getAsLightweight Then
                    ProcedureActive = GetCompleteProcedureFromDataReader(activeProcedureDataReader, ProcedureActive)
                End If

            End While

            Dim ProceduresEnum As IDictionaryEnumerator = Procedures.GetEnumerator()
            While ProceduresEnum.MoveNext
                ProcedureColl.Add(DirectCast(ProceduresEnum.Value, ProcedureActive))
            End While

            Return ProcedureColl

        Catch ex As Exception

            Throw New ConvertDataException("Cannot Convert Data to ProcedureCollection", ex)

        Finally

            Procedures = Nothing
            ProcedureActive = Nothing
            ProcedureColl = Nothing
        End Try

    End Function

    Private Shared Function DbDataReaderColumnExists(ByVal rdr As DbDataReader, columnName As String) As Boolean
        Dim result As Boolean

        If rdr IsNot Nothing Then
            For i As Integer = 0 To rdr.FieldCount - 1
                If rdr.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase) Then
                    result = True
                    Exit For
                End If
            Next
        End If
        Return result
    End Function
    Public Shared Function GetRuleSetActive(ByVal activeProcedureDataReader As IDataReader, ByVal procActive As Procedure) As RuleSetActive
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Creates an Active Ruleset object
        ' </summary>
        ' <param name="stagedProcedureDataReader"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	7/10/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        '
        Dim RuleSetActive As RuleSetActive
        Dim RuleSetIsSet As Boolean = False

        Try

            For Each rlset As RuleSetActive In procActive.RuleSets
                If rlset.RulesetType = Convert.ToInt32(activeProcedureDataReader("RULE_SET_TYPE")) Then
                    RuleSetActive = rlset
                    RuleSetIsSet = True
                End If
            Next

            If Not RuleSetIsSet Then
                RuleSetActive = New RuleSetActive
                'rlSetActive.Hidden = CType(stagedProcedureDataReader("HIDDEN_SW"), Boolean)
                RuleSetActive.RulesetType = Convert.ToInt32(activeProcedureDataReader("RULE_SET_TYPE"))
                RuleSetActive.RuleSetName = activeProcedureDataReader("RULE_SET_NAME").ToString
                RuleSetActive.RulesetID = CInt(activeProcedureDataReader("RULE_SET_ID"))
                RuleSetActive.MaxUnits = CType(activeProcedureDataReader("MAX_UNITS"), Decimal)
            End If

            Return RuleSetActive

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function GetRule(ByVal activeProcedureDataReader As IDataReader, ByRef ruleSetActive As RuleSetActive) As Rule
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets a Valid Rule for the datareader
        ' </summary>
        ' <param name="activeProcedureDataReader"></param>
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
        Dim ReturnRule As Rule

        Try

            For Each Rule As Rule In ruleSetActive
                If TypeOf (Rule) Is AccidentRule Then
                    If CType(Convert.ToInt32(activeProcedureDataReader("RULE_TYPE")), RuleTypes) = RuleTypes.Accident Then
                        ReturnRule = Rule
                        Exit For
                    End If
                ElseIf TypeOf (Rule) Is CoInsuranceRule Then
                    If CType(Convert.ToInt32(activeProcedureDataReader("RULE_TYPE")), RuleTypes) = RuleTypes.CoInsurance Then
                        ReturnRule = Rule
                        Exit For
                    End If
                ElseIf TypeOf (Rule) Is CoPayRule Then
                    If CType(Convert.ToInt32(activeProcedureDataReader("RULE_TYPE")), RuleTypes) = RuleTypes.CoPay Then
                        ReturnRule = Rule
                        Exit For
                    End If
                ElseIf TypeOf (Rule) Is MultilineCoPayRule Then
                    If CType(Convert.ToInt32(activeProcedureDataReader("RULE_TYPE")), RuleTypes) = RuleTypes.MultiLineCoPay Then
                        ReturnRule = Rule
                        Exit For
                    End If
                ElseIf TypeOf (Rule) Is DeductibleRule Then
                    If CType(Convert.ToInt32(activeProcedureDataReader("RULE_TYPE")), RuleTypes) = RuleTypes.Deductible Then
                        ReturnRule = Rule
                        Exit For
                    End If
                ElseIf TypeOf (Rule) Is OutOfPocketRule Then
                    If CType(Convert.ToInt32(activeProcedureDataReader("RULE_TYPE")), RuleTypes) = RuleTypes.OutOfPocket Then
                        ReturnRule = Rule
                        Exit For
                    End If
                ElseIf TypeOf (Rule) Is StandardAccumulatorRule Then
                    If CType(Convert.ToInt32(activeProcedureDataReader("RULE_TYPE")), RuleTypes) = RuleTypes.Standard Then
                        ReturnRule = Rule
                        Exit For
                    End If
                ElseIf TypeOf (Rule) Is DenyRule Then
                    If CType(Convert.ToInt32(activeProcedureDataReader("RULE_TYPE")), RuleTypes) = RuleTypes.Deny Then
                        ReturnRule = Rule
                        Exit For
                    End If
                ElseIf TypeOf (Rule) Is ProviderWriteOffRule Then
                    If CType(Convert.ToInt32(activeProcedureDataReader("RULE_TYPE")), RuleTypes) = RuleTypes.ProviderWriteOff Then
                        ReturnRule = Rule
                        Exit For
                    End If
                ElseIf TypeOf (Rule) Is HRAInEligibleRule Then
                    If CType(Convert.ToInt32(activeProcedureDataReader("RULE_TYPE")), RuleTypes) = RuleTypes.HRAInEligible Then
                        ReturnRule = Rule
                        Exit For
                    End If
                ElseIf TypeOf (Rule) Is PreventativeRule Then
                    If CType(Convert.ToInt32(activeProcedureDataReader("RULE_TYPE")), RuleTypes) = RuleTypes.Preventative Then
                        ReturnRule = Rule
                        Exit For
                    End If
                ElseIf TypeOf (Rule) Is OriginalRule Then
                    If CType(Convert.ToInt32(activeProcedureDataReader("RULE_TYPE")), RuleTypes) = RuleTypes.Original Then
                        ReturnRule = Rule
                        Exit For
                    End If
                End If
            Next

            If ReturnRule Is Nothing Then
                Select Case CType(Convert.ToInt32(activeProcedureDataReader("RULE_TYPE")), RuleTypes)
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
                    Case RuleTypes.HRAInEligible
                        ReturnRule = New HRAInEligibleRule(New Conditions)
                    Case RuleTypes.Preventative
                        ReturnRule = New PreventativeRule(New Conditions)
                    Case RuleTypes.Original
                        ReturnRule = New OriginalRule(New Conditions)
                End Select
                ruleSetActive.Add(ReturnRule)
            End If

            Return ReturnRule

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Shared Function GetProcedureCollectionFromDataReader(ByVal activeProcedureDS As DataSet) As Procedures
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Converts an DataSet to a ProcedureCollection
        ' </summary>
        ' <param name="activeProcedureDataSet"></param>
        ' <returns>ProcedureCollection</returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' [sbandi] created on 12/03/2007.Copied paul's code and Altered for better performance
        ' </history>

        If activeProcedureDS Is Nothing Then Throw New ArgumentNullException("activeProcedureDataSet")

        Dim FROM_DATE As Date, TO_DATE As Date, SEQ_NBR As Integer, PROC_ID As Integer = -1 : Dim BILL_TYPE As String : Dim DIAGNOSIS As String : Dim PLACE_OF_SERV As String : Dim PLAN_TYPE As String : Dim PROC_CODE As String : Dim Provider As String : Dim MODIFIER As String = "" : Dim GENDER As String = "" : Dim MONTHSMIN As Integer = -1 : Dim MONTHSMAX As Integer = -1
        Dim Procedures As New Hashtable
        Dim ProcCollection As New Procedures

        Try

#If TRACE Then
            If CInt(_TraceParallel.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(2).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(2).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
#End If

            'Parallel.ForEach(activeProcedureDS.Tables(0).Rows.Cast(Of DataRow), Sub(DR)
            '                                                                        Dim procActive As ProcedureActive = DirectCast(procedures(CInt(DR("PROC_ID"))), ProcedureActive)
            '                                                                        If procActive Is Nothing Then
            '                                                                            If Not DR("PROC_ID") Is System.DBNull.Value Then
            '                                                                                PROC_ID = CInt(DR("PROC_ID"))
            '                                                                            Else
            '                                                                                PROC_ID = -1
            '                                                                            End If
            '                                                                            If Not DR("BILL_TYPE") Is System.DBNull.Value Then
            '                                                                                BILL_TYPE = CStr(DR("BILL_TYPE")).Trim
            '                                                                            Else
            '                                                                                BILL_TYPE = ""
            '                                                                            End If
            '                                                                            If Not DR("DIAGNOSIS") Is System.DBNull.Value Then
            '                                                                                DIAGNOSIS = CStr(DR("DIAGNOSIS")).Trim
            '                                                                            Else
            '                                                                                DIAGNOSIS = ""
            '                                                                            End If
            '                                                                            If Not DR("PLACE_OF_SERV") Is System.DBNull.Value Then
            '                                                                                PLACE_OF_SERV = CStr(DR("PLACE_OF_SERV")).Trim
            '                                                                            Else
            '                                                                                PLACE_OF_SERV = ""
            '                                                                            End If
            '                                                                            If Not DR("PLAN_TYPE") Is System.DBNull.Value Then
            '                                                                                PLAN_TYPE = CStr(DR("PLAN_TYPE")).Trim
            '                                                                            Else
            '                                                                                PLAN_TYPE = ""
            '                                                                            End If
            '                                                                            If Not DR("OCC_FROM_DATE") Is System.DBNull.Value Then
            '                                                                                FROM_DATE = CDate(DR("OCC_FROM_DATE"))
            '                                                                            Else
            '                                                                                FROM_DATE = New Date(1970, 1, 1)
            '                                                                            End If
            '                                                                            If Not DR("OCC_TO_DATE") Is System.DBNull.Value Then
            '                                                                                TO_DATE = CDate(DR("OCC_TO_DATE"))
            '                                                                            Else
            '                                                                                TO_DATE = New Date(1970, 1, 1)
            '                                                                            End If
            '                                                                            If Not DR("SEQ_NBR") Is System.DBNull.Value Then
            '                                                                                SEQ_NBR = CInt(DR("SEQ_NBR"))
            '                                                                            Else
            '                                                                                SEQ_NBR = -1
            '                                                                            End If
            '                                                                            If Not DR("PROC_CODE") Is System.DBNull.Value Then
            '                                                                                PROC_CODE = CStr(DR("PROC_CODE")).Trim
            '                                                                            Else
            '                                                                                PROC_CODE = ""
            '                                                                            End If
            '                                                                            If Not DR("PROVIDER") Is System.DBNull.Value Then
            '                                                                                Provider = CStr(DR("PROVIDER")).Trim
            '                                                                            Else
            '                                                                                Provider = ""
            '                                                                            End If
            '                                                                            If Not (DR("MODIFIER") Is System.DBNull.Value) Then
            '                                                                                MODIFIER = CStr(DR("MODIFIER"))
            '                                                                            Else
            '                                                                                MODIFIER = ""
            '                                                                            End If
            '                                                                            If Not (DR("GENDER") Is System.DBNull.Value) Then
            '                                                                                GENDER = CStr(DR("GENDER"))
            '                                                                            Else
            '                                                                                GENDER = ""
            '                                                                            End If
            '                                                                            If Not (DR("MONTHS_MIN") Is System.DBNull.Value) Then
            '                                                                                MONTHSMIN = CInt(DR("MONTHS_MIN"))
            '                                                                            Else
            '                                                                                MONTHSMIN = -1
            '                                                                            End If
            '                                                                            If Not (DR("MONTHS_MAX") Is System.DBNull.Value) Then
            '                                                                                MONTHSMAX = CInt(DR("MONTHS_MAX"))
            '                                                                            Else
            '                                                                                MONTHSMAX = -1
            '                                                                            End If

            '                                                                            procActive = New ProcedureActive(PROC_ID, BILL_TYPE, DIAGNOSIS, MODIFIER, GENDER, MONTHSMIN, MONTHSMAX, PLACE_OF_SERV, PLAN_TYPE, PROC_CODE, Provider, Nothing)
            '                                                                            procActive.FromDate = FROM_DATE
            '                                                                            procActive.ToDate = TO_DATE
            '                                                                            procActive.SequenceNumber = SEQ_NBR

            '                                                                            If CInt(_TraceParallel.Level) > 0 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : procActive: " & procActive.ToString & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(2).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(2).GetFileLineNumber.ToString & ")")

            '                                                                            procedures.Add(procActive.ProcedureID, procActive)
            '                                                                        End If
            '                                                                    End Sub)


            Dim ProcActive As ProcedureActive
            For Each DR As DataRow In activeProcedureDS.Tables(0).Rows
                ProcActive = DirectCast(Procedures(CInt(DR("PROC_ID"))), ProcedureActive)
                If ProcActive Is Nothing Then
                    If DR("PROC_ID") IsNot System.DBNull.Value Then
                        PROC_ID = CInt(DR("PROC_ID"))
                    Else
                        PROC_ID = -1
                    End If
                    If DR("BILL_TYPE") IsNot System.DBNull.Value Then
                        BILL_TYPE = CStr(DR("BILL_TYPE")).Trim
                    Else
                        BILL_TYPE = ""
                    End If
                    If DR("DIAGNOSIS") IsNot System.DBNull.Value Then
                        DIAGNOSIS = CStr(DR("DIAGNOSIS")).Trim
                    Else
                        DIAGNOSIS = ""
                    End If
                    If DR("PLACE_OF_SERV") IsNot System.DBNull.Value Then
                        PLACE_OF_SERV = CStr(DR("PLACE_OF_SERV")).Trim
                    Else
                        PLACE_OF_SERV = ""
                    End If
                    If DR("PLAN_TYPE") IsNot System.DBNull.Value Then
                        PLAN_TYPE = CStr(DR("PLAN_TYPE")).Trim
                    Else
                        PLAN_TYPE = ""
                    End If
                    If DR("OCC_FROM_DATE") IsNot System.DBNull.Value Then
                        FROM_DATE = CDate(DR("OCC_FROM_DATE"))
                    Else
                        FROM_DATE = New Date(1970, 1, 1)
                    End If
                    If DR("OCC_TO_DATE") IsNot System.DBNull.Value Then
                        TO_DATE = CDate(DR("OCC_TO_DATE"))
                    Else
                        TO_DATE = New Date(1970, 1, 1)
                    End If
                    If DR("SEQ_NBR") IsNot System.DBNull.Value Then
                        SEQ_NBR = CInt(DR("SEQ_NBR"))
                    Else
                        SEQ_NBR = -1
                    End If
                    If DR("PROC_CODE") IsNot System.DBNull.Value Then
                        PROC_CODE = CStr(DR("PROC_CODE")).Trim
                    Else
                        PROC_CODE = ""
                    End If
                    If DR("PROVIDER") IsNot System.DBNull.Value Then
                        Provider = CStr(DR("PROVIDER")).Trim
                    Else
                        Provider = ""
                    End If
                    If (DR("MODIFIER") IsNot System.DBNull.Value) Then
                        MODIFIER = CStr(DR("MODIFIER"))
                    Else
                        MODIFIER = ""
                    End If
                    If (DR("GENDER") IsNot System.DBNull.Value) Then
                        GENDER = CStr(DR("GENDER"))
                    Else
                        GENDER = ""
                    End If
                    If (DR("MONTHS_MIN") IsNot System.DBNull.Value) Then
                        MONTHSMIN = CInt(DR("MONTHS_MIN"))
                    Else
                        MONTHSMIN = -1
                    End If
                    If Not (DR("MONTHS_MAX") Is System.DBNull.Value) Then
                        MONTHSMAX = CInt(DR("MONTHS_MAX"))
                    Else
                        MONTHSMAX = -1
                    End If

                    ProcActive = New ProcedureActive(PROC_ID, BILL_TYPE, DIAGNOSIS, MODIFIER, GENDER, MONTHSMIN, MONTHSMAX, PLACE_OF_SERV, PLAN_TYPE, PROC_CODE, Provider, Nothing)
                    ProcActive.FromDate = FROM_DATE
                    ProcActive.ToDate = TO_DATE
                    ProcActive.SequenceNumber = SEQ_NBR
                    Procedures.Add(ProcActive.ProcedureID, ProcActive)
                Else
                    If CInt(_TraceParallel.Level) > 0 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : procActive: " & ProcActive.ToString & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(2).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(2).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
                End If
            Next

            Dim Procs As IDictionaryEnumerator = Procedures.GetEnumerator()
            While Procs.MoveNext
                ProcCollection.Add(DirectCast(Procs.Value, ProcedureActive))
            End While

            Return ProcCollection

        Catch ex As Exception

            Throw New ConvertDataException("Cannot Convert Data to ProcedureCollection", ex)

        Finally
#If TRACE Then
            If CInt(_TraceParallel.Level) > 0 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(2).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(2).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
#End If
        End Try
    End Function

    Private Shared Function GetConditionsByRule(ByVal ruleId As Integer) As Conditions
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets the conditions associated with the rule
        ' </summary>
        ' <param name="ruleId"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/25/2006	Created
        '     [paulw] 9/19/2006   Added in parameter for condition that corresponds to ACR MED-0018
        ' </history>
        ' -----------------------------------------------------------------------------


        Dim SQLCommand As String = "FDBMD.RETRIEVE_CONDITION_ACTIVE"
        Dim DBCommandWrapper As DbCommand
        Dim DB As Database
        Dim FirstLoop As Boolean = True
        Dim RuleSetActive As New RuleSetActive
        Dim Condition As Condition
        Dim Conditions As New Conditions

        Try
#If TRACE Then
            If CInt(_TraceCaching.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""))
#End If

            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCommand)

            DBCommandWrapper.CommandTimeout = 1200
            DB.AddInParameter(DBCommandWrapper, "@RULE_ID", DbType.Int32, ruleId)

            Using DBReader As RefCountingDataReader = CType(DB.ExecuteReader(DBCommandWrapper), RefCountingDataReader)

                While (DBReader.InnerReader.Read)
                    Condition = New Condition
                    If DBReader.InnerReader("ACCUM_NAME") IsNot DBNull.Value Then
                        Condition.AccumulatorName = DBReader.InnerReader("ACCUM_NAME").ToString
                    End If
                    If DBReader.InnerReader("DIRECTION") IsNot DBNull.Value Then
                        Condition.Direction = CType(Convert.ToInt32(DBReader.InnerReader("DIRECTION").ToString), DateDirection)
                    End If
                    If DBReader.InnerReader("DURATION") IsNot DBNull.Value Then
                        Condition.Duration = Convert.ToInt32(DBReader.InnerReader("DURATION"))
                    End If
                    If DBReader.InnerReader("DURATION_TYPE") IsNot DBNull.Value Then
                        Condition.DurationType = CType(Convert.ToInt32(DBReader.InnerReader("DURATION_TYPE")), DateType)
                    End If
                    If DBReader.InnerReader("OPERAND") IsNot DBNull.Value Then
                        Condition.Operand = CDec(DBReader.InnerReader("OPERAND"))
                    End If
                    If DBReader.InnerReader("CHECK_FOR_HEADROOM") IsNot DBNull.Value Then
                        Condition.UseInHeadroomCheck = Convert.ToBoolean(DBReader.InnerReader("CHECK_FOR_HEADROOM"))
                    End If
                    If DBReader.InnerReader("MAX_UNITS") IsNot DBNull.Value Then
                        Condition.UseInHeadroomCheck = Convert.ToBoolean(DBReader.InnerReader("MAX_UNITS"))
                    End If
                    If DBReader.InnerReader("REPRICE_EXCEEDED_SW") IsNot DBNull.Value Then
                        Condition.RepriceIfExceeded = Convert.ToBoolean(DBReader.InnerReader("REPRICE_EXCEEDED_SW"))
                    End If
                    Conditions.Add(Condition)
                End While

            End Using

            Return Conditions

        Catch ex As Exception

            Throw New ActiveDataException("Cannot Get Active Conditions", ex)

        Finally
#If TRACE Then
            If _TraceCaching.Level > 0 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""))
#End If
        End Try

    End Function

    Private Shared Function GetAccumulators() As DataSet

        Dim DS As DataSet
        Dim UniqueThreadIdentifier As String
        Dim FStream As FileStream
        Dim XMLSerial As XmlSerializer
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "FDBMD.RETRIEVE_ALL_ACCUMULATOR_HISTORY"
        Dim XMLFilename As String
        Dim AccumulatorsUpdated As Boolean = False

        Try
#If TRACE Then
            If CInt(_TraceCaching.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""))
#End If

            XMLFilename = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_ACCUMULATORS" & ".xml"
            If File.Exists(XMLFilename) AndAlso (DateTime.Compare(File.GetLastWriteTime(XMLFilename), UFCWGeneral.NowDate.AddSeconds(-50)) > 0) Then
                AccumulatorsUpdated = True
            End If

            XMLFilename = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & SQLCall & ".xml"

            UniqueThreadIdentifier = UFCWGeneral.GetUniqueKey()

            DS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.PUBLISH_HISTORY", "PUBLISH_DATE", SQLCall, True, "", AccumulatorsUpdated)
            If DS.Tables.Count = 0 Then
                DB = CMSDALCommon.CreateDatabase()
                DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                DS = DB.ExecuteDataSet(DBCommandWrapper)
                DS.Tables(0).TableName = "CONDITIONS"

                FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                XMLSerial = New XmlSerializer(DS.GetType())
                XMLSerial.Serialize(FStream, DS)

                File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
            End If

            Return DS

        Catch ex As Exception

            Throw New ActiveDataException("Cannot retrieve Plan Conditions", ex)

        Finally


#If TRACE Then
            If CInt(_TraceCaching.Level) > 0 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceCaching.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""))
#End If
            If FStream IsNot Nothing Then FStream.Close()
            FStream = Nothing
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
End Class