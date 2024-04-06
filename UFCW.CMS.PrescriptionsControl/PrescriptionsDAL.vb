Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.IO
Imports System.Data.Common

Friend Class PrescriptionsDAL
    Public Const _DB2databaseName As String = "FDBHRA"
    Public Const _SQLdatabaseName As String = "dbo"
    Public Shared DomainUser As String = SystemInformation.UserName

#Region "Procedures/Functions"

    Public Shared Function CreateDataSetFromXML(XMLFile As String) As DataSet

        'load xml into a dataset to use here
        Dim dSet As New DataSet
        Dim Fs As FileStream = Nothing

        'open the xml file so we can use it to fill the dataset
        Try
            Fs = New FileStream(System.Windows.Forms.Application.StartupPath & "\" & XMLFile, FileMode.Open, FileAccess.Read)
        Catch ex As Exception

	Throw
        End Try

        'fill the dataset
        Try
            dSet.ReadXml(Fs)

            If Not dSet.Tables("Column").Columns.Contains("Visible") Then dSet.Tables("Column").Columns.Add("Visible")
            If Not dSet.Tables("Column").Columns.Contains("FormatIsRegEx") Then dSet.Tables("Column").Columns.Add("FormatIsRegEx")
            If Not dSet.Tables("Column").Columns.Contains("WordWrap") Then dSet.Tables("Column").Columns.Add("WordWrap")
            If Not dSet.Tables("Column").Columns.Contains("ReadOnly") Then dSet.Tables("Column").Columns.Add("ReadOnly")
            If Not dSet.Tables("Column").Columns.Contains("MinimumCharWidth") Then dSet.Tables("Column").Columns.Add("MinimumCharWidth")
            If Not dSet.Tables("Column").Columns.Contains("MaximumCharWidth") Then dSet.Tables("Column").Columns.Add("MaximumCharWidth")

            Return dSet

        Catch ex As Exception

	Throw
            Return Nothing
        Finally
            Fs.Close()
        End Try

    End Function

    Public Shared Function GetPrescriptionsInformation(ByVal PresFromdate As Date, ByVal PresTodate As Date, ByVal familyID As Integer?, Optional ByVal relationID As Short? = Nothing, Optional ByVal DS As DataSet = Nothing, Optional ByVal Transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RETRIEVE_PRESCRIPTIONS_DATA_BY_FAMILYID_RELATIONID")

            DB.AddInParameter(DBCommandWrapper, "@FAMILYID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATIONID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@PRESCRIPTIONS_FROMDATE", DbType.Date, PresFromdate)
            DB.AddInParameter(DBCommandWrapper, "@PRESCRIPTIONS_TODATE", DbType.Date, PresTodate)
            DBCommandWrapper.CommandTimeout = 180

            If DS Is Nothing Then
                If Transaction Is Nothing Then
                    DS = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    DS = DB.ExecuteDataSet(DBCommandWrapper, Transaction)
                End If

                DS.Tables(0).TableName = "Prescriptions"

            Else
                If Transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, DS, "Prescriptions")
                Else
                    DB.LoadDataSet(DBCommandWrapper, DS, "Prescriptions", Transaction)
                End If
            End If

            Return DS

        Catch ex As Exception

	Throw
        End Try
    End Function

    Public Shared Function GetPrescriptionsInformation(ByVal ssn As Integer?, ByVal presFromdate As Date, ByVal presTodate As Date, Optional ByVal ds As DataSet = Nothing, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try

            DB = CMSDALCommon.CreateDatabase()
            DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RETRIEVE_PRESCRIPTIONS_DATA_BY_SSN")

            DB.AddInParameter(DBCommandWrapper, "@SSN", DbType.Int32, If(ssn Is Nothing, Nothing, CType(ssn, Integer?)))
            DB.AddInParameter(DBCommandWrapper, "@PRESCRIPTIONS_FROMDATE", DbType.Date, presFromdate)
            DB.AddInParameter(DBCommandWrapper, "@PRESCRIPTIONS_TODATE", DbType.Date, presTodate)
            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If

                ds.Tables(0).TableName = "Prescriptions"

            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "Prescriptions")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "Prescriptions", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception

	Throw
        End Try
    End Function

#End Region

End Class