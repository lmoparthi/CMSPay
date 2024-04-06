Option Infer On

Imports System.ComponentModel
Imports UFCW.WCF
Imports System.Threading.Tasks

Public Class UFCWDocsControl
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"
    Private _SearchMode As SearchMode = SearchMode.SearchSQLOnly
    Private _ReadOnlyMode As Boolean = True

    Friend WithEvents ResultsDataGridCustomContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents DisplayMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HistoryMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AnnotateMenuItem As System.Windows.Forms.ToolStripMenuItem

    Friend WithEvents ItemImages As System.Windows.Forms.ImageList
    Friend WithEvents DocTypeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents UFCWDOCSDataGrid As DataGridCustom
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents DocClassComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label

    Public ReadOnly Property [ReadOnly] As Boolean = True

    Private _HTI As System.Windows.Forms.DataGrid.HitTestInfo

    Private _PopulateUFCWDocsTask As Task(Of DataSet)
    Private _PopulateClaimMasterTask As Task(Of DataSet)

    Private _UFCWDocsDS As New DataSet
    Private _ClaimMasterDS As New DataSet

    Private _AllAvailableDocsDT As DataTable

    Private _ResultDocClassDT As DataTable
    Private _ResultDocTypeDT As DataTable

    Private _MaxID As String = ""
    Private _TaxID As Integer = -1
    Private _SSN As Integer = -1
    Private _DocID As Long = -1
    Private _BatchNumber As String = ""

    Private _TrustSW As Boolean

    Private _Disposed As Boolean = False
    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.

            If UFCWDOCSDataGrid IsNot Nothing Then
                UFCWDOCSDataGrid.Dispose()
            End If
            UFCWDOCSDataGrid = Nothing

            If _ResultDocTypeDT IsNot Nothing Then
                _ResultDocTypeDT.Dispose()
            End If
            _ResultDocTypeDT = Nothing

            If _UFCWDocsDS IsNot Nothing Then
                _UFCWDocsDS.Dispose()
            End If
            _UFCWDocsDS = Nothing

            If _ClaimMasterDS IsNot Nothing Then
                _ClaimMasterDS.Dispose()
            End If
            _ClaimMasterDS = Nothing

            If _AllAvailableDocsDT IsNot Nothing Then
                _AllAvailableDocsDT.Dispose()
            End If
            _AllAvailableDocsDT = Nothing

            If components IsNot Nothing Then
                components.Dispose()
            End If

        End If

        ' Free any unmanaged objects here.
        '
        _Disposed = True

        ' Call base class implementation.
        MyBase.Dispose(disposing)
    End Sub

