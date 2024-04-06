<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class TestForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    Friend WithEvents ButtonRefresh As System.Windows.Forms.Button
    Friend WithEvents ButtonClear As System.Windows.Forms.Button
    Friend WithEvents FamilyID As System.Windows.Forms.TextBox
    Friend WithEvents RelationID As System.Windows.Forms.TextBox

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.


    Private Sub InitializeComponent()
        Me.ButtonRefresh = New System.Windows.Forms.Button()
        Me.ButtonClear = New System.Windows.Forms.Button()
        Me.FamilyID = New System.Windows.Forms.TextBox()
        Me.RelationID = New System.Windows.Forms.TextBox()
        Me.ReadOnlyCheckBox = New System.Windows.Forms.CheckBox()
        Me.CobControl1 = New COBControl()
        Me.SuspendLayout()
        '
        'ButtonRefresh
        '
        Me.ButtonRefresh.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ButtonRefresh.Location = New System.Drawing.Point(580, 12)
        Me.ButtonRefresh.Name = "ButtonRefresh"
        Me.ButtonRefresh.Size = New System.Drawing.Size(96, 24)
        Me.ButtonRefresh.TabIndex = 3
        Me.ButtonRefresh.Text = "Refresh"
        '
        'ButtonClear
        '
        Me.ButtonClear.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ButtonClear.Location = New System.Drawing.Point(705, 12)
        Me.ButtonClear.Name = "ButtonClear"
        Me.ButtonClear.Size = New System.Drawing.Size(96, 24)
        Me.ButtonClear.TabIndex = 5
        Me.ButtonClear.Text = "Clear"
        '
        'FamilyID
        '
        Me.FamilyID.Location = New System.Drawing.Point(57, 15)
        Me.FamilyID.Name = "FamilyID"
        Me.FamilyID.Size = New System.Drawing.Size(100, 20)
        Me.FamilyID.TabIndex = 14
        Me.FamilyID.Text = "24887"
        '
        'RelationID
        '
        Me.RelationID.Location = New System.Drawing.Point(198, 15)
        Me.RelationID.Name = "RelationID"
        Me.RelationID.Size = New System.Drawing.Size(100, 20)
        Me.RelationID.TabIndex = 15
        Me.RelationID.Text = "0"
        '
        'ReadOnlyCheckBox
        '
        Me.ReadOnlyCheckBox.AutoSize = True
        Me.ReadOnlyCheckBox.Location = New System.Drawing.Point(388, 17)
        Me.ReadOnlyCheckBox.Name = "ReadOnlyCheckBox"
        Me.ReadOnlyCheckBox.Size = New System.Drawing.Size(73, 17)
        Me.ReadOnlyCheckBox.TabIndex = 17
        Me.ReadOnlyCheckBox.Text = "ReadOnly"
        Me.ReadOnlyCheckBox.UseVisualStyleBackColor = True
        '
        'CobControl1
        '
        Me.CobControl1.AppKey = Nothing
        Me.CobControl1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.CobControl1.Location = New System.Drawing.Point(12, 57)
        Me.CobControl1.Name = "CobControl1"
        Me.CobControl1.Size = New System.Drawing.Size(841, 562)
        Me.CobControl1.TabIndex = 18
        '
        'TestForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ClientSize = New System.Drawing.Size(873, 631)
        Me.Controls.Add(Me.CobControl1)
        Me.Controls.Add(Me.ReadOnlyCheckBox)
        Me.Controls.Add(Me.RelationID)
        Me.Controls.Add(Me.FamilyID)
        Me.Controls.Add(Me.ButtonClear)
        Me.Controls.Add(Me.ButtonRefresh)
        Me.Name = "TestForm"
        Me.Text = "TestForm"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ReadOnlyCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents PayerComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents CobControl1 As COBControl
End Class
