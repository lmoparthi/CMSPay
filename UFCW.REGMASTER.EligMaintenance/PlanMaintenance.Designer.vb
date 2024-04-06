<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PlanMaintenance
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PlanMaintenance))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.WaitingPeriodControl = New WaitingPeriodControl()
        Me.FamilyPanel = New System.Windows.Forms.Panel()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnshow = New System.Windows.Forms.Button()
        Me.FamilyIdTextBox = New ExTextBox()
        Me.FamilyIdLabel = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.FamilyPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.SplitContainer1)
        Me.Panel1.Location = New System.Drawing.Point(0, 56)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(692, 564)
        Me.Panel1.TabIndex = 1
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.WaitingPeriodControl)
        Me.SplitContainer1.Panel2Collapsed = True
        Me.SplitContainer1.Size = New System.Drawing.Size(692, 564)
        Me.SplitContainer1.SplitterDistance = 385
        Me.SplitContainer1.TabIndex = 0
        '
        'WaitingPeriodControl
        '
        Me.WaitingPeriodControl.AppKey = "UFCW\RegMaster\"
        Me.WaitingPeriodControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WaitingPeriodControl.Location = New System.Drawing.Point(0, 0)
        Me.WaitingPeriodControl.MinimumSize = New System.Drawing.Size(350, 500)
        Me.WaitingPeriodControl.Name = "WaitingPeriodControl"
        Me.WaitingPeriodControl.Size = New System.Drawing.Size(692, 564)
        Me.WaitingPeriodControl.TabIndex = 0
        '
        'FamilyPanel
        '
        Me.FamilyPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FamilyPanel.Controls.Add(Me.btnClear)
        Me.FamilyPanel.Controls.Add(Me.btnshow)
        Me.FamilyPanel.Controls.Add(Me.FamilyIdTextBox)
        Me.FamilyPanel.Controls.Add(Me.FamilyIdLabel)
        Me.FamilyPanel.Location = New System.Drawing.Point(0, 2)
        Me.FamilyPanel.Name = "FamilyPanel"
        Me.FamilyPanel.Size = New System.Drawing.Size(692, 48)
        Me.FamilyPanel.TabIndex = 8
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(302, 16)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(72, 23)
        Me.btnClear.TabIndex = 4
        Me.btnClear.Text = "Clear"
        '
        'btnshow
        '
        Me.btnshow.Location = New System.Drawing.Point(224, 16)
        Me.btnshow.Name = "btnshow"
        Me.btnshow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnshow.Size = New System.Drawing.Size(72, 23)
        Me.btnshow.TabIndex = 3
        Me.btnshow.Text = "Search"
        '
        'FamilyIdTextBox
        '
        Me.FamilyIdTextBox.Location = New System.Drawing.Point(70, 16)
        Me.FamilyIdTextBox.MaxLength = 9
        Me.FamilyIdTextBox.Name = "FamilyIdTextBox"
        Me.FamilyIdTextBox.Size = New System.Drawing.Size(85, 20)
        Me.FamilyIdTextBox.TabIndex = 0
        '
        'FamilyIdLabel
        '
        Me.FamilyIdLabel.AutoSize = True
        Me.FamilyIdLabel.Location = New System.Drawing.Point(16, 20)
        Me.FamilyIdLabel.Name = "FamilyIdLabel"
        Me.FamilyIdLabel.Size = New System.Drawing.Size(50, 13)
        Me.FamilyIdLabel.TabIndex = 3
        Me.FamilyIdLabel.Text = "Family ID"
        '
        'PlanMaintenance
        '
        Me.AcceptButton = Me.btnshow
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(691, 632)
        Me.Controls.Add(Me.FamilyPanel)
        Me.Controls.Add(Me.Panel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimumSize = New System.Drawing.Size(200, 200)
        Me.Name = "PlanMaintenance"
        Me.Text = "Plan A/B Maintenance"
        Me.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.FamilyPanel.ResumeLayout(False)
        Me.FamilyPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents WaitingPeriodControl As WaitingPeriodControl
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents FamilyPanel As System.Windows.Forms.Panel
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents btnshow As System.Windows.Forms.Button
    Friend WithEvents FamilyIdTextBox As ExTextBox
    Friend WithEvents FamilyIdLabel As System.Windows.Forms.Label

End Class
