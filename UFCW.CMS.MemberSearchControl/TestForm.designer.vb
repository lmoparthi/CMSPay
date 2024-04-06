<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TestForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.MemberSearchControl1 = New MemberSearchControl
        Me.SuspendLayout()
        '
        'MemberSearchControl1
        '
        Me.MemberSearchControl1.AppKey = "UFCW\Medical\"
        Me.MemberSearchControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MemberSearchControl1.Location = New System.Drawing.Point(0, 0)
        Me.MemberSearchControl1.Name = "MemberSearchControl1"
        Me.MemberSearchControl1.ReturnRestricted = False
        Me.MemberSearchControl1.Size = New System.Drawing.Size(733, 492)
        Me.MemberSearchControl1.TabIndex = 0
        '
        'TestForm
        '
        Me.ClientSize = New System.Drawing.Size(733, 492)
        Me.Controls.Add(Me.MemberSearchControl1)
        Me.Name = "TestForm"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox10 As System.Windows.Forms.GroupBox
    Private WithEvents NotesRelationIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label82 As System.Windows.Forms.Label
    Private WithEvents NotesPatSSNTextBox As ExTextBox
    Friend WithEvents Label83 As System.Windows.Forms.Label
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Private WithEvents NotesFamilyIDTextBox As ExTextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Private WithEvents NotesPartSSNTextBox As ExTextBox
    Friend WithEvents Label72 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents MemberSearchControl1 As MemberSearchControl
End Class
