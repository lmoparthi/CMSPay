Imports System.ComponentModel
Imports UFCW.WCF
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class HRAActivityControl
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _FamilyID As Integer? = Nothing
    Private _RelationID As Short? = Nothing
    Private _APPKEY As String = "UFCW\Claims\"

    Private _DocID As Long = -1

    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents HRAEffectiveYearComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents HRATransactionYearComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents HRAActivityDataGrid As DataGridCustom
    Friend WithEvents _HRADataSet As DataSet
    Friend WithEvents HRAEventYearComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ViewOpenButton As System.Windows.Forms.Button
    Friend WithEvents ResultsDataGridCustomContextMenu As ContextMenuStrip
    Friend WithEvents DisplayMenuItem As ToolStripMenuItem
    Private _Loading As Boolean = True

    Public Event BeforeRefresh(ByVal sender As Object, ByRef cancel As Boolean)
    Public Event AfterRefresh(ByVal sender As Object)

    Public Overloads Sub Dispose()
        If HRAActivityDataGrid IsNot Nothing AndAlso HRAActivityDataGrid.DataSource IsNot Nothing Then
            HRAActivityDataGrid.TableStyles.Clear()
            HRAActivityDataGrid.DataSource = Nothing
            HRAActivityDataGrid.Dispose()
        End If
        MyBase.Dispose()
    End Sub
