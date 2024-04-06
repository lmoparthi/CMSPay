
Public Class ColumnSelectorDialog
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
    Friend WithEvents ColList As ExtendedCheckedListBox
    Friend WithEvents SaveForm As System.Windows.Forms.Button
    Friend WithEvents FlipState As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SelectAllToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ResetButton As Button
    Friend WithEvents CancelForm As System.Windows.Forms.Button
    '<System.Diagnostics.DebuggerStepThrough()> 
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.CancelForm = New System.Windows.Forms.Button()
        Me.SaveForm = New System.Windows.Forms.Button()
        Me.FlipState = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SelectAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ColList = New ExtendedCheckedListBox()
        Me.ResetButton = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'CancelForm
        '
        Me.CancelForm.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CancelForm.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelForm.Location = New System.Drawing.Point(218, 298)
        Me.CancelForm.Name = "CancelForm"
        Me.CancelForm.Size = New System.Drawing.Size(68, 23)
        Me.CancelForm.TabIndex = 0
        Me.CancelForm.Text = "&Cancel"
        '
        'SaveForm
        '
        Me.SaveForm.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SaveForm.Location = New System.Drawing.Point(147, 298)
        Me.SaveForm.Name = "SaveForm"
        Me.SaveForm.Size = New System.Drawing.Size(68, 23)
        Me.SaveForm.TabIndex = 2
        Me.SaveForm.Text = "&Save"
        '
        'FlipState
        '
        Me.FlipState.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FlipState.ContextMenuStrip = Me.ContextMenuStrip1
        Me.FlipState.Location = New System.Drawing.Point(5, 298)
        Me.FlipState.Name = "FlipState"
        Me.FlipState.Size = New System.Drawing.Size(68, 23)
        Me.FlipState.TabIndex = 3
        Me.FlipState.Text = "Select &All"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SelectAllToolStripMenuItem, Me.SaveToolStripMenuItem, Me.CToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(165, 70)
        '
        'SelectAllToolStripMenuItem
        '
        Me.SelectAllToolStripMenuItem.Name = "SelectAllToolStripMenuItem"
        Me.SelectAllToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.SelectAllToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.SelectAllToolStripMenuItem.Text = "Select &All"
        Me.SelectAllToolStripMenuItem.Visible = False
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.SaveToolStripMenuItem.Text = "&Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'CToolStripMenuItem
        '
        Me.CToolStripMenuItem.Name = "CToolStripMenuItem"
        Me.CToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.CToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.CToolStripMenuItem.Text = "&Cancel"
        Me.CToolStripMenuItem.Visible = False
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
        Me.ColList.Size = New System.Drawing.Size(292, 274)
        Me.ColList.TabIndex = 1
        '
        'ResetButton
        '
        Me.ResetButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ResetButton.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ResetButton.Location = New System.Drawing.Point(76, 298)
        Me.ResetButton.Name = "ResetButton"
        Me.ResetButton.Size = New System.Drawing.Size(68, 23)
        Me.ResetButton.TabIndex = 4
        Me.ResetButton.Text = "&Reset Grid"
        '
        'ColumnSelectorDialog
        '
        Me.AcceptButton = Me.SaveForm
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.CancelForm
        Me.ClientSize = New System.Drawing.Size(299, 330)
        Me.Controls.Add(Me.ResetButton)
        Me.Controls.Add(Me.ColList)
        Me.Controls.Add(Me.CancelForm)
        Me.Controls.Add(Me.SaveForm)
        Me.Controls.Add(Me.FlipState)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MinimumSize = New System.Drawing.Size(254, 0)
        Me.Name = "ColumnSelectorDialog"
        Me.Text = "Select Columns"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public Enum ExecutionMode
        Export = 0
        Customize = 1
    End Enum


    Private _APPKEY As String = "UFCW\Claims\"
    Private _ColumnsExcluded As Integer = 0
    Private _ColMap As New DataTable("ColumnMap")
    Private _Grid As DataGridCustom
    Private _Mode As Integer
    Private _SubKey As String = ""
    Private _DefaultConfiguration As Boolean = True

#Region "Properties"

    <System.ComponentModel.Browsable(True), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal Value As String)
            _APPKEY = Value
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.Description("Gets the # of Columns selected.")>
    Public ReadOnly Property ColumnsSelected() As Integer
        Get
            Return _ColumnsExcluded
        End Get
    End Property

