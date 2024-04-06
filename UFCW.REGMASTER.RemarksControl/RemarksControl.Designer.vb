<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class REMARKSControl
    Inherits System.Windows.Forms.UserControl

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(REMARKSControl))
        Me.FlagImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.txtRemarks = New System.Windows.Forms.TextBox()
        Me.AddActionButton = New System.Windows.Forms.Button()
        Me.CancelActionButton = New System.Windows.Forms.Button()
        Me.SaveActionButton = New System.Windows.Forms.Button()
        Me.RemarksDataGrid = New DataGridCustom()
        Me.grpEditPanel = New System.Windows.Forms.GroupBox()
        Me.ToolTip2 = New System.Windows.Forms.ToolTip(Me.components)
        Me._PatientRemarksDS = New RemarksDS()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.RemarksDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpEditPanel.SuspendLayout()
        CType(Me._PatientRemarksDS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'FlagImageList
        '
        Me.FlagImageList.ImageStream = CType(resources.GetObject("FlagImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.FlagImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.FlagImageList.Images.SetKeyName(0, "")
        Me.FlagImageList.Images.SetKeyName(1, "")
        Me.FlagImageList.Images.SetKeyName(2, "")
        Me.FlagImageList.Images.SetKeyName(3, "")
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(3, 16)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtRemarks)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.AddActionButton)
        Me.SplitContainer1.Panel2.Controls.Add(Me.CancelActionButton)
        Me.SplitContainer1.Panel2.Controls.Add(Me.SaveActionButton)
        Me.SplitContainer1.Panel2.Controls.Add(Me.RemarksDataGrid)
        Me.SplitContainer1.Size = New System.Drawing.Size(350, 441)
        Me.SplitContainer1.SplitterDistance = 198
        Me.SplitContainer1.TabIndex = 2
        '
        'txtRemarks
        '
        Me.txtRemarks.AcceptsReturn = True
        Me.txtRemarks.AllowDrop = True
        Me.txtRemarks.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRemarks.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ErrorProvider1.SetIconPadding(Me.txtRemarks, 5)
        Me.txtRemarks.Location = New System.Drawing.Point(6, 15)
        Me.txtRemarks.MaxLength = 3700
        Me.txtRemarks.Multiline = True
        Me.txtRemarks.Name = "txtRemarks"
        Me.txtRemarks.ReadOnly = True
        Me.txtRemarks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtRemarks.Size = New System.Drawing.Size(305, 173)
        Me.txtRemarks.TabIndex = 3
        '
        'AddActionButton
        '
        Me.AddActionButton.BackColor = System.Drawing.SystemColors.ControlLight
        Me.AddActionButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AddActionButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.AddActionButton.Location = New System.Drawing.Point(3, 8)
        Me.AddActionButton.Name = "AddActionButton"
        Me.AddActionButton.Size = New System.Drawing.Size(76, 23)
        Me.AddActionButton.TabIndex = 4
        Me.AddActionButton.Text = "Add"
        Me.ToolTip2.SetToolTip(Me.AddActionButton, "Add Remark")
        Me.AddActionButton.UseVisualStyleBackColor = False
        Me.AddActionButton.Visible = False
        '
        'CancelActionButton
        '
        Me.CancelActionButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CancelActionButton.CausesValidation = False
        Me.CancelActionButton.Location = New System.Drawing.Point(189, 8)
        Me.CancelActionButton.Name = "CancelActionButton"
        Me.CancelActionButton.Size = New System.Drawing.Size(76, 23)
        Me.CancelActionButton.TabIndex = 5
        Me.CancelActionButton.Text = "&Cancel"
        Me.CancelActionButton.Visible = False
        '
        'SaveActionButton
        '
        Me.SaveActionButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SaveActionButton.CausesValidation = False
        Me.SaveActionButton.Location = New System.Drawing.Point(272, 8)
        Me.SaveActionButton.Name = "SaveActionButton"
        Me.SaveActionButton.Size = New System.Drawing.Size(76, 23)
        Me.SaveActionButton.TabIndex = 6
        Me.SaveActionButton.Text = "&Save"
        Me.SaveActionButton.Visible = False
        '
        'RemarksDataGrid
        '
        Me.RemarksDataGrid.ADGroupsThatCanCopy = ""
        Me.RemarksDataGrid.ADGroupsThatCanCustomize = ""
        Me.RemarksDataGrid.ADGroupsThatCanExport = ""
        Me.RemarksDataGrid.ADGroupsThatCanFilter = ""
        Me.RemarksDataGrid.ADGroupsThatCanFind = ""
        Me.RemarksDataGrid.ADGroupsThatCanMultiSort = ""
        Me.RemarksDataGrid.ADGroupsThatCanPrint = ""
        Me.RemarksDataGrid.AllowAutoSize = False
        Me.RemarksDataGrid.AllowColumnReorder = False
        Me.RemarksDataGrid.AllowCopy = True
        Me.RemarksDataGrid.AllowCustomize = True
        Me.RemarksDataGrid.AllowDelete = False
        Me.RemarksDataGrid.AllowDragDrop = False
        Me.RemarksDataGrid.AllowEdit = False
        Me.RemarksDataGrid.AllowExport = True
        Me.RemarksDataGrid.AllowFilter = True
        Me.RemarksDataGrid.AllowFind = True
        Me.RemarksDataGrid.AllowGoTo = True
        Me.RemarksDataGrid.AllowMultiSelect = False
        Me.RemarksDataGrid.AllowMultiSort = False
        Me.RemarksDataGrid.AllowNew = False
        Me.RemarksDataGrid.AllowPrint = True
        Me.RemarksDataGrid.AllowRefresh = True
        Me.RemarksDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RemarksDataGrid.AppKey = "UFCW\RegMaster\"
        Me.RemarksDataGrid.AutoSaveCols = True
        Me.RemarksDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.RemarksDataGrid.CaptionVisible = False
        Me.RemarksDataGrid.ColumnHeaderLabel = Nothing
        Me.RemarksDataGrid.ColumnRePositioning = False
        Me.RemarksDataGrid.ColumnResizing = False
        Me.RemarksDataGrid.ConfirmDelete = False
        Me.RemarksDataGrid.CopySelectedOnly = True
        Me.RemarksDataGrid.CurrentBSPosition = -1
        Me.RemarksDataGrid.DataMember = ""
        Me.RemarksDataGrid.DragColumn = 0
        Me.RemarksDataGrid.ExportSelectedOnly = True
        Me.RemarksDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.RemarksDataGrid.HighlightedRow = Nothing
        Me.RemarksDataGrid.HighLightModifiedRows = False
        Me.RemarksDataGrid.IsMouseDown = False
        Me.RemarksDataGrid.LastGoToLine = ""
        Me.RemarksDataGrid.Location = New System.Drawing.Point(0, 37)
        Me.RemarksDataGrid.MultiSort = False
        Me.RemarksDataGrid.Name = "RemarksDataGrid"
        Me.RemarksDataGrid.OldSelectedRow = Nothing
        Me.RemarksDataGrid.PreferredRowHeight = 48
        Me.RemarksDataGrid.PreviousBSPosition = -1
        Me.RemarksDataGrid.ReadOnly = True
        Me.RemarksDataGrid.RetainRowSelectionAfterSort = True
        Me.RemarksDataGrid.SetRowOnRightClick = True
        Me.RemarksDataGrid.ShiftPressed = False
        Me.RemarksDataGrid.SingleClickBooleanColumns = True
        Me.RemarksDataGrid.Size = New System.Drawing.Size(350, 193)
        Me.RemarksDataGrid.Sort = Nothing
        Me.RemarksDataGrid.StyleName = ""
        Me.RemarksDataGrid.SubKey = ""
        Me.RemarksDataGrid.SuppressMouseDown = False
        Me.RemarksDataGrid.SuppressTriangle = False
        Me.RemarksDataGrid.TabIndex = 7
        Me.RemarksDataGrid.TabStop = False
        '
        'grpEditPanel
        '
        Me.grpEditPanel.Controls.Add(Me.SplitContainer1)
        Me.grpEditPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpEditPanel.Location = New System.Drawing.Point(0, 0)
        Me.grpEditPanel.Name = "grpEditPanel"
        Me.grpEditPanel.Size = New System.Drawing.Size(356, 460)
        Me.grpEditPanel.TabIndex = 1
        Me.grpEditPanel.TabStop = False
        Me.grpEditPanel.Text = "Remarks"
        '
        '_PatientRemarksDS
        '
        Me._PatientRemarksDS.DataSetName = "AnnotationDataSet1"
        Me._PatientRemarksDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'REMARKSControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.Controls.Add(Me.grpEditPanel)
        Me.Name = "REMARKSControl"
        Me.Size = New System.Drawing.Size(356, 460)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.RemarksDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpEditPanel.ResumeLayout(False)
        CType(Me._PatientRemarksDS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents FlagImageList As System.Windows.Forms.ImageList
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents RemarksDataGrid As DataGridCustom
    Friend WithEvents grpEditPanel As System.Windows.Forms.GroupBox
    Friend WithEvents ToolTip2 As System.Windows.Forms.ToolTip
    Friend WithEvents _PatientRemarksDS As RemarksDS
    Friend WithEvents ErrorProvider1 As ErrorProvider
    Friend WithEvents txtRemarks As TextBox
    Friend WithEvents AddActionButton As Button
    Public WithEvents CancelActionButton As Button
    Friend WithEvents SaveActionButton As Button
End Class
