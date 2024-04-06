Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports SharedInterfaces
Imports System.ComponentModel

<PlugIn("Accumulator Override", 2, "Main", 11)> Public Class AccumulatorOverride
    Inherits System.Windows.Forms.Form

    Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _DomainUser As String = SystemInformation.UserName

    Private _APPKEY As String = "UFCW\Claims\"

    Private _SharedInterfacesMessage As IMessage
    Private _OpenIndex As Integer

    Private _MemberAccumulatorManager As MemberAccumulatorManager
    Private _UniqueID As String
    Private _UseFamilyRelationIDs As Boolean = True
    Private _FamilyID As Integer
    Private _RelationID As Short?
    Private _AccumulatorOverrideHistoryForm As AccumulatorOverrideHistory

#Region " Windows Form Designer generated code "
    Public Sub New(ByVal objMsg As IMessage, ByVal openIndex As Integer)
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

    Public Sub New(ByRef memAccumManager As MemberAccumulatorManager)

        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        _MemberAccumulatorManager = memAccumManager
        AccumulatorValues.MemberAccumulatorManager = _MemberAccumulatorManager

        Me.WindowState = FormWindowState.Normal
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If components IsNot Nothing Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub
    Public Overloads Sub Dispose()
        Me.MenuStrip.Dispose()

        If _AccumulatorOverrideHistoryForm IsNot Nothing Then _AccumulatorOverrideHistoryForm.Dispose()
        _AccumulatorOverrideHistoryForm = Nothing

        _MemberAccumulatorManager = Nothing

        If Me.AccumulatorValues IsNot Nothing Then Me.AccumulatorValues.Dispose()

        MyBase.Dispose()

    End Sub

    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents AccumulatorValues As AccumulatorValues
    Friend WithEvents MenuStrip As System.Windows.Forms.MenuStrip
    Friend WithEvents ModeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FamilyRelationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SSNToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents PatientSSNTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents SearchButton As System.Windows.Forms.Button
    Friend WithEvents PatientPanel As System.Windows.Forms.Panel
    Friend WithEvents FamilyPanel As System.Windows.Forms.Panel
    Friend WithEvents FamilyIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents FamilyIdLabel As System.Windows.Forms.Label
    Friend WithEvents RelationIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents RelationIDLabel As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents PatientNameTextBox As System.Windows.Forms.TextBox
    Public WithEvents DateOfBirthTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents ParticipantSSNTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AccumulatorOverride))
        Me.PatientSSNTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SearchButton = New System.Windows.Forms.Button()
        Me.PatientPanel = New System.Windows.Forms.Panel()
        Me.FamilyPanel = New System.Windows.Forms.Panel()
        Me.ParticipantSSNTextBox = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.RelationIDTextBox = New System.Windows.Forms.TextBox()
        Me.RelationIDLabel = New System.Windows.Forms.Label()
        Me.FamilyIDTextBox = New System.Windows.Forms.TextBox()
        Me.FamilyIdLabel = New System.Windows.Forms.Label()
        Me.PatientNameTextBox = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.DateOfBirthTextBox = New System.Windows.Forms.TextBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.AccumulatorValues = New AccumulatorValues()
        Me.MenuStrip = New System.Windows.Forms.MenuStrip()
        Me.ModeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FamilyRelationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SSNToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PatientPanel.SuspendLayout()
        Me.FamilyPanel.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.MenuStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'PatientSSNTextBox
        '
        Me.PatientSSNTextBox.Location = New System.Drawing.Point(84, 4)
        Me.PatientSSNTextBox.MaxLength = 9
        Me.PatientSSNTextBox.Name = "PatientSSNTextBox"
        Me.PatientSSNTextBox.Size = New System.Drawing.Size(112, 20)
        Me.PatientSSNTextBox.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(65, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Patient SSN"
        '
        'SearchButton
        '
        Me.SearchButton.Location = New System.Drawing.Point(322, 8)
        Me.SearchButton.Name = "SearchButton"
        Me.SearchButton.Size = New System.Drawing.Size(96, 23)
        Me.SearchButton.TabIndex = 2
        Me.SearchButton.Text = "&Search"
        '
        'PatientPanel
        '
        Me.PatientPanel.Controls.Add(Me.PatientSSNTextBox)
        Me.PatientPanel.Controls.Add(Me.Label1)
        Me.PatientPanel.Location = New System.Drawing.Point(4, 8)
        Me.PatientPanel.Name = "PatientPanel"
        Me.PatientPanel.Size = New System.Drawing.Size(208, 28)
        Me.PatientPanel.TabIndex = 5
        '
        'FamilyPanel
        '
        Me.FamilyPanel.Controls.Add(Me.ParticipantSSNTextBox)
        Me.FamilyPanel.Controls.Add(Me.Label4)
        Me.FamilyPanel.Controls.Add(Me.RelationIDTextBox)
        Me.FamilyPanel.Controls.Add(Me.RelationIDLabel)
        Me.FamilyPanel.Controls.Add(Me.FamilyIDTextBox)
        Me.FamilyPanel.Controls.Add(Me.FamilyIdLabel)
        Me.FamilyPanel.Controls.Add(Me.PatientNameTextBox)
        Me.FamilyPanel.Controls.Add(Me.Label2)
        Me.FamilyPanel.Controls.Add(Me.Label3)
        Me.FamilyPanel.Controls.Add(Me.DateOfBirthTextBox)
        Me.FamilyPanel.Location = New System.Drawing.Point(4, 8)
        Me.FamilyPanel.Name = "FamilyPanel"
        Me.FamilyPanel.Size = New System.Drawing.Size(300, 80)
        Me.FamilyPanel.TabIndex = 6
        '
        'ParticipantSSNTextBox
        '
        Me.ParticipantSSNTextBox.Location = New System.Drawing.Point(128, 54)
        Me.ParticipantSSNTextBox.MaxLength = 9
        Me.ParticipantSSNTextBox.Name = "ParticipantSSNTextBox"
        Me.ParticipantSSNTextBox.ReadOnly = True
        Me.ParticipantSSNTextBox.Size = New System.Drawing.Size(104, 20)
        Me.ParticipantSSNTextBox.TabIndex = 11
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(44, 58)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 13)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "Participant SSN"
        '
        'RelationIdTextBox
        '
        Me.RelationIDTextBox.Location = New System.Drawing.Point(64, 29)
        Me.RelationIDTextBox.MaxLength = 9
        Me.RelationIDTextBox.Name = "RelationIdTextBox"
        Me.RelationIDTextBox.Size = New System.Drawing.Size(60, 20)
        Me.RelationIDTextBox.TabIndex = 4
        '
        'RelationIDLabel
        '
        Me.RelationIDLabel.AutoSize = True
        Me.RelationIDLabel.Location = New System.Drawing.Point(4, 31)
        Me.RelationIDLabel.Name = "RelationIDLabel"
        Me.RelationIDLabel.Size = New System.Drawing.Size(60, 13)
        Me.RelationIDLabel.TabIndex = 5
        Me.RelationIDLabel.Text = "Relation ID"
        '
        'FamilyIdTextBox
        '
        Me.FamilyIDTextBox.Location = New System.Drawing.Point(64, 4)
        Me.FamilyIDTextBox.MaxLength = 9
        Me.FamilyIDTextBox.Name = "FamilyIdTextBox"
        Me.FamilyIDTextBox.Size = New System.Drawing.Size(60, 20)
        Me.FamilyIDTextBox.TabIndex = 2
        '
        'FamilyIdLabel
        '
        Me.FamilyIdLabel.AutoSize = True
        Me.FamilyIdLabel.Location = New System.Drawing.Point(4, 5)
        Me.FamilyIdLabel.Name = "FamilyIdLabel"
        Me.FamilyIdLabel.Size = New System.Drawing.Size(50, 13)
        Me.FamilyIdLabel.TabIndex = 3
        Me.FamilyIdLabel.Text = "Family ID"
        '
        'PatientNameTextBox
        '
        Me.PatientNameTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PatientNameTextBox.Location = New System.Drawing.Point(165, 4)
        Me.PatientNameTextBox.Name = "PatientNameTextBox"
        Me.PatientNameTextBox.ReadOnly = True
        Me.PatientNameTextBox.Size = New System.Drawing.Size(131, 20)
        Me.PatientNameTextBox.TabIndex = 7
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(126, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(35, 16)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Name:"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(131, 31)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(33, 16)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "DOB"
        '
        'DoBTextBox
        '
        Me.DateOfBirthTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DateOfBirthTextBox.Location = New System.Drawing.Point(165, 29)
        Me.DateOfBirthTextBox.Name = "DoBTextBox"
        Me.DateOfBirthTextBox.ReadOnly = True
        Me.DateOfBirthTextBox.Size = New System.Drawing.Size(92, 20)
        Me.DateOfBirthTextBox.TabIndex = 10
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.AccumulatorValues)
        Me.GroupBox1.Location = New System.Drawing.Point(4, 90)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(443, 372)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        '
        'AccumulatorValues
        '
        Me.AccumulatorValues.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.AccumulatorValues.BackColor = System.Drawing.SystemColors.Control
        Me.AccumulatorValues.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AccumulatorValues.IsInEditMode = False
        Me.AccumulatorValues.Location = New System.Drawing.Point(3, 16)
        Me.AccumulatorValues.MinimumSize = New System.Drawing.Size(200, 150)
        Me.AccumulatorValues.Name = "AccumulatorValues"
        Me.AccumulatorValues.ShowClaimView = False
        Me.AccumulatorValues.ShowLineDetails = False
        Me.AccumulatorValues.Size = New System.Drawing.Size(437, 353)
        Me.AccumulatorValues.TabIndex = 10
        '
        'MenuStrip
        '
        Me.MenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ModeToolStripMenuItem})
        Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip.Name = "MenuStrip"
        Me.MenuStrip.Size = New System.Drawing.Size(452, 24)
        Me.MenuStrip.TabIndex = 9
        Me.MenuStrip.Text = "MenuStrip1"
        '
        'ModeToolStripMenuItem
        '
        Me.ModeToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FamilyRelationToolStripMenuItem, Me.SSNToolStripMenuItem})
        Me.ModeToolStripMenuItem.Name = "ModeToolStripMenuItem"
        Me.ModeToolStripMenuItem.Size = New System.Drawing.Size(45, 20)
        Me.ModeToolStripMenuItem.Text = "Mode"
        '
        'FamilyRelationToolStripMenuItem
        '
        Me.FamilyRelationToolStripMenuItem.Name = "FamilyRelationToolStripMenuItem"
        Me.FamilyRelationToolStripMenuItem.Size = New System.Drawing.Size(158, 22)
        Me.FamilyRelationToolStripMenuItem.Text = "Family/Relation"
        '
        'SSNToolStripMenuItem
        '
        Me.SSNToolStripMenuItem.Name = "SSNToolStripMenuItem"
        Me.SSNToolStripMenuItem.Size = New System.Drawing.Size(158, 22)
        Me.SSNToolStripMenuItem.Text = "SSN"
        '
        'AccumulatorOverride
        '
        Me.AcceptButton = Me.SearchButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(452, 474)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.SearchButton)
        Me.Controls.Add(Me.FamilyPanel)
        Me.Controls.Add(Me.PatientPanel)
        Me.Controls.Add(Me.MenuStrip)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(460, 1000)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(460, 500)
        Me.Name = "AccumulatorOverride"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Accumulator Override"
        Me.PatientPanel.ResumeLayout(False)
        Me.PatientPanel.PerformLayout()
        Me.FamilyPanel.ResumeLayout(False)
        Me.FamilyPanel.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.MenuStrip.ResumeLayout(False)
        Me.MenuStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region "Properties"
    Public Property MemberAccumManager() As MemberAccumulatorManager
        Get
            Return _MemberAccumulatorManager
        End Get
        Set(ByVal value As MemberAccumulatorManager)
            _MemberAccumulatorManager = Value
        End Set
    End Property

    Public Property UniqueID() As String
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
        Get
            Return _UniqueID
        End Get
        Set(ByVal value As String)
            _UniqueID = Value
        End Set
    End Property

    <DefaultValue(CBool(False)), Browsable(True), System.ComponentModel.Description("Shows or Hides the Close Button.")> _
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = Value
        End Set
    End Property

    Public Property PatientSSN() As String
        Get
            Return Me.PatientSSNTextBox.Text
        End Get
        Set(ByVal value As String)
            If Value Is Nothing Then Return
            Me.PatientSSNTextBox.Text = Value
        End Set
    End Property

    Public Property ParticipantSSN() As String
        Get
            Return Me.ParticipantSSNTextBox.Text
        End Get
        Set(ByVal value As String)
            If value Is Nothing Then Return
            Me.ParticipantSSNTextBox.Text = value
        End Set
    End Property

    Public Property UseFamilyRelationIDs() As Boolean
        Get
            Return _UseFamilyRelationIDs
        End Get
        Set(ByVal value As Boolean)
            _UseFamilyRelationIDs = Value

            SwapFamilyAndSSNUI()

        End Set
    End Property

    Public Property FamilyID() As Integer
        Get
            Return CInt(Me.FamilyIDTextBox.Text)
        End Get
        Set(ByVal value As Integer)
            Me.FamilyIDTextBox.Text = CStr(Value)
            _FamilyID = value
        End Set
    End Property

    Public Property RelationID() As Short?
        Get
            Return If(Me.RelationIDTextBox.Text.Trim.Length > 0, CType(Me.RelationIDTextBox.Text.Trim, Short?), Nothing)
        End Get
        Set(ByVal value As Short?)
            Me.RelationIDTextBox.Text = CStr(value)
            _RelationID = value
        End Set
    End Property
