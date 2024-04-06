Option Infer On

Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class DetailLineReasons
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _ClaimID As Integer
    Private _LineNumber As Short
    Private _ReadOnly As Boolean
    Private _GridLines As Integer
    Private _APPKEY As String = "UFCW\Claims\"
    Private _Status As String = ""
    Private _ClaimDSCopy As New ClaimDataset
    Private _DateOfService As Date?
    Private _ReasonDT As DataTable
    Private _ClaimDS As New ClaimDataset
    Private WithEvents _ReasonsBS As BindingSource

    Const MAXREASONS As Integer = 5

#Region "Constructors"
    Public Sub New(ByVal claimID As Integer, ByVal lineNumber As Short, ByVal claimDS As ClaimDataset, ByVal [ReadOnly] As Boolean)

        Me.New(claimID, lineNumber, claimDS)

        Me.ReadOnly = [ReadOnly]
    End Sub
    Public Sub New(ByVal claimID As Integer, ByRef medDtlDR As DataRow, ByRef reasonsDT As DataTable)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _ClaimID = claimID
        _LineNumber = CShort(medDtlDR("LINE_NBR"))
        _DateOfService = CDate(If(IsDBNull(medDtlDR("OCC_FROM_DATE")), UFCWGeneral.NowDate, medDtlDR("OCC_FROM_DATE")))
        _ReasonDT = CloneHelper.DeepCopy(reasonsDT)
    End Sub
    Public Sub New(ByVal claimID As Integer, ByVal lineNumber As Short, ByVal claimDS As ClaimDataset)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _ClaimID = claimID
        _LineNumber = lineNumber

        '_ReasonDT = New DataTable("Reason")
        '_ReasonDT.Columns.Add("Name")
        '_ReasonDT.Columns.Add("Desc")
        '_ReasonDT.Columns.Add("Priority", System.Type.GetType("System.Int32"))
        '_ReasonDT.Columns.Add("Print_SW", System.Type.GetType("System.Boolean"))
        '_ReasonDT.Columns.Add("APPLY_STATUS")

        If claimDS IsNot Nothing Then

            _ClaimDS = claimDS

            'Dim QueryReason =
            '    From Reason In claimDS.Tables("REASON").AsEnumerable()
            '    Where Reason.RowState <> DataRowState.Deleted _
            '    AndAlso Reason.Field(Of Short)("LINE_NBR") = _LineNumber
            '    Order By Reason.Field(Of Integer)("PRIORITY")
            '    Select Reason

            'If QueryReason.Count > 0 Then
            '    For Each DR As DataRow In QueryReason
            '        _ClaimDSCopy.REASON.Rows.Add(DR.ItemArray)
            '    Next

            'End If

        End If

    End Sub

    'Form overrides dispose to clean up the component list.

    Public Overloads Sub Dispose()

        LineReasonsDataGrid.TableStyles.Clear()
        LineReasonsDataGrid.DataSource = Nothing
        LineReasonsDataGrid.Dispose()

        If _ClaimDSCopy IsNot Nothing Then
            _ClaimDSCopy.Dispose()
        End If
        _ClaimDSCopy = Nothing
        MyBase.Dispose()
    End Sub

#End Region