#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'HRAActivityControl overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
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
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.HRAEffectiveYearComboBox = New System.Windows.Forms.ComboBox()
        Me.HRATransactionYearComboBox = New System.Windows.Forms.ComboBox()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.ViewOpenButton = New System.Windows.Forms.Button()
        Me.HRAEventYearComboBox = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.HRAActivityDataGrid = New DataGridCustom()
        Me._HRADataSet = New DataSet()
        Me.ResultsDataGridCustomContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.DisplayMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.HRAActivityDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._HRADataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ResultsDataGridCustomContextMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 5)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(74, 13)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Effective Year"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(102, 5)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "Event Year"
        '
        'HRAEffectiveYearComboBox
        '
        Me.HRAEffectiveYearComboBox.FormattingEnabled = True
        Me.HRAEffectiveYearComboBox.Location = New System.Drawing.Point(9, 21)
        Me.HRAEffectiveYearComboBox.Name = "HRAEffectiveYearComboBox"
        Me.HRAEffectiveYearComboBox.Size = New System.Drawing.Size(83, 21)
        Me.HRAEffectiveYearComboBox.TabIndex = 12
        '
        'HRATransactionYearComboBox
        '
        Me.HRATransactionYearComboBox.FormattingEnabled = True
        Me.HRATransactionYearComboBox.Location = New System.Drawing.Point(201, 21)
        Me.HRATransactionYearComboBox.Name = "HRATransactionYearComboBox"
        Me.HRATransactionYearComboBox.Size = New System.Drawing.Size(83, 21)
        Me.HRATransactionYearComboBox.TabIndex = 13
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.IsSplitterFixed = True
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.ViewOpenButton)
        Me.SplitContainer1.Panel1.Controls.Add(Me.HRAEventYearComboBox)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label3)
        Me.SplitContainer1.Panel1.Controls.Add(Me.HRATransactionYearComboBox)
        Me.SplitContainer1.Panel1.Controls.Add(Me.HRAEffectiveYearComboBox)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.HRAActivityDataGrid)
        Me.SplitContainer1.Size = New System.Drawing.Size(438, 358)
        Me.SplitContainer1.SplitterDistance = 46
        Me.SplitContainer1.TabIndex = 0
        '
        'ViewOpenButton
        '
        Me.ViewOpenButton.Location = New System.Drawing.Point(351, 19)
        Me.ViewOpenButton.Name = "ViewOpenButton"
        Me.ViewOpenButton.Size = New System.Drawing.Size(75, 23)
        Me.ViewOpenButton.TabIndex = 16
        Me.ViewOpenButton.Text = "View Open"
        Me.ViewOpenButton.UseVisualStyleBackColor = True
        '
        'HRAEventYearComboBox
        '
        Me.HRAEventYearComboBox.FormattingEnabled = True
        Me.HRAEventYearComboBox.Location = New System.Drawing.Point(105, 21)
        Me.HRAEventYearComboBox.Name = "HRAEventYearComboBox"
        Me.HRAEventYearComboBox.Size = New System.Drawing.Size(83, 21)
        Me.HRAEventYearComboBox.TabIndex = 15
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(199, 5)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(88, 13)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "Transaction Year"
        '
        'HRAActivityDataGrid
        '
        Me.HRAActivityDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.HRAActivityDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.HRAActivityDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.HRAActivityDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.HRAActivityDataGrid.ADGroupsThatCanFind = ""
        Me.HRAActivityDataGrid.ADGroupsThatCanMultiSort = "CMSUsers"
        Me.HRAActivityDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.HRAActivityDataGrid.AllowAutoSize = True
        Me.HRAActivityDataGrid.AllowColumnReorder = True
        Me.HRAActivityDataGrid.AllowCopy = True
        Me.HRAActivityDataGrid.AllowCustomize = True
        Me.HRAActivityDataGrid.AllowDelete = False
        Me.HRAActivityDataGrid.AllowDragDrop = False
        Me.HRAActivityDataGrid.AllowEdit = False
        Me.HRAActivityDataGrid.AllowExport = True
        Me.HRAActivityDataGrid.AllowFilter = True
        Me.HRAActivityDataGrid.AllowFind = True
        Me.HRAActivityDataGrid.AllowGoTo = True
        Me.HRAActivityDataGrid.AllowMultiSelect = True
        Me.HRAActivityDataGrid.AllowMultiSort = True
        Me.HRAActivityDataGrid.AllowNew = False
        Me.HRAActivityDataGrid.AllowPrint = True
        Me.HRAActivityDataGrid.AllowRefresh = False
        Me.HRAActivityDataGrid.AppKey = "UFCW\Claims\"
        Me.HRAActivityDataGrid.AutoSaveCols = True
        Me.HRAActivityDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.HRAActivityDataGrid.CaptionForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.HRAActivityDataGrid.CaptionText = "HRA Activity"
        Me.HRAActivityDataGrid.ColumnHeaderLabel = Nothing
        Me.HRAActivityDataGrid.ColumnRePositioning = False
        Me.HRAActivityDataGrid.ColumnResizing = False
        Me.HRAActivityDataGrid.ConfirmDelete = True
        Me.HRAActivityDataGrid.CopySelectedOnly = True
        Me.HRAActivityDataGrid.DataMember = ""
        Me.HRAActivityDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HRAActivityDataGrid.DragColumn = 0
        Me.HRAActivityDataGrid.ExportSelectedOnly = True
        Me.HRAActivityDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.HRAActivityDataGrid.HighlightedRow = Nothing
        Me.HRAActivityDataGrid.IsMouseDown = False
        Me.HRAActivityDataGrid.LastGoToLine = ""
        Me.HRAActivityDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.HRAActivityDataGrid.MultiSort = False
        Me.HRAActivityDataGrid.Name = "HRAActivityDataGrid"
        Me.HRAActivityDataGrid.OldSelectedRow = Nothing
        Me.HRAActivityDataGrid.ParentRowsVisible = False
        Me.HRAActivityDataGrid.ReadOnly = True
        Me.HRAActivityDataGrid.SetRowOnRightClick = True
        Me.HRAActivityDataGrid.ShiftPressed = False
        Me.HRAActivityDataGrid.SingleClickBooleanColumns = True
        Me.HRAActivityDataGrid.Size = New System.Drawing.Size(438, 308)
        Me.HRAActivityDataGrid.Sort = Nothing
        Me.HRAActivityDataGrid.StyleName = ""
        Me.HRAActivityDataGrid.SubKey = ""
        Me.HRAActivityDataGrid.SuppressTriangle = False
        Me.HRAActivityDataGrid.TabIndex = 8
        '
        '_HRADataSet
        '
        Me._HRADataSet.DataSetName = "HRADataSet"
        Me._HRADataSet.Locale = New System.Globalization.CultureInfo("en-US")
        Me._HRADataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'ResultsDataGridCustomContextMenu
        '
        Me.ResultsDataGridCustomContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DisplayMenuItem})
        Me.ResultsDataGridCustomContextMenu.Name = "ControlMenu"
        Me.ResultsDataGridCustomContextMenu.Size = New System.Drawing.Size(181, 48)
        '
        'DisplayMenuItem
        '
        Me.DisplayMenuItem.Name = "DisplayMenuItem"
        Me.DisplayMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.DisplayMenuItem.Text = "Display Document"
        '
        'HRAActivityControl
        '
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "HRAActivityControl"
        Me.Size = New System.Drawing.Size(438, 358)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.HRAActivityDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._HRADataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResultsDataGridCustomContextMenu.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Properties"
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the FamilyID of the Document.")>
    Public Property FamilyID() As Integer?
        Get
            Return _FamilyID
        End Get
        Set(ByVal value As Integer?)
            _FamilyID = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the FamilyID of the Document.")>
    Public ReadOnly Property HRAHistory() As DataTable
        Get
            Return _HRADataSet.Tables("HRA")
        End Get
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the RelationID of the Document.")>
    Public Property RelationID() As Short?
        Get
            Return _RelationID
        End Get
        Set(ByVal value As Short?)
            _RelationID = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
        End Set
    End Property
