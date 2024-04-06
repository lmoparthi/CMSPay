Imports System.ComponentModel
Imports System.Drawing.Imaging
Imports System.Configuration
Imports System.Data.Common


Public Class LETTERSControl
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _LineOfBusiness As String = ""

    Private _LetterTemplateID As Integer
    Private _LetterTemplateName As String
    Private _AddressesDS As DataSet
    Private _DS As DataSet

    Private _LetterID As Integer
    Private _MaxID As String
    Private _DocumentType As String = ""
    Private _DocumentID As Integer

    Private _ClaimID As Integer?

    Private _FamilyID As Integer
    Private _RelationID As Short?

    Private _ProviderID As Integer?

    Private _LINES(9) As String
    Private _TEXTS(29) As String

    Private _AMTS2(9) As Nullable(Of Decimal)
    Private _AMTS4(4) As Nullable(Of Decimal)
    Private _DATES(7) As Nullable(Of Date)
    Private _NUMS(4) As Nullable(Of Integer)

    Private _DaysToPendClaim As Integer
    Private _Hide As Boolean
    Private _ButtCol As DataGridHighlightButtonColumn
    Private _D As [Delegate]
    Private _APPKEY As String = "UFCW\Medical\"
    Private _Loading As Boolean = True

    Private _HoverCell As New DataGridCell

    Public Event PendedDuration(ByVal DaysToPendClaim As Object, ByVal Hide As Boolean)

    ReadOnly _DomainUser As String = SystemInformation.UserName

    Shared Event RefreshLettersHistory(ByVal sender As Object)

    ' -----------------------------------------------------------------------------
    ' <summary>
    '
    ' </summary>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	6/27/2007	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Overloads Sub Dispose()

        If ToolTip IsNot Nothing Then ToolTip.Dispose()
        ToolTip = Nothing

        If _DS IsNot Nothing Then _DS.Dispose()
        _DS = Nothing

        If _AddressesDS IsNot Nothing Then _AddressesDS.Dispose()
        _AddressesDS = Nothing

        If LettersCXTMenu IsNot Nothing Then LettersCXTMenu.Dispose()
        LettersCXTMenu = Nothing

        If LettersDataGrid IsNot Nothing Then
            If _D IsNot Nothing Then
                RemoveHandler _ButtCol.ColumnButton.Click, CType(_D, Global.System.EventHandler)
            End If
            _D = Nothing
            LettersDataGrid.TableStyles.Clear()
            LettersDataGrid.DataSource = Nothing
            LettersDataGrid.Dispose()
        End If
        LettersDataGrid = Nothing

        If ErrorProvider1 IsNot Nothing Then ErrorProvider1.Dispose()
        ErrorProvider1 = Nothing

        MyBase.Dispose()
    End Sub
