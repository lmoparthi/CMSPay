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
    Friend WithEvents DocumentClass As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents CLAIMID As System.Windows.Forms.TextBox
    Friend WithEvents FAMILYID As System.Windows.Forms.TextBox
    Friend WithEvents RelationID As System.Windows.Forms.TextBox
    Friend WithEvents ProviderID As System.Windows.Forms.TextBox
    Friend WithEvents LettersHistoryControl As LETTERSHistoryControl
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents LettersControl As LETTERSControl
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.DocumentClass = New System.Windows.Forms.TextBox()
        Me.ButtonRefresh = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.CLAIMID = New System.Windows.Forms.TextBox()
        Me.FAMILYID = New System.Windows.Forms.TextBox()
        Me.RelationID = New System.Windows.Forms.TextBox()
        Me.ProviderID = New System.Windows.Forms.TextBox()
        Me.LettersControl = New LETTERSControl()
        Me.LettersHistoryControl = New LETTERSHistoryControl()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'DocumentClass
        '
        Me.DocumentClass.Location = New System.Drawing.Point(68, 15)
        Me.DocumentClass.Name = "DocumentClass"
        Me.DocumentClass.Size = New System.Drawing.Size(80, 20)
        Me.DocumentClass.TabIndex = 1
        Me.DocumentClass.Text = "ELIGIBILITY"
        '
        'ButtonRefresh
        '
        Me.ButtonRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonRefresh.Location = New System.Drawing.Point(541, 11)
        Me.ButtonRefresh.Name = "ButtonRefresh"
        Me.ButtonRefresh.Size = New System.Drawing.Size(96, 24)
        Me.ButtonRefresh.TabIndex = 3
        Me.ButtonRefresh.Text = "Refresh"
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(541, 43)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(96, 24)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Clear"
        '
        'CLAIMID
        '
        Me.CLAIMID.Location = New System.Drawing.Point(349, 43)
        Me.CLAIMID.Name = "CLAIMID"
        Me.CLAIMID.Size = New System.Drawing.Size(75, 20)
        Me.CLAIMID.TabIndex = 7
        '
        'FAMILYID
        '
        Me.FAMILYID.Location = New System.Drawing.Point(211, 15)
        Me.FAMILYID.Name = "FAMILYID"
        Me.FAMILYID.Size = New System.Drawing.Size(73, 20)
        Me.FAMILYID.TabIndex = 8
        '
        'RelationID
        '
        Me.RelationID.Location = New System.Drawing.Point(211, 43)
        Me.RelationID.Name = "RelationID"
        Me.RelationID.Size = New System.Drawing.Size(73, 20)
        Me.RelationID.TabIndex = 9
        '
        'ProviderID
        '
        Me.ProviderID.Location = New System.Drawing.Point(349, 15)
        Me.ProviderID.Name = "ProviderID"
        Me.ProviderID.Size = New System.Drawing.Size(100, 20)
        Me.ProviderID.TabIndex = 13
        '
        'LettersControl
        '
        Me.LettersControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LettersControl.LineOfBusiness = ""
        Me.LettersControl.Location = New System.Drawing.Point(8, 80)
        Me.LettersControl.Name = "LettersControl"
        Me.LettersControl.Size = New System.Drawing.Size(521, 432)
        Me.LettersControl.TabIndex = 16
        '
        'LettersHistoryControl
        '
        Me.LettersHistoryControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LettersHistoryControl.AppKey = "UFCW\Medical\"
        Me.LettersHistoryControl.Location = New System.Drawing.Point(8, 528)
        Me.LettersHistoryControl.Name = "LettersHistoryControl"
        Me.LettersHistoryControl.Size = New System.Drawing.Size(609, 216)
        Me.LettersHistoryControl.TabIndex = 15
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(290, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(46, 13)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Provider"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(154, 18)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(47, 13)
        Me.Label5.TabIndex = 21
        Me.Label5.Text = "FamilyID"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(154, 43)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(37, 13)
        Me.Label6.TabIndex = 22
        Me.Label6.Text = "Rel ID"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(9, 18)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(55, 13)
        Me.Label7.TabIndex = 23
        Me.Label7.Text = "Doc Class"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(290, 46)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(43, 13)
        Me.Label8.TabIndex = 24
        Me.Label8.Text = "ClaimID"
        '
        'TestForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(649, 766)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LettersControl)
        Me.Controls.Add(Me.LettersHistoryControl)
        Me.Controls.Add(Me.ProviderID)
        Me.Controls.Add(Me.RelationID)
        Me.Controls.Add(Me.FAMILYID)
        Me.Controls.Add(Me.CLAIMID)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ButtonRefresh)
        Me.Controls.Add(Me.DocumentClass)
        Me.MinimumSize = New System.Drawing.Size(665, 804)
        Me.Name = "TestForm"
        Me.Text = "TestForm"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub Refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRefresh.Click

        If Me.FAMILYID.Text.Trim.Length > 0 Then
            LettersControl.FamilyID = CInt(Me.FAMILYID.Text)
            LettersControl.RelationID = If(Me.RelationID.Text.Trim.Length < 1, Nothing, CType(Me.RelationID.Text, Short?))
            LettersControl.ClaimID = If(Me.CLAIMID.Text.Trim.Length < 1, Nothing, CType(Me.CLAIMID.Text, Integer?))
            LettersControl.ProviderID = If(Me.ProviderID.Text.Trim.Length < 1, Nothing, CType(Me.ProviderID.Text, Integer?))
            LettersControl.LoadLetters(Me.DocumentClass.Text)

            LettersHistoryControl.ClaimID = If(Me.CLAIMID.Text.Trim.Length < 1, Nothing, CType(Me.CLAIMID.Text, Integer?))
            LettersHistoryControl.FamilyID = If(Me.FAMILYID.Text.Trim.Length < 1, Nothing, CType(Me.FAMILYID.Text, Integer?))
            LettersHistoryControl.RelationID = If(Me.RelationID.Text.Trim.Length < 1, Nothing, CType(Me.RelationID.Text, Short?))

            Select Case Me.DocumentClass.Text.Trim.ToUpper
                Case "ELIGIBILITY", "MEDICAL", "PENSION"
                    If Me.FAMILYID.Text.Trim.Length > 0 Then LettersHistoryControl.LoadLettersHistory(CInt(Me.FAMILYID.Text), If(Me.RelationID.Text.Trim.Length < 1, Nothing, CType(Me.RelationID.Text, Short?)))
                Case "CLAIMS"
                    If Me.CLAIMID.Text.Trim.Length > 0 Then LettersHistoryControl.LoadLettersHistory(CInt(Me.CLAIMID.Text))
            End Select
        End If

    End Sub

End Class