#Region "Public Properties"
    <System.ComponentModel.Description("Gets the modified ClaimDataset.")>
    Public ReadOnly Property ClaimDataset() As ClaimDataset
        Get
            Return _ClaimDSCopy
        End Get
    End Property
    <System.ComponentModel.Description("Gets the Reasonss.")>
    Public ReadOnly Property REASON() As DataTable
        Get
            Return _ReasonDT
        End Get
    End Property

    <System.ComponentModel.Description("Gets the Selected Codes as a single field.")>
    Public ReadOnly Property SelectedCodesFlat() As String
        Get

            If _ReasonDT IsNot Nothing AndAlso _ReasonDT.Rows.Count > 0 Then
                '  Dim FlattenReasonCodesQuery = String.Join(",", _ReasonDT.AsEnumerable.Select(Function(p) If(_Status.Contains("ALL"), If(CShort(p("LINE_NBR")) = 1, p("REASON").ToString, ""), p("REASON").ToString)))
                Dim FlattenReasonCodesQuery = String.Join(",", _ReasonDT.AsEnumerable.Select(Function(p) p("REASON").ToString))
                If FlattenReasonCodesQuery.Any Then
                    Return FlattenReasonCodesQuery
                End If
            End If

            Return Nothing
        End Get
    End Property

    <System.ComponentModel.Description("Gets the Selected Codes as an Array.")>
    Public ReadOnly Property SelectedCodesArray() As String()
        Get

            If _ReasonDT IsNot Nothing AndAlso _ReasonDT.Rows.Count > 0 Then
                Dim FlattenReasonCodesQuery = String.Join(",", _ReasonDT.AsEnumerable.Select(Function(p) p("REASON").ToString))

                If FlattenReasonCodesQuery.Any Then
                    Return FlattenReasonCodesQuery.Split(New Char() {CChar(",")}, StringSplitOptions.RemoveEmptyEntries)
                End If
            End If

            Return Nothing
        End Get
    End Property

    <System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)

            _APPKEY = value
        End Set
    End Property

    Public Property [ReadOnly]() As Boolean
        Get
            Return _ReadOnly
        End Get
        Set(ByVal value As Boolean)
            _ReadOnly = value

            If value = True Then
                LineReasonsDataGrid.Left = 4
                LineReasonsDataGrid.Top = 4
                LineReasonsDataGrid.Height = CancelActionButton.Top - 8
                LineReasonsDataGrid.Width = Me.Width - 16

                Me.AcceptButton = CancelActionButton
                ReasonCodesTextBox.Enabled = False
                ReasonCodesTextBox.TabStop = False
                ReasonCodesTextBox.Visible = False
                ReasonLookupButton.Enabled = False
                ReasonLookupButton.TabStop = False
                ReasonLookupButton.Visible = False
                AddActionButton.Enabled = False
                AddActionButton.TabStop = False
                AddActionButton.Visible = False
                DeleteActionButton.Enabled = False
                DeleteActionButton.TabStop = False
                DeleteActionButton.Visible = False
                SortUpButton.Enabled = False
                SortUpButton.TabStop = False
                SortUpButton.Visible = False
                SortDownButton.Enabled = False
                SortDownButton.TabStop = False
                SortDownButton.Visible = False
                SaveActionButton.Enabled = False
                SaveActionButton.TabStop = False
                SaveActionButton.Visible = False

                CancelActionButton.TabStop = False
            Else
                LineReasonsDataGrid.Left = 8
                LineReasonsDataGrid.Top = 40
                LineReasonsDataGrid.Height = Me.Height - 96
                LineReasonsDataGrid.Width = Me.Width - 56

                Me.AcceptButton = SaveActionButton
                ReasonCodesTextBox.Enabled = True
                ReasonCodesTextBox.TabStop = True
                ReasonCodesTextBox.Visible = True
                ReasonLookupButton.Enabled = True
                ReasonLookupButton.TabStop = True
                ReasonLookupButton.Visible = True
                AddActionButton.Enabled = True
                AddActionButton.TabStop = True
                AddActionButton.Visible = True
                DeleteActionButton.Enabled = True
                DeleteActionButton.TabStop = True
                DeleteActionButton.Visible = True
                SortUpButton.Enabled = True
                SortUpButton.TabStop = True
                SortUpButton.Visible = True
                SortDownButton.Enabled = True
                SortDownButton.TabStop = True
                SortDownButton.Visible = True
                SaveActionButton.Enabled = True
                SaveActionButton.TabStop = True
                SaveActionButton.Visible = True

                CancelActionButton.TabStop = True
            End If
        End Set
    End Property

    Public Property GridLines() As Integer
        Get
            Return _GridLines
        End Get
        Set(ByVal value As Integer)
            _GridLines = value
        End Set
    End Property

    Public Property Status() As String
        Get
            Return _Status
        End Get
        Set(ByVal value As String)
            _Status = value
        End Set
    End Property
#End Region

