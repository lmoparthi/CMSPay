Option Strict On

Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Data.Common
Imports System.Text.RegularExpressions
Imports SharedInterfaces

Public Class NewClaimForm
    Inherits System.Windows.Forms.Form

    Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    ReadOnly _DomainUser As String = SystemInformation.UserName.ToUpper

    Private _APPKEY As String = "UFCW\Claims\"
    Private _Message As IMessage

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
    Friend WithEvents DocumentGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents DocClassComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents DocClassLabel As System.Windows.Forms.Label
    Friend WithEvents DocumentTypeLabel As System.Windows.Forms.Label
    Friend WithEvents DocTypeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents ProviderGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents ProviderTINLabel As System.Windows.Forms.Label
    Friend WithEvents ProviderIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents OkButton As System.Windows.Forms.Button
    Friend Shadows WithEvents CancelButton As System.Windows.Forms.Button
    Friend WithEvents ToolTip As EnhancedToolTip
    Friend WithEvents FamilyGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents RelationIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents RelationIDLabel As System.Windows.Forms.Label
    Friend WithEvents FamilyIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents FamilyIDLabel As System.Windows.Forms.Label
    Friend WithEvents PatientLookupButton As System.Windows.Forms.Button
    Friend WithEvents AssociatedGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents PatientHistoryButton As System.Windows.Forms.Button
    Friend WithEvents DCNTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ClaimIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents JAACheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents ProviderLookupButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.DocumentGroupBox = New System.Windows.Forms.GroupBox()
        Me.DocTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.DocClassComboBox = New System.Windows.Forms.ComboBox()
        Me.DocumentTypeLabel = New System.Windows.Forms.Label()
        Me.DocClassLabel = New System.Windows.Forms.Label()
        Me.ProviderGroupBox = New System.Windows.Forms.GroupBox()
        Me.ProviderLookupButton = New System.Windows.Forms.Button()
        Me.ProviderIDTextBox = New System.Windows.Forms.TextBox()
        Me.ProviderTINLabel = New System.Windows.Forms.Label()
        Me.OkButton = New System.Windows.Forms.Button()
        Me.CancelButton = New System.Windows.Forms.Button()
        Me.ToolTip = New EnhancedToolTip(Me.components)
        Me.PatientLookupButton = New System.Windows.Forms.Button()
        Me.PatientHistoryButton = New System.Windows.Forms.Button()
        Me.DCNTextBox = New System.Windows.Forms.TextBox()
        Me.ClaimIDTextBox = New System.Windows.Forms.TextBox()
        Me.JAACheckBox = New System.Windows.Forms.CheckBox()
        Me.FamilyGroupBox = New System.Windows.Forms.GroupBox()
        Me.RelationIDTextBox = New System.Windows.Forms.TextBox()
        Me.RelationIDLabel = New System.Windows.Forms.Label()
        Me.FamilyIDTextBox = New System.Windows.Forms.TextBox()
        Me.FamilyIDLabel = New System.Windows.Forms.Label()
        Me.AssociatedGroupBox = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.DocumentGroupBox.SuspendLayout()
        Me.ProviderGroupBox.SuspendLayout()
        Me.FamilyGroupBox.SuspendLayout()
        Me.AssociatedGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'DocumentGroupBox
        '
        Me.DocumentGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DocumentGroupBox.Controls.Add(Me.DocTypeComboBox)
        Me.DocumentGroupBox.Controls.Add(Me.DocClassComboBox)
        Me.DocumentGroupBox.Controls.Add(Me.DocumentTypeLabel)
        Me.DocumentGroupBox.Controls.Add(Me.DocClassLabel)
        Me.DocumentGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.DocumentGroupBox.Location = New System.Drawing.Point(16, 110)
        Me.DocumentGroupBox.Name = "DocumentGroupBox"
        Me.DocumentGroupBox.Size = New System.Drawing.Size(253, 86)
        Me.DocumentGroupBox.TabIndex = 1
        Me.DocumentGroupBox.TabStop = False
        Me.DocumentGroupBox.Text = "Document"
        '
        'DocTypeComboBox
        '
        Me.DocTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.DocTypeComboBox.DropDownWidth = 300
        Me.DocTypeComboBox.Location = New System.Drawing.Point(80, 51)
        Me.DocTypeComboBox.Name = "DocTypeComboBox"
        Me.DocTypeComboBox.Size = New System.Drawing.Size(152, 21)
        Me.DocTypeComboBox.TabIndex = 1
        '
        'DocClassComboBox
        '
        Me.DocClassComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.DocClassComboBox.Location = New System.Drawing.Point(80, 20)
        Me.DocClassComboBox.Name = "DocClassComboBox"
        Me.DocClassComboBox.Size = New System.Drawing.Size(152, 21)
        Me.DocClassComboBox.TabIndex = 0
        '
        'DocumentTypeLabel
        '
        Me.DocumentTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.DocumentTypeLabel.Location = New System.Drawing.Point(16, 54)
        Me.DocumentTypeLabel.Name = "DocumentTypeLabel"
        Me.DocumentTypeLabel.Size = New System.Drawing.Size(65, 14)
        Me.DocumentTypeLabel.TabIndex = 0
        Me.DocumentTypeLabel.Text = "Doc Type"
        '
        'DocClassLabel
        '
        Me.DocClassLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.DocClassLabel.Location = New System.Drawing.Point(16, 23)
        Me.DocClassLabel.Name = "DocClassLabel"
        Me.DocClassLabel.Size = New System.Drawing.Size(65, 14)
        Me.DocClassLabel.TabIndex = 2
        Me.DocClassLabel.Text = "Doc Class"
        '
        'ProviderGroupBox
        '
        Me.ProviderGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProviderGroupBox.Controls.Add(Me.ProviderLookupButton)
        Me.ProviderGroupBox.Controls.Add(Me.ProviderIDTextBox)
        Me.ProviderGroupBox.Controls.Add(Me.ProviderTINLabel)
        Me.ProviderGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ProviderGroupBox.Location = New System.Drawing.Point(16, 202)
        Me.ProviderGroupBox.Name = "ProviderGroupBox"
        Me.ProviderGroupBox.Size = New System.Drawing.Size(253, 60)
        Me.ProviderGroupBox.TabIndex = 2
        Me.ProviderGroupBox.TabStop = False
        Me.ProviderGroupBox.Text = "Provider"
        '
        'ProviderLookupButton
        '
        Me.ProviderLookupButton.Location = New System.Drawing.Point(209, 23)
        Me.ProviderLookupButton.Name = "ProviderLookupButton"
        Me.ProviderLookupButton.Size = New System.Drawing.Size(33, 21)
        Me.ProviderLookupButton.TabIndex = 2
        Me.ProviderLookupButton.Text = "?"
        Me.ToolTip.SetToolTip(Me.ProviderLookupButton, "Click to search for Provider")
        '
        'ProviderIDTextBox
        '
        Me.ProviderIDTextBox.BackColor = System.Drawing.SystemColors.Window
        Me.ProviderIDTextBox.Location = New System.Drawing.Point(80, 24)
        Me.ProviderIDTextBox.MaxLength = 9
        Me.ProviderIDTextBox.Name = "ProviderIDTextBox"
        Me.ProviderIDTextBox.ReadOnly = True
        Me.ProviderIDTextBox.Size = New System.Drawing.Size(123, 20)
        Me.ProviderIDTextBox.TabIndex = 1
        Me.ToolTip.SetToolTip(Me.ProviderIDTextBox, "Use Provider Search option (?) to select a valid provider")
        Me.ToolTip.SetToolTipWhenDisabled(Me.ProviderIDTextBox, "Use Provider Search option (?) to select a valid provider" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10))
        '
        'ProviderTINLabel
        '
        Me.ProviderTINLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ProviderTINLabel.Location = New System.Drawing.Point(16, 28)
        Me.ProviderTINLabel.Name = "ProviderTINLabel"
        Me.ProviderTINLabel.Size = New System.Drawing.Size(65, 14)
        Me.ProviderTINLabel.TabIndex = 1
        Me.ProviderTINLabel.Text = "Provider ID"
        '
        'OkButton
        '
        Me.OkButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.OkButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.OkButton.Location = New System.Drawing.Point(63, 385)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(75, 23)
        Me.OkButton.TabIndex = 4
        Me.OkButton.Text = "OK"
        '
        'CancelButton
        '
        Me.CancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CancelButton.Location = New System.Drawing.Point(144, 385)
        Me.CancelButton.Name = "CancelButton"
        Me.CancelButton.Size = New System.Drawing.Size(75, 23)
        Me.CancelButton.TabIndex = 5
        Me.CancelButton.Text = "Cancel"
        '
        'ToolTip
        '
        Me.ToolTip.ShowAlways = True
        '
        'PatientLookupButton
        '
        Me.PatientLookupButton.Location = New System.Drawing.Point(209, 20)
        Me.PatientLookupButton.Name = "PatientLookupButton"
        Me.PatientLookupButton.Size = New System.Drawing.Size(33, 21)
        Me.PatientLookupButton.TabIndex = 3
        Me.PatientLookupButton.Text = "?"
        Me.ToolTip.SetToolTip(Me.PatientLookupButton, "Click to search for Patient")
        '
        'PatientHistoryButton
        '
        Me.PatientHistoryButton.Location = New System.Drawing.Point(209, 19)
        Me.PatientHistoryButton.Name = "PatientHistoryButton"
        Me.PatientHistoryButton.Size = New System.Drawing.Size(33, 21)
        Me.PatientHistoryButton.TabIndex = 1
        Me.PatientHistoryButton.Text = "?"
        Me.ToolTip.SetToolTip(Me.PatientHistoryButton, "Click to search for Claim/DCN")
        '
        'DCNTextBox
        '
        Me.DCNTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.DCNTextBox.Location = New System.Drawing.Point(62, 50)
        Me.DCNTextBox.MaxLength = 15
        Me.DCNTextBox.Name = "DCNTextBox"
        Me.DCNTextBox.Size = New System.Drawing.Size(123, 20)
        Me.DCNTextBox.TabIndex = 2
        Me.ToolTip.SetToolTip(Me.DCNTextBox, "DCN of Associated Anthem Claim")
        '
        'ClaimIDTextBox
        '
        Me.ClaimIDTextBox.Location = New System.Drawing.Point(62, 21)
        Me.ClaimIDTextBox.MaxLength = 8
        Me.ClaimIDTextBox.Name = "ClaimIDTextBox"
        Me.ClaimIDTextBox.Size = New System.Drawing.Size(86, 20)
        Me.ClaimIDTextBox.TabIndex = 0
        Me.ToolTip.SetToolTip(Me.ClaimIDTextBox, "Claim# that this new claim is associated to")
        '
        'JAACheckBox
        '
        Me.JAACheckBox.AutoSize = True
        Me.JAACheckBox.Location = New System.Drawing.Point(197, 53)
        Me.JAACheckBox.Name = "JAACheckBox"
        Me.JAACheckBox.Size = New System.Drawing.Size(45, 17)
        Me.JAACheckBox.TabIndex = 3
        Me.JAACheckBox.Text = "JAA"
        Me.ToolTip.SetToolTip(Me.JAACheckBox, "By checking this box, you are identifying this claim as a JAA claim. Provider Che" &
        "cks will be issued by Anthem")
        Me.JAACheckBox.UseVisualStyleBackColor = True
        '
        'FamilyGroupBox
        '
        Me.FamilyGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyGroupBox.Controls.Add(Me.PatientLookupButton)
        Me.FamilyGroupBox.Controls.Add(Me.RelationIDTextBox)
        Me.FamilyGroupBox.Controls.Add(Me.RelationIDLabel)
        Me.FamilyGroupBox.Controls.Add(Me.FamilyIDTextBox)
        Me.FamilyGroupBox.Controls.Add(Me.FamilyIDLabel)
        Me.FamilyGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.FamilyGroupBox.Location = New System.Drawing.Point(16, 16)
        Me.FamilyGroupBox.Name = "FamilyGroupBox"
        Me.FamilyGroupBox.Size = New System.Drawing.Size(253, 88)
        Me.FamilyGroupBox.TabIndex = 0
        Me.FamilyGroupBox.TabStop = False
        Me.FamilyGroupBox.Text = "Family Information"
        '
        'RelationIDTextBox
        '
        Me.RelationIDTextBox.Location = New System.Drawing.Point(80, 50)
        Me.RelationIDTextBox.MaxLength = 3
        Me.RelationIDTextBox.Name = "RelationIDTextBox"
        Me.RelationIDTextBox.Size = New System.Drawing.Size(123, 20)
        Me.RelationIDTextBox.TabIndex = 4
        Me.ToolTip.SetToolTip(Me.RelationIDTextBox, "A valid Relation ID, use the patient search (?) if not known" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10))
        Me.ToolTip.SetToolTipWhenDisabled(Me.RelationIDTextBox, "A valid Relation ID, use the patient search (?) if not known")
        '
        'RelationIDLabel
        '
        Me.RelationIDLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.RelationIDLabel.Location = New System.Drawing.Point(16, 53)
        Me.RelationIDLabel.Name = "RelationIDLabel"
        Me.RelationIDLabel.Size = New System.Drawing.Size(65, 16)
        Me.RelationIDLabel.TabIndex = 2
        Me.RelationIDLabel.Text = "Relation ID"
        '
        'FamilyIDTextBox
        '
        Me.FamilyIDTextBox.Location = New System.Drawing.Point(80, 21)
        Me.FamilyIDTextBox.MaxLength = 9
        Me.FamilyIDTextBox.Name = "FamilyIDTextBox"
        Me.FamilyIDTextBox.Size = New System.Drawing.Size(123, 20)
        Me.FamilyIDTextBox.TabIndex = 2
        Me.ToolTip.SetToolTip(Me.FamilyIDTextBox, "A valid Family ID, use the patient search (?) if not known")
        Me.ToolTip.SetToolTipWhenDisabled(Me.FamilyIDTextBox, "A valid Family ID, use the patient search (?) if not known")
        '
        'FamilyIDLabel
        '
        Me.FamilyIDLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.FamilyIDLabel.Location = New System.Drawing.Point(16, 24)
        Me.FamilyIDLabel.Name = "FamilyIDLabel"
        Me.FamilyIDLabel.Size = New System.Drawing.Size(65, 16)
        Me.FamilyIDLabel.TabIndex = 1
        Me.FamilyIDLabel.Text = "Family ID"
        '
        'AssociatedGroupBox
        '
        Me.AssociatedGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AssociatedGroupBox.Controls.Add(Me.JAACheckBox)
        Me.AssociatedGroupBox.Controls.Add(Me.PatientHistoryButton)
        Me.AssociatedGroupBox.Controls.Add(Me.DCNTextBox)
        Me.AssociatedGroupBox.Controls.Add(Me.Label1)
        Me.AssociatedGroupBox.Controls.Add(Me.ClaimIDTextBox)
        Me.AssociatedGroupBox.Controls.Add(Me.Label2)
        Me.AssociatedGroupBox.Enabled = False
        Me.AssociatedGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.AssociatedGroupBox.Location = New System.Drawing.Point(16, 268)
        Me.AssociatedGroupBox.Name = "AssociatedGroupBox"
        Me.AssociatedGroupBox.Size = New System.Drawing.Size(253, 88)
        Me.AssociatedGroupBox.TabIndex = 3
        Me.AssociatedGroupBox.TabStop = False
        Me.AssociatedGroupBox.Text = "Claim Association(s)"
        '
        'Label1
        '
        Me.Label1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label1.Location = New System.Drawing.Point(16, 53)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(65, 16)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "DCN"
        '
        'Label2
        '
        Me.Label2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label2.Location = New System.Drawing.Point(16, 24)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(65, 16)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Claim ID"
        '
        'NewClaimForm
        '
        Me.AcceptButton = Me.OkButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(281, 420)
        Me.Controls.Add(Me.AssociatedGroupBox)
        Me.Controls.Add(Me.FamilyGroupBox)
        Me.Controls.Add(Me.CancelButton)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.ProviderGroupBox)
        Me.Controls.Add(Me.DocumentGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "NewClaimForm"
        Me.Text = "New Claim Information"
        Me.DocumentGroupBox.ResumeLayout(False)
        Me.ProviderGroupBox.ResumeLayout(False)
        Me.ProviderGroupBox.PerformLayout()
        Me.FamilyGroupBox.ResumeLayout(False)
        Me.FamilyGroupBox.PerformLayout()
        Me.AssociatedGroupBox.ResumeLayout(False)
        Me.AssociatedGroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    <System.ComponentModel.Description("Represents the application key used when accessing the registry.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
        End Set
    End Property

    Private Sub NewClaimForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        SaveSettings()

    End Sub

    Private Sub NewClaimForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim DocTypesDT As DataTable
        Dim SortedDT As DataTable

        Try
            DocTypesDT = CMSDALFDBMD.RetrieveUserDocTypes(_DomainUser)
            Me.DocTypeComboBox.DataSource = DocTypesDT
            Me.DocTypeComboBox.ValueMember = "DOC_TYPE"
            Me.DocTypeComboBox.DisplayMember = "DOC_TYPE"

            SortedDT = CMSDALCommon.SelectDistinctAndSorted("", DocTypesDT, "DOC_CLASS")
            Me.DocClassComboBox.DataSource = SortedDT
            Me.DocClassComboBox.ValueMember = "DOC_CLASS"
            Me.DocClassComboBox.DisplayMember = "DOC_CLASS"

            SetSettings()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub OkButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OkButton.Click

        Dim Transaction As DbTransaction
        Dim ClaimID As Integer
        Dim HistSum As String
        Dim HistDetail As String

        Try

            OkButton.Enabled = False

            If IsFormValid() Then

                Transaction = CMSDALCommon.BeginTransaction

                ClaimID = CMSDALFDBMD.CreateNewClaim(CInt(FamilyIDTextBox.Text), CInt(RelationIDTextBox.Text), DocClassComboBox.Text, DocTypeComboBox.Text, CInt(ProviderIDTextBox.Text), If(ClaimIDTextBox.Text.ToString.Trim.Length > 0, CInt(ClaimIDTextBox.Text.Trim), Nothing), DCNTextBox.Text.ToString.Trim, JAACheckBox.Checked, SystemInformation.UserName)

                If ClaimID > 0 Then
                    HistSum = "CLAIM ID " & ClaimID & " HAS BEEN CREATED"
                    HistDetail = "USER " & SystemInformation.UserName.ToUpper & " MANUALLY CREATED THIS CLAIM."

                    CMSDALFDBMD.CreateDocHistory(ClaimID, Nothing, "NEW", CInt(FamilyIDTextBox.Text), CShort(RelationIDTextBox.Text), -1, -1, DocClassComboBox.Text, DocTypeComboBox.Text, HistSum, HistDetail, SystemInformation.UserName.ToUpper)
                    CType(Me.Owner.ActiveControl, UFCW.CMS.UI.WorkQueue).OpenWorkScreen("", DocClassComboBox.Text, ClaimID, False, False, Nothing)
                End If

                CMSDALCommon.CommitTransaction(Transaction)

                Me.Close()

            End If

        Catch ex As Exception

            If Transaction IsNot Nothing Then
                CMSDALCommon.RollbackTransaction(Transaction)
            End If
            Throw

        Finally

            OkButton.Enabled = True
        End Try
    End Sub

    Private Function IsFormValid() As Boolean
        Dim EligibilityDS As DataSet
        Try

            'make sure they are both numeric
            If Not IsNumeric(Me.FamilyIDTextBox.Text) OrElse Not IsNumeric(Me.RelationIDTextBox.Text) Then
                MessageBox.Show("Validate that all required information is present", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If

            'validate that the family exists.
            EligibilityDS = CMSDALFDBMD.GetEligibilityInformation(CInt(FamilyIDTextBox.Text), CShort(RelationIDTextBox.Text))

            If EligibilityDS Is Nothing OrElse EligibilityDS.Tables(0).Rows.Count < 1 Then
                MessageBox.Show("FamilyID/RelationID was not found in Eligibility system.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If

            If Me.ProviderIDTextBox.Text.Length < 1 Then
                MessageBox.Show("Validate that all required information is present", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If

            If DCNTextBox.Text.ToString.Trim.Length > 0 Then
                If DCNTextBox.Text.ToString.Trim.Length < 13 OrElse Not DCNTextBox.Text.ToString.StartsWith("20") OrElse Not (DCNTextBox.Text.ToString.EndsWith("80") OrElse DCNTextBox.Text.ToString.EndsWith("84")) Then
                    MessageBox.Show("DCN is a 13 character identifier starting with '20' and ending with 80/84", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return False
                End If
            End If

            If JAACheckBox.Checked AndAlso Not DCNTextBox.Text.ToString.Trim.Length > 0 Then
                MessageBox.Show("JAA Claim requires a WGS/Anthem DCN", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If

            If ClaimIDTextBox.Text.ToString.Trim.Length > 0 OrElse DCNTextBox.Text.ToString.Trim.Length > 0 Then
                Select Case CMSDALFDBMD.ValidatePriorClaim(UFCWGeneral.IsNullIntegerHandler(ClaimIDTextBox.Text), UFCWGeneral.IsNullStringHandler(DCNTextBox.Text))
                    Case 4, 5

                        If MessageBox.Show("Claim with specified DCN already exists in CMS." & Environment.NewLine & "Please Confirm that you are creating this claim as an adjustment to an existing CMS/WGS Claim", "Claim with matching DCN found!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                        Else
                            Return False
                        End If

                    Case 3 'Claim Found, No DCN search

                    Case 0, 2 ' ClaimID not found

                        MessageBox.Show("Specified Claim# was not found (DCN was matched)", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return False

                    Case 1 ' Claim matched but not DCN

                    Case 6 ' DCN not found, no Claim search

                End Select
            End If

            Return True

        Catch ex As Exception
            Throw
        End Try

    End Function
    Private Sub DocTypeComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DocTypeComboBox.SelectedIndexChanged
        ToolTip.SetToolTip(Me.DocTypeComboBox, Me.DocTypeComboBox.Text)
    End Sub

    Private Sub ProvLookupButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProviderLookupButton.Click

        Dim ProviderLookUpForm As ProviderLookUpForm

        Try

            If Me.ProviderIDTextBox.Text.Trim.Length > 0 Then
                ProviderLookUpForm = New ProviderLookUpForm(CInt(Me.ProviderIDTextBox.Text))
            Else
                ProviderLookUpForm = New ProviderLookUpForm()
            End If

            If ProviderLookUpForm.ShowDialog(Me) = DialogResult.OK Then

                If Not IsDBNull(ProviderLookUpForm.SelectedProvider("PROVIDER_ID")) Then
                    Me.ProviderIDTextBox.Text = CStr(ProviderLookUpForm.SelectedProvider("PROVIDER_ID"))
                End If

            End If

        Catch ex As Exception
            Throw
        Finally
            ProviderLookUpForm.Dispose()
        End Try

    End Sub

    Private Sub PatientLookupButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PatientLookupButton.Click

        Dim PatientLookUpForm As PatientLookUpForm

        Try

            PatientLookUpForm = New PatientLookUpForm()

            If PatientLookUpForm.ShowDialog(Me) = DialogResult.OK Then

                FamilyIDTextBox.Text = CStr(PatientLookUpForm.FamilyID)
                RelationIDTextBox.Text = CStr(PatientLookUpForm.RelationID)
            Else
                FamilyIDTextBox.Text = Nothing
                RelationIDTextBox.Text = Nothing
            End If

        Catch ex As Exception
            Throw
        Finally
            PatientLookUpForm.Dispose()
        End Try

    End Sub

    Private Sub NumericTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles FamilyIDTextBox.TextChanged, RelationIDTextBox.TextChanged, ProviderIDTextBox.TextChanged

        Dim TBox As TextBox

        Try

            TBox = CType(sender, TextBox)

            TBox = CType(sender, TextBox)
            Dim digitsOnly As Regex = New Regex("[^\d]")
            TBox.Text = digitsOnly.Replace(TBox.Text, "")

            If FamilyIDTextBox.Text.Trim.Length > 0 AndAlso RelationIDTextBox.Text.Trim.Length > 0 Then
                AssociatedGroupBox.Enabled = True
            End If
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub PatientHistoryButton_Click(sender As Object, e As EventArgs) Handles PatientHistoryButton.Click

        Dim CustServ As CustomerServicePlugIn

        Try

            CustServ = New CustomerServicePlugIn(_Message, -1) With {
                .AppKey = _APPKEY & "PatientHistory\",
                .FamilyID = UFCWGeneral.IsNullIntegerHandler(FamilyIDTextBox.Text),
                .FamilyIDEnabled = False,
                .RelationID = UFCWGeneral.IsNullShortHandler(RelationIDTextBox.Text)
            }

            CustServ.Show()
            CustServ.Search()

            CustServ.Visible = False 'switch to dialog mode
            CustServ.ShowDialog(Me)

        Catch ex As Exception
            Throw
        Finally
            CustServ.Dispose()

        End Try
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

        FName = GetSetting(_APPKEY, Me.Name & "\Settings", "FontName", Me.Font.Name)
        FSize = CSng(GetSetting(_APPKEY, Me.Name & "\Settings", "FontSize", CStr(Me.Font.Size)))
        FStyle = CType(GetSetting(_APPKEY, Me.Name & "\Settings", "FontStyle", CStr(Me.Font.Style)), FontStyle)
        FUnit = CType(GetSetting(_APPKEY, Me.Name & "\Settings", "FontUnit", CStr(Me.Font.Unit)), GraphicsUnit)
        FCharset = CByte(GetSetting(_APPKEY, Me.Name & "\Settings", "FontCharset", CStr(Me.Font.GdiCharSet)))
        Me.Font = New Font(FName, FSize, FStyle, FUnit, FCharset)
    End Sub

    Private Sub SaveSettings()
        Dim WindowState As FormWindowState = Me.WindowState
        SaveSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(WindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = WindowState
        SaveSetting(_APPKEY, Me.Name & "\Settings", "FontName", Me.Font.Name)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "FontSize", CStr(Me.Font.Size))
        SaveSetting(_APPKEY, Me.Name & "\Settings", "FontStyle", CStr(Me.Font.Style))
        SaveSetting(_APPKEY, Me.Name & "\Settings", "FontUnit", CStr(Me.Font.Unit))
        SaveSetting(_APPKEY, Me.Name & "\Settings", "FontCharset", CStr(Me.Font.GdiCharSet))
    End Sub
End Class
'End Namespace