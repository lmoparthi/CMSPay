Option Infer On
Option Strict On

Imports System.ComponentModel
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Text


Public Class EligibilityControl
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _FamilyID As Integer? = Nothing
    Private _RelationID As Short? = Nothing
    Private _DocType As String = ""

    Private _APPKEY As String = "UFCW\Claims\"
    Private _Loading As Boolean = True

    Private _ColumnsDT As DataTable
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents EPToDateTimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents epFromDateTimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents EPCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents EligContextMenuStrip As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ModifyMemtypeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

    Public Event BeforeRefresh(ByVal sender As Object, ByRef Cancel As Boolean)
    Public Event AfterRefresh(ByVal sender As Object)

    Private _ReadOnlyMode As Boolean = True
    Private _SelectedEligPeriod As Date  ''  the selected row eligperiod
    Private _PatientEligiblityDS As EligibilityDataSet

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents EligibilityDataGrid As DataGridCustom
    Friend WithEvents _EligibilityDS As DataSet 'UI.EligibilityDataSet
    Friend WithEvents EligImageList As System.Windows.Forms.ImageList
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EligibilityControl))
        Me.EligibilityDataGrid = New DataGridCustom()
        Me._EligibilityDS = New DataSet 'UFCW.CMS.UI.EligibilityDataSet()
        Me.EligImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.EPToDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.epFromDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.EPCheckBox = New System.Windows.Forms.CheckBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.EligContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ModifyMemtypeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.EligibilityDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._EligibilityDS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.EligContextMenuStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'EligibilityDataGrid
        '
        Me.EligibilityDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.EligibilityDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.EligibilityDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.EligibilityDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.EligibilityDataGrid.ADGroupsThatCanFind = ""
        Me.EligibilityDataGrid.ADGroupsThatCanMultiSort = ""
        Me.EligibilityDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.EligibilityDataGrid.AllowAutoSize = True
        Me.EligibilityDataGrid.AllowColumnReorder = True
        Me.EligibilityDataGrid.AllowCopy = True
        Me.EligibilityDataGrid.AllowCustomize = True
        Me.EligibilityDataGrid.AllowDelete = False
        Me.EligibilityDataGrid.AllowDragDrop = False
        Me.EligibilityDataGrid.AllowEdit = False
        Me.EligibilityDataGrid.AllowExport = True
        Me.EligibilityDataGrid.AllowFilter = False
        Me.EligibilityDataGrid.AllowFind = True
        Me.EligibilityDataGrid.AllowGoTo = True
        Me.EligibilityDataGrid.AllowMultiSelect = False
        Me.EligibilityDataGrid.AllowMultiSort = True
        Me.EligibilityDataGrid.AllowNew = False
        Me.EligibilityDataGrid.AllowPrint = True
        Me.EligibilityDataGrid.AllowRefresh = True
        Me.EligibilityDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.EligibilityDataGrid.AppKey = "UFCW\Claims\"
        Me.EligibilityDataGrid.AutoSaveCols = True
        Me.EligibilityDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.EligibilityDataGrid.CaptionText = "Eligibility"
        Me.EligibilityDataGrid.ColumnHeaderLabel = Nothing
        Me.EligibilityDataGrid.ColumnRePositioning = False
        Me.EligibilityDataGrid.ColumnResizing = False
        Me.EligibilityDataGrid.ConfirmDelete = True
        Me.EligibilityDataGrid.CopySelectedOnly = True
        Me.EligibilityDataGrid.DataMember = ""
        Me.EligibilityDataGrid.DragColumn = 0
        Me.EligibilityDataGrid.ExportSelectedOnly = True
        Me.EligibilityDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.EligibilityDataGrid.HighlightedRow = Nothing
        Me.EligibilityDataGrid.HighLightModifiedRows = False
        Me.EligibilityDataGrid.IsMouseDown = False
        Me.EligibilityDataGrid.LastGoToLine = ""
        Me.EligibilityDataGrid.Location = New System.Drawing.Point(0, 40)
        Me.EligibilityDataGrid.MultiSort = False
        Me.EligibilityDataGrid.Name = "EligibilityDataGrid"
        Me.EligibilityDataGrid.OldSelectedRow = 0
        Me.EligibilityDataGrid.ReadOnly = True
        Me.EligibilityDataGrid.RetainRowSelectionAfterSort = True
        Me.EligibilityDataGrid.SetRowOnRightClick = True
        Me.EligibilityDataGrid.ShiftPressed = False
        Me.EligibilityDataGrid.SingleClickBooleanColumns = True
        Me.EligibilityDataGrid.Size = New System.Drawing.Size(514, 376)
        Me.EligibilityDataGrid.Sort = Nothing
        Me.EligibilityDataGrid.StyleName = ""
        Me.EligibilityDataGrid.SubKey = ""
        Me.EligibilityDataGrid.SuppressTriangle = False
        Me.EligibilityDataGrid.TabIndex = 6
        '
        'EligibilityDataSet
        '
        Me._EligibilityDS.DataSetName = "EligibilityDataSet"
        Me._EligibilityDS.Locale = New System.Globalization.CultureInfo("en-US")
        Me._EligibilityDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'EligImageList
        '
        Me.EligImageList.ImageStream = CType(resources.GetObject("EligImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.EligImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.EligImageList.Images.SetKeyName(0, "")
        Me.EligImageList.Images.SetKeyName(1, "")
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.EPToDateTimePicker)
        Me.GroupBox1.Controls.Add(Me.epFromDateTimePicker)
        Me.GroupBox1.Controls.Add(Me.EPCheckBox)
        Me.GroupBox1.Controls.Add(Me.btnSearch)
        Me.GroupBox1.Location = New System.Drawing.Point(0, -5)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(514, 41)
        Me.GroupBox1.TabIndex = 10
        Me.GroupBox1.TabStop = False
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(256, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(32, 18)
        Me.Label1.TabIndex = 50
        Me.Label1.Text = "And"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(105, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(49, 18)
        Me.Label2.TabIndex = 49
        Me.Label2.Text = "Between"
        '
        'EPToDateTimePicker
        '
        Me.EPToDateTimePicker.Enabled = False
        Me.EPToDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.EPToDateTimePicker.Location = New System.Drawing.Point(289, 12)
        Me.EPToDateTimePicker.Name = "EPToDateTimePicker"
        Me.EPToDateTimePicker.Size = New System.Drawing.Size(95, 20)
        Me.EPToDateTimePicker.TabIndex = 43
        '
        'epFromDateTimePicker
        '
        Me.epFromDateTimePicker.Enabled = False
        Me.epFromDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.epFromDateTimePicker.Location = New System.Drawing.Point(160, 12)
        Me.epFromDateTimePicker.Name = "epFromDateTimePicker"
        Me.epFromDateTimePicker.Size = New System.Drawing.Size(95, 20)
        Me.epFromDateTimePicker.TabIndex = 42
        '
        'EPCheckBox
        '
        Me.EPCheckBox.Location = New System.Drawing.Point(5, 12)
        Me.EPCheckBox.Name = "EPCheckBox"
        Me.EPCheckBox.Size = New System.Drawing.Size(104, 20)
        Me.EPCheckBox.TabIndex = 41
        Me.EPCheckBox.Text = "Eligibility Period"
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(414, 11)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(75, 21)
        Me.btnSearch.TabIndex = 4
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'EligContextMenuStrip
        '
        Me.EligContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ModifyMemtypeToolStripMenuItem})
        Me.EligContextMenuStrip.Name = "EligContextMenuStrip"
        Me.EligContextMenuStrip.Size = New System.Drawing.Size(167, 26)
        '
        'ModifyMemtypeToolStripMenuItem
        '
        Me.ModifyMemtypeToolStripMenuItem.Name = "ModifyMemtypeToolStripMenuItem"
        Me.ModifyMemtypeToolStripMenuItem.Size = New System.Drawing.Size(166, 22)
        Me.ModifyMemtypeToolStripMenuItem.Text = "Modify Memtype"
        '
        'EligibilityControl
        '
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.EligibilityDataGrid)
        Me.Name = "EligibilityControl"
        Me.Size = New System.Drawing.Size(514, 416)
        CType(Me.EligibilityDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._EligibilityDS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.EligContextMenuStrip.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Properties"
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the FamilyID of the Document.")>
    Public Property FamilyID() As Integer?
        Get
            Return _FamilyID
        End Get
        Set(ByVal value As Integer?)
            _FamilyID = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the RelationID of the Document.")>
    Public Property RelationID() As Short?
        Get
            Return _RelationID
        End Get
        Set(ByVal value As Short?)
            _RelationID = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Doc Type of the Current Claim")>
    Public Property DocType() As String
        Get
            Return _DocType
        End Get
        Set(ByVal value As String)
            _DocType = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets the Eligibility Table")>
    Public ReadOnly Property EligibilityDataTable() As DataTable
        Get
            Return _EligibilityDS.Tables("REG_MASTER")
        End Get
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

    <System.ComponentModel.Browsable(True), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Determines if control is in Read Only Mode.")>
    Public Property ReadOnlyMode() As Boolean
        Get
            Return _ReadOnlyMode
        End Get
        Set(ByVal Value As Boolean)
            _ReadOnlyMode = Value
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Indicates if the patient is eligible as of the time tested.")>
    Public ReadOnly Property IsEligible(Optional relationID As Integer = 0) As Boolean
        Get

            Dim Response As Boolean = False

            If _EligibilityDS IsNot Nothing AndAlso _EligibilityDS.Tables("REG_MASTER") IsNot Nothing Then

                Dim EligibilityQuery = From IsPatientEligible In _EligibilityDS.Tables("ISELIGIBLE").AsEnumerable
                                       Where IsPatientEligible.Field(Of Integer)("RELATION_ID") = relationID

                If EligibilityQuery.Any Then
                    Response = True
                End If

            End If

            Return Response

        End Get

    End Property

#End Region

    Private _Disposed As Boolean = False

    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.
            If _PatientEligiblityDS IsNot Nothing Then
                _PatientEligiblityDS.Dispose()
            End If
            _PatientEligiblityDS = Nothing

            If _ColumnsDT IsNot Nothing Then
                _ColumnsDT.Dispose()
            End If
            _ColumnsDT = Nothing

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

#Region "Constructor"
    Public Sub New(ByVal familyID As Integer, ByVal relationID As Short)
        Me.New()

        _FamilyID = familyID
        _RelationID = relationID
    End Sub

    Public Sub New(ByVal familyID As Integer, ByVal relationID As Short, ByVal docType As String)
        Me.New()

        _FamilyID = familyID
        _RelationID = relationID
        _DocType = docType
    End Sub

    Public Sub New(ByVal appKey As String, ByVal familyID As Integer, ByVal relationID As Short)
        Me.New()

        _APPKEY = appKey
        _FamilyID = familyID
        _RelationID = relationID
    End Sub

    Public Sub New(ByVal appKey As String, ByVal familyID As Integer, ByVal relationID As Short, ByVal docType As String)
        Me.New()

        _APPKEY = appKey
        _FamilyID = familyID
        _RelationID = relationID
        _DocType = docType
    End Sub
#End Region

#Region "Form\Button Events"

    Private Sub EligibilityControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        _Loading = False
    End Sub

    Private Sub EligibilityDataGrid_RefreshGridData() Handles EligibilityDataGrid.RefreshGridData
        Dim Cancel As Boolean = False

        Try

            RaiseEvent BeforeRefresh(Me, Cancel)

            If Cancel = False Then
                LoadEligibility()
            End If

            RaiseEvent AfterRefresh(Me)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub EligibilityControl_dispose(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        Try

            If EligibilityDataGrid IsNot Nothing Then
                EligibilityDataGrid.Dispose()
            End If
            EligibilityDataGrid = Nothing

            Me.Dispose()

        Catch ex As Exception
            Throw
        End Try

    End Sub

    'Private Sub SetTableStyle(ByVal dg As DataGridCustom) 'called when no context menu is in use
    '    Try

    '        _BindingManagerBase = Me.BindingContext(dg.DataSource)

    '        RemoveHandler _BindingManagerBase.CurrentChanged, AddressOf BindingManagerBase_CurrentChanged

    '        SetTableStyle(dg, EligContextMenuStrip)

    '        AddHandler _BindingManagerBase.CurrentChanged, AddressOf BindingManagerBase_CurrentChanged

    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub


    Private Sub BindingManagerBase_CurrentChanged(sender As Object, e As EventArgs)

        Dim CM As CurrencyManager
        Dim DR As DataRow

        Try

            CM = CType(sender, CurrencyManager)

            If CM Is Nothing OrElse CM.Position < 0 OrElse CM.Current Is Nothing Then Return

            DR = CType(CM.Current, DataRowView).Row

            If Not IsDBNull(DR("ELIG_PERIOD")) Then
                _SelectedEligPeriod = CDate(DR("ELIG_PERIOD"))
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub SetTableStyle(ByVal dg As DataGridCustom, ByVal dataGridCustomContextMenu As System.Windows.Forms.ContextMenuStrip)

        Try

            SetTableStyleColumns(dg)
            dg.StyleName = dg.Name
            dg.AppKey = _APPKEY 'Provides for Dual Coverage support

            If Not UFCWGeneralAD.REGMVendorAccess Then
                dg.ContextMenuPrepare(dataGridCustomContextMenu)
            End If

            RemoveHandler dg.ResetTableStyle, AddressOf TableStyleReset
            AddHandler dg.ResetTableStyle, AddressOf TableStyleReset

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub
    Private Sub TableStyleReset(ByVal sender As Object, e As EventArgs)
        Dim dg As DataGridCustom = CType(sender, DataGridCustom)
        SetTableStyleColumns(dg)
    End Sub

    Private Sub SetTableStyleColumns(dg As DataGridCustom)

        Dim DGTS As DataGridTableStyle

        Dim TextCol As DataGridFormattableTextBoxColumn
        Dim BoolCol As DataGridColorBoolColumn
        Dim IconCol As DataGridHighlightIconColumn

        Dim ColsDV As DataView
        Dim DefaultStyleDS As DataSet
        Dim XMLStyleName As String
        Dim ResultDT As DataTable
        Dim ColumnSequenceDV As DataView
        Dim DGTSDefault As DataGridTableStyle

        Try


            XMLStyleName = dg.Name

            DefaultStyleDS = DataGridCustom.GetTableStyle(XMLStyleName) ''DefaultStyleDS = dg.GetTableStyle(XMLStyleName)

            If DefaultStyleDS Is Nothing OrElse DefaultStyleDS.Tables.Count < 1 Then Return

            DGTS = New DataGridTableStyle()

            If dg.GetCurrentDataTable Is Nothing Then
                DGTS.MappingName = dg.Name
            Else
                DGTS.MappingName = dg.GetCurrentDataTable.TableName
            End If

            DGTS.GridColumnStyles.Clear()

            If DefaultStyleDS.Tables.Contains(XMLStyleName & "Style") Then
                If DefaultStyleDS.Tables(XMLStyleName & "Style").Columns.Contains("GridLineStyle") Then
                    DGTS.GridLineStyle = If(CBool(DefaultStyleDS.Tables(XMLStyleName & "Style").Rows(0)("GridLineStyle")), DataGridLineStyle.Solid, DataGridLineStyle.None)
                End If
                If DefaultStyleDS.Tables(XMLStyleName & "Style").Columns.Contains("RowHeadersVisible") Then
                    DGTS.RowHeadersVisible = CBool(DefaultStyleDS.Tables(XMLStyleName & "Style").Rows(0)("RowHeadersVisible"))
                End If
            End If

            ResultDT = dg.LoadColumnsSizeAndPosition(dg.Name & "\" & DGTS.MappingName & "\ColumnSettings", DefaultStyleDS.Tables("Column"))

            ColumnSequenceDV = New DataView(ResultDT)
            ColsDV = ColumnSequenceDV

            DGTSDefault = New DataGridTableStyle() 'This can be used to establish the columns displayed by default
            DGTSDefault.MappingName = "Default"

            For intCol = 0 To ColsDV.Count - 1

                If (IsDBNull(ColsDV(intCol).Item("Visible")) OrElse ColsDV(intCol).Item("Visible").ToString.Trim.Length = 0 OrElse CBool(ColsDV(intCol).Item("Visible")) = True) Then
                    TextCol = New DataGridFormattableTextBoxColumn
                    TextCol.MappingName = CStr(ColsDV(intCol).Item("Mapping"))
                    TextCol.HeaderText = CStr(ColsDV(intCol).Item("HeaderText"))
                    If IsDBNull(ColsDV(intCol).Item("Format")) = False AndAlso ColsDV(intCol).Item("Format").ToString.Trim.Length > 0 Then
                        TextCol.Format = CStr(ColsDV(intCol).Item("Format"))
                    End If
                    DGTSDefault.GridColumnStyles.Add(TextCol)
                End If

                If ((IsDBNull(ColsDV(intCol).Item("Visible")) OrElse ColsDV(intCol).Item("Visible").ToString.Trim.Length = 0 OrElse CBool(ColsDV(intCol).Item("Visible")) = True) AndAlso (GetAllSettings(_APPKEY, dg.Name & "\" & DGTS.MappingName & "\Customize\ColumnSelector") Is Nothing OrElse CDbl(GetSetting(_APPKEY, dg.Name & "\" & DGTS.MappingName & "\Customize\ColumnSelector", "Col " & ColsDV(intCol).Item("Mapping").ToString & " Customize", "0")) = 1)) Then
                    If ColsDV(intCol).Item("Type").ToString.Trim = "Text" Then
                        TextCol = New DataGridFormattableTextBoxColumn
                        TextCol.MappingName = CStr(ColsDV(intCol).Item("Mapping"))
                        TextCol.HeaderText = CStr(ColsDV(intCol).Item("HeaderText"))
                        TextCol.Width = CInt(ColsDV(intCol).Item("SizeInPixels"))
                        TextCol.NullText = CStr(ColsDV(intCol).Item("NullText"))
                        TextCol.TextBox.WordWrap = True

                        Try

                            If CBool(ColsDV(intCol).Item("ReadOnly")) Then
                                TextCol.ReadOnly = True
                            End If

                        Catch ex As Exception
                        End Try

                        If IsDBNull(ColsDV(intCol).Item("Format")) = False Then
                            If ColsDV(intCol).Item("Format").ToString.Trim = "YesNo" Then
                                AddHandler TextCol.Formatting, AddressOf DataGridCustom.FormattingYesNo
                            ElseIf ColsDV(intCol).Item("Format").ToString.Trim = "Elig" Then
                                AddHandler TextCol.Formatting, AddressOf FormattingEligibility
                            ElseIf ColsDV(intCol).Item("Format").ToString.Trim = "DentalElig" Then
                                AddHandler TextCol.Formatting, AddressOf FormattingDentalEligibility
                            ElseIf ColsDV(intCol).Item("Format").ToString.Trim.Length > 0 Then
                                TextCol.Format = CStr(ColsDV(intCol).Item("Format"))
                            End If
                        End If

                        DGTS.GridColumnStyles.Add(TextCol)

                    ElseIf ColsDV(intCol).Item("Type").ToString.Contains("Bool") Then
                        BoolCol = New DataGridColorBoolColumn(intCol)
                        BoolCol.MappingName = CStr(ColsDV(intCol).Item("Mapping"))
                        BoolCol.HeaderText = CStr(ColsDV(intCol).Item("HeaderText"))
                        BoolCol.Width = CInt(ColsDV(intCol).Item("SizeInPixels"))
                        BoolCol.NullValue = If(IsNumeric(ColsDV(intCol).Item("NullText").ToString.Trim), CDec(ColsDV(intCol).Item("NullText")), 0)
                        BoolCol.TrueValue = CType("1", Decimal)
                        BoolCol.FalseValue = CType("0", Decimal)
                        BoolCol.AllowNull = False

                        Try

                            If CBool(ColsDV(intCol).Item("ReadOnly")) Then
                                BoolCol.ReadOnly = True
                            End If

                        Catch ex As Exception
                        End Try

                        DGTS.GridColumnStyles.Add(BoolCol)

                    ElseIf ColsDV(intCol).Item("Type").ToString.Trim = "Icon" Then

                        IconCol = New DataGridHighlightIconColumn(EligibilityDataGrid, EligImageList)

                        IconCol.MappingName = CStr(ColsDV(intCol).Item("Mapping"))
                        IconCol.HeaderText = CStr(ColsDV(intCol).Item("HeaderText"))
                        IconCol.Width = CInt(ColsDV(intCol).Item("SizeInPixels"))
                        IconCol.NullText = CStr(ColsDV(intCol).Item("NullText"))
                        IconCol.MaximumCharWidth = CInt(ColsDV(intCol).Item("MaximumCharWidth"))
                        IconCol.MinimumCharWidth = CInt(ColsDV(intCol).Item("MinimumCharWidth"))

                        AddHandler IconCol.PaintCellPicture, AddressOf DetermineCellIcon

                        DGTS.GridColumnStyles.Add(IconCol)
                    End If
                End If
            Next

            dg.TableStyles.Clear()
            dg.TableStyles.Add(DGTS)
            dg.TableStyles.Add(DGTSDefault)

        Catch ex As Exception
            Throw
        Finally

            If ResultDT IsNot Nothing Then ResultDT.Dispose()
            ResultDT = Nothing

            If DGTS IsNot Nothing Then DGTS.Dispose()
            DGTS = Nothing
        End Try

    End Sub

    Private Sub DetermineCellIcon(ByRef pic As Image, ByVal cell As System.Windows.Forms.DataGridCell)

        Dim DV As DataView

        Try
            DV = EligibilityDataGrid.GetDefaultDataView
            If DV IsNot Nothing Then
                If IsDBNull(DV(cell.RowNumber)("MED_ELIG_SW")) = False AndAlso CBool(DV(cell.RowNumber)("MED_ELIG_SW")) Then
                    If IsDBNull(DV(cell.RowNumber)("STATUS")) = False AndAlso CStr(DV(cell.RowNumber)("STATUS")).ToUpper = "COBRA" Then
                        If IsDBNull(DV(cell.RowNumber)("DEN_ELIG_SW")) = False AndAlso CBool(DV(cell.RowNumber)("DEN_ELIG_SW")) = False AndAlso (_DocType.ToUpper = "VISION" OrElse _DocType.ToUpper = "VISION GVA") Then
                            'not elig
                            pic = EligImageList.Images(1)
                        Else
                            If _RelationID > 0 AndAlso CBool(DV(cell.RowNumber)("FAMILY_SW")) = False Then
                                'not elig
                                pic = EligImageList.Images(1)
                            Else
                                'eligible
                                pic = EligImageList.Images(0)
                            End If
                        End If
                    Else
                        If _RelationID > 0 AndAlso CBool(DV(cell.RowNumber)("FAMILY_SW")) = False Then
                            'not elig
                            pic = EligImageList.Images(1)
                        Else
                            'eligible
                            pic = EligImageList.Images(0)
                        End If
                    End If
                Else
                    'not elig
                    pic = EligImageList.Images(1)
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    'Private Sub FormattingFromDate(ByRef value As Object, ByVal RowNum As Integer)
    '    Try
    '        Dim DV As DataView = EligibilityDataGrid.GetDefaultDataView
    '        If DV IsNot Nothing Then
    '            If Format(DV(RowNum)("ELIG_PERIOD"), "MMyyyy") = Format(DV(RowNum)("FROM_DATE"), "MMyyyy") Then
    '                value = Format(DV(RowNum)("FROM_DATE"), "MM/dd/yyyy")
    '            Else
    '                value = Format(DV(RowNum)("ELIG_PERIOD"), "MM/01/yyyy")
    '            End If
    '        Else
    '            value = Format(value, "MM/dd/yyyy")
    '        End If
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub

    'Private Sub FormattingThruDate(ByRef value As Object, ByVal RowNum As Integer)
    '    Try
    '        Dim DV As DataView = EligibilityDataGrid.GetDefaultDataView
    '        If DV IsNot Nothing Then
    '            If Format(DV(RowNum)("ELIG_PERIOD"), "MMyyyy") = Format(DV(RowNum)("THRU_DATE"), "MMyyyy") Then
    '                value = Format(DV(RowNum)("THRU_DATE"), "MM/dd/yyyy")
    '            Else
    '                value = Format(DV(RowNum)("ELIG_PERIOD"), "MM/" & System.DateTime.DaysInMonth(CInt(Format(DV(RowNum)("ELIG_PERIOD"), "yyyy")), CInt(Format(DV(RowNum)("ELIG_PERIOD"), "MM"))) & "/yyyy")
    '            End If
    '        Else
    '            value = Format(value, "MM/dd/yyyy")
    '        End If
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub

    Private Sub FormattingEligibility(ByRef value As Object, ByVal RowNum As Integer)
        Dim Eligible As Boolean = False
        Dim Cobra As Boolean = False
        Dim CobraCore As Boolean = False
        Dim FamilyCov As Boolean = False
        Dim PremReq As Boolean = False
        Dim PremPaid As Boolean = False
        Dim PremFamilyCov As Boolean = False
        Dim CoverageGap As Boolean = False

        Try
            Dim DV As DataView = EligibilityDataGrid.GetDefaultDataView
            If DV IsNot Nothing Then
                If IsDBNull(DV(RowNum)("MED_ELIG_SW")) = False AndAlso CBool(DV(RowNum)("MED_ELIG_SW")) Then
                    Eligible = True
                Else
                    Eligible = False
                End If

                If IsDBNull(DV(RowNum)("STATUS")) = False AndAlso CStr(DV(RowNum)("STATUS")).ToUpper = "COBRA" Then
                    Cobra = True
                Else
                    Cobra = False
                End If

                If IsDBNull(DV(RowNum)("STATUS")) = False AndAlso CStr(DV(RowNum)("STATUS")).ToUpper = "NO COVERAGE" Then
                    CoverageGap = True
                Else
                    CoverageGap = False
                End If

                If Cobra = True AndAlso IsDBNull(DV(RowNum)("DEN_ELIG_SW")) = False AndAlso CBool(DV(RowNum)("DEN_ELIG_SW")) = False AndAlso (_DocType.ToUpper = "VISION" OrElse _DocType.ToUpper = "VISION GVA") Then
                    CobraCore = True
                Else
                    CobraCore = False
                End If

                If _RelationID > 0 AndAlso CBool(DV(RowNum)("FAMILY_SW")) = False Then
                    FamilyCov = False
                Else
                    FamilyCov = True
                End If

                If IsDBNull(DV(RowNum)("PREMIUM_SW")) = False AndAlso CBool(DV(RowNum)("PREMIUM_SW")) Then
                    PremReq = True
                Else
                    PremReq = False
                End If

                If PremReq = True AndAlso IsDBNull(DV(RowNum)("BAL_MED")) = False Then
                    If CStr(DV(RowNum)("BAL_MED")).ToUpper = "Y" OrElse CStr(DV(RowNum)("BAL_MED")).ToUpper = "O" Then
                        PremPaid = True
                    Else
                        PremPaid = False
                    End If
                Else
                    PremPaid = False
                End If

                If PremReq = True AndAlso PremPaid = True AndAlso IsDBNull(DV(RowNum)("PREM_TYPE")) = False Then

                    If CStr(DV(RowNum)("PREM_TYPE")).ToUpper = "EMPCHILD" Then
                        If CStr(DV(RowNum)("RELATION")).ToUpper = "H" OrElse CStr(DV(RowNum)("RELATION")).ToUpper = "W" OrElse CStr(DV(RowNum)("RELATION")).ToUpper = "P" Then
                            PremFamilyCov = False
                        Else
                            PremFamilyCov = True
                        End If
                    ElseIf CStr(DV(RowNum)("PREM_TYPE")).ToUpper = "FAMILY" Then
                        PremFamilyCov = True
                    ElseIf CStr(DV(RowNum)("PREM_TYPE")).ToUpper = "KSINGLE" AndAlso IsDBNull(DV(RowNum)("RELATION")) = False Then
                        If CStr(DV(RowNum)("RELATION")).ToUpper = "H" OrElse CStr(DV(RowNum)("RELATION")).ToUpper = "W" OrElse CStr(DV(RowNum)("RELATION")).ToUpper = "P" Then
                            PremFamilyCov = False
                        Else
                            PremFamilyCov = True
                        End If
                    ElseIf CStr(DV(RowNum)("PREM_TYPE")).ToUpper = "SINGLE" AndAlso IsDBNull(DV(RowNum)("RELATION")) = False Then
                        If CStr(DV(RowNum)("RELATION")).ToUpper = "M" Then
                            PremFamilyCov = True
                        Else
                            PremFamilyCov = False
                        End If
                    Else
                        PremFamilyCov = False
                    End If

                ElseIf PremReq = False Then
                    PremFamilyCov = True
                Else
                    PremFamilyCov = False
                End If

                If CoverageGap Then
                    value = "Not Covered"

                    Exit Try
                End If

                If Eligible = False Then
                    value = "Not Eligible"

                    Exit Try
                End If

                If Cobra = True AndAlso CobraCore = True Then
                    value = "Not Eligible For Vision"

                    Exit Try
                End If

                If FamilyCov = False Then
                    value = "Not Eligible For Family Coverage"

                    Exit Try
                End If

                If PremReq = True AndAlso PremPaid = False Then
                    value = "Not Eligible Premium Not Paid"

                    Exit Try
                End If

                If PremReq = True AndAlso PremPaid = True AndAlso PremFamilyCov = False Then
                    value = "Not Eligible For Family Coverage"

                    Exit Try
                End If

                value = "Eligible"
                If Format(DV(RowNum)("ELIG_PERIOD"), "MMyyyy") = Format(DV(RowNum)("FROM_DATE"), "MMyyyy") Then
                    value = value.ToString & " (From " & Format(DV(RowNum)("FROM_DATE"), "MM/dd/yyyy") & ")"
                End If
                If Format(DV(RowNum)("ELIG_PERIOD"), "MMyyyy") = Format(DV(RowNum)("THRU_DATE"), "MMyyyy") Then
                    value = value.ToString & " (To " & Format(DV(RowNum)("THRU_DATE"), "MM/dd/yyyy") & ")"
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    'Private Sub FormattingEligStatus(ByRef value As Object, ByVal RowNum As Integer)
    '    Try
    '        Dim DV As DataView = EligibilityDataGrid.GetDefaultDataView
    '        If DV IsNot Nothing Then
    '            If IsDBNull(DV(RowNum)("STATUS")) = False AndAlso CStr(DV(RowNum)("STATUS")).ToUpper = "COBRA" Then
    '                If IsDBNull(DV(RowNum)("DEN_ELIG_SW")) = False AndAlso CBool(DV(RowNum)("DEN_ELIG_SW")) = False Then
    '                    value = "COBRA (Core)"
    '                Else
    '                    value = "COBRA (Plus)"
    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub

    'Private Sub FormattingYesNo(ByRef value As Object, ByVal RowNum As Integer)
    '    Try
    '        If IsDBNull(value) = False AndAlso CBool(value) Then
    '            value = "Yes"
    '        Else
    '            value = "No"
    '        End If
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub

    'Private Sub FormattingPremPaid(ByRef value As Object, ByVal RowNum As Integer)
    '    Try
    '        Dim DV As DataView = EligibilityDataGrid.GetDefaultDataView
    '        If DV IsNot Nothing Then
    '            If Not IsDBNull(value) AndAlso Not IsDBNull(DV(RowNum)("PREMIUM_SW")) AndAlso CBool(DV(RowNum)("PREMIUM_SW")) Then
    '                Select Case CStr(value).ToUpper
    '                    Case Is = "Y"
    '                        value = "Yes"
    '                    Case Is = "O"
    '                        value = "Yes (override)"
    '                    Case Is = "N"
    '                        value = "No"
    '                    Case Is = "D"
    '                        value = "No (declined)"
    '                    Case Else
    '                        value = "No"
    '                End Select
    '            Else
    '                value = ""
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub

    'Private Sub FormattingType(ByRef value As Object, ByVal RowNum As Integer)
    '    Try
    '        Dim DV As DataView = EligibilityDataGrid.GetDefaultDataView
    '        Dim Type As String = ""

    '        value = ""
    '        If DV IsNot Nothing Then
    '            If IsDBNull(DV(RowNum)("SURVIVING_SPOUSE_SW")) = False AndAlso CBool(DV(RowNum)("SURVIVING_SPOUSE_SW")) = True Then
    '                value = value.ToString & If(value.ToString.Trim.Length = 0, "", ", ") & "Surviving Spouse"
    '            End If
    '            If IsDBNull(DV(RowNum)("DISABLE_SW")) = False AndAlso CBool(DV(RowNum)("DISABLE_SW")) = True Then
    '                value = value.ToString & If(value.ToString.Trim.Length = 0, "", ", ") & "Disabled"
    '            End If
    '            If IsDBNull(DV(RowNum)("STEP_SW")) = False AndAlso CBool(DV(RowNum)("STEP_SW")) = True Then
    '                value = value.ToString & If(value.ToString.Trim.Length = 0, "", ", ") & "Step Child"
    '            End If
    '            If IsDBNull(DV(RowNum)("FOSTER_SW")) = False AndAlso CBool(DV(RowNum)("FOSTER_SW")) = True Then
    '                value = value.ToString & If(value.ToString.Trim.Length = 0, "", ", ") & "Foster Child"
    '            End If
    '            If IsDBNull(DV(RowNum)("STUDENT_SW")) = False AndAlso CBool(DV(RowNum)("STUDENT_SW")) = True Then
    '                value = value.ToString & If(value.ToString.Trim.Length = 0, "", ", ") & "Student"
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub

    Private Sub FormattingDentalEligibility(ByRef value As Object, ByVal RowNum As Integer)

        If EligibilityDataGrid.GetCurrentDataTable.Columns("BAL_DENT") Is Nothing Then Return

        Dim Eligible As Boolean = False
        Dim Cobra As Boolean = False
        Dim CobraCore As Boolean = False
        Dim FamilyCov As Boolean = False
        Dim PremReq As Boolean = False
        Dim PremPaid As Boolean = False
        Dim PremFamilyCov As Boolean = False
        Dim CoverageGap As Boolean = False
        Dim Retiree As Boolean = False

        Try
            Dim DV As DataView = EligibilityDataGrid.GetDefaultDataView
            If DV IsNot Nothing Then
                If IsDBNull(DV(RowNum)("DEN_ELIG_SW")) = False AndAlso CBool(DV(RowNum)("DEN_ELIG_SW")) Then
                    Eligible = True
                Else
                    Eligible = False
                End If

                ''If IsDBNull(dv(RowNum)("STATUS")) = False AndAlso CStr(dv(RowNum)("STATUS")).ToUpper = "COBRA" Then
                ''    Cobra = True
                ''Else
                ''    Cobra = False
                ''End If

                If IsDBNull(DV(RowNum)("STATUS")) = False AndAlso CStr(DV(RowNum)("STATUS")).ToUpper = "RETIREE" Then
                    Retiree = True
                Else
                    Retiree = False
                End If

                If IsDBNull(DV(RowNum)("STATUS")) = False AndAlso CStr(DV(RowNum)("STATUS")).ToUpper = "NO COVERAGE" Then
                    CoverageGap = True
                Else
                    CoverageGap = False
                End If

                ''If Cobra = True AndAlso IsDBNull(dv(RowNum)("DEN_ELIG_SW")) = False AndAlso CBool(dv(RowNum)("DEN_ELIG_SW")) = False AndAlso (_DocType.ToUpper = "VISION" Or _DocType.ToUpper = "VISION GVA") Then
                ''    CobraCore = True
                ''Else
                ''    CobraCore = False
                ''End If

                If _RelationID > 0 AndAlso CBool(DV(RowNum)("FAMILY_SW")) = False Then
                    FamilyCov = False
                Else
                    FamilyCov = True
                End If

                If IsDBNull(DV(RowNum)("PREMIUM_SW")) = False AndAlso CBool(DV(RowNum)("PREMIUM_SW")) Then
                    PremReq = True
                Else
                    PremReq = False
                End If

                If PremReq = True AndAlso IsDBNull(DV(RowNum)("BAL_DENT")) = False Then
                    If CStr(DV(RowNum)("BAL_DENT")).ToUpper = "Y" OrElse CStr(DV(RowNum)("BAL_DENT")).ToUpper = "O" Then
                        PremPaid = True
                    Else
                        PremPaid = False
                    End If
                Else
                    PremPaid = False
                End If

                If Retiree = True Then  '' for retirees
                    If PremReq = True AndAlso PremPaid = True AndAlso IsDBNull(DV(RowNum)("PREM_TYPE")) = False Then
                        If CStr(DV(RowNum)("PREM_TYPE")).ToUpper = "FAMILY" OrElse CStr(DV(RowNum)("PREM_TYPE")).ToUpper = "KSINGLE" OrElse CStr(DV(RowNum)("PREM_TYPE")).ToUpper = "SINGLE" Then
                            PremFamilyCov = True
                        Else
                            PremFamilyCov = False
                        End If
                    Else
                        PremFamilyCov = False
                    End If
                Else            '' for actives

                    If PremReq = True AndAlso PremPaid = True AndAlso IsDBNull(DV(RowNum)("PREM_TYPE")) = False Then
                        If CStr(DV(RowNum)("PREM_TYPE")).ToUpper = "EMPCHILD" Then
                            If CStr(DV(RowNum)("RELATION")).ToUpper = "H" OrElse CStr(DV(RowNum)("RELATION")).ToUpper = "W" OrElse CStr(DV(RowNum)("RELATION")).ToUpper = "P" Then
                                PremFamilyCov = False
                            Else
                                PremFamilyCov = True
                            End If
                        ElseIf CStr(DV(RowNum)("PREM_TYPE")).ToUpper = "FAMILY" Then
                            PremFamilyCov = True
                        ElseIf CStr(DV(RowNum)("PREM_TYPE")).ToUpper = "KSINGLE" AndAlso IsDBNull(DV(RowNum)("RELATION")) = False Then
                            If CStr(DV(RowNum)("RELATION")).ToUpper = "H" OrElse CStr(DV(RowNum)("RELATION")).ToUpper = "W" OrElse CStr(DV(RowNum)("RELATION")).ToUpper = "P" Then
                                PremFamilyCov = False
                            Else
                                PremFamilyCov = True
                            End If
                        ElseIf CStr(DV(RowNum)("PREM_TYPE")).ToUpper = "SINGLE" AndAlso IsDBNull(DV(RowNum)("RELATION")) = False Then
                            If CStr(DV(RowNum)("RELATION")).ToUpper = "M" Then
                                PremFamilyCov = True
                            Else
                                PremFamilyCov = False
                            End If
                        Else
                            PremFamilyCov = False
                        End If

                    ElseIf PremReq = False Then
                        PremFamilyCov = True
                    Else
                        PremFamilyCov = False
                    End If

                End If

                If CoverageGap Then
                    value = "Not Covered"

                    Exit Try
                End If

                If Eligible = False Then
                    value = "Not Eligible"

                    Exit Try
                End If

                ''If Cobra = True AndAlso CobraCore = True Then
                ''    value = "Not Eligible For Vision"

                ''    Exit Try
                ''End If

                If FamilyCov = False Then
                    value = "Not Eligible For Family Coverage"

                    Exit Try
                End If

                If PremReq = True AndAlso PremPaid = False Then
                    value = "Not Eligible Premium Not Paid"

                    Exit Try
                End If

                If PremReq = True AndAlso PremPaid = True AndAlso PremFamilyCov = False Then
                    value = "Not Eligible For Family Coverage"

                    Exit Try
                End If

                value = "Eligible"
                If Format(DV(RowNum)("ELIG_PERIOD"), "MMyyyy") = Format(DV(RowNum)("FROM_DATE"), "MMyyyy") Then
                    value = value.ToString & " (From " & Format(DV(RowNum)("FROM_DATE"), "MM/dd/yyyy") & ")"
                End If
                If Format(DV(RowNum)("ELIG_PERIOD"), "MMyyyy") = Format(DV(RowNum)("THRU_DATE"), "MMyyyy") Then
                    value = value.ToString & " (To " & Format(DV(RowNum)("THRU_DATE"), "MM/dd/yyyy") & ")"
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub EPCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles EPCheckBox.CheckedChanged
        EPToDateTimePicker.Enabled = EPCheckBox.Checked
        epFromDateTimePicker.Enabled = EPCheckBox.Checked
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        If EligibilityDataGrid.DataSource Is Nothing Then Return

        Dim DV As DataView
        Dim DT As DataTable = New DataTable
        Dim DRV As DataRowView
        Dim Searchfilter As New StringBuilder()

        Try
            If _EligibilityDS.Tables(0) IsNot Nothing AndAlso _EligibilityDS.Tables(0).Rows.Count > 0 Then
                DT = _EligibilityDS.Tables(0).Clone()

                If EPCheckBox.Checked = True Then
                    Searchfilter.Append(" ELIG_PERIOD  >= " & "'" & Format(Me.epFromDateTimePicker.Value, "yyyy-MM-dd") & "' AND ELIG_PERIOD <= '" & Format(EPToDateTimePicker.Value, "yyyy-MM-dd") & "'")
                End If

                DV = New DataView(_EligibilityDS.Tables(0), Searchfilter.ToString, "", DataViewRowState.CurrentRows)
                If DV.Count > 0 Then
                    For Each DRV In DV
                        DT.LoadDataRow(DRV.Row.ItemArray, True)
                    Next
                    EligibilityDataGrid.DataSource = DT
                End If

                If EligibilityDataGrid.GetCurrentDataTable IsNot Nothing Then
                    EligibilityDataGrid.Sort = If(EligibilityDataGrid.LastSortedBy, EligibilityDataGrid.DefaultSort)
                End If

                EligibilityDataGrid.CaptionText = EligibilityDataGrid.GetGridRowCount & " of " & _EligibilityDS.Tables(0).Rows.Count & " matches "
            End If
        Catch ex As Exception
            Throw
        Finally

            If DT IsNot Nothing Then DT.Dispose()
            DT = Nothing

            If DV IsNot Nothing Then DV.Dispose()
            DV = Nothing

            Searchfilter = Nothing

        End Try
    End Sub

    Private Sub ModifyMemtypeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ModifyMemtypeToolStripMenuItem.Click

        If _RelationID <> 0 Then Return
        If _EligibilityDS Is Nothing OrElse _EligibilityDS.Tables.Count = 0 Then Return

        Dim ContextMenu As ContextMenuStrip
        Dim DG As DataGridCustom

        Dim BM As BindingManagerBase
        Dim DR As DataRow

        Try
            ContextMenu = CType(CType(sender, ToolStripMenuItem).Owner, ContextMenuStrip)
            If ContextMenu.SourceControl IsNot Nothing Then

                DG = CType(ContextMenu.SourceControl, DataGridCustom)

                If DG Is Nothing OrElse DG.DataSource Is Nothing Then Exit Sub

                BM = DG.BindingContext(DG.DataSource, DG.DataMember)
                DR = DG.SelectedRowPreview 'used to accomodate context menu click of row that is yet to be selected

                If DR IsNot Nothing Then
                    If IsDBNull(DR("FAMILY_ID")) Then Return

                    Using FRM As New UpdateMemtypeForm(CInt(_FamilyID), _ReadOnlyMode, UFCWGeneral.IsNullDateHandler(DR("ELIG_PERIOD")))
                        FRM.LoadEligibility()

                        If FRM.ShowDialog() = System.Windows.Forms.DialogResult.Yes Then
                            LoadEligibility()
                        End If

                    End Using

                End If

            End If

        Catch ex As Exception

            Throw

        Finally
        End Try

    End Sub

#End Region

#Region "Public Subs\Functions"

    Public Sub LoadEligibility()
        Try

            ClearAll()

            _EligibilityDS = CMSDALFDBMD.GetEligibilityInformation(CInt(_FamilyID), CShort(If(_RelationID Is Nothing, 0S, _RelationID)), _EligibilityDS)

            EligibilityDataGrid.CaptionText = "Eligibility for Family (" & _FamilyID.ToString & ")" & If(_RelationID Is Nothing, "", " Relation (" & _RelationID.ToString & ")")

            EligibilityDataGrid.DataSource = CType(_EligibilityDS.Tables("REG_MASTER"), DataTable)
            SetTableStyle(EligibilityDataGrid, EligContextMenuStrip)
            EligibilityDataGrid.Sort = If(EligibilityDataGrid.LastSortedBy, EligibilityDataGrid.DefaultSort)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub LoadEligibility(ByVal dt As DataTable, Optional ByVal docType As String = Nothing)
        Try

            _FamilyID = CInt(dt.Rows(0)("FAMILY_ID"))
            _RelationID = UFCWGeneral.IsNullShortHandler(dt.Rows(0)("RELATION_ID"))

            If docType IsNot Nothing Then _DocType = docType

            EligibilityDataGrid.DataSource = Nothing

            _EligibilityDS = New DataSet

            _EligibilityDS.EnforceConstraints = False

            _EligibilityDS.Merge(dt)

            _EligibilityDS.Tables(0).TableName = "REG_MASTER"

            EligibilityDataGrid.DataSource = _EligibilityDS.Tables("REG_MASTER")

            SetTableStyle(EligibilityDataGrid, EligContextMenuStrip)

            EligibilityDataGrid.Sort = If(EligibilityDataGrid.LastSortedBy, EligibilityDataGrid.DefaultSort)

            EligibilityDataGrid.CaptionText = "Eligibility for Family (" & _FamilyID.ToString & ") Relation (" & _RelationID.ToString & ")"

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub LoadEligibility(ByVal familyID As Integer?, Optional ByVal relationID As Short? = Nothing)
        Try
            Using WC As New GlobalCursor

                _FamilyID = familyID
                _RelationID = relationID

                If _RelationID > 0 Then
                    DependentEligibility(CInt(familyID), relationID)
                Else
                    LoadEligibility()
                End If

            End Using

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub LoadEligibility(ByVal familyID As Integer, ByVal relationID As Short?, ByVal docType As String)
        Try
            _FamilyID = familyID
            _RelationID = relationID
            _DocType = docType

            LoadEligibility()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub ClearAll()

        EligibilityDataGrid.DataSource = Nothing
        _EligibilityDS.Tables.Clear()
        EligibilityDataGrid.CaptionText = ""
        EPCheckBox.Checked = False
        EPCheckBox.Enabled = True

    End Sub

    Public Sub DependentEligibility(ByVal familyID As Integer, ByVal relationID As Short?)

        If relationID = 0 Then Return

        Dim REG_LIFE_EVENTS_GAPSDT As DataTable
        Dim FromDT As Date
        Dim DT As DataTable

        Try

            REG_LIFE_EVENTS_GAPSDT = New DataTable
            _PatientEligiblityDS = New EligibilityDataSet

            _FamilyID = familyID
            _RelationID = relationID

            REG_LIFE_EVENTS_GAPSDT.TableName = "REG_LIFE_EVENTS_GAPS"

            CMSDALFDBMD.GetEligibilityInformation(familyID, CShort(relationID), _PatientEligiblityDS)

            If _PatientEligiblityDS.REG_MASTER.Rows.Count > 0 Then
                FromDT = CDate(_PatientEligiblityDS.REG_MASTER.Rows(0)("FROM_DATE"))
                DT = CMSDALFDBEL.RetrievePatientsMonthsCoverableGapInfo(familyID, CShort(relationID), FromDT, Now.Date, Nothing)

                If DT IsNot Nothing Then
                    REG_LIFE_EVENTS_GAPSDT = DT.Clone
                    REG_LIFE_EVENTS_GAPSDT.BeginLoadData()
                    For Each DR As DataRow In DT.Rows
                        DR.EndEdit()
                        REG_LIFE_EVENTS_GAPSDT.ImportRow(DR)
                    Next
                    REG_LIFE_EVENTS_GAPSDT.EndLoadData()
                End If

                If Not _PatientEligiblityDS.Tables.Contains("REG_LIFE_EVENTS_GAPS") Then _PatientEligiblityDS.Tables.Add(REG_LIFE_EVENTS_GAPSDT)

                If REG_LIFE_EVENTS_GAPSDT.Rows.Count > 0 Then

                    Dim GapMonths = (REG_LIFE_EVENTS_GAPSDT).AsEnumerable
                    Dim QueryGapPeriods = From Period In GapMonths
                                          Group Period By Period!GAPMONTH, Period!GAPYEAR
                                         Into Periods = Group

                    For Each Period In QueryGapPeriods
                        Dim DR = _PatientEligiblityDS.REG_MASTER.NewREG_MASTERRow
                        DR.ItemArray = _PatientEligiblityDS.REG_MASTER.Rows(0).ItemArray
                        DR.RELATION_ID = CDec(relationID)
                        DR.ELIG_PERIOD = CDate(Period.GAPMONTH.ToString & "/1/" & Period.GAPYEAR.ToString)
                        DR.MED_ELIG_SW = False
                        DR.DEN_ELIG_SW = False
                        DR.STUDENT_SW = False
                        DR.DISABLE_SW = False
                        DR.MEDICAL_PLAN = 0
                        DR.PLAN_DESCRIPTION = ""
                        DR.PLAN_TYPE = ""
                        DR.STATUS = "No Coverage"
                        DR.DESCRIPTION = ""
                        DR.DENTAL_PLAN = 0
                        DR.DESCRIPTION = ""
                        DR.A2COUNT = 0
                        DR.MEMTYPE = ""
                        DR.MEMDESC = ""
                        DR.PLANTYPE = ""
                        DR.PREM_TYPE = ""

                        Try
                            _PatientEligiblityDS.REG_MASTER.AddREG_MASTERRow(DR)
                        Catch ex As Exception
                            Dim PartialDR As DataRow() = _PatientEligiblityDS.REG_MASTER.Select("ELIG_PERIOD = '" & Period.GAPMONTH.ToString & "/1/" & Period.GAPYEAR.ToString & "'")
                            PartialDR(0)("STATUS") = PartialDR(0)("STATUS").ToString & " - Partial Coverage"
                        End Try

                    Next

                End If

                LoadEligibility(_PatientEligiblityDS.REG_MASTER, Nothing)

            ElseIf _PatientEligiblityDS.REG_MASTER.Rows.Count = 0 Then
                EligibilityDataGrid.CaptionText = "Eligibility for Family (" & _FamilyID.ToString & ") Relation (" & _RelationID.ToString & ")"
                EligibilityDataGrid.DataSource = Nothing
            End If

        Catch ex As Exception
            Throw
        Finally

            If REG_LIFE_EVENTS_GAPSDT IsNot Nothing Then REG_LIFE_EVENTS_GAPSDT.Dispose()
            REG_LIFE_EVENTS_GAPSDT = Nothing

        End Try
    End Sub

    Private Sub EligContextMenuStrip_Opening(sender As Object, e As CancelEventArgs) Handles EligContextMenuStrip.Opening

        Dim DGContextMenu As ContextMenuStrip

        Try

            DGContextMenu = CType(sender, ContextMenuStrip)
            DGContextMenu.Items("ModifyMemtypeToolStripMenuItem").Available = False
            DGContextMenu.Items("ModifyMemtypeToolStripMenuItem").Enabled = False

            If UFCWGeneralAD.REGMEligMaintenanceAccess AndAlso Not UFCWGeneralAD.CMSLocals Then
                DGContextMenu.Items("ModifyMemtypeToolStripMenuItem").Available = True

                If Not _ReadOnlyMode Then
                    DGContextMenu.Items("ModifyMemtypeToolStripMenuItem").Enabled = True
                End If

            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

#End Region

End Class
