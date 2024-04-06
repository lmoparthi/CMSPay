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
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents HraBalanceControl1 As HRABalanceControl
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents PATSSN As System.Windows.Forms.TextBox
    Friend WithEvents PARTSSN As System.Windows.Forms.TextBox
    Friend WithEvents RelationIDTBox As System.Windows.Forms.TextBox
    Friend WithEvents txtFAMILYID As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ButtonRefresh As System.Windows.Forms.Button
    Friend WithEvents HraActivityControl1 As HRAActivityControl
    Friend WithEvents HraOverrideControl1 As HRAOverrideControl

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.HraActivityControl1 = New HRAActivityControl()
        Me.HraBalanceControl1 = New HRABalanceControl()
        Me.HraOverrideControl1 = New HRAOverrideControl()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.PATSSN = New System.Windows.Forms.TextBox()
        Me.PARTSSN = New System.Windows.Forms.TextBox()
        Me.RelationIDTBox = New System.Windows.Forms.TextBox()
        Me.txtFAMILYID = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ButtonRefresh = New System.Windows.Forms.Button()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 81)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.HraActivityControl1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.HraBalanceControl1)
        Me.SplitContainer1.Panel2.Controls.Add(Me.HraOverrideControl1)
        Me.SplitContainer1.Size = New System.Drawing.Size(900, 721)
        Me.SplitContainer1.SplitterDistance = 360
        Me.SplitContainer1.TabIndex = 15
        '
        'HraActivityControl1
        '
        Me.HraActivityControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.HraActivityControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.HraActivityControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HraActivityControl1.Location = New System.Drawing.Point(0, 0)
        Me.HraActivityControl1.Name = "HraActivityControl1"
        Me.HraActivityControl1.Size = New System.Drawing.Size(900, 360)
        Me.HraActivityControl1.TabIndex = 16
        '
        'HraBalanceControl1
        '
        Me.HraBalanceControl1.FamilyID = Nothing
        Me.HraBalanceControl1.Location = New System.Drawing.Point(401, 40)
        Me.HraBalanceControl1.Name = "HraBalanceControl1"
        Me.HraBalanceControl1.Size = New System.Drawing.Size(392, 294)
        Me.HraBalanceControl1.TabIndex = 15
        '
        'HraOverrideControl1
        '
        Me.HraOverrideControl1.Location = New System.Drawing.Point(3, 40)
        Me.HraOverrideControl1.Name = "HraOverrideControl1"
        Me.HraOverrideControl1.Size = New System.Drawing.Size(392, 315)
        Me.HraOverrideControl1.TabIndex = 14
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Panel1.Controls.Add(Me.PATSSN)
        Me.Panel1.Controls.Add(Me.PARTSSN)
        Me.Panel1.Controls.Add(Me.RelationIDTBox)
        Me.Panel1.Controls.Add(Me.txtFAMILYID)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Controls.Add(Me.ButtonRefresh)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(900, 75)
        Me.Panel1.TabIndex = 16
        '
        'PATSSN
        '
        Me.PATSSN.Location = New System.Drawing.Point(310, 44)
        Me.PATSSN.Name = "PATSSN"
        Me.PATSSN.Size = New System.Drawing.Size(100, 20)
        Me.PATSSN.TabIndex = 17
        Me.PATSSN.Text = "545415021"
        '
        'PARTSSN
        '
        Me.PARTSSN.Location = New System.Drawing.Point(310, 12)
        Me.PARTSSN.Name = "PARTSSN"
        Me.PARTSSN.Size = New System.Drawing.Size(100, 20)
        Me.PARTSSN.TabIndex = 16
        Me.PARTSSN.Text = "551770438"
        '
        'txtRelationID
        '
        Me.RelationIDTBox.Location = New System.Drawing.Point(151, 16)
        Me.RelationIDTBox.Name = "txtRelationID"
        Me.RelationIDTBox.Size = New System.Drawing.Size(96, 20)
        Me.RelationIDTBox.TabIndex = 15
        '
        'txtFAMILYID
        '
        Me.txtFAMILYID.Location = New System.Drawing.Point(18, 16)
        Me.txtFAMILYID.Name = "txtFAMILYID"
        Me.txtFAMILYID.Size = New System.Drawing.Size(112, 20)
        Me.txtFAMILYID.TabIndex = 14
        Me.txtFAMILYID.Text = "1221584"
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(769, 12)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(96, 24)
        Me.Button1.TabIndex = 13
        Me.Button1.Text = "Clear"
        '
        'ButtonRefresh
        '
        Me.ButtonRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonRefresh.Location = New System.Drawing.Point(647, 13)
        Me.ButtonRefresh.Name = "ButtonRefresh"
        Me.ButtonRefresh.Size = New System.Drawing.Size(96, 24)
        Me.ButtonRefresh.TabIndex = 12
        Me.ButtonRefresh.Text = "Refresh"
        '
        'TestForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(900, 802)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "TestForm"
        Me.Text = "TestForm"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRefresh.Click

        Me.HraBalanceControl1.HRABalance(CType(txtFAMILYID.Text.ToString, Integer), If(RelationIDTBox.Text.Trim.Length < 1, Nothing, CType(RelationIDTBox.Text.Trim, Short?)))
        Me.HraActivityControl1.LoadHRAActivityControl(CType(txtFAMILYID.Text.ToString, Integer), If(RelationIDTBox.Text.Trim.Length < 1, Nothing, CType(RelationIDTBox.Text.Trim, Short?)))

        HraOverrideControl1.LoadHRAOverrideControl(CType(txtFAMILYID.Text.ToString, Integer), If(RelationIDTBox.Text.Trim.Length < 1, Nothing, CType(RelationIDTBox.Text.Trim, Short?)), HraActivityControl1.HRAHistory)
        HraOverrideControl1.Refresh()

    End Sub

    Private Sub RefreshHRA(ByVal sender As Object) Handles HraOverrideControl1.AfterRefresh

        Me.HraOverrideControl1.Clear()
        Me.HraBalanceControl1.RefreshBalance()
        Me.HraActivityControl1.RefreshActivity()

        Me.Refresh()

    End Sub

End Class