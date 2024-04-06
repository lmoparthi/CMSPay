Option Strict On

Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports DDTek.DB2
Imports System.Data.Common

''' -----------------------------------------------------------------------------
''' ''' Project	 : ProviderWork
''' Class	 : Work
''' 
''' -----------------------------------------------------------------------------
''' <summary>
''' Form to handle Provider Work
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	
''' </history>
''' -----------------------------------------------------------------------------
<SharedInterfaces.PlugIn("ProviderWork", "Provider")> Public Class ProviderWork
    Inherits System.Windows.Forms.Form

    Const SHOWLOADTIME As Boolean = False

    Private DoSelectAll As Boolean = False
    Private ErrorProvider As New ErrorProviderExtended

    ReadOnly DomainUser As String = SystemInformation.UserName

    Private Structure ProviderKeyStructure
        Dim ProviderID As Integer
        Dim CommentID As Integer
        Dim ProviderTaxID As Integer
        Dim ProviderTaxType As String
        Dim ParentID As Integer
        Dim Name As String
        Dim LastUpdatedOn As DateTime

        Sub Empty()
            ProviderID = Nothing
            CommentID = Nothing
            ProviderTaxID = Nothing
            ParentID = Nothing
        End Sub

    End Structure

    Private Enum LogAction
        ProviderHeaderAdd = 1
        ProviderHeaderUpdate = 2
        ProviderAddressAdd = 3
        ProviderAddressUpdate = 4
        ProviderLicenseAdd = 5
        ProviderLicenseUpdate = 6
        ProviderCommentsAdd = 7
        ProviderCommentsUpdate = 8
        ProviderTINSSNUpdate = 9
        ProviderAddressDeleted = 10
        ProviderLicenseDeleted = 11
    End Enum

    Private mobjMessage As SharedInterfaces.IMessage
    Private mProviderTaxID As Integer
    Private ProviderKey As New ProviderKeyStructure
    Private ProviderDataRow As DataRow
    Private mTransaction As DbTransaction

    Private ProviderTaxIDTypes As New ArrayList
    Private ProviderAlertTypesDT As DataTable
    Private ProviderAddressTypesDT As DataTable
    Private ProviderAddressStatesDT As DataTable
    Private ProviderLicenseTypesDT As DataTable
    Private ProviderLicensesDT As DataTable

    Private ProviderAssociatedSummaryBS As BindingSource
    Private ProviderHistoryBS As BindingSource
    Private ProviderLicensesBS As BindingSource

    Private _UniqueID As String
    Private LastAlertIndex As Integer = -1
    Private Activating As Boolean = False

    Private p As Pen = New Pen(Color.White)
    Private brBlueFontBrush As SolidBrush = New SolidBrush(Color.Blue)
    Private arBrushes(2) As SolidBrush

    Private lastAddressIndex As Integer = -1
    Private AddressNeedsValidation As Boolean = False

    Private _GridPosX As Integer
    Private _GridPosY As Integer

    Friend WithEvents SystemOnlyProviderCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents ProviderAddressSummaryWithCommentsDS As ProviderAddressSummaryWithCommentsDS
    Private hoverCell As New DataGridCell
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents HRADisallowedProviderCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents ProviderAddressSummaryWithCommentsBS As BindingSource
    Friend WithEvents ProviderHeaderBS As BindingSource
    Private _APPKEY As String = "UFCW\Provider\"

#Region "Public Properties"

    <System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal Value As String)
            _APPKEY = Value
        End Set
    End Property

