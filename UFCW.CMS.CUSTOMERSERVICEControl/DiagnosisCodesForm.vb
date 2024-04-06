Option Strict On
Option Infer On

Imports System.Configuration

Public Class DiagnosisCodesForm
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"

    Private WithEvents _DiagnosisCodesDT As DataTable
    Private WithEvents _DiagnosisCodesBS As BindingSource

    Private _ReadOnly As Boolean

    Private _MAXDIAGNOSIS As Integer = CInt(ConfigurationManager.AppSettings("MAXDIAGNOSIS"))

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal diagnosisCodesDT As DataTable, ByVal [readOnly] As Boolean)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Me.ReadOnly = [readOnly]

        _DiagnosisCodesDT = diagnosisCodesDT.Clone
        For Each DR As DataRow In diagnosisCodesDT.Rows
            _DiagnosisCodesDT.ImportRow(DR)
        Next

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then

                If _DiagnosisCodesDT IsNot Nothing Then
                    _DiagnosisCodesDT.Dispose()
                End If
                _DiagnosisCodesDT = Nothing

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
    Friend WithEvents DiagnosisCodesDataGrid As DataGridCustom
    Friend WithEvents DeleteButton As System.Windows.Forms.Button
    Friend WithEvents MainToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents txtDiagnosisCodes As System.Windows.Forms.TextBox
    Friend WithEvents DiagnosisCodeLookupButton As System.Windows.Forms.Button

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DiagnosisCodesForm))
        Me.AddButton = New System.Windows.Forms.Button()
        Me.CancelActionButton = New System.Windows.Forms.Button()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.DiagnosisCodesDataGrid = New DataGridCustom()
        Me.txtDiagnosisCodes = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SortDownButton = New System.Windows.Forms.Button()
        Me.SortUpButton = New System.Windows.Forms.Button()
        Me.DeleteButton = New System.Windows.Forms.Button()
        Me.MainToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.DiagnosisCodeLookupButton = New System.Windows.Forms.Button()
        CType(Me.DiagnosisCodesDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.MainToolTip.SetToolTip(Me.AddButton, "Add Diagnosis Code")
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
        'DiagnosisCodesDataGrid
        '
        Me.DiagnosisCodesDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.DiagnosisCodesDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.DiagnosisCodesDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DiagnosisCodesDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DiagnosisCodesDataGrid.ADGroupsThatCanFind = ""
        Me.DiagnosisCodesDataGrid.ADGroupsThatCanMultiSort = ""
        Me.DiagnosisCodesDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DiagnosisCodesDataGrid.AllowAutoSize = True
        Me.DiagnosisCodesDataGrid.AllowColumnReorder = False
        Me.DiagnosisCodesDataGrid.AllowCopy = True
        Me.DiagnosisCodesDataGrid.AllowCustomize = True
        Me.DiagnosisCodesDataGrid.AllowDelete = False
        Me.DiagnosisCodesDataGrid.AllowDragDrop = False
        Me.DiagnosisCodesDataGrid.AllowEdit = False
        Me.DiagnosisCodesDataGrid.AllowExport = False
        Me.DiagnosisCodesDataGrid.AllowFilter = True
        Me.DiagnosisCodesDataGrid.AllowFind = True
        Me.DiagnosisCodesDataGrid.AllowGoTo = True
        Me.DiagnosisCodesDataGrid.AllowMultiSelect = True
        Me.DiagnosisCodesDataGrid.AllowMultiSort = False
        Me.DiagnosisCodesDataGrid.AllowNew = False
        Me.DiagnosisCodesDataGrid.AllowPrint = False
        Me.DiagnosisCodesDataGrid.AllowRefresh = False
        Me.DiagnosisCodesDataGrid.AllowSorting = False
        Me.DiagnosisCodesDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DiagnosisCodesDataGrid.AppKey = "UFCW\Claims\"
        Me.DiagnosisCodesDataGrid.AutoSaveCols = True
        Me.DiagnosisCodesDataGrid.BackgroundColor = System.Drawing.SystemColors.Control
        Me.DiagnosisCodesDataGrid.CaptionVisible = False
        Me.DiagnosisCodesDataGrid.ColumnHeaderLabel = Nothing
        Me.DiagnosisCodesDataGrid.ColumnRePositioning = False
        Me.DiagnosisCodesDataGrid.ColumnResizing = False
        Me.DiagnosisCodesDataGrid.ConfirmDelete = False
        Me.DiagnosisCodesDataGrid.CopySelectedOnly = True
        Me.DiagnosisCodesDataGrid.CurrentBSPosition = -1
        Me.DiagnosisCodesDataGrid.DataMember = ""
        Me.DiagnosisCodesDataGrid.DragColumn = 0
        Me.DiagnosisCodesDataGrid.ExportSelectedOnly = True
        Me.DiagnosisCodesDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DiagnosisCodesDataGrid.HighlightedRow = Nothing
        Me.DiagnosisCodesDataGrid.HighLightModifiedRows = False
        Me.DiagnosisCodesDataGrid.IsMouseDown = False
        Me.DiagnosisCodesDataGrid.LastGoToLine = ""
        Me.DiagnosisCodesDataGrid.Location = New System.Drawing.Point(8, 40)
        Me.DiagnosisCodesDataGrid.MultiSort = False
        Me.DiagnosisCodesDataGrid.Name = "DiagnosisCodesDataGrid"
        Me.DiagnosisCodesDataGrid.OldSelectedRow = Nothing
        Me.DiagnosisCodesDataGrid.PreviousBSPosition = -1
        Me.DiagnosisCodesDataGrid.ReadOnly = True
        Me.DiagnosisCodesDataGrid.RetainRowSelectionAfterSort = True
        Me.DiagnosisCodesDataGrid.SetRowOnRightClick = True
        Me.DiagnosisCodesDataGrid.ShiftPressed = False
        Me.DiagnosisCodesDataGrid.SingleClickBooleanColumns = True
        Me.DiagnosisCodesDataGrid.Size = New System.Drawing.Size(472, 224)
        Me.DiagnosisCodesDataGrid.Sort = Nothing
        Me.DiagnosisCodesDataGrid.StyleName = ""
        Me.DiagnosisCodesDataGrid.SubKey = ""
        Me.DiagnosisCodesDataGrid.SuppressMouseDown = False
        Me.DiagnosisCodesDataGrid.SuppressTriangle = False
        Me.DiagnosisCodesDataGrid.TabIndex = 2
        '
        'txtDiagnosisCodes
        '
        Me.txtDiagnosisCodes.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDiagnosisCodes.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDiagnosisCodes.Location = New System.Drawing.Point(97, 9)
        Me.txtDiagnosisCodes.Name = "txtDiagnosisCodes"
        Me.txtDiagnosisCodes.Size = New System.Drawing.Size(304, 20)
        Me.txtDiagnosisCodes.TabIndex = 0
        Me.txtDiagnosisCodes.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(84, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Diagnosis Code:"
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
        'DiagnosisCodeLookupButton
        '
        Me.DiagnosisCodeLookupButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DiagnosisCodeLookupButton.Location = New System.Drawing.Point(401, 8)
        Me.DiagnosisCodeLookupButton.Name = "DiagnosisCodeLookupButton"
        Me.DiagnosisCodeLookupButton.Size = New System.Drawing.Size(32, 23)
        Me.DiagnosisCodeLookupButton.TabIndex = 10
        Me.DiagnosisCodeLookupButton.TabStop = False
        Me.DiagnosisCodeLookupButton.Text = "?"
        Me.MainToolTip.SetToolTip(Me.DiagnosisCodeLookupButton, "Find Valid Diagnoses")
        Me.DiagnosisCodeLookupButton.Visible = False
        '
        'DiagnosisCodesForm
        '
        Me.AcceptButton = Me.SaveButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.CancelActionButton
        Me.ClientSize = New System.Drawing.Size(520, 294)
        Me.Controls.Add(Me.DiagnosisCodesDataGrid)
        Me.Controls.Add(Me.DiagnosisCodeLookupButton)
        Me.Controls.Add(Me.DeleteButton)
        Me.Controls.Add(Me.SortDownButton)
        Me.Controls.Add(Me.SortUpButton)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtDiagnosisCodes)
        Me.Controls.Add(Me.AddButton)
        Me.Controls.Add(Me.CancelActionButton)
        Me.Controls.Add(Me.SaveButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(440, 224)
        Me.Name = "DiagnosisCodesForm"
        Me.ShowInTaskbar = False
        Me.Text = "Diagnosis Code(s)"
        CType(Me.DiagnosisCodesDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
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

                DiagnosisCodesDataGrid.Left = 4
                DiagnosisCodesDataGrid.Top = 4
                DiagnosisCodesDataGrid.Height = CancelActionButton.Top - 8
                DiagnosisCodesDataGrid.Width = Me.Width - 16

                Me.AcceptButton = CancelActionButton
                txtDiagnosisCodes.Enabled = False
                txtDiagnosisCodes.TabStop = False
                txtDiagnosisCodes.Visible = False
                DiagnosisCodeLookupButton.Enabled = False
                DiagnosisCodeLookupButton.TabStop = False
                DiagnosisCodeLookupButton.Visible = False
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
                txtDiagnosisCodes.Enabled = True
                txtDiagnosisCodes.TabStop = True
                txtDiagnosisCodes.Visible = True
                DiagnosisCodeLookupButton.Enabled = True
                DiagnosisCodeLookupButton.TabStop = True
                DiagnosisCodeLookupButton.Visible = True
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

    Public ReadOnly Property SelectedDiagnoses As DataTable
        Get
            Return _DiagnosisCodesDT
        End Get
    End Property
#End Region

#Region "Form Events"
    Private Sub DiagnosisCodesForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not UFCWGeneral.SetFormPosition(Me, _APPKEY) Then Me.CenterToScreen()

            Me.Text = "Search for Diagnoses"

            LoadDiagnosisCodes()

        Catch ex As Exception

	Throw
        End Try
    End Sub

    Private Sub DiagnosisForm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        UFCWGeneral.SaveFormPosition(Me, _APPKEY)

        DiagnosisCodesDataGrid.TableStyles.Clear()
        DiagnosisCodesDataGrid.DataSource = Nothing
        DiagnosisCodesDataGrid.Dispose()

    End Sub

#End Region

    Private Sub LoadDiagnosisCodes()

        Try

            _DiagnosisCodesBS = New BindingSource
            _DiagnosisCodesBS.DataSource = _DiagnosisCodesDT

            DiagnosisCodesDataGrid.DataSource = _DiagnosisCodesBS
            DiagnosisCodesDataGrid.SetTableStyle()

            DiagnosisCodesDataGrid.Sort = "PRIORITY"

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub CancelActionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelActionButton.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SaveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveButton.Click

        If txtDiagnosisCodes.Text <> "" Then
            If AddDiagnosisCode() Then
                Me.DialogResult = DialogResult.OK
            End If
        Else
            Me.DialogResult = DialogResult.OK
        End If
    End Sub

    Private Sub AddButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddButton.Click

        If txtDiagnosisCodes.Text.Trim <> "" Then AddDiagnosisCode()

    End Sub

    Private Sub DeleteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteButton.Click
        Try
            Delete()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DiagnosisCodesDataGrid_OnDelete(ByRef Cancel As Boolean) Handles DiagnosisCodesDataGrid.OnDelete
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

        If _DiagnosisCodesDT.Rows.Count < 1 Then Return

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

                GridDV = DiagnosisCodesDataGrid.GetDefaultDataView
                OrigFirstR = CInt(DiagnosisCodesDataGrid.FirstVisibleRow)

                TmpDT.Columns.Add("DIAG_VALUE")
                TmpDT.Columns.Add("OrigPriority", System.Type.GetType("System.Int32"))
                TmpDT.Columns.Add("Priority", System.Type.GetType("System.Int32"))
                TmpDT.Columns.Add("Selected")

                For Cnt As Integer = 0 To GridDV.Count - 1
                    DR = TmpDT.NewRow

                    DR("DIAG_VALUE") = GridDV(Cnt)("DIAG_VALUE")
                    DR("OrigPriority") = Cnt
                    DR("Priority") = Cnt
                    DR("Selected") = DiagnosisCodesDataGrid.IsSelected(Cnt)

                    TmpDT.Rows.Add(DR)
                Next

                For Cnt As Integer = 0 To _DiagnosisCodesDT.Rows.Count - 1
                    If DiagnosisCodesDataGrid.IsSelected(Cnt) Then
                        TmpDT.Rows(Cnt)("Priority") = CInt(TmpDT.Rows(Cnt)("Priority")) - 1
                    End If
                Next

                For Cnt As Integer = 0 To _DiagnosisCodesDT.Rows.Count - 1
                    If DiagnosisCodesDataGrid.IsSelected(Cnt) Then
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

                DiagnosisCodesDataGrid.MoveGridToRow(OrigFirstR)

                ProcedureCodesDV = New DataView(_DiagnosisCodesDT, "", "DIAG_VALUE", DataViewRowState.CurrentRows)
                DV = New DataView(TmpDT, "", "Priority, OrigPriority DESC", DataViewRowState.CurrentRows)

                For Cnt As Integer = 0 To DV.Count - 1
                    ProcedureCodesDV.RowFilter = "DIAG_VALUE = '" & DV(Cnt)("DIAG_VALUE").ToString & "'"

                    ProcedureCodesDV(0)("Priority") = Cnt
                Next

                For Cnt As Integer = 0 To DV.Count - 1
                    If DV(Cnt)("Selected").ToString = "True" Then
                        DiagnosisCodesDataGrid.Select(Cnt)

                        If Cnt < DiagnosisCodesDataGrid.FirstVisibleRow + 1 Then
                            DiagnosisCodesDataGrid.MoveGridToRow(CInt(If(Cnt > 0, Cnt - (DiagnosisCodesDataGrid.VisibleRowCount / 3) - 1, Cnt)))
                        End If
                    Else
                        DiagnosisCodesDataGrid.UnSelect(Cnt)
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
        If _DiagnosisCodesDT.Rows.Count < 1 Then Exit Sub

        Dim DR As DataRow
        Dim TmpDT As New DataTable("SortOrder")
        Dim GridDV As DataView = DiagnosisCodesDataGrid.GetDefaultDataView
        Dim DV As DataView
        Dim ReasonDV As DataView
        Dim OrigFirstR As Integer = CInt(DiagnosisCodesDataGrid.FirstVisibleRow)
        Dim FirstRow As Integer
        Dim SuccessionCnt As Integer = 1
        Dim LastSel As Boolean = False

        Try
            Using WC As New GlobalCursor

                TmpDT.Columns.Add("DIAG_VALUE")
                TmpDT.Columns.Add("OrigPriority", System.Type.GetType("System.Int32"))
                TmpDT.Columns.Add("Priority", System.Type.GetType("System.Int32"))
                TmpDT.Columns.Add("Selected")

                For Cnt As Integer = 0 To GridDV.Count - 1
                    DR = TmpDT.NewRow

                    DR("DIAG_VALUE") = GridDV(Cnt)("DIAG_VALUE")
                    DR("OrigPriority") = Cnt
                    DR("Priority") = Cnt
                    DR("Selected") = DiagnosisCodesDataGrid.IsSelected(Cnt)

                    TmpDT.Rows.Add(DR)
                Next

                For Cnt As Integer = 0 To _DiagnosisCodesDT.Rows.Count - 1
                    If DiagnosisCodesDataGrid.IsSelected(Cnt) Then
                        TmpDT.Rows(Cnt)("Priority") = CInt(TmpDT.Rows(Cnt)("Priority")) + 1
                    End If
                Next

                For Cnt As Integer = _DiagnosisCodesDT.Rows.Count - 1 To 0 Step -1
                    If DiagnosisCodesDataGrid.IsSelected(Cnt) Then
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

                DiagnosisCodesDataGrid.MoveGridToRow(OrigFirstR)

                ReasonDV = New DataView(_DiagnosisCodesDT, "", "DIAG_VALUE", DataViewRowState.CurrentRows)
                DV = New DataView(TmpDT, "", "Priority, OrigPriority DESC", DataViewRowState.CurrentRows)

                For Cnt As Integer = 0 To DV.Count - 1
                    ReasonDV.RowFilter = "DIAG_VALUE = '" & DV(Cnt)("DIAG_VALUE").ToString & "'"

                    ReasonDV(0)("Priority") = Cnt
                Next

                For Cnt As Integer = 0 To DV.Count - 1
                    If DV(Cnt)("Selected").ToString.ToString = "True" Then
                        DiagnosisCodesDataGrid.Select(Cnt)

                        FirstRow = CInt(DiagnosisCodesDataGrid.FirstVisibleRow)

                        If Cnt < FirstRow OrElse Cnt > (FirstRow + DiagnosisCodesDataGrid.VisibleRowCount - 2) Then
                            DiagnosisCodesDataGrid.MoveGridToRow(CInt(Cnt - (DiagnosisCodesDataGrid.VisibleRowCount / 3)))
                        End If
                    Else
                        DiagnosisCodesDataGrid.UnSelect(Cnt)
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

    Private Sub txtDiagnosisCodes_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDiagnosisCodes.TextChanged

        If txtDiagnosisCodes.Text.Length = 0 Then
            AddButton.Enabled = False
            'Me.AcceptButton = Nothing
        Else
            If DiagnosisCodesDataGrid.GetGridRowCount < _MAXDIAGNOSIS Then
                AddButton.Enabled = True
                'Me.AcceptButton = AddButton
            Else
                AddButton.Enabled = False
                'Me.AcceptButton = Nothing
            End If
        End If

    End Sub

    Private Sub DiagnosisCodesDataGrid_RowCountChanged(ByVal previousRowCount As Integer?, ByVal currentRowCount As Integer) Handles DiagnosisCodesDataGrid.RowCountChanged

        If currentRowCount = 0 Then
            DeleteButton.Enabled = False
            SortUpButton.Enabled = False
            SortDownButton.Enabled = False
        Else
            DeleteButton.Enabled = True
            SortUpButton.Enabled = True
            SortDownButton.Enabled = True
        End If

        If currentRowCount >= _MAXDIAGNOSIS Then
            txtDiagnosisCodes.Enabled = False
            AddButton.Enabled = False
            'Me.AcceptButton = Nothing
        Else
            txtDiagnosisCodes.Enabled = True

            If txtDiagnosisCodes.Text.Length > 0 Then
                AddButton.Enabled = True
                'Me.AcceptButton = AddButton
            End If
        End If
    End Sub

    Private Sub DiagnosisCodesTextBox_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiagnosisCodes.EnabledChanged
        DiagnosisCodeLookupButton.Enabled = txtDiagnosisCodes.Enabled
    End Sub

    Private Function AddDiagnosisCode() As Boolean

        Dim DiagnosisDR As DataRow
        Dim DiagnosisCodesDT As DataTable

        Try

            DiagnosisCodesDT = _DiagnosisCodesDT.Clone

            Dim NoCommaText As String = txtDiagnosisCodes.Text.ToUpper.Replace(",", " ")
            Dim DiagnosisAL As String() = NoCommaText.Split(New Char() {CChar(" ")}, StringSplitOptions.RemoveEmptyEntries)
            Dim Priority As Short = 0

            If (DiagnosisAL.Length + _DiagnosisCodesDT.Rows.Count) > _MAXDIAGNOSIS Then
                txtDiagnosisCodes.SelectionStart = 0
                txtDiagnosisCodes.SelectionLength = txtDiagnosisCodes.Text.Length

                MessageBox.Show("Only " & _MAXDIAGNOSIS & " Diagnosis Are Allowed.  Please Select The Best Match", "Too Many Diagnoses", MessageBoxButtons.OK, MessageBoxIcon.Information)

                txtDiagnosisCodes.Focus()

                Return False
            End If

            Dim Seq As Short = 0
            For Each DR As DataRow In _DiagnosisCodesDT.Rows

                If DR.RowState = DataRowState.Deleted Then Continue For

                Dim NewDR As DataRow = DiagnosisCodesDT.NewRow

                NewDR("DIAG_VALUE") = DR("DIAG_VALUE")
                NewDR("FULL_DESC") = DR("FULL_DESC")
                NewDR("Priority") = Seq

                Seq += 1S

                DiagnosisCodesDT.Rows.Add(NewDR)

            Next

            For Each Diagnosis As String In DiagnosisAL

                If Diagnosis.Length < 3 Then
                    If IsNumeric(Diagnosis) Then
                        Diagnosis = Diagnosis.PadLeft(3, CChar("0"))
                    End If
                End If

                DiagnosisDR = CMSDALFDBMD.RetrieveDiagnosisValuesInformation(Diagnosis)

                If DiagnosisDR Is Nothing Then
                    txtDiagnosisCodes.SelectionStart = InStr(txtDiagnosisCodes.Text.ToUpper, Diagnosis) - 1
                    txtDiagnosisCodes.SelectionLength = Diagnosis.Length

                    MessageBox.Show("Diagnosis " & """" & Diagnosis & """" & " is not a valid Diagnosis code.", "Invalid Diagnosis", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    txtDiagnosisCodes.Focus()

                    Return False
                Else
                    Dim NewDR As DataRow = DiagnosisCodesDT.NewRow

                    NewDR("DIAG_VALUE") = DiagnosisDR("DIAG_VALUE")
                    NewDR("FULL_DESC") = DiagnosisDR("FULL_DESC")
                    NewDR("Priority") = Seq

                    Seq += 1S

                    DiagnosisCodesDT.Rows.Add(NewDR)
                End If
            Next

            DiagnosisCodesDT.AcceptChanges()

            txtDiagnosisCodes.Text = ""

            _DiagnosisCodesDT.Clear()

            For Each DR As DataRow In DiagnosisCodesDT.Rows
                _DiagnosisCodesDT.ImportRow(DR)
            Next

            Return True

        Catch ex As Exception
            Throw

        Finally
            If DiagnosisCodesDT IsNot Nothing Then
                DiagnosisCodesDT.Dispose()
            End If
            DiagnosisCodesDT = Nothing
        End Try

    End Function

    Private Sub Delete()

        Dim DV As DataView
        Dim GridDV As DataView
        Dim TmpDT As New DataTable("SortOrder")
        Dim DR As DataRow

        Try

            Using WC As New GlobalCursor

                GridDV = DiagnosisCodesDataGrid.GetDefaultDataView

                TmpDT.Columns.Add("DIAG_VALUE")
                TmpDT.Columns.Add("OrigPriority", System.Type.GetType("System.Int32"))
                TmpDT.Columns.Add("Priority", System.Type.GetType("System.Int32"))
                TmpDT.Columns.Add("Selected")

                For Cnt As Integer = 0 To GridDV.Count - 1
                    DR = TmpDT.NewRow

                    DR("DIAG_VALUE") = GridDV(Cnt)("DIAG_VALUE")
                    DR("OrigPriority") = Cnt
                    DR("Priority") = Cnt
                    DR("Selected") = DiagnosisCodesDataGrid.IsSelected(Cnt)

                    TmpDT.Rows.Add(DR)
                Next

                DV = New DataView(_DiagnosisCodesDT, "", "DIAG_VALUE", DataViewRowState.CurrentRows)

                For Cnt As Integer = TmpDT.Rows.Count - 1 To 0 Step -1
                    If TmpDT.Rows(Cnt)("Selected").ToString = "True" Then
                        DV.RowFilter = "DIAG_VALUE = '" & TmpDT.Rows(Cnt)("DIAG_VALUE").ToString & "'"
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

    Private Sub DiagnosisCodeLookupButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiagnosisCodeLookupButton.Click

        Dim Frm As DiagnosisCodeLookupForm
        Dim AL As ArrayList

        Try

            Frm = New DiagnosisCodeLookupForm

            If Frm.ShowDialog(Me) = DialogResult.OK Then

                AL = Frm.DiagnosisCodesLookupDataGrid.GetSelectedDataRows()

                Dim FlattenQuery = String.Join(",", (From SelectedDR In AL).Select(Function(p) CType(p, DataRow)("DIAG_VALUE")))

                txtDiagnosisCodes.Text = FlattenQuery.ToString
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