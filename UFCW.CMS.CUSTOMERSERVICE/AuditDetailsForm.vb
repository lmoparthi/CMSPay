Public Class AuditDetailsForm
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
    Friend WithEvents AuditDetailsControl As AuditDetailsControl
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.AuditDetailsControl = New AuditDetailsControl()
        Me.SuspendLayout()
        '
        'AuditDetailsControl
        '
        Me.AuditDetailsControl.Adjuster = Nothing
        Me.AuditDetailsControl.ClaimID = Nothing
        Me.AuditDetailsControl.Cursor = System.Windows.Forms.Cursors.Default
        Me.AuditDetailsControl.Location = New System.Drawing.Point(0, 0)
        Me.AuditDetailsControl.MaximumSize = New System.Drawing.Size(817, 551)
        Me.AuditDetailsControl.MinimumSize = New System.Drawing.Size(817, 551)
        Me.AuditDetailsControl.Name = "AuditDetailsControl"
        Me.AuditDetailsControl.Size = New System.Drawing.Size(817, 551)
        Me.AuditDetailsControl.TabIndex = 0
        '
        'AuditDetailsForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(820, 550)
        Me.Controls.Add(Me.AuditDetailsControl)
        Me.Name = "AuditDetailsForm"
        Me.Text = "AuditDetailsForm"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Friend Property ClaimID() As String
        Get
            Return Me.AuditDetailsControl.ClaimID
        End Get
        Set(ByVal value As String)
            Me.AuditDetailsControl.ClaimID = value
        End Set
    End Property

    Friend Property Adjuster() As String
        Get
            Return Me.AuditDetailsControl.Adjuster
        End Get
        Set(ByVal value As String)
            Me.AuditDetailsControl.Adjuster = Value
        End Set
    End Property

    Private Sub AuditDetailsForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.ClaimId = "1234"
        'Me.Adjuster = "PTW"
        Dim ch As CheckBox() = Me.AuditDetailsControl.GetAllCheckBoxControls()
    End Sub

    Private Sub AuditDetailsControl1_Close(ByVal results As AuditDetailsControl.CloseStatus) Handles AuditDetailsControl.Close
        Me.AuditDetailsControl.Dispose()
        Me.AuditDetailsControl = Nothing
        Me.Close()
    End Sub
End Class