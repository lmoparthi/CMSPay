
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class History
    Inherits System.Windows.Forms.Form


    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.COBHistoryDataGrid = New DataGridCustom
        Me.ExitButton = New System.Windows.Forms.Button
        CType(Me.COBHistoryDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'COBHistoryDataGrid
        '
        Me.COBHistoryDataGrid.AllowColumnReorder = True
        Me.COBHistoryDataGrid.AllowCopy = False
        Me.COBHistoryDataGrid.AllowDelete = False
        Me.COBHistoryDataGrid.AllowDragDrop = False
        Me.COBHistoryDataGrid.AllowEdit = False
        Me.COBHistoryDataGrid.AllowExport = True
        Me.COBHistoryDataGrid.AllowFind = True
        Me.COBHistoryDataGrid.AllowGoTo = True
        Me.COBHistoryDataGrid.AllowMultiSelect = False
        Me.COBHistoryDataGrid.AllowMultiSort = False
        Me.COBHistoryDataGrid.AllowNew = False
        Me.COBHistoryDataGrid.AllowPrint = False
        Me.COBHistoryDataGrid.AllowRefresh = False
        Me.COBHistoryDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.COBHistoryDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.COBHistoryDataGrid.CaptionVisible = False
        Me.COBHistoryDataGrid.ConfirmDelete = True
        Me.COBHistoryDataGrid.CopySelectedOnly = True
        Me.COBHistoryDataGrid.DataMember = ""
        Me.COBHistoryDataGrid.ExportSelectedOnly = True
        Me.COBHistoryDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.COBHistoryDataGrid.LastGoToLine = ""
        Me.COBHistoryDataGrid.Location = New System.Drawing.Point(12, 12)
        Me.COBHistoryDataGrid.MultiSort = False
        Me.COBHistoryDataGrid.Name = "COBHistoryDataGrid"
        Me.COBHistoryDataGrid.ReadOnly = True
        Me.COBHistoryDataGrid.RowHeadersVisible = False
        Me.COBHistoryDataGrid.SetRowOnRightClick = True
        Me.COBHistoryDataGrid.SingleClickBooleanColumns = True
        Me.COBHistoryDataGrid.Size = New System.Drawing.Size(268, 213)
        Me.COBHistoryDataGrid.TabIndex = 0
        '
        'ExitButton
        '
        Me.ExitButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ExitButton.Location = New System.Drawing.Point(205, 231)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(75, 23)
        Me.ExitButton.TabIndex = 2
        Me.ExitButton.Text = "Exit"
        Me.ExitButton.UseVisualStyleBackColor = True
        '
        'History
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ExitButton
        Me.ClientSize = New System.Drawing.Size(292, 266)
        Me.Controls.Add(Me.ExitButton)
        Me.Controls.Add(Me.COBHistoryDataGrid)
        Me.Name = "History"
        Me.Text = "History"
        CType(Me.COBHistoryDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents COBHistoryDataGrid As DataGridCustom
    Friend WithEvents ExitButton As System.Windows.Forms.Button
End Class
