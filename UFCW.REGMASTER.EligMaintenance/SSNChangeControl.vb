Option Strict On
Imports System.Windows.Forms
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ComponentModel
Imports System.Configuration
Imports DDTek.DB2
Imports System.Data.Common
Imports System.Text.RegularExpressions

Public Class SSNChangeControl

    Private _OldFamilyID As Integer
    Private _NewFamilyID As Integer
    Private _APPKEY As String = "UFCW\RegMaster\"
    Private _TotalOldEligMthdtlDS As New EligMthdtlDS
    Private _TotalNewEligMthdtlDS As New EligMthdtlDS
    Private _TotalOldEligHrsDS As New EligAcctHoursDS
    Private _TotalNewEligHrsDS As New EligAcctHoursDS
    Private _DisplayColumnNamesDV As DataView
    Private _ChangedELGHRSRows As New EligAcctHoursDS
    Private _ChangedELGMTHRows As New EligMthdtlDS
    Private _SelectedFrom As Date
    Private _SelectedThru As Date
    Private _SelectedRemoveFrom As Date
    Private _SelectedRemoveThru As Date
    Private _MinEligPeriod As Date
    Private _MaxEligPeriod As Date
    Private _MailFrom As String = System.Configuration.ConfigurationManager.AppSettings("MailFrom")
    Private _SSNCorrectionTOEmail As String = System.Configuration.ConfigurationManager.AppSettings("SSNCorrectionTOEmail")
    Private _SSNCorrectionCCEmail As String = System.Configuration.ConfigurationManager.AppSettings("SSNCorrectionCCEmail")
    Private _SSNCorrectionMailSubject As String = System.Configuration.ConfigurationManager.AppSettings("SSNCorrectionMailSubject")

    Private _MinWorkPeriod As Date
    Private _MaxWorkPeriod As Date

#Region "Properties"

    <Browsable(True), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal Value As String)

            _APPKEY = Value
        End Set
    End Property

#End Region

    Private _Disposed As Boolean = False
    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.
            '
            If _ChangedELGMTHRows IsNot Nothing Then
                _ChangedELGMTHRows.Dispose()
            End If
            _ChangedELGMTHRows = Nothing

            If _ChangedELGHRSRows IsNot Nothing Then
                _ChangedELGHRSRows.Dispose()
            End If
            _ChangedELGHRSRows = Nothing

            If _DisplayColumnNamesDV IsNot Nothing Then
                _DisplayColumnNamesDV.Dispose()
            End If
            _DisplayColumnNamesDV = Nothing

            If _TotalNewEligHrsDS IsNot Nothing Then
                _TotalNewEligHrsDS.Dispose()
            End If
            _TotalNewEligHrsDS = Nothing

            If _TotalOldEligHrsDS IsNot Nothing Then
                _TotalOldEligHrsDS.Dispose()
            End If
            _TotalOldEligHrsDS = Nothing

            If _TotalNewEligMthdtlDS IsNot Nothing Then
                _TotalNewEligMthdtlDS.Dispose()
            End If
            _TotalNewEligMthdtlDS = Nothing

            If _TotalOldEligMthdtlDS IsNot Nothing Then
                _TotalOldEligMthdtlDS.Dispose()
            End If
            _TotalOldEligMthdtlDS = Nothing

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

#Region "Constructor"
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Dim designMode As Boolean = (LicenseManager.UsageMode = LicenseUsageMode.Designtime)
        If Not designMode Then
            btncancelhrs.Enabled = False
            btnSaveHrs.Enabled = False

            btnaddhrsrow.Enabled = False
            btnaddhrsallrow.Enabled = False
            btnremhrsrow.Enabled = False
            btnremhrsallrow.Enabled = False

            btnaddeligrow.Visible = False
            btnaddeligallrow.Visible = False
            btnremeligrow.Visible = False
            btnremeligallrow.Visible = False

        End If

        'dont want to display the default table style
    End Sub
#End Region

