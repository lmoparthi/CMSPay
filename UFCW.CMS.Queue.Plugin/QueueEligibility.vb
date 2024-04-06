Imports System.ComponentModel
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class QueueEligibility
    Inherits System.Windows.Forms.Form

    Private _FamilyID As Integer = -1
    Private _RelationID As Short? = Nothing
    Private _DocType As String = ""

    Private _APPKEY As String = "UFCW\Claims\"

    Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

#Region "Public Properties"

    <DefaultValue("UFCW\Claims\"), Browsable(True), System.ComponentModel.Description("Represents the application key used when accessing the registry.")> _
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = Value
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
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents QueueEligibilityControl As EligibilityControl
    Friend WithEvents CloseButton As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents FamilyIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents RelationIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents PartSSNTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents PatSSNTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PartFNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents PartLNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PatLNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PatFNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim Resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(QueueEligibility))
        Me.QueueEligibilityControl = New EligibilityControl
        Me.CloseButton = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.FamilyIDTextBox = New System.Windows.Forms.TextBox
        Me.RelationIDTextBox = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.PartSSNTextBox = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.PatSSNTextBox = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.PartFNameTextBox = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.PartLNameTextBox = New System.Windows.Forms.TextBox
        Me.PatLNameTextBox = New System.Windows.Forms.TextBox
        Me.PatFNameTextBox = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'QueueEligibilityControl
        '
        Me.QueueEligibilityControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.QueueEligibilityControl.AppKey = "UFCW\Claims\QueueEligibility\"
        Me.QueueEligibilityControl.Location = New System.Drawing.Point(8, 80)
        Me.QueueEligibilityControl.Name = "QueueEligibilityControl"
        Me.QueueEligibilityControl.Size = New System.Drawing.Size(656, 216)
        Me.QueueEligibilityControl.TabIndex = 0
        '
        'CloseButton
        '
        Me.CloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CloseButton.Location = New System.Drawing.Point(584, 300)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(75, 23)
        Me.CloseButton.TabIndex = 1
        Me.CloseButton.Text = "&Cancel"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Family ID:"
        '
        'FamilyIDTextBox
        '
        Me.FamilyIDTextBox.Location = New System.Drawing.Point(64, 4)
        Me.FamilyIDTextBox.Name = "FamilyIDTextBox"
        Me.FamilyIDTextBox.ReadOnly = True
        Me.FamilyIDTextBox.Size = New System.Drawing.Size(100, 20)
        Me.FamilyIDTextBox.TabIndex = 3
        '
        'RelationIDTextBox
        '
        Me.RelationIDTextBox.Location = New System.Drawing.Point(240, 4)
        Me.RelationIDTextBox.Name = "RelationIDTextBox"
        Me.RelationIDTextBox.ReadOnly = True
        Me.RelationIDTextBox.Size = New System.Drawing.Size(48, 20)
        Me.RelationIDTextBox.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(176, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Relation ID:"
        '
        'PartSSNTextBox
        '
        Me.PartSSNTextBox.Location = New System.Drawing.Point(96, 28)
        Me.PartSSNTextBox.Name = "PartSSNTextBox"
        Me.PartSSNTextBox.ReadOnly = True
        Me.PartSSNTextBox.Size = New System.Drawing.Size(100, 20)
        Me.PartSSNTextBox.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 32)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(85, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Participant SSN:"
        '
        'PatSSNTextBox
        '
        Me.PatSSNTextBox.Location = New System.Drawing.Point(96, 52)
        Me.PatSSNTextBox.Name = "PatSSNTextBox"
        Me.PatSSNTextBox.ReadOnly = True
        Me.PatSSNTextBox.Size = New System.Drawing.Size(100, 20)
        Me.PatSSNTextBox.TabIndex = 9
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(24, 56)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(68, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Patient SSN:"
        '
        'PartFNameTextBox
        '
        Me.PartFNameTextBox.Location = New System.Drawing.Point(304, 28)
        Me.PartFNameTextBox.Name = "PartFNameTextBox"
        Me.PartFNameTextBox.ReadOnly = True
        Me.PartFNameTextBox.Size = New System.Drawing.Size(152, 20)
        Me.PartFNameTextBox.TabIndex = 11
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(208, 32)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(91, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Participant Name:"
        '
        'PartLNameTextBox
        '
        Me.PartLNameTextBox.Location = New System.Drawing.Point(464, 28)
        Me.PartLNameTextBox.Name = "PartLNameTextBox"
        Me.PartLNameTextBox.ReadOnly = True
        Me.PartLNameTextBox.Size = New System.Drawing.Size(192, 20)
        Me.PartLNameTextBox.TabIndex = 12
        '
        'PatLNameTextBox
        '
        Me.PatLNameTextBox.Location = New System.Drawing.Point(464, 52)
        Me.PatLNameTextBox.Name = "PatLNameTextBox"
        Me.PatLNameTextBox.ReadOnly = True
        Me.PatLNameTextBox.Size = New System.Drawing.Size(192, 20)
        Me.PatLNameTextBox.TabIndex = 15
        '
        'PatFNameTextBox
        '
        Me.PatFNameTextBox.Location = New System.Drawing.Point(304, 52)
        Me.PatFNameTextBox.Name = "PatFNameTextBox"
        Me.PatFNameTextBox.ReadOnly = True
        Me.PatFNameTextBox.Size = New System.Drawing.Size(152, 20)
        Me.PatFNameTextBox.TabIndex = 14
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(224, 56)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(74, 13)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "Patient Name:"
        '
        'QueueEligibility
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.CloseButton
        Me.ClientSize = New System.Drawing.Size(672, 326)
        Me.Controls.Add(Me.PatLNameTextBox)
        Me.Controls.Add(Me.PatFNameTextBox)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.PartLNameTextBox)
        Me.Controls.Add(Me.PartFNameTextBox)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.PatSSNTextBox)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.PartSSNTextBox)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.RelationIDTextBox)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.FamilyIDTextBox)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.QueueEligibilityControl)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(672, 224)
        Me.Name = "QueueEligibility"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Eligibility"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region "Properties"
    <Browsable(True), System.ComponentModel.Description("Gets or Sets the FamilyID of the Document.")> _
    Public Property FamilyID() As Integer
        Get
            Return _FamilyID
        End Get
        Set(ByVal value As Integer)
            _FamilyID = Value

            FamilyIDTextBox.Text = CStr(_FamilyID)
        End Set
    End Property

    <Browsable(True), System.ComponentModel.Description("Gets or Sets the RelationID of the Document.")> _
    Public Property RelationID() As Short?
        Get
            Return _RelationID
        End Get
        Set(ByVal value As Short?)
            _RelationID = value

            RelationIDTextBox.Text = CStr(_RelationID)
        End Set
    End Property

    <Browsable(True), System.ComponentModel.Description("Gets or Sets the Doc Type of the Current Claim")> _
    Public Property DocType() As String
        Get
            Return _DocType
        End Get
        Set(ByVal value As String)
            _DocType = Value
        End Set
    End Property
