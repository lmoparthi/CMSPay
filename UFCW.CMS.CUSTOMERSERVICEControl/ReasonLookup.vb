Option Strict On

Public Class ReasonLookup
    Inherits System.Windows.Forms.Form

    Friend WithEvents CancelFormButton As System.Windows.Forms.Button

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"
    Private _ReasonDS As DataSet

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

            If _ReasonDS IsNot Nothing Then _ReasonDS.Dispose()

        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents ReasonsDataGrid As DataGridCustom
    Friend WithEvents ReasonValuesDataSet As ReasonValuesDataSet
    Friend WithEvents MultiTimer As System.Timers.Timer
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ReasonsDataGrid = New DataGridCustom()
        Me.ReasonValuesDataSet = New ReasonValuesDataSet()
        Me.MultiTimer = New System.Timers.Timer()
        Me.CancelFormButton = New System.Windows.Forms.Button()
        CType(Me.ReasonsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ReasonValuesDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MultiTimer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ReasonsDataGrid
        '
        Me.ReasonsDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.ReasonsDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.ReasonsDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ReasonsDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ReasonsDataGrid.ADGroupsThatCanFind = ""
        Me.ReasonsDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ReasonsDataGrid.ADGroupsThatCanMultiSort = ""
        Me.ReasonsDataGrid.AllowAutoSize = True
        Me.ReasonsDataGrid.AllowColumnReorder = False
        Me.ReasonsDataGrid.AllowCopy = True
        Me.ReasonsDataGrid.AllowCustomize = True
        Me.ReasonsDataGrid.AllowDelete = False
        Me.ReasonsDataGrid.AllowDragDrop = False
        Me.ReasonsDataGrid.AllowEdit = False
        Me.ReasonsDataGrid.AllowExport = False
        Me.ReasonsDataGrid.AllowFilter = True
        Me.ReasonsDataGrid.AllowFind = True
        Me.ReasonsDataGrid.AllowGoTo = True
        Me.ReasonsDataGrid.AllowMultiSelect = True
        Me.ReasonsDataGrid.AllowMultiSort = False
        Me.ReasonsDataGrid.AllowNew = False
        Me.ReasonsDataGrid.AllowPrint = False
        Me.ReasonsDataGrid.AllowRefresh = False
        Me.ReasonsDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ReasonsDataGrid.AppKey = "UFCW\Claims\"
        Me.ReasonsDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.ReasonsDataGrid.CaptionVisible = False
        Me.ReasonsDataGrid.ColumnHeaderLabel = Nothing
        Me.ReasonsDataGrid.ColumnRePositioning = False
        Me.ReasonsDataGrid.ColumnResizing = False
        Me.ReasonsDataGrid.ConfirmDelete = True
        Me.ReasonsDataGrid.CopySelectedOnly = True
        Me.ReasonsDataGrid.DataMember = ""
        Me.ReasonsDataGrid.DragColumn = 0
        Me.ReasonsDataGrid.ExportSelectedOnly = True
        Me.ReasonsDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ReasonsDataGrid.HighlightedRow = Nothing
        Me.ReasonsDataGrid.IsMouseDown = False
        Me.ReasonsDataGrid.LastGoToLine = ""
        Me.ReasonsDataGrid.Location = New System.Drawing.Point(8, 8)
        Me.ReasonsDataGrid.MultiSort = False
        Me.ReasonsDataGrid.Name = "ReasonsDataGrid"
        Me.ReasonsDataGrid.OldSelectedRow = Nothing
        Me.ReasonsDataGrid.ReadOnly = True
        Me.ReasonsDataGrid.SetRowOnRightClick = True
        Me.ReasonsDataGrid.ShiftPressed = False
        Me.ReasonsDataGrid.SingleClickBooleanColumns = True
        Me.ReasonsDataGrid.Size = New System.Drawing.Size(496, 264)
        Me.ReasonsDataGrid.StyleName = ""
        Me.ReasonsDataGrid.SubKey = ""
        Me.ReasonsDataGrid.SuppressTriangle = False
        Me.ReasonsDataGrid.TabIndex = 0
        '
        'ReasonValuesDataSet
        '
        Me.ReasonValuesDataSet.DataSetName = "ReasonValuesDataSet"
        Me.ReasonValuesDataSet.Locale = New System.Globalization.CultureInfo("en-US")
        Me.ReasonValuesDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'MultiTimer
        '
        Me.MultiTimer.SynchronizingObject = Me
        '
        'CancelFormButton
        '
        Me.CancelFormButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CancelFormButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelFormButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CancelFormButton.Location = New System.Drawing.Point(430, 278)
        Me.CancelFormButton.Name = "CancelFormButton"
        Me.CancelFormButton.Size = New System.Drawing.Size(74, 23)
        Me.CancelFormButton.TabIndex = 22
        Me.CancelFormButton.Text = "Cancel"
        '
        'ReasonLookup
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.CancelFormButton
        Me.ClientSize = New System.Drawing.Size(512, 310)
        Me.Controls.Add(Me.CancelFormButton)
        Me.Controls.Add(Me.ReasonsDataGrid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(392, 176)
        Me.Name = "ReasonLookup"
        Me.ShowInTaskbar = False
        Me.Text = "Select Reason(s)..."
        CType(Me.ReasonsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ReasonValuesDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MultiTimer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region
#Region "Public Properties"
    <System.ComponentModel.Description("Represents the application key used when accessing the registry.")>
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
    Private Sub ReasonLookup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim FormText As String = Me.Text

            If Not UFCWGeneral.SetFormPosition(Me, _APPKEY) Then Me.CenterToScreen()

            Me.Text = "Loading Reasons... Please Wait"
            Me.Show()
            Me.Refresh()

            LoadReasons()

            Me.Text = FormText

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DetailLineReasons_FormClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.FormClosing

        UFCWGeneral.SaveFormPosition(Me, _APPKEY)

    End Sub

#End Region

    Private Sub LoadReasons()
        Try
            _ReasonDS = CMSDALFDBMD.RetrieveReasonValues(Nothing, _ReasonDS)

            ReasonsDataGrid.SuspendLayout()
            ReasonsDataGrid.DataSource = _ReasonDS.Tables("REASON_VALUES")
            SetReasonsTableStyle(ReasonsDataGrid)
            ReasonsDataGrid.ResumeLayout()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SetReasonsTableStyle(ByVal DataGrid As DataGridCustom)
        Dim DGTableStyle As DataGridTableStyle
        Dim TextCol As DataGridHighlightTextBoxColumn
        Dim ColTBox As DataGridTextBox

        Try

            DGTableStyle = New DataGridTableStyle()
            DGTableStyle.MappingName = _ReasonDS.Tables("REASON_VALUES").TableName

            DGTableStyle.GridLineStyle = DataGridLineStyle.None

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "REASON"
            TextCol.HeaderText = "Code"
            TextCol.Width = CInt(GetSetting(_APPKEY, DataGrid.Name & "\" & DGTableStyle.MappingName.ToString & "\ColumnSettings", "Col " & TextCol.MappingName, CStr(UFCWGeneral.MeasureWidthinPixels(6, DataGrid.Font.Name, DataGrid.Font.Size))))
            TextCol.NullText = ""
            ColTBox = CType(TextCol.TextBox, DataGridTextBox)
            AddHandler ColTBox.KeyDown, AddressOf GridKeyDownEvent
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "DESCRIPTION"
            TextCol.HeaderText = "Description"
            TextCol.Width = CInt(GetSetting(_APPKEY, DataGrid.Name & "\" & DGTableStyle.MappingName.ToString & "\ColumnSettings", "Col " & TextCol.MappingName, CStr(UFCWGeneral.MeasureWidthinPixels(115, DataGrid.Font.Name, DataGrid.Font.Size))))
            TextCol.NullText = ""
            ColTBox = CType(TextCol.TextBox, DataGridTextBox)
            AddHandler ColTBox.KeyDown, AddressOf GridKeyDownEvent
            DGTableStyle.GridColumnStyles.Add(TextCol)

            DataGrid.TableStyles.Clear()
            DataGrid.TableStyles.Add(DGTableStyle)

        Catch ex As Exception
            Throw
        Finally
            DGTableStyle = Nothing
        End Try

    End Sub

    Private Sub ReasonsDataGrid_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ReasonsDataGrid.MouseUp
        Dim DG As DataGrid
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo

        Try
            DG = CType(sender, DataGrid)
            HTI = DG.HitTest(e.X, e.Y)

            If e.Button = MouseButtons.Left Then
                Select Case HTI.Type
                    Case Is = DataGrid.HitTestType.Cell
                        If ReasonsDataGrid.IsSelected(HTI.Row) Then
                            ReasonsDataGrid.UnSelect(HTI.Row)
                        Else
                            ReasonsDataGrid.Select(HTI.Row)
                        End If
                    Case Is = DataGrid.HitTestType.RowHeader
                        If ReasonsDataGrid.IsSelected(HTI.Row) = False Then
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
            Throw
        End Try

    End Sub

    Private Sub MultiTimer_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles MultiTimer.Elapsed
        If Control.ModifierKeys <> Keys.Control AndAlso Control.ModifierKeys <> Keys.Shift Then
            Dim Selected As Boolean = False

            For cnt As Integer = 0 To ReasonsDataGrid.GetGridRowCount - 1
                If ReasonsDataGrid.IsSelected(cnt) = True Then
                    Selected = True

                    Exit For
                End If
            Next

            If Selected AndAlso ReasonsDataGrid.LastHitSpot.Type = DataGrid.HitTestType.RowHeader Then
                MultiTimer.Enabled = False
                Me.DialogResult = DialogResult.OK
            End If
        End If
    End Sub

    Private Sub GridKeyDownEvent(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ReasonsDataGrid.KeyDown
        If e.Control OrElse e.Shift Then
            MultiTimer.Enabled = True
        End If
    End Sub

    Private Sub CancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelFormButton.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
    End Sub
End Class

