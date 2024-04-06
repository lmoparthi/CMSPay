
Imports System.ComponentModel

Public Class AccumulatorsHistory
    Inherits System.Windows.Forms.UserControl

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    Private _disposed As Boolean = False

    Protected Overrides Sub Dispose(disposing As Boolean)
        If _disposed Then Return

        If disposing Then
            ' Free any other managed objects here.

            If components IsNot Nothing Then
                components.Dispose()
            End If

            If AccumulatorsHistoryDataGrid IsNot Nothing Then

                AccumulatorsHistoryDataGrid.TableStyles.Clear()
                AccumulatorsHistoryDataGrid.DataSource = Nothing
                AccumulatorsHistoryDataGrid.Dispose()
            End If
            AccumulatorsHistoryDataGrid = Nothing

        End If

        ' Free any unmanaged objects here.
        '
        _disposed = True

        ' Call base class implementation.
        MyBase.Dispose(disposing)
    End Sub

    ''UserControl overrides dispose to clean up the component list.
    'Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
    '    If disposing Then
    '        If Not (components Is Nothing) Then
    '            components.Dispose()
    '        End If
    '    End If
    '    MyBase.Dispose(disposing)
    'End Sub
    'Public Overloads Sub Dispose()

    '    SaveColSettings()

    '    AccumulatorsHistoryDataGrid.TableStyles.Clear()
    '    AccumulatorsHistoryDataGrid.DataSource = Nothing
    '    AccumulatorsHistoryDataGrid.Dispose()
    '    AccumulatorsHistoryDataGrid = Nothing
    'End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents AccumulatorsHistoryDataGrid As DataGridCustom

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.AccumulatorsHistoryDataGrid = New DataGridCustom
        CType(Me.AccumulatorsHistoryDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AccumulatorHistoryDataGrid
        '
        Me.AccumulatorsHistoryDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.AccumulatorsHistoryDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.AccumulatorsHistoryDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.AccumulatorsHistoryDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.AccumulatorsHistoryDataGrid.ADGroupsThatCanFind = ""
        Me.AccumulatorsHistoryDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.AccumulatorsHistoryDataGrid.ADGroupsThatCanMultiSort = ""
        Me.AccumulatorsHistoryDataGrid.AllowAutoSize = True
        Me.AccumulatorsHistoryDataGrid.AllowColumnReorder = True
        Me.AccumulatorsHistoryDataGrid.AllowCopy = True
        Me.AccumulatorsHistoryDataGrid.AllowCustomize = True
        Me.AccumulatorsHistoryDataGrid.AllowDelete = False
        Me.AccumulatorsHistoryDataGrid.AllowDragDrop = False
        Me.AccumulatorsHistoryDataGrid.AllowEdit = True
        Me.AccumulatorsHistoryDataGrid.AllowExport = True
        Me.AccumulatorsHistoryDataGrid.AllowFilter = True
        Me.AccumulatorsHistoryDataGrid.AllowFind = True
        Me.AccumulatorsHistoryDataGrid.AllowGoTo = True
        Me.AccumulatorsHistoryDataGrid.AllowMultiSelect = True
        Me.AccumulatorsHistoryDataGrid.AllowMultiSort = False
        Me.AccumulatorsHistoryDataGrid.AllowNew = False
        Me.AccumulatorsHistoryDataGrid.AllowPrint = True
        Me.AccumulatorsHistoryDataGrid.AllowRefresh = False
        Me.AccumulatorsHistoryDataGrid.AppKey = "UFCW\Claims\"
        Me.AccumulatorsHistoryDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.AccumulatorsHistoryDataGrid.CaptionVisible = False
        Me.AccumulatorsHistoryDataGrid.ConfirmDelete = True
        Me.AccumulatorsHistoryDataGrid.CopySelectedOnly = True
        Me.AccumulatorsHistoryDataGrid.DataMember = ""
        Me.AccumulatorsHistoryDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AccumulatorsHistoryDataGrid.ExportSelectedOnly = True
        Me.AccumulatorsHistoryDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.AccumulatorsHistoryDataGrid.LastGoToLine = ""
        Me.AccumulatorsHistoryDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.AccumulatorsHistoryDataGrid.MultiSort = False
        Me.AccumulatorsHistoryDataGrid.Name = "AccumulatorsHistoryDataGrid"
        Me.AccumulatorsHistoryDataGrid.ReadOnly = True
        Me.AccumulatorsHistoryDataGrid.SetRowOnRightClick = True
        Me.AccumulatorsHistoryDataGrid.SingleClickBooleanColumns = True
        Me.AccumulatorsHistoryDataGrid.Size = New System.Drawing.Size(288, 376)
        Me.AccumulatorsHistoryDataGrid.SuppressTriangle = False
        Me.AccumulatorsHistoryDataGrid.TabIndex = 0
        '
        'AccumulatorHistory
        '
        Me.AutoScroll = True
        Me.Controls.Add(Me.AccumulatorsHistoryDataGrid)
        Me.Name = "AccumulatorHistory"
        Me.Size = New System.Drawing.Size(288, 376)
        CType(Me.AccumulatorsHistoryDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"

    <DefaultValue(CBool(False)), Browsable(True), System.ComponentModel.Description("Shows or Hides the Close Button.")> _
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = Value
        End Set
    End Property

    Public Sub SetFormInfo(ByVal familyID As Integer, ByVal relationID As Short)

        Dim MemAccumManager As MemberAccumulatorManager
        Dim DT As DataTable
        Dim DV As DataView

        Try

            MemAccumManager = New MemberAccumulatorManager(relationID, familyID)
            DT = MemAccumManager.GetOverrideHistory() 'familyId, relationId)
            DV = New DataView(DT) ', "ACCUM_NAME <> 'FIXAC'", "ACCUM_NAME", DataViewRowState.None)
            DV.RowFilter = "ACCUM_NAME <> 'FIXAC'"

            AccumulatorsHistoryDataGrid.DataSource = DV

            AccumulatorsHistoryDataGrid.SetTableStyle()

        Catch ex As Exception
            Throw
        Finally

            DT = Nothing
            DV = Nothing
        End Try

    End Sub
    'Private Sub SetStyleGrid()
    '    Me.AccumulatorHistoryDataGrid.TableStyles.Clear()
    '    Dim tableStyle As New DataGridTableStyle
    '    tableStyle.MappingName = "Table"
    '    'Step 2: Create DataGridColumnStyle for each col
    '    '        we want to see in the grid and in the
    '    '        order that we want to see them.

    '    Dim column As New DataGridTextBoxColumn

    '    column.MappingName = "ACCUM_NAME"
    '    column.HeaderText = "Accumulator"
    '    column.Width = 70
    '    column.NullText = ""
    '    tableStyle.GridColumnStyles.Add(column)

    '    column = New DataGridTextBoxColumn
    '    column.MappingName = "ENTRY_VALUE"
    '    column.HeaderText = "Value"
    '    column.Format = "#.##"
    '    column.Width = 70
    '    column.NullText = ""
    '    tableStyle.GridColumnStyles.Add(column)

    '    column = New DataGridTextBoxColumn
    '    column.MappingName = "APPLY_DATE"
    '    column.HeaderText = "Apply Date"
    '    column.Width = 70
    '    column.Format = "MM/dd/yyyy"
    '    column.NullText = ""
    '    'column.
    '    tableStyle.GridColumnStyles.Add(column)

    '    column = New DataGridTextBoxColumn
    '    column.MappingName = "CLAIM_ID"
    '    column.HeaderText = "Associated Claim"
    '    column.Width = 70
    '    column.Format = ""
    '    column.NullText = ""
    '    'column.
    '    tableStyle.GridColumnStyles.Add(column)

    '    column = New DataGridTextBoxColumn
    '    column.MappingName = "CREATE_USERID"
    '    column.HeaderText = "User"
    '    column.Width = 120
    '    column.NullText = ""
    '    tableStyle.GridColumnStyles.Add(column)

    '    column = New DataGridTextBoxColumn
    '    column.MappingName = "CREATE_DATE"
    '    column.HeaderText = "Date Override"
    '    column.Format = "MM/dd/yyyy hh:mm:ss"
    '    column.Width = 120
    '    column.NullText = ""
    '    tableStyle.GridColumnStyles.Add(column)

    '    tableStyle.AllowSorting = True
    '    Me.AccumulatorHistoryDataGrid.TableStyles.Add(tableStyle)
    'End Sub
End Class