Imports System.ComponentModel
Imports System.Security.Principal

Public Class ProviderSearchControl
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _ProvTIN As Integer? = Nothing
    Private _ProvID As Integer? = Nothing
    Private _ProvName As String
    Private _ProvBS As BindingSource

    Public Event ActionCancel(ByVal sender As Object)
    Public Event ActionSelect(ByVal sender As Object, ByVal PatientRow As DataRow)

    Private _WindowsUserID As WindowsIdentity = WindowsIdentity.GetCurrent()
    Private _WindowsPrincipalForID As WindowsPrincipal = New WindowsPrincipal(_WindowsUserID)

    Private _APPKEY As String = "UFCW\ProviderSearch\"
    Private _Loading As Boolean = True

    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents ClearButton As System.Windows.Forms.Button
    Public WithEvents ActionButton As System.Windows.Forms.Button
    Friend WithEvents CancelFormButton As System.Windows.Forms.Button
    Friend WithEvents NPITextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ProvNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ProvTINTextBox As System.Windows.Forms.RichTextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents ProviderSearchDataGrid As DataGridCustom
    Friend WithEvents ProvDS As ProvDS

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

        ProviderSearchDataGrid.DataSource = Nothing
        Me.ProviderSearchDataGrid.Dispose()

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
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ClearButton = New System.Windows.Forms.Button()
        Me.ActionButton = New System.Windows.Forms.Button()
        Me.CancelFormButton = New System.Windows.Forms.Button()
        Me.NPITextBox = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ProvNameTextBox = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ProvTINTextBox = New System.Windows.Forms.RichTextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ProviderSearchDataGrid = New DataGridCustom()
        Me.ProvDS = New ProvDS()
        CType(Me.ProviderSearchDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ProvDS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ClearButton
        '
        Me.ClearButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ClearButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ClearButton.Location = New System.Drawing.Point(535, 438)
        Me.ClearButton.Name = "ClearButton"
        Me.ClearButton.Size = New System.Drawing.Size(74, 23)
        Me.ClearButton.TabIndex = 4
        Me.ClearButton.Text = "Clear"
        '
        'ActionButton
        '
        Me.ActionButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ActionButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ActionButton.Location = New System.Drawing.Point(440, 438)
        Me.ActionButton.Name = "ActionButton"
        Me.ActionButton.Size = New System.Drawing.Size(74, 23)
        Me.ActionButton.TabIndex = 3
        Me.ActionButton.Text = "Search"
        '
        'CancelFormButton
        '
        Me.CancelFormButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CancelFormButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CancelFormButton.Location = New System.Drawing.Point(630, 438)
        Me.CancelFormButton.Name = "CancelFormButton"
        Me.CancelFormButton.Size = New System.Drawing.Size(74, 23)
        Me.CancelFormButton.TabIndex = 5
        Me.CancelFormButton.Text = "Cancel"
        '
        'NPITextBox
        '
        Me.NPITextBox.Location = New System.Drawing.Point(417, 7)
        Me.NPITextBox.MaxLength = 10
        Me.NPITextBox.Name = "NPITextBox"
        Me.NPITextBox.Size = New System.Drawing.Size(96, 20)
        Me.NPITextBox.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(379, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(32, 16)
        Me.Label3.TabIndex = 31
        Me.Label3.Text = "NPI:"
        '
        'ProvNameTextBox
        '
        Me.ProvNameTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ProvNameTextBox.Location = New System.Drawing.Point(267, 7)
        Me.ProvNameTextBox.MaxLength = 40
        Me.ProvNameTextBox.Name = "ProvNameTextBox"
        Me.ProvNameTextBox.Size = New System.Drawing.Size(104, 20)
        Me.ProvNameTextBox.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(187, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(80, 13)
        Me.Label2.TabIndex = 30
        Me.Label2.Text = "Provider Name:"
        '
        'ProvTINTextBox
        '
        Me.ProvTINTextBox.DetectUrls = False
        Me.ProvTINTextBox.Location = New System.Drawing.Point(75, 7)
        Me.ProvTINTextBox.MaxLength = 9
        Me.ProvTINTextBox.Multiline = False
        Me.ProvTINTextBox.Name = "ProvTINTextBox"
        Me.ProvTINTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None
        Me.ProvTINTextBox.Size = New System.Drawing.Size(104, 20)
        Me.ProvTINTextBox.TabIndex = 0
        Me.ProvTINTextBox.Text = ""
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 13)
        Me.Label1.TabIndex = 28
        Me.Label1.Text = "Provider TIN:"
        '
        'ProviderSearchDataGrid
        '
        Me.ProviderSearchDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.ProviderSearchDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.ProviderSearchDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ProviderSearchDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ProviderSearchDataGrid.ADGroupsThatCanFind = ""
        Me.ProviderSearchDataGrid.ADGroupsThatCanMultiSort = ""
        Me.ProviderSearchDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ProviderSearchDataGrid.AllowAutoSize = True
        Me.ProviderSearchDataGrid.AllowColumnReorder = True
        Me.ProviderSearchDataGrid.AllowCopy = True
        Me.ProviderSearchDataGrid.AllowCustomize = True
        Me.ProviderSearchDataGrid.AllowDelete = False
        Me.ProviderSearchDataGrid.AllowDragDrop = False
        Me.ProviderSearchDataGrid.AllowEdit = False
        Me.ProviderSearchDataGrid.AllowExport = True
        Me.ProviderSearchDataGrid.AllowFilter = False
        Me.ProviderSearchDataGrid.AllowFind = True
        Me.ProviderSearchDataGrid.AllowGoTo = True
        Me.ProviderSearchDataGrid.AllowMultiSelect = False
        Me.ProviderSearchDataGrid.AllowMultiSort = False
        Me.ProviderSearchDataGrid.AllowNew = False
        Me.ProviderSearchDataGrid.AllowPrint = False
        Me.ProviderSearchDataGrid.AllowRefresh = False
        Me.ProviderSearchDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProviderSearchDataGrid.AppKey = "UFCW\Provider\"
        Me.ProviderSearchDataGrid.AutoSaveCols = True
        Me.ProviderSearchDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.ProviderSearchDataGrid.ColumnHeaderLabel = Nothing
        Me.ProviderSearchDataGrid.ColumnRePositioning = False
        Me.ProviderSearchDataGrid.ColumnResizing = False
        Me.ProviderSearchDataGrid.ConfirmDelete = True
        Me.ProviderSearchDataGrid.CopySelectedOnly = True
        Me.ProviderSearchDataGrid.CurrentBSPosition = -1
        Me.ProviderSearchDataGrid.DataMember = ""
        Me.ProviderSearchDataGrid.DragColumn = 0
        Me.ProviderSearchDataGrid.ExportSelectedOnly = True
        Me.ProviderSearchDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ProviderSearchDataGrid.HighlightedRow = Nothing
        Me.ProviderSearchDataGrid.HighLightModifiedRows = False
        Me.ProviderSearchDataGrid.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.ProviderSearchDataGrid.IsMouseDown = False
        Me.ProviderSearchDataGrid.LastGoToLine = ""
        Me.ProviderSearchDataGrid.Location = New System.Drawing.Point(3, 35)
        Me.ProviderSearchDataGrid.MultiSort = False
        Me.ProviderSearchDataGrid.Name = "ProviderSearchDataGrid"
        Me.ProviderSearchDataGrid.OldSelectedRow = Nothing
        Me.ProviderSearchDataGrid.PreviousBSPosition = -1
        Me.ProviderSearchDataGrid.ReadOnly = True
        Me.ProviderSearchDataGrid.RetainRowSelectionAfterSort = True
        Me.ProviderSearchDataGrid.SetRowOnRightClick = True
        Me.ProviderSearchDataGrid.ShiftPressed = False
        Me.ProviderSearchDataGrid.SingleClickBooleanColumns = True
        Me.ProviderSearchDataGrid.Size = New System.Drawing.Size(701, 395)
        Me.ProviderSearchDataGrid.Sort = Nothing
        Me.ProviderSearchDataGrid.StyleName = ""
        Me.ProviderSearchDataGrid.SubKey = ""
        Me.ProviderSearchDataGrid.SuppressMouseDown = False
        Me.ProviderSearchDataGrid.SuppressTriangle = False
        Me.ProviderSearchDataGrid.TabIndex = 6
        '
        'ProvDS
        '
        Me.ProvDS.DataSetName = "ProvDS"
        Me.ProvDS.EnforceConstraints = False
        Me.ProvDS.Locale = New System.Globalization.CultureInfo("en-US")
        Me.ProvDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'ProviderSearchControl
        '
        Me.Controls.Add(Me.NPITextBox)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ProvNameTextBox)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ProvTINTextBox)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ProviderSearchDataGrid)
        Me.Controls.Add(Me.ClearButton)
        Me.Controls.Add(Me.ActionButton)
        Me.Controls.Add(Me.CancelFormButton)
        Me.Name = "ProviderSearchControl"
        Me.Size = New System.Drawing.Size(710, 471)
        CType(Me.ProviderSearchDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ProvDS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region "Public Properties"

    <Browsable(True), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
        End Set
    End Property


    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets the selected TaxID of the selected Provider.")>
    Public ReadOnly Property ProvTIN() As Integer?
        Get
            Return _ProvTIN
        End Get
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets the ProviderID of the selected Provider.")>
    Public ReadOnly Property ProviderID() As Integer?
        Get
            Return _ProvID
        End Get
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets the Provider Name selected.")>
    Public ReadOnly Property ProviderName() As String
        Get
            Return _ProvName
        End Get
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal appKey As String)
        Me.New()

        _APPKEY = appKey

    End Sub

