<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class EligCalcElementsControl
    Inherits System.Windows.Forms.UserControl

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.ModifyActionButton = New System.Windows.Forms.Button()
        Me.AddActionButton = New System.Windows.Forms.Button()
        Me.SaveActionButton = New System.Windows.Forms.Button()
        Me.CancelActionButton = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.ChecklistDataGrid = New DataGridCustom()
        Me.grpEditPanel = New System.Windows.Forms.GroupBox()
        Me.HistoryButton = New System.Windows.Forms.Button()
        Me.TransparentContainer2 = New TransparentContainer()
        Me.cmbLocal = New ExComboBox()
        Me.TransparentContainer1 = New TransparentContainer()
        Me.cmbMemType = New ExComboBox()
        Me.txtTermDate = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtfamilyid = New System.Windows.Forms.TextBox()
        Me.txtrelationid = New System.Windows.Forms.TextBox()
        Me.txtEntryDate = New System.Windows.Forms.TextBox()
        Me.FromLabel = New System.Windows.Forms.Label()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider()
        Me.GroupBox2.SuspendLayout()
        CType(Me.ChecklistDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpEditPanel.SuspendLayout()
        Me.TransparentContainer2.SuspendLayout()
        Me.TransparentContainer1.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ModifyActionButton
        '
        Me.ModifyActionButton.Location = New System.Drawing.Point(73, 158)
        Me.ModifyActionButton.Name = "ModifyActionButton"
        Me.ModifyActionButton.Size = New System.Drawing.Size(60, 23)
        Me.ModifyActionButton.TabIndex = 4
        Me.ModifyActionButton.Text = "Modify"
        Me.ModifyActionButton.UseVisualStyleBackColor = True
        '
        'AddActionButton
        '
        Me.AddActionButton.Location = New System.Drawing.Point(7, 158)
        Me.AddActionButton.Name = "AddActionButton"
        Me.AddActionButton.Size = New System.Drawing.Size(60, 23)
        Me.AddActionButton.TabIndex = 3
        Me.AddActionButton.Text = "Add"
        Me.AddActionButton.UseVisualStyleBackColor = True
        '
        'SaveActionButton
        '
        Me.SaveActionButton.CausesValidation = False
        Me.SaveActionButton.Enabled = False
        Me.SaveActionButton.Location = New System.Drawing.Point(260, 158)
        Me.SaveActionButton.Name = "SaveActionButton"
        Me.SaveActionButton.Size = New System.Drawing.Size(60, 23)
        Me.SaveActionButton.TabIndex = 6
        Me.SaveActionButton.Text = "Save"
        Me.SaveActionButton.UseVisualStyleBackColor = True
        '
        'CancelActionButton
        '
        Me.CancelActionButton.CausesValidation = False
        Me.CancelActionButton.Location = New System.Drawing.Point(194, 158)
        Me.CancelActionButton.Name = "CancelActionButton"
        Me.CancelActionButton.Size = New System.Drawing.Size(60, 23)
        Me.CancelActionButton.TabIndex = 5
        Me.CancelActionButton.Text = "Cancel"
        Me.CancelActionButton.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.ChecklistDataGrid)
        Me.GroupBox2.Location = New System.Drawing.Point(3, 187)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(532, 171)
        Me.GroupBox2.TabIndex = 121
        Me.GroupBox2.TabStop = False
        '
        'ChecklistDataGrid
        '
        Me.ChecklistDataGrid.ADGroupsThatCanCopy = ""
        Me.ChecklistDataGrid.ADGroupsThatCanCustomize = ""
        Me.ChecklistDataGrid.ADGroupsThatCanExport = ""
        Me.ChecklistDataGrid.ADGroupsThatCanFilter = ""
        Me.ChecklistDataGrid.ADGroupsThatCanFind = ""
        Me.ChecklistDataGrid.ADGroupsThatCanMultiSort = ""
        Me.ChecklistDataGrid.ADGroupsThatCanPrint = ""
        Me.ChecklistDataGrid.AllowAutoSize = True
        Me.ChecklistDataGrid.AllowColumnReorder = True
        Me.ChecklistDataGrid.AllowCopy = True
        Me.ChecklistDataGrid.AllowCustomize = True
        Me.ChecklistDataGrid.AllowDelete = False
        Me.ChecklistDataGrid.AllowDragDrop = False
        Me.ChecklistDataGrid.AllowEdit = False
        Me.ChecklistDataGrid.AllowExport = False
        Me.ChecklistDataGrid.AllowFilter = True
        Me.ChecklistDataGrid.AllowFind = True
        Me.ChecklistDataGrid.AllowGoTo = True
        Me.ChecklistDataGrid.AllowMultiSelect = False
        Me.ChecklistDataGrid.AllowMultiSort = False
        Me.ChecklistDataGrid.AllowNew = False
        Me.ChecklistDataGrid.AllowPrint = True
        Me.ChecklistDataGrid.AllowRefresh = True
        Me.ChecklistDataGrid.AppKey = "UFCW\RegMaster\"
        Me.ChecklistDataGrid.AutoSaveCols = True
        Me.ChecklistDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.ChecklistDataGrid.ColumnHeaderLabel = Nothing
        Me.ChecklistDataGrid.ColumnRePositioning = False
        Me.ChecklistDataGrid.ColumnResizing = False
        Me.ChecklistDataGrid.ConfirmDelete = True
        Me.ChecklistDataGrid.CopySelectedOnly = True
        Me.ChecklistDataGrid.CurrentBSPosition = -1
        Me.ChecklistDataGrid.DataMember = ""
        Me.ChecklistDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ChecklistDataGrid.DragColumn = 0
        Me.ChecklistDataGrid.ExportSelectedOnly = True
        Me.ChecklistDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ChecklistDataGrid.HighlightedRow = Nothing
        Me.ChecklistDataGrid.HighLightModifiedRows = False
        Me.ChecklistDataGrid.IsMouseDown = False
        Me.ChecklistDataGrid.LastGoToLine = ""
        Me.ChecklistDataGrid.Location = New System.Drawing.Point(3, 16)
        Me.ChecklistDataGrid.MultiSort = False
        Me.ChecklistDataGrid.Name = "ChecklistDataGrid"
        Me.ChecklistDataGrid.OldSelectedRow = 0
        Me.ChecklistDataGrid.PreviousBSPosition = -1
        Me.ChecklistDataGrid.ReadOnly = True
        Me.ChecklistDataGrid.RetainRowSelectionAfterSort = True
        Me.ChecklistDataGrid.SetRowOnRightClick = True
        Me.ChecklistDataGrid.ShiftPressed = False
        Me.ChecklistDataGrid.SingleClickBooleanColumns = True
        Me.ChecklistDataGrid.Size = New System.Drawing.Size(526, 152)
        Me.ChecklistDataGrid.Sort = Nothing
        Me.ChecklistDataGrid.StyleName = ""
        Me.ChecklistDataGrid.SubKey = ""
        Me.ChecklistDataGrid.SuppressMouseDown = False
        Me.ChecklistDataGrid.SuppressTriangle = False
        Me.ChecklistDataGrid.TabIndex = 7
        '
        'grpEditPanel
        '
        Me.grpEditPanel.Controls.Add(Me.HistoryButton)
        Me.grpEditPanel.Controls.Add(Me.TransparentContainer2)
        Me.grpEditPanel.Controls.Add(Me.TransparentContainer1)
        Me.grpEditPanel.Controls.Add(Me.txtTermDate)
        Me.grpEditPanel.Controls.Add(Me.Label5)
        Me.grpEditPanel.Controls.Add(Me.Label4)
        Me.grpEditPanel.Controls.Add(Me.Label3)
        Me.grpEditPanel.Controls.Add(Me.Label1)
        Me.grpEditPanel.Controls.Add(Me.Label2)
        Me.grpEditPanel.Controls.Add(Me.txtfamilyid)
        Me.grpEditPanel.Controls.Add(Me.txtrelationid)
        Me.grpEditPanel.Controls.Add(Me.txtEntryDate)
        Me.grpEditPanel.Controls.Add(Me.FromLabel)
        Me.grpEditPanel.Location = New System.Drawing.Point(3, 0)
        Me.grpEditPanel.Name = "grpEditPanel"
        Me.grpEditPanel.Size = New System.Drawing.Size(532, 153)
        Me.grpEditPanel.TabIndex = 120
        Me.grpEditPanel.TabStop = False
        '
        'HistoryButton
        '
        Me.HistoryButton.CausesValidation = False
        Me.HistoryButton.Location = New System.Drawing.Point(466, 19)
        Me.HistoryButton.Name = "HistoryButton"
        Me.HistoryButton.Size = New System.Drawing.Size(60, 23)
        Me.HistoryButton.TabIndex = 107
        Me.HistoryButton.Text = "History"
        Me.HistoryButton.UseVisualStyleBackColor = True
        '
        'TransparentContainer2
        '
        Me.TransparentContainer2.Controls.Add(Me.cmbLocal)
        Me.TransparentContainer2.Location = New System.Drawing.Point(105, 91)
        Me.TransparentContainer2.Name = "TransparentContainer2"
        Me.TransparentContainer2.Size = New System.Drawing.Size(130, 22)
        Me.TransparentContainer2.TabIndex = 106
        Me.TransparentContainer2.Text = "TransparentContainer2"
        '
        'cmbLocal
        '
        Me.cmbLocal.DropDownHeight = 100
        Me.cmbLocal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbLocal.DropDownWidth = 250
        Me.cmbLocal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.cmbLocal.FormattingEnabled = True
        Me.cmbLocal.IntegralHeight = False
        Me.cmbLocal.ItemHeight = 13
        Me.cmbLocal.Location = New System.Drawing.Point(0, 0)
        Me.cmbLocal.Name = "cmbLocal"
        Me.cmbLocal.ReadOnly = False
        Me.cmbLocal.Size = New System.Drawing.Size(102, 21)
        Me.cmbLocal.TabIndex = 3
        '
        'TransparentContainer1
        '
        Me.TransparentContainer1.Controls.Add(Me.cmbMemType)
        Me.TransparentContainer1.Location = New System.Drawing.Point(105, 64)
        Me.TransparentContainer1.Name = "TransparentContainer1"
        Me.TransparentContainer1.Size = New System.Drawing.Size(207, 22)
        Me.TransparentContainer1.TabIndex = 105
        Me.TransparentContainer1.Text = "TransparentContainer1"
        '
        'cmbMemType
        '
        Me.cmbMemType.DropDownHeight = 100
        Me.cmbMemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMemType.DropDownWidth = 250
        Me.cmbMemType.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbMemType.FormattingEnabled = True
        Me.cmbMemType.IntegralHeight = False
        Me.cmbMemType.ItemHeight = 14
        Me.cmbMemType.Location = New System.Drawing.Point(0, 0)
        Me.cmbMemType.Name = "cmbMemType"
        Me.cmbMemType.ReadOnly = False
        Me.cmbMemType.Size = New System.Drawing.Size(145, 22)
        Me.cmbMemType.TabIndex = 2
        '
        'txtTermDate
        '
        Me.txtTermDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTermDate.Location = New System.Drawing.Point(106, 117)
        Me.txtTermDate.MaxLength = 10
        Me.txtTermDate.Name = "txtTermDate"
        Me.txtTermDate.Size = New System.Drawing.Size(91, 20)
        Me.txtTermDate.TabIndex = 3
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(41, 121)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(57, 13)
        Me.Label5.TabIndex = 104
        Me.Label5.Text = "Term Date"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(24, 65)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(73, 13)
        Me.Label4.TabIndex = 102
        Me.Label4.Text = "Last Memtype"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(40, 91)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 13)
        Me.Label3.TabIndex = 101
        Me.Label3.Text = "Last Local"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(203, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(57, 13)
        Me.Label1.TabIndex = 100
        Me.Label1.Text = "RelationID"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(54, 17)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 13)
        Me.Label2.TabIndex = 99
        Me.Label2.Text = "FamilyID"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtfamilyid
        '
        Me.txtfamilyid.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtfamilyid.Location = New System.Drawing.Point(106, 14)
        Me.txtfamilyid.MaxLength = 15
        Me.txtfamilyid.Name = "txtfamilyid"
        Me.txtfamilyid.ReadOnly = True
        Me.txtfamilyid.Size = New System.Drawing.Size(91, 20)
        Me.txtfamilyid.TabIndex = 97
        '
        'txtrelationid
        '
        Me.txtrelationid.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtrelationid.Location = New System.Drawing.Point(266, 15)
        Me.txtrelationid.MaxLength = 10
        Me.txtrelationid.Name = "txtrelationid"
        Me.txtrelationid.ReadOnly = True
        Me.txtrelationid.Size = New System.Drawing.Size(31, 20)
        Me.txtrelationid.TabIndex = 98
        '
        'txtEntryDate
        '
        Me.txtEntryDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEntryDate.Location = New System.Drawing.Point(105, 39)
        Me.txtEntryDate.MaxLength = 10
        Me.txtEntryDate.Name = "txtEntryDate"
        Me.txtEntryDate.Size = New System.Drawing.Size(91, 20)
        Me.txtEntryDate.TabIndex = 0
        '
        'FromLabel
        '
        Me.FromLabel.AutoSize = True
        Me.FromLabel.Location = New System.Drawing.Point(2, 40)
        Me.FromLabel.Name = "FromLabel"
        Me.FromLabel.Size = New System.Drawing.Size(97, 13)
        Me.FromLabel.TabIndex = 93
        Me.FromLabel.Text = "Industry Entry Date"
        Me.FromLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'EligCalcElementsControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ModifyActionButton)
        Me.Controls.Add(Me.AddActionButton)
        Me.Controls.Add(Me.SaveActionButton)
        Me.Controls.Add(Me.CancelActionButton)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.grpEditPanel)
        Me.Name = "EligCalcElementsControl"
        Me.Size = New System.Drawing.Size(538, 361)
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.ChecklistDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpEditPanel.ResumeLayout(False)
        Me.grpEditPanel.PerformLayout()
        Me.TransparentContainer2.ResumeLayout(False)
        Me.TransparentContainer1.ResumeLayout(False)
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ModifyActionButton As System.Windows.Forms.Button
    Friend WithEvents AddActionButton As System.Windows.Forms.Button
    Friend WithEvents SaveActionButton As System.Windows.Forms.Button
    Friend WithEvents CancelActionButton As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents ChecklistDataGrid As DataGridCustom
    Friend WithEvents grpEditPanel As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtfamilyid As System.Windows.Forms.TextBox
    Friend WithEvents txtrelationid As System.Windows.Forms.TextBox
    Friend WithEvents txtEntryDate As System.Windows.Forms.TextBox
    Friend WithEvents FromLabel As System.Windows.Forms.Label
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtTermDate As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TransparentContainer2 As TransparentContainer
    Friend WithEvents cmbLocal As ExComboBox
    Friend WithEvents TransparentContainer1 As TransparentContainer
    Friend WithEvents cmbMemType As ExComboBox
    Friend WithEvents HistoryButton As Windows.Forms.Button
End Class
