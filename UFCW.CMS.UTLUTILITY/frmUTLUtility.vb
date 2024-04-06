Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Configuration
Imports System.Security.Principal
Imports UFCW.WCF
Imports System.Data.Common
Imports SharedInterfaces
<PlugIn("UTL", "Queue")> Public Class UTLUtility
    Inherits System.Windows.Forms.Form

    Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    ReadOnly _DomainUser As String = SystemInformation.UserName
    Private _APPKEY As String = "UFCW\Claims\"

    Private Structure PatientKey
        Dim FamilyID As Integer
        Dim ParticipantSSN As Integer
        Dim RelationID As Integer
        Dim PatientSSN As Integer
        Dim PatientFName As Object
        Dim PatientLName As Object
        Dim SecuritySW As Boolean

        Sub Empty()
            FamilyID = Nothing
            ParticipantSSN = Nothing
            RelationID = Nothing
            PatientSSN = Nothing
            PatientFName = Nothing
            PatientLName = Nothing
            SecuritySW = Nothing
        End Sub
    End Structure

    <System.ComponentModel.Description("Represents the application key used when accessing the registry.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
        End Set
    End Property

    Private _SearchResultsDT As DataTable
    Private _Message As IMessage
    Private _UniqueID As String
    Private _FNDisplay As Display
    Private _ClaimMasterDR As ClaimDataset.CLAIM_MASTERRow
    Private _MedHDRDR As ClaimDataset.MEDHDRRow
    Private _UserPrincipal As WindowsPrincipal
    Private _ClaimDS As New ClaimDataset
    Private _ProviderDS As New ProvDataSet
    Private _OrigPartSSN As Object
    Private _AnnotationsDS As New AnnotationsDataSet
    Private _AnnotationAdded As Boolean = False
    Private _Saved As Boolean = False
    Private WithEvents _ClaimMasterBS As BindingSource
    Private WithEvents _MedHdrBS As BindingSource

    Friend WithEvents EnhancedToolTip As EnhancedToolTip
    Friend WithEvents StepTab As System.Windows.Forms.TabControl
    Friend WithEvents ParticipantTabPage As System.Windows.Forms.TabPage
    Friend WithEvents ClaimPanel As System.Windows.Forms.Panel
    Friend WithEvents gpbProviderInformation As System.Windows.Forms.GroupBox
    Friend WithEvents ProviderRenderingNPITextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents ProviderIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents ProvLookupButton As System.Windows.Forms.Button
    Friend WithEvents ProviderTINTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents ProviderLicenseNoTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents BCCZIPTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label50 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents FamilyIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label84 As System.Windows.Forms.Label
    Friend WithEvents PartSSNTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label85 As System.Windows.Forms.Label
    Friend WithEvents PartNameFirstTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PartNameMiddleTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PartNameLastTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PartLookupButton As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents PatLookupButton As System.Windows.Forms.Button
    Friend WithEvents PatNameMiddleTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PatNameLastTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PatNameFirstTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PatRelationIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents PatAcctNoTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label53 As System.Windows.Forms.Label
    Friend WithEvents PatSSNTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents PatSexTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PatDOBTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ClaimHistoryButton As System.Windows.Forms.Button
    Friend WithEvents DocumentTabPage As System.Windows.Forms.TabPage
    Friend WithEvents QueuePanel As System.Windows.Forms.Panel
    Friend WithEvents gpbDocumentInformation As System.Windows.Forms.GroupBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents ClaimIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents EmployeeItemLabel As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents PriorityTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PageCountTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents MaxIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents DocTypeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents ReferenceClaimTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents OpenDateTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents ReceivedDateTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents BatchNumTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents DocIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ReferenceIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents AnnotateTabPage As System.Windows.Forms.TabPage
    Friend WithEvents ArchiveButton As System.Windows.Forms.Button
    Friend WithEvents AnnotationsControl As AnnotationsControl
    Friend WithEvents RouteButton As System.Windows.Forms.Button
    Private PricingExcludeByDocType As String() = ConfigurationManager.AppSettings("PricingExcludeByDocType").Split(CChar(","))

#Region " Windows Form Designer generated code "
    Public Sub New(ByVal objMsg As IMessage, ByVal claimID As Integer, ByVal mode As String, Optional ByVal transaction As DbTransaction = Nothing)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        Try

            _Message = objMsg

            LoadDocTypeComboBox()

            GetClaim(claimID)

            Using FNDisplay As New Display
                FNDisplay.Display(_ClaimMasterDR("DOCID"))
            End Using

        Catch ex As ApplicationException

            MessageBox.Show(ex.Message, "Application Issue requires attention.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            Else
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try

    End Sub

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
    Friend WithEvents PreviousPanelButton As System.Windows.Forms.Button
    Friend WithEvents FinishWizardButton As System.Windows.Forms.Button
    Friend WithEvents CancelWizard As System.Windows.Forms.Button
    Friend WithEvents NextPanelButton As System.Windows.Forms.Button
    Friend WithEvents ClassGrid As DataGridCustom
    Friend WithEvents ImageWarning As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim Resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UTLUtility))
        Me.PreviousPanelButton = New System.Windows.Forms.Button()
        Me.FinishWizardButton = New System.Windows.Forms.Button()
        Me.CancelWizard = New System.Windows.Forms.Button()
        Me.NextPanelButton = New System.Windows.Forms.Button()
        Me.ImageWarning = New System.Windows.Forms.Label()
        Me.ProviderRenderingNPITextBox = New System.Windows.Forms.TextBox()
        Me.ProviderTINTextBox = New System.Windows.Forms.TextBox()
        Me.ProviderLicenseNoTextBox = New System.Windows.Forms.TextBox()
        Me.PartSSNTextBox = New System.Windows.Forms.TextBox()
        Me.PatAcctNoTextBox = New System.Windows.Forms.TextBox()
        Me.PatSSNTextBox = New System.Windows.Forms.TextBox()
        Me.StepTab = New System.Windows.Forms.TabControl()
        Me.ParticipantTabPage = New System.Windows.Forms.TabPage()
        Me.ClaimPanel = New System.Windows.Forms.Panel()
        Me.gpbProviderInformation = New System.Windows.Forms.GroupBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.ProviderIDTextBox = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.ProvLookupButton = New System.Windows.Forms.Button()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.BCCZIPTextBox = New System.Windows.Forms.TextBox()
        Me.Label50 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.FamilyIDTextBox = New System.Windows.Forms.TextBox()
        Me.Label84 = New System.Windows.Forms.Label()
        Me.Label85 = New System.Windows.Forms.Label()
        Me.PartNameFirstTextBox = New System.Windows.Forms.TextBox()
        Me.PartNameMiddleTextBox = New System.Windows.Forms.TextBox()
        Me.PartNameLastTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PartLookupButton = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.PatLookupButton = New System.Windows.Forms.Button()
        Me.PatNameMiddleTextBox = New System.Windows.Forms.TextBox()
        Me.PatNameLastTextBox = New System.Windows.Forms.TextBox()
        Me.PatNameFirstTextBox = New System.Windows.Forms.TextBox()
        Me.PatRelationIDTextBox = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label53 = New System.Windows.Forms.Label()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.PatSexTextBox = New System.Windows.Forms.TextBox()
        Me.PatDOBTextBox = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ClaimHistoryButton = New System.Windows.Forms.Button()
        Me.DocumentTabPage = New System.Windows.Forms.TabPage()
        Me.QueuePanel = New System.Windows.Forms.Panel()
        Me.gpbDocumentInformation = New System.Windows.Forms.GroupBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.ClaimIDTextBox = New System.Windows.Forms.TextBox()
        Me.EmployeeItemLabel = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.PriorityTextBox = New System.Windows.Forms.TextBox()
        Me.PageCountTextBox = New System.Windows.Forms.TextBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.MaxIDTextBox = New System.Windows.Forms.TextBox()
        Me.DocTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.ReferenceClaimTextBox = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.OpenDateTextBox = New System.Windows.Forms.TextBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.ReceivedDateTextBox = New System.Windows.Forms.TextBox()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.BatchNumTextBox = New System.Windows.Forms.TextBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.DocIDTextBox = New System.Windows.Forms.TextBox()
        Me.ReferenceIDTextBox = New System.Windows.Forms.TextBox()
        Me.AnnotateTabPage = New System.Windows.Forms.TabPage()
        Me.ArchiveButton = New System.Windows.Forms.Button()
        Me.AnnotationsControl = New AnnotationsControl()
        Me.RouteButton = New System.Windows.Forms.Button()
        Me.EnhancedToolTip = New EnhancedToolTip(Me.components)
        Me.StepTab.SuspendLayout()
        Me.ParticipantTabPage.SuspendLayout()
        Me.ClaimPanel.SuspendLayout()
        Me.gpbProviderInformation.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.DocumentTabPage.SuspendLayout()
        Me.QueuePanel.SuspendLayout()
        Me.gpbDocumentInformation.SuspendLayout()
        Me.AnnotateTabPage.SuspendLayout()
        Me.SuspendLayout()
        '
        'PreviousPanelButton
        '
        Me.PreviousPanelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PreviousPanelButton.Location = New System.Drawing.Point(213, 367)
        Me.PreviousPanelButton.Name = "PreviousPanelButton"
        Me.PreviousPanelButton.Size = New System.Drawing.Size(75, 23)
        Me.PreviousPanelButton.TabIndex = 1
        Me.PreviousPanelButton.Text = "&Previous"
        Me.EnhancedToolTip.SetToolTip(Me.PreviousPanelButton, "Go back to Previous Tab")
        Me.EnhancedToolTip.SetToolTipWhenDisabled(Me.PreviousPanelButton, "No Prior Panels")
        '
        'FinishWizardButton
        '
        Me.FinishWizardButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.FinishWizardButton.Enabled = False
        Me.FinishWizardButton.Location = New System.Drawing.Point(399, 367)
        Me.FinishWizardButton.Name = "FinishWizardButton"
        Me.FinishWizardButton.Size = New System.Drawing.Size(75, 23)
        Me.FinishWizardButton.TabIndex = 2
        Me.FinishWizardButton.Text = "&Finish"
        '
        'CancelWizard
        '
        Me.CancelWizard.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CancelWizard.Location = New System.Drawing.Point(120, 367)
        Me.CancelWizard.Name = "CancelWizard"
        Me.CancelWizard.Size = New System.Drawing.Size(75, 23)
        Me.CancelWizard.TabIndex = 3
        Me.CancelWizard.Text = "&Cancel"
        '
        'NextPanelButton
        '
        Me.NextPanelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.NextPanelButton.Location = New System.Drawing.Point(306, 367)
        Me.NextPanelButton.Name = "NextPanelButton"
        Me.NextPanelButton.Size = New System.Drawing.Size(75, 23)
        Me.NextPanelButton.TabIndex = 4
        Me.NextPanelButton.Text = "&Next"
        Me.EnhancedToolTip.SetToolTip(Me.NextPanelButton, "Proceed to Next Tab")
        Me.EnhancedToolTip.SetToolTipWhenDisabled(Me.NextPanelButton, "No additional Panels")
        '
        'ImageWarning
        '
        Me.ImageWarning.AutoSize = True
        Me.ImageWarning.BackColor = System.Drawing.Color.Transparent
        Me.ImageWarning.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ImageWarning.Font = New System.Drawing.Font("Gill Sans MT", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ImageWarning.ForeColor = System.Drawing.Color.Red
        Me.ImageWarning.Location = New System.Drawing.Point(209, -2)
        Me.ImageWarning.Name = "ImageWarning"
        Me.ImageWarning.Size = New System.Drawing.Size(181, 21)
        Me.ImageWarning.TabIndex = 95
        Me.ImageWarning.Text = "ARCHIVED DOCUMENT"
        Me.ImageWarning.Visible = False
        '
        'ProviderRenderingNPITextBox
        '
        Me.ProviderRenderingNPITextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ProviderRenderingNPITextBox.Location = New System.Drawing.Point(286, 40)
        Me.ProviderRenderingNPITextBox.MaxLength = 10
        Me.ProviderRenderingNPITextBox.Name = "ProviderRenderingNPITextBox"
        Me.ProviderRenderingNPITextBox.Size = New System.Drawing.Size(80, 20)
        Me.ProviderRenderingNPITextBox.TabIndex = 3
        Me.EnhancedToolTip.SetToolTip(Me.ProviderRenderingNPITextBox, "NPI of physician/office where services were rendered.")
        '
        'ProviderTINTextBox
        '
        Me.ProviderTINTextBox.Location = New System.Drawing.Point(112, 14)
        Me.ProviderTINTextBox.MaxLength = 10
        Me.ProviderTINTextBox.Name = "ProviderTINTextBox"
        Me.ProviderTINTextBox.Size = New System.Drawing.Size(80, 20)
        Me.ProviderTINTextBox.TabIndex = 51
        Me.EnhancedToolTip.SetToolTip(Me.ProviderTINTextBox, "Provider TaxID (99-9999999, 999-99-9999)")
        '
        'ProviderLicenseNoTextBox
        '
        Me.ProviderLicenseNoTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ProviderLicenseNoTextBox.Location = New System.Drawing.Point(112, 40)
        Me.ProviderLicenseNoTextBox.MaxLength = 11
        Me.ProviderLicenseNoTextBox.Name = "ProviderLicenseNoTextBox"
        Me.ProviderLicenseNoTextBox.Size = New System.Drawing.Size(80, 20)
        Me.ProviderLicenseNoTextBox.TabIndex = 2
        Me.EnhancedToolTip.SetToolTip(Me.ProviderLicenseNoTextBox, "Primary license of rendering physician")
        '
        'PartSSNTextBox
        '
        Me.PartSSNTextBox.Location = New System.Drawing.Point(248, 19)
        Me.PartSSNTextBox.MaxLength = 11
        Me.PartSSNTextBox.Name = "PartSSNTextBox"
        Me.PartSSNTextBox.Size = New System.Drawing.Size(68, 20)
        Me.PartSSNTextBox.TabIndex = 2
        Me.EnhancedToolTip.SetToolTip(Me.PartSSNTextBox, "The Participant's SSN")
        '
        'PatAcctNoTextBox
        '
        Me.PatAcctNoTextBox.Location = New System.Drawing.Point(392, 72)
        Me.PatAcctNoTextBox.MaxLength = 11
        Me.PatAcctNoTextBox.Name = "PatAcctNoTextBox"
        Me.PatAcctNoTextBox.Size = New System.Drawing.Size(96, 20)
        Me.PatAcctNoTextBox.TabIndex = 17
        Me.EnhancedToolTip.SetToolTip(Me.PatAcctNoTextBox, "Patient Account# assigned by Provider")
        '
        'PatSSNTextBox
        '
        Me.PatSSNTextBox.Location = New System.Drawing.Point(248, 16)
        Me.PatSSNTextBox.MaxLength = 11
        Me.PatSSNTextBox.Name = "PatSSNTextBox"
        Me.PatSSNTextBox.Size = New System.Drawing.Size(68, 20)
        Me.PatSSNTextBox.TabIndex = 9
        Me.EnhancedToolTip.SetToolTip(Me.PatSSNTextBox, "The Patient's SSN")
        '
        'StepTab
        '
        Me.StepTab.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.StepTab.Controls.Add(Me.ParticipantTabPage)
        Me.StepTab.Controls.Add(Me.DocumentTabPage)
        Me.StepTab.Controls.Add(Me.AnnotateTabPage)
        Me.StepTab.Location = New System.Drawing.Point(13, 13)
        Me.StepTab.Name = "StepTab"
        Me.StepTab.SelectedIndex = 0
        Me.StepTab.Size = New System.Drawing.Size(579, 348)
        Me.StepTab.TabIndex = 96
        '
        'ParticipantTabPage
        '
        Me.ParticipantTabPage.Controls.Add(Me.ClaimPanel)
        Me.ParticipantTabPage.Location = New System.Drawing.Point(4, 22)
        Me.ParticipantTabPage.Name = "ParticipantTabPage"
        Me.ParticipantTabPage.Padding = New System.Windows.Forms.Padding(3)
        Me.ParticipantTabPage.Size = New System.Drawing.Size(571, 322)
        Me.ParticipantTabPage.TabIndex = 0
        Me.ParticipantTabPage.Text = "Participant"
        Me.ParticipantTabPage.UseVisualStyleBackColor = True
        '
        'ClaimPanel
        '
        Me.ClaimPanel.Controls.Add(Me.gpbProviderInformation)
        Me.ClaimPanel.Controls.Add(Me.GroupBox2)
        Me.ClaimPanel.Controls.Add(Me.GroupBox1)
        Me.ClaimPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ClaimPanel.Location = New System.Drawing.Point(3, 3)
        Me.ClaimPanel.Name = "ClaimPanel"
        Me.ClaimPanel.Size = New System.Drawing.Size(565, 316)
        Me.ClaimPanel.TabIndex = 7
        '
        'gpbProviderInformation
        '
        Me.gpbProviderInformation.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gpbProviderInformation.Controls.Add(Me.ProviderRenderingNPITextBox)
        Me.gpbProviderInformation.Controls.Add(Me.Label19)
        Me.gpbProviderInformation.Controls.Add(Me.ProviderIDTextBox)
        Me.gpbProviderInformation.Controls.Add(Me.Label18)
        Me.gpbProviderInformation.Controls.Add(Me.ProvLookupButton)
        Me.gpbProviderInformation.Controls.Add(Me.ProviderTINTextBox)
        Me.gpbProviderInformation.Controls.Add(Me.Label17)
        Me.gpbProviderInformation.Controls.Add(Me.ProviderLicenseNoTextBox)
        Me.gpbProviderInformation.Controls.Add(Me.Label15)
        Me.gpbProviderInformation.Controls.Add(Me.BCCZIPTextBox)
        Me.gpbProviderInformation.Controls.Add(Me.Label50)
        Me.gpbProviderInformation.Location = New System.Drawing.Point(8, 200)
        Me.gpbProviderInformation.Name = "gpbProviderInformation"
        Me.gpbProviderInformation.Size = New System.Drawing.Size(549, 76)
        Me.gpbProviderInformation.TabIndex = 0
        Me.gpbProviderInformation.TabStop = False
        Me.gpbProviderInformation.Text = "Provider Information"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(203, 43)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(77, 13)
        Me.Label19.TabIndex = 60
        Me.Label19.Text = "Rendering NPI"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProviderIDTextBox
        '
        Me.ProviderIDTextBox.Location = New System.Drawing.Point(344, 16)
        Me.ProviderIDTextBox.MaxLength = 10
        Me.ProviderIDTextBox.Name = "ProviderIDTextBox"
        Me.ProviderIDTextBox.ReadOnly = True
        Me.ProviderIDTextBox.Size = New System.Drawing.Size(112, 20)
        Me.ProviderIDTextBox.TabIndex = 1
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(248, 16)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(60, 13)
        Me.Label18.TabIndex = 57
        Me.Label18.Text = "Provider ID"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProvLookupButton
        '
        Me.ProvLookupButton.Location = New System.Drawing.Point(196, 14)
        Me.ProvLookupButton.Name = "ProvLookupButton"
        Me.ProvLookupButton.Size = New System.Drawing.Size(27, 20)
        Me.ProvLookupButton.TabIndex = 0
        Me.ProvLookupButton.TabStop = False
        Me.ProvLookupButton.Text = "?"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(381, 43)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(64, 13)
        Me.Label17.TabIndex = 53
        Me.Label17.Text = "Provider Zip"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(18, 17)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(81, 13)
        Me.Label15.TabIndex = 50
        Me.Label15.Text = "Provider Tax ID"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'BCCZIPTextBox
        '
        Me.BCCZIPTextBox.Location = New System.Drawing.Point(450, 40)
        Me.BCCZIPTextBox.MaxLength = 5
        Me.BCCZIPTextBox.Name = "BCCZIPTextBox"
        Me.BCCZIPTextBox.Size = New System.Drawing.Size(68, 20)
        Me.BCCZIPTextBox.TabIndex = 4
        '
        'Label50
        '
        Me.Label50.AutoSize = True
        Me.Label50.Location = New System.Drawing.Point(17, 43)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(86, 13)
        Me.Label50.TabIndex = 56
        Me.Label50.Text = "Provider License"
        Me.Label50.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.FamilyIDTextBox)
        Me.GroupBox2.Controls.Add(Me.Label84)
        Me.GroupBox2.Controls.Add(Me.PartSSNTextBox)
        Me.GroupBox2.Controls.Add(Me.Label85)
        Me.GroupBox2.Controls.Add(Me.PartNameFirstTextBox)
        Me.GroupBox2.Controls.Add(Me.PartNameMiddleTextBox)
        Me.GroupBox2.Controls.Add(Me.PartNameLastTextBox)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.PartLookupButton)
        Me.GroupBox2.Location = New System.Drawing.Point(8, 16)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(549, 72)
        Me.GroupBox2.TabIndex = 48
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Participant Information"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label5.Location = New System.Drawing.Point(48, 21)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(50, 13)
        Me.Label5.TabIndex = 27
        Me.Label5.Text = "Family ID"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label6.Location = New System.Drawing.Point(200, 21)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(29, 13)
        Me.Label6.TabIndex = 26
        Me.Label6.Text = "SSN"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(8, 45)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(88, 13)
        Me.Label7.TabIndex = 25
        Me.Label7.Text = "Participant Name"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FamilyIDTextBox
        '
        Me.FamilyIDTextBox.Location = New System.Drawing.Point(104, 19)
        Me.FamilyIDTextBox.MaxLength = 11
        Me.FamilyIDTextBox.Name = "FamilyIDTextBox"
        Me.FamilyIDTextBox.ReadOnly = True
        Me.FamilyIDTextBox.Size = New System.Drawing.Size(80, 20)
        Me.FamilyIDTextBox.TabIndex = 1
        '
        'Label84
        '
        Me.Label84.AutoSize = True
        Me.Label84.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label84.Location = New System.Drawing.Point(4, 20)
        Me.Label84.Name = "Label84"
        Me.Label84.Size = New System.Drawing.Size(0, 13)
        Me.Label84.TabIndex = 24
        '
        'Label85
        '
        Me.Label85.AutoSize = True
        Me.Label85.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label85.Location = New System.Drawing.Point(160, 16)
        Me.Label85.Name = "Label85"
        Me.Label85.Size = New System.Drawing.Size(0, 13)
        Me.Label85.TabIndex = 21
        '
        'PartNameFirstTextBox
        '
        Me.PartNameFirstTextBox.Location = New System.Drawing.Point(104, 43)
        Me.PartNameFirstTextBox.MaxLength = 40
        Me.PartNameFirstTextBox.Name = "PartNameFirstTextBox"
        Me.PartNameFirstTextBox.ReadOnly = True
        Me.PartNameFirstTextBox.Size = New System.Drawing.Size(152, 20)
        Me.PartNameFirstTextBox.TabIndex = 3
        '
        'PartNameMiddleTextBox
        '
        Me.PartNameMiddleTextBox.Location = New System.Drawing.Point(264, 43)
        Me.PartNameMiddleTextBox.MaxLength = 1
        Me.PartNameMiddleTextBox.Name = "PartNameMiddleTextBox"
        Me.PartNameMiddleTextBox.ReadOnly = True
        Me.PartNameMiddleTextBox.Size = New System.Drawing.Size(16, 20)
        Me.PartNameMiddleTextBox.TabIndex = 4
        '
        'PartNameLastTextBox
        '
        Me.PartNameLastTextBox.Location = New System.Drawing.Point(288, 43)
        Me.PartNameLastTextBox.MaxLength = 40
        Me.PartNameLastTextBox.Name = "PartNameLastTextBox"
        Me.PartNameLastTextBox.ReadOnly = True
        Me.PartNameLastTextBox.Size = New System.Drawing.Size(196, 20)
        Me.PartNameLastTextBox.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(4, 48)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(0, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PartLookupButton
        '
        Me.PartLookupButton.Location = New System.Drawing.Point(488, 42)
        Me.PartLookupButton.Name = "PartLookupButton"
        Me.PartLookupButton.Size = New System.Drawing.Size(27, 20)
        Me.PartLookupButton.TabIndex = 6
        Me.PartLookupButton.TabStop = False
        Me.PartLookupButton.Text = "?"
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.Label14)
        Me.GroupBox1.Controls.Add(Me.PatLookupButton)
        Me.GroupBox1.Controls.Add(Me.PatNameMiddleTextBox)
        Me.GroupBox1.Controls.Add(Me.PatNameLastTextBox)
        Me.GroupBox1.Controls.Add(Me.PatNameFirstTextBox)
        Me.GroupBox1.Controls.Add(Me.PatRelationIDTextBox)
        Me.GroupBox1.Controls.Add(Me.Label16)
        Me.GroupBox1.Controls.Add(Me.PatAcctNoTextBox)
        Me.GroupBox1.Controls.Add(Me.Label53)
        Me.GroupBox1.Controls.Add(Me.PatSSNTextBox)
        Me.GroupBox1.Controls.Add(Me.Label39)
        Me.GroupBox1.Controls.Add(Me.PatSexTextBox)
        Me.GroupBox1.Controls.Add(Me.PatDOBTextBox)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.ClaimHistoryButton)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 96)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(549, 104)
        Me.GroupBox1.TabIndex = 49
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Patient Information"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label9.Location = New System.Drawing.Point(32, 18)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(60, 13)
        Me.Label9.TabIndex = 23
        Me.Label9.Text = "Relation ID"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(320, 74)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(65, 13)
        Me.Label10.TabIndex = 22
        Me.Label10.Text = "Patient Acct"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label11.Location = New System.Drawing.Point(200, 18)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(29, 13)
        Me.Label11.TabIndex = 21
        Me.Label11.Text = "SSN"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(184, 74)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(61, 13)
        Me.Label12.TabIndex = 20
        Me.Label12.Text = "Patient Sex"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(32, 74)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(66, 13)
        Me.Label13.TabIndex = 19
        Me.Label13.Text = "Patient DOB"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(24, 46)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(71, 13)
        Me.Label14.TabIndex = 18
        Me.Label14.Text = "Patient Name"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PatLookupButton
        '
        Me.PatLookupButton.Location = New System.Drawing.Point(488, 43)
        Me.PatLookupButton.Name = "PatLookupButton"
        Me.PatLookupButton.Size = New System.Drawing.Size(27, 20)
        Me.PatLookupButton.TabIndex = 14
        Me.PatLookupButton.TabStop = False
        Me.PatLookupButton.Text = "?"
        '
        'PatNameMiddleTextBox
        '
        Me.PatNameMiddleTextBox.Location = New System.Drawing.Point(264, 44)
        Me.PatNameMiddleTextBox.MaxLength = 1
        Me.PatNameMiddleTextBox.Name = "PatNameMiddleTextBox"
        Me.PatNameMiddleTextBox.ReadOnly = True
        Me.PatNameMiddleTextBox.Size = New System.Drawing.Size(16, 20)
        Me.PatNameMiddleTextBox.TabIndex = 12
        '
        'PatNameLastTextBox
        '
        Me.PatNameLastTextBox.Location = New System.Drawing.Point(288, 44)
        Me.PatNameLastTextBox.MaxLength = 40
        Me.PatNameLastTextBox.Name = "PatNameLastTextBox"
        Me.PatNameLastTextBox.ReadOnly = True
        Me.PatNameLastTextBox.Size = New System.Drawing.Size(196, 20)
        Me.PatNameLastTextBox.TabIndex = 13
        '
        'PatNameFirstTextBox
        '
        Me.PatNameFirstTextBox.Location = New System.Drawing.Point(104, 44)
        Me.PatNameFirstTextBox.MaxLength = 40
        Me.PatNameFirstTextBox.Name = "PatNameFirstTextBox"
        Me.PatNameFirstTextBox.ReadOnly = True
        Me.PatNameFirstTextBox.Size = New System.Drawing.Size(156, 20)
        Me.PatNameFirstTextBox.TabIndex = 11
        '
        'PatRelationIDTextBox
        '
        Me.PatRelationIDTextBox.Location = New System.Drawing.Point(104, 16)
        Me.PatRelationIDTextBox.MaxLength = 11
        Me.PatRelationIDTextBox.Name = "PatRelationIDTextBox"
        Me.PatRelationIDTextBox.ReadOnly = True
        Me.PatRelationIDTextBox.Size = New System.Drawing.Size(44, 20)
        Me.PatRelationIDTextBox.TabIndex = 8
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label16.Location = New System.Drawing.Point(4, 24)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(0, 13)
        Me.Label16.TabIndex = 15
        '
        'Label53
        '
        Me.Label53.AutoSize = True
        Me.Label53.Location = New System.Drawing.Point(280, 74)
        Me.Label53.Name = "Label53"
        Me.Label53.Size = New System.Drawing.Size(0, 13)
        Me.Label53.TabIndex = 12
        Me.Label53.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label39.Location = New System.Drawing.Point(128, 18)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(0, 13)
        Me.Label39.TabIndex = 8
        '
        'PatSexTextBox
        '
        Me.PatSexTextBox.BackColor = System.Drawing.SystemColors.Control
        Me.PatSexTextBox.Location = New System.Drawing.Point(256, 72)
        Me.PatSexTextBox.MaxLength = 6
        Me.PatSexTextBox.Name = "PatSexTextBox"
        Me.PatSexTextBox.ReadOnly = True
        Me.PatSexTextBox.Size = New System.Drawing.Size(48, 20)
        Me.PatSexTextBox.TabIndex = 16
        '
        'PatDOBTextBox
        '
        Me.PatDOBTextBox.BackColor = System.Drawing.SystemColors.Control
        Me.PatDOBTextBox.Location = New System.Drawing.Point(104, 72)
        Me.PatDOBTextBox.MaxLength = 10
        Me.PatDOBTextBox.Name = "PatDOBTextBox"
        Me.PatDOBTextBox.ReadOnly = True
        Me.PatDOBTextBox.Size = New System.Drawing.Size(68, 20)
        Me.PatDOBTextBox.TabIndex = 15
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(152, 74)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(0, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(4, 80)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(0, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(4, 52)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(0, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ClaimHistoryButton
        '
        Me.ClaimHistoryButton.Enabled = False
        Me.ClaimHistoryButton.Location = New System.Drawing.Point(336, 15)
        Me.ClaimHistoryButton.Name = "ClaimHistoryButton"
        Me.ClaimHistoryButton.Size = New System.Drawing.Size(84, 23)
        Me.ClaimHistoryButton.TabIndex = 10
        Me.ClaimHistoryButton.Text = "Claim History"
        Me.ClaimHistoryButton.Visible = False
        '
        'DocumentTabPage
        '
        Me.DocumentTabPage.Controls.Add(Me.QueuePanel)
        Me.DocumentTabPage.Location = New System.Drawing.Point(4, 22)
        Me.DocumentTabPage.Name = "DocumentTabPage"
        Me.DocumentTabPage.Padding = New System.Windows.Forms.Padding(3)
        Me.DocumentTabPage.Size = New System.Drawing.Size(571, 322)
        Me.DocumentTabPage.TabIndex = 1
        Me.DocumentTabPage.Text = "Document"
        Me.DocumentTabPage.UseVisualStyleBackColor = True
        '
        'QueuePanel
        '
        Me.QueuePanel.Controls.Add(Me.gpbDocumentInformation)
        Me.QueuePanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.QueuePanel.Location = New System.Drawing.Point(3, 3)
        Me.QueuePanel.Name = "QueuePanel"
        Me.QueuePanel.Size = New System.Drawing.Size(565, 316)
        Me.QueuePanel.TabIndex = 7
        '
        'gpbDocumentInformation
        '
        Me.gpbDocumentInformation.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gpbDocumentInformation.Controls.Add(Me.Label20)
        Me.gpbDocumentInformation.Controls.Add(Me.Label25)
        Me.gpbDocumentInformation.Controls.Add(Me.btnSave)
        Me.gpbDocumentInformation.Controls.Add(Me.Label32)
        Me.gpbDocumentInformation.Controls.Add(Me.ClaimIDTextBox)
        Me.gpbDocumentInformation.Controls.Add(Me.EmployeeItemLabel)
        Me.gpbDocumentInformation.Controls.Add(Me.Label29)
        Me.gpbDocumentInformation.Controls.Add(Me.PriorityTextBox)
        Me.gpbDocumentInformation.Controls.Add(Me.PageCountTextBox)
        Me.gpbDocumentInformation.Controls.Add(Me.Label24)
        Me.gpbDocumentInformation.Controls.Add(Me.MaxIDTextBox)
        Me.gpbDocumentInformation.Controls.Add(Me.DocTypeComboBox)
        Me.gpbDocumentInformation.Controls.Add(Me.Label23)
        Me.gpbDocumentInformation.Controls.Add(Me.ReferenceClaimTextBox)
        Me.gpbDocumentInformation.Controls.Add(Me.Label8)
        Me.gpbDocumentInformation.Controls.Add(Me.Label22)
        Me.gpbDocumentInformation.Controls.Add(Me.OpenDateTextBox)
        Me.gpbDocumentInformation.Controls.Add(Me.Label21)
        Me.gpbDocumentInformation.Controls.Add(Me.ReceivedDateTextBox)
        Me.gpbDocumentInformation.Controls.Add(Me.Label28)
        Me.gpbDocumentInformation.Controls.Add(Me.BatchNumTextBox)
        Me.gpbDocumentInformation.Controls.Add(Me.Label26)
        Me.gpbDocumentInformation.Controls.Add(Me.DocIDTextBox)
        Me.gpbDocumentInformation.Controls.Add(Me.ReferenceIDTextBox)
        Me.gpbDocumentInformation.Location = New System.Drawing.Point(16, 8)
        Me.gpbDocumentInformation.Name = "gpbDocumentInformation"
        Me.gpbDocumentInformation.Size = New System.Drawing.Size(509, 300)
        Me.gpbDocumentInformation.TabIndex = 118
        Me.gpbDocumentInformation.TabStop = False
        Me.gpbDocumentInformation.Text = "Document Information"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(12, 279)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(74, 13)
        Me.Label20.TabIndex = 119
        Me.Label20.Text = "Reference ID:"
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(34, 178)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(66, 13)
        Me.Label25.TabIndex = 106
        Me.Label25.Text = "Page Count:"
        '
        'btnSave
        '
        Me.btnSave.Enabled = False
        Me.btnSave.Location = New System.Drawing.Point(428, 21)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 21)
        Me.btnSave.TabIndex = 117
        Me.btnSave.Text = "&Save"
        Me.btnSave.Visible = False
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Location = New System.Drawing.Point(51, 103)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(49, 13)
        Me.Label32.TabIndex = 116
        Me.Label32.Text = "Claim ID:"
        '
        'ClaimIDTextBox
        '
        Me.ClaimIDTextBox.Location = New System.Drawing.Point(112, 100)
        Me.ClaimIDTextBox.Name = "ClaimIDTextBox"
        Me.ClaimIDTextBox.ReadOnly = True
        Me.ClaimIDTextBox.Size = New System.Drawing.Size(144, 20)
        Me.ClaimIDTextBox.TabIndex = 104
        Me.ClaimIDTextBox.TabStop = False
        '
        'EmployeeItemLabel
        '
        Me.EmployeeItemLabel.AutoSize = True
        Me.EmployeeItemLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EmployeeItemLabel.Location = New System.Drawing.Point(364, 109)
        Me.EmployeeItemLabel.Name = "EmployeeItemLabel"
        Me.EmployeeItemLabel.Size = New System.Drawing.Size(119, 13)
        Me.EmployeeItemLabel.TabIndex = 115
        Me.EmployeeItemLabel.Text = "***Employee Item***"
        Me.EmployeeItemLabel.Visible = False
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Location = New System.Drawing.Point(59, 228)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(41, 13)
        Me.Label29.TabIndex = 114
        Me.Label29.Text = "Priority:"
        '
        'PriorityTextBox
        '
        Me.PriorityTextBox.Location = New System.Drawing.Point(112, 225)
        Me.PriorityTextBox.Name = "PriorityTextBox"
        Me.PriorityTextBox.ReadOnly = True
        Me.PriorityTextBox.Size = New System.Drawing.Size(64, 20)
        Me.PriorityTextBox.TabIndex = 111
        Me.PriorityTextBox.TabStop = False
        '
        'PageCountTextBox
        '
        Me.PageCountTextBox.Location = New System.Drawing.Point(112, 175)
        Me.PageCountTextBox.Name = "PageCountTextBox"
        Me.PageCountTextBox.ReadOnly = True
        Me.PageCountTextBox.Size = New System.Drawing.Size(47, 20)
        Me.PageCountTextBox.TabIndex = 108
        Me.PageCountTextBox.TabStop = False
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(46, 153)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(54, 13)
        Me.Label24.TabIndex = 103
        Me.Label24.Text = "Maxim ID:"
        '
        'MaxIDTextBox
        '
        Me.MaxIDTextBox.Location = New System.Drawing.Point(112, 150)
        Me.MaxIDTextBox.Name = "MaxIDTextBox"
        Me.MaxIDTextBox.ReadOnly = True
        Me.MaxIDTextBox.Size = New System.Drawing.Size(221, 20)
        Me.MaxIDTextBox.TabIndex = 107
        Me.MaxIDTextBox.TabStop = False
        '
        'DocTypeComboBox
        '
        Me.DocTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.DocTypeComboBox.ItemHeight = 13
        Me.DocTypeComboBox.Location = New System.Drawing.Point(112, 21)
        Me.DocTypeComboBox.Name = "DocTypeComboBox"
        Me.DocTypeComboBox.Size = New System.Drawing.Size(304, 21)
        Me.DocTypeComboBox.TabIndex = 94
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(12, 253)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(88, 13)
        Me.Label23.TabIndex = 99
        Me.Label23.Text = "Reference Claim:"
        '
        'ReferenceClaimTextBox
        '
        Me.ReferenceClaimTextBox.Location = New System.Drawing.Point(112, 250)
        Me.ReferenceClaimTextBox.Name = "ReferenceClaimTextBox"
        Me.ReferenceClaimTextBox.ReadOnly = True
        Me.ReferenceClaimTextBox.Size = New System.Drawing.Size(100, 20)
        Me.ReferenceClaimTextBox.TabIndex = 112
        Me.ReferenceClaimTextBox.TabStop = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(18, 53)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(82, 13)
        Me.Label8.TabIndex = 92
        Me.Label8.Text = "Received Date:"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(38, 78)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(62, 13)
        Me.Label22.TabIndex = 96
        Me.Label22.Text = "Open Date:"
        '
        'OpenDateTextBox
        '
        Me.OpenDateTextBox.Location = New System.Drawing.Point(112, 75)
        Me.OpenDateTextBox.Name = "OpenDateTextBox"
        Me.OpenDateTextBox.ReadOnly = True
        Me.OpenDateTextBox.Size = New System.Drawing.Size(100, 20)
        Me.OpenDateTextBox.TabIndex = 102
        Me.OpenDateTextBox.TabStop = False
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(43, 26)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(57, 13)
        Me.Label21.TabIndex = 93
        Me.Label21.Text = "Doc Type:"
        '
        'ReceivedDateTextBox
        '
        Me.ReceivedDateTextBox.Location = New System.Drawing.Point(112, 50)
        Me.ReceivedDateTextBox.Name = "ReceivedDateTextBox"
        Me.ReceivedDateTextBox.ReadOnly = True
        Me.ReceivedDateTextBox.Size = New System.Drawing.Size(100, 20)
        Me.ReceivedDateTextBox.TabIndex = 101
        Me.ReceivedDateTextBox.TabStop = False
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Location = New System.Drawing.Point(22, 203)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(78, 13)
        Me.Label28.TabIndex = 113
        Me.Label28.Text = "Batch Number:"
        '
        'BatchNumTextBox
        '
        Me.BatchNumTextBox.Location = New System.Drawing.Point(112, 200)
        Me.BatchNumTextBox.Name = "BatchNumTextBox"
        Me.BatchNumTextBox.ReadOnly = True
        Me.BatchNumTextBox.Size = New System.Drawing.Size(278, 20)
        Me.BatchNumTextBox.TabIndex = 109
        Me.BatchNumTextBox.TabStop = False
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(56, 128)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(44, 13)
        Me.Label26.TabIndex = 110
        Me.Label26.Text = "Doc ID:"
        '
        'DocIDTextBox
        '
        Me.DocIDTextBox.Location = New System.Drawing.Point(112, 125)
        Me.DocIDTextBox.Name = "DocIDTextBox"
        Me.DocIDTextBox.ReadOnly = True
        Me.DocIDTextBox.Size = New System.Drawing.Size(144, 20)
        Me.DocIDTextBox.TabIndex = 105
        Me.DocIDTextBox.TabStop = False
        '
        'ReferenceIDTextBox
        '
        Me.ReferenceIDTextBox.Location = New System.Drawing.Point(112, 276)
        Me.ReferenceIDTextBox.Name = "ReferenceIDTextBox"
        Me.ReferenceIDTextBox.ReadOnly = True
        Me.ReferenceIDTextBox.Size = New System.Drawing.Size(140, 20)
        Me.ReferenceIDTextBox.TabIndex = 118
        Me.ReferenceIDTextBox.TabStop = False
        '
        'AnnotateTabPage
        '
        Me.AnnotateTabPage.Controls.Add(Me.ArchiveButton)
        Me.AnnotateTabPage.Controls.Add(Me.AnnotationsControl)
        Me.AnnotateTabPage.Controls.Add(Me.RouteButton)
        Me.AnnotateTabPage.Location = New System.Drawing.Point(4, 22)
        Me.AnnotateTabPage.Name = "AnnotateTabPage"
        Me.AnnotateTabPage.Padding = New System.Windows.Forms.Padding(3)
        Me.AnnotateTabPage.Size = New System.Drawing.Size(571, 322)
        Me.AnnotateTabPage.TabIndex = 2
        Me.AnnotateTabPage.Text = "Annotate"
        Me.AnnotateTabPage.UseVisualStyleBackColor = True
        '
        'ArchiveButton
        '
        Me.ArchiveButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ArchiveButton.Location = New System.Drawing.Point(328, 293)
        Me.ArchiveButton.Name = "ArchiveButton"
        Me.ArchiveButton.Size = New System.Drawing.Size(75, 23)
        Me.ArchiveButton.TabIndex = 103
        Me.ArchiveButton.Text = "&Archive"
        Me.EnhancedToolTip.SetToolTip(Me.ArchiveButton, "Archive & Complete claim")
        Me.EnhancedToolTip.SetToolTipWhenDisabled(Me.ArchiveButton, "Archive not available for JAA claim")
        '
        'AnnotationsControl
        '
        Me.AnnotationsControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AnnotationsControl.AppKey = "UFCW\Claims\"
        Me.AnnotationsControl.Location = New System.Drawing.Point(3, 3)
        Me.AnnotationsControl.Name = "AnnotationsControl"
        Me.AnnotationsControl.Size = New System.Drawing.Size(565, 284)
        Me.AnnotationsControl.TabIndex = 103
        '
        'RouteButton
        '
        Me.RouteButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.RouteButton.Location = New System.Drawing.Point(164, 293)
        Me.RouteButton.Name = "RouteButton"
        Me.RouteButton.Size = New System.Drawing.Size(75, 23)
        Me.RouteButton.TabIndex = 102
        Me.RouteButton.Text = "&Route"
        Me.EnhancedToolTip.SetToolTip(Me.RouteButton, "Route claim to specified examiner")
        '
        'EnhancedToolTip
        '
        Me.EnhancedToolTip.AutomaticDelay = 50
        Me.EnhancedToolTip.AutoPopDelay = 5000
        Me.EnhancedToolTip.InitialDelay = 50
        Me.EnhancedToolTip.ReshowDelay = 10
        '
        'UTLUtility
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(600, 405)
        Me.Controls.Add(Me.ImageWarning)
        Me.Controls.Add(Me.StepTab)
        Me.Controls.Add(Me.NextPanelButton)
        Me.Controls.Add(Me.CancelWizard)
        Me.Controls.Add(Me.FinishWizardButton)
        Me.Controls.Add(Me.PreviousPanelButton)
        'Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(616, 1000)
        Me.MinimumSize = New System.Drawing.Size(616, 443)
        Me.Name = "UTLUtility"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "UTL Utility"
        Me.TopMost = True
        Me.StepTab.ResumeLayout(False)
        Me.ParticipantTabPage.ResumeLayout(False)
        Me.ClaimPanel.ResumeLayout(False)
        Me.gpbProviderInformation.ResumeLayout(False)
        Me.gpbProviderInformation.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.DocumentTabPage.ResumeLayout(False)
        Me.QueuePanel.ResumeLayout(False)
        Me.gpbDocumentInformation.ResumeLayout(False)
        Me.gpbDocumentInformation.PerformLayout()
        Me.AnnotateTabPage.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub UTLUtility_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        SaveSettings()
    End Sub

    Private Sub frmUTLUtility_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        SetSettings()

        PreviousPanelButton.Enabled = False
        FinishWizardButton.Enabled = False

        StepTab.SelectTab(0)

        AnnotationsControl.CancelActionButton.Visible = False

    End Sub
    Private Function DecryptSSN(ByVal ssn As String) As Integer

        If ssn IsNot Nothing AndAlso ssn.ToString.Trim.Length > 0 Then

            ssn = ssn.ToString.Trim

            Return CInt(CStr(ssn.ToUpper.Replace("N"c, "0"c).
                Replace("E"c, "1"c).Replace("D"c, "2"c).Replace("T"c, "3"c).
                Replace("F"c, "4"c).Replace("C"c, "5"c).Replace("A"c, "6"c).
                Replace("P"c, "7"c).Replace("W"c, "8"c).Replace("S"c, "9"c).
                Replace("-"c, " "c).Replace("B"c, " "c).Replace("G"c, " "c).Replace("H"c, " "c).
                Replace("I", " "c).Replace("J"c, " "c).Replace("K"c, " "c).Replace("L"c, " "c).
                Replace("M"c, " "c).Replace("N"c, " "c).Replace("O"c, " "c).Replace("P"c, " "c).
                Replace("Q"c, " "c).Replace("R"c, " "c).Replace("U"c, " "c).Replace("V"c, " "c).
                Replace("X"c, " "c).Replace("Y"c, " "c).Replace("Z"c, " "c)).Trim.Replace(" "c, ""))

        End If
    End Function
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets a claim
    ' </summary>
    ' <param name="claimId"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[malkoi]	2007-06-27	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub GetClaim(ByVal claimID As Integer)
        Try

            'Init Active Directory
            AppDomain.CurrentDomain.SetPrincipalPolicy(System.Security.Principal.PrincipalPolicy.WindowsPrincipal)
            _UserPrincipal = CType(System.Threading.Thread.CurrentPrincipal, WindowsPrincipal)

            _ClaimDS = CType(CMSDALFDBMD.RetrieveClaimWithUpdate(claimID, _ClaimDS), ClaimDataset)

            _ClaimMasterBS = New BindingSource With {
                .DataSource = _ClaimDS.Tables("CLAIM_MASTER")
            }
            If _ClaimMasterBS IsNot Nothing AndAlso _ClaimMasterBS.Current IsNot Nothing Then
                _ClaimMasterDR = CType(DirectCast(_ClaimMasterBS.Current, DataRowView).Row, ClaimDataset.CLAIM_MASTERRow)
                If _ClaimMasterDR IsNot Nothing Then
                    _ClaimMasterDR("PART_SSN") = DecryptSSN(CStr(_ClaimDS.CLAIM_MASTER(0)("PART_SSN")))
                    _ClaimMasterDR("PAT_SSN") = DecryptSSN(CStr(_ClaimDS.CLAIM_MASTER(0)("PAT_SSN")))
                    _ClaimMasterBS.EndEdit()
                End If
            End If


            If _ClaimDS.Tables("MEDHDR") IsNot Nothing AndAlso _ClaimDS.Tables("MEDHDR").Rows.Count > 0 Then
                _MedHdrBS = New BindingSource With {
                        .DataSource = _ClaimDS.Tables("MEDHDR")
                }
                If _MedHdrBS IsNot Nothing AndAlso _MedHdrBS.Current IsNot Nothing Then
                    _MedHDRDR = CType(DirectCast(_MedHdrBS.Current, DataRowView).Row, ClaimDataset.MEDHDRRow)
                End If
                If _MedHDRDR IsNot Nothing Then
                    _MedHDRDR("PART_SSN") = DecryptSSN(CStr(_MedHDRDR("PART_SSN")))
                    _MedHDRDR("PAT_SSN") = DecryptSSN(CStr(_MedHDRDR("PAT_SSN")))
                    _MedHdrBS.EndEdit()
                End If

            End If

            SetUI()

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub SetUI()

        Try

            LoadClaimMasterDataBindings()

            If _MedHDRDR Is Nothing Then
                Throw New ApplicationException("Claim#" & UFCWGeneral.IsNullStringHandler(_ClaimMasterDR("CLAIM_ID"), "") & " is missing MEDHDR information. Archive Claim or Report this issue to IS." & Environment.NewLine)
            End If

            LoadMedHdrDataBindings()

            Me.Text = Me.Text.ToString & " - " & UFCWGeneral.IsNullStringHandler(_ClaimMasterDR("CLAIM_ID"), "")
            Me.Text = Me.Text & " [IDM - " & UFCWGeneral.IsNullStringHandler(_ClaimMasterDR("DOCID"), "") & "]"
            Me.ImageWarning.Visible = CBool(_ClaimMasterDR("ARCHIVE_SW"))

            ValidatePartSSN("") ' This force a lookup and refresh Participant info

        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub LoadClaimMasterDataBindings()

        Dim Bind As Binding

        Try
            _ClaimMasterBS.SuspendBinding()
            _MedHdrBS.SuspendBinding()

            FamilyIDTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "FAMILY_ID", True)
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            FamilyIDTextBox.DataBindings.Add(Bind)

            PartSSNTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "PART_SSN", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged
            AddHandler Bind.Format, AddressOf SSNBinding_Format
            AddHandler Bind.Parse, AddressOf SSNBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            PartSSNTextBox.DataBindings.Add(Bind)

            PartNameFirstTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "PART_FNAME", True)
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            PartNameFirstTextBox.DataBindings.Add(Bind)

            PartNameMiddleTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "PART_INT", True)
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            PartNameMiddleTextBox.DataBindings.Add(Bind)

            PartNameLastTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "PART_LNAME", True)
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            PartNameLastTextBox.DataBindings.Add(Bind)

            PatRelationIDTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "RELATION_ID", True)
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            PatRelationIDTextBox.DataBindings.Add(Bind)

            PatSSNTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "PAT_SSN", True)
            AddHandler Bind.Format, AddressOf SSNBinding_Format
            AddHandler Bind.Parse, AddressOf SSNBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            PatSSNTextBox.DataBindings.Add(Bind)

            PatNameFirstTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "PAT_FNAME", True)
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            PatNameFirstTextBox.DataBindings.Add(Bind)

            PatNameMiddleTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "PAT_INT", True)
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            PatNameMiddleTextBox.DataBindings.Add(Bind)

            PatNameLastTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "PAT_LNAME", True)
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            PatNameLastTextBox.DataBindings.Add(Bind)

            ReceivedDateTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "REC_DATE", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.Never 'ReadOnly
            Bind.FormatString = "MM-dd-yyyy"
            ReceivedDateTextBox.DataBindings.Add(Bind)

            OpenDateTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "OPEN_DATE", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.Never 'Readonly
            Bind.FormatString = "MM-dd-yyyy"
            OpenDateTextBox.DataBindings.Add(Bind)

            ClaimIDTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "CLAIM_ID", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.Never
            ClaimIDTextBox.DataBindings.Add(Bind)

            DocIDTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "DOCID", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.Never
            DocIDTextBox.DataBindings.Add(Bind)

            MaxIDTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "MAXID", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.Never
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            MaxIDTextBox.DataBindings.Add(Bind)

            PageCountTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "PAGE_COUNT", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.Never
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            PageCountTextBox.DataBindings.Add(Bind)

            BatchNumTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "BATCH", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.Never
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            BatchNumTextBox.DataBindings.Add(Bind)

            PriorityTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "PRIORITY", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.Never
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            PriorityTextBox.DataBindings.Add(Bind)

            ReferenceClaimTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "REFRENCE_CLAIM", True)
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            ReferenceClaimTextBox.DataBindings.Add(Bind)

            ReferenceIDTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "REFERENCE_ID", True)
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            ReferenceIDTextBox.DataBindings.Add(Bind)

            DocTypeComboBox.DataBindings.Clear()
            Bind = New Binding("SelectedValue", _ClaimMasterBS, "DOC_TYPE", True)
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            DocTypeComboBox.DataBindings.Add(Bind)

            ProviderTINTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "PROV_TIN", True, DataSourceUpdateMode.OnPropertyChanged)
            AddHandler Bind.Format, AddressOf TINBinding_Format
            AddHandler Bind.Parse, AddressOf TINBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            ProviderTINTextBox.DataBindings.Add(Bind)

            ProviderIDTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "PROV_ID", True)
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            ProviderIDTextBox.DataBindings.Add(Bind)


            _ClaimMasterBS.ResumeBinding()
            _MedHdrBS.ResumeBinding()

        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub LoadMedHdrDataBindings()

        Dim Bind As Binding

        Try
            _MedHdrBS.SuspendBinding()

            ProviderLicenseNoTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _MedHdrBS, "PROV_LICENSE", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            ProviderLicenseNoTextBox.DataBindings.Add(Bind)

            ProviderRenderingNPITextBox.DataBindings.Clear()
            Bind = New Binding("Text", _MedHdrBS, "RENDERING_NPI", True)
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            AddHandler Bind.Parse, AddressOf ProviderRenderingNPIBinding_Parse
            AddHandler Bind.Format, AddressOf ProviderRenderingNPIBinding_Format
            ProviderRenderingNPITextBox.DataBindings.Add(Bind)

            PatDOBTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _MedHdrBS, "PAT_DOB", True, DataSourceUpdateMode.OnPropertyChanged, "N/A")
            Bind.FormatString = "MM-dd-yyyy"
            AddHandler Bind.Parse, AddressOf UFCWGeneral.DateOnlyBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            PatDOBTextBox.DataBindings.Add(Bind)

            PatAcctNoTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _MedHdrBS, "PAT_ACCT_NBR", True)
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            PatAcctNoTextBox.DataBindings.Add(Bind)


            BCCZIPTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _MedHdrBS, "PROV_ZIP", True, DataSourceUpdateMode.OnPropertyChanged)
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            AddHandler Bind.Parse, AddressOf ZipBinding_Parse
            AddHandler Bind.Format, AddressOf ZipBinding_Format
            BCCZIPTextBox.DataBindings.Add(Bind)

            PatSexTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _MedHdrBS, "PAT_SEX", True)
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            PatSexTextBox.DataBindings.Add(Bind)

            _MedHdrBS.ResumeBinding()

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub LoadDocTypeComboBox()
        Dim DT2 As System.Data.DataTable

        Try

            DocTypeComboBox.DataSource = Nothing
            DocTypeComboBox.Items.Clear()

            DT2 = CMSDALFDBMD.RetrieveActiveDocTypes

            DocTypeComboBox.DataSource = DT2
            DocTypeComboBox.DisplayMember = "DOC_TYPE"
            DocTypeComboBox.ValueMember = "DOC_TYPE"


            'For Cnt As Integer = 0 To DT2.Rows.Count - 1
            '    DocTypeComboBox.Items.Add(DT2.Rows(Cnt)("DOC_TYPE"))
            'Next
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Function CheckForUserDocTypeSecurity(ByVal User As String, ByVal DocClass As String, ByVal DocType As String) As Boolean
        Try
            Dim DV As New DataView(CMSDALFDBMD.RetrieveUserDocTypes(User), "DOC_CLASS = '" & DocClass & "' And DOC_TYPE = '" & DocType & "'", "DOC_CLASS, DOC_TYPE", DataViewRowState.CurrentRows)
            If DV.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub SaveChanges(Optional ByVal routing As Boolean = False, Optional ByVal routeDia As RouteDialog = Nothing)

        Dim Transaction As DbTransaction
        Dim PatientNameChange As Boolean = False
        Dim PartSSNChange As Boolean = False
        Dim PatientKeyChange As Boolean = False
        Dim AmountChange As Boolean = False
        Dim DocTypeChange As Boolean = False
        Dim SecurityChange As Boolean = False
        Dim DocSec As String = ""

        Try

            Transaction = CMSDALCommon.BeginTransaction

            If CInt(UnFormatSSN(PartSSNTextBox.Text)) <> CInt(_ClaimMasterDR("PART_SSN").ToString.Trim) Then
                PartSSNChange = True
            End If

            If DocTypeComboBox.Text <> _ClaimMasterDR("DOC_TYPE").ToString.Trim Then
                DocTypeChange = True
            End If

            If routing Then
                If HasChanges() Then
                    CMSDALFDBMD.UpdateClaimMasterWithRoute(CInt(_ClaimMasterDR("CLAIM_ID")),
                                                            UFCWGeneral.IsNullIntegerHandler(UnFormatSSN(PartSSNTextBox.Text)), UFCWGeneral.IsNullIntegerHandler(UnFormatSSN(PatSSNTextBox.Text)),
                                                            CInt(FamilyIDTextBox.Text), CShort(PatRelationIDTextBox.Text),
                                                            CShort(_ClaimMasterDR("PRIORITY")), Math.Abs(CDec(_ClaimMasterDR("SECURITY_SW"))),
                                                            Math.Abs(CDec(_ClaimMasterDR("ATTACH_SW"))), Math.Abs(CDec(_ClaimMasterDR("DUPLICATE_SW"))),
                                                            0,
                                                            UFCWGeneral.IsNullStringHandler(_ClaimMasterDR("STATUS")),
                                                            UFCWGeneral.IsNullDateHandler(_ClaimMasterDR("STATUS_DATE")),
                                                             UFCWGeneral.IsNullStringHandler(_ClaimMasterDR("DOC_CLASS")),
                                                             UFCWGeneral.IsNullStringHandler(DocTypeComboBox.Text.Trim),
                                                            UFCWGeneral.IsNullDateHandler(_ClaimMasterDR("DATE_OF_SERVICE")),
                                                             UFCWGeneral.IsNullStringHandler(PatNameFirstTextBox.Text.Trim), UFCWGeneral.IsNullStringHandler(PatNameMiddleTextBox.Text.Trim),
                                                             UFCWGeneral.IsNullStringHandler(PatNameLastTextBox.Text.Trim), UFCWGeneral.IsNullStringHandler(PartNameFirstTextBox.Text.Trim),
                                                             UFCWGeneral.IsNullStringHandler(PartNameMiddleTextBox.Text.Trim), UFCWGeneral.IsNullStringHandler(PartNameLastTextBox.Text.Trim),
                                                            UFCWGeneral.IsNullIntegerHandler(ProviderIDTextBox.Text.Trim),
                                                            UFCWGeneral.IsNullIntegerHandler(UnFormatTIN(ProviderTINTextBox.Text.Trim)),
                                                            UFCWGeneral.IsNullIntegerHandler(_ClaimMasterDR("REFRENCE_CLAIM")), _DomainUser.ToUpper, Transaction)
                Else
                    CMSDALFDBMD.UpdateClaimMasterRouting(CInt(_ClaimMasterDR("CLAIM_ID")), _DomainUser.ToUpper, Transaction)
                End If
            Else
                CMSDALFDBMD.UpdateClaimMaster(CInt(_ClaimMasterDR("CLAIM_ID")), UFCWGeneral.IsNullIntegerHandler(UnFormatSSN(PartSSNTextBox.Text)), UFCWGeneral.IsNullIntegerHandler(UnFormatSSN(PatSSNTextBox.Text)),
                                                CInt(FamilyIDTextBox.Text), CShort(PatRelationIDTextBox.Text),
                                                CShort(_ClaimMasterDR("PRIORITY")), Math.Abs(CDec(_ClaimMasterDR("SECURITY_SW"))),
                                                Math.Abs(CDec(_ClaimMasterDR("ATTACH_SW"))), Math.Abs(CDec(_ClaimMasterDR("DUPLICATE_SW"))),
                                                Math.Abs(CDec(_ClaimMasterDR("BUSY_SW"))), CStr(_ClaimMasterDR("STATUS")),
                                                UFCWGeneral.IsNullDateHandler(_ClaimMasterDR("STATUS_DATE")), UFCWGeneral.IsNullStringHandler(_ClaimMasterDR("DOC_CLASS")),
                                                DocTypeComboBox.Text.Trim, UFCWGeneral.IsNullDateHandler(_ClaimMasterDR("DATE_OF_SERVICE")),
                                                PatNameFirstTextBox.Text.Trim, PatNameMiddleTextBox.Text.Trim,
                                                PatNameLastTextBox.Text.Trim, PartNameFirstTextBox.Text.Trim,
                                                PartNameMiddleTextBox.Text.Trim, PartNameLastTextBox.Text.Trim,
                                                UFCWGeneral.IsNullIntegerHandler(UnFormatTIN(ProviderTINTextBox.Text.Trim)), UFCWGeneral.IsNullIntegerHandler(ProviderIDTextBox.Text.Trim),
                                                UFCWGeneral.IsNullIntegerHandler(_ClaimMasterDR("REFRENCE_CLAIM")), _DomainUser.ToUpper, Transaction)
            End If

            If HasChanges() Then
                CMSDALFDBMD.UpdateMEDHDR(CInt(_ClaimMasterDR("CLAIM_ID")), Math.Abs(CDec(_ClaimMasterDR("SECURITY_SW"))),
                                        CInt(FamilyIDTextBox.Text), CShort(PatRelationIDTextBox.Text),
                                        UFCWGeneral.IsNullIntegerHandler(UnFormatSSN(PartSSNTextBox.Text)), UFCWGeneral.IsNullIntegerHandler(UnFormatSSN(PatSSNTextBox.Text)),
                                        UFCWGeneral.IsNullStringHandler(_MedHDRDR("SYSTEM_CODE")),
                                        UFCWGeneral.IsNullStringHandler(_MedHDRDR("STATUS")),
                                        UFCWGeneral.IsNullDateHandler(_MedHDRDR("STATUS_DATE")),
                                        UFCWGeneral.IsNullStringHandler(_MedHDRDR("CLAIM_TYPE")),
                                        UFCWGeneral.IsNullStringHandler(_MedHDRDR("ADMITTANCE")),
                                        UFCWGeneral.IsNullDateHandler(_MedHDRDR("REC_DATE")), UFCWGeneral.IsNullDateHandler(_MedHDRDR("NETWRK_REC_DATE")),
                                        UFCWGeneral.IsNullDateHandler(_MedHDRDR("OCC_FROM_DATE")), UFCWGeneral.IsNullDateHandler(_MedHDRDR("OCC_TO_DATE")),
                                        UFCWGeneral.IsNullDateHandler(_MedHDRDR("INCIDENT_DATE")),
                                        UFCWGeneral.IsNullStringHandler(_MedHDRDR("PPO")), UFCWGeneral.IsNullStringHandler(_MedHDRDR("COB")),
                                        UFCWGeneral.IsNullStringHandler(_MedHDRDR("PAYEE")), UFCWGeneral.IsNullStringHandler(_MedHDRDR("PRICED_BY")),
                                        UFCWGeneral.IsNullStringHandler(_MedHDRDR("PRICING_ERROR")),
                                        Math.Abs(CDec(_MedHDRDR("ATTACH_SW"))),
                                        Math.Abs(CDec(_ClaimMasterDR("DUPLICATE_SW"))), Math.Abs(CDec(_MedHDRDR("NON_PAR_SW"))),
                                        Math.Abs(CDec(_MedHDRDR("OUT_OF_AREA_SW"))), Math.Abs(CDec(_MedHDRDR("AUTO_ACCIDENT_SW"))),
                                        Math.Abs(CDec(_MedHDRDR("WORKERS_COMP_SW"))), Math.Abs(CDec(_MedHDRDR("PREVENTATIVE_SW"))), Math.Abs(CDec(_MedHDRDR("OTH_ACCIDENT_SW"))), Math.Abs(CDec(_MedHDRDR("CHIRO_SW"))),
                                       Math.Abs(CDec(_MedHDRDR("PARITY_SW"))), Math.Abs(CDec(_MedHDRDR("SED_SW"))),
                                        Math.Abs(CDec(_MedHDRDR("AUTHORIZED_SW"))), Math.Abs(CDec(_MedHDRDR("ASSIGN_OF_BEN_SW"))),
                                        Math.Abs(CDec(_MedHDRDR("ADJUSTMENT_SW"))), Math.Abs(CDec(_MedHDRDR("OTH_INS_SW"))), Math.Abs(CDec(_MedHDRDR("OTH_INS_REFUSAL_SW"))),
                                        UFCWGeneral.IsNullIntegerHandler(_MedHDRDR("OTH_INS_ID")), UFCWGeneral.IsNullStringHandler(_MedHDRDR("OTH_INS_POLICY_NBR")),
                                        UFCWGeneral.IsNullStringHandler(PatNameFirstTextBox.Text), UFCWGeneral.IsNullStringHandler(PatNameLastTextBox.Text), UFCWGeneral.IsNullStringHandler(PatSexTextBox.Text),
                                        UFCWGeneral.IsNullDateHandler(PatDOBTextBox.Text.Trim),
                                        UFCWGeneral.IsNullStringHandler(PatAcctNoTextBox.Text),
                                        UFCWGeneral.IsNullIntegerHandler(_MedHDRDR("PAT_ZIP")),
                                       UFCWGeneral.IsNullIntegerHandler(_MedHDRDR("PAT_ZIP2")),
                                        UFCWGeneral.IsNullIntegerHandler(UnFormatTIN(ProviderTINTextBox.Text.Trim)),
                                        UFCWGeneral.IsNullIntegerHandler(ProviderIDTextBox.Text.Trim),
                                       UFCWGeneral.IsNullIntegerHandler(BCCZIPTextBox.Text.Trim),
                                       UFCWGeneral.IsNullIntegerHandler(_MedHDRDR("PROV_ZIP2")),
                                        UFCWGeneral.IsNullStringHandler(ProviderLicenseNoTextBox.Text),
                                         UFCWGeneral.IsNullDecimalHandler(UFCWGeneral.ToNullDecimalHandler(ProviderRenderingNPITextBox.Text.Trim)),
                                        UFCWGeneral.IsNullIntegerHandler(_MedHDRDR("BILL_TAXID")),
                                         UFCWGeneral.IsNullStringHandler(_MedHDRDR("BILL_NAME")), UFCWGeneral.IsNullStringHandler(_MedHDRDR("BILL_ADDR1")),
                                         UFCWGeneral.IsNullStringHandler(_MedHDRDR("BILL_ADDR2")), UFCWGeneral.IsNullStringHandler(_MedHDRDR("BILL_CITY")),
                                         UFCWGeneral.IsNullStringHandler(_MedHDRDR("BILL_STATE")),
                                        UFCWGeneral.IsNullIntegerHandler(_MedHDRDR("BILL_ZIP")),
                                        UFCWGeneral.IsNullIntegerHandler(_MedHDRDR("BILL_ZIP2")),
                                        UFCWGeneral.IsNullDecimalHandler(_MedHDRDR("TOT_CHRG_AMT")),
                                       UFCWGeneral.IsNullDecimalHandler(_MedHDRDR("TOT_PRICED_AMT")),
                                        UFCWGeneral.IsNullDecimalHandler(_MedHDRDR("TOT_ALLOWED_AMT")),
                                        UFCWGeneral.IsNullDecimalHandler(_MedHDRDR("TOT_OTH_INS_AMT")),
                                       UFCWGeneral.IsNullDecimalHandler(_MedHDRDR("TOT_PAID_AMT")),
                                       UFCWGeneral.IsNullDecimalHandler(_MedHDRDR("TOT_PROCESSED_AMT")),
                                       UFCWGeneral.IsNullIntegerHandler(_MedHDRDR("HOLD_DAYS")),
                                        UFCWGeneral.IsNullDateHandler(_MedHDRDR("HOLD_DATE")),
                                        UFCWGeneral.IsNullStringHandler(_MedHDRDR("HOLD_TIME")), _DomainUser.ToUpper, Transaction)

            End If

            If PartSSNChange = True Then
                If CBool(_ClaimMasterDR("SECURITY_SW")) Then

                    DocSec = CStr(CType(ConfigurationManager.GetSection("FNDocSecurity"), IDictionary)("EMP"))
                Else
                    DocSec = CStr(CType(ConfigurationManager.GetSection("FNDocSecurity"), IDictionary)("REG"))
                End If

                Dim FNDocument As New Document(CLng(_ClaimMasterDR("DOCID")))
                If FNDocument.UpdateSSN(CLng(_ClaimMasterDR("DOCID")), CInt(_ClaimMasterDR("PART_SSN")), DocSec, DocSec, DocSec) = False Then
                    CMSDALCommon.RollbackTransaction(Transaction)
                    Return
                End If
            End If

            If routing Then

                Dim HistSum As String = "CLAIM ID " & Format(_ClaimMasterDR("CLAIM_ID"), "00000000") & " REASSIGNED TO " & routeDia.Recipient.ToUpper
                HistSum = "CLAIM ID " & Format(_ClaimMasterDR("CLAIM_ID"), "00000000") & " REASSIGNED TO " & routeDia.Recipient.ToUpper

                Dim HistDetail As String = routeDia.Comment.ToUpper
                CMSDALFDBMD.CreateDocHistory(CInt(_ClaimMasterDR("CLAIM_ID")), CLng(_ClaimMasterDR("DOCID")), "ROUTE", CInt(_ClaimMasterDR("FAMILY_ID")), CShort(_ClaimMasterDR("RELATION_ID")), CInt(_ClaimMasterDR("PART_SSN")), CInt(_ClaimMasterDR("PAT_SSN")), CStr(_ClaimMasterDR("DOC_CLASS")), CStr(_ClaimMasterDR("DOC_TYPE")), HistSum, HistDetail, _DomainUser.ToUpper, Transaction)
                CMSDALFDBMD.PendToUser(CInt(_ClaimMasterDR("CLAIM_ID")), routeDia.Recipient.Trim.ToUpper, Transaction)

                CMSDALFDBMD.CreateRoutingHistory(CInt(_ClaimMasterDR("CLAIM_ID")), _DomainUser.ToUpper, routeDia.Recipient.Trim.ToUpper, routeDia.Comment.ToUpper, _DomainUser.ToUpper, Transaction)

                SaveAnnotations(Transaction)

            End If

            CMSDALCommon.CommitTransaction(Transaction)

        Catch ex As Exception
            If Transaction IsNot Nothing Then
                CMSDALCommon.RollbackTransaction(Transaction)
            End If
            Throw
        Finally
        End Try

    End Sub
    Private Function HasChanges() As Boolean
        Try
            '        'Participant
            '        If PartSSNTextBox.Text.Trim.Length > 0 Then
            '            If CDbl(UnFormatSSN(PartSSNTextBox.Text)) <> _ClaimMasterDR.PART_SSN Then Return True
            '        End If

            '        If PartNameFirstTextBox.Text.Trim.Length > 0 Then
            '            If IsDBNull(_ClaimMasterDR("PART_FNAME")) OrElse PartNameFirstTextBox.Text <> _ClaimMasterDR.PART_FNAME Then Return True
            '        End If

            '        If PartNameLastTextBox.Text.Trim.Length > 0 Then
            '            If IsDBNull(_ClaimMasterDR("PART_LNAME")) OrElse PartNameLastTextBox.Text <> _ClaimMasterDR.PART_LNAME Then Return True
            '        End If

            '        If PartNameMiddleTextBox.Text.Trim.Length > 0 Then
            '            If IsDBNull(_ClaimMasterDR("PART_INT")) OrElse PartNameMiddleTextBox.Text <> _ClaimMasterDR.PART_INT Then Return True
            '        End If

            '        If FamilyIDTextBox.Text.Trim.Length > 0 Then
            '            If CDbl(FamilyIDTextBox.Text) <> _ClaimMasterDR.FAMILY_ID Then Return True
            '        End If

            '        'Patient
            '        If PatSSNTextBox.Text.Trim.Length > 0 Then
            '            If CDbl(UnFormatSSN(PatSSNTextBox.Text)) <> _ClaimMasterDR.PAT_SSN Then Return True
            '        End If


            '        If PatNameFirstTextBox.Text.Trim.Length > 0 Then
            '            If IsDBNull(_ClaimMasterDR("PAT_FNAME")) OrElse PatNameFirstTextBox.Text <> _ClaimMasterDR.PAT_FNAME Then Return True
            '        End If

            '        If PatNameLastTextBox.Text.Trim.Length > 0 Then
            '            If IsDBNull(_ClaimMasterDR("PAT_LNAME")) OrElse PatNameLastTextBox.Text <> _ClaimMasterDR.PAT_LNAME Then Return True
            '        End If

            '        If PatNameMiddleTextBox.Text.Trim.Length > 0 Then
            '            If IsDBNull(_ClaimMasterDR("PAT_INT")) OrElse PatNameMiddleTextBox.Text <> _ClaimMasterDR.PAT_INT Then Return True
            '        End If

            '        If PatRelationIDTextBox.Text.Trim.Length > 0 Then
            '            If CDbl(PatRelationIDTextBox.Text) <> _ClaimMasterDR.RELATION_ID Then Return True
            '        End If

            '        If PatDOBTextBox.Text <> "N/A" Then
            '            If PatDOBTextBox.Text <> _MedHDRDR.PAT_DOB.ToString("yyyy-MM-dd") Then Return True
            '        End If

            '        If PatSexTextBox.Text <> _MedHDRDR.PAT_SEX Then Return True

            '        If PatAcctNoTextBox.Text <> _MedHDRDR.PAT_ACCT_NBR.ToString Then Return True

            '        'Provider
            '        If ProviderTINTextBox.Text.Trim.Length > 0 Then
            '            If CDbl(UFCWGeneral.UnFormatTIN(ProviderTINTextBox.Text)) <> _ClaimMasterDR.PROV_TIN Then Return True
            '        End If

            '        If ProviderLicenseNoTextBox.Text.Trim.Length > 0 Then
            '            If IsDBNull(_MedHDRDR("PROV_LICENSE")) OrElse ProviderLicenseNoTextBox.Text <> _MedHDRDR.PROV_LICENSE Then Return True
            '        End If

            '        If ProviderRenderingNPITextBox.Text.Trim.Length > 0 Then
            '            If IsDBNull(_MedHDRDR("RENDERING_NPI")) OrElse CDbl(ProviderRenderingNPITextBox.Text) <> _MedHDRDR.RENDERING_NPI Then Return True
            '        End If

            '        If BCCZIPTextBox.Text <> "0" Then
            '            If IsDBNull(_MedHDRDR.PROV_ZIP) OrElse BCCZIPTextBox.Text <> _MedHDRDR.PROV_ZIP.ToString Then Return True
            '        End If

            '        'Doc Type
            '        If DocTypeComboBox.Text <> _ClaimMasterDR.DOC_TYPE Then Return True
            If DirectCast(_ClaimMasterBS.DataSource, DataTable).GetChanges IsNot Nothing AndAlso DirectCast(_ClaimMasterBS.DataSource, DataTable).GetChanges.Rows.Count > 0 Then
                If IdentifyChanges(DirectCast(_ClaimMasterBS.DataSource, DataTable), _ClaimMasterDR) Then Return True
            End If
            If DirectCast(_MedHdrBS.DataSource, DataTable).GetChanges IsNot Nothing AndAlso DirectCast(_MedHdrBS.DataSource, DataTable).GetChanges.Rows.Count > 0 Then
                If IdentifyChanges(DirectCast(_MedHdrBS.DataSource, DataTable), _MedHDRDR) Then Return True
            End If
            'If _ClaimMasterDR.RowState = DataRowState.Modified Then Return True
            'If _MedHDRDR.RowState = DataRowState.Modified Then Return True

        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Function IdentifyChanges(ByVal TargetDT As DataTable, ByVal TargetDR As DataRow) As Boolean
        Try
            Dim ChangeTable As DataTable = TargetDT.GetChanges()
            Dim r As DataRow
            Dim ChangeCnt As Integer = 0
            If ChangeTable IsNot Nothing Then
                r = ChangeTable.Rows(0)
                For cnt As Integer = 0 To ChangeTable.Columns.Count - 1
                    If IsDBNull(r(cnt)) = False And IsDBNull(TargetDR(cnt)) = False Then
                        If UFCWGeneral.IsNullStringHandler(r(cnt)) <> UFCWGeneral.IsNullStringHandler(TargetDR(cnt)) Then
                            ChangeCnt += 1
                        End If
                    ElseIf IsDBNull(r(cnt)) = False And IsDBNull(TargetDR(cnt)) = True Then
                        ChangeCnt += 1
                    ElseIf IsDBNull(r(cnt)) = True And IsDBNull(TargetDR(cnt)) = False Then
                        If TargetDR(cnt).GetType <> GetType(Decimal) AndAlso TargetDR(cnt).GetType <> GetType(Date) AndAlso Not ChangeTable.Columns(cnt).ToString.StartsWith("TOT") Then
                            ChangeCnt += 1
                        End If
                    End If
                Next
            End If
            If ChangeCnt > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Sub SaveAnnotations(ByRef transaction As DbTransaction)

        Try

            If _ClaimDS.ANNOTATIONS.GetChanges IsNot Nothing AndAlso _ClaimDS.ANNOTATIONS.GetChanges.Rows.Count > 0 Then
                For Each DR As DataRow In _ClaimDS.ANNOTATIONS.GetChanges(DataRowState.Added).Rows
                    CMSDALFDBMD.CreateAnnotation(CInt(_ClaimMasterDR("CLAIM_ID")), CInt(_ClaimMasterDR("FAMILY_ID")), CShort(_ClaimMasterDR("RELATION_ID")),
                                              UFCWGeneral.IsNullIntegerHandler(_ClaimMasterDR("PART_SSN")), UFCWGeneral.IsNullIntegerHandler(_ClaimMasterDR("PAT_SSN")),
                                               UFCWGeneral.IsNullStringHandler(_ClaimMasterDR("PART_FNAME")), UFCWGeneral.IsNullStringHandler(_ClaimMasterDR("PART_LNAME")),
                                               UFCWGeneral.IsNullStringHandler(_ClaimMasterDR("PAT_FNAME")), UFCWGeneral.IsNullStringHandler(_ClaimMasterDR("PAT_LNAME")),
                                               UFCWGeneral.IsNullStringHandler(DR("ANNOTATION")), DR("FLAG"), _DomainUser.ToUpper, transaction)
                Next
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub PartLookupButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PartLookupButton.Click

        Dim CancelChanges As Boolean = False
        Dim PartLookupFrm As ParticipantLookUpForm
        Dim DR As DataRow

        Try

            PartLookupFrm = New ParticipantLookUpForm(PartNameLastTextBox.Text, PartNameFirstTextBox.Text, True)

            If PartLookupFrm.ShowDialog(Me) = DialogResult.OK Then
                DR = PartLookupFrm.PatientRow

                If CBool(DR("TRUST_SW")) Then
                    If UFCWGeneralAD.CMSCanAdjudicateEmployee = False Then
                        If MessageBox.Show("You are not authorized to work on Employee Claims." &
                                            vbCrLf & "You must save changes back to the queue." & vbCrLf &
                                            "Are you sure you want to make this change?", "Confirm Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                        Else
                            CancelChanges = True
                        End If
                    End If
                End If

                If CancelChanges = False Then
                    FamilyIDTextBox.Text = DR("FAMILY_ID").ToString

                    PartSSNTextBox.Text = If(IsDBNull(DR("PART_SSNO")), "", FormatSSN(DR("PART_SSNO").ToString))

                    PartNameFirstTextBox.Text = If(IsDBNull(DR("FIRST_NAME")), "", DR("FIRST_NAME").ToString)
                    PartNameMiddleTextBox.Text = If(IsDBNull(DR("MIDDLE_INITIAL")), "", DR("MIDDLE_INITIAL").ToString)

                    PartNameLastTextBox.Text = If(IsDBNull(DR("LAST_NAME")), "", DR("LAST_NAME").ToString)

                    _ClaimMasterDR("SECURITY_SW") = DR("TRUST_SW")

                    Me.BindingContext(_ClaimMasterDR).EndCurrentEdit()

                End If

                PartSSNTextBox.Focus()
                PartLookupButton.Focus()

            End If

        Catch ex As Exception
            Throw
        Finally

            If PartLookupFrm IsNot Nothing Then PartLookupFrm.Dispose()
            PartLookupFrm = Nothing

        End Try
    End Sub

    Private Sub PatLookupButton_Clickc(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PatLookupButton.Click
        Dim PatLookup As PatientLookup
        Dim DV As DataView
        Dim CurrentRowIndex As Integer

        Try
            'Modified to use Text box instead of DataSet
            PatLookup = New PatientLookup(CInt(FamilyIDTextBox.Text))

            If PatLookup.ShowDialog(Me) = DialogResult.OK Then
                DV = PatLookup.PatientDataGrid.GetCurrentDataView
                CurrentRowIndex = PatLookup.PatientDataGrid.CurrentRowIndex

                PatRelationIDTextBox.Text = UFCWGeneral.IsNullStringHandler(DV(CurrentRowIndex)("RELATION_ID"), "")
                PatSSNTextBox.Text = FormatSSN(UFCWGeneral.IsNullStringHandler(DV(CurrentRowIndex)("SSNO"), ""))

                PatNameFirstTextBox.Text = UFCWGeneral.IsNullStringHandler(DV(CurrentRowIndex)("FIRST_NAME"), "")
                PatNameMiddleTextBox.Text = UFCWGeneral.IsNullStringHandler(DV(CurrentRowIndex)("MIDDLE_INITIAL"), "")
                PatNameLastTextBox.Text = UFCWGeneral.IsNullStringHandler(DV(CurrentRowIndex)("LAST_NAME"), "")

                PatDOBTextBox.Text = UFCWGeneral.IsNullStringHandler(DV(CurrentRowIndex)("BIRTH_DATE"), "")
                PatAcctNoTextBox.Text = UFCWGeneral.IsNullStringHandler(_MedHDRDR("PAT_ACCT_NBR"), "")
                PatSexTextBox.Text = UFCWGeneral.IsNullStringHandler(DV(CurrentRowIndex)("GENDER"), "")
                'If PatRelationIDTextBox.Text.Trim.Length > 0 Then
                '    If CInt(PatRelationIDTextBox.Text) = 0 Then
                '        _ClaimMasterBS.EndEdit()
                '    Else
                '        _MedHdrBS.EndEdit()
                '    End If
                'End If

            End If

        Catch ex As Exception
            Throw
        Finally
            PatLookup.Dispose()
            PatLookup = Nothing

        End Try
    End Sub

    Private Sub NextPanel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NextPanelButton.Click

        Try

            DisplayPanel(+1)

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub DisplayPanel(offsetIndex As Integer)

        Try

            RemoveHandler StepTab.SelectedIndexChanged, AddressOf StepTab_SelectedIndexChanged

            StepTab.SelectTab(CInt(StepTab.SelectedIndex) + offsetIndex)

            Select Case CInt(StepTab.SelectedIndex)

                Case 0 'Claim Panel'

                    PreviousPanelButton.Enabled = False
                    FinishWizardButton.Enabled = False
                    NextPanelButton.Enabled = True

                Case 1 'Queue Panel'

                    PreviousPanelButton.Enabled = True
                    FinishWizardButton.Enabled = False
                    NextPanelButton.Enabled = True

                Case 2 'Annotations Panel'

                    PreviousPanelButton.Enabled = True
                    NextPanelButton.Enabled = False

                    If _MedHDRDR IsNot Nothing AndAlso _MedHDRDR("PRICED_BY").ToString.ToUpper.Contains("JAA") Then
                        ArchiveButton.Enabled = False
                    End If

                    If _AnnotationAdded AndAlso PatRelationIDTextBox.Text <> "-1" Then FinishWizardButton.Enabled = True

                    SetAnnotationsControl()

            End Select

        Catch ex As Exception
            Throw
        Finally
            AddHandler StepTab.SelectedIndexChanged, AddressOf StepTab_SelectedIndexChanged
        End Try

    End Sub

    Private Sub CancelWizard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelWizard.Click
        CloseUTLWizard()
    End Sub

    Private Sub CloseUTLWizard()
        If MessageBox.Show("Are you sure you want to cancel?", "Exit Wizard", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Me.Close()
        End If
    End Sub

    Private Sub PreviousPanel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PreviousPanelButton.Click
        DisplayPanel(-1)
    End Sub

    Private Sub FinishWizard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FinishWizardButton.Click

        Dim Transaction As DbTransaction
        Dim Unpended As Boolean = False

        Try
            'Give the user a choice, commit or dont.
            If ProviderIDTextBox.Text.Trim.Length < 1 Then

                MessageBox.Show("Provider ID is not set, reselect Provider to complete update", "Invalid Provider ID", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                StepTab.SelectTab(1)

            Else
                If MessageBox.Show("Are you ready to commit your changes?", "Save Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    'If the user is ready and the form has changes, lets save em
                    ' If HasChanges() Then
                    SaveChanges()
                    _Saved = True
                    ' End If

                    'After they have entered in an annotation lets ensure it gets saved
                    Transaction = CMSDALCommon.BeginTransaction

                    SaveAnnotations(Transaction)
                    'We ultimately unpend the user, regardless if there were changes to the form or not after we save the annotations.
                    'If CheckForUserDocTypeSecurity(DomainUser.ToUpper, ClaimMasterDR("DOC_CLASS"), ClaimMasterDR("DOC_TYPE")) = False Then
                    Unpended = True
                    CMSDALFDBMD.UnPendUser(CInt(_ClaimMasterDR("CLAIM_ID")), _DomainUser.ToUpper, Transaction)

                    If Array.IndexOf(PricingExcludeByDocType, DocTypeComboBox.Text.Trim) < 0 AndAlso _MedHDRDR("PRICED_BY").ToString.Trim.Length < 1 Then
                        Dim HistSum As String = "CLAIM ID " & Format(_ClaimMasterDR("CLAIM_ID"), "00000000") & " WAS SUBMITTED FOR BLUE CROSS BATCH PRICING"
                        Dim HistDetail As String = "FUNCTION: BLUE CROSS BATCH PRICE" & Microsoft.VisualBasic.vbCrLf &
                                        "CLAIM ID: " & Format(_ClaimMasterDR("CLAIM_ID"), "00000000")
                        CMSDALFDBMD.CreateDocHistory(CInt(_ClaimMasterDR("CLAIM_ID")), CLng(_ClaimMasterDR("DOCID")), "REPRICE", CInt(_ClaimMasterDR("FAMILY_ID")), CShort(_ClaimMasterDR("RELATION_ID")), CInt(_ClaimMasterDR("PART_SSN")), CInt(_ClaimMasterDR("PAT_SSN")), CStr(_ClaimMasterDR("DOC_CLASS")), CStr(_ClaimMasterDR("DOC_TYPE")), HistSum, HistDetail, _DomainUser.ToUpper, Transaction)

                        CMSDALFDBMD.CreateBCPricing(CInt(_ClaimMasterDR("CLAIM_ID")), _DomainUser.ToUpper, Transaction)
                        CMSDALFDBMD.UpdateClaimMasterStatus(CInt(_ClaimMasterDR("CLAIM_ID")), "PRICING", _DomainUser.ToUpper, Transaction)
                    Else
                        CMSDALFDBMD.UpdateClaimMasterStatus(CInt(_ClaimMasterDR("CLAIM_ID")), "OPEN", _DomainUser.ToUpper, Transaction)
                    End If

                    'End If
                    'We create a doc history for this UTL
                    CMSDALFDBMD.CreateDocHistory(CInt(_ClaimMasterDR("CLAIM_ID")), CLng(_ClaimMasterDR("DOCID")), "UPDATEDBYUTL", CInt(FamilyIDTextBox.Text), CShort(PatRelationIDTextBox.Text), CInt(UnFormatSSN(PartSSNTextBox.Text)), CInt(UnFormatSSN(PatSSNTextBox.Text)), CStr(_ClaimMasterDR("DOC_CLASS")), DocTypeComboBox.Text, "Claim " & _ClaimMasterDR("CLAIM_ID").ToString & " Reassigned to type " & DocTypeComboBox.Text, "Re-routed by system after change", _DomainUser.ToUpper, Transaction)
                    CMSDALFDBMD.RetrieveDocType(_ClaimMasterDR.DOC_CLASS, DocTypeComboBox.SelectedValue.ToString)
                    CMSDALCommon.CommitTransaction(Transaction)

                    Me.Close()
                End If

            End If
        Catch ex As Exception
            If Transaction IsNot Nothing Then
                CMSDALCommon.RollbackTransaction(Transaction)
            End If
        End Try

    End Sub

    Private Sub RouteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RouteButton.Click
        Try
            If RouteClaim() Then Me.Close()
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Function RouteClaim() As Boolean

        Dim RouteDia As RouteDialog
        Try

            RouteDia = New RouteDialog(CInt(_ClaimMasterDR("CLAIM_ID")), CStr(_ClaimMasterDR("DOC_TYPE")), "' '")

            If RouteDia.ShowDialog(Me) = DialogResult.OK Then

                SaveChanges(True, RouteDia)
                _Saved = True

                Return True
            End If

            Return False

        Catch ex As Exception
            Throw
        Finally
            If RouteDia IsNot Nothing Then RouteDia.Close()
            RouteDia = Nothing
        End Try
    End Function

    Private Sub ArchiveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ArchiveButton.Click
        ArchiveClaim()
    End Sub

    Private Sub ArchiveClaim()
        Dim Transaction As DbTransaction

        Try

            _ClaimMasterDR("ARCHIVE_SW") = True

            Transaction = CMSDALCommon.BeginTransaction

            CMSDALFDBMD.ArchiveClaimMaster(CInt(_ClaimMasterDR("CLAIM_ID")), "COMPLETED", _DomainUser.ToUpper, Transaction)

            Dim HistSum As String = "CLAIM ID " & Format(_ClaimMasterDR("CLAIM_ID"), "00000000") & " HAS BEEN ARCHIVED OUT OF THE QUEUE"
            Dim HistDetail As String = "ADJUSTER " & _DomainUser.ToUpper & " ARCHIVED THIS ITEM." & Microsoft.VisualBasic.vbCrLf &
                                        "NO FURTHER PROCESSING IS NECESSARY."
            CMSDALFDBMD.CreateDocHistory(CInt(_ClaimMasterDR("CLAIM_ID")), UFCWGeneral.IsNullLongHandler(_ClaimMasterDR("DOCID")), "COMPLETE", CInt(_ClaimMasterDR("FAMILY_ID")), CShort(_ClaimMasterDR("RELATION_ID")), CInt(_ClaimMasterDR("PART_SSN")), CInt(_ClaimMasterDR("PAT_SSN")), CStr(_ClaimMasterDR("DOC_CLASS")), CStr(_ClaimMasterDR("DOC_TYPE")), HistSum, HistDetail, _DomainUser.ToUpper, Transaction)
            SaveAnnotations(Transaction)

            CMSDALCommon.CommitTransaction(Transaction)

            MessageBox.Show("Claim ID: " + ClaimIDTextBox.Text + " was successfully archived!", "Archive Operation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            Me.Close()

        Catch ex As Exception
            If Transaction IsNot Nothing Then
                CMSDALCommon.RollbackTransaction(Transaction)
            End If
            Throw
        End Try
    End Sub

    Private Sub ProvLookupButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProvLookupButton.Click

        Dim ProvLookup As ProviderLookup
        Dim DV As DataView
        Dim CurrentRowIndex As Integer

        Try
            'Modified to use text box rather than what is in the Dataset
            If IsDBNull(_ClaimMasterDR("PROV_TIN")) = False Then
                ProvLookup = New ProviderLookup(CInt(UFCWGeneral.UnFormatTIN(ProviderTINTextBox.Text)))
            Else
                ProvLookup = New ProviderLookup
            End If

            If ProvLookup.ShowDialog(Me) = DialogResult.OK Then
                DV = ProvLookup.ProviderDataGrid.GetDefaultDataView
                CurrentRowIndex = ProvLookup.ProviderDataGrid.CurrentRowIndex

                'If (Not IsDBNull(_ClaimMasterDR("PROV_ID")) AndAlso IsDBNull(DV(CurrentRowIndex)("PROVIDER_ID"))) OrElse (IsDBNull(_ClaimMasterDR("PROV_ID")) AndAlso Not IsDBNull(DV(CurrentRowIndex)("PROVIDER_ID"))) Then
                ' ProviderIDTextBox.Text = CStr(DV(CurrentRowIndex)("PROVIDER_ID"))
                _ClaimMasterDR("PROV_ID") = CStr(DV(CurrentRowIndex)("PROVIDER_ID"))
                _MedHDRDR("PROV_LICENSE") = DBNull.Value
                _MedHDRDR("RENDERING_NPI") = DBNull.Value

                ' Me.ProviderLicenseNoTextBox.Text = "" 'forced to reset due to provider# change
                ' Me.ProviderRenderingNPITextBox.Text = "" 'forced to reset due to provider# change
                'ElseIf Not IsDBNull(_ClaimMasterDR("PROV_ID")) AndAlso CInt(_ClaimMasterDR("PROV_ID")) <> CInt(DV(CurrentRowIndex)("PROVIDER_ID")) Then
                '    '  ProviderIDTextBox.Text = CStr(DV(CurrentRowIndex)("PROVIDER_ID"))
                '    _ClaimMaste'rDR("PROV_ID") = CStr(DV(CurrentRowIndex)("PROVIDER_ID"))
                '    Me.ProviderLicenseNoTextBox.Text = "" 'forced to reset due to provider# change
                '    Me.ProviderRenderingNPITextBox.Text = "" 'forced to reset due to provider# change
                'End If

                '  If (Not IsDBNull(_ClaimMasterDR("PROV_NAME")) AndAlso IsDBNull(DV(CurrentRowIndex)("NAME1"))) OrElse (IsDBNull(_ClaimMasterDR("PROV_NAME")) AndAlso Not IsDBNull(DV(CurrentRowIndex)("NAME1"))) Then
                _ClaimMasterDR("PROV_NAME") = DV(CurrentRowIndex)("NAME1")
                'ElseIf Not IsDBNull(_ClaimMasterDR("PROV_NAME")) AndAlso _ClaimMasterDR("PROV_NAME").ToString.Trim <> DV(CurrentRowIndex)("NAME1").ToString.Trim Then
                '    _ClaimMasterDR("PROV_NAME") = DV(CurrentRowIndex)("NAME1")
                'End If
                _ClaimMasterDR("PROV_TIN") = DV(CurrentRowIndex)("TAXID")
                ''If (Not IsDBNull(_ClaimMasterDR("PROV_TIN")) AndAlso IsDBNull(DV(CurrentRowIndex)("TAXID"))) OrElse (IsDBNull(_ClaimMasterDR("PROV_TIN")) AndAlso Not IsDBNull(DV(CurrentRowIndex)("TAXID"))) Then
                'ProviderTINTextBox.Text = CStr(DV(CurrentRowIndex)("TAXID"))
                'ElseIf Not IsDBNull(_ClaimMasterDR("PROV_NAME")) AndAlso _ClaimMasterDR("PROV_TIN").ToString.Trim <> DV(CurrentRowIndex)("TAXID").ToString.Trim Then
                'ProviderTINTextBox.Text = CStr(DV(CurrentRowIndex)("TAXID"))
                'End If
                _MedHDRDR("PROV_ZIP") = DV(CurrentRowIndex)("ZIP")
                'If (Not IsDBNull(_MedHDRDR("PROV_ZIP")) AndAlso IsDBNull(DV(CurrentRowIndex)("ZIP"))) OrElse (IsDBNull(_MedHDRDR("PROV_ZIP")) AndAlso Not IsDBNull(DV(CurrentRowIndex)("ZIP"))) Then
                '    BCCZIPTextBox.Text = CStr(If(IsDBNull(DV(CurrentRowIndex)("ZIP")), "", DV(CurrentRowIndex)("ZIP")))
                'ElseIf Not IsDBNull(_MedHDRDR("PROV_ZIP")) AndAlso _MedHDRDR("PROV_ZIP").ToString.Trim <> DV(CurrentRowIndex)("ZIP").ToString.Trim Then
                '    BCCZIPTextBox.Text = CStr(DV(CurrentRowIndex)("ZIP"))
                'End If
                Me.BindingContext(_ClaimDS.CLAIM_MASTER).EndCurrentEdit()
                _ClaimMasterBS.EndEdit()
                _MedHdrBS.EndEdit()
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

#Region "SSN Validation Stuff"
    'Private Sub PartSSNTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PartSSNTextBox.TextChanged
    '    Dim TBox As TextBox = CType(sender, TextBox)
    '    Dim intCnt As Integer
    '    Dim strTmp As String

    '    If IsNumeric(TBox.Text) = False AndAlso Len(TBox.Text) > 0 Then
    '        PartSSNTextBox.MaxLength = 11
    '        strTmp = TBox.Text
    '        For intCnt = 1 To Len(strTmp)
    '            If IsNumeric(Mid(strTmp, intCnt, 1)) = False AndAlso Len(strTmp) > 0 _
    '                                         AndAlso Mid(strTmp, intCnt, 1) <> "-" Then
    '                strTmp = Replace(strTmp, Mid(strTmp, intCnt, 1), "")
    '            End If
    '        Next
    '        TBox.Text = strTmp
    '    Else
    '        PartSSNTextBox.MaxLength = 9
    '    End If
    'End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' looks up participant info when the user presses Enter
    ' </summary>
    ' <param name="sender"></param>
    ' <param name="e"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	1/22/2007	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub PartSSNTextBox_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles PartSSNTextBox.KeyUp
        Try
            If e.KeyCode = Keys.Enter Then
                ValidatePartSSN()
            End If
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            Else
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Load the "before" so participant can be validated
    ' </summary>
    ' <param name="sender"></param>
    ' <param name="e"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	1/23/2007	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub PartSSNTextBox_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles PartSSNTextBox.Validating
        Try
            _OrigPartSSN = _ClaimMasterDR("PART_SSN")

        Catch ex As Exception

            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            Else
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' looks up participant info
    ' </summary>
    ' <param name="sender"></param>
    ' <param name="e"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	1/19/2007	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub PartSSNTextBox_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles PartSSNTextBox.Validated
        Try

            ValidatePartSSN(PartSSNTextBox.Text)
            PartSSNTextBox.Text = FormatSSN(PartSSNTextBox.Text)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    '' -----------------------------------------------------------------------------
    '' <summary>
    '' looks up participant info
    '' </summary>
    '' <remarks>
    '' </remarks>
    '' <history>
    '' 	[Nick Snyder]	1/22/2007	Created
    '' </history>
    '' -----------------------------------------------------------------------------
    Private Sub ValidatePartSSN(Optional ByVal startingSSN As String = Nothing)
        Dim PartDR As DataRow
        Dim PartSSN As String
        Dim OrigSSN As String = startingSSN
        Dim SaveAndQuit As Boolean = False
        Dim CancelChanges As Boolean = False

        Try
            If IsNumeric(PartSSNTextBox.Text) Then : PartSSNTextBox.MaxLength = 9
            Else : PartSSNTextBox.MaxLength = 11
            End If

            If OrigSSN Is Nothing Then
                OrigSSN = CStr(_ClaimMasterDR("PART_SSN"))
            Else
                OrigSSN = UnFormatSSN(OrigSSN)
            End If

            ' PartSSNTextBox.Text = CStr(DecryptSSN(PartSSNTextBox.Text))
            PartSSN = UnFormatSSN(PartSSNTextBox.Text)

            If IsNumeric(PartSSN) Then
                PartDR = CMSDALFDBMD.RetrieveParticipantInfo(CInt(PartSSN))

                If PartDR IsNot Nothing Then
                    NextPanelButton.Enabled = True
                    If PartDR("SSNO").ToString.Trim <> PartSSN OrElse OrigSSN <> PartSSN OrElse PartDR("LAST_NAME").ToString.Trim <> Me.PartNameLastTextBox.Text.ToString.Trim Then
                        If CBool(PartDR("TRUST_SW")) = True Then
                            If UFCWGeneralAD.CMSCanAdjudicateEmployee() = False Then
                                If MessageBox.Show("You are not authorized to work on Employee Claims." &
                                                vbCrLf & "You must save changes back to the queue." & vbCrLf &
                                                "Are you sure you want to make this change?", "Confirm Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                                    SaveAndQuit = True
                                Else
                                    CancelChanges = True
                                    'PartSSNTextBox.Text = FormatSSN(CStr(_ClaimMasterDR("PART_SSN")))
                                End If
                            End If
                        End If

                        If SaveAndQuit = False AndAlso CancelChanges = False Then

                        End If

                        If CancelChanges = False Then
                            If IsDBNull(_ClaimMasterDR("FAMILY_ID")) = False AndAlso CInt(_ClaimMasterDR("FAMILY_ID")) <> CInt(PartDR("FAMILY_ID")) Then
                                FamilyIDTextBox.Text = CStr(PartDR("FAMILY_ID"))
                            End If

                            If IsDBNull(_ClaimMasterDR("PART_SSN")) = False AndAlso _ClaimMasterDR("PART_SSN").ToString.Trim <> PartDR("PART_SSNO").ToString.Trim Then
                                PartSSNTextBox.Text = FormatSSN(CStr(PartDR("PART_SSNO")))
                            End If

                            If (IsDBNull(_ClaimMasterDR("PART_FNAME")) = False AndAlso IsDBNull(PartDR("FIRST_NAME"))) OrElse (IsDBNull(_ClaimMasterDR("PART_FNAME")) AndAlso IsDBNull(PartDR("FIRST_NAME")) = False) Then
                                PartNameFirstTextBox.Text = CStr(PartDR("FIRST_NAME"))
                            ElseIf IsDBNull(_ClaimMasterDR("PART_FNAME")) = False AndAlso _ClaimMasterDR("PART_FNAME").ToString.Trim <> PartDR("FIRST_NAME").ToString.Trim Then
                                PartNameFirstTextBox.Text = CStr(PartDR("FIRST_NAME"))
                            End If

                            Try
                                PartNameMiddleTextBox.Text = CStr(PartDR("MIDDLE_INITIAL").ToString)
                            Catch ex As Exception
                                PartNameMiddleTextBox.Text = ""
                            End Try

                            If (IsDBNull(_ClaimMasterDR("PART_LNAME")) = False AndAlso IsDBNull(PartDR("LAST_NAME")) = True) OrElse (IsDBNull(_ClaimMasterDR("PART_LNAME")) = True AndAlso IsDBNull(PartDR("LAST_NAME")) = False) Then
                                PartNameLastTextBox.Text = CStr(PartDR("LAST_NAME"))
                            ElseIf IsDBNull(_ClaimMasterDR("PART_LNAME")) = False AndAlso _ClaimMasterDR("PART_LNAME").ToString.Trim <> PartDR("LAST_NAME").ToString.Trim Then
                                PartNameLastTextBox.Text = CStr(PartDR("LAST_NAME"))
                            End If
                            'This may be needed if requirements change to prevent a user from
                            'proceeding through UTL if the PartSSN is invalid on load
                            'If PartNameLastTextBox.Text = "UNKNOWN" Then
                            '    MessageBox.Show("SSN is UNKNOWN, please enter in a valid PartSSN!", "UNKNOWN PartSSN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            '    NextPanel.Enabled = False
                            'Else
                            '    NextPanel.Enabled = True
                            'End If
                            If IsDBNull(_ClaimMasterDR("SECURITY_SW")) = False AndAlso CBool(_ClaimMasterDR("SECURITY_SW")) <> CBool(PartDR("TRUST_SW")) Then
                                _ClaimMasterDR("SECURITY_SW") = PartDR("TRUST_SW")
                            End If

                            If CInt(PartDR("RELATION_ID")) <> 0 Then
                                If IsDBNull(_ClaimMasterDR("RELATION_ID")) = False AndAlso CInt(_ClaimMasterDR("RELATION_ID")) <> -1 Then
                                    _ClaimMasterDR("RELATION_ID") = -1
                                End If
                                If IsDBNull(_ClaimMasterDR("PAT_SSN")) = False AndAlso CInt(_ClaimMasterDR("PAT_SSN")) <> 0 Then
                                    _ClaimMasterDR("PAT_SSN") = 0
                                End If
                                If IsDBNull(_ClaimMasterDR("PAT_FNAME")) = False Then
                                    _ClaimMasterDR("PAT_FNAME") = DBNull.Value
                                End If
                                If IsDBNull(_ClaimMasterDR("PAT_INT")) = False Then
                                    _ClaimMasterDR("PAT_INT") = DBNull.Value
                                End If
                                If IsDBNull(_ClaimMasterDR("PAT_LNAME")) = False Then
                                    _ClaimMasterDR("PAT_LNAME") = DBNull.Value
                                End If
                                If _ClaimDS.MEDHDR.Rows.Count > 0 Then
                                    If IsDBNull(_MedHDRDR("PAT_DOB")) = False Then
                                        _MedHDRDR("PAT_DOB") = DBNull.Value
                                    End If

                                    If IsDBNull(_MedHDRDR("PAT_SEX")) = False Then
                                        _MedHDRDR("PAT_SEX") = DBNull.Value
                                    End If
                                End If

                            Else
                                'Added By malko: Relation ID is not updating on the UI, setting it here
                                Try
                                    PatRelationIDTextBox.Text = CStr(PartDR("RELATION_ID"))
                                Catch ex As Exception
                                    PatRelationIDTextBox.Text = ""
                                End Try

                                If IsDBNull(_ClaimMasterDR("PAT_SSN")) = False AndAlso _ClaimMasterDR("PAT_SSN").ToString.Trim <> PartSSN Then
                                    PatSSNTextBox.Text = FormatSSN(PartSSN)
                                End If

                                If (IsDBNull(_ClaimMasterDR("PAT_FNAME")) = False AndAlso IsDBNull(PartDR("FIRST_NAME")) = True) OrElse (IsDBNull(_ClaimMasterDR("PAT_FNAME")) = True AndAlso IsDBNull(PartDR("FIRST_NAME")) = False) Then
                                    PatNameFirstTextBox.Text = CStr(PartDR("FIRST_NAME"))
                                ElseIf IsDBNull(_ClaimMasterDR("PAT_FNAME")) = False AndAlso _ClaimMasterDR("PAT_FNAME").ToString.Trim <> PartDR("FIRST_NAME").ToString.Trim Then
                                    PatNameFirstTextBox.Text = CStr(PartDR("FIRST_NAME"))
                                End If
                                'Modified by malko: Testing for DBNull errors
                                Try
                                    PatNameMiddleTextBox.Text = CStr(PartDR("MIDDLE_INITIAL").ToString)
                                Catch ex As Exception
                                    PatNameMiddleTextBox.Text = ""
                                End Try

                                If (IsDBNull(_ClaimMasterDR("PAT_LNAME")) = False AndAlso IsDBNull(PartDR("LAST_NAME"))) OrElse (IsDBNull(_ClaimMasterDR("PAT_LNAME")) AndAlso IsDBNull(PartDR("LAST_NAME")) = False) Then
                                    PatNameLastTextBox.Text = CStr(PartDR("LAST_NAME"))
                                ElseIf IsDBNull(_ClaimMasterDR("PAT_LNAME")) = False AndAlso _ClaimMasterDR("PAT_LNAME").ToString.Trim <> PartDR("LAST_NAME").ToString.Trim Then
                                    PatNameLastTextBox.Text = CStr(PartDR("LAST_NAME"))
                                End If

                                If _ClaimDS.MEDHDR.Rows.Count > 0 Then
                                    If (IsDBNull(_MedHDRDR("PAT_DOB")) = False AndAlso IsDBNull(PartDR("BIRTH_DATE"))) OrElse (IsDBNull(_MedHDRDR("PAT_DOB")) AndAlso IsDBNull(PartDR("BIRTH_DATE")) = False) Then
                                        PatDOBTextBox.Text = CStr(PartDR("BIRTH_DATE").ToString)
                                    ElseIf IsDBNull(_MedHDRDR("PAT_DOB")) = False AndAlso CDate(_MedHDRDR("PAT_DOB")) <> CDate(PartDR("BIRTH_DATE")) Then
                                        PatDOBTextBox.Text = CStr(PartDR("BIRTH_DATE"))
                                    End If

                                    'Modified by malko: Testing for DBNull errors
                                    Try
                                        PatSexTextBox.Text = CStr(PartDR("GENDER"))
                                    Catch ex As Exception
                                        PatSexTextBox.Text = ""
                                    End Try
                                End If

                            End If
                            _ClaimMasterBS.EndEdit()
                            _MedHdrBS.EndEdit()

                        End If

                    End If
                Else
                    'If the user enters in an invalid PartSSN, return UNKNOWN to LastName and clear out
                    'UI controls for Patient and Participant, prevent from moving forward by disabling
                    'NextPanel Button until they fix the PartSSN
                    MessageBox.Show("SSN is UNKNOWN, please enter in a valid PartSSN!", "UNKNOWN PartSSN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    NextPanelButton.Enabled = False
                    PartNameLastTextBox.Text = "UNKNOWN"
                    PartNameFirstTextBox.Text = ""
                    PartNameMiddleTextBox.Text = ""
                    FamilyIDTextBox.Text = ""

                    PatNameFirstTextBox.Text = ""
                    PatNameLastTextBox.Text = ""
                    PatNameMiddleTextBox.Text = ""
                    PatRelationIDTextBox.Text = ""
                    PatSSNTextBox.Text = "0"
                    PatDOBTextBox.Text = "N/A"
                    PatSexTextBox.Text = ""
                    PatAcctNoTextBox.Text = ""
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    If IsDBNull(_ClaimMasterDR("PART_FNAME")) = False Then
                        _ClaimMasterDR("PART_FNAME") = DBNull.Value
                    End If
                    If IsDBNull(_ClaimMasterDR("PART_INT")) = False Then
                        _ClaimMasterDR("PART_INT") = DBNull.Value
                    End If
                    If IsDBNull(_ClaimMasterDR("PART_LNAME")) = False Then
                        _ClaimMasterDR("PART_LNAME") = DBNull.Value
                    End If

                    _ClaimMasterBS.EndEdit()
                    _MedHdrBS.EndEdit()
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    'Private Sub PatSSNTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PatSSNTextBox.TextChanged
    '    Dim TBox As TextBox = CType(sender, TextBox)
    '    Dim intCnt As Integer
    '    Dim strTmp As String

    '    If IsNumeric(TBox.Text) = False AndAlso Len(TBox.Text) > 0 Then
    '        PatSSNTextBox.MaxLength = 11
    '        strTmp = TBox.Text
    '        For intCnt = 1 To Len(strTmp)
    '            If IsNumeric(Mid(strTmp, intCnt, 1)) = False AndAlso Len(strTmp) > 0 _
    '                                         AndAlso Mid(strTmp, intCnt, 1) <> "-" Then
    '                strTmp = Replace(strTmp, Mid(strTmp, intCnt, 1), "")
    '            End If
    '        Next
    '        TBox.Text = strTmp
    '    Else
    '        PatSSNTextBox.MaxLength = 9
    '    End If
    'End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' looks up patient info when the user presses Enter
    ' </summary>
    ' <param name="sender"></param>
    ' <param name="e"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	1/22/2007	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub PatSSNTextBox_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles PatSSNTextBox.KeyUp
        Try
            If e.KeyCode = Keys.Enter Then
                ValidatePatientSSN()
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' looks up patient info
    ' </summary>
    ' <param name="sender"></param>
    ' <param name="e"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	1/19/2007	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub PatSSNTextBox_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles PatSSNTextBox.Validated
        Try
            ValidatePatientSSN()

            PatSSNTextBox.Text = FormatSSN(PatSSNTextBox.Text)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' looks up patient info
    ' </summary>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	1/22/2007	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub ValidatePatientSSN()
        Dim PatRow As DataRow
        Dim SSN As String

        Try
            If IsNumeric(PatSSNTextBox.Text) Then : PatSSNTextBox.MaxLength = 9
            Else : PatSSNTextBox.MaxLength = 11
            End If

            PatSSNTextBox.Text = CStr(DecryptSSN(PatSSNTextBox.Text))
            SSN = UnFormatSSN(PatSSNTextBox.Text)
            If IsNumeric(SSN) = True AndAlso IsNumeric(FamilyIDTextBox.Text) Then
                PatRow = CMSDALFDBMD.RetrievePatientInfo(CInt(FamilyIDTextBox.Text), CInt(SSN))

                If Not PatRow Is Nothing Then
                    If IsDBNull(_ClaimMasterDR("RELATION_ID")) = False AndAlso CInt(_ClaimMasterDR("RELATION_ID")) <> CInt(PatRow("RELATION_ID")) Then
                        PatRelationIDTextBox.Text = CStr(PatRow("RELATION_ID"))

                    End If

                    If IsDBNull(_ClaimMasterDR("PAT_SSN")) = False AndAlso _ClaimMasterDR("PAT_SSN").ToString.Trim <> PatRow("SSNO").ToString.Trim Then
                        PatSSNTextBox.Text = CStr(PatRow("SSNO"))
                    End If

                    If (IsDBNull(_ClaimMasterDR("PAT_FNAME")) = False AndAlso IsDBNull(PatRow("FIRST_NAME"))) OrElse (IsDBNull(_ClaimMasterDR("PAT_FNAME")) AndAlso IsDBNull(PatRow("FIRST_NAME")) = False) Then
                        PatNameFirstTextBox.Text = CStr(PatRow("FIRST_NAME"))
                    ElseIf IsDBNull(_ClaimMasterDR("PAT_FNAME")) = False AndAlso _ClaimMasterDR("PAT_FNAME").ToString.Trim <> PatRow("FIRST_NAME").ToString.Trim Then
                        PatNameFirstTextBox.Text = CStr(PatRow("FIRST_NAME"))
                    End If
                    'Modified by malko: Testing null values
                    Try
                        If (IsDBNull(_ClaimMasterDR("PAT_INT")) = False AndAlso IsDBNull(PatRow("MIDDLE_INITIAL"))) OrElse (IsDBNull(_ClaimMasterDR("PAT_INT")) AndAlso IsDBNull(PatRow("MIDDLE_INITIAL")) = False) Then
                            PatNameMiddleTextBox.Text = CStr(PatRow("MIDDLE_INITIAL"))
                        ElseIf IsDBNull(_ClaimMasterDR("PAT_INT")) = False AndAlso _ClaimMasterDR("PAT_INT").ToString.Trim <> PatRow("MIDDLE_INITIAL").ToString.Trim Then
                            PatNameMiddleTextBox.Text = CStr(PatRow("MIDDLE_INITIAL"))
                        End If

                    Catch ex As Exception
                        PatNameMiddleTextBox.Text = ""
                    End Try

                    If (IsDBNull(_ClaimMasterDR("PAT_LNAME")) = False AndAlso IsDBNull(PatRow("LAST_NAME")) = True) OrElse (IsDBNull(_ClaimMasterDR("PAT_LNAME")) = True AndAlso IsDBNull(PatRow("LAST_NAME")) = False) Then
                        PatNameLastTextBox.Text = CStr(PatRow("LAST_NAME"))
                    ElseIf IsDBNull(_ClaimMasterDR("PAT_LNAME")) = False AndAlso _ClaimMasterDR("PAT_LNAME").ToString.Trim <> PatRow("LAST_NAME").ToString.Trim Then
                        PatNameLastTextBox.Text = CStr(PatRow("LAST_NAME"))
                    End If

                    If _ClaimDS.MEDHDR.Rows.Count > 0 Then
                        If (IsDBNull(_MedHDRDR("PAT_DOB")) = False AndAlso IsDBNull(PatRow("BIRTH_DATE")) = True) OrElse (IsDBNull(_MedHDRDR("PAT_DOB")) AndAlso IsDBNull(PatRow("BIRTH_DATE")) = False) Then
                            _MedHDRDR("PAT_DOB") = PatRow("BIRTH_DATE")
                        ElseIf IsDBNull(_MedHDRDR("PAT_DOB")) = False AndAlso CDate(_MedHDRDR("PAT_DOB")) <> CDate(PatRow("BIRTH_DATE")) Then
                            _MedHDRDR("PAT_DOB") = PatRow("BIRTH_DATE")
                        End If

                        'Modified by malko: Testing null values
                        Try
                            If (IsDBNull(_MedHDRDR("PAT_SEX")) = False AndAlso IsDBNull(PatRow("GENDER"))) OrElse (IsDBNull(_MedHDRDR("PAT_SEX")) AndAlso IsDBNull(PatRow("GENDER")) = False) Then
                                _MedHDRDR("PAT_SEX") = PatRow("GENDER")
                            ElseIf IsDBNull(_MedHDRDR("PAT_SEX")) = False AndAlso _MedHDRDR("PAT_SEX").ToString.Trim <> PatRow("GENDER").ToString.Trim Then
                                _MedHDRDR("PAT_SEX") = PatRow("GENDER")
                            End If

                        Catch ex As Exception
                            _MedHDRDR("PAT_SEX") = ""
                        End Try

                    End If

                    _ClaimMasterBS.EndEdit()
                    _MedHdrBS.EndEdit()
                Else
                    If IsDBNull(_ClaimMasterDR("RELATION_ID")) = False AndAlso CInt(_ClaimMasterDR("RELATION_ID")) <> -1 Then
                        _ClaimMasterDR("RELATION_ID") = -1
                    End If

                    If IsDBNull(_ClaimMasterDR("PAT_FNAME")) = False Then
                        _ClaimMasterDR("PAT_FNAME") = DBNull.Value
                    End If

                    If IsDBNull(_ClaimMasterDR("PAT_INT")) = False Then
                        _ClaimMasterDR("PAT_INT") = DBNull.Value
                    End If

                    If IsDBNull(_ClaimMasterDR("PAT_LNAME")) = False Then
                        _ClaimMasterDR("PAT_LNAME") = DBNull.Value
                    End If
                    If _ClaimDS.MEDHDR.Rows.Count > 0 Then
                        If IsDBNull(_MedHDRDR("PAT_DOB")) = False Then
                            _MedHDRDR("PAT_DOB") = DBNull.Value
                        End If
                        If IsDBNull(_MedHDRDR("PAT_SEX")) = False Then
                            _MedHDRDR("PAT_SEX") = DBNull.Value
                        End If
                    End If
                    _ClaimMasterBS.EndEdit()
                    _MedHdrBS.EndEdit()

                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
#End Region

    Public Shared Function UnFormatSSN(ByVal strSSN As String) As String
        If Replace(Replace(Replace(strSSN, " ", ""), "-", ""), "/", "") <> "" Then
            Return Format(CLng(Replace(Replace(Replace(strSSN, " ", ""), "-", ""), "/", "")), "0########")
        Else
            Return ""
        End If
    End Function
    Public Shared Function FormatSSN(ByVal strSSN As String) As String
        Dim strTemp As String

        strTemp = UnFormatSSN(strSSN)
        If strTemp.Trim <> "" Then
            Return Microsoft.VisualBasic.Left(strTemp, 3) & "-" & Microsoft.VisualBasic.Mid(strTemp, 4, 2) & "-" & Microsoft.VisualBasic.Right(strTemp, 4)
        Else
            Return ""
        End If
    End Function

    'Private Sub ProviderTINTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProviderTINTextBox.TextChanged
    '    Dim TBox As TextBox = CType(sender, TextBox)
    '    Dim intCnt As Integer
    '    Dim strTmp As String

    '    If IsNumeric(TBox.Text) = False AndAlso Len(TBox.Text) > 0 Then
    '        strTmp = TBox.Text
    '        For intCnt = 1 To Len(strTmp)
    '            If IsNumeric(Mid(strTmp, intCnt, 1)) = False AndAlso Len(strTmp) > 0 _
    '                                         AndAlso Mid(strTmp, intCnt, 1) <> "-" Then
    '                strTmp = Replace(strTmp, Mid(strTmp, intCnt, 1), "")
    '            End If
    '        Next
    '        TBox.Text = strTmp
    '    End If
    'End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Looks up provider id and name when enter is pressed
    ' </summary>
    ' <param name="sender"></param>
    ' <param name="e"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	1/24/2007	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub ProviderTINTextBox_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ProviderTINTextBox.KeyUp
        Try
            If e.KeyCode = Keys.Enter Then
                ValidateProvTIN()
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Looks up provider id and name
    ' </summary>
    ' <param name="sender"></param>
    ' <param name="e"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	1/24/2007	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub ProviderTINTextBox_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProviderTINTextBox.Validated
        Try
            ValidateProvTIN()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Looks up provider id and name
    ' </summary>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	1/24/2007	Created
    '     [malko]        6/27/2007   stole from Nicks's code and put in here
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub ValidateProvTIN()

        Dim ProvDR As DataRow
        Dim TIN As String

        Try
            TIN = UnFormatTIN(ProviderTINTextBox.Text)
            If IsNumeric(TIN) Then
                ProvDR = CMSDALFDBMD.RetrieveCompleteProviderInfoByTIN(CInt(TIN))

                If ProvDR IsNot Nothing Then
                    If (IsDBNull(_ClaimMasterDR("PROV_ID")) = False AndAlso IsDBNull(ProvDR("PROVIDER_ID"))) OrElse (IsDBNull(_ClaimMasterDR("PROV_ID")) AndAlso IsDBNull(ProvDR("PROVIDER_ID")) = False) Then
                        _ClaimMasterDR("PROV_ID") = ProvDR("PROVIDER_ID")
                        ProviderIDTextBox.Text = CStr(ProvDR("PROVIDER_ID"))
                    ElseIf IsDBNull(_ClaimMasterDR("PROV_ID")) = False AndAlso _ClaimMasterDR("PROV_ID").ToString.Trim <> ProvDR("PROVIDER_ID").ToString.Trim Then
                        _ClaimMasterDR("PROV_ID") = ProvDR("PROVIDER_ID")
                        ProviderIDTextBox.Text = CStr(ProvDR("PROVIDER_ID"))
                    End If

                    If (IsDBNull(_ClaimMasterDR("PROV_NAME")) = False AndAlso IsDBNull(ProvDR("NAME1"))) OrElse (IsDBNull(_ClaimMasterDR("PROV_NAME")) AndAlso IsDBNull(ProvDR("NAME1")) = False) Then
                        _ClaimMasterDR("PROV_NAME") = ProvDR("NAME1")
                    ElseIf IsDBNull(_ClaimMasterDR("PROV_NAME")) = False AndAlso _ClaimMasterDR("PROV_NAME").ToString.Trim <> ProvDR("NAME1").ToString.Trim Then
                        _ClaimMasterDR("PROV_NAME") = ProvDR("NAME1")
                    End If

                    If (IsDBNull(_ClaimMasterDR("PROV_TIN")) = False AndAlso IsDBNull(ProvDR("TAXID"))) OrElse (IsDBNull(_ClaimMasterDR("PROV_TIN")) AndAlso IsDBNull(ProvDR("TAXID")) = False) Then
                        ProviderTINTextBox.Text = CStr(ProvDR("TAXID"))
                    ElseIf IsDBNull(_ClaimMasterDR("PROV_NAME")) = False AndAlso _ClaimMasterDR("PROV_TIN").ToString.Trim <> ProvDR("TAXID").ToString.Trim Then
                        ProviderTINTextBox.Text = CStr(ProvDR("TAXID"))
                    End If

                    BCCZIPTextBox.Text = CStr(If(IsDBNull(ProvDR("ZIP")), "", ProvDR("ZIP")))

                    ProviderLicenseNoTextBox.Text = CStr(If(IsDBNull(ProvDR("LICENSE")), "", ProvDR("LICENSE")))
                    ProviderRenderingNPITextBox.Text = CStr(If(IsDBNull(ProvDR("NPI")), "", ProvDR("NPI")))

                    Me.BindingContext(_ClaimDS.CLAIM_MASTER).EndCurrentEdit()
                Else
                    If IsDBNull(_ClaimMasterDR("PROV_ID")) = False Then
                        _ClaimMasterDR("PROV_ID") = DBNull.Value
                    End If

                    If IsDBNull(_ClaimMasterDR("PROV_NAME")) = False AndAlso _ClaimMasterDR("PROV_NAME").ToString.Trim <> "***INVALID PROVIDER***" Then
                        _ClaimMasterDR("PROV_NAME") = "***INVALID PROVIDER***"
                    End If

                    If IsDBNull(_ClaimMasterDR("PROV_TIN")) = False AndAlso _ClaimMasterDR("PROV_TIN").ToString.Trim <> TIN Then
                        _ClaimMasterDR("PROV_TIN") = TIN
                    End If

                    Me.BindingContext(_ClaimDS.CLAIM_MASTER).EndCurrentEdit()
                End If
            Else
                If IsDBNull(_ClaimMasterDR("PROV_ID")) = False Then
                    _ClaimMasterDR("PROV_ID") = DBNull.Value
                End If

                If IsDBNull(_ClaimMasterDR("PROV_NAME")) = False AndAlso _ClaimMasterDR("PROV_NAME").ToString.Trim <> "***INVALID PROVIDER***" Then
                    _ClaimMasterDR("PROV_NAME") = "***INVALID PROVIDER***"
                End If

                If IsDBNull(_ClaimMasterDR("PROV_TIN")) = False Then
                    _ClaimMasterDR("PROV_TIN") = DBNull.Value
                End If

                Me.BindingContext(_ClaimDS.CLAIM_MASTER).EndCurrentEdit()
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' formats an TIN
    ' </summary>
    ' <param name="tin"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[nick snyder]	8/16/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function FormatTIN(ByVal tin As String) As String
        tin = UnFormatTIN(tin)

        If IsNumeric(tin) = True Then
            tin = Format(CLng(tin), "00-0000000")
        End If

        Return tin
    End Function

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Unformats an TIN
    ' </summary>
    ' <param name="tin"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[nick snyder]	8/16/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function UnFormatTIN(ByVal tin As String) As String
        tin = Replace(Replace(Replace(tin.ToUpper.Trim, "/", ""), "-", ""), " ", "")
        tin = UnFormatSSN(tin)

        Return tin
    End Function
    Private Sub TINBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' adjusts TIN values entered for a databinding
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
            If Not IsDBNull(e.Value) AndAlso e.Value.ToString.Trim.Length = 0 Then
                e.Value = DBNull.Value
            ElseIf Not IsDBNull(e.Value) AndAlso Not IsNumeric(e.Value) Then
                e.Value = UFCWGeneral.UnFormatTIN(CStr(e.Value))
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub TINBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' formats TIN values entered for a databinding
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
                e.Value = UFCWGeneral.FormatTIN(CStr(e.Value))
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub
    Private Sub ZipBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Dim eOriginal As String = e.Value.ToString

        Try
            If Not IsDBNull(e.Value) AndAlso e.Value IsNot Nothing AndAlso e.Value.ToString.Trim.Length > 0 AndAlso CStr(e.Value) <> String.Format("{0:00000}", e.Value) Then
                e.Value = String.Format("{0:00000}", e.Value)
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub ZipBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)

        Dim eOriginal As String = e.Value.ToString

        Try
            If e.Value.ToString.Trim.Length < 1 Then
                e.Value = System.DBNull.Value
            Else
                e.Value = String.Format("{0:00000}", e.Value)
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub ProviderRenderingNPIBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Dim eOriginal As String = e.Value.ToString

        Try
            If Not IsDBNull(e.Value) AndAlso e.Value IsNot Nothing AndAlso e.Value.ToString.Trim.Length > 0 Then
                e.Value = String.Format("{0:0000000000}", e.Value)
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub ProviderRenderingNPIBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Dim eOriginal As String = e.Value.ToString

        Try
            If e.Value.ToString.Trim.Length < 1 Then
                e.Value = System.DBNull.Value
            Else
                e.Value = String.Format("{0:0000000000}", e.Value)
            End If

        Catch ex As Exception
            Throw
        Finally
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

        End Try
    End Sub
    Private Sub SetAnnotationsControl()
        Try

            With AnnotationsControl
                .ClaimID = If(ClaimIDTextBox.Text.Trim = "", 0, CInt(ClaimIDTextBox.Text))
                .FamilyID = If(FamilyIDTextBox.Text.Trim = "", 0, CInt(FamilyIDTextBox.Text))
                .ParticipantFirst = PartNameFirstTextBox.Text
                .ParticipantLast = PartNameLastTextBox.Text
                .ParticipantSSN = If(PartSSNTextBox.Text.Trim = "", 0, CInt(UnFormatSSN(PartSSNTextBox.Text)))
                .PatientFirst = PatNameFirstTextBox.Text
                .PatientLast = PatNameLastTextBox.Text
                .PatientSSN = If(PatSSNTextBox.Text.Trim = "", 0, CInt(UnFormatSSN(PatSSNTextBox.Text)))
                .RelationID = If(PatRelationIDTextBox.Text.Trim = "", 0, CInt(UnFormatSSN(PatRelationIDTextBox.Text)))
                .Annotations = _ClaimDS.ANNOTATIONS
                .Refresh()
                .Annotations.AcceptChanges()
            End With

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub AnnotationsControl_Save(sender As Object, e As AnnotationsEvent) Handles AnnotationsControl.Save
        Dim AddRow As DataRow
        Dim DT As System.Data.DataTable

        Try

            DT = Me.AnnotationsControl.Annotations.GetChanges(DataRowState.Added)

            If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
                For Each DR As DataRow In DT.Rows
                    AddRow = _ClaimDS.ANNOTATIONS.NewRow

                    For Each DC As DataColumn In _ClaimDS.ANNOTATIONS.Columns
                        If DC.AutoIncrement = False Then
                            AddRow.Item(DC.ColumnName) = DR(DC.ColumnName)
                        End If
                    Next

                    _ClaimDS.ANNOTATIONS.Rows.Add(AddRow)
                Next
            End If

            Me.AnnotationsControl.Annotations.AcceptChanges()

            _AnnotationAdded = True

            FinishWizardButton.Enabled = True

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub UTLUtility_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        Dim Transaction As DbTransaction

        Try
            Transaction = CMSDALCommon.BeginTransaction

            CMSDALFDBMD.UnBusyItem(CInt(_ClaimMasterDR("CLAIM_ID")), _DomainUser.ToUpper, Transaction)
            CMSDALFDBMD.ReleaseFamilyLock(CInt(_ClaimMasterDR("FAMILY_ID")), Transaction)

            CMSDALCommon.CommitTransaction(Transaction)

        Catch ex As Exception
            If Transaction IsNot Nothing Then
                CMSDALCommon.RollbackTransaction(Transaction)
            End If
            Throw
        End Try

    End Sub

    Private Sub PartSSNTextBox_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PartSSNTextBox.Click
        PartSSNTextBox.Text = UnFormatSSN(PartSSNTextBox.Text)
    End Sub

    Private Sub PatSSNTextBox_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PatSSNTextBox.Click
        PatSSNTextBox.Text = UnFormatSSN(PatSSNTextBox.Text)
    End Sub

    Private Sub StepTab_SelectedIndexChanged(sender As Object, e As EventArgs) Handles StepTab.SelectedIndexChanged
        DisplayPanel(0)
    End Sub

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
    Private Sub SaveSettings()
        Dim lWindowState As FormWindowState = Me.WindowState
        SaveSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(lWindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = lWindowState

        SaveSetting(_APPKEY, Me.Name & "\Settings", "FontName", Me.Font.Name)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "FontSize", CStr(Me.Font.Size))
        SaveSetting(_APPKEY, Me.Name & "\Settings", "FontStyle", CStr(Me.Font.Style))
        SaveSetting(_APPKEY, Me.Name & "\Settings", "FontUnit", CStr(Me.Font.Unit))
        SaveSetting(_APPKEY, Me.Name & "\Settings", "FontCharset", CStr(Me.Font.GdiCharSet))

    End Sub

    Private Sub SetSettings()
        Dim FName As String = ""
        Dim FSize As Single
        Dim FStyle As New FontStyle
        Dim FUnit As New GraphicsUnit
        Dim FCharset As Byte

        Me.Visible = False

        Me.Top = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)))
        Me.Height = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)))
        Me.Width = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)

        FName = GetSetting(_APPKEY, Me.Name & "Settings", "FontName", Me.Font.Name)
        FSize = CSng(GetSetting(_APPKEY, Me.Name & "Settings", "FontSize", CStr(Me.Font.Size)))
        FStyle = CType(GetSetting(_APPKEY, Me.Name & "Settings", "FontStyle", CStr(Me.Font.Style)), FontStyle)
        FUnit = CType(GetSetting(_APPKEY, Me.Name & "Settings", "FontUnit", CStr(Me.Font.Unit)), GraphicsUnit)
        FCharset = CByte(GetSetting(_APPKEY, Me.Name & "Settings", "FontCharset", CStr(Me.Font.GdiCharSet)))

        Me.Font = New Font(FName, FSize, FStyle, FUnit, FCharset)

    End Sub

End Class