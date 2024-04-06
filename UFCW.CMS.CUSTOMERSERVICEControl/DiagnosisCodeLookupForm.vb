Imports System.Text

Public Class DiagnosisCodeLookupForm
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"

    Private _SB As New StringBuilder
    Private _SBLastKeyCapturedTime As Date = UFCWGeneral.NowDate
    Private _FoundDRs() As DataRow

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
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents DiagnosisCodesLookupDataGrid As DataGridCustom
    Friend WithEvents MultiTimer As System.Timers.Timer
    Friend WithEvents DiagnosisValuesDataSet As DiagnosisValuesDataSet
    Friend WithEvents OKButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.DiagnosisCodesLookupDataGrid = New DataGridCustom()
        Me.MultiTimer = New System.Timers.Timer()
        Me.DiagnosisValuesDataSet = New DiagnosisValuesDataSet()
        Me.OKButton = New System.Windows.Forms.Button()
        CType(Me.DiagnosisCodesLookupDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MultiTimer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DiagnosisValuesDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DiagnosisDataGrid
        '
        Me.DiagnosisCodesLookupDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.DiagnosisCodesLookupDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.DiagnosisCodesLookupDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DiagnosisCodesLookupDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DiagnosisCodesLookupDataGrid.ADGroupsThatCanFind = ""
        Me.DiagnosisCodesLookupDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DiagnosisCodesLookupDataGrid.ADGroupsThatCanMultiSort = ""
        Me.DiagnosisCodesLookupDataGrid.AllowAutoSize = True
        Me.DiagnosisCodesLookupDataGrid.AllowColumnReorder = False
        Me.DiagnosisCodesLookupDataGrid.AllowCopy = True
        Me.DiagnosisCodesLookupDataGrid.AllowCustomize = False
        Me.DiagnosisCodesLookupDataGrid.AllowDelete = False
        Me.DiagnosisCodesLookupDataGrid.AllowDragDrop = False
        Me.DiagnosisCodesLookupDataGrid.AllowEdit = False
        Me.DiagnosisCodesLookupDataGrid.AllowExport = False
        Me.DiagnosisCodesLookupDataGrid.AllowFilter = False
        Me.DiagnosisCodesLookupDataGrid.AllowFind = True
        Me.DiagnosisCodesLookupDataGrid.AllowGoTo = True
        Me.DiagnosisCodesLookupDataGrid.AllowMultiSelect = True
        Me.DiagnosisCodesLookupDataGrid.AllowMultiSort = False
        Me.DiagnosisCodesLookupDataGrid.AllowNew = False
        Me.DiagnosisCodesLookupDataGrid.AllowPrint = False
        Me.DiagnosisCodesLookupDataGrid.AllowRefresh = False
        Me.DiagnosisCodesLookupDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DiagnosisCodesLookupDataGrid.AppKey = "UFCW\Claims\"
        Me.DiagnosisCodesLookupDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.DiagnosisCodesLookupDataGrid.CaptionVisible = False
        Me.DiagnosisCodesLookupDataGrid.ColumnHeaderLabel = Nothing
        Me.DiagnosisCodesLookupDataGrid.ColumnRePositioning = False
        Me.DiagnosisCodesLookupDataGrid.ColumnResizing = False
        Me.DiagnosisCodesLookupDataGrid.ConfirmDelete = True
        Me.DiagnosisCodesLookupDataGrid.CopySelectedOnly = True
        Me.DiagnosisCodesLookupDataGrid.DataMember = ""
        Me.DiagnosisCodesLookupDataGrid.DragColumn = 0
        Me.DiagnosisCodesLookupDataGrid.ExportSelectedOnly = True
        Me.DiagnosisCodesLookupDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DiagnosisCodesLookupDataGrid.HighlightedRow = Nothing
        Me.DiagnosisCodesLookupDataGrid.IsMouseDown = False
        Me.DiagnosisCodesLookupDataGrid.LastGoToLine = ""
        Me.DiagnosisCodesLookupDataGrid.Location = New System.Drawing.Point(8, 8)
        Me.DiagnosisCodesLookupDataGrid.MultiSort = False
        Me.DiagnosisCodesLookupDataGrid.Name = "DiagnosisCodesLookupDataGrid"
        Me.DiagnosisCodesLookupDataGrid.OldSelectedRow = Nothing
        Me.DiagnosisCodesLookupDataGrid.ReadOnly = True
        Me.DiagnosisCodesLookupDataGrid.SetRowOnRightClick = True
        Me.DiagnosisCodesLookupDataGrid.ShiftPressed = False
        Me.DiagnosisCodesLookupDataGrid.SingleClickBooleanColumns = False
        Me.DiagnosisCodesLookupDataGrid.Size = New System.Drawing.Size(536, 320)
        Me.DiagnosisCodesLookupDataGrid.StyleName = ""
        Me.DiagnosisCodesLookupDataGrid.SubKey = ""
        Me.DiagnosisCodesLookupDataGrid.SuppressTriangle = False
        Me.DiagnosisCodesLookupDataGrid.TabIndex = 0
        Me.DiagnosisCodesLookupDataGrid.TabStop = False
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
        'OKButton
        '
        Me.OKButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OKButton.Location = New System.Drawing.Point(467, 332)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(75, 23)
        Me.OKButton.TabIndex = 44
        Me.OKButton.Text = "OK"
        '
        'DiagnosisLookup
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.OKButton
        Me.ClientSize = New System.Drawing.Size(552, 358)
        Me.Controls.Add(Me.OKButton)
        Me.Controls.Add(Me.DiagnosisCodesLookupDataGrid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(392, 176)
        Me.Name = "DiagnosisCodeLookup"
        Me.ShowInTaskbar = False
        Me.Text = "Select Diagnosis ..."
        CType(Me.DiagnosisCodesLookupDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MultiTimer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DiagnosisValuesDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Form Events"
    Private Sub DiagnosisLookup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            DiagnosisCodesLookupDataGrid.AllowMultiSelect = True ''False

            If Not UFCWGeneral.SetFormPosition(Me, _APPKEY) Then Me.CenterToScreen()

            Dim FormText As String = Me.Text

            Me.Text = "Loading Diagnoses... Please Wait"
            Me.Show()
            Me.Refresh()

            LoadDiagnosis()

            Me.Text = FormText

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DetailLineDiagnosis_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        UFCWGeneral.SaveFormPosition(Me)
    End Sub

#End Region

    Private Sub LoadDiagnosis()
        Dim DiagnosisDS As DataSet

        Try
            Using WC As New GlobalCursor

                DiagnosisDS = CMSDALFDBMD.RetrieveDiagnosisValuesEffectiveAsOf(UFCWGeneral.NowDate)
                DiagnosisDS.Tables(0).DefaultView.Sort = "DIAG_VALUE"

                DiagnosisCodesLookupDataGrid.SuspendLayout()
                DiagnosisCodesLookupDataGrid.DataSource = DiagnosisDS.Tables(0).DefaultView
                DiagnosisCodesLookupDataGrid.SetTableStyle()
                DiagnosisCodesLookupDataGrid.ResumeLayout()

            End Using

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub DiagnosisDataGrid_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        Dim DT As DataTable

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

                    If TypeOf DiagnosisCodesLookupDataGrid.DataSource Is DataView Then
                        DT = CType(DiagnosisCodesLookupDataGrid.DataSource, DataView).ToTable
                    Else
                        DT = CType(DiagnosisCodesLookupDataGrid.DataSource, DataTable)
                    End If

                    Debug.WriteLine("Selecting " & "DIAG_VALUE LIKE '" & _SB.ToString.ToUpper & "%'")

                    DT.DefaultView.Sort = "DIAG_VALUE"

                    _FoundDRs = DT.Select("DIAG_VALUE LIKE '" & _SB.ToString.ToUpper & "%'", "DIAG_VALUE")

                    If _FoundDRs.Length > 0 Then
                        BindingContext(DT).Position = DT.DefaultView.Find(_FoundDRs(0)("DIAG_VALUE"))
                        DiagnosisCodesLookupDataGrid.MoveGridToRow(BindingContext(DT).Position)
                        DiagnosisCodesLookupDataGrid.Select(1)
                    End If

                Catch ex As Exception
                    Throw
                End Try
            End If

            If Asc(e.KeyChar) = Keys.Enter Then DiagnosisCodesLookupDataGrid.Find(_SB.ToString())

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub DiagnosisDataGrid_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DiagnosisCodesLookupDataGrid.MouseUp
        Dim DG As DataGrid
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo

        Try
            DG = CType(sender, DataGrid)
            HTI = DG.HitTest(e.X, e.Y)


            If e.Button = MouseButtons.Left Then
                Select Case HTI.Type
                    Case Is = DataGrid.HitTestType.Cell
                        If DiagnosisCodesLookupDataGrid.IsSelected(HTI.Row) = True Then
                            DiagnosisCodesLookupDataGrid.UnSelect(HTI.Row)
                        Else
                            DiagnosisCodesLookupDataGrid.Select(HTI.Row)
                        End If
                    Case Is = DataGrid.HitTestType.RowHeader
                        If DiagnosisCodesLookupDataGrid.IsSelected(HTI.Row) = False Then
                            Exit Try
                        End If
                End Select

                If HTI.Type = DataGrid.HitTestType.Cell OrElse HTI.Type = DataGrid.HitTestType.RowHeader Then
                    DiagnosisCodesLookupDataGrid.Select(HTI.Row)
                End If
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub DiagnosisDataGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles DiagnosisCodesLookupDataGrid.DoubleClick

        Select Case CType(sender, DataGridCustom).LastHitSpot.Type
            Case Is = System.Windows.Forms.DataGrid.HitTestType.None

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell
                Me.DialogResult = DialogResult.OK
            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader
                Me.DialogResult = DialogResult.OK
            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ParentRows

        End Select

    End Sub

End Class