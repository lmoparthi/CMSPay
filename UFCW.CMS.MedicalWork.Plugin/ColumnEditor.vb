Imports System.Configuration

Public Class ColumnEditor
    Inherits System.Windows.Forms.Form

    Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String
    Private _ColsDV As DataView

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal RegKey As String)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _APPKEY = RegKey
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
    Friend WithEvents ColumnsCheckedListBox As System.Windows.Forms.CheckedListBox
    Friend WithEvents SaveButton As System.Windows.Forms.Button
    Friend WithEvents EscButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.ColumnsCheckedListBox = New System.Windows.Forms.CheckedListBox()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.EscButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ColumnsCheckedListBox
        '
        Me.ColumnsCheckedListBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ColumnsCheckedListBox.CheckOnClick = True
        Me.ColumnsCheckedListBox.Location = New System.Drawing.Point(8, 8)
        Me.ColumnsCheckedListBox.Name = "ColumnsCheckedListBox"
        Me.ColumnsCheckedListBox.Size = New System.Drawing.Size(240, 334)
        Me.ColumnsCheckedListBox.TabIndex = 0
        '
        'SaveButton
        '
        Me.SaveButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SaveButton.Location = New System.Drawing.Point(176, 344)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(75, 23)
        Me.SaveButton.TabIndex = 1
        Me.SaveButton.Text = "&Save"
        '
        'EscButton
        '
        Me.EscButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.EscButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.EscButton.Location = New System.Drawing.Point(8, 344)
        Me.EscButton.Name = "EscButton"
        Me.EscButton.Size = New System.Drawing.Size(75, 23)
        Me.EscButton.TabIndex = 2
        Me.EscButton.Text = "&Cancel"
        '
        'ColumnEditor
        '
        Me.AcceptButton = Me.SaveButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.EscButton
        Me.ClientSize = New System.Drawing.Size(258, 368)
        Me.Controls.Add(Me.EscButton)
        Me.Controls.Add(Me.SaveButton)
        Me.Controls.Add(Me.ColumnsCheckedListBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "ColumnEditor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Visible Columns"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub EscButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EscButton.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Sub SaveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveButton.Click
        Try
            For Cnt As Integer = 0 To _ColsDV.Count - 1
                SaveSetting(_APPKEY, "MedicalWork\MEDDTL\ColumnSettings", "Col " & _ColsDV(Cnt)("Mapping").ToString & " Visible", ColumnsCheckedListBox.GetItemChecked(Cnt).ToString)
            Next

            Me.DialogResult = DialogResult.OK
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function GetColumns() As DataTable

        Dim DT As New DataTable("Columns")
        Dim SampleTable As IDictionary
        Dim DR As DataRow

        DT.Columns.Add("Position", System.Type.GetType("System.Int32"))
        DT.Columns.Add("Mapping", System.Type.GetType("System.String"))
        DT.Columns.Add("HeaderText", System.Type.GetType("System.String"))
        DT.Columns.Add("Format", System.Type.GetType("System.String"))
        DT.Columns.Add("NullText", System.Type.GetType("System.Object"))
        DT.Columns.Add("Visible", System.Type.GetType("System.Boolean"))
        DT.Columns.Add("Type", System.Type.GetType("System.String"))
        DT.Columns.Add("Method", System.Type.GetType("System.String"))
        'DT.Columns.Add("Alignment", System.Type.GetType("System.String"))

        For Cnt As Integer = 1 To CInt(CType(ConfigurationManager.GetSection("MedicalWorkColumns"), IDictionary)("ColumnCount"))
            sampleTable = CType(ConfigurationManager.GetSection("MedicalWorkColumns"), IDictionary)

            DR = DT.NewRow
            DR("Position") = Cnt - 1
            DR("Mapping") = sampleTable.Item("Col" & CStr(Cnt) & "Mapping")
            DR("HeaderText") = sampleTable.Item("Col" & CStr(Cnt) & "HeaderText")
            DR("Format") = sampleTable.Item("Col" & CStr(Cnt) & "Format")
            DR("NullText") = sampleTable.Item("Col" & CStr(Cnt) & "NullText")
            DR("Visible") = sampleTable.Item("Col" & CStr(Cnt) & "Visible")
            DR("Type") = sampleTable.Item("Col" & CStr(Cnt) & "Type")
            DR("Method") = sampleTable.Item("Col" & CStr(Cnt) & "Method")

            DT.Rows.Add(DR)
        Next

        Return DT
    End Function

    Private Function AdjustUserColumnPositioning(ByVal columnDT As DataTable) As DataView
        Dim Setting As String = ""
        Dim TmpTbl As New DataTable("COLSCopy")
        Dim DR As DataRow
        Dim DV As DataView

        Try

            TmpTbl.Columns.Add("Mapping")

            For Cnt As Integer = 0 To columnDT.Rows.Count - 1
                DR = TmpTbl.NewRow

                DR("Mapping") = columnDT.Rows(Cnt).Item("Mapping")

                TmpTbl.Rows.Add(DR)
            Next

            For Cnt As Integer = 0 To columnDT.Rows.Count - 1
                Setting = GetSetting(_AppKey, "MedicalWork\MEDDTL\ColumnSettings", "Col " & TmpTbl.Rows(Cnt)("Mapping").ToString & " Pos", "-2")
                If CDbl(Setting) <> -2 Then
                    DV = New DataView(columnDT, "Mapping = '" & TmpTbl.Rows(Cnt)("Mapping").ToString & "'", "Mapping", DataViewRowState.CurrentRows)

                    DV(0)("Position") = CInt(Setting)
                End If

                Setting = GetSetting(_AppKey, "MedicalWork\MEDDTL\ColumnSettings", "Col " & TmpTbl.Rows(Cnt)("Mapping").ToString & " Visible", "-2")
                If Setting <> "-2" Then
                    DV = New DataView(columnDT, "Mapping = '" & TmpTbl.Rows(Cnt)("Mapping").ToString & "'", "Mapping", DataViewRowState.CurrentRows)

                    DV(0)("Visible") = CBool(Setting)
                End If
            Next

            Dim Cols As New DataView(columnDT, "", "Position", DataViewRowState.CurrentRows)

            Return Cols
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub ColumnEditor_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim SysCols As DataTable = GetColumns()

        Try
            _ColsDV = AdjustUserColumnPositioning(Syscols)

            ColumnsCheckedListBox.DataSource = _ColsDV
            ColumnsCheckedListBox.DisplayMember = "HeaderText"
            'ColumnsCheckedListBox.ValueMember = "Visible"

            For cnt As Integer = 0 To _ColsDV.Count - 1
                ColumnsCheckedListBox.SetItemChecked(cnt, CBool(_ColsDV(cnt)("Visible")))
            Next

            'For cnt As Integer = 0 To cols.Count - 1
            '    colindx = ColumnsCheckedListBox.Items.Add(cols(cnt)("HeaderText"), CBool(cols(cnt)("Visible")))
            'Next

        Catch ex As Exception
            Throw
        End Try
    End Sub
End Class