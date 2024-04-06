<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TerminationsControl
    Inherits System.Windows.Forms.UserControl

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.AddActionButton = New System.Windows.Forms.Button()
        Me.SaveActionButton = New System.Windows.Forms.Button()
        Me.CancelActionButton = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.TerminationLookupDataGrid = New DataGridCustom()
        Me.grpEditPanel = New System.Windows.Forms.GroupBox()
        Me.cmbCobraLetterTC = New TransparentContainer()
        Me.cmbCobraLetter = New ExComboBox()
        Me.HistoryButton = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbRetirementTC = New TransparentContainer()
        Me.cmbRetirement = New ExComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtMemName = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtPartSSN = New ExTextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtFamilyID = New ExTextBox()
        Me.txtTermDt = New System.Windows.Forms.TextBox()
        Me.FromLabel = New System.Windows.Forms.Label()
        Me.RemoveActionButton = New System.Windows.Forms.Button()
        Me._TDS = New TerminationsDS()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.lblCurrentStatus = New System.Windows.Forms.Label()
        Me.GroupBox2.SuspendLayout()
        CType(Me.TerminationLookupDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpEditPanel.SuspendLayout()
        Me.cmbCobraLetterTC.SuspendLayout()
        Me.cmbRetirementTC.SuspendLayout()
        CType(Me._TDS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AddActionButton
        '
        Me.AddActionButton.Location = New System.Drawing.Point(9, 157)
        Me.AddActionButton.Name = "AddActionButton"
        Me.AddActionButton.Size = New System.Drawing.Size(60, 23)
        Me.AddActionButton.TabIndex = 4
        Me.AddActionButton.Text = "Add"
        Me.AddActionButton.UseVisualStyleBackColor = True
        '
        'SaveActionButton
        '
        Me.SaveActionButton.CausesValidation = False
        Me.SaveActionButton.Enabled = False
        Me.SaveActionButton.Location = New System.Drawing.Point(357, 157)
        Me.SaveActionButton.Name = "SaveActionButton"
        Me.SaveActionButton.Size = New System.Drawing.Size(60, 23)
        Me.SaveActionButton.TabIndex = 8
        Me.SaveActionButton.Text = "Save"
        Me.SaveActionButton.UseVisualStyleBackColor = True
        '
        'CancelActionButton
        '
        Me.CancelActionButton.CausesValidation = False
        Me.CancelActionButton.Location = New System.Drawing.Point(291, 157)
        Me.CancelActionButton.Name = "CancelActionButton"
        Me.CancelActionButton.Size = New System.Drawing.Size(60, 23)
        Me.CancelActionButton.TabIndex = 7
        Me.CancelActionButton.Text = "Cancel"
        Me.CancelActionButton.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.TerminationLookupDataGrid)
        Me.GroupBox2.Location = New System.Drawing.Point(3, 186)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(461, 254)
        Me.GroupBox2.TabIndex = 121
        Me.GroupBox2.TabStop = False
        '
        'TerminationLookupDataGrid
        '
        Me.TerminationLookupDataGrid.ADGroupsThatCanCopy = ""
        Me.TerminationLookupDataGrid.ADGroupsThatCanCustomize = ""
        Me.TerminationLookupDataGrid.ADGroupsThatCanExport = ""
        Me.TerminationLookupDataGrid.ADGroupsThatCanFilter = ""
        Me.TerminationLookupDataGrid.ADGroupsThatCanFind = ""
        Me.TerminationLookupDataGrid.ADGroupsThatCanMultiSort = ""
        Me.TerminationLookupDataGrid.ADGroupsThatCanPrint = ""
        Me.TerminationLookupDataGrid.AllowAutoSize = True
        Me.TerminationLookupDataGrid.AllowColumnReorder = True
        Me.TerminationLookupDataGrid.AllowCopy = True
        Me.TerminationLookupDataGrid.AllowCustomize = True
        Me.TerminationLookupDataGrid.AllowDelete = False
        Me.TerminationLookupDataGrid.AllowDragDrop = False
        Me.TerminationLookupDataGrid.AllowEdit = False
        Me.TerminationLookupDataGrid.AllowExport = False
        Me.TerminationLookupDataGrid.AllowFilter = True
        Me.TerminationLookupDataGrid.AllowFind = True
        Me.TerminationLookupDataGrid.AllowGoTo = True
        Me.TerminationLookupDataGrid.AllowMultiSelect = False
        Me.TerminationLookupDataGrid.AllowMultiSort = False
        Me.TerminationLookupDataGrid.AllowNew = False
        Me.TerminationLookupDataGrid.AllowPrint = True
        Me.TerminationLookupDataGrid.AllowRefresh = True
        Me.TerminationLookupDataGrid.AppKey = "UFCW\RegMaster\"
        Me.TerminationLookupDataGrid.AutoSaveCols = True
        Me.TerminationLookupDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.TerminationLookupDataGrid.ColumnHeaderLabel = Nothing
        Me.TerminationLookupDataGrid.ColumnRePositioning = False
        Me.TerminationLookupDataGrid.ColumnResizing = False
        Me.TerminationLookupDataGrid.ConfirmDelete = True
        Me.TerminationLookupDataGrid.CopySelectedOnly = True
        Me.TerminationLookupDataGrid.CurrentBSPosition = -1
        Me.TerminationLookupDataGrid.DataMember = ""
        Me.TerminationLookupDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TerminationLookupDataGrid.DragColumn = 0
        Me.TerminationLookupDataGrid.ExportSelectedOnly = True
        Me.TerminationLookupDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.TerminationLookupDataGrid.HighlightedRow = Nothing
        Me.TerminationLookupDataGrid.HighLightModifiedRows = False
        Me.TerminationLookupDataGrid.IsMouseDown = False
        Me.TerminationLookupDataGrid.LastGoToLine = ""
        Me.TerminationLookupDataGrid.Location = New System.Drawing.Point(3, 16)
        Me.TerminationLookupDataGrid.MultiSort = False
        Me.TerminationLookupDataGrid.Name = "TerminationLookupDataGrid"
        Me.TerminationLookupDataGrid.OldSelectedRow = 0
        Me.TerminationLookupDataGrid.PreviousBSPosition = -1
        Me.TerminationLookupDataGrid.ReadOnly = True
        Me.TerminationLookupDataGrid.RetainRowSelectionAfterSort = True
        Me.TerminationLookupDataGrid.SetRowOnRightClick = True
        Me.TerminationLookupDataGrid.ShiftPressed = False
        Me.TerminationLookupDataGrid.SingleClickBooleanColumns = True
        Me.TerminationLookupDataGrid.Size = New System.Drawing.Size(455, 235)
        Me.TerminationLookupDataGrid.Sort = Nothing
        Me.TerminationLookupDataGrid.StyleName = ""
        Me.TerminationLookupDataGrid.SubKey = ""
        Me.TerminationLookupDataGrid.SuppressMouseDown = False
        Me.TerminationLookupDataGrid.SuppressTriangle = False
        Me.TerminationLookupDataGrid.TabIndex = 103
        '
        'grpEditPanel
        '
        Me.grpEditPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpEditPanel.Controls.Add(Me.cmbCobraLetterTC)
        Me.grpEditPanel.Controls.Add(Me.HistoryButton)
        Me.grpEditPanel.Controls.Add(Me.Label5)
        Me.grpEditPanel.Controls.Add(Me.cmbRetirementTC)
        Me.grpEditPanel.Controls.Add(Me.Label4)
        Me.grpEditPanel.Controls.Add(Me.Label1)
        Me.grpEditPanel.Controls.Add(Me.txtMemName)
        Me.grpEditPanel.Controls.Add(Me.Label3)
        Me.grpEditPanel.Controls.Add(Me.txtPartSSN)
        Me.grpEditPanel.Controls.Add(Me.Label2)
        Me.grpEditPanel.Controls.Add(Me.txtFamilyID)
        Me.grpEditPanel.Controls.Add(Me.txtTermDt)
        Me.grpEditPanel.Controls.Add(Me.FromLabel)
        Me.grpEditPanel.Location = New System.Drawing.Point(3, 3)
        Me.grpEditPanel.Name = "grpEditPanel"
        Me.grpEditPanel.Size = New System.Drawing.Size(459, 148)
        Me.grpEditPanel.TabIndex = 0
        Me.grpEditPanel.TabStop = False
        '
        'cmbCobraLetterTC
        '
        Me.cmbCobraLetterTC.Controls.Add(Me.cmbCobraLetter)
        Me.cmbCobraLetterTC.Location = New System.Drawing.Point(106, 114)
        Me.cmbCobraLetterTC.Name = "cmbCobraLetterTC"
        Me.cmbCobraLetterTC.Size = New System.Drawing.Size(57, 21)
        Me.cmbCobraLetterTC.TabIndex = 0
        '
        'cmbCobraLetter
        '
        Me.cmbCobraLetter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCobraLetter.DropDownWidth = 50
        Me.cmbCobraLetter.FormattingEnabled = True
        Me.cmbCobraLetter.Items.AddRange(New Object() {"N", "Y"})
        Me.cmbCobraLetter.Location = New System.Drawing.Point(0, 0)
        Me.cmbCobraLetter.Name = "cmbCobraLetter"
        Me.cmbCobraLetter.ReadOnly = False
        Me.cmbCobraLetter.Size = New System.Drawing.Size(42, 21)
        Me.cmbCobraLetter.TabIndex = 2
        '
        'HistoryButton
        '
        Me.HistoryButton.CausesValidation = False
        Me.HistoryButton.Enabled = False
        Me.HistoryButton.Location = New System.Drawing.Point(393, 12)
        Me.HistoryButton.Name = "HistoryButton"
        Me.HistoryButton.Size = New System.Drawing.Size(60, 23)
        Me.HistoryButton.TabIndex = 108
        Me.HistoryButton.Text = "History"
        Me.HistoryButton.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(0, 114)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(102, 13)
        Me.Label5.TabIndex = 107
        Me.Label5.Text = "Send COBRA Letter"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbRetirementTC
        '
        Me.cmbRetirementTC.Controls.Add(Me.cmbRetirement)
        Me.cmbRetirementTC.Location = New System.Drawing.Point(106, 90)
        Me.cmbRetirementTC.Name = "cmbRetirementTC"
        Me.cmbRetirementTC.Size = New System.Drawing.Size(57, 21)
        Me.cmbRetirementTC.TabIndex = 0
        '
        'cmbRetirement
        '
        Me.cmbRetirement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbRetirement.DropDownWidth = 50
        Me.cmbRetirement.FormattingEnabled = True
        Me.cmbRetirement.Items.AddRange(New Object() {"N", "Y"})
        Me.cmbRetirement.Location = New System.Drawing.Point(0, 0)
        Me.cmbRetirement.Name = "cmbRetirement"
        Me.cmbRetirement.ReadOnly = False
        Me.cmbRetirement.Size = New System.Drawing.Size(42, 21)
        Me.cmbRetirement.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(2, 90)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(100, 13)
        Me.Label4.TabIndex = 105
        Me.Label4.Text = "Retirement Pending"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 42)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 13)
        Me.Label1.TabIndex = 104
        Me.Label1.Text = "Participant Name"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtMemName
        '
        Me.txtMemName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMemName.Location = New System.Drawing.Point(106, 39)
        Me.txtMemName.MaxLength = 15
        Me.txtMemName.Name = "txtMemName"
        Me.txtMemName.ReadOnly = True
        Me.txtMemName.Size = New System.Drawing.Size(268, 20)
        Me.txtMemName.TabIndex = 103
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(231, 17)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(29, 13)
        Me.Label3.TabIndex = 102
        Me.Label3.Text = "SSN"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtPartSSN
        '
        Me.txtPartSSN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPartSSN.Location = New System.Drawing.Point(283, 14)
        Me.txtPartSSN.MaxLength = 15
        Me.txtPartSSN.Name = "txtPartSSN"
        Me.txtPartSSN.ReadOnly = True
        Me.txtPartSSN.Size = New System.Drawing.Size(91, 20)
        Me.txtPartSSN.TabIndex = 101
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(75, 17)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(27, 13)
        Me.Label2.TabIndex = 99
        Me.Label2.Text = "FID:"
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
        Me.txtFamilyID.TabIndex = 97
        '
        'txtTermDt
        '
        Me.txtTermDt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTermDt.Location = New System.Drawing.Point(106, 65)
        Me.txtTermDt.MaxLength = 10
        Me.txtTermDt.Name = "txtTermDt"
        Me.txtTermDt.Size = New System.Drawing.Size(91, 20)
        Me.txtTermDt.TabIndex = 0
        '
        'FromLabel
        '
        Me.FromLabel.AutoSize = True
        Me.FromLabel.Location = New System.Drawing.Point(45, 68)
        Me.FromLabel.Name = "FromLabel"
        Me.FromLabel.Size = New System.Drawing.Size(57, 13)
        Me.FromLabel.TabIndex = 93
        Me.FromLabel.Text = "Term Date"
        Me.FromLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'DeleteActionButton
        '
        Me.RemoveActionButton.Location = New System.Drawing.Point(75, 157)
        Me.RemoveActionButton.Name = "DeleteActionButton"
        Me.RemoveActionButton.Size = New System.Drawing.Size(60, 23)
        Me.RemoveActionButton.TabIndex = 5
        Me.RemoveActionButton.Text = "Remove"
        Me.RemoveActionButton.UseVisualStyleBackColor = True
        '
        '_TDS
        '
        Me._TDS.DataSetName = "TerminationsDS"
        Me._TDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'lblCurrentStatus
        '
        Me.lblCurrentStatus.AutoSize = True
        Me.lblCurrentStatus.Location = New System.Drawing.Point(150, 162)
        Me.lblCurrentStatus.Name = "lblCurrentStatus"
        Me.lblCurrentStatus.Size = New System.Drawing.Size(0, 13)
        Me.lblCurrentStatus.TabIndex = 122
        '
        'TerminationsControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.Controls.Add(Me.lblCurrentStatus)
        Me.Controls.Add(Me.RemoveActionButton)
        Me.Controls.Add(Me.AddActionButton)
        Me.Controls.Add(Me.SaveActionButton)
        Me.Controls.Add(Me.CancelActionButton)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.grpEditPanel)
        Me.Name = "TerminationsControl"
        Me.Size = New System.Drawing.Size(461, 443)
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.TerminationLookupDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpEditPanel.ResumeLayout(False)
        Me.grpEditPanel.PerformLayout()
        Me.cmbCobraLetterTC.ResumeLayout(False)
        Me.cmbRetirementTC.ResumeLayout(False)
        CType(Me._TDS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents AddActionButton As System.Windows.Forms.Button
    Friend WithEvents SaveActionButton As System.Windows.Forms.Button
    Friend WithEvents CancelActionButton As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents TerminationLookupDataGrid As DataGridCustom
    Friend WithEvents grpEditPanel As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtPartSSN As ExTextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtFamilyID As ExTextBox
    Friend WithEvents txtTermDt As System.Windows.Forms.TextBox
    Friend WithEvents FromLabel As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtMemName As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents RemoveActionButton As System.Windows.Forms.Button
    Friend WithEvents cmbRetirement As ExComboBox
    Friend WithEvents cmbCobraLetter As ExComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents _TDS As TerminationsDS
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents HistoryButton As Windows.Forms.Button
    Friend WithEvents cmbCobraLetterTC As TransparentContainer
    Friend WithEvents cmbRetirementTC As TransparentContainer
    Friend WithEvents lblCurrentStatus As Windows.Forms.Label
End Class
