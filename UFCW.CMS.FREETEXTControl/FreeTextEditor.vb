Option Strict On
Option Infer On

Imports System.ComponentModel
Imports System.Data.DataTableExtensions

Public Class FreeTextEditor
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _FamilyID As Integer
    Private _RelationID As Short? = Nothing
    Private _PartSSN As Integer
    Private _PatSSN As Integer
    Private _PartFName As String
    Private _PartLName As String
    Private _PatFName As String
    Private _PatLName As String
    Private _PatAlert As Boolean
    Private _PartAlert As Boolean
    Private _APPKEY As String = "UFCW\Claims\"
    Private _ReadOnly As Boolean = False
    Private _AppName As String = ""
    Private _AllowSearch As Boolean = False

    Private _Loading As Boolean = True
    Private _FamilyFreeTextDV As DataView
    Private _PatientFreeTextDV As DataView
    Private _BlinkStartTime As Nullable(Of DateTime)

    Private WithEvents FreeTextDataSet As DataSet
    Private WithEvents FamilyFreeTextDataGrid As DataGridCustom
    Private WithEvents PatientFreeTextDataGrid As DataGridCustom

    ReadOnly _DomainUser As String = SystemInformation.UserName

    Private Const EM_SETREADONLY As Integer = &HCF
    Private Const GW_Child As Integer = 5
    Friend WithEvents BlinkTimer As System.Windows.Forms.Timer
    Friend WithEvents AlertPanel As System.Windows.Forms.Panel
    Friend WithEvents AlertsToolStrip As ToolStrip
    Friend WithEvents PatientHighAlertLabel As ToolStripLabel
    Friend WithEvents PatientAlertLabel As ToolStripLabel
    Friend WithEvents NotesSplitContainer As System.Windows.Forms.SplitContainer

    Private Function EnumChildWindowsCallBackFunction(ByVal cb As ComboBox, ByVal hWnd As IntPtr, ByVal lparam As IntPtr) As Boolean

        If hWnd <> IntPtr.Zero Then

            Dim ReadonlyValue As IntPtr = If((_ReadOnly), New IntPtr(1), IntPtr.Zero)

            NativeMethods.SendMessage(hWnd, EM_SETREADONLY, ReadonlyValue, IntPtr.Zero)

            cb.Invalidate()

            Return True
        End If

        Return False

    End Function
    Public Overloads Sub Dispose()

        If _FamilyFreeTextDV IsNot Nothing Then _FamilyFreeTextDV.Dispose()
        _FamilyFreeTextDV = Nothing
        If _PatientFreeTextDV IsNot Nothing Then _PatientFreeTextDV.Dispose()
        _PatientFreeTextDV = Nothing
        If FreeTextDataSet IsNot Nothing Then FreeTextDataSet.Dispose()
        FreeTextDataSet = Nothing
        If FamilyFreeTextDataGrid IsNot Nothing Then FamilyFreeTextDataGrid.Dispose()
        FamilyFreeTextDataGrid = Nothing
        If PatientFreeTextDataGrid IsNot Nothing Then PatientFreeTextDataGrid.Dispose()
        PatientFreeTextDataGrid = Nothing

        MyBase.Dispose()

    End Sub
