Imports System.Collections.Generic

Imports UFCW.WCF


Public Class RecentDocsViewer
    Inherits System.Windows.Forms.Form

    Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _FNDisplay As Display
    Friend WithEvents ExitButton As System.Windows.Forms.Button
    Private _APPKEY As String = "UFCW\Claims\"

#Region "Public Properties"
    <System.ComponentModel.Description("Represents the application key used when accessing the registry.")> _
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
    Friend WithEvents FirstButton As System.Windows.Forms.Button
    Friend WithEvents PreviousButton As System.Windows.Forms.Button
    Friend WithEvents NextButton As System.Windows.Forms.Button
    Friend WithEvents LastButton As System.Windows.Forms.Button
    Friend WithEvents DisplayButton As System.Windows.Forms.Button
    Friend WithEvents RecentDocsDataGrid As DataGridCustom
    Friend WithEvents RecentDocsDataGridCustomContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents DisplayMenuItem As System.Windows.Forms.ToolStripMenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.RecentDocsDataGrid = New DataGridCustom()
        Me.RecentDocsDataGridCustomContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.DisplayMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FirstButton = New System.Windows.Forms.Button()
        Me.PreviousButton = New System.Windows.Forms.Button()
        Me.NextButton = New System.Windows.Forms.Button()
        Me.LastButton = New System.Windows.Forms.Button()
        Me.DisplayButton = New System.Windows.Forms.Button()
        Me.ExitButton = New System.Windows.Forms.Button()
        CType(Me.RecentDocsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RecentDocsDataGridCustomContextMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'RecentDocsDataGrid
        '
        Me.RecentDocsDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.RecentDocsDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.RecentDocsDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.RecentDocsDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.RecentDocsDataGrid.ADGroupsThatCanFind = ""
        Me.RecentDocsDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.RecentDocsDataGrid.ADGroupsThatCanMultiSort = ""
        Me.RecentDocsDataGrid.AllowAutoSize = True
        Me.RecentDocsDataGrid.AllowColumnReorder = False
        Me.RecentDocsDataGrid.AllowCopy = False
        Me.RecentDocsDataGrid.AllowCustomize = False
        Me.RecentDocsDataGrid.AllowDelete = False
        Me.RecentDocsDataGrid.AllowDragDrop = False
        Me.RecentDocsDataGrid.AllowEdit = False
        Me.RecentDocsDataGrid.AllowExport = False
        Me.RecentDocsDataGrid.AllowFilter = False
        Me.RecentDocsDataGrid.AllowFind = False
        Me.RecentDocsDataGrid.AllowGoTo = False
        Me.RecentDocsDataGrid.AllowMultiSelect = False
        Me.RecentDocsDataGrid.AllowMultiSort = False
        Me.RecentDocsDataGrid.AllowNew = False
        Me.RecentDocsDataGrid.AllowPrint = False
        Me.RecentDocsDataGrid.AllowRefresh = False
        Me.RecentDocsDataGrid.AllowSorting = False
        Me.RecentDocsDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RecentDocsDataGrid.AppKey = "UFCW\Claims\"
        Me.RecentDocsDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.RecentDocsDataGrid.CaptionVisible = False
        Me.RecentDocsDataGrid.ColumnHeaderLabel = Nothing
        Me.RecentDocsDataGrid.ColumnRePositioning = False
        Me.RecentDocsDataGrid.ColumnResizing = False
        Me.RecentDocsDataGrid.ConfirmDelete = True
        Me.RecentDocsDataGrid.ContextMenuStrip = Me.RecentDocsDataGridCustomContextMenu
        Me.RecentDocsDataGrid.CopySelectedOnly = True
        Me.RecentDocsDataGrid.DataMember = ""
        Me.RecentDocsDataGrid.DragColumn = 0
        Me.RecentDocsDataGrid.ExportSelectedOnly = True
        Me.RecentDocsDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.RecentDocsDataGrid.HighlightedRow = -1
        Me.RecentDocsDataGrid.IsMouseDown = False
        Me.RecentDocsDataGrid.LastGoToLine = ""
        Me.RecentDocsDataGrid.Location = New System.Drawing.Point(8, 4)
        Me.RecentDocsDataGrid.MultiSort = False
        Me.RecentDocsDataGrid.Name = "RecentDocsDataGrid"
        Me.RecentDocsDataGrid.OldSelectedRow = Nothing
        Me.RecentDocsDataGrid.ReadOnly = True
        Me.RecentDocsDataGrid.SetRowOnRightClick = True
        Me.RecentDocsDataGrid.ShiftPressed = False
        Me.RecentDocsDataGrid.SingleClickBooleanColumns = True
        Me.RecentDocsDataGrid.Size = New System.Drawing.Size(220, 272)
        Me.RecentDocsDataGrid.StyleName = ""
        Me.RecentDocsDataGrid.SubKey = ""
        Me.RecentDocsDataGrid.SuppressTriangle = False
        Me.RecentDocsDataGrid.TabIndex = 0
        '
        'RecentDocsDataGridCustomContextMenu
        '
        Me.RecentDocsDataGridCustomContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DisplayMenuItem})
        Me.RecentDocsDataGridCustomContextMenu.Name = "RecentDocsDataGridContextMenu"
        Me.RecentDocsDataGridCustomContextMenu.Size = New System.Drawing.Size(155, 26)
        '
        'DisplayMenuItem
        '
        Me.DisplayMenuItem.Name = "DisplayMenuItem"
        Me.DisplayMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
        Me.DisplayMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.DisplayMenuItem.Text = "&Display"
        '
        'FirstButton
        '
        Me.FirstButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FirstButton.Location = New System.Drawing.Point(236, 12)
        Me.FirstButton.Name = "FirstButton"
        Me.FirstButton.Size = New System.Drawing.Size(64, 23)
        Me.FirstButton.TabIndex = 1
        Me.FirstButton.Text = "&First"
        Me.FirstButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PreviousButton
        '
        Me.PreviousButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PreviousButton.Location = New System.Drawing.Point(236, 40)
        Me.PreviousButton.Name = "PreviousButton"
        Me.PreviousButton.Size = New System.Drawing.Size(64, 23)
        Me.PreviousButton.TabIndex = 2
        Me.PreviousButton.Text = "&Previous"
        Me.PreviousButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'NextButton
        '
        Me.NextButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NextButton.Location = New System.Drawing.Point(236, 68)
        Me.NextButton.Name = "NextButton"
        Me.NextButton.Size = New System.Drawing.Size(64, 23)
        Me.NextButton.TabIndex = 3
        Me.NextButton.Text = "&Next"
        Me.NextButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LastButton
        '
        Me.LastButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LastButton.Location = New System.Drawing.Point(236, 96)
        Me.LastButton.Name = "LastButton"
        Me.LastButton.Size = New System.Drawing.Size(64, 23)
        Me.LastButton.TabIndex = 4
        Me.LastButton.Text = "&Last"
        Me.LastButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'DisplayButton
        '
        Me.DisplayButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DisplayButton.Location = New System.Drawing.Point(236, 150)
        Me.DisplayButton.Name = "DisplayButton"
        Me.DisplayButton.Size = New System.Drawing.Size(64, 23)
        Me.DisplayButton.TabIndex = 5
        Me.DisplayButton.Text = "&Display"
        Me.DisplayButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ExitButton
        '
        Me.ExitButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ExitButton.Location = New System.Drawing.Point(236, 253)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(64, 23)
        Me.ExitButton.TabIndex = 6
        Me.ExitButton.Text = "E&xit"
        Me.ExitButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'RecentDocsViewer
        '
        Me.AcceptButton = Me.DisplayButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.ExitButton
        Me.ClientSize = New System.Drawing.Size(308, 282)
        Me.Controls.Add(Me.ExitButton)
        Me.Controls.Add(Me.DisplayButton)
        Me.Controls.Add(Me.LastButton)
        Me.Controls.Add(Me.NextButton)
        Me.Controls.Add(Me.PreviousButton)
        Me.Controls.Add(Me.FirstButton)
        Me.Controls.Add(Me.RecentDocsDataGrid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(250, 188)
        Me.Name = "RecentDocsViewer"
        Me.Text = "IDM Document(s) Navigator"
        CType(Me.RecentDocsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RecentDocsDataGridCustomContextMenu.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Form Events"
    Private Sub RecentDocsViewer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' sets up the contextmenu and calls to load the grid
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/13/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        SetSettings()

        LoadRecentDocs()

    End Sub

    Private Sub RecentDocsViewer_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        SaveSettings()
    End Sub

    Private Sub RecentDocsViewer_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub SetSettings()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Sets the basic form settings.  Windowstate, height, width, top, and left.
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	11/16/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Me.Top = If(CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)))
        Me.Height = CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = If(CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)))
        Me.Width = CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(_AppKey, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)
    End Sub

    Private Sub SaveSettings()
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
        Dim WindowState As FormWindowState = Me.WindowState
        SaveSetting(_AppKey, Me.Name & "\Settings", "WindowState", CInt(WindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = WindowState

        ' SaveColSettings()
    End Sub

    'Private Sub SaveColSettings()
    '    ' -----------------------------------------------------------------------------
    '    ' <summary>
    '    ' Saves settings for the grid columns
    '    ' </summary>
    '    ' <remarks>
    '    ' </remarks>
    '    ' <history>
    '    ' 	[Nick Snyder]	2/15/2006	Created
    '    ' </history>
    '    ' -----------------------------------------------------------------------------

    '    If RecentDocsDataGrid IsNot Nothing Then
    '        If RecentDocsDataGrid.DataSource IsNot Nothing Then
    '            RecentDocsDataGrid.SaveColumnsSizeAndPosition(_AppKey, RecentDocsDataGrid.Name & "\" & RecentDocsDataGrid.GetCurrentDataTable.TableName & "\ColumnSettings")
    '            RecentDocsDataGrid.SaveSortByColumnName(_AppKey, RecentDocsDataGrid.Name & "\" & RecentDocsDataGrid.GetCurrentDataTable.TableName & "\Sort", RecentDocsDataGrid.GetGridSortColumn)
    '        End If
    '    End If

    'End Sub
#End Region

#Region "Menu\Button Events"
    Private Sub LastButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LastButton.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' displays the oldest Document viewed in the IDM Viewer
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/13/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim Max As Integer

        Try

            Max = RecentDocsDataGrid.GetGridRowCount

            If Max > 0 Then
                RecentDocsDataGrid.CurrentRowIndex = Max - 1

                Using FNDisplay As New Display
                    FNDisplay.Display(New List(Of Long?) From {CLng(RecentDocsDataGrid.GetCurrentDataView(RecentDocsDataGrid.CurrentRowIndex)("DOCID"))})
                End Using

            End If

        Catch ex As ApplicationException

            MessageBox.Show(ex.Message, "FileNet unavailable, Restarting application may resolve connectivity issues.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        Catch ex As Exception
            Throw
        Finally
            SetNavigationButtons()
        End Try
    End Sub

    Private Sub NextButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NextButton.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' displays the previous Document viewed in the IDM Viewer
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/13/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim Max As Integer

        Try

            Max = RecentDocsDataGrid.GetGridRowCount

            If Max > 0 AndAlso RecentDocsDataGrid.CurrentRowIndex < Max - 1 Then
                RecentDocsDataGrid.CurrentRowIndex += 1

                Using FNDisplay As New Display
                    FNDisplay.Display(New List(Of Long?) From {CLng(RecentDocsDataGrid.GetCurrentDataView(RecentDocsDataGrid.CurrentRowIndex)("DOCID"))})
                End Using

            End If
        Catch ex As ApplicationException

            MessageBox.Show(ex.Message, "FileNet unavailable, Restarting application may resolve connectivity issues.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        Catch ex As Exception
            Throw
        Finally
            SetNavigationButtons()
        End Try
    End Sub

    Private Sub PreviousButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PreviousButton.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' displays the next Document viewed in the IDM Viewer
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/13/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            If RecentDocsDataGrid.GetGridRowCount > 0 AndAlso RecentDocsDataGrid.CurrentRowIndex > 0 Then
                RecentDocsDataGrid.CurrentRowIndex -= 1

                Using FNDisplay As New Display
                    FNDisplay.Display(New List(Of Long?) From {CLng(RecentDocsDataGrid.GetCurrentDataView(RecentDocsDataGrid.CurrentRowIndex)("DOCID"))})
                End Using

            End If

        Catch ex As ApplicationException

            MessageBox.Show(ex.Message, "FileNet unavailable, Restarting application may resolve connectivity issues.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        Catch ex As Exception
            Throw
        Finally
            SetNavigationButtons()
        End Try
    End Sub

    Private Sub FirstButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FirstButton.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' displays the most recent Document viewed in the IDM Viewer
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/13/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            If RecentDocsDataGrid.GetGridRowCount > 0 Then
                RecentDocsDataGrid.CurrentRowIndex = 0

                Using FNDisplay As New Display
                    FNDisplay.Display(New List(Of Long?) From {CLng(RecentDocsDataGrid.GetCurrentDataView(RecentDocsDataGrid.CurrentRowIndex)("DOCID"))})
                End Using
            End If

        Catch ex As ApplicationException

            MessageBox.Show(ex.Message, "FileNet unavailable, Restarting application may resolve connectivity issues.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        Catch ex As Exception
            Throw
        Finally
            SetNavigationButtons()
        End Try
    End Sub

    Private Sub SetNavigationButtons()

        Dim Max As Integer = Math.Max(RecentDocsDataGrid.GetGridRowCount - 1, 0)

        FirstButton.Enabled = False
        PreviousButton.Enabled = False
        NextButton.Enabled = False
        LastButton.Enabled = False

        If RecentDocsDataGrid.CurrentRowIndex = Max Then
            FirstButton.Enabled = True
            If RecentDocsDataGrid.CurrentRowIndex > 0 Then PreviousButton.Enabled = True
        End If

        If RecentDocsDataGrid.CurrentRowIndex = 0 Then
            If Max > 0 Then
                NextButton.Enabled = True
                LastButton.Enabled = True
            End If
        End If

        If RecentDocsDataGrid.CurrentRowIndex > 0 AndAlso RecentDocsDataGrid.CurrentRowIndex < Max Then
            FirstButton.Enabled = True
            PreviousButton.Enabled = True
            NextButton.Enabled = True
            LastButton.Enabled = True
        End If



    End Sub

    Private Sub DisplayButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DisplayButton.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' calls to display selected docs
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/13/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        DisplayDocs()
    End Sub

    Private Sub DisplayMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DisplayMenuItem.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' calls to display selected docs
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/13/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        DisplayDocs()
    End Sub
#End Region

#Region "Custom Subs\Functions"
    Private Sub LoadRecentDocs()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' loads the grid with the most recent information
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/13/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim LastAccessed As DateTime = UFCWGeneral.NowDate
        Dim DS As DataSet
        Dim DV As DataView
        Dim Cnt As Integer = 0

        Try

            DS = Display.GetRecentDocs

            If DS IsNot Nothing Then

                DV = New DataView(DS.Tables("RecentDocs"), "", "LastAccessed DESC", DataViewRowState.CurrentRows)

                RecentDocsDataGrid.DataSource = DV

            End If

            RecentDocsDataGrid.SetTableStyle()

            If DS IsNot Nothing AndAlso DV.Count > 0 Then
                RecentDocsDataGrid.Focus()
                RecentDocsDataGrid.CurrentRowIndex = 0

                PreviousButton.Enabled = False
                FirstButton.Enabled = False
            Else
                FirstButton.Enabled = False
                PreviousButton.Enabled = False
                NextButton.Enabled = False
                LastButton.Enabled = False

                DisplayButton.Enabled = False
            End If

            If RecentDocsDataGrid.DefaultContextMenu IsNot Nothing Then
                ToolStripManager.RevertMerge(RecentDocsDataGrid.DefaultContextMenu)
                RecentDocsDataGrid.DefaultContextMenuPrepare() 'need to modify default menu to exclude items not permitted for use

                If RecentDocsDataGridCustomContextMenu IsNot Nothing Then
                    ToolStripManager.RevertMerge(RecentDocsDataGridCustomContextMenu)
                    If RecentDocsDataGridCustomContextMenu.Items.Count > 0 Then
                        If RecentDocsDataGrid.DefaultContextMenu.Items.Count > 0 Then RecentDocsDataGridCustomContextMenu.Items.Add(New ToolStripSeparator())
                        ToolStripManager.Merge(RecentDocsDataGrid.DefaultContextMenu, RecentDocsDataGridCustomContextMenu)
                    End If
                End If

            End If

            If RecentDocsDataGridCustomContextMenu IsNot Nothing AndAlso RecentDocsDataGridCustomContextMenu.Items.Count > 0 Then
                RecentDocsDataGrid.ContextMenuStrip = RecentDocsDataGridCustomContextMenu
            End If

            AddHandler RecentDocsSerializer.RecentDocsRefreshAvailable, Sub(arg As RecentDocsEvent)
                                                                            If RecentDocsDataGrid.InvokeRequired Then
                                                                                RecentDocsDataGrid.Invoke(Sub()
                                                                                                              RecentDocsDataGrid.DataSource = New DataView(arg.RecentDocsDS.Tables("RecentDocs"), "", "LastAccessed DESC", DataViewRowState.CurrentRows)
                                                                                                          End Sub)

                                                                            Else
                                                                                RecentDocsDataGrid.DataSource = New DataView(arg.RecentDocsDS.Tables("RecentDocs"), "", "LastAccessed DESC", DataViewRowState.CurrentRows)
                                                                            End If
                                                                        End Sub

        Catch ex As Exception
            Throw
        Finally
            DV = Nothing

        End Try

    End Sub

    Private Sub DisplayDocs()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' displays all selected docs or the current row in the IDM Viewer
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/13/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim Docs As New List(Of Long?)
        Dim Doclist As ArrayList
        Try

            Doclist = RecentDocsDataGrid.GetSelectedDataRows()
            For Each DR As DataRow In Doclist
                If IsDBNull(DR("DOCID")) = False Then
                    Docs.Add(CLng(DR("DOCID")))
                End If
            Next

            If Docs.Count > 0 Then
                Using FNDisplay As New Display
                    FNDisplay.Display(Docs)
                End Using
            End If

        Catch ex As ApplicationException
            MessageBox.Show(ex.Message, "FileNet unavailable, Restarting application may resolve connectivity issues.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

#End Region

#Region "Other Control Events"
    Private Sub RecentDocsDataGrid_CurrentRowChanged(ByVal CurrentRowIndex As Integer?, ByVal LastRowIndex As Integer?) Handles RecentDocsDataGrid.CurrentRowChanged

        FirstButton.Enabled = True
        PreviousButton.Enabled = True
        NextButton.Enabled = True
        LastButton.Enabled = True

        If CurrentRowIndex = 0 Then
            NextButton.Enabled = False
            LastButton.Enabled = False
        ElseIf CurrentRowIndex = RecentDocsDataGrid.GetGridRowCount - 1 Then
            FirstButton.Enabled = False
            PreviousButton.Enabled = False
        End If
    End Sub
#End Region

    Private Sub ExitButton_Click(sender As Object, e As EventArgs) Handles ExitButton.Click
        Me.Close()
    End Sub
End Class