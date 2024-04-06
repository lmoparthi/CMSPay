Imports UFCWGeneral.Generals

Public Class ColumnSelector
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
    Friend WithEvents ColList As ExtendedCheckedListBox
    Friend WithEvents SaveForm As System.Windows.Forms.Button
    Friend WithEvents CancelForm As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.CancelForm = New System.Windows.Forms.Button
        Me.ColList = New UFCW.CMS.CustomerServiceControl.ExtendedCheckedListBox
        Me.SaveForm = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'CancelForm
        '
        Me.CancelForm.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CancelForm.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelForm.Location = New System.Drawing.Point(107, 169)
        Me.CancelForm.Name = "CancelForm"
        Me.CancelForm.Size = New System.Drawing.Size(75, 23)
        Me.CancelForm.TabIndex = 0
        Me.CancelForm.Text = "&Cancel"
        '
        'ColList
        '
        Me.ColList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ColList.AutoCheck = True
        Me.ColList.CheckMember = Nothing
        Me.ColList.CheckOnClick = True
        Me.ColList.Location = New System.Drawing.Point(3, 8)
        Me.ColList.Name = "ColList"
        Me.ColList.Size = New System.Drawing.Size(187, 154)
        Me.ColList.TabIndex = 1
        '
        'SaveForm
        '
        Me.SaveForm.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SaveForm.Location = New System.Drawing.Point(12, 169)
        Me.SaveForm.Name = "SaveForm"
        Me.SaveForm.Size = New System.Drawing.Size(75, 23)
        Me.SaveForm.TabIndex = 2
        Me.SaveForm.Text = "&Save"
        '
        'ColumnSelector
        '
        Me.AcceptButton = Me.SaveForm
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.CancelForm
        Me.ClientSize = New System.Drawing.Size(194, 194)
        Me.Controls.Add(Me.SaveForm)
        Me.Controls.Add(Me.ColList)
        Me.Controls.Add(Me.CancelForm)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Name = "ColumnSelector"
        Me.Text = "Output Columns"
        Me.ResumeLayout(False)

    End Sub

#End Region
    Private _APPKEY As String = "UFCW\Claims\"
    Private mStyle As DataGridTableStyle
    Private ColMap As New DataTable("ColumnMap")
    Private Loading As Boolean = True
    Private Section As String
#Region "Properties"

    <System.ComponentModel.Browsable(True), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")> _
          Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal Value As String)
            _APPKEY = Value
        End Set
    End Property

#End Region

    Sub New(ByVal GStyle As DataGridTableStyle, ByVal Path As String)
        Me.New()
        mStyle = GStyle
        Section = Path
    End Sub

    Private Sub SetSettings()

        Me.Top = IIf(CInt(GetSetting(AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)))
        Me.Height = CInt(GetSetting(AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = IIf(CInt(GetSetting(AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)))
        Me.Width = CInt(GetSetting(AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(AppKey, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)

    End Sub

    Private Sub SaveSettings()

        Dim lWindowState As FormWindowState = Me.WindowState
        SaveSetting(AppKey, Me.Name & "\Settings", "WindowState", CInt(lWindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = lWindowState

    End Sub

    Private Sub ColumnSelector_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ColMap.Columns.Add("Mapping")
        ColMap.Columns.Add("HeaderText")
        ColMap.Columns.Add("State")

        LoadColumns()

        SetSettings()

        Loading = False
    End Sub

    Private Sub CancelForm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelForm.Click
        Me.Close()
    End Sub

    Private Sub SaveForm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveForm.Click

        Dim myEnumerator As IEnumerator
        myEnumerator = ColList.CheckedItems.GetEnumerator
        DeleteSetting(AppKey, Section)

        While myEnumerator.MoveNext() <> False

            SaveSetting(AppKey, Section, "Col " & CType(myEnumerator.Current, DataRowView).Item(0) & " Export", 1)

        End While

        Me.Close()
    End Sub

    Private Sub ColumnSelector_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        SaveSettings()
    End Sub

    Private Sub LoadColumns()
        Dim Setting(,) As String = Nothing
        Try
            Dim ColStyle As DataGridColumnStyle
            Dim r As DataRow
            Dim cnt As Integer = 0

            ColList.DataSource = ColMap
            ColList.DisplayMember = "HeaderText"
            ColList.CheckMember = "State"

            ColList.SuspendLayout()

            For Each ColStyle In mStyle.GridColumnStyles
                r = ColMap.NewRow
                r("Mapping") = ColStyle.MappingName
                r("HeaderText") = ColStyle.HeaderText
                r("State") = GetSetting(AppKey, Section, "Col " & ColStyle.MappingName & " Export", 0)

                ColMap.Rows.Add(r)
            Next

            ColList.ResumeLayout()
            ColList.Refresh()

            ColList.checkmember = "State"


            'Setting = GetAllSettings(AppKey, Section)
            'If IsNothing(Setting) Then
            '    For cnt = 0 To ColMap.Rows.Count - 1
            '        SaveSetting(AppKey, Section, "Col " & ColMap.Rows(cnt)("Mapping") & " Export", 1)
            '    Next
            'End If


            'For cnt = 0 To ColMap.Rows.Count - 1
            '    ColList.SetItemChecked(cnt, GetSetting(AppKey, Section, "Col " & ColMap.Rows(cnt)("Mapping") & " Export", 1))
            'Next

        Catch ex As Exception
        End Try
    End Sub

    Private Sub ColList_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ColList.SelectedValueChanged
        If Loading = True Then Exit Sub

        If ColList.GetItemChecked(ColList.SelectedIndex) = True Then
            SaveSetting(AppKey, Section, "Col " & ColMap.Rows(ColList.SelectedIndex)("Mapping") & " Export", 1)
        Else
            SaveSetting(AppKey, Section, "Col " & ColMap.Rows(ColList.SelectedIndex)("Mapping") & " Export", 0)
        End If
    End Sub

    Private Sub ColList_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles ColList.ItemCheck
        If Loading = True Then Exit Sub

        If e.NewValue = CheckState.Checked Then
            SaveSetting(AppKey, Section, "Col " & ColMap.Rows(ColList.SelectedIndex)("Mapping") & " Export", 1)
        Else
            SaveSetting(AppKey, Section, "Col " & ColMap.Rows(ColList.SelectedIndex)("Mapping") & " Export", 0)
        End If
    End Sub
End Class
