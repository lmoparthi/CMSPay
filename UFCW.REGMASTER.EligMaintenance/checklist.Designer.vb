<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmchecklist
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmchecklist))
        Me.FamilyPanel = New System.Windows.Forms.Panel()
        Me.ClearActionButton = New System.Windows.Forms.Button()
        Me.SearchActionButton = New System.Windows.Forms.Button()
        Me.FamilyIdTextBox = New ExTextBox()
        Me.FamilyIdLabel = New System.Windows.Forms.Label()
        Me.ExitActionButton = New System.Windows.Forms.Button()
        Me.EligCalcElementsControl = New EligCalcElementsControl()
        Me.FamilyPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'FamilyPanel
        '
        Me.FamilyPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FamilyPanel.CausesValidation = False
        Me.FamilyPanel.Controls.Add(Me.ClearActionButton)
        Me.FamilyPanel.Controls.Add(Me.SearchActionButton)
        Me.FamilyPanel.Controls.Add(Me.FamilyIdTextBox)
        Me.FamilyPanel.Controls.Add(Me.FamilyIdLabel)
        Me.FamilyPanel.Location = New System.Drawing.Point(0, 0)
        Me.FamilyPanel.Name = "FamilyPanel"
        Me.FamilyPanel.Size = New System.Drawing.Size(699, 48)
        Me.FamilyPanel.TabIndex = 9
        '
        'ClearActionButton
        '
        Me.ClearActionButton.CausesValidation = False
        Me.ClearActionButton.Location = New System.Drawing.Point(302, 16)
        Me.ClearActionButton.Name = "ClearActionButton"
        Me.ClearActionButton.Size = New System.Drawing.Size(72, 23)
        Me.ClearActionButton.TabIndex = 4
        Me.ClearActionButton.Text = "Clear"
        '
        'SearchActionButton
        '
        Me.SearchActionButton.Location = New System.Drawing.Point(224, 16)
        Me.SearchActionButton.Name = "SearchActionButton"
        Me.SearchActionButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SearchActionButton.Size = New System.Drawing.Size(72, 23)
        Me.SearchActionButton.TabIndex = 3
        Me.SearchActionButton.Text = "Search"
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
        'ExitActionButton
        '
        Me.ExitActionButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExitActionButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ExitActionButton.Location = New System.Drawing.Point(611, 407)
        Me.ExitActionButton.Name = "ExitActionButton"
        Me.ExitActionButton.Size = New System.Drawing.Size(76, 23)
        Me.ExitActionButton.TabIndex = 11
        Me.ExitActionButton.Text = "Exit"
        Me.ExitActionButton.UseVisualStyleBackColor = True
        '
        'EligCalcElementsControl
        '
        Me.EligCalcElementsControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.EligCalcElementsControl.AppKey = "UFCW\RegMaster\"
        Me.EligCalcElementsControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.EligCalcElementsControl.CausesValidation = False
        Me.EligCalcElementsControl.Location = New System.Drawing.Point(0, 54)
        Me.EligCalcElementsControl.Name = "EligCalcElementsControl"
        Me.EligCalcElementsControl.Size = New System.Drawing.Size(699, 347)
        Me.EligCalcElementsControl.TabIndex = 10
        '
        'frmchecklist
        '
        Me.AcceptButton = Me.SearchActionButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.CancelButton = Me.ExitActionButton
        Me.ClientSize = New System.Drawing.Size(699, 436)
        Me.Controls.Add(Me.ExitActionButton)
        Me.Controls.Add(Me.EligCalcElementsControl)
        Me.Controls.Add(Me.FamilyPanel)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmchecklist"
        Me.Text = "Checklist"
        Me.FamilyPanel.ResumeLayout(False)
        Me.FamilyPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents FamilyPanel As System.Windows.Forms.Panel
    Friend WithEvents ClearActionButton As System.Windows.Forms.Button
    Friend WithEvents SearchActionButton As System.Windows.Forms.Button
    Friend WithEvents FamilyIdTextBox As ExTextBox
    Friend WithEvents FamilyIdLabel As System.Windows.Forms.Label
    Friend WithEvents EligCalcElementsControl As EligCalcElementsControl
    Friend WithEvents ExitActionButton As Windows.Forms.Button
End Class
