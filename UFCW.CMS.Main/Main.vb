Option Strict On

Imports UFCW.WCF
Imports System.Configuration
Imports System.Reflection
Imports System.Security.Principal
Imports System.Threading
Imports UFCW.WCF.FileNet
Imports System.Threading.Tasks
Imports System.IO
Imports System.Deployment.Application
Imports Microsoft.Win32

''' -----------------------------------------------------------------------------
''' Project	 : Main
''' Class	 : Claims.UI.Main
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' Main Form to Plug components into
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[Nick Snyder]	2/15/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public NotInheritable Class Main
    Inherits System.Windows.Forms.Form
    Implements SharedInterfaces.IMessage

    Private Shared _TraceMessaging As New BooleanSwitch("TraceMessaging", "Trace Switch in App.Config")
    Private Shared _TraceParallel As New TraceSwitch("TraceParallel", "Parallel Trace Switch in App.Config", "0")
    Private Shared _TraceCloning As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private Shared _APPKEY As String = "UFCW\Claims\"

    Private Shared _SingleInstance As Boolean = False

    Const _CRYPTORKEY As String = "UFCW CMS ACCESS"
    Const MAXFORMS As Integer = 25
    Const MAXGUIHANDLES As Long = 8975933078237088100
    Const PLUINFILTER As String = "UFCW.CMS.*.Plugin.dll"

    Private WithEvents _PlugInController As PlugInController


    Private _Frm As Form

    Private _OpenChildrenCount As Integer = 0
    Private _TotalOpened As Integer = 0
    Private _OpenChildrenCounts As New Hashtable
    Private _UserPrincipal As WindowsPrincipal

    Private _FNUserName As String '= "UFCWEMP"
    Private _FNPWD As String '= "ufcwpw"

    Private _Children As New Hashtable

    Private _ComparefolderLoc As String = ConfigurationManager.AppSettings("ComparefolderLoc")
    Private _ExcludeXMLFiles As String = ConfigurationManager.AppSettings("ExcludeXMLFiles")

    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WindowToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ArrangeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ResetWindowsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HorizontalToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents VerticalToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CascadeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

    'supports handling multi threaded calls
    Delegate Sub StatusMessageInvokeDelegate(StatusMessage As String)
    Private StatusMessageHandler As StatusMessageInvokeDelegate = AddressOf UpdateInfoStatusBarPanel_Text

    Delegate Sub CursorInvokeDelegate(Cursor As Cursor)
    Private CursorHandler As CursorInvokeDelegate = AddressOf UpdateCursor
    Friend WithEvents Timer1 As Windows.Forms.Timer
    Friend WithEvents ResetTempFilesToolStripMenuItem As ToolStripMenuItem

    Shared _MainMutex As Mutex

#Region "Public Properties"
    <System.ComponentModel.Description("Represents the application key used when accessing the registry.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
        End Set
    End Property
#End Region

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
            If _Frm IsNot Nothing Then
                _Frm.Dispose()
            End If
            _Frm = Nothing
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents MainToolBar As System.Windows.Forms.ToolBar
    Friend WithEvents ToolBarImageList As System.Windows.Forms.ImageList
    Friend WithEvents MainStatusBar As System.Windows.Forms.StatusBar
    Friend WithEvents InfoStatusBarPanel As System.Windows.Forms.StatusBarPanel
    Friend WithEvents DomainUserStatusBarPanel As System.Windows.Forms.StatusBarPanel
    Friend WithEvents DateStatusBarPanel As System.Windows.Forms.StatusBarPanel
    Friend WithEvents StatusProgressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents DataStatusBarPanel As System.Windows.Forms.StatusBarPanel
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.MainToolBar = New System.Windows.Forms.ToolBar()
        Me.ToolBarImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.MainStatusBar = New System.Windows.Forms.StatusBar()
        Me.InfoStatusBarPanel = New System.Windows.Forms.StatusBarPanel()
        Me.DomainUserStatusBarPanel = New System.Windows.Forms.StatusBarPanel()
        Me.DataStatusBarPanel = New System.Windows.Forms.StatusBarPanel()
        Me.DateStatusBarPanel = New System.Windows.Forms.StatusBarPanel()
        Me.StatusProgressBar = New System.Windows.Forms.ProgressBar()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WindowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ArrangeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HorizontalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VerticalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CascadeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ResetWindowsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ResetTempFilesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        CType(Me.InfoStatusBarPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DomainUserStatusBarPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataStatusBarPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateStatusBarPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainToolBar
        '
        Me.MainToolBar.ButtonSize = New System.Drawing.Size(24, 24)
        Me.MainToolBar.DropDownArrows = True
        Me.MainToolBar.ImageList = Me.ToolBarImageList
        Me.MainToolBar.Location = New System.Drawing.Point(0, 24)
        Me.MainToolBar.Name = "MainToolBar"
        Me.MainToolBar.ShowToolTips = True
        Me.MainToolBar.Size = New System.Drawing.Size(1048, 30)
        Me.MainToolBar.TabIndex = 1
        '
        'ToolBarImageList
        '
        Me.ToolBarImageList.ImageStream = CType(resources.GetObject("ToolBarImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ToolBarImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.ToolBarImageList.Images.SetKeyName(0, "")
        Me.ToolBarImageList.Images.SetKeyName(1, "")
        Me.ToolBarImageList.Images.SetKeyName(2, "")
        Me.ToolBarImageList.Images.SetKeyName(3, "")
        Me.ToolBarImageList.Images.SetKeyName(4, "")
        Me.ToolBarImageList.Images.SetKeyName(5, "")
        Me.ToolBarImageList.Images.SetKeyName(6, "")
        Me.ToolBarImageList.Images.SetKeyName(7, "")
        Me.ToolBarImageList.Images.SetKeyName(8, "")
        Me.ToolBarImageList.Images.SetKeyName(9, "")
        Me.ToolBarImageList.Images.SetKeyName(10, "")
        Me.ToolBarImageList.Images.SetKeyName(11, "")
        Me.ToolBarImageList.Images.SetKeyName(12, "")
        Me.ToolBarImageList.Images.SetKeyName(13, "")
        Me.ToolBarImageList.Images.SetKeyName(14, "DentalReport.ico")
        '
        'MainStatusBar
        '
        Me.MainStatusBar.Location = New System.Drawing.Point(0, 487)
        Me.MainStatusBar.Name = "MainStatusBar"
        Me.MainStatusBar.Panels.AddRange(New System.Windows.Forms.StatusBarPanel() {Me.InfoStatusBarPanel, Me.DomainUserStatusBarPanel, Me.DataStatusBarPanel, Me.DateStatusBarPanel})
        Me.MainStatusBar.ShowPanels = True
        Me.MainStatusBar.Size = New System.Drawing.Size(1048, 22)
        Me.MainStatusBar.TabIndex = 3
        Me.MainStatusBar.Text = "StatusBar1"
        '
        'InfoStatusBarPanel
        '
        Me.InfoStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring
        Me.InfoStatusBarPanel.Name = "InfoStatusBarPanel"
        Me.InfoStatusBarPanel.Width = 831
        '
        'DomainUserStatusBarPanel
        '
        Me.DomainUserStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.DomainUserStatusBarPanel.MinWidth = 100
        Me.DomainUserStatusBarPanel.Name = "DomainUserStatusBarPanel"
        '
        'DataStatusBarPanel
        '
        Me.DataStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.DataStatusBarPanel.MinWidth = 50
        Me.DataStatusBarPanel.Name = "DataStatusBarPanel"
        Me.DataStatusBarPanel.Width = 50
        '
        'DateStatusBarPanel
        '
        Me.DateStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.DateStatusBarPanel.MinWidth = 50
        Me.DateStatusBarPanel.Name = "DateStatusBarPanel"
        Me.DateStatusBarPanel.Width = 50
        '
        'StatusProgressBar
        '
        Me.StatusProgressBar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.StatusProgressBar.Location = New System.Drawing.Point(832, 489)
        Me.StatusProgressBar.Name = "StatusProgressBar"
        Me.StatusProgressBar.Size = New System.Drawing.Size(200, 19)
        Me.StatusProgressBar.TabIndex = 5
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.WindowToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.MdiWindowListItem = Me.WindowToolStripMenuItem
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1048, 24)
        Me.MenuStrip1.TabIndex = 7
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(93, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'WindowToolStripMenuItem
        '
        Me.WindowToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ArrangeToolStripMenuItem})
        Me.WindowToolStripMenuItem.Name = "WindowToolStripMenuItem"
        Me.WindowToolStripMenuItem.Size = New System.Drawing.Size(63, 20)
        Me.WindowToolStripMenuItem.Text = "Window"
        '
        'ArrangeToolStripMenuItem
        '
        Me.ArrangeToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HorizontalToolStripMenuItem, Me.VerticalToolStripMenuItem, Me.CascadeToolStripMenuItem})
        Me.ArrangeToolStripMenuItem.Name = "ArrangeToolStripMenuItem"
        Me.ArrangeToolStripMenuItem.Size = New System.Drawing.Size(116, 22)
        Me.ArrangeToolStripMenuItem.Text = "Arrange"
        '
        'HorizontalToolStripMenuItem
        '
        Me.HorizontalToolStripMenuItem.Name = "HorizontalToolStripMenuItem"
        Me.HorizontalToolStripMenuItem.Size = New System.Drawing.Size(129, 22)
        Me.HorizontalToolStripMenuItem.Text = "Horizontal"
        '
        'VerticalToolStripMenuItem
        '
        Me.VerticalToolStripMenuItem.Name = "VerticalToolStripMenuItem"
        Me.VerticalToolStripMenuItem.Size = New System.Drawing.Size(129, 22)
        Me.VerticalToolStripMenuItem.Text = "Vertical"
        '
        'CascadeToolStripMenuItem
        '
        Me.CascadeToolStripMenuItem.Name = "CascadeToolStripMenuItem"
        Me.CascadeToolStripMenuItem.Size = New System.Drawing.Size(129, 22)
        Me.CascadeToolStripMenuItem.Text = "Cascade"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem, Me.ResetWindowsToolStripMenuItem, Me.ResetTempFilesToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'ResetWindowsToolStripMenuItem
        '
        Me.ResetWindowsToolStripMenuItem.Name = "ResetWindowsToolStripMenuItem"
        Me.ResetWindowsToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.ResetWindowsToolStripMenuItem.Text = "Reset Windows"
        '
        'ResetTempFilesToolStripMenuItem
        '
        Me.ResetTempFilesToolStripMenuItem.Name = "ResetTempFilesToolStripMenuItem"
        Me.ResetTempFilesToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.ResetTempFilesToolStripMenuItem.Text = "Reset Cache Files"
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'Main
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(1048, 509)
        Me.Controls.Add(Me.StatusProgressBar)
        Me.Controls.Add(Me.MainStatusBar)
        Me.Controls.Add(Me.MainToolBar)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "UFCW Claims System"
        CType(Me.InfoStatusBarPanel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DomainUserStatusBarPanel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataStatusBarPanel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateStatusBarPanel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region "Form Events"

    Private Sub Main_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Try

            FileNetCleanup()

            CloseAllChildren()  ' this gracefully closes all forms mdichildren or not

        Catch IgnoreExceptions As Exception
        End Try

    End Sub
    'Public Shared Function IsOnScreen(ByVal form As Form) As Boolean
    '    ' Create rectangle
    '    Dim formRectangle As New Rectangle(form.Left, form.Top, form.Width, form.Height)

    '    ' Test
    '    Return Screen.AllScreens.Any(Function(s) s.WorkingArea.IntersectsWith(formRectangle))
    'End Function

    'Public Shared Function SetFormPosition(ByRef frm As Form, Optional appKEY As String = "") As Boolean

    '    Dim FName As String
    '    Dim FSize As Single
    '    Dim FStyle As FontStyle
    '    Dim FUnit As GraphicsUnit
    '    Dim FCharset As Byte
    '    Dim InRegistry As Boolean

    '    Try
    '        If appKEY = "" Then appKEY = If(ConfigurationManager.AppSettings("AppKey"), "")

    '        Using key As RegistryKey = Registry.CurrentUser.OpenSubKey($"SOFTWARE\VB and VBA Program Settings\{appKEY}")

    '            If key IsNot Nothing Then
    '                For Each valueName As String In key.GetSubKeyNames()
    '                    InRegistry = True

    '                    Debug.Print($"Key: {valueName}, Value: {key.GetValue(valueName)}")

    '                    Exit For
    '                Next
    '            Else
    '                Debug.Print($"The specified registry path '{appKEY}' does not exist.")
    '            End If
    '        End Using

    '        If appKEY.Length > 0 AndAlso InRegistry Then

    '            frm.Top = If(CInt(GetSetting(appKEY, frm.Name & "\Settings", "Top", frm.Top.ToString)) < 0, 0, CInt(GetSetting(appKEY, frm.Name & "\Settings", "Top", frm.Top.ToString)))
    '            frm.Height = CInt(GetSetting(appKEY, frm.Name & "\Settings", "Height", frm.Height.ToString))
    '            frm.Left = If(CInt(GetSetting(appKEY, frm.Name & "\Settings", "Left", frm.Left.ToString)) < 0, 0, CInt(GetSetting(appKEY, frm.Name & "\Settings", "Left", frm.Left.ToString)))
    '            frm.Width = CInt(GetSetting(appKEY, frm.Name & "\Settings", "Width", frm.Width.ToString))
    '            frm.WindowState = CType(GetSetting(appKEY, frm.Name & "\Settings", "WindowState", CInt(frm.WindowState).ToString), FormWindowState)

    '            FStyle = New FontStyle
    '            FUnit = New GraphicsUnit

    '            FName = GetSetting(appKEY, frm.Name & "\Settings", "FontName", frm.Font.Name)
    '            FSize = CSng(GetSetting(appKEY, frm.Name & "\Settings", "FontSize", CStr(frm.Font.Size)))

    '            FStyle = CType(GetSetting(appKEY, frm.Name & "\Settings", "FontStyle", CStr(frm.Font.Style)), FontStyle)
    '            FUnit = CType(GetSetting(appKEY, frm.Name & "\Settings", "FontUnit", CStr(frm.Font.Unit)), GraphicsUnit)
    '            FCharset = CByte(GetSetting(appKEY, frm.Name & "\Settings", "FontCharset", CStr(frm.Font.GdiCharSet)))

    '            frm.Font = New Font(FName, FSize, FStyle, FUnit, FCharset)

    '        End If

    '        Return IsOnScreen(frm)

    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Function

    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim MsgFrm As New Message
        Dim Result As String
        'Dim ExPreLoadRules As ExecutePreLoadRules
        'Dim ExPreLoadProcedures As ExecutePreLoadProcedures
        'Dim RulesThread As Thread
        'Dim ProcedureThread As Thread
        'Dim ExPreLoadPlaceOfServiceValues As ExecutePreLoadPlaceOfServiceValues
        'Dim PlaceOfServiceValuesThread As Thread
        'Dim ExPreLoadDiagnosis As ExecutePreLoadDiagnosis
        'Dim DiagnosisThread As Thread

        'Init Active Directory
        AppDomain.CurrentDomain.SetPrincipalPolicy(System.Security.Principal.PrincipalPolicy.WindowsPrincipal)

        'Check the access to application
        If Not UFCWGeneralAD.CMSPay() Then
            MessageBox.Show("You do not have permission to use this application", "Check Access (CMSPay)", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
            Return
        End If

        Me.ControlBox = False

        If Not UFCWGeneral.SetFormPosition(Me, _APPKEY) Then Me.CenterToScreen()

        InfoStatusBarPanel.Text = "Loading Application.  Please Wait..."

        If Not _ComparefolderLoc = "" AndAlso _ComparefolderLoc IsNot Nothing Then
            Result = UFCWGeneral.CompareFolders(_ComparefolderLoc, Application.StartupPath, _ExcludeXMLFiles)
            If Result.Length > 0 Then
                MsgFrm.DistributionLoc = _ComparefolderLoc
                MsgFrm.txtResult.Text = Result
                If Result.Contains("MISSING") OrElse Result.Contains("DIFFERENT") Then
                    MsgFrm.MsgLabel.Text = "You do not have the current version of the software. Please restart your PC. If error returns PLEASE CALL HELP DESK"
                    MsgFrm.FormStatus = "ERROR"
                    MsgFrm.ShowDialog()
                    Me.Close()
                    Exit Sub
                Else
                    MsgFrm.MsgLabel.Text = ""
                    MsgFrm.MsgLabel.ForeColor = Color.Blue
                    MsgFrm.ShowIcon = False
                    MsgFrm.Text = "Click OK to Continue..."
                    MsgFrm.FormStatus = "WARNING"
                    MsgFrm.ShowDialog()
                End If
            End If
        End If

        Me.Show() 'Required to initialize toolbar
        Me.Refresh()

        'Moved to Queue to avoid unnecassecary plan dependancy
        'Task.Factory.StartNew(Sub()
        'PlanController.GetWildCardProcedureCollection("A0001", 0, UFCWGeneral.NowDate)
        'End Sub)

        ''Start Cache Load of the rules
        'ExPreLoadRules = New ExecutePreLoadRules
        'RulesThread = New Thread(New ThreadStart(AddressOf ExPreLoadRules.Execute))
        'RulesThread.IsBackground = True
        'RulesThread.Priority = ThreadPriority.Highest
        'RulesThread.Start()

        StatusProgressBar.Value = 20

        Task.Factory.StartNew(Sub()
                                  AccumulatorDAL.GetActiveAccumulators()
                              End Sub).ContinueWith(Sub()
                                                        PlanController.GetAllPlanAccumulators()
                                                    End Sub)

        ''Start Cache Load of the rules
        'ExPreLoadProcedures = New ExecutePreLoadProcedures
        'ProcedureThread = New Thread(New ThreadStart(AddressOf ExPreLoadProcedures.Execute))
        'ProcedureThread.IsBackground = True
        'ProcedureThread.Priority = ThreadPriority.AboveNormal
        'ProcedureThread.Start()

        StatusProgressBar.Value = 30

        Dim FNInfo As Tuple(Of String, String) = WCFWrapperCommon.InitializeFileNetUserInfo()

        Using FNSession As New Session(New WCFProcessInfo With {.ByProcessName = False, .ProcessID = Process.GetCurrentProcess.Id, .ProcessName = Process.GetCurrentProcess.ProcessName})

            FNSession.Logon(FNInfo.Item1, FNInfo.Item2)

        End Using

        StatusProgressBar.Value = 35

        Task.Factory.StartNew(Sub()
                                  CMSDALFDBMD.RetrievePlaceOfServiceValues()
                              End Sub)


        ''Start Cache Load of the rules
        'ExPreLoadPlaceOfServiceValues = New ExecutePreLoadPlaceOfServiceValues
        'PlaceOfServiceValuesThread = New Thread(New ThreadStart(AddressOf ExPreLoadPlaceOfServiceValues.Execute))
        'PlaceOfServiceValuesThread.IsBackground = True
        'PlaceOfServiceValuesThread.Start()

        StatusProgressBar.Value = 40

        Task.Factory.StartNew(Sub()
                                  CMSDALFDBMD.RetrieveProcedureValues()
                              End Sub)

        StatusProgressBar.Value = 60

        'needed for plugins
        AddHandler MainToolBar.ButtonClick, AddressOf Me.btnPlugIn_Click

        _PlugInController = New PlugInController(PLUINFILTER)
        _PlugInController.LoadPlugIns()

        StatusProgressBar.Value = 80

        If _PlugInController.Contains("Queue") Then
            'Load the Queue Screen
            OpenPlugIn("Queue")
        End If

        StatusProgressBar.Value = 90

        Task.Factory.StartNew(Sub()
                                  CMSDALFDBMD.RetrieveDiagnosisXRef()
                              End Sub)

        Task.Factory.StartNew(Sub()
                                  CMSDALFDBMD.RetrieveDiagnosisValues()
                              End Sub)

        ''Start Cache Load of the rules
        'ExPreLoadDiagnosis = New ExecutePreLoadDiagnosis
        'DiagnosisThread = New Thread(New ThreadStart(AddressOf ExPreLoadDiagnosis.Execute))
        'DiagnosisThread.IsBackground = True
        'DiagnosisThread.Priority = ThreadPriority.AboveNormal
        'DiagnosisThread.Start()

        StatusProgressBar.Value = 100

        DomainUserStatusBarPanel.Text = SystemInformation.UserName
        DataStatusBarPanel.Text = "Server=" & CMSDALCommon.GetServerName(Nothing) & ";DB=" & CMSDALCommon.GetDatabaseName(Nothing)

        Timer1.Enabled = True

        InfoStatusBarPanel.Text = ""

        Me.ControlBox = True
        StatusProgressBar.Visible = False

        If MsgFrm IsNot Nothing Then MsgFrm.Dispose()
        MsgFrm = Nothing

    End Sub

    Private Sub Main_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try

            UFCWGeneral.SaveFormPosition(Me, _APPKEY)

            CMSDALFDBMD.SaveClaimsHistoryXML()

        Catch IgnoreExceptions As Exception
        End Try

    End Sub

    Private Sub mfrm_FormClosing(ByVal sender As Object, ByVal e As EventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' this is called when a plugin closes.  to prevent orphaned busy items it takes
        ' all children of its plugins and adds refrence to this forms children.
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim ClosingForm As Form = CType(sender, Form)
        Dim ClosingUIDPI As PropertyInfo
        Dim ClosingChildUIDPI As PropertyInfo
        Dim ChildrenPI As PropertyInfo
        Dim ClosingUID As String = ""
        Dim ChildUID As String = ""
        Dim SubChildren As Hashtable
        Dim SubChildForm As Form

        Try
            If _OpenChildrenCounts.Count > 0 Then
                If _OpenChildrenCounts.Contains(ClosingForm.Name) Then
                    _OpenChildrenCounts.Item(ClosingForm.Name) = CInt(_OpenChildrenCounts.Item(ClosingForm.Name)) - 1
                End If
            End If

            ClosingUIDPI = ClosingForm.GetType.GetProperty("UniqueID")
            If ClosingUIDPI IsNot Nothing Then
                ClosingUID = ClosingForm.GetType.InvokeMember("UniqueID", BindingFlags.GetProperty, Nothing, ClosingForm, Array.Empty(Of Object)()).ToString

                If _Children.ContainsKey(ClosingUID) Then
                    ChildrenPI = ClosingForm.GetType.GetProperty("Children")
                    If ChildrenPI IsNot Nothing Then
                        SubChildren = CType(ClosingForm.GetType.InvokeMember("Children", BindingFlags.GetProperty, Nothing, ClosingForm, Array.Empty(Of Object)()), Hashtable)

                        For Each SubChildForm In SubChildren.Values
                            ClosingChildUIDPI = SubChildForm.GetType.GetProperty("UniqueID")

                            If ClosingChildUIDPI IsNot Nothing Then
                                ChildUID = SubChildForm.GetType.InvokeMember("UniqueID", BindingFlags.GetProperty, Nothing, SubChildForm, Array.Empty(Of Object)()).ToString
                                _Children.Add(ChildUID, SubChildForm)

                                'AddHandler SubChildForm.FormClosing, AddressOf mfrm_FormClosing
                            End If
                        Next

                        _OpenChildrenCount += SubChildren.Count
                    End If

                    _Children.Remove(ClosingUID)
                End If
            End If

            Me.Refresh()

            'If Not SubChildForm Is Nothing Then RemoveHandler SubChildForm.FormClosing, AddressOf mfrm_FormClosing

            System.GC.Collect()

            _OpenChildrenCount -= 1

        Catch ex As Exception

            Throw
        Finally
        End Try

    End Sub

    Private Sub Main_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Me.BringToFront()
    End Sub

#End Region

#Region "Menu\Button Events"

    Private Sub DeleteClaimsRegistry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            My.Computer.Registry.CurrentUser.DeleteSubKeyTree("Software\VB and VBA Program Settings\UFCW\Claims")
        Catch ex As Exception
        Finally
            My.Computer.Registry.CurrentUser.Flush()
        End Try
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub HorizontalToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HorizontalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub

    Private Sub VerticalToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VerticalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub

    Private Sub CascadeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CascadeToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        Dim AboutF As New About
        AboutF.ShowDialog(Me)
        AboutF.Dispose()
        AboutF = Nothing
    End Sub

    Private Sub ResetWindowsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResetWindowsToolStripMenuItem.Click
        Try
            My.Computer.Registry.CurrentUser.DeleteSubKeyTree("Software\VB and VBA Program Settings\UFCW\Claims")
        Catch ex As Exception
        Finally
            My.Computer.Registry.CurrentUser.Flush()
        End Try

    End Sub

    Private Sub ExitMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Close the application a terminate processing
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	11/16/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Me.Close()
    End Sub

    Private Sub AboutMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Displays the Help About Form to show Version and Other info.
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	2/15/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim AboutF As New About
        AboutF.ShowDialog(Me)
        AboutF.Dispose()
        AboutF = Nothing
    End Sub

    Private Sub HorizontalMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Layers the children forms Accross
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	11/16/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub

    Private Sub VerticalMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Layers the children forms Top to Bottom
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	11/16/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub

    Private Sub CascadeMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Layers the children forms one in front of the other with only the title bar showing
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	11/16/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub
#End Region

#Region "Plug In Code"
    Private Shared Sub PlugInLoading(ByVal plugInControl As SharedInterfaces.PlugInAttribute, ByRef Cancel As Boolean) Handles _PlugInController.PlugInLoading
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

        If plugInControl.Destination.ToLower = "main" Then

            If plugInControl.MenuText = "Accumulator Override" Then

                If Not UFCWGeneralAD.CMSCanOverrideAccumulators Then
                    Cancel = True
                End If

            ElseIf plugInControl.MenuText.LastIndexOf("Reports") > -1 Then

                If Not UFCWGeneralAD.CMSCanRunReports Then
                    Cancel = True
                End If

            ElseIf plugInControl.MenuText.LastIndexOf("HRA Override") > -1 Then

                If Not UFCWGeneralAD.CMSCanReprocess Then
                    Cancel = True
                End If
            ElseIf plugInControl.MenuText.LastIndexOf("ALERTS Override") > -1 Then

                If Not UFCWGeneralAD.CMSCanModifyAlerts Then
                    Cancel = True
                End If
            End If
        Else
            Cancel = True
        End If
    End Sub

    Private Sub PlugInController_AddingPlugIn(ByVal plugInControl As SharedInterfaces.PlugInAttribute) Handles _PlugInController.AddingPlugIn
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Addes the Plugin to the form
        ' </summary>
        ' <param name="PlugIn"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	12/12/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim TBButtonIndex As Integer

        Try
            TBButtonIndex = MainToolBar.Buttons.Add(plugInControl.MenuText)

            MainToolBar.Buttons(TBButtonIndex).Tag = plugInControl.MenuText
            MainToolBar.Buttons(TBButtonIndex).ImageIndex = plugInControl.ImageIndex

        Catch ex As Exception

            Throw
        End Try
    End Sub

    Private Sub MainStatusMessage(ByVal statusMessage As String) Implements SharedInterfaces.IMessage.StatusMessage
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

        If MainStatusBar.InvokeRequired Then
#If TRACE Then
            If _TraceMessaging.Enabled Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " I: Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & " : " & statusMessage & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceMessaging" & vbTab)
#End If
            MainStatusBar.Invoke(StatusMessageHandler, New Object() {statusMessage})
        Else
#If TRACE Then
            If _TraceMessaging.Enabled Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & " : " & statusMessage & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceMessaging" & vbTab)
#End If
            UpdateInfoStatusBarPanel_Text(statusMessage)
        End If

    End Sub

    Private Sub UpdateInfoStatusBarPanel_Text(statusMessage As String)

        InfoStatusBarPanel.Text = statusMessage
        MainStatusBar.Update()

#If TRACE Then
        If _TraceMessaging.Enabled Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & " : " & InfoStatusBarPanel.Text & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceMessaging" & vbTab)
#End If

    End Sub
    Private Sub mnuPlugIn_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' procedure to call a given plugin
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim MenuItem As MenuItem = CType(sender, MenuItem)
        Dim MenuText As String = MenuItem.Text

        If MenuText <> "" Then
            OpenPlugIn(MenuText)
        End If
    End Sub

    Private Sub btnPlugIn_Click(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' procedure to call a given plugin
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim ButtonTag As String = e.Button.Tag.ToString

        If ButtonTag <> "" Then
            OpenPlugIn(ButtonTag)
        End If
    End Sub

    Private Function OpenPlugIn(ByVal plugInControlName As String) As Boolean
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
        Dim Indx As Integer = 0
        Dim UID As String
        Dim OpenChildCnt As Integer = 0
        Dim P As PropertyInfo

        Try

            If _OpenChildrenCounts.Contains(plugInControlName.Replace(" ", "")) Then Indx = CInt(_OpenChildrenCounts.Item(plugInControlName.Replace(" ", "")))

            If _OpenChildrenCounts.Contains(plugInControlName.Replace(" ", "")) Then
                OpenChildCnt = CInt(_OpenChildrenCounts.Item(plugInControlName.Replace(" ", "")))
            End If

            If OpenChildCnt < MAXFORMS Then

                _Frm = _PlugInController.LaunchPlugIn(plugInControlName, CType(Me, SharedInterfaces.IMessage), Indx)

                If _Frm IsNot Nothing Then

                    _Frm.MdiParent = Me
                    _Frm.Hide()

                    If _OpenChildrenCounts.Contains(plugInControlName.Replace(" ", "")) Then
                        _OpenChildrenCounts.Item(plugInControlName.Replace(" ", "")) = CInt(_OpenChildrenCounts.Item(plugInControlName.Replace(" ", ""))) + 1
                    Else
                        _OpenChildrenCounts.Add(plugInControlName.Replace(" ", ""), 1)
                    End If

                    _OpenChildrenCount += 1
                    _TotalOpened += 1

                    P = _Frm.GetType.GetProperty("UniqueID")
                    If P IsNot Nothing Then
                        UID = "M" & _TotalOpened
                        _Frm.GetType.InvokeMember("UniqueID", BindingFlags.SetProperty, Nothing, _Frm, New Object() {UID})
                        _Children.Add(UID, _Frm)
                    End If

                    _Frm.Show()
                End If
            Else
                MessageBox.Show("There are too many " & plugInControlName & " Screens Open." & Chr(13) & "Close some " & plugInControlName & " Screens and try again.",
                                "Too Many Screens Open", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End If

            Me.Refresh()

            Return True

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Function

#End Region

#Region "Custom Subs\Functions"

    Private Sub UpdateCursor(cursor As Cursor)

        Me.Cursor = cursor

    End Sub

    Private Function CloseAllChildren() As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gracefully close all child forms to prevent busy item locks and other problems
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/23/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim ChildForm As Form
        Dim Enumerator As IDictionaryEnumerator

        Try
            Enumerator = _Children.GetEnumerator()
            While Enumerator.MoveNext()
                ChildForm = CType(Enumerator.Value, Form)
                'attempt close
                ChildForm.Close()
                'check if close occurred
                If ChildForm.GetType.GetProperty("ShowCancelOnClose") IsNot Nothing Then
                    ChildForm.GetType.InvokeMember("ShowCancelOnClose", BindingFlags.SetProperty, Nothing, ChildForm, New Object() {False})
                End If
                ChildForm.Dispose()
                ChildForm = Nothing
            End While

            _Children.Clear()

            Return True

        Catch ex As Exception

            Throw
        End Try
    End Function

    Private Shared Sub FileNetCleanup()
        Try

            Using fnSession As New Session(New WCFProcessInfo With {.ByProcessName = _SingleInstance, .ProcessID = Process.GetCurrentProcess.Id, .ProcessName = Process.GetCurrentProcess.ProcessName})
                fnSession.Logoff()
            End Using

        Catch IgnoreExceptions As Exception

        End Try
    End Sub

#End Region

    <STAThread()> Public Shared Sub Main()

        CMSDALCommon.InitializeEL()

        Dim CurrentDomain As AppDomain = AppDomain.CurrentDomain

        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException, True)

        AddHandler Application.ThreadException, AddressOf UFCWThreadExceptionHandler.Application_ThreadException
        AddHandler CurrentDomain.UnhandledException, AddressOf UFCWThreadExceptionHandler.CurrentDomain_UnhandledException

        Dim ExecutablePath As String
        Dim dataDirectory As String

        If ApplicationDeployment.IsNetworkDeployed Then ' This means the application is running as a ClickOnce application.
            ' ApplicationDeployment.CurrentDeployment.UpdateLocation.AbsolutePath = where the clcikonce was installed from

            ExecutablePath = AppDomain.CurrentDomain.BaseDirectory
            dataDirectory = ApplicationDeployment.CurrentDeployment.DataDirectory

            ' Use the dataDirectory as needed
            SaveSetting("UFCW\UFCW.CMS.REPORTS\", "ClickOnce", "FileInfo", ExecutablePath)
            SaveSetting("UFCW\UFCW.CMS.REPORTS\", "ClickOnce", "FileInfoData", dataDirectory)

            'Dim fileInfo As New FileInfo(ExecutablePath)

            '' Get the last modified date
            'SaveSetting("UFCW\UFCW.CMS.REPORTS\", "ClickOnce", "FileInfoDate", fileInfo.LastWriteTime.ToLongTimeString)

        Else
            ExecutablePath = System.Reflection.Assembly.GetExecutingAssembly().Location
            Dim ExecutableDirectory As String = System.IO.Path.GetDirectoryName(ExecutablePath)
            'Dim fileInfo As New FileInfo(ExecutablePath)

            If System.Diagnostics.Debugger.IsAttached Then
                SaveSetting("UFCW\UFCW.CMS.REPORTS\", "Debug", "FileInfo", ExecutablePath)
                ' Get the last modified date
                'SaveSetting("UFCW\UFCW.CMS.REPORTS\", "Debug", "FileInfoDate", fileInfo.LastWriteTime.ToLongTimeString)
            Else
                SaveSetting("UFCW\UFCW.CMS.REPORTS\", "Prod", "FileInfo", ExecutablePath)
                ' Get the last modified date
                'SaveSetting("UFCW\UFCW.CMS.REPORTS\", "Debug", "FileInfoDate", fileInfo.LastWriteTime.ToLongTimeString)
            End If


        End If

#If DEBUG Then 'allow more than one instance when testing
#Else
        _MainMutex = New Mutex(False, "aUniqueName" & UFCWGeneral.Mode)
        If (Not _MainMutex.WaitOne(0, False)) AndAlso Not My.Computer.Keyboard.ShiftKeyDown Then
            Application.Exit()
            Return
        End If

        _SingleInstance = True

#End If

        Application.Run(New Main)

    End Sub
    Private Sub ResetTempFilesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ResetTempFilesToolStripMenuItem.Click
        Dim di As DirectoryInfo
        Try
            di = New DirectoryInfo(System.Windows.Forms.Application.StartupPath)
            For Each FI As FileInfo In di.GetFiles("*.xml").Where(Function(item) item.ToString.ToUpper.StartsWith(CMSDALCommon.GetDatabaseName()))
                File.SetAttributes(FI.Name, FileAttributes.Normal)
                FI.Delete()
            Next
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        DateStatusBarPanel.Text = Format(UFCWGeneral.NowDate, "MM-dd-yyyy HH:mm:ss")

    End Sub
End Class

#Region "BackThread Class"
'Public Class ExecutePreLoadRules

'    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

'    Sub New()

'    End Sub

'    Public Shared Sub Execute()
'        Try

'            Using WC As New GlobalCursor
'                PlanController.GetWildCardProcedureCollection("A0001", 0, UFCWGeneral.NowDate)
'            End Using

'        Catch ex As Exception
'            If System.Threading.Thread.CurrentThread.ThreadState <> ThreadState.AbortRequested Then
'                Throw
'            End If
'        End Try
'    End Sub
'End Class

Public Class ExecutePreLoadPlaceOfServiceValues
    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Sub New()

    End Sub

    Public Shared Sub Execute()
        Try
            CMSDALFDBMD.RetrievePlaceOfServiceValues()

        Catch ex As Exception
            If System.Threading.Thread.CurrentThread.ThreadState <> ThreadState.AbortRequested Then
                Throw
            End If
        End Try
    End Sub
End Class

Public Class ExecutePreLoadProcedures
    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
    Sub New()

    End Sub

    Public Shared Sub Execute()
        Try
            CMSDALFDBMD.RetrieveProcedureValues()

        Catch ex As Exception
            If System.Threading.Thread.CurrentThread.ThreadState <> ThreadState.AbortRequested Then
                Throw
            End If
        End Try
    End Sub
End Class

'Public Class ExecutePreLoadDiagnosis
'    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
'    Sub New()

'    End Sub

'    Public Sub Execute()
'        Try
'            CMSDALFDBMD.RetrieveDiagnosisValues()

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

#End Region

'Module MainFormExtensions

'    Sub New()
'    End Sub

'    Private Delegate Sub SetTextDelegate(control As Control, text As String)
'    Private Delegate Sub SetTagDelegate(control As Control, tagValue As Object)

'    <System.Runtime.CompilerServices.Extension> _
'    Public Sub SetCursor(control As Cursor)

'    End Sub

'    <System.Runtime.CompilerServices.Extension> _
'    Public Function SetCursor2(control As Cursor) As Cursor

'    End Function


'    <System.Runtime.CompilerServices.Extension> _
'    Public Sub SetText(control As Control, text As String)
'        If control.InvokeRequired Then
'            control.Invoke(New SetTextDelegate(AddressOf SetText), New Object() {control, text})
'        Else
'            control.Text = text
'        End If
'    End Sub

'    <System.Runtime.CompilerServices.Extension> _
'    Public Sub SetTag(control As Control, tag As Object)
'        If control.InvokeRequired Then
'            control.Invoke(New SetTagDelegate(AddressOf SetTag), New Object() {control, tag})
'        Else
'            control.Tag = tag
'        End If
'    End Sub

'End Module

