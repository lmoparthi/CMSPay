Public Class AccumulatorHistory
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
    Friend WithEvents AccumulatorHistoryCtrl As UFCW.CMS.AccumulatorControl.AccumulatorHistory
    Friend WithEvents OkButton As System.Windows.Forms.Button
    Private Sub InitializeComponent()
        Me.AccumulatorHistoryCtrl = New UFCW.CMS.AccumulatorControl.AccumulatorHistory
        Me.OkButton = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'AccumulatorHistoryCtrl
        '
        Me.AccumulatorHistoryCtrl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AccumulatorHistoryCtrl.AutoScroll = True
        Me.AccumulatorHistoryCtrl.Location = New System.Drawing.Point(0, 0)
        Me.AccumulatorHistoryCtrl.Name = "AccumulatorHistoryCtrl"
        Me.AccumulatorHistoryCtrl.Size = New System.Drawing.Size(456, 248)
        Me.AccumulatorHistoryCtrl.TabIndex = 2
        '
        'OkButton
        '
        Me.OkButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OkButton.Location = New System.Drawing.Point(360, 264)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(75, 23)
        Me.OkButton.TabIndex = 3
        Me.OkButton.Text = "OK"
        '
        'AccumulatorHistory
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(456, 294)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.AccumulatorHistoryCtrl)
        Me.Location = New System.Drawing.Point(4, 15)
        Me.Name = "AccumulatorHistory"
        Me.Text = "Accumulator History"
        Me.ResumeLayout(False)

    End Sub

#End Region


    Private Sub AccumulatorHistory_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        Me.AccumulatorHistoryCtrl.Dispose()
        Me.Dispose()
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OkButton.Click
        Me.Close()
    End Sub
End Class
