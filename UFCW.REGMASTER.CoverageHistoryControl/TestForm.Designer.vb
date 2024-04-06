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
        Me.ReadOnlyCheckBox = New System.Windows.Forms.CheckBox()
        Me.RelationID = New System.Windows.Forms.TextBox()
        Me.FamilyID = New System.Windows.Forms.TextBox()
        Me.ButtonClear = New System.Windows.Forms.Button()
        Me.ButtonRefresh = New System.Windows.Forms.Button()
        Me.ButtonShowCoverage = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ReadOnlyCheckBox
        '
        Me.ReadOnlyCheckBox.AutoSize = True
        Me.ReadOnlyCheckBox.Location = New System.Drawing.Point(351, 24)
        Me.ReadOnlyCheckBox.Name = "ReadOnlyCheckBox"
        Me.ReadOnlyCheckBox.Size = New System.Drawing.Size(73, 17)
        Me.ReadOnlyCheckBox.TabIndex = 27
        Me.ReadOnlyCheckBox.Text = "ReadOnly"
        Me.ReadOnlyCheckBox.UseVisualStyleBackColor = True
        '
        'RelationID
        '
        Me.RelationID.Location = New System.Drawing.Point(161, 22)
        Me.RelationID.Name = "RelationID"
        Me.RelationID.Size = New System.Drawing.Size(100, 20)
        Me.RelationID.TabIndex = 26
        Me.RelationID.Text = "0"
        '
        'FamilyID
        '
        Me.FamilyID.Location = New System.Drawing.Point(20, 22)
        Me.FamilyID.Name = "FamilyID"
        Me.FamilyID.Size = New System.Drawing.Size(100, 20)
        Me.FamilyID.TabIndex = 25
        Me.FamilyID.Text = "24887"
        '
        'ButtonClear
        '
        Me.ButtonClear.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ButtonClear.Location = New System.Drawing.Point(646, 19)
        Me.ButtonClear.Name = "ButtonClear"
        Me.ButtonClear.Size = New System.Drawing.Size(96, 24)
        Me.ButtonClear.TabIndex = 24
        Me.ButtonClear.Text = "Clear"
        '
        'ButtonRefresh
        '
        Me.ButtonRefresh.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ButtonRefresh.Location = New System.Drawing.Point(521, 19)
        Me.ButtonRefresh.Name = "ButtonRefresh"
        Me.ButtonRefresh.Size = New System.Drawing.Size(96, 24)
        Me.ButtonRefresh.TabIndex = 23
        Me.ButtonRefresh.Text = "Refresh"
        '
        'ButtonShowCoverage
        '
        Me.ButtonShowCoverage.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ButtonShowCoverage.Location = New System.Drawing.Point(328, 72)
        Me.ButtonShowCoverage.Name = "ButtonShowCoverage"
        Me.ButtonShowCoverage.Size = New System.Drawing.Size(137, 24)
        Me.ButtonShowCoverage.TabIndex = 28
        Me.ButtonShowCoverage.Text = "Show Coverage Form"
        '
        'TestForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.ButtonShowCoverage)
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

    Friend WithEvents ReadOnlyCheckBox As Windows.Forms.CheckBox
    Friend WithEvents RelationID As Windows.Forms.TextBox
    Friend WithEvents FamilyID As Windows.Forms.TextBox
    Friend WithEvents ButtonClear As Windows.Forms.Button
    Friend WithEvents ButtonRefresh As Windows.Forms.Button
    Friend WithEvents ButtonShowCoverage As Windows.Forms.Button
End Class
