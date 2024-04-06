Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data

Friend Class DentalDAL

    ''Private Shared _TraceCaching As New BooleanSwitch("TraceCaching", "Trace Switch in App.Config")

    Public Shared Event RecentClaimRefreshAvailable(e As RecentClaimEventArgs)

    Public Const _DB2SchemaName As String = "FDBDN"

    Private Shared _ComputerName As String = SystemInformation.ComputerName
    Private Shared _LastSQL As String

    Public Shared _DomainUser As String = SystemInformation.UserName

#Region "Public Properties"

    Public Shared ReadOnly Property LastSQL() As String
        Get
            Return _LastSQL
        End Get
    End Property

    Public Shared ReadOnly Property ComputerName() As String
        Get
            Return _ComputerName
        End Get
    End Property
#End Region
    Public Shared Function GetDentalPREAuthPENDClaimDetail(ByVal familyID As Integer?, ByVal relationID As Short?, ByVal claimID As Integer?, ByVal gridType As String) As DataTable

        Dim DS As DataSet = GetDentalPREAuthPENDClaimDetail(familyID, relationID, claimID, gridType, Nothing, Nothing)

        If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
            Return DS.Tables(0)
        Else
            Return Nothing
        End If

    End Function
    Public Shared Function GetDentalPREAuthPENDClaimDetail(ByVal familyID As Integer?, ByVal relationID As Short?, ByVal claimID As Integer?, ByVal gridType As String, Optional ByVal DS As DataSet = Nothing, Optional ByVal Transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBDN.RETRIEVE_DENTAL_PREAUTH_PEND_CLAIM_DETAIL")

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@GRID_TYPE", DbType.String, gridType)

            DBCommandWrapper.CommandTimeout = 180

            If DS Is Nothing Then
                If Transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, Transaction)
                End If

                DS.Tables(0).TableName = "Dental"

            Else
                If Transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "Dental")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "Dental", Transaction)
                End If
            End If

            Return DS

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetDentalInformation(ByVal fromDate As Date, ByVal toDate As Date, ByVal ssn As Integer?, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBDN.RETRIEVE_DENTAL_SUMMARY_BY_SSN_DOS")

            DB.AddInParameter(DBCommandWrapper, "@SSN", DbType.Int32, If(ssn Is Nothing, Nothing, CType(ssn, Integer?)))
            DB.AddInParameter(DBCommandWrapper, "@vFDOS_FROM", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@vFDOS_THRU", DbType.Date, toDate)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If

                ds.Tables(0).TableName = "Dental"
                'ds.Tables(0).DefaultView.RowFilter = "FDOS >= #" & fromDate & "# AND LDOS <= #" & toDate & "#"

            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "Dental")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "Dental", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function
    'Public Shared Function GetDentalPreAUTHAndPending(ByVal familyID As Integer?, ByVal relationID As Short?, ByVal gridType As String) As DataTable

    '    Dim DS As DataSet = GetDentalPreAUTHAndPending(familyID, relationID, reportType, gridType, Nothing, Nothing)

    '    If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
    '        Return DS.Tables(0)
    '    Else
    '        Return Nothing
    '    End If

    'End Function
    'Public Shared Function GetDentalPreAUTHAndPending(ByVal familyID As Integer?, ByVal relationID As Short?, ByVal reportType As String, ByVal gridType As String, Optional ByVal DS As DataSet = Nothing, Optional ByVal Transaction As DbTransaction = Nothing) As DataSet

    '    Dim DB As Database
    '    Dim DBCommandWrapper As DbCommand
    '    Try

    '        DB = CMSDALCommon.CreateDatabase()
    '        DBCommandWrapper = DB.GetStoredProcCommand("FDBDN.RETRIEVE_DENTAL_PREAUTH_PEND")

    '        DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
    '        DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
    '        DB.AddInParameter(DBCommandWrapper, "@REPORT_TYPE", DbType.String, reportType)
    '        DB.AddInParameter(DBCommandWrapper, "@GRID_TYPE", DbType.String, gridType)

    '        DBCommandWrapper.CommandTimeout = 180

    '        If DS Is Nothing Then
    '            If Transaction Is Nothing Then
    '                DS = DB.ExecuteDataSet(DBCommandWrapper)
    '            Else
    '                DS = DB.ExecuteDataSet(DBCommandWrapper, Transaction)
    '            End If

    '            DS.Tables(0).TableName = "Dental"

    '        Else
    '            If Transaction Is Nothing Then
    '                DB.LoadDataSet(DBCommandWrapper, DS, "Dental")
    '            Else
    '                DB.LoadDataSet(DBCommandWrapper, DS, "Dental", Transaction)
    '            End If
    '        End If

    '        Return DS

    '    Catch ex As Exception
    '        Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '        If (Rethrow) Then
    '            Throw
    '        End If
    '    End Try
    'End Function

    Public Shared Function GetDentalPendOrPreAuthInformation(ByVal familyID As Integer, ByVal relationID As Short?, ByVal gridType As String, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBDN.RETRIEVE_DENTAL_SUMMARY_PREAUTH_PEND")

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@GRID_TYPE", DbType.String, gridType)
            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else

                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "DentalDentalPREAuth")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "DentalDentalPREAuth", transaction)

                End If
            End If

            ds.Tables(0).TableName = "DentalPREAuth"


            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GetDentalAccumulators(ByVal familyID As Integer?, ByVal relationID As Short?, ByVal yearType As String, ByVal firstDay As Date, ByVal lastDay As Date, ByVal gridType As String) As DataTable

        Dim DS As DataSet = GetDentalAccumulators(familyID, relationID, yearType, firstDay, lastDay, gridType, Nothing, Nothing)

        If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
            Return DS.Tables(0)
        Else
            Return Nothing
        End If

    End Function
    Public Shared Function GetDentalAccumulators(ByVal familyID As Integer?, ByVal relationID As Short?, ByVal yearType As String, ByVal firstDay As Date, ByVal lastDay As Date, ByVal gridType As String, Optional ByVal DS As DataSet = Nothing, Optional ByVal Transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBDN.RETRIEVE_DENTAL_ACCUMULATORS")

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@YEAR_TYPE", DbType.String, yearType)
            DB.AddInParameter(DBCommandWrapper, "@vFIRST_DAY", DbType.Date, firstDay)
            DB.AddInParameter(DBCommandWrapper, "@vLAST_DAY", DbType.Date, lastDay)
            DB.AddInParameter(DBCommandWrapper, "@GRID_TYPE", DbType.String, gridType)

            DBCommandWrapper.CommandTimeout = 180

            If DS Is Nothing Then
                If Transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, Transaction)
                End If

                DS.Tables(0).TableName = "Dental"

            Else
                If Transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "Dental")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "Dental", Transaction)
                End If
            End If

            Return DS

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetDentalInformation(ByVal fromDate As Date, ByVal toDate As Date, ByVal familyID As Integer, Optional ByVal relationID As Short? = Nothing, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBDN.RETRIEVE_DENTAL_SUMMARY_BY_FAMILYID_RELATIONID_DOS")

            DB.AddInParameter(DBCommandWrapper, "@FAMILYID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATIONID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@vFDOS_FROM", DbType.Date, fromDate)
            DB.AddInParameter(DBCommandWrapper, "@vFDOS_THRU", DbType.Date, toDate)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else

                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "Dental")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "Dental", transaction)

                End If
            End If

            ds.Tables(0).TableName = "Dental"

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function UpdateDentalHistoryStatus(ByVal familyID As Integer?, ByVal relationID As Short?, ByVal claimID As Integer?, ByVal status As String, Optional ByVal transaction As DbTransaction = Nothing) As Boolean

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            status = "REPRINT_EOB "
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBDN.UPDATE_DENTAL_HISTORY_STATUS_BY_CLAIM")

            DB.AddInParameter(DBCommandWrapper, "@FAMILYID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATIONID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@PROCESS_STATUS", DbType.String, status)
            DB.AddInParameter(DBCommandWrapper, "@ONLINE_USERID", DbType.String, _DomainUser.ToUpper)
            DBCommandWrapper.CommandTimeout = 180


            DB.ExecuteDataSet(DBCommandWrapper)

            Return True
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GetDentalInformation(ByVal familyID As Integer, Optional ByVal relationID As Short? = Nothing, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBDN.RETRIEVE_DENTAL_SUMMARY_BY_FAMILYID_RELATIONID")

            DB.AddInParameter(DBCommandWrapper, "@FAMILYID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATIONID", DbType.Int16, relationID)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else

                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "Dental")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "Dental", transaction)

                End If
            End If

            ds.Tables(0).TableName = "Dental"

            'ds.Tables(0).DefaultView.RowFilter = "FDOS >= #" & fromDate & "# AND LDOS <= #" & toDate & "#"

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function

    'Public Shared Sub UpdateDentalHistoryStatus(ByVal familyID As Integer?, ByVal relationID As Short?, ByVal claimID As Integer?, ByVal status As String, Optional ByVal transaction As DbTransaction = Nothing)

    '    Dim DB As Database
    '    Dim DBCommandWrapper As DbCommand

    '    Try

    '        DB = CMSDALCommon.CreateDatabase()
    '        DBCommandWrapper = DB.GetStoredProcCommand("FDBDN.UPDATE_DENTAL_HISTORY_STATUS_BY_CLAIM")

    '        DB.AddInParameter(DBCommandWrapper, "@FAMILYID", DbType.Int32, familyID)
    '        DB.AddInParameter(DBCommandWrapper, "@RELATIONID", DbType.Int16, relationID)
    '        DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
    '        DB.AddInParameter(DBCommandWrapper, "@PROCESS_STATUS", DbType.String, status)
    '        DB.AddInParameter(DBCommandWrapper, "@ONLINE_USERID", DbType.String, _DomainUser.ToUpper)
    '        DBCommandWrapper.CommandTimeout = 180


    '        DB.ExecuteDataSet(DBCommandWrapper)


    '    Catch ex As Exception
    '        Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '        If (Rethrow) Then
    '            Throw
    '        End If
    '    End Try
    'End Sub

    Public Shared Function GetDentalClaimDetail(ByVal familyID As Integer?, ByVal relationID As Short?, ByVal claimID As Integer?) As DataTable

        Dim DS As DataSet = GetDentalClaimDetail(familyID, relationID, claimID, Nothing, Nothing)

        If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
            Return DS.Tables(0)
        Else
            Return Nothing
        End If

    End Function
    Public Shared Function GetDentalClaimDetail(ByVal familyID As Integer?, ByVal relationID As Short?, ByVal claimID As Integer?, Optional ByVal DS As DataSet = Nothing, Optional ByVal Transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBDN.RETRIEVE_DENTAL_CLAIM_BY_FAMILYID_RELATIONID")

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM", DbType.Int32, claimID)


            DBCommandWrapper.CommandTimeout = 180

            If DS Is Nothing Then
                If Transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, Transaction)
                End If

                DS.Tables(0).TableName = "Dental"

            Else
                If Transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "Dental")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "Dental", Transaction)
                End If
            End If

            Return DS

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetDentalLineDetail(ByVal familyID As Integer?, ByVal relationID As Short?, ByVal claimID As Integer?, ByVal procedure As String) As DataTable

        Dim DS As DataSet = GetDentalLineDetail(familyID, relationID, claimID, procedure, Nothing, Nothing)

        If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
            Return DS.Tables(0)
        Else
            Return Nothing
        End If

    End Function

    Public Shared Function GetDentalLineDetail(ByVal familyID As Integer?, ByVal relationID As Short?, ByVal claimID As Integer?, ByVal procedure As String, Optional ByVal DS As DataSet = Nothing, Optional ByVal Transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBDN.RETRIEVE_DENTAL_LINE_DETAIL")

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@PROCEDURE", DbType.String, procedure)

            DBCommandWrapper.CommandTimeout = 180

            If DS Is Nothing Then
                If Transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, Transaction)
                End If

                DS.Tables(0).TableName = "Dental"

            Else
                If Transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "Dental")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "Dental", Transaction)
                End If
            End If

            Return DS

        Catch ex As Exception
            Throw
        End Try
    End Function

