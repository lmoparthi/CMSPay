<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DentalHistory
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.RelationIDLabel = New System.Windows.Forms.Label()
        Me.FamilyIDTextBox = New System.Windows.Forms.TextBox()
        Me.FamilyIDLabel = New System.Windows.Forms.Label()
        Me.RelationIDTextBox = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SSNTextBox = New System.Windows.Forms.TextBox()
        Me.SSNCheckBox = New System.Windows.Forms.CheckBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.DentalControl1 = New DentalControl()
        Me.MainStatusBar = New System.Windows.Forms.StatusBar()
        Me.InfoStatusBarPanel = New System.Windows.Forms.StatusBarPanel()
        Me.DomainUserStatusBarPanel = New System.Windows.Forms.StatusBarPanel()
        Me.DataStatusBarPanel = New System.Windows.Forms.StatusBarPanel()
        Me.DateStatusBarPanel = New System.Windows.Forms.StatusBarPanel()
        CType(Me.InfoStatusBarPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DomainUserStatusBarPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataStatusBarPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateStatusBarPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RelationIDLabel
        '
        Me.RelationIDLabel.AutoSize = True
        Me.RelationIDLabel.Location = New System.Drawing.Point(474, 50)
        Me.RelationIDLabel.Name = "RelationIDLabel"
        Me.RelationIDLabel.Size = New System.Drawing.Size(60, 13)
        Me.RelationIDLabel.TabIndex = 1
        Me.RelationIDLabel.Text = "Relation ID"
        '
        'FamilyIDTextBox
        '
        Me.FamilyIDTextBox.Location = New System.Drawing.Point(120, 47)
        Me.FamilyIDTextBox.Name = "FamilyIDTextBox"
        Me.FamilyIDTextBox.Size = New System.Drawing.Size(100, 20)
        Me.FamilyIDTextBox.TabIndex = 0
        Me.FamilyIDTextBox.Text = "24887"
        '
        'FamilyIDLabel
        '
        Me.FamilyIDLabel.AutoSize = True
        Me.FamilyIDLabel.Location = New System.Drawing.Point(40, 47)
        Me.FamilyIDLabel.Name = "FamilyIDLabel"
        Me.FamilyIDLabel.Size = New System.Drawing.Size(50, 13)
        Me.FamilyIDLabel.TabIndex = 3
        Me.FamilyIDLabel.Text = "Family ID"
        '
        'RelationIDTextBox
        '
        Me.RelationIDTextBox.Location = New System.Drawing.Point(569, 47)
        Me.RelationIDTextBox.Name = "RelationIDTextBox"
        Me.RelationIDTextBox.Size = New System.Drawing.Size(73, 20)
        Me.RelationIDTextBox.TabIndex = 4
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(168, 105)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(112, 35)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Go"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'SSNTextBox
        '
        Me.SSNTextBox.Location = New System.Drawing.Point(320, 47)
        Me.SSNTextBox.Name = "SSNTextBox"
        Me.SSNTextBox.Size = New System.Drawing.Size(130, 20)
        Me.SSNTextBox.TabIndex = 7
        '
        'SSNCheckBox
        '
        Me.SSNCheckBox.AutoSize = True
        Me.SSNCheckBox.Location = New System.Drawing.Point(246, 50)
        Me.SSNCheckBox.Name = "SSNCheckBox"
        Me.SSNCheckBox.Size = New System.Drawing.Size(48, 17)
        Me.SSNCheckBox.TabIndex = 10
        Me.SSNCheckBox.Text = "SSN"
        Me.SSNCheckBox.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(310, 105)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(118, 35)
        Me.Button2.TabIndex = 11
        Me.Button2.Text = "Clear"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'DentalControl1
        '
        Me.DentalControl1.AutoScroll = True
        Me.DentalControl1.Location = New System.Drawing.Point(13, 181)
        Me.DentalControl1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.DentalControl1.Name = "DentalControl1"
        Me.DentalControl1.Size = New System.Drawing.Size(758, 618)
        Me.DentalControl1.TabIndex = 10
        '
        'MainStatusBar
        '
        Me.MainStatusBar.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.5!)
        Me.MainStatusBar.Location = New System.Drawing.Point(0, 802)
        Me.MainStatusBar.Name = "MainStatusBar"
        Me.MainStatusBar.Panels.AddRange(New System.Windows.Forms.StatusBarPanel() {Me.InfoStatusBarPanel, Me.DomainUserStatusBarPanel, Me.DataStatusBarPanel, Me.DateStatusBarPanel})
        Me.MainStatusBar.ShowPanels = True
        Me.MainStatusBar.Size = New System.Drawing.Size(800, 20)
        Me.MainStatusBar.TabIndex = 12
        Me.MainStatusBar.Text = "StatusBar1"
        '
        'InfoStatusBarPanel
        '
        Me.InfoStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring
        Me.InfoStatusBarPanel.Name = "InfoStatusBarPanel"
        Me.InfoStatusBarPanel.Width = 513
        '
        'DomainUserStatusBarPanel
        '
        Me.DomainUserStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.DomainUserStatusBarPanel.MinWidth = 120
        Me.DomainUserStatusBarPanel.Name = "DomainUserStatusBarPanel"
        Me.DomainUserStatusBarPanel.Width = 120
        '
        'DataStatusBarPanel
        '
        Me.DataStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.DataStatusBarPanel.MinWidth = 50
        Me.DataStatusBarPanel.Name = "DataStatusBarPanel"
        Me.DataStatusBarPanel.Width = 50
        '
        'DateStatusBarPanel
        '
        Me.DateStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.DateStatusBarPanel.MinWidth = 100
        Me.DateStatusBarPanel.Name = "DateStatusBarPanel"
        '
        'DentalHistory
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(800, 822)
        Me.Controls.Add(Me.MainStatusBar)
        Me.Controls.Add(Me.SSNTextBox)
        Me.Controls.Add(Me.SSNCheckBox)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.DentalControl1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.RelationIDTextBox)
        Me.Controls.Add(Me.FamilyIDLabel)
        Me.Controls.Add(Me.FamilyIDTextBox)
        Me.Controls.Add(Me.RelationIDLabel)
        Me.Name = "DentalHistory"
        Me.Text = "Dental History"
        CType(Me.InfoStatusBarPanel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DomainUserStatusBarPanel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataStatusBarPanel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateStatusBarPanel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RelationIDLabel As Label
    Friend WithEvents FamilyIDTextBox As TextBox
    Friend WithEvents FamilyIDLabel As Label
    Friend WithEvents RelationIDTextBox As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents SSNTextBox As TextBox
    Friend WithEvents SSNCheckBox As CheckBox
    Friend WithEvents DentalControl1 As DentalControl
    Friend WithEvents Button2 As Button
    Friend WithEvents MainStatusBar As StatusBar
    Friend WithEvents InfoStatusBarPanel As StatusBarPanel
    Friend WithEvents DomainUserStatusBarPanel As StatusBarPanel
    Friend WithEvents DataStatusBarPanel As StatusBarPanel
    Friend WithEvents DateStatusBarPanel As StatusBarPanel

End Class
