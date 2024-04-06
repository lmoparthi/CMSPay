Public Class frmDataCapture
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _BookMarkText As String
    Private _ChangeAllowed As Boolean
    Private _InvalidCharacterEntered As Boolean = False
#Region " Windows Form Designer generated code "
    Public ReadOnly Property BookMarkText() As String
        Get
            BookMarkText = _BookMarkText
        End Get
    End Property

    Public Sub New(ByVal headerDescription As String, ByVal bookMarkText As String, ByVal changeAllowed As Boolean, ByVal outputColumn As String)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _BookMarkText = bookMarkText
        _ChangeAllowed = changeAllowed

        Me.Text = headerDescription.Trim

        Me.rtfDataCapture.Text = _BookMarkText
        Me.rtfDataCapture.Enabled = _ChangeAllowed

        If outputColumn.ToUpper.Contains("DATE") OrElse outputColumn.ToUpper.Contains("AMT") OrElse outputColumn.ToUpper.Contains("NUM") Then
            Me.rtfDataCapture.AcceptsTab = False
            Me.rtfDataCapture.Multiline = False
            Me.AcceptButton = DataCaptureAction
        End If

        If outputColumn.ToUpper.Contains("NUM") Then
            Me.rtfDataCapture.MaxLength = 9
            AddHandler rtfDataCapture.KeyDown, AddressOf rtfDataCapture_KeyDown_Number
        End If

        If outputColumn.ToUpper.Contains("AMT") Then
            Me.rtfDataCapture.MaxLength = 12
            AddHandler rtfDataCapture.KeyDown, AddressOf rtfDataCapture_KeyDown_Amt
        End If

        If outputColumn.ToUpper.Contains("DATE") Then
            Me.rtfDataCapture.MaxLength = 10
            AddHandler rtfDataCapture.KeyDown, AddressOf rtfDataCapture_KeyDown_Date
        End If

        AddHandler rtfDataCapture.VisibleChanged, AddressOf rtfDataCapture_VisibleChanged

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
    Friend WithEvents DataCaptureAction As System.Windows.Forms.Button
    Friend WithEvents rtfDataCapture As System.Windows.Forms.RichTextBox
    Friend WithEvents DataCaptureCancel As System.Windows.Forms.Button
    Friend WithEvents lblReplace As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.rtfDataCapture = New System.Windows.Forms.RichTextBox
        Me.DataCaptureAction = New System.Windows.Forms.Button
        Me.DataCaptureCancel = New System.Windows.Forms.Button
        Me.lblReplace = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'rtfDataCapture
        '
        Me.rtfDataCapture.AcceptsTab = True
        Me.rtfDataCapture.AllowDrop = True
        Me.rtfDataCapture.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtfDataCapture.AutoWordSelection = True
        Me.rtfDataCapture.HideSelection = False
        Me.rtfDataCapture.ImeMode = System.Windows.Forms.ImeMode.On
        Me.rtfDataCapture.Location = New System.Drawing.Point(0, 0)
        Me.rtfDataCapture.Name = "rtfDataCapture"
        Me.rtfDataCapture.Size = New System.Drawing.Size(388, 160)
        Me.rtfDataCapture.TabIndex = 0
        Me.rtfDataCapture.Text = ""
        '
        'DataCaptureAction
        '
        Me.DataCaptureAction.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataCaptureAction.Location = New System.Drawing.Point(296, 168)
        Me.DataCaptureAction.Name = "DataCaptureAction"
        Me.DataCaptureAction.Size = New System.Drawing.Size(88, 24)
        Me.DataCaptureAction.TabIndex = 1
        Me.DataCaptureAction.Text = "OK"
        '
        'DataCaptureCancel
        '
        Me.DataCaptureCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.DataCaptureCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.DataCaptureCancel.Location = New System.Drawing.Point(8, 168)
        Me.DataCaptureCancel.Name = "DataCaptureCancel"
        Me.DataCaptureCancel.Size = New System.Drawing.Size(88, 24)
        Me.DataCaptureCancel.TabIndex = 2
        Me.DataCaptureCancel.Text = "Cancel"
        '
        'lblReplace
        '
        Me.lblReplace.Location = New System.Drawing.Point(112, 168)
        Me.lblReplace.Name = "lblReplace"
        Me.lblReplace.Size = New System.Drawing.Size(168, 24)
        Me.lblReplace.TabIndex = 3
        '
        'frmDataCapture
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.DataCaptureCancel
        Me.ClientSize = New System.Drawing.Size(392, 206)
        Me.Controls.Add(Me.lblReplace)
        Me.Controls.Add(Me.DataCaptureCancel)
        Me.Controls.Add(Me.DataCaptureAction)
        Me.Controls.Add(Me.rtfDataCapture)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDataCapture"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub DataCaptureAction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataCaptureAction.Click

        Me.Hide()
        Me.DialogResult = DialogResult.OK

        _BookMarkText = Me.rtfDataCapture.Text

    End Sub

    Private Sub DataCaptureCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataCaptureCancel.Click

        Me.Hide()
        Me.DialogResult = DialogResult.Cancel

        _BookMarkText = ""

    End Sub

    Private Sub rtfDataCapture_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rtfDataCapture.TextChanged

        Dim R As System.Text.RegularExpressions.Regex
        Dim Matches As System.Text.RegularExpressions.MatchCollection

        R = New System.Text.RegularExpressions.Regex("@", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Matches = R.Matches(CType(sender, RichTextBox).Text)
        If Matches.Count > 0 Then
            lblReplace.Text = Matches.Count & " @'s need to be resolved."
        Else
            lblReplace.Text = ""
        End If
    End Sub

    Private Sub rtfDataCapture_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        RemoveHandler rtfDataCapture.VisibleChanged, AddressOf rtfDataCapture_VisibleChanged

        Dim R As System.Text.RegularExpressions.Regex
        Dim Matches As System.Text.RegularExpressions.MatchCollection

        R = New System.Text.RegularExpressions.Regex("@", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Matches = R.Matches(CType(sender, RichTextBox).Text)
        If Matches.Count > 0 Then
            lblReplace.Text = Matches.Count & " @'s need to be resolved."
        Else
            lblReplace.Text = ""
        End If

        ' HighLightSpecified(rtfDataCapture, "@", Color.Red)   ' LM commented out To make users Not show red color 3/22/2024

        rtfDataCapture.Find("@")

        AddHandler rtfDataCapture.VisibleChanged, AddressOf rtfDataCapture_VisibleChanged

    End Sub
    Public Sub HighLightSpecified(ByVal T As RichTextBox, ByVal Word As String, ByVal color1 As Color)
        Dim Pos As Integer = 0
        Dim S As String = T.Text
        Dim I As Integer = 0
        Dim StopWhile As Boolean = False

        While Not StopWhile
            Dim J As Integer = S.IndexOf(Word, I)
            If J < 0 Then
                StopWhile = True
            Else
                T.Select(J, Word.Length)
                T.SelectionColor = color1
                I = J + 1
            End If
        End While
        T.Select(Pos, 0)
    End Sub

    Private Sub rtfDataCapture_KeyDown_Number(sender As Object, e As KeyEventArgs)

        _InvalidCharacterEntered = False

        ' Determine whether the keystroke is a number from the top of the keyboard.
        If e.KeyCode < Keys.D0 OrElse e.KeyCode > Keys.D9 Then
            ' Determine whether the keystroke is a number from the keypad.
            If e.KeyCode < Keys.NumPad0 OrElse e.KeyCode > Keys.NumPad9 Then
                ' Determine whether the keystroke is a backspace.

                If e.KeyCode <> Keys.Back AndAlso e.KeyCode <> Keys.Delete Then
                    ' A non-numerical keystroke was pressed. 
                    ' Set the flag to true and evaluate in KeyPress event.
                    _InvalidCharacterEntered = True
                End If
            End If
        End If

        'If shift key was pressed, it's not a number.
        If Control.ModifierKeys = Keys.Shift OrElse Control.ModifierKeys = Keys.Alt OrElse Control.ModifierKeys = Keys.Control Then
            _InvalidCharacterEntered = True
        End If

    End Sub

    Private Sub rtfDataCapture_KeyDown_Amt(sender As Object, e As KeyEventArgs)

        _InvalidCharacterEntered = False

        ' Determine whether the keystroke is a number from the top of the keyboard.
        If e.KeyCode < Keys.D0 OrElse e.KeyCode > Keys.D9 Then
            ' Determine whether the keystroke is a number from the keypad.
            If e.KeyCode < Keys.NumPad0 OrElse e.KeyCode > Keys.NumPad9 Then
                ' Determine whether the keystroke is a backspace.

                If e.KeyCode <> Keys.Back AndAlso e.KeyCode <> Keys.Delete AndAlso e.KeyCode <> Keys.OemPeriod AndAlso e.KeyCode <> Keys.OemMinus Then
                    ' A non-numerical keystroke was pressed. 
                    ' Set the flag to true and evaluate in KeyPress event.
                    _InvalidCharacterEntered = True
                End If
            End If
        End If

        'If shift key was pressed, it's not a number.
        If Control.ModifierKeys = Keys.Shift OrElse Control.ModifierKeys = Keys.Alt OrElse Control.ModifierKeys = Keys.Control Then
            _InvalidCharacterEntered = True
        End If

    End Sub

    Private Sub rtfDataCapture_KeyDown_Date(sender As Object, e As KeyEventArgs)

        _InvalidCharacterEntered = False

        ' Determine whether the keystroke is a number from the top of the keyboard.
        If e.KeyCode < Keys.D0 OrElse e.KeyCode > Keys.D9 Then
            ' Determine whether the keystroke is a number from the keypad.
            If e.KeyCode < Keys.NumPad0 OrElse e.KeyCode > Keys.NumPad9 Then
                ' Determine whether the keystroke is a backspace.

                If e.KeyCode <> Keys.Back AndAlso e.KeyCode <> Keys.Delete AndAlso e.KeyCode <> Keys.Divide AndAlso e.KeyCode <> Keys.OemMinus AndAlso e.KeyCode <> Keys.Oem2 Then
                    ' A non-numerical keystroke was pressed. 
                    ' Set the flag to true and evaluate in KeyPress event.
                    _InvalidCharacterEntered = True
                End If
            End If
        End If

        'If shift key was pressed, it's not a number.
        If Control.ModifierKeys = Keys.Shift OrElse Control.ModifierKeys = Keys.Alt OrElse Control.ModifierKeys = Keys.Control OrElse e.KeyCode = Keys.OemBackslash OrElse e.KeyCode = Keys.Oem5 Then
            _InvalidCharacterEntered = True
        End If

    End Sub

    Private Sub rtfDataCapture_KeyPress(sender As Object, e As KeyPressEventArgs) Handles rtfDataCapture.KeyPress
        If _InvalidCharacterEntered = True Then
            ' Stop the character from being entered into the control since it is non-numerical.
            e.Handled = True
        End If
    End Sub
End Class