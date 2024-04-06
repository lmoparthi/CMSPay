<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AlertControl
    Inherits System.Windows.Forms.UserControl

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.grpEditPanel = New System.Windows.Forms.GroupBox()
        Me.txtComments = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbRelationID = New ExComboBox()
        Me.lblRelationID = New System.Windows.Forms.Label()
        Me.chkSameAlert = New System.Windows.Forms.CheckBox()
        Me.cmbTermReason = New ExComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbAlert = New ExComboBox()
        Me.Label55 = New System.Windows.Forms.Label()
        Me.txtThruDate = New System.Windows.Forms.TextBox()
        Me.txtFromDate = New System.Windows.Forms.TextBox()
        Me.txtpasscode = New System.Windows.Forms.TextBox()
        Me.FromLabel = New System.Windows.Forms.Label()
        Me.Label56 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TerminateEventButton = New System.Windows.Forms.Button()
        Me.AddEventButton = New System.Windows.Forms.Button()
        Me.CancelEventButton = New System.Windows.Forms.Button()
        Me.SaveEventButton = New System.Windows.Forms.Button()
        Me.HistoryButton = New System.Windows.Forms.Button()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.VoidEventButton = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ModifyEventButton = New System.Windows.Forms.Button()
        Me.AlertDataGrid = New DataGridCustom()
        Me.grpEditPanel.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AlertDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpEditPanel
        '
        Me.grpEditPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpEditPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.grpEditPanel.Controls.Add(Me.txtComments)
        Me.grpEditPanel.Controls.Add(Me.Label1)
        Me.grpEditPanel.Controls.Add(Me.cmbRelationID)
        Me.grpEditPanel.Controls.Add(Me.lblRelationID)
        Me.grpEditPanel.Controls.Add(Me.chkSameAlert)
        Me.grpEditPanel.Controls.Add(Me.cmbTermReason)
        Me.grpEditPanel.Controls.Add(Me.HistoryButton)
        Me.grpEditPanel.Controls.Add(Me.Label2)
        Me.grpEditPanel.Controls.Add(Me.cmbAlert)
        Me.grpEditPanel.Controls.Add(Me.Label55)
        Me.grpEditPanel.Controls.Add(Me.txtThruDate)
        Me.grpEditPanel.Controls.Add(Me.txtFromDate)
        Me.grpEditPanel.Controls.Add(Me.txtpasscode)
        Me.grpEditPanel.Controls.Add(Me.FromLabel)
        Me.grpEditPanel.Controls.Add(Me.Label56)
        Me.grpEditPanel.Controls.Add(Me.Label3)
        Me.grpEditPanel.Location = New System.Drawing.Point(5, 6)
        Me.grpEditPanel.Name = "grpEditPanel"
        Me.grpEditPanel.Size = New System.Drawing.Size(653, 218)
        Me.grpEditPanel.TabIndex = 307
        Me.grpEditPanel.TabStop = False
        Me.grpEditPanel.Text = "Alert Information"
        '
        'txtComments
        '
        Me.txtComments.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtComments.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtComments.Location = New System.Drawing.Point(78, 163)
        Me.txtComments.MaxLength = 150
        Me.txtComments.Multiline = True
        Me.txtComments.Name = "txtComments"
        Me.txtComments.Size = New System.Drawing.Size(319, 42)
        Me.txtComments.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(12, 175)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(62, 23)
        Me.Label1.TabIndex = 128
        Me.Label1.Text = "Comments"
        '
        'cmbRelationID
        '
        Me.cmbRelationID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbRelationID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbRelationID.FormattingEnabled = True
        Me.cmbRelationID.Location = New System.Drawing.Point(78, 17)
        Me.cmbRelationID.Name = "cmbRelationID"
        Me.cmbRelationID.Size = New System.Drawing.Size(44, 21)
        Me.cmbRelationID.TabIndex = 126
        Me.ToolTip1.SetToolTip(Me.cmbRelationID, "Step Parent Relation ID")
        '
        'lblRelationID
        '
        Me.lblRelationID.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRelationID.Location = New System.Drawing.Point(8, 20)
        Me.lblRelationID.Name = "lblRelationID"
        Me.lblRelationID.Size = New System.Drawing.Size(65, 15)
        Me.lblRelationID.TabIndex = 125
        Me.lblRelationID.Text = " Relation ID"
        '
        'chkSameAlert
        '
        Me.chkSameAlert.AutoCheck = False
        Me.chkSameAlert.BackColor = System.Drawing.Color.MistyRose
        Me.chkSameAlert.Enabled = False
        Me.chkSameAlert.ForeColor = System.Drawing.Color.Red
        Me.chkSameAlert.Location = New System.Drawing.Point(212, 19)
        Me.chkSameAlert.Name = "chkSameAlert"
        Me.chkSameAlert.Size = New System.Drawing.Size(153, 17)
        Me.chkSameAlert.TabIndex = 97
        Me.chkSameAlert.Text = "Alert Same as Member"
        Me.chkSameAlert.UseVisualStyleBackColor = False
        Me.chkSameAlert.Visible = False
        '
        'cmbTermReason
        '
        Me.cmbTermReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTermReason.DropDownWidth = 170
        Me.cmbTermReason.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbTermReason.FormattingEnabled = True
        Me.cmbTermReason.Location = New System.Drawing.Point(79, 137)
        Me.cmbTermReason.MaxLength = 2
        Me.cmbTermReason.Name = "cmbTermReason"
        Me.cmbTermReason.ReadOnly = False
        Me.cmbTermReason.Size = New System.Drawing.Size(318, 21)
        Me.cmbTermReason.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(29, 133)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(49, 35)
        Me.Label2.TabIndex = 96
        Me.Label2.Text = "Void Reason"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbAlert
        '
        Me.cmbAlert.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbAlert.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbAlert.FormattingEnabled = True
        Me.cmbAlert.Location = New System.Drawing.Point(78, 41)
        Me.cmbAlert.MaxLength = 2
        Me.cmbAlert.Name = "cmbAlert"
        Me.cmbAlert.ReadOnly = False
        Me.cmbAlert.Size = New System.Drawing.Size(319, 21)
        Me.cmbAlert.TabIndex = 0
        '
        'Label55
        '
        Me.Label55.Location = New System.Drawing.Point(41, 44)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(28, 13)
        Me.Label55.TabIndex = 92
        Me.Label55.Text = "Alert"
        '
        'txtThruDt
        '
        Me.txtThruDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThruDate.Location = New System.Drawing.Point(270, 111)
        Me.txtThruDate.MaxLength = 10
        Me.txtThruDate.Name = "txtThruDate"
        Me.txtThruDate.ReadOnly = True
        Me.txtThruDate.Size = New System.Drawing.Size(91, 20)
        Me.txtThruDate.TabIndex = 3
        Me.txtThruDate.Text = "12-31-9999"
        '
        'txtFromDt
        '
        Me.txtFromDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFromDate.Location = New System.Drawing.Point(79, 111)
        Me.txtFromDate.MaxLength = 10
        Me.txtFromDate.Name = "txtFromDate"
        Me.txtFromDate.Size = New System.Drawing.Size(91, 20)
        Me.txtFromDate.TabIndex = 2
        '
        'txtpasscode
        '
        Me.txtpasscode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtpasscode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtpasscode.Location = New System.Drawing.Point(78, 65)
        Me.txtpasscode.MaxLength = 55
        Me.txtpasscode.Multiline = True
        Me.txtpasscode.Name = "txtpasscode"
        Me.txtpasscode.Size = New System.Drawing.Size(319, 42)
        Me.txtpasscode.TabIndex = 1
        '
        'FromLabel
        '
        Me.FromLabel.AutoSize = True
        Me.FromLabel.Location = New System.Drawing.Point(17, 116)
        Me.FromLabel.Name = "FromLabel"
        Me.FromLabel.Size = New System.Drawing.Size(56, 13)
        Me.FromLabel.TabIndex = 89
        Me.FromLabel.Text = "From Date"
        Me.FromLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label56
        '
        Me.Label56.Location = New System.Drawing.Point(12, 70)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(62, 37)
        Me.Label56.TabIndex = 2
        Me.Label56.Text = "Passcode / Remark"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(208, 113)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(57, 13)
        Me.Label3.TabIndex = 90
        Me.Label3.Text = "Term Date"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnTerminate
        '
        Me.TerminateEventButton.Location = New System.Drawing.Point(171, 231)
        Me.TerminateEventButton.Name = "TerminateEventButton"
        Me.TerminateEventButton.Size = New System.Drawing.Size(69, 23)
        Me.TerminateEventButton.TabIndex = 6
        Me.TerminateEventButton.Text = "Terminate"
        Me.ToolTip1.SetToolTip(Me.TerminateEventButton, "To end  the existing Alert by Providing TermDate")
        Me.TerminateEventButton.UseVisualStyleBackColor = True
        '
        'AddButton
        '
        Me.AddEventButton.BackColor = System.Drawing.SystemColors.Control
        Me.AddEventButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AddEventButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.AddEventButton.Location = New System.Drawing.Point(9, 231)
        Me.AddEventButton.Name = "AddEventButton"
        Me.AddEventButton.Size = New System.Drawing.Size(69, 23)
        Me.AddEventButton.TabIndex = 5
        Me.AddEventButton.Text = "Add"
        Me.ToolTip1.SetToolTip(Me.AddEventButton, "To Add new Alert")
        Me.AddEventButton.UseVisualStyleBackColor = True
        '
        'CancelButton
        '
        Me.CancelEventButton.Location = New System.Drawing.Point(333, 230)
        Me.CancelEventButton.Name = "CancelEventButton"
        Me.CancelEventButton.Size = New System.Drawing.Size(69, 23)
        Me.CancelEventButton.TabIndex = 9
        Me.CancelEventButton.Text = "Cancel"
        Me.ToolTip1.SetToolTip(Me.CancelEventButton, "Cancel the Alert Changes")
        Me.CancelEventButton.UseVisualStyleBackColor = True
        '
        'SaveButton
        '
        Me.SaveEventButton.Enabled = False
        Me.SaveEventButton.Location = New System.Drawing.Point(414, 230)
        Me.SaveEventButton.Name = "SaveEventButton"
        Me.SaveEventButton.Size = New System.Drawing.Size(69, 23)
        Me.SaveEventButton.TabIndex = 10
        Me.SaveEventButton.Text = "Save"
        Me.ToolTip1.SetToolTip(Me.SaveEventButton, "Saves the Alert Changes")
        Me.SaveEventButton.UseVisualStyleBackColor = True
        '
        'HistoryButton
        '
        Me.HistoryButton.Location = New System.Drawing.Point(409, 20)
        Me.HistoryButton.Name = "HistoryButton"
        Me.HistoryButton.Size = New System.Drawing.Size(69, 23)
        Me.HistoryButton.TabIndex = 8
        Me.HistoryButton.Text = "History"
        Me.ToolTip1.SetToolTip(Me.HistoryButton, "View all Alerts modification")
        Me.HistoryButton.UseVisualStyleBackColor = True
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'btnVoid
        '
        Me.VoidEventButton.Location = New System.Drawing.Point(252, 231)
        Me.VoidEventButton.Name = "VoidEventButton"
        Me.VoidEventButton.Size = New System.Drawing.Size(69, 23)
        Me.VoidEventButton.TabIndex = 7
        Me.VoidEventButton.Text = "Void"
        Me.ToolTip1.SetToolTip(Me.VoidEventButton, "To void the Alert (Like It never happened before  by providing Void Reason")
        Me.VoidEventButton.UseVisualStyleBackColor = True
        '
        'ToolTip1
        '
        Me.ToolTip1.AutoPopDelay = 5000
        Me.ToolTip1.InitialDelay = 500
        Me.ToolTip1.ReshowDelay = 100
        Me.ToolTip1.ShowAlways = True
        '
        'ModifyButton
        '
        Me.ModifyEventButton.BackColor = System.Drawing.SystemColors.Control
        Me.ModifyEventButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ModifyEventButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ModifyEventButton.Location = New System.Drawing.Point(90, 231)
        Me.ModifyEventButton.Name = "ModifyEventButton"
        Me.ModifyEventButton.Size = New System.Drawing.Size(69, 23)
        Me.ModifyEventButton.TabIndex = 308
        Me.ModifyEventButton.Text = "Modify"
        Me.ToolTip1.SetToolTip(Me.ModifyEventButton, "To Modify Alert")
        Me.ModifyEventButton.UseVisualStyleBackColor = True
        '
        'AlertDataGrid
        '
        Me.AlertDataGrid.ADGroupsThatCanCopy = ""
        Me.AlertDataGrid.ADGroupsThatCanCustomize = ""
        Me.AlertDataGrid.ADGroupsThatCanExport = ""
        Me.AlertDataGrid.ADGroupsThatCanFilter = ""
        Me.AlertDataGrid.ADGroupsThatCanFind = ""
        Me.AlertDataGrid.ADGroupsThatCanMultiSort = ""
        Me.AlertDataGrid.ADGroupsThatCanPrint = ""
        Me.AlertDataGrid.AllowAutoSize = True
        Me.AlertDataGrid.AllowColumnReorder = True
        Me.AlertDataGrid.AllowCopy = True
        Me.AlertDataGrid.AllowCustomize = True
        Me.AlertDataGrid.AllowDelete = False
        Me.AlertDataGrid.AllowDragDrop = False
        Me.AlertDataGrid.AllowEdit = False
        Me.AlertDataGrid.AllowExport = False
        Me.AlertDataGrid.AllowFilter = True
        Me.AlertDataGrid.AllowFind = True
        Me.AlertDataGrid.AllowGoTo = True
        Me.AlertDataGrid.AllowMultiSelect = False
        Me.AlertDataGrid.AllowMultiSort = False
        Me.AlertDataGrid.AllowNew = False
        Me.AlertDataGrid.AllowPrint = True
        Me.AlertDataGrid.AllowRefresh = True
        Me.AlertDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AlertDataGrid.AppKey = "UFCW\RegMaster\"
        Me.AlertDataGrid.AutoSaveCols = True
        Me.AlertDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.AlertDataGrid.ColumnHeaderLabel = Nothing
        Me.AlertDataGrid.ColumnRePositioning = False
        Me.AlertDataGrid.ColumnResizing = False
        Me.AlertDataGrid.ConfirmDelete = True
        Me.AlertDataGrid.CopySelectedOnly = True
        Me.AlertDataGrid.DataMember = ""
        Me.AlertDataGrid.DragColumn = 0
        Me.AlertDataGrid.ExportSelectedOnly = True
        Me.AlertDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.AlertDataGrid.HighlightedRow = Nothing
        Me.AlertDataGrid.HighLightModifiedRows = False
        Me.AlertDataGrid.IsMouseDown = False
        Me.AlertDataGrid.LastGoToLine = ""
        Me.AlertDataGrid.Location = New System.Drawing.Point(3, 262)
        Me.AlertDataGrid.MultiSort = False
        Me.AlertDataGrid.Name = "AlertDataGrid"
        Me.AlertDataGrid.OldSelectedRow = Nothing
        Me.AlertDataGrid.ReadOnly = True
        Me.AlertDataGrid.RetainRowSelectionAfterSort = True
        Me.AlertDataGrid.SetRowOnRightClick = True
        Me.AlertDataGrid.ShiftPressed = False
        Me.AlertDataGrid.SingleClickBooleanColumns = True
        Me.AlertDataGrid.Size = New System.Drawing.Size(655, 129)
        Me.AlertDataGrid.Sort = Nothing
        Me.AlertDataGrid.StyleName = ""
        Me.AlertDataGrid.SubKey = ""
        Me.AlertDataGrid.SuppressTriangle = False
        Me.AlertDataGrid.TabIndex = 11
        '
        'AlertControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ModifyEventButton)
        Me.Controls.Add(Me.VoidEventButton)
        Me.Controls.Add(Me.TerminateEventButton)
        Me.Controls.Add(Me.AddEventButton)
        Me.Controls.Add(Me.CancelEventButton)
        Me.Controls.Add(Me.SaveEventButton)
        Me.Controls.Add(Me.grpEditPanel)
        Me.Controls.Add(Me.AlertDataGrid)
        Me.Name = "AlertControl"
        Me.Size = New System.Drawing.Size(661, 390)
        Me.grpEditPanel.ResumeLayout(False)
        Me.grpEditPanel.PerformLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AlertDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpEditPanel As System.Windows.Forms.GroupBox
    Friend WithEvents txtThruDate As System.Windows.Forms.TextBox
    Friend WithEvents txtFromDate As System.Windows.Forms.TextBox
    Friend WithEvents txtpasscode As System.Windows.Forms.TextBox
    Friend WithEvents FromLabel As System.Windows.Forms.Label
    Friend WithEvents Label56 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents AlertDataGrid As DataGridCustom
    Friend WithEvents cmbAlert As ExComboBox
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents cmbTermReason As ExComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TerminateEventButton As System.Windows.Forms.Button
    Friend WithEvents AddEventButton As System.Windows.Forms.Button
    Friend WithEvents CancelEventButton As System.Windows.Forms.Button
    Friend WithEvents SaveEventButton As System.Windows.Forms.Button
    Friend WithEvents HistoryButton As System.Windows.Forms.Button
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents chkSameAlert As System.Windows.Forms.CheckBox
    Friend WithEvents VoidEventButton As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents ModifyEventButton As System.Windows.Forms.Button
    Friend WithEvents cmbRelationID As ExComboBox
    Friend WithEvents lblRelationID As System.Windows.Forms.Label
    Friend WithEvents txtComments As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label

End Class
