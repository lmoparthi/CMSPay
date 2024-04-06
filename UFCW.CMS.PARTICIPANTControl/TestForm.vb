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
    Friend WithEvents FamilyID As System.Windows.Forms.TextBox
    Friend WithEvents RelationID As System.Windows.Forms.TextBox
    Friend WithEvents PARTICIPANTInfo As PARTICIPANTControl
    Friend WithEvents ButtonRefresh As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.PARTICIPANTInfo = New PARTICIPANTControl
        Me.FamilyID = New System.Windows.Forms.TextBox
        Me.RelationID = New System.Windows.Forms.TextBox
        Me.ButtonRefresh = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'PARTICIPANTInfo
        '
        Me.PARTICIPANTInfo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PARTICIPANTInfo.FamilyID = -1
        Me.PARTICIPANTInfo.Location = New System.Drawing.Point(0, 8)
        Me.PARTICIPANTInfo.Name = "PARTICIPANTInfo"
        Me.PARTICIPANTInfo.RelationID = -1
        Me.PARTICIPANTInfo.Size = New System.Drawing.Size(360, 288)
        Me.PARTICIPANTInfo.TabIndex = 0
        '
        'FamilyID
        '
        Me.FamilyID.Location = New System.Drawing.Point(400, 64)
        Me.FamilyID.Name = "FamilyID"
        Me.FamilyID.Size = New System.Drawing.Size(128, 20)
        Me.FamilyID.TabIndex = 1
        Me.FamilyID.Text = "24887"
        '
        'RelationID
        '
        Me.RelationID.Location = New System.Drawing.Point(400, 88)
        Me.RelationID.Name = "RelationID"
        Me.RelationID.Size = New System.Drawing.Size(128, 20)
        Me.RelationID.TabIndex = 2
        Me.RelationID.Text = "0"
        '
        'ButtonRefresh
        '
        Me.ButtonRefresh.Location = New System.Drawing.Point(416, 120)
        Me.ButtonRefresh.Name = "ButtonRefresh"
        Me.ButtonRefresh.Size = New System.Drawing.Size(96, 24)
        Me.ButtonRefresh.TabIndex = 3
        Me.ButtonRefresh.Text = "Refresh"
        '
        'TestForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(600, 454)
        Me.Controls.Add(Me.ButtonRefresh)
        Me.Controls.Add(Me.FamilyID)
        Me.Controls.Add(Me.RelationID)
        Me.Controls.Add(Me.PARTICIPANTInfo)
        Me.Name = "TestForm"
        Me.Text = "TestForm"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRefresh.Click

        PARTICIPANTInfo.FamilyID = CType(Me.FamilyID.Text, Integer)
        PARTICIPANTInfo.RelationID = CType(Me.RelationID.Text, Integer)
        PARTICIPANTInfo.LoadPARTICIPANT()

    End Sub

End Class