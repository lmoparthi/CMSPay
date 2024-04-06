Public Class ManualAccumulators
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        AddHandler ManualAccumulatorValues.ManualAccumulatorsModified, AddressOf ManualAccumulatorValues_ManualAccumulatorsModified
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
    Friend WithEvents ManualAccumulatorValues As ManualAccumulatorValuesControl
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.ManualAccumulatorValues = New ManualAccumulatorValuesControl()
        Me.SuspendLayout()
        '
        'ManualAccumulatorValues
        '
        Me.ManualAccumulatorValues.IsInEditMode = False
        Me.ManualAccumulatorValues.Location = New System.Drawing.Point(0, 8)
        Me.ManualAccumulatorValues.Name = "ManualAccumulatorValues"
        Me.ManualAccumulatorValues.Size = New System.Drawing.Size(272, 368)
        Me.ManualAccumulatorValues.TabIndex = 0
        '
        'ManualAccumulators
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(272, 382)
        Me.Controls.Add(Me.ManualAccumulatorValues)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ManualAccumulators"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Manual Accumulator(s)"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub

    Public Event ManualAccumulatorsModified()

#End Region

    Private Sub ManualAccumulatorValues_ManualAccumulatorsModified()
        RaiseEvent ManualAccumulatorsModified()
    End Sub
End Class