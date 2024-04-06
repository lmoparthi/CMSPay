Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class PatientLookup
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _FamilyID As Integer
    Private _APPKEY As String = "UFCW\Claims\"

    Friend WithEvents CloseFormButton As System.Windows.Forms.Button

#Region " Windows Form Designer generated code "
    Public Sub New(ByVal FamilyID As Integer)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _FamilyID = FamilyID
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
    Friend WithEvents EligDataset As EligDataset
    Public WithEvents PatientLookupDataGrid As DataGridCustom
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.PatientLookupDataGrid = New DataGridCustom()
        Me.EligDataset = New EligDataset()
        Me.CloseFormButton = New System.Windows.Forms.Button()
        CType(Me.PatientLookupDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EligDataset, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PatientLookupDataGrid
        '
        Me.PatientLookupDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.PatientLookupDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.PatientLookupDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PatientLookupDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PatientLookupDataGrid.ADGroupsThatCanFind = ""
        Me.PatientLookupDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PatientLookupDataGrid.ADGroupsThatCanMultiSort = ""
        Me.PatientLookupDataGrid.AllowAutoSize = True
        Me.PatientLookupDataGrid.AllowColumnReorder = False
        Me.PatientLookupDataGrid.AllowCopy = False
        Me.PatientLookupDataGrid.AllowCustomize = True
        Me.PatientLookupDataGrid.AllowDelete = False
        Me.PatientLookupDataGrid.AllowDragDrop = False
        Me.PatientLookupDataGrid.AllowEdit = False
        Me.PatientLookupDataGrid.AllowExport = False
        Me.PatientLookupDataGrid.AllowFilter = True
        Me.PatientLookupDataGrid.AllowFind = True
        Me.PatientLookupDataGrid.AllowGoTo = True
        Me.PatientLookupDataGrid.AllowMultiSelect = False
        Me.PatientLookupDataGrid.AllowMultiSort = False
        Me.PatientLookupDataGrid.AllowNew = False
        Me.PatientLookupDataGrid.AllowPrint = False
        Me.PatientLookupDataGrid.AllowRefresh = False
        Me.PatientLookupDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PatientLookupDataGrid.AppKey = "UFCW\Claims\"
        Me.PatientLookupDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.PatientLookupDataGrid.ColumnHeaderLabel = Nothing
        Me.PatientLookupDataGrid.ColumnRePositioning = False
        Me.PatientLookupDataGrid.ColumnResizing = False
        Me.PatientLookupDataGrid.ConfirmDelete = True
        Me.PatientLookupDataGrid.CopySelectedOnly = True
        Me.PatientLookupDataGrid.DataMember = ""
        Me.PatientLookupDataGrid.DragColumn = 0
        Me.PatientLookupDataGrid.ExportSelectedOnly = True
        Me.PatientLookupDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.PatientLookupDataGrid.HighlightedRow = Nothing
        Me.PatientLookupDataGrid.IsMouseDown = False
        Me.PatientLookupDataGrid.LastGoToLine = ""
        Me.PatientLookupDataGrid.Location = New System.Drawing.Point(8, 8)
        Me.PatientLookupDataGrid.MultiSort = False
        Me.PatientLookupDataGrid.Name = "PatientLookupDataGrid"
        Me.PatientLookupDataGrid.OldSelectedRow = Nothing
        Me.PatientLookupDataGrid.ReadOnly = True
        Me.PatientLookupDataGrid.SetRowOnRightClick = True
        Me.PatientLookupDataGrid.ShiftPressed = False
        Me.PatientLookupDataGrid.SingleClickBooleanColumns = True
        Me.PatientLookupDataGrid.Size = New System.Drawing.Size(576, 298)
        Me.PatientLookupDataGrid.StyleName = ""
        Me.PatientLookupDataGrid.SubKey = ""
        Me.PatientLookupDataGrid.SuppressTriangle = False
        Me.PatientLookupDataGrid.TabIndex = 0
        '
        'EligDataset
        '
        Me.EligDataset.DataSetName = "EligDataset"
        Me.EligDataset.Locale = New System.Globalization.CultureInfo("en-US")
        Me.EligDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'CloseFormButton
        '
        Me.CloseFormButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CloseFormButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CloseFormButton.Location = New System.Drawing.Point(505, 308)
        Me.CloseFormButton.Name = "CloseFormButton"
        Me.CloseFormButton.Size = New System.Drawing.Size(75, 23)
        Me.CloseFormButton.TabIndex = 1
        Me.CloseFormButton.Text = "Close"
        Me.CloseFormButton.UseVisualStyleBackColor = True
        '
        'PatientLookup
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.CloseFormButton
        Me.ClientSize = New System.Drawing.Size(592, 334)
        Me.Controls.Add(Me.CloseFormButton)
        Me.Controls.Add(Me.PatientLookupDataGrid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Name = "PatientLookup"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Select Patient:"
        CType(Me.PatientLookupDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EligDataset, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Public Properties"
    Public ReadOnly Property PatientTable() As DataTable
        Get
            Return EligDataset.REG_MASTER
        End Get
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

    Private Sub PatientLookup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            SetSettings()

            PatientLookupDataGrid.CaptionText = "Searching..."

            EligDataset = CType(CMSDALFDBMD.RetrievePatientsDemographics(_FamilyID, EligDataset), EligDataset)

            PatientLookupDataGrid.SuspendLayout()
            PatientLookupDataGrid.DataSource = EligDataset.REG_MASTER
            PatientLookupDataGrid.SetTableStyle()
            PatientLookupDataGrid.ResumeLayout()

            PatientLookupDataGrid.CaptionText = "Showing " & EligDataset.REG_MASTER.Rows.Count & " Patient" & If(EligDataset.REG_MASTER.Rows.Count = 1, "", "s")

        Catch ex As Exception
            PatientLookupDataGrid.CaptionText = "Showing 0 Patients"
            Throw
        End Try
    End Sub

    Private Sub PatientLookup_FormClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.FormClosing
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
        Dim TheWindowState As FormWindowState = Me.WindowState
        SaveSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(TheWindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = TheWindowState

    End Sub

    Private Sub PatientDataGrid_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PatientLookupDataGrid.MouseUp
        Dim DG As DataGrid
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo

        Try
            DG = CType(sender, DataGrid)
            HTI = DG.HitTest(e.X, e.Y)

            If e.Button = MouseButtons.Left Then
                Select Case HTI.Type
                    Case Is = DataGrid.HitTestType.Cell
                        If PatientLookupDataGrid.IsSelected(HTI.Row) Then
                            PatientLookupDataGrid.UnSelect(HTI.Row)
                        Else
                            PatientLookupDataGrid.Select(HTI.Row)
                        End If
                    Case Is = DataGrid.HitTestType.RowHeader
                        If Not PatientLookupDataGrid.IsSelected(HTI.Row) Then
                            Exit Try
                        End If
                End Select

                If HTI.Type = DataGrid.HitTestType.Cell Or HTI.Type = DataGrid.HitTestType.RowHeader Then
                    Me.DialogResult = DialogResult.OK
                End If
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub CloseFormButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseFormButton.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
    End Sub

End Class