Option Strict On

Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Imports System.Text

Public Class ModifierLookup
    Inherits System.Windows.Forms.Form

    Private _APPKEY As String = "UFCW\Claims\"
    Private _modifier As String
    Private sb As New StringBuilder
    Private sbLastKeyCapturedTime As Date = UFCWGeneral.NowDate
    Private foundRow() As DataRow
    Friend WithEvents OKButton As System.Windows.Forms.Button

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
#End Region

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
    Friend WithEvents ModifierValuesDataSet As ModifierValuesDataSet
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim Resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ModifierLookup))
        Me.ModifierDataGrid = New DataGridCustom()
        Me.MultiTimer = New System.Timers.Timer()
        Me.SqlConnection1 = New System.Data.SqlClient.SqlConnection()
        Me.SqlSelectCommand2 = New System.Data.SqlClient.SqlCommand()
        Me.SqlInsertCommand2 = New System.Data.SqlClient.SqlCommand()
        Me.SqlUpdateCommand2 = New System.Data.SqlClient.SqlCommand()
        Me.SqlDeleteCommand2 = New System.Data.SqlClient.SqlCommand()
        Me.SqlDataAdapter2 = New System.Data.SqlClient.SqlDataAdapter()
        Me.ModifierValuesDataSet = New ModifierValuesDataSet()
        Me.OKButton = New System.Windows.Forms.Button()
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
        Me.ModifierDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ModifierDataGrid.ADGroupsThatCanMultiSort = ""
        Me.ModifierDataGrid.AllowAutoSize = True
        Me.ModifierDataGrid.AllowColumnReorder = False
        Me.ModifierDataGrid.AllowCopy = True
        Me.ModifierDataGrid.AllowCustomize = False
        Me.ModifierDataGrid.AllowDelete = False
        Me.ModifierDataGrid.AllowDragDrop = False
        Me.ModifierDataGrid.AllowEdit = False
        Me.ModifierDataGrid.AllowExport = False
        Me.ModifierDataGrid.AllowFilter = False
        Me.ModifierDataGrid.AllowFind = True
        Me.ModifierDataGrid.AllowGoTo = True
        Me.ModifierDataGrid.AllowMultiSelect = False
        Me.ModifierDataGrid.AllowMultiSort = False
        Me.ModifierDataGrid.AllowNew = False
        Me.ModifierDataGrid.AllowPrint = False
        Me.ModifierDataGrid.AllowRefresh = False
        Me.ModifierDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ModifierDataGrid.AppKey = "UFCW\Claims\"
        Me.ModifierDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.ModifierDataGrid.CaptionVisible = False
        Me.ModifierDataGrid.ColumnHeaderLabel = Nothing
        Me.ModifierDataGrid.ColumnRePositioning = False
        Me.ModifierDataGrid.ColumnResizing = False
        Me.ModifierDataGrid.ConfirmDelete = True
        Me.ModifierDataGrid.CopySelectedOnly = True
        Me.ModifierDataGrid.DataMember = ""
        Me.ModifierDataGrid.DragColumn = 0
        Me.ModifierDataGrid.ExportSelectedOnly = True
        Me.ModifierDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ModifierDataGrid.HighlightedRow = Nothing
        Me.ModifierDataGrid.IsMouseDown = False
        Me.ModifierDataGrid.LastGoToLine = ""
        Me.ModifierDataGrid.Location = New System.Drawing.Point(8, 8)
        Me.ModifierDataGrid.MultiSort = False
        Me.ModifierDataGrid.Name = "ModifierDataGrid"
        Me.ModifierDataGrid.OldSelectedRow = Nothing
        Me.ModifierDataGrid.ReadOnly = True
        Me.ModifierDataGrid.SetRowOnRightClick = True
        Me.ModifierDataGrid.ShiftPressed = False
        Me.ModifierDataGrid.SingleClickBooleanColumns = False
        Me.ModifierDataGrid.Size = New System.Drawing.Size(536, 315)
        Me.ModifierDataGrid.StyleName = ""
        Me.ModifierDataGrid.SubKey = ""
        Me.ModifierDataGrid.SuppressTriangle = False
        Me.ModifierDataGrid.TabIndex = 0
        Me.ModifierDataGrid.TabStop = False
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
        Me.SqlInsertCommand2.CommandText = Resources.GetString("SqlInsertCommand2.CommandText")
        Me.SqlInsertCommand2.Connection = Me.SqlConnection1
        Me.SqlInsertCommand2.Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@MODIFIER_VALUE", System.Data.SqlDbType.VarChar, 50, "MODIFIER_VALUE"), New System.Data.SqlClient.SqlParameter("@FROM_DATE", System.Data.SqlDbType.DateTime, 8, "FROM_DATE"), New System.Data.SqlClient.SqlParameter("@THRU_DATE", System.Data.SqlDbType.DateTime, 8, "THRU_DATE"), New System.Data.SqlClient.SqlParameter("@FULL_DESC", System.Data.SqlDbType.VarChar, 255, "FULL_DESC"), New System.Data.SqlClient.SqlParameter("@CREATE_USERID", System.Data.SqlDbType.VarChar, 40, "CREATE_USERID"), New System.Data.SqlClient.SqlParameter("@CREATE_DATE", System.Data.SqlDbType.DateTime, 8, "CREATE_DATE"), New System.Data.SqlClient.SqlParameter("@USERID", System.Data.SqlDbType.VarChar, 40, "USERID"), New System.Data.SqlClient.SqlParameter("@LASTUPDT", System.Data.SqlDbType.DateTime, 8, "LASTUPDT")})
        '
        'SqlUpdateCommand2
        '
        Me.SqlUpdateCommand2.CommandText = Resources.GetString("SqlUpdateCommand2.CommandText")
        Me.SqlUpdateCommand2.Connection = Me.SqlConnection1
        Me.SqlUpdateCommand2.Parameters.AddRange(New System.Data.SqlClient.SqlParameter() {New System.Data.SqlClient.SqlParameter("@MODIFIER_VALUE", System.Data.SqlDbType.VarChar, 50, "MODIFIER_VALUE"), New System.Data.SqlClient.SqlParameter("@FROM_DATE", System.Data.SqlDbType.DateTime, 8, "FROM_DATE"), New System.Data.SqlClient.SqlParameter("@THRU_DATE", System.Data.SqlDbType.DateTime, 8, "THRU_DATE"), New System.Data.SqlClient.SqlParameter("@FULL_DESC", System.Data.SqlDbType.VarChar, 255, "FULL_DESC"), New System.Data.SqlClient.SqlParameter("@CREATE_USERID", System.Data.SqlDbType.VarChar, 40, "CREATE_USERID"), New System.Data.SqlClient.SqlParameter("@CREATE_DATE", System.Data.SqlDbType.DateTime, 8, "CREATE_DATE"), New System.Data.SqlClient.SqlParameter("@USERID", System.Data.SqlDbType.VarChar, 40, "USERID"), New System.Data.SqlClient.SqlParameter("@LASTUPDT", System.Data.SqlDbType.DateTime, 8, "LASTUPDT"), New System.Data.SqlClient.SqlParameter("@Original_FROM_DATE", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "FROM_DATE", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_MODIFIER_VALUE", System.Data.SqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "MODIFIER_VALUE", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CREATE_DATE", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CREATE_DATE", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_CREATE_USERID", System.Data.SqlDbType.VarChar, 40, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "CREATE_USERID", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_FULL_DESC", System.Data.SqlDbType.VarChar, 255, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "FULL_DESC", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_LASTUPDT", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "LASTUPDT", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_THRU_DATE", System.Data.SqlDbType.DateTime, 8, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "THRU_DATE", System.Data.DataRowVersion.Original, Nothing), New System.Data.SqlClient.SqlParameter("@Original_USERID", System.Data.SqlDbType.VarChar, 40, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "USERID", System.Data.DataRowVersion.Original, Nothing)})
        '
        'SqlDeleteCommand2
        '
        Me.SqlDeleteCommand2.CommandText = Resources.GetString("SqlDeleteCommand2.CommandText")
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
        'OKButton
        '
        Me.OKButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OKButton.Location = New System.Drawing.Point(469, 329)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(75, 23)
        Me.OKButton.TabIndex = 47
        Me.OKButton.Text = "OK"
        '
        'ModifierLookup
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.OKButton
        Me.ClientSize = New System.Drawing.Size(552, 358)
        Me.Controls.Add(Me.OKButton)
        Me.Controls.Add(Me.ModifierDataGrid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(392, 176)
        Me.Name = "ModifierLookup"
        Me.ShowInTaskbar = False
        Me.Text = "Select Modifier..."
        CType(Me.ModifierDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MultiTimer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ModifierValuesDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Form Events"
    Private Sub ModifierLookup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            ModifierDataGrid.AllowMultiSelect = False

            If Not UFCWGeneral.SetFormPosition(Me, _APPKEY) Then Me.CenterToScreen()

            Dim FormText As String = Me.Text

            Me.Text = "Loading Modifiers... Please Wait"
            Me.Show()
            Me.Refresh()

            LoadModifier()

            Me.Text = FormText

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DetailLineModifier_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        UFCWGeneral.SaveFormPosition(Me, _APPKEY)
    End Sub

#End Region

    Private Sub LoadModifier()
        Try
            Using WC As New GlobalCursor

                ModifierDataGrid.SuspendLayout()

                ModifierValuesDataSet = CType(CMSDALFDBMD.RetrieveModifierValues(Nothing, ModifierValuesDataSet), ModifierValuesDataSet)

                ModifierDataGrid.DataSource = ModifierValuesDataSet.MODIFIER_VALUES

                ModifierDataGrid.SetTableStyle()

                ModifierDataGrid.ResumeLayout()

            End Using

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub ModifierDataGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ModifierDataGrid.DoubleClick
        Select Case CType(sender, DataGridCustom).LastHitSpot.Type
            Case Is = System.Windows.Forms.DataGrid.HitTestType.None

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell
                Me.DialogResult = DialogResult.OK
            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader
                Me.DialogResult = DialogResult.OK
            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ParentRows

        End Select

    End Sub

    Private Sub ModifierDataGrid_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ModifierDataGrid.MouseUp
        Dim DG As DataGrid = CType(sender, DataGrid)
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo
        HTI = DG.HitTest(e.X, e.Y)

        Try
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
                    ModifierDataGrid.Select(HTI.Row)
                    ''If Control.ModifierKeys <> Keys.Control AndAlso Control.ModifierKeys <> Keys.Shift Then
                    ''    Me.DialogResult = DialogResult.OK
                    ''Else
                    ''    'MultiTimer.Enabled = True
                    ''End If
                End If
            End If
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub ModifierDataGrid_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        Try
            If Char.IsLetterOrDigit(e.KeyChar) Then
                e.Handled = True

                If DateDiff("s", sbLastKeyCapturedTime, UFCWGeneral.NowDate) > 0 Then
                    sb = New StringBuilder
                End If

                sb.Append(e.KeyChar.ToString())
                sbLastKeyCapturedTime = UFCWGeneral.NowDate
            Else
                sb = New StringBuilder
            End If

            If sb.Length > 0 Then

                Try
                    Dim DT As DataTable = ModifierValuesDataSet.MODIFIER_VALUES ''ModifierDataGrid.DataSource

                    Debug.WriteLine("Selecting " & "MODIFIER_VALUE LIKE '" & sb.ToString.ToUpper & "%'")

                    dt.DefaultView.Sort = "MODIFIER_VALUE"

                    foundRow = dt.Select("MODIFIER_VALUE LIKE '" & sb.ToString.ToUpper & "%'", "MODIFIER_VALUE")

                    If foundRow.Length > 0 Then
                        BindingContext(dt).Position = dt.DefaultView.Find(foundRow(0)("MODIFIER_VALUE"))
                        ModifierDataGrid.MoveGridToRow(BindingContext(dt).Position)
                        ModifierDataGrid.Select(1)
                    End If
                Catch ex As Exception
                    Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
                    If (rethrow) Then
                        Throw
                    End If
                End Try
            End If

            If Asc(e.KeyChar) = Keys.Enter Then ModifierDataGrid.Find(sb.ToString())

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub
End Class