#End Region

#Region "Constructor"
    Public Sub New(ByVal familyID As Integer, ByVal relationID As Short?)
        Me.New()
        _FamilyID = familyID
        _RelationID = relationID

        LoadHRAActivityControl()

    End Sub
#End Region

#Region "Form\Button Events"
    Private Sub HRAActivityControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            _Loading = False

        Catch ex As Exception

	Throw
        End Try
    End Sub

    Public Sub RefreshActivity()
        Try

            HRADataGrid_RefreshGridData()

        Catch ex As Exception

	Throw
        End Try
    End Sub

    Private Sub HRADataGrid_RefreshGridData()
        Try
            Dim Cancel As Boolean = False

            RaiseEvent BeforeRefresh(Me, Cancel)

            If Not Cancel Then
                LoadHRAActivityControl()
            End If

            RaiseEvent AfterRefresh(Me)

        Catch ex As Exception

	Throw
        End Try
    End Sub

    Private Sub HRAActivityControl_dispose(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        Me.Dispose()
    End Sub

#End Region

#Region "Custom Subs\Functions"
    Public Sub LoadHRAActivityControl(ByVal familyID As Integer, ByVal relationID As Short?)
        Try
            _FamilyID = familyID
            _RelationID = relationID

            LoadHRAActivityControl()

        Catch ex As Exception

	Throw
        End Try
    End Sub

    Public Sub LoadHRAActivityControl(ByVal familyID As Integer, ByVal relationID As Short?, hraDataSet As DataSet)
        Try
            _FamilyID = familyID
            _RelationID = relationID

            LoadHRAActivityControl(hraDataSet)

        Catch ex As Exception

	Throw
        End Try
    End Sub

    Public Sub LoadHRAActivityControl(Optional hraDataSet As DataSet = Nothing)
        Try
            HRAActivityDataGrid.SuspendLayout()

            HRAActivityDataGrid.DataSource = Nothing
            _HRADataSet.Tables.Clear()

            If hraDataSet IsNot Nothing Then
                _HRADataSet = hraDataSet
            Else
                _HRADataSet = CType(CMSDALFDBHRA.RetrieveHRAAllHistory(CInt(_FamilyID), _RelationID, _HRADataSet), DataSet)
            End If

            HRAActivityDataGrid.DataSource = _HRADataSet.Tables(0)
            HRAActivityDataGrid.Sort = If(HRAActivityDataGrid.LastSortedBy, HRAActivityDataGrid.DefaultSort)

            ViewOpenButton.Text = "View Open"

            _HRADataSet.Tables(0).DefaultView.RowFilter = AddOpenOrALLFilter(_HRADataSet.Tables(0).DefaultView.RowFilter)

            HRAActivityDataGrid.SetTableStyle()
            HRAActivityDataGrid.ContextMenuPrepare(ResultsDataGridCustomContextMenu)

            HRAActivityDataGrid.ResumeLayout()

            RemoveHandler HRAEffectiveYearComboBox.SelectedIndexChanged, AddressOf HRAYearComboBox_SelectedIndexChanged
            RemoveHandler HRATransactionYearComboBox.SelectedIndexChanged, AddressOf HRAYearComboBox_SelectedIndexChanged
            RemoveHandler HRAEventYearComboBox.SelectedIndexChanged, AddressOf HRAYearComboBox_SelectedIndexChanged

            HRAEffectiveYearComboBox.DataSource = CMSDALCommon.SelectDistinctAndSortedWithFormat("", _HRADataSet.Tables(0), "EFFECTIVE_DATE DESC", True, "yyyy")
            HRAEffectiveYearComboBox.DisplayMember = "EFFECTIVE_DATE"

            HRATransactionYearComboBox.DataSource = CMSDALCommon.SelectDistinctAndSortedWithFormat("", _HRADataSet.Tables(0), "TRANSACTION_DATE DESC", True, "yyyy")
            HRATransactionYearComboBox.DisplayMember = "TRANSACTION_DATE"

            HRAEventYearComboBox.DataSource = CMSDALCommon.SelectDistinctAndSortedWithFormat("", _HRADataSet.Tables(0), "EVENT_DATE DESC", True, "yyyy")
            HRAEventYearComboBox.DisplayMember = "EVENT_DATE"

            AddHandler HRAEffectiveYearComboBox.SelectedIndexChanged, AddressOf HRAYearComboBox_SelectedIndexChanged
            AddHandler HRATransactionYearComboBox.SelectedIndexChanged, AddressOf HRAYearComboBox_SelectedIndexChanged
            AddHandler HRAEventYearComboBox.SelectedIndexChanged, AddressOf HRAYearComboBox_SelectedIndexChanged

        Catch ex As Exception

	Throw
        End Try
    End Sub

    Public Sub ClearAll()
        HRAActivityDataGrid.DataSource = Nothing
    End Sub

#End Region

    Private Sub HRAYearComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles HRAEffectiveYearComboBox.SelectedIndexChanged, HRATransactionYearComboBox.SelectedIndexChanged, HRAEventYearComboBox.SelectedIndexChanged
        Dim HRAActivityFilter As String

        Try

            RemoveHandler HRAEffectiveYearComboBox.SelectedIndexChanged, AddressOf HRAYearComboBox_SelectedIndexChanged
            RemoveHandler HRATransactionYearComboBox.SelectedIndexChanged, AddressOf HRAYearComboBox_SelectedIndexChanged
            RemoveHandler HRAEventYearComboBox.SelectedIndexChanged, AddressOf HRAYearComboBox_SelectedIndexChanged

            If HRAEffectiveYearComboBox.Text.ToUpper = "(ALL)" Then
                HRAEffectiveYearComboBox.SelectedIndex = 0
            Else
                HRAActivityFilter = "(EFFECTIVE_DATE >= #" & HRAEffectiveYearComboBox.Text & "/1/1# AND " & "EFFECTIVE_DATE <= #" & HRAEffectiveYearComboBox.Text & "/12/31#)"
            End If

            If HRATransactionYearComboBox.Text.ToUpper = "(ALL)" Then
                HRATransactionYearComboBox.SelectedIndex = 0
            Else
                HRAActivityFilter &= If(HRAActivityFilter Is Nothing OrElse HRAActivityFilter.Trim.Length = 0, "", " AND ").ToString & "(TRANSACTION_DATE >= #" & HRATransactionYearComboBox.Text & "/1/1# AND " & "TRANSACTION_DATE <= #" & HRATransactionYearComboBox.Text & "/12/31#)"
            End If

            If HRAEventYearComboBox.Text.ToUpper = "(ALL)" Then
                HRAEventYearComboBox.SelectedIndex = 0
            Else
                HRAActivityFilter &= If(HRAActivityFilter Is Nothing OrElse HRAActivityFilter.Trim.Length = 0, "", " AND ").ToString & "(EVENT_DATE >= #" & HRAEventYearComboBox.Text & "/1/1# AND " & "EVENT_DATE <= #" & HRAEventYearComboBox.Text & "/12/31#)"
            End If

            HRAActivityDataGrid.GetCurrentDataView.RowFilter = AddOpenOrALLFilter(HRAActivityFilter)

            AddHandler HRAEffectiveYearComboBox.SelectedIndexChanged, AddressOf HRAYearComboBox_SelectedIndexChanged
            AddHandler HRATransactionYearComboBox.SelectedIndexChanged, AddressOf HRAYearComboBox_SelectedIndexChanged
            AddHandler HRAEventYearComboBox.SelectedIndexChanged, AddressOf HRAYearComboBox_SelectedIndexChanged

        Catch ex As Exception

        End Try

    End Sub

    Private Sub ViewOpenButton_Click(sender As Object, e As EventArgs) Handles ViewOpenButton.Click

        HRAActivityDataGrid.SuspendLayout()

        Select Case ViewOpenButton.Text
            Case "View Open"
                ViewOpenButton.Text = "View All"
            Case Else
                ViewOpenButton.Text = "View Open"
        End Select

        If _HRADataSet IsNot Nothing AndAlso _HRADataSet.Tables.Count > 0 Then
            _HRADataSet.Tables(0).DefaultView.RowFilter = AddOpenOrALLFilter(_HRADataSet.Tables(0).DefaultView.RowFilter)
        End If

        HRAActivityDataGrid.SetTableStyle()
        HRAActivityDataGrid.ContextMenuPrepare(ResultsDataGridCustomContextMenu)

        HRAActivityDataGrid.ResumeLayout()

    End Sub
    Private Sub Resultsdisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DisplayMenuItem.Click
        DisplayDocument()
    End Sub
    Private Sub DisplayDocument()

        Dim BM As BindingManagerBase
        Dim DR As DataRow
        Dim Docs() As Object
        Dim Cnt As Integer = 0
        Dim Tot As Integer = 0

        Try
            BM = Me.HRAActivityDataGrid.BindingContext(Me.HRAActivityDataGrid.DataSource, Me.HRAActivityDataGrid.DataMember)

            If BM.Count > 0 Then

                DR = CType(BM.Current, DataRowView).Row

                If DR.Table.Columns.Contains("DOCID") AndAlso UFCWGeneral.IsNullLongHandler(DR("DOCID")) IsNot Nothing Then
                    ReDim Docs(1)

                    Docs(0) = DR("DOCID")

                    Using FNDisplay As New Display
                        FNDisplay.Display(Docs)
                        _DocID = CLng(DR("DOCID"))
                    End Using

                ElseIf DR.Table.Columns.Contains("MAXIM_ID") AndAlso UFCWGeneral.IsNullStringHandler(DR("MAXIM_ID")) IsNot Nothing Then

                    Using FNDisplay As New Display
                        Dim FNDocProperties As FileNet.FileNetDocumentProperties = FNDisplay.Display(DR("MAXIM_ID").ToString)
                        _DocID = FNDocProperties.ID
                    End Using
                Else
                    MessageBox.Show("There is no document to display.", "No Document", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

            End If

        Catch ex As ApplicationException

            MessageBox.Show(ex.Message, "FileNet unavailable, Restarting application may resolve connectivity issues.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        Catch ex As Exception

	Throw
        End Try
    End Sub

    Private Function AddOpenOrALLFilter(hraActivityFilter As String) As String

        If hraActivityFilter Is Nothing Then hraActivityFilter = ""

        Select Case ViewOpenButton.Text
            Case "View Open" 'then looking at ALL
                hraActivityFilter = hraActivityFilter.ToString.Replace("(PROCESS_STATUS = 'OPEN' OR STATUS_CODE = 'NEW') AND ", "").Replace("(PROCESS_STATUS = 'OPEN' OR STATUS_CODE = 'NEW')", "")
            Case Else
                hraActivityFilter = If(hraActivityFilter.Trim.Length > 0, If(hraActivityFilter.Contains("(PROCESS_STATUS = 'OPEN' OR STATUS_CODE = 'NEW')"), hraActivityFilter, "(PROCESS_STATUS = 'OPEN' OR STATUS_CODE = 'NEW') AND " & hraActivityFilter), "(PROCESS_STATUS = 'OPEN' OR STATUS_CODE = 'NEW')")
        End Select

        Return hraActivityFilter

    End Function

    Private Sub ResultsDataGridCustomContextMenu_Opening(sender As Object, e As CancelEventArgs) Handles ResultsDataGridCustomContextMenu.Opening

        Dim DataGridCustomContextMenu As ContextMenuStrip
        Dim DG As DataGridCustom

        DataGridCustomContextMenu = CType(sender, ContextMenuStrip)
        DG = CType(DirectCast(sender, System.Windows.Forms.ContextMenuStrip).SourceControl, DataGridCustom)

        DataGridCustomContextMenu.Items("DisplayMenuItem").Available = False

        If DG.GetGridRowCount > 0 Then
            DataGridCustomContextMenu.Items("DisplayMenuItem").Available = True
        End If

    End Sub
End Class