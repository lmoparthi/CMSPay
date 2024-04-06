<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AlertsHistoryViewerForm
    Inherits System.Windows.Forms.Form

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ExitButton = New System.Windows.Forms.Button()
        Me.AlertDataGrid = New DataGridCustom()
        CType(Me.AlertDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ExitButton
        '
        Me.ExitButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ExitButton.Location = New System.Drawing.Point(385, 273)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(75, 23)
        Me.ExitButton.TabIndex = 1
        Me.ExitButton.Text = "Exit"
        Me.ExitButton.UseVisualStyleBackColor = True
        '
        'AlertDataGrid
        '
        Me.AlertDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.AlertDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.AlertDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.AlertDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.AlertDataGrid.ADGroupsThatCanFind = ""
        Me.AlertDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.AlertDataGrid.ADGroupsThatCanMultiSort = ""
        Me.AlertDataGrid.AllowAutoSize = True
        Me.AlertDataGrid.AllowColumnReorder = True
        Me.AlertDataGrid.AllowCopy = True
        Me.AlertDataGrid.AllowCustomize = True
        Me.AlertDataGrid.AllowDelete = False
        Me.AlertDataGrid.AllowDragDrop = False
        Me.AlertDataGrid.AllowEdit = False
        Me.AlertDataGrid.AllowExport = True
        Me.AlertDataGrid.AllowFilter = True
        Me.AlertDataGrid.AllowFind = True
        Me.AlertDataGrid.AllowGoTo = True
        Me.AlertDataGrid.AllowMultiSelect = False
        Me.AlertDataGrid.AllowMultiSort = False
        Me.AlertDataGrid.AllowNew = False
        Me.AlertDataGrid.AllowPrint = True
        Me.AlertDataGrid.AllowRefresh = False
        Me.AlertDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AlertDataGrid.AppKey = "UFCW\Claims\"
        Me.AlertDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.AlertDataGrid.CaptionVisible = False
        Me.AlertDataGrid.ColumnHeaderLabel = Nothing
        Me.AlertDataGrid.ColumnRePositioning = False
        Me.AlertDataGrid.ColumnResizing = False
        Me.AlertDataGrid.ConfirmDelete = True
        Me.AlertDataGrid.CopySelectedOnly = True
        Me.AlertDataGrid.DataMember = ""
        Me.AlertDataGrid.DragColumn = 0
        Me.AlertDataGrid.ExportSelectedOnly = True
        Me.AlertDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.AlertDataGrid.HighlightedRow = Nothing
        Me.AlertDataGrid.IsMouseDown = False
        Me.AlertDataGrid.LastGoToLine = ""
        Me.AlertDataGrid.Location = New System.Drawing.Point(1, 2)
        Me.AlertDataGrid.MultiSort = False
        Me.AlertDataGrid.Name = "AlertDataGrid"
        Me.AlertDataGrid.OldSelectedRow = Nothing
        Me.AlertDataGrid.ReadOnly = True
        Me.AlertDataGrid.SetRowOnRightClick = True
        Me.AlertDataGrid.ShiftPressed = False
        Me.AlertDataGrid.SingleClickBooleanColumns = True
        Me.AlertDataGrid.Size = New System.Drawing.Size(468, 264)
        Me.AlertDataGrid.StyleName = ""
        Me.AlertDataGrid.SubKey = ""
        Me.AlertDataGrid.SuppressTriangle = False
        Me.AlertDataGrid.TabIndex = 2
        '
        'AlertsHistoryViewerForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.ExitButton
        Me.ClientSize = New System.Drawing.Size(472, 302)
        Me.Controls.Add(Me.AlertDataGrid)
        Me.Controls.Add(Me.ExitButton)
        Me.Name = "AlertsHistoryViewerForm"
        Me.Text = "Alerts History"
        CType(Me.AlertDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ExitButton As System.Windows.Forms.Button
    Friend WithEvents AlertsHistoryDataGrid As DataGridCustom
    Friend WithEvents AlertDataGrid As DataGridCustom
End Class
