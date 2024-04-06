Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports UFCW.CMS.DAL
Public Class ParticipantLookup
    Inherits System.Windows.Forms.Form

    Private _PartFirstName As String
    Private _PartLastName As String
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
    Friend WithEvents FirstNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents LastNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents SearchButton As System.Windows.Forms.Button
    Friend WithEvents EligDataset As EligDataset
    Public WithEvents ParticipantDataGrid As DataGridPlus.DataGridCustom
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.ParticipantDataGrid = New DataGridPlus.DataGridCustom
        Me.Label1 = New System.Windows.Forms.Label
        Me.FirstNameTextBox = New System.Windows.Forms.TextBox
        Me.LastNameTextBox = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.SearchButton = New System.Windows.Forms.Button
        Me.EligDataset = New EligDataset
        CType(Me.ParticipantDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EligDataset, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ParticipantDataGrid
        '
        Me.ParticipantDataGrid.AllowColumnReorder = False
        Me.ParticipantDataGrid.AllowCopy = False
        Me.ParticipantDataGrid.AllowDelete = False
        Me.ParticipantDataGrid.AllowDragDrop = False
        Me.ParticipantDataGrid.AllowEdit = False
        Me.ParticipantDataGrid.AllowExport = False
        Me.ParticipantDataGrid.AllowFind = True
        Me.ParticipantDataGrid.AllowGoTo = True
        Me.ParticipantDataGrid.AllowMultiSelect = False
        Me.ParticipantDataGrid.AllowMultiSort = False
        Me.ParticipantDataGrid.AllowNew = False
        Me.ParticipantDataGrid.AllowPrint = False
        Me.ParticipantDataGrid.AllowRefresh = False
        Me.ParticipantDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ParticipantDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.ParticipantDataGrid.ConfirmDelete = True
        Me.ParticipantDataGrid.CopySelectedOnly = True
        Me.ParticipantDataGrid.DataMember = ""
        Me.ParticipantDataGrid.ExportSelectedOnly = True
        Me.ParticipantDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ParticipantDataGrid.LastGoToLine = ""
        Me.ParticipantDataGrid.Location = New System.Drawing.Point(8, 32)
        Me.ParticipantDataGrid.MultiSort = False
        Me.ParticipantDataGrid.Name = "ParticipantDataGrid"
        Me.ParticipantDataGrid.ReadOnly = True
        Me.ParticipantDataGrid.SetRowOnRightClick = True
        Me.ParticipantDataGrid.SingleClickBooleanColumns = True
        Me.ParticipantDataGrid.Size = New System.Drawing.Size(576, 208)
        Me.ParticipantDataGrid.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 16)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "First Name:"
        '
        'FirstNameTextBox
        '
        Me.FirstNameTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.FirstNameTextBox.Location = New System.Drawing.Point(72, 4)
        Me.FirstNameTextBox.MaxLength = 40
        Me.FirstNameTextBox.Name = "FirstNameTextBox"
        Me.FirstNameTextBox.Size = New System.Drawing.Size(168, 20)
        Me.FirstNameTextBox.TabIndex = 0
        Me.FirstNameTextBox.Text = ""
        '
        'LastNameTextBox
        '
        Me.LastNameTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.LastNameTextBox.Location = New System.Drawing.Point(312, 4)
        Me.LastNameTextBox.MaxLength = 40
        Me.LastNameTextBox.Name = "LastNameTextBox"
        Me.LastNameTextBox.Size = New System.Drawing.Size(184, 20)
        Me.LastNameTextBox.TabIndex = 1
        Me.LastNameTextBox.Text = ""
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(248, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 16)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Last Name:"
        '
        'SearchButton
        '
        Me.SearchButton.Enabled = False
        Me.SearchButton.Location = New System.Drawing.Point(504, 4)
        Me.SearchButton.Name = "SearchButton"
        Me.SearchButton.TabIndex = 2
        Me.SearchButton.Text = "&Search"
        '
        'EligDataset
        '
        Me.EligDataset.DataSetName = "EligDataset"
        Me.EligDataset.Locale = New System.Globalization.CultureInfo("en-US")
        '
        'ParticipantLookup
        '
        Me.AcceptButton = Me.SearchButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(592, 246)
        Me.Controls.Add(Me.SearchButton)
        Me.Controls.Add(Me.LastNameTextBox)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.FirstNameTextBox)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ParticipantDataGrid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MinimumSize = New System.Drawing.Size(600, 192)
        Me.Name = "ParticipantLookup"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Select Participant:"
        CType(Me.ParticipantDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EligDataset, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Constructors"
    Sub New(ByVal PartLastName As String)
        Me.New()

        _PartLastName = PartLastName

        LastNameTextBox.Text = PartLastName
    End Sub

    Sub New(ByVal PartFirstName As String, ByVal PartLastName As String)
        Me.New()

        _PartFirstName = PartFirstName
        _PartLastName = PartLastName

        FirstNameTextBox.Text = PartFirstName
        LastNameTextBox.Text = PartLastName
    End Sub
#End Region

#Region "Public Properties"
    <System.ComponentModel.Description("Represents the application key used when accessing the registry.")> _
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal Value As String)
            _APPKEY = Value
        End Set
    End Property

    Public ReadOnly Property ParticipantTable() As DataTable
        Get
            Return EligDataset.REG_MASTER
        End Get
    End Property
