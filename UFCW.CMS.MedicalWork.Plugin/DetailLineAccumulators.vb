Imports System.ComponentModel
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling


Public Class DetailLineAccumulators
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _ClaimID As Integer
    Private _LineNumber As Short
    Private _AccumManager As MemberAccumulatorManager
    Private _ReadOnly As Boolean
    Private _EntryVal As Decimal
    Private _GridLines As Integer
    Private _Status As String = ""
    Private _UpdateDT As DataTable
    Private _UpdateAllMode As Boolean
    Private WithEvents _LineAccumulatorsDT As DataTable

    Private _APPKEY As String = "UFCW\Claims\"

#Region "Custom Constructor/Destructor "
    Public Sub New(ByVal claimID As Integer, ByVal lineNumber As Short, ByVal detailLineAccumulatorsDT As DataTable, ByVal [readOnly] As Boolean, Optional ByVal updateAllMode As Boolean = False)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _ClaimID = claimID
        _LineNumber = lineNumber
        _ReadOnly = [readOnly]

        _UpdateAllMode = updateAllMode    '- ---LM 03/13/2024
        _LineAccumulatorsDT = CMSDALCommon.CreateAccumulatorValuesDT

        _UpdateDT = CMSDALCommon.CreateAccumulatorValuesDT

        If detailLineAccumulatorsDT IsNot Nothing Then

            Dim DetailLineAccumulatorsDV As New DataView(detailLineAccumulatorsDT, "", "", DataViewRowState.CurrentRows)

            DetailLineAccumulatorsDV.Sort = "LINE_NBR, DISPLAY_ORDER"
            DetailLineAccumulatorsDV.RowFilter = "ENTRY_VALUE <> 0 AND LINE_NBR = " & _LineNumber

            If DetailLineAccumulatorsDV.Count > 0 Then
                For Cnt As Integer = 0 To DetailLineAccumulatorsDV.Count - 1
                    _LineAccumulatorsDT.Rows.Add(DetailLineAccumulatorsDV(Cnt).Row.ItemArray)
                Next
            End If

            _LineAccumulatorsDT.AcceptChanges()
        End If
    End Sub

    'Form overrides dispose to clean up the component list.

#End Region