#Region " Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        'UserControl overrides dispose to clean up the component list.

        Dim GridHandle As System.IntPtr = UFCWDOCSDataGrid.Handle 'this ensure the datagrid is available for thread activities

    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UFCWDocsControl))
        Me.ResultsDataGridCustomContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.DisplayMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HistoryMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AnnotateMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ItemImages = New System.Windows.Forms.ImageList(Me.components)
        Me.DocTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.DocClassComboBox = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.UFCWDOCSDataGrid = New DataGridCustom()
        Me.ResultsDataGridCustomContextMenu.SuspendLayout()
        CType(Me.UFCWDOCSDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ResultsDataGridCustomContextMenu
        '
        Me.ResultsDataGridCustomContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DisplayMenuItem, Me.HistoryMenuItem, Me.AnnotateMenuItem})
        Me.ResultsDataGridCustomContextMenu.Name = "ControlMenu"
        Me.ResultsDataGridCustomContextMenu.Size = New System.Drawing.Size(181, 92)
        '
        'DisplayMenuItem
        '
        Me.DisplayMenuItem.Name = "DisplayMenuItem"
        Me.DisplayMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.DisplayMenuItem.Text = "Display Document"
        '
        'HistoryMenuItem
        '
        Me.HistoryMenuItem.Name = "HistoryMenuItem"
        Me.HistoryMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.HistoryMenuItem.Text = "History"
        '
        'AnnotateMenuItem
        '
        Me.AnnotateMenuItem.Name = "AnnotateMenuItem"
        Me.AnnotateMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.AnnotateMenuItem.Text = "Annotate"
        '
        'ItemImages
        '
        Me.ItemImages.ImageStream = CType(resources.GetObject("ItemImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ItemImages.TransparentColor = System.Drawing.Color.Transparent
        Me.ItemImages.Images.SetKeyName(0, "")
        Me.ItemImages.Images.SetKeyName(1, "")
        Me.ItemImages.Images.SetKeyName(2, "")
        Me.ItemImages.Images.SetKeyName(3, "")
        Me.ItemImages.Images.SetKeyName(4, "")
        Me.ItemImages.Images.SetKeyName(5, "")
        Me.ItemImages.Images.SetKeyName(6, "")
        Me.ItemImages.Images.SetKeyName(7, "")
        Me.ItemImages.Images.SetKeyName(8, "")
        Me.ItemImages.Images.SetKeyName(9, "")
        Me.ItemImages.Images.SetKeyName(10, "")
        Me.ItemImages.Images.SetKeyName(11, "")
        Me.ItemImages.Images.SetKeyName(12, "")
        Me.ItemImages.Images.SetKeyName(13, "")
        Me.ItemImages.Images.SetKeyName(14, "")
        Me.ItemImages.Images.SetKeyName(15, "")
        Me.ItemImages.Images.SetKeyName(16, "")
        Me.ItemImages.Images.SetKeyName(17, "")
        '
        'DocTypeComboBox
        '
        Me.DocTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.DocTypeComboBox.FormattingEnabled = True
        Me.DocTypeComboBox.Items.AddRange(New Object() {"(all)"})
        Me.DocTypeComboBox.Location = New System.Drawing.Point(363, 2)
        Me.DocTypeComboBox.Name = "DocTypeComboBox"
        Me.DocTypeComboBox.Size = New System.Drawing.Size(210, 21)
        Me.DocTypeComboBox.TabIndex = 17
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(2, 5)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(55, 13)
        Me.Label4.TabIndex = 23
        Me.Label4.Text = "Doc Class"
        '
        'DocClassComboBox
        '
        Me.DocClassComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.DocClassComboBox.FormattingEnabled = True
        Me.DocClassComboBox.Items.AddRange(New Object() {"(all)"})
        Me.DocClassComboBox.Location = New System.Drawing.Point(67, 2)
        Me.DocClassComboBox.Name = "DocClassComboBox"
        Me.DocClassComboBox.Size = New System.Drawing.Size(210, 21)
        Me.DocClassComboBox.TabIndex = 22
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(290, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(54, 13)
        Me.Label1.TabIndex = 24
        Me.Label1.Text = "Doc Type"
        '
        'UFCWDOCSDataGrid
        '
        Me.UFCWDOCSDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.UFCWDOCSDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.UFCWDOCSDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.UFCWDOCSDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.UFCWDOCSDataGrid.ADGroupsThatCanFind = ""
        Me.UFCWDOCSDataGrid.ADGroupsThatCanMultiSort = ""
        Me.UFCWDOCSDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.UFCWDOCSDataGrid.AllowAutoSize = True
        Me.UFCWDOCSDataGrid.AllowColumnReorder = True
        Me.UFCWDOCSDataGrid.AllowCopy = True
        Me.UFCWDOCSDataGrid.AllowCustomize = True
        Me.UFCWDOCSDataGrid.AllowDelete = False
        Me.UFCWDOCSDataGrid.AllowDragDrop = False
        Me.UFCWDOCSDataGrid.AllowEdit = False
        Me.UFCWDOCSDataGrid.AllowExport = True
        Me.UFCWDOCSDataGrid.AllowFilter = True
        Me.UFCWDOCSDataGrid.AllowFind = True
        Me.UFCWDOCSDataGrid.AllowGoTo = True
        Me.UFCWDOCSDataGrid.AllowMultiSelect = False
        Me.UFCWDOCSDataGrid.AllowMultiSort = False
        Me.UFCWDOCSDataGrid.AllowNew = False
        Me.UFCWDOCSDataGrid.AllowPrint = True
        Me.UFCWDOCSDataGrid.AllowRefresh = False
        Me.UFCWDOCSDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UFCWDOCSDataGrid.AppKey = "UFCW\Claims\"
        Me.UFCWDOCSDataGrid.AutoSaveCols = True
        Me.UFCWDOCSDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.UFCWDOCSDataGrid.ColumnHeaderLabel = Nothing
        Me.UFCWDOCSDataGrid.ColumnRePositioning = False
        Me.UFCWDOCSDataGrid.ColumnResizing = False
        Me.UFCWDOCSDataGrid.ConfirmDelete = True
        Me.UFCWDOCSDataGrid.ContextMenuStrip = Me.ResultsDataGridCustomContextMenu
        Me.UFCWDOCSDataGrid.CopySelectedOnly = True
        Me.UFCWDOCSDataGrid.DataMember = ""
        Me.UFCWDOCSDataGrid.DragColumn = 0
        Me.UFCWDOCSDataGrid.ExportSelectedOnly = True
        Me.UFCWDOCSDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.UFCWDOCSDataGrid.HighlightedRow = Nothing
        Me.UFCWDOCSDataGrid.IsMouseDown = False
        Me.UFCWDOCSDataGrid.LastGoToLine = ""
        Me.UFCWDOCSDataGrid.Location = New System.Drawing.Point(0, 29)
        Me.UFCWDOCSDataGrid.MultiSort = False
        Me.UFCWDOCSDataGrid.Name = "UFCWDOCSDataGrid"
        Me.UFCWDOCSDataGrid.OldSelectedRow = Nothing
        Me.UFCWDOCSDataGrid.ParentRowsVisible = False
        Me.UFCWDOCSDataGrid.ReadOnly = True
        Me.UFCWDOCSDataGrid.SetRowOnRightClick = True
        Me.UFCWDOCSDataGrid.ShiftPressed = False
        Me.UFCWDOCSDataGrid.SingleClickBooleanColumns = True
        Me.UFCWDOCSDataGrid.Size = New System.Drawing.Size(648, 361)
        Me.UFCWDOCSDataGrid.Sort = Nothing
        Me.UFCWDOCSDataGrid.StyleName = ""
        Me.UFCWDOCSDataGrid.SubKey = ""
        Me.UFCWDOCSDataGrid.SuppressTriangle = False
        Me.UFCWDOCSDataGrid.TabIndex = 15
        '
        'UFCWDocsControl
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.DocTypeComboBox)
        Me.Controls.Add(Me.DocClassComboBox)
        Me.Controls.Add(Me.UFCWDOCSDataGrid)
        Me.Name = "UFCWDocsControl"
        Me.Size = New System.Drawing.Size(651, 393)
        Me.ResultsDataGridCustomContextMenu.ResumeLayout(False)
        CType(Me.UFCWDOCSDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region "Properties"

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Determines if control is in Read Only Mode.")>
    Public Property ReadOnlyMode() As Boolean
        Get
            Return _ReadOnlyMode
        End Get
        Set(ByVal Value As Boolean)

            _ReadOnlyMode = Value

        End Set
    End Property

    <Browsable(True), System.ComponentModel.Description("Search Environment(s).")>
    Public Property Mode() As SearchMode
        Get
            Return _SearchMode
        End Get
        Set(ByVal value As SearchMode)
            _SearchMode = value
        End Set
    End Property

    <Browsable(True), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property SSN() As Integer
        Get
            Return _SSN
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property DocID() As Long
        Get
            Return _DocID
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property DocumentsFound() As Integer
        Get
            Return If(_UFCWDocsDS Is Nothing OrElse _UFCWDocsDS.Tables.Count < 1, 0I, _UFCWDocsDS.Tables(0).Rows.Count)
        End Get
    End Property

#End Region

    Public Sub LoadUFCWDOCSBySSNAndTaxID(ByVal ssn As Integer, ByVal taxID As Integer)
        LoadUFCWDOCSBySSNAndTaxID(ssn, taxID, Nothing, Nothing, UFCWGeneralAD.REGMEmployeeAccess)
    End Sub

    Public Sub LoadUFCWDOCSBySSNAndTaxID(ByVal ssn As Integer, ByVal taxID As Integer, ByVal trustsw As Boolean)
        LoadUFCWDOCSBySSNAndTaxID(ssn, taxID, Nothing, Nothing, trustsw)
    End Sub

    Public Sub LoadUFCWDOCSBySSNAndTaxID(ByVal ssn As Integer, ByVal taxID As Integer, ByVal selectedDate As Date, ByVal trustsw As Boolean)
        LoadUFCWDOCSBySSNAndTaxID(ssn, taxID, selectedDate, selectedDate, trustsw)
    End Sub

    Private Sub CollectUFCWDocsInfoBySSNAndTaxID(ByVal ssn As Integer, ByVal taxID As Integer, ByVal beginDate As Nullable(Of Date), ByVal endDate As Nullable(Of Date))
        _PopulateUFCWDocsTask = Task(Of DataSet).Factory.StartNew(Function() UFCWDocsDAL.GetUFCWDocsFromSSNAndTAXID(ssn, taxID, beginDate, endDate, _UFCWDocsDS))
    End Sub

    Private Sub CollectClaimsMasterInfoBySSNAndTaxID(ByVal ssn As Integer, ByVal taxID As Integer, ByVal beginDate As Nullable(Of Date), ByVal endDate As Nullable(Of Date))
        _PopulateClaimMasterTask = Task(Of DataSet).Factory.StartNew(Function() CMSDALFDBMD.RetrieveClaimMasterDocsBySSNTaxID(ssn, taxID, beginDate, endDate, _ClaimMasterDS))
    End Sub

    Public Sub LoadUFCWDOCSBySSNAndTaxID(ByVal ssn As Integer, ByVal taxID As Integer, ByVal beginDate As Nullable(Of Date), ByVal endDate As Nullable(Of Date), ByVal trustsw As Boolean)

        Try

            UFCWDOCSDataGrid.SuspendLayout()
            UFCWDOCSDataGrid.DataSource = Nothing

            ClearData()

            _TaxID = taxID
            _SSN = ssn

            If _SearchMode = SearchMode.SearchDB2AndSQL Then
                CollectClaimsMasterInfoBySSNAndTaxID(ssn, taxID, beginDate, endDate)
            End If
            CollectUFCWDocsInfoBySSNAndTaxID(ssn, taxID, beginDate, endDate)

            PopulateDocumentsInfo(trustsw)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Delegate Sub LoadUFCWDOCSFromSSNDelegate(ssn As Integer, ByVal beginDate As Nullable(Of Date), ByVal endDate As Nullable(Of Date), trustSW As Boolean)

    Public Sub LoadUFCWDOCSFromSSN(ByVal ssn As Integer)

        Try

            LoadUFCWDOCSFromSSN(ssn, Nothing, Nothing, UFCWGeneralAD.REGMEmployeeAccess)

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Sub LoadUFCWDOCSFromSSN(ByVal ssn As Integer, ByVal trustSW As Boolean)

        Try

            LoadUFCWDOCSFromSSN(ssn, Nothing, Nothing, trustSW)

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Sub LoadUFCWDOCSFromSSN(ByVal ssn As Integer, ByVal selectedDate As Date, ByVal trustSW As Boolean)

        Try

            LoadUFCWDOCSFromSSN(ssn, selectedDate, selectedDate, trustSW)

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Sub LoadUFCWDOCSFromSSN(ByVal ssn As Integer, ByVal beginDate As Nullable(Of Date), ByVal endDate As Nullable(Of Date), ByVal trustSW As Boolean)

        Try

            If UFCWDOCSDataGrid.InvokeRequired Then
                BeginInvoke(New LoadUFCWDOCSFromSSNDelegate(AddressOf LoadUFCWDOCSFromSSNPrivate), New Object() {ssn, beginDate, endDate, trustSW})
            Else
                LoadUFCWDOCSFromSSNPrivate(ssn, beginDate, endDate, trustSW)
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub LoadUFCWDOCSFromSSNPrivate(ByVal ssn As Integer, ByVal beginDate As Nullable(Of Date), ByVal endDate As Nullable(Of Date), ByVal trustSW As Boolean)

        Try

            UFCWDOCSDataGrid.SuspendLayout()
            UFCWDOCSDataGrid.DataSource = Nothing

            ClearData()

            _SSN = ssn
            _TrustSW = trustSW

            If _SearchMode = SearchMode.SearchDB2AndSQL Then
                CollectClaimsMasterInfoBySSN(ssn, beginDate, endDate)
            End If
            CollectUFCWDocsInfoBySSN(ssn, beginDate, endDate)

            PopulateDocumentsInfo(trustSW)

        Catch ex As Exception

            Throw
        End Try
    End Sub

    Private Sub CollectUFCWDocsInfoBySSN(ByVal ssn As Integer, ByVal beginDate As Nullable(Of Date), ByVal endDate As Nullable(Of Date))
        Try

            _PopulateUFCWDocsTask = Task(Of DataSet).Factory.StartNew(Function() UFCWDocsDAL.GetUFCWDocsFromSSN(ssn, beginDate, endDate, _UFCWDocsDS))

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub CollectClaimsMasterInfoBySSN(ByVal ssn As Integer, ByVal beginDate As Nullable(Of Date), ByVal endDate As Nullable(Of Date))
        Try

            _PopulateClaimMasterTask = Task(Of DataSet).Factory.StartNew(Function() CMSDALFDBMD.RetrieveClaimMasterDocsBySSN(ssn, beginDate, endDate, _ClaimMasterDS))

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub PopulateDocumentsInfo(ByVal trustSW As Boolean)

        Try

            If _PopulateUFCWDocsTask IsNot Nothing Then
                If _PopulateClaimMasterTask IsNot Nothing Then
                    Task.WaitAll(_PopulateUFCWDocsTask, _PopulateClaimMasterTask)
                    _ClaimMasterDS = _PopulateClaimMasterTask.Result
                Else
                    _PopulateUFCWDocsTask.Wait()
                End If

                _UFCWDocsDS = _PopulateUFCWDocsTask.Result

                _PopulateClaimMasterTask = Nothing
                _PopulateUFCWDocsTask = Nothing

                _AllAvailableDocsDT = ApplySecuritytoResults(trustSW)

                UFCWDOCSDataGrid.ResumeLayout()

                LoadUFCWClasses()

            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Sub LoadUFCWDOCSByReceivedDate(ByVal selectedDate As Date)
        Try

            LoadUFCWDOCSByReceivedDate(selectedDate, selectedDate, UFCWGeneralAD.REGMEmployeeAccess)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub LoadUFCWDOCSByReceivedDate(ByVal selectedDate As Date, ByVal trustsw As Boolean)
        Try

            LoadUFCWDOCSByReceivedDate(selectedDate, selectedDate, trustsw)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub CollectUFCWDocsInfoByReceivedDate(ByVal beginDate As Nullable(Of Date), ByVal endDate As Nullable(Of Date))
        Try

            _PopulateUFCWDocsTask = Task(Of DataSet).Factory.StartNew(Function() UFCWDocsDAL.GetUFCWDocsByReceivedDateCurrentUser(beginDate, endDate, _UFCWDocsDS))

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub CollectClaimsMasterInfoByReceivedDate(ByVal beginDate As Nullable(Of Date), ByVal endDate As Nullable(Of Date))
        Try

            _PopulateClaimMasterTask = Task(Of DataSet).Factory.StartNew(Function() CMSDALFDBMD.RetrieveClaimMasterDocsByReceivedDate(beginDate, endDate, _ClaimMasterDS))

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub LoadUFCWDOCSByReceivedDate(ByVal beginDate As Date, ByVal endDate As Date, ByVal trustsw As Boolean)

        Try

            UFCWDOCSDataGrid.SuspendLayout()
            UFCWDOCSDataGrid.DataSource = Nothing

            ClearData()

            If _SearchMode = SearchMode.SearchDB2AndSQL Then
                CollectClaimsMasterInfoByReceivedDate(beginDate, endDate)
            End If
            CollectUFCWDocsInfoByReceivedDate(beginDate, endDate)

            PopulateDocumentsInfo(trustsw)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub LoadUFCWDOCSByReceivedDateAllDocClasses(ByVal selectedDate As Date)
        Try

            LoadUFCWDOCSByReceivedDateAllDocClasses(selectedDate, selectedDate, UFCWGeneralAD.REGMEmployeeAccess)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub LoadUFCWDOCSByReceivedDateAllDocClasses(ByVal selectedDate As Date, ByVal trustsw As Boolean)
        Try

            LoadUFCWDOCSByReceivedDateAllDocClasses(selectedDate, selectedDate, trustsw)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub CollectUFCWDocsInfoByReceivedDateAllDocClasses(ByVal beginDate As Nullable(Of Date), ByVal endDate As Nullable(Of Date))
        Try

            _PopulateUFCWDocsTask = Task(Of DataSet).Factory.StartNew(Function() UFCWDocsDAL.GetUFCWDocsByReceivedDateAllUsers(beginDate, endDate, _UFCWDocsDS))

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub CollectClaimsMasterInfoByReceivedDateAllDocClasses(ByVal beginDate As Nullable(Of Date), ByVal endDate As Nullable(Of Date))
        Try

            _PopulateClaimMasterTask = Task(Of DataSet).Factory.StartNew(Function() CMSDALFDBMD.RetrieveClaimMasterDocsByReceivedDate(beginDate, endDate, _ClaimMasterDS))

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub LoadUFCWDOCSByReceivedDateAllDocClasses(ByVal beginDate As Date, ByVal endDate As Date, ByVal trustsw As Boolean)

        Try

            UFCWDOCSDataGrid.SuspendLayout()
            UFCWDOCSDataGrid.DataSource = Nothing

            ClearData()

            If _SearchMode = SearchMode.SearchDB2AndSQL Then
                CollectClaimsMasterInfoByReceivedDateAllDocClasses(beginDate, endDate)
            End If
            CollectUFCWDocsInfoByReceivedDateAllDocClasses(beginDate, endDate)

            PopulateDocumentsInfo(trustsw)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub CollectUFCWDocsInfoByMaxID(ByVal maxID As String)
        _PopulateUFCWDocsTask = Task(Of DataSet).Factory.StartNew(Function() UFCWDocsDAL.GetUFCWDocsFromMaximID(maxID, _UFCWDocsDS))
    End Sub

    Private Sub CollectClaimsMasterInfoByMaxID(ByVal maxID As String)
        _PopulateClaimMasterTask = Task(Of DataSet).Factory.StartNew(Function() CMSDALFDBMD.RetrieveClaimMasterDocsByMaxID(maxID, _ClaimMasterDS))
    End Sub

    Public Sub LoadUFCWDOCSFromMaxID(ByVal maxID As String, Optional trustSW As Boolean = False)

        Try

            UFCWDOCSDataGrid.SuspendLayout()
            UFCWDOCSDataGrid.DataSource = Nothing

            ClearData()

            _MaxID = maxID

            If _SearchMode = SearchMode.SearchDB2AndSQL Then
                CollectClaimsMasterInfoByMaxID(maxID)
            End If
            CollectUFCWDocsInfoByMaxID(maxID)

            PopulateDocumentsInfo(trustSW)

        Catch ex As Exception

            Throw
        End Try
    End Sub

    Private Sub CollectUFCWDocsInfoByDocID(ByVal docID As Long)
        _PopulateUFCWDocsTask = Task(Of DataSet).Factory.StartNew(Function() UFCWDocsDAL.GetUFCWDocsFromDOCID(docID.ToString, _UFCWDocsDS))
    End Sub

    Private Sub CollectClaimsMasterInfoByDocID(ByVal docID As Long)
        _PopulateClaimMasterTask = Task(Of DataSet).Factory.StartNew(Function() CMSDALFDBMD.RetrieveClaimMasterDocsByDocID(docID, _ClaimMasterDS))
    End Sub

    Public Sub LoadUFCWDOCSFromDocID(ByVal docID As Long, Optional trustSW As Boolean = False)

        Try

            _MaxID = ""
            _SSN = -1
            _DocID = docID

            UFCWDOCSDataGrid.SuspendLayout()
            UFCWDOCSDataGrid.DataSource = Nothing

            ClearData()

            If _SearchMode = SearchMode.SearchDB2AndSQL Then
                CollectClaimsMasterInfoByDocID(docID)
            End If
            CollectUFCWDocsInfoByDocID(docID)

            PopulateDocumentsInfo(trustSW)

        Catch ex As Exception

            Throw

        End Try
    End Sub

    Public Sub LoadUFCWDOCSFromProviderTIN(ByVal taxID As Integer)
        LoadUFCWDOCSFromProviderTIN(taxID, Nothing, Nothing, UFCWGeneralAD.REGMEmployeeAccess)
    End Sub

    Public Sub LoadUFCWDOCSFromProviderTIN(ByVal taxID As Integer, ByVal trustSW As Boolean)
        LoadUFCWDOCSFromProviderTIN(taxID, Nothing, Nothing, trustSW)
    End Sub

    Public Sub LoadUFCWDOCSFromProviderTIN(ByVal taxID As Integer, ByVal selectedDate As Date, ByVal trustSW As Boolean)
        LoadUFCWDOCSFromProviderTIN(taxID, selectedDate, selectedDate, trustSW)
    End Sub

    Private Sub CollectUFCWDocsInfoByProviderTIN(ByVal taxID As Integer, ByVal beginDate As Nullable(Of Date), ByVal endDate As Nullable(Of Date))
        _PopulateUFCWDocsTask = Task(Of DataSet).Factory.StartNew(Function() UFCWDocsDAL.GetUFCWDocsFromTAXID(taxID, beginDate, endDate, _UFCWDocsDS))
    End Sub

    Private Sub CollectClaimsMasterInfoByProviderTIN(ByVal taxID As Integer, ByVal beginDate As Nullable(Of Date), ByVal endDate As Nullable(Of Date))
        _PopulateClaimMasterTask = Task(Of DataSet).Factory.StartNew(Function() CMSDALFDBMD.RetrieveClaimMasterDocsByTaxID(taxID, beginDate, endDate, _ClaimMasterDS))
    End Sub

    Public Sub LoadUFCWDOCSFromProviderTIN(ByVal taxID As Integer, ByVal beginDate As Nullable(Of Date), ByVal endDate As Nullable(Of Date), ByVal trustSW As Boolean)

        Try

            UFCWDOCSDataGrid.SuspendLayout()
            UFCWDOCSDataGrid.DataSource = Nothing

            ClearData()

            _TaxID = taxID

            If _SearchMode = SearchMode.SearchDB2AndSQL Then
                CollectClaimsMasterInfoByProviderTIN(taxID, beginDate, endDate)
            End If
            CollectUFCWDocsInfoByProviderTIN(taxID, beginDate, endDate)

            PopulateDocumentsInfo(trustSW)

        Catch ex As Exception

            Throw

        End Try
    End Sub

    Public Sub LoadUFCWDOCSFromBATCH(ByVal batchNumber As String)

        LoadUFCWDOCSFromBATCH(batchNumber, UFCWGeneralAD.REGMEmployeeAccess)

    End Sub

    Private Sub CollectUFCWDocsInfoByBatch(ByVal batchNumber As String)
        _PopulateUFCWDocsTask = Task(Of DataSet).Factory.StartNew(Function() UFCWDocsDAL.GetUFCWDocsFromBATCH(batchNumber, _UFCWDocsDS))
    End Sub

    Private Sub CollectClaimsMasterInfoByBatch(ByVal batchNumber As String)
        _PopulateClaimMasterTask = Task(Of DataSet).Factory.StartNew(Function() CMSDALFDBMD.RetrieveClaimMasterDocsByBatch(batchNumber, _ClaimMasterDS))
    End Sub
    Public Sub LoadUFCWDOCSFromBATCH(ByVal batchNumber As String, ByVal trustSW As Boolean, ufcwDocsDS As DataSet, Optional claimMasterDS As DataSet = Nothing)

        _BatchNumber = batchNumber

        Dim FilteredResultDT As DataTable
        Dim NonTrustMembersDT As DataTable

        Try

            UFCWDOCSDataGrid.SuspendLayout()

            If _UFCWDocsDS Is Nothing Then
                _UFCWDocsDS = UFCWDocsDAL.GetUFCWDocsFromBATCH(_BatchNumber)
            End If

            If _UFCWDocsDS.Tables(0).Rows.Count > 0 Then
                If _TrustSW = False Then

                    Dim DistinctSSNosDT As DataTable = CMSDALCommon.SelectDistinctAndSorted("", _UFCWDocsDS.Tables(0), "SSN")

                    If DistinctSSNosDT.Rows.Count = 1 Then
                        _SSN = CInt(DistinctSSNosDT.Rows(0)("SSN"))
                    End If

                    Dim UISyncContext = TaskScheduler.FromCurrentSynchronizationContext()

                    Dim NonTrustMembersTask As Task(Of DataTable) = Task(Of DataTable).Factory.StartNew(Function() NonTrustSSNs(_UFCWDocsDS.Tables(0), DistinctSSNosDT))
                    Dim NonTrustMembersCallBack = NonTrustMembersTask.ContinueWith(Sub(antecedent)
                                                                                       NonTrustMembersDT = antecedent.Result

                                                                                       If NonTrustMembersDT.Rows.Count < 1 Then
                                                                                           FilteredResultDT = New DataTable 'nothing to show as all SSN's in results require TRUST access
                                                                                       Else
                                                                                           FilteredResultDT = JoinTablesBySSN(_UFCWDocsDS.Tables(0), NonTrustMembersDT)
                                                                                       End If

                                                                                       _AllAvailableDocsDT = FilteredResultDT

                                                                                       UFCWDOCSDataGrid.DataSource = FilteredResultDT
                                                                                       UFCWDOCSDataGrid.SetTableStyle()
                                                                                       UFCWDOCSDataGrid.Sort = If(UFCWDOCSDataGrid.LastSortedBy, UFCWDOCSDataGrid.DefaultSort)
                                                                                       UFCWDOCSDataGrid.ContextMenuPrepare(ResultsDataGridCustomContextMenu)

                                                                                   End Sub, UISyncContext)

                Else
                    _AllAvailableDocsDT = _UFCWDocsDS.Tables(0)

                    UFCWDOCSDataGrid.DataSource = _UFCWDocsDS.Tables(0)
                    UFCWDOCSDataGrid.SetTableStyle()
                    UFCWDOCSDataGrid.Sort = If(UFCWDOCSDataGrid.LastSortedBy, UFCWDOCSDataGrid.DefaultSort)
                    UFCWDOCSDataGrid.ContextMenuPrepare(ResultsDataGridCustomContextMenu)

                End If
            End If

            LoadUFCWClasses()

        Catch ex As Exception

            Throw

        Finally
            UFCWDOCSDataGrid.ResumeLayout()

        End Try

    End Sub

    Public Sub LoadUFCWDOCSFromBATCH(ByVal batchNumber As String, ByVal trustSW As Boolean)
        Try

            UFCWDOCSDataGrid.SuspendLayout()
            UFCWDOCSDataGrid.DataSource = Nothing

            ClearData()

            _BatchNumber = batchNumber

            If _SearchMode = SearchMode.SearchDB2AndSQL Then
                CollectClaimsMasterInfoByBatch(batchNumber)
            End If
            CollectUFCWDocsInfoByBatch(batchNumber)

            PopulateDocumentsInfo(trustSW)

        Catch ex As Exception

            Throw

        End Try

    End Sub

    Private Function ApplySecuritytoResults(trustsw As Boolean) As DataTable

        Dim FilteredResultDT As DataTable
        Dim CombinedResultDT As DataTable
        Dim NonTrustMembersDT As DataTable
        Dim DistinctSSNosDT As DataTable

        Select Case True
            Case _UFCWDocsDS IsNot Nothing AndAlso _UFCWDocsDS.Tables(0).Rows.Count > 0 AndAlso _ClaimMasterDS IsNot Nothing AndAlso _ClaimMasterDS.Tables(0).Rows.Count > 0
                CombinedResultDT = MergeTables(_UFCWDocsDS, _ClaimMasterDS)
            Case (_UFCWDocsDS Is Nothing OrElse _UFCWDocsDS.Tables(0).Rows.Count < 1) AndAlso _ClaimMasterDS IsNot Nothing AndAlso _ClaimMasterDS.Tables(0).Rows.Count > 0
                CombinedResultDT = _ClaimMasterDS.Tables(0)
            Case _UFCWDocsDS IsNot Nothing AndAlso _UFCWDocsDS.Tables(0).Rows.Count > 0 AndAlso (_ClaimMasterDS Is Nothing OrElse _ClaimMasterDS.Tables(0).Rows.Count < 1)
                CombinedResultDT = _UFCWDocsDS.Tables(0)
            Case Else
                UFCWDOCSDataGrid.DataSource = Nothing
                UFCWDOCSDataGrid.CaptionText = "No results found."

                Return Nothing
        End Select

        CombinedResultDT.TableName = "UFCWDocs"

        If CombinedResultDT.Rows.Count > 0 Then

            DistinctSSNosDT = CMSDALCommon.SelectDistinctAndSorted("", CombinedResultDT, "SSN")

            If DistinctSSNosDT.Rows.Count = 1 Then
                _SSN = CInt(DistinctSSNosDT.Rows(0)("SSN"))
            End If

            If trustsw = False Then
                UFCWDOCSDataGrid.CaptionText = "Searching..."

                'Identify all the members who have a TRUST_SW = 0 (Non Employee)
                NonTrustMembersDT = NonTrustSSNs(CombinedResultDT, DistinctSSNosDT)

                If NonTrustMembersDT.Rows.Count < 1 AndAlso trustsw = False Then
                    FilteredResultDT = New DataTable 'nothing to show as all SSN's in results require TRUST access
                Else
                    FilteredResultDT = JoinTablesBySSN(CombinedResultDT, NonTrustMembersDT)
                End If

                If FilteredResultDT IsNot Nothing AndAlso FilteredResultDT.Rows.Count > 0 Then
                    FilteredResultDT.TableName = "UFCWDocs"
                    UFCWDOCSDataGrid.DataSource = FilteredResultDT
                    UFCWDOCSDataGrid.SetTableStyle()
                    UFCWDOCSDataGrid.Sort = If(UFCWDOCSDataGrid.LastSortedBy, UFCWDOCSDataGrid.DefaultSort)
                    UFCWDOCSDataGrid.ContextMenuPrepare(ResultsDataGridCustomContextMenu)

                    UFCWDOCSDataGrid.CaptionText = UFCWDOCSDataGrid.GetGridRowCount & " of " & CombinedResultDT.Rows.Count & " matches "
                    Return FilteredResultDT

                Else
                    UFCWDOCSDataGrid.DataSource = Nothing
                    UFCWDOCSDataGrid.CaptionText = "Insufficient Access to view results."

                    Return Nothing

                End If

            Else
                CombinedResultDT.TableName = "UFCWDocs"
                UFCWDOCSDataGrid.DataSource = CombinedResultDT
                UFCWDOCSDataGrid.SetTableStyle()
                UFCWDOCSDataGrid.Sort = If(UFCWDOCSDataGrid.LastSortedBy, UFCWDOCSDataGrid.DefaultSort)
                UFCWDOCSDataGrid.ContextMenuPrepare(ResultsDataGridCustomContextMenu)

                UFCWDOCSDataGrid.CaptionText = UFCWDOCSDataGrid.GetGridRowCount & " of " & CombinedResultDT.Rows.Count & " matches "
                Return CombinedResultDT

            End If
        End If

    End Function

    Private Sub LoadUFCWClasses()

        If _AllAvailableDocsDT Is Nothing OrElse _AllAvailableDocsDT.Rows.Count < 1 Then Return

        Try

            DocClassComboBox.Visible = True

            RemoveHandler DocClassComboBox.SelectedIndexChanged, AddressOf DocClassComboBox_SelectedIndexChanged
            RemoveHandler DocTypeComboBox.SelectedIndexChanged, AddressOf DocTypeComboBox_SelectedIndexChanged

            Dim UISyncContext = TaskScheduler.FromCurrentSynchronizationContext()

            Dim PopulateDocClassTask As Task(Of DataTable) = Task(Of DataTable).Factory.StartNew(Function() CMSDALCommon.SelectDistinctAndSorted("Docclass", _AllAvailableDocsDT, "DocumentClass", True))
            Dim PopulateDocClassCallBack = PopulateDocClassTask.ContinueWith(Sub(antecedent)
                                                                                 _ResultDocClassDT = antecedent.Result
                                                                                 DocClassComboBox.DataSource = _ResultDocClassDT
                                                                                 DocClassComboBox.DisplayMember = "DocumentClass"
                                                                                 AddHandler DocClassComboBox.SelectedIndexChanged, AddressOf DocClassComboBox_SelectedIndexChanged

                                                                             End Sub, UISyncContext)

            Dim PopulateDocTypeTask As Task(Of DataTable) = Task(Of DataTable).Factory.StartNew(Function() CMSDALCommon.SelectDistinctAndSorted("DocType", _AllAvailableDocsDT, "DocType", True))
            Dim PopulateDocTypeCallBack = PopulateDocTypeTask.ContinueWith(Sub(antecedent)
                                                                               _ResultDocTypeDT = antecedent.Result
                                                                               DocTypeComboBox.DataSource = _ResultDocTypeDT
                                                                               DocTypeComboBox.DisplayMember = "DocType"
                                                                               AddHandler DocTypeComboBox.SelectedIndexChanged, AddressOf DocTypeComboBox_SelectedIndexChanged

                                                                           End Sub, UISyncContext)

        Catch ex As Exception

            Throw

        End Try
    End Sub

    Private Sub Resultsdisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DisplayMenuItem.Click '', UFCWDOCSDataGrid.DoubleClick
        DisplayDocument()
    End Sub

    Public Sub Clearall()

        _MaxID = ""
        _SSN = -1
        _DocID = -1

        ClearUI()
        ClearData()

    End Sub

    Private Sub ClearUI()

        If UFCWDOCSDataGrid IsNot Nothing Then
            UFCWDOCSDataGrid.DataBindings.Clear()
            UFCWDOCSDataGrid.DataSource = Nothing
            UFCWDOCSDataGrid.TableStyles.Clear()
            UFCWDOCSDataGrid.CaptionText = ""
        End If

        DocClassComboBox.DataSource = Nothing
        DocTypeComboBox.DataSource = Nothing

        If DocClassComboBox.DataSource Is Nothing Then DocClassComboBox.Items.Clear()
        If DocTypeComboBox.DataSource Is Nothing Then DocTypeComboBox.Items.Clear()

    End Sub

    Private Sub ClearData()

        _UFCWDocsDS = Nothing
        _ClaimMasterDS = Nothing
        _AllAvailableDocsDT = Nothing

        _SSN = -1
        _TaxID = -1
        _DocID = -1
        _MaxID = ""
        _BatchNumber = ""

    End Sub

    Private Sub DocClassComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DocClassComboBox.SelectedIndexChanged

        Dim DV As DataView
        Dim DT As DataTable = New DataTable
        Dim DRV As DataRowView

        If DocTypeComboBox.SelectedIndex = -1 Then Return

        Try
            If _AllAvailableDocsDT IsNot Nothing Then

                If DocClassComboBox.Text <> "(all)" Then

                    DT = _AllAvailableDocsDT.Clone()
                    DV = New DataView(_AllAvailableDocsDT, "DocumentClass='" & DocClassComboBox.Text & "'", "DocType", DataViewRowState.CurrentRows)
                    If DV.Count > 0 Then
                        For Each DRV In DV
                            DT.LoadDataRow(DRV.Row.ItemArray, True)
                        Next
                    End If

                    UFCWDOCSDataGrid.DataSource = DT

                    DocTypeComboBox.DataSource = Nothing
                    _ResultDocTypeDT = CMSDALCommon.SelectDistinctAndSorted("DocType", DT, "DocType", True)
                    DocTypeComboBox.DataSource = _ResultDocTypeDT
                    DocTypeComboBox.DisplayMember = "DocType"

                Else
                    _ResultDocTypeDT = CMSDALCommon.SelectDistinctAndSorted("DocType", _AllAvailableDocsDT, "DocType", True)
                    DocTypeComboBox.DataSource = _ResultDocTypeDT
                    DocTypeComboBox.DisplayMember = "DocType"
                End If

                UFCWDOCSDataGrid.CaptionText = UFCWDOCSDataGrid.GetGridRowCount & " of " & _AllAvailableDocsDT.Rows.Count & " matches "

            End If

        Catch ex As Exception

            Throw

        End Try
    End Sub

    Private Sub DocTypeComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DocTypeComboBox.SelectedIndexChanged

        Dim DV As DataView
        Dim DT As New DataTable
        Dim DRV As DataRowView
        Dim Filter As String

        If DocTypeComboBox.SelectedIndex = -1 Then Return

        Try
            If _AllAvailableDocsDT IsNot Nothing AndAlso _AllAvailableDocsDT.Rows.Count > 0 Then

                If DocClassComboBox.Text <> "(all)" AndAlso DocTypeComboBox.Text <> "(all)" Then
                    Filter = "DocumentClass='" & DocClassComboBox.Text & "' and DocType='" & DocTypeComboBox.Text & "'"
                ElseIf DocClassComboBox.Text <> "(all)" AndAlso DocTypeComboBox.Text = "(all)" Then
                    Filter = "DocumentClass='" & DocClassComboBox.Text & "'"
                ElseIf DocClassComboBox.Text = "(all)" AndAlso DocTypeComboBox.Text <> "(all)" Then
                    Filter = "DocType='" & DocTypeComboBox.Text & "'"
                Else
                    Filter = ""
                End If

                DT = _AllAvailableDocsDT.Clone()
                DV = New DataView(_AllAvailableDocsDT, Filter, "DocType", DataViewRowState.CurrentRows)
                If DV.Count > 0 Then
                    For Each DRV In DV
                        DT.LoadDataRow(DRV.Row.ItemArray, True)
                    Next
                    UFCWDOCSDataGrid.DataSource = DT
                End If
                UFCWDOCSDataGrid.Sort = If(UFCWDOCSDataGrid.LastSortedBy, UFCWDOCSDataGrid.DefaultSort)

                UFCWDOCSDataGrid.CaptionText = UFCWDOCSDataGrid.GetGridRowCount & " of " & _AllAvailableDocsDT.Rows.Count & " matches "
            End If

        Catch ex As Exception

            Throw

        End Try
    End Sub

    Private Sub HistoryMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HistoryMenuItem.Click
        Dim DT As DataTable
        Dim Style As New DataGridTableStyle()

        Try
            If UFCWDOCSDataGrid.DataSource Is Nothing Then
                Return
            ElseIf TypeOf (UFCWDOCSDataGrid.DataSource) Is DataTable Then
                DT = CType(UFCWDOCSDataGrid.DataSource, DataTable)
            End If

            Style = UFCWDOCSDataGrid.TableStyles(DT.TableName)

            If Style Is Nothing Then Return

            Using DForm As New CMSHistoryViewerForm(ItemImages, CStr(UFCWDOCSDataGrid.Item(UFCWDOCSDataGrid.CurrentRowIndex, Style.GridColumnStyles.IndexOf(Style.GridColumnStyles("DocumentID")))))
                DForm.ShowDialog(Me)
            End Using

        Catch ex As Exception

            Throw

        Finally
        End Try

    End Sub

    Private Sub AnnotateMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AnnotateMenuItem.Click
        Dim DGTS As New DataGridTableStyle()
        Dim DT As DataTable
        Dim Doc As String
        Dim DocClass As String
        Dim DocType As String
        Dim SSN As String

        Try
            If UFCWDOCSDataGrid.DataSource Is Nothing Then
                Return
            ElseIf TypeOf (UFCWDOCSDataGrid.DataSource) Is DataTable Then
                DT = CType(UFCWDOCSDataGrid.DataSource, DataTable)
            End If

            DGTS = UFCWDOCSDataGrid.TableStyles(DT.TableName)

            If DGTS Is Nothing Then Return

            If DGTS.GridColumnStyles.IndexOf(DGTS.GridColumnStyles("DocumentID")) < 0 OrElse
                    DGTS.GridColumnStyles.IndexOf(DGTS.GridColumnStyles("SSN")) < 0 OrElse DocClassComboBox.Text = "" OrElse
                    DGTS.GridColumnStyles.IndexOf(DGTS.GridColumnStyles("DocType")) < 0 Then
                Return
            End If

            Doc = CStr(UFCWDOCSDataGrid.Item(UFCWDOCSDataGrid.CurrentRowIndex, DGTS.GridColumnStyles.IndexOf(DGTS.GridColumnStyles("DocumentID"))))
            DocClass = CStr(UFCWDOCSDataGrid.Item(UFCWDOCSDataGrid.CurrentRowIndex, DGTS.GridColumnStyles.IndexOf(DGTS.GridColumnStyles("DocumentClass"))))
            DocType = CStr(UFCWDOCSDataGrid.Item(UFCWDOCSDataGrid.CurrentRowIndex, DGTS.GridColumnStyles.IndexOf(DGTS.GridColumnStyles("DocType"))))
            SSN = CStr(UFCWDOCSDataGrid.Item(UFCWDOCSDataGrid.CurrentRowIndex, DGTS.GridColumnStyles.IndexOf(DGTS.GridColumnStyles("SSN"))))

            Using AntForm As Annotate = New Annotate(Doc, SSN, DocClass, DocType)
                AntForm.ShowDialog(Me)
            End Using

        Catch ex As Exception

            Throw

        Finally

        End Try

    End Sub

    Public Function GetParticipant() As DataRow
        Dim DR As DataRow
        Try
            If _SSN > -1 Then DR = UFCWDocsDAL.RetrieveParticipantInfofromPartSSN(_SSN)

            Return DR

        Catch ex As Exception

            Throw

        End Try
    End Function

    Public Shared Function NonTrustSSNs(combinedResultDT As DataTable, distinctSSNosDT As DataTable) As DataTable

        Dim SQLQuery As String = ""
        Dim SSNRange As String = ""

        Try

            If combinedResultDT IsNot Nothing AndAlso combinedResultDT.Rows.Count > 0 Then

                distinctSSNosDT.DefaultView.Sort = "SSN"

                For Each DVR As DataRowView In distinctSSNosDT.DefaultView
                    If IsDBNull(DVR("SSN")) Then Continue For
                    SSNRange += CType(DVR("SSN"), String) & ","
                Next

                SSNRange = SSNRange.Remove(SSNRange.LastIndexOf(","))
                SQLQuery = "  SELECT DISTINCT SSNO, 0 AS TRUST_SW "
                SQLQuery += " FROM FDBEL.REG_MASTER "
                SQLQuery += " WHERE TRUST_SW = 0 AND SSNO IN ( " & SSNRange & " ) "
                SQLQuery += " FOR READ ONLY WITH UR "

                Return UFCWDocsDAL.RetrieveBatchRows(SQLQuery).Tables(0)

            End If

        Catch ex As Exception

            Throw

        End Try

    End Function

    Public Shared Function NonLocalsSSNs(combinedResultDT As DataTable, distinctSSNosDT As DataTable) As DataTable

        Dim SQLQuery As String = ""
        Dim SSNRange As String = ""

        Try

            If combinedResultDT IsNot Nothing AndAlso combinedResultDT.Rows.Count > 0 Then

                distinctSSNosDT.DefaultView.Sort = "SSN"

                For Each DVR As DataRowView In distinctSSNosDT.DefaultView
                    If IsDBNull(DVR("SSN")) Then Continue For
                    SSNRange += CType(DVR("SSN"), String) & ","
                Next

                SSNRange = SSNRange.Remove(SSNRange.LastIndexOf(","))
                SQLQuery = "  SELECT DISTINCT SSNO, 0 AS LOCAL_SW "
                SQLQuery += " FROM FDBEL.REG_MASTER "
                SQLQuery += " WHERE TRUST_SW = 0 AND LOCAL_SW = 0 AND SSNO IN ( " & SSNRange & " ) "
                SQLQuery += " FOR READ ONLY WITH UR "

                Return UFCWDocsDAL.RetrieveBatchRows(SQLQuery).Tables(0)

            End If

        Catch ex As Exception

            Throw

        End Try

    End Function

    Public Function GetBatchRows(ByVal ssn As Integer) As DataSet

        Dim SQLQuery As String = ""
        Dim SSNRange As String = ""
        Dim DRs() As DataRow

        Try
            If _UFCWDocsDS IsNot Nothing Then
                If _UFCWDocsDS.Tables(0).Rows.Count > 0 Then DRs = _UFCWDocsDS.Tables(0).Select("SSN=" & ssn)
                If DRs.Length > 0 Then
                    For I As Integer = 0 To DRs.GetUpperBound(0) - 1
                        If IsDBNull(DRs(I)("SSN")) Then Continue For
                        SSNRange += CType(DRs(I)("SSN"), String) & ","
                    Next
                    SSNRange = SSNRange.Remove(SSNRange.LastIndexOf(","))

                    SQLQuery = "  SELECT DISTINCT SSNO, 1 AS TRUST_SW "
                    SQLQuery += " FROM FDBEL.REG_MASTER "
                    SQLQuery += " WHERE TRUST_SW = 1 AND SSNO IN ( " & SSNRange & " ) "
                    SQLQuery += " FOR READ ONLY WITH UR "

                    Return UFCWDocsDAL.RetrieveBatchRows(SQLQuery)

                End If
            End If
        Catch ex As Exception

            Throw

        End Try

    End Function

    Private Sub UFCWDOCSDataGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles UFCWDOCSDataGrid.DoubleClick

        Select Case CType(sender, DataGridCustom).LastHitSpot.Type
            Case Is = System.Windows.Forms.DataGrid.HitTestType.None

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell
                DisplayDocument()
            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader
                DisplayDocument()
            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ParentRows

        End Select

    End Sub

    Private Sub DisplayDocument()

        Dim BM As BindingManagerBase
        Dim DR As DataRow
        Dim Docs() As Object
        Dim Cnt As Integer = 0
        Dim Tot As Integer = 0

        Try
            BM = Me.UFCWDOCSDataGrid.BindingContext(Me.UFCWDOCSDataGrid.DataSource, Me.UFCWDOCSDataGrid.DataMember)

            If BM.Count > 0 Then

                DR = CType(BM.Current, DataRowView).Row

                ReDim Docs(1)

                Docs(0) = DR("DocumentID")
                If Docs(0) Is System.DBNull.Value Then
                    MessageBox.Show("There is no document to display.", "No Document", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else

                    Using FNDisplay As New Display
                        FNDisplay.Display(Docs)
                        _DocID = CLng(DR("DocumentID"))
                    End Using

                End If
            End If

        Catch ex As ApplicationException

            MessageBox.Show(ex.Message, "FileNet unavailable, Restarting application may resolve connectivity issues.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        Catch ex As Exception

            Throw

        End Try
    End Sub

    Private Sub UFCWDocsControl_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

        If UFCWDOCSDataGrid IsNot Nothing Then 'save prior style changes in between searches
            UFCWDOCSDataGrid.TableStyles.Clear()
            UFCWDOCSDataGrid.DataSource = Nothing
            Me.UFCWDOCSDataGrid.Dispose()

        End If

        Me.Dispose()

    End Sub

    Private Shared Function MergeTables(primaryDS As DataSet, secondaryDS As DataSet) As DataTable

        Dim PrimaryDT As DataTable
        Dim SecondaryDT As DataTable

        Try

            If primaryDS.Tables(0).Rows.Count < 1 OrElse secondaryDS.Tables(0).Rows.Count < 1 Then Return primaryDS.Tables(0)

            PrimaryDT = primaryDS.Tables(0)
            SecondaryDT = secondaryDS.Tables(0)

            Dim Query =
                From SQL In PrimaryDT.AsEnumerable().Union(SecondaryDT.AsEnumerable())

            Return Query.CopyToDataTable

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Shared Function JoinTablesBySSN(primaryDT As DataTable, secondaryDT As DataTable) As DataTable

        Try

            If primaryDT.Rows.Count < 1 OrElse secondaryDT.Rows.Count < 1 Then Return primaryDT

            Dim Query =
                From SQL In primaryDT.AsEnumerable()
                Join DB2 In secondaryDT.AsEnumerable()
                On SQL.Field(Of Decimal)("SSN") Equals DB2.Field(Of Integer)("SSNO")
                Select SQL

            Return Query.CopyToDataTable

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Sub ResultsDataGridCustomContextMenu_Opening(sender As Object, e As CancelEventArgs) Handles ResultsDataGridCustomContextMenu.Opening

        Dim DGContextMenu As ContextMenuStrip
        Dim DG As DataGridCustom
        Dim DR As DataRow

        Try

            DGContextMenu = CType(sender, ContextMenuStrip)

            DGContextMenu.Items("AnnotateMenuItem").Available = False
            DGContextMenu.Items("DisplayMenuItem").Available = False
            DGContextMenu.Items("HistoryMenuItem").Available = False

            DG = CType(DirectCast(sender, System.Windows.Forms.ContextMenuStrip).SourceControl, DataGridCustom)
            If DG IsNot Nothing AndAlso DG.DataSource IsNot Nothing Then

                DR = DG.SelectedRowPreview

                If DR IsNot Nothing Then

                    If (DR.Table.Columns.Contains("DOCUMENTID") AndAlso Not IsDBNull(DR("DOCUMENTID"))) OrElse (DR.Table.Columns.Contains("MAXIMID") AndAlso Not IsDBNull(DR("MAXIMID"))) Then
                        DGContextMenu.Items("AnnotateMenuItem").Available = True
                        DGContextMenu.Items("DisplayMenuItem").Available = True
                    End If

                    If Not UFCWGeneralAD.CMSLocals Then
                        DGContextMenu.Items("HistoryMenuItem").Available = True
                    End If
                End If

            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub
End Class
Public Enum SearchMode
    SearchSQLOnly
    SearchDB2AndSQL
End Enum