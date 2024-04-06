Option Infer On
Option Strict On
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.IO
Imports System.Xml.Serialization
Imports System.Data.Common
Imports System.Security.Principal

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.LETTERSControl
''' Class	 : UFCW.CMS.LETTERSControlDAL
'''
''' -----------------------------------------------------------------------------
''' <summary>
'''
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[MAS]	4/17/2007	Created
''' </history>
''' -----------------------------------------------------------------------------
Friend Class LettersDAL
    Public Const _SQLdatabaseName As String = "dbo"
    Public Shared _DomainUser As String = SystemInformation.UserName

    Private Shared _WindowsUserID As WindowsIdentity = WindowsIdentity.GetCurrent()
    Private Shared _WindowsPrincipalForID As WindowsPrincipal = New WindowsPrincipal(_WindowsUserID)

    Private Shared _LettersDS As DataSet
    Private Shared _LineOfBusiness As String
    Private Shared _LettersSyncLock As New Object

    Private Sub New()

    End Sub
    Public Shared Function GetBookMarks(ByVal letterTemplateID As Integer, Optional ByVal ds As DataSet = Nothing) As DataSet

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBMD.RETRIEVE_BOOKMARKS_BY_LETTERID"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@LETTERTEMPLATEID", DbType.Int32, letterTemplateID)

            If ds Is Nothing Then
                ds = DB.ExecuteDataSet(DBCommandWrapper)
                ds.Tables(0).TableName = "LETTER_BOOKMARKS"
            Else
                DB.LoadDataSet(DBCommandWrapper, ds, "LETTER_BOOKMARKS")
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GetLettersHistoryByCLAIMID(ByVal claimID As Object, Optional ByVal ds As DataSet = Nothing) As DataSet

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBMD.RETRIEVE_LETTERS_HISTORY_BY_CLAIMID"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIMID", DbType.Int32, claimID)

            If ds Is Nothing Then
                ds = DB.ExecuteDataSet(DBCommandWrapper)
                ds.Tables(0).TableName = "LETTERS"
            Else
                DB.LoadDataSet(DBCommandWrapper, ds, "LETTERS")
            End If

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GetLettersHistoryByFamilyIDRelationID(ByVal schema As String, ByVal familyID As Integer, ByVal relationID As Integer?, Optional ByVal DS As DataSet = Nothing) As DataSet

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBIS.RETRIEVE_LETTERS_HISTORY_BY_FAMILYID_RELATIONID"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@SCHEMA", DbType.String, schema.ToUpper)
            DB.AddInParameter(DBCommandWrapper, "@FAMILYID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATIONID", DbType.Int16, relationID)

            If DS Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommandWrapper)
                DS.Tables(0).TableName = "LETTERS"
            Else
                DB.LoadDataSet(DBCommandWrapper, DS, "LETTERS")
            End If

            Return DS

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function RetrieveLetters(ByVal lineOfBusiness As String) As DataTable

        Dim DT As DataTable

        Try

            If _LettersDS Is Nothing OrElse _LineOfBusiness <> lineOfBusiness Then
                If _LettersDS IsNot Nothing AndAlso _LettersDS.Tables.Contains("LETTER_MASTER") Then
                    _LettersDS.Tables.Remove("LETTER_MASTER")
                End If
                _LineOfBusiness = lineOfBusiness
                _LettersDS = LoadLetters(lineOfBusiness, _LettersDS)
            End If

            If _LettersDS.Tables.Count > 0 Then
                _LettersDS.Tables(0).TableName = "LETTER_MASTER"

                Dim QueryLETTER_MASTER =
                    From LETTER_MASTER In _LettersDS.Tables("LETTER_MASTER").AsEnumerable()
                    Where LETTER_MASTER.Field(Of String)("DOC_CLASS") = lineOfBusiness
                    Select LETTER_MASTER

                If QueryLETTER_MASTER.Count > 0 Then
                    DT = QueryLETTER_MASTER.CopyToDataTable
                    DT.TableName = "LETTER_MASTER"
                    If lineOfBusiness <> "CLAIMS" Then
                        DT.Columns.Remove("ENABLE_PEND")
                        DT.Columns.Remove("ENABLE_HIDE")
                        DT.Columns.Remove("PEND_DURATION_IN_DAYS")
                    End If
                End If

            End If

            Return DT

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Function
    Private Shared Function LoadLetters(ByVal lineOfBusiness As String, Optional ByVal ds As DataSet = Nothing) As DataSet

        SyncLock _LettersSyncLock

            Dim XMLFilename As String
            Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
            Dim FStream As FileStream
            Dim DB As Database
            Dim DBCommandWrapper As DbCommand
            Dim SQLCall As String
            Dim XMLDS As DataSet
            Dim XMLSerial As XmlSerializer

            Try

                Select Case lineOfBusiness
                    Case "MEDICAL", "CLAIMS"
                        XMLFilename = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_LETTERS" & ".xml"
                        SQLCall = "FDBMD.RETRIEVE_LETTERS"
                        XMLDS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.LETTER_MASTER", "LASTUPDT", "FDBMD.RETRIEVE_LETTERS")
                    Case Else
                        XMLFilename = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_LETTERS_WITH_DOCCLASS" & ".xml"
                        SQLCall = "FDBMD.RETRIEVE_LETTERS_WITH_DOCCLASS"
                        XMLDS = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "HOURS.UFCWLETDESC", "LASTUPDT", "FDBMD.RETRIEVE_LETTERS_WITH_DOCCLASS")
                End Select

                If XMLDS.Tables.Count = 0 Then
                    DB = CMSDALCommon.CreateDatabase()
                    DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                    If XMLDS Is Nothing Then
                        XMLDS = DB.ExecuteDataSet(DBCommandWrapper)
                    Else
                        DB.LoadDataSet(DBCommandWrapper, XMLDS, "LETTER_MASTER")
                    End If

                    _LettersDS = XMLDS

                    FStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)

                    XMLSerial = New XmlSerializer(XMLDS.GetType())
                    XMLSerial.Serialize(FStream, XMLDS)

                    File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                Else
                    _LettersDS = XMLDS
                End If

                If ds IsNot Nothing Then 'if typed dataset was submitted return data in typed format.
                    ds.Merge(_LettersDS)
                    Return ds
                Else
                    Return _LettersDS
                End If


            Catch ex As Exception
                Throw

            Finally

                If FStream IsNot Nothing Then
                    FStream.Close()
                End If

                FStream = Nothing

            End Try
        End SyncLock
    End Function
    Public Shared Function GetLetters(ByVal docClass As String, Optional ByVal ds As DataSet = Nothing) As DataSet

        Dim XMLFilename As String = System.Windows.Forms.Application.StartupPath & "\" & CMSDALCommon.GetDatabaseName() & "." & "FDBMD.RETRIEVE_LETTERS" & ".xml"
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim UniqueThreadIdentifier As String = UFCWGeneral.GetUniqueKey()
        Dim SQLCall As String
        Dim objLock As Object = New Object
        Dim LetterDV As DataView
        Dim LetterDS As New DataSet

        LetterDS = ds

        Try
            SyncLock (objLock)
                If _LettersDS Is Nothing Then
                    ds = CMSXMLHandler.ToAndFromDataset(UniqueThreadIdentifier, "FDBMD.LETTER_MASTER", "LASTUPDT", "FDBMD.RETRIEVE_LETTERS")
                    If ds.Tables.Count = 0 Then
                        SQLCall = "FDBMD.RETRIEVE_LETTERS"
                        DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

                        If ds Is Nothing Then
                            ds = DB.ExecuteDataSet(DBCommandWrapper)
                        Else
                            DB.LoadDataSet(DBCommandWrapper, ds, "LETTER_MASTER")
                        End If

                        _LettersDS = ds

                        Dim FStream As FileStream = New FileStream(XMLFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)
                        Dim xmlSerial As New XmlSerializer(ds.GetType())
                        xmlSerial.Serialize(FStream, ds)
                        FStream.Close()
                        File.SetAttributes(XMLFilename, FileAttributes.ReadOnly)
                    Else
                        _LettersDS = ds
                    End If
                Else
                    ds = _LettersDS
                End If
            End SyncLock

            LetterDV = New DataView(_LettersDS.Tables("LETTER_MASTER"), "DOC_CLASS='" & docClass & "'", "", DataViewRowState.CurrentRows)

            If LetterDS.Tables(0).Rows.Count > 0 Then LetterDS.Tables(0).Rows.Clear()
            For i As Integer = 0 To LetterDV.Count - 1
                LetterDS.Tables(0).ImportRow(LetterDV(i).Row)
            Next
            ds = LetterDS

            Return ds

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function CreateDataSetFromXML(ByVal xmlFile As String) As DataSet

        'load xml into a dataset to use here
        Dim DS As New DataSet
        Dim FS As FileStream

        'open the xml file so we can use it to fill the dataset
        Try
            FS = New FileStream(xmlFile, FileMode.Open, FileAccess.Read)
        Catch ex As Exception
            Throw
        End Try

        'fill the dataset
        Try
            DS.ReadXml(FS)

            Return DS

        Catch ex As Exception
            Throw
        Finally
            FS.Close()
        End Try

    End Function
    Public Shared Function CreateClaimMaster(ByRef DocumentID As Long, ByRef FAMILY_ID As Integer, ByVal RELATION_ID As Short, ByVal PART_SSN As Integer, ByVal PAT_SSN As Integer,
                                     ByVal PRIORITY As Object, ByVal SECURITY_SW As Object, ByVal ATTACH_SW As Object, ByVal DUPLICATE_SW As Object,
                                     ByVal ARCHIVE_SW As Object, ByVal STATUS As Object, ByVal STATUS_DATE As Object, ByVal DOC_CLASS As Object, ByVal DOC_TYPE As Object,
                                     ByVal MAXID As Object, ByVal BATCH As Object, ByVal REC_DATE As Object, ByVal OPEN_DATE As Object,
                                     ByVal DATE_OF_SERVICE As Object, ByVal PAGE_COUNT As Object, ByVal PAT_FNAME As Object, ByVal PAT_INT As Object, ByVal PAT_LNAME As Object,
                                     ByVal PART_FNAME As Object, ByVal PART_INT As Object, ByVal PART_LNAME As Object, ByVal PROV_TIN As Object, ByVal PROV_ID As Object,
                                     Optional ByVal Transaction As DbTransaction = Nothing) As Int32

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try

            SQLCall = "FDBMD.CREATE_CLAIM_MASTER"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, FAMILY_ID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, RELATION_ID)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, PART_SSN)
            DB.AddInParameter(DBCommandWrapper, "@PAT_SSN", DbType.Int32, PAT_SSN)
            DB.AddInParameter(DBCommandWrapper, "@PRIORITY", DbType.Int32, PRIORITY)
            DB.AddInParameter(DBCommandWrapper, "@SECURITY_SW", DbType.Decimal, SECURITY_SW)
            DB.AddInParameter(DBCommandWrapper, "@ATTACH_SW", DbType.Decimal, ATTACH_SW)
            DB.AddInParameter(DBCommandWrapper, "@DUPLICATE_SW", DbType.Decimal, DUPLICATE_SW)
            DB.AddInParameter(DBCommandWrapper, "@BUSY_SW", DbType.Decimal, 0D)
            DB.AddInParameter(DBCommandWrapper, "@ARCHIVE_SW", DbType.Decimal, ARCHIVE_SW)
            DB.AddInParameter(DBCommandWrapper, "@STATUS", DbType.String, STATUS)
            DB.AddInParameter(DBCommandWrapper, "@STATUS_DATE", DbType.DateTime, STATUS_DATE)
            DB.AddInParameter(DBCommandWrapper, "@DOC_CLASS", DbType.String, DOC_CLASS)
            DB.AddInParameter(DBCommandWrapper, "@DOC_TYPE", DbType.String, DOC_TYPE)
            DB.AddInParameter(DBCommandWrapper, "@DOCID", DbType.Decimal, DocumentID)
            DB.AddInParameter(DBCommandWrapper, "@MAXID", DbType.String, MAXID)
            DB.AddInParameter(DBCommandWrapper, "@BATCH", DbType.String, BATCH)
            DB.AddInParameter(DBCommandWrapper, "@REC_DATE", DbType.Date, REC_DATE)
            DB.AddInParameter(DBCommandWrapper, "@OPEN_DATE", DbType.DateTime, OPEN_DATE)
            DB.AddInParameter(DBCommandWrapper, "@DATE_OF_SERVICE", DbType.Date, DATE_OF_SERVICE)
            DB.AddInParameter(DBCommandWrapper, "@PAGE_COUNT", DbType.Int32, PAGE_COUNT)
            DB.AddInParameter(DBCommandWrapper, "@PAT_FNAME", DbType.String, PAT_FNAME)
            DB.AddInParameter(DBCommandWrapper, "@PAT_INT", DbType.String, PAT_INT)
            DB.AddInParameter(DBCommandWrapper, "@PAT_LNAME", DbType.String, PAT_LNAME)
            DB.AddInParameter(DBCommandWrapper, "@PART_FNAME", DbType.String, PART_FNAME)
            DB.AddInParameter(DBCommandWrapper, "@PART_INT", DbType.String, PART_INT)
            DB.AddInParameter(DBCommandWrapper, "@PART_LNAME", DbType.String, PART_LNAME)
            DB.AddInParameter(DBCommandWrapper, "@PROV_TIN", DbType.Int32, PROV_TIN)
            DB.AddInParameter(DBCommandWrapper, "@PROV_ID", DbType.Int32, PROV_ID)
            DB.AddInParameter(DBCommandWrapper, "@REFRENCE_CLAIM", DbType.Int32, DBNull.Value)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser.ToUpper)

            DB.AddOutParameter(DBCommandWrapper, "@CLAIMID", DbType.Int32, 10)

            If Transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, Transaction)
            End If

            Return CInt(DB.GetParameterValue(DBCommandWrapper, "@CLAIMID"))

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Sub CreateClaimHistory(claimID As Integer?, docID As Long, transactionType As String, familyID As Integer,
                                         relationID As Short, partSSN As Integer, patSSN As Integer,
                                         docType As String, summary As String, detail As String,
                                         Optional ByRef transaction As DbTransaction = Nothing)

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try

            SQLCall = "FDBMD.CREATE_DOC_HISTORY"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@CLAIM_ID", DbType.Int32, If(claimID Is Nothing, 0, claimID))
            DB.AddInParameter(DBCommandWrapper, "@DOCID", DbType.Int32, docID)
            DB.AddInParameter(DBCommandWrapper, "@TRANSACTION_TYPE", DbType.String, transactionType)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, partSSN)
            DB.AddInParameter(DBCommandWrapper, "@PAT_SSN", DbType.Int32, patSSN)
            DB.AddInParameter(DBCommandWrapper, "@DOC_CLASS", DbType.String, "CLAIMS")
            DB.AddInParameter(DBCommandWrapper, "@DOC_TYPE", DbType.String, docType)
            DB.AddInParameter(DBCommandWrapper, "@SUMMARY", DbType.String, summary)
            DB.AddInParameter(DBCommandWrapper, "@DETAIL", DbType.String, detail)
            DB.AddInParameter(DBCommandWrapper, "@CREATE_USERID", DbType.String, _DomainUser.ToUpper)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub CreateEligibilityHistory(ByVal docID As Long?, ByVal transactionType As String, familyID As Integer, relationID As Short, partSSN As Integer, patSSN As Integer, ByVal docType As String, ByVal summary As String, ByVal detail As String, Optional ByVal transaction As DbTransaction = Nothing)
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBEL.CREATE_REG_HISTORY"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int16, relationID)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, partSSN)
            DB.AddInParameter(DBCommandWrapper, "@PAT_SSN", DbType.Int32, patSSN)
            DB.AddInParameter(DBCommandWrapper, "@TRANSACTION_TYPE", DbType.String, transactionType)
            DB.AddInParameter(DBCommandWrapper, "@DOC_CLASS", DbType.String, "ELIGIBILITY")
            DB.AddInParameter(DBCommandWrapper, "@DOC_TYPE", DbType.String, docType)
            DB.AddInParameter(DBCommandWrapper, "@DOCID", DbType.Int32, docID)
            DB.AddInParameter(DBCommandWrapper, "@SUMMARY", DbType.String, Replace(CStr(summary), vbCrLf, "\N"))
            DB.AddInParameter(DBCommandWrapper, "@DETAIL", DbType.String, Replace(CStr(detail), vbCrLf, "\N"))
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, _DomainUser)

            If transaction IsNot Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper)
            End If
        Catch ex As Exception
            Dim Rethrow As Boolean = True
            If (Rethrow) Then
                Throw
            End If
        End Try
    End Sub
    Public Shared Function BeginTransaction() As DbTransaction

        Dim DB As Database
        Dim DBConnection As DbConnection
        Dim Transaction As DbTransaction

        Try
            DB = CMSDALCommon.CreateDatabase()
            DBConnection = DB.CreateConnection()

            DBConnection.Open()
            Transaction = DBConnection.BeginTransaction()

            Return Transaction

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Sub CommitTransaction(ByRef transaction As DbTransaction)
        Try
            If Not transaction Is Nothing AndAlso Not transaction.Connection Is Nothing Then
                transaction.Commit()
            End If
        Catch ex As Exception
            Throw
        Finally
            transaction = Nothing
        End Try
    End Sub
    Public Shared Function GetPARTICIPANTInformation(ByVal familyId As Integer, ByVal relationId As Integer, Optional ByVal DS As DataSet = Nothing) As DataSet

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBMD.RETRIEVE_PARTICIPANT_ADDRESS_BY_FAMILY_ID"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyId)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationId)

            If DS Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommandWrapper)
            Else
                DB.LoadDataSet(DBCommandWrapper, DS, "REG_ADDRESS")
            End If

            Return DS

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GetPARTICIPANTAddresses(ByVal familyID As Integer, ByVal relationID As Integer?, Optional ByVal DS As DataSet = Nothing) As DataSet

        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String
        Dim Tablenames() As String = {"PARTICIPANT_ADDRESS", "PATIENT_ADDRESS"}

        Try
            SQLCall = "FDBMD.RETRIEVE_PARTICIPANT_ADDRESSES"
            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, familyID)
            DB.AddInParameter(DBCommandWrapper, "@RELATION_ID", DbType.Int32, relationID)

            If DS Is Nothing Then
                DS = DB.ExecuteDataSet(DBCommandWrapper)
                DS.Tables(0).TableName = "PARTICIPANT_ADDRESS"
                DS.Tables(1).TableName = "PATIENT_ADDRESS"
            Else
                DB.LoadDataSet(DBCommandWrapper, DS, Tablenames)
            End If

            Return DS

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Sub RollbackTransaction(ByRef transaction As DbTransaction)
        Try

            If transaction IsNot Nothing AndAlso transaction.Connection IsNot Nothing AndAlso transaction.Connection.State <> ConnectionState.Closed Then
                transaction.Rollback()
            End If

        Catch ex As Exception
            Throw
        Finally
            If transaction IsNot Nothing Then transaction.Dispose()
            transaction = Nothing
        End Try
    End Sub
    'Public Shared Sub CreateLetterRequest(ByVal letterTemplateID As Integer, ByVal letterTemplateName As String, ByVal claimID As Integer?, ByVal providerID As Integer?,
    '                                      ByVal participantDR As DataRow, ByVal patientDR As DataRow,
    '                                      ByRef letterID As Integer, ByRef maxID As String, lines As String(), Optional ByRef transaction As DbTransaction = Nothing)

    '    Dim DB As Database = CMSDALCommon.CreateDatabase()
    '    Dim DBCommandWrapper As DbCommand
    '    Dim SQLCall As String

    '    Try

    '        SQLCall = "FDBMD.CREATE_LETTERS_MEDICAL"

    '        DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

    '        DB.AddInParameter(DBCommandWrapper, "@LETTERTEMPLATEID", DbType.Int32, letterTemplateID)
    '        DB.AddInParameter(DBCommandWrapper, "@LETTERTEMPLATENAME", DbType.String, letterTemplateName)
    '        DB.AddInParameter(DBCommandWrapper, "@CLAIMID", DbType.Int32, If(claimID Is Nothing, 0, claimID))
    '        DB.AddInParameter(DBCommandWrapper, "@PROVIDERID", DbType.Int32, If(providerID Is Nothing, 0, providerID))
    '        DB.AddInParameter(DBCommandWrapper, "@FAMILYID", DbType.Int32, patientDR("FAMILY_ID"))
    '        DB.AddInParameter(DBCommandWrapper, "@RELATIONID", DbType.Int16, patientDR("RELATION_ID"))
    '        DB.AddInParameter(DBCommandWrapper, "@PATSSN", DbType.Int32, patientDR("SSNO"))
    '        DB.AddInParameter(DBCommandWrapper, "@PARTSSN", DbType.Int32, participantDR("SSNO"))
    '        DB.AddInParameter(DBCommandWrapper, "@PARTLNAME", DbType.String, participantDR("LAST_NAME"))
    '        DB.AddInParameter(DBCommandWrapper, "@PARTFNAME", DbType.String, participantDR("FIRST_NAME"))
    '        DB.AddInParameter(DBCommandWrapper, "@PATLNAME", DbType.String, patientDR("LAST_NAME"))
    '        DB.AddInParameter(DBCommandWrapper, "@PATFNAME", DbType.String, patientDR("FIRST_NAME"))
    '        DB.AddInParameter(DBCommandWrapper, "@PARTADD1", DbType.String, participantDR("ADDRESS1"))
    '        DB.AddInParameter(DBCommandWrapper, "@PARTADD2", DbType.String, participantDR("ADDRESS2"))
    '        DB.AddInParameter(DBCommandWrapper, "@PARTCITY", DbType.String, participantDR("CITY"))
    '        DB.AddInParameter(DBCommandWrapper, "@PARTSTATE", DbType.String, participantDR("STATE"))
    '        DB.AddInParameter(DBCommandWrapper, "@PARTZIP", DbType.String, If(Not IsDBNull(participantDR("ZIP1")) AndAlso participantDR("ZIP1").ToString.Trim.Length > 0 AndAlso CInt(participantDR("ZIP1")) > 0, CInt(participantDR("ZIP1")).ToString("D5") & If(Not IsDBNull(participantDR("ZIP2")) AndAlso participantDR("ZIP2").ToString.Trim.Length > 0 AndAlso CInt(participantDR("ZIP2")) > 0, "-" & CInt(participantDR("ZIP2")).ToString("D4"), ""), ""))
    '        DB.AddInParameter(DBCommandWrapper, "@PARTCOUNTRY", DbType.String, participantDR("COUNTRY"))
    '        DB.AddInParameter(DBCommandWrapper, "@LINE01", DbType.String, If(CType(lines, String())(0) Is Nothing, Nothing, CType(lines, String())(0)))
    '        DB.AddInParameter(DBCommandWrapper, "@LINE02", DbType.String, If(CType(lines, String())(1) Is Nothing, Nothing, CType(lines, String())(1)))
    '        DB.AddInParameter(DBCommandWrapper, "@LINE03", DbType.String, If(CType(lines, String())(2) Is Nothing, Nothing, CType(lines, String())(2)))
    '        DB.AddInParameter(DBCommandWrapper, "@LINE04", DbType.String, If(CType(lines, String())(3) Is Nothing, Nothing, CType(lines, String())(3)))
    '        DB.AddInParameter(DBCommandWrapper, "@LINE05", DbType.String, If(CType(lines, String())(4) Is Nothing, Nothing, CType(lines, String())(4)))
    '        DB.AddInParameter(DBCommandWrapper, "@USERID", DbType.String, _DomainUser.ToUpper)

    '        DB.AddOutParameter(DBCommandWrapper, "@LETTER_ID", DbType.Int32, 4)
    '        DB.AddOutParameter(DBCommandWrapper, "@MAXID", DbType.String, 26)

    '        If transaction Is Nothing Then
    '            DB.ExecuteNonQuery(DBCommandWrapper)
    '        Else
    '            DB.ExecuteNonQuery(DBCommandWrapper, transaction)
    '        End If

    '        letterID = CInt(DB.GetParameterValue(DBCommandWrapper, "@LETTER_ID"))
    '        maxID = CStr(DB.GetParameterValue(DBCommandWrapper, "@MAXID"))

    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub
    Public Shared Sub CreateLetterRequest(ByVal letterTEMPLATEID As Integer, ByVal letterTEMPLATENAME As String, participantDR As DataRow, patientDR As DataRow,
                                                     ByVal lines() As String, ByVal dates As Date?(), ByVal amts2 As Decimal?(), ByVal amts4 As Decimal?(), ByVal nums As Integer?(), ByVal texts() As String,
                                                     ByRef letterID As Integer, ByRef maxID As String, ByVal docClass As String, ByVal docType As String, Optional ByRef transaction As DbTransaction = Nothing)


        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try

            SQLCall = "FDBIS.CREATE_LETTERS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@LETTERTEMPLATEID", DbType.Int32, letterTEMPLATEID)
            DB.AddInParameter(DBCommandWrapper, "@LETTERTEMPLATENAME", DbType.String, letterTEMPLATENAME)
            DB.AddInParameter(DBCommandWrapper, "@CLAIMID", DbType.Int32, If(nums(0) Is Nothing, 0, nums(0)))
            DB.AddInParameter(DBCommandWrapper, "@PROVIDERID", DbType.Int32, If(nums(1) Is Nothing, 0, nums(1)))
            DB.AddInParameter(DBCommandWrapper, "@FAMILYID", DbType.Int32, patientDR("FAMILY_ID"))
            DB.AddInParameter(DBCommandWrapper, "@RELATIONID", DbType.Int16, patientDR("RELATION_ID"))
            DB.AddInParameter(DBCommandWrapper, "@PATSSN", DbType.Int32, patientDR("SSNO"))
            DB.AddInParameter(DBCommandWrapper, "@PATLNAME", DbType.String, patientDR("LAST_NAME"))
            DB.AddInParameter(DBCommandWrapper, "@PATFNAME", DbType.String, patientDR("FIRST_NAME"))
            DB.AddInParameter(DBCommandWrapper, "@PATMI", DbType.String, patientDR("MIDDLE_INITIAL"))
            DB.AddInParameter(DBCommandWrapper, "@PATADD1", DbType.String, patientDR("ADDRESS1"))
            DB.AddInParameter(DBCommandWrapper, "@PATADD2", DbType.String, patientDR("ADDRESS2"))
            DB.AddInParameter(DBCommandWrapper, "@PATCITY", DbType.String, patientDR("CITY"))
            DB.AddInParameter(DBCommandWrapper, "@PATSTATE", DbType.String, patientDR("STATE"))
            DB.AddInParameter(DBCommandWrapper, "@PATZIP", DbType.String, If(Not IsDBNull(patientDR("ZIP1")) AndAlso patientDR("ZIP1").ToString.Trim.Length > 0 AndAlso CInt(patientDR("ZIP1")) > 0, CInt(patientDR("ZIP1")).ToString("D5"), "") & If(Not IsDBNull(patientDR("ZIP2")) AndAlso patientDR("ZIP2").ToString.Trim.Length > 0 AndAlso CInt(patientDR("ZIP2")) > 0, "-" & CInt(patientDR("ZIP2")).ToString("D4"), ""))
            DB.AddInParameter(DBCommandWrapper, "@PATCOUNTRY", DbType.String, If(CBool(patientDR("FOREIGN_SW")), patientDR("COUNTRY"), ""))
            DB.AddInParameter(DBCommandWrapper, "@NUM03", DbType.Int32, participantDR("SSNO"))
            DB.AddInParameter(DBCommandWrapper, "@TEXT11", DbType.String, UFCWGeneral.ToNullStringHandler(texts(0)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT12", DbType.String, UFCWGeneral.ToNullStringHandler(texts(1)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT13", DbType.String, UFCWGeneral.ToNullStringHandler(texts(2)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT14", DbType.String, UFCWGeneral.ToNullStringHandler(texts(3)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT15", DbType.String, UFCWGeneral.ToNullStringHandler(texts(4)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT16", DbType.String, UFCWGeneral.ToNullStringHandler(texts(5)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT17", DbType.String, UFCWGeneral.ToNullStringHandler(texts(6)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT18", DbType.String, UFCWGeneral.ToNullStringHandler(texts(7)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT19", DbType.String, UFCWGeneral.ToNullStringHandler(texts(8)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT20", DbType.String, UFCWGeneral.ToNullStringHandler(texts(9)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT21", DbType.String, UFCWGeneral.ToNullStringHandler(texts(10)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT22", DbType.String, UFCWGeneral.ToNullStringHandler(texts(11)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT23", DbType.String, UFCWGeneral.ToNullStringHandler(texts(12)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT24", DbType.String, UFCWGeneral.ToNullStringHandler(texts(13)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT25", DbType.String, UFCWGeneral.ToNullStringHandler(texts(14)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT26", DbType.String, UFCWGeneral.ToNullStringHandler(texts(15)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT27", DbType.String, UFCWGeneral.ToNullStringHandler(texts(16)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT28", DbType.String, UFCWGeneral.ToNullStringHandler(texts(17)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT29", DbType.String, UFCWGeneral.ToNullStringHandler(texts(18)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT30", DbType.String, UFCWGeneral.ToNullStringHandler(texts(19)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT31", DbType.String, UFCWGeneral.ToNullStringHandler(texts(20)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT32", DbType.String, UFCWGeneral.ToNullStringHandler(texts(21)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT33", DbType.String, UFCWGeneral.ToNullStringHandler(texts(22)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT34", DbType.String, UFCWGeneral.ToNullStringHandler(texts(23)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT35", DbType.String, UFCWGeneral.ToNullStringHandler(texts(24)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT36", DbType.String, UFCWGeneral.ToNullStringHandler(texts(25)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT37", DbType.String, UFCWGeneral.ToNullStringHandler(texts(26)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT38", DbType.String, UFCWGeneral.ToNullStringHandler(texts(27)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT39", DbType.String, UFCWGeneral.ToNullStringHandler(texts(28)))
            DB.AddInParameter(DBCommandWrapper, "@TEXT40", DbType.String, UFCWGeneral.ToNullStringHandler(texts(29)))
            DB.AddInParameter(DBCommandWrapper, "@LINE01", DbType.String, UFCWGeneral.IsNullStringHandler(lines(0)))
            DB.AddInParameter(DBCommandWrapper, "@LINE02", DbType.String, UFCWGeneral.IsNullStringHandler(lines(1)))
            DB.AddInParameter(DBCommandWrapper, "@LINE03", DbType.String, UFCWGeneral.IsNullStringHandler(lines(2)))
            DB.AddInParameter(DBCommandWrapper, "@LINE04", DbType.String, UFCWGeneral.IsNullStringHandler(lines(3)))
            DB.AddInParameter(DBCommandWrapper, "@LINE05", DbType.String, UFCWGeneral.IsNullStringHandler(lines(4)))
            DB.AddInParameter(DBCommandWrapper, "@LINE06", DbType.String, UFCWGeneral.IsNullStringHandler(lines(5)))
            DB.AddInParameter(DBCommandWrapper, "@LINE07", DbType.String, UFCWGeneral.IsNullStringHandler(lines(6)))
            DB.AddInParameter(DBCommandWrapper, "@LINE08", DbType.String, UFCWGeneral.IsNullStringHandler(lines(7)))
            DB.AddInParameter(DBCommandWrapper, "@LINE09", DbType.String, UFCWGeneral.IsNullStringHandler(lines(8)))
            DB.AddInParameter(DBCommandWrapper, "@LINE10", DbType.String, UFCWGeneral.IsNullStringHandler(lines(9)))
            DB.AddInParameter(DBCommandWrapper, "@DATE01", DbType.Date, dates(0))
            DB.AddInParameter(DBCommandWrapper, "@DATE02", DbType.Date, dates(1))
            DB.AddInParameter(DBCommandWrapper, "@DATE03", DbType.Date, dates(2))
            DB.AddInParameter(DBCommandWrapper, "@DATE04", DbType.Date, dates(3))
            DB.AddInParameter(DBCommandWrapper, "@DATE05", DbType.Date, dates(4))
            DB.AddInParameter(DBCommandWrapper, "@DATE06", DbType.Date, dates(5))
            DB.AddInParameter(DBCommandWrapper, "@DATE07", DbType.Date, dates(6))
            DB.AddInParameter(DBCommandWrapper, "@DATE08", DbType.Date, dates(7))
            DB.AddInParameter(DBCommandWrapper, "@AMT201", DbType.Decimal, amts2(0))
            DB.AddInParameter(DBCommandWrapper, "@AMT202", DbType.Decimal, amts2(1))
            DB.AddInParameter(DBCommandWrapper, "@AMT203", DbType.Decimal, amts2(2))
            DB.AddInParameter(DBCommandWrapper, "@AMT204", DbType.Decimal, amts2(3))
            DB.AddInParameter(DBCommandWrapper, "@AMT205", DbType.Decimal, amts2(4))
            DB.AddInParameter(DBCommandWrapper, "@AMT206", DbType.Decimal, amts2(5))
            DB.AddInParameter(DBCommandWrapper, "@AMT207", DbType.Decimal, amts2(6))
            DB.AddInParameter(DBCommandWrapper, "@AMT208", DbType.Decimal, amts2(7))
            DB.AddInParameter(DBCommandWrapper, "@AMT209", DbType.Decimal, amts2(8))
            DB.AddInParameter(DBCommandWrapper, "@AMT210", DbType.Decimal, amts2(9))
            DB.AddInParameter(DBCommandWrapper, "@AMT401", DbType.Decimal, amts4(0))
            DB.AddInParameter(DBCommandWrapper, "@AMT402", DbType.Decimal, amts4(1))
            DB.AddInParameter(DBCommandWrapper, "@AMT403", DbType.Decimal, amts4(2))
            DB.AddInParameter(DBCommandWrapper, "@AMT404", DbType.Decimal, amts4(3))
            DB.AddInParameter(DBCommandWrapper, "@AMT405", DbType.Decimal, amts4(4))
            DB.AddInParameter(DBCommandWrapper, "@NUM04", DbType.Int32, nums(3))
            DB.AddInParameter(DBCommandWrapper, "@NUM05", DbType.Int32, nums(4))
            DB.AddInParameter(DBCommandWrapper, "@USERID", DbType.String, _DomainUser.ToUpper)
            DB.AddInParameter(DBCommandWrapper, "@DOC_CLASS", DbType.String, docClass)
            DB.AddInParameter(DBCommandWrapper, "@DOC_TYPE", DbType.String, docType)

            DB.AddOutParameter(DBCommandWrapper, "@LETTER_ID", DbType.Int32, 4)
            DB.AddOutParameter(DBCommandWrapper, "@MAXID", DbType.String, 26)

            If transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, transaction)
            End If

            letterID = CInt(DB.GetParameterValue(DBCommandWrapper, "@LETTER_ID"))
            maxID = CStr(DB.GetParameterValue(DBCommandWrapper, "@MAXID"))

        Catch ex As Exception
            Throw
        End Try
    End Sub
    'Public Shared Sub UpdateLettersMedical(ByVal LetterID As Integer, ByVal Status As String, ByVal UserID As String, Optional ByVal Transaction As DbTransaction = Nothing)
    '    Dim DB As Database = CMSDALCommon.CreateDatabase()
    '    Dim DBCommandWrapper As DbCommand
    '    Dim SQLCall As String

    '    Try
    '        SQLCall = "FDBMD.UPDATE_LETTERS_MEDICAL"

    '        DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
    '        DB.AddInParameter(DBCommandWrapper, "@LETTER_ID", DbType.Int32, LetterID)
    '        DB.AddInParameter(DBCommandWrapper, "@MAIL_STATUS", DbType.String, Status)
    '        DB.AddInParameter(DBCommandWrapper, "@USERID", DbType.String, UserID)

    '        If Transaction Is Nothing Then
    '            DB.ExecuteNonQuery(DBCommandWrapper)
    '        Else
    '            DB.ExecuteNonQuery(DBCommandWrapper, Transaction)
    '        End If
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub
    Public Shared Sub UpdateLetters(ByVal LetterID As Integer, ByVal Status As String, ByVal UserID As String, ByVal lastUpdt As Date, ByVal docClass As String, Optional ByVal Transaction As DbTransaction = Nothing)
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBIS.UPDATE_LETTERS"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)
            DB.AddInParameter(DBCommandWrapper, "@IDENT", DbType.Int32, LetterID)
            DB.AddInParameter(DBCommandWrapper, "@LASTUPDT", DbType.DateTime, lastUpdt)
            DB.AddInParameter(DBCommandWrapper, "@MAILSTATUS", DbType.String, Status)
            DB.AddInParameter(DBCommandWrapper, "@USERID", DbType.String, UserID)
            DB.AddInParameter(DBCommandWrapper, "@DOC_CLASS", DbType.String, docClass)

            If Transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, Transaction)
            End If

        Catch ex As DDTek.DB2.DB2Exception When ex.SQLState = "20028"

            Throw

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Sub UpdateClaimMasterForLetterControl(ByVal DocID As Decimal, ByVal PartSSN As Integer?, ByVal PatSSN As Integer?, ByVal FamilyID As Object, ByVal ReferenceClaim As Integer?,
                                                        ByVal UserRights As String, Optional ByVal Transaction As DbTransaction = Nothing)
        Dim DB As Database = CMSDALCommon.CreateDatabase()
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            SQLCall = "FDBMD.UPDATE_CLAIM_MASTER_FOR_LETTERHISTORY"

            DBCommandWrapper = DB.GetStoredProcCommand(SQLCall)

            DB.AddInParameter(DBCommandWrapper, "@DOCID", DbType.Decimal, DocID)
            DB.AddInParameter(DBCommandWrapper, "@PART_SSN", DbType.Int32, PartSSN)
            DB.AddInParameter(DBCommandWrapper, "@PAT_SSN", DbType.Int32, PatSSN)
            DB.AddInParameter(DBCommandWrapper, "@FAMILY_ID", DbType.Int32, FamilyID)
            DB.AddInParameter(DBCommandWrapper, "@REFRENCE_CLAIM", DbType.Int32, ReferenceClaim)
            DB.AddInParameter(DBCommandWrapper, "@USER_RIGHTS", DbType.String, UserRights)

            If Transaction Is Nothing Then
                DB.ExecuteNonQuery(DBCommandWrapper)
            Else
                DB.ExecuteNonQuery(DBCommandWrapper, Transaction)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Shared Function CMSCanRunReports() As Boolean

#If DEBUG And CMSCanRunReports Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And CMSCanRunReports = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanRunReports")
#End If
        'Return True

    End Function
    Public Shared Function REGMSupervisorAccess() As Boolean

#If DEBUG And REGMLifeEventDeleteAccess = True Then '' This is supervisor level security in REGM
        Return True
#ElseIf DEBUG And REGMLifeEventDeleteAccess = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\REGMLifeEventDeleteAccess")
#End If
        'Return True
    End Function
    Public Shared Function CMSCanOverrideAccumulatorsAccess() As Boolean

#If DEBUG And CMSCanOverrideAccumulators = True Then
        Return True
#ElseIf DEBUG And CMSCanOverrideAccumulators = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanOverrideAccumulators")
#End If

    End Function
End Class