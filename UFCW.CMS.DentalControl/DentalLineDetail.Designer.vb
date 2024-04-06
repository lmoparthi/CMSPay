<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DentalLineDetail
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
        Me.components = New System.ComponentModel.Container()
        Me.DetailSplitContainer = New System.Windows.Forms.SplitContainer()
        Me.TotalGroupBox = New System.Windows.Forms.GroupBox()
        Me.TotalPaidTextBox = New System.Windows.Forms.TextBox()
        Me.Label69 = New System.Windows.Forms.Label()
        Me.TotalOtherTextBox = New System.Windows.Forms.TextBox()
        Me.Label68 = New System.Windows.Forms.Label()
        Me.TotalChargesTextBox = New System.Windows.Forms.TextBox()
        Me.Label67 = New System.Windows.Forms.Label()
        Me.DentalLineDetailDataGrid = New DataGridCustom()
        Me.DentalAccumulatorValues = New DentalAccumulatorValues()
        CType(Me.DetailSplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DetailSplitContainer.Panel1.SuspendLayout()
        Me.DetailSplitContainer.Panel2.SuspendLayout()
        Me.DetailSplitContainer.SuspendLayout()
        Me.TotalGroupBox.SuspendLayout()
        CType(Me.DentalLineDetailDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DetailSplitContainer
        '
        Me.DetailSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DetailSplitContainer.Location = New System.Drawing.Point(0, 0)
        Me.DetailSplitContainer.Name = "DetailSplitContainer"
        Me.DetailSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'DetailSplitContainer.Panel1
        '
        Me.DetailSplitContainer.Panel1.Controls.Add(Me.TotalGroupBox)
        Me.DetailSplitContainer.Panel1.Controls.Add(Me.DentalLineDetailDataGrid)
        '
        'DetailSplitContainer.Panel2
        '
        Me.DetailSplitContainer.Panel2.Controls.Add(Me.DentalAccumulatorValues)
        Me.DetailSplitContainer.Size = New System.Drawing.Size(1034, 781)
        Me.DetailSplitContainer.SplitterDistance = 334
        Me.DetailSplitContainer.SplitterWidth = 8
        Me.DetailSplitContainer.TabIndex = 74
        '
        'TotalGroupBox
        '
        Me.TotalGroupBox.Controls.Add(Me.TotalPaidTextBox)
        Me.TotalGroupBox.Controls.Add(Me.Label69)
        Me.TotalGroupBox.Controls.Add(Me.TotalOtherTextBox)
        Me.TotalGroupBox.Controls.Add(Me.Label68)
        Me.TotalGroupBox.Controls.Add(Me.TotalChargesTextBox)
        Me.TotalGroupBox.Controls.Add(Me.Label67)
        Me.TotalGroupBox.Dock = System.Windows.Forms.DockStyle.Top
        Me.TotalGroupBox.Location = New System.Drawing.Point(0, 0)
        Me.TotalGroupBox.Name = "TotalGroupBox"
        Me.TotalGroupBox.Size = New System.Drawing.Size(1034, 32)
        Me.TotalGroupBox.TabIndex = 11
        Me.TotalGroupBox.TabStop = False
        '
        'TotalPaidTextBox
        '
        Me.TotalPaidTextBox.BackColor = System.Drawing.SystemColors.Window
        Me.TotalPaidTextBox.Location = New System.Drawing.Point(364, 9)
        Me.TotalPaidTextBox.Name = "TotalPaidTextBox"
        Me.TotalPaidTextBox.ReadOnly = True
        Me.TotalPaidTextBox.Size = New System.Drawing.Size(83, 20)
        Me.TotalPaidTextBox.TabIndex = 92
        Me.TotalPaidTextBox.TabStop = False
        '
        'Label69
        '
        Me.Label69.AutoSize = True
        Me.Label69.Location = New System.Drawing.Point(330, 12)
        Me.Label69.Name = "Label69"
        Me.Label69.Size = New System.Drawing.Size(28, 13)
        Me.Label69.TabIndex = 95
        Me.Label69.Text = "Paid"
        Me.Label69.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TotalOtherTextBox
        '
        Me.TotalOtherTextBox.BackColor = System.Drawing.SystemColors.Window
        Me.TotalOtherTextBox.Location = New System.Drawing.Point(206, 9)
        Me.TotalOtherTextBox.Name = "TotalOtherTextBox"
        Me.TotalOtherTextBox.ReadOnly = True
        Me.TotalOtherTextBox.Size = New System.Drawing.Size(83, 20)
        Me.TotalOtherTextBox.TabIndex = 91
        Me.TotalOtherTextBox.TabStop = False
        '
        'Label68
        '
        Me.Label68.AutoSize = True
        Me.Label68.Location = New System.Drawing.Point(167, 11)
        Me.Label68.Name = "Label68"
        Me.Label68.Size = New System.Drawing.Size(33, 13)
        Me.Label68.TabIndex = 94
        Me.Label68.Text = "Other"
        Me.Label68.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TotalChargesTextBox
        '
        Me.TotalChargesTextBox.BackColor = System.Drawing.SystemColors.Window
        Me.TotalChargesTextBox.Location = New System.Drawing.Point(57, 9)
        Me.TotalChargesTextBox.Name = "TotalChargesTextBox"
        Me.TotalChargesTextBox.ReadOnly = True
        Me.TotalChargesTextBox.Size = New System.Drawing.Size(83, 20)
        Me.TotalChargesTextBox.TabIndex = 90
        Me.TotalChargesTextBox.TabStop = False
        '
        'Label67
        '
        Me.Label67.AutoSize = True
        Me.Label67.Location = New System.Drawing.Point(3, 12)
        Me.Label67.Name = "Label67"
        Me.Label67.Size = New System.Drawing.Size(46, 13)
        Me.Label67.TabIndex = 93
        Me.Label67.Text = "Charges"
        Me.Label67.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'DentalLineDetailDataGrid
        '
        Me.DentalLineDetailDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.DentalLineDetailDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.DentalLineDetailDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DentalLineDetailDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DentalLineDetailDataGrid.ADGroupsThatCanFind = ""
        Me.DentalLineDetailDataGrid.ADGroupsThatCanMultiSort = ""
        Me.DentalLineDetailDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DentalLineDetailDataGrid.AllowAutoSize = True
        Me.DentalLineDetailDataGrid.AllowColumnReorder = True
        Me.DentalLineDetailDataGrid.AllowCopy = True
        Me.DentalLineDetailDataGrid.AllowCustomize = True
        Me.DentalLineDetailDataGrid.AllowDelete = True
        Me.DentalLineDetailDataGrid.AllowDragDrop = False
        Me.DentalLineDetailDataGrid.AllowEdit = True
        Me.DentalLineDetailDataGrid.AllowExport = True
        Me.DentalLineDetailDataGrid.AllowFilter = True
        Me.DentalLineDetailDataGrid.AllowFind = True
        Me.DentalLineDetailDataGrid.AllowGoTo = True
        Me.DentalLineDetailDataGrid.AllowMultiSelect = True
        Me.DentalLineDetailDataGrid.AllowMultiSort = False
        Me.DentalLineDetailDataGrid.AllowNew = True
        Me.DentalLineDetailDataGrid.AllowPrint = True
        Me.DentalLineDetailDataGrid.AllowRefresh = True
        Me.DentalLineDetailDataGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DentalLineDetailDataGrid.AppKey = "UFCW\Claims\"
        Me.DentalLineDetailDataGrid.AutoSaveCols = True
        Me.DentalLineDetailDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.DentalLineDetailDataGrid.ColumnHeaderLabel = Nothing
        Me.DentalLineDetailDataGrid.ColumnRePositioning = False
        Me.DentalLineDetailDataGrid.ColumnResizing = False
        Me.DentalLineDetailDataGrid.ConfirmDelete = True
        Me.DentalLineDetailDataGrid.CopySelectedOnly = False
        Me.DentalLineDetailDataGrid.CurrentBSPosition = -1
        Me.DentalLineDetailDataGrid.DataMember = ""
        Me.DentalLineDetailDataGrid.DragColumn = 0
        Me.DentalLineDetailDataGrid.ExportSelectedOnly = False
        Me.DentalLineDetailDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DentalLineDetailDataGrid.HighlightedRow = Nothing
        Me.DentalLineDetailDataGrid.HighLightModifiedRows = False
        Me.DentalLineDetailDataGrid.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.DentalLineDetailDataGrid.IsMouseDown = False
        Me.DentalLineDetailDataGrid.LastGoToLine = ""
        Me.DentalLineDetailDataGrid.Location = New System.Drawing.Point(0, 34)
        Me.DentalLineDetailDataGrid.MultiSort = False
        Me.DentalLineDetailDataGrid.Name = "DentalLineDetailDataGrid"
        Me.DentalLineDetailDataGrid.OldSelectedRow = Nothing
        Me.DentalLineDetailDataGrid.PreviousBSPosition = -1
        Me.DentalLineDetailDataGrid.ReadOnly = True
        Me.DentalLineDetailDataGrid.RetainRowSelectionAfterSort = True
        Me.DentalLineDetailDataGrid.SetRowOnRightClick = True
        Me.DentalLineDetailDataGrid.ShiftPressed = False
        Me.DentalLineDetailDataGrid.SingleClickBooleanColumns = True
        Me.DentalLineDetailDataGrid.Size = New System.Drawing.Size(1031, 286)
        Me.DentalLineDetailDataGrid.Sort = Nothing
        Me.DentalLineDetailDataGrid.StyleName = ""
        Me.DentalLineDetailDataGrid.SubKey = ""
        Me.DentalLineDetailDataGrid.SuppressMouseDown = False
        Me.DentalLineDetailDataGrid.SuppressTriangle = False
        Me.DentalLineDetailDataGrid.TabIndex = 0
        '
        'DentalAccumulatorValues
        '
        Me.DentalAccumulatorValues.AutoSize = True
        Me.DentalAccumulatorValues.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DentalAccumulatorValues.Location = New System.Drawing.Point(0, 0)
        Me.DentalAccumulatorValues.MinimumSize = New System.Drawing.Size(580, 479)
        Me.DentalAccumulatorValues.Name = "DentalAccumulatorValues"
        Me.DentalAccumulatorValues.Size = New System.Drawing.Size(1034, 479)
        Me.DentalAccumulatorValues.TabIndex = 0
        '
        'DentalLineDetail
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(1034, 781)
        Me.Controls.Add(Me.DetailSplitContainer)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(572, 800)
        Me.Name = "DentalLineDetail"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "Dental LineDetail"
        Me.TopMost = True
        Me.DetailSplitContainer.Panel1.ResumeLayout(False)
        Me.DetailSplitContainer.Panel2.ResumeLayout(False)
        Me.DetailSplitContainer.Panel2.PerformLayout()
        CType(Me.DetailSplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DetailSplitContainer.ResumeLayout(False)
        Me.TotalGroupBox.ResumeLayout(False)
        Me.TotalGroupBox.PerformLayout()
        CType(Me.DentalLineDetailDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Private _ClaimDetailBS As BindingSource
    Private _ClaimDetailDT As DataTable
    Private _LineDetailDT As DataTable
    Private _AccumulatorDetailBS As BindingSource
    Private _AccumulatorDetailDT As DataTable
    Friend WithEvents DentalLineDetailDataGrid As DataGridCustom
    Friend WithEvents DetailSplitContainer As SplitContainer
    Friend WithEvents TotalGroupBox As GroupBox
    Friend WithEvents TotalPaidTextBox As TextBox
    Friend WithEvents Label69 As Label
    Friend WithEvents TotalOtherTextBox As TextBox
    Friend WithEvents Label68 As Label
    Friend WithEvents TotalChargesTextBox As TextBox
    Friend WithEvents Label67 As Label
    Friend WithEvents DentalAccumulatorValues As DentalAccumulatorValues
End Class
