Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class PatientLookup
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _FamilyID As Integer
    Private _APPKEY As String = "UFCW\Claims\"

#Region " Windows Form Designer generated code "
    Public Sub New(ByVal familyID As Integer)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _FamilyID = familyID
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If (components IsNot Nothing) Then
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
    Public WithEvents PatientDataGrid As DataGridCustom
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.PatientDataGrid = New DataGridCustom()
        Me.EligDataset = New EligDataset()
        CType(Me.PatientDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EligDataset, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PatientDataGrid
        '
        Me.PatientDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.PatientDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.PatientDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PatientDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PatientDataGrid.ADGroupsThatCanFind = ""
        Me.PatientDataGrid.ADGroupsThatCanMultiSort = ""
        Me.PatientDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PatientDataGrid.AllowAutoSize = True
        Me.PatientDataGrid.AllowColumnReorder = False
        Me.PatientDataGrid.AllowCopy = False
        Me.PatientDataGrid.AllowCustomize = True
        Me.PatientDataGrid.AllowDelete = False
        Me.PatientDataGrid.AllowDragDrop = False
        Me.PatientDataGrid.AllowEdit = False
        Me.PatientDataGrid.AllowExport = False
        Me.PatientDataGrid.AllowFilter = True
        Me.PatientDataGrid.AllowFind = True
        Me.PatientDataGrid.AllowGoTo = True
        Me.PatientDataGrid.AllowMultiSelect = False
        Me.PatientDataGrid.AllowMultiSort = False
        Me.PatientDataGrid.AllowNew = False
        Me.PatientDataGrid.AllowPrint = False
        Me.PatientDataGrid.AllowRefresh = False
        Me.PatientDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PatientDataGrid.AppKey = "UFCW\Claims\"
        Me.PatientDataGrid.AutoSaveCols = True
        Me.PatientDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.PatientDataGrid.ColumnHeaderLabel = Nothing
        Me.PatientDataGrid.ColumnRePositioning = False
        Me.PatientDataGrid.ColumnResizing = False
        Me.PatientDataGrid.ConfirmDelete = True
        Me.PatientDataGrid.CopySelectedOnly = True
        Me.PatientDataGrid.CurrentBSPosition = -1
        Me.PatientDataGrid.DataMember = ""
        Me.PatientDataGrid.DragColumn = 0
        Me.PatientDataGrid.ExportSelectedOnly = True
        Me.PatientDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.PatientDataGrid.HighlightedRow = Nothing
        Me.PatientDataGrid.HighLightModifiedRows = False
        Me.PatientDataGrid.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.PatientDataGrid.IsMouseDown = False
        Me.PatientDataGrid.LastGoToLine = ""
        Me.PatientDataGrid.Location = New System.Drawing.Point(13, 12)
        Me.PatientDataGrid.MultiSort = False
        Me.PatientDataGrid.Name = "PatientDataGrid"
        Me.PatientDataGrid.OldSelectedRow = Nothing
        Me.PatientDataGrid.PreviousBSPosition = -1
        Me.PatientDataGrid.ReadOnly = True
        Me.PatientDataGrid.RetainRowSelectionAfterSort = True
        Me.PatientDataGrid.SetRowOnRightClick = True
        Me.PatientDataGrid.ShiftPressed = False
        Me.PatientDataGrid.SingleClickBooleanColumns = True
        Me.PatientDataGrid.Size = New System.Drawing.Size(566, 248)
        Me.PatientDataGrid.Sort = Nothing
        Me.PatientDataGrid.StyleName = ""
        Me.PatientDataGrid.SubKey = ""
        Me.PatientDataGrid.SuppressMouseDown = False
        Me.PatientDataGrid.SuppressTriangle = False
        Me.PatientDataGrid.TabIndex = 0
        '
        'EligDataset
        '
        Me.EligDataset.DataSetName = "EligDataset"
        Me.EligDataset.Locale = New System.Globalization.CultureInfo("en-US")
        Me.EligDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'PatientLookup
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(8, 19)
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ClientSize = New System.Drawing.Size(592, 274)
        Me.Controls.Add(Me.PatientDataGrid)
        Me.Name = "PatientLookup"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Select Patient:"
        CType(Me.PatientDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EligDataset, System.ComponentModel.ISupportInitialize).EndInit()
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

    Public ReadOnly Property PatientTable() As DataTable
        Get
            Return EligDataset.REG_MASTER
        End Get
    End Property
#End Region

    Private Sub PatientLookup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            SetSettings()

            PatientDataGrid.CaptionText = "Searching..."

            EligDataset = CType(CMSDALFDBMD.RetrievePatients(_FamilyID, EligDataset), EligDataset)

            PatientDataGrid.DataSource = EligDataset.REG_MASTER
            PatientDataGrid.SetTableStyle()

            PatientDataGrid.CaptionText = "Showing " & EligDataset.REG_MASTER.Rows.Count & " Patient" & If(EligDataset.REG_MASTER.Rows.Count = 1, "", "s")

        Catch ex As Exception
            PatientDataGrid.CaptionText = "Showing 0 Patients"

            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub PatientLookup_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
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
        Dim WindowState As FormWindowState = Me.WindowState
        SaveSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(WindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = WindowState

    End Sub

    Private Sub PatientDataGrid_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PatientDataGrid.MouseUp
        Dim DG As DataGrid
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo

        Try
            DG = CType(sender, DataGrid)
            HTI = DG.HitTest(e.X, e.Y)

            If e.Button = MouseButtons.Left Then
                Select Case HTI.Type
                    Case Is = DataGrid.HitTestType.Cell
                        If PatientDataGrid.IsSelected(HTI.Row) = True Then
                            PatientDataGrid.UnSelect(HTI.Row)
                        Else
                            PatientDataGrid.Select(HTI.Row)
                        End If
                    Case Is = DataGrid.HitTestType.RowHeader
                        If PatientDataGrid.IsSelected(HTI.Row) = False Then
                            Exit Try
                        End If
                End Select

                If HTI.Type = DataGrid.HitTestType.Cell Or HTI.Type = DataGrid.HitTestType.RowHeader Then
                    Me.DialogResult = DialogResult.OK

                    'If Control.ModifierKeys <> Keys.Control AndAlso Control.ModifierKeys <> Keys.Shift Then
                    '    Me.DialogResult = DialogResult.OK
                    'Else
                    '    MultiTimer.Enabled = True
                    'End If
                End If
            End If
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        End Try
    End Sub
End Class