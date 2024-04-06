
Public Class FilterDialog
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents ApplyFilter As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SelectAllToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FilterByColumn As System.Windows.Forms.ComboBox
    Friend WithEvents FilterByColumnValue As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ResetFilter As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents CancelForm As System.Windows.Forms.Button
    '<System.Diagnostics.DebuggerStepThrough()> 
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.CancelForm = New System.Windows.Forms.Button
        Me.ApplyFilter = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SelectAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.FilterByColumn = New System.Windows.Forms.ComboBox
        Me.FilterByColumnValue = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.ResetFilter = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'CancelForm
        '
        Me.CancelForm.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CancelForm.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelForm.Location = New System.Drawing.Point(169, 76)
        Me.CancelForm.Name = "CancelForm"
        Me.CancelForm.Size = New System.Drawing.Size(75, 23)
        Me.CancelForm.TabIndex = 0
        Me.CancelForm.Text = "&Cancel"
        '
        'ApplyFilter
        '
        Me.ApplyFilter.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ApplyFilter.Location = New System.Drawing.Point(88, 76)
        Me.ApplyFilter.Name = "ApplyFilter"
        Me.ApplyFilter.Size = New System.Drawing.Size(75, 23)
        Me.ApplyFilter.TabIndex = 2
        Me.ApplyFilter.Text = "&Add Filter"
        Me.ToolTip1.SetToolTip(Me.ApplyFilter, "Uses the column / filter values selected to limit what is displayed")
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SelectAllToolStripMenuItem, Me.SaveToolStripMenuItem, Me.CToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(168, 70)
        '
        'SelectAllToolStripMenuItem
        '
        Me.SelectAllToolStripMenuItem.Name = "SelectAllToolStripMenuItem"
        Me.SelectAllToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.SelectAllToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
        Me.SelectAllToolStripMenuItem.Text = "Select &All"
        Me.SelectAllToolStripMenuItem.Visible = False
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
        Me.SaveToolStripMenuItem.Text = "&Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'CToolStripMenuItem
        '
        Me.CToolStripMenuItem.Name = "CToolStripMenuItem"
        Me.CToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.CToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
        Me.CToolStripMenuItem.Text = "&Cancel"
        Me.CToolStripMenuItem.Visible = False
        '
        'FilterByColumn
        '
        Me.FilterByColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.FilterByColumn.Location = New System.Drawing.Point(66, 12)
        Me.FilterByColumn.Name = "FilterByColumn"
        Me.FilterByColumn.Size = New System.Drawing.Size(176, 21)
        Me.FilterByColumn.TabIndex = 3
        '
        'FilterByColumnValue
        '
        Me.FilterByColumnValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.FilterByColumnValue.Enabled = False
        Me.FilterByColumnValue.Location = New System.Drawing.Point(66, 39)
        Me.FilterByColumnValue.Name = "FilterByColumnValue"
        Me.FilterByColumnValue.Size = New System.Drawing.Size(176, 21)
        Me.FilterByColumnValue.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(42, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Column"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 42)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Filter Value"
        '
        'ResetFilter
        '
        Me.ResetFilter.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ResetFilter.Location = New System.Drawing.Point(7, 76)
        Me.ResetFilter.Name = "ResetFilter"
        Me.ResetFilter.Size = New System.Drawing.Size(75, 23)
        Me.ResetFilter.TabIndex = 7
        Me.ResetFilter.Text = "&Reset"
        Me.ToolTip1.SetToolTip(Me.ResetFilter, "Removes all filters")
        '
        'FilterDialog
        '
        Me.AcceptButton = Me.ApplyFilter
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.CancelForm
        Me.ClientSize = New System.Drawing.Size(248, 108)
        Me.Controls.Add(Me.ResetFilter)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.FilterByColumnValue)
        Me.Controls.Add(Me.FilterByColumn)
        Me.Controls.Add(Me.CancelForm)
        Me.Controls.Add(Me.ApplyFilter)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MinimumSize = New System.Drawing.Size(254, 0)
        Me.Name = "FilterDialog"
        Me.Text = "Select Filter Criteria"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private _APPKEY As String = "UFCW\Claims\"
    Private _DG As DataGridCustom
    Private _FilterMappingName As String
    Private _FilterColumnHeading As String
    Private _FilterValue As String
    Private _FilterFormattedValue As String

    Private FilterColumns As DataTable

    Private WithEvents FilterColumnsBS As BindingSource

