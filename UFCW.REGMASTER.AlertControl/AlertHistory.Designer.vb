<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AlertHistory
    Inherits System.Windows.Forms.Form

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.AlertHistoryDataGrid = New DataGridCustom()
        Me.ExitButton = New System.Windows.Forms.Button()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.txtHistory = New System.Windows.Forms.TextBox()
        CType(Me.AlertHistoryDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'AlertHistoryDataGrid
        '
        Me.AlertHistoryDataGrid.ADGroupsThatCanCopy = ""
        Me.AlertHistoryDataGrid.ADGroupsThatCanCustomize = ""
        Me.AlertHistoryDataGrid.ADGroupsThatCanExport = ""
        Me.AlertHistoryDataGrid.ADGroupsThatCanFilter = ""
        Me.AlertHistoryDataGrid.ADGroupsThatCanFind = ""
        Me.AlertHistoryDataGrid.ADGroupsThatCanPrint = ""
        Me.AlertHistoryDataGrid.ADGroupsThatCanMultiSort = ""
        Me.AlertHistoryDataGrid.AllowAutoSize = True
        Me.AlertHistoryDataGrid.AllowColumnReorder = True
        Me.AlertHistoryDataGrid.AllowCopy = False
        Me.AlertHistoryDataGrid.AllowCustomize = True
        Me.AlertHistoryDataGrid.AllowDelete = False
        Me.AlertHistoryDataGrid.AllowDragDrop = False
        Me.AlertHistoryDataGrid.AllowEdit = False
        Me.AlertHistoryDataGrid.AllowExport = True
        Me.AlertHistoryDataGrid.AllowFilter = True
        Me.AlertHistoryDataGrid.AllowFind = True
        Me.AlertHistoryDataGrid.AllowGoTo = True
        Me.AlertHistoryDataGrid.AllowMultiSelect = False
        Me.AlertHistoryDataGrid.AllowMultiSort = False
        Me.AlertHistoryDataGrid.AllowNew = False
        Me.AlertHistoryDataGrid.AllowPrint = False
        Me.AlertHistoryDataGrid.AllowRefresh = True
        Me.AlertHistoryDataGrid.AppKey = "UFCW\RegMaster\"
        Me.AlertHistoryDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.AlertHistoryDataGrid.CaptionVisible = False
        Me.AlertHistoryDataGrid.ColumnHeaderLabel = Nothing
        Me.AlertHistoryDataGrid.ColumnRePositioning = False
        Me.AlertHistoryDataGrid.ColumnResizing = False
        Me.AlertHistoryDataGrid.ConfirmDelete = True
        Me.AlertHistoryDataGrid.CopySelectedOnly = True
        Me.AlertHistoryDataGrid.DataMember = ""
        Me.AlertHistoryDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AlertHistoryDataGrid.DragColumn = 0
        Me.AlertHistoryDataGrid.ExportSelectedOnly = True
        Me.AlertHistoryDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.AlertHistoryDataGrid.HighlightedRow = Nothing
        Me.AlertHistoryDataGrid.IsMouseDown = False
        Me.AlertHistoryDataGrid.LastGoToLine = ""
        Me.AlertHistoryDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.AlertHistoryDataGrid.MultiSort = False
        Me.AlertHistoryDataGrid.Name = "AlertHistoryDataGrid"
        Me.AlertHistoryDataGrid.OldSelectedRow = Nothing
        Me.AlertHistoryDataGrid.ReadOnly = True
        Me.AlertHistoryDataGrid.RowHeadersVisible = False
        Me.AlertHistoryDataGrid.SetRowOnRightClick = True
        Me.AlertHistoryDataGrid.ShiftPressed = False
        Me.AlertHistoryDataGrid.SingleClickBooleanColumns = True
        Me.AlertHistoryDataGrid.Size = New System.Drawing.Size(192, 224)
        Me.AlertHistoryDataGrid.StyleName = ""
        Me.AlertHistoryDataGrid.SubKey = ""
        Me.AlertHistoryDataGrid.SuppressTriangle = False
        Me.AlertHistoryDataGrid.TabIndex = 0
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
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(-1, 1)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.AlertHistoryDataGrid)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtHistory)
        Me.SplitContainer1.Size = New System.Drawing.Size(297, 224)
        Me.SplitContainer1.SplitterDistance = 192
        Me.SplitContainer1.TabIndex = 3
        '
        'txtHistory
        '
        Me.txtHistory.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtHistory.Location = New System.Drawing.Point(0, 0)
        Me.txtHistory.Multiline = True
        Me.txtHistory.Name = "txtHistory"
        Me.txtHistory.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtHistory.Size = New System.Drawing.Size(101, 224)
        Me.txtHistory.TabIndex = 0
        '
        'AlertHistory
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ExitButton
        Me.ClientSize = New System.Drawing.Size(292, 266)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.ExitButton)
        Me.Name = "AlertHistory"
        Me.Text = "History"
        CType(Me.AlertHistoryDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents AlertHistoryDataGrid As DataGridCustom
    Friend WithEvents ExitButton As System.Windows.Forms.Button
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents txtHistory As System.Windows.Forms.TextBox
End Class
