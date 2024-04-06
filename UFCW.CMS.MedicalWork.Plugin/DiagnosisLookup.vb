Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Imports System.Threading
Imports System.Text

Public Class DiagnosisLookupForm
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
    Private _APPKEY As String = "UFCW\Claims\"

    Private Shared _DiagDV As DataView
    Private Shared _DateOfService As Date?

    Private _EQ As EProcedureQuery
    Private _DiagnosisDS As DataSet
    Private WithEvents _DiagBS As BindingSource
    Friend WithEvents CancelFormButton As System.Windows.Forms.Button

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
    Friend WithEvents DiagnosisLookupDataGrid As DataGridCustom
    Friend WithEvents MultiTimer As System.Timers.Timer
    Friend WithEvents DiagnosisValuesDataSet As DiagnosisValuesDataSet
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.DiagnosisLookupDataGrid = New DataGridCustom()
        Me.MultiTimer = New System.Timers.Timer()
        Me.DiagnosisValuesDataSet = New DiagnosisValuesDataSet()
        Me.CancelFormButton = New System.Windows.Forms.Button()
        CType(Me.DiagnosisLookupDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MultiTimer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DiagnosisValuesDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DiagnosisLookupDataGrid
        '
        Me.DiagnosisLookupDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.DiagnosisLookupDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.DiagnosisLookupDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DiagnosisLookupDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DiagnosisLookupDataGrid.ADGroupsThatCanFind = ""
        Me.DiagnosisLookupDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DiagnosisLookupDataGrid.ADGroupsThatCanMultiSort = ""
        Me.DiagnosisLookupDataGrid.AllowAutoSize = True
        Me.DiagnosisLookupDataGrid.AllowColumnReorder = False
        Me.DiagnosisLookupDataGrid.AllowCopy = True
        Me.DiagnosisLookupDataGrid.AllowCustomize = True
        Me.DiagnosisLookupDataGrid.AllowDelete = False
        Me.DiagnosisLookupDataGrid.AllowDragDrop = False
        Me.DiagnosisLookupDataGrid.AllowEdit = False
        Me.DiagnosisLookupDataGrid.AllowExport = False
        Me.DiagnosisLookupDataGrid.AllowFilter = True
        Me.DiagnosisLookupDataGrid.AllowFind = True
        Me.DiagnosisLookupDataGrid.AllowGoTo = True
        Me.DiagnosisLookupDataGrid.AllowMultiSelect = True
        Me.DiagnosisLookupDataGrid.AllowMultiSort = False
        Me.DiagnosisLookupDataGrid.AllowNew = False
        Me.DiagnosisLookupDataGrid.AllowPrint = False
        Me.DiagnosisLookupDataGrid.AllowRefresh = False
        Me.DiagnosisLookupDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DiagnosisLookupDataGrid.AppKey = "UFCW\Claims\"
        Me.DiagnosisLookupDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.DiagnosisLookupDataGrid.CaptionVisible = False
        Me.DiagnosisLookupDataGrid.ColumnHeaderLabel = Nothing
        Me.DiagnosisLookupDataGrid.ColumnRePositioning = False
        Me.DiagnosisLookupDataGrid.ColumnResizing = False
        Me.DiagnosisLookupDataGrid.ConfirmDelete = True
        Me.DiagnosisLookupDataGrid.CopySelectedOnly = True
        Me.DiagnosisLookupDataGrid.DataMember = ""
        Me.DiagnosisLookupDataGrid.DragColumn = 0
        Me.DiagnosisLookupDataGrid.ExportSelectedOnly = True
        Me.DiagnosisLookupDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DiagnosisLookupDataGrid.HighlightedRow = Nothing
        Me.DiagnosisLookupDataGrid.IsMouseDown = False
        Me.DiagnosisLookupDataGrid.LastGoToLine = ""
        Me.DiagnosisLookupDataGrid.Location = New System.Drawing.Point(8, 8)
        Me.DiagnosisLookupDataGrid.MultiSort = False
        Me.DiagnosisLookupDataGrid.Name = "DiagnosisLookupDataGrid"
        Me.DiagnosisLookupDataGrid.OldSelectedRow = Nothing
        Me.DiagnosisLookupDataGrid.ReadOnly = True
        Me.DiagnosisLookupDataGrid.SetRowOnRightClick = True
        Me.DiagnosisLookupDataGrid.ShiftPressed = False
        Me.DiagnosisLookupDataGrid.SingleClickBooleanColumns = True
        Me.DiagnosisLookupDataGrid.Size = New System.Drawing.Size(536, 319)
        Me.DiagnosisLookupDataGrid.StyleName = ""
        Me.DiagnosisLookupDataGrid.SubKey = ""
        Me.DiagnosisLookupDataGrid.SuppressTriangle = False
        Me.DiagnosisLookupDataGrid.TabIndex = 0
        '
        'MultiTimer
        '
        Me.MultiTimer.SynchronizingObject = Me
        '
        'DiagnosisValuesDataSet
        '
        Me.DiagnosisValuesDataSet.DataSetName = "DiagnosisValuesDataSet"
        Me.DiagnosisValuesDataSet.Locale = New System.Globalization.CultureInfo("en-US")
        Me.DiagnosisValuesDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'CancelFormButton
        '
        Me.CancelFormButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CancelFormButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelFormButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CancelFormButton.Location = New System.Drawing.Point(470, 333)
        Me.CancelFormButton.Name = "CancelFormButton"
        Me.CancelFormButton.Size = New System.Drawing.Size(74, 23)
        Me.CancelFormButton.TabIndex = 23
        Me.CancelFormButton.Text = "Cancel"
        '
        'DiagnosisLookup
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.CancelFormButton
        Me.ClientSize = New System.Drawing.Size(552, 358)
        Me.Controls.Add(Me.CancelFormButton)
        Me.Controls.Add(Me.DiagnosisLookupDataGrid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(392, 176)
        Me.Name = "DiagnosisLookup"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "Select Diagnosis(Diagnoses)..."
        CType(Me.DiagnosisLookupDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MultiTimer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DiagnosisValuesDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Form Events"
    Private sb As New StringBuilder
    Private sbLastKeyCapturedTime As Date = UFCWGeneral.NowDate
    Private foundRow() As DataRow

    Private Sub ProcedureCodesDataGrid_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        Try
            If Char.IsLetterOrDigit(e.KeyChar) Then
                e.Handled = True

                If DateDiff("s", sbLastKeyCapturedTime, UFCWGeneral.NowDate) > 0 Then
                    sb = New StringBuilder
                End If

                sb.Append(e.KeyChar.ToString())
                sbLastKeyCapturedTime = UFCWGeneral.NowDate
            Else
                sb = New StringBuilder
            End If

            If sb.Length > 0 Then

                Try

                    Dim DT As DataTable = CType(DiagnosisLookupDataGrid.DataSource, DataTable)

                    Debug.WriteLine("Selecting " & "DIAG_VALUE LIKE '" & sb.ToString.ToUpper & "%'")

                    DT.DefaultView.Sort = "DIAG_VALUE"

                    foundRow = DT.Select("DIAG_VALUE LIKE '" & sb.ToString.ToUpper & "%'", "DIAG_VALUE")

                    If foundRow.Length > 0 Then
                        BindingContext(DT).Position = DT.DefaultView.Find(foundRow(0)("DIAG_VALUE"))
                        DiagnosisLookupDataGrid.MoveGridToRow(BindingContext(DT).Position)
                        DiagnosisLookupDataGrid.Select(1)
                    End If

                Catch ex As Exception
                    Throw
                End Try
            End If

            If Asc(e.KeyChar) = Keys.Enter Then DiagnosisLookupDataGrid.Find(sb.ToString())

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        Finally
        End Try
    End Sub

    Private Sub DiagnosisLookup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            SetSettings()

            Dim FormText As String = Me.Text

            Me.Text = "Loading Diagnoses... Please Wait"

            LoadDiagnosis()

            Me.Show()

            Me.Text = FormText

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub DetailLineDiagnosis_FormClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.FormClosing
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

    Public Property DateOfService() As Date ''UFCW.CMS.UI.DiagnosisValuesDataSet
        Get
            Return CDate(_DateOfService)
        End Get
        Set(ByVal value As Date)
            _DateOfService = value
        End Set
    End Property
    Public Property DiagnosisDV() As DataView ''UFCW.CMS.UI.DiagnosisValuesDataSet
        Get
            Return _DiagDV
        End Get
        Set(ByVal value As DataView)
            _DiagDV = value
        End Set
    End Property
#End Region

    Private Sub LoadDiagnosis()

        Dim WorkerThread As Thread
        Try

            If _DateOfService Is Nothing Then
                _DateOfService = UFCWGeneral.NowDate
            End If

            _EQ = New EProcedureQuery(CDate(_DateOfService))

            WorkerThread = New Thread(AddressOf _EQ.Execute)
            WorkerThread.IsBackground = True
            WorkerThread.Start()
            Thread.Sleep(0)
            WorkerThread.Join()

            _DiagnosisDS = _EQ.DBResultSet
            _DiagBS = New BindingSource With {
                .DataSource = _DiagnosisDS.Tables(0),
                .Sort = "DIAG_VALUE"
            }
            DiagnosisLookupDataGrid.SuspendLayout()
            DiagnosisLookupDataGrid.DataSource = _DiagBS
            DiagnosisLookupDataGrid.SetTableStyle()

            DiagnosisLookupDataGrid.ResumeLayout()

        Catch ex As Exception
            Throw
        Finally
            _EQ = Nothing
            WorkerThread = Nothing
        End Try
    End Sub

    Private Sub SetDiagnosisTableStyle(ByVal DataGrid As DataGridCustom)
        Dim DGTableStyle As DataGridTableStyle
        Dim TextCol As DataGridHighlightTextBoxColumn
        Dim BoolCol As DataGridHighlightBoolColumn
        Dim coltxtbx As DataGridTextBox

        Try

            DGTableStyle = New DataGridTableStyle()
            DGTableStyle.MappingName = _DiagnosisDS.Tables("DIAGNOSIS_VALUES").TableName

            DGTableStyle.GridLineStyle = DataGridLineStyle.None

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "DIAG_VALUE"
            TextCol.HeaderText = "Code"
            TextCol.Width = CInt(GetSetting(_APPKEY, DataGrid.Name & "\" & DGTableStyle.MappingName.ToString & "\ColumnSettings", "Col " & TextCol.MappingName, CStr(UFCWGeneral.MeasureWidthinPixels(6, DataGrid.Font.Name, DataGrid.Font.Size))))
            TextCol.NullText = ""
            coltxtbx = CType(TextCol.TextBox, DataGridTextBox)
            AddHandler coltxtbx.KeyDown, AddressOf GridKeyDownEvent
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "SHORT_DESC"
            TextCol.HeaderText = "Short Description"
            TextCol.Width = CInt(GetSetting(_APPKEY, DataGrid.Name & "\" & DGTableStyle.MappingName.ToString & "\ColumnSettings", "Col " & TextCol.MappingName, CStr(UFCWGeneral.MeasureWidthinPixels(40, DataGrid.Font.Name, DataGrid.Font.Size))))
            TextCol.NullText = ""
            coltxtbx = CType(TextCol.TextBox, DataGridTextBox)
            AddHandler coltxtbx.KeyDown, AddressOf GridKeyDownEvent
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "FULL_DESC"
            TextCol.HeaderText = "Full Description"
            TextCol.Width = CInt(GetSetting(_APPKEY, DataGrid.Name & "\" & DGTableStyle.MappingName.ToString & "\ColumnSettings", "Col " & TextCol.MappingName, CStr(UFCWGeneral.MeasureWidthinPixels(162, DataGrid.Font.Name, DataGrid.Font.Size))))
            TextCol.NullText = ""
            coltxtbx = CType(TextCol.TextBox, DataGridTextBox)
            AddHandler coltxtbx.KeyDown, AddressOf GridKeyDownEvent
            DGTableStyle.GridColumnStyles.Add(TextCol)

            BoolCol = New DataGridHighlightBoolColumn
            BoolCol.MappingName = "PREVENTATIVE_USE_SW"
            BoolCol.HeaderText = "Preventative"
            BoolCol.Width = CInt(GetSetting(_APPKEY, DataGrid.Name & "\" & DGTableStyle.MappingName.ToString & "\ColumnSettings", "Col " & TextCol.MappingName, CStr(UFCWGeneral.MeasureWidthinPixels(11, DataGrid.Font.Name, DataGrid.Font.Size))))
            BoolCol.NullText = "0"
            BoolCol.TrueValue = CType("1", Decimal)
            BoolCol.FalseValue = CType("0", Decimal)
            BoolCol.AllowNull = False
            DGTableStyle.GridColumnStyles.Add(BoolCol)

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "SOURCE"
            TextCol.HeaderText = "CodeSet"
            TextCol.Width = CInt(GetSetting(_APPKEY, DataGrid.Name & "\" & DGTableStyle.MappingName.ToString & "\ColumnSettings", "Col " & TextCol.MappingName, CStr(UFCWGeneral.MeasureWidthinPixels(162, DataGrid.Font.Name, DataGrid.Font.Size))))
            TextCol.NullText = ""
            coltxtbx = CType(TextCol.TextBox, DataGridTextBox)
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "VALIDITY"
            TextCol.HeaderText = "Validity"
            TextCol.Width = CInt(GetSetting(_APPKEY, DataGrid.Name & "\" & DGTableStyle.MappingName.ToString & "\ColumnSettings", "Col " & TextCol.MappingName, CStr(UFCWGeneral.MeasureWidthinPixels(162, DataGrid.Font.Name, DataGrid.Font.Size))))
            TextCol.NullText = ""
            coltxtbx = CType(TextCol.TextBox, DataGridTextBox)
            DGTableStyle.GridColumnStyles.Add(TextCol)

            DataGrid.TableStyles.Clear()
            DataGrid.TableStyles.Add(DGTableStyle)

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        Finally
            DGTableStyle = Nothing
        End Try

    End Sub

    Private Sub DiagnosisDataGrid_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DiagnosisLookupDataGrid.MouseUp
        Dim DG As DataGrid
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo

        Try
            DG = CType(sender, DataGrid)
            HTI = DG.HitTest(e.X, e.Y)

            If e.Button = MouseButtons.Left Then
                Select Case HTI.Type
                    Case Is = DataGrid.HitTestType.Cell
                        If DiagnosisLookupDataGrid.IsSelected(HTI.Row) = True Then
                            DiagnosisLookupDataGrid.UnSelect(HTI.Row)
                        Else
                            DiagnosisLookupDataGrid.Select(HTI.Row)
                        End If
                    Case Is = DataGrid.HitTestType.RowHeader
                        If DiagnosisLookupDataGrid.IsSelected(HTI.Row) = False Then
                            Exit Try
                        End If
                End Select

                If HTI.Type = DataGrid.HitTestType.Cell OrElse HTI.Type = DataGrid.HitTestType.RowHeader Then
                    If Control.ModifierKeys <> Keys.Control AndAlso Control.ModifierKeys <> Keys.Shift Then
                        Me.DialogResult = DialogResult.OK
                    Else
                        MultiTimer.Enabled = True
                    End If
                End If
            End If
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        End Try

    End Sub

    Private Sub MultiTimer_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles MultiTimer.Elapsed
        If Control.ModifierKeys <> Keys.Control AndAlso Control.ModifierKeys <> Keys.Shift Then
            Dim Selected As Boolean = False

            For cnt As Integer = 0 To DiagnosisLookupDataGrid.GetGridRowCount - 1
                If DiagnosisLookupDataGrid.IsSelected(cnt) Then
                    Selected = True
                    Exit For
                End If
            Next

            If Selected AndAlso DiagnosisLookupDataGrid.LastHitSpot.Type = DataGrid.HitTestType.RowHeader Then
                MultiTimer.Enabled = False
                Me.DialogResult = DialogResult.OK
            End If
        End If
    End Sub

    Private Sub GridKeyDownEvent(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DiagnosisLookupDataGrid.KeyDown
        If e.Control OrElse e.Shift Then
            MultiTimer.Enabled = True
        End If
    End Sub

    Private Sub CancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelFormButton.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
    End Sub
End Class
#Region "BackThread Class"
Public Class EProcedureQuery

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _DiagnosisDS As DataSet
    Private _DateOfService As Date

    'Sub New(ByVal DiagnosisValuesDataSet As DiagnosisValuesDataSet)
    '    DiagnosisDataSet = DiagnosisValuesDataSet
    'End Sub
    Sub New(ByVal EffectiveDate As Date)
        _DateOfService = EffectiveDate
    End Sub
    Public ReadOnly Property DBResultSet() As DataSet
        Get
            Return _DiagnosisDS
        End Get
    End Property

    Public Sub Execute()
        Try
            _DiagnosisDS = CMSDALFDBMD.RetrieveDiagnosisValuesEffectiveAsOf(_DateOfService)
            'Me.DBResultSet = dsResult
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        End Try
    End Sub
End Class
#End Region