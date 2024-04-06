Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Imports System.Text
Imports System.Threading.Tasks

Public Class ProcedureCodeLookupForm
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _EffectiveDate? As Date
    Private _ProcCode As Object = System.DBNull.Value
    Private _APPKEY As String = "UFCW\Claims\"
    Private _Status As String = ""

    Private _ProcedureCodesDR As DataRow
    Private _ProcedureCodesDS As DataSet
    Private WithEvents _ProcedureCodesBS As BindingSource

    Private _SB As New StringBuilder
    Private _LastKeyCapturedTime As Date = UFCWGeneral.NowDate
    Private _FoundDRs() As DataRow
    Private _DT As DataTable

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Me.VScroll = True
    End Sub

    'Form overrides dispose to clean up the component list.
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
    Friend WithEvents ProcedureCodeLookupDataGrid As DataGridCustom
    Friend WithEvents ProcCodesDataSet As ProcCodesDataSet
    Friend WithEvents ClearAllButton As System.Windows.Forms.Button
    Friend WithEvents UpdateAllButton As System.Windows.Forms.Button
    Friend WithEvents CancelActionButton As System.Windows.Forms.Button
    Friend WithEvents UpdateLineButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ProcedureCodeLookupDataGrid = New DataGridCustom()
        Me.ProcCodesDataSet = New ProcCodesDataSet()
        Me.ClearAllButton = New System.Windows.Forms.Button()
        Me.UpdateAllButton = New System.Windows.Forms.Button()
        Me.CancelActionButton = New System.Windows.Forms.Button()
        Me.UpdateLineButton = New System.Windows.Forms.Button()
        CType(Me.ProcedureCodeLookupDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ProcCodesDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ProcedureCodesLookupDataGrid
        '
        Me.ProcedureCodeLookupDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.ProcedureCodeLookupDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.ProcedureCodeLookupDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ProcedureCodeLookupDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ProcedureCodeLookupDataGrid.ADGroupsThatCanFind = ""
        Me.ProcedureCodeLookupDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ProcedureCodeLookupDataGrid.ADGroupsThatCanMultiSort = ""
        Me.ProcedureCodeLookupDataGrid.AllowAutoSize = True
        Me.ProcedureCodeLookupDataGrid.AllowColumnReorder = False
        Me.ProcedureCodeLookupDataGrid.AllowCopy = True
        Me.ProcedureCodeLookupDataGrid.AllowCustomize = True
        Me.ProcedureCodeLookupDataGrid.AllowDelete = False
        Me.ProcedureCodeLookupDataGrid.AllowDragDrop = False
        Me.ProcedureCodeLookupDataGrid.AllowEdit = False
        Me.ProcedureCodeLookupDataGrid.AllowExport = False
        Me.ProcedureCodeLookupDataGrid.AllowFilter = True
        Me.ProcedureCodeLookupDataGrid.AllowFind = True
        Me.ProcedureCodeLookupDataGrid.AllowGoTo = True
        Me.ProcedureCodeLookupDataGrid.AllowMultiSelect = False
        Me.ProcedureCodeLookupDataGrid.AllowMultiSort = False
        Me.ProcedureCodeLookupDataGrid.AllowNew = False
        Me.ProcedureCodeLookupDataGrid.AllowPrint = False
        Me.ProcedureCodeLookupDataGrid.AllowRefresh = False
        Me.ProcedureCodeLookupDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProcedureCodeLookupDataGrid.AppKey = "UFCW\Claims\"
        Me.ProcedureCodeLookupDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.ProcedureCodeLookupDataGrid.CaptionVisible = False
        Me.ProcedureCodeLookupDataGrid.ColumnHeaderLabel = Nothing
        Me.ProcedureCodeLookupDataGrid.ColumnRePositioning = False
        Me.ProcedureCodeLookupDataGrid.ColumnResizing = False
        Me.ProcedureCodeLookupDataGrid.ConfirmDelete = True
        Me.ProcedureCodeLookupDataGrid.CopySelectedOnly = True
        Me.ProcedureCodeLookupDataGrid.DataMember = ""
        Me.ProcedureCodeLookupDataGrid.DragColumn = 0
        Me.ProcedureCodeLookupDataGrid.ExportSelectedOnly = True
        Me.ProcedureCodeLookupDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ProcedureCodeLookupDataGrid.HighlightedRow = Nothing
        Me.ProcedureCodeLookupDataGrid.IsMouseDown = False
        Me.ProcedureCodeLookupDataGrid.LastGoToLine = ""
        Me.ProcedureCodeLookupDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.ProcedureCodeLookupDataGrid.MultiSort = False
        Me.ProcedureCodeLookupDataGrid.Name = "ProcedureCodeLookupDataGrid"
        Me.ProcedureCodeLookupDataGrid.OldSelectedRow = Nothing
        Me.ProcedureCodeLookupDataGrid.ReadOnly = True
        Me.ProcedureCodeLookupDataGrid.SetRowOnRightClick = False
        Me.ProcedureCodeLookupDataGrid.ShiftPressed = False
        Me.ProcedureCodeLookupDataGrid.SingleClickBooleanColumns = True
        Me.ProcedureCodeLookupDataGrid.Size = New System.Drawing.Size(551, 451)
        Me.ProcedureCodeLookupDataGrid.StyleName = ""
        Me.ProcedureCodeLookupDataGrid.SubKey = ""
        Me.ProcedureCodeLookupDataGrid.SuppressTriangle = False
        Me.ProcedureCodeLookupDataGrid.TabIndex = 0
        Me.ProcedureCodeLookupDataGrid.TabStop = False
        '
        'ProcCodesDataSet
        '
        Me.ProcCodesDataSet.DataSetName = "ProcCodesDataSet"
        Me.ProcCodesDataSet.Locale = New System.Globalization.CultureInfo("en-US")
        Me.ProcCodesDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'ClearAllButton
        '
        Me.ClearAllButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ClearAllButton.Location = New System.Drawing.Point(302, 459)
        Me.ClearAllButton.Name = "ClearAllButton"
        Me.ClearAllButton.Size = New System.Drawing.Size(75, 23)
        Me.ClearAllButton.TabIndex = 16
        Me.ClearAllButton.Text = "Clear All"
        '
        'updateAllButton
        '
        Me.UpdateAllButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UpdateAllButton.Location = New System.Drawing.Point(387, 459)
        Me.UpdateAllButton.Name = "updateAllButton"
        Me.UpdateAllButton.Size = New System.Drawing.Size(75, 23)
        Me.UpdateAllButton.TabIndex = 15
        Me.UpdateAllButton.Text = "Update All"
        '
        'CancelActionButton
        '
        Me.CancelActionButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CancelActionButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelActionButton.Location = New System.Drawing.Point(12, 459)
        Me.CancelActionButton.Name = "CancelActionButton"
        Me.CancelActionButton.Size = New System.Drawing.Size(75, 23)
        Me.CancelActionButton.TabIndex = 13
        Me.CancelActionButton.Text = "Cancel"
        '
        'SaveButton
        '
        Me.UpdateLineButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UpdateLineButton.Location = New System.Drawing.Point(472, 459)
        Me.UpdateLineButton.Name = "SaveButton"
        Me.UpdateLineButton.Size = New System.Drawing.Size(75, 23)
        Me.UpdateLineButton.TabIndex = 14
        Me.UpdateLineButton.Text = "Update Line"
        '
        'ProcedureCodeLookup
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.CancelActionButton
        Me.ClientSize = New System.Drawing.Size(551, 489)
        Me.Controls.Add(Me.ClearAllButton)
        Me.Controls.Add(Me.UpdateAllButton)
        Me.Controls.Add(Me.CancelActionButton)
        Me.Controls.Add(Me.UpdateLineButton)
        Me.Controls.Add(Me.ProcedureCodeLookupDataGrid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.KeyPreview = True
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(392, 176)
        Me.Name = "ProcedureCodeLookup"
        Me.Text = "Select Procedure Code..."
        Me.TopMost = True
        CType(Me.ProcedureCodeLookupDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ProcCodesDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Constructors"
    Sub New(ByVal EffectiveDate? As Date)
        Me.New()

        _EffectiveDate = EffectiveDate
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
    Public Property EffectiveDate() As Nullable(Of Date)
        Get
            Return _EffectiveDate
        End Get
        Set(ByVal value As Nullable(Of Date))
            _EffectiveDate = value
        End Set
    End Property
    Public Property ProcCode() As Object
        Get
            Return _ProcCode
        End Get
        Set(ByVal value As Object)
            _ProcCode = value
        End Set
    End Property
    Public ReadOnly Property SelectedProcedureCodeDataRow() As DataRow
        Get
            Return _ProcedureCodesDR
        End Get
    End Property

    Public ReadOnly Property Status() As String
        Get
            Return _Status
        End Get
    End Property
#End Region

#Region "Form Events"
    Private Sub ProcedureCodesLookup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim FormText As String

        Try
            SetSettings()

            FormText = Me.Text

            Me.Text = "Loading Procedures... Please Wait"
            Me.Show()

            Task.Factory.StartNew(Sub() LoadProcedureCodes(AddressOf LoadProcedureCodesResultsHandler))

            Me.Text = FormText

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Delegate Sub RefreshProcedureCodesResultsDelegate(ProcedureCodesResults As DataSet)
    Private Sub LoadProcedureCodes(CallBack As RefreshProcedureCodesResultsDelegate)
        Try

            CallBack(CMSDALFDBMD.RetrieveProcedureValuesAsOfEffectiveDate(_EffectiveDate))

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub LoadProcedureCodesResultsHandler(ProcedureCodesResults As DataSet)

        Try

            If Me.InvokeRequired Then

                BeginInvoke(New RefreshProcedureCodesResultsDelegate(AddressOf LoadProcedureCodesDataGrid), New Object() {ProcedureCodesResults})

            Else
                LoadProcedureCodesDataGrid(ProcedureCodesResults)
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub LoadProcedureCodesDataGrid(ProcedureCodesResults As DataSet)

        _ProcedureCodesDS = ProcedureCodesResults
        _ProcedureCodesBS = New BindingSource

        _ProcedureCodesBS.DataMember = "PROCEDURE_VALUES"
        _ProcedureCodesBS.DataSource = _ProcedureCodesDS

        ProcedureCodeLookupDataGrid.SuspendLayout()
        ProcedureCodeLookupDataGrid.DataSource = _ProcedureCodesBS
        ProcedureCodeLookupDataGrid.SetTableStyle()
        ProcedureCodeLookupDataGrid.ResumeLayout()

    End Sub

    Private Sub DetailLineProcedureCodes_FormClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.FormClosing
        SaveSettings()
    End Sub

    Private Sub SetSettings()
        Me.Top = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)))
        Me.Height = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)))
        Me.Width = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)
    End Sub

    Private Sub SaveSettings()
        Dim lWindowState As FormWindowState = Me.WindowState
        SaveSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(lWindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = lWindowState

    End Sub
#End Region

    Private Sub ProcedureCodesDataGrid_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ProcedureCodeLookupDataGrid.MouseUp
        Dim DG As DataGrid
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo

        Try
            DG = CType(sender, DataGrid)
            HTI = DG.HitTest(e.X, e.Y)

            If e.Button = MouseButtons.Left Then
                Select Case HTI.Type
                    Case Is = DataGrid.HitTestType.Cell
                        If ProcedureCodeLookupDataGrid.IsSelected(HTI.Row) = True Then
                            ProcedureCodeLookupDataGrid.UnSelect(HTI.Row)
                        Else
                            ProcedureCodeLookupDataGrid.Select(HTI.Row)
                        End If
                    Case Is = DataGrid.HitTestType.RowHeader
                        If ProcedureCodeLookupDataGrid.IsSelected(HTI.Row) = False Then
                            Exit Try
                        End If
                    Case Is = DataGrid.HitTestType.None
                        If HTI.Row = -1 Then
                            Me.DialogResult = DialogResult.None
                            Exit Try
                        End If
                End Select

                If HTI.Type = DataGrid.HitTestType.Cell Or HTI.Type = DataGrid.HitTestType.RowHeader Then
                    Me.ProcCode = ProcedureCodeLookupDataGrid.Item(HTI.Row, 0).ToString()
                    ProcedureCodeLookupDataGrid.Select(HTI.Row)
                    ''Me.DialogResult = DialogResult.OK
                End If
            End If
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub ProcedureCodesDataGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ProcedureCodeLookupDataGrid.DoubleClick

        Select Case CType(sender, DataGridCustom).LastHitSpot.Type
            Case Is = System.Windows.Forms.DataGrid.HitTestType.None

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell

                _Status = "updateline"
                _ProcedureCodesDR = CType(_ProcedureCodesBS.Current, DataRowView).Row
                Me.DialogResult = DialogResult.OK

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader

                _Status = "updateline"
                _ProcedureCodesDR = CType(_ProcedureCodesBS.Current, DataRowView).Row
                Me.DialogResult = DialogResult.OK

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ParentRows

        End Select

    End Sub

    Private Sub UpdateLineButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpdateLineButton.Click

        If _ProcedureCodesBS.Position < 0 OrElse Not ProcedureCodeLookupDataGrid.IsSelected(_ProcedureCodesBS.Position) Then
            MsgBox("Select Row and then Update Button or Double Click row to select Update Line.", MsgBoxStyle.Information, "Select Procedure to Continue")
        Else
            _Status = "UPDATELINE"
            Me.DialogResult = DialogResult.OK
            _ProcedureCodesDR = CType(_ProcedureCodesBS.Current, DataRowView).Row
        End If
    End Sub

    Private Sub UpdateAllButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpdateAllButton.Click
        If _ProcedureCodesBS.Position < 0 OrElse Not ProcedureCodeLookupDataGrid.IsSelected(_ProcedureCodesBS.Position) Then
            MsgBox("Select Row and then Update Button or Double Click row to select Update Line.", MsgBoxStyle.Information, "Select Procedure to Continue")
        Else
            _Status = "UPDATEALL"
            Me.DialogResult = DialogResult.OK
            _ProcedureCodesDR = CType(_ProcedureCodesBS.Current, DataRowView).Row
        End If
    End Sub

    Private Sub ClearAllButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ClearAllButton.Click
        _Status = "CLEARALL"
        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub CancelActionButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelActionButton.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Sub ProcedureCodesDataGrid_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        Dim DT As DataTable

        Try
            If Char.IsLetterOrDigit(e.KeyChar) Then
                e.Handled = True

                If DateDiff("s", _LastKeyCapturedTime, UFCWGeneral.NowDate) > 0 Then
                    _SB = New StringBuilder
                End If

                _SB.Append(e.KeyChar.ToString())
                _LastKeyCapturedTime = UFCWGeneral.NowDate
            Else
                _SB = New StringBuilder
            End If

            If _SB.Length > 0 Then

                Try

                    DT = CType(_ProcedureCodesBS.DataSource, DataSet).Tables(_ProcedureCodesBS.DataMember)

                    DT.DefaultView.Sort = "PROC_VALUE"

                    _FoundDRs = DT.Select("PROC_VALUE LIKE '" & _SB.ToString.ToUpper & "%'", "PROC_VALUE")

                    If _FoundDRs.Length > 0 Then
                        Dim RowNum As Integer = _ProcedureCodesBS.Find("PROC_VALUE", _FoundDRs(0)("PROC_VALUE").ToString)
                        If RowNum > -1 Then _ProcedureCodesBS.Position = RowNum
                        'ProcedureCodesLookupDataGrid.Select(1)
                    End If

                Catch ex As Exception
                    Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
                    If (Rethrow) Then
                        Throw
                    End If
                End Try
            End If

            If Asc(e.KeyChar) = Keys.Enter Then ProcedureCodeLookupDataGrid.Find(_SB.ToString())

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub _ProcedureCodesBS_PositionChanged(sender As Object, e As EventArgs) Handles _ProcedureCodesBS.PositionChanged

    End Sub
End Class
#Region "BackThread Class"
Public Class ExecuteProcedureQuery

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _ProcCodeLookupForm As New ProcedureCodeLookupForm
    Private _EffectiveDate? As Date
    Private _ProcCodeDS As ProcCodesDataSet
    Private _ResultDS As DataSet

    Sub New(ByVal ds As DataSet, ByVal effectiveDate? As Date)
        _ResultDS = ds
        _EffectiveDate = effectiveDate
    End Sub
    Public Property DBResultSet() As DataSet
        Get
            Return _ResultDS
        End Get
        Set(ByVal value As DataSet)
            _ResultDS = value
        End Set
    End Property
    Public Sub Execute()
        Try

            _ResultDS = CMSDALFDBMD.RetrieveProcedureValuesAsOfEffectiveDate(_EffectiveDate, _ResultDS)
            Me.DBResultSet = _ResultDS

        Catch ex As Exception
            Throw
        End Try
    End Sub
End Class
#End Region
