Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Imports System.Text
Imports System.Configuration

Public Class ProcedureCodeLookup
    Inherits System.Windows.Forms.Form


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
    Friend WithEvents ProcedureCodesDataGrid As DataGridCustom
    Friend WithEvents ProcCodesDataSet As ProcCodesDataSet
    Friend WithEvents OKButton As System.Windows.Forms.Button
    Friend WithEvents MultiTimer As System.Timers.Timer
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ProcedureCodesDataGrid = New DataGridCustom()
        Me.ProcCodesDataSet = New ProcCodesDataSet()
        Me.OKButton = New System.Windows.Forms.Button()
        Me.MultiTimer = New System.Timers.Timer()
        CType(Me.ProcedureCodesDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ProcCodesDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MultiTimer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ProcedureCodesDataGrid
        '
        Me.ProcedureCodesDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.ProcedureCodesDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.ProcedureCodesDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ProcedureCodesDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ProcedureCodesDataGrid.ADGroupsThatCanFind = ""
        Me.ProcedureCodesDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ProcedureCodesDataGrid.ADGroupsThatCanMultiSort = ""
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
        Me.ProcedureCodesDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProcedureCodesDataGrid.AppKey = "UFCW\Claims\"
        Me.ProcedureCodesDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.ProcedureCodesDataGrid.CaptionVisible = False
        Me.ProcedureCodesDataGrid.ColumnHeaderLabel = Nothing
        Me.ProcedureCodesDataGrid.ColumnRePositioning = False
        Me.ProcedureCodesDataGrid.ColumnResizing = False
        Me.ProcedureCodesDataGrid.ConfirmDelete = True
        Me.ProcedureCodesDataGrid.CopySelectedOnly = True
        Me.ProcedureCodesDataGrid.DataMember = ""
        Me.ProcedureCodesDataGrid.DragColumn = 0
        Me.ProcedureCodesDataGrid.ExportSelectedOnly = True
        Me.ProcedureCodesDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ProcedureCodesDataGrid.HighlightedRow = Nothing
        Me.ProcedureCodesDataGrid.IsMouseDown = False
        Me.ProcedureCodesDataGrid.LastGoToLine = ""
        Me.ProcedureCodesDataGrid.Location = New System.Drawing.Point(8, 8)
        Me.ProcedureCodesDataGrid.MultiSort = False
        Me.ProcedureCodesDataGrid.Name = "ProcedureCodesDataGrid"
        Me.ProcedureCodesDataGrid.OldSelectedRow = Nothing
        Me.ProcedureCodesDataGrid.ReadOnly = True
        Me.ProcedureCodesDataGrid.SetRowOnRightClick = True
        Me.ProcedureCodesDataGrid.ShiftPressed = False
        Me.ProcedureCodesDataGrid.SingleClickBooleanColumns = False
        Me.ProcedureCodesDataGrid.Size = New System.Drawing.Size(536, 320)
        Me.ProcedureCodesDataGrid.StyleName = ""
        Me.ProcedureCodesDataGrid.SubKey = ""
        Me.ProcedureCodesDataGrid.SuppressTriangle = False
        Me.ProcedureCodesDataGrid.TabIndex = 0
        Me.ProcedureCodesDataGrid.TabStop = False
        '
        'ProcCodesDataSet
        '
        Me.ProcCodesDataSet.DataSetName = "ProcCodesDataSet"
        Me.ProcCodesDataSet.Locale = New System.Globalization.CultureInfo("en-US")
        '
        'OKButton
        '
        Me.OKButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OKButton.Location = New System.Drawing.Point(464, 333)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(75, 23)
        Me.OKButton.TabIndex = 43
        Me.OKButton.Text = "OK"
        '
        'MultiTimer
        '
        Me.MultiTimer.SynchronizingObject = Me
        '
        'ProcedureCodeLookup
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.OKButton
        Me.ClientSize = New System.Drawing.Size(552, 358)
        Me.Controls.Add(Me.OKButton)
        Me.Controls.Add(Me.ProcedureCodesDataGrid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(392, 176)
        Me.Name = "ProcedureCodeLookup"
        Me.ShowInTaskbar = False
        Me.Text = "Select Procedure Code..."
        CType(Me.ProcedureCodesDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ProcCodesDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MultiTimer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Constructors"
    Sub New(ByVal effectiveDate As Date?)
        Me.New()

        _EffectiveDate = effectiveDate
    End Sub
#End Region

#Region "Properties"
    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _SB As New StringBuilder
    Private _SBLastKeyCapturedTime As Date = General.NowDate
    Private _FoundDR() As DataRow
    Private _DT As DataTable
    Private _EffectiveDate As Date?
    Private _APPKEY As String = "UFCW\Claims\"

    Public Property EffectiveDate() As Date?
        Get
            Return _EffectiveDate
        End Get
        Set(ByVal value As Date?)
            _EffectiveDate = value
        End Set
    End Property

    <System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)

            _APPKEY = value
        End Set
    End Property

#End Region

#Region "Form Events"
    Private Sub ProcedureCodesLookup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            SetSettings()
            ProcedureCodesDataGrid.AllowMultiSelect = True ''False

            Dim FormText As String = Me.Text

            Me.Text = "Loading Procedures... Please Wait"
            Me.Show()
            Me.Refresh()

            LoadProcedureCodes()

            Me.Text = FormText

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub DetailLineProcedureCodes_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
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

    Private Sub LoadProcedureCodes()
        Try

            Using WC As New GlobalCursor

                ProcedureCodesDataGrid.SuspendLayout()
                ProcedureCodesDataGrid.DataSource = CMSDALFDBMD.RetrieveProcedureValues().Tables("PROCEDURE_VALUES")

                ProcedureCodesDataGrid.SetTableStyle()
                ProcedureCodesDataGrid.ResumeLayout()

            End Using

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        Finally
        End Try
    End Sub

    Private Sub ProcedureCodesDataGrid_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ProcedureCodesDataGrid.MouseUp
        Dim DG As DataGrid
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo

        Try
            DG = CType(sender, DataGrid)
            HTI = DG.HitTest(e.X, e.Y)

            If e.Button = MouseButtons.Left Then
                Select Case HTI.Type
                    Case Is = DataGrid.HitTestType.Cell
                        If ProcedureCodesDataGrid.IsSelected(HTI.Row) Then
                            ProcedureCodesDataGrid.UnSelect(HTI.Row)
                        Else
                            ProcedureCodesDataGrid.Select(HTI.Row)
                        End If
                    Case Is = DataGrid.HitTestType.RowHeader
                        If ProcedureCodesDataGrid.IsSelected(HTI.Row) = False Then
                            Exit Try
                        End If
                End Select

                If HTI.Type = DataGrid.HitTestType.Cell OrElse HTI.Type = DataGrid.HitTestType.RowHeader Then
                    ProcedureCodesDataGrid.Select(HTI.Row)
                End If
            End If

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        End Try

    End Sub

    Private Sub ProcedureCodesDataGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ProcedureCodesDataGrid.DoubleClick
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

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub ProcedureCodesDataGrid_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
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
                    Dim DT As DataTable = CType(ProcedureCodesDataGrid.DataSource, DataTable)

                    Debug.WriteLine("Selecting " & "PROC_VALUE LIKE '" & _SB.ToString.ToUpper & "%'")

                    dt.DefaultView.Sort = "PROC_VALUE"

                    _FoundDR = dt.Select("PROC_VALUE LIKE '" & _SB.ToString.ToUpper & "%'", "PROC_VALUE")

                    If _FoundDR.Length > 0 Then
                        BindingContext(dt).Position = dt.DefaultView.Find(_FoundDR(0)("PROC_VALUE"))
                        ProcedureCodesDataGrid.MoveGridToRow(BindingContext(dt).Position)
                        ProcedureCodesDataGrid.Select(1)
                    End If

                Catch ex As Exception
                    Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
                    If (rethrow) Then
                        Throw
                    End If
                End Try
            End If

            If Asc(e.KeyChar) = Keys.Enter Then ProcedureCodesDataGrid.Find(_SB.ToString())

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        Finally
        End Try
    End Sub
End Class