#Region " Public Properties"

    <System.ComponentModel.Browsable(True), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")> _
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal Value As String)
            _APPKEY = Value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Gets the column selected to filter against.")> _
    Public ReadOnly Property FilterMappingName() As String
        Get
            Return _FilterMappingName
        End Get
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Gets the column heading selected to filter against.")> _
    Public ReadOnly Property FilterColumnHeading() As String
        Get
            Return _FilterColumnHeading
        End Get
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Gets the column value selected to filter against.")> _
    Public ReadOnly Property FilterValue() As String
        Get
            Return _FilterValue
        End Get
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Gets the column value selected to filter against with any formatting applied.")> _
    Public ReadOnly Property FilterFormattedValue() As String
        Get
            Return _FilterFormattedValue
        End Get
    End Property

#End Region

    Private _Disposed As Boolean = False
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)

        If _Disposed Then Return

        If disposing Then

            If FilterColumnsBS IsNot Nothing Then FilterColumnsBS.Dispose()
            FilterColumnsBS = Nothing

            If FilterColumns IsNot Nothing Then FilterColumns.Dispose()
            FilterColumns = Nothing

            If (components IsNot Nothing) Then
                components.Dispose()
            End If

        End If

        _Disposed = True

        MyBase.Dispose(disposing)
    End Sub

    Sub New(ByVal DG As DataGridCustom, ByVal appKEY As String)
        Me.New()
        _APPKEY = appKEY
        _DG = DG
    End Sub


    Private Sub SetSettings()

        Me.Top = CInt(If(CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString))))
        Me.Height = CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = CInt(If(CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString))))
        Me.Width = CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(Me.AppKey, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)

    End Sub

    Private Sub SaveSettings()

        Dim lWindowState As FormWindowState = Me.WindowState
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "WindowState", CInt(lWindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = lWindowState

    End Sub

    Private Sub FilterDialog_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        FilterColumns = New DataTable("FilterColumns")

        FilterColumns.Columns.Add("Mapping")
        FilterColumns.Columns.Add("DisplayText")
        FilterColumns.Columns.Add("Format")

        LoadColumns()

        SetSettings()

    End Sub

    Private Sub CancelForm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelForm.Click

        _FilterMappingName = Nothing
        _FilterValue = Nothing

        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel

        Me.Close()
    End Sub

    Private Sub SaveForm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ApplyFilter.Click

        If FilterByColumnValue.SelectedIndex = -1 Then
            MsgBox("You must select a filter value before you continue.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Incomplete request")
            Exit Sub
        End If

        _FilterValue = CStr(CType(FilterByColumnValue.SelectedItem, DataRowView)("Data"))
        _FilterFormattedValue = CStr(CType(FilterByColumnValue.SelectedItem, DataRowView)("FormattedData"))

        Me.DialogResult = System.Windows.Forms.DialogResult.OK

        Me.Close()

    End Sub
    Private Sub ResetFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResetFilter.Click

        _FilterMappingName = Nothing
        _FilterValue = Nothing

        Me.DialogResult = System.Windows.Forms.DialogResult.OK

        Me.Close()
    End Sub

    Private Sub FilterDialog_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        SaveSettings()
    End Sub

    Private Sub LoadColumns()
        Dim TextSize As Decimal = 0
        Dim ColStyle As DataGridColumnStyle
        Dim DR As DataRow

        Try

            For Each ColStyle In _DG.TableStyles("Default").GridColumnStyles
                If ColStyle.HeaderText.Trim.Length > 0 AndAlso _DG.GetCurrentDataTable.Columns.Contains(ColStyle.MappingName) Then
                    DR = FilterColumns.NewRow
                    DR("Mapping") = ColStyle.MappingName
                    DR("DisplayText") = ColStyle.HeaderText
                    DR("Format") = DirectCast(ColStyle, DataGridFormattableTextBoxColumn).Format.ToString

                    FilterColumns.Rows.Add(DR)
                End If
            Next

            For Each FilterDR As DataRow In FilterColumns.Rows
                If UFCWGeneral.MeasureWidthinPixels(FilterDR("DisplayText").ToString.Trim.Length, Me.Font.Name, Me.Font.Size) > TextSize Then
                    TextSize = UFCWGeneral.MeasureWidthinPixels(FilterDR("DisplayText").ToString.Trim.Length, Me.Font.Name, Me.Font.Size)
                End If
            Next

            FilterByColumn.SuspendLayout()
            FilterColumns.DefaultView.Sort = "DisplayText"
            FilterColumnsBS = New BindingSource(FilterColumns, "")

            FilterByColumn.DataSource = FilterColumnsBS
            FilterByColumn.DisplayMember = "DisplayText"
            FilterByColumn.ValueMember = "Mapping"
            FilterByColumn.SelectedIndex = -1
            FilterByColumn.DropDownWidth = CInt(TextSize)

            FilterByColumn.ResumeLayout()
            Me.ToolTip1.SetToolTip(Me.ResetFilter, "Removes all current filters." & Environment.NewLine & _DG.GetCurrentDataView.RowFilter)

        Catch ex As Exception
            Throw
        Finally

        End Try
    End Sub

    Private Sub FilterByColumn_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles FilterByColumn.DropDownClosed
        Dim CB As ComboBox = CType(sender, ComboBox)

        If CB.SelectedIndex = 0 AndAlso _FilterColumnHeading Is Nothing Then
            FilterColumnsBS_PositionChanged(CB.DataSource, Nothing)
        End If
    End Sub

    Private Sub FilterColumnsBS_PositionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles FilterColumnsBS.PositionChanged

        Dim TextSize As Decimal = 0
        Dim CM As CurrencyManager
        Dim DV As DataRowView
        Dim DistinctDT As DataTable

        Const DROP_DOWN_MIN_WIDTH As Integer = 100

        Try

            CM = CType(sender, System.Windows.Forms.BindingSource).CurrencyManager
            DV = CType(CM.Current, DataRowView)

            _FilterMappingName = CStr(DV("Mapping"))
            _FilterColumnHeading = CStr(DV("DisplayText"))

            FilterByColumnValue.SuspendLayout()
            FilterByColumnValue.DataSource = Nothing

            DistinctDT = SelectDistinct(_DG.GetCurrentDataTable, CStr(DV("Mapping")), CStr(DV("Format")))
            For Each DR As DataRow In DistinctDT.Rows
                If UFCWGeneral.MeasureWidthinPixels(DR("FormattedData").ToString.Trim.Length, Me.Font.Name, Me.Font.Size) > TextSize Then
                    TextSize = UFCWGeneral.MeasureWidthinPixels(DR("FormattedData").ToString.Trim.Length, Me.Font.Name, Me.Font.Size)
                End If
            Next

            If CInt(TextSize) > 0 Then
            FilterByColumnValue.DropDownWidth = CInt(TextSize)
            Else
                FilterByColumnValue.DropDownWidth = DROP_DOWN_MIN_WIDTH
            End If

            FilterByColumnValue.DataSource = DistinctDT

            FilterByColumnValue.DisplayMember = "FormattedData"
            FilterByColumnValue.ValueMember = "Data"

            FilterByColumnValue.SelectedIndex = -1


        Catch ex As Exception
            Throw
        Finally

            FilterByColumnValue.ResumeLayout()
            FilterByColumnValue.Enabled = True

        End Try

    End Sub

    Public Shared Function SelectDistinct(ByVal sourceTable As DataTable, ByVal fieldName As String, ByVal fieldFormat As String) As DataTable
        Dim DT As DataTable
        Dim DR As DataRow
        Dim LastValue As Object

        Try

            DT = New DataTable(sourceTable.TableName)
            DT.Columns.Add("FormattedData")
            DT.Columns.Add("Data")

            For Each DR In sourceTable.Select("", fieldName)
                If LastValue Is Nothing OrElse Not ColumnEqual(LastValue, DR(fieldName)) Then
                    LastValue = DR(fieldName)
                    If Not LastValue Is System.DBNull.Value Then
                        DT.Rows.Add(String.Format("{0:" & fieldFormat & "}", LastValue), LastValue)
                    End If
                End If
            Next

            Return DT

        Catch ex As Exception

            Throw
        Finally
            DT = Nothing
        End Try
    End Function

    Public Shared Function ColumnEqual(ByVal A As Object, ByVal B As Object) As Boolean

        If A Is DBNull.Value AndAlso B Is DBNull.Value Then Return True ' Both are DBNull.Value.
        If A Is DBNull.Value OrElse B Is DBNull.Value Then Return False ' Only one is DBNull.Value.

        Return A.ToString.Trim = B.ToString.Trim                    ' Value type standard comparison

    End Function

End Class

