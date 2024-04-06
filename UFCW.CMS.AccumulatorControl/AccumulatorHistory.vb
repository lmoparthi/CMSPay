Imports UFCW.CMS.Accumulator
Public Class AccumulatorHistory
    Inherits System.Windows.Forms.UserControl

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'UserControl overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub
    Public Overloads Sub Dispose()
        AccumulatorHistoryDataGrid.TableStyles.Clear()
        AccumulatorHistoryDataGrid.DataSource = Nothing
        AccumulatorHistoryDataGrid.Dispose()
        AccumulatorHistoryDataGrid = Nothing
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents AccumulatorHistoryDataGrid As System.Windows.Forms.DataGrid
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.AccumulatorHistoryDataGrid = New System.Windows.Forms.DataGrid
        CType(Me.AccumulatorHistoryDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AccumulatorHistoryDataGrid
        '
        Me.AccumulatorHistoryDataGrid.DataMember = ""
        Me.AccumulatorHistoryDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AccumulatorHistoryDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.AccumulatorHistoryDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.AccumulatorHistoryDataGrid.Name = "AccumulatorHistoryDataGrid"
        Me.AccumulatorHistoryDataGrid.ReadOnly = True
        Me.AccumulatorHistoryDataGrid.Size = New System.Drawing.Size(288, 376)
        Me.AccumulatorHistoryDataGrid.TabIndex = 0
        '
        'AccumulatorHistory
        '
        Me.AutoScroll = True
        Me.Controls.Add(Me.AccumulatorHistoryDataGrid)
        Me.Name = "AccumulatorHistory"
        Me.Size = New System.Drawing.Size(288, 376)
        CType(Me.AccumulatorHistoryDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub AccumulatorHistory_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetStyleGrid()
    End Sub

    Public Sub SetFormInfo(ByVal familyId As Integer, ByVal relationId As Integer)
        'AccumulatorHistoryDataGrid
        Dim memAccumManager As MemberAccumulatorManager = New MemberAccumulatorManager(relationId, familyId)
        Dim dt As DataTable = memAccumManager.GetOverrideHistory() 'familyId, relationId)
        Dim dv As DataView = New DataView(dt) ', "ACCUM_NAME <> 'FIXAC'", "ACCUM_NAME", DataViewRowState.None)
        dv.RowFilter = "ACCUM_NAME <> 'FIXAC'"
        Me.AccumulatorHistoryDataGrid.DataSource = dv
        SetStyleGrid()
    End Sub

    Private Sub SetStyleGrid()
        Me.AccumulatorHistoryDataGrid.TableStyles.Clear()
        Dim tableStyle As New DataGridTableStyle
        tableStyle.MappingName = "Table"
        'Step 2: Create DataGridColumnStyle for each col 
        '        we want to see in the grid and in the 
        '        order that we want to see them.

        Dim column As New DataGridTextBoxColumn

        column.MappingName = "ACCUM_NAME"
        column.HeaderText = "Accumulator"
        column.Width = 70
        column.NullText = ""
        tableStyle.GridColumnStyles.Add(column)

        column = New DataGridTextBoxColumn
        column.MappingName = "ENTRY_VALUE"
        column.HeaderText = "Value"
        column.Format = "#.##"
        column.Width = 70
        column.NullText = ""
        tableStyle.GridColumnStyles.Add(column)

        column = New DataGridTextBoxColumn
        column.MappingName = "APPLY_DATE"
        column.HeaderText = "Apply Date"
        column.Width = 70
        column.Format = "MM/dd/yyyy"
        column.NullText = ""
        'column.
        tableStyle.GridColumnStyles.Add(column)

        column = New DataGridTextBoxColumn
        column.MappingName = "CLAIM_ID"
        column.HeaderText = "Associated Claim"
        column.Width = 70
        column.Format = ""
        column.NullText = ""
        'column.
        tableStyle.GridColumnStyles.Add(column)

        column = New DataGridTextBoxColumn
        column.MappingName = "CREATE_USERID"
        column.HeaderText = "User"
        column.Width = 120
        column.NullText = ""
        tableStyle.GridColumnStyles.Add(column)

        column = New DataGridTextBoxColumn
        column.MappingName = "CREATE_DATE"
        column.HeaderText = "Date Override"
        column.Format = "MM/dd/yyyy hh:mm:ss"
        column.Width = 120
        column.NullText = ""
        tableStyle.GridColumnStyles.Add(column)

        tableStyle.AllowSorting = True
        Me.AccumulatorHistoryDataGrid.TableStyles.Add(tableStyle)
    End Sub
End Class
