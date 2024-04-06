Option Strict On
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Windows.Forms
Imports System.ComponentModel
Imports DDTek.DB2
Imports System.Data.Common
Imports System.Reflection

Public Class EligAcctHrsMaintForm

#Region "Variables"

    Private _FamilyID As Integer
    Private _RelationID As Integer
    Private _APPKEY As String = "UFCW\RegMaster\"
    Private _ReadOnlyMode As Boolean = True

    Private _ReturnDialogStatus As DialogResult = DialogResult.None
    Private _ViewHistory As Boolean?
    Private _ChangedDRs As DataSet

    Private _EligSpecialAcctValuesDT As DataTable
    Private _EligSpecialAcctValuesBS As BindingSource

    Private _EligRetireeElementsDS As DataSet
    Private _RetrieveA2CountDS As DataSet
    Private _SpecialUserEntryAcctDT As DataTable

    Private _SelectedEligPeriod As Date?
    Private _TotalSADS As New EligAcctHoursDS
    Private _SelectedMonthHoursDT As New DataTable

    Private WithEvents _EAHBS As BindingSource

    Private _HoverCell As New DataGridCell

    Private _RecCntHMOAdj As Integer       '' used to display msg after adding record
    Private _AllEligMonthHoursDS As DataSet  '' all the rows from elig account hrs table
    Private _EligibilityCalculate As Boolean = False

    Private _InEligibleforSpecialAccountsDT As DataTable
    Private _EligMetalDescriptionsDT As DataTable
    Private _Memtype As String = ""    '' used for  validation

    ReadOnly _REGMEmployeeAccess As Boolean = UFCWGeneralAD.REGMEmployeeAccess
    ReadOnly _REGMReadOnlyAccess As Boolean = UFCWGeneralAD.REGMReadOnlyAccess
    ReadOnly _REGMVendorAccess As Boolean = UFCWGeneralAD.REGMVendorAccess
    ReadOnly _REGMSupervisor As Boolean = UFCWGeneralAD.REGMSupervisor

#End Region

    Private _Disposed As Boolean = False
    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.
            '
            If _EligSpecialAcctValuesDT IsNot Nothing Then
                _EligSpecialAcctValuesDT.Dispose()
            End If
            _EligSpecialAcctValuesDT = Nothing

            If _SpecialUserEntryAcctDT IsNot Nothing Then
                _SpecialUserEntryAcctDT.Dispose()
            End If
            _SpecialUserEntryAcctDT = Nothing

            If _InEligibleforSpecialAccountsDT IsNot Nothing Then
                _InEligibleforSpecialAccountsDT.Dispose()
            End If
            _InEligibleforSpecialAccountsDT = Nothing

            If _EligMetalDescriptionsDT IsNot Nothing Then
                _EligMetalDescriptionsDT.Dispose()
            End If
            _EligMetalDescriptionsDT = Nothing

            If _AllEligMonthHoursDS IsNot Nothing Then
                _AllEligMonthHoursDS.Dispose()
            End If
            _AllEligMonthHoursDS = Nothing

            If _TotalSADS IsNot Nothing Then
                _TotalSADS.Dispose()
            End If
            _TotalSADS = Nothing

            If _EAHDS IsNot Nothing Then
                _EAHDS.Dispose()
            End If
            _EAHDS = Nothing

            If _EAHBS IsNot Nothing Then
                _EAHBS.Dispose()
            End If
            _EAHBS = Nothing

            If _SelectedMonthHoursDT IsNot Nothing Then
                _SelectedMonthHoursDT.Dispose()
            End If
            _SelectedMonthHoursDT = Nothing

            If EligSpecialAcctDataGrid IsNot Nothing Then
                EligSpecialAcctDataGrid.Dispose()
            End If
            EligSpecialAcctDataGrid = Nothing


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

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the FamilyID of the Document.")>
    Public Property FamilyID() As Integer
        Get
            Return _FamilyID
        End Get
        Set(ByVal Value As Integer)
            _FamilyID = Value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the RelationID of the Document.")>
    Public Property RelationID() As Integer
        Get
            Return _RelationID
        End Get
        Set(ByVal Value As Integer)
            _RelationID = Value
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
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("History Button is visible")>
    Public Property VisibleHistory() As Boolean
        Get
            Return If(_ViewHistory Is Nothing, False, CBool(_ViewHistory))
        End Get
        Set(ByVal value As Boolean)
            _ViewHistory = value
        End Set
    End Property

    Public Property EligHoursDataSet() As DataSet
        Get
            Return _AllEligMonthHoursDS
        End Get
        Set(ByVal value As DataSet)
            _AllEligMonthHoursDS = value
        End Set
    End Property
    Public Property EligibiltyCalculated() As Boolean
        Get
            Return _EligibilityCalculate
        End Get
        Set(ByVal Value As Boolean)
            _EligibilityCalculate = Value
        End Set
    End Property
    Public Property Memtype() As String
        Get
            Return _Memtype
        End Get
        Set(ByVal Value As String)

            _Memtype = Value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Indicates that Save has not been used.")>
    Public ReadOnly Property ChangesPending() As Boolean
        Get
            Return UnCommittedChangesExist()
        End Get
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
            LoadSpecialAccounts()
            LoadIneligibleforSpecialAccounts()
            LoadEligMetalDescriptions()
        End If

        'dont want to display the default table style
    End Sub

    Public Sub New(ByVal familyID As Integer, ByVal readonlymode As Boolean, ByRef eligPeriod As Date?)

        Me.New()

        _FamilyID = familyID
        _ReadOnlyMode = readonlymode

        If IsDate(eligPeriod) Then
            _SelectedEligPeriod = eligPeriod
        Else
            _SelectedEligPeriod = RegMasterDAL.GlobalEligPeriod
        End If

        ProcessControls(Me, True, True)

    End Sub

#End Region

