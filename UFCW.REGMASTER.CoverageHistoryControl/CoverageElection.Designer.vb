<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CoverageElectionForm
    Inherits System.Windows.Forms.Form

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblUIMessaging = New System.Windows.Forms.Label()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.grpmed = New System.Windows.Forms.GroupBox()
        Me.MedUIContainer = New System.Windows.Forms.SplitContainer()
        Me.grpMedEditPanel = New System.Windows.Forms.GroupBox()
        Me.TransparentContainer1 = New TransparentContainer()
        Me.cmbMedLetter = New ExComboBox()
        Me.txtMedFromDate = New System.Windows.Forms.TextBox()
        Me.MedHMONetworkValidationPanel = New TransparentContainer()
        Me.cmbHMONetwork = New ExComboBox()
        Me.txtMedThruDate = New System.Windows.Forms.TextBox()
        Me.MedCoverageValidationPanel = New TransparentContainer()
        Me.cmbMedCoverage = New ExComboBox()
        Me.MedHistoryButton = New System.Windows.Forms.Button()
        Me.MedHMONetworkButton = New System.Windows.Forms.Button()
        Me.LabelNetwork = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.FromLabel = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.MedDeleteButton = New System.Windows.Forms.Button()
        Me.MedAddButton = New System.Windows.Forms.Button()
        Me.MedModifyButton = New System.Windows.Forms.Button()
        Me.MedSaveButton = New System.Windows.Forms.Button()
        Me.MedCancelButton = New System.Windows.Forms.Button()
        Me.MedCoverageDataGrid = New DataGridCustom()
        Me.grpDen = New System.Windows.Forms.GroupBox()
        Me.DenUIContainer = New System.Windows.Forms.SplitContainer()
        Me.grpDenEditPanel = New System.Windows.Forms.GroupBox()
        Me.TransparentContainer2 = New TransparentContainer()
        Me.cmbDenLetter = New ExComboBox()
        Me.txtDenThruDate = New System.Windows.Forms.TextBox()
        Me.txtDenFromDate = New System.Windows.Forms.TextBox()
        Me.DenHistoryButton = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.DenCoverageValidationPanel = New TransparentContainer()
        Me.cmbDenCoverageValues = New ExComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.DenDeleteButton = New System.Windows.Forms.Button()
        Me.DenAddButton = New System.Windows.Forms.Button()
        Me.DenModifyButton = New System.Windows.Forms.Button()
        Me.DenSaveButton = New System.Windows.Forms.Button()
        Me.DenCancelButton = New System.Windows.Forms.Button()
        Me.DenCoverageDataGrid = New DataGridCustom()
        Me.ExitButton = New System.Windows.Forms.Button()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Panel1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.grpmed.SuspendLayout()
        CType(Me.MedUIContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MedUIContainer.Panel1.SuspendLayout()
        Me.MedUIContainer.Panel2.SuspendLayout()
        Me.MedUIContainer.SuspendLayout()
        Me.grpMedEditPanel.SuspendLayout()
        Me.TransparentContainer1.SuspendLayout()
        Me.MedHMONetworkValidationPanel.SuspendLayout()
        Me.MedCoverageValidationPanel.SuspendLayout()
        CType(Me.MedCoverageDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpDen.SuspendLayout()
        CType(Me.DenUIContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DenUIContainer.Panel1.SuspendLayout()
        Me.DenUIContainer.Panel2.SuspendLayout()
        Me.DenUIContainer.SuspendLayout()
        Me.grpDenEditPanel.SuspendLayout()
        Me.TransparentContainer2.SuspendLayout()
        Me.DenCoverageValidationPanel.SuspendLayout()
        CType(Me.DenCoverageDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.lblUIMessaging)
        Me.Panel1.Controls.Add(Me.SplitContainer1)
        Me.Panel1.Controls.Add(Me.ExitButton)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(941, 368)
        Me.Panel1.TabIndex = 0
        '
        'lblUIMessaging
        '
        Me.lblUIMessaging.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblUIMessaging.AutoSize = True
        Me.lblUIMessaging.ForeColor = System.Drawing.Color.Red
        Me.lblUIMessaging.Location = New System.Drawing.Point(18, 341)
        Me.lblUIMessaging.Name = "lblUIMessaging"
        Me.lblUIMessaging.Size = New System.Drawing.Size(0, 13)
        Me.lblUIMessaging.TabIndex = 309
        Me.lblUIMessaging.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblUIMessaging.Visible = False
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.IsSplitterFixed = True
        Me.SplitContainer1.Location = New System.Drawing.Point(3, 3)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.grpmed)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.grpDen)
        Me.SplitContainer1.Size = New System.Drawing.Size(935, 333)
        Me.SplitContainer1.SplitterDistance = 467
        Me.SplitContainer1.TabIndex = 0
        Me.SplitContainer1.TabStop = False
        '
        'grpmed
        '
        Me.grpmed.Controls.Add(Me.MedUIContainer)
        Me.grpmed.Controls.Add(Me.MedCoverageDataGrid)
        Me.grpmed.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpmed.Location = New System.Drawing.Point(0, 0)
        Me.grpmed.Name = "grpmed"
        Me.grpmed.Size = New System.Drawing.Size(467, 333)
        Me.grpmed.TabIndex = 0
        Me.grpmed.TabStop = False
        Me.grpmed.Text = "Medical Coverage"
        '
        'MedUIContainer
        '
        Me.MedUIContainer.IsSplitterFixed = True
        Me.MedUIContainer.Location = New System.Drawing.Point(1, 16)
        Me.MedUIContainer.Name = "MedUIContainer"
        Me.MedUIContainer.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'MedUIContainer.Panel1
        '
        Me.MedUIContainer.Panel1.Controls.Add(Me.grpMedEditPanel)
        '
        'MedUIContainer.Panel2
        '
        Me.MedUIContainer.Panel2.CausesValidation = False
        Me.MedUIContainer.Panel2.Controls.Add(Me.MedDeleteButton)
        Me.MedUIContainer.Panel2.Controls.Add(Me.MedAddButton)
        Me.MedUIContainer.Panel2.Controls.Add(Me.MedModifyButton)
        Me.MedUIContainer.Panel2.Controls.Add(Me.MedSaveButton)
        Me.MedUIContainer.Panel2.Controls.Add(Me.MedCancelButton)
        Me.MedUIContainer.Size = New System.Drawing.Size(458, 163)
        Me.MedUIContainer.SplitterDistance = 128
        Me.MedUIContainer.SplitterWidth = 1
        Me.MedUIContainer.TabIndex = 0
        Me.MedUIContainer.TabStop = False
        '
        'grpMedEditPanel
        '
        Me.grpMedEditPanel.Controls.Add(Me.TransparentContainer1)
        Me.grpMedEditPanel.Controls.Add(Me.txtMedFromDate)
        Me.grpMedEditPanel.Controls.Add(Me.MedHMONetworkValidationPanel)
        Me.grpMedEditPanel.Controls.Add(Me.txtMedThruDate)
        Me.grpMedEditPanel.Controls.Add(Me.MedCoverageValidationPanel)
        Me.grpMedEditPanel.Controls.Add(Me.MedHistoryButton)
        Me.grpMedEditPanel.Controls.Add(Me.MedHMONetworkButton)
        Me.grpMedEditPanel.Controls.Add(Me.LabelNetwork)
        Me.grpMedEditPanel.Controls.Add(Me.Label6)
        Me.grpMedEditPanel.Controls.Add(Me.Label4)
        Me.grpMedEditPanel.Controls.Add(Me.FromLabel)
        Me.grpMedEditPanel.Controls.Add(Me.Label3)
        Me.grpMedEditPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpMedEditPanel.Location = New System.Drawing.Point(0, 0)
        Me.grpMedEditPanel.Name = "grpMedEditPanel"
        Me.grpMedEditPanel.Size = New System.Drawing.Size(458, 128)
        Me.grpMedEditPanel.TabIndex = 0
        Me.grpMedEditPanel.TabStop = False
        '
        'TransparentContainer1
        '
        Me.TransparentContainer1.Controls.Add(Me.cmbMedLetter)
        Me.TransparentContainer1.Location = New System.Drawing.Point(321, 22)
        Me.TransparentContainer1.Name = "TransparentContainer1"
        Me.TransparentContainer1.Size = New System.Drawing.Size(59, 23)
        Me.TransparentContainer1.TabIndex = 5
        Me.TransparentContainer1.Text = "TransparentContainer1"
        '
        'cmbMedLetter
        '
        Me.cmbMedLetter.BackColor = System.Drawing.SystemColors.Control
        Me.cmbMedLetter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMedLetter.DropDownWidth = 50
        Me.cmbMedLetter.FormattingEnabled = True
        Me.cmbMedLetter.Items.AddRange(New Object() {"N", "Y"})
        Me.cmbMedLetter.Location = New System.Drawing.Point(0, 0)
        Me.cmbMedLetter.Name = "cmbMedLetter"
        Me.cmbMedLetter.ReadOnly = False
        Me.cmbMedLetter.Size = New System.Drawing.Size(42, 21)
        Me.cmbMedLetter.TabIndex = 6
        '
        'txtMedFromDate
        '
        Me.txtMedFromDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMedFromDate.Location = New System.Drawing.Point(103, 22)
        Me.txtMedFromDate.MaxLength = 10
        Me.txtMedFromDate.Name = "txtMedFromDate"
        Me.txtMedFromDate.ReadOnly = True
        Me.txtMedFromDate.Size = New System.Drawing.Size(91, 20)
        Me.txtMedFromDate.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.txtMedFromDate, "start date of Medical coverage")
        Me.txtMedFromDate.WordWrap = False
        '
        'MedHMONetworkValidationPanel
        '
        Me.MedHMONetworkValidationPanel.Controls.Add(Me.cmbHMONetwork)
        Me.MedHMONetworkValidationPanel.Location = New System.Drawing.Point(103, 99)
        Me.MedHMONetworkValidationPanel.Name = "MedHMONetworkValidationPanel"
        Me.MedHMONetworkValidationPanel.Size = New System.Drawing.Size(218, 23)
        Me.MedHMONetworkValidationPanel.TabIndex = 3
        '
        'cmbHMONetwork
        '
        Me.cmbHMONetwork.BackColor = System.Drawing.SystemColors.Control
        Me.cmbHMONetwork.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbHMONetwork.DropDownWidth = 200
        Me.cmbHMONetwork.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbHMONetwork.FormattingEnabled = True
        Me.cmbHMONetwork.Location = New System.Drawing.Point(0, 0)
        Me.cmbHMONetwork.MaxLength = 2
        Me.cmbHMONetwork.Name = "cmbHMONetwork"
        Me.cmbHMONetwork.ReadOnly = False
        Me.cmbHMONetwork.Size = New System.Drawing.Size(195, 21)
        Me.cmbHMONetwork.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.cmbHMONetwork, "Select the Plan Network")
        '
        'txtMedThruDate
        '
        Me.txtMedThruDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMedThruDate.Location = New System.Drawing.Point(103, 46)
        Me.txtMedThruDate.MaxLength = 10
        Me.txtMedThruDate.Name = "txtMedThruDate"
        Me.txtMedThruDate.ReadOnly = True
        Me.txtMedThruDate.Size = New System.Drawing.Size(91, 20)
        Me.txtMedThruDate.TabIndex = 0
        Me.txtMedThruDate.TabStop = False
        Me.ToolTip1.SetToolTip(Me.txtMedThruDate, "end date of Medical coverage")
        Me.txtMedThruDate.WordWrap = False
        '
        'MedCoverageValidationPanel
        '
        Me.MedCoverageValidationPanel.Controls.Add(Me.cmbMedCoverage)
        Me.MedCoverageValidationPanel.Location = New System.Drawing.Point(103, 73)
        Me.MedCoverageValidationPanel.Name = "MedCoverageValidationPanel"
        Me.MedCoverageValidationPanel.Size = New System.Drawing.Size(280, 24)
        Me.MedCoverageValidationPanel.TabIndex = 1
        '
        'cmbMedCoverage
        '
        Me.cmbMedCoverage.BackColor = System.Drawing.SystemColors.Control
        Me.cmbMedCoverage.DropDownHeight = 100
        Me.cmbMedCoverage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMedCoverage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbMedCoverage.FormattingEnabled = True
        Me.cmbMedCoverage.IntegralHeight = False
        Me.cmbMedCoverage.ItemHeight = 13
        Me.cmbMedCoverage.Location = New System.Drawing.Point(0, 0)
        Me.cmbMedCoverage.MaxLength = 2
        Me.cmbMedCoverage.Name = "cmbMedCoverage"
        Me.cmbMedCoverage.ReadOnly = False
        Me.cmbMedCoverage.Size = New System.Drawing.Size(259, 21)
        Me.cmbMedCoverage.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.cmbMedCoverage, "Choose the Medical Coverage")
        '
        'MedHistoryButton
        '
        Me.MedHistoryButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MedHistoryButton.CausesValidation = False
        Me.MedHistoryButton.Location = New System.Drawing.Point(390, 22)
        Me.MedHistoryButton.Name = "MedHistoryButton"
        Me.MedHistoryButton.Size = New System.Drawing.Size(60, 23)
        Me.MedHistoryButton.TabIndex = 0
        Me.MedHistoryButton.TabStop = False
        Me.MedHistoryButton.Text = "History"
        Me.MedHistoryButton.UseVisualStyleBackColor = True
        '
        'MedHMONetworkButton
        '
        Me.MedHMONetworkButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MedHMONetworkButton.CausesValidation = False
        Me.MedHMONetworkButton.Location = New System.Drawing.Point(390, 51)
        Me.MedHMONetworkButton.Name = "MedHMONetworkButton"
        Me.MedHMONetworkButton.Size = New System.Drawing.Size(60, 23)
        Me.MedHMONetworkButton.TabIndex = 0
        Me.MedHMONetworkButton.TabStop = False
        Me.MedHMONetworkButton.Text = "Network"
        Me.MedHMONetworkButton.UseVisualStyleBackColor = True
        '
        'LabelNetwork
        '
        Me.LabelNetwork.AutoSize = True
        Me.LabelNetwork.Location = New System.Drawing.Point(42, 101)
        Me.LabelNetwork.Name = "LabelNetwork"
        Me.LabelNetwork.Size = New System.Drawing.Size(47, 13)
        Me.LabelNetwork.TabIndex = 0
        Me.LabelNetwork.Text = "Network"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(253, 25)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(62, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Send Letter"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(7, 73)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(93, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Medical Coverage"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FromLabel
        '
        Me.FromLabel.AutoSize = True
        Me.FromLabel.Location = New System.Drawing.Point(42, 22)
        Me.FromLabel.Name = "FromLabel"
        Me.FromLabel.Size = New System.Drawing.Size(56, 13)
        Me.FromLabel.TabIndex = 0
        Me.FromLabel.Text = "From Date"
        Me.FromLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(42, 46)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(55, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Thru Date"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'MedDeleteButton
        '
        Me.MedDeleteButton.Location = New System.Drawing.Point(142, 7)
        Me.MedDeleteButton.Name = "MedDeleteButton"
        Me.MedDeleteButton.Size = New System.Drawing.Size(60, 23)
        Me.MedDeleteButton.TabIndex = 9
        Me.MedDeleteButton.Text = "Delete"
        Me.MedDeleteButton.UseVisualStyleBackColor = True
        '
        'MedAddButton
        '
        Me.MedAddButton.Location = New System.Drawing.Point(14, 7)
        Me.MedAddButton.Name = "MedAddButton"
        Me.MedAddButton.Size = New System.Drawing.Size(60, 23)
        Me.MedAddButton.TabIndex = 7
        Me.MedAddButton.Text = "Add"
        Me.MedAddButton.UseVisualStyleBackColor = True
        '
        'MedModifyButton
        '
        Me.MedModifyButton.Location = New System.Drawing.Point(78, 7)
        Me.MedModifyButton.Name = "MedModifyButton"
        Me.MedModifyButton.Size = New System.Drawing.Size(60, 23)
        Me.MedModifyButton.TabIndex = 8
        Me.MedModifyButton.Text = "Modify"
        Me.MedModifyButton.UseVisualStyleBackColor = True
        '
        'MedSaveButton
        '
        Me.MedSaveButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MedSaveButton.Enabled = False
        Me.MedSaveButton.Location = New System.Drawing.Point(390, 7)
        Me.MedSaveButton.Name = "MedSaveButton"
        Me.MedSaveButton.Size = New System.Drawing.Size(60, 23)
        Me.MedSaveButton.TabIndex = 11
        Me.MedSaveButton.Text = "Save"
        Me.MedSaveButton.UseVisualStyleBackColor = True
        '
        'MedCancelButton
        '
        Me.MedCancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MedCancelButton.CausesValidation = False
        Me.MedCancelButton.Location = New System.Drawing.Point(325, 7)
        Me.MedCancelButton.Name = "MedCancelButton"
        Me.MedCancelButton.Size = New System.Drawing.Size(60, 23)
        Me.MedCancelButton.TabIndex = 12
        Me.MedCancelButton.Text = "Cancel"
        Me.MedCancelButton.UseVisualStyleBackColor = True
        '
        'MedCoverageDataGrid
        '
        Me.MedCoverageDataGrid.ADGroupsThatCanCopy = ""
        Me.MedCoverageDataGrid.ADGroupsThatCanCustomize = ""
        Me.MedCoverageDataGrid.ADGroupsThatCanExport = ""
        Me.MedCoverageDataGrid.ADGroupsThatCanFilter = ""
        Me.MedCoverageDataGrid.ADGroupsThatCanFind = ""
        Me.MedCoverageDataGrid.ADGroupsThatCanMultiSort = ""
        Me.MedCoverageDataGrid.ADGroupsThatCanPrint = ""
        Me.MedCoverageDataGrid.AllowAutoSize = True
        Me.MedCoverageDataGrid.AllowColumnReorder = True
        Me.MedCoverageDataGrid.AllowCopy = True
        Me.MedCoverageDataGrid.AllowCustomize = True
        Me.MedCoverageDataGrid.AllowDelete = False
        Me.MedCoverageDataGrid.AllowDragDrop = True
        Me.MedCoverageDataGrid.AllowEdit = False
        Me.MedCoverageDataGrid.AllowExport = True
        Me.MedCoverageDataGrid.AllowFilter = False
        Me.MedCoverageDataGrid.AllowFind = True
        Me.MedCoverageDataGrid.AllowGoTo = True
        Me.MedCoverageDataGrid.AllowMultiSelect = False
        Me.MedCoverageDataGrid.AllowMultiSort = True
        Me.MedCoverageDataGrid.AllowNew = False
        Me.MedCoverageDataGrid.AllowPrint = True
        Me.MedCoverageDataGrid.AllowRefresh = False
        Me.MedCoverageDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MedCoverageDataGrid.AppKey = "UFCW\RegMaster\"
        Me.MedCoverageDataGrid.AutoSaveCols = True
        Me.MedCoverageDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.MedCoverageDataGrid.CaptionText = "Medical Coverage"
        Me.MedCoverageDataGrid.ColumnHeaderLabel = Nothing
        Me.MedCoverageDataGrid.ColumnRePositioning = False
        Me.MedCoverageDataGrid.ColumnResizing = False
        Me.MedCoverageDataGrid.ConfirmDelete = True
        Me.MedCoverageDataGrid.CopySelectedOnly = True
        Me.MedCoverageDataGrid.CurrentBSPosition = -1
        Me.MedCoverageDataGrid.DataMember = ""
        Me.MedCoverageDataGrid.DragColumn = 0
        Me.MedCoverageDataGrid.ExportSelectedOnly = True
        Me.MedCoverageDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.MedCoverageDataGrid.HighlightedRow = Nothing
        Me.MedCoverageDataGrid.HighLightModifiedRows = False
        Me.MedCoverageDataGrid.IsMouseDown = False
        Me.MedCoverageDataGrid.LastGoToLine = ""
        Me.MedCoverageDataGrid.Location = New System.Drawing.Point(3, 181)
        Me.MedCoverageDataGrid.MultiSort = False
        Me.MedCoverageDataGrid.Name = "MedCoverageDataGrid"
        Me.MedCoverageDataGrid.OldSelectedRow = 0
        Me.MedCoverageDataGrid.PreviousBSPosition = -1
        Me.MedCoverageDataGrid.ReadOnly = True
        Me.MedCoverageDataGrid.RetainRowSelectionAfterSort = True
        Me.MedCoverageDataGrid.SetRowOnRightClick = True
        Me.MedCoverageDataGrid.ShiftPressed = False
        Me.MedCoverageDataGrid.SingleClickBooleanColumns = True
        Me.MedCoverageDataGrid.Size = New System.Drawing.Size(458, 144)
        Me.MedCoverageDataGrid.Sort = Nothing
        Me.MedCoverageDataGrid.StyleName = ""
        Me.MedCoverageDataGrid.SubKey = ""
        Me.MedCoverageDataGrid.SuppressMouseDown = False
        Me.MedCoverageDataGrid.SuppressTriangle = False
        Me.MedCoverageDataGrid.TabIndex = 0
        Me.MedCoverageDataGrid.TabStop = False
        '
        'grpDen
        '
        Me.grpDen.Controls.Add(Me.DenUIContainer)
        Me.grpDen.Controls.Add(Me.DenCoverageDataGrid)
        Me.grpDen.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpDen.Location = New System.Drawing.Point(0, 0)
        Me.grpDen.Name = "grpDen"
        Me.grpDen.Size = New System.Drawing.Size(464, 333)
        Me.grpDen.TabIndex = 0
        Me.grpDen.TabStop = False
        Me.grpDen.Text = "Dental Coverage"
        '
        'DenUIContainer
        '
        Me.DenUIContainer.IsSplitterFixed = True
        Me.DenUIContainer.Location = New System.Drawing.Point(1, 16)
        Me.DenUIContainer.Name = "DenUIContainer"
        Me.DenUIContainer.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'DenUIContainer.Panel1
        '
        Me.DenUIContainer.Panel1.Controls.Add(Me.grpDenEditPanel)
        '
        'DenUIContainer.Panel2
        '
        Me.DenUIContainer.Panel2.Controls.Add(Me.DenDeleteButton)
        Me.DenUIContainer.Panel2.Controls.Add(Me.DenAddButton)
        Me.DenUIContainer.Panel2.Controls.Add(Me.DenModifyButton)
        Me.DenUIContainer.Panel2.Controls.Add(Me.DenSaveButton)
        Me.DenUIContainer.Panel2.Controls.Add(Me.DenCancelButton)
        Me.DenUIContainer.Size = New System.Drawing.Size(458, 163)
        Me.DenUIContainer.SplitterDistance = 128
        Me.DenUIContainer.SplitterWidth = 1
        Me.DenUIContainer.TabIndex = 0
        Me.DenUIContainer.TabStop = False
        '
        'grpDenEditPanel
        '
        Me.grpDenEditPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpDenEditPanel.Controls.Add(Me.TransparentContainer2)
        Me.grpDenEditPanel.Controls.Add(Me.txtDenThruDate)
        Me.grpDenEditPanel.Controls.Add(Me.txtDenFromDate)
        Me.grpDenEditPanel.Controls.Add(Me.DenHistoryButton)
        Me.grpDenEditPanel.Controls.Add(Me.Label7)
        Me.grpDenEditPanel.Controls.Add(Me.DenCoverageValidationPanel)
        Me.grpDenEditPanel.Controls.Add(Me.Label1)
        Me.grpDenEditPanel.Controls.Add(Me.Label2)
        Me.grpDenEditPanel.Controls.Add(Me.Label5)
        Me.grpDenEditPanel.Location = New System.Drawing.Point(1, 1)
        Me.grpDenEditPanel.Name = "grpDenEditPanel"
        Me.grpDenEditPanel.Size = New System.Drawing.Size(452, 128)
        Me.grpDenEditPanel.TabIndex = 0
        Me.grpDenEditPanel.TabStop = False
        '
        'TransparentContainer2
        '
        Me.TransparentContainer2.Controls.Add(Me.cmbDenLetter)
        Me.TransparentContainer2.Location = New System.Drawing.Point(278, 25)
        Me.TransparentContainer2.Name = "TransparentContainer2"
        Me.TransparentContainer2.Size = New System.Drawing.Size(68, 23)
        Me.TransparentContainer2.TabIndex = 15
        Me.TransparentContainer2.Text = "TransparentContainer2"
        '
        'cmbDenLetter
        '
        Me.cmbDenLetter.BackColor = System.Drawing.SystemColors.Control
        Me.cmbDenLetter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDenLetter.DropDownWidth = 50
        Me.cmbDenLetter.FormattingEnabled = True
        Me.cmbDenLetter.Items.AddRange(New Object() {"N", "Y"})
        Me.cmbDenLetter.Location = New System.Drawing.Point(0, 0)
        Me.cmbDenLetter.Name = "cmbDenLetter"
        Me.cmbDenLetter.ReadOnly = True
        Me.cmbDenLetter.Size = New System.Drawing.Size(42, 21)
        Me.cmbDenLetter.TabIndex = 16
        '
        'txtDenThruDate
        '
        Me.txtDenThruDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDenThruDate.Location = New System.Drawing.Point(102, 46)
        Me.txtDenThruDate.MaxLength = 10
        Me.txtDenThruDate.Name = "txtDenThruDate"
        Me.txtDenThruDate.ReadOnly = True
        Me.txtDenThruDate.Size = New System.Drawing.Size(91, 20)
        Me.txtDenThruDate.TabIndex = 0
        Me.txtDenThruDate.TabStop = False
        Me.ToolTip1.SetToolTip(Me.txtDenThruDate, "end date of Dental coverage")
        '
        'txtDenFromDate
        '
        Me.txtDenFromDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDenFromDate.Location = New System.Drawing.Point(102, 22)
        Me.txtDenFromDate.MaxLength = 10
        Me.txtDenFromDate.Name = "txtDenFromDate"
        Me.txtDenFromDate.ReadOnly = True
        Me.txtDenFromDate.Size = New System.Drawing.Size(91, 20)
        Me.txtDenFromDate.TabIndex = 13
        Me.ToolTip1.SetToolTip(Me.txtDenFromDate, "start date of Dental coverage")
        '
        'DenHistoryButton
        '
        Me.DenHistoryButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DenHistoryButton.CausesValidation = False
        Me.DenHistoryButton.Location = New System.Drawing.Point(386, 23)
        Me.DenHistoryButton.Name = "DenHistoryButton"
        Me.DenHistoryButton.Size = New System.Drawing.Size(60, 23)
        Me.DenHistoryButton.TabIndex = 0
        Me.DenHistoryButton.TabStop = False
        Me.DenHistoryButton.Text = "History"
        Me.DenHistoryButton.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(210, 29)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(62, 13)
        Me.Label7.TabIndex = 111
        Me.Label7.Text = "Send Letter"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'DenCoverageValidationPanel
        '
        Me.DenCoverageValidationPanel.Controls.Add(Me.cmbDenCoverageValues)
        Me.DenCoverageValidationPanel.Location = New System.Drawing.Point(102, 71)
        Me.DenCoverageValidationPanel.Name = "DenCoverageValidationPanel"
        Me.DenCoverageValidationPanel.Size = New System.Drawing.Size(241, 21)
        Me.DenCoverageValidationPanel.TabIndex = 14
        '
        'cmbDenCoverageValues
        '
        Me.cmbDenCoverageValues.BackColor = System.Drawing.SystemColors.Control
        Me.cmbDenCoverageValues.DropDownHeight = 100
        Me.cmbDenCoverageValues.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDenCoverageValues.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbDenCoverageValues.FormattingEnabled = True
        Me.cmbDenCoverageValues.IntegralHeight = False
        Me.cmbDenCoverageValues.ItemHeight = 13
        Me.cmbDenCoverageValues.Location = New System.Drawing.Point(0, 0)
        Me.cmbDenCoverageValues.MaxLength = 2
        Me.cmbDenCoverageValues.Name = "cmbDenCoverageValues"
        Me.cmbDenCoverageValues.ReadOnly = True
        Me.cmbDenCoverageValues.Size = New System.Drawing.Size(220, 21)
        Me.cmbDenCoverageValues.TabIndex = 15
        Me.ToolTip1.SetToolTip(Me.cmbDenCoverageValues, "Choose the Dental Coverage")
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 75)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(87, 13)
        Me.Label1.TabIndex = 113
        Me.Label1.Text = "Dental Coverage"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(39, 23)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 13)
        Me.Label2.TabIndex = 111
        Me.Label2.Text = "From Date"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(39, 47)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(55, 13)
        Me.Label5.TabIndex = 112
        Me.Label5.Text = "Thru Date"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'DenDeleteButton
        '
        Me.DenDeleteButton.Location = New System.Drawing.Point(136, 7)
        Me.DenDeleteButton.Name = "DenDeleteButton"
        Me.DenDeleteButton.Size = New System.Drawing.Size(60, 23)
        Me.DenDeleteButton.TabIndex = 18
        Me.DenDeleteButton.Text = "Delete"
        Me.DenDeleteButton.UseVisualStyleBackColor = True
        '
        'DenAddButton
        '
        Me.DenAddButton.Location = New System.Drawing.Point(8, 7)
        Me.DenAddButton.Name = "DenAddButton"
        Me.DenAddButton.Size = New System.Drawing.Size(60, 23)
        Me.DenAddButton.TabIndex = 16
        Me.DenAddButton.Text = "Add"
        Me.DenAddButton.UseVisualStyleBackColor = True
        '
        'DenModifyButton
        '
        Me.DenModifyButton.Location = New System.Drawing.Point(72, 7)
        Me.DenModifyButton.Name = "DenModifyButton"
        Me.DenModifyButton.Size = New System.Drawing.Size(60, 23)
        Me.DenModifyButton.TabIndex = 17
        Me.DenModifyButton.Text = "Modify"
        Me.DenModifyButton.UseVisualStyleBackColor = True
        '
        'DenSaveButton
        '
        Me.DenSaveButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DenSaveButton.Location = New System.Drawing.Point(387, 7)
        Me.DenSaveButton.Name = "DenSaveButton"
        Me.DenSaveButton.Size = New System.Drawing.Size(60, 23)
        Me.DenSaveButton.TabIndex = 20
        Me.DenSaveButton.Text = "Save"
        Me.DenSaveButton.UseVisualStyleBackColor = True
        '
        'DenCancelButton
        '
        Me.DenCancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DenCancelButton.CausesValidation = False
        Me.DenCancelButton.Location = New System.Drawing.Point(321, 7)
        Me.DenCancelButton.Name = "DenCancelButton"
        Me.DenCancelButton.Size = New System.Drawing.Size(60, 23)
        Me.DenCancelButton.TabIndex = 21
        Me.DenCancelButton.Text = "Cancel"
        Me.DenCancelButton.UseVisualStyleBackColor = True
        '
        'DenCoverageDataGrid
        '
        Me.DenCoverageDataGrid.ADGroupsThatCanCopy = ""
        Me.DenCoverageDataGrid.ADGroupsThatCanCustomize = ""
        Me.DenCoverageDataGrid.ADGroupsThatCanExport = ""
        Me.DenCoverageDataGrid.ADGroupsThatCanFilter = ""
        Me.DenCoverageDataGrid.ADGroupsThatCanFind = ""
        Me.DenCoverageDataGrid.ADGroupsThatCanMultiSort = ""
        Me.DenCoverageDataGrid.ADGroupsThatCanPrint = ""
        Me.DenCoverageDataGrid.AllowAutoSize = True
        Me.DenCoverageDataGrid.AllowColumnReorder = True
        Me.DenCoverageDataGrid.AllowCopy = True
        Me.DenCoverageDataGrid.AllowCustomize = True
        Me.DenCoverageDataGrid.AllowDelete = False
        Me.DenCoverageDataGrid.AllowDragDrop = True
        Me.DenCoverageDataGrid.AllowEdit = False
        Me.DenCoverageDataGrid.AllowExport = True
        Me.DenCoverageDataGrid.AllowFilter = False
        Me.DenCoverageDataGrid.AllowFind = True
        Me.DenCoverageDataGrid.AllowGoTo = True
        Me.DenCoverageDataGrid.AllowMultiSelect = False
        Me.DenCoverageDataGrid.AllowMultiSort = True
        Me.DenCoverageDataGrid.AllowNew = False
        Me.DenCoverageDataGrid.AllowPrint = True
        Me.DenCoverageDataGrid.AllowRefresh = False
        Me.DenCoverageDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DenCoverageDataGrid.AppKey = "UFCW\RegMaster\"
        Me.DenCoverageDataGrid.AutoSaveCols = True
        Me.DenCoverageDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.DenCoverageDataGrid.CaptionText = "Dental Coverage"
        Me.DenCoverageDataGrid.ColumnHeaderLabel = Nothing
        Me.DenCoverageDataGrid.ColumnRePositioning = False
        Me.DenCoverageDataGrid.ColumnResizing = False
        Me.DenCoverageDataGrid.ConfirmDelete = True
        Me.DenCoverageDataGrid.CopySelectedOnly = True
        Me.DenCoverageDataGrid.CurrentBSPosition = -1
        Me.DenCoverageDataGrid.DataMember = ""
        Me.DenCoverageDataGrid.DragColumn = 0
        Me.DenCoverageDataGrid.ExportSelectedOnly = True
        Me.DenCoverageDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DenCoverageDataGrid.HighlightedRow = Nothing
        Me.DenCoverageDataGrid.HighLightModifiedRows = False
        Me.DenCoverageDataGrid.IsMouseDown = False
        Me.DenCoverageDataGrid.LastGoToLine = ""
        Me.DenCoverageDataGrid.Location = New System.Drawing.Point(3, 181)
        Me.DenCoverageDataGrid.MultiSort = False
        Me.DenCoverageDataGrid.Name = "DenCoverageDataGrid"
        Me.DenCoverageDataGrid.OldSelectedRow = 0
        Me.DenCoverageDataGrid.PreviousBSPosition = -1
        Me.DenCoverageDataGrid.ReadOnly = True
        Me.DenCoverageDataGrid.RetainRowSelectionAfterSort = True
        Me.DenCoverageDataGrid.SetRowOnRightClick = True
        Me.DenCoverageDataGrid.ShiftPressed = False
        Me.DenCoverageDataGrid.SingleClickBooleanColumns = True
        Me.DenCoverageDataGrid.Size = New System.Drawing.Size(452, 147)
        Me.DenCoverageDataGrid.Sort = Nothing
        Me.DenCoverageDataGrid.StyleName = ""
        Me.DenCoverageDataGrid.SubKey = ""
        Me.DenCoverageDataGrid.SuppressMouseDown = False
        Me.DenCoverageDataGrid.SuppressTriangle = False
        Me.DenCoverageDataGrid.TabIndex = 0
        Me.DenCoverageDataGrid.TabStop = False
        '
        'ExitButton
        '
        Me.ExitButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Yes
        Me.ExitButton.Location = New System.Drawing.Point(867, 341)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(60, 23)
        Me.ExitButton.TabIndex = 2
        Me.ExitButton.Text = "Exit"
        Me.ExitButton.UseVisualStyleBackColor = True
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'CoverageElectionForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.CancelButton = Me.ExitButton
        Me.ClientSize = New System.Drawing.Size(941, 368)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(957, 406)
        Me.Name = "CoverageElectionForm"
        Me.Text = "Medical and Dental Coverage Election"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.grpmed.ResumeLayout(False)
        Me.MedUIContainer.Panel1.ResumeLayout(False)
        Me.MedUIContainer.Panel2.ResumeLayout(False)
        CType(Me.MedUIContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MedUIContainer.ResumeLayout(False)
        Me.grpMedEditPanel.ResumeLayout(False)
        Me.grpMedEditPanel.PerformLayout()
        Me.TransparentContainer1.ResumeLayout(False)
        Me.MedHMONetworkValidationPanel.ResumeLayout(False)
        Me.MedCoverageValidationPanel.ResumeLayout(False)
        CType(Me.MedCoverageDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpDen.ResumeLayout(False)
        Me.DenUIContainer.Panel1.ResumeLayout(False)
        Me.DenUIContainer.Panel2.ResumeLayout(False)
        CType(Me.DenUIContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DenUIContainer.ResumeLayout(False)
        Me.grpDenEditPanel.ResumeLayout(False)
        Me.grpDenEditPanel.PerformLayout()
        Me.TransparentContainer2.ResumeLayout(False)
        Me.DenCoverageValidationPanel.ResumeLayout(False)
        CType(Me.DenCoverageDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents grpmed As System.Windows.Forms.GroupBox
    Friend WithEvents grpDen As System.Windows.Forms.GroupBox
    Friend WithEvents MedCoverageDataGrid As DataGridCustom
    Friend WithEvents DenCoverageDataGrid As DataGridCustom
    Friend WithEvents MedAddButton As System.Windows.Forms.Button
    Friend WithEvents MedModifyButton As System.Windows.Forms.Button
    Friend WithEvents MedSaveButton As System.Windows.Forms.Button
    Friend WithEvents MedCancelButton As System.Windows.Forms.Button
    Friend WithEvents DenAddButton As System.Windows.Forms.Button
    Friend WithEvents DenModifyButton As System.Windows.Forms.Button
    Friend WithEvents DenSaveButton As System.Windows.Forms.Button
    Friend WithEvents DenCancelButton As System.Windows.Forms.Button
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents MedDeleteButton As System.Windows.Forms.Button
    Friend WithEvents DenDeleteButton As System.Windows.Forms.Button
    Friend WithEvents ExitButton As Windows.Forms.Button
    Friend WithEvents lblUIMessaging As Windows.Forms.Label
    Friend WithEvents MedUIContainer As System.Windows.Forms.SplitContainer
    Friend WithEvents grpMedEditPanel As Windows.Forms.GroupBox
    Friend WithEvents MedCoverageValidationPanel As TransparentContainer
    Friend WithEvents MedHMONetworkValidationPanel As TransparentContainer
    Friend WithEvents cmbMedCoverage As ExComboBox
    Friend WithEvents MedHistoryButton As Windows.Forms.Button
    Friend WithEvents MedHMONetworkButton As Windows.Forms.Button
    Friend WithEvents cmbHMONetwork As ExComboBox
    Friend WithEvents LabelNetwork As Windows.Forms.Label
    Friend WithEvents Label6 As Windows.Forms.Label
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents txtMedThruDate As Windows.Forms.TextBox
    Friend WithEvents txtMedFromDate As Windows.Forms.TextBox
    Friend WithEvents FromLabel As Windows.Forms.Label
    Friend WithEvents Label3 As Windows.Forms.Label
    Friend WithEvents DenUIContainer As System.Windows.Forms.SplitContainer
    Friend WithEvents grpDenEditPanel As Windows.Forms.GroupBox
    Friend WithEvents DenCoverageValidationPanel As TransparentContainer
    Friend WithEvents DenHistoryButton As Windows.Forms.Button
    Friend WithEvents Label7 As Windows.Forms.Label
    Friend WithEvents cmbDenCoverageValues As ExComboBox
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents txtDenThruDate As Windows.Forms.TextBox
    Friend WithEvents txtDenFromDate As Windows.Forms.TextBox
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents Label5 As Windows.Forms.Label
    Friend WithEvents TransparentContainer1 As TransparentContainer
    Friend WithEvents cmbMedLetter As ExComboBox
    Friend WithEvents TransparentContainer2 As TransparentContainer
    Friend WithEvents cmbDenLetter As ExComboBox
End Class
