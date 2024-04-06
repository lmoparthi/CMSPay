Option Strict On

Imports System.ComponentModel
Imports System.Windows.Forms
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling


Public Class REGMasterHistoryViewerForm
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
    Shared _SharedHistoryDT As DataTable

    Private _FamilyID As Integer
    Private _RelationID As Integer
    Private WithEvents _HistoryBS As New BindingSource

    Private _ALLHistoryDT As DataTable
    Private _DubClick As Boolean = False
    Private _APPKEY As String = "UFCW\RegMaster\"
    Private _DisplayColumnNames As DataView

    Private _ModeFilter As String = ""

    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents txtHistoryDetail As System.Windows.Forms.TextBox

    Private _Disposed As Boolean = False

    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then

            ClearDataBindings(Me)

            ' Free any other managed objects here.
            '
            _DisplayColumnNames = Nothing
            If _DisplayColumnNames IsNot Nothing Then
                _DisplayColumnNames.Dispose()
            End If
            _DisplayColumnNames = Nothing

            If _HistoryBS IsNot Nothing Then
                _HistoryBS.DataMember = ""
                _HistoryBS.DataSource = Nothing
                _HistoryBS.Dispose()
            End If
            _HistoryBS = Nothing

            If REGMHistoryDataGrid IsNot Nothing Then
                If REGMHistoryDataGrid.DataSource IsNot Nothing Then REGMHistoryDataGrid.DataSource = Nothing
                REGMHistoryDataGrid.Dispose()
            End If
            REGMHistoryDataGrid = Nothing

            If components IsNot Nothing Then
                components.Dispose()
            End If

        End If

        ' Free any unmanaged objects here.
        '
        _Disposed = True

        ' Call base class implementation.
        MyBase.Dispose(disposing)
    End Sub

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
    Friend WithEvents REGMHistoryDataGrid As DataGridCustom
    Friend WithEvents FamilyCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents PatientCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents HistoryImageList As System.Windows.Forms.ImageList
    Friend WithEvents HistoryToolTip As System.Windows.Forms.ToolTip
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.REGMHistoryDataGrid = New DataGridCustom()
        Me.FamilyCheckBox = New System.Windows.Forms.CheckBox()
        Me.PatientCheckBox = New System.Windows.Forms.CheckBox()
        Me.HistoryImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.HistoryToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.txtHistoryDetail = New System.Windows.Forms.TextBox()
        CType(Me.REGMHistoryDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'REGMHistoryDataGrid
        '
        Me.REGMHistoryDataGrid.ADGroupsThatCanCopy = ""
        Me.REGMHistoryDataGrid.ADGroupsThatCanCustomize = ""
        Me.REGMHistoryDataGrid.ADGroupsThatCanExport = ""
        Me.REGMHistoryDataGrid.ADGroupsThatCanFilter = ""
        Me.REGMHistoryDataGrid.ADGroupsThatCanFind = ""
        Me.REGMHistoryDataGrid.ADGroupsThatCanMultiSort = ""
        Me.REGMHistoryDataGrid.ADGroupsThatCanPrint = ""
        Me.REGMHistoryDataGrid.AllowAutoSize = True
        Me.REGMHistoryDataGrid.AllowColumnReorder = True
        Me.REGMHistoryDataGrid.AllowCopy = True
        Me.REGMHistoryDataGrid.AllowCustomize = True
        Me.REGMHistoryDataGrid.AllowDelete = False
        Me.REGMHistoryDataGrid.AllowDragDrop = False
        Me.REGMHistoryDataGrid.AllowEdit = False
        Me.REGMHistoryDataGrid.AllowExport = True
        Me.REGMHistoryDataGrid.AllowFilter = True
        Me.REGMHistoryDataGrid.AllowFind = True
        Me.REGMHistoryDataGrid.AllowGoTo = True
        Me.REGMHistoryDataGrid.AllowMultiSelect = True
        Me.REGMHistoryDataGrid.AllowMultiSort = False
        Me.REGMHistoryDataGrid.AllowNew = False
        Me.REGMHistoryDataGrid.AllowPrint = False
        Me.REGMHistoryDataGrid.AllowRefresh = True
        Me.REGMHistoryDataGrid.AppKey = "UFCW\RegMaster\"
        Me.REGMHistoryDataGrid.AutoSaveCols = True
        Me.REGMHistoryDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.REGMHistoryDataGrid.CaptionText = "0 Entries"
        Me.REGMHistoryDataGrid.ColumnHeaderLabel = Nothing
        Me.REGMHistoryDataGrid.ColumnRePositioning = False
        Me.REGMHistoryDataGrid.ColumnResizing = False
        Me.REGMHistoryDataGrid.ConfirmDelete = True
        Me.REGMHistoryDataGrid.CopySelectedOnly = True
        Me.REGMHistoryDataGrid.DataMember = ""
        Me.REGMHistoryDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.REGMHistoryDataGrid.DragColumn = 0
        Me.REGMHistoryDataGrid.ExportSelectedOnly = True
        Me.REGMHistoryDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.REGMHistoryDataGrid.HighlightedRow = Nothing
        Me.REGMHistoryDataGrid.HighLightModifiedRows = False
        Me.REGMHistoryDataGrid.IsMouseDown = False
        Me.REGMHistoryDataGrid.LastGoToLine = ""
        Me.REGMHistoryDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.REGMHistoryDataGrid.MultiSort = False
        Me.REGMHistoryDataGrid.Name = "REGMHistoryDataGrid"
        Me.REGMHistoryDataGrid.OldSelectedRow = Nothing
        Me.REGMHistoryDataGrid.ReadOnly = True
        Me.REGMHistoryDataGrid.RetainRowSelectionAfterSort = True
        Me.REGMHistoryDataGrid.SetRowOnRightClick = True
        Me.REGMHistoryDataGrid.ShiftPressed = False
        Me.REGMHistoryDataGrid.SingleClickBooleanColumns = True
        Me.REGMHistoryDataGrid.Size = New System.Drawing.Size(319, 264)
        Me.REGMHistoryDataGrid.Sort = Nothing
        Me.REGMHistoryDataGrid.StyleName = ""
        Me.REGMHistoryDataGrid.SubKey = ""
        Me.REGMHistoryDataGrid.SuppressTriangle = False
        Me.REGMHistoryDataGrid.TabIndex = 0
        '
        'FamilyCheckBox
        '
        Me.FamilyCheckBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.FamilyCheckBox.Location = New System.Drawing.Point(8, 216)
        Me.FamilyCheckBox.Name = "FamilyCheckBox"
        Me.FamilyCheckBox.Size = New System.Drawing.Size(184, 24)
        Me.FamilyCheckBox.TabIndex = 1
        Me.FamilyCheckBox.Text = "Show Entire History For Family"
        Me.FamilyCheckBox.Visible = False
        '
        'PatientCheckBox
        '
        Me.PatientCheckBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PatientCheckBox.Location = New System.Drawing.Point(8, 240)
        Me.PatientCheckBox.Name = "PatientCheckBox"
        Me.PatientCheckBox.Size = New System.Drawing.Size(184, 24)
        Me.PatientCheckBox.TabIndex = 2
        Me.PatientCheckBox.Text = "Show Entire History For Patient"
        Me.PatientCheckBox.Visible = False
        '
        'HistoryImageList
        '
        Me.HistoryImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.HistoryImageList.ImageSize = New System.Drawing.Size(16, 16)
        Me.HistoryImageList.TransparentColor = System.Drawing.Color.Transparent
        '
        'HistoryToolTip
        '
        Me.HistoryToolTip.AutoPopDelay = 5000
        Me.HistoryToolTip.InitialDelay = 500
        Me.HistoryToolTip.ReshowDelay = 10000
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BackColor = System.Drawing.SystemColors.Window
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.REGMHistoryDataGrid)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtHistoryDetail)
        Me.SplitContainer1.Size = New System.Drawing.Size(429, 264)
        Me.SplitContainer1.SplitterDistance = 319
        Me.SplitContainer1.TabIndex = 3
        '
        'TxtHistoryDetail
        '
        Me.txtHistoryDetail.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.txtHistoryDetail.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtHistoryDetail.Location = New System.Drawing.Point(0, 0)
        Me.txtHistoryDetail.Multiline = True
        Me.txtHistoryDetail.Name = "TxtHistoryDetail"
        Me.txtHistoryDetail.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtHistoryDetail.Size = New System.Drawing.Size(106, 264)
        Me.txtHistoryDetail.TabIndex = 0
        '
        'HistoryViewer
        '
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.PatientCheckBox)
        Me.Controls.Add(Me.FamilyCheckBox)
        Me.Name = "HistoryViewer"
        Me.Size = New System.Drawing.Size(429, 264)
        CType(Me.REGMHistoryDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Properties"
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Applies Filter to History Data.")>
    Public Property Mode() As REGMasterHistoryMode
        Get
            Return _Mode
        End Get
        Set(ByVal Value As REGMasterHistoryMode)
            _Mode = Value
            Select Case _Mode
                Case REGMasterHistoryMode.All
                    _ModeFilter = "ELIG%"
                Case REGMasterHistoryMode.Address
                    _ModeFilter = "%ADDRESS%"
                Case REGMasterHistoryMode.HMONetwork
                    _ModeFilter = "ELIGCOVHMO%"
                Case REGMasterHistoryMode.MedicalCoverage
                    _ModeFilter = "ELIGCOV%"
                Case REGMasterHistoryMode.DentalCoverage
                    _ModeFilter = "ELIGCOVDEN%"
                Case REGMasterHistoryMode.Risk
                    _ModeFilter = "RISK%"
                Case REGMasterHistoryMode.Memtype
                    _ModeFilter = "MEMTYPE%"
                Case REGMasterHistoryMode.SpecialHours
                    _ModeFilter = "SPECIAL%"
                Case REGMasterHistoryMode.LifeEvents
                    _ModeFilter = "LIFEEVENT%"
                Case REGMasterHistoryMode.Phone
                    _ModeFilter = "%PHONE%"
                Case REGMasterHistoryMode.EMail
                    _ModeFilter = "%EMAIL%"
                Case REGMasterHistoryMode.ERE
                    _ModeFilter = "%ERE%"
                Case REGMasterHistoryMode.TERM
                    _ModeFilter = "TERM%"
            End Select
        End Set
    End Property

    Private Property _Mode As REGMasterHistoryMode = REGMasterHistoryMode.All

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

    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
        End Set
    End Property

    Shared Property RMHistory() As DataTable
        Get
            Return _SharedHistoryDT
        End Get
        Set(ByVal value As DataTable)
            _SharedHistoryDT = value
        End Set
    End Property
#End Region

#Region "Constructor"

    Public Sub New(ByVal familyID As Integer, ByVal relationID As Integer)
        Me.New()

        _FamilyID = familyID
        _RelationID = relationID

    End Sub
#End Region

#Region "Form\Button Events"

    Public Sub LoadHistory(ByVal familyID As Integer, ByVal relationID As Integer)
        Try
            _FamilyID = familyID
            _RelationID = relationID

            RefreshHistory()

            FilterAndDisplayHistory(_ALLHistoryDT)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub LoadHistory(ByVal familyID As Integer, ByVal relationID As Integer, mode As REGMasterHistoryMode, ByVal dt As DataTable)

        Me.Mode = mode 'this will populate the filter 

        _FamilyID = familyID
        _RelationID = relationID

        FilterAndDisplayHistory(dt)

    End Sub

    Public Sub LoadHistory(ByVal familyID As Integer, ByVal relationID As Integer, ByVal dt As DataTable)
        'use provided history set

        Try
            _FamilyID = familyID
            _RelationID = relationID

            _ALLHistoryDT = Nothing

            RefreshHistory(dt)

            FilterAndDisplayHistory(dt)

        Catch ex As Exception
            Throw
        Finally

        End Try
    End Sub

    Private Sub FilterAndDisplayHistory(ByVal dt As DataTable)

        Dim SelectFilter As String = ""
        Dim DRs As DataRow()
        Dim HistoryDT As DataTable

        Try
            If dt Is Nothing Then Return

            _HistoryBS = New BindingSource

            If _ModeFilter.Trim.Length > 0 Then
                SelectFilter = "TRANSACTION_TYPE Like '" & _ModeFilter & "'"
            End If

            If _RelationID > -1 Then
                SelectFilter &= If(SelectFilter.Length > 0, " AND ", "") & "RELATION_ID=" & _RelationID.ToString
            End If

            If FamilyCheckBox IsNot Nothing AndAlso FamilyCheckBox.Checked Then

                DRs = dt.Select()

            ElseIf PatientCheckBox IsNot Nothing AndAlso PatientCheckBox.Checked Then

                DRs = dt.Select("RELATION_ID=" & _RelationID)

            ElseIf SelectFilter.Length > 0 Then

                DRs = dt.Select(SelectFilter)

            End If

            HistoryDT = dt.Clone

            For Each DR As DataRow In DRs
                HistoryDT.ImportRow(DR)
            Next

            _HistoryBS.DataSource = HistoryDT

            txtHistoryDetail.DataBindings.Clear()
            Dim Bind As Binding = New Binding("Text", _HistoryBS, "DETAIL", False, DataSourceUpdateMode.OnPropertyChanged)
            txtHistoryDetail.DataBindings.Add(Bind)

            REGMHistoryDataGrid.DataSource = _HistoryBS
            REGMHistoryDataGrid.SetTableStyle()
            REGMHistoryDataGrid.Sort = If(REGMHistoryDataGrid.LastSortedBy, REGMHistoryDataGrid.DefaultSort)
            REGMHistoryDataGrid.CaptionText = HistoryDT.Rows.Count & If(HistoryDT.Rows.Count = 1, " Entry", " Entries")

            If _HistoryBS.Count > 0 Then _HistoryBS.Position = 0

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub ClearDataBindings(parentCtrl As Control)

        For Each C As Control In parentCtrl.Controls

            If C.Controls.Count > 0 Then ClearDataBindings(C) 'recursive for grouping controls

            C.DataBindings.Clear()

            If TypeOf (C) Is RadioButton OrElse TypeOf (C) Is TextBox OrElse TypeOf (C) Is ComboBox OrElse TypeOf (C) Is DateTimePicker OrElse TypeOf (C) Is CheckBox OrElse TypeOf (C) Is DataGrid Then
                If TypeOf (C) Is DataGrid OrElse TypeOf (C) Is DataGridCustom Then
                    CType(C, DataGridCustom).CaptionText = ""
                    CType(C, DataGridCustom).DataMember = ""
                    CType(C, DataGridCustom).DataSource = Nothing
                ElseIf TypeOf (C) Is CheckBox Then
                    CType(C, CheckBox).CheckState = CheckState.Unchecked
                ElseIf TypeOf (C) Is ComboBox Then
                    CType(C, ComboBox).SelectedIndex = -1
                Else
                    C.ResetText()
                End If

            End If

        Next

    End Sub
    Private Sub RefreshButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            _ALLHistoryDT = Nothing

            RefreshHistory()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub HistoryDataGrid_RefreshGridData() Handles REGMHistoryDataGrid.RefreshGridData
        Try

            _ALLHistoryDT = Nothing

            RefreshHistory()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub HistoryDataGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles REGMHistoryDataGrid.DoubleClick
        _DubClick = True
    End Sub

#End Region

#Region "Custom Subs\Functions"
    Public Sub FilterHistory(ByVal familyID As Integer, ByVal relationID As Integer)

        _FamilyID = familyID
        _RelationID = relationID

        RefreshHistory()

    End Sub

    Public Sub RefreshHistory(Optional dt As DataTable = Nothing)

        Try

            If _ALLHistoryDT Is Nothing OrElse _ALLHistoryDT.Rows.Count < 1 Then
                _ALLHistoryDT = If(dt Is Nothing, RegMasterDAL.RetrieveRegHistoryByFamily(_FamilyID), dt)
            End If

            If _SharedHistoryDT IsNot Nothing Then
                _SharedHistoryDT.Rows.Clear()
            End If

            _SharedHistoryDT = Nothing
            _SharedHistoryDT = _ALLHistoryDT.Clone

            For Each DR As DataRow In _ALLHistoryDT.Rows
                _SharedHistoryDT.ImportRow(DR)
            Next

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub HistoryDataGrid_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles REGMHistoryDataGrid.MouseUp
        Dim DG As DataGrid
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo

        Try
            DG = CType(sender, DataGrid)
            HTI = DG.HitTest(e.X, e.Y)

            Select Case HTI.Type
                Case Is = System.Windows.Forms.DataGrid.HitTestType.None
                    Me.REGMHistoryDataGrid.AllowRefresh = True
                Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell
                    REGMHistoryDataGrid.CurrentRowIndex = HTI.Row
                Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader

                Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader
                    REGMHistoryDataGrid.CurrentRowIndex = HTI.Row
                    Dim DV As DataView = REGMHistoryDataGrid.GetDefaultDataView
                    'If e.Button = MouseButtons.Left Then
                    '    TxtHistoryDetail.Text = CStr(DV(REGMHistoryDataGrid.CurrentRowIndex)("DETAIL"))
                    'End If
                    If e.Button = MouseButtons.Left AndAlso _DubClick = True Then
                        Dim DetailForm As New RegMasterDetailViewerForm(CInt(DV(REGMHistoryDataGrid.CurrentRowIndex)("FAMILY_ID")), CStr(DV(REGMHistoryDataGrid.CurrentRowIndex)("SUMMARY")), CStr(DV(REGMHistoryDataGrid.CurrentRowIndex)("DETAIL")), CStr(DV(REGMHistoryDataGrid.CurrentRowIndex)("CREATE_USERID")), CDate(DV(REGMHistoryDataGrid.CurrentRowIndex)("CREATE_DATE")))

                        DetailForm.ShowDialog(Me)
                        DetailForm.Dispose()
                        DetailForm = Nothing
                    End If
                Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize

                Case Is = System.Windows.Forms.DataGrid.HitTestType.RowResize

                Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption
                    REGMHistoryDataGrid.CurrentRowIndex = 0
                Case Is = System.Windows.Forms.DataGrid.HitTestType.ParentRows

            End Select

        Catch ex As Exception
            Throw
        Finally
            _DubClick = False
        End Try
    End Sub

    Public Sub ClearAll()
        If REGMHistoryDataGrid IsNot Nothing Then
            REGMHistoryDataGrid.DataMember = ""
            REGMHistoryDataGrid.DataSource = Nothing
            REGMHistoryDataGrid.CaptionText = ""
        End If

        If _ALLHistoryDT IsNot Nothing Then _ALLHistoryDT = Nothing
        If _SharedHistoryDT IsNot Nothing Then _SharedHistoryDT = Nothing
        If _DisplayColumnNames IsNot Nothing Then _DisplayColumnNames = Nothing
    End Sub

#End Region

End Class
Public Enum REGMasterHistoryMode
    All
    Risk
    Memtype
    HMONetwork
    MedicalCoverage
    DentalCoverage
    SpecialHours
    Address
    LifeEvents
    Phone
    EMail
    ERE
    TERM
End Enum
