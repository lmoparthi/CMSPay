Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ComponentModel
Imports SharedInterfaces

<PlugIn("HRA Override", 2, "Main", 9)> Public Class HRAOverrideHistory
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"

    Private _SharedInterfacesMessage As IMessage
    Private _OpenIndex As Integer
    Private _UniqueID As String
    Private _FamilyID As Integer
    Private _RelationID As Integer
    Private _DomainUser As String = SystemInformation.UserName
    Friend WithEvents RelationIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label

    Private _HRAOverrideHistoryForm As HRAOverrideHistory

#Region " Windows Form Designer generated code "
    Public Sub New(ByVal objMsg As IMessage, ByVal OpenIndex As Integer)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _SharedInterfacesMessage = objMsg
        _OpenIndex = OpenIndex

        Me.WindowState = FormWindowState.Normal

    End Sub
    Public Sub New()

        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        Me.WindowState = FormWindowState.Normal
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
    Public Overloads Sub Dispose()
        If Not _HRAOverrideHistoryForm Is Nothing Then _HRAOverrideHistoryForm.Dispose()
        _HRAOverrideHistoryForm = Nothing
        MyBase.Dispose()
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents FamilyPanel As System.Windows.Forms.Panel
    Friend WithEvents FamilyIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents FamilyIdLabel As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents HraBalanceControl1 As HRABalanceControl
    Friend WithEvents RetrieveButton As System.Windows.Forms.Button
    Friend WithEvents SelectFamilyButton As System.Windows.Forms.Button
    Friend WithEvents HRABalanceControl As HRABalanceControl
    Friend WithEvents HRAOverrideControl As HRAOverrideControl
    Friend WithEvents HRAActivityControl As HRAActivityControl

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.FamilyPanel = New System.Windows.Forms.Panel()
        Me.RetrieveButton = New System.Windows.Forms.Button()
        Me.FamilyIDTextBox = New System.Windows.Forms.TextBox()
        Me.FamilyIdLabel = New System.Windows.Forms.Label()
        Me.SelectFamilyButton = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.HRABalanceControl = New HRABalanceControl()
        Me.HRAOverrideControl = New HRAOverrideControl()
        Me.HRAActivityControl = New HRAActivityControl()
        Me.RelationIDTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.FamilyPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'FamilyPanel
        '
        Me.FamilyPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyPanel.Controls.Add(Me.RelationIDTextBox)
        Me.FamilyPanel.Controls.Add(Me.Label1)
        Me.FamilyPanel.Controls.Add(Me.RetrieveButton)
        Me.FamilyPanel.Controls.Add(Me.FamilyIDTextBox)
        Me.FamilyPanel.Controls.Add(Me.FamilyIdLabel)
        Me.FamilyPanel.Controls.Add(Me.SelectFamilyButton)
        Me.FamilyPanel.Location = New System.Drawing.Point(8, 8)
        Me.FamilyPanel.Name = "FamilyPanel"
        Me.FamilyPanel.Size = New System.Drawing.Size(432, 48)
        Me.FamilyPanel.TabIndex = 6
        '
        'RetrieveButton
        '
        Me.RetrieveButton.Location = New System.Drawing.Point(308, 16)
        Me.RetrieveButton.Name = "RetrieveButton"
        Me.RetrieveButton.Size = New System.Drawing.Size(104, 23)
        Me.RetrieveButton.TabIndex = 8
        Me.RetrieveButton.Text = "&Retrieve Balance"
        '
        'FamilyIDTextBox
        '
        Me.FamilyIDTextBox.Location = New System.Drawing.Point(63, 18)
        Me.FamilyIDTextBox.MaxLength = 9
        Me.FamilyIDTextBox.Name = "FamilyIDTextBox"
        Me.FamilyIDTextBox.Size = New System.Drawing.Size(64, 20)
        Me.FamilyIDTextBox.TabIndex = 2
        '
        'FamilyIdLabel
        '
        Me.FamilyIdLabel.AutoSize = True
        Me.FamilyIdLabel.Location = New System.Drawing.Point(7, 21)
        Me.FamilyIdLabel.Name = "FamilyIdLabel"
        Me.FamilyIdLabel.Size = New System.Drawing.Size(50, 13)
        Me.FamilyIdLabel.TabIndex = 3
        Me.FamilyIdLabel.Text = "Family ID"
        '
        'SelectFamilyButton
        '
        Me.SelectFamilyButton.Location = New System.Drawing.Point(220, 18)
        Me.SelectFamilyButton.Name = "SelectFamilyButton"
        Me.SelectFamilyButton.Size = New System.Drawing.Size(24, 20)
        Me.SelectFamilyButton.TabIndex = 7
        Me.SelectFamilyButton.Text = "..."
        Me.ToolTip1.SetToolTip(Me.SelectFamilyButton, "Search for Family Id by Name")
        '
        'HRABalanceControl
        '
        Me.HRABalanceControl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HRABalanceControl.FamilyID = Nothing
        Me.HRABalanceControl.Location = New System.Drawing.Point(8, 57)
        Me.HRABalanceControl.Name = "HRABalanceControl"
        Me.HRABalanceControl.Size = New System.Drawing.Size(432, 121)
        Me.HRABalanceControl.TabIndex = 7
        '
        'HRAOverrideControl
        '
        Me.HRAOverrideControl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HRAOverrideControl.Location = New System.Drawing.Point(36, 184)
        Me.HRAOverrideControl.Name = "HRAOverrideControl"
        Me.HRAOverrideControl.Size = New System.Drawing.Size(384, 311)
        Me.HRAOverrideControl.TabIndex = 0
        '
        'HRAActivityControl
        '
        Me.HRAActivityControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HRAActivityControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.HRAActivityControl.Location = New System.Drawing.Point(8, 494)
        Me.HRAActivityControl.Name = "HRAActivityControl"
        Me.HRAActivityControl.Size = New System.Drawing.Size(440, 244)
        Me.HRAActivityControl.TabIndex = 8
        '
        'RelationIDTextBox
        '
        Me.RelationIDTextBox.Location = New System.Drawing.Point(174, 18)
        Me.RelationIDTextBox.MaxLength = 9
        Me.RelationIDTextBox.Name = "RelationIDTextBox"
        Me.RelationIDTextBox.Size = New System.Drawing.Size(31, 20)
        Me.RelationIDTextBox.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(131, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(37, 13)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Rel ID"
        '
        'HRAOverrideHistory
        '
        Me.AcceptButton = Me.RetrieveButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(456, 750)
        Me.Controls.Add(Me.HRAActivityControl)
        Me.Controls.Add(Me.HRAOverrideControl)
        Me.Controls.Add(Me.HRABalanceControl)
        Me.Controls.Add(Me.FamilyPanel)
        Me.MinimumSize = New System.Drawing.Size(420, 500)
        Me.Name = "HRAOverrideHistory"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "HRA Manual Override"
        Me.TopMost = True
        Me.FamilyPanel.ResumeLayout(False)
        Me.FamilyPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Properties"
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

    <DefaultValue(CBool(False)), Browsable(True), System.ComponentModel.Description("Shows or Hides the Close Button.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
        End Set
    End Property

    Public Property FamilyID() As Integer
        Get
            Return CInt(Me.FamilyIDTextBox.Text)
        End Get
        Set(ByVal value As Integer)
            Me.FamilyIDTextBox.Text = CStr(value)
            _FamilyID = value
        End Set
    End Property

#End Region

#Region "Form Events"
    Private Sub RefreshHRA(ByVal sender As Object) Handles HRAOverrideControl.AfterRefresh

        Me.HRAOverrideControl.Clear()
        Me.HRABalanceControl.RefreshBalance()
        Me.HRAActivityControl.RefreshActivity()

        Me.Refresh()

    End Sub

    Private Sub HRAOverrideHistory_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If _OpenIndex = 0 Then
                Me.Text = "HRA Override"
            Else
                Me.Text = "HRA Override - " & _OpenIndex
            End If

            Me.WindowState = FormWindowState.Normal

            AddHandler FamilyIDTextBox.KeyPress, New System.Windows.Forms.KeyPressEventHandler(AddressOf HandleKeyPress)

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub HRAOverride_FormClosing(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.FormClosing
        Try
            Me.Visible = False
            Me.MdiParent = Nothing
            If _HRAOverrideHistoryForm IsNot Nothing Then _HRAOverrideHistoryForm.Close()
            'frm.Dispose()
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

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
    Private Sub SetSettings()
        Me.Top = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)))
        Me.Height = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)))
        Me.Width = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)
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

        Dim WindowState As FormWindowState = Me.WindowState
        SaveSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(WindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = WindowState

        Me.Opacity = 100

    End Sub
    Private Sub SearchButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Search()
    End Sub
    Public Sub Search()
        Try
            If _SharedInterfacesMessage IsNot Nothing Then _SharedInterfacesMessage.StatusMessage("Retrieving HRA...")

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Finally
            If Not _SharedInterfacesMessage Is Nothing Then _SharedInterfacesMessage.StatusMessage("Accumulators Loaded...")
        End Try
    End Sub
    Private Sub OverrideHistoryButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Try
        Catch ex As Exception
            Dim s As String = ex.Message
        End Try

    End Sub

    Private Sub RetrieveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RetrieveButton.Click

        If Me.FamilyIDTextBox.Text.Length > 0 AndAlso IsNumeric(Me.FamilyIDTextBox.Text) Then
            Me.HRABalanceControl.HRABalance(CInt(Me.FamilyIDTextBox.Text), CType(If(Me.RelationIDTextBox.Text.Trim.Length > 0 AndAlso IsNumeric(Me.RelationIDTextBox.Text), Me.RelationIDTextBox.Text.Trim, Nothing), Short?))
            Me.HRAActivityControl.LoadHRAActivityControl(CInt(Me.HRABalanceControl.FamilyID), CType(If(Me.RelationIDTextBox.Text.Trim.Length > 0 AndAlso IsNumeric(Me.RelationIDTextBox.Text), Me.RelationIDTextBox.Text.Trim, Nothing), Short?))
            Me.HRAOverrideControl.LoadHRAOverrideControl(CInt(Me.HRABalanceControl.FamilyID), HRAActivityControl.HRAHistory)
        End If

    End Sub

    Private Sub SelectFamilyButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectFamilyButton.Click

        Dim Frm As PatientLookUpForm

        Try

            Frm = New PatientLookUpForm()

            If Frm.ShowDialog(Me) = DialogResult.OK Then
                Me.FamilyIDTextBox.Text = CStr(Frm.FamilyID)
            Else
                Me.FamilyIDTextBox.Text = Nothing
                Me.HRAOverrideControl.Clear()
                Me.HRABalanceControl.ClearAll()
                Me.HRAActivityControl.ClearAll()

            End If

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            Else
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Finally
            Frm.Dispose()
            Frm = Nothing
        End Try

    End Sub
    Private Sub HandleKeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)

        'ignore if not digit or control key
        If (Not (System.Char.IsDigit(e.KeyChar)) AndAlso Not (System.Char.IsControl(e.KeyChar))) Then
            e.Handled = True
        End If

    End Sub

#End Region

#Region "Control Events"

    Public Sub EnableInputs()
        Me.FamilyIDTextBox.ReadOnly = False
    End Sub
    Public Sub DisableInputs()
        Me.FamilyIDTextBox.ReadOnly = True
    End Sub

#End Region

End Class