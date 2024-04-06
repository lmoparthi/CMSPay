Option Strict On

Imports System.ComponentModel
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Public Class PrescriptionsControl
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _FamilyID As Integer = -1
    Private _RelationID As Short? = Nothing
    Private _SSN As Integer? = Nothing
    Private _APPKEY As String = "UFCW\Claims\"

    Public Event BeforeRefresh(ByVal sender As Object, ByRef Cancel As Boolean)
    Public Event AfterRefresh(ByVal sender As Object)

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Dim DesignMode As Boolean = (LicenseManager.UsageMode = LicenseUsageMode.Designtime)

        If Not DesignMode Then
            PrescriptionsFromDate.MinDate = DateTime.Today.AddMonths(-36)
            PrescriptionsFromDate.MaxDate = DateTime.Today()
            PrescriptionsFromDate.Value = DateTime.Today.AddMonths(-36)
            PrescriptionsFromDate.Checked = False

            PrescriptionsToDate.MinDate = DateTime.Today.AddMonths(-36)
            PrescriptionsToDate.MaxDate = DateTime.Today()
            PrescriptionsToDate.Value = DateTime.Today()
            PrescriptionsToDate.Checked = False

        End If

    End Sub

    Public Sub New(ByVal familyID As Integer, ByVal relationID As Short?)
        Me.New()

        _FamilyID = familyID
        _RelationID = relationID

    End Sub

    Private Sub PrescriptionsControl_dispose(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Disposed

        PrescriptionsDataGrid.TableStyles.Clear()
        PrescriptionsDataGrid.DataSource = Nothing
        PrescriptionsDataGrid.Dispose()

        MyBase.Dispose()

    End Sub

    'PremiumsControl overrides dispose to clean up the component list.
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
    Friend WithEvents PrescriptionsDataGrid As DataGridCustom
    Friend WithEvents _PrescriptionsDS As PrescriptionsDataSet
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PrescriptionsFromDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents PrescriptionsToDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents RefreshButton As System.Windows.Forms.Button

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.RefreshButton = New System.Windows.Forms.Button()
        Me.PrescriptionsToDate = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.PrescriptionsFromDate = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me._PrescriptionsDS = New PrescriptionsDataSet()
        Me.PrescriptionsDataGrid = New DataGridCustom()
        Me.GroupBox1.SuspendLayout()
        CType(Me._PrescriptionsDS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PrescriptionsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.RefreshButton)
        Me.GroupBox1.Controls.Add(Me.PrescriptionsToDate)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.PrescriptionsFromDate)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(0, -4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(438, 32)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        '
        'RefreshButton
        '
        Me.RefreshButton.Location = New System.Drawing.Point(345, 9)
        Me.RefreshButton.Name = "RefreshButton"
        Me.RefreshButton.Size = New System.Drawing.Size(75, 21)
        Me.RefreshButton.TabIndex = 4
        Me.RefreshButton.Text = "Refresh"
        Me.RefreshButton.UseVisualStyleBackColor = True
        '
        'PrescriptionsToDate
        '
        Me.PrescriptionsToDate.Checked = False
        Me.PrescriptionsToDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.PrescriptionsToDate.Location = New System.Drawing.Point(231, 9)
        Me.PrescriptionsToDate.Name = "PrescriptionsToDate"
        Me.PrescriptionsToDate.ShowCheckBox = True
        Me.PrescriptionsToDate.Size = New System.Drawing.Size(96, 20)
        Me.PrescriptionsToDate.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(207, 12)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(20, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        '
        'PrescriptionsFromDate
        '
        Me.PrescriptionsFromDate.Checked = False
        Me.PrescriptionsFromDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.PrescriptionsFromDate.Location = New System.Drawing.Point(106, 9)
        Me.PrescriptionsFromDate.Name = "PrescriptionsFromDate"
        Me.PrescriptionsFromDate.ShowCheckBox = True
        Me.PrescriptionsFromDate.Size = New System.Drawing.Size(97, 20)
        Me.PrescriptionsFromDate.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(93, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Prescriptions From"
        '
        '_PrescriptionsDS
        '
        Me._PrescriptionsDS.DataSetName = "PrescriptionsDataSet"
        Me._PrescriptionsDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'PrescriptionsDataGrid
        '
        Me.PrescriptionsDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.PrescriptionsDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.PrescriptionsDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PrescriptionsDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PrescriptionsDataGrid.ADGroupsThatCanFind = ""
        Me.PrescriptionsDataGrid.ADGroupsThatCanMultiSort = ""
        Me.PrescriptionsDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PrescriptionsDataGrid.AllowAutoSize = True
        Me.PrescriptionsDataGrid.AllowColumnReorder = True
        Me.PrescriptionsDataGrid.AllowCopy = True
        Me.PrescriptionsDataGrid.AllowCustomize = True
        Me.PrescriptionsDataGrid.AllowDelete = False
        Me.PrescriptionsDataGrid.AllowDragDrop = False
        Me.PrescriptionsDataGrid.AllowEdit = False
        Me.PrescriptionsDataGrid.AllowExport = True
        Me.PrescriptionsDataGrid.AllowFilter = True
        Me.PrescriptionsDataGrid.AllowFind = True
        Me.PrescriptionsDataGrid.AllowGoTo = True
        Me.PrescriptionsDataGrid.AllowMultiSelect = False
        Me.PrescriptionsDataGrid.AllowMultiSort = True
        Me.PrescriptionsDataGrid.AllowNew = False
        Me.PrescriptionsDataGrid.AllowPrint = True
        Me.PrescriptionsDataGrid.AllowRefresh = False
        Me.PrescriptionsDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PrescriptionsDataGrid.AppKey = "UFCW\Claims\"
        Me.PrescriptionsDataGrid.AutoSaveCols = True
        Me.PrescriptionsDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.PrescriptionsDataGrid.CaptionText = "Prescriptions"
        Me.PrescriptionsDataGrid.ColumnHeaderLabel = Nothing
        Me.PrescriptionsDataGrid.ColumnRePositioning = False
        Me.PrescriptionsDataGrid.ColumnResizing = False
        Me.PrescriptionsDataGrid.ConfirmDelete = True
        Me.PrescriptionsDataGrid.CopySelectedOnly = True
        Me.PrescriptionsDataGrid.CurrentBSPosition = -1
        Me.PrescriptionsDataGrid.DataMember = ""
        Me.PrescriptionsDataGrid.DragColumn = 0
        Me.PrescriptionsDataGrid.ExportSelectedOnly = True
        Me.PrescriptionsDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.PrescriptionsDataGrid.HighlightedRow = Nothing
        Me.PrescriptionsDataGrid.HighLightModifiedRows = False
        Me.PrescriptionsDataGrid.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.PrescriptionsDataGrid.IsMouseDown = False
        Me.PrescriptionsDataGrid.LastGoToLine = ""
        Me.PrescriptionsDataGrid.Location = New System.Drawing.Point(0, 30)
        Me.PrescriptionsDataGrid.MultiSort = False
        Me.PrescriptionsDataGrid.Name = "PrescriptionsDataGrid"
        Me.PrescriptionsDataGrid.OldSelectedRow = Nothing
        Me.PrescriptionsDataGrid.ParentRowsVisible = False
        Me.PrescriptionsDataGrid.PreviousBSPosition = -1
        Me.PrescriptionsDataGrid.ReadOnly = True
        Me.PrescriptionsDataGrid.RetainRowSelectionAfterSort = True
        Me.PrescriptionsDataGrid.SetRowOnRightClick = True
        Me.PrescriptionsDataGrid.ShiftPressed = False
        Me.PrescriptionsDataGrid.SingleClickBooleanColumns = True
        Me.PrescriptionsDataGrid.Size = New System.Drawing.Size(438, 328)
        Me.PrescriptionsDataGrid.Sort = Nothing
        Me.PrescriptionsDataGrid.StyleName = ""
        Me.PrescriptionsDataGrid.SubKey = ""
        Me.PrescriptionsDataGrid.SuppressMouseDown = False
        Me.PrescriptionsDataGrid.SuppressTriangle = False
        Me.PrescriptionsDataGrid.TabIndex = 0
        '
        'PrescriptionsControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.PrescriptionsDataGrid)
        Me.Name = "PrescriptionsControl"
        Me.Size = New System.Drawing.Size(440, 360)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me._PrescriptionsDS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PrescriptionsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region

#Region "Properties"
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
    Public Property RelationID() As Short?
        Get
            Return _RelationID
        End Get
        Set(ByVal value As Short?)
            _RelationID = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property PrescriptionFrom() As Date
        Get
            Return PrescriptionsFromDate.Value
        End Get
        Set(ByVal value As Date)
            PrescriptionsFromDate.Value = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property PrescriptionTo() As Date
        Get
            Return PrescriptionsToDate.Value.Date

        End Get
        Set(ByVal value As Date)
            PrescriptionsToDate.Value = value
        End Set
    End Property
#End Region

#Region "Constructor"

    Public Sub New(ByVal familyID As Integer, ByVal relationID As Short?, ByVal presFrom As Date, ByVal presTo As Date)
        Me.New()
        _FamilyID = familyID
        _RelationID = relationID
        _SSN = Nothing

        PrescriptionsFromDate.Value = presFrom.Date
        PrescriptionsFromDate.Checked = True
        PrescriptionsToDate.Value = presTo.Date
        PrescriptionsToDate.Checked = True

        LoadPrescriptionsControl()
    End Sub

    Public Sub New(ByVal ssn As Integer, ByVal presFrom As Date, ByVal presTo As Date)
        Me.New()

        _SSN = ssn
        _FamilyID = Nothing
        _RelationID = Nothing

        PrescriptionsFromDate.Value = presFrom.Date
        PrescriptionsFromDate.Checked = True
        PrescriptionsToDate.Value = presTo.Date
        PrescriptionsToDate.Checked = True

        LoadPrescriptionsControl()
    End Sub
#End Region

#Region "Custom Subs\Functions"
    Public Sub LoadPrescriptionsControl(ByVal presFrom As Date, ByVal presTo As Date, ByVal ssn As Integer?, Optional ByRef prescriptionsDataSet As PrescriptionsDataSet = Nothing)
        Try
            _SSN = ssn
            _FamilyID = Nothing
            _RelationID = Nothing

            PrescriptionsFromDate.Value = presFrom.Date
            PrescriptionsFromDate.Checked = True
            PrescriptionsToDate.Value = presTo.Date
            PrescriptionsToDate.Checked = True

            LoadPrescriptionsControl(prescriptionsDataSet)

        Catch ex As Exception

	Throw
        End Try
    End Sub

    Public Sub LoadPrescriptionsControl(ByVal familyID As Integer, ByVal relationID As Short?, Optional ByRef prescriptionsDataSet As PrescriptionsDataSet = Nothing)
        Try
            _FamilyID = familyID
            _RelationID = relationID
            _SSN = Nothing

            LoadPrescriptionsControl(prescriptionsDataSet)

        Catch ex As Exception

	Throw
        End Try
    End Sub

    Public Sub LoadPrescriptionsControl(ByVal presFrom As Date, ByVal presTo As Date, ByVal familyID As Integer, ByVal relationID As Short?, Optional ByRef prescriptionsDataSet As PrescriptionsDataSet = Nothing)
        Try
            _FamilyID = familyID
            _RelationID = relationID
            _SSN = Nothing

            PrescriptionsFromDate.MinDate = presFrom.Date
            PrescriptionsFromDate.MaxDate = presTo.Date
            PrescriptionsFromDate.Value = presFrom.Date
            PrescriptionsFromDate.Checked = True

            PrescriptionsToDate.MinDate = presFrom.Date
            PrescriptionsToDate.MaxDate = presTo.Date
            PrescriptionsToDate.Value = presTo.Date
            PrescriptionsToDate.Checked = True

            LoadPrescriptionsControl(prescriptionsDataSet)

        Catch ex As Exception

	Throw
        End Try
    End Sub

    Private Sub LoadPrescriptionsControl(Optional ByRef prescriptionsDS As PrescriptionsDataSet = Nothing)
        Try

            PrescriptionsDataGrid.DataSource = Nothing

            If prescriptionsDS Is Nothing Then
                _PrescriptionsDS.Tables.Clear()

                If _FamilyID < 0 Then
                    _PrescriptionsDS = CType(PrescriptionsDAL.GetPrescriptionsInformation(_SSN, PrescriptionsFromDate.Value.Date, PrescriptionsToDate.Value.Date, _PrescriptionsDS), PrescriptionsDataSet)
                Else
                    _PrescriptionsDS = CType(PrescriptionsDAL.GetPrescriptionsInformation(PrescriptionsFromDate.Value.Date, PrescriptionsToDate.Value.Date, _FamilyID, _RelationID, _PrescriptionsDS), PrescriptionsDataSet)
                End If
            Else
                _PrescriptionsDS = prescriptionsDS
            End If

            PrescriptionsDataGrid.DataSource = _PrescriptionsDS.Tables("Prescriptions")
            PrescriptionsDataGrid.SetTableStyle()
            PrescriptionsDataGrid.Sort = If(PrescriptionsDataGrid.LastSortedBy, PrescriptionsDataGrid.DefaultSort)

            If Not PrescriptionsFromDate.Checked AndAlso _PrescriptionsDS.Tables("Prescriptions").Rows.Count > 0 Then
                PrescriptionsFromDate.MinDate = (From r In _PrescriptionsDS.Tables("Prescriptions").AsEnumerable Select r.Field(Of Date)("DISPENSE_DATE")).Min
                PrescriptionsFromDate.Value = PrescriptionsFromDate.MinDate
                PrescriptionsFromDate.Checked = False

                PrescriptionsToDate.MinDate = PrescriptionsFromDate.MinDate
                PrescriptionsToDate.Checked = False
            End If

        Catch ex As Exception

	Throw
        End Try
    End Sub

    Public Sub ClearAll()

        If PrescriptionsDataGrid IsNot Nothing Then
            PrescriptionsDataGrid.DataSource = Nothing
        End If

        PrescriptionsFromDate.MinDate = DateTime.Today.AddMonths(-36)
        PrescriptionsFromDate.MaxDate = DateTime.Today()
        PrescriptionsFromDate.Value = DateTime.Today.AddMonths(-36)
        PrescriptionsFromDate.Checked = False

        PrescriptionsToDate.MinDate = DateTime.Today.AddMonths(-36)
        PrescriptionsToDate.MaxDate = DateTime.Today()
        PrescriptionsToDate.Value = DateTime.Today()
        PrescriptionsToDate.Checked = False

    End Sub

    Private Sub RefreshButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshButton.Click

        DirectCast(PrescriptionsDataGrid.DataSource, DataTable).DefaultView.RowFilter = "DISPENSE_DATE >= #" & PrescriptionsFromDate.Value & "# AND DISPENSE_DATE <= #" & PrescriptionsToDate.Value & "#"

    End Sub

#End Region

End Class