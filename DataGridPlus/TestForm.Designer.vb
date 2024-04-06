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

    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.LoadStyleButton = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.TestDataGrid = New DataGridCustom()
        Me.DefaultButton = New System.Windows.Forms.Button()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        CType(Me.TestDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LoadStyleButton
        '
        Me.LoadStyleButton.Location = New System.Drawing.Point(449, 28)
        Me.LoadStyleButton.Name = "LoadStyleButton"
        Me.LoadStyleButton.Size = New System.Drawing.Size(75, 23)
        Me.LoadStyleButton.TabIndex = 1
        Me.LoadStyleButton.Text = "Load Style"
        Me.LoadStyleButton.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(13, 28)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(391, 20)
        Me.TextBox1.TabIndex = 2
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(410, 28)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(24, 23)
        Me.Button2.TabIndex = 3
        Me.Button2.Text = "..."
        Me.Button2.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.Filter = "xml files (*.xml)|*.xml|txt files (*.txt)|*.txt|All files (*.*)|*.*"
        Me.OpenFileDialog1.InitialDirectory = "C:\"
        Me.OpenFileDialog1.RestoreDirectory = True
        '
        'TestDataGrid
        '
        Me.TestDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.TestDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.TestDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.TestDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.TestDataGrid.ADGroupsThatCanFind = ""
        Me.TestDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.TestDataGrid.ADGroupsThatCanMultiSort = ""
        Me.TestDataGrid.AllowAutoSize = True
        Me.TestDataGrid.AllowColumnReorder = True
        Me.TestDataGrid.AllowCopy = True
        Me.TestDataGrid.AllowCustomize = True
        Me.TestDataGrid.AllowDelete = True
        Me.TestDataGrid.AllowDragDrop = False
        Me.TestDataGrid.AllowEdit = True
        Me.TestDataGrid.AllowExport = True
        Me.TestDataGrid.AllowFilter = True
        Me.TestDataGrid.AllowFind = True
        Me.TestDataGrid.AllowGoTo = True
        Me.TestDataGrid.AllowMultiSelect = True
        Me.TestDataGrid.AllowMultiSort = True
        Me.TestDataGrid.AllowNew = True
        Me.TestDataGrid.AllowPrint = True
        Me.TestDataGrid.AllowRefresh = True
        Me.TestDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TestDataGrid.AppKey = "UFCW\Claims\"
        Me.TestDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.TestDataGrid.ColumnHeaderLabel = Nothing
        Me.TestDataGrid.ColumnRePositioning = False
        Me.TestDataGrid.ColumnResizing = False
        Me.TestDataGrid.ConfirmDelete = True
        Me.TestDataGrid.CopySelectedOnly = True
        Me.TestDataGrid.DataMember = ""
        Me.TestDataGrid.DragColumn = 0
        Me.TestDataGrid.ExportSelectedOnly = True
        Me.TestDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.TestDataGrid.IsMouseDown = False
        Me.TestDataGrid.LastGoToLine = ""
        Me.TestDataGrid.Location = New System.Drawing.Point(12, 82)
        Me.TestDataGrid.MultiSort = False
        Me.TestDataGrid.Name = "TestDataGrid"
        Me.TestDataGrid.OldSelectedRow = 0
        Me.TestDataGrid.ReadOnly = True
        Me.TestDataGrid.SetRowOnRightClick = True
        Me.TestDataGrid.ShiftPressed = False
        Me.TestDataGrid.SingleClickBooleanColumns = True
        Me.TestDataGrid.Size = New System.Drawing.Size(631, 298)
        Me.TestDataGrid.StyleName = ""
        Me.TestDataGrid.SubKey = ""
        Me.TestDataGrid.SuppressTriangle = False
        Me.TestDataGrid.TabIndex = 0
        '
        'DefaultButton
        '
        Me.DefaultButton.Location = New System.Drawing.Point(543, 28)
        Me.DefaultButton.Name = "DefaultButton"
        Me.DefaultButton.Size = New System.Drawing.Size(75, 23)
        Me.DefaultButton.TabIndex = 4
        Me.DefaultButton.Text = "Default"
        Me.DefaultButton.UseVisualStyleBackColor = True
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(449, 59)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(78, 17)
        Me.CheckBox1.TabIndex = 5
        Me.CheckBox1.Text = "MultiSelect"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'TestForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(655, 392)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.DefaultButton)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.LoadStyleButton)
        Me.Controls.Add(Me.TestDataGrid)
        Me.Name = "TestForm"
        Me.Text = "TestForm"
        CType(Me.TestDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TestDataGrid As DataGridCustom
    Friend WithEvents LoadStyleButton As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents DefaultButton As System.Windows.Forms.Button
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
End Class
