Imports System.ComponentModel
Imports System.Configuration

Public Class AnnotationsControl
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private Shared _DefaultProviderName As String = CType(ConfigurationManager.GetSection("dataConfiguration"), Microsoft.Practices.EnterpriseLibrary.Data.Configuration.DatabaseSettings).DefaultDatabase

    Private _ClaimID As Integer
    Private _FamilyID As Integer
    Private _RelationID As Integer
    Private _PartSSN As Integer
    Private _PatSSN As Integer
    Private _PartFName As Object
    Private _PartLName As Object
    Private _PatFName As Object
    Private _PatLName As Object
    Private _DialogResult As DialogResult
    Private _SaveAnnotations As Boolean = True

    Private _APPKEY As String = "UFCW\Claims\"

    Private _AnnotationsDS As New AnnotationsDataSet

    '    Private LastButton As Button = Nothing
    Private _TextCol As DataGridHighlightTextBoxColumn
    Private _ColTBox As DataGridTextBox
    Private _IconCol As DataGridHighlightIconColumn

    Private _DomainUser As String = SystemInformation.UserName

    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents AnnotationTextBox As System.Windows.Forms.TextBox
    Friend WithEvents FlagImageSelector As ImageSelector.ImageSelector
    Friend WithEvents ImageItem1 As ImageSelector.ImageItem
    Friend WithEvents ImageItem2 As ImageSelector.ImageItem
    Friend WithEvents ImageItem3 As ImageSelector.ImageItem
    Friend WithEvents ImageItem4 As ImageSelector.ImageItem
    Friend WithEvents ImageItem5 As ImageSelector.ImageItem
    Public WithEvents CancelActionButton As System.Windows.Forms.Button
    Friend WithEvents SaveButton As System.Windows.Forms.Button
    Private WithEvents AnnotationDataGrid As DataGridCustom
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents AddButton As System.Windows.Forms.Button

    Private _HoverCell As New DataGridCell
    Private WithEvents EnhancedToolTip As EnhancedToolTip

    Const MAXANNOTATIONLENGTH As Integer = 3700

    Public Event Closing(sender As Object, e As EventArgs)
    Public Event Save(sender As Object, e As AnnotationsEvent)


