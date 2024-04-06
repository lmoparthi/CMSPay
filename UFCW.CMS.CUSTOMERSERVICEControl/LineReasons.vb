Option Strict On
Option Infer On

Public Class LineReasonsForm
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _ClaimID As Integer
    Private _LineNumber As Short
    Private _APPKEY As String = "UFCW\Claims\"

    Friend WithEvents _ClaimDS As New ClaimDataset
    Private _ReadOnly As Boolean

    Const MAXREASONS As Integer = 5

#Region " Windows Form Designer generated code "
    Public Sub New(ByVal ClaimID As Integer, ByVal LineNumber As Short, ByVal reasonDT As DataTable)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _ClaimID = ClaimID
        _LineNumber = LineNumber

        Dim DV As New DataView(reasonDT, "", "", DataViewRowState.CurrentRows)

        DV.Sort = "LINE_NBR"
        DV.RowFilter = "LINE_NBR = " & _LineNumber

        If DV.Count > 0 Then
            For cnt As Integer = 0 To DV.Count - 1
                _ClaimDS.REASON.Rows.Add(DV(cnt).Row.ItemArray)
            Next
        End If
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
            If _SelectedReasons IsNot Nothing Then _SelectedReasons.Dispose()

        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents AddButton As System.Windows.Forms.Button
    Friend WithEvents CancelActionButton As System.Windows.Forms.Button
    Friend WithEvents SaveButton As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents SortDownButton As System.Windows.Forms.Button
    Friend WithEvents SortUpButton As System.Windows.Forms.Button
    Friend WithEvents LineReasonsDataGrid As DataGridCustom
    Friend WithEvents DeleteButton As System.Windows.Forms.Button
    Friend WithEvents MainToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents ReasonCodesTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ReasonLookupButton As System.Windows.Forms.Button
    Private _SelectedReasons As ClaimDataset.REASONDataTable

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LineReasonsForm))
        Me.AddButton = New System.Windows.Forms.Button()
        Me.CancelActionButton = New System.Windows.Forms.Button()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.LineReasonsDataGrid = New DataGridCustom()
        Me.ReasonCodesTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SortDownButton = New System.Windows.Forms.Button()
        Me.SortUpButton = New System.Windows.Forms.Button()
        Me.DeleteButton = New System.Windows.Forms.Button()
        Me.MainToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.ReasonLookupButton = New System.Windows.Forms.Button()
        CType(Me.LineReasonsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AddButton
        '
        Me.AddButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AddButton.Enabled = False
        Me.AddButton.Location = New System.Drawing.Point(440, 8)
        Me.AddButton.Name = "AddButton"
        Me.AddButton.Size = New System.Drawing.Size(75, 23)
        Me.AddButton.TabIndex = 1
        Me.AddButton.Text = "&Add"
        Me.MainToolTip.SetToolTip(Me.AddButton, "Add Reason Code")
        Me.AddButton.Visible = False
        '
        'CancelActionButton
        '
        Me.CancelActionButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CancelActionButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelActionButton.Location = New System.Drawing.Point(8, 268)
        Me.CancelActionButton.Name = "CancelActionButton"
        Me.CancelActionButton.Size = New System.Drawing.Size(75, 23)
        Me.CancelActionButton.TabIndex = 6
        Me.CancelActionButton.Text = "&Cancel"
        Me.MainToolTip.SetToolTip(Me.CancelActionButton, "Cancel Changes")
        '
        'SaveButton
        '
        Me.SaveButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SaveButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.SaveButton.Location = New System.Drawing.Point(440, 268)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(75, 23)
        Me.SaveButton.TabIndex = 7
        Me.SaveButton.Text = "&Save"
        Me.MainToolTip.SetToolTip(Me.SaveButton, "Save Changes")
        Me.SaveButton.Visible = False
        '
        'LineReasonsDataGrid
        '
        Me.LineReasonsDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.LineReasonsDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.LineReasonsDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LineReasonsDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LineReasonsDataGrid.ADGroupsThatCanFind = ""
        Me.LineReasonsDataGrid.ADGroupsThatCanMultiSort = ""
        Me.LineReasonsDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LineReasonsDataGrid.AllowAutoSize = True
        Me.LineReasonsDataGrid.AllowColumnReorder = False
        Me.LineReasonsDataGrid.AllowCopy = True
        Me.LineReasonsDataGrid.AllowCustomize = True
        Me.LineReasonsDataGrid.AllowDelete = False
        Me.LineReasonsDataGrid.AllowDragDrop = False
        Me.LineReasonsDataGrid.AllowEdit = False
        Me.LineReasonsDataGrid.AllowExport = False
        Me.LineReasonsDataGrid.AllowFilter = True
        Me.LineReasonsDataGrid.AllowFind = True
        Me.LineReasonsDataGrid.AllowGoTo = True
        Me.LineReasonsDataGrid.AllowMultiSelect = True
        Me.LineReasonsDataGrid.AllowMultiSort = False
        Me.LineReasonsDataGrid.AllowNew = False
        Me.LineReasonsDataGrid.AllowPrint = False
        Me.LineReasonsDataGrid.AllowRefresh = False
        Me.LineReasonsDataGrid.AllowSorting = False
        Me.LineReasonsDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LineReasonsDataGrid.AppKey = "UFCW\Claims\"
        Me.LineReasonsDataGrid.AutoSaveCols = True
        Me.LineReasonsDataGrid.BackgroundColor = System.Drawing.SystemColors.Control
        Me.LineReasonsDataGrid.CaptionVisible = False
        Me.LineReasonsDataGrid.ColumnHeaderLabel = Nothing
        Me.LineReasonsDataGrid.ColumnRePositioning = False
        Me.LineReasonsDataGrid.ColumnResizing = False
        Me.LineReasonsDataGrid.ConfirmDelete = False
        Me.LineReasonsDataGrid.CopySelectedOnly = True
        Me.LineReasonsDataGrid.CurrentBSPosition = -1
        Me.LineReasonsDataGrid.DataMember = ""
        Me.LineReasonsDataGrid.DragColumn = 0
        Me.LineReasonsDataGrid.ExportSelectedOnly = True
        Me.LineReasonsDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.LineReasonsDataGrid.HighlightedRow = Nothing
        Me.LineReasonsDataGrid.HighLightModifiedRows = False
        Me.LineReasonsDataGrid.IsMouseDown = False
        Me.LineReasonsDataGrid.LastGoToLine = ""
        Me.LineReasonsDataGrid.Location = New System.Drawing.Point(8, 40)
        Me.LineReasonsDataGrid.MultiSort = False
        Me.LineReasonsDataGrid.Name = "LineReasonsDataGrid"
        Me.LineReasonsDataGrid.OldSelectedRow = Nothing
        Me.LineReasonsDataGrid.PreviousBSPosition = -1
        Me.LineReasonsDataGrid.ReadOnly = True
        Me.LineReasonsDataGrid.RetainRowSelectionAfterSort = True
        Me.LineReasonsDataGrid.SetRowOnRightClick = True
        Me.LineReasonsDataGrid.ShiftPressed = False
        Me.LineReasonsDataGrid.SingleClickBooleanColumns = True
        Me.LineReasonsDataGrid.Size = New System.Drawing.Size(472, 224)
        Me.LineReasonsDataGrid.Sort = Nothing
        Me.LineReasonsDataGrid.StyleName = ""
        Me.LineReasonsDataGrid.SubKey = ""
        Me.LineReasonsDataGrid.SuppressMouseDown = False
        Me.LineReasonsDataGrid.SuppressTriangle = False
        Me.LineReasonsDataGrid.TabIndex = 2
        '
        'ReasonCodesTextBox
        '
        Me.ReasonCodesTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ReasonCodesTextBox.Location = New System.Drawing.Point(88, 8)
        Me.ReasonCodesTextBox.Name = "ReasonCodesTextBox"
        Me.ReasonCodesTextBox.Size = New System.Drawing.Size(304, 20)
        Me.ReasonCodesTextBox.TabIndex = 0
        Me.ReasonCodesTextBox.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Reason Code:"
        Me.Label1.Visible = False
        '
        'SortDownButton
        '
        Me.SortDownButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SortDownButton.Enabled = False
        Me.SortDownButton.Image = CType(resources.GetObject("SortDownButton.Image"), System.Drawing.Image)
        Me.SortDownButton.Location = New System.Drawing.Point(484, 136)
        Me.SortDownButton.Name = "SortDownButton"
        Me.SortDownButton.Size = New System.Drawing.Size(32, 30)
        Me.SortDownButton.TabIndex = 5
        Me.MainToolTip.SetToolTip(Me.SortDownButton, "Sort Item(s) Down")
        '
        'SortUpButton
        '
        Me.SortUpButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SortUpButton.Enabled = False
        Me.SortUpButton.Image = CType(resources.GetObject("SortUpButton.Image"), System.Drawing.Image)
        Me.SortUpButton.Location = New System.Drawing.Point(484, 96)
        Me.SortUpButton.Name = "SortUpButton"
        Me.SortUpButton.Size = New System.Drawing.Size(32, 30)
        Me.SortUpButton.TabIndex = 4
        Me.MainToolTip.SetToolTip(Me.SortUpButton, "Sort Item(s) Up")
        '
        'DeleteButton
        '
        Me.DeleteButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DeleteButton.Enabled = False
        Me.DeleteButton.Image = CType(resources.GetObject("DeleteButton.Image"), System.Drawing.Image)
        Me.DeleteButton.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.DeleteButton.Location = New System.Drawing.Point(484, 40)
        Me.DeleteButton.Name = "DeleteButton"
        Me.DeleteButton.Size = New System.Drawing.Size(32, 30)
        Me.DeleteButton.TabIndex = 3
        Me.MainToolTip.SetToolTip(Me.DeleteButton, "Remove Reason Code(s)")
        Me.DeleteButton.Visible = False
        '
        'ReasonLookupButton
        '
        Me.ReasonLookupButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ReasonLookupButton.Location = New System.Drawing.Point(392, 8)
        Me.ReasonLookupButton.Name = "ReasonLookupButton"
        Me.ReasonLookupButton.Size = New System.Drawing.Size(32, 23)
        Me.ReasonLookupButton.TabIndex = 10
        Me.ReasonLookupButton.TabStop = False
        Me.ReasonLookupButton.Text = "?"
        Me.MainToolTip.SetToolTip(Me.ReasonLookupButton, "Find Valid Reason Codes")
        Me.ReasonLookupButton.Visible = False
        '
        'LineReasonsForm
        '
        Me.AcceptButton = Me.SaveButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.CancelActionButton
        Me.ClientSize = New System.Drawing.Size(520, 294)
        Me.Controls.Add(Me.LineReasonsDataGrid)
        Me.Controls.Add(Me.ReasonLookupButton)
        Me.Controls.Add(Me.DeleteButton)
        Me.Controls.Add(Me.SortDownButton)
        Me.Controls.Add(Me.SortUpButton)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ReasonCodesTextBox)
        Me.Controls.Add(Me.AddButton)
        Me.Controls.Add(Me.CancelActionButton)
        Me.Controls.Add(Me.SaveButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(440, 224)
        Me.Name = "LineReasonsForm"
        Me.ShowInTaskbar = False
        Me.Text = "Line 0 Reasons"
        CType(Me.LineReasonsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region "Constructors"
    Public Sub New(ByVal claimID As Integer, ByVal lineNumber As Short, ByVal reasonDT As ClaimDataset.REASONDataTable, ByVal [ReadOnly] As Boolean)
        Me.New(claimID, lineNumber, reasonDT)

        Me.ReadOnly = [ReadOnly]
    End Sub
#End Region

#Region "Public Properties"

    <System.ComponentModel.Description("Gets the ClaimDataset.")>
    Public ReadOnly Property ClaimDataset() As ClaimDataset
        Get
            Return _ClaimDS
        End Get
    End Property

    <System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)

            _APPKEY = value
        End Set
    End Property

    Public Property [ReadOnly]() As Boolean
        Get
            Return _ReadOnly
        End Get
        Set(ByVal value As Boolean)
            _ReadOnly = value

            If _ReadOnly Then

                LineReasonsDataGrid.Left = 4
                LineReasonsDataGrid.Top = 4
                LineReasonsDataGrid.Height = CancelActionButton.Top - 8
                LineReasonsDataGrid.Width = Me.Width - 16

                Me.AcceptButton = CancelActionButton
                ReasonCodesTextBox.Enabled = False
                ReasonCodesTextBox.TabStop = False
                ReasonCodesTextBox.Visible = False
                ReasonLookupButton.Enabled = False
                ReasonLookupButton.TabStop = False
                ReasonLookupButton.Visible = False
                AddButton.Enabled = False
                AddButton.TabStop = False
                AddButton.Visible = False
                DeleteButton.Enabled = False
                DeleteButton.TabStop = False
                DeleteButton.Visible = False
                SortUpButton.Enabled = False
                SortUpButton.TabStop = False
                SortUpButton.Visible = False
                SortDownButton.Enabled = False
                SortDownButton.TabStop = False
                SortDownButton.Visible = False
                SaveButton.Enabled = False
                SaveButton.TabStop = False
                SaveButton.Visible = False

                CancelActionButton.TabStop = False
            Else

                Me.AcceptButton = SaveButton
                ReasonCodesTextBox.Enabled = True
                ReasonCodesTextBox.TabStop = True
                ReasonCodesTextBox.Visible = True
                ReasonLookupButton.Enabled = True
                ReasonLookupButton.TabStop = True
                ReasonLookupButton.Visible = True
                AddButton.Enabled = True
                AddButton.TabStop = True
                AddButton.Visible = True
                DeleteButton.Enabled = True
                DeleteButton.TabStop = True
                DeleteButton.Visible = True
                SortUpButton.Enabled = True
                SortUpButton.TabStop = True
                SortUpButton.Visible = True
                SortDownButton.Enabled = True
                SortDownButton.TabStop = True
                SortDownButton.Visible = True
                SaveButton.Enabled = True
                SaveButton.TabStop = True
                SaveButton.Visible = True

                CancelActionButton.TabStop = True
            End If
        End Set
    End Property

    Public ReadOnly Property SelectedReasons As ClaimDataset.REASONDataTable
        Get
            Return _SelectedReasons
        End Get
    End Property
#End Region

#Region "Form Events"
    Private Sub DetailLineReasons_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not UFCWGeneral.SetFormPosition(Me, _APPKEY) Then Me.CenterToScreen()

            Me.Text = If(_ClaimID = -1, "Search for", "Claim [" & _ClaimID & "] Line " & _LineNumber) & " Reason(s)"

            LoadReasons()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DetailLineReasons_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        UFCWGeneral.SaveFormPosition(Me, _APPKEY)

        LineReasonsDataGrid.TableStyles.Clear()
        LineReasonsDataGrid.DataSource = Nothing
        LineReasonsDataGrid.Dispose()

        If _ClaimDS IsNot Nothing Then
            _ClaimDS.Dispose()
        End If
        _ClaimDS = Nothing
    End Sub

#End Region

    Private Sub LoadReasons()

        Try

            SetReasonsTableStyle()

            LineReasonsDataGrid.DataSource = _ClaimDS.REASON

            LineReasonsDataGrid.Sort = "PRIORITY"

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SetReasonsTableStyle()

        Dim DGTableStyle As DataGridTableStyle
        Dim TextCol As DataGridHighlightTextBoxColumn
        Dim CurMan As CurrencyManager

        Try
            CurMan = CType(Me.BindingContext(_ClaimDS.REASON), CurrencyManager)

            DGTableStyle = New DataGridTableStyle(CurMan)
            DGTableStyle.MappingName = _ClaimDS.REASON.TableName
            DGTableStyle.GridColumnStyles.Clear()
            DGTableStyle.GridLineStyle = DataGridLineStyle.None

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "REASON"
            TextCol.HeaderText = "Code"
            TextCol.Width = CInt(GetSetting(_APPKEY, "MedicalWork\REASONS\ColumnSettings", "Col " & TextCol.MappingName, CStr(100)))
            TextCol.NullText = ""
            'coltxtbx = TextCol.TextBox
            'AddHandler coltxtbx.KeyUp, AddressOf GridKeyDownEvent
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "DESCRIPTION"
            TextCol.HeaderText = "Description"
            TextCol.Width = CInt(GetSetting(_APPKEY, "MedicalWork\REASONS\ColumnSettings", "Col " & TextCol.MappingName, CStr(220)))
            TextCol.NullText = ""
            'coltxtbx = TextCol.TextBox
            'AddHandler coltxtbx.KeyUp, AddressOf GridKeyDownEvent
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "PRIORITY"
            TextCol.HeaderText = "Priority"
            TextCol.Width = CInt(GetSetting(_APPKEY, "MedicalWork\REASONS\ColumnSettings", "Col " & TextCol.MappingName, CStr(59)))
            TextCol.NullText = ""
            'coltxtbx = TextCol.TextBox
            'AddHandler coltxtbx.KeyUp, AddressOf GridKeyDownEvent
            DGTableStyle.GridColumnStyles.Add(TextCol)

        Catch ex As Exception
            Throw
        End Try

        Try
            LineReasonsDataGrid.TableStyles.Clear()
            LineReasonsDataGrid.TableStyles.Add(DGTableStyle)
        Catch ex As Exception
            Throw
        End Try

        CurMan = Nothing
        DGTableStyle = Nothing
    End Sub

    Private Sub CancelActionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelActionButton.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SaveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveButton.Click
        If ReasonCodesTextBox.Text <> "" Then
            If AddReason() Then
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        Else
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub AddButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddButton.Click

        If ReasonCodesTextBox.Text.Trim <> "" Then AddReason()

    End Sub

    Private Sub DeleteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteButton.Click
        Try
            Delete()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LineReasonsDataGrid_OnDelete(ByRef Cancel As Boolean) Handles LineReasonsDataGrid.OnDelete
        Try
            Delete()

            'required to stop the grid from performing its own delete (causes an error on last delete)
            Cancel = True
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SortUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SortUpButton.Click
        Try
            SortItemsUp()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SortDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SortDownButton.Click
        Try
            SortItemsDown()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SortItemsUp()
        If _ClaimDS.REASON.Rows.Count < 1 Then Return

        Dim DR As DataRow
        Dim TmpDT As New DataTable("SortOrder")
        Dim GridDV As DataView
        Dim DV As DataView
        Dim ReasonDV As DataView
        Dim OrigFirstR As Integer
        Dim SuccessionCnt As Integer = 1
        Dim LastSel As Boolean = False

        Try
            Using WC As New GlobalCursor

                GridDV = LineReasonsDataGrid.GetDefaultDataView
                OrigFirstR = CInt(LineReasonsDataGrid.FirstVisibleRow)

                TmpDT.Columns.Add("Reason")
                TmpDT.Columns.Add("OrigPriority", System.Type.GetType("System.Int32"))
                TmpDT.Columns.Add("Priority", System.Type.GetType("System.Int32"))
                TmpDT.Columns.Add("Selected")

                For Cnt As Integer = 0 To GridDV.Count - 1
                    DR = TmpDT.NewRow

                    DR("Reason") = GridDV(Cnt)("REASON")
                    DR("OrigPriority") = Cnt
                    DR("Priority") = Cnt
                    DR("Selected") = LineReasonsDataGrid.IsSelected(Cnt)

                    TmpDT.Rows.Add(DR)
                Next

                For Cnt As Integer = 0 To _ClaimDS.REASON.Rows.Count - 1
                    If LineReasonsDataGrid.IsSelected(Cnt) Then
                        TmpDT.Rows(Cnt)("Priority") = CInt(TmpDT.Rows(Cnt)("Priority")) - 1
                    End If
                Next

                For Cnt As Integer = 0 To _ClaimDS.REASON.Rows.Count - 1
                    If LineReasonsDataGrid.IsSelected(Cnt) Then
                        If LastSel Then
                            TmpDT.Rows(Cnt)("Priority") = CInt(TmpDT.Rows(Cnt)("Priority")) - SuccessionCnt
                            TmpDT.Rows(Cnt)("OrigPriority") = CInt(TmpDT.Rows(Cnt)("Priority")) - SuccessionCnt

                            SuccessionCnt += 1
                        End If
                        LastSel = True
                    Else
                        SuccessionCnt = 1
                        LastSel = False
                    End If
                Next

                LineReasonsDataGrid.MoveGridToRow(OrigFirstR)

                ReasonDV = New DataView(_ClaimDS.REASON, "", "REASON", DataViewRowState.CurrentRows)
                DV = New DataView(TmpDT, "", "Priority, OrigPriority DESC", DataViewRowState.CurrentRows)

                For Cnt As Integer = 0 To DV.Count - 1
                    ReasonDV.RowFilter = "REASON = '" & DV(Cnt)("REASON").ToString & "'"

                    ReasonDV(0)("Priority") = Cnt
                Next

                For Cnt As Integer = 0 To DV.Count - 1
                    If DV(Cnt)("Selected").ToString = "True" Then
                        LineReasonsDataGrid.Select(Cnt)

                        If Cnt < LineReasonsDataGrid.FirstVisibleRow + 1 Then
                            LineReasonsDataGrid.MoveGridToRow(CInt(If(Cnt > 0, Cnt - (LineReasonsDataGrid.VisibleRowCount / 3) - 1, Cnt)))
                        End If
                    Else
                        LineReasonsDataGrid.UnSelect(Cnt)
                    End If
                Next

            End Using

        Catch ex As Exception
            Throw
        Finally
            If TmpDT IsNot Nothing Then TmpDT.Dispose()
            TmpDT = Nothing

        End Try
    End Sub

    Private Sub SortItemsDown()
        If _ClaimDS.REASON.Rows.Count < 1 Then Exit Sub

        Dim DR As DataRow
        Dim TmpDT As New DataTable("SortOrder")
        Dim GridDV As DataView = LineReasonsDataGrid.GetDefaultDataView
        Dim DV As DataView
        Dim ReasonDV As DataView
        Dim OrigFirstR As Integer = CInt(LineReasonsDataGrid.FirstVisibleRow)
        Dim FirstRow As Integer
        Dim SuccessionCnt As Integer = 1
        Dim LastSel As Boolean = False

        Try
            Using WC As New GlobalCursor

                TmpDT.Columns.Add("Reason")
                TmpDT.Columns.Add("OrigPriority", System.Type.GetType("System.Int32"))
                TmpDT.Columns.Add("Priority", System.Type.GetType("System.Int32"))
                TmpDT.Columns.Add("Selected")

                For Cnt As Integer = 0 To GridDV.Count - 1
                    DR = TmpDT.NewRow

                    DR("Reason") = GridDV(Cnt)("REASON")
                    DR("OrigPriority") = Cnt
                    DR("Priority") = Cnt
                    DR("Selected") = LineReasonsDataGrid.IsSelected(Cnt)

                    TmpDT.Rows.Add(DR)
                Next

                For Cnt As Integer = 0 To _ClaimDS.REASON.Rows.Count - 1
                    If LineReasonsDataGrid.IsSelected(Cnt) Then
                        TmpDT.Rows(Cnt)("Priority") = CInt(TmpDT.Rows(Cnt)("Priority")) + 1
                    End If
                Next

                For Cnt As Integer = _ClaimDS.REASON.Rows.Count - 1 To 0 Step -1
                    If LineReasonsDataGrid.IsSelected(Cnt) Then
                        If LastSel Then
                            TmpDT.Rows(Cnt)("Priority") = CInt(TmpDT.Rows(Cnt)("Priority")) + SuccessionCnt
                            TmpDT.Rows(Cnt)("OrigPriority") = CInt(TmpDT.Rows(Cnt)("Priority")) + SuccessionCnt

                            SuccessionCnt += 1
                        End If
                        LastSel = True
                    Else
                        SuccessionCnt = 1
                        LastSel = False
                    End If
                Next

                LineReasonsDataGrid.MoveGridToRow(OrigFirstR)

                ReasonDV = New DataView(_ClaimDS.REASON, "", "REASON", DataViewRowState.CurrentRows)
                DV = New DataView(TmpDT, "", "Priority, OrigPriority DESC", DataViewRowState.CurrentRows)

                For Cnt As Integer = 0 To DV.Count - 1
                    ReasonDV.RowFilter = "REASON = '" & DV(Cnt)("REASON").ToString & "'"

                    ReasonDV(0)("Priority") = Cnt
                Next

                For Cnt As Integer = 0 To DV.Count - 1
                    If DV(Cnt)("Selected").ToString.ToString = "True" Then
                        LineReasonsDataGrid.Select(Cnt)

                        FirstRow = CInt(LineReasonsDataGrid.FirstVisibleRow)

                        If Cnt < FirstRow OrElse Cnt > (FirstRow + LineReasonsDataGrid.VisibleRowCount - 2) Then
                            LineReasonsDataGrid.MoveGridToRow(CInt(Cnt - (LineReasonsDataGrid.VisibleRowCount / 3)))
                        End If
                    Else
                        LineReasonsDataGrid.UnSelect(Cnt)
                    End If
                Next

            End Using

        Catch ex As Exception
            Throw
        Finally
            If TmpDT IsNot Nothing Then TmpDT.Dispose()
            TmpDT = Nothing
        End Try
    End Sub

    Private Sub ReasonCodesTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReasonCodesTextBox.TextChanged
        If ReasonCodesTextBox.Text.Length = 0 Then
            AddButton.Enabled = False
            'Me.AcceptButton = Nothing
        Else
            If LineReasonsDataGrid.GetGridRowCount < MAXREASONS Then
                AddButton.Enabled = True
                'Me.AcceptButton = AddButton
            Else
                AddButton.Enabled = False
                'Me.AcceptButton = Nothing
            End If
        End If
    End Sub

    Private Sub LineReasonsDataGrid_RowCountChanged(ByVal previousRowCount As Integer?, ByVal currentRowCount As Integer) Handles LineReasonsDataGrid.RowCountChanged

        If currentRowCount = 0 Then
            DeleteButton.Enabled = False
            SortUpButton.Enabled = False
            SortDownButton.Enabled = False
        Else
            DeleteButton.Enabled = True
            SortUpButton.Enabled = True
            SortDownButton.Enabled = True
        End If

        If currentRowCount >= MAXREASONS Then
            ReasonCodesTextBox.Enabled = False
            AddButton.Enabled = False
            'Me.AcceptButton = Nothing
        Else
            ReasonCodesTextBox.Enabled = True

            If ReasonCodesTextBox.Text.Length > 0 Then
                AddButton.Enabled = True
                'Me.AcceptButton = AddButton
            End If
        End If
    End Sub

    Private Sub ReasonCodesTextBox_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ReasonCodesTextBox.EnabledChanged
        ReasonLookupButton.Enabled = ReasonCodesTextBox.Enabled
    End Sub

    Private Function AddReason() As Boolean
        Dim Source As String
        Dim Reasons() As String
        Dim DT As DataTable
        Dim DV As DataView
        Dim DR As DataRow
        Dim Reason As String
        Dim ReasonDR As DataRow
        Dim ReasonDesc As String
        Dim ReasonActiveAsOf As Date? = Now

        Try
            Source = ReasonCodesTextBox.Text
            Source = Source.Replace(" ", "")

            Reasons = Source.ToUpper.Split(CChar(","))

            DT = New DataTable("Reason")
            DT.Columns.Add("Name")
            DT.Columns.Add("Desc")
            DT.Columns.Add("Priority", System.Type.GetType("System.Int32"))
            DT.Columns.Add("Print_SW", System.Type.GetType("System.Boolean"))
            DT.Columns.Add("APPLY_STATUS")

            If Reasons.Length + LineReasonsDataGrid.GetGridRowCount > MAXREASONS Then
                ReasonCodesTextBox.SelectionStart = 0
                ReasonCodesTextBox.SelectionLength = ReasonCodesTextBox.Text.Length

                MessageBox.Show("Only " & MAXREASONS & " Reasons Are Allowed.  Please Select The Best Match", "Too Many Reasons", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ReasonCodesTextBox.Focus()

                Exit Try
            End If

            For Cnt As Integer = 0 To Reasons.Length - 1
                Reason = Reasons(Cnt)
                If Reason.Length < 3 Then
                    If IsNumeric(Reason) Then
                        Reason = Reason.PadLeft(3, CChar("0"))
                    End If
                End If

                Dim QueryMEDDTL =
                    From MEDDTL In _ClaimDS.Tables("MEDDTL").AsEnumerable()
                    Where MEDDTL.RowState <> DataRowState.Deleted _
                    AndAlso MEDDTL.Field(Of Short)("LINE_NBR") = _LineNumber
                    Select MEDDTL

                If QueryMEDDTL.Any Then
                    DV = QueryMEDDTL.AsDataView
                    ReasonActiveAsOf = CType(DV(0)("OCC_FROM_DATE"), Date?)
                End If

                ReasonDR = CMSDALFDBMD.RetrieveReasonValuesInformation(Reason, ReasonActiveAsOf)

                If ReasonDR Is Nothing Then
                    ReasonCodesTextBox.SelectionStart = InStr(ReasonCodesTextBox.Text.ToUpper, Reasons(Cnt)) - 1
                    ReasonCodesTextBox.SelectionLength = Reasons(Cnt).Length

                    MessageBox.Show("Reason " & """" & Reasons(Cnt) & """" & " is not a valid reason code.", "Invalid Reason", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    ReasonCodesTextBox.Focus()

                    Exit Try
                Else
                    ReasonDesc = ReasonDR("DESCRIPTION").ToString

                    DV = New DataView(DT, "Name = '" & Reason & "'", "Name", DataViewRowState.CurrentRows)

                    If DV.Count = 0 Then
                        DV = New DataView(_ClaimDS.REASON, "REASON = '" & Reason & "'", "REASON", DataViewRowState.CurrentRows)

                        If DV.Count = 0 Then
                            DR = DT.NewRow

                            DR("Name") = Reason
                            DR("Desc") = ReasonDesc
                            DR("Priority") = _ClaimDS.REASON.Rows.Count + DT.Rows.Count
                            DR("Print_SW") = ReasonDR("PRINT_SW")
                            DR("APPLY_STATUS") = ReasonDR("APPLY_STATUS")

                            DT.Rows.Add(DR)
                        End If
                    End If
                End If
            Next

            For Cnt As Integer = 0 To DT.Rows.Count - 1
                DR = _ClaimDS.REASON.NewRow

                DR("CLAIM_ID") = _ClaimID
                DR("LINE_NBR") = _LineNumber
                DR("REASON") = DT.Rows(Cnt)("Name")
                DR("DESCRIPTION") = DT.Rows(Cnt)("Desc")
                DR("PRIORITY") = DT.Rows(Cnt)("Priority")
                DR("PRINT_SW") = DT.Rows(Cnt)("Print_SW")
                DR("APPLY_STATUS") = DT.Rows(Cnt)("APPLY_STATUS")

                _ClaimDS.REASON.Rows.Add(DR)
            Next

            _SelectedReasons = New ClaimDataset.REASONDataTable

            For Each DR In _ClaimDS.REASON.Rows
                _SelectedReasons.ImportRow(DR)
            Next

            ReasonCodesTextBox.Text = ""

            Return True

        Catch ex As Exception
            Throw

        Finally
            DV = Nothing
            DT = Nothing
        End Try
    End Function

    Private Sub Delete()

        Dim DV As DataView
        Dim GridDV As DataView
        Dim TmpDT As New DataTable("SortOrder")
        Dim DR As DataRow

        Try

            Using WC As New GlobalCursor

                GridDV = LineReasonsDataGrid.GetDefaultDataView

                TmpDT.Columns.Add("Reason")
                TmpDT.Columns.Add("OrigPriority", System.Type.GetType("System.Int32"))
                TmpDT.Columns.Add("Priority", System.Type.GetType("System.Int32"))
                TmpDT.Columns.Add("Selected")

                For Cnt As Integer = 0 To GridDV.Count - 1
                    DR = TmpDT.NewRow

                    DR("Reason") = GridDV(Cnt)("REASON")
                    DR("OrigPriority") = Cnt
                    DR("Priority") = Cnt
                    DR("Selected") = LineReasonsDataGrid.IsSelected(Cnt)

                    TmpDT.Rows.Add(DR)
                Next

                DV = New DataView(_ClaimDS.REASON, "", "REASON", DataViewRowState.CurrentRows)

                For Cnt As Integer = TmpDT.Rows.Count - 1 To 0 Step -1
                    If TmpDT.Rows(Cnt)("Selected").ToString = "True" Then
                        DV.RowFilter = "REASON = '" & TmpDT.Rows(Cnt)("REASON").ToString & "'"
                        DV(0).Row.Delete()
                    End If
                Next

                'Re-Prioritize
                For Cnt As Integer = 0 To GridDV.Count - 1
                    GridDV(Cnt).Row.Item("PRIORITY") = Cnt
                Next

            End Using

        Catch ex As Exception
            Throw
        Finally
            If TmpDT IsNot Nothing Then TmpDT.Dispose()
            TmpDT = Nothing
        End Try
    End Sub

    Private Sub ReasonLookupButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReasonLookupButton.Click
        Dim Frm As New ReasonLookup
        Dim Codes As String = ""

        Try
            If Frm.ShowDialog(Me) = DialogResult.OK Then
                For Cnt As Integer = 0 To Frm.ReasonsDataGrid.GetGridRowCount - 1
                    If Frm.ReasonsDataGrid.IsSelected(Cnt) Then
                        Codes &= If(Codes <> "", ", ", "") & Frm.ReasonsDataGrid.Item(Cnt, CInt(Frm.ReasonsDataGrid.GetColumnPosition("REASON"))).ToString
                    End If
                Next

                If Codes <> "" Then
                    If ReasonCodesTextBox.Text = "" Then
                        ReasonCodesTextBox.Text = Codes
                    Else
                        ReasonCodesTextBox.Text &= ", " & Codes
                    End If
                End If
            End If
        Catch ex As Exception
            Throw
        Finally
            ReasonCodesTextBox.Focus()
        End Try
    End Sub

End Class