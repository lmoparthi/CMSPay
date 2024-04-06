<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WaitingPeriodControl
    Inherits System.Windows.Forms.UserControl

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.btnModify = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.CancelButton = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.WaitPeriodDataGrid = New DataGridCustom()
        Me.grpEditPanel = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtfamilyid = New System.Windows.Forms.TextBox()
        Me.txtrelationid = New System.Windows.Forms.TextBox()
        Me.txtEligDt = New System.Windows.Forms.TextBox()
        Me.FromLabel = New System.Windows.Forms.Label()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.WPDS = New WaitPeriodDS()
        Me.GroupBox2.SuspendLayout()
        CType(Me.WaitPeriodDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpEditPanel.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.WPDS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnModify
        '
        Me.btnModify.Location = New System.Drawing.Point(74, 101)
        Me.btnModify.Name = "btnModify"
        Me.btnModify.Size = New System.Drawing.Size(60, 23)
        Me.btnModify.TabIndex = 111
        Me.btnModify.Text = "Modify"
        Me.btnModify.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(8, 101)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(60, 23)
        Me.btnAdd.TabIndex = 110
        Me.btnAdd.Text = "Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'SaveButton
        '
        Me.SaveButton.Enabled = False
        Me.SaveButton.Location = New System.Drawing.Point(261, 101)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(60, 23)
        Me.SaveButton.TabIndex = 113
        Me.SaveButton.Text = "Save"
        Me.SaveButton.UseVisualStyleBackColor = True
        '
        'CancelButton
        '
        Me.CancelButton.Location = New System.Drawing.Point(195, 101)
        Me.CancelButton.Name = "CancelButton"
        Me.CancelButton.Size = New System.Drawing.Size(60, 23)
        Me.CancelButton.TabIndex = 112
        Me.CancelButton.Text = "Cancel"
        Me.CancelButton.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.WaitPeriodDataGrid)
        Me.GroupBox2.Location = New System.Drawing.Point(0, 125)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(461, 156)
        Me.GroupBox2.TabIndex = 115
        Me.GroupBox2.TabStop = False
        '
        'WaitPeriodDataGrid
        '
        Me.WaitPeriodDataGrid.ADGroupsThatCanCopy = ""
        Me.WaitPeriodDataGrid.ADGroupsThatCanCustomize = ""
        Me.WaitPeriodDataGrid.ADGroupsThatCanExport = ""
        Me.WaitPeriodDataGrid.ADGroupsThatCanFilter = ""
        Me.WaitPeriodDataGrid.ADGroupsThatCanFind = ""
        Me.WaitPeriodDataGrid.ADGroupsThatCanMultiSort = ""
        Me.WaitPeriodDataGrid.ADGroupsThatCanPrint = ""
        Me.WaitPeriodDataGrid.AllowAutoSize = True
        Me.WaitPeriodDataGrid.AllowColumnReorder = True
        Me.WaitPeriodDataGrid.AllowCopy = True
        Me.WaitPeriodDataGrid.AllowCustomize = True
        Me.WaitPeriodDataGrid.AllowDelete = False
        Me.WaitPeriodDataGrid.AllowDragDrop = False
        Me.WaitPeriodDataGrid.AllowEdit = False
        Me.WaitPeriodDataGrid.AllowExport = False
        Me.WaitPeriodDataGrid.AllowFilter = True
        Me.WaitPeriodDataGrid.AllowFind = True
        Me.WaitPeriodDataGrid.AllowGoTo = True
        Me.WaitPeriodDataGrid.AllowMultiSelect = False
        Me.WaitPeriodDataGrid.AllowMultiSort = False
        Me.WaitPeriodDataGrid.AllowNew = False
        Me.WaitPeriodDataGrid.AllowPrint = True
        Me.WaitPeriodDataGrid.AllowRefresh = True
        Me.WaitPeriodDataGrid.AppKey = "UFCW\RegMaster\"
        Me.WaitPeriodDataGrid.AutoSaveCols = True
        Me.WaitPeriodDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.WaitPeriodDataGrid.ColumnHeaderLabel = Nothing
        Me.WaitPeriodDataGrid.ColumnRePositioning = False
        Me.WaitPeriodDataGrid.ColumnResizing = False
        Me.WaitPeriodDataGrid.ConfirmDelete = True
        Me.WaitPeriodDataGrid.CopySelectedOnly = True
        Me.WaitPeriodDataGrid.DataMember = ""
        Me.WaitPeriodDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WaitPeriodDataGrid.DragColumn = 0
        Me.WaitPeriodDataGrid.ExportSelectedOnly = True
        Me.WaitPeriodDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.WaitPeriodDataGrid.HighlightedRow = Nothing
        Me.WaitPeriodDataGrid.HighLightModifiedRows = False
        Me.WaitPeriodDataGrid.IsMouseDown = False
        Me.WaitPeriodDataGrid.LastGoToLine = ""
        Me.WaitPeriodDataGrid.Location = New System.Drawing.Point(3, 16)
        Me.WaitPeriodDataGrid.MultiSort = False
        Me.WaitPeriodDataGrid.Name = "WaitPeriodDataGrid"
        Me.WaitPeriodDataGrid.OldSelectedRow = Nothing
        Me.WaitPeriodDataGrid.ReadOnly = True
        Me.WaitPeriodDataGrid.RetainRowSelectionAfterSort = True
        Me.WaitPeriodDataGrid.SetRowOnRightClick = True
        Me.WaitPeriodDataGrid.ShiftPressed = False
        Me.WaitPeriodDataGrid.SingleClickBooleanColumns = True
        Me.WaitPeriodDataGrid.Size = New System.Drawing.Size(455, 137)
        Me.WaitPeriodDataGrid.Sort = Nothing
        Me.WaitPeriodDataGrid.StyleName = ""
        Me.WaitPeriodDataGrid.SubKey = ""
        Me.WaitPeriodDataGrid.SuppressTriangle = False
        Me.WaitPeriodDataGrid.TabIndex = 103
        '
        'grpEditPanel
        '
        Me.grpEditPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpEditPanel.Controls.Add(Me.Label1)
        Me.grpEditPanel.Controls.Add(Me.Label2)
        Me.grpEditPanel.Controls.Add(Me.txtfamilyid)
        Me.grpEditPanel.Controls.Add(Me.txtrelationid)
        Me.grpEditPanel.Controls.Add(Me.txtEligDt)
        Me.grpEditPanel.Controls.Add(Me.FromLabel)
        Me.grpEditPanel.Location = New System.Drawing.Point(0, 3)
        Me.grpEditPanel.Name = "grpEditPanel"
        Me.grpEditPanel.Size = New System.Drawing.Size(461, 93)
        Me.grpEditPanel.TabIndex = 114
        Me.grpEditPanel.TabStop = False
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
        'txtfamilyid
        '
        Me.txtfamilyid.Enabled = False
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
        Me.txtrelationid.Enabled = False
        Me.txtrelationid.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtrelationid.Location = New System.Drawing.Point(107, 40)
        Me.txtrelationid.MaxLength = 10
        Me.txtrelationid.Name = "txtrelationid"
        Me.txtrelationid.ReadOnly = True
        Me.txtrelationid.Size = New System.Drawing.Size(31, 20)
        Me.txtrelationid.TabIndex = 98
        '
        'txtEligDt
        '
        Me.txtEligDt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEligDt.Location = New System.Drawing.Point(106, 65)
        Me.txtEligDt.MaxLength = 10
        Me.txtEligDt.Name = "txtEligDt"
        Me.txtEligDt.Size = New System.Drawing.Size(91, 20)
        Me.txtEligDt.TabIndex = 0
        '
        'FromLabel
        '
        Me.FromLabel.AutoSize = True
        Me.FromLabel.Location = New System.Drawing.Point(4, 68)
        Me.FromLabel.Name = "FromLabel"
        Me.FromLabel.Size = New System.Drawing.Size(94, 13)
        Me.FromLabel.TabIndex = 93
        Me.FromLabel.Text = "First Eligibility Date"
        Me.FromLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'WPDS
        '
        Me.WPDS.DataSetName = "WaitPeriodDS"
        Me.WPDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'WaitingPeriodControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnModify)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.SaveButton)
        Me.Controls.Add(Me.CancelButton)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.grpEditPanel)
        Me.Name = "WaitingPeriodControl"
        Me.Size = New System.Drawing.Size(464, 284)
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.WaitPeriodDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpEditPanel.ResumeLayout(False)
        Me.grpEditPanel.PerformLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.WPDS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnModify As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents SaveButton As System.Windows.Forms.Button
    Friend WithEvents CancelButton As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents WaitPeriodDataGrid As DataGridCustom
    Friend WithEvents grpEditPanel As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtfamilyid As System.Windows.Forms.TextBox
    Friend WithEvents txtrelationid As System.Windows.Forms.TextBox
    Friend WithEvents txtEligDt As System.Windows.Forms.TextBox
    Friend WithEvents FromLabel As System.Windows.Forms.Label
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents WPDS As WaitPeriodDS

End Class