#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'UserControl1 overrides dispose to clean up the component list.
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
    Friend WithEvents LettersDataGrid As DataGridCustom
    Friend WithEvents ToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents LettersCXTMenu As System.Windows.Forms.ContextMenu
    Friend WithEvents MenuGenerateLetter As System.Windows.Forms.MenuItem
    Friend WithEvents ActionButton As System.Windows.Forms.Button
    Public WithEvents Status As System.Windows.Forms.Label
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents FindMenuItem As System.Windows.Forms.MenuItem

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.LettersDataGrid = New DataGridCustom()
        Me.LettersCXTMenu = New System.Windows.Forms.ContextMenu()
        Me.MenuGenerateLetter = New System.Windows.Forms.MenuItem()
        Me.FindMenuItem = New System.Windows.Forms.MenuItem()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.ActionButton = New System.Windows.Forms.Button()
        Me.Status = New System.Windows.Forms.Label()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        CType(Me.LettersDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LettersDataGrid
        '
        Me.LettersDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.LettersDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.LettersDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LettersDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LettersDataGrid.ADGroupsThatCanFind = ""
        Me.LettersDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LettersDataGrid.ADGroupsThatCanMultiSort = ""
        Me.LettersDataGrid.AllowAutoSize = True
        Me.LettersDataGrid.AllowColumnReorder = False
        Me.LettersDataGrid.AllowCopy = False
        Me.LettersDataGrid.AllowCustomize = True
        Me.LettersDataGrid.AllowDelete = False
        Me.LettersDataGrid.AllowDragDrop = False
        Me.LettersDataGrid.AllowEdit = False
        Me.LettersDataGrid.AllowExport = False
        Me.LettersDataGrid.AllowFilter = True
        Me.LettersDataGrid.AllowFind = True
        Me.LettersDataGrid.AllowGoTo = False
        Me.LettersDataGrid.AllowMultiSelect = False
        Me.LettersDataGrid.AllowMultiSort = False
        Me.LettersDataGrid.AllowNew = False
        Me.LettersDataGrid.AllowPrint = False
        Me.LettersDataGrid.AllowRefresh = False
        Me.LettersDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LettersDataGrid.AppKey = "UFCW\Claims\"
        Me.LettersDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.LettersDataGrid.CaptionText = "Available Letters"
        Me.LettersDataGrid.ColumnHeaderLabel = Nothing
        Me.LettersDataGrid.ColumnRePositioning = False
        Me.LettersDataGrid.ColumnResizing = False
        Me.LettersDataGrid.ConfirmDelete = True
        Me.LettersDataGrid.ContextMenu = Me.LettersCXTMenu
        Me.LettersDataGrid.CopySelectedOnly = True
        Me.LettersDataGrid.DataMember = ""
        Me.LettersDataGrid.DragColumn = 0
        Me.LettersDataGrid.ExportSelectedOnly = True
        Me.LettersDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.LettersDataGrid.HighlightedRow = Nothing
        Me.LettersDataGrid.IsMouseDown = False
        Me.LettersDataGrid.LastGoToLine = ""
        Me.LettersDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.LettersDataGrid.MultiSort = False
        Me.LettersDataGrid.Name = "LettersDataGrid"
        Me.LettersDataGrid.OldSelectedRow = Nothing
        Me.LettersDataGrid.ReadOnly = True
        Me.LettersDataGrid.SetRowOnRightClick = True
        Me.LettersDataGrid.ShiftPressed = False
        Me.LettersDataGrid.SingleClickBooleanColumns = True
        Me.LettersDataGrid.Size = New System.Drawing.Size(424, 360)
        Me.LettersDataGrid.StyleName = ""
        Me.LettersDataGrid.SubKey = ""
        Me.LettersDataGrid.SuppressTriangle = False
        Me.LettersDataGrid.TabIndex = 0
        Me.ToolTip.SetToolTip(Me.LettersDataGrid, "Double click a Letter to continue")
        '
        'Letters
        '
        Me.LettersCXTMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuGenerateLetter, Me.FindMenuItem})
        '
        'MenuGenerateLetter
        '
        Me.MenuGenerateLetter.Index = 0
        Me.MenuGenerateLetter.Text = "Generate Selected Letter"
        '
        'FindMenuItem
        '
        Me.FindMenuItem.Index = 1
        Me.FindMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlF
        Me.FindMenuItem.Text = "Find"
        '
        'btnAction
        '
        Me.ActionButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ActionButton.Location = New System.Drawing.Point(0, 400)
        Me.ActionButton.Name = "ActionButton"
        Me.ActionButton.Size = New System.Drawing.Size(104, 24)
        Me.ActionButton.TabIndex = 1
        Me.ActionButton.Text = "Select Letter"
        '
        'lblStatus
        '
        Me.Status.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Status.BackColor = System.Drawing.SystemColors.Control
        Me.Status.ForeColor = System.Drawing.Color.Red
        Me.Status.Location = New System.Drawing.Point(0, 376)
        Me.Status.Name = "Status"
        Me.Status.Size = New System.Drawing.Size(416, 16)
        Me.Status.TabIndex = 2
        Me.Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'LETTERSControl
        '
        Me.Controls.Add(Me.Status)
        Me.Controls.Add(Me.ActionButton)
        Me.Controls.Add(Me.LettersDataGrid)
        Me.Name = "LETTERSControl"
        Me.Size = New System.Drawing.Size(424, 432)
        Me.ToolTip.SetToolTip(Me, "Double click a Letter to continue")
        CType(Me.LettersDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Properties"
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Patients Last Name.")>
    Public ReadOnly Property PATLNAME() As String
        Get
            Return _AddressesDS.Tables("PATIENT_ADDRESS").Rows(0)("LAST_NAME").ToString
        End Get
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Patients First Name.")>
    Public ReadOnly Property PATFNAME() As String
        Get
            Return _AddressesDS.Tables("PATIENT_ADDRESS").Rows(0)("FIRST_NAME").ToString
        End Get
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Participants Last Name.")>
    Public ReadOnly Property PARTLNAME() As String
        Get
            Return _AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("LAST_NAME").ToString
        End Get
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Participants First Name.")>
    Public ReadOnly Property PARTFNAME() As String
        Get
            Return _AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("FIRST_NAME").ToString
        End Get
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Participants 1st Address Line.")>
    Public ReadOnly Property PARTADD1() As String
        Get
            Return _AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("ADDRESS1").ToString
        End Get
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Participants 2nd Address Line.")>
    Public ReadOnly Property PARTADD2() As String
        Get
            Return _AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("ADDRESS2").ToString
        End Get
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Participants City.")>
    Public ReadOnly Property PARTCITY() As String
        Get
            Return _AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("CITY").ToString
        End Get
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Participants State.")>
    Public ReadOnly Property PARTSTATE() As String
        Get
            Return _AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("STATE").ToString
        End Get
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Participants ZIP.")>
    Public ReadOnly Property PARTZIP() As Integer
        Get
            Return CInt(_AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("ZIP1"))
        End Get
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Participants ZIP4.")>
    Public ReadOnly Property PARTZIP4() As Integer
        Get
            Return CInt(_AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("ZIP2"))
        End Get
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets the Generated MAXID.")>
    Public ReadOnly Property MAXID() As String
        Get
            Return _MaxID
        End Get
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("The number of days to wait for return correspondance.")>
    Public ReadOnly Property DaysToPendClaim() As Integer
        Get
            Return _DaysToPendClaim
        End Get
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Indicates that Hide status should be used when Pending.")>
    Public ReadOnly Property HideWhilePended() As Boolean
        Get
            Return _Hide
        End Get
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets the Generated Letter Identifier.")>
    Public ReadOnly Property LETTERID() As Integer
        Get
            Return _LetterID
        End Get
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Participant SSN.")>
    Public ReadOnly Property PARTSSN() As Integer
        Get
            Return CInt(_AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("PART_SSNO"))
        End Get
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Patient SSN.")>
    Public ReadOnly Property PATSSN() As Integer
        Get
            Return CInt(_AddressesDS.Tables("PATIENT_ADDRESS").Rows(0)("SSNO"))
        End Get
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the ProviderID.")>
    Public Property ProviderID() As Integer?
        Get
            Return _ProviderID
        End Get
        Set(ByVal value As Integer?)
            _ProviderID = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the FamilyID.")>
    Public Property FamilyID() As Integer
        Get
            Return _FamilyID
        End Get
        Set(ByVal value As Integer)
            _FamilyID = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the RelationID.")>
    Public Property RelationID() As Short?
        Get
            Return _RelationID
        End Get
        Set(ByVal value As Short?)
            _RelationID = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the ClaimID.")>
    Public Property ClaimID() As Integer?
        Get
            Return _ClaimID
        End Get
        Set(ByVal value As Integer?)
            _ClaimID = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets the Letter Template ID.")>
    Public ReadOnly Property LetterTemplateID() As Integer
        Get
            Return _LetterTemplateID
        End Get
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), System.ComponentModel.Description("Gets or Sets the Line Of Business.")>
    Public Property LineOfBusiness() As String
        Get
            Return _LineOfBusiness
        End Get
        Set(ByVal value As String)
            _LineOfBusiness = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the DocumentType (used in logging).")>
    Public Property DocumentType() As String
        Get
            Return _DocumentType
        End Get
        Set(ByVal value As String)
            _DocumentType = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets the DocumentID of associated Claim (used in logging).")>
    Public ReadOnly Property DocumentID() As Integer
        Get
            Return _DocumentID
        End Get
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the DataSet containing the Patient and Participants Demographics.")>
    Public Property Addresses() As DataSet
        Get
            Return _AddressesDS
        End Get
        Set(ByVal value As DataSet)
            _AddressesDS = value
        End Set
    End Property

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

