<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class NPIRegistryControl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim DataGridNPILicenses As DataGridCustom
        Me.NPIREGISTRYLICENSESBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.ProvDS = New ProvDS
        Me.NPI_REGISTRY_LICENSES = New System.Windows.Forms.DataGridTableStyle
        Me.HLTHCARE_PROV_TXNMY_CD = New System.Windows.Forms.DataGridTextBoxColumn
        Me.PROV_LIC_NUM = New System.Windows.Forms.DataGridTextBoxColumn
        Me.PROV_LIC_NUM_STATE_CD = New System.Windows.Forms.DataGridTextBoxColumn
        Me.HLTHCARE_PROV_PRIM_TXNMY_SW = New System.Windows.Forms.DataGridTextBoxColumn
        Me.NPIGroupBox = New System.Windows.Forms.GroupBox
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.Label21 = New System.Windows.Forms.Label
        Me.Label22 = New System.Windows.Forms.Label
        Me.TextBox44 = New System.Windows.Forms.TextBox
        Me.NPIREGISTRYBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.Label23 = New System.Windows.Forms.Label
        Me.TextBox46 = New System.Windows.Forms.TextBox
        Me.TextBox47 = New System.Windows.Forms.TextBox
        Me.Label19 = New System.Windows.Forms.Label
        Me.TextBox41 = New System.Windows.Forms.TextBox
        Me.TextBox40 = New System.Windows.Forms.TextBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.TextBox39 = New System.Windows.Forms.TextBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.TextBox38 = New System.Windows.Forms.TextBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.OrgLegalBusinessNameTextBox = New System.Windows.Forms.TextBox
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.TextBox37 = New System.Windows.Forms.TextBox
        Me.TextBox33 = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.TextBox34 = New System.Windows.Forms.TextBox
        Me.TextBox35 = New System.Windows.Forms.TextBox
        Me.TextBox36 = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.TextBox32 = New System.Windows.Forms.TextBox
        Me.TextBox31 = New System.Windows.Forms.TextBox
        Me.TextBox30 = New System.Windows.Forms.TextBox
        Me.TextBox29 = New System.Windows.Forms.TextBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Label26 = New System.Windows.Forms.Label
        Me.Label25 = New System.Windows.Forms.Label
        Me.TextBox21 = New System.Windows.Forms.TextBox
        Me.TextBox22 = New System.Windows.Forms.TextBox
        Me.TextBox23 = New System.Windows.Forms.TextBox
        Me.TextBox24 = New System.Windows.Forms.TextBox
        Me.TextBox25 = New System.Windows.Forms.TextBox
        Me.TextBox26 = New System.Windows.Forms.TextBox
        Me.TextBox27 = New System.Windows.Forms.TextBox
        Me.TextBox28 = New System.Windows.Forms.TextBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label24 = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.TextBox20 = New System.Windows.Forms.TextBox
        Me.TextBox19 = New System.Windows.Forms.TextBox
        Me.TextBox18 = New System.Windows.Forms.TextBox
        Me.TextBox17 = New System.Windows.Forms.TextBox
        Me.TextBox16 = New System.Windows.Forms.TextBox
        Me.TextBox15 = New System.Windows.Forms.TextBox
        Me.TextBox14 = New System.Windows.Forms.TextBox
        Me.TextBox13 = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.TextBox12 = New System.Windows.Forms.TextBox
        Me.TextBox7 = New System.Windows.Forms.TextBox
        Me.TextBox8 = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.TextBox9 = New System.Windows.Forms.TextBox
        Me.TextBox10 = New System.Windows.Forms.TextBox
        Me.TextBox11 = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.TextBox6 = New System.Windows.Forms.TextBox
        Me.TextBox5 = New System.Windows.Forms.TextBox
        Me.TextBox4 = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.DataGridNPIProviderOther = New DataGridCustom
        Me.NPIREGISTRYOTHERPROVIDERSBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.NPIREGISTRYOTHERPROVIDERS = New System.Windows.Forms.DataGridTableStyle
        Me.OTH_PROV_IDENT = New System.Windows.Forms.DataGridTextBoxColumn
        Me.OTH_PROV_IDENT_TYPE_CD = New System.Windows.Forms.DataGridTextBoxColumn
        Me.OTH_PROV_IDENT_STATE = New System.Windows.Forms.DataGridTextBoxColumn
        Me.OTH_PROV_IDENT_ISSUER = New System.Windows.Forms.DataGridTextBoxColumn
        Me.ReplacementNPITextBox = New System.Windows.Forms.TextBox
        Me.EntityTypeTextBox = New System.Windows.Forms.TextBox
        Me.NPITextBox = New System.Windows.Forms.TextBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        DataGridNPILicenses = New DataGridCustom
        CType(DataGridNPILicenses, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NPIREGISTRYLICENSESBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()









        CType(Me.ProvDS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.NPIGroupBox.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        CType(Me.NPIREGISTRYBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()

        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.DataGridNPIProviderOther, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NPIREGISTRYOTHERPROVIDERSBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DataGridNPILicenses
        '
        DataGridNPILicenses.ADGroupsThatCanCopy = "CMSUsers"
        DataGridNPILicenses.ADGroupsThatCanCustomize = "CMSUsers"
        DataGridNPILicenses.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        DataGridNPILicenses.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        DataGridNPILicenses.ADGroupsThatCanFind = ""
        DataGridNPILicenses.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        DataGridNPILicenses.ADGroupsThatCanMultiSort = ""
        DataGridNPILicenses.AllowAutoSize = True
        DataGridNPILicenses.AllowColumnReorder = True
        DataGridNPILicenses.AllowCopy = True
        DataGridNPILicenses.AllowCustomize = True
        DataGridNPILicenses.AllowDelete = False
        DataGridNPILicenses.AllowDragDrop = False
        DataGridNPILicenses.AllowEdit = False
        DataGridNPILicenses.AllowExport = True
        DataGridNPILicenses.AllowFilter = False
        DataGridNPILicenses.AllowFind = True
        DataGridNPILicenses.AllowGoTo = True
        DataGridNPILicenses.AllowMultiSelect = False
        DataGridNPILicenses.AllowMultiSort = False
        DataGridNPILicenses.AllowNew = False
        DataGridNPILicenses.AllowPrint = False
        DataGridNPILicenses.AllowRefresh = False
        DataGridNPILicenses.AppKey = "UFCW\Claims\"
        DataGridNPILicenses.BackgroundColor = System.Drawing.SystemColors.Window
        DataGridNPILicenses.CaptionText = "Provider Taxonomy"
        DataGridNPILicenses.ConfirmDelete = True
        DataGridNPILicenses.CopySelectedOnly = True
        DataGridNPILicenses.DataMember = ""
        DataGridNPILicenses.DataSource = Me.NPIREGISTRYLICENSESBindingSource
        DataGridNPILicenses.Dock = System.Windows.Forms.DockStyle.Fill
        DataGridNPILicenses.ExportSelectedOnly = True
        DataGridNPILicenses.HeaderForeColor = System.Drawing.SystemColors.ControlText
        DataGridNPILicenses.LastGoToLine = ""
        DataGridNPILicenses.Location = New System.Drawing.Point(0, 0)
        DataGridNPILicenses.MultiSort = False
        DataGridNPILicenses.Name = "DataGridNPILicenses"
        DataGridNPILicenses.ReadOnly = True
        DataGridNPILicenses.SetRowOnRightClick = True
        DataGridNPILicenses.SingleClickBooleanColumns = True
        DataGridNPILicenses.Size = New System.Drawing.Size(523, 88)
        DataGridNPILicenses.SuppressTriangle = False
        DataGridNPILicenses.TabIndex = 0
        DataGridNPILicenses.TableStyles.AddRange(New System.Windows.Forms.DataGridTableStyle() {Me.NPI_REGISTRY_LICENSES})
        '
        'NPIREGISTRYLICENSESBindingSource
        '
        Me.NPIREGISTRYLICENSESBindingSource.DataMember = "NPI_REGISTRY_LICENSES"
        Me.NPIREGISTRYLICENSESBindingSource.DataSource = Me.ProvDS
        '
        'ProvDS
        '
        Me.ProvDS.DataSetName = "ProvDS"
        Me.ProvDS.EnforceConstraints = False
        Me.ProvDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema





        '
        'NPI_REGISTRY_LICENSES
        '
        Me.NPI_REGISTRY_LICENSES.DataGrid = DataGridNPILicenses
        Me.NPI_REGISTRY_LICENSES.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.HLTHCARE_PROV_TXNMY_CD, Me.PROV_LIC_NUM, Me.PROV_LIC_NUM_STATE_CD, Me.HLTHCARE_PROV_PRIM_TXNMY_SW})
        Me.NPI_REGISTRY_LICENSES.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.NPI_REGISTRY_LICENSES.MappingName = "NPI_REGISTRY_LICENSES"
        Me.NPI_REGISTRY_LICENSES.ReadOnly = True
        Me.NPI_REGISTRY_LICENSES.RowHeadersVisible = False
        '
        'HLTHCARE_PROV_TXNMY_CD
        '
        Me.HLTHCARE_PROV_TXNMY_CD.Format = ""
        Me.HLTHCARE_PROV_TXNMY_CD.FormatInfo = Nothing
        Me.HLTHCARE_PROV_TXNMY_CD.HeaderText = "Taxonomy"
        Me.HLTHCARE_PROV_TXNMY_CD.MappingName = "HLTHCARE_PROV_TXNMY_CD"
        Me.HLTHCARE_PROV_TXNMY_CD.NullText = ""
        Me.HLTHCARE_PROV_TXNMY_CD.ReadOnly = True
        Me.HLTHCARE_PROV_TXNMY_CD.Width = 75
        '
        'PROV_LIC_NUM
        '
        Me.PROV_LIC_NUM.Format = ""
        Me.PROV_LIC_NUM.FormatInfo = Nothing
        Me.PROV_LIC_NUM.HeaderText = "License"
        Me.PROV_LIC_NUM.MappingName = "PROV_LIC_NUM"
        Me.PROV_LIC_NUM.NullText = ""
        Me.PROV_LIC_NUM.ReadOnly = True
        Me.PROV_LIC_NUM.Width = 75
        '
        'PROV_LIC_NUM_STATE_CD
        '
        Me.PROV_LIC_NUM_STATE_CD.Format = ""
        Me.PROV_LIC_NUM_STATE_CD.FormatInfo = Nothing
        Me.PROV_LIC_NUM_STATE_CD.HeaderText = "Licensing State "
        Me.PROV_LIC_NUM_STATE_CD.MappingName = "PROV_LIC_NUM_STATE_CD"
        Me.PROV_LIC_NUM_STATE_CD.NullText = ""
        Me.PROV_LIC_NUM_STATE_CD.ReadOnly = True
        Me.PROV_LIC_NUM_STATE_CD.Width = 90
        '
        'HLTHCARE_PROV_PRIM_TXNMY_SW
        '
        Me.HLTHCARE_PROV_PRIM_TXNMY_SW.Format = ""
        Me.HLTHCARE_PROV_PRIM_TXNMY_SW.FormatInfo = Nothing
        Me.HLTHCARE_PROV_PRIM_TXNMY_SW.HeaderText = "Primary Taxonomy"
        Me.HLTHCARE_PROV_PRIM_TXNMY_SW.MappingName = "HLTHCARE_PROV_PRIM_TXNMY_SW"
        Me.HLTHCARE_PROV_PRIM_TXNMY_SW.NullText = ""
        Me.HLTHCARE_PROV_PRIM_TXNMY_SW.ReadOnly = True
        Me.HLTHCARE_PROV_PRIM_TXNMY_SW.Width = 110
        '
        'NPIGroupBox
        '
        Me.NPIGroupBox.Controls.Add(Me.GroupBox4)
        Me.NPIGroupBox.Controls.Add(Me.Label19)
        Me.NPIGroupBox.Controls.Add(Me.TextBox41)
        Me.NPIGroupBox.Controls.Add(Me.TextBox40)
        Me.NPIGroupBox.Controls.Add(Me.Label18)
        Me.NPIGroupBox.Controls.Add(Me.Label17)
        Me.NPIGroupBox.Controls.Add(Me.TextBox39)
        Me.NPIGroupBox.Controls.Add(Me.Label16)
        Me.NPIGroupBox.Controls.Add(Me.TextBox38)
        Me.NPIGroupBox.Controls.Add(Me.Label15)
        Me.NPIGroupBox.Controls.Add(Me.Label14)
        Me.NPIGroupBox.Controls.Add(Me.Label13)
        Me.NPIGroupBox.Controls.Add(Me.Label4)
        Me.NPIGroupBox.Controls.Add(Me.OrgLegalBusinessNameTextBox)
        Me.NPIGroupBox.Controls.Add(Me.GroupBox3)
        Me.NPIGroupBox.Controls.Add(Me.Label9)
        Me.NPIGroupBox.Controls.Add(Me.TextBox32)
        Me.NPIGroupBox.Controls.Add(Me.TextBox31)
        Me.NPIGroupBox.Controls.Add(Me.TextBox30)
        Me.NPIGroupBox.Controls.Add(Me.TextBox29)
        Me.NPIGroupBox.Controls.Add(Me.GroupBox2)
        Me.NPIGroupBox.Controls.Add(Me.GroupBox1)
        Me.NPIGroupBox.Controls.Add(Me.Label8)
        Me.NPIGroupBox.Controls.Add(Me.TextBox12)
        Me.NPIGroupBox.Controls.Add(Me.TextBox7)
        Me.NPIGroupBox.Controls.Add(Me.TextBox8)
        Me.NPIGroupBox.Controls.Add(Me.Label7)
        Me.NPIGroupBox.Controls.Add(Me.TextBox9)
        Me.NPIGroupBox.Controls.Add(Me.TextBox10)
        Me.NPIGroupBox.Controls.Add(Me.TextBox11)
        Me.NPIGroupBox.Controls.Add(Me.Label6)
        Me.NPIGroupBox.Controls.Add(Me.TextBox6)
        Me.NPIGroupBox.Controls.Add(Me.TextBox5)
        Me.NPIGroupBox.Controls.Add(Me.TextBox4)
        Me.NPIGroupBox.Controls.Add(Me.Label5)
        Me.NPIGroupBox.Controls.Add(Me.TextBox3)
        Me.NPIGroupBox.Controls.Add(Me.TextBox2)
        Me.NPIGroupBox.Controls.Add(Me.TextBox1)
        Me.NPIGroupBox.Controls.Add(Me.Label3)
        Me.NPIGroupBox.Controls.Add(Me.Label2)
        Me.NPIGroupBox.Controls.Add(Me.Label1)
        Me.NPIGroupBox.Controls.Add(Me.SplitContainer1)
        Me.NPIGroupBox.Controls.Add(Me.ReplacementNPITextBox)
        Me.NPIGroupBox.Controls.Add(Me.EntityTypeTextBox)
        Me.NPIGroupBox.Controls.Add(Me.NPITextBox)
        Me.NPIGroupBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.NPIGroupBox.Location = New System.Drawing.Point(0, 0)
        Me.NPIGroupBox.Name = "NPIGroupBox"
        Me.NPIGroupBox.Size = New System.Drawing.Size(524, 751)
        Me.NPIGroupBox.TabIndex = 0
        Me.NPIGroupBox.TabStop = False
        Me.NPIGroupBox.Text = "National NPI Registry"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.Label21)
        Me.GroupBox4.Controls.Add(Me.Label22)
        Me.GroupBox4.Controls.Add(Me.TextBox44)
        Me.GroupBox4.Controls.Add(Me.Label23)
        Me.GroupBox4.Controls.Add(Me.TextBox46)
        Me.GroupBox4.Controls.Add(Me.TextBox47)
        Me.GroupBox4.Location = New System.Drawing.Point(5, 181)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(515, 60)
        Me.GroupBox4.TabIndex = 64
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Parent Organization Info"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(87, 19)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(35, 13)
        Me.Label21.TabIndex = 55
        Me.Label21.Text = "Name"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(87, 41)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(25, 13)
        Me.Label22.TabIndex = 54
        Me.Label22.Text = "TIN"
        '
        'TextBox44
        '
        Me.TextBox44.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PARENT_ORG_TIN", True))
        Me.TextBox44.Location = New System.Drawing.Point(128, 38)
        Me.TextBox44.Name = "TextBox44"
        Me.TextBox44.ReadOnly = True
        Me.TextBox44.Size = New System.Drawing.Size(65, 20)
        Me.TextBox44.TabIndex = 52
        Me.ToolTip1.SetToolTip(Me.TextBox44, "Provider Organization Subpart TIN")
        '
        'NPIREGISTRYBindingSource
        '
        Me.NPIREGISTRYBindingSource.DataMember = "NPI_REGISTRY"
        Me.NPIREGISTRYBindingSource.DataSource = Me.ProvDS
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(1, 19)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(45, 13)
        Me.Label23.TabIndex = 51
        Me.Label23.Text = "SubPart"
        '
        'TextBox46
        '
        Me.TextBox46.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PARENT_ORG_LBN", True))
        Me.TextBox46.Location = New System.Drawing.Point(128, 16)
        Me.TextBox46.Name = "TextBox46"
        Me.TextBox46.ReadOnly = True
        Me.TextBox46.Size = New System.Drawing.Size(364, 20)
        Me.TextBox46.TabIndex = 49
        Me.ToolTip1.SetToolTip(Me.TextBox46, "Provider Organization Subpart Legal Business Name")
        '
        'TextBox47
        '
        Me.TextBox47.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "IS_ORG_SUBPART", True))
        Me.TextBox47.Location = New System.Drawing.Point(50, 16)
        Me.TextBox47.Name = "TextBox47"
        Me.TextBox47.ReadOnly = True
        Me.TextBox47.Size = New System.Drawing.Size(19, 20)
        Me.TextBox47.TabIndex = 48
        Me.ToolTip1.SetToolTip(Me.TextBox47, "Indicates Provider is part of the following group")
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(266, 119)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(56, 13)
        Me.Label19.TabIndex = 61
        Me.Label19.Text = "Sole Prop."
        '
        'TextBox41
        '
        Me.TextBox41.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "IS_SOLE_PROPRIETOR", True))
        Me.TextBox41.Location = New System.Drawing.Point(338, 115)
        Me.TextBox41.Name = "TextBox41"
        Me.TextBox41.ReadOnly = True
        Me.TextBox41.Size = New System.Drawing.Size(23, 20)
        Me.TextBox41.TabIndex = 60
        Me.ToolTip1.SetToolTip(Me.TextBox41, "Provider Gender")
        '
        'TextBox40
        '
        Me.TextBox40.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "EMP_IDENTIFICATION_NUM_EIN", True))
        Me.TextBox40.Location = New System.Drawing.Point(135, 19)
        Me.TextBox40.Name = "TextBox40"
        Me.TextBox40.ReadOnly = True
        Me.TextBox40.Size = New System.Drawing.Size(65, 20)
        Me.TextBox40.TabIndex = 59
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(108, 23)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(25, 13)
        Me.Label18.TabIndex = 58
        Me.Label18.Text = "EIN"
        '
        'Label17
        '
        Me.Label17.Location = New System.Drawing.Point(314, 48)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(44, 13)
        Me.Label17.TabIndex = 57
        Me.Label17.Text = "Reason"
        '
        'TextBox39
        '
        Me.TextBox39.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "NPI_DEACT_REASON_CD", True))
        Me.TextBox39.Location = New System.Drawing.Point(361, 44)
        Me.TextBox39.Name = "TextBox39"
        Me.TextBox39.ReadOnly = True
        Me.TextBox39.Size = New System.Drawing.Size(23, 20)
        Me.TextBox39.TabIndex = 56
        Me.ToolTip1.SetToolTip(Me.TextBox39, "Reason NPI was deactivated")
        '
        'Label16
        '
        Me.Label16.Location = New System.Drawing.Point(388, 48)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(65, 13)
        Me.Label16.TabIndex = 55
        Me.Label16.Text = "Reactivated"
        '
        'TextBox38
        '
        Me.TextBox38.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "NPI_REACTIVATION_DATE", True))
        Me.TextBox38.Location = New System.Drawing.Point(455, 44)
        Me.TextBox38.Name = "TextBox38"
        Me.TextBox38.ReadOnly = True
        Me.TextBox38.Size = New System.Drawing.Size(67, 20)
        Me.TextBox38.TabIndex = 54
        Me.ToolTip1.SetToolTip(Me.TextBox38, "Date NPI was reactivated")
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(176, 48)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(65, 13)
        Me.Label15.TabIndex = 53
        Me.Label15.Text = "Deactivated"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(286, 23)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(53, 13)
        Me.Label14.TabIndex = 52
        Me.Label14.Text = "Assigned "
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(408, 23)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(47, 13)
        Me.Label13.TabIndex = 51
        Me.Label13.Text = "Modified"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(1, 74)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(80, 13)
        Me.Label4.TabIndex = 50
        Me.Label4.Text = "Business Name"
        '
        'OrgLegalBusinessNameTextBox
        '
        Me.OrgLegalBusinessNameTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_ORG_NAME_LEGAL_BUS_NAME", True))
        Me.OrgLegalBusinessNameTextBox.Location = New System.Drawing.Point(87, 71)
        Me.OrgLegalBusinessNameTextBox.Name = "OrgLegalBusinessNameTextBox"
        Me.OrgLegalBusinessNameTextBox.ReadOnly = True
        Me.OrgLegalBusinessNameTextBox.Size = New System.Drawing.Size(435, 20)
        Me.OrgLegalBusinessNameTextBox.TabIndex = 49
        Me.ToolTip1.SetToolTip(Me.OrgLegalBusinessNameTextBox, "Provider Organization Name (Legal Business Name)")
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label12)
        Me.GroupBox3.Controls.Add(Me.Label11)
        Me.GroupBox3.Controls.Add(Me.TextBox37)
        Me.GroupBox3.Controls.Add(Me.TextBox33)
        Me.GroupBox3.Controls.Add(Me.Label10)
        Me.GroupBox3.Controls.Add(Me.TextBox34)
        Me.GroupBox3.Controls.Add(Me.TextBox35)
        Me.GroupBox3.Controls.Add(Me.TextBox36)
        Me.GroupBox3.Location = New System.Drawing.Point(4, 242)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(515, 87)
        Me.GroupBox3.TabIndex = 48
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Authorized Official"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(47, 68)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(51, 13)
        Me.Label12.TabIndex = 55
        Me.Label12.Text = "Contact#"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(47, 45)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(27, 13)
        Me.Label11.TabIndex = 54
        Me.Label11.Text = "Title"
        '
        'TextBox37
        '
        Me.TextBox37.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "AUTH_OFF_PHONE_NUM", True))
        Me.TextBox37.Location = New System.Drawing.Point(109, 64)
        Me.TextBox37.Name = "TextBox37"
        Me.TextBox37.ReadOnly = True
        Me.TextBox37.Size = New System.Drawing.Size(163, 20)
        Me.TextBox37.TabIndex = 53
        Me.ToolTip1.SetToolTip(Me.TextBox37, "Authorized Official Telephone Number")
        '
        'TextBox33
        '
        Me.TextBox33.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "AUTH_OFF_TITLE_OR_POSITION", True))
        Me.TextBox33.Location = New System.Drawing.Point(109, 41)
        Me.TextBox33.Name = "TextBox33"
        Me.TextBox33.ReadOnly = True
        Me.TextBox33.Size = New System.Drawing.Size(383, 20)
        Me.TextBox33.TabIndex = 52
        Me.ToolTip1.SetToolTip(Me.TextBox33, "Authorized Official Title or Position")
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(47, 22)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(35, 13)
        Me.Label10.TabIndex = 51
        Me.Label10.Text = "Name"
        '
        'TextBox34
        '
        Me.TextBox34.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "AUTH_OFF_MIDDLE_NAME", True))
        Me.TextBox34.Location = New System.Drawing.Point(266, 19)
        Me.TextBox34.Name = "TextBox34"
        Me.TextBox34.ReadOnly = True
        Me.TextBox34.Size = New System.Drawing.Size(69, 20)
        Me.TextBox34.TabIndex = 50
        Me.ToolTip1.SetToolTip(Me.TextBox34, "Authorized Official Middle Name")
        '
        'TextBox35
        '
        Me.TextBox35.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "AUTH_OFF_LAST_NAME", True))
        Me.TextBox35.Location = New System.Drawing.Point(337, 19)
        Me.TextBox35.Name = "TextBox35"
        Me.TextBox35.ReadOnly = True
        Me.TextBox35.Size = New System.Drawing.Size(155, 20)
        Me.TextBox35.TabIndex = 49
        Me.ToolTip1.SetToolTip(Me.TextBox35, "Authorized Official Last Name")
        '
        'TextBox36
        '
        Me.TextBox36.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "AUTH_OFF_FIRST_NAME", True))
        Me.TextBox36.Location = New System.Drawing.Point(109, 19)
        Me.TextBox36.Name = "TextBox36"
        Me.TextBox36.ReadOnly = True
        Me.TextBox36.Size = New System.Drawing.Size(155, 20)
        Me.TextBox36.TabIndex = 48
        Me.ToolTip1.SetToolTip(Me.TextBox36, "Authorized Official First Name")
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(192, 119)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(42, 13)
        Me.Label9.TabIndex = 42
        Me.Label9.Text = "Gender"
        '
        'TextBox32
        '
        Me.TextBox32.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_GENDER_CD", True))
        Me.TextBox32.Location = New System.Drawing.Point(242, 115)
        Me.TextBox32.Name = "TextBox32"
        Me.TextBox32.ReadOnly = True
        Me.TextBox32.Size = New System.Drawing.Size(23, 20)
        Me.TextBox32.TabIndex = 41
        Me.ToolTip1.SetToolTip(Me.TextBox32, "Provider Gender")
        '
        'TextBox31
        '
        Me.TextBox31.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "NPI_DEACT_DATE", True))
        Me.TextBox31.Location = New System.Drawing.Point(245, 44)
        Me.TextBox31.Name = "TextBox31"
        Me.TextBox31.ReadOnly = True
        Me.TextBox31.Size = New System.Drawing.Size(67, 20)
        Me.TextBox31.TabIndex = 40
        Me.ToolTip1.SetToolTip(Me.TextBox31, "Date NPI was deactivated")
        '
        'TextBox30
        '
        Me.TextBox30.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "CMS_LAST_UPDATE_DATE", True))
        Me.TextBox30.Location = New System.Drawing.Point(455, 19)
        Me.TextBox30.Name = "TextBox30"
        Me.TextBox30.ReadOnly = True
        Me.TextBox30.Size = New System.Drawing.Size(67, 20)
        Me.TextBox30.TabIndex = 39
        Me.ToolTip1.SetToolTip(Me.TextBox30, "Date data submitted was last modified")
        '
        'TextBox29
        '
        Me.TextBox29.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_ENUMERATION_DATE", True))
        Me.TextBox29.Location = New System.Drawing.Point(340, 19)
        Me.TextBox29.Name = "TextBox29"
        Me.TextBox29.ReadOnly = True
        Me.TextBox29.Size = New System.Drawing.Size(67, 20)
        Me.TextBox29.TabIndex = 38
        Me.ToolTip1.SetToolTip(Me.TextBox29, "Date NPI was assigned")
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label26)
        Me.GroupBox2.Controls.Add(Me.Label25)
        Me.GroupBox2.Controls.Add(Me.TextBox21)
        Me.GroupBox2.Controls.Add(Me.TextBox22)
        Me.GroupBox2.Controls.Add(Me.TextBox23)
        Me.GroupBox2.Controls.Add(Me.TextBox24)
        Me.GroupBox2.Controls.Add(Me.TextBox25)
        Me.GroupBox2.Controls.Add(Me.TextBox26)
        Me.GroupBox2.Controls.Add(Me.TextBox27)
        Me.GroupBox2.Controls.Add(Me.TextBox28)
        Me.GroupBox2.Location = New System.Drawing.Point(3, 443)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(515, 110)
        Me.GroupBox2.TabIndex = 37
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Business Practice Address"
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(285, 88)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(24, 13)
        Me.Label26.TabIndex = 55
        Me.Label26.Text = "Fax"
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(17, 88)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(38, 13)
        Me.Label25.TabIndex = 54
        Me.Label25.Text = "Phone"
        '
        'TextBox21
        '
        Me.TextBox21.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_BUS_PRAC_ADD_FAX_NUM", True))
        Me.TextBox21.Location = New System.Drawing.Point(326, 85)
        Me.TextBox21.Name = "TextBox21"
        Me.TextBox21.ReadOnly = True
        Me.TextBox21.Size = New System.Drawing.Size(163, 20)
        Me.TextBox21.TabIndex = 42
        Me.ToolTip1.SetToolTip(Me.TextBox21, "Provider Business Location Address Fax Number")
        '
        'TextBox22
        '
        Me.TextBox22.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_BUS_PRAC_ADD_PHONE_NUM", True))
        Me.TextBox22.Location = New System.Drawing.Point(60, 85)
        Me.TextBox22.Name = "TextBox22"
        Me.TextBox22.ReadOnly = True
        Me.TextBox22.Size = New System.Drawing.Size(163, 20)
        Me.TextBox22.TabIndex = 41
        Me.ToolTip1.SetToolTip(Me.TextBox22, "Provider Business Location Address Telephone Number")
        '
        'TextBox23
        '
        Me.TextBox23.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_BUS_PRAC_ADD_CTRY_NON_US", True))
        Me.TextBox23.Location = New System.Drawing.Point(364, 63)
        Me.TextBox23.Name = "TextBox23"
        Me.TextBox23.ReadOnly = True
        Me.TextBox23.Size = New System.Drawing.Size(125, 20)
        Me.TextBox23.TabIndex = 40
        Me.ToolTip1.SetToolTip(Me.TextBox23, "Provider Business Location Address Country Code (If outside U.S.)")
        '
        'TextBox24
        '
        Me.TextBox24.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_BUS_PRAC_ADD_POSTAL_CD", True))
        Me.TextBox24.Location = New System.Drawing.Point(258, 63)
        Me.TextBox24.Name = "TextBox24"
        Me.TextBox24.ReadOnly = True
        Me.TextBox24.Size = New System.Drawing.Size(100, 20)
        Me.TextBox24.TabIndex = 39
        Me.ToolTip1.SetToolTip(Me.TextBox24, "Provider Business Location Address Postal Code")
        '
        'TextBox25
        '
        Me.TextBox25.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_BUS_PRAC_ADD_STATE_NAME", True))
        Me.TextBox25.Location = New System.Drawing.Point(229, 63)
        Me.TextBox25.Name = "TextBox25"
        Me.TextBox25.ReadOnly = True
        Me.TextBox25.Size = New System.Drawing.Size(23, 20)
        Me.TextBox25.TabIndex = 38
        Me.ToolTip1.SetToolTip(Me.TextBox25, "Provider Business Location Address State Name")
        '
        'TextBox26
        '
        Me.TextBox26.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_BUS_PRAC_ADD_CITY_NAME", True))
        Me.TextBox26.Location = New System.Drawing.Point(60, 63)
        Me.TextBox26.Name = "TextBox26"
        Me.TextBox26.ReadOnly = True
        Me.TextBox26.Size = New System.Drawing.Size(163, 20)
        Me.TextBox26.TabIndex = 37
        Me.ToolTip1.SetToolTip(Me.TextBox26, "Provider Business Location Address City Name")
        '
        'TextBox27
        '
        Me.TextBox27.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_SEC_LN_BUS_PRAC_ADD", True))
        Me.TextBox27.Location = New System.Drawing.Point(60, 41)
        Me.TextBox27.Name = "TextBox27"
        Me.TextBox27.ReadOnly = True
        Me.TextBox27.Size = New System.Drawing.Size(429, 20)
        Me.TextBox27.TabIndex = 36
        Me.ToolTip1.SetToolTip(Me.TextBox27, "Provider Second Line Business Location Address")
        '
        'TextBox28
        '
        Me.TextBox28.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_FST_LN_BUS_PRAC_ADD", True))
        Me.TextBox28.Location = New System.Drawing.Point(60, 19)
        Me.TextBox28.Name = "TextBox28"
        Me.TextBox28.ReadOnly = True
        Me.TextBox28.Size = New System.Drawing.Size(429, 20)
        Me.TextBox28.TabIndex = 35
        Me.ToolTip1.SetToolTip(Me.TextBox28, "Provider First Line Business Location Address")
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label24)
        Me.GroupBox1.Controls.Add(Me.Label20)
        Me.GroupBox1.Controls.Add(Me.TextBox20)
        Me.GroupBox1.Controls.Add(Me.TextBox19)
        Me.GroupBox1.Controls.Add(Me.TextBox18)
        Me.GroupBox1.Controls.Add(Me.TextBox17)
        Me.GroupBox1.Controls.Add(Me.TextBox16)
        Me.GroupBox1.Controls.Add(Me.TextBox15)
        Me.GroupBox1.Controls.Add(Me.TextBox14)
        Me.GroupBox1.Controls.Add(Me.TextBox13)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 331)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(515, 110)
        Me.GroupBox1.TabIndex = 36
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Business Mailing Address"
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(17, 89)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(38, 13)
        Me.Label24.TabIndex = 53
        Me.Label24.Text = "Phone"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(285, 89)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(24, 13)
        Me.Label20.TabIndex = 52
        Me.Label20.Text = "Fax"
        '
        'TextBox20
        '
        Me.TextBox20.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_BUS_MAIL_ADD_FAX_NUM", True))
        Me.TextBox20.Location = New System.Drawing.Point(326, 86)
        Me.TextBox20.Name = "TextBox20"
        Me.TextBox20.ReadOnly = True
        Me.TextBox20.Size = New System.Drawing.Size(163, 20)
        Me.TextBox20.TabIndex = 42
        Me.ToolTip1.SetToolTip(Me.TextBox20, "Provider Business Mailing Address Fax Number")
        '
        'TextBox19
        '
        Me.TextBox19.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_BUS_MAIL_ADD_PHONE_NUM", True))
        Me.TextBox19.Location = New System.Drawing.Point(60, 86)
        Me.TextBox19.Name = "TextBox19"
        Me.TextBox19.ReadOnly = True
        Me.TextBox19.Size = New System.Drawing.Size(163, 20)
        Me.TextBox19.TabIndex = 41
        Me.ToolTip1.SetToolTip(Me.TextBox19, "Provider Business Mailing Address Telephone Number")
        '
        'TextBox18
        '
        Me.TextBox18.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_BUS_MAIL_ADD_CTRY_NON_US", True))
        Me.TextBox18.Location = New System.Drawing.Point(364, 64)
        Me.TextBox18.Name = "TextBox18"
        Me.TextBox18.ReadOnly = True
        Me.TextBox18.Size = New System.Drawing.Size(125, 20)
        Me.TextBox18.TabIndex = 40
        Me.ToolTip1.SetToolTip(Me.TextBox18, "Provider Business Mailing Address Country Code (If outside U.S.)")
        '
        'TextBox17
        '
        Me.TextBox17.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_BUS_MAIL_ADD_POSTAL_CD", True))
        Me.TextBox17.Location = New System.Drawing.Point(258, 64)
        Me.TextBox17.Name = "TextBox17"
        Me.TextBox17.ReadOnly = True
        Me.TextBox17.Size = New System.Drawing.Size(100, 20)
        Me.TextBox17.TabIndex = 39
        Me.ToolTip1.SetToolTip(Me.TextBox17, "Provider Business Mailing Address Postal Code")
        '
        'TextBox16
        '
        Me.TextBox16.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_BUS_MAIL_ADD_STATE_NAME", True))
        Me.TextBox16.Location = New System.Drawing.Point(229, 64)
        Me.TextBox16.Name = "TextBox16"
        Me.TextBox16.ReadOnly = True
        Me.TextBox16.Size = New System.Drawing.Size(23, 20)
        Me.TextBox16.TabIndex = 38
        Me.ToolTip1.SetToolTip(Me.TextBox16, "Provider Business Mailing Address State Name")
        '
        'TextBox15
        '
        Me.TextBox15.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_BUS_MAIL_ADD_CITY_NAME", True))
        Me.TextBox15.Location = New System.Drawing.Point(60, 64)
        Me.TextBox15.Name = "TextBox15"
        Me.TextBox15.ReadOnly = True
        Me.TextBox15.Size = New System.Drawing.Size(163, 20)
        Me.TextBox15.TabIndex = 37
        Me.ToolTip1.SetToolTip(Me.TextBox15, "Provider Business Mailing Address City Name")
        '
        'TextBox14
        '
        Me.TextBox14.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_SEC_LN_BUS_MAIL_ADD", True))
        Me.TextBox14.Location = New System.Drawing.Point(60, 41)
        Me.TextBox14.Name = "TextBox14"
        Me.TextBox14.ReadOnly = True
        Me.TextBox14.Size = New System.Drawing.Size(429, 20)
        Me.TextBox14.TabIndex = 36
        Me.ToolTip1.SetToolTip(Me.TextBox14, "Provider Second Line Business Mailing Address")
        '
        'TextBox13
        '
        Me.TextBox13.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_FST_LN_BUS_MAIL_ADD", True))
        Me.TextBox13.Location = New System.Drawing.Point(60, 19)
        Me.TextBox13.Name = "TextBox13"
        Me.TextBox13.ReadOnly = True
        Me.TextBox13.Size = New System.Drawing.Size(429, 20)
        Me.TextBox13.TabIndex = 35
        Me.ToolTip1.SetToolTip(Me.TextBox13, "Provider First Line Business Mailing Address")
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(1, 163)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(83, 13)
        Me.Label8.TabIndex = 28
        Me.Label8.Text = "Other Credential"
        '
        'TextBox12
        '
        Me.TextBox12.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_OTH_CREDENTIAL_TXT", True))
        Me.TextBox12.Location = New System.Drawing.Point(87, 159)
        Me.TextBox12.Name = "TextBox12"
        Me.TextBox12.ReadOnly = True
        Me.TextBox12.Size = New System.Drawing.Size(104, 20)
        Me.TextBox12.TabIndex = 27
        Me.ToolTip1.SetToolTip(Me.TextBox12, "Provider Other Name Credential")
        '
        'TextBox7
        '
        Me.TextBox7.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_OTH_NAME_SUF_TXT", True))
        Me.TextBox7.Location = New System.Drawing.Point(495, 137)
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.ReadOnly = True
        Me.TextBox7.Size = New System.Drawing.Size(27, 20)
        Me.TextBox7.TabIndex = 26
        Me.ToolTip1.SetToolTip(Me.TextBox7, "Provider Other Last Prefix")
        '
        'TextBox8
        '
        Me.TextBox8.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_OTH_NAME_PRE_TXT", True))
        Me.TextBox8.Location = New System.Drawing.Point(87, 137)
        Me.TextBox8.Name = "TextBox8"
        Me.TextBox8.ReadOnly = True
        Me.TextBox8.Size = New System.Drawing.Size(23, 20)
        Me.TextBox8.TabIndex = 25
        Me.ToolTip1.SetToolTip(Me.TextBox8, "Provider Other Last Suffix")
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(1, 141)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(64, 13)
        Me.Label7.TabIndex = 24
        Me.Label7.Text = "Other Name"
        '
        'TextBox9
        '
        Me.TextBox9.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_OTH_MIDDLE_NAME", True))
        Me.TextBox9.Location = New System.Drawing.Point(267, 137)
        Me.TextBox9.Name = "TextBox9"
        Me.TextBox9.ReadOnly = True
        Me.TextBox9.Size = New System.Drawing.Size(69, 20)
        Me.TextBox9.TabIndex = 23
        Me.ToolTip1.SetToolTip(Me.TextBox9, "Provider Other Middle Initial")
        '
        'TextBox10
        '
        Me.TextBox10.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_OTH_LAST_NAME", True))
        Me.TextBox10.Location = New System.Drawing.Point(338, 137)
        Me.TextBox10.Name = "TextBox10"
        Me.TextBox10.ReadOnly = True
        Me.TextBox10.Size = New System.Drawing.Size(155, 20)
        Me.TextBox10.TabIndex = 22
        Me.ToolTip1.SetToolTip(Me.TextBox10, "Provider Other Last Name")
        '
        'TextBox11
        '
        Me.TextBox11.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_OTH_FIRST_NAME", True))
        Me.TextBox11.Location = New System.Drawing.Point(111, 137)
        Me.TextBox11.Name = "TextBox11"
        Me.TextBox11.ReadOnly = True
        Me.TextBox11.Size = New System.Drawing.Size(154, 20)
        Me.TextBox11.TabIndex = 21
        Me.ToolTip1.SetToolTip(Me.TextBox11, "Provider Other First Name")
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(1, 119)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(54, 13)
        Me.Label6.TabIndex = 18
        Me.Label6.Text = "Credential"
        '
        'TextBox6
        '
        Me.TextBox6.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_CREDENTIAL_TXT", True))
        Me.TextBox6.Location = New System.Drawing.Point(87, 115)
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.ReadOnly = True
        Me.TextBox6.Size = New System.Drawing.Size(104, 20)
        Me.TextBox6.TabIndex = 17
        Me.ToolTip1.SetToolTip(Me.TextBox6, "Provider Credential")
        '
        'TextBox5
        '
        Me.TextBox5.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_NAME_SUF_TXT", True))
        Me.TextBox5.Location = New System.Drawing.Point(495, 93)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.ReadOnly = True
        Me.TextBox5.Size = New System.Drawing.Size(27, 20)
        Me.TextBox5.TabIndex = 16
        Me.ToolTip1.SetToolTip(Me.TextBox5, "Provider Name Suffix")
        '
        'TextBox4
        '
        Me.TextBox4.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_NAME_PRE_TXT", True))
        Me.TextBox4.Location = New System.Drawing.Point(87, 93)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.ReadOnly = True
        Me.TextBox4.Size = New System.Drawing.Size(23, 20)
        Me.TextBox4.TabIndex = 15
        Me.ToolTip1.SetToolTip(Me.TextBox4, "Provider Name Prefix")
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(1, 97)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(77, 13)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "Provider Name"
        '
        'TextBox3
        '
        Me.TextBox3.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_MIDDLE_NAME", True))
        Me.TextBox3.Location = New System.Drawing.Point(267, 93)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ReadOnly = True
        Me.TextBox3.Size = New System.Drawing.Size(69, 20)
        Me.TextBox3.TabIndex = 13
        Me.ToolTip1.SetToolTip(Me.TextBox3, "Provider Middle Name")
        '
        'TextBox2
        '
        Me.TextBox2.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_LAST_NAME_LEGAL_NAME", True))
        Me.TextBox2.Location = New System.Drawing.Point(338, 93)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(155, 20)
        Me.TextBox2.TabIndex = 12
        Me.ToolTip1.SetToolTip(Me.TextBox2, "Provider Last Name (Legal Name)")
        '
        'TextBox1
        '
        Me.TextBox1.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "PROV_FIRST_NAME", True))
        Me.TextBox1.Location = New System.Drawing.Point(111, 93)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(154, 20)
        Me.TextBox1.TabIndex = 10
        Me.ToolTip1.SetToolTip(Me.TextBox1, "Provider First Name")
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(1, 48)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(91, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Replacement NPI"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(200, 23)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Entity Type"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(1, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(25, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "NPI"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(1, 557)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(DataGridNPILicenses)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.DataGridNPIProviderOther)
        Me.SplitContainer1.Size = New System.Drawing.Size(523, 190)
        Me.SplitContainer1.SplitterDistance = 88
        Me.SplitContainer1.SplitterWidth = 8
        Me.SplitContainer1.TabIndex = 5
        '
        'DataGridNPIProviderOther







































































        '
        Me.DataGridNPIProviderOther.ADGroupsThatCanCopy = "CMSUsers"
        Me.DataGridNPIProviderOther.ADGroupsThatCanCustomize = "CMSUsers"
        Me.DataGridNPIProviderOther.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DataGridNPIProviderOther.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DataGridNPIProviderOther.ADGroupsThatCanFind = ""
        Me.DataGridNPIProviderOther.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DataGridNPIProviderOther.ADGroupsThatCanMultiSort = ""
        Me.DataGridNPIProviderOther.AllowAutoSize = True
        Me.DataGridNPIProviderOther.AllowColumnReorder = True
        Me.DataGridNPIProviderOther.AllowCopy = True
        Me.DataGridNPIProviderOther.AllowCustomize = True
        Me.DataGridNPIProviderOther.AllowDelete = False
        Me.DataGridNPIProviderOther.AllowDragDrop = False
        Me.DataGridNPIProviderOther.AllowEdit = False
        Me.DataGridNPIProviderOther.AllowExport = True
        Me.DataGridNPIProviderOther.AllowFilter = False
        Me.DataGridNPIProviderOther.AllowFind = True
        Me.DataGridNPIProviderOther.AllowGoTo = True
        Me.DataGridNPIProviderOther.AllowMultiSelect = False
        Me.DataGridNPIProviderOther.AllowMultiSort = False
        Me.DataGridNPIProviderOther.AllowNew = False
        Me.DataGridNPIProviderOther.AllowPrint = False
        Me.DataGridNPIProviderOther.AllowRefresh = False
        Me.DataGridNPIProviderOther.AppKey = "UFCW\Claims\"
        Me.DataGridNPIProviderOther.BackgroundColor = System.Drawing.SystemColors.Window
        Me.DataGridNPIProviderOther.CaptionText = "Associated Providers"
        Me.DataGridNPIProviderOther.ConfirmDelete = True
        Me.DataGridNPIProviderOther.CopySelectedOnly = True
        Me.DataGridNPIProviderOther.DataMember = ""
        Me.DataGridNPIProviderOther.DataSource = Me.NPIREGISTRYOTHERPROVIDERSBindingSource
        Me.DataGridNPIProviderOther.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridNPIProviderOther.ExportSelectedOnly = True
        Me.DataGridNPIProviderOther.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DataGridNPIProviderOther.LastGoToLine = ""
        Me.DataGridNPIProviderOther.Location = New System.Drawing.Point(0, 0)
        Me.DataGridNPIProviderOther.MultiSort = False
        Me.DataGridNPIProviderOther.Name = "DataGridNPIProviderOther"
        Me.DataGridNPIProviderOther.ReadOnly = True
        Me.DataGridNPIProviderOther.RowHeadersVisible = False
        Me.DataGridNPIProviderOther.SetRowOnRightClick = True
        Me.DataGridNPIProviderOther.SingleClickBooleanColumns = True
        Me.DataGridNPIProviderOther.Size = New System.Drawing.Size(523, 94)
        Me.DataGridNPIProviderOther.SuppressTriangle = False
        Me.DataGridNPIProviderOther.TabIndex = 0
        Me.DataGridNPIProviderOther.TableStyles.AddRange(New System.Windows.Forms.DataGridTableStyle() {Me.NPIREGISTRYOTHERPROVIDERS})
        '
        'NPIREGISTRYOTHERPROVIDERSBindingSource
        '
        Me.NPIREGISTRYOTHERPROVIDERSBindingSource.DataMember = "NPI_REGISTRY_OTHER_PROVIDERS"
        Me.NPIREGISTRYOTHERPROVIDERSBindingSource.DataSource = Me.ProvDS




        '
        'NPIREGISTRYOTHERPROVIDERS
        '
        Me.NPIREGISTRYOTHERPROVIDERS.DataGrid = Me.DataGridNPIProviderOther
        Me.NPIREGISTRYOTHERPROVIDERS.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.OTH_PROV_IDENT, Me.OTH_PROV_IDENT_TYPE_CD, Me.OTH_PROV_IDENT_STATE, Me.OTH_PROV_IDENT_ISSUER})
        Me.NPIREGISTRYOTHERPROVIDERS.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.NPIREGISTRYOTHERPROVIDERS.MappingName = "NPI_REGISTRY_OTHER_PROVIDERS"
        Me.NPIREGISTRYOTHERPROVIDERS.PreferredColumnWidth = 175
        Me.NPIREGISTRYOTHERPROVIDERS.ReadOnly = True
        Me.NPIREGISTRYOTHERPROVIDERS.RowHeadersVisible = False
        '
        'OTH_PROV_IDENT
        '
        Me.OTH_PROV_IDENT.Format = ""
        Me.OTH_PROV_IDENT.FormatInfo = Nothing
        Me.OTH_PROV_IDENT.HeaderText = "Other Provider"
        Me.OTH_PROV_IDENT.MappingName = "OTH_PROV_IDENT"
        Me.OTH_PROV_IDENT.NullText = ""
        Me.OTH_PROV_IDENT.ReadOnly = True
        Me.OTH_PROV_IDENT.Width = 90
        '
        'OTH_PROV_IDENT_TYPE_CD
        '
        Me.OTH_PROV_IDENT_TYPE_CD.Format = ""
        Me.OTH_PROV_IDENT_TYPE_CD.FormatInfo = Nothing
        Me.OTH_PROV_IDENT_TYPE_CD.HeaderText = "Provider Identity Type"
        Me.OTH_PROV_IDENT_TYPE_CD.MappingName = "OTH_PROV_IDENT_TYPE_CD"
        Me.OTH_PROV_IDENT_TYPE_CD.NullText = ""
        Me.OTH_PROV_IDENT_TYPE_CD.ReadOnly = True
        Me.OTH_PROV_IDENT_TYPE_CD.Width = 120
        '
        'OTH_PROV_IDENT_STATE
        '
        Me.OTH_PROV_IDENT_STATE.Format = ""
        Me.OTH_PROV_IDENT_STATE.FormatInfo = Nothing
        Me.OTH_PROV_IDENT_STATE.HeaderText = "State"
        Me.OTH_PROV_IDENT_STATE.MappingName = "OTH_PROV_IDENT_STATE"
        Me.OTH_PROV_IDENT_STATE.NullText = ""
        Me.OTH_PROV_IDENT_STATE.ReadOnly = True
        Me.OTH_PROV_IDENT_STATE.Width = 50
        '
        'OTH_PROV_IDENT_ISSUER
        '
        Me.OTH_PROV_IDENT_ISSUER.Format = ""
        Me.OTH_PROV_IDENT_ISSUER.FormatInfo = Nothing
        Me.OTH_PROV_IDENT_ISSUER.HeaderText = "Other Provider Identity Issuer"
        Me.OTH_PROV_IDENT_ISSUER.MappingName = "OTH_PROV_IDENT_ISSUER"
        Me.OTH_PROV_IDENT_ISSUER.NullText = ""
        Me.OTH_PROV_IDENT_ISSUER.ReadOnly = True
        Me.OTH_PROV_IDENT_ISSUER.Width = 175
        '
        'ReplacementNPITextBox










        '
        Me.ReplacementNPITextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "REPLACEMENT_NPI", True))
        Me.ReplacementNPITextBox.Location = New System.Drawing.Point(95, 44)
        Me.ReplacementNPITextBox.Name = "ReplacementNPITextBox"
        Me.ReplacementNPITextBox.ReadOnly = True
        Me.ReplacementNPITextBox.Size = New System.Drawing.Size(77, 20)
        Me.ReplacementNPITextBox.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.ReplacementNPITextBox, "NPI previously replaced")
        '
        'EntityTypeTextBox
        '
        Me.EntityTypeTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "ENTITY_TYPE_CD", True))
        Me.EntityTypeTextBox.Location = New System.Drawing.Point(261, 19)
        Me.EntityTypeTextBox.Name = "EntityTypeTextBox"
        Me.EntityTypeTextBox.ReadOnly = True
        Me.EntityTypeTextBox.Size = New System.Drawing.Size(24, 20)
        Me.EntityTypeTextBox.TabIndex = 2

        '
        'NPITextBox
        '
        Me.NPITextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.NPIREGISTRYBindingSource, "NPI", True))
        Me.NPITextBox.Location = New System.Drawing.Point(28, 19)
        Me.NPITextBox.Name = "NPITextBox"
        Me.NPITextBox.ReadOnly = True
        Me.NPITextBox.Size = New System.Drawing.Size(77, 20)
        Me.NPITextBox.TabIndex = 1
        '
        'NPIRegistryControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.NPIGroupBox)
        Me.Name = "NPIRegistryControl"
        Me.Size = New System.Drawing.Size(524, 751)
        CType(DataGridNPILicenses, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NPIREGISTRYLICENSESBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ProvDS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.NPIGroupBox.ResumeLayout(False)
        Me.NPIGroupBox.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        CType(Me.NPIREGISTRYBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)

        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.DataGridNPIProviderOther, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NPIREGISTRYOTHERPROVIDERSBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents NPIGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents DataGridNPIProviderOther As DataGridCustom
    Friend WithEvents ReplacementNPITextBox As System.Windows.Forms.TextBox
    Friend WithEvents EntityTypeTextBox As System.Windows.Forms.TextBox
    Friend WithEvents NPITextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents TextBox12 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox7 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox8 As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TextBox9 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox10 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox11 As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TextBox6 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox20 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox19 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox18 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox17 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox16 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox15 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox14 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox13 As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents OrgLegalBusinessNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox37 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox33 As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TextBox34 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox35 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox36 As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TextBox32 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox31 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox30 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox29 As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox21 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox22 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox23 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox24 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox25 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox26 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox27 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox28 As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents TextBox39 As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents TextBox38 As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents TextBox44 As System.Windows.Forms.TextBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents TextBox46 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox47 As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents TextBox41 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox40 As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents ProvDS As ProvDS
    Friend WithEvents NPIREGISTRYBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents NPIREGISTRYLICENSESBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents NPIREGISTRYOTHERPROVIDERSBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents NPI_REGISTRY_LICENSES As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents NPIREGISTRYOTHERPROVIDERS As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents OTH_PROV_IDENT As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents OTH_PROV_IDENT_TYPE_CD As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents OTH_PROV_IDENT_STATE As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents OTH_PROV_IDENT_ISSUER As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents HLTHCARE_PROV_TXNMY_CD As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents PROV_LIC_NUM As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents PROV_LIC_NUM_STATE_CD As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents HLTHCARE_PROV_PRIM_TXNMY_SW As System.Windows.Forms.DataGridTextBoxColumn

End Class