#End Region

#Region "Form Events"

    Private Sub ProviderSearchControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If _ProvName IsNot Nothing AndAlso _ProvName.ToString.Trim.Length > 0 Then
                Me.ProvNameTextBox.Text = _ProvName.ToString.Trim
            End If

            If _ProvTIN.ToString.Trim.Length > 0 AndAlso _ProvTIN > 0 Then
                Me.ProvTINTextBox.Text = _ProvTIN.ToString.Trim
            End If

        Catch ex As Exception
            Throw
        Finally
            _Loading = False
        End Try

    End Sub

    Private Sub CancelFormButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelFormButton.Click

        _ProvTIN = Nothing
        _ProvID = Nothing
        _ProvName = Nothing

        RaiseEvent ActionCancel(Me)

    End Sub

    Private Sub ActionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ActionButton.Click
        Try

            If Me.ActionButton.Text = "Search" Then

                If ProvTINTextBox.Text.Trim.Length > 0 OrElse ProvNameTextBox.Text.Trim.Length > 0 OrElse NPITextBox.Text.Trim.Length > 0 Then
                    Search()
                Else
                    MsgBox("No search criteria specified.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Missing Search criteria")
                End If

            Else 'Button text is Select
                SelectRow()
            End If

        Catch ex As Exception
            Throw
        Finally

        End Try
    End Sub

    Private Sub ProviderLookUp_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ProviderSearchDataGrid.TableStyles.Clear() 'don't display default style

    End Sub

    Private Sub PersonDataGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ProviderSearchDataGrid.MouseDoubleClick

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
        Dim DR As DataRow

        Try
            If _ProvBS Is Nothing OrElse _ProvBS.Position < 0 OrElse _ProvBS.Current Is Nothing Then Return

            DR = CType(_ProvBS.Current, DataRowView).Row

            _ProvName = CStr(DR("NAME1"))
            _ProvTIN = CInt(DR("TAXID"))
                _ProvID = CInt(DR("PROVIDER_ID"))

                Me.ProvTINTextBox.Text = String.Format("000000000", DR("TAXID"))
                Me.ProvNameTextBox.Text = CStr(DR("NAME1"))

                RaiseEvent ActionSelect(Me, DR)

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub ProviderDataGrid_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ProviderSearchDataGrid.MouseDown

        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo

        Try
            HTI = CType(sender, DataGridCustom).HitTest(e.X, e.Y)

            If e.Button = MouseButtons.Left Then
                Select Case HTI.Type
                    Case Is = DataGrid.HitTestType.Cell, DataGrid.HitTestType.RowHeader
                        CType(sender, DataGridCustom).SuppressTriangle = False
                End Select

            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub ProviderDataGrid_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ProviderSearchDataGrid.MouseUp

        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo

        Try
            HTI = CType(sender, DataGridCustom).HitTest(e.X, e.Y)

            If e.Button = MouseButtons.Left Then
                Select Case HTI.Type
                    Case Is = DataGrid.HitTestType.Cell, DataGrid.HitTestType.RowHeader
                        CType(sender, DataGridCustom).Select(HTI.Row)
                        Me.ActionButton.Text = "Select"
                End Select

            End If
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub ProvNameTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProvNameTextBox.TextChanged
        Me.ActionButton.Text = "Search"
    End Sub

    Private Sub ProvTINTextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles ProvTINTextBox.KeyDown

        Dim strBuffer As String
        Dim stuff As IDataObject = Clipboard.GetDataObject()

        If e.Control OrElse e.Shift Then
            If e.KeyCode = Keys.V OrElse e.KeyCode = Keys.Insert Then
                If (e.Control AndAlso e.KeyCode = Keys.V) OrElse (e.Shift AndAlso e.KeyCode = Keys.Insert) Then
                    e.Handled = True
                    strBuffer = CType(stuff.GetData(DataFormats.Text), String)
                    For IntCnt As Integer = 1 To Len(strBuffer)
                        If Not IsNumeric(Mid(strBuffer, IntCnt, 1)) AndAlso Len(strBuffer) > 0 Then
                            strBuffer = Replace(strBuffer, Mid(strBuffer, IntCnt, 1), "")
                        End If
                    Next

                    ProvTINTextBox.Text = strBuffer
                End If
            End If
        End If
    End Sub

    Private Sub ProvTINBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProvTINTextBox.TextChanged

        Dim TBox As RichTextBox = CType(sender, RichTextBox)
        Dim IntCnt As Integer
        Dim StrTmp As String

        If Not IsNumeric(TBox.Text) AndAlso Len(TBox.Text) > 0 Then
            StrTmp = TBox.Text
            For IntCnt = 1 To Len(StrTmp)
                If Not IsNumeric(Mid(StrTmp, IntCnt, 1)) AndAlso Len(StrTmp) > 0 Then
                    StrTmp = Replace(StrTmp, Mid(StrTmp, IntCnt, 1), "")
                End If
            Next
            TBox.Text = StrTmp

        End If

        Me.ActionButton.Text = "Search"

    End Sub

    Private Sub NPITextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NPITextBox.TextChanged

        Dim TBox As TextBox = CType(sender, TextBox)
        Dim IntCnt As Integer
        Dim StrTmp As String

        If Not IsNumeric(TBox.Text) AndAlso Len(TBox.Text) > 0 Then
            StrTmp = TBox.Text
            For IntCnt = 1 To Len(StrTmp)
                If Not IsNumeric(Mid(StrTmp, IntCnt, 1)) AndAlso Len(StrTmp) > 0 Then
                    StrTmp = Replace(StrTmp, Mid(StrTmp, IntCnt, 1), "")
                End If
            Next
            TBox.Text = StrTmp

        End If

        Me.ActionButton.Text = "Search"

    End Sub

    Private Sub ClearButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearButton.Click

        Me.ProvTINTextBox.Text = ""
        Me.ProvNameTextBox.Text = ""

        ProvDS.PROVIDER.Rows.Clear()
        ProviderSearchDataGrid.CaptionText = ""

    End Sub

    Private Sub Search()
        Try


            ProviderSearchDataGrid.CaptionText = "Searching..."
            ProviderSearchDataGrid.Refresh()
            ProviderSearchDataGrid.SuspendLayout()

            ProvDS.PROVIDER.Rows.Clear()

            ProvDS = CType(CMSDALFDBMD.RetrieveProviders(If(IsNumeric(ProvTINTextBox.Text.Replace("-", "")), CType(ProvTINTextBox.Text.Replace("-", ""), Integer?), Nothing), TryCast(ProvNameTextBox.Text, String), If(IsNumeric(NPITextBox.Text.Trim), CType(NPITextBox.Text.Trim, Decimal?), Nothing), ProvDS), ProvDS)

            ProviderSearchDataGrid.SuppressTriangle = True
            _ProvBS = New BindingSource
            _ProvBS.DataSource = ProvDS.Tables("PROVIDER")

            ProviderSearchDataGrid.DataSource = _ProvBS
            ProviderSearchDataGrid.Sort = If(ProviderSearchDataGrid.LastSortedBy, ProviderSearchDataGrid.DefaultSort)

            ProviderSearchDataGrid.SetTableStyle()
            ProviderSearchDataGrid.ResumeLayout()

            ProviderSearchDataGrid.CaptionText = "Showing " & ProvDS.PROVIDER.Rows.Count & " Provider" & If(ProvDS.PROVIDER.Rows.Count = 1, "", "s")

        Catch ex As Exception

            Throw

        Finally

        End Try
    End Sub

#End Region

End Class
