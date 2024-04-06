Public Class FamilyChooser
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
    Friend WithEvents SelectButton As System.Windows.Forms.Button
    Friend Shadows WithEvents CancelButton As System.Windows.Forms.Button
    Public WithEvents FamilyRelationDataGrid As DataGridCustom
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.FamilyRelationDataGrid = New DataGridCustom()
        Me.SelectButton = New System.Windows.Forms.Button()
        Me.CancelButton = New System.Windows.Forms.Button()
        CType(Me.FamilyRelationDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'FamilyRelationDataGrid
        '
        Me.FamilyRelationDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.FamilyRelationDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.FamilyRelationDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.FamilyRelationDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.FamilyRelationDataGrid.ADGroupsThatCanFind = ""
        Me.FamilyRelationDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.FamilyRelationDataGrid.ADGroupsThatCanMultiSort = ""
        Me.FamilyRelationDataGrid.AllowAutoSize = True
        Me.FamilyRelationDataGrid.AllowColumnReorder = False
        Me.FamilyRelationDataGrid.AllowCopy = True
        Me.FamilyRelationDataGrid.AllowCustomize = True
        Me.FamilyRelationDataGrid.AllowDelete = False
        Me.FamilyRelationDataGrid.AllowDragDrop = False
        Me.FamilyRelationDataGrid.AllowEdit = False
        Me.FamilyRelationDataGrid.AllowExport = False
        Me.FamilyRelationDataGrid.AllowFilter = True
        Me.FamilyRelationDataGrid.AllowFind = False
        Me.FamilyRelationDataGrid.AllowGoTo = False
        Me.FamilyRelationDataGrid.AllowMultiSelect = False
        Me.FamilyRelationDataGrid.AllowMultiSort = False
        Me.FamilyRelationDataGrid.AllowNew = True
        Me.FamilyRelationDataGrid.AllowPrint = True
        Me.FamilyRelationDataGrid.AllowRefresh = False
        Me.FamilyRelationDataGrid.AllowSorting = False
        Me.FamilyRelationDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyRelationDataGrid.AppKey = "UFCW\Claims\"
        Me.FamilyRelationDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.FamilyRelationDataGrid.ColumnHeaderLabel = Nothing
        Me.FamilyRelationDataGrid.ColumnRePositioning = False
        Me.FamilyRelationDataGrid.ColumnResizing = False
        Me.FamilyRelationDataGrid.ConfirmDelete = True
        Me.FamilyRelationDataGrid.CopySelectedOnly = True
        Me.FamilyRelationDataGrid.DataMember = ""
        Me.FamilyRelationDataGrid.DragColumn = 0
        Me.FamilyRelationDataGrid.ExportSelectedOnly = True
        Me.FamilyRelationDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.FamilyRelationDataGrid.HighlightedRow = Nothing
        Me.FamilyRelationDataGrid.IsMouseDown = False
        Me.FamilyRelationDataGrid.LastGoToLine = ""
        Me.FamilyRelationDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.FamilyRelationDataGrid.MultiSort = False
        Me.FamilyRelationDataGrid.Name = "FamilyRelationDataGrid"
        Me.FamilyRelationDataGrid.OldSelectedRow = Nothing
        Me.FamilyRelationDataGrid.ReadOnly = True
        Me.FamilyRelationDataGrid.SetRowOnRightClick = True
        Me.FamilyRelationDataGrid.ShiftPressed = False
        Me.FamilyRelationDataGrid.SingleClickBooleanColumns = True
        Me.FamilyRelationDataGrid.Size = New System.Drawing.Size(604, 216)
        Me.FamilyRelationDataGrid.StyleName = ""
        Me.FamilyRelationDataGrid.SubKey = ""
        Me.FamilyRelationDataGrid.SuppressTriangle = False
        Me.FamilyRelationDataGrid.TabIndex = 0
        '
        'SelectButton
        '
        Me.SelectButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SelectButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.SelectButton.Location = New System.Drawing.Point(215, 224)
        Me.SelectButton.Name = "SelectButton"
        Me.SelectButton.Size = New System.Drawing.Size(75, 23)
        Me.SelectButton.TabIndex = 1
        Me.SelectButton.Text = "Select"
        '
        'CancelButton
        '
        Me.CancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CancelButton.Location = New System.Drawing.Point(319, 224)
        Me.CancelButton.Name = "CancelButton"
        Me.CancelButton.Size = New System.Drawing.Size(75, 23)
        Me.CancelButton.TabIndex = 2
        Me.CancelButton.Text = "Cancel"
        '
        'FamilyChooser
        '
        Me.AcceptButton = Me.SelectButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(608, 266)
        Me.Controls.Add(Me.CancelButton)
        Me.Controls.Add(Me.SelectButton)
        Me.Controls.Add(Me.FamilyRelationDataGrid)
        Me.Name = "FamilyChooser"
        Me.Text = "Family Relation Chooser"
        CType(Me.FamilyRelationDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _FamilyID As Integer? = Nothing
    Private _RelationID As Short? = Nothing
    Private _Firstname As String
    Private _Lastname As String
    Public ReadOnly Property SelectFamilyID() As Integer?
        Get
            Return _FamilyID
        End Get
    End Property

    Public ReadOnly Property SelectRelationID() As Short?
        Get
            Return _RelationID
        End Get
    End Property
    Public ReadOnly Property SelectFirstName() As String
        Get
            Return _Firstname
        End Get
    End Property
    Public ReadOnly Property SelectLastName() As String
        Get
            Return _Lastname
        End Get
    End Property
    Private Sub DoneButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelButton.Click
        Me.Close()
    End Sub

    Private Sub SelectButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectButton.Click

        Dim DT As DataTable

        Try
            DT = CType(Me.FamilyRelationDataGrid.DataSource, DataTable)
            For I As Integer = 0 To DT.Rows.Count - 1
                'dr = dv.Table.Rows(i)
                If Me.FamilyRelationDataGrid.IsSelected(I) Then
                    _FamilyID = CInt(DT.Rows(I)("FAMILY_ID"))
                    _RelationID = UFCWGeneral.IsNullShortHandler(DT.Rows(I)("RELATION_ID"))
                    _Firstname = CStr(DT.Rows(I)("FIRST_NAME"))
                    _Lastname = CStr(DT.Rows(I)("LAST_NAME"))
                End If
            Next
            Me.Hide()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub SetStyleGrid()
        Me.FamilyRelationDataGrid.TableStyles.Clear()
        Dim DGTS As New DataGridTableStyle
        DGTS.MappingName = "Table"
        'Step 2: Create DataGridColumnStyle for each col
        '        we want to see in the grid and in the
        '        order that we want to see them.

        Dim column As New DataGridTextBoxColumn
        'column = New DataGridTextBoxColumn
        'column.MappingName = "CLAIM_ID"
        'column.HeaderText = "Claim #"
        'column.Width = 45
        'tableStyle.GridColumnStyles.Add(column)

        column.MappingName = "FAMILY_ID"
        column.HeaderText = "Family ID"
        column.Width = 70
        column.NullText = ""
        DGTS.GridColumnStyles.Add(column)

        column = New DataGridTextBoxColumn
        column.MappingName = "RELATION_ID"
        column.HeaderText = "Relation ID"
        column.Width = 70
        column.NullText = ""
        DGTS.GridColumnStyles.Add(column)

        column = New DataGridTextBoxColumn
        column.MappingName = "FIRST_NAME"
        column.HeaderText = "First Name"
        column.Width = 90
        column.NullText = ""
        DGTS.GridColumnStyles.Add(column)

        column = New DataGridTextBoxColumn
        column.MappingName = "LAST_NAME"
        column.HeaderText = "Last Name"
        column.Width = 90
        column.NullText = ""
        DGTS.GridColumnStyles.Add(column)

        column = New DataGridTextBoxColumn
        column.MappingName = "BIRTH_DATE"
        column.HeaderText = "Birthdate"
        column.Width = 70
        column.Format = "MM/dd/yyyy"
        column.NullText = ""
        DGTS.GridColumnStyles.Add(column)

        column = New DataGridTextBoxColumn
        column.MappingName = "FROM_DATE"
        column.HeaderText = "From Date"
        column.Width = 70
        column.Format = "MM/dd/yyyy"
        column.NullText = ""
        'column.
        DGTS.GridColumnStyles.Add(column)

        column = New DataGridTextBoxColumn
        column.MappingName = "THRU_DATE"
        column.HeaderText = "Thru Date"
        column.Width = 70
        column.Format = "MM/dd/yyyy"
        column.NullText = ""
        DGTS.GridColumnStyles.Add(column)

        column = New DataGridTextBoxColumn
        column.MappingName = "SSNO"
        column.HeaderText = "SSN"
        column.Format = "###-##-####"
        column.Width = 70
        column.NullText = ""
        DGTS.GridColumnStyles.Add(column)

        column = New DataGridTextBoxColumn
        column.MappingName = "PART_SSNO"
        column.HeaderText = "Part SSN"
        column.Format = "###-##-####"
        column.Width = 70
        column.NullText = ""
        DGTS.GridColumnStyles.Add(column)

        DGTS.AllowSorting = False
        Me.FamilyRelationDataGrid.TableStyles.Add(DGTS)
    End Sub

    Private Sub FamilyRelationDataGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles FamilyRelationDataGrid.DoubleClick

        Select Case CType(sender, DataGridCustom).LastHitSpot.Type

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell, System.Windows.Forms.DataGrid.HitTestType.RowHeader

                Dim DT As DataTable = CType(CType(sender, DataGridCustom).DataSource, DataTable)
                For I As Integer = 0 To DT.Rows.Count - 1
                    If CType(sender, DataGridCustom).IsSelected(I) Then
                        _FamilyID = CInt(DT.Rows(I)("FAMILY_ID"))
                        _RelationID = UFCWGeneral.IsNullShortHandler(DT.Rows(I)("RELATION_ID"))
                        _Firstname = CStr(DT.Rows(I)("FIRST_NAME"))
                        _Lastname = CStr(DT.Rows(I)("LAST_NAME"))
                    End If
                    If _FamilyID Is Nothing OrElse _FamilyID = -1 Then
                        _FamilyID = CInt(DT.Rows(0)("FAMILY_ID"))
                        _RelationID = UFCWGeneral.IsNullShortHandler(DT.Rows(0)("RELATION_ID"))
                        _Firstname = CStr(DT.Rows(0)("FIRST_NAME"))
                        _Lastname = CStr(DT.Rows(0)("LAST_NAME"))
                    End If
                Next

                Me.Hide()

        End Select

    End Sub

    Private Sub FamilyChooser_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        FamilyRelationDataGrid.DataSource = Nothing
        FamilyRelationDataGrid.TableStyles.Clear()
        FamilyRelationDataGrid.Dispose()
        FamilyRelationDataGrid = Nothing
    End Sub
End Class