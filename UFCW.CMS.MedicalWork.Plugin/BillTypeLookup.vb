Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Imports System.Text
Imports System.Threading.Tasks

Public Class BillTypeLookupForm
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _EffectiveDate? As Date
    Private _BillType As Object = System.DBNull.Value
    Private _APPKEY As String = "UFCW\Claims\"
    Private _Status As String = ""

    Private _BillTypeDR As DataRow
    Private _BillTypeDS As DataSet
    Private WithEvents _BillTypeBS As BindingSource

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
    Friend WithEvents BillTypeLookupDataGrid As DataGridCustom
    Friend WithEvents ProcCodesDataSet As ProcCodesDataSet
    Friend WithEvents ClearAllButton As System.Windows.Forms.Button
    Friend WithEvents UpdateAllButton As System.Windows.Forms.Button
    Friend WithEvents CancelActionButton As System.Windows.Forms.Button
    Friend WithEvents UpdateLineButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.BillTypeLookupDataGrid = New DataGridCustom()
        Me.ProcCodesDataSet = New ProcCodesDataSet()
        Me.ClearAllButton = New System.Windows.Forms.Button()
        Me.UpdateAllButton = New System.Windows.Forms.Button()
        Me.CancelActionButton = New System.Windows.Forms.Button()
        Me.UpdateLineButton = New System.Windows.Forms.Button()
        CType(Me.BillTypeLookupDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ProcCodesDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BillTypeLookupDataGrid
        '
        Me.BillTypeLookupDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.BillTypeLookupDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.BillTypeLookupDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.BillTypeLookupDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.BillTypeLookupDataGrid.ADGroupsThatCanFind = ""
        Me.BillTypeLookupDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.BillTypeLookupDataGrid.ADGroupsThatCanMultiSort = ""
        Me.BillTypeLookupDataGrid.AllowAutoSize = True
        Me.BillTypeLookupDataGrid.AllowColumnReorder = False
        Me.BillTypeLookupDataGrid.AllowCopy = True
        Me.BillTypeLookupDataGrid.AllowCustomize = True
        Me.BillTypeLookupDataGrid.AllowDelete = False
        Me.BillTypeLookupDataGrid.AllowDragDrop = False
        Me.BillTypeLookupDataGrid.AllowEdit = False
        Me.BillTypeLookupDataGrid.AllowExport = False
        Me.BillTypeLookupDataGrid.AllowFilter = True
        Me.BillTypeLookupDataGrid.AllowFind = True
        Me.BillTypeLookupDataGrid.AllowGoTo = True
        Me.BillTypeLookupDataGrid.AllowMultiSelect = False
        Me.BillTypeLookupDataGrid.AllowMultiSort = False
        Me.BillTypeLookupDataGrid.AllowNew = False
        Me.BillTypeLookupDataGrid.AllowPrint = False
        Me.BillTypeLookupDataGrid.AllowRefresh = False
        Me.BillTypeLookupDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BillTypeLookupDataGrid.AppKey = "UFCW\Claims\"
        Me.BillTypeLookupDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.BillTypeLookupDataGrid.CaptionVisible = False
        Me.BillTypeLookupDataGrid.ColumnHeaderLabel = Nothing
        Me.BillTypeLookupDataGrid.ColumnRePositioning = False
        Me.BillTypeLookupDataGrid.ColumnResizing = False
        Me.BillTypeLookupDataGrid.ConfirmDelete = True
        Me.BillTypeLookupDataGrid.CopySelectedOnly = True
        Me.BillTypeLookupDataGrid.DataMember = ""
        Me.BillTypeLookupDataGrid.DragColumn = 0
        Me.BillTypeLookupDataGrid.ExportSelectedOnly = True
        Me.BillTypeLookupDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.BillTypeLookupDataGrid.HighlightedRow = Nothing
        Me.BillTypeLookupDataGrid.IsMouseDown = False
        Me.BillTypeLookupDataGrid.LastGoToLine = ""
        Me.BillTypeLookupDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.BillTypeLookupDataGrid.MultiSort = False
        Me.BillTypeLookupDataGrid.Name = "BillTypeLookupDataGrid"
        Me.BillTypeLookupDataGrid.OldSelectedRow = Nothing
        Me.BillTypeLookupDataGrid.ReadOnly = True
        Me.BillTypeLookupDataGrid.SetRowOnRightClick = False
        Me.BillTypeLookupDataGrid.ShiftPressed = False
        Me.BillTypeLookupDataGrid.SingleClickBooleanColumns = True
        Me.BillTypeLookupDataGrid.Size = New System.Drawing.Size(551, 451)
        Me.BillTypeLookupDataGrid.StyleName = ""
        Me.BillTypeLookupDataGrid.SubKey = ""
        Me.BillTypeLookupDataGrid.SuppressTriangle = False
        Me.BillTypeLookupDataGrid.TabIndex = 0
        Me.BillTypeLookupDataGrid.TabStop = False
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
        Me.Controls.Add(Me.BillTypeLookupDataGrid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.KeyPreview = True
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(392, 176)
        Me.Name = "ProcedureCodeLookup"
        Me.Text = "Select Procedure Code..."
        Me.TopMost = True
        CType(Me.BillTypeLookupDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
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
    Public Property BillType() As Object
        Get
            Return _BillType
        End Get
        Set(ByVal value As Object)
            _BillType = value
        End Set
    End Property
    Public ReadOnly Property SelectedBillTypeDataRow() As DataRow
        Get
            Return _BillTypeDR
        End Get
    End Property

    Public ReadOnly Property Status() As String
        Get
            Return _Status
        End Get
    End Property
#End Region

#Region "Form Events"
    Private Sub BillTypeLookup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim FormText As String

        Try
            SetSettings()

            FormText = Me.Text

            Me.Text = "Loading BillTypes... Please Wait"
            Me.Show()

            Task.Factory.StartNew(Sub() LoadBillType(AddressOf LoadBillTypeResultsHandler))

            Me.Text = FormText

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Delegate Sub RefreshBillTypeResultsDelegate(BillTypeResults As DataSet)
    Private Sub LoadBillType(CallBack As RefreshBillTypeResultsDelegate)
        Try

            CallBack(CMSDALFDBMD.RetrieveBillTypeValues(_EffectiveDate))

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        Finally
        End Try
    End Sub

    Private Sub LoadBillTypeResultsHandler(BillTypeResults As DataSet)

        Try

            If Me.InvokeRequired Then

                BeginInvoke(New RefreshBillTypeResultsDelegate(AddressOf LoadBillTypeDataGrid), New Object() {BillTypeResults})

            Else
                LoadBillTypeDataGrid(BillTypeResults)
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub LoadBillTypeDataGrid(BillTypeResults As DataSet)

        _BillTypeDS = BillTypeResults
        _BillTypeBS = New BindingSource

        _BillTypeBS.DataMember = "BILL_TYPE_VALUES"
        _BillTypeBS.DataSource = _BillTypeDS

        BillTypeLookupDataGrid.SuspendLayout()
        BillTypeLookupDataGrid.DataSource = _BillTypeBS
        BillTypeLookupDataGrid.SetTableStyle()
        BillTypeLookupDataGrid.ResumeLayout()

    End Sub

    Private Sub DetailLineBillType_FormClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.FormClosing
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

    Private Sub BillTypeDataGrid_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BillTypeLookupDataGrid.MouseUp
        Dim DG As DataGrid
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo

        Try
            DG = CType(sender, DataGrid)
            HTI = DG.HitTest(e.X, e.Y)

            If e.Button = MouseButtons.Left Then
                Select Case HTI.Type
                    Case Is = DataGrid.HitTestType.Cell
                        If BillTypeLookupDataGrid.IsSelected(HTI.Row) = True Then
                            BillTypeLookupDataGrid.UnSelect(HTI.Row)
                        Else
                            BillTypeLookupDataGrid.Select(HTI.Row)
                        End If
                    Case Is = DataGrid.HitTestType.RowHeader
                        If BillTypeLookupDataGrid.IsSelected(HTI.Row) = False Then
                            Exit Try
                        End If
                    Case Is = DataGrid.HitTestType.None
                        If HTI.Row = -1 Then
                            Me.DialogResult = DialogResult.None
                            Exit Try
                        End If
                End Select

                If HTI.Type = DataGrid.HitTestType.Cell Or HTI.Type = DataGrid.HitTestType.RowHeader Then
                    Me.BillType = BillTypeLookupDataGrid.Item(HTI.Row, 0).ToString()
                    BillTypeLookupDataGrid.Select(HTI.Row)
                    ''Me.DialogResult = DialogResult.OK
                End If
            End If
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        End Try

    End Sub

    Private Sub BillTypeDataGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles BillTypeLookupDataGrid.DoubleClick

        Select Case CType(sender, DataGridCustom).LastHitSpot.Type
            Case Is = System.Windows.Forms.DataGrid.HitTestType.None

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell

                _Status = "UPDATELINE"
                _BillTypeDR = CType(_BillTypeBS.Current, DataRowView).Row
                Me.DialogResult = DialogResult.OK

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader

                _Status = "UPDATELINE"
                _BillTypeDR = CType(_BillTypeBS.Current, DataRowView).Row
                Me.DialogResult = DialogResult.OK

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ParentRows

        End Select

    End Sub

    Private Sub UpdateLineButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpdateLineButton.Click

        If _BillTypeBS.Position < 0 OrElse Not BillTypeLookupDataGrid.IsSelected(_BillTypeBS.Position) Then
            MsgBox("Select Row and then Update Button or Double Click row to select Update Line.", MsgBoxStyle.Information, "Select Bill Type to Continue")
        Else
            _Status = "UPDATELINE"
            Me.DialogResult = DialogResult.OK
            _BillTypeDR = CType(_BillTypeBS.Current, DataRowView).Row
        End If
    End Sub

    Private Sub UpdateAllButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpdateAllButton.Click
        If _BillTypeBS.Position < 0 OrElse Not BillTypeLookupDataGrid.IsSelected(_BillTypeBS.Position) Then
            MsgBox("Select Row and then Update Button or Double Click row to select Update Line.", MsgBoxStyle.Information, "Select Bill Type to Continue")
        Else
            _Status = "UPDATEALL"
            Me.DialogResult = DialogResult.OK
            _BillTypeDR = CType(_BillTypeBS.Current, DataRowView).Row
        End If
    End Sub

    Private Sub ClearAllButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ClearAllButton.Click
        _Status = "CLEARALL"
        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub CancelActionButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelActionButton.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Sub BillTypeDataGrid_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
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

                    DT = CType(_BillTypeBS.DataSource, DataSet).Tables(_BillTypeBS.DataMember)

                    DT.DefaultView.Sort = "BILL_TYPE_VALUE"

                    _FoundDRs = DT.Select("BILL_TYPE_VALUE LIKE '" & _SB.ToString.ToUpper & "%'", "BILL_TYPE_VALUE")

                    If _FoundDRs.Length > 0 Then
                        Dim RowNum As Integer = _BillTypeBS.Find("BILL_TYPE_VALUE", _FoundDRs(0)("BILL_TYPE_VALUE").ToString)
                        If RowNum > -1 Then _BillTypeBS.Position = RowNum
                        'BillTypeLookupDataGrid.Select(1)
                    End If

                Catch ex As Exception
                    Throw
                End Try
            End If

            If Asc(e.KeyChar) = Keys.Enter Then BillTypeLookupDataGrid.Find(_SB.ToString())

        Catch ex As Exception
            Throw
        End Try
    End Sub

End Class
#Region "BackThread Class"
Public Class ExecuteBillTypeQuery

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _BillTypeLookupForm As New BillTypeLookupForm
    Private _EffectiveDate? As Date
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
            _ResultDS = CMSDALFDBMD.RetrieveBillTypeValues(_EffectiveDate, _ResultDS)
            Me.DBResultSet = _ResultDS
        Catch ex As Exception
            Throw
        End Try
    End Sub
End Class
#End Region
