Imports System.ComponentModel
Imports System.Text.RegularExpressions

Public Class MemberSearchControl
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _FamilyID As Integer = -1
    Private _RelationID As Integer = -1
    Private _PartSSN As Integer
    Private _PatSSN As Integer
    Private _FirstName As String
    Private _LastName As String
    Private _PatientDR As DataRow
    Private _ReturnRestricted As Boolean = False

    Public Event ActionCancel(ByVal sender As Object)
    Public Event ActionSearch(ByVal sender As Object)
    Public Event ActionClear(ByVal sender As Object)
    Public Event ActionSelect(ByVal sender As Object, ByVal patientRow As DataRow)

    Private _APPKEY As String = "UFCW\Medical\MemberSearch\"
    Private _Loading As Boolean = True

    Friend WithEvents SoundexCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents SSNTextBox As ExTextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents FirstNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents LastNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents LastNameLabel As System.Windows.Forms.Label
    Friend WithEvents FirstNameLabel As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents MemberSearchDataGrid As DataGridCustom
    Friend WithEvents ClearButton As System.Windows.Forms.Button
    Public WithEvents ActionButton As System.Windows.Forms.Button
    Friend WithEvents CancelFormButton As System.Windows.Forms.Button
    Friend WithEvents ErrorProvider1 As ErrorProvider

    'ReadOnly DomainUser As String = SystemInformation.UserName

    Private _ColumnsDT As DataTable

    Protected Overrides Sub OnCreateControl()

        MyBase.OnCreateControl()

        If Me.ParentForm Is Nothing Then

            Return

        End If

        Me.ParentForm.AcceptButton = ActionButton
        Me.ParentForm.CancelButton = CancelFormButton

    End Sub

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    Public Overloads Sub Dispose()

        MyBase.Dispose()

    End Sub

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)

        Me.MemberSearchDataGrid.DataSource = Nothing
        Me.MemberSearchDataGrid.Dispose()

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
    Friend WithEvents EligImageList As System.Windows.Forms.ImageList
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.SoundexCheckBox = New System.Windows.Forms.CheckBox()
        Me.SSNTextBox = New ExTextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.FirstNameTextBox = New System.Windows.Forms.TextBox()
        Me.LastNameTextBox = New System.Windows.Forms.TextBox()
        Me.LastNameLabel = New System.Windows.Forms.Label()
        Me.FirstNameLabel = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MemberSearchDataGrid = New DataGridCustom()
        Me.ClearButton = New System.Windows.Forms.Button()
        Me.ActionButton = New System.Windows.Forms.Button()
        Me.CancelFormButton = New System.Windows.Forms.Button()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        CType(Me.MemberSearchDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SoundexCheckBox
        '
        Me.SoundexCheckBox.AutoSize = True
        Me.SoundexCheckBox.Location = New System.Drawing.Point(514, 6)
        Me.SoundexCheckBox.Name = "SoundexCheckBox"
        Me.SoundexCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.SoundexCheckBox.Size = New System.Drawing.Size(85, 17)
        Me.SoundexCheckBox.TabIndex = 17
        Me.SoundexCheckBox.Text = "Sounds Like"
        Me.ToolTip1.SetToolTip(Me.SoundexCheckBox, "If checked Last Name searches will be expanded to include phonetic (Sounds Like) " &
        "alternatives")
        Me.SoundexCheckBox.UseVisualStyleBackColor = True
        '
        'SSNTextBox
        '
        Me.SSNTextBox.Location = New System.Drawing.Point(68, 28)
        Me.SSNTextBox.Name = "SSNTextBox"
        Me.SSNTextBox.Size = New System.Drawing.Size(110, 20)
        Me.SSNTextBox.TabIndex = 16
        Me.ToolTip1.SetToolTip(Me.SSNTextBox, "Enter either a 9 digit ssn or the last 4 digits of the ssn")
        '
        'Label1
        '
        Me.Label1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label1.Location = New System.Drawing.Point(4, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 16)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "SSN:"
        Me.ToolTip1.SetToolTip(Me.Label1, "Enter either Full 9 digit SSN or Last 4 Digits")
        '
        'FirstNameTextBox
        '
        Me.FirstNameTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.FirstNameTextBox.Location = New System.Drawing.Point(334, 4)
        Me.FirstNameTextBox.Name = "FirstNameTextBox"
        Me.FirstNameTextBox.Size = New System.Drawing.Size(171, 20)
        Me.FirstNameTextBox.TabIndex = 14
        Me.ToolTip1.SetToolTip(Me.FirstNameTextBox, "You may enter a single character or the entire first name to limit the results in" &
        "THE search. If unsure use the 1st letter only.")
        '
        'LastNameTextBox
        '
        Me.LastNameTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.LastNameTextBox.Location = New System.Drawing.Point(68, 4)
        Me.LastNameTextBox.Name = "LastNameTextBox"
        Me.LastNameTextBox.Size = New System.Drawing.Size(152, 20)
        Me.LastNameTextBox.TabIndex = 12
        Me.ToolTip1.SetToolTip(Me.LastNameTextBox, "Enter either a complete or partial last name")
        '
        'LastNameLabel
        '
        Me.LastNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.LastNameLabel.Location = New System.Drawing.Point(4, 6)
        Me.LastNameLabel.Name = "LastNameLabel"
        Me.LastNameLabel.Size = New System.Drawing.Size(64, 16)
        Me.LastNameLabel.TabIndex = 11
        Me.LastNameLabel.Text = "Last Name:"
        '
        'FirstNameLabel
        '
        Me.FirstNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.FirstNameLabel.Location = New System.Drawing.Point(227, 6)
        Me.FirstNameLabel.Name = "FirstNameLabel"
        Me.FirstNameLabel.Size = New System.Drawing.Size(107, 16)
        Me.FirstNameLabel.TabIndex = 13
        Me.FirstNameLabel.Text = "First Name (Optional):"
        '
        'MemberSearchDataGrid
        '
        Me.MemberSearchDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.MemberSearchDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.MemberSearchDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.MemberSearchDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.MemberSearchDataGrid.ADGroupsThatCanFind = ""
        Me.MemberSearchDataGrid.ADGroupsThatCanMultiSort = ""
        Me.MemberSearchDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.MemberSearchDataGrid.AllowAutoSize = True
        Me.MemberSearchDataGrid.AllowColumnReorder = True
        Me.MemberSearchDataGrid.AllowCopy = True
        Me.MemberSearchDataGrid.AllowCustomize = True
        Me.MemberSearchDataGrid.AllowDelete = False
        Me.MemberSearchDataGrid.AllowDragDrop = False
        Me.MemberSearchDataGrid.AllowEdit = False
        Me.MemberSearchDataGrid.AllowExport = True
        Me.MemberSearchDataGrid.AllowFilter = True
        Me.MemberSearchDataGrid.AllowFind = True
        Me.MemberSearchDataGrid.AllowGoTo = True
        Me.MemberSearchDataGrid.AllowMultiSelect = False
        Me.MemberSearchDataGrid.AllowMultiSort = True
        Me.MemberSearchDataGrid.AllowNew = False
        Me.MemberSearchDataGrid.AllowPrint = False
        Me.MemberSearchDataGrid.AllowRefresh = False
        Me.MemberSearchDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MemberSearchDataGrid.AppKey = "UFCW\Medical\"
        Me.MemberSearchDataGrid.AutoSaveCols = True
        Me.MemberSearchDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.MemberSearchDataGrid.CaptionVisible = False
        Me.MemberSearchDataGrid.ColumnHeaderLabel = Nothing
        Me.MemberSearchDataGrid.ColumnRePositioning = False
        Me.MemberSearchDataGrid.ColumnResizing = False
        Me.MemberSearchDataGrid.ConfirmDelete = True
        Me.MemberSearchDataGrid.CopySelectedOnly = True
        Me.MemberSearchDataGrid.CurrentBSPosition = -1
        Me.MemberSearchDataGrid.DataMember = ""
        Me.MemberSearchDataGrid.DragColumn = 0
        Me.MemberSearchDataGrid.ExportSelectedOnly = True
        Me.MemberSearchDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.MemberSearchDataGrid.HighlightedRow = Nothing
        Me.MemberSearchDataGrid.HighLightModifiedRows = False
        Me.MemberSearchDataGrid.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.MemberSearchDataGrid.IsMouseDown = False
        Me.MemberSearchDataGrid.LastGoToLine = ""
        Me.MemberSearchDataGrid.Location = New System.Drawing.Point(2, 53)
        Me.MemberSearchDataGrid.MultiSort = False
        Me.MemberSearchDataGrid.Name = "MemberSearchDataGrid"
        Me.MemberSearchDataGrid.OldSelectedRow = Nothing
        Me.MemberSearchDataGrid.PreviousBSPosition = -1
        Me.MemberSearchDataGrid.ReadOnly = True
        Me.MemberSearchDataGrid.RetainRowSelectionAfterSort = True
        Me.MemberSearchDataGrid.SetRowOnRightClick = True
        Me.MemberSearchDataGrid.ShiftPressed = False
        Me.MemberSearchDataGrid.SingleClickBooleanColumns = True
        Me.MemberSearchDataGrid.Size = New System.Drawing.Size(704, 388)
        Me.MemberSearchDataGrid.Sort = Nothing
        Me.MemberSearchDataGrid.StyleName = ""
        Me.MemberSearchDataGrid.SubKey = ""
        Me.MemberSearchDataGrid.SuppressMouseDown = False
        Me.MemberSearchDataGrid.SuppressTriangle = False
        Me.MemberSearchDataGrid.TabIndex = 21
        '
        'ClearButton
        '
        Me.ClearButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ClearButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ClearButton.Location = New System.Drawing.Point(536, 445)
        Me.ClearButton.Name = "ClearButton"
        Me.ClearButton.Size = New System.Drawing.Size(74, 23)
        Me.ClearButton.TabIndex = 23
        Me.ClearButton.Text = "Clear"
        '
        'ActionButton
        '
        Me.ActionButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ActionButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ActionButton.Location = New System.Drawing.Point(441, 445)
        Me.ActionButton.Name = "ActionButton"
        Me.ActionButton.Size = New System.Drawing.Size(74, 23)
        Me.ActionButton.TabIndex = 22
        Me.ActionButton.Text = "Search"
        '
        'CancelFormButton
        '
        Me.CancelFormButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CancelFormButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CancelFormButton.Location = New System.Drawing.Point(631, 445)
        Me.CancelFormButton.Name = "CancelFormButton"
        Me.CancelFormButton.Size = New System.Drawing.Size(74, 23)
        Me.CancelFormButton.TabIndex = 24
        Me.CancelFormButton.Text = "Cancel"
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'MemberSearchControl
        '
        Me.Controls.Add(Me.ClearButton)
        Me.Controls.Add(Me.ActionButton)
        Me.Controls.Add(Me.CancelFormButton)
        Me.Controls.Add(Me.MemberSearchDataGrid)
        Me.Controls.Add(Me.SoundexCheckBox)
        Me.Controls.Add(Me.SSNTextBox)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.FirstNameTextBox)
        Me.Controls.Add(Me.LastNameTextBox)
        Me.Controls.Add(Me.LastNameLabel)
        Me.Controls.Add(Me.FirstNameLabel)
        Me.Name = "MemberSearchControl"
        Me.Size = New System.Drawing.Size(710, 471)
        CType(Me.MemberSearchDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region "Properties"

    <Browsable(True), System.ComponentModel.Description("Determines if the user can select an item restricted based on their priviledges.")>
    Public Property ReturnRestricted() As Boolean
        Get
            Return _ReturnRestricted
        End Get
        Set(ByVal value As Boolean)
            _ReturnRestricted = value
        End Set
    End Property

    <Browsable(True), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the FamilyID of the Document.")>
    Public Property FamilyID() As Integer
        Get
            Return _FamilyID
        End Get
        Set(ByVal value As Integer)
            _FamilyID = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the RelationID of the Document.")>
    Public Property RelationID() As Integer
        Get
            Return _RelationID
        End Get
        Set(ByVal value As Integer)
            _RelationID = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("The SSN of the Participant selected.")>
    Public Property PartSSN() As Integer
        Get
            Return _PartSSN
        End Get
        Set(ByVal value As Integer)
            _PartSSN = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("The SSN of the Patient selected.")>
    Public Property PatSSN() As Integer
        Get
            Return _PatSSN
        End Get
        Set(ByVal value As Integer)
            _PatSSN = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("The First Name of the Patient selected.")>
    Public Property FirstName() As String
        Get
            Return _FirstName
        End Get
        Set(ByVal value As String)
            _FirstName = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("The Last Name of the Patient selected.")>
    Public Property LastName() As String
        Get
            Return _LastName
        End Get
        Set(ByVal value As String)
            _LastName = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("The Patient selected.")>
    Public ReadOnly Property PatientRow() As DataRow
        Get
            Return _PatientDR
        End Get
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal AppKey As String)
        Me.New()

        _APPKEY = AppKey

    End Sub