End Class
'Friend Class DentalDAL
'    Public Const _DB2databaseName As String = "FDBDN"
'    Public Const _SQLdatabaseName As String = "dbo"
'    Public Shared DomainUser As String = SystemInformation.UserName

'#Region "Procedures/Functions"

'    Public Shared Function GetDentalInformation(ByVal ssn As Integer?, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

'        Dim DB As Database
'        Dim DBCommandWrapper As DbCommand

'        Try

'            DB = CMSDALCommon.CreateDatabase()
'            DBCommandWrapper = DB.GetStoredProcCommand("FDBDN.RETRIEVE_DENTAL_SUMMARY_BY_SSN")

'            DB.AddInParameter(DBCommandWrapper, "@SSN", DbType.Int32, If(ssn Is Nothing, Nothing, CType(ssn, Integer?)))


'            DBCommandWrapper.CommandTimeout = 180

'            If ds Is Nothing Then
'                If transaction Is Nothing Then
'                    ds = DB.ExecuteDataSet(DBCommandWrapper)
'                Else
'                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
'                End If

'                ds.Tables(0).TableName = "Dental"

'            Else
'                If transaction Is Nothing Then
'                    DB.LoadDataSet(DBCommandWrapper, ds, "Dental")
'                Else
'                    DB.LoadDataSet(DBCommandWrapper, ds, "Dental", transaction)
'                End If
'            End If

