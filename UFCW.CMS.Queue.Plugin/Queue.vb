Option Strict On

Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data.Common
Imports System.Reflection
Imports System.Security.Principal
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Xml
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports UFCW.WCF

''' -----------------------------------------------------------------------------
''' Project	 : Queue
''' Class	 : Claims.UI.Queue
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
<SharedInterfaces.PlugIn("Queue", 0, "Main", 6)> Public Class WorkQueue
    Inherits System.Windows.Forms.Form
    Implements SharedInterfaces.IMessage

    Private Shared _TraceMessaging As New BooleanSwitch("TraceMessaging", "Trace Switch in App.Config")

    Const MAXWORKFORMS As Integer = 40
    Const MAXGUIHANDLES As Long = 8975933078237088100
    Const PLUINFILTER As String = "UFCW.*.Plugin.dll"

    Const GR_GDIOBJECTS As Integer = 0
    Const GR_USEROBJECTS As Integer = 1

    Delegate Sub RefreshQueueResultsDelegate(queueResultsDT As DataTable)

    ReadOnly _SHOWLOADTIME As Boolean = CBool(CType(ConfigurationManager.GetSection("ShowLoadTime"), IDictionary)("QueueOn"))

    Private ReadOnly _DomainUser As String = SystemInformation.UserName
    Friend _UserPrincipal As WindowsPrincipal

    Private WithEvents _PlugInController As PlugInController

    Private _APPKEY As String = "UFCW\Claims\"
    Private _UniqueID As String

    Private _OpenIndex As Integer
    Private _Form As Form
    Private _QueueSharedInterfacesMessage As SharedInterfaces.IMessage
    Private _Stopwatch As Stopwatch
    Private _QueueDataTable As DataTable
    Private _LastDocClass As String = "MEDICAL"
    Private _LastDocType As String = ""
    Private _LastMode As String = ""
    Private _LastOutMessage As String = ""
    Private _Cancel As Boolean = False
    Private _DoubleClicked As Boolean = False
    Private _OpenChildrenCount As Integer = 0
    Private _TotalOpened As Integer = 0
    Private _OpenChildrenCounts As New Hashtable
    Private _DelDocs() As String

    Private _HoverCell As New DataGridCell
    Private _Children As New Hashtable

    Private _ButtCol As DataGridHighlightButtonColumn
    Private _BoolCol As DataGridHighlightBoolColumn
    Private _IconCol As DataGridHighlightIconColumn
    Private _GenericDelegate As [Delegate]

    Private _QueueBS As New BindingSource
    'Private _WorkTypeBS As BindingSource

    Private _EditableTextBox As Boolean

    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents FamilyIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents HistoryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ClaimHistoryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RecentDocumentsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ClaimHistoryFrm As ClaimHistory

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal sharedInterfaceMessage As SharedInterfaces.IMessage, ByVal openIndex As Integer)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _QueueSharedInterfacesMessage = sharedInterfaceMessage
        _OpenIndex = openIndex

        If Not UFCWGeneralAD.CMSCanCreateClaim Then NewClaimButton.Hide()

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If components IsNot Nothing Then
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
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents BatchNumberTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TINTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents QueueDataGrid As DataGridCustom
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents OpenButton As System.Windows.Forms.Button
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents MyAssignmentsCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents WorkTypeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents RefreshButton As System.Windows.Forms.Button
    Friend WithEvents GetNextButton As System.Windows.Forms.Button
    Friend WithEvents PartSSNTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents ClaimIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents QueueImageList As System.Windows.Forms.ImageList
    Friend WithEvents DocIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents QueueDataGridCustomContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents MaxItemsCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents DisplayMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RecentDocsMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents HistoryMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AnnotationsMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AnnotationsDataSet As AnnotationsDataSet
    Friend WithEvents MaxItemsTextBox As System.Windows.Forms.TextBox
    Friend WithEvents EligibilityMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ModeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents ModeLabel As System.Windows.Forms.Label
    Friend WithEvents AuditSepMenuItem As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ReleaseMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewClaimButton As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim Resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(WorkQueue))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.FamilyIDTextBox = New System.Windows.Forms.TextBox()
        Me.NewClaimButton = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.ClaimIDTextBox = New System.Windows.Forms.TextBox()
        Me.PartSSNTextBox = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.DocIDTextBox = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.BatchNumberTextBox = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.TINTextBox = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ModeComboBox = New System.Windows.Forms.ComboBox()
        Me.OpenButton = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.MaxItemsTextBox = New System.Windows.Forms.TextBox()
        Me.MaxItemsCheckBox = New System.Windows.Forms.CheckBox()
        Me.MyAssignmentsCheckBox = New System.Windows.Forms.CheckBox()
        Me.WorkTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.RefreshButton = New System.Windows.Forms.Button()
        Me.GetNextButton = New System.Windows.Forms.Button()
        Me.ModeLabel = New System.Windows.Forms.Label()
        Me.QueueImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.QueueDataGridCustomContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.DisplayMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RecentDocsMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.HistoryMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AnnotationsMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EligibilityMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AuditSepMenuItem = New System.Windows.Forms.ToolStripSeparator()
        Me.ReleaseMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.QueueDataGrid = New DataGridCustom()
        Me.AnnotationsDataSet = New AnnotationsDataSet()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.HistoryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ClaimHistoryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RecentDocumentsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.QueueDataGridCustomContextMenu.SuspendLayout()
        CType(Me.QueueDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AnnotationsDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.FamilyIDTextBox)
        Me.Panel1.Controls.Add(Me.NewClaimButton)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.ClaimIDTextBox)
        Me.Panel1.Controls.Add(Me.PartSSNTextBox)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.DocIDTextBox)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.BatchNumberTextBox)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.TINTextBox)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 250)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(895, 36)
        Me.Panel1.TabIndex = 24
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(279, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(50, 13)
        Me.Label2.TabIndex = 21
        Me.Label2.Text = "Family ID"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FamilyIDTextBox
        '
        Me.FamilyIDTextBox.Location = New System.Drawing.Point(333, 12)
        Me.FamilyIDTextBox.MaxLength = 9
        Me.FamilyIDTextBox.Name = "FamilyIDTextBox"
        Me.FamilyIDTextBox.Size = New System.Drawing.Size(72, 20)
        Me.FamilyIDTextBox.TabIndex = 22
        '
        'NewClaimButton
        '
        Me.NewClaimButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NewClaimButton.Location = New System.Drawing.Point(812, 9)
        Me.NewClaimButton.Name = "NewClaimButton"
        Me.NewClaimButton.Size = New System.Drawing.Size(75, 23)
        Me.NewClaimButton.TabIndex = 20
        Me.NewClaimButton.Text = "New"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(3, 15)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(46, 13)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "Claim ID"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ClaimIDTextBox
        '
        Me.ClaimIDTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ClaimIDTextBox.Location = New System.Drawing.Point(53, 12)
        Me.ClaimIDTextBox.MaxLength = 15
        Me.ClaimIDTextBox.Name = "ClaimIDTextBox"
        Me.ClaimIDTextBox.Size = New System.Drawing.Size(102, 20)
        Me.ClaimIDTextBox.TabIndex = 15
        '
        'PartSSNTextBox
        '
        Me.PartSSNTextBox.Location = New System.Drawing.Point(467, 12)
        Me.PartSSNTextBox.MaxLength = 11
        Me.PartSSNTextBox.Name = "PartSSNTextBox"
        Me.PartSSNTextBox.Size = New System.Drawing.Size(72, 20)
        Me.PartSSNTextBox.TabIndex = 18
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(158, 15)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(41, 13)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Doc ID"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'DocIDTextBox
        '
        Me.DocIDTextBox.Location = New System.Drawing.Point(203, 12)
        Me.DocIDTextBox.MaxLength = 9
        Me.DocIDTextBox.Name = "DocIDTextBox"
        Me.DocIDTextBox.Size = New System.Drawing.Size(72, 20)
        Me.DocIDTextBox.TabIndex = 16
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(644, 15)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(35, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Batch"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'BatchNumberTextBox
        '
        Me.BatchNumberTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BatchNumberTextBox.Location = New System.Drawing.Point(683, 12)
        Me.BatchNumberTextBox.MaxLength = 500
        Me.BatchNumberTextBox.Name = "BatchNumberTextBox"
        Me.BatchNumberTextBox.Size = New System.Drawing.Size(123, 20)
        Me.BatchNumberTextBox.TabIndex = 17
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(543, 15)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(25, 13)
        Me.Label6.TabIndex = 9
        Me.Label6.Text = "TIN"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TINTextBox
        '
        Me.TINTextBox.Location = New System.Drawing.Point(572, 12)
        Me.TINTextBox.MaxLength = 10
        Me.TINTextBox.Name = "TINTextBox"
        Me.TINTextBox.Size = New System.Drawing.Size(68, 20)
        Me.TINTextBox.TabIndex = 19
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(409, 15)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(54, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Part. SSN"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox1.Controls.Add(Me.ModeComboBox)
        Me.GroupBox1.Controls.Add(Me.OpenButton)
        Me.GroupBox1.Controls.Add(Me.GroupBox3)
        Me.GroupBox1.Controls.Add(Me.WorkTypeComboBox)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.RefreshButton)
        Me.GroupBox1.Controls.Add(Me.GetNextButton)
        Me.GroupBox1.Controls.Add(Me.ModeLabel)
        Me.GroupBox1.Location = New System.Drawing.Point(4, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(883, 72)
        Me.GroupBox1.TabIndex = 22
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Work Filters"
        '
        'ModeComboBox
        '
        Me.ModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ModeComboBox.Items.AddRange(New Object() {"REGULAR", "AUDIT"})
        Me.ModeComboBox.Location = New System.Drawing.Point(280, 32)
        Me.ModeComboBox.Name = "ModeComboBox"
        Me.ModeComboBox.Size = New System.Drawing.Size(82, 21)
        Me.ModeComboBox.TabIndex = 10
        '
        'OpenButton
        '
        Me.OpenButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OpenButton.Location = New System.Drawing.Point(803, 32)
        Me.OpenButton.Name = "OpenButton"
        Me.OpenButton.Size = New System.Drawing.Size(75, 23)
        Me.OpenButton.TabIndex = 6
        Me.OpenButton.Text = "Open"
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.MaxItemsTextBox)
        Me.GroupBox3.Controls.Add(Me.MaxItemsCheckBox)
        Me.GroupBox3.Controls.Add(Me.MyAssignmentsCheckBox)
        Me.GroupBox3.Location = New System.Drawing.Point(383, 16)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(256, 48)
        Me.GroupBox3.TabIndex = 3
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Display Filters"
        '
        'MaxItemsTextBox
        '
        Me.MaxItemsTextBox.Location = New System.Drawing.Point(192, 16)
        Me.MaxItemsTextBox.Name = "MaxItemsTextBox"
        Me.MaxItemsTextBox.Size = New System.Drawing.Size(60, 20)
        Me.MaxItemsTextBox.TabIndex = 2
        '
        'MaxItemsCheckBox
        '
        Me.MaxItemsCheckBox.Location = New System.Drawing.Point(120, 20)
        Me.MaxItemsCheckBox.Name = "MaxItemsCheckBox"
        Me.MaxItemsCheckBox.Size = New System.Drawing.Size(76, 16)
        Me.MaxItemsCheckBox.TabIndex = 1
        Me.MaxItemsCheckBox.Text = "Max Items"
        '
        'MyAssignmentsCheckBox
        '
        Me.MyAssignmentsCheckBox.Location = New System.Drawing.Point(16, 16)
        Me.MyAssignmentsCheckBox.Name = "MyAssignmentsCheckBox"
        Me.MyAssignmentsCheckBox.Size = New System.Drawing.Size(116, 24)
        Me.MyAssignmentsCheckBox.TabIndex = 0
        Me.MyAssignmentsCheckBox.Text = "My Assignments"
        '
        'WorkTypeComboBox
        '
        Me.WorkTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        '  Me.WorkTypeComboBox.Items.AddRange(New Object() {"Medical 2000+"})
        Me.WorkTypeComboBox.Location = New System.Drawing.Point(24, 32)
        Me.WorkTypeComboBox.Name = "WorkTypeComboBox"
        Me.WorkTypeComboBox.Size = New System.Drawing.Size(244, 21)
        Me.WorkTypeComboBox.TabIndex = 1
        Me.WorkTypeComboBox.DropDownHeight = 150
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(16, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 23)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Type of Work"
        '
        'RefreshButton
        '
        Me.RefreshButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RefreshButton.Location = New System.Drawing.Point(643, 32)
        Me.RefreshButton.Name = "RefreshButton"
        Me.RefreshButton.Size = New System.Drawing.Size(75, 23)
        Me.RefreshButton.TabIndex = 4
        Me.RefreshButton.Text = "Refresh"
        '
        'GetNextButton
        '
        Me.GetNextButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GetNextButton.Location = New System.Drawing.Point(723, 32)
        Me.GetNextButton.Name = "GetNextButton"
        Me.GetNextButton.Size = New System.Drawing.Size(75, 23)
        Me.GetNextButton.TabIndex = 5
        Me.GetNextButton.Text = "Get Next"
        '
        'ModeLabel
        '
        Me.ModeLabel.Location = New System.Drawing.Point(272, 16)
        Me.ModeLabel.Name = "ModeLabel"
        Me.ModeLabel.Size = New System.Drawing.Size(52, 23)
        Me.ModeLabel.TabIndex = 11
        Me.ModeLabel.Text = "Mode"
        '
        'QueueImageList
        '
        Me.QueueImageList.ImageStream = CType(Resources.GetObject("QueueImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.QueueImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.QueueImageList.Images.SetKeyName(0, "")
        Me.QueueImageList.Images.SetKeyName(1, "")
        Me.QueueImageList.Images.SetKeyName(2, "")
        Me.QueueImageList.Images.SetKeyName(3, "")
        Me.QueueImageList.Images.SetKeyName(4, "")
        Me.QueueImageList.Images.SetKeyName(5, "")
        Me.QueueImageList.Images.SetKeyName(6, "")
        Me.QueueImageList.Images.SetKeyName(7, "")
        Me.QueueImageList.Images.SetKeyName(8, "")
        Me.QueueImageList.Images.SetKeyName(9, "")
        Me.QueueImageList.Images.SetKeyName(10, "")
        Me.QueueImageList.Images.SetKeyName(11, "")
        Me.QueueImageList.Images.SetKeyName(12, "")
        Me.QueueImageList.Images.SetKeyName(13, "")
        Me.QueueImageList.Images.SetKeyName(14, "")
        Me.QueueImageList.Images.SetKeyName(15, "")
        Me.QueueImageList.Images.SetKeyName(16, "")
        Me.QueueImageList.Images.SetKeyName(17, "")
        Me.QueueImageList.Images.SetKeyName(18, "")
        Me.QueueImageList.Images.SetKeyName(19, "")
        Me.QueueImageList.Images.SetKeyName(20, "")
        Me.QueueImageList.Images.SetKeyName(21, "")
        Me.QueueImageList.Images.SetKeyName(22, "")
        Me.QueueImageList.Images.SetKeyName(23, "")
        Me.QueueImageList.Images.SetKeyName(24, "")
        '
        'QueueDataGridCustomContextMenu
        '
        Me.QueueDataGridCustomContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DisplayMenuItem, Me.RecentDocsMenuItem, Me.MenuItem1, Me.HistoryMenuItem, Me.AnnotationsMenuItem, Me.EligibilityMenuItem, Me.AuditSepMenuItem, Me.ReleaseMenuItem})
        Me.QueueDataGridCustomContextMenu.Name = "DataGridCustomContextMenu"
        Me.QueueDataGridCustomContextMenu.Size = New System.Drawing.Size(181, 148)
        '
        'DisplayMenuItem
        '
        Me.DisplayMenuItem.Name = "DisplayMenuItem"
        Me.DisplayMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
        Me.DisplayMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.DisplayMenuItem.Text = "&Display"
        '
        'RecentDocsMenuItem
        '
        Me.RecentDocsMenuItem.Name = "RecentDocsMenuItem"
        Me.RecentDocsMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.R), System.Windows.Forms.Keys)
        Me.RecentDocsMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.RecentDocsMenuItem.Text = "&Recent Docs"
        '
        'MenuItem1
        '
        Me.MenuItem1.Name = "MenuItem1"
        Me.MenuItem1.Size = New System.Drawing.Size(177, 6)
        '
        'HistoryMenuItem
        '
        Me.HistoryMenuItem.Name = "HistoryMenuItem"
        Me.HistoryMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.H), System.Windows.Forms.Keys)
        Me.HistoryMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.HistoryMenuItem.Text = "&History"
        '
        'AnnotationsMenuItem
        '
        Me.AnnotationsMenuItem.Name = "AnnotationsMenuItem"
        Me.AnnotationsMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.AnnotationsMenuItem.Text = "&Annotations"
        '
        'EligibilityMenuItem
        'SaveColumnsSizeAndPosition
        Me.EligibilityMenuItem.Name = "EligibilityMenuItem"
        Me.EligibilityMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.EligibilityMenuItem.Text = "&Eligibility"
        '
        'AuditSepMenuItem
        '
        Me.AuditSepMenuItem.Name = "AuditSepMenuItem"
        Me.AuditSepMenuItem.Size = New System.Drawing.Size(177, 6)
        '
        'ReleaseMenuItem
        '
        Me.ReleaseMenuItem.Name = "ReleaseMenuItem"
        Me.ReleaseMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.ReleaseMenuItem.Text = "Relea&se"
        '
        'QueueDataGrid
        '
        Me.QueueDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.QueueDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.QueueDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.QueueDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.QueueDataGrid.ADGroupsThatCanFind = ""
        Me.QueueDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.QueueDataGrid.ADGroupsThatCanMultiSort = ""
        Me.QueueDataGrid.AllowAutoSize = True
        Me.QueueDataGrid.AllowColumnReorder = True
        Me.QueueDataGrid.AllowCopy = True
        Me.QueueDataGrid.AllowCustomize = True
        Me.QueueDataGrid.AllowDelete = False
        Me.QueueDataGrid.AllowDragDrop = False
        Me.QueueDataGrid.AllowEdit = False
        Me.QueueDataGrid.AllowExport = True
        Me.QueueDataGrid.AllowFilter = True
        Me.QueueDataGrid.AllowFind = True
        Me.QueueDataGrid.AllowGoTo = True
        Me.QueueDataGrid.AllowMultiSelect = False
        Me.QueueDataGrid.AllowMultiSort = True
        Me.QueueDataGrid.AllowNew = False
        Me.QueueDataGrid.AllowPrint = True
        Me.QueueDataGrid.AllowRefresh = False
        Me.QueueDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.QueueDataGrid.AppKey = "UFCW\Claims\"
        Me.QueueDataGrid.BackgroundColor = System.Drawing.SystemColors.Control
        Me.QueueDataGrid.CaptionText = "0 - Pending Work Items In "
        Me.QueueDataGrid.ColumnHeaderLabel = Nothing
        Me.QueueDataGrid.ColumnRePositioning = False
        Me.QueueDataGrid.ColumnResizing = False
        Me.QueueDataGrid.ConfirmDelete = True
        Me.QueueDataGrid.ContextMenuStrip = Me.QueueDataGridCustomContextMenu
        Me.QueueDataGrid.CopySelectedOnly = True
        Me.QueueDataGrid.DataMember = ""
        Me.QueueDataGrid.DragColumn = 0
        Me.QueueDataGrid.ExportSelectedOnly = True
        Me.QueueDataGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.QueueDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.QueueDataGrid.HighlightedRow = Nothing
        Me.QueueDataGrid.IsMouseDown = False
        Me.QueueDataGrid.LastGoToLine = ""
        Me.QueueDataGrid.Location = New System.Drawing.Point(4, 80)
        Me.QueueDataGrid.MultiSort = False
        Me.QueueDataGrid.Name = "QueueDataGrid"
        Me.QueueDataGrid.OldSelectedRow = Nothing
        Me.QueueDataGrid.ReadOnly = True
        Me.QueueDataGrid.SetRowOnRightClick = True
        Me.QueueDataGrid.ShiftPressed = False
        Me.QueueDataGrid.SingleClickBooleanColumns = True
        Me.QueueDataGrid.Size = New System.Drawing.Size(883, 168)
        Me.QueueDataGrid.StyleName = ""
        Me.QueueDataGrid.SubKey = ""
        Me.QueueDataGrid.SuppressTriangle = False
        Me.QueueDataGrid.TabIndex = 20
        '
        'AnnotationsDataSet
        '
        Me.AnnotationsDataSet.DataSetName = "AnnotationsDataSet"
        Me.AnnotationsDataSet.Locale = New System.Globalization.CultureInfo("en-US")
        Me.AnnotationsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HistoryToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(895, 24)
        Me.MenuStrip1.TabIndex = 25
        Me.MenuStrip1.Text = "MenuStrip1"
        Me.MenuStrip1.Visible = False
        '
        'HistoryToolStripMenuItem
        '
        Me.HistoryToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ClaimHistoryToolStripMenuItem, Me.RecentDocumentsToolStripMenuItem})
        Me.HistoryToolStripMenuItem.Name = "HistoryToolStripMenuItem"
        Me.HistoryToolStripMenuItem.Size = New System.Drawing.Size(57, 20)
        Me.HistoryToolStripMenuItem.Text = "History"
        '
        'ClaimHistoryToolStripMenuItem
        '
        Me.ClaimHistoryToolStripMenuItem.Name = "ClaimHistoryToolStripMenuItem"
        Me.ClaimHistoryToolStripMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.ClaimHistoryToolStripMenuItem.Text = "Recent Claim(s)"
        '
        'RecentDocumentsToolStripMenuItem
        '
        Me.RecentDocumentsToolStripMenuItem.Name = "RecentDocumentsToolStripMenuItem"
        Me.RecentDocumentsToolStripMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.RecentDocumentsToolStripMenuItem.Text = "Recent Document(s)"
        '
        'WorkQueue
        '
        Me.AcceptButton = Me.RefreshButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(895, 286)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.QueueDataGrid)
        Me.Controls.Add(Me.GroupBox1)
        Me.Icon = CType(Resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MinimumSize = New System.Drawing.Size(896, 300)
        Me.Name = "WorkQueue"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Work Queue"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.QueueDataGridCustomContextMenu.ResumeLayout(False)
        CType(Me.QueueDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AnnotationsDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region "Public Properties"
    <System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)

            _APPKEY = value
        End Set
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' this property gets or sets a unique id suppilied by the calling form
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	3/24/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property UniqueID() As String
        Get
            Return _UniqueID
        End Get
        Set(ByVal value As String)
            _UniqueID = value
        End Set
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' this property allows access to child forms
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	3/24/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public ReadOnly Property Children() As Hashtable
        Get
            Return _Children
        End Get
    End Property

#End Region

#Region "Form Events"

    Private Sub WorkQueue_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Try
            RemoveChildrenHandlers()
        Catch IgnoreException As Exception
            Throw
        End Try
    End Sub

    Private Sub Queue_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            SetSettings()

            Me.ControlBox = False

            If _OpenIndex <> 0 Then Me.Text = "Work Queue - " & _OpenIndex

            Me.Show()

            _QueueSharedInterfacesMessage.StatusMessage("Logging On. Please Wait...")

            'Init Active Directory
            AppDomain.CurrentDomain.SetPrincipalPolicy(System.Security.Principal.PrincipalPolicy.WindowsPrincipal)
            _UserPrincipal = CType(System.Threading.Thread.CurrentPrincipal, WindowsPrincipal)

            ApplyUserSecurity()

            'Plugins
            _PlugInController = New PlugInController(PLUINFILTER)
            _PlugInController.LoadPlugIns()

            QueueDataGrid.CaptionText = ""
            MyAssignmentsCheckBox.Checked = True

            _QueueSharedInterfacesMessage.StatusMessage("Loading User Work Types.  Please Wait...")

            Me.Refresh()

            RemoveHandler ModeComboBox.SelectedIndexChanged, AddressOf ModeComboBox_SelectedIndexChanged 'Prevent DocTypes XML from causing problems

            LoadDocTypes()

            WorkTypeComboBox.SelectedIndex = 0

            If ModeComboBox.Text = "" Then
                ModeComboBox.SelectedIndex = 0
            End If

            If _OpenIndex = 0 Then RefreshQueue()

            CMSDALFDBMD.ClearUserMachineClaimLocks(_DomainUser.ToUpper, SystemInformation.ComputerName)

            _LastOutMessage = Me.Text + " Loaded"
            _QueueSharedInterfacesMessage.StatusMessage(_LastOutMessage)

            Me.ControlBox = True

        Catch ex As Exception
            Me.ControlBox = True
            Throw
        Finally
            AddHandler ModeComboBox.SelectedIndexChanged, AddressOf ModeComboBox_SelectedIndexChanged
        End Try
    End Sub

    Private Sub WorkQueue_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            SaveSettings()
        Catch IgnoreException As Exception
            Throw
        End Try
    End Sub

    Private Sub RemoveChildrenHandlers()
        Dim ChildFrm As Form
        Dim GEEnum As Collections.IDictionaryEnumerator

        Try
            GEEnum = _Children.GetEnumerator
            GEEnum.Reset()

            For Cnt As Integer = 0 To _Children.Values.Count - 1
                GEEnum.MoveNext()

                ChildFrm = CType(GEEnum.Entry.Value, Form)

                RemoveHandler ChildFrm.FormClosing, AddressOf PlugInForm_FormClosing
            Next

        Catch ex As Exception
            Throw
        Finally

            If ChildFrm IsNot Nothing Then ChildFrm.Dispose()
            ChildFrm = Nothing
            GEEnum = Nothing
        End Try
    End Sub

    Private Sub PlugInForm_FormClosing(ByVal sender As Object, ByVal e As EventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' this is called when a plugin closes.
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim Frm As Form
        Dim UIDPI As PropertyInfo
        Dim UID As String = ""

        Try

            If TypeOf e Is System.Windows.Forms.FormClosingEventArgs AndAlso Not CType(e, System.Windows.Forms.FormClosingEventArgs).Cancel Then

                Frm = CType(sender, Form)

                If _OpenChildrenCounts.Count > 0 Then
                    If _OpenChildrenCounts.Contains(CType(sender, Form).Name) = True Then
                        _OpenChildrenCounts.Item(CType(sender, Form).Name) = CInt(_OpenChildrenCounts.Item(CType(sender, Form).Name)) - 1
                    End If
                End If

                UIDPI = Frm.GetType.GetProperty("UniqueID")
                If UIDPI IsNot Nothing Then
                    UID = CStr(Frm.GetType.InvokeMember("UniqueID", BindingFlags.GetProperty, Nothing, Frm, New Object() {}))

                    RemoveHandler Frm.FormClosing, AddressOf PlugInForm_FormClosing

                    If _Children.ContainsKey(UID) Then
                        _Children.Remove(UID)
                    End If
                End If

                _OpenChildrenCount -= 1

                If _OpenChildrenCount = 0 Then
                    Display.Hide()
                End If
            End If

        Catch ex As Exception
            Throw
        Finally

            If Frm IsNot Nothing Then
                Frm.Dispose()
            End If
            Frm = Nothing

        End Try

    End Sub

    Private Sub Queue_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Enter
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' resets the status message when this form gets focus
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        If _LastOutMessage <> "" Then
            _QueueSharedInterfacesMessage.StatusMessage(_LastOutMessage)
        Else
            _LastOutMessage = Me.Text + " Ready"
            _QueueSharedInterfacesMessage.StatusMessage(_LastOutMessage)
        End If
    End Sub

    Private Sub SetSettings()

        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Sets the basic form settings.  Windowstate, height, width, top, and left.
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	11/16/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Me.Top = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)))
        Me.Height = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)))
        Me.Width = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)

        ModeComboBox.Text = GetSetting(_APPKEY, Me.Name & "\Settings", "LastMode", ModeComboBox.Text)
    End Sub

    Private Sub SaveSettings()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Saves the basic form settings.  Windowstate, height, width, top, and left.
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	11/16/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim WindowState As FormWindowState = Me.WindowState
        SaveSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(WindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = WindowState

        SaveSetting(_APPKEY, Me.Name & "\Settings", "LastMode", ModeComboBox.Text)

        Me.Opacity = 100
        SaveColSettings()
    End Sub

    Private Sub SaveColSettings()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Saves settings for the grid columns
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	2/15/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            If QueueDataGrid IsNot Nothing Then
                If QueueDataGrid.DataSource IsNot Nothing Then
                    QueueDataGrid.SaveColumnsSizeAndPosition(_APPKEY & "\" & QueueDataGrid.Name & "\" & QueueDataGrid.GetCurrentDataTable.TableName & "\ColumnSettings")
                End If
            End If
        Catch ex As Exception
            Throw
        End Try

    End Sub
#End Region

#Region "Menu\Button Events"
    Private Sub RefreshButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshButton.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Loads the Queue Grid
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	2/15/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            If RefreshButton.Text = "&Cancel" Then
                _Cancel = True
            Else
                If (Me.MyAssignmentsCheckBox.CheckState = CheckState.Checked OrElse ClaimIDTextBox.Text.ToString.Trim.Length > 0 OrElse DocIDTextBox.Text.ToString.Trim.Length > 0 OrElse BatchNumberTextBox.Text.ToString.Trim.Length > 0 OrElse PartSSNTextBox.Text.ToString.Trim.Length > 0 OrElse TINTextBox.Text.ToString.Trim.Length > 0 OrElse FamilyIDTextBox.Text.ToString.Trim.Length > 0) OrElse (MessageBox.Show("Refreshing items without 'My Assignments' checked may take an extended period." & vbCrLf & "Do You want to continue?", "My Assigments is not selected", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes) Then
                    RefreshQueue()
                End If
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub QueueDataGrid_RefreshGridData() Handles QueueDataGrid.RefreshGridData
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Loads the Queue Grid
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	4/3/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            RefreshQueue()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub GetNextButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GetNextButton.Click
        Try
            GetNextButton.Enabled = False

            ' -----------------------------------------------------------------------------
            ' <summary>
            ' Gets the next availible item from the Queue
            ' </summary>
            ' <param name="sender"></param>
            ' <param name="e"></param>
            ' <remarks>
            ' </remarks>
            ' <history>
            ' 	[Nick Snyder]	2/15/2006	Created
            ' </history>
            ' -----------------------------------------------------------------------------
            Dim DocClass As String = "MEDICAL"

            Dim DocType As String = ""
            If WorkTypeComboBox.Text <> "" AndAlso WorkTypeComboBox.Text.ToUpper <> "(ALL)" Then
                DocType = WorkTypeComboBox.Text.TrimEnd
            Else
                MessageBox.Show("A Specific Type Of Work Must Be Selected First.", "Choose Work Type", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            If _OpenChildrenCount >= MAXWORKFORMS Then
                MessageBox.Show("There are too many Work Screens Open." & Chr(13) & "Try closing some Work Screens and then Get A New Item.",
                                "Too Many Screens Open", MessageBoxButtons.OK, MessageBoxIcon.Warning)

                Exit Sub
            End If

            GetNextItem(DocClass, DocType)

        Catch ex As Exception
            Throw
        Finally

            GetNextButton.Enabled = True

        End Try
    End Sub

    Private Sub OpenButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenButton.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Opens Selected items in the Grid from the Queue
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	2/15/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim DialogAnswer As DialogResult
        Dim DR As DataRow
        Try

            DialogAnswer = New DialogResult

            ReDim _DelDocs(0)

            If QueueDataGrid.GetGridRowCount > 0 Then
                _QueueSharedInterfacesMessage.StatusMessage("Retrieving Item.  Please Wait...")
                DR = DirectCast(_QueueBS.Current, DataRowView).Row
                If DR Is Nothing OrElse DR("CLAIM_ID") Is DBNull.Value Then Return

                Using WC As New GlobalCursor
                    OpenButton.Enabled = False
                    If OpenWorkScreen(_LastMode.ToUpper, _LastDocClass, CInt(DR("CLAIM_ID")), False, False, Nothing) Then
                        CMSDALFDBMD.WriteToClaimsHistoryXML(CInt(DR("CLAIM_ID")), UFCWGeneral.IsNullIntegerHandler(DR("PART_SSN")), UFCWGeneral.IsNullLongHandler(DR("DOCID")), UFCWGeneral.IsNullIntegerHandler(DR("FAMILY_ID")), Now(), "Opened")
                        _DelDocs(UBound(_DelDocs, 1)) = DR("CLAIM_ID").ToString
                        ReDim Preserve _DelDocs(UBound(_DelDocs, 1) + 1)
                    Else
                        If _OpenChildrenCount >= MAXWORKFORMS Then GoTo CleanUp
                    End If

                    'If Doclist.Count = 0 Then
                    '    If OpenWorkScreen(_LastMode.ToUpper, _LastDocClass, CInt(DR("CLAIM_ID")), False, False, Nothing) Then
                    '        CMSDALFDBMD.WriteToClaimsHistoryXML(CInt(DR("CLAIM_ID")), UFCWGeneral.IsNullIntegerHandler(DR("PART_SSN")), UFCWGeneral.IsNullLongHandler(DR("DOCID")), UFCWGeneral.IsNullIntegerHandler(DR("FAMILY_ID")), Now(), "Opened")
                    '        _DelDocs(UBound(_DelDocs, 1)) = DR("CLAIM_ID").ToString
                    '        ReDim Preserve _DelDocs(UBound(_DelDocs, 1) + 1)
                    '    End If

                    'ElseIf Doclist.Count = 1 Then
                    '    DR = DirectCast(Doclist.Item(0), DataRow)
                    '    If OpenWorkScreen(_LastMode.ToUpper, _LastDocClass, CInt(DR("CLAIM_ID").ToString), False, False, Nothing) Then
                    '        CMSDALFDBMD.WriteToClaimsHistoryXML(CInt(DR("CLAIM_ID")), UFCWGeneral.IsNullIntegerHandler(DR("PART_SSN")), UFCWGeneral.IsNullLongHandler(DR("DOCID")), UFCWGeneral.IsNullIntegerHandler(DR("FAMILY_ID")), Now(), "Opened")
                    '        _DelDocs(UBound(_DelDocs, 1)) = DR("CLAIM_ID").ToString
                    '        ReDim Preserve _DelDocs(UBound(_DelDocs, 1) + 1)
                    '    End If

                    'ElseIf Doclist.Count >= 2 Then
                    '    For Each DR In Doclist
                    '        If OpenWorkScreen(_LastMode.ToUpper, _LastDocClass, CInt(DR("CLAIM_ID").ToString), False, True, DialogAnswer) Then

                    '            CMSDALFDBMD.WriteToClaimsHistoryXML(CInt(DR("CLAIM_ID")), UFCWGeneral.IsNullIntegerHandler(DR("PART_SSN")), UFCWGeneral.IsNullLongHandler(DR("DOCID")), UFCWGeneral.IsNullIntegerHandler(DR("FAMILY_ID")), Now(), "Opened")

                    '            _DelDocs(UBound(_DelDocs, 1)) = DR("CLAIM_ID").ToString
                    '            ReDim Preserve _DelDocs(UBound(_DelDocs, 1) + 1)

                    '            OpenCount += 1
                    '        Else
                    '            If _OpenChildrenCount >= MAXWORKFORMS Then Exit For
                    '        End If
                    '    Next
                    'End If

CleanUp:
                    ReDim Preserve _DelDocs(UBound(_DelDocs, 1) - 1)

                    DeleteQueueItems()
                End Using
            End If

        Catch ex As Exception
            _QueueSharedInterfacesMessage.StatusMessage("Open Error")
            Throw
        Finally
            _QueueSharedInterfacesMessage.StatusMessage("")
            OpenButton.Enabled = True

        End Try
    End Sub

    Private Sub DisplayMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DisplayMenuItem.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Calls to Display the FileNet Image in the IDM Viewer
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	2/27/2006	Created
        ' 	Lalitha Moparthi  11/29/2022 updated
        ' </history>
        ' -----------------------------------------------------------------------------
        If QueueDataGrid.GetGridRowCount = 0 Then Exit Sub

        Dim Docs As New List(Of Long?)
        Dim Doclist As ArrayList

        Try

            Using WC As New GlobalCursor

                Doclist = QueueDataGrid.GetSelectedDataRows()
                For Each DR As DataRow In Doclist
                    If IsDBNull(DR("DOCID")) = False Then
                        Docs.Add(CLng(DR("DOCID")))
                    End If
                Next

                If Docs.Count > 0 Then
                    Using FNDisplay As New Display
                        FNDisplay.Display(Docs)
                    End Using
                End If
            End Using

        Catch ex As Exception
            Throw
        Finally

        End Try
    End Sub
    Private Sub QueueDataGrid_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles QueueDataGrid.DoubleClick
        Try
            _DoubleClicked = True

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub QueueDataGrid_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles QueueDataGrid.MouseUp
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' this sub opens work items on a double click
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DG As DataGridCustom
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo
        Dim DR As DataRow
        Dim BS As BindingSource
        Try
            DG = CType(sender, DataGridCustom)
            HTI = DG.HitTest(e.X, e.Y)

            BS = DirectCast(DG.DataSource, BindingSource)
            If BS IsNot Nothing AndAlso BS.Position > -1 Then
                DR = DirectCast(BS.Current, DataRowView).Row
            Else
                Exit Sub
            End If


            If QueueDataGrid.GetGridRowCount > 0 Then
                Select Case HTI.Type
                    Case Is = System.Windows.Forms.DataGrid.HitTestType.None

                    Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell
                        BS.Position = HTI.Row
                    Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader
                        BS.Position = HTI.Row

                        If _DoubleClicked Then

                            _QueueSharedInterfacesMessage.StatusMessage("Retrieving Item.  Please Wait...")

                            Using WC As New GlobalCursor

                                If DR IsNot Nothing AndAlso Not IsDBNull(DR("CLAIM_ID")) Then
                                    If OpenWorkScreen(_LastMode.ToUpper, _LastDocClass, CInt(DR("CLAIM_ID")), False, False, Nothing) Then

                                        CMSDALFDBMD.WriteToClaimsHistoryXML(CInt(DR("CLAIM_ID")), UFCWGeneral.IsNullIntegerHandler(DR("PART_SSN")), UFCWGeneral.IsNullLongHandler(DR("DOCID")), UFCWGeneral.IsNullIntegerHandler(DR("FAMILY_ID")), Now(), "Opened")
                                        ReDim _DelDocs(0)
                                        _DelDocs(UBound(_DelDocs, 1)) = DR("CLAIM_ID").ToString
                                        DeleteQueueItems()
                                    End If
                                End If
                            End Using


                            _QueueSharedInterfacesMessage.StatusMessage("")
                        End If

                    Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize

                    Case Is = System.Windows.Forms.DataGrid.HitTestType.RowResize

                    Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption
                        BS.Position = 0
                    Case Is = System.Windows.Forms.DataGrid.HitTestType.ParentRows

                End Select
            End If
        Catch ex As Exception
            _QueueSharedInterfacesMessage.StatusMessage("Open Error")
            Throw
        Finally
            _DoubleClicked = False
        End Try
    End Sub

    Private Sub RecentDocsMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RecentDocsMenuItem.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' makes a call to display the recent documents form of the FileNet Display
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/14/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DisplayRecentDocuments As RecentDocsViewer

        Try
            DisplayRecentDocuments = New RecentDocsViewer
            DisplayRecentDocuments.Show(Me)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub HistoryMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HistoryMenuItem.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' opens up a document history viewer
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	10/19/2006	Created
        ' Lalitha Moparthi 11/29/20222 Updated
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim QHist As QueueHistory
        Dim DR As DataRow

        Try

            DR = DirectCast(_QueueBS.Current, DataRowView).Row
            If DR Is Nothing OrElse DR("CLAIM_ID") Is DBNull.Value Then Return

            AnnotationsDataSet.ANNOTATIONS.Rows.Clear()
            AnnotationsDataSet = CType(CMSDALFDBMD.RetrieveClaimAnnotations(CInt(DR("CLAIM_ID")), AnnotationsDataSet), AnnotationsDataSet)

            QHist = New QueueHistory(UFCWGeneral.IsNullStringHandler(DR("DOC_CLASS")),
                                                              CInt(DR("CLAIM_ID")),
                                                              CInt(DR("FAMILY_ID")),
                                                              CInt(DR("RELATION_ID")),
                                                              If(IsDBNull(DR("PART_SSN")), 0, CInt(DR("PART_SSN"))),
                                                              If(IsDBNull(DR("PAT_SSN")), 0, CInt(DR("PAT_SSN"))),
                                                              UFCWGeneral.IsNullStringHandler(DR("PART_FNAME")),
                                                               UFCWGeneral.IsNullStringHandler(DR("PART_LNAME")),
                                                               UFCWGeneral.IsNullStringHandler(DR("PAT_FNAME")),
                                                               UFCWGeneral.IsNullStringHandler(DR("PAT_LNAME")),
                                                              AnnotationsDataSet.ANNOTATIONS)


            QHist.ShowDialog()

        Catch ex As Exception
            Throw
        Finally
            If QHist IsNot Nothing Then
                QHist.Dispose()
            End If
        End Try
    End Sub

    Private Sub CreateAnnotations(ByVal sender As System.Object, ByVal e As AnnotationsEvent)
        Dim Transaction As DbTransaction
        Dim DT As DataTable
        Try
            If e.AnnotationsTable Is Nothing Then Exit Sub

            DT = e.AnnotationsTable.GetChanges(DataRowState.Added)

            If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
                Transaction = CMSDALCommon.BeginTransaction

                For Each DR As DataRow In DT.Rows

                    CMSDALFDBMD.CreateAnnotation(CInt(DR("CLAIM_ID")), CInt(DR("FAMILY_ID")), CShort(DR("RELATION_ID")),
                                          CInt(DR("PART_SSN")), CInt(DR("PAT_SSN")),
                                          CStr(DR("PART_FNAME")), CStr(DR("PART_LNAME")),
                                          CStr(DR("PAT_FNAME")), CStr(DR("PAT_LNAME")),
                                          CStr(DR("ANNOTATION")), DR("FLAG"), _DomainUser.ToUpper, Transaction)

                Next

                CMSDALCommon.CommitTransaction(Transaction)
            End If

        Catch ex As Exception
            If Transaction IsNot Nothing Then
                CMSDALCommon.RollbackTransaction(Transaction)
            End If
            Throw
        End Try

    End Sub

    Private Sub AnnotationsMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AnnotationsMenuItem.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' opens a annotation viewer
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	10/19/2006	Created
        ' Lalitha Moparthi 11.29.2022 Updated
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DR As DataRow
        Dim AnnotateForm As AnnotationDialog

        Try
            DR = DirectCast(_QueueBS.Current, DataRowView).Row
            If DR Is Nothing OrElse DR("CLAIM_ID") Is DBNull.Value Then Return

            AnnotationsDataSet.ANNOTATIONS.Rows.Clear()
            AnnotationsDataSet = CType(CMSDALFDBMD.RetrieveClaimAnnotations(CInt(DR("CLAIM_ID")), AnnotationsDataSet), AnnotationsDataSet)

            AnnotateForm = New AnnotationDialog(CInt(DR("CLAIM_ID")), CInt(DR("FAMILY_ID")), CInt(DR("RELATION_ID")), CInt(DR("PART_SSN")), CInt(DR("PAT_SSN")), DR("PART_FNAME").ToString, DR("PART_LNAME").ToString, DR("PAT_FNAME").ToString, DR("PAT_LNAME").ToString, AnnotationsDataSet.ANNOTATIONS)

            AddHandler AnnotateForm.AnnotationsModified, AddressOf CreateAnnotations

            AnnotateForm.ShowDialog(Me)

        Catch ex As Exception
            Throw
        Finally
            If AnnotateForm IsNot Nothing Then
                AnnotateForm.Dispose()
            End If
            AnnotateForm = Nothing

        End Try
    End Sub

    Private Sub EligibilityMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EligibilityMenuItem.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' opens an Eligibility Viewer
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	1/23/2007	Created
        ' Lalitha Moparthi 11/29/2022 Updated
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim QElig As QueueEligibility
        Dim DR As DataRow
        Dim Bind As Binding
        Try
            DR = DirectCast(_QueueBS.Current, DataRowView).Row
            If DR Is Nothing OrElse DR("CLAIM_ID") Is DBNull.Value Then Return

            QElig = New QueueEligibility(CInt(DR("FAMILY_ID")), UFCWGeneral.IsNullShortHandler(DR("RELATION_ID")), CStr(DR("DOC_TYPE")))

            QElig.LoadEligibility()

            QElig.PartSSNTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _QueueBS, "PART_SSN", True)
            AddHandler Bind.Format, AddressOf SSNBinding_Format
            AddHandler Bind.Parse, AddressOf SSNBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            QElig.PartSSNTextBox.DataBindings.Add(Bind)

            QElig.PatSSNTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _QueueBS, "PAT_SSN", True)
            AddHandler Bind.Format, AddressOf SSNBinding_Format
            AddHandler Bind.Parse, AddressOf SSNBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            QElig.PatSSNTextBox.DataBindings.Add(Bind)

            QElig.PartLNameTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _QueueBS, "PART_LNAME", True)
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            QElig.PartLNameTextBox.DataBindings.Add(Bind)

            QElig.PartFNameTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _QueueBS, "PAT_LNAME", True)
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            QElig.PartFNameTextBox.DataBindings.Add(Bind)

            QElig.PatLNameTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _QueueBS, "PAT_LNAME", True)
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            QElig.PatLNameTextBox.DataBindings.Add(Bind)

            QElig.PatFNameTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _QueueBS, "PAT_FNAME", True)
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            QElig.PatFNameTextBox.DataBindings.Add(Bind)


            QElig.ShowDialog(Me)

        Catch ex As Exception
            Throw
        Finally
            QElig.Dispose()
            QElig = Nothing
        End Try
    End Sub

    Private Sub ReleaseMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReleaseMenuItem.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	2/9/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DR As DataRow
        Try

            DR = DirectCast(_QueueBS.Current, DataRowView).Row

            If DR Is Nothing OrElse DR("CLAIM_ID") Is DBNull.Value Then Return

            _QueueSharedInterfacesMessage.StatusMessage("Retrieving Item.  Please Wait...")

            ReDim _DelDocs(0)

            If ReleaseAuditItem(CInt(DR("CLAIM_ID")), False) Then
                _DelDocs(UBound(_DelDocs, 1)) = CStr(DR("CLAIM_ID"))
                ReDim Preserve _DelDocs(UBound(_DelDocs, 1) + 1)
            End If

CleanUp:
            ReDim Preserve _DelDocs(UBound(_DelDocs, 1) - 1)
            DeleteQueueItems()

        Catch ex As Exception
            _QueueSharedInterfacesMessage.StatusMessage("Release Error")
            Throw
        Finally
            _QueueSharedInterfacesMessage.StatusMessage("")
        End Try
    End Sub
#End Region

#Region "Other Control Events"
    Private Sub SearchByTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DocIDTextBox.TextChanged, TINTextBox.TextChanged, PartSSNTextBox.TextChanged
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Prevent the user from typing invalid characters.  Non-numeric.
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	11/16/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim TBox As TextBox
        Try
            TBox = CType(sender, TextBox)
            Dim digitsOnly As Regex = New Regex("[^\d]")
            TBox.Text = digitsOnly.Replace(TBox.Text, "")

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Function RowsMultiSelected(dg As DataGrid) As ArrayList

        Dim Value As Boolean = False
        Dim AL As New ArrayList()
        Dim CM As CurrencyManager
        Dim DV As DataView

        Try

            If dg.DataSource IsNot Nothing Then
                CM = CType(Me.BindingContext(dg.DataSource, dg.DataMember), CurrencyManager)
                DV = CType(CM.List, DataView)

                For I As Integer = 0 To DV.Count - 1
                    If dg.IsSelected(I) Then
                        AL.Add(DV(I))
                    End If
                Next

                Return AL
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Sub DataGridCustomContextMenu_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles QueueDataGridCustomContextMenu.Opening
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' hides the display image option if nothing is able to display
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DG As DataGridCustom
        Dim DR As DataRow
        Dim DGContextMenuStrip As ContextMenuStrip

        Try

            DGContextMenuStrip = CType(sender, ContextMenuStrip)
            DG = CType(DirectCast(sender, System.Windows.Forms.ContextMenuStrip).SourceControl, DataGridCustom)
            If DG.DataSource Is Nothing Then Exit Sub
            DGContextMenuStrip.Items("DisplayMenuItem").Available = False
            DGContextMenuStrip.Items("DisplayMenuItem").Enabled = False
            DGContextMenuStrip.Items("HistoryMenuItem").Available = False
            DGContextMenuStrip.Items("HistoryMenuItem").Enabled = False
            DGContextMenuStrip.Items("AnnotationsMenuItem").Available = False
            DGContextMenuStrip.Items("AnnotationsMenuItem").Enabled = False
            DGContextMenuStrip.Items("EligibilityMenuItem").Available = False
            DGContextMenuStrip.Items("EligibilityMenuItem").Enabled = False
            DGContextMenuStrip.Items(6).Available = False   'AuditSepMenuItem.Visible 
            DGContextMenuStrip.Items(6).Enabled = False
            DGContextMenuStrip.Items("ReleaseMenuItem").Available = False
            DGContextMenuStrip.Items("ReleaseMenuItem").Enabled = False

            If DirectCast(DG.DataSource, BindingSource).Current IsNot Nothing Then
                DR = DirectCast(DirectCast(DG.DataSource, BindingSource).Current, DataRowView).Row
            End If

            If DR IsNot Nothing Then
                DGContextMenuStrip.Items("DisplayMenuItem").Available = True
                DGContextMenuStrip.Items("DisplayMenuItem").Enabled = True
                DGContextMenuStrip.Items("HistoryMenuItem").Available = True
                DGContextMenuStrip.Items("HistoryMenuItem").Enabled = True
                DGContextMenuStrip.Items("AnnotationsMenuItem").Available = True
                DGContextMenuStrip.Items("AnnotationsMenuItem").Enabled = True
                DGContextMenuStrip.Items("EligibilityMenuItem").Available = True
                DGContextMenuStrip.Items("EligibilityMenuItem").Enabled = True
                If ModeComboBox.Text.ToUpper = "AUDIT" Then
                    DGContextMenuStrip.Items(6).Available = True   'AuditSepMenuItem.Visible 
                    DGContextMenuStrip.Items(6).Enabled = True
                    DGContextMenuStrip.Items("ReleaseMenuItem").Available = True
                    DGContextMenuStrip.Items("ReleaseMenuItem").Enabled = True
                End If
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub QueueDataGrid_RowCountChanged(ByVal previousRowCount As Integer?, ByVal currentRowCount As Integer) Handles QueueDataGrid.RowCountChanged
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' enables and disables buttons as you can use them
        ' </summary>
        ' <param name="PreviousRowCount"></param>
        ' <param name="CurrentRowCount"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	5/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        If currentRowCount = 0 Then
            OpenButton.Enabled = False
            DisplayMenuItem.Enabled = False
            RecentDocsMenuItem.Enabled = False
        Else
            OpenButton.Enabled = True
            DisplayMenuItem.Enabled = True
            RecentDocsMenuItem.Enabled = True
        End If
    End Sub
    Private Sub MaxItemsTextBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MaxItemsTextBox.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub
    Private Sub MaxItemsTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MaxItemsTextBox.TextChanged
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' auto checks the maxitems checkbox if maxitems have been typed
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	1/8/2007	Created 
        '  Lalitha Moparthi 12/02/2022 updated to use regular expressions
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim TBox As TextBox
        Try
            TBox = CType(sender, TextBox)
            Dim digitsOnly As Regex = New Regex("[^\d]")
            TBox.Text = digitsOnly.Replace(TBox.Text, "")

            If MaxItemsCheckBox.Checked = False AndAlso Len(MaxItemsTextBox.Text) > 0 Then
                MaxItemsCheckBox.Checked = True
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ModeComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ModeComboBox.SelectedIndexChanged
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' loads the user doc types for the selected mode
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	2/15/2007	Created
        ' Lalitha Moparthi 12/02/2022 Updated to use Binding Source
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DocType As String
        Try
            If WorkTypeComboBox.Text = "" Then Exit Sub
            DocType = WorkTypeComboBox.Text
            LoadDocTypes()
            If WorkTypeComboBox.Items.IndexOf(DocType) <> -1 Then
                WorkTypeComboBox.SelectedIndex = WorkTypeComboBox.Items.IndexOf(DocType)
            Else
                WorkTypeComboBox.SelectedIndex = 0
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
#End Region

#Region "Custom Subs\Functions"
    Public Function GetNextAudit(ByVal docClass As String, ByVal docType As String) As Integer
        Dim ClaimID As Integer
        Try

            ClaimID = CMSDALFDBMD.GetNextAudit(_DomainUser.ToUpper, UFCWGeneralAD.CMSCanAdjudicateEmployee(), docClass, docType)
            If ClaimID = Nothing Then
                MessageBox.Show("There are not anymore Items available of type " & docClass & ":" & docType & "(" & ModeComboBox.Text.ToUpper & ")", "No Available Items", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return -1
            End If

            Return ClaimID

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetNext(ByVal docClass As String, ByVal docType As String) As Integer
        Dim ClaimID As Integer
        Try

            ClaimID = CMSDALFDBMD.GetNext(_DomainUser.ToUpper, UFCWGeneralAD.CMSCanAdjudicateEmployee(), docClass, docType, SystemInformation.ComputerName)
            If ClaimID = Nothing Then
                MessageBox.Show("There are no more Items available of type " & docClass & ":" & docType, "No Available Items", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return -1
            ElseIf ClaimID = 0 Then
                MessageBox.Show("System was busy, please retry your previous action.", "System Busy", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return -1
            End If

            Return ClaimID

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Sub CreateDocHistoryEntry(ByVal claimId As Integer, ByVal claimDR As DataRow, ByVal functionName As String, ByVal assignedTo As String, ByVal Status As String, Optional ByRef transaction As DbTransaction = Nothing)

        Dim HistSum As String
        Dim HistDetail As String

        Try
            HistSum = "CLAIM ID " & Format(claimId, "00000000") & " ASSIGNED TO " & assignedTo
            HistDetail = "FUNCTION: " & functionName & Microsoft.VisualBasic.vbCrLf &
                            "CLAIM ID: " & Format(claimId, "00000000") & Microsoft.VisualBasic.vbCrLf &
                            "DOCID: " & claimDR("DOCID").ToString & Microsoft.VisualBasic.vbCrLf &
                            "DOC CLASS: " & claimDR("DOC_CLASS").ToString & Microsoft.VisualBasic.vbCrLf &
                            "DOC TYPE: " & claimDR("DOC_TYPE").ToString & Microsoft.VisualBasic.vbCrLf &
                            "ASSIGNED TO " & assignedTo

            CMSDALFDBMD.CreateDocHistory(claimId, UFCWGeneral.IsNullLongHandler(claimDR("DOCID")), If(functionName.ToUpper = "GET NEXT", "GETNEXT", "GETNEXTAUDIT"), CInt(claimDR("FAMILY_ID")), CShort(claimDR("RELATION_ID")), UFCWGeneral.IsNullIntegerHandler(claimDR("PART_SSN")), UFCWGeneral.IsNullIntegerHandler(claimDR("PAT_SSN")), CStr(claimDR("DOC_CLASS")), CStr(claimDR("DOC_TYPE")), HistSum, HistDetail, _DomainUser.ToUpper, transaction)

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Function GetNextItem(ByVal docClass As String, ByVal docType As String) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' gets the next available item for the given type and opens the work screen to display
        ' </summary>
        ' <param name="DocClass"></param>
        ' <param name="DocType"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim Transaction As DbTransaction
        Dim ClaimID As Integer
        Dim BusyState As Boolean = False
        Dim ClaimDR As DataRow

        Dim ClaimIDTimeStr As String = ""

        Try
            _QueueSharedInterfacesMessage.StatusMessage("Looking For Next Item.  Please Wait...")

            Using WC As New GlobalCursor

                If ModeComboBox.Text.ToUpper = "AUDIT" Then
                    ClaimID = GetNextAudit(docClass, docType)
                    If ClaimID = -1 Then Exit Try
                    BusyState = True
                Else
                    ClaimID = GetNext(docClass, docType)
                    If ClaimID = -1 Then Exit Try
                    BusyState = True
                End If

                ClaimDR = GetClaimMetaData(ClaimID)

                If ModeComboBox.Text.ToUpper = "AUDIT" Then
                    CreateDocHistoryEntry(ClaimID, ClaimDR, "GET NEXT AUDIT", "AUDITOR: " & _DomainUser.ToUpper, "GETNEXTAUDIT")
                Else
                    CreateDocHistoryEntry(ClaimID, ClaimDR, "GET NEXT", _DomainUser.ToUpper, "GETNEXT", Transaction)
                End If

                If OpenWorkScreen(ModeComboBox.Text.ToUpper, _LastDocClass, ClaimID, True, False, Nothing) Then

                    CMSDALFDBMD.WriteToClaimsHistoryXML(CInt(ClaimDR("CLAIM_ID")), UFCWGeneral.IsNullIntegerHandler(ClaimDR("PART_SSN")), UFCWGeneral.IsNullLongHandler(ClaimDR("DOCID")), UFCWGeneral.IsNullIntegerHandler(ClaimDR("FAMILY_ID")), Now(), "Opened")

                    ReDim _DelDocs(0)

                    _DelDocs(UBound(_DelDocs, 1)) = CStr(ClaimID)

                    DeleteQueueItems()

                Else

                    CMSDALFDBMD.UnBusyItem(ClaimID, _DomainUser.ToUpper)
                    BusyState = False

                    If _OpenChildrenCount >= MAXWORKFORMS Then
                        'Unable to open
                    End If

                End If
            End Using

        Catch db2 As DDTek.DB2.DB2Exception

            If db2.Number = 911 Or db2.Number = 913 Then
                MessageBox.Show("A Timeout has occurred.  Please try Get Next Item again", "Timeout", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Throw
            End If

        Catch ex As Exception
            _QueueSharedInterfacesMessage.StatusMessage("Get Next Error")

            If BusyState Then
                Try
                    Transaction = CMSDALCommon.BeginTransaction()

                    CMSDALFDBMD.UnBusyItem(ClaimID, _DomainUser.ToUpper, Transaction)
                    CMSDALFDBMD.ReleaseFamilyLock(CInt(ClaimDR("FAMILY_ID")), Transaction)
                    CMSDALCommon.CommitTransaction(Transaction)

                Catch exx As Exception
                    CMSDALCommon.RollbackTransaction(Transaction)
                    Throw
                Finally
                    BusyState = False
                End Try
            End If
            Throw
        Finally
            _QueueSharedInterfacesMessage.StatusMessage("")
        End Try
    End Function

    Public Function GetPluginName(ByVal claimDR As DataRow, ByVal docClass As String) As String
        Try

            If CStr(claimDR("DOC_TYPE")).Trim.ToUpper.StartsWith("UTL") OrElse UFCWGeneralAD.CMSUTL Then
                Return "UTL"
                'ElseIf CStr(claimMetaData("DOC_TYPE")).Trim.ToUpper.Contains("COPAY") Then
                '    Return "COPAY"
            Else
                Return StrConv(docClass, VbStrConv.ProperCase)
            End If

        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Function DetermineIfClaimIsOkToOpen(ByVal claimID As Integer, ByVal mode As String, ByVal claimDR As DataRow, ByRef transaction As DbTransaction) As Boolean
        Try
            If claimDR Is Nothing Then
                If transaction IsNot Nothing Then
                    CMSDALCommon.RollbackTransaction(transaction)
                End If
                MessageBox.Show("Item " & claimID & " is now locked by another user" & Microsoft.VisualBasic.vbCrLf &
                                "or it has already been completed.", "Item Locked or Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            If My.Computer.Keyboard.ShiftKeyDown Then
                ReleaseClaim(claimID, claimDR, transaction)
            End If
            If IsDBNull(claimDR("STATUS")) = False Then
                If mode.ToUpper = "AUDIT" Then
                    If claimDR("STATUS").ToString.Trim <> "AUDIT" AndAlso claimDR("STATUS").ToString.Trim <> "AUDITING" Then
                        If transaction IsNot Nothing Then
                            CMSDALCommon.RollbackTransaction(transaction)
                        End If
                        MessageBox.Show("Item " & claimID & " has already been completed.", "Item Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return False
                    End If
                Else
                    If claimDR("STATUS").ToString.Trim <> "NEW" AndAlso claimDR("STATUS").ToString.Trim <> "OPEN" AndAlso claimDR("STATUS").ToString.Trim <> "INPROGRESS" Then
                        If transaction IsNot Nothing Then
                            CMSDALCommon.RollbackTransaction(transaction)
                        End If
                        MessageBox.Show("Item " & claimID & " has already been completed.", "Item Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return False
                    End If
                End If
            Else
                MessageBox.Show("Item " & claimID & " has in.", "Item Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If


            If CBool(claimDR("BUSY_SW").ToString) AndAlso Not My.Computer.Keyboard.ShiftKeyDown Then
                If transaction IsNot Nothing Then
                    CMSDALCommon.RollbackTransaction(transaction)
                End If
                MessageBox.Show("Item " & claimID & " is now in use." & Microsoft.VisualBasic.vbCrLf &
                                        "Please select another item.", "Item Already In Use", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            If Not CStr(claimDR("DOC_TYPE")).ToUpper.Trim.StartsWith("UTL") Then
                If Not UFCWGeneralAD.CMSPay Then
                    MessageBox.Show("Item " & claimID & " is not of the type UTL." & Microsoft.VisualBasic.vbCrLf &
                                     "Please select another item.", "Access Prohibited via Security", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
                If IsFamilyLocked(CInt(claimDR("FAMILY_ID")), transaction) AndAlso Not My.Computer.Keyboard.ShiftKeyDown Then
                    If transaction IsNot Nothing Then
                        CMSDALCommon.RollbackTransaction(transaction)
                    End If
                    MessageBox.Show("This Family is now Locked by another user." & Microsoft.VisualBasic.vbCrLf &
                                    "Please select another item and try to work this item later.", "Participant Lock", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            End If

            Return True

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Function

    Public Sub BusyTheClaim(ByVal claimID As Integer, ByRef transaction As DbTransaction)
        Try
            CMSDALFDBMD.BusyItem(claimID, _DomainUser.ToUpper, transaction)

            If transaction IsNot Nothing Then
                CMSDALCommon.CommitTransaction(transaction)
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub
    Public Sub InsertFamilyLock(ByVal claimID As Integer, ByVal claimDR As DataRow, ByVal mode As String, ByRef transaction As DbTransaction)
        Try
            If mode.ToUpper <> "AUDIT" AndAlso Not CStr(claimDR("DOC_TYPE")).ToUpper.StartsWith("UTL_") Then
                CMSDALFDBMD.InsertFamilyLock(CInt(claimDR("FAMILY_ID")), UFCWGeneral.IsNullShortHandler(claimDR("RELATION_ID")), claimID, _DomainUser.ToUpper, SystemInformation.ComputerName, transaction)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Function AttemptToOpenPlugin(ByVal claimID As Integer, ByVal docClass As String, ByVal busy As Boolean, ByVal claimDR As DataRow, ByVal mode As String, ByVal multiOpen As Boolean, ByRef dialogAnswer As DialogResult, ByRef transaction As DbTransaction) As Boolean

        Dim NotMine As Boolean = False
        Dim WasOpened As Boolean

        Try
            Using YesAllDialog As YesToAllDialog = New YesToAllDialog

                Dim BusyState As Boolean = busy
                Dim FamilyLocked As Boolean = busy

                If busy Then 'busy is only set to true when the user clicks GetNext
                    WasOpened = OpenPlugIn(GetPluginName(claimDR, docClass), claimID, mode.ToUpper)

                    If Not WasOpened Then ReleaseClaim(claimID, claimDR, transaction)

                Else
                    If DetermineIfClaimIsOkToOpen(claimID, mode, claimDR, transaction) Then
                        BusyTheClaim(claimID, transaction)
                        BusyState = True

                        If GetPendedTo(mode, claimDR) = "" OrElse GetPendedTo(mode, claimDR).ToUpper <> _DomainUser.ToUpper Then
                            NotMine = True
                            BusyState = TakePossesionOfClaim(claimID, GetPendedTo(mode, claimDR), multiOpen, dialogAnswer)
                            If BusyState = False Then Return False
                        End If

                        transaction = CMSDALCommon.BeginTransaction

                        PendClaimWithUser(CStr(claimID), mode, transaction)

                        InsertFamilyLock(claimID, claimDR, mode, transaction)
                        FamilyLocked = True

                        If NotMine Then
                            If mode.ToUpper = "AUDIT" Then
                                Me.CreateDocHistoryEntry(claimID, claimDR, "OPEN AUDIT ITEM", " AUDITOR " & _DomainUser.ToUpper, "TAKENFORAUDIT", transaction)
                            Else
                                Me.CreateDocHistoryEntry(claimID, claimDR, "OPEN ITEM", _DomainUser.ToUpper, "TAKENPOSSESSIONOF", transaction)
                            End If
                        End If

                        CMSDALCommon.CommitTransaction(transaction)

                        WasOpened = OpenPlugIn(GetPluginName(claimDR, docClass), claimID, mode.ToUpper)

                        If Not WasOpened Then ReleaseClaim(claimID, claimDR, Nothing)

                    End If
                End If

                Return WasOpened

            End Using

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Sub ReleaseClaim(ByVal claimID As Integer, ByVal claimDR As DataRow, ByRef transaction As DbTransaction)
        Try

            CMSDALFDBMD.UnBusyItem(claimID, _DomainUser.ToUpper, transaction)
            CMSDALFDBMD.ReleaseFamilyLock(CInt(claimDR("FAMILY_ID")), transaction)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub PendClaimWithUser(ByVal claimID As String, ByVal mode As String, ByRef transaction As DbTransaction)
        Try

            If mode.ToUpper = "AUDIT" Then
                CMSDALFDBMD.PendToAuditor(CInt(claimID), _DomainUser.ToUpper, transaction)
            Else
                CMSDALFDBMD.PendToUser(CInt(claimID), _DomainUser.ToUpper, transaction)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Friend Function OpenWorkScreen(ByVal mode As String, ByVal docClass As String, ByVal claimID As Integer, ByVal busy As Boolean, ByVal multiOpen As Boolean, ByRef dialogAnswer As DialogResult, Optional ByRef transaction As DbTransaction = Nothing) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' this procedure tries to open the correct work form for the given class
        ' </summary>
        ' <param name="DocClass"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	2/28/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim ClaimDR As DataRow
        Dim ReturnVal As Boolean
        Dim docs() As Object
        ReDim docs(1)

        Try

            ClaimDR = GetClaimMetaData(claimID)

            If _PlugInController.Contains(GetPluginName(ClaimDR, docClass)) = True Then
                ReturnVal = AttemptToOpenPlugin(claimID, docClass, busy, ClaimDR, mode, multiOpen, dialogAnswer, transaction)
            Else
                MessageBox.Show("The Plugin for class " & """" & GetPluginName(ClaimDR, docClass) & """" & " is not installed." & Chr(13) & "You will be unable to work items of this Classification.",
                                "Plugin Not Installed", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            Return ReturnVal

        Catch ex As Exception

            Try
                CMSDALFDBMD.UnBusyItem(claimID, _DomainUser.ToUpper)
                CMSDALFDBMD.ReleaseFamilyLock(CInt(ClaimDR("FAMILY_ID")))
            Catch exx As Exception
                Throw
            End Try
            Throw
        Finally

        End Try
    End Function

    Public Function TakePossesionOfClaim(ByVal claimID As Integer, ByVal pendUser As String, ByVal multiOpen As Boolean, ByVal dialogAnswer As DialogResult) As Boolean
        Dim Transaction As DbTransaction
        Dim YesAllDialog As YesToAllDialog

        Try
            YesAllDialog = New YesToAllDialog

            If multiOpen Then
                If dialogAnswer = Nothing OrElse (dialogAnswer <> DialogResult.Retry AndAlso dialogAnswer <> DialogResult.Abort) Then
                    dialogAnswer = YesAllDialog.ShowDialog(Me, "Item " & claimID & " is Assigned To:" & pendUser & Microsoft.VisualBasic.vbCrLf &
                                                                                                            "Do You Want To Take Possession Of It?", "Take Possession of Item")
                End If
            ElseIf pendUser.Trim.Length < 1 Then 'only warn if item belongs to someone
                dialogAnswer = DialogResult.Yes
            Else
                dialogAnswer = MessageBox.Show("Item " & claimID & " is Assigned To:" & pendUser & Microsoft.VisualBasic.vbCrLf &
                                            "Do You Want To Take Possession Of It?", "Take Possession of Item",
                                            MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            End If

            If dialogAnswer <> DialogResult.Yes AndAlso dialogAnswer <> DialogResult.Retry Then
                Transaction = CMSDALCommon.BeginTransaction

                CMSDALFDBMD.UnBusyItem(claimID, _DomainUser.ToUpper, Transaction)

                CMSDALCommon.CommitTransaction(Transaction)

                Return False
            End If

            Return True

        Catch ex As Exception
            If Transaction IsNot Nothing Then
                CMSDALCommon.RollbackTransaction(Transaction)
            End If
            Throw
        End Try
    End Function

    Public Function GetPendedTo(ByVal mode As String, ByVal claimDR As DataRow) As String
        Try

            If mode.ToUpper = "AUDIT" Then
                If IsDBNull(claimDR("AUDITED_BY")) = False Then
                    Return claimDR("AUDITED_BY").ToString.ToUpper
                End If
            Else
                If IsDBNull(claimDR("PENDED_TO")) = False Then
                    Return claimDR("PENDED_TO").ToString.ToUpper
                End If
            End If

            Return ""

        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Sub LoadDocTypes()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Fills the DocType Selection combobox with active doctypes
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	2/17/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            WorkTypeComboBox.DataSource = Nothing
            WorkTypeComboBox.Items.Clear()

            Select Case ModeComboBox.Text.ToUpper.ToString
                Case "AUDIT"
                    WorkTypeComboBox.DataSource = CMSDALCommon.SelectDistinctAndSorted("", CMSDALFDBMD.RetrieveUserDocTypes(_DomainUser.ToUpper, "AUDIT"), "DOC_TYPE", True)
                Case "REGULAR"
                    WorkTypeComboBox.DataSource = CMSDALCommon.SelectDistinctAndSorted("", CMSDALFDBMD.RetrieveUserDocTypes(_DomainUser.ToUpper, ""), "DOC_TYPE", True)
                Case Else
                    WorkTypeComboBox.DataSource = CMSDALCommon.SelectDistinctAndSorted("", CMSDALFDBMD.RetrieveUserDocTypes(_DomainUser.ToUpper, "BOTH"), "DOC_TYPE", True)
            End Select

            WorkTypeComboBox.DisplayMember = "DOC_TYPE"

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub RefreshQueue()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Loads the Queue Grid based on the values supplied by the user
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	2/15/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim OutMessage As String = ""
        Dim FinishDuration As String = ""
        Dim EmployeeAccess As Boolean

        Try

            _LastDocClass = "MEDICAL"

            If WorkTypeComboBox.Text <> "" AndAlso WorkTypeComboBox.Text.ToUpper <> "(ALL)" Then
                _LastDocType = WorkTypeComboBox.Text
            Else
                _LastDocType = Nothing
            End If

            _LastMode = ModeComboBox.Text.ToUpper
            SaveSetting(_APPKEY, "Queue\Settings", "LastMode", ModeComboBox.Text)

            _Cancel = False

            If _QueueDataTable IsNot Nothing Then _QueueDataTable.Clear()

            RefreshButton.Enabled = False

            _QueueSharedInterfacesMessage.StatusMessage("Refreshing. Please Wait...")
            QueueDataGrid.CaptionText = "Refreshing. Please Wait..."

            EmployeeAccess = UFCWGeneralAD.CMSCanAdjudicateEmployee()

            _Stopwatch = New Stopwatch
            _Stopwatch.Start()

            Task.Factory.StartNew(Sub() RefreshQueueQuery(AddressOf RefreshQueueResultsHandler, _LastMode, EmployeeAccess, _DomainUser.ToUpper, _LastDocClass, _LastDocType, If(MyAssignmentsCheckBox.Checked, _DomainUser.ToUpper, Nothing), If(MaxItemsCheckBox.Checked AndAlso MaxItemsTextBox.Text.ToString.Trim.Length > 0, CType(MaxItemsTextBox.Text.Trim, Integer?), CType(Nothing, Integer?)), If(ClaimIDTextBox.Text.Trim.Length > 0, ClaimIDTextBox.Text.Trim, Nothing), If(FamilyIDTextBox.Text.ToString.Trim.Length > 0, CType(FamilyIDTextBox.Text.ToString.Trim, Integer?), CType(Nothing, Integer?)), If(DocIDTextBox.Text.ToString.Trim.Length > 0, CType(DocIDTextBox.Text.ToString.Trim, Integer?), CType(Nothing, Integer?)), If(BatchNumberTextBox.Text.ToString.Trim.Length > 0, BatchNumberTextBox.Text.Trim, Nothing), If(PartSSNTextBox.Text.ToString.Trim.Length > 0, CType(UFCWGeneral.UnFormatSSN(PartSSNTextBox.Text.ToString.Trim), Integer?), CType(Nothing, Integer?)), If(TINTextBox.Text.ToString.Trim.Length > 0, CType(UFCWGeneral.UnFormatTIN(TINTextBox.Text.ToString.Trim), Integer?), CType(Nothing, Integer?))))

        Catch ex As Exception
            Throw
        Finally
            RefreshButton.Text = "&Refresh"
            RefreshButton.Enabled = True
        End Try
    End Sub

    Private Sub Tick(state As Object)
        Dim Elapsed As TimeSpan = _Stopwatch.Elapsed
        Dim StatusMessage As String = "Refresh Duration " & String.Format("{0:00}:{1:00}",
                                   Elapsed.Minutes,
                                   Elapsed.Seconds)

        SharedInterfacesMessage(StatusMessage)

    End Sub

    Private Sub DeleteQueueItems()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Deletes items from the queue based on an array (DelDocs) of CLAIM_ID's
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/1/2006	Created
        ' Lalitha Moparthi 12/01/2022 Updated to use BindingSource instead of view
        ' </history>
        ' -----------------------------------------------------------------------------
        If _QueueDataTable Is Nothing Then Return

        Try
            Using WC As New GlobalCursor
                _QueueDataTable = DirectCast(_QueueBS.DataSource, DataTable)
                Dim DelResults As Generic.IEnumerable(Of DataRow) = (From P In _QueueDataTable.AsEnumerable
                                                                     Where _DelDocs.Contains(CStr(P.Field(Of Int32)("CLAIM_ID")))
                                                                     Select P).ToArray

                For Each dr As DataRow In DelResults
                    _QueueDataTable.Rows.Remove(dr)
                Next
                _QueueBS.EndEdit()
                QueueDataGrid.Select(_QueueBS.Position)

            End Using

            _LastOutMessage = "Found " & _QueueDataTable.Rows.Count & " Row" & If(_QueueDataTable.Rows.Count <> 1, "s", "")
            _QueueSharedInterfacesMessage.StatusMessage(_LastOutMessage)

            QueueDataGrid.CaptionText = StrConv(_QueueDataTable.Rows.Count & " - Pending Work Item" & If(_QueueDataTable.Rows.Count <> 1, "s", "") & " In " & _LastDocClass & If(_LastDocType = "", "", ":" & _LastDocType), VbStrConv.ProperCase)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ApplyUserSecurity()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Initializes Controls based on AD roles
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	4/7/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            If UFCWGeneralAD.CMSCanAudit = False Then
                ModeLabel.Visible = False
                ModeComboBox.Visible = False
            End If

            If ModeComboBox.Text = "" Then
                ModeComboBox.SelectedIndex = 0
            End If

            If UFCWGeneralAD.CMSCanPickWork = False Then
                MyAssignmentsCheckBox.Enabled = False
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function PopulateICON(ByVal dt As DataTable) As DataTable
        Try

            For Each DR As DataRow In dt.Rows
                If Not IsDBNull(DR("STATUS")) Then
                    Select Case DR("STATUS").ToString.ToLower
                        Case Is = "new"
                            If CBool(DR("DUPLICATE_SW")) = True Then
                                DR("ICON") = 23
                            Else
                                DR("ICON") = 4
                            End If

                        Case Is = "open"
                            If CBool(DR("DUPLICATE_SW").ToString) = True Then
                                DR("ICON") = 23
                            Else
                                DR("ICON") = 14
                            End If
                        Case Is = "inprogress", "audit", "auditing"
                            If ModeComboBox.Text.ToUpper = "AUDIT" Then
                                If IsDBNull(DR("AUDITED_BY")) = False AndAlso DR("AUDITED_BY").ToString.Trim.Length > 0 Then
                                    DR("ICON") = 18
                                Else
                                    DR("ICON") = 9
                                End If
                            Else
                                If DR("USERID").ToString = "FBMD0280" Then
                                    DR("ICON") = 22
                                ElseIf IsDBNull(DR("AUDITED_BY")) = False AndAlso DR("AUDITED_BY").ToString.Trim.Length > 0 Then
                                    DR("ICON") = 18
                                ElseIf IsDBNull(DR("ROUTED_BY")) = False AndAlso DR("ROUTED_BY").ToString.Trim.Length > 0 Then
                                    If Not IsDBNull(DR("PRICED_BY")) AndAlso DR("PRICED_BY").ToString.Trim.Length > 0 Then
                                        If Not IsDBNull(DR("PRICING_ERROR")) AndAlso DR("PRICING_ERROR").ToString.Trim.Length > 0 Then
                                            DR("ICON") = 21
                                        Else
                                            DR("ICON") = 19
                                        End If

                                    Else
                                        DR("ICON") = 1
                                    End If

                                ElseIf Not IsDBNull(DR("PRICED_BY")) AndAlso DR("PRICED_BY").ToString.Trim.Length > 0 Then

                                    If Not IsDBNull(DR("PRICING_ERROR")) AndAlso DR("PRICING_ERROR").ToString.Trim.Length > 0 Then
                                        DR("ICON") = 20
                                    Else
                                        If CBool(DR("DUPLICATE_SW")) = True Then
                                            DR("ICON") = 24
                                        Else
                                            DR("ICON") = 17
                                        End If
                                    End If

                                Else
                                    DR("ICON") = 9
                                End If
                            End If

                    End Select
                End If
            Next

            Return dt

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Sub DetermineCellIcon(ByRef pic As Image, ByVal cell As System.Windows.Forms.DataGridCell)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' this determines what icon should be displayed based on the value of other
        ' columns in the row, mainly STATUS
        ' </summary>
        ' <param name="Pic"></param> is the picture to display in the grid to be returned
        ' <param name="cell"></param> is the information needed to make the determination
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	2/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try
            If QueueDataGrid.GetDefaultDataView IsNot Nothing Then
                pic = QueueImageList.Images(CInt(QueueDataGrid.GetDefaultDataView(cell.RowNumber)("ICON")))
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Function DetermineCellIconTooltip(ByVal rowNumber As Integer, ByRef toolTipText As String) As Integer
        Dim DV As DataView

        Try
            DV = QueueDataGrid.GetDefaultDataView

            If DV IsNot Nothing AndAlso IsDBNull(DV(rowNumber)("STATUS")) = False Then
                Select Case DV(rowNumber)("STATUS").ToString.ToLower
                    Case Is = "new"
                        If CBool(DV(rowNumber)("DUPLICATE_SW")) = True Then
                            DetermineCellIconTooltip = 23
                            toolTipText = "New Claim (Possible duplicate)"
                        Else
                            DetermineCellIconTooltip = 4
                            toolTipText = "New Claim"
                        End If

                    Case Is = "open"
                        If CBool(DV(rowNumber)("DUPLICATE_SW").ToString) = True Then
                            toolTipText = "Open Claim (Possible duplicate)"
                            DetermineCellIconTooltip = 23
                        Else
                            toolTipText = "Open Claim"
                            DetermineCellIconTooltip = 14
                        End If
                    Case Is = "inprogress", "audit", "auditing"
                        If ModeComboBox.Text.ToUpper = "AUDIT" Then
                            If IsDBNull(DV(rowNumber)("AUDITED_BY")) = False AndAlso DV(rowNumber)("AUDITED_BY").ToString.Trim.Length > 0 Then
                                toolTipText = "Awaiting Audit"
                                DetermineCellIconTooltip = 18
                            Else
                                toolTipText = ""
                                DetermineCellIconTooltip = 9
                            End If
                        Else
                            If DV(rowNumber)("USERID").ToString.Trim = "FBMD0280" Then
                                toolTipText = "ReOpened by Check Process"
                                DetermineCellIconTooltip = 22
                            ElseIf IsDBNull(DV(rowNumber)("AUDITED_BY")) = False AndAlso DV(rowNumber)("AUDITED_BY").ToString.Trim.Length > 0 Then
                                toolTipText = "Awaiting Audit"
                                DetermineCellIconTooltip = 18
                            ElseIf IsDBNull(DV(rowNumber)("ROUTED_BY")) = False AndAlso DV(rowNumber)("ROUTED_BY").ToString.Trim.Length > 0 Then
                                If Not IsDBNull(DV(rowNumber)("PRICED_BY")) AndAlso DV(rowNumber)("PRICED_BY").ToString.Trim.Length > 0 Then
                                    If Not IsDBNull(DV(rowNumber)("PRICING_ERROR")) AndAlso DV(rowNumber)("PRICING_ERROR").ToString.Trim.Length > 0 Then
                                        toolTipText = "Assigned to Examineer via Route and Failed BC Pricing"
                                        DetermineCellIconTooltip = 21
                                    Else
                                        toolTipText = "Assigned to Examineer via Route and Completed BC Pricing"
                                        DetermineCellIconTooltip = 19
                                    End If

                                Else
                                    toolTipText = "Assigned to Examineer via Route"
                                    DetermineCellIconTooltip = 1
                                End If

                            ElseIf Not IsDBNull(DV(rowNumber)("PRICED_BY")) AndAlso DV(rowNumber)("PRICED_BY").ToString.Trim.Length > 0 Then

                                If Not IsDBNull(DV(rowNumber)("PRICING_ERROR")) AndAlso DV(rowNumber)("PRICING_ERROR").ToString.Trim.Length > 0 Then
                                    toolTipText = "Failed BC Pricing"
                                    DetermineCellIconTooltip = 20
                                Else
                                    If CBool(DV(rowNumber)("DUPLICATE_SW")) = True Then
                                        toolTipText = "Completed Pricing (Possible Duplicate)"
                                        DetermineCellIconTooltip = 24
                                    Else
                                        toolTipText = "Completed BC Pricing"
                                        DetermineCellIconTooltip = 17
                                    End If
                                End If

                            Else
                                toolTipText = ""
                                DetermineCellIconTooltip = 9
                            End If
                        End If

                End Select
                'dv.Dispose()
                'dv = Nothing
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetClaimMetaData(ByVal claimID As Integer, Optional ByRef transaction As DbTransaction = Nothing) As DataRow
        Dim DS As DataSet

        Try
            DS = CMSDALFDBMD.RetrieveClaim(claimID, Nothing, Nothing, transaction)

            If DS IsNot Nothing AndAlso DS.Tables.Count > 0 AndAlso DS.Tables(0).Rows.Count > 0 Then
                Return DS.Tables(0).Rows(0)
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function IsFamilyLocked(ByVal familyID As Integer, Optional ByRef transaction As DbTransaction = Nothing) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="FamilyID"></param>
        ' <param name="Transaction"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/27/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DT As DataTable

        Try
            DT = CMSDALFDBMD.RetrieveFamilyLock(familyID, transaction)
            If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
                Return True
            End If

            Return False

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function ReleaseAuditItem(ByVal claimID As Integer, ByVal multiOpen As Boolean, Optional ByRef dialogAnswer As DialogResult = Nothing) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Releases Audit Item(s)
        ' </summary>
        ' <param name="Mode"></param>
        ' <param name="DocClass"></param>
        ' <param name="ClaimID"></param>
        ' <param name="Busy"></param>
        ' <param name="MultiOpen"></param>
        ' <param name="DialogAnswer"></param>
        ' <param name="Transaction"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	2/9/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim FinishTran As Boolean = False
        Dim PendUser As String = ""
        Dim BusyState As Boolean = False
        Dim ClaimMetaData As DataRow
        Dim YesAllDialog As New YesToAllDialog
        Dim NotMine As Boolean = False
        Dim NewItem As Boolean = False
        Dim HistSum As String
        Dim HistDetail As String
        Dim Transaction As DbTransaction

        Try
            Using WC As New GlobalCursor

                FinishTran = True
                Transaction = CMSDALCommon.BeginTransaction

                CMSDALFDBMD.LockClaimMaster(Transaction)

                ClaimMetaData = GetClaimMetaData(claimID, Transaction)

                If ClaimMetaData Is Nothing Then
                    CMSDALCommon.RollbackTransaction(Transaction)
                    MessageBox.Show("Item " & claimID & " is now locked by another user" & Microsoft.VisualBasic.vbCrLf &
                                    "or it has already been completed.", "Item Locked or Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                If CBool(ClaimMetaData("BUSY_SW")) Then
                    CMSDALCommon.RollbackTransaction(Transaction)
                    MessageBox.Show("Item " & claimID & " is now in use by another user." & Microsoft.VisualBasic.vbCrLf &
                                            "Please select another item.", "Item Already In Use", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    CMSDALFDBMD.BusyItem(claimID, _DomainUser.ToUpper, Transaction)
                    BusyState = True
                    CMSDALCommon.CommitTransaction(Transaction)
                End If

                If IsDBNull(ClaimMetaData("AUDITED_BY")) = False Then
                    PendUser = ClaimMetaData("AUDITED_BY").ToString.ToUpper
                Else
                    PendUser = ""
                End If

                If PendUser <> "" AndAlso PendUser.ToUpper <> _DomainUser.ToUpper Then
                    NotMine = True

                    If multiOpen Then
                        If IsNothing(dialogAnswer) OrElse (dialogAnswer <> DialogResult.Retry AndAlso dialogAnswer <> DialogResult.Abort) Then
                            dialogAnswer = YesAllDialog.ShowDialog(Me, "Item " & claimID & " is Assigned To:" & PendUser & Microsoft.VisualBasic.vbCrLf &
                                                                                                                       "Do You Want To Release It?", "Release Item")
                        End If
                    Else
                        dialogAnswer = MessageBox.Show("Item " & claimID & " is Assigned To:" & PendUser & Microsoft.VisualBasic.vbCrLf &
                                                       "Do You Want To Release It?", "Release Item",
                                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    End If

                    If dialogAnswer <> DialogResult.Yes AndAlso dialogAnswer <> DialogResult.Retry Then
                        Transaction = CMSDALCommon.BeginTransaction

                        CMSDALFDBMD.UnBusyItem(claimID, _DomainUser.ToUpper, Transaction)
                        BusyState = False

                        If Transaction IsNot Nothing AndAlso Transaction.Connection IsNot Nothing Then
                            CMSDALCommon.CommitTransaction(Transaction)
                        End If

                        Exit Try
                    End If
                ElseIf PendUser = "" Then
                    NotMine = True
                    NewItem = True

                    If multiOpen = True Then
                        If IsNothing(dialogAnswer) OrElse (dialogAnswer <> DialogResult.Retry AndAlso dialogAnswer <> DialogResult.Abort) Then
                            dialogAnswer = YesAllDialog.ShowDialog(Me, "Are You Sure Want To Release" & Microsoft.VisualBasic.vbCrLf &
                                                            "Document " & claimID,
                                                            "Release Item")
                        End If
                    Else
                        dialogAnswer = MessageBox.Show("Are You Sure Want To Release" & Microsoft.VisualBasic.vbCrLf &
                                                            "Document " & claimID,
                                                            "Release Item",
                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    End If

                    If dialogAnswer <> DialogResult.Yes AndAlso dialogAnswer <> DialogResult.Retry Then
                        Transaction = CMSDALCommon.BeginTransaction

                        CMSDALFDBMD.UnBusyItem(claimID, _DomainUser.ToUpper, Transaction)
                        BusyState = False

                        If Transaction IsNot Nothing AndAlso Transaction.Connection IsNot Nothing Then
                            CMSDALCommon.CommitTransaction(Transaction)
                        End If

                        Exit Try
                    End If
                End If

                FinishTran = True
                Transaction = CMSDALCommon.BeginTransaction

                CMSDALFDBMD.LockClaimMaster(Transaction)

                CMSDALFDBMD.PendToUser(claimID, _DomainUser.ToUpper, Transaction)

                If NotMine Then
                    HistSum = "CLAIM ID " & Format(claimID, "00000000") & " ASSIGNED TO AUDITOR " & _DomainUser.ToUpper
                    HistDetail = "FUNCTION: OPEN AUDIT ITEM" & Microsoft.VisualBasic.vbCrLf &
                                    "CLAIM ID: " & Format(claimID, "00000000") & Microsoft.VisualBasic.vbCrLf &
                                    "DOCID: " & ClaimMetaData("DOCID").ToString & Microsoft.VisualBasic.vbCrLf &
                                    "DOC CLASS: " & ClaimMetaData("DOC_CLASS").ToString & Microsoft.VisualBasic.vbCrLf &
                                    "DOC TYPE: " & ClaimMetaData("DOC_TYPE").ToString & Microsoft.VisualBasic.vbCrLf &
                                    "ASSIGNED TO AUDITOR: " & _DomainUser.ToUpper &
                                    If(NewItem = True, "", Microsoft.VisualBasic.vbCrLf & "TAKEN FROM: " & PendUser.ToUpper)
                    CMSDALFDBMD.CreateDocHistory(claimID, UFCWGeneral.IsNullLongHandler(ClaimMetaData("DOCID")), "GETNEXT", CInt(ClaimMetaData("FAMILY_ID")), CShort(ClaimMetaData("RELATION_ID")), UFCWGeneral.IsNullIntegerHandler(ClaimMetaData("PART_SSN")), UFCWGeneral.IsNullIntegerHandler(ClaimMetaData("PAT_SSN")), CStr(ClaimMetaData("DOC_CLASS")), CStr(ClaimMetaData("DOC_TYPE")), HistSum, HistDetail, _DomainUser.ToUpper, Transaction)
                End If

                CMSDALFDBMD.UpdateClaimMasterStatus(claimID, "WAITPROCESSING", _DomainUser.ToUpper, Transaction)

                HistSum = "CLAIM ID " & Format(claimID, "00000000") & " IS COMPLETE"
                HistDetail = "AUDITOR " & _DomainUser.ToUpper & " BULK RELEASED THIS DOCUMENT FROM AUDIT." & Microsoft.VisualBasic.vbCrLf &
                                                    "ENDING WORKFLOW PROCESSING."
                CMSDALFDBMD.CreateDocHistory(claimID, UFCWGeneral.IsNullLongHandler(ClaimMetaData("DOCID")), "AUDITRELEASE", CInt(ClaimMetaData("FAMILY_ID")), CShort(ClaimMetaData("RELATION_ID")), CInt(ClaimMetaData("PART_SSN")), CInt(ClaimMetaData("PAT_SSN")), CStr(ClaimMetaData("DOC_CLASS")), CStr(ClaimMetaData("DOC_TYPE")), HistSum, HistDetail, _DomainUser.ToUpper, Transaction)
                CMSDALFDBMD.UnBusyItem(claimID, _DomainUser.ToUpper, Transaction)

                BusyState = False
                CMSDALCommon.CommitTransaction(Transaction)
                Return True
            End Using

        Catch ex As Exception
            If Transaction IsNot Nothing Then
                CMSDALCommon.RollbackTransaction(Transaction)
            End If
            If BusyState Then
                CMSDALFDBMD.UnBusyItem(claimID, _DomainUser.ToUpper)
                BusyState = False
            End If
            Throw
        End Try
    End Function

    Private Sub FormatGridCells(ByVal sender As Object, ByVal e As DataGridFormatCellEventArgs)
        'Bold cell if New to current user

        Dim Mode As String
        Dim DR As DataRow
        Dim DV As DataView


        Try

            Mode = ModeComboBox.Text.ToUpper()

            DV = QueueDataGrid.GetDefaultDataView
            If DV IsNot Nothing Then
                DR = DV(e.RowNumber).Row
            End If
            If DR Is Nothing Then Exit Sub

            If Not IsDBNull(DR("USERID")) AndAlso Not IsDBNull(DR(If(Mode = "REGULAR", "PENDED_TO", "AUDITED_BY"))) Then
                If DR("USERID").ToString.Trim.ToUpper <> DR(If(Mode = "REGULAR", "PENDED_TO", "AUDITED_BY")).ToString.Trim.ToUpper Then
                    If IsDBNull(DR("LAST_ACCESSED_BY")) Then
                        e.TextFont = New Font(e.TextFont.Name, e.TextFont.Size, FontStyle.Bold)
                    ElseIf Not IsDBNull(DR("LAST_ACCESSED_BY")) AndAlso DR("LAST_ACCESSED_BY").ToString.Trim.ToUpper <> DR(If(Mode = "REGULAR", "PENDED_TO", "AUDITED_BY")).ToString.Trim.ToUpper Then
                        e.TextFont = New Font(e.TextFont.Name, e.TextFont.Size, FontStyle.Bold)
                    End If
                End If
            End If

        Catch ex As Exception
            Throw
        Finally
            DR = Nothing
        End Try

    End Sub
    Private Sub FormattingDate(ByRef value As Object, ByVal rowNum As Integer)
        Dim DV As DataView
        Try
            DV = QueueDataGrid.GetDefaultDataView
            If DV IsNot Nothing Then
                If Not IsDBNull(value) Then value = Format(value, "MM-dd-yyyy")
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function ClaimsCount(ByVal path As String) As Integer
        Dim Rank As Integer = 0
        Dim Settings As XmlReaderSettings
        Try
            Settings = New XmlReaderSettings()
            Settings.IgnoreComments = True
            Using XmlReader As XmlReader = XmlReader.Create(path, Settings)
                While (XmlReader.Read())
                    If (XmlReader.NodeType = XmlNodeType.Element AndAlso "Rank" = XmlReader.LocalName) Then
                        Rank += 1
                    End If
                End While
            End Using

            Return Rank

        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region
#Region "Style"

    Private Sub SetTableStyle(ByVal dg As DataGridCustom) 'called when no context menu is in use
        SetTableStyle(dg, Nothing, True)
    End Sub

    Private Sub SetTableStyle(ByVal dg As DataGridCustom, ByVal editable As Boolean) 'called when no context menu is in use, but editflag is supplied
        SetTableStyle(dg, Nothing, editable)
    End Sub
    Private Sub TableStyleReset(ByVal sender As Object, e As EventArgs)
        Dim dg As DataGridCustom = CType(sender, DataGridCustom)
        SetTableStyleColumns(dg, _EditableTextBox)
    End Sub

    Private Sub SetTableStyle(ByVal dg As DataGridCustom, ByVal dataGridCustomContextMenu As System.Windows.Forms.ContextMenuStrip, Optional ByVal editable As Boolean = True)
        Try
            _EditableTextBox = editable

            SetTableStyleColumns(dg, editable)
            dg.StyleName = Me.Name
            dg.AppKey = _APPKEY
            dg.ContextMenuPrepare(dataGridCustomContextMenu)

            RemoveHandler dg.ResetTableStyle, AddressOf TableStyleReset
            AddHandler dg.ResetTableStyle, AddressOf TableStyleReset

        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub SetTableStyleColumns(ByVal dg As DataGridCustom, ByVal editable As Boolean)

        Dim DGTS As DataGridTableStyle
        Dim DGTSDefault As DataGridTableStyle

        Dim BoolCol As DataGridColorBoolColumn
        Dim IconCol As DataGridHighlightIconColumn
        Dim TextCol As DataGridFormattableTextBoxColumn

        Dim ColsDV As DataView
        Dim DefaultStyleDS As DataSet
        Dim XMLStyleName As String
        Dim Resultdt As DataTable
        Dim ColumnSequenceDV As DataView

        Try

            XMLStyleName = ModeComboBox.Text.ToUpper & dg.Name

            DefaultStyleDS = DataGridCustom.GetTableStyle(XMLStyleName)

            DGTS = New DataGridTableStyle()

            If dg.GetCurrentDataTable Is Nothing Then
                DGTS.MappingName = dg.Name
            Else
                DGTS.MappingName = dg.GetCurrentDataTable.TableName
            End If

            DGTS.GridColumnStyles.Clear()

            If DefaultStyleDS.Tables.Contains(XMLStyleName & "Style") Then
                If DefaultStyleDS.Tables(XMLStyleName & "Style").Columns.Contains("GridLineStyle") Then
                    DGTS.GridLineStyle = If(CBool(DefaultStyleDS.Tables(XMLStyleName & "Style").Rows(0)("GridLineStyle").ToString) = True, DataGridLineStyle.Solid, DataGridLineStyle.None)
                End If
                If DefaultStyleDS.Tables(XMLStyleName & "Style").Columns.Contains("RowHeadersVisible") Then
                    DGTS.RowHeadersVisible = CBool(DefaultStyleDS.Tables(XMLStyleName & "Style").Rows(0)("RowHeadersVisible").ToString)
                End If
            End If

            Resultdt = dg.LoadColumnsSizeAndPosition(dg.Name & "\" & DGTS.MappingName & "\ColumnSettings", DefaultStyleDS.Tables("Column"))

            ColumnSequenceDV = New DataView(Resultdt)
            ColsDV = ColumnSequenceDV

            If dg.GetCurrentDataTable IsNot Nothing Then
                If dg.GetCurrentDataTable.Columns.Contains("Icon") Then
                    IconCol = New DataGridHighlightIconColumn(dg, QueueImageList)
                    IconCol.HeaderText = ""
                    IconCol.MappingName = "Icon"
                    IconCol.NullText = ""

                    If QueueImageList.Images.Count > 0 Then
                        IconCol.Width = QueueImageList.Images(0).Size.Width
                    Else
                        IconCol.Width = 16
                    End If

                    IconCol.MaximumCharWidth = IconCol.Width
                    IconCol.MinimumCharWidth = IconCol.Width

                    AddHandler IconCol.PaintCellPicture, AddressOf DetermineCellIcon

                    DGTS.GridColumnStyles.Add(IconCol)
                End If
            End If

            DGTSDefault = New DataGridTableStyle() 'This can be used to establish the columns displayed by default
            DGTSDefault.MappingName = "Default"

            For IntCol As Integer = 0 To ColsDV.Count - 1

                If (IsDBNull(ColsDV(IntCol).Item("Visible")) OrElse ColsDV(IntCol).Item("Visible").ToString.Trim.Length = 0 OrElse CBool(ColsDV(IntCol).Item("Visible")) = True) Then
                    TextCol = New DataGridFormattableTextBoxColumn
                    TextCol.MappingName = ColsDV(IntCol).Item("Mapping").ToString
                    TextCol.HeaderText = ColsDV(IntCol).Item("HeaderText").ToString
                    If IsDBNull(ColsDV(IntCol).Item("Format")) = False AndAlso ColsDV(IntCol).Item("Format").ToString.Trim.Length > 0 Then
                        TextCol.Format = ColsDV(IntCol).Item("Format").ToString
                    End If
                    DGTSDefault.GridColumnStyles.Add(TextCol)
                End If

                If ((IsDBNull(ColsDV(IntCol).Item("Visible")) OrElse ColsDV(IntCol).Item("Visible").ToString.Trim.Length = 0 OrElse CBool(ColsDV(IntCol).Item("Visible")) = True) AndAlso (GetAllSettings(_APPKEY, dg.Name & "\" & DGTS.MappingName & "\Customize\ColumnSelector") Is Nothing OrElse CDbl(GetSetting(_APPKEY, dg.Name & "\" & DGTS.MappingName & "\Customize\ColumnSelector", "Col " & ColsDV(IntCol).Item("Mapping").ToString & " Customize", CStr(0))) = 1)) Then
                    If ColsDV(IntCol).Item("Type").ToString = "Text" Or Not editable Then
                        TextCol = New DataGridFormattableTextBoxColumn(IntCol)
                        TextCol.MappingName = ColsDV(IntCol).Item("Mapping").ToString
                        TextCol.HeaderText = ColsDV(IntCol).Item("HeaderText").ToString
                        TextCol.Width = CInt(ColsDV(IntCol).Item("SizeInPixels"))
                        TextCol.NullText = ColsDV(IntCol).Item("NullText").ToString
                        TextCol.TextBox.WordWrap = True

                        Try

                            If Not IsDBNull(ColsDV(IntCol).Item("ReadOnly")) AndAlso ColsDV(IntCol).Item("ReadOnly").ToString.Trim.Length > 0 AndAlso CBool(ColsDV(IntCol).Item("ReadOnly")) = True Then
                                TextCol.ReadOnly = True
                            End If

                        Catch ex As Exception
                        End Try

                        If IsDBNull(ColsDV(IntCol).Item("Format")) = False Then
                            If ColsDV(IntCol).Item("Format").ToString = "MM-dd-yyyy" Then
                                AddHandler TextCol.Formatting, AddressOf FormattingDate
                            ElseIf ColsDV(IntCol).Item("Format").ToString = "YesNo" Then
                                AddHandler TextCol.Formatting, AddressOf DataGridCustom.FormattingYesNo
                            ElseIf ColsDV(IntCol).Item("Format").ToString <> "" Then
                                TextCol.Format = ColsDV(IntCol).Item("Format").ToString
                            End If
                        End If

                        AddHandler TextCol.SetCellFormat, AddressOf FormatGridCells

                        DGTS.GridColumnStyles.Add(TextCol)

                    ElseIf ColsDV(IntCol).Item("Type").ToString.Contains("Button") Then

                        _ButtCol = New DataGridHighlightButtonColumn(dg)
                        _ButtCol.MappingName = ColsDV(IntCol).Item("Mapping").ToString
                        _ButtCol.HeaderText = ColsDV(IntCol).Item("HeaderText").ToString
                        _ButtCol.Width = CInt(ColsDV(IntCol).Item("SizeInPixels"))
                        _ButtCol.NullText = ColsDV(IntCol).Item("NullText").ToString

                        _GenericDelegate = [Delegate].CreateDelegate(GetType(System.EventHandler), Me, ColsDV(IntCol).Item("Method").ToString)
                        AddHandler _ButtCol.ColumnButton.Click, CType(_GenericDelegate, Global.System.EventHandler)

                        DGTS.PreferredRowHeight = _ButtCol.ColumnButton.Height + 2
                        DGTS.GridColumnStyles.Add(_ButtCol)

                    ElseIf ColsDV(IntCol).Item("Type").ToString.Contains("Bool") Then
                        BoolCol = New DataGridColorBoolColumn(IntCol)
                        BoolCol.MappingName = ColsDV(IntCol).Item("Mapping").ToString
                        BoolCol.HeaderText = ColsDV(IntCol).Item("HeaderText").ToString
                        BoolCol.Width = CInt(GetSetting(_APPKEY, dg.Name & "\" & DGTS.MappingName.ToString & "\ColumnSettings", "Col " & ColsDV(IntCol).Item("Mapping").ToString, CStr(UFCWGeneral.MeasureWidthinPixels(CInt(ColsDV(IntCol).Item("DefaultCharWidth")), dg.Font.Name, dg.Font.Size))))
                        BoolCol.NullText = ColsDV(IntCol).Item("NullText").ToString
                        BoolCol.TrueValue = CType("1", Decimal)
                        BoolCol.FalseValue = CType("0", Decimal)
                        BoolCol.AllowNull = False

                        Try

                            If Not IsDBNull(ColsDV(IntCol).Item("ReadOnly")) AndAlso CBool(ColsDV(IntCol).Item("ReadOnly")) = True Then
                                BoolCol.ReadOnly = True
                            End If

                        Catch ex As Exception
                        End Try

                        DGTS.GridColumnStyles.Add(BoolCol)
                    End If
                End If

            Next

            dg.TableStyles.Clear()
            dg.TableStyles.Add(DGTS)
            dg.TableStyles.Add(DGTSDefault)

        Catch ex As Exception
            Throw
        End Try

    End Sub

#End Region

#Region "Plug In Code"

    Private Sub SharedInterfacesMessage(StatusMessage As String)

        If _TraceMessaging.Enabled Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & " : " & StatusMessage & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceMessaging" & vbTab)

        _QueueSharedInterfacesMessage.StatusMessage(StatusMessage)

        'Task.Factory.StartNew(Sub() _QueueSharedInterfacesMessage.StatusMessage(StatusMessage))

    End Sub

    Public Sub QueueStatusMessage(ByVal statusMessage As String) Implements SharedInterfaces.IMessage.StatusMessage
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' plugins inteface this sub to update status messages to display on this form
        ' </summary>
        ' <param name="msg"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        'required by interface

        SharedInterfacesMessage(statusMessage)

    End Sub

    Private Sub PlugInLoading(ByVal plugIn As SharedInterfaces.PlugInAttribute, ByRef cancel As Boolean) Handles _PlugInController.PlugInLoading

        ' -----------------------------------------------------------------------------
        ' <summary>
        ' this is an event called from plugin controller to allow any controls to be added
        ' that will trigger plugins
        ' </summary>
        ' <param name="PlugIn"></param>
        ' <param name="Cancel"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            If plugIn.Destination.ToLower = "queue" Then
                'No Controls needed to trigger plugins.  plugins triggered by queue items docclass.
            Else
                cancel = True
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function OpenPlugIn(ByVal plugInName As String, ByVal claimID As Integer, ByVal mode As String, Optional ByRef transaction As DbTransaction = Nothing) As Boolean

        ' -----------------------------------------------------------------------------
        ' <summary>
        ' this function maintains open children and limits the number of forms that can open.
        ' opening too many child forms causes a crash
        ' </summary>
        ' <param name="PlugInName"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim UID As String
        Dim PlugInForm As Form
        Dim P As PropertyInfo

        Try
            PlugInForm = _PlugInController.LaunchPlugIn(plugInName, CType(Me, SharedInterfaces.IMessage), claimID, mode.ToUpper, transaction)

            If PlugInForm IsNot Nothing Then
                AddHandler PlugInForm.FormClosing, AddressOf PlugInForm_FormClosing

                If _OpenChildrenCounts.Contains(PlugInForm.Name) Then
                    _OpenChildrenCounts.Item(PlugInForm.Name) = CInt(_OpenChildrenCounts.Item(PlugInForm.Name)) + 1
                Else
                    _OpenChildrenCounts.Add(PlugInForm.Name, 1)
                End If

                _OpenChildrenCount += 1
                _TotalOpened += 1

                P = PlugInForm.GetType.GetProperty("UniqueID")
                If P IsNot Nothing Then
                    UID = _UniqueID & "-Q" & _TotalOpened
                    PlugInForm.GetType.InvokeMember("UniqueID", BindingFlags.SetProperty, Nothing, PlugInForm, New Object() {UID})
                    _Children.Add(UID, PlugInForm)
                End If
                PlugInForm.Show()

                Return True
            End If

            Return False

        Catch ex As Exception
            Throw New Exception("Plugin: " & plugInName & " " & If(ex.InnerException IsNot Nothing, ex.InnerException.Message, "") & " " & ex.Message)
        End Try
    End Function

#End Region

    Private Sub NewClaimButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewClaimButton.Click

        Dim NewClaimForm As NewClaimForm

        Try
            NewClaimButton.Enabled = False

            NewClaimForm = New NewClaimForm
            NewClaimForm.ShowDialog(Me)

        Catch ex As Exception
            Throw
        Finally

            If NewClaimForm IsNot Nothing Then NewClaimForm.Dispose()
            NewClaimForm.Dispose()

            NewClaimButton.Enabled = True
        End Try
    End Sub

    Private Sub QueueDataGrid_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles QueueDataGrid.MouseMove

        Dim ToolTipText As String
        Dim HTI As DataGrid.HitTestInfo
        Dim DG As DataGridCustom

        Try
            DG = CType(sender, DataGridCustom)

            HTI = DG.HitTest(e.X, e.Y)

            ' Do not display hover text if it is a drag event
            If (e.Button <> MouseButtons.Left) Then

                ' Check if the target is a different cell from the previous one
                If HTI.Type = DataGrid.HitTestType.Cell AndAlso
                  HTI.Row <> _HoverCell.RowNumber OrElse HTI.Column <> _HoverCell.ColumnNumber Then

                    ' Store the new hit row
                    _HoverCell.RowNumber = HTI.Row
                    _HoverCell.ColumnNumber = HTI.Column

                End If

            End If

            If _HoverCell.RowNumber > -1 AndAlso _HoverCell.RowNumber <= (DG.GetGridRowCount) Then
                If _HoverCell.ColumnNumber = 0 Then
                    DetermineCellIconTooltip(_HoverCell.RowNumber, ToolTipText)
                End If
            End If

            If ToolTipText IsNot Nothing AndAlso ToolTipText.ToString.Trim.Length > 0 Then
                If String.Compare(Me.ToolTip1.GetToolTip(DG), ToolTipText.ToString) <> 0 Then
                    ToolTip1.Active = True
                    ToolTip1.SetToolTip(DG, ToolTipText.ToString)
                End If
            Else
                ToolTip1.Active = False
                ToolTip1.SetToolTip(DG, "")
            End If

        Catch ex As Exception
        End Try

    End Sub

    Private Sub OpenHistoryItem_Click(ByVal sender As Object, ByVal claimID As Integer) Handles ClaimHistoryFrm.OpenClaim
        Try

            OpenWorkScreen(_LastMode.ToUpper, _LastDocClass, claimID, False, False, Nothing)

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub RefreshQueueQuery(callBack As RefreshQueueResultsDelegate, ByVal mode As String, ByVal employeeAccess As Boolean, ByVal userRights As String, ByVal docClass As String, ByVal docType As String, ByVal myAssignments As String, ByVal maxItems As Integer?, ByVal identifier As String, ByVal familyID As Integer?, ByVal docID As Integer?, ByVal batch As String, ByVal partSSN As Integer?, ByVal tin As Integer?)

        Dim ResultSetDT As DataTable

        Try

            Using WC As New GlobalCursor

                Using StopwatchTimer As Threading.Timer = New Threading.Timer(AddressOf Tick, Nothing, 0, 100)

                    If mode.ToUpper = "AUDIT" Then
                        ResultSetDT = CMSDALFDBMD.RetrieveAuditQueue(employeeAccess, userRights, docClass, docType, myAssignments, maxItems, identifier, familyID, docID, batch, partSSN, tin)
                    Else
                        ResultSetDT = CMSDALFDBMD.RetrieveQueue(employeeAccess, userRights, docClass, docType, myAssignments, maxItems, identifier, familyID, docID, batch, partSSN, tin)
                    End If

                    callBack(ResultSetDT)

                End Using
            End Using

        Catch ex As Exception
            If System.Threading.Thread.CurrentThread.ThreadState <> ThreadState.AbortRequested Then
                Throw
            End If
        Finally

        End Try

    End Sub

    Private Sub RefreshQueueResultsHandler(queueResultsDT As DataTable)
        Try
            If Me.InvokeRequired Then
                BeginInvoke(New RefreshQueueResultsDelegate(AddressOf RefreshQueueResults), New Object() {queueResultsDT})
            Else
                RefreshQueueResults(queueResultsDT)
            End If
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub RefreshQueueResults(queueResultsDT As DataTable)

        Dim Elapsed As TimeSpan
        Dim FinishDuration As String

        Try

            _QueueDataTable = queueResultsDT

            Elapsed = _Stopwatch.Elapsed
            FinishDuration = String.Format("{0:00}:{1:00}.{2:000}",
                                       Elapsed.Minutes,
                                       Elapsed.Seconds,
                                       Elapsed.Milliseconds)

            If _Cancel Then

                _LastOutMessage = "User Cancelled Refresh After " & FinishDuration
                _QueueSharedInterfacesMessage.StatusMessage(_LastOutMessage)

                QueueDataGrid.CaptionText = StrConv("0 - Pending Work Item In " & _LastDocClass & If(_LastDocType = "", "", ":" & _LastDocType) & If(ModeComboBox.Text.ToUpper <> "REGULAR", " (" & ModeComboBox.Text.ToUpper & ")", ""), VbStrConv.ProperCase)

                MessageBox.Show(_LastOutMessage, "User Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Else
                If _QueueDataTable IsNot Nothing Then

                    Using WC As New GlobalCursor

                        _QueueDataTable = PopulateICON(_QueueDataTable)

                        QueueDataGrid.SuspendLayout()

                        AddHandler _QueueBS.CurrentChanged, AddressOf QueueBindingSource_CurrentChanged

                        _QueueBS.DataSource = _QueueDataTable
                        QueueDataGrid.DataSource = _QueueBS

                        SetTableStyle(QueueDataGrid, QueueDataGridCustomContextMenu, True)

                        QueueDataGrid.Sort = If(QueueDataGrid.LastSortedBy, QueueDataGrid.DefaultSort(ModeComboBox.Text.ToUpper & QueueDataGrid.Name))

                        QueueDataGrid.ResumeLayout()

                        QueueDataGrid.Focus()

                        _LastOutMessage = "Found " & _QueueDataTable.Rows.Count & " Item" & If(_QueueDataTable.Rows.Count <> 1, "s", "") & " in " & FinishDuration
                        _QueueSharedInterfacesMessage.StatusMessage(_LastOutMessage)

                        QueueDataGrid.CaptionText = StrConv(_QueueDataTable.Rows.Count & " - Pending Work Item" & If(_QueueDataTable.Rows.Count <> 1, "s", "") & " In " & _LastDocClass & If(_LastDocType = "", "", ":" & _LastDocType) & If(ModeComboBox.Text.ToUpper <> "REGULAR", " (" & ModeComboBox.Text.ToUpper & ")", ""), VbStrConv.ProperCase)

                    End Using

                Else
                    _LastOutMessage = "Nothing Found After " & FinishDuration
                    _QueueSharedInterfacesMessage.StatusMessage(_LastOutMessage)

                    QueueDataGrid.CaptionText = StrConv("0 - Pending Work Item In " & _LastDocClass & If(_LastDocType = "", "", ":" & _LastDocType) & If(ModeComboBox.Text.ToUpper <> "REGULAR", " (" & ModeComboBox.Text.ToUpper & ")", ""), VbStrConv.ProperCase)
                End If
            End If

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Sub
    Private Sub QueueBindingSource_CurrentChanged(sender As Object, e As System.EventArgs)

        Dim BS As BindingSource
        Dim DR As DataRow

        Try

            BS = CType(sender, BindingSource)

            If BS.Current Is Nothing Then Return

            DR = CType(BS.Current, DataRowView).Row


        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub ClaimHistoryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClaimHistoryToolStripMenuItem.Click

        Try

            ClaimHistoryFrm = New ClaimHistory

            ClaimHistoryFrm.Show(Me)

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Sub

    Private Sub RecentDocumentsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RecentDocumentsToolStripMenuItem.Click

        ' -----------------------------------------------------------------------------
        ' <summary>
        ' makes a call to display the recent documents form of the FileNet Display
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/14/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DisplayRecentDocumentsFrm As RecentDocsViewer

        Try

            DisplayRecentDocumentsFrm = New RecentDocsViewer
            DisplayRecentDocumentsFrm.Show(Me)

        Catch ex As Exception
            MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub
    Private Sub UCaseBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Converts the value to upper case
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	9/8/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim eOriginal As String = e.Value.ToString

        Try
            If Not IsDBNull(e.Value) AndAlso e.Value IsNot Nothing AndAlso e.Value.ToString.Trim.Length > 0 AndAlso CStr(e.Value) <> CStr(e.Value).ToUpper Then
                e.Value = CStr(e.Value).ToUpper
#If TRACE Then
                If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(CType(sender, Binding).BindableComponent, TextBox).Name & " Proposed: " & CStr(TryCast(e.Value, String)) & " Original: " & eOriginal & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub UCaseBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	9/8/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim eOriginal As String = e.Value.ToString

        Try
            If Not IsDBNull(e.Value) AndAlso CStr(e.Value).Trim.Length > 0 AndAlso e.Value.ToString.ToUpper <> CStr(e.Value).ToUpper Then
                e.Value = CStr(e.Value).ToUpper
            Else
                e.Value = Nothing
            End If

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(CType(sender, Binding).BindableComponent, TextBox).Name & " Proposed: " & CStr(TryCast(e.Value, String)) & " Original: " & eOriginal & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
        End Try
    End Sub
    Private Sub BindingCompleteEventHandler(ByVal sender As Object, ByVal e As System.Windows.Forms.BindingCompleteEventArgs)
        Try
            If Not e.BindingCompleteState = BindingCompleteState.Success Then
                MessageBox.Show("Control " & e.Binding.Control.Name & " " & e.ErrorText, "Problem converting data to database format", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub SSNBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' adjusts SSN values entered for a databinding
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim eOriginal As String = e.Value.ToString

        Try
            If Not IsDBNull(e.Value) AndAlso Not IsNumeric(e.Value) Then
                e.Value = UFCWGeneral.UnFormatSSN(CStr(e.Value))
            End If

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(CType(sender, Binding).BindableComponent, TextBox).Name & " Proposed: " & CStr(TryCast(e.Value, String)) & " Original: " & eOriginal & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
        End Try
    End Sub
    Private Sub SSNBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' formats SSN values entered for a databinding
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim eOriginal As String = e.Value.ToString

        Try
            If Not IsDBNull(e.Value) AndAlso e.Value IsNot Nothing AndAlso e.Value.ToString.Trim.Length > 0 AndAlso IsNumeric(e.Value) Then
                e.Value = UFCWGeneral.FormatSSN(CStr(e.Value))
            End If

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(CType(sender, Binding).BindableComponent, TextBox).Name & " Proposed: " & CStr(TryCast(e.Value, String)) & " Original: " & eOriginal & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
        End Try
    End Sub
End Class

'#Region "BackThread Class"
'Public Class ExecuteQuery
'    Public _Mode As String
'    Public _EmployeeAccess As Boolean
'    Public _UserRights As String
'    Public _DocClass As String
'    Public _DocType As String
'    Public _MyAssignments As String
'    Public _MaxItems As Integer?
'    Public _ClaimID As String
'    Public _FamilyID As Integer?
'    Public _DocID As Integer?
'    Public _Batch As String
'    Public _PartSSN As Integer?
'    Public _TIN As Integer?
'    Public ResultSet As DataTable

'    Sub New(ByVal Mode As String, ByVal EmployeeAccess As Boolean, ByVal UserRights As String, ByVal DocClass As String, Optional ByVal DocType As String = Nothing, Optional ByVal MyAssignments As String = Nothing, Optional ByVal MaxItems As Integer? = Nothing, Optional ByRef ClaimID As String = Nothing, Optional ByVal FamilyID As Integer? = Nothing, Optional ByVal DocID As Integer? = Nothing, Optional ByVal Batch As String = Nothing, Optional ByVal PartSSN As Integer? = Nothing, Optional ByVal TIN As Integer? = Nothing)
'        _Mode = Mode
'        _EmployeeAccess = EmployeeAccess
'        _UserRights = UserRights
'        _DocClass = DocClass
'        _DocType = DocType
'        _MyAssignments = MyAssignments
'        _MaxItems = MaxItems
'        _ClaimID = ClaimID
'        _FamilyID = FamilyID
'        _DocID = DocID
'        _Batch = Batch
'        _PartSSN = PartSSN
'        _TIN = TIN
'    End Sub

'    Public Sub Execute()
'        Try
'            If _Mode.ToUpper = "AUDIT" Then
'                ResultSet = CMSDALFDBMD.RetrieveAuditQueue(_EmployeeAccess, _UserRights, _DocClass, _DocType, _MyAssignments, _MaxItems, _ClaimID, _FamilyID, _DocID, _Batch, _PartSSN, _TIN)
'            Else
'                ResultSet = CMSDALFDBMD.RetrieveQueue(_EmployeeAccess, _UserRights, _DocClass, _DocType, _MyAssignments, _MaxItems, _ClaimID, _FamilyID, _DocID, _Batch, _PartSSN, _TIN)
'            End If

'        Catch ex As Exception
'            If System.Threading.Thread.CurrentThread.ThreadState <> ThreadState.AbortRequested Then
'                Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
'                If (rethrow) Then
'                    MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
'                End If
'            End If
'        End Try
'    End Sub
'End Class

'Public Class DurationTimer
'    Private Shared _Timer As System.Timers.Timer
'    Private Shared _Stopwatch As New Stopwatch
'    Private Shared _SharedInterfacesMessage As SharedInterfaces.IMessage
'    Public Shared Sub Main(SharedInterfacesMessage As SharedInterfaces.IMessage)

'        _SharedInterfacesMessage = SharedInterfacesMessage
'        ' Create a timer with a two second interval.
'        _Timer = New System.Timers.Timer(1000)
'        ' Hook up the Elapsed event for the timer.
'        AddHandler _Timer.Elapsed, AddressOf OnTimedEvent

'        _Timer.Enabled = True

'    End Sub

'    ' The event handler for the Timer.Elapsed event.
'    Private Shared Sub OnTimedEvent(source As Object, e As ElapsedEventArgs)
'        Dim elapsed As TimeSpan = _Stopwatch.Elapsed

'        _SharedInterfacesMessage.StatusMessage("Refresh Duration " & String.Format("{0:00}:{1:00}:{2:00}.{3:000}", _
'                                   Math.Floor(elapsed.TotalHours), _
'                                   elapsed.Minutes, _
'                                   elapsed.Seconds, _
'                                   elapsed.Milliseconds))
'    End Sub
'End Class
'#End Region