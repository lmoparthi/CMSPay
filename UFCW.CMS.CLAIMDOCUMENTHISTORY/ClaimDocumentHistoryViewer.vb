Option Strict On

Imports System.ComponentModel

Imports System.Data.Common
Imports System.Collections.Specialized

Public Class ClaimDocumentHistoryViewer
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Public Event Close(ByVal sender As Object)
    '   Public Event ShowingAnnotations(ByVal sender As Object, ByVal e As ShowingAnnotationsEvent)
    Public Event AnnotationsModified(ByVal sender As Object, ByVal e As AnnotationsEvent)

    Private _ClaimID As Integer
    Private _FamilyID As Integer
    Private _RelationID As Integer
    Private _PartSSN As Integer
    Private _PatSSN As Integer
    Private _PartFName As String
    Private _PartLName As String
    Private _PatFName As String
    Private _PatLName As String
    Private _ShowClose As Boolean = True
    Private _HistoryDT As DataTable
    Private _AnnotationsDT As DataTable

    Private _DubClick As Boolean = False
    Private WithEvents BindingManager As BindingManagerBase

    Private _APPKEY As String = "UFCW\Claims\"
    Friend WithEvents HistoryViewerSplitContainer As System.Windows.Forms.SplitContainer
    Public WithEvents AnnotateButton As System.Windows.Forms.Button
    Friend WithEvents CloseButton As System.Windows.Forms.Button
    Friend WithEvents RefreshButton As System.Windows.Forms.Button
    Friend WithEvents PatientCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents FamilyCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents ClaimDocumentHistoryDataGrid As DataGridCustom
    Friend WithEvents HistoryImageList As System.Windows.Forms.ImageList
    Friend WithEvents HistoryToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents DetailTextBox As System.Windows.Forms.TextBox

    ReadOnly _DomainUser As String = SystemInformation.UserName

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
            If components IsNot Nothing Then
                components.Dispose()
            End If
            If _HistoryDT IsNot Nothing Then
                _HistoryDT.Dispose()
            End If
            If _AnnotationsDT IsNot Nothing Then
                _AnnotationsDT.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ClaimDocumentHistoryViewer))
        Me.HistoryViewerSplitContainer = New System.Windows.Forms.SplitContainer()
        Me.AnnotateButton = New System.Windows.Forms.Button()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.RefreshButton = New System.Windows.Forms.Button()
        Me.PatientCheckBox = New System.Windows.Forms.CheckBox()
        Me.FamilyCheckBox = New System.Windows.Forms.CheckBox()
        Me.ClaimDocumentHistoryDataGrid = New DataGridCustom()
        Me.DetailTextBox = New System.Windows.Forms.TextBox()
        Me.HistoryImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.HistoryToolTip = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.HistoryViewerSplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.HistoryViewerSplitContainer.Panel1.SuspendLayout()
        Me.HistoryViewerSplitContainer.Panel2.SuspendLayout()
        Me.HistoryViewerSplitContainer.SuspendLayout()
        CType(Me.ClaimDocumentHistoryDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'HistoryViewerSplitContainer
        '
        Me.HistoryViewerSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HistoryViewerSplitContainer.Location = New System.Drawing.Point(0, 0)
        Me.HistoryViewerSplitContainer.Name = "HistoryViewerSplitContainer"
        '
        'HistoryViewerSplitContainer.Panel1
        '
        Me.HistoryViewerSplitContainer.Panel1.Controls.Add(Me.AnnotateButton)
        Me.HistoryViewerSplitContainer.Panel1.Controls.Add(Me.CloseButton)
        Me.HistoryViewerSplitContainer.Panel1.Controls.Add(Me.RefreshButton)
        Me.HistoryViewerSplitContainer.Panel1.Controls.Add(Me.PatientCheckBox)
        Me.HistoryViewerSplitContainer.Panel1.Controls.Add(Me.FamilyCheckBox)
        Me.HistoryViewerSplitContainer.Panel1.Controls.Add(Me.ClaimDocumentHistoryDataGrid)
        Me.HistoryViewerSplitContainer.Panel1MinSize = 50
        '
        'HistoryViewerSplitContainer.Panel2
        '
        Me.HistoryViewerSplitContainer.Panel2.Controls.Add(Me.DetailTextBox)
        Me.HistoryViewerSplitContainer.Panel2MinSize = 50
        Me.HistoryViewerSplitContainer.Size = New System.Drawing.Size(785, 264)
        Me.HistoryViewerSplitContainer.SplitterDistance = 430
        Me.HistoryViewerSplitContainer.TabIndex = 0
        '
        'AnnotateButton
        '
        Me.AnnotateButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AnnotateButton.Location = New System.Drawing.Point(272, 236)
        Me.AnnotateButton.Name = "AnnotateButton"
        Me.AnnotateButton.Size = New System.Drawing.Size(75, 23)
        Me.AnnotateButton.TabIndex = 11
        Me.AnnotateButton.Text = "&Annotate"
        '
        'CloseButton
        '
        Me.CloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CloseButton.Location = New System.Drawing.Point(192, 236)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(75, 23)
        Me.CloseButton.TabIndex = 10
        Me.CloseButton.Text = "&Close"
        '
        'RefreshButton
        '
        Me.RefreshButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RefreshButton.Location = New System.Drawing.Point(352, 236)
        Me.RefreshButton.Name = "RefreshButton"
        Me.RefreshButton.Size = New System.Drawing.Size(75, 23)
        Me.RefreshButton.TabIndex = 9
        Me.RefreshButton.Text = "&Refresh"
        '
        'PatientCheckBox
        '
        Me.PatientCheckBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PatientCheckBox.Location = New System.Drawing.Point(8, 240)
        Me.PatientCheckBox.Name = "PatientCheckBox"
        Me.PatientCheckBox.Size = New System.Drawing.Size(184, 24)
        Me.PatientCheckBox.TabIndex = 8
        Me.PatientCheckBox.Text = "Show Entire History For Patient"
        '
        'FamilyCheckBox
        '
        Me.FamilyCheckBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.FamilyCheckBox.Location = New System.Drawing.Point(8, 216)
        Me.FamilyCheckBox.Name = "FamilyCheckBox"
        Me.FamilyCheckBox.Size = New System.Drawing.Size(184, 24)
        Me.FamilyCheckBox.TabIndex = 7
        Me.FamilyCheckBox.Text = "Show Entire History For Family"
        '
        'ClaimDocumentHistoryDataGrid
        '
        Me.ClaimDocumentHistoryDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.ClaimDocumentHistoryDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.ClaimDocumentHistoryDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ClaimDocumentHistoryDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ClaimDocumentHistoryDataGrid.ADGroupsThatCanFind = ""
        Me.ClaimDocumentHistoryDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ClaimDocumentHistoryDataGrid.ADGroupsThatCanMultiSort = ""
        Me.ClaimDocumentHistoryDataGrid.AllowAutoSize = True
        Me.ClaimDocumentHistoryDataGrid.AllowColumnReorder = False
        Me.ClaimDocumentHistoryDataGrid.AllowCopy = True
        Me.ClaimDocumentHistoryDataGrid.AllowCustomize = True
        Me.ClaimDocumentHistoryDataGrid.AllowDelete = False
        Me.ClaimDocumentHistoryDataGrid.AllowDragDrop = False
        Me.ClaimDocumentHistoryDataGrid.AllowEdit = False
        Me.ClaimDocumentHistoryDataGrid.AllowExport = True
        Me.ClaimDocumentHistoryDataGrid.AllowFilter = False
        Me.ClaimDocumentHistoryDataGrid.AllowFind = True
        Me.ClaimDocumentHistoryDataGrid.AllowGoTo = True
        Me.ClaimDocumentHistoryDataGrid.AllowMultiSelect = True
        Me.ClaimDocumentHistoryDataGrid.AllowMultiSort = True
        Me.ClaimDocumentHistoryDataGrid.AllowNew = False
        Me.ClaimDocumentHistoryDataGrid.AllowPrint = False
        Me.ClaimDocumentHistoryDataGrid.AllowRefresh = False
        Me.ClaimDocumentHistoryDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ClaimDocumentHistoryDataGrid.AppKey = "UFCW\Claims\"
        Me.ClaimDocumentHistoryDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.ClaimDocumentHistoryDataGrid.CaptionText = "0 Entries"
        Me.ClaimDocumentHistoryDataGrid.ColumnHeaderLabel = Nothing
        Me.ClaimDocumentHistoryDataGrid.ColumnRePositioning = False
        Me.ClaimDocumentHistoryDataGrid.ColumnResizing = False
        Me.ClaimDocumentHistoryDataGrid.ConfirmDelete = True
        Me.ClaimDocumentHistoryDataGrid.CopySelectedOnly = True
        Me.ClaimDocumentHistoryDataGrid.DataMember = ""
        Me.ClaimDocumentHistoryDataGrid.DragColumn = 0
        Me.ClaimDocumentHistoryDataGrid.ExportSelectedOnly = True
        Me.ClaimDocumentHistoryDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ClaimDocumentHistoryDataGrid.HighlightedRow = Nothing
        Me.ClaimDocumentHistoryDataGrid.IsMouseDown = False
        Me.ClaimDocumentHistoryDataGrid.LastGoToLine = ""
        Me.ClaimDocumentHistoryDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.ClaimDocumentHistoryDataGrid.MultiSort = False
        Me.ClaimDocumentHistoryDataGrid.Name = "ClaimDocumentHistoryDataGrid"
        Me.ClaimDocumentHistoryDataGrid.OldSelectedRow = Nothing
        Me.ClaimDocumentHistoryDataGrid.ReadOnly = True
        Me.ClaimDocumentHistoryDataGrid.SetRowOnRightClick = True
        Me.ClaimDocumentHistoryDataGrid.ShiftPressed = False
        Me.ClaimDocumentHistoryDataGrid.SingleClickBooleanColumns = True
        Me.ClaimDocumentHistoryDataGrid.Size = New System.Drawing.Size(430, 212)
        Me.ClaimDocumentHistoryDataGrid.StyleName = ""
        Me.ClaimDocumentHistoryDataGrid.SubKey = ""
        Me.ClaimDocumentHistoryDataGrid.SuppressTriangle = False
        Me.ClaimDocumentHistoryDataGrid.TabIndex = 6
        '
        'DetailTextBox
        '
        Me.DetailTextBox.BackColor = System.Drawing.Color.White
        Me.DetailTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DetailTextBox.Location = New System.Drawing.Point(0, 0)
        Me.DetailTextBox.Multiline = True
        Me.DetailTextBox.Name = "DetailTextBox"
        Me.DetailTextBox.ReadOnly = True
        Me.DetailTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.DetailTextBox.Size = New System.Drawing.Size(351, 264)
        Me.DetailTextBox.TabIndex = 24
        '
        'HistoryImageList
        '
        Me.HistoryImageList.ImageStream = CType(resources.GetObject("HistoryImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.HistoryImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.HistoryImageList.Images.SetKeyName(0, "")
        Me.HistoryImageList.Images.SetKeyName(1, "")
        Me.HistoryImageList.Images.SetKeyName(2, "")
        Me.HistoryImageList.Images.SetKeyName(3, "")
        Me.HistoryImageList.Images.SetKeyName(4, "")
        Me.HistoryImageList.Images.SetKeyName(5, "")
        Me.HistoryImageList.Images.SetKeyName(6, "")
        Me.HistoryImageList.Images.SetKeyName(7, "")
        '
        'HistoryToolTip
        '
        Me.HistoryToolTip.AutoPopDelay = 5000
        Me.HistoryToolTip.InitialDelay = 500
        Me.HistoryToolTip.ReshowDelay = 10000
        '
        'ClaimDocumentHistoryViewer
        '
        Me.Controls.Add(Me.HistoryViewerSplitContainer)
        Me.Name = "ClaimDocumentHistoryViewer"
        Me.Size = New System.Drawing.Size(785, 264)
        Me.HistoryViewerSplitContainer.Panel1.ResumeLayout(False)
        Me.HistoryViewerSplitContainer.Panel2.ResumeLayout(False)
        Me.HistoryViewerSplitContainer.Panel2.PerformLayout()
        CType(Me.HistoryViewerSplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.HistoryViewerSplitContainer.ResumeLayout(False)
        CType(Me.ClaimDocumentHistoryDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Properties"
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the ClaimID of the Document.")>
    Public Property ClaimID() As Integer
        Get
            Return _ClaimID
        End Get
        Set(ByVal value As Integer)
            _ClaimID = value
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

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Participant SSN of the Document.")>
    Public Property ParticipantSSN() As Integer
        Get
            Return _PartSSN
        End Get
        Set(ByVal value As Integer)
            _PartSSN = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Patient SSN of the Document.")>
    Public Property PatientSSN() As Integer
        Get
            Return _PatSSN
        End Get
        Set(ByVal value As Integer)
            _PatSSN = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Participant First Name of the Document.")>
    Public Property ParticipantFirst() As String
        Get
            Return _PartFName
        End Get
        Set(ByVal value As String)
            _PartFName = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Participant Last Name of the Document.")>
    Public Property ParticipantLast() As String
        Get
            Return _PartLName
        End Get
        Set(ByVal value As String)
            _PartLName = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Patient First Name of the Document.")>
    Public Property PatientFirst() As String
        Get
            Return _PatFName
        End Get
        Set(ByVal value As String)
            _PatFName = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Patient Last Name of the Document.")>
    Public Property PatientLast() As String
        Get
            Return _PatLName
        End Get
        Set(ByVal value As String)
            _PatLName = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Patient Last Name of the Document.")>
    Public Property Annotations() As DataTable
        Get
            Return _AnnotationsDT
        End Get
        Set(ByVal value As DataTable)

            Dim DV As New DataView(value, "", "", DataViewRowState.CurrentRows)

            If _AnnotationsDT Is Nothing Then _AnnotationsDT = value.Clone

            _AnnotationsDT.Rows.Clear()

            If DV.Count > 0 Then
                For Cnt As Integer = 0 To DV.Count - 1
                    _AnnotationsDT.Rows.Add(DV(Cnt).Row.ItemArray)
                Next

                _AnnotationsDT.AcceptChanges()
            End If
        End Set
    End Property

    <DefaultValue(CBool(False)), Browsable(True), System.ComponentModel.Description("Shows or Hides the Close Button.")>
    Public Property ShowClose() As Boolean
        Get
            Return _ShowClose
        End Get
        Set(ByVal value As Boolean)
            _ShowClose = value

            Me.CloseButton.Visible = value
        End Set
    End Property

    <DefaultValue(CBool(False)), Browsable(True), System.ComponentModel.Description("Represents the application key used when accessing the registry.")>
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
    Public Sub New(ByVal ClaimID As Integer, ByVal familyID As Integer, ByVal relationID As Integer, ByVal partSSN As Integer, ByVal patSSN As Integer, ByVal partFName As String, ByVal partLName As String, ByVal patFName As String, ByVal patLName As String, ByVal annotationDT As DataTable)
        Me.New()

        _ClaimID = ClaimID
        _FamilyID = familyID
        _RelationID = relationID
        _PartSSN = partSSN
        _PatSSN = patSSN
        _PartFName = partFName
        _PartLName = partLName
        _PatFName = patFName
        _PatLName = patLName

        Dim DV As New DataView(annotationDT, "", "", DataViewRowState.CurrentRows)
        If DV.Count > 0 Then
            For Cnt As Integer = 0 To DV.Count - 1
                _AnnotationsDT.Rows.Add(DV(Cnt).Row.ItemArray)
            Next

            _AnnotationsDT.AcceptChanges()
        End If
    End Sub

    Public Sub New(ByVal claimID As Integer)
        Me.New()

        _ClaimID = CInt(claimID)
        Me.FamilyCheckBox.Visible = False
        Me.PatientCheckBox.Visible = False

    End Sub

    Public Sub New(ByVal appKey As String, ByVal claimID As Integer, ByVal familyID As Integer, ByVal relationID As Integer, ByVal partSSN As Integer, ByVal patSSN As Integer, ByVal partFName As String, ByVal partLName As String, ByVal patFName As String, ByVal patLName As String, ByVal annotationDT As DataTable)
        Me.New()

        _APPKEY = appKey
        _ClaimID = claimID
        _FamilyID = familyID
        _RelationID = relationID
        _PartSSN = partSSN
        _PatSSN = patSSN
        _PartFName = partFName
        _PartLName = partLName
        _PatFName = patFName
        _PatLName = patLName

        Dim DV As New DataView(annotationDT, "", "", DataViewRowState.CurrentRows)
        If DV.Count > 0 Then
            For Cnt As Integer = 0 To DV.Count - 1
                _AnnotationsDT.Rows.Add(DV(Cnt).Row.ItemArray)
            Next

            _AnnotationsDT.AcceptChanges()
        End If
    End Sub

    Public Sub New(ByVal claimID As Integer, ByVal familyID As Integer, ByVal relationID As Integer, ByVal partSSN As Integer, ByVal patSSN As Integer, ByVal partFName As String, ByVal partLName As String, ByVal patFName As String, ByVal patLName As String)
        Me.New()

        _ClaimID = claimID
        _FamilyID = familyID
        _RelationID = relationID
        _PartSSN = partSSN
        _PatSSN = patSSN
        _PartFName = partFName
        _PartLName = partLName
        _PatFName = patFName
        _PatLName = patLName
    End Sub

    Public Sub New(ByVal appKey As String, ByVal claimID As Integer, ByVal familyID As Integer, ByVal relationID As Integer, ByVal partSSN As Integer, ByVal patSSN As Integer, ByVal partFName As String, ByVal partLName As String, ByVal patFName As String, ByVal patLName As String)
        Me.New()

        _APPKEY = appKey
        _ClaimID = claimID
        _FamilyID = familyID
        _RelationID = relationID
        _PartSSN = partSSN
        _PatSSN = patSSN
        _PartFName = partFName
        _PartLName = partLName
        _PatFName = patFName
        _PatLName = patLName
    End Sub
#End Region

#Region "Form\Button Events"
    Private Sub CloseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseButton.Click
        RaiseEvent Close(Me)
    End Sub

    Private Sub RefreshButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshButton.Click
        Try
            RefreshHistory()

        Catch ex As Exception

            Throw
        End Try
    End Sub

    Private Sub CreateAnnotations(ByVal sender As System.Object, ByVal e As AnnotationsEvent)
        Dim Transaction As DbTransaction
        Dim DR As DataRow
        Dim DT As DataTable

        Try

            DT = e.AnnotationsTable.GetChanges(DataRowState.Added)

            If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
                Transaction = CMSDALCommon.BeginTransaction

                For Each DR In DT.Rows
                    CMSDALFDBMD.CreateAnnotation(_ClaimID, _FamilyID, CShort(_RelationID), _PartSSN, _PatSSN, CStr(_PartFName), CStr(_PartLName), CStr(_PatFName), CStr(_PatLName),
                                          CStr(DR("ANNOTATION")), DR("FLAG"), _DomainUser.ToUpper, Transaction)
                Next

                CMSDALCommon.CommitTransaction(Transaction)
            End If

            _AnnotationsDT = e.AnnotationsTable

        Catch ex As Exception
            Try

                If Transaction IsNot Nothing AndAlso Transaction.Connection IsNot Nothing AndAlso Transaction.Connection.State <> ConnectionState.Closed Then
                    CMSDALCommon.RollbackTransaction(Transaction)
                End If

            Finally
            End Try

        Finally

            If Transaction IsNot Nothing Then Transaction.Dispose()

        End Try

    End Sub
    Private Sub AnnotateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AnnotateButton.Click
        Dim AnnotateForm As AnnotationDialog


        Try

            AnnotateForm = New AnnotationDialog(_ClaimID, _FamilyID, _RelationID, _PartSSN, _PatSSN, _PartFName, _PartLName, _PatFName, _PatLName, _AnnotationsDT)
            AddHandler AnnotateForm.AnnotationsModified, AddressOf CreateAnnotations

            AnnotateForm.ShowDialog(Me)

        Catch ex As Exception


            Throw
        Finally

            AnnotateForm.Dispose()
            AnnotateForm = Nothing

        End Try
    End Sub

    Private Sub ClaimDocumentHistoryDataGrid_PositionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles BindingManager.PositionChanged
        Dim DV As DataView

        Try

            DV = ClaimDocumentHistoryDataGrid.GetDefaultDataView
            DetailTextBox.Text = DV(ClaimDocumentHistoryDataGrid.CurrentRowIndex)("DETAIL").ToString

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub ClaimDocumentHistoryDataGrid_RefreshGridData() Handles ClaimDocumentHistoryDataGrid.RefreshGridData
        Try
            RefreshHistory()

        Catch ex As Exception

            Throw
        End Try
    End Sub

    Private Sub ClaimDocumentHistoryDataGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ClaimDocumentHistoryDataGrid.DoubleClick
        Select Case CType(sender, DataGridCustom).LastHitSpot.Type
            Case Is = System.Windows.Forms.DataGrid.HitTestType.None

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell
                _DubClick = True
            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader
                _DubClick = True
            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ParentRows

        End Select
    End Sub

    Private Sub ClaimDocumentHistoryDataGrid_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ClaimDocumentHistoryDataGrid.MouseUp
        Dim DG As DataGrid
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo

        Try
            DG = CType(sender, DataGrid)

            HTI = DG.HitTest(e.X, e.Y)

            Select Case HTI.Type
                Case Is = System.Windows.Forms.DataGrid.HitTestType.None

                Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell
                    ClaimDocumentHistoryDataGrid.CurrentRowIndex = HTI.Row
                Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader

                Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader
                    ClaimDocumentHistoryDataGrid.CurrentRowIndex = HTI.Row
                    If e.Button = MouseButtons.Left AndAlso _DubClick = True Then
                        Dim DV As DataView = ClaimDocumentHistoryDataGrid.GetDefaultDataView
                        Dim DetailForm As New DetailViewer(CInt(DV(ClaimDocumentHistoryDataGrid.CurrentRowIndex)("CLAIM_ID")), CStr(DV(ClaimDocumentHistoryDataGrid.CurrentRowIndex)("SUMMARY")), CStr(DV(ClaimDocumentHistoryDataGrid.CurrentRowIndex)("DETAIL")), CStr(DV(ClaimDocumentHistoryDataGrid.CurrentRowIndex)("CREATE_USERID")), CDate(DV(ClaimDocumentHistoryDataGrid.CurrentRowIndex)("CREATE_DATE")))

                        DetailForm.ShowDialog(Me)
                        DetailForm.Dispose()
                        DetailForm = Nothing
                    End If
                Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize

                Case Is = System.Windows.Forms.DataGrid.HitTestType.RowResize

                Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption
                    ClaimDocumentHistoryDataGrid.CurrentRowIndex = 0
                Case Is = System.Windows.Forms.DataGrid.HitTestType.ParentRows

            End Select

        Catch ex As Exception

            Throw
        Finally
            _DubClick = False
        End Try
    End Sub

#End Region

#Region "Custom Subs\Functions"

    Public Sub RefreshHistory(ByVal claimID As Integer, ByVal familyID As Integer, ByVal relationID As Integer)
        _ClaimID = claimID
        _FamilyID = familyID
        _RelationID = relationID

        RefreshHistory()

    End Sub

    Public Sub RefreshHistory(ByVal claimID As Integer, ByVal familyID As Integer, ByVal relationID As Integer, ByVal historyDT As DataTable)
        _ClaimID = claimID
        _FamilyID = familyID
        _RelationID = relationID

        _HistoryDT = New DataTable

        _HistoryDT.Load(historyDT.CreateDataReader)

        LoadHistoryGrid()

        HistoryViewerSplitContainer.SplitterDistance = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "HistoryViewerSplitContainerPos", CStr(HistoryViewerSplitContainer.SplitterDistance)))

    End Sub

    Public Sub RefreshHistory(ByVal appkey As String, ByVal claimID As Integer, ByVal familyID As Integer, ByVal relationID As Integer)

        _APPKEY = appkey
        _ClaimID = claimID
        _FamilyID = familyID
        _RelationID = relationID

        RefreshHistory()

    End Sub

    Public Sub RefreshHistory()
        Try
            If PatientCheckBox.Checked = True Then
                _HistoryDT = CMSDALFDBMD.RetrieveDocHistoryByPatient(_FamilyID, _RelationID)
            ElseIf FamilyCheckBox.Checked = True Then
                _HistoryDT = CMSDALFDBMD.RetrieveDocHistoryByFamily(_FamilyID)
            Else
                _HistoryDT = CMSDALFDBMD.RetrieveDocHistory(_ClaimID)

            End If

            If _FamilyID <= 0 Then
                If _HistoryDT.Rows.Count > 0 Then
                    _FamilyID = CInt(_HistoryDT.Rows(0)("FAMILY_ID"))
                    _RelationID = CInt(_HistoryDT.Rows(0)("RELATION_ID"))
                End If
            End If

            LoadHistoryGrid()

        Catch ex As Exception

            Throw
        End Try
    End Sub

    Public Sub LoadHistoryGrid()
        Dim DV As DataView

        Try

            _HistoryDT.Columns.Add("Icon")
            ClaimDocumentHistoryDataGrid.DataSource = _HistoryDT
            BindingManager = Me.BindingContext(ClaimDocumentHistoryDataGrid.DataSource)

            ClaimDocumentHistoryDataGrid.Sort = If(ClaimDocumentHistoryDataGrid.LastSortedBy, ClaimDocumentHistoryDataGrid.DefaultSort)

            DV = ClaimDocumentHistoryDataGrid.GetDefaultDataView

            If _HistoryDT.Rows.Count > 0 Then
                DetailTextBox.Text = DV(ClaimDocumentHistoryDataGrid.CurrentRowIndex)("DETAIL").ToString
            End If

            ClaimDocumentHistoryDataGrid.CaptionText = _HistoryDT.Rows.Count & If(_HistoryDT.Rows.Count = 1, " Entry", " Entries")

            SetTableStyle(ClaimDocumentHistoryDataGrid)

        Catch ex As Exception

            Throw
        End Try
    End Sub

    Private Sub SetTableStyle(ByVal dg As DataGridCustom)
        SetTableStyle(dg, Nothing)
    End Sub

    Private Sub SetTableStyle(ByVal dg As DataGridCustom, ByVal dataGridCustomContextMenu As System.Windows.Forms.ContextMenuStrip)

        Try

            SetTableStyleColumns(dg)
            dg.StyleName = dg.Name
            dg.AppKey = _APPKEY

            dg.ContextMenuPrepare(dataGridCustomContextMenu)

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

    Private Sub SetTableStyleColumns(ByVal dg As DataGridCustom)

        Dim DGTS As DataGridTableStyle

        Dim TextCol As DataGridFormattableTextBoxColumn
        Dim BoolCol As DataGridColorBoolColumn
        Dim IconCol As DataGridHighlightIconColumn

        Dim IntCol As Integer
        Dim ColsDV As DataView
        Dim DefaultStyleDS As DataSet
        Dim XMLStyleName As String

        Try

            XMLStyleName = dg.Name

            DefaultStyleDS = DataGridCustom.GetTableStyle(XMLStyleName)

            If DefaultStyleDS Is Nothing OrElse DefaultStyleDS.Tables.Count < 1 Then Return

            DGTS = New DataGridTableStyle With {
                .MappingName = dg.GetCurrentDataTable.TableName
            }

            DGTS.GridColumnStyles.Clear()

            If DefaultStyleDS.Tables.Contains(XMLStyleName & "Style") Then
                If DefaultStyleDS.Tables(XMLStyleName & "Style").Columns.Contains("GridLineStyle") Then
                    DGTS.GridLineStyle = If(CBool(DefaultStyleDS.Tables(XMLStyleName & "Style").Rows(0)("GridLineStyle")), DataGridLineStyle.Solid, DataGridLineStyle.None)
                End If
                If DefaultStyleDS.Tables(XMLStyleName & "Style").Columns.Contains("RowHeadersVisible") Then
                    DGTS.RowHeadersVisible = CBool(DefaultStyleDS.Tables(XMLStyleName & "Style").Rows(0)("RowHeadersVisible"))
                End If
            End If

            Dim ResultDT As DataTable = dg.LoadColumnsSizeAndPosition(dg.Name & "\" & DGTS.MappingName & "\ColumnSettings", DefaultStyleDS.Tables("Column"))

            Dim ColumnSequenceDV As New DataView(ResultDT)
            ColsDV = ColumnSequenceDV

            Dim DGTSDefault As New DataGridTableStyle With {
                .MappingName = "Default"
            } 'This can be used to establish the columns displayed by default

            For IntCol = 0 To ColsDV.Count - 1

                Dim GetUserGroups As StringCollection

                GetUserGroups = UFCWGeneralAD.GetUserGroupMembership()

                If ColsDV.Table.Columns.Contains("GAC") AndAlso ColsDV(IntCol).Item("GAC").ToString.Trim.Length > 0 Then

                    If Not GetUserGroups.Contains(ColsDV(IntCol).Item("GAC").ToString) Then
                        Continue For
                    End If

                End If

                If (IsDBNull(ColsDV(IntCol).Item("Visible")) OrElse ColsDV(IntCol).Item("Visible").ToString.Trim.Length = 0 OrElse CBool(ColsDV(IntCol).Item("Visible")) = True) Then
                    TextCol = New DataGridFormattableTextBoxColumn With {
                        .MappingName = CStr(ColsDV(IntCol).Item("Mapping")),
                        .HeaderText = CStr(ColsDV(IntCol).Item("HeaderText"))
                    }
                    If IsDBNull(ColsDV(IntCol).Item("Format")) = False AndAlso ColsDV(IntCol).Item("Format").ToString.Trim.Length > 0 Then
                        TextCol.Format = CStr(ColsDV(IntCol).Item("Format"))
                    End If
                    DGTSDefault.GridColumnStyles.Add(TextCol)
                End If

                If ((IsDBNull(ColsDV(IntCol).Item("Visible")) OrElse ColsDV(IntCol).Item("Visible").ToString.Trim.Length = 0 OrElse CBool(ColsDV(IntCol).Item("Visible")) = True) AndAlso (IsNothing(GetAllSettings(_APPKEY, dg.Name & "\" & DGTS.MappingName & "\Customize\ColumnSelector")) OrElse CDbl(GetSetting(_APPKEY, dg.Name & "\" & DGTS.MappingName & "\Customize\ColumnSelector", "Col " & ColsDV(IntCol).Item("Mapping").ToString & " Customize", "0")) = 1)) Then
                    If ColsDV(IntCol).Item("Type").ToString.Trim = "Text" Then
                        TextCol = New DataGridFormattableTextBoxColumn With {
                            .MappingName = CStr(ColsDV(IntCol).Item("Mapping")),
                            .HeaderText = CStr(ColsDV(IntCol).Item("HeaderText")),
                            .Width = CInt(ColsDV(IntCol).Item("SizeInPixels")),
                            .NullText = CStr(ColsDV(IntCol).Item("NullText"))
                        }
                        TextCol.TextBox.WordWrap = True

                        Try

                            If CBool(ColsDV(IntCol).Item("ReadOnly")) Then
                                TextCol.ReadOnly = True
                            End If

                        Catch ex As Exception
                        End Try

                        If IsDBNull(ColsDV(IntCol).Item("Format")) = False AndAlso ColsDV(IntCol).Item("Format").ToString.Trim.Length > 0 AndAlso ColsDV(IntCol).Item("Format").ToString.Trim <> "YesNo" Then
                            TextCol.Format = CStr(ColsDV(IntCol).Item("Format"))
                        ElseIf ColsDV(IntCol).Item("Format").ToString.Trim = "YesNo" Then
                            AddHandler TextCol.Formatting, AddressOf FormattingYesNo
                        End If

                        DGTS.GridColumnStyles.Add(TextCol)

                    ElseIf ColsDV(IntCol).Item("Type").ToString.Trim = "Bool" Then
                        BoolCol = New DataGridColorBoolColumn(IntCol) With {
                            .MappingName = CStr(ColsDV(IntCol).Item("Mapping")),
                            .HeaderText = CStr(ColsDV(IntCol).Item("HeaderText")),
                            .Width = CInt(ColsDV(IntCol).Item("SizeInPixels")),
                            .NullText = CStr(ColsDV(IntCol).Item("NullText")),
                            .TrueValue = CType("1", Decimal),
                            .FalseValue = CType("0", Decimal),
                            .AllowNull = False
                        }

                        Try

                            If CBool(ColsDV(IntCol).Item("ReadOnly")) Then
                                BoolCol.ReadOnly = True
                            End If

                        Catch ex As Exception
                        End Try

                        DGTS.GridColumnStyles.Add(BoolCol)

                    ElseIf ColsDV(IntCol).Item("Type").ToString.Trim = "Icon" Then

                        IconCol = New DataGridHighlightIconColumn(ClaimDocumentHistoryDataGrid) With {
                            .MappingName = CStr(ColsDV(IntCol).Item("Mapping")),
                            .HeaderText = CStr(ColsDV(IntCol).Item("HeaderText")),
                            .Width = CInt(ColsDV(IntCol).Item("SizeInPixels")),
                            .NullText = CStr(ColsDV(IntCol).Item("NullText")),
                            .MinimumCharWidth = CInt(ColsDV(IntCol).Item("MinimumCharWidth")),
                            .MaximumCharWidth = CInt(ColsDV(IntCol).Item("MaximumCharWidth"))
                        }

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
            DGTS = Nothing
        End Try

    End Sub
    Private Sub FormattingYesNo(ByRef value As Object, ByVal rowNum As Integer)
        Try
            If IsDBNull(value) = False AndAlso CBool(value) Then
                value = "Yes"
            Else
                value = "No"
            End If
        Catch ex As Exception

            Throw
        End Try
    End Sub
    Private Sub DetermineCellIcon(ByRef pic As Image, ByVal cell As System.Windows.Forms.DataGridCell)
        Try
            Dim DV As DataView = ClaimDocumentHistoryDataGrid.GetDefaultDataView
            If DV IsNot Nothing AndAlso IsDBNull(DV(cell.RowNumber)("TRANSACTION_TYPE")) = False Then
                Select Case CStr(DV(cell.RowNumber)("TRANSACTION_TYPE")).ToUpper.Replace("(V1.1)", "")
                    Case Is = "INSERTED"
                        pic = HistoryImageList.Images(0)
                    Case Is = "GETNEXT"
                        pic = HistoryImageList.Images(1)
                    Case Is = "UPDATE"
                        pic = HistoryImageList.Images(2)
                    Case Is = "REPRICE"
                        pic = HistoryImageList.Images(3)
                    Case Is = "ROUTE"
                        pic = HistoryImageList.Images(4)
                    Case Is = "COMPLETE"
                        pic = HistoryImageList.Images(5)
                    Case Is = "LETTER"
                        pic = HistoryImageList.Images(6)
                    Case Is = "AUDIT"
                        pic = HistoryImageList.Images(7)
                    Case Else
                        pic = Nothing
                End Select
            Else
                pic = Nothing
            End If

        Catch ex As Exception

            Throw
        End Try
    End Sub

    Private Sub ClaimDocumentHistoryViewer_dispose(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Disposed

        If ClaimDocumentHistoryDataGrid IsNot Nothing Then
            ClaimDocumentHistoryDataGrid.TableStyles.Clear()
            ClaimDocumentHistoryDataGrid.DataSource = Nothing
            Me.ClaimDocumentHistoryDataGrid.Dispose()
            Me.Dispose()
        End If

    End Sub

#End Region

End Class
