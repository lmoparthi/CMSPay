Option Strict On
Imports System.Text

Public Class SortDialog
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
    Friend WithEvents pnlBorder As System.Windows.Forms.Panel
    Friend WithEvents ColGrid As DataGridCustom
    Friend WithEvents VSplit As System.Windows.Forms.Splitter
    Friend WithEvents SortPanel As System.Windows.Forms.Panel
    Friend WithEvents SortUp As System.Windows.Forms.Button
    Friend WithEvents SortDown As System.Windows.Forms.Button
    Friend WithEvents TTip As System.Windows.Forms.ToolTip
    Friend WithEvents Ok As System.Windows.Forms.Button
    Friend WithEvents Cancel As System.Windows.Forms.Button
    Friend WithEvents ControlPanel As System.Windows.Forms.Panel
    Friend WithEvents RemCol As System.Windows.Forms.Button
    Friend WithEvents RemAll As System.Windows.Forms.Button
    Friend WithEvents AddAll As System.Windows.Forms.Button
    Friend WithEvents AddCol As System.Windows.Forms.Button
    Friend WithEvents SortGrid As DataGridCustom
    '<System.Diagnostics.DebuggerStepThrough()> 
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(SortDialog))
        Me.pnlBorder = New System.Windows.Forms.Panel
        Me.SortGrid = New DataGridCustom
        Me.ControlPanel = New System.Windows.Forms.Panel
        Me.RemCol = New System.Windows.Forms.Button
        Me.RemAll = New System.Windows.Forms.Button
        Me.AddAll = New System.Windows.Forms.Button
        Me.AddCol = New System.Windows.Forms.Button
        Me.SortPanel = New System.Windows.Forms.Panel
        Me.SortDown = New System.Windows.Forms.Button
        Me.SortUp = New System.Windows.Forms.Button
        Me.VSplit = New System.Windows.Forms.Splitter
        Me.ColGrid = New DataGridCustom
        Me.TTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.Ok = New System.Windows.Forms.Button
        Me.Cancel = New System.Windows.Forms.Button
        Me.pnlBorder.SuspendLayout()
        CType(Me.SortGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ControlPanel.SuspendLayout()
        Me.SortPanel.SuspendLayout()
        CType(Me.ColGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlBorder
        '
        Me.pnlBorder.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlBorder.Controls.Add(Me.SortGrid)
        Me.pnlBorder.Controls.Add(Me.ControlPanel)
        Me.pnlBorder.Controls.Add(Me.SortPanel)
        Me.pnlBorder.Controls.Add(Me.VSplit)
        Me.pnlBorder.Controls.Add(Me.ColGrid)
        Me.pnlBorder.Location = New System.Drawing.Point(8, 8)
        Me.pnlBorder.Name = "pnlBorder"
        Me.pnlBorder.Size = New System.Drawing.Size(512, 200)
        Me.pnlBorder.TabIndex = 0
        '
        'SortGrid
        '
        Me.SortGrid.AllowColumnReorder = False
        Me.SortGrid.AllowCopy = False
        Me.SortGrid.AllowDelete = False
        Me.SortGrid.AllowDragDrop = False
        Me.SortGrid.AllowEdit = True
        Me.SortGrid.AllowExport = False
        Me.SortGrid.AllowFind = False
        Me.SortGrid.AllowGoTo = False
        Me.SortGrid.AllowMultiSelect = True
        Me.SortGrid.AllowMultiSort = False
        Me.SortGrid.AllowNew = False
        Me.SortGrid.AllowRefresh = False
        Me.SortGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.SortGrid.CaptionText = "Sorting"
        Me.SortGrid.ColumnHeadersVisible = False
        Me.SortGrid.ConfirmDelete = True
        Me.SortGrid.CopySelectedOnly = True
        Me.SortGrid.DataMember = ""
        Me.SortGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SortGrid.ExportSelectedOnly = True
        Me.SortGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.SortGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.SortGrid.Location = New System.Drawing.Point(197, 0)
        Me.SortGrid.MultiSort = False
        Me.SortGrid.Name = "SortGrid"
        Me.SortGrid.Size = New System.Drawing.Size(275, 200)
        Me.SortGrid.TabIndex = 6
        Me.TTip.SetToolTip(Me.SortGrid, "Column Sorting")
        '
        'ControlPanel
        '
        Me.ControlPanel.Controls.Add(Me.RemCol)
        Me.ControlPanel.Controls.Add(Me.RemAll)
        Me.ControlPanel.Controls.Add(Me.AddAll)
        Me.ControlPanel.Controls.Add(Me.AddCol)
        Me.ControlPanel.Dock = System.Windows.Forms.DockStyle.Left
        Me.ControlPanel.Location = New System.Drawing.Point(149, 0)
        Me.ControlPanel.Name = "ControlPanel"
        Me.ControlPanel.Size = New System.Drawing.Size(48, 200)
        Me.ControlPanel.TabIndex = 5
        '
        'RemCol
        '
        Me.RemCol.Location = New System.Drawing.Point(8, 152)
        Me.RemCol.Name = "RemCol"
        Me.RemCol.Size = New System.Drawing.Size(32, 23)
        Me.RemCol.TabIndex = 3
        Me.RemCol.Text = "<"
        Me.TTip.SetToolTip(Me.RemCol, "Remove Selected Columns From Sorting")
        '
        'RemAll
        '
        Me.RemAll.Location = New System.Drawing.Point(8, 120)
        Me.RemAll.Name = "RemAll"
        Me.RemAll.Size = New System.Drawing.Size(32, 23)
        Me.RemAll.TabIndex = 2
        Me.RemAll.Text = "<<"
        Me.TTip.SetToolTip(Me.RemAll, "Remove All Columns From Sorting")
        '
        'AddAll
        '
        Me.AddAll.Location = New System.Drawing.Point(8, 72)
        Me.AddAll.Name = "AddAll"
        Me.AddAll.Size = New System.Drawing.Size(32, 23)
        Me.AddAll.TabIndex = 1
        Me.AddAll.Text = ">>"
        Me.TTip.SetToolTip(Me.AddAll, "Add All Columns For Sorting")
        '
        'AddCol
        '
        Me.AddCol.Location = New System.Drawing.Point(8, 40)
        Me.AddCol.Name = "AddCol"
        Me.AddCol.Size = New System.Drawing.Size(32, 23)
        Me.AddCol.TabIndex = 0
        Me.AddCol.Text = ">"
        Me.TTip.SetToolTip(Me.AddCol, "Add Selected Columns For Sorting")
        '
        'SortPanel
        '
        Me.SortPanel.Controls.Add(Me.SortDown)
        Me.SortPanel.Controls.Add(Me.SortUp)
        Me.SortPanel.Dock = System.Windows.Forms.DockStyle.Right
        Me.SortPanel.Location = New System.Drawing.Point(472, 0)
        Me.SortPanel.Name = "SortPanel"
        Me.SortPanel.Size = New System.Drawing.Size(40, 200)
        Me.SortPanel.TabIndex = 3
        '
        'SortDown
        '
        Me.SortDown.Image = CType(resources.GetObject("SortDown.Image"), System.Drawing.Image)
        Me.SortDown.Location = New System.Drawing.Point(4, 110)
        Me.SortDown.Name = "SortDown"
        Me.SortDown.Size = New System.Drawing.Size(32, 30)
        Me.SortDown.TabIndex = 2
        Me.TTip.SetToolTip(Me.SortDown, "Decrease Selected Columns Sort Position")
        '
        'SortUp
        '
        Me.SortUp.Image = CType(resources.GetObject("SortUp.Image"), System.Drawing.Image)
        Me.SortUp.Location = New System.Drawing.Point(4, 62)
        Me.SortUp.Name = "SortUp"
        Me.SortUp.Size = New System.Drawing.Size(32, 30)
        Me.SortUp.TabIndex = 1
        Me.TTip.SetToolTip(Me.SortUp, "Increase Selected Columns Sort Position")
        '
        'VSplit
        '
        Me.VSplit.BackColor = System.Drawing.SystemColors.ControlDark
        Me.VSplit.Location = New System.Drawing.Point(144, 0)
        Me.VSplit.Name = "VSplit"
        Me.VSplit.Size = New System.Drawing.Size(5, 200)
        Me.VSplit.TabIndex = 2
        Me.VSplit.TabStop = False
        '
        'ColGrid
        '
        Me.ColGrid.AllowColumnReorder = False
        Me.ColGrid.AllowCopy = False
        Me.ColGrid.AllowDelete = False
        Me.ColGrid.AllowDragDrop = False
        Me.ColGrid.AllowEdit = False
        Me.ColGrid.AllowExport = False
        Me.ColGrid.AllowFind = False
        Me.ColGrid.AllowGoTo = False
        Me.ColGrid.AllowMultiSelect = True
        Me.ColGrid.AllowMultiSort = False
        Me.ColGrid.AllowNew = False
        Me.ColGrid.AllowRefresh = False
        Me.ColGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.ColGrid.CaptionText = "Available Columns"
        Me.ColGrid.ColumnHeadersVisible = False
        Me.ColGrid.ConfirmDelete = True
        Me.ColGrid.CopySelectedOnly = True
        Me.ColGrid.DataMember = ""
        Me.ColGrid.Dock = System.Windows.Forms.DockStyle.Left
        Me.ColGrid.ExportSelectedOnly = True
        Me.ColGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.ColGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ColGrid.Location = New System.Drawing.Point(0, 0)
        Me.ColGrid.MultiSort = False
        Me.ColGrid.Name = "ColGrid"
        Me.ColGrid.ReadOnly = True
        Me.ColGrid.Size = New System.Drawing.Size(144, 200)
        Me.ColGrid.TabIndex = 0
        Me.TTip.SetToolTip(Me.ColGrid, "Columns Available For Sort")
        '
        'Ok
        '
        Me.Ok.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Ok.Location = New System.Drawing.Point(448, 216)
        Me.Ok.Name = "Ok"
        Me.Ok.TabIndex = 1
        Me.Ok.Text = "&Ok"
        '
        'Cancel
        '
        Me.Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Cancel.Location = New System.Drawing.Point(368, 216)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.TabIndex = 2
        Me.Cancel.Text = "&Cancel"
        '
        'SortDialog
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(528, 245)
        Me.Controls.Add(Me.Cancel)
        Me.Controls.Add(Me.Ok)
        Me.Controls.Add(Me.pnlBorder)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(416, 240)
        Me.Name = "SortDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Choose Sort Options"
        Me.pnlBorder.ResumeLayout(False)
        CType(Me.SortGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ControlPanel.ResumeLayout(False)
        Me.SortPanel.ResumeLayout(False)
        CType(Me.ColGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private _APPKEY As String = "UFCW\"

    Private _MyGrid As DataGridCustom
    Private _ColumnTable As New DataTable("Columns")
    Private _SortTable As New DataTable("Sorting")

    Private _Disposed As Boolean = False
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)

        If _Disposed Then Return

        If disposing Then

            If _ColumnTable IsNot Nothing Then _ColumnTable.Dispose()
            _ColumnTable = Nothing

            If _SortTable IsNot Nothing Then _SortTable.Dispose()
            _SortTable = Nothing

            If (components IsNot Nothing) Then
                components.Dispose()
            End If

        End If

        _Disposed = True

        MyBase.Dispose(disposing)
    End Sub

    <System.ComponentModel.Description("Specify the application grouping criteria (e.g Claims, Worflow, etc).")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal Value As String)
            _APPKEY = Value

        End Set
    End Property

    Public Property SortColumns As String

        Get
            Dim FinalSortColumns As New StringBuilder

            For Each DR As DataRow In _SortTable.Rows
                'TmpSort &= If(cnt > 0, ", ", "") & If(CStr(_SortTable.Rows(cnt)("Column")).StartsWith("[") = True, "", "[") & _SortTable.Rows(cnt)("Column").ToString & If(CStr(_SortTable.Rows(cnt)("Column")).EndsWith("]") = True, " ", "] ") & _SortTable.Rows(cnt)("Order").ToString
                FinalSortColumns.Append(If(FinalSortColumns.Length > 0, ", [", "[") & DR("COLUMN").ToString.Trim & "] " & DR("ORDER").ToString)
            Next

            Return FinalSortColumns.ToString

        End Get
        Set(value As String)

            _SortTable.Clear()
            Dim Cols() As String = value.Split(CChar(","))

            For Each col As String In Cols
                Dim DR As DataRow = _SortTable.NewRow
                DR("Column") = col.ToUpper.Replace("[", "").Replace("]", "").Replace("DESC", "").Replace("ASC", "")
                DR("Order") = If(col.ToUpper.Contains(" DESC"), "Desc", "Asc")
            Next

        End Set
    End Property

    Sub New(ByVal dg As DataGridCustom)
        Me.New()

        Dim DT As DataTable
        Dim DR As DataRow

        Dim DGTS As DataGridTableStyle
        Dim TextCol As DataGridHighlightTextBoxColumn
        Dim ComboCol As DataGridHighlightComboBoxColumn

        Dim Cnt As Integer
        Dim DV As DataView
        Dim Mapping As String = ""
        Dim DispText As String = ""
        Dim Pos As Integer? = 0

        Try
            _MyGrid = dg

            _ColumnTable.Columns.Add("Column")
            _ColumnTable.Columns.Add("HeaderText")

            _SortTable.Columns.Add("Column")
            _SortTable.Columns.Add("HeaderText")
            _SortTable.Columns.Add("Order")

            _SortTable.Columns("Column").ReadOnly = True

            DGTS = New DataGridTableStyle
            DGTS.MappingName = "Columns"

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "HeaderText"
            TextCol.HeaderText = "Column Name"
            TextCol.NullText = ""
            TextCol.Width = ColGrid.Width - ColGrid.RowHeaderWidth - 6

            DGTS.GridColumnStyles.Add(TextCol)

            ColGrid.TableStyles.Add(DGTS)
            ColGrid.DataSource = _ColumnTable


            DGTS = New DataGridTableStyle
            DGTS.MappingName = "Sorting"

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "HeaderText"
            TextCol.HeaderText = "Column Name"
            TextCol.NullText = ""
            TextCol.ReadOnly = True
            TextCol.Width = SortGrid.Width - SortGrid.RowHeaderWidth - 66

            DGTS.GridColumnStyles.Add(TextCol)

            ComboCol = New DataGridHighlightComboBoxColumn(SortGrid)
            ComboCol.MappingName = "Order"
            ComboCol.HeaderText = "Sort Order"
            ComboCol.NullText = ""
            ComboCol._ColumnComboBox.DropDownStyle = ComboBoxStyle.DropDownList
            ComboCol._ColumnComboBox.Items.Add("ASC")
            ComboCol._ColumnComboBox.Items.Add("DESC")
            ComboCol.Width = 60

            DGTS.PreferredRowHeight = ComboCol._ColumnComboBox.Height + 2
            DGTS.GridColumnStyles.Add(ComboCol)

            SortGrid.TableStyles.Add(DGTS)
            SortGrid.DataSource = _SortTable

            DT = _MyGrid.GetCurrentDataTable

            'For Cnt = 0 To UBound(Cols, 1)
            '    Cols(Cnt) = Cols(Cnt).Trim
            'Next

            'For Cnt = 0 To UBound(Cols, 1)
            '    If Cols(Cnt).TrimEnd <> "" Then
            '        If Cols(Cnt).ToUpper.EndsWith("DESC") Then
            '            Order = "DESC"
            '        Else
            '            Order = "ASC"
            '        End If

            '        If Cols(Cnt).ToUpper.EndsWith("DESC") Then
            '            Col = Microsoft.VisualBasic.Left(Cols(Cnt), Len(Cols(Cnt)) - 4).TrimEnd
            '        ElseIf Cols(Cnt).ToUpper.EndsWith("ASC") Then
            '            Col = Microsoft.VisualBasic.Left(Cols(Cnt), Len(Cols(Cnt)) - 3).TrimEnd
            '        Else
            '            Col = Cols(Cnt).TrimEnd
            '        End If

            '        Col = Replace(Replace(Col, "[", ""), "]", "")

            '        If _MyGrid.GetColumnPosition(Col) >= 0 Then
            '            DR = _SortTable.NewRow
            '            DR("Column") = Col
            '            DR("Order") = Order

            '            _SortTable.Rows.Add(DR)
            '        End If
            '    End If
            'Next

            'FindValidRows:
            'For Cnt = 0 To _SortTable.Rows.Count - 1
            '    DV.RowFilter = "Column = '" & _SortTable.Rows(Cnt)("Column").ToString.Trim & "'"

            '    Pos = _MyGrid.GetColumnPosition(_SortTable.Rows(Cnt)("Column").ToString.Trim)

            '    If DV.Count > 0 Then
            '        DispText = _MyGrid.GetCurrentTableStyle.GridColumnStyles(CInt(Pos)).HeaderText

            '        If DispText.Trim = "" Then
            '            DispText = _SortTable.Rows(Cnt)("Column").ToString.Trim
            '        End If

            '        _SortTable.Rows(Cnt)("HeaderText") = DispText
            '    End If
            'Next

            'DV = New DataView(_SortTable, "", "Column", DataViewRowState.CurrentRows)

            For Cnt = 0 To _MyGrid.GetGridColumnCount - 1
                Mapping = _MyGrid.GetCurrentTableStyle.GridColumnStyles(Cnt).MappingName
                DispText = _MyGrid.GetCurrentTableStyle.GridColumnStyles(Cnt).HeaderText

                If DispText.Trim = "" Then
                    DispText = Mapping
                End If

                DR = _ColumnTable.NewRow

                DR("Column") = Mapping
                DR("HeaderText") = DispText

                _ColumnTable.Rows.Add(DR)
            Next

            Dim CurrentSort As String = _MyGrid.Sort

            Dim Cols() As String

            If CurrentSort.Length > 0 Then
                Cols = CurrentSort.Split(CChar(","))

                For Each col As String In Cols
                    DR = _SortTable.NewRow
                    DR("Column") = col.ToUpper.Replace("[", "").Replace("]", "").Replace("DESC", "").Replace("ASC", "")
                    DR("Order") = If(col.ToUpper.Contains(" DESC"), "DESC", "ASC")
                    DR("HeaderText") = _ColumnTable.Select("COLUMN = '" & DR("Column").ToString.Trim & "'")(0)("HeaderText").ToString.Trim
                    _SortTable.Rows.Add(DR)
                Next

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message & ex.StackTrace, "Error During SortDialog.New", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            DV = Nothing
            DT = Nothing

            If DGTS IsNot Nothing Then DGTS.Dispose()
            DGTS = Nothing
            If TextCol IsNot Nothing Then TextCol.Dispose()
            TextCol = Nothing
            If ComboCol IsNot Nothing Then ComboCol.Dispose()
            ComboCol = Nothing

        End Try
    End Sub

    Private Sub SortDialog_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetSettings()
    End Sub

    Private Sub SortDialog_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        SaveSettings()
    End Sub

    Private Sub SetSettings()

        Me.Top = CInt(If(CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString))))
        Me.Height = CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = CInt(If(CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString))))
        Me.Width = CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(Me.AppKey, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)

        VSplit.SplitPosition = CInt(GetSetting(Me.AppKey, "SortSelector\Settings", "VSplit", CStr(VSplit.SplitPosition)))

    End Sub

    Private Sub SaveSettings()
        Dim lWindowState As FormWindowState = Me.WindowState
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "WindowState", CInt(lWindowState).ToString)
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "VSplit", CStr(VSplit.SplitPosition))

        Me.WindowState = FormWindowState.Normal
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = lWindowState

    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Sub Ok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Ok.Click

        Me.DialogResult = DialogResult.OK

    End Sub

    Private Sub ColGrid_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles ColGrid.Resize
        Dim TS As DataGridTableStyle = ColGrid.GetCurrentTableStyle

        If Not TS Is Nothing Then
            TS.GridColumnStyles(0).Width = ColGrid.Width - ColGrid.RowHeaderWidth - 6
        End If
    End Sub

    Private Sub SortGrid_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles SortGrid.Resize
        Dim TS As DataGridTableStyle = SortGrid.GetCurrentTableStyle

        If Not TS Is Nothing Then
            TS.GridColumnStyles(0).Width = SortGrid.Width - SortGrid.RowHeaderWidth - TS.GridColumnStyles(1).Width - 6
        End If
    End Sub

    Private Sub AddAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddAll.Click
        Dim cnt As Integer
        Dim DR As DataRow

        Me.Cursor = Cursors.WaitCursor

        For cnt = 0 To _ColumnTable.Rows.Count - 1
            DR = _SortTable.NewRow

            DR("Column") = _ColumnTable.Rows(cnt)("Column")
            DR("HeaderText") = _ColumnTable.Rows(cnt)("HeaderText")
            DR("Order") = "ASC"

            _SortTable.Rows.Add(DR)
        Next

        For cnt = _ColumnTable.Rows.Count - 1 To 0 Step -1
            _ColumnTable.Rows.RemoveAt(cnt)
        Next

        SortGrid.MoveGridToRow(SortGrid.GetGridRowCount)

        Me.Cursor = Cursors.Default
    End Sub

    Private Sub RemAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemAll.Click
        Dim cnt As Integer
        Dim DR As DataRow

        Me.Cursor = Cursors.WaitCursor

        For cnt = 0 To _SortTable.Rows.Count - 1
            DR = _ColumnTable.NewRow

            DR("Column") = _SortTable.Rows(cnt)("Column")
            DR("HeaderText") = _SortTable.Rows(cnt)("HeaderText")

            _ColumnTable.Rows.Add(DR)
        Next

        For cnt = _SortTable.Rows.Count - 1 To 0 Step -1
            _SortTable.Rows.RemoveAt(cnt)
        Next

        ColGrid.MoveGridToRow(ColGrid.GetGridRowCount)

        Me.Cursor = Cursors.Default
    End Sub

    Private Sub AddCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddCol.Click
        If _ColumnTable.Rows.Count < 1 Then Exit Sub

        Dim cnt As Integer
        Dim DR As DataRow
        Dim DelRows(0) As Integer

        Me.Cursor = Cursors.WaitCursor

        For cnt = 0 To _ColumnTable.Rows.Count - 1
            If ColGrid.IsSelected(cnt) = True Then
                DR = _SortTable.NewRow

                DR("Column") = _ColumnTable.Rows(cnt)("Column")
                DR("HeaderText") = _ColumnTable.Rows(cnt)("HeaderText")
                DR("Order") = "ASC"

                _SortTable.Rows.Add(DR)

                DelRows(UBound(DelRows, 1)) = cnt

                ReDim Preserve DelRows(UBound(DelRows, 1) + 1)
            End If
        Next

        ReDim Preserve DelRows(UBound(DelRows, 1) - 1)

        If UBound(DelRows, 1) < 0 Then
            DR = _SortTable.NewRow

            DR("Column") = _ColumnTable.Rows(ColGrid.CurrentRowIndex)("Column")
            DR("HeaderText") = _ColumnTable.Rows(ColGrid.CurrentRowIndex)("HeaderText")
            DR("Order") = "ASC"

            _SortTable.Rows.Add(DR)

            ReDim Preserve DelRows(UBound(DelRows, 1) + 1)

            DelRows(UBound(DelRows, 1)) = ColGrid.CurrentRowIndex
        End If

        SortGrid.MoveGridToRow(SortGrid.GetGridRowCount)

        For cnt = UBound(DelRows, 1) To 0 Step -1
            _ColumnTable.Rows.RemoveAt(DelRows(cnt))
        Next

        Me.Cursor = Cursors.Default

    End Sub

    Private Sub RemCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemCol.Click
        If _SortTable.Rows.Count < 1 Then Exit Sub

        Dim cnt As Integer
        Dim DR As DataRow
        Dim DelRows(0) As Integer

        Me.Cursor = Cursors.WaitCursor

        For cnt = 0 To _SortTable.Rows.Count - 1
            If SortGrid.IsSelected(cnt) = True Then
                DR = _ColumnTable.NewRow

                DR("Column") = _SortTable.Rows(cnt)("Column")
                DR("HeaderText") = _SortTable.Rows(cnt)("HeaderText")

                _ColumnTable.Rows.Add(DR)

                DelRows(UBound(DelRows, 1)) = cnt

                ReDim Preserve DelRows(UBound(DelRows, 1) + 1)
            End If
        Next

        ReDim Preserve DelRows(UBound(DelRows, 1) - 1)

        If UBound(DelRows, 1) < 0 Then
            DR = _ColumnTable.NewRow

            DR("Column") = _SortTable.Rows(SortGrid.CurrentRowIndex)("Column")
            DR("HeaderText") = _SortTable.Rows(SortGrid.CurrentRowIndex)("HeaderText")

            _ColumnTable.Rows.Add(DR)

            ReDim Preserve DelRows(UBound(DelRows, 1) + 1)

            DelRows(UBound(DelRows, 1)) = SortGrid.CurrentRowIndex
        End If

        ColGrid.MoveGridToRow(ColGrid.GetGridRowCount)

        For cnt = UBound(DelRows, 1) To 0 Step -1
            _SortTable.Rows.RemoveAt(DelRows(cnt))
        Next
        'For cnt = ColumnTable.Rows.Count - 1 To 0 Step -1
        '    If ColGrid.IsSelected(cnt) = True Then
        '        ColumnTable.Rows.RemoveAt(cnt)
        '        cnt += 1
        '    End If
        'Next

        Me.Cursor = Cursors.Default
    End Sub

    Private Sub SortUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SortUp.Click

        If _SortTable.Rows.Count < 1 Then Exit Sub

        Dim DR As DataRow
        Dim TmpDT As DataTable
        Dim DV As DataView
        Dim OrigFirstR As Integer?

        Try

            Me.Cursor = Cursors.WaitCursor

            TmpDT = New DataTable("SortOrder")
            OrigFirstR = SortGrid.FirstVisibleRow

            TmpDT.Columns.Add("Column")
            TmpDT.Columns.Add("HeaderText")
            TmpDT.Columns.Add("Order")
            TmpDT.Columns.Add("OrigPosition", System.Type.GetType("System.Int32"))
            TmpDT.Columns.Add("Position", System.Type.GetType("System.Int32"))
            TmpDT.Columns.Add("Selected")

            For Cnt As Integer = 0 To _SortTable.Rows.Count - 1
                DR = TmpDT.NewRow

                DR("Column") = _SortTable.Rows(Cnt)("Column")
                DR("HeaderText") = _SortTable.Rows(Cnt)("HeaderText")
                DR("Order") = _SortTable.Rows(Cnt)("Order")
                DR("OrigPosition") = Cnt
                DR("Position") = Cnt
                DR("Selected") = SortGrid.IsSelected(Cnt)

                TmpDT.Rows.Add(DR)
            Next

            For Cnt As Integer = 0 To _SortTable.Rows.Count - 1
                If SortGrid.IsSelected(Cnt) = True Then
                    TmpDT.Rows(Cnt)("Position") = CInt(TmpDT.Rows(Cnt)("Position")) - 1
                End If
            Next

            For Cnt As Integer = 0 To _SortTable.Rows.Count - 1
                If CBool(TmpDT.Rows(Cnt)("Selected")) Then
                    If Cnt > 0 Then
                        If CBool(TmpDT.Rows(Cnt - 1)("Selected")) Then
                            If Cnt > 1 Then
                                TmpDT.Rows(Cnt - 2)("Position") = CInt(TmpDT.Rows(Cnt - 2)("Position")) + 2
                            End If
                        End If
                    End If
                End If
            Next

            DV = New DataView(TmpDT, "", "Position, OrigPosition DESC", DataViewRowState.CurrentRows)

            _SortTable.Rows.Clear()

            For Cnt As Integer = 0 To DV.Count - 1
                DR = _SortTable.NewRow

                DR("Column") = DV(Cnt)("Column")
                DR("HeaderText") = DV(Cnt)("HeaderText")
                DR("Order") = DV(Cnt)("Order")

                _SortTable.Rows.Add(DR)
            Next

            SortGrid.MoveGridToRow(CInt(OrigFirstR))

            For Cnt As Integer = 0 To DV.Count - 1
                If CBool(DV(Cnt)("Selected")) Then
                    SortGrid.Select(Cnt)

                    If Cnt < SortGrid.FirstVisibleRow + 1 Then
                        SortGrid.MoveGridToRow(CInt(If(Cnt > 0, Cnt - (SortGrid.VisibleRowCount / 3) - 1, Cnt)))
                    End If
                End If
            Next

        Catch ex As Exception
            Throw
        Finally

            If DV IsNot Nothing Then DV.Dispose()
            DV = Nothing

            If TmpDT IsNot Nothing Then TmpDT.Dispose()
            TmpDT = Nothing

            Me.Cursor = Cursors.Default

        End Try
    End Sub

    Private Sub SortDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SortDown.Click

        If _SortTable.Rows.Count < 1 Then Exit Sub

        Dim DR As DataRow
        Dim TmpDT As DataTable
        Dim DV As DataView
        Dim FirstRow As Integer? = 0
        Dim OrigFirstR As Integer?

        Try

            Me.Cursor = Cursors.WaitCursor

            OrigFirstR = SortGrid.FirstVisibleRow

            TmpDT = New DataTable("SortOrder")

            TmpDT.Columns.Add("Column")
            TmpDT.Columns.Add("HeaderText")
            TmpDT.Columns.Add("Order")
            TmpDT.Columns.Add("OrigPosition", System.Type.GetType("System.Int32"))
            TmpDT.Columns.Add("Position", System.Type.GetType("System.Int32"))
            TmpDT.Columns.Add("Selected")

            For Cnt = 0 To _SortTable.Rows.Count - 1
                DR = TmpDT.NewRow

                DR("Column") = _SortTable.Rows(Cnt)("Column")
                DR("HeaderText") = _SortTable.Rows(Cnt)("HeaderText")
                DR("Order") = _SortTable.Rows(Cnt)("Order")
                DR("OrigPosition") = Cnt
                DR("Position") = Cnt
                DR("Selected") = SortGrid.IsSelected(Cnt)

                TmpDT.Rows.Add(DR)
            Next

            For Cnt As Integer = 0 To _SortTable.Rows.Count - 1
                If SortGrid.IsSelected(Cnt) = True Then
                    TmpDT.Rows(Cnt)("Position") = CInt(TmpDT.Rows(Cnt)("Position")) + 1
                End If
            Next

            For Cnt As Integer = 0 To _SortTable.Rows.Count - 1
                If CBool(TmpDT.Rows(Cnt)("Selected")) Then
                    If Cnt < _SortTable.Rows.Count - 1 Then
                        If CBool(TmpDT.Rows(Cnt + 1)("Selected")) Then
                            If Cnt < _SortTable.Rows.Count - 2 Then
                                TmpDT.Rows(Cnt + 2)("Position") = CInt(TmpDT.Rows(Cnt + 2)("Position")) - 2
                            End If
                        End If
                    End If
                End If
            Next

            DV = New DataView(TmpDT, "", "Position, OrigPosition DESC", DataViewRowState.CurrentRows)

            _SortTable.Rows.Clear()

            For Cnt As Integer = 0 To DV.Count - 1
                DR = _SortTable.NewRow

                DR("Column") = DV(Cnt)("Column")
                DR("HeaderText") = DV(Cnt)("HeaderText")
                DR("Order") = DV(Cnt)("Order")

                _SortTable.Rows.Add(DR)
            Next

            SortGrid.MoveGridToRow(CInt(OrigFirstR))

            For Cnt As Integer = 0 To DV.Count - 1
                If CBool(DV(Cnt)("Selected")) Then
                    SortGrid.Select(Cnt)

                    FirstRow = SortGrid.FirstVisibleRow

                    If Cnt < FirstRow Or Cnt > (FirstRow + SortGrid.VisibleRowCount - 2) Then
                        SortGrid.MoveGridToRow(CInt(Cnt - (SortGrid.VisibleRowCount / 3)))
                    End If

                End If
            Next


        Catch ex As Exception
            Throw
        Finally

            If DV IsNot Nothing Then DV.Dispose()
            DV = Nothing

            If TmpDT IsNot Nothing Then TmpDT.Dispose()
            TmpDT = Nothing

            Me.Cursor = Cursors.Default

        End Try
    End Sub
End Class

