Option Strict On
Option Infer On

Imports System.Configuration

Public Class ProcedureCodesForm
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"

    Private WithEvents _ProcedureCodesDT As DataTable
    Private WithEvents _ProcedureCodesBS As BindingSource

    Private _ReadOnly As Boolean

    Private _MAXPROCEDURES As Integer = CInt(ConfigurationManager.AppSettings("MAXPROCEDURES"))

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal procedureCodesDT As DataTable, ByVal [readOnly] As Boolean)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Me.ReadOnly = [readOnly]

        _ProcedureCodesDT = procedureCodesDT.Clone
        For Each DR As DataRow In procedureCodesDT.Rows
            _ProcedureCodesDT.ImportRow(DR)
        Next

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then

                If _ProcedureCodesDT IsNot Nothing Then
                    _ProcedureCodesDT.Dispose()
                End If
                _ProcedureCodesDT = Nothing

                components.Dispose()
            End If
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
    Friend WithEvents ProcedureCodesDataGrid As DataGridCustom
    Friend WithEvents DeleteButton As System.Windows.Forms.Button
    Friend WithEvents MainToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents txtProcedures As System.Windows.Forms.TextBox
    Friend WithEvents ProcedureCodeLookupButton As System.Windows.Forms.Button

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ProcedureCodesForm))
        Me.AddButton = New System.Windows.Forms.Button()
        Me.CancelActionButton = New System.Windows.Forms.Button()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.ProcedureCodesDataGrid = New DataGridCustom()
        Me.txtProcedures = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SortDownButton = New System.Windows.Forms.Button()
        Me.SortUpButton = New System.Windows.Forms.Button()
        Me.DeleteButton = New System.Windows.Forms.Button()
        Me.MainToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.ProcedureCodeLookupButton = New System.Windows.Forms.Button()
        CType(Me.ProcedureCodesDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
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
        'ProcedureCodesDataGrid
        '
        Me.ProcedureCodesDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.ProcedureCodesDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.ProcedureCodesDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ProcedureCodesDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ProcedureCodesDataGrid.ADGroupsThatCanFind = ""
        Me.ProcedureCodesDataGrid.ADGroupsThatCanMultiSort = ""
        Me.ProcedureCodesDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ProcedureCodesDataGrid.AllowAutoSize = True
        Me.ProcedureCodesDataGrid.AllowColumnReorder = False
        Me.ProcedureCodesDataGrid.AllowCopy = True
        Me.ProcedureCodesDataGrid.AllowCustomize = True
        Me.ProcedureCodesDataGrid.AllowDelete = False
        Me.ProcedureCodesDataGrid.AllowDragDrop = False
        Me.ProcedureCodesDataGrid.AllowEdit = False
        Me.ProcedureCodesDataGrid.AllowExport = False
        Me.ProcedureCodesDataGrid.AllowFilter = True
        Me.ProcedureCodesDataGrid.AllowFind = True
        Me.ProcedureCodesDataGrid.AllowGoTo = True
        Me.ProcedureCodesDataGrid.AllowMultiSelect = True
        Me.ProcedureCodesDataGrid.AllowMultiSort = False
        Me.ProcedureCodesDataGrid.AllowNew = False
        Me.ProcedureCodesDataGrid.AllowPrint = False
        Me.ProcedureCodesDataGrid.AllowRefresh = False
        Me.ProcedureCodesDataGrid.AllowSorting = False
        Me.ProcedureCodesDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProcedureCodesDataGrid.AppKey = "UFCW\Claims\"
        Me.ProcedureCodesDataGrid.AutoSaveCols = True
        Me.ProcedureCodesDataGrid.BackgroundColor = System.Drawing.SystemColors.Control
        Me.ProcedureCodesDataGrid.CaptionVisible = False
        Me.ProcedureCodesDataGrid.ColumnHeaderLabel = Nothing
        Me.ProcedureCodesDataGrid.ColumnRePositioning = False
        Me.ProcedureCodesDataGrid.ColumnResizing = False
        Me.ProcedureCodesDataGrid.ConfirmDelete = False
        Me.ProcedureCodesDataGrid.CopySelectedOnly = True
        Me.ProcedureCodesDataGrid.CurrentBSPosition = -1
        Me.ProcedureCodesDataGrid.DataMember = ""
        Me.ProcedureCodesDataGrid.DragColumn = 0
        Me.ProcedureCodesDataGrid.ExportSelectedOnly = True
        Me.ProcedureCodesDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ProcedureCodesDataGrid.HighlightedRow = Nothing
        Me.ProcedureCodesDataGrid.HighLightModifiedRows = False
        Me.ProcedureCodesDataGrid.IsMouseDown = False
        Me.ProcedureCodesDataGrid.LastGoToLine = ""
        Me.ProcedureCodesDataGrid.Location = New System.Drawing.Point(8, 40)
        Me.ProcedureCodesDataGrid.MultiSort = False
        Me.ProcedureCodesDataGrid.Name = "ProcedureCodesDataGrid"
        Me.ProcedureCodesDataGrid.OldSelectedRow = Nothing
        Me.ProcedureCodesDataGrid.PreviousBSPosition = -1
        Me.ProcedureCodesDataGrid.ReadOnly = True
        Me.ProcedureCodesDataGrid.RetainRowSelectionAfterSort = True
        Me.ProcedureCodesDataGrid.SetRowOnRightClick = True
        Me.ProcedureCodesDataGrid.ShiftPressed = False
        Me.ProcedureCodesDataGrid.SingleClickBooleanColumns = True
        Me.ProcedureCodesDataGrid.Size = New System.Drawing.Size(472, 224)
        Me.ProcedureCodesDataGrid.Sort = Nothing
        Me.ProcedureCodesDataGrid.StyleName = ""
        Me.ProcedureCodesDataGrid.SubKey = ""
        Me.ProcedureCodesDataGrid.SuppressMouseDown = False
        Me.ProcedureCodesDataGrid.SuppressTriangle = False
        Me.ProcedureCodesDataGrid.TabIndex = 2
        '
        'txtProcedures
        '
        Me.txtProcedures.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtProcedures.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtProcedures.Location = New System.Drawing.Point(97, 9)
        Me.txtProcedures.Name = "txtProcedures"
        Me.txtProcedures.Size = New System.Drawing.Size(304, 20)
        Me.txtProcedures.TabIndex = 0
        Me.txtProcedures.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(87, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Procedure Code:"
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
        'ProcedureCodeLookupButton
        '
        Me.ProcedureCodeLookupButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProcedureCodeLookupButton.Location = New System.Drawing.Point(401, 8)
        Me.ProcedureCodeLookupButton.Name = "ProcedureCodeLookupButton"
        Me.ProcedureCodeLookupButton.Size = New System.Drawing.Size(32, 23)
        Me.ProcedureCodeLookupButton.TabIndex = 10
        Me.ProcedureCodeLookupButton.TabStop = False
        Me.ProcedureCodeLookupButton.Text = "?"
        Me.MainToolTip.SetToolTip(Me.ProcedureCodeLookupButton, "Find Valid Procedure Codes")
        Me.ProcedureCodeLookupButton.Visible = False
        '
        'ProcedureCodesForm
        '
        Me.AcceptButton = Me.SaveButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.CancelActionButton
        Me.ClientSize = New System.Drawing.Size(520, 294)
        Me.Controls.Add(Me.ProcedureCodesDataGrid)
        Me.Controls.Add(Me.ProcedureCodeLookupButton)
        Me.Controls.Add(Me.DeleteButton)
        Me.Controls.Add(Me.SortDownButton)
        Me.Controls.Add(Me.SortUpButton)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtProcedures)
        Me.Controls.Add(Me.AddButton)
        Me.Controls.Add(Me.CancelActionButton)
        Me.Controls.Add(Me.SaveButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(440, 224)
        Me.Name = "ProcedureCodesForm"
        Me.ShowInTaskbar = False
        Me.Text = "Procedure Code(s)"
        CType(Me.ProcedureCodesDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region "Public Properties"

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

                ProcedureCodesDataGrid.Left = 4
                ProcedureCodesDataGrid.Top = 4
                ProcedureCodesDataGrid.Height = CancelActionButton.Top - 8
                ProcedureCodesDataGrid.Width = Me.Width - 16

                Me.AcceptButton = CancelActionButton
                txtProcedures.Enabled = False
                txtProcedures.TabStop = False
                txtProcedures.Visible = False
                ProcedureCodeLookupButton.Enabled = False
                ProcedureCodeLookupButton.TabStop = False
                ProcedureCodeLookupButton.Visible = False
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
                txtProcedures.Enabled = True
                txtProcedures.TabStop = True
                txtProcedures.Visible = True
                ProcedureCodeLookupButton.Enabled = True
                ProcedureCodeLookupButton.TabStop = True
                ProcedureCodeLookupButton.Visible = True
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

    Public ReadOnly Property SelectedProcedures As DataTable
        Get
            Return _ProcedureCodesDT
        End Get
    End Property
#End Region

#Region "Form Events"
    Private Sub ProcedureCodesForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not UFCWGeneral.SetFormPosition(Me, _APPKEY) Then Me.CenterToScreen()

            Me.Text = "Search for Procedure(s)"

            LoadProcedureCodes()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ProcedureCodesForm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        UFCWGeneral.SaveFormPosition(Me, _APPKEY)

        ProcedureCodesDataGrid.TableStyles.Clear()
        ProcedureCodesDataGrid.DataSource = Nothing
        ProcedureCodesDataGrid.Dispose()

    End Sub

#End Region

    Private Sub LoadProcedureCodes()

        Try

            _ProcedureCodesBS = New BindingSource
            _ProcedureCodesBS.DataSource = _ProcedureCodesDT

            ProcedureCodesDataGrid.DataSource = _ProcedureCodesBS
            ProcedureCodesDataGrid.SetTableStyle()

            ProcedureCodesDataGrid.Sort = "PRIORITY"

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub CancelActionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelActionButton.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SaveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveButton.Click

        If txtProcedures.Text <> "" Then
            If AddProcedureCode() Then
                Me.DialogResult = DialogResult.OK
            End If
        Else
            Me.DialogResult = DialogResult.OK
        End If
    End Sub

    Private Sub AddButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddButton.Click

        If txtProcedures.Text.Trim <> "" Then AddProcedureCode()

    End Sub

    Private Sub DeleteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteButton.Click
        Try
            Delete()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ProcedureCodesDataGrid_OnDelete(ByRef Cancel As Boolean) Handles ProcedureCodesDataGrid.OnDelete
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

        If _ProcedureCodesDT.Rows.Count < 1 Then Return

        Dim DR As DataRow
        Dim TmpDT As New DataTable("SortOrder")
        Dim GridDV As DataView
        Dim DV As DataView
        Dim ProcedureCodesDV As DataView
        Dim OrigFirstR As Integer
        Dim SuccessionCnt As Integer = 1
        Dim LastSel As Boolean = False

        Try
            Using WC As New GlobalCursor

                GridDV = ProcedureCodesDataGrid.GetDefaultDataView
                OrigFirstR = CInt(ProcedureCodesDataGrid.FirstVisibleRow)

                TmpDT.Columns.Add("PROC_VALUE")
                TmpDT.Columns.Add("OrigPriority", System.Type.GetType("System.Int32"))
                TmpDT.Columns.Add("Priority", System.Type.GetType("System.Int32"))
                TmpDT.Columns.Add("Selected")

                For Cnt As Integer = 0 To GridDV.Count - 1
                    DR = TmpDT.NewRow

                    DR("PROC_VALUE") = GridDV(Cnt)("PROC_VALUE")
                    DR("OrigPriority") = Cnt
                    DR("Priority") = Cnt
                    DR("Selected") = ProcedureCodesDataGrid.IsSelected(Cnt)

                    TmpDT.Rows.Add(DR)
                Next

                For Cnt As Integer = 0 To _ProcedureCodesDT.Rows.Count - 1
                    If ProcedureCodesDataGrid.IsSelected(Cnt) Then
                        TmpDT.Rows(Cnt)("Priority") = CInt(TmpDT.Rows(Cnt)("Priority")) - 1
                    End If
                Next

                For Cnt As Integer = 0 To _ProcedureCodesDT.Rows.Count - 1
                    If ProcedureCodesDataGrid.IsSelected(Cnt) Then
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

                ProcedureCodesDataGrid.MoveGridToRow(OrigFirstR)

                ProcedureCodesDV = New DataView(_ProcedureCodesDT, "", "PROC_VALUE", DataViewRowState.CurrentRows)
                DV = New DataView(TmpDT, "", "Priority, OrigPriority DESC", DataViewRowState.CurrentRows)

                For Cnt As Integer = 0 To DV.Count - 1
                    ProcedureCodesDV.RowFilter = "PROC_VALUE = '" & DV(Cnt)("PROC_VALUE").ToString & "'"

                    ProcedureCodesDV(0)("Priority") = Cnt
                Next

                For Cnt As Integer = 0 To DV.Count - 1
                    If DV(Cnt)("Selected").ToString = "True" Then
                        ProcedureCodesDataGrid.Select(Cnt)

                        If Cnt < ProcedureCodesDataGrid.FirstVisibleRow + 1 Then
                            ProcedureCodesDataGrid.MoveGridToRow(CInt(If(Cnt > 0, Cnt - (ProcedureCodesDataGrid.VisibleRowCount / 3) - 1, Cnt)))
                        End If
                    Else
                        ProcedureCodesDataGrid.UnSelect(Cnt)
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
        If _ProcedureCodesDT.Rows.Count < 1 Then Exit Sub

        Dim DR As DataRow
        Dim TmpDT As New DataTable("SortOrder")
        Dim GridDV As DataView = ProcedureCodesDataGrid.GetDefaultDataView
        Dim DV As DataView
        Dim ReasonDV As DataView
        Dim OrigFirstR As Integer = CInt(ProcedureCodesDataGrid.FirstVisibleRow)
        Dim FirstRow As Integer
        Dim SuccessionCnt As Integer = 1
        Dim LastSel As Boolean = False

        Try
            Using WC As New GlobalCursor

                TmpDT.Columns.Add("PROC_VALUE")
                TmpDT.Columns.Add("OrigPriority", System.Type.GetType("System.Int32"))
                TmpDT.Columns.Add("Priority", System.Type.GetType("System.Int32"))
                TmpDT.Columns.Add("Selected")

                For Cnt As Integer = 0 To GridDV.Count - 1
                    DR = TmpDT.NewRow

                    DR("PROC_VALUE") = GridDV(Cnt)("PROC_VALUE")
                    DR("OrigPriority") = Cnt
                    DR("Priority") = Cnt
                    DR("Selected") = ProcedureCodesDataGrid.IsSelected(Cnt)

                    TmpDT.Rows.Add(DR)
                Next

                For Cnt As Integer = 0 To _ProcedureCodesDT.Rows.Count - 1
                    If ProcedureCodesDataGrid.IsSelected(Cnt) Then
                        TmpDT.Rows(Cnt)("Priority") = CInt(TmpDT.Rows(Cnt)("Priority")) + 1
                    End If
                Next

                For Cnt As Integer = _ProcedureCodesDT.Rows.Count - 1 To 0 Step -1
                    If ProcedureCodesDataGrid.IsSelected(Cnt) Then
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

                ProcedureCodesDataGrid.MoveGridToRow(OrigFirstR)

                ReasonDV = New DataView(_ProcedureCodesDT, "", "PROC_VALUE", DataViewRowState.CurrentRows)
                DV = New DataView(TmpDT, "", "Priority, OrigPriority DESC", DataViewRowState.CurrentRows)

                For Cnt As Integer = 0 To DV.Count - 1
                    ReasonDV.RowFilter = "PROC_VALUE = '" & DV(Cnt)("PROC_VALUE").ToString & "'"

                    ReasonDV(0)("Priority") = Cnt
                Next

                For Cnt As Integer = 0 To DV.Count - 1
                    If DV(Cnt)("Selected").ToString.ToString = "True" Then
                        ProcedureCodesDataGrid.Select(Cnt)

                        FirstRow = CInt(ProcedureCodesDataGrid.FirstVisibleRow)

                        If Cnt < FirstRow OrElse Cnt > (FirstRow + ProcedureCodesDataGrid.VisibleRowCount - 2) Then
                            ProcedureCodesDataGrid.MoveGridToRow(CInt(Cnt - (ProcedureCodesDataGrid.VisibleRowCount / 3)))
                        End If
                    Else
                        ProcedureCodesDataGrid.UnSelect(Cnt)
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

    Private Sub ProcedureCodesTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtProcedures.TextChanged
        If txtProcedures.Text.Length = 0 Then
            AddButton.Enabled = False
            'Me.AcceptButton = Nothing
        Else
            If ProcedureCodesDataGrid.GetGridRowCount < _MAXPROCEDURES Then
                AddButton.Enabled = True
                'Me.AcceptButton = AddButton
            Else
                AddButton.Enabled = False
                'Me.AcceptButton = Nothing
            End If
        End If
    End Sub

    Private Sub ProcedureCodesDataGrid_RowCountChanged(ByVal previousRowCount As Integer?, ByVal currentRowCount As Integer) Handles ProcedureCodesDataGrid.RowCountChanged

        If currentRowCount = 0 Then
            DeleteButton.Enabled = False
            SortUpButton.Enabled = False
            SortDownButton.Enabled = False
        Else
            DeleteButton.Enabled = True
            SortUpButton.Enabled = True
            SortDownButton.Enabled = True
        End If

        If currentRowCount >= _MAXPROCEDURES Then
            txtProcedures.Enabled = False
            AddButton.Enabled = False
            'Me.AcceptButton = Nothing
        Else
            txtProcedures.Enabled = True

            If txtProcedures.Text.Length > 0 Then
                AddButton.Enabled = True
                'Me.AcceptButton = AddButton
            End If
        End If
    End Sub

    Private Sub ProcedureCodesTextBox_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtProcedures.EnabledChanged
        ProcedureCodeLookupButton.Enabled = txtProcedures.Enabled
    End Sub

    Private Function AddProcedureCode() As Boolean

        Dim ProcedureDR As DataRow
        Dim ProcedureCodesDT As DataTable

        Try

            ProcedureCodesDT = _ProcedureCodesDT.Clone

            Dim NoCommaText As String = txtProcedures.Text.ToUpper.Replace(",", " ")
            Dim ProceduresAL As String() = NoCommaText.Split(New Char() {CChar(" ")}, StringSplitOptions.RemoveEmptyEntries)
            Dim Priority As Short = 0

            If (ProceduresAL.Length + _ProcedureCodesDT.Rows.Count) > _MAXPROCEDURES Then
                txtProcedures.SelectionStart = 0
                txtProcedures.SelectionLength = txtProcedures.Text.Length

                MessageBox.Show("Only " & _MAXPROCEDURES & " Procedures Are Allowed.  Please Select The Best Match", "Too Many Procedures", MessageBoxButtons.OK, MessageBoxIcon.Information)

                txtProcedures.Focus()

                Return False
            End If

            Dim Seq As Short = 0
            For Each DR As DataRow In _ProcedureCodesDT.Rows
                Dim NewDR As DataRow = ProcedureCodesDT.NewRow

                NewDR("PROC_VALUE") = DR("PROC_VALUE")
                NewDR("FULL_DESC") = DR("FULL_DESC")
                NewDR("Priority") = Seq

                Seq += 1S

                ProcedureCodesDT.Rows.Add(NewDR)

            Next

            For Each Procedure As String In ProceduresAL

                If Procedure.Length < 3 Then
                    If IsNumeric(Procedure) Then
                        Procedure = Procedure.PadLeft(3, CChar("0"))
                    End If
                End If

                ProcedureDR = CMSDALFDBMD.RetrieveProcedureValueInformation(Procedure)

                If ProcedureDR Is Nothing Then
                    txtProcedures.SelectionStart = InStr(txtProcedures.Text.ToUpper, Procedure) - 1
                    txtProcedures.SelectionLength = Procedure.Length

                    MessageBox.Show("Procedure " & """" & Procedure & """" & " is not a valid Procedure code.", "Invalid Procedure", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    txtProcedures.Focus()

                    Return False
                Else
                    Dim NewDR As DataRow = ProcedureCodesDT.NewRow

                    NewDR("PROC_VALUE") = ProcedureDR("PROC_VALUE")
                    NewDR("FULL_DESC") = ProcedureDR("FULL_DESC")
                    NewDR("Priority") = Seq

                    Seq += 1S

                    ProcedureCodesDT.Rows.Add(NewDR)
                End If
            Next

            ProcedureCodesDT.AcceptChanges()

            txtProcedures.Text = ""

            _ProcedureCodesDT.Clear()

            For Each DR As DataRow In ProcedureCodesDT.Rows
                _ProcedureCodesDT.ImportRow(DR)
            Next

            Return True

        Catch ex As Exception
            Throw

        Finally
            If ProcedureCodesDT IsNot Nothing Then
                ProcedureCodesDT.Dispose()
            End If
            ProcedureCodesDT = Nothing
        End Try

    End Function

    Private Sub Delete()

        Dim DV As DataView
        Dim GridDV As DataView
        Dim TmpDT As New DataTable("SortOrder")
        Dim DR As DataRow

        Try

            Using WC As New GlobalCursor

                GridDV = ProcedureCodesDataGrid.GetDefaultDataView

                TmpDT.Columns.Add("PROC_VALUE")
                TmpDT.Columns.Add("OrigPriority", System.Type.GetType("System.Int32"))
                TmpDT.Columns.Add("Priority", System.Type.GetType("System.Int32"))
                TmpDT.Columns.Add("Selected")

                For Cnt As Integer = 0 To GridDV.Count - 1
                    DR = TmpDT.NewRow

                    DR("PROC_VALUE") = GridDV(Cnt)("PROC_VALUE")
                    DR("OrigPriority") = Cnt
                    DR("Priority") = Cnt
                    DR("Selected") = ProcedureCodesDataGrid.IsSelected(Cnt)

                    TmpDT.Rows.Add(DR)
                Next

                DV = New DataView(_ProcedureCodesDT, "", "PROC_VALUE", DataViewRowState.CurrentRows)

                For Cnt As Integer = TmpDT.Rows.Count - 1 To 0 Step -1
                    If TmpDT.Rows(Cnt)("Selected").ToString = "True" Then
                        DV.RowFilter = "PROC_VALUE = '" & TmpDT.Rows(Cnt)("PROC_VALUE").ToString & "'"
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

    Private Sub ProcedureCodeLookupButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProcedureCodeLookupButton.Click

        Dim Frm As ProcedureCodeLookupForm
        Dim AL As ArrayList

        Try

            Frm = New ProcedureCodeLookupForm

            If Frm.ShowDialog(Me) = DialogResult.OK Then

                AL = Frm.ProcedureCodesLookupDataGrid.GetSelectedDataRows()

                Dim FlattenQuery = String.Join(",", (From SelectedDR In AL).Select(Function(p) CType(p, DataRow)("PROC_VALUE")))

                txtProcedures.Text = FlattenQuery.ToString
            End If

        Catch ex As Exception
            Throw

        Finally
            If Frm IsNot Nothing Then
                Frm.Close()
                Frm.Dispose()
            End If
            Frm = Nothing
        End Try
    End Sub

End Class