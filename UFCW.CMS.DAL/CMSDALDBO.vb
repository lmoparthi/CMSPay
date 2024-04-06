Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data.Common

Public NotInheritable Class CMSDALDBO

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private Sub New()

    End Sub

    Public Shared Sub CreateHistory(ByVal dbInstance As String, ByVal documentID As Long?, ByVal transactionType As String,
                                    ByVal partSSN As Integer?, ByVal docClass As String,
                                    ByVal docType As String, ByVal summary As String, ByVal detail As String, ByVal userID As String, ByVal claimID As Integer?,
                                    ByVal familyID As Integer?, ByVal relationID As Short?, ByVal patSSN As Integer?,
                                    Optional ByVal dbTransaction As DbTransaction = Nothing)


        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "ImgWorkFlow.dbo.CREATE_DOC_HISTORY"

        Try

            DB = CMSDALCommon.CreateDatabase(dbInstance)

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, claimID)
            DB.AddInParameter(DBCommandWrapper, "@DOCID", DbType.Decimal, documentID)
            DB.AddInParameter(DBCommandWrapper, "@TRANSACTION_TYPE", DbType.String, transactionType)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, partSSN)
            DB.AddInParameter(DBCommandWrapper, "@PAT_SSN", DbType.Int32, patSSN)
            DB.AddInParameter(DBCommandWrapper, "@DOC_CLASS", DbType.String, docClass)
            DB.AddInParameter(DBCommandWrapper, "@DOC_TYPE", DbType.String, docType)
            DB.AddInParameter(DBCommandWrapper, "@SUMMARY", DbType.String, summary)
            DB.AddInParameter(DBCommandWrapper, "@DETAIL", DbType.String, detail)
            DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String, userID)

            If dbTransaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, dbTransaction)
            End If

        Catch ex As Exception
            Throw

        Finally

        End Try
    End Sub

    Public Shared Sub CreateUFCWDocs(ByVal dbInstance As String, ByRef maxID As String, ByRef documentID As Long?, ByVal docClass As String,
                                     ByVal securitySW As Decimal, ByVal recDate As Date?, ByVal docType As String, ByVal archiveSW As Decimal,
                                     ByVal itemStatus As String, ByVal duplicateSW As Decimal, ByVal dupOriginal As Integer?,
                                     ByVal claimType As String, ByVal partSSN As Integer?, ByVal partLName As String,
                                     ByVal partInt As String, ByVal partFName As String, ByVal partEmployer As String, ByVal patLName As String,
                                     ByVal patInt As String, ByVal patFName As String, ByVal provPrefix As String, ByVal provID As Integer?,
                                     ByVal provLicense As String, ByVal provSuffix As String, ByVal dateOfService As Date?, ByVal scanDate As Date?,
                                     ByVal pageCount As Integer?, ByVal batch As String, ByVal indexDate As Date?,
                                     ByVal checkAccountNumber As Integer?, ByVal checkAmt As Decimal?, ByVal checkCode As String, ByVal checkDate As Date?,
                                     ByVal checkNumber As String, ByVal pstPeriod As Date?, ByVal memStatus As String, ByVal coverageType As String,
                                     ByVal paymentMethod As String, ByVal eligStatus As String, ByVal premiumAuthSignature As Decimal, ByVal letterID As Integer?,
                                     ByVal attachSW As Decimal, ByVal adjusterName As String, ByVal completedBy As String, ByVal lastUpdatedBy As String,
                                     ByVal familyID As Integer?, ByVal relationID As Short?,
                                     Optional ByVal dbTransaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "ImgWorkFlow.dbo.CreateUFCWDocs"

        Try

            DB = CMSDALCommon.CreateDatabase(dbInstance)

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@MAXID", DbType.String, maxID)
            DB.AddInParameter(DBCommandWrapper, "@DOCID", DbType.Decimal, documentID)
            DB.AddInParameter(DBCommandWrapper, "@DOC_CLASS", DbType.String, docClass)
            DB.AddInParameter(DBCommandWrapper, "@SECURITY_SW", DbType.Byte, securitySW)
            DB.AddInParameter(DBCommandWrapper, "@REC_DATE", DbType.DateTime, recDate)
            DB.AddInParameter(DBCommandWrapper, "@DOC_TYPE", DbType.String, docType)
            DB.AddInParameter(DBCommandWrapper, "@ARCHIVE_SW", DbType.Byte, archiveSW)
            DB.AddInParameter(DBCommandWrapper, "@ITEM_STATUS", DbType.String, itemStatus)
            DB.AddInParameter(DBCommandWrapper, "@DUPLICATE", DbType.Byte, duplicateSW)
            DB.AddInParameter(DBCommandWrapper, "@DUPORIGINAL", DbType.Int32, dupOriginal)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_TYPE", DbType.String, claimType)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, partSSN)

            If (IsDBNull(partLName) OrElse partLName Is Nothing OrElse partLName.Trim.Length = 0) Then
                partLName = Nothing
            Else
                Replace(partLName, "'", "''")
            End If
            DB.AddInParameter(DBCommandWrapper, "@PART_LNAME", DbType.String, partLName)

            If (IsDBNull(partInt) OrElse partInt Is Nothing OrElse partInt.Trim.Length = 0) Then
                partInt = Nothing
            Else
                Replace(partInt, "'", "''")
            End If
            DB.AddInParameter(DBCommandWrapper, "@PART_INT", DbType.String, partInt)

            If (IsDBNull(partFName) OrElse partFName Is Nothing OrElse partFName.Trim.Length = 0) Then
                partFName = Nothing
            Else
                Replace(partFName, "'", "''")
            End If
            DB.AddInParameter(DBCommandWrapper, "@PART_FNAME", DbType.String, partFName)

            If (IsDBNull(partEmployer) OrElse partEmployer Is Nothing OrElse partEmployer.Trim.Length = 0) Then
                partEmployer = Nothing
            Else
                Replace(partEmployer, "'", "''")
            End If
            DB.AddInParameter(DBCommandWrapper, "@PART_EMPLOYER", DbType.String, partEmployer)

            If (IsDBNull(patLName) OrElse patLName Is Nothing OrElse patLName.Trim.Length = 0) Then
                patLName = Nothing
            Else
                Replace(patLName, "'", "''")
            End If
            DB.AddInParameter(DBCommandWrapper, "@PAT_LNAME", DbType.String, patLName)

            If (IsDBNull(patInt) OrElse patInt Is Nothing OrElse patInt.Trim.Length = 0) Then
                patInt = Nothing
            Else
                Replace(patInt, "'", "''")
            End If
            DB.AddInParameter(DBCommandWrapper, "@PAT_INT", DbType.String, patInt)

            If (IsDBNull(patFName) OrElse patFName Is Nothing OrElse patFName.Trim.Length = 0) Then
                patFName = Nothing
            Else
                Replace(patFName, "'", "''")
            End If
            DB.AddInParameter(DBCommandWrapper, "@PAT_FNAME", DbType.String, patFName)

            DB.AddInParameter(DBCommandWrapper, "@PROV_PREFIX", DbType.String, provPrefix)
            DB.AddInParameter(DBCommandWrapper, "@PROV_ID", DbType.Int32, provID)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LICENSE", DbType.String, provLicense)
            DB.AddInParameter(DBCommandWrapper, "@PROV_SUFFIX", DbType.String, provSuffix)
            DB.AddInParameter(DBCommandWrapper, "@DATE_OF_SERVICE", DbType.Date, UFCWGeneral.ToNullDateHandler(dateOfService))
            DB.AddInParameter(DBCommandWrapper, "@SCAN_DATE", DbType.DateTime, UFCWGeneral.ToNullDateHandler(scanDate))
            DB.AddInParameter(DBCommandWrapper, "@PAGE_COUNT", DbType.Int32, pageCount)
            DB.AddInParameter(DBCommandWrapper, "@BATCH", DbType.String, batch)
            DB.AddInParameter(DBCommandWrapper, "@INDEX_DATE", DbType.DateTime, UFCWGeneral.ToNullDateHandler(indexDate))
            DB.AddInParameter(DBCommandWrapper, "@CHECK_ACCOUNT_NUMBER", DbType.Int32, checkAccountNumber)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_AMT", DbType.Currency, checkAmt)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_CODE", DbType.String, checkCode)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_DATE", DbType.DateTime, UFCWGeneral.ToNullDateHandler(checkDate))
            DB.AddInParameter(DBCommandWrapper, "@CHECK_NUMBER", DbType.String, checkNumber)
            DB.AddInParameter(DBCommandWrapper, "@PSTPERIOD", DbType.DateTime, UFCWGeneral.ToNullDateHandler(pstPeriod))
            DB.AddInParameter(DBCommandWrapper, "@MEM_STATUS", DbType.String, memStatus)
            DB.AddInParameter(DBCommandWrapper, "@COVERAGE_TYPE", DbType.String, coverageType)
            DB.AddInParameter(DBCommandWrapper, "@PAYMENT_METHOD", DbType.String, paymentMethod)

            If (IsDBNull(eligStatus) OrElse IsNothing(eligStatus) OrElse eligStatus.ToString.Trim.Length = 0) Then
                eligStatus = Nothing
            Else
                Replace(eligStatus, "'", "''")
            End If
            DB.AddInParameter(DBCommandWrapper, "@ELIG_STATUS", DbType.String, eligStatus)

            DB.AddInParameter(DBCommandWrapper, "@PREMIUM_AUTH_SIGNATURE", DbType.Byte, premiumAuthSignature)

            DB.AddInParameter(DBCommandWrapper, "@LETTERID", DbType.Int32, letterID)
            DB.AddInParameter(DBCommandWrapper, "@COMPLETED_BY", DbType.String, completedBy)

            DB.AddInParameter(DBCommandWrapper, "@ATTACH_SW", DbType.Byte, attachSW)

            DB.AddInParameter(DBCommandWrapper, "@ADJUSTER_NAME", DbType.String, adjusterName)
            DB.AddInParameter(DBCommandWrapper, "@FAMILYID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATIONID", DbType.Int16, relationID)

            If dbTransaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, dbTransaction)
            End If

        Catch ex As Exception
            Throw

        Finally

        End Try
    End Sub

    Public Shared Sub CreateUFCWMaster(ByVal dbInstance As String, ByRef maxID As String, ByRef documentID As Long?, ByVal docClass As String,
                                     ByVal securitySW As Decimal, ByVal recDate As Date?, ByVal docType As String, ByVal archiveSW As Decimal,
                                     ByVal itemStatus As String, ByVal duplicateSW As Decimal, ByVal dupOriginal As Integer?,
                                     ByVal claimType As String, ByVal partSSN As Integer?, ByVal partLName As String,
                                     ByVal partInt As String, ByVal partFName As String, ByVal partEmployer As String, ByVal patLName As String,
                                     ByVal patInt As String, ByVal patFName As String, ByVal provPrefix As String, ByVal provID As Integer?,
                                     ByVal provLicense As String, ByVal provSuffix As String, ByVal dateOfService As Date?, ByVal scanDate As Date?,
                                     ByVal pageCount As Integer?, ByVal batch As String, ByVal indexDate As Date?,
                                     ByVal checkAccountNumber As Integer?, ByVal checkAmt As Decimal?, ByVal checkCode As String, ByVal checkDate As Date?,
                                     ByVal checkNumber As String, ByVal pstPeriod As Date?, ByVal memStatus As String, ByVal coverageType As String,
                                     ByVal paymentMethod As String, ByVal eligStatus As String, ByVal premiumAuthSignature As Decimal, ByVal letterID As Integer?,
                                     ByVal attachSW As Decimal, ByVal adjusterName As String, ByVal completedBy As String, ByVal lastUpdatedBy As String,
                                     Optional ByRef dbTransaction As DbTransaction = Nothing)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "ImgWorkFlow.dbo.CREATE_UFCWDOCS"

        Try

            DB = CMSDALCommon.CreateDatabase(dbInstance)

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@MAXID", DbType.String, maxID)
            DB.AddInParameter(DBCommandWrapper, "@DOCID", DbType.Decimal, documentID)
            DB.AddInParameter(DBCommandWrapper, "@DOC_CLASS", DbType.String, docClass)
            DB.AddInParameter(DBCommandWrapper, "@SECURITY_SW", DbType.Byte, securitySW)
            DB.AddInParameter(DBCommandWrapper, "@REC_DATE", DbType.DateTime, recDate)
            DB.AddInParameter(DBCommandWrapper, "@DOC_TYPE", DbType.String, docType)
            DB.AddInParameter(DBCommandWrapper, "@ARCHIVE_SW", DbType.Byte, archiveSW)
            DB.AddInParameter(DBCommandWrapper, "@ITEM_STATUS", DbType.String, itemStatus)
            DB.AddInParameter(DBCommandWrapper, "@DUPLICATE", DbType.Byte, duplicateSW)
            DB.AddInParameter(DBCommandWrapper, "@DUPORIGINAL", DbType.Int32, dupOriginal)
            DB.AddInParameter(DBCommandWrapper, "@CLAIM_TYPE", DbType.String, claimType)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, partSSN)

            If (IsDBNull(partLName) OrElse partLName Is Nothing OrElse partLName.Trim.Length = 0) Then
                partLName = Nothing
            Else
                Replace(partLName, "'", "''")
            End If
            DB.AddInParameter(DBCommandWrapper, "@PART_LNAME", DbType.String, partLName)

            If (IsDBNull(partInt) OrElse IsNothing(partInt) OrElse partInt.Trim.Length = 0) Then
                partInt = Nothing
            Else
                Replace(partInt, "'", "''")
            End If
            DB.AddInParameter(DBCommandWrapper, "@PART_INT", DbType.String, partInt)

            If (IsDBNull(partFName) OrElse partFName Is Nothing OrElse partFName.Trim.Length = 0) Then
                partFName = Nothing
            Else
                Replace(partFName, "'", "''")
            End If
            DB.AddInParameter(DBCommandWrapper, "@PART_FNAME", DbType.String, partFName)

            If (IsDBNull(partEmployer) OrElse partEmployer Is Nothing OrElse partEmployer.Trim.Length = 0) Then
                partEmployer = Nothing
            Else
                Replace(partEmployer, "'", "''")
            End If
            DB.AddInParameter(DBCommandWrapper, "@PART_EMPLOYER", DbType.String, partEmployer)

            If (IsDBNull(patLName) OrElse patLName Is Nothing OrElse patLName.Trim.Length = 0) Then
                patLName = Nothing
            Else
                Replace(patLName, "'", "''")
            End If
            DB.AddInParameter(DBCommandWrapper, "@PAT_LNAME", DbType.String, patLName)

            If (IsDBNull(patInt) OrElse patInt Is Nothing OrElse patInt.Trim.Length = 0) Then
                patInt = Nothing
            Else
                Replace(patInt, "'", "''")
            End If
            DB.AddInParameter(DBCommandWrapper, "@PAT_INT", DbType.String, patInt)

            If (IsDBNull(patFName) OrElse patFName Is Nothing OrElse patFName.Trim.Length = 0) Then
                patFName = Nothing
            Else
                Replace(patFName, "'", "''")
            End If
            DB.AddInParameter(DBCommandWrapper, "@PAT_FNAME", DbType.String, patFName)

            DB.AddInParameter(DBCommandWrapper, "@PROV_PREFIX", DbType.String, provPrefix)
            DB.AddInParameter(DBCommandWrapper, "@PROV_ID", DbType.Int32, provID)
            DB.AddInParameter(DBCommandWrapper, "@PROV_LICENSE", DbType.String, provLicense)
            DB.AddInParameter(DBCommandWrapper, "@PROV_SUFFIX", DbType.String, provSuffix)
            DB.AddInParameter(DBCommandWrapper, "@DATE_OF_SERVICE", DbType.Date, dateOfService)
            DB.AddInParameter(DBCommandWrapper, "@SCAN_DATE", DbType.DateTime, scanDate)
            DB.AddInParameter(DBCommandWrapper, "@PAGE_COUNT", DbType.Int32, pageCount)
            DB.AddInParameter(DBCommandWrapper, "@BATCH", DbType.String, batch)
            DB.AddInParameter(DBCommandWrapper, "@INDEX_DATE", DbType.DateTime, indexDate)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_ACCOUNT_NUMBER", DbType.Int32, checkAccountNumber)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_AMT", DbType.Currency, checkAmt)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_CODE", DbType.String, checkCode)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_DATE", DbType.DateTime, checkDate)
            DB.AddInParameter(DBCommandWrapper, "@CHECK_NUMBER", DbType.String, checkNumber)
            DB.AddInParameter(DBCommandWrapper, "@PSTPERIOD", DbType.DateTime, pstPeriod)
            DB.AddInParameter(DBCommandWrapper, "@MEM_STATUS", DbType.String, memStatus)
            DB.AddInParameter(DBCommandWrapper, "@COVERAGE_TYPE", DbType.String, coverageType)
            DB.AddInParameter(DBCommandWrapper, "@PAYMENT_METHOD", DbType.String, paymentMethod)

            If (IsDBNull(eligStatus) OrElse eligStatus Is Nothing OrElse eligStatus.ToString.Trim.Length = 0) Then
                eligStatus = Nothing
            Else
                Replace(eligStatus, "'", "''")
            End If
            DB.AddInParameter(DBCommandWrapper, "@ELIG_STATUS", DbType.String, eligStatus)

            DB.AddInParameter(DBCommandWrapper, "@PREMIUM_AUTH_SIGNATURE", DbType.Byte, premiumAuthSignature)

            DB.AddInParameter(DBCommandWrapper, "@LETTERID", DbType.Int32, letterID)
            DB.AddInParameter(DBCommandWrapper, "@COMPLETED_BY", DbType.String, completedBy)

            DB.AddInParameter(DBCommandWrapper, "@ATTACH_SW", DbType.Byte, attachSW)

            DB.AddInParameter(DBCommandWrapper, "@ADJUSTER_NAME", DbType.String, adjusterName)

            If dbTransaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, dbTransaction)
            End If

        Catch ex As Exception
            Throw
        Finally

        End Try
    End Sub

    Public Shared Sub ExecuteSP(ByVal dbInstance As String, ByVal spName As String, ByVal spInputParms As Object, Optional ByVal spTimeOut As Integer = 120)

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try

            DB = CMSDALCommon.CreateDatabase(dbInstance)
            SQLCall = CMSDALCommon.GetDatabaseName(dbInstance) & "." & spName

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
                                Case "INTEGER"
                                    ParamType = DbType.Int32
                                Case "DECIMAL"
                                    ParamType = DbType.Decimal
                                Case "SHORT"
                                    ParamType = DbType.Int16
                                Case "TIMESTAMP"
                                    ParamType = DbType.DateTime
                                Case Else
                                    Throw New ArgumentException("Unrecognized Stored Procedure Type Parameter (Name/Type(DATE,STRING,INTEGER,DECIMAL,SHORT,TIMESTAMP)/Value)", InputParms(X + 1))
                            End Select

                            DB.AddInParameter(DBCommandWrapper, InputParms(X), CType(ParamType, DbType), ResolveinputParms(InputParms, X + 1))
                        Next

                    End If
                End If

            End If

            DBCommandWrapper.CommandTimeout = spTimeOut

            DB.ExecuteNonQuery(DBCommandWrapper)

        Catch ex As Exception

            Throw

        Finally
            If DBCommandWrapper.Connection IsNot Nothing Then
                DBCommandWrapper.Connection.Close()
                DBCommandWrapper.Connection.Dispose()
            End If
        End Try

    End Sub

    Public Shared Function ExecuteSPWithOutputParameters(ByVal dbInstance As String, ByVal spName As String, ByVal spInputParms As Object, ByVal spOutputParms As Object, Optional ByVal spTimeOut As Integer = 120) As Object

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim DBDataReader As RefCountingDataReader
        Dim SQLCall As String
        Dim ReturnArray As List(Of Object)
        Dim DT As DataTable

        Dim ParamType As Integer

        Try

            DB = CMSDALCommon.CreateDatabase(dbInstance)
            SQLCall = CMSDALCommon.GetDatabaseName(dbInstance) & "." & spName

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            If spInputParms IsNot Nothing AndAlso spInputParms.ToString.Trim.Length > 0 Then

                Dim InputParms() As String = spInputParms.ToString.Split(If(spInputParms.ToString.Contains("|"), CChar("|"), CChar(",")))

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

            If spOutputParms IsNot Nothing AndAlso spOutputParms.ToString.Trim.Length > 0 Then

                Dim OutputParms() As String = spOutputParms.ToString.Split(If(spOutputParms.ToString.Contains("|"), CChar("|"), CChar(",")))

                If OutputParms.Any Then

                    For X As Integer = 0 To OutputParms.Length - 1 Step 3

                        Select Case OutputParms(X + 1).ToUpper
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
                                Throw New ArgumentException("Unrecognized Stored Procedure Type Parameter (Name/Type(DATE,STRING,INTEGER,DECIMAL,SHORT,TIMESTAMP)/Value)", OutputParms(X + 1))
                        End Select

                        DB.AddOutParameter(DBCommandWrapper, OutputParms(X), CType(ParamType, DbType), CInt(OutputParms(X + 2)))
                    Next

                End If
            End If

            DBCommandWrapper.CommandTimeout = 180

            DT = New DataTable

            DBDataReader = CType(DB.ExecuteReader(DBCommandWrapper), RefCountingDataReader)

            DT.Load(DBDataReader)

            If DT IsNot Nothing Then
                ReturnArray.Add(DT)
            End If

            If spOutputParms IsNot Nothing AndAlso spOutputParms.ToString.Trim.Length > 0 Then

                Dim outputParms() As String = spOutputParms.ToString.Split(If(spOutputParms.ToString.Contains("|"), CChar("|"), CChar(",")))

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

        Catch ex As Exception

            Throw

        Finally

            If DBDataReader IsNot Nothing Then DBDataReader.Close()
            DBDataReader = Nothing

            If DBCommandWrapper.Connection IsNot Nothing Then
                DBCommandWrapper.Connection.Close()
                DBCommandWrapper.Connection.Dispose()
            End If

            If DT IsNot Nothing Then DT.Dispose()

        End Try

    End Function

    Public Shared Function ExecuteSPWithResultSet(ByVal dbInstance As String, ByVal spName As String, ByVal spInputParms As Object, Optional ByVal spTimeOut As Integer = 120) As DataTable

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim DBDataReader As RefCountingDataReader
        Dim SQLCall As String
        Dim DT As DataTable

        Try

            DB = CMSDALCommon.CreateDatabase(dbInstance)
            SQLCall = CMSDALCommon.GetDatabaseName(dbInstance) & "." & spName
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

            DBCommandWrapper.CommandTimeout = 180

            DT = New DataTable

            DBDataReader = CType(DB.ExecuteReader(DBCommandWrapper), RefCountingDataReader)

            DT.Load(DBDataReader)

            Return DT

        Catch ex As Exception

            Throw

        Finally

            If DBDataReader IsNot Nothing Then DBDataReader.Close()
            DBDataReader = Nothing

            If DBCommandWrapper.Connection IsNot Nothing Then
                DBCommandWrapper.Connection.Close()
                DBCommandWrapper.Connection.Dispose()
            End If

            If DT IsNot Nothing Then DT.Dispose()

        End Try

    End Function

    Public Shared Function ExecuteSQLViaReaderWithResultSet(ByVal dbInstance As String, ByVal sqlQuery As String, Optional ByVal sqlTimeOut As Integer = 120) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim DBDataReader As RefCountingDataReader
        Dim DS As DataSet
        Dim ReturnDS As DataSet
        Dim ReaderDT As DataTable
        Dim SchemaDT As DataTable

        Try

            DB = CMSDALCommon.CreateDatabase(dbInstance)

            DBCommandWrapper = DB.GetSqlStringCommand(sqlQuery)
            DBCommandWrapper.CommandTimeout = sqlTimeOut

            DS = DB.ExecuteDataSet(DBCommandWrapper)
            DBDataReader = CType(DB.ExecuteReader(DBCommandWrapper), RefCountingDataReader)

            ReturnDS = New DataSet

            SchemaDT = DBDataReader.InnerReader.GetSchemaTable

            ReaderDT = New DataTable

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

            Return If(ReturnDS.Tables.Count > 0, ReturnDS, Nothing)

        Catch ex As Exception

            Throw

        Finally

            If DBDataReader IsNot Nothing Then DBDataReader.Close()
            DBDataReader = Nothing

            If ReaderDT IsNot Nothing Then ReaderDT.Dispose()
            ReaderDT = Nothing

            If SchemaDT IsNot Nothing Then SchemaDT.Dispose()
            SchemaDT = Nothing

        End Try

    End Function

    Public Shared Function ExecuteSQLWithOutResultSet(ByVal dbInstance As String, ByVal sqlQuery As String, Optional ByVal sqlTimeOut As Integer = 120, Optional ByVal dbTransaction As DbTransaction = Nothing) As Integer

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand

        Try

            DB = CMSDALCommon.CreateDatabase(dbInstance)

            DBCommandWrapper = DB.GetSqlStringCommand(sqlQuery)
            DBCommandWrapper.CommandTimeout = sqlTimeOut

            If dbTransaction Is Nothing Then
                Return DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                Return DB.ExecuteNonQuery(DBCommandWrapper, dbTransaction)
            End If

        Catch ex As Exception

            Throw

        End Try

    End Function

    Public Shared Function ExecuteSQLWithResultSet(ByVal dbInstance As String, ByVal sqlQuery As String, Optional ByVal sqlTimeOut As Integer = 120) As DataTable

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim DS As DataSet

        Try

            DB = CMSDALCommon.CreateDatabase(dbInstance)

            DBCommandWrapper = DB.GetSqlStringCommand(sqlQuery)
            DBCommandWrapper.CommandTimeout = sqlTimeOut

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            If DS.Tables.Count > 0 Then
                Return DS.Tables(0)
            Else
                Return Nothing
            End If

        Catch ex As Exception

            Throw

        End Try

    End Function

    Public Shared Function ExecuteSSIS(ByVal dbInstance As String, ByVal ssisPackageName As String, Optional ByVal timeOut As Integer = 120) As Integer

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLQuery As String

        Try

            DB = CMSDALCommon.CreateDatabase(dbInstance)
            SQLQuery = "exec msdb.dbo.sp_start_job N'" & ssisPackageName.Trim & "'"

            DBCommandWrapper = DB.GetSqlStringCommand(SQLQuery)
            DBCommandWrapper.CommandTimeout = timeOut

            Return DB.ExecuteNonQuery(DBCommandWrapper)

        Catch ex As Exception

            Throw

        End Try

    End Function

    Public Shared Function GenerateMaxID(ByVal dbInstance As String, ByVal Reference As String) As Decimal

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try

            DB = CMSDALCommon.CreateDatabase(dbInstance)
            SQLCall = "ImgWorkFlow.dbo.RETRIEVE_NEXT_MAXID"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@Reference", DbType.String, Reference)
            DB.AddOutParameter(DBCommandWrapper, "@MAXID", DbType.Decimal, 18)

            DBCommandWrapper.CommandTimeout = 180

            DB.ExecuteNonQuery(DBCommandWrapper)

            Return CDec(DB.GetParameterValue(DBCommandWrapper, "@MAXID"))

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Function

    Public Shared Function RetrieveEDIByFileName(ByVal dbInstance As String, ByVal fileName As String, ByVal tableNames() As String, ByVal ds As DataSet, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try

            DB = CMSDALCommon.CreateDatabase(dbInstance)
            SQLCall = CMSDALCommon.GetDatabaseName(dbInstance) & "." & "RETRIEVE_EDI_CLAIM_BY_FILENAME"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FILENAME", DbType.String, fileName)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If

                For X As Integer = 0 To tableNames.Length
                    ds.Tables(X).TableName = tableNames(X)
                Next

            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, tableNames)
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, tableNames, transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        Finally
            If transaction Is Nothing AndAlso DBCommandWrapper.Connection IsNot Nothing Then
                DBCommandWrapper.Connection.Close()
                DBCommandWrapper.Connection.Dispose()
            End If

        End Try
    End Function

    Public Shared Function RetrieveEDIByMaximID(ByVal dbInstance As String, ByVal maximID As String, ByVal tableNames() As String, ByVal ds As DataSet, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try

            DB = CMSDALCommon.CreateDatabase(dbInstance)
            SQLCall = CMSDALCommon.GetDatabaseName(dbInstance) & "." & "RETRIEVE_EDI_CLAIM_BY_MAXIMID"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@MaximID", DbType.String, maximID)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If

                For X As Integer = 0 To tableNames.Length
                    ds.Tables(X).TableName = tableNames(X)
                Next

            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, tableNames)
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, tableNames, transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        Finally
            If transaction Is Nothing AndAlso DBCommandWrapper.Connection IsNot Nothing Then
                DBCommandWrapper.Connection.Close()
                DBCommandWrapper.Connection.Dispose()
            End If
        End Try
    End Function

    Public Shared Function RetrieveModifiedHRAAction(ByVal dbInstance As String, ByVal ds As DataSet, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try

            DB = CMSDALCommon.CreateDatabase(dbInstance)
            SQLCall = CMSDALCommon.GetDatabaseName(dbInstance) & "." & "RETRIEVE_MODIFIED_HRA_ACTION"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DBCommandWrapper.CommandTimeout = 180

            If ds Is Nothing Then
                If transaction Is Nothing Then
                    ds = DB.ExecuteDataSet(DBCommandWrapper)
                Else
                    ds = DB.ExecuteDataSet(DBCommandWrapper, transaction)
                End If

                ds.Tables(0).TableName = "HRA_ACTION"

            Else
                If transaction Is Nothing Then
                    DB.LoadDataSet(DBCommandWrapper, ds, "HRA_ACTION")
                Else
                    DB.LoadDataSet(DBCommandWrapper, ds, "HRA_ACTION", transaction)
                End If
            End If

            Return ds

        Catch ex As Exception
            Throw
        Finally
            If transaction Is Nothing AndAlso DBCommandWrapper.Connection IsNot Nothing Then
                DBCommandWrapper.Connection.Close()
                DBCommandWrapper.Connection.Dispose()
            End If
        End Try
    End Function

    Public Shared Function RetrieveRoleSettings(ByVal dbInstance As String, ByVal settingName As String) As Object

        Dim DB As Database
        Dim DS As DataSet
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase(dbInstance)
            SQLCall = If(CMSDALCommon.GetDatabaseName(dbInstance).Length = 0, "medical.dbo", CMSDALCommon.GetDatabaseName(dbInstance)) & "." & "RETRIEVE_ROLE_SETTINGS_BY_NAME"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@SETTING_NAME", DbType.String, settingName)

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0).Rows(0)("SETTING_VALUE")
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function RetrieveSSNByDocumentID(ByVal dbInstance As String, ByVal docID As Integer) As Integer

        Return RetrieveSSNByDocumentID(dbInstance, CType(docID, Decimal))

    End Function
    Public Shared Function RetrieveSSNByDocumentID(ByVal dbInstance As String, ByVal docID As Decimal) As Integer

        Dim DS As DataSet

        Try

            DS = RetrieveSSNByDocumentID(dbInstance, docID, Nothing)

            If DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return CInt(DS.Tables(0)(0)("SSN"))
            End If

            Return 0

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Shared Function RetrieveSSNByDocumentID(ByVal dbInstance As String, ByVal docid As Decimal, ByVal ds As DataSet, Optional ByVal transaction As DbTransaction = Nothing) As DataSet

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try

            DB = CMSDALCommon.CreateDatabase(dbInstance)
            SQLCall = CMSDALCommon.GetDatabaseName(dbInstance) & "." & "RETRIEVE_UFCWDocs_SSN_BY_DOCID"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@DOCID", DbType.Decimal, docid)

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

            Return ds

        Catch ex As Exception
            Throw
        Finally
            If transaction Is Nothing AndAlso DBCommandWrapper.Connection IsNot Nothing Then
                DBCommandWrapper.Connection.Close()
                DBCommandWrapper.Connection.Dispose()
            End If
        End Try
    End Function
    Public Shared Function UpdateEDI_837IMAGEINDEXByFilename(ByVal dbInstance As String, ByVal fileName As String, ByVal transaction As DbTransaction) As String

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try

            DB = CMSDALCommon.CreateDatabase(dbInstance)
            SQLCall = CMSDALCommon.GetDatabaseName(dbInstance) & "." & "UPDATE_EDI_837IMAGEINDEX_BY_FILENAME"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@FileName", DbType.String, fileName)
            DB.AddOutParameter(DBCommandWrapper, "@MAXID", DbType.String, 26)

            DBCommandWrapper.CommandTimeout = 180

            DB.ExecuteNonQuery(DBCommandWrapper, transaction)

            Return DB.GetParameterValue(DBCommandWrapper, "@MAXID").ToString

        Catch ex As Exception
            Throw
        Finally
            If transaction Is Nothing AndAlso DBCommandWrapper.Connection IsNot Nothing Then
                DBCommandWrapper.Connection.Close()
                DBCommandWrapper.Connection.Dispose()
            End If
        End Try
    End Function
    Private Shared Function ResolveinputParms(inputParms() As String, p1 As Integer) As Object

        Select Case inputParms(p1).ToUpper
            Case "CURRENT"
                Return UFCWGeneral.NowDate.Date
            Case "NULL"
                Return Nothing
            Case Else
                Return inputParms(p1)
        End Select

    End Function

    Public Shared Function RetrieveImagingDocTypesByUserID(ByVal dbInstance As String, ByVal userID As String) As DataTable

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim DS As DataSet
        Dim SQLCall As String = "ImgWorkFlow.dbo.RETRIEVE_WFL_IMAGINGDOCTYPES_BY_USERID"
        Try

            DB = CMSDALCommon.CreateDatabase(dbInstance)
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@USERID", DbType.String, userID)

            DBCommandWrapper.CommandTimeout = 120

            DS = DB.ExecuteDataSet(DBCommandWrapper)

            If DS.Tables.Count > 0 Then
                Return DS.Tables(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

End Class
