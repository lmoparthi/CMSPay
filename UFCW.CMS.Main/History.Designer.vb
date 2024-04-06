<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class History
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
        Me.HistoryDataGrid = New DataGridPlus.DataGridCustom
        Me.RefreshButton = New System.Windows.Forms.Button
        Me.ExitButton = New System.Windows.Forms.Button
        CType(Me.HistoryDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'HistoryDataGrid
        '
        Me.HistoryDataGrid.AllowColumnReorder = True
        Me.HistoryDataGrid.AllowCopy = True
        Me.HistoryDataGrid.AllowDelete = True
        Me.HistoryDataGrid.AllowDragDrop = False
        Me.HistoryDataGrid.AllowEdit = True
        Me.HistoryDataGrid.AllowExport = True
        Me.HistoryDataGrid.AllowFind = True
        Me.HistoryDataGrid.AllowGoTo = True
        Me.HistoryDataGrid.AllowMultiSelect = True
        Me.HistoryDataGrid.AllowMultiSort = False
        Me.HistoryDataGrid.AllowNew = True
        Me.HistoryDataGrid.AllowPrint = True
        Me.HistoryDataGrid.AllowRefresh = True
        Me.HistoryDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HistoryDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.HistoryDataGrid.ConfirmDelete = True
        Me.HistoryDataGrid.CopySelectedOnly = True
        Me.HistoryDataGrid.DataMember = ""
        Me.HistoryDataGrid.ExportSelectedOnly = True
        Me.HistoryDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.HistoryDataGrid.LastGoToLine = ""
        Me.HistoryDataGrid.Location = New System.Drawing.Point(12, 12)
        Me.HistoryDataGrid.MultiSort = False
        Me.HistoryDataGrid.Name = "HistoryDataGrid"
        Me.HistoryDataGrid.SetRowOnRightClick = True
        Me.HistoryDataGrid.SingleClickBooleanColumns = True
        Me.HistoryDataGrid.Size = New System.Drawing.Size(268, 213)
        Me.HistoryDataGrid.TabIndex = 0
        '
        'RefreshButton
        '
        Me.RefreshButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RefreshButton.Location = New System.Drawing.Point(124, 231)
        Me.RefreshButton.Name = "RefreshButton"
        Me.RefreshButton.Size = New System.Drawing.Size(75, 23)
        Me.RefreshButton.TabIndex = 1
        Me.RefreshButton.Text = "Refresh"
        Me.RefreshButton.UseVisualStyleBackColor = True
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
        Me.Controls.Add(Me.RefreshButton)
        Me.Controls.Add(Me.HistoryDataGrid)
        Me.Name = "History"
        Me.Text = "History"
        CType(Me.HistoryDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents HistoryDataGrid As DataGridPlus.DataGridCustom
    Friend WithEvents RefreshButton As System.Windows.Forms.Button
    Friend WithEvents ExitButton As System.Windows.Forms.Button
End Class
