Public Class DTDialog
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
    Private _DT As DataTable

    Friend WithEvents CancelFormButton As System.Windows.Forms.Button

    Private _APPKEY As String = "UFCW\Claims\"

#Region " Windows Form Designer generated code "
    Public Sub New(ByVal DT As DataTable, ByVal DialogTitle As String)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _DT = DT

        DG.Name = DT.TableName & "DataGrid"
        DG.DataSource = _DT
        DG.SetTableStyle()

        Me.Text = DialogTitle

    End Sub

    Public Sub New(ByVal DT As DataTable)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _DT = DT

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
    Friend WithEvents DG As DataGridCustom
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.DG = New DataGridCustom()
        Me.CancelFormButton = New System.Windows.Forms.Button()
        CType(Me.DG, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DG
        '
        Me.DG.ADGroupsThatCanCopy = "CMSUsers"
        Me.DG.ADGroupsThatCanCustomize = "CMSUsers"
        Me.DG.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DG.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DG.ADGroupsThatCanFind = ""
        Me.DG.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DG.ADGroupsThatCanMultiSort = ""
        Me.DG.AllowAutoSize = True
        Me.DG.AllowColumnReorder = True
        Me.DG.AllowCopy = False
        Me.DG.AllowCustomize = True
        Me.DG.AllowDelete = False
        Me.DG.AllowDragDrop = False
        Me.DG.AllowEdit = False
        Me.DG.AllowExport = False
        Me.DG.AllowFilter = True
        Me.DG.AllowFind = True
        Me.DG.AllowGoTo = True
        Me.DG.AllowMultiSelect = False
        Me.DG.AllowMultiSort = True
        Me.DG.AllowNew = False
        Me.DG.AllowPrint = False
        Me.DG.AllowRefresh = False
        Me.DG.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DG.AppKey = "UFCW\Claims\"
        Me.DG.BackgroundColor = System.Drawing.SystemColors.Window
        Me.DG.CaptionVisible = False
        Me.DG.ColumnHeaderLabel = Nothing
        Me.DG.ColumnRePositioning = False
        Me.DG.ColumnResizing = False
        Me.DG.ConfirmDelete = True
        Me.DG.CopySelectedOnly = True
        Me.DG.DataMember = ""
        Me.DG.DragColumn = 0
        Me.DG.ExportSelectedOnly = True
        Me.DG.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DG.HighlightedRow = Nothing
        Me.DG.IsMouseDown = False
        Me.DG.LastGoToLine = ""
        Me.DG.Location = New System.Drawing.Point(4, 4)
        Me.DG.MultiSort = False
        Me.DG.Name = "DG"
        Me.DG.OldSelectedRow = Nothing
        Me.DG.ReadOnly = True
        Me.DG.SetRowOnRightClick = True
        Me.DG.ShiftPressed = False
        Me.DG.SingleClickBooleanColumns = True
        Me.DG.Size = New System.Drawing.Size(544, 306)
        Me.DG.StyleName = ""
        Me.DG.SubKey = ""
        Me.DG.SuppressTriangle = False
        Me.DG.TabIndex = 0
        '
        'CancelFormButton
        '
        Me.CancelFormButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CancelFormButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelFormButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CancelFormButton.Location = New System.Drawing.Point(474, 316)
        Me.CancelFormButton.Name = "CancelFormButton"
        Me.CancelFormButton.Size = New System.Drawing.Size(74, 23)
        Me.CancelFormButton.TabIndex = 23
        Me.CancelFormButton.Text = "Cancel"
        '
        'DTDialog
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.CancelFormButton
        Me.ClientSize = New System.Drawing.Size(552, 346)
        Me.Controls.Add(Me.CancelFormButton)
        Me.Controls.Add(Me.DG)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Name = "DTDialog"
        Me.Text = "DTDialog"
        CType(Me.DG, System.ComponentModel.ISupportInitialize).EndInit()
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

    Private Sub DTDialog_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try

            SaveSettings()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub CancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelFormButton.Click
        Try

            Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.Close()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DTDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim AlertDV As DataView

        Try

            SetSettings()

            AlertDV = New DataView(_DT, "", "Severity DESC, LineNumber, Category", DataViewRowState.CurrentRows)

            DG.DataSource = AlertDV

        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub SetSettings()
        Me.Top = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)))
        Me.Height = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)))
        Me.Width = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)
    End Sub

    Private Sub SaveSettings()

        Dim lWindowState As FormWindowState

        Try

            lWindowState = Me.WindowState
            SaveSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(lWindowState).ToString)

            Me.WindowState = FormWindowState.Normal
            SaveSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)
            SaveSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString)
            SaveSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)
            SaveSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString)
            Me.WindowState = lWindowState

        Catch ex As Exception
            Throw
        End Try
    End Sub

End Class