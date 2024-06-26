Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Imports System.Text
Imports System.Configuration

Public Class DiagnosisLookup
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
    Friend WithEvents DiagnosisDataGrid As DataGridCustom
    Friend WithEvents MultiTimer As System.Timers.Timer
    Friend WithEvents DiagnosisValuesDataSet As DiagnosisValuesDataSet
    Friend WithEvents OKButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.DiagnosisDataGrid = New DataGridCustom()
        Me.MultiTimer = New System.Timers.Timer()
        Me.DiagnosisValuesDataSet = New DiagnosisValuesDataSet()
        Me.OKButton = New System.Windows.Forms.Button()
        CType(Me.DiagnosisDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MultiTimer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DiagnosisValuesDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DiagnosisDataGrid
        '
        Me.DiagnosisDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.DiagnosisDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.DiagnosisDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DiagnosisDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DiagnosisDataGrid.ADGroupsThatCanFind = ""
        Me.DiagnosisDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DiagnosisDataGrid.ADGroupsThatCanMultiSort = ""
        Me.DiagnosisDataGrid.AllowAutoSize = True
        Me.DiagnosisDataGrid.AllowColumnReorder = False
        Me.DiagnosisDataGrid.AllowCopy = True
        Me.DiagnosisDataGrid.AllowCustomize = False
        Me.DiagnosisDataGrid.AllowDelete = False
        Me.DiagnosisDataGrid.AllowDragDrop = False
        Me.DiagnosisDataGrid.AllowEdit = False
        Me.DiagnosisDataGrid.AllowExport = False
        Me.DiagnosisDataGrid.AllowFilter = False
        Me.DiagnosisDataGrid.AllowFind = True
        Me.DiagnosisDataGrid.AllowGoTo = True
        Me.DiagnosisDataGrid.AllowMultiSelect = True
        Me.DiagnosisDataGrid.AllowMultiSort = False
        Me.DiagnosisDataGrid.AllowNew = False
        Me.DiagnosisDataGrid.AllowPrint = False
        Me.DiagnosisDataGrid.AllowRefresh = False
        Me.DiagnosisDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DiagnosisDataGrid.AppKey = "UFCW\Claims\"
        Me.DiagnosisDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.DiagnosisDataGrid.CaptionVisible = False
        Me.DiagnosisDataGrid.ColumnHeaderLabel = Nothing
        Me.DiagnosisDataGrid.ColumnRePositioning = False
        Me.DiagnosisDataGrid.ColumnResizing = False
        Me.DiagnosisDataGrid.ConfirmDelete = True
        Me.DiagnosisDataGrid.CopySelectedOnly = True
        Me.DiagnosisDataGrid.DataMember = ""
        Me.DiagnosisDataGrid.DragColumn = 0
        Me.DiagnosisDataGrid.ExportSelectedOnly = True
        Me.DiagnosisDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DiagnosisDataGrid.HighlightedRow = Nothing
        Me.DiagnosisDataGrid.IsMouseDown = False
        Me.DiagnosisDataGrid.LastGoToLine = ""
        Me.DiagnosisDataGrid.Location = New System.Drawing.Point(8, 8)
        Me.DiagnosisDataGrid.MultiSort = False
        Me.DiagnosisDataGrid.Name = "DiagnosisDataGrid"
        Me.DiagnosisDataGrid.OldSelectedRow = Nothing
        Me.DiagnosisDataGrid.ReadOnly = True
        Me.DiagnosisDataGrid.SetRowOnRightClick = True
        Me.DiagnosisDataGrid.ShiftPressed = False
        Me.DiagnosisDataGrid.SingleClickBooleanColumns = False
        Me.DiagnosisDataGrid.Size = New System.Drawing.Size(536, 320)
        Me.DiagnosisDataGrid.StyleName = ""
        Me.DiagnosisDataGrid.SubKey = ""
        Me.DiagnosisDataGrid.SuppressTriangle = False
        Me.DiagnosisDataGrid.TabIndex = 0
        Me.DiagnosisDataGrid.TabStop = False
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
        Me.Controls.Add(Me.DiagnosisDataGrid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(392, 176)
        Me.Name = "DiagnosisLookup"
        Me.ShowInTaskbar = False
        Me.Text = "Select Diagnosis ..."
        CType(Me.DiagnosisDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MultiTimer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DiagnosisValuesDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Form Events"
    Private Sub DiagnosisLookup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            DiagnosisDataGrid.AllowMultiSelect = True ''False
            SetSettings()

            Dim FormText As String = Me.Text

            Me.Text = "Loading Diagnoses... Please Wait"
            Me.Show()
            Me.Refresh()

            LoadDiagnosis()

            Me.Text = FormText

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub DetailLineDiagnosis_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
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

    Private Sub LoadDiagnosis()
        Dim DiagnosisDS As DataSet

        Try
            Using WC As New GlobalCursor

                DiagnosisDS = CMSDALFDBMD.RetrieveDiagnosisValuesEffectiveAsOf(General.NowDate)
                DiagnosisDS.Tables(0).DefaultView.Sort = "DIAG_VALUE"

                DiagnosisDataGrid.SuspendLayout()
                DiagnosisDataGrid.DataSource = DiagnosisDS.Tables(0).DefaultView
                DiagnosisDataGrid.SetTableStyle()
                DiagnosisDataGrid.ResumeLayout()

            End Using

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        Finally
        End Try

    End Sub

    Private Sub DiagnosisDataGrid_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        Dim DT As DataTable

        Try
            If Char.IsLetterOrDigit(e.KeyChar) Then
                e.Handled = True

                If DateDiff("s", _SBLastKeyCapturedTime, General.NowDate) > 0 Then
                    _SB = New StringBuilder
                End If

                _SB.Append(e.KeyChar.ToString())
                _SBLastKeyCapturedTime = General.NowDate
            Else
                _SB = New StringBuilder
            End If

            If _SB.Length > 0 Then

                Try

                    If TypeOf DiagnosisDataGrid.DataSource Is DataView Then
                        DT = CType(DiagnosisDataGrid.DataSource, DataView).ToTable
                    Else
                        DT = CType(DiagnosisDataGrid.DataSource, DataTable)
                    End If

                    Debug.WriteLine("Selecting " & "DIAG_VALUE LIKE '" & _SB.ToString.ToUpper & "%'")

                    DT.DefaultView.Sort = "DIAG_VALUE"

                    _FoundDRs = DT.Select("DIAG_VALUE LIKE '" & _SB.ToString.ToUpper & "%'", "DIAG_VALUE")

                    If _FoundDRs.Length > 0 Then
                        BindingContext(DT).Position = DT.DefaultView.Find(_FoundDRs(0)("DIAG_VALUE"))
                        DiagnosisDataGrid.MoveGridToRow(BindingContext(DT).Position)
                        DiagnosisDataGrid.Select(1)
                    End If

                Catch ex As Exception
                    Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
                    If (Rethrow) Then
                        Throw
                    End If
                End Try
            End If

            If Asc(e.KeyChar) = Keys.Enter Then DiagnosisDataGrid.Find(_SB.ToString())

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        Finally
        End Try
    End Sub

    Private Sub DiagnosisDataGrid_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DiagnosisDataGrid.MouseUp
        Dim DG As DataGrid
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo

        Try
            DG = CType(sender, DataGrid)
            HTI = DG.HitTest(e.X, e.Y)


            If e.Button = MouseButtons.Left Then
                Select Case HTI.Type
                    Case Is = DataGrid.HitTestType.Cell
                        If DiagnosisDataGrid.IsSelected(HTI.Row) = True Then
                            DiagnosisDataGrid.UnSelect(HTI.Row)
                        Else
                            DiagnosisDataGrid.Select(HTI.Row)
                        End If
                    Case Is = DataGrid.HitTestType.RowHeader
                        If DiagnosisDataGrid.IsSelected(HTI.Row) = False Then
                            Exit Try
                        End If
                End Select

                If HTI.Type = DataGrid.HitTestType.Cell OrElse HTI.Type = DataGrid.HitTestType.RowHeader Then
                    DiagnosisDataGrid.Select(HTI.Row)
                End If
            End If
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub

    Private Sub DiagnosisDataGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles DiagnosisDataGrid.DoubleClick

        Select Case CType(sender, DataGridCustom).LastHitSpot.type
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