#End Region

#Region "Constructor"
    Public Sub New(ByVal familyID As Integer, ByVal relationID As Short?)
        Me.New()

        _FamilyID = familyID
        _RelationID = relationID

        FamilyIDTextBox.Text = CStr(_FamilyID)
        RelationIDTextBox.Text = CStr(_RelationID)
    End Sub

    Public Sub New(ByVal familyID As Integer, ByVal relationID As Short?, ByVal docType As String)
        Me.New()

        _FamilyID = familyID
        _RelationID = relationID
        _DocType = DocType

        FamilyIDTextBox.Text = CStr(_FamilyID)
        RelationIDTextBox.Text = CStr(_RelationID)
    End Sub
#End Region

    Private Sub QueueEligibility_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            SetSettings()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub QueueEligibility_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Try
            SaveSettings()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub CloseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseButton.Click
        Try
            Me.Close()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SetSettings()
        Me.Top = If(CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)))
        Me.Height = CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = If(CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)))
        Me.Width = CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(_AppKey, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)

    End Sub

    Private Sub SaveSettings()

        Dim WindowState As FormWindowState = Me.WindowState
        SaveSetting(_AppKey, Me.Name & "\Settings", "WindowState", CInt(WindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = WindowState

    End Sub

    Public Sub LoadEligibility(ByVal familyID As Integer, ByVal relationID As Short?)
        Try
            _FamilyID = familyID
            _RelationID = relationID

            FamilyIDTextBox.Text = CStr(_FamilyID)
            RelationIDTextBox.Text = CStr(_RelationID)

            QueueEligibilityControl.LoadEligibility(_FamilyID, _RelationID)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub LoadEligibility(ByVal gamilyID As Integer, ByVal relationID As Short?, ByVal DocType As String)
        Try
            _FamilyID = FamilyID
            _RelationID = RelationID
            _DocType = DocType

            FamilyIDTextBox.Text = CStr(_FamilyID)
            RelationIDTextBox.Text = CStr(_RelationID)

            QueueEligibilityControl.LoadEligibility(_FamilyID, _RelationID, _DocType)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub LoadEligibility()
        Try
            QueueEligibilityControl.LoadEligibility(_FamilyID, _RelationID, _DocType)
        Catch ex As Exception
            Throw
        End Try
    End Sub
End Class