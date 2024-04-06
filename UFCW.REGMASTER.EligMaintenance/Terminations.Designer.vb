<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTerminations
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTerminations))
        Me.FamilyPanel = New System.Windows.Forms.Panel()
        Me.txtSSN = New ExTextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnshow = New System.Windows.Forms.Button()
        Me.FamilyIdTextBox = New ExTextBox()
        Me.FamilyIdLabel = New System.Windows.Forms.Label()
        Me.TerminationsControl = New TerminationsControl()
        Me.FamilyPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'FamilyPanel
        '
        Me.FamilyPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FamilyPanel.Controls.Add(Me.txtSSN)
        Me.FamilyPanel.Controls.Add(Me.Label2)
        Me.FamilyPanel.Controls.Add(Me.Label1)
        Me.FamilyPanel.Controls.Add(Me.btnClear)
        Me.FamilyPanel.Controls.Add(Me.btnshow)
        Me.FamilyPanel.Controls.Add(Me.FamilyIdTextBox)
        Me.FamilyPanel.Controls.Add(Me.FamilyIdLabel)
        Me.FamilyPanel.Location = New System.Drawing.Point(-1, 4)
        Me.FamilyPanel.Name = "FamilyPanel"
        Me.FamilyPanel.Size = New System.Drawing.Size(658, 48)
        Me.FamilyPanel.TabIndex = 10
        '
        'txtSSN
        '
        Me.txtSSN.Location = New System.Drawing.Point(267, 15)
        Me.txtSSN.MaxLength = 9
        Me.txtSSN.Name = "txtSSN"
        Me.txtSSN.Size = New System.Drawing.Size(85, 20)
        Me.txtSSN.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(232, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(29, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "SSN"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(171, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(23, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "OR"
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(483, 14)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(72, 23)
        Me.btnClear.TabIndex = 4
        Me.btnClear.Text = "Clear"
        '
        'btnshow
        '
        Me.btnshow.Location = New System.Drawing.Point(405, 14)
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
        'TerminationsControl
        '
        Me.TerminationsControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TerminationsControl.AppKey = "UFCW\RegMaster\"
        Me.TerminationsControl.Location = New System.Drawing.Point(2, 58)
        Me.TerminationsControl.Name = "TerminationsControl"
        Me.TerminationsControl.Size = New System.Drawing.Size(654, 434)
        Me.TerminationsControl.TabIndex = 11
        '
        'frmTerminations
        '
        Me.AcceptButton = Me.btnshow
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(659, 495)
        Me.Controls.Add(Me.TerminationsControl)
        Me.Controls.Add(Me.FamilyPanel)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmTerminations"
        Me.Text = "Terminations"
        Me.FamilyPanel.ResumeLayout(False)
        Me.FamilyPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents FamilyPanel As System.Windows.Forms.Panel
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents btnshow As System.Windows.Forms.Button
    Friend WithEvents FamilyIdTextBox As ExTextBox
    Friend WithEvents FamilyIdLabel As System.Windows.Forms.Label
    Friend WithEvents TerminationsControl As TerminationsControl
    Friend WithEvents txtSSN As ExTextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
