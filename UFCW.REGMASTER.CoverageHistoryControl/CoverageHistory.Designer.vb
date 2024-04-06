Imports System.ComponentModel
Imports System.Windows.Forms

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CoverageHistory
    Inherits System.Windows.Forms.UserControl

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.MedCoverageHistoryDataGrid = New DataGridCustom()
        Me.CHSplitContainer = New System.Windows.Forms.SplitContainer()
        Me.DenCoverageHistoryDataGrid = New DataGridCustom()
        Me.NetworkContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.HMONetworkInfoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CoverageMaintenanceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.MedCoverageHistoryDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CHSplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.CHSplitContainer.Panel1.SuspendLayout()
        Me.CHSplitContainer.Panel2.SuspendLayout()
        Me.CHSplitContainer.SuspendLayout()
        CType(Me.DenCoverageHistoryDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.NetworkContextMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'MedicalCoverageHistoryDataGrid
        '
        Me.MedCoverageHistoryDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.MedCoverageHistoryDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.MedCoverageHistoryDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.MedCoverageHistoryDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.MedCoverageHistoryDataGrid.ADGroupsThatCanFind = ""
        Me.MedCoverageHistoryDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.MedCoverageHistoryDataGrid.ADGroupsThatCanMultiSort = ""
        Me.MedCoverageHistoryDataGrid.AllowAutoSize = True
        Me.MedCoverageHistoryDataGrid.AllowColumnReorder = True
        Me.MedCoverageHistoryDataGrid.AllowCopy = True
        Me.MedCoverageHistoryDataGrid.AllowCustomize = True
        Me.MedCoverageHistoryDataGrid.AllowDelete = False
        Me.MedCoverageHistoryDataGrid.AllowDragDrop = True
        Me.MedCoverageHistoryDataGrid.AllowEdit = False
        Me.MedCoverageHistoryDataGrid.AllowExport = True
        Me.MedCoverageHistoryDataGrid.AllowFilter = False
        Me.MedCoverageHistoryDataGrid.AllowFind = True
        Me.MedCoverageHistoryDataGrid.AllowGoTo = True
        Me.MedCoverageHistoryDataGrid.AllowMultiSelect = False
        Me.MedCoverageHistoryDataGrid.AllowMultiSort = True
        Me.MedCoverageHistoryDataGrid.AllowNew = False
        Me.MedCoverageHistoryDataGrid.AllowPrint = True
        Me.MedCoverageHistoryDataGrid.AllowRefresh = True
        Me.MedCoverageHistoryDataGrid.AppKey = "UFCW\RegMaster\"
        Me.MedCoverageHistoryDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.MedCoverageHistoryDataGrid.CaptionText = "Medical Coverage History"
        Me.MedCoverageHistoryDataGrid.ColumnHeaderLabel = Nothing
        Me.MedCoverageHistoryDataGrid.ColumnRePositioning = False
        Me.MedCoverageHistoryDataGrid.ColumnResizing = False
        Me.MedCoverageHistoryDataGrid.ConfirmDelete = True
        Me.MedCoverageHistoryDataGrid.CopySelectedOnly = True
        Me.MedCoverageHistoryDataGrid.DataMember = ""
        Me.MedCoverageHistoryDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MedCoverageHistoryDataGrid.DragColumn = 0
        Me.MedCoverageHistoryDataGrid.ExportSelectedOnly = True
        Me.MedCoverageHistoryDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.MedCoverageHistoryDataGrid.HighlightedRow = Nothing
        Me.MedCoverageHistoryDataGrid.IsMouseDown = False
        Me.MedCoverageHistoryDataGrid.LastGoToLine = ""
        Me.MedCoverageHistoryDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.MedCoverageHistoryDataGrid.MultiSort = False
        Me.MedCoverageHistoryDataGrid.Name = "MedCoverageHistoryDataGrid"
        Me.MedCoverageHistoryDataGrid.OldSelectedRow = 0
        Me.MedCoverageHistoryDataGrid.ReadOnly = True
        Me.MedCoverageHistoryDataGrid.SetRowOnRightClick = True
        Me.MedCoverageHistoryDataGrid.ShiftPressed = False
        Me.MedCoverageHistoryDataGrid.SingleClickBooleanColumns = True
        Me.MedCoverageHistoryDataGrid.Size = New System.Drawing.Size(207, 445)
        Me.MedCoverageHistoryDataGrid.StyleName = ""
        Me.MedCoverageHistoryDataGrid.SubKey = ""
        Me.MedCoverageHistoryDataGrid.SuppressTriangle = False
        Me.MedCoverageHistoryDataGrid.TabIndex = 7
        '
        'CHSplitContainer
        '
        Me.CHSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CHSplitContainer.Location = New System.Drawing.Point(0, 0)
        Me.CHSplitContainer.Name = "CHSplitContainer"
        '
        'CHSplitContainer.Panel1
        '
        Me.CHSplitContainer.Panel1.Controls.Add(Me.MedCoverageHistoryDataGrid)
        '
        'CHSplitContainer.Panel2
        '
        Me.CHSplitContainer.Panel2.Controls.Add(Me.DenCoverageHistoryDataGrid)
        Me.CHSplitContainer.Size = New System.Drawing.Size(462, 445)
        Me.CHSplitContainer.SplitterDistance = 207
        Me.CHSplitContainer.TabIndex = 8
        '
        'DentalCoverageHistoryDataGrid
        '
        Me.DenCoverageHistoryDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.DenCoverageHistoryDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.DenCoverageHistoryDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DenCoverageHistoryDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DenCoverageHistoryDataGrid.ADGroupsThatCanFind = ""
        Me.DenCoverageHistoryDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DenCoverageHistoryDataGrid.ADGroupsThatCanMultiSort = ""
        Me.DenCoverageHistoryDataGrid.AllowAutoSize = True
        Me.DenCoverageHistoryDataGrid.AllowColumnReorder = True
        Me.DenCoverageHistoryDataGrid.AllowCopy = True
        Me.DenCoverageHistoryDataGrid.AllowCustomize = True
        Me.DenCoverageHistoryDataGrid.AllowDelete = False
        Me.DenCoverageHistoryDataGrid.AllowDragDrop = True
        Me.DenCoverageHistoryDataGrid.AllowEdit = False
        Me.DenCoverageHistoryDataGrid.AllowExport = True
        Me.DenCoverageHistoryDataGrid.AllowFilter = False
        Me.DenCoverageHistoryDataGrid.AllowFind = True
        Me.DenCoverageHistoryDataGrid.AllowGoTo = True
        Me.DenCoverageHistoryDataGrid.AllowMultiSelect = False
        Me.DenCoverageHistoryDataGrid.AllowMultiSort = True
        Me.DenCoverageHistoryDataGrid.AllowNew = False
        Me.DenCoverageHistoryDataGrid.AllowPrint = True
        Me.DenCoverageHistoryDataGrid.AllowRefresh = True
        Me.DenCoverageHistoryDataGrid.AppKey = "UFCW\RegMaster\"
        Me.DenCoverageHistoryDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.DenCoverageHistoryDataGrid.CaptionText = "Dental Coverage History"
        Me.DenCoverageHistoryDataGrid.ColumnHeaderLabel = Nothing
        Me.DenCoverageHistoryDataGrid.ColumnRePositioning = False
        Me.DenCoverageHistoryDataGrid.ColumnResizing = False
        Me.DenCoverageHistoryDataGrid.ConfirmDelete = True
        Me.DenCoverageHistoryDataGrid.CopySelectedOnly = True
        Me.DenCoverageHistoryDataGrid.DataMember = ""
        Me.DenCoverageHistoryDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DenCoverageHistoryDataGrid.DragColumn = 0
        Me.DenCoverageHistoryDataGrid.ExportSelectedOnly = True
        Me.DenCoverageHistoryDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DenCoverageHistoryDataGrid.HighlightedRow = Nothing
        Me.DenCoverageHistoryDataGrid.IsMouseDown = False
        Me.DenCoverageHistoryDataGrid.LastGoToLine = ""
        Me.DenCoverageHistoryDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.DenCoverageHistoryDataGrid.MultiSort = False
        Me.DenCoverageHistoryDataGrid.Name = "DenCoverageHistoryDataGrid"
        Me.DenCoverageHistoryDataGrid.OldSelectedRow = 0
        Me.DenCoverageHistoryDataGrid.ReadOnly = True
        Me.DenCoverageHistoryDataGrid.SetRowOnRightClick = True
        Me.DenCoverageHistoryDataGrid.ShiftPressed = False
        Me.DenCoverageHistoryDataGrid.SingleClickBooleanColumns = True
        Me.DenCoverageHistoryDataGrid.Size = New System.Drawing.Size(251, 445)
        Me.DenCoverageHistoryDataGrid.StyleName = ""
        Me.DenCoverageHistoryDataGrid.SubKey = ""
        Me.DenCoverageHistoryDataGrid.SuppressTriangle = False
        Me.DenCoverageHistoryDataGrid.TabIndex = 8
        '
        'NetworkContextMenu
        '
        Me.NetworkContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CoverageMaintenanceToolStripMenuItem, Me.HMONetworkInfoToolStripMenuItem})
        Me.NetworkContextMenu.Name = "ResultsDataGridCustomContextMenu"
        Me.NetworkContextMenu.Size = New System.Drawing.Size(197, 48)
        '
        'HMONetworkInfoToolStripMenuItem
        '
        Me.HMONetworkInfoToolStripMenuItem.Name = "HMONetworkInfoToolStripMenuItem"
        Me.HMONetworkInfoToolStripMenuItem.Size = New System.Drawing.Size(196, 22)
        Me.HMONetworkInfoToolStripMenuItem.Text = "HMO Network Info"
        '
        'CoverageMaintenanceToolStripMenuItem
        '
        Me.CoverageMaintenanceToolStripMenuItem.Name = "CoverageMaintenanceToolStripMenuItem"
        Me.CoverageMaintenanceToolStripMenuItem.Size = New System.Drawing.Size(196, 22)
        Me.CoverageMaintenanceToolStripMenuItem.Text = "Coverage Maintenance"
        '
        'CoverageHistory
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.CHSplitContainer)
        Me.Name = "CoverageHistory"
        Me.Size = New System.Drawing.Size(462, 445)
        CType(Me.MedCoverageHistoryDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.CHSplitContainer.Panel1.ResumeLayout(False)
        Me.CHSplitContainer.Panel2.ResumeLayout(False)
        CType(Me.CHSplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.CHSplitContainer.ResumeLayout(False)
        CType(Me.DenCoverageHistoryDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.NetworkContextMenu.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents MedCoverageHistoryDataGrid As DataGridCustom
    Friend WithEvents CHSplitContainer As System.Windows.Forms.SplitContainer
    Friend WithEvents DenCoverageHistoryDataGrid As DataGridCustom
    Friend WithEvents NetworkContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents HMONetworkInfoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CoverageMaintenanceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
