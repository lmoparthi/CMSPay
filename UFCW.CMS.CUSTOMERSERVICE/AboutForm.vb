Imports System.IO
Imports System.Security.Principal
Imports System.Data
Imports System.ComponentModel

Public Class AboutForm
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _WindowsUserID As WindowsIdentity = WindowsIdentity.GetCurrent()
    Private _WindowsPrincipalForID As WindowsPrincipal = New WindowsPrincipal(_WindowsUserID)
    Private _DomainUser As String = SystemInformation.UserName

    Friend WithEvents cbCMSUserAccess As System.Windows.Forms.CheckBox
    Friend WithEvents cbCanRemovePricingAccess As System.Windows.Forms.CheckBox
    Friend WithEvents cbCanReopenPartialAccess As System.Windows.Forms.CheckBox
    Friend WithEvents cbCanReopenFullAccess As System.Windows.Forms.CheckBox
    Friend WithEvents cbCanRunReports As System.Windows.Forms.CheckBox
    Friend WithEvents cbCanModifyCOB As System.Windows.Forms.CheckBox
    Friend WithEvents cbCMSDentalAccess As System.Windows.Forms.CheckBox
    Friend WithEvents cbCanViewHours As System.Windows.Forms.CheckBox
    Friend WithEvents cbCanViewEligibilityHours As System.Windows.Forms.CheckBox
    Friend WithEvents cbCanCreateClaim As System.Windows.Forms.CheckBox
    Friend WithEvents cbEligMaintenanceAccess As CheckBox
    Friend WithEvents cbCanRePrintEOB As CheckBox
    Friend WithEvents cbCMSCanViewRxDetail As CheckBox
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents DocumentSecurity As ListBox
    Friend WithEvents FileVersions As ListBox
    Friend WithEvents lblCommandLineArgs As Label
    Friend WithEvents lblWorkstation As Label
    Friend WithEvents lblDefaultDB2Database As Label
    Friend WithEvents lblDefaultSQLDatabase As Label
    Friend WithEvents lblADUser As Label

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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblProductVersion As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Pic As System.Windows.Forms.PictureBox
    Friend WithEvents OK As System.Windows.Forms.Button
    Friend WithEvents cbCanUTL As System.Windows.Forms.CheckBox
    Friend WithEvents cbCanOverride As System.Windows.Forms.CheckBox
    Friend WithEvents cbReprocess As System.Windows.Forms.CheckBox
    Friend WithEvents cbCanPickWork As System.Windows.Forms.CheckBox
    Friend WithEvents cbAuditAccess As System.Windows.Forms.CheckBox
    Friend WithEvents cbLocalsEmployeeAccess As System.Windows.Forms.CheckBox
    Friend WithEvents cbEmployeeAccess As System.Windows.Forms.CheckBox
    Friend WithEvents cbEligibilityEmployeeAccess As System.Windows.Forms.CheckBox
    Friend WithEvents cbEligibilityAccess As System.Windows.Forms.CheckBox
    Friend WithEvents cbHRAAccess As System.Windows.Forms.CheckBox
    Friend WithEvents cbHRAEmployeeAccess As System.Windows.Forms.CheckBox
    Friend WithEvents cbLocalAccess As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AboutForm))
        Me.OK = New System.Windows.Forms.Button()
        Me.Pic = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblProductVersion = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cbLocalsEmployeeAccess = New System.Windows.Forms.CheckBox()
        Me.cbCanUTL = New System.Windows.Forms.CheckBox()
        Me.cbCanOverride = New System.Windows.Forms.CheckBox()
        Me.cbReprocess = New System.Windows.Forms.CheckBox()
        Me.cbCanPickWork = New System.Windows.Forms.CheckBox()
        Me.cbAuditAccess = New System.Windows.Forms.CheckBox()
        Me.cbEmployeeAccess = New System.Windows.Forms.CheckBox()
        Me.cbLocalAccess = New System.Windows.Forms.CheckBox()
        Me.cbEligibilityEmployeeAccess = New System.Windows.Forms.CheckBox()
        Me.cbEligibilityAccess = New System.Windows.Forms.CheckBox()
        Me.cbHRAAccess = New System.Windows.Forms.CheckBox()
        Me.cbHRAEmployeeAccess = New System.Windows.Forms.CheckBox()
        Me.cbCMSUserAccess = New System.Windows.Forms.CheckBox()
        Me.cbCanRemovePricingAccess = New System.Windows.Forms.CheckBox()
        Me.cbCanReopenPartialAccess = New System.Windows.Forms.CheckBox()
        Me.cbCanReopenFullAccess = New System.Windows.Forms.CheckBox()
        Me.cbCanRunReports = New System.Windows.Forms.CheckBox()
        Me.cbCanModifyCOB = New System.Windows.Forms.CheckBox()
        Me.cbCMSDentalAccess = New System.Windows.Forms.CheckBox()
        Me.cbCanViewHours = New System.Windows.Forms.CheckBox()
        Me.cbCanViewEligibilityHours = New System.Windows.Forms.CheckBox()
        Me.cbCanCreateClaim = New System.Windows.Forms.CheckBox()
        Me.cbEligMaintenanceAccess = New System.Windows.Forms.CheckBox()
        Me.cbCanRePrintEOB = New System.Windows.Forms.CheckBox()
        Me.cbCMSCanViewRxDetail = New System.Windows.Forms.CheckBox()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.DocumentSecurity = New System.Windows.Forms.ListBox()
        Me.FileVersions = New System.Windows.Forms.ListBox()
        Me.lblCommandLineArgs = New System.Windows.Forms.Label()
        Me.lblWorkstation = New System.Windows.Forms.Label()
        Me.lblADUser = New System.Windows.Forms.Label()
        Me.lblDefaultDB2Database = New System.Windows.Forms.Label()
        Me.lblDefaultSQLDatabase = New System.Windows.Forms.Label()
        CType(Me.Pic, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'OK
        '
        Me.OK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OK.Location = New System.Drawing.Point(518, 555)
        Me.OK.Name = "OK"
        Me.OK.Size = New System.Drawing.Size(75, 23)
        Me.OK.TabIndex = 0
        Me.OK.Text = "OK"
        '
        'Pic
        '
        Me.Pic.Image = CType(resources.GetObject("Pic.Image"), System.Drawing.Image)
        Me.Pic.Location = New System.Drawing.Point(8, 8)
        Me.Pic.Name = "Pic"
        Me.Pic.Size = New System.Drawing.Size(64, 48)
        Me.Pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Pic.TabIndex = 1
        Me.Pic.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(78, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(172, 17)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "CMS Customer Service"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(78, 24)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(16, 17)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "v"
        '
        'lblProductVersion
        '
        Me.lblProductVersion.AutoSize = True
        Me.lblProductVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProductVersion.Location = New System.Drawing.Point(94, 24)
        Me.lblProductVersion.Name = "lblProductVersion"
        Me.lblProductVersion.Size = New System.Drawing.Size(17, 17)
        Me.lblProductVersion.TabIndex = 5
        Me.lblProductVersion.Text = "0"
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(8, 72)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(232, 32)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "This Product Was Designed For Use By: So. Cal. UFCW Trust Fund"
        '
        'cbLocalsEmployeeAccess
        '
        Me.cbLocalsEmployeeAccess.AutoSize = True
        Me.cbLocalsEmployeeAccess.Enabled = False
        Me.cbLocalsEmployeeAccess.Location = New System.Drawing.Point(254, 8)
        Me.cbLocalsEmployeeAccess.Name = "cbLocalsEmployeeAccess"
        Me.cbLocalsEmployeeAccess.Size = New System.Drawing.Size(175, 17)
        Me.cbLocalsEmployeeAccess.TabIndex = 8
        Me.cbLocalsEmployeeAccess.Text = "Union Locals Employee Access"
        '
        'cbCanUTL
        '
        Me.cbCanUTL.AutoSize = True
        Me.cbCanUTL.Enabled = False
        Me.cbCanUTL.Location = New System.Drawing.Point(446, 98)
        Me.cbCanUTL.Name = "cbCanUTL"
        Me.cbCanUTL.Size = New System.Drawing.Size(47, 17)
        Me.cbCanUTL.TabIndex = 19
        Me.cbCanUTL.Text = "UTL"
        '
        'cbCanOverride
        '
        Me.cbCanOverride.AutoSize = True
        Me.cbCanOverride.Enabled = False
        Me.cbCanOverride.Location = New System.Drawing.Point(446, 44)
        Me.cbCanOverride.Name = "cbCanOverride"
        Me.cbCanOverride.Size = New System.Drawing.Size(88, 17)
        Me.cbCanOverride.TabIndex = 18
        Me.cbCanOverride.Text = "Can Override"
        '
        'cbReprocess
        '
        Me.cbReprocess.AutoSize = True
        Me.cbReprocess.Enabled = False
        Me.cbReprocess.Location = New System.Drawing.Point(446, 80)
        Me.cbReprocess.Name = "cbReprocess"
        Me.cbReprocess.Size = New System.Drawing.Size(77, 17)
        Me.cbReprocess.TabIndex = 17
        Me.cbReprocess.Text = "Reprocess"
        '
        'cbCanPickWork
        '
        Me.cbCanPickWork.AutoSize = True
        Me.cbCanPickWork.Enabled = False
        Me.cbCanPickWork.Location = New System.Drawing.Point(446, 26)
        Me.cbCanPickWork.Name = "cbCanPickWork"
        Me.cbCanPickWork.Size = New System.Drawing.Size(98, 17)
        Me.cbCanPickWork.TabIndex = 16
        Me.cbCanPickWork.Text = "Can Pick Work"
        '
        'cbAuditAccess
        '
        Me.cbAuditAccess.AutoSize = True
        Me.cbAuditAccess.Enabled = False
        Me.cbAuditAccess.Location = New System.Drawing.Point(446, 62)
        Me.cbAuditAccess.Name = "cbAuditAccess"
        Me.cbAuditAccess.Size = New System.Drawing.Size(88, 17)
        Me.cbAuditAccess.TabIndex = 15
        Me.cbAuditAccess.Text = "Audit Access"
        '
        'cbEmployeeAccess
        '
        Me.cbEmployeeAccess.AutoSize = True
        Me.cbEmployeeAccess.Enabled = False
        Me.cbEmployeeAccess.Location = New System.Drawing.Point(254, 44)
        Me.cbEmployeeAccess.Name = "cbEmployeeAccess"
        Me.cbEmployeeAccess.Size = New System.Drawing.Size(110, 17)
        Me.cbEmployeeAccess.TabIndex = 20
        Me.cbEmployeeAccess.Text = "Employee Access"
        '
        'cbLocalAccess
        '
        Me.cbLocalAccess.AutoSize = True
        Me.cbLocalAccess.Enabled = False
        Me.cbLocalAccess.Location = New System.Drawing.Point(254, 116)
        Me.cbLocalAccess.Name = "cbLocalAccess"
        Me.cbLocalAccess.Size = New System.Drawing.Size(126, 17)
        Me.cbLocalAccess.TabIndex = 21
        Me.cbLocalAccess.Text = "Union Locals Access"
        '
        'cbEligibilityEmployeeAccess
        '
        Me.cbEligibilityEmployeeAccess.AutoSize = True
        Me.cbEligibilityEmployeeAccess.Enabled = False
        Me.cbEligibilityEmployeeAccess.Location = New System.Drawing.Point(254, 26)
        Me.cbEligibilityEmployeeAccess.Name = "cbEligibilityEmployeeAccess"
        Me.cbEligibilityEmployeeAccess.Size = New System.Drawing.Size(152, 17)
        Me.cbEligibilityEmployeeAccess.TabIndex = 22
        Me.cbEligibilityEmployeeAccess.Text = "Eligibility Employee Access"
        '
        'cbEligibilityAccess
        '
        Me.cbEligibilityAccess.AutoSize = True
        Me.cbEligibilityAccess.Enabled = False
        Me.cbEligibilityAccess.Location = New System.Drawing.Point(254, 62)
        Me.cbEligibilityAccess.Name = "cbEligibilityAccess"
        Me.cbEligibilityAccess.Size = New System.Drawing.Size(103, 17)
        Me.cbEligibilityAccess.TabIndex = 23
        Me.cbEligibilityAccess.Text = "Eligibility Access"
        '
        'cbHRAAccess
        '
        Me.cbHRAAccess.AutoSize = True
        Me.cbHRAAccess.Enabled = False
        Me.cbHRAAccess.Location = New System.Drawing.Point(254, 80)
        Me.cbHRAAccess.Name = "cbHRAAccess"
        Me.cbHRAAccess.Size = New System.Drawing.Size(87, 17)
        Me.cbHRAAccess.TabIndex = 24
        Me.cbHRAAccess.Text = "HRA Access"
        '
        'cbHRAEmployeeAccess
        '
        Me.cbHRAEmployeeAccess.AutoSize = True
        Me.cbHRAEmployeeAccess.Enabled = False
        Me.cbHRAEmployeeAccess.Location = New System.Drawing.Point(254, 98)
        Me.cbHRAEmployeeAccess.Name = "cbHRAEmployeeAccess"
        Me.cbHRAEmployeeAccess.Size = New System.Drawing.Size(136, 17)
        Me.cbHRAEmployeeAccess.TabIndex = 25
        Me.cbHRAEmployeeAccess.Text = "HRA Employee Access"
        '
        'cbCMSUserAccess
        '
        Me.cbCMSUserAccess.AutoSize = True
        Me.cbCMSUserAccess.Enabled = False
        Me.cbCMSUserAccess.Location = New System.Drawing.Point(446, 8)
        Me.cbCMSUserAccess.Name = "cbCMSUserAccess"
        Me.cbCMSUserAccess.Size = New System.Drawing.Size(74, 17)
        Me.cbCMSUserAccess.TabIndex = 26
        Me.cbCMSUserAccess.Text = "CMS User"
        '
        'cbCanRemovePricingAccess
        '
        Me.cbCanRemovePricingAccess.AutoSize = True
        Me.cbCanRemovePricingAccess.Enabled = False
        Me.cbCanRemovePricingAccess.Location = New System.Drawing.Point(446, 116)
        Me.cbCanRemovePricingAccess.Name = "cbCanRemovePricingAccess"
        Me.cbCanRemovePricingAccess.Size = New System.Drawing.Size(123, 17)
        Me.cbCanRemovePricingAccess.TabIndex = 27
        Me.cbCanRemovePricingAccess.Text = "Can Remove Pricing"
        '
        'cbCanReopenPartialAccess
        '
        Me.cbCanReopenPartialAccess.AutoSize = True
        Me.cbCanReopenPartialAccess.Enabled = False
        Me.cbCanReopenPartialAccess.Location = New System.Drawing.Point(446, 134)
        Me.cbCanReopenPartialAccess.Name = "cbCanReopenPartialAccess"
        Me.cbCanReopenPartialAccess.Size = New System.Drawing.Size(127, 17)
        Me.cbCanReopenPartialAccess.TabIndex = 29
        Me.cbCanReopenPartialAccess.Text = "Reopen Own Access"
        '
        'cbCanReopenFullAccess
        '
        Me.cbCanReopenFullAccess.AutoSize = True
        Me.cbCanReopenFullAccess.Enabled = False
        Me.cbCanReopenFullAccess.Location = New System.Drawing.Point(254, 134)
        Me.cbCanReopenFullAccess.Name = "cbCanReopenFullAccess"
        Me.cbCanReopenFullAccess.Size = New System.Drawing.Size(116, 17)
        Me.cbCanReopenFullAccess.TabIndex = 28
        Me.cbCanReopenFullAccess.Text = "Reopen All Access"
        '
        'cbCanRunReports
        '
        Me.cbCanRunReports.AutoSize = True
        Me.cbCanRunReports.Enabled = False
        Me.cbCanRunReports.Location = New System.Drawing.Point(254, 152)
        Me.cbCanRunReports.Name = "cbCanRunReports"
        Me.cbCanRunReports.Size = New System.Drawing.Size(86, 17)
        Me.cbCanRunReports.TabIndex = 30
        Me.cbCanRunReports.Text = "Run Reports"
        '
        'cbCanModifyCOB
        '
        Me.cbCanModifyCOB.AutoSize = True
        Me.cbCanModifyCOB.Enabled = False
        Me.cbCanModifyCOB.Location = New System.Drawing.Point(446, 152)
        Me.cbCanModifyCOB.Name = "cbCanModifyCOB"
        Me.cbCanModifyCOB.Size = New System.Drawing.Size(82, 17)
        Me.cbCanModifyCOB.TabIndex = 31
        Me.cbCanModifyCOB.Text = "Modify COB"
        '
        'cbCMSDentalAccess
        '
        Me.cbCMSDentalAccess.AutoSize = True
        Me.cbCMSDentalAccess.Enabled = False
        Me.cbCMSDentalAccess.Location = New System.Drawing.Point(254, 171)
        Me.cbCMSDentalAccess.Name = "cbCMSDentalAccess"
        Me.cbCMSDentalAccess.Size = New System.Drawing.Size(57, 17)
        Me.cbCMSDentalAccess.TabIndex = 32
        Me.cbCMSDentalAccess.Text = "Dental"
        '
        'cbCanViewHours
        '
        Me.cbCanViewHours.AutoSize = True
        Me.cbCanViewHours.Enabled = False
        Me.cbCanViewHours.Location = New System.Drawing.Point(446, 171)
        Me.cbCanViewHours.Name = "cbCanViewHours"
        Me.cbCanViewHours.Size = New System.Drawing.Size(80, 17)
        Me.cbCanViewHours.TabIndex = 46
        Me.cbCanViewHours.Text = "View Hours"
        '
        'cbCanViewEligibilityHours
        '
        Me.cbCanViewEligibilityHours.AutoSize = True
        Me.cbCanViewEligibilityHours.Enabled = False
        Me.cbCanViewEligibilityHours.Location = New System.Drawing.Point(446, 189)
        Me.cbCanViewEligibilityHours.Name = "cbCanViewEligibilityHours"
        Me.cbCanViewEligibilityHours.Size = New System.Drawing.Size(91, 17)
        Me.cbCanViewEligibilityHours.TabIndex = 47
        Me.cbCanViewEligibilityHours.Text = "View Eligibility"
        '
        'cbCanCreateClaim
        '
        Me.cbCanCreateClaim.AutoSize = True
        Me.cbCanCreateClaim.Enabled = False
        Me.cbCanCreateClaim.Location = New System.Drawing.Point(254, 189)
        Me.cbCanCreateClaim.Name = "cbCanCreateClaim"
        Me.cbCanCreateClaim.Size = New System.Drawing.Size(107, 17)
        Me.cbCanCreateClaim.TabIndex = 48
        Me.cbCanCreateClaim.Text = "Can Create Claim"
        '
        'cbEligMaintenanceAccess
        '
        Me.cbEligMaintenanceAccess.AutoSize = True
        Me.cbEligMaintenanceAccess.Enabled = False
        Me.cbEligMaintenanceAccess.Location = New System.Drawing.Point(254, 208)
        Me.cbEligMaintenanceAccess.Name = "cbEligMaintenanceAccess"
        Me.cbEligMaintenanceAccess.Size = New System.Drawing.Size(130, 17)
        Me.cbEligMaintenanceAccess.TabIndex = 51
        Me.cbEligMaintenanceAccess.Text = "Can Maintain Eligibility"
        '
        'cbCanRePrintEOB
        '
        Me.cbCanRePrintEOB.AutoSize = True
        Me.cbCanRePrintEOB.Enabled = False
        Me.cbCanRePrintEOB.Location = New System.Drawing.Point(446, 208)
        Me.cbCanRePrintEOB.Name = "cbCanRePrintEOB"
        Me.cbCanRePrintEOB.Size = New System.Drawing.Size(108, 17)
        Me.cbCanRePrintEOB.TabIndex = 52
        Me.cbCanRePrintEOB.Text = "Can RePrint EOB"
        '
        'cbCMSCanViewRxDetail
        '
        Me.cbCMSCanViewRxDetail.AutoSize = True
        Me.cbCMSCanViewRxDetail.Enabled = False
        Me.cbCMSCanViewRxDetail.Location = New System.Drawing.Point(254, 226)
        Me.cbCMSCanViewRxDetail.Name = "cbCMSCanViewRxDetail"
        Me.cbCMSCanViewRxDetail.Size = New System.Drawing.Size(91, 17)
        Me.cbCMSCanViewRxDetail.TabIndex = 53
        Me.cbCMSCanViewRxDetail.Text = "Enhanced Rx"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(11, 249)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.DocumentSecurity)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.FileVersions)
        Me.SplitContainer1.Size = New System.Drawing.Size(582, 300)
        Me.SplitContainer1.SplitterDistance = 150
        Me.SplitContainer1.TabIndex = 54
        '
        'DocumentSecurity
        '
        Me.DocumentSecurity.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DocumentSecurity.HorizontalScrollbar = True
        Me.DocumentSecurity.Location = New System.Drawing.Point(0, 0)
        Me.DocumentSecurity.Name = "DocumentSecurity"
        Me.DocumentSecurity.SelectionMode = System.Windows.Forms.SelectionMode.None
        Me.DocumentSecurity.Size = New System.Drawing.Size(582, 150)
        Me.DocumentSecurity.Sorted = True
        Me.DocumentSecurity.TabIndex = 51
        '
        'FileVersions
        '
        Me.FileVersions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FileVersions.HorizontalScrollbar = True
        Me.FileVersions.Location = New System.Drawing.Point(0, 0)
        Me.FileVersions.Name = "FileVersions"
        Me.FileVersions.SelectionMode = System.Windows.Forms.SelectionMode.None
        Me.FileVersions.Size = New System.Drawing.Size(582, 146)
        Me.FileVersions.Sorted = True
        Me.FileVersions.TabIndex = 8
        '
        'lblCommandLineArgs
        '
        Me.lblCommandLineArgs.AutoSize = True
        Me.lblCommandLineArgs.Location = New System.Drawing.Point(15, 116)
        Me.lblCommandLineArgs.Name = "lblCommandLineArgs"
        Me.lblCommandLineArgs.Size = New System.Drawing.Size(19, 13)
        Me.lblCommandLineArgs.TabIndex = 55
        Me.lblCommandLineArgs.Text = "    "
        '
        'lblWorkstation
        '
        Me.lblWorkstation.AutoSize = True
        Me.lblWorkstation.Location = New System.Drawing.Point(15, 194)
        Me.lblWorkstation.Name = "lblWorkstation"
        Me.lblWorkstation.Size = New System.Drawing.Size(19, 13)
        Me.lblWorkstation.TabIndex = 56
        Me.lblWorkstation.Text = "    "
        '
        'lblADUser
        '
        Me.lblADUser.AutoSize = True
        Me.lblADUser.Location = New System.Drawing.Point(15, 220)
        Me.lblADUser.Name = "lblADUser"
        Me.lblADUser.Size = New System.Drawing.Size(19, 13)
        Me.lblADUser.TabIndex = 57
        Me.lblADUser.Text = "    "
        '
        'lblDefaultDB2Database
        '
        Me.lblDefaultDB2Database.AutoSize = True
        Me.lblDefaultDB2Database.Location = New System.Drawing.Point(15, 168)
        Me.lblDefaultDB2Database.Name = "lblDefaultDB2Database"
        Me.lblDefaultDB2Database.Size = New System.Drawing.Size(19, 13)
        Me.lblDefaultDB2Database.TabIndex = 58
        Me.lblDefaultDB2Database.Text = "    "
        '
        'lblDefaultSQLDatabase
        '
        Me.lblDefaultSQLDatabase.AutoSize = True
        Me.lblDefaultSQLDatabase.Location = New System.Drawing.Point(15, 142)
        Me.lblDefaultSQLDatabase.Name = "lblDefaultSQLDatabase"
        Me.lblDefaultSQLDatabase.Size = New System.Drawing.Size(19, 13)
        Me.lblDefaultSQLDatabase.TabIndex = 59
        Me.lblDefaultSQLDatabase.Text = "    "
        '
        'AboutForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(598, 580)
        Me.Controls.Add(Me.lblDefaultSQLDatabase)
        Me.Controls.Add(Me.lblDefaultDB2Database)
        Me.Controls.Add(Me.lblADUser)
        Me.Controls.Add(Me.lblWorkstation)
        Me.Controls.Add(Me.lblCommandLineArgs)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.cbCMSCanViewRxDetail)
        Me.Controls.Add(Me.cbCanRePrintEOB)
        Me.Controls.Add(Me.cbEligMaintenanceAccess)
        Me.Controls.Add(Me.cbCanCreateClaim)
        Me.Controls.Add(Me.cbCanViewEligibilityHours)
        Me.Controls.Add(Me.cbCanViewHours)
        Me.Controls.Add(Me.cbCMSDentalAccess)
        Me.Controls.Add(Me.cbCanModifyCOB)
        Me.Controls.Add(Me.cbCanRunReports)
        Me.Controls.Add(Me.cbCanReopenPartialAccess)
        Me.Controls.Add(Me.cbCanReopenFullAccess)
        Me.Controls.Add(Me.cbCanRemovePricingAccess)
        Me.Controls.Add(Me.cbCMSUserAccess)
        Me.Controls.Add(Me.cbHRAEmployeeAccess)
        Me.Controls.Add(Me.cbHRAAccess)
        Me.Controls.Add(Me.cbEligibilityAccess)
        Me.Controls.Add(Me.cbEligibilityEmployeeAccess)
        Me.Controls.Add(Me.cbLocalAccess)
        Me.Controls.Add(Me.cbEmployeeAccess)
        Me.Controls.Add(Me.cbCanUTL)
        Me.Controls.Add(Me.cbCanOverride)
        Me.Controls.Add(Me.cbReprocess)
        Me.Controls.Add(Me.cbCanPickWork)
        Me.Controls.Add(Me.cbAuditAccess)
        Me.Controls.Add(Me.cbLocalsEmployeeAccess)
        Me.Controls.Add(Me.lblProductVersion)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Pic)
        Me.Controls.Add(Me.OK)
        Me.Controls.Add(Me.Label3)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AboutForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "About Customer Service"
        CType(Me.Pic, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub About_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not UFCWGeneral.SetFormPosition(Me) Then Me.CenterToScreen()

        lblProductVersion.Text = Application.ProductVersion

        CollectVersions()

        CollectSecurity()

        Dim ArgCount As Integer = 0

        If Environment.GetCommandLineArgs.Count > 1 Then
            lblCommandLineArgs.Text = String.Join(", ", Environment.GetCommandLineArgs, 1, Environment.GetCommandLineArgs.Count - 1)
        End If

        lblADUser.Text = UFCWGeneral.DomainUser
        lblWorkstation.Text = UFCWGeneral.ComputerName
        lblDefaultDB2Database.Text = CMSDALCommon.DefaultDB2Database
        lblDefaultSQLDatabase.Text = CMSDALCommon.DefaultSQLDatabase

    End Sub

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        Me.Close()
    End Sub

    Private Sub CollectVersions()
        Dim FInfo As FileInfo = New FileInfo(Application.ExecutablePath)
        Dim DirInfo As DirectoryInfo = New DirectoryInfo(FInfo.DirectoryName)

        Dim DLLFileArray() As FileInfo = DirInfo.GetFiles("*.dll")
        Dim EXEFileArray() As FileInfo = DirInfo.GetFiles("*.exe")

        FileVersions.BeginUpdate()
        For Each FileInfo As FileInfo In DLLFileArray
            If FileInfo.Name.IndexOf("UFCW.CMS") >= 0 Then
                With System.Diagnostics.FileVersionInfo.GetVersionInfo(FileInfo.FullName)
                    FileVersions.Items.Add($"{FileInfo.FullName} ({ .ProductVersion}, { .FileMajorPart}.{ .FileMinorPart}.{ .FileBuildPart}.{ .FilePrivatePart})")
                End With
            End If
        Next
        For Each FileInfo As FileInfo In EXEFileArray
            With System.Diagnostics.FileVersionInfo.GetVersionInfo(FileInfo.FullName)
                FileVersions.Items.Add($"{FileInfo.FullName} ({ .ProductVersion}, { .FileMajorPart}.{ .FileMinorPart}.{ .FileBuildPart}.{ .FilePrivatePart})")
            End With
        Next
        FileVersions.EndUpdate()

    End Sub

    Private Sub CollectSecurity()


        Dim DocSecurityDT As DataTable
        Dim ConnectionName As String
        Dim DocClassSecurityDT As DataTable

        Try
            ConnectionName = CMSDALCommon.DefaultDatabase

            Me.cbCMSUserAccess.Checked = UFCWGeneralAD.CMSUsers
            Me.cbCMSDentalAccess.Checked = UFCWGeneralAD.CMSDental
            Me.cbLocalsEmployeeAccess.Checked = UFCWGeneralAD.CMSLocalsEmployee
            Me.cbLocalAccess.Checked = UFCWGeneralAD.CMSLocals
            Me.cbEmployeeAccess.Checked = UFCWGeneralAD.CMSCanAdjudicateEmployee
            Me.cbCanPickWork.Checked = UFCWGeneralAD.CMSCanPickWork
            Me.cbAuditAccess.Checked = UFCWGeneralAD.CMSCanAudit
            Me.cbReprocess.Checked = UFCWGeneralAD.CMSCanReprocess
            Me.cbCanOverride.Checked = UFCWGeneralAD.CMSCanOverrideAccumulators
            Me.cbCanCreateClaim.Checked = UFCWGeneralAD.CMSCanCreateClaim
            Me.cbCanUTL.Checked = UFCWGeneralAD.CMSUTL
            Me.cbEligibilityAccess.Checked = UFCWGeneralAD.CMSEligibility
            Me.cbEligibilityEmployeeAccess.Checked = UFCWGeneralAD.CMSEligibilityEmployee
            Me.cbHRAAccess.Checked = UFCWGeneralAD.CMSHRA
            Me.cbHRAEmployeeAccess.Checked = UFCWGeneralAD.CMSHRAEmployee
            Me.cbCanRemovePricingAccess.Checked = UFCWGeneralAD.CMSCanRemovePricing
            Me.cbCanReopenFullAccess.Checked = UFCWGeneralAD.CMSCanReopenFull
            Me.cbCanReopenPartialAccess.Checked = UFCWGeneralAD.CMSCanReopenPartial
            Me.cbCanRunReports.Checked = UFCWGeneralAD.CMSCanRunReports
            Me.cbCanModifyCOB.Checked = UFCWGeneralAD.CMSCanModifyCOB
            Me.cbCanViewHours.Checked = UFCWGeneralAD.CMSCanViewHours
            Me.cbCanViewEligibilityHours.Checked = UFCWGeneralAD.CMSCanViewEligibilityHours
            Me.cbEligMaintenanceAccess.Checked = UFCWGeneralAD.REGMEligMaintenanceAccess
            Me.cbCanRePrintEOB.Checked = UFCWGeneralAD.CMSCanRePrintEOB
            Me.cbCMSCanViewRxDetail.Checked = UFCWGeneralAD.CMSCanViewRxDetail

            DocSecurityDT = CMSDALDBO.ExecuteSQLWithResultSet("ImgWorkflow", "SELECT B.* FROM IMGWORKFLOW.DBO.WFL_USERDOCCLASSES A INNER JOIN IMGWORKFLOW.DBO.WFL_USERDOCTYPES B ON A.NTUserID = B.NTUserID AND A.DocumentClass = B.DocumentClass WHERE A.NTUSERID  = '" & _DomainUser.ToString & "'")

            For Each DR As DataRow In DocSecurityDT.Rows
                DocumentSecurity.Items.Add($"{DR("DocumentClass")}{vbTab} -> {DR("DocType")}")
            Next

            DocClassSecurityDT = CMSDALDBO.ExecuteSQLWithResultSet("ImgWorkflow", "SELECT B.* FROM IMGWORKFLOW.DBO.WFL_USERDOCCLASSES A INNER JOIN IMGWORKFLOW.DBO.WFL_USERDOCTYPES B ON A.NTUserID = B.NTUserID And A.DocumentClass = B.DocumentClass WHERE A.EmployeeItem = 1 And A.NTUSERID  = '" & _DomainUser.ToString & "'")

            For Each DR As DataRow In DocClassSecurityDT.Rows
                DocumentSecurity.Items.Add($"{DR("DocumentClass")}{vbTab} -> {DR("DocType")} (Employee)")
            Next

        Catch ex As Exception

        End Try



    End Sub

    Private Sub AboutForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

        UFCWGeneral.SaveFormPosition(Me)

    End Sub
End Class