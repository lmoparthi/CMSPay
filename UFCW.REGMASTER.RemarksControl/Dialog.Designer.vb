<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RemarksDialog
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
    Friend WithEvents RemarksControl As REMARKSControl

    Private Sub InitializeComponent()
        Me.RemarksControl = New REMARKSControl()
        Me.SuspendLayout()
        '
        'RemarksControl
        '
        Me.RemarksControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RemarksControl.Location = New System.Drawing.Point(0, 0)
        Me.RemarksControl.Name = "RemarksControl"
        Me.RemarksControl.Size = New System.Drawing.Size(475, 388)
        Me.RemarksControl.TabIndex = 0
        '
        'Dialog
        '
        Me.ClientSize = New System.Drawing.Size(475, 388)
        Me.Controls.Add(Me.RemarksControl)
        Me.Name = "Dialog"
        Me.Text = "Remark"
        Me.ResumeLayout(False)

    End Sub
End Class
