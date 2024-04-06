Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ComponentModel

Public Class AccumulatorSelector
    Inherits System.Windows.Forms.Form

    Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _AccumsDT As DataTable

    Private _APPKEY As String = "UFCW\Claims\"

#Region "Public Properties"

    <DefaultValue("UFCW\Claims\"), Browsable(True), System.ComponentModel.Description("Represents the application key used when accessing the registry.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = Value
        End Set
    End Property
#End Region

#Region " Windows Form Designer generated code "
    Public Sub New(ByVal AvailableAccums As DataTable)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _AccumsDT = AvailableAccums
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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents AccumulatorComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ValueTextBox As System.Windows.Forms.TextBox
    Friend WithEvents CloseButton As System.Windows.Forms.Button
    Friend WithEvents AddButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.AccumulatorComboBox = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ValueTextBox = New System.Windows.Forms.TextBox()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.AddButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(69, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Accumulator:"
        '
        'AccumulatorComboBox
        '
        Me.AccumulatorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.AccumulatorComboBox.Location = New System.Drawing.Point(80, 12)
        Me.AccumulatorComboBox.Name = "AccumulatorComboBox"
        Me.AccumulatorComboBox.Size = New System.Drawing.Size(100, 21)
        Me.AccumulatorComboBox.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(40, 44)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(36, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Apply:"
        '
        'ValueTextBox
        '
        Me.ValueTextBox.Location = New System.Drawing.Point(80, 40)
        Me.ValueTextBox.MaxLength = 20
        Me.ValueTextBox.Name = "ValueTextBox"
        Me.ValueTextBox.Size = New System.Drawing.Size(100, 20)
        Me.ValueTextBox.TabIndex = 3
        '
        'CloseButton
        '
        Me.CloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CloseButton.Location = New System.Drawing.Point(8, 66)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(75, 23)
        Me.CloseButton.TabIndex = 4
        Me.CloseButton.Text = "&Cancel"
        '
        'AddButton
        '
        Me.AddButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AddButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.AddButton.Enabled = False
        Me.AddButton.Location = New System.Drawing.Point(108, 66)
        Me.AddButton.Name = "AddButton"
        Me.AddButton.Size = New System.Drawing.Size(75, 23)
        Me.AddButton.TabIndex = 5
        Me.AddButton.Text = "&Add"
        '
        'AccumulatorSelector
        '
        Me.AcceptButton = Me.AddButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.CloseButton
        Me.ClientSize = New System.Drawing.Size(190, 92)
        Me.Controls.Add(Me.AddButton)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.ValueTextBox)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.AccumulatorComboBox)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AccumulatorSelector"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Add Accumulator:"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub AccumulatorSelector_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim DV As New DataView(_AccumsDT, "", "ACCUM_NAME", DataViewRowState.CurrentRows)

            For Cnt As Integer = 0 To DV.Count - 1
                AccumulatorComboBox.Items.Add(DV(Cnt)("ACCUM_NAME"))
            Next

            SetSettings()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub AccumulatorSelector_FormClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.FormClosing
        Try
            SaveSettings()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SetSettings()
        Me.Top = If(CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)))
        Me.Left = If(CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)))
        Me.WindowState = CType(GetSetting(_AppKey, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Saves the basic form settings.  Windowstate, height, width, top, and left.
    ' </summary>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	11/16/2005	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub SaveSettings()

        Dim TheWindowState As FormWindowState = Me.WindowState
        SaveSetting(_AppKey, Me.Name & "\Settings", "WindowState", CInt(TheWindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = TheWindowState

    End Sub

    Private Sub CloseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseButton.Click
        Try
            Me.DialogResult = DialogResult.Cancel
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub AddButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddButton.Click
        Try
            Me.DialogResult = DialogResult.OK
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub AccumulatorComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles AccumulatorComboBox.SelectedIndexChanged
        Try
            If AccumulatorComboBox.Text <> "" AndAlso ValueTextBox.Text.Trim <> "" AndAlso IsNumeric(ValueTextBox.Text) = True Then
                AddButton.Enabled = True
            Else
                AddButton.Enabled = False
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ValueTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ValueTextBox.TextChanged
        Dim TBox As TextBox
        Dim IntCnt As Integer
        Dim StrTmp As String

        Try

            TBox = CType(sender, TextBox)

            If IsNumeric(TBox.Text) = False AndAlso Len(TBox.Text) > 0 Then
                StrTmp = TBox.Text
                For IntCnt = 1 To Len(StrTmp)
                    If IsNumeric(Mid(StrTmp, IntCnt, 1)) = False AndAlso Len(StrTmp) > 0 _
                                                AndAlso Mid(StrTmp, IntCnt, 1) <> "." Then
                        StrTmp = Replace(StrTmp, Mid(StrTmp, IntCnt, 1), "")
                    End If
                Next
                TBox.Text = StrTmp
            End If

            If AccumulatorComboBox.Text <> "" AndAlso ValueTextBox.Text.Trim <> "" AndAlso IsNumeric(ValueTextBox.Text) Then
                AddButton.Enabled = True
            Else
                AddButton.Enabled = False
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
End Class