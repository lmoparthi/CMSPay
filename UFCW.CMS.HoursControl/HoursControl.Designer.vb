<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HoursControl
    Inherits System.Windows.Forms.UserControl

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.HoursDataGrid = New DataGridCustom()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.AndLabel3 = New System.Windows.Forms.Label()
        Me.BetweenLabel3 = New System.Windows.Forms.Label()
        Me.PPToDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.PPFromDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.ppCheckBox = New System.Windows.Forms.CheckBox()
        Me.EPToDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.epFromDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.EPCheckBox = New System.Windows.Forms.CheckBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        CType(Me.HoursDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'HoursDataGrid
        '
        Me.HoursDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.HoursDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.HoursDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators,REGMReadOnlyAccess"
        Me.HoursDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators,REGMReadOnlyAccess"
        Me.HoursDataGrid.ADGroupsThatCanFind = ""
        Me.HoursDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators,REGMReadOnlyAccess"
        Me.HoursDataGrid.ADGroupsThatCanMultiSort = ""
        Me.HoursDataGrid.AllowAutoSize = True
        Me.HoursDataGrid.AllowColumnReorder = True
        Me.HoursDataGrid.AllowCopy = True
        Me.HoursDataGrid.AllowCustomize = True
        Me.HoursDataGrid.AllowDelete = False
        Me.HoursDataGrid.AllowDragDrop = False
        Me.HoursDataGrid.AllowEdit = False
        Me.HoursDataGrid.AllowExport = True
        Me.HoursDataGrid.AllowFilter = True
        Me.HoursDataGrid.AllowFind = True
        Me.HoursDataGrid.AllowGoTo = True
        Me.HoursDataGrid.AllowMultiSelect = True
        Me.HoursDataGrid.AllowMultiSort = True
        Me.HoursDataGrid.AllowNew = False
        Me.HoursDataGrid.AllowPrint = False
        Me.HoursDataGrid.AllowRefresh = False
        Me.HoursDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HoursDataGrid.AppKey = "UFCW\Claims\"
        Me.HoursDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.HoursDataGrid.CaptionText = "Hours"
        Me.HoursDataGrid.ColumnHeaderLabel = Nothing
        Me.HoursDataGrid.ColumnRePositioning = False
        Me.HoursDataGrid.ColumnResizing = False
        Me.HoursDataGrid.ConfirmDelete = True
        Me.HoursDataGrid.CopySelectedOnly = True
        Me.HoursDataGrid.DataMember = ""
        Me.HoursDataGrid.DragColumn = 0
        Me.HoursDataGrid.ExportSelectedOnly = True
        Me.HoursDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.HoursDataGrid.HighlightedRow = Nothing
        Me.HoursDataGrid.IsMouseDown = False
        Me.HoursDataGrid.LastGoToLine = ""
        Me.HoursDataGrid.Location = New System.Drawing.Point(3, 58)
        Me.HoursDataGrid.MultiSort = False
        Me.HoursDataGrid.Name = "HoursDataGrid"
        Me.HoursDataGrid.OldSelectedRow = 0
        Me.HoursDataGrid.ReadOnly = True
        Me.HoursDataGrid.SetRowOnRightClick = True
        Me.HoursDataGrid.ShiftPressed = False
        Me.HoursDataGrid.SingleClickBooleanColumns = True
        Me.HoursDataGrid.Size = New System.Drawing.Size(550, 339)
        Me.HoursDataGrid.StyleName = ""
        Me.HoursDataGrid.SubKey = ""
        Me.HoursDataGrid.SuppressTriangle = False
        Me.HoursDataGrid.TabIndex = 7
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.AndLabel3)
        Me.GroupBox1.Controls.Add(Me.BetweenLabel3)
        Me.GroupBox1.Controls.Add(Me.PPToDateTimePicker)
        Me.GroupBox1.Controls.Add(Me.PPFromDateTimePicker)
        Me.GroupBox1.Controls.Add(Me.ppCheckBox)
        Me.GroupBox1.Controls.Add(Me.EPToDateTimePicker)
        Me.GroupBox1.Controls.Add(Me.epFromDateTimePicker)
        Me.GroupBox1.Controls.Add(Me.EPCheckBox)
        Me.GroupBox1.Controls.Add(Me.btnSearch)
        Me.GroupBox1.Location = New System.Drawing.Point(3, -5)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(550, 57)
        Me.GroupBox1.TabIndex = 10
        Me.GroupBox1.TabStop = False
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(254, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(32, 16)
        Me.Label1.TabIndex = 50
        Me.Label1.Text = "And"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(103, 33)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(49, 16)
        Me.Label2.TabIndex = 49
        Me.Label2.Text = "Between"
        '
        'AndLabel3
        '
        Me.AndLabel3.Location = New System.Drawing.Point(254, 14)
        Me.AndLabel3.Name = "AndLabel3"
        Me.AndLabel3.Size = New System.Drawing.Size(32, 16)
        Me.AndLabel3.TabIndex = 48
        Me.AndLabel3.Text = "And"
        '
        'BetweenLabel3
        '
        Me.BetweenLabel3.Location = New System.Drawing.Point(103, 13)
        Me.BetweenLabel3.Name = "BetweenLabel3"
        Me.BetweenLabel3.Size = New System.Drawing.Size(49, 16)
        Me.BetweenLabel3.TabIndex = 47
        Me.BetweenLabel3.Text = "Between"
        '
        'PPToDateTimePicker
        '
        Me.PPToDateTimePicker.Enabled = False
        Me.PPToDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.PPToDateTimePicker.Location = New System.Drawing.Point(287, 10)
        Me.PPToDateTimePicker.Name = "PPToDateTimePicker"
        Me.PPToDateTimePicker.Size = New System.Drawing.Size(95, 20)
        Me.PPToDateTimePicker.TabIndex = 46
        '
        'PPFromDateTimePicker
        '
        Me.PPFromDateTimePicker.Enabled = False
        Me.PPFromDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.PPFromDateTimePicker.Location = New System.Drawing.Point(158, 10)
        Me.PPFromDateTimePicker.Name = "PPFromDateTimePicker"
        Me.PPFromDateTimePicker.Size = New System.Drawing.Size(95, 20)
        Me.PPFromDateTimePicker.TabIndex = 45
        '
        'ppCheckBox
        '
        Me.ppCheckBox.Location = New System.Drawing.Point(5, 10)
        Me.ppCheckBox.Name = "ppCheckBox"
        Me.ppCheckBox.Size = New System.Drawing.Size(104, 16)
        Me.ppCheckBox.TabIndex = 44
        Me.ppCheckBox.Text = "Hours Worked"
        '
        'EPToDateTimePicker
        '
        Me.EPToDateTimePicker.Enabled = False
        Me.EPToDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.EPToDateTimePicker.Location = New System.Drawing.Point(287, 32)
        Me.EPToDateTimePicker.Name = "EPToDateTimePicker"
        Me.EPToDateTimePicker.Size = New System.Drawing.Size(95, 20)
        Me.EPToDateTimePicker.TabIndex = 43
        '
        'epFromDateTimePicker
        '
        Me.epFromDateTimePicker.Enabled = False
        Me.epFromDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.epFromDateTimePicker.Location = New System.Drawing.Point(158, 32)
        Me.epFromDateTimePicker.Name = "epFromDateTimePicker"
        Me.epFromDateTimePicker.Size = New System.Drawing.Size(95, 20)
        Me.epFromDateTimePicker.TabIndex = 42
        '
        'EPCheckBox
        '
        Me.EPCheckBox.Location = New System.Drawing.Point(5, 32)
        Me.EPCheckBox.Name = "EPCheckBox"
        Me.EPCheckBox.Size = New System.Drawing.Size(104, 18)
        Me.EPCheckBox.TabIndex = 41
        Me.EPCheckBox.Text = "Eligibility Period"
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(405, 19)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(75, 21)
        Me.btnSearch.TabIndex = 4
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'HoursControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.HoursDataGrid)
        Me.Name = "HoursControl"
        Me.Size = New System.Drawing.Size(553, 400)
        CType(Me.HoursDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents HoursDataGrid As DataGridCustom
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents AndLabel3 As System.Windows.Forms.Label
    Friend WithEvents BetweenLabel3 As System.Windows.Forms.Label
    Friend WithEvents PPToDateTimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents PPFromDateTimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents ppCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents EPToDateTimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents epFromDateTimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents EPCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button

End Class
