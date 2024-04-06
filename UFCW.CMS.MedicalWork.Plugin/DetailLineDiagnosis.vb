Option Infer On

Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class DetailLineDiagnosisForm
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _ClaimID As Integer
    Private _LineNumber As Short
    Private _DateOfService As Date?

    Private _APPKEY As String = "UFCW\Claims\"
    Private _Status As String = ""

    Const MAXDIAGNOSIS As Integer = 12

    Private _MedDiagDT As DataTable
    Private _MedDiagBS As BindingSource


#Region " Windows Form Designer generated code "
    Public Sub New(ByVal claimID As Integer, ByRef medDtlDR As DataRow, ByRef medDiagDT As DataTable)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _ClaimID = claimID
        _LineNumber = CShort(medDtlDR("LINE_NBR"))
        _DateOfService = CType(If(IsDBNull(medDtlDR("OCC_FROM_DATE")), UFCWGeneral.NowDate, medDtlDR("OCC_FROM_DATE")), Date?)

        _MedDiagDT = CloneHelper.DeepCopy(medDiagDT)

    End Sub

#End Region

#Region "Public Properties"
    <System.ComponentModel.Description("Gets the modified Diagnoses.")>
    Public ReadOnly Property MEDDIAG() As DataTable
        Get
            Return _MedDiagDT
        End Get
    End Property

    <System.ComponentModel.Description("Represents the application key used when accessing the registry.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
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
    Private Sub DetailLineDiagnosis_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            SetSettings()
            Me.Text = "Claim [" & _ClaimID & "] Line " & _LineNumber & " Diagnoses"

            LoadDiagnosis()

            'LineDiagnosisDataGrid.ContextMenuPrepare(DiagnosisCustomContextMenu)

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        End Try
    End Sub

    Private Sub DetailLineDiagnosis_FormClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.FormClosing
        SaveSettings()
    End Sub

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

    End Sub
#End Region

    Private Sub LoadDiagnosis()
        Try
            _MedDiagBS = New BindingSource With {
                .DataSource = _MedDiagDT,
                .Sort = "PRIORITY"
            }
            LineDiagnosisDataGrid.SuspendLayout()
            LineDiagnosisDataGrid.DataSource = _MedDiagBS
            LineDiagnosisDataGrid.ResumeLayout()
            LineDiagnosisDataGrid.SetTableStyle()
            If _MedDiagBS IsNot Nothing AndAlso _MedDiagBS.Position > -1 Then
                LineDiagnosisDataGrid.Select(_MedDiagBS.Position)
            End If
            LineDiagnosisDataGrid_RowCountChanged(-1, -1)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub CancelActionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelActionButton.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub DiagnosisLookupButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiagnosisLookupButton.Click

        Dim Frm As New DiagnosisLookupForm
        Dim Codes As String = ""

        Try
            Frm.DateOfService = CDate(If(IsDBNull(_DateOfService), UFCWGeneral.NowDate, _DateOfService)) 'used to limit diagnosis choices to only those active during claim line timeframe
            If Frm.ShowDialog(Me) = DialogResult.OK Then
                For Each dr As DataRow In Frm.DiagnosisLookupDataGrid.GetSelectedDataRows
                    Codes &= If(Codes <> "", ", ", "") & dr("DIAG_VALUE").ToString
                Next
                If Codes <> "" Then
                    If DiagnosisCodesTextBox.Text = "" Then
                        DiagnosisCodesTextBox.Text = Codes
                    Else
                        DiagnosisCodesTextBox.Text &= ", " & Codes
                    End If
                End If
            End If
        Catch ex As Exception
            Throw
        Finally
            If Frm IsNot Nothing Then
                Frm.Dispose()
            End If
            DiagnosisCodesTextBox.Focus()
        End Try
    End Sub

    Private Sub AddButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddButton.Click
        Try
            If ValidateAddedDiagnoses() Then AddDiagnoses()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DeleteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteButton.Click
        Try
            Delete()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LineDiagnosisDataGrid_OnDelete(ByRef cancel As Boolean) Handles LineDiagnosisDataGrid.OnDelete
        Try
            DeleteButton.PerformClick()
            'required to stop the grid from performing its own delete (causes an error on last delete)
            cancel = True
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ClearLineButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearLineButton.Click
        Me.Status = "CLEARLINE"
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub ClearAllButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ClearAllButton.Click
        Me.Status = "CLEARALL"
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub AddMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddMenuItem.Click
        Try
            AddButton.PerformClick()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SortUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SortUpButton.Click
        Try
            SortItemsUp()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SortDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SortDownButton.Click
        Try
            SortItemsDown()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SortItemsUp()

        Dim DR As DataRow

        Try
            If _MedDiagBS Is Nothing AndAlso _MedDiagBS.Position < 0 Then Exit Sub

            Using WC As New GlobalCursor

                DR = CType(_MedDiagBS.Current, DataRowView).Row

                Dim AdjustItemsQuery = (From r In _MedDiagDT.AsEnumerable
                                        Where r.Field(Of Integer)("PRIORITY") < CInt(DR("PRIORITY"))
                                        Order By r.Field(Of Integer)("PRIORITY") Descending).FirstOrDefault

                If AdjustItemsQuery Is Nothing Then Return

                'if gaps in priority existed in the original list this will retain gaps
                Dim PrioritySwap As Integer = CInt(AdjustItemsQuery("PRIORITY"))
                AdjustItemsQuery("PRIORITY") = DR("PRIORITY")
                DR("PRIORITY") = PrioritySwap
                _MedDiagBS.EndEdit()

            End Using

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub SortItemsDown()

        Dim DR As DataRow

        Try

            If _MedDiagBS Is Nothing AndAlso _MedDiagBS.Position < 0 Then Return

            Using WC As New GlobalCursor

                DR = CType(_MedDiagBS.Current, DataRowView).Row

                Dim AdjustItemsQuery = (From r In _MedDiagDT.AsEnumerable
                                        Where r.Field(Of Integer)("PRIORITY") > CInt(DR("PRIORITY"))
                                        Order By r.Field(Of Integer)("PRIORITY")).FirstOrDefault

                If AdjustItemsQuery Is Nothing Then Return

                'if gaps in priority existed in the original list this will retain gaps
                Dim PrioritySwap As Integer = CInt(AdjustItemsQuery("PRIORITY"))
                AdjustItemsQuery("PRIORITY") = DR("PRIORITY")
                DR("PRIORITY") = PrioritySwap
                _MedDiagBS.EndEdit()

            End Using

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub Delete()
        Dim DR As DataRow
        Dim GridDV As DataView
        Try

            Using WC As New GlobalCursor

                If LineDiagnosisDataGrid.GetSelectedDataRows.Count <> 1 Then Return

                DR = CType(_MedDiagBS.Current, DataRowView).Row
                DR.Delete()

                'Re-Prioritize
                GridDV = LineDiagnosisDataGrid.GetDefaultDataView

                For Cnt As Integer = 0 To GridDV.Count - 1
                    GridDV(Cnt).Row.Item("PRIORITY") = Cnt
                Next

                DirectCast(_MedDiagBS.DataSource, DataTable).AcceptChanges()

                _MedDiagBS.EndEdit()

                LineDiagnosisDataGrid.RefreshData()

            End Using


        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub DiagnosisCodesTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiagnosisCodesTextBox.TextChanged
        If DiagnosisCodesTextBox.Text.Length = 0 Then
            AddButton.Enabled = False
        Else
            If LineDiagnosisDataGrid.GetGridRowCount < MAXDIAGNOSIS Then
                AddButton.Enabled = True
            Else
                AddButton.Enabled = False
            End If
        End If
    End Sub

    Private Sub LineDiagnosisDataGrid_RowCountChanged(ByVal previousRowCount As Integer?, ByVal currentRowCount As Integer) Handles LineDiagnosisDataGrid.RowCountChanged
        currentRowCount = LineDiagnosisDataGrid.GetGridRowCount
        If currentRowCount = 0 Then
            DeleteButton.Enabled = False
            SortUpButton.Enabled = False
            SortDownButton.Enabled = False
        Else
            DeleteButton.Enabled = True
            SortUpButton.Enabled = True
            SortDownButton.Enabled = True
        End If

        If currentRowCount >= MAXDIAGNOSIS Then
            DiagnosisCodesTextBox.Enabled = False
            AddButton.Enabled = False
        Else
            DiagnosisCodesTextBox.Enabled = True

            If DiagnosisCodesTextBox.Text.Length > 0 Then
                AddButton.Enabled = True
            End If
        End If

    End Sub

    Private Sub DiagnosisCodesTextBox_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DiagnosisCodesTextBox.EnabledChanged
        DiagnosisLookupButton.Enabled = DiagnosisCodesTextBox.Enabled
    End Sub

    Private Sub UpdateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateLineButton.Click, UpdateAllButton.Click

        If ValidateAddedDiagnoses() Then

            If AddDiagnoses() Then

                If CType(sender, Button).Name = "UpdateLineButton" Then
                    _Status = "UPDATELINE"
                Else
                    _Status = "UPDATEALL"
                End If

                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        End If

    End Sub

    Private Function ValidateAddedDiagnoses() As Boolean

        Dim Source As String = DiagnosisCodesTextBox.Text
        Dim DiagnosisCodes2Add() As String
        Dim DiagnosisDR As DataRow

        Try
            Source = Source.Replace(" ", "")

            DiagnosisCodes2Add = Source.ToUpper.Split(CChar(","))
            If DiagnosisCodes2Add.Length + _MedDiagBS.Count > MAXDIAGNOSIS Then
                DiagnosisCodesTextBox.SelectionStart = 0
                DiagnosisCodesTextBox.SelectionLength = DiagnosisCodesTextBox.Text.Length

                MessageBox.Show("Only " & MAXDIAGNOSIS & " Diagnoses are allowed.", "To Many Diagnoses", MessageBoxButtons.OK, MessageBoxIcon.Information)

                DiagnosisCodesTextBox.Focus()

                Return False

            End If

            If DiagnosisCodesTextBox.Text.Trim.Length > 0 Then
                For Each DiagnosisCode As String In DiagnosisCodes2Add

                    DiagnosisDR = CMSDALFDBMD.RetrieveDiagnosisValuesInformation(DiagnosisCode, _DateOfService)
                    If DiagnosisDR Is Nothing Then
                        DiagnosisCodesTextBox.SelectionStart = InStr(DiagnosisCodesTextBox.Text.ToUpper, DiagnosisCode) - 1
                        DiagnosisCodesTextBox.SelectionLength = DiagnosisCode.Length

                        MessageBox.Show("Diagnosis " & """" & DiagnosisCode & """" & " is not valid.", "Invalid Diagnosis", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        DiagnosisCodesTextBox.Focus()

                        Return False

                    End If
                Next
            End If

            Return True

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Function


    Private Function AddDiagnoses() As Boolean

        Dim Source As String = DiagnosisCodesTextBox.Text
        Dim Diagnoses() As String
        Dim DiagnosisDR As DataRow
        Try
            If Source.Trim.Length > 0 Then

                Source = Source.Replace(" ", "")

                Diagnoses = Source.ToUpper.Split(CChar(","))

                For Each Diagnosis As String In Diagnoses

                    DiagnosisDR = CMSDALFDBMD.RetrieveDiagnosisPreventativeStatusEffectiveAsOf(Diagnosis, _DateOfService)
                    If _MedDiagBS.Find("DIAGNOSIS", Diagnosis) < 0 Then
                        Dim DR As DataRow = _MedDiagDT.NewRow
                        DR("CLAIM_ID") = _ClaimID
                        DR("LINE_NBR") = _LineNumber
                        DR("DIAGNOSIS") = Diagnosis
                        DR("SHORT_DESC") = CStr(DiagnosisDR("SHORT_DESC"))
                        DR("FULL_DESC") = CStr(DiagnosisDR("FULL_DESC"))
                        DR("PREVENTATIVE_USE_SW") = CDec(DiagnosisDR("PREVENTATIVE_USE_SW"))

                        Dim NextPriorityQuery As Integer
                        If _MedDiagDT.Rows.Count > 0 Then
                            NextPriorityQuery = (From r In _MedDiagDT.AsEnumerable Select r.Field(Of Integer)("PRIORITY")).Max
                            NextPriorityQuery += 1
                        End If
                        DR("PRIORITY") = NextPriorityQuery '1st entry should be zero.
                        _MedDiagDT.Rows.Add(DR)
                    End If
                Next
                DiagnosisCodesTextBox.Text = ""
            End If
            _MedDiagBS.EndEdit()
            Return True

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Function
End Class