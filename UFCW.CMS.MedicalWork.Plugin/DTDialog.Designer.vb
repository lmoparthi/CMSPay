<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DTDialog
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
        Me.components = New System.ComponentModel.Container
        Me.DG = New DataGridPlus.DataGridCustom
        Me.CancelFormButton = New System.Windows.Forms.Button
        CType(Me.DG, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DG
        '
        Me.DG.ADGroupsThatCanCopy = "CMSUsers"
        Me.DG.ADGroupsThatCanCustomize = "CMSUsers"
        Me.DG.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DG.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DG.ADGroupsThatCanFind = ""
        Me.DG.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DG.ADGroupsThatCanSort = ""
        Me.DG.AllowAutoSize = True
        Me.DG.AllowColumnReorder = True
        Me.DG.AllowCopy = False
        Me.DG.AllowCustomize = True
        Me.DG.AllowDelete = False
        Me.DG.AllowDragDrop = False
        Me.DG.AllowEdit = False
        Me.DG.AllowExport = False
        Me.DG.AllowFilter = True
        Me.DG.AllowFind = True
        Me.DG.AllowGoTo = True
        Me.DG.AllowMultiSelect = False
        Me.DG.AllowMultiSort = True
        Me.DG.AllowNew = False
        Me.DG.AllowPrint = False
        Me.DG.AllowRefresh = False
        Me.DG.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DG.AppKey = "UFCW\Claims\"
        Me.DG.BackgroundColor = System.Drawing.SystemColors.Window
        Me.DG.CaptionVisible = False
        Me.DG.ConfirmDelete = True
        Me.DG.CopySelectedOnly = True
        Me.DG.DataMember = ""
        Me.DG.ExportSelectedOnly = True
        Me.DG.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DG.LastGoToLine = ""
        Me.DG.Location = New System.Drawing.Point(4, 4)
        Me.DG.MultiSort = False
        Me.DG.Name = "DG"
        Me.DG.SetRowOnRightClick = True
        Me.DG.SingleClickBooleanColumns = True
        Me.DG.Size = New System.Drawing.Size(544, 306)
        Me.DG.SuppressTriangle = False
        Me.DG.TabIndex = 0
        '
        'CancelFormButton
        '
        Me.CancelFormButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CancelFormButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelFormButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CancelFormButton.Location = New System.Drawing.Point(474, 316)
        Me.CancelFormButton.Name = "CancelFormButton"
        Me.CancelFormButton.Size = New System.Drawing.Size(74, 23)
        Me.CancelFormButton.TabIndex = 23
        Me.CancelFormButton.Text = "Cancel"
        '
        'DTDialog
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.CancelFormButton
        Me.ClientSize = New System.Drawing.Size(552, 346)
        Me.Controls.Add(Me.CancelFormButton)
        Me.Controls.Add(Me.DG)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Name = "DTDialog"
        Me.Text = "DTDialog"
        CType(Me.DG, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
Friend WithEvents CancelFormButton As System.Windows.Forms.Button
Friend WithEvents DG As DataGridPlus.DataGridCustom

End Class