#Region "Public Properties"

    <System.ComponentModel.Description("Represents the application key used when accessing the registry.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
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
    Public ReadOnly Property LineAccumulatorsDT() As DataTable
        Get
            Return _LineAccumulatorsDT
        End Get
    End Property

    Public Property UpdateAllMode() As Boolean
        Get
            Return _UpdateAllMode
        End Get
        Set(ByVal value As Boolean)
            _UpdateAllMode = value
        End Set
    End Property

#End Region

#Region "Form Events"
    Private Sub DetailLineAccumulators_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            LoadAccumulators()

            AddButton.Enabled = Not _ReadOnly 'False
            DeleteMenuItem.Visible = Not _ReadOnly 'False

            If _UpdateAllMode Then
                ClearAllButton.Visible = False
                UpdateLineButton.Visible = False
                ClearLineButton.Visible = False
                Me.Text = "Claim [" & _ClaimID & "] ALL  Accumulators"
            Else
                Me.Text = "Claim [" & _ClaimID & "] Line " & _LineNumber & " Accumulators"
            End If
            LineAccumulatorsDataGrid.AllowDelete = Not _ReadOnly 'False


        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DetailLineAccumulators_FormClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.FormClosing
        Try
            SaveSettings()
        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

#Region "Menu\Button Events"

    Private Sub CancelActionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelActionButton.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Sub AddButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddButton.Click
        Try
            AddAccumulator()
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub DeleteActionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteMenuItem.Click
        Try
            Delete()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    'Private Sub AddMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddMenuItem.Click
    '    Try
    '        AddButton.PerformClick()
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub

    Private Sub SaveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateLineButton.Click
        Try

            Me.Status = "UpdateLine"
            If AccumulatorTextBox.Text <> "" Then
                If AddAccumulator() Then
                    Me.DialogResult = DialogResult.OK
                    Me.Close()
                End If
            Else
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub UpdateAllButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateAllButton.Click

        Try
            Me.Status = "UpdateAll"

            If AccumulatorTextBox.Text <> "" Then
                If AddUpdateAccumulator() = True Then
                    Me.DialogResult = DialogResult.OK
                    Me.Close()
                End If
            Else
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub ClearAllButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearAllButton.Click
        Me.Status = "ClearAll"
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub ClearLineButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearLineButton.Click
        Me.Status = "ClearLine"
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub AccumulatorLookupButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AccumulatorLookupButton.Click

        Dim DT As DataTable
        Dim DV As DataView

        Dim F As AccumulatorLookup
        Dim Codes As String = ""
        Dim Cnt As Integer = 0
        Dim DispOrder As Integer = 99999
        Dim DR As DataRow
        Dim source As String
        Dim selectedAccums() As String

        Try
            DT = AccumulatorController.GetAccumulators.Copy
            DV = New DataView(DT, " ACCUM_NAME Not Like 'AC%'", "ACCUM_NAME", DataViewRowState.CurrentRows)

            F = New AccumulatorLookup(DV.ToTable)

            If F.ShowDialog(Me) = DialogResult.OK Then
                _EntryVal = F.entryValue

                source = F.selectAccums
                selectedAccums = source.Split(CChar(","))

                For Cnt = 0 To selectedAccums.Length - 1
                    DV = New DataView(_LineAccumulatorsDT, "ACCUM_NAME = '" & selectedAccums(Cnt) & "'", "ACCUM_NAME", DataViewRowState.CurrentRows)
                    If DV.Count = 0 Then
                        DR = _LineAccumulatorsDT.NewRow
                        DR("CLAIM_ID") = _ClaimID
                        DR("ACCUM_NAME") = selectedAccums(Cnt).Trim()
                        DR("ENTRY_VALUE") = _EntryVal
                        DR("OVERRIDE_SW") = True
                        DR("LINE_NBR") = _LineNumber
                        DR("DISPLAY_ORDER") = DispOrder

                        _LineAccumulatorsDT.Rows.Add(DR)
                    End If
                Next

            End If
        Catch ex As Exception
            Throw
        Finally
            LineAccumulatorsDataGrid.Focus()
        End Try
    End Sub

    Private Sub AccumulatorTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles AccumulatorTextBox.TextChanged
        If AccumulatorTextBox.Text.Length = 0 Then
            AddButton.Enabled = False
        Else
            AddButton.Enabled = True
        End If
    End Sub

    Private Sub LineAccumulatorsDataGrid_RowCountChanged(ByVal previousRowCount As Integer?, ByVal currentRowCount As Integer) Handles LineAccumulatorsDataGrid.RowCountChanged
        If LineAccumulatorsDataGrid.GetGridRowCount > 0 Then
            Me.UpdateAllButton.Enabled = True
        Else
            Me.UpdateAllButton.Enabled = True
        End If
    End Sub

#End Region
    Private Sub LoadAccumulators()
        Try
            LineAccumulatorsDataGrid.DataSource = _LineAccumulatorsDT
            LineAccumulatorsDataGrid.SetTableStyle()
            LineAccumulatorsDataGrid.ContextMenuPrepare(AccumCustomContextMenu)
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub Delete()
        Dim DR As DataRow
        Dim BM As BindingManagerBase
        Try

            BM = Me.LineAccumulatorsDataGrid.BindingContext(Me.LineAccumulatorsDataGrid.DataSource, Me.LineAccumulatorsDataGrid.DataMember)

            If BM Is Nothing OrElse BM.Count < 1 Then Exit Sub

            Using WC As New GlobalCursor

                DR = CType(BM.Current, DataRowView).Row

                If DR Is Nothing OrElse (DR.RowState <> DataRowState.Added) Then Return

                DR.Delete()

            End Using
        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Function AddAccumulator() As Boolean
        Dim Source As String = AccumulatorTextBox.Text.Replace(" ", "")
        Dim Accumulators() As String
        Dim Accumulator As String
        Dim FoundRows() As DataRowView
        Dim DR As DataRow
        Dim DispOrder As Integer = 99999
        Dim OriginalDT As DataTable
        Dim DT As DataTable
        Dim AccumDV As DataView
        Dim DV As DataView
        Dim AccumOriginalDV As DataView

        If Source.StartsWith("AC") Then
            MessageBox.Show("Accident accumulator(s) " & Source & " cannot be added directly.", "Use Incident Date to Add Accident Accumulator", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Function
        End If

        Try

            OriginalDT = AccumulatorController.GetAccumulators.Copy
            AccumOriginalDV = New DataView(_LineAccumulatorsDT, "", "ACCUM_NAME", DataViewRowState.CurrentRows)

            Accumulators = Source.ToUpper.Split(CChar(","))

            DT = CMSDALCommon.CreateAccumulatorValuesDT

            For Cnt As Integer = 0 To Accumulators.Length - 1
                Accumulator = Accumulators(Cnt).ToUpper.Trim
                AccumDV = New DataView(OriginalDT, "ACTIVE_SW = 1", "ACCUM_NAME", DataViewRowState.CurrentRows)
                ''checking whether the entered value is valid
                FoundRows = AccumDV.FindRows(New Object() {Accumulator})

                If FoundRows.Length > 0 Then
                    DV = New DataView(DT, "ACCUM_NAME = '" & Accumulator & "'", "ACCUM_NAME", DataViewRowState.CurrentRows)
                    If DV.Count = 0 Then
                        DV = New DataView(_LineAccumulatorsDT, "ACCUM_NAME = '" & Accumulator & "'", "ACCUM_NAME", DataViewRowState.CurrentRows)
                        ''checking the duplicate values
                        If DV.Count = 0 Then
                            DR = DT.NewRow
                            DR("CLAIM_ID") = _ClaimID
                            DR("ACCUM_NAME") = Accumulator
                            DR("ENTRY_VALUE") = _EntryVal
                            DR("OVERRIDE_SW") = True
                            DR("LINE_NBR") = _LineNumber
                            DR("DISPLAY_ORDER") = DispOrder

                            DT.Rows.Add(DR)
                        End If
                    End If
                Else
                    MessageBox.Show("Accumulator " & """" & Accumulators(Cnt) & """" & " is not a valid Accumulator code.", "Invalid Accumulator", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    AccumulatorTextBox.Focus()
                    Exit Try
                End If
            Next

            For Cnt As Integer = 0 To DT.Rows.Count - 1
                DR = _LineAccumulatorsDT.NewRow
                DR("CLAIM_ID") = _ClaimID
                DR("ACCUM_NAME") = DT.Rows(Cnt)("ACCUM_NAME")
                DR("ENTRY_VALUE") = _EntryVal
                DR("OVERRIDE_SW") = True
                DR("LINE_NBR") = _LineNumber
                DR("DISPLAY_ORDER") = DispOrder

                _LineAccumulatorsDT.Rows.Add(DR)
            Next

            AccumulatorTextBox.Text = ""
            Return True

        Catch ex As Exception
            Throw
        Finally

            AccumDV = Nothing
            DT = Nothing

        End Try
    End Function

    Private Function AddUpdateAccumulator() As Boolean
        'when user click on updateall button this method will invoke

        Dim SpecifiedAccumulators As String = AccumulatorTextBox.Text.ToString.Trim
        Dim Accumulators() As String
        Dim Accumulator As String
        Dim FoundDRV() As DataRowView
        Dim DR As DataRow
        Dim DispOrder As Integer = 99999
        Dim OriginalDT As DataTable
        Dim AccumDV As DataView
        Dim DV As DataView

        Try
            OriginalDT = AccumulatorController.GetAccumulators.Copy

            SpecifiedAccumulators = SpecifiedAccumulators.Replace(" ", "")
            Accumulators = SpecifiedAccumulators.ToUpper.Split(CChar(","))

            For Cnt As Integer = 0 To Accumulators.Length - 1
                Accumulator = Accumulators(Cnt).ToUpper.Trim
                AccumDV = New DataView(OriginalDT, "", "ACCUM_NAME", DataViewRowState.CurrentRows)
                ''checking whether the entered value is valid
                FoundDRV = AccumDV.FindRows(New Object() {Accumulator})
                If FoundDRV.Length > 0 Then
                    DV = New DataView(_UpdateDT, "ACCUM_NAME = '" & Accumulator & "'", "ACCUM_NAME", DataViewRowState.CurrentRows)
                    If DV.Count = 0 Then
                        DV = New DataView(_LineAccumulatorsDT, "ACCUM_NAME = '" & Accumulator & "'", "ACCUM_NAME", DataViewRowState.CurrentRows)
                        ''checking the duplicate values
                        If DV.Count = 0 Then
                            DR = _UpdateDT.NewRow
                            DR("CLAIM_ID") = _ClaimID
                            DR("ACCUM_NAME") = Accumulator
                            DR("ENTRY_VALUE") = _EntryVal
                            DR("OVERRIDE_SW") = True
                            DR("LINE_NBR") = _LineNumber
                            DR("DISPLAY_ORDER") = DispOrder

                            _UpdateDT.Rows.Add(DR)
                        End If
                    End If
                Else
                    MessageBox.Show("Accumulator " & """" & Accumulators(Cnt) & """" & " is NOT a valid Accumulator code.", "Invalid Accumulator", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    AccumulatorTextBox.Focus()
                    Exit Try
                End If
            Next

            For Cnt As Integer = 0 To _UpdateDT.Rows.Count - 1
                DR = _LineAccumulatorsDT.NewRow
                DR("CLAIM_ID") = _ClaimID
                DR("ACCUM_NAME") = _UpdateDT.Rows(Cnt)("ACCUM_NAME")
                DR("ENTRY_VALUE") = _UpdateDT.Rows(Cnt)("ENTRY_VALUE")
                DR("OVERRIDE_SW") = _UpdateDT.Rows(Cnt)("OVERRIDE_SW")
                DR("LINE_NBR") = _UpdateDT.Rows(Cnt)("LINE_NBR")
                DR("DISPLAY_ORDER") = _UpdateDT.Rows(Cnt)("DISPLAY_ORDER")

                _LineAccumulatorsDT.Rows.Add(DR)
            Next
            AccumulatorTextBox.Text = ""

            Return True

        Catch ex As Exception
            Throw
        Finally
            AccumDV = Nothing
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
        Dim TheWindowState As FormWindowState = Me.WindowState
        SaveSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(TheWindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = TheWindowState

        SaveColSettings()
    End Sub
    Private Sub SaveColSettings()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Saves column settings.
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	11/16/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        If LineAccumulatorsDataGrid IsNot Nothing Then
            If LineAccumulatorsDataGrid.DataSource IsNot Nothing Then
                LineAccumulatorsDataGrid.SaveColumnsSizeAndPosition(LineAccumulatorsDataGrid.Name & "\" & LineAccumulatorsDataGrid.GetCurrentDataTable.TableName & "\ColumnSettings")
                LineAccumulatorsDataGrid.SaveSortBy(LineAccumulatorsDataGrid.LastSortedBy)
            End If
        End If

    End Sub

End Class