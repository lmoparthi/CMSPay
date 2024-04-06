<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ClaimHistory
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ClaimsHistoryDataGrid = New DataGridCustom()
        Me.ClaimsHistoryDataGridCustomContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.DisplayMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitButton = New System.Windows.Forms.Button()
        CType(Me.ClaimsHistoryDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ClaimsHistoryDataGridCustomContextMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'ClaimsHistoryDataGrid
        '
        Me.ClaimsHistoryDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.ClaimsHistoryDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.ClaimsHistoryDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ClaimsHistoryDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ClaimsHistoryDataGrid.ADGroupsThatCanFind = ""
        Me.ClaimsHistoryDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ClaimsHistoryDataGrid.ADGroupsThatCanMultiSort = ""
        Me.ClaimsHistoryDataGrid.AllowAutoSize = True
        Me.ClaimsHistoryDataGrid.AllowColumnReorder = True
        Me.ClaimsHistoryDataGrid.AllowCopy = False
        Me.ClaimsHistoryDataGrid.AllowCustomize = True
        Me.ClaimsHistoryDataGrid.AllowDelete = False
        Me.ClaimsHistoryDataGrid.AllowDragDrop = False
        Me.ClaimsHistoryDataGrid.AllowEdit = False
        Me.ClaimsHistoryDataGrid.AllowExport = True
        Me.ClaimsHistoryDataGrid.AllowFilter = True
        Me.ClaimsHistoryDataGrid.AllowFind = True
        Me.ClaimsHistoryDataGrid.AllowGoTo = True
        Me.ClaimsHistoryDataGrid.AllowMultiSelect = False
        Me.ClaimsHistoryDataGrid.AllowMultiSort = False
        Me.ClaimsHistoryDataGrid.AllowNew = False
        Me.ClaimsHistoryDataGrid.AllowPrint = False
        Me.ClaimsHistoryDataGrid.AllowRefresh = False
        Me.ClaimsHistoryDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ClaimsHistoryDataGrid.AppKey = "UFCW\Claims\"
        Me.ClaimsHistoryDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.ClaimsHistoryDataGrid.CaptionVisible = False
        Me.ClaimsHistoryDataGrid.ColumnHeaderLabel = Nothing
        Me.ClaimsHistoryDataGrid.ColumnRePositioning = False
        Me.ClaimsHistoryDataGrid.ColumnResizing = False
        Me.ClaimsHistoryDataGrid.ConfirmDelete = True
        Me.ClaimsHistoryDataGrid.ContextMenuStrip = Me.ClaimsHistoryDataGridCustomContextMenu
        Me.ClaimsHistoryDataGrid.CopySelectedOnly = True
        Me.ClaimsHistoryDataGrid.DataMember = ""
        Me.ClaimsHistoryDataGrid.DragColumn = 0
        Me.ClaimsHistoryDataGrid.ExportSelectedOnly = True
        Me.ClaimsHistoryDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ClaimsHistoryDataGrid.HighlightedRow = Nothing
        Me.ClaimsHistoryDataGrid.IsMouseDown = False
        Me.ClaimsHistoryDataGrid.LastGoToLine = ""
        Me.ClaimsHistoryDataGrid.Location = New System.Drawing.Point(12, 12)
        Me.ClaimsHistoryDataGrid.MultiSort = False
        Me.ClaimsHistoryDataGrid.Name = "ClaimsHistoryDataGrid"
        Me.ClaimsHistoryDataGrid.OldSelectedRow = Nothing
        Me.ClaimsHistoryDataGrid.ReadOnly = True
        Me.ClaimsHistoryDataGrid.RowHeadersVisible = False
        Me.ClaimsHistoryDataGrid.SetRowOnRightClick = True
        Me.ClaimsHistoryDataGrid.ShiftPressed = False
        Me.ClaimsHistoryDataGrid.SingleClickBooleanColumns = True
        Me.ClaimsHistoryDataGrid.Size = New System.Drawing.Size(268, 213)
        Me.ClaimsHistoryDataGrid.StyleName = ""
        Me.ClaimsHistoryDataGrid.SubKey = ""
        Me.ClaimsHistoryDataGrid.SuppressTriangle = False
        Me.ClaimsHistoryDataGrid.TabIndex = 0
        '
        'ClaimsHistoryDataGridCustomContextMenu
        '
        Me.ClaimsHistoryDataGridCustomContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DisplayMenuItem, Me.MenuItem1})
        Me.ClaimsHistoryDataGridCustomContextMenu.Name = "DataGridCustomContextMenu"
        Me.ClaimsHistoryDataGridCustomContextMenu.Size = New System.Drawing.Size(155, 32)
        '
        'DisplayMenuItem
        '
        Me.DisplayMenuItem.Name = "DisplayMenuItem"
        Me.DisplayMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
        Me.DisplayMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.DisplayMenuItem.Text = "&Display"
        '
        'MenuItem1
        '
        Me.MenuItem1.Name = "MenuItem1"
        Me.MenuItem1.Size = New System.Drawing.Size(151, 6)
        '
        'ExitButton
        '
        Me.ExitButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ExitButton.Location = New System.Drawing.Point(205, 231)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(75, 23)
        Me.ExitButton.TabIndex = 2
        Me.ExitButton.Text = "E&xit"
        Me.ExitButton.UseVisualStyleBackColor = True
        '
        'ClaimHistory
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ExitButton
        Me.ClientSize = New System.Drawing.Size(292, 266)
        Me.Controls.Add(Me.ExitButton)
        Me.Controls.Add(Me.ClaimsHistoryDataGrid)
        Me.Name = "ClaimHistory"
        Me.Text = "Claim History"
        CType(Me.ClaimsHistoryDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ClaimsHistoryDataGridCustomContextMenu.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ClaimsHistoryDataGrid As DataGridCustom
    Friend WithEvents ExitButton As System.Windows.Forms.Button
    Friend WithEvents ClaimsHistoryDataGridCustomContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents DisplayMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItem1 As System.Windows.Forms.ToolStripSeparator
End Class
