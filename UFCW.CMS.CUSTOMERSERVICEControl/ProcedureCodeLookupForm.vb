Imports System.Text

Public Class ProcedureCodeLookupForm
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
    Friend WithEvents ProcedureCodesLookupDataGrid As DataGridCustom
    Friend WithEvents ProcCodesDataSet As ProcCodesDataSet
    Friend WithEvents OKButton As System.Windows.Forms.Button
    Friend WithEvents MultiTimer As System.Timers.Timer
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ProcedureCodesLookupDataGrid = New DataGridCustom()
        Me.ProcCodesDataSet = New ProcCodesDataSet()
        Me.OKButton = New System.Windows.Forms.Button()
        Me.MultiTimer = New System.Timers.Timer()
        CType(Me.ProcedureCodesLookupDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ProcCodesDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MultiTimer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ProcedureCodesLookupDataGrid
        '
        Me.ProcedureCodesLookupDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.ProcedureCodesLookupDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.ProcedureCodesLookupDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ProcedureCodesLookupDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ProcedureCodesLookupDataGrid.ADGroupsThatCanFind = ""
        Me.ProcedureCodesLookupDataGrid.ADGroupsThatCanMultiSort = ""
        Me.ProcedureCodesLookupDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ProcedureCodesLookupDataGrid.AllowAutoSize = True
        Me.ProcedureCodesLookupDataGrid.AllowColumnReorder = False
        Me.ProcedureCodesLookupDataGrid.AllowCopy = True
        Me.ProcedureCodesLookupDataGrid.AllowCustomize = True
        Me.ProcedureCodesLookupDataGrid.AllowDelete = False
        Me.ProcedureCodesLookupDataGrid.AllowDragDrop = False
        Me.ProcedureCodesLookupDataGrid.AllowEdit = False
        Me.ProcedureCodesLookupDataGrid.AllowExport = False
        Me.ProcedureCodesLookupDataGrid.AllowFilter = True
        Me.ProcedureCodesLookupDataGrid.AllowFind = True
        Me.ProcedureCodesLookupDataGrid.AllowGoTo = True
        Me.ProcedureCodesLookupDataGrid.AllowMultiSelect = True
        Me.ProcedureCodesLookupDataGrid.AllowMultiSort = False
        Me.ProcedureCodesLookupDataGrid.AllowNew = False
        Me.ProcedureCodesLookupDataGrid.AllowPrint = False
        Me.ProcedureCodesLookupDataGrid.AllowRefresh = False
        Me.ProcedureCodesLookupDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProcedureCodesLookupDataGrid.AppKey = "UFCW\Claims\"
        Me.ProcedureCodesLookupDataGrid.AutoSaveCols = True
        Me.ProcedureCodesLookupDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.ProcedureCodesLookupDataGrid.CaptionVisible = False
        Me.ProcedureCodesLookupDataGrid.ColumnHeaderLabel = Nothing
        Me.ProcedureCodesLookupDataGrid.ColumnRePositioning = False
        Me.ProcedureCodesLookupDataGrid.ColumnResizing = False
        Me.ProcedureCodesLookupDataGrid.ConfirmDelete = True
        Me.ProcedureCodesLookupDataGrid.CopySelectedOnly = True
        Me.ProcedureCodesLookupDataGrid.CurrentBSPosition = -1
        Me.ProcedureCodesLookupDataGrid.DataMember = ""
        Me.ProcedureCodesLookupDataGrid.DragColumn = 0
        Me.ProcedureCodesLookupDataGrid.ExportSelectedOnly = True
        Me.ProcedureCodesLookupDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ProcedureCodesLookupDataGrid.HighlightedRow = Nothing
        Me.ProcedureCodesLookupDataGrid.HighLightModifiedRows = False
        Me.ProcedureCodesLookupDataGrid.IsMouseDown = False
        Me.ProcedureCodesLookupDataGrid.LastGoToLine = ""
        Me.ProcedureCodesLookupDataGrid.Location = New System.Drawing.Point(8, 8)
        Me.ProcedureCodesLookupDataGrid.MultiSort = False
        Me.ProcedureCodesLookupDataGrid.Name = "ProcedureCodesLookupDataGrid"
        Me.ProcedureCodesLookupDataGrid.OldSelectedRow = Nothing
        Me.ProcedureCodesLookupDataGrid.PreviousBSPosition = -1
        Me.ProcedureCodesLookupDataGrid.ReadOnly = True
        Me.ProcedureCodesLookupDataGrid.RetainRowSelectionAfterSort = True
        Me.ProcedureCodesLookupDataGrid.SetRowOnRightClick = True
        Me.ProcedureCodesLookupDataGrid.ShiftPressed = False
        Me.ProcedureCodesLookupDataGrid.SingleClickBooleanColumns = False
        Me.ProcedureCodesLookupDataGrid.Size = New System.Drawing.Size(536, 320)
        Me.ProcedureCodesLookupDataGrid.Sort = Nothing
        Me.ProcedureCodesLookupDataGrid.StyleName = ""
        Me.ProcedureCodesLookupDataGrid.SubKey = ""
        Me.ProcedureCodesLookupDataGrid.SuppressMouseDown = False
        Me.ProcedureCodesLookupDataGrid.SuppressTriangle = False
        Me.ProcedureCodesLookupDataGrid.TabIndex = 0
        Me.ProcedureCodesLookupDataGrid.TabStop = False
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
        Me.Controls.Add(Me.ProcedureCodesLookupDataGrid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(392, 176)
        Me.Name = "ProcedureCodeLookup"
        Me.ShowInTaskbar = False
        Me.Text = "Select Procedure Code..."
        CType(Me.ProcedureCodesLookupDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
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
    Private _SBLastKeyCapturedTime As Date = UFCWGeneral.NowDate
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
            ProcedureCodesLookupDataGrid.AllowMultiSelect = True ''False

            If Not UFCWGeneral.SetFormPosition(Me, _APPKEY) Then Me.CenterToScreen()

            Dim FormText As String = Me.Text

            Me.Text = "Loading Procedures... Please Wait"
            Me.Show()
            Me.Refresh()

            LoadProcedureCodes()

            Me.Text = FormText

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DetailLineProcedureCodes_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        UFCWGeneral.SaveFormPosition(Me, _APPKEY)
    End Sub

#End Region

    Private Sub LoadProcedureCodes()
        Try

            Using WC As New GlobalCursor

                ProcedureCodesLookupDataGrid.SuspendLayout()
                ProcedureCodesLookupDataGrid.DataSource = CMSDALFDBMD.RetrieveProcedureValues().Tables("PROCEDURE_VALUES")

                ProcedureCodesLookupDataGrid.SetTableStyle()
                ProcedureCodesLookupDataGrid.ResumeLayout()

            End Using

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub ProcedureCodesDataGrid_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ProcedureCodesLookupDataGrid.MouseUp
        Dim DG As DataGrid
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo

        Try
            DG = CType(sender, DataGrid)
            HTI = DG.HitTest(e.X, e.Y)

            If e.Button = MouseButtons.Left Then
                Select Case HTI.Type
                    Case Is = DataGrid.HitTestType.Cell
                        If ProcedureCodesLookupDataGrid.IsSelected(HTI.Row) Then
                            ProcedureCodesLookupDataGrid.UnSelect(HTI.Row)
                        Else
                            ProcedureCodesLookupDataGrid.Select(HTI.Row)
                        End If
                    Case Is = DataGrid.HitTestType.RowHeader
                        If ProcedureCodesLookupDataGrid.IsSelected(HTI.Row) = False Then Return
                End Select

                If HTI.Type = DataGrid.HitTestType.Cell OrElse HTI.Type = DataGrid.HitTestType.RowHeader Then
                    ProcedureCodesLookupDataGrid.Select(HTI.Row)
                End If

            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub ProcedureCodesDataGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ProcedureCodesLookupDataGrid.DoubleClick
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

        If ProcedureCodesLookupDataGrid.GetSelectedDataRows.Count > 0 Then
            Me.DialogResult = DialogResult.OK
        Else
            Me.DialogResult = DialogResult.Cancel
        End If

    End Sub

    Private Sub ProcedureCodesDataGrid_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
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
                    Dim DT As DataTable = CType(ProcedureCodesLookupDataGrid.DataSource, DataTable)

                    Debug.WriteLine("Selecting " & "PROC_VALUE LIKE '" & _SB.ToString.ToUpper & "%'")

                    DT.DefaultView.Sort = "PROC_VALUE"

                    _FoundDR = DT.Select("PROC_VALUE LIKE '" & _SB.ToString.ToUpper & "%'", "PROC_VALUE")

                    If _FoundDR.Length > 0 Then
                        BindingContext(DT).Position = DT.DefaultView.Find(_FoundDR(0)("PROC_VALUE"))
                        ProcedureCodesLookupDataGrid.MoveGridToRow(BindingContext(DT).Position)
                        ProcedureCodesLookupDataGrid.Select(1)
                    End If

                Catch ex As Exception
                    Throw
                End Try
            End If

            If Asc(e.KeyChar) = Keys.Enter Then ProcedureCodesLookupDataGrid.Find(_SB.ToString())

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub
End Class