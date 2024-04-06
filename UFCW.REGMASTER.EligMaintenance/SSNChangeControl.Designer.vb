<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SSNChangeControl
    Inherits System.Windows.Forms.UserControl

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim EligibiltyTabPage As System.Windows.Forms.TabPage
        Me.pnlBorder = New System.Windows.Forms.Panel()
        Me.NewSSNDataGrid = New DataGridCustom()
        Me.ControlPanel = New System.Windows.Forms.Panel()
        Me.btnsaveelig = New System.Windows.Forms.Button()
        Me.btncancelelig = New System.Windows.Forms.Button()
        Me.btnmodifyelig = New System.Windows.Forms.Button()
        Me.btnremeligrow = New System.Windows.Forms.Button()
        Me.btnremeligallrow = New System.Windows.Forms.Button()
        Me.btnaddeligallrow = New System.Windows.Forms.Button()
        Me.btnaddeligrow = New System.Windows.Forms.Button()
        Me.VSplit = New System.Windows.Forms.Splitter()
        Me.OldSSNDataGrid = New DataGridCustom()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.grpremarks = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtRemarks = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.SSNTabControl = New System.Windows.Forms.TabControl()
        Me.HoursTabpage = New System.Windows.Forms.TabPage()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.NewHoursDataGrid = New DataGridCustom()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.btnSaveHrs = New System.Windows.Forms.Button()
        Me.btncancelhrs = New System.Windows.Forms.Button()
        Me.btnremhrsrow = New System.Windows.Forms.Button()
        Me.btnremhrsallrow = New System.Windows.Forms.Button()
        Me.btnModifyHrs = New System.Windows.Forms.Button()
        Me.btnaddhrsallrow = New System.Windows.Forms.Button()
        Me.btnaddhrsrow = New System.Windows.Forms.Button()
        Me.Splitter2 = New System.Windows.Forms.Splitter()
        Me.OldHoursDataGrid = New DataGridCustom()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.txtNewSSN = New ExTextBox()
        Me.btnNewClear = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnNewSearch = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtNewFID = New ExTextBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.txtOldSSN = New ExTextBox()
        Me.btnoldsearch = New System.Windows.Forms.Button()
        Me.txtOldFID = New ExTextBox()
        Me.btnoldClear = New System.Windows.Forms.Button()
        Me.FamilyIdLabel = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.OLD_EMD_DS = New EligMthdtlDS()
        Me.NEW_EMD_DS = New EligMthdtlDS()
        Me.OLD_ELIG_HOURS = New EligAcctHoursDS()
        Me.NEW_ELIG_HOURS = New EligAcctHoursDS()
        EligibiltyTabPage = New System.Windows.Forms.TabPage()
        EligibiltyTabPage.SuspendLayout()
        Me.pnlBorder.SuspendLayout()
        CType(Me.NewSSNDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ControlPanel.SuspendLayout()
        CType(Me.OldSSNDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.grpremarks.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SSNTabControl.SuspendLayout()
        Me.HoursTabpage.SuspendLayout()
        Me.Panel4.SuspendLayout()
        CType(Me.NewHoursDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel5.SuspendLayout()
        CType(Me.OldHoursDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OLD_EMD_DS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NEW_EMD_DS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OLD_ELIG_HOURS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NEW_ELIG_HOURS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'EligibiltyTabPage
        '
        EligibiltyTabPage.BackColor = System.Drawing.SystemColors.Control
        EligibiltyTabPage.Controls.Add(Me.pnlBorder)
        EligibiltyTabPage.Location = New System.Drawing.Point(4, 22)
        EligibiltyTabPage.Name = "EligibiltyTabPage"
        EligibiltyTabPage.Padding = New System.Windows.Forms.Padding(3)
        EligibiltyTabPage.Size = New System.Drawing.Size(943, 509)
        EligibiltyTabPage.TabIndex = 0
        EligibiltyTabPage.Text = "ELIGIBILITY"
        '
        'pnlBorder
        '
        Me.pnlBorder.Controls.Add(Me.NewSSNDataGrid)
        Me.pnlBorder.Controls.Add(Me.ControlPanel)
        Me.pnlBorder.Controls.Add(Me.VSplit)
        Me.pnlBorder.Controls.Add(Me.OldSSNDataGrid)
        Me.pnlBorder.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlBorder.Location = New System.Drawing.Point(3, 3)
        Me.pnlBorder.Name = "pnlBorder"
        Me.pnlBorder.Size = New System.Drawing.Size(937, 503)
        Me.pnlBorder.TabIndex = 3
        '
        'NewSSNDataGrid
        '
        Me.NewSSNDataGrid.ADGroupsThatCanCopy = ""
        Me.NewSSNDataGrid.ADGroupsThatCanCustomize = ""
        Me.NewSSNDataGrid.ADGroupsThatCanExport = ""
        Me.NewSSNDataGrid.ADGroupsThatCanFilter = ""
        Me.NewSSNDataGrid.ADGroupsThatCanFind = ""
        Me.NewSSNDataGrid.ADGroupsThatCanMultiSort = ""
        Me.NewSSNDataGrid.ADGroupsThatCanPrint = ""
        Me.NewSSNDataGrid.AllowAutoSize = True
        Me.NewSSNDataGrid.AllowColumnReorder = False
        Me.NewSSNDataGrid.AllowCopy = False
        Me.NewSSNDataGrid.AllowCustomize = True
        Me.NewSSNDataGrid.AllowDelete = False
        Me.NewSSNDataGrid.AllowDragDrop = False
        Me.NewSSNDataGrid.AllowEdit = True
        Me.NewSSNDataGrid.AllowExport = False
        Me.NewSSNDataGrid.AllowFilter = True
        Me.NewSSNDataGrid.AllowFind = False
        Me.NewSSNDataGrid.AllowGoTo = False
        Me.NewSSNDataGrid.AllowMultiSelect = True
        Me.NewSSNDataGrid.AllowMultiSort = False
        Me.NewSSNDataGrid.AllowNew = False
        Me.NewSSNDataGrid.AllowPrint = True
        Me.NewSSNDataGrid.AllowRefresh = True
        Me.NewSSNDataGrid.AppKey = "UFCW\RegMaster\"
        Me.NewSSNDataGrid.AutoSaveCols = True
        Me.NewSSNDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.NewSSNDataGrid.CaptionText = "New SSN"
        Me.NewSSNDataGrid.ColumnHeaderLabel = Nothing
        Me.NewSSNDataGrid.ColumnRePositioning = False
        Me.NewSSNDataGrid.ColumnResizing = False
        Me.NewSSNDataGrid.ConfirmDelete = True
        Me.NewSSNDataGrid.CopySelectedOnly = True
        Me.NewSSNDataGrid.CurrentBSPosition = -1
        Me.NewSSNDataGrid.DataMember = ""
        Me.NewSSNDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.NewSSNDataGrid.DragColumn = 0
        Me.NewSSNDataGrid.ExportSelectedOnly = True
        Me.NewSSNDataGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.NewSSNDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.NewSSNDataGrid.HighlightedRow = Nothing
        Me.NewSSNDataGrid.HighLightModifiedRows = False
        Me.NewSSNDataGrid.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.NewSSNDataGrid.IsMouseDown = False
        Me.NewSSNDataGrid.LastGoToLine = ""
        Me.NewSSNDataGrid.Location = New System.Drawing.Point(497, 0)
        Me.NewSSNDataGrid.MultiSort = False
        Me.NewSSNDataGrid.Name = "NewSSNDataGrid"
        Me.NewSSNDataGrid.OldSelectedRow = 0
        Me.NewSSNDataGrid.PreviousBSPosition = -1
        Me.NewSSNDataGrid.ReadOnly = True
        Me.NewSSNDataGrid.RetainRowSelectionAfterSort = True
        Me.NewSSNDataGrid.SetRowOnRightClick = True
        Me.NewSSNDataGrid.ShiftPressed = False
        Me.NewSSNDataGrid.SingleClickBooleanColumns = True
        Me.NewSSNDataGrid.Size = New System.Drawing.Size(440, 503)
        Me.NewSSNDataGrid.Sort = Nothing
        Me.NewSSNDataGrid.StyleName = ""
        Me.NewSSNDataGrid.SubKey = ""
        Me.NewSSNDataGrid.SuppressMouseDown = False
        Me.NewSSNDataGrid.SuppressTriangle = False
        Me.NewSSNDataGrid.TabIndex = 6
        '
        'ControlPanel
        '
        Me.ControlPanel.Controls.Add(Me.btnsaveelig)
        Me.ControlPanel.Controls.Add(Me.btncancelelig)
        Me.ControlPanel.Controls.Add(Me.btnmodifyelig)
        Me.ControlPanel.Controls.Add(Me.btnremeligrow)
        Me.ControlPanel.Controls.Add(Me.btnremeligallrow)
        Me.ControlPanel.Controls.Add(Me.btnaddeligallrow)
        Me.ControlPanel.Controls.Add(Me.btnaddeligrow)
        Me.ControlPanel.Dock = System.Windows.Forms.DockStyle.Left
        Me.ControlPanel.Location = New System.Drawing.Point(434, 0)
        Me.ControlPanel.Name = "ControlPanel"
        Me.ControlPanel.Size = New System.Drawing.Size(63, 503)
        Me.ControlPanel.TabIndex = 5
        '
        'btnsaveelig
        '
        Me.btnsaveelig.Enabled = False
        Me.btnsaveelig.Location = New System.Drawing.Point(6, 324)
        Me.btnsaveelig.Name = "btnsaveelig"
        Me.btnsaveelig.Size = New System.Drawing.Size(47, 23)
        Me.btnsaveelig.TabIndex = 23
        Me.btnsaveelig.Text = "save"
        Me.btnsaveelig.Visible = False
        '
        'btncancelelig
        '
        Me.btncancelelig.Enabled = False
        Me.btncancelelig.Location = New System.Drawing.Point(6, 362)
        Me.btncancelelig.Name = "btncancelelig"
        Me.btncancelelig.Size = New System.Drawing.Size(47, 23)
        Me.btncancelelig.TabIndex = 24
        Me.btncancelelig.Text = "cancel"
        Me.btncancelelig.Visible = False
        '
        'btnmodifyelig
        '
        Me.btnmodifyelig.Enabled = False
        Me.btnmodifyelig.Location = New System.Drawing.Point(8, 46)
        Me.btnmodifyelig.Name = "btnmodifyelig"
        Me.btnmodifyelig.Size = New System.Drawing.Size(47, 23)
        Me.btnmodifyelig.TabIndex = 22
        Me.btnmodifyelig.Text = "modify"
        Me.btnmodifyelig.Visible = False
        '
        'btnremeligrow
        '
        Me.btnremeligrow.Location = New System.Drawing.Point(8, 210)
        Me.btnremeligrow.Name = "btnremeligrow"
        Me.btnremeligrow.Size = New System.Drawing.Size(32, 23)
        Me.btnremeligrow.TabIndex = 3
        Me.btnremeligrow.Text = "<"
        Me.ToolTip1.SetToolTip(Me.btnremeligrow, "Remove Selected row from new SSN")
        '
        'btnremeligallrow
        '
        Me.btnremeligallrow.Location = New System.Drawing.Point(8, 239)
        Me.btnremeligallrow.Name = "btnremeligallrow"
        Me.btnremeligallrow.Size = New System.Drawing.Size(32, 23)
        Me.btnremeligallrow.TabIndex = 2
        Me.btnremeligallrow.Text = "<<"
        Me.ToolTip1.SetToolTip(Me.btnremeligallrow, "Remove All rows from new SSN")
        '
        'btnaddeligallrow
        '
        Me.btnaddeligallrow.Location = New System.Drawing.Point(8, 130)
        Me.btnaddeligallrow.Name = "btnaddeligallrow"
        Me.btnaddeligallrow.Size = New System.Drawing.Size(32, 23)
        Me.btnaddeligallrow.TabIndex = 1
        Me.btnaddeligallrow.Text = ">>"
        Me.ToolTip1.SetToolTip(Me.btnaddeligallrow, "Add All rows to new SSN")
        '
        'btnaddeligrow
        '
        Me.btnaddeligrow.Location = New System.Drawing.Point(8, 98)
        Me.btnaddeligrow.Name = "btnaddeligrow"
        Me.btnaddeligrow.Size = New System.Drawing.Size(32, 23)
        Me.btnaddeligrow.TabIndex = 0
        Me.btnaddeligrow.Text = ">"
        Me.ToolTip1.SetToolTip(Me.btnaddeligrow, "Add Selected row to new SSN")
        '
        'VSplit
        '
        Me.VSplit.BackColor = System.Drawing.SystemColors.ControlDark
        Me.VSplit.Location = New System.Drawing.Point(424, 0)
        Me.VSplit.Name = "VSplit"
        Me.VSplit.Size = New System.Drawing.Size(10, 503)
        Me.VSplit.TabIndex = 2
        Me.VSplit.TabStop = False
        '
        'OldSSNDataGrid
        '
        Me.OldSSNDataGrid.ADGroupsThatCanCopy = ""
        Me.OldSSNDataGrid.ADGroupsThatCanCustomize = ""
        Me.OldSSNDataGrid.ADGroupsThatCanExport = ""
        Me.OldSSNDataGrid.ADGroupsThatCanFilter = ""
        Me.OldSSNDataGrid.ADGroupsThatCanFind = ""
        Me.OldSSNDataGrid.ADGroupsThatCanMultiSort = ""
        Me.OldSSNDataGrid.ADGroupsThatCanPrint = ""
        Me.OldSSNDataGrid.AllowAutoSize = True
        Me.OldSSNDataGrid.AllowColumnReorder = False
        Me.OldSSNDataGrid.AllowCopy = False
        Me.OldSSNDataGrid.AllowCustomize = True
        Me.OldSSNDataGrid.AllowDelete = False
        Me.OldSSNDataGrid.AllowDragDrop = False
        Me.OldSSNDataGrid.AllowEdit = False
        Me.OldSSNDataGrid.AllowExport = False
        Me.OldSSNDataGrid.AllowFilter = True
        Me.OldSSNDataGrid.AllowFind = False
        Me.OldSSNDataGrid.AllowGoTo = False
        Me.OldSSNDataGrid.AllowMultiSelect = True
        Me.OldSSNDataGrid.AllowMultiSort = False
        Me.OldSSNDataGrid.AllowNew = False
        Me.OldSSNDataGrid.AllowPrint = True
        Me.OldSSNDataGrid.AllowRefresh = True
        Me.OldSSNDataGrid.AppKey = "UFCW\RegMaster\"
        Me.OldSSNDataGrid.AutoSaveCols = True
        Me.OldSSNDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.OldSSNDataGrid.CaptionText = "Old SSN"
        Me.OldSSNDataGrid.ColumnHeaderLabel = Nothing
        Me.OldSSNDataGrid.ColumnRePositioning = False
        Me.OldSSNDataGrid.ColumnResizing = False
        Me.OldSSNDataGrid.ConfirmDelete = True
        Me.OldSSNDataGrid.CopySelectedOnly = True
        Me.OldSSNDataGrid.CurrentBSPosition = -1
        Me.OldSSNDataGrid.DataMember = ""
        Me.OldSSNDataGrid.Dock = System.Windows.Forms.DockStyle.Left
        Me.OldSSNDataGrid.DragColumn = 0
        Me.OldSSNDataGrid.ExportSelectedOnly = True
        Me.OldSSNDataGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.OldSSNDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.OldSSNDataGrid.HighlightedRow = Nothing
        Me.OldSSNDataGrid.HighLightModifiedRows = False
        Me.OldSSNDataGrid.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.OldSSNDataGrid.IsMouseDown = False
        Me.OldSSNDataGrid.LastGoToLine = ""
        Me.OldSSNDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.OldSSNDataGrid.MultiSort = False
        Me.OldSSNDataGrid.Name = "OldSSNDataGrid"
        Me.OldSSNDataGrid.OldSelectedRow = 0
        Me.OldSSNDataGrid.PreviousBSPosition = -1
        Me.OldSSNDataGrid.ReadOnly = True
        Me.OldSSNDataGrid.RetainRowSelectionAfterSort = True
        Me.OldSSNDataGrid.SetRowOnRightClick = True
        Me.OldSSNDataGrid.ShiftPressed = False
        Me.OldSSNDataGrid.SingleClickBooleanColumns = True
        Me.OldSSNDataGrid.Size = New System.Drawing.Size(424, 503)
        Me.OldSSNDataGrid.Sort = Nothing
        Me.OldSSNDataGrid.StyleName = ""
        Me.OldSSNDataGrid.SubKey = ""
        Me.OldSSNDataGrid.SuppressMouseDown = False
        Me.OldSSNDataGrid.SuppressTriangle = False
        Me.OldSSNDataGrid.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.grpremarks)
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Controls.Add(Me.GroupBox3)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(961, 711)
        Me.Panel1.TabIndex = 0
        '
        'grpremarks
        '
        Me.grpremarks.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpremarks.Controls.Add(Me.Label4)
        Me.grpremarks.Controls.Add(Me.txtRemarks)
        Me.grpremarks.Location = New System.Drawing.Point(3, 89)
        Me.grpremarks.Name = "grpremarks"
        Me.grpremarks.Size = New System.Drawing.Size(952, 72)
        Me.grpremarks.TabIndex = 20
        Me.grpremarks.TabStop = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(75, 16)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(49, 13)
        Me.Label4.TabIndex = 95
        Me.Label4.Text = "Remarks"
        '
        'txtRemarks
        '
        Me.txtRemarks.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtRemarks.Enabled = False
        Me.txtRemarks.Location = New System.Drawing.Point(141, 13)
        Me.txtRemarks.Multiline = True
        Me.txtRemarks.Name = "txtRemarks"
        Me.txtRemarks.Size = New System.Drawing.Size(640, 52)
        Me.txtRemarks.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.SSNTabControl)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 163)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(950, 545)
        Me.GroupBox1.TabIndex = 14
        Me.GroupBox1.TabStop = False
        '
        'SSNTabControl
        '
        Me.SSNTabControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SSNTabControl.Controls.Add(Me.HoursTabpage)
        Me.SSNTabControl.Controls.Add(EligibiltyTabPage)
        Me.SSNTabControl.Location = New System.Drawing.Point(3, 10)
        Me.SSNTabControl.Name = "SSNTabControl"
        Me.SSNTabControl.SelectedIndex = 0
        Me.SSNTabControl.Size = New System.Drawing.Size(951, 535)
        Me.SSNTabControl.TabIndex = 0
        '
        'HoursTabpage
        '
        Me.HoursTabpage.BackColor = System.Drawing.SystemColors.Control
        Me.HoursTabpage.Controls.Add(Me.Panel4)
        Me.HoursTabpage.Location = New System.Drawing.Point(4, 22)
        Me.HoursTabpage.Name = "HoursTabpage"
        Me.HoursTabpage.Padding = New System.Windows.Forms.Padding(3)
        Me.HoursTabpage.Size = New System.Drawing.Size(943, 509)
        Me.HoursTabpage.TabIndex = 1
        Me.HoursTabpage.Text = "HOURS"
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.NewHoursDataGrid)
        Me.Panel4.Controls.Add(Me.Panel5)
        Me.Panel4.Controls.Add(Me.Splitter2)
        Me.Panel4.Controls.Add(Me.OldHoursDataGrid)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel4.Location = New System.Drawing.Point(3, 3)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(937, 503)
        Me.Panel4.TabIndex = 4
        '
        'NewHoursDataGrid
        '
        Me.NewHoursDataGrid.ADGroupsThatCanCopy = ""
        Me.NewHoursDataGrid.ADGroupsThatCanCustomize = ""
        Me.NewHoursDataGrid.ADGroupsThatCanExport = ""
        Me.NewHoursDataGrid.ADGroupsThatCanFilter = ""
        Me.NewHoursDataGrid.ADGroupsThatCanFind = ""
        Me.NewHoursDataGrid.ADGroupsThatCanMultiSort = ""
        Me.NewHoursDataGrid.ADGroupsThatCanPrint = ""
        Me.NewHoursDataGrid.AllowAutoSize = True
        Me.NewHoursDataGrid.AllowColumnReorder = False
        Me.NewHoursDataGrid.AllowCopy = False
        Me.NewHoursDataGrid.AllowCustomize = True
        Me.NewHoursDataGrid.AllowDelete = False
        Me.NewHoursDataGrid.AllowDragDrop = False
        Me.NewHoursDataGrid.AllowEdit = True
        Me.NewHoursDataGrid.AllowExport = False
        Me.NewHoursDataGrid.AllowFilter = True
        Me.NewHoursDataGrid.AllowFind = False
        Me.NewHoursDataGrid.AllowGoTo = False
        Me.NewHoursDataGrid.AllowMultiSelect = True
        Me.NewHoursDataGrid.AllowMultiSort = False
        Me.NewHoursDataGrid.AllowNew = False
        Me.NewHoursDataGrid.AllowPrint = True
        Me.NewHoursDataGrid.AllowRefresh = True
        Me.NewHoursDataGrid.AppKey = "UFCW\RegMaster\"
        Me.NewHoursDataGrid.AutoSaveCols = True
        Me.NewHoursDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.NewHoursDataGrid.CaptionText = "New SSN Hours"
        Me.NewHoursDataGrid.ColumnHeaderLabel = Nothing
        Me.NewHoursDataGrid.ColumnRePositioning = False
        Me.NewHoursDataGrid.ColumnResizing = False
        Me.NewHoursDataGrid.ConfirmDelete = True
        Me.NewHoursDataGrid.CopySelectedOnly = True
        Me.NewHoursDataGrid.CurrentBSPosition = -1
        Me.NewHoursDataGrid.DataMember = ""
        Me.NewHoursDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.NewHoursDataGrid.DragColumn = 0
        Me.NewHoursDataGrid.ExportSelectedOnly = True
        Me.NewHoursDataGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.NewHoursDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.NewHoursDataGrid.HighlightedRow = Nothing
        Me.NewHoursDataGrid.HighLightModifiedRows = False
        Me.NewHoursDataGrid.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.NewHoursDataGrid.IsMouseDown = False
        Me.NewHoursDataGrid.LastGoToLine = ""
        Me.NewHoursDataGrid.Location = New System.Drawing.Point(490, 0)
        Me.NewHoursDataGrid.MultiSort = False
        Me.NewHoursDataGrid.Name = "NewHoursDataGrid"
        Me.NewHoursDataGrid.OldSelectedRow = 0
        Me.NewHoursDataGrid.PreviousBSPosition = -1
        Me.NewHoursDataGrid.ReadOnly = True
        Me.NewHoursDataGrid.RetainRowSelectionAfterSort = True
        Me.NewHoursDataGrid.SetRowOnRightClick = True
        Me.NewHoursDataGrid.ShiftPressed = False
        Me.NewHoursDataGrid.SingleClickBooleanColumns = True
        Me.NewHoursDataGrid.Size = New System.Drawing.Size(447, 503)
        Me.NewHoursDataGrid.Sort = Nothing
        Me.NewHoursDataGrid.StyleName = ""
        Me.NewHoursDataGrid.SubKey = ""
        Me.NewHoursDataGrid.SuppressMouseDown = False
        Me.NewHoursDataGrid.SuppressTriangle = False
        Me.NewHoursDataGrid.TabIndex = 6
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.btnSaveHrs)
        Me.Panel5.Controls.Add(Me.btncancelhrs)
        Me.Panel5.Controls.Add(Me.btnremhrsrow)
        Me.Panel5.Controls.Add(Me.btnremhrsallrow)
        Me.Panel5.Controls.Add(Me.btnModifyHrs)
        Me.Panel5.Controls.Add(Me.btnaddhrsallrow)
        Me.Panel5.Controls.Add(Me.btnaddhrsrow)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel5.Location = New System.Drawing.Point(434, 0)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(56, 503)
        Me.Panel5.TabIndex = 5
        '
        'btnSaveHrs
        '
        Me.btnSaveHrs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveHrs.Location = New System.Drawing.Point(3, 333)
        Me.btnSaveHrs.Name = "btnSaveHrs"
        Me.btnSaveHrs.Size = New System.Drawing.Size(47, 23)
        Me.btnSaveHrs.TabIndex = 20
        Me.btnSaveHrs.Text = "Save"
        '
        'btncancelhrs
        '
        Me.btncancelhrs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btncancelhrs.Location = New System.Drawing.Point(3, 371)
        Me.btncancelhrs.Name = "btncancelhrs"
        Me.btncancelhrs.Size = New System.Drawing.Size(48, 23)
        Me.btncancelhrs.TabIndex = 21
        Me.btncancelhrs.Text = "Cancel"
        '
        'btnremhrsrow
        '
        Me.btnremhrsrow.Location = New System.Drawing.Point(8, 226)
        Me.btnremhrsrow.Name = "btnremhrsrow"
        Me.btnremhrsrow.Size = New System.Drawing.Size(32, 23)
        Me.btnremhrsrow.TabIndex = 3
        Me.btnremhrsrow.Text = "<"
        Me.ToolTip1.SetToolTip(Me.btnremhrsrow, "Remove Selected range of rows from new SSN")
        '
        'btnremhrsallrow
        '
        Me.btnremhrsallrow.Location = New System.Drawing.Point(8, 255)
        Me.btnremhrsallrow.Name = "btnremhrsallrow"
        Me.btnremhrsallrow.Size = New System.Drawing.Size(32, 23)
        Me.btnremhrsallrow.TabIndex = 2
        Me.btnremhrsallrow.Text = "<<"
        Me.ToolTip1.SetToolTip(Me.btnremhrsallrow, "Remove All rows from new SSN")
        '
        'btnModifyHrs
        '
        Me.btnModifyHrs.Enabled = False
        Me.btnModifyHrs.Location = New System.Drawing.Point(3, 56)
        Me.btnModifyHrs.Name = "btnModifyHrs"
        Me.btnModifyHrs.Size = New System.Drawing.Size(47, 23)
        Me.btnModifyHrs.TabIndex = 19
        Me.btnModifyHrs.Text = "Modify"
        '
        'btnaddhrsallrow
        '
        Me.btnaddhrsallrow.Location = New System.Drawing.Point(8, 146)
        Me.btnaddhrsallrow.Name = "btnaddhrsallrow"
        Me.btnaddhrsallrow.Size = New System.Drawing.Size(32, 23)
        Me.btnaddhrsallrow.TabIndex = 1
        Me.btnaddhrsallrow.Text = ">>"
        Me.ToolTip1.SetToolTip(Me.btnaddhrsallrow, "Add All rows to new SSN")
        '
        'btnaddhrsrow
        '
        Me.btnaddhrsrow.Location = New System.Drawing.Point(8, 114)
        Me.btnaddhrsrow.Name = "btnaddhrsrow"
        Me.btnaddhrsrow.Size = New System.Drawing.Size(32, 23)
        Me.btnaddhrsrow.TabIndex = 0
        Me.btnaddhrsrow.Text = ">"
        Me.ToolTip1.SetToolTip(Me.btnaddhrsrow, "Add Selected range of rows to new SSN")
        '
        'Splitter2
        '
        Me.Splitter2.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Splitter2.Location = New System.Drawing.Point(424, 0)
        Me.Splitter2.Name = "Splitter2"
        Me.Splitter2.Size = New System.Drawing.Size(10, 503)
        Me.Splitter2.TabIndex = 2
        Me.Splitter2.TabStop = False
        '
        'OldHoursDataGrid
        '
        Me.OldHoursDataGrid.ADGroupsThatCanCopy = ""
        Me.OldHoursDataGrid.ADGroupsThatCanCustomize = ""
        Me.OldHoursDataGrid.ADGroupsThatCanExport = ""
        Me.OldHoursDataGrid.ADGroupsThatCanFilter = ""
        Me.OldHoursDataGrid.ADGroupsThatCanFind = ""
        Me.OldHoursDataGrid.ADGroupsThatCanMultiSort = ""
        Me.OldHoursDataGrid.ADGroupsThatCanPrint = ""
        Me.OldHoursDataGrid.AllowAutoSize = True
        Me.OldHoursDataGrid.AllowColumnReorder = False
        Me.OldHoursDataGrid.AllowCopy = False
        Me.OldHoursDataGrid.AllowCustomize = True
        Me.OldHoursDataGrid.AllowDelete = False
        Me.OldHoursDataGrid.AllowDragDrop = False
        Me.OldHoursDataGrid.AllowEdit = False
        Me.OldHoursDataGrid.AllowExport = False
        Me.OldHoursDataGrid.AllowFilter = True
        Me.OldHoursDataGrid.AllowFind = False
        Me.OldHoursDataGrid.AllowGoTo = False
        Me.OldHoursDataGrid.AllowMultiSelect = True
        Me.OldHoursDataGrid.AllowMultiSort = False
        Me.OldHoursDataGrid.AllowNew = False
        Me.OldHoursDataGrid.AllowPrint = True
        Me.OldHoursDataGrid.AllowRefresh = True
        Me.OldHoursDataGrid.AppKey = "UFCW\RegMaster\"
        Me.OldHoursDataGrid.AutoSaveCols = True
        Me.OldHoursDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.OldHoursDataGrid.CaptionText = "Old SSN Hours"
        Me.OldHoursDataGrid.ColumnHeaderLabel = Nothing
        Me.OldHoursDataGrid.ColumnRePositioning = False
        Me.OldHoursDataGrid.ColumnResizing = False
        Me.OldHoursDataGrid.ConfirmDelete = True
        Me.OldHoursDataGrid.CopySelectedOnly = True
        Me.OldHoursDataGrid.CurrentBSPosition = -1
        Me.OldHoursDataGrid.DataMember = ""
        Me.OldHoursDataGrid.Dock = System.Windows.Forms.DockStyle.Left
        Me.OldHoursDataGrid.DragColumn = 0
        Me.OldHoursDataGrid.ExportSelectedOnly = True
        Me.OldHoursDataGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.OldHoursDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.OldHoursDataGrid.HighlightedRow = Nothing
        Me.OldHoursDataGrid.HighLightModifiedRows = False
        Me.OldHoursDataGrid.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.OldHoursDataGrid.IsMouseDown = False
        Me.OldHoursDataGrid.LastGoToLine = ""
        Me.OldHoursDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.OldHoursDataGrid.MultiSort = False
        Me.OldHoursDataGrid.Name = "OldHoursDataGrid"
        Me.OldHoursDataGrid.OldSelectedRow = 0
        Me.OldHoursDataGrid.PreviousBSPosition = -1
        Me.OldHoursDataGrid.ReadOnly = True
        Me.OldHoursDataGrid.RetainRowSelectionAfterSort = True
        Me.OldHoursDataGrid.SetRowOnRightClick = True
        Me.OldHoursDataGrid.ShiftPressed = False
        Me.OldHoursDataGrid.SingleClickBooleanColumns = True
        Me.OldHoursDataGrid.Size = New System.Drawing.Size(424, 503)
        Me.OldHoursDataGrid.Sort = Nothing
        Me.OldHoursDataGrid.StyleName = ""
        Me.OldHoursDataGrid.SubKey = ""
        Me.OldHoursDataGrid.SuppressMouseDown = False
        Me.OldHoursDataGrid.SuppressTriangle = False
        Me.OldHoursDataGrid.TabIndex = 0
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.GroupBox5)
        Me.GroupBox3.Controls.Add(Me.GroupBox4)
        Me.GroupBox3.Location = New System.Drawing.Point(3, 1)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(952, 82)
        Me.GroupBox3.TabIndex = 18
        Me.GroupBox3.TabStop = False
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.txtNewSSN)
        Me.GroupBox5.Controls.Add(Me.btnNewClear)
        Me.GroupBox5.Controls.Add(Me.Label5)
        Me.GroupBox5.Controls.Add(Me.btnNewSearch)
        Me.GroupBox5.Controls.Add(Me.Label3)
        Me.GroupBox5.Controls.Add(Me.txtNewFID)
        Me.GroupBox5.Location = New System.Drawing.Point(477, 7)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(468, 65)
        Me.GroupBox5.TabIndex = 19
        Me.GroupBox5.TabStop = False
        '
        'txtNewSSN
        '
        Me.txtNewSSN.Location = New System.Drawing.Point(62, 15)
        Me.txtNewSSN.MaxLength = 9
        Me.txtNewSSN.Name = "txtNewSSN"
        Me.txtNewSSN.Size = New System.Drawing.Size(111, 20)
        Me.txtNewSSN.TabIndex = 11
        '
        'btnNewClear
        '
        Me.btnNewClear.CausesValidation = False
        Me.btnNewClear.Location = New System.Drawing.Point(282, 25)
        Me.btnNewClear.Name = "btnNewClear"
        Me.btnNewClear.Size = New System.Drawing.Size(72, 23)
        Me.btnNewClear.TabIndex = 15
        Me.btnNewClear.Text = "Clear"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 38)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(50, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Family ID"
        '
        'btnNewSearch
        '
        Me.btnNewSearch.Location = New System.Drawing.Point(204, 25)
        Me.btnNewSearch.Name = "btnNewSearch"
        Me.btnNewSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnNewSearch.Size = New System.Drawing.Size(72, 23)
        Me.btnNewSearch.TabIndex = 13
        Me.btnNewSearch.Text = "Search"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(27, 18)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(29, 13)
        Me.Label3.TabIndex = 12
        Me.Label3.Text = "SSN"
        '
        'txtNewFID
        '
        Me.txtNewFID.Enabled = False
        Me.txtNewFID.Location = New System.Drawing.Point(62, 38)
        Me.txtNewFID.MaxLength = 9
        Me.txtNewFID.Name = "txtNewFID"
        Me.txtNewFID.ReadOnly = True
        Me.txtNewFID.Size = New System.Drawing.Size(100, 20)
        Me.txtNewFID.TabIndex = 14
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.txtOldSSN)
        Me.GroupBox4.Controls.Add(Me.btnoldsearch)
        Me.GroupBox4.Controls.Add(Me.txtOldFID)
        Me.GroupBox4.Controls.Add(Me.btnoldClear)
        Me.GroupBox4.Controls.Add(Me.FamilyIdLabel)
        Me.GroupBox4.Controls.Add(Me.Label2)
        Me.GroupBox4.Location = New System.Drawing.Point(2, 7)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(474, 65)
        Me.GroupBox4.TabIndex = 18
        Me.GroupBox4.TabStop = False
        '
        'txtOldSSN
        '
        Me.txtOldSSN.Location = New System.Drawing.Point(76, 11)
        Me.txtOldSSN.MaxLength = 9
        Me.txtOldSSN.Name = "txtOldSSN"
        Me.txtOldSSN.Size = New System.Drawing.Size(108, 20)
        Me.txtOldSSN.TabIndex = 6
        '
        'btnoldsearch
        '
        Me.btnoldsearch.Location = New System.Drawing.Point(265, 25)
        Me.btnoldsearch.Name = "btnoldsearch"
        Me.btnoldsearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnoldsearch.Size = New System.Drawing.Size(72, 23)
        Me.btnoldsearch.TabIndex = 3
        Me.btnoldsearch.Text = "Search"
        '
        'txtOldFID
        '
        Me.txtOldFID.Enabled = False
        Me.txtOldFID.Location = New System.Drawing.Point(76, 34)
        Me.txtOldFID.MaxLength = 9
        Me.txtOldFID.Name = "txtOldFID"
        Me.txtOldFID.ReadOnly = True
        Me.txtOldFID.Size = New System.Drawing.Size(85, 20)
        Me.txtOldFID.TabIndex = 0
        '
        'btnoldClear
        '
        Me.btnoldClear.CausesValidation = False
        Me.btnoldClear.Location = New System.Drawing.Point(343, 25)
        Me.btnoldClear.Name = "btnoldClear"
        Me.btnoldClear.Size = New System.Drawing.Size(72, 23)
        Me.btnoldClear.TabIndex = 4
        Me.btnoldClear.Text = "Clear"
        '
        'FamilyIdLabel
        '
        Me.FamilyIdLabel.AutoSize = True
        Me.FamilyIdLabel.Location = New System.Drawing.Point(22, 38)
        Me.FamilyIdLabel.Name = "FamilyIdLabel"
        Me.FamilyIdLabel.Size = New System.Drawing.Size(50, 13)
        Me.FamilyIdLabel.TabIndex = 3
        Me.FamilyIdLabel.Text = "Family ID"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(41, 11)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(29, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "SSN"
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'OLD_EMD_DS
        '
        Me.OLD_EMD_DS.DataSetName = "EligMthdtlDS"
        Me.OLD_EMD_DS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'NEW_EMD_DS
        '
        Me.NEW_EMD_DS.DataSetName = "EligMthdtlDS"
        Me.NEW_EMD_DS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'OLD_ELIG_HOURS
        '
        Me.OLD_ELIG_HOURS.DataSetName = "EligAcctHoursDS"
        Me.OLD_ELIG_HOURS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'NEW_ELIG_HOURS
        '
        Me.NEW_ELIG_HOURS.DataSetName = "EligAcctHoursDS"
        Me.NEW_ELIG_HOURS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'SSNChangeControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Panel1)
        Me.Name = "SSNChangeControl"
        Me.Size = New System.Drawing.Size(961, 711)
        EligibiltyTabPage.ResumeLayout(False)
        Me.pnlBorder.ResumeLayout(False)
        CType(Me.NewSSNDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ControlPanel.ResumeLayout(False)
        CType(Me.OldSSNDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.grpremarks.ResumeLayout(False)
        Me.grpremarks.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.SSNTabControl.ResumeLayout(False)
        Me.HoursTabpage.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        CType(Me.NewHoursDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel5.ResumeLayout(False)
        CType(Me.OldHoursDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OLD_EMD_DS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NEW_EMD_DS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OLD_ELIG_HOURS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NEW_ELIG_HOURS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TTip As System.Windows.Forms.ToolTip
    Friend WithEvents txtOldSSN As ExTextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnoldClear As System.Windows.Forms.Button
    Friend WithEvents btnoldsearch As System.Windows.Forms.Button
    Friend WithEvents txtOldFID As ExTextBox
    Friend WithEvents FamilyIdLabel As System.Windows.Forms.Label
    Friend WithEvents OLD_EMD_DS As EligMthdtlDS
    Friend WithEvents pnlBorder As System.Windows.Forms.Panel
    Friend WithEvents NewSSNDataGrid As DataGridCustom
    Friend WithEvents ControlPanel As System.Windows.Forms.Panel
    Friend WithEvents btnremeligrow As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnremeligallrow As System.Windows.Forms.Button
    Friend WithEvents btnaddeligallrow As System.Windows.Forms.Button
    Friend WithEvents btnaddeligrow As System.Windows.Forms.Button
    Friend WithEvents VSplit As System.Windows.Forms.Splitter
    Friend WithEvents OldSSNDataGrid As DataGridCustom
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents NEW_EMD_DS As EligMthdtlDS
    Friend WithEvents SSNTabControl As System.Windows.Forms.TabControl
    Friend WithEvents HoursTabpage As System.Windows.Forms.TabPage
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents NewHoursDataGrid As DataGridCustom
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents btnremhrsrow As System.Windows.Forms.Button
    Friend WithEvents btnremhrsallrow As System.Windows.Forms.Button
    Friend WithEvents btnaddhrsallrow As System.Windows.Forms.Button
    Friend WithEvents btnaddhrsrow As System.Windows.Forms.Button
    Friend WithEvents Splitter2 As System.Windows.Forms.Splitter
    Friend WithEvents OldHoursDataGrid As DataGridCustom
    Friend WithEvents OLD_ELIG_HOURS As EligAcctHoursDS
    Friend WithEvents NEW_ELIG_HOURS As EligAcctHoursDS
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents txtNewSSN As ExTextBox
    Friend WithEvents btnNewClear As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnNewSearch As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtNewFID As ExTextBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents btncancelhrs As System.Windows.Forms.Button
    Friend WithEvents btnSaveHrs As System.Windows.Forms.Button
    Private WithEvents btnModifyHrs As System.Windows.Forms.Button
    Private WithEvents btnmodifyelig As System.Windows.Forms.Button
    Friend WithEvents txtRemarks As System.Windows.Forms.TextBox
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents btnsaveelig As System.Windows.Forms.Button
    Friend WithEvents btncancelelig As System.Windows.Forms.Button
    Friend WithEvents grpremarks As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label

End Class
