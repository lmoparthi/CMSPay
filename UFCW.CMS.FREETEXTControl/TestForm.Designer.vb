<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TestForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.GroupBox10 = New System.Windows.Forms.GroupBox
        Me.NotesRelationIDTextBox = New System.Windows.Forms.TextBox
        Me.Label82 = New System.Windows.Forms.Label
        Me.NotesPatSSNTextBox = New System.Windows.Forms.TextBox
        Me.Label83 = New System.Windows.Forms.Label
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.NotesFamilyIDTextBox = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.NotesPartSSNTextBox = New System.Windows.Forms.TextBox
        Me.Label72 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.ReadOnlyCheckBox = New System.Windows.Forms.CheckBox
        Me.FreeTextEditor1 = New FreeTextEditor
        Me.GroupBox10.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox10
        '
        Me.GroupBox10.Controls.Add(Me.NotesRelationIDTextBox)
        Me.GroupBox10.Controls.Add(Me.Label82)
        Me.GroupBox10.Controls.Add(Me.NotesPatSSNTextBox)
        Me.GroupBox10.Controls.Add(Me.Label83)
        Me.GroupBox10.Location = New System.Drawing.Point(260, 12)
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.Size = New System.Drawing.Size(256, 40)
        Me.GroupBox10.TabIndex = 17
        Me.GroupBox10.TabStop = False
        Me.GroupBox10.Text = "Patient:"
        '
        'NotesRelationIDTextBox
        '
        Me.NotesRelationIDTextBox.Location = New System.Drawing.Point(68, 16)
        Me.NotesRelationIDTextBox.Name = "NotesRelationIDTextBox"
        Me.NotesRelationIDTextBox.Size = New System.Drawing.Size(72, 20)
        Me.NotesRelationIDTextBox.TabIndex = 0
        '
        'Label82
        '
        Me.Label82.AutoSize = True
        Me.Label82.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label82.Location = New System.Drawing.Point(4, 20)
        Me.Label82.Name = "Label82"
        Me.Label82.Size = New System.Drawing.Size(60, 13)
        Me.Label82.TabIndex = 16
        Me.Label82.Text = "Relation ID"
        '
        'NotesPatSSNTextBox
        '
        Me.NotesPatSSNTextBox.Location = New System.Drawing.Point(180, 16)
        Me.NotesPatSSNTextBox.MaxLength = 11
        Me.NotesPatSSNTextBox.Name = "NotesPatSSNTextBox"
        Me.NotesPatSSNTextBox.Size = New System.Drawing.Size(72, 20)
        Me.NotesPatSSNTextBox.TabIndex = 1
        '
        'Label83
        '
        Me.Label83.AutoSize = True
        Me.Label83.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label83.Location = New System.Drawing.Point(148, 20)
        Me.Label83.Name = "Label83"
        Me.Label83.Size = New System.Drawing.Size(29, 13)
        Me.Label83.TabIndex = 14
        Me.Label83.Text = "SSN"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.NotesFamilyIDTextBox)
        Me.GroupBox4.Controls.Add(Me.Label12)
        Me.GroupBox4.Controls.Add(Me.NotesPartSSNTextBox)
        Me.GroupBox4.Controls.Add(Me.Label72)
        Me.GroupBox4.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(248, 40)
        Me.GroupBox4.TabIndex = 16
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Family:"
        '
        'NotesFamilyIDTextBox
        '
        Me.NotesFamilyIDTextBox.Location = New System.Drawing.Point(60, 16)
        Me.NotesFamilyIDTextBox.Name = "NotesFamilyIDTextBox"
        Me.NotesFamilyIDTextBox.Size = New System.Drawing.Size(72, 20)
        Me.NotesFamilyIDTextBox.TabIndex = 0
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label12.Location = New System.Drawing.Point(4, 20)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(50, 13)
        Me.Label12.TabIndex = 16
        Me.Label12.Text = "Family ID"
        '
        'NotesPartSSNTextBox
        '
        Me.NotesPartSSNTextBox.Location = New System.Drawing.Point(172, 16)
        Me.NotesPartSSNTextBox.MaxLength = 11
        Me.NotesPartSSNTextBox.Name = "NotesPartSSNTextBox"
        Me.NotesPartSSNTextBox.Size = New System.Drawing.Size(72, 20)
        Me.NotesPartSSNTextBox.TabIndex = 1
        '
        'Label72
        '
        Me.Label72.AutoSize = True
        Me.Label72.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label72.Location = New System.Drawing.Point(140, 20)
        Me.Label72.Name = "Label72"
        Me.Label72.Size = New System.Drawing.Size(29, 13)
        Me.Label72.TabIndex = 14
        Me.Label72.Text = "SSN"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(551, 28)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 18
        Me.Button1.Text = "Go"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ReadOnlyCheckBox
        '
        Me.ReadOnlyCheckBox.AutoSize = True
        Me.ReadOnlyCheckBox.Location = New System.Drawing.Point(542, 77)
        Me.ReadOnlyCheckBox.Name = "ReadOnlyCheckBox"
        Me.ReadOnlyCheckBox.Size = New System.Drawing.Size(76, 17)
        Me.ReadOnlyCheckBox.TabIndex = 20
        Me.ReadOnlyCheckBox.Text = "Read Only"
        Me.ReadOnlyCheckBox.UseVisualStyleBackColor = True
        '
        'FreeTextEditor1
        '
        Me.FreeTextEditor1.AppKey = "UFCW\Claims\"
        Me.FreeTextEditor1.AutoScroll = True
        Me.FreeTextEditor1.BackColor = System.Drawing.SystemColors.Control
        Me.FreeTextEditor1.Location = New System.Drawing.Point(12, 58)
        Me.FreeTextEditor1.Name = "FreeTextEditor1"
        Me.FreeTextEditor1.Size = New System.Drawing.Size(508, 552)
        Me.FreeTextEditor1.TabIndex = 21
        '
        'TestForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(653, 616)
        Me.Controls.Add(Me.FreeTextEditor1)
        Me.Controls.Add(Me.ReadOnlyCheckBox)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.GroupBox10)
        Me.Controls.Add(Me.GroupBox4)
        Me.Name = "TestForm"
        Me.Text = "TestForm"
        Me.GroupBox10.ResumeLayout(False)
        Me.GroupBox10.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox10 As System.Windows.Forms.GroupBox
    Private WithEvents NotesRelationIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label82 As System.Windows.Forms.Label
    Private WithEvents NotesPatSSNTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label83 As System.Windows.Forms.Label
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Private WithEvents NotesFamilyIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Private WithEvents NotesPartSSNTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label72 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ReadOnlyCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents FreeTextEditor1 As FreeTextEditor
End Class
