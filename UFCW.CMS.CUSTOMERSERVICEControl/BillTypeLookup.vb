Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Imports System.Text

Public Class BillTypeLookup
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"
    Private _SB As New StringBuilder
    Private _SBLastKeyCapturedTime As Date = UFCWGeneral.NowDate
    Private _FoundDRs() As DataRow
    Private _BillTypeDS As DataSet

    Friend WithEvents OKButton As System.Windows.Forms.Button

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If

            If _BillTypeDS IsNot Nothing Then _BillTypeDS.Dispose()

        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.MultiTimer = New System.Timers.Timer()
        Me.BillTypeDataGrid = New DataGridCustom()
        Me.OKButton = New System.Windows.Forms.Button()
        CType(Me.MultiTimer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BillTypeDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MultiTimer
        '
        Me.MultiTimer.SynchronizingObject = Me
        '
        'BillTypeDataGrid
        '
        Me.BillTypeDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.BillTypeDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.BillTypeDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.BillTypeDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.BillTypeDataGrid.ADGroupsThatCanFind = ""
        Me.BillTypeDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.BillTypeDataGrid.ADGroupsThatCanMultiSort = ""
        Me.BillTypeDataGrid.AllowAutoSize = True
        Me.BillTypeDataGrid.AllowColumnReorder = False
        Me.BillTypeDataGrid.AllowCopy = True
        Me.BillTypeDataGrid.AllowCustomize = False
        Me.BillTypeDataGrid.AllowDelete = False
        Me.BillTypeDataGrid.AllowDragDrop = False
        Me.BillTypeDataGrid.AllowEdit = False
        Me.BillTypeDataGrid.AllowExport = False
        Me.BillTypeDataGrid.AllowFilter = False
        Me.BillTypeDataGrid.AllowFind = True
        Me.BillTypeDataGrid.AllowGoTo = True
        Me.BillTypeDataGrid.AllowMultiSelect = False
        Me.BillTypeDataGrid.AllowMultiSort = False
        Me.BillTypeDataGrid.AllowNew = False
        Me.BillTypeDataGrid.AllowPrint = False
        Me.BillTypeDataGrid.AllowRefresh = False
        Me.BillTypeDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BillTypeDataGrid.AppKey = "UFCW\Claims\"
        Me.BillTypeDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.BillTypeDataGrid.CaptionVisible = False
        Me.BillTypeDataGrid.ColumnHeaderLabel = Nothing
        Me.BillTypeDataGrid.ColumnRePositioning = False
        Me.BillTypeDataGrid.ColumnResizing = False
        Me.BillTypeDataGrid.ConfirmDelete = True
        Me.BillTypeDataGrid.CopySelectedOnly = True
        Me.BillTypeDataGrid.DataMember = ""
        Me.BillTypeDataGrid.DragColumn = 0
        Me.BillTypeDataGrid.ExportSelectedOnly = True
        Me.BillTypeDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.BillTypeDataGrid.HighlightedRow = Nothing
        Me.BillTypeDataGrid.IsMouseDown = False
        Me.BillTypeDataGrid.LastGoToLine = ""
        Me.BillTypeDataGrid.Location = New System.Drawing.Point(8, 7)
        Me.BillTypeDataGrid.MultiSort = False
        Me.BillTypeDataGrid.Name = "BillTypeDataGrid"
        Me.BillTypeDataGrid.OldSelectedRow = Nothing
        Me.BillTypeDataGrid.ReadOnly = True
        Me.BillTypeDataGrid.SetRowOnRightClick = True
        Me.BillTypeDataGrid.ShiftPressed = False
        Me.BillTypeDataGrid.SingleClickBooleanColumns = False
        Me.BillTypeDataGrid.Size = New System.Drawing.Size(368, 317)
        Me.BillTypeDataGrid.StyleName = ""
        Me.BillTypeDataGrid.SubKey = ""
        Me.BillTypeDataGrid.SuppressTriangle = False
        Me.BillTypeDataGrid.TabIndex = 0
        Me.BillTypeDataGrid.TabStop = False
        '
        'OKButton
        '
        Me.OKButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OKButton.Location = New System.Drawing.Point(301, 330)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(75, 23)
        Me.OKButton.TabIndex = 46
        Me.OKButton.Text = "OK"
        '
        'BillTypeLookup
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.OKButton
        Me.ClientSize = New System.Drawing.Size(384, 358)
        Me.Controls.Add(Me.OKButton)
        Me.Controls.Add(Me.BillTypeDataGrid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(392, 176)
        Me.Name = "BillTypeLookup"
        Me.ShowInTaskbar = False
        Me.Text = "Select BillType..."
        CType(Me.MultiTimer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BillTypeDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BillTypeDataGrid As DataGridCustom
    Friend WithEvents MultiTimer As System.Timers.Timer
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
#End Region

#Region "Form Events"

    Private Sub BilltypeLookup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            BillTypeDataGrid.AllowMultiSelect = False

            If Not UFCWGeneral.SetFormPosition(Me, _APPKEY) Then Me.CenterToScreen()

            Dim FormText As String = Me.Text

            Me.Text = "Loading BillTypes... Please Wait"
            Me.Show()
            Me.Refresh()

            LoadBilltype()

            Me.Text = FormText

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub BillTypeLookup_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        UFCWGeneral.SaveFormPosition(Me, _APPKEY)

    End Sub

#End Region

    Private Sub LoadBilltype()
        Try
            Using WC As New GlobalCursor

                _BillTypeDS = CMSDALFDBMD.RetrieveBillTypeValues(UFCWGeneral.NowDate)
                _BillTypeDS.Tables(0).TableName = "BILL_TYPE_VALUES"

                BillTypeDataGrid.SuspendLayout()
                BillTypeDataGrid.DataSource = _BillTypeDS.Tables("BILL_TYPE_VALUES")

                BillTypeDataGrid.SetTableStyle()

                BillTypeDataGrid.ResumeLayout()

            End Using


        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub BillTypeDataGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles BillTypeDataGrid.DoubleClick

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

    Private Sub BillTypeDataGrid_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BillTypeDataGrid.MouseUp
        Dim DG As DataGrid
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo

        Try
            DG = CType(sender, DataGrid)
            HTI = DG.HitTest(e.X, e.Y)

            If e.Button = MouseButtons.Left Then
                Select Case HTI.Type
                    Case Is = DataGrid.HitTestType.Cell
                        If BillTypeDataGrid.IsSelected(HTI.Row) = True Then
                            BillTypeDataGrid.UnSelect(HTI.Row)
                        Else
                            BillTypeDataGrid.Select(HTI.Row)
                        End If
                    Case Is = DataGrid.HitTestType.RowHeader
                        If BillTypeDataGrid.IsSelected(HTI.Row) = False Then
                            Exit Try
                        End If
                End Select

                If HTI.Type = DataGrid.HitTestType.Cell OrElse HTI.Type = DataGrid.HitTestType.RowHeader Then
                    BillTypeDataGrid.Select(HTI.Row)
                    ''If Control.ModifierKeys <> Keys.Control AndAlso Control.ModifierKeys <> Keys.Shift Then
                    ''    Me.DialogResult = DialogResult.OK
                    ''Else
                    ''    'MultiTimer.Enabled = True
                    ''End If
                End If
            End If
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub BillTypeDataGrid_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
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
                    DT = _BillTypeDS.Tables("BILL_TYPE_VALUES")

                    Debug.WriteLine("Selecting " & "BILL_TYPE_VALUE LIKE '" & _SB.ToString.ToUpper & "%'")

                    DT.DefaultView.Sort = "BILL_TYPE_VALUE"

                    _FoundDRs = DT.Select("BILL_TYPE_VALUE LIKE '" & _SB.ToString.ToUpper & "%'", "BILL_TYPE_VALUE")

                    If _FoundDRs.Length > 0 Then
                        BindingContext(DT).Position = DT.DefaultView.Find(_FoundDRs(0)("BILL_TYPE_VALUE"))
                        BillTypeDataGrid.MoveGridToRow(BindingContext(DT).Position)
                        BillTypeDataGrid.Select(1)
                    End If
                Catch ex As Exception
                    Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
                    If (Rethrow) Then
                        Throw
                    End If
                End Try
            End If

            If Asc(e.KeyChar) = Keys.Enter Then BillTypeDataGrid.Find(_SB.ToString())

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

End Class

