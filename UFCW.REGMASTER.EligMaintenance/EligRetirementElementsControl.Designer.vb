<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EligRetirementElementsControl
    Inherits System.Windows.Forms.UserControl

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.DeleteActionButton = New System.Windows.Forms.Button()
        Me.ModifyActionButton = New System.Windows.Forms.Button()
        Me.AddActionButton = New System.Windows.Forms.Button()
        Me.SaveActionButton = New System.Windows.Forms.Button()
        Me.CancelActionButton = New System.Windows.Forms.Button()
        Me.grpEditPanel = New System.Windows.Forms.GroupBox()
        Me.HistoryActionButton = New System.Windows.Forms.Button()
        Me.TransparentContainer1 = New TransparentContainer()
        Me.cmbRetPlan = New ExComboBox()
        Me.txtThruDate = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtFamilyID = New System.Windows.Forms.TextBox()
        Me.txtFromDate = New System.Windows.Forms.TextBox()
        Me.FromLabel = New System.Windows.Forms.Label()
        Me.RetireeHistoryDataGrid = New DataGridCustom()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.grpEditPanel.SuspendLayout()
        Me.TransparentContainer1.SuspendLayout()
        CType(Me.RetireeHistoryDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.IsSplitterFixed = True
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.MinimumSize = New System.Drawing.Size(553, 373)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.DeleteActionButton)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ModifyActionButton)
        Me.SplitContainer1.Panel1.Controls.Add(Me.AddActionButton)
        Me.SplitContainer1.Panel1.Controls.Add(Me.SaveActionButton)
        Me.SplitContainer1.Panel1.Controls.Add(Me.CancelActionButton)
        Me.SplitContainer1.Panel1.Controls.Add(Me.grpEditPanel)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.RetireeHistoryDataGrid)
        Me.SplitContainer1.Size = New System.Drawing.Size(553, 373)
        Me.SplitContainer1.SplitterDistance = 196
        Me.SplitContainer1.TabIndex = 127
        '
        'DeleteActionButton
        '
        Me.DeleteActionButton.Location = New System.Drawing.Point(149, 166)
        Me.DeleteActionButton.Name = "DeleteActionButton"
        Me.DeleteActionButton.Size = New System.Drawing.Size(60, 23)
        Me.DeleteActionButton.TabIndex = 129
        Me.DeleteActionButton.Text = "Delete"
        Me.DeleteActionButton.UseVisualStyleBackColor = True
        '
        'ModifyActionButton
        '
        Me.ModifyActionButton.Location = New System.Drawing.Point(81, 166)
        Me.ModifyActionButton.Name = "ModifyActionButton"
        Me.ModifyActionButton.Size = New System.Drawing.Size(60, 23)
        Me.ModifyActionButton.TabIndex = 128
        Me.ModifyActionButton.Text = "Modify"
        Me.ModifyActionButton.UseVisualStyleBackColor = True
        '
        'AddActionButton
        '
        Me.AddActionButton.Location = New System.Drawing.Point(15, 166)
        Me.AddActionButton.Name = "AddActionButton"
        Me.AddActionButton.Size = New System.Drawing.Size(60, 23)
        Me.AddActionButton.TabIndex = 127
        Me.AddActionButton.Text = "Add"
        Me.AddActionButton.UseVisualStyleBackColor = True
        '
        'SaveActionButton
        '
        Me.SaveActionButton.CausesValidation = False
        Me.SaveActionButton.Enabled = False
        Me.SaveActionButton.Location = New System.Drawing.Point(404, 167)
        Me.SaveActionButton.Name = "SaveActionButton"
        Me.SaveActionButton.Size = New System.Drawing.Size(60, 23)
        Me.SaveActionButton.TabIndex = 131
        Me.SaveActionButton.Text = "Save"
        Me.SaveActionButton.UseVisualStyleBackColor = True
        '
        'CancelActionButton
        '
        Me.CancelActionButton.CausesValidation = False
        Me.CancelActionButton.Location = New System.Drawing.Point(338, 167)
        Me.CancelActionButton.Name = "CancelActionButton"
        Me.CancelActionButton.Size = New System.Drawing.Size(60, 23)
        Me.CancelActionButton.TabIndex = 130
        Me.CancelActionButton.Text = "Cancel"
        Me.CancelActionButton.UseVisualStyleBackColor = True
        '
        'grpEditPanel
        '
        Me.grpEditPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpEditPanel.Controls.Add(Me.HistoryActionButton)
        Me.grpEditPanel.Controls.Add(Me.TransparentContainer1)
        Me.grpEditPanel.Controls.Add(Me.txtThruDate)
        Me.grpEditPanel.Controls.Add(Me.Label5)
        Me.grpEditPanel.Controls.Add(Me.Label4)
        Me.grpEditPanel.Controls.Add(Me.Label2)
        Me.grpEditPanel.Controls.Add(Me.txtFamilyID)
        Me.grpEditPanel.Controls.Add(Me.txtFromDate)
        Me.grpEditPanel.Controls.Add(Me.FromLabel)
        Me.grpEditPanel.Location = New System.Drawing.Point(10, 8)
        Me.grpEditPanel.Name = "grpEditPanel"
        Me.grpEditPanel.Size = New System.Drawing.Size(469, 153)
        Me.grpEditPanel.TabIndex = 132
        Me.grpEditPanel.TabStop = False
        '
        'HistoryActionButton
        '
        Me.HistoryActionButton.CausesValidation = False
        Me.HistoryActionButton.Location = New System.Drawing.Point(394, 19)
        Me.HistoryActionButton.Name = "HistoryActionButton"
        Me.HistoryActionButton.Size = New System.Drawing.Size(60, 23)
        Me.HistoryActionButton.TabIndex = 113
        Me.HistoryActionButton.Text = "History"
        Me.HistoryActionButton.UseVisualStyleBackColor = True
        '
        'TransparentContainer1
        '
        Me.TransparentContainer1.Controls.Add(Me.cmbRetPlan)
        Me.TransparentContainer1.Location = New System.Drawing.Point(106, 91)
        Me.TransparentContainer1.Name = "TransparentContainer1"
        Me.TransparentContainer1.Size = New System.Drawing.Size(77, 23)
        Me.TransparentContainer1.TabIndex = 107
        '
        'cmbRetPlan
        '
        Me.cmbRetPlan.BackColor = System.Drawing.SystemColors.Control
        Me.cmbRetPlan.DropDownHeight = 100
        Me.cmbRetPlan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbRetPlan.DropDownWidth = 250
        Me.cmbRetPlan.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbRetPlan.FormattingEnabled = True
        Me.cmbRetPlan.IntegralHeight = False
        Me.cmbRetPlan.ItemHeight = 13
        Me.cmbRetPlan.Items.AddRange(New Object() {""})
        Me.cmbRetPlan.Location = New System.Drawing.Point(0, 0)
        Me.cmbRetPlan.Name = "cmbRetPlan"
        Me.cmbRetPlan.ReadOnly = False
        Me.cmbRetPlan.Size = New System.Drawing.Size(51, 21)
        Me.cmbRetPlan.TabIndex = 3
        '
        'txtThruDate
        '
        Me.txtThruDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThruDate.Location = New System.Drawing.Point(106, 65)
        Me.txtThruDate.MaxLength = 10
        Me.txtThruDate.Name = "txtThruDate"
        Me.txtThruDate.ReadOnly = True
        Me.txtThruDate.Size = New System.Drawing.Size(91, 20)
        Me.txtThruDate.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(39, 68)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(55, 13)
        Me.Label5.TabIndex = 106
        Me.Label5.Text = "Thru Date"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(29, 93)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(65, 13)
        Me.Label4.TabIndex = 102
        Me.Label4.Text = "Retiree Plan"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(44, 17)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(50, 13)
        Me.Label2.TabIndex = 99
        Me.Label2.Text = "Family ID"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtFamilyID
        '
        Me.txtFamilyID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFamilyID.Location = New System.Drawing.Point(106, 14)
        Me.txtFamilyID.MaxLength = 15
        Me.txtFamilyID.Name = "txtFamilyID"
        Me.txtFamilyID.ReadOnly = True
        Me.txtFamilyID.Size = New System.Drawing.Size(91, 20)
        Me.txtFamilyID.TabIndex = 8
        '
        'txtFromDate
        '
        Me.txtFromDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFromDate.Location = New System.Drawing.Point(106, 40)
        Me.txtFromDate.MaxLength = 10
        Me.txtFromDate.Name = "txtFromDate"
        Me.txtFromDate.Size = New System.Drawing.Size(91, 20)
        Me.txtFromDate.TabIndex = 0
        '
        'FromLabel
        '
        Me.FromLabel.AutoSize = True
        Me.FromLabel.Location = New System.Drawing.Point(38, 43)
        Me.FromLabel.Name = "FromLabel"
        Me.FromLabel.Size = New System.Drawing.Size(56, 13)
        Me.FromLabel.TabIndex = 93
        Me.FromLabel.Text = "From Date"
        Me.FromLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'RetireeHistoryDataGrid
        '
        Me.RetireeHistoryDataGrid.ADGroupsThatCanCopy = ""
        Me.RetireeHistoryDataGrid.ADGroupsThatCanCustomize = ""
        Me.RetireeHistoryDataGrid.ADGroupsThatCanExport = ""
        Me.RetireeHistoryDataGrid.ADGroupsThatCanFilter = ""
        Me.RetireeHistoryDataGrid.ADGroupsThatCanFind = ""
        Me.RetireeHistoryDataGrid.ADGroupsThatCanMultiSort = ""
        Me.RetireeHistoryDataGrid.ADGroupsThatCanPrint = ""
        Me.RetireeHistoryDataGrid.AllowAutoSize = True
        Me.RetireeHistoryDataGrid.AllowColumnReorder = True
        Me.RetireeHistoryDataGrid.AllowCopy = True
        Me.RetireeHistoryDataGrid.AllowCustomize = True
        Me.RetireeHistoryDataGrid.AllowDelete = False
        Me.RetireeHistoryDataGrid.AllowDragDrop = False
        Me.RetireeHistoryDataGrid.AllowEdit = False
        Me.RetireeHistoryDataGrid.AllowExport = False
        Me.RetireeHistoryDataGrid.AllowFilter = True
        Me.RetireeHistoryDataGrid.AllowFind = True
        Me.RetireeHistoryDataGrid.AllowGoTo = True
        Me.RetireeHistoryDataGrid.AllowMultiSelect = False
        Me.RetireeHistoryDataGrid.AllowMultiSort = False
        Me.RetireeHistoryDataGrid.AllowNew = False
        Me.RetireeHistoryDataGrid.AllowPrint = True
        Me.RetireeHistoryDataGrid.AllowRefresh = True
        Me.RetireeHistoryDataGrid.AppKey = "UFCW\RegMaster\"
        Me.RetireeHistoryDataGrid.AutoSaveCols = True
        Me.RetireeHistoryDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.RetireeHistoryDataGrid.ColumnHeaderLabel = Nothing
        Me.RetireeHistoryDataGrid.ColumnRePositioning = False
        Me.RetireeHistoryDataGrid.ColumnResizing = False
        Me.RetireeHistoryDataGrid.ConfirmDelete = True
        Me.RetireeHistoryDataGrid.CopySelectedOnly = True
        Me.RetireeHistoryDataGrid.CurrentBSPosition = -1
        Me.RetireeHistoryDataGrid.DataMember = ""
        Me.RetireeHistoryDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RetireeHistoryDataGrid.DragColumn = 0
        Me.RetireeHistoryDataGrid.ExportSelectedOnly = True
        Me.RetireeHistoryDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.RetireeHistoryDataGrid.HighlightedRow = Nothing
        Me.RetireeHistoryDataGrid.HighLightModifiedRows = False
        Me.RetireeHistoryDataGrid.IsMouseDown = False
        Me.RetireeHistoryDataGrid.LastGoToLine = ""
        Me.RetireeHistoryDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.RetireeHistoryDataGrid.MultiSort = False
        Me.RetireeHistoryDataGrid.Name = "RetireeHistoryDataGrid"
        Me.RetireeHistoryDataGrid.OldSelectedRow = 0
        Me.RetireeHistoryDataGrid.PreviousBSPosition = -1
        Me.RetireeHistoryDataGrid.ReadOnly = True
        Me.RetireeHistoryDataGrid.RetainRowSelectionAfterSort = True
        Me.RetireeHistoryDataGrid.SetRowOnRightClick = True
        Me.RetireeHistoryDataGrid.ShiftPressed = False
        Me.RetireeHistoryDataGrid.SingleClickBooleanColumns = True
        Me.RetireeHistoryDataGrid.Size = New System.Drawing.Size(553, 173)
        Me.RetireeHistoryDataGrid.Sort = Nothing
        Me.RetireeHistoryDataGrid.StyleName = ""
        Me.RetireeHistoryDataGrid.SubKey = ""
        Me.RetireeHistoryDataGrid.SuppressMouseDown = False
        Me.RetireeHistoryDataGrid.SuppressTriangle = False
        Me.RetireeHistoryDataGrid.TabIndex = 9
        '
        'EligRetirementElementsControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "EligRetirementElementsControl"
        Me.Size = New System.Drawing.Size(553, 373)
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.grpEditPanel.ResumeLayout(False)
        Me.grpEditPanel.PerformLayout()
        Me.TransparentContainer1.ResumeLayout(False)
        CType(Me.RetireeHistoryDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents SplitContainer1 As Windows.Forms.SplitContainer
    Friend WithEvents DeleteActionButton As Windows.Forms.Button
    Friend WithEvents ModifyActionButton As Windows.Forms.Button
    Friend WithEvents AddActionButton As Windows.Forms.Button
    Friend WithEvents SaveActionButton As Windows.Forms.Button
    Friend WithEvents CancelActionButton As Windows.Forms.Button
    Friend WithEvents grpEditPanel As Windows.Forms.GroupBox
    Friend WithEvents TransparentContainer1 As TransparentContainer
    Friend WithEvents cmbRetPlan As ExComboBox
    Friend WithEvents txtThruDate As Windows.Forms.TextBox
    Friend WithEvents Label5 As Windows.Forms.Label
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents txtFamilyID As Windows.Forms.TextBox
    Friend WithEvents txtFromDate As Windows.Forms.TextBox
    Friend WithEvents FromLabel As Windows.Forms.Label
    Friend WithEvents RetireeHistoryDataGrid As DataGridCustom
    Friend WithEvents HistoryActionButton As Windows.Forms.Button
End Class
