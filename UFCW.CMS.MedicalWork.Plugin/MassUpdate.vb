
Public Class MassUpdateForm
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
    Private _MappingName As String

    Public Property FieldName() As String
        Get
            Return FieldNameLabel.Text.Replace(":", "").Trim
        End Get
        Set(ByVal value As String)
            FieldNameLabel.Text = value & ":"
        End Set
    End Property

    Public Property MappingName() As String
        Get
            Return _MappingName
        End Get
        Set(ByVal value As String)
            _MappingName = value
        End Set
    End Property


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
            If (components IsNot Nothing) Then
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
    Friend WithEvents UpdateAllButton As System.Windows.Forms.Button
    Friend WithEvents TheCancelButton As System.Windows.Forms.Button
    Friend WithEvents FieldNameLabel As System.Windows.Forms.Label
    Friend WithEvents FieldValue As System.Windows.Forms.TextBox
    Friend WithEvents ActionComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents PlanComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.UpdateAllButton = New System.Windows.Forms.Button()
        Me.TheCancelButton = New System.Windows.Forms.Button()
        Me.FieldNameLabel = New System.Windows.Forms.Label()
        Me.FieldValue = New System.Windows.Forms.TextBox()
        Me.ActionComboBox = New System.Windows.Forms.ComboBox()
        Me.PlanComboBox = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'UpdateAllButton
        '
        Me.UpdateAllButton.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.UpdateAllButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.UpdateAllButton.Location = New System.Drawing.Point(119, 72)
        Me.UpdateAllButton.Name = "UpdateAllButton"
        Me.UpdateAllButton.Size = New System.Drawing.Size(75, 23)
        Me.UpdateAllButton.TabIndex = 2
        Me.UpdateAllButton.Text = "&Update All"
        '
        'TheCancelButton
        '
        Me.TheCancelButton.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TheCancelButton.CausesValidation = False
        Me.TheCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.TheCancelButton.Location = New System.Drawing.Point(39, 72)
        Me.TheCancelButton.Name = "TheCancelButton"
        Me.TheCancelButton.Size = New System.Drawing.Size(75, 23)
        Me.TheCancelButton.TabIndex = 1
        Me.TheCancelButton.Text = "&Cancel"
        '
        'FieldNameLabel
        '
        Me.FieldNameLabel.AutoSize = True
        Me.FieldNameLabel.Location = New System.Drawing.Point(16, 4)
        Me.FieldNameLabel.Name = "FieldNameLabel"
        Me.FieldNameLabel.Size = New System.Drawing.Size(60, 13)
        Me.FieldNameLabel.TabIndex = 0
        Me.FieldNameLabel.Text = "FieldName:"
        '
        'FieldValue
        '
        Me.FieldValue.Location = New System.Drawing.Point(22, 24)
        Me.FieldValue.Name = "FieldValue"
        Me.FieldValue.Size = New System.Drawing.Size(136, 20)
        Me.FieldValue.TabIndex = 0
        '
        'ActionComboBox
        '
        Me.ActionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ActionComboBox.Items.AddRange(New Object() {"PAY", "DENY"})
        Me.ActionComboBox.Location = New System.Drawing.Point(22, 24)
        Me.ActionComboBox.Name = "ActionComboBox"
        Me.ActionComboBox.Size = New System.Drawing.Size(136, 21)
        Me.ActionComboBox.TabIndex = 3
        Me.ActionComboBox.Visible = False
        '
        'PlanComboBox
        '
        Me.PlanComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.PlanComboBox.Location = New System.Drawing.Point(22, 24)
        Me.PlanComboBox.Name = "PlanComboBox"
        Me.PlanComboBox.Size = New System.Drawing.Size(136, 21)
        Me.PlanComboBox.TabIndex = 4
        Me.PlanComboBox.Visible = False
        '
        'MassUpdate
        '
        Me.AcceptButton = Me.UpdateAllButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(211, 124)
        Me.ControlBox = False
        Me.Controls.Add(Me.FieldNameLabel)
        Me.Controls.Add(Me.TheCancelButton)
        Me.Controls.Add(Me.UpdateAllButton)
        Me.Controls.Add(Me.FieldValue)
        Me.Controls.Add(Me.ActionComboBox)
        Me.Controls.Add(Me.PlanComboBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(217, 130)
        Me.Name = "MassUpdate"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Update All:"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TheCancelButton.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub UpdateAllButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateAllButton.Click

        Me.DialogResult = DialogResult.OK
        Me.Close()

    End Sub

    Private Sub FieldValue_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles FieldValue.Validating

        Dim DisplayDate As Date?

        Select Case _MappingName
            Case Is = "PROC_CODE"

            Case Is = "MED_PLAN"
            Case Is = "MODIFIER"

            Case Is = "DIAGNOSIS", "DIAGNOSES"

            Case Is = "PLACE_OF_SERV"

            Case Is = "BILL_TYPE"

            Case Is = "OCC_FROM_DATE"

                DisplayDate = UFCWGeneral.ValidateDate(CType(sender, TextBox).Text.Trim)

                If DisplayDate Is Nothing Then

                    e.Cancel = True 'this will cancel the Validated event
                    Me.ActiveControl = CType(sender, TextBox)
                    CType(sender, TextBox).SelectAll()
                    MessageBox.Show(If(CType(sender, TextBox).Text.Trim.Length = 0, "Date is Invalid", "Invalid Date Specified"), "Invalid Date format", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Else
                    CType(sender, TextBox).Text = DisplayDate.Value.ToShortDateString
                End If

            Case Is = "OCC_TO_DATE"

                DisplayDate = UFCWGeneral.ValidateDate(CType(sender, TextBox).Text.Trim)

                If DisplayDate Is Nothing Then

                    e.Cancel = True 'this will cancel the Validated event
                    Me.ActiveControl = CType(sender, TextBox)
                    CType(sender, TextBox).SelectAll()
                    MessageBox.Show(If(CType(sender, TextBox).Text.Trim.Length = 0, "Date is Invalid", "Invalid Date Specified"), "Invalid Date format", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Else
                    CType(sender, TextBox).Text = DisplayDate.Value.ToShortDateString
                End If

            Case Is = "CHRG_AMT"
            Case Is = "PRICED_AMT"
            Case Is = "OTH_INS_AMT"
            Case Is = "PAID_AMT"
            Case Is = "DAYS_UNITS"

            Case Is = "STATUS"

            Case Is = "REASON_SW"

            Case Is = "Accumulators"

        End Select

    End Sub

    Private Sub MassUpdate_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        FieldValue.SelectionStart = 0
        FieldValue.SelectionLength = Len(FieldValue.Text)
        FieldValue.Focus()
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        Const WM_KEYDOWN As Integer = &H100
        ' Only Process Key Down events for the Escape Key
        If msg.Msg = WM_KEYDOWN Then
            If keyData = Keys.Escape Then
                ' .net comboBox doesn't claim this command key until
                ' later, so Escape won't close the dropped down part of 
                ' the comboBox until after the form's ProcessCmdKey runs.
                ' Check if the active control is a dropped down combo box.
                If Not IsDroppedDownComboBox(msg.HWnd) Then
                    DialogResult = DialogResult.Cancel
                    ' Could call cancel click handler here
                    ' Return true to indicate that Escape key was handled
                    Return True
                End If
            End If
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    Protected Function IsDroppedDownComboBox(theHandle As IntPtr) As Boolean
        ' For ProcessCmdKey, use window handle to check if we're 
        ' dealing with a dropped down ComboBox.
        Dim active As Control = Control.FromHandle(theHandle)
        If TypeOf active Is ComboBox Then
            Return TryCast(active, ComboBox).DroppedDown
        End If
        Return False
    End Function


End Class