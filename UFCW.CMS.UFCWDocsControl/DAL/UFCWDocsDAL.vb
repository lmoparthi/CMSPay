Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.IO
Imports System.Data.Common
Imports System.Configuration

Public Class UFCWDocsDAL

    Private Shared _DomainUser As String = SystemInformation.UserName
    Private Shared _DefaultProviderName As String = CType(ConfigurationManager.GetSection("dataConfiguration"), Microsoft.Practices.EnterpriseLibrary.Data.Configuration.DatabaseSettings).DefaultDatabase

    Public Shared Function GetUFCWDocsByReceivedDateCurrentUser(beginDate? As Date, endDate? As Date, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim ResultDS As DataSet = New DataSet
        Dim ResultDV As DataView
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase("ImgWorkflow")
            SQLCall = CMSDALCommon.GetDatabaseName("ImgWorkflow") & "." & "RETRIEVE_UFCWDOCS_BY_RECDATE_BY_USERID"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FROMDATE", DbType.DateTime, CDate(If(beginDate Is Nothing, "1753-01-01", beginDate.Value.ToShortDateString) & " 00:00:00"))
            DB.AddInParameter(DBCommandWrapper, "@TODATE", DbType.DateTime, CDate(If(endDate Is Nothing, "9999-12-31", endDate.Value.ToShortDateString) & " 23:59:59"))
            DB.AddInParameter(DBCommandWrapper, "@NTUSERID", DbType.String, _DomainUser)

            DBCommandWrapper.CommandTimeout = 180
            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "UFCWDocs")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "UFCWDocs", transaction)
                End If
            End If

            Dim DT As DataTable = ds.Tables(0).Clone

            ResultDV = New DataView(ds.Tables(0), " Documentclass <> 'Claims' ", "", DataViewRowState.CurrentRows)

            If ResultDV.Count > 0 Then
                For Each DRV As DataRowView In ResultDV
                    DT.LoadDataRow(DRV.Row.ItemArray, True)
                Next
            End If
            ResultDS.Tables.Add(DT)

            Return ResultDS

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GetUFCWDocsByReceivedDateAllUsers(beginDate? As Date, endDate? As Date, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim ResultDS As DataSet = New DataSet
        Dim ResultDV As DataView
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = CMSDALCommon.GetDatabaseName($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance") & "." & "RETRIEVE_UFCWDOCS_BY_RECDATE"

        Try
            DB = CMSDALCommon.CreateDatabase($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance")

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FROMDATE", DbType.DateTime, CDate(If(beginDate Is Nothing, "1753-01-01", beginDate.Value.ToShortDateString) & " 00:00:00"))
            DB.AddInParameter(DBCommandWrapper, "@TODATE", DbType.DateTime, CDate(If(endDate Is Nothing, "9999-12-31", endDate.Value.ToShortDateString) & " 23:59:59"))

            DBCommandWrapper.CommandTimeout = 180
            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "UFCWDocs")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "UFCWDocs", transaction)
                End If
            End If

            Dim DT As DataTable = ds.Tables(0).Clone

            ResultDV = New DataView(ds.Tables(0), " Documentclass <> 'Claims' ", "", DataViewRowState.CurrentRows)

            If ResultDV.Count > 0 Then
                For Each DRV As DataRowView In ResultDV
                    DT.LoadDataRow(DRV.Row.ItemArray, True)
                Next
            End If
            ResultDS.Tables.Add(DT)

            Return ResultDS

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GetUFCWDocsFromSSN(ByVal ssn As Integer, beginDate? As Date, endDate? As Date, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim ResultDS As DataSet = New DataSet
        Dim ResultDV As DataView
        Dim DBCommandWrapper As DbCommand
        Dim DB As Database
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance")
            SQLCall = CMSDALCommon.GetDatabaseName($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance") & "." & "RETRIEVE_UFCWDocs_BY_SSN_RECDATE_BY_USERID"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@SSN", DbType.Int32, ssn)
            DB.AddInParameter(DBCommandWrapper, "@FROMDATE", DbType.DateTime, CDate(If(beginDate Is Nothing, "1753-01-01", beginDate.Value.ToShortDateString) & " 00:00:00"))
            DB.AddInParameter(DBCommandWrapper, "@TODATE", DbType.DateTime, CDate(If(endDate Is Nothing, "9999-12-31", endDate.Value.ToShortDateString) & " 23:59:59"))
            DB.AddInParameter(DBCommandWrapper, "@NTUSERID", DbType.String, _DomainUser)

            DBCommandWrapper.CommandTimeout = 180
            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
                ds.Tables(0).TableName = "UFCWDocs"
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "UFCWDocs")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "UFCWDocs", transaction)
                End If
            End If

            Dim DT As DataTable = ds.Tables(0).Clone

            ResultDV = New DataView(ds.Tables(0), " Documentclass <> 'Claims' AND Documentclass <> 'CLAIMS' ", "", DataViewRowState.CurrentRows)

            If ResultDV.Count > 0 Then
                For Each drv As DataRowView In ResultDV
                    DT.LoadDataRow(drv.Row.ItemArray, True)
                Next
            End If
            ResultDS.Tables.Add(DT)

            Return ResultDS

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetUFCWDocsFromSSN(ByVal ssn As Integer, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Dim ResultDS As New DataSet
        Dim ResultDV As DataView
        Dim DT As DataTable

        Try
            DB = CMSDALCommon.CreateDatabase($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance")
            SQLCall = CMSDALCommon.GetDatabaseName($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance") & "." & "RETRIEVE_UFCWDOCS_BY_SSN_BY_USERID"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@SSN", DbType.Int32, ssn)
            DB.AddInParameter(DBCommandWrapper, "@NTUSERID", DbType.String, _DomainUser)

            DBCommandWrapper.CommandTimeout = 180
            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If
            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "UFCWDocs")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "UFCWDocs", transaction)
                End If
            End If

            DT = ds.Tables(0).Clone

            ResultDV = New DataView(ds.Tables(0), " Documentclass <> 'Claims' ", "", DataViewRowState.CurrentRows)

            If ResultDV.Count > 0 Then
                For Each DRV As DataRowView In ResultDV
                    DT.LoadDataRow(DRV.Row.ItemArray, True)
                Next
            End If

            ResultDS.Tables.Add(DT)

            Return ResultDS

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetUFCWDocsFromSSNAndTAXID(ByVal ssn As Integer, ByVal taxID As Integer, beginDate? As Date, endDate? As Date, Optional ByVal DS As DataSet = Nothing, Optional ByVal Transaction As DbTransaction = Nothing) As DataSet
        Dim ResultDS As DataSet = New DataSet
        Dim ResultDV As DataView = Nothing
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance")
            SQLCall = CMSDALCommon.GetDatabaseName($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance") & "." & "RETRIEVE_UFCWDOCS_BY_SSN_TAXID_RECDATE_BY_USERID"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@SSN", DbType.Int32, ssn)
            DB.AddInParameter(DBCommandWrapper, "@TAXID", DbType.Int32, taxID)
            DB.AddInParameter(DBCommandWrapper, "@FROMDATE", DbType.DateTime, CDate(If(beginDate Is Nothing, "1753-01-01", beginDate.Value.ToShortDateString) & " 00:00:00"))
            DB.AddInParameter(DBCommandWrapper, "@TODATE", DbType.DateTime, CDate(If(endDate Is Nothing, "9999-12-31", endDate.Value.ToShortDateString) & " 23:59:59"))
            DB.AddInParameter(DBCommandWrapper, "@NTUSERID", DbType.String, _DomainUser)

            DBCommandWrapper.CommandTimeout = 180
            If DS Is Nothing Then
                If Transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, Transaction)
                End If
            Else
                If Transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "UFCWDocs")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "UFCWDocs", Transaction)
                End If
            End If

            Dim DT As DataTable = DS.Tables(0).Clone

            ResultDV = New DataView(DS.Tables(0), " Documentclass <> 'Claims' ", "", DataViewRowState.CurrentRows)

            If ResultDV.Count > 0 Then
                For Each drv As DataRowView In ResultDV
                    DT.LoadDataRow(drv.Row.ItemArray, True)
                Next
            End If
            ResultDS.Tables.Add(DT)

            Return ResultDS

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetUFCWDocsFromSSNAndTAXID(ByVal SSN As Integer, ByVal TAXID As Integer, Optional ByVal DS As DataSet = Nothing, Optional ByVal Transaction As DbTransaction = Nothing) As DataSet
        Dim ResultDS As DataSet = New DataSet
        Dim ResultDV As DataView = Nothing
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance")
            SQLCall = CMSDALCommon.GetDatabaseName($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance") & "." & "RETRIEVE_UFCWDOCS_BY_SSN_TAXID_BY_USERID"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@SSN", DbType.Int32, SSN)
            DB.AddInParameter(DBCommandWrapper, "@TAXID", DbType.Int32, TAXID)
            DB.AddInParameter(DBCommandWrapper, "@NTUSERID", DbType.String, _DomainUser)

            DBCommandWrapper.CommandTimeout = 180
            If DS Is Nothing Then
                If Transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, Transaction)
                End If
            Else
                If Transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "UFCWDocs")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "UFCWDocs", Transaction)
                End If
            End If

            Dim DT As DataTable = DS.Tables(0).Clone

            ResultDV = New DataView(DS.Tables(0), " Documentclass <> 'Claims' ", "", DataViewRowState.CurrentRows)

            If ResultDV.Count > 0 Then
                For Each drv As DataRowView In ResultDV
                    DT.LoadDataRow(drv.Row.ItemArray, True)
                Next
            End If
            ResultDS.Tables.Add(DT)

            Return ResultDS

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GetUFCWDocsFromDOCID(ByVal DOCID As String, Optional ByVal DS As DataSet = Nothing, Optional ByVal Transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try

            DB = CMSDALCommon.CreateDatabase($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance")
            SQLCall = CMSDALCommon.GetDatabaseName($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance") & "." & "RETRIEVE_UFCWDOCS_BY_DOCID_BY_USERID"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@DOCID", DbType.String, DOCID)
            DB.AddInParameter(DBCommandWrapper, "@NTUSERID", DbType.String, _DomainUser)
            DBCommandWrapper.CommandTimeout = 180

            If DS Is Nothing Then
                If Transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, Transaction)
                End If
            Else
                If Transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "UFCWDocs")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "UFCWDocs", Transaction)
                End If
            End If
            Return DS

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetUFCWDocsFromBATCH(ByVal BatchNumber As String, Optional ByVal DS As DataSet = Nothing, Optional ByVal Transaction As DbTransaction = Nothing) As DataSet

        Dim ResultDS As DataSet = New DataSet
        Dim ResultDV As DataView = Nothing
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance")
            SQLCall = CMSDALCommon.GetDatabaseName($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance") & "." & "RETRIEVE_UFCWDOCS_BY_BATCH_BY_USERID"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@BATCHNUMBER", DbType.String, BatchNumber)
            DB.AddInParameter(DBCommandWrapper, "@NTUSERID", DbType.String, _DomainUser)

            DBCommandWrapper.CommandTimeout = 180
            If DS Is Nothing Then
                If Transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, Transaction)
                End If
            Else
                If Transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "UFCWDocs")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "UFCWDocs", Transaction)
                End If
            End If

            Dim DT As DataTable = DS.Tables(0).Clone
            'Return DS
            ResultDV = New DataView(DS.Tables(0), " Documentclass <> 'Claims' ", "", DataViewRowState.CurrentRows)
            If ResultDV.Count > 0 Then
                For Each DRV As DataRowView In ResultDV
                    DT.LoadDataRow(DRV.Row.ItemArray, True)
                Next
            End If
            ResultDS.Tables.Add(DT)

            Return ResultDS

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GetUFCWDocsFromMaximID(ByVal MaximID As String, Optional ByVal DS As DataSet = Nothing, Optional ByVal Transaction As DbTransaction = Nothing) As DataSet
        Dim ResultDS As DataSet = New DataSet
        Dim ResultDV As DataView = Nothing
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance")
            SQLCall = CMSDALCommon.GetDatabaseName($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance") & "." & "RETRIEVE_UFCWDOCS_BY_MAXIMID_BY_USERID"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@MAXIMID", DbType.String, MaximID)
            DB.AddInParameter(DBCommandWrapper, "@NTUSERID", DbType.String, _DomainUser)

            DBCommandWrapper.CommandTimeout = 180
            If DS Is Nothing Then
                If Transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, Transaction)
                End If
            Else
                If Transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "UFCWDocs")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "UFCWDocs", Transaction)
                End If
            End If

            Dim DT As DataTable = DS.Tables(0).Clone
            'Return DS
            ResultDV = New DataView(DS.Tables(0), " Documentclass <> 'Claims' ", "", DataViewRowState.CurrentRows)
            If ResultDV.Count > 0 Then
                For Each drv As DataRowView In ResultDV
                    DT.LoadDataRow(drv.Row.ItemArray, True)
                Next
            End If
            ResultDS.Tables.Add(DT)

            Return ResultDS

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GetUFCWDocsFromTAXID(ByVal taxID As Integer, beginDate? As Date, endDate? As Date, Optional ByVal DS As DataSet = Nothing, Optional ByVal Transaction As DbTransaction = Nothing) As DataSet
        Dim ResultDS As DataSet = New DataSet
        Dim ResultDV As DataView = Nothing
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance")
            SQLCall = CMSDALCommon.GetDatabaseName($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance") & "." & "RETRIEVE_UFCWDOCS_BY_TAXID_RECDATE_BY_USERID"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@TAXID", DbType.Int32, taxID)
            DB.AddInParameter(DBCommandWrapper, "@FROMDATE", DbType.DateTime, CDate(If(beginDate Is Nothing, "1753-01-01", beginDate.Value.ToShortDateString) & " 00:00:00"))
            DB.AddInParameter(DBCommandWrapper, "@TODATE", DbType.DateTime, CDate(If(endDate Is Nothing, "9999-12-31", endDate.Value.ToShortDateString) & " 23:59:59"))
            DB.AddInParameter(DBCommandWrapper, "@NTUSERID", DbType.String, _DomainUser)

            DBCommandWrapper.CommandTimeout = 180
            If DS Is Nothing Then
                If Transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, Transaction)
                End If
            Else
                If Transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "UFCWDocs")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "UFCWDocs", Transaction)
                End If
            End If

            Return DS

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GetUFCWDocsFromTAXID(ByVal taxXID As Integer, Optional ByVal DS As DataSet = Nothing, Optional ByVal Transaction As DbTransaction = Nothing) As DataSet
        Dim ResultDS As DataSet = New DataSet
        Dim ResultDV As DataView = Nothing
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance")
            SQLCall = CMSDALCommon.GetDatabaseName($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance") & "." & "RETRIEVE_UFCWDOCS_BY_TAXID_BY_USERID"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@TAXID", DbType.Int32, taxXID)
            DB.AddInParameter(DBCommandWrapper, "@NTUSERID", DbType.String, _DomainUser)

            DBCommandWrapper.CommandTimeout = 180
            If DS Is Nothing Then
                If Transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, Transaction)
                End If
            Else
                If Transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "UFCWDocs")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "UFCWDocs", Transaction)
                End If
            End If
            Return DS

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function CreateDataSetFromXML(ByVal XMLFile As String) As DataSet

        'load xml into a dataset to use here
        Dim DS As New DataSet

        'open the xml file so we can use it to fill the dataset
        Try
            Using Fs As New FileStream(System.Windows.Forms.Application.StartupPath & "\" & XMLFile, FileMode.Open, FileAccess.Read)

                DS.ReadXml(Fs)

                If Not DS.Tables("Column").Columns.Contains("Visible") Then DS.Tables("Column").Columns.Add("Visible")
                If Not DS.Tables("Column").Columns.Contains("FormatIsRegEx") Then DS.Tables("Column").Columns.Add("FormatIsRegEx")
                If Not DS.Tables("Column").Columns.Contains("WordWrap") Then DS.Tables("Column").Columns.Add("WordWrap")
                If Not DS.Tables("Column").Columns.Contains("ReadOnly") Then DS.Tables("Column").Columns.Add("ReadOnly")
                If Not DS.Tables("Column").Columns.Contains("MinimumCharWidth") Then DS.Tables("Column").Columns.Add("MinimumCharWidth")
                If Not DS.Tables("Column").Columns.Contains("MaximumCharWidth") Then DS.Tables("Column").Columns.Add("MaximumCharWidth")

                Return DS
            End Using

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Function

    Public Shared Function RetrieveParticipantInfo(ByVal FAMILYID As Integer, Optional ByVal Transaction As DbTransaction = Nothing) As DataRow
        Try
            Dim DS As DataSet
            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBMD.RETRIEVE_REG_MASTER_BY_FAMILYID"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, FAMILYID)

            DBCommandWrapper.CommandTimeout = 180

            If Transaction Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DS = DB.ExecuteDataSet(DBCommandWrapper, Transaction)
            End If

            Dim _dataTable As New DataTable("REG_MASTER")
            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0).Rows(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveParticipantInfofromPartSSN(ByVal PartSSN As Integer, Optional ByVal Transaction As DbTransaction = Nothing) As DataRow
        Try
            Dim DS As DataSet
            Dim DB As Database = CMSDALCommon.CreateDatabase()
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String = "FDBMD.RETRIEVE_REG_MASTER_BY_PARTSSN"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, PartSSN)

            DBCommandWrapper.CommandTimeout = 180

            If Transaction Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DS = DB.ExecuteDataSet(DBCommandWrapper, Transaction)
            End If

            Dim _dataTable As New DataTable("REG_MASTER")
            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0).Rows(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveBatchRows(ByVal BatchSql As String) As DataSet
        Dim DS As DataSet
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetSqlStringCommand(BatchSql)
            DBCommandWrapper.CommandTimeout = 120
            DS = DB.ExecuteDataSet(DBCommandWrapper)

            If DS.Tables.Count > 0 Then
                Return DS
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

End Class