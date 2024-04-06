<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMainframeView
    Inherits System.Windows.Forms.Form

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMainframeView))
        Me.ExitButton = New System.Windows.Forms.Button()
        Me.MainframeviewDataGrid = New DataGridCustom()
        CType(Me.MainframeviewDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ExitButton
        '
        Me.ExitButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ExitButton.Location = New System.Drawing.Point(382, 230)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(76, 23)
        Me.ExitButton.TabIndex = 3
        Me.ExitButton.Text = "Exit"
        Me.ExitButton.UseVisualStyleBackColor = True
        '
        'MainframeviewDataGrid
        '
        Me.MainframeviewDataGrid.ADGroupsThatCanCopy = "REGMUser,CMSUsers"
        Me.MainframeviewDataGrid.ADGroupsThatCanCustomize = "REGMUser,CMSUsers"
        Me.MainframeviewDataGrid.ADGroupsThatCanExport = "REGMUser,CMSUsers"
        Me.MainframeviewDataGrid.ADGroupsThatCanFilter = "REGMUser,CMSUsers"
        Me.MainframeviewDataGrid.ADGroupsThatCanFind = ""
        Me.MainframeviewDataGrid.ADGroupsThatCanMultiSort = "REGMUser,CMSUsers"
        Me.MainframeviewDataGrid.ADGroupsThatCanPrint = ""
        Me.MainframeviewDataGrid.AllowAutoSize = True
        Me.MainframeviewDataGrid.AllowColumnReorder = True
        Me.MainframeviewDataGrid.AllowCopy = True
        Me.MainframeviewDataGrid.AllowCustomize = True
        Me.MainframeviewDataGrid.AllowDelete = False
        Me.MainframeviewDataGrid.AllowDragDrop = True
        Me.MainframeviewDataGrid.AllowEdit = False
        Me.MainframeviewDataGrid.AllowExport = True
        Me.MainframeviewDataGrid.AllowFilter = True
        Me.MainframeviewDataGrid.AllowFind = True
        Me.MainframeviewDataGrid.AllowGoTo = True
        Me.MainframeviewDataGrid.AllowMultiSelect = True
        Me.MainframeviewDataGrid.AllowMultiSort = False
        Me.MainframeviewDataGrid.AllowNew = False
        Me.MainframeviewDataGrid.AllowPrint = True
        Me.MainframeviewDataGrid.AllowRefresh = False
        Me.MainframeviewDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MainframeviewDataGrid.AppKey = "UFCW\RegMaster\"
        Me.MainframeviewDataGrid.AutoSaveCols = True
        Me.MainframeviewDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.MainframeviewDataGrid.ColumnHeaderLabel = Nothing
        Me.MainframeviewDataGrid.ColumnRePositioning = False
        Me.MainframeviewDataGrid.ColumnResizing = False
        Me.MainframeviewDataGrid.ConfirmDelete = True
        Me.MainframeviewDataGrid.CopySelectedOnly = True
        Me.MainframeviewDataGrid.DataMember = ""
        Me.MainframeviewDataGrid.DragColumn = 0
        Me.MainframeviewDataGrid.ExportSelectedOnly = True
        Me.MainframeviewDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.MainframeviewDataGrid.HighlightedRow = Nothing
        Me.MainframeviewDataGrid.HighLightModifiedRows = False
        Me.MainframeviewDataGrid.IsMouseDown = False
        Me.MainframeviewDataGrid.LastGoToLine = ""
        Me.MainframeviewDataGrid.Location = New System.Drawing.Point(1, 2)
        Me.MainframeviewDataGrid.MultiSort = False
        Me.MainframeviewDataGrid.Name = "MainframeviewDataGrid"
        Me.MainframeviewDataGrid.OldSelectedRow = 0
        Me.MainframeviewDataGrid.ReadOnly = True
        Me.MainframeviewDataGrid.RetainRowSelectionAfterSort = True
        Me.MainframeviewDataGrid.SetRowOnRightClick = True
        Me.MainframeviewDataGrid.ShiftPressed = False
        Me.MainframeviewDataGrid.SingleClickBooleanColumns = True
        Me.MainframeviewDataGrid.Size = New System.Drawing.Size(457, 222)
        Me.MainframeviewDataGrid.Sort = Nothing
        Me.MainframeviewDataGrid.StyleName = ""
        Me.MainframeviewDataGrid.SubKey = ""
        Me.MainframeviewDataGrid.SuppressTriangle = False
        Me.MainframeviewDataGrid.TabIndex = 2
        '
        'frmMainframeView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(459, 254)
        Me.Controls.Add(Me.ExitButton)
        Me.Controls.Add(Me.MainframeviewDataGrid)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMainframeView"
        Me.Text = "Total Hours View"
        CType(Me.MainframeviewDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ExitButton As System.Windows.Forms.Button
    Friend WithEvents MainframeviewDataGrid As DataGridCustom
End Class
