Option Strict On

Public Class FindForm
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
    Friend WithEvents FindStr As System.Windows.Forms.TextBox
    Friend WithEvents Find As System.Windows.Forms.Button
    Friend WithEvents SingleRow As System.Windows.Forms.CheckBox
    Friend WithEvents Cols As System.Windows.Forms.ComboBox
    Friend WithEvents Match As System.Windows.Forms.CheckBox
    Friend WithEvents CloseMe As System.Windows.Forms.Button
    Friend WithEvents CursorStart As System.Windows.Forms.CheckBox
    '<System.Diagnostics.DebuggerStepThrough()> 
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.FindStr = New System.Windows.Forms.TextBox()
        Me.Find = New System.Windows.Forms.Button()
        Me.SingleRow = New System.Windows.Forms.CheckBox()
        Me.Cols = New System.Windows.Forms.ComboBox()
        Me.Match = New System.Windows.Forms.CheckBox()
        Me.CloseMe = New System.Windows.Forms.Button()
        Me.CursorStart = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(59, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Find What:"
        '
        'FindStr
        '
        Me.FindStr.Location = New System.Drawing.Point(72, 8)
        Me.FindStr.Name = "FindStr"
        Me.FindStr.Size = New System.Drawing.Size(256, 20)
        Me.FindStr.TabIndex = 0
        '
        'Find
        '
        Me.Find.Location = New System.Drawing.Point(336, 5)
        Me.Find.Name = "Find"
        Me.Find.Size = New System.Drawing.Size(72, 23)
        Me.Find.TabIndex = 5
        Me.Find.Text = "&Find Next"
        '
        'SingleRow
        '
        Me.SingleRow.Location = New System.Drawing.Point(16, 40)
        Me.SingleRow.Name = "SingleRow"
        Me.SingleRow.Size = New System.Drawing.Size(136, 24)
        Me.SingleRow.TabIndex = 1
        Me.SingleRow.Text = "Search Only Column:"
        '
        'Cols
        '
        Me.Cols.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cols.Location = New System.Drawing.Point(152, 40)
        Me.Cols.Name = "Cols"
        Me.Cols.Size = New System.Drawing.Size(176, 21)
        Me.Cols.TabIndex = 2
        '
        'Match
        '
        Me.Match.Location = New System.Drawing.Point(16, 64)
        Me.Match.Name = "Match"
        Me.Match.Size = New System.Drawing.Size(104, 24)
        Me.Match.TabIndex = 3
        Me.Match.Text = "Match Case"
        '
        'CloseMe
        '
        Me.CloseMe.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CloseMe.Location = New System.Drawing.Point(336, 40)
        Me.CloseMe.Name = "CloseMe"
        Me.CloseMe.Size = New System.Drawing.Size(72, 23)
        Me.CloseMe.TabIndex = 6
        Me.CloseMe.Text = "&Close"
        '
        'CursorStart
        '
        Me.CursorStart.Location = New System.Drawing.Point(16, 88)
        Me.CursorStart.Name = "CursorStart"
        Me.CursorStart.Size = New System.Drawing.Size(120, 24)
        Me.CursorStart.TabIndex = 4
        Me.CursorStart.Text = "Start From Cursor"
        '
        'FindForm
        '
        Me.AcceptButton = Me.Find
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.CloseMe
        Me.ClientSize = New System.Drawing.Size(410, 119)
        Me.Controls.Add(Me.CursorStart)
        Me.Controls.Add(Me.CloseMe)
        Me.Controls.Add(Me.Match)
        Me.Controls.Add(Me.Cols)
        Me.Controls.Add(Me.SingleRow)
        Me.Controls.Add(Me.Find)
        Me.Controls.Add(Me.FindStr)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FindForm"
        Me.Text = "Find"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

End Class

Public Class FindDialog
    Inherits FindForm

    Const increOne As Integer = 1

    Private _MyGrid As DataGridCustom
    Private _MPos(2) As Long
    Private _TabHolder As TabControl
    Private _TabPage As TabPage
    Private _LastFoundString As String = ""

    Sub New(ByVal tableName As String, ByVal dg As DataGridCustom, Optional ByVal tabs As TabControl = Nothing)
        MyBase.New()

        _MPos(0) = 0 '-1
        _MPos(1) = 0 '-1
        _MPos(2) = 0 '-1

        _MyGrid = dg

        _TabHolder = tabs

        If tabs IsNot Nothing Then _TabPage = _TabHolder.SelectedTab
    End Sub

    Public Property FindPage() As TabPage
        Get
            Return _TabPage
        End Get
        Set(ByVal Value As TabPage)
            _TabPage = Value
        End Set
    End Property

    Public Property SearchText() As String
        Get
            Return FindStr.Text
        End Get
        Set(ByVal Value As String)
            FindStr.Text = Value
        End Set
    End Property

    Private Sub Cols_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cols.TextChanged
        SingleRow.Checked = True
    End Sub

    Private Sub CloseMe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseMe.Click
        Me.Hide()
    End Sub

    Private Sub Find_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Find.Click
        Try
            FindNext()
        Catch ex As Exception
            MessageBox.Show(ex.Message & ex.StackTrace, "Error During Find_Click", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub FindStr_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles FindStr.KeyUp
        Try
            If e.KeyCode = Keys.F3 Then
                FindNext()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & ex.StackTrace, "Error During Find_Click", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub FindNext()
        Try
            _MyGrid.Select()
            SearchNext()
            Me.Select()

        Catch ex As Exception
            MessageBox.Show(ex.Message & ex.StackTrace, "Error During FindNext", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub ResetLastFoundPosition()
        _MPos(0) = 0
        _MPos(1) = 0
        _MPos(2) = 0
    End Sub

    Public Sub SearchNext()

        Dim Found As Boolean = False
        Dim DGTS As DataGridTableStyle
        Dim BoolCol As DataGridHighlightBoolColumn
        Dim TextCol As DataGridHighlightTextBoxColumn

        If FindStr.Text = "" Then Exit Sub
        Try

            If _LastFoundString <> FindStr.Text Then
                _MPos(0) = 0
                _MPos(1) = 0
                _MPos(2) = 0
            End If
            _LastFoundString = FindStr.Text

            If _TabHolder IsNot Nothing Then
                If _TabHolder.SelectedTab IsNot _TabPage Then
                    _MPos(0) = 0
                    _MPos(1) = 0
                    _MPos(2) = 0

                    'Tab = TabHolder.SelectedTab
                End If
            End If

            If CursorStart.Checked Then
                _MPos(0) = _MyGrid.CurrentRowIndex
                _MPos(1) = _MyGrid.CurrentCell.ColumnNumber + 1
                _MPos(2) = 0
            End If

            If SingleRow.Checked Then
                Found = FindColItem(Match.Checked)
            Else
                Found = FindItem(Match.Checked)
            End If

            If Found Then
                DGTS = _MyGrid.GetCurrentTableStyle

                If DGTS Is Nothing Then Exit Sub

                If TypeOf DGTS.GridColumnStyles(CInt(_MPos(1))) Is DataGridHighlightTextBoxColumn Then
                    TextCol = CType(DGTS.GridColumnStyles(CInt(_MPos(1))), DataGridHighlightTextBoxColumn)
                    _MyGrid.HighlightedCell = New DataGridCell(CInt(_MPos(0)), CInt(_MPos(1)))
                    TextCol.HighlightCell = New DataGridCell(CInt(_MPos(0)), CInt(_MPos(1)))

                    _MyGrid.EndEdit(DGTS.GridColumnStyles(CInt(_MPos(1))), _MyGrid.CurrentCell.RowNumber, False)
                    _MyGrid.BeginEdit(DGTS.GridColumnStyles(CInt(_MPos(1))), _MyGrid.CurrentCell.RowNumber)

                    _MyGrid.Refresh()
                ElseIf TypeOf DGTS.GridColumnStyles(CInt(_MPos(1))) Is DataGridHighlightBoolColumn Then
                    BoolCol = CType(DGTS.GridColumnStyles(CInt(_MPos(1))), DataGridHighlightBoolColumn)
                    _MyGrid.HighlightedCell = New DataGridCell(CInt(_MPos(0)), CInt(_MPos(1)))
                    BoolCol.HighlightCell = New DataGridCell(CInt(_MPos(0)), CInt(_MPos(1)))

                    _MyGrid.EndEdit(DGTS.GridColumnStyles(CInt(_MPos(1))), _MyGrid.CurrentCell.RowNumber, False)
                    _MyGrid.BeginEdit(DGTS.GridColumnStyles(CInt(_MPos(1))), _MyGrid.CurrentCell.RowNumber)

                    _MyGrid.Refresh()
                End If
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function FindColItem(ByVal match As Boolean) As Boolean
        Dim DR As DataRow
        Dim Cnt As Long
        Dim TotItms As Integer
        Dim DGTS As DataGridTableStyle
        Dim SrchString As String = ""
        Dim GridCol As DataGridColumnStyle
        Dim TBoxCol As System.Windows.Forms.DataGridTextBoxColumn

        Try
            TotItms = _MyGrid.BindingContext(_MyGrid.DataSource, _MyGrid.DataMember).Count - 1
            DGTS = _MyGrid.GetCurrentTableStyle

            If DGTS Is Nothing Then Return False
Start:
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor

            For Cnt = _MPos(0) To TotItms
                'rCnt += increOne

                If match = True Then
                    If Not _MyGrid.Item(CInt(Cnt), Cols.SelectedIndex) Is DBNull.Value Then
                        GridCol = _MyGrid.GetCurrentTableStyle.GridColumnStyles(Cols.SelectedIndex)

                        If Not TypeOf GridCol Is DataGridBoolColumn AndAlso Not TypeOf GridCol Is DataGridBoolColumn _
                                        AndAlso Not TypeOf GridCol Is DataGridHighlightBoolColumn Then
                            TBoxCol = CType(GridCol, System.Windows.Forms.DataGridTextBoxColumn)

                            If TBoxCol.Format <> "" Then
                                SrchString = Format(_MyGrid.Item(CInt(Cnt), Cols.SelectedIndex), TBoxCol.Format)
                                If SrchString = TBoxCol.Format Then
                                    SrchString = CStr(_MyGrid.Item(CInt(Cnt), Cols.SelectedIndex))
                                End If
                            Else
                                SrchString = CStr(_MyGrid.Item(CInt(Cnt), Cols.SelectedIndex))
                            End If
                        Else
                            SrchString = CStr(_MyGrid.Item(CInt(Cnt), Cols.SelectedIndex))
                        End If

                        If InStr(SrchString, FindStr.Text) > 0 AndAlso Cnt > _MPos(0) Then
                            _MyGrid.CurrentCell = New DataGridCell(CInt(Cnt), Cols.SelectedIndex)
                            _MPos(0) = Cnt
                            _MPos(1) = Cols.SelectedIndex
                            _MPos(2) = InStr(CStr(DR.Item(Cols.SelectedIndex)), FindStr.Text)
                            Return True
                        ElseIf InStr(SrchString, FindStr.Text) > 0 AndAlso Cnt = _MPos(0) Then
                            If InStr(SrchString, FindStr.Text) > _MPos(2) Then
                                _MyGrid.CurrentCell = New DataGridCell(CInt(Cnt), Cols.SelectedIndex)
                                _MPos(0) = Cnt
                                _MPos(1) = Cols.SelectedIndex
                                _MPos(2) = InStr(CStr(SrchString), FindStr.Text)
                                Return True
                            End If
                        End If
                    End If
                Else
                    If _MyGrid.Item(CInt(Cnt), Cols.SelectedIndex) IsNot DBNull.Value Then
                        GridCol = _MyGrid.GetCurrentTableStyle.GridColumnStyles(Cols.SelectedIndex)

                        If Not TypeOf GridCol Is DataGridBoolColumn AndAlso Not TypeOf GridCol Is DataGridBoolColumn _
                                        AndAlso Not TypeOf GridCol Is DataGridHighlightBoolColumn Then
                            TBoxCol = CType(GridCol, System.Windows.Forms.DataGridTextBoxColumn)

                            If TBoxCol.Format <> "" Then
                                SrchString = Format(_MyGrid.Item(CInt(Cnt), Cols.SelectedIndex), TBoxCol.Format)
                                If SrchString = TBoxCol.Format Then
                                    SrchString = CStr(_MyGrid.Item(CInt(Cnt), Cols.SelectedIndex))
                                End If
                            Else
                                SrchString = CStr(_MyGrid.Item(CInt(Cnt), Cols.SelectedIndex))
                            End If
                        Else
                            SrchString = CStr(_MyGrid.Item(CInt(Cnt), Cols.SelectedIndex))
                        End If

                        If InStr(UCase(CStr(SrchString)), UCase(CStr(FindStr.Text))) > 0 AndAlso Cnt > _MPos(0) Then
                            _MyGrid.CurrentCell = New DataGridCell(CInt(Cnt), Cols.SelectedIndex)
                            _MPos(0) = Cnt
                            _MPos(1) = Cols.SelectedIndex
                            _MPos(2) = InStr(UCase(CStr(SrchString)), UCase(CStr(FindStr.Text)))
                            Return True
                        ElseIf InStr(CStr(UCase(SrchString)), UCase(CStr(FindStr.Text))) > 0 AndAlso Cnt = _MPos(0) Then
                            If InStr(UCase(CStr(SrchString)), UCase(CStr(FindStr.Text))) > _MPos(2) Then
                                _MyGrid.CurrentCell = New DataGridCell(CInt(Cnt), Cols.SelectedIndex)
                                _MPos(0) = Cnt
                                _MPos(1) = Cols.SelectedIndex
                                _MPos(2) = InStr(UCase(CStr(SrchString)), UCase(CStr(FindStr.Text)))
                                Return True
                            End If
                        End If
                    End If
                End If
            Next

            If MessageBox.Show("Item Was Not Found. Start Search From Start?", "Not Found", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                _MPos(0) = 0 '-1
                _MPos(1) = 0 '-1
                _MPos(2) = 0 '-1
                GoTo Start
            End If

            Return False

        Catch ex As Exception
            MessageBox.Show(ex.Message & ex.StackTrace, "Error During FindColItem", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '    'ErrorOut(Application.ExecutablePath, "FindColItem", ex.Message & ex.StackTrace, MsgBoxStyle.Critical, "Error")
            Return False
        Finally
            System.Windows.Forms.Cursor.Current = Cursors.Default
        End Try
    End Function

    Private Function FindItem(ByVal match As Boolean) As Boolean

        Dim RowCnt As Long
        Dim ColCnt As Long
        Dim DGCS As DataGridColumnStyle
        Dim TotItms As Integer
        Dim DGTS As DataGridTableStyle
        Dim SrchString As String = ""
        Dim GridCol As DataGridColumnStyle
        Dim TBoxCol As System.Windows.Forms.DataGridTextBoxColumn

        Try
            TotItms = _MyGrid.BindingContext(_MyGrid.DataSource, _MyGrid.DataMember).Count - 1
            DGTS = _MyGrid.GetCurrentTableStyle

            If DGTS Is Nothing Then Return False

Start:
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor

            For RowCnt = _MPos(0) To TotItms
                For Each DGCS In DGTS.GridColumnStyles
                    ColCnt = DGTS.GridColumnStyles.IndexOf(DGCS)

                    If match Then
                        If _MyGrid.Item(CInt(RowCnt), CInt(ColCnt)) IsNot DBNull.Value Then
                            GridCol = _MyGrid.GetCurrentTableStyle.GridColumnStyles(CInt(ColCnt))

                            If Not TypeOf GridCol Is DataGridBoolColumn AndAlso Not TypeOf GridCol Is DataGridBoolColumn _
                                            AndAlso Not TypeOf GridCol Is DataGridHighlightBoolColumn Then
                                TBoxCol = CType(GridCol, System.Windows.Forms.DataGridTextBoxColumn)

                                If TBoxCol.Format <> "" Then
                                    SrchString = Format(_MyGrid.Item(CInt(RowCnt), CInt(ColCnt)), TBoxCol.Format)
                                    If SrchString = TBoxCol.Format Then
                                        SrchString = CStr(_MyGrid.Item(CInt(RowCnt), CInt(ColCnt)))
                                    End If
                                Else
                                    SrchString = CStr(_MyGrid.Item(CInt(RowCnt), CInt(ColCnt)))
                                End If
                            Else
                                SrchString = CStr(_MyGrid.Item(CInt(RowCnt), CInt(ColCnt)))
                            End If

                            If InStr(SrchString, FindStr.Text) > 0 Then
                                If RowCnt > _MPos(0) Then
                                    _MyGrid.CurrentCell = New DataGridCell(CInt(RowCnt), CInt(ColCnt))
                                    _MPos(0) = RowCnt
                                    _MPos(1) = ColCnt
                                    _MPos(2) = InStr(SrchString, FindStr.Text)
                                    Return True
                                ElseIf RowCnt = _MPos(0) AndAlso ColCnt > _MPos(1) Then
                                    _MyGrid.CurrentCell = New DataGridCell(CInt(RowCnt), CInt(ColCnt))
                                    _MPos(0) = RowCnt
                                    _MPos(1) = ColCnt
                                    _MPos(2) = InStr(SrchString, FindStr.Text)
                                    Return True
                                ElseIf RowCnt = _MPos(0) AndAlso ColCnt = _MPos(1) AndAlso InStr(SrchString, FindStr.Text) > _MPos(2) Then
                                    _MyGrid.CurrentCell = New DataGridCell(CInt(RowCnt), CInt(ColCnt))
                                    _MPos(0) = RowCnt
                                    _MPos(1) = ColCnt
                                    _MPos(2) = InStr(SrchString, FindStr.Text)
                                    Return True
                                End If
                            End If
                        End If
                    Else
                        If _MyGrid.Item(CInt(RowCnt), CInt(ColCnt)) IsNot DBNull.Value Then
                            GridCol = _MyGrid.GetCurrentTableStyle.GridColumnStyles(CInt(ColCnt))

                            If Not TypeOf GridCol Is DataGridBoolColumn AndAlso Not TypeOf GridCol Is DataGridBoolColumn _
                                            AndAlso Not TypeOf GridCol Is DataGridHighlightBoolColumn Then
                                TBoxCol = CType(GridCol, System.Windows.Forms.DataGridTextBoxColumn)

                                If TBoxCol.Format <> "" Then
                                    SrchString = Format(_MyGrid.Item(CInt(RowCnt), CInt(ColCnt)), TBoxCol.Format)
                                    If SrchString = TBoxCol.Format Then
                                        SrchString = CStr(_MyGrid.Item(CInt(RowCnt), CInt(ColCnt)))
                                    End If
                                Else
                                    SrchString = CStr(_MyGrid.Item(CInt(RowCnt), CInt(ColCnt)))
                                End If
                            Else
                                SrchString = CStr(_MyGrid.Item(CInt(RowCnt), CInt(ColCnt)))
                            End If

                            If InStr(UCase(CStr(SrchString)), UCase(FindStr.Text)) > 0 Then
                                If RowCnt > _MPos(0) Then
                                    _MyGrid.CurrentCell = New DataGridCell(CInt(RowCnt), CInt(ColCnt))
                                    _MPos(0) = RowCnt
                                    _MPos(1) = ColCnt
                                    _MPos(2) = InStr(UCase(CStr(SrchString)), UCase(CStr(FindStr.Text)))
                                    Return True
                                ElseIf RowCnt = _MPos(0) AndAlso ColCnt > _MPos(1) Then
                                    _MyGrid.CurrentCell = New DataGridCell(CInt(RowCnt), CInt(ColCnt))
                                    _MPos(0) = RowCnt
                                    _MPos(1) = ColCnt
                                    _MPos(2) = InStr(UCase(CStr(SrchString)), UCase(CStr(FindStr.Text)))
                                    Return True
                                ElseIf RowCnt = _MPos(0) AndAlso ColCnt = _MPos(1) AndAlso InStr(UCase(CStr(SrchString)), UCase(CStr(FindStr.Text))) > _MPos(2) Then
                                    _MyGrid.CurrentCell = New DataGridCell(CInt(RowCnt), CInt(ColCnt))
                                    _MPos(0) = RowCnt
                                    _MPos(1) = ColCnt
                                    _MPos(2) = InStr(UCase(CStr(SrchString)), UCase(CStr(FindStr.Text)))
                                    Return True
                                End If
                            End If
                        End If
                    End If
                Next
            Next

            If MessageBox.Show("Item Was Not Found. Start Search From Start?", "Not Found", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                _MPos(0) = 0 '-1
                _MPos(1) = 0 '-1
                _MPos(2) = 0 '-1
                GoTo Start
            End If

            Return False

        Catch ex As Exception
            MessageBox.Show(ex.Message & ex.StackTrace, "Error During FindItem", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '    'ErrorOut(Application.ExecutablePath, "FindItem", ex.Message & ex.StackTrace, MsgBoxStyle.Critical, "Error")
            Return False
        Finally
            System.Windows.Forms.Cursor.Current = Cursors.Default
        End Try
    End Function

    Private Sub SingleRow_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SingleRow.CheckedChanged
        If SingleRow.Checked = True AndAlso Cols.SelectedIndex < 0 AndAlso Cols.Items.Count > 0 Then Cols.SelectedIndex = 0
    End Sub

    Private Sub Cols_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cols.SelectedIndexChanged
        SingleRow.Checked = True
    End Sub

    Private Sub FindDialog_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        Dim F As FindDialog
        Dim Tmp As String = ""
        Dim Tmpchk As Boolean = False
        Dim DGCS As DataGridColumnStyle
        Dim DGTS As DataGridTableStyle

        Try
            F = CType(sender, FindDialog)
            DGTS = _MyGrid.GetCurrentTableStyle

            If DGTS Is Nothing Then Exit Sub

            If F.Visible Then
                Tmp = F.Cols.Text
                Tmpchk = F.SingleRow.Checked

                Cols.Items.Clear()
                For Each DGCS In DGTS.GridColumnStyles
                    Cols.Items.Add(DGCS.HeaderText)
                Next

                If Cols.Items.IndexOf(Tmp) <> -1 Then
                    Cols.SelectedIndex = Cols.Items.IndexOf(Tmp)
                End If
                F.SingleRow.Checked = Tmpchk
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & ex.StackTrace, "Error During FindDialog_Activated", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '    'ErrorOut(Application.ExecutablePath, "FindItem", ex.Message & ex.StackTrace, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Private Sub FindDialog_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        Dim DGTS As DataGridTableStyle
        Dim TextCol As DataGridHighlightTextBoxColumn

        Try
            DGTS = _MyGrid.GetCurrentTableStyle

            If DGTS Is Nothing Then Return

            _MyGrid.HighlightedCell = New DataGridCell(-1, -1)

            If Me.ParentForm IsNot Nothing Then Me.ParentForm.Refresh()
            If Me.Owner IsNot Nothing Then Me.Owner.Refresh()

            If TypeOf DGTS.GridColumnStyles(CInt(_MPos(1))) Is DataGridHighlightTextBoxColumn Then
                TextCol = CType(DGTS.GridColumnStyles(CInt(_MPos(1))), DataGridHighlightTextBoxColumn)
                TextCol.HighlightCell = Nothing
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message & ex.StackTrace, "Error During FindDialog_Closing", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '    'ErrorOut(Application.ExecutablePath, "FindItem", ex.Message & ex.StackTrace, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub
End Class