#End Region

    Sub New(ByVal grid As DataGridCustom)
        Me.New()
        _Grid = grid
    End Sub

    Private _Disposed As Boolean = False

    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.
            If _ColMap IsNot Nothing Then _ColMap.Dispose()
            _ColMap = Nothing

            If Not (components Is Nothing) Then
                components.Dispose()
            End If

        End If

        ' Free any unmanaged objects here.
        '
        _Disposed = True

        ' Call base class implementation.
        MyBase.Dispose(disposing)
    End Sub

    Sub New(ByVal appKEY As String, ByVal grid As DataGridCustom, Optional ByVal mode As Integer = 0, Optional ByVal subKey As String = "")
        Me.New()
        _APPKEY = appKEY
        _Grid = grid
        _Mode = mode
        '_StyleName = StyleName
        _SubKey = subKey

    End Sub


    Private Sub SetSettings()

        Me.Top = CInt(If(CInt(GetSetting(AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString))))
        Me.Height = CInt(GetSetting(AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = CInt(If(CInt(GetSetting(AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString))))
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
        _ColMap.Columns.Add("Mapping")
        _ColMap.Columns.Add("HeaderText")
        _ColMap.Columns.Add("State")

        LoadColumns()

        SetSettings()

        If _DefaultConfiguration Then
            FlipState.Text = "UnSelect &All"
        End If

    End Sub

    Private Sub CancelForm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelForm.Click

        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel

        Me.Close()
    End Sub

    Private Sub SaveForm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveForm.Click

        Dim myEnumerator As IEnumerator
        myEnumerator = ColList.CheckedItems.GetEnumerator

        Try
            DeleteSetting(AppKey, _Grid.Name & "\" & _Grid.GetCurrentDataTable.TableName & _SubKey & If(_Mode = ExecutionMode.Export, "\Export", "\Customize") & "\ColumnSelector")
        Catch ex As Exception
        End Try

        While myEnumerator.MoveNext() <> False

            SaveSetting(AppKey, _Grid.Name & "\" & _Grid.GetCurrentDataTable.TableName & _SubKey & If(_Mode = ExecutionMode.Export, "\Export", "\Customize") & "\ColumnSelector", "Col " & CType(myEnumerator.Current, DataRowView).Item(0).ToString.Trim & If(_Mode = ExecutionMode.Export, " Export", " Customize"), CStr(1))

        End While

        _ColumnsExcluded = ColList.Items.Count - ColList.CheckedItems.Count

        Me.DialogResult = System.Windows.Forms.DialogResult.OK

        Me.Close()

    End Sub

    Private Sub ColumnSelector_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        SaveSettings()
    End Sub

    Private Sub LoadColumns()
        Dim ColStyle As DataGridColumnStyle
        Dim DR As DataRow
        Dim DT As DataTable

        Try

            _DefaultConfiguration = True

            ColList.DataSource = _ColMap
            ColList.DisplayMember = "HeaderText"
            ColList.CheckMember = "State"

            ColList.SuspendLayout()

            If _Grid.DataSource IsNot Nothing Then
                DT = _Grid.GetCurrentDataTable

                If DT IsNot Nothing Then

                    For Each ColStyle In _Grid.TableStyles(If(_Grid.DataSource IsNot Nothing AndAlso DT.TableName.Trim.Length > 0, DT.TableName, "Default")).GridColumnStyles
                        If ColStyle.HeaderText.Trim.Length > 0 AndAlso ColStyle.Width > 0 AndAlso DT.Columns.Contains(ColStyle.MappingName) Then
                            DR = _ColMap.NewRow
                            DR("Mapping") = ColStyle.MappingName
                            DR("HeaderText") = ColStyle.HeaderText
                            DR("State") = GetSetting(AppKey, _Grid.Name & "\" & DT.TableName & _SubKey & If(_Mode = ExecutionMode.Export, "\Export", "\Customize") & "\ColumnSelector", "Col " & ColStyle.MappingName & If(_Mode = ExecutionMode.Export, " Export", " Customize"), CStr(If(_Mode = ExecutionMode.Export, 0, 1)))

                            If CDbl(GetSetting(AppKey, _Grid.Name & "\" & DT.TableName & _SubKey & If(_Mode = ExecutionMode.Export, "\Export", "\Customize") & "\ColumnSelector", "Col " & ColStyle.MappingName & If(_Mode = ExecutionMode.Export, " Export", " Customize"), CStr(If(_Mode = ExecutionMode.Export, 0, 1)))) = 0 Then
                                _DefaultConfiguration = False
                            End If

                            _ColMap.Rows.Add(DR)
                        End If
                    Next

                    ColList.CheckMember = "State"
                End If
            End If

        Catch ex As Exception
            Throw
        Finally
            ColList.ResumeLayout()
            ColList.Refresh()

        End Try
    End Sub

    Private Sub FlipState_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FlipState.Click

        Dim CheckState As Boolean

        ColList.SuspendLayout()

        Select Case FlipState.Text
            Case "Select &All"
                CheckState = True
                FlipState.Text = "UnSelect &All"
            Case "UnSelect &All"
                CheckState = False
                FlipState.Text = "Select &All"
        End Select

        ColList.ResumeLayout()

        For i As Integer = 0 To ColList.Items.Count - 1
            ColList.SetItemChecked(i, CheckState)
        Next

        ColList.ResumeLayout()

    End Sub

    Private Sub SelectAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAllToolStripMenuItem.Click
        FlipState.PerformClick()
    End Sub

    Private Sub ResetButton_Click(sender As Object, e As EventArgs) Handles ResetButton.Click

        Try
            Try
                My.Computer.Registry.CurrentUser.DeleteSubKeyTree("Software\VB and VBA Program Settings\" & AppKey & _Grid.Name)
            Catch ex As Exception
            Finally
                My.Computer.Registry.CurrentUser.Flush()
            End Try

        Catch ex As Exception
        End Try

        Me.DialogResult = System.Windows.Forms.DialogResult.OK

        Me.Close()

    End Sub
End Class
