<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RegMasterHistoryForm
    Inherits System.Windows.Forms.Form

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ExitButton = New System.Windows.Forms.Button()
        Me.HistoryViewer1 = New REGMasterHistoryViewerForm()
        Me.SuspendLayout()
        '
        'ExitButton
        '
        Me.ExitButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ExitButton.Location = New System.Drawing.Point(561, 216)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(75, 23)
        Me.ExitButton.TabIndex = 2
        Me.ExitButton.Text = "Exit"
        Me.ExitButton.UseVisualStyleBackColor = True
        '
        'HistoryViewer1
        '
        Me.HistoryViewer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HistoryViewer1.AppKey = "UFCW\RegMaster\"
        Me.HistoryViewer1.BackColor = System.Drawing.SystemColors.Window
        Me.HistoryViewer1.Location = New System.Drawing.Point(0, 0)
        Me.HistoryViewer1.Mode = REGMasterHistoryMode.Risk
        Me.HistoryViewer1.Name = "HistoryViewer1"
        Me.HistoryViewer1.Size = New System.Drawing.Size(648, 210)
        Me.HistoryViewer1.TabIndex = 3
        '
        'HistoryForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ExitButton
        Me.ClientSize = New System.Drawing.Size(648, 251)
        Me.Controls.Add(Me.HistoryViewer1)
        Me.Controls.Add(Me.ExitButton)
        Me.Name = "HistoryForm"
        Me.Text = "History"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ExitButton As System.Windows.Forms.Button
    Friend WithEvents HistoryViewer1 As REGMasterHistoryViewerForm
End Class
