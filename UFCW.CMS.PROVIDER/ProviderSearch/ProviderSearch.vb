Option Strict On

Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Reflection
Imports System.Security.Principal
Imports System.Threading
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Data.Common


''' -----------------------------------------------------------------------------
''' Project	 : Queue
''' Class	 : UFCW.CMS.PROVIDERS.UI.Search
''' 
''' -----------------------------------------------------------------------------
''' <summary>
''' Form to manage workflow of work items
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[Nick Snyder]	2/15/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<SharedInterfaces.PlugIn("Search", 0, "Provider", 1)> Public Class ProviderSearch
    Inherits System.Windows.Forms.Form

    Implements SharedInterfaces.IMessage

    Const MAXWORKFORMS As Integer = 40
    Const MAXGUIHANDLES As Long = 8975933078237088100
    Const PLUINFILTER As String = "UFCW.CMS.*.Plugin.dll"

    ReadOnly DomainUser As String = SystemInformation.UserName

    Private DoSelectAll As Boolean = False
    Private mOpenIndex As Integer
    Private mProviderfrm As ProviderWork
    Private mNPIfrm As NPIView
    Private mfrm As Form
    Private WithEvents mplug As PlugInController
    Private mobjMessage As SharedInterfaces.IMessage

    Private WithEvents _ProviderUFCWBS As BindingSource
    Private WithEvents _ProviderNPIBS As BindingSource

    'Note _ProviderDS is an AutoImplemented field created by the Strongly Typed Dataset

    Private LastDocClass As String = ""
    Private LastDocType As String = ""
    Private LastOutMessage As String = ""
    Private mCancel As Boolean = False
    Private DubClick As Boolean = False
    Private OpenChildrenCount As Integer = 0
    Private TotalOpened As Integer = 0
    Private OpenChildrenCounts As New Hashtable
    Private DelDocs() As String
    Private bizUserPrincipal As WindowsPrincipal

    Friend WithEvents SplitContainer As System.Windows.Forms.SplitContainer
    Friend WithEvents UFCWProvidersDataGrid As DataGridCustom
    Friend WithEvents NPIProvidersDataGrid As DataGridCustom

    Private _Children As New Hashtable
    Private _UniqueID As String
    Friend WithEvents NPISearchContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Private _APPKEY As String = "UFCW\ProviderSearch\"

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal objMsg As SharedInterfaces.IMessage, ByVal OpenIndex As Integer)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        mobjMessage = objMsg
        mOpenIndex = OpenIndex
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents SearchButton As System.Windows.Forms.Button
    Friend WithEvents ProviderNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ProviderDS As ProvidersDS
    Friend WithEvents OpenButton As System.Windows.Forms.Button
    Friend WithEvents NewButton As System.Windows.Forms.Button
    Friend WithEvents ProviderNPITextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ProviderTINTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents ProviderSearchContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents OpenMenuItem As System.Windows.Forms.ToolStripMenuItem

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ProviderSearch))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ProviderTINTextBox = New System.Windows.Forms.TextBox()
        Me.ProviderSearchContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.OpenMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ProviderNPITextBox = New System.Windows.Forms.TextBox()
        Me.NewButton = New System.Windows.Forms.Button()
        Me.OpenButton = New System.Windows.Forms.Button()
        Me.ProviderNameTextBox = New System.Windows.Forms.TextBox()
        Me.SearchButton = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.ProviderDS = New ProvidersDS()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.UFCWProvidersDataGrid = New DataGridCustom()
        Me.NPIProvidersDataGrid = New DataGridCustom()
        Me.SplitContainer = New System.Windows.Forms.SplitContainer()
        Me.NPISearchContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ProviderSearchContextMenu.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.ProviderDS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UFCWProvidersDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NPIProvidersDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer.Panel1.SuspendLayout()
        Me.SplitContainer.Panel2.SuspendLayout()
        Me.SplitContainer.SuspendLayout()
        Me.NPISearchContextMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 338)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(772, 20)
        Me.Panel1.TabIndex = 24
        '
        'ProviderTINTextBox
        '
        Me.ProviderTINTextBox.Location = New System.Drawing.Point(80, 24)
        Me.ProviderTINTextBox.MaxLength = 12
        Me.ProviderTINTextBox.Name = "ProviderTINTextBox"
        Me.ProviderTINTextBox.Size = New System.Drawing.Size(100, 20)
        Me.ProviderTINTextBox.TabIndex = 0
        Me.ToolTip.SetToolTip(Me.ProviderTINTextBox, "Enter either the SSN or TAXID of the Provider")
        '
        'ProviderSearchContextMenu
        '
        Me.ProviderSearchContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenMenuItem})
        Me.ProviderSearchContextMenu.Name = "ProviderSearchContextMenu"
        Me.ProviderSearchContextMenu.Size = New System.Drawing.Size(147, 26)
        '
        'OpenMenuItem
        '
        Me.OpenMenuItem.Name = "OpenMenuItem"
        Me.OpenMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.OpenMenuItem.Size = New System.Drawing.Size(146, 22)
        Me.OpenMenuItem.Text = "Open"
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox1.Controls.Add(Me.ProviderNPITextBox)
        Me.GroupBox1.Controls.Add(Me.NewButton)
        Me.GroupBox1.Controls.Add(Me.OpenButton)
        Me.GroupBox1.Controls.Add(Me.ProviderNameTextBox)
        Me.GroupBox1.Controls.Add(Me.SearchButton)
        Me.GroupBox1.Controls.Add(Me.ProviderTINTextBox)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Location = New System.Drawing.Point(4, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(764, 56)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Search By"
        '
        'ProviderNPITextBox
        '
        Me.ProviderNPITextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ProviderNPITextBox.Location = New System.Drawing.Point(380, 24)
        Me.ProviderNPITextBox.MaxLength = 10
        Me.ProviderNPITextBox.Name = "ProviderNPITextBox"
        Me.ProviderNPITextBox.Size = New System.Drawing.Size(108, 20)
        Me.ProviderNPITextBox.TabIndex = 2
        Me.ToolTip.SetToolTip(Me.ProviderNPITextBox, "10 Digit NPI number")
        '
        'NewButton
        '
        Me.NewButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NewButton.Location = New System.Drawing.Point(589, 24)
        Me.NewButton.Name = "NewButton"
        Me.NewButton.Size = New System.Drawing.Size(75, 23)
        Me.NewButton.TabIndex = 4
        Me.NewButton.Text = "New"
        Me.ToolTip.SetToolTip(Me.NewButton, "Used to create a new provider")
        '
        'OpenButton
        '
        Me.OpenButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OpenButton.Location = New System.Drawing.Point(676, 24)
        Me.OpenButton.Name = "OpenButton"
        Me.OpenButton.Size = New System.Drawing.Size(76, 24)
        Me.OpenButton.TabIndex = 5
        Me.OpenButton.Text = "Open"
        Me.ToolTip.SetToolTip(Me.OpenButton, "After selecting a provider use this button to view the provider details.")
        '
        'ProviderNameTextBox
        '
        Me.ProviderNameTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ProviderNameTextBox.Location = New System.Drawing.Point(224, 24)
        Me.ProviderNameTextBox.MaxLength = 50
        Me.ProviderNameTextBox.Name = "ProviderNameTextBox"
        Me.ProviderNameTextBox.Size = New System.Drawing.Size(124, 20)
        Me.ProviderNameTextBox.TabIndex = 1
        Me.ToolTip.SetToolTip(Me.ProviderNameTextBox, "Compared against Provider Title column (Partial Name search is OK)")
        '
        'SearchButton
        '
        Me.SearchButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SearchButton.Location = New System.Drawing.Point(504, 24)
        Me.SearchButton.Name = "SearchButton"
        Me.SearchButton.Size = New System.Drawing.Size(75, 23)
        Me.SearchButton.TabIndex = 3
        Me.SearchButton.Text = "Search"
        Me.ToolTip.SetToolTip(Me.SearchButton, "Enter either ProviderID, Name (Partial OK) or NPI before using this function.")
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(352, 24)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(25, 13)
        Me.Label4.TabIndex = 26
        Me.Label4.Text = "NPI"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(188, 24)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(35, 13)
        Me.Label6.TabIndex = 24
        Me.Label6.Text = "Name"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(12, 24)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(60, 13)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "Provider ID"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ProviderDS
        '
        Me.ProviderDS.DataSetName = "ProviderDS"
        Me.ProviderDS.EnforceConstraints = False
        Me.ProviderDS.Locale = New System.Globalization.CultureInfo("en-US")
        Me.ProviderDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'UFCWProvidersDataGrid
        '
        Me.UFCWProvidersDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.UFCWProvidersDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.UFCWProvidersDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.UFCWProvidersDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.UFCWProvidersDataGrid.ADGroupsThatCanFind = ""
        Me.UFCWProvidersDataGrid.ADGroupsThatCanMultiSort = ""
        Me.UFCWProvidersDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.UFCWProvidersDataGrid.AllowAutoSize = True
        Me.UFCWProvidersDataGrid.AllowColumnReorder = True
        Me.UFCWProvidersDataGrid.AllowCopy = True
        Me.UFCWProvidersDataGrid.AllowCustomize = True
        Me.UFCWProvidersDataGrid.AllowDelete = False
        Me.UFCWProvidersDataGrid.AllowDragDrop = False
        Me.UFCWProvidersDataGrid.AllowEdit = False
        Me.UFCWProvidersDataGrid.AllowExport = False
        Me.UFCWProvidersDataGrid.AllowFilter = True
        Me.UFCWProvidersDataGrid.AllowFind = True
        Me.UFCWProvidersDataGrid.AllowGoTo = True
        Me.UFCWProvidersDataGrid.AllowMultiSelect = False
        Me.UFCWProvidersDataGrid.AllowMultiSort = True
        Me.UFCWProvidersDataGrid.AllowNew = False
        Me.UFCWProvidersDataGrid.AllowPrint = False
        Me.UFCWProvidersDataGrid.AllowRefresh = True
        Me.UFCWProvidersDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UFCWProvidersDataGrid.AppKey = "UFCW\ProviderSearch\"
        Me.UFCWProvidersDataGrid.AutoSaveCols = True
        Me.UFCWProvidersDataGrid.BackgroundColor = System.Drawing.SystemColors.Control
        Me.UFCWProvidersDataGrid.CaptionText = "Matching UFCW Providers"
        Me.UFCWProvidersDataGrid.ColumnHeaderLabel = Nothing
        Me.UFCWProvidersDataGrid.ColumnRePositioning = False
        Me.UFCWProvidersDataGrid.ColumnResizing = False
        Me.UFCWProvidersDataGrid.ConfirmDelete = True
        Me.UFCWProvidersDataGrid.CopySelectedOnly = True
        Me.UFCWProvidersDataGrid.CurrentBSPosition = -1
        Me.UFCWProvidersDataGrid.DataMember = ""
        Me.UFCWProvidersDataGrid.DragColumn = 0
        Me.UFCWProvidersDataGrid.ExportSelectedOnly = True
        Me.UFCWProvidersDataGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.UFCWProvidersDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.UFCWProvidersDataGrid.HighlightedRow = Nothing
        Me.UFCWProvidersDataGrid.HighLightModifiedRows = False
        Me.UFCWProvidersDataGrid.IsMouseDown = False
        Me.UFCWProvidersDataGrid.LastGoToLine = ""
        Me.UFCWProvidersDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.UFCWProvidersDataGrid.MultiSort = False
        Me.UFCWProvidersDataGrid.Name = "UFCWProvidersDataGrid"
        Me.UFCWProvidersDataGrid.OldSelectedRow = Nothing
        Me.UFCWProvidersDataGrid.PreviousBSPosition = -1
        Me.UFCWProvidersDataGrid.ReadOnly = True
        Me.UFCWProvidersDataGrid.RetainRowSelectionAfterSort = True
        Me.UFCWProvidersDataGrid.SetRowOnRightClick = True
        Me.UFCWProvidersDataGrid.ShiftPressed = False
        Me.UFCWProvidersDataGrid.SingleClickBooleanColumns = True
        Me.UFCWProvidersDataGrid.Size = New System.Drawing.Size(768, 127)
        Me.UFCWProvidersDataGrid.Sort = Nothing
        Me.UFCWProvidersDataGrid.StyleName = ""
        Me.UFCWProvidersDataGrid.SubKey = ""
        Me.UFCWProvidersDataGrid.SuppressMouseDown = False
        Me.UFCWProvidersDataGrid.SuppressTriangle = False
        Me.UFCWProvidersDataGrid.TabIndex = 2
        Me.ToolTip.SetToolTip(Me.UFCWProvidersDataGrid, "Select item and hit open to view Provider details")
        '
        'NPIProvidersDataGrid
        '
        Me.NPIProvidersDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.NPIProvidersDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.NPIProvidersDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.NPIProvidersDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.NPIProvidersDataGrid.ADGroupsThatCanFind = ""
        Me.NPIProvidersDataGrid.ADGroupsThatCanMultiSort = ""
        Me.NPIProvidersDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.NPIProvidersDataGrid.AllowAutoSize = True
        Me.NPIProvidersDataGrid.AllowColumnReorder = True
        Me.NPIProvidersDataGrid.AllowCopy = True
        Me.NPIProvidersDataGrid.AllowCustomize = True
        Me.NPIProvidersDataGrid.AllowDelete = False
        Me.NPIProvidersDataGrid.AllowDragDrop = False
        Me.NPIProvidersDataGrid.AllowEdit = False
        Me.NPIProvidersDataGrid.AllowExport = False
        Me.NPIProvidersDataGrid.AllowFilter = True
        Me.NPIProvidersDataGrid.AllowFind = True
        Me.NPIProvidersDataGrid.AllowGoTo = True
        Me.NPIProvidersDataGrid.AllowMultiSelect = False
        Me.NPIProvidersDataGrid.AllowMultiSort = True
        Me.NPIProvidersDataGrid.AllowNew = False
        Me.NPIProvidersDataGrid.AllowPrint = False
        Me.NPIProvidersDataGrid.AllowRefresh = True
        Me.NPIProvidersDataGrid.AppKey = "UFCW\ProviderSearch\"
        Me.NPIProvidersDataGrid.AutoSaveCols = True
        Me.NPIProvidersDataGrid.BackgroundColor = System.Drawing.SystemColors.Control
        Me.NPIProvidersDataGrid.CaptionText = "Matching NPI Providers"
        Me.NPIProvidersDataGrid.ColumnHeaderLabel = Nothing
        Me.NPIProvidersDataGrid.ColumnRePositioning = False
        Me.NPIProvidersDataGrid.ColumnResizing = False
        Me.NPIProvidersDataGrid.ConfirmDelete = True
        Me.NPIProvidersDataGrid.CopySelectedOnly = True
        Me.NPIProvidersDataGrid.CurrentBSPosition = -1
        Me.NPIProvidersDataGrid.DataMember = ""
        Me.NPIProvidersDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.NPIProvidersDataGrid.DragColumn = 0
        Me.NPIProvidersDataGrid.ExportSelectedOnly = True
        Me.NPIProvidersDataGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.NPIProvidersDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.NPIProvidersDataGrid.HighlightedRow = Nothing
        Me.NPIProvidersDataGrid.HighLightModifiedRows = False
        Me.NPIProvidersDataGrid.IsMouseDown = False
        Me.NPIProvidersDataGrid.LastGoToLine = ""
        Me.NPIProvidersDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.NPIProvidersDataGrid.MultiSort = False
        Me.NPIProvidersDataGrid.Name = "NPIProvidersDataGrid"
        Me.NPIProvidersDataGrid.OldSelectedRow = Nothing
        Me.NPIProvidersDataGrid.PreviousBSPosition = -1
        Me.NPIProvidersDataGrid.ReadOnly = True
        Me.NPIProvidersDataGrid.RetainRowSelectionAfterSort = True
        Me.NPIProvidersDataGrid.SetRowOnRightClick = True
        Me.NPIProvidersDataGrid.ShiftPressed = False
        Me.NPIProvidersDataGrid.SingleClickBooleanColumns = True
        Me.NPIProvidersDataGrid.Size = New System.Drawing.Size(768, 159)
        Me.NPIProvidersDataGrid.Sort = Nothing
        Me.NPIProvidersDataGrid.StyleName = ""
        Me.NPIProvidersDataGrid.SubKey = ""
        Me.NPIProvidersDataGrid.SuppressMouseDown = False
        Me.NPIProvidersDataGrid.SuppressTriangle = False
        Me.NPIProvidersDataGrid.TabIndex = 3
        Me.ToolTip.SetToolTip(Me.NPIProvidersDataGrid, "Select item and hit open to view Provider details")
        '
        'SplitContainer
        '
        Me.SplitContainer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer.Location = New System.Drawing.Point(4, 66)
        Me.SplitContainer.Margin = New System.Windows.Forms.Padding(0)
        Me.SplitContainer.Name = "SplitContainer"
        Me.SplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer.Panel1
        '
        Me.SplitContainer.Panel1.Controls.Add(Me.UFCWProvidersDataGrid)
        '
        'SplitContainer.Panel2
        '
        Me.SplitContainer.Panel2.Controls.Add(Me.NPIProvidersDataGrid)
        Me.SplitContainer.Size = New System.Drawing.Size(768, 290)
        Me.SplitContainer.SplitterDistance = 127
        Me.SplitContainer.TabIndex = 25
        '
        'NPISearchContextMenu
        '
        Me.NPISearchContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1})
        Me.NPISearchContextMenu.Name = "ProviderSearchContextMenu"
        Me.NPISearchContextMenu.Size = New System.Drawing.Size(147, 26)
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(146, 22)
        Me.ToolStripMenuItem1.Text = "Open"
        '
        'ProviderSearch
        '
        Me.AcceptButton = Me.SearchButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(772, 358)
        Me.Controls.Add(Me.SplitContainer)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(780, 300)
        Me.Name = "ProviderSearch"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Search for Provider"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ProviderSearchContextMenu.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.ProviderDS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UFCWProvidersDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NPIProvidersDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer.Panel1.ResumeLayout(False)
        Me.SplitContainer.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer.ResumeLayout(False)
        Me.NPISearchContextMenu.ResumeLayout(False)
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

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' this property allows access to child forms
    ''' </summary>
    ''' <value></value>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	3/24/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public ReadOnly Property Children() As Hashtable
        Get
            Return _Children
        End Get
    End Property

    <Browsable(True), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal Value As String)
            _APPKEY = Value
        End Set
    End Property
#End Region
#Region "BackThread Class"
    Public Class ExecuteQuery
        Friend _ProviderName As String
        Friend _TIN As String
        Friend _NPI As String
        Friend ResultSet As New ProvidersDS

        Sub New(ByVal TIN As String, ByVal ProviderName As String, ByVal NPI As String)
            _ProviderName = ProviderName
            _TIN = TIN
            _NPI = NPI
        End Sub

        Public Sub Execute()
            Dim tin As Integer
            Dim npi As Decimal

            Try
                If Len(Trim$(_ProviderName)) > 0 Then
                    ResultSet = CType(ProviderDAL.RetrieveNPIAndUFCWProvidersByName(_ProviderName & "%", ResultSet), ProvidersDS)
                ElseIf Len(Trim$(_TIN)) > 0 AndAlso Len(Trim$(_TIN)) < 7 AndAlso Integer.TryParse(_TIN, tin) Then
                    ResultSet = CType(ProviderDAL.RetrieveProvidersByPROVIDERID(tin, ResultSet), ProvidersDS)
                ElseIf Len(Trim$(_TIN)) > 0 AndAlso Integer.TryParse(_TIN, tin) Then
                    ResultSet = CType(ProviderDAL.RetrieveNPIAndUFCWProvidersByTAXID(tin, ResultSet), ProvidersDS)
                ElseIf Len(Trim$(_NPI)) > 0 AndAlso Decimal.TryParse(_NPI, npi) Then
                    ResultSet = CType(ProviderDAL.RetrieveNPIAndUFCWProvidersByNPI(npi, ResultSet), ProvidersDS)
                End If

            Catch ex As Exception
                If System.Threading.Thread.CurrentThread.ThreadState <> ThreadState.AbortRequested Then
                    Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
                    If (rethrow) Then
                        MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End If
            End Try
        End Sub

    End Class
#End Region

#Region "Form Events"

    Private Sub ProviderSearch_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        SaveSettings()

    End Sub
    Private Sub Queue_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If mOpenIndex <> 0 Then Me.Text = "Work Queue - " & mOpenIndex

            SetSettings()

            Me.Show()

            mobjMessage.StatusMessage("Logging On.  Please Wait...")

            Me.Refresh()

            'Init Active Directory
            AppDomain.CurrentDomain.SetPrincipalPolicy(Security.Principal.PrincipalPolicy.WindowsPrincipal)
            bizUserPrincipal = CType(System.Threading.Thread.CurrentPrincipal, WindowsPrincipal)

            If UFCWGeneralAD.CMSCanModifyProvider OrElse UFCWGeneralAD.CMSCanAddProvider OrElse UFCWGeneralAD.CMSAdministrators Then
                'all functions available
            Else
                'read only access
                NewButton.Enabled = False
            End If

            'Plugins
            mplug = New PlugInController(PLUINFILTER)
            Me.Refresh()

            mplug.LoadPlugIns()

            Me.Refresh()

            mobjMessage.StatusMessage(LastOutMessage)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub Queue_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Try
            SaveSettings()

            mobjMessage.StatusMessage("")

            RemoveChildrenHandlers()
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub RemoveChildrenHandlers()
        Dim f As Form
        Dim GE As Collections.IDictionaryEnumerator
        Dim cnt As Integer = 0

        Try
            GE = _Children.GetEnumerator
            GE.Reset()

            For cnt = 0 To _Children.Values.Count - 1
                GE.MoveNext()

                f = CType(GE.Entry.Value, Form)

                RemoveHandler f.Closed, AddressOf mfrm_Closed
            Next

            GE = Nothing
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub

    Private Sub mfrm_Closed(ByVal sender As Object, ByVal e As EventArgs)
        Dim frm As Form = CType(sender, Form)
        Dim UIDPI As PropertyInfo
        Dim UID As String = ""

        If OpenChildrenCounts.Count > 0 Then
            If OpenChildrenCounts.Contains(CType(sender, Form).Name) = True Then
                OpenChildrenCounts.Item(CType(sender, Form).Name) = CInt(OpenChildrenCounts.Item(CType(sender, Form).Name)) - 1
            End If
        End If

        UIDPI = frm.GetType.GetProperty("UniqueID")
        If Not UIDPI Is Nothing Then
            UID = CStr(frm.GetType.InvokeMember("UniqueID", BindingFlags.GetProperty, Nothing, frm, New Object() {}))

            If _Children.ContainsKey(UID) = True Then
                _Children.Remove(UID)
            End If
        End If

        sender = Nothing

        OpenChildrenCount -= 1

        If OpenChildrenCount = 0 Then
            Me.Focus()
        End If

    End Sub
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' resets the status message when this form gets focus
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	3/24/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub Provider_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Enter
        If LastOutMessage <> "" Then
            mobjMessage.StatusMessage(LastOutMessage)
        Else
            LastOutMessage = Me.Text + " Ready"
            mobjMessage.StatusMessage(LastOutMessage)
        End If
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
        'Me.Visible = False

        Me.Top = CInt(GetSetting(AppKey, "\Settings", "Top", CStr(Me.Top)))
        Me.Height = CInt(GetSetting(AppKey, "\Settings", "Height", CStr(Me.Height)))
        Me.Left = CInt(GetSetting(AppKey, "\Settings", "Left", CStr(Me.Left)))
        Me.Width = CInt(GetSetting(AppKey, "\Settings", "Width", CStr(Me.Width)))
        Me.WindowState = CType(GetSetting(AppKey, "\Settings", "WindowState", CStr(Me.WindowState)), FormWindowState)
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
        Dim lWindowState As Integer = FormWindowState.Maximized

        SaveSetting(AppKey, "\Settings", "WindowState", CStr(lWindowState))
        Me.WindowState = 0
        SaveSetting(AppKey, "\Settings", "Top", CStr(Me.Top))
        SaveSetting(AppKey, "\Settings", "Height", CStr(Me.Height))
        SaveSetting(AppKey, "\Settings", "Left", CStr(Me.Left))
        SaveSetting(AppKey, "\Settings", "Width", CStr(Me.Width))

    End Sub

#End Region

#Region "Menu\Button Events"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Loads the Queue Grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	2/15/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub SearchButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchButton.Click
        Dim SearchCount As Integer = 0

        If ProviderTINTextBox.Text.ToString.Trim.Length < 1 AndAlso ProviderNameTextBox.Text.ToString.Trim.Length < 1 AndAlso ProviderNPITextBox.Text.ToString.Trim.Length < 1 Then
            MessageBox.Show("Enter Search Criteria ", "Search cancelled", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            Try
                If ProviderTINTextBox.Text.ToString.Trim.Length > 0 Then SearchCount += 1
                If ProviderNameTextBox.Text.ToString.Trim.Length > 0 Then SearchCount += 1
                If ProviderNPITextBox.Text.ToString.Trim.Length > 0 Then SearchCount += 1

                If SearchCount > 1 Then
                    MessageBox.Show("You can only search by a single Search value (Provider, Name or NPI) ", "Search cancelled", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Else
                    SearchForProvider()
                End If

            Catch ex As Exception
                Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
                If (rethrow) Then
                    MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Try
        End If
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Loads the Queue Grid
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	4/3/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub ProvidersDataGrid_RefreshGridData() Handles UFCWProvidersDataGrid.RefreshGridData, NPIProvidersDataGrid.RefreshGridData
        Try
            SearchForProvider()
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

#End Region

#Region "Custom Subs\Functions"


    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Loads the Queue Grid based on the values supplied by the user
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	2/15/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub SearchForProvider()

        Dim STime As DateTime = Now
        Dim OutMessage As String = ""
        Dim FinishDuration As String = ""
        Dim sb As New System.Text.StringBuilder

        Try
            mCancel = False

            If Not _ProviderDS Is Nothing Then _ProviderDS.Clear()

            Me.Cursor = Cursors.WaitCursor

            mobjMessage.StatusMessage("Refreshing. Please Wait...")

            If ProviderTINTextBox.Text.Trim.Length > 0 AndAlso ProviderTINTextBox.Text.Trim.Length < 9 Then
                Me.SplitContainer.Panel2Collapsed = True

            Else
                Me.SplitContainer.Panel2Collapsed = False
            End If

            If ProviderTINTextBox.Text.Trim.Length > 0 OrElse ProviderNameTextBox.Text.Trim.Length > 0 Then
                UFCWProvidersDataGrid.CaptionText = "Refreshing. Please Wait..."
            End If

            If ProviderNameTextBox.Text.Trim.Length > 0 OrElse ProviderNPITextBox.Text.Trim.Length > 0 Then
                NPIProvidersDataGrid.CaptionText = "Refreshing. Please Wait..."
            End If

            Me.Refresh()

            Dim EQ As ExecuteQuery = New ExecuteQuery(ProviderTINTextBox.Text, ProviderNameTextBox.Text, ProviderNPITextBox.Text)

            Dim WorkerThread As Thread = New Thread(AddressOf EQ.Execute)

            WorkerThread.IsBackground = True

            WorkerThread.Start()

            Do Until WorkerThread.ThreadState = ThreadState.Stopped
                Thread.Sleep(0)
                Application.DoEvents()
                mobjMessage.StatusMessage("Refresh Duration " & cTime.ReturnDuration(STime))
                'Me.Refresh()

                If mCancel = True Then
                    WorkerThread.Priority = ThreadPriority.AboveNormal

                    WorkerThread.Abort()

                    Do Until WorkerThread.ThreadState = ThreadState.Aborted Or WorkerThread.ThreadState = ThreadState.Stopped
                        Thread.Sleep(0)
                        Application.DoEvents()
                        mobjMessage.StatusMessage("Cancelling Refresh...(" & cTime.ReturnDuration(STime) & ")")
                    Loop

                    Exit Do
                End If
            Loop

            _ProviderDS = EQ.ResultSet

            WorkerThread = Nothing
            EQ = Nothing

            FinishDuration = cTime.ReturnDuration(STime)

            If mCancel = True Then

                OutMessage = "User Cancelled Refresh After " & FinishDuration
                LastOutMessage = OutMessage
                mobjMessage.StatusMessage(OutMessage)

                UFCWProvidersDataGrid.CaptionText = OutMessage
                NPIProvidersDataGrid.CaptionText = OutMessage

                MessageBox.Show(OutMessage, "User Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Else
                If _ProviderDS.Tables("PROVIDER") IsNot Nothing Then

                    _ProviderUFCWBS = New BindingSource
                    _ProviderUFCWBS.DataSource = _ProviderDS.Tables("PROVIDER")
                    UFCWProvidersDataGrid.DataSource = _ProviderUFCWBS

                    UFCWProvidersDataGrid.SetTableStyle()
                    UFCWProvidersDataGrid.ContextMenuPrepare(ProviderSearchContextMenu)

                    UFCWProvidersDataGrid.Focus()

                    UFCWProvidersDataGrid.CaptionText = sb.Append("Found " & _ProviderDS.Tables("PROVIDER").Rows.Count.ToString & " Item" & IIf(_ProviderDS.Tables("PROVIDER").Rows.Count <> 1, "s", "").ToString & " in " & FinishDuration).ToString
                Else
                    LastOutMessage = "Nothing Found After " & FinishDuration

                    UFCWProvidersDataGrid.CaptionText = sb.Append(StrConv("0 - Pending Work Item In " & LastDocClass & IIf(LastDocType = "", "", ":" & LastDocType).ToString, VbStrConv.ProperCase)).ToString

                End If

                If _ProviderDS.Tables("NPI_REGISTRY") IsNot Nothing Then

                    _ProviderNPIBS = New BindingSource
                    _ProviderNPIBS.DataSource = _ProviderDS.Tables("NPI_REGISTRY")
                    NPIProvidersDataGrid.DataSource = _ProviderNPIBS

                    NPIProvidersDataGrid.SetTableStyle()
                    NPIProvidersDataGrid.ContextMenuPrepare(NPISearchContextMenu)

                    NPIProvidersDataGrid.Focus()

                    NPIProvidersDataGrid.CaptionText = sb.Append("Found " & _ProviderDS.Tables("NPI_REGISTRY").Rows.Count.ToString & " Item" & IIf(_ProviderDS.Tables("NPI_REGISTRY").Rows.Count <> 1, "s", "").ToString & " in " & FinishDuration).ToString
                Else
                    LastOutMessage = "Nothing Found After " & FinishDuration

                    NPIProvidersDataGrid.CaptionText = sb.Append(StrConv("0 - Pending Work Item In " & LastDocClass & IIf(LastDocType = "", "", ":" & LastDocType).ToString, VbStrConv.ProperCase)).ToString
                End If
            End If
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        Finally
            mobjMessage.StatusMessage("Search completed in " & FinishDuration)
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

    Private Function OpenWorkScreenUFCW(Optional ByVal ProviderID As Integer = 0, Optional ByVal Busy As Boolean = False, Optional ByVal Transaction As DbTransaction = Nothing) As Boolean

        Dim FinishTran As Boolean = False
        Dim Opened As Boolean = False
        Dim PendUser As String = ""
        Dim BusyState As Boolean = False
        Dim FamilyLocked As Boolean = False
        Dim YesAllDialog As New YesToAllDialog

        Try

            If mplug.Contains("ProviderWork") Then
                Opened = OpenProviderPlugIn("ProviderWork", ProviderID, Transaction)
                Return Opened
            Else
                Return False
            End If

        Catch ex As Exception
            If Transaction IsNot Nothing AndAlso Transaction.Connection IsNot Nothing Then
                CMSDALCommon.RollbackTransaction(Transaction)
            End If

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If

            Return False
        Finally

        End Try
    End Function
    Private Function OpenWorkScreenNPI(Optional ByVal NPI As Decimal = 0) As Boolean

        Try

            If mplug.Contains("NPIView") Then
                Return OpenNPIPlugIn("NPIView", NPI)
            Else
                Return False
            End If

        Catch ex As Exception

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If

            Return False
        Finally

        End Try
    End Function

    Public Shared Function GetProviderMetaData(ByVal ProviderID As Integer, ByVal Transaction As DbTransaction) As DataRow
        Dim DS As DataSet

        Try
            DS = ProviderDAL.RetrieveProvider(ProviderID)

            If DS IsNot Nothing AndAlso DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0).Rows(0)
            End If

            Return Nothing

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            Else
                Return Nothing
            End If
        End Try
    End Function

    Private Sub ProvidersDataGridContextMenu_Opening(ByVal sender As Object, ByVal e As System.EventArgs) Handles ProviderSearchContextMenu.Opening

        If UFCWProvidersDataGrid.GetGridRowCount > 0 Then
            OpenMenuItem.Enabled = True
        Else
            OpenMenuItem.Enabled = False
        End If
    End Sub
    Private Sub TextBoxMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ProviderTINTextBox.MouseUp, ProviderNameTextBox.MouseUp, ProviderNPITextBox.MouseUp

        If DoSelectAll Then
            DoSelectAll = False
            CType(sender, TextBox).SelectAll()
        End If

    End Sub
    Private Sub TextBoxEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles ProviderTINTextBox.Enter, ProviderNameTextBox.Enter, ProviderNPITextBox.Enter

        CType(sender, TextBox).SelectAll()
        DoSelectAll = True

    End Sub

#Region "Plug In Code"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' this is an event called from plugin controller to allow any controls to be added
    ''' that will trigger plugins
    ''' </summary>
    ''' <param name="PlugIn"></param>
    ''' <param name="cancel"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	3/24/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub PlugInLoading(ByVal plugIn As SharedInterfaces.PlugInAttribute, ByRef cancel As Boolean) Handles mplug.PlugInLoading
        If plugIn.Destination.ToLower = "provider" Then
            'No Controls needed to triger plugins.  plugins triggered by queue items docclass.
        Else
            cancel = True
        End If
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' plugins inteface this sub to update status messages to display on this form
    ''' </summary>
    ''' <param name="msg"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	3/24/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Sub StatusMessage(ByVal msg As String) Implements SharedInterfaces.IMessage.StatusMessage
        'InfoStatusBarPanel.Text = msg
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' this function maintains open children and limits the number of forms that can open.
    ''' opening too many child forms causes a crash
    ''' </summary>
    ''' <param name="PlugInName"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	3/24/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub OpenProvider(ByVal sender As Object, ByVal providerEvent As ProviderEventArgs)
        Dim Opened As Boolean = False

        Opened = OpenProviderPlugIn("ProviderWork", providerEvent.ProviderID, Nothing)

    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' this function maintains open children and limits the number of forms that can open.
    ''' opening too many child forms causes a crash
    ''' </summary>
    ''' <param name="PlugInName"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	3/24/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub OpenNPI(ByVal sender As Object, ByVal npiEvent As NPIEventArgs)
        Dim Opened As Boolean = False

        Opened = OpenNPIPlugIn("NPIView", npiEvent.NPI)

    End Sub

    Private Function OpenProviderPlugIn(ByVal plugInName As String, ByVal providerID As Integer, ByVal transaction As DbTransaction) As Boolean
        Try
            'Dim Indx As Integer = 0
            Dim UID As String

            'If OpenChildrenCount < MAXFORMS Then
            'If hndls + 150 < 18500 Then
            If GetGuiResourcesGDICount() + 51 <= MAXGUIHANDLES Then

                mProviderfrm = CType(mplug.LaunchPlugIn(plugInName, CType(Me, SharedInterfaces.IMessage), providerID, transaction), ProviderWork)

                If mProviderfrm IsNot Nothing Then
                    AddHandler mProviderfrm.Closed, AddressOf mfrm_Closed
                    AddHandler mProviderfrm.OpenProvider, AddressOf OpenProvider

                    If OpenChildrenCounts.Contains(mProviderfrm.Name) = True Then
                        OpenChildrenCounts.Item(mProviderfrm.Name) = CInt(OpenChildrenCounts.Item(mProviderfrm.Name)) + 1
                    Else
                        OpenChildrenCounts.Add(mProviderfrm.Name, 1)
                    End If

                    OpenChildrenCount += 1
                    TotalOpened += 1

                    Dim P As PropertyInfo
                    P = mProviderfrm.GetType.GetProperty("UniqueID")
                    If Not P Is Nothing Then
                        UID = _UniqueID & "-Q" & TotalOpened
                        mProviderfrm.GetType.InvokeMember("UniqueID", BindingFlags.SetProperty, Nothing, mProviderfrm, New Object() {UID})
                        _Children.Add(UID, mProviderfrm)
                    End If

                    mProviderfrm.Show()

                    Return True
                End If

                Return False
            Else
                MessageBox.Show("There are too many Work Screens Open." & Chr(13) & "Try closing some Work Screens and then open this Document.",
                                "Too Many Screens Open", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            Return False
        End Try
    End Function

    Private Function OpenNPIPlugIn(ByVal plugInName As String, ByVal npi As Decimal) As Boolean
        Try
            'Dim Indx As Integer = 0
            Dim UID As String

            'If OpenChildrenCounts.Contains(PlugInName) = True Then Indx = OpenChildrenCounts.Item(PlugInName)

            'Dim hndls As Integer = GetHandleCount()

            'hndls = 1

            'If OpenChildrenCount < MAXFORMS Then
            'If hndls + 150 < 18500 Then
            If GetGuiResourcesGDICount() + 51 <= MAXGUIHANDLES Then
                'If OpenChildrenCount < MAXWORKFORMS Then
                mNPIfrm = CType(mplug.LaunchPlugIn(plugInName, CType(Me, SharedInterfaces.IMessage), npi), NPIView)

                If Not mNPIfrm Is Nothing Then
                    AddHandler mNPIfrm.Closed, AddressOf mfrm_Closed
                    AddHandler mNPIfrm.OpenNPI, AddressOf OpenNPI

                    If OpenChildrenCounts.Contains(mNPIfrm.Name) = True Then
                        OpenChildrenCounts.Item(mNPIfrm.Name) = CInt(OpenChildrenCounts.Item(mNPIfrm.Name)) + 1
                    Else
                        OpenChildrenCounts.Add(mNPIfrm.Name, 1)
                    End If

                    OpenChildrenCount += 1
                    TotalOpened += 1

                    Dim P As PropertyInfo
                    P = mNPIfrm.GetType.GetProperty("UniqueID")
                    If Not P Is Nothing Then
                        UID = _UniqueID & "-Q" & TotalOpened
                        mNPIfrm.GetType.InvokeMember("UniqueID", BindingFlags.SetProperty, Nothing, mNPIfrm, New Object() {UID})
                        _Children.Add(UID, mNPIfrm)
                    End If

                    mNPIfrm.Show()

                    Return True
                End If

                Return False
            Else
                MessageBox.Show("There are too many NPI Screens Open." & Chr(13) & "Try closing some NPI Screens and then open this Document.",
                                "Too Many Screens Open", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            Return False
        End Try
    End Function

    Public Shared Function GetGuiResourcesGDICount() As Long
        Return NativeMethods.GetGuiResources(Process.GetCurrentProcess.Handle, 0)
    End Function

#End Region

#Region "Customized Form Handling"


    Private Sub OpenButtonUFCW_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UFCWProvidersDataGrid.DoubleClick, NPIProvidersDataGrid.DoubleClick

        Try

            Select Case CType(sender, DataGrid).Name
                Case "UFCWProvidersDataGrid"
                    OpenWorkScreenUFCW(CInt(CType(_ProviderUFCWBS.Current, DataRowView).Row("PROVIDER_ID")), Nothing)
                Case Else
                    OpenWorkScreenNPI(CDec(CType(_ProviderNPIBS.Current, DataRowView).Row("NPI")))
            End Select

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

    Private Sub OpenButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenButton.Click, OpenMenuItem.Click

        Dim DR As DataRow

        Try
            If _ProviderUFCWBS IsNot Nothing AndAlso _ProviderUFCWBS.Position > -1 AndAlso UFCWProvidersDataGrid.IsSelected(_ProviderUFCWBS.Position) Then

                mobjMessage.StatusMessage("Retrieving Item.  Please Wait...")
                Me.Cursor = Cursors.WaitCursor

                DR = CType(_ProviderUFCWBS.Current, DataRowView).Row
                Dim DialogAnswer As New DialogResult

                ReDim DelDocs(0)

                OpenWorkScreenUFCW(CInt(DR("PROVIDER_ID")), Nothing)

                UFCWProvidersDataGrid.UnSelect(_ProviderUFCWBS.Position)

                ReDim Preserve DelDocs(UBound(DelDocs, 1) - 1)

            End If

            If _ProviderNPIBS IsNot Nothing AndAlso _ProviderNPIBS.Position > -1 AndAlso NPIProvidersDataGrid.IsSelected(_ProviderNPIBS.Position) Then

                mobjMessage.StatusMessage("Retrieving Item.  Please Wait...")
                Me.Cursor = Cursors.WaitCursor

                DR = CType(_ProviderNPIBS.Current, DataRowView).Row

                Dim DialogAnswer As New DialogResult

                OpenWorkScreenNPI(CDec(DR("NPI")))

                NPIProvidersDataGrid.UnSelect(_ProviderNPIBS.Position)

            End If

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

    Private Sub NewButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewButton.Click

        Try
            mobjMessage.StatusMessage("Starting Application.  Please Wait...")

            Me.Cursor = Cursors.WaitCursor

            OpenWorkScreenUFCW()

CleanUp:

        Catch ex As Exception
            mobjMessage.StatusMessage("Error Starting application.")
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
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Prevent the user from typing invalid characters.  Non-numeric and not a dash(-)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	11/16/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub ProviderNameTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProviderNameTextBox.TextChanged
        Dim TBox As TextBox = CType(sender, TextBox)
        Dim CursorLocation As Integer = TBox.SelectionStart

        TBox.Text = Regex.Replace(TBox.Text, "[%]", String.Empty)
        TBox.SelectionStart = CursorLocation

    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Prevent the user from typing invalid characters.  Non-numeric and not a dash(-)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	11/16/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub NumericOnlyTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProviderNPITextBox.TextChanged, ProviderTINTextBox.TextChanged
        Dim TBox As TextBox = CType(sender, TextBox)
        Dim CursorLocation As Integer = TBox.SelectionStart

        TBox.Text = Regex.Replace(TBox.Text, "\D", String.Empty)
        If TBox.Text.Trim.Length = 10 Then TBox.Text = TBox.Text.TrimStart("0"c)
        TBox.SelectionStart = CursorLocation

    End Sub

    Private Sub UFCWProvidersDataGrid_Navigate(ByVal sender As System.Object, ByVal e As System.Windows.Forms.NavigateEventArgs) Handles UFCWProvidersDataGrid.Navigate, NPIProvidersDataGrid.Navigate

        CType(sender, DataGrid).Select(CType(sender, DataGridCustom).CurrentCell.RowNumber)

    End Sub

#End Region

End Class

