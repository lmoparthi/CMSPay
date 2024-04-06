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
    Friend WithEvents SSNRefreshButton As System.Windows.Forms.Button
    Friend WithEvents DocumentClass As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents CLAIMID As System.Windows.Forms.TextBox
    Friend WithEvents FAMILYID As System.Windows.Forms.TextBox
    Friend WithEvents RelationID As System.Windows.Forms.TextBox
    Friend WithEvents PARTSSN As System.Windows.Forms.TextBox
    Friend WithEvents PATSSN As System.Windows.Forms.TextBox
    Friend WithEvents ProviderID As System.Windows.Forms.TextBox
    Friend WithEvents ManualAccumulatorValues As UI.ManualAccumulatorValues
    Friend WithEvents AccumulatorValues As UI.AccumulatorValues
    Friend WithEvents ShowLineDetailsCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents ClaimRefreshButton As System.Windows.Forms.Button
    Friend WithEvents FamilyRefreshButton As System.Windows.Forms.Button
    Friend WithEvents EditCheckBox As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.DocumentClass = New System.Windows.Forms.TextBox()
        Me.SSNRefreshButton = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.CLAIMID = New System.Windows.Forms.TextBox()
        Me.FAMILYID = New System.Windows.Forms.TextBox()
        Me.RelationID = New System.Windows.Forms.TextBox()
        Me.PARTSSN = New System.Windows.Forms.TextBox()
        Me.PATSSN = New System.Windows.Forms.TextBox()
        Me.ProviderID = New System.Windows.Forms.TextBox()
        Me.EditCheckBox = New System.Windows.Forms.CheckBox()
        Me.ShowLineDetailsCheckBox = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.ClaimRefreshButton = New System.Windows.Forms.Button()
        Me.AccumulatorValues = New UFCW.CMS.UI.AccumulatorValues()
        Me.ManualAccumulatorValues = New UFCW.CMS.UI.ManualAccumulatorValues()
        Me.FamilyRefreshButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'DocumentClass
        '
        Me.DocumentClass.Location = New System.Drawing.Point(105, 8)
        Me.DocumentClass.Name = "DocumentClass"
        Me.DocumentClass.Size = New System.Drawing.Size(128, 20)
        Me.DocumentClass.TabIndex = 1
        Me.DocumentClass.Text = "CLAIMS"
        '
        'SSNRefreshButton
        '
        Me.SSNRefreshButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SSNRefreshButton.CausesValidation = False
        Me.SSNRefreshButton.Location = New System.Drawing.Point(1029, 8)
        Me.SSNRefreshButton.Name = "SSNRefreshButton"
        Me.SSNRefreshButton.Size = New System.Drawing.Size(103, 24)
        Me.SSNRefreshButton.TabIndex = 3
        Me.SSNRefreshButton.Text = "Refresh By SSN"
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(1029, 98)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(103, 24)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Clear"
        '
        'CLAIMID
        '
        Me.CLAIMID.Location = New System.Drawing.Point(105, 40)
        Me.CLAIMID.Name = "CLAIMID"
        Me.CLAIMID.Size = New System.Drawing.Size(104, 20)
        Me.CLAIMID.TabIndex = 7
        Me.CLAIMID.Text = "10595748"
        '
        'FAMILYID
        '
        Me.FAMILYID.Location = New System.Drawing.Point(287, 8)
        Me.FAMILYID.Name = "FAMILYID"
        Me.FAMILYID.Size = New System.Drawing.Size(67, 20)
        Me.FAMILYID.TabIndex = 8
        Me.FAMILYID.Text = "48688"
        '
        'RelationID
        '
        Me.RelationID.Location = New System.Drawing.Point(287, 40)
        Me.RelationID.Name = "RelationID"
        Me.RelationID.Size = New System.Drawing.Size(47, 20)
        Me.RelationID.TabIndex = 9
        Me.RelationID.Text = "0"
        '
        'PARTSSN
        '
        Me.PARTSSN.Location = New System.Drawing.Point(409, 8)
        Me.PARTSSN.Name = "PARTSSN"
        Me.PARTSSN.Size = New System.Drawing.Size(100, 20)
        Me.PARTSSN.TabIndex = 10
        Me.PARTSSN.Text = "558178698"
        '
        'PATSSN
        '
        Me.PATSSN.Location = New System.Drawing.Point(409, 40)
        Me.PATSSN.Name = "PATSSN"
        Me.PATSSN.Size = New System.Drawing.Size(100, 20)
        Me.PATSSN.TabIndex = 11
        Me.PATSSN.Text = "604046251"
        '
        'ProviderID
        '
        Me.ProviderID.Location = New System.Drawing.Point(570, 8)
        Me.ProviderID.Name = "ProviderID"
        Me.ProviderID.Size = New System.Drawing.Size(100, 20)
        Me.ProviderID.TabIndex = 13
        Me.ProviderID.Text = "88741"
        '
        'EditCheckBox
        '
        Me.EditCheckBox.Checked = True
        Me.EditCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.EditCheckBox.Location = New System.Drawing.Point(693, 16)
        Me.EditCheckBox.Name = "EditCheckBox"
        Me.EditCheckBox.Size = New System.Drawing.Size(104, 24)
        Me.EditCheckBox.TabIndex = 15
        Me.EditCheckBox.Text = "Edit Mode ?"
        '
        'ShowLineDetailsCheckBox
        '
        Me.ShowLineDetailsCheckBox.AutoSize = True
        Me.ShowLineDetailsCheckBox.Checked = True
        Me.ShowLineDetailsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ShowLineDetailsCheckBox.Location = New System.Drawing.Point(693, 46)
        Me.ShowLineDetailsCheckBox.Name = "ShowLineDetailsCheckBox"
        Me.ShowLineDetailsCheckBox.Size = New System.Drawing.Size(134, 17)
        Me.ShowLineDetailsCheckBox.TabIndex = 18
        Me.ShowLineDetailsCheckBox.Text = "Show Line Item Details"
        Me.ShowLineDetailsCheckBox.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(239, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(47, 13)
        Me.Label1.TabIndex = 19
        Me.Label1.Text = "FamilyID"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(239, 43)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 13)
        Me.Label2.TabIndex = 20
        Me.Label2.Text = "RelID"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(360, 11)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(48, 13)
        Me.Label3.TabIndex = 21
        Me.Label3.Text = "PartSSN"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(360, 43)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(45, 13)
        Me.Label4.TabIndex = 22
        Me.Label4.Text = "PatSSN"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(515, 11)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(40, 13)
        Me.Label5.TabIndex = 23
        Me.Label5.Text = "ProvID"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(21, 11)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(81, 13)
        Me.Label6.TabIndex = 24
        Me.Label6.Text = "DocumentClass"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(21, 43)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(43, 13)
        Me.Label7.TabIndex = 25
        Me.Label7.Text = "ClaimID"
        '
        'ClaimRefreshButton
        '
        Me.ClaimRefreshButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ClaimRefreshButton.CausesValidation = False
        Me.ClaimRefreshButton.Location = New System.Drawing.Point(1029, 38)
        Me.ClaimRefreshButton.Name = "ClaimRefreshButton"
        Me.ClaimRefreshButton.Size = New System.Drawing.Size(103, 24)
        Me.ClaimRefreshButton.TabIndex = 26
        Me.ClaimRefreshButton.Text = "Refresh By Claim"
        '
        'AccumulatorValues
        '
        Me.AccumulatorValues.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AccumulatorValues.BackColor = System.Drawing.SystemColors.Control
        Me.AccumulatorValues.IsInEditMode = False
        Me.AccumulatorValues.Location = New System.Drawing.Point(304, 80)
        Me.AccumulatorValues.MinimumSize = New System.Drawing.Size(200, 150)
        Me.AccumulatorValues.Name = "AccumulatorValues"
        Me.AccumulatorValues.ShowClaimView = False
        Me.AccumulatorValues.ShowHistory = False
        Me.AccumulatorValues.ShowLineDetails = False
        Me.AccumulatorValues.Size = New System.Drawing.Size(717, 368)
        Me.AccumulatorValues.TabIndex = 17
        '
        'ManualAccumulatorValues
        '
        Me.ManualAccumulatorValues.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ManualAccumulatorValues.IsInEditMode = False
        Me.ManualAccumulatorValues.Location = New System.Drawing.Point(24, 80)
        Me.ManualAccumulatorValues.Name = "ManualAccumulatorValues"
        Me.ManualAccumulatorValues.Size = New System.Drawing.Size(272, 368)
        Me.ManualAccumulatorValues.TabIndex = 14
        '
        'FamilyRefreshButton
        '
        Me.FamilyRefreshButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyRefreshButton.CausesValidation = False
        Me.FamilyRefreshButton.Location = New System.Drawing.Point(1029, 68)
        Me.FamilyRefreshButton.Name = "FamilyRefreshButton"
        Me.FamilyRefreshButton.Size = New System.Drawing.Size(103, 24)
        Me.FamilyRefreshButton.TabIndex = 27
        Me.FamilyRefreshButton.Text = "Refresh By Family"
        '
        'TestForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(1141, 480)
        Me.Controls.Add(Me.FamilyRefreshButton)
        Me.Controls.Add(Me.ClaimRefreshButton)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ShowLineDetailsCheckBox)
        Me.Controls.Add(Me.AccumulatorValues)
        Me.Controls.Add(Me.EditCheckBox)
        Me.Controls.Add(Me.ManualAccumulatorValues)
        Me.Controls.Add(Me.ProviderID)
        Me.Controls.Add(Me.PATSSN)
        Me.Controls.Add(Me.PARTSSN)
        Me.Controls.Add(Me.RelationID)
        Me.Controls.Add(Me.FAMILYID)
        Me.Controls.Add(Me.CLAIMID)
        Me.Controls.Add(Me.DocumentClass)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.SSNRefreshButton)
        Me.Name = "TestForm"
        Me.Text = "TestForm"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub SSNRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SSNRefreshButton.Click

        AccumulatorValues.SetFormInfo(Me.PATSSN.Text.ToString, Now)

        ManualAccumulatorValues.RefreshManualAccumulators()

    End Sub

    Private Sub ShowLineDetailsCheckBox_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ShowLineDetailsCheckBox.CheckStateChanged

        AccumulatorValues.ShowLineDetails = CBool(ShowLineDetailsCheckBox.CheckState)

    End Sub
    Private Sub EditCheckBox_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles EditCheckBox.CheckStateChanged

        AccumulatorValues.IsInEditMode = CBool(EditCheckBox.CheckState)
        ManualAccumulatorValues.IsInEditMode = CBool(EditCheckBox.CheckState)

    End Sub

    Private Sub ClaimRefreshButton_Click(sender As System.Object, e As System.EventArgs) Handles ClaimRefreshButton.Click

        AccumulatorValues.SetFormInfo(CType(Me.FAMILYID.Text, Integer), CType(Me.RelationID.Text, Integer), Now, CType(Me.CLAIMID.Text, Integer), True)

        ManualAccumulatorValues.RefreshManualAccumulators()

    End Sub

    Private Sub FamilyRefreshButton_Click(sender As Object, e As EventArgs) Handles FamilyRefreshButton.Click

        AccumulatorValues.SetFormInfo(CType(Me.FAMILYID.Text, Integer), CType(Me.RelationID.Text, Integer), Now)

        ManualAccumulatorValues.RefreshManualAccumulators()

    End Sub
End Class

