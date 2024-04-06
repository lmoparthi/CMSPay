Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class ProviderLookup
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _ProvTIN As Integer
    Private _ProvName As String
    Private _APPKEY As String = "UFCW\Claims\"

#Region " Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents SearchButton As System.Windows.Forms.Button
    Friend WithEvents ProvTINTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ProvNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ProvDataSet As ProvDataSet
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents NPITextBox As System.Windows.Forms.TextBox
    Public WithEvents ProviderDataGrid As DataGridCustom
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.ProviderDataGrid = New DataGridCustom
        Me.Label1 = New System.Windows.Forms.Label
        Me.ProvTINTextBox = New System.Windows.Forms.TextBox
        Me.ProvNameTextBox = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.SearchButton = New System.Windows.Forms.Button
        Me.ProvDataSet = New ProvDataSet
        Me.Label3 = New System.Windows.Forms.Label
        Me.NPITextBox = New System.Windows.Forms.TextBox
        CType(Me.ProviderDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ProvDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ProviderDataGrid
        '
        Me.ProviderDataGrid.AllowColumnReorder = False
        Me.ProviderDataGrid.AllowCopy = False
        Me.ProviderDataGrid.AllowDelete = False
        Me.ProviderDataGrid.AllowDragDrop = False
        Me.ProviderDataGrid.AllowEdit = False
        Me.ProviderDataGrid.AllowExport = False
        Me.ProviderDataGrid.AllowFind = True
        Me.ProviderDataGrid.AllowGoTo = True
        Me.ProviderDataGrid.AllowMultiSelect = False
        Me.ProviderDataGrid.AllowMultiSort = False
        Me.ProviderDataGrid.AllowNew = False
        Me.ProviderDataGrid.AllowPrint = False
        Me.ProviderDataGrid.AllowRefresh = False
        Me.ProviderDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProviderDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.ProviderDataGrid.ConfirmDelete = True
        Me.ProviderDataGrid.CopySelectedOnly = True
        Me.ProviderDataGrid.DataMember = ""
        Me.ProviderDataGrid.ExportSelectedOnly = True
        Me.ProviderDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ProviderDataGrid.LastGoToLine = ""
        Me.ProviderDataGrid.Location = New System.Drawing.Point(8, 32)
        Me.ProviderDataGrid.MultiSort = False
        Me.ProviderDataGrid.Name = "ProviderDataGrid"
        Me.ProviderDataGrid.ReadOnly = True
        Me.ProviderDataGrid.SetRowOnRightClick = True
        Me.ProviderDataGrid.SingleClickBooleanColumns = True
        Me.ProviderDataGrid.Size = New System.Drawing.Size(576, 208)
        Me.ProviderDataGrid.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 16)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Provider TIN:"
        '
        'ProvTINTextBox
        '
        Me.ProvTINTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ProvTINTextBox.Location = New System.Drawing.Point(80, 6)
        Me.ProvTINTextBox.MaxLength = 40
        Me.ProvTINTextBox.Name = "ProvTINTextBox"
        Me.ProvTINTextBox.Size = New System.Drawing.Size(104, 20)
        Me.ProvTINTextBox.TabIndex = 0
        Me.ProvTINTextBox.Text = ""
        '
        'ProvNameTextBox
        '
        Me.ProvNameTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ProvNameTextBox.Location = New System.Drawing.Point(272, 6)
        Me.ProvNameTextBox.MaxLength = 40
        Me.ProvNameTextBox.Name = "ProvNameTextBox"
        Me.ProvNameTextBox.Size = New System.Drawing.Size(104, 20)
        Me.ProvNameTextBox.TabIndex = 1
        Me.ProvNameTextBox.Text = ""
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(192, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(83, 16)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Provider Name:"
        '
        'SearchButton
        '
        Me.SearchButton.Enabled = False
        Me.SearchButton.Location = New System.Drawing.Point(512, 5)
        Me.SearchButton.Name = "SearchButton"
        Me.SearchButton.TabIndex = 3
        Me.SearchButton.Text = "&Search"
        '
        'ProvDataSet
        '
        Me.ProvDataSet.DataSetName = "ProvDataSet"
        Me.ProvDataSet.Locale = New System.Globalization.CultureInfo("en-US")
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(384, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(32, 16)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "NPI:"
        '
        'NPITextBox
        '
        Me.NPITextBox.Location = New System.Drawing.Point(408, 6)
        Me.NPITextBox.MaxLength = 10
        Me.NPITextBox.Name = "NPITextBox"
        Me.NPITextBox.Size = New System.Drawing.Size(96, 20)
        Me.NPITextBox.TabIndex = 2
        Me.NPITextBox.Text = ""
        '
        'ProviderLookup
        '
        Me.AcceptButton = Me.SearchButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(592, 246)
        Me.Controls.Add(Me.NPITextBox)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.SearchButton)
        Me.Controls.Add(Me.ProvNameTextBox)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ProvTINTextBox)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ProviderDataGrid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MinimumSize = New System.Drawing.Size(600, 192)
        Me.Name = "ProviderLookup"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Select Provider:"
        CType(Me.ProviderDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ProvDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Constructors"
    Sub New(ByVal ProvTIN As Integer)
        Me.New()

        _ProvTIN = ProvTIN

        ProvTINTextBox.Text = CStr(_ProvTIN)
    End Sub

    Sub New(ByVal ProvName As String)
        Me.New()

        _ProvName = ProvName

        ProvNameTextBox.Text = _ProvName
    End Sub

    Sub New(ByVal ProvTIN As Integer, ByVal ProvName As String)
        Me.New()

        _ProvTIN = ProvTIN
        _ProvName = ProvName

        ProvTINTextBox.Text = CStr(_ProvTIN)
        ProvNameTextBox.Text = _ProvName
    End Sub
#End Region

#Region "Public Properties"
    <System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)

            _APPKEY = value
        End Set
    End Property

    Public ReadOnly Property ProviderTable() As DataTable
        Get
            Return ProvDataSet.PROVIDER
        End Get
    End Property
