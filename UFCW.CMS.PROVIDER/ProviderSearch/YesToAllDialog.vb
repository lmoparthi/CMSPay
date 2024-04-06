Public Class YesToAllDialog
    Inherits System.Windows.Forms.Form

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
    Friend WithEvents Yes As System.Windows.Forms.Button
    Friend WithEvents YesToAll As System.Windows.Forms.Button
    Friend WithEvents Message As System.Windows.Forms.Label
    Friend WithEvents No As System.Windows.Forms.Button
    Friend WithEvents NoToAll As System.Windows.Forms.Button
    Friend WithEvents Pics As System.Windows.Forms.ImageList
    Friend WithEvents DispIcon As System.Windows.Forms.PictureBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(YesToAllDialog))
        Me.Yes = New System.Windows.Forms.Button()
        Me.YesToAll = New System.Windows.Forms.Button()
        Me.No = New System.Windows.Forms.Button()
        Me.NoToAll = New System.Windows.Forms.Button()
        Me.Message = New System.Windows.Forms.Label()
        Me.Pics = New System.Windows.Forms.ImageList(Me.components)
        Me.DispIcon = New System.Windows.Forms.PictureBox()
        Me.SuspendLayout()
        '
        'Yes
        '
        Me.Yes.Anchor = (System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)
        Me.Yes.Location = New System.Drawing.Point(8, 64)
        Me.Yes.Name = "Yes"
        Me.Yes.TabIndex = 2
        Me.Yes.Text = "&Yes"
        '
        'YesToAll
        '
        Me.YesToAll.Anchor = (System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)
        Me.YesToAll.Location = New System.Drawing.Point(88, 64)
        Me.YesToAll.Name = "YesToAll"
        Me.YesToAll.TabIndex = 3
        Me.YesToAll.Text = "Yes To &All"
        '
        'No
        '
        Me.No.Anchor = (System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)
        Me.No.Location = New System.Drawing.Point(168, 64)
        Me.No.Name = "No"
        Me.No.TabIndex = 4
        Me.No.Text = "&No"
        '
        'NoToAll
        '
        Me.NoToAll.Anchor = (System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)
        Me.NoToAll.Location = New System.Drawing.Point(248, 64)
        Me.NoToAll.Name = "NoToAll"
        Me.NoToAll.TabIndex = 5
        Me.NoToAll.Text = "No To A&ll"
        '
        'Message
        '
        Me.Message.Location = New System.Drawing.Point(56, 16)
        Me.Message.Name = "Message"
        Me.Message.Size = New System.Drawing.Size(264, 40)
        Me.Message.TabIndex = 1
        Me.Message.Text = "MESSAGE"
        '
        'Pics
        '
        Me.Pics.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.Pics.ImageSize = New System.Drawing.Size(32, 32)
        Me.Pics.ImageStream = CType(resources.GetObject("Pics.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.Pics.TransparentColor = System.Drawing.Color.Transparent
        '
        'DispIcon
        '
        Me.DispIcon.Location = New System.Drawing.Point(8, 16)
        Me.DispIcon.Name = "DispIcon"
        Me.DispIcon.Size = New System.Drawing.Size(32, 24)
        Me.DispIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.DispIcon.TabIndex = 6
        Me.DispIcon.TabStop = False
        '
        'YesToAllDialog
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(330, 98)
        Me.ControlBox = False
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.DispIcon, Me.NoToAll, Me.No, Me.YesToAll, Me.Yes, Me.Message})
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "YesToAllDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Caption Text"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Shadows Function ShowDialog(ByVal owner As System.Windows.Forms.IWin32Window, ByVal MessageText As String, ByVal CaptionText As String) As DialogResult
        Me.Text = CaptionText
        Message.Text = MessageText
        MyBase.ShowDialog(owner)
        Return Me.DialogResult
    End Function

    Shadows Function ShowDialog(ByVal MessageText As String, ByVal CaptionText As String) As Object
        Me.Text = CaptionText
        Message.Text = MessageText
        Dim messicon As New MessageBoxIcon()

        MyBase.ShowDialog(Me.Owner)
        Return Me.DialogResult
    End Function

    Shadows Function Show(ByVal MessageText As String, ByVal CaptionText As String) As DialogResult
        Me.Text = CaptionText
        Message.Text = MessageText
        MyBase.ShowDialog(Me.Owner)
        Return Me.DialogResult
    End Function

    Private Sub Yes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Yes.Click
        Me.DialogResult = DialogResult.Yes
        Me.Close()
    End Sub

    Private Sub YesToAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles YesToAll.Click
        Me.DialogResult = DialogResult.Retry
        Me.Close()
    End Sub

    Private Sub No_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles No.Click
        Me.DialogResult = DialogResult.No
        Me.Close()
    End Sub

    Private Sub NoToAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NoToAll.Click
        Me.DialogResult = DialogResult.Abort
        Me.Close()
    End Sub

    Private Sub YesToAllDialog_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DispIcon.Image = Pics.Images(0)
    End Sub
End Class
