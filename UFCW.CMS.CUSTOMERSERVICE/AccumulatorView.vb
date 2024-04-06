Imports System.Security.Principal
Public Class AccumulatorView
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

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
    Friend WithEvents AccumulatorValues1 As AccumulatorValues
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.AccumulatorValues1 = New AccumulatorValues
        Me.Button1 = New System.Windows.Forms.Button
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'AccumulatorValues1
        '
        Me.AccumulatorValues1.ReadOnly = True
        Me.AccumulatorValues1.IsInEditMode = False
        Me.AccumulatorValues1.Location = New System.Drawing.Point(0, 56)
        Me.AccumulatorValues1.MemberAccumulatorManager = Nothing
        Me.AccumulatorValues1.Name = "AccumulatorValues1"
        Me.AccumulatorValues1.ShowLineDetails = False
        Me.AccumulatorValues1.Size = New System.Drawing.Size(584, 368)
        Me.AccumulatorValues1.TabIndex = 0
        Me.AccumulatorValues1.UserID = ""
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(200, 16)
        Me.Button1.Name = "Button1"
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Search"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(80, 16)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.TabIndex = 2
        Me.TextBox1.Text = "1122766"
        '
        'AccumulatorView
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(368, 422)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.AccumulatorValues1)
        Me.Name = "AccumulatorView"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub AccumulatorView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.AccumulatorValues1.ShowLineDetails = False
        Me.AccumulatorValues1.IsInEditMode = True
        AppDomain.CurrentDomain.SetPrincipalPolicy(Security.Principal.PrincipalPolicy.WindowsPrincipal)
        Dim UserID As String = CType(System.Threading.Thread.CurrentPrincipal, WindowsPrincipal).Identity.Name.ToUpper
        Me.AccumulatorValues1.UserId = UserID
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.AccumulatorValues1.SetFormInfo(Me.TextBox1.Text, UFCWGeneral.NowDate)
    End Sub

    Private Sub AccumulatorView_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Me.AccumulatorValues1.Dispose()
        Me.AccumulatorValues1 = Nothing
    End Sub
End Class