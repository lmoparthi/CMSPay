<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DetailLineReasons
    Inherits System.Windows.Forms.Form

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DetailLineReasons))
        Me.AddActionButton = New System.Windows.Forms.Button()
        Me.CancelActionButton = New System.Windows.Forms.Button()
        Me.SaveActionButton = New System.Windows.Forms.Button()
        Me.LineReasonsDataGrid = New DataGridCustom()
        Me.ReasonCodesTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SortDownButton = New System.Windows.Forms.Button()
        Me.SortUpButton = New System.Windows.Forms.Button()
        Me.DeleteActionButton = New System.Windows.Forms.Button()
        Me.MainToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.ReasonLookupButton = New System.Windows.Forms.Button()
        Me.updateAllButton = New System.Windows.Forms.Button()
        Me.ClearAllButton = New System.Windows.Forms.Button()
        Me.ClearLineButton = New System.Windows.Forms.Button()
        Me.ReasonCustomContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.DeleteMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.LineReasonsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ReasonCustomContextMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'AddActionButton
        '
        Me.AddActionButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AddActionButton.Enabled = False
        Me.AddActionButton.Location = New System.Drawing.Point(440, 8)
        Me.AddActionButton.Name = "AddActionButton"
        Me.AddActionButton.Size = New System.Drawing.Size(75, 23)
        Me.AddActionButton.TabIndex = 1
        Me.AddActionButton.Text = "&Add"
        Me.MainToolTip.SetToolTip(Me.AddActionButton, "Add Reason Code")
        '
        'CancelActionButton
        '
        Me.CancelActionButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CancelActionButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelActionButton.Location = New System.Drawing.Point(8, 268)
        Me.CancelActionButton.Name = "CancelActionButton"
        Me.CancelActionButton.Size = New System.Drawing.Size(75, 23)
        Me.CancelActionButton.TabIndex = 6
        Me.CancelActionButton.Text = "&Cancel"
        Me.MainToolTip.SetToolTip(Me.CancelActionButton, "Cancel Changes")
        '
        'SaveActionButton
        '
        Me.SaveActionButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SaveActionButton.Location = New System.Drawing.Point(440, 268)
        Me.SaveActionButton.Name = "SaveActionButton"
        Me.SaveActionButton.Size = New System.Drawing.Size(75, 23)
        Me.SaveActionButton.TabIndex = 7
        Me.SaveActionButton.Text = "Update &Line"
        Me.MainToolTip.SetToolTip(Me.SaveActionButton, "Update Selected Line")
        '
        'LineReasonsDataGrid
        '
        Me.LineReasonsDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.LineReasonsDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.LineReasonsDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LineReasonsDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LineReasonsDataGrid.ADGroupsThatCanFind = ""
        Me.LineReasonsDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LineReasonsDataGrid.ADGroupsThatCanMultiSort = ""
        Me.LineReasonsDataGrid.AllowAutoSize = False
        Me.LineReasonsDataGrid.AllowColumnReorder = False
        Me.LineReasonsDataGrid.AllowCopy = False
        Me.LineReasonsDataGrid.AllowCustomize = False
        Me.LineReasonsDataGrid.AllowDelete = True
        Me.LineReasonsDataGrid.AllowDragDrop = False
        Me.LineReasonsDataGrid.AllowEdit = False
        Me.LineReasonsDataGrid.AllowExport = False
        Me.LineReasonsDataGrid.AllowFilter = False
        Me.LineReasonsDataGrid.AllowFind = False
        Me.LineReasonsDataGrid.AllowGoTo = False
        Me.LineReasonsDataGrid.AllowMultiSelect = True
        Me.LineReasonsDataGrid.AllowMultiSort = False
        Me.LineReasonsDataGrid.AllowNavigation = False
        Me.LineReasonsDataGrid.AllowNew = False
        Me.LineReasonsDataGrid.AllowPrint = False
        Me.LineReasonsDataGrid.AllowRefresh = False
        Me.LineReasonsDataGrid.AllowSorting = False
        Me.LineReasonsDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LineReasonsDataGrid.AppKey = "UFCW\Claims\"
        Me.LineReasonsDataGrid.BackgroundColor = System.Drawing.SystemColors.Control
        Me.LineReasonsDataGrid.CaptionVisible = False
        Me.LineReasonsDataGrid.ColumnHeaderLabel = Nothing
        Me.LineReasonsDataGrid.ColumnRePositioning = False
        Me.LineReasonsDataGrid.ColumnResizing = False
        Me.LineReasonsDataGrid.ConfirmDelete = True
        Me.LineReasonsDataGrid.CopySelectedOnly = True
        Me.LineReasonsDataGrid.DataMember = ""
        Me.LineReasonsDataGrid.DragColumn = 0
        Me.LineReasonsDataGrid.ExportSelectedOnly = True
        Me.LineReasonsDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.LineReasonsDataGrid.HighlightedRow = Nothing
        Me.LineReasonsDataGrid.IsMouseDown = False
        Me.LineReasonsDataGrid.LastGoToLine = ""
        Me.LineReasonsDataGrid.Location = New System.Drawing.Point(8, 40)
        Me.LineReasonsDataGrid.MultiSort = False
        Me.LineReasonsDataGrid.Name = "LineReasonsDataGrid"
        Me.LineReasonsDataGrid.OldSelectedRow = Nothing
        Me.LineReasonsDataGrid.ReadOnly = True
        Me.LineReasonsDataGrid.SetRowOnRightClick = True
        Me.LineReasonsDataGrid.ShiftPressed = False
        Me.LineReasonsDataGrid.SingleClickBooleanColumns = True
        Me.LineReasonsDataGrid.Size = New System.Drawing.Size(472, 224)
        Me.LineReasonsDataGrid.StyleName = ""
        Me.LineReasonsDataGrid.SubKey = ""
        Me.LineReasonsDataGrid.SuppressTriangle = False
        Me.LineReasonsDataGrid.TabIndex = 0
        '
        'ReasonCodesTextBox
        '
        Me.ReasonCodesTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ReasonCodesTextBox.Location = New System.Drawing.Point(88, 8)
        Me.ReasonCodesTextBox.Name = "ReasonCodesTextBox"
        Me.ReasonCodesTextBox.Size = New System.Drawing.Size(298, 20)
        Me.ReasonCodesTextBox.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Reason Code:"
        '
        'SortDownButton
        '
        Me.SortDownButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SortDownButton.Enabled = False
        Me.SortDownButton.Image = CType(resources.GetObject("SortDownButton.Image"), System.Drawing.Image)
        Me.SortDownButton.Location = New System.Drawing.Point(484, 136)
        Me.SortDownButton.Name = "SortDownButton"
        Me.SortDownButton.Size = New System.Drawing.Size(32, 30)
        Me.SortDownButton.TabIndex = 5
        Me.MainToolTip.SetToolTip(Me.SortDownButton, "Sort Item(s) Down")
        '
        'SortUpButton
        '
        Me.SortUpButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SortUpButton.Enabled = False
        Me.SortUpButton.Image = CType(resources.GetObject("SortUpButton.Image"), System.Drawing.Image)
        Me.SortUpButton.Location = New System.Drawing.Point(484, 96)
        Me.SortUpButton.Name = "SortUpButton"
        Me.SortUpButton.Size = New System.Drawing.Size(32, 30)
        Me.SortUpButton.TabIndex = 4
        Me.MainToolTip.SetToolTip(Me.SortUpButton, "Sort Item(s) Up")
        '
        'DeleteActionButton
        '
        Me.DeleteActionButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DeleteActionButton.Enabled = False
        Me.DeleteActionButton.Image = CType(resources.GetObject("DeleteActionButton.Image"), System.Drawing.Image)
        Me.DeleteActionButton.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.DeleteActionButton.Location = New System.Drawing.Point(484, 40)
        Me.DeleteActionButton.Name = "DeleteActionButton"
        Me.DeleteActionButton.Size = New System.Drawing.Size(32, 30)
        Me.DeleteActionButton.TabIndex = 3
        Me.MainToolTip.SetToolTip(Me.DeleteActionButton, "Remove Reason Code(s)")
        '
        'ReasonLookupButton
        '
        Me.ReasonLookupButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ReasonLookupButton.Location = New System.Drawing.Point(392, 8)
        Me.ReasonLookupButton.Name = "ReasonLookupButton"
        Me.ReasonLookupButton.Size = New System.Drawing.Size(32, 23)
        Me.ReasonLookupButton.TabIndex = 10
        Me.ReasonLookupButton.TabStop = False
        Me.ReasonLookupButton.Text = "?"
        Me.MainToolTip.SetToolTip(Me.ReasonLookupButton, "Find Valid Reason Codes")
        '
        'updateAllButton
        '
        Me.updateAllButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.updateAllButton.Location = New System.Drawing.Point(359, 268)
        Me.updateAllButton.Name = "updateAllButton"
        Me.updateAllButton.Size = New System.Drawing.Size(75, 23)
        Me.updateAllButton.TabIndex = 11
        Me.updateAllButton.Text = "&Update All"
        Me.MainToolTip.SetToolTip(Me.updateAllButton, "Update all Lines")
        '
        'ClearAllButton
        '
        Me.ClearAllButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ClearAllButton.Location = New System.Drawing.Point(123, 268)
        Me.ClearAllButton.Name = "ClearAllButton"
        Me.ClearAllButton.Size = New System.Drawing.Size(75, 23)
        Me.ClearAllButton.TabIndex = 12
        Me.ClearAllButton.Text = "Cl&ear All"
        Me.MainToolTip.SetToolTip(Me.ClearAllButton, "Clear All Lines")
        '
        'ClearLineButton
        '
        Me.ClearLineButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ClearLineButton.Location = New System.Drawing.Point(204, 268)
        Me.ClearLineButton.Name = "ClearLineButton"
        Me.ClearLineButton.Size = New System.Drawing.Size(75, 23)
        Me.ClearLineButton.TabIndex = 13
        Me.ClearLineButton.Text = "Clear Line"
        Me.MainToolTip.SetToolTip(Me.ClearLineButton, "Clear Reasons from Line")
        '
        'ReasonCustomContextMenu
        '
        Me.ReasonCustomContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DeleteMenuItem})
        Me.ReasonCustomContextMenu.Name = "AccumContextMenu"
        Me.ReasonCustomContextMenu.Size = New System.Drawing.Size(97, 26)
        '
        'DeleteMenuItem
        '
        Me.DeleteMenuItem.Name = "DeleteMenuItem"
        Me.DeleteMenuItem.Size = New System.Drawing.Size(96, 22)
        Me.DeleteMenuItem.Text = "&Delete"
        '
        'DetailLineReasons
        '
        Me.AcceptButton = Me.SaveActionButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.CancelActionButton
        Me.ClientSize = New System.Drawing.Size(520, 294)
        Me.Controls.Add(Me.ClearLineButton)
        Me.Controls.Add(Me.LineReasonsDataGrid)
        Me.Controls.Add(Me.ClearAllButton)
        Me.Controls.Add(Me.updateAllButton)
        Me.Controls.Add(Me.ReasonLookupButton)
        Me.Controls.Add(Me.DeleteActionButton)
        Me.Controls.Add(Me.SortDownButton)
        Me.Controls.Add(Me.SortUpButton)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ReasonCodesTextBox)
        Me.Controls.Add(Me.AddActionButton)
        Me.Controls.Add(Me.CancelActionButton)
        Me.Controls.Add(Me.SaveActionButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MinimumSize = New System.Drawing.Size(528, 320)
        Me.Name = "DetailLineReasons"
        Me.ShowInTaskbar = False
        Me.Text = "Line 0 Reasons"
        CType(Me.LineReasonsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ReasonCustomContextMenu.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents AddActionButton As System.Windows.Forms.Button
    Friend WithEvents CancelActionButton As System.Windows.Forms.Button
    Friend WithEvents SaveActionButton As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents SortDownButton As System.Windows.Forms.Button
    Friend WithEvents SortUpButton As System.Windows.Forms.Button
    Friend WithEvents LineReasonsDataGrid As DataGridCustom
    Friend WithEvents DeleteActionButton As System.Windows.Forms.Button
    Friend WithEvents MainToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents ReasonCodesTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ReasonLookupButton As System.Windows.Forms.Button
    Friend WithEvents updateAllButton As System.Windows.Forms.Button
    Friend WithEvents ClearAllButton As System.Windows.Forms.Button
    Friend WithEvents ReasonCustomContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents DeleteMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ClearLineButton As System.Windows.Forms.Button

End Class