#End Region

    Private Sub ParticipantLookup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            SetSettings()
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub ParticipantLookup_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        SaveSettings()
    End Sub

    Private Sub SetSettings()
        Me.Top = If(CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)))
        Me.Height = CInt(GetSetting(Me.APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = If(CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)))
        Me.Width = CInt(GetSetting(Me.APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(Me.APPKEY, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)
    End Sub

    Private Sub SaveSettings()
        Dim lWindowState As FormWindowState = Me.WindowState
        SaveSetting(Me.APPKEY, Me.Name & "\Settings", "WindowState", CInt(lWindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(Me.APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(Me.APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(Me.APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(Me.APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = lWindowState

        SaveColSettings()
    End Sub

    Private Sub SaveColSettings()

        If Not IsNothing(ParticipantDataGrid) Then
            If Not IsNothing(ParticipantDataGrid.DataSource) Then
                ParticipantDataGrid.SaveColumnsSizeAndPosition(Me.AppKey, ParticipantDataGrid.Name & "\" & ParticipantDataGrid.GetCurrentDataTable.TableName & "\ColumnSettings")
                ParticipantDataGrid.SaveSortByColumnName(Me.AppKey, ParticipantDataGrid.Name & "\" & ParticipantDataGrid.GetCurrentDataTable.TableName & "\Sort", ParticipantDataGrid.GetGridSortColumn)
            End If
        End If

    End Sub

    Private Sub SetTableStyle()
        Dim DGTableStyle As DataGridTableStyle
        Dim TextCol As DataGridPlus.DataGridHighlightTextBoxColumn
        Dim coltxtbx As DataGridTextBox
        Dim CurMan As CurrencyManager

        Try
            SaveColSettings()

            'CurMan = CType(Me.BindingContext(EligDataset.REG_MASTER), CurrencyManager)

            DGTableStyle = New DataGridTableStyle  'CurMan)
            DGTableStyle.MappingName = "Table" 'EligDataset.REG_MASTER.TableName
            DGTableStyle.GridColumnStyles.Clear()
            DGTableStyle.GridLineStyle = DataGridLineStyle.None

            TextCol = New DataGridPlus.DataGridHighlightTextBoxColumn
            TextCol.MappingName = "FAMILY_ID"
            TextCol.HeaderText = "Family ID"
            TextCol.Width = CInt(GetSetting(Me.AppKey, "ParticipantLookup\ColumnSettings", "Col " & TextCol.MappingName, "80"))
            TextCol.NullText = ""
            coltxtbx = CType(TextCol.TextBox, DataGridTextBox)
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridPlus.DataGridHighlightTextBoxColumn
            TextCol.MappingName = "SSNO"
            TextCol.HeaderText = "SSN"
            TextCol.Width = CInt(GetSetting(Me.AppKey, "ParticipantLookup\ColumnSettings", "Col " & TextCol.MappingName, "65"))
            TextCol.NullText = ""
            coltxtbx = CType(TextCol.TextBox, DataGridTextBox)
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridPlus.DataGridHighlightTextBoxColumn
            TextCol.MappingName = "FIRST_NAME"
            TextCol.HeaderText = "First Name"
            TextCol.Width = CInt(GetSetting(Me.AppKey, "ParticipantLookup\ColumnSettings", "Col " & TextCol.MappingName, "100"))
            TextCol.NullText = ""
            coltxtbx = CType(TextCol.TextBox, DataGridTextBox)
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridPlus.DataGridHighlightTextBoxColumn
            TextCol.MappingName = "MIDDLE_INITIAL"
            TextCol.HeaderText = "Initial"
            TextCol.Width = CInt(GetSetting(Me.AppKey, "ParticipantLookup\ColumnSettings", "Col " & TextCol.MappingName, "50"))
            TextCol.NullText = ""
            coltxtbx = CType(TextCol.TextBox, DataGridTextBox)
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridPlus.DataGridHighlightTextBoxColumn
            TextCol.MappingName = "LAST_NAME"
            TextCol.HeaderText = "Last Name"
            TextCol.Width = CInt(GetSetting(Me.AppKey, "ParticipantLookup\ColumnSettings", "Col " & TextCol.MappingName, "100"))
            TextCol.NullText = ""
            coltxtbx = CType(TextCol.TextBox, DataGridTextBox)
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridPlus.DataGridHighlightTextBoxColumn
            TextCol.MappingName = "BIRTH_DATE"
            TextCol.HeaderText = "Birth Date"
            TextCol.Width = CInt(GetSetting(Me.AppKey, "ParticipantLookup\ColumnSettings", "Col " & TextCol.MappingName, "75"))
            TextCol.NullText = ""
            TextCol.Format = "MM-dd-yyyy"
            coltxtbx = CType(TextCol.TextBox, DataGridTextBox)
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridPlus.DataGridHighlightTextBoxColumn
            TextCol.MappingName = "GENDER"
            TextCol.HeaderText = "Gender"
            TextCol.Width = CInt(GetSetting(Me.AppKey, "ParticipantLookup\ColumnSettings", "Col " & TextCol.MappingName, "65"))
            TextCol.NullText = ""
            coltxtbx = CType(TextCol.TextBox, DataGridTextBox)
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridPlus.DataGridHighlightTextBoxColumn
            TextCol.MappingName = "ADDRESS1"
            TextCol.HeaderText = "Address"
            TextCol.Width = CInt(GetSetting(Me.AppKey, "ParticipantLookup\ColumnSettings", "Col " & TextCol.MappingName, "65"))
            TextCol.NullText = ""
            coltxtbx = CType(TextCol.TextBox, DataGridTextBox)
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridPlus.DataGridHighlightTextBoxColumn
            TextCol.MappingName = "CITY"
            TextCol.HeaderText = "City"
            TextCol.Width = CInt(GetSetting(Me.AppKey, "ParticipantLookup\ColumnSettings", "Col " & TextCol.MappingName, "65"))
            TextCol.NullText = ""
            coltxtbx = CType(TextCol.TextBox, DataGridTextBox)
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridPlus.DataGridHighlightTextBoxColumn
            TextCol.MappingName = "STATE"
            TextCol.HeaderText = "State"
            TextCol.Width = CInt(GetSetting(Me.AppKey, "ParticipantLookup\ColumnSettings", "Col " & TextCol.MappingName, "65"))
            TextCol.NullText = ""
            coltxtbx = CType(TextCol.TextBox, DataGridTextBox)
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridPlus.DataGridHighlightTextBoxColumn
            TextCol.MappingName = "ZIP1"
            TextCol.HeaderText = "Zip"
            TextCol.Width = CInt(GetSetting(Me.AppKey, "ParticipantLookup\ColumnSettings", "Col " & TextCol.MappingName, "65"))
            TextCol.NullText = ""
            coltxtbx = CType(TextCol.TextBox, DataGridTextBox)
            DGTableStyle.GridColumnStyles.Add(TextCol)
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        End Try

        Try
            ParticipantDataGrid.TableStyles.Clear()
            ParticipantDataGrid.TableStyles.Add(DGTableStyle)
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        End Try

        CurMan = Nothing
        DGTableStyle = Nothing
    End Sub

    Private Sub ParticipantDataGrid_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ParticipantDataGrid.MouseUp
        Dim myGrid As DataGrid = CType(sender, DataGrid)
        Dim hti As System.Windows.Forms.DataGrid.HitTestInfo
        hti = myGrid.HitTest(e.X, e.Y)

        Try
            If e.Button = MouseButtons.Left Then
                Select Case hti.Type
                    Case Is = DataGrid.HitTestType.Cell
                        If ParticipantDataGrid.IsSelected(hti.Row) = True Then
                            ParticipantDataGrid.UnSelect(hti.Row)
                        Else
                            ParticipantDataGrid.Select(hti.Row)
                        End If
                    Case Is = DataGrid.HitTestType.RowHeader
                        If ParticipantDataGrid.IsSelected(hti.Row) = False Then
                            Exit Try
                        End If
                End Select

                If hti.Type = DataGrid.HitTestType.Cell Or hti.Type = DataGrid.HitTestType.RowHeader Then
                    Me.DialogResult = DialogResult.OK

                    'If Control.ModifierKeys <> Keys.Control AndAlso Control.ModifierKeys <> Keys.Shift Then
                    '    Me.DialogResult = DialogResult.OK
                    'Else
                    '    MultiTimer.Enabled = True
                    'End If
                End If
            End If
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub

    Private Sub SearchButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchButton.Click
        Try
            Search()
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub Search()
        Try
            ParticipantDataGrid.CaptionText = "Searching..."
            ParticipantDataGrid.Refresh()

            EligDataset.REG_MASTER.Rows.Clear()

            Dim dt As DataTable = CMSDALFDBMD.RetrieveParticipantsByFirstNameLastName(FirstNameTextBox.Text, LastNameTextBox.Text)

            SetTableStyle()

            ParticipantDataGrid.DataSource = dt

            ParticipantDataGrid.CaptionText = "Showing " & dt.Rows.Count & " Participant" & If(dt.Rows.Count = 1, "", "s")

        Catch ex As Exception
            ParticipantDataGrid.CaptionText = "Showing 0 Participants"

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub FirstNameTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FirstNameTextBox.TextChanged
        Try
            If FirstNameTextBox.Text.Trim = "" And LastNameTextBox.Text.Trim = "" Then
                SearchButton.Enabled = False
            Else
                SearchButton.Enabled = True
            End If
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub LastNameTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LastNameTextBox.TextChanged
        Try
            If FirstNameTextBox.Text.Trim = "" And LastNameTextBox.Text.Trim = "" Then
                SearchButton.Enabled = False
            Else
                SearchButton.Enabled = True
            End If
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub
End Class
