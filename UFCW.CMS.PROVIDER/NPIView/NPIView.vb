Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Security.Principal


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
<SharedInterfaces.PlugIn("NPIView", "Provider")> Public Class NPIView
    Inherits System.Windows.Forms.Form

    Const APPKEY As String = "UFCW\Provider\NPIView"
    Const SHOWLOADTIME As Boolean = False

    Private DoSelectAll As Boolean = False

    Private bizUserPrincipal As WindowsPrincipal

    ReadOnly DomainUser As String = SystemInformation.UserName

    Private _NPI As Decimal
    Private _UniqueID As String

    Private LastAlertIndex As Integer = -1
    Private Activating As Boolean = False

    Private mobjMessage As SharedInterfaces.IMessage

    Private lastAddressIndex As Integer = -1
    Private AddressNeedsValidation As Boolean = False

    Friend WithEvents NpiRegistryControl As NPIRegistryControl
    Friend WithEvents ExitButton As System.Windows.Forms.Button

    Private hoverCell As New DataGridCell

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal objMsg As SharedInterfaces.IMessage, ByVal NPI As Decimal)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        mobjMessage = objMsg
        _NPI = NPI

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
    'System.Windows.Forms.TextBox
    Friend WithEvents AboutMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents FileMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents AccumulatorsGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents WorkStatusBar As System.Windows.Forms.StatusBar
    Friend WithEvents InfoStatusBarPanel As System.Windows.Forms.StatusBarPanel
    Friend WithEvents DomainUserStatusBarPanel As System.Windows.Forms.StatusBarPanel
    Friend WithEvents DataStatusBarPanel As System.Windows.Forms.StatusBarPanel
    Friend WithEvents DateStatusBarPanel As System.Windows.Forms.StatusBarPanel
    Friend WithEvents MenuExit As System.Windows.Forms.MenuItem
    Friend WithEvents ProviderHistory As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumn1 As System.Windows.Forms.DataGridTextBoxColumn
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(NPIView))
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.FileMenuItem = New System.Windows.Forms.MenuItem
        Me.MenuExit = New System.Windows.Forms.MenuItem
        Me.MenuItem5 = New System.Windows.Forms.MenuItem
        Me.AboutMenuItem = New System.Windows.Forms.MenuItem
        Me.MenuItem11 = New System.Windows.Forms.MenuItem
        Me.MenuItem12 = New System.Windows.Forms.MenuItem
        Me.MenuItem13 = New System.Windows.Forms.MenuItem
        Me.MenuItem14 = New System.Windows.Forms.MenuItem
        Me.MenuItem15 = New System.Windows.Forms.MenuItem
        Me.MenuItem16 = New System.Windows.Forms.MenuItem
        Me.MenuItem17 = New System.Windows.Forms.MenuItem
        Me.MenuItem18 = New System.Windows.Forms.MenuItem
        Me.MenuItem19 = New System.Windows.Forms.MenuItem
        Me.MenuItem20 = New System.Windows.Forms.MenuItem
        Me.ProviderHistory = New System.Windows.Forms.DataGridTableStyle
        Me.DataGridTextBoxColumn1 = New System.Windows.Forms.DataGridTextBoxColumn
        Me.AccumulatorsGroupBox = New System.Windows.Forms.GroupBox
        Me.WorkStatusBar = New System.Windows.Forms.StatusBar
        Me.InfoStatusBarPanel = New System.Windows.Forms.StatusBarPanel
        Me.DomainUserStatusBarPanel = New System.Windows.Forms.StatusBarPanel
        Me.DataStatusBarPanel = New System.Windows.Forms.StatusBarPanel
        Me.DateStatusBarPanel = New System.Windows.Forms.StatusBarPanel
        Me.ExitButton = New System.Windows.Forms.Button
        Me.NpiRegistryControl = New NPIRegistryControl
        CType(Me.InfoStatusBarPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DomainUserStatusBarPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataStatusBarPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateStatusBarPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.FileMenuItem, Me.MenuItem5})
        '
        'FileMenuItem
        '
        Me.FileMenuItem.Index = 0
        Me.FileMenuItem.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuExit})
        Me.FileMenuItem.Text = "File"
        '
        'MenuExit
        '
        Me.MenuExit.Index = 0
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
        'ProviderHistory
        '
        Me.ProviderHistory.DataGrid = Nothing
        Me.ProviderHistory.HeaderForeColor = System.Drawing.SystemColors.ControlText
        '
        'DataGridTextBoxColumn1
        '
        Me.DataGridTextBoxColumn1.Format = ""
        Me.DataGridTextBoxColumn1.FormatInfo = Nothing
        Me.DataGridTextBoxColumn1.Width = 75
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
        'WorkStatusBar
        '
        Me.WorkStatusBar.Location = New System.Drawing.Point(0, 791)
        Me.WorkStatusBar.Name = "WorkStatusBar"
        Me.WorkStatusBar.Panels.AddRange(New System.Windows.Forms.StatusBarPanel() {Me.InfoStatusBarPanel, Me.DomainUserStatusBarPanel, Me.DataStatusBarPanel, Me.DateStatusBarPanel})
        Me.WorkStatusBar.ShowPanels = True
        Me.WorkStatusBar.Size = New System.Drawing.Size(532, 16)
        Me.WorkStatusBar.SizingGrip = False
        Me.WorkStatusBar.TabIndex = 101
        '
        'InfoStatusBarPanel
        '
        Me.InfoStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring
        Me.InfoStatusBarPanel.Name = "InfoStatusBarPanel"
        Me.InfoStatusBarPanel.Width = 502
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
        'ExitButton
        '
        Me.ExitButton.CausesValidation = False
        Me.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ExitButton.Location = New System.Drawing.Point(455, 760)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(72, 23)
        Me.ExitButton.TabIndex = 103
        Me.ExitButton.Text = "Exit"
        '
        'NpiRegistryControl
        '
        Me.NpiRegistryControl.Location = New System.Drawing.Point(3, 3)
        Me.NpiRegistryControl.Name = "NpiRegistryControl"
        Me.NpiRegistryControl.Size = New System.Drawing.Size(524, 751)
        Me.NpiRegistryControl.TabIndex = 102
        '
        'NPIView
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.AutoScroll = True
        Me.CancelButton = Me.ExitButton
        Me.ClientSize = New System.Drawing.Size(532, 807)
        Me.Controls.Add(Me.ExitButton)
        Me.Controls.Add(Me.NpiRegistryControl)
        Me.Controls.Add(Me.WorkStatusBar)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Menu = Me.MainMenu1
        Me.Name = "NPIView"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "NPI Registry Info"
        CType(Me.InfoStatusBarPanel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DomainUserStatusBarPanel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataStatusBarPanel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateStatusBarPanel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Private Sub ExitButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MenuExit.Click, ExitButton.Click

        Me.Close()

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
    Public Event OpenNPI(ByVal sender As Object, ByVal e As NPIEventArgs)
    Private Sub Work_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            'Init Active Directory
            AppDomain.CurrentDomain.SetPrincipalPolicy(Security.Principal.PrincipalPolicy.WindowsPrincipal)
            bizUserPrincipal = CType(System.Threading.Thread.CurrentPrincipal, WindowsPrincipal)

            LoadNPI()

            DomainUserStatusBarPanel.Text = SystemInformation.UserName
            DataStatusBarPanel.Text = "Server=" & CMSDALCommon.GetServerName(Nothing) & ";DB=" & CMSDALCommon.GetDatabaseName(Nothing)
            DateStatusBarPanel.Text = Format(Now, "MM-dd-yyyy")

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

#End Region

#Region "Load Form"
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
    Private Sub LoadNPI()
        Try
            Me.Cursor = Cursors.WaitCursor

            Me.NpiRegistryControl.NPI = _NPI
            Me.NpiRegistryControl.LoadNPI()

        Catch ex As Exception
            Me.Cursor = Cursors.Default

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub AboutMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutMenuItem.Click

        Dim AboutF As New AboutWork

        AboutF.ShowDialog(Me)
        AboutF.Dispose()
        AboutF = Nothing

    End Sub

#End Region

End Class

Public Class NPIEventArgs
    Inherits EventArgs
    Private _NPI As Decimal

    'Constructor.
    '
    Public Sub New(ByVal NPI As Decimal)
        Me._NPI = NPI
    End Sub

    '
    Public ReadOnly Property NPI() As Decimal
        Get
            Return _NPI
        End Get
    End Property

End Class



