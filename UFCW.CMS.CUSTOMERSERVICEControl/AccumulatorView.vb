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
    Friend WithEvents AccumulatorValues As AccumulatorValues
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.AccumulatorValues = New AccumulatorValues()
        Me.SuspendLayout()
        '
        'AccumulatorValues
        '
        Me.AccumulatorValues.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AccumulatorValues.BackColor = System.Drawing.SystemColors.Control
        Me.AccumulatorValues.IsInEditMode = False
        Me.AccumulatorValues.Location = New System.Drawing.Point(0, 0)
        Me.AccumulatorValues.MinimumSize = New System.Drawing.Size(200, 150)
        Me.AccumulatorValues.Name = "AccumulatorValues"
        Me.AccumulatorValues.ShowClaimView = False
        Me.AccumulatorValues.ShowHistory = False
        Me.AccumulatorValues.ShowLineDetails = False
        Me.AccumulatorValues.Size = New System.Drawing.Size(432, 364)
        Me.AccumulatorValues.TabIndex = 0
        '
        'AccumulatorView
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(376, 366)
        Me.Controls.Add(Me.AccumulatorValues)
        Me.Name = "AccumulatorView"
        Me.Text = "AccumulatorView"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub AccumulatorView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.AccumulatorValues.ShowLineDetails = False
        Me.AccumulatorValues.IsInEditMode = False
    End Sub

    'Private Sub AccumulatorValues_Resize(ByVal changedHeight As Integer) Handles AccumulatorValues.Resize
    '    Me.Height += changedHeight
    '    'Me.Width = AccumulatorValues.Width
    'End Sub

    Private Sub AccumulatorView_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Me.AccumulatorValues.Dispose()
        Me.AccumulatorValues = Nothing
    End Sub
End Class