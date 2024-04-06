Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Imports System.Text
Imports System.Threading.Tasks

Public Class POSLookupForm
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _EffectiveDate? As Date
    Private _POS As Object = System.DBNull.Value
    Private _APPKEY As String = "UFCW\Claims\"
    Private _Status As String = ""

    Private _POSDR As DataRow
    Private _POSDS As DataSet
    Private WithEvents _POSBS As BindingSource

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
    Friend WithEvents POSLookupDataGrid As DataGridCustom
    Friend WithEvents ClearAllButton As System.Windows.Forms.Button
    Friend WithEvents UpdateAllButton As System.Windows.Forms.Button
    Friend WithEvents CancelActionButton As System.Windows.Forms.Button
    Friend WithEvents UpdateLineButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.POSLookupDataGrid = New DataGridCustom()
        Me.ClearAllButton = New System.Windows.Forms.Button()
        Me.UpdateAllButton = New System.Windows.Forms.Button()
        Me.CancelActionButton = New System.Windows.Forms.Button()
        Me.UpdateLineButton = New System.Windows.Forms.Button()
        CType(Me.POSLookupDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'POSLookupDataGrid
        '
        Me.POSLookupDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.POSLookupDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.POSLookupDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.POSLookupDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.POSLookupDataGrid.ADGroupsThatCanFind = ""
        Me.POSLookupDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.POSLookupDataGrid.ADGroupsThatCanMultiSort = ""
        Me.POSLookupDataGrid.AllowAutoSize = True
        Me.POSLookupDataGrid.AllowColumnReorder = False
        Me.POSLookupDataGrid.AllowCopy = True
        Me.POSLookupDataGrid.AllowCustomize = True
        Me.POSLookupDataGrid.AllowDelete = False
        Me.POSLookupDataGrid.AllowDragDrop = False
        Me.POSLookupDataGrid.AllowEdit = False
        Me.POSLookupDataGrid.AllowExport = False
        Me.POSLookupDataGrid.AllowFilter = True
        Me.POSLookupDataGrid.AllowFind = True
        Me.POSLookupDataGrid.AllowGoTo = True
        Me.POSLookupDataGrid.AllowMultiSelect = False
        Me.POSLookupDataGrid.AllowMultiSort = False
        Me.POSLookupDataGrid.AllowNew = False
        Me.POSLookupDataGrid.AllowPrint = False
        Me.POSLookupDataGrid.AllowRefresh = False
        Me.POSLookupDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.POSLookupDataGrid.AppKey = "UFCW\Claims\"
        Me.POSLookupDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.POSLookupDataGrid.CaptionVisible = False
        Me.POSLookupDataGrid.ColumnHeaderLabel = Nothing
        Me.POSLookupDataGrid.ColumnRePositioning = False
        Me.POSLookupDataGrid.ColumnResizing = False
        Me.POSLookupDataGrid.ConfirmDelete = True
        Me.POSLookupDataGrid.CopySelectedOnly = True
        Me.POSLookupDataGrid.DataMember = ""
        Me.POSLookupDataGrid.DragColumn = 0
        Me.POSLookupDataGrid.ExportSelectedOnly = True
        Me.POSLookupDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.POSLookupDataGrid.HighlightedRow = Nothing
        Me.POSLookupDataGrid.IsMouseDown = False
        Me.POSLookupDataGrid.LastGoToLine = ""
        Me.POSLookupDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.POSLookupDataGrid.MultiSort = False
        Me.POSLookupDataGrid.Name = "POSLookupDataGrid"
        Me.POSLookupDataGrid.OldSelectedRow = Nothing
        Me.POSLookupDataGrid.ReadOnly = True
        Me.POSLookupDataGrid.SetRowOnRightClick = False
        Me.POSLookupDataGrid.ShiftPressed = False
        Me.POSLookupDataGrid.SingleClickBooleanColumns = True
        Me.POSLookupDataGrid.Size = New System.Drawing.Size(551, 451)
        Me.POSLookupDataGrid.StyleName = ""
        Me.POSLookupDataGrid.SubKey = ""
        Me.POSLookupDataGrid.SuppressTriangle = False
        Me.POSLookupDataGrid.TabIndex = 0
        Me.POSLookupDataGrid.TabStop = False
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
        'POSLookup
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.CancelActionButton
        Me.ClientSize = New System.Drawing.Size(551, 489)
        Me.Controls.Add(Me.ClearAllButton)
        Me.Controls.Add(Me.UpdateAllButton)
        Me.Controls.Add(Me.CancelActionButton)
        Me.Controls.Add(Me.UpdateLineButton)
        Me.Controls.Add(Me.POSLookupDataGrid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.KeyPreview = True
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(392, 176)
        Me.Name = "POSLookup"
        Me.Text = "Select Place of Service..."
        Me.TopMost = True
        CType(Me.POSLookupDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
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
    Public Property POS() As Object
        Get
            Return _POS
        End Get
        Set(ByVal value As Object)
            _POS = value
        End Set
    End Property
    Public ReadOnly Property SelectedPOSDataRow() As DataRow
        Get
            Return _POSDR
        End Get
    End Property

    Public ReadOnly Property Status() As String
        Get
            Return _Status
        End Get
    End Property
#End Region

#Region "Form Events"
    Private Sub POSLookup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim FormText As String

        Try
            SetSettings()

            FormText = Me.Text

            Me.Text = "Loading Place of Service... Please Wait"
            Me.Show()

            Task.Factory.StartNew(Sub() LoadPOS(AddressOf LoadPOSResultsHandler))

            Me.Text = FormText

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        End Try
    End Sub

    Delegate Sub RefreshPOSResultsDelegate(POSResults As DataSet)
    Private Sub LoadPOS(CallBack As RefreshPOSResultsDelegate)
        Try

            CallBack(CMSDALFDBMD.RetrievePlaceOfServiceValues(_EffectiveDate))

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        Finally
        End Try
    End Sub

    Private Sub LoadPOSResultsHandler(POSResults As DataSet)

        Try

            If Me.InvokeRequired Then

                BeginInvoke(New RefreshPOSResultsDelegate(AddressOf LoadPOSDataGrid), New Object() {POSResults})

            Else
                LoadPOSDataGrid(POSResults)
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub LoadPOSDataGrid(POSResults As DataSet)

        _POSDS = POSResults
        _POSBS = New BindingSource

        _POSBS.DataMember = "PLACE_OF_SERV_VALUES"
        _POSBS.DataSource = _POSDS

        POSLookupDataGrid.SuspendLayout()
        POSLookupDataGrid.DataSource = _POSBS
        POSLookupDataGrid.SetTableStyle()
        POSLookupDataGrid.ResumeLayout()

    End Sub

    Private Sub DetailLinePOS_FormClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.FormClosing
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

    Private Sub POSDataGrid_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles POSLookupDataGrid.MouseUp
        Dim DG As DataGrid
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo

        Try
            DG = CType(sender, DataGrid)
            HTI = DG.HitTest(e.X, e.Y)

            If e.Button = MouseButtons.Left Then
                Select Case HTI.Type
                    Case Is = DataGrid.HitTestType.Cell
                        If POSLookupDataGrid.IsSelected(HTI.Row) = True Then
                            POSLookupDataGrid.UnSelect(HTI.Row)
                        Else
                            POSLookupDataGrid.Select(HTI.Row)
                        End If
                    Case Is = DataGrid.HitTestType.RowHeader
                        If POSLookupDataGrid.IsSelected(HTI.Row) = False Then
                            Exit Try
                        End If
                    Case Is = DataGrid.HitTestType.None
                        If HTI.Row = -1 Then
                            Me.DialogResult = DialogResult.None
                            Exit Try
                        End If
                End Select

                If HTI.Type = DataGrid.HitTestType.Cell Or HTI.Type = DataGrid.HitTestType.RowHeader Then
                    _POS = POSLookupDataGrid.Item(HTI.Row, 0).ToString()
                    POSLookupDataGrid.Select(HTI.Row)
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

    Private Sub POSDataGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles POSLookupDataGrid.DoubleClick

        Select Case CType(sender, DataGridCustom).LastHitSpot.Type
            Case Is = System.Windows.Forms.DataGrid.HitTestType.None

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell

                _Status = "UPDATELINE"
                _POSDR = CType(_POSBS.Current, DataRowView).Row
                Me.DialogResult = DialogResult.OK

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader

                _Status = "UPDATELINE"
                _POSDR = CType(_POSBS.Current, DataRowView).Row
                Me.DialogResult = DialogResult.OK

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ParentRows

        End Select

    End Sub

    Private Sub UpdateLineButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpdateLineButton.Click

        If _POSBS.Position < 0 OrElse Not POSLookupDataGrid.IsSelected(_POSBS.Position) Then
            MsgBox("Select Row and then Update Button or Double Click row to select Update Line.", MsgBoxStyle.Information, "Select Place of Service to Continue")
        Else
            _Status = "UPDATELINE"
            Me.DialogResult = DialogResult.OK
            _POSDR = CType(_POSBS.Current, DataRowView).Row
        End If
    End Sub

    Private Sub UpdateAllButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpdateAllButton.Click
        If _POSBS.Position < 0 OrElse Not POSLookupDataGrid.IsSelected(_POSBS.Position) Then
            MsgBox("Select Row and then Update Button or Double Click row to select Update Line.", MsgBoxStyle.Information, "Select Place of Service to Continue")
        Else
            _Status = "UPDATEALL"
            Me.DialogResult = DialogResult.OK
            _POSDR = CType(_POSBS.Current, DataRowView).Row
        End If
    End Sub

    Private Sub ClearAllButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ClearAllButton.Click
        _Status = "CLEARALL"
        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub CancelActionButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelActionButton.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Sub POSDataGrid_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
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

                    DT = CType(_POSBS.DataSource, DataSet).Tables(_POSBS.DataMember)

                    DT.DefaultView.Sort = "PLACE_OF_SERV_VALUE"

                    _FoundDRs = DT.Select("PLACE_OF_SERV_VALUE LIKE '" & _SB.ToString.ToUpper & "%'", "PLACE_OF_SERV_VALUE")

                    If _FoundDRs.Length > 0 Then
                        Dim RowNum As Integer = _POSBS.Find("PLACE_OF_SERV_VALUE", _FoundDRs(0)("PLACE_OF_SERV_VALUE").ToString)
                        If RowNum > -1 Then _POSBS.Position = RowNum
                        'POSLookupDataGrid.Select(1)
                    End If

                Catch ex As Exception
                    Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
                    If (Rethrow) Then
                        Throw
                    End If
                End Try
            End If

            If Asc(e.KeyChar) = Keys.Enter Then POSLookupDataGrid.Find(_SB.ToString())

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        Finally
        End Try
    End Sub

End Class
#Region "BackThread Class"
Public Class ExecutePOSQuery

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _POSLookupForm As New POSLookupForm
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

            _ResultDS = CMSDALFDBMD.RetrievePlaceOfServiceValues(_EffectiveDate, _ResultDS)
            Me.DBResultSet = _ResultDS

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        End Try
    End Sub
End Class
#End Region
