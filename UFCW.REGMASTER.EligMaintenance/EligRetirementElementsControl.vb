Option Strict On
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Data.Common
Imports DDTek.DB2
Imports System.Reflection

Public Class EligRetirementElementsControl
    Inherits System.Windows.Forms.UserControl
#Region "Variables"

    Private _FamilyID As Integer = -1
    Private _APPKEY As String = "UFCW\RegMaster\"
    Private _ReadOnlyMode As Boolean = True
    Private _ChangedERERows As DataSet
    Private _ViewHistory As Boolean?

    Private WithEvents _EREBS As BindingSource
    Private WithEvents _EREDS As New DataSet

    Private _TotalEREDS As New DataSet
    Private _LocalDT As New DataTable
    Private _RetPlanDT As New DataTable
    Private _PreviousDR As DataRow

    ReadOnly _REGMEmployeeAccess As Boolean = UFCWGeneralAD.REGMEmployeeAccess
    ReadOnly _REGMReadOnlyAccess As Boolean = UFCWGeneralAD.REGMReadOnlyAccess
    ReadOnly _REGMVendorAccess As Boolean = UFCWGeneralAD.REGMVendorAccess
    ReadOnly _REGMRegMasterDeleteAccess As Boolean = UFCWGeneralAD.REGMRegMasterDeleteAccess

#End Region

    Private _Disposed As Boolean = False
    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.
            '
            If RetireeHistoryDataGrid IsNot Nothing Then RetireeHistoryDataGrid.Dispose()
            RetireeHistoryDataGrid = Nothing

            If _ChangedERERows IsNot Nothing Then _ChangedERERows.Dispose()
            _ChangedERERows = Nothing

            If _LocalDT IsNot Nothing Then _LocalDT.Dispose()
            _LocalDT = Nothing

            If _RetPlanDT IsNot Nothing Then _RetPlanDT.Dispose()
            _RetPlanDT = Nothing

            If _EREBS IsNot Nothing Then _EREBS.Dispose()
            _EREBS = Nothing

            If _TotalEREDS IsNot Nothing Then _TotalEREDS.Dispose()
            _TotalEREDS = Nothing

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

#Region "Properties"

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("History Button is visible")>
    Public Property VisibleHistory() As Boolean
        Get
            Return If(_ViewHistory Is Nothing, False, CBool(_ViewHistory))
        End Get
        Set(ByVal value As Boolean)
            _ViewHistory = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Indicates that Save has not been used.")>
    Public ReadOnly Property ChangesPending() As Boolean
        Get
            Return UnCommittedChangesExist()
        End Get
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the FamilyID of the Document.")>
    Public Property FamilyID() As Integer
        Get
            Return _FamilyID
        End Get
        Set(ByVal Value As Integer)
            _FamilyID = Value
        End Set
    End Property

    <Browsable(True), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal Value As String)

            _APPKEY = Value
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Determines if control is in Read Only Mode.")>
    Public Property ReadOnlyMode() As Boolean
        Get
            Return _ReadOnlyMode
        End Get
        Set(ByVal Value As Boolean)
            _ReadOnlyMode = Value
        End Set
    End Property

    Public Property WaitPeriodDataset() As DataSet
        Get
            Return _TotalEREDS
        End Get
        Set(ByVal Value As DataSet)
            _TotalEREDS = Value
        End Set
    End Property

#End Region

#Region "Constructor"
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Dim designMode As Boolean = (LicenseManager.UsageMode = LicenseUsageMode.Designtime)
        If Not designMode Then
            LoadRetireeTypes()
        End If

        'dont want to display the default table style
    End Sub

    Public Sub New(ByVal readonlymode As Boolean, ByVal FamilyID As Integer)
        Me.New()

        _FamilyID = FamilyID
        _ReadOnlyMode = readonlymode

        txtFamilyID.Text = _FamilyID.ToString
        grpEditPanel.Enabled = False

    End Sub
#End Region

