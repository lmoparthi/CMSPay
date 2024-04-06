<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class A2countMaintenance
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(A2countMaintenance))
        Me.FamilyPanel = New System.Windows.Forms.Panel()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnshow = New System.Windows.Forms.Button()
        Me.FamilyIdTextBox = New ExTextBox()
        Me.FamilyIdLabel = New System.Windows.Forms.Label()
        Me.A2CountControl = New A2CountControl()
        Me.FamilyPanel.SuspendLayout()
        Me.SuspendLayout()
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
        Me.FamilyPanel.Location = New System.Drawing.Point(0, 1)
        Me.FamilyPanel.Name = "FamilyPanel"
        Me.FamilyPanel.Size = New System.Drawing.Size(675, 48)
        Me.FamilyPanel.TabIndex = 9
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
        'A2CountControl
        '
        Me.A2CountControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.A2CountControl.AppKey = "UFCW\RegMaster\"
        Me.A2CountControl.Location = New System.Drawing.Point(3, 55)
        Me.A2CountControl.Name = "A2CountControl"
        Me.A2CountControl.Size = New System.Drawing.Size(669, 469)
        Me.A2CountControl.TabIndex = 10
        '
        'A2countMaintenance
        '
        Me.AcceptButton = Me.btnshow
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(674, 523)
        Me.Controls.Add(Me.A2CountControl)
        Me.Controls.Add(Me.FamilyPanel)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimumSize = New System.Drawing.Size(200, 200)
        Me.Name = "A2countMaintenance"
        Me.Text = "A2 Count Maintenance"
        Me.FamilyPanel.ResumeLayout(False)
        Me.FamilyPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents FamilyPanel As System.Windows.Forms.Panel
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents btnshow As System.Windows.Forms.Button
    Friend WithEvents FamilyIdTextBox As ExTextBox
    Friend WithEvents FamilyIdLabel As System.Windows.Forms.Label
    Friend WithEvents A2CountControl As A2CountControl
End Class
