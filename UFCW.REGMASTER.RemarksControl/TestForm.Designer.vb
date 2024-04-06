<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TestForm
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
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.RemarksControl1 = New REMARKSControl()
        Me.RemarksControl2 = New REMARKSControl()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(13, 42)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.RemarksControl1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.RemarksControl2)
        Me.SplitContainer1.Size = New System.Drawing.Size(492, 507)
        Me.SplitContainer1.SplitterDistance = 253
        Me.SplitContainer1.TabIndex = 1
        '
        'RemarksControl1
        '
        Me.RemarksControl1.AutoValidate = System.Windows.Forms.AutoValidate.Disable
        Me.RemarksControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RemarksControl1.Location = New System.Drawing.Point(0, 0)
        Me.RemarksControl1.Name = "RemarksControl1"
        Me.RemarksControl1.Size = New System.Drawing.Size(492, 253)
        Me.RemarksControl1.TabIndex = 1
        '
        'RemarksControl2
        '
        Me.RemarksControl2.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.RemarksControl2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RemarksControl2.Location = New System.Drawing.Point(0, 0)
        Me.RemarksControl2.Name = "RemarksControl2"
        Me.RemarksControl2.Size = New System.Drawing.Size(492, 250)
        Me.RemarksControl2.TabIndex = 0
        '
        'TestForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(517, 561)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "TestForm"
        Me.Text = "TestForm"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents RemarksControl1 As REMARKSControl
    Friend WithEvents RemarksControl2 As REMARKSControl
End Class