#End Region

#Region "Form\Button Events"

    Private Sub MemberSearchControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If _FirstName IsNot Nothing AndAlso _FirstName.ToString.Trim.Length > 0 Then
                Me.FirstNameTextBox.Text = _FirstName.ToString.Trim
            End If

            If _LastName IsNot Nothing AndAlso _LastName.ToString.Trim.Length > 0 Then
                Me.LastNameTextBox.Text = _LastName.ToString.Trim
            End If

            If _LastName IsNot Nothing AndAlso _LastName.ToString.Trim.Length > 0 Then ActionButton.PerformClick()

        Catch IgnoreException As Exception

        Finally
            _Loading = False
        End Try

    End Sub
#End Region

#Region "Form Events"

    Private Sub CancelFormButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelFormButton.Click

        _PartSSN = Nothing
        _PatSSN = Nothing
        _FamilyID = Nothing
        _RelationID = Nothing
        _FirstName = Nothing
        _LastName = Nothing

        RaiseEvent ActionCancel(Me)

    End Sub

    Private Sub ActionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ActionButton.Click

        Dim DS As DataSet
        Dim DT As DataTable

        Try

            If Me.ActionButton.Text = "Search" Then

                If Me.LastNameTextBox.Text.ToUpper.Trim.Length = 0 AndAlso Me.FirstNameTextBox.Text.ToUpper.Trim.Length > 0 Then

                    MsgBox("You must provide at least the first character of the 'Last Name' when providing a 'First Name'.", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Information, MsgBoxStyle), "Search incomplete")
                    Return

                ElseIf Me.SSNTextBox.Text.ToUpper.Trim.Length > 0 Then
                    If (Me.SSNTextBox.Text.ToUpper.Trim.Length = 4 AndAlso Not IsNumeric(SSNTextBox.Text)) OrElse (SSNTextBox.Text.Trim.Length > 4 AndAlso SSNTextBox.Text.Trim.Replace("-", "").Length <> 9 AndAlso Not IsNumeric(SSNTextBox.Text.Trim.Replace("-", ""))) Then
                        MsgBox("You must provide either the last 4 digits of the SSN or a complete 9 digit SSN (Including leading zeroes).", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Information, MsgBoxStyle), "Search cancelled")
                        Return
                    End If
                End If

                If Me.LastNameTextBox.Text.ToUpper.Trim.Length > 0 AndAlso Me.SSNTextBox.Text.ToUpper.Trim.Length = 4 Then

                    Using WC As New GlobalCursor

                        If SoundexCheckBox.Checked Then
                            DS = CMSDALFDBMD.RetrievePeopleBySoundexWithLast4SSN(Me.SSNTextBox.Text, If(Me.FirstNameTextBox.Text.ToUpper.Trim.EndsWith("%"), Me.FirstNameTextBox.Text.ToUpper.Trim, Me.FirstNameTextBox.Text.ToUpper.Trim & "%"), If(Me.LastNameTextBox.Text.ToUpper.Trim.EndsWith("%"), Me.LastNameTextBox.Text.ToUpper.Trim, Me.LastNameTextBox.Text.ToUpper.Trim & "%"))
                        Else
                            DS = CMSDALFDBMD.RetrievePeopleWithLast4SSN(Me.SSNTextBox.Text, If(Me.FirstNameTextBox.Text.ToUpper.Trim.EndsWith("%"), Me.FirstNameTextBox.Text.ToUpper.Trim, Me.FirstNameTextBox.Text.ToUpper.Trim & "%"), If(Me.LastNameTextBox.Text.ToUpper.Trim.EndsWith("%"), Me.LastNameTextBox.Text.ToUpper.Trim, Me.LastNameTextBox.Text.ToUpper.Trim & "%"))
                        End If

                        DT = DS.Tables(0)

                        If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
                            DT.TableName = "PatientLookup"
                            Me.MemberSearchDataGrid.DataSource = DT
                            ToolTip1.SetToolTip(MemberSearchDataGrid, "Select a row")
                        Else
                            Me.MemberSearchDataGrid.DataSource = Nothing
                            ToolTip1.SetToolTip(MemberSearchDataGrid, "")
                        End If

                    End Using

                ElseIf SSNTextBox.Text.Trim.Replace("-", "").Length = 9 Then

                    Using WC As New GlobalCursor

                        DT = CMSDALFDBMD.RetrievePatientsBySSN(CInt(SSNTextBox.Text.Trim.Replace("-", "")))

                        If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
                            DT.TableName = "PatientLookup"
                            Me.MemberSearchDataGrid.DataSource = DT
                            ToolTip1.SetToolTip(MemberSearchDataGrid, "Select a row")
                        Else

                            Me.MemberSearchDataGrid.DataSource = Nothing
                            ToolTip1.SetToolTip(MemberSearchDataGrid, "")
                        End If

                    End Using

                ElseIf Me.LastNameTextBox.Text.ToUpper.Trim.Length > 0 Then

                    Using WC As New GlobalCursor

                        If SoundexCheckBox.Checked Then
                            DS = CMSDALFDBMD.RetrievePeopleBySoundex(If(Me.FirstNameTextBox.Text.ToUpper.Trim.EndsWith("%"), Me.FirstNameTextBox.Text.ToUpper.Trim, Me.FirstNameTextBox.Text.ToUpper.Trim & "%"), If(Me.LastNameTextBox.Text.ToUpper.Trim.EndsWith("%"), Me.LastNameTextBox.Text.ToUpper.Trim, Me.LastNameTextBox.Text.ToUpper.Trim & "%"))
                        Else
                            DS = CMSDALFDBMD.RetrievePeople(If(Me.FirstNameTextBox.Text.ToUpper.Trim.EndsWith("%"), Me.FirstNameTextBox.Text.ToUpper.Trim, Me.FirstNameTextBox.Text.ToUpper.Trim & "%"), If(Me.LastNameTextBox.Text.ToUpper.Trim.EndsWith("%"), Me.LastNameTextBox.Text.ToUpper.Trim, Me.LastNameTextBox.Text.ToUpper.Trim & "%"))
                        End If

                        DT = DS.Tables(0)
                        DT.TableName = "PatientLookup"

                        If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
                            DT.TableName = "PatientLookup"
                            Me.MemberSearchDataGrid.DataSource = DT
                            ToolTip1.SetToolTip(MemberSearchDataGrid, "Select a row")
                        Else

                            Me.MemberSearchDataGrid.DataSource = Nothing
                            ToolTip1.SetToolTip(MemberSearchDataGrid, "")
                        End If

                    End Using

                ElseIf Me.SSNTextBox.Text.ToUpper.Trim.Length = 4 Then

                    Using WC As New GlobalCursor

                        DS = CMSDALFDBMD.RetrievePeopleByLast4SSN(Me.SSNTextBox.Text)
                        DT = DS.Tables(0)
                        DT.TableName = "PatientLookup"

                        If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
                            DT.TableName = "PatientLookup"
                            Me.MemberSearchDataGrid.DataSource = DT
                            ToolTip1.SetToolTip(MemberSearchDataGrid, "Select a row")
                        Else

                            Me.MemberSearchDataGrid.DataSource = Nothing
                            ToolTip1.SetToolTip(MemberSearchDataGrid, "")
                        End If

                    End Using

                Else
                    MsgBox("You must either provide at least the first character of the 'Last Name' to continue, or the Last 4 Digits of the patients SSN", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Information, MsgBoxStyle), "Search incomplete")
                End If

                If MemberSearchDataGrid.GetCurrentDataTable IsNot Nothing Then
                    MemberSearchDataGrid.SetTableStyle()
                    MemberSearchDataGrid.Sort = If(MemberSearchDataGrid.LastSortedBy, MemberSearchDataGrid.DefaultSort)
                Else
                    MsgBox("No Matches found.", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Information, MsgBoxStyle), "Search Complete")
                End If

                RaiseEvent ActionSearch(Me)

            Else 'Button text is Select
                SelectRow()
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub PersonDataGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles MemberSearchDataGrid.DoubleClick

        Select Case CType(sender, DataGridCustom).LastHitSpot.Type
            Case Is = System.Windows.Forms.DataGrid.HitTestType.None

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell
                ActionButton.PerformClick()

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader
                ActionButton.PerformClick()

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ParentRows

        End Select

    End Sub

    Private Sub SelectRow()
        Dim DV As DataView
        Dim BM As BindingManagerBase
        Dim DR As DataRow

        Try

            DV = MemberSearchDataGrid.GetCurrentDataView

            BM = Me.MemberSearchDataGrid.BindingContext(Me.MemberSearchDataGrid.DataSource, Me.MemberSearchDataGrid.DataMember)
            DR = CType(BM.Current, DataRowView).Row

            If DR IsNot Nothing Then

                If CType(DR("SSNO"), String).Contains("Restricted") Then
                    MsgBox("Your system priviledges do not allow the selection of restricted items.", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Information, MsgBoxStyle), "Invalid selection")
                    Exit Sub
                End If

                If CType(DR("SSNO"), String).Contains("Restricted") Then
                    _PatSSN = CType(Nothing, Integer)
                Else
                    _PatSSN = CInt(DR("SSNO"))
                End If

                If CType(DR("PART_SSNO"), String).Contains("Restricted") Then
                    _PartSSN = CType(Nothing, Integer)
                Else
                    _PartSSN = CInt(DR("PART_SSNO"))
                End If

                _FirstName = CStr(DR("FIRST_NAME"))
                _LastName = CStr(DR("LAST_NAME"))
                _FamilyID = CInt(DR("FAMILY_ID"))
                _RelationID = CInt(DR("RELATION_ID"))
                _PatientDR = DR

                Me.FirstNameTextBox.Text = CStr(DR("FIRST_NAME"))
                Me.LastNameTextBox.Text = CStr(DR("LAST_NAME"))

                RaiseEvent ActionSelect(Me, DR)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub PersonDataGrid_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MemberSearchDataGrid.MouseDown

        Select Case CType(sender, DataGridCustom).LastHitSpot.Type
            Case Is = System.Windows.Forms.DataGrid.HitTestType.None

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell
                Me.ActionButton.Text = "Select"

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader
                Me.ActionButton.Text = "Select"
            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ParentRows

        End Select

    End Sub

    Private Sub LastNameTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LastNameTextBox.TextChanged
        Me.ActionButton.Text = "Search"
    End Sub

    Private Sub FirstNameTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FirstNameTextBox.TextChanged
        Me.ActionButton.Text = "Search"
    End Sub

    Private Sub ClearButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearButton.Click

        Me.FirstNameTextBox.Text = ""
        Me.LastNameTextBox.Text = ""
        Me.SSNTextBox.Text = ""
        Me.SoundexCheckBox.CheckState = CheckState.Unchecked

        MemberSearchDataGrid.DataSource = Nothing

        RaiseEvent ActionClear(Me)

    End Sub

    Private Sub SSNTextBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles SSNTextBox.KeyPress
        Dim EntryOkRegex As Regex = New Regex("^[0-9\-]+$")

        Me.ActionButton.Text = "Search"

        If Not (System.Char.IsControl(e.KeyChar)) AndAlso Not EntryOkRegex.IsMatch(e.KeyChar) Then
            e.Handled = True
        End If

    End Sub

    Private Sub SSNTextBox_Validating(sender As Object, e As CancelEventArgs) Handles SSNTextBox.Validating
        Dim Result As DialogResult = DialogResult.None
        Dim TBox As TextBox = CType(sender, TextBox)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ErrorProvider1.ClearError(TBox)

            If TBox.Text.Trim.Length > 0 AndAlso TBox.Text.Trim.Equals("000-00-0000") Then
                ErrorProvider1.SetErrorWithTracking(TBox, "Enter valid SSN.")
                MessageBox.Show("Please enter valid SSN 000-00-0000, 000000000, or last 4 digits 0000", "SSN", MessageBoxButtons.OK, MessageBoxIcon.Error)

            ElseIf (TBox.Text.Trim.Length > 0) Then

                If (TBox.Text.Trim.Length <> 4 AndAlso TBox.Text.Trim.Length <> 9 AndAlso TBox.Text.Trim.Length <> 11) OrElse (TBox.Text.Trim.Length = 4 AndAlso Not IsNumeric(TBox.Text)) OrElse (TBox.Text.Trim.Length = 9 AndAlso Not IsNumeric(TBox.Text)) OrElse (TBox.Text.Contains("-") AndAlso TBox.Text.Replace("-", "").Trim.Length <> 9) Then
                    ErrorProvider1.SetErrorWithTracking(TBox, "Enter Valid SSN.")
                    MessageBox.Show("Please valid SSN 000-00-0000, 000000000, or last 4 digits 0000", "SSN", MessageBoxButtons.OK, MessageBoxIcon.Error)

                End If
            End If

            If ErrorProvider1.GetError(TBox).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
            End If

        Catch ex As Exception

            Throw

        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try
    End Sub

#End Region

End Class