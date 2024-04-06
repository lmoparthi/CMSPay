Imports System.Text

Public Class PlaceOfServiceLookup
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"
    Private _SB As New StringBuilder
    Private _SBLastKeyCapturedTime As Date = UFCWGeneral.NowDate
    Private _FoundDR() As DataRow
    Friend WithEvents OKButton As System.Windows.Forms.Button
    Private _POSDS As DataSet

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
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If

            If _POSDS IsNot Nothing Then _POSDS.Dispose()
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
        Me.PlaceOfServiceDataGrid = New DataGridCustom()
        Me.OKButton = New System.Windows.Forms.Button()
        CType(Me.PlaceOfServiceDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PlaceOfServiceDataGrid
        '
        Me.PlaceOfServiceDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.PlaceOfServiceDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.PlaceOfServiceDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PlaceOfServiceDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PlaceOfServiceDataGrid.ADGroupsThatCanFind = ""
        Me.PlaceOfServiceDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PlaceOfServiceDataGrid.ADGroupsThatCanMultiSort = ""
        Me.PlaceOfServiceDataGrid.AllowAutoSize = True
        Me.PlaceOfServiceDataGrid.AllowColumnReorder = False
        Me.PlaceOfServiceDataGrid.AllowCopy = True
        Me.PlaceOfServiceDataGrid.AllowCustomize = False
        Me.PlaceOfServiceDataGrid.AllowDelete = False
        Me.PlaceOfServiceDataGrid.AllowDragDrop = False
        Me.PlaceOfServiceDataGrid.AllowEdit = False
        Me.PlaceOfServiceDataGrid.AllowExport = False
        Me.PlaceOfServiceDataGrid.AllowFilter = False
        Me.PlaceOfServiceDataGrid.AllowFind = True
        Me.PlaceOfServiceDataGrid.AllowGoTo = True
        Me.PlaceOfServiceDataGrid.AllowMultiSelect = False
        Me.PlaceOfServiceDataGrid.AllowMultiSort = False
        Me.PlaceOfServiceDataGrid.AllowNew = False
        Me.PlaceOfServiceDataGrid.AllowPrint = False
        Me.PlaceOfServiceDataGrid.AllowRefresh = False
        Me.PlaceOfServiceDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PlaceOfServiceDataGrid.AppKey = "UFCW\Claims\"
        Me.PlaceOfServiceDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.PlaceOfServiceDataGrid.CaptionVisible = False
        Me.PlaceOfServiceDataGrid.ColumnHeaderLabel = Nothing
        Me.PlaceOfServiceDataGrid.ColumnRePositioning = False
        Me.PlaceOfServiceDataGrid.ColumnResizing = False
        Me.PlaceOfServiceDataGrid.ConfirmDelete = True
        Me.PlaceOfServiceDataGrid.CopySelectedOnly = True
        Me.PlaceOfServiceDataGrid.DataMember = ""
        Me.PlaceOfServiceDataGrid.DragColumn = 0
        Me.PlaceOfServiceDataGrid.ExportSelectedOnly = True
        Me.PlaceOfServiceDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.PlaceOfServiceDataGrid.HighlightedRow = Nothing
        Me.PlaceOfServiceDataGrid.IsMouseDown = False
        Me.PlaceOfServiceDataGrid.LastGoToLine = ""
        Me.PlaceOfServiceDataGrid.Location = New System.Drawing.Point(8, 3)
        Me.PlaceOfServiceDataGrid.MultiSort = False
        Me.PlaceOfServiceDataGrid.Name = "PlaceOfServiceDataGrid"
        Me.PlaceOfServiceDataGrid.OldSelectedRow = Nothing
        Me.PlaceOfServiceDataGrid.ReadOnly = True
        Me.PlaceOfServiceDataGrid.SetRowOnRightClick = True
        Me.PlaceOfServiceDataGrid.ShiftPressed = False
        Me.PlaceOfServiceDataGrid.SingleClickBooleanColumns = False
        Me.PlaceOfServiceDataGrid.Size = New System.Drawing.Size(536, 312)
        Me.PlaceOfServiceDataGrid.StyleName = ""
        Me.PlaceOfServiceDataGrid.SubKey = ""
        Me.PlaceOfServiceDataGrid.SuppressTriangle = False
        Me.PlaceOfServiceDataGrid.TabIndex = 0
        Me.PlaceOfServiceDataGrid.TabStop = False
        '
        'OKButton
        '
        Me.OKButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OKButton.Location = New System.Drawing.Point(469, 321)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(75, 23)
        Me.OKButton.TabIndex = 47
        Me.OKButton.Text = "OK"
        '
        'PlaceOfServiceLookup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.OKButton
        Me.ClientSize = New System.Drawing.Size(552, 350)
        Me.Controls.Add(Me.OKButton)
        Me.Controls.Add(Me.PlaceOfServiceDataGrid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.KeyPreview = True
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(392, 176)
        Me.Name = "PlaceOfServiceLookup"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "Select Place Of Service..."
        CType(Me.PlaceOfServiceDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PlaceOfServiceDataGrid As DataGridCustom
    Friend WithEvents MultiTimer As System.Timers.Timer
#End Region

#Region "Form Events"

    Private Sub PlaceofserviceLookup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            PlaceOfServiceDataGrid.AllowMultiSelect = False

            If Not UFCWGeneral.SetFormPosition(Me, _APPKEY) Then Me.CenterToScreen()

            Dim FormText As String = Me.Text

            Me.Text = "Loading PlaceOfServices... Please Wait"
            Me.Show()
            Me.Refresh()

            LoadPlaceofservice()

            Me.Text = FormText

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub PlaceofserviceLookup_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        UFCWGeneral.SaveFormPosition(Me, _APPKEY)
    End Sub

#End Region

    Private Sub LoadPlaceofservice()
        Try
            Using WC As New GlobalCursor

                PlaceOfServiceDataGrid.SuspendLayout()

                _POSDS = CMSDALFDBMD.RetrievePlaceOfServiceValues(UFCWGeneral.NowDate)
                _POSDS.Tables(0).TableName = "PLACE_OF_SERVICE_VALUES"

                PlaceOfServiceDataGrid.DataSource = _POSDS.Tables("PLACE_OF_SERVICE_VALUES")

                PlaceOfServiceDataGrid.SetTableStyle()
                PlaceOfServiceDataGrid.ResumeLayout()

            End Using

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub PlaceOfServiceDataGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles PlaceOfServiceDataGrid.DoubleClick
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

    Private Sub PlaceOfServiceDataGrid_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PlaceOfServiceDataGrid.MouseUp
        Dim DG As DataGrid
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo

        Try
            DG = CType(sender, DataGrid)
            HTI = DG.HitTest(e.X, e.Y)

            If e.Button = MouseButtons.Left Then
                Select Case HTI.Type
                    Case Is = DataGrid.HitTestType.Cell
                        If PlaceOfServiceDataGrid.IsSelected(HTI.Row) Then
                            PlaceOfServiceDataGrid.UnSelect(HTI.Row)
                        Else
                            PlaceOfServiceDataGrid.Select(HTI.Row)
                        End If
                    Case Is = DataGrid.HitTestType.RowHeader
                        If PlaceOfServiceDataGrid.IsSelected(HTI.Row) = False Then
                            Exit Try
                        End If
                End Select

                If HTI.Type = DataGrid.HitTestType.Cell Or HTI.Type = DataGrid.HitTestType.RowHeader Then
                    PlaceOfServiceDataGrid.Select(HTI.Row)
                End If
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub PlaceOfServiceDataGrid_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        Try
            If Char.IsLetterOrDigit(e.KeyChar) Then
                e.Handled = True

                If DateDiff("s", _SBLastKeyCapturedTime, UFCWGeneral.NowDate) > 0 Then
                    _SB = New StringBuilder
                End If

                _SB.Append(e.KeyChar)
                _SBLastKeyCapturedTime = UFCWGeneral.NowDate
            Else
                _SB = New StringBuilder
            End If

            If _SB.Length > 0 Then

                Try
                    Dim DT As DataTable = _POSDS.Tables("PLACE_OF_SERVICE_VALUES")

                    dt.DefaultView.Sort = "PLACE_OF_SERV_VALUE"

                    _FoundDR = dt.Select("PLACE_OF_SERV_VALUE LIKE '" & _SB.ToString.ToUpper & "%'", "PLACE_OF_SERV_VALUE")

                    If _FoundDR.Length > 0 Then
                        BindingContext(dt).Position = dt.DefaultView.Find(_FoundDR(0)("PLACE_OF_SERV_VALUE"))
                        PlaceOfServiceDataGrid.MoveGridToRow(BindingContext(dt).Position)
                        PlaceOfServiceDataGrid.Select(1)
                    End If
                Catch ex As Exception
                    Throw
                End Try
            End If

            If Asc(e.KeyChar) = Keys.Enter Then PlaceOfServiceDataGrid.Find(_SB.ToString())

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

End Class