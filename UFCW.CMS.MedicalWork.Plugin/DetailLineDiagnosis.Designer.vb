<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DetailLineDiagnosisForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DetailLineDiagnosisForm))
        Me.AddButton = New System.Windows.Forms.Button()
        Me.CancelActionButton = New System.Windows.Forms.Button()
        Me.LineDiagnosisDataGrid = New DataGridCustom()
        Me.DiagnosisCodesTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SortDownButton = New System.Windows.Forms.Button()
        Me.SortUpButton = New System.Windows.Forms.Button()
        Me.DeleteButton = New System.Windows.Forms.Button()
        Me.MainToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.DiagnosisLookupButton = New System.Windows.Forms.Button()
        Me.ClearAllButton = New System.Windows.Forms.Button()
        Me.UpdateAllButton = New System.Windows.Forms.Button()
        Me.UpdateLineButton = New System.Windows.Forms.Button()
        Me.ClearLineButton = New System.Windows.Forms.Button()
        Me.DiagnosisCustomContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.LineDiagnosisDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DiagnosisCustomContextMenu.SuspendLayout()
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
        Me.MainToolTip.SetToolTip(Me.AddButton, "Add Diagnosis Code")
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
        'LineDiagnosisDataGrid
        '
        Me.LineDiagnosisDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.LineDiagnosisDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.LineDiagnosisDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LineDiagnosisDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LineDiagnosisDataGrid.ADGroupsThatCanFind = ""
        Me.LineDiagnosisDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LineDiagnosisDataGrid.ADGroupsThatCanMultiSort = ""
        Me.LineDiagnosisDataGrid.AllowAutoSize = False
        Me.LineDiagnosisDataGrid.AllowColumnReorder = False
        Me.LineDiagnosisDataGrid.AllowCopy = False
        Me.LineDiagnosisDataGrid.AllowCustomize = False
        Me.LineDiagnosisDataGrid.AllowDelete = True
        Me.LineDiagnosisDataGrid.AllowDragDrop = False
        Me.LineDiagnosisDataGrid.AllowEdit = False
        Me.LineDiagnosisDataGrid.AllowExport = False
        Me.LineDiagnosisDataGrid.AllowFilter = False
        Me.LineDiagnosisDataGrid.AllowFind = False
        Me.LineDiagnosisDataGrid.AllowGoTo = False
        Me.LineDiagnosisDataGrid.AllowMultiSelect = True
        Me.LineDiagnosisDataGrid.AllowMultiSort = False
        Me.LineDiagnosisDataGrid.AllowNavigation = False
        Me.LineDiagnosisDataGrid.AllowNew = False
        Me.LineDiagnosisDataGrid.AllowPrint = False
        Me.LineDiagnosisDataGrid.AllowRefresh = False
        Me.LineDiagnosisDataGrid.AllowSorting = False
        Me.LineDiagnosisDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LineDiagnosisDataGrid.AppKey = "UFCW\Claims\"
        Me.LineDiagnosisDataGrid.BackgroundColor = System.Drawing.SystemColors.Control
        Me.LineDiagnosisDataGrid.CaptionVisible = False
        Me.LineDiagnosisDataGrid.ColumnHeaderLabel = Nothing
        Me.LineDiagnosisDataGrid.ColumnRePositioning = False
        Me.LineDiagnosisDataGrid.ColumnResizing = False
        Me.LineDiagnosisDataGrid.ConfirmDelete = True
        Me.LineDiagnosisDataGrid.CopySelectedOnly = True
        Me.LineDiagnosisDataGrid.DataMember = ""
        Me.LineDiagnosisDataGrid.DragColumn = 0
        Me.LineDiagnosisDataGrid.ExportSelectedOnly = True
        Me.LineDiagnosisDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.LineDiagnosisDataGrid.HighlightedRow = Nothing
        Me.LineDiagnosisDataGrid.IsMouseDown = False
        Me.LineDiagnosisDataGrid.LastGoToLine = ""
        Me.LineDiagnosisDataGrid.Location = New System.Drawing.Point(8, 40)
        Me.LineDiagnosisDataGrid.MultiSort = False
        Me.LineDiagnosisDataGrid.Name = "LineDiagnosisDataGrid"
        Me.LineDiagnosisDataGrid.OldSelectedRow = Nothing
        Me.LineDiagnosisDataGrid.ReadOnly = True
        Me.LineDiagnosisDataGrid.SetRowOnRightClick = True
        Me.LineDiagnosisDataGrid.ShiftPressed = False
        Me.LineDiagnosisDataGrid.SingleClickBooleanColumns = True
        Me.LineDiagnosisDataGrid.Size = New System.Drawing.Size(472, 224)
        Me.LineDiagnosisDataGrid.StyleName = ""
        Me.LineDiagnosisDataGrid.SubKey = ""
        Me.LineDiagnosisDataGrid.SuppressTriangle = True
        Me.LineDiagnosisDataGrid.TabIndex = 2
        '
        'DiagnosisCodesTextBox
        '
        Me.DiagnosisCodesTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DiagnosisCodesTextBox.Location = New System.Drawing.Point(96, 8)
        Me.DiagnosisCodesTextBox.Name = "DiagnosisCodesTextBox"
        Me.DiagnosisCodesTextBox.Size = New System.Drawing.Size(296, 20)
        Me.DiagnosisCodesTextBox.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(84, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Diagnosis Code:"
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
        Me.MainToolTip.SetToolTip(Me.DeleteButton, "Remove Diagnosis Code(s)")
        '
        'DiagnosisLookupButton
        '
        Me.DiagnosisLookupButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DiagnosisLookupButton.Location = New System.Drawing.Point(396, 8)
        Me.DiagnosisLookupButton.Name = "DiagnosisLookupButton"
        Me.DiagnosisLookupButton.Size = New System.Drawing.Size(32, 23)
        Me.DiagnosisLookupButton.TabIndex = 10
        Me.DiagnosisLookupButton.TabStop = False
        Me.DiagnosisLookupButton.Text = "?"
        Me.MainToolTip.SetToolTip(Me.DiagnosisLookupButton, "Find Valid Diagnosis Codes")
        '
        'ClearAllButton
        '
        Me.ClearAllButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ClearAllButton.Location = New System.Drawing.Point(123, 268)
        Me.ClearAllButton.Name = "ClearAllButton"
        Me.ClearAllButton.Size = New System.Drawing.Size(75, 23)
        Me.ClearAllButton.TabIndex = 15
        Me.ClearAllButton.Text = "Cl&ear All"
        Me.MainToolTip.SetToolTip(Me.ClearAllButton, "Clear Diagnosis from ALL Lines")
        '
        'UpdateAllButton
        '
        Me.UpdateAllButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UpdateAllButton.Location = New System.Drawing.Point(359, 268)
        Me.UpdateAllButton.Name = "UpdateAllButton"
        Me.UpdateAllButton.Size = New System.Drawing.Size(75, 23)
        Me.UpdateAllButton.TabIndex = 14
        Me.UpdateAllButton.Text = "&Update All"
        Me.MainToolTip.SetToolTip(Me.UpdateAllButton, "Update all Lines")
        '
        'UpdateLineButton
        '
        Me.UpdateLineButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UpdateLineButton.Location = New System.Drawing.Point(440, 268)
        Me.UpdateLineButton.Name = "UpdateLineButton"
        Me.UpdateLineButton.Size = New System.Drawing.Size(75, 23)
        Me.UpdateLineButton.TabIndex = 13
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
        Me.ClearLineButton.Text = "Cl&ear Line"
        Me.MainToolTip.SetToolTip(Me.ClearLineButton, "Clear Diagnosis for Detail Line")
        '
        'DiagnosisCustomContextMenu
        '
        Me.DiagnosisCustomContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddMenuItem})
        Me.DiagnosisCustomContextMenu.Name = "AccumContextMenu"
        Me.DiagnosisCustomContextMenu.Size = New System.Drawing.Size(97, 26)
        '
        'AddMenuItem
        '
        Me.AddMenuItem.Name = "AddMenuItem"
        Me.AddMenuItem.Size = New System.Drawing.Size(96, 22)
        Me.AddMenuItem.Text = "&Add"
        '
        'DetailLineDiagnosis
        '
        Me.AcceptButton = Me.UpdateLineButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.CancelActionButton
        Me.ClientSize = New System.Drawing.Size(520, 294)
        Me.Controls.Add(Me.ClearAllButton)
        Me.Controls.Add(Me.ClearLineButton)
        Me.Controls.Add(Me.UpdateAllButton)
        Me.Controls.Add(Me.DiagnosisLookupButton)
        Me.Controls.Add(Me.UpdateLineButton)
        Me.Controls.Add(Me.DeleteButton)
        Me.Controls.Add(Me.SortDownButton)
        Me.Controls.Add(Me.SortUpButton)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DiagnosisCodesTextBox)
        Me.Controls.Add(Me.AddButton)
        Me.Controls.Add(Me.CancelActionButton)
        Me.Controls.Add(Me.LineDiagnosisDataGrid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MinimumSize = New System.Drawing.Size(528, 320)
        Me.Name = "DetailLineDiagnosis"
        Me.ShowInTaskbar = False
        Me.Text = "Line 0 Diagnosis"
        CType(Me.LineDiagnosisDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DiagnosisCustomContextMenu.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DiagnosisCustomContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AddMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ClearLineButton As System.Windows.Forms.Button
    Friend WithEvents AddButton As System.Windows.Forms.Button
    Friend WithEvents CancelActionButton As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents SortDownButton As System.Windows.Forms.Button
    Friend WithEvents SortUpButton As System.Windows.Forms.Button
    Friend WithEvents LineDiagnosisDataGrid As DataGridCustom
    Friend WithEvents DeleteButton As System.Windows.Forms.Button
    Friend WithEvents MainToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents DiagnosisCodesTextBox As System.Windows.Forms.TextBox
    Friend WithEvents DiagnosisLookupButton As System.Windows.Forms.Button
    Friend WithEvents ClearAllButton As System.Windows.Forms.Button
    Friend WithEvents UpdateAllButton As System.Windows.Forms.Button
    Friend WithEvents UpdateLineButton As System.Windows.Forms.Button

End Class
