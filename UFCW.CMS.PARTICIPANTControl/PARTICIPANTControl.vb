Imports System.ComponentModel

Public Class PARTICIPANTControl
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _FamilyID As Integer = -1
    Private _RelationID As Integer = -1

    Private _APPKEY As String = "UFCW\Claims\"
    Private _Loading As Boolean = True

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
        If Not RegAddressDS Is Nothing Then RegAddressDS.Dispose()
        RegAddressDS = Nothing
        MyBase.Dispose()
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents PARTICIPANTCHECKAddressGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents PARTICIPANTCHECKCountryTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PARTICIPANTCHECKZip4TextBox As System.Windows.Forms.TextBox
    Friend WithEvents PARTICIPANTCHECKAddress1TextBox As System.Windows.Forms.TextBox
    Friend WithEvents PARTICIPANTCHECKCityTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PARTICIPANTCHECKAddress2TextBox As System.Windows.Forms.TextBox
    Friend WithEvents PARTICIPANTMAILAddressGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents PARTICIPANTMAILCountryTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents PARTICIPANTMAILZip4TextBox As System.Windows.Forms.TextBox
    Friend WithEvents PARTICIPANTMAILAddress1TextBox As System.Windows.Forms.TextBox
    Friend WithEvents PARTICIPANTMAILZipTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PARTICIPANTMAILCityTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PARTICIPANTMAILAddress2TextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents PARTICIPANTMAILStateListBox As System.Windows.Forms.TextBox
    Friend WithEvents PARTICIPANTCHECKStateListBox As System.Windows.Forms.TextBox
    Friend WithEvents PARTICIPANTCHECKZipTextBox As System.Windows.Forms.TextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents RegAddressDS As RegAddressDS
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.PARTICIPANTCHECKAddressGroupBox = New System.Windows.Forms.GroupBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.PARTICIPANTCHECKCountryTextBox = New System.Windows.Forms.TextBox
        Me.RegAddressDS = New RegAddressDS
        Me.Label2 = New System.Windows.Forms.Label
        Me.PARTICIPANTCHECKZip4TextBox = New System.Windows.Forms.TextBox
        Me.PARTICIPANTCHECKAddress1TextBox = New System.Windows.Forms.TextBox
        Me.PARTICIPANTCHECKCityTextBox = New System.Windows.Forms.TextBox
        Me.PARTICIPANTCHECKAddress2TextBox = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.PARTICIPANTCHECKStateListBox = New System.Windows.Forms.TextBox
        Me.PARTICIPANTCHECKZipTextBox = New System.Windows.Forms.TextBox
        Me.PARTICIPANTMAILAddressGroupBox = New System.Windows.Forms.GroupBox
        Me.PARTICIPANTMAILStateListBox = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.PARTICIPANTMAILCountryTextBox = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.PARTICIPANTMAILZip4TextBox = New System.Windows.Forms.TextBox
        Me.PARTICIPANTMAILAddress1TextBox = New System.Windows.Forms.TextBox
        Me.PARTICIPANTMAILZipTextBox = New System.Windows.Forms.TextBox
        Me.PARTICIPANTMAILCityTextBox = New System.Windows.Forms.TextBox
        Me.PARTICIPANTMAILAddress2TextBox = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.PARTICIPANTCHECKAddressGroupBox.SuspendLayout()
        CType(Me.RegAddressDS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PARTICIPANTMAILAddressGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'PARTICIPANTCHECKAddressGroupBox
        '
        Me.PARTICIPANTCHECKAddressGroupBox.Controls.Add(Me.Label1)
        Me.PARTICIPANTCHECKAddressGroupBox.Controls.Add(Me.PARTICIPANTCHECKCountryTextBox)
        Me.PARTICIPANTCHECKAddressGroupBox.Controls.Add(Me.Label2)
        Me.PARTICIPANTCHECKAddressGroupBox.Controls.Add(Me.PARTICIPANTCHECKZip4TextBox)
        Me.PARTICIPANTCHECKAddressGroupBox.Controls.Add(Me.PARTICIPANTCHECKAddress1TextBox)
        Me.PARTICIPANTCHECKAddressGroupBox.Controls.Add(Me.PARTICIPANTCHECKCityTextBox)
        Me.PARTICIPANTCHECKAddressGroupBox.Controls.Add(Me.PARTICIPANTCHECKAddress2TextBox)
        Me.PARTICIPANTCHECKAddressGroupBox.Controls.Add(Me.Label3)
        Me.PARTICIPANTCHECKAddressGroupBox.Controls.Add(Me.Label4)
        Me.PARTICIPANTCHECKAddressGroupBox.Controls.Add(Me.Label5)
        Me.PARTICIPANTCHECKAddressGroupBox.Controls.Add(Me.Label6)
        Me.PARTICIPANTCHECKAddressGroupBox.Controls.Add(Me.PARTICIPANTCHECKStateListBox)
        Me.PARTICIPANTCHECKAddressGroupBox.Controls.Add(Me.PARTICIPANTCHECKZipTextBox)
        Me.PARTICIPANTCHECKAddressGroupBox.Location = New System.Drawing.Point(8, 144)
        Me.PARTICIPANTCHECKAddressGroupBox.Name = "PARTICIPANTCHECKAddressGroupBox"
        Me.PARTICIPANTCHECKAddressGroupBox.Size = New System.Drawing.Size(336, 131)
        Me.PARTICIPANTCHECKAddressGroupBox.TabIndex = 2
        Me.PARTICIPANTCHECKAddressGroupBox.TabStop = False
        Me.PARTICIPANTCHECKAddressGroupBox.Text = "Check Address"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 104)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(44, 16)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Country"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PARTICIPANTCHECKCountryTextBox
        '
        Me.PARTICIPANTCHECKCountryTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.RegAddressDS, "REG_ADDRESS.CHECK_COUNTRY"))
        Me.PARTICIPANTCHECKCountryTextBox.Location = New System.Drawing.Point(64, 102)
        Me.PARTICIPANTCHECKCountryTextBox.Name = "PARTICIPANTCHECKCountryTextBox"
        Me.PARTICIPANTCHECKCountryTextBox.ReadOnly = True
        Me.PARTICIPANTCHECKCountryTextBox.Size = New System.Drawing.Size(264, 20)
        Me.PARTICIPANTCHECKCountryTextBox.TabIndex = 16
        Me.PARTICIPANTCHECKCountryTextBox.Text = ""
        '
        'RegAddressDS
        '
        Me.RegAddressDS.DataSetName = "RegAddressDS"
        Me.RegAddressDS.Locale = New System.Globalization.CultureInfo("en-US")
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(234, 82)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(8, 16)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "-"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PARTICIPANTCHECKZip4TextBox
        '
        Me.PARTICIPANTCHECKZip4TextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.RegAddressDS, "REG_ADDRESS.CHECK_ZIP2"))
        Me.PARTICIPANTCHECKZip4TextBox.Location = New System.Drawing.Point(248, 81)
        Me.PARTICIPANTCHECKZip4TextBox.MaxLength = 4
        Me.PARTICIPANTCHECKZip4TextBox.Name = "PARTICIPANTCHECKZip4TextBox"
        Me.PARTICIPANTCHECKZip4TextBox.ReadOnly = True
        Me.PARTICIPANTCHECKZip4TextBox.Size = New System.Drawing.Size(40, 20)
        Me.PARTICIPANTCHECKZip4TextBox.TabIndex = 17
        Me.PARTICIPANTCHECKZip4TextBox.Text = ""
        '
        'PARTICIPANTCHECKAddress1TextBox
        '
        Me.PARTICIPANTCHECKAddress1TextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.PARTICIPANTCHECKAddress1TextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.RegAddressDS, "REG_ADDRESS.CHECK_ADDRESS1"))
        Me.PARTICIPANTCHECKAddress1TextBox.Location = New System.Drawing.Point(64, 19)
        Me.PARTICIPANTCHECKAddress1TextBox.MaxLength = 50
        Me.PARTICIPANTCHECKAddress1TextBox.Name = "PARTICIPANTCHECKAddress1TextBox"
        Me.PARTICIPANTCHECKAddress1TextBox.ReadOnly = True
        Me.PARTICIPANTCHECKAddress1TextBox.Size = New System.Drawing.Size(264, 20)
        Me.PARTICIPANTCHECKAddress1TextBox.TabIndex = 18
        Me.PARTICIPANTCHECKAddress1TextBox.Text = ""
        '
        'PARTICIPANTCHECKCityTextBox
        '
        Me.PARTICIPANTCHECKCityTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.PARTICIPANTCHECKCityTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.RegAddressDS, "REG_ADDRESS.CHECK_CITY"))
        Me.PARTICIPANTCHECKCityTextBox.Location = New System.Drawing.Point(64, 60)
        Me.PARTICIPANTCHECKCityTextBox.MaxLength = 50
        Me.PARTICIPANTCHECKCityTextBox.Name = "PARTICIPANTCHECKCityTextBox"
        Me.PARTICIPANTCHECKCityTextBox.ReadOnly = True
        Me.PARTICIPANTCHECKCityTextBox.Size = New System.Drawing.Size(264, 20)
        Me.PARTICIPANTCHECKCityTextBox.TabIndex = 2
        Me.PARTICIPANTCHECKCityTextBox.Text = ""
        '
        'PARTICIPANTCHECKAddress2TextBox
        '
        Me.PARTICIPANTCHECKAddress2TextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.PARTICIPANTCHECKAddress2TextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.RegAddressDS, "REG_ADDRESS.CHECK_ADDRESS2"))
        Me.PARTICIPANTCHECKAddress2TextBox.Location = New System.Drawing.Point(64, 39)
        Me.PARTICIPANTCHECKAddress2TextBox.MaxLength = 50
        Me.PARTICIPANTCHECKAddress2TextBox.Name = "PARTICIPANTCHECKAddress2TextBox"
        Me.PARTICIPANTCHECKAddress2TextBox.ReadOnly = True
        Me.PARTICIPANTCHECKAddress2TextBox.Size = New System.Drawing.Size(264, 20)
        Me.PARTICIPANTCHECKAddress2TextBox.TabIndex = 19
        Me.PARTICIPANTCHECKAddress2TextBox.Text = ""
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(128, 83)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(20, 16)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Zip"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(16, 84)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(31, 16)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "State"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(16, 19)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(46, 16)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Address"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(16, 62)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(24, 16)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "City"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PARTICIPANTCHECKStateListBox
        '
        Me.PARTICIPANTCHECKStateListBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.RegAddressDS, "REG_ADDRESS.CHECK_STATE"))
        Me.PARTICIPANTCHECKStateListBox.Location = New System.Drawing.Point(64, 81)
        Me.PARTICIPANTCHECKStateListBox.MaxLength = 4
        Me.PARTICIPANTCHECKStateListBox.Name = "PARTICIPANTCHECKStateListBox"
        Me.PARTICIPANTCHECKStateListBox.ReadOnly = True
        Me.PARTICIPANTCHECKStateListBox.Size = New System.Drawing.Size(40, 20)
        Me.PARTICIPANTCHECKStateListBox.TabIndex = 21
        Me.PARTICIPANTCHECKStateListBox.Text = ""
        '
        'PARTICIPANTCHECKZipTextBox
        '
        Me.PARTICIPANTCHECKZipTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.RegAddressDS, "REG_ADDRESS.CHECK_ZIP1"))
        Me.PARTICIPANTCHECKZipTextBox.Location = New System.Drawing.Point(160, 81)
        Me.PARTICIPANTCHECKZipTextBox.MaxLength = 5
        Me.PARTICIPANTCHECKZipTextBox.Name = "PARTICIPANTCHECKZipTextBox"
        Me.PARTICIPANTCHECKZipTextBox.ReadOnly = True
        Me.PARTICIPANTCHECKZipTextBox.Size = New System.Drawing.Size(72, 20)
        Me.PARTICIPANTCHECKZipTextBox.TabIndex = 4
        Me.PARTICIPANTCHECKZipTextBox.Text = ""
        '
        'PARTICIPANTMAILAddressGroupBox
        '
        Me.PARTICIPANTMAILAddressGroupBox.Controls.Add(Me.PARTICIPANTMAILStateListBox)
        Me.PARTICIPANTMAILAddressGroupBox.Controls.Add(Me.Label7)
        Me.PARTICIPANTMAILAddressGroupBox.Controls.Add(Me.PARTICIPANTMAILCountryTextBox)
        Me.PARTICIPANTMAILAddressGroupBox.Controls.Add(Me.Label8)
        Me.PARTICIPANTMAILAddressGroupBox.Controls.Add(Me.PARTICIPANTMAILZip4TextBox)
        Me.PARTICIPANTMAILAddressGroupBox.Controls.Add(Me.PARTICIPANTMAILAddress1TextBox)
        Me.PARTICIPANTMAILAddressGroupBox.Controls.Add(Me.PARTICIPANTMAILZipTextBox)
        Me.PARTICIPANTMAILAddressGroupBox.Controls.Add(Me.PARTICIPANTMAILCityTextBox)
        Me.PARTICIPANTMAILAddressGroupBox.Controls.Add(Me.PARTICIPANTMAILAddress2TextBox)
        Me.PARTICIPANTMAILAddressGroupBox.Controls.Add(Me.Label9)
        Me.PARTICIPANTMAILAddressGroupBox.Controls.Add(Me.Label10)
        Me.PARTICIPANTMAILAddressGroupBox.Controls.Add(Me.Label11)
        Me.PARTICIPANTMAILAddressGroupBox.Controls.Add(Me.Label12)
        Me.PARTICIPANTMAILAddressGroupBox.Controls.Add(Me.TextBox1)
        Me.PARTICIPANTMAILAddressGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.PARTICIPANTMAILAddressGroupBox.Name = "PARTICIPANTMAILAddressGroupBox"
        Me.PARTICIPANTMAILAddressGroupBox.Size = New System.Drawing.Size(336, 128)
        Me.PARTICIPANTMAILAddressGroupBox.TabIndex = 3
        Me.PARTICIPANTMAILAddressGroupBox.TabStop = False
        Me.PARTICIPANTMAILAddressGroupBox.Text = "Mail Address"
        '
        'PARTICIPANTMAILStateListBox
        '
        Me.PARTICIPANTMAILStateListBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.RegAddressDS, "REG_ADDRESS.STATE"))
        Me.PARTICIPANTMAILStateListBox.Location = New System.Drawing.Point(64, 79)
        Me.PARTICIPANTMAILStateListBox.MaxLength = 4
        Me.PARTICIPANTMAILStateListBox.Name = "PARTICIPANTMAILStateListBox"
        Me.PARTICIPANTMAILStateListBox.ReadOnly = True
        Me.PARTICIPANTMAILStateListBox.Size = New System.Drawing.Size(40, 20)
        Me.PARTICIPANTMAILStateListBox.TabIndex = 20
        Me.PARTICIPANTMAILStateListBox.Text = ""
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(16, 103)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(44, 16)
        Me.Label7.TabIndex = 15
        Me.Label7.Text = "Country"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PARTICIPANTMAILCountryTextBox
        '
        Me.PARTICIPANTMAILCountryTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.RegAddressDS, "REG_ADDRESS.COUNTRY"))
        Me.PARTICIPANTMAILCountryTextBox.Location = New System.Drawing.Point(64, 100)
        Me.PARTICIPANTMAILCountryTextBox.Name = "PARTICIPANTMAILCountryTextBox"
        Me.PARTICIPANTMAILCountryTextBox.ReadOnly = True
        Me.PARTICIPANTMAILCountryTextBox.Size = New System.Drawing.Size(264, 20)
        Me.PARTICIPANTMAILCountryTextBox.TabIndex = 16
        Me.PARTICIPANTMAILCountryTextBox.Text = ""
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(232, 82)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(8, 16)
        Me.Label8.TabIndex = 5
        Me.Label8.Text = "-"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PARTICIPANTMAILZip4TextBox
        '
        Me.PARTICIPANTMAILZip4TextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.RegAddressDS, "REG_ADDRESS.ZIP2"))
        Me.PARTICIPANTMAILZip4TextBox.Location = New System.Drawing.Point(248, 80)
        Me.PARTICIPANTMAILZip4TextBox.MaxLength = 4
        Me.PARTICIPANTMAILZip4TextBox.Name = "PARTICIPANTMAILZip4TextBox"
        Me.PARTICIPANTMAILZip4TextBox.ReadOnly = True
        Me.PARTICIPANTMAILZip4TextBox.Size = New System.Drawing.Size(40, 20)
        Me.PARTICIPANTMAILZip4TextBox.TabIndex = 17
        Me.PARTICIPANTMAILZip4TextBox.Text = ""
        '
        'PARTICIPANTMAILAddress1TextBox
        '
        Me.PARTICIPANTMAILAddress1TextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.PARTICIPANTMAILAddress1TextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.RegAddressDS, "REG_ADDRESS.ADDRESS1"))
        Me.PARTICIPANTMAILAddress1TextBox.Location = New System.Drawing.Point(64, 19)
        Me.PARTICIPANTMAILAddress1TextBox.MaxLength = 50
        Me.PARTICIPANTMAILAddress1TextBox.Name = "PARTICIPANTMAILAddress1TextBox"
        Me.PARTICIPANTMAILAddress1TextBox.ReadOnly = True
        Me.PARTICIPANTMAILAddress1TextBox.Size = New System.Drawing.Size(264, 20)
        Me.PARTICIPANTMAILAddress1TextBox.TabIndex = 18
        Me.PARTICIPANTMAILAddress1TextBox.Text = ""
        '
        'PARTICIPANTMAILZipTextBox
        '
        Me.PARTICIPANTMAILZipTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.RegAddressDS, "REG_ADDRESS.ZIP1"))
        Me.PARTICIPANTMAILZipTextBox.Location = New System.Drawing.Point(152, 80)
        Me.PARTICIPANTMAILZipTextBox.MaxLength = 5
        Me.PARTICIPANTMAILZipTextBox.Name = "PARTICIPANTMAILZipTextBox"
        Me.PARTICIPANTMAILZipTextBox.ReadOnly = True
        Me.PARTICIPANTMAILZipTextBox.Size = New System.Drawing.Size(72, 20)
        Me.PARTICIPANTMAILZipTextBox.TabIndex = 4
        Me.PARTICIPANTMAILZipTextBox.Text = ""
        '
        'PARTICIPANTMAILCityTextBox
        '
        Me.PARTICIPANTMAILCityTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.PARTICIPANTMAILCityTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.RegAddressDS, "REG_ADDRESS.CITY"))
        Me.PARTICIPANTMAILCityTextBox.Location = New System.Drawing.Point(64, 59)
        Me.PARTICIPANTMAILCityTextBox.MaxLength = 50
        Me.PARTICIPANTMAILCityTextBox.Name = "PARTICIPANTMAILCityTextBox"
        Me.PARTICIPANTMAILCityTextBox.ReadOnly = True
        Me.PARTICIPANTMAILCityTextBox.Size = New System.Drawing.Size(264, 20)
        Me.PARTICIPANTMAILCityTextBox.TabIndex = 2
        Me.PARTICIPANTMAILCityTextBox.Text = ""
        '
        'PARTICIPANTMAILAddress2TextBox
        '
        Me.PARTICIPANTMAILAddress2TextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.PARTICIPANTMAILAddress2TextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.RegAddressDS, "REG_ADDRESS.ADDRESS2"))
        Me.PARTICIPANTMAILAddress2TextBox.Location = New System.Drawing.Point(64, 39)
        Me.PARTICIPANTMAILAddress2TextBox.MaxLength = 50
        Me.PARTICIPANTMAILAddress2TextBox.Name = "PARTICIPANTMAILAddress2TextBox"
        Me.PARTICIPANTMAILAddress2TextBox.ReadOnly = True
        Me.PARTICIPANTMAILAddress2TextBox.Size = New System.Drawing.Size(264, 20)
        Me.PARTICIPANTMAILAddress2TextBox.TabIndex = 19
        Me.PARTICIPANTMAILAddress2TextBox.Text = ""
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(120, 80)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(20, 16)
        Me.Label9.TabIndex = 3
        Me.Label9.Text = "Zip"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(16, 82)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(31, 16)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "State"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(16, 19)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(46, 16)
        Me.Label11.TabIndex = 1
        Me.Label11.Text = "Address"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(16, 61)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(24, 16)
        Me.Label12.TabIndex = 1
        Me.Label12.Text = "City"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextBox1
        '
        Me.TextBox1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBox1.Location = New System.Drawing.Point(64, 24)
        Me.TextBox1.MaxLength = 50
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(264, 20)
        Me.TextBox1.TabIndex = 18
        Me.TextBox1.Text = ""
        '
        'PARTICIPANTControl
        '
        Me.Controls.Add(Me.PARTICIPANTMAILAddressGroupBox)
        Me.Controls.Add(Me.PARTICIPANTCHECKAddressGroupBox)
        Me.Name = "PARTICIPANTControl"
        Me.Size = New System.Drawing.Size(352, 280)
        Me.PARTICIPANTCHECKAddressGroupBox.ResumeLayout(False)
        CType(Me.RegAddressDS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PARTICIPANTMAILAddressGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Properties"
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the FamilyID of the Document.")> _
    Public Property FamilyID() As Integer
        Get
            Return _FamilyID
        End Get
        Set(ByVal value As Integer)
            _FamilyID = Value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the RelationID of the Document.")> _
    Public Property RelationID() As Integer
        Get
            Return _RelationID
        End Get
        Set(ByVal value As Integer)
            _RelationID = Value
        End Set
    End Property

#End Region

#Region "Constructor"
    Public Sub New(ByVal FamilyID As Integer, ByVal RelationID As Integer)
        Me.New()

        _FamilyID = FamilyID
        _RelationID = RelationID
    End Sub

#End Region

#Region "Form\Button Events"
    Private Sub PARTICIPANTControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        _Loading = False
    End Sub

#End Region

#Region "Custom Subs\Functions"
    Public Sub LoadPARTICIPANT(ByVal FamilyID As Integer, ByVal RelationID As Integer)
        Try
            _FamilyID = FamilyID
            _RelationID = RelationID

            LoadPARTICIPANT()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub LoadPARTICIPANT()

        Try

            RegAddressDS.REG_ADDRESS.Rows.Clear()
            RegAddressDS = CType(RegMastDAL.GetPARTICIPANTInformation(_FamilyID, _RelationID, RegAddressDS), RegAddressDS)

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub AllowNull_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            If CStr(e.Value).Trim = "" Then
                e.Value = DBNull.Value
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function LoadPARTICIPANT(ByVal familyId As Integer, ByVal relationId As Integer, ByVal DS As DataSet) As DataSet
        Try
            RegAddressDS.REG_ADDRESS.Rows.Clear()
            RegAddressDS = CType(RegMastDAL.GetPARTICIPANTInformation(_FamilyID, _RelationID, RegAddressDS), RegAddressDS)

            Return RegAddressDS

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Sub LoadPARTICIPANT(ByVal DT As DataTable)
        Try
            _FamilyID = FamilyID
            _RelationID = RelationID

            RegAddressDS.REG_ADDRESS.Rows.Clear()

            If DT.Rows.Count > 0 Then RegAddressDS.REG_ADDRESS.ImportRow(DT.Rows(0))

        Catch ex As Exception
            Throw
        End Try
    End Sub
#End Region

End Class