#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub
    Public Sub New(ByVal ClaimID As Integer, ByVal FamilyID As Integer, ByVal RelationID As Integer, ByVal PartSSN As Integer, ByVal PatSSN As Integer, ByVal PartFName As Object, ByVal PartLName As Object, ByVal PatFName As Object, ByVal PatLName As Object, ByVal AnnotationTable As DataTable)
        Me.New()

        _ClaimID = ClaimID
        _FamilyID = FamilyID
        _RelationID = RelationID
        _PartSSN = PartSSN
        _PatSSN = PatSSN
        _PartFName = PartFName
        _PartLName = PartLName
        _PatFName = PatFName
        _PatLName = PatLName

        If UFCWGeneralAD.CMSLocalsEmployee Then
            AnnotationTextBox.Enabled = False
            AddButton.Enabled = False
            SaveButton.Enabled = False
            FlagImageSelector.Enabled = False
        End If

        Dim dv As New DataView(AnnotationTable, "", "", DataViewRowState.CurrentRows)
        If dv.Count > 0 Then
            For cnt As Integer = 0 To dv.Count - 1
                _AnnotationsDS.ANNOTATIONS.Rows.Add(dv(cnt).Row.ItemArray)
            Next

            _AnnotationsDS.ANNOTATIONS.AcceptChanges()
        End If
    End Sub



    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents FlagImageList As System.Windows.Forms.ImageList
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AnnotationsControl))
        Me.FlagImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.AnnotationTextBox = New System.Windows.Forms.TextBox()
        Me.FlagImageSelector = New ImageSelector.ImageSelector()
        Me.ImageItem1 = New ImageSelector.ImageItem()
        Me.ImageItem2 = New ImageSelector.ImageItem()
        Me.ImageItem3 = New ImageSelector.ImageItem()
        Me.ImageItem4 = New ImageSelector.ImageItem()
        Me.ImageItem5 = New ImageSelector.ImageItem()
        Me.CancelActionButton = New System.Windows.Forms.Button()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.AnnotationDataGrid = New DataGridCustom()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.AddButton = New System.Windows.Forms.Button()
        Me.EnhancedToolTip = New EnhancedToolTip(Me.components)
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.FlagImageSelector.SuspendLayout()
        CType(Me.AnnotationDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'FlagImageList
        '
        Me.FlagImageList.ImageStream = CType(Resources.GetObject("FlagImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.FlagImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.FlagImageList.Images.SetKeyName(0, "")
        Me.FlagImageList.Images.SetKeyName(1, "")
        Me.FlagImageList.Images.SetKeyName(2, "")
        Me.FlagImageList.Images.SetKeyName(3, "")
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
        Me.SplitContainer1.Panel1.Controls.Add(Me.AnnotationTextBox)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.FlagImageSelector)
        Me.SplitContainer1.Panel2.Controls.Add(Me.CancelActionButton)
        Me.SplitContainer1.Panel2.Controls.Add(Me.SaveButton)
        Me.SplitContainer1.Panel2.Controls.Add(Me.AnnotationDataGrid)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label1)
        Me.SplitContainer1.Panel2.Controls.Add(Me.AddButton)
        Me.SplitContainer1.Size = New System.Drawing.Size(336, 485)
        Me.SplitContainer1.SplitterDistance = 242
        Me.SplitContainer1.TabIndex = 18
        '
        'AnnotationTextBox
        '
        Me.AnnotationTextBox.AcceptsReturn = True
        Me.AnnotationTextBox.AllowDrop = True
        Me.AnnotationTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AnnotationTextBox.Location = New System.Drawing.Point(0, 0)
        Me.AnnotationTextBox.MaxLength = 3700
        Me.AnnotationTextBox.Multiline = True
        Me.AnnotationTextBox.Name = "AnnotationTextBox"
        Me.AnnotationTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.AnnotationTextBox.Size = New System.Drawing.Size(336, 242)
        Me.AnnotationTextBox.TabIndex = 12
        '
        'FlagImageSelector
        '
        Me.FlagImageSelector.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FlagImageSelector.Controls.Add(Me.ImageItem1)
        Me.FlagImageSelector.Controls.Add(Me.ImageItem2)
        Me.FlagImageSelector.Controls.Add(Me.ImageItem3)
        Me.FlagImageSelector.Controls.Add(Me.ImageItem4)
        Me.FlagImageSelector.Controls.Add(Me.ImageItem5)
        Me.FlagImageSelector.ImageList = Me.FlagImageList
        Me.FlagImageSelector.Items.Add(Me.ImageItem1)
        Me.FlagImageSelector.Items.Add(Me.ImageItem2)
        Me.FlagImageSelector.Items.Add(Me.ImageItem3)
        Me.FlagImageSelector.Items.Add(Me.ImageItem4)
        Me.FlagImageSelector.Items.Add(Me.ImageItem5)
        Me.FlagImageSelector.Location = New System.Drawing.Point(39, 9)
        Me.FlagImageSelector.Name = "FlagImageSelector"
        Me.FlagImageSelector.Size = New System.Drawing.Size(176, 31)
        Me.FlagImageSelector.TabIndex = 29
        '
        'ImageItem1
        '
        Me.ImageItem1.BackColor = System.Drawing.SystemColors.Control
        Me.ImageItem1.Image = CType(Resources.GetObject("ImageItem1.Image"), System.Drawing.Image)
        Me.ImageItem1.ImageIndex = 0
        Me.ImageItem1.Location = New System.Drawing.Point(4, 4)
        Me.ImageItem1.Name = "ImageItem1"
        Me.ImageItem1.Owner = Me.FlagImageSelector
        Me.ImageItem1.Size = New System.Drawing.Size(24, 24)
        Me.ImageItem1.TabIndex = 0
        '
        'ImageItem2
        '
        Me.ImageItem2.BackColor = System.Drawing.SystemColors.Control
        Me.ImageItem2.Image = CType(Resources.GetObject("ImageItem2.Image"), System.Drawing.Image)
        Me.ImageItem2.ImageIndex = 1
        Me.ImageItem2.Location = New System.Drawing.Point(30, 4)
        Me.ImageItem2.Name = "ImageItem2"
        Me.ImageItem2.Owner = Me.FlagImageSelector
        Me.ImageItem2.Size = New System.Drawing.Size(24, 24)
        Me.ImageItem2.TabIndex = 1
        '
        'ImageItem3
        '
        Me.ImageItem3.BackColor = System.Drawing.SystemColors.Control
        Me.ImageItem3.Image = CType(Resources.GetObject("ImageItem3.Image"), System.Drawing.Image)
        Me.ImageItem3.ImageIndex = 2
        Me.ImageItem3.Location = New System.Drawing.Point(56, 4)
        Me.ImageItem3.Name = "ImageItem3"
        Me.ImageItem3.Owner = Me.FlagImageSelector
        Me.ImageItem3.Size = New System.Drawing.Size(24, 24)
        Me.ImageItem3.TabIndex = 2
        '
        'ImageItem4
        '
        Me.ImageItem4.BackColor = System.Drawing.SystemColors.Control
        Me.ImageItem4.Image = CType(Resources.GetObject("ImageItem4.Image"), System.Drawing.Image)
        Me.ImageItem4.ImageIndex = 3
        Me.ImageItem4.Location = New System.Drawing.Point(82, 4)
        Me.ImageItem4.Name = "ImageItem4"
        Me.ImageItem4.Owner = Me.FlagImageSelector
        Me.ImageItem4.Size = New System.Drawing.Size(24, 24)
        Me.ImageItem4.TabIndex = 3
        '
        'ImageItem5
        '
        Me.ImageItem5.BackColor = System.Drawing.SystemColors.Control
        Me.ImageItem5.ImageIndex = 4
        Me.ImageItem5.Location = New System.Drawing.Point(108, 4)
        Me.ImageItem5.Name = "ImageItem5"
        Me.ImageItem5.Owner = Me.FlagImageSelector
        Me.ImageItem5.Size = New System.Drawing.Size(24, 24)
        Me.ImageItem5.TabIndex = 4
        '
        'CancelActionButton
        '
        Me.CancelActionButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CancelActionButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelActionButton.Location = New System.Drawing.Point(6, 213)
        Me.CancelActionButton.Name = "CancelActionButton"
        Me.CancelActionButton.Size = New System.Drawing.Size(76, 23)
        Me.CancelActionButton.TabIndex = 27
        Me.CancelActionButton.Text = "&Cancel"
        '
        'SaveButton
        '
        Me.SaveButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SaveButton.Enabled = False
        Me.SaveButton.Location = New System.Drawing.Point(257, 213)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(76, 23)
        Me.SaveButton.TabIndex = 28
        Me.SaveButton.Text = "&Save"
        Me.EnhancedToolTip.SetToolTip(Me.SaveButton, "Save Annotation")
        Me.EnhancedToolTip.SetToolTipWhenDisabled(Me.SaveButton, "Enter Annotation to enable")
        '
        'AnnotationDataGrid
        '
        Me.AnnotationDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.AnnotationDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.AnnotationDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.AnnotationDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.AnnotationDataGrid.ADGroupsThatCanFind = ""
        Me.AnnotationDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.AnnotationDataGrid.ADGroupsThatCanMultiSort = ""
        Me.AnnotationDataGrid.AllowAutoSize = True
        Me.AnnotationDataGrid.AllowColumnReorder = False
        Me.AnnotationDataGrid.AllowCopy = True
        Me.AnnotationDataGrid.AllowCustomize = True
        Me.AnnotationDataGrid.AllowDelete = False
        Me.AnnotationDataGrid.AllowDragDrop = False
        Me.AnnotationDataGrid.AllowEdit = False
        Me.AnnotationDataGrid.AllowExport = True
        Me.AnnotationDataGrid.AllowFilter = True
        Me.AnnotationDataGrid.AllowFind = True
        Me.AnnotationDataGrid.AllowGoTo = True
        Me.AnnotationDataGrid.AllowMultiSelect = False
        Me.AnnotationDataGrid.AllowMultiSort = False
        Me.AnnotationDataGrid.AllowNew = False
        Me.AnnotationDataGrid.AllowPrint = True
        Me.AnnotationDataGrid.AllowRefresh = False
        Me.AnnotationDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AnnotationDataGrid.AppKey = "UFCW\Claims\"
        Me.AnnotationDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.AnnotationDataGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.AnnotationDataGrid.CaptionVisible = False
        Me.AnnotationDataGrid.ConfirmDelete = True
        Me.AnnotationDataGrid.CopySelectedOnly = True
        Me.AnnotationDataGrid.DataMember = ""
        Me.AnnotationDataGrid.ExportSelectedOnly = True
        Me.AnnotationDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.AnnotationDataGrid.LastGoToLine = ""
        Me.AnnotationDataGrid.Location = New System.Drawing.Point(3, 46)
        Me.AnnotationDataGrid.MultiSort = False
        Me.AnnotationDataGrid.Name = "AnnotationDataGrid"
        Me.AnnotationDataGrid.PreferredRowHeight = 48
        Me.AnnotationDataGrid.ReadOnly = True
        Me.AnnotationDataGrid.SetRowOnRightClick = True
        Me.AnnotationDataGrid.SingleClickBooleanColumns = True
        Me.AnnotationDataGrid.Size = New System.Drawing.Size(330, 161)
        Me.AnnotationDataGrid.StyleName = ""
        Me.AnnotationDataGrid.SubKey = ""
        Me.AnnotationDataGrid.SuppressTriangle = False
        Me.AnnotationDataGrid.TabIndex = 26
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(30, 13)
        Me.Label1.TabIndex = 25
        Me.Label1.Text = "Flag:"
        '
        'AddButton
        '
        Me.AddButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AddButton.Enabled = False
        Me.AddButton.Location = New System.Drawing.Point(257, 13)
        Me.AddButton.Name = "AddButton"
        Me.AddButton.Size = New System.Drawing.Size(76, 23)
        Me.AddButton.TabIndex = 24
        Me.AddButton.Text = "&Add"
        '
        'EnhancedToolTip
        '
        Me.EnhancedToolTip.AutomaticDelay = 50
        Me.EnhancedToolTip.ShowAlways = True
        '
        'AnnotationsControl
        '
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "AnnotationsControl"
        Me.Size = New System.Drawing.Size(336, 485)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        Me.SplitContainer1.ResumeLayout(False)
        Me.FlagImageSelector.ResumeLayout(False)
        CType(Me.AnnotationDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region
#Region " Properties "
    <System.ComponentModel.Description("Represents the application key used when accessing the registry.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
        End Set
    End Property
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
    Public Property ParticipantFirst() As Object
        Get
            Return _PartFName
        End Get
        Set(ByVal value As Object)
            _PartFName = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Participant Last Name of the Document.")>
    Public Property ParticipantLast() As Object
        Get
            Return _PartLName
        End Get
        Set(ByVal value As Object)
            _PartLName = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Patient First Name of the Document.")>
    Public Property PatientFirst() As Object
        Get
            Return _PatFName
        End Get
        Set(ByVal value As Object)
            _PatFName = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Patient Last Name of the Document.")>
    Public Property PatientLast() As Object
        Get
            Return _PatLName
        End Get
        Set(ByVal value As Object)
            _PatLName = value
        End Set
    End Property

    <DefaultValue(CBool(False)), Browsable(True), System.ComponentModel.Description("Enables Saving of Annotations.")>
    Public Property SaveAnnotations() As Boolean
        Get
            Return _SaveAnnotations
        End Get
        Set(ByVal value As Boolean)
            _SaveAnnotations = value
            SaveButton.Enabled = value

        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Patient Last Name of the Document.")> _
    Public Property Annotations() As DataTable
        Get
            Return _AnnotationsDS.ANNOTATIONS
        End Get
        Set(ByVal value As DataTable)

            If value Is Nothing Then Return

            Dim DV As New DataView(value, "", "", DataViewRowState.CurrentRows)
            If DV.Count > 0 Then
                _AnnotationsDS.ANNOTATIONS.Rows.Clear()
                For Cnt As Integer = 0 To DV.Count - 1
                    _AnnotationsDS.ANNOTATIONS.Rows.Add(DV(Cnt).Row.ItemArray)
                Next
                _AnnotationsDS.ANNOTATIONS.AcceptChanges()
            End If
        End Set
    End Property
#End Region

    Protected Overrides Sub Dispose(disposing As Boolean)

        If _IconCol IsNot Nothing Then AddHandler _IconCol.PaintCellPicture, AddressOf DetermineCellIcon

        If _AnnotationsDS IsNot Nothing Then _AnnotationsDS.Dispose()
        _AnnotationsDS = Nothing

        If AnnotationDataGrid IsNot Nothing Then AnnotationDataGrid.Dispose()
        AnnotationDataGrid = Nothing

        If AnnotationDataGrid IsNot Nothing Then
            AnnotationDataGrid.TableStyles.Clear()
        End If


        If FlagImageList IsNot Nothing Then FlagImageList.Dispose()
        FlagImageList = Nothing

        FlagImageSelector.ImageList = Nothing
        FlagImageSelector.Dispose()
        FlagImageSelector = Nothing

        If _TextCol IsNot Nothing Then _TextCol.Dispose()
        _TextCol = Nothing
        If _ColTBox IsNot Nothing Then _ColTBox.Dispose()
        _ColTBox = Nothing
        If _IconCol IsNot Nothing Then _IconCol.Dispose()
        _IconCol = Nothing

        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If

        MyBase.Dispose(disposing)

    End Sub

    Private Sub AddButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddButton.Click

        Try

            AddAnnotation()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub CancelActionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelActionButton.Click

        Try

            RaiseEvent Closing(Me, Nothing)

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub SaveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveButton.Click

        If AnnotationTextBox.Text <> "" Then
            AddAnnotation()
        End If

        RaiseEvent Save(Me, New AnnotationsEvent(Annotations))

    End Sub

    Private Sub AnnotationTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AnnotationTextBox.TextChanged

        If AnnotationTextBox.Text.ToString.Trim = "" Then
            AddButton.Enabled = False
            SaveButton.Enabled = False
        Else
            AddButton.Enabled = True
            SaveButton.Enabled = True
        End If

    End Sub

    Private Sub AddAnnotation()

        Dim Img As Image
        Dim MS As New System.IO.MemoryStream
        Dim BAs() As Byte
        Dim DR As DataRow
        Dim DGTS As DataGridTableStyle

        Try

            If FlagImageSelector.SelectedItem IsNot Nothing Then
                Img = FlagImageSelector.SelectedItem
            Else
                Img = FlagImageSelector.Items(3).Image
            End If

            Img.Save(MS, Img.RawFormat)

            BAs = MS.ToArray

            DR = _AnnotationsDS.ANNOTATIONS.NewRow

            DR("CLAIM_ID") = _ClaimID
            DR("FAMILY_ID") = _FamilyID
            DR("RELATION_ID") = _RelationID
            DR("PART_SSN") = _PartSSN
            DR("PAT_SSN") = _PatSSN
            DR("PART_FNAME") = _PartFName
            DR("PART_LNAME") = _PartLName
            DR("PAT_FNAME") = _PatFName
            DR("PAT_LNAME") = _PatLName
            DR("ANNOTATION") = AnnotationTextBox.Text.ToUpper
            DR("FLAG") = BAs
            DR("CREATE_DATE") = UFCWGeneral.NowDate
            DR("CREATE_USERID") = _DomainUser.ToUpper

            _AnnotationsDS.ANNOTATIONS.Rows.Add(DR)

            AnnotationTextBox.Text = ""
            FlagImageSelector.UnSelectAll()

            DGTS = Me.AnnotationDataGrid.GetCurrentTableStyle
            AnnotationDataGrid.SuspendLayout()
            AnnotationDataGrid.AutoSizeRowHeight(1, DGTS.GridColumnStyles(1).Width)
            AnnotationDataGrid.ResumeLayout()

            'done to resolve missing scrollbar issue
            DGTS.GridColumnStyles.Item(0).Width = DGTS.GridColumnStyles.Item(0).Width() + 1
            DGTS.GridColumnStyles.Item(0).Width = DGTS.GridColumnStyles.Item(0).Width() - 1

            SaveButton.Enabled = True

        Catch ex As Exception
            Throw
        Finally

            MS.Close()

            MS = Nothing
            BAs = Nothing
            DR = Nothing

        End Try
    End Sub

    Private Sub AnnotationDataGrid_ColumnPositionChanged(Column As DataGridColumnStyle, LastPosition As Nullable, NewPosition As Integer)
        AnnotationDataGrid.SaveColumnsSizeAndPosition(AnnotationDataGrid.Name & "\" & AnnotationDataGrid.GetCurrentTableStyle.MappingName & "\ColumnSettings")
    End Sub

    Private Sub AnnotationDataGrid_GridSortChanged(ByVal sorting As String) Handles AnnotationDataGrid.GridSortChanged
        Dim DGTS As DataGridTableStyle

        Try

            DGTS = Me.AnnotationDataGrid.GetCurrentTableStyle
            AnnotationDataGrid.SuspendLayout()
            AnnotationDataGrid.AutoSizeRowHeight(1, DGTS.GridColumnStyles(1).Width)
            AnnotationDataGrid.ResumeLayout()

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub LoadAnnotations()

        Dim DGTS As DataGridTableStyle

        Try

            AnnotationDataGrid.SuspendLayout()
            AnnotationDataGrid.DataSource = _AnnotationsDS.ANNOTATIONS
            AnnotationDataGrid.Sort = "CREATE_DATE"

            SetTableStyle(AnnotationDataGrid)

            DGTS = Me.AnnotationDataGrid.GetCurrentTableStyle

            AnnotationDataGrid.AutoSizeRowHeight(1, DGTS.GridColumnStyles(1).Width)
            AnnotationDataGrid.ResumeLayout()

            'done to resolve missing scrollbar issue

            DGTS.GridColumnStyles.Item(0).Width = DGTS.GridColumnStyles.Item(0).Width() + 1
            DGTS.GridColumnStyles.Item(0).Width = DGTS.GridColumnStyles.Item(0).Width() - 1

        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub SetTableStyle(ByVal dg As DataGridCustom) 'called when no context menu is in use

        Try

            SetTableStyle(dg, Nothing)

        Catch ex As Exception
            Throw
        End Try

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
        Dim DGTSDefault As DataGridTableStyle

        Dim TextCol As DataGridFormattableTextBoxColumn
        Dim BoolCol As DataGridColorBoolColumn
        Dim IconCol As DataGridHighlightIconColumn
        Dim MultiLineTextCol As DataGridFormattableMultiLineTextBoxColumn

        Dim ColsDV As DataView
        Dim DefaultStyleDS As DataSet
        Dim XMLStyleName As String
        Dim ResultDT As DataTable
        Dim ColumnSequenceDV As DataView

        Try

            XMLStyleName = dg.Name

            DefaultStyleDS = DataGridCustom.GetTableStyle(XMLStyleName)

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

            DGTSDefault = New DataGridTableStyle With {
                .MappingName = "Default"
            } 'This can be used to establish the columns displayed by default

            For IntCol As Integer = 0 To ColsDV.Count - 1

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
                    If ColsDV(IntCol).Item("Type").ToString = "Text" And Not IsDBNull(ColsDV(IntCol).Item("WordWrap")) AndAlso ColsDV(IntCol).Item("WordWrap").ToString.Trim.Length > 0 AndAlso CBool(ColsDV(IntCol).Item("WordWrap")) = True Then
                        MultiLineTextCol = New DataGridFormattableMultiLineTextBoxColumn With {
                            .MappingName = CStr(ColsDV(IntCol).Item("Mapping")),
                            .HeaderText = CStr(ColsDV(IntCol).Item("HeaderText")),
                            .Width = CInt(ColsDV(IntCol).Item("SizeInPixels"))
                        }
                        If Not IsDBNull(ColsDV(IntCol).Item("MinimumCharWidth")) AndAlso ColsDV(IntCol).Item("MinimumCharWidth").ToString.Trim.Length > 0 Then
                            MultiLineTextCol.MinimumCharWidth = CInt(ColsDV(IntCol).Item("MinimumCharWidth"))
                        End If
                        If Not IsDBNull(ColsDV(IntCol).Item("MaximumCharWidth")) AndAlso ColsDV(IntCol).Item("MaximumCharWidth").ToString.Trim.Length > 0 Then
                            MultiLineTextCol.MinimumCharWidth = CInt(ColsDV(IntCol).Item("MaximumCharWidth"))
                        End If

                        MultiLineTextCol.NullText = CStr(ColsDV(IntCol).Item("NullText"))

                        If Not IsDBNull(ColsDV(IntCol).Item("WordWrap")) AndAlso ColsDV(IntCol).Item("WordWrap").ToString.Trim.Length > 0 AndAlso CBool(ColsDV(IntCol).Item("WordWrap")) = True Then
                            MultiLineTextCol.TextBox.WordWrap = CBool(ColsDV(IntCol).Item("WordWrap"))
                        End If

                        If Not IsDBNull(ColsDV(IntCol).Item("ReadOnly")) AndAlso ColsDV(IntCol).Item("ReadOnly").ToString.Trim.Length > 0 AndAlso CBool(ColsDV(IntCol).Item("ReadOnly")) = True Then
                            MultiLineTextCol.ReadOnly = True
                        End If

                        If IsDBNull(ColsDV(IntCol).Item("Format")) = False AndAlso ColsDV(IntCol).Item("Format").ToString.Trim.Length > 0 Then
                            Select Case ColsDV(IntCol).Item("Format").ToString.ToUpper
                                Case "YESNO", "DECIMALBOOLEAN2YESNO"
                                    AddHandler MultiLineTextCol.Formatting, AddressOf DataGridCustom.FormattingYesNo
                                Case "1STCHARACTERMASK"
                                    AddHandler MultiLineTextCol.Formatting, AddressOf DataGridCustom.Formatting1STCHARACTERMASK
                                Case "LOWER"
                                    AddHandler MultiLineTextCol.Formatting, AddressOf DataGridCustom.FormattingLOWER
                                Case "UPPER"
                                    AddHandler MultiLineTextCol.Formatting, AddressOf DataGridCustom.FormattingUPPER
                                Case Else
                                    MultiLineTextCol.Format = CStr(ColsDV(IntCol).Item("Format"))
                                    Select Case MultiLineTextCol.Format.ToUpper
                                        Case "C"
                                            AddHandler MultiLineTextCol.UnFormatting, AddressOf DataGridCustom.UnFormattingCurrency
                                    End Select
                            End Select

                        End If

                        If IsDBNull(ColsDV(IntCol).Item("FormatIsRegEx")) = False AndAlso ColsDV(IntCol).Item("FormatIsRegEx").ToString.Trim.Length > 0 Then
                            MultiLineTextCol.FormatIsRegEx = CBool(ColsDV(IntCol).Item("FormatIsRegEx"))
                        End If

                        DGTS.GridColumnStyles.Add(MultiLineTextCol)

                    ElseIf ColsDV(IntCol).Item("Type").ToString = "Text" Then
                        TextCol = New DataGridFormattableTextBoxColumn With {
                            .MappingName = CStr(ColsDV(IntCol).Item("Mapping")),
                            .HeaderText = CStr(ColsDV(IntCol).Item("HeaderText")),
                            .Width = CInt(ColsDV(IntCol).Item("SizeInPixels")),
                            .NullText = CStr(ColsDV(IntCol).Item("NullText"))
                        }
                        TextCol.TextBox.WordWrap = True
                        TextCol.TextBox.Multiline = True

                        Try

                            If CBool(ColsDV(IntCol).Item("ReadOnly")) Then
                                TextCol.ReadOnly = True
                            End If

                        Catch ex As Exception
                        End Try

                        If IsDBNull(ColsDV(IntCol).Item("Format")) = False AndAlso ColsDV(IntCol).Item("Format").ToString.Trim.Length > 0 Then
                            Select Case ColsDV(IntCol).Item("Format").ToString.ToUpper
                                Case "YESNO", "DECIMALBOOLEAN2YESNO"
                                    AddHandler TextCol.Formatting, AddressOf DataGridCustom.FormattingYesNo
                                Case "1STCHARACTERMASK"
                                    AddHandler TextCol.Formatting, AddressOf DataGridCustom.Formatting1STCHARACTERMASK
                                Case "LOWER"
                                    AddHandler TextCol.Formatting, AddressOf DataGridCustom.FormattingLOWER
                                Case "UPPER"
                                    AddHandler TextCol.Formatting, AddressOf DataGridCustom.FormattingUPPER
                                Case Else
                                    TextCol.Format = CStr(ColsDV(IntCol).Item("Format"))
                                    Select Case TextCol.Format.ToUpper
                                        Case "C"
                                            AddHandler TextCol.UnFormatting, AddressOf DataGridCustom.UnFormattingCurrency
                                    End Select
                            End Select

                        End If

                        DGTS.GridColumnStyles.Add(TextCol)

                    ElseIf ColsDV(IntCol).Item("Type").ToString.Contains("Bool") Then
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

                        IconCol = New DataGridHighlightIconColumn(AnnotationDataGrid, FlagImageList) With {
                            .MappingName = CStr(ColsDV(IntCol).Item("Mapping")),
                            .HeaderText = ColsDV(IntCol).Item("HeaderText").ToString,
                            .Width = CInt(ColsDV(IntCol).Item("SizeInPixels")),
                            .NullText = ColsDV(IntCol).Item("NullText").ToString,
                            .MaximumCharWidth = CInt(ColsDV(IntCol).Item("MaximumCharWidth")),
                            .MinimumCharWidth = CInt(ColsDV(IntCol).Item("MinimumCharWidth"))
                        }

                        AddHandler IconCol.PaintCellPicture, AddressOf DetermineCellIcon
                        DGTS.GridColumnStyles.Add(IconCol)
                    End If
                End If
            Next

            dg.TableStyles.Clear()
            dg.TableStyles.Add(DGTS)
            dg.TableStyles.Add(DGTSDefault)

            'done to resolve missing scrollbar issue
            DGTS.GridColumnStyles.Item(0).Width = DGTS.GridColumnStyles.Item(0).Width() + 1
            DGTS.GridColumnStyles.Item(0).Width = DGTS.GridColumnStyles.Item(0).Width() - 1

        Catch ex As Exception

            Throw

        Finally
            DGTS = Nothing
        End Try

    End Sub

    Private Sub DetermineCellIcon(ByRef pic As Image, ByVal cell As System.Windows.Forms.DataGridCell)
        Dim MS As System.IO.MemoryStream
        Dim BAs() As Byte
        Dim DV As DataView

        Try
            DV = AnnotationDataGrid.GetDefaultDataView
            If DV IsNot Nothing AndAlso IsDBNull(DV(cell.RowNumber)("FLAG")) = False Then

                BAs = CType(DV(cell.RowNumber)("FLAG"), Byte())

                MS = New System.IO.MemoryStream(BAs, 0, BAs.Length)
                MS.Write(BAs, 0, BAs.Length)

                pic = Image.FromStream(MS, True)
            Else
                pic = FlagImageList.Images(0)
            End If

        Catch ex As Exception

            Throw

        Finally
            MS = Nothing
            BAs = Nothing
        End Try
    End Sub

    Private Sub AnnotationsControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try

            If Not DesignMode Then
                LoadAnnotations()
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub AnnotationDataGrid_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles AnnotationDataGrid.MouseMove

        Dim HTI As DataGrid.HitTestInfo
        Dim DV As DataView

        Try
            HTI = CType(sender, DataGridCustom).HitTest(e.X, e.Y)

            ' Do not display hover text if it is a drag event
            If (e.Button <> MouseButtons.Left) Then

                ' Check if the target is a different cell from the previous one
                If HTI.Type = DataGrid.HitTestType.Cell AndAlso HTI.Row <> _HoverCell.RowNumber OrElse HTI.Column <> _HoverCell.ColumnNumber Then

                    ' Store the new hit row
                    _HoverCell.RowNumber = HTI.Row
                    _HoverCell.ColumnNumber = HTI.Column

                End If

            End If

            If _HoverCell.RowNumber > -1 AndAlso _HoverCell.RowNumber <= (CType(sender, DataGridCustom).GetGridRowCount) Then
                If _HoverCell.ColumnNumber = 1 Then
                    DV = CType(sender, DataGridCustom).GetDefaultDataView
                    If DV IsNot Nothing AndAlso Not IsDBNull(DV(_HoverCell.RowNumber)("ANNOTATION")) Then
                        EnhancedToolTip.Active = True
                        EnhancedToolTip.SetToolTip(CType(sender, DataGridCustom), DV(_HoverCell.RowNumber)("ANNOTATION").ToString)
                    Else
                        EnhancedToolTip.Active = False
                        EnhancedToolTip.SetToolTip(CType(sender, DataGridCustom), "")

                    End If
                Else
                    EnhancedToolTip.Active = False
                    EnhancedToolTip.SetToolTip(CType(sender, DataGridCustom), "")

                End If
            Else
                EnhancedToolTip.Active = False
                EnhancedToolTip.SetToolTip(CType(sender, DataGridCustom), "")

            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

End Class
Public Class AnnotationsEvent
    Inherits Global.System.EventArgs

    Private _AnnotationsDT As DataTable
    Public ReadOnly Property AnnotationsTable As DataTable
        Get
            Return _AnnotationsDT
        End Get
    End Property
    Public Sub New(ByVal annotationsDT As DataTable)
        MyBase.New()
        _AnnotationsDT = annotationsDT
    End Sub

End Class
