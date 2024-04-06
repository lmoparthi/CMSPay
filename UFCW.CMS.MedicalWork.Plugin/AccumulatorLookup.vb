Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling


Public Class AccumulatorLookup
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _AccumsDT As DataTable
    Private _ChkSelect As New DataColumn("select")
    Private _SelectedAccums As String
    Private _EntryAmt As Decimal
    Private _APPKEY As String = "UFCW\Claims\"
    Private _Selected As Integer
    Private _DubClick As Boolean = False

    Private _AccumDV As DataView

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal AvailableAccums As DataTable)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        _AccumsDT = AvailableAccums
        _ChkSelect.DataType = System.Type.GetType("System.Decimal")
        _ChkSelect.DefaultValue = 0
        _AccumsDT.Columns.Add(_ChkSelect)

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
    Friend WithEvents AddButton As System.Windows.Forms.Button
    Friend WithEvents ValueTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents CloseButton As System.Windows.Forms.Button
    Friend WithEvents MainToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents AccumulatorLookupDataGrid As DataGridCustom

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.AddButton = New System.Windows.Forms.Button()
        Me.ValueTextBox = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.MainToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.AccumulatorLookupDataGrid = New DataGridCustom()
        CType(Me.AccumulatorLookupDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AddButton
        '
        Me.AddButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AddButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.AddButton.Location = New System.Drawing.Point(440, 280)
        Me.AddButton.Name = "AddButton"
        Me.AddButton.Size = New System.Drawing.Size(65, 23)
        Me.AddButton.TabIndex = 3
        Me.AddButton.Text = "&Add"
        Me.MainToolTip.SetToolTip(Me.AddButton, "Add Apply Value to Checked Accumulators")
        '
        'ValueTextBox
        '
        Me.ValueTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ValueTextBox.Location = New System.Drawing.Point(416, 256)
        Me.ValueTextBox.MaxLength = 20
        Me.ValueTextBox.Name = "ValueTextBox"
        Me.ValueTextBox.Size = New System.Drawing.Size(88, 20)
        Me.ValueTextBox.TabIndex = 2
        Me.MainToolTip.SetToolTip(Me.ValueTextBox, "Apply Value for all Checked Accumulators")
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(376, 257)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(36, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Apply:"
        Me.MainToolTip.SetToolTip(Me.Label2, "Apply Value for all Checked Accumulators")
        '
        'CloseButton
        '
        Me.CloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CloseButton.Location = New System.Drawing.Point(8, 280)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(65, 23)
        Me.CloseButton.TabIndex = 4
        Me.CloseButton.Text = "&Cancel"
        Me.MainToolTip.SetToolTip(Me.CloseButton, "Cancel the Window")
        '
        'AccumulatorLookupDataGrid
        '
        Me.AccumulatorLookupDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.AccumulatorLookupDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.AccumulatorLookupDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.AccumulatorLookupDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.AccumulatorLookupDataGrid.ADGroupsThatCanFind = ""
        Me.AccumulatorLookupDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.AccumulatorLookupDataGrid.ADGroupsThatCanMultiSort = ""
        Me.AccumulatorLookupDataGrid.AllowAutoSize = True
        Me.AccumulatorLookupDataGrid.AllowColumnReorder = False
        Me.AccumulatorLookupDataGrid.AllowCopy = False
        Me.AccumulatorLookupDataGrid.AllowCustomize = False
        Me.AccumulatorLookupDataGrid.AllowDelete = False
        Me.AccumulatorLookupDataGrid.AllowDragDrop = False
        Me.AccumulatorLookupDataGrid.AllowEdit = True
        Me.AccumulatorLookupDataGrid.AllowExport = False
        Me.AccumulatorLookupDataGrid.AllowFilter = False
        Me.AccumulatorLookupDataGrid.AllowFind = False
        Me.AccumulatorLookupDataGrid.AllowGoTo = False
        Me.AccumulatorLookupDataGrid.AllowMultiSelect = False
        Me.AccumulatorLookupDataGrid.AllowMultiSort = False
        Me.AccumulatorLookupDataGrid.AllowNavigation = False
        Me.AccumulatorLookupDataGrid.AllowNew = False
        Me.AccumulatorLookupDataGrid.AllowPrint = False
        Me.AccumulatorLookupDataGrid.AllowRefresh = False
        Me.AccumulatorLookupDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AccumulatorLookupDataGrid.AppKey = "UFCW\Claims\"
        Me.AccumulatorLookupDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.AccumulatorLookupDataGrid.CaptionVisible = False
        Me.AccumulatorLookupDataGrid.ColumnHeaderLabel = Nothing
        Me.AccumulatorLookupDataGrid.ColumnRePositioning = False
        Me.AccumulatorLookupDataGrid.ColumnResizing = False
        Me.AccumulatorLookupDataGrid.ConfirmDelete = True
        Me.AccumulatorLookupDataGrid.CopySelectedOnly = True
        Me.AccumulatorLookupDataGrid.DataMember = ""
        Me.AccumulatorLookupDataGrid.DragColumn = 0
        Me.AccumulatorLookupDataGrid.ExportSelectedOnly = True
        Me.AccumulatorLookupDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.AccumulatorLookupDataGrid.HighlightedRow = Nothing
        Me.AccumulatorLookupDataGrid.IsMouseDown = False
        Me.AccumulatorLookupDataGrid.LastGoToLine = ""
        Me.AccumulatorLookupDataGrid.Location = New System.Drawing.Point(8, 8)
        Me.AccumulatorLookupDataGrid.MultiSort = False
        Me.AccumulatorLookupDataGrid.Name = "AccumulatorLookupDataGrid"
        Me.AccumulatorLookupDataGrid.OldSelectedRow = Nothing
        Me.AccumulatorLookupDataGrid.ReadOnly = True
        Me.AccumulatorLookupDataGrid.SetRowOnRightClick = True
        Me.AccumulatorLookupDataGrid.ShiftPressed = False
        Me.AccumulatorLookupDataGrid.SingleClickBooleanColumns = True
        Me.AccumulatorLookupDataGrid.Size = New System.Drawing.Size(488, 242)
        Me.AccumulatorLookupDataGrid.StyleName = ""
        Me.AccumulatorLookupDataGrid.SubKey = ""
        Me.AccumulatorLookupDataGrid.SuppressTriangle = False
        Me.AccumulatorLookupDataGrid.TabIndex = 8
        Me.MainToolTip.SetToolTip(Me.AccumulatorLookupDataGrid, "Check the Accumulator Code")
        '
        'AccumulatorLookup
        '
        Me.AcceptButton = Me.AddButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.CloseButton
        Me.ClientSize = New System.Drawing.Size(512, 310)
        Me.Controls.Add(Me.AccumulatorLookupDataGrid)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.ValueTextBox)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.AddButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Location = New System.Drawing.Point(20, 80)
        Me.MinimumSize = New System.Drawing.Size(392, 176)
        Me.Name = "AccumulatorLookup"
        Me.Text = "Select Accumulator(s)..."
        CType(Me.AccumulatorLookupDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

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

    Public Property entryValue() As Decimal
        Get
            Return _EntryAmt
        End Get
        Set(ByVal value As Decimal)
            _EntryAmt = value
        End Set
    End Property
    Public Property selectAccums() As String
        Get
            Return _SelectedAccums
        End Get
        Set(ByVal value As String)
            _SelectedAccums = value
        End Set
    End Property
#End Region

#Region "Form Events"
    Private Sub AccumulatorLookup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim FormText As String

        Try
            FormText = Me.Text

            SetSettings()

            Me.Text = "Loading Accumulators... Please Wait"
            Me.Show()
            Me.Refresh()
            Me.Text = FormText

            _AccumDV = New DataView(_AccumsDT, "ACTIVE_SW = true", "", DataViewRowState.CurrentRows)
            AccumulatorLookupDataGrid.DataSource = _AccumDV
            AccumulatorLookupDataGrid.SetTableStyle()

            'SetAccumTableStyle()

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub AccumulatorLookup_FormClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.FormClosing
        SaveSettings()
        If _Selected = 0 Then Me.DialogResult = DialogResult.Cancel
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

#Region "Other"
    Private Sub SetAccumTableStyle()

        Dim DGTableStyle As DataGridTableStyle
        Dim TextCol As DataGridHighlightTextBoxColumn
        Dim BoolCol As DataGridHighlightBoolColumn
        Dim CurMan As CurrencyManager

        Try
            CurMan = CType(Me.BindingContext(_AccumDV), CurrencyManager)

            DGTableStyle = New DataGridTableStyle(CurMan)
            '' DGTableStyle.MappingName = Accums.TableName
            DGTableStyle.GridColumnStyles.Clear()
            DGTableStyle.GridLineStyle = DataGridLineStyle.None

            '''' For Check box column
            BoolCol = New DataGridHighlightBoolColumn
            BoolCol.MappingName = "select"
            BoolCol.HeaderText = "Select"
            BoolCol.Width = CInt(GetSetting(_APPKEY, "MedicalWork\AccumulatorLookup\ColumnSettings", "Col " & BoolCol.MappingName, "10"))
            BoolCol.FalseValue = False
            BoolCol.TrueValue = True
            BoolCol.NullText = "False"
            BoolCol.ReadOnly = False
            DGTableStyle.GridColumnStyles.Add(BoolCol)

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "ACCUM_NAME"
            TextCol.HeaderText = "Accumulator Name"
            TextCol.Width = CInt(GetSetting(_APPKEY, "MedicalWork\AccumulatorLookup\ColumnSettings", "Col " & TextCol.MappingName, "65"))
            TextCol.NullText = ""
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "ACCUM_DESC"
            TextCol.HeaderText = "Description"
            TextCol.Width = CInt(GetSetting(_APPKEY, "MedicalWork\AccumulatorLookup\ColumnSettings", "Col " & TextCol.MappingName, "300"))
            TextCol.NullText = ""
            DGTableStyle.GridColumnStyles.Add(TextCol)

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        End Try

        Try
            AccumulatorLookupDataGrid.TableStyles.Clear()
            AccumulatorLookupDataGrid.TableStyles.Add(DGTableStyle)

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        End Try
        CurMan = Nothing
        DGTableStyle = Nothing
    End Sub

    Private Sub AddButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddButton.Click

        Dim Cnt As Integer

        Try
            If ValueTextBox.Text = "" Then
                Me.entryValue = CDec(0.0)
            Else
                Me.entryValue = CDec(ValueTextBox.Text)
            End If

            For Cnt = 0 To AccumulatorLookupDataGrid.GetGridRowCount - 1
                If CBool(AccumulatorLookupDataGrid.Item(Cnt, CInt(AccumulatorLookupDataGrid.GetColumnPosition("select")))) = True Then
                    _Selected += 1
                End If
            Next

            Me.DialogResult = DialogResult.Cancel

            If _Selected > 0 Then
                For Cnt = 0 To AccumulatorLookupDataGrid.GetGridRowCount - 1
                    If CBool(AccumulatorLookupDataGrid.Item(Cnt, CInt(AccumulatorLookupDataGrid.GetColumnPosition("select")))) = True Then
                        Me.selectAccums &= If(Me._SelectedAccums <> "", ", ", "") & _AccumDV.Item(Cnt)("ACCUM_NAME").ToString
                    End If
                Next
                Me.DialogResult = DialogResult.OK
            End If

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        End Try
    End Sub

    Private Sub CloseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseButton.Click
        Try
            Me.DialogResult = DialogResult.Cancel
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub ValueTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ValueTextBox.TextChanged
        Try
            Dim TBox As TextBox = CType(sender, TextBox)
            Dim intCnt As Integer
            Dim strTmp As String

            If IsNumeric(TBox.Text) = False AndAlso Len(TBox.Text) > 0 Then
                strTmp = TBox.Text
                For intCnt = 1 To Len(strTmp)
                    If IsNumeric(Mid(strTmp, intCnt, 1)) = False AndAlso Len(strTmp) > 0 _
                                                AndAlso Mid(strTmp, intCnt, 1) <> "." Then
                        strTmp = Replace(strTmp, Mid(strTmp, intCnt, 1), "")
                    End If
                Next
                TBox.Text = strTmp
            End If
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub AccumulatorLookupDataGrid_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles AccumulatorLookupDataGrid.MouseUp

        Dim DataGrid As DataGridCustom
        Dim hti As System.Windows.Forms.DataGrid.HitTestInfo
        Dim bm As BindingManagerBase
        Dim DR As DataRow

        Try
            DataGrid = CType(sender, DataGridCustom)
            hti = DataGrid.HitTest(e.X, e.Y)
            bm = DataGrid.BindingContext(DataGrid.DataSource, DataGrid.DataMember)
            DR = CType(bm.Current, DataRowView).Row

            If hti.Type = DataGridCustom.HitTestType.Cell AndAlso hti.Column = DataGrid.GetColumnPosition("select") Then
                Select Case Not CBool(DataGrid(hti.Row, hti.Column))
                    Case True
                        DR("select") = 1
                    Case Else
                        DR("select") = 0
                End Select
            End If

        Catch ex As Exception

            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        Finally
            _DubClick = False
        End Try

    End Sub

#End Region

End Class