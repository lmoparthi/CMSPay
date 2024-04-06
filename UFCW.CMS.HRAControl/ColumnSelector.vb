Public Class ColumnSelector
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents ColList As System.Windows.Forms.CheckedListBox
    Friend WithEvents CloseForm As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.CloseForm = New System.Windows.Forms.Button()
        Me.ColList = New System.Windows.Forms.CheckedListBox()
        Me.SuspendLayout()
        '
        'CloseForm
        '
        Me.CloseForm.Anchor = (System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right)
        Me.CloseForm.Location = New System.Drawing.Point(104, 169)
        Me.CloseForm.Name = "CloseForm"
        Me.CloseForm.TabIndex = 0
        Me.CloseForm.Text = "&Close"
        '
        'ColList
        '
        Me.ColList.Anchor = (((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right)
        Me.ColList.Location = New System.Drawing.Point(8, 8)
        Me.ColList.Name = "ColList"
        Me.ColList.Size = New System.Drawing.Size(176, 154)
        Me.ColList.TabIndex = 1
        '
        'ColumnSelector
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(194, 194)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.ColList, Me.CloseForm})
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "ColumnSelector"
        Me.Text = "Output Columns"
        Me.ResumeLayout(False)

    End Sub

#End Region

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"
    Private _DGTS As DataGridTableStyle
    Private _ColMapDT As New DataTable("ColumnMap")
    Private _Loading As Boolean = True
    Private _Section As String
#Region "Properties"

    <System.ComponentModel.Browsable(True), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
        End Set
    End Property

#End Region
    'Form overrides dispose to clean up the component list.

    Private _Disposed As Boolean

    ' Protected implementation of Dispose pattern.
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If Not _Disposed Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If

                If _ColMapDT IsNot Nothing Then
                    _ColMapDT.Dispose()
                End If
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            ' TODO: set large fields to null.
            _Disposed = True
        End If

        ' Call the base class implementation.
        MyBase.Dispose(disposing)
    End Sub

    Sub New(ByVal dgts As DataGridTableStyle, ByVal path As String)
        Me.New()
        _DGTS = dgts
        _Section = path
    End Sub

    Private Sub SetSettings()
        Me.Top = If(CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)))
        Me.Height = CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = If(CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)))
        Me.Width = CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(_AppKey, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)
    End Sub

    Private Sub SaveSettings()
        Dim WindowState As FormWindowState = Me.WindowState
        SaveSetting(_AppKey, Me.Name & "\Settings", "WindowState", CInt(WindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = WindowState
    End Sub

    Private Sub ColumnSelector_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        _ColMapDT.Columns.Add("Mapping")
        _ColMapDT.Columns.Add("HeaderText")

        LoadColumns()

        SetSettings()

        _Loading = False
    End Sub

    Private Sub CloseForm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseForm.Click
        Me.Close()
    End Sub

    Private Sub ColumnSelector_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        SaveSettings()
    End Sub

    Private Sub LoadColumns()
        Dim Setting(,) As String = Nothing
        Dim ColStyle As DataGridColumnStyle
        Dim DR As DataRow
        Dim Cnt As Integer = 0

        Try

            ColList.DataSource = _ColMapDT
            ColList.DisplayMember = "HeaderText"

            For Each ColStyle In _DGTS.GridColumnStyles
                DR = _ColMapDT.NewRow
                DR("Mapping") = ColStyle.MappingName
                DR("HeaderText") = ColStyle.HeaderText
                _ColMapDT.Rows.Add(DR)
            Next

            Setting = GetAllSettings(_AppKey, _Section)
            If Setting Is Nothing Then
                For Cnt = 0 To _ColMapDT.Rows.Count - 1
                    SaveSetting(_APPKEY, _Section, "Col " & _ColMapDT.Rows(Cnt)("Mapping").ToString & " Export", "1")
                Next
            End If

            For Cnt = 0 To _ColMapDT.Rows.Count - 1
                ColList.SetItemChecked(Cnt, CBool(GetSetting(_AppKey, _Section, "Col " & _ColMapDT.Rows(Cnt)("Mapping").ToString & " Export", "1")))
            Next

        Catch ex As Exception
        End Try
    End Sub

    Private Sub ColList_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ColList.SelectedValueChanged
        If _Loading Then Exit Sub

        If ColList.GetItemChecked(ColList.SelectedIndex) Then
            SaveSetting(_APPKEY, _Section, "Col " & _ColMapDT.Rows(ColList.SelectedIndex)("Mapping").ToString & " Export", "1")
        Else
            SaveSetting(_APPKEY, _Section, "Col " & _ColMapDT.Rows(ColList.SelectedIndex)("Mapping").ToString & " Export", "0")
        End If
    End Sub

    Private Sub ColList_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles ColList.ItemCheck
        If _Loading Then Exit Sub

        If e.NewValue = CheckState.Checked Then
            SaveSetting(_AppKey, _Section, "Col " & _ColMapDT.Rows(ColList.SelectedIndex)("Mapping").ToString & " Export", "1")
        Else
            SaveSetting(_AppKey, _Section, "Col " & _ColMapDT.Rows(ColList.SelectedIndex)("Mapping").ToString & " Export", "0")
        End If
    End Sub
End Class