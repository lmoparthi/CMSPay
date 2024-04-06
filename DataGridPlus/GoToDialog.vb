
Public Class GoToDialog
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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents LineNum As System.Windows.Forms.TextBox
    Friend WithEvents OK As System.Windows.Forms.Button
    Friend WithEvents Cancel As System.Windows.Forms.Button
    '<System.Diagnostics.DebuggerStepThrough()> 
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LineNum = New System.Windows.Forms.TextBox()
        Me.OK = New System.Windows.Forms.Button()
        Me.Cancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Line Number:"
        '
        'LineNum
        '
        Me.LineNum.Anchor = ((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right)
        Me.LineNum.Location = New System.Drawing.Point(80, 8)
        Me.LineNum.Name = "LineNum"
        Me.LineNum.Size = New System.Drawing.Size(92, 20)
        Me.LineNum.TabIndex = 1
        Me.LineNum.Text = ""
        '
        'OK
        '
        Me.OK.Anchor = (System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)
        Me.OK.Enabled = False
        Me.OK.Location = New System.Drawing.Point(8, 36)
        Me.OK.Name = "OK"
        Me.OK.TabIndex = 2
        Me.OK.Text = "&OK"
        '
        'Cancel
        '
        Me.Cancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)
        Me.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel.Location = New System.Drawing.Point(96, 36)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.TabIndex = 3
        Me.Cancel.Text = "&Cancel"
        '
        'GoToDialog
        '
        Me.AcceptButton = Me.OK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.Cancel
        Me.ClientSize = New System.Drawing.Size(178, 63)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.Cancel, Me.OK, Me.LineNum, Me.Label1})
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "GoToDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Go To Line:"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private _MyGrid As DataGridCustom

    Sub New(ByVal grid As DataGridCustom)
        Me.New()

        _MyGrid = Grid
    End Sub

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click

        Dim Tot As Integer
        Dim DGTS As DataGridTableStyle
        Dim GCS As DataGridColumnStyle
        Dim TextCol As DataGridHighlightTextBoxColumn
        Dim BoolCol As DataGridHighlightBoolColumn

        Try

            Tot = _MyGrid.GetGridRowCount

            If CInt(LineNum.Text) > Tot Then
                LineNum.Text = CStr(Tot)
            ElseIf CInt(LineNum.Text) < 1 Then
                LineNum.Text = CStr(1)
            End If

            _MyGrid.MoveGridToRow(CInt(LineNum.Text) - 1)

            DGTS = _MyGrid.GetCurrentTableStyle
            If DGTS Is Nothing Then Exit Sub

            For Each GCS In DGTS.GridColumnStyles
                If TypeOf GCS Is DataGridHighlightTextBoxColumn Then
                    TextCol = CType(GCS, DataGridHighlightTextBoxColumn)
                    _MyGrid.HighlightedRow = CInt(LineNum.Text) - 1
                    TextCol.HighlightRow = CInt(LineNum.Text) - 1
                    _MyGrid.Refresh()

                ElseIf TypeOf GCS Is DataGridHighlightBoolColumn Then
                    BoolCol = CType(GCS, DataGridHighlightBoolColumn)
                    _MyGrid.HighlightedRow = CInt(LineNum.Text) - 1
                    BoolCol.HighlightRow = CInt(LineNum.Text) - 1

                    _MyGrid.Refresh()
                End If
            Next

            _MyGrid.Select()

            _MyGrid.LastGoToLine = LineNum.Text

            Me.Close()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

    Private Sub LineNum_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LineNum.TextChanged
        Dim TBox As TextBox
        Dim IntCnt As Integer
        Dim StrTmp As String

        Try

            TBox = CType(sender, TextBox)

            If IsNumeric(TBox.Text) = False And Len(TBox.Text) > 0 Then
                StrTmp = TBox.Text
                For IntCnt = 1 To Len(StrTmp)
                    If IsNumeric(Mid(StrTmp, IntCnt, 1)) = False And Len(StrTmp) > 0 Then
                        StrTmp = Replace(StrTmp, Mid(StrTmp, IntCnt, 1), "")
                    End If
                Next
                TBox.Text = StrTmp
            End If

            If Len(LineNum.Text) > 0 Then
                OK.Enabled = True
            Else
                OK.Enabled = False
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

End Class

