Option Strict On

Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling


Public Class ModifierLookupForm
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"

    <System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)

            _APPKEY = value
        End Set
    End Property

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
    Friend WithEvents ModifierDataGrid As DataGridCustom
    Friend WithEvents MultiTimer As System.Timers.Timer
    Friend WithEvents SqlConnection1 As System.Data.SqlClient.SqlConnection
    Friend WithEvents SqlSelectCommand2 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlInsertCommand2 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlUpdateCommand2 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlDeleteCommand2 As System.Data.SqlClient.SqlCommand
    Friend WithEvents SqlDataAdapter2 As System.Data.SqlClient.SqlDataAdapter
    Friend WithEvents CancelFormButton As System.Windows.Forms.Button
    Friend WithEvents ModifierValuesDataSet As ModifierValuesDataSet
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ModifierLookupForm))
        Me.ModifierDataGrid = New DataGridCustom()
        Me.MultiTimer = New System.Timers.Timer()
        Me.SqlConnection1 = New System.Data.SqlClient.SqlConnection()
        Me.SqlSelectCommand2 = New System.Data.SqlClient.SqlCommand()
        Me.SqlInsertCommand2 = New System.Data.SqlClient.SqlCommand()
        Me.SqlUpdateCommand2 = New System.Data.SqlClient.SqlCommand()
        Me.SqlDeleteCommand2 = New System.Data.SqlClient.SqlCommand()
        Me.SqlDataAdapter2 = New System.Data.SqlClient.SqlDataAdapter()
        Me.ModifierValuesDataSet = New ModifierValuesDataSet()
        Me.CancelFormButton = New System.Windows.Forms.Button()
        CType(Me.ModifierDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MultiTimer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ModifierValuesDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ModifierDataGrid
        '
        Me.ModifierDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.ModifierDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.ModifierDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ModifierDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ModifierDataGrid.ADGroupsThatCanFind = ""
        Me.ModifierDataGrid.ADGroupsThatCanMultiSort = ""
        Me.ModifierDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ModifierDataGrid.AllowAutoSize = True
        Me.ModifierDataGrid.AllowColumnReorder = False
        Me.ModifierDataGrid.AllowCopy = True
        Me.ModifierDataGrid.AllowCustomize = True
        Me.ModifierDataGrid.AllowDelete = False
        Me.ModifierDataGrid.AllowDragDrop = False
        Me.ModifierDataGrid.AllowEdit = False
        Me.ModifierDataGrid.AllowExport = False
        Me.ModifierDataGrid.AllowFilter = True
        Me.ModifierDataGrid.AllowFind = True
        Me.ModifierDataGrid.AllowGoTo = True
        Me.ModifierDataGrid.AllowMultiSelect = True
        Me.ModifierDataGrid.AllowMultiSort = False
        Me.ModifierDataGrid.AllowNew = False
        Me.ModifierDataGrid.AllowPrint = False
        Me.ModifierDataGrid.AllowRefresh = False
        Me.ModifierDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ModifierDataGrid.AppKey = "UFCW\Claims\"
        Me.ModifierDataGrid.AutoSaveCols = True
        Me.ModifierDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.ModifierDataGrid.CaptionVisible = False
        Me.ModifierDataGrid.ColumnHeaderLabel = Nothing
        Me.ModifierDataGrid.ColumnRePositioning = False
        Me.ModifierDataGrid.ColumnResizing = False
        Me.ModifierDataGrid.ConfirmDelete = True
        Me.ModifierDataGrid.CopySelectedOnly = True
        Me.ModifierDataGrid.CurrentBSPosition = -1
        Me.ModifierDataGrid.DataMember = ""
        Me.ModifierDataGrid.DragColumn = 0
        Me.ModifierDataGrid.ExportSelectedOnly = True
        Me.ModifierDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ModifierDataGrid.HighlightedRow = Nothing
        Me.ModifierDataGrid.HighLightModifiedRows = False
        Me.ModifierDataGrid.IsMouseDown = False
        Me.ModifierDataGrid.LastGoToLine = ""
        Me.ModifierDataGrid.Location = New System.Drawing.Point(8, 8)
        Me.ModifierDataGrid.MultiSort = False
        Me.ModifierDataGrid.Name = "ModifierDataGrid"
        Me.ModifierDataGrid.OldSelectedRow = Nothing
        Me.ModifierDataGrid.PreviousBSPosition = -1
        Me.ModifierDataGrid.ReadOnly = True
        Me.ModifierDataGrid.RetainRowSelectionAfterSort = True
        Me.ModifierDataGrid.SetRowOnRightClick = True
        Me.ModifierDataGrid.ShiftPressed = False
        Me.ModifierDataGrid.SingleClickBooleanColumns = True
        Me.ModifierDataGrid.Size = New System.Drawing.Size(536, 319)
        Me.ModifierDataGrid.Sort = Nothing
        Me.ModifierDataGrid.StyleName = ""
        Me.ModifierDataGrid.SubKey = ""
        Me.ModifierDataGrid.SuppressMouseDown = False
        Me.ModifierDataGrid.SuppressTriangle = False
        Me.ModifierDataGrid.TabIndex = 0
        '
        'MultiTimer
        '
        Me.MultiTimer.SynchronizingObject = Me
        '
        'SqlConnection1
        '
        Me.SqlConnection1.ConnectionString = "workstation id=UFCWC102T;packet size=4096;integrated security=SSPI;data source=UF" &
    "CWSQL;persist security info=False;initial catalog=Medical"
        Me.SqlConnection1.FireInfoMessageEventOnUserErrors = False
        '
        'SqlSelectCommand2
        '
        Me.SqlSelectCommand2.CommandText = "SELECT MODIFIER_VALUE, FROM_DATE, THRU_DATE, FULL_DESC, CREATE_USERID, CREATE_DAT" &
    "E, USERID, LASTUPDT FROM MODIFIER_VALUES"
        Me.SqlSelectCommand2.Connection = Me.SqlConnection1
        '
        'SqlInsertCommand2
        '
        Me.SqlInsertCommand2.CommandText = resources.GetString("SqlInsertCommand2.CommandText")
        Me.SqlInsertCommand2.Connection = Me.SqlConnection1
        Me.SqlInsertCommand2.Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@MODIFIER_VALUE", System.Data.SqlDbType.VarChar, 50, "MODIFIER_VALUE"), New System.Data.SqlClient.SqlParameter("@FROM_DATE", System.Data.SqlDbType.DateTime, 8, "FROM_DATE"), New System.Data.SqlClient.SqlParameter("@THRU_DATE", System.Data.SqlDbType.DateTime, 8, "THRU_DATE"), New System.Data.SqlClient.SqlParameter("@FULL_DESC", System.Data.SqlDbType.VarChar, 255, "FULL_DESC"), New System.Data.SqlClient.SqlParameter("@CREATE_USERID", System.Data.SqlDbType.VarChar, 40, "CREATE_USERID"), New System.Data.SqlClient.SqlParameter("@CREATE_DATE", System.Data.SqlDbType.DateTime, 8, "CREATE_DATE"), New System.Data.SqlClient.SqlParameter("@USERID", System.Data.SqlDbType.VarChar, 40, "USERID"), New System.Data.SqlClient.SqlParameter("@LASTUPDT", System.Data.SqlDbType.DateTime, 8, "LASTUPDT")})
        '
        'SqlUpdateCommand2
        '
        Me.SqlUpdateCommand2.CommandText = resources.GetString("SqlUpdateCommand2.CommandText")
        Me.SqlUpdateCommand2.Connection = Me.SqlConnection1
        Me.SqlUpdateCommand2.Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@MODIFIER_VALUE", System.Data.SqlDbType.VarChar, 50, "MODIFIER_VALUE"), New System.Data.SqlClient.SqlParameter("@FROM_DATE", System.Data.SqlDbType.DateTime, 8, "FROM_DATE"), New System.Data.SqlClient.SqlParameter("@THRU_DATE", System.Data.SqlDbType.DateTime, 8, "THRU_DATE"), New System.Data.SqlClient.SqlParameter("@FULL_DESC", System.Data.SqlDbType.VarChar, 255, "FULL_DESC"), New System.Data.SqlClient.SqlParameter("@CREATE_USERID", System.Data.SqlDbType.VarChar, 40, "CREATE_USERID"), New System.Data.SqlClient.SqlParameter("@CREATE_DATE", System.Data.SqlDbType.DateTime, 8, "CREATE_DATE"), New System.Data.SqlClient.SqlParameter("@USERID", System.Data.SqlDbType.VarChar, 40, "USERID"), New System.Data.SqlClient.SqlParameter("@LASTUPDT", System.Data.SqlDbType.DateTime, 8, "LASTUPDT"), New System.Data.SqlClient.SqlParameter("@Original_FROM_DATE", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "FROM_DATE", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_MODIFIER_VALUE", System.Data.SqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "MODIFIER_VALUE", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CREATE_DATE", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CREATE_DATE", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CREATE_USERID", System.Data.SqlDbType.VarChar, 40, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CREATE_USERID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_FULL_DESC", System.Data.SqlDbType.VarChar, 255, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "FULL_DESC", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_LASTUPDT", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "LASTUPDT", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_THRU_DATE", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "THRU_DATE", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_USERID", System.Data.SqlDbType.VarChar, 40, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "USERID", System.Data.DataRowVersion.Original, Nothing)})
        '
        'SqlDeleteCommand2
        '
        Me.SqlDeleteCommand2.CommandText = resources.GetString("SqlDeleteCommand2.CommandText")
        Me.SqlDeleteCommand2.Connection = Me.SqlConnection1
        Me.SqlDeleteCommand2.Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@Original_FROM_DATE", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "FROM_DATE", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_MODIFIER_VALUE", System.Data.SqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "MODIFIER_VALUE", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CREATE_DATE", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CREATE_DATE", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CREATE_USERID", System.Data.SqlDbType.VarChar, 40, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CREATE_USERID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_FULL_DESC", System.Data.SqlDbType.VarChar, 255, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "FULL_DESC", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_LASTUPDT", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "LASTUPDT", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_THRU_DATE", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "THRU_DATE", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_USERID", System.Data.SqlDbType.VarChar, 40, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "USERID", System.Data.DataRowVersion.Original, Nothing)})
        '
        'SqlDataAdapter2
        '
        Me.SqlDataAdapter2.DeleteCommand = Me.SqlDeleteCommand2
        Me.SqlDataAdapter2.InsertCommand = Me.SqlInsertCommand2
        Me.SqlDataAdapter2.SelectCommand = Me.SqlSelectCommand2
        Me.SqlDataAdapter2.TableMappings.AddRange(New System.Data.Common.DataTableMapping() {New System.Data.Common.DataTableMapping("Table", "MODIFIER_VALUES", New System.Data.Common.DataColumnMapping() {New System.Data.Common.DataColumnMapping("MODIFIER_VALUE", "MODIFIER_VALUE"), New System.Data.Common.DataColumnMapping("FROM_DATE", "FROM_DATE"), New System.Data.Common.DataColumnMapping("THRU_DATE", "THRU_DATE"), New System.Data.Common.DataColumnMapping("FULL_DESC", "FULL_DESC"), New System.Data.Common.DataColumnMapping("CREATE_USERID", "CREATE_USERID"), New System.Data.Common.DataColumnMapping("CREATE_DATE", "CREATE_DATE"), New System.Data.Common.DataColumnMapping("USERID", "USERID"), New System.Data.Common.DataColumnMapping("LASTUPDT", "LASTUPDT")})})
        Me.SqlDataAdapter2.UpdateCommand = Me.SqlUpdateCommand2
        '
        'ModifierValuesDataSet
        '
        Me.ModifierValuesDataSet.DataSetName = "ModifierValuesDataSet"
        Me.ModifierValuesDataSet.Locale = New System.Globalization.CultureInfo("en-US")
        Me.ModifierValuesDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'CancelFormButton
        '
        Me.CancelFormButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CancelFormButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelFormButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CancelFormButton.Location = New System.Drawing.Point(470, 333)
        Me.CancelFormButton.Name = "CancelFormButton"
        Me.CancelFormButton.Size = New System.Drawing.Size(74, 23)
        Me.CancelFormButton.TabIndex = 22
        Me.CancelFormButton.Text = "Cancel"
        '
        'ModifierLookup
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.CancelFormButton
        Me.ClientSize = New System.Drawing.Size(552, 358)
        Me.Controls.Add(Me.CancelFormButton)
        Me.Controls.Add(Me.ModifierDataGrid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(392, 176)
        Me.Name = "ModifierLookup"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "Select Modifier(s)..."
        CType(Me.ModifierDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MultiTimer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ModifierValuesDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Form Events"
    Private Sub ModifierLookup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            SetSettings()

            Dim FormText As String = Me.Text

            Me.Text = "Loading Modifiers... Please Wait"
            Me.Show()
            Me.Refresh()

            LoadModifier()

            Me.Text = FormText

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub DetailLineModifier_FormClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.FormClosing
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
        Dim lWindowState As FormWindowState = Me.WindowState
        SaveSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(lWindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = lWindowState

    End Sub
#End Region

    Private Sub LoadModifier()
        Try
            ModifierValuesDataSet = CType(CMSDALFDBMD.RetrieveModifierValues(Nothing, ModifierValuesDataSet), ModifierValuesDataSet)

            ModifierDataGrid.SuspendLayout()
            ModifierDataGrid.DataSource = ModifierValuesDataSet.MODIFIER_VALUES
            SetModifierTableStyle(ModifierDataGrid)
            ModifierDataGrid.ResumeLayout()

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        End Try
    End Sub

    Private Sub SetModifierTableStyle(ByVal DataGrid As DataGridCustom)

        Dim DGTableStyle As DataGridTableStyle
        Dim TextCol As DataGridHighlightTextBoxColumn
        Dim coltxtbx As DataGridTextBox

        Try

            DGTableStyle = New DataGridTableStyle()
            DGTableStyle.MappingName = ModifierValuesDataSet.MODIFIER_VALUES.TableName

            DGTableStyle.GridLineStyle = DataGridLineStyle.None

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "MODIFIER_VALUE"
            TextCol.HeaderText = "Code"
            TextCol.Width = CInt(GetSetting(_APPKEY, DataGrid.Name & "\" & DGTableStyle.MappingName.ToString & "\ColumnSettings", "Col " & TextCol.MappingName, CStr(UFCWGeneral.MeasureWidthinPixels(5, DataGrid.Font.Name, DataGrid.Font.Size))))
            TextCol.NullText = ""
            coltxtbx = CType(TextCol.TextBox, DataGridTextBox)
            AddHandler coltxtbx.KeyDown, AddressOf GridKeyDownEvent
            DGTableStyle.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridHighlightTextBoxColumn
            TextCol.MappingName = "FULL_DESC"
            TextCol.HeaderText = "Description"
            TextCol.Width = CInt(GetSetting(_APPKEY, DataGrid.Name & "\" & DGTableStyle.MappingName.ToString & "\ColumnSettings", "Col " & TextCol.MappingName, CStr(UFCWGeneral.MeasureWidthinPixels(178, DataGrid.Font.Name, DataGrid.Font.Size))))
            TextCol.NullText = ""
            coltxtbx = CType(TextCol.TextBox, DataGridTextBox)
            AddHandler coltxtbx.KeyDown, AddressOf GridKeyDownEvent
            DGTableStyle.GridColumnStyles.Add(TextCol)

            ModifierDataGrid.TableStyles.Clear()
            ModifierDataGrid.TableStyles.Add(DGTableStyle)

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        Finally
            DGTableStyle = Nothing
        End Try

    End Sub

    Private Sub ModifierDataGrid_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ModifierDataGrid.MouseUp
        Dim DG As DataGrid
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo

        Try
            DG = CType(sender, DataGrid)
            HTI = DG.HitTest(e.X, e.Y)

            If e.Button = MouseButtons.Left Then
                Select Case HTI.Type
                    Case Is = DataGrid.HitTestType.Cell
                        If ModifierDataGrid.IsSelected(HTI.Row) = True Then
                            ModifierDataGrid.UnSelect(HTI.Row)
                        Else
                            ModifierDataGrid.Select(HTI.Row)
                        End If
                    Case Is = DataGrid.HitTestType.RowHeader
                        If ModifierDataGrid.IsSelected(HTI.Row) = False Then
                            Exit Try
                        End If
                End Select

                If HTI.Type = DataGrid.HitTestType.Cell OrElse HTI.Type = DataGrid.HitTestType.RowHeader Then
                    If Control.ModifierKeys <> Keys.Control AndAlso Control.ModifierKeys <> Keys.Shift Then
                        Me.DialogResult = DialogResult.OK
                    Else
                        MultiTimer.Enabled = True
                    End If
                End If
            End If
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        End Try

    End Sub

    Private Sub MultiTimer_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles MultiTimer.Elapsed
        If Control.ModifierKeys <> Keys.Control AndAlso Control.ModifierKeys <> Keys.Shift Then
            Dim Selected As Boolean = False

            For cnt As Integer = 0 To ModifierDataGrid.GetGridRowCount - 1
                If ModifierDataGrid.IsSelected(cnt) Then
                    Selected = True
                    Exit For
                End If
            Next

            If Selected AndAlso ModifierDataGrid.LastHitSpot.Type = DataGrid.HitTestType.RowHeader Then
                MultiTimer.Enabled = False
                Me.DialogResult = DialogResult.OK
            End If
        End If
    End Sub

    Private Sub GridKeyDownEvent(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ModifierDataGrid.KeyDown
        If e.Control OrElse e.Shift Then
            MultiTimer.Enabled = True
        End If
    End Sub

    Private Sub CancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelFormButton.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
    End Sub

End Class