#Region "Form\Button Events"

    Private Sub btnoldsearch_Click(sender As Object, e As EventArgs) Handles btnoldsearch.Click
        Oldclearall()

        Try
            If Me.txtOldSSN.Text.Length = 0 Then

                MessageBox.Show("Please Enter FamilyID OR SSN")

            ElseIf txtOldSSN.Text.Length > 0 Then   '' SSN search

                ''checking two socials are equal
                If txtOldSSN.Text.Length > 0 AndAlso txtNewSSN.Text.Length > 0 Then
                    If txtNewSSN.Text.Equals(txtOldSSN.Text) Then
                        MessageBox.Show("Old social and New social are same", "Same SSN", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return
                    End If
                End If

                Dim ParticipentDS As DataSet = RegMasterDAL.RetrieveSecureRegMasterByPartSSNO(CInt((Me.txtOldSSN.Text)))
                If (ParticipentDS.Tables("REG_MASTER").Rows.Count > 0) Then
                    _OldFamilyID = CInt(ParticipentDS.Tables("REG_MASTER").Rows(0)("FAMILY_ID"))
                    Me.txtOldFID.Text = CStr(_OldFamilyID)

                    btnoldsearch.Enabled = False
                    txtOldFID.Enabled = False
                    txtOldFID.ReadOnly = True
                    txtOldSSN.Enabled = False
                    txtOldSSN.ReadOnly = True

                    LoadmthdtlwithOldSocial()
                    LoadOldSocialHours()
                Else
                    MessageBox.Show("No Matching Results", "No Records", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

            End If
        Catch ex As Exception

            Throw
        Finally
        End Try
    End Sub

    Private Sub btnoldClear_Click(sender As Object, e As EventArgs) Handles btnoldClear.Click

        If OldSSNDataGrid.DataSource IsNot Nothing Then
            OldSSNDataGrid.DataSource = Nothing

            OLD_ELIG_HOURS.Clear()
            If OldHoursDataGrid.DataSource IsNot Nothing Then
                OldHoursDataGrid.DataSource = Nothing
            End If
            OLD_EMD_DS.Clear()
            If OldSSNDataGrid.DataSource IsNot Nothing Then
                OldSSNDataGrid.DataSource = Nothing
            End If
        End If

        txtOldSSN.Clear()
        txtOldFID.Clear()

        txtOldSSN.Enabled = True
        txtOldSSN.ReadOnly = False
        btnoldsearch.Enabled = True

        btncancelhrs.Enabled = False
        btnSaveHrs.Enabled = False
        btnModifyHrs.Enabled = False

        btnaddhrsrow.Enabled = False
        btnaddhrsallrow.Enabled = False
        btnremhrsrow.Enabled = False
        btnremhrsallrow.Enabled = False

        btncancelelig.Visible = False
        btnsaveelig.Visible = False
        btnmodifyelig.Visible = False

        btnaddeligrow.Enabled = False
        btnaddeligallrow.Enabled = False
        btnremeligrow.Enabled = False
        btnremeligallrow.Enabled = False

        Oldclearall()
    End Sub

    Private Sub btnNewSearch_Click(sender As Object, e As EventArgs) Handles btnNewSearch.Click
        Try

            NEWclearall()
            If Me.txtNewSSN.Text.Length = 0 Then

                MessageBox.Show("Please Enter FamilyID OR SSN")

            ElseIf txtNewSSN.Text.Length > 0 Then   '' SSN search

                ''checking two socials are equal
                If txtOldSSN.Text.Length > 0 AndAlso txtNewSSN.Text.Length > 0 Then
                    If txtNewSSN.Text.Equals(txtOldSSN.Text) Then
                        MessageBox.Show("Old Social and New Social are same", "Same SSN", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return
                    End If
                End If

                Dim ParticipentDS As DataSet = RegMasterDAL.RetrieveSecureRegMasterByPartSSNO(CInt((Me.txtNewSSN.Text)))
                If (ParticipentDS.Tables("REG_MASTER").Rows.Count > 0) Then
                    _NewFamilyID = CInt(ParticipentDS.Tables("REG_MASTER").Rows(0)("FAMILY_ID"))
                    Me.txtNewFID.Text = CStr(_NewFamilyID)
                    btnNewSearch.Enabled = False
                    txtNewSSN.Enabled = False
                    txtNewSSN.ReadOnly = True
                    LoadmthdtlwithNewSocial()
                    LoadnewSocialHours()
                Else
                    MessageBox.Show("No Matching Results", "No Records", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

            End If
        Catch ex As Exception

            Throw
        Finally
        End Try
    End Sub

    Private Sub btnNewClear_Click(sender As Object, e As EventArgs) Handles btnNewClear.Click

        If NewSSNDataGrid.DataSource IsNot Nothing Then
            NewSSNDataGrid.DataSource = Nothing

            NEW_ELIG_HOURS.Clear()
            If NewHoursDataGrid.DataSource IsNot Nothing Then
                NewHoursDataGrid.DataSource = Nothing
            End If
            NEW_EMD_DS.Clear()
            If NewSSNDataGrid.DataSource IsNot Nothing Then
                NewSSNDataGrid.DataSource = Nothing
            End If

        End If
        txtNewSSN.Clear()
        txtNewSSN.Enabled = True
        txtNewSSN.ReadOnly = False
        btnNewSearch.Enabled = True
        txtNewFID.Clear()

        btncancelhrs.Enabled = False
        btnSaveHrs.Enabled = False
        btnModifyHrs.Enabled = False

        btnaddhrsrow.Enabled = False
        btnaddhrsallrow.Enabled = False
        btnremhrsrow.Enabled = False
        btnremhrsallrow.Enabled = False

        btncancelelig.Visible = False
        btnsaveelig.Visible = False
        btnmodifyelig.Visible = False

        btnaddeligrow.Enabled = False
        btnaddeligallrow.Enabled = False
        btnremeligrow.Enabled = False
        btnremeligallrow.Enabled = False

        NEWclearall()
    End Sub

    '' Modify hours
    Private Sub btnModifyHrs_Click(sender As Object, e As EventArgs) Handles btnModifyHrs.Click

        If OldHoursDataGrid Is Nothing Then Return

        btncancelhrs.Enabled = True
        btnSaveHrs.Enabled = True
        btnModifyHrs.Enabled = False

        btnaddhrsrow.Enabled = True
        btnaddhrsallrow.Enabled = True
        btnremhrsrow.Enabled = True
        btnremhrsallrow.Enabled = True

        ''btnaddeligrow.Enabled = True
        ''btnaddeligallrow.Enabled = True
        ''btnremeligrow.Enabled = True
        ''btnremeligallrow.Enabled = True

        grpremarks.Enabled = True
        txtRemarks.Enabled = True
        txtRemarks.Focus()
    End Sub

    ''Cancel Hours
    Private Sub btncancelhrs_Click(sender As Object, e As EventArgs) Handles btncancelhrs.Click
        Dim Result As DialogResult = DialogResult.None
        Try
            Result = MessageBox.Show(Me, "Do you want to Cancel the changes?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If Result = DialogResult.Yes Then

                OLD_ELIG_HOURS.ELIG_ACCT_HOURS.RejectChanges()
                NEW_ELIG_HOURS.ELIG_ACCT_HOURS.RejectChanges()

                OldHoursDataGrid.DataMember = ""
                OldHoursDataGrid.DataSource = Nothing

                NewHoursDataGrid.DataMember = ""
                NewHoursDataGrid.DataSource = Nothing

                LoadOldSocialHours()
                LoadnewSocialHours()
                txtRemarks.Clear()

                _ChangedELGHRSRows.ELIG_ACCT_HOURS.Rows.Clear()

            ElseIf Result = DialogResult.No Then
                btncancelhrs.Enabled = True
                btnSaveHrs.Enabled = True
            End If
        Catch ex As Exception
        Finally
        End Try

    End Sub
    ''save hrs
    Private Sub btnSaveHrs_Click(sender As Object, e As EventArgs) Handles btnSaveHrs.Click

        Dim Transaction As DbTransaction
        Dim Result As DialogResult = DialogResult.None

        Try
            btnSaveHrs.Enabled = False
            btncancelhrs.Enabled = False

            If ValidateRemarks() Then
                btnSaveHrs.Enabled = True
                btncancelhrs.Enabled = True
                Return
            End If

            ''_ChangedELGMTHRows = CType(NEW_EMD_DS.GetChanges(), UFCW.REGMASTER.EligMthdtlDS)

            If _ChangedELGHRSRows IsNot Nothing AndAlso _ChangedELGHRSRows.ELIG_ACCT_HOURS.Rows.Count = 0 Then     '' when new row added,deleted then cancel, save buttons are enabled
                MessageBox.Show("There are no changes.", "No Changes", MessageBoxButtons.OK, MessageBoxIcon.Information)
                btnSaveHrs.Enabled = False
                btncancelhrs.Enabled = False
                Return
            Else

                ''to get max eligibility period in added rows

                Dim MaxEligPeriod = _ChangedELGHRSRows.ELIG_ACCT_HOURS.AsEnumerable
                _MaxEligPeriod = (From r In MaxEligPeriod Select r.Field(Of Date)("ELIG_PERIOD")).Max
                _MinEligPeriod = (From r In MaxEligPeriod Select r.Field(Of Date)("ELIG_PERIOD")).Min

                Result = MessageBox.Show(Me, "You are ready to move eligibility from " & Environment.NewLine &
                                    CStr(_MinEligPeriod) & " thru " & CStr(_MaxEligPeriod), "Eligibility Periods", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If Result = DialogResult.Yes Then
                    ''continue to save
                ElseIf Result = DialogResult.No Then
                    btnSaveHrs.Enabled = True
                    btncancelhrs.Enabled = True
                    Return
                End If

                '' to get work periods to include in the email
                Dim WorkPeriod = _ChangedELGHRSRows.ELIG_ACCT_HOURS.AsEnumerable
                _MaxWorkPeriod = (From r In MaxEligPeriod Select r.Field(Of Date)("PSTPERIOD")).Max
                _MinWorkPeriod = (From r In MaxEligPeriod Select r.Field(Of Date)("PSTPERIOD")).Min

            End If


            If _ChangedELGHRSRows IsNot Nothing Then
                Transaction = RegMasterDAL.BeginTransaction

                Try

                    If SaveELGHRSchanges(Transaction) = True Then
                        RegMasterDAL.CommitTransaction(Transaction)
                        MessageBox.Show("Eligibity Hours and Eligibility moved Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        _ChangedELGHRSRows.ELIG_ACCT_HOURS.Rows.Clear()
                        NEW_ELIG_HOURS.ELIG_ACCT_HOURS.Rows.Clear()
                        _TotalNewEligHrsDS = Nothing
                        LoadnewSocialHours()

                        OLD_ELIG_HOURS.ELIG_ACCT_HOURS.Rows.Clear()
                        _TotalOldEligHrsDS = Nothing
                        LoadOldSocialHours()

                        NEW_EMD_DS.Tables("ELIG_MTHDTL").Rows.Clear()
                        _TotalNewEligMthdtlDS = Nothing
                        LoadmthdtlwithNewSocial()
                    Else
                        RegMasterDAL.RollbackTransaction(Transaction)
                        MessageBox.Show("Error while moving Eligibity." & vbCrLf & "Please try again ", "Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If

                Catch db2ex As DB2Exception

                    Try

                        If Transaction IsNot Nothing Then
                            RegMasterDAL.RollbackTransaction(Transaction)
                        End If

                    Catch ex2 As Exception
                    End Try

                    Select Case db2ex.Number
                        Case -438, -1822
                            MessageBox.Show("The item(s) you are attempting to update has been changed by another process." &
                                   vbCrLf & "Exit and re-enter the  data.", "Save rejected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Case Else
                            Throw
                    End Select

                Catch ex As Exception

                    Try

                        If Transaction IsNot Nothing Then
                            RegMasterDAL.RollbackTransaction(Transaction)
                        End If

                    Catch ex2 As Exception
                    End Try

                    Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
                    If (rethrow) Then
                        Throw
                    End If

                End Try
            End If

        Catch ex As Exception

            btnSaveHrs.Enabled = True
            btncancelhrs.Enabled = True


            Throw

        Finally
            If Transaction IsNot Nothing Then Transaction.Dispose()
            Transaction = Nothing
        End Try

    End Sub
    '' Hours add row
    Private Sub btnaddhrsrow_Click(sender As Object, e As EventArgs) Handles btnaddhrsrow.Click

        If OLD_ELIG_HOURS.ELIG_ACCT_HOURS.Rows.Count = 0 Then Return
        Dim Result As DialogResult = DialogResult.None
        Dim Cnt As Integer


        Try

            Result = MessageBox.Show(Me, "Are you ready to copy records from eligibility period " & Environment.NewLine &
                                      CStr(_SelectedFrom) & " thru " & CStr(_SelectedThru), "Eligibility Periods", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If Result = DialogResult.Yes Then
                For Cnt = 0 To OLD_ELIG_HOURS.ELIG_ACCT_HOURS.Rows.Count - 1
                    If OldHoursDataGrid.IsSelected(Cnt) = True Then
                        NEW_ELIG_HOURS.ELIG_ACCT_HOURS.ImportRow(OLD_ELIG_HOURS.ELIG_ACCT_HOURS.Rows(Cnt))
                        _ChangedELGHRSRows.ELIG_ACCT_HOURS.ImportRow(OLD_ELIG_HOURS.ELIG_ACCT_HOURS.Rows(Cnt))
                    End If
                    OldHoursDataGrid.UnSelect(Cnt)
                    NewHoursDataGrid.MoveGridToRow(OLD_ELIG_HOURS.ELIG_ACCT_HOURS.Rows.Count + Cnt)
                Next
                btnSaveHrs.Enabled = True
                btncancelhrs.Enabled = True
                ''  NEW_ELIG_HOURS.ELIG_ACCT_HOURS.AcceptChanges()
                _SelectedFrom = Nothing
                _SelectedThru = Nothing

            ElseIf Result = DialogResult.No Then

            End If


        Catch ex As Exception

        End Try

    End Sub
    ''Hours add all rows
    Private Sub btnaddhrsallrow_Click(sender As Object, e As EventArgs) Handles btnaddhrsallrow.Click
        If OLD_ELIG_HOURS.ELIG_ACCT_HOURS.Rows.Count = 0 Then Return
        Dim Result As DialogResult = DialogResult.None
        Try

            Result = MessageBox.Show(Me, "Are you ready to copy all records ", "Total records", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If Result = DialogResult.Yes Then
                For Each DR As DataRow In OLD_ELIG_HOURS.ELIG_ACCT_HOURS.Rows

                    NEW_ELIG_HOURS.ELIG_ACCT_HOURS.ImportRow(DR)
                    _ChangedELGHRSRows.ELIG_ACCT_HOURS.ImportRow(DR)
                Next
                btnSaveHrs.Enabled = True
                btncancelhrs.Enabled = True
                ''  NEW_ELIG_HOURS.ELIG_ACCT_HOURS.AcceptChanges()
                _SelectedFrom = Nothing
                _SelectedThru = Nothing

            ElseIf Result = DialogResult.No Then

            End If


        Catch ex As Exception

        End Try
    End Sub

    ''Hours remove selected row
    Private Sub btnremhrsrow_Click(sender As Object, e As EventArgs) Handles btnremhrsrow.Click

        If _ChangedELGHRSRows.ELIG_ACCT_HOURS.Rows.Count = 0 Then Return
        Dim DV As DataView
        Dim Result As DialogResult = DialogResult.None

        Try

            Result = MessageBox.Show(Me, "Are you ready to remove records from eligibility period " & Environment.NewLine &
                                              CStr(_SelectedRemoveFrom) & " thru " & CStr(_SelectedRemoveThru), "Eligibility Periods", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If Result = DialogResult.Yes Then

                ' ''For cnt = 0 To NEW_ELIG_HOURS.ELIG_ACCT_HOURS.Rows.Count - 1
                ' ''    If NewhrsDataGrid.IsSelected(cnt) = True Then
                ' ''        '' Dim dtnew As DataTable = NewhrsDataGrid.GetCurrentDataTable
                ' ''        ''dr = dtnew.Rows(cnt)

                ' ''        dr = NewhrsDataGrid.GetTableRowFromGridPosition(cnt)
                ' ''        NEW_ELIG_HOURS.ELIG_ACCT_HOURS.Rows.Remove(dr)
                ' ''        ''    _ChangedELGHRSRows.ELIG_ACCT_HOURS.Rows.Remove(dr)
                ' ''        cnt = cnt - 1
                ' ''    End If
                ' ''Next


                Dim ar As ArrayList = NewHoursDataGrid.GetSelectedDataRows()
                '' from new grid
                For i As Integer = 0 To ar.Count - 1
                    Dim dr1 As DataRow = CType(ar(i), DataRowView).Row
                    Dim str As String = "FAMILY_ID = " & CStr(dr1("FAMILY_ID")) & " AND ACCTNO = " & CStr(dr1("ACCTNO")) & " AND PSTPERIOD = '" & CStr(dr1("PSTPERIOD")) & "'"


                    '' to remove from changed elig rows
                    DV = New DataView(_ChangedELGHRSRows.ELIG_ACCT_HOURS, str, "", DataViewRowState.CurrentRows)
                    If DV.Count > 0 Then
                        Dim drnew As DataRow = DV(0).Row
                        _ChangedELGHRSRows.ELIG_ACCT_HOURS.Rows.Remove(drnew)

                        ''we can able to delete rows in NEW_ELIG_HOURS  when only  we have a row in _ChangedELGHRSRows

                        DV = New DataView(NEW_ELIG_HOURS.ELIG_ACCT_HOURS, str, "", DataViewRowState.CurrentRows)
                        If DV.Count > 0 Then
                            Dim drnew1 As DataRow = DV(0).Row
                            NEW_ELIG_HOURS.ELIG_ACCT_HOURS.Rows.Remove(drnew1)
                        End If
                        _SelectedRemoveFrom = Nothing
                        _SelectedRemoveThru = Nothing

                    Else
                        MessageBox.Show("You are unable to delete the row having " & Environment.NewLine & "Familyid= " & CStr(dr1("FAMILY_ID")) & "  Acctno= " & CStr(dr1("ACCTNO")) & " and WorkPeriod= " & CStr(dr1("PSTPERIOD")), "Unable to delete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If

                Next
                btnSaveHrs.Enabled = True
                btncancelhrs.Enabled = True
            ElseIf Result = DialogResult.No Then
            End If

        Catch ex As Exception

            Throw
        End Try

    End Sub
    ''Hours remove all added rows
    Private Sub btnremhrsallrow_Click(sender As Object, e As EventArgs) Handles btnremhrsallrow.Click
        ''If NEW_ELIG_HOURS.ELIG_ACCT_HOURS.Rows.Count < 1 Then Exit Sub

        ''Dim cnt As Integer
        ''For cnt = 0 To NEW_ELIG_HOURS.ELIG_ACCT_HOURS.Rows.Count - 1
        ''    If NewhrsDataGrid.IsSelected(cnt) = True Then
        ''        NEW_ELIG_HOURS.ELIG_ACCT_HOURS.Rows.RemoveAt(cnt)
        ''    End If
        ''Next

        If _ChangedELGHRSRows.ELIG_ACCT_HOURS.Rows.Count = 0 Then Return
        Dim DV As DataView
        Dim Result As DialogResult = DialogResult.None
        Try

            Result = MessageBox.Show(Me, "Are you ready to remove all the records", "All Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If Result = DialogResult.Yes Then

                Dim NewDT As DataTable = OldHoursDataGrid.GetCurrentDataTable

                For Each DR As DataRow In NewDT.Rows
                    Dim str As String = "FAMILY_ID = " & CStr(DR("FAMILY_ID")) & " AND ACCTNO = " & CStr(DR("ACCTNO")) & " AND PSTPERIOD = '" & CStr(DR("PSTPERIOD")) & "'"

                    '' to remove from changed elig rows
                    DV = New DataView(_ChangedELGHRSRows.ELIG_ACCT_HOURS, str, "", DataViewRowState.CurrentRows)
                    If DV.Count > 0 Then
                        Dim NewDR As DataRow = DV(0).Row
                        _ChangedELGHRSRows.ELIG_ACCT_HOURS.Rows.Remove(NewDR)

                        ''we can able to delete rows in NEW_ELIG_HOURS  when only  we have a row in _ChangedELGHRSRows

                        DV = New DataView(NEW_ELIG_HOURS.ELIG_ACCT_HOURS, str, "", DataViewRowState.CurrentRows)
                        If DV.Count > 0 Then
                            Dim drnew1 As DataRow = DV(0).Row
                            NEW_ELIG_HOURS.ELIG_ACCT_HOURS.Rows.Remove(drnew1)
                        End If
                        _SelectedRemoveFrom = Nothing
                        _SelectedRemoveThru = Nothing
                    End If
                Next

                btnSaveHrs.Enabled = True
                btncancelhrs.Enabled = True
            ElseIf Result = DialogResult.No Then
            End If

        Catch ex As Exception

            Throw
        End Try


    End Sub


#Region "Eligibility"

    Private Sub btnmodifyelig_Click(sender As Object, e As EventArgs) Handles btnmodifyelig.Click
        If OldSSNDataGrid Is Nothing Then Return
        btncancelelig.Enabled = True
        btnsaveelig.Enabled = True
        btnmodifyelig.Enabled = False

        btnaddeligrow.Enabled = True
        btnaddeligallrow.Enabled = True
        btnremeligrow.Enabled = True
        btnremeligallrow.Enabled = True

        txtRemarks.Enabled = True

    End Sub
    '' cancel  Eligibility
    Private Sub btncancelelig_Click(sender As Object, e As EventArgs) Handles btncancelelig.Click
        Dim Result As DialogResult = DialogResult.None
        Try
            Result = MessageBox.Show(Me, "Do you want to Cancel the changes?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If Result = DialogResult.Yes Then

                OLD_EMD_DS.ELIG_MTHDTL.RejectChanges()
                NEW_EMD_DS.ELIG_MTHDTL.RejectChanges()


                OldSSNDataGrid.DataMember = ""
                OldSSNDataGrid.DataSource = Nothing

                NewSSNDataGrid.DataMember = ""
                NewSSNDataGrid.DataSource = Nothing

                LoadmthdtlwithOldSocial()
                LoadmthdtlwithNewSocial()
            ElseIf Result = DialogResult.No Then
                btncancelhrs.Enabled = True
                btnSaveHrs.Enabled = True
            End If
        Catch ex As Exception
        Finally
        End Try

    End Sub
    ''save Eligibility
    ''Private Sub btnsaveelig_Click(sender As Object, e As EventArgs) Handles btnsaveelig.Click
    ''    Dim Transaction As DbTransaction = Nothing
    ''    Try
    ''        btnsaveelig.Enabled = False
    ''        btncancelelig.Enabled = False

    ''        If ValidateRemarks() Then
    ''            btnsaveelig.Enabled = True
    ''            btncancelelig.Enabled = True
    ''            Return
    ''        End If

    ''        ''_ChangedELGMTHRows = CType(NEW_EMD_DS.GetChanges(), UFCW.REGMASTER.EligMthdtlDS)

    ''        If (_ChangedELGMTHRows) IsNot Nothing AndAlso _ChangedELGMTHRows.ELIG_MTHDTL.Rows.Count = 0 Then     '' when new row added,deleted then cancel, save buttons are enabled
    ''            MessageBox.Show("There are no changes.", "No Changes", MessageBoxButtons.OK, MessageBoxIcon.Information)
    ''            btnsaveelig.Enabled = False
    ''            btncancelelig.Enabled = False
    ''            Return
    ''        End If


    ''        If (_ChangedELGMTHRows) IsNot Nothing Then
    ''            Transaction = RegMasterDAL.BeginTransaction

    ''            Try

    ''                If SaveELGMTHchanges(Transaction) = True Then
    ''                    RegMasterDAL.CommitTransaction(Transaction)
    ''                    MessageBox.Show("Eligibity moved Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)

    ''                Else
    ''                    RegMasterDAL.RollbackTransaction(Transaction)
    ''                    MessageBox.Show("Error while saving Eligibity." & vbCrLf & "Please try again ", "Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error)
    ''                End If


    ''            Catch db2ex As DB2Exception

    ''                Select Case db2ex.ErrorCode
    ''                    Case -438
    ''                        MessageBox.Show("The item(s) you are attempting to update has been changed by another process." & _
    ''                               vbCrLf & "Exit and re-enter the  data.", "Save rejected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    ''                End Select

    ''                RegMasterDAL.RollbackTransaction(Transaction)

    ''            Catch ex As Exception

    ''                Try

    ''                    RegMasterDAL.RollbackTransaction(Transaction)

    ''                Catch ex2 As Exception

    ''                End Try

    ''                Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    ''                If (rethrow) Then
    ''                    Throw
    ''                End If

    ''            End Try
    ''        End If

    ''    Catch ex As Exception

    ''        btnsaveelig.Enabled = True
    ''        btncancelelig.Enabled = True

    ''        Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    ''        If (rethrow) Then
    ''            Throw
    ''        End If

    ''    Finally
    ''        If Not Transaction Is Nothing Then Transaction.Dispose()
    ''        Transaction = Nothing
    ''    End Try

    ''End Sub

    ''Eligibility add row
    Private Sub btnaddeligrow_Click(sender As Object, e As EventArgs) Handles btnaddeligrow.Click
        If OLD_EMD_DS.ELIG_MTHDTL.Rows.Count < 1 Then Exit Sub
        Dim Result As DialogResult = DialogResult.None
        Dim Cnt As Integer
        Dim DelRows(0) As Integer

        ''  Me.Cursor = Cursors.WaitCursor


        Result = MessageBox.Show(Me, "Are you ready to move records for eligibility period from " & CStr(_SelectedFrom) & " thru " & CStr(_SelectedThru), "Eligibility Period", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)

        For Cnt = 0 To OLD_EMD_DS.ELIG_MTHDTL.Rows.Count - 1
            If OldSSNDataGrid.IsSelected(Cnt) = True Then
                NEW_EMD_DS.ELIG_MTHDTL.ImportRow(OLD_EMD_DS.ELIG_MTHDTL.Rows(Cnt))
                _ChangedELGMTHRows.ELIG_MTHDTL.ImportRow(OLD_EMD_DS.ELIG_MTHDTL.Rows(Cnt))
                '' OLD_EMD_DS.ELIG_MTHDTL.Rows.RemoveAt(cnt)
            End If
        Next
        ''   Me.Cursor = Cursors.Default

    End Sub
    Private Sub btnaddeligallrow_Click(sender As Object, e As EventArgs) Handles btnaddeligallrow.Click
        If OLD_EMD_DS.ELIG_MTHDTL.Rows.Count < 1 Then Exit Sub

        Dim Cnt As Integer

        For Cnt = 0 To OLD_EMD_DS.ELIG_MTHDTL.Rows.Count - 1
            If OldSSNDataGrid.IsSelected(Cnt) = True Then
                NEW_EMD_DS.ELIG_MTHDTL.ImportRow(OLD_EMD_DS.ELIG_MTHDTL.Rows(Cnt))
                _ChangedELGMTHRows.ELIG_MTHDTL.ImportRow(OLD_EMD_DS.ELIG_MTHDTL.Rows(Cnt))
                OLD_EMD_DS.ELIG_MTHDTL.Rows.RemoveAt(Cnt)
            End If
        Next
    End Sub

    Private Sub btnremeligrow_Click(sender As Object, e As EventArgs) Handles btnremeligrow.Click
        If NEW_EMD_DS.ELIG_MTHDTL.Rows.Count < 1 Then Exit Sub

        Dim Cnt As Integer
        For Cnt = 0 To NEW_ELIG_HOURS.ELIG_ACCT_HOURS.Rows.Count - 1
            If NewSSNDataGrid.IsSelected(Cnt) = True Then
                If NEW_EMD_DS.ELIG_MTHDTL.Rows(Cnt).RowState = DataRowState.Added Then
                    NEW_EMD_DS.ELIG_MTHDTL.Rows.RemoveAt(Cnt)
                End If
            End If
        Next
    End Sub

    Private Sub btnremeligallrow_Click(sender As Object, e As EventArgs) Handles btnremeligallrow.Click
        If NEW_EMD_DS.ELIG_MTHDTL.Rows.Count < 1 Then Exit Sub
        Dim Cnt As Integer
        For Cnt = 0 To NEW_ELIG_HOURS.ELIG_ACCT_HOURS.Rows.Count - 1
            If NewSSNDataGrid.IsSelected(Cnt) = True Then
                If NEW_EMD_DS.ELIG_MTHDTL.Rows(Cnt).RowState = DataRowState.Added Then
                    NEW_EMD_DS.ELIG_MTHDTL.Rows.RemoveAt(Cnt)
                End If
            End If
        Next
    End Sub

#End Region

    Private Sub OldhrsDataGrid_MouseDown(sender As Object, e As MouseEventArgs) Handles OldHoursDataGrid.MouseDown
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo
        Dim MyGrid As DataGridCustom

        Try
            MyGrid = CType(sender, DataGridCustom)

            HTI = MyGrid.HitTest(e.X, e.Y)
            Select Case HTI.Type
                Case System.Windows.Forms.DataGrid.HitTestType.RowHeader, DataGrid.HitTestType.Cell
                    If ((Control.ModifierKeys And Keys.Shift) = Keys.Shift) Or
                        ((Control.ModifierKeys And Keys.ShiftKey) = Keys.ShiftKey) Or
                        ((Control.ModifierKeys And Keys.Control) = Keys.Control) Or
                        ((Control.ModifierKeys And Keys.ControlKey) = Keys.ControlKey) Then
                    ElseIf e.Button = MouseButtons.Right And Not MyGrid.IsSelected(HTI.Row) Then
                        MyGrid.ResetSelection()
                    ElseIf e.Button = MouseButtons.Right Then
                    ElseIf e.Button = MouseButtons.Left Then
                    Else
                        MyGrid.ResetSelection()
                    End If
                    MyGrid.Select(HTI.Row)
            End Select

            If HTI.Row > -1 Then

                Dim CM As CurrencyManager
                Dim DGDRV As DataRowView
                Dim DGRow As DataRow

                CM = DirectCast(MyGrid.BindingContext(MyGrid.DataSource, MyGrid.DataMember), CurrencyManager)
                If CM IsNot Nothing AndAlso CM.Position > -1 Then
                    DGDRV = DirectCast(CM.Current, DataRowView)
                    DGRow = DGDRV.Row

                    If DGRow IsNot Nothing AndAlso Not IsDBNull(DGRow("ELIG_PERIOD")) Then

                        _SelectedFrom = CDate(DGRow("ELIG_PERIOD"))

                    End If

                End If

            End If

        Catch ex As Exception


            Throw

        End Try
    End Sub

    Private Sub OldhrsDataGrid_MouseUp(sender As Object, e As MouseEventArgs) Handles OldHoursDataGrid.MouseUp
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo
        Dim MyGrid As DataGridCustom

        Try
            MyGrid = CType(sender, DataGridCustom)
            HTI = MyGrid.HitTest(e.X, e.Y)

            Select Case HTI.Type
                Case System.Windows.Forms.DataGrid.HitTestType.RowHeader, DataGrid.HitTestType.Cell
                    If ((Control.ModifierKeys And Keys.Shift) = Keys.Shift) Or
                        ((Control.ModifierKeys And Keys.ShiftKey) = Keys.ShiftKey) Or
                        ((Control.ModifierKeys And Keys.Control) = Keys.Control) Or
                        ((Control.ModifierKeys And Keys.ControlKey) = Keys.ControlKey) Then
                    ElseIf e.Button = MouseButtons.Right And Not MyGrid.IsSelected(HTI.Row) Then
                        MyGrid.ResetSelection()
                    ElseIf e.Button = MouseButtons.Right Then
                    ElseIf e.Button = MouseButtons.Left Then
                    Else
                        MyGrid.ResetSelection()
                    End If
                    MyGrid.Select(HTI.Row)
            End Select

            If HTI.Row > -1 Then

                Dim CM As CurrencyManager
                Dim DGDRV As DataRowView
                Dim DGRow As DataRow

                CM = DirectCast(MyGrid.BindingContext(MyGrid.DataSource, MyGrid.DataMember), CurrencyManager)
                If CM IsNot Nothing AndAlso CM.Position > -1 Then
                    DGDRV = DirectCast(CM.Current, DataRowView)
                    DGRow = DGDRV.Row

                    If DGRow IsNot Nothing AndAlso Not IsDBNull(DGRow("ELIG_PERIOD")) Then

                        _SelectedThru = CDate(DGRow("ELIG_PERIOD"))

                    End If

                    If _SelectedThru < _SelectedFrom Then
                        Dim TempDate As Date = _SelectedFrom '' greater date
                        _SelectedThru = TempDate
                        _SelectedFrom = CDate(DGRow("ELIG_PERIOD"))
                    End If

                End If

            End If

        Catch ex As Exception


            Throw

        End Try
    End Sub

    Private Sub NewhrsDataGrid_MouseDown(sender As Object, e As MouseEventArgs) Handles NewHoursDataGrid.MouseDown
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo
        Dim MyGrid As DataGridCustom

        Try
            MyGrid = CType(sender, DataGridCustom)
            HTI = MyGrid.HitTest(e.X, e.Y)
            Select Case HTI.Type
                Case System.Windows.Forms.DataGrid.HitTestType.RowHeader, DataGrid.HitTestType.Cell
                    If ((Control.ModifierKeys And Keys.Shift) = Keys.Shift) Or
                        ((Control.ModifierKeys And Keys.ShiftKey) = Keys.ShiftKey) Or
                        ((Control.ModifierKeys And Keys.Control) = Keys.Control) Or
                        ((Control.ModifierKeys And Keys.ControlKey) = Keys.ControlKey) Then
                    ElseIf e.Button = MouseButtons.Right And Not MyGrid.IsSelected(HTI.Row) Then
                        MyGrid.ResetSelection()
                    ElseIf e.Button = MouseButtons.Right Then
                    ElseIf e.Button = MouseButtons.Left Then
                    Else
                        MyGrid.ResetSelection()
                    End If
                    MyGrid.Select(HTI.Row)
            End Select

            If HTI.Row > -1 Then

                Dim CM As CurrencyManager
                Dim DGDRV As DataRowView
                Dim DGRow As DataRow

                CM = DirectCast(MyGrid.BindingContext(MyGrid.DataSource, MyGrid.DataMember), CurrencyManager)
                If CM IsNot Nothing AndAlso CM.Position > -1 Then
                    DGDRV = DirectCast(CM.Current, DataRowView)
                    DGRow = DGDRV.Row

                    If DGRow IsNot Nothing AndAlso Not IsDBNull(DGRow("ELIG_PERIOD")) Then

                        _SelectedRemoveFrom = CDate(DGRow("ELIG_PERIOD"))

                    End If

                End If

            End If

        Catch ex As Exception


            Throw

        End Try
    End Sub

    Private Sub NewhrsDataGrid_MouseUp(sender As Object, e As MouseEventArgs) Handles NewHoursDataGrid.MouseUp
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo
        Dim MyGrid As DataGridCustom

        Try
            MyGrid = CType(sender, DataGridCustom)
            HTI = MyGrid.HitTest(e.X, e.Y)
            Select Case HTI.Type
                Case System.Windows.Forms.DataGrid.HitTestType.RowHeader, DataGrid.HitTestType.Cell
                    If ((Control.ModifierKeys And Keys.Shift) = Keys.Shift) Or
                        ((Control.ModifierKeys And Keys.ShiftKey) = Keys.ShiftKey) Or
                        ((Control.ModifierKeys And Keys.Control) = Keys.Control) Or
                        ((Control.ModifierKeys And Keys.ControlKey) = Keys.ControlKey) Then
                    ElseIf e.Button = MouseButtons.Right And Not MyGrid.IsSelected(HTI.Row) Then
                        MyGrid.ResetSelection()
                    ElseIf e.Button = MouseButtons.Right Then
                    ElseIf e.Button = MouseButtons.Left Then
                    Else
                        MyGrid.ResetSelection()
                    End If
                    MyGrid.Select(HTI.Row)
            End Select

            If HTI.Row > -1 Then

                Dim CM As CurrencyManager
                Dim DGDRV As DataRowView
                Dim DGRow As DataRow

                CM = DirectCast(MyGrid.BindingContext(MyGrid.DataSource, MyGrid.DataMember), CurrencyManager)
                If CM IsNot Nothing AndAlso CM.Position > -1 Then
                    DGDRV = DirectCast(CM.Current, DataRowView)
                    DGRow = DGDRV.Row

                    If DGRow IsNot Nothing AndAlso Not IsDBNull(DGRow("ELIG_PERIOD")) Then

                        _SelectedRemoveThru = CDate(DGRow("ELIG_PERIOD"))

                    End If

                    If _SelectedRemoveThru < _SelectedRemoveFrom Then
                        Dim TempDate As Date = _SelectedRemoveFrom '' greater date
                        _SelectedRemoveThru = TempDate
                        _SelectedRemoveFrom = CDate(DGRow("ELIG_PERIOD"))
                    End If
                End If

            End If

        Catch ex As Exception


            Throw

        End Try
    End Sub

    Private Sub NewhrsDataGrid_RefreshGridData() Handles NewHoursDataGrid.RefreshGridData
        LoadnewSocialHours()
    End Sub

    Private Sub OldhrsDataGrid_RefreshGridData() Handles OldHoursDataGrid.RefreshGridData
        LoadOldSocialHours()
    End Sub

    Private Sub NewSSNDataGrid_RefreshGridData() Handles NewSSNDataGrid.RefreshGridData
        LoadmthdtlwithNewSocial()
    End Sub

    Private Sub OldSSNDataGrid_RefreshGridData() Handles OldSSNDataGrid.RefreshGridData
        LoadmthdtlwithOldSocial()
    End Sub

#End Region

#Region "Custom Subs\Functions"

    Public Sub LoadOldSocialHours()

        Try

            ''  OLD_ELIG_HOURS.EnforceConstraints = False

            If _TotalOldEligHrsDS IsNot Nothing Then
                If Me._TotalOldEligHrsDS.Tables("ELIG_ACCT_HOURS").Rows.Count > 0 Then
                    OLD_ELIG_HOURS.Tables("ELIG_ACCT_HOURS").Rows.Clear()
                    OLD_ELIG_HOURS = CType(_TotalOldEligHrsDS.Copy, EligAcctHoursDS)
                End If
            End If

            If OLD_ELIG_HOURS.Tables("ELIG_ACCT_HOURS").Rows.Count = 0 Then  '' only retrieve data for first time
                OLD_ELIG_HOURS = CType(RegMasterDAL.RetrieveEligHoursforSSNchange(_OldFamilyID, OLD_ELIG_HOURS), EligAcctHoursDS)
                Me._TotalOldEligHrsDS = CType(OLD_ELIG_HOURS.Copy, EligAcctHoursDS)
            End If

            OldHoursDataGrid.DataSource = OLD_ELIG_HOURS.ELIG_ACCT_HOURS
            OldHoursDataGrid.SetTableStyle()

            btncancelhrs.Enabled = False
            btnSaveHrs.Enabled = False
            btnModifyHrs.Enabled = True

            btnaddhrsrow.Enabled = False
            btnaddhrsallrow.Enabled = False
            btnremhrsrow.Enabled = False
            btnremhrsallrow.Enabled = False

            txtRemarks.Enabled = False
            grpremarks.Enabled = False

        Catch ex As Exception

            Throw
        End Try
    End Sub
    Public Sub LoadnewSocialHours()

        Try

            ''   NEW_ELIG_HOURS.EnforceConstraints = False

            If _TotalNewEligHrsDS IsNot Nothing Then
                If _TotalNewEligHrsDS.Tables("ELIG_ACCT_HOURS").Rows.Count > 0 Then
                    NEW_ELIG_HOURS.Tables("ELIG_ACCT_HOURS").Rows.Clear()
                    NEW_ELIG_HOURS = CType(_TotalNewEligHrsDS.Copy, EligAcctHoursDS)
                End If
            End If

            If NEW_ELIG_HOURS.Tables("ELIG_ACCT_HOURS").Rows.Count = 0 Then  '' only retrieve data for first time
                NEW_ELIG_HOURS = CType(RegMasterDAL.RetrieveEligHoursforSSNchange(_NewFamilyID, NEW_ELIG_HOURS), EligAcctHoursDS)
                _TotalNewEligHrsDS = CType(NEW_ELIG_HOURS.Copy, EligAcctHoursDS)
            End If

            NewHoursDataGrid.DataSource = NEW_ELIG_HOURS.ELIG_ACCT_HOURS
            NewHoursDataGrid.SetTableStyle()
            NewHoursDataGrid.Sort = If(NewHoursDataGrid.LastSortedBy, NewHoursDataGrid.DefaultSort)

            btncancelhrs.Enabled = False
            btnSaveHrs.Enabled = False
            btnModifyHrs.Enabled = True

            btnaddhrsrow.Enabled = False
            btnaddhrsallrow.Enabled = False
            btnremhrsrow.Enabled = False
            btnremhrsallrow.Enabled = False

            grpremarks.Enabled = False
            txtRemarks.Enabled = False

        Catch ex As Exception

            Throw
        End Try
    End Sub

    Public Sub LoadmthdtlwithOldSocial()

        Try

            '' OLD_EMD_DS.EnforceConstraints = False

            If _TotalOldEligMthdtlDS IsNot Nothing Then
                If _TotalOldEligMthdtlDS.Tables("ELIG_MTHDTL").Rows.Count > 0 Then
                    OLD_EMD_DS.Tables("ELIG_MTHDTL").Rows.Clear()
                    OLD_EMD_DS = CType(_TotalOldEligMthdtlDS.Copy, EligMthdtlDS)
                End If
            End If

            If OLD_EMD_DS.Tables("ELIG_MTHDTL").Rows.Count = 0 Then  '' only retrieve data for first time
                OLD_EMD_DS = CType(RegMasterDAL.RetrieveEligMTHDTLByFamilyID(_OldFamilyID, OLD_EMD_DS), EligMthdtlDS)
                _TotalOldEligMthdtlDS = CType(OLD_EMD_DS.Copy, EligMthdtlDS)
            End If

            OldSSNDataGrid.DataSource = OLD_EMD_DS.ELIG_MTHDTL
            ''  OldSSNDataGrid.SetTableStyle()
            SetTableStyle(OldSSNDataGrid, Nothing, OLD_EMD_DS.ELIG_MTHDTL)

            ''btncancelelig.Enabled = False
            ''btnsaveelig.Enabled = False
            ''btnmodifyelig.Enabled = True

            btnaddeligrow.Enabled = False
            btnaddeligallrow.Enabled = False
            btnremeligrow.Enabled = False
            btnremeligallrow.Enabled = False

        Catch ex As Exception

            Throw
        End Try
    End Sub
    Public Sub LoadmthdtlwithNewSocial()

        Try
            ''  NEW_EMD_DS.EnforceConstraints = False

            If _TotalNewEligMthdtlDS IsNot Nothing Then
                If _TotalNewEligMthdtlDS.Tables("ELIG_MTHDTL").Rows.Count > 0 Then
                    NEW_EMD_DS.Tables("ELIG_MTHDTL").Rows.Clear()
                    NEW_EMD_DS = CType(_TotalNewEligMthdtlDS.Copy, EligMthdtlDS)
                End If
            End If

            If NEW_EMD_DS.Tables("ELIG_MTHDTL").Rows.Count = 0 Then  '' only retrieve data for first time
                NEW_EMD_DS = CType(RegMasterDAL.RetrieveEligMTHDTLByFamilyID(_NewFamilyID, NEW_EMD_DS), EligMthdtlDS)
                _TotalNewEligMthdtlDS = CType(NEW_EMD_DS.Copy, EligMthdtlDS)
            End If

            NewSSNDataGrid.DataSource = NEW_EMD_DS.ELIG_MTHDTL
            SetTableStyle(NewSSNDataGrid, Nothing, NEW_EMD_DS.ELIG_MTHDTL)
            '' OldSSNDataGrid.SetTableStyle()

            ''btncancelelig.Enabled = False
            ''btnsaveelig.Enabled = False
            ''btnmodifyelig.Enabled = True

            btnaddeligrow.Enabled = False
            btnaddeligallrow.Enabled = False
            btnremeligrow.Enabled = False
            btnremeligallrow.Enabled = False



        Catch ex As Exception

            Throw
        End Try
    End Sub

    Private Sub SetTableStyle(ByVal dg As DataGridCustom, ByVal dataGridCustomContextMenu As System.Windows.Forms.ContextMenuStrip, Optional ByVal dt As DataTable = Nothing)

        If dt Is Nothing Then Return

        Dim DefaultDGTS As DataGridTableStyle
        Dim DGTS As New DataGridTableStyle
        Dim TextCol As DataGridFormattableTextBoxColumn
        Dim BoolCol As DataGridColorBoolColumn
        Dim intCol As Integer
        Dim CurMan As CurrencyManager
        Dim ColsDV As DataView
        Dim DSDefaultStyle As DataSet

        Try

            Try

                Dim XMLStyleName As String = CType(CType(ConfigurationManager.GetSection("MTHDTLDataGrid"), IDictionary)("StyleLocation"), String)
                DSDefaultStyle = RegMasterDAL.CreateDataSetFromXML(XMLStyleName)

            Catch ex As System.NullReferenceException
                MessageBox.Show("Please check < MTHDTLDataGrid.xml > entry in Config files ", "Missing Entry", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            Catch ex As System.IO.FileNotFoundException
                MessageBox.Show("Please check < MTHDTLDataGrid.xml > entry in Config files ", "Missing Entry", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            Catch ex As Exception

                Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
                If (rethrow) Then
                    Throw
                End If
            End Try

            CurMan = CType(Me.BindingContext(dt), CurrencyManager)

            DGTS = New DataGridTableStyle(CurMan)
            If dt Is Nothing Then
                DGTS.MappingName = dg.GetCurrentDataTable.TableName
            Else
                DGTS.MappingName = dt.TableName
            End If

            DGTS.GridColumnStyles.Clear()
            DGTS.GridLineStyle = DataGridLineStyle.None

            Dim ResultDT As DataTable = dg.LoadColumnsSizeAndPosition(dg.Name & "\" & DGTS.MappingName & "\ColumnSettings", DSDefaultStyle.Tables(1))
            Dim ColumnSequenceDV As New DataView(ResultDT)
            ColsDV = ColumnSequenceDV
            _DisplayColumnNamesDV = ColsDV

            DefaultDGTS = New DataGridTableStyle() 'This can be used to establish the columns displayed by default
            DefaultDGTS.MappingName = "Default"

            For intCol = 0 To ColsDV.Count - 1

                If (IsDBNull(ColsDV(intCol).Item("Visible")) OrElse ColsDV(intCol).Item("Visible").ToString.Trim.Length = 0 OrElse CBool(ColsDV(intCol).Item("Visible")) = True) Then
                    TextCol = New DataGridFormattableTextBoxColumn
                    TextCol.MappingName = CStr(ColsDV(intCol).Item("Mapping"))
                    TextCol.HeaderText = CStr(ColsDV(intCol).Item("HeaderText"))
                    If IsDBNull(ColsDV(intCol).Item("Format")) = False AndAlso CStr(ColsDV(intCol).Item("Format")) <> "" Then
                        TextCol.Format = CStr(ColsDV(intCol).Item("Format"))
                    End If
                    DefaultDGTS.GridColumnStyles.Add(TextCol)
                End If

                If ((IsDBNull(ColsDV(intCol).Item("Visible")) OrElse ColsDV(intCol).Item("Visible").ToString.Trim.Length = 0 OrElse CBool(ColsDV(intCol).Item("Visible")) = True) AndAlso (IsNothing(GetAllSettings(Me.AppKey, DGTS.MappingName & "\Customize\ColumnSelector")) OrElse CDbl(GetSetting(Me.AppKey, DGTS.MappingName & "\Customize\ColumnSelector", "Col " & CStr(ColsDV(intCol).Item("Mapping")) & " Customize", CStr(0))) = 1)) Then

                    If CStr(ColsDV(intCol).Item("Type")) = "Text" Then
                        TextCol = New DataGridFormattableTextBoxColumn ''DataGridHighlightTextBoxColumn
                        TextCol.MappingName = CStr(ColsDV(intCol).Item("Mapping"))
                        TextCol.HeaderText = CStr(ColsDV(intCol).Item("HeaderText"))
                        TextCol.Width = CInt(GetSetting(AppKey, dg.Name & "\" & DGTS.MappingName.ToString & "\ColumnSettings", "Col " & CStr(ColsDV(intCol).Item("Mapping")), CStr(UFCWGeneral.MeasureWidthinPixels(CInt(ColsDV(intCol).Item("DefaultCharWidth")), dg.Font.Name, dg.Font.Size))))
                        TextCol.NullText = CStr(ColsDV(intCol).Item("NullText"))
                        TextCol.TextBox.WordWrap = True

                        Try

                            If CBool(ColsDV(intCol).Item("ReadOnly")) Then
                                TextCol.ReadOnly = True
                            End If

                        Catch ex As Exception
                        End Try

                        If IsDBNull(ColsDV(intCol).Item("Format")) = False AndAlso CStr(ColsDV(intCol).Item("Format")) <> "" Then
                            TextCol.Format = CStr(ColsDV(intCol).Item("Format"))
                        End If

                        DGTS.GridColumnStyles.Add(TextCol)

                    ElseIf CStr(ColsDV(intCol).Item("Type")) = "Bool" Then
                        BoolCol = New DataGridColorBoolColumn(intCol) ''DataGridHighlightBoolColumn
                        BoolCol.MappingName = CStr(ColsDV(intCol).Item("Mapping"))
                        BoolCol.HeaderText = CStr(ColsDV(intCol).Item("HeaderText"))
                        BoolCol.Width = CInt(GetSetting(AppKey, dg.Name & "\" & DGTS.MappingName.ToString & "\ColumnSettings", "Col " & CStr(ColsDV(intCol).Item("Mapping")), CStr(UFCWGeneral.MeasureWidthinPixels(CInt(ColsDV(intCol).Item("DefaultCharWidth")), dg.Font.Name, dg.Font.Size))))
                        BoolCol.NullText = CStr(ColsDV(intCol).Item("NullText"))
                        BoolCol.TrueValue = CType("1", Decimal)
                        BoolCol.FalseValue = CType("0", Decimal)
                        BoolCol.AllowNull = False

                        Try

                            If CBool(ColsDV(intCol).Item("ReadOnly")) Then
                                BoolCol.ReadOnly = True
                            End If

                        Catch ex As Exception
                        End Try
                        DGTS.GridColumnStyles.Add(BoolCol)
                    End If
                End If
            Next
        Catch ex As Exception

            Throw
        End Try

        Try
            dg.TableStyles.Clear()
            dg.TableStyles.Add(DGTS)
            dg.TableStyles.Add(DefaultDGTS)
            dg.ContextMenuPrepare(dataGridCustomContextMenu)

        Catch ex As Exception

            Throw
        Finally
            CurMan = Nothing
            DGTS = Nothing
        End Try


    End Sub

    Public Function ValidateRemarks() As Boolean

        If txtRemarks.Text.Trim.Length > 0 Then
        Else
            MessageBox.Show("Remarks is Required", "REMARK", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return True
        End If
        Return False
    End Function

    Public Function MadeHRSChanges() As Boolean
        Dim strchangesmade As String = ""
        Try
            '' _ChangedELGHRSRows = CType(_ChangedELGHRSRows.GetChanges(), EligAcctHoursDS)
            If _ChangedELGHRSRows IsNot Nothing AndAlso _ChangedELGHRSRows.Tables("ELIG_ACCT_HOURS").Rows.Count > 0 Then
                Return True
            End If
        Catch ex As Exception

            Throw
        End Try
        Return False
    End Function

    Public Function MadeELGMTHDTLChanges() As Boolean
        Dim ChangesMade As String = ""

        Try
            _ChangedELGMTHRows = CType(_ChangedELGMTHRows.GetChanges(), EligMthdtlDS)
            If _ChangedELGMTHRows IsNot Nothing AndAlso _ChangedELGMTHRows.Tables("ELIG_MTHDTL").Rows.Count > 0 Then
                If _ChangedELGMTHRows IsNot Nothing AndAlso _ChangedELGMTHRows.Tables("ELIG_MTHDTL").Rows.Count > 0 Then

                    For i = 0 To _ChangedELGMTHRows.Tables("ELIG_MTHDTL").Rows.Count - 1
                        Dim DR As DataRow = _ChangedELGMTHRows.Tables("ELIG_MTHDTL").Rows(i)
                        If DR.RowState = DataRowState.Added Then
                            Return True
                        End If
                    Next
                End If
            End If
        Catch ex As Exception

            Throw
        End Try
        Return False
    End Function

    ''Public Function SaveELGMTHchanges(ByRef Transaction As DbTransaction) As Boolean
    ''    Dim HistSum As String = ""
    ''    Dim HistDetail As String = ""
    ''    Dim arreligperiod As Array

    ''    Try

    ''        For Each r As DataRow In _ChangedELGMTHRows.Tables("ELIG_MTHDTL").Rows


    ''            HistSum = "ELIGIBILITY MOVED TO FAMILYID " & txtnewfamid.Text.ToString
    ''            HistDetail = UFCWGeneral.DomainUser.ToUpper & " ELIGIBILTY MOVED TO NEW FAMILYID: " & txtnewfamid.Text.ToString & " FROM OLD FAMILYID " & txtoldfam.Text.ToString & " AND  ELIGBILITY PERIODS " & _
    ''                                                       "FROM "

    ''            ''RegMasterDAL.InsertMTHDTLfromSSNchanges(CInt(txtoldfam.Text.ToString), CInt(txtnewfamid.Text.ToString), CDate(r("ELIG_PERIOD")), CStr(r("STATUS")), _
    ''            ''                                        CStr(r("PLANTYPE")), CStr(r("MEMTYPE")), CInt(r("LOCALNO")), CInt(r("MEDICAL_PLAN")), CInt(r("DENTAL_PLAN")), _
    ''            ''                                        CDec(r("MED_ELIG_SW")), CDec(r("DEN_ELIG_SW")), CDec(r("PREMIUM_SW")), CDec(r("FAMILY_SW")), _
    ''            ''                                        CStr(r("RET_PLAN")), CShort(CInt(r("A2COUNT"))), CDate(r("PLAN_AB_1ST_ELGDATE")), CDec(r("BREAK_IN_SERVICE_SW")), Transaction)

    ''        Next

    ''        ''  RegMasterDAL.InsertSSNchanges(CInt(txtoldSSN.Text), CInt(txtoldfam.Text), CInt(txtnewSSN.Text), CInt(txtnewfamid.Text), txtremarks.Text, Transaction)

    ''        ''   RegMasterDAL.InsertMTHDTLfromSSNchanges(CInt(txtoldSSN.Text), CInt(txtoldfam.Text), CInt(txtnewSSN.Text), CInt(txtnewfamid.Text), txtremarks.Text, arreligperiod, Transaction)

    ''        RegMasterDAL.CreateRegHistory(CInt(txtnewfamid.Text), 0, Nothing, Nothing, "SSNCHANGES", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, Transaction)

    ''        HistSum = ""
    ''        HistDetail = ""
    ''        Return True
    ''    Catch ex As Exception

    ''        Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    ''        If (rethrow) Then
    ''            Throw
    ''        End If
    ''        Return False
    ''    End Try
    ''End Function

    Public Function SaveELGHRSchanges(ByRef transaction As DbTransaction) As Boolean

        Dim HistSum As String = ""
        Dim HistDetail As String = ""
        Dim movedhrs As Decimal
        Dim mailbody As String = ""

        Try

            If _ChangedELGHRSRows.Tables("ELIG_ACCT_HOURS").Rows.Count > 0 Then

                HistSum = "ELIGIBILITY HOURS MOVED TO FAMILYID " & txtNewFID.Text.ToString
                HistDetail = UFCWGeneral.DomainUser.ToUpper & " MOVED ELIGIBILTY TO NEW FAMILYID: " & txtNewFID.Text.ToString & " FROM OLD FAMILYID " & txtOldFID.Text.ToString & " AND  ELIGIBILITY PERIOD " &
                                                           "FROM " & CStr(_MinEligPeriod) & " THRU " & CStr(_MaxEligPeriod)
                RegMasterDAL.InsertMTHDTLfromSSNchanges(CInt(txtOldSSN.Text.Trim.ToString), CInt(txtOldFID.Text.ToString), CInt(txtNewSSN.Text.Trim.ToString), CInt(txtNewFID.Text.ToString), CStr(txtRemarks.Text), _MinEligPeriod, _MaxEligPeriod, transaction)
                movedhrs = RegMasterDAL.InsertACCTHOURSfromSSNchanges(CInt(txtOldSSN.Text.Trim.ToString), CInt(txtOldFID.Text.ToString), CInt(txtNewSSN.Text.Trim.ToString), CInt(txtNewFID.Text.ToString), CStr(txtRemarks.Text), _MinEligPeriod, _MaxEligPeriod, transaction)

                mailbody = " Eligibility Hours moved from Social  " & UFCWGeneral.FormatSSN(txtOldSSN.Text.ToString) & " to New Social " & UFCWGeneral.FormatSSN(txtNewSSN.Text.ToString) &
                           " For work period " & CStr(_MinWorkPeriod) & " THRU " & CStr(_MaxWorkPeriod) &
                           " ( For Eligibility period " & CStr(_MinEligPeriod) & " THRU " & CStr(_MaxEligPeriod) & " )" &
                           " for a TOTAL hours of " & movedhrs.ToString & ". " &
                            txtRemarks.Text & " Remark added by " & UFCWGeneral.DomainUser & "."


                CreateEmail(movedhrs, mailbody.ToString, transaction)

            End If
            RegMasterDAL.CreateRegHistory(CInt(txtNewFID.Text), 0, Nothing, Nothing, "SSNCHANGES", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, transaction)

            Return True

        Catch ex As Exception


            Throw
        Finally
            mailbody = Nothing
        End Try
    End Function

    Public Sub CreateEmail(ByVal hours As Decimal, ByVal mailBody As String, ByVal transaction As DbTransaction)
        Try
            If UFCWEMail.MailEnabled Then
                UFCWEMail.SendMail(_SSNCorrectionTOEmail, _SSNCorrectionCCEmail, "", _MailFrom, _SSNCorrectionMailSubject, mailBody, Nothing)
            End If

        Catch ex As Exception


            Throw
        Finally
            mailBody = Nothing
        End Try
    End Sub

    Private Sub NEWclearall()
        _NewFamilyID = -1


        If _TotalNewEligMthdtlDS IsNot Nothing Then
            _TotalNewEligMthdtlDS = Nothing
        End If


        If _TotalNewEligHrsDS IsNot Nothing Then
            _TotalNewEligHrsDS = Nothing
        End If

        If (_ChangedELGHRSRows) IsNot Nothing AndAlso _ChangedELGHRSRows.ELIG_ACCT_HOURS.Rows.Count > 0 Then
            _ChangedELGHRSRows.ELIG_ACCT_HOURS.Clear()
        End If

        If (_ChangedELGMTHRows) IsNot Nothing AndAlso _ChangedELGMTHRows.ELIG_MTHDTL.Rows.Count > 0 Then
            _ChangedELGMTHRows.ELIG_MTHDTL.Rows.Clear()
        End If



        If Not IsNothing(_SelectedFrom) Then _SelectedFrom = Nothing
        If Not IsNothing(_SelectedThru) Then _SelectedThru = Nothing
        If Not IsNothing(_SelectedRemoveFrom) Then _SelectedRemoveFrom = Nothing
        If Not IsNothing(_SelectedRemoveThru) Then _SelectedRemoveThru = Nothing
        If Not IsNothing(_MinEligPeriod) Then _MinEligPeriod = Nothing
        If Not IsNothing(_MaxEligPeriod) Then _MaxEligPeriod = Nothing
        If Not IsNothing(_MinWorkPeriod) Then _MinWorkPeriod = Nothing
        If Not IsNothing(_MaxWorkPeriod) Then _MaxWorkPeriod = Nothing

    End Sub

    Private Sub Oldclearall()
        _OldFamilyID = -1

        If _TotalOldEligMthdtlDS IsNot Nothing Then
            _TotalOldEligMthdtlDS = Nothing
        End If

        If _TotalOldEligHrsDS IsNot Nothing Then
            _TotalOldEligHrsDS = Nothing
        End If

        If _ChangedELGHRSRows IsNot Nothing AndAlso _ChangedELGHRSRows.ELIG_ACCT_HOURS.Rows.Count > 0 Then
            _ChangedELGHRSRows.ELIG_ACCT_HOURS.Clear()
        End If

        If _ChangedELGMTHRows IsNot Nothing AndAlso _ChangedELGMTHRows.ELIG_MTHDTL.Rows.Count > 0 Then
            _ChangedELGMTHRows.ELIG_MTHDTL.Rows.Clear()
        End If


        If Not IsNothing(_SelectedFrom) Then _SelectedFrom = Nothing
        If Not IsNothing(_SelectedThru) Then _SelectedThru = Nothing
        If Not IsNothing(_SelectedRemoveFrom) Then _SelectedRemoveFrom = Nothing
        If Not IsNothing(_SelectedRemoveThru) Then _SelectedRemoveThru = Nothing
        If Not IsNothing(_MinEligPeriod) Then _MinEligPeriod = Nothing
        If Not IsNothing(_MaxEligPeriod) Then _MaxEligPeriod = Nothing
        If Not IsNothing(_MinWorkPeriod) Then _MinWorkPeriod = Nothing
        If Not IsNothing(_MaxWorkPeriod) Then _MaxWorkPeriod = Nothing

    End Sub

    Private Sub txtSSN_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtOldSSN.KeyPress, txtNewSSN.KeyPress
        Dim EntryOkRegex As Regex = New Regex("^[0-9\-]+$")

        If Not (System.Char.IsControl(e.KeyChar)) AndAlso Not EntryOkRegex.IsMatch(e.KeyChar) Then
            e.Handled = True
        End If

    End Sub

    Private Sub txtSSN_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOldSSN.Enter, txtNewSSN.Enter

        Dim TBox As TextBox = CType(sender, TextBox)

        Try

            If TBox.Text.Replace("-", "").Equals("000000000") Then TBox.Text = ""

        Catch ex As Exception

        End Try

    End Sub
    Private Sub txtSSN_Validating(sender As Object, e As CancelEventArgs) Handles txtOldSSN.Validating, txtNewSSN.Validating

        Dim TBox As TextBox = CType(sender, TextBox)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ErrorProvider1.ClearError(TBox)

            If TBox.Text.Trim.Length > 0 Then

                If (TBox.Text.Trim.Length <> 9 AndAlso TBox.Text.Trim.Length <> 11) OrElse (TBox.Text.Trim.Length = 9 AndAlso Not IsNumeric(TBox.Text)) OrElse (TBox.Text.Contains("-") AndAlso TBox.Text.Replace("-", "").Trim.Length <> 9) Then
                    ErrorProvider1.SetErrorWithTracking(TBox, "SSN invalid. use 9 digits in formats 123-45-6789 or 123456789 only")
                End If

            End If

            If ErrorProvider1.GetError(TBox).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
            Else
                If TBox.Text.Trim.Length > 0 AndAlso TBox.Text <> UFCWGeneral.FormatSSN(TBox.Text) Then
                    TBox.Text = UFCWGeneral.FormatSSN(TBox.Text)
                End If
            End If

        Catch ex As Exception

            Throw

        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

#End Region

End Class