#End Region

    Private Sub ProviderLookup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            SetSettings()
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub ProviderLookup_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        SaveSettings()
    End Sub

    Private Sub SetSettings()
        Me.Top = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)))
        Me.Height = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)))
        Me.Width = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)
    End Sub

    Private Sub SaveSettings()
        Dim WindowState As FormWindowState = Me.WindowState
        SaveSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(WindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = WindowState

    End Sub

    Private Sub SetTableStyle()
        Dim DGTableStyle As DataGridTableStyle
        Dim TextCol As DataGridHighlightTextBoxColumn
        Dim TextBoxCol As DataGridTextBox
        Dim CurMan As CurrencyManager

        Try

            CurMan = CType(Me.BindingContext(ProvDataSet.PROVIDER), CurrencyManager)

            DGTableStyle = New DataGridTableStyle(CurMan)
            DGTableStyle.MappingName = ProvDataSet.PROVIDER.TableName
            DGTableStyle.GridColumnStyles.Clear()
            DGTableStyle.GridLineStyle = DataGridLineStyle.None

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "PROVIDER_ID"
            TextCol.HeaderText = "Prov. ID"
            TextCol.Width = CInt(GetSetting(_APPKEY, "ProviderLookup\ColumnSettings", "Col " & TextCol.MappingName, "80"))
            TextCol.NullText = ""
            TextBoxCol = CType(TextCol.TextBox, DataGridTextBox)
            'AddHandler coltxtbx.KeyDown, AddressOf GridKeyDownEvent
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "NPI"
            TextCol.HeaderText = "NPI"
            TextCol.Width = CInt(GetSetting(_APPKEY, "ProviderLookup\ColumnSettings", "Col " & TextCol.MappingName, "65"))
            TextCol.NullText = ""
            TextCol.Format = ""
            TextBoxCol = CType(TextCol.TextBox, DataGridTextBox)
            'AddHandler coltxtbx.KeyDown, AddressOf GridKeyDownEvent
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "TAXID"
            TextCol.HeaderText = "TIN"
            TextCol.Width = CInt(GetSetting(_APPKEY, "ProviderLookup\ColumnSettings", "Col " & TextCol.MappingName, "65"))
            TextCol.NullText = ""
            TextCol.Format = "00-0000000"
            TextBoxCol = CType(TextCol.TextBox, DataGridTextBox)
            'AddHandler coltxtbx.KeyDown, AddressOf GridKeyDownEvent
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "NAME1"
            TextCol.HeaderText = "Provider Name"
            TextCol.Width = CInt(GetSetting(_APPKEY, "ProviderLookup\ColumnSettings", "Col " & TextCol.MappingName, "100"))
            TextCol.NullText = ""
            TextBoxCol = CType(TextCol.TextBox, DataGridTextBox)
            'AddHandler coltxtbx.KeyDown, AddressOf GridKeyDownEvent
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "SUSPEND_SW"
            TextCol.HeaderText = "Provider Suspended"
            TextCol.Width = CInt(GetSetting(_APPKEY, "ProviderLookup\ColumnSettings", "Col " & TextCol.MappingName, "100"))
            TextCol.NullText = ""
            TextBoxCol = CType(TextCol.TextBox, DataGridTextBox)
            'AddHandler coltxtbx.KeyDown, AddressOf GridKeyDownEvent
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "ADDRESSSUSPENDED"
            TextCol.HeaderText = "Address Suspended"
            TextCol.Width = CInt(GetSetting(_APPKEY, "ProviderLookup\ColumnSettings", "Col " & TextCol.MappingName, "100"))
            TextCol.NullText = ""
            TextBoxCol = CType(TextCol.TextBox, DataGridTextBox)
            'AddHandler coltxtbx.KeyDown, AddressOf GridKeyDownEvent
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "ADDRESS_LINE1"
            TextCol.HeaderText = "Address"
            TextCol.Width = CInt(GetSetting(_APPKEY, "ProviderLookup\ColumnSettings", "Col " & TextCol.MappingName, "300"))
            TextCol.NullText = ""
            TextBoxCol = CType(TextCol.TextBox, DataGridTextBox)
            'AddHandler coltxtbx.KeyDown, AddressOf GridKeyDownEvent
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "CITY"
            TextCol.HeaderText = "City"
            TextCol.Width = CInt(GetSetting(_APPKEY, "ProviderLookup\ColumnSettings", "Col " & TextCol.MappingName, "150"))
            TextCol.NullText = ""
            TextBoxCol = CType(TextCol.TextBox, DataGridTextBox)
            'AddHandler coltxtbx.KeyDown, AddressOf GridKeyDownEvent
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "STATE"
            TextCol.HeaderText = "State"
            TextCol.Width = CInt(GetSetting(_APPKEY, "ProviderLookup\ColumnSettings", "Col " & TextCol.MappingName, "150"))
            TextCol.NullText = ""
            TextBoxCol = CType(TextCol.TextBox, DataGridTextBox)
            'AddHandler coltxtbx.KeyDown, AddressOf GridKeyDownEvent
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "ZIP"
            TextCol.HeaderText = "Zip"
            TextCol.Width = CInt(GetSetting(_APPKEY, "ProviderLookup\ColumnSettings", "Col " & TextCol.MappingName, "100"))
            TextCol.NullText = ""
            TextBoxCol = CType(TextCol.TextBox, DataGridTextBox)
            'AddHandler coltxtbx.KeyDown, AddressOf GridKeyDownEvent
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "COUNTRY"
            TextCol.HeaderText = "Country"
            TextCol.Width = CInt(GetSetting(_APPKEY, "ProviderLookup\ColumnSettings", "Col " & TextCol.MappingName, "150"))
            TextCol.NullText = ""
            TextBoxCol = CType(TextCol.TextBox, DataGridTextBox)
            'AddHandler coltxtbx.KeyDown, AddressOf GridKeyDownEvent
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "PPOC_ELIGIBLE_SW"
            TextCol.HeaderText = "PPOC Provider"
            TextCol.Width = CInt(GetSetting(_APPKEY, "ProviderLookup\ColumnSettings", "Col " & TextCol.MappingName, "100"))
            TextCol.NullText = ""
            TextBoxCol = CType(TextCol.TextBox, DataGridTextBox)
            'AddHandler coltxtbx.KeyDown, AddressOf GridKeyDownEvent
            AddHandler TextCol.Formatting, AddressOf FormattingPPOC
            DGTableStyle.GridColumnStyles.Add(TextCol)
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        End Try

        Try
            ProviderDataGrid.TableStyles.Clear()
            ProviderDataGrid.TableStyles.Add(DGTableStyle)
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        End Try

        CurMan = Nothing
        DGTableStyle = Nothing
    End Sub

    Private Sub FormattingPPOC(ByRef value As Object, ByVal RowNum As Integer)
        Try
            If IsDBNull(value) = False AndAlso CBool(value) = True Then
                value = "Yes"
            Else
                value = "No"
            End If
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        End Try
    End Sub

    Private Sub ProviderDataGrid_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ProviderDataGrid.MouseUp
        Dim DG As DataGrid
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo

        Try

            DG = CType(sender, DataGrid)
            HTI = DG.HitTest(e.X, e.Y)

            If e.Button = MouseButtons.Left Then
                Select Case HTI.Type
                    Case Is = DataGrid.HitTestType.Cell
                        If ProviderDataGrid.IsSelected(HTI.Row) = True Then
                            ProviderDataGrid.UnSelect(HTI.Row)
                        Else
                            ProviderDataGrid.Select(HTI.Row)
                        End If
                    Case Is = DataGrid.HitTestType.RowHeader
                        If ProviderDataGrid.IsSelected(HTI.Row) = False Then
                            Exit Try
                        End If
                End Select

                If HTI.Type = DataGrid.HitTestType.Cell Or HTI.Type = DataGrid.HitTestType.RowHeader Then
                    Me.DialogResult = DialogResult.OK

                    'If Control.ModifierKeys <> Keys.Control AndAlso Control.ModifierKeys <> Keys.Shift Then
                    '    Me.DialogResult = DialogResult.OK
                    'Else
                    '    MultiTimer.Enabled = True
                    'End If
                End If
            End If
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        End Try
    End Sub

    Private Sub SearchButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchButton.Click
        Try
            Search()
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub Search()
        Try
            ProviderDataGrid.CaptionText = "Searching..."
            ProviderDataGrid.Refresh()

            ProvDataSet.PROVIDER.Rows.Clear()

            ProvDataSet = CType(CMSDALFDBMD.RetrieveProviders(UFCWGeneral.IsNullIntegerHandler(Replace(ProvTINTextBox.Text, "-", ""), "ProvTINTextBox", True), ProvNameTextBox.Text, UFCWGeneral.IsNullDecimalHandler(NPITextBox.Text, "NPITextBox", True), ProvDataSet), ProvDataSet)

            SetTableStyle()

            ProviderDataGrid.DataSource = ProvDataSet.PROVIDER

            ProviderDataGrid.CaptionText = "Showing " & ProvDataSet.PROVIDER.Rows.Count & " Provider" & If(ProvDataSet.PROVIDER.Rows.Count = 1, "", "s")

            ProviderDataGrid.Focus()
        Catch ex As Exception
            ProviderDataGrid.CaptionText = "Showing 0 Providers"

            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub ProvTINTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProvTINTextBox.TextChanged
        Try
            Dim TBox As TextBox = CType(sender, TextBox)
            Dim IntCnt As Integer
            Dim StrTmp As String

            If IsNumeric(TBox.Text) = False AndAlso Len(TBox.Text) > 0 Then
                StrTmp = TBox.Text
                For IntCnt = 1 To Len(StrTmp)
                    If IsNumeric(Mid(StrTmp, IntCnt, 1)) = False AndAlso Len(StrTmp) > 0 _
                                                AndAlso Mid(StrTmp, IntCnt, 1) <> "-" Then
                        StrTmp = Replace(StrTmp, Mid(StrTmp, IntCnt, 1), "")
                    End If
                Next
                TBox.Text = StrTmp
            End If

            If ProvTINTextBox.Text.Trim = "" AndAlso ProvNameTextBox.Text.Trim = "" AndAlso Me.NPITextBox.Text.Trim = "" Then
                SearchButton.Enabled = False
            Else
                SearchButton.Enabled = True
            End If
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub LastNameTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProvNameTextBox.TextChanged
        Try
            If ProvTINTextBox.Text.Trim = "" AndAlso ProvNameTextBox.Text.Trim = "" AndAlso Me.NPITextBox.Text.Trim = "" Then
                SearchButton.Enabled = False
            Else
                SearchButton.Enabled = True
            End If
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub NPITextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles NPITextBox.TextChanged
        Dim TBox As TextBox
        Dim intCnt As Integer
        Dim strTmp As String

        Try

            TBox = CType(sender, TextBox)

            If IsNumeric(TBox.Text) = False AndAlso Len(TBox.Text) > 0 Then
                strTmp = TBox.Text
                For intCnt = 1 To Len(strTmp)
                    If IsNumeric(Mid(strTmp, intCnt, 1)) = False AndAlso Len(strTmp) > 0 _
                                                AndAlso Mid(strTmp, intCnt, 1) <> "-" Then
                        strTmp = Replace(strTmp, Mid(strTmp, intCnt, 1), "")
                    End If
                Next
                TBox.Text = strTmp
            End If

            If ProvTINTextBox.Text.Trim = "" AndAlso ProvNameTextBox.Text.Trim = "" AndAlso NPITextBox.Text.Trim.Trim = "" Then
                SearchButton.Enabled = False
            Else
                SearchButton.Enabled = True
            End If
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

End Class