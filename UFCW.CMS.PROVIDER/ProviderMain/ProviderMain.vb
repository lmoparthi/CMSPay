Imports System.Reflection
Imports System.Security.Principal

Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Runtime.InteropServices

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

Public Class ProviderMain
    Inherits System.Windows.Forms.Form

    Implements SharedInterfaces.IMessage

    Const APPKEY As String = "UFCW\Provider\"
    Const CRYPTORKEY As String = "UFCW Provider ACCESS"
    Const MAXFORMS As Integer = 25
    Const MAXGUIHANDLES As Long = 8975933078237088100
    Const PLUINFILTER As String = "UFCW.CMS.*.dll"

    Private WithEvents mplug As PlugInController
    Private mfrm As Form
    Private OpenChildrenCount As Integer = 0
    Private TotalOpened As Integer = 0
    Private OpenChildrenCounts As New Hashtable
    Private bizUserPrincipal As WindowsPrincipal

    Private _Children As New Hashtable

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
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents ExitMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents AboutMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem4 As System.Windows.Forms.MenuItem
    Friend WithEvents HorizontalMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents VerticalMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents CascadeMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents MainStatusBar As System.Windows.Forms.StatusBar
    Friend WithEvents InfoStatusBarPanel As System.Windows.Forms.StatusBarPanel
    Friend WithEvents DomainUserStatusBarPanel As System.Windows.Forms.StatusBarPanel
    Friend WithEvents DateStatusBarPanel As System.Windows.Forms.StatusBarPanel
    Friend WithEvents WindowMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents StatusProgressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents DataStatusBarPanel As System.Windows.Forms.StatusBarPanel
    Friend WithEvents ProviderMainMenu As System.Windows.Forms.MainMenu
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ProviderMain))
        Me.MainToolBar = New System.Windows.Forms.ToolBar
        Me.ToolBarImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.ProviderMainMenu = New System.Windows.Forms.MainMenu
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.ExitMenuItem = New System.Windows.Forms.MenuItem
        Me.WindowMenuItem = New System.Windows.Forms.MenuItem
        Me.MenuItem4 = New System.Windows.Forms.MenuItem
        Me.HorizontalMenuItem = New System.Windows.Forms.MenuItem
        Me.VerticalMenuItem = New System.Windows.Forms.MenuItem
        Me.CascadeMenuItem = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.AboutMenuItem = New System.Windows.Forms.MenuItem
        Me.MainStatusBar = New System.Windows.Forms.StatusBar
        Me.InfoStatusBarPanel = New System.Windows.Forms.StatusBarPanel
        Me.DomainUserStatusBarPanel = New System.Windows.Forms.StatusBarPanel
        Me.DataStatusBarPanel = New System.Windows.Forms.StatusBarPanel
        Me.DateStatusBarPanel = New System.Windows.Forms.StatusBarPanel
        Me.StatusProgressBar = New System.Windows.Forms.ProgressBar
        CType(Me.InfoStatusBarPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DomainUserStatusBarPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataStatusBarPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateStatusBarPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MainToolBar
        '
        Me.MainToolBar.ButtonSize = New System.Drawing.Size(24, 24)
        Me.MainToolBar.DropDownArrows = True
        Me.MainToolBar.ImageList = Me.ToolBarImageList
        Me.MainToolBar.Location = New System.Drawing.Point(0, 0)
        Me.MainToolBar.Name = "MainToolBar"
        Me.MainToolBar.ShowToolTips = True
        Me.MainToolBar.Size = New System.Drawing.Size(1048, 30)
        Me.MainToolBar.TabIndex = 1
        '
        'ToolBarImageList
        '
        Me.ToolBarImageList.ImageSize = New System.Drawing.Size(16, 16)
        Me.ToolBarImageList.ImageStream = CType(resources.GetObject("ToolBarImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ToolBarImageList.TransparentColor = System.Drawing.Color.Transparent
        '
        'ProviderMainMenu
        '
        Me.ProviderMainMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem1, Me.WindowMenuItem, Me.MenuItem2})
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 0
        Me.MenuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.ExitMenuItem})
        Me.MenuItem1.Text = "&File"
        '
        'ExitMenuItem
        '
        Me.ExitMenuItem.Index = 0
        Me.ExitMenuItem.Text = "&Exit"
        '
        'WindowMenuItem
        '
        Me.WindowMenuItem.Index = 1
        Me.WindowMenuItem.MdiList = True
        Me.WindowMenuItem.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem4})
        Me.WindowMenuItem.Text = "&Window"
        '
        'MenuItem4
        '
        Me.MenuItem4.Index = 0
        Me.MenuItem4.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.HorizontalMenuItem, Me.VerticalMenuItem, Me.CascadeMenuItem})
        Me.MenuItem4.Text = "&Arrange"
        '
        'HorizontalMenuItem
        '
        Me.HorizontalMenuItem.Index = 0
        Me.HorizontalMenuItem.Text = "&Horizontal"
        '
        'VerticalMenuItem
        '
        Me.VerticalMenuItem.Index = 1
        Me.VerticalMenuItem.Text = "&Vertical"
        '
        'CascadeMenuItem
        '
        Me.CascadeMenuItem.Index = 2
        Me.CascadeMenuItem.Text = "&Cascade"
        '
        'MenuItem2
        '
        Me.MenuItem2.Index = 2
        Me.MenuItem2.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.AboutMenuItem})
        Me.MenuItem2.Text = "&Help"
        '
        'AboutMenuItem
        '
        Me.AboutMenuItem.Index = 0
        Me.AboutMenuItem.Text = "&About"
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
        Me.InfoStatusBarPanel.Width = 832
        '
        'DomainUserStatusBarPanel
        '
        Me.DomainUserStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.DomainUserStatusBarPanel.MinWidth = 100
        '
        'DataStatusBarPanel
        '
        Me.DataStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.DataStatusBarPanel.MinWidth = 50
        Me.DataStatusBarPanel.Width = 50
        '
        'DateStatusBarPanel
        '
        Me.DateStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.DateStatusBarPanel.MinWidth = 50
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
        'ProviderMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(1048, 509)
        Me.Controls.Add(Me.StatusProgressBar)
        Me.Controls.Add(Me.MainStatusBar)
        Me.Controls.Add(Me.MainToolBar)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.Menu = Me.ProviderMainMenu
        Me.Name = "ProviderMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "UFCW Provider System"
        CType(Me.InfoStatusBarPanel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DomainUserStatusBarPanel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataStatusBarPanel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateStatusBarPanel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Form Events"
    <STAThread()> Public Shared Sub Main()
        CMSDALCommon.InitializeEL()

        AddHandler Application.ThreadException, AddressOf UFCWThreadExceptionHandler.Application_ThreadException
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf UFCWThreadExceptionHandler.CurrentDomain_UnhandledException

        Using SplashFrm As New SplashForm
            SplashFrm.Show()
        End Using

        Try

            Application.Run(New ProviderMain)

        Catch ex As Exception

            DoCrashProcedure(ex)
            MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Shared Sub DoCrashProcedure(ByVal ex As Exception)

        Logger.Write(ex.Message & Environment.NewLine & ex.StackTrace)

        Logger.Write(New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString)

        Application.Exit()

    End Sub

    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            SetSettings()

            InfoStatusBarPanel.Text = "Loading Application.  Please Wait..."

            Me.Show() 'Required to initialize toolbar
            Me.Refresh()

            StatusProgressBar.Value = 20

            'Init Active Directory
            AppDomain.CurrentDomain.SetPrincipalPolicy(Security.Principal.PrincipalPolicy.WindowsPrincipal)
            bizUserPrincipal = CType(System.Threading.Thread.CurrentPrincipal, WindowsPrincipal)

            StatusProgressBar.Value = 40

            '            Dim configData As GridStyleData = DirectCast(ConfigurationManager.GetConfiguration("DataGridStyles"), GridStyleData)

            StatusProgressBar.Value = 60

            'needed for plugins
            AddHandler MainToolBar.ButtonClick, AddressOf Me.btnPlugIn_Click

            mplug = New PlugInController(PLUINFILTER)
            mplug.LoadPlugIns()

            'Me.Show() 'Required to initialize toolbar

            StatusProgressBar.Value = 80

            Me.Refresh()

            If mplug.Contains("Search") = True Then
                'Load the Search Screen
                OpenPlugIn("Search")
            End If

            StatusProgressBar.Value = 100

            DomainUserStatusBarPanel.Text = SystemInformation.UserName
            DataStatusBarPanel.Text = "Server=" & CMSDALCommon.GetServerName(Nothing) & ";DB=" & CMSDALCommon.GetDatabaseName(Nothing)
            DateStatusBarPanel.Text = Format(Now, "MM-dd-yyyy")

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Finally
            StatusProgressBar.Visible = False
        End Try
    End Sub

    Private Sub Main_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        CloseAllChildren()  ' this gracefully closes all forms mdichildren or not

        SaveSettings()

    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' this is called when a plugin closes.  to prevent orphaned busy items it takes
    ''' all children of its plugins and adds refrence to this forms children.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	3/24/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub mfrm_Closed(ByVal sender As Object, ByVal e As EventArgs)
        Dim ClosingForm As Form = CType(sender, Form)
        Dim ClosingUIDPI As PropertyInfo
        Dim ClosingChildUIDPI As PropertyInfo
        Dim ChildrenPI As PropertyInfo
        Dim ClosingUID As String = ""
        Dim ChildUID As String = ""
        Dim SubChildren As Hashtable
        Dim SubChildForm As Form

        Try
            If OpenChildrenCounts.Count > 0 Then
                If OpenChildrenCounts.Contains(ClosingForm.Name) = True Then
                    OpenChildrenCounts.Item(ClosingForm.Name) = CInt(OpenChildrenCounts.Item(ClosingForm.Name)) - 1
                End If
            End If

            ClosingUIDPI = ClosingForm.GetType.GetProperty("UniqueID")
            If ClosingUIDPI IsNot Nothing Then
                ClosingUID = CStr(ClosingForm.GetType.InvokeMember("UniqueID", BindingFlags.GetProperty, Nothing, ClosingForm, New Object() {}))

                If _Children.ContainsKey(ClosingUID) = True Then
                    ChildrenPI = ClosingForm.GetType.GetProperty("Children")
                    If ChildrenPI IsNot Nothing Then
                        SubChildren = CType(ClosingForm.GetType.InvokeMember("Children", BindingFlags.GetProperty, Nothing, ClosingForm, New Object() {}), Hashtable)

                        For Each SubChildForm In SubChildren.Values
                            ClosingChildUIDPI = SubChildForm.GetType.GetProperty("UniqueID")

                            If ClosingChildUIDPI IsNot Nothing Then
                                ChildUID = CStr(SubChildForm.GetType.InvokeMember("UniqueID", BindingFlags.GetProperty, Nothing, SubChildForm, New Object() {}))
                                _Children.Add(ChildUID, SubChildForm)

                                AddHandler SubChildForm.Closed, AddressOf mfrm_Closed
                            End If
                        Next

                        OpenChildrenCount += SubChildren.Count
                    End If

                    _Children.Remove(ClosingUID)
                End If
            End If

            sender = Nothing

            OpenChildrenCount -= 1
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Finally

        End Try
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Gracefully close all child forms to prevent busy item locks and other problems
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	3/23/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub CloseAllChildren()
        Dim f As Form
        Dim GE As Collections.IDictionaryEnumerator

        Try
            Do Until _Children.Values.Count = 0
                GE = _Children.GetEnumerator
                GE.Reset()
                GE.MoveNext()

                f = CType(GE.Entry.Value, Form)

                f.Close()
                f.Dispose()
                f = Nothing

                GE = Nothing
            Loop

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
        Me.Visible = False

        Me.Top = CInt(GetSetting(APPKEY, "MainForm\Settings", "Top", CStr(Me.Top)))
        Me.Height = CInt(GetSetting(APPKEY, "MainForm\Settings", "Height", CStr(Me.Height)))
        Me.Left = CInt(GetSetting(APPKEY, "MainForm\Settings", "Left", CStr(Me.Left)))
        Me.Width = CInt(GetSetting(APPKEY, "MainForm\Settings", "Width", CStr(Me.Width)))
        Me.WindowState = CType(GetSetting(APPKEY, "MainForm\Settings", "WindowState", CStr(Me.WindowState)), FormWindowState)
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

        SaveSetting(APPKEY, "MainForm\Settings", "WindowState", CStr(lWindowState))
        Me.WindowState = 0
        SaveSetting(APPKEY, "MainForm\Settings", "Top", CStr(Me.Top))
        SaveSetting(APPKEY, "MainForm\Settings", "Height", CStr(Me.Height))
        SaveSetting(APPKEY, "MainForm\Settings", "Left", CStr(Me.Left))
        SaveSetting(APPKEY, "MainForm\Settings", "Width", CStr(Me.Width))

        'If LastClass <> "" Then SaveSetting(APPKEY, "Inbox\Settings", "LastWorkType", LastClass)
        'SaveSetting(APPKEY, "RouteForm\Settings", "LastRoutee", "")
    End Sub
#End Region

#Region "Menu\Button Events"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Handles the clicking of the toolbar buttons and performs the correct operation
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	11/16/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub MainToolBar_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles MainToolBar.ButtonClick
        'If e.Button Is CustomerServiceToolBarButton Then
        'If InBoxForm.DialogResult = DialogResult.Abort Then
        '    InBoxForm = New InBox(SQLCONN, FNSession, FNDisplay, FNPrintFax, MainImages, ItemImages)

        '    AddHandler InBoxForm.Closing, AddressOf InboxClosing

        '    DisplayMenu.Enabled = True
        '    PrintMenu.Enabled = True
        '    TBar.Buttons(4).Visible = True
        '    TBar.Buttons(5).Visible = True

        '    InBoxForm.MdiParent = Me
        '    InBoxForm.WindowState = FormWindowState.Maximized

        '    InBoxForm.Show()
        'End If
        'InBoxForm.Select()
        'End If
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Close the application a terminate processing
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	11/16/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub ExitMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitMenuItem.Click
        Me.Close()
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Displays the Help About Form to show Version and Other info.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	2/15/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub AboutMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutMenuItem.Click
        Dim AboutF As New AboutMain
        AboutF.ShowDialog(Me)
        AboutF.Dispose()
        AboutF = Nothing
    End Sub
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Layers the children forms Accross
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	11/16/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub HorizontalMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HorizontalMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Layers the children forms Top to Bottom
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	11/16/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub VerticalMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VerticalMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Layers the children forms one in front of the other with only the title bar showing
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	11/16/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub CascadeMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CascadeMenuItem.Click
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub
#End Region

#Region "Plug In Code"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' this is an event called from plugin controller to allow any controls to be added
    ''' that will trigger plugins
    ''' </summary>
    ''' <param name="PlugIn"></param>
    ''' <param name="Cancel"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	3/24/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub PlugInLoading(ByVal PlugIn As SharedInterfaces.PlugInAttribute, ByRef Cancel As Boolean) Handles mplug.PlugInLoading

        If PlugIn.Destination.ToLower = "provider" Then
        Else
            Cancel = True
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
        InfoStatusBarPanel.Text = msg
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' procedure to call a given plugin
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	3/24/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Friend Sub mnuPlugIn_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim mnu As MenuItem = CType(sender, MenuItem)
        Dim strText As String = mnu.Text

        If strText <> "" Then
            OpenPlugIn(strText)
        End If
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' procedure to call a given plugin
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[Nick Snyder]	3/24/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Friend Sub btnPlugIn_Click(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs)
        Dim strText As String = CStr(e.Button.Tag)

        If strText <> "" Then
            OpenPlugIn(strText)
        End If
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
    Private Function OpenPlugIn(ByVal PlugInName As String) As Boolean
        Try
            Dim Indx As Integer = 0
            Dim UID As String

            If OpenChildrenCounts.Contains(PlugInName) Then Indx = CInt(OpenChildrenCounts.Item(PlugInName))

            'Dim hndls As Integer = GetHandleCount()

            'hndls = 1

            'If OpenChildrenCount < MAXFORMS Then
            'If hndls + 150 < 18500 Then
            If GetGuiResourcesGDICount() + 51 <= MAXGUIHANDLES Then

                mfrm = mplug.LaunchPlugIn(PlugInName, CType(Me, SharedInterfaces.IMessage), Indx)

                If Not mfrm Is Nothing Then
                    AddHandler mfrm.Closed, AddressOf mfrm_Closed
                    mfrm.MdiParent = Me

                    If OpenChildrenCounts.Contains(PlugInName) = True Then
                        OpenChildrenCounts.Item(PlugInName) = CInt(OpenChildrenCounts.Item(PlugInName)) + 1
                    Else
                        OpenChildrenCounts.Add(PlugInName, 1)
                    End If

                    OpenChildrenCount += 1
                    TotalOpened += 1

                    Dim P As PropertyInfo
                    P = mfrm.GetType.GetProperty("UniqueID")
                    If Not P Is Nothing Then
                        UID = "M" & TotalOpened
                        mfrm.GetType.InvokeMember("UniqueID", BindingFlags.SetProperty, Nothing, mfrm, New Object() {UID})
                        _Children.Add(UID, mfrm)
                    End If
                    'mfrm.Tag = "M" & TotalOpened
                    '_Children.Add(mfrm.Tag.ToString, mfrm)

                    mfrm.Show()
                End If
            Else
                MessageBox.Show("There are too many " & PlugInName & " Screens Open." & Chr(13) & "Close some " & PlugInName & " Screens and try again.", _
                                "Too Many Screens Open", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End If
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Function

    Public Shared Function GetGuiResourcesGDICount() As Long
        Return NativeMethods.GetGuiResources(Process.GetCurrentProcess.Handle, 0)
    End Function

    Public Shared Function GetGuiResourcesUserCount() As Long
        Return NativeMethods.GetGuiResources(Process.GetCurrentProcess.Handle, 1)
    End Function

#End Region

#Region "Custom Subs\Functions"
    Private Function GetSystemTime() As DateTime
        Return Now
    End Function

#End Region

    Private Sub Main_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        Me.BringToFront()
    End Sub
End Class
Friend Enum ProcessAccessFlags As UInteger
    All = &H1F0FFF
    Terminate = &H1
    CreateThread = &H2
    VMOperation = &H8
    VMRead = &H10
    VMWrite = &H20
    DupHandle = &H40
    SetInformation = &H200
    QueryInformation = &H400
    Synchronize = &H100000
End Enum

Friend NotInheritable Class NativeMethods
    Private Sub New()
    End Sub

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Friend Shared Function OpenProcess(ByVal dwDesiredAccess As ProcessAccessFlags, <MarshalAs(UnmanagedType.Bool)> ByVal bInheritHandle As Boolean, ByVal dwProcessId As Integer) As IntPtr
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Friend Shared Function CloseHandle(ByVal hObject As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Friend Shared Function GetExitCodeProcess(ByVal hProcess As IntPtr, ByRef lpExitCode As UInteger) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("user32.dll", SetLastError:=True)> _
    Friend Shared Function GetGuiResources(hProcess As IntPtr, uiFlags As UInteger) As UInteger
    End Function

End Class