#End Region

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal objMsg As SharedInterfaces.IMessage, ByVal ProviderTaxID As Integer, Optional ByVal Transaction As DbTransaction = Nothing)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        mobjMessage = objMsg
        mProviderTaxID = ProviderTaxID

    End Sub


    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents MenuItem5 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem11 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem12 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem13 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem14 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem15 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem16 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem17 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem18 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem19 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem20 As System.Windows.Forms.MenuItem
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label54 As System.Windows.Forms.Label
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents Label56 As System.Windows.Forms.Label
    Friend WithEvents Label58 As System.Windows.Forms.Label
    Friend WithEvents AnnotateButton As System.Windows.Forms.Button
    Friend WithEvents ProviderIDTextBox As System.Windows.Forms.TextBox 'System.Windows.Forms.TextBox
    Friend WithEvents AboutMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ProviderZipTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ProviderCityTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ProviderNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents AddressesTabPage As System.Windows.Forms.TabPage
    Friend WithEvents WorkToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents DIList As System.Windows.Forms.ImageList
    Friend WithEvents DetailGridContextMenu As System.Windows.Forms.ContextMenu
    Friend WithEvents UpdateMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents FileMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents RefreshMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ToolbarContextMenu As System.Windows.Forms.ContextMenu
    Friend WithEvents HoldContextMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents RefreshContextMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents PendContextMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents AccumulatorsGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents ClearMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ProviderTabPage As System.Windows.Forms.TabPage
    Friend WithEvents LicensesTabPage As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ExitButton As System.Windows.Forms.Button
    Friend WithEvents ProviderZip4TextBox As System.Windows.Forms.TextBox
    Friend WithEvents ProviderLicensesDS As LicensesDS
    Friend WithEvents ProviderAltPhoneTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents ProviderAltContactTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents AddressTypesDS As AddressTypesDS
    Friend WithEvents AddressTabs As System.Windows.Forms.TabControl
    Friend WithEvents ProviderAssociatedSummaryDS As ProviderAssociatedSummaryDS
    Friend WithEvents LicensesDataGrid As DataGridCustom
    Friend WithEvents AssociatedProvidersDataGrid As DataGridCustom
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ProviderAltEmailTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ProviderAltPhoneExtTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents ProviderPrimaryContactTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ProviderPrimaryPhoneExtTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ProviderPrimaryEmailTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label70 As System.Windows.Forms.Label
    Friend WithEvents ProviderPrimaryPhoneTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents ProviderAddressGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents SuspendedProviderAddressCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents PrimaryContactGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents AlternativeContactGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents ProviderAddress1TextBox As System.Windows.Forms.TextBox
    Friend WithEvents ProviderAddress2TextBox As System.Windows.Forms.TextBox
    Friend WithEvents SuspendedProviderCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents AlertTypesDS As ProviderAlertTypesDS
    Friend WithEvents ProviderAlertTypesListBox As System.Windows.Forms.ComboBox
    Friend WithEvents WorkStatusBar As System.Windows.Forms.StatusBar
    Friend WithEvents InfoStatusBarPanel As System.Windows.Forms.StatusBarPanel
    Friend WithEvents DomainUserStatusBarPanel As System.Windows.Forms.StatusBarPanel
    Friend WithEvents DataStatusBarPanel As System.Windows.Forms.StatusBarPanel
    Friend WithEvents DateStatusBarPanel As System.Windows.Forms.StatusBarPanel
    Friend WithEvents StatesDS As StatesDS
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents PPOCProviderCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents ProviderAddressesDS As AddressesDS
    Friend WithEvents ProviderUpdateButton As System.Windows.Forms.Button
    Friend WithEvents ProviderAddButton As System.Windows.Forms.Button
    Friend WithEvents ProviderNPITextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents AddressModifyButton As System.Windows.Forms.Button
    Friend WithEvents ProviderAddressTypesListBox As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ProviderStateListBox As System.Windows.Forms.ComboBox
    Friend WithEvents ProviderTaxIDTypeListBox As System.Windows.Forms.ComboBox
    Friend WithEvents MenuExit As System.Windows.Forms.MenuItem
    Friend WithEvents StatesDV As System.Data.DataView
    Friend WithEvents ProviderCountryTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Comments As System.Windows.Forms.TabPage
    Friend WithEvents History As System.Windows.Forms.TabPage
    Friend WithEvents ProviderComments As System.Windows.Forms.RichTextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents ProviderHistoryDS As ProviderHistoryDS
    Friend WithEvents ProviderHistoryDataGrid As DataGridCustom
    Friend WithEvents ProviderHistory As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumn1 As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents lblProviderCommentsModifiedBy As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents lblProviderCommentsModifiedAt As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ProviderWork))
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.FileMenuItem = New System.Windows.Forms.MenuItem()
        Me.RefreshMenuItem = New System.Windows.Forms.MenuItem()
        Me.MenuExit = New System.Windows.Forms.MenuItem()
        Me.MenuItem5 = New System.Windows.Forms.MenuItem()
        Me.AboutMenuItem = New System.Windows.Forms.MenuItem()
        Me.MenuItem11 = New System.Windows.Forms.MenuItem()
        Me.MenuItem12 = New System.Windows.Forms.MenuItem()
        Me.MenuItem13 = New System.Windows.Forms.MenuItem()
        Me.MenuItem14 = New System.Windows.Forms.MenuItem()
        Me.MenuItem15 = New System.Windows.Forms.MenuItem()
        Me.MenuItem16 = New System.Windows.Forms.MenuItem()
        Me.MenuItem17 = New System.Windows.Forms.MenuItem()
        Me.MenuItem18 = New System.Windows.Forms.MenuItem()
        Me.MenuItem19 = New System.Windows.Forms.MenuItem()
        Me.MenuItem20 = New System.Windows.Forms.MenuItem()
        Me.ToolbarContextMenu = New System.Windows.Forms.ContextMenu()
        Me.HoldContextMenuItem = New System.Windows.Forms.MenuItem()
        Me.RefreshContextMenuItem = New System.Windows.Forms.MenuItem()
        Me.PendContextMenuItem = New System.Windows.Forms.MenuItem()
        Me.AddressTabs = New System.Windows.Forms.TabControl()
        Me.AddressesTabPage = New System.Windows.Forms.TabPage()
        Me.ProviderAddressTypesListBox = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ProviderAddressGroupBox = New System.Windows.Forms.GroupBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.ProviderCountryTextBox = New System.Windows.Forms.TextBox()
        Me.ProviderAddressSummaryWithCommentsBS = New System.Windows.Forms.BindingSource(Me.components)
        Me.ProviderAddressSummaryWithCommentsDS = New ProviderAddressSummaryWithCommentsDS()
        Me.ProviderStateListBox = New System.Windows.Forms.ComboBox()
        Me.StatesDV = New System.Data.DataView()
        Me.StatesDS = New StatesDS()
        Me.SuspendedProviderAddressCheckBox = New System.Windows.Forms.CheckBox()
        Me.PrimaryContactGroupBox = New System.Windows.Forms.GroupBox()
        Me.ProviderPrimaryContactTextBox = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.ProviderPrimaryPhoneExtTextBox = New System.Windows.Forms.TextBox()
        Me.ProviderPrimaryEmailTextBox = New System.Windows.Forms.TextBox()
        Me.Label70 = New System.Windows.Forms.Label()
        Me.ProviderPrimaryPhoneTextBox = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.AlternativeContactGroupBox = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ProviderAltContactTextBox = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.ProviderAltPhoneTextBox = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ProviderAltEmailTextBox = New System.Windows.Forms.TextBox()
        Me.ProviderAltPhoneExtTextBox = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
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
        Me.AddressModifyButton = New System.Windows.Forms.Button()
        Me.LicensesTabPage = New System.Windows.Forms.TabPage()
        Me.LicensesDataGrid = New DataGridCustom()
        Me.Comments = New System.Windows.Forms.TabPage()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.lblProviderCommentsModifiedAt = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.lblProviderCommentsModifiedBy = New System.Windows.Forms.Label()
        Me.ProviderComments = New System.Windows.Forms.RichTextBox()
        Me.History = New System.Windows.Forms.TabPage()
        Me.ProviderHistoryDataGrid = New DataGridCustom()
        Me.ProviderHistory = New System.Windows.Forms.DataGridTableStyle()
        Me.DataGridTextBoxColumn1 = New System.Windows.Forms.DataGridTextBoxColumn()
        Me.ProviderTabPage = New System.Windows.Forms.TabPage()
        Me.AssociatedProvidersDataGrid = New DataGridCustom()
        Me.ProviderLicensesDS = New LicensesDS()
        Me.ProviderAssociatedSummaryDS = New ProviderAssociatedSummaryDS()
        Me.AddressTypesDS = New AddressTypesDS()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ProviderUpdateButton = New System.Windows.Forms.Button()
        Me.AnnotateButton = New System.Windows.Forms.Button()
        Me.DetailGridContextMenu = New System.Windows.Forms.ContextMenu()
        Me.UpdateMenuItem = New System.Windows.Forms.MenuItem()
        Me.ClearMenuItem = New System.Windows.Forms.MenuItem()
        Me.ProviderIDTextBox = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.ProviderNameTextBox = New System.Windows.Forms.TextBox()
        Me.WorkToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.ProviderNPITextBox = New System.Windows.Forms.TextBox()
        Me.ProviderTaxIDTypeListBox = New System.Windows.Forms.ComboBox()
        Me.PPOCProviderCheckBox = New System.Windows.Forms.CheckBox()
        Me.ProviderAlertTypesListBox = New System.Windows.Forms.ComboBox()
        Me.AlertTypesDS = New ProviderAlertTypesDS()
        Me.SuspendedProviderCheckBox = New System.Windows.Forms.CheckBox()
        Me.ProviderAddButton = New System.Windows.Forms.Button()
        Me.ExitButton = New System.Windows.Forms.Button()
        Me.SystemOnlyProviderCheckBox = New System.Windows.Forms.CheckBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.HRADisallowedProviderCheckBox = New System.Windows.Forms.CheckBox()
        Me.DIList = New System.Windows.Forms.ImageList(Me.components)
        Me.AccumulatorsGroupBox = New System.Windows.Forms.GroupBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ProviderAddressesDS = New AddressesDS()
        Me.WorkStatusBar = New System.Windows.Forms.StatusBar()
        Me.InfoStatusBarPanel = New System.Windows.Forms.StatusBarPanel()
        Me.DomainUserStatusBarPanel = New System.Windows.Forms.StatusBarPanel()
        Me.DataStatusBarPanel = New System.Windows.Forms.StatusBarPanel()
        Me.DateStatusBarPanel = New System.Windows.Forms.StatusBarPanel()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.ProviderHistoryDS = New ProviderHistoryDS()
        Me.ProviderHeaderBS = New System.Windows.Forms.BindingSource(Me.components)
        Me.AddressTabs.SuspendLayout()
        Me.AddressesTabPage.SuspendLayout()
        Me.ProviderAddressGroupBox.SuspendLayout()
        CType(Me.ProviderAddressSummaryWithCommentsBS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ProviderAddressSummaryWithCommentsDS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.StatesDV, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.StatesDS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PrimaryContactGroupBox.SuspendLayout()
        Me.AlternativeContactGroupBox.SuspendLayout()
        Me.LicensesTabPage.SuspendLayout()
        CType(Me.LicensesDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Comments.SuspendLayout()
        Me.History.SuspendLayout()
        CType(Me.ProviderHistoryDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ProviderTabPage.SuspendLayout()
        CType(Me.AssociatedProvidersDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ProviderLicensesDS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ProviderAssociatedSummaryDS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AddressTypesDS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AlertTypesDS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.ProviderAddressesDS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InfoStatusBarPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DomainUserStatusBarPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataStatusBarPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateStatusBarPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ProviderHistoryDS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ProviderHeaderBS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.FileMenuItem, Me.MenuItem5})
        '
        'FileMenuItem
        '
        Me.FileMenuItem.Index = 0
        Me.FileMenuItem.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.RefreshMenuItem, Me.MenuExit})
        Me.FileMenuItem.Text = "File"
        '
        'RefreshMenuItem
        '
        Me.RefreshMenuItem.Index = 0
        Me.RefreshMenuItem.Text = "&Refresh"
        '
        'MenuExit
        '
        Me.MenuExit.Index = 1
        Me.MenuExit.Text = "E&xit"
        '
        'MenuItem5
        '
        Me.MenuItem5.Index = 1
        Me.MenuItem5.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.AboutMenuItem})
        Me.MenuItem5.Text = "Help"
        '
        'AboutMenuItem
        '
        Me.AboutMenuItem.Index = 0
        Me.AboutMenuItem.Text = "&About"
        '
        'MenuItem11
        '
        Me.MenuItem11.Index = 2
        Me.MenuItem11.Text = "Hold"
        '
        'MenuItem12
        '
        Me.MenuItem12.Index = 1
        Me.MenuItem12.Text = "Cancel"
        '
        'MenuItem13
        '
        Me.MenuItem13.Index = 3
        Me.MenuItem13.Text = "Re-Price"
        '
        'MenuItem14
        '
        Me.MenuItem14.Index = -1
        Me.MenuItem14.Text = "Edit"
        '
        'MenuItem15
        '
        Me.MenuItem15.Index = 4
        Me.MenuItem15.Text = "Override"
        '
        'MenuItem16
        '
        Me.MenuItem16.Index = 0
        Me.MenuItem16.Text = "Save"
        '
        'MenuItem17
        '
        Me.MenuItem17.Index = -1
        Me.MenuItem17.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem16, Me.MenuItem12, Me.MenuItem11, Me.MenuItem13, Me.MenuItem15})
        Me.MenuItem17.Text = "File"
        '
        'MenuItem18
        '
        Me.MenuItem18.Index = -1
        Me.MenuItem18.Text = "View"
        '
        'MenuItem19
        '
        Me.MenuItem19.Index = -1
        Me.MenuItem19.Text = "Help"
        '
        'MenuItem20
        '
        Me.MenuItem20.Index = -1
        Me.MenuItem20.Text = "Tools"
        '
        'ToolbarContextMenu
        '
        Me.ToolbarContextMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.HoldContextMenuItem, Me.RefreshContextMenuItem, Me.PendContextMenuItem})
        '
        'HoldContextMenuItem
        '
        Me.HoldContextMenuItem.Index = 0
        Me.HoldContextMenuItem.Text = "&Hold"
        '
        'RefreshContextMenuItem
        '
        Me.RefreshContextMenuItem.Index = 1
        Me.RefreshContextMenuItem.Text = "&Refresh"
        '
        'PendContextMenuItem
        '
        Me.PendContextMenuItem.Index = 2
        Me.PendContextMenuItem.Text = "&Pend"
        '
        'AddressTabs
        '
        Me.AddressTabs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AddressTabs.Controls.Add(Me.AddressesTabPage)
        Me.AddressTabs.Controls.Add(Me.LicensesTabPage)
        Me.AddressTabs.Controls.Add(Me.Comments)
        Me.AddressTabs.Controls.Add(Me.History)
        Me.AddressTabs.Controls.Add(Me.ProviderTabPage)
        Me.AddressTabs.Location = New System.Drawing.Point(4, 139)
        Me.AddressTabs.Name = "AddressTabs"
        Me.AddressTabs.SelectedIndex = 0
        Me.AddressTabs.Size = New System.Drawing.Size(544, 380)
        Me.AddressTabs.TabIndex = 1
        Me.WorkToolTip.SetToolTip(Me.AddressTabs, "Click Tab to view additional information")
        '
        'AddressesTabPage
        '
        Me.AddressesTabPage.Controls.Add(Me.ProviderAddressTypesListBox)
        Me.AddressesTabPage.Controls.Add(Me.Label4)
        Me.AddressesTabPage.Controls.Add(Me.ProviderAddressGroupBox)
        Me.AddressesTabPage.Controls.Add(Me.AddressModifyButton)
        Me.AddressesTabPage.Location = New System.Drawing.Point(4, 22)
        Me.AddressesTabPage.Name = "AddressesTabPage"
        Me.AddressesTabPage.Size = New System.Drawing.Size(536, 354)
        Me.AddressesTabPage.TabIndex = 1
        Me.AddressesTabPage.Text = "Addresses"
        Me.AddressesTabPage.Visible = False
        '
        'ProviderAddressTypesListBox
        '
        Me.ProviderAddressTypesListBox.AllowDrop = True
        Me.ProviderAddressTypesListBox.DisplayMember = "List of valid addresses (MAIL address is required)"
        Me.ProviderAddressTypesListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ProviderAddressTypesListBox.Location = New System.Drawing.Point(68, 12)
        Me.ProviderAddressTypesListBox.Name = "ProviderAddressTypesListBox"
        Me.ProviderAddressTypesListBox.Size = New System.Drawing.Size(264, 21)
        Me.ProviderAddressTypesListBox.TabIndex = 0
        Me.ProviderAddressTypesListBox.ValueMember = "List of valid addresses (MAIL address is required)"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 12)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(49, 13)
        Me.Label4.TabIndex = 78
        Me.Label4.Text = "Category"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProviderAddressGroupBox
        '
        Me.ProviderAddressGroupBox.Controls.Add(Me.Label16)
        Me.ProviderAddressGroupBox.Controls.Add(Me.ProviderCountryTextBox)
        Me.ProviderAddressGroupBox.Controls.Add(Me.ProviderStateListBox)
        Me.ProviderAddressGroupBox.Controls.Add(Me.SuspendedProviderAddressCheckBox)
        Me.ProviderAddressGroupBox.Controls.Add(Me.PrimaryContactGroupBox)
        Me.ProviderAddressGroupBox.Controls.Add(Me.AlternativeContactGroupBox)
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
        Me.ProviderAddressGroupBox.Location = New System.Drawing.Point(8, 44)
        Me.ProviderAddressGroupBox.Name = "ProviderAddressGroupBox"
        Me.ProviderAddressGroupBox.Size = New System.Drawing.Size(520, 308)
        Me.ProviderAddressGroupBox.TabIndex = 0
        Me.ProviderAddressGroupBox.TabStop = False
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(12, 100)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(43, 13)
        Me.Label16.TabIndex = 15
        Me.Label16.Text = "Country"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProviderCountryTextBox
        '
        Me.ProviderCountryTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ProviderCountryTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderAddressSummaryWithCommentsBS, "COUNTRY", True))
        Me.ProviderCountryTextBox.Location = New System.Drawing.Point(60, 100)
        Me.ProviderCountryTextBox.Name = "ProviderCountryTextBox"
        Me.ProviderCountryTextBox.Size = New System.Drawing.Size(264, 20)
        Me.ProviderCountryTextBox.TabIndex = 13
        Me.WorkToolTip.SetToolTip(Me.ProviderCountryTextBox, "Required if State and Zip are blank")
        '
        'ProviderAddressSummaryWithCommentsBS
        '
        Me.ProviderAddressSummaryWithCommentsBS.DataMember = "PROVIDERADDRESSSUMMARYWITHCOMMENTS"
        Me.ProviderAddressSummaryWithCommentsBS.DataSource = Me.ProviderAddressSummaryWithCommentsDS
        '
        'ProviderAddressSummaryWithCommentsDS
        '
        Me.ProviderAddressSummaryWithCommentsDS.DataSetName = "ProviderAddressSummaryWithCommentsDS"
        Me.ProviderAddressSummaryWithCommentsDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'ProviderStateListBox
        '
        Me.ProviderStateListBox.AllowDrop = True
        Me.ProviderStateListBox.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.ProviderAddressSummaryWithCommentsBS, "STATE", True))
        Me.ProviderStateListBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderAddressSummaryWithCommentsBS, "STATE", True))
        Me.ProviderStateListBox.DataSource = Me.StatesDV
        Me.ProviderStateListBox.DisplayMember = "ABBRV"
        Me.ProviderStateListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ProviderStateListBox.Location = New System.Drawing.Point(60, 76)
        Me.ProviderStateListBox.MaxLength = 2
        Me.ProviderStateListBox.Name = "ProviderStateListBox"
        Me.ProviderStateListBox.Size = New System.Drawing.Size(52, 21)
        Me.ProviderStateListBox.TabIndex = 3
        Me.WorkToolTip.SetToolTip(Me.ProviderStateListBox, "State (Required)")
        Me.ProviderStateListBox.ValueMember = "ABBRV"
        '
        'StatesDV
        '
        Me.StatesDV.AllowDelete = False
        Me.StatesDV.AllowEdit = False
        Me.StatesDV.AllowNew = False
        Me.StatesDV.ApplyDefaultSort = True
        Me.StatesDV.Sort = "ABBRV"
        Me.StatesDV.Table = Me.StatesDS.StatesDS
        '
        'StatesDS
        '
        Me.StatesDS.DataSetName = "StatesDS"
        Me.StatesDS.Locale = New System.Globalization.CultureInfo("en-US")
        Me.StatesDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'SuspendedProviderAddressCheckBox
        '
        Me.SuspendedProviderAddressCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.SuspendedProviderAddressCheckBox.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.ProviderAddressSummaryWithCommentsBS, "AddressSuspended", True))
        Me.SuspendedProviderAddressCheckBox.Location = New System.Drawing.Point(384, 28)
        Me.SuspendedProviderAddressCheckBox.Name = "SuspendedProviderAddressCheckBox"
        Me.SuspendedProviderAddressCheckBox.Size = New System.Drawing.Size(112, 18)
        Me.SuspendedProviderAddressCheckBox.TabIndex = 12
        Me.SuspendedProviderAddressCheckBox.Text = "Suspend Address"
        Me.WorkToolTip.SetToolTip(Me.SuspendedProviderAddressCheckBox, "Prevents Address from being validated, and used in Check processes.")
        '
        'PrimaryContactGroupBox
        '
        Me.PrimaryContactGroupBox.Controls.Add(Me.ProviderPrimaryContactTextBox)
        Me.PrimaryContactGroupBox.Controls.Add(Me.Label8)
        Me.PrimaryContactGroupBox.Controls.Add(Me.ProviderPrimaryPhoneExtTextBox)
        Me.PrimaryContactGroupBox.Controls.Add(Me.ProviderPrimaryEmailTextBox)
        Me.PrimaryContactGroupBox.Controls.Add(Me.Label70)
        Me.PrimaryContactGroupBox.Controls.Add(Me.ProviderPrimaryPhoneTextBox)
        Me.PrimaryContactGroupBox.Controls.Add(Me.Label10)
        Me.PrimaryContactGroupBox.Controls.Add(Me.Label11)
        Me.PrimaryContactGroupBox.Location = New System.Drawing.Point(8, 124)
        Me.PrimaryContactGroupBox.Name = "PrimaryContactGroupBox"
        Me.PrimaryContactGroupBox.Size = New System.Drawing.Size(344, 80)
        Me.PrimaryContactGroupBox.TabIndex = 10
        Me.PrimaryContactGroupBox.TabStop = False
        Me.PrimaryContactGroupBox.Text = "Primary Contact"
        '
        'ProviderPrimaryContactTextBox
        '
        Me.ProviderPrimaryContactTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ProviderPrimaryContactTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderAddressSummaryWithCommentsBS, "CONTACT1", True))
        Me.ProviderPrimaryContactTextBox.Location = New System.Drawing.Point(52, 16)
        Me.ProviderPrimaryContactTextBox.MaxLength = 50
        Me.ProviderPrimaryContactTextBox.Name = "ProviderPrimaryContactTextBox"
        Me.ProviderPrimaryContactTextBox.Size = New System.Drawing.Size(264, 20)
        Me.ProviderPrimaryContactTextBox.TabIndex = 0
        Me.WorkToolTip.SetToolTip(Me.ProviderPrimaryContactTextBox, "Primary Contact (Optional)")
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(8, 20)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(35, 13)
        Me.Label8.TabIndex = 78
        Me.Label8.Text = "Name"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProviderPrimaryPhoneExtTextBox
        '
        Me.ProviderPrimaryPhoneExtTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderAddressSummaryWithCommentsBS, "EXTENSION1", True))
        Me.ProviderPrimaryPhoneExtTextBox.Location = New System.Drawing.Point(188, 36)
        Me.ProviderPrimaryPhoneExtTextBox.MaxLength = 4
        Me.ProviderPrimaryPhoneExtTextBox.Name = "ProviderPrimaryPhoneExtTextBox"
        Me.ProviderPrimaryPhoneExtTextBox.Size = New System.Drawing.Size(72, 20)
        Me.ProviderPrimaryPhoneExtTextBox.TabIndex = 3
        '
        'ProviderPrimaryEmailTextBox
        '
        Me.ProviderPrimaryEmailTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ProviderPrimaryEmailTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderAddressSummaryWithCommentsBS, "EMAIL1", True))
        Me.ProviderPrimaryEmailTextBox.Location = New System.Drawing.Point(52, 56)
        Me.ProviderPrimaryEmailTextBox.MaxLength = 50
        Me.ProviderPrimaryEmailTextBox.Name = "ProviderPrimaryEmailTextBox"
        Me.ProviderPrimaryEmailTextBox.Size = New System.Drawing.Size(264, 20)
        Me.ProviderPrimaryEmailTextBox.TabIndex = 4
        Me.WorkToolTip.SetToolTip(Me.ProviderPrimaryEmailTextBox, "Email address in format name@provider.com")
        '
        'Label70
        '
        Me.Label70.AutoSize = True
        Me.Label70.Location = New System.Drawing.Point(8, 60)
        Me.Label70.Name = "Label70"
        Me.Label70.Size = New System.Drawing.Size(32, 13)
        Me.Label70.TabIndex = 74
        Me.Label70.Text = "Email"
        Me.Label70.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProviderPrimaryPhoneTextBox
        '
        Me.ProviderPrimaryPhoneTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderAddressSummaryWithCommentsBS, "PHONE1", True))
        Me.ProviderPrimaryPhoneTextBox.Location = New System.Drawing.Point(52, 36)
        Me.ProviderPrimaryPhoneTextBox.MaxLength = 10
        Me.ProviderPrimaryPhoneTextBox.Name = "ProviderPrimaryPhoneTextBox"
        Me.ProviderPrimaryPhoneTextBox.Size = New System.Drawing.Size(104, 20)
        Me.ProviderPrimaryPhoneTextBox.TabIndex = 1
        Me.WorkToolTip.SetToolTip(Me.ProviderPrimaryPhoneTextBox, "10 Digit Phone number (Must include area code)")
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(8, 40)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(38, 13)
        Me.Label10.TabIndex = 73
        Me.Label10.Text = "Phone"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(160, 40)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(25, 13)
        Me.Label11.TabIndex = 2
        Me.Label11.Text = "Ext."
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'AlternativeContactGroupBox
        '
        Me.AlternativeContactGroupBox.Controls.Add(Me.Label3)
        Me.AlternativeContactGroupBox.Controls.Add(Me.ProviderAltContactTextBox)
        Me.AlternativeContactGroupBox.Controls.Add(Me.Label9)
        Me.AlternativeContactGroupBox.Controls.Add(Me.ProviderAltPhoneTextBox)
        Me.AlternativeContactGroupBox.Controls.Add(Me.Label2)
        Me.AlternativeContactGroupBox.Controls.Add(Me.ProviderAltEmailTextBox)
        Me.AlternativeContactGroupBox.Controls.Add(Me.ProviderAltPhoneExtTextBox)
        Me.AlternativeContactGroupBox.Controls.Add(Me.Label6)
        Me.AlternativeContactGroupBox.Location = New System.Drawing.Point(8, 208)
        Me.AlternativeContactGroupBox.Name = "AlternativeContactGroupBox"
        Me.AlternativeContactGroupBox.Size = New System.Drawing.Size(344, 88)
        Me.AlternativeContactGroupBox.TabIndex = 11
        Me.AlternativeContactGroupBox.TabStop = False
        Me.AlternativeContactGroupBox.Text = "Alternative Contact"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 68)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(32, 13)
        Me.Label3.TabIndex = 87
        Me.Label3.Text = "Email"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProviderAltContactTextBox
        '
        Me.ProviderAltContactTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ProviderAltContactTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderAddressSummaryWithCommentsBS, "CONTACT2", True))
        Me.ProviderAltContactTextBox.Location = New System.Drawing.Point(56, 20)
        Me.ProviderAltContactTextBox.MaxLength = 50
        Me.ProviderAltContactTextBox.Name = "ProviderAltContactTextBox"
        Me.ProviderAltContactTextBox.Size = New System.Drawing.Size(264, 20)
        Me.ProviderAltContactTextBox.TabIndex = 0
        Me.WorkToolTip.SetToolTip(Me.ProviderAltContactTextBox, "Alternative Contact (Optional)")
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(12, 24)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(35, 13)
        Me.Label9.TabIndex = 83
        Me.Label9.Text = "Name"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProviderAltPhoneTextBox
        '
        Me.ProviderAltPhoneTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderAddressSummaryWithCommentsBS, "PHONE2", True))
        Me.ProviderAltPhoneTextBox.Location = New System.Drawing.Point(56, 40)
        Me.ProviderAltPhoneTextBox.MaxLength = 10
        Me.ProviderAltPhoneTextBox.Name = "ProviderAltPhoneTextBox"
        Me.ProviderAltPhoneTextBox.Size = New System.Drawing.Size(104, 20)
        Me.ProviderAltPhoneTextBox.TabIndex = 1
        Me.WorkToolTip.SetToolTip(Me.ProviderAltPhoneTextBox, "10 Digit Phone number (Must include area code)")
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 44)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(38, 13)
        Me.Label2.TabIndex = 81
        Me.Label2.Text = "Phone"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProviderAltEmailTextBox
        '
        Me.ProviderAltEmailTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ProviderAltEmailTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderAddressSummaryWithCommentsBS, "EMAIL2", True))
        Me.ProviderAltEmailTextBox.Location = New System.Drawing.Point(56, 64)
        Me.ProviderAltEmailTextBox.MaxLength = 50
        Me.ProviderAltEmailTextBox.Name = "ProviderAltEmailTextBox"
        Me.ProviderAltEmailTextBox.Size = New System.Drawing.Size(264, 20)
        Me.ProviderAltEmailTextBox.TabIndex = 4
        Me.WorkToolTip.SetToolTip(Me.ProviderAltEmailTextBox, "Email address in format name@provider.com")
        '
        'ProviderAltPhoneExtTextBox
        '
        Me.ProviderAltPhoneExtTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderAddressSummaryWithCommentsBS, "EXTENSION2", True))
        Me.ProviderAltPhoneExtTextBox.Location = New System.Drawing.Point(188, 40)
        Me.ProviderAltPhoneExtTextBox.MaxLength = 4
        Me.ProviderAltPhoneExtTextBox.Name = "ProviderAltPhoneExtTextBox"
        Me.ProviderAltPhoneExtTextBox.Size = New System.Drawing.Size(72, 20)
        Me.ProviderAltPhoneExtTextBox.TabIndex = 3
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(160, 44)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(25, 13)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Ext."
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(221, 76)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(10, 13)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "-"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProviderZip4TextBox
        '
        Me.ProviderZip4TextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderAddressSummaryWithCommentsBS, "ZIP_4", True))
        Me.ProviderZip4TextBox.Location = New System.Drawing.Point(237, 76)
        Me.ProviderZip4TextBox.MaxLength = 4
        Me.ProviderZip4TextBox.Name = "ProviderZip4TextBox"
        Me.ProviderZip4TextBox.Size = New System.Drawing.Size(44, 20)
        Me.ProviderZip4TextBox.TabIndex = 5
        Me.WorkToolTip.SetToolTip(Me.ProviderZip4TextBox, "ZIP+4 (Optional)")
        '
        'ProviderAddress1TextBox
        '
        Me.ProviderAddress1TextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ProviderAddress1TextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderAddressSummaryWithCommentsBS, "ADDRESS_LINE1", True))
        Me.ProviderAddress1TextBox.Location = New System.Drawing.Point(60, 16)
        Me.ProviderAddress1TextBox.MaxLength = 55
        Me.ProviderAddress1TextBox.Name = "ProviderAddress1TextBox"
        Me.ProviderAddress1TextBox.Size = New System.Drawing.Size(264, 20)
        Me.ProviderAddress1TextBox.TabIndex = 0
        Me.WorkToolTip.SetToolTip(Me.ProviderAddress1TextBox, "Address (Required)")
        '
        'ProviderZipTextBox
        '
        Me.ProviderZipTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderAddressSummaryWithCommentsBS, "ZIP", True))
        Me.ProviderZipTextBox.Location = New System.Drawing.Point(141, 76)
        Me.ProviderZipTextBox.MaxLength = 5
        Me.ProviderZipTextBox.Name = "ProviderZipTextBox"
        Me.ProviderZipTextBox.Size = New System.Drawing.Size(72, 20)
        Me.ProviderZipTextBox.TabIndex = 4
        Me.WorkToolTip.SetToolTip(Me.ProviderZipTextBox, "5 Digit ZIP (Required)")
        '
        'ProviderCityTextBox
        '
        Me.ProviderCityTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ProviderCityTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderAddressSummaryWithCommentsBS, "CITY", True))
        Me.ProviderCityTextBox.Location = New System.Drawing.Point(60, 56)
        Me.ProviderCityTextBox.MaxLength = 50
        Me.ProviderCityTextBox.Name = "ProviderCityTextBox"
        Me.ProviderCityTextBox.Size = New System.Drawing.Size(264, 20)
        Me.ProviderCityTextBox.TabIndex = 2
        Me.WorkToolTip.SetToolTip(Me.ProviderCityTextBox, "City (Required)")
        '
        'ProviderAddress2TextBox
        '
        Me.ProviderAddress2TextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ProviderAddress2TextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderAddressSummaryWithCommentsBS, "ADDRESS_LINE2", True))
        Me.ProviderAddress2TextBox.Location = New System.Drawing.Point(60, 36)
        Me.ProviderAddress2TextBox.MaxLength = 55
        Me.ProviderAddress2TextBox.Name = "ProviderAddress2TextBox"
        Me.ProviderAddress2TextBox.Size = New System.Drawing.Size(264, 20)
        Me.ProviderAddress2TextBox.TabIndex = 1
        Me.WorkToolTip.SetToolTip(Me.ProviderAddress2TextBox, "Secondary Address line (Optional)")
        '
        'Label54
        '
        Me.Label54.AutoSize = True
        Me.Label54.Location = New System.Drawing.Point(117, 80)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(22, 13)
        Me.Label54.TabIndex = 3
        Me.Label54.Text = "Zip"
        Me.Label54.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label55
        '
        Me.Label55.AutoSize = True
        Me.Label55.Location = New System.Drawing.Point(12, 80)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(32, 13)
        Me.Label55.TabIndex = 2
        Me.Label55.Text = "State"
        Me.Label55.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label56
        '
        Me.Label56.AutoSize = True
        Me.Label56.Location = New System.Drawing.Point(12, 16)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(45, 13)
        Me.Label56.TabIndex = 1
        Me.Label56.Text = "Address"
        Me.Label56.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label58
        '
        Me.Label58.AutoSize = True
        Me.Label58.Location = New System.Drawing.Point(12, 60)
        Me.Label58.Name = "Label58"
        Me.Label58.Size = New System.Drawing.Size(24, 13)
        Me.Label58.TabIndex = 1
        Me.Label58.Text = "City"
        Me.Label58.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'AddressModifyButton
        '
        Me.AddressModifyButton.Location = New System.Drawing.Point(396, 8)
        Me.AddressModifyButton.Name = "AddressModifyButton"
        Me.AddressModifyButton.Size = New System.Drawing.Size(132, 23)
        Me.AddressModifyButton.TabIndex = 1
        Me.AddressModifyButton.Text = "Remove Address"
        Me.WorkToolTip.SetToolTip(Me.AddressModifyButton, "Removes selected address from Provider")
        '
        'LicensesTabPage
        '
        Me.LicensesTabPage.Controls.Add(Me.LicensesDataGrid)
        Me.LicensesTabPage.Location = New System.Drawing.Point(4, 22)
        Me.LicensesTabPage.Name = "LicensesTabPage"
        Me.LicensesTabPage.Size = New System.Drawing.Size(536, 354)
        Me.LicensesTabPage.TabIndex = 5
        Me.LicensesTabPage.Text = "License(s)"
        Me.LicensesTabPage.Visible = False
        '
        'LicensesDataGrid
        '
        Me.LicensesDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.LicensesDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.LicensesDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LicensesDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LicensesDataGrid.ADGroupsThatCanFind = ""
        Me.LicensesDataGrid.ADGroupsThatCanMultiSort = ""
        Me.LicensesDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LicensesDataGrid.AllowAutoSize = True
        Me.LicensesDataGrid.AllowColumnReorder = True
        Me.LicensesDataGrid.AllowCopy = False
        Me.LicensesDataGrid.AllowCustomize = True
        Me.LicensesDataGrid.AllowDelete = True
        Me.LicensesDataGrid.AllowDragDrop = False
        Me.LicensesDataGrid.AllowEdit = True
        Me.LicensesDataGrid.AllowExport = False
        Me.LicensesDataGrid.AllowFilter = True
        Me.LicensesDataGrid.AllowFind = True
        Me.LicensesDataGrid.AllowGoTo = True
        Me.LicensesDataGrid.AllowMultiSelect = True
        Me.LicensesDataGrid.AllowMultiSort = True
        Me.LicensesDataGrid.AllowNew = True
        Me.LicensesDataGrid.AllowPrint = False
        Me.LicensesDataGrid.AllowRefresh = True
        Me.LicensesDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LicensesDataGrid.AppKey = "UFCW\Provider\"
        Me.LicensesDataGrid.AutoSaveCols = True
        Me.LicensesDataGrid.BackgroundColor = System.Drawing.SystemColors.Control
        Me.LicensesDataGrid.CaptionText = "Associated Licenses"
        Me.LicensesDataGrid.ColumnHeaderLabel = Nothing
        Me.LicensesDataGrid.ColumnRePositioning = False
        Me.LicensesDataGrid.ColumnResizing = False
        Me.LicensesDataGrid.ConfirmDelete = True
        Me.LicensesDataGrid.CopySelectedOnly = True
        Me.LicensesDataGrid.CurrentBSPosition = -1
        Me.LicensesDataGrid.DataMember = ""
        Me.LicensesDataGrid.DragColumn = 0
        Me.LicensesDataGrid.ExportSelectedOnly = True
        Me.LicensesDataGrid.GridLineColor = System.Drawing.SystemColors.ControlDarkDark
        Me.LicensesDataGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.LicensesDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.LicensesDataGrid.HighlightedRow = Nothing
        Me.LicensesDataGrid.HighLightModifiedRows = False
        Me.LicensesDataGrid.IsMouseDown = False
        Me.LicensesDataGrid.LastGoToLine = ""
        Me.LicensesDataGrid.Location = New System.Drawing.Point(4, 8)
        Me.LicensesDataGrid.MultiSort = False
        Me.LicensesDataGrid.Name = "LicensesDataGrid"
        Me.LicensesDataGrid.OldSelectedRow = Nothing
        Me.LicensesDataGrid.PreviousBSPosition = -1
        Me.LicensesDataGrid.RetainRowSelectionAfterSort = True
        Me.LicensesDataGrid.SetRowOnRightClick = True
        Me.LicensesDataGrid.ShiftPressed = False
        Me.LicensesDataGrid.SingleClickBooleanColumns = True
        Me.LicensesDataGrid.Size = New System.Drawing.Size(522, 328)
        Me.LicensesDataGrid.Sort = Nothing
        Me.LicensesDataGrid.StyleName = ""
        Me.LicensesDataGrid.SubKey = ""
        Me.LicensesDataGrid.SuppressMouseDown = False
        Me.LicensesDataGrid.SuppressTriangle = False
        Me.LicensesDataGrid.TabIndex = 24
        Me.WorkToolTip.SetToolTip(Me.LicensesDataGrid, "Provider License(s)")
        '
        'Comments
        '
        Me.Comments.Controls.Add(Me.Label19)
        Me.Comments.Controls.Add(Me.lblProviderCommentsModifiedAt)
        Me.Comments.Controls.Add(Me.Label17)
        Me.Comments.Controls.Add(Me.lblProviderCommentsModifiedBy)
        Me.Comments.Controls.Add(Me.ProviderComments)
        Me.Comments.Location = New System.Drawing.Point(4, 22)
        Me.Comments.Name = "Comments"
        Me.Comments.Size = New System.Drawing.Size(536, 354)
        Me.Comments.TabIndex = 6
        Me.Comments.Text = "Comments"
        '
        'Label19
        '
        Me.Label19.Location = New System.Drawing.Point(335, 328)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(24, 16)
        Me.Label19.TabIndex = 4
        Me.Label19.Text = "On"
        '
        'lblProviderCommentsModifiedAt
        '
        Me.lblProviderCommentsModifiedAt.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderAddressSummaryWithCommentsBS, "CommentTimestamp", True))
        Me.lblProviderCommentsModifiedAt.Location = New System.Drawing.Point(369, 328)
        Me.lblProviderCommentsModifiedAt.Name = "lblProviderCommentsModifiedAt"
        Me.lblProviderCommentsModifiedAt.Size = New System.Drawing.Size(152, 16)
        Me.lblProviderCommentsModifiedAt.TabIndex = 3
        '
        'Label17
        '
        Me.Label17.Location = New System.Drawing.Point(9, 328)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(96, 16)
        Me.Label17.TabIndex = 2
        Me.Label17.Text = "Modified Last By:"
        '
        'lblProviderCommentsModifiedBy
        '
        Me.lblProviderCommentsModifiedBy.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderAddressSummaryWithCommentsBS, "CommentCreatedBy", True))
        Me.lblProviderCommentsModifiedBy.Location = New System.Drawing.Point(112, 328)
        Me.lblProviderCommentsModifiedBy.Name = "lblProviderCommentsModifiedBy"
        Me.lblProviderCommentsModifiedBy.Size = New System.Drawing.Size(216, 16)
        Me.lblProviderCommentsModifiedBy.TabIndex = 1
        '
        'ProviderComments
        '
        Me.ProviderComments.AcceptsTab = True
        Me.ProviderComments.AutoWordSelection = True
        Me.ProviderComments.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderAddressSummaryWithCommentsBS, "COMMENT_TEXT", True))
        Me.ProviderComments.Location = New System.Drawing.Point(8, 16)
        Me.ProviderComments.MaxLength = 1500
        Me.ProviderComments.Name = "ProviderComments"
        Me.ProviderComments.Size = New System.Drawing.Size(520, 304)
        Me.ProviderComments.TabIndex = 0
        Me.ProviderComments.Text = ""
        Me.WorkToolTip.SetToolTip(Me.ProviderComments, "Enter comments")
        '
        'History
        '
        Me.History.Controls.Add(Me.ProviderHistoryDataGrid)
        Me.History.Location = New System.Drawing.Point(4, 22)
        Me.History.Name = "History"
        Me.History.Size = New System.Drawing.Size(536, 354)
        Me.History.TabIndex = 7
        Me.History.Text = "History"
        '
        'ProviderHistoryDataGrid
        '
        Me.ProviderHistoryDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.ProviderHistoryDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.ProviderHistoryDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ProviderHistoryDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ProviderHistoryDataGrid.ADGroupsThatCanFind = ""
        Me.ProviderHistoryDataGrid.ADGroupsThatCanMultiSort = ""
        Me.ProviderHistoryDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ProviderHistoryDataGrid.AllowAutoSize = True
        Me.ProviderHistoryDataGrid.AllowColumnReorder = True
        Me.ProviderHistoryDataGrid.AllowCopy = False
        Me.ProviderHistoryDataGrid.AllowCustomize = True
        Me.ProviderHistoryDataGrid.AllowDelete = False
        Me.ProviderHistoryDataGrid.AllowDragDrop = False
        Me.ProviderHistoryDataGrid.AllowEdit = False
        Me.ProviderHistoryDataGrid.AllowExport = True
        Me.ProviderHistoryDataGrid.AllowFilter = True
        Me.ProviderHistoryDataGrid.AllowFind = True
        Me.ProviderHistoryDataGrid.AllowGoTo = True
        Me.ProviderHistoryDataGrid.AllowMultiSelect = False
        Me.ProviderHistoryDataGrid.AllowMultiSort = False
        Me.ProviderHistoryDataGrid.AllowNew = False
        Me.ProviderHistoryDataGrid.AllowPrint = True
        Me.ProviderHistoryDataGrid.AllowRefresh = True
        Me.ProviderHistoryDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProviderHistoryDataGrid.AppKey = "UFCW\Provider\"
        Me.ProviderHistoryDataGrid.AutoSaveCols = True
        Me.ProviderHistoryDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.ProviderHistoryDataGrid.CaptionText = "Provider History"
        Me.ProviderHistoryDataGrid.ColumnHeaderLabel = Nothing
        Me.ProviderHistoryDataGrid.ColumnRePositioning = False
        Me.ProviderHistoryDataGrid.ColumnResizing = False
        Me.ProviderHistoryDataGrid.ConfirmDelete = True
        Me.ProviderHistoryDataGrid.CopySelectedOnly = True
        Me.ProviderHistoryDataGrid.CurrentBSPosition = -1
        Me.ProviderHistoryDataGrid.DataMember = ""
        Me.ProviderHistoryDataGrid.DragColumn = 0
        Me.ProviderHistoryDataGrid.ExportSelectedOnly = True
        Me.ProviderHistoryDataGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.ProviderHistoryDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ProviderHistoryDataGrid.HighlightedRow = Nothing
        Me.ProviderHistoryDataGrid.HighLightModifiedRows = False
        Me.ProviderHistoryDataGrid.IsMouseDown = False
        Me.ProviderHistoryDataGrid.LastGoToLine = ""
        Me.ProviderHistoryDataGrid.Location = New System.Drawing.Point(16, 8)
        Me.ProviderHistoryDataGrid.MultiSort = False
        Me.ProviderHistoryDataGrid.Name = "ProviderHistoryDataGrid"
        Me.ProviderHistoryDataGrid.OldSelectedRow = Nothing
        Me.ProviderHistoryDataGrid.PreviousBSPosition = -1
        Me.ProviderHistoryDataGrid.RetainRowSelectionAfterSort = True
        Me.ProviderHistoryDataGrid.RowHeadersVisible = False
        Me.ProviderHistoryDataGrid.SetRowOnRightClick = True
        Me.ProviderHistoryDataGrid.ShiftPressed = False
        Me.ProviderHistoryDataGrid.SingleClickBooleanColumns = True
        Me.ProviderHistoryDataGrid.Size = New System.Drawing.Size(504, 336)
        Me.ProviderHistoryDataGrid.Sort = Nothing
        Me.ProviderHistoryDataGrid.StyleName = ""
        Me.ProviderHistoryDataGrid.SubKey = ""
        Me.ProviderHistoryDataGrid.SuppressMouseDown = False
        Me.ProviderHistoryDataGrid.SuppressTriangle = False
        Me.ProviderHistoryDataGrid.TabIndex = 0
        Me.ProviderHistoryDataGrid.TableStyles.AddRange(New System.Windows.Forms.DataGridTableStyle() {Me.ProviderHistory})
        '
        'ProviderHistory
        '
        Me.ProviderHistory.DataGrid = Me.ProviderHistoryDataGrid
        Me.ProviderHistory.GridColumnStyles.AddRange(New System.Windows.Forms.DataGridColumnStyle() {Me.DataGridTextBoxColumn1})
        Me.ProviderHistory.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ProviderHistory.RowHeadersVisible = False
        '
        'DataGridTextBoxColumn1
        '
        Me.DataGridTextBoxColumn1.Format = ""
        Me.DataGridTextBoxColumn1.FormatInfo = Nothing
        Me.DataGridTextBoxColumn1.Width = 75
        '
        'ProviderTabPage
        '
        Me.ProviderTabPage.Controls.Add(Me.AssociatedProvidersDataGrid)
        Me.ProviderTabPage.Location = New System.Drawing.Point(4, 22)
        Me.ProviderTabPage.Name = "ProviderTabPage"
        Me.ProviderTabPage.Size = New System.Drawing.Size(536, 354)
        Me.ProviderTabPage.TabIndex = 0
        Me.ProviderTabPage.Text = "Associated Providers"
        '
        'AssociatedProvidersDataGrid
        '
        Me.AssociatedProvidersDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.AssociatedProvidersDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.AssociatedProvidersDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.AssociatedProvidersDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.AssociatedProvidersDataGrid.ADGroupsThatCanFind = ""
        Me.AssociatedProvidersDataGrid.ADGroupsThatCanMultiSort = ""
        Me.AssociatedProvidersDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.AssociatedProvidersDataGrid.AllowAutoSize = True
        Me.AssociatedProvidersDataGrid.AllowColumnReorder = True
        Me.AssociatedProvidersDataGrid.AllowCopy = True
        Me.AssociatedProvidersDataGrid.AllowCustomize = True
        Me.AssociatedProvidersDataGrid.AllowDelete = False
        Me.AssociatedProvidersDataGrid.AllowDragDrop = True
        Me.AssociatedProvidersDataGrid.AllowEdit = False
        Me.AssociatedProvidersDataGrid.AllowExport = True
        Me.AssociatedProvidersDataGrid.AllowFilter = True
        Me.AssociatedProvidersDataGrid.AllowFind = True
        Me.AssociatedProvidersDataGrid.AllowGoTo = True
        Me.AssociatedProvidersDataGrid.AllowMultiSelect = True
        Me.AssociatedProvidersDataGrid.AllowMultiSort = True
        Me.AssociatedProvidersDataGrid.AllowNew = False
        Me.AssociatedProvidersDataGrid.AllowPrint = False
        Me.AssociatedProvidersDataGrid.AllowRefresh = True
        Me.AssociatedProvidersDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AssociatedProvidersDataGrid.AppKey = "UFCW\Provider\"
        Me.AssociatedProvidersDataGrid.AutoSaveCols = True
        Me.AssociatedProvidersDataGrid.BackgroundColor = System.Drawing.SystemColors.Control
        Me.AssociatedProvidersDataGrid.CaptionText = "Associated Providers"
        Me.AssociatedProvidersDataGrid.ColumnHeaderLabel = Nothing
        Me.AssociatedProvidersDataGrid.ColumnRePositioning = False
        Me.AssociatedProvidersDataGrid.ColumnResizing = False
        Me.AssociatedProvidersDataGrid.ConfirmDelete = True
        Me.AssociatedProvidersDataGrid.CopySelectedOnly = True
        Me.AssociatedProvidersDataGrid.CurrentBSPosition = -1
        Me.AssociatedProvidersDataGrid.DataMember = ""
        Me.AssociatedProvidersDataGrid.DragColumn = 0
        Me.AssociatedProvidersDataGrid.ExportSelectedOnly = True
        Me.AssociatedProvidersDataGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.AssociatedProvidersDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.AssociatedProvidersDataGrid.HighlightedRow = Nothing
        Me.AssociatedProvidersDataGrid.HighLightModifiedRows = False
        Me.AssociatedProvidersDataGrid.IsMouseDown = False
        Me.AssociatedProvidersDataGrid.LastGoToLine = ""
        Me.AssociatedProvidersDataGrid.Location = New System.Drawing.Point(7, 13)
        Me.AssociatedProvidersDataGrid.MultiSort = False
        Me.AssociatedProvidersDataGrid.Name = "AssociatedProvidersDataGrid"
        Me.AssociatedProvidersDataGrid.OldSelectedRow = Nothing
        Me.AssociatedProvidersDataGrid.PreviousBSPosition = -1
        Me.AssociatedProvidersDataGrid.ReadOnly = True
        Me.AssociatedProvidersDataGrid.RetainRowSelectionAfterSort = True
        Me.AssociatedProvidersDataGrid.SetRowOnRightClick = True
        Me.AssociatedProvidersDataGrid.ShiftPressed = False
        Me.AssociatedProvidersDataGrid.SingleClickBooleanColumns = True
        Me.AssociatedProvidersDataGrid.Size = New System.Drawing.Size(522, 328)
        Me.AssociatedProvidersDataGrid.Sort = Nothing
        Me.AssociatedProvidersDataGrid.StyleName = ""
        Me.AssociatedProvidersDataGrid.SubKey = ""
        Me.AssociatedProvidersDataGrid.SuppressMouseDown = False
        Me.AssociatedProvidersDataGrid.SuppressTriangle = False
        Me.AssociatedProvidersDataGrid.TabIndex = 25
        Me.WorkToolTip.SetToolTip(Me.AssociatedProvidersDataGrid, "Associated Providers that roll up to a specific group provider")
        '
        'ProviderLicensesDS
        '
        Me.ProviderLicensesDS.DataSetName = "ProviderLicensesDS"
        Me.ProviderLicensesDS.EnforceConstraints = False
        Me.ProviderLicensesDS.Locale = New System.Globalization.CultureInfo("en-US")
        Me.ProviderLicensesDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'ProviderAssociatedSummaryDS
        '
        Me.ProviderAssociatedSummaryDS.DataSetName = "ProviderAssociatedSummaryDS"
        Me.ProviderAssociatedSummaryDS.Locale = New System.Globalization.CultureInfo("en-US")
        Me.ProviderAssociatedSummaryDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'AddressTypesDS
        '
        Me.AddressTypesDS.DataSetName = "AddressTypesDS"
        Me.AddressTypesDS.Locale = New System.Globalization.CultureInfo("en-US")
        Me.AddressTypesDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(77, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Provider Name"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProviderUpdateButton
        '
        Me.ProviderUpdateButton.Location = New System.Drawing.Point(448, 48)
        Me.ProviderUpdateButton.Name = "ProviderUpdateButton"
        Me.ProviderUpdateButton.Size = New System.Drawing.Size(72, 23)
        Me.ProviderUpdateButton.TabIndex = 3
        Me.ProviderUpdateButton.Text = "Update"
        Me.WorkToolTip.SetToolTip(Me.ProviderUpdateButton, "Updates existing Provider with any changes made below")
        '
        'AnnotateButton
        '
        Me.AnnotateButton.Location = New System.Drawing.Point(428, 168)
        Me.AnnotateButton.Name = "AnnotateButton"
        Me.AnnotateButton.Size = New System.Drawing.Size(72, 23)
        Me.AnnotateButton.TabIndex = 0
        Me.AnnotateButton.Text = "Annotate"
        '
        'DetailGridContextMenu
        '
        Me.DetailGridContextMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.UpdateMenuItem, Me.ClearMenuItem})
        '
        'UpdateMenuItem
        '
        Me.UpdateMenuItem.Index = 0
        Me.UpdateMenuItem.Text = "&Update All..."
        '
        'ClearMenuItem
        '
        Me.ClearMenuItem.Index = 1
        Me.ClearMenuItem.Text = "&Clear All..."
        '
        'ProviderIDTextBox
        '
        Me.ProviderIDTextBox.Location = New System.Drawing.Point(100, 36)
        Me.ProviderIDTextBox.MaxLength = 10
        Me.ProviderIDTextBox.Name = "ProviderIDTextBox"
        Me.ProviderIDTextBox.Size = New System.Drawing.Size(100, 20)
        Me.ProviderIDTextBox.TabIndex = 2
        Me.WorkToolTip.SetToolTip(Me.ProviderIDTextBox, "Enter either TAXID or SSN ")
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(8, 40)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(81, 13)
        Me.Label15.TabIndex = 10
        Me.Label15.Text = "Provider Tax ID"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProviderNameTextBox
        '
        Me.ProviderNameTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ProviderNameTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderAddressSummaryWithCommentsBS, "NAME1", True))
        Me.ProviderNameTextBox.Location = New System.Drawing.Point(100, 16)
        Me.ProviderNameTextBox.MaxLength = 60
        Me.ProviderNameTextBox.Name = "ProviderNameTextBox"
        Me.ProviderNameTextBox.Size = New System.Drawing.Size(309, 20)
        Me.ProviderNameTextBox.TabIndex = 1
        Me.WorkToolTip.SetToolTip(Me.ProviderNameTextBox, "Enter Complete Provider Title")
        '
        'ProviderNPITextBox
        '
        Me.ProviderNPITextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderAddressSummaryWithCommentsBS, "NPI", True))
        Me.ProviderNPITextBox.Location = New System.Drawing.Point(100, 56)
        Me.ProviderNPITextBox.MaxLength = 10
        Me.ProviderNPITextBox.Name = "ProviderNPITextBox"
        Me.ProviderNPITextBox.Size = New System.Drawing.Size(100, 20)
        Me.ProviderNPITextBox.TabIndex = 5
        Me.WorkToolTip.SetToolTip(Me.ProviderNPITextBox, "Provider 10 digit NPI#")
        '
        'ProviderTaxIDTypeListBox
        '
        Me.ProviderTaxIDTypeListBox.AllowDrop = True
        Me.ProviderTaxIDTypeListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ProviderTaxIDTypeListBox.Location = New System.Drawing.Point(204, 36)
        Me.ProviderTaxIDTypeListBox.Name = "ProviderTaxIDTypeListBox"
        Me.ProviderTaxIDTypeListBox.Size = New System.Drawing.Size(60, 21)
        Me.ProviderTaxIDTypeListBox.TabIndex = 3
        Me.WorkToolTip.SetToolTip(Me.ProviderTaxIDTypeListBox, "Identify TAXID as either SSN or TAXID")
        '
        'PPOCProviderCheckBox
        '
        Me.PPOCProviderCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.PPOCProviderCheckBox.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.ProviderAddressSummaryWithCommentsBS, "PPOC_ELIGIBLE_SW", True))
        Me.PPOCProviderCheckBox.Location = New System.Drawing.Point(272, 38)
        Me.PPOCProviderCheckBox.Name = "PPOCProviderCheckBox"
        Me.PPOCProviderCheckBox.Size = New System.Drawing.Size(123, 17)
        Me.PPOCProviderCheckBox.TabIndex = 4
        Me.PPOCProviderCheckBox.Text = "PPOC OK"
        Me.WorkToolTip.SetToolTip(Me.PPOCProviderCheckBox, "Check to permit PPOC")
        '
        'ProviderAlertTypesListBox
        '
        Me.ProviderAlertTypesListBox.DataSource = Me.AlertTypesDS
        Me.ProviderAlertTypesListBox.DisplayMember = "RETRIEVE_PROVIDER_ALERT_TYPES.DESCRIPTION"
        Me.ProviderAlertTypesListBox.DropDownWidth = 200
        Me.ProviderAlertTypesListBox.Location = New System.Drawing.Point(100, 76)
        Me.ProviderAlertTypesListBox.Name = "ProviderAlertTypesListBox"
        Me.ProviderAlertTypesListBox.Size = New System.Drawing.Size(164, 21)
        Me.ProviderAlertTypesListBox.TabIndex = 6
        Me.WorkToolTip.SetToolTip(Me.ProviderAlertTypesListBox, "Alert assigned to provider")
        Me.ProviderAlertTypesListBox.ValueMember = "RETRIEVE_PROVIDER_ALERT_TYPES.ALERT"
        '
        'AlertTypesDS
        '
        Me.AlertTypesDS.DataSetName = "AlertTypesDS"
        Me.AlertTypesDS.Locale = New System.Globalization.CultureInfo("en-US")
        Me.AlertTypesDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'SuspendedProviderCheckBox
        '
        Me.SuspendedProviderCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.SuspendedProviderCheckBox.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.ProviderAddressSummaryWithCommentsBS, "ProviderSuspended", True))
        Me.SuspendedProviderCheckBox.Location = New System.Drawing.Point(272, 57)
        Me.SuspendedProviderCheckBox.Name = "SuspendedProviderCheckBox"
        Me.SuspendedProviderCheckBox.Size = New System.Drawing.Size(123, 17)
        Me.SuspendedProviderCheckBox.TabIndex = 7
        Me.SuspendedProviderCheckBox.Text = "Suspended"
        Me.WorkToolTip.SetToolTip(Me.SuspendedProviderCheckBox, "Check to tag Provider as suspended")
        '
        'ProviderAddButton
        '
        Me.ProviderAddButton.Location = New System.Drawing.Point(448, 16)
        Me.ProviderAddButton.Name = "ProviderAddButton"
        Me.ProviderAddButton.Size = New System.Drawing.Size(72, 23)
        Me.ProviderAddButton.TabIndex = 2
        Me.ProviderAddButton.Text = "Add"
        Me.WorkToolTip.SetToolTip(Me.ProviderAddButton, "Creates new Provider record and associated addresses (Incomplete Addresses must b" &
        "e 'Suspended')")
        '
        'ExitButton
        '
        Me.ExitButton.CausesValidation = False
        Me.ExitButton.Location = New System.Drawing.Point(448, 80)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(72, 23)
        Me.ExitButton.TabIndex = 4
        Me.ExitButton.Text = "Exit"
        Me.WorkToolTip.SetToolTip(Me.ExitButton, "Exit to previous screen")
        '
        'SystemOnlyProviderCheckBox
        '
        Me.SystemOnlyProviderCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.SystemOnlyProviderCheckBox.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.ProviderAddressSummaryWithCommentsBS, "ELECTRONIC_USE_ONLY_SW", True))
        Me.SystemOnlyProviderCheckBox.Location = New System.Drawing.Point(272, 76)
        Me.SystemOnlyProviderCheckBox.Name = "SystemOnlyProviderCheckBox"
        Me.SystemOnlyProviderCheckBox.Size = New System.Drawing.Size(123, 17)
        Me.SystemOnlyProviderCheckBox.TabIndex = 80
        Me.SystemOnlyProviderCheckBox.Text = "System Only"
        Me.WorkToolTip.SetToolTip(Me.SystemOnlyProviderCheckBox, "Check to tag Provider as for system use only")
        '
        'TextBox1
        '
        Me.TextBox1.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.ProviderAddressSummaryWithCommentsBS, "ProviderTimestamp", True))
        Me.TextBox1.Location = New System.Drawing.Point(100, 97)
        Me.TextBox1.MaxLength = 10
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(118, 20)
        Me.TextBox1.TabIndex = 81
        Me.WorkToolTip.SetToolTip(Me.TextBox1, "Provider 10 digit NPI#")
        '
        'HRADisallowedProviderCheckBox
        '
        Me.HRADisallowedProviderCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.HRADisallowedProviderCheckBox.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Me.ProviderAddressSummaryWithCommentsBS, "HRA_INELIGIBLE_SW", True))
        Me.HRADisallowedProviderCheckBox.Location = New System.Drawing.Point(272, 95)
        Me.HRADisallowedProviderCheckBox.Name = "HRADisallowedProviderCheckBox"
        Me.HRADisallowedProviderCheckBox.Size = New System.Drawing.Size(123, 17)
        Me.HRADisallowedProviderCheckBox.TabIndex = 83
        Me.HRADisallowedProviderCheckBox.Text = "HRA Disallowed"
        Me.WorkToolTip.SetToolTip(Me.HRADisallowedProviderCheckBox, "Check to tag Provider as ineligible to receive HRA payments")
        '
        'DIList
        '
        Me.DIList.ImageStream = CType(resources.GetObject("DIList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.DIList.TransparentColor = System.Drawing.Color.Transparent
        Me.DIList.Images.SetKeyName(0, "")
        Me.DIList.Images.SetKeyName(1, "")
        Me.DIList.Images.SetKeyName(2, "")
        Me.DIList.Images.SetKeyName(3, "")
        '
        'AccumulatorsGroupBox
        '
        Me.AccumulatorsGroupBox.BackColor = System.Drawing.SystemColors.ControlLight
        Me.AccumulatorsGroupBox.Location = New System.Drawing.Point(8, 416)
        Me.AccumulatorsGroupBox.Name = "AccumulatorsGroupBox"
        Me.AccumulatorsGroupBox.Size = New System.Drawing.Size(524, 48)
        Me.AccumulatorsGroupBox.TabIndex = 48
        Me.AccumulatorsGroupBox.TabStop = False
        Me.AccumulatorsGroupBox.Text = "GroupBox3"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.HRADisallowedProviderCheckBox)
        Me.GroupBox1.Controls.Add(Me.Label14)
        Me.GroupBox1.Controls.Add(Me.TextBox1)
        Me.GroupBox1.Controls.Add(Me.SystemOnlyProviderCheckBox)
        Me.GroupBox1.Controls.Add(Me.ProviderNPITextBox)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.ProviderTaxIDTypeListBox)
        Me.GroupBox1.Controls.Add(Me.PPOCProviderCheckBox)
        Me.GroupBox1.Controls.Add(Me.ProviderAlertTypesListBox)
        Me.GroupBox1.Controls.Add(Me.SuspendedProviderCheckBox)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.ProviderNameTextBox)
        Me.GroupBox1.Controls.Add(Me.ProviderIDTextBox)
        Me.GroupBox1.Controls.Add(Me.Label15)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.AnnotateButton)
        Me.GroupBox1.Location = New System.Drawing.Point(4, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(428, 121)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Provider Information"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(9, 100)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(71, 13)
        Me.Label14.TabIndex = 82
        Me.Label14.Text = "Last Updated"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(8, 60)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(67, 13)
        Me.Label13.TabIndex = 79
        Me.Label13.Text = "Provider NPI"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(8, 80)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(28, 13)
        Me.Label5.TabIndex = 53
        Me.Label5.Text = "Alert"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProviderAddressesDS
        '
        Me.ProviderAddressesDS.DataSetName = "ProviderAddressesDS"
        Me.ProviderAddressesDS.Locale = New System.Globalization.CultureInfo("en-US")
        Me.ProviderAddressesDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'WorkStatusBar
        '
        Me.WorkStatusBar.Location = New System.Drawing.Point(0, 522)
        Me.WorkStatusBar.Name = "WorkStatusBar"
        Me.WorkStatusBar.Panels.AddRange(New System.Windows.Forms.StatusBarPanel() {Me.InfoStatusBarPanel, Me.DomainUserStatusBarPanel, Me.DataStatusBarPanel, Me.DateStatusBarPanel})
        Me.WorkStatusBar.ShowPanels = True
        Me.WorkStatusBar.Size = New System.Drawing.Size(546, 16)
        Me.WorkStatusBar.TabIndex = 101
        '
        'InfoStatusBarPanel
        '
        Me.InfoStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring
        Me.InfoStatusBarPanel.Name = "InfoStatusBarPanel"
        Me.InfoStatusBarPanel.Width = 499
        '
        'DomainUserStatusBarPanel
        '
        Me.DomainUserStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.DomainUserStatusBarPanel.Name = "DomainUserStatusBarPanel"
        Me.DomainUserStatusBarPanel.Width = 10
        '
        'DataStatusBarPanel
        '
        Me.DataStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.DataStatusBarPanel.Name = "DataStatusBarPanel"
        Me.DataStatusBarPanel.Width = 10
        '
        'DateStatusBarPanel
        '
        Me.DateStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.DateStatusBarPanel.Name = "DateStatusBarPanel"
        Me.DateStatusBarPanel.Width = 10
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'ProviderHistoryDS
        '
        Me.ProviderHistoryDS.DataSetName = "ProviderHistoryDS"
        Me.ProviderHistoryDS.Locale = New System.Globalization.CultureInfo("en-US")
        Me.ProviderHistoryDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'ProviderHeaderBS
        '
        Me.ProviderHeaderBS.DataMember = "ProviderAddressSummaryWithComments"
        Me.ProviderHeaderBS.DataSource = Me.ProviderAddressSummaryWithCommentsDS
        '
        'ProviderWork
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(546, 538)
        Me.Controls.Add(Me.WorkStatusBar)
        Me.Controls.Add(Me.ExitButton)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.AddressTabs)
        Me.Controls.Add(Me.ProviderAddButton)
        Me.Controls.Add(Me.ProviderUpdateButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Menu = Me.MainMenu1
        Me.Name = "ProviderWork"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Provider Maintainance"
        Me.AddressTabs.ResumeLayout(False)
        Me.AddressesTabPage.ResumeLayout(False)
        Me.AddressesTabPage.PerformLayout()
        Me.ProviderAddressGroupBox.ResumeLayout(False)
        Me.ProviderAddressGroupBox.PerformLayout()
        CType(Me.ProviderAddressSummaryWithCommentsBS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ProviderAddressSummaryWithCommentsDS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.StatesDV, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.StatesDS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PrimaryContactGroupBox.ResumeLayout(False)
        Me.PrimaryContactGroupBox.PerformLayout()
        Me.AlternativeContactGroupBox.ResumeLayout(False)
        Me.AlternativeContactGroupBox.PerformLayout()
        Me.LicensesTabPage.ResumeLayout(False)
        CType(Me.LicensesDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Comments.ResumeLayout(False)
        Me.History.ResumeLayout(False)
        CType(Me.ProviderHistoryDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ProviderTabPage.ResumeLayout(False)
        CType(Me.AssociatedProvidersDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ProviderLicensesDS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ProviderAssociatedSummaryDS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AddressTypesDS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AlertTypesDS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.ProviderAddressesDS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InfoStatusBarPanel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DomainUserStatusBarPanel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataStatusBarPanel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateStatusBarPanel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ProviderHistoryDS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ProviderHeaderBS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Properties"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' this property gets or sets a unique id suppilied by the calling form
    ''' </summary>
    ''' <value></value>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	3/24/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Property UniqueID() As String
        Get
            Return _UniqueID
        End Get
        Set(ByVal Value As String)
            _UniqueID = Value
        End Set
    End Property
#End Region

#Region "Form Events"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' performs main initialization of form
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[nick snyder]	8/16/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Event OpenProvider(ByVal sender As Object, ByVal e As ProviderEventArgs)
    Private Sub Work_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            'Init Active Directory
            AppDomain.CurrentDomain.SetPrincipalPolicy(Security.Principal.PrincipalPolicy.WindowsPrincipal)

            arBrushes(0) = New SolidBrush(Color.Blue)
            arBrushes(1) = New SolidBrush(Color.Green)
            arBrushes(2) = New SolidBrush(Color.Pink)

            SetSettings()

            ProviderTaxIDTypes.Add(New ListCategory("SSN", "S"))
            ProviderTaxIDTypes.Add(New ListCategory("TIN", "T"))

            LoadProviderAddressTypesList()
            LoadProviderLicenseTypesList()
            LoadProviderAlertTypesList()
            LoadStatesList()


            Select Case mProviderTaxID
                Case 0

                    Me.Text = "Process Provider (Add)"

                    Me.ProviderAddButton.Enabled = True
                    Me.ProviderUpdateButton.Enabled = False
                    Me.ProviderAddressGroupBox.Enabled = True

                    Me.ProviderAddressTypesListBox.Text = "MAIL"

                    Dim BlankDataRow As DataRow = ProviderAddressSummaryWithCommentsDS.ProviderAddressSummaryWithComments.NewRow

                    BlankDataRow("AddressType") = "MAIL"
                    BlankDataRow("ADDRESS_TYPE") = 1
                    BlankDataRow("PROVIDER_ID") = 0
                    'BlankDataRow("TAXID") = 
                    BlankDataRow("ALERT") = "$"
                    BlankDataRow("NAME1") = ""
                    BlankDataRow("NAME2") = ""
                    BlankDataRow("TAXID_TYPE") = "S"
                    BlankDataRow("STATE") = "  "

                    ProviderAddressSummaryWithCommentsDS.ProviderAddressSummaryWithComments.Rows.Add(BlankDataRow)


                Case Else

                    Me.Text = "Process Provider (Modify) - " & Format(mProviderTaxID, "000000000")

                    LoadProvider()

            End Select

            Me.ProviderAlertTypesListBox.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", ProviderAddressSummaryWithCommentsBS, "ALERT"))
            Me.ProviderAlertTypesListBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", ProviderAddressSummaryWithCommentsBS, "AlertDescription"))

            Me.ProviderTaxIDTypeListBox.DataSource = ProviderTaxIDTypes
            Me.ProviderTaxIDTypeListBox.DisplayMember = "ListDescription"
            Me.ProviderTaxIDTypeListBox.ValueMember = "ItemData"

            Me.ProviderTaxIDTypeListBox.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", ProviderAddressSummaryWithCommentsBS, "TAXID_TYPE"))

            '           Me.ProviderTaxIDTypeListBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", ProviderAddressSummaryWithCommentsBS, "TAXID_TYPE"))

            'These fields are subject to some sort of validation
            ErrorProvider.Controls.Add(ProviderNameTextBox, "Provider Name")
            ErrorProvider.Controls.Add(ProviderIDTextBox, "Provider Tax ID")
            ErrorProvider.Controls.Add(ProviderNPITextBox, "Provider NPI")
            ErrorProvider.Controls.Add(ProviderTaxIDTypeListBox, "Provider Tax Type")
            ErrorProvider.Controls.Add(ProviderAddress1TextBox, "Provider 'MAIL' Address")
            ErrorProvider.Controls.Add(ProviderCityTextBox, "Provider 'MAIL' City")
            ErrorProvider.Controls.Add(ProviderCountryTextBox, "Provider 'MAIL' Country")
            ErrorProvider.Controls.Add(ProviderStateListBox, "Provider 'MAIL' State")
            ErrorProvider.Controls.Add(ProviderZipTextBox, "Provider 'MAIL' Zip")
            ErrorProvider.Controls.Add(ProviderZip4TextBox, "Provider 'MAIL' ZIP+4 Code")
            ErrorProvider.Controls.Add(ProviderPrimaryContactTextBox, "Primary Contact Name")
            ErrorProvider.Controls.Add(ProviderPrimaryPhoneTextBox, "Primary Contact Number")
            ErrorProvider.Controls.Add(ProviderPrimaryPhoneExtTextBox, "Primary Contact Extension")
            ErrorProvider.Controls.Add(ProviderPrimaryEmailTextBox, "Primary Email Address")
            ErrorProvider.Controls.Add(ProviderAltContactTextBox, "Alternative Contact Name")
            ErrorProvider.Controls.Add(ProviderAltEmailTextBox, "Alternative Email Address")
            ErrorProvider.Controls.Add(ProviderAltPhoneTextBox, "Alternative Contact Number")
            ErrorProvider.Controls.Add(ProviderAltPhoneExtTextBox, "Alternative Contact Extension")

            'These fields are mandatory' unless satisfied via 
            ErrorProvider.Controls(ProviderIDTextBox).Required = True
            ErrorProvider.Controls(ProviderTaxIDTypeListBox).Required = True
            ErrorProvider.Controls(ProviderNameTextBox).Required = True
            ErrorProvider.Controls(ProviderAddress1TextBox).Required = True
            ErrorProvider.Controls(ProviderCityTextBox).Required = True

            ErrorProvider.Controls(ProviderStateListBox).Required = True
            ErrorProvider.Controls(ProviderStateListBox).Conditional = True
            ErrorProvider.Controls(ProviderStateListBox).Conditions = New String() {"ProviderCountryTextBox"} 'not required is Country is specified

            ErrorProvider.Controls(ProviderZipTextBox).Required = True
            ErrorProvider.Controls(ProviderZipTextBox).Conditional = True
            ErrorProvider.Controls(ProviderZipTextBox).Conditions = New String() {"ProviderCountryTextBox"} 'not required is Country is specified

            ProviderIDTextBox.DataBindings.Clear()

            Dim Bind As Binding = New Binding("Text", ProviderAddressSummaryWithCommentsBS, "TAXID", True)
            AddHandler Bind.Format, AddressOf ProviderIDTextBox_Format
            AddHandler Bind.Parse, AddressOf ProviderIDTextBox_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            ProviderIDTextBox.DataBindings.Add(Bind)

            AddHandler ProviderComments.DataBindings(0).Parse, AddressOf ReplaceCRLF_Parse
            AddHandler ProviderAddress2TextBox.DataBindings(0).Parse, AddressOf AllowNull_Parse
            AddHandler ProviderZipTextBox.DataBindings(0).Parse, AddressOf AllowNull_Parse
            AddHandler ProviderZip4TextBox.DataBindings(0).Parse, AddressOf AllowNull_Parse
            AddHandler ProviderPrimaryContactTextBox.DataBindings(0).Parse, AddressOf AllowNull_Parse
            AddHandler ProviderPrimaryPhoneTextBox.DataBindings(0).Parse, AddressOf AllowNull_Parse
            AddHandler ProviderPrimaryPhoneExtTextBox.DataBindings(0).Parse, AddressOf AllowNull_Parse
            AddHandler ProviderPrimaryEmailTextBox.DataBindings(0).Parse, AddressOf AllowNull_Parse
            AddHandler ProviderAltContactTextBox.DataBindings(0).Parse, AddressOf AllowNull_Parse
            AddHandler ProviderAltPhoneTextBox.DataBindings(0).Parse, AddressOf AllowNull_Parse
            AddHandler ProviderAltPhoneExtTextBox.DataBindings(0).Parse, AddressOf AllowNull_Parse
            AddHandler ProviderAltEmailTextBox.DataBindings(0).Parse, AddressOf AllowNull_Parse

            'Set summary error message
            ErrorProvider.MandatoryMessage = "Highlighted fields are mandatory,"
            ErrorProvider.ConditionalMessage = "A valid address must be supplied: Enter either a Valid U.S address, or provide a Country,"

            DomainUserStatusBarPanel.Text = SystemInformation.UserName
            DataStatusBarPanel.Text = "Server=" & CMSDALCommon.GetServerName(Nothing) & ";DB=" & CMSDALCommon.GetDatabaseName(Nothing)
            DateStatusBarPanel.Text = Format(Now, "MM-dd-yyyy")

            If UFCWGeneralAD.CMSCanModifyProvider OrElse UFCWGeneralAD.CMSCanAddProvider OrElse UFCWGeneralAD.CMSAdministrators Then
                'all functions available
            Else
                'read only access
                ProviderAddButton.Enabled = False
                ProviderUpdateButton.Enabled = False
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' when closing, the item needs to be released by setting the busy_sw = 0
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	3/24/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub Work_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Try

            SaveSettings()

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Syncs the image with this claim if an image exists
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	4/27/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub Work_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        If Activating = True Then Exit Sub

        Try
            Activating = True

            Me.WindowState = FormWindowState.Normal

            Me.Refresh()

            Activating = False

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Sets the basic form settings.  Windowstate, height, width, top, and left.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	11/16/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub SetSettings()
        Dim FName As String = ""
        Dim FSize As Single
        Dim FStyle As New FontStyle
        Dim FUnit As New GraphicsUnit
        Dim FCharset As Byte

        Me.Visible = False

        Me.Top = CInt(GetSetting(AppKey, "\Settings", "Top", CStr(Me.Top)))
        Me.Left = CInt(GetSetting(AppKey, "\Settings", "Left", CStr(Me.Left)))
        Me.WindowState = CType(GetSetting(AppKey, "\Settings", "WindowState", CStr(Me.WindowState)), FormWindowState)

        FName = GetSetting(AppKey, "\Settings", "FontName", Me.Font.Name)
        FSize = CSng(GetSetting(AppKey, "\Settings", "FontSize", CStr(Me.Font.Size)))
        FStyle = CType(GetSetting(AppKey, "\Settings", "FontStyle", CStr(Me.Font.Style)), FontStyle)
        FUnit = CType(GetSetting(AppKey, "\Settings", "FontUnit", CStr(Me.Font.Unit)), GraphicsUnit)
        FCharset = CByte(GetSetting(AppKey, "\Settings", "FontCharset", CStr(Me.Font.GdiCharSet)))

        Me.Font = New Font(FName, FSize, FStyle, FUnit, FCharset)

    End Sub
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Saves the basic form settings.  Windowstate, height, width, top, and left.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	11/16/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub SaveSettings()
        Dim lWindowState As Integer = Me.WindowState

        SaveSetting(AppKey, "\Settings", "WindowState", CStr(lWindowState))
        Me.WindowState = 0
        SaveSetting(AppKey, "\Settings", "Top", CStr(Me.Top))
        SaveSetting(AppKey, "\Settings", "Height", CStr(Me.Height))
        SaveSetting(AppKey, "\Settings", "Left", CStr(Me.Left))
        SaveSetting(AppKey, "\Settings", "Width", CStr(Me.Width))

        SaveSetting(AppKey, "\Settings", "FontName", Me.Font.Name)
        SaveSetting(AppKey, "\Settings", "FontSize", CStr(Me.Font.Size))
        SaveSetting(AppKey, "\Settings", "FontStyle", CStr(Me.Font.Style))
        SaveSetting(AppKey, "\Settings", "FontUnit", CStr(Me.Font.Unit))
        SaveSetting(AppKey, "\Settings", "FontCharset", CStr(Me.Font.GdiCharSet))

    End Sub
    Private Sub AddressTabs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddressTabs.Click

        SaveSettings()

        If AddressTabs.SelectedIndex <> 0 Then 'if moving from address tab validate address information

            If ProviderAddressSummaryWithCommentsBS.Count > 0 Then

                If AddressModifyButton.Text = "Remove Address" AndAlso ErrorProvider.CheckRequiredAndShowSummaryErrorMessage() = False Then 'Current address failed validation
                    AddressTabs.SelectedIndex() = 0 'restrict to address tab until address is validated
                End If
            End If
        End If

    End Sub

#End Region

#Region "Menu\Button (Non Changing) Events"

    Private Sub RefreshMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshMenuItem.Click
        Try
            LoadProvider()

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                'throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub ExitButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ExitButton.Click, MenuExit.Click

        ProviderAddressSummaryWithCommentsBS.EndEdit()
        ProviderLicensesBS.EndEdit()

        If ProviderAddressSummaryWithCommentsDS.HasChanges OrElse ProviderLicensesDS.HasChanges Then
            If MsgBox("Changes have not been saved to the database. Exit without saving changes?", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkCancel, MsgBoxStyle), "Last chance") = MsgBoxResult.Ok Then
                Me.Close()
            End If
        Else
            Me.Close()
        End If

    End Sub

#End Region

#Region "Menu\Button (Changing) Events"

    Private Sub AddressModifyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddressModifyButton.Click

        Dim ListCategory As ListCategory

        Try

            ListCategory = CType(Me.ProviderAddressTypesListBox.SelectedItem, ListCategory)

            Select Case Me.AddressModifyButton.Text
                Case "Remove Address"
                    Dim ProviderAddressDr As DataRow() = ProviderAddressSummaryWithCommentsDS.ProviderAddressSummaryWithComments.Select("ADDRESS_TYPE = " & CType(Me.ProviderAddressTypesListBox.SelectedItem, ListCategory).ItemData.ToString)
                    ProviderAddressDr(0).Delete()

                    ProviderAddressSummaryWithCommentsBS.Filter() = "AddressType='MAIL'"

                    Me.ProviderAddressTypesListBox.Text = "MAIL"

                    Me.AddressModifyButton.Text = "Enable Address"

                Case "Enable Address"

                    Dim BlankDataRow As DataRow = ProviderAddressSummaryWithCommentsDS.ProviderAddressSummaryWithComments.NewRow

                    BlankDataRow("AddressType") = ListCategory.ListDescription
                    BlankDataRow("ADDRESS_TYPE") = ListCategory.ItemData
                    BlankDataRow("PROVIDER_ID") = ProviderKey.ProviderID
                    BlankDataRow("TAXID") = ProviderKey.ProviderTaxID
                    BlankDataRow("NAME1") = ProviderKey.Name
                    BlankDataRow("TAXID_TYPE") = ProviderKey.ProviderTaxType
                    BlankDataRow("STATE") = "  "

                    ProviderAddressSummaryWithCommentsDS.ProviderAddressSummaryWithComments.Rows.Add(BlankDataRow)

                    Me.AddressModifyButton.Text = "Remove Address"

            End Select

            Me.ProviderAddressGroupBox.Enabled = True
            InfoStatusBarPanel.Text = "Press 'Update' to complete change(s)."

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub ProviderUpdateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProviderUpdateButton.Click, ProviderAddButton.Click

        Dim IgnoreDuplicates As Boolean = True
        If Not ProviderUpdateButton.Enabled = True Then   ''' New Provider
            Dim ResultSet As New DataSet
            Try
                If Len(Trim(ProviderIDTextBox.Text)) > 0 Then
                    ResultSet = ProviderDAL.RetrieveProvidersByTAXID(CInt(ProviderIDTextBox.Text))
                    If ResultSet.Tables(0).Rows.Count > 0 Then
                        MessageBox.Show("Provider TaxID is already in Use with" & Environment.NewLine & "the Provider " & CType(ResultSet.Tables(0).Rows(0)("NAME1"), String) & Environment.NewLine & "at " & CType(ResultSet.Tables(0).Rows(0)("CompositeMailAddress"), String), "Duplicate Provider", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    End If
                End If
            Catch ex As Exception
                Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
                If (rethrow) Then
                    Throw
                End If
            Finally
                ResultSet.Dispose()
                ResultSet = Nothing
            End Try
        End If

        If SuspendedProviderAddressCheckBox.CheckState = CheckState.Unchecked AndAlso ErrorProvider.CheckRequiredAndShowSummaryErrorMessage() = False Then
        Else

            ProviderAddressSummaryWithCommentsBS.EndEdit()
            ProviderLicensesBS.EndEdit()

            Dim CurrentTransaction As DbTransaction

            Dim i As Integer
            Dim ProviderHeaderRow As DataRow = CType(ProviderAddressSummaryWithCommentsBS.Current, DataRowView).Row

            If IsDate(ProviderHeaderRow("ProviderTimestamp")) = False AndAlso ProviderDAL.RetrieveProviderCount(mProviderTaxID, UFCWGeneral.IsNullIntegerHandler(ProviderHeaderRow("TAXID")), UFCWGeneral.IsNullDecimalHandler(ProviderHeaderRow("NPI"))) > 0 Then
                If MsgBox("Possible duplicates found, do you want to continue?", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkCancel, MsgBoxStyle), "Last chance") = MsgBoxResult.Ok Then
                    IgnoreDuplicates = True
                Else
                    IgnoreDuplicates = False

                    InfoStatusBarPanel.Text = "Action cancelled."

                End If
            End If

            If IgnoreDuplicates = True Then
                Try
                    Me.Cursor = Cursors.WaitCursor

                    CurrentTransaction = CMSDALCommon.BeginTransaction()

                    ProviderLicensesBS.EndEdit()

                    Dim LicenseChangesDT As DataTable = ProviderLicensesDS.ProviderLicenses.GetChanges

                    If ProviderHeaderRow.RowState = DataRowState.Added AndAlso Not IsDBNull(ProviderHeaderRow("COMMENT_TEXT", DataRowVersion.Current)) Then
                        ProviderHeaderRow("COMMENT_ID") = ProviderDAL.ModifyProviderComments(TryCast(ProviderHeaderRow("COMMENT_TEXT"), String), CurrentTransaction)

                        ProviderDAL.ModifyProviderHistory(mProviderTaxID, LogAction.ProviderCommentsUpdate, "COMMENT_ID = " & ProviderHeaderRow("COMMENT_ID").ToString, CurrentTransaction)

                    Else
                        If ProviderHeaderRow.RowState <> DataRowState.Added AndAlso Not IsDBNull(ProviderHeaderRow("COMMENT_TEXT", DataRowVersion.Current)) _
                        AndAlso ProviderHeaderRow("COMMENT_TEXT", DataRowVersion.Current).ToString <> CStr(IIf(IsDBNull(ProviderHeaderRow("COMMENT_TEXT", DataRowVersion.Original)), " ", ProviderHeaderRow("COMMENT_TEXT", DataRowVersion.Original).ToString)) Then
                            ProviderHeaderRow("COMMENT_ID") = ProviderDAL.ModifyProviderComments(TryCast(ProviderHeaderRow("COMMENT_TEXT"), String), CurrentTransaction)

                            ProviderDAL.ModifyProviderHistory(mProviderTaxID, LogAction.ProviderCommentsUpdate, "COMMENT_ID = " & ProviderHeaderRow("COMMENT_ID").ToString, CurrentTransaction)

                        End If
                    End If

                    ProviderDAL.ModifyProvider(mProviderTaxID, UFCWGeneral.IsNullDateHandler(ProviderHeaderRow("ProviderTimestamp")), CInt(ProviderHeaderRow("TAXID")),
                        TryCast(ProviderHeaderRow("TAXID_TYPE"), String), CInt(ProviderHeaderRow("ProviderSuspended")),
                        Math.Abs(CInt(ProviderHeaderRow("PPOC_ELIGIBLE_SW"))), Math.Abs(CInt(ProviderHeaderRow("HRA_INELIGIBLE_SW"))), Math.Abs(CInt(ProviderHeaderRow("ELECTRONIC_USE_ONLY_SW"))), UFCWGeneral.IsNullDecimalHandler(ProviderHeaderRow("NPI")), UFCWGeneral.IsNullIntegerHandler(ProviderHeaderRow("COMMENT_ID")), TryCast(ProviderHeaderRow("ALERT"), String),
                        TryCast(ProviderHeaderRow("NAME1"), String), TryCast(ProviderHeaderRow("NAME2"), String), CurrentTransaction)

                    If ProviderHeaderRow.RowState <> DataRowState.Added Then
                        If ProviderHeaderRow("TAXID_TYPE", DataRowVersion.Current).ToString <> ProviderHeaderRow("TAXID_TYPE", DataRowVersion.Original).ToString OrElse ProviderHeaderRow("TAXID", DataRowVersion.Current).ToString <> ProviderHeaderRow("TAXID", DataRowVersion.Original).ToString Then
                            ProviderDAL.ModifyProviderHistory(mProviderTaxID, LogAction.ProviderTINSSNUpdate, "TAXID Changed " & ProviderHeaderRow("TAXID", DataRowVersion.Original).ToString & "-" & ProviderHeaderRow("TAXID", DataRowVersion.Current).ToString, CurrentTransaction)
                        End If
                    End If

                    ProviderDAL.ModifyProviderHistory(mProviderTaxID, CInt(IIf(IsDate(ProviderHeaderRow("ProviderTimestamp")), LogAction.ProviderHeaderUpdate, LogAction.ProviderHeaderAdd)), "", CurrentTransaction)

                    If LicenseChangesDT IsNot Nothing Then
                        For i = 0 To LicenseChangesDT.Rows.Count - 1
                            Dim LicenseDeletedDR As DataRow = LicenseChangesDT.Rows.Item(i)

                            Select Case LicenseChangesDT.Rows.Item(i).RowState
                                Case DataRowState.Deleted
                                    ProviderDAL.DeleteProviderLicense(CInt(LicenseDeletedDR("LICENSE_ID", DataRowVersion.Original)), UFCWGeneral.IsNullDateHandler(LicenseDeletedDR("LicenseTimestamp", DataRowVersion.Original)), CurrentTransaction)

                                    ProviderDAL.ModifyProviderHistory(mProviderTaxID, LogAction.ProviderLicenseDeleted, "LICENSE: " & LicenseDeletedDR("LICENSE_ID", DataRowVersion.Original).ToString() & " ID: " & LicenseDeletedDR("LICENSE", DataRowVersion.Original).ToString(), CurrentTransaction)

                                Case Else
                                    ProviderDAL.ModifyProviderLicense(mProviderTaxID, UFCWGeneral.IsNullDateHandler(LicenseChangesDT.Rows.Item(i)("LicenseTimestamp")), LicenseChangesDT.Rows.Item(i)("LICENSE").ToString,
                                             LicenseChangesDT.Rows.Item(i)("LICENSE_DESCRIPTION").ToString, LicenseChangesDT.Rows.Item(i)("LICENSE_ID").ToString, CurrentTransaction)
                                    ProviderDAL.ModifyProviderHistory(mProviderTaxID, LogAction.ProviderLicenseAdd, "LICENSE ID: " & LicenseChangesDT.Rows.Item(i)("LICENSE").ToString(), CurrentTransaction)
                            End Select

                        Next
                    End If

                    Dim OriginalDT As DataTable = ProviderAddressSummaryWithCommentsDS.ProviderAddressSummaryWithComments
                    Dim AddressChangesDT As DataTable = ProviderAddressSummaryWithCommentsDS.ProviderAddressSummaryWithComments.GetChanges

                    If AddressChangesDT IsNot Nothing Then
                        For i = 0 To AddressChangesDT.Rows.Count - 1
                            Dim ColumnsModified As Integer = 0
                            Dim iColumn As Integer = 0
                            Dim AddressRow As DataRow = AddressChangesDT.Rows.Item(i)

                            Select Case AddressChangesDT.Rows.Item(i).RowState
                                Case DataRowState.Deleted

                                    ProviderDAL.DeleteProviderAddress(mProviderTaxID, CInt(AddressRow("ADDRESS_TYPE", DataRowVersion.Original)), UFCWGeneral.IsNullDateHandler(AddressRow("AddressTimestamp", DataRowVersion.Original)), CurrentTransaction)
                                    ProviderDAL.ModifyProviderHistory(mProviderTaxID, LogAction.ProviderAddressDeleted, AddressRow("AddressType", DataRowVersion.Original).ToString(), CurrentTransaction)

                                Case DataRowState.Added

                                    ProviderDAL.ModifyProviderAddress(mProviderTaxID,
                                        UFCWGeneral.IsNullDateHandler(AddressChangesDT.Rows.Item(i)("AddressTimestamp")), CInt(AddressChangesDT.Rows.Item(i)("ADDRESS_TYPE")), CInt(AddressChangesDT.Rows.Item(i)("AddressSuspended")),
                                        TryCast(AddressChangesDT.Rows.Item(i)("ADDRESS_LINE1"), String), TryCast(AddressChangesDT.Rows.Item(i)("ADDRESS_LINE2"), String),
                                        TryCast(AddressChangesDT.Rows.Item(i)("CITY"), String), TryCast(AddressChangesDT.Rows.Item(i)("COUNTRY"), String), TryCast(AddressChangesDT.Rows.Item(i)("STATE"), String), CDec(AddressChangesDT.Rows.Item(i)("ZIP")), UFCWGeneral.IsNullDecimalHandler(AddressChangesDT.Rows.Item(i)("ZIP_4")),
                                        TryCast(AddressChangesDT.Rows.Item(i)("EMAIL1"), String), UFCWGeneral.IsNullDecimalHandler(AddressChangesDT.Rows.Item(i)("PHONE1")), UFCWGeneral.IsNullIntegerHandler(AddressChangesDT.Rows.Item(i)("EXTENSION1")), TryCast(AddressChangesDT.Rows.Item(i)("CONTACT1"), String),
                                        TryCast(AddressChangesDT.Rows.Item(i)("EMAIL2"), String), UFCWGeneral.IsNullDecimalHandler(AddressChangesDT.Rows.Item(i)("PHONE2")), UFCWGeneral.IsNullIntegerHandler(AddressChangesDT.Rows.Item(i)("EXTENSION2")), TryCast(AddressChangesDT.Rows.Item(i)("CONTACT2"), String), CurrentTransaction)


                                    ProviderDAL.ModifyProviderHistory(mProviderTaxID, CInt(IIf(IsDate(AddressChangesDT.Rows.Item(i)("AddressTimestamp")), LogAction.ProviderAddressUpdate, LogAction.ProviderAddressAdd)), CStr(AddressChangesDT.Rows.Item(i)("AddressType")), CurrentTransaction)

                                Case Else

                                    For iColumn = 0 To AddressChangesDT.Columns.Count - 1
                                        Try
                                            If AddressRow(iColumn, DataRowVersion.Current).ToString() <> AddressRow(iColumn, DataRowVersion.Original).ToString() Then 'These are all header items
                                                If AddressRow.Table.Columns.Item(iColumn).ColumnName <> "COMMENT_TEXT" AndAlso
                                                    AddressRow.Table.Columns.Item(iColumn).ColumnName <> "TAXID" AndAlso
                                                    AddressRow.Table.Columns.Item(iColumn).ColumnName <> "TAXID_TYPE" AndAlso
                                                    AddressRow.Table.Columns.Item(iColumn).ColumnName <> "ProviderSuspended" AndAlso
                                                    AddressRow.Table.Columns.Item(iColumn).ColumnName <> "PPOC_ELIGIBLE_SW" AndAlso
                                                    AddressRow.Table.Columns.Item(iColumn).ColumnName <> "HRA_INELIGIBLE_SW" AndAlso
                                                    AddressRow.Table.Columns.Item(iColumn).ColumnName <> "NPI" AndAlso
                                                    AddressRow.Table.Columns.Item(iColumn).ColumnName <> "ALERT" AndAlso
                                                    AddressRow.Table.Columns.Item(iColumn).ColumnName <> "NAME1" AndAlso
                                                    AddressRow.Table.Columns.Item(iColumn).ColumnName <> "NAME2" Then 'Note Comments are recorded as a different type of change and should not be reflected as address changes

                                                    ColumnsModified += 1
                                                End If

                                            End If

                                        Catch
                                        End Try

                                    Next

                                    If ColumnsModified > 0 Then

                                        ProviderDAL.ModifyProviderAddress(mProviderTaxID,
                                            UFCWGeneral.IsNullDateHandler(AddressChangesDT.Rows.Item(i)("AddressTimestamp")), CInt(AddressChangesDT.Rows.Item(i)("ADDRESS_TYPE")), CInt(AddressChangesDT.Rows.Item(i)("AddressSuspended")),
                                            TryCast(AddressChangesDT.Rows.Item(i)("ADDRESS_LINE1"), String), TryCast(AddressChangesDT.Rows.Item(i)("ADDRESS_LINE2"), String),
                                            TryCast(AddressChangesDT.Rows.Item(i)("CITY"), String), TryCast(AddressChangesDT.Rows.Item(i)("COUNTRY"), String), TryCast(AddressChangesDT.Rows.Item(i)("STATE"), String), CDec(AddressChangesDT.Rows.Item(i)("ZIP")), UFCWGeneral.IsNullDecimalHandler(AddressChangesDT.Rows.Item(i)("ZIP_4")),
                                            TryCast(AddressChangesDT.Rows.Item(i)("EMAIL1"), String), UFCWGeneral.IsNullDecimalHandler(AddressChangesDT.Rows.Item(i)("PHONE1")), UFCWGeneral.IsNullIntegerHandler(AddressChangesDT.Rows.Item(i)("EXTENSION1")), TryCast(AddressChangesDT.Rows.Item(i)("CONTACT1"), String),
                                            TryCast(AddressChangesDT.Rows.Item(i)("EMAIL2"), String), UFCWGeneral.IsNullDecimalHandler(AddressChangesDT.Rows.Item(i)("PHONE2")), UFCWGeneral.IsNullIntegerHandler(AddressChangesDT.Rows.Item(i)("EXTENSION2")), TryCast(AddressChangesDT.Rows.Item(i)("CONTACT2"), String), CurrentTransaction)

                                        ProviderDAL.ModifyProviderHistory(mProviderTaxID, CInt(IIf(IsDate(AddressChangesDT.Rows.Item(i)("AddressTimestamp")), LogAction.ProviderAddressUpdate, LogAction.ProviderAddressAdd)), CStr(AddressChangesDT.Rows.Item(i)("AddressType")), CurrentTransaction)

                                    End If

                            End Select

                        Next
                    End If

                    CMSDALCommon.CommitTransaction(CurrentTransaction)

                    LoadProvider()

                    ProviderAddressSummaryWithCommentsBS.Filter() = "AddressType='MAIL'"

                    Me.Text = "Process Provider (Modify) - " & Format(mProviderTaxID, "000000000")

                    Me.Refresh()

                    InfoStatusBarPanel.Text = "Action completed successfully."

                Catch ex As DB2Exception

                    Try
                        CMSDALCommon.RollbackTransaction(CurrentTransaction) 'note this will have no effect if commital occured
                    Catch
                    End Try

                    Select Case ex.Number
                        Case -803
                            ex.HelpLink = "Action failed due to duplicate entry "
                        Case -438 'note this code represents a user raised error
                            ex.HelpLink = "Action failed due to missing entry "
                    End Select

                    If ex.HelpLink Is Nothing Then
                        InfoStatusBarPanel.Text = "Action Failed."
                        Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
                        If (rethrow) Then
                            MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If

                    Else
                        InfoStatusBarPanel.Text = ex.HelpLink
                        Beep()

                        MsgBox(InfoStatusBarPanel.Text, CType(MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, MsgBoxStyle), "Duplicate")
                    End If

                Catch ex As Exception

                    CMSDALCommon.RollbackTransaction(CurrentTransaction) 'note this will have no effect if commital occured

                    Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
                    If (rethrow) Then
                        MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If

                    InfoStatusBarPanel.Text = "Action failed."

                Finally

                    Me.Cursor = Cursors.Default

                End Try
            End If
        End If
    End Sub

#End Region

#Region "Load Combo\List Boxes"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Loads the available Alert Types to select
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	4/20/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub LoadProviderAlertTypesList()
        Try
            ProviderAlertTypesDT = ProviderDAL.RetrieveProviderAlertTypes

            ProviderAlertTypesListBox.SuspendLayout()

            ProviderAlertTypesListBox.DataSource = Nothing

            ProviderAlertTypesListBox.DataSource = ProviderAlertTypesDT
            ProviderAlertTypesListBox.ValueMember = "ALERT"
            ProviderAlertTypesListBox.DisplayMember = "DESCRIPTION"
            ProviderAlertTypesListBox.PerformLayout()


        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        Finally

        End Try
    End Sub


    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Loads the available Address Types to select
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	4/20/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub LoadStatesList()
        Try
            Dim DT As DataTable
            DT = ProviderDAL.RetrieveStates

            Dim BlankStateRow As DataRow = DT.NewRow

            BlankStateRow("ABBRV") = "  "
            BlankStateRow("STATE") = "  "

            DT.Rows.Add(BlankStateRow)

            ProviderStateListBox.BeginUpdate()

            ProviderStateListBox.DataSource = DT
            ProviderStateListBox.ValueMember = "ABBRV"
            ProviderStateListBox.DisplayMember = "ABBRV"

            ProviderAddressStatesDT = DT

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If

        Finally

            ProviderStateListBox.EndUpdate()

        End Try
    End Sub
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Loads the available Address Types to select
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	4/20/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub LoadProviderAddressTypesList()
        Try
            Dim DT As DataTable
            DT = ProviderDAL.RetrieveProviderAddressTypes

            ProviderAddressTypesListBox.DataSource = Nothing
            ProviderAddressTypesListBox.Items.Clear()

            ProviderAddressTypesListBox.BeginUpdate()

            For cnt As Integer = 0 To DT.Rows.Count - 1
                ProviderAddressTypesListBox.Items.Add(New ListCategory(CStr(DT.Rows(cnt)("NAME")), DT.Rows(cnt)("ADDRESS_TYPE")))
            Next

            ProviderAddressTypesDT = DT

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If

        Finally

            ProviderAddressTypesListBox.EndUpdate()

        End Try
    End Sub
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Loads the available plans to select
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	4/20/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub LoadProviderLicenseTypesList()
        Try
            Dim DT As DataTable
            DT = ProviderDAL.RetrieveProviderLicenseTypes
            Dim R As DataRow = DT.NewRow

            ProviderLicenseTypesDT = DT

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub
#End Region

#Region "Load Provider and List Data"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Loads all data for the entire Claim
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	5/15/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub LoadProvider()

        Try
            Me.Cursor = Cursors.WaitCursor

            ProviderAddressSummaryWithCommentsDS.ProviderAddressSummaryWithComments.Rows.Clear()
            ProviderAddressSummaryWithCommentsBS.Filter() = ""

            ProviderAddressSummaryWithCommentsDS = CType(ProviderDAL.RetrieveProviderAddressSummaryWithComments(mProviderTaxID, False, ProviderAddressSummaryWithCommentsDS), ProviderAddressSummaryWithCommentsDS)

            ProviderAddressSummaryWithCommentsBS = New BindingSource
            ProviderAddressSummaryWithCommentsBS.DataSource = ProviderAddressSummaryWithCommentsDS.Tables("PROVIDERADDRESSSUMMARYWITHCOMMENTS")

            If ProviderAddressSummaryWithCommentsDS.Tables("PROVIDERADDRESSSUMMARYWITHCOMMENTS").Rows.Count > 0 Then

                ProviderAddressSummaryWithCommentsBS.Filter = "AddressType='MAIL'"

                ProviderDataRow = ProviderAddressSummaryWithCommentsDS.ProviderAddressSummaryWithComments.DefaultView(0).Row
                ProviderKey.ProviderID = CInt(ProviderDataRow("PROVIDER_ID"))
                ProviderKey.ProviderTaxID = CInt(ProviderDataRow("TaxID"))
                ProviderKey.ProviderTaxType = CStr(ProviderDataRow("TaxID_Type"))
                ProviderKey.Name = CStr(ProviderDataRow("Name1"))
                ProviderKey.LastUpdatedOn = CDate(ProviderDataRow("ProviderTimestamp"))

                'ProviderTaxIDTypeListBox.Text = IIf(ProviderDataRow("TaxID_Type") = "S", "SSN", "TIN")

                Me.ProviderAddButton.Enabled = False
                Me.ProviderUpdateButton.Enabled = True
                Me.ProviderAddressTypesListBox.Text = "MAIL"

            Else
                Me.ProviderUpdateButton.Enabled = False
                Me.ProviderAddButton.Enabled = True
            End If

            LoadProviderHistory()
            LoadProviderAssociated()
            LoadProviderLicenses()

        Catch ex As Exception

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub ProviderIDTextBox_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ProviderIDTextBox.Validating

        Try

            If IsDBNull(CType(sender, TextBox).Text) OrElse Not IsNumeric(CType(sender, TextBox).Text) Then
                e.Cancel = True
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            Else
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub BindingCompleteEventHandler(ByVal sender As Object, ByVal e As System.Windows.Forms.BindingCompleteEventArgs)

        Try

            If Not e.BindingCompleteState = BindingCompleteState.Success Then
                Debug.Print("Control " & e.Binding.Control.Name & " " & e.ErrorText, "Problem converting data to database format", MessageBoxButtons.OK, MessageBoxIcon.Error)
                MessageBox.Show("TaxID is required and must be numeric.", "Invalid TaxID", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            Else
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try

    End Sub

    Private Sub ProviderIDTextBox_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try

            If IsDBNull(e.Value) = False AndAlso IsNumeric(e.Value) = True Then
                e.Value = String.Format("{0:000000000}", CDec(e.Value))
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            Else
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub ProviderIDTextBox_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            If IsDBNull(e.Value) = False AndAlso IsNumeric(e.Value) = True Then
                e.Value = String.Format("{0:000000000}", CDec(e.Value))
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            Else
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Loads the licenses associated to the provider
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	4/20/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub LoadProviderLicenses()

        Try

            ProviderLicensesDS.ProviderLicenses.Rows.Clear()

            ProviderLicensesDS = CType(ProviderDAL.RetrieveProviderLicenses(mProviderTaxID, ProviderLicensesDS), LicensesDS)

            ProviderLicensesBS = New BindingSource
            ProviderLicensesBS.DataSource = ProviderLicensesDS.Tables("PROVIDERLICENSES")
            ProviderLicensesBS.Sort = "PROVIDER_ID, LICENSE"

            LicensesDataGrid.DataSource = ProviderLicensesBS 'Empty Grid needed in support of initial license add
            LicensesDataGrid.SetTableStyle()

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Loads the licenses associated to the provider
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	4/20/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub LoadProviderAssociated()
        Try

            ProviderAssociatedSummaryDS.ProviderAssociatedSummary.Rows.Clear()

            ProviderAssociatedSummaryDS = CType(ProviderDAL.RetrieveAssociatedProviders(mProviderTaxID, ProviderAssociatedSummaryDS), ProviderAssociatedSummaryDS)

            ProviderAssociatedSummaryBS = New BindingSource
            ProviderAssociatedSummaryBS.DataSource = ProviderAssociatedSummaryDS.Tables("ProviderAssociatedSummary")

            AssociatedProvidersDataGrid.DataSource = ProviderAssociatedSummaryBS
            AssociatedProvidersDataGrid.SetTableStyle()

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub
    Private Sub LoadProviderHistory()
        Try

            ProviderHistoryDS.ProviderHistory.Rows.Clear()

            ProviderHistoryDS = CType(ProviderDAL.RetrieveProviderHistory(mProviderTaxID, ProviderHistoryDS), ProviderHistoryDS)

            ProviderHistoryBS = New BindingSource
            ProviderHistoryBS.DataSource = ProviderHistoryDS.Tables("ProviderHistory")

            ProviderHistoryDataGrid.DataSource = ProviderHistoryBS 'ProviderHistoryDS.Tables.Item("ProviderHistory")
            ProviderHistoryDataGrid.SetTableStyle()

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' adjusts SSN values entered for a databinding
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[nick snyder]	8/16/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------

#End Region

#Region "Custom Formatting/Validation Functions"

    Private Sub SSNBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            If IsDBNull(e.Value) = False AndAlso IsNumeric(e.Value) = False Then
                e.Value = UnFormatSSN(CStr(e.Value))
            End If
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' formats SSN values entered for a databinding
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[nick snyder]	8/16/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub SSNBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            If IsDBNull(e.Value) = False AndAlso IsNumeric(e.Value) = False Then
                e.Value = FormatSSN(CStr(e.Value))
            End If
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' adjusts TIN values entered for a databinding
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[nick snyder]	8/16/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub TINBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            If IsDBNull(e.Value) = False AndAlso IsNumeric(e.Value) = False Then
                e.Value = UnFormatTIN(CStr(e.Value))
            End If
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' formats TIN values entered for a databinding
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[nick snyder]	8/16/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub TINBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            If IsDBNull(e.Value) = False AndAlso IsNumeric(e.Value) = False Then
                e.Value = FormatTIN(CStr(e.Value))
            End If
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' formats combobox values entered for a databinding
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[nick snyder]	8/16/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub EmailTextBox_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ProviderPrimaryEmailTextBox.Validating, ProviderAltEmailTextBox.Validating

        Try

            Dim ValidationString As TextBox = CType(sender, TextBox)
            If ValidationString.Text.Trim(CChar(" ")).Length > 0 AndAlso Not RegularExpression.IsEmail(ValidationString.Text) Then
                ' Set the error if the name is not valid.
                ErrorProvider.SetError(CType(sender, Control), "Email format is invalid.")

                '                e.Cancel = True
            Else
                ' Clear the error, if any, in the error provider.
                ErrorProvider.SetError(CType(sender, Control), "")
                '                sender.ForeColor = sender.defaultforecolor()
            End If
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        Finally

        End Try

    End Sub
    Private Sub CountryTextBox_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ProviderCountryTextBox.Validating

        Try

            Dim ValidationString As TextBox = CType(sender, TextBox)
            If ValidationString.Text.Trim(CChar(" ")).Length > 0 AndAlso (Me.ProviderStateListBox.Text.Trim(CChar(" ")).Length = 0 AndAlso Me.ProviderZipTextBox.Text.Trim(CChar(" ")).Length = 0) Then
                ' Set the error if the name is not valid.
                ErrorProvider.SetError(CType(sender, Control), "Country is blank.")

                '                e.Cancel = True
            Else
                ' Clear the error, if any, in the error provider.
                ErrorProvider.SetError(CType(sender, Control), "")
                '                sender.ForeColor = sender.defaultforecolor()
            End If
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        Finally

        End Try

    End Sub
    Private Sub PhoneTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ProviderPrimaryPhoneTextBox.Validating, ProviderAltPhoneTextBox.Validating

        Try
            Dim ValidationString As TextBox = CType(sender, TextBox)
            If ValidationString.Text.Trim(CChar(" ")).Length > 0 AndAlso Not RegularExpression.IsPhoneNumber(ValidationString.Text) Then
                ' Set the error if the name is not valid.
                ErrorProvider.SetError(CType(sender, Control), "Phone format is not valid.")


                '                e.Cancel = True
            Else
                ' Clear the error, if any, in the error provider.
                ErrorProvider.SetError(CType(sender, Control), "")
                '                sender.ForeColor = sender.defaultforecolor()
            End If
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        Finally

        End Try

    End Sub

    Private Sub PreventNumberTextBox_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ProviderStateListBox.KeyPress

        If (Char.IsLetter(e.KeyChar) = False) Then
            e.Handled = True
        End If

    End Sub

    Private Sub AllowNull_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            If CStr(e.Value).Trim = "" Then
                e.Value = DBNull.Value
            End If
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub
    Private Sub ReplaceCRLF_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            e.Value = Replace(CStr(e.Value), vbCrLf, "\n")
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' formats an ssn
    ''' </summary>
    ''' <param name="strSSN"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[nick snyder]	8/16/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function FormatSSN(ByVal strSSN As String) As String
        Dim strTemp As String

        strTemp = UnFormatSSN(strSSN)
        If strTemp.Trim <> "" Then
            Return Microsoft.VisualBasic.Left(strTemp, 3) & "-" & Microsoft.VisualBasic.Mid(strTemp, 4, 2) & "-" & Microsoft.VisualBasic.Right(strTemp, 4)
        Else
            Return ""
        End If
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' unformats an ssn
    ''' </summary>
    ''' <param name="strSSN"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[nick snyder]	8/16/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function UnFormatSSN(ByVal strSSN As String) As String
        If Replace(Replace(Replace(strSSN, " ", ""), "-", ""), "/", "") <> "" Then
            Return Format(CLng(Replace(Replace(Replace(strSSN, " ", ""), "-", ""), "/", "")), "0########")
        Else
            Return ""
        End If
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' formats an TIN
    ''' </summary>
    ''' <param name="tin"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[nick snyder]	8/16/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function FormatTIN(ByVal tin As String) As String

        tin = UnFormatTIN(tin)

        If IsNumeric(tin) = True Then
            tin = Format(CLng(tin), "00-0000000")
        End If

        Return tin
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Unformats an TIN
    ''' </summary>
    ''' <param name="tin"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[nick snyder]	8/16/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function UnFormatTIN(ByVal tin As String) As String
        tin = Replace(Replace(Replace(tin.ToUpper.Trim, "/", ""), "-", ""), " ", "")
        tin = UnFormatSSN(tin)

        Return tin
    End Function

#End Region

#Region "Grid Handling Functions"

    Private Sub AssociatedProvidersDataGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles AssociatedProvidersDataGrid.DoubleClick
        Dim DR As DataRow

        Try
            If ProviderAssociatedSummaryBS Is Nothing OrElse ProviderAssociatedSummaryBS.Position < 0 OrElse ProviderAssociatedSummaryBS.Current Is Nothing Then Return

            mobjMessage.StatusMessage("Retrieving Item.  Please Wait...")
            Me.Cursor = Cursors.WaitCursor

            DR = CType(ProviderAssociatedSummaryBS.Current, DataRowView).Row

            RaiseEvent OpenProvider(Me, New ProviderEventArgs(CInt(DR("PROVIDER_ID"))))

CleanUp:

        Catch ex As Exception

            mobjMessage.StatusMessage("Open Error")
            Me.Cursor = Cursors.Default

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Finally
            mobjMessage.StatusMessage("")
            Me.Cursor = Cursors.Default
        End Try

    End Sub
    Private Sub ProviderLicenseViewChange(ByVal sender As Object, ByVal args As System.ComponentModel.ListChangedEventArgs)

        Select Case args.ListChangedType
            Case ComponentModel.ListChangedType.ItemAdded

                'prevent recursive processing
                ProviderLicensesBS.RaiseListChangedEvents = False

                CType(sender, DataView).Item(args.NewIndex).Row("PROVIDER_ID") = CType(CType(CType(ProviderAddressSummaryWithCommentsDS.ProviderAddressSummaryWithComments.DefaultView.Item(0), System.Data.DataRowView).Row, System.Data.DataRow), ProviderAddressSummaryWithCommentsDS.ProviderAddressSummaryWithCommentsRow).PROVIDER_ID

                ProviderLicensesBS.RaiseListChangedEvents = True

        End Select

    End Sub

#End Region

#Region "Address Functions"

    Private Sub ProviderAddressTypesListBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProviderAddressTypesListBox.SelectedIndexChanged

        Dim SelectedItem As ListCategory = CType(CType(sender, ComboBox).SelectedItem, ListCategory)

        Me.AddressModifyButton.Enabled = False

        Try
            If ProviderAddressSummaryWithCommentsBS.Position > -1 Then

                If ProviderAddressTypesListBox.Visible = True AndAlso AddressModifyButton.Text = "Remove Address" AndAlso ErrorProvider.CheckRequiredAndShowSummaryErrorMessage() = False Then 'Current address failed validation
                    RemoveHandler ProviderAddressTypesListBox.SelectedIndexChanged, AddressOf ProviderAddressTypesListBox_SelectedIndexChanged 'stop recursive reentry
                    ProviderAddressTypesListBox.SelectedIndex = lastAddressIndex
                    AddHandler ProviderAddressTypesListBox.SelectedIndexChanged, AddressOf ProviderAddressTypesListBox_SelectedIndexChanged
                Else
                    ProviderAddressSummaryWithCommentsBS.Filter() = "AddressType='" & SelectedItem.ListDescription & "'"

                    If ProviderAddressSummaryWithCommentsBS.Count < 1 Then
                        Me.AddressModifyButton.Text = "Enable Address"
                        Me.ProviderAddressGroupBox.Enabled = False
                    Else
                        Me.AddressModifyButton.Text = "Remove Address"
                        Me.ProviderAddressGroupBox.Enabled = True
                    End If

                    If SelectedItem.ListDescription <> "MAIL" Then
                        Me.AddressModifyButton.Enabled = True
                    Else
                        Me.AddressModifyButton.Enabled = False
                        Me.ProviderAddressGroupBox.Enabled = True
                    End If

                End If

            Else
                ProviderAddressSummaryWithCommentsBS.Filter() = "AddressType='" & SelectedItem.ListDescription & "'"

                If ProviderAddressSummaryWithCommentsBS.Count < 1 Then
                    Me.AddressModifyButton.Text = "Enable Address"
                    Me.ProviderAddressGroupBox.Enabled = False
                Else
                    Me.AddressModifyButton.Text = "Remove Address"
                    Me.ProviderAddressGroupBox.Enabled = True
                End If

                If SelectedItem.ListDescription <> "MAIL" Then
                    Me.AddressModifyButton.Enabled = True
                Else
                    Me.AddressModifyButton.Enabled = False
                    Me.ProviderAddressGroupBox.Enabled = True
                End If

            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        Finally
            lastAddressIndex = ProviderAddressTypesListBox.SelectedIndex
        End Try

    End Sub

#End Region

    Private Sub AboutMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutMenuItem.Click

        Dim FRM As New AboutWorkForm

        FRM.ShowDialog(Me)
        FRM.Dispose()
        FRM = Nothing

    End Sub

    Private Sub TextBoxMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ProviderNameTextBox.MouseUp, ProviderNPITextBox.MouseUp, ProviderIDTextBox.MouseUp, ProviderCityTextBox.MouseUp, ProviderNameTextBox.MouseUp, ProviderZip4TextBox.MouseUp, ProviderAltPhoneTextBox.MouseUp, ProviderAltContactTextBox.MouseUp, ProviderAltEmailTextBox.MouseUp, ProviderAltPhoneExtTextBox.MouseUp, ProviderPrimaryContactTextBox.MouseUp, ProviderPrimaryPhoneExtTextBox.MouseUp, ProviderPrimaryEmailTextBox.MouseUp, ProviderPrimaryPhoneTextBox.MouseUp, ProviderAddress1TextBox.MouseUp, ProviderAddress2TextBox.MouseUp

        If DoSelectAll Then
            DoSelectAll = False
            CType(sender, TextBox).SelectAll()
        End If

    End Sub
    Private Sub TextBoxEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles ProviderNameTextBox.Enter, ProviderIDTextBox.Enter, ProviderNPITextBox.Enter, ProviderCityTextBox.Enter, ProviderNameTextBox.Enter, ProviderZip4TextBox.Enter, ProviderAltPhoneTextBox.Enter, ProviderAltContactTextBox.Enter, ProviderAltEmailTextBox.Enter, ProviderAltPhoneExtTextBox.Enter, ProviderPrimaryContactTextBox.Enter, ProviderPrimaryPhoneExtTextBox.Enter, ProviderPrimaryEmailTextBox.Enter, ProviderPrimaryPhoneTextBox.Enter, ProviderAddress1TextBox.Enter, ProviderAddress2TextBox.Enter

        CType(sender, TextBox).SelectAll()
        DoSelectAll = True

    End Sub

    Private Sub SuspendedProviderAddressCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SuspendedProviderAddressCheckBox.CheckedChanged

        If CType(sender, CheckBox).CheckState = CheckState.Checked Then
            ErrorProvider.ClearAllErrorMessages()
        End If

    End Sub
    Private Sub WorkStatusBar_DrawItem(ByVal sender As Object, ByVal sbdevent As System.Windows.Forms.StatusBarDrawItemEventArgs) Handles WorkStatusBar.DrawItem

        Dim g As Graphics = sbdevent.Graphics
        Dim sb As StatusBar = CType(sender, StatusBar)
        Dim rectf As RectangleF = New RectangleF(sbdevent.Bounds.X, sbdevent.Bounds.Y, sbdevent.Bounds.Width, sbdevent.Bounds.Height)

        g.DrawRectangle(p, sbdevent.Bounds)
        sbdevent.Graphics.FillRectangle(arBrushes(sbdevent.Index), sbdevent.Bounds)
        g.DrawString("Panel" & sbdevent.Index, sb.Font, brBlueFontBrush, rectf)

    End Sub

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If

            Dim i As Integer
            p.Dispose()
            brBlueFontBrush.Dispose()
            For i = 0 To arBrushes.Length - 1
                arBrushes(i).Dispose()
            Next
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub ProviderHistoryDataGrid_RefreshGridData() Handles ProviderHistoryDataGrid.RefreshGridData

        LoadProviderHistory()

    End Sub

    Private Sub ProviderHistoryDataGrid_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles ProviderHistoryDataGrid.MouseHover

        If hoverCell.RowNumber > -1 AndAlso (CType(sender, DataGridCustom).Item(hoverCell.RowNumber, 0) IsNot Nothing AndAlso CType(sender, DataGridCustom).Item(hoverCell.RowNumber, 1) IsNot Nothing) Then
            Me.WorkToolTip.SetToolTip(Me.ProviderHistoryDataGrid, CType(sender, DataGridCustom).Item(hoverCell.RowNumber, 0).ToString & " " & CType(sender, DataGridCustom).Item(hoverCell.RowNumber, 1).ToString)
        Else
            Me.WorkToolTip.SetToolTip(Me.ProviderHistoryDataGrid, "")
        End If

    End Sub

    Private Sub ProviderHistoryDataGrid_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ProviderHistoryDataGrid.MouseMove

        Dim hti As DataGrid.HitTestInfo
        hti = CType(sender, DataGridCustom).HitTest(e.X, e.Y)

        ' Do not display hover text if it is a drag event 
        If (e.Button <> MouseButtons.Left) Then

            ' Check if the target is a different cell from the previous one 
            If hti.Type = DataGrid.HitTestType.Cell AndAlso
              hti.Row <> hoverCell.RowNumber Or hti.Column <> hoverCell.ColumnNumber Then

                ' Store the new hit row 
                hoverCell.RowNumber = hti.Row
                hoverCell.ColumnNumber = hti.Column

            End If

        End If

    End Sub

End Class

Public Class ProviderEventArgs
    Inherits EventArgs
    Private _ProviderID As Integer

    'Constructor.
    '
    Public Sub New(ByVal ProviderID As Integer)
        Me._ProviderID = ProviderID
    End Sub

    '
    Public ReadOnly Property ProviderID() As Integer
        Get
            Return _ProviderID
        End Get
    End Property

End Class



