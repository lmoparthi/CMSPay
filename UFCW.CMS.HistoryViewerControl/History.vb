Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data.Common
Imports System.Data
Imports System.Configuration

Public Class CMSHistoryViewerForm
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
    Private _DefaultProviderName As String = CType(ConfigurationManager.GetSection("dataConfiguration"), Microsoft.Practices.EnterpriseLibrary.Data.Configuration.DatabaseSettings).DefaultDatabase

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents SqlSelectCommand1 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlInsertCommand1 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlUpdateCommand1 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlDeleteCommand1 As System.Data.SqlClient.SqlCommand
    Friend WithEvents HistAdapter As System.Data.SqlClient.SqlDataAdapter
    Friend WithEvents HistDataset As HistDataset
    Friend WithEvents SqlSelectCommand2 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlInsertCommand2 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlUpdateCommand2 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlDeleteCommand2 As System.Data.SqlClient.SqlCommand
    Friend WithEvents DocsAdapter As System.Data.SqlClient.SqlDataAdapter
    Friend WithEvents DocsDataset As DocsDataset
    Friend WithEvents RefreshGrid As System.Windows.Forms.Button
    Friend WithEvents CloseForm As System.Windows.Forms.Button
    Friend WithEvents EntireHist As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ScanDate As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ReceiveDate As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Status As System.Windows.Forms.TextBox
    Friend WithEvents HistoryDataGrid As DataGridCustom
    Public WithEvents holder As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents IndexDate As System.Windows.Forms.TextBox
    Friend WithEvents Annotate As System.Windows.Forms.Button
    Friend WithEvents TTip As System.Windows.Forms.ToolTip
    Friend WithEvents HistoryImageList As System.Windows.Forms.ImageList
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CMSHistoryViewerForm))
        Me.SqlSelectCommand1 = New System.Data.SqlClient.SqlCommand
        Me.SqlInsertCommand1 = New System.Data.SqlClient.SqlCommand
        Me.SqlUpdateCommand1 = New System.Data.SqlClient.SqlCommand
        Me.SqlDeleteCommand1 = New System.Data.SqlClient.SqlCommand
        Me.HistAdapter = New System.Data.SqlClient.SqlDataAdapter
        Me.HistDataset = New HistDataset
        Me.SqlSelectCommand2 = New System.Data.SqlClient.SqlCommand
        Me.SqlInsertCommand2 = New System.Data.SqlClient.SqlCommand
        Me.SqlUpdateCommand2 = New System.Data.SqlClient.SqlCommand
        Me.SqlDeleteCommand2 = New System.Data.SqlClient.SqlCommand
        Me.DocsAdapter = New System.Data.SqlClient.SqlDataAdapter
        Me.DocsDataset = New DocsDataset
        Me.holder = New System.Windows.Forms.Panel
        Me.Label4 = New System.Windows.Forms.Label
        Me.IndexDate = New System.Windows.Forms.TextBox
        Me.Annotate = New System.Windows.Forms.Button
        Me.RefreshGrid = New System.Windows.Forms.Button
        Me.CloseForm = New System.Windows.Forms.Button
        Me.EntireHist = New System.Windows.Forms.CheckBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.ScanDate = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.ReceiveDate = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Status = New System.Windows.Forms.TextBox
        Me.HistoryDataGrid = New DataGridCustom
        Me.TTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.HistoryImageList = New System.Windows.Forms.ImageList(Me.components)
        CType(Me.HistDataset, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DocsDataset, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.holder.SuspendLayout()
        CType(Me.HistoryDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SqlSelectCommand1
        '
        Me.SqlSelectCommand1.CommandText = "SELECT SQLTranID, TranID, ProcessDateTime, TransactionType, DocumentID, SSN, SSNA" &
            "lias, HistoryText, Detail, Code, DocClass, DocType, userid, lastupdt, CompleteDa" &
            "te FROM History"
        '
        'SqlInsertCommand1
        '
        Me.SqlInsertCommand1.CommandText = resources.GetString("SqlInsertCommand1.CommandText")
        Me.SqlInsertCommand1.Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@TranID", System.Data.SqlDbType.[Decimal], 9, System.Data.ParameterDirection.Input, False, CType(18, Byte), CType(0, Byte), "TranID", System.Data.DataRowVersion.Current, Nothing), New System.Data.SqlClient.SqlParameter("@ProcessDateTime", System.Data.SqlDbType.DateTime, 4, "ProcessDateTime"), New System.Data.SqlClient.SqlParameter("@TransactionType", System.Data.SqlDbType.NVarChar, 16, "TransactionType"), New System.Data.SqlClient.SqlParameter("@DocumentID", System.Data.SqlDbType.NVarChar, 16, "DocumentID"), New System.Data.SqlClient.SqlParameter("@SSN", System.Data.SqlDbType.NVarChar, 11, "SSN"), New System.Data.SqlClient.SqlParameter("@SSNAlias", System.Data.SqlDbType.NVarChar, 11, "SSNAlias"), New System.Data.SqlClient.SqlParameter("@HistoryText", System.Data.SqlDbType.NVarChar, 255, "HistoryText"), New System.Data.SqlClient.SqlParameter("@Detail", System.Data.SqlDbType.NVarChar, 255, "Detail"), New System.Data.SqlClient.SqlParameter("@Code", System.Data.SqlDbType.NVarChar, 10, "Code"), New System.Data.SqlClient.SqlParameter("@DocClass", System.Data.SqlDbType.NVarChar, 16, "DocClass"), New System.Data.SqlClient.SqlParameter("@DocType", System.Data.SqlDbType.NVarChar, 16, "DocType"), New System.Data.SqlClient.SqlParameter("@userid", System.Data.SqlDbType.NVarChar, 50, "userid"), New System.Data.SqlClient.SqlParameter("@lastupdt", System.Data.SqlDbType.DateTime, 4, "lastupdt"), New System.Data.SqlClient.SqlParameter("@CompleteDate", System.Data.SqlDbType.DateTime, 4, "CompleteDate")})
        '
        'SqlUpdateCommand1
        '
        Me.SqlUpdateCommand1.CommandText = resources.GetString("SqlUpdateCommand1.CommandText")
        Me.SqlUpdateCommand1.Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@TranID", System.Data.SqlDbType.[Decimal], 9, System.Data.ParameterDirection.Input, False, CType(18, Byte), CType(0, Byte), "TranID", System.Data.DataRowVersion.Current, Nothing), New System.Data.SqlClient.SqlParameter("@ProcessDateTime", System.Data.SqlDbType.DateTime, 4, "ProcessDateTime"), New System.Data.SqlClient.SqlParameter("@TransactionType", System.Data.SqlDbType.NVarChar, 16, "TransactionType"), New System.Data.SqlClient.SqlParameter("@DocumentID", System.Data.SqlDbType.NVarChar, 16, "DocumentID"), New System.Data.SqlClient.SqlParameter("@SSN", System.Data.SqlDbType.NVarChar, 11, "SSN"), New System.Data.SqlClient.SqlParameter("@SSNAlias", System.Data.SqlDbType.NVarChar, 11, "SSNAlias"), New System.Data.SqlClient.SqlParameter("@HistoryText", System.Data.SqlDbType.NVarChar, 255, "HistoryText"), New System.Data.SqlClient.SqlParameter("@Detail", System.Data.SqlDbType.NVarChar, 255, "Detail"), New System.Data.SqlClient.SqlParameter("@Code", System.Data.SqlDbType.NVarChar, 10, "Code"), New System.Data.SqlClient.SqlParameter("@DocClass", System.Data.SqlDbType.NVarChar, 16, "DocClass"), New System.Data.SqlClient.SqlParameter("@DocType", System.Data.SqlDbType.NVarChar, 16, "DocType"), New System.Data.SqlClient.SqlParameter("@userid", System.Data.SqlDbType.NVarChar, 50, "userid"), New System.Data.SqlClient.SqlParameter("@lastupdt", System.Data.SqlDbType.DateTime, 4, "lastupdt"), New System.Data.SqlClient.SqlParameter("@CompleteDate", System.Data.SqlDbType.DateTime, 4, "CompleteDate"), New System.Data.SqlClient.SqlParameter("@Original_SQLTranID", System.Data.SqlDbType.[Decimal], 9, System.Data.ParameterDirection.Input, False, CType(18, Byte), CType(0, Byte), "SQLTranID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Code", System.Data.SqlDbType.NVarChar, 10, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Code", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CompleteDate", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CompleteDate", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Detail", System.Data.SqlDbType.NVarChar, 255, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Detail", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_DocClass", System.Data.SqlDbType.NVarChar, 16, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DocClass", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_DocType", System.Data.SqlDbType.NVarChar, 16, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DocType", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_DocumentID", System.Data.SqlDbType.NVarChar, 16, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DocumentID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_HistoryText", System.Data.SqlDbType.NVarChar, 255, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "HistoryText", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ProcessDateTime", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ProcessDateTime", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_SSN", System.Data.SqlDbType.NVarChar, 11, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SSN", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_SSNAlias", System.Data.SqlDbType.NVarChar, 11, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SSNAlias", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_TranID", System.Data.SqlDbType.[Decimal], 9, System.Data.ParameterDirection.Input, False, CType(18, Byte), CType(0, Byte), "TranID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_TransactionType", System.Data.SqlDbType.NVarChar, 16, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "TransactionType", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_lastupdt", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "lastupdt", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_userid", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "userid", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@SQLTranID", System.Data.SqlDbType.[Decimal], 9, System.Data.ParameterDirection.Input, False, CType(18, Byte), CType(0, Byte), "SQLTranID", System.Data.DataRowVersion.Current, Nothing)})
        '
        'SqlDeleteCommand1
        '
        Me.SqlDeleteCommand1.CommandText = resources.GetString("SqlDeleteCommand1.CommandText")
        Me.SqlDeleteCommand1.Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Original_SQLTranID", System.Data.SqlDbType.[Decimal], 9, System.Data.ParameterDirection.Input, False, CType(18, Byte), CType(0, Byte), "SQLTranID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Code", System.Data.SqlDbType.NVarChar, 10, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Code", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CompleteDate", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CompleteDate", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_Detail", System.Data.SqlDbType.NVarChar, 255, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Detail", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_DocClass", System.Data.SqlDbType.NVarChar, 16, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DocClass", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_DocType", System.Data.SqlDbType.NVarChar, 16, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DocType", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_DocumentID", System.Data.SqlDbType.NVarChar, 16, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DocumentID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_HistoryText", System.Data.SqlDbType.NVarChar, 255, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "HistoryText", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ProcessDateTime", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ProcessDateTime", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_SSN", System.Data.SqlDbType.NVarChar, 11, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SSN", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_SSNAlias", System.Data.SqlDbType.NVarChar, 11, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SSNAlias", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_TranID", System.Data.SqlDbType.[Decimal], 9, System.Data.ParameterDirection.Input, False, CType(18, Byte), CType(0, Byte), "TranID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_TransactionType", System.Data.SqlDbType.NVarChar, 16, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "TransactionType", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_lastupdt", System.Data.SqlDbType.DateTime, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "lastupdt", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_userid", System.Data.SqlDbType.NVarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "userid", System.Data.DataRowVersion.Original, Nothing)})
        '
        'HistAdapter
        '
        Me.HistAdapter.DeleteCommand = Me.SqlDeleteCommand1
        Me.HistAdapter.InsertCommand = Me.SqlInsertCommand1
        Me.HistAdapter.SelectCommand = Me.SqlSelectCommand1
        Me.HistAdapter.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "History", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("SQLTranID", "SQLTranID"), New System.Data.Common.DataColumnMapping("TranID", "TranID"), New System.Data.Common.DataColumnMapping("ProcessDateTime", "ProcessDateTime"), New System.Data.Common.DataColumnMapping("TransactionType", "TransactionType"), New System.Data.Common.DataColumnMapping("DocumentID", "DocumentID"), New System.Data.Common.DataColumnMapping("SSN", "SSN"), New System.Data.Common.DataColumnMapping("SSNAlias", "SSNAlias"), New System.Data.Common.DataColumnMapping("HistoryText", "HistoryText"), New System.Data.Common.DataColumnMapping("Detail", "Detail"), New System.Data.Common.DataColumnMapping("Code", "Code"), New System.Data.Common.DataColumnMapping("DocClass", "DocClass"), New System.Data.Common.DataColumnMapping("DocType", "DocType"), New System.Data.Common.DataColumnMapping("userid", "userid"), New System.Data.Common.DataColumnMapping("lastupdt", "lastupdt"), New System.Data.Common.DataColumnMapping("CompleteDate", "CompleteDate")})})
        Me.HistAdapter.UpdateCommand = Me.SqlUpdateCommand1
        '
        'HistDataset
        '
        Me.HistDataset.DataSetName = "HistDataset"
        Me.HistDataset.Locale = New System.Globalization.CultureInfo("en-US")
        Me.HistDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'SqlSelectCommand2
        '
        Me.SqlSelectCommand2.CommandText = resources.GetString("SqlSelectCommand2.CommandText")
        '
        'SqlInsertCommand2
        '
        Me.SqlInsertCommand2.CommandText = resources.GetString("SqlInsertCommand2.CommandText")
        Me.SqlInsertCommand2.Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@MaximID", System.Data.SqlDbType.VarChar, 26, "MaximID"), New System.Data.SqlClient.SqlParameter("@DocumentID", System.Data.SqlDbType.VarChar, 32, "DocumentID"), New System.Data.SqlClient.SqlParameter("@DocumentClass", System.Data.SqlDbType.VarChar, 32, "DocumentClass"), New System.Data.SqlClient.SqlParameter("@EntryInsertTime", System.Data.SqlDbType.DateTime, 8, "EntryInsertTime"), New System.Data.SqlClient.SqlParameter("@EmployeeItem", System.Data.SqlDbType.Bit, 1, "EmployeeItem"), New System.Data.SqlClient.SqlParameter("@ReceiveDate", System.Data.SqlDbType.DateTime, 8, "ReceiveDate"), New System.Data.SqlClient.SqlParameter("@DocType", System.Data.SqlDbType.VarChar, 64, "DocType"), New System.Data.SqlClient.SqlParameter("@ArchiveOnly", System.Data.SqlDbType.Bit, 1, "ArchiveOnly"), New System.Data.SqlClient.SqlParameter("@ItemStatus", System.Data.SqlDbType.VarChar, 32, "ItemStatus"), New System.Data.SqlClient.SqlParameter("@SSN", System.Data.SqlDbType.VarChar, 11, "SSN"), New System.Data.SqlClient.SqlParameter("@ParticipantNameLast", System.Data.SqlDbType.VarChar, 50, "ParticipantNameLast"), New System.Data.SqlClient.SqlParameter("@ParticipantNameMiddle", System.Data.SqlDbType.VarChar, 1, "ParticipantNameMiddle"), New System.Data.SqlClient.SqlParameter("@ParticipantNameFirst", System.Data.SqlDbType.VarChar, 50, "ParticipantNameFirst"), New System.Data.SqlClient.SqlParameter("@PatientNameLast", System.Data.SqlDbType.VarChar, 50, "PatientNameLast"), New System.Data.SqlClient.SqlParameter("@PatientNameMiddle", System.Data.SqlDbType.VarChar, 1, "PatientNameMiddle"), New System.Data.SqlClient.SqlParameter("@PatientNameFirst", System.Data.SqlDbType.VarChar, 50, "PatientNameFirst"), New System.Data.SqlClient.SqlParameter("@ProviderID", System.Data.SqlDbType.VarChar, 12, "ProviderID"), New System.Data.SqlClient.SqlParameter("@ProviderSuffix", System.Data.SqlDbType.VarChar, 3, "ProviderSuffix"), New System.Data.SqlClient.SqlParameter("@DateOfService", System.Data.SqlDbType.DateTime, 8, "DateOfService"), New System.Data.SqlClient.SqlParameter("@ScanDate", System.Data.SqlDbType.DateTime, 8, "ScanDate"), New System.Data.SqlClient.SqlParameter("@PageCount", System.Data.SqlDbType.Int, 4, "PageCount"), New System.Data.SqlClient.SqlParameter("@BatchNumber", System.Data.SqlDbType.VarChar, 24, "BatchNumber"), New System.Data.SqlClient.SqlParameter("@IndexDate", System.Data.SqlDbType.DateTime, 8, "IndexDate"), New System.Data.SqlClient.SqlParameter("@CheckAccountNumber", System.Data.SqlDbType.Int, 4, "CheckAccountNumber"), New System.Data.SqlClient.SqlParameter("@CheckAmount", System.Data.SqlDbType.Money, 8, "CheckAmount"), New System.Data.SqlClient.SqlParameter("@CheckCode", System.Data.SqlDbType.VarChar, 32, "CheckCode"), New System.Data.SqlClient.SqlParameter("@CheckDate", System.Data.SqlDbType.DateTime, 8, "CheckDate"), New System.Data.SqlClient.SqlParameter("@CheckNumber", System.Data.SqlDbType.VarChar, 16, "CheckNumber"), New System.Data.SqlClient.SqlParameter("@CompletedBy", System.Data.SqlDbType.VarChar, 64, "CompletedBy"), New System.Data.SqlClient.SqlParameter("@CompletedOn", System.Data.SqlDbType.DateTime, 8, "CompletedOn"), New System.Data.SqlClient.SqlParameter("@LastUpdateBy", System.Data.SqlDbType.VarChar, 64, "LastUpdateBy"), New System.Data.SqlClient.SqlParameter("@LastUpdateTime", System.Data.SqlDbType.DateTime, 8, "LastUpdateTime")})
        '
        'SqlUpdateCommand2
        '
        Me.SqlUpdateCommand2.CommandText = resources.GetString("SqlUpdateCommand2.CommandText")
        Me.SqlUpdateCommand2.Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@MaximID", System.Data.SqlDbType.VarChar, 26, "MaximID"), New System.Data.SqlClient.SqlParameter("@DocumentID", System.Data.SqlDbType.VarChar, 32, "DocumentID"), New System.Data.SqlClient.SqlParameter("@DocumentClass", System.Data.SqlDbType.VarChar, 32, "DocumentClass"), New System.Data.SqlClient.SqlParameter("@EntryInsertTime", System.Data.SqlDbType.DateTime, 8, "EntryInsertTime"), New System.Data.SqlClient.SqlParameter("@EmployeeItem", System.Data.SqlDbType.Bit, 1, "EmployeeItem"), New System.Data.SqlClient.SqlParameter("@ReceiveDate", System.Data.SqlDbType.DateTime, 8, "ReceiveDate"), New System.Data.SqlClient.SqlParameter("@DocType", System.Data.SqlDbType.VarChar, 64, "DocType"), New System.Data.SqlClient.SqlParameter("@ArchiveOnly", System.Data.SqlDbType.Bit, 1, "ArchiveOnly"), New System.Data.SqlClient.SqlParameter("@ItemStatus", System.Data.SqlDbType.VarChar, 32, "ItemStatus"), New System.Data.SqlClient.SqlParameter("@SSN", System.Data.SqlDbType.VarChar, 11, "SSN"), New System.Data.SqlClient.SqlParameter("@ParticipantNameLast", System.Data.SqlDbType.VarChar, 50, "ParticipantNameLast"), New System.Data.SqlClient.SqlParameter("@ParticipantNameMiddle", System.Data.SqlDbType.VarChar, 1, "ParticipantNameMiddle"), New System.Data.SqlClient.SqlParameter("@ParticipantNameFirst", System.Data.SqlDbType.VarChar, 50, "ParticipantNameFirst"), New System.Data.SqlClient.SqlParameter("@PatientNameLast", System.Data.SqlDbType.VarChar, 50, "PatientNameLast"), New System.Data.SqlClient.SqlParameter("@PatientNameMiddle", System.Data.SqlDbType.VarChar, 1, "PatientNameMiddle"), New System.Data.SqlClient.SqlParameter("@PatientNameFirst", System.Data.SqlDbType.VarChar, 50, "PatientNameFirst"), New System.Data.SqlClient.SqlParameter("@ProviderID", System.Data.SqlDbType.VarChar, 12, "ProviderID"), New System.Data.SqlClient.SqlParameter("@ProviderSuffix", System.Data.SqlDbType.VarChar, 3, "ProviderSuffix"), New System.Data.SqlClient.SqlParameter("@DateOfService", System.Data.SqlDbType.DateTime, 8, "DateOfService"), New System.Data.SqlClient.SqlParameter("@ScanDate", System.Data.SqlDbType.DateTime, 8, "ScanDate"), New System.Data.SqlClient.SqlParameter("@PageCount", System.Data.SqlDbType.Int, 4, "PageCount"), New System.Data.SqlClient.SqlParameter("@BatchNumber", System.Data.SqlDbType.VarChar, 24, "BatchNumber"), New System.Data.SqlClient.SqlParameter("@IndexDate", System.Data.SqlDbType.DateTime, 8, "IndexDate"), New System.Data.SqlClient.SqlParameter("@CheckAccountNumber", System.Data.SqlDbType.Int, 4, "CheckAccountNumber"), New System.Data.SqlClient.SqlParameter("@CheckAmount", System.Data.SqlDbType.Money, 8, "CheckAmount"), New System.Data.SqlClient.SqlParameter("@CheckCode", System.Data.SqlDbType.VarChar, 32, "CheckCode"), New System.Data.SqlClient.SqlParameter("@CheckDate", System.Data.SqlDbType.DateTime, 8, "CheckDate"), New System.Data.SqlClient.SqlParameter("@CheckNumber", System.Data.SqlDbType.VarChar, 16, "CheckNumber"), New System.Data.SqlClient.SqlParameter("@CompletedBy", System.Data.SqlDbType.VarChar, 64, "CompletedBy"), New System.Data.SqlClient.SqlParameter("@CompletedOn", System.Data.SqlDbType.DateTime, 8, "CompletedOn"), New System.Data.SqlClient.SqlParameter("@LastUpdateBy", System.Data.SqlDbType.VarChar, 64, "LastUpdateBy"), New System.Data.SqlClient.SqlParameter("@LastUpdateTime", System.Data.SqlDbType.DateTime, 8, "LastUpdateTime"), New System.Data.SqlClient.SqlParameter("@Original_DocumentID", System.Data.SqlDbType.VarChar, 32, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DocumentID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ArchiveOnly", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ArchiveOnly", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_BatchNumber", System.Data.SqlDbType.VarChar, 24, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "BatchNumber", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CheckAccountNumber", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CheckAccountNumber", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CheckAmount", System.Data.SqlDbType.Money, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CheckAmount", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CheckCode", System.Data.SqlDbType.VarChar, 32, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CheckCode", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CheckDate", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CheckDate", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CheckNumber", System.Data.SqlDbType.VarChar, 16, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CheckNumber", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CompletedBy", System.Data.SqlDbType.VarChar, 64, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CompletedBy", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CompletedOn", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CompletedOn", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_DateOfService", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DateOfService", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_DocType", System.Data.SqlDbType.VarChar, 64, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DocType", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_DocumentClass", System.Data.SqlDbType.VarChar, 32, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DocumentClass", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_EmployeeItem", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "EmployeeItem", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_EntryInsertTime", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "EntryInsertTime", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_IndexDate", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "IndexDate", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ItemStatus", System.Data.SqlDbType.VarChar, 32, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ItemStatus", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_LastUpdateBy", System.Data.SqlDbType.VarChar, 64, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "LastUpdateBy", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_LastUpdateTime", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "LastUpdateTime", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_MaximID", System.Data.SqlDbType.VarChar, 26, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "MaximID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_PageCount", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "PageCount", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ParticipantNameFirst", System.Data.SqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ParticipantNameFirst", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ParticipantNameLast", System.Data.SqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ParticipantNameLast", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ParticipantNameMiddle", System.Data.SqlDbType.VarChar, 1, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ParticipantNameMiddle", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_PatientNameFirst", System.Data.SqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "PatientNameFirst", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_PatientNameLast", System.Data.SqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "PatientNameLast", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_PatientNameMiddle", System.Data.SqlDbType.VarChar, 1, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "PatientNameMiddle", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ProviderID", System.Data.SqlDbType.VarChar, 12, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ProviderID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ProviderSuffix", System.Data.SqlDbType.VarChar, 3, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ProviderSuffix", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ReceiveDate", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ReceiveDate", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_SSN", System.Data.SqlDbType.VarChar, 11, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SSN", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ScanDate", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ScanDate", System.Data.DataRowVersion.Original, Nothing)})
        '
        'SqlDeleteCommand2
        '
        Me.SqlDeleteCommand2.CommandText = resources.GetString("SqlDeleteCommand2.CommandText")
        Me.SqlDeleteCommand2.Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Original_DocumentID", System.Data.SqlDbType.VarChar, 32, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DocumentID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ArchiveOnly", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ArchiveOnly", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_BatchNumber", System.Data.SqlDbType.VarChar, 24, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "BatchNumber", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CheckAccountNumber", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CheckAccountNumber", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CheckAmount", System.Data.SqlDbType.Money, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CheckAmount", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CheckCode", System.Data.SqlDbType.VarChar, 32, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CheckCode", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CheckDate", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CheckDate", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CheckNumber", System.Data.SqlDbType.VarChar, 16, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CheckNumber", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CompletedBy", System.Data.SqlDbType.VarChar, 64, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CompletedBy", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CompletedOn", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CompletedOn", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_DateOfService", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DateOfService", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_DocType", System.Data.SqlDbType.VarChar, 64, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DocType", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_DocumentClass", System.Data.SqlDbType.VarChar, 32, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "DocumentClass", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_EmployeeItem", System.Data.SqlDbType.Bit, 1, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "EmployeeItem", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_EntryInsertTime", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "EntryInsertTime", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_IndexDate", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "IndexDate", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ItemStatus", System.Data.SqlDbType.VarChar, 32, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ItemStatus", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_LastUpdateBy", System.Data.SqlDbType.VarChar, 64, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "LastUpdateBy", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_LastUpdateTime", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "LastUpdateTime", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_MaximID", System.Data.SqlDbType.VarChar, 26, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "MaximID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_PageCount", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "PageCount", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ParticipantNameFirst", System.Data.SqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ParticipantNameFirst", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ParticipantNameLast", System.Data.SqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ParticipantNameLast", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ParticipantNameMiddle", System.Data.SqlDbType.VarChar, 1, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ParticipantNameMiddle", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_PatientNameFirst", System.Data.SqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "PatientNameFirst", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_PatientNameLast", System.Data.SqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "PatientNameLast", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_PatientNameMiddle", System.Data.SqlDbType.VarChar, 1, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "PatientNameMiddle", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ProviderID", System.Data.SqlDbType.VarChar, 12, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ProviderID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ProviderSuffix", System.Data.SqlDbType.VarChar, 3, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ProviderSuffix", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ReceiveDate", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ReceiveDate", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_SSN", System.Data.SqlDbType.VarChar, 11, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SSN", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_ScanDate", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ScanDate", System.Data.DataRowVersion.Original, Nothing)})
        '
        'DocsAdapter
        '
        Me.DocsAdapter.DeleteCommand = Me.SqlDeleteCommand2
        Me.DocsAdapter.InsertCommand = Me.SqlInsertCommand2
        Me.DocsAdapter.SelectCommand = Me.SqlSelectCommand2
        Me.DocsAdapter.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "UFCWDocs", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("MaximID", "MaximID"), New System.Data.Common.DataColumnMapping("DocumentID", "DocumentID"), New System.Data.Common.DataColumnMapping("DocumentClass", "DocumentClass"), New System.Data.Common.DataColumnMapping("EntryInsertTime", "EntryInsertTime"), New System.Data.Common.DataColumnMapping("EmployeeItem", "EmployeeItem"), New System.Data.Common.DataColumnMapping("ReceiveDate", "ReceiveDate"), New System.Data.Common.DataColumnMapping("DocType", "DocType"), New System.Data.Common.DataColumnMapping("ArchiveOnly", "ArchiveOnly"), New System.Data.Common.DataColumnMapping("ItemStatus", "ItemStatus"), New System.Data.Common.DataColumnMapping("SSN", "SSN"), New System.Data.Common.DataColumnMapping("ParticipantNameLast", "ParticipantNameLast"), New System.Data.Common.DataColumnMapping("ParticipantNameMiddle", "ParticipantNameMiddle"), New System.Data.Common.DataColumnMapping("ParticipantNameFirst", "ParticipantNameFirst"), New System.Data.Common.DataColumnMapping("PatientNameLast", "PatientNameLast"), New System.Data.Common.DataColumnMapping("PatientNameMiddle", "PatientNameMiddle"), New System.Data.Common.DataColumnMapping("PatientNameFirst", "PatientNameFirst"), New System.Data.Common.DataColumnMapping("ProviderID", "ProviderID"), New System.Data.Common.DataColumnMapping("ProviderSuffix", "ProviderSuffix"), New System.Data.Common.DataColumnMapping("DateOfService", "DateOfService"), New System.Data.Common.DataColumnMapping("ScanDate", "ScanDate"), New System.Data.Common.DataColumnMapping("PageCount", "PageCount"), New System.Data.Common.DataColumnMapping("BatchNumber", "BatchNumber"), New System.Data.Common.DataColumnMapping("IndexDate", "IndexDate"), New System.Data.Common.DataColumnMapping("CheckAccountNumber", "CheckAccountNumber"), New System.Data.Common.DataColumnMapping("CheckAmount", "CheckAmount"), New System.Data.Common.DataColumnMapping("CheckCode", "CheckCode"), New System.Data.Common.DataColumnMapping("CheckDate", "CheckDate"), New System.Data.Common.DataColumnMapping("CheckNumber", "CheckNumber"), New System.Data.Common.DataColumnMapping("CompletedBy", "CompletedBy"), New System.Data.Common.DataColumnMapping("CompletedOn", "CompletedOn"), New System.Data.Common.DataColumnMapping("LastUpdateBy", "LastUpdateBy"), New System.Data.Common.DataColumnMapping("LastUpdateTime", "LastUpdateTime")})})
        Me.DocsAdapter.UpdateCommand = Me.SqlUpdateCommand2
        '
        'DocsDataset
        '
        Me.DocsDataset.DataSetName = "DocsDataset"
        Me.DocsDataset.Locale = New System.Globalization.CultureInfo("en-US")
        Me.DocsDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'holder
        '
        Me.holder.Controls.Add(Me.Label4)
        Me.holder.Controls.Add(Me.IndexDate)
        Me.holder.Controls.Add(Me.Annotate)
        Me.holder.Controls.Add(Me.RefreshGrid)
        Me.holder.Controls.Add(Me.CloseForm)
        Me.holder.Controls.Add(Me.EntireHist)
        Me.holder.Controls.Add(Me.Label3)
        Me.holder.Controls.Add(Me.ScanDate)
        Me.holder.Controls.Add(Me.Label2)
        Me.holder.Controls.Add(Me.ReceiveDate)
        Me.holder.Controls.Add(Me.Label1)
        Me.holder.Controls.Add(Me.Status)
        Me.holder.Controls.Add(Me.HistoryDataGrid)
        Me.holder.Dock = System.Windows.Forms.DockStyle.Fill
        Me.holder.Location = New System.Drawing.Point(0, 0)
        Me.holder.Name = "holder"
        Me.holder.Size = New System.Drawing.Size(424, 237)
        Me.holder.TabIndex = 0
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(344, 6)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(62, 13)
        Me.Label4.TabIndex = 23
        Me.Label4.Text = "Index Date:"
        '
        'IndexDate
        '
        Me.IndexDate.BackColor = System.Drawing.Color.White
        Me.IndexDate.Location = New System.Drawing.Point(344, 24)
        Me.IndexDate.Name = "IndexDate"
        Me.IndexDate.ReadOnly = True
        Me.IndexDate.Size = New System.Drawing.Size(73, 20)
        Me.IndexDate.TabIndex = 22
        '
        'Annotate
        '
        Me.Annotate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Annotate.Location = New System.Drawing.Point(280, 208)
        Me.Annotate.Name = "Annotate"
        Me.Annotate.Size = New System.Drawing.Size(64, 23)
        Me.Annotate.TabIndex = 21
        Me.Annotate.Text = "&Annotate"
        Me.TTip.SetToolTip(Me.Annotate, "Create a History Annotation")
        '
        'RefreshGrid
        '
        Me.RefreshGrid.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RefreshGrid.Location = New System.Drawing.Point(352, 208)
        Me.RefreshGrid.Name = "RefreshGrid"
        Me.RefreshGrid.Size = New System.Drawing.Size(64, 23)
        Me.RefreshGrid.TabIndex = 20
        Me.RefreshGrid.Text = "&Refresh"
        Me.TTip.SetToolTip(Me.RefreshGrid, "Refresh History")
        '
        'CloseForm
        '
        Me.CloseForm.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CloseForm.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CloseForm.Location = New System.Drawing.Point(208, 208)
        Me.CloseForm.Name = "CloseForm"
        Me.CloseForm.Size = New System.Drawing.Size(64, 23)
        Me.CloseForm.TabIndex = 19
        Me.CloseForm.Text = "&Close"
        Me.TTip.SetToolTip(Me.CloseForm, "Close & Exit Form")
        '
        'EntireHist
        '
        Me.EntireHist.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.EntireHist.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EntireHist.Location = New System.Drawing.Point(7, 210)
        Me.EntireHist.Name = "EntireHist"
        Me.EntireHist.Size = New System.Drawing.Size(176, 16)
        Me.EntireHist.TabIndex = 18
        Me.EntireHist.Text = "Show History For Entire SSN"
        Me.TTip.SetToolTip(Me.EntireHist, "Show Entire History")
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(248, 6)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 13)
        Me.Label3.TabIndex = 17
        Me.Label3.Text = "Scan Date:"
        '
        'ScanDate
        '
        Me.ScanDate.BackColor = System.Drawing.Color.White
        Me.ScanDate.Location = New System.Drawing.Point(248, 24)
        Me.ScanDate.Name = "ScanDate"
        Me.ScanDate.ReadOnly = True
        Me.ScanDate.Size = New System.Drawing.Size(73, 20)
        Me.ScanDate.TabIndex = 16
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(151, 6)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(76, 13)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "Receive Date:"
        '
        'ReceiveDate
        '
        Me.ReceiveDate.BackColor = System.Drawing.Color.White
        Me.ReceiveDate.Location = New System.Drawing.Point(151, 24)
        Me.ReceiveDate.Name = "ReceiveDate"
        Me.ReceiveDate.ReadOnly = True
        Me.ReceiveDate.Size = New System.Drawing.Size(73, 20)
        Me.ReceiveDate.TabIndex = 14
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(95, 13)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "Processing Status:"
        '
        'Status
        '
        Me.Status.BackColor = System.Drawing.Color.White
        Me.Status.Location = New System.Drawing.Point(7, 24)
        Me.Status.Name = "Status"
        Me.Status.ReadOnly = True
        Me.Status.Size = New System.Drawing.Size(120, 20)
        Me.Status.TabIndex = 12
        '
        'HistoryDataGrid
        '
        Me.HistoryDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.HistoryDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.HistoryDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.HistoryDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.HistoryDataGrid.ADGroupsThatCanFind = ""
        Me.HistoryDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.HistoryDataGrid.ADGroupsThatCanMultiSort = ""
        Me.HistoryDataGrid.AllowAutoSize = True
        Me.HistoryDataGrid.AllowColumnReorder = True
        Me.HistoryDataGrid.AllowCopy = True
        Me.HistoryDataGrid.AllowCustomize = True
        Me.HistoryDataGrid.AllowDelete = False
        Me.HistoryDataGrid.AllowDragDrop = False
        Me.HistoryDataGrid.AllowEdit = False
        Me.HistoryDataGrid.AllowExport = True
        Me.HistoryDataGrid.AllowFilter = True
        Me.HistoryDataGrid.AllowFind = True
        Me.HistoryDataGrid.AllowGoTo = True
        Me.HistoryDataGrid.AllowMultiSelect = False
        Me.HistoryDataGrid.AllowMultiSort = False
        Me.HistoryDataGrid.AllowNew = False
        Me.HistoryDataGrid.AllowPrint = True
        Me.HistoryDataGrid.AllowRefresh = False
        Me.HistoryDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HistoryDataGrid.AppKey = "UFCW\Claims\"
        Me.HistoryDataGrid.AutoSaveCols = True
        Me.HistoryDataGrid.BackgroundColor = System.Drawing.Color.White
        Me.HistoryDataGrid.ConfirmDelete = True
        Me.HistoryDataGrid.CopySelectedOnly = True
        Me.HistoryDataGrid.DataMember = ""
        Me.HistoryDataGrid.ExportSelectedOnly = True
        Me.HistoryDataGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.HistoryDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.HistoryDataGrid.LastGoToLine = ""
        Me.HistoryDataGrid.Location = New System.Drawing.Point(7, 54)
        Me.HistoryDataGrid.MultiSort = False
        Me.HistoryDataGrid.Name = "HistoryDataGrid"
        Me.HistoryDataGrid.ReadOnly = True
        Me.HistoryDataGrid.SetRowOnRightClick = True
        Me.HistoryDataGrid.SingleClickBooleanColumns = True
        Me.HistoryDataGrid.Size = New System.Drawing.Size(410, 148)
        Me.HistoryDataGrid.SuppressTriangle = True
        Me.HistoryDataGrid.TabIndex = 11
        '
        'HistoryImageList
        '
        Me.HistoryImageList.ImageStream = CType(resources.GetObject("HistoryImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.HistoryImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.HistoryImageList.Images.SetKeyName(0, "")
        Me.HistoryImageList.Images.SetKeyName(1, "")
        Me.HistoryImageList.Images.SetKeyName(2, "")
        Me.HistoryImageList.Images.SetKeyName(3, "")
        Me.HistoryImageList.Images.SetKeyName(4, "")
        Me.HistoryImageList.Images.SetKeyName(5, "")
        Me.HistoryImageList.Images.SetKeyName(6, "")
        Me.HistoryImageList.Images.SetKeyName(7, "")
        Me.HistoryImageList.Images.SetKeyName(8, "")
        Me.HistoryImageList.Images.SetKeyName(9, "")
        Me.HistoryImageList.Images.SetKeyName(10, "")
        Me.HistoryImageList.Images.SetKeyName(11, "")
        Me.HistoryImageList.Images.SetKeyName(12, "")
        Me.HistoryImageList.Images.SetKeyName(13, "")
        '
        'History
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(424, 237)
        Me.Controls.Add(Me.holder)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(432, 160)
        Me.Name = "History"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "History"
        CType(Me.HistDataset, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DocsDataset, System.ComponentModel.ISupportInitialize).EndInit()
        Me.holder.ResumeLayout(False)
        Me.holder.PerformLayout()
        CType(Me.HistoryDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Properties"

    Private _APPKEY As String = "UFCW\Claims\"

    Private _DocID As String = ""
    Private _SSN As String = ""
    Private _HistSQL As String = ""
    Private _DubClick As Boolean = False
    Shared _HistoryDT As DataTable

    <System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)

            _APPKEY = value
        End Set
    End Property

    Shared Property HistoryValues() As DataTable
        Get
            Return _HistoryDT
        End Get
        Set(ByVal value As DataTable)
            _HistoryDT = value
        End Set
    End Property

#End Region

    Sub New(ByVal iList As ImageList, ByVal documentID As String, ByVal ssn As String)

        Me.New()

        HistoryImageList = iList

        _DocID = documentID
        _SSN = ssn

        LoadDoc()
        LoadHist()

        SetTitle()
    End Sub

    Sub New(ByVal iList As ImageList, ByVal documentID As String)
        Me.New()

        HistoryImageList = iList

        _DocID = documentID

        LoadDoc()
        LoadHist()

        SetTitle()
    End Sub

    Public Sub RefreshHistory()
        LoadHist()
    End Sub

    Private Sub SetSettings()
        Me.Top = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)))
        Me.Height = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)))
        Me.Width = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)
    End Sub

    Private Sub SaveSettings()
        Dim WindowState As FormWindowState = Me.WindowState
        SaveSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(WindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = WindowState

    End Sub

    Private Sub SetTableStyle(ByVal dg As DataGridCustom)

        Try

            SetTableStyleColumns(dg)
            dg.StyleName = dg.Name
            dg.AppKey = _APPKEY
            dg.ContextMenuPrepare()

            RemoveHandler dg.ResetTableStyle, AddressOf TableStyleReset
            AddHandler dg.ResetTableStyle, AddressOf TableStyleReset

        Catch ex As Exception

            Throw
        Finally
        End Try

    End Sub
    Private Sub TableStyleReset(ByVal sender As Object, e As EventArgs)
        Dim dg As DataGridCustom = CType(sender, DataGridCustom)
        SetTableStyleColumns(dg)
    End Sub

    Private Sub SetTableStyleColumns(ByVal dg As DataGridCustom)

        Dim DGTS As DataGridTableStyle

        Dim TextCol As DataGridFormattableTextBoxColumn
        Dim BoolCol As DataGridColorBoolColumn
        Dim IconCol As DataGridHighlightIconColumn

        Dim ColsDV As DataView
        Dim DefaultStyleDS As DataSet
        Dim XMLName As String
        Dim ResultDT As DataTable
        Dim ColumnSequenceDV As DataView
        Dim DGTSDefault As DataGridTableStyle

        Try

            XMLName = dg.Name

            DefaultStyleDS = DataGridCustom.GetTableStyle(XMLName)

            DGTS = New DataGridTableStyle()
            DGTS.MappingName = dg.GetCurrentDataTable.TableName

            DGTS.GridColumnStyles.Clear()

            If DefaultStyleDS.Tables.Contains(XMLName & "Style") Then
                If DefaultStyleDS.Tables(XMLName & "Style").Columns.Contains("GridLineStyle") Then
                    DGTS.GridLineStyle = If(CBool(DefaultStyleDS.Tables(XMLName & "Style").Rows(0)("GridLineStyle")), DataGridLineStyle.Solid, DataGridLineStyle.None)
                End If
                If DefaultStyleDS.Tables(XMLName & "Style").Columns.Contains("RowHeadersVisible") Then
                    DGTS.RowHeadersVisible = CBool(DefaultStyleDS.Tables(XMLName & "Style").Rows(0)("RowHeadersVisible"))
                End If
            End If

            ResultDT = dg.LoadColumnsSizeAndPosition(dg.Name & "\" & DGTS.MappingName & "\ColumnSettings", DefaultStyleDS.Tables("Column"))
            ColumnSequenceDV = New DataView(ResultDT)
            ColsDV = ColumnSequenceDV

            DGTSDefault = New DataGridTableStyle() 'This can be used to establish the columns displayed by default
            DGTSDefault.MappingName = "Default"

            For IntCol As Integer = 0 To ColsDV.Count - 1

                If (IsDBNull(ColsDV(IntCol).Item("Visible")) OrElse ColsDV(IntCol).Item("Visible").ToString.Trim.Length = 0 OrElse CBool(ColsDV(IntCol).Item("Visible")) = True) Then
                    TextCol = New DataGridFormattableTextBoxColumn
                    TextCol.MappingName = CStr(ColsDV(IntCol).Item("Mapping"))
                    TextCol.HeaderText = CStr(ColsDV(IntCol).Item("HeaderText"))
                    If IsDBNull(ColsDV(IntCol).Item("Format")) = False AndAlso ColsDV(IntCol).Item("Format").ToString.Trim.Length > 0 Then
                        TextCol.Format = CStr(ColsDV(IntCol).Item("Format"))
                    End If
                    DGTSDefault.GridColumnStyles.Add(TextCol)
                End If

                If ((IsDBNull(ColsDV(IntCol).Item("Visible")) OrElse ColsDV(IntCol).Item("Visible").ToString.Trim.Length = 0 OrElse CBool(ColsDV(IntCol).Item("Visible")) = True) AndAlso (IsNothing(GetAllSettings(_APPKEY, dg.Name & "\" & DGTS.MappingName & "\Customize\ColumnSelector")) OrElse CDbl(GetSetting(_APPKEY, dg.Name & "\" & DGTS.MappingName & "\Customize\ColumnSelector", "Col " & ColsDV(IntCol).Item("Mapping").ToString & " Customize", "0")) = 1)) Then
                    If ColsDV(IntCol).Item("Type").ToString.Trim = "Text" Then
                        TextCol = New DataGridFormattableTextBoxColumn(IntCol)
                        TextCol.MappingName = CStr(ColsDV(IntCol).Item("Mapping"))
                        TextCol.HeaderText = CStr(ColsDV(IntCol).Item("HeaderText"))
                        TextCol.Width = CInt(ColsDV(IntCol).Item("SizeInPixels"))
                        TextCol.NullText = CStr(ColsDV(IntCol).Item("NullText"))
                        TextCol.TextBox.WordWrap = True

                        Try

                            If CBool(ColsDV(IntCol).Item("ReadOnly")) Then
                                TextCol.ReadOnly = True
                            End If

                        Catch ex As Exception
                        End Try

                        If IsDBNull(ColsDV(IntCol).Item("Format")) = False AndAlso ColsDV(IntCol).Item("Format").ToString.Trim.Length > 0 AndAlso ColsDV(IntCol).Item("Format").ToString.Trim <> "YesNo" Then
                            TextCol.Format = ColsDV(IntCol).Item("Format").ToString
                        ElseIf ColsDV(IntCol).Item("Format").ToString.Trim = "YesNo" Then
                            AddHandler TextCol.Formatting, AddressOf DataGridCustom.FormattingYesNo
                        End If

                        DGTS.GridColumnStyles.Add(TextCol)

                    ElseIf ColsDV(IntCol).Item("Type").ToString.Trim = "Bool" Then
                        BoolCol = New DataGridColorBoolColumn(IntCol)
                        BoolCol.MappingName = ColsDV(IntCol).Item("Mapping").ToString
                        BoolCol.HeaderText = ColsDV(IntCol).Item("HeaderText").ToString
                        BoolCol.Width = CInt(ColsDV(IntCol).Item("SizeInPixels"))
                        BoolCol.NullValue = If(IsNumeric(ColsDV(IntCol).Item("NullText").ToString.Trim), CDec(ColsDV(IntCol).Item("NullText")), 0)
                        BoolCol.TrueValue = CType("1", Decimal)
                        BoolCol.FalseValue = CType("0", Decimal)
                        BoolCol.AllowNull = False

                        Try

                            If CBool(ColsDV(IntCol).Item("ReadOnly")) Then
                                BoolCol.ReadOnly = True
                            End If

                        Catch ex As Exception
                        End Try

                        DGTS.GridColumnStyles.Add(BoolCol)

                    ElseIf ColsDV(IntCol).Item("Type").ToString.Trim = "Icon" Then

                        IconCol = New DataGridHighlightIconColumn(HistoryDataGrid)

                        IconCol.MappingName = ColsDV(IntCol).Item("Mapping").ToString
                        IconCol.HeaderText = ColsDV(IntCol).Item("HeaderText").ToString
                        IconCol.Width = CInt(ColsDV(IntCol).Item("SizeInPixels"))
                        IconCol.NullText = CStr(ColsDV(IntCol).Item("NullText"))

                        IconCol.MinimumCharWidth = CInt(ColsDV(IntCol).Item("MinimumCharWidth"))
                        IconCol.MaximumCharWidth = CInt(ColsDV(IntCol).Item("MaximumCharWidth"))

                        AddHandler IconCol.PaintCellPicture, AddressOf DetermineCellIcon
                        DGTS.GridColumnStyles.Add(IconCol)
                    End If
                End If

            Next

            dg.TableStyles.Clear()
            dg.TableStyles.Add(DGTS)
            dg.TableStyles.Add(DGTSDefault)

        Catch ex As Exception

            Throw

        Finally

            If ResultDT IsNot Nothing Then ResultDT.Dispose()
            ResultDT = Nothing

            If ColumnSequenceDV IsNot Nothing Then ColumnSequenceDV.Dispose()
            ColumnSequenceDV = Nothing

            If DGTS IsNot Nothing Then DGTS.Dispose()
            DGTS = Nothing
        End Try

    End Sub

    Private Sub DetermineCellIcon(ByRef pic As Image, ByVal cell As System.Windows.Forms.DataGridCell)
        Dim DV As DataView

        Try
            DV = HistoryDataGrid.GetDefaultDataView
            If DV IsNot Nothing AndAlso IsDBNull(DV(cell.RowNumber)("Code")) = False Then
                Select Case CInt(Asc(DV(cell.RowNumber)("Code").ToString) - &H40)
                    Case Is = 1
                        pic = HistoryImageList.Images(9)
                    Case Is = 2
                        pic = HistoryImageList.Images(7)
                    Case Is = 4
                        pic = HistoryImageList.Images(1)
                    Case Is = 8
                        pic = HistoryImageList.Images(5)
                    Case Is = 16
                        pic = HistoryImageList.Images(3)
                    Case Is = 32
                        pic = HistoryImageList.Images(2)
                    Case Is = 64
                        pic = HistoryImageList.Images(13)
                    Case Is = 128
                        pic = HistoryImageList.Images(11)
                    Case Else
                        pic = Nothing
                End Select
            Else
                pic = Nothing
            End If

        Catch ex As Exception

            Throw
        End Try
    End Sub

    Private Sub FormattingYesNo(ByRef value As Object, ByVal rowNum As Integer)
        Try
            If IsDBNull(value) = False AndAlso CBool(value) Then
                value = "Yes"
            Else
                value = "No"
            End If
        Catch ex As Exception

            Throw
        End Try
    End Sub

    Private Sub LoadDoc()
        Dim DeadlockCnt As Integer = 0
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String


        Try
ReQuery:
            DocsDataset.Clear()

            DB = CMSDALCommon.CreateDatabase($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance")
            SQLCall = "SELECT MaximID, DocumentID, DocumentClass, EntryInsertTime, EmployeeItem, ReceiveDate, DocType, ArchiveOnly, ItemStatus, SSN, ParticipantNameLast, ParticipantNameMiddle, ParticipantNameFirst, PatientNameLast, PatientNameMiddle, PatientNameFirst, ProviderID, ProviderSuffix, DateOfService, ScanDate, PageCount, BatchNumber, IndexDate, CheckAccountNumber, CheckAmount, CheckCode, CheckDate, CheckNumber, CompletedBy, CompletedOn, LastUpdateBy, LastUpdateTime FROM UFCWDocs WITH (nolock) "

            SQLCall &= " Where DocumentID = '" & _DocID & "'"

            DBCommandWrapper = DB.GetSqlStringCommand(SQLCall)
            DBCommandWrapper.CommandTimeout = 180

            DB.LoadDataSet(DBCommandWrapper, DocsDataset, "UFCWDocs")

            If DocsDataset.UFCWDocs.Rows.Count > 0 Then
                If IsDBNull(DocsDataset.UFCWDocs.Rows(0).Item("SSN")) = False Then
                    _SSN = UFCWGeneral.FormatSSN(CStr(DocsDataset.UFCWDocs.Rows(0).Item("SSN")))
                End If
                If IsDBNull(DocsDataset.UFCWDocs.Rows(0).Item("ItemStatus")) = False Then
                    Status.Text = CStr(DocsDataset.UFCWDocs.Rows(0).Item("ItemStatus"))
                End If
                If IsDBNull(DocsDataset.UFCWDocs.Rows(0).Item("ReceiveDate")) = False Then
                    ReceiveDate.Text = Format(DocsDataset.UFCWDocs.Rows(0).Item("ReceiveDate"), "MM-dd-yyyy")
                End If
                If IsDBNull(DocsDataset.UFCWDocs.Rows(0).Item("ScanDate")) = False Then
                    ScanDate.Text = Format(DocsDataset.UFCWDocs.Rows(0).Item("ScanDate"), "MM-dd-yyyy")
                End If
                If IsDBNull(DocsDataset.UFCWDocs.Rows(0).Item("IndexDate")) = False Then
                    IndexDate.Text = Format(DocsDataset.UFCWDocs.Rows(0).Item("IndexDate"), "MM-dd-yyyy")
                End If
            End If

        Catch sqlex As SqlClient.SqlException When sqlex.Number = 1205 And DeadlockCnt < 10
            DeadlockCnt += 1
            GoTo ReQuery
        Catch sqlex As SqlClient.SqlException
            Throw
        Catch ex As Exception

            Throw

        End Try
    End Sub

    Private Sub LoadHist()
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            Using WC As New GlobalCursor
                DB = CMSDALCommon.CreateDatabase($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance")

                HistDataset.Clear()

                _HistSQL = "SELECT SQLTranID, TranID, ProcessDateTime, TransactionType, DocumentID, SSN, SSNAlias, HistoryText, Detail, Code, DocClass, DocType, userid, lastupdt, CompleteDate FROM History "

                If EntireHist.Checked = False Then
                    SQLCall = _HistSQL & " Where DocumentID = '" & _DocID & "'"
                Else
                    SQLCall = _HistSQL & " Where SSN = '" & UFCWGeneral.UnFormatSSN(_SSN) & "'"
                End If

                SQLCall &= " Order By ProcessDateTime"

                DBCommandWrapper = DB.GetSqlStringCommand(SQLCall)
                DBCommandWrapper.CommandTimeout = 180

                DB.LoadDataSet(DBCommandWrapper, HistDataset, "History")

                If HistDataset.History.Columns.Contains("Icon") = False Then
                    HistDataset.History.Columns.Add("Icon")
                End If

                HistoryDataGrid.DataSource = HistDataset.History
                _HistoryDT = HistDataset.History
                SetTableStyle(HistoryDataGrid)

                HistoryDataGrid.CaptionText = HistDataset.History.Rows.Count & " History Entries:"

            End Using

        Catch sqlex As SqlClient.SqlException
            Throw
        Catch ex As Exception

            Throw
        Finally
        End Try

    End Sub

    Private Sub SetTitle()
        Me.Text = "History For " & DocsDataset.UFCWDocs.Rows(0).Item("DocumentClass").ToString & " " & _DocID
    End Sub

    Private Sub History_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        SaveSettings()
    End Sub

    Private Sub CloseForm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseForm.Click
        Me.Close()
    End Sub

    Private Sub History_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetSettings()
    End Sub

    Private Sub RefreshGrid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshGrid.Click
        LoadHist()
    End Sub

    Private Sub HistGrid_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HistoryDataGrid.DoubleClick
        _DubClick = True
    End Sub

    Private Sub Annotate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Annotate.Click
        Annotation(sender, e)
    End Sub

    Public Sub Annotation(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim AForm As Annotate
        Dim DR As DataRow
        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "Select DocumentClass, DocType From UFCWDOCS Where DocumentID = '" & _DocID & "'"
        Dim DocClass As String
        Dim DocType As String

        Try


            If HistDataset.History.Rows.Count > 0 Then
                DR = HistDataset.History.Rows(HistoryDataGrid.CurrentRowIndex)
                AForm = New Annotate(CStr(DR("DocumentID")), UFCWGeneral.UnFormatSSN(CStr(DR("SSN"))), CStr(DR("DocClass")), CStr(DR("DocType")))
            Else

                DB = CMSDALCommon.CreateDatabase($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance")

                DBCommandWrapper = DB.GetSqlStringCommand(SQLCall)
                DBCommandWrapper.CommandTimeout = 180

                Using DBDataReader As IDataReader = DB.ExecuteReader(DBCommandWrapper)
                    DBDataReader.Read()

                    DocClass = CStr(DBDataReader("DocumentClass"))
                    DocType = CStr(DBDataReader("DocType"))

                End Using

                AForm = New Annotate(_DocID, UFCWGeneral.UnFormatSSN(_SSN), DocClass, DocType)
            End If

            If AForm.ShowDialog(Me) = DialogResult.OK Then LoadHist()

        Catch ex As Exception


            Throw
        Finally

            If AForm IsNot Nothing Then AForm.Dispose()
            AForm = Nothing

        End Try

    End Sub

    Private Sub HistGrid_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles HistoryDataGrid.MouseUp
        Dim DG As DataGrid
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo

        Try
            DG = CType(sender, DataGrid)
            HTI = DG.HitTest(e.X, e.Y)

            Select Case HTI.Type
                Case Is = System.Windows.Forms.DataGrid.HitTestType.None

                Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell
                    HistoryDataGrid.CurrentRowIndex = HTI.Row
                Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader

                Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader
                    HistoryDataGrid.CurrentRowIndex = HTI.Row
                    If e.Button = MouseButtons.Left AndAlso _DubClick = True Then

                        Dim DV As DataView = HistoryDataGrid.GetDefaultDataView
                        Dim AShowForm As New AnnotationViewer(CStr(DV(HistoryDataGrid.CurrentRowIndex)("SQLTranID")))

                        AShowForm.ShowDialog(Me)
                        AShowForm.Dispose()
                    End If
                Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize

                Case Is = System.Windows.Forms.DataGrid.HitTestType.RowResize

                Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption
                    HistoryDataGrid.CurrentRowIndex = 0
                Case Is = System.Windows.Forms.DataGrid.HitTestType.ParentRows

            End Select

        Catch ex As Exception

            Throw

        Finally
            _DubClick = False
        End Try
    End Sub

    Private Sub HistGrid_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles HistoryDataGrid.KeyUp
        If e.KeyCode = Keys.F5 Then
            LoadHist()
        End If
    End Sub

End Class