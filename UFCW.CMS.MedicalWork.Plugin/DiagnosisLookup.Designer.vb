<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DiagnosisLookup
    Inherits System.Windows.Forms.Form

Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

Private components As System.ComponentModel.IContainer

<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.DiagnosisLookupDataGrid = New DataGridPlus.DataGridCustom
        Me.MultiTimer = New System.Timers.Timer
        Me.DiagnosisValuesDataSet = New UFCW.CMS.UI.DiagnosisValuesDataSet
        Me.CancelFormButton = New System.Windows.Forms.Button
        CType(Me.DiagnosisLookupDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MultiTimer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DiagnosisValuesDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DiagnosisDataGrid
        '
        Me.DiagnosisLookupDataGrid.AllowColumnReorder = False
        Me.DiagnosisLookupDataGrid.AllowCopy = True
        Me.DiagnosisLookupDataGrid.AllowDelete = False
        Me.DiagnosisLookupDataGrid.AllowDragDrop = False
        Me.DiagnosisLookupDataGrid.AllowEdit = False
        Me.DiagnosisLookupDataGrid.AllowExport = False
        Me.DiagnosisLookupDataGrid.AllowFind = True
        Me.DiagnosisLookupDataGrid.AllowGoTo = True
        Me.DiagnosisLookupDataGrid.AllowMultiSelect = True
        Me.DiagnosisLookupDataGrid.AllowMultiSort = False
        Me.DiagnosisLookupDataGrid.AllowNew = False
        Me.DiagnosisLookupDataGrid.AllowPrint = False
        Me.DiagnosisLookupDataGrid.AllowRefresh = False
        Me.DiagnosisLookupDataGrid.AllowSorting = False
        Me.DiagnosisLookupDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DiagnosisLookupDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.DiagnosisLookupDataGrid.CaptionVisible = False
        Me.DiagnosisLookupDataGrid.ConfirmDelete = True
        Me.DiagnosisLookupDataGrid.CopySelectedOnly = True
        Me.DiagnosisLookupDataGrid.DataMember = ""
        Me.DiagnosisLookupDataGrid.ExportSelectedOnly = True
        Me.DiagnosisLookupDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DiagnosisLookupDataGrid.LastGoToLine = ""
        Me.DiagnosisLookupDataGrid.Location = New System.Drawing.Point(8, 8)
        Me.DiagnosisLookupDataGrid.MultiSort = False
        Me.DiagnosisLookupDataGrid.Name = "DiagnosisLookupDataGrid"
        Me.DiagnosisLookupDataGrid.SetRowOnRightClick = True
        Me.DiagnosisLookupDataGrid.SingleClickBooleanColumns = True
        Me.DiagnosisLookupDataGrid.Size = New System.Drawing.Size(536, 319)
        Me.DiagnosisLookupDataGrid.SuppressTriangle = False
        Me.DiagnosisLookupDataGrid.TabIndex = 0
        '
        'MultiTimer
        '
        Me.MultiTimer.SynchronizingObject = Me
        '
        'DiagnosisValuesDataSet
        '
        Me.DiagnosisValuesDataSet.DataSetName = "DiagnosisValuesDataSet"
        Me.DiagnosisValuesDataSet.Locale = New System.Globalization.CultureInfo("en-US")
        Me.DiagnosisValuesDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'CancelFormButton
        '
        Me.CancelFormButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CancelFormButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelFormButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CancelFormButton.Location = New System.Drawing.Point(470, 333)
        Me.CancelFormButton.Name = "CancelFormButton"
        Me.CancelFormButton.Size = New System.Drawing.Size(74, 23)
        Me.CancelFormButton.TabIndex = 23
        Me.CancelFormButton.Text = "Cancel"
        '
        'DiagnosisLookup
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.CancelFormButton
        Me.ClientSize = New System.Drawing.Size(552, 358)
        Me.Controls.Add(Me.CancelFormButton)
        Me.Controls.Add(Me.DiagnosisLookupDataGrid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(392, 176)
        Me.Name = "DiagnosisLookup"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "Select Diagnosis(Diagnoses)..."
        CType(Me.DiagnosisLookupDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MultiTimer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DiagnosisValuesDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
Friend WithEvents CancelFormButton As System.Windows.Forms.Button
Friend WithEvents DiagnosisLookupDataGrid As DataGridPlus.DataGridCustom
Friend WithEvents MultiTimer As System.Timers.Timer
Friend WithEvents DiagnosisValuesDataSet As DiagnosisValuesDataSet

End Class