'            Return ds

'        Catch ex As Exception
'            Throw
'        End Try
'    End Function
'    Public Shared Function GetDentalInformation(ByVal familyID As Integer, Optional ByVal relationID As Short? = Nothing, Optional ByVal DS As DataSet = Nothing, Optional ByVal Transaction As DbTransaction = Nothing) As DataSet
'        Dim DB As Database
'        Dim DBCommandWrapper As DbCommand

'        Try
'            DB = CMSDALCommon.CreateDatabase()
'            DBCommandWrapper = DB.GetStoredProcCommand("FDBDN.RETRIEVE_DENTAL_SUMMARY_BY_FAMILYID_RELATIONID")

'            DB.AddInParameter(DBCommandWrapper, "@FAMILYID", DbType.Int32, familyID)
'            DB.AddInParameter(DBCommandWrapper, "@RELATIONID", DbType.Int16, relationID)

'            DBCommandWrapper.CommandTimeout = 180

'            If DS Is Nothing Then
'                If Transaction Is Nothing Then
'                    DS = DB.ExecuteDataSet(DBCommandWrapper)
'                Else
'                    DS = DB.ExecuteDataSet(DBCommandWrapper, Transaction)
'                End If
'            Else
'                If Transaction Is Nothing Then
'                    DB.LoadDataSet(DBCommandWrapper, DS, "DENTAL")
'                Else
'                    DB.LoadDataSet(DBCommandWrapper, DS, "DENTAL", Transaction)
'                End If
'            End If