#Region " Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        Try

            'Add any initialization after the InitializeComponent() call
            Dim designMode As Boolean = (LicenseManager.UsageMode = LicenseUsageMode.Designtime)

            If Not designMode Then

            End If

        Catch ex As Exception

        End Try

    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Private WithEvents PatientNotesRadioButton As System.Windows.Forms.RadioButton
    Private WithEvents FamilyNotesRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents PatientGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents Label82 As System.Windows.Forms.Label
    Friend WithEvents Label83 As System.Windows.Forms.Label
    Friend WithEvents FamilyGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label72 As System.Windows.Forms.Label
    Private WithEvents NotesCancelButton As System.Windows.Forms.Button
    Private WithEvents NotesSaveButton As System.Windows.Forms.Button
    Private WithEvents NotesTextBox As System.Windows.Forms.TextBox
    Friend WithEvents EnterNotesLabel As System.Windows.Forms.Label
    Public WithEvents NotesRelationIDTextBox As System.Windows.Forms.TextBox
    Public WithEvents NotesPatSSNTextBox As ExTextBox
    Public WithEvents NotesFamilyIDTextBox As ExTextBox
    Public WithEvents NotesPartSSNTextBox As ExTextBox

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.PatientNotesRadioButton = New System.Windows.Forms.RadioButton()
        Me.FamilyNotesRadioButton = New System.Windows.Forms.RadioButton()
        Me.PatientGroupBox = New System.Windows.Forms.GroupBox()
        Me.NotesRelationIDTextBox = New System.Windows.Forms.TextBox()
        Me.Label82 = New System.Windows.Forms.Label()
        Me.NotesPatSSNTextBox = New ExTextBox()
        Me.Label83 = New System.Windows.Forms.Label()
        Me.FamilyGroupBox = New System.Windows.Forms.GroupBox()
        Me.NotesFamilyIDTextBox = New ExTextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.NotesPartSSNTextBox = New ExTextBox()
        Me.Label72 = New System.Windows.Forms.Label()
        Me.NotesCancelButton = New System.Windows.Forms.Button()
        Me.NotesSaveButton = New System.Windows.Forms.Button()
        Me.NotesTextBox = New System.Windows.Forms.TextBox()
        Me.EnterNotesLabel = New System.Windows.Forms.Label()
        Me.NotesSplitContainer = New System.Windows.Forms.SplitContainer()
        Me.FamilyFreeTextDataGrid = New DataGridCustom()
        Me.PatientFreeTextDataGrid = New DataGridCustom()
        Me.BlinkTimer = New System.Windows.Forms.Timer(Me.components)
        Me.AlertPanel = New System.Windows.Forms.Panel()
        Me.AlertsToolStrip = New System.Windows.Forms.ToolStrip()
        Me.PatientHighAlertLabel = New System.Windows.Forms.ToolStripLabel()
        Me.PatientAlertLabel = New System.Windows.Forms.ToolStripLabel()
        Me.PatientGroupBox.SuspendLayout()
        Me.FamilyGroupBox.SuspendLayout()
        CType(Me.NotesSplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.NotesSplitContainer.Panel1.SuspendLayout()
        Me.NotesSplitContainer.Panel2.SuspendLayout()
        Me.NotesSplitContainer.SuspendLayout()
        CType(Me.FamilyFreeTextDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PatientFreeTextDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.AlertPanel.SuspendLayout()
        Me.AlertsToolStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'PatientNotesRadioButton
        '
        Me.PatientNotesRadioButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PatientNotesRadioButton.Location = New System.Drawing.Point(404, 50)
        Me.PatientNotesRadioButton.Name = "PatientNotesRadioButton"
        Me.PatientNotesRadioButton.Size = New System.Drawing.Size(92, 20)
        Me.PatientNotesRadioButton.TabIndex = 20
        Me.PatientNotesRadioButton.Text = "Patient Notes"
        '
        'FamilyNotesRadioButton
        '
        Me.FamilyNotesRadioButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyNotesRadioButton.Checked = True
        Me.FamilyNotesRadioButton.Location = New System.Drawing.Point(308, 50)
        Me.FamilyNotesRadioButton.Name = "FamilyNotesRadioButton"
        Me.FamilyNotesRadioButton.Size = New System.Drawing.Size(92, 20)
        Me.FamilyNotesRadioButton.TabIndex = 19
        Me.FamilyNotesRadioButton.TabStop = True
        Me.FamilyNotesRadioButton.Text = "Family Notes"
        '
        'PatientGroupBox
        '
        Me.PatientGroupBox.Controls.Add(Me.NotesRelationIDTextBox)
        Me.PatientGroupBox.Controls.Add(Me.Label82)
        Me.PatientGroupBox.Controls.Add(Me.NotesPatSSNTextBox)
        Me.PatientGroupBox.Controls.Add(Me.Label83)
        Me.PatientGroupBox.Location = New System.Drawing.Point(252, 8)
        Me.PatientGroupBox.Name = "PatientGroupBox"
        Me.PatientGroupBox.Size = New System.Drawing.Size(256, 40)
        Me.PatientGroupBox.TabIndex = 15
        Me.PatientGroupBox.TabStop = False
        Me.PatientGroupBox.Text = "Patient:"
        '
        'NotesRelationIDTextBox
        '
        Me.NotesRelationIDTextBox.Location = New System.Drawing.Point(68, 16)
        Me.NotesRelationIDTextBox.Name = "NotesRelationIDTextBox"
        Me.NotesRelationIDTextBox.ReadOnly = True
        Me.NotesRelationIDTextBox.Size = New System.Drawing.Size(72, 20)
        Me.NotesRelationIDTextBox.TabIndex = 0
        '
        'Label82
        '
        Me.Label82.AutoSize = True
        Me.Label82.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label82.Location = New System.Drawing.Point(4, 20)
        Me.Label82.Name = "Label82"
        Me.Label82.Size = New System.Drawing.Size(60, 13)
        Me.Label82.TabIndex = 16
        Me.Label82.Text = "Relation ID"
        '
        'NotesPatSSNTextBox
        '
        Me.NotesPatSSNTextBox.Location = New System.Drawing.Point(180, 16)
        Me.NotesPatSSNTextBox.MaxLength = 11
        Me.NotesPatSSNTextBox.Name = "NotesPatSSNTextBox"
        Me.NotesPatSSNTextBox.ReadOnly = True
        Me.NotesPatSSNTextBox.Size = New System.Drawing.Size(72, 20)
        Me.NotesPatSSNTextBox.TabIndex = 1
        '
        'Label83
        '
        Me.Label83.AutoSize = True
        Me.Label83.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label83.Location = New System.Drawing.Point(148, 20)
        Me.Label83.Name = "Label83"
        Me.Label83.Size = New System.Drawing.Size(29, 13)
        Me.Label83.TabIndex = 14
        Me.Label83.Text = "SSN"
        '
        'FamilyGroupBox
        '
        Me.FamilyGroupBox.Controls.Add(Me.NotesFamilyIDTextBox)
        Me.FamilyGroupBox.Controls.Add(Me.Label12)
        Me.FamilyGroupBox.Controls.Add(Me.NotesPartSSNTextBox)
        Me.FamilyGroupBox.Controls.Add(Me.Label72)
        Me.FamilyGroupBox.Location = New System.Drawing.Point(4, 8)
        Me.FamilyGroupBox.Name = "FamilyGroupBox"
        Me.FamilyGroupBox.Size = New System.Drawing.Size(248, 40)
        Me.FamilyGroupBox.TabIndex = 13
        Me.FamilyGroupBox.TabStop = False
        Me.FamilyGroupBox.Text = "Family:"
        '
        'NotesFamilyIDTextBox
        '
        Me.NotesFamilyIDTextBox.Location = New System.Drawing.Point(60, 16)
        Me.NotesFamilyIDTextBox.Name = "NotesFamilyIDTextBox"
        Me.NotesFamilyIDTextBox.ReadOnly = True
        Me.NotesFamilyIDTextBox.Size = New System.Drawing.Size(72, 20)
        Me.NotesFamilyIDTextBox.TabIndex = 0
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label12.Location = New System.Drawing.Point(4, 20)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(50, 13)
        Me.Label12.TabIndex = 16
        Me.Label12.Text = "Family ID"
        '
        'NotesPartSSNTextBox
        '
        Me.NotesPartSSNTextBox.Location = New System.Drawing.Point(172, 16)
        Me.NotesPartSSNTextBox.MaxLength = 11
        Me.NotesPartSSNTextBox.Name = "NotesPartSSNTextBox"
        Me.NotesPartSSNTextBox.ReadOnly = True
        Me.NotesPartSSNTextBox.Size = New System.Drawing.Size(72, 20)
        Me.NotesPartSSNTextBox.TabIndex = 1
        '
        'Label72
        '
        Me.Label72.AutoSize = True
        Me.Label72.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label72.Location = New System.Drawing.Point(140, 20)
        Me.Label72.Name = "Label72"
        Me.Label72.Size = New System.Drawing.Size(29, 13)
        Me.Label72.TabIndex = 14
        Me.Label72.Text = "SSN"
        '
        'NotesCancelButton
        '
        Me.NotesCancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NotesCancelButton.Location = New System.Drawing.Point(346, 138)
        Me.NotesCancelButton.Name = "NotesCancelButton"
        Me.NotesCancelButton.Size = New System.Drawing.Size(75, 23)
        Me.NotesCancelButton.TabIndex = 18
        Me.NotesCancelButton.Text = "Cancel"
        '
        'NotesSaveButton
        '
        Me.NotesSaveButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NotesSaveButton.Enabled = False
        Me.NotesSaveButton.Location = New System.Drawing.Point(428, 138)
        Me.NotesSaveButton.Name = "NotesSaveButton"
        Me.NotesSaveButton.Size = New System.Drawing.Size(75, 23)
        Me.NotesSaveButton.TabIndex = 17
        Me.NotesSaveButton.Text = "Save"
        '
        'NotesTextBox
        '
        Me.NotesTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NotesTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.NotesTextBox.Location = New System.Drawing.Point(8, 70)
        Me.NotesTextBox.MaxLength = 500
        Me.NotesTextBox.Multiline = True
        Me.NotesTextBox.Name = "NotesTextBox"
        Me.NotesTextBox.Size = New System.Drawing.Size(494, 56)
        Me.NotesTextBox.TabIndex = 16
        '
        'EnterNotesLabel
        '
        Me.EnterNotesLabel.AutoSize = True
        Me.EnterNotesLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EnterNotesLabel.Location = New System.Drawing.Point(9, 54)
        Me.EnterNotesLabel.Name = "EnterNotesLabel"
        Me.EnterNotesLabel.Size = New System.Drawing.Size(244, 13)
        Me.EnterNotesLabel.TabIndex = 14
        Me.EnterNotesLabel.Text = "Enter any applicable notes for this Family."
        '
        'NotesSplitContainer
        '
        Me.NotesSplitContainer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NotesSplitContainer.BackColor = System.Drawing.SystemColors.Control
        Me.NotesSplitContainer.Location = New System.Drawing.Point(4, 189)
        Me.NotesSplitContainer.Name = "NotesSplitContainer"
        Me.NotesSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'NotesSplitContainer.Panel1
        '
        Me.NotesSplitContainer.Panel1.Controls.Add(Me.FamilyFreeTextDataGrid)
        '
        'NotesSplitContainer.Panel2
        '
        Me.NotesSplitContainer.Panel2.Controls.Add(Me.PatientFreeTextDataGrid)
        Me.NotesSplitContainer.Size = New System.Drawing.Size(499, 359)
        Me.NotesSplitContainer.SplitterDistance = 178
        Me.NotesSplitContainer.TabIndex = 25
        '
        'FamilyFreeTextDataGrid
        '
        Me.FamilyFreeTextDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.FamilyFreeTextDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.FamilyFreeTextDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.FamilyFreeTextDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.FamilyFreeTextDataGrid.ADGroupsThatCanFind = ""
        Me.FamilyFreeTextDataGrid.ADGroupsThatCanMultiSort = ""
        Me.FamilyFreeTextDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.FamilyFreeTextDataGrid.AllowAutoSize = False
        Me.FamilyFreeTextDataGrid.AllowColumnReorder = False
        Me.FamilyFreeTextDataGrid.AllowCopy = True
        Me.FamilyFreeTextDataGrid.AllowCustomize = True
        Me.FamilyFreeTextDataGrid.AllowDelete = False
        Me.FamilyFreeTextDataGrid.AllowDragDrop = False
        Me.FamilyFreeTextDataGrid.AllowEdit = False
        Me.FamilyFreeTextDataGrid.AllowExport = True
        Me.FamilyFreeTextDataGrid.AllowFilter = True
        Me.FamilyFreeTextDataGrid.AllowFind = True
        Me.FamilyFreeTextDataGrid.AllowGoTo = True
        Me.FamilyFreeTextDataGrid.AllowMultiSelect = False
        Me.FamilyFreeTextDataGrid.AllowMultiSort = False
        Me.FamilyFreeTextDataGrid.AllowNew = False
        Me.FamilyFreeTextDataGrid.AllowPrint = True
        Me.FamilyFreeTextDataGrid.AllowRefresh = False
        Me.FamilyFreeTextDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyFreeTextDataGrid.AppKey = "UFCW\Claims\"
        Me.FamilyFreeTextDataGrid.AutoSaveCols = True
        Me.FamilyFreeTextDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.FamilyFreeTextDataGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FamilyFreeTextDataGrid.CaptionText = "Family Notes:"
        Me.FamilyFreeTextDataGrid.ColumnHeaderLabel = Nothing
        Me.FamilyFreeTextDataGrid.ColumnHeadersVisible = False
        Me.FamilyFreeTextDataGrid.ColumnRePositioning = False
        Me.FamilyFreeTextDataGrid.ColumnResizing = False
        Me.FamilyFreeTextDataGrid.ConfirmDelete = True
        Me.FamilyFreeTextDataGrid.CopySelectedOnly = True
        Me.FamilyFreeTextDataGrid.DataMember = ""
        Me.FamilyFreeTextDataGrid.DragColumn = 0
        Me.FamilyFreeTextDataGrid.ExportSelectedOnly = True
        Me.FamilyFreeTextDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.FamilyFreeTextDataGrid.HighlightedRow = Nothing
        Me.FamilyFreeTextDataGrid.HighLightModifiedRows = False
        Me.FamilyFreeTextDataGrid.IsMouseDown = False
        Me.FamilyFreeTextDataGrid.LastGoToLine = ""
        Me.FamilyFreeTextDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.FamilyFreeTextDataGrid.MultiSort = False
        Me.FamilyFreeTextDataGrid.Name = "FamilyFreeTextDataGrid"
        Me.FamilyFreeTextDataGrid.OldSelectedRow = Nothing
        Me.FamilyFreeTextDataGrid.ReadOnly = True
        Me.FamilyFreeTextDataGrid.RetainRowSelectionAfterSort = True
        Me.FamilyFreeTextDataGrid.RowHeadersVisible = False
        Me.FamilyFreeTextDataGrid.SetRowOnRightClick = True
        Me.FamilyFreeTextDataGrid.ShiftPressed = False
        Me.FamilyFreeTextDataGrid.SingleClickBooleanColumns = True
        Me.FamilyFreeTextDataGrid.Size = New System.Drawing.Size(499, 180)
        Me.FamilyFreeTextDataGrid.Sort = Nothing
        Me.FamilyFreeTextDataGrid.StyleName = ""
        Me.FamilyFreeTextDataGrid.SubKey = ""
        Me.FamilyFreeTextDataGrid.SuppressTriangle = False
        Me.FamilyFreeTextDataGrid.TabIndex = 11
        '
        'PatientFreeTextDataGrid
        '
        Me.PatientFreeTextDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.PatientFreeTextDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.PatientFreeTextDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PatientFreeTextDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PatientFreeTextDataGrid.ADGroupsThatCanFind = ""
        Me.PatientFreeTextDataGrid.ADGroupsThatCanMultiSort = ""
        Me.PatientFreeTextDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PatientFreeTextDataGrid.AllowAutoSize = True
        Me.PatientFreeTextDataGrid.AllowColumnReorder = False
        Me.PatientFreeTextDataGrid.AllowCopy = True
        Me.PatientFreeTextDataGrid.AllowCustomize = True
        Me.PatientFreeTextDataGrid.AllowDelete = False
        Me.PatientFreeTextDataGrid.AllowDragDrop = False
        Me.PatientFreeTextDataGrid.AllowEdit = False
        Me.PatientFreeTextDataGrid.AllowExport = True
        Me.PatientFreeTextDataGrid.AllowFilter = True
        Me.PatientFreeTextDataGrid.AllowFind = True
        Me.PatientFreeTextDataGrid.AllowGoTo = True
        Me.PatientFreeTextDataGrid.AllowMultiSelect = False
        Me.PatientFreeTextDataGrid.AllowMultiSort = False
        Me.PatientFreeTextDataGrid.AllowNew = False
        Me.PatientFreeTextDataGrid.AllowPrint = True
        Me.PatientFreeTextDataGrid.AllowRefresh = False
        Me.PatientFreeTextDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PatientFreeTextDataGrid.AppKey = "UFCW\Claims\"
        Me.PatientFreeTextDataGrid.AutoSaveCols = True
        Me.PatientFreeTextDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.PatientFreeTextDataGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PatientFreeTextDataGrid.CaptionText = "Patient Notes:"
        Me.PatientFreeTextDataGrid.ColumnHeaderLabel = Nothing
        Me.PatientFreeTextDataGrid.ColumnHeadersVisible = False
        Me.PatientFreeTextDataGrid.ColumnRePositioning = False
        Me.PatientFreeTextDataGrid.ColumnResizing = False
        Me.PatientFreeTextDataGrid.ConfirmDelete = True
        Me.PatientFreeTextDataGrid.CopySelectedOnly = True
        Me.PatientFreeTextDataGrid.DataMember = ""
        Me.PatientFreeTextDataGrid.DragColumn = 0
        Me.PatientFreeTextDataGrid.ExportSelectedOnly = True
        Me.PatientFreeTextDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.PatientFreeTextDataGrid.HighlightedRow = Nothing
        Me.PatientFreeTextDataGrid.HighLightModifiedRows = False
        Me.PatientFreeTextDataGrid.IsMouseDown = False
        Me.PatientFreeTextDataGrid.LastGoToLine = ""
        Me.PatientFreeTextDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.PatientFreeTextDataGrid.MultiSort = False
        Me.PatientFreeTextDataGrid.Name = "PatientFreeTextDataGrid"
        Me.PatientFreeTextDataGrid.OldSelectedRow = Nothing
        Me.PatientFreeTextDataGrid.ReadOnly = True
        Me.PatientFreeTextDataGrid.RetainRowSelectionAfterSort = True
        Me.PatientFreeTextDataGrid.RowHeadersVisible = False
        Me.PatientFreeTextDataGrid.SetRowOnRightClick = True
        Me.PatientFreeTextDataGrid.ShiftPressed = False
        Me.PatientFreeTextDataGrid.SingleClickBooleanColumns = True
        Me.PatientFreeTextDataGrid.Size = New System.Drawing.Size(499, 178)
        Me.PatientFreeTextDataGrid.Sort = Nothing
        Me.PatientFreeTextDataGrid.StyleName = ""
        Me.PatientFreeTextDataGrid.SubKey = ""
        Me.PatientFreeTextDataGrid.SuppressTriangle = False
        Me.PatientFreeTextDataGrid.TabIndex = 16
        '
        'BlinkTimer
        '
        Me.BlinkTimer.Interval = 200
        '
        'AlertPanel
        '
        Me.AlertPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AlertPanel.AutoScroll = True
        Me.AlertPanel.Controls.Add(Me.AlertsToolStrip)
        Me.AlertPanel.Location = New System.Drawing.Point(11, 132)
        Me.AlertPanel.Name = "AlertPanel"
        Me.AlertPanel.Size = New System.Drawing.Size(332, 38)
        Me.AlertPanel.TabIndex = 28
        '
        'AlertsToolStrip
        '
        Me.AlertsToolStrip.CanOverflow = False
        Me.AlertsToolStrip.Dock = System.Windows.Forms.DockStyle.Left
        Me.AlertsToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.AlertsToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PatientHighAlertLabel, Me.PatientAlertLabel})
        Me.AlertsToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.AlertsToolStrip.Location = New System.Drawing.Point(0, 0)
        Me.AlertsToolStrip.Name = "AlertsToolStrip"
        Me.AlertsToolStrip.Size = New System.Drawing.Size(3, 38)
        Me.AlertsToolStrip.TabIndex = 29
        Me.AlertsToolStrip.Text = "ToolStrip1"
        '
        'PatientHighAlertLabel
        '
        Me.PatientHighAlertLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.PatientHighAlertLabel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.PatientHighAlertLabel.ForeColor = System.Drawing.Color.OrangeRed
        Me.PatientHighAlertLabel.Name = "PatientHighAlertLabel"
        Me.PatientHighAlertLabel.Size = New System.Drawing.Size(0, 25)
        '
        'PatientAlertLabel
        '
        Me.PatientAlertLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.PatientAlertLabel.Name = "PatientAlertLabel"
        Me.PatientAlertLabel.Size = New System.Drawing.Size(0, 25)
        '
        'FreeTextEditor
        '
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.NotesSplitContainer)
        Me.Controls.Add(Me.PatientNotesRadioButton)
        Me.Controls.Add(Me.FamilyNotesRadioButton)
        Me.Controls.Add(Me.PatientGroupBox)
        Me.Controls.Add(Me.FamilyGroupBox)
        Me.Controls.Add(Me.NotesCancelButton)
        Me.Controls.Add(Me.NotesSaveButton)
        Me.Controls.Add(Me.NotesTextBox)
        Me.Controls.Add(Me.EnterNotesLabel)
        Me.Controls.Add(Me.AlertPanel)
        Me.Name = "FreeTextEditor"
        Me.Size = New System.Drawing.Size(508, 552)
        Me.PatientGroupBox.ResumeLayout(False)
        Me.PatientGroupBox.PerformLayout()
        Me.FamilyGroupBox.ResumeLayout(False)
        Me.FamilyGroupBox.PerformLayout()
        Me.NotesSplitContainer.Panel1.ResumeLayout(False)
        Me.NotesSplitContainer.Panel2.ResumeLayout(False)
        CType(Me.NotesSplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.NotesSplitContainer.ResumeLayout(False)
        CType(Me.FamilyFreeTextDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PatientFreeTextDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.AlertPanel.ResumeLayout(False)
        Me.AlertPanel.PerformLayout()
        Me.AlertsToolStrip.ResumeLayout(False)
        Me.AlertsToolStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region "Public Properties"
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the FamilyID of the Document.")>
    Public Property FamilyID() As Integer
        Get
            Return _FamilyID
        End Get
        Set(ByVal value As Integer)
            _FamilyID = value
            NotesFamilyIDTextBox.Text = value.ToString
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the RelationID of the Document.")>
    Public Property RelationID() As Short?
        Get
            Return _RelationID
        End Get
        Set(ByVal value As Short?)
            _RelationID = value
            NotesRelationIDTextBox.Text = If(value Is Nothing, "", value.ToString)
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Participant SSN of the Document.")>
    Public Property ParticipantSSN() As Integer
        Get
            Return _PartSSN
        End Get
        Set(ByVal value As Integer)
            _PartSSN = value
            NotesPartSSNTextBox.Text = CStr(value)
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Patient SSN of the Document.")>
    Public Property PatientSSN() As Integer
        Get
            Return _PatSSN
        End Get
        Set(ByVal value As Integer)
            _PatSSN = value
            NotesPatSSNTextBox.Text = CStr(value)
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Participant First Name of the Document.")>
    Public Property ParticipantFirst() As String
        Get
            Return CStr(_PartFName)
        End Get
        Set(ByVal value As String)
            _PartFName = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Participant Last Name of the Document.")>
    Public Property ParticipantLast() As String
        Get
            Return CStr(_PartLName)
        End Get
        Set(ByVal value As String)
            _PartLName = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Patient First Name of the Document.")>
    Public Property PatientFirst() As String
        Get
            Return CStr(_PatFName)
        End Get
        Set(ByVal value As String)
            _PatFName = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Patient Last Name of the Document.")>
    Public Property PatientLast() As String
        Get
            Return CStr(_PatLName)
        End Get
        Set(ByVal value As String)
            _PatLName = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Patient Last Name of the Document.")>
    Public Property PatAlert() As Boolean
        Get
            Return _PatAlert
        End Get
        Set(ByVal value As Boolean)
            _PatAlert = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Patient Last Name of the Document.")>
    Public Property PartAlert() As Boolean
        Get
            Return _PartAlert
        End Get
        Set(ByVal value As Boolean)
            _PartAlert = value
        End Set
    End Property

    <DefaultValue(CBool(False)), Browsable(True), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
        End Set
    End Property

    <DefaultValue(CBool(False)), Browsable(True), System.ComponentModel.Description("Gets or Sets the ReadOnly state of the control.")>
    Public Property ReadOnlyMode() As Boolean
        Get
            Return _ReadOnly
        End Get
        Set(ByVal value As Boolean)
            _ReadOnly = value
        End Set
    End Property

    <DefaultValue(CBool(False)), Browsable(True), System.ComponentModel.Description("Allows the User to Type Search Parameters")>
    Public Property AllowSearch() As Boolean
        Get
            Return _AllowSearch
        End Get
        Set(ByVal value As Boolean)
            _AllowSearch = value

            NotesPartSSNTextBox.ReadOnly = value
            NotesPatSSNTextBox.ReadOnly = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets Whether The Control Contains Data")>
    Public ReadOnly Property HasData() As Boolean
        Get
            If (Me.PatientHighAlertLabel.Text.Trim.Length > 0 OrElse Me.PatientAlertLabel.Text.Trim.Length > 0) OrElse (FamilyFreeTextDataGrid IsNot Nothing AndAlso (FamilyFreeTextDataGrid.GetGridRowCount > 0 OrElse PatientFreeTextDataGrid.GetGridRowCount > 0)) Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
    <BrowsableAttribute(False)> Protected Shadows ReadOnly Property DesignMode() As Boolean

        Get

            ' Returns true if this control or any of its ancestors is in design mode()

            If MyBase.DesignMode Then

                Return True

            Else

                Dim ParentCtrl As Control = Me.Parent

                While ParentCtrl IsNot Nothing

                    Dim Site As ISite = ParentCtrl.Site

                    If Site IsNot Nothing AndAlso Site.DesignMode Then

                        Return True

                    End If

                    ParentCtrl = ParentCtrl.Parent

                End While

                Return False

            End If

        End Get

    End Property

    <DefaultValue(CBool(False)), Browsable(True), System.ComponentModel.Description("Gets or Sets the Appname.")>
    Public Property AppName() As String
        Get
            Return _AppName
        End Get
        Set(ByVal value As String)
            _AppName = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Can provide REGDATA data to process for caching purpose")>
    Public Property FreeTextDS() As DataSet
        Get
            Return FreeTextDataSet
        End Get
        Set(ByVal value As DataSet)
            FreeTextDataSet = value
        End Set
    End Property
#End Region

    Private _Disposed As Boolean = False

    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.
            If _FamilyFreeTextDV IsNot Nothing Then
                _FamilyFreeTextDV.Dispose()
            End If
            _FamilyFreeTextDV = Nothing

            If _PatientFreeTextDV IsNot Nothing Then
                _PatientFreeTextDV.Dispose()
            End If
            _PatientFreeTextDV = Nothing

            If Not (components Is Nothing) Then
                components.Dispose()
            End If

        End If

        ' Free any unmanaged objects here.
        '
        _Disposed = True

        ' Call base class implementation.
        MyBase.Dispose(disposing)
    End Sub

#Region "Constructors"
    Sub New(ByVal familyID As Integer, ByVal relationID As Short?, ByVal partSSN As Integer, ByVal patSSN As Integer, ByVal partFName As String, ByVal partLName As String, ByVal patFName As String, ByVal patLName As String)

        Me.New()

        Dim designMode As Boolean = (LicenseManager.UsageMode = LicenseUsageMode.Designtime)

        If Not designMode Then
            _FamilyID = familyID
            NotesFamilyIDTextBox.Text = CStr(familyID)
            _RelationID = relationID
            NotesRelationIDTextBox.Text = If(relationID Is Nothing, "", relationID.ToString)
            _PartSSN = partSSN
            NotesPartSSNTextBox.Text = partSSN.ToString
            _PatSSN = patSSN
            NotesPatSSNTextBox.Text = patSSN.ToString
            _PartFName = partFName
            _PartLName = partLName
            _PatFName = patFName
            _PatLName = patLName

        End If

    End Sub
#End Region

#Region "Form\Button Events"

    Private Sub FreeTextEditor_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        _Loading = False

    End Sub

    Private Sub NotesSaveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NotesSaveButton.Click
        Try
            SaveFreeText()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub NotesCancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NotesCancelButton.Click
        NotesTextBox.Text = ""
    End Sub

    Private Sub NotesTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NotesTextBox.TextChanged
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' disables the save if the user hasn't typed anything to save
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        If NotesTextBox.Text = "" Then
            NotesSaveButton.Enabled = False
        Else
            NotesSaveButton.Enabled = True
        End If
    End Sub

    Private Sub NotesPartSSNTextBox_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotesPartSSNTextBox.Validated
        If _Loading = True Then Exit Sub

        Try

            If _AllowSearch = True Then
                LookupInfo()
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub NotesPatSSNTextBox_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotesPatSSNTextBox.Validated
        If _Loading = True Then Exit Sub

        Try
            If _AllowSearch = True Then
                LookupInfo()
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub FamilyNotesRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FamilyNotesRadioButton.CheckedChanged
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' updates the label text when the user changes options
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        If AppName.Length > 0 AndAlso AppName.ToUpper = "ELIGIBILITY" Then Exit Sub
        Try
            If FamilyNotesRadioButton.Checked = True Then
                EnterNotesLabel.Text = "Enter any applicable notes for this Family."
            End If

            If _Loading = True Then Exit Try

            If Not Me.DesignMode Then
                LoadFreeText()
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub PatientNotesRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PatientNotesRadioButton.CheckedChanged
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' updates the label text when the user changes options
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        If AppName.Length > 0 AndAlso AppName.ToUpper = "ELIGIBILITY" Then Exit Sub
        Try
            If PatientNotesRadioButton.Checked = True Then
                EnterNotesLabel.Text = "Enter any applicable notes for this Patient."
            End If

            If _Loading = True Then Exit Try

            LoadFreeText()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub FamilyFreeTextDataGrid_RefreshGridData()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' refreshes the grid from the refresh menu item
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            LoadFreeText()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub PatientFreeTextDataGrid_RefreshGridData()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' refreshes the grid from the refresh menu item
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            LoadFreeText()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub NotesFamilyIDTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NotesFamilyIDTextBox.TextChanged
        If CStr(NotesFamilyIDTextBox.Text).Length > 0 Then
            If _FamilyID <> CInt(NotesFamilyIDTextBox.Text) Then
                _FamilyID = CInt(NotesFamilyIDTextBox.Text)
                LoadFreeText()
            End If
        End If
    End Sub

    Private Sub NotesPartSSNTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NotesPartSSNTextBox.TextChanged
        If CStr(NotesPartSSNTextBox.Text).Length > 0 Then
            If _PartSSN <> CInt(UFCWGeneral.UnFormatSSN(NotesPartSSNTextBox.Text)) Then
                _PartSSN = CInt(UFCWGeneral.UnFormatSSN(NotesPartSSNTextBox.Text))
                LoadFreeText()
            End If
        End If
    End Sub

    Private Sub NotesRelationIDTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NotesRelationIDTextBox.TextChanged
        If CStr(NotesRelationIDTextBox.Text).Length > 0 Then
            If RelationID <> If(NotesRelationIDTextBox.Text.Trim.Length > 0, CType(NotesRelationIDTextBox.Text.Trim, Short), Nothing) Then
                _RelationID = CType(NotesRelationIDTextBox.Text, Short?)
                LoadFreeText()
            End If
        End If
    End Sub

    Private Sub NotesPatSSNTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NotesPatSSNTextBox.TextChanged
        If CStr(NotesPatSSNTextBox.Text).Length > 0 Then
            If _PatSSN <> CInt(UFCWGeneral.UnFormatSSN(NotesPatSSNTextBox.Text)) Then
                _PatSSN = CInt(UFCWGeneral.UnFormatSSN(NotesPatSSNTextBox.Text))
                LoadFreeText()
            End If
        End If
    End Sub
#End Region

#Region "Public Subs\Functions"

    Public Sub ClearFreeText()

        NotesFamilyIDTextBox.DataBindings.Clear()
        NotesPartSSNTextBox.DataBindings.Clear()
        NotesRelationIDTextBox.DataBindings.Clear()
        NotesPatSSNTextBox.DataBindings.Clear()

        _FamilyID = Nothing
        _RelationID = Nothing
        _PartSSN = Nothing
        _PatSSN = Nothing
        _PartFName = Nothing
        _PartLName = Nothing
        _PatFName = Nothing
        _PatLName = Nothing

        _BlinkStartTime = Nothing

    End Sub

    Public Sub LoadFreeText(ByVal fREE_TEXTDT As DataTable, ByVal rEG_ALERTINFODT As DataTable)

        Dim REG_ALERTS As DataTable = rEG_ALERTINFODT.Clone
        REG_ALERTS.TableName = "REG_ALERTS"

        FreeTextDataSet = New DataSet

        FreeTextDataSet.Tables.Add(New DataTable("FREE_TEXT"))
        FreeTextDataSet.Tables.Add(REG_ALERTS)

        FreeTextDataSet.EnforceConstraints = False
        FreeTextDataSet.Tables("FREE_TEXT").Load(fREE_TEXTDT.CreateDataReader)
        FreeTextDataSet.Tables("REG_ALERTS").Load(rEG_ALERTINFODT.CreateDataReader)

        Call DisplayFreeText()

    End Sub

    Public Sub LoadFreeText()

        Try

            If FreeTextDataSet IsNot Nothing Then
                FreeTextDataSet.Tables("FREE_TEXT").Rows.Clear()
                FreeTextDataSet.Tables("REG_ALERTS").Rows.Clear()
            Else
                FreeTextDataSet = New DataSet
                FreeTextDataSet.Tables.Add(New DataTable("FREE_TEXT"))
                FreeTextDataSet.Tables.Add(New DataTable("REG_ALERTS"))
            End If

            FreeTextDataSet = CMSDALFDBMD.RetrieveFreeTextAndAlerts(_FamilyID, FreeTextDataSet)

            DisplayFreeText()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub DisplayFreeText()
        Try

            _FamilyFreeTextDV = New DataView(FreeTextDataSet.Tables("FREE_TEXT"), "RELATION_ID = -1", "RELATION_ID", DataViewRowState.CurrentRows)
            If _AppName.Length > 0 AndAlso _AppName.ToUpper = "ELIGIBILITY" Then
                _FamilyFreeTextDV = New DataView(FreeTextDataSet.Tables("REG_ALERTS"), "RELATION_ID = -1", "RELATION_ID", DataViewRowState.CurrentRows)
            End If

            FamilyFreeTextDataGrid.DataSource = _FamilyFreeTextDV
            FamilyFreeTextDataGrid.SetTableStyle()
            FamilyFreeTextDataGrid.Sort = If(FamilyFreeTextDataGrid.LastSortedBy, FamilyFreeTextDataGrid.DefaultSort)

            PartAlert = False

            If _RelationID IsNot Nothing Then
                _PatientFreeTextDV = New DataView(FreeTextDataSet.Tables("FREE_TEXT"), "RELATION_ID = " & _RelationID, "RELATION_ID", DataViewRowState.CurrentRows)
                If _AppName.Length > 0 AndAlso _AppName.ToUpper = "ELIGIBILITY" Then
                    _PatientFreeTextDV = New DataView(FreeTextDataSet.Tables("REG_ALERTS"), "RELATION_ID = 0 AND PASSPHRASE<>'' ", "RELATION_ID", DataViewRowState.CurrentRows)
                End If

                PatientFreeTextDataGrid.DataSource = _PatientFreeTextDV
                PatientFreeTextDataGrid.SetTableStyle()
                PatientFreeTextDataGrid.Sort = If(PatientFreeTextDataGrid.LastSortedBy, PatientFreeTextDataGrid.DefaultSort)

            End If

            PatAlert = False

            If _ReadOnly Then
                NotesTextBox.Visible = False
                NotesCancelButton.Visible = False
                NotesSaveButton.Visible = False
                EnterNotesLabel.Visible = False
                NotesSplitContainer.Top = NotesTextBox.Top + 10
            Else
                NotesTextBox.Visible = True
                NotesCancelButton.Visible = True
                NotesSaveButton.Visible = True
                EnterNotesLabel.Visible = True
                EnterNotesLabel.Top = 54
                NotesSplitContainer.Top = 180
            End If

            PatientAlertLabel.Text = ""
            PatientHighAlertLabel.Text = ""

            If FreeTextDataSet.Tables("REG_ALERTS").Rows.Count > 0 Then

                If Not IsDBNull(FreeTextDataSet.Tables("REG_ALERTS").Rows(0)("ALERT_REASON")) AndAlso FreeTextDataSet.Tables("REG_ALERTS").Rows(0)("ALERT_REASON").ToString.Trim.Length > 0 Then

                    _PartAlert = True

                    Dim SelectedDRs As DataRow() = FreeTextDataSet.Tables("REG_ALERTS").Select("RELATION_ID = " & If(_RelationID Is Nothing, 0S, _RelationID).ToString)
                    If SelectedDRs.Length > 0 Then _PatAlert = True

                    Dim QueryParticipantAlerts =
                        From Alerts In FreeTextDataSet.Tables("REG_ALERTS").AsEnumerable()
                        Where (CInt(Alerts("FAMILY_ALERT")) = 1 OrElse CShort(Alerts("RELATION_ID")) = If(_RelationID Is Nothing, 0S, _RelationID))

                    For Each DR In QueryParticipantAlerts
                        Select Case DR("HIGHLIGHTED_ALERT").ToString
                            Case "0"
                                Me.PatientAlertLabel.Text &= If(Me.PatientAlertLabel.Text.Trim.Length > 0, " \ ", "") & DR("ALERT_REASON_DESC").ToString & If(DR("PASSPHRASE").ToString.Trim.Length > 0, " : ( " & DR("PASSPHRASE").ToString.Trim & " )", "")
                            Case Else 'highlight requested
                                Me.PatientHighAlertLabel.Text &= If(Me.PatientHighAlertLabel.Text.Trim.Length > 0, " \ ", "") & DR("ALERT_REASON_DESC").ToString & If(DR("PASSPHRASE").ToString.Trim.Length > 0, " : ( " & DR("PASSPHRASE").ToString.Trim & " )", "")
                        End Select
                    Next

                    If Me.PatientHighAlertLabel.Text.Trim.Length > 0 AndAlso Me.PatientAlertLabel.Text.Trim.Length > 0 Then
                        Me.PatientAlertLabel.Text = " \ " & Me.PatientAlertLabel.Text
                    End If

                End If
            End If

            Dim DGTS As DataGridTableStyle = Me.FamilyFreeTextDataGrid.GetCurrentTableStyle
            If DGTS IsNot Nothing Then
                FamilyFreeTextDataGrid.SuspendLayout()
                FamilyFreeTextDataGrid.AutoSizeRowHeight(1, DGTS.GridColumnStyles(1).Width)
                FamilyFreeTextDataGrid.ResumeLayout()
            End If

            DGTS = Me.PatientFreeTextDataGrid.GetCurrentTableStyle
            If DGTS IsNot Nothing Then
                PatientFreeTextDataGrid.SuspendLayout()
                PatientFreeTextDataGrid.AutoSizeRowHeight(1, DGTS.GridColumnStyles(1).Width)
                PatientFreeTextDataGrid.ResumeLayout()
            End If

            If _RelationID Is Nothing Then 'Hide Patient elements as the search was at the family level
                PatientNotesRadioButton.Visible = False
                FamilyNotesRadioButton.Visible = False
                PatientGroupBox.Visible = False
                NotesSplitContainer.Panel2Collapsed = True
            Else
                PatientNotesRadioButton.Visible = True
                FamilyNotesRadioButton.Visible = True
                PatientGroupBox.Visible = True
                NotesSplitContainer.Panel2Collapsed = False

                If NotesSplitContainer.Height / 2 > NotesSplitContainer.Panel1MinSize Then
                    NotesSplitContainer.SplitterDistance = CInt(NotesSplitContainer.Height / 2)
                End If

            End If

        Catch ex As Exception
            Throw
        Finally
            BlinkTimer.Enabled = True
        End Try
    End Sub

#End Region

#Region "Custom Subs\Functions"
    Private Sub LookupInfo()

        Dim DR As DataRow

        Try

            DR = CMSDALFDBMD.RetrieveRegMaster(CInt(NotesPartSSNTextBox.Text), CInt(NotesPatSSNTextBox.Text))

            If DR IsNot Nothing Then
                _FamilyID = CInt(DR("FAMILY_ID"))
                _RelationID = UFCWGeneral.IsNullShortHandler(DR("RELATION_ID"))
                _PartSSN = CInt(NotesPartSSNTextBox.Text)
                _PatSSN = CInt(NotesPatSSNTextBox.Text)
                _PatFName = UFCWGeneral.IsNullStringHandler(DR("FIRST_NAME"))
                _PatLName = UFCWGeneral.IsNullStringHandler(DR("LAST_NAME"))

                If UFCWGeneral.IsNullShortHandler(DR("RELATION_ID")) = 0 Then
                    _PartFName = UFCWGeneral.IsNullStringHandler(DR("FIRST_NAME"))
                    _PartLName = UFCWGeneral.IsNullStringHandler(DR("LAST_NAME"))
                Else
                    DR = CMSDALFDBMD.RetrieveRegMaster(CInt(NotesPartSSNTextBox.Text), CInt(NotesPartSSNTextBox.Text))

                    _PartFName = UFCWGeneral.IsNullStringHandler(DR("FIRST_NAME"))
                    _PartLName = UFCWGeneral.IsNullStringHandler(DR("LAST_NAME"))
                End If

            End If

        Catch ex As Exception
            Throw
        Finally
            DR = Nothing
        End Try
    End Sub

    Private Sub SaveFreeText()
        Dim RelationID As Short

        Try
            If FamilyNotesRadioButton.Checked Then
                RelationID = -1
            Else
                RelationID = CShort(_RelationID)
            End If


            CMSDALFDBMD.CreateFreeText(_FamilyID, CShort(RelationID), _PartSSN, _PatSSN,
                                  _PartFName, _PartLName, _PatFName, _PatLName,
                                  NotesTextBox.Text, _DomainUser.ToUpper)

            LoadFreeText()

            NotesTextBox.Text = ""

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadFreeText(ByVal familyID As Integer, ByVal relationID As Short?, ByVal partSSN As Integer, ByVal patSSN As Integer, ByVal partFName As String, ByVal partLName As String, ByVal patFName As String, ByVal patLName As String)
        Try
            _FamilyID = familyID
            _RelationID = relationID
            _PartSSN = partSSN
            _PatSSN = patSSN

            _PartFName = partFName
            _PartLName = partLName
            _PatFName = patFName
            _PatLName = patLName

            LoadFreeText()

        Catch ex As Exception
            Throw
        End Try
    End Sub


    Private Sub BlinkTimer_Tick(sender As System.Object, e As System.EventArgs) Handles BlinkTimer.Tick

        If IsNothing(_BlinkStartTime) Then _BlinkStartTime = UFCWGeneral.NowDate

        If (UFCWGeneral.NowDate.AddSeconds(-3) > _BlinkStartTime) Then
            BlinkTimer.Enabled = False
            _BlinkStartTime = Nothing
            PatientHighAlertLabel.Enabled = True
        Else
            PatientHighAlertLabel.Enabled = Not PatientHighAlertLabel.Enabled
        End If

    End Sub

#End Region

End Class

