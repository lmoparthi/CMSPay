Public Class AccumulatorFamilyChooser
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

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
    Friend WithEvents SelectButton As System.Windows.Forms.Button
    Friend Shadows WithEvents CancelButton As System.Windows.Forms.Button
    Public WithEvents FamilyRelationDataGrid As DataGridCustom
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.FamilyRelationDataGrid = New DataGridCustom
        Me.SelectButton = New System.Windows.Forms.Button
        Me.CancelButton = New System.Windows.Forms.Button
        CType(Me.FamilyRelationDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'FamilyRelationDataGrid
        '
        Me.FamilyRelationDataGrid.AllowColumnReorder = False
        Me.FamilyRelationDataGrid.AllowCopy = False
        Me.FamilyRelationDataGrid.AllowDelete = False
        Me.FamilyRelationDataGrid.AllowDragDrop = False
        Me.FamilyRelationDataGrid.AllowEdit = False
        Me.FamilyRelationDataGrid.AllowExport = False
        Me.FamilyRelationDataGrid.AllowFind = False
        Me.FamilyRelationDataGrid.AllowGoTo = False
        Me.FamilyRelationDataGrid.AllowMultiSelect = False
        Me.FamilyRelationDataGrid.AllowMultiSort = False
        Me.FamilyRelationDataGrid.AllowNew = False
        Me.FamilyRelationDataGrid.AllowPrint = False
        Me.FamilyRelationDataGrid.AllowRefresh = False
        Me.FamilyRelationDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyRelationDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.FamilyRelationDataGrid.ConfirmDelete = False
        Me.FamilyRelationDataGrid.CopySelectedOnly = False
        Me.FamilyRelationDataGrid.DataMember = ""
        Me.FamilyRelationDataGrid.ExportSelectedOnly = True
        Me.FamilyRelationDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.FamilyRelationDataGrid.LastGoToLine = ""
        Me.FamilyRelationDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.FamilyRelationDataGrid.MultiSort = False
        Me.FamilyRelationDataGrid.Name = "FamilyRelationDataGrid"
        Me.FamilyRelationDataGrid.ReadOnly = True
        Me.FamilyRelationDataGrid.SetRowOnRightClick = True
        Me.FamilyRelationDataGrid.SingleClickBooleanColumns = True
        Me.FamilyRelationDataGrid.Size = New System.Drawing.Size(604, 216)
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
        'AccumulatorFamilyChooser
        '
        Me.AcceptButton = Me.SelectButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(608, 266)
        Me.Controls.Add(Me.CancelButton)
        Me.Controls.Add(Me.SelectButton)
        Me.Controls.Add(Me.FamilyRelationDataGrid)
        Me.Name = "AccumulatorFamilyChooser"
        Me.Text = "Family Relation Chooser"
        CType(Me.FamilyRelationDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private _FamilyID As Integer? = Nothing
    Private _RelationID As Short? = Nothing
    Private _FirstName As String
    Private _LastName As String
    Private _PartSSN As String

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
            Return _FirstName
        End Get
    End Property
    Public ReadOnly Property SelectLastName() As String
        Get
            Return _LastName
        End Get
    End Property
    Public ReadOnly Property SelectPartSSN() As String
        Get
            Return _PartSSN
        End Get
    End Property
    Private Sub DoneButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelButton.Click
        Me.Close()
    End Sub

    Private Sub SelectButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectButton.Click
        Dim DT As DataTable = CType(Me.FamilyRelationDataGrid.DataSource, DataTable)
        For I As Integer = 0 To DT.Rows.Count - 1
            'dr = dv.Table.Rows(i)
            If Me.FamilyRelationDataGrid.IsSelected(I) Then
                _FamilyID = CInt(DT.Rows(I)("FAMILY_ID"))
                _RelationID = UFCWGeneral.IsNullShortHandler(DT.Rows(I)("RELATION_ID"))
                _FirstName = CStr(DT.Rows(I)("FIRST_NAME"))
                _LastName = CStr(DT.Rows(I)("LAST_NAME"))
                _PartSSN = CStr(DT.Rows(I)("PART_SSNO"))
            End If
        Next
        Me.Hide()
    End Sub

    Public Sub SetStyleGrid()

        Dim TableStyle As DataGridTableStyle
        Dim Column As DataGridTextBoxColumn

        Try

            FamilyRelationDataGrid.TableStyles.Clear()
            TableStyle = New DataGridTableStyle
            TableStyle.MappingName = "Table"
            'Step 2: Create DataGridColumnStyle for each col
            '        we want to see in the grid and in the
            '        order that we want to see them.

            Column = New DataGridTextBoxColumn
            'column = New DataGridTextBoxColumn
            'column.MappingName = "CLAIM_ID"
            'column.HeaderText = "Claim #"
            'column.Width = 45
            'tableStyle.GridColumnStyles.Add(column)

            Column.MappingName = "FAMILY_ID"
            Column.HeaderText = "Family ID"
            Column.Width = 70
            Column.NullText = ""
            TableStyle.GridColumnStyles.Add(Column)

            Column = New DataGridTextBoxColumn
            Column.MappingName = "RELATION_ID"
            Column.HeaderText = "Relation ID"
            Column.Width = 70
            Column.NullText = ""
            TableStyle.GridColumnStyles.Add(Column)

            Column = New DataGridTextBoxColumn
            Column.MappingName = "FIRST_NAME"
            Column.HeaderText = "First Name"
            Column.Width = 90
            Column.NullText = ""
            TableStyle.GridColumnStyles.Add(Column)

            Column = New DataGridTextBoxColumn
            Column.MappingName = "LAST_NAME"
            Column.HeaderText = "Last Name"
            Column.Width = 90
            Column.NullText = ""
            TableStyle.GridColumnStyles.Add(Column)

            Column = New DataGridTextBoxColumn
            Column.MappingName = "BIRTH_DATE"
            Column.HeaderText = "Birthdate"
            Column.Width = 70
            Column.Format = "MM/dd/yyyy"
            Column.NullText = ""
            TableStyle.GridColumnStyles.Add(Column)

            Column = New DataGridTextBoxColumn
            Column.MappingName = "FROM_DATE"
            Column.HeaderText = "From Date"
            Column.Width = 70
            Column.Format = "MM/dd/yyyy"
            Column.NullText = ""
            'column.
            TableStyle.GridColumnStyles.Add(Column)

            Column = New DataGridTextBoxColumn
            Column.MappingName = "THRU_DATE"
            Column.HeaderText = "Thru Date"
            Column.Width = 70
            Column.Format = "MM/dd/yyyy"
            Column.NullText = ""
            TableStyle.GridColumnStyles.Add(Column)

            Column = New DataGridTextBoxColumn
            Column.MappingName = "SSNO"
            Column.HeaderText = "SSN"
            Column.Format = "###-##-####"
            Column.Width = 70
            Column.NullText = ""
            TableStyle.GridColumnStyles.Add(Column)

            Column = New DataGridTextBoxColumn
            Column.MappingName = "PART_SSNO"
            Column.HeaderText = "Part SSN"
            Column.Format = "###-##-####"
            Column.Width = 70
            Column.NullText = ""
            TableStyle.GridColumnStyles.Add(Column)

            TableStyle.AllowSorting = False

            FamilyRelationDataGrid.TableStyles.Add(TableStyle)

        Finally
            If TableStyle IsNot Nothing Then TableStyle.Dispose()
            TableStyle = Nothing
        End Try

    End Sub

    Private Sub FamilyRelationDataGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles FamilyRelationDataGrid.DoubleClick

        Select Case CType(sender, DataGridCustom).LastHitSpot.Type

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell, System.Windows.Forms.DataGrid.HitTestType.RowHeader
                Dim DT As DataTable = CType(CType(sender, DataGridCustom).DataSource, DataTable)
                For I As Integer = 0 To DT.Rows.Count - 1
                    If CType(sender, DataGridCustom).IsSelected(I) Then
                        _FamilyID = CInt(DT.Rows(I)("FAMILY_ID"))
                        _RelationID = UFCWGeneral.IsNullShortHandler(DT.Rows(I)("RELATION_ID"))
                        _FirstName = CStr(DT.Rows(I)("FIRST_NAME"))
                        _LastName = CStr(DT.Rows(I)("LAST_NAME"))
                    End If
                    If _FamilyID Is Nothing OrElse _FamilyID = -1 Then
                        _FamilyID = CInt(DT.Rows(0)("FAMILY_ID"))
                        _RelationID = UFCWGeneral.IsNullShortHandler(DT.Rows(0)("RELATION_ID"))
                        _FirstName = CStr(DT.Rows(0)("FIRST_NAME"))
                        _LastName = CStr(DT.Rows(0)("LAST_NAME"))
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