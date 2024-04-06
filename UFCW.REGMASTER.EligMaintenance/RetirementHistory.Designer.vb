<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RetirementHistory
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RetirementHistory))
        Me.FamilyPanel = New System.Windows.Forms.Panel()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.SearchButton = New System.Windows.Forms.Button()
        Me.FamilyIdTextBox = New ExTextBox()
        Me.FamilyIdLabel = New System.Windows.Forms.Label()
        Me.EligRetirementElements = New EligRetirementElementsControl()
        Me.FamilyPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'FamilyPanel
        '
        Me.FamilyPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FamilyPanel.Controls.Add(Me.btnClear)
        Me.FamilyPanel.Controls.Add(Me.SearchButton)
        Me.FamilyPanel.Controls.Add(Me.FamilyIdTextBox)
        Me.FamilyPanel.Controls.Add(Me.FamilyIdLabel)
        Me.FamilyPanel.Location = New System.Drawing.Point(-1, 0)
        Me.FamilyPanel.Name = "FamilyPanel"
        Me.FamilyPanel.Size = New System.Drawing.Size(705, 48)
        Me.FamilyPanel.TabIndex = 10
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(302, 16)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(72, 23)
        Me.btnClear.TabIndex = 101
        Me.btnClear.Text = "Clear"
        '
        'SearchButton
        '
        Me.SearchButton.Location = New System.Drawing.Point(224, 16)
        Me.SearchButton.Name = "SearchButton"
        Me.SearchButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SearchButton.Size = New System.Drawing.Size(72, 23)
        Me.SearchButton.TabIndex = 100
        Me.SearchButton.Text = "Search"
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
        'EligRetirementElements
        '
        Me.EligRetirementElements.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.EligRetirementElements.AppKey = "UFCW\RegMaster\"
        Me.EligRetirementElements.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.EligRetirementElements.Location = New System.Drawing.Point(-1, 54)
        Me.EligRetirementElements.Name = "EligRetirementElements"
        Me.EligRetirementElements.Size = New System.Drawing.Size(705, 403)
        Me.EligRetirementElements.TabIndex = 0
        '
        'RetirementHistory
        '
        Me.AcceptButton = Me.SearchButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.ClientSize = New System.Drawing.Size(706, 456)
        Me.Controls.Add(Me.FamilyPanel)
        Me.Controls.Add(Me.EligRetirementElements)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "RetirementHistory"
        Me.Text = "Retirement History"
        Me.FamilyPanel.ResumeLayout(False)
        Me.FamilyPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents EligRetirementElements As EligRetirementElementsControl
    Friend WithEvents FamilyPanel As System.Windows.Forms.Panel
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents SearchButton As System.Windows.Forms.Button
    Friend WithEvents FamilyIdTextBox As ExTextBox
    Friend WithEvents FamilyIdLabel As System.Windows.Forms.Label
End Class
