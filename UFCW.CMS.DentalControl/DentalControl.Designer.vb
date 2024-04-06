<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DentalControl
    Inherits System.Windows.Forms.UserControl

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Friend WithEvents DentalDataGrid As DataGridCustom
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents DentalFromDate As DateTimePicker
    Friend WithEvents DentalToDate As DateTimePicker
    Friend WithEvents RefreshButton As Button
    Friend WithEvents DentalContextMenuStrip As ContextMenuStrip
    Friend WithEvents LineDetailsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RePrintEOBToolStripMenuItem As ToolStripMenuItem
    'Friend WithEvents _DentalDS As DataSet
    Friend WithEvents _DentalDT As DataTable
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.LineDetailsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RePrintEOBToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DentalContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RefreshButton = New System.Windows.Forms.Button()
        Me.DentalToDate = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.DentalFromDate = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.DentalDataGrid = New DataGridCustom()
        Me.DentalSplitContainer = New System.Windows.Forms.SplitContainer()
        Me.DentalPREAUTHSplitContainer = New System.Windows.Forms.SplitContainer()
        Me.DentalPENDDataGrid = New DataGridCustom()
        Me.DentalPREAuthDataGrid = New DataGridCustom()
        Me._DentalDS = New System.Data.DataSet()
        Me._DentalPREAuthDS = New System.Data.DataSet()
        Me._DentalPendDS = New System.Data.DataSet()
        Me.DentalPreAuthContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.PreAuthLineDetailsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DentalContextMenuStrip.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.DentalDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DentalSplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DentalSplitContainer.Panel1.SuspendLayout()
        Me.DentalSplitContainer.Panel2.SuspendLayout()
        Me.DentalSplitContainer.SuspendLayout()
        CType(Me.DentalPREAUTHSplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DentalPREAUTHSplitContainer.Panel1.SuspendLayout()
        Me.DentalPREAUTHSplitContainer.Panel2.SuspendLayout()
        Me.DentalPREAUTHSplitContainer.SuspendLayout()
        CType(Me.DentalPENDDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DentalPREAuthDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._DentalDS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._DentalPREAuthDS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._DentalPendDS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DentalPreAuthContextMenuStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'LineDetailsToolStripMenuItem
        '
        Me.LineDetailsToolStripMenuItem.Name = "LineDetailsToolStripMenuItem"
        Me.LineDetailsToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.L), System.Windows.Forms.Keys)
        Me.LineDetailsToolStripMenuItem.Size = New System.Drawing.Size(174, 22)
        Me.LineDetailsToolStripMenuItem.Text = "&Line Details"
        '
        'RePrintEOBToolStripMenuItem
        '
        Me.RePrintEOBToolStripMenuItem.Name = "RePrintEOBToolStripMenuItem"
        Me.RePrintEOBToolStripMenuItem.Size = New System.Drawing.Size(174, 22)
        Me.RePrintEOBToolStripMenuItem.Text = "Reprint EOB"
        '
        'DentalContextMenuStrip
        '
        Me.DentalContextMenuStrip.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.DentalContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LineDetailsToolStripMenuItem, Me.RePrintEOBToolStripMenuItem})
        Me.DentalContextMenuStrip.Name = "DentalContextMenuStrip"
        Me.DentalContextMenuStrip.Size = New System.Drawing.Size(175, 48)
        '
        'RefreshButton
        '
        Me.RefreshButton.Location = New System.Drawing.Point(390, 3)
        Me.RefreshButton.Name = "RefreshButton"
        Me.RefreshButton.Size = New System.Drawing.Size(121, 25)
        Me.RefreshButton.TabIndex = 4
        Me.RefreshButton.Text = "Refresh"
        Me.RefreshButton.UseVisualStyleBackColor = True
        '
        'DentalToDate
        '
        Me.DentalToDate.Checked = False
        Me.DentalToDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DentalToDate.Location = New System.Drawing.Point(247, 7)
        Me.DentalToDate.Name = "DentalToDate"
        Me.DentalToDate.ShowCheckBox = True
        Me.DentalToDate.Size = New System.Drawing.Size(110, 20)
        Me.DentalToDate.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(211, 12)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(20, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        '
        'DentalFromDate
        '
        Me.DentalFromDate.Checked = False
        Me.DentalFromDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DentalFromDate.Location = New System.Drawing.Point(89, 7)
        Me.DentalFromDate.Name = "DentalFromDate"
        Me.DentalFromDate.ShowCheckBox = True
        Me.DentalFromDate.Size = New System.Drawing.Size(102, 20)
        Me.DentalFromDate.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(12, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Dental From"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.IsSplitterFixed = True
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.RefreshButton)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.DentalToDate)
        Me.SplitContainer1.Panel1.Controls.Add(Me.DentalFromDate)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label2)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.DentalDataGrid)
        Me.SplitContainer1.Size = New System.Drawing.Size(1220, 290)
        Me.SplitContainer1.SplitterDistance = 35
        Me.SplitContainer1.TabIndex = 3
        '
        'DentalDataGrid
        '
        Me.DentalDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.DentalDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.DentalDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DentalDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DentalDataGrid.ADGroupsThatCanFind = ""
        Me.DentalDataGrid.ADGroupsThatCanMultiSort = ""
        Me.DentalDataGrid.ADGroupsThatCanPrint = "CMSCanRePrintEOB"
        Me.DentalDataGrid.AllowAutoSize = True
        Me.DentalDataGrid.AllowColumnReorder = True
        Me.DentalDataGrid.AllowCopy = True
        Me.DentalDataGrid.AllowCustomize = True
        Me.DentalDataGrid.AllowDelete = False
        Me.DentalDataGrid.AllowDragDrop = False
        Me.DentalDataGrid.AllowEdit = False
        Me.DentalDataGrid.AllowExport = True
        Me.DentalDataGrid.AllowFilter = True
        Me.DentalDataGrid.AllowFind = True
        Me.DentalDataGrid.AllowGoTo = True
        Me.DentalDataGrid.AllowMultiSelect = False
        Me.DentalDataGrid.AllowMultiSort = True
        Me.DentalDataGrid.AllowNew = False
        Me.DentalDataGrid.AllowPrint = True
        Me.DentalDataGrid.AllowRefresh = True
        Me.DentalDataGrid.AppKey = "UFCW\Claims\"
        Me.DentalDataGrid.AutoSaveCols = True
        Me.DentalDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.DentalDataGrid.CaptionText = "Dental"
        Me.DentalDataGrid.ColumnHeaderLabel = Nothing
        Me.DentalDataGrid.ColumnRePositioning = False
        Me.DentalDataGrid.ColumnResizing = False
        Me.DentalDataGrid.ConfirmDelete = True
        Me.DentalDataGrid.CopySelectedOnly = True
        Me.DentalDataGrid.CurrentBSPosition = -1
        Me.DentalDataGrid.DataMember = ""
        Me.DentalDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DentalDataGrid.DragColumn = 0
        Me.DentalDataGrid.ExportSelectedOnly = True
        Me.DentalDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DentalDataGrid.HighlightedRow = Nothing
        Me.DentalDataGrid.HighLightModifiedRows = False
        Me.DentalDataGrid.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.DentalDataGrid.IsMouseDown = False
        Me.DentalDataGrid.LastGoToLine = ""
        Me.DentalDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.DentalDataGrid.MultiSort = False
        Me.DentalDataGrid.Name = "DentalDataGrid"
        Me.DentalDataGrid.OldSelectedRow = Nothing
        Me.DentalDataGrid.ParentRowsVisible = False
        Me.DentalDataGrid.PreviousBSPosition = -1
        Me.DentalDataGrid.ReadOnly = True
        Me.DentalDataGrid.RetainRowSelectionAfterSort = True
        Me.DentalDataGrid.SetRowOnRightClick = True
        Me.DentalDataGrid.ShiftPressed = False
        Me.DentalDataGrid.SingleClickBooleanColumns = True
        Me.DentalDataGrid.Size = New System.Drawing.Size(1220, 251)
        Me.DentalDataGrid.Sort = Nothing
        Me.DentalDataGrid.StyleName = ""
        Me.DentalDataGrid.SubKey = ""
        Me.DentalDataGrid.SuppressMouseDown = False
        Me.DentalDataGrid.SuppressTriangle = False
        Me.DentalDataGrid.TabIndex = 0
        '
        'DentalSplitContainer
        '
        Me.DentalSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DentalSplitContainer.Location = New System.Drawing.Point(0, 0)
        Me.DentalSplitContainer.Name = "DentalSplitContainer"
        Me.DentalSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'DentalSplitContainer.Panel1
        '
        Me.DentalSplitContainer.Panel1.Controls.Add(Me.DentalPREAUTHSplitContainer)
        '
        'DentalSplitContainer.Panel2
        '
        Me.DentalSplitContainer.Panel2.Controls.Add(Me.SplitContainer1)
        Me.DentalSplitContainer.Size = New System.Drawing.Size(1220, 450)
        Me.DentalSplitContainer.SplitterDistance = 156
        Me.DentalSplitContainer.TabIndex = 4
        '
        'DentalPREAUTHSplitContainer
        '
        Me.DentalPREAUTHSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DentalPREAUTHSplitContainer.Location = New System.Drawing.Point(0, 0)
        Me.DentalPREAUTHSplitContainer.Name = "DentalPREAUTHSplitContainer"
        '
        'DentalPREAUTHSplitContainer.Panel1
        '
        Me.DentalPREAUTHSplitContainer.Panel1.Controls.Add(Me.DentalPENDDataGrid)
        '
        'DentalPREAUTHSplitContainer.Panel2
        '
        Me.DentalPREAUTHSplitContainer.Panel2.Controls.Add(Me.DentalPREAuthDataGrid)
        Me.DentalPREAUTHSplitContainer.Size = New System.Drawing.Size(1220, 156)
        Me.DentalPREAUTHSplitContainer.SplitterDistance = 561
        Me.DentalPREAUTHSplitContainer.TabIndex = 0
        '
        'DentalPENDDataGrid
        '
        Me.DentalPENDDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.DentalPENDDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.DentalPENDDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DentalPENDDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DentalPENDDataGrid.ADGroupsThatCanFind = ""
        Me.DentalPENDDataGrid.ADGroupsThatCanMultiSort = ""
        Me.DentalPENDDataGrid.ADGroupsThatCanPrint = "CMSCanRePrintEOB"
        Me.DentalPENDDataGrid.AllowAutoSize = True
        Me.DentalPENDDataGrid.AllowColumnReorder = True
        Me.DentalPENDDataGrid.AllowCopy = True
        Me.DentalPENDDataGrid.AllowCustomize = True
        Me.DentalPENDDataGrid.AllowDelete = False
        Me.DentalPENDDataGrid.AllowDragDrop = False
        Me.DentalPENDDataGrid.AllowEdit = False
        Me.DentalPENDDataGrid.AllowExport = True
        Me.DentalPENDDataGrid.AllowFilter = True
        Me.DentalPENDDataGrid.AllowFind = True
        Me.DentalPENDDataGrid.AllowGoTo = True
        Me.DentalPENDDataGrid.AllowMultiSelect = False
        Me.DentalPENDDataGrid.AllowMultiSort = True
        Me.DentalPENDDataGrid.AllowNew = False
        Me.DentalPENDDataGrid.AllowPrint = True
        Me.DentalPENDDataGrid.AllowRefresh = True
        Me.DentalPENDDataGrid.AppKey = "UFCW\Claims\"
        Me.DentalPENDDataGrid.AutoSaveCols = True
        Me.DentalPENDDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.DentalPENDDataGrid.CaptionText = "Pending Review And Processing"
        Me.DentalPENDDataGrid.ColumnHeaderLabel = Nothing
        Me.DentalPENDDataGrid.ColumnRePositioning = False
        Me.DentalPENDDataGrid.ColumnResizing = False
        Me.DentalPENDDataGrid.ConfirmDelete = True
        Me.DentalPENDDataGrid.CopySelectedOnly = True
        Me.DentalPENDDataGrid.CurrentBSPosition = -1
        Me.DentalPENDDataGrid.DataMember = ""
        Me.DentalPENDDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DentalPENDDataGrid.DragColumn = 0
        Me.DentalPENDDataGrid.ExportSelectedOnly = True
        Me.DentalPENDDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DentalPENDDataGrid.HighlightedRow = Nothing
        Me.DentalPENDDataGrid.HighLightModifiedRows = False
        Me.DentalPENDDataGrid.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.DentalPENDDataGrid.IsMouseDown = False
        Me.DentalPENDDataGrid.LastGoToLine = ""
        Me.DentalPENDDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.DentalPENDDataGrid.MultiSort = False
        Me.DentalPENDDataGrid.Name = "DentalPENDDataGrid"
        Me.DentalPENDDataGrid.OldSelectedRow = Nothing
        Me.DentalPENDDataGrid.ParentRowsVisible = False
        Me.DentalPENDDataGrid.PreviousBSPosition = -1
        Me.DentalPENDDataGrid.ReadOnly = True
        Me.DentalPENDDataGrid.RetainRowSelectionAfterSort = True
        Me.DentalPENDDataGrid.SetRowOnRightClick = True
        Me.DentalPENDDataGrid.ShiftPressed = False
        Me.DentalPENDDataGrid.SingleClickBooleanColumns = True
        Me.DentalPENDDataGrid.Size = New System.Drawing.Size(561, 156)
        Me.DentalPENDDataGrid.Sort = Nothing
        Me.DentalPENDDataGrid.StyleName = ""
        Me.DentalPENDDataGrid.SubKey = ""
        Me.DentalPENDDataGrid.SuppressMouseDown = False
        Me.DentalPENDDataGrid.SuppressTriangle = False
        Me.DentalPENDDataGrid.TabIndex = 4
        '
        'DentalPREAuthDataGrid
        '
        Me.DentalPREAuthDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.DentalPREAuthDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.DentalPREAuthDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DentalPREAuthDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DentalPREAuthDataGrid.ADGroupsThatCanFind = ""
        Me.DentalPREAuthDataGrid.ADGroupsThatCanMultiSort = ""
        Me.DentalPREAuthDataGrid.ADGroupsThatCanPrint = "CMSCanRePrintEOB"
        Me.DentalPREAuthDataGrid.AllowAutoSize = True
        Me.DentalPREAuthDataGrid.AllowColumnReorder = True
        Me.DentalPREAuthDataGrid.AllowCopy = True
        Me.DentalPREAuthDataGrid.AllowCustomize = True
        Me.DentalPREAuthDataGrid.AllowDelete = False
        Me.DentalPREAuthDataGrid.AllowDragDrop = False
        Me.DentalPREAuthDataGrid.AllowEdit = False
        Me.DentalPREAuthDataGrid.AllowExport = True
        Me.DentalPREAuthDataGrid.AllowFilter = True
        Me.DentalPREAuthDataGrid.AllowFind = True
        Me.DentalPREAuthDataGrid.AllowGoTo = True
        Me.DentalPREAuthDataGrid.AllowMultiSelect = False
        Me.DentalPREAuthDataGrid.AllowMultiSort = True
        Me.DentalPREAuthDataGrid.AllowNew = False
        Me.DentalPREAuthDataGrid.AllowPrint = True
        Me.DentalPREAuthDataGrid.AllowRefresh = True
        Me.DentalPREAuthDataGrid.AppKey = "UFCW\Claims\"
        Me.DentalPREAuthDataGrid.AutoSaveCols = True
        Me.DentalPREAuthDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.DentalPREAuthDataGrid.CaptionText = "Pre Authorizations"
        Me.DentalPREAuthDataGrid.ColumnHeaderLabel = Nothing
        Me.DentalPREAuthDataGrid.ColumnRePositioning = False
        Me.DentalPREAuthDataGrid.ColumnResizing = False
        Me.DentalPREAuthDataGrid.ConfirmDelete = True
        Me.DentalPREAuthDataGrid.CopySelectedOnly = True
        Me.DentalPREAuthDataGrid.CurrentBSPosition = -1
        Me.DentalPREAuthDataGrid.DataMember = ""
        Me.DentalPREAuthDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DentalPREAuthDataGrid.DragColumn = 0
        Me.DentalPREAuthDataGrid.ExportSelectedOnly = True
        Me.DentalPREAuthDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DentalPREAuthDataGrid.HighlightedRow = Nothing
        Me.DentalPREAuthDataGrid.HighLightModifiedRows = False
        Me.DentalPREAuthDataGrid.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.DentalPREAuthDataGrid.IsMouseDown = False
        Me.DentalPREAuthDataGrid.LastGoToLine = ""
        Me.DentalPREAuthDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.DentalPREAuthDataGrid.MultiSort = False
        Me.DentalPREAuthDataGrid.Name = "DentalPREAuthDataGrid"
        Me.DentalPREAuthDataGrid.OldSelectedRow = Nothing
        Me.DentalPREAuthDataGrid.ParentRowsVisible = False
        Me.DentalPREAuthDataGrid.PreviousBSPosition = -1
        Me.DentalPREAuthDataGrid.ReadOnly = True
        Me.DentalPREAuthDataGrid.RetainRowSelectionAfterSort = True
        Me.DentalPREAuthDataGrid.SetRowOnRightClick = True
        Me.DentalPREAuthDataGrid.ShiftPressed = False
        Me.DentalPREAuthDataGrid.SingleClickBooleanColumns = True
        Me.DentalPREAuthDataGrid.Size = New System.Drawing.Size(655, 156)
        Me.DentalPREAuthDataGrid.Sort = Nothing
        Me.DentalPREAuthDataGrid.StyleName = ""
        Me.DentalPREAuthDataGrid.SubKey = ""
        Me.DentalPREAuthDataGrid.SuppressMouseDown = False
        Me.DentalPREAuthDataGrid.SuppressTriangle = False
        Me.DentalPREAuthDataGrid.TabIndex = 3
        '
        '_DentalDS
        '
        Me._DentalDS.DataSetName = "DentalDataSet"
        Me._DentalDS.Locale = New System.Globalization.CultureInfo("en-US")
        '
        '_DentalPREAuthDS
        '
        Me._DentalPREAuthDS.DataSetName = "DentalPREAuthDS"
        '
        '_DentalPendDS
        '
        Me._DentalPendDS.DataSetName = "DentalPendDS"
        '
        'DentalPreAuthContextMenuStrip
        '
        Me.DentalPreAuthContextMenuStrip.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.DentalPreAuthContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PreAuthLineDetailsToolStripMenuItem})
        Me.DentalPreAuthContextMenuStrip.Name = "DentalContextMenuStrip"
        Me.DentalPreAuthContextMenuStrip.Size = New System.Drawing.Size(181, 48)
        '
        'PreAuthLineDetailsToolStripMenuItem
        '
        Me.PreAuthLineDetailsToolStripMenuItem.Name = "PreAuthLineDetailsToolStripMenuItem"
        Me.PreAuthLineDetailsToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.L), System.Windows.Forms.Keys)
        Me.PreAuthLineDetailsToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.PreAuthLineDetailsToolStripMenuItem.Text = "&Line Details"
        '
        'DentalControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.Controls.Add(Me.DentalSplitContainer)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "DentalControl"
        Me.Size = New System.Drawing.Size(1220, 450)
        Me.DentalContextMenuStrip.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.DentalDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DentalSplitContainer.Panel1.ResumeLayout(False)
        Me.DentalSplitContainer.Panel2.ResumeLayout(False)
        CType(Me.DentalSplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DentalSplitContainer.ResumeLayout(False)
        Me.DentalPREAUTHSplitContainer.Panel1.ResumeLayout(False)
        Me.DentalPREAUTHSplitContainer.Panel2.ResumeLayout(False)
        CType(Me.DentalPREAUTHSplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DentalPREAUTHSplitContainer.ResumeLayout(False)
        CType(Me.DentalPENDDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DentalPREAuthDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._DentalDS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._DentalPREAuthDS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._DentalPendDS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DentalPreAuthContextMenuStrip.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents DentalSplitContainer As SplitContainer
    Friend WithEvents DentalPREAUTHSplitContainer As SplitContainer
    Friend WithEvents DentalPREAuthDataGrid As DataGridCustom
    ' Friend WithEvents _DentalPREAuthDS As DataSet
    Friend WithEvents DentalPENDDataGrid As DataGridCustom
    Friend WithEvents _DentalDS As DataSet
    Friend WithEvents _DentalPREAuthDS As DataSet
    Friend WithEvents _DentalPendDS As DataSet
    Friend WithEvents DentalPreAuthContextMenuStrip As ContextMenuStrip
    Friend WithEvents PreAuthLineDetailsToolStripMenuItem As ToolStripMenuItem
End Class
