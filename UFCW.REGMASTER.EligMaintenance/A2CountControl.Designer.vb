<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class A2CountControl
    Inherits System.Windows.Forms.UserControl


    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ModifyActionButton = New System.Windows.Forms.Button()
        Me.SaveActionButton = New System.Windows.Forms.Button()
        Me.CancelActionButton = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.A2CountDataGrid = New DataGridCustom()
        Me.controlGroupBox = New System.Windows.Forms.GroupBox()
        Me.grpA2cnt = New System.Windows.Forms.GroupBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtA2count = New System.Windows.Forms.TextBox()
        Me.grpStart = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtStartHoursDate = New System.Windows.Forms.TextBox()
        Me.grpEditPanel = New System.Windows.Forms.GroupBox()
        Me.lblACAElgDate = New System.Windows.Forms.Label()
        Me.txtACAEligDate = New System.Windows.Forms.TextBox()
        Me.rsStartHours = New System.Windows.Forms.RadioButton()
        Me.rbA2Count = New System.Windows.Forms.RadioButton()
        Me.cmbReasons = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.AddActionButton = New System.Windows.Forms.Button()
        Me._A2DS = New A2countDS()
        Me.GroupBox2.SuspendLayout()
        CType(Me.A2CountDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.controlGroupBox.SuspendLayout()
        Me.grpA2cnt.SuspendLayout()
        Me.grpStart.SuspendLayout()
        Me.grpEditPanel.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._A2DS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnModify
        '
        Me.ModifyActionButton.Location = New System.Drawing.Point(76, 161)
        Me.ModifyActionButton.Name = "btnModify"
        Me.ModifyActionButton.Size = New System.Drawing.Size(60, 23)
        Me.ModifyActionButton.TabIndex = 9
        Me.ModifyActionButton.Text = "Modify"
        Me.ModifyActionButton.UseVisualStyleBackColor = True
        '
        'SaveButton
        '
        Me.SaveActionButton.Enabled = False
        Me.SaveActionButton.Location = New System.Drawing.Point(272, 160)
        Me.SaveActionButton.Name = "SaveButton"
        Me.SaveActionButton.Size = New System.Drawing.Size(60, 23)
        Me.SaveActionButton.TabIndex = 11
        Me.SaveActionButton.Text = "Save"
        Me.SaveActionButton.UseVisualStyleBackColor = True
        '
        'btncancel
        '
        Me.CancelActionButton.Location = New System.Drawing.Point(206, 160)
        Me.CancelActionButton.Name = "btncancel"
        Me.CancelActionButton.Size = New System.Drawing.Size(60, 23)
        Me.CancelActionButton.TabIndex = 10
        Me.CancelActionButton.Text = "Cancel"
        Me.CancelActionButton.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.A2CountDataGrid)
        Me.GroupBox2.Location = New System.Drawing.Point(6, 190)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(480, 202)
        Me.GroupBox2.TabIndex = 12
        Me.GroupBox2.TabStop = False
        '
        'A2countDataGrid
        '
        Me.A2CountDataGrid.ADGroupsThatCanCopy = ""
        Me.A2CountDataGrid.ADGroupsThatCanCustomize = ""
        Me.A2CountDataGrid.ADGroupsThatCanExport = ""
        Me.A2CountDataGrid.ADGroupsThatCanFilter = ""
        Me.A2CountDataGrid.ADGroupsThatCanFind = ""
        Me.A2CountDataGrid.ADGroupsThatCanMultiSort = ""
        Me.A2CountDataGrid.ADGroupsThatCanPrint = ""
        Me.A2CountDataGrid.AllowAutoSize = True
        Me.A2CountDataGrid.AllowColumnReorder = True
        Me.A2CountDataGrid.AllowCopy = True
        Me.A2CountDataGrid.AllowCustomize = True
        Me.A2CountDataGrid.AllowDelete = False
        Me.A2CountDataGrid.AllowDragDrop = False
        Me.A2CountDataGrid.AllowEdit = False
        Me.A2CountDataGrid.AllowExport = False
        Me.A2CountDataGrid.AllowFilter = True
        Me.A2CountDataGrid.AllowFind = True
        Me.A2CountDataGrid.AllowGoTo = True
        Me.A2CountDataGrid.AllowMultiSelect = False
        Me.A2CountDataGrid.AllowMultiSort = False
        Me.A2CountDataGrid.AllowNew = False
        Me.A2CountDataGrid.AllowPrint = True
        Me.A2CountDataGrid.AllowRefresh = True
        Me.A2CountDataGrid.AppKey = "UFCW\RegMaster\"
        Me.A2CountDataGrid.AutoSaveCols = True
        Me.A2CountDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.A2CountDataGrid.ColumnHeaderLabel = Nothing
        Me.A2CountDataGrid.ColumnRePositioning = False
        Me.A2CountDataGrid.ColumnResizing = False
        Me.A2CountDataGrid.ConfirmDelete = True
        Me.A2CountDataGrid.CopySelectedOnly = True
        Me.A2CountDataGrid.DataMember = ""
        Me.A2CountDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.A2CountDataGrid.DragColumn = 0
        Me.A2CountDataGrid.ExportSelectedOnly = True
        Me.A2CountDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.A2CountDataGrid.HighlightedRow = Nothing
        Me.A2CountDataGrid.HighLightModifiedRows = False
        Me.A2CountDataGrid.IsMouseDown = False
        Me.A2CountDataGrid.LastGoToLine = ""
        Me.A2CountDataGrid.Location = New System.Drawing.Point(3, 16)
        Me.A2CountDataGrid.MultiSort = False
        Me.A2CountDataGrid.Name = "A2countDataGrid"
        Me.A2CountDataGrid.OldSelectedRow = 0
        Me.A2CountDataGrid.ReadOnly = True
        Me.A2CountDataGrid.RetainRowSelectionAfterSort = True
        Me.A2CountDataGrid.SetRowOnRightClick = True
        Me.A2CountDataGrid.ShiftPressed = False
        Me.A2CountDataGrid.SingleClickBooleanColumns = True
        Me.A2CountDataGrid.Size = New System.Drawing.Size(474, 183)
        Me.A2CountDataGrid.Sort = Nothing
        Me.A2CountDataGrid.StyleName = ""
        Me.A2CountDataGrid.SubKey = ""
        Me.A2CountDataGrid.SuppressTriangle = False
        Me.A2CountDataGrid.TabIndex = 103
        '
        'controlGroupBox
        '
        Me.controlGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.controlGroupBox.Controls.Add(Me.grpA2cnt)
        Me.controlGroupBox.Controls.Add(Me.grpStart)
        Me.controlGroupBox.Controls.Add(Me.grpEditPanel)
        Me.controlGroupBox.Location = New System.Drawing.Point(3, 3)
        Me.controlGroupBox.Name = "controlGroupBox"
        Me.controlGroupBox.Size = New System.Drawing.Size(480, 152)
        Me.controlGroupBox.TabIndex = 0
        Me.controlGroupBox.TabStop = False
        '
        'grpA2cnt
        '
        Me.grpA2cnt.Controls.Add(Me.Label5)
        Me.grpA2cnt.Controls.Add(Me.txtA2count)
        Me.grpA2cnt.Location = New System.Drawing.Point(250, 102)
        Me.grpA2cnt.Name = "grpA2cnt"
        Me.grpA2cnt.Size = New System.Drawing.Size(227, 44)
        Me.grpA2cnt.TabIndex = 111
        Me.grpA2cnt.TabStop = False
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(15, 13)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(53, 18)
        Me.Label5.TabIndex = 107
        Me.Label5.Text = "A2count:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtA2count
        '
        Me.txtA2count.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtA2count.Location = New System.Drawing.Point(78, 13)
        Me.txtA2count.MaxLength = 3
        Me.txtA2count.Name = "txtA2count"
        Me.txtA2count.Size = New System.Drawing.Size(45, 20)
        Me.txtA2count.TabIndex = 6
        '
        'grpStart
        '
        Me.grpStart.Controls.Add(Me.Label1)
        Me.grpStart.Controls.Add(Me.txtStartHoursDate)
        Me.grpStart.Location = New System.Drawing.Point(3, 102)
        Me.grpStart.Name = "grpStart"
        Me.grpStart.Size = New System.Drawing.Size(246, 44)
        Me.grpStart.TabIndex = 110
        Me.grpStart.TabStop = False
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(32, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(102, 26)
        Me.Label1.TabIndex = 109
        Me.Label1.Text = "Start of consecutive worked hours:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtstarthours
        '
        Me.txtStartHoursDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStartHoursDate.Location = New System.Drawing.Point(134, 13)
        Me.txtStartHoursDate.Name = "txtstarthours"
        Me.txtStartHoursDate.Size = New System.Drawing.Size(93, 20)
        Me.txtStartHoursDate.TabIndex = 5
        '
        'grpEditPanel
        '
        Me.grpEditPanel.Controls.Add(Me.lblACAElgDate)
        Me.grpEditPanel.Controls.Add(Me.txtACAEligDate)
        Me.grpEditPanel.Controls.Add(Me.rsStartHours)
        Me.grpEditPanel.Controls.Add(Me.rbA2Count)
        Me.grpEditPanel.Controls.Add(Me.cmbReasons)
        Me.grpEditPanel.Controls.Add(Me.Label4)
        Me.grpEditPanel.Location = New System.Drawing.Point(3, 9)
        Me.grpEditPanel.Name = "grpEditPanel"
        Me.grpEditPanel.Size = New System.Drawing.Size(474, 95)
        Me.grpEditPanel.TabIndex = 1
        Me.grpEditPanel.TabStop = False
        Me.grpEditPanel.Text = "Override"
        '
        'lblacaelgdate
        '
        Me.lblACAElgDate.Location = New System.Drawing.Point(55, 64)
        Me.lblACAElgDate.Name = "lblacaelgdate"
        Me.lblACAElgDate.Size = New System.Drawing.Size(81, 21)
        Me.lblACAElgDate.TabIndex = 111
        Me.lblACAElgDate.Text = "ACA Elig Date:"
        Me.lblACAElgDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblACAElgDate.Visible = False
        '
        'txtACAEligdt
        '
        Me.txtACAEligDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtACAEligDate.Location = New System.Drawing.Point(134, 65)
        Me.txtACAEligDate.Name = "txtACAEligdt"
        Me.txtACAEligDate.Size = New System.Drawing.Size(93, 20)
        Me.txtACAEligDate.TabIndex = 4
        Me.txtACAEligDate.Visible = False
        '
        'rbstarthours
        '
        Me.rsStartHours.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rsStartHours.Location = New System.Drawing.Point(13, 15)
        Me.rsStartHours.Name = "rbstarthours"
        Me.rsStartHours.Size = New System.Drawing.Size(199, 17)
        Me.rsStartHours.TabIndex = 2
        Me.rsStartHours.TabStop = True
        Me.rsStartHours.Text = "Start of consecutive hours"
        Me.rsStartHours.UseVisualStyleBackColor = True
        '
        'rbA2count
        '
        Me.rbA2Count.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbA2Count.Location = New System.Drawing.Point(214, 14)
        Me.rbA2Count.Name = "rbA2count"
        Me.rbA2Count.Size = New System.Drawing.Size(83, 18)
        Me.rbA2Count.TabIndex = 3
        Me.rbA2Count.TabStop = True
        Me.rbA2Count.Text = "A2 count"
        Me.rbA2Count.UseVisualStyleBackColor = True
        '
        'cmbreason
        '
        Me.cmbReasons.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbReasons.DropDownWidth = 350
        Me.cmbReasons.FormattingEnabled = True
        Me.cmbReasons.Location = New System.Drawing.Point(134, 38)
        Me.cmbReasons.Name = "cmbreason"
        Me.cmbReasons.Size = New System.Drawing.Size(314, 21)
        Me.cmbReasons.TabIndex = 4
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(28, 38)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(120, 18)
        Me.Label4.TabIndex = 104
        Me.Label4.Text = "Reason for Override:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'btnAdd
        '
        Me.AddActionButton.Location = New System.Drawing.Point(10, 161)
        Me.AddActionButton.Name = "btnAdd"
        Me.AddActionButton.Size = New System.Drawing.Size(60, 23)
        Me.AddActionButton.TabIndex = 112
        Me.AddActionButton.Text = "Add"
        Me.AddActionButton.UseVisualStyleBackColor = True
        '
        'A2DS
        '
        Me._A2DS.DataSetName = "A2countDS"
        Me._A2DS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'A2CountControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.AddActionButton)
        Me.Controls.Add(Me.ModifyActionButton)
        Me.Controls.Add(Me.SaveActionButton)
        Me.Controls.Add(Me.CancelActionButton)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.controlGroupBox)
        Me.Name = "A2CountControl"
        Me.Size = New System.Drawing.Size(486, 395)
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.A2CountDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.controlGroupBox.ResumeLayout(False)
        Me.grpA2cnt.ResumeLayout(False)
        Me.grpA2cnt.PerformLayout()
        Me.grpStart.ResumeLayout(False)
        Me.grpStart.PerformLayout()
        Me.grpEditPanel.ResumeLayout(False)
        Me.grpEditPanel.PerformLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._A2DS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ModifyActionButton As System.Windows.Forms.Button
    Friend WithEvents SaveActionButton As System.Windows.Forms.Button
    Friend WithEvents CancelActionButton As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents A2CountDataGrid As DataGridCustom
    Friend WithEvents controlGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents grpEditPanel As System.Windows.Forms.GroupBox
    Friend WithEvents rsStartHours As System.Windows.Forms.RadioButton
    Friend WithEvents rbA2Count As System.Windows.Forms.RadioButton
    Friend WithEvents cmbReasons As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtStartHoursDate As System.Windows.Forms.TextBox
    Friend WithEvents txtA2count As System.Windows.Forms.TextBox
    Friend WithEvents grpA2cnt As System.Windows.Forms.GroupBox
    Friend WithEvents grpStart As System.Windows.Forms.GroupBox
    Friend WithEvents _A2DS As A2countDS
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents AddActionButton As System.Windows.Forms.Button
    Friend WithEvents lblACAElgDate As System.Windows.Forms.Label
    Friend WithEvents txtACAEligDate As System.Windows.Forms.TextBox

End Class
