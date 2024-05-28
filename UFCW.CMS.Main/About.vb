Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.IO
Imports System.Security.Principal
Imports System.Threading.Tasks


Public Class About
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents ImagingDocumentTypes As System.Windows.Forms.ListBox

    Private _WindowsPrincipalForID As WindowsPrincipal
    Private _DomainUser As String = SystemInformation.UserName
    Friend WithEvents lblDefaultSQLDatabase As Label
    Friend WithEvents lblDefaultDB2Database As Label
    Friend WithEvents lblADUser As Label
    Friend WithEvents lblWorkstation As Label
    Friend WithEvents lblCommandLineArgs As Label
    Private _DefaultProviderName As String = CType(ConfigurationManager.GetSection("dataConfiguration"), Microsoft.Practices.EnterpriseLibrary.Data.Configuration.DatabaseSettings).DefaultDatabase

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
            If (components IsNot Nothing) Then
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
    Friend WithEvents Versions As System.Windows.Forms.ListBox
    Friend WithEvents cbCMSUserAccess As System.Windows.Forms.CheckBox
    Friend WithEvents cbHRAEmployeeAccess As System.Windows.Forms.CheckBox
    Friend WithEvents cbHRAAccess As System.Windows.Forms.CheckBox
    Friend WithEvents cbEligibilityAccess As System.Windows.Forms.CheckBox
    Friend WithEvents cbEligibilityEmployeeAccess As System.Windows.Forms.CheckBox
    Friend WithEvents cbLocalAccess As System.Windows.Forms.CheckBox
    Friend WithEvents cbEmployeeAccess As System.Windows.Forms.CheckBox
    Friend WithEvents cbCanUTL As System.Windows.Forms.CheckBox
    Friend WithEvents cbCanOverride As System.Windows.Forms.CheckBox
    Friend WithEvents cbReprocess As System.Windows.Forms.CheckBox
    Friend WithEvents cbCanPickWork As System.Windows.Forms.CheckBox
    Friend WithEvents cbAuditAccess As System.Windows.Forms.CheckBox
    Friend WithEvents cbLocalEmployeeAccess As System.Windows.Forms.CheckBox
    Friend WithEvents cbCanRemovePricingAccess As System.Windows.Forms.CheckBox
    Friend WithEvents cbCanReopenPartialAccess As System.Windows.Forms.CheckBox
    Friend WithEvents cbCanReopenFullAccess As System.Windows.Forms.CheckBox
    Friend WithEvents cbCanModifyCOB As System.Windows.Forms.CheckBox
    Friend WithEvents cbCanRunReports As System.Windows.Forms.CheckBox
    Friend WithEvents cbCanViewHours As System.Windows.Forms.CheckBox
    Friend WithEvents cbCanViewEligibilityHours As System.Windows.Forms.CheckBox
    Friend WithEvents cbCanModifyAlerts As System.Windows.Forms.CheckBox
    Friend WithEvents cbCanCreateClaim As System.Windows.Forms.CheckBox
    Friend WithEvents CMSDocumentTypes As System.Windows.Forms.ListBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(About))
        Me.OK = New System.Windows.Forms.Button()
        Me.Pic = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblProductVersion = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Versions = New System.Windows.Forms.ListBox()
        Me.CMSDocumentTypes = New System.Windows.Forms.ListBox()
        Me.cbCMSUserAccess = New System.Windows.Forms.CheckBox()
        Me.cbHRAEmployeeAccess = New System.Windows.Forms.CheckBox()
        Me.cbHRAAccess = New System.Windows.Forms.CheckBox()
        Me.cbEligibilityAccess = New System.Windows.Forms.CheckBox()
        Me.cbEligibilityEmployeeAccess = New System.Windows.Forms.CheckBox()
        Me.cbLocalAccess = New System.Windows.Forms.CheckBox()
        Me.cbEmployeeAccess = New System.Windows.Forms.CheckBox()
        Me.cbCanUTL = New System.Windows.Forms.CheckBox()
        Me.cbCanOverride = New System.Windows.Forms.CheckBox()
        Me.cbReprocess = New System.Windows.Forms.CheckBox()
        Me.cbCanPickWork = New System.Windows.Forms.CheckBox()
        Me.cbAuditAccess = New System.Windows.Forms.CheckBox()
        Me.cbLocalEmployeeAccess = New System.Windows.Forms.CheckBox()
        Me.cbCanRemovePricingAccess = New System.Windows.Forms.CheckBox()
        Me.cbCanReopenPartialAccess = New System.Windows.Forms.CheckBox()
        Me.cbCanReopenFullAccess = New System.Windows.Forms.CheckBox()
        Me.cbCanModifyCOB = New System.Windows.Forms.CheckBox()
        Me.cbCanRunReports = New System.Windows.Forms.CheckBox()
        Me.cbCanViewHours = New System.Windows.Forms.CheckBox()
        Me.cbCanViewEligibilityHours = New System.Windows.Forms.CheckBox()
        Me.cbCanModifyAlerts = New System.Windows.Forms.CheckBox()
        Me.cbCanCreateClaim = New System.Windows.Forms.CheckBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ImagingDocumentTypes = New System.Windows.Forms.ListBox()
        Me.lblDefaultSQLDatabase = New System.Windows.Forms.Label()
        Me.lblDefaultDB2Database = New System.Windows.Forms.Label()
        Me.lblADUser = New System.Windows.Forms.Label()
        Me.lblWorkstation = New System.Windows.Forms.Label()
        Me.lblCommandLineArgs = New System.Windows.Forms.Label()
        CType(Me.Pic, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'OK
        '
        Me.OK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OK.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.OK.Location = New System.Drawing.Point(497, 736)
        Me.OK.Name = "OK"
        Me.OK.Size = New System.Drawing.Size(75, 24)
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
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(78, 5)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(170, 35)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Claims Management System"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(80, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(16, 17)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "v"
        '
        'Version
        '
        Me.lblProductVersion.AutoSize = True
        Me.lblProductVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProductVersion.Location = New System.Drawing.Point(102, 41)
        Me.lblProductVersion.Name = "Version"
        Me.lblProductVersion.Size = New System.Drawing.Size(17, 17)
        Me.lblProductVersion.TabIndex = 5
        Me.lblProductVersion.Text = "0"
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(7, 58)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(232, 32)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "This Product Was Designed For Use By: So. Cal. UFCW Trust Fund"
        '
        'Versions
        '
        Me.Versions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Versions.HorizontalScrollbar = True
        Me.Versions.Location = New System.Drawing.Point(4, 490)
        Me.Versions.Name = "Versions"
        Me.Versions.SelectionMode = System.Windows.Forms.SelectionMode.None
        Me.Versions.Size = New System.Drawing.Size(567, 238)
        Me.Versions.Sorted = True
        Me.Versions.TabIndex = 7
        Me.ToolTip1.SetToolTip(Me.Versions, "Software Version Info")
        '
        'CMSDocumentTypes
        '
        Me.CMSDocumentTypes.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CMSDocumentTypes.HorizontalScrollbar = True
        Me.CMSDocumentTypes.Location = New System.Drawing.Point(4, 220)
        Me.CMSDocumentTypes.Name = "CMSDocumentTypes"
        Me.CMSDocumentTypes.SelectionMode = System.Windows.Forms.SelectionMode.None
        Me.CMSDocumentTypes.Size = New System.Drawing.Size(567, 134)
        Me.CMSDocumentTypes.Sorted = True
        Me.CMSDocumentTypes.TabIndex = 13
        Me.ToolTip1.SetToolTip(Me.CMSDocumentTypes, "Permitted CMS Document Types ")
        '
        'cbCMSUserAccess
        '
        Me.cbCMSUserAccess.AutoSize = True
        Me.cbCMSUserAccess.Enabled = False
        Me.cbCMSUserAccess.Location = New System.Drawing.Point(436, 5)
        Me.cbCMSUserAccess.Name = "cbCMSUserAccess"
        Me.cbCMSUserAccess.Size = New System.Drawing.Size(74, 17)
        Me.cbCMSUserAccess.TabIndex = 39
        Me.cbCMSUserAccess.Text = "CMS User"
        '
        'cbHRAEmployeeAccess
        '
        Me.cbHRAEmployeeAccess.Enabled = False
        Me.cbHRAEmployeeAccess.Location = New System.Drawing.Point(254, 95)
        Me.cbHRAEmployeeAccess.Name = "cbHRAEmployeeAccess"
        Me.cbHRAEmployeeAccess.Size = New System.Drawing.Size(141, 17)
        Me.cbHRAEmployeeAccess.TabIndex = 38
        Me.cbHRAEmployeeAccess.Text = "HRA Employee Access"
        '
        'cbHRAAccess
        '
        Me.cbHRAAccess.Enabled = False
        Me.cbHRAAccess.Location = New System.Drawing.Point(254, 77)
        Me.cbHRAAccess.Name = "cbHRAAccess"
        Me.cbHRAAccess.Size = New System.Drawing.Size(125, 17)
        Me.cbHRAAccess.TabIndex = 37
        Me.cbHRAAccess.Text = "HRA Access"
        '
        'cbEligibilityAccess
        '
        Me.cbEligibilityAccess.Enabled = False
        Me.cbEligibilityAccess.Location = New System.Drawing.Point(254, 59)
        Me.cbEligibilityAccess.Name = "cbEligibilityAccess"
        Me.cbEligibilityAccess.Size = New System.Drawing.Size(125, 17)
        Me.cbEligibilityAccess.TabIndex = 36
        Me.cbEligibilityAccess.Text = "Eligibility Access"
        '
        'cbEligibilityEmployeeAccess
        '
        Me.cbEligibilityEmployeeAccess.Enabled = False
        Me.cbEligibilityEmployeeAccess.Location = New System.Drawing.Point(254, 23)
        Me.cbEligibilityEmployeeAccess.Name = "cbEligibilityEmployeeAccess"
        Me.cbEligibilityEmployeeAccess.Size = New System.Drawing.Size(159, 17)
        Me.cbEligibilityEmployeeAccess.TabIndex = 35
        Me.cbEligibilityEmployeeAccess.Text = "Eligibility Employee Access"
        '
        'cbLocalAccess
        '
        Me.cbLocalAccess.Enabled = False
        Me.cbLocalAccess.Location = New System.Drawing.Point(254, 113)
        Me.cbLocalAccess.Name = "cbLocalAccess"
        Me.cbLocalAccess.Size = New System.Drawing.Size(131, 16)
        Me.cbLocalAccess.TabIndex = 34
        Me.cbLocalAccess.Text = "Union Locals Access"
        '
        'cbEmployeeAccess
        '
        Me.cbEmployeeAccess.Enabled = False
        Me.cbEmployeeAccess.Location = New System.Drawing.Point(254, 41)
        Me.cbEmployeeAccess.Name = "cbEmployeeAccess"
        Me.cbEmployeeAccess.Size = New System.Drawing.Size(125, 17)
        Me.cbEmployeeAccess.TabIndex = 33
        Me.cbEmployeeAccess.Text = "Employee Access"
        '
        'cbCanUTL
        '
        Me.cbCanUTL.AutoSize = True
        Me.cbCanUTL.Enabled = False
        Me.cbCanUTL.Location = New System.Drawing.Point(436, 95)
        Me.cbCanUTL.Name = "cbCanUTL"
        Me.cbCanUTL.Size = New System.Drawing.Size(47, 17)
        Me.cbCanUTL.TabIndex = 32
        Me.cbCanUTL.Text = "UTL"
        '
        'cbCanOverride
        '
        Me.cbCanOverride.AutoSize = True
        Me.cbCanOverride.Enabled = False
        Me.cbCanOverride.Location = New System.Drawing.Point(436, 41)
        Me.cbCanOverride.Name = "cbCanOverride"
        Me.cbCanOverride.Size = New System.Drawing.Size(88, 17)
        Me.cbCanOverride.TabIndex = 31
        Me.cbCanOverride.Text = "Can Override"
        '
        'cbReprocess
        '
        Me.cbReprocess.AutoSize = True
        Me.cbReprocess.Enabled = False
        Me.cbReprocess.Location = New System.Drawing.Point(436, 77)
        Me.cbReprocess.Name = "cbReprocess"
        Me.cbReprocess.Size = New System.Drawing.Size(77, 17)
        Me.cbReprocess.TabIndex = 30
        Me.cbReprocess.Text = "Reprocess"
        '
        'cbCanPickWork
        '
        Me.cbCanPickWork.AutoSize = True
        Me.cbCanPickWork.Enabled = False
        Me.cbCanPickWork.Location = New System.Drawing.Point(436, 23)
        Me.cbCanPickWork.Name = "cbCanPickWork"
        Me.cbCanPickWork.Size = New System.Drawing.Size(98, 17)
        Me.cbCanPickWork.TabIndex = 29
        Me.cbCanPickWork.Text = "Can Pick Work"
        '
        'cbAuditAccess
        '
        Me.cbAuditAccess.AutoSize = True
        Me.cbAuditAccess.Enabled = False
        Me.cbAuditAccess.Location = New System.Drawing.Point(436, 59)
        Me.cbAuditAccess.Name = "cbAuditAccess"
        Me.cbAuditAccess.Size = New System.Drawing.Size(88, 17)
        Me.cbAuditAccess.TabIndex = 28
        Me.cbAuditAccess.Text = "Audit Access"
        '
        'cbLocalEmployeeAccess
        '
        Me.cbLocalEmployeeAccess.Enabled = False
        Me.cbLocalEmployeeAccess.Location = New System.Drawing.Point(254, 5)
        Me.cbLocalEmployeeAccess.Name = "cbLocalEmployeeAccess"
        Me.cbLocalEmployeeAccess.Size = New System.Drawing.Size(190, 17)
        Me.cbLocalEmployeeAccess.TabIndex = 27
        Me.cbLocalEmployeeAccess.Text = "Union Locals Employee Access"
        '
        'cbCanRemovePricingAccess
        '
        Me.cbCanRemovePricingAccess.AutoSize = True
        Me.cbCanRemovePricingAccess.Enabled = False
        Me.cbCanRemovePricingAccess.Location = New System.Drawing.Point(436, 113)
        Me.cbCanRemovePricingAccess.Name = "cbCanRemovePricingAccess"
        Me.cbCanRemovePricingAccess.Size = New System.Drawing.Size(123, 17)
        Me.cbCanRemovePricingAccess.TabIndex = 40
        Me.cbCanRemovePricingAccess.Text = "Can Remove Pricing"
        '
        'cbCanReopenPartialAccess
        '
        Me.cbCanReopenPartialAccess.AutoSize = True
        Me.cbCanReopenPartialAccess.Enabled = False
        Me.cbCanReopenPartialAccess.Location = New System.Drawing.Point(436, 131)
        Me.cbCanReopenPartialAccess.Name = "cbCanReopenPartialAccess"
        Me.cbCanReopenPartialAccess.Size = New System.Drawing.Size(127, 17)
        Me.cbCanReopenPartialAccess.TabIndex = 42
        Me.cbCanReopenPartialAccess.Text = "Reopen Own Access"
        '
        'cbCanReopenFullAccess
        '
        Me.cbCanReopenFullAccess.Enabled = False
        Me.cbCanReopenFullAccess.Location = New System.Drawing.Point(254, 130)
        Me.cbCanReopenFullAccess.Name = "cbCanReopenFullAccess"
        Me.cbCanReopenFullAccess.Size = New System.Drawing.Size(131, 17)
        Me.cbCanReopenFullAccess.TabIndex = 41
        Me.cbCanReopenFullAccess.Text = "Reopen All Access"
        '
        'cbCanModifyCOB
        '
        Me.cbCanModifyCOB.AutoSize = True
        Me.cbCanModifyCOB.Enabled = False
        Me.cbCanModifyCOB.Location = New System.Drawing.Point(436, 149)
        Me.cbCanModifyCOB.Name = "cbCanModifyCOB"
        Me.cbCanModifyCOB.Size = New System.Drawing.Size(82, 17)
        Me.cbCanModifyCOB.TabIndex = 44
        Me.cbCanModifyCOB.Text = "Modify COB"
        '
        'cbCanRunReports
        '
        Me.cbCanRunReports.AutoSize = True
        Me.cbCanRunReports.Enabled = False
        Me.cbCanRunReports.Location = New System.Drawing.Point(254, 148)
        Me.cbCanRunReports.Name = "cbCanRunReports"
        Me.cbCanRunReports.Size = New System.Drawing.Size(86, 17)
        Me.cbCanRunReports.TabIndex = 43
        Me.cbCanRunReports.Text = "Run Reports"
        '
        'cbCanViewHours
        '
        Me.cbCanViewHours.AutoSize = True
        Me.cbCanViewHours.Enabled = False
        Me.cbCanViewHours.Location = New System.Drawing.Point(254, 167)
        Me.cbCanViewHours.Name = "cbCanViewHours"
        Me.cbCanViewHours.Size = New System.Drawing.Size(80, 17)
        Me.cbCanViewHours.TabIndex = 45
        Me.cbCanViewHours.Text = "View Hours"
        '
        'cbCanViewEligibilityHours
        '
        Me.cbCanViewEligibilityHours.AutoSize = True
        Me.cbCanViewEligibilityHours.Enabled = False
        Me.cbCanViewEligibilityHours.Location = New System.Drawing.Point(436, 167)
        Me.cbCanViewEligibilityHours.Name = "cbCanViewEligibilityHours"
        Me.cbCanViewEligibilityHours.Size = New System.Drawing.Size(91, 17)
        Me.cbCanViewEligibilityHours.TabIndex = 46
        Me.cbCanViewEligibilityHours.Text = "View Eligibility"
        Me.cbCanViewEligibilityHours.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'cbCanModifyAlerts
        '
        Me.cbCanModifyAlerts.AutoSize = True
        Me.cbCanModifyAlerts.Enabled = False
        Me.cbCanModifyAlerts.Location = New System.Drawing.Point(254, 185)
        Me.cbCanModifyAlerts.Name = "cbCanModifyAlerts"
        Me.cbCanModifyAlerts.Size = New System.Drawing.Size(94, 17)
        Me.cbCanModifyAlerts.TabIndex = 47
        Me.cbCanModifyAlerts.Text = "Manage Alerts"
        '
        'cbCanCreateClaim
        '
        Me.cbCanCreateClaim.AutoSize = True
        Me.cbCanCreateClaim.Enabled = False
        Me.cbCanCreateClaim.Location = New System.Drawing.Point(436, 185)
        Me.cbCanCreateClaim.Name = "cbCanCreateClaim"
        Me.cbCanCreateClaim.Size = New System.Drawing.Size(107, 17)
        Me.cbCanCreateClaim.TabIndex = 48
        Me.cbCanCreateClaim.Text = "Can Create Claim"
        '
        'ImagingDocumentTypes
        '
        Me.ImagingDocumentTypes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ImagingDocumentTypes.HorizontalScrollbar = True
        Me.ImagingDocumentTypes.Location = New System.Drawing.Point(4, 360)
        Me.ImagingDocumentTypes.Name = "ImagingDocumentTypes"
        Me.ImagingDocumentTypes.SelectionMode = System.Windows.Forms.SelectionMode.None
        Me.ImagingDocumentTypes.Size = New System.Drawing.Size(568, 121)
        Me.ImagingDocumentTypes.Sorted = True
        Me.ImagingDocumentTypes.TabIndex = 51
        Me.ToolTip1.SetToolTip(Me.ImagingDocumentTypes, "Permitted Imaging Document Types ")
        '
        'lblDefaultSQLDatabase
        '
        Me.lblDefaultSQLDatabase.AutoSize = True
        Me.lblDefaultSQLDatabase.Location = New System.Drawing.Point(5, 119)
        Me.lblDefaultSQLDatabase.Name = "lblDefaultSQLDatabase"
        Me.lblDefaultSQLDatabase.Size = New System.Drawing.Size(19, 13)
        Me.lblDefaultSQLDatabase.TabIndex = 64
        Me.lblDefaultSQLDatabase.Text = "    "
        '
        'lblDefaultDB2Database
        '
        Me.lblDefaultDB2Database.AutoSize = True
        Me.lblDefaultDB2Database.Location = New System.Drawing.Point(5, 145)
        Me.lblDefaultDB2Database.Name = "lblDefaultDB2Database"
        Me.lblDefaultDB2Database.Size = New System.Drawing.Size(19, 13)
        Me.lblDefaultDB2Database.TabIndex = 63
        Me.lblDefaultDB2Database.Text = "    "
        '
        'lblADUser
        '
        Me.lblADUser.AutoSize = True
        Me.lblADUser.Location = New System.Drawing.Point(5, 197)
        Me.lblADUser.Name = "lblADUser"
        Me.lblADUser.Size = New System.Drawing.Size(19, 13)
        Me.lblADUser.TabIndex = 62
        Me.lblADUser.Text = "    "
        '
        'lblWorkstation
        '
        Me.lblWorkstation.AutoSize = True
        Me.lblWorkstation.Location = New System.Drawing.Point(5, 171)
        Me.lblWorkstation.Name = "lblWorkstation"
        Me.lblWorkstation.Size = New System.Drawing.Size(19, 13)
        Me.lblWorkstation.TabIndex = 61
        Me.lblWorkstation.Text = "    "
        '
        'lblCommandLineArgs
        '
        Me.lblCommandLineArgs.AutoSize = True
        Me.lblCommandLineArgs.Location = New System.Drawing.Point(5, 93)
        Me.lblCommandLineArgs.Name = "lblCommandLineArgs"
        Me.lblCommandLineArgs.Size = New System.Drawing.Size(19, 13)
        Me.lblCommandLineArgs.TabIndex = 60
        Me.lblCommandLineArgs.Text = "    "
        '
        'About
        '
        Me.AcceptButton = Me.OK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.OK
        Me.ClientSize = New System.Drawing.Size(577, 762)
        Me.Controls.Add(Me.lblDefaultSQLDatabase)
        Me.Controls.Add(Me.lblDefaultDB2Database)
        Me.Controls.Add(Me.lblADUser)
        Me.Controls.Add(Me.lblWorkstation)
        Me.Controls.Add(Me.lblCommandLineArgs)
        Me.Controls.Add(Me.ImagingDocumentTypes)
        Me.Controls.Add(Me.cbCanCreateClaim)
        Me.Controls.Add(Me.cbCanModifyAlerts)
        Me.Controls.Add(Me.cbCanViewEligibilityHours)
        Me.Controls.Add(Me.cbCanViewHours)
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
        Me.Controls.Add(Me.cbLocalEmployeeAccess)
        Me.Controls.Add(Me.CMSDocumentTypes)
        Me.Controls.Add(Me.Versions)
        Me.Controls.Add(Me.lblProductVersion)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Pic)
        Me.Controls.Add(Me.OK)
        Me.Controls.Add(Me.Label3)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "About"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "About UFCW CMS"
        CType(Me.Pic, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub About_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not UFCWGeneral.SetFormPosition(Me) Then Me.CenterToScreen()

        lblProductVersion.Text = Application.ProductVersion

        Call CollectVersions()
        Call LoadUserDocTypes()
        Call LoadImagingDocTypes()

        lblADUser.Text = UFCWGeneral.DomainUser
        lblWorkstation.Text = UFCWGeneral.ComputerName
        lblDefaultDB2Database.Text = CMSDALCommon.DefaultDB2Database

        Try
            lblDefaultSQLDatabase.Text = CMSDALCommon.DefaultSQLDatabase
        Catch ex As Exception

        End Try

        Me.cbCMSUserAccess.Checked = UFCWGeneralAD.CMSUsers()
        Me.cbLocalEmployeeAccess.Checked = UFCWGeneralAD.CMSLocalsEmployee()
        Me.cbLocalAccess.Checked = UFCWGeneralAD.CMSLocals()
        Me.cbEmployeeAccess.Checked = UFCWGeneralAD.CMSCanAdjudicateEmployee()
        Me.cbCanPickWork.Checked = UFCWGeneralAD.CMSCanPickWork()
        Me.cbAuditAccess.Checked = UFCWGeneralAD.CMSCanAudit()
        Me.cbReprocess.Checked = UFCWGeneralAD.CMSCanReprocess()
        Me.cbCanOverride.Checked = UFCWGeneralAD.CMSCanOverrideAccumulators()
        Me.cbCanCreateClaim.Checked = UFCWGeneralAD.CMSCanCreateClaim()
        Me.cbCanUTL.Checked = UFCWGeneralAD.CMSUTL()
        Me.cbEligibilityAccess.Checked = UFCWGeneralAD.CMSEligibility()
        Me.cbEligibilityEmployeeAccess.Checked = UFCWGeneralAD.CMSEligibilityEmployee()
        Me.cbHRAAccess.Checked = UFCWGeneralAD.CMSHRA()
        Me.cbHRAEmployeeAccess.Checked = UFCWGeneralAD.CMSHRAEmployee()
        Me.cbCanRemovePricingAccess.Checked = UFCWGeneralAD.CMSCanRemovePricing()
        Me.cbCanReopenFullAccess.Checked = UFCWGeneralAD.CMSCanReopenFull()
        Me.cbCanReopenPartialAccess.Checked = UFCWGeneralAD.CMSCanReopenPartial()
        Me.cbCanRunReports.Checked = UFCWGeneralAD.CMSCanRunReports()
        Me.cbCanModifyCOB.Checked = UFCWGeneralAD.CMSCanModifyCOB()
        Me.cbCanViewHours.Checked = UFCWGeneralAD.CMSCanViewHours()
        Me.cbCanViewEligibilityHours.Checked = UFCWGeneralAD.CMSCanViewEligibilityHours()
        Me.cbCanModifyAlerts.Checked = UFCWGeneralAD.CMSCanModifyAlerts
    End Sub
    Private Sub LoadUserDocTypes()

        Dim DomainUser As String = SystemInformation.UserName

        CMSDocumentTypes.Items.Clear()
        CMSDocumentTypes.DataSource = CMSDALFDBMD.RetrieveUserDocTypes(DomainUser)
        CMSDocumentTypes.DisplayMember = "DOC_TYPE"

    End Sub

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        Me.Close()
    End Sub
    Private Sub CollectVersions()
        Dim FInfo As FileInfo = New FileInfo(Application.ExecutablePath)
        Dim dInfo As DirectoryInfo = New DirectoryInfo(FInfo.DirectoryName)

        Dim configFileArray() As FileInfo = dInfo.GetFiles("*.config")
        Dim xmlFileArray() As FileInfo = dInfo.GetFiles("*.xml")
        Dim dllFileArray() As FileInfo = dInfo.GetFiles("*.dll")
        Dim exeFileArray() As FileInfo = dInfo.GetFiles("*.exe")


        ProcessFiles(configFileArray)
        ProcessFiles(xmlFileArray)
        ProcessFiles(dllFileArray)
        ProcessFiles(exeFileArray)


        'Versions.BeginUpdate()
        'For Each fi As FileInfo In exeFileArray
        '    With System.Diagnostics.FileVersionInfo.GetVersionInfo(fi.FullName)
        '        Versions.Items.Add($"{fi.FullName} ( { .FileMajorPart}.{ .FileMinorPart}.{ .FileBuildPart}.{ .FilePrivatePart} )")
        '    End With
        'Next
        'For Each fi As FileInfo In dllFileArray
        '    With System.Diagnostics.FileVersionInfo.GetVersionInfo(fi.FullName)
        '        Versions.Items.Add($"{fi.FullName} ( { .FileMajorPart}.{ .FileMinorPart}.{ .FileBuildPart}.{ .FilePrivatePart} )")
        '    End With
        'Next
        'For Each fi As FileInfo In xmlFileArray
        '    With System.Diagnostics.FileVersionInfo.GetVersionInfo(fi.FullName)
        '        Versions.Items.Add($"{fi.FullName} ( { .FileMajorPart}.{ .FileMinorPart}.{ .FileBuildPart}.{ .FilePrivatePart} )")
        '    End With
        'Next
        'For Each fi As FileInfo In configFileArray
        '    With System.Diagnostics.FileVersionInfo.GetVersionInfo(fi.FullName)
        '        Versions.Items.Add($"{fi.FullName} ( { .FileMajorPart}.{ .FileMinorPart}.{ .FileBuildPart}.{ .FilePrivatePart} )")
        '    End With
        'Next
        'Versions.EndUpdate()

    End Sub
    Private Sub ProcessFiles(ByVal verFileArray As FileInfo())
        ' Create a list to store version information strings
        Dim versionInfoList As New List(Of String)
        Try
            Parallel.ForEach(verFileArray, Sub(fi)
                                               ' Get the file version information for the current file
                                               Dim versionInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(fi.FullName)

                                               ' Format the version information
                                               Dim versionString As String = $"{fi.FullName} ({versionInfo.FileMajorPart}.{versionInfo.FileMinorPart}.{versionInfo.FileBuildPart}.{versionInfo.FilePrivatePart})"

                                               ' Lock the list to avoid race conditions
                                               SyncLock versionInfoList
                                                   versionInfoList.Add(versionString)
                                               End SyncLock
                                           End Sub)
            ' Add all collected version information strings to the ListBox
            Versions.Items.AddRange(versionInfoList.ToArray())
        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub LoadImagingDocTypes()
        Dim DocSecurityDT As DataTable = CMSDALDBO.RetrieveImagingDocTypesByUserID("ImgWorkflow", _DomainUser)

        For Each DR As DataRow In DocSecurityDT.Rows
            ImagingDocumentTypes.Items.Add(DR("DocumentClass").ToString & vbTab & " -> " & DR("DocType").ToString)
        Next

    End Sub

End Class