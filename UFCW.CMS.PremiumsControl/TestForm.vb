Imports UFCW.WCF
Imports UFCW.WCF.FileNet

Public Class TestForm
    Inherits System.Windows.Forms.Form

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
    Friend WithEvents ButtonRefresh As System.Windows.Forms.Button
    Friend WithEvents FamilyID As System.Windows.Forms.TextBox
    Friend WithEvents PremiumsControl As PremiumsHistoryControl
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents SplitContainer2 As SplitContainer
    Friend WithEvents PremiumsHistoryControl1 As PremiumsHistoryControl
    Friend WithEvents PremiumsEnrollmentControl1 As PremiumsEnrollmentControl
    Friend WithEvents PremiumPaymentsHistoryControl1 As PremiumPaymentsHistoryControl
    Friend WithEvents Button1 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.FamilyID = New System.Windows.Forms.TextBox()
        Me.ButtonRefresh = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.PremiumsHistoryControl1 = New PremiumsHistoryControl()
        Me.PremiumsEnrollmentControl1 = New PremiumsEnrollmentControl()
        Me.PremiumPaymentsHistoryControl1 = New PremiumPaymentsHistoryControl()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.SuspendLayout()
        '
        'FamilyID
        '
        Me.FamilyID.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyID.Location = New System.Drawing.Point(723, 8)
        Me.FamilyID.Name = "FamilyID"
        Me.FamilyID.Size = New System.Drawing.Size(96, 20)
        Me.FamilyID.TabIndex = 1
        Me.FamilyID.Text = "674605"
        '
        'ButtonRefresh
        '
        Me.ButtonRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonRefresh.Location = New System.Drawing.Point(723, 120)
        Me.ButtonRefresh.Name = "ButtonRefresh"
        Me.ButtonRefresh.Size = New System.Drawing.Size(96, 24)
        Me.ButtonRefresh.TabIndex = 3
        Me.ButtonRefresh.Text = "Refresh"
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(723, 160)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(96, 24)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Clear"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(12, 8)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.SplitContainer2)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.PremiumPaymentsHistoryControl1)
        Me.SplitContainer1.Size = New System.Drawing.Size(681, 575)
        Me.SplitContainer1.SplitterDistance = 402
        Me.SplitContainer1.TabIndex = 6
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.PremiumsHistoryControl1)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.PremiumsEnrollmentControl1)
        Me.SplitContainer2.Size = New System.Drawing.Size(402, 575)
        Me.SplitContainer2.SplitterDistance = 261
        Me.SplitContainer2.TabIndex = 0
        '
        'PremiumsHistoryControl1
        '
        Me.PremiumsHistoryControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PremiumsHistoryControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PremiumsHistoryControl1.Location = New System.Drawing.Point(0, 0)
        Me.PremiumsHistoryControl1.Name = "PremiumsHistoryControl1"
        Me.PremiumsHistoryControl1.Size = New System.Drawing.Size(402, 261)
        Me.PremiumsHistoryControl1.TabIndex = 9
        '
        'PremiumsEnrollmentControl1
        '
        Me.PremiumsEnrollmentControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PremiumsEnrollmentControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PremiumsEnrollmentControl1.Location = New System.Drawing.Point(0, 0)
        Me.PremiumsEnrollmentControl1.Name = "PremiumsEnrollmentControl1"
        Me.PremiumsEnrollmentControl1.Size = New System.Drawing.Size(402, 310)
        Me.PremiumsEnrollmentControl1.TabIndex = 10
        '
        'PremiumPaymentsHistoryControl1
        '
        Me.PremiumPaymentsHistoryControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PremiumPaymentsHistoryControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PremiumPaymentsHistoryControl1.Location = New System.Drawing.Point(0, 0)
        Me.PremiumPaymentsHistoryControl1.Name = "PremiumPaymentsHistoryControl1"
        Me.PremiumPaymentsHistoryControl1.Size = New System.Drawing.Size(275, 575)
        Me.PremiumPaymentsHistoryControl1.TabIndex = 0
        '
        'TestForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(843, 595)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ButtonRefresh)
        Me.Controls.Add(Me.FamilyID)
        Me.Name = "TestForm"
        Me.Text = "TestForm"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub Refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRefresh.Click

        Me.PremiumsHistoryControl1.FamilyID = CType(Me.FamilyID.Text, Integer)
        PremiumsHistoryControl1.LoadPremiumsControl()

        Me.PremiumPaymentsHistoryControl1.FamilyID = CType(Me.FamilyID.Text, Integer)
        PremiumPaymentsHistoryControl1.LoadPremiumPaymentsControl()

        Me.PremiumsEnrollmentControl1.FamilyID = CType(Me.FamilyID.Text, Integer)
        PremiumsEnrollmentControl1.LoadPremiumsControl()

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        PremiumsHistoryControl1.ClearAll()
        PremiumsEnrollmentControl1.ClearAll()
        PremiumPaymentsHistoryControl1.ClearAll()

    End Sub

    Private Sub TestForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim FNInfo As Tuple(Of String, String) = WCFWrapperCommon.InitializeFileNetUserInfo()

        Using FNSession As New Session(New WCFProcessInfo With {.ByProcessName = False, .ProcessID = Process.GetCurrentProcess.Id, .ProcessName = Process.GetCurrentProcess.ProcessName})
            If FNSession.LoggedOn = False Then FNSession.Logon(FNInfo.Item1, FNInfo.Item2)
        End Using

    End Sub

End Class