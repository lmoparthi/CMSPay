Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Imports System.Text

Public Class NDCLookup
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"

    Private _NDCDS As DataSet
    Private _NDCDV As DataView
    Private _SB As New StringBuilder
    Private _SBLastKeyCapturedTime As Date = UFCWGeneral.NowDate
    Private _FoundDR() As DataRow

    Friend WithEvents ResultsMenu As System.Windows.Forms.ContextMenu
    Friend WithEvents ResultsDisplayDocumentMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ResultsHistoryMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents NDCLookupDataGrid As DataGridCustom
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents GFCLookupDataGrid As DataGridCustom
    Friend WithEvents LBLY2KLookupDataGrid As DataGridCustom


    Private WithEvents _BM As BindingManagerBase

    <System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)

            _APPKEY = value
        End Set
    End Property

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
            If _NDCDS IsNot Nothing Then _NDCDS.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents MultiTimer As System.Timers.Timer
    Friend WithEvents NDCValuesDataSet As NDCValuesDataSet
    Friend WithEvents OKButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.MultiTimer = New System.Timers.Timer()
        Me.NDCValuesDataSet = New NDCValuesDataSet()
        Me.OKButton = New System.Windows.Forms.Button()
        Me.ResultsMenu = New System.Windows.Forms.ContextMenu()
        Me.ResultsDisplayDocumentMenuItem = New System.Windows.Forms.MenuItem()
        Me.ResultsHistoryMenuItem = New System.Windows.Forms.MenuItem()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.NDCLookupDataGrid = New DataGridCustom()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.GFCLookupDataGrid = New DataGridCustom()
        Me.LBLY2KLookupDataGrid = New DataGridCustom()
        CType(Me.MultiTimer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NDCValuesDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.NDCLookupDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        CType(Me.GFCLookupDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LBLY2KLookupDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MultiTimer
        '
        Me.MultiTimer.SynchronizingObject = Me
        '
        'NDCValuesDataSet
        '
        Me.NDCValuesDataSet.DataSetName = "NDCValuesDataSet"
        Me.NDCValuesDataSet.Locale = New System.Globalization.CultureInfo("en-US")
        Me.NDCValuesDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'OKButton
        '
        Me.OKButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OKButton.Location = New System.Drawing.Point(465, 665)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(75, 23)
        Me.OKButton.TabIndex = 44
        Me.OKButton.Text = "OK"
        '
        'ResultsMenu
        '
        Me.ResultsMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.ResultsDisplayDocumentMenuItem, Me.ResultsHistoryMenuItem})
        '
        'ResultsDisplayDocumentMenuItem
        '
        Me.ResultsDisplayDocumentMenuItem.Index = 0
        Me.ResultsDisplayDocumentMenuItem.Text = "GFC"
        '
        'ResultsHistoryMenuItem
        '
        Me.ResultsHistoryMenuItem.Index = 1
        Me.ResultsHistoryMenuItem.Text = "Labelling"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(8, 6)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.NDCLookupDataGrid)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainer2)
        Me.SplitContainer1.Size = New System.Drawing.Size(534, 653)
        Me.SplitContainer1.SplitterDistance = 479
        Me.SplitContainer1.TabIndex = 45
        '
        'NDCLookupDataGrid
        '
        Me.NDCLookupDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.NDCLookupDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.NDCLookupDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.NDCLookupDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.NDCLookupDataGrid.ADGroupsThatCanFind = ""
        Me.NDCLookupDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.NDCLookupDataGrid.ADGroupsThatCanMultiSort = ""
        Me.NDCLookupDataGrid.AllowAutoSize = True
        Me.NDCLookupDataGrid.AllowColumnReorder = True
        Me.NDCLookupDataGrid.AllowCopy = False
        Me.NDCLookupDataGrid.AllowCustomize = True
        Me.NDCLookupDataGrid.AllowDelete = False
        Me.NDCLookupDataGrid.AllowDragDrop = False
        Me.NDCLookupDataGrid.AllowEdit = False
        Me.NDCLookupDataGrid.AllowExport = False
        Me.NDCLookupDataGrid.AllowFilter = True
        Me.NDCLookupDataGrid.AllowFind = True
        Me.NDCLookupDataGrid.AllowGoTo = True
        Me.NDCLookupDataGrid.AllowMultiSelect = False
        Me.NDCLookupDataGrid.AllowMultiSort = False
        Me.NDCLookupDataGrid.AllowNew = False
        Me.NDCLookupDataGrid.AllowPrint = False
        Me.NDCLookupDataGrid.AllowRefresh = False
        Me.NDCLookupDataGrid.AppKey = "UFCW\Claims\"
        Me.NDCLookupDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.NDCLookupDataGrid.CaptionVisible = False
        Me.NDCLookupDataGrid.ColumnHeaderLabel = Nothing
        Me.NDCLookupDataGrid.ColumnRePositioning = False
        Me.NDCLookupDataGrid.ColumnResizing = False
        Me.NDCLookupDataGrid.ConfirmDelete = True
        Me.NDCLookupDataGrid.CopySelectedOnly = True
        Me.NDCLookupDataGrid.DataMember = ""
        Me.NDCLookupDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.NDCLookupDataGrid.DragColumn = 0
        Me.NDCLookupDataGrid.ExportSelectedOnly = True
        Me.NDCLookupDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.NDCLookupDataGrid.HighlightedRow = Nothing
        Me.NDCLookupDataGrid.IsMouseDown = False
        Me.NDCLookupDataGrid.LastGoToLine = ""
        Me.NDCLookupDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.NDCLookupDataGrid.MultiSort = False
        Me.NDCLookupDataGrid.Name = "NDCLookupDataGrid"
        Me.NDCLookupDataGrid.OldSelectedRow = Nothing
        Me.NDCLookupDataGrid.ReadOnly = True
        Me.NDCLookupDataGrid.SetRowOnRightClick = True
        Me.NDCLookupDataGrid.ShiftPressed = False
        Me.NDCLookupDataGrid.SingleClickBooleanColumns = False
        Me.NDCLookupDataGrid.Size = New System.Drawing.Size(534, 479)
        Me.NDCLookupDataGrid.StyleName = ""
        Me.NDCLookupDataGrid.SubKey = ""
        Me.NDCLookupDataGrid.SuppressTriangle = False
        Me.NDCLookupDataGrid.TabIndex = 1
        Me.NDCLookupDataGrid.TabStop = False
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.GFCLookupDataGrid)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.LBLY2KLookupDataGrid)
        Me.SplitContainer2.Size = New System.Drawing.Size(534, 170)
        Me.SplitContainer2.SplitterDistance = 109
        Me.SplitContainer2.TabIndex = 0
        '
        'GFCLookupDataGrid
        '
        Me.GFCLookupDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.GFCLookupDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.GFCLookupDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.GFCLookupDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.GFCLookupDataGrid.ADGroupsThatCanFind = ""
        Me.GFCLookupDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.GFCLookupDataGrid.ADGroupsThatCanMultiSort = ""
        Me.GFCLookupDataGrid.AllowAutoSize = True
        Me.GFCLookupDataGrid.AllowColumnReorder = True
        Me.GFCLookupDataGrid.AllowCopy = False
        Me.GFCLookupDataGrid.AllowCustomize = True
        Me.GFCLookupDataGrid.AllowDelete = False
        Me.GFCLookupDataGrid.AllowDragDrop = False
        Me.GFCLookupDataGrid.AllowEdit = False
        Me.GFCLookupDataGrid.AllowExport = False
        Me.GFCLookupDataGrid.AllowFilter = False
        Me.GFCLookupDataGrid.AllowFind = True
        Me.GFCLookupDataGrid.AllowGoTo = True
        Me.GFCLookupDataGrid.AllowMultiSelect = False
        Me.GFCLookupDataGrid.AllowMultiSort = False
        Me.GFCLookupDataGrid.AllowNew = False
        Me.GFCLookupDataGrid.AllowPrint = False
        Me.GFCLookupDataGrid.AllowRefresh = False
        Me.GFCLookupDataGrid.AppKey = "UFCW\Claims\"
        Me.GFCLookupDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.GFCLookupDataGrid.CaptionVisible = False
        Me.GFCLookupDataGrid.ColumnHeaderLabel = Nothing
        Me.GFCLookupDataGrid.ColumnRePositioning = False
        Me.GFCLookupDataGrid.ColumnResizing = False
        Me.GFCLookupDataGrid.ConfirmDelete = True
        Me.GFCLookupDataGrid.CopySelectedOnly = False
        Me.GFCLookupDataGrid.DataMember = ""
        Me.GFCLookupDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GFCLookupDataGrid.DragColumn = 0
        Me.GFCLookupDataGrid.ExportSelectedOnly = False
        Me.GFCLookupDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.GFCLookupDataGrid.HighlightedRow = Nothing
        Me.GFCLookupDataGrid.IsMouseDown = False
        Me.GFCLookupDataGrid.LastGoToLine = ""
        Me.GFCLookupDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.GFCLookupDataGrid.MultiSort = False
        Me.GFCLookupDataGrid.Name = "GFCLookupDataGrid"
        Me.GFCLookupDataGrid.OldSelectedRow = Nothing
        Me.GFCLookupDataGrid.ReadOnly = True
        Me.GFCLookupDataGrid.SetRowOnRightClick = True
        Me.GFCLookupDataGrid.ShiftPressed = False
        Me.GFCLookupDataGrid.SingleClickBooleanColumns = False
        Me.GFCLookupDataGrid.Size = New System.Drawing.Size(534, 109)
        Me.GFCLookupDataGrid.StyleName = ""
        Me.GFCLookupDataGrid.SubKey = ""
        Me.GFCLookupDataGrid.SuppressTriangle = False
        Me.GFCLookupDataGrid.TabIndex = 2
        Me.GFCLookupDataGrid.TabStop = False
        '
        'LBLY2KLookupDataGrid
        '
        Me.LBLY2KLookupDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.LBLY2KLookupDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.LBLY2KLookupDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LBLY2KLookupDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LBLY2KLookupDataGrid.ADGroupsThatCanFind = ""
        Me.LBLY2KLookupDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LBLY2KLookupDataGrid.ADGroupsThatCanMultiSort = ""
        Me.LBLY2KLookupDataGrid.AllowAutoSize = True
        Me.LBLY2KLookupDataGrid.AllowColumnReorder = True
        Me.LBLY2KLookupDataGrid.AllowCopy = False
        Me.LBLY2KLookupDataGrid.AllowCustomize = True
        Me.LBLY2KLookupDataGrid.AllowDelete = False
        Me.LBLY2KLookupDataGrid.AllowDragDrop = False
        Me.LBLY2KLookupDataGrid.AllowEdit = False
        Me.LBLY2KLookupDataGrid.AllowExport = False
        Me.LBLY2KLookupDataGrid.AllowFilter = False
        Me.LBLY2KLookupDataGrid.AllowFind = True
        Me.LBLY2KLookupDataGrid.AllowGoTo = True
        Me.LBLY2KLookupDataGrid.AllowMultiSelect = False
        Me.LBLY2KLookupDataGrid.AllowMultiSort = False
        Me.LBLY2KLookupDataGrid.AllowNew = False
        Me.LBLY2KLookupDataGrid.AllowPrint = False
        Me.LBLY2KLookupDataGrid.AllowRefresh = False
        Me.LBLY2KLookupDataGrid.AppKey = "UFCW\Claims\"
        Me.LBLY2KLookupDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.LBLY2KLookupDataGrid.CaptionVisible = False
        Me.LBLY2KLookupDataGrid.ColumnHeaderLabel = Nothing
        Me.LBLY2KLookupDataGrid.ColumnRePositioning = False
        Me.LBLY2KLookupDataGrid.ColumnResizing = False
        Me.LBLY2KLookupDataGrid.ConfirmDelete = True
        Me.LBLY2KLookupDataGrid.CopySelectedOnly = False
        Me.LBLY2KLookupDataGrid.DataMember = ""
        Me.LBLY2KLookupDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LBLY2KLookupDataGrid.DragColumn = 0
        Me.LBLY2KLookupDataGrid.ExportSelectedOnly = True
        Me.LBLY2KLookupDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.LBLY2KLookupDataGrid.HighlightedRow = Nothing
        Me.LBLY2KLookupDataGrid.IsMouseDown = False
        Me.LBLY2KLookupDataGrid.LastGoToLine = ""
        Me.LBLY2KLookupDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.LBLY2KLookupDataGrid.MultiSort = False
        Me.LBLY2KLookupDataGrid.Name = "LBLY2KLookupDataGrid"
        Me.LBLY2KLookupDataGrid.OldSelectedRow = Nothing
        Me.LBLY2KLookupDataGrid.ReadOnly = True
        Me.LBLY2KLookupDataGrid.SetRowOnRightClick = True
        Me.LBLY2KLookupDataGrid.ShiftPressed = False
        Me.LBLY2KLookupDataGrid.SingleClickBooleanColumns = False
        Me.LBLY2KLookupDataGrid.Size = New System.Drawing.Size(534, 57)
        Me.LBLY2KLookupDataGrid.StyleName = ""
        Me.LBLY2KLookupDataGrid.SubKey = ""
        Me.LBLY2KLookupDataGrid.SuppressTriangle = False
        Me.LBLY2KLookupDataGrid.TabIndex = 2
        Me.LBLY2KLookupDataGrid.TabStop = False
        '
        'NDCLookup
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.OKButton
        Me.ClientSize = New System.Drawing.Size(550, 691)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.OKButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(392, 176)
        Me.Name = "NDCLookup"
        Me.ShowInTaskbar = False
        Me.Text = "Rebook NDC ..."
        CType(Me.MultiTimer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NDCValuesDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.NDCLookupDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        CType(Me.GFCLookupDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LBLY2KLookupDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Form Events"
    Private Sub NDCLookup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not UFCWGeneral.SetFormPosition(Me, _APPKEY) Then Me.CenterToScreen()

            Dim FormText As String = Me.Text

            Me.Text = "Loading NDC... Please Wait"
            Me.Show()
            Me.Refresh()

            LoadNDC()

            Me.Text = FormText

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub NDC_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        UFCWGeneral.SaveFormPosition(Me, _APPKEY)
    End Sub

#End Region

    Private Sub LoadNDC()
        Try
            Using WC As New GlobalCursor

                _NDCDS = CMSDALFDBMD.RetrieveNDCValues
                NDCLookupDataGrid.SuspendLayout()
                NDCLookupDataGrid.DataSource = _NDCDS.Tables("NDC_VALUES")
                _BM = Me.BindingContext(NDCLookupDataGrid.DataSource)

                NDCLookupDataGrid.SetTableStyle()
                NDCLookupDataGrid.ResumeLayout()

                DisplayAssociatedInfo()

            End Using

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub RowChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _BM.PositionChanged

        DisplayAssociatedInfo()

    End Sub

    Private Sub NDCDataGrid_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        Try
            If Char.IsLetterOrDigit(e.KeyChar) Then
                e.Handled = True

                If DateDiff("s", _SBLastKeyCapturedTime, UFCWGeneral.NowDate) > 0 Then
                    _SB = New StringBuilder
                End If

                _SB.Append(e.KeyChar.ToString())
                _SBLastKeyCapturedTime = UFCWGeneral.NowDate
            Else
                _SB = New StringBuilder
            End If

            If _SB.Length > 0 Then

                Try
                    Dim DT As DataTable = CType(NDCLookupDataGrid.DataSource, DataTable)

                    Debug.WriteLine("Selecting " & "NDC LIKE '" & _SB.ToString.ToUpper & "%'")

                    dt.DefaultView.Sort = "NDC"

                    _FoundDR = dt.Select("NDC LIKE '" & _SB.ToString.ToUpper & "%'", "NDC")

                    If _FoundDR.Length > 0 Then
                        BindingContext(dt).Position = dt.DefaultView.Find(_FoundDR(0)("NDC"))
                        NDCLookupDataGrid.MoveGridToRow(BindingContext(dt).Position)
                        NDCLookupDataGrid.Select(1)
                    End If

                Catch ex As Exception
                    Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
                    If (rethrow) Then
                        Throw
                    End If
                End Try
            End If

            If Asc(e.KeyChar) = Keys.Enter Then NDCLookupDataGrid.Find(_SB.ToString())

        Catch ex As Exception

	Throw
        Finally
        End Try
    End Sub

    Private Sub NDCDataGrid_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles NDCLookupDataGrid.MouseUp
        Dim DG As DataGrid = CType(sender, DataGrid)
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo
        HTI = DG.HitTest(e.X, e.Y)

        Try
            If e.Button = MouseButtons.Left Then
                Select Case HTI.Type
                    Case Is = DataGrid.HitTestType.Cell
                        If NDCLookupDataGrid.IsSelected(HTI.Row) = True Then
                            NDCLookupDataGrid.UnSelect(HTI.Row)
                        Else
                            NDCLookupDataGrid.Select(HTI.Row)
                        End If
                    Case Is = DataGrid.HitTestType.RowHeader
                        If NDCLookupDataGrid.IsSelected(HTI.Row) = False Then
                            Exit Try
                        End If

                End Select

                If HTI.Type = DataGrid.HitTestType.Cell OrElse HTI.Type = DataGrid.HitTestType.RowHeader Then
                    NDCLookupDataGrid.Select(HTI.Row)
                End If
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub DisplayAssociatedInfo()

        If Me.NDCLookupDataGrid.DataSource Is Nothing Then Return

        Dim bm As BindingManagerBase = Me.NDCLookupDataGrid.BindingContext(Me.NDCLookupDataGrid.DataSource, Me.NDCLookupDataGrid.DataMember)
        Dim DR As DataRow = CType(bm.Current, DataRowView).Row

        Try

            If _NDCDS.Tables("REDBOOK_GFC") IsNot Nothing Then _NDCDS.Tables("REDBOOK_GFC").Clear()
            If _NDCDS.Tables("REDBOOK_LBL_Y2K") IsNot Nothing Then _NDCDS.Tables("REDBOOK_LBL_Y2K").Clear()

            _NDCDS = CMSDALFDBMD.RetrieveGFCandLBLByNDC(CStr(dr("NDC11")), _NDCDS)

            GFCLookupDataGrid.SuspendLayout()
            LBLY2KLookupDataGrid.SuspendLayout()

            GFCLookupDataGrid.DataSource = _NDCDS.Tables("REDBOOK_GFC")
            LBLY2KLookupDataGrid.DataSource = _NDCDS.Tables("REDBOOK_LBL_Y2K")

            GFCLookupDataGrid.SetTableStyle()
            LBLY2KLookupDataGrid.SetTableStyle()

            GFCLookupDataGrid.ResumeLayout()
            LBLY2KLookupDataGrid.ResumeLayout()

        Catch ex As Exception
            Throw
        End Try

    End Sub

End Class