#Region "Form\Button Events"

    Private Sub AddButton_Click(sender As System.Object, e As System.EventArgs) Handles AddActionButton.Click

        Try

            AddActionButton.Enabled = False
            DeleteActionButton.Enabled = False

            AddERELine()

            cmbRetPlan.SelectedIndex = -1

            txtFromDate.Focus()

        Catch ex As Exception


                Throw

        Finally

        End Try
    End Sub

    Private Sub CancelActionButton_Click(sender As System.Object, e As System.EventArgs) Handles CancelActionButton.Click
        Dim Result As DialogResult = DialogResult.None

        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

        Try

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

            CancelActionButton.Enabled = False
            SaveActionButton.Enabled = False

            Result = MessageBox.Show(Me, "Do you want to Cancel the changes?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If Result = DialogResult.Yes Then

                _EREDS.Tables("ELGRETIREE_ELEMENTS").RejectChanges()

                ClearErrors()

                ProcessControls(CType(grpEditPanel, Object))

            ElseIf Result = DialogResult.No Then
                CancelActionButton.Enabled = True
                SaveActionButton.Enabled = True
            End If

        Catch ex As Exception
            Throw
        Finally
            Me.AutoValidate = HoldAutoValidate
        End Try

    End Sub

    Private Sub SaveActionButton_Click(sender As System.Object, e As System.EventArgs) Handles SaveActionButton.Click

        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

        Try

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

            CancelActionButton.Enabled = False
            SaveActionButton.Enabled = False

            If Not Me.ValidateChildren(ValidationConstraints.Enabled) OrElse VerifyEREChanges() Then
                SaveActionButton.Enabled = True
                CancelActionButton.Enabled = True
                Exit Sub
            End If

            _EREBS.EndEdit()

            _ChangedERERows = _EREDS.GetChanges()

            If _ChangedERERows Is Nothing Then     '' when new row added,deleted then cancel, save buttons are enabled
                MessageBox.Show("There are no changes to the record.", "No Changes", MessageBoxButtons.OK, MessageBoxIcon.Information)
                SaveActionButton.Enabled = False
                CancelActionButton.Enabled = False
                Exit Sub
            End If

            If _ChangedERERows IsNot Nothing Then

                Try

                    If ApplyEREChanges() Then

                        MessageBox.Show("Retiree History Record Saved Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        _TotalEREDS = Nothing
                        _ChangedERERows.Clear()

                        '' Message to user to refresh eligiblity tab
                        RegMasterDAL.MadeEligibilityChanges = True
                        MessageBox.Show("Please refresh Eligibility Tab to see these changes. ", "Refresh Eligibility", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else

                        SaveActionButton.Enabled = True
                        CancelActionButton.Enabled = True
                        Exit Sub

                    End If

                Catch ex As Exception

                    Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
                    If (rethrow) Then
                        Throw
                    End If

                End Try
            End If

        Catch ex As Exception


                Throw

        Finally

            ProcessControls(CType(grpEditPanel, Object))

            Me.AutoValidate = HoldAutoValidate

        End Try

    End Sub

    Private Sub ModifyActionButton_Click(sender As Object, e As EventArgs) Handles ModifyActionButton.Click
        Dim DR As DataRow

        Try

            If _EREBS Is Nothing OrElse _EREBS.Position < 0 OrElse _EREBS.Count < 1 Then Return

            DR = DirectCast(_EREBS.Current, DataRowView).Row

            DR.BeginEdit() ' trigger currentitemchanged
            DR.EndEdit() ' trigger currentitemchanged

            ProcessControls(CType(grpEditPanel, Object))

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Function AdjustCurrentAndPriorDates() As Boolean

        Dim CurrentDR As DataRow 'row being expired, changed, etc
        Dim PriorDR As DataRow 'row possiblly modified to reflect new THRU_DATE
        Dim NewDR As DataRow 'Final Current Row, could represent future date, or the approved row.

        Dim Changed As Boolean = False

        Dim BS As BindingSource

        Try

            BS = _EREBS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Dim CurrentQuery =
                        From ERE In _EREDS.Tables("ELGRETIREE_ELEMENTS").AsEnumerable()
                        Where ERE.RowState <> DataRowState.Added
                        Order By ERE.Field(Of Date)("THRU_DATE", DataRowVersion.Original) Descending

            For Each AddresseDR As DataRow In CurrentQuery

                If CurrentDR IsNot Nothing AndAlso PriorDR Is Nothing Then
                    PriorDR = AddresseDR
                    Exit For
                End If

                If CurrentDR Is Nothing Then
                    CurrentDR = AddresseDR
                End If
            Next

            Dim AddedQuery =
                From Addresses In _EREDS.Tables("ELGRETIREE_ELEMENTS").AsEnumerable()
                Where Addresses.RowState = DataRowState.Added

            If AddedQuery.Any Then
                NewDR = AddedQuery.First
            End If

            If NewDR IsNot Nothing AndAlso CurrentDR IsNot Nothing Then

                If Not IsDBNull(NewDR("FROM_DATE")) AndAlso (CDate(CurrentDR("THRU_DATE", DataRowVersion.Original)) = CDate("9999-12-31") OrElse (CDate(CurrentDR("THRU_DATE", DataRowVersion.Original)).AddDays(1) <> CDate(NewDR("FROM_DATE")))) Then
                    CurrentDR.BeginEdit()
                    CurrentDR("THRU_DATE") = CDate(NewDR("FROM_DATE")).AddDays(-1)
                    CurrentDR.EndEdit()
                    Changed = True
                End If

            ElseIf PriorDR IsNot Nothing Then

                If CurrentDR.RowState = DataRowState.Deleted Then

                    PriorDR.BeginEdit()

                    PriorDR("THRU_DATE") = CDate("9999-12-31")

                    Changed = True

                    PriorDR.EndEdit()

                ElseIf CDate(CurrentDR("FROM_DATE")) = CDate(PriorDR("FROM_DATE", DataRowVersion.Original)) Then

                    PriorDR.Delete() 'prior row completely removed

                    Changed = True

                ElseIf CDate(PriorDR("THRU_DATE")) <> CDate(txtFromDate.Text).AddDays(-1) Then
                    'prior row is adjusted

                    PriorDR.BeginEdit()

                    PriorDR("THRU_DATE") = CDate(txtFromDate.Text).AddDays(-1)

                    Changed = True

                    PriorDR.EndEdit()

                End If

            End If

            Return Changed

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Function


    'Private Sub txtFromDate_Leave(sender As Object, e As EventArgs)

    '    If _EREBS Is Nothing OrElse _EREBS.Position < 0 Then Return

    '    Dim DR As DataRow

    '    Dim FromDates = New List(Of Date)
    '    Dim nextfromdate As Date = Nothing : Dim nextkey As Integer = 0
    '    Dim previousfromdate As Date = Nothing
    '    Dim previouskey As Integer = 0 : Dim previousthrudate As Date
    '    Dim currentkey As Integer = 0 : Dim currentthrudate As Date = CDate("12-31-9999")
    '    Dim fromdatedict As New Dictionary(Of Integer, Date)
    '    Dim key As Integer = 0

    '    Try
    '        DR = DirectCast(_EREBS.Current, DataRowView).Row

    '        '' date is always start of the month.

    '        Dim dtfrom As Date = CDate(txtFromDate.Text)
    '        Dim startday As Date = dtfrom.AddDays(-(DatePart(DateInterval.Day, dtfrom) * 1.0 - 1))
    '        If DateDiff(DateInterval.Day, startday, CDate(DR("FROM_DATE"))) <> 0 Then
    '            txtFromDate.Text = Format(startday, "MM-dd-yyyy")
    '            DR("FROM_DATE") = txtFromDate.Text
    '        End If

    '        '' no overlaps

    '        If IsDate(DR("FROM_DATE")) Then

    '            Dim drallfromdate() As DataRow = _EREDS.Tables("ELGRETIREE_ELEMENTS").Select("", "FROM_DATE", DataViewRowState.CurrentRows)
    '            If drallfromdate.Count > 1 Then
    '                Dim allfromdate = drallfromdate.AsEnumerable
    '                '' frmdate = (From r In allfromdate Select r.Field(Of Date)("FROM_DATE")).ToArray

    '                FromDates = (From r In allfromdate Order By r("FROM_DATE") Select r.Field(Of Date)("FROM_DATE")).ToList

    '                For Each r In FromDates
    '                    fromdatedict.Add(key, CDate(FromDates(key)))
    '                    If CDate(FromDates(key)) = CDate(txtFromDate.Text) Then
    '                        currentkey = key
    '                    End If
    '                    key += 1
    '                Next

    '                nextkey = currentkey + 1
    '                previouskey = currentkey - 1

    '                If fromdatedict.ContainsKey(nextkey) Then
    '                    nextfromdate = CDate(FromDates(nextkey))
    '                    currentthrudate = CDate(Format(UFCWGeneral.MonthEndDate(nextfromdate.AddDays(-1)), "MM-dd-yyyy"))
    '                End If

    '                '' PREVIOUS ROW VALUES
    '                If fromdatedict.ContainsKey(previouskey) Then
    '                    previousthrudate = CDate(Format(UFCWGeneral.MonthEndDate(CDate(txtFromDate.Text).AddDays(-1)), "MM-dd-yyyy"))
    '                    previousfromdate = fromdatedict(previouskey)
    '                End If

    '                Dim PreviousDRs() As DataRow = _EREDS.Tables("ELGRETIREE_ELEMENTS").Select("FROM_DATE <> THRU_DATE AND FROM_DATE = '" & CStr(previousfromdate) & "'", "FROM_DATE", DataViewRowState.CurrentRows)
    '                If PreviousDRs.Any Then
    '                    PreviousDRs(0)("THRU_DATE") = previousthrudate
    '                End If

    '                '' current thru date

    '                txtThruDate.Text = CStr(currentthrudate)
    '                DR("THRU_DATE") = currentthrudate

    '                cmbRetPlan.Enabled = True

    '                LoadRetireeTypes()

    '                RetireeHistoryDataGrid.Refresh()


    '            ElseIf drallfromdate.Count = 1 Then   '' no record, setting retiree
    '                LoadRetireeTypes()
    '                txtThruDate.Text = "12-31-9999"
    '                DR("THRU_DATE") = "12-31-9999"
    '                cmbRetPlan.Enabled = True
    '            End If
    '        End If
    '        ''

    '    Catch ex As Exception

    '    End Try
    'End Sub

    'Private Sub cmbRetPlan_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    '    Dim RetireeDR As DataRow
    '    Dim SelectRetireeDR As Object
    '    Dim DV As DataView
    '    Dim DR As DataRow

    '    Dim CBox As ExComboBox = CType(sender, ExComboBox)

    '    If _EREBS Is Nothing OrElse _EREBS.Position < 0 OrElse _EREBS.Count < 1 OrElse CBox.ReadOnly Then Return

    '    Try

    '        If CBox.SelectedIndex > -1 Then
    '            DR = DirectCast(_EREBS.Current, DataRowView).Row

    '            If DR.RowState <> DataRowState.Unchanged Then

    '                If IsDBNull(DR("RETIREE_PLAN")) OrElse (Not IsDBNull(DR("RETIREE_PLAN")) AndAlso Not DR("RETIREE_PLAN").ToString.Equals(If(IsNothing(CBox.SelectedValue), DBNull.Value, CType(sender, ComboBox).SelectedValue).ToString)) Then
    '                    SelectRetireeDR = If((CBox.Text) Is Nothing, DBNull.Value, CBox.SelectedValue)
    '                End If

    '                If SelectRetireeDR IsNot Nothing Then
    '                    DR("RETIREE_PLAN") = CObj(CStr(SelectRetireeDR).Substring(0, 1))
    '                End If

    '                If CBox.SelectedIndex > -1 Then
    '                    If DR.RowState <> DataRowState.Unchanged Then
    '                        Dim percent As String = ""

    '                        percent = CBox.Text.Substring(1)

    '                        DV = New DataView(_RetPlanDT, "RETIREE_PLAN = '" & CStr(DR("RETIREE_PLAN")) & "' AND RETIREE_PERCENT = " & percent, "", DataViewRowState.CurrentRows)
    '                        If DV.Count > 0 Then
    '                            RetireeDR = DV(0).Row    '' seleted row
    '                            If RetireeDR IsNot Nothing Then
    '                                '' assigning values from selected row
    '                                '' RETIREE_PERCENT
    '                                DR("RETIREE_PLAN") = RetireeDR("RETIREE_PLAN")
    '                                DR("RETIREE_PERCENT") = percent
    '                            End If
    '                        End If
    '                    End If
    '                End If
    '            End If
    '        End If

    '    Catch ex As Exception
    '        Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '        If (rethrow) Then
    '            Throw
    '        End If
    '    Finally
    '        If DV IsNot Nothing Then
    '            DV.Dispose()
    '        End If
    '        DV = Nothing
    '        DR = Nothing

    '    End Try
    'End Sub

    '    Private Sub RetireeHistoryDataGrid_CurrentRowChanged(CurrentRowIndex As Integer?, LastRowIndex As Integer?)

    '        If CurrentRowIndex = -1 Then Exit Sub
    '        If IsNothing(LastRowIndex) Then LastRowIndex = -1
    '        Dim dv As DataView = RetireeHistoryDataGrid.GetDefaultDataView
    '        Dim Result As DialogResult = DialogResult.None
    '        Dim Transaction As DbTransaction = Nothing
    '        Dim _AllowCloseCancel As Boolean = True

    '        Try

    '            If dv Is Nothing Then Return

    '            ' ''row updated, when navigating to another forcing the cursor in the same row.
    '            If LastRowIndex = -1 Then LastRowIndex = 0

    '            If (Not LastRowIndex = RetireeHistoryDataGrid.GetGridRowCount) Then
    '                If (dv(CInt(LastRowIndex)).Row.RowState = DataRowState.Modified AndAlso DataGridCustom.IdentifyChanges(dv(CInt(LastRowIndex)).Row, RetireeHistoryDataGrid).Length > 0) OrElse (dv(CInt(LastRowIndex)).Row.RowState = DataRowState.Added) Then
    '                    If LastRowIndex = -1 Then LastRowIndex = CurrentRowIndex
    '                    _EREBS.Position = CInt(LastRowIndex)
    '                    RetireeHistoryDataGrid.Select(_EREBS.Position)
    '                    ModifyButton.Enabled = False
    '                    If UnCommittedChangesExist() AndAlso dv(RetireeHistoryDataGrid.CurrentRowIndex).Row.RowState <> DataRowState.Added Then
    '                        GoTo rep
    '                    End If
    '                Else
    '                    ModifyButton.Enabled = True
    '                End If

    '            End If

    '            If dv(RetireeHistoryDataGrid.CurrentRowIndex).Row.RowState <> DataRowState.Added Then
    '                If UnCommittedChangesExist() Then
    'rep:
    '                    Result = MessageBox.Show(Me, "Changes have been made to Retiree History Record." & vbCrLf &
    '                                    "Would you like to save the changes?", "Save Changes", If(_AllowCloseCancel = True, MessageBoxButtons.YesNoCancel, MessageBoxButtons.YesNo), MessageBoxIcon.Question)

    '                    ClearErrors()
    '                    If Result = DialogResult.Yes Then
    '                        ''Cancel if info not valid
    '                        If VerifyEREChanges() Then
    '                            If _AllowCloseCancel = True Then
    '                                Result = DialogResult.Cancel
    '                                GoTo ExitSub ''
    '                            Else
    '                                Result = DialogResult.None
    '                                GoTo ExitSub
    '                            End If
    '                        End If
    '                        Transaction = RegMasterDAL.BeginTransaction

    '                        If ApplyEREChanges(Transaction) = True Then
    '                            RegMasterDAL.CommitTransaction(Transaction)
    '                            MessageBox.Show("Retiree History record Saved Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '                            _EREDS.AcceptChanges()
    '                        Else
    '                            RegMasterDAL.RollbackTransaction(Transaction)
    '                            MessageBox.Show("Error while saving Retiree History record." & vbCrLf & "Please try again ", "Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                        End If
    '                    End If
    'ExitSub:
    '                    If Result = DialogResult.No Then

    '                        _EREBS.EndEdit()
    '                        _EREDS.Tables("ELGRETIREE_ELEMENTS").RejectChanges()

    '                        grpEditPanel.Enabled = False
    '                        CancelButton.Enabled = False
    '                        SaveButton.Enabled = False
    '                        AddButton.Enabled = True
    '                        ModifyButton.Enabled = True
    '                        DeleteButton.Enabled = True

    '                    End If

    '                    If Result = DialogResult.Cancel Then
    '                        _EREBS.Position = CInt(LastRowIndex)
    '                        RetireeHistoryDataGrid.Select(_EREBS.Position)
    '                    End If

    '                Else  ''Madechange()
    '                    grpEditPanel.Enabled = False
    '                    AddButton.Enabled = True
    '                    DeleteButton.Enabled = True
    '                    CancelButton.Enabled = False
    '                    SaveButton.Enabled = False
    '                End If
    '            End If

    '        Catch ex As Exception
    '            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '            If (rethrow) Then
    '                Throw
    '            End If
    '        End Try
    '    End Sub

    Private Sub DeleteERELine()

        Dim OriginalCurrentDR As DataRow 'row being expired, changed, etc
        Dim OriginalPriorDR As DataRow 'row possiblly modified to reflect new THRU_DATE

        Try

#If DEBUG Then
            RemoveHandler _EREBS.PositionChanged, AddressOf EREBS_PositionChanged
#End If

            Dim OriginalCurrentQuery =
                        From ExistingCoverage In CType(_EREBS.DataSource, DataTable).AsEnumerable
                        Where ExistingCoverage.RowState <> DataRowState.Deleted
                        Order By ExistingCoverage.Field(Of Date)("THRU_DATE") Descending

            For Each CoverageDR As DataRow In OriginalCurrentQuery

                If OriginalCurrentDR IsNot Nothing AndAlso OriginalPriorDR Is Nothing Then
                    OriginalPriorDR = CoverageDR
                    Exit For
                End If

                If OriginalCurrentDR Is Nothing Then
                    OriginalCurrentDR = CoverageDR
                End If
            Next

            OriginalCurrentDR.Delete()
            OriginalCurrentDR.EndEdit()

            _EREBS.EndEdit()

            If _EREBS.Position > -1 Then

                AdjustCurrentAndPriorDates()

                RetireeHistoryDataGrid.Select(_EREBS.Position)

            End If

        Catch ex As Exception


                Throw
        Finally
#If DEBUG Then
            AddHandler _EREBS.PositionChanged, AddressOf EREBS_PositionChanged
#End If
        End Try
    End Sub
    Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteActionButton.Click

        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

        Try

            If _EREBS Is Nothing OrElse _EREBS.Position < 0 Then Return

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from occurring when buttons are disabled

            DeleteActionButton.Enabled = False

            If MessageBox.Show(Me, "Are you sure you want to Delete the current Retiree History Record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                DeleteERELine()

            End If

            ProcessControls(CType(grpEditPanel, Object))

        Catch ex As Exception


                Throw

        Finally
            Me.AutoValidate = HoldAutoValidate
        End Try


        'Dim Transaction As DbTransaction = Nothing
        'Dim DR As DataRow = Nothing

        'Try

        '    SaveActionButton.Enabled = False
        '    CancelActionButton.Enabled = False

        '    Try
        '        If MessageBox.Show(Me, "Are you sure you want to Delete the current Retiree History Record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

        '            FixRetireefromthrudates(DR)

        '            Transaction = RegMasterDAL.BeginTransaction
        '            If DeleteRetireeHistoryRecord(DR, Transaction) = True Then

        '            Else
        '                MessageBox.Show("Error while Deleting coverage record.", "Save rejected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '                RegMasterDAL.RollbackTransaction(Transaction)
        '                DeleteActionButton.Enabled = True
        '                Return
        '            End If

        '            If ApplyEREChanges(Transaction) Then
        '                RegMasterDAL.CommitTransaction(Transaction)
        '                MessageBox.Show("Retiree History Record Deleted Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '                _EREDS.AcceptChanges()
        '                _EREDS.Tables("ELGRETIREE_ELEMENTS").Rows.Clear()                        '' when added new coverage we are terminating old coverage and needs to get from db.
        '                _TotalEREDS = Nothing

        '                LoadEligRetireeElements()

        '            Else
        '                RegMasterDAL.RollbackTransaction(Transaction)
        '                MessageBox.Show("Error while Deleting Retiree History." & vbCrLf & "Please Refresh and try again ", "Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '            End If
        '        End If

        '    Catch db2ex As DB2Exception

        '        Select Case db2ex.Number
        '            Case -438
        '                MessageBox.Show("Error while Deleting Retiree History record.", "Save rejected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        '        End Select

        '        RegMasterDAL.RollbackTransaction(Transaction)

        '    Catch ex As Exception

        '        Try
        '            RegMasterDAL.RollbackTransaction(Transaction)
        '        Catch ex2 As Exception

        '        End Try

        '        Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
        '        If (rethrow) Then
        '            Throw
        '        End If

        '    End Try

        'Catch ex As Exception

        '    SaveActionButton.Enabled = True
        '    CancelActionButton.Enabled = True ' this is done in the error handler just in case a failure occurs

        '    Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
        '    If (rethrow) Then
        '        Throw
        '    End If

        'Finally

        'DR = Nothing
        'If Transaction IsNot Nothing Then Transaction.Dispose()
        'Transaction = Nothing
        'End Try

    End Sub

#End Region

#Region "Custom Subs\Functions"

    Public Sub LoadEligRetireeElements(ByVal readonlymode As Boolean, ByVal familyID As Integer)

        _FamilyID = familyID
        _ReadOnlyMode = readonlymode

        txtFamilyID.Text = _FamilyID.ToString

        LoadEligRetireeElements()

    End Sub

    Public Sub LoadEligRetireeElements()

        Try

            ClearErrors()

            LoadRetireeTypes()

            If _TotalEREDS Is Nothing OrElse _TotalEREDS.Tables.Count < 1 OrElse _TotalEREDS.Tables("ELGRETIREE_ELEMENTS").Rows.Count < 1 Then
                _TotalEREDS = RegMasterDAL.RetrieveEligRetireeElementsByFamilyID(_FamilyID)
            End If

            _EREDS = New DataSet
            _EREDS.Tables.Add(_TotalEREDS.Tables("ELGRETIREE_ELEMENTS").Clone)

            For Each DR As DataRow In _TotalEREDS.Tables("ELGRETIREE_ELEMENTS").Rows
                _EREDS.Tables("ELGRETIREE_ELEMENTS").ImportRow(DR)
            Next

            RemoveHandler _EREDS.Tables("ELGRETIREE_ELEMENTS").ColumnChanging, AddressOf EREDS_ColumnChanging
            AddHandler _EREDS.Tables("ELGRETIREE_ELEMENTS").ColumnChanging, AddressOf EREDS_ColumnChanging
            RemoveHandler _EREDS.Tables("ELGRETIREE_ELEMENTS").RowChanging, AddressOf EREDS_RowChanging
            AddHandler _EREDS.Tables("ELGRETIREE_ELEMENTS").RowChanging, AddressOf EREDS_RowChanging

            _EREBS = New BindingSource
            _EREBS.DataSource = _EREDS.Tables("ELGRETIREE_ELEMENTS")

            RetireeHistoryDataGrid.DataSource = _EREBS
            RetireeHistoryDataGrid.SetTableStyle()
            RetireeHistoryDataGrid.Sort = If(RetireeHistoryDataGrid.LastSortedBy, RetireeHistoryDataGrid.DefaultSort)

            LoadEREDataBindings()

            ProcessControls(CType(grpEditPanel, Object))

        Catch ex As Exception

                Throw
        End Try
    End Sub

    Private Sub EREDS_RowChanged(sender As Object, e As DataRowChangeEventArgs)
        Dim BS As BindingSource
        Dim CM As CurrencyManager

        Try

            CM = CType(BindingContext(CType(sender, DataTable)), CurrencyManager)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & CM.Position.ToString & "/" & CM.Count.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & CM.Position.ToString & "/" & CM.Count.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

    Private Sub EREDS_RowChanging(sender As Object, e As DataRowChangeEventArgs)
        Dim BS As BindingSource
        Dim CM As CurrencyManager

        Try

            CM = CType(BindingContext(CType(sender, DataTable)), CurrencyManager)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & CM.Position.ToString & "/" & CM.Count.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & CM.Position.ToString & "/" & CM.Count.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

    Private Sub EREDS_ColumnChanged(sender As Object, e As DataColumnChangeEventArgs)
        Dim BS As BindingSource

        Try

            BS = DirectCast(_EREBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

    Private Sub EREDS_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs)
        Dim BS As BindingSource

        Try

            BS = DirectCast(_EREBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))


        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

    Private Sub LoadEREDataBindings()
        Dim Bind As Binding

        Try

            txtFamilyID.DataBindings.Clear()
            Bind = New Binding("Text", _EREBS, "FAMILY_ID", True)
            txtFamilyID.DataBindings.Add(Bind)

            txtFromDate.DataBindings.Clear()
            Bind = New Binding("Text", _EREBS, "FROM_DATE", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            Bind.FormatString = "MM-dd-yyyy"
            AddHandler Bind.Parse, AddressOf UFCWGeneral.DateOnlyBinding_Parse
            txtFromDate.DataBindings.Add(Bind)

            txtThruDate.DataBindings.Clear()
            Bind = New Binding("Text", _EREBS, "THRU_DATE", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            Bind.FormatString = "MM-dd-yyyy"
            AddHandler Bind.Parse, AddressOf UFCWGeneral.DateOnlyBinding_Parse
            txtThruDate.DataBindings.Add(Bind)

            cmbRetPlan.DataBindings.Clear()
            Bind = New Binding("SelectedValue", _EREBS, "RETIREE_CODE", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            cmbRetPlan.DataBindings.Add(Bind)

            _EREBS.ResumeBinding()

        Catch ex As Exception


                Throw

        Finally

        End Try

    End Sub

    Private Sub SetUIElements(ByVal readOnlyMode As Boolean)
        'this overrides the blanket changes made via ProcessFormControls

        Dim DR As DataRow

        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

        Try

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

            grpEditPanel.SuspendLayout()

            If (_EREBS IsNot Nothing AndAlso _EREBS.Position > -1) Then
                DR = CType(_EREBS.Current, DataRowView).Row
            End If

            If Not readOnlyMode Then readOnlyMode = _REGMReadOnlyAccess

            Me.CancelActionButton.Visible = Not readOnlyMode
            Me.CancelActionButton.Enabled = False

            Me.SaveActionButton.Visible = Not readOnlyMode
            Me.SaveActionButton.Enabled = False

            Me.AddActionButton.Visible = Not readOnlyMode
            Me.AddActionButton.Enabled = False

            Me.ModifyActionButton.Visible = Not readOnlyMode
            Me.ModifyActionButton.Enabled = False

            Me.DeleteActionButton.Visible = Not readOnlyMode
            Me.DeleteActionButton.Enabled = False

            Me.txtFamilyID.ReadOnly = True
            Me.txtFamilyID.TabStop = False

            Me.txtFromDate.ReadOnly = True
            Me.txtFromDate.TabStop = False

            Me.txtThruDate.ReadOnly = True
            Me.txtThruDate.TabStop = False

            Me.cmbRetPlan.ReadOnly = True
            Me.cmbRetPlan.TabStop = False

            If (_ViewHistory Is Nothing OrElse _ViewHistory) Then
                HistoryActionButton.Visible = True
                HistoryActionButton.Enabled = True
            End If

            If readOnlyMode OrElse _REGMVendorAccess OrElse _REGMReadOnlyAccess OrElse _FamilyID < 1 Then

                AddActionButton.Visible = False
                ModifyActionButton.Visible = False
                DeleteActionButton.Visible = False

                CancelActionButton.Visible = False
                SaveActionButton.Visible = False

                If Not _REGMReadOnlyAccess AndAlso Not _REGMVendorAccess AndAlso Not _ReadOnlyMode Then

                    If DR IsNot Nothing AndAlso (Not IsDBNull(DR("THRU_DATE")) AndAlso CDate(DR("THRU_DATE")) = CDate("9999-12-31")) Then
                        If DR.RowState = DataRowState.Unchanged AndAlso Not _EREDS.HasChanges Then
                            Me.ModifyActionButton.Visible = True
                            Me.ModifyActionButton.Enabled = True
                        End If
                    End If

                End If

            Else
                'modify/new has been activated

                AddActionButton.Visible = True
                ModifyActionButton.Visible = True
                DeleteActionButton.Visible = True

                CancelActionButton.Visible = True
                SaveActionButton.Visible = True

                If DR IsNot Nothing Then

                    If DR.RowState = DataRowState.Added Then 'dependents not participants

                        Me.cmbRetPlan.ReadOnly = False
                        Me.cmbRetPlan.TabStop = True

                        Me.txtFromDate.ReadOnly = False
                        Me.txtFromDate.TabStop = True

                    ElseIf DR.RowState = DataRowState.Modified AndAlso (Not IsDBNull(DR("THRU_DATE")) AndAlso CDate(DR("THRU_DATE")) = CDate("9999-12-31")) Then

                        Me.txtFromDate.ReadOnly = False
                        Me.txtFromDate.TabStop = True

                        Me.cmbRetPlan.ReadOnly = False
                        Me.cmbRetPlan.TabStop = True

                    End If
                End If

                If _EREDS.HasChanges Then
                    Me.SaveActionButton.Enabled = True
                    Me.CancelActionButton.Enabled = True
                End If

                If DR IsNot Nothing Then

                    If DR.RowState = DataRowState.Unchanged AndAlso Not _EREDS.HasChanges Then

                        Me.AddActionButton.Enabled = True

                        If Not IsDBNull(DR("THRU_DATE")) AndAlso CDate(DR("THRU_DATE")) = CDate("9999-12-31") Then
                            If _REGMRegMasterDeleteAccess Then Me.DeleteActionButton.Enabled = True
                            Me.ModifyActionButton.Enabled = True
                        End If

                    End If

                ElseIf Not _EREDS.HasChanges Then 'dont allow add if delete in progress.

                    Me.AddActionButton.Enabled = True

                End If

            End If

            If Not _REGMRegMasterDeleteAccess Then
                Me.DeleteActionButton.Visible = False
            End If

            grpEditPanel.ResumeLayout()
            grpEditPanel.Refresh()

        Catch ex As Exception

                Throw

        Finally
            Me.AutoValidate = HoldAutoValidate
        End Try

    End Sub

    Private Sub ProcessControls(ByRef controlContainer As Object)
        'targeted to local edit panel only

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ProcessControlsInContainer(CType(controlContainer, Object), _ReadOnlyMode, False)

            SetUIElements(_ReadOnlyMode)

        Catch ex As Exception

                Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub ProcessControlsInContainer(ByRef controlContainer As Object, ByVal readOnlyMode As Boolean, Optional ByVal excludeButtons As Boolean = False)

        Dim Ctrl As Control
        Dim CtrlName As String

        Try
            '            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If controlContainer Is Nothing Then Return

            Ctrl = CType(controlContainer, Control)

            If Ctrl Is Nothing Then Return

            CtrlName = Ctrl.Name.ToUpper

            For Each ChildCtrl As Object In Ctrl.Controls 'recursive to accomodate groupings

                Dim CtrlMethod As MethodInfo
                Dim CtrlProperty As PropertyInfo
                Dim result As Object
                Dim SharedCtrl As Control

                If TypeOf ChildCtrl Is UserControl Then
                    If ExtensionMethods.HasProperty(ChildCtrl, "ReadOnly") Then
                        If Not ExtensionMethods.HasMethod(ChildCtrl, "ProcessControls") Then

                            CtrlProperty = ChildCtrl.GetType().GetProperty("ReadOnly")
                            If Not CtrlProperty.CanWrite Then
                                result = CtrlProperty.GetValue(ChildCtrl)
                                If CBool(result) = True Then Continue For '
                            End If

                        End If
                    End If
                End If

                If Not (ExtensionMethods.HasProperty(ChildCtrl, NameOf(readOnlyMode)) OrElse ExtensionMethods.HasMethod(ChildCtrl, "ProcessControls")) Then
                    ProcessSubControls(ChildCtrl, readOnlyMode, excludeButtons)

                Else

                    SharedCtrl = DirectCast(ChildCtrl, Control)

                    'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Mid: " & Me.Name & " : (" & readOnlyMode.ToString & ") " & If(SharedCtrl.Parent.Name IsNot Nothing, SharedCtrl.Parent.Name & " : ", "") & SharedCtrl.Name & " : " & SharedCtrl.GetType.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

                    If ExtensionMethods.HasProperty(ChildCtrl, NameOf(readOnlyMode)) Then
                        CtrlProperty = ChildCtrl.GetType().GetProperty("ReadOnlyMode")
                        CtrlProperty.SetValue(ChildCtrl, readOnlyMode, Nothing)
                    End If

                    If ExtensionMethods.HasMethod(ChildCtrl, "ProcessControls") Then
                        CtrlMethod = ChildCtrl.GetType().GetMethod("ProcessControls")

                        Select Case CtrlMethod.GetParameters().Length
                            Case 0
                                result = CtrlMethod.Invoke(ChildCtrl, Array.Empty(Of Object))
                            Case 1
                                result = CtrlMethod.Invoke(ChildCtrl, New Object() {readOnlyMode})
                            Case 2
                                result = CtrlMethod.Invoke(ChildCtrl, New Object() {readOnlyMode, excludeButtons})
                        End Select
                    End If

                End If
            Next

        Catch ex As Exception

                Throw
        Finally
            'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Public Sub ProcessSubControls(ByRef ctrl As Object, ByVal readOnlyMode As Boolean, Optional ByVal excludeButtons As Boolean = False)

        Dim ParentCtrl As Control

        Try
            ParentCtrl = DirectCast(ctrl, Control)

            If ParentCtrl.IsDisposed Then Return

            '  Ignore the control unless it's a textbox.
            'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " : (" & readOnlyMode.ToString & ") " & If(ParentCtrl.Parent.Name IsNot Nothing, ParentCtrl.Parent.Name & " : ", "") & ParentCtrl.Name & " : " & ctrl.GetType.ToString & " : " & If(TypeOf (ctrl) Is TextBox, CType(ctrl, TextBox).ReadOnly, ParentCtrl.Enabled).ToString & " -> " & If(TypeOf (ctrl) Is TextBox, readOnlyMode, Not readOnlyMode).ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If TypeOf (ctrl) Is RadioButton OrElse TypeOf (ctrl) Is TextBox OrElse TypeOf (ctrl) Is ComboBox OrElse TypeOf (ctrl) Is DateTimePicker OrElse TypeOf (ctrl) Is Button OrElse TypeOf (ctrl) Is CheckBox OrElse TypeOf (ctrl) Is Label OrElse TypeOf (ctrl) Is DataGrid Then
                If TypeOf (ctrl) Is DataGrid Then
                ElseIf TypeOf (ctrl) Is Label Then
                    CType(ctrl, Label).Enabled = True 'remain enabled to maintain color
                ElseIf TypeOf (ctrl) Is TextBox Then
                    If CType(ctrl, TextBox).ReadOnly <> readOnlyMode Then
                        CType(ctrl, TextBox).ReadOnly = readOnlyMode
                        CType(ctrl, TextBox).TabStop = Not readOnlyMode
                    End If
                ElseIf TypeOf (ctrl) Is ExComboBox Then
                    If CType(ctrl, ExComboBox).ReadOnly <> readOnlyMode Then
                        CType(ctrl, ExComboBox).ReadOnly = readOnlyMode
                        CType(ctrl, ExComboBox).TabStop = Not readOnlyMode
                    End If
                ElseIf TypeOf (ctrl) Is ComboBox Then
                    If CType(ctrl, ComboBox).Enabled = readOnlyMode Then
                        CType(ctrl, ComboBox).Enabled = Not readOnlyMode
                    End If
                ElseIf TypeOf (ctrl) Is DateTimePicker Then
                    If CType(ctrl, DateTimePicker).Enabled = readOnlyMode Then
                        CType(ctrl, DateTimePicker).Enabled = Not readOnlyMode
                    End If
                ElseIf TypeOf (ctrl) Is Button Then
                    If Not excludeButtons Then 'Use this when Buttons should not be affected by Read Only processes
                        If CType(ctrl, Button).Enabled = readOnlyMode Then
                            CType(ctrl, Button).Enabled = Not readOnlyMode
                        End If
                    End If
                ElseIf TypeOf (ctrl) Is RadioButton Then
                    If CType(ctrl, RadioButton).Enabled = readOnlyMode Then
                        CType(ctrl, RadioButton).Enabled = Not readOnlyMode
                    End If
                ElseIf TypeOf (ctrl) Is CheckBox Then
                    If CType(ctrl, CheckBox).Enabled = readOnlyMode Then
                        CType(ctrl, CheckBox).Enabled = Not readOnlyMode
                    End If
                End If
            Else

                'continue down container chain
                For Each ChildCtrl As Object In ParentCtrl.Controls

                    'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Mid: " & Me.Name & " : (" & readOnlyMode.ToString & ") " & If(ParentCtrl.Parent.Name IsNot Nothing, ParentCtrl.Parent.Name & " : ", "") & ParentCtrl.Name & " : " & ChildCtrl.GetType.ToString & " : " & If(TypeOf (ChildCtrl) Is TextBox, CType(ChildCtrl, TextBox).ReadOnly, CType(ParentCtrl, Control).Enabled).ToString & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

                    Dim CtrlMethod As MethodInfo
                    Dim CtrlProperty As PropertyInfo
                    Dim result As Object
                    Dim SharedCtrl As Control

                    'To prevent unnecassary processing of readonly controls add this property to the control Public ReadOnly Property [ReadOnly] As Boolean = True
                    If TypeOf ChildCtrl Is UserControl Then
                        If ExtensionMethods.HasProperty(ChildCtrl, "ReadOnly") Then
                            If Not ExtensionMethods.HasMethod(ChildCtrl, "ProcessControls") Then

                                CtrlProperty = ChildCtrl.GetType().GetProperty("ReadOnly")
                                If Not CtrlProperty.CanWrite Then
                                    result = CtrlProperty.GetValue(ChildCtrl)
                                    If CBool(result) = True Then Continue For '
                                End If

                            End If
                        End If
                    End If

                    ProcessSubControls(ChildCtrl, readOnlyMode, excludeButtons)
                Next

            End If

        Catch ex As Exception

                Throw
        Finally
            'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " : (" & readOnlyMode.ToString & ") " & If(ParentCtrl.Parent.Name IsNot Nothing, ParentCtrl.Parent.Name & " : ", "") & ParentCtrl.Name & " : " & ctrl.GetType.ToString & " : " & If(TypeOf (ctrl) Is TextBox, CType(ctrl, TextBox).ReadOnly, ParentCtrl.Enabled).ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub ClearDataBindings(parentCtrl As Control)
        Try

            For Each C As Control In parentCtrl.Controls

                If C.Controls.Count > 0 Then ClearDataBindings(C) 'recursive for grouping controls

                C.DataBindings.Clear()

                If TypeOf (C) Is RadioButton OrElse TypeOf (C) Is TextBox OrElse TypeOf (C) Is ComboBox OrElse TypeOf (C) Is DateTimePicker OrElse TypeOf (C) Is CheckBox OrElse TypeOf (C) Is DataGrid Then
                    If TypeOf (C) Is DataGrid OrElse TypeOf (C) Is DataGridCustom Then
                        CType(C, DataGridCustom).CaptionText = ""
                        CType(C, DataGridCustom).DataMember = ""
                        CType(C, DataGridCustom).DataSource = Nothing
                    ElseIf TypeOf (C) Is CheckBox Then
                        CType(C, CheckBox).CheckState = CheckState.Unchecked
                    ElseIf TypeOf (C) Is ComboBox Then
                        CType(C, ComboBox).SelectedIndex = -1
                    Else
                        C.ResetText()
                    End If

                End If

            Next

        Catch ex As Exception
            Throw
        Finally

        End Try
    End Sub

    Public Sub ClearAll()

        ClearErrors()

        If RetireeHistoryDataGrid.DataSource IsNot Nothing AndAlso RetireeHistoryDataGrid.DataSource IsNot Nothing Then
            RetireeHistoryDataGrid.DataMember = ""
            RetireeHistoryDataGrid.DataSource = Nothing
        End If

        _EREBS = New BindingSource
        _TotalEREDS = New DataSet
        _EREDS = New DataSet

        ClearDataBindings(grpEditPanel)

        If _ChangedERERows IsNot Nothing Then _ChangedERERows.Dispose()
        _ChangedERERows = Nothing

        _FamilyID = -1
        cmbRetPlan.SelectedIndex = -1
        _PreviousDR = Nothing

        ProcessControls(CType(grpEditPanel, Object))

    End Sub

    Private Sub AddERELine()
        Dim DR As DataRow

        Try
            DR = _EREDS.Tables("ELGRETIREE_ELEMENTS").NewRow
            DR.BeginEdit()

            DR("FAMILY_ID") = _FamilyID
            DR("RELATION_ID") = 0
            DR("FROM_DATE") = DBNull.Value
            DR("THRU_DATE") = "12-31-9999"
            DR("RETIREE_PERCENT") = DBNull.Value
            DR("RETIREE_CODE") = DBNull.Value
            DR("STOP_CODE") = 0

            DR("CREATE_USERID") = UFCWGeneral.DomainUser.ToUpper
            DR("CREATE_DATE") = CDate(Now)
            DR("BATCH_USERID") = UFCWGeneral.DomainUser.ToUpper   '' ONLINE_USERID
            DR("BATCH_DATE") = CDate(Now)  ''ONLINE_DATE
            DR("ONLINE_USERID") = UFCWGeneral.DomainUser.ToUpper     ''CREATE_USERID
            DR("ONLINE_DATE") = CDate(Now)

            DR.EndEdit()

            _EREDS.Tables("ELGRETIREE_ELEMENTS").Rows.Add(DR)

        Catch ex As Exception


                Throw

        End Try
    End Sub

    Private Function VerifyEREChanges() As Boolean
        Dim RetireeDRs() As DataRow
        Dim DR As DataRow

        Try

            ClearErrors()

            If _EREBS.Count > 0 AndAlso _EREBS.Position > -1 Then

                DR = DirectCast(_EREBS.Current, DataRowView).Row

                ''  Finding any overlaping period is occuring
                Dim overlapping As Boolean = False
                If DR.RowState = DataRowState.Modified Then
                    If (Not IsDBNull(DR("FROM_DATE"))) AndAlso (Not IsDBNull(DR("THRU_DATE"))) Then
                        RetireeDRs = _EREDS.Tables("ELGRETIREE_ELEMENTS").Select("", "", DataViewRowState.Unchanged)
                        If RetireeDRs.Length > 0 Then
                            For Each drrow As DataRow In RetireeDRs
                                overlapping = OverlappingPeriods(CDate(DR("FROM_DATE")), CDate(DR("THRU_DATE")), CDate(drrow("FROM_DATE")), CDate(drrow("THRU_DATE")))
                                If overlapping = True Then
                                    If CDate(DR("FROM_DATE")) > CDate(drrow("FROM_DATE")) Then
                                        drrow("THRU_DATE") = CDate(DR("FROM_DATE")).AddDays(-1)
                                    Else
                                        MessageBox.Show("Overlapping of Retiree History is not valid." & Environment.NewLine & " Please correct the date before Continuing", "Overlapping", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        Return True
                                    End If
                                End If
                            Next
                        End If
                    End If
                ElseIf DR.RowState = DataRowState.Added Then
                    If (Not IsDBNull(DR("FROM_DATE"))) AndAlso (Not IsDBNull(DR("THRU_DATE"))) AndAlso CStr(DR("THRU_DATE")) <> "12/31/9999" Then
                        RetireeDRs = _EREDS.Tables("ELGRETIREE_ELEMENTS").Select("", "", DataViewRowState.Unchanged)
                        If RetireeDRs.Length > 0 Then
                            For Each drrow As DataRow In RetireeDRs
                                overlapping = OverlappingPeriods(CDate(DR("FROM_DATE")), CDate(DR("THRU_DATE")), CDate(drrow("FROM_DATE")), CDate(drrow("THRU_DATE")))
                                If overlapping = True Then
                                    If CDate(DR("FROM_DATE")) > CDate(drrow("FROM_DATE")) Then
                                        drrow("THRU_DATE") = CDate(DR("FROM_DATE")).AddDays(-1)
                                    Else
                                        MessageBox.Show("Overlapping of Retiree History is not valid." & Environment.NewLine & " Please correct the date before Continuing", "Overlapping", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        Return True
                                    End If
                                End If
                            Next
                        End If
                    End If
                End If

                ''In the dataset from date is > thru date give msg.
                If DR.RowState = DataRowState.Modified Then
                    If (Not IsDBNull(DR("FROM_DATE"))) AndAlso (Not IsDBNull(DR("THRU_DATE"))) Then
                        RetireeDRs = _EREDS.Tables("ELGRETIREE_ELEMENTS").Select("", "", DataViewRowState.CurrentRows)
                        If RetireeDRs.Length > 1 Then
                            For Each drrow As DataRow In RetireeDRs
                                If CDate(drrow("FROM_DATE")) > CDate(drrow("THRU_DATE")) Then
                                    MessageBox.Show("From date of one record is later than Thru date and is Invalid." & Environment.NewLine & " Please correct the date before Continuing", "From Date", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    Return True
                                End If
                            Next
                        End If
                    End If
                End If

                If ErrorProvider1.GetErrorCount > 0 Then
                    Return True
                End If

            End If

        Catch ex As Exception


                Throw
        End Try

    End Function

    Public Function ApplyEREChanges() As Boolean

        'Latest Thru date can only be 9999-12-31

        'Add: If Prior row exists its thru date is terminated with Last Day of Month Prior to row being added From date
        ' Monthly Detail rows now covered are adjusted to use RET Plan of added row

        'Modify: Only can occur to From date item of 9999-12-31, From Date or RetPlan
        ' Prior row if it exists is adjusted to to have its Last Day of Month Prior to row being modified From date
        '  Modification could move From date up or down within limits of prior rows range (modification cannot remove a row)
        ' Monthly Detail rows now covered are adjusted to use RET Plan of modified row

        'Delete: Only can occur to From date item of 9999-12-31
        ' Prior row if it exists is adjusted to to have 9999-12-31 end date
        ' Monthly Detail rows that were covered by deleted row period are adjusted to use prior RET Plan if once exists

        Dim HistSum As String = ""
        Dim HistDetail As String = ""
        Dim ActivityTimestamp As DateTime = DateTime.Now
        Dim Transaction As DbTransaction

        Try
            _ChangedERERows = _EREDS.GetChanges()

            If _ChangedERERows Is Nothing Then Return True

            Transaction = RegMasterDAL.BeginTransaction

            For Each DR As DataRow In _ChangedERERows.Tables("ELGRETIREE_ELEMENTS").Rows

                HistSum = ""
                HistDetail = ""

                HistDetail = DataGridCustom.IdentifyChanges(DR, RetireeHistoryDataGrid)

                If DR.RowState = DataRowState.Modified Then 'Could be both Current and Previous rows if current from date is modified.

                    If HistDetail.Length > 0 Then
                        'new
                        HistSum = " RETIREE HISTORY FOR FAMILYID: " & Me.FamilyID & " WAS MODIFIED"
                        HistDetail = UFCWGeneral.DomainUser.ToUpper & Environment.NewLine & " MODIFICATIONS WERE: " & Environment.NewLine & HistDetail

                        RegMasterDAL.UpdateEligRetireeElements(CInt(DR("FAMILY_ID")), CDate(DR("FROM_DATE")), CDate(DR("THRU_DATE")), CStr(DR("RETIREE_PLAN")), CDec(DR("RETIREE_PERCENT")), CDate(DR("FROM_DATE", DataRowVersion.Original)), ActivityTimestamp, CDate(DR("ONLINE_DATE", DataRowVersion.Original)), Transaction)
                        RegMasterDAL.CreateRegHistory(Me.FamilyID, 0, Nothing, Nothing, "ERERETUPDATE", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, Transaction)

                    End If

                ElseIf DR.RowState = DataRowState.Deleted Then 'Can only be the top most aka Current Row

                    Dim OriginalCurrentQuery =
                        From ExistingCoverage In CType(_EREBS.DataSource, DataTable).AsEnumerable
                        Where ExistingCoverage.RowState <> DataRowState.Deleted
                        Order By ExistingCoverage.Field(Of Date)("THRU_DATE") Descending

                    Dim CurrentDR As DataRow = OriginalCurrentQuery.FirstOrDefault

                    HistSum = "RETIREE HISTORY RECORD WAS DELETED"
                    HistDetail = UFCWGeneral.DomainUser.ToUpper & Environment.NewLine & " DELETION WAS: " & Environment.NewLine & HistDetail & If(CurrentDR IsNot Nothing, Environment.NewLine & "Monthly Detail updated to RET_PLAN " & CStr(CurrentDR("RETIREE_CODE")), "")

                    RegMasterDAL.DeleteEligRetireeElements(CInt(DR("FAMILY_ID", DataRowVersion.Original)), CDate(DR("FROM_DATE", DataRowVersion.Original)), CDate(DR("THRU_DATE", DataRowVersion.Original)), ActivityTimestamp, CDate(DR("ONLINE_DATE", DataRowVersion.Original)), Transaction)
                    RegMasterDAL.CreateRegHistory(CInt(DR("FAMILY_ID", DataRowVersion.Original)), CInt(DR("RELATION_ID", DataRowVersion.Original)), Nothing, Nothing, "ERERETDELETE", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, Transaction)

                ElseIf DR.RowState = DataRowState.Added Then

                    HistSum = "RETIREE HISTORY  FOR FAMILYID: " & Me.FamilyID & " WAS ADDED"
                    HistDetail = UFCWGeneral.DomainUser.ToUpper & " ADDED THE ELIG RETIREE HISTORY RECORD " & Environment.NewLine & HistDetail

                    RegMasterDAL.AddEligRetireeElements(CInt(DR("FAMILY_ID")), CDate(DR("FROM_DATE")), CDate(DR("THRU_DATE")), CStr(DR("RETIREE_PLAN")), CDec(DR("RETIREE_PERCENT")), ActivityTimestamp, Transaction)
                    RegMasterDAL.CreateRegHistory(Me.FamilyID, 0, Nothing, Nothing, "ERERETADD", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, Transaction)

                End If
            Next

            RegMasterDAL.CommitTransaction(Transaction)

            Dim EREQuery = (From ERE As DataRow In _EREDS.Tables("ELGRETIREE_ELEMENTS").AsEnumerable()
                            Where ERE.RowState = DataRowState.Modified OrElse ERE.RowState = DataRowState.Added)

            For Each DR As DataRow In EREQuery
                DR("ONLINE_DATE") = ActivityTimestamp
                DR("ONLINE_USERID") = UFCWGeneral.DomainUser.ToUpper
            Next

            _EREDS.AcceptChanges()

            Return True

        Catch db2ex As DB2Exception

            Try
                If Transaction IsNot Nothing Then
                    RegMasterDAL.RollbackTransaction(Transaction)
                End If
            Catch ex2 As Exception
            End Try

            Select Case db2ex.Number
                Case -438, -1822
                    MessageBox.Show(db2ex.Message.Replace("SQL0438N: Application raised error with diagnostic text: ", "") & vbCrLf & "The item(s) you are attempting to update has been changed." &
                                   vbCrLf & "Refresh the Participant data.", "Save rejected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Case Else
                    Dim rethrow As Boolean = ExceptionPolicy.HandleException(db2ex, "General")
                    If (rethrow) Then
                        Throw
                    End If
            End Select

        Catch ex As Exception

            Try
                If Transaction IsNot Nothing Then
                    RegMasterDAL.RollbackTransaction(Transaction)
                End If
            Catch ex2 As Exception
            End Try


                Throw
        Finally
            If Transaction IsNot Nothing Then Transaction.Dispose()
            Transaction = Nothing

        End Try
    End Function

    Public Sub ClearErrors()
        ErrorProvider1.Clear()
    End Sub

    Private Function UnCommittedChangesExist() As Boolean

        Dim Modifications As String = ""
        Try

            If _EREDS Is Nothing Then Return False

            _ChangedERERows = _EREDS.GetChanges()

            If RetireeHistoryDataGrid IsNot Nothing AndAlso _ChangedERERows IsNot Nothing AndAlso _ChangedERERows.Tables("ELGRETIREE_ELEMENTS").Rows.Count > 0 Then

                For Each DR As DataRow In _ChangedERERows.Tables("ELGRETIREE_ELEMENTS").Rows
                    If DR.RowState <> DataRowState.Added Then
                        'attempt to exclude rows accidently changed during navigation operations
                        Modifications = DataGridCustom.IdentifyChanges(DR, RetireeHistoryDataGrid)

                        If Modifications IsNot Nothing AndAlso Modifications.Length > 0 Then
                            Return True
                        End If

                    ElseIf DR.RowState = DataRowState.Added Then
                        Return True
                    End If
                Next

            End If

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Sub LoadRetireeTypes(Optional ByVal filter As String = "")

        Dim DT As DataTable
        Dim strfilter As String = ""
        Dim FromDate As Date

        Try

            If _RetPlanDT Is Nothing OrElse _RetPlanDT.Rows.Count < 1 Then
                _RetPlanDT = RegMasterDAL.RetrieveRetireePercentValues
            End If
            DT = _RetPlanDT

            cmbRetPlan.DataSource = Nothing
            cmbRetPlan.Items.Clear()

            If txtFromDate.Text.Length > 0 Then
                FromDate = CDate(txtFromDate.Text)
                strfilter = "'" & FromDate & "' >= FROM_DATE AND '" & FromDate & "' <= THRU_DATE "
            Else
                strfilter = ""
            End If

            Dim DV As New DataView(DT, strfilter, "RETIREE_CODE", DataViewRowState.CurrentRows)

            cmbRetPlan.ValueMember = "RETIREE_CODE"
            cmbRetPlan.DisplayMember = "RETIREE_CODE"
            cmbRetPlan.DataSource = DV
            cmbRetPlan.SelectedIndex = -1

        Catch ex As Exception

                Throw

        Finally

        End Try
    End Sub

    Private Shared Function OverlappingPeriods(ByVal period1_start As Date, ByVal period1_end As Date, ByVal period2_start As Date, ByVal period2_end As Date) As Boolean

        If Date.Compare(period1_start, period2_end) <= 0 And Date.Compare(period1_end, period2_start) >= 0 Then
            Return True
        End If
        Return False

    End Function

    Private Function DeleteRetireeHistoryRecord(ByVal DR As DataRow, ByRef Transaction As DbTransaction) As Boolean
        Dim HistSum As String = ""
        Dim HistDetail As String = ""
        Try
            HistSum = "RETIREE HISTORY RECORD WAS DELETED"
            HistDetail = UFCWGeneral.DomainUser.ToUpper & " DELETED THE RETIREE HISTORY RECORD HAVING : FROM DATE  '" & CStr(DR("FROM_DATE")) & "' THRU DATE '" & CStr(DR("THRU_DATE")) & "', RETIREE PLAN " & CStr(DR("RETIREE_PLAN")) & " AND RETIREE PERCENT IS '" & CStr(DR("RETIREE_PERCENT")) & "'"

            RegMasterDAL.DeleteEligRetireElements(CInt(DR("FAMILY_ID")), CDate(DR("FROM_DATE")), CDate(DR("THRU_DATE")), CStr(If(_PreviousDR Is Nothing, "", _PreviousDR("RETIREE_PLAN"))), CStr(If(_PreviousDR Is Nothing, "", _PreviousDR("RETIREE_PERCENT"))), Transaction)
            RegMasterDAL.CreateRegHistory(CInt(DR("FAMILY_ID")), CInt(DR("RELATION_ID")), Nothing, Nothing, "RETIREEDELETE", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, Transaction)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
                Return False
            End If
        End Try
        Return True
    End Function

    Private Sub FixRetireefromthrudates(ByVal dr As DataRow)
        ''no overlaps
        Dim thrudate = New List(Of Date)
        Dim nextkey As Integer = 0
        Dim previousfromdate As Date = Nothing
        Dim previouskey As Integer = 0 : Dim previousthrudate As Date
        Dim currentkey As Integer = 0
        Dim thrudatedict As New Dictionary(Of Integer, Date)
        Dim key As Integer = 0
        _PreviousDR = Nothing

        Dim drallthrudate() As DataRow = _EREDS.Tables("ELGRETIREE_ELEMENTS").Select("", "THRU_DATE", DataViewRowState.CurrentRows)
        If drallthrudate.Length > 1 Then
            ''   Dim allthrudate = drallthrudate.AsEnumerable
            thrudate = (From r In drallthrudate Order By r("THRU_DATE") Select r.Field(Of Date)("THRU_DATE")).ToList

            For Each r In thrudate
                thrudatedict.Add(key, CDate(thrudate(key)))
                If CDate(thrudate(key)) = CDate(dr("THRU_DATE")) Then
                    currentkey = key
                End If
                key += 1
            Next

            nextkey = currentkey + 1
            previouskey = currentkey - 1

            '' PREVIOUS ROW VALUES
            If thrudatedict.ContainsKey(previouskey) Then
                previousthrudate = thrudatedict(previouskey)
            End If

            Dim PreviousDRs() As DataRow = _EREDS.Tables("ELGRETIREE_ELEMENTS").Select("FROM_DATE <> THRU_DATE AND THRU_DATE = '" & CStr(previousthrudate) & "'", "THRU_DATE", DataViewRowState.CurrentRows)
            If PreviousDRs.Any Then
                PreviousDRs(0)("THRU_DATE") = CDate(dr("THRU_DATE"))
                _PreviousDR = PreviousDRs(0)
            End If

        End If
    End Sub

#End Region

#Region "Formatting"

    Private Sub DateOnlyBinding_BindingComplete(ByVal sender As Object, ByVal e As BindingCompleteEventArgs)
        Try
            ErrorProvider1.SetError(CType(e.Binding.BindableComponent, Control), "")

            If e.BindingCompleteState <> BindingCompleteState.Success Then
                ErrorProvider1.SetError(CType(e.Binding.BindableComponent, Control), "Date format invalid. Use mmddyy or mmddyyyy")
            End If

        Catch ex As Exception


                Throw

        End Try
    End Sub

    Private Sub BindingCompleteEventHandler(ByVal sender As Object, ByVal e As System.Windows.Forms.BindingCompleteEventArgs)

        Try

            If Not e.BindingCompleteState = BindingCompleteState.Success Then
                MessageBox.Show("Control " & e.Binding.Control.Name & " " & e.ErrorText, "Problem converting data to database format", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception

                Throw
        End Try

    End Sub

    Private Sub cmbRetPlan_DropDown(sender As Object, e As EventArgs) Handles cmbRetPlan.DropDown

        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ErrorProvider1.ClearError(CBox)

        Catch ex As Exception

                Throw
        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

    Private Sub txtFromDate_Validating(sender As Object, e As CancelEventArgs) Handles txtFromDate.Validating

        Dim HoldDate As Date?
        Dim TBox As TextBox = CType(sender, TextBox)
        Dim DR As DataRow

        Try

            If _Disposed OrElse _EREBS Is Nothing OrElse _EREBS.Position < 0 OrElse TBox.ReadOnly Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ErrorProvider1.ClearError(TBox)

            If TBox.Text.Trim.Length < 1 Then
                SetErrorWithTracking(ErrorProvider1, TBox, "From Date is required.")
            Else
                HoldDate = UFCWGeneral.ValidateDate(TBox.Text)
                If HoldDate Is Nothing Then

                    SetErrorWithTracking(ErrorProvider1, TBox, "Date format must be mm/dd/yyyy or mm-dd-yyyy.")

                Else

                    If CDate(HoldDate).Day <> 1 Then
                        HoldDate = CDate(CDate(HoldDate).Month.ToString & "-01-" & CDate(HoldDate).Year.ToString)
                        SetErrorWithTracking(ErrorProvider1, TBox, "From Date adjusted to use 1st day of month.")
                    End If

                    Dim CurrentDR As DataRow 'row being expired, changed, etc
                    Dim PriorDR As DataRow 'row possiblly modified to reflect new THRU_DATE

                    Dim CurrentQuery =
                                    From ExistingERE In _EREDS.Tables("ELGRETIREE_ELEMENTS").AsEnumerable()
                                    Where ExistingERE.RowState <> DataRowState.Deleted AndAlso ExistingERE.RowState <> DataRowState.Added
                                    Order By ExistingERE.Field(Of Date)("THRU_DATE") Descending

                    For Each DR In CurrentQuery

                        If CurrentDR IsNot Nothing AndAlso PriorDR Is Nothing Then
                            PriorDR = DR
                            Exit For
                        End If

                        If CurrentDR Is Nothing Then
                            CurrentDR = DR
                        End If
                    Next

                    If PriorDR IsNot Nothing Then
                        If CDate(HoldDate) < CDate(CurrentDR("FROM_DATE")) Then
                            ErrorProvider1.SetErrorWithTracking(TBox, " From Date would delete current active row. Use Delete function.")
                        End If
                    End If

                    Dim OverlapQuery = From ERE As DataRow In _EREDS.Tables("ELGRETIREE_ELEMENTS").AsEnumerable() 'no need to test for changed items as this is family cache
                                       Where ERE.RowState = DataRowState.Unchanged _
                                       AndAlso ERE.Field(Of Date?)("FROM_DATE", DataRowVersion.Original) >= HoldDate _
                                       AndAlso ERE.Field(Of Date?)("THRU_DATE", DataRowVersion.Original) = CDate("9999-12-31")

                    If OverlapQuery.Any Then
                        SetErrorWithTracking(ErrorProvider1, TBox, "From Date overlaps with existing row.")
                    End If

                    If TBox.Text <> CDate(HoldDate).ToString("MM-dd-yyyy") Then
                        TBox.Text = CDate(HoldDate).ToString("MM-dd-yyyy")
                    End If

                End If
            End If

            If ErrorProvider1.GetError(TBox).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
        End Try

    End Sub

    Private Sub cmbRetPlan_Validating(sender As Object, e As CancelEventArgs) Handles cmbRetPlan.Validating

        Dim CBox As ExComboBox = CType(sender, ExComboBox)
        Dim DR As DataRow

        Try

            ErrorProvider1.ClearError(CBox)

            If _EREBS Is Nothing OrElse _EREBS.Position < 0 OrElse _EREBS.Count < 1 OrElse CBox.ReadOnly Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If CBox.SelectedIndex < 0 Then
                ErrorProvider1.SetErrorWithTracking(CBox, " Retirement Plan selection required.")
            End If

            If ErrorProvider1.GetError(CBox).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
            Else
                DR = DirectCast(_EREBS.Current, DataRowView).Row

                If IsDBNull(DR("RETIREE_CODE")) OrElse (DR("RETIREE_CODE").ToString <> CBox.SelectedValue.ToString) Then
                    _EREBS.RaiseListChangedEvents = False
                    DR("RETIREE_PLAN") = CBox.SelectedValue.ToString.Substring(0, 1)
                    DR("RETIREE_PERCENT") = CBox.SelectedValue.ToString.Substring(1, 1)
                    _EREBS.RaiseListChangedEvents = True
                End If
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally

        End Try

    End Sub

    Private Sub cmbRetPlan_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbRetPlan.Validated

        Dim DR As DataRow
        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Try

            If _EREBS Is Nothing OrElse _EREBS.Position < 0 OrElse _EREBS.Count < 1 OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = DirectCast(_EREBS.Current, DataRowView).Row
            DR.EndEdit() 'needed to refresh grid via databinding

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Sub

    Private Sub cmbRetPlan_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbRetPlan.SelectedIndexChanged

        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Dim BS As BindingSource

        Try

            If _EREBS Is Nothing OrElse _EREBS.Position < 0 OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

            BS = DirectCast(_EREBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " IO:  " & Me.Name & ":" & CBox.Name & " BS(" & BS.Position.ToString & ") Val(" & CBox.SelectedValue.ToString & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))


        Catch ex As Exception

                Throw
        Finally
        End Try

    End Sub

    Private Sub cmbRetPlan_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbRetPlan.SelectionChangeCommitted
        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Dim DR As DataRow
        Dim BS As BindingSource

        Try

            If _EREBS Is Nothing OrElse _EREBS.Position < 0 OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

            BS = DirectCast(_EREBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & CBox.Name & " BS(" & BS.Position.ToString & ") Val(" & CBox.Text & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = DirectCast(_EREBS.Current, DataRowView).Row

            CType(CBox.Parent, TransparentContainer).ValidateChildren() 'this will trigger validation of the cmbbox triggering write of value to DS

            'do not compare DR to control at this point as control values have been commited
            'If IsDBNull(DR("RETIREE_CODE")) OrElse (DR("RETIREE_CODE").ToString <> CBox.SelectedValue.ToString) Then
            '    _EREBS.RaiseListChangedEvents = False
            '    DR("RETIREE_PLAN") = CBox.SelectedValue.ToString.Substring(0, 1)
            '    DR("RETIREE_PERCENT") = CBox.SelectedValue.ToString.Substring(1, 1)
            '    _EREBS.RaiseListChangedEvents = True
            'End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: (" & Me.Name & ":" & CBox.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
        End Try

    End Sub


    Private Sub EREBS_CurrentChanged(sender As Object, e As EventArgs) Handles _EREBS.CurrentChanged

        Dim BS As BindingSource

        Try

            BS = DirectCast(sender, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If BS Is Nothing OrElse BS.Position < 0 OrElse BS.Count < 1 Then Return 'no current row, most likely an item filter value was changed

            Dim DR As DataRow = DirectCast(BS.Current, DataRowView).Row

            ProcessControls(CType(grpEditPanel, Object))

        Catch ex As Exception
            Throw
        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

    Private Sub EREBS_CurrentItemChanged(sender As Object, e As EventArgs) Handles _EREBS.CurrentItemChanged

        Dim BS As BindingSource

        Try

            BS = DirectCast(sender, BindingSource)

            If BS Is Nothing OrElse BS.Position < 0 OrElse BS.Count < 1 Then Return 'no current row, most likely an item filter value was changed

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Dim DR As DataRow = DirectCast(BS.Current, DataRowView).Row

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally

        End Try
    End Sub

    Private Sub EREBS_PositionChanged(sender As Object, e As EventArgs) Handles _EREBS.PositionChanged

        Dim BS As BindingSource

        Try

            BS = DirectCast(sender, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If BS Is Nothing OrElse BS.Position < 0 OrElse BS.Count < 1 Then Return 'no current row, most likely an item filter value was changed

            Dim DR As DataRow = DirectCast(BS.Current, DataRowView).Row

        Catch ex As Exception
            Throw
        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

    Private Sub EREBS_ListChanged(sender As Object, e As ListChangedEventArgs) Handles _EREBS.ListChanged

        Dim BS As BindingSource

        Try
            BS = CType(sender, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") RS(" & If(BS.Position < 0 ORELSE BS.Current Is Nothing, "N/A", DirectCast(BS.Current, System.Data.DataRowView).Row.RowState.ToString) & ") O(" & e.OldIndex.ToString & ") N(" & e.NewIndex.ToString & ") CT(" & e.ListChangedType.ToString & ") SEL(" & If(BS Is Nothing OrElse BS.Count < 1 OrElse BS.Position < 0 OrElse RetireeHistoryDataGrid.DataSource Is Nothing, "N/A", RetireeHistoryDataGrid.IsSelected(BS.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Select Case e.ListChangedType
                Case ListChangedType.ItemDeleted 'account for rows deleted due to a cancel action

                Case ListChangedType.ItemMoved

                    Select Case DirectCast(BS.Current, System.Data.DataRowView).Row.RowState
                        Case DataRowState.Modified

                        Case DataRowState.Added

                            If e.OldIndex <> e.NewIndex OrElse (BS.Position > -1 AndAlso BS.Count = 1) Then
                                RetireeHistoryDataGrid.Select(BS.Position)
                            End If

                    End Select

                Case ListChangedType.ItemChanged

                    If BS.Count = 0 Then 'item was changed in some way that excludes it from list due to filter exclusion
                        Return
                    End If

                    Select Case DirectCast(BS.Current, System.Data.DataRowView).Row.RowState
                        Case DataRowState.Modified


                        Case DataRowState.Added

                    End Select

                Case ListChangedType.Reset 'triggered by sorts or changes in grid filter

                    If BS.Position > -1 AndAlso BS.Position <> e.NewIndex Then
                        If e.NewIndex > -1 Then BS.Position = e.NewIndex
                        BS.ResetCurrentItem()
                    End If

                Case ListChangedType.ItemAdded 'includes items reincluded when filters change

                    If BS.Position <> e.NewIndex OrElse (BS.Position > -1 AndAlso BS.Count = 1) Then 'first item added
                        If e.NewIndex > -1 Then BS.Position = e.NewIndex
                        If e.NewIndex > -1 Then RetireeHistoryDataGrid.Select(e.NewIndex)
                    End If

            End Select

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") RS(" & If(BS.Position < 0 ORELSE BS.Current Is Nothing, "N/A", DirectCast(BS.Current, System.Data.DataRowView).Row.RowState.ToString) & ") O(" & e.OldIndex.ToString & ") N(" & e.NewIndex.ToString & ") CT(" & e.ListChangedType.ToString & ") SEL(" & If(BS Is Nothing OrElse BS.Count < 1 OrElse BS.Position < 0 OrElse RetireeHistoryDataGrid.DataSource Is Nothing, "N/A", RetireeHistoryDataGrid.IsSelected(BS.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub txtFromDate_Validated(sender As Object, e As EventArgs) Handles txtFromDate.Validated

        Dim HoldDate As Date?
        Dim TBox As TextBox = CType(sender, TextBox)
        Dim DR As DataRow

        Try

            If _Disposed OrElse _EREBS Is Nothing OrElse _EREBS.Position < 0 OrElse TBox.ReadOnly Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            AdjustCurrentAndPriorDates()

            _EREBS.EndEdit() 'trigger CurrentItemChanged 

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
        End Try

    End Sub

    Private Sub EligRetirementElementsControl_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged

        Try
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ProcessControls(CType(SplitContainer1, Object)) ' disable UI Input elements until a row is added

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub txtThruDate_Validating(sender As Object, e As CancelEventArgs) Handles txtThruDate.Validating

        Dim HoldDate As Date?
        Dim TBox As TextBox = CType(sender, TextBox)
        Dim DR As DataRow

        Try

            If _Disposed OrElse _EREBS Is Nothing OrElse _EREBS.Position < 0 OrElse TBox.ReadOnly Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ErrorProvider1.ClearError(TBox)

            If TBox.Text.Trim.Length < 1 Then
                SetErrorWithTracking(ErrorProvider1, TBox, "Thru Date is required.")
            Else
                HoldDate = UFCWGeneral.ValidateDate(TBox.Text)
                If HoldDate Is Nothing Then

                    SetErrorWithTracking(ErrorProvider1, TBox, "Date format must be mm/dd/yyyy or mm-dd-yyyy.")

                Else

                    If IsDate(txtFromDate.Text) AndAlso CDate(txtFromDate.Text) > CDate(HoldDate) Then
                        SetErrorWithTracking(ErrorProvider1, TBox, "Thru Date is earlier than From date and invalid.")
                    End If


                    If TBox.Text <> CDate(HoldDate).ToString("MM-dd-yyyy") Then
                        TBox.Text = CDate(HoldDate).ToString("MM-dd-yyyy")
                    End If

                End If
            End If

            If ErrorProvider1.GetError(TBox).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
        End Try

    End Sub

    Private Sub HistoryActionButton_Click(sender As Object, e As EventArgs) Handles HistoryActionButton.Click
        Dim FRM As RegMasterHistoryForm

        Try

            FRM = New RegMasterHistoryForm
            FRM.FamilyID = _FamilyID
            FRM.RelationID = 0
            FRM.Mode = REGMasterHistoryMode.ERE
            FRM.ShowDialog()

            FRM.Close()

        Catch ex As Exception
            Throw
        Finally
            If FRM IsNot Nothing Then FRM.Dispose()
            FRM = Nothing
        End Try

    End Sub

#End Region

End Class
