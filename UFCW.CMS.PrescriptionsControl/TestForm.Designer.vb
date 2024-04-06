<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TestForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.RelationIDTextBox = New System.Windows.Forms.TextBox()
        Me.Label82 = New System.Windows.Forms.Label()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.FamilyIDTextBox = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.PrescriptionsControl1 = New PrescriptionsControl()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'RelationIDTextBox
        '
        Me.RelationIDTextBox.Location = New System.Drawing.Point(202, 15)
        Me.RelationIDTextBox.Name = "RelationIDTextBox"
        Me.RelationIDTextBox.Size = New System.Drawing.Size(26, 20)
        Me.RelationIDTextBox.TabIndex = 0
        '
        'Label82
        '
        Me.Label82.AutoSize = True
        Me.Label82.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label82.Location = New System.Drawing.Point(138, 19)
        Me.Label82.Name = "Label82"
        Me.Label82.Size = New System.Drawing.Size(60, 13)
        Me.Label82.TabIndex = 16
        Me.Label82.Text = "Relation ID"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.RelationIDTextBox)
        Me.GroupBox4.Controls.Add(Me.FamilyIDTextBox)
        Me.GroupBox4.Controls.Add(Me.Label82)
        Me.GroupBox4.Controls.Add(Me.Label12)
        Me.GroupBox4.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(246, 40)
        Me.GroupBox4.TabIndex = 16
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Family:"
        '
        'FamilyIDTextBox
        '
        Me.FamilyIDTextBox.Location = New System.Drawing.Point(60, 16)
        Me.FamilyIDTextBox.Name = "FamilyIDTextBox"
        Me.FamilyIDTextBox.Size = New System.Drawing.Size(72, 20)
        Me.FamilyIDTextBox.TabIndex = 0
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label12.Location = New System.Drawing.Point(4, 20)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(50, 13)
        Me.Label12.TabIndex = 16
        Me.Label12.Text = "Family ID"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(551, 28)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 18
        Me.Button1.Text = "Go"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'PrescriptionsControl1
        '
        Me.PrescriptionsControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PrescriptionsControl1.Location = New System.Drawing.Point(19, 58)
        Me.PrescriptionsControl1.Name = "PrescriptionsControl1"
        Me.PrescriptionsControl1.Size = New System.Drawing.Size(622, 533)
        Me.PrescriptionsControl1.TabIndex = 21
        '
        'TestForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(653, 616)
        Me.Controls.Add(Me.PrescriptionsControl1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.GroupBox4)
        Me.Name = "TestForm"
        Me.Text = "TestForm"
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents RelationIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label82 As System.Windows.Forms.Label
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Private WithEvents FamilyIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents PrescriptionsControl As PrescriptionsControl
    Friend WithEvents PrescriptionsControl1 As PrescriptionsControl
End Class
