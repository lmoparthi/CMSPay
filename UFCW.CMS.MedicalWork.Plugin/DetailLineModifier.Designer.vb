<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DetailLineModifierForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DetailLineModifierForm))
        Me.AddButton = New System.Windows.Forms.Button()
        Me.CancelActionButton = New System.Windows.Forms.Button()
        Me.LineModifiersDataGrid = New DataGridCustom()
        Me.ModifierCodesTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SortDownButton = New System.Windows.Forms.Button()
        Me.SortUpButton = New System.Windows.Forms.Button()
        Me.DeleteButton = New System.Windows.Forms.Button()
        Me.MainToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.ModifierLookupButton = New System.Windows.Forms.Button()
        Me.ClearAllButton = New System.Windows.Forms.Button()
        Me.UpdateAllButton = New System.Windows.Forms.Button()
        Me.UpdateLineButton = New System.Windows.Forms.Button()
        Me.ClearLineButton = New System.Windows.Forms.Button()
        Me.ModifierCustomContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.DeleteMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.LineModifiersDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ModifierCustomContextMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'AddButton
        '
        Me.AddButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AddButton.Enabled = False
        Me.AddButton.Location = New System.Drawing.Point(440, 8)
        Me.AddButton.Name = "AddButton"
        Me.AddButton.Size = New System.Drawing.Size(75, 23)
        Me.AddButton.TabIndex = 1
        Me.AddButton.Text = "&Add"
        Me.MainToolTip.SetToolTip(Me.AddButton, "Add Modifier Code")
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
        'LineModifiersDataGrid
        '
        Me.LineModifiersDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.LineModifiersDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.LineModifiersDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LineModifiersDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LineModifiersDataGrid.ADGroupsThatCanFind = ""
        Me.LineModifiersDataGrid.ADGroupsThatCanMultiSort = ""
        Me.LineModifiersDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LineModifiersDataGrid.AllowAutoSize = False
        Me.LineModifiersDataGrid.AllowColumnReorder = False
        Me.LineModifiersDataGrid.AllowCopy = False
        Me.LineModifiersDataGrid.AllowCustomize = False
        Me.LineModifiersDataGrid.AllowDelete = False
        Me.LineModifiersDataGrid.AllowDragDrop = False
        Me.LineModifiersDataGrid.AllowEdit = False
        Me.LineModifiersDataGrid.AllowExport = False
        Me.LineModifiersDataGrid.AllowFilter = False
        Me.LineModifiersDataGrid.AllowFind = False
        Me.LineModifiersDataGrid.AllowGoTo = False
        Me.LineModifiersDataGrid.AllowMultiSelect = False
        Me.LineModifiersDataGrid.AllowMultiSort = False
        Me.LineModifiersDataGrid.AllowNavigation = False
        Me.LineModifiersDataGrid.AllowNew = False
        Me.LineModifiersDataGrid.AllowPrint = False
        Me.LineModifiersDataGrid.AllowRefresh = False
        Me.LineModifiersDataGrid.AllowSorting = False
        Me.LineModifiersDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LineModifiersDataGrid.AppKey = "UFCW\Claims\"
        Me.LineModifiersDataGrid.AutoSaveCols = True
        Me.LineModifiersDataGrid.BackgroundColor = System.Drawing.SystemColors.Control
        Me.LineModifiersDataGrid.CaptionVisible = False
        Me.LineModifiersDataGrid.ColumnHeaderLabel = Nothing
        Me.LineModifiersDataGrid.ColumnRePositioning = False
        Me.LineModifiersDataGrid.ColumnResizing = False
        Me.LineModifiersDataGrid.ConfirmDelete = True
        Me.LineModifiersDataGrid.CopySelectedOnly = True
        Me.LineModifiersDataGrid.CurrentBSPosition = -1
        Me.LineModifiersDataGrid.DataMember = ""
        Me.LineModifiersDataGrid.DragColumn = 0
        Me.LineModifiersDataGrid.ExportSelectedOnly = True
        Me.LineModifiersDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.LineModifiersDataGrid.HighlightedRow = Nothing
        Me.LineModifiersDataGrid.HighLightModifiedRows = False
        Me.LineModifiersDataGrid.IsMouseDown = False
        Me.LineModifiersDataGrid.LastGoToLine = ""
        Me.LineModifiersDataGrid.Location = New System.Drawing.Point(8, 40)
        Me.LineModifiersDataGrid.MultiSort = False
        Me.LineModifiersDataGrid.Name = "LineModifiersDataGrid"
        Me.LineModifiersDataGrid.OldSelectedRow = Nothing
        Me.LineModifiersDataGrid.PreviousBSPosition = -1
        Me.LineModifiersDataGrid.ReadOnly = True
        Me.LineModifiersDataGrid.RetainRowSelectionAfterSort = True
        Me.LineModifiersDataGrid.SetRowOnRightClick = True
        Me.LineModifiersDataGrid.ShiftPressed = False
        Me.LineModifiersDataGrid.SingleClickBooleanColumns = True
        Me.LineModifiersDataGrid.Size = New System.Drawing.Size(472, 224)
        Me.LineModifiersDataGrid.Sort = Nothing
        Me.LineModifiersDataGrid.StyleName = ""
        Me.LineModifiersDataGrid.SubKey = ""
        Me.LineModifiersDataGrid.SuppressMouseDown = False
        Me.LineModifiersDataGrid.SuppressTriangle = False
        Me.LineModifiersDataGrid.TabIndex = 2
        '
        'ModifierCodesTextBox
        '
        Me.ModifierCodesTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ModifierCodesTextBox.Location = New System.Drawing.Point(96, 8)
        Me.ModifierCodesTextBox.Name = "ModifierCodesTextBox"
        Me.ModifierCodesTextBox.Size = New System.Drawing.Size(296, 20)
        Me.ModifierCodesTextBox.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Modifier Code:"
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
        'DeleteButton
        '
        Me.DeleteButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DeleteButton.Enabled = False
        Me.DeleteButton.Image = CType(resources.GetObject("DeleteButton.Image"), System.Drawing.Image)
        Me.DeleteButton.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.DeleteButton.Location = New System.Drawing.Point(484, 40)
        Me.DeleteButton.Name = "DeleteButton"
        Me.DeleteButton.Size = New System.Drawing.Size(32, 30)
        Me.DeleteButton.TabIndex = 3
        Me.MainToolTip.SetToolTip(Me.DeleteButton, "Remove Modifier Code(s)")
        '
        'ModifierLookupButton
        '
        Me.ModifierLookupButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ModifierLookupButton.Location = New System.Drawing.Point(394, 8)
        Me.ModifierLookupButton.Name = "ModifierLookupButton"
        Me.ModifierLookupButton.Size = New System.Drawing.Size(32, 23)
        Me.ModifierLookupButton.TabIndex = 10
        Me.ModifierLookupButton.TabStop = False
        Me.ModifierLookupButton.Text = "?"
        Me.MainToolTip.SetToolTip(Me.ModifierLookupButton, "Find Valid Modifier Codes")
        '
        'ClearAllButton
        '
        Me.ClearAllButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ClearAllButton.Location = New System.Drawing.Point(123, 268)
        Me.ClearAllButton.Name = "ClearAllButton"
        Me.ClearAllButton.Size = New System.Drawing.Size(75, 23)
        Me.ClearAllButton.TabIndex = 13
        Me.ClearAllButton.Text = "Cl&ear All"
        Me.MainToolTip.SetToolTip(Me.ClearAllButton, "Clear All Lines")
        '
        'UpdateAllButton
        '
        Me.UpdateAllButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UpdateAllButton.Location = New System.Drawing.Point(359, 268)
        Me.UpdateAllButton.Name = "UpdateAllButton"
        Me.UpdateAllButton.Size = New System.Drawing.Size(75, 23)
        Me.UpdateAllButton.TabIndex = 15
        Me.UpdateAllButton.Text = "&Update All"
        Me.MainToolTip.SetToolTip(Me.UpdateAllButton, "Update all Lines")
        '
        'UpdateLineButton
        '
        Me.UpdateLineButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UpdateLineButton.Location = New System.Drawing.Point(440, 268)
        Me.UpdateLineButton.Name = "UpdateLineButton"
        Me.UpdateLineButton.Size = New System.Drawing.Size(75, 23)
        Me.UpdateLineButton.TabIndex = 14
        Me.UpdateLineButton.Text = "Update &Line"
        Me.MainToolTip.SetToolTip(Me.UpdateLineButton, "Update Selected Line")
        '
        'ClearLineButton
        '
        Me.ClearLineButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ClearLineButton.Location = New System.Drawing.Point(204, 268)
        Me.ClearLineButton.Name = "ClearLineButton"
        Me.ClearLineButton.Size = New System.Drawing.Size(75, 23)
        Me.ClearLineButton.TabIndex = 16
        Me.ClearLineButton.Text = "Clear Line"
        Me.MainToolTip.SetToolTip(Me.ClearLineButton, "Clear All Modifiers from Line")
        '
        'ModifierCustomContextMenu
        '
        Me.ModifierCustomContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DeleteMenuItem})
        Me.ModifierCustomContextMenu.Name = "AccumContextMenu"
        Me.ModifierCustomContextMenu.Size = New System.Drawing.Size(97, 26)
        '
        'DeleteMenuItem
        '
        Me.DeleteMenuItem.Name = "DeleteMenuItem"
        Me.DeleteMenuItem.Size = New System.Drawing.Size(96, 22)
        Me.DeleteMenuItem.Text = "&Delete"
        '
        'DetailLineModifier
        '
        Me.AcceptButton = Me.UpdateLineButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.CancelActionButton
        Me.ClientSize = New System.Drawing.Size(520, 294)
        Me.Controls.Add(Me.ClearLineButton)
        Me.Controls.Add(Me.ClearAllButton)
        Me.Controls.Add(Me.UpdateLineButton)
        Me.Controls.Add(Me.UpdateAllButton)
        Me.Controls.Add(Me.ModifierLookupButton)
        Me.Controls.Add(Me.DeleteButton)
        Me.Controls.Add(Me.SortDownButton)
        Me.Controls.Add(Me.SortUpButton)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ModifierCodesTextBox)
        Me.Controls.Add(Me.AddButton)
        Me.Controls.Add(Me.CancelActionButton)
        Me.Controls.Add(Me.LineModifiersDataGrid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MinimumSize = New System.Drawing.Size(528, 320)
        Me.Name = "DetailLineModifier"
        Me.ShowInTaskbar = False
        Me.Text = "Line 0 Modifier"
        CType(Me.LineModifiersDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ModifierCustomContextMenu.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ModifierCustomContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents DeleteMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ClearLineButton As System.Windows.Forms.Button
    Friend WithEvents AddButton As System.Windows.Forms.Button
    Friend WithEvents CancelActionButton As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents SortDownButton As System.Windows.Forms.Button
    Friend WithEvents SortUpButton As System.Windows.Forms.Button
    Friend WithEvents LineModifiersDataGrid As DataGridCustom
    Friend WithEvents DeleteButton As System.Windows.Forms.Button
    Friend WithEvents MainToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents ModifierCodesTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ModifierLookupButton As System.Windows.Forms.Button
    Friend WithEvents ClearAllButton As System.Windows.Forms.Button
    Friend WithEvents UpdateAllButton As System.Windows.Forms.Button
    Friend WithEvents UpdateLineButton As System.Windows.Forms.Button

End Class
