Option Strict On
Public Class RegMasterDetailViewerForm
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _Summary As String
    Private _Detail As String
    Private _User As String
    Private _Date As DateTime

    Const _APPKEY As String = "UFCW\RegMaster\"

#Region " Windows Form Designer generated code "
    Public Sub New(ByVal FamilyID As Integer, ByVal Summary As String, ByVal Detail As String, ByVal User As String, ByVal CreateDate As DateTime)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _Summary = Summary
        _Detail = Detail
        _User = User
        _Date = CreateDate

        Me.Text = FamilyID & " History Detail"
        SummaryTextBox.Text = Summary
        DetailTextBox.Text = Detail
        UserTextBox.Text = User
        DateTextBox.Text = CStr(CreateDate)
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
    Friend WithEvents DetailTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents DateTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents UserTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents SummaryTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(RegMasterDetailViewerForm))
        Me.DetailTextBox = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.DateTextBox = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.UserTextBox = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.SummaryTextBox = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'DetailTextBox
        '
        Me.DetailTextBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DetailTextBox.BackColor = System.Drawing.Color.White
        Me.DetailTextBox.Location = New System.Drawing.Point(16, 119)
        Me.DetailTextBox.Multiline = True
        Me.DetailTextBox.Name = "DetailTextBox"
        Me.DetailTextBox.ReadOnly = True
        Me.DetailTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.DetailTextBox.Size = New System.Drawing.Size(328, 136)
        Me.DetailTextBox.TabIndex = 23
        Me.DetailTextBox.Text = ""
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(8, 103)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(36, 16)
        Me.Label4.TabIndex = 22
        Me.Label4.Text = "Detail:"
        '
        'DateTextBox
        '
        Me.DateTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DateTextBox.BackColor = System.Drawing.Color.White
        Me.DateTextBox.Location = New System.Drawing.Point(216, 71)
        Me.DateTextBox.Name = "DateTextBox"
        Me.DateTextBox.ReadOnly = True
        Me.DateTextBox.Size = New System.Drawing.Size(128, 20)
        Me.DateTextBox.TabIndex = 21
        Me.DateTextBox.Text = ""
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(212, 56)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 16)
        Me.Label3.TabIndex = 20
        Me.Label3.Text = "Date:"
        '
        'UserTextBox
        '
        Me.UserTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UserTextBox.BackColor = System.Drawing.Color.White
        Me.UserTextBox.Location = New System.Drawing.Point(16, 71)
        Me.UserTextBox.Name = "UserTextBox"
        Me.UserTextBox.ReadOnly = True
        Me.UserTextBox.Size = New System.Drawing.Size(192, 20)
        Me.UserTextBox.TabIndex = 19
        Me.UserTextBox.Text = ""
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(31, 16)
        Me.Label2.TabIndex = 18
        Me.Label2.Text = "User:"
        '
        'SummaryTextBox
        '
        Me.SummaryTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SummaryTextBox.BackColor = System.Drawing.Color.White
        Me.SummaryTextBox.Location = New System.Drawing.Point(16, 23)
        Me.SummaryTextBox.Name = "SummaryTextBox"
        Me.SummaryTextBox.ReadOnly = True
        Me.SummaryTextBox.Size = New System.Drawing.Size(328, 20)
        Me.SummaryTextBox.TabIndex = 17
        Me.SummaryTextBox.Text = ""
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 16)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Summary:"
        '
        'DetailViewer
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(352, 262)
        Me.Controls.Add(Me.DetailTextBox)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.DateTextBox)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.UserTextBox)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.SummaryTextBox)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "DetailViewer"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "0 History Detail"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub DetailViewer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetSettings()
    End Sub

    Private Sub DetailViewer_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        SaveSettings()
    End Sub

    Private Sub SetSettings()

        Me.Top = CInt(GetSetting(_APPKEY, "History\DetailViewer\Settings", "Top", CStr(Me.Top)))
        Me.Height = CInt(GetSetting(_APPKEY, "History\DetailViewer\Settings", "Height", CStr(Me.Height)))
        Me.Left = CInt(GetSetting(_APPKEY, "History\DetailViewer\Settings", "Left", CStr(Me.Left)))
        Me.Width = CInt(GetSetting(_APPKEY, "History\DetailViewer\Settings", "Width", CStr(Me.Width)))

    End Sub

    Private Sub SaveSettings()
        Dim WindowState As Integer = Me.WindowState
        Me.WindowState = 0
        SaveSetting(_APPKEY, "History\DetailViewer\Settings", "Top", CStr(Me.Top))
        SaveSetting(_APPKEY, "History\DetailViewer\Settings", "Height", CStr(Me.Height))
        SaveSetting(_APPKEY, "History\DetailViewer\Settings", "Left", CStr(Me.Left))
        SaveSetting(_APPKEY, "History\DetailViewer\Settings", "Width", CStr(Me.Width))
    End Sub
End Class
