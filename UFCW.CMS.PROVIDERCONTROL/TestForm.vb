Public Class TestForm
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
    Friend WithEvents ButtonRefresh As System.Windows.Forms.Button
    Friend WithEvents ProviderID As System.Windows.Forms.TextBox
    Friend WithEvents ProviderControl As ProviderControl
    Friend WithEvents NPI As System.Windows.Forms.TextBox
    Friend WithEvents NpiRegistryControl As NPIRegistryControl
    Friend WithEvents Button1 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.ProviderID = New System.Windows.Forms.TextBox
        Me.ButtonRefresh = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.ProviderControl = New ProviderControl
        Me.NPI = New System.Windows.Forms.TextBox
        Me.NpiRegistryControl = New NPIRegistryControl
        Me.SuspendLayout()
        '
        'ProviderID
        '
        Me.ProviderID.Location = New System.Drawing.Point(93, 222)
        Me.ProviderID.Name = "ProviderID"
        Me.ProviderID.Size = New System.Drawing.Size(96, 20)
        Me.ProviderID.TabIndex = 1
        Me.ProviderID.Text = "90558"
        '
        'ButtonRefresh
        '
        Me.ButtonRefresh.Location = New System.Drawing.Point(184, 257)
        Me.ButtonRefresh.Name = "ButtonRefresh"
        Me.ButtonRefresh.Size = New System.Drawing.Size(96, 24)
        Me.ButtonRefresh.TabIndex = 3
        Me.ButtonRefresh.Text = "Refresh"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(184, 297)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(96, 24)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Clear"
        '
        'ProviderControl
        '
        Me.ProviderControl.Location = New System.Drawing.Point(8, 8)
        Me.ProviderControl.Name = "ProviderControl"
        Me.ProviderControl.Size = New System.Drawing.Size(448, 208)
        Me.ProviderControl.TabIndex = 4
        Me.ProviderControl.TaxID = 0
        '
        'NPI
        '
        Me.NPI.Location = New System.Drawing.Point(274, 222)
        Me.NPI.Name = "NPI"
        Me.NPI.Size = New System.Drawing.Size(96, 20)
        Me.NPI.TabIndex = 7
        Me.NPI.Text = "1457352494"
        '
        'NpiRegistryControl
        '
        Me.NpiRegistryControl.Location = New System.Drawing.Point(452, 8)
        Me.NpiRegistryControl.Name = "NpiRegistryControl"
        Me.NpiRegistryControl.Size = New System.Drawing.Size(524, 737)
        Me.NpiRegistryControl.TabIndex = 8
        '
        'TestForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(1056, 834)
        Me.Controls.Add(Me.NpiRegistryControl)
        Me.Controls.Add(Me.NPI)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ProviderControl)
        Me.Controls.Add(Me.ButtonRefresh)
        Me.Controls.Add(Me.ProviderID)
        Me.Name = "TestForm"
        Me.Text = "TestForm"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub Refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRefresh.Click

        Me.ProviderControl.ProviderID = CType(Me.ProviderID.Text, Integer)
        Me.NpiRegistryControl.NPI = CType(If(Me.NPI.Text.ToString.Trim.Length > 0, CDec(Me.NPI.Text.ToString.Trim), 0), Integer)

        ProviderControl.LoadProvider()
        NPIRegistryControl.LoadNPI()

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        ProviderControl.ClearProvider()

    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NPI.TextChanged

    End Sub

    Private Sub TestForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class

