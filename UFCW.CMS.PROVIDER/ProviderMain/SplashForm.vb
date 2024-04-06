Imports System.Reflection
Imports System.Diagnostics
Imports System.IO

Public Class SplashForm
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
    Friend WithEvents VersionLabel As System.Windows.Forms.Label
    Friend WithEvents CopyrightLabel As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(SplashForm))
        Me.VersionLabel = New System.Windows.Forms.Label
        Me.CopyrightLabel = New System.Windows.Forms.Label
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'VersionLabel
        '
        Me.VersionLabel.BackColor = System.Drawing.Color.FromArgb(CType(199, Byte), CType(203, Byte), CType(206, Byte))
        Me.VersionLabel.Location = New System.Drawing.Point(264, 248)
        Me.VersionLabel.Name = "VersionLabel"
        Me.VersionLabel.Size = New System.Drawing.Size(184, 24)
        Me.VersionLabel.TabIndex = 0
        Me.VersionLabel.Text = "Version"
        Me.VersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CopyrightLabel
        '
        Me.CopyrightLabel.BackColor = System.Drawing.Color.FromArgb(CType(199, Byte), CType(203, Byte), CType(206, Byte))
        Me.CopyrightLabel.Location = New System.Drawing.Point(264, 272)
        Me.CopyrightLabel.Name = "CopyrightLabel"
        Me.CopyrightLabel.Size = New System.Drawing.Size(184, 16)
        Me.CopyrightLabel.TabIndex = 1
        Me.CopyrightLabel.Text = "copyright"
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 3000
        '
        'SplashForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Info
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.ClientSize = New System.Drawing.Size(448, 344)
        Me.Controls.Add(Me.CopyrightLabel)
        Me.Controls.Add(Me.VersionLabel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SplashForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SplashForm"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Form and Control Events"

    Private Sub SplashForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim copyright As AssemblyCopyrightAttribute = CType(AssemblyCopyrightAttribute.GetCustomAttribute(Reflection.Assembly.GetExecutingAssembly(), GetType(AssemblyCopyrightAttribute)), AssemblyCopyrightAttribute)
            Dim description As AssemblyDescriptionAttribute = CType(AssemblyDescriptionAttribute.GetCustomAttribute(System.Reflection.Assembly.GetExecutingAssembly(), GetType(AssemblyDescriptionAttribute)), AssemblyDescriptionAttribute)

            ' set labels 'copyright
            Me.CopyrightLabel.Text = "Copyright: " & copyright.Copyright
            Me.VersionLabel.Text = "Version: " & Application.ProductVersion
        Catch ex As Exception

        End Try
        

    End Sub


    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        Me.Close()
    End Sub
#End Region

End Class