'            DS.Tables(0).TableName = "DENTAL"
'            'DS.Tables(1).TableName = "ISELIGIBLE"

'            Return DS

'        Catch ex As Exception
'            Throw
'        End Try
'    End Function
'    Public Shared Function CreateDataSetFromXML(XMLFile As String) As DataSet

'        'load xml into a dataset to use here
'        Dim dSet As New DataSet
'        Dim Fs As FileStream = Nothing

'        'open the xml file so we can use it to fill the dataset
'        Try
'            Fs = New FileStream(System.Windows.Forms.Application.StartupPath & "\" & XMLFile, FileMode.Open, FileAccess.Read)

'        Catch ex As Exception
'            Throw
'        End Try

'        'fill the dataset
'        Try
'            dSet.ReadXml(Fs)

'            If Not dSet.Tables("Column").Columns.Contains("Visible") Then dSet.Tables("Column").Columns.Add("Visible")
'            If Not dSet.Tables("Column").Columns.Contains("FormatIsRegEx") Then dSet.Tables("Column").Columns.Add("FormatIsRegEx")
'            If Not dSet.Tables("Column").Columns.Contains("WordWrap") Then dSet.Tables("Column").Columns.Add("WordWrap")
'            If Not dSet.Tables("Column").Columns.Contains("ReadOnly") Then dSet.Tables("Column").Columns.Add("ReadOnly")
'            If Not dSet.Tables("Column").Columns.Contains("MinimumCharWidth") Then dSet.Tables("Column").Columns.Add("MinimumCharWidth")
'            If Not dSet.Tables("Column").Columns.Contains("MaximumCharWidth") Then dSet.Tables("Column").Columns.Add("MaximumCharWidth")

