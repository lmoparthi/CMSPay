Imports System.ComponentModel
Public Class InsurerPayerDialog
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    'Public Event Save(ByVal dt As AnnotationDataSet.ANNOTATIONSDataTable)
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
    Friend WithEvents PayerFreeTextBox As System.Windows.Forms.TextBox
    Friend WithEvents DialogCancelButton As System.Windows.Forms.Button
    Friend WithEvents DialogSaveButton As System.Windows.Forms.Button
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    Public Overloads Sub Dispose()
        MyBase.Dispose()
    End Sub
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    'Friend WithEvents EligibilityControl1 As UFCW.CMS.Eligibility.EligibilityControl
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.PayerFreeTextBox = New System.Windows.Forms.TextBox()
        Me.DialogCancelButton = New System.Windows.Forms.Button()
        Me.DialogSaveButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'PayerFreeTextBox
        '
        Me.PayerFreeTextBox.Dock = System.Windows.Forms.DockStyle.Top
        Me.PayerFreeTextBox.Location = New System.Drawing.Point(0, 0)
        Me.PayerFreeTextBox.MaxLength = 255
        Me.PayerFreeTextBox.Multiline = True
        Me.PayerFreeTextBox.Name = "PayerFreeTextBox"
        Me.PayerFreeTextBox.Size = New System.Drawing.Size(330, 68)
        Me.PayerFreeTextBox.TabIndex = 0
        '
        'DialogCancelButton
        '
        Me.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.DialogCancelButton.Location = New System.Drawing.Point(75, 77)
        Me.DialogCancelButton.Name = "DialogCancelButton"
        Me.DialogCancelButton.Size = New System.Drawing.Size(75, 23)
        Me.DialogCancelButton.TabIndex = 1
        Me.DialogCancelButton.Text = "Cancel"
        Me.DialogCancelButton.UseVisualStyleBackColor = True
        '
        'DialogSaveButton
        '
        Me.DialogSaveButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.DialogSaveButton.Location = New System.Drawing.Point(178, 77)
        Me.DialogSaveButton.Name = "DialogSaveButton"
        Me.DialogSaveButton.Size = New System.Drawing.Size(75, 23)
        Me.DialogSaveButton.TabIndex = 2
        Me.DialogSaveButton.Text = "Save"
        Me.DialogSaveButton.UseVisualStyleBackColor = True
        '
        'InsurerPayerDialog
        '
        Me.AcceptButton = Me.DialogSaveButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(330, 108)
        Me.Controls.Add(Me.DialogSaveButton)
        Me.Controls.Add(Me.DialogCancelButton)
        Me.Controls.Add(Me.PayerFreeTextBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "InsurerPayerDialog"
        Me.Text = "Insurer Payer Name"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
    Private _APPKEY As String = "UFCW\Claims\"

    Public Sub New(ByVal claimID As Integer, ByVal familyID As Integer, ByVal relationID As Integer, ByVal partSSN As Integer, ByVal patSSN As Integer, ByVal partFName As Object, ByVal partLName As Object, ByVal patFName As Object, ByVal patLName As Object, ByVal annotationDT As DataTable)
        Me.New()
    End Sub

    Private Sub Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DialogSaveButton.Click, DialogCancelButton.Click
        Me.Hide()
    End Sub

End Class