#Region "Form Events"
    Private Sub UpdateAllButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles updateAllButton.Click
        Try
            _Status = "UPDATEALL"
            AddUpdateReason()
            '    If UpdateAllReasons() Then
            Me.DialogResult = DialogResult.OK
            Me.Close()
            '       End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DetailLineReasons_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            SetSettings()
            Me.Text = "Claim [" & _ClaimID & "] Line " & _LineNumber & " Reasons"

            LoadReasons()

            LineReasonsDataGrid.ContextMenuPrepare(ReasonCustomContextMenu)
            ReasonCodesTextBox.Select()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DetailLineReasons_FormClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.FormClosing

        SaveSettings()

    End Sub

    Private Sub CancelActionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelActionButton.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SaveActionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveActionButton.Click
        Try
            _Status = "UPDATELINE"
            AddUpdateReason()
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ReasonLookupButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReasonLookupButton.Click
        Dim Frm As New ReasonLookup
        Dim Codes As String = ""

        Try
            If Frm.ShowDialog(Me) = DialogResult.OK Then
                For Cnt As Integer = 0 To Frm.ReasonsDataGrid.GetGridRowCount - 1
                    If Frm.ReasonsDataGrid.IsSelected(Cnt) Then
                        Codes &= If(Codes <> "", ", ", "") & Frm.ReasonsDataGrid.Item(Cnt, CInt(Frm.ReasonsDataGrid.GetColumnPosition("REASON"))).ToString
                    End If
                Next

                If Codes <> "" Then
                    If ReasonCodesTextBox.Text = "" Then
                        ReasonCodesTextBox.Text = Codes
                    Else
                        ReasonCodesTextBox.Text &= ", " & Codes
                    End If
                End If
            End If
        Catch ex As Exception
            Throw
        Finally
            ReasonCodesTextBox.Focus()
        End Try
    End Sub

    Private Sub AddActionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddActionButton.Click

        Try
            '   If AddUpdateReason() Then AddReason()
            AddUpdateReason()

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub DeleteActionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteActionButton.Click, DeleteMenuItem.Click
        Try
            Delete()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ClearLineButton_Click(sender As System.Object, e As System.EventArgs) Handles ClearLineButton.Click
        Try

            _Status = "CLEARLINE"
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ClearAllButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ClearAllButton.Click
        Try

            _Status = "CLEARALL"
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    'Private Sub AddMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddMenuItem.Click
    '    Try
    '        AddActionButton.PerformClick()
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub

    Private Sub LineReasonsDataGrid_OnDelete(ByRef Cancel As Boolean) Handles LineReasonsDataGrid.OnDelete
        Try
            DeleteActionButton.PerformClick()

            'required to stop the grid from performing its own delete (causes an error on last delete)
            Cancel = True
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SortUpButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SortUpButton.Click
        Try
            SortItemsUp()
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub SortDownButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SortDownButton.Click
        Try
            SortItemsDown()
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub ReasonCodesTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReasonCodesTextBox.TextChanged
        If ReasonCodesTextBox.Text.Length = 0 Then
            AddActionButton.Enabled = False
        Else
            If LineReasonsDataGrid.GetGridRowCount < MAXREASONS Then
                AddActionButton.Enabled = True
            Else
                AddActionButton.Enabled = False
            End If
        End If
    End Sub

    Private Sub LineReasonsDataGrid_RowCountChanged(ByVal previousRowCount As Integer?, ByVal currentRowCount As Integer) Handles LineReasonsDataGrid.RowCountChanged
        currentRowCount = LineReasonsDataGrid.GetGridRowCount
        If currentRowCount = 0 Then
            DeleteActionButton.Enabled = False
            SortUpButton.Enabled = False
            SortDownButton.Enabled = False
        Else
            DeleteActionButton.Enabled = True
            SortUpButton.Enabled = True
            SortDownButton.Enabled = True
        End If

        If currentRowCount >= MAXREASONS Then
            ReasonCodesTextBox.Enabled = False
            AddActionButton.Enabled = False
        Else
            ReasonCodesTextBox.Enabled = True

            If ReasonCodesTextBox.Text.Length > 0 Then
                AddActionButton.Enabled = True
            End If
        End If
    End Sub

    Private Sub ReasonCodesTextBox_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ReasonCodesTextBox.EnabledChanged
        ReasonLookupButton.Enabled = ReasonCodesTextBox.Enabled
    End Sub