'            Return dSet

'        Catch ex As Exception
'            Throw
'        Finally
'            Fs.Close()
'        End Try

'    End Function

'    Public Shared Sub ExecuteReprintEOB(ByVal familyID As Integer?, ByVal relationID As Short?, ByVal claimID As Integer?, ByVal process_status As String)

'        Dim DB As Database
'        Dim DBCommandWrapper As DbCommand

'        Try
'            process_status = "REPRINT_EOB"
'            DB = CMSDALCommon.CreateDatabase()
'            DBCommandWrapper = DB.GetStoredProcCommand("FDBDN.UPDATE_DENTAL_HISTORY_STATUS_BY_CLAIM")

'            DB.AddInParameter(DBCommandWrapper, "@FAMILYID", DbType.Int32, familyID)
'            DB.AddInParameter(DBCommandWrapper, "@RELATIONID", DbType.Int16, relationID)
'            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
'            DB.AddInParameter(DBCommandWrapper, "@PROCESS_STATUS", DbType.String, process_status)

'            DBCommandWrapper.CommandTimeout = 180

'            DB.ExecuteDataSet(DBCommandWrapper)

'        Catch ex As Exception
'            Throw
'        End Try

'    End Sub

'    'Public Shared Function GetDentalLineDetail(ByVal familyID As Integer?, ByVal relationID As Short?, ByVal claimID As Integer?) As DataTable

'    '    Dim DS As DataSet = GetDentalLineDetail(familyID, relationID, claimID, Nothing, Nothing)

'    '    If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
'    '        Return DS.Tables(0)
'    '    Else
'    '        Return Nothing
'    '    End If

'    'End Function
'    'Public Shared Function GetDentalLineDetail(ByVal familyID As Integer?, ByVal relationID As Short?, ByVal claimID As Integer?, Optional ByVal DS As DataSet = Nothing, Optional ByVal Transaction As DbTransaction = Nothing) As DataSet

'    '    Dim DB As Database
'    '    Dim DBCommandWrapper As DbCommand
'    '    Try

'    '        DB = CMSDALCommon.CreateDatabase()
'    '        DBCommandWrapper = DB.GetStoredProcCommand("FDBDN.RETRIEVE_DENTAL_CLAIM_BY_FAMILYID_RELATIONID")

'    '        DB.AddInParameter(DBCommandWrapper, "@FAMILYID", DbType.Int32, familyID)
'    '        DB.AddInParameter(DBCommandWrapper, "@RELATIONID", DbType.Int16, relationID)
'    '        DB.AddInParameter(DBCommandWrapper, "@CLAIMID", DbType.Int32, claimID)


'    '        DBCommandWrapper.CommandTimeout = 180

'    '        If DS Is Nothing Then
'    '            If Transaction Is Nothing Then
'    '                DS = DB.ExecuteDataSet(DBCommandWrapper)
'    '            Else
'    '                DS = DB.ExecuteDataSet(DBCommandWrapper, Transaction)
'    '            End If

'    '            DS.Tables(0).TableName = "Dental"

'    '        Else
'    '            If Transaction Is Nothing Then
'    '                DB.LoadDataSet(DBCommandWrapper, DS, "Dental")
'    '            Else
'    '                DB.LoadDataSet(DBCommandWrapper, DS, "Dental", Transaction)
'    '            End If
'    '        End If

'    '        Return DS

'    '    Catch ex As Exception
'    '        Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
'    '        If (Rethrow) Then
'    '            Throw
'    '        End If
'    '    End Try
'    'End Function

'#End Region
'End Class
