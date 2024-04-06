
Public Class MemberLookUpForm
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
    Friend WithEvents LastNameLabel As System.Windows.Forms.Label
    Friend WithEvents LastNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents FirstNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents FindButton As System.Windows.Forms.Button
    Friend Shadows WithEvents CancelButton As System.Windows.Forms.Button
    Friend WithEvents PersonDataGrid As DataGridCustom
    Friend WithEvents FirstNameLabel As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.LastNameLabel = New System.Windows.Forms.Label
        Me.LastNameTextBox = New System.Windows.Forms.TextBox
        Me.FirstNameTextBox = New System.Windows.Forms.TextBox
        Me.FirstNameLabel = New System.Windows.Forms.Label
        Me.FindButton = New System.Windows.Forms.Button
        Me.CancelButton = New System.Windows.Forms.Button
        Me.PersonDataGrid = New DataGridCustom
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.PersonDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LastNameLabel
        '
        Me.LastNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.LastNameLabel.Location = New System.Drawing.Point(8, 16)
        Me.LastNameLabel.Name = "LastNameLabel"
        Me.LastNameLabel.Size = New System.Drawing.Size(64, 16)
        Me.LastNameLabel.TabIndex = 0
        Me.LastNameLabel.Text = "Last Name:"
        '
        'LastNameTextBox
        '
        Me.LastNameTextBox.Location = New System.Drawing.Point(72, 14)
        Me.LastNameTextBox.Name = "LastNameTextBox"
        Me.LastNameTextBox.Size = New System.Drawing.Size(152, 20)
        Me.LastNameTextBox.TabIndex = 1
        Me.LastNameTextBox.Text = ""
        '
        'FirstNameTextBox
        '
        Me.FirstNameTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FirstNameTextBox.Location = New System.Drawing.Point(348, 14)
        Me.FirstNameTextBox.Name = "FirstNameTextBox"
        Me.FirstNameTextBox.Size = New System.Drawing.Size(300, 20)
        Me.FirstNameTextBox.TabIndex = 3
        Me.FirstNameTextBox.Text = ""
        '
        'FirstNameLabel
        '
        Me.FirstNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.FirstNameLabel.Location = New System.Drawing.Point(238, 16)
        Me.FirstNameLabel.Name = "FirstNameLabel"
        Me.FirstNameLabel.Size = New System.Drawing.Size(128, 16)
        Me.FirstNameLabel.TabIndex = 2
        Me.FirstNameLabel.Text = "First Name (Optional):"
        '
        'FindButton
        '
        Me.FindButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.FindButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.FindButton.Location = New System.Drawing.Point(165, 192)
        Me.FindButton.Name = "FindButton"
        Me.FindButton.Size = New System.Drawing.Size(80, 23)
        Me.FindButton.TabIndex = 4
        Me.FindButton.Text = "Find"
        '
        'CancelButton
        '
        Me.CancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CancelButton.Location = New System.Drawing.Point(269, 192)
        Me.CancelButton.Name = "CancelButton"
        Me.CancelButton.Size = New System.Drawing.Size(80, 23)
        Me.CancelButton.TabIndex = 5
        Me.CancelButton.Text = "Cancel"
        '
        'PersonDataGrid
        '
        Me.PersonDataGrid.AllowColumnReorder = True
        Me.PersonDataGrid.AllowCopy = True
        Me.PersonDataGrid.AllowDelete = True
        Me.PersonDataGrid.AllowDragDrop = False
        Me.PersonDataGrid.AllowEdit = True
        Me.PersonDataGrid.AllowExport = True
        Me.PersonDataGrid.AllowFind = True
        Me.PersonDataGrid.AllowGoTo = True
        Me.PersonDataGrid.AllowMultiSelect = True
        Me.PersonDataGrid.AllowMultiSort = False
        Me.PersonDataGrid.AllowNew = True
        Me.PersonDataGrid.AllowPrint = True
        Me.PersonDataGrid.AllowRefresh = False
        Me.PersonDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PersonDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.PersonDataGrid.ConfirmDelete = True
        Me.PersonDataGrid.CopySelectedOnly = True
        Me.PersonDataGrid.DataMember = ""
        Me.PersonDataGrid.ExportSelectedOnly = True
        Me.PersonDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.PersonDataGrid.LastGoToLine = ""
        Me.PersonDataGrid.Location = New System.Drawing.Point(8, 40)
        Me.PersonDataGrid.MultiSort = False
        Me.PersonDataGrid.Name = "PersonDataGrid"
        Me.PersonDataGrid.ReadOnly = True
        Me.PersonDataGrid.SetRowOnRightClick = True
        Me.PersonDataGrid.SingleClickBooleanColumns = True
        Me.PersonDataGrid.Size = New System.Drawing.Size(648, 144)
        Me.PersonDataGrid.TabIndex = 6
        '
        'MemberLookUpForm
        '
        Me.AcceptButton = Me.FindButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(664, 222)
        Me.Controls.Add(Me.PersonDataGrid)
        Me.Controls.Add(Me.CancelButton)
        Me.Controls.Add(Me.FindButton)
        Me.Controls.Add(Me.FirstNameTextBox)
        Me.Controls.Add(Me.FirstNameLabel)
        Me.Controls.Add(Me.LastNameTextBox)
        Me.Controls.Add(Me.LastNameLabel)
        Me.Name = "MemberLookUpForm"
        Me.Text = "SSN Lookup"
        CType(Me.PersonDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _SelectedFamilyID As Integer
    Private _SelectedFirstName As String
    Private _SelectedLastName As String
    Public ReadOnly Property SelectedFamilyID() As Integer
        Get
            Return _selectedFamilyID
        End Get
    End Property

    Public ReadOnly Property SelectedFirstName() As String
        Get
            Return _selectedFirstName
        End Get
    End Property
    Public ReadOnly Property SelectedLastName() As String
        Get
            Return _SelectedLastName
        End Get
    End Property
    Private Sub CancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelButton.Click
        Me.Hide()
    End Sub

    Private Sub FindButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindButton.Click
        Dim DS As DataSet
        Dim DT As DataTable


        Try
            If Me.FindButton.Text = "Find" Then

                Using WC As New GlobalCursor
                    DS = CMSDALFDBMD.RetrievePeople(If(Me.FirstNameTextBox.Text.ToUpper.Trim.EndsWith("%"), Me.FirstNameTextBox.Text.ToUpper.Trim, Me.FirstNameTextBox.Text.ToUpper.Trim & "%"), If(Me.LastNameTextBox.Text.ToUpper.Trim.EndsWith("%"), Me.LastNameTextBox.Text.ToUpper.Trim, Me.LastNameTextBox.Text.ToUpper.Trim & "%"))
                End Using

                DT = DS.Tables(0)
                DT.TableName = "PatientLookup"

                Me.PersonDataGrid.DataSource = DT
                If DT.Rows.Count > 0 Then
                    ToolTip1.SetToolTip(PersonDataGrid, "Select a row")
                Else
                    ToolTip1.SetToolTip(PersonDataGrid, "")
                End If
            Else
                SetInfo()
                Me.Hide()
            End If
        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

#Region "Style"

    ' -----------------------------------------------------------------------------
    ' <summary>
    '
    ' </summary>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2007	Created
    '     [malkoi] 7/18/2007 Modified according to ACR MED-590, added Family ID and Relation ID Columns
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub SetPersonGridStyle()
        Try
            'Step 1: Create a DataGridTableStyle &
            '        set mappingname to table.
            Dim tableStyle As New DataGridTableStyle
            tableStyle.MappingName = "PatientLookup"
            'Step 2: Create DataGridColumnStyle for each col
            '        we want to see in the grid and in the
            '        order that we want to see them.

            Dim column As New DataGridTextBoxColumn
            column.MappingName = "SSNO"
            column.HeaderText = "Patient SSN"
            column.Width = 80
            column.NullText = ""
            tableStyle.GridColumnStyles.Add(column)

            column = New DataGridTextBoxColumn
            column.MappingName = "PART_SSNO"
            column.HeaderText = "Part. SSN"
            column.Width = 80
            column.NullText = ""
            tableStyle.GridColumnStyles.Add(column)

            column = New DataGridTextBoxColumn
            column.MappingName = "FIRST_NAME"
            column.HeaderText = "First Name"
            column.NullText = ""
            column.Width = 70
            tableStyle.GridColumnStyles.Add(column)

            column = New DataGridTextBoxColumn
            column.MappingName = "MIDDLE_INITIAL"
            column.HeaderText = "Middle Int."
            column.Width = 70
            column.NullText = ""
            tableStyle.GridColumnStyles.Add(column)

            column = New DataGridTextBoxColumn
            column.MappingName = "LAST_NAME"
            column.HeaderText = "Last Name"
            column.Width = 70
            column.NullText = ""
            tableStyle.GridColumnStyles.Add(column)

            column = New DataGridTextBoxColumn
            column.MappingName = "GENDER"
            column.HeaderText = "Gender"
            column.Width = 45
            column.NullText = ""
            tableStyle.GridColumnStyles.Add(column)
            tableStyle.AllowSorting = False

            column = New DataGridTextBoxColumn
            column.MappingName = "BIRTH_DATE"
            column.HeaderText = "Date of Birth"
            column.Width = 85
            column.Format = "MM/dd/yyyy"
            column.NullText = ""
            tableStyle.GridColumnStyles.Add(column)
            tableStyle.AllowSorting = False

            column = New DataGridTextBoxColumn
            column.MappingName = "FAMILY_ID"
            column.HeaderText = "Family ID"
            column.Width = 80
            column.NullText = ""
            tableStyle.GridColumnStyles.Add(column)
            tableStyle.AllowSorting = False

            column = New DataGridTextBoxColumn
            column.MappingName = "RELATION_ID"
            column.HeaderText = "Relation ID"
            column.Width = 80
            column.NullText = ""
            tableStyle.GridColumnStyles.Add(column)
            tableStyle.AllowSorting = False

            Me.PersonDataGrid.TableStyles.Add(tableStyle)
        Catch ex As Exception
            MsgBox("An Error Has occurred", MsgBoxStyle.Exclamation, "UFCW Member Lookup Values")
        End Try
    End Sub
#End Region

    Private Sub SSNLookUp_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetPersonGridStyle()
    End Sub

    Private Sub PersonDataGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles PersonDataGrid.DoubleClick

        Select Case CType(sender, DataGridCustom).LastHitSpot.type
            Case Is = System.Windows.Forms.DataGrid.HitTestType.None

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell
                SetInfo()
                Me.Hide()
            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader
                SetInfo()
                Me.Hide()
            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ParentRows

        End Select

    End Sub

    Private Sub PersonDataGrid_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PersonDataGrid.CurrentCellChanged
        Try
            SetInfo()
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "")
        End Try
    End Sub
    Private Sub SetInfo()
        Dim Good As Boolean
        Dim DV As DataView

        Try
            DV = PersonDataGrid.GetCurrentDataView

            For I As Integer = 0 To DV.Table.Rows.Count - 1
                Good = True
                Try
                    PersonDataGrid.IsSelected(I)
                Catch ex As Exception
                    Good = False
                End Try
                If Good Then
                    If PersonDataGrid.IsSelected(I) Then
                        _SelectedFamilyID = CInt(DV(I)("FAMILY_ID"))
                        _SelectedFirstName = CStr(DV(I)("FIRST_NAME"))
                        _SelectedLastName = CStr(DV(I)("LAST_NAME"))
                        Me.FirstNameTextBox.Text = CStr(DV(I)("FIRST_NAME"))
                        Me.LastNameTextBox.Text = CStr(DV(I)("LAST_NAME"))
                    End If
                End If
            Next
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub PersonDataGrid_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PersonDataGrid.MouseDown
        SetInfo()
        Me.FindButton.Text = "Select"
    End Sub

    Private Sub LastNameTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LastNameTextBox.TextChanged
        Me.FindButton.Text = "Find"
    End Sub

    Private Sub FirstNameTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FirstNameTextBox.TextChanged
        Me.FindButton.Text = "Find"
    End Sub

    Private Sub MemberLookUpForm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        PersonDataGrid.TableStyles.Clear()
        PersonDataGrid.DataSource = Nothing
        Me.PersonDataGrid.Dispose()
    End Sub
End Class