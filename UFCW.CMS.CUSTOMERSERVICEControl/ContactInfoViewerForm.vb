Imports System.ComponentModel

Public Class ContactInfoViewerForm
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"

    Private _FamilyID As Integer
    Private _RelationID As Integer

    Friend WithEvents ExitButton As System.Windows.Forms.Button
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents ContactInfoAddressDataGrid As DataGridCustom
    Friend WithEvents SplitContainer As SplitContainer
    Friend WithEvents ContactInfoPhoneDataGrid As DataGridCustom
    Friend WithEvents ContactInfoEmailDataGrid As DataGridCustom
    Private _ContactInfoDS As DataSet

#Region "Public Properties"
    <DefaultValue("UFCW\Claims\"), Browsable(True), System.ComponentModel.Description("Represents the application key used when accessing the registry.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
        End Set
    End Property

    <Browsable(False), System.ComponentModel.Description("Used to load the Life Events from an external source.")>
    Public Property ContactInfoDS() As DataSet
        Get
            Return _ContactInfoDS
        End Get
        Set(ByVal value As DataSet)
            _ContactInfoDS = value
        End Set
    End Property
#End Region

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal FamilyID As Integer)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _FamilyID = FamilyID
        _RelationID = 0

    End Sub

    Public Sub New(ByVal FamilyID As Integer, ByVal RelationID As Integer)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _FamilyID = FamilyID
        _RelationID = RelationID

    End Sub
    Public Sub New(ByVal ContactInfoDS As DataSet)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        If ContactInfoDS.Tables.Count < 1 OrElse ContactInfoDS.Tables(0).Rows.Count < 1 Then Throw New ArgumentException("DataSet must be preloaded to use this method")

        _FamilyID = CInt(ContactInfoDS.Tables(0).Rows(0)("FAMILY_ID").ToString)
        _RelationID = CInt(ContactInfoDS.Tables(0).Rows(0)("RELATION_ID").ToString)

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
            If _ContactInfoDS IsNot Nothing Then _ContactInfoDS.Dispose()

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
        Me.ExitButton = New System.Windows.Forms.Button()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.ContactInfoAddressDataGrid = New DataGridCustom()
        Me.SplitContainer = New System.Windows.Forms.SplitContainer()
        Me.ContactInfoPhoneDataGrid = New DataGridCustom()
        Me.ContactInfoEmailDataGrid = New DataGridCustom()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.ContactInfoAddressDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer.Panel1.SuspendLayout()
        Me.SplitContainer.Panel2.SuspendLayout()
        Me.SplitContainer.SuspendLayout()
        CType(Me.ContactInfoPhoneDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ContactInfoEmailDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ExitButton
        '
        Me.ExitButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ExitButton.Location = New System.Drawing.Point(422, 583)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(75, 23)
        Me.ExitButton.TabIndex = 1
        Me.ExitButton.Text = "Exit"
        Me.ExitButton.UseVisualStyleBackColor = True
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.ContactInfoAddressDataGrid)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainer)
        Me.SplitContainer1.Size = New System.Drawing.Size(509, 612)
        Me.SplitContainer1.SplitterDistance = 306
        Me.SplitContainer1.TabIndex = 3
        '
        'ContactInfoAddressDataGrid
        '
        Me.ContactInfoAddressDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.ContactInfoAddressDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.ContactInfoAddressDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ContactInfoAddressDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ContactInfoAddressDataGrid.ADGroupsThatCanFind = ""
        Me.ContactInfoAddressDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ContactInfoAddressDataGrid.ADGroupsThatCanMultiSort = ""
        Me.ContactInfoAddressDataGrid.AllowAutoSize = True
        Me.ContactInfoAddressDataGrid.AllowColumnReorder = True
        Me.ContactInfoAddressDataGrid.AllowCopy = True
        Me.ContactInfoAddressDataGrid.AllowCustomize = True
        Me.ContactInfoAddressDataGrid.AllowDelete = False
        Me.ContactInfoAddressDataGrid.AllowDragDrop = False
        Me.ContactInfoAddressDataGrid.AllowEdit = False
        Me.ContactInfoAddressDataGrid.AllowExport = True
        Me.ContactInfoAddressDataGrid.AllowFilter = True
        Me.ContactInfoAddressDataGrid.AllowFind = True
        Me.ContactInfoAddressDataGrid.AllowGoTo = True
        Me.ContactInfoAddressDataGrid.AllowMultiSelect = True
        Me.ContactInfoAddressDataGrid.AllowMultiSort = False
        Me.ContactInfoAddressDataGrid.AllowNew = False
        Me.ContactInfoAddressDataGrid.AllowPrint = True
        Me.ContactInfoAddressDataGrid.AllowRefresh = True
        Me.ContactInfoAddressDataGrid.AppKey = "UFCW\Claims\"
        Me.ContactInfoAddressDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.ContactInfoAddressDataGrid.CaptionText = "Address"
        Me.ContactInfoAddressDataGrid.ColumnHeaderLabel = Nothing
        Me.ContactInfoAddressDataGrid.ColumnRePositioning = False
        Me.ContactInfoAddressDataGrid.ColumnResizing = False
        Me.ContactInfoAddressDataGrid.ConfirmDelete = True
        Me.ContactInfoAddressDataGrid.CopySelectedOnly = False
        Me.ContactInfoAddressDataGrid.DataMember = ""
        Me.ContactInfoAddressDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ContactInfoAddressDataGrid.DragColumn = 0
        Me.ContactInfoAddressDataGrid.ExportSelectedOnly = False
        Me.ContactInfoAddressDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ContactInfoAddressDataGrid.HighlightedRow = Nothing
        Me.ContactInfoAddressDataGrid.IsMouseDown = False
        Me.ContactInfoAddressDataGrid.LastGoToLine = ""
        Me.ContactInfoAddressDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.ContactInfoAddressDataGrid.MultiSort = False
        Me.ContactInfoAddressDataGrid.Name = "ContactInfoAddressDataGrid"
        Me.ContactInfoAddressDataGrid.OldSelectedRow = Nothing
        Me.ContactInfoAddressDataGrid.ReadOnly = True
        Me.ContactInfoAddressDataGrid.SetRowOnRightClick = True
        Me.ContactInfoAddressDataGrid.ShiftPressed = False
        Me.ContactInfoAddressDataGrid.SingleClickBooleanColumns = True
        Me.ContactInfoAddressDataGrid.Size = New System.Drawing.Size(509, 306)
        Me.ContactInfoAddressDataGrid.StyleName = ""
        Me.ContactInfoAddressDataGrid.SubKey = ""
        Me.ContactInfoAddressDataGrid.SuppressTriangle = False
        Me.ContactInfoAddressDataGrid.TabIndex = 1
        '
        'SplitContainer
        '
        Me.SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer.Name = "SplitContainer"
        Me.SplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer.Panel1
        '
        Me.SplitContainer.Panel1.Controls.Add(Me.ContactInfoPhoneDataGrid)
        '
        'SplitContainer.Panel2
        '
        Me.SplitContainer.Panel2.Controls.Add(Me.ContactInfoEmailDataGrid)
        Me.SplitContainer.Size = New System.Drawing.Size(509, 302)
        Me.SplitContainer.SplitterDistance = 148
        Me.SplitContainer.TabIndex = 3
        '
        'ContactInfoPhoneDataGrid
        '
        Me.ContactInfoPhoneDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.ContactInfoPhoneDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.ContactInfoPhoneDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ContactInfoPhoneDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ContactInfoPhoneDataGrid.ADGroupsThatCanFind = ""
        Me.ContactInfoPhoneDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ContactInfoPhoneDataGrid.ADGroupsThatCanMultiSort = ""
        Me.ContactInfoPhoneDataGrid.AllowAutoSize = True
        Me.ContactInfoPhoneDataGrid.AllowColumnReorder = True
        Me.ContactInfoPhoneDataGrid.AllowCopy = True
        Me.ContactInfoPhoneDataGrid.AllowCustomize = True
        Me.ContactInfoPhoneDataGrid.AllowDelete = False
        Me.ContactInfoPhoneDataGrid.AllowDragDrop = False
        Me.ContactInfoPhoneDataGrid.AllowEdit = False
        Me.ContactInfoPhoneDataGrid.AllowExport = True
        Me.ContactInfoPhoneDataGrid.AllowFilter = True
        Me.ContactInfoPhoneDataGrid.AllowFind = True
        Me.ContactInfoPhoneDataGrid.AllowGoTo = True
        Me.ContactInfoPhoneDataGrid.AllowMultiSelect = True
        Me.ContactInfoPhoneDataGrid.AllowMultiSort = False
        Me.ContactInfoPhoneDataGrid.AllowNew = False
        Me.ContactInfoPhoneDataGrid.AllowPrint = True
        Me.ContactInfoPhoneDataGrid.AllowRefresh = True
        Me.ContactInfoPhoneDataGrid.AppKey = "UFCW\Claims\"
        Me.ContactInfoPhoneDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.ContactInfoPhoneDataGrid.CaptionText = "Phone"
        Me.ContactInfoPhoneDataGrid.ColumnHeaderLabel = Nothing
        Me.ContactInfoPhoneDataGrid.ColumnRePositioning = False
        Me.ContactInfoPhoneDataGrid.ColumnResizing = False
        Me.ContactInfoPhoneDataGrid.ConfirmDelete = True
        Me.ContactInfoPhoneDataGrid.CopySelectedOnly = False
        Me.ContactInfoPhoneDataGrid.DataMember = ""
        Me.ContactInfoPhoneDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ContactInfoPhoneDataGrid.DragColumn = 0
        Me.ContactInfoPhoneDataGrid.ExportSelectedOnly = False
        Me.ContactInfoPhoneDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ContactInfoPhoneDataGrid.HighlightedRow = Nothing
        Me.ContactInfoPhoneDataGrid.IsMouseDown = False
        Me.ContactInfoPhoneDataGrid.LastGoToLine = ""
        Me.ContactInfoPhoneDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.ContactInfoPhoneDataGrid.MultiSort = False
        Me.ContactInfoPhoneDataGrid.Name = "ContactInfoPhoneDataGrid"
        Me.ContactInfoPhoneDataGrid.OldSelectedRow = Nothing
        Me.ContactInfoPhoneDataGrid.ReadOnly = True
        Me.ContactInfoPhoneDataGrid.SetRowOnRightClick = True
        Me.ContactInfoPhoneDataGrid.ShiftPressed = False
        Me.ContactInfoPhoneDataGrid.SingleClickBooleanColumns = True
        Me.ContactInfoPhoneDataGrid.Size = New System.Drawing.Size(509, 148)
        Me.ContactInfoPhoneDataGrid.StyleName = ""
        Me.ContactInfoPhoneDataGrid.SubKey = ""
        Me.ContactInfoPhoneDataGrid.SuppressTriangle = False
        Me.ContactInfoPhoneDataGrid.TabIndex = 0
        '
        'ContactInfoEmailDataGrid
        '
        Me.ContactInfoEmailDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.ContactInfoEmailDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.ContactInfoEmailDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ContactInfoEmailDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ContactInfoEmailDataGrid.ADGroupsThatCanFind = ""
        Me.ContactInfoEmailDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ContactInfoEmailDataGrid.ADGroupsThatCanMultiSort = ""
        Me.ContactInfoEmailDataGrid.AllowAutoSize = True
        Me.ContactInfoEmailDataGrid.AllowColumnReorder = True
        Me.ContactInfoEmailDataGrid.AllowCopy = True
        Me.ContactInfoEmailDataGrid.AllowCustomize = True
        Me.ContactInfoEmailDataGrid.AllowDelete = False
        Me.ContactInfoEmailDataGrid.AllowDragDrop = False
        Me.ContactInfoEmailDataGrid.AllowEdit = False
        Me.ContactInfoEmailDataGrid.AllowExport = True
        Me.ContactInfoEmailDataGrid.AllowFilter = True
        Me.ContactInfoEmailDataGrid.AllowFind = True
        Me.ContactInfoEmailDataGrid.AllowGoTo = True
        Me.ContactInfoEmailDataGrid.AllowMultiSelect = True
        Me.ContactInfoEmailDataGrid.AllowMultiSort = False
        Me.ContactInfoEmailDataGrid.AllowNew = False
        Me.ContactInfoEmailDataGrid.AllowPrint = True
        Me.ContactInfoEmailDataGrid.AllowRefresh = True
        Me.ContactInfoEmailDataGrid.AppKey = "UFCW\Claims\"
        Me.ContactInfoEmailDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.ContactInfoEmailDataGrid.CaptionText = "Email"
        Me.ContactInfoEmailDataGrid.ColumnHeaderLabel = Nothing
        Me.ContactInfoEmailDataGrid.ColumnRePositioning = False
        Me.ContactInfoEmailDataGrid.ColumnResizing = False
        Me.ContactInfoEmailDataGrid.ConfirmDelete = True
        Me.ContactInfoEmailDataGrid.CopySelectedOnly = False
        Me.ContactInfoEmailDataGrid.DataMember = ""
        Me.ContactInfoEmailDataGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ContactInfoEmailDataGrid.DragColumn = 0
        Me.ContactInfoEmailDataGrid.ExportSelectedOnly = False
        Me.ContactInfoEmailDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ContactInfoEmailDataGrid.HighlightedRow = Nothing
        Me.ContactInfoEmailDataGrid.IsMouseDown = False
        Me.ContactInfoEmailDataGrid.LastGoToLine = ""
        Me.ContactInfoEmailDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.ContactInfoEmailDataGrid.MultiSort = False
        Me.ContactInfoEmailDataGrid.Name = "ContactInfoEmailDataGrid"
        Me.ContactInfoEmailDataGrid.OldSelectedRow = Nothing
        Me.ContactInfoEmailDataGrid.ReadOnly = True
        Me.ContactInfoEmailDataGrid.SetRowOnRightClick = True
        Me.ContactInfoEmailDataGrid.ShiftPressed = False
        Me.ContactInfoEmailDataGrid.SingleClickBooleanColumns = True
        Me.ContactInfoEmailDataGrid.Size = New System.Drawing.Size(509, 150)
        Me.ContactInfoEmailDataGrid.StyleName = ""
        Me.ContactInfoEmailDataGrid.SubKey = ""
        Me.ContactInfoEmailDataGrid.SuppressTriangle = False
        Me.ContactInfoEmailDataGrid.TabIndex = 0
        '
        'ContactInfoViewerForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.ExitButton
        Me.ClientSize = New System.Drawing.Size(509, 612)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.ExitButton)
        Me.Name = "ContactInfoViewerForm"
        Me.Text = "Contact Info"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.ContactInfoAddressDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer.Panel1.ResumeLayout(False)
        Me.SplitContainer.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer.ResumeLayout(False)
        CType(Me.ContactInfoPhoneDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ContactInfoEmailDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Custom Procedure"

    Private Sub LoadContactInfo()
        Try
            Using WC As New GlobalCursor

                If _ContactInfoDS Is Nothing Then
                    _ContactInfoDS = CMSDALFDBEL.RetrieveContactInfoByFamilyIDRelationID(_FamilyID, _RelationID)
                End If

                ContactInfoEmailDataGrid.SuspendLayout()
                ContactInfoPhoneDataGrid.SuspendLayout()
                ContactInfoAddressDataGrid.SuspendLayout()

                ContactInfoEmailDataGrid.DataSource = _ContactInfoDS.Tables("REG_EMAIL")
                ContactInfoPhoneDataGrid.DataSource = _ContactInfoDS.Tables("REG_PHONE")
                ContactInfoAddressDataGrid.DataSource = _ContactInfoDS.Tables("REG_ADDRESS")

                ContactInfoPhoneDataGrid.SetTableStyle()
                ContactInfoEmailDataGrid.SetTableStyle()
                ContactInfoAddressDataGrid.SetTableStyle()

                If _ContactInfoDS.Tables("REG_REGISTRATION_INFO").Rows.Count > 0 Then
                    Me.Text += " Family: " & _FamilyID.ToString & " Relation:" & _RelationID.ToString & " (Registered on Portal - " & _ContactInfoDS.Tables("REG_REGISTRATION_INFO")(0)("REGISTRATION_COMPLETED_DATE").ToString & ")"
                End If

            End Using

        Catch ex As Exception
            Throw
        Finally

            ContactInfoEmailDataGrid.ResumeLayout()
            ContactInfoPhoneDataGrid.ResumeLayout()
            ContactInfoAddressDataGrid.ResumeLayout()

        End Try
    End Sub

    Private Sub ContactInfoForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not UFCWGeneral.SetFormPosition(Me, _APPKEY) Then Me.CenterToScreen()

        LoadContactInfo()
    End Sub

#End Region

#Region "Form Events"

    Private Sub ContactInfoForm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

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