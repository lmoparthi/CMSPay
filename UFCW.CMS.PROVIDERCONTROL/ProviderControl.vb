Imports System.ComponentModel

Public Class ProviderControl
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _ProviderID As Integer = -1
    Private _TaxID As Integer = -1
    Private _NPI As Decimal = -1
    Private _APPKEY As String = "UFCW\Claims\"
    Private _Loading As Boolean = True

    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents TaxIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TextBox6 As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox7 As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ProviderMailAddressBS As System.Windows.Forms.BindingSource
    Friend WithEvents ProvDS As ProvDS
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents ONLINEDATETextBox As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents CheckBox3 As System.Windows.Forms.CheckBox

    ReadOnly _DomainUser As String = SystemInformation.UserName

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'UserControl1 overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    Public Overloads Sub Dispose()
        If Not ProviderMailAddressDS Is Nothing Then ProviderMailAddressDS.Dispose()
        ProviderMailAddressDS = Nothing
        MyBase.Dispose()
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents ProviderAddressGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents ProviderCountryTextBox As System.Windows.Forms.TextBox
    Friend WithEvents SuspendedProviderAddressCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents ProviderZip4TextBox As System.Windows.Forms.TextBox
    Friend WithEvents ProviderAddress1TextBox As System.Windows.Forms.TextBox
    Friend WithEvents ProviderZipTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ProviderCityTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ProviderAddress2TextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label54 As System.Windows.Forms.Label
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents Label56 As System.Windows.Forms.Label
    Friend WithEvents Label58 As System.Windows.Forms.Label
    Friend WithEvents ProviderStateTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ProviderMailAddressDS As ProviderMailAddress
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ProviderAddressGroupBox = New System.Windows.Forms.GroupBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.ONLINEDATETextBox = New System.Windows.Forms.TextBox()
        Me.ProvDS = New ProvDS()
        Me.TextBox7 = New System.Windows.Forms.TextBox()
        Me.ProviderMailAddressDS = New ProviderMailAddress()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.TextBox6 = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ProviderStateTextBox = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.ProviderCountryTextBox = New System.Windows.Forms.TextBox()
        Me.SuspendedProviderAddressCheckBox = New System.Windows.Forms.CheckBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.ProviderZip4TextBox = New System.Windows.Forms.TextBox()
        Me.ProviderAddress1TextBox = New System.Windows.Forms.TextBox()
        Me.ProviderZipTextBox = New System.Windows.Forms.TextBox()
        Me.ProviderCityTextBox = New System.Windows.Forms.TextBox()
        Me.ProviderAddress2TextBox = New System.Windows.Forms.TextBox()
        Me.Label54 = New System.Windows.Forms.Label()
        Me.Label55 = New System.Windows.Forms.Label()
        Me.Label56 = New System.Windows.Forms.Label()
        Me.Label58 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.CheckBox3 = New System.Windows.Forms.CheckBox()
        Me.CheckBox2 = New System.Windows.Forms.CheckBox()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.TaxIDTextBox = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.ProviderMailAddressBS = New System.Windows.Forms.BindingSource(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ProviderAddressGroupBox.SuspendLayout()
        CType(Me.ProvDS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ProviderMailAddressDS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.ProviderMailAddressBS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ProviderAddressGroupBox
        '
        Me.ProviderAddressGroupBox.Controls.Add(Me.Label9)
        Me.ProviderAddressGroupBox.Controls.Add(Me.ONLINEDATETextBox)
        Me.ProviderAddressGroupBox.Controls.Add(Me.TextBox7)
        Me.ProviderAddressGroupBox.Controls.Add(Me.Label8)
        Me.ProviderAddressGroupBox.Controls.Add(Me.TextBox6)
        Me.ProviderAddressGroupBox.Controls.Add(Me.Label6)
        Me.ProviderAddressGroupBox.Controls.Add(Me.TextBox5)
        Me.ProviderAddressGroupBox.Controls.Add(Me.Label5)
        Me.ProviderAddressGroupBox.Controls.Add(Me.ProviderStateTextBox)
        Me.ProviderAddressGroupBox.Controls.Add(Me.Label16)
        Me.ProviderAddressGroupBox.Controls.Add(Me.ProviderCountryTextBox)
        Me.ProviderAddressGroupBox.Controls.Add(Me.SuspendedProviderAddressCheckBox)
        Me.ProviderAddressGroupBox.Controls.Add(Me.Label7)
        Me.ProviderAddressGroupBox.Controls.Add(Me.ProviderZip4TextBox)
        Me.ProviderAddressGroupBox.Controls.Add(Me.ProviderAddress1TextBox)
        Me.ProviderAddressGroupBox.Controls.Add(Me.ProviderZipTextBox)
        Me.ProviderAddressGroupBox.Controls.Add(Me.ProviderCityTextBox)
        Me.ProviderAddressGroupBox.Controls.Add(Me.ProviderAddress2TextBox)
        Me.ProviderAddressGroupBox.Controls.Add(Me.Label54)
        Me.ProviderAddressGroupBox.Controls.Add(Me.Label55)
        Me.ProviderAddressGroupBox.Controls.Add(Me.Label56)
        Me.ProviderAddressGroupBox.Controls.Add(Me.Label58)
        Me.ProviderAddressGroupBox.Location = New System.Drawing.Point(3, 55)
        Me.ProviderAddressGroupBox.Name = "ProviderAddressGroupBox"
        Me.ProviderAddressGroupBox.Size = New System.Drawing.Size(426, 145)
        Me.ProviderAddressGroupBox.TabIndex = 1
        Me.ProviderAddressGroupBox.TabStop = False
        Me.ProviderAddressGroupBox.Text = "Mail Address"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(257, 124)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(48, 13)
        Me.Label9.TabIndex = 24
        Me.Label9.Text = "Updated"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ONLINEDATETextBox
        '
        Me.ONLINEDATETextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProvDS, "PROVIDER.ONLINE_DATE", True))
        Me.ONLINEDATETextBox.Location = New System.Drawing.Point(306, 121)
        Me.ONLINEDATETextBox.Name = "ONLINEDATETextBox"
        Me.ONLINEDATETextBox.ReadOnly = True
        Me.ONLINEDATETextBox.Size = New System.Drawing.Size(114, 20)
        Me.ONLINEDATETextBox.TabIndex = 23
        '
        'ProvDS
        '
        Me.ProvDS.DataSetName = "ProvDS"
        Me.ProvDS.EnforceConstraints = False
        Me.ProvDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'TextBox7
        '
        Me.TextBox7.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderMailAddressDS, "PROVIDER_ADDRESS.CONTACT1", True))
        Me.TextBox7.Location = New System.Drawing.Point(60, 121)
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.ReadOnly = True
        Me.TextBox7.Size = New System.Drawing.Size(184, 20)
        Me.TextBox7.TabIndex = 22
        '
        'ProviderMailAddressDS
        '
        Me.ProviderMailAddressDS.DataSetName = "ProviderMailAddressDS"
        Me.ProviderMailAddressDS.EnforceConstraints = False
        Me.ProviderMailAddressDS.Locale = New System.Globalization.CultureInfo("en-US")
        Me.ProviderMailAddressDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(9, 124)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(44, 13)
        Me.Label8.TabIndex = 21
        Me.Label8.Text = "Contact"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextBox6
        '
        Me.TextBox6.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderMailAddressDS, "PROVIDER_ADDRESS.EMAIL1", True))
        Me.TextBox6.Location = New System.Drawing.Point(209, 100)
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.ReadOnly = True
        Me.TextBox6.Size = New System.Drawing.Size(211, 20)
        Me.TextBox6.TabIndex = 20
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(161, 103)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(39, 13)
        Me.Label6.TabIndex = 19
        Me.Label6.Text = "EMAIL"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextBox5
        '
        Me.TextBox5.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderMailAddressDS, "PROVIDER_ADDRESS.PHONENUMBER", True))
        Me.TextBox5.Location = New System.Drawing.Point(60, 100)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.ReadOnly = True
        Me.TextBox5.Size = New System.Drawing.Size(95, 20)
        Me.TextBox5.TabIndex = 18
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(9, 103)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(38, 13)
        Me.Label5.TabIndex = 17
        Me.Label5.Text = "Phone"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProviderStateTextBox
        '
        Me.ProviderStateTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderMailAddressDS, "PROVIDER_ADDRESS.STATE", True))
        Me.ProviderStateTextBox.Location = New System.Drawing.Point(60, 79)
        Me.ProviderStateTextBox.MaxLength = 4
        Me.ProviderStateTextBox.Name = "ProviderStateTextBox"
        Me.ProviderStateTextBox.ReadOnly = True
        Me.ProviderStateTextBox.Size = New System.Drawing.Size(32, 20)
        Me.ProviderStateTextBox.TabIndex = 16
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(257, 83)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(43, 13)
        Me.Label16.TabIndex = 15
        Me.Label16.Text = "Country"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProviderCountryTextBox
        '
        Me.ProviderCountryTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderMailAddressDS, "PROVIDER_ADDRESS.COUNTRY", True))
        Me.ProviderCountryTextBox.Location = New System.Drawing.Point(306, 79)
        Me.ProviderCountryTextBox.Name = "ProviderCountryTextBox"
        Me.ProviderCountryTextBox.ReadOnly = True
        Me.ProviderCountryTextBox.Size = New System.Drawing.Size(114, 20)
        Me.ProviderCountryTextBox.TabIndex = 13
        '
        'SuspendedProviderAddressCheckBox
        '
        Me.SuspendedProviderAddressCheckBox.AutoCheck = False
        Me.SuspendedProviderAddressCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.SuspendedProviderAddressCheckBox.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.ProviderMailAddressDS, "PROVIDER_ADDRESS.AddressSuspended", True))
        Me.SuspendedProviderAddressCheckBox.Enabled = False
        Me.SuspendedProviderAddressCheckBox.Location = New System.Drawing.Point(330, 25)
        Me.SuspendedProviderAddressCheckBox.Name = "SuspendedProviderAddressCheckBox"
        Me.SuspendedProviderAddressCheckBox.Size = New System.Drawing.Size(85, 42)
        Me.SuspendedProviderAddressCheckBox.TabIndex = 12
        Me.SuspendedProviderAddressCheckBox.Text = "Address Suspended"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(193, 83)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(10, 13)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "-"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProviderZip4TextBox
        '
        Me.ProviderZip4TextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderMailAddressDS, "PROVIDER_ADDRESS.ZIP_4", True))
        Me.ProviderZip4TextBox.Location = New System.Drawing.Point(209, 79)
        Me.ProviderZip4TextBox.MaxLength = 4
        Me.ProviderZip4TextBox.Name = "ProviderZip4TextBox"
        Me.ProviderZip4TextBox.ReadOnly = True
        Me.ProviderZip4TextBox.Size = New System.Drawing.Size(42, 20)
        Me.ProviderZip4TextBox.TabIndex = 5
        '
        'ProviderAddress1TextBox
        '
        Me.ProviderAddress1TextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ProviderAddress1TextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderMailAddressDS, "PROVIDER_ADDRESS.ADDRESS_LINE1", True))
        Me.ProviderAddress1TextBox.Location = New System.Drawing.Point(60, 16)
        Me.ProviderAddress1TextBox.MaxLength = 50
        Me.ProviderAddress1TextBox.Name = "ProviderAddress1TextBox"
        Me.ProviderAddress1TextBox.ReadOnly = True
        Me.ProviderAddress1TextBox.Size = New System.Drawing.Size(264, 20)
        Me.ProviderAddress1TextBox.TabIndex = 0
        '
        'ProviderZipTextBox
        '
        Me.ProviderZipTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderMailAddressDS, "PROVIDER_ADDRESS.ZIP", True))
        Me.ProviderZipTextBox.Location = New System.Drawing.Point(126, 79)
        Me.ProviderZipTextBox.MaxLength = 5
        Me.ProviderZipTextBox.Name = "ProviderZipTextBox"
        Me.ProviderZipTextBox.ReadOnly = True
        Me.ProviderZipTextBox.Size = New System.Drawing.Size(61, 20)
        Me.ProviderZipTextBox.TabIndex = 4
        '
        'ProviderCityTextBox
        '
        Me.ProviderCityTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ProviderCityTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderMailAddressDS, "PROVIDER_ADDRESS.CITY", True))
        Me.ProviderCityTextBox.Location = New System.Drawing.Point(60, 58)
        Me.ProviderCityTextBox.MaxLength = 50
        Me.ProviderCityTextBox.Name = "ProviderCityTextBox"
        Me.ProviderCityTextBox.ReadOnly = True
        Me.ProviderCityTextBox.Size = New System.Drawing.Size(264, 20)
        Me.ProviderCityTextBox.TabIndex = 2
        '
        'ProviderAddress2TextBox
        '
        Me.ProviderAddress2TextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ProviderAddress2TextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderMailAddressDS, "PROVIDER_ADDRESS.ADDRESS_LINE2", True))
        Me.ProviderAddress2TextBox.Location = New System.Drawing.Point(60, 37)
        Me.ProviderAddress2TextBox.MaxLength = 50
        Me.ProviderAddress2TextBox.Name = "ProviderAddress2TextBox"
        Me.ProviderAddress2TextBox.ReadOnly = True
        Me.ProviderAddress2TextBox.Size = New System.Drawing.Size(264, 20)
        Me.ProviderAddress2TextBox.TabIndex = 1
        '
        'Label54
        '
        Me.Label54.AutoSize = True
        Me.Label54.Location = New System.Drawing.Point(98, 82)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(22, 13)
        Me.Label54.TabIndex = 3
        Me.Label54.Text = "Zip"
        Me.Label54.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label55
        '
        Me.Label55.AutoSize = True
        Me.Label55.Location = New System.Drawing.Point(9, 83)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(32, 13)
        Me.Label55.TabIndex = 2
        Me.Label55.Text = "State"
        Me.Label55.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label56
        '
        Me.Label56.AutoSize = True
        Me.Label56.Location = New System.Drawing.Point(9, 19)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(45, 13)
        Me.Label56.TabIndex = 1
        Me.Label56.Text = "Address"
        Me.Label56.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label58
        '
        Me.Label58.AutoSize = True
        Me.Label58.Location = New System.Drawing.Point(9, 61)
        Me.Label58.Name = "Label58"
        Me.Label58.Size = New System.Drawing.Size(24, 13)
        Me.Label58.TabIndex = 1
        Me.Label58.Text = "City"
        Me.Label58.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.CheckBox3)
        Me.Panel1.Controls.Add(Me.CheckBox2)
        Me.Panel1.Controls.Add(Me.CheckBox1)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.TextBox4)
        Me.Panel1.Controls.Add(Me.TaxIDTextBox)
        Me.Panel1.Controls.Add(Me.TextBox3)
        Me.Panel1.Controls.Add(Me.TextBox1)
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(425, 47)
        Me.Panel1.TabIndex = 2
        '
        'CheckBox3
        '
        Me.CheckBox3.AutoCheck = False
        Me.CheckBox3.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox3.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.ProviderMailAddressDS, "PROVIDER_ADDRESS.SUSPEND_SW", True))
        Me.CheckBox3.Enabled = False
        Me.CheckBox3.Location = New System.Drawing.Point(287, 23)
        Me.CheckBox3.Name = "CheckBox3"
        Me.CheckBox3.Size = New System.Drawing.Size(66, 23)
        Me.CheckBox3.TabIndex = 15
        Me.CheckBox3.Text = "No HRA"
        Me.ToolTip1.SetToolTip(Me.CheckBox3, "Provider is InEligibile for HRA payments")
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoCheck = False
        Me.CheckBox2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox2.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.ProviderMailAddressDS, "PROVIDER_ADDRESS.PPOC_ELIGIBLE_SW", True))
        Me.CheckBox2.Enabled = False
        Me.CheckBox2.Location = New System.Drawing.Point(221, 23)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(55, 23)
        Me.CheckBox2.TabIndex = 14
        Me.CheckBox2.Text = "PPOC"
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoCheck = False
        Me.CheckBox1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox1.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.ProviderMailAddressDS, "PROVIDER_ADDRESS.SUSPEND_SW", True))
        Me.CheckBox1.Enabled = False
        Me.CheckBox1.Location = New System.Drawing.Point(364, 23)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(53, 23)
        Me.CheckBox1.TabIndex = 13
        Me.CheckBox1.Text = "Susp."
        Me.ToolTip1.SetToolTip(Me.CheckBox1, "Provider is Suspended")
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(114, 27)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(25, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "NPI"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(3, 27)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(42, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "TAX ID"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(250, 6)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Provider ID"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(35, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Name"
        '
        'TextBox4
        '
        Me.TextBox4.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderMailAddressDS, "PROVIDER_ADDRESS.NPI", True))
        Me.TextBox4.Location = New System.Drawing.Point(141, 24)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.ReadOnly = True
        Me.TextBox4.Size = New System.Drawing.Size(77, 20)
        Me.TextBox4.TabIndex = 4
        '
        'TaxIDTextBox
        '
        Me.TaxIDTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderMailAddressDS, "PROVIDER_ADDRESS.TAXID", True))
        Me.TaxIDTextBox.Location = New System.Drawing.Point(46, 24)
        Me.TaxIDTextBox.Name = "TaxIDTextBox"
        Me.TaxIDTextBox.ReadOnly = True
        Me.TaxIDTextBox.Size = New System.Drawing.Size(67, 20)
        Me.TaxIDTextBox.TabIndex = 3
        '
        'TextBox3
        '
        Me.TextBox3.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderMailAddressDS, "PROVIDER_ADDRESS.PROVIDER_ID", True))
        Me.TextBox3.Location = New System.Drawing.Point(316, 3)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ReadOnly = True
        Me.TextBox3.Size = New System.Drawing.Size(104, 20)
        Me.TextBox3.TabIndex = 2
        '
        'TextBox1
        '
        Me.TextBox1.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderMailAddressDS, "PROVIDER_ADDRESS.NAME1", True))
        Me.TextBox1.Location = New System.Drawing.Point(46, 3)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(202, 20)
        Me.TextBox1.TabIndex = 0
        '
        'ProviderMailAddressBS
        '
        Me.ProviderMailAddressBS.DataMember = "PROVIDER_ADDRESS"
        Me.ProviderMailAddressBS.DataSource = Me.ProvDS
        '
        'ProviderControl
        '
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ProviderAddressGroupBox)
        Me.Name = "ProviderControl"
        Me.Size = New System.Drawing.Size(434, 203)
        Me.ProviderAddressGroupBox.ResumeLayout(False)
        Me.ProviderAddressGroupBox.PerformLayout()
        CType(Me.ProvDS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ProviderMailAddressDS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.ProviderMailAddressBS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Properties"
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the ProviderID.")> _
    Public Property ProviderID() As Integer
        Get
            Return If(_ProviderID = -1, Nothing, _ProviderID)
        End Get
        Set(ByVal value As Integer)
            _ProviderID = value
        End Set
    End Property

    Public Property TaxID() As Integer
        Get
            Return If(_TaxID = -1, Nothing, _TaxID)
        End Get
        Set(ByVal value As Integer)
            _TaxID = value
        End Set
    End Property

    Public Property NPI() As Decimal
        Get
            Return If(_NPI = -1, Nothing, _NPI)
        End Get
        Set(ByVal value As Decimal)
            _NPI = value
        End Set
    End Property

#End Region

#Region "Constructor"
    Public Sub New(ByVal providerID As Integer)
        Me.New()

        _ProviderID = providerID
    End Sub

#End Region

#Region "Form\Button Events"
    Private Sub ProviderControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        _Loading = False
    End Sub

#End Region

#Region "Custom Subs\Functions"
    Public Sub LoadProvider(ByVal provider As String)

        Try
            Select Case provider.Trim.Length
                Case 0 To 6
                    _ProviderID = CInt(provider)
                    _TaxID = -1
                    _NPI = -1
                Case 7 To 9
                    _ProviderID = -1
                    _TaxID = CInt(provider)
                    _NPI = -1
                Case Else
                    _NPI = CDec(provider)
                    _TaxID = -1
                    _ProviderID = -1
            End Select

            LoadProvider()

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Sub ClearProvider()

        ProviderMailAddressDS.PROVIDER_ADDRESS.Rows.Clear()
        _ProviderID = -1
        _TaxID = -1
        _NPI = -1

    End Sub
    Public Sub LoadProvider()

        Try

            ProviderMailAddressDS.PROVIDER_ADDRESS.Rows.Clear()

            If _TaxID > 0 Then
                ProviderMailAddressDS = CType(ProviderDAL.GetProviderInformationByTaxID(_TaxID, ProviderMailAddressDS), ProviderMailAddress)
            ElseIf _ProviderID > 0 Then
                ProviderMailAddressDS = CType(ProviderDAL.GetProviderInformationByProviderID(_ProviderID, ProviderMailAddressDS), ProviderMailAddress)
            Else
                ProviderMailAddressDS = CType(ProviderDAL.GetProviderInformationByNPI(_NPI, ProviderMailAddressDS), ProviderMailAddress)
            End If

            If ProviderMailAddressDS.Tables(0).Rows.Count > 0 Then
                _ProviderID = CInt(ProviderMailAddressDS.Tables("PROVIDER_ADDRESS").Rows(0)("Provider_ID"))
                _TaxID = CInt(ProviderMailAddressDS.Tables("PROVIDER_ADDRESS").Rows(0)("TaxID"))

                If Not IsDBNull(ProviderMailAddressDS.Tables("PROVIDER_ADDRESS").Rows(0)("NPI")) Then
                    _NPI = CDec(ProviderMailAddressDS.Tables("PROVIDER_ADDRESS").Rows(0)("NPI"))
                End If

                TaxIDTextBox.DataBindings.Clear()
                Dim Bind As Binding = New Binding("Text", Me.ProviderMailAddressDS, "PROVIDER_ADDRESS.TAXID")
                Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged
                AddHandler Bind.Format, AddressOf TINBinding_Format
                AddHandler Bind.Parse, AddressOf TINBinding_Parse
                TaxIDTextBox.DataBindings.Add(Bind)

                ProviderZipTextBox.DataBindings.Clear()
                Bind = New Binding("Text", Me.ProviderMailAddressDS, "PROVIDER_ADDRESS.ZIP")
                Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged
                AddHandler Bind.Format, AddressOf ZIPBinding_Format
                ProviderZipTextBox.DataBindings.Add(Bind)

            Else
                _ProviderID = -1
                _TaxID = -1
                _NPI = -1
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Sub LoadProvider(ByVal dt As DataTable)

        Try

            ProviderMailAddressDS.PROVIDER_ADDRESS.Rows.Clear()

            If dt.Rows.Count > 0 Then ProviderMailAddressDS.Merge(dt)

        Catch ex As Exception
            Throw
        End Try

    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' adjusts TIN values entered for a databinding
    ' </summary>
    ' <param name="sender"></param>
    ' <param name="e"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[nick snyder]	8/16/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub TINBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            If IsDBNull(e.Value) = False AndAlso e.Value.ToString.Trim = "" Then
                e.Value = DBNull.Value
            ElseIf IsDBNull(e.Value) = False AndAlso IsNumeric(e.Value) = False Then
                e.Value = UnFormatTIN(CStr(e.Value))
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' formats TIN values entered for a databinding
    ' </summary>
    ' <param name="sender"></param>
    ' <param name="e"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[nick snyder]	8/16/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub ZIPBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            If IsDBNull(e.Value) = False AndAlso IsNumeric(e.Value) = True Then
                e.Value = Format(e.Value, "00000")
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' formats TIN values entered for a databinding
    ' </summary>
    ' <param name="sender"></param>
    ' <param name="e"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[nick snyder]	8/16/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub TINBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            If IsDBNull(e.Value) = False AndAlso IsNumeric(e.Value) = True Then
                e.Value = FormatTIN(CStr(e.Value))
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' formats an TIN
    ' </summary>
    ' <param name="tin"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[nick snyder]	8/16/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function FormatTIN(ByVal tin As String) As String

        tin = CStr(UnFormatTIN(tin))

        If IsNumeric(tin) Then
            tin = Format(CLng(tin), "00-0000000")
        End If

        Return tin
    End Function

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Unformats an TIN
    ' </summary>
    ' <param name="tin"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[nick snyder]	8/16/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function UnFormatTIN(ByVal strTIN As String) As Long

        If Replace(Replace(Replace(strTIN, " ", ""), "-", ""), "/", "") <> "" Then
            Return CLng(Format(CLng(Replace(Replace(Replace(strTIN, " ", ""), "-", ""), "/", "")), "0########"))
        Else
            Return Nothing
        End If

    End Function

    Private Sub TextBox1_MouseHover(sender As Object, e As System.EventArgs) Handles TextBox1.MouseHover
        ToolTip1.SetToolTip(TextBox1, TextBox1.Text)
    End Sub

#End Region

End Class