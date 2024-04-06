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
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents HoursControl1 As HoursControl
    Friend WithEvents EligibilityHoursControl As EligibilityHoursControl
    Friend WithEvents FAMILYID As System.Windows.Forms.TextBox

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.ButtonRefresh = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.FAMILYID = New System.Windows.Forms.TextBox
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.HoursControl1 = New HoursControl
        Me.EligibilityHoursControl = New EligibilityHoursControl
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonRefresh
        '
        Me.ButtonRefresh.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ButtonRefresh.Location = New System.Drawing.Point(7, 12)
        Me.ButtonRefresh.Name = "ButtonRefresh"
        Me.ButtonRefresh.Size = New System.Drawing.Size(96, 24)
        Me.ButtonRefresh.TabIndex = 3
        Me.ButtonRefresh.Text = "Refresh"
        '
        'Button1
        '
        Me.Button1.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Button1.Location = New System.Drawing.Point(140, 12)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(96, 24)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Clear"
        '
        'FAMILYID
        '
        Me.FAMILYID.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.FAMILYID.Location = New System.Drawing.Point(272, 15)
        Me.FAMILYID.Name = "FAMILYID"
        Me.FAMILYID.Size = New System.Drawing.Size(112, 20)
        Me.FAMILYID.TabIndex = 8
        Me.FAMILYID.Text = "639930"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(7, 42)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.HoursControl1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.EligibilityHoursControl)
        Me.SplitContainer1.Size = New System.Drawing.Size(734, 588)
        Me.SplitContainer1.SplitterDistance = 294
        Me.SplitContainer1.TabIndex = 14
        '
        'HoursControl1
        '
        Me.HoursControl1.AppKey = "UFCW\Hours\"
        Me.HoursControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HoursControl1.Location = New System.Drawing.Point(0, 0)
        Me.HoursControl1.Name = "HoursControl1"
        Me.HoursControl1.Size = New System.Drawing.Size(734, 294)
        Me.HoursControl1.TabIndex = 14
        '
        'EligibilityHoursControl
        '
        Me.EligibilityHoursControl.AppKey = "UFCW\Hours\"
        Me.EligibilityHoursControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EligibilityHoursControl.Location = New System.Drawing.Point(0, 0)
        Me.EligibilityHoursControl.Name = "EligibilityHoursControl"
        Me.EligibilityHoursControl.Size = New System.Drawing.Size(734, 290)
        Me.EligibilityHoursControl.TabIndex = 0
        '
        'TestForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(753, 631)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.FAMILYID)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ButtonRefresh)
        Me.Name = "TestForm"
        Me.Text = "TestForm"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub Refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRefresh.Click

        Me.HoursControl1.LoadHours(CType(Me.FAMILYID.Text.ToString, Integer))
        Me.EligibilityHoursControl.LoadHours(CType(Me.FAMILYID.Text.ToString, Integer))

    End Sub

End Class