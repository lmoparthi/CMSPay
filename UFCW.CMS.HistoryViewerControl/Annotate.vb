Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data.Common

Imports System.Data
Imports System.Configuration

Public Class Annotate
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
    Private _DefaultProviderName As String = CType(ConfigurationManager.GetSection("dataConfiguration"), Microsoft.Practices.EnterpriseLibrary.Data.Configuration.DatabaseSettings).DefaultDatabase

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
    Friend WithEvents Annotation As System.Windows.Forms.TextBox
    Friend WithEvents Cancel As System.Windows.Forms.Button
    Friend WithEvents AnnotationGrid As System.Windows.Forms.DataGrid
    Friend WithEvents Save As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Annotate))
        Me.Annotation = New System.Windows.Forms.TextBox
        Me.Cancel = New System.Windows.Forms.Button
        Me.Save = New System.Windows.Forms.Button
        Me.AnnotationGrid = New System.Windows.Forms.DataGrid
        CType(Me.AnnotationGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Annotation
        '
        Me.Annotation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Annotation.Location = New System.Drawing.Point(8, 9)
        Me.Annotation.MaxLength = 255
        Me.Annotation.Multiline = True
        Me.Annotation.Name = "Annotation"
        Me.Annotation.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.Annotation.Size = New System.Drawing.Size(383, 133)
        Me.Annotation.TabIndex = 0
        '
        'Cancel
        '
        Me.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel.Location = New System.Drawing.Point(8, 149)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Cancel.TabIndex = 1
        Me.Cancel.Text = "&Cancel"
        '
        'Save
        '
        Me.Save.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Save.Enabled = False
        Me.Save.Location = New System.Drawing.Point(316, 149)
        Me.Save.Name = "Save"
        Me.Save.Size = New System.Drawing.Size(75, 23)
        Me.Save.TabIndex = 2
        Me.Save.Text = "&Save"
        '
        'AnnotationGrid
        '
        Me.AnnotationGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AnnotationGrid.BackgroundColor = System.Drawing.Color.White
        Me.AnnotationGrid.DataMember = ""
        Me.AnnotationGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.AnnotationGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.AnnotationGrid.Location = New System.Drawing.Point(8, 178)
        Me.AnnotationGrid.Name = "AnnotationGrid"
        Me.AnnotationGrid.ReadOnly = True
        Me.AnnotationGrid.Size = New System.Drawing.Size(383, 173)
        Me.AnnotationGrid.TabIndex = 13
        '
        'Annotate
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.Cancel
        Me.ClientSize = New System.Drawing.Size(399, 368)
        Me.Controls.Add(Me.AnnotationGrid)
        Me.Controls.Add(Me.Save)
        Me.Controls.Add(Me.Cancel)
        Me.Controls.Add(Me.Annotation)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(200, 152)
        Me.Name = "Annotate"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Annotate"
        CType(Me.AnnotationGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region "Properties"
    Private DocID As String = ""
    Private SSN As String = ""
    Private DocClass As String = ""
    Private DocType As String = ""
    Private UserID As String = SystemInformation.UserName
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

#End Region

    Sub New(ByVal documentID As String, ByVal ssn As String, ByVal docClass As String, ByVal docType As String)
        Me.New()

        Try

            DocID = documentID
            Me.SSN = ssn
            Me.DocClass = docClass
            Me.DocType = docType

            Me.Text = "Annotate " & DocID
            loadAnnotations()
            Me.DialogResult = DialogResult.Cancel

            SetSettings()

        Catch ex As Exception

            Throw

        End Try

    End Sub

    Private Sub SetSettings()
        Me.Top = CInt(GetSetting(_APPKEY, "Annotate\Settings", "Top", CStr(Me.Top)))
        Me.Height = CInt(GetSetting(_APPKEY, "Annotate\Settings", "Height", CStr(Me.Height)))
        Me.Left = CInt(GetSetting(_APPKEY, "Annotate\Settings", "Left", CStr(Me.Left)))
        Me.Width = CInt(GetSetting(_APPKEY, "Annotate\Settings", "Width", CStr(Me.Width)))
        Me.WindowState = CType(GetSetting(_APPKEY, "Annotate\Settings", "WindowState", CStr(Me.WindowState)), FormWindowState)
    End Sub

    Public Sub SaveSettings()
        Dim WindowState As Integer = Me.WindowState

        SaveSetting(_APPKEY, "Annotate\Settings", "WindowState", CStr(WindowState))
        Me.WindowState = 0
        SaveSetting(_APPKEY, "Annotate\Settings", "Top", CStr(Me.Top))
        SaveSetting(_APPKEY, "Annotate\Settings", "Height", CStr(Me.Height))
        SaveSetting(_APPKEY, "Annotate\Settings", "Left", CStr(Me.Left))
        SaveSetting(_APPKEY, "Annotate\Settings", "Width", CStr(Me.Width))
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Save.Click

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String

        Try
            DB = CMSDALCommon.CreateDatabase($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance")

            SQLCall = "INSERT INTO History  (ProcessDateTime, DocumentID, SSN, Detail,  HistoryText, Code, DocClass, DocType, userid) " &
                                    "VALUES  (GETDATE(), '" & DocID & "', '" & UnFormatSSN(SSN) & "', '" & Replace(Annotation.Text, "'", "''") & "', '" & Replace(Annotation.Text, "'", "''") & "', '`','" & DocClass.Trim & "', '" & DocType.Trim & "', '" & UserID & "')"

            DBCommandWrapper = DB.GetSqlStringCommand(SQLCall)
            DBCommandWrapper.CommandTimeout = 180

            DB.ExecuteNonQuery(DBCommandWrapper)

            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception

            Throw
        End Try
    End Sub

    Private Sub Annotation_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Annotation.TextChanged
        If Annotation.Text <> "" Then
            Save.Enabled = True
        Else
            Save.Enabled = False
        End If
    End Sub

    Private Sub Annotate_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        SaveSettings()
    End Sub

    Private Sub Annotate_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetSettings()
    End Sub

    Private Sub loadAnnotations(Optional ByVal dt As DataTable = Nothing)
        Dim DS As DataSet : Dim AnnotateDT As DataTable = Nothing

        Dim SQLCall As String = "SELECT SQLTranID, TranID, ProcessDateTime, TransactionType, DocumentID, SSN, SSNAlias," &
                                   "HistoryText, Detail, Code, DocClass, DocType, userid, lastupdt, CompleteDate FROM History " &
                                   " Where DocumentID = '" & DocID & "'" &
                                   " and code='`'  "

        Dim DB As Database = CMSDALCommon.CreateDatabase($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance")
        Dim DBCommandWrapper As DbCommand

        Try
            If dt Is Nothing Then  '' from annotate menu option
                DBCommandWrapper = DB.GetSqlStringCommand(SQLCall)
                DBCommandWrapper.CommandTimeout = 120
                DS = DB.ExecuteDataSet(DBCommandWrapper)
                If DS.Tables.Count > 0 Then
                    AnnotateDT = DS.Tables(0)
                End If

            Else  '' from history form

                Dim DR As DataRow() = dt.Select("Code ='`' ", "ProcessDateTime")

                For Each dr1 As DataRow In DR
                    AnnotateDT.ImportRow(dr1)
                Next
            End If

            AnnotationGrid.DataSource = AnnotateDT

            SetTableStyle(AnnotateDT)

        Catch ex As Exception

            Throw

        End Try
    End Sub

    Private Sub SetTableStyle(ByVal DT As DataTable)

        Dim TextCol As New DataGridTextBoxColumn()
        Dim CurMan As CurrencyManager
        Dim DGTS As New DataGridTableStyle()

        Try
            CurMan = CType(Me.BindingContext(DT), CurrencyManager)

            DGTS = New DataGridTableStyle(CurMan) With {
                .MappingName = DT.TableName
            }
            DGTS.GridColumnStyles.Clear()
            DGTS.GridLineStyle = DataGridLineStyle.None

            TextCol = New DataGridTextBoxColumn With {
                .MappingName = "HistoryText"
            }
            TextCol.Width = CInt(GetSetting(AppKey, "Annotate\ColumnSettings\" & DT.TableName, "Col " & TextCol.MappingName, "100"))
            TextCol.HeaderText = "Summary"
            TextCol.NullText = ""
            DGTS.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridTextBoxColumn With {
                .MappingName = "UserID"
            }
            TextCol.Width = CInt(GetSetting(AppKey, "Annotate\ColumnSettings\" & DT.TableName, "Col " & TextCol.MappingName, "100"))
            TextCol.HeaderText = "Who"
            TextCol.NullText = ""
            DGTS.GridColumnStyles.Add(TextCol)

            TextCol = New DataGridTextBoxColumn With {
                .MappingName = "ProcessDateTime"
            }
            TextCol.Width = CInt(GetSetting(AppKey, "Annotate\ColumnSettings\" & DT.TableName, "Col " & TextCol.MappingName, "100"))
            TextCol.HeaderText = "When"
            TextCol.NullText = ""
            TextCol.Format = "MM-dd-yyyy hh:mm:ss tt"
            DGTS.GridColumnStyles.Add(TextCol)

            AnnotationGrid.TableStyles.Clear()
            AnnotationGrid.TableStyles.Add(DGTS)

        Catch ex As Exception

            Throw

        Finally

            If TextCol IsNot Nothing Then TextCol.Dispose()
            TextCol = Nothing

            If DGTS IsNot Nothing Then DGTS.Dispose()
            DGTS = Nothing
        End Try
    End Sub

    Public Shared Function UnFormatSSN(ByVal strSSN As String) As String
        If Replace(Replace(Replace(strSSN, " ", ""), "-", ""), "/", "") <> "" Then
            Return Format(CInt(Replace(Replace(Replace(strSSN, " ", ""), "-", ""), "/", "")), "0########")
        Else
            Return ""
        End If
    End Function
    Public Shared Function FormatSSN(ByVal strSSN As String) As String
        Dim StrTemp As String

        StrTemp = UnFormatSSN(strSSN)
        If StrTemp.Trim <> "" Then
            Return Microsoft.VisualBasic.Left(StrTemp, 3) & "-" & Microsoft.VisualBasic.Mid(StrTemp, 4, 2) & "-" & Microsoft.VisualBasic.Right(StrTemp, 4)
        Else
            Return ""
        End If
    End Function

End Class