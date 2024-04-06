Imports System.ComponentModel

Public Class LifeEventsViewerForm
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"
    Private _FamilyID As Integer
    Private _RelationID As Short? = Nothing
    Private _FromDate As Date
    Private _ThruDate As Date

    Friend WithEvents LifeEventsDataGrid As DataGridCustom
    Friend WithEvents ExitButton As System.Windows.Forms.Button

    Private _LifeEventsDS As DataSet

#Region "Public Properties"
    <DefaultValue("UFCW\Claims\"), Browsable(True), System.ComponentModel.Description("Represents the application key used when accessing the registry.")> _
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = Value
        End Set
    End Property

    <Browsable(False), System.ComponentModel.Description("Used to load the Life Events from an external source.")> _
    Public Property LifeEventsDS() As DataSet
        Get
            Return _LifeEventsDS
        End Get
        Set(ByVal value As DataSet)
            _LifeEventsDS = Value
        End Set
    End Property
#End Region

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal familyID As Integer, ByVal relationID As Short?, ByVal fromDate As Date, ByVal thruDate As Date)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _FamilyID = FamilyID
        _RelationID = RelationID
        _FromDate = FromDate
        _ThruDate = ThruDate

    End Sub

    Public Sub New(ByVal lifeEventsDS As DataSet, ByVal fromDate As Date, ByVal thruDate As Date)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        If LifeEventsDS.Tables.Count < 1 OrElse LifeEventsDS.Tables(0).Rows.Count < 1 Then Throw New ArgumentException("DataSet must be preloaded to use this method")

        _FamilyID = CInt(LifeEventsDS.Tables(0).Rows(0)("FAMILY_ID").ToString)
        _RelationID = UFCWGeneral.IsNullShortHandler(lifeEventsDS.Tables(0).Rows(0)("RELATION_ID"))
        _FromDate = fromDate
        _ThruDate = thruDate

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
            If _LifeEventsDS IsNot Nothing Then _LifeEventsDS.Dispose()

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
        Me.LifeEventsDataGrid = New DataGridCustom()
        Me.ExitButton = New System.Windows.Forms.Button()
        CType(Me.LifeEventsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LifeEventsDataGrid
        '
        Me.LifeEventsDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.LifeEventsDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.LifeEventsDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LifeEventsDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LifeEventsDataGrid.ADGroupsThatCanFind = ""
        Me.LifeEventsDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LifeEventsDataGrid.ADGroupsThatCanMultiSort = ""
        Me.LifeEventsDataGrid.AllowAutoSize = True
        Me.LifeEventsDataGrid.AllowColumnReorder = True
        Me.LifeEventsDataGrid.AllowCopy = True
        Me.LifeEventsDataGrid.AllowCustomize = True
        Me.LifeEventsDataGrid.AllowDelete = False
        Me.LifeEventsDataGrid.AllowDragDrop = False
        Me.LifeEventsDataGrid.AllowEdit = False
        Me.LifeEventsDataGrid.AllowExport = True
        Me.LifeEventsDataGrid.AllowFilter = True
        Me.LifeEventsDataGrid.AllowFind = True
        Me.LifeEventsDataGrid.AllowGoTo = True
        Me.LifeEventsDataGrid.AllowMultiSelect = False
        Me.LifeEventsDataGrid.AllowMultiSort = False
        Me.LifeEventsDataGrid.AllowNew = False
        Me.LifeEventsDataGrid.AllowPrint = True
        Me.LifeEventsDataGrid.AllowRefresh = False
        Me.LifeEventsDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LifeEventsDataGrid.AppKey = "UFCW\Claims\"
        Me.LifeEventsDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.LifeEventsDataGrid.CaptionVisible = False
        Me.LifeEventsDataGrid.ColumnHeaderLabel = Nothing
        Me.LifeEventsDataGrid.ColumnRePositioning = False
        Me.LifeEventsDataGrid.ColumnResizing = False
        Me.LifeEventsDataGrid.ConfirmDelete = True
        Me.LifeEventsDataGrid.CopySelectedOnly = True
        Me.LifeEventsDataGrid.DataMember = ""
        Me.LifeEventsDataGrid.DragColumn = 0
        Me.LifeEventsDataGrid.ExportSelectedOnly = True
        Me.LifeEventsDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.LifeEventsDataGrid.HighlightedRow = Nothing
        Me.LifeEventsDataGrid.IsMouseDown = False
        Me.LifeEventsDataGrid.LastGoToLine = ""
        Me.LifeEventsDataGrid.Location = New System.Drawing.Point(2, 3)
        Me.LifeEventsDataGrid.MultiSort = False
        Me.LifeEventsDataGrid.Name = "LifeEventsDataGrid"
        Me.LifeEventsDataGrid.OldSelectedRow = Nothing
        Me.LifeEventsDataGrid.ReadOnly = True
        Me.LifeEventsDataGrid.SetRowOnRightClick = True
        Me.LifeEventsDataGrid.ShiftPressed = False
        Me.LifeEventsDataGrid.SingleClickBooleanColumns = True
        Me.LifeEventsDataGrid.Size = New System.Drawing.Size(468, 264)
        Me.LifeEventsDataGrid.StyleName = ""
        Me.LifeEventsDataGrid.SubKey = ""
        Me.LifeEventsDataGrid.SuppressTriangle = False
        Me.LifeEventsDataGrid.TabIndex = 0
        '
        'ExitButton
        '
        Me.ExitButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ExitButton.Location = New System.Drawing.Point(385, 273)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(75, 23)
        Me.ExitButton.TabIndex = 1
        Me.ExitButton.Text = "Exit"
        Me.ExitButton.UseVisualStyleBackColor = True
        '
        'LifeEventsViewerForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.ExitButton
        Me.ClientSize = New System.Drawing.Size(472, 302)
        Me.Controls.Add(Me.ExitButton)
        Me.Controls.Add(Me.LifeEventsDataGrid)
        Me.Name = "LifeEventsViewerForm"
        Me.Text = "Life Events History"
        CType(Me.LifeEventsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region
#Region "Custom Procedure"

    Private Sub LoadLifeEvents()
        Dim TempDT As DataTable

        Try
            Using WC As New GlobalCursor

                If _LifeEventsDS Is Nothing Then
                    _LifeEventsDS = CMSDALFDBEL.RetrievePatientsLifeEvents(_FamilyID)
                End If

                If Not _LifeEventsDS.Tables.Contains("REG_LIFE_EVENTS_GAPS") Then
                    TempDT = CMSDALFDBEL.RetrievePatientsSummarizedCoverableGapInfo(_FamilyID, CShort(_RelationID), _FromDate, If(_ThruDate > Now, UFCWGeneral.NowDate, _ThruDate))
                Else
                    TempDT = _LifeEventsDS.Tables("REG_LIFE_EVENTS_GAPS")
                End If

                If TempDT IsNot Nothing AndAlso TempDT.Rows.Count > 0 Then
                    For Each DR As DataRow In TempDT.Rows
                        _LifeEventsDS.Tables("REG_LIFE_EVENTS").Rows.Add(New Object() {_FamilyID, _RelationID, DR("BeginDate"), DR("EndDate"), 999, "NOT COVERABLE"})
                    Next
                Else   ''Displaying terminated Audit related Events
                    If DateDiff("d", _FromDate, _ThruDate) = 0 Then
                        TempDT = CMSDALFDBEL.RetrieveRegLifeEventsForAuditByFamilyIDRelationID(_FamilyID, _RelationID)
                        LifeEventsDataGrid.DataSource = TempDT
                        LifeEventsDataGrid.SetTableStyle()
                        Exit Sub
                    End If
                End If

                _LifeEventsDS.Tables("REG_LIFE_EVENTS").DefaultView.RowFilter = "RELATION_ID = " & _RelationID
                _LifeEventsDS.Tables("REG_LIFE_EVENTS").DefaultView.Sort = "FROM_DATE,THRU_DATE"

                LifeEventsDataGrid.DataSource = _LifeEventsDS.Tables("REG_LIFE_EVENTS")
                LifeEventsDataGrid.SetTableStyle()

            End Using

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

#End Region
#Region "Form Events"
    Private Sub LifeEventsViewerForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not UFCWGeneral.SetFormPosition(Me, _APPKEY) Then Me.CenterToScreen()

        LoadLifeEvents()

    End Sub

    Private Sub LifeEventsViewerForm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        Try
            UFCWGeneral.SaveFormPosition(Me, _APPKEY)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ExitButton_Click(sender As System.Object, e As System.EventArgs) Handles ExitButton.Click
        Me.Close()
    End Sub

#End Region

End Class