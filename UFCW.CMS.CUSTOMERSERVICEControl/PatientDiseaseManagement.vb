Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ComponentModel


Public Class PatientDiseaseManagementForm
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"
    Private _FamilyID As Integer
    Private _RelationID As Short?
    Private _DiseaseManagementDS As New DataSet

    Friend WithEvents Panel1 As Panel
    Friend WithEvents SplitContainer As SplitContainer
    Friend WithEvents SplitContainerTop As SplitContainer
    Friend WithEvents DNPDataGrid As DataGridCustom
    Friend WithEvents SplitContainerBottom As SplitContainer
    Friend WithEvents DisincentiveLettersDataGrid As DataGridCustom
    Friend WithEvents DiseaseManagementAlertsDataGrid As DataGridCustom
    Friend WithEvents SplitContainerVeryTop As SplitContainer
    Friend WithEvents CandidateFileDataGrid As DataGridCustom
    Friend WithEvents IncentiveViewDataGrid As DataGridCustom
    Friend WithEvents ExitButton As System.Windows.Forms.Button


#Region "Public Properties"
    <DefaultValue("UFCW\Claims\"), Browsable(True), System.ComponentModel.Description("Represents the application key used when accessing the registry.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
        End Set
    End Property

    <Browsable(False), System.ComponentModel.Description("Used to load the Disease Management Data from an external source.")>
    Public Property DiseaseManagementDS() As DataSet
        Get
            Return _DiseaseManagementDS
        End Get
        Set(ByVal value As DataSet)
            _DiseaseManagementDS = value
        End Set
    End Property
#End Region

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal familyID As Integer, ByVal relationID As Short?)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _FamilyID = familyID
        _RelationID = relationID

        Me.Text &= " (FID: " & _FamilyID.ToString & " - " & _RelationID.ToString & " )"

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If components IsNot Nothing Then
                components.Dispose()
            End If
            If _DiseaseManagementDS IsNot Nothing Then _DiseaseManagementDS.Dispose()

        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ExitButton = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.SplitContainer = New System.Windows.Forms.SplitContainer()
        Me.SplitContainerTop = New System.Windows.Forms.SplitContainer()
        Me.SplitContainerVeryTop = New System.Windows.Forms.SplitContainer()
        Me.CandidateFileDataGrid = New DataGridCustom()
        Me.IncentiveViewDataGrid = New DataGridCustom()
        Me.DNPDataGrid = New DataGridCustom()
        Me.SplitContainerBottom = New System.Windows.Forms.SplitContainer()
        Me.DisincentiveLettersDataGrid = New DataGridCustom()
        Me.DiseaseManagementAlertsDataGrid = New DataGridCustom()
        Me.Panel1.SuspendLayout()
        CType(Me.SplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer.Panel1.SuspendLayout()
        Me.SplitContainer.Panel2.SuspendLayout()
        Me.SplitContainer.SuspendLayout()
        CType(Me.SplitContainerTop, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerTop.Panel1.SuspendLayout()
        Me.SplitContainerTop.Panel2.SuspendLayout()
        Me.SplitContainerTop.SuspendLayout()
        CType(Me.SplitContainerVeryTop, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerVeryTop.Panel1.SuspendLayout()
        Me.SplitContainerVeryTop.Panel2.SuspendLayout()
        Me.SplitContainerVeryTop.SuspendLayout()
        CType(Me.CandidateFileDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.IncentiveViewDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DNPDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainerBottom, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerBottom.Panel1.SuspendLayout()
        Me.SplitContainerBottom.Panel2.SuspendLayout()
        Me.SplitContainerBottom.SuspendLayout()
        CType(Me.DisincentiveLettersDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DiseaseManagementAlertsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ExitButton
        '
        Me.ExitButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ExitButton.Location = New System.Drawing.Point(741, 608)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(75, 23)
        Me.ExitButton.TabIndex = 1
        Me.ExitButton.Text = "Exit"
        Me.ExitButton.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.SplitContainer)
        Me.Panel1.Location = New System.Drawing.Point(12, 12)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(804, 590)
        Me.Panel1.TabIndex = 2
        '
        'SplitContainer
        '
        Me.SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer.Name = "SplitContainer"
        Me.SplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer.Panel1
        '
        Me.SplitContainer.Panel1.Controls.Add(Me.SplitContainerTop)
        '
        'SplitContainer.Panel2
        '
        Me.SplitContainer.Panel2.Controls.Add(Me.SplitContainerBottom)
        Me.SplitContainer.Size = New System.Drawing.Size(804, 590)
        Me.SplitContainer.SplitterDistance = 295
        Me.SplitContainer.TabIndex = 0
        '
        'SplitContainerTop
        '
        Me.SplitContainerTop.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerTop.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainerTop.Name = "SplitContainerTop"
        Me.SplitContainerTop.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainerTop.Panel1
        '
        Me.SplitContainerTop.Panel1.Controls.Add(Me.SplitContainerVeryTop)
        Me.SplitContainerTop.Panel1MinSize = 100
        '
        'SplitContainerTop.Panel2
        '
        Me.SplitContainerTop.Panel2.Controls.Add(Me.DNPDataGrid)
        Me.SplitContainerTop.Panel2MinSize = 100
        Me.SplitContainerTop.Size = New System.Drawing.Size(804, 295)
        Me.SplitContainerTop.SplitterDistance = 148
        Me.SplitContainerTop.TabIndex = 0
        '
        'SplitContainerVeryTop
        '
        Me.SplitContainerVeryTop.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerVeryTop.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainerVeryTop.Name = "SplitContainerVeryTop"
        Me.SplitContainerVeryTop.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainerVeryTop.Panel1
        '
        Me.SplitContainerVeryTop.Panel1.Controls.Add(Me.CandidateFileDataGrid)
        '
        'SplitContainerVeryTop.Panel2
        '
        Me.SplitContainerVeryTop.Panel2.Controls.Add(Me.IncentiveViewDataGrid)
        Me.SplitContainerVeryTop.Size = New System.Drawing.Size(804, 148)
        Me.SplitContainerVeryTop.SplitterDistance = 74
        Me.SplitContainerVeryTop.TabIndex = 0
        '
        'CandidateFileDataGrid
        '
        Me.CandidateFileDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.CandidateFileDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.CandidateFileDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.CandidateFileDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.CandidateFileDataGrid.ADGroupsThatCanFind = ""
        Me.CandidateFileDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.CandidateFileDataGrid.ADGroupsThatCanMultiSort = ""
        Me.CandidateFileDataGrid.AllowAutoSize = True
        Me.CandidateFileDataGrid.AllowColumnReorder = True
        Me.CandidateFileDataGrid.AllowCopy = True
        Me.CandidateFileDataGrid.AllowCustomize = True
        Me.CandidateFileDataGrid.AllowDelete = False
        Me.CandidateFileDataGrid.AllowDragDrop = False
        Me.CandidateFileDataGrid.AllowEdit = False
        Me.CandidateFileDataGrid.AllowExport = True
        Me.CandidateFileDataGrid.AllowFilter = True
        Me.CandidateFileDataGrid.AllowFind = True
        Me.CandidateFileDataGrid.AllowGoTo = True
        Me.CandidateFileDataGrid.AllowMultiSelect = True
        Me.CandidateFileDataGrid.AllowMultiSort = False
        Me.CandidateFileDataGrid.AllowNew = False
        Me.CandidateFileDataGrid.AllowPrint = True
        Me.CandidateFileDataGrid.AllowRefresh = False
        Me.CandidateFileDataGrid.AppKey = "UFCW\Claims\"
        Me.CandidateFileDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.CandidateFileDataGrid.CaptionText = "Candidate File"
        Me.CandidateFileDataGrid.ColumnHeaderLabel = Nothing
        Me.CandidateFileDataGrid.ColumnRePositioning = False
        Me.CandidateFileDataGrid.ColumnResizing = False
        Me.CandidateFileDataGrid.ConfirmDelete = True
        Me.CandidateFileDataGrid.CopySelectedOnly = False
        Me.CandidateFileDataGrid.DataMember = ""
        Me.CandidateFileDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CandidateFileDataGrid.DragColumn = 0
        Me.CandidateFileDataGrid.ExportSelectedOnly = False
        Me.CandidateFileDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.CandidateFileDataGrid.HighlightedRow = Nothing
        Me.CandidateFileDataGrid.IsMouseDown = False
        Me.CandidateFileDataGrid.LastGoToLine = ""
        Me.CandidateFileDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.CandidateFileDataGrid.MultiSort = False
        Me.CandidateFileDataGrid.Name = "CandidateFileDataGrid"
        Me.CandidateFileDataGrid.OldSelectedRow = Nothing
        Me.CandidateFileDataGrid.ReadOnly = True
        Me.CandidateFileDataGrid.SetRowOnRightClick = True
        Me.CandidateFileDataGrid.ShiftPressed = False
        Me.CandidateFileDataGrid.SingleClickBooleanColumns = True
        Me.CandidateFileDataGrid.Size = New System.Drawing.Size(804, 74)
        Me.CandidateFileDataGrid.StyleName = ""
        Me.CandidateFileDataGrid.SubKey = ""
        Me.CandidateFileDataGrid.SuppressTriangle = False
        Me.CandidateFileDataGrid.TabIndex = 0
        '
        'IncentiveViewDataGrid
        '
        Me.IncentiveViewDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.IncentiveViewDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.IncentiveViewDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.IncentiveViewDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.IncentiveViewDataGrid.ADGroupsThatCanFind = ""
        Me.IncentiveViewDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.IncentiveViewDataGrid.ADGroupsThatCanMultiSort = ""
        Me.IncentiveViewDataGrid.AllowAutoSize = True
        Me.IncentiveViewDataGrid.AllowColumnReorder = True
        Me.IncentiveViewDataGrid.AllowCopy = True
        Me.IncentiveViewDataGrid.AllowCustomize = True
        Me.IncentiveViewDataGrid.AllowDelete = False
        Me.IncentiveViewDataGrid.AllowDragDrop = False
        Me.IncentiveViewDataGrid.AllowEdit = False
        Me.IncentiveViewDataGrid.AllowExport = True
        Me.IncentiveViewDataGrid.AllowFilter = True
        Me.IncentiveViewDataGrid.AllowFind = True
        Me.IncentiveViewDataGrid.AllowGoTo = True
        Me.IncentiveViewDataGrid.AllowMultiSelect = True
        Me.IncentiveViewDataGrid.AllowMultiSort = False
        Me.IncentiveViewDataGrid.AllowNew = False
        Me.IncentiveViewDataGrid.AllowPrint = True
        Me.IncentiveViewDataGrid.AllowRefresh = False
        Me.IncentiveViewDataGrid.AppKey = "UFCW\Claims\"
        Me.IncentiveViewDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.IncentiveViewDataGrid.CaptionText = "Incentive View"
        Me.IncentiveViewDataGrid.ColumnHeaderLabel = Nothing
        Me.IncentiveViewDataGrid.ColumnRePositioning = False
        Me.IncentiveViewDataGrid.ColumnResizing = False
        Me.IncentiveViewDataGrid.ConfirmDelete = True
        Me.IncentiveViewDataGrid.CopySelectedOnly = False
        Me.IncentiveViewDataGrid.DataMember = ""
        Me.IncentiveViewDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.IncentiveViewDataGrid.DragColumn = 0
        Me.IncentiveViewDataGrid.ExportSelectedOnly = False
        Me.IncentiveViewDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.IncentiveViewDataGrid.HighlightedRow = Nothing
        Me.IncentiveViewDataGrid.IsMouseDown = False
        Me.IncentiveViewDataGrid.LastGoToLine = ""
        Me.IncentiveViewDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.IncentiveViewDataGrid.MultiSort = False
        Me.IncentiveViewDataGrid.Name = "IncentiveViewDataGrid"
        Me.IncentiveViewDataGrid.OldSelectedRow = Nothing
        Me.IncentiveViewDataGrid.ReadOnly = True
        Me.IncentiveViewDataGrid.SetRowOnRightClick = True
        Me.IncentiveViewDataGrid.ShiftPressed = False
        Me.IncentiveViewDataGrid.SingleClickBooleanColumns = True
        Me.IncentiveViewDataGrid.Size = New System.Drawing.Size(804, 70)
        Me.IncentiveViewDataGrid.StyleName = ""
        Me.IncentiveViewDataGrid.SubKey = ""
        Me.IncentiveViewDataGrid.SuppressTriangle = False
        Me.IncentiveViewDataGrid.TabIndex = 2
        '
        'DNPDataGrid
        '
        Me.DNPDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.DNPDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.DNPDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DNPDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DNPDataGrid.ADGroupsThatCanFind = ""
        Me.DNPDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DNPDataGrid.ADGroupsThatCanMultiSort = ""
        Me.DNPDataGrid.AllowAutoSize = True
        Me.DNPDataGrid.AllowColumnReorder = True
        Me.DNPDataGrid.AllowCopy = True
        Me.DNPDataGrid.AllowCustomize = True
        Me.DNPDataGrid.AllowDelete = False
        Me.DNPDataGrid.AllowDragDrop = False
        Me.DNPDataGrid.AllowEdit = False
        Me.DNPDataGrid.AllowExport = True
        Me.DNPDataGrid.AllowFilter = True
        Me.DNPDataGrid.AllowFind = True
        Me.DNPDataGrid.AllowGoTo = True
        Me.DNPDataGrid.AllowMultiSelect = True
        Me.DNPDataGrid.AllowMultiSort = False
        Me.DNPDataGrid.AllowNew = False
        Me.DNPDataGrid.AllowPrint = True
        Me.DNPDataGrid.AllowRefresh = False
        Me.DNPDataGrid.AppKey = "UFCW\Claims\"
        Me.DNPDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.DNPDataGrid.CaptionText = "DNP"
        Me.DNPDataGrid.ColumnHeaderLabel = Nothing
        Me.DNPDataGrid.ColumnRePositioning = False
        Me.DNPDataGrid.ColumnResizing = False
        Me.DNPDataGrid.ConfirmDelete = True
        Me.DNPDataGrid.CopySelectedOnly = False
        Me.DNPDataGrid.DataMember = ""
        Me.DNPDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DNPDataGrid.DragColumn = 0
        Me.DNPDataGrid.ExportSelectedOnly = False
        Me.DNPDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DNPDataGrid.HighlightedRow = Nothing
        Me.DNPDataGrid.IsMouseDown = False
        Me.DNPDataGrid.LastGoToLine = ""
        Me.DNPDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.DNPDataGrid.MultiSort = False
        Me.DNPDataGrid.Name = "DNPDataGrid"
        Me.DNPDataGrid.OldSelectedRow = Nothing
        Me.DNPDataGrid.ReadOnly = True
        Me.DNPDataGrid.SetRowOnRightClick = True
        Me.DNPDataGrid.ShiftPressed = False
        Me.DNPDataGrid.SingleClickBooleanColumns = True
        Me.DNPDataGrid.Size = New System.Drawing.Size(804, 143)
        Me.DNPDataGrid.StyleName = ""
        Me.DNPDataGrid.SubKey = ""
        Me.DNPDataGrid.SuppressTriangle = False
        Me.DNPDataGrid.TabIndex = 0
        '
        'SplitContainerBottom
        '
        Me.SplitContainerBottom.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerBottom.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainerBottom.Name = "SplitContainerBottom"
        Me.SplitContainerBottom.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainerBottom.Panel1
        '
        Me.SplitContainerBottom.Panel1.Controls.Add(Me.DisincentiveLettersDataGrid)
        '
        'SplitContainerBottom.Panel2
        '
        Me.SplitContainerBottom.Panel2.Controls.Add(Me.DiseaseManagementAlertsDataGrid)
        Me.SplitContainerBottom.Size = New System.Drawing.Size(804, 291)
        Me.SplitContainerBottom.SplitterDistance = 145
        Me.SplitContainerBottom.TabIndex = 0
        '
        'DisincentiveLettersDataGrid
        '
        Me.DisincentiveLettersDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.DisincentiveLettersDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.DisincentiveLettersDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DisincentiveLettersDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DisincentiveLettersDataGrid.ADGroupsThatCanFind = ""
        Me.DisincentiveLettersDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DisincentiveLettersDataGrid.ADGroupsThatCanMultiSort = ""
        Me.DisincentiveLettersDataGrid.AllowAutoSize = True
        Me.DisincentiveLettersDataGrid.AllowColumnReorder = True
        Me.DisincentiveLettersDataGrid.AllowCopy = True
        Me.DisincentiveLettersDataGrid.AllowCustomize = True
        Me.DisincentiveLettersDataGrid.AllowDelete = False
        Me.DisincentiveLettersDataGrid.AllowDragDrop = False
        Me.DisincentiveLettersDataGrid.AllowEdit = False
        Me.DisincentiveLettersDataGrid.AllowExport = True
        Me.DisincentiveLettersDataGrid.AllowFilter = True
        Me.DisincentiveLettersDataGrid.AllowFind = True
        Me.DisincentiveLettersDataGrid.AllowGoTo = True
        Me.DisincentiveLettersDataGrid.AllowMultiSelect = True
        Me.DisincentiveLettersDataGrid.AllowMultiSort = False
        Me.DisincentiveLettersDataGrid.AllowNew = False
        Me.DisincentiveLettersDataGrid.AllowPrint = True
        Me.DisincentiveLettersDataGrid.AllowRefresh = False
        Me.DisincentiveLettersDataGrid.AppKey = "UFCW\Claims\"
        Me.DisincentiveLettersDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.DisincentiveLettersDataGrid.CaptionText = "Letter(s)"
        Me.DisincentiveLettersDataGrid.ColumnHeaderLabel = Nothing
        Me.DisincentiveLettersDataGrid.ColumnRePositioning = False
        Me.DisincentiveLettersDataGrid.ColumnResizing = False
        Me.DisincentiveLettersDataGrid.ConfirmDelete = True
        Me.DisincentiveLettersDataGrid.CopySelectedOnly = False
        Me.DisincentiveLettersDataGrid.DataMember = ""
        Me.DisincentiveLettersDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DisincentiveLettersDataGrid.DragColumn = 0
        Me.DisincentiveLettersDataGrid.ExportSelectedOnly = False
        Me.DisincentiveLettersDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DisincentiveLettersDataGrid.HighlightedRow = Nothing
        Me.DisincentiveLettersDataGrid.IsMouseDown = False
        Me.DisincentiveLettersDataGrid.LastGoToLine = ""
        Me.DisincentiveLettersDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.DisincentiveLettersDataGrid.MultiSort = False
        Me.DisincentiveLettersDataGrid.Name = "DisincentiveLettersDataGrid"
        Me.DisincentiveLettersDataGrid.OldSelectedRow = Nothing
        Me.DisincentiveLettersDataGrid.ReadOnly = True
        Me.DisincentiveLettersDataGrid.SetRowOnRightClick = True
        Me.DisincentiveLettersDataGrid.ShiftPressed = False
        Me.DisincentiveLettersDataGrid.SingleClickBooleanColumns = True
        Me.DisincentiveLettersDataGrid.Size = New System.Drawing.Size(804, 145)
        Me.DisincentiveLettersDataGrid.StyleName = ""
        Me.DisincentiveLettersDataGrid.SubKey = ""
        Me.DisincentiveLettersDataGrid.SuppressTriangle = False
        Me.DisincentiveLettersDataGrid.TabIndex = 0
        '
        'DiseaseManagementAlertsDataGrid
        '
        Me.DiseaseManagementAlertsDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.DiseaseManagementAlertsDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.DiseaseManagementAlertsDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DiseaseManagementAlertsDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DiseaseManagementAlertsDataGrid.ADGroupsThatCanFind = ""
        Me.DiseaseManagementAlertsDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DiseaseManagementAlertsDataGrid.ADGroupsThatCanMultiSort = ""
        Me.DiseaseManagementAlertsDataGrid.AllowAutoSize = True
        Me.DiseaseManagementAlertsDataGrid.AllowColumnReorder = True
        Me.DiseaseManagementAlertsDataGrid.AllowCopy = True
        Me.DiseaseManagementAlertsDataGrid.AllowCustomize = True
        Me.DiseaseManagementAlertsDataGrid.AllowDelete = False
        Me.DiseaseManagementAlertsDataGrid.AllowDragDrop = False
        Me.DiseaseManagementAlertsDataGrid.AllowEdit = False
        Me.DiseaseManagementAlertsDataGrid.AllowExport = True
        Me.DiseaseManagementAlertsDataGrid.AllowFilter = True
        Me.DiseaseManagementAlertsDataGrid.AllowFind = True
        Me.DiseaseManagementAlertsDataGrid.AllowGoTo = True
        Me.DiseaseManagementAlertsDataGrid.AllowMultiSelect = True
        Me.DiseaseManagementAlertsDataGrid.AllowMultiSort = False
        Me.DiseaseManagementAlertsDataGrid.AllowNew = False
        Me.DiseaseManagementAlertsDataGrid.AllowPrint = True
        Me.DiseaseManagementAlertsDataGrid.AllowRefresh = False
        Me.DiseaseManagementAlertsDataGrid.AppKey = "UFCW\Claims\"
        Me.DiseaseManagementAlertsDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.DiseaseManagementAlertsDataGrid.CaptionText = "Disease Management Alerts"
        Me.DiseaseManagementAlertsDataGrid.ColumnHeaderLabel = Nothing
        Me.DiseaseManagementAlertsDataGrid.ColumnRePositioning = False
        Me.DiseaseManagementAlertsDataGrid.ColumnResizing = False
        Me.DiseaseManagementAlertsDataGrid.ConfirmDelete = True
        Me.DiseaseManagementAlertsDataGrid.CopySelectedOnly = False
        Me.DiseaseManagementAlertsDataGrid.DataMember = ""
        Me.DiseaseManagementAlertsDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DiseaseManagementAlertsDataGrid.DragColumn = 0
        Me.DiseaseManagementAlertsDataGrid.ExportSelectedOnly = False
        Me.DiseaseManagementAlertsDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DiseaseManagementAlertsDataGrid.HighlightedRow = Nothing
        Me.DiseaseManagementAlertsDataGrid.IsMouseDown = False
        Me.DiseaseManagementAlertsDataGrid.LastGoToLine = ""
        Me.DiseaseManagementAlertsDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.DiseaseManagementAlertsDataGrid.MultiSort = False
        Me.DiseaseManagementAlertsDataGrid.Name = "DiseaseManagementAlertsDataGrid"
        Me.DiseaseManagementAlertsDataGrid.OldSelectedRow = Nothing
        Me.DiseaseManagementAlertsDataGrid.ReadOnly = True
        Me.DiseaseManagementAlertsDataGrid.SetRowOnRightClick = True
        Me.DiseaseManagementAlertsDataGrid.ShiftPressed = False
        Me.DiseaseManagementAlertsDataGrid.SingleClickBooleanColumns = True
        Me.DiseaseManagementAlertsDataGrid.Size = New System.Drawing.Size(804, 142)
        Me.DiseaseManagementAlertsDataGrid.StyleName = ""
        Me.DiseaseManagementAlertsDataGrid.SubKey = ""
        Me.DiseaseManagementAlertsDataGrid.SuppressTriangle = False
        Me.DiseaseManagementAlertsDataGrid.TabIndex = 0
        '
        'PatientDiseaseManagementForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.ExitButton
        Me.ClientSize = New System.Drawing.Size(828, 637)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ExitButton)
        Me.Name = "PatientDiseaseManagementForm"
        Me.Text = "Patient Disease Management History"
        Me.Panel1.ResumeLayout(False)
        Me.SplitContainer.Panel1.ResumeLayout(False)
        Me.SplitContainer.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer.ResumeLayout(False)
        Me.SplitContainerTop.Panel1.ResumeLayout(False)
        Me.SplitContainerTop.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerTop, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerTop.ResumeLayout(False)
        Me.SplitContainerVeryTop.Panel1.ResumeLayout(False)
        Me.SplitContainerVeryTop.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerVeryTop, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerVeryTop.ResumeLayout(False)
        CType(Me.CandidateFileDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.IncentiveViewDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DNPDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerBottom.Panel1.ResumeLayout(False)
        Me.SplitContainerBottom.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerBottom, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerBottom.ResumeLayout(False)
        CType(Me.DisincentiveLettersDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DiseaseManagementAlertsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Form Events"
    Private Sub DiseaseManagementForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetSettings()

        LoadDiseaseManagement()

    End Sub

    Private Sub ExitButton_Click(sender As System.Object, e As System.EventArgs) Handles ExitButton.Click
        Me.Close()
    End Sub

#End Region

#Region "custom Code"
    Private Sub LoadDiseaseManagement()
        Try

            If _DiseaseManagementDS Is Nothing OrElse _DiseaseManagementDS.Tables.Count = 0 Then
                _DiseaseManagementDS = CMSDALFDBMD.RetrieveDiseaseManagement(_FamilyID, _DiseaseManagementDS)
            End If

            If _RelationID IsNot Nothing Then
                _DiseaseManagementDS.Tables("DM_DISINCENTIVE_DNP").DefaultView.RowFilter = "RELATION_ID = " & _RelationID.ToString
                _DiseaseManagementDS.Tables("UFCWLETTER").DefaultView.RowFilter = "RELATION_ID = " & _RelationID.ToString
                _DiseaseManagementDS.Tables("REG_ALERTS").DefaultView.RowFilter = "RELATION_ID = " & _RelationID.ToString
                _DiseaseManagementDS.Tables("DM_CANDIDATES").DefaultView.RowFilter = "RELATION_ID = " & _RelationID.ToString
                _DiseaseManagementDS.Tables("ALL_DM_INCENTIVE").DefaultView.RowFilter = "RELATION_ID = " & _RelationID.ToString
            End If

            DNPDataGrid.DataSource = _DiseaseManagementDS.Tables("DM_DISINCENTIVE_DNP").DefaultView
            DisincentiveLettersDataGrid.DataSource = _DiseaseManagementDS.Tables("UFCWLETTER").DefaultView
            DiseaseManagementAlertsDataGrid.DataSource = _DiseaseManagementDS.Tables("REG_ALERTS").DefaultView
            CandidateFileDataGrid.DataSource = _DiseaseManagementDS.Tables("DM_CANDIDATES").DefaultView
            IncentiveViewDataGrid.DataSource = _DiseaseManagementDS.Tables("ALL_DM_INCENTIVE").DefaultView

            DNPDataGrid.SetTableStyle()
            DisincentiveLettersDataGrid.SetTableStyle()
            DiseaseManagementAlertsDataGrid.SetTableStyle()
            CandidateFileDataGrid.SetTableStyle()
            IncentiveViewDataGrid.SetTableStyle()

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub SaveSettings()

        Try
            UFCWGeneral.SaveFormPosition(Me, _APPKEY)

            SaveSetting(_APPKEY, Me.Name & "\Settings", "SplitContainer", CStr(SplitContainer.SplitterDistance))
            SaveSetting(_APPKEY, Me.Name & "\Settings", "SplitContainerTop", CStr(SplitContainerTop.SplitterDistance))
            SaveSetting(_APPKEY, Me.Name & "\Settings", "SplitContainerVeryTop", CStr(SplitContainerVeryTop.SplitterDistance))
            SaveSetting(_APPKEY, Me.Name & "\Settings", "SplitContainerBottom", CStr(SplitContainerBottom.SplitterDistance))

        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub SetSettings()

        Try

            If Not UFCWGeneral.SetFormPosition(Me, _APPKEY) Then Me.CenterToScreen()

            SplitContainer.SplitterDistance = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "SplitContainer", CStr(SplitContainer.SplitterDistance)))
            SplitContainerTop.SplitterDistance = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "SplitContainerTop", CStr(SplitContainerTop.SplitterDistance)))
            SplitContainerVeryTop.SplitterDistance = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "SplitContainerVeryTop", CStr(SplitContainerVeryTop.SplitterDistance)))
            SplitContainerBottom.SplitterDistance = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "SplitContainerBottom", CStr(SplitContainerBottom.SplitterDistance)))

        Catch ex As Exception
        End Try

    End Sub

    Private Sub PatientDiseaseManagementForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

        SaveSettings()
    End Sub
#End Region
End Class