Imports UFCW.WCF
Imports UFCW.WCF.FileNet

Imports System.Security.Principal
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports System.Configuration
Imports System.Deployment.Application
Imports System.Threading.Tasks

Public Class CustomerServiceMain
    Inherits System.Windows.Forms.Form

    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents CustomerServiceControl As CustomerServiceControl
    Friend WithEvents MenuItem5 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemDiagnosis As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemProcedures As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemModifiers As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemBillTypes As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemPlaceOfService As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemNDC As System.Windows.Forms.MenuItem
    Friend WithEvents ResetClaimsRegistryKeysMenu As System.Windows.Forms.MenuItem

    Private Shared ReadOnly _TraceCloning As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private ReadOnly _APPKEY As String = UFCWGeneral.CustomerServiceAPPKEY

    Private _FrmLoading As Boolean
    Private ReadOnly _ComparefolderLoc As String = System.Configuration.ConfigurationManager.AppSettings("ComparefolderLoc")
    Private ReadOnly _ExcludeXMLFiles As String = ConfigurationManager.AppSettings("ExcludeXMLFiles")
    Private ReadOnly _WindowsUserID As WindowsIdentity = UFCWGeneral.WindowsUserID
    Private ReadOnly _WindowsPrincipalForID As WindowsPrincipal = UFCWGeneral.WindowsPrincipalForID

    Private Shared ReadOnly _RuntimeArgs As String() = Environment.GetCommandLineArgs()

    'As New WindowsPrincipal(_WindowsUserID)

    <STAThread()> Public Shared Sub Main()

        '0 is execution name and path
        If _RuntimeArgs.Length > 1 Then
            If (CMSDALCommon.DefaultEnvironment <> _RuntimeArgs(1).ToString AndAlso _RuntimeArgs(1).ToString = "Q") Then CMSDALCommon.EnvironmentOverride = _RuntimeArgs(1).ToString
        End If

        CMSDALCommon.InitializeEL()

        AddHandler Application.ThreadException, AddressOf UFCWThreadExceptionHandler.Application_ThreadException
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf UFCWThreadExceptionHandler.CurrentDomain_UnhandledException

        Dim ExecutablePath As String
        Dim dataDirectory As String

        If ApplicationDeployment.IsNetworkDeployed Then ' This means the application is running as a ClickOnce application.
            ' ApplicationDeployment.CurrentDeployment.UpdateLocation.AbsolutePath = where the clcikonce was installed from

            ExecutablePath = AppDomain.CurrentDomain.BaseDirectory
            dataDirectory = ApplicationDeployment.CurrentDeployment.DataDirectory

            ' Use the dataDirectory as needed
            SaveSetting("UFCW\UFCW.CMS.CustomerService\", "ClickOnce", "FileInfo", ExecutablePath)
            SaveSetting("UFCW\UFCW.CMS.CustomerService\", "ClickOnce", "FileInfoData", dataDirectory)

        Else
            ExecutablePath = System.Reflection.Assembly.GetExecutingAssembly().Location
            Dim ExecutableDirectory As String = System.IO.Path.GetDirectoryName(ExecutablePath)

            If System.Diagnostics.Debugger.IsAttached Then
                SaveSetting("UFCW\UFCW.CMS.CustomerService\", "Debug", "FileInfo", ExecutablePath)
            Else
                SaveSetting("UFCW\UFCW.CMS.CustomerService\", "Prod", "FileInfo", ExecutablePath)
            End If
        End If

        Using SplashFrm As New SplashForm
            SplashFrm.Show()
        End Using

        Try

            Application.Run(New CustomerServiceMain)

        Catch ex As Exception
            Dim HoldExMessage As String = ex.Message
            Dim HoldExStackTrace As String = ex.StackTrace

            DoCrashProcedure(ex)
            MessageBox.Show($"Message: {HoldExMessage} Trace: {HoldExStackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try

    End Sub

    Shared Sub DoCrashProcedure(ByVal ex As Exception)

        Logger.Write(ex.Message & Environment.NewLine & ex.StackTrace)

        Logger.Write(New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString)

        Application.Exit()

    End Sub

    <System.Diagnostics.DebuggerStepThrough()>
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)

        Select Case ((m.WParam.ToInt64() And &HFFFF) And &HFFF0)

            Case &HF060 ' The user chose to close the form.
                Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable
        End Select

        MyBase.WndProc(m)

    End Sub

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
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem4 As System.Windows.Forms.MenuItem
    Friend WithEvents MainStatusBar As System.Windows.Forms.StatusBar
    Friend WithEvents InfoStatusBarPanel As System.Windows.Forms.StatusBarPanel
    Friend WithEvents DomainUserStatusBarPanel As System.Windows.Forms.StatusBarPanel
    Friend WithEvents DataStatusBarPanel As System.Windows.Forms.StatusBarPanel
    Friend WithEvents DateStatusBarPanel As System.Windows.Forms.StatusBarPanel
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CustomerServiceMain))
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.MenuItem2 = New System.Windows.Forms.MenuItem()
        Me.MenuItem3 = New System.Windows.Forms.MenuItem()
        Me.MenuItem5 = New System.Windows.Forms.MenuItem()
        Me.MenuItemDiagnosis = New System.Windows.Forms.MenuItem()
        Me.MenuItemProcedures = New System.Windows.Forms.MenuItem()
        Me.MenuItemModifiers = New System.Windows.Forms.MenuItem()
        Me.MenuItemBillTypes = New System.Windows.Forms.MenuItem()
        Me.MenuItemPlaceOfService = New System.Windows.Forms.MenuItem()
        Me.MenuItemNDC = New System.Windows.Forms.MenuItem()
        Me.MenuItem1 = New System.Windows.Forms.MenuItem()
        Me.MenuItem4 = New System.Windows.Forms.MenuItem()
        Me.ResetClaimsRegistryKeysMenu = New System.Windows.Forms.MenuItem()
        Me.MainStatusBar = New System.Windows.Forms.StatusBar()
        Me.InfoStatusBarPanel = New System.Windows.Forms.StatusBarPanel()
        Me.DomainUserStatusBarPanel = New System.Windows.Forms.StatusBarPanel()
        Me.DataStatusBarPanel = New System.Windows.Forms.StatusBarPanel()
        Me.DateStatusBarPanel = New System.Windows.Forms.StatusBarPanel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.CustomerServiceControl = New CustomerServiceControl()
        CType(Me.InfoStatusBarPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DomainUserStatusBarPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataStatusBarPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateStatusBarPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem2, Me.MenuItem5, Me.MenuItem1})
        '
        'MenuItem2
        '
        Me.MenuItem2.Index = 0
        Me.MenuItem2.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem3})
        Me.MenuItem2.Text = "File"
        '
        'MenuItem3
        '
        Me.MenuItem3.Index = 0
        Me.MenuItem3.Text = "Close"
        '
        'MenuItem5
        '
        Me.MenuItem5.Index = 1
        Me.MenuItem5.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItemDiagnosis, Me.MenuItemProcedures, Me.MenuItemModifiers, Me.MenuItemBillTypes, Me.MenuItemPlaceOfService, Me.MenuItemNDC})
        Me.MenuItem5.Text = "LookUp"
        '
        'MenuItemDiagnosis
        '
        Me.MenuItemDiagnosis.Index = 0
        Me.MenuItemDiagnosis.Text = "Diagnosis"
        '
        'MenuItemProcedures
        '
        Me.MenuItemProcedures.Index = 1
        Me.MenuItemProcedures.Text = "Procedures"
        '
        'MenuItemModifiers
        '
        Me.MenuItemModifiers.Index = 2
        Me.MenuItemModifiers.Text = "Modifiers"
        '
        'MenuItemBillTypes
        '
        Me.MenuItemBillTypes.Index = 3
        Me.MenuItemBillTypes.Text = "Bill Types"
        '
        'MenuItemPlaceOfService
        '
        Me.MenuItemPlaceOfService.Index = 4
        Me.MenuItemPlaceOfService.Text = "Place Of Service"
        '
        'MenuItemNDC
        '
        Me.MenuItemNDC.Index = 5
        Me.MenuItemNDC.Text = "NDC"
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 2
        Me.MenuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem4, Me.ResetClaimsRegistryKeysMenu})
        Me.MenuItem1.Text = "&Help"
        '
        'MenuItem4
        '
        Me.MenuItem4.Index = 0
        Me.MenuItem4.Text = "About"
        '
        'ResetClaimsRegistryKeysMenu
        '
        Me.ResetClaimsRegistryKeysMenu.Index = 1
        Me.ResetClaimsRegistryKeysMenu.Text = "&Reset Windows"
        '
        'MainStatusBar
        '
        Me.MainStatusBar.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.5!)
        Me.MainStatusBar.Location = New System.Drawing.Point(0, 478)
        Me.MainStatusBar.Name = "MainStatusBar"
        Me.MainStatusBar.Panels.AddRange(New System.Windows.Forms.StatusBarPanel() {Me.InfoStatusBarPanel, Me.DomainUserStatusBarPanel, Me.DataStatusBarPanel, Me.DateStatusBarPanel})
        Me.MainStatusBar.ShowPanels = True
        Me.MainStatusBar.Size = New System.Drawing.Size(577, 14)
        Me.MainStatusBar.TabIndex = 4
        Me.MainStatusBar.Text = "StatusBar1"
        '
        'InfoStatusBarPanel
        '
        Me.InfoStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring
        Me.InfoStatusBarPanel.Name = "InfoStatusBarPanel"
        Me.InfoStatusBarPanel.Width = 238
        '
        'DomainUserStatusBarPanel
        '
        Me.DomainUserStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.DomainUserStatusBarPanel.MinWidth = 120
        Me.DomainUserStatusBarPanel.Name = "DomainUserStatusBarPanel"
        Me.DomainUserStatusBarPanel.Width = 120
        '
        'DataStatusBarPanel
        '
        Me.DataStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.DataStatusBarPanel.MinWidth = 50
        Me.DataStatusBarPanel.Name = "DataStatusBarPanel"
        Me.DataStatusBarPanel.Text = "System / DataBase"
        Me.DataStatusBarPanel.Width = 102
        '
        'DateStatusBarPanel
        '
        Me.DateStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.DateStatusBarPanel.MinWidth = 100
        Me.DateStatusBarPanel.Name = "DateStatusBarPanel"
        Me.DateStatusBarPanel.Text = "Date / Time"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.CustomerServiceControl)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(577, 478)
        Me.Panel1.TabIndex = 5
        '
        'CustomerServiceControl
        '
        Me.CustomerServiceControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CustomerServiceControl.Location = New System.Drawing.Point(0, 0)
        Me.CustomerServiceControl.Name = "CustomerServiceControl"
        Me.CustomerServiceControl.Size = New System.Drawing.Size(577, 478)
        Me.CustomerServiceControl.TabIndex = 0
        '
        'CustomerServiceMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(577, 492)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.MainStatusBar)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Menu = Me.MainMenu1
        Me.MinimumSize = New System.Drawing.Size(400, 400)
        Me.Name = "CustomerServiceMain"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Customer Service"
        CType(Me.InfoStatusBarPanel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DomainUserStatusBarPanel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataStatusBarPanel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateStatusBarPanel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub CustomerServiceMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim Frm As Message
        Dim Result As String

        Try

            Frm = New Message

            'Check the access to application
            If Not (UFCWGeneralAD.CMSUsers OrElse UFCWGeneralAD.CMSLocals OrElse UFCWGeneralAD.CMSDental OrElse UFCWGeneralAD.CMSEligibility) Then
                MessageBox.Show("You do not have permission to use this application", "Check Access", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()
                Exit Sub
            End If

            If Not _ComparefolderLoc = "" AndAlso _ComparefolderLoc IsNot Nothing Then
                Result = UFCWGeneral.CompareFolders(_ComparefolderLoc, Application.StartupPath, _ExcludeXMLFiles)
                If Result.Length > 0 Then
                    Frm.DistributionLoc = _ComparefolderLoc
                    Frm.txtResult.Text = Result
                    If Result.Contains("MISSING") OrElse Result.Contains("DIFFERENT") Then
                        Frm.MsgLabel.Text = "You do not have the current version of the software. Please restart your PC. If error returns PLEASE CALL HELP DESK"
                        Frm.FormStatus = "ERROR"
                        Frm.ShowDialog(Me)
                        Me.Close()
                        Exit Sub
                    Else
                        Frm.MsgLabel.Text = ""
                        Frm.MsgLabel.ForeColor = Color.Blue
                        Frm.ShowIcon = False
                        Frm.Text = "Click OK to Continue..."
                        Frm.FormStatus = "WARNING"
                        Frm.ShowDialog(Me)
                    End If
                End If
            End If

            Dim FNTask As Task = Task.Factory.StartNew(
                    Sub()
                        Dim FNInfo As Tuple(Of String, String) = WCFWrapperCommon.InitializeFileNetUserInfo()

                        Using FnSession As New Session(New WCFProcessInfo With {.ByProcessName = True, .ProcessID = Process.GetCurrentProcess.Id, .ProcessName = Process.GetCurrentProcess.ProcessName})
                            FnSession.Logon(FNInfo.Item1, FNInfo.Item2)

                            'UpdateStatusBarPanel(SBar, FNUserPanelIndex, $"FN User ID: {FnSession.UserName}")
                            'UpdateStatusBarPanel(SBar, ServerPanelIndex, $"FN IMS: {FnSession.LibraryName}")

                        End Using
                    End Sub
                    )

            If Not UFCWGeneral.SetFormPosition(Me, _APPKEY) Then Me.CenterToScreen()

            Me.CustomerServiceControl.AppKey = _APPKEY
            Me.CustomerServiceControl.InitializeControl()

            _FrmLoading = True

            Me.MainStatusBar.Location = New Point(0, 720)
            Me.MainStatusBar.Size = New Size(546, 12)

            Me.DomainUserStatusBarPanel.Text = SystemInformation.UserName
            Me.DataStatusBarPanel.Text = "Server=" & CMSDALCommon.GetServerName(Nothing) & ";DB=" & CMSDALCommon.GetDatabaseName(Nothing)
            If Not Me.DataStatusBarPanel.Text.Contains("PLOC") Then
                Me.DataStatusBarPanel.Style = StatusBarPanelStyle.OwnerDraw
                AddHandler Me.MainStatusBar.DrawItem, AddressOf DrawCustomStatusBarPanel
            End If
            Me.DateStatusBarPanel.Text = Format(UFCWGeneral.NowDate, "MM-dd-yyyy")


            Me.InfoStatusBarPanel.Text = "Enter Search Criteria and press Enter to continue"

            If _RuntimeArgs.Length > 1 Then
                For Each Arg As String In _RuntimeArgs

                    Me.InfoStatusBarPanel.Text = "Remote search invoked using: " & Arg

                    Dim Parms As String() = Arg.Split(CChar("="))

                    Select Case True
                        Case Arg.Contains("CLAIM_ID")
                            Me.CustomerServiceControl.ClaimID = Parms(1)
                        Case Arg.Contains("FAMILY_ID")
                            Me.CustomerServiceControl.FamilyID = CType(Parms(1), Integer?)
                        Case Arg.Contains("PAT_SSN")
                            Me.CustomerServiceControl.PatientSSN = Parms(1)
                        Case Arg.Contains("PROV_TIN")
                            Me.CustomerServiceControl.ProviderID = Parms(1)
                    End Select
                Next

                Me.CustomerServiceControl.Search()

            End If

        Catch ex As Exception
            Me.ControlBox = True

            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            Else
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Finally

            If Frm IsNot Nothing Then
                Frm.Dispose()
            End If
            Frm = Nothing

        End Try
    End Sub
    Private Sub DrawCustomStatusBarPanel(ByVal sender As Object, ByVal e As StatusBarDrawItemEventArgs)

        ' Draw a blue background in the owner-drawn panel.
        'e.Graphics.FillRectangle(Brushes., e.Bounds)

        ' Create a StringFormat object to align text in the panel.
        Using textFormat As New StringFormat()

            ' Center the text in the middle of the line.
            textFormat.LineAlignment = StringAlignment.Near

            ' Align the text to the left.
            textFormat.Alignment = StringAlignment.Near

            ' Draw the panel's text in dark blue using the Panel 
            ' and Bounds properties of the StatusBarEventArgs object 
            ' and the StringFormat object.
            e.Graphics.DrawString(e.Panel.Text, e.Font, Brushes.Red, New RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height), textFormat)
        End Using
    End Sub

    Private Sub CustomerServiceMain_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        Try

            UFCWGeneral.SaveFormPosition(Me)

            Using FNSession As New Session(New WCFProcessInfo With {.ByProcessName = False, .ProcessID = Process.GetCurrentProcess.Id, .ProcessName = Process.GetCurrentProcess.ProcessName})
                FNSession.Logoff()
            End Using

        Catch ex As Exception
        Finally

            If CustomerServiceControl IsNot Nothing Then
                CustomerServiceControl.Dispose()
            End If

            CustomerServiceControl = Nothing
        End Try
    End Sub

    Private Sub CustomerServiceMain_Activated(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles MyBase.Activated
        Try

            If _FrmLoading Then
                CustomerServiceControl.SetFocus()
                Me.Refresh()
                _FrmLoading = False
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub MenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem3.Click
        Try
            Me.Close()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub MenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem4.Click
        Dim Frm As AboutForm

        Try
            Frm = New AboutForm
            Frm.ShowDialog(Me)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub MenuItemNDC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemNDC.Click
        Try

            CustomerServiceControl.ShowNDCLookup()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub MenuItemDiagnosis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemDiagnosis.Click
        Try

            CustomerServiceControl.ShowDiagnosisLookup()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub MenuItemModifiers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemModifiers.Click
        Try

            CustomerServiceControl.ShowModifierLookup()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub MenuItemProcedures_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemProcedures.Click

        Try

            CustomerServiceControl.ShowProcedureLookup()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub MenuItemBillTypes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemBillTypes.Click
        Try

            CustomerServiceControl.ShowBillTypeLookup()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub MenuItemPlaceOfService_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemPlaceOfService.Click
        Try

            CustomerServiceControl.ShowPlaceOfServiceLookup()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DeleteClaimsRegistry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResetClaimsRegistryKeysMenu.Click
        Try
            My.Computer.Registry.CurrentUser.DeleteSubKeyTree("Software\VB and VBA Program Settings\" & _APPKEY)
        Catch ex As Exception
        Finally
            My.Computer.Registry.CurrentUser.Flush()
        End Try
    End Sub

End Class