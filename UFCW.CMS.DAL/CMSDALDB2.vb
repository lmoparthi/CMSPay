Imports DDTek.DB2
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.IO
Imports System.Data.Common



''' -----------------------------------------------------------------------------
''' Project	 : JobsDAL
''' Class	 : JobsDAL
''' 
''' -----------------------------------------------------------------------------
''' <summary>
''' Data Access Layer for the CMS UI
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[Nick Snyder]	4/26/2006	Created
''' </history>
''' -----------------------------------------------------------------------------

Public NotInheritable Class CMSDALDB2

    Public Const _DB2databaseName As String = "FDBMD"
    Private Shared ReadOnly _DDTEKLogDirectory As String = If(System.Configuration.ConfigurationManager.AppSettings("DDTEKLogDirectory"), Nothing)

    Public Shared _DB As Database
    Public Shared _DBTransaction As DbTransaction
    Public Shared _DBConnection As DbConnection

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
#Region "Constructor"
    Shared Sub New()

        Try

            CMSDALCommon.SetupDDTEKLogging(_DDTEKLogDirectory)

        Catch ex As Exception

            Throw

        End Try

    End Sub

    Private Sub New()

        Try

            CMSDALCommon.SetupDDTEKLogging(_DDTEKLogDirectory)

        Catch ex As Exception

            Throw

        End Try

    End Sub
#End Region
#Region "CRUD"

    Public Shared Function CreateSharedDatabase(ByVal oleProvider As String) As Database

        _DB = CMSDALCommon.CreateDatabase(oleProvider)

    End Function
    Public Shared Function CreateSharedConnection() As DbConnection

        Try
            _DBConnection = _DB.CreateConnection()

            _DBConnection.Open()

            Return _DBConnection

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function CreateSharedTransaction() As DbTransaction

        Try
            _DBTransaction = _DBConnection.BeginTransaction()

            Return _DBTransaction

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Sub CloseSharedConnection()

        Try

            If _DBConnection IsNot Nothing Then _DBConnection.Close()

            _DBConnection = Nothing
            _DBTransaction = Nothing
            _DB = Nothing

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Shared Sub CommitSharedTransaction()

        Try
            If _DBTransaction IsNot Nothing AndAlso _DBTransaction.Connection IsNot Nothing AndAlso _DBTransaction.Connection.State <> ConnectionState.Closed Then
                _DBTransaction.Commit()
            End If

        Catch ex As Exception
            Throw

        Finally

        End Try

    End Sub

    Public Shared Sub RollbackTransaction(ByVal transaction As DbTransaction)

        Try
            If _DBTransaction IsNot Nothing AndAlso _DBTransaction.Connection IsNot Nothing Then
                _DBTransaction.Rollback()
            End If

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Sub
    Public Shared Sub ExecuteSP(ByVal spName As String, ByVal spInputParms As Object)
        ExecuteSP(spName, spInputParms, 120)
    End Sub

    Public Shared Sub ExecuteSP(ByVal spName As String, ByVal spInputParms As Object, ByVal spTimeOut As Integer)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            DB = CMSDALCommon.CreateDatabase

            DBCommandWrapper = DB.GetStoredProcCommand(spName)

            If spInputParms IsNot Nothing AndAlso spInputParms.ToString.Trim.Length > 0 Then

                If TypeOf spInputParms Is SPParameter Then
                    DB.AddInParameter(DBCommandWrapper, DirectCast(spInputParms, SPParameter).ParameterName, DirectCast(spInputParms, SPParameter).ParameterType, DirectCast(spInputParms, SPParameter).ParameterContent)
                Else
                    Dim InputParms() As String = spInputParms.ToString.Split(If(spInputParms.ToString.Contains("|"), CChar("|"), CChar(","))).Select((Function(s) s.Trim)).ToArray
                    Dim ParamType As Integer

                    If InputParms.Any Then

                        For x As Integer = 0 To InputParms.Length - 1 Step 3

                            Select Case InputParms(x + 1).ToUpper
                                Case "DATE"
                                    ParamType = DbType.Date
                                Case "STRING"
                                    ParamType = DbType.String
                                Case "INTEGER", "INT"
                                    ParamType = DbType.Int32
                                Case "DECIMAL", "DEC"
                                    ParamType = DbType.Decimal
                                Case "SHORT"
                                    ParamType = DbType.Int16
                                Case "TIMESTAMP"
                                    ParamType = DbType.DateTime
                                Case "ARRAY"
                                    ParamType = DbType.String
                                Case Else
                                    Throw New ArgumentException("Unrecognized Stored Procedure Type Parameter (Name/Type(DATE,STRING,INTEGER,DECIMAL,SHORT,TIMESTAMP)/Value)", InputParms(x + 1))
                            End Select

                            DB.AddInParameter(DBCommandWrapper, InputParms(x), CType(ParamType, DbType), ResolveinputParms(InputParms, x + 2))
                        Next

                    End If
                End If
            End If

            DBCommandWrapper.CommandTimeout = spTimeOut

            DB.ExecuteDataSet(DBCommandWrapper)

        Catch ex As DB2Exception

            Throw

        Catch ex As Exception

            Throw

        Finally

            DBCommandWrapper = Nothing
            DB = Nothing

        End Try

    End Sub

    Public Shared Function ExecuteSPViaReaderWithResultSet(ByVal spName As String, ByVal spInputParms As Object, Optional ByVal spTimeOut As Integer = 120) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim DBDataReader As RefCountingDataReader
        Dim SchemaDT As DataTable

        Dim SQLCall As String = spName

        Try
            DB = CMSDALCommon.CreateDatabase()

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            If spInputParms IsNot Nothing AndAlso spInputParms.ToString.Trim.Length > 0 Then

                If TypeOf spInputParms Is SPParameter Then
                    DB.AddInParameter(DBCommandWrapper, DirectCast(spInputParms, SPParameter).ParameterName, DirectCast(spInputParms, SPParameter).ParameterType, DirectCast(spInputParms, SPParameter).ParameterContent)
                Else
                    Dim InputParms() As String = spInputParms.ToString.Split(If(spInputParms.ToString.Contains("|"), CChar("|"), CChar(","))).Select((Function(s) s.Trim)).ToArray
                    Dim ParamType As Integer

                    If InputParms.Any Then

                        For x As Integer = 0 To InputParms.Length - 1 Step 3

                            Select Case InputParms(x + 1).ToUpper
                                Case "DATE"
                                    ParamType = DbType.Date
                                Case "STRING"
                                    ParamType = DbType.String
                                Case "INTEGER", "INT"
                                    ParamType = DbType.Int32
                                Case "DECIMAL", "DEC"
                                    ParamType = DbType.Decimal
                                Case "SHORT"
                                    ParamType = DbType.Int16
                                Case "TIMESTAMP"
                                    ParamType = DbType.DateTime
                                Case "ARRAY"
                                    ParamType = DbType.Object
                                Case Else
                                    Throw New ArgumentException("Unrecognized Stored Procedure Type Parameter (Name/Type(DATE,STRING,INTEGER,DECIMAL,SHORT,TIMESTAMP)/Value)", InputParms(x + 1))
                            End Select

                            DB.AddInParameter(DBCommandWrapper, InputParms(x).ToString.Trim, CType(ParamType, DbType), ResolveinputParms(InputParms, x + 2))
                        Next

                    End If

                End If

            End If
            DBCommandWrapper.CommandTimeout = spTimeOut

            DBDataReader = CType(DB.ExecuteReader(DBCommandWrapper), RefCountingDataReader)

            SchemaDT = DBDataReader.InnerReader.GetSchemaTable

            If SchemaDT Is Nothing Then
                Throw New ApplicationException("Stored Procedure did not return a result set")
            End If

            SchemaDT.TableName = "Schema"

            Using ReaderDT As DataTable = CMSDALCommon.ConvertSchemaToDataTable(SchemaDT)

                Do While DBDataReader.Read

                    Dim DR As DataRow = ReaderDT.NewRow()
                    For I As Integer = 0 To ReaderDT.Columns.Count - 1
                        DR((CType(ReaderDT.Columns(I), DataColumn))) = DBDataReader.InnerReader(I)
                    Next I
                    ReaderDT.Rows.Add(DR)

                Loop

                Using ReturnDS As New DataSet

                    If ReaderDT IsNot Nothing Then
                        ReturnDS.Tables.Add(ReaderDT)
                    End If

                    If SchemaDT IsNot Nothing Then
                        SchemaDT.TableName = "Schema" & ReturnDS.Tables.Count.ToString
                        ReturnDS.Tables.Add(SchemaDT)
                    End If

                    If ReturnDS.Tables.Count > 0 Then
                        Return ReturnDS
                    Else
                        Return Nothing
                    End If

                End Using

            End Using

        Catch ex As ApplicationException

            Throw

        Catch ex As DB2Exception

            Throw

        Catch ex As Exception

            Throw

        Finally

            If DBDataReader IsNot Nothing Then DBDataReader.Close()
            DBDataReader = Nothing
            If DBCommandWrapper IsNot Nothing Then DBCommandWrapper.Dispose()
            DBCommandWrapper = Nothing

            DB = Nothing

        End Try
    End Function

    Public Shared Function ExecuteSPWithOutputParameters(ByVal spName As String, ByVal spInputParms As Object, ByVal spOutputParms As Object) As Object
        ExecuteSPWithOutputParameters(spName, spInputParms, spOutputParms, 120)
    End Function

    Public Shared Function ExecuteSPWithOutputParameters(ByVal spName As String, ByVal spInputParms As Object, ByVal spOutputParms As Object, ByVal SPTimeOut As Integer) As Object

        Dim DB As Database = DatabaseFactory.CreateDatabase
        Dim DBCommandWrapper As DbCommand
        Dim ReturnArray As List(Of Object)
        Dim outputParms() As String
        Dim ds As DataSet

        Try

            DBCommandWrapper = DB.GetStoredProcCommand(spName)

            If spInputParms IsNot Nothing AndAlso spInputParms.ToString.Trim.Length > 0 Then

                If TypeOf spInputParms Is SPParameter Then
                    DB.AddInParameter(DBCommandWrapper, DirectCast(spInputParms, SPParameter).ParameterName, DirectCast(spInputParms, SPParameter).ParameterType, DirectCast(spInputParms, SPParameter).ParameterContent)
                Else
                    Dim InputParms() As String = spInputParms.ToString.Split(If(spInputParms.ToString.Contains("|"), CChar("|"), CChar(","))).Select((Function(s) s.Trim)).ToArray
                    Dim ParamType As Integer

                    If inputParms.Any Then

                        For x As Integer = 0 To inputParms.Length - 1 Step 3

                            Select Case inputParms(x + 1).ToUpper
                                Case "DATE"
                                    ParamType = DbType.Date
                                Case "STRING"
                                    ParamType = DbType.String
                                Case "INTEGER", "INT"
                                    ParamType = DbType.Int32
                                Case "DECIMAL", "DEC"
                                    ParamType = DbType.Decimal
                                Case "SHORT"
                                    ParamType = DbType.Int16
                                Case "TIMESTAMP"
                                    ParamType = DbType.DateTime
                                Case Else
                                    Throw New ArgumentException("Unrecognized Stored Procedure Type Parameter (Name/Type(DATE,STRING,INTEGER,DECIMAL,SHORT,TIMESTAMP)/Value)", inputParms(x + 1))
                            End Select

                            DB.AddInParameter(DBCommandWrapper, inputParms(x), CType(ParamType, DbType), ResolveinputParms(inputParms, x + 2))
                        Next

                    End If
                End If
            End If

            If spOutputParms IsNot Nothing AndAlso spOutputParms.ToString.Trim.Length > 0 Then

                outputParms = spOutputParms.ToString.Split(If(spOutputParms.ToString.Contains("|"), CChar("|"), CChar(",")))
                Dim ParamType As Integer

                If outputParms.Any Then

                    For x As Integer = 0 To outputParms.Length - 1 Step 3

                        Select Case outputParms(x + 1).ToUpper
                            Case "DATE"
                                ParamType = DbType.Date
                            Case "STRING"
                                ParamType = DbType.String
                            Case "INTEGER"
                                ParamType = DbType.Int32
                            Case "DECIMAL"
                                ParamType = DbType.Decimal
                            Case "SHORT"
                                ParamType = DbType.Int16
                            Case "TIMESTAMP"
                                ParamType = DbType.DateTime
                            Case Else
                                Throw New ArgumentException("Unrecognized Stored Procedure Type Parameter (Name/Type(DATE,STRING,INTEGER,DECIMAL,SHORT,TIMESTAMP)/Size)", outputParms(x + 1))
                        End Select

                        DB.AddOutParameter(DBCommandWrapper, outputParms(x), CType(ParamType, DbType), CInt(outputParms(x + 2)))
                    Next

                End If
            End If

            DBCommandWrapper.CommandTimeout = SPTimeOut

            ds = DB.ExecuteDataSet(DBCommandWrapper)

            'Code to collect output parameters into an object array will be needed here

            If ds IsNot Nothing Then
                ReturnArray.Add(ds)
            End If

            If spOutputParms IsNot Nothing AndAlso spOutputParms.ToString.Trim.Length > 0 Then

                outputParms = spOutputParms.ToString.Split(If(spOutputParms.ToString.Contains("|"), CChar("|"), CChar(",")))

                If outputParms.Any Then

                    For x As Integer = 0 To outputParms.Length Step 3

                        If DB.GetParameterValue(DBCommandWrapper, outputParms(x)) Is System.DBNull.Value Then
                        Else
                            ReturnArray.Add(DB.GetParameterValue(DBCommandWrapper, outputParms(x)))
                        End If

                    Next

                End If
            End If

            Return ReturnArray

        Catch ex As DB2Exception

            Throw

        Catch ex As Exception

            Throw

        Finally

            DBCommandWrapper = Nothing
            DB = Nothing

        End Try

    End Function

    Public Shared Function ExecuteSPWithResultSet(ByVal spName As String, ByVal spInputParms As Object, Optional ByVal spTimeOut As Integer = 120) As DataTable

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = spName

        Try
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            If spInputParms IsNot Nothing AndAlso spInputParms.ToString.Trim.Length > 0 Then

                If TypeOf spInputParms Is SPParameter Then
                    DB.AddInParameter(DBCommandWrapper, DirectCast(spInputParms, SPParameter).ParameterName, DirectCast(spInputParms, SPParameter).ParameterType, DirectCast(spInputParms, SPParameter).ParameterContent)
                Else
                    Dim InputParms() As String = spInputParms.ToString.Split(If(spInputParms.ToString.Contains("|"), CChar("|"), CChar(","))).Select((Function(s) s.Trim)).ToArray
                    Dim ParamType As Integer

                    If InputParms.Any Then

                        For X As Integer = 0 To InputParms.Length - 1 Step 3

                            Select Case InputParms(X + 1).ToUpper
                                Case "DATE"
                                    ParamType = DbType.Date
                                Case "STRING"
                                    ParamType = DbType.String
                                Case "INTEGER", "INT"
                                    ParamType = DbType.Int32
                                Case "DECIMAL", "DEC"
                                    ParamType = DbType.Decimal
                                Case "SHORT"
                                    ParamType = DbType.Int16
                                Case "TIMESTAMP"
                                    ParamType = DbType.DateTime
                                Case Else
                                    Throw New ArgumentException("Unrecognized Stored Procedure Type Parameter (Name/Type(DATE,STRING,INTEGER,DECIMAL,SHORT,TIMESTAMP)/Value)", InputParms(X + 1))
                            End Select

                            DB.AddInParameter(DBCommandWrapper, InputParms(X).ToString, CType(ParamType, DbType), ResolveinputParms(InputParms, X + 2))
                        Next

                    End If
                End If
            End If

            DBCommandWrapper.CommandTimeout = spTimeOut

            Dim ds As DataSet = DB.ExecuteDataSet(DBCommandWrapper)

            If ds.Tables.Count > 0 Then
                Return ds.Tables(0)
            End If

            Return Nothing

        Catch ex As DB2Exception

            Throw

        Catch ex As Exception

            Throw

        Finally

            DBCommandWrapper = Nothing
            DB = Nothing

        End Try
    End Function

    Public Shared Function ExecuteSQLViaReaderWithResultSet(ByVal SQLQuery As String, Optional ByVal dBTimeOut As Integer = 120) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim DBDataReader As RefCountingDataReader
        Dim SchemaDT As DataTable

        Try

            DB = CMSDALCommon.CreateDatabase()

            If CBool(If(System.Configuration.ConfigurationManager.AppSettings("UseRUNIMMEDIATE") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("UseRUNIMMEDIATE")))) Then
                DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RUNIMMEDIATESELECTLOG")
                DB.AddInParameter(DBCommandWrapper, "SQLInput", DbType.String, SQLQuery)
            Else
                DBCommandWrapper = DB.GetSqlStringCommand(SQLQuery.ToString)
            End If

            DBCommandWrapper.CommandTimeout = dBTimeOut

            DBDataReader = CType(DB.ExecuteReader(DBCommandWrapper), RefCountingDataReader)

            Using ReturnDS As DataSet = New DataSet

                SchemaDT = DBDataReader.InnerReader.GetSchemaTable

                Dim ReaderDT As DataTable

                If SchemaDT IsNot Nothing Then ReaderDT = CMSDALCommon.ConvertSchemaToDataTable(SchemaDT)

                Do While DBDataReader.Read

                    Dim DR As DataRow = ReaderDT.NewRow()
                    For I As Integer = 0 To ReaderDT.Columns.Count - 1
                        DR((CType(ReaderDT.Columns(I), DataColumn))) = DBDataReader.InnerReader(I)
                    Next I
                    ReaderDT.Rows.Add(DR)

                Loop

                If ReaderDT IsNot Nothing Then
                    ReturnDS.Tables.Add(ReaderDT)
                End If

                If SchemaDT IsNot Nothing Then
                    SchemaDT.TableName = "Schema" & ReturnDS.Tables.Count.ToString
                    ReturnDS.Tables.Add(SchemaDT)
                End If

                'Note: This does not support executions containing multiple SQL requests. 

                Return If(ReturnDS.Tables.Count > 0, ReturnDS, Nothing)

            End Using

        Catch ex As DB2Exception

            Throw

        Catch ex As Exception

            Throw

        Finally

            If DBDataReader IsNot Nothing Then DBDataReader.Close()

            If DBCommandWrapper IsNot Nothing Then DBCommandWrapper.Dispose()
            DBCommandWrapper = Nothing

        End Try

    End Function

    Public Shared Function ExecuteSQLWithOutResultSet(ByVal SQLQuery As String, Optional ByVal dBTimeOut As Integer = 120, Optional ByVal dbTransaction As DbTransaction = Nothing) As Integer

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try
            If dbTransaction Is Nothing OrElse dbTransaction IsNot _DBTransaction Then
                DB = CMSDALCommon.CreateDatabase()
            Else
                DB = _DB
            End If

            DBCommandWrapper = DB.GetSqlStringCommand(SQLQuery)
            DBCommandWrapper.CommandTimeout = dBTimeOut

            If dbTransaction Is Nothing Then
                Return DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                Return DB.ExecuteNonQuery(DBCommandWrapper, dbTransaction)
            End If

        Catch ex As DB2Exception

            Throw

        Catch ex As Exception

            Throw

        Finally

            DBCommandWrapper = Nothing

            If dbTransaction Is Nothing OrElse dbTransaction IsNot _DBTransaction Then
                DB = Nothing
            End If

        End Try

    End Function

    Public Shared Function ExecuteSPWithReturnCount(ByVal SPName As String, Optional ByVal dBTimeOut As Integer = 120, Optional ByVal dbTransaction As DbTransaction = Nothing) As Integer

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim DBParm As DbParameter

        Dim DBReturn As Object

        Try
            If dbTransaction Is Nothing OrElse dbTransaction IsNot _DBTransaction Then
                DB = CMSDALCommon.CreateDatabase()
            Else
                DB = _DB
            End If

            DBCommandWrapper = DB.GetStoredProcCommand(SPName)
            DBCommandWrapper.CommandTimeout = dBTimeOut
            DBParm = DBCommandWrapper.CreateParameter()

            DBParm.Direction = ParameterDirection.ReturnValue
            DBParm.ParameterName = "RETURN_VALUE"

            DBCommandWrapper.Parameters.Add(DBParm)

            If dbTransaction Is Nothing Then
                DBReturn = DB.ExecuteScalar(DBCommandWrapper)
            Else
                DBReturn = DB.ExecuteScalar(DBCommandWrapper, dbTransaction)
            End If

            DBReturn = DB.GetParameterValue(DBCommandWrapper, "RETURN_VALUE")

            Return CInt(DBReturn)

        Catch ex As DB2Exception

            Throw

        Catch ex As Exception

            Throw

        Finally

            DBCommandWrapper = Nothing

            If dbTransaction Is Nothing OrElse dbTransaction IsNot _DBTransaction Then
                DB = Nothing
            End If

        End Try

    End Function
    Public Shared Function ExecuteSQLWithResultSet(ByVal SQLQuery As String, Optional ByVal dBTimeOut As Integer = 120) As DataTable

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand

        Try

            If CBool(If(System.Configuration.ConfigurationManager.AppSettings("UseRUNIMMEDIATE") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("UseRUNIMMEDIATE")))) Then
                DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RUNIMMEDIATESELECTLOG")
                DB.AddInParameter(DBCommandWrapper, "SQLInput", DbType.String, SQLQuery)
            Else
                DBCommandWrapper = DB.GetSqlStringCommand(SQLQuery.ToString)
            End If

            DBCommandWrapper.CommandTimeout = dBTimeOut

            Dim ds As DataSet = DB.ExecuteDataSet(DBCommandWrapper)

            If ds.Tables.Count > 0 Then
                Return ds.Tables(0)
            Else
                Return Nothing
            End If

        Catch ex As DB2Exception

            Throw

        Catch ex As Exception

            Throw

        Finally

            DBCommandWrapper = Nothing

        End Try

    End Function

    Public Shared Function ExportSPWithResultSet(ByVal fullName As String, ByVal excludeHeader As Boolean, ByVal spName As String, ByVal spInputParms As Object, Optional ByVal SPTimeOut As Integer = 120) As Long

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = spName

        Dim SW As StreamWriter
        Dim RowCount As Long = 0

        Try
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            If spInputParms IsNot Nothing AndAlso spInputParms.ToString.Trim.Length > 0 Then

                Dim InputParms() As String = spInputParms.ToString.Split(If(spInputParms.ToString.Contains("|"), CChar("|"), CChar(","))).Select((Function(s) s.Trim)).ToArray
                Dim ParamType As Integer

                If InputParms.Any Then

                    For X As Integer = 0 To InputParms.Length - 1 Step 3

                        Select Case InputParms(X + 1).ToUpper
                            Case "DATE"
                                ParamType = DbType.Date
                            Case "STRING"
                                ParamType = DbType.String
                            Case "INTEGER", "INT"
                                ParamType = DbType.Int32
                            Case "DECIMAL", "DEC"
                                ParamType = DbType.Decimal
                            Case "SHORT"
                                ParamType = DbType.Int16
                            Case "TIMESTAMP"
                                ParamType = DbType.DateTime
                            Case Else
                                Throw New ArgumentException("Unrecognized Stored Procedure Type Parameter (Name/Type(DATE,STRING,INTEGER,DECIMAL,SHORT,TIMESTAMP)/Value)", InputParms(X + 1))
                        End Select

                        DB.AddInParameter(DBCommandWrapper, InputParms(X), CType(ParamType, DbType), ResolveinputParms(InputParms, X + 2))
                    Next

                End If
            End If
            DBCommandWrapper.CommandTimeout = SPTimeOut

            Dim DS As DataSet = DB.ExecuteDataSet(DBCommandWrapper)

            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then

                Try
                    '' Create the TXTfile to which grid data will be exported.
                    SW = New StreamWriter(fullName, False)

                    For Each DR As DataRow In DS.Tables(0).Rows

                        For X As Integer = 0 To DS.Tables(0).Columns.Count - 1
                            SW.Write(If(X > 0, ",", "") & DR(X).ToString)
                        Next

                        SW.Write(SW.NewLine)

                        RowCount += 1

                    Next

                Catch ex As Exception
                    Throw
                Finally
                    If SW IsNot Nothing Then SW.Close()
                    SW = Nothing
                End Try

                Return RowCount

            End If

        Catch ex As DB2Exception

            Throw

        Catch ex As Exception

            Throw

        Finally

            DBCommandWrapper = Nothing
            DB = Nothing

        End Try
    End Function

    Public Shared Function ExportSPWithResultSet(ByVal fullName As String, ByVal spName As String, ByVal spInputParms As Object,
                                                 Optional ByVal csvUseQuotes As Boolean = False, Optional ByVal csvDelimitCharacter As String = ",",
                                                 Optional ByVal spTimeOut As Integer = 120) As Long

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = spName

        Dim SW As StreamWriter
        Dim RowCount As Long = 0
        Dim DoubleQuote As String

        Try
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            If spInputParms IsNot Nothing AndAlso spInputParms.ToString.Trim.Length > 0 Then

                If TypeOf spInputParms Is SPParameter Then
                    DB.AddInParameter(DBCommandWrapper, DirectCast(spInputParms, SPParameter).ParameterName, DirectCast(spInputParms, SPParameter).ParameterType, DirectCast(spInputParms, SPParameter).ParameterContent)
                Else
                    Dim InputParms() As String = spInputParms.ToString.Split(If(spInputParms.ToString.Contains("|"), CChar("|"), CChar(","))).Select((Function(s) s.Trim)).ToArray
                    Dim ParamType As Integer

                    If InputParms.Any Then

                        For X As Integer = 0 To InputParms.Length - 1 Step 3

                            Select Case InputParms(X + 1).ToUpper
                                Case "DATE"
                                    ParamType = DbType.Date
                                Case "STRING"
                                    ParamType = DbType.String
                                Case "INTEGER", "INT"
                                    ParamType = DbType.Int32
                                Case "DECIMAL", "DEC"
                                    ParamType = DbType.Decimal
                                Case "SHORT"
                                    ParamType = DbType.Int16
                                Case "TIMESTAMP"
                                    ParamType = DbType.DateTime
                                Case Else
                                    Throw New ArgumentException("Unrecognized Stored Procedure Type Parameter (Name/Type(DATE,STRING,INTEGER,DECIMAL,SHORT,TIMESTAMP)/Value)", InputParms(X + 1))
                            End Select

                            DB.AddInParameter(DBCommandWrapper, InputParms(X), CType(ParamType, DbType), ResolveinputParms(InputParms, X + 2))
                        Next

                    End If
                End If
            End If

            DBCommandWrapper.CommandTimeout = spTimeOut

            Dim DS As DataSet = DB.ExecuteDataSet(DBCommandWrapper)

            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then

                Try
                    '' Create the TXTfile to which grid data will be exported.
                    SW = New StreamWriter(fullName, False)

                    If csvDelimitCharacter.ToUpper.Trim = "TAB" Then
                        csvDelimitCharacter = Convert.ToChar(Windows.Forms.Keys.Tab)
                    End If

                    If csvDelimitCharacter = "" Then
                        csvDelimitCharacter = ","
                    End If

                    If csvUseQuotes Then
                        DoubleQuote = """"
                    End If

                    For Each DR As DataRow In DS.Tables(0).Rows

                        For X As Integer = 0 To DS.Tables(0).Columns.Count - 1
                            SW.Write(If(X > 0, csvDelimitCharacter, "") & DoubleQuote & DR(X).ToString & DoubleQuote)
                        Next

                        SW.Write(SW.NewLine)

                        RowCount += 1

                    Next

                Catch ex As Exception
                    Throw
                Finally
                    If SW IsNot Nothing Then SW.Close()
                    SW = Nothing
                End Try

                Return RowCount

            End If

        Catch ex As DB2Exception

            Throw

        Catch ex As Exception

            Throw

        Finally

            DBCommandWrapper = Nothing

        End Try
    End Function

    Public Shared Function ExportSQLResultsViaReader(ByVal FullName As String, ByVal excludeHeader As Boolean, ByVal SQLQuery As String, Optional ByVal SPTimeOut As Integer = 120) As Long

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim DBDataReader As RefCountingDataReader

        Dim SW As StreamWriter
        Dim RowCount As Long = 0

        Try

            If CBool(If(System.Configuration.ConfigurationManager.AppSettings("UseRUNIMMEDIATE") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("UseRUNIMMEDIATE")))) Then
                DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RUNIMMEDIATESELECTLOG")
                DB.AddInParameter(DBCommandWrapper, "SQLInput", DbType.String, SQLQuery)
            Else
                DBCommandWrapper = DB.GetSqlStringCommand(SQLQuery.ToString)
            End If

            DBCommandWrapper.CommandTimeout = SPTimeOut

            DBDataReader = CType(DB.ExecuteReader(DBCommandWrapper), RefCountingDataReader)

            Try
                '' Create the TXTfile to which grid data will be exported.
                SW = New StreamWriter(FullName, False)

                '' Now write all the rows.
                While DBDataReader.Read

                    For x As Integer = 0 To DBDataReader.FieldCount - 1
                        SW.Write(If(x > 0, ",", "") & If(Not DBDataReader.IsDBNull(x), DBDataReader.GetString(x), ""))
                    Next

                    SW.Write(SW.NewLine)

                    RowCount += 1

                End While

                Return RowCount

            Catch ex As Exception
                Throw
            Finally
                If SW IsNot Nothing Then SW.Close()
                SW = Nothing
            End Try

        Catch ex As DB2Exception

            Throw

        Catch ex As Exception

            Throw

        Finally

            If DBDataReader IsNot Nothing Then DBDataReader.Close()

            If DBCommandWrapper IsNot Nothing Then DBCommandWrapper.Dispose()
            DBCommandWrapper = Nothing

        End Try

    End Function

    Public Shared Function ExportSQLResultsViaReader(ByVal fullName As String, ByVal excludeHeader As Boolean, ByVal sqlQuery As String, Optional ByVal csvUseQuotes As Boolean = False,
                                                     Optional ByVal csvDelimitCharacter As String = ",", Optional ByVal spTimeOut As Integer = 120) As Long

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim DBDataReader As RefCountingDataReader

        Dim SW As StreamWriter
        Dim RowCount As Long
        Dim DoubleQuote As String
        Dim SchemaDT As DataTable

        Try

            If CBool(If(System.Configuration.ConfigurationManager.AppSettings("UseRUNIMMEDIATE") Is Nothing, 0, CInt(System.Configuration.ConfigurationManager.AppSettings("UseRUNIMMEDIATE")))) Then
                DBCommandWrapper = DB.GetStoredProcCommand("FDBMD.RUNIMMEDIATESELECTLOG")
                DB.AddInParameter(DBCommandWrapper, "SQLInput", DbType.String, sqlQuery)
            Else
                DBCommandWrapper = DB.GetSqlStringCommand(sqlQuery.ToString)
            End If

            DBCommandWrapper.CommandTimeout = spTimeOut

            DBDataReader = CType(DB.ExecuteReader(DBCommandWrapper), RefCountingDataReader)

            Try
                '' Create the TXTfile to which grid data will be exported.
                SW = New StreamWriter(fullName, False)

                If csvDelimitCharacter.ToUpper.Trim = "TAB" Then
                    csvDelimitCharacter = Convert.ToChar(Windows.Forms.Keys.Tab)
                End If

                If csvDelimitCharacter = "" Then
                    csvDelimitCharacter = ","
                End If

                If csvUseQuotes Then
                    DoubleQuote = """"
                End If

                If Not excludeHeader Then
                    SchemaDT = DBDataReader.GetSchemaTable()
                    For Each DR As DataRow In SchemaDT.Rows
                        'For each property of the field...
                        For Each DC As DataColumn In SchemaDT.Columns
                            SW.Write(DoubleQuote & DR(DC).ToString & DoubleQuote)
                            Exit For
                        Next
                        If CInt(DR(1).ToString) < (SchemaDT.Rows.Count - 1) Then SW.Write(csvDelimitCharacter)
                    Next
                    SW.Write(SW.NewLine)
                End If

                '' Now write all the rows.
                While DBDataReader.Read

                    For x As Integer = 0 To DBDataReader.FieldCount - 1
                        SW.Write(If(x > 0, csvDelimitCharacter, "") & DoubleQuote & If(Not DBDataReader.IsDBNull(x), DBDataReader.GetString(x), "") & DoubleQuote)
                    Next

                    SW.Write(SW.NewLine)

                    RowCount += 1

                End While

                Return RowCount

            Catch ex As Exception
                Throw
            Finally
                If SW IsNot Nothing Then SW.Close()
                SW = Nothing
            End Try

        Catch ex As DB2Exception

            Throw

        Catch ex As Exception

            Throw

        Finally

            If DBDataReader IsNot Nothing Then DBDataReader.Close()

            If DBCommandWrapper IsNot Nothing Then DBCommandWrapper.Dispose()
            DBCommandWrapper = Nothing

        End Try

    End Function

    Public Shared Function ExportSPResultsViaReader(ByVal fullName As String, ByVal spName As String, ByVal spInputParms As Object, Optional ByVal csvUseQuotes As Boolean = False,
                                                     Optional ByVal csvDelimitCharacter As String = ",", Optional ByVal spTimeOut As Integer = 120) As Long

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand

        Dim SW As StreamWriter
        Dim RowCount As Long
        Dim DoubleQuote As String

        Try

            DBCommandWrapper = DB.GetStoredProcCommand(spName)

            DBCommandWrapper.CommandTimeout = spTimeOut

            If spInputParms IsNot Nothing AndAlso spInputParms.ToString.Trim.Length > 0 Then

                If TypeOf spInputParms Is SPParameter Then
                    DB.AddInParameter(DBCommandWrapper, DirectCast(spInputParms, SPParameter).ParameterName, DirectCast(spInputParms, SPParameter).ParameterType, DirectCast(spInputParms, SPParameter).ParameterContent)
                Else
                    Dim InputParms() As String = spInputParms.ToString.Split(If(spInputParms.ToString.Contains("|"), CChar("|"), CChar(","))).Select((Function(s) s.Trim)).ToArray
                    Dim ParamType As Integer

                    If InputParms.Any Then

                        For X As Integer = 0 To InputParms.Length - 1 Step 3

                            Select Case InputParms(X + 1).ToUpper
                                Case "DATE"
                                    ParamType = DbType.Date
                                Case "STRING"
                                    ParamType = DbType.String
                                Case "INTEGER", "INT"
                                    ParamType = DbType.Int32
                                Case "DECIMAL", "DEC"
                                    ParamType = DbType.Decimal
                                Case "SHORT"
                                    ParamType = DbType.Int16
                                Case "TIMESTAMP"
                                    ParamType = DbType.DateTime
                                Case Else
                                    Throw New ArgumentException("Unrecognized Stored Procedure Type Parameter (Name/Type(DATE,STRING,INTEGER,DECIMAL,SHORT,TIMESTAMP)/Value)", InputParms(X + 1))
                            End Select

                            DB.AddInParameter(DBCommandWrapper, InputParms(X), CType(ParamType, DbType), ResolveinputParms(InputParms, X + 2))
                        Next

                    End If
                End If
            End If

            Try

                Using DBDataReader As RefCountingDataReader = CType(DB.ExecuteReader(DBCommandWrapper), RefCountingDataReader)

                    SW = New StreamWriter(fullName, False)

                    If csvDelimitCharacter.ToUpper.Trim = "TAB" Then
                        csvDelimitCharacter = Convert.ToChar(Windows.Forms.Keys.Tab)
                    End If

                    If csvDelimitCharacter = "" Then
                        csvDelimitCharacter = ","
                    End If

                    If csvUseQuotes Then
                        DoubleQuote = """"
                    End If

                    Dim SchemaDT As DataTable = DBDataReader.GetSchemaTable()

                    ' Output the schema information
                    For Each DR As DataRow In SchemaDT.Rows
                        'For each property of the field...
                        For Each DC As DataColumn In SchemaDT.Columns
                            SW.Write(DoubleQuote & DR(DC).ToString & DoubleQuote)
                            Exit For
                        Next
                        If CInt(DR(1).ToString) < (SchemaDT.Rows.Count - 1) Then SW.Write(csvDelimitCharacter)
                    Next
                    SW.Write(SW.NewLine)

                    ' Retrieve the data
                    While DBDataReader.Read()

                        RowCount += 1

                        For x As Integer = 0 To DBDataReader.FieldCount - 1
                            SW.Write(If(x > 0, csvDelimitCharacter, "") & DoubleQuote & If(Not DBDataReader.IsDBNull(x), DBDataReader.GetString(x), "") & DoubleQuote)
                        Next

                        SW.Write(SW.NewLine)

                    End While
                End Using

                Return RowCount

            Catch ex As Exception
                Throw
            Finally
                If SW IsNot Nothing Then SW.Close()
                SW = Nothing
            End Try

        Catch ex As DB2Exception

            Throw

        Catch ex As Exception

            Throw

        Finally

            If DBCommandWrapper IsNot Nothing Then DBCommandWrapper.Dispose()
            DBCommandWrapper = Nothing

        End Try

    End Function

    Private Shared Function ResolveinputParms(inputParms() As String, p1 As Integer) As Object

        Select Case True
            Case inputParms(p1).Contains("CURRENT")
                If inputParms(p1).ToString.Trim.Length <> 7 Then
                    Dim subparm As String()

                    If inputParms(p1).ToString.Contains("-") Then
                        subparm = inputParms(p1).ToString.Trim.Split(CChar("-"))
                        Return UFCWGeneral.NowDate.AddDays(-CInt(subparm(1))).ToString("yyyy-MM-dd")
                    ElseIf inputParms(p1).ToString.Contains("+") Then
                        Return UFCWGeneral.NowDate.AddDays(CInt(subparm(1))).ToString("yyyy-MM-dd")
                    End If

                Else
                    Return UFCWGeneral.NowDate.Date.ToString("yyyy-MM-dd")
                End If
            Case inputParms(p1).Contains("NULL")
                Return Nothing
            Case Else
                Return inputParms(p1)
        End Select

    End Function

#End Region

    Public Shared ReadOnly Property EligCutOff() As Date
        Get
            Dim SQL As String
            Dim DT As DataTable

            SQL = "SELECT TODAY FROM FDBIS.CALENDAR " &
                    "WHERE MONTHNUMBER = MONTH(CURRENT DATE) AND " &
                    "YEAR = YEAR(CURRENT DATE) AND " &
                    "ISELIGIBILITYCUTOFF = 1"
            Try
                DT = CMSDALDB2.ExecuteSQLWithResultSet(SQL, 600)
                If (DT IsNot Nothing AndAlso DT.Rows.Count > 0) Then
                    Return CDate(DT.Rows(0)(0))
                End If

            Catch ex As Exception
                Throw
            Finally
                If DT IsNot Nothing Then DT.Dispose()
                DT = Nothing
            End Try

        End Get
    End Property

    Public Shared ReadOnly Property PensionCutOff() As Date
        Get
            Dim SQL As String
            Dim DT As DataTable

            SQL = "SELECT TODAY FROM FDBIS.CALENDAR " &
                  "WHERE MONTHNUMBER = MONTH(CURRENT DATE) " &
                  "AND YEAR = YEAR(CURRENT DATE) " &
                  "AND ISPENSIONCUTOFF = 1"
            Try
                DT = CMSDALDB2.ExecuteSQLWithResultSet(SQL, 600)
                If (DT IsNot Nothing AndAlso DT.Rows.Count > 0) Then
                    Return CDate(DT.Rows(0)(0))
                End If
            Catch ex As Exception
                Throw
            Finally
                If DT IsNot Nothing Then DT.Dispose()
                DT = Nothing
            End Try

        End Get
    End Property

End Class

