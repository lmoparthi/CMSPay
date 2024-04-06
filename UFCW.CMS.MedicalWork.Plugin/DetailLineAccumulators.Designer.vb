<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DetailLineAccumulators
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
        Me.CancelActionButton = New System.Windows.Forms.Button()
        Me.AddButton = New System.Windows.Forms.Button()
        Me.AccumCustomContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.DeleteMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AccumulatorTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.MainToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.AccumulatorLookupButton = New System.Windows.Forms.Button()
        Me.ClearAllButton = New System.Windows.Forms.Button()
        Me.UpdateAllButton = New System.Windows.Forms.Button()
        Me.UpdateLineButton = New System.Windows.Forms.Button()
        Me.ClearLineButton = New System.Windows.Forms.Button()
        Me.LineAccumulatorsDataGrid = New DataGridCustom()
        Me.AccumCustomContextMenu.SuspendLayout()
        CType(Me.LineAccumulatorsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CancelActionButton
        '
        Me.CancelActionButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CancelActionButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelActionButton.Location = New System.Drawing.Point(8, 273)
        Me.CancelActionButton.Name = "CancelActionButton"
        Me.CancelActionButton.Size = New System.Drawing.Size(75, 23)
        Me.CancelActionButton.TabIndex = 4
        Me.CancelActionButton.Text = "&Cancel"
        Me.MainToolTip.SetToolTip(Me.CancelActionButton, "Cancel the Window")
        '
        'AddButton
        '
        Me.AddButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AddButton.Enabled = False
        Me.AddButton.Location = New System.Drawing.Point(439, 8)
        Me.AddButton.Name = "AddButton"
        Me.AddButton.Size = New System.Drawing.Size(66, 23)
        Me.AddButton.TabIndex = 2
        Me.AddButton.Text = "&Add"
        Me.MainToolTip.SetToolTip(Me.AddButton, "Add Accumulator Code")
        '
        'AccumCustomContextMenu
        '
        Me.AccumCustomContextMenu.ImageScalingSize = New System.Drawing.Size(28, 28)
        Me.AccumCustomContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DeleteMenuItem})
        Me.AccumCustomContextMenu.Name = "AccumContextMenu"
        Me.AccumCustomContextMenu.Size = New System.Drawing.Size(97, 26)
        '
        'DeleteMenuItem
        '
        Me.DeleteMenuItem.Name = "DeleteMenuItem"
        Me.DeleteMenuItem.Size = New System.Drawing.Size(96, 22)
        Me.DeleteMenuItem.Text = "&Delete"
        '
        'AccumulatorTextBox
        '
        Me.AccumulatorTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AccumulatorTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.AccumulatorTextBox.Location = New System.Drawing.Point(104, 8)
        Me.AccumulatorTextBox.Name = "AccumulatorTextBox"
        Me.AccumulatorTextBox.Size = New System.Drawing.Size(291, 20)
        Me.AccumulatorTextBox.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(0, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(104, 16)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Accumulator Code:"
        '
        'AccumulatorLookupButton
        '
        Me.AccumulatorLookupButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AccumulatorLookupButton.Location = New System.Drawing.Point(400, 8)
        Me.AccumulatorLookupButton.Name = "AccumulatorLookupButton"
        Me.AccumulatorLookupButton.Size = New System.Drawing.Size(32, 23)
        Me.AccumulatorLookupButton.TabIndex = 1
        Me.AccumulatorLookupButton.TabStop = False
        Me.AccumulatorLookupButton.Text = "?"
        Me.MainToolTip.SetToolTip(Me.AccumulatorLookupButton, "Find Valid Accumulator Codes")
        '
        'ClearAllButton
        '
        Me.ClearAllButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ClearAllButton.Location = New System.Drawing.Point(164, 273)
        Me.ClearAllButton.Name = "ClearAllButton"
        Me.ClearAllButton.Size = New System.Drawing.Size(75, 23)
        Me.ClearAllButton.TabIndex = 15
        Me.ClearAllButton.Text = "Cl&ear All"
        Me.MainToolTip.SetToolTip(Me.ClearAllButton, "Clear Accumulators for ALL Detail Lines")
        '
        'UpdateAllButton
        '
        Me.UpdateAllButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UpdateAllButton.Location = New System.Drawing.Point(347, 273)
        Me.UpdateAllButton.Name = "UpdateAllButton"
        Me.UpdateAllButton.Size = New System.Drawing.Size(75, 23)
        Me.UpdateAllButton.TabIndex = 14
        Me.UpdateAllButton.Text = "&Update All"
        Me.MainToolTip.SetToolTip(Me.UpdateAllButton, "Update Modified Accumulator(s) for ALL Detail Lines")
        '
        'UpdateLineButton
        '
        Me.UpdateLineButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UpdateLineButton.Location = New System.Drawing.Point(429, 273)
        Me.UpdateLineButton.Name = "UpdateLineButton"
        Me.UpdateLineButton.Size = New System.Drawing.Size(75, 23)
        Me.UpdateLineButton.TabIndex = 13
        Me.UpdateLineButton.Text = "Update &Line"
        Me.MainToolTip.SetToolTip(Me.UpdateLineButton, "Update Modified Accumulator(s) for Detail Line")
        '
        'ClearLineButton
        '
        Me.ClearLineButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ClearLineButton.Location = New System.Drawing.Point(245, 273)
        Me.ClearLineButton.Name = "ClearLineButton"
        Me.ClearLineButton.Size = New System.Drawing.Size(75, 23)
        Me.ClearLineButton.TabIndex = 16
        Me.ClearLineButton.Text = "Cl&ear Line"
        Me.MainToolTip.SetToolTip(Me.ClearLineButton, "Clear Accumulators for Detail Line")
        '
        'LineAccumulatorsDataGrid
        '
        Me.LineAccumulatorsDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.LineAccumulatorsDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.LineAccumulatorsDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LineAccumulatorsDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LineAccumulatorsDataGrid.ADGroupsThatCanFind = ""
        Me.LineAccumulatorsDataGrid.ADGroupsThatCanMultiSort = ""
        Me.LineAccumulatorsDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LineAccumulatorsDataGrid.AllowAutoSize = True
        Me.LineAccumulatorsDataGrid.AllowColumnReorder = False
        Me.LineAccumulatorsDataGrid.AllowCopy = False
        Me.LineAccumulatorsDataGrid.AllowCustomize = True
        Me.LineAccumulatorsDataGrid.AllowDelete = True
        Me.LineAccumulatorsDataGrid.AllowDragDrop = False
        Me.LineAccumulatorsDataGrid.AllowEdit = True
        Me.LineAccumulatorsDataGrid.AllowExport = False
        Me.LineAccumulatorsDataGrid.AllowFilter = False
        Me.LineAccumulatorsDataGrid.AllowFind = False
        Me.LineAccumulatorsDataGrid.AllowGoTo = False
        Me.LineAccumulatorsDataGrid.AllowMultiSelect = True
        Me.LineAccumulatorsDataGrid.AllowMultiSort = False
        Me.LineAccumulatorsDataGrid.AllowNavigation = False
        Me.LineAccumulatorsDataGrid.AllowNew = False
        Me.LineAccumulatorsDataGrid.AllowPrint = False
        Me.LineAccumulatorsDataGrid.AllowRefresh = False
        Me.LineAccumulatorsDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LineAccumulatorsDataGrid.AppKey = "UFCW\Claims\"
        Me.LineAccumulatorsDataGrid.AutoSaveCols = True
        Me.LineAccumulatorsDataGrid.BackgroundColor = System.Drawing.SystemColors.Control
        Me.LineAccumulatorsDataGrid.CaptionVisible = False
        Me.LineAccumulatorsDataGrid.ColumnHeaderLabel = Nothing
        Me.LineAccumulatorsDataGrid.ColumnRePositioning = False
        Me.LineAccumulatorsDataGrid.ColumnResizing = False
        Me.LineAccumulatorsDataGrid.ConfirmDelete = True
        Me.LineAccumulatorsDataGrid.CopySelectedOnly = True
        Me.LineAccumulatorsDataGrid.CurrentBSPosition = -1
        Me.LineAccumulatorsDataGrid.DataMember = ""
        Me.LineAccumulatorsDataGrid.DragColumn = 0
        Me.LineAccumulatorsDataGrid.ExportSelectedOnly = True
        Me.LineAccumulatorsDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.LineAccumulatorsDataGrid.HighlightedRow = Nothing
        Me.LineAccumulatorsDataGrid.HighLightModifiedRows = False
        Me.LineAccumulatorsDataGrid.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.LineAccumulatorsDataGrid.IsMouseDown = False
        Me.LineAccumulatorsDataGrid.LastGoToLine = ""
        Me.LineAccumulatorsDataGrid.Location = New System.Drawing.Point(3, 34)
        Me.LineAccumulatorsDataGrid.MultiSort = False
        Me.LineAccumulatorsDataGrid.Name = "LineAccumulatorsDataGrid"
        Me.LineAccumulatorsDataGrid.OldSelectedRow = Nothing
        Me.LineAccumulatorsDataGrid.PreviousBSPosition = -1
        Me.LineAccumulatorsDataGrid.RetainRowSelectionAfterSort = True
        Me.LineAccumulatorsDataGrid.SetRowOnRightClick = True
        Me.LineAccumulatorsDataGrid.ShiftPressed = False
        Me.LineAccumulatorsDataGrid.SingleClickBooleanColumns = True
        Me.LineAccumulatorsDataGrid.Size = New System.Drawing.Size(501, 230)
        Me.LineAccumulatorsDataGrid.Sort = Nothing
        Me.LineAccumulatorsDataGrid.StyleName = ""
        Me.LineAccumulatorsDataGrid.SubKey = ""
        Me.LineAccumulatorsDataGrid.SuppressMouseDown = False
        Me.LineAccumulatorsDataGrid.SuppressTriangle = False
        Me.LineAccumulatorsDataGrid.TabIndex = 3
        '
        'DetailLineAccumulators
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.CancelActionButton
        Me.ClientSize = New System.Drawing.Size(512, 314)
        Me.Controls.Add(Me.LineAccumulatorsDataGrid)
        Me.Controls.Add(Me.ClearLineButton)
        Me.Controls.Add(Me.UpdateAllButton)
        Me.Controls.Add(Me.ClearAllButton)
        Me.Controls.Add(Me.AccumulatorLookupButton)
        Me.Controls.Add(Me.UpdateLineButton)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.AccumulatorTextBox)
        Me.Controls.Add(Me.AddButton)
        Me.Controls.Add(Me.CancelActionButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Location = New System.Drawing.Point(80, 80)
        Me.MinimumSize = New System.Drawing.Size(528, 320)
        Me.Name = "DetailLineAccumulators"
        Me.ShowInTaskbar = False
        Me.Text = "Line 0 Accumulators"
        Me.AccumCustomContextMenu.ResumeLayout(False)
        CType(Me.LineAccumulatorsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents CancelActionButton As System.Windows.Forms.Button
    Friend WithEvents AddButton As System.Windows.Forms.Button
    Friend WithEvents AccumCustomContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents DeleteMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents AccumulatorTextBox As System.Windows.Forms.TextBox
    Friend WithEvents MainToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents AccumulatorLookupButton As System.Windows.Forms.Button
    Friend WithEvents ClearAllButton As System.Windows.Forms.Button
    Friend WithEvents UpdateAllButton As System.Windows.Forms.Button
    Friend WithEvents UpdateLineButton As System.Windows.Forms.Button
    Friend WithEvents ClearLineButton As System.Windows.Forms.Button
    Friend WithEvents LineAccumulatorsDataGrid As DataGridCustom
End Class