#Region "Form\Button Events"

    Private Sub AddActionButton_Click(sender As System.Object, e As System.EventArgs) Handles AddActionButton.Click

        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

        Try

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            AddActionButton.Enabled = False

            AddSpecialAcct()

        Catch ex As Exception


                Throw

        Finally
            Me.AutoValidate = HoldAutoValidate

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub ModifyActionButton_Click(sender As System.Object, e As System.EventArgs) Handles ModifyActionButton.Click

        If _EAHBS.Position < 0 OrElse _EAHBS.Position < 0 OrElse _EAHBS.Current Is Nothing Then Return

        Dim DR As DataRow

        Try

            ModifyActionButton.Enabled = False
            AddActionButton.Enabled = False
            DeleteActionButton.Enabled = False

            DR = DirectCast(_EAHBS.Current, DataRowView).Row
            DR.BeginEdit()
            DR.EndEdit() 'sets modified status.

        Catch ex As Exception

                Throw
        Finally
            DR = Nothing
        End Try

    End Sub

    Private Sub CancelActionButton_Click(sender As System.Object, e As System.EventArgs) Handles CancelActionButton.Click
        Dim Result As DialogResult = DialogResult.None

        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

        Try

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

            CancelActionButton.Enabled = False
            SaveActionButton.Enabled = False

            _ChangedDRs = CType(_EAHDS.GetChanges(), EligAcctHoursDS)

            If _ChangedDRs IsNot Nothing Then
                Result = MessageBox.Show(Me, "Do you want to Cancel the changes?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            End If

            If Result = DialogResult.Yes Then

                _EAHBS.CancelEdit()
                _EAHDS.Tables("ELIG_ACCT_HOURS").RejectChanges()

                ClearErrors()

                _EAHBS.ResetBindings(False)

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

    Private Sub DeleteActionButton_Click(sender As System.Object, e As System.EventArgs) Handles DeleteActionButton.Click

        If _EAHBS.Position < 0 OrElse _EAHBS.Position < 0 OrElse _EAHBS.Current Is Nothing Then Return

        Dim DR As DataRow

        Try

            DeleteActionButton.Enabled = False

            DR = CType(_EAHBS.Current, DataRowView).Row

            '' Only User Entered Special accounts can be deleted.

            Try
                If MessageBox.Show(Me, "Are you sure you want to Delete" & Environment.NewLine & "Eligibility Special Account Hours record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                    If DeleteSpecialAccount(DR) Then

                        MessageBox.Show("Eligibility Special Account Hours Deleted Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        If _SelectedEligPeriod <= RegMasterDAL.GlobalEligPeriod Then
                            '' Message to user to calculate eligiblity for current an retro periods
                            MessageBox.Show("Change Eligibility calculation is required. ", "Calculate Eligibility", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If

                        _EAHBS.ResetBindings(False)

                        _ReturnDialogStatus = DialogResult.Yes

                    Else
                        MessageBox.Show("Error while Deleting Eligibility Special Account Hours." & vbCrLf & "Please Refresh and try again ", "Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End If

            Catch ex As Exception

                Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
                If (rethrow) Then
                    Throw
                End If

            End Try

        Catch ex As Exception

            CancelActionButton.Enabled = True ' this is done in the error handler just in case a failure occurs


                Throw

        Finally

            If DR IsNot Nothing Then DR = Nothing

        End Try

    End Sub

    Private Sub SaveActionButton_Click(sender As System.Object, e As System.EventArgs) Handles SaveActionButton.Click

        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate
        Dim DR As DataRow

        Try

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

            SaveActionButton.Enabled = False
            CancelActionButton.Enabled = False

            ClearErrors()

            DR = DirectCast(_EAHBS.Current, DataRowView).Row

            If Not ValidateChildren() Then
                SaveActionButton.Enabled = True
                CancelActionButton.Enabled = True
                Return
            End If

            DR.EndEdit()

            _ChangedDRs = CType(_EAHDS.GetChanges(), EligAcctHoursDS)

            If _ChangedDRs IsNot Nothing Then

                If SaveChanges() Then
                    MessageBox.Show("Eligibility Special Account Hours saved successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    _SelectedMonthHoursDT = Nothing

                    If _RecCntHMOAdj > 0 Then
                        MessageBox.Show("Participant Enrolled in HMO", "HMO", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If

                    If _SelectedEligPeriod <= RegMasterDAL.GlobalEligPeriod Then
                        '' Message to user to calculate eligiblity for current an retro periods
                        MessageBox.Show("For this Hours change Eligibility calculation is required. ", "Calculate Eligibility", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If

                    _EAHBS.ResetBindings(False)

                    _ReturnDialogStatus = DialogResult.Yes

                Else
                    MessageBox.Show("Error while saving Eligibility Special Account Hours record." & vbCrLf & "Please try again ", "Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If

            End If

        Catch ex As Exception

            CancelActionButton.Enabled = True


                Throw

        Finally

            Me.AutoValidate = HoldAutoValidate

        End Try

    End Sub

    Private Sub EligSpecialAcctDataGrid_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles EligSpecialAcctDataGrid.MouseMove
        Dim HTI As DataGrid.HitTestInfo

        Try
            HTI = CType(sender, DataGridCustom).HitTest(e.X, e.Y)

            ' Do not display hover text if it is a drag event 
            If (e.Button <> MouseButtons.Left) Then
                ' Check if the target is a different cell from the previous one 
                If HTI.Type = DataGrid.HitTestType.Cell And
                  HTI.Row <> _HoverCell.RowNumber OrElse HTI.Column <> _HoverCell.ColumnNumber Then
                    ' Store the new hit row 
                    _HoverCell.RowNumber = HTI.Row
                    _HoverCell.ColumnNumber = HTI.Column
                End If
            End If

            If HTI.Type = DataGrid.HitTestType.Cell Then
                If _HoverCell.RowNumber > -1 AndAlso _HoverCell.RowNumber <= (CType(sender, DataGridCustom).GetGridRowCount) Then
                    If EligSpecialAcctDataGrid.GetColumnMapping(_HoverCell.ColumnNumber) = "ACCTNO" Then

                        If _HoverCell.RowNumber = -1 OrElse _HoverCell.ColumnNumber = -1 Then
                            Exit Sub
                        End If

                        If IsDBNull(EligSpecialAcctDataGrid.Item(HTI.Row, HTI.Column)) Then
                            Exit Sub
                        End If

                        Dim LFDataRow As DataRow()

                        If EligSpecialAcctDataGrid.Item(_HoverCell) IsNot Nothing AndAlso CStr(EligSpecialAcctDataGrid.Item(_HoverCell)).Trim.Length > 0 Then LFDataRow = _EligSpecialAcctValuesDT.Select("ACCTNO =" & CStr(EligSpecialAcctDataGrid.Item(_HoverCell)))
                        If LFDataRow IsNot Nothing AndAlso LFDataRow.Length > 0 Then
                            If CStr(EligSpecialAcctDataGrid.Item(_HoverCell)) = CStr(LFDataRow(0).Item("ACCTNO")) Then
                                ToolTip1.Active = True
                                ToolTip1.SetToolTip(EligSpecialAcctDataGrid, CStr(LFDataRow(0).Item("SHORT_DESC")))
                            Else
                                ToolTip1.Active = False
                                ToolTip1.SetToolTip(EligSpecialAcctDataGrid, "")
                            End If
                        End If

                        If LFDataRow IsNot Nothing Then LFDataRow = Nothing

                    Else
                        ToolTip1.Active = False
                        ToolTip1.SetToolTip(EligSpecialAcctDataGrid, "")
                    End If
                Else
                    ToolTip1.Active = False
                    ToolTip1.SetToolTip(EligSpecialAcctDataGrid, "")
                End If
            End If

        Catch ex As Exception

                Throw
        Finally
            If HTI IsNot Nothing Then HTI = Nothing
        End Try
    End Sub

    Private Sub EligAcctHrsMaint_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed

        Try

            SaveSettings()

            Me.DialogResult = _ReturnDialogStatus

            Me.Close()

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub EligAcctHrsMaint_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Try

            If UnCommittedChangesExist() Then
                MessageBox.Show(Me, "Changes have been made to Special Account Maintenance Screen." & vbCrLf &
                                             "Please Complete the changes before continuing", "Save changes", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)

                e.Cancel = True

            End If

        Catch ex As Exception

                Throw
        Finally
        End Try
    End Sub

    Private Sub cmbSpecialAcct_SelectedValueChanged(sender As Object, e As EventArgs) Handles cmbSpecialAcct.SelectedValueChanged

        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Dim BS As BindingSource

        Try

            If _EAHBS Is Nothing OrElse _EAHBS.Position < 0 OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

            BS = DirectCast(_EAHBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & CBox.Name & " BS(" & BS.Position.ToString & ") Val(" & CBox.SelectedValue.ToString & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            '_EAHBS.ResetCurrentItem()
            'EligSpecialAcctDataGrid.RefreshData()

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
        End Try

    End Sub

    Private Sub cmbSpecialAcct_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSpecialAcct.SelectedIndexChanged

        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Dim BS As BindingSource

        Try

            If _EAHBS Is Nothing OrElse _EAHBS.Position < 0 OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

            BS = DirectCast(_EAHBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & CBox.Name & " BS(" & BS.Position.ToString & ") Val(" & CBox.SelectedValue.ToString & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            '_EAHBS.ResetCurrentItem()
            'EligSpecialAcctDataGrid.RefreshData()

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
        End Try

    End Sub

    Private Sub cmbSpecialAcct_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbSpecialAcct.SelectionChangeCommitted
        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Dim DR As DataRow
        Dim BS As BindingSource

        Try

            If _EAHBS Is Nothing OrElse _EAHBS.Position < 0 OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

            BS = DirectCast(_EAHBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & CBox.Name & " BS(" & BS.Position.ToString & ") Val(" & CBox.SelectedValue.ToString & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = DirectCast(_EAHBS.Current, DataRowView).Row

            CType(CBox.Parent, TransparentContainer).ValidateChildren() 'this will trigger validation of the cmbbox triggering write of value to DS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
        End Try

    End Sub

    Private Sub cmbSpecialAcct_Validating(sender As Object, e As CancelEventArgs) Handles cmbSpecialAcct.Validating

        Dim CBox As ExComboBox = CType(sender, ExComboBox)
        Dim BS As BindingSource
        Dim DR As DataRow

        Try

            ErrorProvider1.ClearError(CBox)

            If _EAHBS Is Nothing OrElse _EAHBS.Position < 0 OrElse CBox.ReadOnly Then Return

            BS = DirectCast(_EAHBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & CBox.Name & " BS(" & BS.Position.ToString & ") Val(" & If(CBox.SelectedValue Is Nothing, "N/A", CBox.SelectedValue.ToString) & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If CBox.SelectedIndex < 0 Then
                ErrorProvider1.SetErrorWithTracking(CBox, " Account is required.")
            Else

                DR = DirectCast(_EAHBS.Current, DataRowView).Row
                Dim DuplicatesQuery =
                    From Dupes In _EAHDS.Tables("ELIG_ACCT_HOURS").AsEnumerable()
                    Where Dupes.RowState = DataRowState.Unchanged _
                    AndAlso Dupes.Field(Of Integer)("ACCTNO") = CInt(CType(cmbSpecialAcct.SelectedItem, DataRowView)("ACCTNO"))

                If DR.RowState = DataRowState.Added AndAlso DuplicatesQuery.Any Then
                    ErrorProvider1.SetErrorWithTracking(CBox, " AcctNo already in use.")

                ElseIf Not IsDBNull(CType(cmbSpecialAcct.SelectedItem, DataRowView)("GROUP_FUNCTIONALITY")) AndAlso CType(cmbSpecialAcct.SelectedItem, DataRowView)("GROUP_FUNCTIONALITY").ToString.Contains("RETIREE") Then
                    If _EligRetireeElementsDS Is Nothing OrElse _EligRetireeElementsDS.Tables.Count < 1 OrElse _EligRetireeElementsDS.Tables(0).Rows.Count < 1 Then
                        MessageBox.Show("Retiree information is missing. Please add it first before adding Retiree Account Data'", "Retiree Setup Required.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        ErrorProvider1.SetErrorWithTracking(CBox, " Retiree information is missing.")
                    End If
                ElseIf _RetrieveA2CountDS IsNot Nothing AndAlso _RetrieveA2CountDS.Tables.Contains("A2BKCNT") AndAlso _RetrieveA2CountDS.Tables("A2BKCNT").Rows.Count > 0 Then 'Active 110's Only

                    Dim A2BCntDR As DataRow = _RetrieveA2CountDS.Tables("A2BKCNT").Rows(0)

                    If _EligMetalDescriptionsDT IsNot Nothing AndAlso _EligMetalDescriptionsDT.Rows.Count > 0 Then
                        Dim MetalQuery = From Metal As DataRow In _EligMetalDescriptionsDT.AsEnumerable()
                                         Where Metal.Field(Of String)("MEMTYPE").Trim = _Memtype.Trim _
                                        AndAlso CDate(A2BCntDR("OLDESTHRS")) >= Metal.Field(Of Date)("FROM_WORK_PERIOD") _
                                        AndAlso CDate(A2BCntDR("OLDESTHRS")) <= Metal.Field(Of Date)("THRU_WORK_PERIOD") _
                                        AndAlso CInt(A2BCntDR("A2COUNT")) >= Metal.Field(Of Integer)("MIN_SENIORITY") _
                                        AndAlso CInt(A2BCntDR("A2COUNT")) <= Metal.Field(Of Integer)("MAX_SENIORITY")

                        If MetalQuery.Any Then

                            Dim MetalDR As DataRow = MetalQuery.First

                            Dim InEligibleQuery = From InEligible As DataRow In _InEligibleforSpecialAccountsDT.AsEnumerable()
                                                  Where InEligible.Field(Of String)("MEMTYPE").Trim = _Memtype.Trim _
                                                AndAlso InEligible.Field(Of String)("METAL_DESC").Trim = MetalDR("METAL_DESC").ToString.Trim _
                                                AndAlso InEligible.Field(Of Integer)("ACCTNO") = CInt(CType(cmbSpecialAcct.SelectedItem, DataRowView)("ACCTNO")) _
                                                  AndAlso _SelectedEligPeriod >= InEligible.Field(Of Date)("FROM_DATE") AndAlso _SelectedEligPeriod <= InEligible.Field(Of Date)("THRU_DATE")

                            If InEligibleQuery.Any Then
                                Dim InEligibleDR As DataRow = InEligibleQuery.First
                                ErrorProvider1.SetErrorWithTracking(CBox, InEligibleDR("REASON").ToString)
                            End If

                        End If

                    End If

                End If
            End If

            If ErrorProvider1.GetError(CBox).Trim.Length > 0 Then 'are there any errors  
                e.Cancel = True
                Return
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Sub

    Private Sub cmbSpecialAcct_Validated(sender As Object, e As EventArgs) Handles cmbSpecialAcct.Validated

        Dim DR As DataRow
        Dim CBox As ExComboBox = CType(sender, ExComboBox)
        Dim BS As BindingSource
        Dim HoldDate As Date?

        Try

            If _EAHBS Is Nothing OrElse _EAHBS.Position < 0 OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

            BS = DirectCast(_EAHBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & CBox.Name & " BS(" & BS.Position.ToString & ") Val(" & CBox.SelectedValue.ToString & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = DirectCast(_EAHBS.Current, DataRowView).Row

            If (Not DR.HasVersion(DataRowVersion.Proposed) AndAlso CType(cmbSpecialAcct.SelectedItem, DataRowView)("ACCTNO").ToString <> DR("ACCTNO").ToString) OrElse
                    (DR.HasVersion(DataRowVersion.Proposed) AndAlso CType(cmbSpecialAcct.SelectedItem, DataRowView)("ACCTNO").ToString <> DR("ACCTNO", DataRowVersion.Proposed).ToString) Then

                'When the selected Acct is first selected (not modified) or does not match the current or proposed DR version, the DR should be reinitialized to remove stale values.
                DR.BeginEdit()

                DR("HOURS") = System.DBNull.Value
                txtDays.Text = ""
                txtHours.Text = ""

                DR("SHORT_DESC") = CType(cmbSpecialAcct.SelectedItem, DataRowView)("SHORT_DESC")
                DR("CONSEC_SERVICE_SW") = CType(cmbSpecialAcct.SelectedItem, DataRowView)("CONSEC_SERVICE_SW")
                DR("HOURS_CODE") = CType(cmbSpecialAcct.SelectedItem, DataRowView)("OLD_ENTRY_CODE")

                If CInt(CType(cmbSpecialAcct.SelectedItem, DataRowView)("ENTER_HOURS_SW")) = 0 Then      '' IF THE SWITCH value = 1 then we r entering the user entered value
                    DR("WEIGHTED_HOURS") = CType(cmbSpecialAcct.SelectedItem, DataRowView)("WEIGHTED_HOURS")
                    DR("HOURS") = 0
                    txtHours.Text = "0"
                End If

            End If

            If IsDate(DR("ELIG_PERIOD")) Then
                HoldDate = CDate(DR("ELIG_PERIOD")).AddMonths(-(CInt(CType(cmbSpecialAcct.SelectedItem, DataRowView)("SKIP_MONTH_FACTOR"))))
                If IsDBNull(DR("PSTPERIOD")) OrElse HoldDate <> CDate(DR("PSTPERIOD")) Then
                    DR.BeginEdit()
                    DR("PSTPERIOD") = HoldDate
                End If
            End If

            'DONOT use BindingSource or Row ENDEDIT Here, it causes intefering events to trigger

            SetUIElements(_REGMReadOnlyAccess) 'call to establish any form changes triggered by selection

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Sub

    Private Sub txtHours_Validated(sender As Object, e As EventArgs) Handles txtHours.Validated

        Dim Tbox As TextBox = CType(sender, TextBox)
        Dim DR As DataRow

        Try

            If _EAHBS Is Nothing OrElse _EAHBS.Position < 0 OrElse Tbox.ReadOnly OrElse cmbSpecialAcct.SelectedIndex < 0 OrElse Tbox.Text.Trim.Length < 1 Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = DirectCast(_EAHBS.Current, DataRowView).Row

            DR.BeginEdit()

            DR("HOURS") = CDec(txtHours.Text)
            txtDays.Text = (CDec(txtHours.Text) / CDec(CType(cmbSpecialAcct.SelectedItem, DataRowView)("HOURS_PER_DAY"))).ToString("0.00")

            If CBool(CType(cmbSpecialAcct.SelectedItem, DataRowView)("ENTER_HOURS_SW")) Then      '' IF THE SWITCH value = 1 then we r entering the user entered value
                DR("WEIGHTED_HOURS") = DR("HOURS")
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
        End Try

    End Sub

    Private Sub txtHours_Validating(sender As Object, e As CancelEventArgs) Handles txtHours.Validating

        Dim Tbox As TextBox = CType(sender, TextBox)
        Dim DR As DataRow
        Dim SpecialAcctDR As DataRow

        Try

            ErrorProvider1.ClearError(Tbox)

            If _EAHBS Is Nothing OrElse _EAHBS.Position < 0 OrElse Tbox.ReadOnly OrElse cmbSpecialAcct.SelectedIndex < 0 Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = CType(_EAHBS.Current, DataRowView).Row
            SpecialAcctDR = CType(_EligSpecialAcctValuesBS.Current, DataRowView).Row

            If CBool(SpecialAcctDR("ENTER_HOURS_SW")) AndAlso SpecialAcctDR("DESCRIPTION").ToString.ToUpper.Contains("HOURS") AndAlso (Tbox.Text.Trim.Length < 1 OrElse CDec(Tbox.Text.Trim) = 0) Then
                ErrorProvider1.SetErrorWithTracking(Tbox, " Number of Hours required.")
            ElseIf CBool(SpecialAcctDR("ENTER_HOURS_SW")) AndAlso Tbox.Text.Trim.Length > 0 Then
                Dim PstPeriod As Date = CDate(DR("ELIG_PERIOD")).AddMonths(-(CInt(SpecialAcctDR("SKIP_MONTH_FACTOR"))))
                Dim DaysInMonth As Integer = System.DateTime.DaysInMonth(PstPeriod.Year, PstPeriod.Month)
                If CInt(Tbox.Text) > (DaysInMonth * CInt(CType(cmbSpecialAcct.SelectedItem, DataRowView)("HOURS_PER_DAY"))) Then
                    MessageBox.Show("Allowed Maximum Hours " & (DaysInMonth * CInt(CType(cmbSpecialAcct.SelectedItem, DataRowView)("HOURS_PER_DAY"))).ToString & " exceeded.", "Max Hours Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ErrorProvider1.SetErrorWithTracking(Tbox, " Max Hours Exceeded.")
                End If
            End If

            If ErrorProvider1.GetError(Tbox).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
                Return
            End If

        Catch ex As Exception

                Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

    Private Sub txtDays_Validating(sender As Object, e As CancelEventArgs) Handles txtDays.Validating

        Dim Tbox As TextBox = CType(sender, TextBox)
        Dim DR As DataRow
        Dim SpecialAcctDR As DataRow

        Try

            ErrorProvider1.ClearError(Tbox)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If _EAHBS Is Nothing OrElse _EAHBS.Position < 0 OrElse Tbox.ReadOnly OrElse cmbSpecialAcct.SelectedIndex < 0 Then Return

            DR = CType(_EAHBS.Current, DataRowView).Row
            SpecialAcctDR = CType(_EligSpecialAcctValuesBS.Current, DataRowView).Row

            If CBool(SpecialAcctDR("ENTER_HOURS_SW")) AndAlso Not SpecialAcctDR("DESCRIPTION").ToString.ToUpper.Contains("HOURS") AndAlso (Tbox.Text.Trim.Length < 1 OrElse CDec(Tbox.Text.Trim) = 0) Then
                ErrorProvider1.SetErrorWithTracking(Tbox, " Number of Days required.")
            ElseIf CBool(SpecialAcctDR("ENTER_HOURS_SW")) AndAlso Tbox.Text.Trim.Length > 0 Then
                Dim PstPeriod As Date = CDate(DR("ELIG_PERIOD")).AddMonths(-(CInt(SpecialAcctDR("SKIP_MONTH_FACTOR"))))
                Dim DaysInMonth As Integer = System.DateTime.DaysInMonth(PstPeriod.Year, PstPeriod.Month)
                If CInt(Tbox.Text) > DaysInMonth Then
                    MessageBox.Show("Allowed Maximum days " & DaysInMonth.ToString & " exceeded.", "Max Days Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ErrorProvider1.SetErrorWithTracking(Tbox, " Max Days Exceeded.")
                End If
            End If

            If ErrorProvider1.GetError(Tbox).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
                Return
            End If

        Catch ex As Exception

                Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub txtDays_Validated(sender As Object, e As EventArgs) Handles txtDays.Validated

        Dim Tbox As TextBox = CType(sender, TextBox)
        Dim DR As DataRow

        Try
            If _EAHBS Is Nothing OrElse _EAHBS.Position < 0 OrElse Tbox.ReadOnly OrElse cmbSpecialAcct.SelectedIndex < 0 OrElse Tbox.Text.Trim.Length < 1 Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = DirectCast(_EAHBS.Current, DataRowView).Row

            If txtHours.Text <> (CDec(txtDays.Text) * CDec(CType(cmbSpecialAcct.SelectedItem, DataRowView)("HOURS_PER_DAY"))).ToString("0.00") Then
                DR.BeginEdit()
                'populate read only field with calculated hours based upon #Hours per day * #Days
                txtHours.Text = (CDec(txtDays.Text) * CDec(CType(cmbSpecialAcct.SelectedItem, DataRowView)("HOURS_PER_DAY"))).ToString("0.00")
                txtHours.DataBindings("Text").WriteValue()

                If CBool(CType(cmbSpecialAcct.SelectedItem, DataRowView)("ENTER_HOURS_SW")) Then      '' IF THE SWITCH value = 1 then we r entering the user entered value
                    DR("WEIGHTED_HOURS") = (CDec(txtDays.Text) * CDec(CType(cmbSpecialAcct.SelectedItem, DataRowView)("HOURS_PER_DAY"))).ToString("0.00")
                End If

            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
        End Try
    End Sub

    Private Sub txtDays_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDays.KeyPress
        If Char.IsDigit(e.KeyChar) OrElse Char.IsControl(e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub eligDateTimePicker_Validating(sender As Object, e As CancelEventArgs) Handles eligDateTimePicker.Validating

        Dim DTP As DateTimePicker = CType(sender, DateTimePicker)
        Dim DR As DataRow
        Dim EligMonthDate As Date

        Try

            ErrorProvider1.ClearError(DTP)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            EligMonthDate = CDate(CStr(eligDateTimePicker.Value.Month) & "/1/" & CStr(eligDateTimePicker.Value.Year))


            If Not _REGMSupervisor AndAlso DateDiff(DateInterval.Month, EligMonthDate, RegMasterDAL.GlobalEligPeriod) > 12 Then
                MessageBox.Show("Period exceeds 12 months.", "Max Period Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ErrorProvider1.SetErrorWithTracking(DTP, " Max Period Exceeded.")
            End If

            If ErrorProvider1.GetError(DTP).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
                Return
            End If

        Catch ex As Exception

                Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub
    Private Sub eligDateTimePicker_Validated(sender As Object, e As EventArgs) Handles eligDateTimePicker.Validated
        Dim DTP As DateTimePicker = CType(sender, DateTimePicker)
        Dim DR As DataRow

        Try
            If _EAHBS Is Nothing OrElse _EAHBS.Position < 0 OrElse _EAHBS.Count < 1 Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = DirectCast(_EAHBS.Current, DataRowView).Row


            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
        End Try

    End Sub
    Private Sub eligDateTimePicker_CloseUp(sender As Object, e As EventArgs) Handles eligDateTimePicker.CloseUp

        Dim DTP As DateTimePicker = CType(sender, DateTimePicker)
        Dim DR As DataRow
        Dim EligMonthDate As String

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & DTP.Name & " Val(" & DTP.Text.ToString & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            CType(DTP.Parent, TransparentContainer).ValidateChildren() 'this will trigger validation of the cmbbox triggering write of value to DS

            If ErrorProvider1.GetError(DTP).Trim.Length > 0 Then Return

            EligMonthDate = CStr(eligDateTimePicker.Value.Month) & "/1/" & CStr(eligDateTimePicker.Value.Year)
            _SelectedEligPeriod = CType(EligMonthDate, Date?)

            If _EAHBS IsNot Nothing AndAlso _EAHBS.Position > -1 AndAlso _EAHBS.Count > 0 Then
                DR = DirectCast(_EAHBS.Current, DataRowView).Row
            End If

            If DR Is Nothing OrElse IsDBNull(DR("ELIGIBILITY_MONTH")) OrElse _SelectedEligPeriod <> CDate(DR("ELIGIBILITY_MONTH")) Then
                LoadEligAccountHours()
                SetUIElements(_ReadOnlyMode)
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
        End Try

    End Sub

    Private Sub eligDateTimePicker_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles eligDateTimePicker.Enter
        ''SendKeys.Send("%{DOWN}")
    End Sub

    Private Sub CalculateEligibilityActionButton_Click(sender As Object, e As EventArgs) Handles CalculateEligibilityActionButton.Click

        Dim EligCalc As CalculateEligibility

        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

        Try

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            CalculateEligibilityActionButton.Enabled = False

            If _SelectedEligPeriod > RegMasterDAL.GlobalEligPeriod Then
                MessageBox.Show("You cannot calculate Eligibility for a future period", "Future Eligibility", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            ''Initialising 
            EligCalc = New CalculateEligibility(_FamilyID, CDate(_SelectedEligPeriod))

            ''check status of rows as they are new to calculate eligibilty
            If EligCalc.RetrieveRowStatusbeforeEligcalculation(_FamilyID, CDate(_SelectedEligPeriod)) = False Then
                Return
            End If

            '' There are changes in elig_acct_hours row status
            Dim ReturnedStatus As Boolean = False

            ReturnedStatus = EligCalc.DetermineEligibility(_FamilyID, CDate(_SelectedEligPeriod))
            If ReturnedStatus Then
                _EligibilityCalculate = True
                RegMasterDAL.MadeEligibilityChanges = True
                MessageBox.Show("Eligibility calculated Successfully. Please check Eligibilty Tab for results", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Error while Calculating Eligibility." & vbCrLf & "Please try again ", "Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            Else
                MessageBox.Show("Error while Calculating Eligibility." & vbCrLf & "Please try again ", "Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Finally

            If EligCalc IsNot Nothing Then
                EligCalc.disposeobjects()
            End If
            EligCalc = Nothing

            Me.AutoValidate = HoldAutoValidate

        End Try
    End Sub

#End Region

#Region "Custom Subs\Functions"

    Public Sub LoadEligAccountHours()
        Try

            grpEditPanel.SuspendLayout()

            ClearErrors()
            ClearDataBindings(Me)

            _EAHDS.EnforceConstraints = False
            _TotalSADS.EnforceConstraints = False
            _TotalSADS.ELIG_ACCT_HOURS.Rows.Clear()

            If _TotalSADS IsNot Nothing AndAlso (_TotalSADS.ELIG_ACCT_HOURS.Rows.Count = 0) Then

                Dim SelectedMonthDV As DataView = New DataView(_AllEligMonthHoursDS.Tables(0), "ELIGIBILITY_MONTH = '" & Format(_SelectedEligPeriod, "yyyy-MM-dd") & "'", "", DataViewRowState.CurrentRows)
                _SelectedMonthHoursDT = SelectedMonthDV.ToTable

                If SelectedMonthDV.Count = 1 Then
                    If CStr(SelectedMonthDV(0)("Type")).ToUpper = "NO HOURS" Then              ''''not to display this when no hours
                        _SelectedMonthHoursDT = Nothing
                    End If
                End If

                'used to determine if Retiree Special Hours can be used.
                _EligRetireeElementsDS = RegMasterDAL.RetrieveEligRetireeElementsByFamilyID(_FamilyID) 'Only 1 row per family, not elig month aware.

                _RetrieveA2CountDS = RegMasterDAL.RetrieveA2CountByFamilyid(_FamilyID)

            End If

            If _SelectedMonthHoursDT IsNot Nothing Then
                _TotalSADS.ELIG_ACCT_HOURS.Merge(_SelectedMonthHoursDT)    '' To convert untyped to typed
            End If

            EligSpecialAcctDataGrid.CaptionText = "Eligibility Special Hours for Period " & CType(_SelectedEligPeriod, Date?)

            If _EAHDS IsNot Nothing AndAlso _EAHDS.ELIG_ACCT_HOURS.Rows.Count = 0 Then
                _EAHDS = _TotalSADS
            End If
            _EAHDS.AcceptChanges()


            RemoveHandler _EAHDS.ELIG_ACCT_HOURS.ColumnChanging, AddressOf EAHDS_ColumnChanging
            AddHandler _EAHDS.ELIG_ACCT_HOURS.ColumnChanging, AddressOf EAHDS_ColumnChanging
            RemoveHandler _EAHDS.ELIG_ACCT_HOURS.RowChanging, AddressOf EAHDS_RowChanging
            AddHandler _EAHDS.ELIG_ACCT_HOURS.RowChanging, AddressOf EAHDS_RowChanging

            _EAHBS = New BindingSource
            _EAHBS.RaiseListChangedEvents = False

            _EAHBS.DataSource = _EAHDS.Tables("ELIG_ACCT_HOURS")

            EligSpecialAcctDataGrid.DataMember = ""
            EligSpecialAcctDataGrid.DataSource = _EAHBS
            EligSpecialAcctDataGrid.SetTableStyle()

            _EAHBS.Sort = If(EligSpecialAcctDataGrid.LastSortedBy, EligSpecialAcctDataGrid.DefaultSort)

            LoadDataBindings()

            _EAHBS.RaiseListChangedEvents = True
            _EAHBS.ResetBindings(False)

            eligDateTimePicker.MaxDate = UFCWGeneral.MonthEndDate(DateAdd(DateInterval.Month, 2, RegMasterDAL.GlobalEligPeriod))
            eligDateTimePicker.Value = New DateTime(CDate(_SelectedEligPeriod).Ticks)
            eligDateTimePicker.Checked = True

            grpEditPanel.ResumeLayout()

        Catch ex As Exception

                Throw
        End Try
    End Sub

    Private Sub LoadDataBindings()
        Dim Bind As Binding

        Try

            txtFamilyID.Text = _FamilyID.ToString
            txtRelationID.Text = CStr(0)

            cmbSpecialAcct.DataBindings.Clear()
            Bind = New Binding("SelectedValue", _EAHBS, "ACCTNO", True, DataSourceUpdateMode.OnValidation) ' False, DataSourceUpdateMode.OnPropertyChanged)
            'AddHandler Bind.Parse, AddressOf cmbSpecialAcct_Parse
            'AddHandler Bind.BindingComplete, AddressOf cmbBindingComplete
            cmbSpecialAcct.DataBindings.Add(Bind)

            eligDateTimePicker.DataBindings.Clear()
            Bind = New Binding("Text", _EAHBS, "ELIG_PERIOD", True, DataSourceUpdateMode.OnValidation)
            eligDateTimePicker.DataBindings.Add(Bind)

            txtHours.DataBindings.Clear()
            Bind = New Binding("Text", _EAHBS, "HOURS", True, DataSourceUpdateMode.OnValidation)
            Bind.DataSourceNullValue = 0
            'AddHandler Bind.Parse, AddressOf HoursBinding_Parse
            'AddHandler Bind.Format, AddressOf HoursBinding_Format
            'AddHandler Bind.BindingComplete, AddressOf HoursBindingComplete
            txtHours.DataBindings.Add(Bind)

            'txtDays.DataBindings.Clear()
            'Bind = New Binding("Text", _EAHBS, "HOURS", True, DataSourceUpdateMode.Never)
            'AddHandler Bind.Parse, AddressOf DaysBinding_Parse
            'AddHandler Bind.BindingComplete, AddressOf DaysBindingComplete
            'txtDays.DataBindings.Add(Bind)

            txtSpecialRemarks.DataBindings.Clear()
            Bind = New Binding("Text", _EAHBS, "REMARKS", True, DataSourceUpdateMode.OnPropertyChanged)
            txtSpecialRemarks.DataBindings.Add(Bind)

        Catch ex As Exception


                Throw

        Finally

        End Try

    End Sub

    'Private Sub DaysBindingComplete(sender As Object, e As BindingCompleteEventArgs)
    '    Dim DR As DataRow

    '    Try

    '        DR = DirectCast(_EAHBS.Current, DataRowView).Row

    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub

    'Private Sub DaysBinding_Parse(sender As Object, e As ConvertEventArgs)
    '    Dim DR As DataRow

    '    Try

    '        DR = DirectCast(_EAHBS.Current, DataRowView).Row

    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub

    'Private Sub HoursBinding_Parse(sender As Object, e As ConvertEventArgs)

    '    Dim DR As DataRow

    '    Try

    '        DR = DirectCast(_EAHBS.Current, DataRowView).Row
    '        'If IsDBNull(DR("HOURS")) OrElse CDec(DR("HOURS")) <> (CDec(txtDays.Text) * CDec(CType(cmbSpecialAcct.SelectedItem, DataRowView)("HOURS_PER_DAY"))) Then
    '        '    DR("HOURS") = CDec(txtDays.Text) * CDec(CType(cmbSpecialAcct.SelectedItem, DataRowView)("HOURS_PER_DAY"))
    '        'End If

    '    Catch ex As Exception
    '        Throw
    '    End Try

    'End Sub

    'Private Sub HoursBindingComplete(sender As Object, e As BindingCompleteEventArgs)
    '    Dim DR As DataRow

    '    Try

    '        DR = DirectCast(_EAHBS.Current, DataRowView).Row

    '        If cmbSpecialAcct.SelectedIndex > -1 AndAlso CBool(CType(cmbSpecialAcct.SelectedItem, DataRowView)("ENTER_HOURS_SW")) Then      '' IF THE SWITCH value = 1 then we r entering the user entered value
    '            'If (IsDBNull(DR("WEIGHTED_HOURS")) AndAlso Not IsDBNull(DR("HOURS"))) OrElse CDec(DR("WEIGHTED_HOURS")) <> CDec(DR("HOURS")) Then
    '            '    DR("WEIGHTED_HOURS") = DR("HOURS")
    '            'End If
    '        End If

    '    Catch ex As Exception
    '        Throw
    '    End Try

    'End Sub

    'Private Sub cmbSpecialAcct_Parse(sender As Object, e As ConvertEventArgs)

    '    ' if binding uses propertychanged associated updates should happen here, else in the validating event
    '    ' Maybe it should always happen here??

    '    'Dim BS As BindingSource
    '    'Dim DR As DataRow
    '    'Dim CBox As ExComboBox = CType(CType(sender, Binding).BindableComponent, ExComboBox)

    '    'Try

    '    '    If _EAHBS Is Nothing OrElse _EAHBS.Position < 0 OrElse CBox.ReadOnly Then Return

    '    '    BS = DirectCast(_EAHBS, BindingSource)

    '    '    Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & CBox.Name & " BS(" & BS.Position.ToString & ") Val(" & If(CBox.SelectedValue Is Nothing, "N/A", CBox.SelectedValue.ToString) & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

    '    '    If CBox.SelectedIndex > 0 Then

    '    '        DR = DirectCast(_EAHBS.Current, DataRowView).Row

    '    '        If CType(cmbSpecialAcct.SelectedItem, DataRowView)("ACCTNO").ToString <> DR("ACCTNO").ToString Then

    '    '            _EAHBS.RaiseListChangedEvents = False
    '    '            DR.BeginEdit()

    '    '            DR("HOURS") = System.DBNull.Value
    '    '            txtDays.Text = ""

    '    '            DR("SHORT_DESC") = CType(cmbSpecialAcct.SelectedItem, DataRowView)("SHORT_DESC")

    '    '            DR("CONSEC_SERVICE_SW") = CType(cmbSpecialAcct.SelectedItem, DataRowView)("CONSEC_SERVICE_SW")

    '    '            DR("HOURS_CODE") = CType(cmbSpecialAcct.SelectedItem, DataRowView)("OLD_ENTRY_CODE")

    '    '            If CInt(CType(cmbSpecialAcct.SelectedItem, DataRowView)("ENTER_HOURS_SW")) = 0 Then      '' IF THE SWITCH value = 1 then we r entering the user entered value
    '    '                DR("WEIGHTED_HOURS") = CType(cmbSpecialAcct.SelectedItem, DataRowView)("WEIGHTED_HOURS")
    '    '                DR("HOURS") = 0
    '    '            End If

    '    '            DR("PSTPERIOD") = CDate(DR("ELIG_PERIOD")).AddMonths(-(CInt(CType(cmbSpecialAcct.SelectedItem, DataRowView)("SKIP_MONTH_FACTOR"))))

    '    '            _EAHBS.RaiseListChangedEvents = True

    '    '        End If
    '    '    End If

    '    'Catch ex As Exception
    '    '    Throw
    '    'Finally
    '    '    Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") Val(" & e.Value.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
    '    'End Try

    'End Sub

    'Private Sub cmbBindingComplete(sender As Object, e As BindingCompleteEventArgs)

    '    Dim BS As BindingSource

    '    Try

    '        BS = DirectCast(_EAHBS, BindingSource)

    '        Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") Val(" & e.Binding.Control.Name & " " & e.ErrorText & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

    '        If Not e.BindingCompleteState = BindingCompleteState.Success Then
    '            MessageBox.Show("Control " & e.Binding.Control.Name & " " & e.ErrorText, "Problem converting data to database format", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        Else
    '            '                Debug.Print("BindingCompleteEventHandler - " & e.Binding.Control.Name & " - " & CType(CType(sender, Binding).Control, ComboBox).SelectedIndex.ToString & " - " & If(CType(CType(sender, Binding).Control, ComboBox).SelectedValue Is Nothing, "", CType(CType(sender, Binding).Control, ComboBox).SelectedValue).ToString & " - " & CType(CType(sender, Binding).Control, ComboBox).Text & " - " & CType(CType(sender, Binding).Control, ComboBox).SelectedText)
    '        End If

    '    Catch ex As Exception
    '        Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '        If (rethrow) Then
    '            Throw
    '            'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End If
    '    End Try

    'End Sub

    Private Sub AddSpecialAcct()

        Dim DR As DataRow

        Try

            _EAHDS.EnforceConstraints = False
            DR = _EAHDS.Tables("ELIG_ACCT_HOURS").NewRow
            DR.BeginEdit()

            DR("FAMILY_ID") = _FamilyID
            DR("RELATION_ID") = _RelationID
            DR("PSTPERIOD") = DBNull.Value 'calculated based upon account selection
            DR("ACCTNO") = DBNull.Value
            DR("EMPLR_TYPE") = DBNull.Value
            DR("PLANTYPE") = DBNull.Value
            DR("MEMTYPE") = DBNull.Value
            DR("MEMCLASS") = DBNull.Value
            DR("HOURS") = DBNull.Value
            DR("HOURS_CODE") = DBNull.Value

            DR("CREATE_USERID") = UFCWGeneral.DomainUser.ToUpper
            DR("CREATE_DATE") = Date.Now
            DR("BATCH_USERID") = UFCWGeneral.DomainUser.ToUpper
            DR("BATCH_DATE") = CDate(Now)
            DR("ONLINE_USERID") = UFCWGeneral.DomainUser.ToUpper
            DR("ONLINE_DATE") = CDate(Now)

            DR("ELIG_PERIOD") = _SelectedEligPeriod
            DR("CONSEC_SERVICE_SW") = 1
            DR("PROCESS_STATUS") = "P"
            DR("WEIGHTED_HOURS") = DBNull.Value
            DR("REMARKS") = ""
            DR("SHORT_DESC") = ""

            DR.EndEdit()

            _EAHDS.Tables("ELIG_ACCT_HOURS").Rows.Add(DR)

        Catch ex As Exception


            Throw

        End Try
    End Sub

    Public Sub LoadSpecialAccounts()

        Try

            _EligSpecialAcctValuesDT = RegMasterDAL.RetrieveEligSpecialAcctValues

            _EligSpecialAcctValuesBS = New BindingSource
            _EligSpecialAcctValuesBS.DataSource = _EligSpecialAcctValuesDT
            _EligSpecialAcctValuesBS.Filter = "USER_ENTRY_SW = 1" 'when enabled list should be restricted to user selectable only.
            _EligSpecialAcctValuesBS.Sort = "DESCRIPTION"

            cmbSpecialAcct.ValueMember = "ACCTNO"
            cmbSpecialAcct.DisplayMember = "DESCRIPTION"
            cmbSpecialAcct.DataSource = _EligSpecialAcctValuesBS

            cmbSpecialAcct.SelectedIndex = -1

        Catch ex As Exception

                Throw

        Finally

        End Try
    End Sub

    Public Sub LoadIneligibleforSpecialAccounts()    ''This method is for validating metal and special accounts

        Try

            _InEligibleforSpecialAccountsDT = RegMasterDAL.RetrieveIneligibleforSpecialAccounts

        Catch ex As Exception

                Throw

        Finally

        End Try
    End Sub

    Public Sub LoadEligMetalDescriptions()                                     ''This method is for validating metal and special accounts

        Try
            _EligMetalDescriptionsDT = RegMasterDAL.RetrieveEligMetalDescriptions

        Catch ex As Exception

                Throw

        Finally

        End Try
    End Sub

    Private Sub SaveSettings()
        Dim lWindowState As FormWindowState = Me.WindowState
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "WindowState", CInt(lWindowState).ToString)

        '' Me.WindowState = FormWindowState.Normal
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = lWindowState

    End Sub

    Private Sub SetSettings()

        Me.Top = If(CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)))
        Me.Height = CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = If(CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)))
        Me.Width = CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(Me.AppKey, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)
    End Sub

    Public Sub ClearErrors()
        ErrorProvider1.Clear()
        lblmessage.Text = ""
    End Sub

    Public Sub ClearAll()

        EligSpecialAcctDataGrid.DataSource = Nothing
        EligSpecialAcctDataGrid.CaptionText = ""

        _TotalSADS = New EligAcctHoursDS
        _TotalSADS.EnforceConstraints = False

        _EAHDS = New EligAcctHoursDS
        _EAHDS.EnforceConstraints = False

        _SelectedMonthHoursDT = New DataTable

        _ChangedDRs = Nothing
        _HoverCell = Nothing

        _RecCntHMOAdj = 0

        _EligibilityCalculate = False

    End Sub

    Private Function UnCommittedChangesExist() As Boolean

        Dim Modifications As String = ""
        Try

            If _EAHDS Is Nothing Then Return False

            _ChangedDRs = CType(_EAHDS.GetChanges, EligAcctHoursDS)

            If EligSpecialAcctDataGrid IsNot Nothing AndAlso _ChangedDRs IsNot Nothing AndAlso _ChangedDRs.Tables("ELIG_ACCT_HOURS").Rows.Count > 0 Then

                For Each DR As DataRow In _ChangedDRs.Tables("ELIG_ACCT_HOURS").Rows
                    If DR.RowState <> DataRowState.Added Then
                        'attempt to exclude rows accidently changed during navigation operations
                        Modifications = DataGridCustom.IdentifyChanges(DR, EligSpecialAcctDataGrid)

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

    Public Function SaveChanges() As Boolean

        Dim ChgCnt As Integer = 0
        Dim HistSum As String = ""
        Dim HistDetail As String = ""
        Dim Strchanges As String = ""

        Dim ActivityTimeStamp As DateTime = Date.Now

        Dim Transaction As DbTransaction

        Try
            Transaction = RegMasterDAL.BeginTransaction

            _ChangedDRs = CType(_EAHDS.GetChanges(), EligAcctHoursDS)

            For Each DR As DataRow In _ChangedDRs.Tables("ELIG_ACCT_HOURS").Rows

                HistSum = ""
                HistDetail = ""

                HistDetail = DataGridCustom.IdentifyChanges(DR, EligSpecialAcctDataGrid)

                If DR.RowState = DataRowState.Modified Then 'ADD

                    ChgCnt += 1

                    If HistDetail.Trim.Length > 0 Then

                        HistSum = "SPECIAL ACCOUNT HOURS FOR FAMILYID: " & Me.FamilyID & " WAS MODIFIED"
                        HistDetail = UFCWGeneral.DomainUser.ToUpper & " MODIFIED SPECIAL ACCOUNT HOURS RECORD " & Microsoft.VisualBasic.vbCrLf &
                                                       "HAVING ACCOUNT NO " & DR("ACCTNO").ToString & " FOR THE ELIGIBILITY PERIOD " & DR("ELIG_PERIOD").ToString &
                                                       " THE MODIFICATIONS WERE: " & HistDetail

                        RegMasterDAL.UpdateEligSpecialAccountHours(CInt(DR("FAMILY_ID")), CInt(DR("RELATION_ID")), CDate(DR("PSTPERIOD")),
                                                  CInt(DR("ACCTNO")), CDate(DR("ELIG_PERIOD")), CDec(DR("HOURS")), If(IsDBNull(DR("HOURS_CODE")), Nothing, DR("HOURS_CODE").ToString), UFCWGeneral.DomainUser.ToUpper,
                                                  CDec(DR("CONSEC_SERVICE_SW")), CDec(DR("WEIGHTED_HOURS")), If(IsDBNull(DR("REMARKS")), Nothing, DR("REMARKS").ToString), CInt(DR("ACCTNO", DataRowVersion.Original)), Transaction)

                        RegMasterDAL.CreateRegHistory(Me.FamilyID, Me.RelationID, Nothing, Nothing, "SPECIALHRSUPDATE", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, Transaction)
                    End If

                ElseIf DR.RowState = DataRowState.Added Then 'ADD

                    HistSum = "SPECIAL ACCOUNT HOURS FOR FAMILYID: " & Me.FamilyID & " WAS ADDED"
                    HistDetail = UFCWGeneral.DomainUser.ToUpper & " ADDED SPECIAL ACCOUNT HOURS RECORD " & Microsoft.VisualBasic.vbCrLf &
                                                       "HAVING ACCOUNT NO " & DR("ACCTNO").ToString & " FOR THE ELIGIBILITY PERIOD " & DR("ELIG_PERIOD").ToString &
                                                       " THE ADDITIONS WERE: " & HistDetail
                    ChgCnt += 1

                    _RecCntHMOAdj = RegMasterDAL.AddEligSpecialAccountHours(CInt(DR("FAMILY_ID")), CInt(DR("RELATION_ID")),
                                                CDate(DR("PSTPERIOD")), CInt(DR("ACCTNO")), CDate(DR("ELIG_PERIOD")), If(IsDBNull(DR("HOURS")), 0, CDec(DR("HOURS"))), If(IsDBNull(DR("HOURS_CODE")), Nothing, DR("HOURS_CODE").ToString), UFCWGeneral.DomainUser.ToUpper,
                                                CDec(DR("CONSEC_SERVICE_SW")), CDec(DR("WEIGHTED_HOURS")), If(IsDBNull(DR("REMARKS")), Nothing, DR("REMARKS").ToString), Transaction)

                    RegMasterDAL.CreateRegHistory(Me.FamilyID, Me.RelationID, Nothing, Nothing, "SPECIALHRSADD", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, Transaction)


                End If
            Next

            RegMasterDAL.CommitTransaction(Transaction)

            Dim EAHQuery = (From EAH As DataRow In _EAHDS.Tables("ELIG_ACCT_HOURS").AsEnumerable()
                            Where EAH.RowState = DataRowState.Modified OrElse EAH.RowState = DataRowState.Added)

            For Each DR As DataRow In EAHQuery
                DR("ONLINE_DATE") = ActivityTimeStamp
                DR("ONLINE_USERID") = UFCWGeneral.DomainUser.ToUpper
            Next

            _EAHDS.AcceptChanges()

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
                    MessageBox.Show("The item(s) you are attempting to update has been changed by another process." &
                               vbCrLf & "Exit and re-enter the Eligibility Special Account Hours form to refresh the data.", "Save rejected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Case -803
                    MessageBox.Show("Hours for the specified Account Number already exist for the calculated Post Period." &
                               vbCrLf & "Exit and re-enter the Eligibility Special Account Hours form to refresh the data.", "Save Rejected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
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


                Throw
        Finally
            If Transaction IsNot Nothing Then Transaction.Dispose()
            Transaction = Nothing

        End Try
    End Function

    Private Function DeleteSpecialAccount(ByVal dr As DataRow) As Boolean

        Dim HistSum As String
        Dim HistDetail As String
        Dim Transaction As DbTransaction

        Try

            Transaction = RegMasterDAL.BeginTransaction

            dr.Delete()

            HistDetail = DataGridCustom.IdentifyChanges(dr, EligSpecialAcctDataGrid)

            HistSum = "SPECIAL ACCOUNT " & CStr(dr("ACCTNO", DataRowVersion.Original)) & " WAS DELETED"
            HistDetail = UFCWGeneral.DomainUser.ToUpper & " DELETED THE SPECIAL ACCOUNT RECORD " & HistDetail

            RegMasterDAL.DeleteEligSpecialAccountHours(CInt(dr("FAMILY_ID", DataRowVersion.Original)), CInt(dr("RELATION_ID", DataRowVersion.Original)), CInt(dr("ACCTNO", DataRowVersion.Original)), CDate(dr("ELIG_PERIOD", DataRowVersion.Original)), UFCWGeneral.DomainUser.ToUpper, Transaction)
            RegMasterDAL.CreateRegHistory(CInt(dr("FAMILY_ID", DataRowVersion.Original)), CInt(dr("RELATION_ID", DataRowVersion.Original)), Nothing, Nothing, "SPECIALHRSDELETE", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, Transaction)

            RegMasterDAL.CommitTransaction(Transaction)

            _EAHDS.AcceptChanges()

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
                    MessageBox.Show("The item(s) you are attempting to delete has been changed by another process." &
                               vbCrLf & "Exit and re-enter the Eligibility Special Account Hours record Tab to refresh the data.", "Delete Failed.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
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


                Throw
        Finally
            If Transaction IsNot Nothing Then Transaction.Dispose()
            Transaction = Nothing

        End Try

    End Function

    'Private Sub LoadEligibility()

    '    Dim EligibilityControl As New EligibilityControl(_FamilyID, 0)
    '    Dim DT As DataTable
    '    Dim DV As DataView

    '    Try

    '        If InvokeRequired Then
    '            Me.Invoke(New MethodInvoker(AddressOf LoadEligibility))
    '        Else
    '            EligibilityControl.LoadEligibility(_FamilyID, 0)

    '            DT = EligibilityControl.EligibilityDataTable
    '            If DT.Rows.Count > 0 Then

    '                DV = New DataView(DT, "ELIG_PERIOD='" & CDate(_SelectedEligPeriod) & "'", "", DataViewRowState.CurrentRows)
    '                If DV.Count > 0 Then
    '                    If IsDBNull(DV(0)("MED_ELIG_SW")) = False AndAlso CBool(DV(0)("MED_ELIG_SW")) = True Then  ''Medical Eligibility
    '                        lbleligible.Text = "Member Eligible"
    '                    Else
    '                        lbleligible.Text = "Member Not Eligible"
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

    '        If EligibilityControl IsNot Nothing Then
    '            EligibilityControl.Dispose()
    '        End If
    '        EligibilityControl = Nothing

    '        If DV IsNot Nothing Then
    '            DV.Dispose()
    '        End If
    '        DV = Nothing

    '    End Try
    'End Sub

    Private Sub HistoryButton_Click(sender As Object, e As EventArgs) Handles HistoryButton.Click
        Dim HistoryF As RegMasterHistoryForm

        Try

            HistoryF = New RegMasterHistoryForm
            HistoryF.FamilyID = _FamilyID
            HistoryF.RelationID = -1
            HistoryF.Mode = REGMasterHistoryMode.SpecialHours
            HistoryF.ShowDialog()

            HistoryF.Close()

        Catch ex As Exception
            Throw
        Finally
            HistoryF.Dispose()
            HistoryF = Nothing
        End Try

    End Sub
#End Region

#Region "Formatting"
    Private Sub BindingCompleteEventHandler(ByVal sender As Object, ByVal e As System.Windows.Forms.BindingCompleteEventArgs)

        Try

            If Not e.BindingCompleteState = BindingCompleteState.Success Then
                MessageBox.Show("Control " & e.Binding.Control.Name & " " & e.ErrorText, "Problem converting data to database format", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try

    End Sub

    'Private Sub HoursBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)

    '    Try

    '        If IsDBNull(e.Value) = False AndAlso CStr(e.Value).Trim <> "" Then
    '            e.Value = Format(e.Value, "0.00")
    '        End If

    '    Catch ex As Exception

    '            Throw
    '    End Try
    'End Sub

    Private Sub EAHBS_ListChanged(sender As Object, e As ListChangedEventArgs) Handles _EAHBS.ListChanged

        Dim BS As BindingSource

        Try
            BS = DirectCast(_EAHBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") RS(" & If(BS.Position < 0 OrElse BS.Current Is Nothing, "N/A", DirectCast(BS.Current, System.Data.DataRowView).Row.RowState.ToString) & ") O(" & e.OldIndex.ToString & ") N(" & e.NewIndex.ToString & ") CT(" & e.ListChangedType.ToString & ") SEL(" & If(BS Is Nothing OrElse BS.Count < 1 OrElse BS.Position < 0 OrElse EligSpecialAcctDataGrid.DataSource Is Nothing, "N/A", EligSpecialAcctDataGrid.IsSelected(BS.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Select Case e.ListChangedType
                Case ListChangedType.ItemDeleted 'account for rows deleted due to a cancel action

                Case ListChangedType.ItemMoved

                    Select Case DirectCast(BS.Current, System.Data.DataRowView).Row.RowState
                        Case DataRowState.Modified

                        Case DataRowState.Added

                            If e.OldIndex <> e.NewIndex OrElse (BS.Position > -1 AndAlso BS.Count = 1) Then
                                EligSpecialAcctDataGrid.Select(BS.Position)
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
                        If e.NewIndex > -1 Then
                            BS.Position = e.NewIndex
                            BS.ResetCurrentItem()
                        End If
                    End If

                Case ListChangedType.ItemAdded 'includes items reincluded when filters change

                    If BS.Position <> e.NewIndex OrElse BS.Position > -1 AndAlso BS.Count = 1 Then 'first item added
                        If e.NewIndex > -1 Then BS.Position = e.NewIndex
                        If e.NewIndex > -1 Then EligSpecialAcctDataGrid.Select(e.NewIndex)
                    End If

            End Select

            ProcessControls(CType(grpEditPanel, Object), _ReadOnlyMode)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") RS(" & If(BS.Position < 0 OrElse BS.Current Is Nothing, "N/A", DirectCast(BS.Current, System.Data.DataRowView).Row.RowState.ToString) & ") O(" & e.OldIndex.ToString & ") N(" & e.NewIndex.ToString & ") CT(" & e.ListChangedType.ToString & ") SEL(" & If(BS Is Nothing OrElse BS.Count < 1 OrElse BS.Position < 0 OrElse EligSpecialAcctDataGrid.DataSource Is Nothing, "N/A", EligSpecialAcctDataGrid.IsSelected(BS.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub EAHBS_CurrentChanged(sender As Object, e As EventArgs) Handles _EAHBS.CurrentChanged
        Dim BS As BindingSource

        Try

            BS = DirectCast(_EAHBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If BS Is Nothing OrElse BS.Position < 0 OrElse BS.Count < 1 Then Return 'no current row, most likely an item filter value was changed

            Dim DR As DataRow = DirectCast(BS.Current, DataRowView).Row

            ProcessControls(CType(grpEditPanel, Object), _ReadOnlyMode)

        Catch ex As Exception
            Throw
        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub SetUIElements(readOnlyMode As Boolean)
        Dim DR As DataRow
        Dim DRs As DataRow()

        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

        Try

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            grpEditPanel.SuspendLayout()

            If Not readOnlyMode Then readOnlyMode = _REGMReadOnlyAccess
            If UFCWGeneralAD.REGMVendorAccess Then _ViewHistory = False

            If _EAHBS IsNot Nothing AndAlso _EAHBS.Position > -1 AndAlso _EAHBS.Current IsNot Nothing AndAlso _EAHBS.Count > 0 Then
                DR = CType(_EAHBS.Current, DataRowView).Row
            End If

            If DR IsNot Nothing AndAlso (_ViewHistory Is Nothing OrElse _ViewHistory) Then
                Me.HistoryButton.Enabled = True
                Me.HistoryButton.Visible = True
            End If

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

            Me.CalculateEligibilityActionButton.Visible = Not readOnlyMode
            Me.CalculateEligibilityActionButton.Enabled = False

            Dim AddedQuery = (From Added As DataRow In _EAHDS.Tables("ELIG_ACCT_HOURS").AsEnumerable()
                              Where Added.RowState = DataRowState.Added)

            ProcessSubControls(CType(grpEditPanel, Object), True, True) 'lock everything down except buttons

            If Not AddedQuery.Any Then
                eligDateTimePicker.Enabled = True
            End If

            lblmessage.Text = ""

            If readOnlyMode Then

                AddActionButton.Visible = False
                ModifyActionButton.Visible = False
                DeleteActionButton.Visible = False

                CancelActionButton.Visible = False
                SaveActionButton.Visible = False

                If DR IsNot Nothing AndAlso cmbSpecialAcct.SelectedIndex > -1 AndAlso Not IsDBNull(CType(cmbSpecialAcct.SelectedItem, DataRowView)("HOURS_PER_DAY")) AndAlso IsNumber(txtHours.Text) Then
                    txtDays.Text = (CDec(txtHours.Text) / CDec(CType(cmbSpecialAcct.SelectedItem, DataRowView)("HOURS_PER_DAY"))).ToString("0.00")
                Else
                    txtDays.Text = ""
                End If

            Else

                AddActionButton.Visible = True
                ModifyActionButton.Visible = True
                CalculateEligibilityActionButton.Visible = True

                If _REGMSupervisor Then
                    DeleteActionButton.Visible = True
                End If

                CancelActionButton.Visible = True
                SaveActionButton.Visible = True
                eligDateTimePicker.Enabled = True

                If DR IsNot Nothing Then 'based upon row status / content decide how to present controls

                    If _EAHDS.HasChanges Then
                        Me.SaveActionButton.Enabled = True
                        Me.CancelActionButton.Enabled = True
                    End If

                    If DR.RowState = DataRowState.Added OrElse DR.RowState = DataRowState.Modified Then

                        eligDateTimePicker.Enabled = False

                        If cmbSpecialAcct.SelectedIndex > -1 Then

                            lblmessage.Text = "You are giving eligibilty for the work period " & Format(DateAdd(DateInterval.Month, -CInt(CType(cmbSpecialAcct.SelectedItem, DataRowView)("SKIP_MONTH_FACTOR")), eligDateTimePicker.Value.Date), "MMM yyyy") & " and eligibility Period " & eligDateTimePicker.Text

                            If CBool(CType(cmbSpecialAcct.SelectedItem, DataRowView)("ENTER_HOURS_SW")) Then
                                If CType(cmbSpecialAcct.SelectedItem, DataRowView)("DESCRIPTION").ToString.ToUpper.Contains("HOURS") Then
                                    txtHours.ReadOnly = False
                                    txtDays.ReadOnly = True
                                Else
                                    txtDays.ReadOnly = False
                                    txtHours.ReadOnly = True
                                End If
                            End If

                        End If
                    End If

                    If txtDays.Text.Trim.Length < 1 AndAlso cmbSpecialAcct.SelectedIndex > -1 AndAlso Not IsDBNull(CType(cmbSpecialAcct.SelectedItem, DataRowView)("HOURS_PER_DAY")) AndAlso IsNumber(txtHours.Text) Then
                        txtDays.Text = (CDec(txtHours.Text) / CDec(CType(cmbSpecialAcct.SelectedItem, DataRowView)("HOURS_PER_DAY"))).ToString("0.00")
                    ElseIf cmbSpecialAcct.SelectedIndex < 0 OrElse IsDBNull(CType(cmbSpecialAcct.SelectedItem, DataRowView)("HOURS_PER_DAY")) OrElse CInt(CType(cmbSpecialAcct.SelectedItem, DataRowView)("HOURS_PER_DAY")) = 0 Then
                        txtDays.Text = ""
                    End If

                    If DR.RowState = DataRowState.Added Then

                        'eligDateTimePicker.Enabled = False

                        cmbSpecialAcct.ReadOnly = False
                        txtSpecialRemarks.ReadOnly = False

                        AddActionButton.Enabled = False
                        ModifyActionButton.Enabled = False
                        DeleteActionButton.Enabled = False

                    ElseIf DR.RowState = DataRowState.Modified Then

                        cmbSpecialAcct.ReadOnly = False
                        txtSpecialRemarks.ReadOnly = False

                        AddActionButton.Enabled = False
                        ModifyActionButton.Enabled = False
                        DeleteActionButton.Enabled = False

                    ElseIf DR.RowState = DataRowState.Unchanged Then

                        SaveActionButton.Enabled = False

                        CalculateEligibilityActionButton.Enabled = True

                        If Not AddedQuery.Any Then
                            AddActionButton.Enabled = True
                        End If

                        If DR("SHORT_DESC").ToString.Trim.Length > 0 Then

                            ModifyActionButton.Enabled = True
                            DeleteActionButton.Enabled = True

                        End If

                    Else

                        AddActionButton.Enabled = False
                        ModifyActionButton.Enabled = False

                    End If

                Else
                    AddActionButton.Enabled = True

                    ModifyActionButton.Enabled = False
                    DeleteActionButton.Enabled = False

                    txtDays.Text = ""

                End If
            End If

            grpEditPanel.ResumeLayout() 'needed to ensure transparent controls child controls draw correctly 
            grpEditPanel.Refresh()

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw

        Finally
            Me.AutoValidate = HoldAutoValidate
        End Try

    End Sub

    Public Sub ProcessControls()
        'Impact Entire control

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ProcessControls(Me, _ReadOnlyMode, False)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
        End Try

    End Sub

    Private Sub ProcessControls(ByRef controlContainer As Object, readOnlyMode As Boolean, Optional excludeButtons As Boolean = True)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ProcessControlsInContainer(CType(controlContainer, Object), readOnlyMode, excludeButtons)

            SetUIElements(readOnlyMode)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
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
                        'CType(ctrl, TextBox).TabStop = Not readOnlyMode
                    End If
                ElseIf TypeOf (ctrl) Is ExComboBox Then
                    If CType(ctrl, ExComboBox).ReadOnly <> readOnlyMode Then
                        CType(ctrl, ExComboBox).ReadOnly = readOnlyMode
                        'CType(ctrl, ExComboBox).TabStop = Not readOnlyMode
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

    Private Sub EAHDS_RowChanged(sender As Object, e As DataRowChangeEventArgs)
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

    Private Sub EAHDS_RowChanging(sender As Object, e As DataRowChangeEventArgs)
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

    Private Sub EAHDS_ColumnChanged(sender As Object, e As DataColumnChangeEventArgs)
        Dim BS As BindingSource

        Try

            BS = DirectCast(_EAHBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

    Private Sub EAHDS_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs)
        Dim BS As BindingSource

        Try

            BS = DirectCast(_EAHBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))


        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

    Private Sub txtSpecialRemarks_Validating(sender As Object, e As CancelEventArgs) Handles txtSpecialRemarks.Validating

        Dim Tbox As TextBox = CType(sender, TextBox)
        Dim DR As DataRow

        Try

            ErrorProvider1.ClearError(Tbox)

            If _EAHBS Is Nothing OrElse _EAHBS.Position < 0 OrElse Tbox.ReadOnly Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = CType(_EAHBS.Current, DataRowView).Row

            If Tbox.Text.Trim.Length < 1 Then
                ErrorProvider1.SetErrorWithTracking(Tbox, " Remarks required.")
            End If

            If ErrorProvider1.GetError(Tbox).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
                Return
            End If

        Catch ex As Exception

                Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

#End Region

End Class