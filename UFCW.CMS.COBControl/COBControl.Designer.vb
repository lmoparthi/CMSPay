<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class COBControl
    Inherits System.Windows.Forms.UserControl


    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.COBDataGrid = New DataGridCustom()
        Me.DataGridTableStyle1 = New System.Windows.Forms.DataGridTableStyle()
        Me.grpEditPanel = New System.Windows.Forms.GroupBox()
        Me.OTH_INS_REFUSAL_SWCheckBox = New ExCheckBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.WORKING_SPOUSE_SWCheckBox = New ExCheckBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.grpEditOIPanel = New System.Windows.Forms.GroupBox()
        Me.OTH_SEXTContainer = New TransparentContainer()
        Me.OTH_SEXComboBox = New ExComboBox()
        Me.HICNLabel = New System.Windows.Forms.Label()
        Me.HICNTextBox = New System.Windows.Forms.TextBox()
        Me.HistoryButton = New System.Windows.Forms.Button()
        Me.EXTENSION1TextBox = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.OTH_POLICYTextBox = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.OTH_SUB_ACCT_NBRTextBox = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.OTH_DOBTextBox = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.OTH_RELATIONTContainer = New TransparentContainer()
        Me.OTH_RELATIONComboBox = New ExComboBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.OTH_LNAMETextBox = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.OTH_FNAMETextBox = New System.Windows.Forms.TextBox()
        Me.OTH_SSNTextBox = New ExTextBox()
        Me.OTH_PAT_ACCT_NBRTextBox = New System.Windows.Forms.TextBox()
        Me.STATETextBox = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.COUNTRYTextBox = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.ZIP_4TextBox = New System.Windows.Forms.TextBox()
        Me.ADDRESS_LINE1TextBox = New System.Windows.Forms.TextBox()
        Me.ZIPTextBox = New System.Windows.Forms.TextBox()
        Me.CITYTextBox = New System.Windows.Forms.TextBox()
        Me.ADDRESS_LINE2TextBox = New System.Windows.Forms.TextBox()
        Me.Label55 = New System.Windows.Forms.Label()
        Me.Label56 = New System.Windows.Forms.Label()
        Me.Label58 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.PayerTContainer = New TransparentContainer()
        Me.InsurerFreeFormButton = New System.Windows.Forms.Button()
        Me.PayerComboBox = New ExComboBox()
        Me.PHONETextBox = New System.Windows.Forms.TextBox()
        Me.DOCIDTextBox = New System.Windows.Forms.TextBox()
        Me.FamilyMembersWithOILabel = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.UPDATE_REASONTextBox = New System.Windows.Forms.TextBox()
        Me.CancelActionButton = New System.Windows.Forms.Button()
        Me.PrevButton = New System.Windows.Forms.Button()
        Me.NextButton = New System.Windows.Forms.Button()
        Me.DeleteActionButton = New System.Windows.Forms.Button()
        Me.AddActionButton = New System.Windows.Forms.Button()
        Me.SaveActionButton = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.OCC_TO_DATETextBox = New System.Windows.Forms.TextBox()
        Me.OCC_FROM_DATETextBox = New System.Windows.Forms.TextBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.Panel1.SuspendLayout()
        CType(Me.COBDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpEditPanel.SuspendLayout()
        Me.grpEditOIPanel.SuspendLayout()
        Me.OTH_SEXTContainer.SuspendLayout()
        Me.OTH_RELATIONTContainer.SuspendLayout()
        Me.PayerTContainer.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.COBDataGrid)
        Me.Panel1.Controls.Add(Me.grpEditPanel)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(596, 450)
        Me.Panel1.TabIndex = 0
        '
        'COBDataGrid
        '
        Me.COBDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.COBDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.COBDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.COBDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.COBDataGrid.ADGroupsThatCanFind = ""
        Me.COBDataGrid.ADGroupsThatCanMultiSort = ""
        Me.COBDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.COBDataGrid.AllowAutoSize = True
        Me.COBDataGrid.AllowColumnReorder = True
        Me.COBDataGrid.AllowCopy = True
        Me.COBDataGrid.AllowCustomize = True
        Me.COBDataGrid.AllowDelete = False
        Me.COBDataGrid.AllowDragDrop = False
        Me.COBDataGrid.AllowEdit = False
        Me.COBDataGrid.AllowExport = True
        Me.COBDataGrid.AllowFilter = False
        Me.COBDataGrid.AllowFind = True
        Me.COBDataGrid.AllowGoTo = True
        Me.COBDataGrid.AllowMultiSelect = False
        Me.COBDataGrid.AllowMultiSort = True
        Me.COBDataGrid.AllowNew = False
        Me.COBDataGrid.AllowPrint = False
        Me.COBDataGrid.AllowRefresh = False
        Me.COBDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.COBDataGrid.AppKey = "UFCW\Claims\"
        Me.COBDataGrid.AutoSaveCols = True
        Me.COBDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.COBDataGrid.CaptionVisible = False
        Me.COBDataGrid.ColumnHeaderLabel = Nothing
        Me.COBDataGrid.ColumnRePositioning = False
        Me.COBDataGrid.ColumnResizing = False
        Me.COBDataGrid.ConfirmDelete = True
        Me.COBDataGrid.CopySelectedOnly = True
        Me.COBDataGrid.CurrentBSPosition = -1
        Me.COBDataGrid.DataMember = ""
        Me.COBDataGrid.DragColumn = 0
        Me.COBDataGrid.ExportSelectedOnly = True
        Me.COBDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.COBDataGrid.HighlightedRow = Nothing
        Me.COBDataGrid.HighLightModifiedRows = True
        Me.COBDataGrid.IsMouseDown = False
        Me.COBDataGrid.LastGoToLine = ""
        Me.COBDataGrid.Location = New System.Drawing.Point(2, 313)
        Me.COBDataGrid.MultiSort = False
        Me.COBDataGrid.Name = "COBDataGrid"
        Me.COBDataGrid.OldSelectedRow = Nothing
        Me.COBDataGrid.PreviousBSPosition = -1
        Me.COBDataGrid.ReadOnly = True
        Me.COBDataGrid.RetainRowSelectionAfterSort = True
        Me.COBDataGrid.SetRowOnRightClick = True
        Me.COBDataGrid.ShiftPressed = False
        Me.COBDataGrid.SingleClickBooleanColumns = True
        Me.COBDataGrid.Size = New System.Drawing.Size(590, 133)
        Me.COBDataGrid.Sort = Nothing
        Me.COBDataGrid.StyleName = ""
        Me.COBDataGrid.SubKey = ""
        Me.COBDataGrid.SuppressMouseDown = False
        Me.COBDataGrid.SuppressTriangle = False
        Me.COBDataGrid.TabIndex = 0
        Me.COBDataGrid.TableStyles.AddRange(New System.Windows.Forms.DataGridTableStyle() {Me.DataGridTableStyle1})
        '
        'DataGridTableStyle1
        '
        Me.DataGridTableStyle1.DataGrid = Me.COBDataGrid
        Me.DataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DataGridTableStyle1.MappingName = "MEDOTHERINSBindingSource"
        '
        'grpEditPanel
        '
        Me.grpEditPanel.Controls.Add(Me.OTH_INS_REFUSAL_SWCheckBox)
        Me.grpEditPanel.Controls.Add(Me.Label13)
        Me.grpEditPanel.Controls.Add(Me.WORKING_SPOUSE_SWCheckBox)
        Me.grpEditPanel.Controls.Add(Me.Label15)
        Me.grpEditPanel.Controls.Add(Me.grpEditOIPanel)
        Me.grpEditPanel.Controls.Add(Me.DOCIDTextBox)
        Me.grpEditPanel.Controls.Add(Me.FamilyMembersWithOILabel)
        Me.grpEditPanel.Controls.Add(Me.Label14)
        Me.grpEditPanel.Controls.Add(Me.UPDATE_REASONTextBox)
        Me.grpEditPanel.Controls.Add(Me.CancelActionButton)
        Me.grpEditPanel.Controls.Add(Me.PrevButton)
        Me.grpEditPanel.Controls.Add(Me.NextButton)
        Me.grpEditPanel.Controls.Add(Me.DeleteActionButton)
        Me.grpEditPanel.Controls.Add(Me.AddActionButton)
        Me.grpEditPanel.Controls.Add(Me.SaveActionButton)
        Me.grpEditPanel.Controls.Add(Me.Label2)
        Me.grpEditPanel.Controls.Add(Me.Label1)
        Me.grpEditPanel.Controls.Add(Me.OCC_TO_DATETextBox)
        Me.grpEditPanel.Controls.Add(Me.OCC_FROM_DATETextBox)
        Me.grpEditPanel.Location = New System.Drawing.Point(3, 3)
        Me.grpEditPanel.Name = "grpEditPanel"
        Me.grpEditPanel.Size = New System.Drawing.Size(590, 307)
        Me.grpEditPanel.TabIndex = 0
        Me.grpEditPanel.TabStop = False
        '
        'OTH_INS_REFUSAL_SWCheckBox
        '
        Me.OTH_INS_REFUSAL_SWCheckBox.AutoSize = True
        Me.OTH_INS_REFUSAL_SWCheckBox.BackColor = System.Drawing.SystemColors.Control
        Me.OTH_INS_REFUSAL_SWCheckBox.BoxState = System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedDisabled
        Me.OTH_INS_REFUSAL_SWCheckBox.ClickState = False
        Me.OTH_INS_REFUSAL_SWCheckBox.Location = New System.Drawing.Point(304, 30)
        Me.OTH_INS_REFUSAL_SWCheckBox.Name = "OTH_INS_REFUSAL_SWCheckBox"
        Me.OTH_INS_REFUSAL_SWCheckBox.ReadOnly = True
        Me.OTH_INS_REFUSAL_SWCheckBox.Size = New System.Drawing.Size(93, 17)
        Me.OTH_INS_REFUSAL_SWCheckBox.TabIndex = 5
        Me.OTH_INS_REFUSAL_SWCheckBox.Text = "O/I Declined?"
        Me.ToolTip1.SetToolTip(Me.OTH_INS_REFUSAL_SWCheckBox, "Other Insurance Declined ?")
        Me.OTH_INS_REFUSAL_SWCheckBox.UseVisualStyleBackColor = False
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(138, 284)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(82, 13)
        Me.Label13.TabIndex = 100
        Me.Label13.Text = "Update Reason"
        '
        'WORKING_SPOUSE_SWCheckBox
        '
        Me.WORKING_SPOUSE_SWCheckBox.AutoSize = True
        Me.WORKING_SPOUSE_SWCheckBox.BackColor = System.Drawing.SystemColors.Control
        Me.WORKING_SPOUSE_SWCheckBox.BoxState = System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedDisabled
        Me.WORKING_SPOUSE_SWCheckBox.ClickState = False
        Me.WORKING_SPOUSE_SWCheckBox.Location = New System.Drawing.Point(183, 30)
        Me.WORKING_SPOUSE_SWCheckBox.Name = "WORKING_SPOUSE_SWCheckBox"
        Me.WORKING_SPOUSE_SWCheckBox.ReadOnly = True
        Me.WORKING_SPOUSE_SWCheckBox.Size = New System.Drawing.Size(111, 17)
        Me.WORKING_SPOUSE_SWCheckBox.TabIndex = 111
        Me.WORKING_SPOUSE_SWCheckBox.Text = "Spouse Working?"
        Me.ToolTip1.SetToolTip(Me.WORKING_SPOUSE_SWCheckBox, "Is Spouse Working ?")
        Me.WORKING_SPOUSE_SWCheckBox.UseVisualStyleBackColor = False
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(5, 284)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(41, 13)
        Me.Label15.TabIndex = 99
        Me.Label15.Text = "Doc ID"
        Me.ToolTip1.SetToolTip(Me.Label15, "Document# representing source document that information was obtained from")
        '
        'grpEditOIPanel
        '
        Me.grpEditOIPanel.Controls.Add(Me.OTH_SEXTContainer)
        Me.grpEditOIPanel.Controls.Add(Me.HICNLabel)
        Me.grpEditOIPanel.Controls.Add(Me.HICNTextBox)
        Me.grpEditOIPanel.Controls.Add(Me.HistoryButton)
        Me.grpEditOIPanel.Controls.Add(Me.EXTENSION1TextBox)
        Me.grpEditOIPanel.Controls.Add(Me.Label19)
        Me.grpEditOIPanel.Controls.Add(Me.Label4)
        Me.grpEditOIPanel.Controls.Add(Me.Label3)
        Me.grpEditOIPanel.Controls.Add(Me.OTH_POLICYTextBox)
        Me.grpEditOIPanel.Controls.Add(Me.Label16)
        Me.grpEditOIPanel.Controls.Add(Me.OTH_SUB_ACCT_NBRTextBox)
        Me.grpEditOIPanel.Controls.Add(Me.Label12)
        Me.grpEditOIPanel.Controls.Add(Me.OTH_DOBTextBox)
        Me.grpEditOIPanel.Controls.Add(Me.Label11)
        Me.grpEditOIPanel.Controls.Add(Me.OTH_RELATIONTContainer)
        Me.grpEditOIPanel.Controls.Add(Me.Label10)
        Me.grpEditOIPanel.Controls.Add(Me.Label9)
        Me.grpEditOIPanel.Controls.Add(Me.Label8)
        Me.grpEditOIPanel.Controls.Add(Me.OTH_LNAMETextBox)
        Me.grpEditOIPanel.Controls.Add(Me.Label7)
        Me.grpEditOIPanel.Controls.Add(Me.Label6)
        Me.grpEditOIPanel.Controls.Add(Me.OTH_FNAMETextBox)
        Me.grpEditOIPanel.Controls.Add(Me.OTH_SSNTextBox)
        Me.grpEditOIPanel.Controls.Add(Me.OTH_PAT_ACCT_NBRTextBox)
        Me.grpEditOIPanel.Controls.Add(Me.STATETextBox)
        Me.grpEditOIPanel.Controls.Add(Me.Label17)
        Me.grpEditOIPanel.Controls.Add(Me.COUNTRYTextBox)
        Me.grpEditOIPanel.Controls.Add(Me.Label18)
        Me.grpEditOIPanel.Controls.Add(Me.ZIP_4TextBox)
        Me.grpEditOIPanel.Controls.Add(Me.ADDRESS_LINE1TextBox)
        Me.grpEditOIPanel.Controls.Add(Me.ZIPTextBox)
        Me.grpEditOIPanel.Controls.Add(Me.CITYTextBox)
        Me.grpEditOIPanel.Controls.Add(Me.ADDRESS_LINE2TextBox)
        Me.grpEditOIPanel.Controls.Add(Me.Label55)
        Me.grpEditOIPanel.Controls.Add(Me.Label56)
        Me.grpEditOIPanel.Controls.Add(Me.Label58)
        Me.grpEditOIPanel.Controls.Add(Me.Label5)
        Me.grpEditOIPanel.Controls.Add(Me.PayerTContainer)
        Me.grpEditOIPanel.Controls.Add(Me.PHONETextBox)
        Me.grpEditOIPanel.Location = New System.Drawing.Point(3, 53)
        Me.grpEditOIPanel.Name = "grpEditOIPanel"
        Me.grpEditOIPanel.Size = New System.Drawing.Size(584, 220)
        Me.grpEditOIPanel.TabIndex = 60
        Me.grpEditOIPanel.TabStop = False
        Me.grpEditOIPanel.Text = "Other Insurance Info"
        '
        'OTH_SEXTContainer
        '
        Me.OTH_SEXTContainer.Controls.Add(Me.OTH_SEXComboBox)
        Me.OTH_SEXTContainer.Location = New System.Drawing.Point(414, 196)
        Me.OTH_SEXTContainer.Name = "OTH_SEXTContainer"
        Me.OTH_SEXTContainer.Size = New System.Drawing.Size(61, 21)
        Me.OTH_SEXTContainer.TabIndex = 109
        '
        'OTH_SEXComboBox
        '
        Me.OTH_SEXComboBox.BackColor = System.Drawing.SystemColors.Control
        Me.OTH_SEXComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.OTH_SEXComboBox.FormattingEnabled = True
        Me.OTH_SEXComboBox.Items.AddRange(New Object() {"", "M", "F"})
        Me.OTH_SEXComboBox.Location = New System.Drawing.Point(0, 0)
        Me.OTH_SEXComboBox.Name = "OTH_SEXComboBox"
        Me.OTH_SEXComboBox.ReadOnly = True
        Me.OTH_SEXComboBox.Size = New System.Drawing.Size(61, 21)
        Me.OTH_SEXComboBox.TabIndex = 23
        Me.ToolTip1.SetToolTip(Me.OTH_SEXComboBox, "Gender of person providing Insurance - Optional")
        '
        'HICNLabel
        '
        Me.HICNLabel.AutoSize = True
        Me.HICNLabel.Location = New System.Drawing.Point(463, 139)
        Me.HICNLabel.Name = "HICNLabel"
        Me.HICNLabel.Size = New System.Drawing.Size(63, 13)
        Me.HICNLabel.TabIndex = 106
        Me.HICNLabel.Text = "HICN / MBI"
        '
        'HICNTextBox
        '
        Me.HICNTextBox.Location = New System.Drawing.Point(463, 156)
        Me.HICNTextBox.MaxLength = 30
        Me.HICNTextBox.Name = "HICNTextBox"
        Me.HICNTextBox.ReadOnly = True
        Me.HICNTextBox.Size = New System.Drawing.Size(85, 20)
        Me.HICNTextBox.TabIndex = 105
        Me.ToolTip1.SetToolTip(Me.HICNTextBox, "Medicare Identifier for Patient - Optional")
        '
        'HistoryButton
        '
        Me.HistoryButton.CausesValidation = False
        Me.HistoryButton.Location = New System.Drawing.Point(518, 13)
        Me.HistoryButton.Name = "HistoryButton"
        Me.HistoryButton.Size = New System.Drawing.Size(60, 23)
        Me.HistoryButton.TabIndex = 104
        Me.HistoryButton.Text = "History"
        Me.HistoryButton.UseVisualStyleBackColor = True
        '
        'EXTENSION1TextBox
        '
        Me.EXTENSION1TextBox.Location = New System.Drawing.Point(530, 115)
        Me.EXTENSION1TextBox.MaxLength = 10
        Me.EXTENSION1TextBox.Name = "EXTENSION1TextBox"
        Me.EXTENSION1TextBox.ReadOnly = True
        Me.EXTENSION1TextBox.Size = New System.Drawing.Size(40, 20)
        Me.EXTENSION1TextBox.TabIndex = 15
        Me.ToolTip1.SetToolTip(Me.EXTENSION1TextBox, "Other Insurers Telephone Extension (Optional)")
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(86, 119)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(22, 13)
        Me.Label19.TabIndex = 103
        Me.Label19.Text = "Zip"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(4, 19)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(39, 13)
        Me.Label4.TabIndex = 102
        Me.Label4.Text = "Insurer"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(4, 139)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(75, 13)
        Me.Label3.TabIndex = 101
        Me.Label3.Text = "Policy / Group"
        Me.ToolTip1.SetToolTip(Me.Label3, "Policy or Group # identifying other insurers plan information")
        '
        'OTH_POLICYTextBox
        '
        Me.OTH_POLICYTextBox.Location = New System.Drawing.Point(6, 156)
        Me.OTH_POLICYTextBox.MaxLength = 30
        Me.OTH_POLICYTextBox.Name = "OTH_POLICYTextBox"
        Me.OTH_POLICYTextBox.ReadOnly = True
        Me.OTH_POLICYTextBox.Size = New System.Drawing.Size(80, 20)
        Me.OTH_POLICYTextBox.TabIndex = 16
        Me.ToolTip1.SetToolTip(Me.OTH_POLICYTextBox, "Other Insurers Policy/Group # for Owner of Insurance - Optional")
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(100, 139)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(71, 13)
        Me.Label16.TabIndex = 99
        Me.Label16.Text = "Subscriber ID"
        '
        'OTH_SUB_ACCT_NBRTextBox
        '
        Me.OTH_SUB_ACCT_NBRTextBox.Location = New System.Drawing.Point(97, 156)
        Me.OTH_SUB_ACCT_NBRTextBox.MaxLength = 30
        Me.OTH_SUB_ACCT_NBRTextBox.Name = "OTH_SUB_ACCT_NBRTextBox"
        Me.OTH_SUB_ACCT_NBRTextBox.ReadOnly = True
        Me.OTH_SUB_ACCT_NBRTextBox.Size = New System.Drawing.Size(80, 20)
        Me.OTH_SUB_ACCT_NBRTextBox.TabIndex = 17
        Me.ToolTip1.SetToolTip(Me.OTH_SUB_ACCT_NBRTextBox, "Other Insurer's Subscriber Identifier for Owner of Insurance - Optional")
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(493, 179)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(30, 13)
        Me.Label12.TabIndex = 95
        Me.Label12.Text = "DOB"
        '
        'OTH_DOBTextBox
        '
        Me.OTH_DOBTextBox.Location = New System.Drawing.Point(497, 196)
        Me.OTH_DOBTextBox.MaxLength = 10
        Me.OTH_DOBTextBox.Name = "OTH_DOBTextBox"
        Me.OTH_DOBTextBox.ReadOnly = True
        Me.OTH_DOBTextBox.Size = New System.Drawing.Size(76, 20)
        Me.OTH_DOBTextBox.TabIndex = 24
        Me.ToolTip1.SetToolTip(Me.OTH_DOBTextBox, "Date of Birth of person providing Insurance - Optional")
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(277, 139)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(65, 13)
        Me.Label11.TabIndex = 94
        Me.Label11.Text = "Relationship"
        '
        'OTH_RELATIONTContainer
        '
        Me.OTH_RELATIONTContainer.Controls.Add(Me.OTH_RELATIONComboBox)
        Me.OTH_RELATIONTContainer.Location = New System.Drawing.Point(279, 156)
        Me.OTH_RELATIONTContainer.Name = "OTH_RELATIONTContainer"
        Me.OTH_RELATIONTContainer.Size = New System.Drawing.Size(80, 21)
        Me.OTH_RELATIONTContainer.TabIndex = 108
        '
        'OTH_RELATIONComboBox
        '
        Me.OTH_RELATIONComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.OTH_RELATIONComboBox.BackColor = System.Drawing.SystemColors.Control
        Me.OTH_RELATIONComboBox.DisplayMember = "RELATIONSHIP_VALUE"
        Me.OTH_RELATIONComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.OTH_RELATIONComboBox.DropDownWidth = 180
        Me.OTH_RELATIONComboBox.FormattingEnabled = True
        Me.OTH_RELATIONComboBox.Location = New System.Drawing.Point(0, 0)
        Me.OTH_RELATIONComboBox.Name = "OTH_RELATIONComboBox"
        Me.OTH_RELATIONComboBox.ReadOnly = True
        Me.OTH_RELATIONComboBox.Size = New System.Drawing.Size(80, 21)
        Me.OTH_RELATIONComboBox.TabIndex = 19
        Me.ToolTip1.SetToolTip(Me.OTH_RELATIONComboBox, "Relationship to patient of person with alternative insurance - Optional")
        Me.OTH_RELATIONComboBox.ValueMember = "RELATIONSHIP_VALUE"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(412, 179)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(42, 13)
        Me.Label10.TabIndex = 93
        Me.Label10.Text = "Gender"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(183, 179)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(58, 13)
        Me.Label9.TabIndex = 91
        Me.Label9.Text = "Last Name"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(4, 179)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(57, 13)
        Me.Label8.TabIndex = 90
        Me.Label8.Text = "First Name"
        '
        'OTH_LNAMETextBox
        '
        Me.OTH_LNAMETextBox.Location = New System.Drawing.Point(184, 196)
        Me.OTH_LNAMETextBox.MaxLength = 40
        Me.OTH_LNAMETextBox.Name = "OTH_LNAMETextBox"
        Me.OTH_LNAMETextBox.ReadOnly = True
        Me.OTH_LNAMETextBox.Size = New System.Drawing.Size(211, 20)
        Me.OTH_LNAMETextBox.TabIndex = 22
        Me.ToolTip1.SetToolTip(Me.OTH_LNAMETextBox, "Last Name of person providing Insurance - Optional")
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(187, 139)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(82, 13)
        Me.Label7.TabIndex = 89
        Me.Label7.Text = "Subscriber SSN"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(370, 139)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(54, 13)
        Me.Label6.TabIndex = 92
        Me.Label6.Text = "Patient ID"
        '
        'OTH_FNAMETextBox
        '
        Me.OTH_FNAMETextBox.Location = New System.Drawing.Point(6, 196)
        Me.OTH_FNAMETextBox.MaxLength = 40
        Me.OTH_FNAMETextBox.Name = "OTH_FNAMETextBox"
        Me.OTH_FNAMETextBox.ReadOnly = True
        Me.OTH_FNAMETextBox.Size = New System.Drawing.Size(160, 20)
        Me.OTH_FNAMETextBox.TabIndex = 21
        Me.ToolTip1.SetToolTip(Me.OTH_FNAMETextBox, "First Name of person providing Insurance - Optional")
        '
        'OTH_SSNTextBox
        '
        Me.OTH_SSNTextBox.Location = New System.Drawing.Point(188, 156)
        Me.OTH_SSNTextBox.MaxLength = 11
        Me.OTH_SSNTextBox.Name = "OTH_SSNTextBox"
        Me.OTH_SSNTextBox.ReadOnly = True
        Me.OTH_SSNTextBox.Size = New System.Drawing.Size(80, 20)
        Me.OTH_SSNTextBox.TabIndex = 18
        Me.ToolTip1.SetToolTip(Me.OTH_SSNTextBox, "SSN of Subscriber with alternative insurance - Optional")
        '
        'OTH_PAT_ACCT_NBRTextBox
        '
        Me.OTH_PAT_ACCT_NBRTextBox.Location = New System.Drawing.Point(370, 156)
        Me.OTH_PAT_ACCT_NBRTextBox.MaxLength = 30
        Me.OTH_PAT_ACCT_NBRTextBox.Name = "OTH_PAT_ACCT_NBRTextBox"
        Me.OTH_PAT_ACCT_NBRTextBox.ReadOnly = True
        Me.OTH_PAT_ACCT_NBRTextBox.Size = New System.Drawing.Size(80, 20)
        Me.OTH_PAT_ACCT_NBRTextBox.TabIndex = 20
        Me.ToolTip1.SetToolTip(Me.OTH_PAT_ACCT_NBRTextBox, "Other Insurers Identifier for Patient - Optional")
        '
        'STATETextBox
        '
        Me.STATETextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.STATETextBox.Location = New System.Drawing.Point(50, 115)
        Me.STATETextBox.MaxLength = 4
        Me.STATETextBox.Name = "STATETextBox"
        Me.STATETextBox.ReadOnly = True
        Me.STATETextBox.Size = New System.Drawing.Size(32, 20)
        Me.STATETextBox.TabIndex = 10
        Me.ToolTip1.SetToolTip(Me.STATETextBox, "Other Insurers State Code - Optional")
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(238, 119)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(43, 13)
        Me.Label17.TabIndex = 7
        Me.Label17.Text = "Country"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'COUNTRYTextBox
        '
        Me.COUNTRYTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.COUNTRYTextBox.Location = New System.Drawing.Point(286, 115)
        Me.COUNTRYTextBox.Name = "COUNTRYTextBox"
        Me.COUNTRYTextBox.ReadOnly = True
        Me.COUNTRYTextBox.Size = New System.Drawing.Size(114, 20)
        Me.COUNTRYTextBox.TabIndex = 13
        Me.ToolTip1.SetToolTip(Me.COUNTRYTextBox, "Other Insurers Country - Optional ")
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(175, 119)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(10, 13)
        Me.Label18.TabIndex = 76
        Me.Label18.Text = "-"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ZIP_4TextBox
        '
        Me.ZIP_4TextBox.Location = New System.Drawing.Point(192, 115)
        Me.ZIP_4TextBox.MaxLength = 4
        Me.ZIP_4TextBox.Name = "ZIP_4TextBox"
        Me.ZIP_4TextBox.ReadOnly = True
        Me.ZIP_4TextBox.Size = New System.Drawing.Size(42, 20)
        Me.ZIP_4TextBox.TabIndex = 12
        Me.ToolTip1.SetToolTip(Me.ZIP_4TextBox, "Other Insurers ZIP+4 - Optional")
        '
        'ADDRESS_LINE1TextBox
        '
        Me.ADDRESS_LINE1TextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ADDRESS_LINE1TextBox.Location = New System.Drawing.Point(50, 43)
        Me.ADDRESS_LINE1TextBox.MaxLength = 50
        Me.ADDRESS_LINE1TextBox.Name = "ADDRESS_LINE1TextBox"
        Me.ADDRESS_LINE1TextBox.ReadOnly = True
        Me.ADDRESS_LINE1TextBox.Size = New System.Drawing.Size(264, 20)
        Me.ADDRESS_LINE1TextBox.TabIndex = 7
        Me.ToolTip1.SetToolTip(Me.ADDRESS_LINE1TextBox, "Other Insurers Address (Line 1) - Optional")
        '
        'ZIPTextBox
        '
        Me.ZIPTextBox.Location = New System.Drawing.Point(110, 115)
        Me.ZIPTextBox.MaxLength = 5
        Me.ZIPTextBox.Name = "ZIPTextBox"
        Me.ZIPTextBox.ReadOnly = True
        Me.ZIPTextBox.Size = New System.Drawing.Size(61, 20)
        Me.ZIPTextBox.TabIndex = 11
        Me.ToolTip1.SetToolTip(Me.ZIPTextBox, "Other Insurers ZIP - Optional")
        '
        'CITYTextBox
        '
        Me.CITYTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.CITYTextBox.Location = New System.Drawing.Point(50, 90)
        Me.CITYTextBox.MaxLength = 50
        Me.CITYTextBox.Name = "CITYTextBox"
        Me.CITYTextBox.ReadOnly = True
        Me.CITYTextBox.Size = New System.Drawing.Size(264, 20)
        Me.CITYTextBox.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.CITYTextBox, "Other Insurers City - Optional")
        '
        'ADDRESS_LINE2TextBox
        '
        Me.ADDRESS_LINE2TextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ADDRESS_LINE2TextBox.Location = New System.Drawing.Point(50, 67)
        Me.ADDRESS_LINE2TextBox.MaxLength = 50
        Me.ADDRESS_LINE2TextBox.Name = "ADDRESS_LINE2TextBox"
        Me.ADDRESS_LINE2TextBox.ReadOnly = True
        Me.ADDRESS_LINE2TextBox.Size = New System.Drawing.Size(264, 20)
        Me.ADDRESS_LINE2TextBox.TabIndex = 8
        Me.ToolTip1.SetToolTip(Me.ADDRESS_LINE2TextBox, "Other Insurers Address (Line 2) - Optional")
        '
        'Label55
        '
        Me.Label55.AutoSize = True
        Me.Label55.Location = New System.Drawing.Point(4, 119)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(32, 13)
        Me.Label55.TabIndex = 72
        Me.Label55.Text = "State"
        Me.Label55.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label56
        '
        Me.Label56.AutoSize = True
        Me.Label56.Location = New System.Drawing.Point(4, 46)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(45, 13)
        Me.Label56.TabIndex = 71
        Me.Label56.Text = "Address"
        Me.Label56.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label58
        '
        Me.Label58.AutoSize = True
        Me.Label58.Location = New System.Drawing.Point(4, 93)
        Me.Label58.Name = "Label58"
        Me.Label58.Size = New System.Drawing.Size(24, 13)
        Me.Label58.TabIndex = 69
        Me.Label58.Text = "City"
        Me.Label58.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(404, 119)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(38, 13)
        Me.Label5.TabIndex = 67
        Me.Label5.Text = "Phone"
        '
        'PayerTContainer
        '
        Me.PayerTContainer.Controls.Add(Me.InsurerFreeFormButton)
        Me.PayerTContainer.Controls.Add(Me.PayerComboBox)
        Me.PayerTContainer.Location = New System.Drawing.Point(50, 16)
        Me.PayerTContainer.Name = "PayerTContainer"
        Me.PayerTContainer.Size = New System.Drawing.Size(305, 21)
        Me.PayerTContainer.TabIndex = 107
        '
        'InsurerFreeFormButton
        '
        Me.InsurerFreeFormButton.Location = New System.Drawing.Point(238, 0)
        Me.InsurerFreeFormButton.Name = "InsurerFreeFormButton"
        Me.InsurerFreeFormButton.Size = New System.Drawing.Size(25, 21)
        Me.InsurerFreeFormButton.TabIndex = 6
        Me.InsurerFreeFormButton.Text = "..."
        Me.ToolTip1.SetToolTip(Me.InsurerFreeFormButton, "Click to Enter Freeform Insurer name (when Insurer is not identified in drop down" &
        " list)")
        Me.InsurerFreeFormButton.UseVisualStyleBackColor = True
        '
        'PayerComboBox
        '
        Me.PayerComboBox.BackColor = System.Drawing.SystemColors.Control
        Me.PayerComboBox.DisplayMember = "PAYER_ID"
        Me.PayerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.PayerComboBox.DropDownWidth = 500
        Me.PayerComboBox.FormattingEnabled = True
        Me.PayerComboBox.Location = New System.Drawing.Point(0, 0)
        Me.PayerComboBox.MaxDropDownItems = 10
        Me.PayerComboBox.Name = "PayerComboBox"
        Me.PayerComboBox.ReadOnly = True
        Me.PayerComboBox.Size = New System.Drawing.Size(221, 21)
        Me.PayerComboBox.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.PayerComboBox, "Alternative insurance provider")
        Me.PayerComboBox.ValueMember = "PAYER_ID"
        '
        'PHONETextBox
        '
        Me.PHONETextBox.Location = New System.Drawing.Point(448, 115)
        Me.PHONETextBox.MaxLength = 10
        Me.PHONETextBox.Name = "PHONETextBox"
        Me.PHONETextBox.ReadOnly = True
        Me.PHONETextBox.Size = New System.Drawing.Size(76, 20)
        Me.PHONETextBox.TabIndex = 14
        Me.ToolTip1.SetToolTip(Me.PHONETextBox, "Other Insurers Telephone 10 digits Max (optional)")
        '
        'DOCIDTextBox
        '
        Me.DOCIDTextBox.Location = New System.Drawing.Point(52, 281)
        Me.DOCIDTextBox.MaxLength = 8
        Me.DOCIDTextBox.Name = "DOCIDTextBox"
        Me.DOCIDTextBox.ReadOnly = True
        Me.DOCIDTextBox.Size = New System.Drawing.Size(79, 20)
        Me.DOCIDTextBox.TabIndex = 25
        Me.ToolTip1.SetToolTip(Me.DOCIDTextBox, "Source Document ID for the information posted ")
        '
        'FamilyMembersWithOILabel
        '
        Me.FamilyMembersWithOILabel.AutoSize = True
        Me.FamilyMembersWithOILabel.Location = New System.Drawing.Point(560, 32)
        Me.FamilyMembersWithOILabel.Name = "FamilyMembersWithOILabel"
        Me.FamilyMembersWithOILabel.Size = New System.Drawing.Size(13, 13)
        Me.FamilyMembersWithOILabel.TabIndex = 43
        Me.FamilyMembersWithOILabel.Text = "0"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(414, 32)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(143, 13)
        Me.Label14.TabIndex = 42
        Me.Label14.Text = "# of Family Members with OI:"
        '
        'UPDATE_REASONTextBox
        '
        Me.UPDATE_REASONTextBox.Location = New System.Drawing.Point(226, 281)
        Me.UPDATE_REASONTextBox.MaxLength = 30
        Me.UPDATE_REASONTextBox.Name = "UPDATE_REASONTextBox"
        Me.UPDATE_REASONTextBox.ReadOnly = True
        Me.UPDATE_REASONTextBox.Size = New System.Drawing.Size(171, 20)
        Me.UPDATE_REASONTextBox.TabIndex = 26
        Me.ToolTip1.SetToolTip(Me.UPDATE_REASONTextBox, "Specify reason for update")
        '
        'CancelActionButton
        '
        Me.CancelActionButton.CausesValidation = False
        Me.CancelActionButton.Enabled = False
        Me.CancelActionButton.Location = New System.Drawing.Point(458, 279)
        Me.CancelActionButton.Name = "CancelActionButton"
        Me.CancelActionButton.Size = New System.Drawing.Size(60, 23)
        Me.CancelActionButton.TabIndex = 27
        Me.CancelActionButton.Text = "Cancel"
        Me.ToolTip1.SetToolTip(Me.CancelActionButton, "Cancel pending changes (Changes made since last save)")
        Me.CancelActionButton.UseVisualStyleBackColor = True
        '
        'PrevButton
        '
        Me.PrevButton.BackColor = System.Drawing.Color.Silver
        Me.PrevButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PrevButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.PrevButton.Location = New System.Drawing.Point(549, -1)
        Me.PrevButton.Name = "PrevButton"
        Me.PrevButton.Size = New System.Drawing.Size(16, 15)
        Me.PrevButton.TabIndex = 1
        Me.PrevButton.Text = "<"
        Me.ToolTip1.SetToolTip(Me.PrevButton, "Move to previous line")
        Me.PrevButton.UseVisualStyleBackColor = False
        '
        'NextButton
        '
        Me.NextButton.BackColor = System.Drawing.Color.Silver
        Me.NextButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NextButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.NextButton.Location = New System.Drawing.Point(568, -1)
        Me.NextButton.Name = "NextButton"
        Me.NextButton.Size = New System.Drawing.Size(16, 15)
        Me.NextButton.TabIndex = 2
        Me.NextButton.Text = ">"
        Me.ToolTip1.SetToolTip(Me.NextButton, "Move to next line")
        Me.NextButton.UseVisualStyleBackColor = False
        '
        'DeleteActionButton
        '
        Me.DeleteActionButton.BackColor = System.Drawing.Color.Silver
        Me.DeleteActionButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DeleteActionButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.DeleteActionButton.Location = New System.Drawing.Point(511, -1)
        Me.DeleteActionButton.Name = "DeleteActionButton"
        Me.DeleteActionButton.Size = New System.Drawing.Size(16, 15)
        Me.DeleteActionButton.TabIndex = 0
        Me.DeleteActionButton.Text = "-"
        Me.ToolTip1.SetToolTip(Me.DeleteActionButton, "Delete selected line")
        Me.DeleteActionButton.UseVisualStyleBackColor = False
        '
        'AddActionButton
        '
        Me.AddActionButton.BackColor = System.Drawing.Color.Silver
        Me.AddActionButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AddActionButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.AddActionButton.Location = New System.Drawing.Point(530, -1)
        Me.AddActionButton.Name = "AddActionButton"
        Me.AddActionButton.Size = New System.Drawing.Size(16, 15)
        Me.AddActionButton.TabIndex = 2
        Me.AddActionButton.Text = "+"
        Me.ToolTip1.SetToolTip(Me.AddActionButton, "Add new line")
        Me.AddActionButton.UseVisualStyleBackColor = False
        '
        'SaveActionButton
        '
        Me.SaveActionButton.CausesValidation = False
        Me.SaveActionButton.Enabled = False
        Me.SaveActionButton.Location = New System.Drawing.Point(524, 279)
        Me.SaveActionButton.Name = "SaveActionButton"
        Me.SaveActionButton.Size = New System.Drawing.Size(60, 23)
        Me.SaveActionButton.TabIndex = 28
        Me.SaveActionButton.Text = "Save"
        Me.ToolTip1.SetToolTip(Me.SaveActionButton, "Save Pending changes (changes mades since last save)")
        Me.SaveActionButton.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(92, 11)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(76, 13)
        Me.Label2.TabIndex = 29
        Me.Label2.Text = "Insurance End"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 13)
        Me.Label1.TabIndex = 28
        Me.Label1.Text = "Insurance Start"
        '
        'OCC_TO_DATETextBox
        '
        Me.OCC_TO_DATETextBox.Location = New System.Drawing.Point(92, 29)
        Me.OCC_TO_DATETextBox.MaxLength = 10
        Me.OCC_TO_DATETextBox.Name = "OCC_TO_DATETextBox"
        Me.OCC_TO_DATETextBox.ReadOnly = True
        Me.OCC_TO_DATETextBox.Size = New System.Drawing.Size(76, 20)
        Me.OCC_TO_DATETextBox.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.OCC_TO_DATETextBox, "End Date availability (mm/dd/yyyy) of other insurance. Use End of Year 12/31 if u" &
        "nsure, or unknown.")
        '
        'OCC_FROM_DATETextBox
        '
        Me.OCC_FROM_DATETextBox.Location = New System.Drawing.Point(6, 29)
        Me.OCC_FROM_DATETextBox.MaxLength = 10
        Me.OCC_FROM_DATETextBox.Name = "OCC_FROM_DATETextBox"
        Me.OCC_FROM_DATETextBox.ReadOnly = True
        Me.OCC_FROM_DATETextBox.Size = New System.Drawing.Size(76, 20)
        Me.OCC_FROM_DATETextBox.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.OCC_FROM_DATETextBox, "Start Date availability (mm/dd/yyyy) of other insurance")
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'COBControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.Controls.Add(Me.Panel1)
        Me.Name = "COBControl"
        Me.Size = New System.Drawing.Size(596, 450)
        Me.Panel1.ResumeLayout(False)
        CType(Me.COBDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpEditPanel.ResumeLayout(False)
        Me.grpEditPanel.PerformLayout()
        Me.grpEditOIPanel.ResumeLayout(False)
        Me.grpEditOIPanel.PerformLayout()
        Me.OTH_SEXTContainer.ResumeLayout(False)
        Me.OTH_RELATIONTContainer.ResumeLayout(False)
        Me.PayerTContainer.ResumeLayout(False)
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents COBDataGrid As DataGridCustom
    Friend WithEvents grpEditPanel As System.Windows.Forms.GroupBox
    Friend WithEvents OCC_TO_DATETextBox As System.Windows.Forms.TextBox
    Friend WithEvents OCC_FROM_DATETextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents SaveActionButton As System.Windows.Forms.Button
    Friend WithEvents PrevButton As System.Windows.Forms.Button
    Friend WithEvents NextButton As System.Windows.Forms.Button
    Friend WithEvents DeleteActionButton As System.Windows.Forms.Button
    Friend WithEvents AddActionButton As System.Windows.Forms.Button
    Friend WithEvents CancelActionButton As System.Windows.Forms.Button
    Friend WithEvents DataGridTableStyle1 As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents UPDATE_REASONTextBox As System.Windows.Forms.TextBox
    Friend WithEvents FamilyMembersWithOILabel As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents DOCIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents grpEditOIPanel As System.Windows.Forms.GroupBox
    Friend WithEvents STATETextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents COUNTRYTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents ZIP_4TextBox As System.Windows.Forms.TextBox
    Friend WithEvents ADDRESS_LINE1TextBox As System.Windows.Forms.TextBox
    Friend WithEvents ZIPTextBox As System.Windows.Forms.TextBox
    Friend WithEvents CITYTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ADDRESS_LINE2TextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents Label56 As System.Windows.Forms.Label
    Friend WithEvents Label58 As System.Windows.Forms.Label
    Friend WithEvents InsurerFreeFormButton As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents PayerComboBox As ExComboBox
    Friend WithEvents PHONETextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents OTH_POLICYTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents OTH_SUB_ACCT_NBRTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents OTH_DOBTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents OTH_RELATIONComboBox As ExComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents OTH_SEXComboBox As ExComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents OTH_LNAMETextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents OTH_FNAMETextBox As System.Windows.Forms.TextBox
    Friend WithEvents OTH_SSNTextBox As ExTextBox
    Friend WithEvents OTH_PAT_ACCT_NBRTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents EXTENSION1TextBox As System.Windows.Forms.TextBox
    Friend WithEvents HistoryButton As System.Windows.Forms.Button
    Friend WithEvents HICNLabel As Label
    Friend WithEvents HICNTextBox As TextBox
    Friend WithEvents PayerTContainer As TransparentContainer
    Friend WithEvents OTH_SEXTContainer As TransparentContainer
    Friend WithEvents OTH_RELATIONTContainer As TransparentContainer
    Friend WithEvents OTH_INS_REFUSAL_SWCheckBox As ExCheckBox
    Friend WithEvents WORKING_SPOUSE_SWCheckBox As ExCheckBox
End Class