#End Region

#Region "Form Events"

    Private Sub AccumulatorOverride_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            SetSettings()

            If _OpenIndex = 0 Then
                Me.Text = "Accumulator Override"
            Else
                Me.Text = "Accumulator Override - " & _OpenIndex
            End If
            Me.AcceptButton = Me.SearchButton

            Me.UseFamilyRelationIDs = _UseFamilyRelationIDs
            AccumulatorValues.ShowLineDetails = False
            AccumulatorValues.UserId = _DomainUser

            Me.WindowState = FormWindowState.Normal
            Me.AccumulatorValues.MemberAccumulatorManager = _MemberAccumulatorManager

            SwapFamilyAndSSNUI()

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub AccumulatorOverride_FormClosing(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.FormClosing
        Try
            Me.Visible = False
            Me.MdiParent = Nothing

            If Not _AccumulatorOverrideHistoryForm Is Nothing Then _AccumulatorOverrideHistoryForm.Close()

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub SetSettings()
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
        Me.Top = If(CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)))
        Me.Height = CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = If(CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)))
        Me.Width = CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(_AppKey, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)
    End Sub

    Private Sub SaveSettings()
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

        Dim lWindowState As FormWindowState = Me.WindowState

        SaveSetting(_AppKey, Me.Name & "\Settings", "WindowState", CInt(lWindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = lWindowState

        Me.Opacity = 100

    End Sub
#End Region

#Region "Control Events"
    Private Sub PatientSSNTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PatientSSNTextBox.TextChanged

        Dim TBox As TextBox
        Dim IntCnt As Integer
        Dim StrTmp As String

        Try
            TBox = CType(sender, TextBox)

            If Not IsNumeric(TBox.Text) AndAlso Len(TBox.Text) > 0 Then
                StrTmp = TBox.Text
                For IntCnt = 1 To Len(StrTmp)
                    If IsNumeric(Mid(StrTmp, IntCnt, 1)) = False AndAlso Len(StrTmp) > 0 Then
                        StrTmp = Replace(StrTmp, Mid(StrTmp, IntCnt, 1), "")
                    End If
                Next
                TBox.Text = StrTmp
            End If
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub SearchButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchButton.Click
        Search()
    End Sub
    Private Function IsValidForm() As Boolean
        If _UseFamilyRelationIDs Then
            If Me.FamilyIDTextBox.Text.Length < 1 OrElse Not IsNumeric(Me.FamilyIDTextBox.Text) OrElse Me.RelationIDTextBox.Text.Length < 1 OrElse Not IsNumeric(Me.RelationIDTextBox.Text) Then
                Return False
            End If
        Else
            If Me.PatientSSNTextBox.Text.Length < 1 OrElse Not IsNumeric(Me.PatientSSNTextBox.Text) Then
                Return False
            End If
        End If
        Return True
    End Function
    Public Sub Search()
        Try
            If IsValidForm() Then

                AccumulatorValues.IsInEditMode = True
                AccumulatorValues.ShowHistory = True
                AccumulatorValues.ReadOnly = False
                If _SharedInterfacesMessage IsNot Nothing Then _SharedInterfacesMessage.StatusMessage("Loading Accumulators...")

                If _UseFamilyRelationIDs Then
                    AccumulatorValues.SetFormInfo(If(Me.FamilyIDTextBox.Text.Trim.Length > 0, CInt(Me.FamilyIDTextBox.Text.Trim), Nothing), If(Me.RelationIDTextBox.Text.Trim.Length > 0, CShort(Me.RelationIDTextBox.Text.Trim), Nothing))
                Else
                    AccumulatorValues.SetFormInfo(PatientSSNTextBox.Text.ToString, New Date(UFCWGeneral.NowDate.Year, 12, 31))

                End If
            Else
                MessageBox.Show("Not enough information to search by. Specify Relation ID", "Accumulator", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Finally
            If _SharedInterfacesMessage IsNot Nothing Then _SharedInterfacesMessage.StatusMessage("Accumulators Loaded...")

        End Try
    End Sub

    Private Sub OverrideHistoryButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim FamilyID As Integer?
        Dim RelationID As Short?
        Dim DR As DataRow
        Dim FamilyRelationChooserForm As FamilyRelationChooser
        Dim DT As DataTable

        Try
            _AccumulatorOverrideHistoryForm = New AccumulatorOverrideHistory
            _AccumulatorOverrideHistoryForm.MdiParent = Me.MdiParent

            If Me.AccumulatorValues.FamilyID = 0 Then

                DT = CMSDALFDBMD.RetrievePatientsBySSN(CInt(Me.PatientSSNTextBox.Text.Replace("-"c, "")))

                If DT IsNot Nothing Then
                    If DT.Rows.Count > 1 Then

                        FamilyRelationChooserForm = New FamilyRelationChooser
                        FamilyRelationChooserForm.FamilyRelationDataGrid.DataSource = DT
                        FamilyRelationChooserForm.SetStyleGrid()
                        FamilyRelationChooserForm.ShowDialog()

                        FamilyID = FamilyRelationChooserForm.SelectFamilyID
                        RelationID = FamilyRelationChooserForm.SelectRelationID

                        Me.RelationIDTextBox.Text = CStr(RelationID)

                        Me.FamilyIDTextBox.Text = CStr(FamilyID)

                        If FamilyID Is Nothing OrElse FamilyID = -1 Then
                            MessageBox.Show("No valid Family ID was chosen", "Family ID", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            Return
                        End If
                    Else
                        DR = DT.Rows(0)
                        If DR IsNot Nothing Then
                            If DR("FAMILY_ID") IsNot System.DBNull.Value AndAlso DR("RELATION_ID") IsNot System.DBNull.Value Then
                                Me.FamilyID = CInt(DR("FAMILY_ID"))
                                Me.RelationID = UFCWGeneral.IsNullShortHandler(DR("RELATION_ID"))
                            End If
                        Else
                            Return
                        End If
                    End If
                End If
            Else
                Me.FamilyID = AccumulatorValues.FamilyID
                Me.RelationID = AccumulatorValues.RelationID
            End If

            _AccumulatorOverrideHistoryForm.AccumulatorHistory.SetFormInfo(CInt(Me.FamilyID), CShort(Me.RelationID))
            _AccumulatorOverrideHistoryForm.Show()

        Catch ex As Exception
            Throw
        Finally

            If FamilyRelationChooserForm IsNot Nothing Then
                FamilyRelationChooserForm.Close()
            End If
            FamilyRelationChooserForm = Nothing
        End Try

    End Sub

    Private Sub AccumulatorOverride_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        If Me.AccumulatorValues.HasChanges Then
            If MessageBox.Show("You are about to discard all changes.  Continue?", "Accumulator Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                e.Cancel = False
            Else
                e.Cancel = True
            End If
        Else
            '
        End If

    End Sub

    Private Sub FamilyRelationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FamilyRelationToolStripMenuItem.Click
        Me.SSNToolStripMenuItem.Checked = False
        Me.FamilyRelationToolStripMenuItem.Checked = True
        Me._UseFamilyRelationIDs = True
        SwapFamilyAndSSNUI()
    End Sub

    Private Sub SSNToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SSNToolStripMenuItem.Click
        Me.SSNToolStripMenuItem.Checked = True
        Me.FamilyRelationToolStripMenuItem.Checked = False
        Me._UseFamilyRelationIDs = False
        SwapFamilyAndSSNUI()
    End Sub

    Private Sub PatientNameTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PatientNameTextBox.TextChanged
        Me.ToolTip1.SetToolTip(Me.PatientNameTextBox, Me.PatientNameTextBox.Text)
    End Sub

    Private Sub AccumulatorValues_Closing() Handles AccumulatorValues.Closing
        Me.Close()
    End Sub

    Private Sub FamilyRelationTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FamilyIDTextBox.TextChanged, RelationIDTextBox.TextChanged

        Dim TBox As TextBox
        Dim StrTmp As String

        Try
            TBox = CType(sender, TextBox)
            If IsNumeric(TBox.Text) = False AndAlso Len(TBox.Text) > 0 Then
                StrTmp = TBox.Text
                For IntCnt As Integer = 1 To Len(StrTmp)
                    If IsNumeric(Mid(StrTmp, IntCnt, 1)) = False AndAlso Len(StrTmp) > 0 Then
                        StrTmp = Replace(StrTmp, Mid(StrTmp, IntCnt, 1), "")
                    End If
                Next
                TBox.Text = StrTmp
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

#End Region

#Region "Misc"
    Private Sub SwapFamilyAndSSNUI()
        If _UseFamilyRelationIDs Then
            AccumulatorValues.Location = New System.Drawing.Point(0, 92)
            Me.Size = New System.Drawing.Size(420, 525)
            Me.AccumulatorValues.Size = New System.Drawing.Size(420, 372)
            Me.PatientPanel.Visible = False
            Me.FamilyPanel.Visible = True
        Else
            AccumulatorValues.Location = New System.Drawing.Point(0, 56)
            Me.AccumulatorValues.Size = New System.Drawing.Size(420, 368)
            Me.Size = New System.Drawing.Size(420, 525)
            Me.PatientPanel.Visible = True
            Me.FamilyPanel.Visible = False
        End If
    End Sub

    Public Sub EnableInputs()
        Me.PatientSSNTextBox.ReadOnly = False
        Me.FamilyIDTextBox.ReadOnly = False
        Me.RelationIDTextBox.ReadOnly = False
    End Sub
    Public Sub DisableInputs()
        Me.PatientSSNTextBox.ReadOnly = True
        Me.FamilyIDTextBox.ReadOnly = True
        Me.RelationIDTextBox.ReadOnly = True
    End Sub
#End Region

End Class