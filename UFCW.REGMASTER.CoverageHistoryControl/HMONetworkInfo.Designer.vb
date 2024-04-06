<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class HMONetworkInfoForm
    Inherits System.Windows.Forms.Form

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.HMONetworkDataGrid = New DataGridCustom()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.AddButton = New System.Windows.Forms.Button()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.CancelChangeButton = New System.Windows.Forms.Button()
        Me.ExitButton = New System.Windows.Forms.Button()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.FromLabel = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtThruDate = New System.Windows.Forms.TextBox()
        Me.txtFromDate = New System.Windows.Forms.TextBox()
        Me.Label55 = New System.Windows.Forms.Label()
        Me.cmbHMONetwork = New System.Windows.Forms.ComboBox()
        Me.controlGroupBox = New System.Windows.Forms.GroupBox()
        Me.HistoryButton = New System.Windows.Forms.Button()
        Me._HMONetworkBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.ModifyButton = New System.Windows.Forms.Button()
        CType(Me.HMONetworkDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.controlGroupBox.SuspendLayout()
        CType(Me._HMONetworkBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'HMONetworkDataGrid
        '
        Me.HMONetworkDataGrid.ADGroupsThatCanCopy = ""
        Me.HMONetworkDataGrid.ADGroupsThatCanCustomize = ""
        Me.HMONetworkDataGrid.ADGroupsThatCanExport = ""
        Me.HMONetworkDataGrid.ADGroupsThatCanFilter = ""
        Me.HMONetworkDataGrid.ADGroupsThatCanFind = ""
        Me.HMONetworkDataGrid.ADGroupsThatCanMultiSort = ""
        Me.HMONetworkDataGrid.ADGroupsThatCanPrint = ""
        Me.HMONetworkDataGrid.AllowAutoSize = True
        Me.HMONetworkDataGrid.AllowColumnReorder = True
        Me.HMONetworkDataGrid.AllowCopy = True
        Me.HMONetworkDataGrid.AllowCustomize = True
        Me.HMONetworkDataGrid.AllowDelete = False
        Me.HMONetworkDataGrid.AllowDragDrop = False
        Me.HMONetworkDataGrid.AllowEdit = False
        Me.HMONetworkDataGrid.AllowExport = False
        Me.HMONetworkDataGrid.AllowFilter = True
        Me.HMONetworkDataGrid.AllowFind = True
        Me.HMONetworkDataGrid.AllowGoTo = True
        Me.HMONetworkDataGrid.AllowMultiSelect = False
        Me.HMONetworkDataGrid.AllowMultiSort = False
        Me.HMONetworkDataGrid.AllowNew = False
        Me.HMONetworkDataGrid.AllowPrint = True
        Me.HMONetworkDataGrid.AllowRefresh = True
        Me.HMONetworkDataGrid.AppKey = "UFCW\RegMaster\"
        Me.HMONetworkDataGrid.AutoSaveCols = True
        Me.HMONetworkDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.HMONetworkDataGrid.CausesValidation = False
        Me.HMONetworkDataGrid.ColumnHeaderLabel = Nothing
        Me.HMONetworkDataGrid.ColumnRePositioning = False
        Me.HMONetworkDataGrid.ColumnResizing = False
        Me.HMONetworkDataGrid.ConfirmDelete = True
        Me.HMONetworkDataGrid.CopySelectedOnly = True
        Me.HMONetworkDataGrid.DataMember = ""
        Me.HMONetworkDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HMONetworkDataGrid.DragColumn = 0
        Me.HMONetworkDataGrid.ExportSelectedOnly = True
        Me.HMONetworkDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.HMONetworkDataGrid.HighlightedRow = Nothing
        Me.HMONetworkDataGrid.HighLightModifiedRows = False
        Me.HMONetworkDataGrid.IsMouseDown = False
        Me.HMONetworkDataGrid.LastGoToLine = ""
        Me.HMONetworkDataGrid.Location = New System.Drawing.Point(3, 16)
        Me.HMONetworkDataGrid.MultiSort = False
        Me.HMONetworkDataGrid.Name = "HMONetworkDataGrid"
        Me.HMONetworkDataGrid.OldSelectedRow = Nothing
        Me.HMONetworkDataGrid.ReadOnly = True
        Me.HMONetworkDataGrid.SetRowOnRightClick = True
        Me.HMONetworkDataGrid.ShiftPressed = False
        Me.HMONetworkDataGrid.SingleClickBooleanColumns = True
        Me.HMONetworkDataGrid.Size = New System.Drawing.Size(471, 289)
        Me.HMONetworkDataGrid.Sort = Nothing
        Me.HMONetworkDataGrid.StyleName = ""
        Me.HMONetworkDataGrid.SubKey = ""
        Me.HMONetworkDataGrid.SuppressTriangle = False
        Me.HMONetworkDataGrid.TabIndex = 103
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.HMONetworkDataGrid)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 147)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(477, 308)
        Me.GroupBox2.TabIndex = 96
        Me.GroupBox2.TabStop = False
        '
        'AddButton
        '
        Me.AddButton.CausesValidation = False
        Me.AddButton.Location = New System.Drawing.Point(15, 118)
        Me.AddButton.Name = "AddButton"
        Me.AddButton.Size = New System.Drawing.Size(60, 23)
        Me.AddButton.TabIndex = 97
        Me.AddButton.Text = "Add"
        Me.AddButton.UseVisualStyleBackColor = True
        '
        'SaveButton
        '
        Me.SaveButton.CausesValidation = False
        Me.SaveButton.Location = New System.Drawing.Point(201, 118)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(60, 23)
        Me.SaveButton.TabIndex = 100
        Me.SaveButton.Text = "Save"
        Me.SaveButton.UseVisualStyleBackColor = True
        '
        'CancelChangeButton
        '
        Me.CancelChangeButton.CausesValidation = False
        Me.CancelChangeButton.Location = New System.Drawing.Point(267, 118)
        Me.CancelChangeButton.Name = "CancelChangeButton"
        Me.CancelChangeButton.Size = New System.Drawing.Size(60, 23)
        Me.CancelChangeButton.TabIndex = 99
        Me.CancelChangeButton.Text = "Cancel"
        Me.CancelChangeButton.UseVisualStyleBackColor = True
        '
        'ExitButton
        '
        Me.ExitButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExitButton.CausesValidation = False
        Me.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ExitButton.Location = New System.Drawing.Point(414, 118)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(60, 23)
        Me.ExitButton.TabIndex = 101
        Me.ExitButton.Text = "Exit"
        Me.ExitButton.UseVisualStyleBackColor = True
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'FromLabel
        '
        Me.FromLabel.AutoSize = True
        Me.FromLabel.Location = New System.Drawing.Point(53, 22)
        Me.FromLabel.Name = "FromLabel"
        Me.FromLabel.Size = New System.Drawing.Size(56, 13)
        Me.FromLabel.TabIndex = 93
        Me.FromLabel.Text = "From Date"
        Me.FromLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(54, 44)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(55, 13)
        Me.Label3.TabIndex = 94
        Me.Label3.Text = "Thru Date"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtThruDt
        '
        Me.txtThruDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThruDate.Location = New System.Drawing.Point(118, 42)
        Me.txtThruDate.MaxLength = 10
        Me.txtThruDate.Name = "txtThruDate"
        Me.txtThruDate.Size = New System.Drawing.Size(91, 20)
        Me.txtThruDate.TabIndex = 92
        '
        'txtFromDt
        '
        Me.txtFromDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFromDate.Location = New System.Drawing.Point(118, 19)
        Me.txtFromDate.MaxLength = 10
        Me.txtFromDate.Name = "txtFromDate"
        Me.txtFromDate.Size = New System.Drawing.Size(91, 20)
        Me.txtFromDate.TabIndex = 91
        '
        'Label55
        '
        Me.Label55.AutoSize = True
        Me.Label55.Location = New System.Drawing.Point(62, 67)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(47, 13)
        Me.Label55.TabIndex = 96
        Me.Label55.Text = "Network"
        '
        'cmbHMONetwork
        '
        Me.cmbHMONetwork.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbHMONetwork.DropDownWidth = 200
        Me.cmbHMONetwork.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbHMONetwork.Location = New System.Drawing.Point(118, 65)
        Me.cmbHMONetwork.MaxLength = 2
        Me.cmbHMONetwork.Name = "cmbHMONetwork"
        Me.cmbHMONetwork.Size = New System.Drawing.Size(195, 21)
        Me.cmbHMONetwork.TabIndex = 95
        '
        'controlGroupBox
        '
        Me.controlGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.controlGroupBox.Controls.Add(Me.HistoryButton)
        Me.controlGroupBox.Controls.Add(Me.cmbHMONetwork)
        Me.controlGroupBox.Controls.Add(Me.Label55)
        Me.controlGroupBox.Controls.Add(Me.txtFromDate)
        Me.controlGroupBox.Controls.Add(Me.txtThruDate)
        Me.controlGroupBox.Controls.Add(Me.Label3)
        Me.controlGroupBox.Controls.Add(Me.FromLabel)
        Me.controlGroupBox.Location = New System.Drawing.Point(13, 13)
        Me.controlGroupBox.Name = "controlGroupBox"
        Me.controlGroupBox.Size = New System.Drawing.Size(467, 99)
        Me.controlGroupBox.TabIndex = 95
        Me.controlGroupBox.TabStop = False
        '
        'HistoryButton
        '
        Me.HistoryButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HistoryButton.CausesValidation = False
        Me.HistoryButton.Location = New System.Drawing.Point(401, 16)
        Me.HistoryButton.Name = "HistoryButton"
        Me.HistoryButton.Size = New System.Drawing.Size(60, 23)
        Me.HistoryButton.TabIndex = 102
        Me.HistoryButton.Text = "History"
        Me.HistoryButton.UseVisualStyleBackColor = True
        '
        '_HMONetworkBindingSource
        '
        Me._HMONetworkBindingSource.AllowNew = False
        '
        'ModifyButton
        '
        Me.ModifyButton.CausesValidation = False
        Me.ModifyButton.Location = New System.Drawing.Point(81, 118)
        Me.ModifyButton.Name = "ModifyButton"
        Me.ModifyButton.Size = New System.Drawing.Size(60, 23)
        Me.ModifyButton.TabIndex = 102
        Me.ModifyButton.Text = "Modify"
        Me.ModifyButton.UseVisualStyleBackColor = True
        '
        'HMONetworkInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange
        Me.CancelButton = Me.ExitButton
        Me.CausesValidation = False
        Me.ClientSize = New System.Drawing.Size(492, 466)
        Me.Controls.Add(Me.ModifyButton)
        Me.Controls.Add(Me.ExitButton)
        Me.Controls.Add(Me.AddButton)
        Me.Controls.Add(Me.SaveButton)
        Me.Controls.Add(Me.CancelChangeButton)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.controlGroupBox)
        Me.Name = "HMONetworkInfo"
        Me.Text = "HMO Network Information"
        CType(Me.HMONetworkDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.controlGroupBox.ResumeLayout(False)
        Me.controlGroupBox.PerformLayout()
        CType(Me._HMONetworkBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents HMONetworkDataGrid As DataGridCustom
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents AddButton As System.Windows.Forms.Button
    Friend WithEvents SaveButton As System.Windows.Forms.Button
    Friend Shadows WithEvents CancelChangeButton As System.Windows.Forms.Button
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents _HMONetworkBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents controlGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents cmbHMONetwork As System.Windows.Forms.ComboBox
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents txtFromDate As System.Windows.Forms.TextBox
    Friend WithEvents txtThruDate As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents FromLabel As System.Windows.Forms.Label
    Friend WithEvents ExitButton As Windows.Forms.Button
    Friend WithEvents ModifyButton As Windows.Forms.Button
    Friend WithEvents HistoryButton As Windows.Forms.Button
End Class
