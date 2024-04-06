<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CoverageAndNetworkHistoryForm
    Inherits System.Windows.Forms.Form

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.HMONetworkHistoryDataGrid = New DataGridCustom()
        Me.ExitButton = New System.Windows.Forms.Button()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.txtHistoryDetail = New System.Windows.Forms.TextBox()
        CType(Me.HMONetworkHistoryDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'HMONetworkHistoryDataGrid
        '
        Me.HMONetworkHistoryDataGrid.ADGroupsThatCanCopy = ""
        Me.HMONetworkHistoryDataGrid.ADGroupsThatCanCustomize = ""
        Me.HMONetworkHistoryDataGrid.ADGroupsThatCanExport = ""
        Me.HMONetworkHistoryDataGrid.ADGroupsThatCanFilter = ""
        Me.HMONetworkHistoryDataGrid.ADGroupsThatCanFind = ""
        Me.HMONetworkHistoryDataGrid.ADGroupsThatCanMultiSort = ""
        Me.HMONetworkHistoryDataGrid.ADGroupsThatCanPrint = ""
        Me.HMONetworkHistoryDataGrid.AllowAutoSize = True
        Me.HMONetworkHistoryDataGrid.AllowColumnReorder = True
        Me.HMONetworkHistoryDataGrid.AllowCopy = False
        Me.HMONetworkHistoryDataGrid.AllowCustomize = True
        Me.HMONetworkHistoryDataGrid.AllowDelete = False
        Me.HMONetworkHistoryDataGrid.AllowDragDrop = False
        Me.HMONetworkHistoryDataGrid.AllowEdit = False
        Me.HMONetworkHistoryDataGrid.AllowExport = True
        Me.HMONetworkHistoryDataGrid.AllowFilter = True
        Me.HMONetworkHistoryDataGrid.AllowFind = True
        Me.HMONetworkHistoryDataGrid.AllowGoTo = True
        Me.HMONetworkHistoryDataGrid.AllowMultiSelect = False
        Me.HMONetworkHistoryDataGrid.AllowMultiSort = False
        Me.HMONetworkHistoryDataGrid.AllowNew = False
        Me.HMONetworkHistoryDataGrid.AllowPrint = False
        Me.HMONetworkHistoryDataGrid.AllowRefresh = True
        Me.HMONetworkHistoryDataGrid.AppKey = "UFCW\RegMaster\"
        Me.HMONetworkHistoryDataGrid.AutoSaveCols = True
        Me.HMONetworkHistoryDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.HMONetworkHistoryDataGrid.CaptionVisible = False
        Me.HMONetworkHistoryDataGrid.ColumnHeaderLabel = Nothing
        Me.HMONetworkHistoryDataGrid.ColumnRePositioning = False
        Me.HMONetworkHistoryDataGrid.ColumnResizing = False
        Me.HMONetworkHistoryDataGrid.ConfirmDelete = True
        Me.HMONetworkHistoryDataGrid.CopySelectedOnly = True
        Me.HMONetworkHistoryDataGrid.DataMember = ""
        Me.HMONetworkHistoryDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HMONetworkHistoryDataGrid.DragColumn = 0
        Me.HMONetworkHistoryDataGrid.ExportSelectedOnly = True
        Me.HMONetworkHistoryDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.HMONetworkHistoryDataGrid.HighlightedRow = Nothing
        Me.HMONetworkHistoryDataGrid.HighLightModifiedRows = False
        Me.HMONetworkHistoryDataGrid.IsMouseDown = False
        Me.HMONetworkHistoryDataGrid.LastGoToLine = ""
        Me.HMONetworkHistoryDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.HMONetworkHistoryDataGrid.MultiSort = False
        Me.HMONetworkHistoryDataGrid.Name = "HMONetworkHistoryDataGrid"
        Me.HMONetworkHistoryDataGrid.OldSelectedRow = Nothing
        Me.HMONetworkHistoryDataGrid.ReadOnly = True
        Me.HMONetworkHistoryDataGrid.RetainRowSelectionAfterSort = True
        Me.HMONetworkHistoryDataGrid.RowHeadersVisible = False
        Me.HMONetworkHistoryDataGrid.SetRowOnRightClick = True
        Me.HMONetworkHistoryDataGrid.ShiftPressed = False
        Me.HMONetworkHistoryDataGrid.SingleClickBooleanColumns = True
        Me.HMONetworkHistoryDataGrid.Size = New System.Drawing.Size(484, 210)
        Me.HMONetworkHistoryDataGrid.Sort = Nothing
        Me.HMONetworkHistoryDataGrid.StyleName = ""
        Me.HMONetworkHistoryDataGrid.SubKey = ""
        Me.HMONetworkHistoryDataGrid.SuppressTriangle = False
        Me.HMONetworkHistoryDataGrid.TabIndex = 0
        '
        'ExitButton
        '
        Me.ExitButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ExitButton.Location = New System.Drawing.Point(561, 216)
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
        Me.SplitContainer1.Location = New System.Drawing.Point(-2, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.HMONetworkHistoryDataGrid)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtHistoryDetail)
        Me.SplitContainer1.Size = New System.Drawing.Size(652, 210)
        Me.SplitContainer1.SplitterDistance = 484
        Me.SplitContainer1.TabIndex = 3
        '
        'txtHistoryDetail
        '
        Me.txtHistoryDetail.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtHistoryDetail.Location = New System.Drawing.Point(0, 0)
        Me.txtHistoryDetail.Multiline = True
        Me.txtHistoryDetail.Name = "txtHistoryDetail"
        Me.txtHistoryDetail.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtHistoryDetail.Size = New System.Drawing.Size(164, 210)
        Me.txtHistoryDetail.TabIndex = 0
        '
        'CoverageAndNetworkHistory
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ExitButton
        Me.ClientSize = New System.Drawing.Size(648, 251)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.ExitButton)
        Me.Name = "CoverageAndNetworkHistory"
        Me.Text = "History"
        CType(Me.HMONetworkHistoryDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents HMONetworkHistoryDataGrid As DataGridCustom
    Friend WithEvents ExitButton As System.Windows.Forms.Button
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents txtHistoryDetail As System.Windows.Forms.TextBox
End Class
