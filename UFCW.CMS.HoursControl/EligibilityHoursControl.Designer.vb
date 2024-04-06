<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EligibilityHoursControl
    Inherits System.Windows.Forms.UserControl

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.EligibilityHoursDataGrid = New DataGridCustom()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.AndLabel3 = New System.Windows.Forms.Label()
        Me.BetweenLabel3 = New System.Windows.Forms.Label()
        Me.PPToDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.PPFromDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.ppCheckBox = New System.Windows.Forms.CheckBox()
        Me.EPToDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.epFromDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.EPCheckBox = New System.Windows.Forms.CheckBox()
        Me.cmbSpecial = New System.Windows.Forms.ComboBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.EligAcctHrsContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SpecialAccountsMaintenanceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SpecialAccountRemarksToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CalculateEligibilityToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TotalHoursViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetForRecalculationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.EligibilityHoursDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.EligAcctHrsContextMenuStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'EligibilityHoursDataGrid
        '
        Me.EligibilityHoursDataGrid.ADGroupsThatCanCopy = "CMSUsers,REGMUser"
        Me.EligibilityHoursDataGrid.ADGroupsThatCanCustomize = "CMSUsers,REGMUser"
        Me.EligibilityHoursDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators,REGMUser"
        Me.EligibilityHoursDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators,REGMUser"
        Me.EligibilityHoursDataGrid.ADGroupsThatCanFind = ""
        Me.EligibilityHoursDataGrid.ADGroupsThatCanMultiSort = "REGMUser"
        Me.EligibilityHoursDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.EligibilityHoursDataGrid.AllowAutoSize = True
        Me.EligibilityHoursDataGrid.AllowColumnReorder = True
        Me.EligibilityHoursDataGrid.AllowCopy = True
        Me.EligibilityHoursDataGrid.AllowCustomize = True
        Me.EligibilityHoursDataGrid.AllowDelete = False
        Me.EligibilityHoursDataGrid.AllowDragDrop = False
        Me.EligibilityHoursDataGrid.AllowEdit = False
        Me.EligibilityHoursDataGrid.AllowExport = True
        Me.EligibilityHoursDataGrid.AllowFilter = True
        Me.EligibilityHoursDataGrid.AllowFind = True
        Me.EligibilityHoursDataGrid.AllowGoTo = True
        Me.EligibilityHoursDataGrid.AllowMultiSelect = True
        Me.EligibilityHoursDataGrid.AllowMultiSort = True
        Me.EligibilityHoursDataGrid.AllowNew = False
        Me.EligibilityHoursDataGrid.AllowPrint = True
        Me.EligibilityHoursDataGrid.AllowRefresh = True
        Me.EligibilityHoursDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.EligibilityHoursDataGrid.AppKey = "UFCW\Claims\"
        Me.EligibilityHoursDataGrid.AutoSaveCols = True
        Me.EligibilityHoursDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.EligibilityHoursDataGrid.CaptionText = "Eligibility Hours"
        Me.EligibilityHoursDataGrid.ColumnHeaderLabel = Nothing
        Me.EligibilityHoursDataGrid.ColumnRePositioning = False
        Me.EligibilityHoursDataGrid.ColumnResizing = False
        Me.EligibilityHoursDataGrid.ConfirmDelete = True
        Me.EligibilityHoursDataGrid.CopySelectedOnly = True
        Me.EligibilityHoursDataGrid.DataMember = ""
        Me.EligibilityHoursDataGrid.DragColumn = 0
        Me.EligibilityHoursDataGrid.ExportSelectedOnly = True
        Me.EligibilityHoursDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.EligibilityHoursDataGrid.HighlightedRow = Nothing
        Me.EligibilityHoursDataGrid.HighLightModifiedRows = False
        Me.EligibilityHoursDataGrid.IsMouseDown = False
        Me.EligibilityHoursDataGrid.LastGoToLine = ""
        Me.EligibilityHoursDataGrid.Location = New System.Drawing.Point(3, 84)
        Me.EligibilityHoursDataGrid.MultiSort = False
        Me.EligibilityHoursDataGrid.Name = "EligibilityHoursDataGrid"
        Me.EligibilityHoursDataGrid.OldSelectedRow = 0
        Me.EligibilityHoursDataGrid.ReadOnly = True
        Me.EligibilityHoursDataGrid.RetainRowSelectionAfterSort = True
        Me.EligibilityHoursDataGrid.SetRowOnRightClick = True
        Me.EligibilityHoursDataGrid.ShiftPressed = False
        Me.EligibilityHoursDataGrid.SingleClickBooleanColumns = True
        Me.EligibilityHoursDataGrid.Size = New System.Drawing.Size(651, 313)
        Me.EligibilityHoursDataGrid.Sort = Nothing
        Me.EligibilityHoursDataGrid.StyleName = ""
        Me.EligibilityHoursDataGrid.SubKey = ""
        Me.EligibilityHoursDataGrid.SuppressTriangle = False
        Me.EligibilityHoursDataGrid.TabIndex = 7
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.AndLabel3)
        Me.GroupBox1.Controls.Add(Me.BetweenLabel3)
        Me.GroupBox1.Controls.Add(Me.PPToDateTimePicker)
        Me.GroupBox1.Controls.Add(Me.PPFromDateTimePicker)
        Me.GroupBox1.Controls.Add(Me.ppCheckBox)
        Me.GroupBox1.Controls.Add(Me.EPToDateTimePicker)
        Me.GroupBox1.Controls.Add(Me.epFromDateTimePicker)
        Me.GroupBox1.Controls.Add(Me.EPCheckBox)
        Me.GroupBox1.Controls.Add(Me.cmbSpecial)
        Me.GroupBox1.Controls.Add(Me.btnSearch)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(645, 85)
        Me.GroupBox1.TabIndex = 9
        Me.GroupBox1.TabStop = False
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(10, 13)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(89, 16)
        Me.Label3.TabIndex = 51
        Me.Label3.Text = "Special Account"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(262, 59)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(32, 16)
        Me.Label1.TabIndex = 50
        Me.Label1.Text = "And"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(111, 58)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(49, 16)
        Me.Label2.TabIndex = 49
        Me.Label2.Text = "Between"
        '
        'AndLabel3
        '
        Me.AndLabel3.Location = New System.Drawing.Point(262, 39)
        Me.AndLabel3.Name = "AndLabel3"
        Me.AndLabel3.Size = New System.Drawing.Size(32, 16)
        Me.AndLabel3.TabIndex = 48
        Me.AndLabel3.Text = "And"
        '
        'BetweenLabel3
        '
        Me.BetweenLabel3.Location = New System.Drawing.Point(111, 38)
        Me.BetweenLabel3.Name = "BetweenLabel3"
        Me.BetweenLabel3.Size = New System.Drawing.Size(49, 16)
        Me.BetweenLabel3.TabIndex = 47
        Me.BetweenLabel3.Text = "Between"
        '
        'PPToDateTimePicker
        '
        Me.PPToDateTimePicker.Enabled = False
        Me.PPToDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.PPToDateTimePicker.Location = New System.Drawing.Point(295, 35)
        Me.PPToDateTimePicker.Name = "PPToDateTimePicker"
        Me.PPToDateTimePicker.Size = New System.Drawing.Size(95, 20)
        Me.PPToDateTimePicker.TabIndex = 46
        '
        'PPFromDateTimePicker
        '
        Me.PPFromDateTimePicker.Enabled = False
        Me.PPFromDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.PPFromDateTimePicker.Location = New System.Drawing.Point(166, 35)
        Me.PPFromDateTimePicker.Name = "PPFromDateTimePicker"
        Me.PPFromDateTimePicker.Size = New System.Drawing.Size(95, 20)
        Me.PPFromDateTimePicker.TabIndex = 45
        '
        'ppCheckBox
        '
        Me.ppCheckBox.Location = New System.Drawing.Point(13, 35)
        Me.ppCheckBox.Name = "ppCheckBox"
        Me.ppCheckBox.Size = New System.Drawing.Size(104, 16)
        Me.ppCheckBox.TabIndex = 44
        Me.ppCheckBox.Text = "Work Period"
        '
        'EPToDateTimePicker
        '
        Me.EPToDateTimePicker.Enabled = False
        Me.EPToDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.EPToDateTimePicker.Location = New System.Drawing.Point(295, 57)
        Me.EPToDateTimePicker.Name = "EPToDateTimePicker"
        Me.EPToDateTimePicker.Size = New System.Drawing.Size(95, 20)
        Me.EPToDateTimePicker.TabIndex = 43
        '
        'epFromDateTimePicker
        '
        Me.epFromDateTimePicker.Enabled = False
        Me.epFromDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.epFromDateTimePicker.Location = New System.Drawing.Point(166, 57)
        Me.epFromDateTimePicker.Name = "epFromDateTimePicker"
        Me.epFromDateTimePicker.Size = New System.Drawing.Size(95, 20)
        Me.epFromDateTimePicker.TabIndex = 42
        '
        'EPCheckBox
        '
        Me.EPCheckBox.Location = New System.Drawing.Point(13, 57)
        Me.EPCheckBox.Name = "EPCheckBox"
        Me.EPCheckBox.Size = New System.Drawing.Size(104, 18)
        Me.EPCheckBox.TabIndex = 41
        Me.EPCheckBox.Text = "Eligibility Period"
        '
        'cmbSpecial
        '
        Me.cmbSpecial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSpecial.FormattingEnabled = True
        Me.cmbSpecial.Location = New System.Drawing.Point(114, 10)
        Me.cmbSpecial.Name = "cmbSpecial"
        Me.cmbSpecial.Size = New System.Drawing.Size(227, 21)
        Me.cmbSpecial.TabIndex = 5
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(430, 39)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(75, 21)
        Me.btnSearch.TabIndex = 4
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'EligAcctHrsContextMenuStrip
        '
        Me.EligAcctHrsContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SpecialAccountsMaintenanceToolStripMenuItem, Me.SpecialAccountRemarksToolStripMenuItem, Me.CalculateEligibilityToolStripMenuItem, Me.TotalHoursViewToolStripMenuItem, Me.SetForRecalculationToolStripMenuItem})
        Me.EligAcctHrsContextMenuStrip.Name = "EligAcctHrsContextMenuStrip"
        Me.EligAcctHrsContextMenuStrip.Size = New System.Drawing.Size(237, 114)
        '
        'SpecialAccountsMaintenanceToolStripMenuItem
        '
        Me.SpecialAccountsMaintenanceToolStripMenuItem.Name = "SpecialAccountsMaintenanceToolStripMenuItem"
        Me.SpecialAccountsMaintenanceToolStripMenuItem.Size = New System.Drawing.Size(236, 22)
        Me.SpecialAccountsMaintenanceToolStripMenuItem.Text = "Special Accounts Maintenance"
        '
        'SpecialAccountRemarksToolStripMenuItem
        '
        Me.SpecialAccountRemarksToolStripMenuItem.Name = "SpecialAccountRemarksToolStripMenuItem"
        Me.SpecialAccountRemarksToolStripMenuItem.Size = New System.Drawing.Size(236, 22)
        Me.SpecialAccountRemarksToolStripMenuItem.Text = "Special Account Remarks"
        '
        'CalculateEligibilityToolStripMenuItem
        '
        Me.CalculateEligibilityToolStripMenuItem.Name = "CalculateEligibilityToolStripMenuItem"
        Me.CalculateEligibilityToolStripMenuItem.Size = New System.Drawing.Size(236, 22)
        Me.CalculateEligibilityToolStripMenuItem.Text = "Calculate Eligibility"
        '
        'TotalHoursViewToolStripMenuItem
        '
        Me.TotalHoursViewToolStripMenuItem.Name = "TotalHoursViewToolStripMenuItem"
        Me.TotalHoursViewToolStripMenuItem.Size = New System.Drawing.Size(236, 22)
        Me.TotalHoursViewToolStripMenuItem.Text = "Total Hours View"
        '
        'SetForRecalculationToolStripMenuItem
        '
        Me.SetForRecalculationToolStripMenuItem.Name = "SetForRecalculationToolStripMenuItem"
        Me.SetForRecalculationToolStripMenuItem.Size = New System.Drawing.Size(236, 22)
        Me.SetForRecalculationToolStripMenuItem.Text = "Set Recalculation Switches"
        '
        'EligibilityHoursControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.EligibilityHoursDataGrid)
        Me.Name = "EligibilityHoursControl"
        Me.Size = New System.Drawing.Size(651, 400)
        CType(Me.EligibilityHoursDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.EligAcctHrsContextMenuStrip.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents EligibilityHoursDataGrid As DataGridCustom
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents cmbSpecial As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents AndLabel3 As System.Windows.Forms.Label
    Friend WithEvents BetweenLabel3 As System.Windows.Forms.Label
    Friend WithEvents PPToDateTimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents PPFromDateTimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents ppCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents EPToDateTimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents epFromDateTimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents EPCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents EligAcctHrsContextMenuStrip As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SpecialAccountsMaintenanceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SpecialAccountRemarksToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CalculateEligibilityToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TotalHoursViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetForRecalculationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
