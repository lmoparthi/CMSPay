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
    Friend WithEvents PartSSN As System.Windows.Forms.TextBox

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.


    Private Sub InitializeComponent()
        Me.ButtonRefresh = New System.Windows.Forms.Button()
        Me.ButtonClear = New System.Windows.Forms.Button()
        Me.PartSSN = New System.Windows.Forms.TextBox()
        Me.ReadOnlyCheckBox = New System.Windows.Forms.CheckBox()
        Me.UFCWDocsControl1 = New UFCWDocsControl()
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
        'PartSSN
        '
        Me.PartSSN.Location = New System.Drawing.Point(57, 15)
        Me.PartSSN.Name = "PartSSN"
        Me.PartSSN.Size = New System.Drawing.Size(100, 20)
        Me.PartSSN.TabIndex = 14
        Me.PartSSN.Text = "The PART SSN"
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
        'UFCWDocsControl1
        '
        Me.UFCWDocsControl1.AppKey = Nothing
        Me.UFCWDocsControl1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.UFCWDocsControl1.Location = New System.Drawing.Point(12, 57)
        Me.UFCWDocsControl1.Mode = SearchMode.SearchSQLOnly
        Me.UFCWDocsControl1.Name = "UFCWDocsControl1"
        Me.UFCWDocsControl1.Size = New System.Drawing.Size(841, 562)
        Me.UFCWDocsControl1.TabIndex = 18
        '
        'TestForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ClientSize = New System.Drawing.Size(873, 631)
        Me.Controls.Add(Me.UFCWDocsControl1)
        Me.Controls.Add(Me.ReadOnlyCheckBox)
        Me.Controls.Add(Me.PartSSN)
        Me.Controls.Add(Me.ButtonClear)
        Me.Controls.Add(Me.ButtonRefresh)
        Me.Name = "TestForm"
        Me.Text = "TestForm"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ReadOnlyCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents PayerComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents UFCWDocsControl1 As UFCWDocsControl
End Class