#End Region

#Region "Private Methods"
    Private Sub LoadReasons()
        Try
            _ReasonsBS = New BindingSource With {
                .DataSource = _ReasonDT,
                .Sort = "PRIORITY"
            }
            ' LineReasonsDataGrid.SuspendLayout()
            LineReasonsDataGrid.DataSource = _ReasonsBS
            '  LineReasonsDataGrid.ResumeLayout()
            LineReasonsDataGrid.SetTableStyle()
            'If _ReasonsBS IsNot Nothing AndAlso _ReasonsBS.Position > -1 Then
            '    LineReasonsDataGrid.Select(_ReasonsBS.Position)
            'End If
            LineReasonsDataGrid_RowCountChanged(-1, -1)
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub SortItemsUp()
        If _ReasonsBS Is Nothing AndAlso _ReasonsBS.Position < 0 Then Exit Sub

        Dim DR As DataRow

        Try
            Using WC As New GlobalCursor

                DR = CType(_ReasonsBS.Current, DataRowView).Row

                Dim AdjustItemsQuery = (From r In _ReasonDT.AsEnumerable
                                        Where r.Field(Of Integer)("PRIORITY") < CInt(DR("PRIORITY"))
                                        Order By r.Field(Of Integer)("PRIORITY") Descending).FirstOrDefault

                If AdjustItemsQuery Is Nothing Then Exit Sub

                'if gaps in priority existed in the original list this will retain gaps
                Dim PrioritySwap As Integer = CInt(AdjustItemsQuery("PRIORITY"))
                    AdjustItemsQuery("PRIORITY") = DR("PRIORITY")
                    DR("PRIORITY") = PrioritySwap
                _ReasonsBS.EndEdit()

            End Using

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SortItemsDown()
        If _ReasonsBS Is Nothing AndAlso _ReasonsBS.Position < 0 Then Exit Sub

        Dim DR As DataRow

        Try
            Using WC As New GlobalCursor

                DR = CType(_ReasonsBS.Current, DataRowView).Row

                Dim AdjustItemsQuery = (From r In _ReasonDT.AsEnumerable
                                        Where r.Field(Of Integer)("PRIORITY") > CInt(DR("PRIORITY"))
                                        Order By r.Field(Of Integer)("PRIORITY")).FirstOrDefault

                If AdjustItemsQuery Is Nothing Then Exit Sub

                'if gaps in priority existed in the original list this will retain gaps
                Dim PrioritySwap As Integer = CInt(AdjustItemsQuery("PRIORITY"))
                AdjustItemsQuery("PRIORITY") = DR("PRIORITY")
                DR("PRIORITY") = PrioritySwap
                _ReasonsBS.EndEdit()

            End Using

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function AddReason() As Boolean
        Dim Source As String = ReasonCodesTextBox.Text
        Dim Reasons() As String
        Dim DR As DataRow
        Dim Reason As String
        Dim ReasonDR As DataRow

        Try
            Source = Source.Replace(" ", "")

            Reasons = Source.ToUpper.Split(CChar(","))

            For Each Reason In Reasons
                ReasonDR = CMSDALFDBMD.RetrieveReasonValuesInformation(Reason, _DateOfService)
                If _ReasonsBS.Find("REASON", Reason) < 0 Then
                    DR = _ReasonDT.NewRow
                    DR("CLAIM_ID") = _ClaimID
                    DR("LINE_NBR") = _LineNumber
                    DR("REASON") = Reason
                    DR("DESCRIPTION") = CStr(ReasonDR("DESCRIPTION"))
                    Dim NextPriorityQuery As Integer = 0
                    If _ReasonDT.Rows.Count > 0 Then
                        NextPriorityQuery = (From r In _ReasonDT.AsEnumerable Select r.Field(Of Integer)("PRIORITY")).Max
                        NextPriorityQuery += 1
                    End If
                    DR("PRIORITY") = NextPriorityQuery '1st entry should be zero.
                    DR("PRINT_SW") = ReasonDR("PRINT_SW")
                    DR("APPLY_STATUS") = ReasonDR("APPLY_STATUS")
                    _ReasonDT.Rows.Add(DR)
                End If
            Next
            ReasonCodesTextBox.Text = ""
            _ReasonsBS.EndEdit()
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub Delete()
        Dim DR As DataRow
        Dim GridDV As DataView

        Try

            Using WC As New GlobalCursor

                ' If LineReasonsDataGrid.GetSelectedDataRows.Count <> 1 Then Exit Sub
                If _ReasonsBS Is Nothing OrElse _ReasonsBS.Current Is Nothing Then Exit Sub

                DR = CType(_ReasonsBS.Current, DataRowView).Row
                DR.Delete()

                'Re-Prioritize
                GridDV = LineReasonsDataGrid.GetDefaultDataView

                For Cnt As Integer = 0 To GridDV.Count - 1
                    GridDV(Cnt).Row.Item("PRIORITY") = Cnt
                Next
                _ReasonsBS.EndEdit()

                DirectCast(_ReasonsBS.DataSource, DataTable).AcceptChanges()

            End Using
        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub AddUpdateReason()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' This will add reasons to the grid
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[sbandi]	9/04/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim Source As String = ReasonCodesTextBox.Text
        Dim Reasons() As String
        Dim Reason As String
        Dim ReasonDR As DataRow
        Dim DR As DataRow
        Dim DV As DataView
        Try
            Source = Source.Replace(" ", "")

            If Source.Length = 0 Then Return

            Reasons = Source.ToUpper.Split(CChar(","))

            If (Reasons.Length + LineReasonsDataGrid.GetGridRowCount) > MAXREASONS Then
                ReasonCodesTextBox.SelectionStart = 0
                ReasonCodesTextBox.SelectionLength = ReasonCodesTextBox.Text.Length
                MessageBox.Show("Only " & MAXREASONS & " Reasons Are Allowed.  Please Select The Best Match", "Too Many Reasons", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ReasonCodesTextBox.Focus()
                Exit Sub
            End If

            For Each RC As String In Reasons
                Reason = RC
                If Reason.Length > 3 Then
                    MessageBox.Show("Reason " & """" & RC & """" & " is not a valid reason code.", "Invalid Reason", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ReasonCodesTextBox.Focus()
                    Exit Try
                Else
                    If IsNumeric(Reason) = True Then
                        Reason = Reason.PadLeft(3, CChar("0"))
                    End If
                End If
                ReasonDR = CMSDALFDBMD.RetrieveReasonValuesInformation(Reason, _DateOfService)
                If ReasonDR Is Nothing Then
                    If ReasonCodesTextBox.Text.Length > 0 Then
                        ReasonCodesTextBox.SelectionStart = InStr(ReasonCodesTextBox.Text.ToUpper, RC) - 1
                        ReasonCodesTextBox.SelectionLength = RC.Length
                        MessageBox.Show("Reason " & """" & RC & """" & " is not a valid reason code.", "Invalid Reason", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        ReasonCodesTextBox.Focus()
                        Exit Sub
                    End If
                Else
                    DV = New DataView(_ReasonDT, "REASON = '" & Reason & "'", "REASON", DataViewRowState.CurrentRows)
                    If DV.Count = 0 Then
                        DR = _ReasonDT.NewRow
                        ''DR("Name") = Reason
                        ''DR("Desc") = CStr(ReasonDR("DESCRIPTION"))
                        ''Dim NextPriorityQuery As Integer = 0
                        ''If _ReasonDT.Rows.Count > 0 Then
                        ''    NextPriorityQuery = (From r In _ReasonDT.AsEnumerable Select r.Field(Of Integer)("PRIORITY")).Max
                        ''    NextPriorityQuery += 1
                        ''End If
                        ''DR("PRIORITY") = NextPriorityQuery '1st entry should be zero.
                        ''DR("Print_SW") = ReasonDR("PRINT_SW")
                        ''DR("APPLY_STATUS") = ReasonDR("APPLY_STATUS")
                        ''_ReasonDT.Rows.Add(DR)
                        DR("CLAIM_ID") = _ClaimID
                        DR("LINE_NBR") = _LineNumber
                        DR("REASON") = Reason
                        DR("DESCRIPTION") = CStr(ReasonDR("DESCRIPTION"))
                        Dim NextPriorityQuery As Integer = 0
                        If _ReasonDT.Rows.Count > 0 Then
                            NextPriorityQuery = (From r In _ReasonDT.AsEnumerable Select r.Field(Of Integer)("PRIORITY")).Max
                            NextPriorityQuery += 1
                        End If
                        DR("PRIORITY") = NextPriorityQuery '1st entry should be zero.
                        DR("PRINT_SW") = ReasonDR("PRINT_SW")
                        DR("APPLY_STATUS") = ReasonDR("APPLY_STATUS")
                        _ReasonDT.Rows.Add(DR)
                    End If
                End If

            Next
            _ClaimDSCopy.REASON.Clear()
            _ClaimDSCopy.REASON.Merge(_ReasonDT)
            ''For Cnt As Integer = 0 To _ReasonDT.Rows.Count - 1
            ''    DR = _ClaimDSCopy.REASON.NewRow

            ''    DR("CLAIM_ID") = _ClaimID
            ''    DR("LINE_NBR") = _LineNumber
            ''    DR("REASON") = _ReasonDT.Rows(Cnt)("Name")
            ''    DR("DESCRIPTION") = _ReasonDT.Rows(Cnt)("Desc")
            ''    DR("PRIORITY") = _ReasonDT.Rows(Cnt)("PRIORITY")
            ''    DR("PRINT_SW") = _ReasonDT.Rows(Cnt)("Print_SW")
            ''    DR("APPLY_STATUS") = _ReasonDT.Rows(Cnt)("APPLY_STATUS")

            ''    _ClaimDSCopy.REASON.Rows.Add(DR)
            ''Next

            ReasonCodesTextBox.Text = ""

            '_ReasonsBS.EndEdit()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function UpdateAllReasons() As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Adding all reasons to grid at a time
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[sbandi]	9/04/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DR As DataRow
        Dim UpdateDV As DataView

        Try
            _ClaimDSCopy.REASON.Clear()

            For GLines As Integer = 1 To Me.GridLines
                UpdateDV = New DataView(_ClaimDS.MEDDTL, "STATUS <> 'MERGED' AND LINE_NBR = " & GLines, "Line_NBR", DataViewRowState.CurrentRows)
                If UpdateDV.Count > 0 Then
                    For Cnt As Integer = 0 To _ReasonDT.Rows.Count - 1
                        DR = _ClaimDSCopy.REASON.NewRow

                        DR("CLAIM_ID") = _ClaimID
                        DR("LINE_NBR") = GLines
                        DR("REASON") = _ReasonDT.Rows(Cnt)("Name")
                        DR("DESCRIPTION") = _ReasonDT.Rows(Cnt)("Desc")
                        DR("PRIORITY") = _ReasonDT.Rows(Cnt)("Priority")
                        DR("PRINT_SW") = _ReasonDT.Rows(Cnt)("Print_SW")
                        DR("APPLY_STATUS") = _ReasonDT.Rows(Cnt)("APPLY_STATUS")

                        _ClaimDSCopy.REASON.Rows.Add(DR)
                    Next
                End If
            Next

            Return True

        Catch ex As Exception
            Throw
        Finally
            _ReasonDT = Nothing
        End Try
    End Function

    Private Sub SetSettings()
        Me.Top = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)))
        Me.Height = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)))
        Me.Width = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)
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

        Dim lWindowState As FormWindowState = Me.WindowState
        SaveSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(lWindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = lWindowState

    End Sub
#End Region

End Class