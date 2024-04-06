
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data.Common
Imports System.Configuration

Public Class AnnotationViewer
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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents HistText As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents HistDetail As System.Windows.Forms.TextBox
    Friend WithEvents User As System.Windows.Forms.TextBox
    Friend WithEvents ProcDate As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(AnnotationViewer))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.HistText = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.User = New System.Windows.Forms.TextBox()
        Me.ProcDate = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.HistDetail = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Summary:"
        '
        'HistText
        '
        Me.HistText.Anchor = ((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right)
        Me.HistText.BackColor = System.Drawing.Color.White
        Me.HistText.Location = New System.Drawing.Point(16, 32)
        Me.HistText.Name = "HistText"
        Me.HistText.ReadOnly = True
        Me.HistText.Size = New System.Drawing.Size(248, 20)
        Me.HistText.TabIndex = 1
        Me.HistText.Text = ""
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 64)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(31, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "User:"
        '
        'User
        '
        Me.User.Anchor = ((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right)
        Me.User.BackColor = System.Drawing.Color.White
        Me.User.Location = New System.Drawing.Point(16, 80)
        Me.User.Name = "User"
        Me.User.ReadOnly = True
        Me.User.Size = New System.Drawing.Size(152, 20)
        Me.User.TabIndex = 3
        Me.User.Text = ""
        '
        'ProcDate
        '
        Me.ProcDate.Anchor = (System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right)
        Me.ProcDate.BackColor = System.Drawing.Color.White
        Me.ProcDate.Location = New System.Drawing.Point(184, 80)
        Me.ProcDate.Name = "ProcDate"
        Me.ProcDate.ReadOnly = True
        Me.ProcDate.Size = New System.Drawing.Size(80, 20)
        Me.ProcDate.TabIndex = 5
        Me.ProcDate.Text = ""
        '
        'Label3
        '
        Me.Label3.Anchor = (System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(184, 64)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Date:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(13, 112)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(36, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Detail:"
        '
        'HistDetail
        '
        Me.HistDetail.Anchor = (((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right)
        Me.HistDetail.BackColor = System.Drawing.Color.White
        Me.HistDetail.Location = New System.Drawing.Point(16, 128)
        Me.HistDetail.Multiline = True
        Me.HistDetail.Name = "HistDetail"
        Me.HistDetail.ReadOnly = True
        Me.HistDetail.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.HistDetail.Size = New System.Drawing.Size(248, 72)
        Me.HistDetail.TabIndex = 7
        Me.HistDetail.Text = ""
        '
        'AnnotationViewer
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(272, 213)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.HistDetail, Me.Label4, Me.ProcDate, Me.Label3, Me.User, Me.Label2, Me.HistText, Me.Label1})
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(280, 240)
        Me.Name = "AnnotationViewer"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "History Detail"
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Properties"
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

    Sub New(ByVal histID As String)
        Me.New()

        Dim DB As Database
        Dim DBCommandWrapper As DbCommand
        Dim SQLCall As String = "Select * From History Where SQLTranID = " & histID

        Try

            DB = CMSDALCommon.CreateDatabase($"ImgWorkflow SQ{If(_DefaultProviderName.Contains(" P "), "L", "1")} Database Instance")

            DBCommandWrapper = DB.GetSqlStringCommand(SQLCall)
            DBCommandWrapper.CommandTimeout = 180

            Using Rdr As Data.IDataReader = DB.ExecuteReader(DBCommandWrapper)
                Rdr.Read()

                If Not IsDBNull(Rdr.Item("HistoryText")) Then
                    HistText.Text = CStr(Rdr.Item("HistoryText"))
                End If

                If Not IsDBNull(Rdr.Item("userid")) Then
                    User.Text = Rdr.Item("userid").ToString
                End If

                If Not IsDBNull(Rdr.Item("ProcessDateTime")) Then
                    ProcDate.Text = Format(Rdr.Item("ProcessDateTime"), "MM-dd-yyyy")
                End If

                If Not IsDBNull(Rdr.Item("Detail")) Then
                    HistDetail.Text = Replace(Rdr.Item("Detail").ToString, "\n", Microsoft.VisualBasic.vbCrLf)
                End If

                Me.Text = Rdr.Item("DocumentID").ToString & " History Detail"

            End Using

        Catch ex As Exception

            Throw
        Finally

        End Try
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

    Private Sub AnnotationViewer_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        SaveSettings()
    End Sub

    Private Sub AnnotationViewer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetSettings()
    End Sub
End Class