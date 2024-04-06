<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EligAcctHrsMaintForm
    Inherits System.Windows.Forms.Form

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EligAcctHrsMaintForm))
        Me.grpEditPanel = New System.Windows.Forms.GroupBox()
        Me.lblHours = New System.Windows.Forms.Label()
        Me.txtDays = New System.Windows.Forms.TextBox()
        Me.lblmessage = New System.Windows.Forms.Label()
        Me.txtHours = New System.Windows.Forms.TextBox()
        Me.lbldays = New System.Windows.Forms.Label()
        Me.txtSpecialRemarks = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbSpecialAcctTC = New TransparentContainer()
        Me.cmbSpecialAcct = New ExComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.eligDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtFamilyID = New System.Windows.Forms.TextBox()
        Me.txtRelationID = New System.Windows.Forms.TextBox()
        Me.lbleligible = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.EligSpecialAcctDataGrid = New DataGridCustom()
        Me.DeleteActionButton = New System.Windows.Forms.Button()
        Me.ModifyActionButton = New System.Windows.Forms.Button()
        Me.AddActionButton = New System.Windows.Forms.Button()
        Me.SaveActionButton = New System.Windows.Forms.Button()
        Me.CancelActionButton = New System.Windows.Forms.Button()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me._EAHDS = New EligAcctHoursDS()
        Me.CalculateEligibilityActionButton = New System.Windows.Forms.Button()
        Me.grpSelectPanel = New System.Windows.Forms.GroupBox()
        Me.eligDateTimePickerTC = New TransparentContainer()
        Me.ExitButton = New System.Windows.Forms.Button()
        Me.HistoryButton = New System.Windows.Forms.Button()
        Me.grpEditPanel.SuspendLayout()
        Me.cmbSpecialAcctTC.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.EligSpecialAcctDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._EAHDS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpSelectPanel.SuspendLayout()
        Me.eligDateTimePickerTC.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpEditPanel
        '
        Me.grpEditPanel.Controls.Add(Me.lblHours)
        Me.grpEditPanel.Controls.Add(Me.txtDays)
        Me.grpEditPanel.Controls.Add(Me.lblmessage)
        Me.grpEditPanel.Controls.Add(Me.txtHours)
        Me.grpEditPanel.Controls.Add(Me.lbldays)
        Me.grpEditPanel.Controls.Add(Me.txtSpecialRemarks)
        Me.grpEditPanel.Controls.Add(Me.Label5)
        Me.grpEditPanel.Controls.Add(Me.Label4)
        Me.grpEditPanel.Controls.Add(Me.cmbSpecialAcctTC)
        Me.grpEditPanel.Location = New System.Drawing.Point(2, 50)
        Me.grpEditPanel.Name = "grpEditPanel"
        Me.grpEditPanel.Size = New System.Drawing.Size(603, 140)
        Me.grpEditPanel.TabIndex = 102
        Me.grpEditPanel.TabStop = False
        '
        'lblHours
        '
        Me.lblHours.AutoSize = True
        Me.lblHours.Location = New System.Drawing.Point(415, 40)
        Me.lblHours.Name = "lblHours"
        Me.lblHours.Size = New System.Drawing.Size(35, 13)
        Me.lblHours.TabIndex = 111
        Me.lblHours.Text = "Hours"
        Me.lblHours.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtDays
        '
        Me.txtDays.Location = New System.Drawing.Point(539, 37)
        Me.txtDays.MaxLength = 2
        Me.txtDays.Name = "txtDays"
        Me.txtDays.Size = New System.Drawing.Size(40, 20)
        Me.txtDays.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.txtDays, "No of days for some special accounts")
        '
        'lblmessage
        '
        Me.lblmessage.AutoSize = True
        Me.lblmessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblmessage.ForeColor = System.Drawing.Color.Red
        Me.lblmessage.Location = New System.Drawing.Point(132, 14)
        Me.lblmessage.Name = "lblmessage"
        Me.lblmessage.Size = New System.Drawing.Size(0, 13)
        Me.lblmessage.TabIndex = 110
        Me.lblmessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ToolTip1.SetToolTip(Me.lblmessage, "Eligibility Period you are working on")
        '
        'txtHours
        '
        Me.txtHours.Location = New System.Drawing.Point(456, 37)
        Me.txtHours.MaxLength = 5
        Me.txtHours.Name = "txtHours"
        Me.txtHours.Size = New System.Drawing.Size(40, 20)
        Me.txtHours.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.txtHours, "number of hours for some accounts")
        '
        'lbldays
        '
        Me.lbldays.AutoSize = True
        Me.lbldays.Location = New System.Drawing.Point(502, 40)
        Me.lbldays.Name = "lbldays"
        Me.lbldays.Size = New System.Drawing.Size(31, 13)
        Me.lbldays.TabIndex = 108
        Me.lbldays.Text = "Days"
        Me.lbldays.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtSpecialRemarks
        '
        Me.txtSpecialRemarks.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSpecialRemarks.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSpecialRemarks.Location = New System.Drawing.Point(135, 63)
        Me.txtSpecialRemarks.MaxLength = 295
        Me.txtSpecialRemarks.Multiline = True
        Me.txtSpecialRemarks.Name = "txtSpecialRemarks"
        Me.txtSpecialRemarks.Size = New System.Drawing.Size(319, 65)
        Me.txtSpecialRemarks.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.txtSpecialRemarks, "Reason for adding Eligibility special account")
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(62, 63)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(49, 13)
        Me.Label5.TabIndex = 106
        Me.Label5.Text = "Remarks"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(26, 40)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(85, 13)
        Me.Label4.TabIndex = 105
        Me.Label4.Text = "Special Account"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbSpecialAcctTC
        '
        Me.cmbSpecialAcctTC.Controls.Add(Me.cmbSpecialAcct)
        Me.cmbSpecialAcctTC.Location = New System.Drawing.Point(135, 36)
        Me.cmbSpecialAcctTC.Name = "cmbSpecialAcctTC"
        Me.cmbSpecialAcctTC.Size = New System.Drawing.Size(290, 21)
        Me.cmbSpecialAcctTC.TabIndex = 0
        '
        'cmbSpecialAcct
        '
        Me.cmbSpecialAcct.DropDownHeight = 100
        Me.cmbSpecialAcct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSpecialAcct.DropDownWidth = 400
        Me.cmbSpecialAcct.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbSpecialAcct.FormattingEnabled = True
        Me.cmbSpecialAcct.IntegralHeight = False
        Me.cmbSpecialAcct.ItemHeight = 13
        Me.cmbSpecialAcct.Items.AddRange(New Object() {"HOURS"})
        Me.cmbSpecialAcct.Location = New System.Drawing.Point(0, 0)
        Me.cmbSpecialAcct.MaxLength = 2
        Me.cmbSpecialAcct.Name = "cmbSpecialAcct"
        Me.cmbSpecialAcct.ReadOnly = False
        Me.cmbSpecialAcct.Size = New System.Drawing.Size(266, 21)
        Me.cmbSpecialAcct.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.cmbSpecialAcct, "choose the special Account")
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(51, 19)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(79, 13)
        Me.Label3.TabIndex = 104
        Me.Label3.Text = "Eligibility Period"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'eligDateTimePicker
        '
        Me.eligDateTimePicker.CustomFormat = "MMM yyyy"
        Me.eligDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.eligDateTimePicker.Location = New System.Drawing.Point(0, 0)
        Me.eligDateTimePicker.Name = "eligDateTimePicker"
        Me.eligDateTimePicker.Size = New System.Drawing.Size(73, 20)
        Me.eligDateTimePicker.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.eligDateTimePicker, "Select Eligibility Period you want to enter Hours for.")
        Me.eligDateTimePicker.Value = New Date(1753, 1, 1, 0, 0, 0, 0)
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(447, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 13)
        Me.Label1.TabIndex = 100
        Me.Label1.Text = "Relation ID"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(284, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(27, 13)
        Me.Label2.TabIndex = 99
        Me.Label2.Text = "FID:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtFamilyID
        '
        Me.txtFamilyID.Enabled = False
        Me.txtFamilyID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFamilyID.Location = New System.Drawing.Point(315, 14)
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
        Me.txtRelationID.Location = New System.Drawing.Point(513, 14)
        Me.txtRelationID.MaxLength = 10
        Me.txtRelationID.Name = "txtRelationID"
        Me.txtRelationID.ReadOnly = True
        Me.txtRelationID.Size = New System.Drawing.Size(38, 20)
        Me.txtRelationID.TabIndex = 98
        '
        'lbleligible
        '
        Me.lbleligible.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbleligible.ForeColor = System.Drawing.Color.Red
        Me.lbleligible.Location = New System.Drawing.Point(691, 202)
        Me.lbleligible.Name = "lbleligible"
        Me.lbleligible.Size = New System.Drawing.Size(575, 23)
        Me.lbleligible.TabIndex = 112
        Me.lbleligible.Visible = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.EligSpecialAcctDataGrid)
        Me.GroupBox2.Location = New System.Drawing.Point(2, 230)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(1327, 255)
        Me.GroupBox2.TabIndex = 103
        Me.GroupBox2.TabStop = False
        '
        'EligSpecialAcctDataGrid
        '
        Me.EligSpecialAcctDataGrid.ADGroupsThatCanCopy = ""
        Me.EligSpecialAcctDataGrid.ADGroupsThatCanCustomize = ""
        Me.EligSpecialAcctDataGrid.ADGroupsThatCanExport = ""
        Me.EligSpecialAcctDataGrid.ADGroupsThatCanFilter = ""
        Me.EligSpecialAcctDataGrid.ADGroupsThatCanFind = ""
        Me.EligSpecialAcctDataGrid.ADGroupsThatCanMultiSort = ""
        Me.EligSpecialAcctDataGrid.ADGroupsThatCanPrint = ""
        Me.EligSpecialAcctDataGrid.AllowAutoSize = True
        Me.EligSpecialAcctDataGrid.AllowColumnReorder = True
        Me.EligSpecialAcctDataGrid.AllowCopy = True
        Me.EligSpecialAcctDataGrid.AllowCustomize = True
        Me.EligSpecialAcctDataGrid.AllowDelete = False
        Me.EligSpecialAcctDataGrid.AllowDragDrop = False
        Me.EligSpecialAcctDataGrid.AllowEdit = False
        Me.EligSpecialAcctDataGrid.AllowExport = False
        Me.EligSpecialAcctDataGrid.AllowFilter = True
        Me.EligSpecialAcctDataGrid.AllowFind = True
        Me.EligSpecialAcctDataGrid.AllowGoTo = True
        Me.EligSpecialAcctDataGrid.AllowMultiSelect = False
        Me.EligSpecialAcctDataGrid.AllowMultiSort = False
        Me.EligSpecialAcctDataGrid.AllowNew = False
        Me.EligSpecialAcctDataGrid.AllowPrint = True
        Me.EligSpecialAcctDataGrid.AllowRefresh = False
        Me.EligSpecialAcctDataGrid.AppKey = "UFCW\RegMaster\"
        Me.EligSpecialAcctDataGrid.AutoSaveCols = True
        Me.EligSpecialAcctDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.EligSpecialAcctDataGrid.CaptionForeColor = System.Drawing.SystemColors.InfoText
        Me.EligSpecialAcctDataGrid.ColumnHeaderLabel = Nothing
        Me.EligSpecialAcctDataGrid.ColumnRePositioning = False
        Me.EligSpecialAcctDataGrid.ColumnResizing = False
        Me.EligSpecialAcctDataGrid.ConfirmDelete = True
        Me.EligSpecialAcctDataGrid.CopySelectedOnly = True
        Me.EligSpecialAcctDataGrid.CurrentBSPosition = -1
        Me.EligSpecialAcctDataGrid.DataMember = ""
        Me.EligSpecialAcctDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EligSpecialAcctDataGrid.DragColumn = 0
        Me.EligSpecialAcctDataGrid.ExportSelectedOnly = True
        Me.EligSpecialAcctDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.EligSpecialAcctDataGrid.HighlightedRow = Nothing
        Me.EligSpecialAcctDataGrid.HighLightModifiedRows = False
        Me.EligSpecialAcctDataGrid.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.EligSpecialAcctDataGrid.IsMouseDown = False
        Me.EligSpecialAcctDataGrid.LastGoToLine = ""
        Me.EligSpecialAcctDataGrid.Location = New System.Drawing.Point(3, 16)
        Me.EligSpecialAcctDataGrid.MultiSort = False
        Me.EligSpecialAcctDataGrid.Name = "EligSpecialAcctDataGrid"
        Me.EligSpecialAcctDataGrid.OldSelectedRow = 0
        Me.EligSpecialAcctDataGrid.PreviousBSPosition = -1
        Me.EligSpecialAcctDataGrid.ReadOnly = True
        Me.EligSpecialAcctDataGrid.RetainRowSelectionAfterSort = True
        Me.EligSpecialAcctDataGrid.SetRowOnRightClick = True
        Me.EligSpecialAcctDataGrid.ShiftPressed = False
        Me.EligSpecialAcctDataGrid.SingleClickBooleanColumns = True
        Me.EligSpecialAcctDataGrid.Size = New System.Drawing.Size(1321, 236)
        Me.EligSpecialAcctDataGrid.Sort = Nothing
        Me.EligSpecialAcctDataGrid.StyleName = ""
        Me.EligSpecialAcctDataGrid.SubKey = ""
        Me.EligSpecialAcctDataGrid.SuppressMouseDown = False
        Me.EligSpecialAcctDataGrid.SuppressTriangle = False
        Me.EligSpecialAcctDataGrid.TabIndex = 103
        Me.EligSpecialAcctDataGrid.TabStop = False
        '
        'DeleteActionButton
        '
        Me.DeleteActionButton.Location = New System.Drawing.Point(142, 199)
        Me.DeleteActionButton.Name = "DeleteActionButton"
        Me.DeleteActionButton.Size = New System.Drawing.Size(60, 23)
        Me.DeleteActionButton.TabIndex = 8
        Me.DeleteActionButton.Text = "Delete"
        Me.DeleteActionButton.UseVisualStyleBackColor = True
        '
        'ModifyActionButton
        '
        Me.ModifyActionButton.Location = New System.Drawing.Point(75, 199)
        Me.ModifyActionButton.Name = "ModifyActionButton"
        Me.ModifyActionButton.Size = New System.Drawing.Size(60, 23)
        Me.ModifyActionButton.TabIndex = 7
        Me.ModifyActionButton.Text = "Modify"
        Me.ModifyActionButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ModifyActionButton.UseVisualStyleBackColor = True
        '
        'AddActionButton
        '
        Me.AddActionButton.Location = New System.Drawing.Point(9, 199)
        Me.AddActionButton.Name = "AddActionButton"
        Me.AddActionButton.Size = New System.Drawing.Size(60, 23)
        Me.AddActionButton.TabIndex = 6
        Me.AddActionButton.Text = "Add"
        Me.AddActionButton.UseVisualStyleBackColor = True
        '
        'SaveActionButton
        '
        Me.SaveActionButton.CausesValidation = False
        Me.SaveActionButton.Enabled = False
        Me.SaveActionButton.Location = New System.Drawing.Point(314, 199)
        Me.SaveActionButton.Name = "SaveActionButton"
        Me.SaveActionButton.Size = New System.Drawing.Size(60, 23)
        Me.SaveActionButton.TabIndex = 10
        Me.SaveActionButton.Text = "Save"
        Me.SaveActionButton.UseVisualStyleBackColor = True
        '
        'CancelActionButton
        '
        Me.CancelActionButton.CausesValidation = False
        Me.CancelActionButton.Location = New System.Drawing.Point(248, 199)
        Me.CancelActionButton.Name = "CancelActionButton"
        Me.CancelActionButton.Size = New System.Drawing.Size(60, 23)
        Me.CancelActionButton.TabIndex = 9
        Me.CancelActionButton.Text = "Cancel"
        Me.CancelActionButton.UseVisualStyleBackColor = True
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        '_EAHDS
        '
        Me._EAHDS.DataSetName = "EligAcctHoursDS"
        Me._EAHDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'CalculateEligibilityActionButton
        '
        Me.CalculateEligibilityActionButton.CausesValidation = False
        Me.CalculateEligibilityActionButton.Location = New System.Drawing.Point(441, 199)
        Me.CalculateEligibilityActionButton.Name = "CalculateEligibilityActionButton"
        Me.CalculateEligibilityActionButton.Size = New System.Drawing.Size(147, 23)
        Me.CalculateEligibilityActionButton.TabIndex = 11
        Me.CalculateEligibilityActionButton.Text = "Calculate Eligibility"
        Me.CalculateEligibilityActionButton.UseVisualStyleBackColor = True
        '
        'grpSelectPanel
        '
        Me.grpSelectPanel.Controls.Add(Me.eligDateTimePickerTC)
        Me.grpSelectPanel.Controls.Add(Me.txtFamilyID)
        Me.grpSelectPanel.Controls.Add(Me.txtRelationID)
        Me.grpSelectPanel.Controls.Add(Me.Label2)
        Me.grpSelectPanel.Controls.Add(Me.Label1)
        Me.grpSelectPanel.Controls.Add(Me.Label3)
        Me.grpSelectPanel.Location = New System.Drawing.Point(8, 7)
        Me.grpSelectPanel.Name = "grpSelectPanel"
        Me.grpSelectPanel.Size = New System.Drawing.Size(597, 46)
        Me.grpSelectPanel.TabIndex = 113
        Me.grpSelectPanel.TabStop = False
        '
        'eligDateTimePickerTC
        '
        Me.eligDateTimePickerTC.Controls.Add(Me.eligDateTimePicker)
        Me.eligDateTimePickerTC.Location = New System.Drawing.Point(140, 15)
        Me.eligDateTimePickerTC.Name = "eligDateTimePickerTC"
        Me.eligDateTimePickerTC.Size = New System.Drawing.Size(93, 20)
        Me.eligDateTimePickerTC.TabIndex = 0
        '
        'ExitButton
        '
        Me.ExitButton.CausesValidation = False
        Me.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ExitButton.Location = New System.Drawing.Point(623, 199)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(60, 23)
        Me.ExitButton.TabIndex = 12
        Me.ExitButton.Text = "Exit"
        Me.ExitButton.UseVisualStyleBackColor = True
        '
        'HistoryButton
        '
        Me.HistoryButton.CausesValidation = False
        Me.HistoryButton.Location = New System.Drawing.Point(623, 19)
        Me.HistoryButton.Name = "HistoryButton"
        Me.HistoryButton.Size = New System.Drawing.Size(60, 23)
        Me.HistoryButton.TabIndex = 13
        Me.HistoryButton.Text = "History"
        Me.HistoryButton.UseVisualStyleBackColor = True
        '
        'EligAcctHrsMaintForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.CancelButton = Me.ExitButton
        Me.ClientSize = New System.Drawing.Size(1329, 490)
        Me.Controls.Add(Me.HistoryButton)
        Me.Controls.Add(Me.ExitButton)
        Me.Controls.Add(Me.grpSelectPanel)
        Me.Controls.Add(Me.lbleligible)
        Me.Controls.Add(Me.CalculateEligibilityActionButton)
        Me.Controls.Add(Me.DeleteActionButton)
        Me.Controls.Add(Me.ModifyActionButton)
        Me.Controls.Add(Me.AddActionButton)
        Me.Controls.Add(Me.SaveActionButton)
        Me.Controls.Add(Me.CancelActionButton)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.grpEditPanel)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimumSize = New System.Drawing.Size(726, 528)
        Me.Name = "EligAcctHrsMaintForm"
        Me.Text = "Eligibilty Special Account Hours Maintenance"
        Me.grpEditPanel.ResumeLayout(False)
        Me.grpEditPanel.PerformLayout()
        Me.cmbSpecialAcctTC.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.EligSpecialAcctDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._EAHDS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpSelectPanel.ResumeLayout(False)
        Me.grpSelectPanel.PerformLayout()
        Me.eligDateTimePickerTC.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpEditPanel As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents eligDateTimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtFamilyID As System.Windows.Forms.TextBox
    Friend WithEvents txtRelationID As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmbSpecialAcct As ExComboBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents EligSpecialAcctDataGrid As DataGridCustom
    Friend WithEvents DeleteActionButton As System.Windows.Forms.Button
    Friend WithEvents ModifyActionButton As System.Windows.Forms.Button
    Friend WithEvents AddActionButton As System.Windows.Forms.Button
    Friend WithEvents SaveActionButton As System.Windows.Forms.Button
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents _EAHDS As EligAcctHoursDS
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtSpecialRemarks As System.Windows.Forms.TextBox
    Friend WithEvents txtHours As System.Windows.Forms.TextBox
    Friend WithEvents lbldays As System.Windows.Forms.Label
    Friend WithEvents lblmessage As System.Windows.Forms.Label
    Friend WithEvents txtDays As System.Windows.Forms.TextBox
    Friend WithEvents lblHours As System.Windows.Forms.Label
    Friend WithEvents CancelActionButton As System.Windows.Forms.Button
    Friend WithEvents CalculateEligibilityActionButton As System.Windows.Forms.Button
    Friend WithEvents lbleligible As System.Windows.Forms.Label
    Friend WithEvents grpSelectPanel As System.Windows.Forms.GroupBox
    Friend WithEvents ExitButton As Windows.Forms.Button
    Friend WithEvents HistoryButton As Windows.Forms.Button
    Friend WithEvents cmbSpecialAcctTC As TransparentContainer
    Friend WithEvents eligDateTimePickerTC As TransparentContainer
End Class
