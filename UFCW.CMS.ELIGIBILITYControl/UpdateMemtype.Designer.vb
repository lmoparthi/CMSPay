<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UpdateMemtypeForm
    Inherits System.Windows.Forms.Form

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
        Me.MemTypeDataGrid = New DataGridCustom()
        Me.controlGroupBox = New System.Windows.Forms.GroupBox()
        Me.cmbMemTypeTransparentContainer = New TransparentContainer()
        Me.cmbMemType = New ExComboBox()
        Me.HistoryButton = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtFamilyID = New System.Windows.Forms.TextBox()
        Me.txtRelationID = New System.Windows.Forms.TextBox()
        Me.txtEligibiltyMonth = New System.Windows.Forms.TextBox()
        Me.FromLabel = New System.Windows.Forms.Label()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me._MemTypeDS = New EligMthdtlDS()
        Me.ExitButton = New System.Windows.Forms.Button()
        Me.GroupBox2.SuspendLayout()
        CType(Me.MemTypeDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.controlGroupBox.SuspendLayout()
        Me.cmbMemTypeTransparentContainer.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._MemTypeDS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ModifyActionButton
        '
        Me.ModifyActionButton.Location = New System.Drawing.Point(69, 167)
        Me.ModifyActionButton.Name = "ModifyActionButton"
        Me.ModifyActionButton.Size = New System.Drawing.Size(60, 23)
        Me.ModifyActionButton.TabIndex = 123
        Me.ModifyActionButton.Text = "Modify"
        Me.ModifyActionButton.UseVisualStyleBackColor = True
        '
        'SaveActionButton
        '
        Me.SaveActionButton.Enabled = False
        Me.SaveActionButton.Location = New System.Drawing.Point(256, 167)
        Me.SaveActionButton.Name = "SaveActionButton"
        Me.SaveActionButton.Size = New System.Drawing.Size(60, 23)
        Me.SaveActionButton.TabIndex = 125
        Me.SaveActionButton.Text = "Save"
        Me.SaveActionButton.UseVisualStyleBackColor = True
        '
        'CancelActionButton
        '
        Me.CancelActionButton.Location = New System.Drawing.Point(190, 167)
        Me.CancelActionButton.Name = "CancelActionButton"
        Me.CancelActionButton.Size = New System.Drawing.Size(60, 23)
        Me.CancelActionButton.TabIndex = 124
        Me.CancelActionButton.Text = "Cancel"
        Me.CancelActionButton.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.MemTypeDataGrid)
        Me.GroupBox2.Location = New System.Drawing.Point(-1, 196)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(532, 145)
        Me.GroupBox2.TabIndex = 127
        Me.GroupBox2.TabStop = False
        '
        'MemTypeDataGrid
        '
        Me.MemTypeDataGrid.ADGroupsThatCanCopy = ""
        Me.MemTypeDataGrid.ADGroupsThatCanCustomize = ""
        Me.MemTypeDataGrid.ADGroupsThatCanExport = ""
        Me.MemTypeDataGrid.ADGroupsThatCanFilter = ""
        Me.MemTypeDataGrid.ADGroupsThatCanFind = ""
        Me.MemTypeDataGrid.ADGroupsThatCanMultiSort = ""
        Me.MemTypeDataGrid.ADGroupsThatCanPrint = ""
        Me.MemTypeDataGrid.AllowAutoSize = True
        Me.MemTypeDataGrid.AllowColumnReorder = True
        Me.MemTypeDataGrid.AllowCopy = True
        Me.MemTypeDataGrid.AllowCustomize = True
        Me.MemTypeDataGrid.AllowDelete = False
        Me.MemTypeDataGrid.AllowDragDrop = False
        Me.MemTypeDataGrid.AllowEdit = False
        Me.MemTypeDataGrid.AllowExport = False
        Me.MemTypeDataGrid.AllowFilter = True
        Me.MemTypeDataGrid.AllowFind = True
        Me.MemTypeDataGrid.AllowGoTo = True
        Me.MemTypeDataGrid.AllowMultiSelect = False
        Me.MemTypeDataGrid.AllowMultiSort = False
        Me.MemTypeDataGrid.AllowNew = False
        Me.MemTypeDataGrid.AllowPrint = True
        Me.MemTypeDataGrid.AllowRefresh = True
        Me.MemTypeDataGrid.AppKey = "UFCW\RegMaster\"
        Me.MemTypeDataGrid.AutoSaveCols = True
        Me.MemTypeDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.MemTypeDataGrid.ColumnHeaderLabel = Nothing
        Me.MemTypeDataGrid.ColumnRePositioning = False
        Me.MemTypeDataGrid.ColumnResizing = False
        Me.MemTypeDataGrid.ConfirmDelete = True
        Me.MemTypeDataGrid.CopySelectedOnly = True
        Me.MemTypeDataGrid.DataMember = ""
        Me.MemTypeDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MemTypeDataGrid.DragColumn = 0
        Me.MemTypeDataGrid.ExportSelectedOnly = True
        Me.MemTypeDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.MemTypeDataGrid.HighlightedRow = Nothing
        Me.MemTypeDataGrid.HighLightModifiedRows = False
        Me.MemTypeDataGrid.IsMouseDown = False
        Me.MemTypeDataGrid.LastGoToLine = ""
        Me.MemTypeDataGrid.Location = New System.Drawing.Point(3, 16)
        Me.MemTypeDataGrid.MultiSort = False
        Me.MemTypeDataGrid.Name = "MemTypeDataGrid"
        Me.MemTypeDataGrid.OldSelectedRow = 0
        Me.MemTypeDataGrid.ReadOnly = True
        Me.MemTypeDataGrid.RetainRowSelectionAfterSort = True
        Me.MemTypeDataGrid.SetRowOnRightClick = True
        Me.MemTypeDataGrid.ShiftPressed = False
        Me.MemTypeDataGrid.SingleClickBooleanColumns = True
        Me.MemTypeDataGrid.Size = New System.Drawing.Size(526, 126)
        Me.MemTypeDataGrid.Sort = Nothing
        Me.MemTypeDataGrid.StyleName = ""
        Me.MemTypeDataGrid.SubKey = ""
        Me.MemTypeDataGrid.SuppressMouseDown = False
        Me.MemTypeDataGrid.SuppressTriangle = False
        Me.MemTypeDataGrid.TabIndex = 7
        '
        'controlGroupBox
        '
        Me.controlGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.controlGroupBox.Controls.Add(Me.cmbMemTypeTransparentContainer)
        Me.controlGroupBox.Controls.Add(Me.HistoryButton)
        Me.controlGroupBox.Controls.Add(Me.Label4)
        Me.controlGroupBox.Controls.Add(Me.Label1)
        Me.controlGroupBox.Controls.Add(Me.Label2)
        Me.controlGroupBox.Controls.Add(Me.txtFamilyID)
        Me.controlGroupBox.Controls.Add(Me.txtRelationID)
        Me.controlGroupBox.Controls.Add(Me.txtEligibiltyMonth)
        Me.controlGroupBox.Controls.Add(Me.FromLabel)
        Me.controlGroupBox.Location = New System.Drawing.Point(-1, 9)
        Me.controlGroupBox.Name = "controlGroupBox"
        Me.controlGroupBox.Size = New System.Drawing.Size(532, 152)
        Me.controlGroupBox.TabIndex = 126
        Me.controlGroupBox.TabStop = False
        '
        'cmbMemTypeTransparentContainer
        '
        Me.cmbMemTypeTransparentContainer.Controls.Add(Me.cmbMemType)
        Me.cmbMemTypeTransparentContainer.Location = New System.Drawing.Point(107, 91)
        Me.cmbMemTypeTransparentContainer.Name = "cmbMemTypeTransparentContainer"
        Me.cmbMemTypeTransparentContainer.Size = New System.Drawing.Size(175, 23)
        Me.cmbMemTypeTransparentContainer.TabIndex = 114
        '
        'cmbMemType
        '
        Me.cmbMemType.DropDownHeight = 100
        Me.cmbMemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMemType.DropDownWidth = 250
        Me.cmbMemType.FormattingEnabled = True
        Me.cmbMemType.IntegralHeight = False
        Me.cmbMemType.ItemHeight = 13
        Me.cmbMemType.Location = New System.Drawing.Point(0, 1)
        Me.cmbMemType.Name = "cmbMemType"
        Me.cmbMemType.ReadOnly = True
        Me.cmbMemType.Size = New System.Drawing.Size(144, 21)
        Me.cmbMemType.TabIndex = 2
        '
        'HistoryButton
        '
        Me.HistoryButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HistoryButton.CausesValidation = False
        Me.HistoryButton.Location = New System.Drawing.Point(459, 19)
        Me.HistoryButton.Name = "HistoryButton"
        Me.HistoryButton.Size = New System.Drawing.Size(60, 23)
        Me.HistoryButton.TabIndex = 113
        Me.HistoryButton.Text = "History"
        Me.HistoryButton.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(44, 95)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(50, 13)
        Me.Label4.TabIndex = 102
        Me.Label4.Text = "Memtype"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(44, 41)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(54, 13)
        Me.Label1.TabIndex = 100
        Me.Label1.Text = "Relationid"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(54, 17)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(44, 13)
        Me.Label2.TabIndex = 99
        Me.Label2.Text = "Familyid"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtFamilyID
        '
        Me.txtFamilyID.Enabled = False
        Me.txtFamilyID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFamilyID.Location = New System.Drawing.Point(106, 14)
        Me.txtFamilyID.MaxLength = 15
        Me.txtFamilyID.Name = "txtFamilyID"
        Me.txtFamilyID.ReadOnly = True
        Me.txtFamilyID.Size = New System.Drawing.Size(91, 20)
        Me.txtFamilyID.TabIndex = 97
        '
        'txtRelationID
        '
        Me.txtRelationID.Enabled = False
        Me.txtRelationID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRelationID.Location = New System.Drawing.Point(107, 40)
        Me.txtRelationID.MaxLength = 10
        Me.txtRelationID.Name = "txtRelationID"
        Me.txtRelationID.ReadOnly = True
        Me.txtRelationID.Size = New System.Drawing.Size(31, 20)
        Me.txtRelationID.TabIndex = 98
        '
        'txtEligibiltyMonth
        '
        Me.txtEligibiltyMonth.Enabled = False
        Me.txtEligibiltyMonth.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEligibiltyMonth.Location = New System.Drawing.Point(106, 65)
        Me.txtEligibiltyMonth.MaxLength = 10
        Me.txtEligibiltyMonth.Name = "txtEligibiltyMonth"
        Me.txtEligibiltyMonth.ReadOnly = True
        Me.txtEligibiltyMonth.Size = New System.Drawing.Size(91, 20)
        Me.txtEligibiltyMonth.TabIndex = 0
        '
        'FromLabel
        '
        Me.FromLabel.AutoSize = True
        Me.FromLabel.Location = New System.Drawing.Point(22, 68)
        Me.FromLabel.Name = "FromLabel"
        Me.FromLabel.Size = New System.Drawing.Size(79, 13)
        Me.FromLabel.TabIndex = 93
        Me.FromLabel.Text = "Eligibility Month"
        Me.FromLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        '_MemTypeDS
        '
        Me._MemTypeDS.DataSetName = "EligMthdtlDS"
        Me._MemTypeDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'ExitButton
        '
        Me.ExitButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Yes
        Me.ExitButton.Location = New System.Drawing.Point(458, 347)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(60, 23)
        Me.ExitButton.TabIndex = 128
        Me.ExitButton.Text = "Exit"
        Me.ExitButton.UseVisualStyleBackColor = True
        '
        'UpdateMemtypeForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(530, 377)
        Me.Controls.Add(Me.ExitButton)
        Me.Controls.Add(Me.ModifyActionButton)
        Me.Controls.Add(Me.SaveActionButton)
        Me.Controls.Add(Me.CancelActionButton)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.controlGroupBox)
        Me.Name = "UpdateMemtypeForm"
        Me.Text = "Modify Memtype "
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.MemTypeDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.controlGroupBox.ResumeLayout(False)
        Me.controlGroupBox.PerformLayout()
        Me.cmbMemTypeTransparentContainer.ResumeLayout(False)
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._MemTypeDS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ModifyActionButton As System.Windows.Forms.Button
    Friend WithEvents SaveActionButton As System.Windows.Forms.Button
    Friend WithEvents CancelActionButton As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents MemTypeDataGrid As DataGridCustom
    Friend WithEvents controlGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtFamilyID As System.Windows.Forms.TextBox
    Friend WithEvents txtRelationID As System.Windows.Forms.TextBox
    Friend WithEvents txtEligibiltyMonth As System.Windows.Forms.TextBox
    Friend WithEvents FromLabel As System.Windows.Forms.Label
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents _MemTypeDS As EligMthdtlDS
    Friend WithEvents ExitButton As Button
    Friend WithEvents HistoryButton As Button
    Friend WithEvents cmbMemTypeTransparentContainer As TransparentContainer
    Friend WithEvents cmbMemType As ExComboBox
End Class