#Region "Constructor"
    Public Sub New(ByVal LetterID As Integer)
        Me.New()

        _LetterID = LetterID
    End Sub
    Public Sub New(ByVal LetterID As Integer, ByVal DocumentClass As String)
        Me.New()

        _LetterID = LetterID
        _LineOfBusiness = DocumentClass
    End Sub

#End Region

#Region "Form\Button Events"

    Private Sub LettersControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        _Loading = False
    End Sub

    Private Sub SelectLetter()

        Dim BM As BindingManagerBase
        Dim DR As DataRow
        Dim DG As DataGridCustom

        Try

            DG = CType(LettersDataGrid, DataGridCustom)

            If DG.GetGridRowCount = 0 Then Exit Sub

            BM = DG.BindingContext(DG.DataSource, DG.DataMember)
            DR = CType(BM.Current, DataRowView).Row

            If DR IsNot Nothing Then

                _LetterTemplateID = CInt(DR("LETTERTEMPLATE_ID"))
                _LetterTemplateName = CStr(DR("LETTER_NAME")).Trim

                If DR.Table.Columns.Contains("ENABLE_HIDE") Then _Hide = CBool(DR("ENABLE_HIDE"))
                If DR.Table.Columns.Contains("PEND_DURATION_IN_DAYS") AndAlso DR("PEND_DURATION_IN_DAYS") IsNot DBNull.Value Then
                    _DaysToPendClaim = CInt(DR("ENABLE_HIDE"))
                End If

                LoadBookMarkGrid(CInt(DR("LETTERTEMPLATE_ID")))

                ActionButton.Text = "Generate Letter"

                Me.Status.Text = "Enter all required fields '@' markers must be removed to continue."

            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub ActionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuGenerateLetter.Click, ActionButton.Click

        Dim OpenCount As Integer = 0
        Dim SelCount As Integer = 0
        Dim FndIndx As Integer = -1

        Try
            Me.Status.Text = ""

            Select Case ActionButton.Text
                Case "Select Letter"

                    SelectLetter()

                Case "Generate Letter"

                    Using WC As New GlobalCursor

                        If Not ValidateBookmarks() Then
                            Me.Status.Text = "Please correct data errors, Hover over RED ! for more details."
                            Return
                        End If

                        TransferBookmarks()

                        GenerateLetterEntry(_LetterTemplateID, _LetterTemplateName)

                        RaiseEvent RefreshLettersHistory(Me)

                        Me.Status.Text = "Letter scheduled for printing id/maxid( " & _LetterID.ToString & " / " & _MaxID.ToString & ")"

                        ActionButton.Text = "Select Letter"

                        LoadLetters(_LineOfBusiness)

                    End Using

            End Select

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub
    Private Sub LetterGridClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LettersDataGrid.DoubleClick

        Try

            Select Case CType(sender, DataGridCustom).LastHitSpot.Type
                Case Is = System.Windows.Forms.DataGrid.HitTestType.None

                Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell, Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader

                    Status.Text = ""

                    Select Case ActionButton.Text

                        Case "Select Letter"

                            SelectLetter()

                        Case "Generate Letter"

                            ModifyBookMarkText()

                    End Select

                Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader

                Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize

                Case Is = System.Windows.Forms.DataGrid.HitTestType.RowResize

                Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption

                Case Is = System.Windows.Forms.DataGrid.HitTestType.ParentRows

            End Select


        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub
    Private Sub LoadBookMarkGrid(ByVal letterTemplateID As Integer)

        Try

            Using WC As New GlobalCursor

                If _DS IsNot Nothing AndAlso _DS.Tables.Contains("LETTER_BOOKMARKS") Then _DS.Tables("LETTER_BOOKMARKS").Rows.Clear()

                _DS = LettersDAL.GetBookMarks(letterTemplateID, _DS)

                LettersDataGrid.DataSource = _DS.Tables("LETTER_BOOKMARKS")

                If _DS IsNot Nothing Then
                    LettersDataGrid.SetTableStyle()
                End If

                LettersDataGrid.CaptionText = "Generate Letter"
                LettersDataGrid.Focus()

            End Using

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub
    Private Sub FindMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindMenuItem.Click
        LettersDataGrid.Find()
    End Sub

#End Region

#Region "Custom Subs\Functions"

    Public Sub LoadLetters(ByVal lineOfBusiness As String)
        Try
            _LineOfBusiness = lineOfBusiness

            LoadLettersDS()

            ActionButton.Text = "Select Letter"
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Sub LoadLetters()
        Try
            _LineOfBusiness = Nothing

            LoadLettersDS()

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub
    Private Sub LoadLettersHistoryDS()

        Try
            _DS = LettersDAL.GetLettersHistoryByCLAIMID(_ClaimID, _DS)
        Catch ex As Exception
            Throw
        End Try

    End Sub
    Public Sub ClearLetters()

        If _DS IsNot Nothing AndAlso _DS.Tables.Contains("LETTER_BOOKMARKS") Then _DS.Tables("LETTER_BOOKMARKS").Rows.Clear()

        If _DS IsNot Nothing AndAlso _DS.Tables.Contains("LETTER_MASTER") Then _DS.Tables("LETTER_MASTER").Rows.Clear()

        ActionButton.Text = "Select Letter"
        LettersDataGrid.DataSource = Nothing
        Status.Text = ""

    End Sub
    Private Sub GenerateLetterEntry(ByVal letterTemplateID As Integer, ByVal letterTemplateName As String)

        Dim Transaction As DbTransaction

        Try

            Transaction = LettersDAL.BeginTransaction()

            If _LineOfBusiness = "MEDICAL" Then
                LettersDAL.CreateLetterRequest(letterTemplateID, letterTemplateName, _AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0), _AddressesDS.Tables("PATIENT_ADDRESS").Rows(0), _LINES, _DATES, _AMTS2, _AMTS4, _NUMS, _TEXTS, _LetterID, _MaxID, _LineOfBusiness, _LetterTemplateName, Transaction)
                '                LettersDAL.CreateLetterRequest(letterTemplateID, letterTemplateName, _ClaimID, _ProviderID, _AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0), _AddressesDS.Tables("PATIENT_ADDRESS").Rows(0), _LetterID, _MaxID, _LINES, Transaction)
                LettersDAL.CreateClaimHistory(Nothing, _DocumentID, "INSERTED", _FamilyID, CShort(_RelationID), CInt(_AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("SSNO")), CInt(_AddressesDS.Tables("PATIENT_ADDRESS").Rows(0)("SSNO")), _DocumentType, "Letter (" & _LetterTemplateName & ") Requested", "Adjuster " & _DomainUser & " requested that letter " & _LetterTemplateID & "/" & _LetterTemplateName & " be generated. System created LetterID(" & _LetterID & " / " & _MaxID & " )", Transaction)
            ElseIf _LineOfBusiness = "CLAIMS" Then
                LettersDAL.CreateLetterRequest(letterTemplateID, letterTemplateName, _AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0), _AddressesDS.Tables("PATIENT_ADDRESS").Rows(0), _LINES, _DATES, _AMTS2, _AMTS4, _NUMS, _TEXTS, _LetterID, _MaxID, _LineOfBusiness, _LetterTemplateName, Transaction)
                'LettersDAL.CreateLetterRequest(letterTemplateID, letterTemplateName, _ClaimID, _ProviderID, _AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0), _AddressesDS.Tables("PATIENT_ADDRESS").Rows(0), _LetterID, _MaxID, _LINES, Transaction)
                LettersDAL.CreateClaimHistory(_ClaimID, _DocumentID, "INSERTED", _FamilyID, CShort(_RelationID), CInt(_AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("SSNO")), CInt(_AddressesDS.Tables("PATIENT_ADDRESS").Rows(0)("SSNO")), _DocumentType, "Letter (" & _LetterTemplateName & ") Requested", "Adjuster " & _DomainUser & " requested that letter " & _LetterTemplateID & "/" & _LetterTemplateName & " be generated. System created LetterID(" & _LetterID & " / " & _MaxID & " )", Transaction)
            ElseIf _LineOfBusiness = "ELIGIBILITY" Then
                LettersDAL.CreateLetterRequest(letterTemplateID, letterTemplateName, _AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0), _AddressesDS.Tables("PATIENT_ADDRESS").Rows(0), _LINES, _DATES, _AMTS2, _AMTS4, _NUMS, _TEXTS, _LetterID, _MaxID, _LineOfBusiness, _LetterTemplateName, Transaction)
                LettersDAL.CreateEligibilityHistory(_DocumentID, "INSERTED", _FamilyID, CShort(_RelationID), CInt(_AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("SSNO")), CInt(_AddressesDS.Tables("PATIENT_ADDRESS").Rows(0)("SSNO")), _DocumentType, "Letter (" & _LetterTemplateName & ") Requested", "Adjuster " & _DomainUser & " requested that letter " & _LetterTemplateID & "/" & _LetterTemplateName & " be generated. System created LetterID(" & _LetterID & " / " & _MaxID & " )", Transaction)
            ElseIf _LineOfBusiness = "PENSION" Then
                LettersDAL.CreateLetterRequest(letterTemplateID, letterTemplateName, _AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0), _AddressesDS.Tables("PATIENT_ADDRESS").Rows(0), _LINES, _DATES, _AMTS2, _AMTS4, _NUMS, _TEXTS, _LetterID, _MaxID, _LineOfBusiness, _LetterTemplateName, Transaction)
                '                LettersDAL.CreatePensionHistory(_DocumentID, "INSERTED", _FamilyID, CShort(_RelationID), CInt(_AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("SSNO")), CInt(_AddressesDS.Tables("PATIENT_ADDRESS").Rows(0)("SSNO")), _DocumentType, "Letter (" & _LetterTemplateName & ") Requested", "Adjuster " & _DomainUser & " requested that letter " & _LetterTemplateID & "/" & _LetterTemplateName & " be generated. System created LetterID(" & _LetterID & " / " & _MaxID & " )", Transaction)
            End If

            LettersDAL.CommitTransaction(Transaction)

            RaiseEvent PendedDuration(_DaysToPendClaim, _Hide)

        Catch ex As Exception

            Try

                If Transaction IsNot Nothing AndAlso Transaction.Connection IsNot Nothing AndAlso Transaction.Connection.State <> ConnectionState.Closed Then
                    LettersDAL.RollbackTransaction(Transaction)
                End If

            Finally
            End Try

            Throw
        Finally
            If Transaction IsNot Nothing Then Transaction.Dispose()
            Transaction = Nothing

        End Try

    End Sub
    Private Sub LoadLettersDS()
        Dim LettersDT As DataTable
        Dim DV As DataView
        Try

            Using WC As New GlobalCursor

                LettersDataGrid.DataSource = Nothing

                _DS = Nothing

                LettersDT = LettersDAL.RetrieveLetters(_LineOfBusiness)

                If LettersDT IsNot Nothing Then
                    DV = LettersDT.AsDataView
                    DV.Sort = "LETTER_NAME"
                    LettersDataGrid.DataSource = DV
                    LettersDataGrid.SetTableStyle()
                End If

                If _AddressesDS IsNot Nothing Then
                    If _AddressesDS.Tables.Contains("PATIENT_ADDRESS") Then _AddressesDS.Tables("PATIENT_ADDRESS").Rows.Clear()
                    If _AddressesDS.Tables.Contains("PARTICIPANT_ADDRESS") Then _AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows.Clear()
                End If
                _AddressesDS = LettersDAL.GetPARTICIPANTAddresses(_FamilyID, _RelationID, _AddressesDS)
            End Using

        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub TableStyleReset(ByVal sender As Object, e As EventArgs)
        Dim dg As DataGridCustom = CType(sender, DataGridCustom)
        SetTableStyleColumns(dg)
    End Sub

    Private Sub SetTableStyleColumns(ByVal dg As DataGridCustom)

        Dim DGTableStyle As DataGridTableStyle
        Dim TextCol As DataGridHighlightTextBoxColumn
        Dim Col As Integer
        Dim ColsDV As DataView
        Dim XMLStyleName As String

        Dim DSDefaultStyle As DataSet

        Try

            Try
                XMLStyleName = CStr(CType(ConfigurationManager.GetSection(dg.Name), IDictionary)("StyleLocation"))
                DSDefaultStyle = CMSXMLHandler.CreateDataSetFromXML(XMLStyleName)
                Dim ColumnSequenceDV As New DataView(DSDefaultStyle.Tables(1))

                ColumnSequenceDV.Sort = "DefaultOrder"

                ColsDV = ColumnSequenceDV

            Catch ex As Exception
                Throw
            End Try

            If dg.GetCurrentDataTable Is Nothing Then
                DGTableStyle.MappingName = dg.Name
            Else
                DGTableStyle.MappingName = dg.GetCurrentDataTable.TableName
            End If

            DGTableStyle.GridColumnStyles.Clear()
            DGTableStyle.GridLineStyle = DataGridLineStyle.None

            For Col = 0 To ColsDV.Count - 1
                If ColsDV(Col).Item("Type").ToString.Trim = "Text" Then
                    TextCol = New DataGridHighlightTextBoxColumn
                    TextCol.MappingName = CStr(ColsDV(Col).Item("Mapping"))
                    TextCol.HeaderText = CStr(ColsDV(Col).Item("HeaderText"))
                    TextCol.Width = CInt(GetSetting(_APPKEY, dg.Name & "\ColumnSettings", "Col " & ColsDV(Col).Item("Mapping").ToString, CStr(UFCWGeneral.MeasureWidthinPixels(CInt(ColsDV(Col).Item("DefaultCharWidth")), dg.Font.Name, dg.Font.Size))))
                    TextCol.NullText = CStr(ColsDV(Col).Item("NullText"))
                    TextCol.TextBox.WordWrap = True

                    Try

                        If CBool(ColsDV(Col).Item("ReadOnly")) Then
                            TextCol.ReadOnly = True
                        End If

                    Catch ex As Exception

                    End Try

                    If IsDBNull(ColsDV(Col).Item("Format")) = False AndAlso ColsDV(Col).Item("Format").ToString.Trim.Length > 0 Then
                        TextCol.Format = CStr(ColsDV(Col).Item("Format"))
                    End If

                    DGTableStyle.GridColumnStyles.Add(TextCol)
                Else
                    _ButtCol = New DataGridHighlightButtonColumn(dg)
                    _ButtCol.Alignment = HorizontalAlignment.Left
                    _ButtCol.MappingName = CStr(ColsDV(Col).Item("Mapping"))
                    _ButtCol.HeaderText = CStr(ColsDV(Col).Item("HeaderText"))
                    _ButtCol.Width = CInt(GetSetting(_APPKEY, dg.Name & "\ColumnSettings", "Col " & ColsDV(Col).Item("Mapping").ToString, CStr(UFCWGeneral.MeasureWidthinPixels(CInt(ColsDV(Col).Item("DefaultCharWidth")), dg.Font.Name, dg.Font.Size))))
                    _ButtCol.NullText = CStr(ColsDV(Col).Item("NullText"))
                    _ButtCol.TextBox.WordWrap = True

                    _D = [Delegate].CreateDelegate(GetType(System.EventHandler), Me, ColsDV(Col).Item("Method").ToString)
                    AddHandler _ButtCol.ColumnButton.Click, CType(_D, Global.System.EventHandler)

                    ' coltxtbx = TextCol.TextBox
                    'AddHandler TextCol.CellFormat, AddressOf DataGrid_CellFormating

                    DGTableStyle.PreferredRowHeight = (DGTableStyle.PreferredRowHeight * 2) + 2
                    DGTableStyle.GridColumnStyles.Add(_ButtCol)
                End If

            Next

            dg.TableStyles.Clear()
            dg.TableStyles.Add(DGTableStyle)

        Catch ex As Exception
            Throw
        Finally
            DGTableStyle = Nothing
        End Try

    End Sub
    Sub DataGrid_CellFormat(ByVal col As DataGridHighlightTextBoxColumn, ByVal rowindex As Integer, ByRef cellvalue As Object, ByRef cellFormat As String)

        Try

            Try

                If LettersDataGrid.DataSource.ToString.IndexOf("LETTER_BOOKMARKS") > -1 AndAlso col.MappingName = "MODIFIABLE" Then
                    Select Case CType(cellvalue.ToString, Integer)
                        Case 0
                            col.ReadOnly = False
                        Case Else
                            col.ReadOnly = True
                    End Select
                End If

            Catch ex As Exception
            End Try

            If col.Format.Equals("1stCharacterMask") Then
                Dim FirstChar As String = CStr(cellvalue).Substring(0, 1)

                Select Case FirstChar
                    Case "T"
                        cellFormat = "{0:00'-'0000000}"
                    Case "S"
                        cellFormat = "{0:000'-'00'-'0000}"
                    Case Else
                        cellFormat = "{0}" ' instructs format to reflect original value
                End Select

                cellvalue = CInt(CStr(cellvalue).Substring(1))

            ElseIf col.Format.Equals("LicenseCharacterMask") Then

            ElseIf col.Format.Equals("DecimalBoolean2YesNo") Then

                Select Case CType(cellvalue.ToString, Integer)
                    Case 0
                        cellFormat = "No"
                    Case Else
                        cellFormat = "Yes"
                End Select

            ElseIf col.Format.Equals("CompressSpacesMask") Then

            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub ShowDetailLineText(ByRef headerDescription As String, ByRef bookMarkText As String, ByVal modifiable As Boolean, ByVal outputColumn As String, ByVal participantDR As DataRow, ByVal patientDR As DataRow)

        Dim DataCapture As frmDataCapture

        Try

            'todo: extend this to dynamically read from xml

            bookMarkText = Replace(bookMarkText, "\n", ControlChars.CrLf)
            bookMarkText = Replace(bookMarkText, "\t", ControlChars.Tab)
            bookMarkText = Replace(bookMarkText, "@CURRENTDATE", UFCWGeneral.NowDate.ToString("MM/dd/yyyy"))
            bookMarkText = Replace(bookMarkText, "@CURRENTDAY", UFCWGeneral.NowDate.ToString("dddd"))
            bookMarkText = Replace(bookMarkText, "@CURRENTMONTH", UFCWGeneral.NowDate.ToString("MMMM"))
            bookMarkText = Replace(bookMarkText, "@CURRENTTIME", UFCWGeneral.NowDate.ToString("h:mm tt"))
            bookMarkText = Replace(bookMarkText, "@CURRENTUSER", _DomainUser) 'needs to be coded
            bookMarkText = Replace(bookMarkText, "@CURRENTUSERABBREV", "") 'needs to be coded
            bookMarkText = Replace(bookMarkText, "@PATIENTFULLNAME", patientDR("FIRST_NAME").ToString & " " & patientDR("MIDDLE_INITIAL").ToString & " " & patientDR("LAST_NAME").ToString) 'needs to be coded
            bookMarkText = Replace(bookMarkText, "@PATIENTFIRSTNAME", patientDR("FIRST_NAME").ToString) 'needs to be coded
            bookMarkText = Replace(bookMarkText, "@PATIENTLASTNAME", patientDR("LAST_NAME").ToString) 'needs to be coded
            bookMarkText = Replace(bookMarkText, "@PARTICIPANTFULLNAME", participantDR("FIRST_NAME").ToString & " " & participantDR("MIDDLE_INITIAL").ToString & " " & participantDR("LAST_NAME").ToString) 'needs to be coded
            bookMarkText = Replace(bookMarkText, "@PARTICIPANTFIRSTNAME", participantDR("FIRST_NAME").ToString) 'needs to be coded
            bookMarkText = Replace(bookMarkText, "@PARTICIPANTLASTNAME", participantDR("LAST_NAME").ToString) 'needs to be coded

            NativeMethods.SetNativeEnabled(Me.Handle, False)

            DataCapture = New frmDataCapture(headerDescription, bookMarkText, modifiable, outputColumn)

            DataCapture.ShowDialog(Me)

            Select Case DataCapture.DialogResult
                Case DialogResult.OK
                    bookMarkText = DataCapture.BookMarkText
                    bookMarkText = Replace(bookMarkText, ControlChars.CrLf, "\n")
                    bookMarkText = Replace(bookMarkText, ControlChars.Lf, "\n")
                    bookMarkText = Replace(bookMarkText, ControlChars.NewLine, "\n")
                    bookMarkText = Replace(bookMarkText, ControlChars.Tab, "\t")
                Case DialogResult.Cancel
                    bookMarkText = ""
            End Select

        Catch ex As Exception
            Throw
        Finally
            DataCapture.Dispose()
            DataCapture = Nothing

            NativeMethods.SetNativeEnabled(Me.Handle, True)
        End Try
    End Sub
    Private Sub ModifyBookMarkText()

        Dim BM As BindingManagerBase
        Dim DR As DataRow
        Dim DG As DataGridCustom
        Dim BookMarkText As String = ""

        Try

            DG = CType(LettersDataGrid, DataGridCustom)

            If DG.GetGridRowCount = 0 Then Exit Sub

            BM = DG.BindingContext(DG.DataSource, DG.DataMember)
            DR = CType(BM.Current, DataRowView).Row

            If DR IsNot Nothing Then

                If CBool(DR("MODIFIABLE")) Then
                    LettersDataGrid.AllowEdit = True

                    If Not IsDBNull(DR("MODIFIED_TEXT")) AndAlso DR("MODIFIED_TEXT").ToString.Trim.Length > 0 Then
                        BookMarkText = DR("MODIFIED_TEXT").ToString.Trim
                    ElseIf Not IsDBNull(DR("DEFAULT_TEXT")) AndAlso DR("DEFAULT_TEXT").ToString.Trim.Length > 0 Then
                        BookMarkText = DR("DEFAULT_TEXT").ToString.Trim
                    End If

                    ShowDetailLineText(DR("DESCRIPTION").ToString.Trim, BookMarkText, CBool(DR("MODIFIABLE")), DR("OUTPUT_COLUMN").ToString, _AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0), _AddressesDS.Tables("PATIENT_ADDRESS").Rows(0))

                    If BookMarkText IsNot Nothing AndAlso BookMarkText.Trim.Length > 0 Then
                        DR("MODIFIED_TEXT") = BookMarkText.Trim
                        DR.RowError = ""
                    End If

                Else
                    MsgBox("Data cannot be modified", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Invalid request")
                    LettersDataGrid.Refresh()
                End If

            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub ModifyTextButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Status.Text = ""

        ModifyBookMarkText()

    End Sub
    Private Function ValidateBookmarks() As Boolean

        Dim DT As DataTable = CType(LettersDataGrid.DataSource, DataTable)

        Try
            For Each DR As DataRow In DT.Rows

                If CBool(DR.Item("REQUIRED")) Then
                    If Not IsDBNull(DR.Item("MODIFIED_TEXT")) Then
                        If DR.Item("MODIFIED_TEXT").ToString.Trim.Length < 1 Then
                            DR.RowError = "Valid entry is required"
                        ElseIf CStr(DR.Item("MODIFIED_TEXT")).IndexOf("@") > 0 Then
                            DR.RowError = "Remove all placement markers (@)"
                        ElseIf DR.Item("OUTPUT_COLUMN").ToString.Contains("DATE") Then
                            If Not IsDate(DR.Item("MODIFIED_TEXT").ToString.Trim) Then
                                DR.RowError = "Valid date required."
                            End If
                        ElseIf DR.Item("OUTPUT_COLUMN").ToString.Contains("AMT") OrElse DR.Item("OUTPUT_COLUMN").ToString.Contains("NUM") Then
                            If Not IsNumeric(DR.Item("MODIFIED_TEXT").ToString.Trim) Then
                                DR.RowError = "Must be an Amount or Number."
                            End If
                        End If
                    Else
                        DR.RowError = "Valid entry is required"
                    End If
                Else
                    If Not IsDBNull(DR.Item("MODIFIED_TEXT")) Then
                        If DR.Item("MODIFIED_TEXT").ToString.Trim.Length > 0 Then
                            If CStr(DR.Item("MODIFIED_TEXT")).IndexOf("@") > 0 Then
                                DR.RowError = "Remove all placement markers (@)"
                            ElseIf DR.Item("OUTPUT_COLUMN").ToString.Contains("DATE") Then
                                If Not IsDate(DR.Item("MODIFIED_TEXT").ToString.Trim) Then
                                    DR.RowError = "Valid date required."
                                End If
                            ElseIf DR.Item("OUTPUT_COLUMN").ToString.Contains("AMT") OrElse DR.Item("OUTPUT_COLUMN").ToString.Contains("NUM") Then
                                If Not IsNumeric(DR.Item("MODIFIED_TEXT").ToString.Trim) Then
                                    DR.RowError = "Must be an Amount or Number."
                                End If
                            End If
                        End If
                    End If
                End If
            Next

            If DT.HasErrors Then Return False

            Return True

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Function
    Private Sub TransferBookmarks()

        Dim LineX As Integer = 0
        Dim TEXTX As Integer = 0
        Dim AMTS2X As Integer = 0
        Dim AMTS4X As Integer = 0
        Dim DATESX As Integer = 0
        Dim NUMSX As Integer = 0

        Dim DT As DataTable = CType(LettersDataGrid.DataSource, DataTable)

        Try

            Array.Clear(_LINES, 0, 9)
            Array.Clear(_TEXTS, 0, 30)
            Array.Clear(_AMTS2, 0, 10)
            Array.Clear(_AMTS4, 0, 5)
            Array.Clear(_DATES, 0, 8)
            Array.Clear(_NUMS, 0, 5)

            _NUMS(NUMSX) = _ClaimID
            NUMSX += 1
            _NUMS(NUMSX) = _ProviderID
            NUMSX += 1
            _NUMS(NUMSX) = CInt(_AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("SSNO"))
            NUMSX += 1

            _TEXTS(TEXTX) = _AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("LAST_NAME").ToString 'Text11
            TEXTX += 1
            _TEXTS(TEXTX) = _AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("FIRST_NAME").ToString 'Text12
            TEXTX += 1
            _TEXTS(TEXTX) = _AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("MIDDLE_INITIAL").ToString 'Text13
            TEXTX += 1
            _TEXTS(TEXTX) = _AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("ADDRESS1").ToString 'Text14
            TEXTX += 1
            _TEXTS(TEXTX) = _AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("ADDRESS2").ToString 'Text15
            TEXTX += 1
            _TEXTS(TEXTX) = _AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("CITY").ToString 'Text16
            TEXTX += 1
            _TEXTS(TEXTX) = _AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("STATE").ToString 'Text17
            TEXTX += 1
            _TEXTS(TEXTX) = If(Not IsDBNull(_AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("ZIP1")) AndAlso _AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("ZIP1").ToString.Trim.Length > 0 AndAlso CInt(_AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("ZIP1")) > 0, CInt(_AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("ZIP1")).ToString("D5") & If(Not IsDBNull(_AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("ZIP2")) AndAlso _AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("ZIP2").ToString.Trim.Length > 0 AndAlso CInt(_AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("ZIP2")) > 0, "-" & CInt(_AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("ZIP2")).ToString("D4"), ""), "") 'Text18
            TEXTX += 1
            _TEXTS(TEXTX) = If(CBool(_AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("FOREIGN_SW")), _AddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("COUNTRY"), "").ToString 'Text19
            TEXTX += 1

            Dim TruncateLength As Integer = If(_LineOfBusiness = "CLAIMS", 2000, 90)

            For Each DR As DataRow In DT.Rows
                Select Case True
                    Case DR.Item("OUTPUT_COLUMN").ToString.Contains("LINE")
                        _LINES(LineX) = DR.Item("MODIFIED_TEXT").ToString.TrimEnd.Substring(0, If(DR.Item("MODIFIED_TEXT").ToString.TrimEnd.Length > TruncateLength, TruncateLength, DR.Item("MODIFIED_TEXT").ToString.TrimEnd.Length))
                        LineX += 1
                    Case DR.Item("OUTPUT_COLUMN").ToString.Contains("TEXT")
                        _TEXTS(TEXTX) = DR.Item("MODIFIED_TEXT").ToString.TrimEnd.Substring(0, If(DR.Item("MODIFIED_TEXT").ToString.TrimEnd.Length > 90, 90, DR.Item("MODIFIED_TEXT").ToString.TrimEnd.Length))
                        TEXTX += 1
                    Case DR.Item("OUTPUT_COLUMN").ToString.Contains("AMT2")
                        _AMTS2(AMTS2X) = CDec(DR.Item("MODIFIED_TEXT"))
                        AMTS2X += 1
                    Case DR.Item("OUTPUT_COLUMN").ToString.Contains("AMT4")
                        _AMTS4(AMTS2X) = CDec(DR.Item("MODIFIED_TEXT"))
                        AMTS4X += 1
                    Case DR.Item("OUTPUT_COLUMN").ToString.Contains("DATE")
                        _DATES(DATESX) = CDate(DR.Item("MODIFIED_TEXT"))
                        DATESX += 1
                    Case DR.Item("OUTPUT_COLUMN").ToString.Contains("NUM")
                        _NUMS(NUMSX) = CInt(DR.Item("MODIFIED_TEXT"))
                        NUMSX += 1
                End Select
            Next

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub
    Private Sub LettersDataGrid_MouseHover(ByVal sender As Object, ByVal e As EventArgs) Handles LettersDataGrid.MouseHover

        Try

            If _HoverCell.RowNumber > -1 AndAlso _HoverCell.RowNumber <= CType(sender, DataGridCustom).VisibleRowCount Then
                Me.ToolTip.SetToolTip(Me.LettersDataGrid, "")
            Else
                Me.ToolTip.SetToolTip(Me.LettersDataGrid, "")
            End If

        Catch ex As Exception
        End Try

    End Sub
    Private Sub LettersDataGrid_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles LettersDataGrid.MouseMove

        Dim HTI As DataGrid.HitTestInfo
        HTI = CType(sender, DataGridCustom).HitTest(e.X, e.Y)

        ' Do not display hover text if it is a drag event
        If (e.Button <> MouseButtons.Left) Then

            ' Check if the target is a different cell from the previous one
            If HTI.Type = DataGrid.HitTestType.Cell AndAlso (HTI.Row <> _HoverCell.RowNumber OrElse HTI.Column <> _HoverCell.ColumnNumber) Then

                ' Store the new hit row
                _HoverCell.RowNumber = HTI.Row
                _HoverCell.ColumnNumber = HTI.Column

            End If

        End If

    End Sub

#End Region

End Class