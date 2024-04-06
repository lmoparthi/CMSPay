Option Strict On

Imports System.ComponentModel
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Data.Common
Imports DDTek.DB2
Imports System.Reflection

Public Class AlertControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private Shared _DomainUser As String = SystemInformation.UserName
    Private Shared _APPKEY As String = "UFCW\RegMaster\"

    Public Event SavingRecord()
    Public Event CompleteSavingRecord()

    Private _FamilyID As Integer?
    Private _RelationID As Integer?
    Private _ReadOnlyMode As Boolean = True

    Private WithEvents _TotalAlertDS As New DataSet
    Private WithEvents _AlertBS As BindingSource
    Private WithEvents _AlertDS As New DataSet

    ReadOnly _REGMEmployeeAccess As Boolean = UFCWGeneralAD.REGMEmployeeAccess
    ReadOnly _REGMReadOnlyAccess As Boolean = UFCWGeneralAD.REGMReadOnlyAccess
    ReadOnly _REGMVendorAccess As Boolean = UFCWGeneralAD.REGMVendorAccess
    ReadOnly _REGMRegMasterDeleteAccess As Boolean = UFCWGeneralAD.REGMRegMasterDeleteAccess
    ReadOnly _REGMLifeEventDeleteAccess As Boolean = UFCWGeneralAD.REGMLifeEventDeleteAccess
    ReadOnly _REGMSupervisor As Boolean = UFCWGeneralAD.REGMSupervisor

    Private _ChangedAlertRowsDS As DataSet
    Private _HoverCell As New DataGridCell

    Private _DisplayColumnNamesDV As DataView
    Private _ViewHistory As Boolean?
    Private _ButtonName As String = ""
    Private _CallingAppID As Integer = 0     '''' In the DB  CS=1 , REG M=2, for Both = NULL
    Private _RegAlertTerminationValuesDT As DataTable
    Private _RegAlertReasonValuesDT As DataTable
    Private _RelationIDs As ArrayList = Nothing
    Private _CallingAppName As String = System.Configuration.ConfigurationManager.AppSettings("AppName")

    Private _Disposed As Boolean = False

    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.

            If AlertDataGrid IsNot Nothing Then
                AlertDataGrid.Dispose()
            End If
            AlertDataGrid = Nothing

            If _TotalAlertDS IsNot Nothing Then
                _TotalAlertDS.Dispose()
            End If
            _TotalAlertDS = Nothing

            If _ChangedAlertRowsDS IsNot Nothing Then
                _ChangedAlertRowsDS.Dispose()
            End If
            _ChangedAlertRowsDS = Nothing

            If _RegAlertTerminationValuesDT IsNot Nothing Then
                _RegAlertTerminationValuesDT.Dispose()
            End If
            _RegAlertTerminationValuesDT = Nothing

            If _RegAlertReasonValuesDT IsNot Nothing Then
                _RegAlertReasonValuesDT.Dispose()
            End If
            _RegAlertReasonValuesDT = Nothing

            If components IsNot Nothing Then
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

    <System.ComponentModel.Browsable(True), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Determines if control is in Read Only Mode.")>
    Public Property ReadOnlyMode() As Boolean
        Get
            Return _ReadOnlyMode
        End Get
        Set(ByVal value As Boolean)
            _ReadOnlyMode = value
            ProcessControls(CType(grpEditPanel, Object), _ReadOnlyMode)
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the FamilyID of the Document.")>
    Public Property FamilyID() As Integer
        Get
            Return CInt(_FamilyID)
        End Get
        Set(ByVal value As Integer)
            _FamilyID = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the RelationID of the Document.")>
    Public Property RelationID() As Integer
        Get
            Return CInt(_RelationID)
        End Get
        Set(ByVal value As Integer)
            _RelationID = value
        End Set
    End Property

    <Browsable(True), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Determines if History can be viewed.")>
    Public Property VisibleHistory() As Boolean
        Get
            Return CBool(If(_ViewHistory Is Nothing, True, _ViewHistory))
        End Get
        Set(ByVal value As Boolean)

            If _ViewHistory Is Nothing OrElse _ViewHistory <> value Then
                _ViewHistory = value

                If _ViewHistory Is Nothing OrElse _ViewHistory = True Then
                    HistoryButton.Enabled = True
                    HistoryButton.Visible = True
                Else
                    HistoryButton.Enabled = False
                    HistoryButton.Visible = False
                End If
            End If

        End Set
    End Property

    Public Property CallingAppID() As Integer
        Get
            Return CInt(_CallingAppID)
        End Get
        Set(ByVal value As Integer)
            _CallingAppID = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        Dim designMode As Boolean = (LicenseManager.UsageMode = LicenseUsageMode.Designtime)

        If Not designMode Then
            PreLoad()
            txtThruDate.Text = ""

            If _CallingAppName IsNot Nothing AndAlso _CallingAppName.Length > 0 AndAlso _CallingAppName.ToUpper = "ELIGIBILITY" Then

            Else
                _ViewHistory = False
            End If
        End If

    End Sub

    Public Sub New(ByVal familyID As Integer, ByVal relationID As Integer?)
        Me.New()

        _FamilyID = familyID
        _RelationID = relationID
    End Sub

    Public Sub PreLoad()
        LoadAlertValues()
        LoadAlertTerminationValues()
    End Sub
#End Region

#Region "Form\Button Events"

    Private Sub cmbAlert_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbAlert.SelectedIndexChanged
        Dim DR As DataRow
        Dim DV As DataView

        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Try

            If Not CBox.Enabled OrElse CBox.ReadOnly OrElse _AlertBS Is Nothing OrElse _AlertBS.Position < 0 OrElse _AlertBS.Count < 1 OrElse CBox.SelectedIndex < 0 Then Return

            DR = DirectCast(_AlertBS.Current, DataRowView).Row

            If IsDBNull(DR("ALERT_REASON")) OrElse (Not IsDBNull(DR("ALERT_REASON")) AndAlso Not DR("ALERT_REASON").ToString.Equals(If(CBox.SelectedValue Is Nothing, DBNull.Value, CBox.SelectedValue).ToString)) Then
                DR("ALERT_REASON") = If(CBox.SelectedValue Is Nothing, DBNull.Value, CBox.SelectedValue)
            End If

            '' enable/ disable txtpasscode basing on the selected ALERT_REASON

            If Not IsDBNull(DR("ALERT_REASON")) Then

                DV = New DataView(_RegAlertReasonValuesDT, "ALERT_REASON = " & CStr(DR("ALERT_REASON")), "", DataViewRowState.CurrentRows)
                If DV.Count > 0 AndAlso CInt(DV(0)("REMARKS_NEEDED_SW")) = 1 Then
                    txtpasscode.ReadOnly = False
                Else
                    txtpasscode.ReadOnly = True
                End If

                '' cheking if it is family level alert or  not . ie., If family level alert the relation id for the alert is always 0 . 
                If DV.Count > 0 AndAlso CInt(DV(0)("FAMILY_ALERT")) = 1 AndAlso CInt(DR("RELATION_ID")) <> 0 Then
                    DR("RELATION_ID") = 0
                End If

            End If

        Catch ex As Exception

                Throw
        Finally
            If DV IsNot Nothing Then
                DV.Dispose()
            End If
            DV = Nothing

        End Try
    End Sub

    Private Sub cmbTermReason_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbTermReason.SelectedIndexChanged

        Dim DR As DataRow
        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Try

            If Not CBox.Enabled OrElse CBox.ReadOnly OrElse _AlertBS Is Nothing OrElse _AlertBS.Position < 0 OrElse _AlertBS.Count < 1 OrElse CBox.SelectedIndex < 0 Then Return

            DR = DirectCast(_AlertBS.Current, DataRowView).Row


        Catch ex As Exception

                Throw
        Finally
        End Try
    End Sub

    Private Sub AddEventButton_Click(sender As System.Object, e As System.EventArgs) Handles AddEventButton.Click
        Dim DRs As DataRow()

        Try

            AddEventButton.Enabled = False
            TerminateEventButton.Enabled = False
            ModifyEventButton.Enabled = False
            VoidEventButton.Enabled = False

            ProcessSubControls(CType(grpEditPanel, Object), False, True)

            chkSameAlert.Checked = False
            chkSameAlert.Enabled = False
            chkSameAlert.Visible = False

            If _RelationID IsNot Nothing AndAlso Not _RelationID = 0 AndAlso _RelationID > 0 Then
                DRs = _AlertDS.Tables("REG_ALERTS").Select("RELATION_ID= 0")
                If DRs.Length > 0 Then
                    For Each TypeDR As DataRow In DRs
                        _AlertDS.Tables("REG_ALERTS").Rows.Remove(TypeDR) ''Remove the default member alert(s) if user added new rec.
                    Next
                End If
            End If

            If _RelationID Is Nothing Then _RelationID = CInt(cmbRelationID.Text)

            LoadPHIAlertValuesforApplication()

            cmbAlert.Focus()

            AddALERTLine()

            cmbAlert.SelectedIndex = -1
            cmbTermReason.SelectedIndex = -1

        Catch ex As Exception

                Throw
        Finally
            If _AlertBS.Position > -1 Then AlertDataGrid.Select(_AlertBS.Position)
        End Try
    End Sub

    Private Sub btnTerminate_Click(sender As System.Object, e As System.EventArgs) Handles TerminateEventButton.Click

        If Not _REGMSupervisor AndAlso Not UFCWGeneralAD.CMSCanAudit Then
            MessageBox.Show("You are not authorized to Terminate Alert", "Access Restricted", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If

        If _AlertBS Is Nothing OrElse _AlertBS.Position < 0 OrElse _AlertBS.Count < 1 Then Return

        Try
            cmbTermReason.ReadOnly = False
            txtThruDate.ReadOnly = False
            txtComments.ReadOnly = False

            cmbAlert.ReadOnly = True
            cmbTermReason.ReadOnly = True

            txtFromDate.ReadOnly = True
            txtpasscode.ReadOnly = True

            CancelEventButton.Enabled = True
            SaveEventButton.Enabled = True

            TerminateEventButton.Enabled = False
            VoidEventButton.Enabled = False
            AddEventButton.Enabled = False

            _ButtonName = "TERMINATE"

        Catch ex As Exception

                Throw
        End Try
    End Sub

    Private Sub HistoryButton_Click(sender As System.Object, e As System.EventArgs) Handles HistoryButton.Click

        Dim HistoryFrm As AlertHistory
        Try

            HistoryFrm = New AlertHistory
            HistoryFrm.FamilyID = CInt(_FamilyID)
            HistoryFrm.RelationID = CInt(_RelationID)
            HistoryFrm.ShowDialog(Me)

        Catch ex As Exception

                Throw
        Finally

            If HistoryFrm IsNot Nothing Then HistoryFrm.Dispose()
            HistoryFrm = Nothing
        End Try
    End Sub

    Private Sub CancelEventButton_Click(sender As System.Object, e As System.EventArgs) Handles CancelEventButton.Click

        If _FamilyID Is Nothing OrElse _RelationID Is Nothing Then Return

        Dim Result As DialogResult = DialogResult.None
        Dim ChangedRMRows As DataSet

        Try
            ChangedRMRows = _AlertDS.GetChanges()

            Result = MessageBox.Show(Me, "Do you want to Cancel the changes?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If Result = DialogResult.Yes Then

                _AlertBS.EndEdit()

                AddEventButton.Enabled = True
                ModifyEventButton.Enabled = True
                TerminateEventButton.Enabled = True
                VoidEventButton.Enabled = True

                If _AlertDS.HasChanges Then
                    _AlertDS.Tables("REG_ALERTS").RejectChanges()
                End If

                ClearErrors()
                CancelEventButton.Enabled = False
                SaveEventButton.Enabled = False

                LoadALERTS() ''refresh grid

            ElseIf Result = DialogResult.No Then
                CancelEventButton.Enabled = True
                SaveEventButton.Enabled = True
            End If
        Catch ex As Exception

        Finally
            _ButtonName = ""
        End Try
    End Sub

    Private Sub SaveEventButton_Click(sender As System.Object, e As System.EventArgs) Handles SaveEventButton.Click

        Try
            SaveEventButton.Enabled = False 'this will reenable if the underlying dataset changes
            CancelEventButton.Enabled = False 'this will reenable if the underlying dataset changes

            _AlertBS.EndEdit()

            If VerifyALERTChanges() Then
                SaveEventButton.Enabled = True
                CancelEventButton.Enabled = True
                AddEventButton.Enabled = False
                Exit Sub
            End If

            _ChangedAlertRowsDS = _AlertDS.GetChanges()

            If _ChangedAlertRowsDS Is Nothing Then     '' when new row added,deleted then cancel, save buttons are enabled
                MessageBox.Show("Nothing to Save", "No Changes", MessageBoxButtons.OK)
                SaveEventButton.Enabled = False
                CancelEventButton.Enabled = False
                Exit Sub
            End If

            If _ChangedAlertRowsDS IsNot Nothing Then

                RaiseEvent SavingRecord()

                If ApplyALERTChanges() Then
                    MessageBox.Show("Alert Record Saved Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    _AlertDS.AcceptChanges()

                    ClearAll()

                    LoadALERTS()

                    AddEventButton.Enabled = True
                Else
                    SaveEventButton.Enabled = True
                    CancelEventButton.Enabled = True
                End If

            End If

        Catch ex As Exception

            SaveEventButton.Enabled = True 'note, this is done in the error handler just in case a failure occurs
            CancelEventButton.Enabled = True 'note, this is done in the error handler just in case a failure occurs


                Throw

        Finally

            If ErrorProviderErrorsList(ErrorProvider1).Length = 0 AndAlso _AlertBS.Count > 0 Then
                _AlertBS.Position = 0
                AlertDataGrid.Select(_AlertBS.Position)
            End If

            RaiseEvent CompleteSavingRecord()
            _ButtonName = ""
        End Try

    End Sub

    Private Sub VoidEventButton_Click(sender As System.Object, e As System.EventArgs) Handles VoidEventButton.Click

        If Not _REGMSupervisor AndAlso Not UFCWGeneralAD.CMSCanAudit Then
            MessageBox.Show("You are not authorized to Void Alert", "Access Restricted", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If

        If _AlertBS Is Nothing OrElse _AlertBS.Position < 0 OrElse _AlertBS.Count < 1 Then Return

        cmbAlert.ReadOnly = True

        txtpasscode.ReadOnly = True
        txtThruDate.ReadOnly = True
        txtFromDate.ReadOnly = True

        cmbTermReason.ReadOnly = False

        CancelEventButton.Enabled = True
        SaveEventButton.Enabled = True
        HistoryButton.Enabled = True

        TerminateEventButton.Enabled = False
        VoidEventButton.Enabled = False
        AddEventButton.Enabled = False

        _ButtonName = "VOID"
    End Sub

    Private Sub txtFromDt_Leave(sender As Object, e As System.EventArgs) Handles txtFromDate.Leave
        Me.Focus()
    End Sub

    Private Sub ModifyEventButton_Click(sender As System.Object, e As System.EventArgs) Handles ModifyEventButton.Click

        Dim DR As DataRow

        If _AlertBS Is Nothing OrElse _AlertBS.Position < 0 OrElse _AlertBS.Count < 1 Then Return

        Try

            ModifyEventButton.Enabled = False
            AddEventButton.Enabled = False
            VoidEventButton.Enabled = False
            TerminateEventButton.Enabled = False

            DR = DirectCast(_AlertBS.Current, DataRowView).Row

            DR.BeginEdit() 'sets modified status.
            DR.EndEdit() 'sets modified status.

            ProcessControls(CType(grpEditPanel, Object), _ReadOnlyMode)

        Catch ex As Exception

                Throw
        Finally
            DR = Nothing
        End Try
    End Sub

    Private Sub AlertDataGrid_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles AlertDataGrid.MouseMove

        Dim HTI As DataGrid.HitTestInfo
        Dim DRArray As DataRow()

        Try
            HTI = CType(sender, DataGridCustom).HitTest(e.X, e.Y)

            ' Do not display hover text if it is a drag event 
            If (e.Button <> MouseButtons.Left) Then
                ' Check if the target is a different cell from the previous one 
                If HTI.Type = DataGrid.HitTestType.Cell AndAlso HTI.Row <> _HoverCell.RowNumber OrElse HTI.Column <> _HoverCell.ColumnNumber Then
                    ' Store the new hit row 
                    _HoverCell.RowNumber = HTI.Row
                    _HoverCell.ColumnNumber = HTI.Column
                End If
            End If

            If HTI.Type = DataGrid.HitTestType.Cell Then
                If _HoverCell.RowNumber > -1 AndAlso _HoverCell.RowNumber <= (CType(sender, DataGridCustom).GetGridRowCount) Then
                    If AlertDataGrid.GetColumnMapping(_HoverCell.ColumnNumber) = "TERMINATION_REASON" Then
                        If _HoverCell.RowNumber = -1 OrElse _HoverCell.ColumnNumber = -1 Then
                            Exit Sub
                        End If

                        If IsDBNull(AlertDataGrid.Item(HTI.Row, HTI.Column)) OrElse CStr(AlertDataGrid.Item(_HoverCell)).Trim.Length < 1 Then
                            Exit Sub
                        End If

                        DRArray = _RegAlertTerminationValuesDT.Select("ALERT_TERM_REASON =" & CStr(AlertDataGrid.Item(_HoverCell)))
                        If DRArray IsNot Nothing AndAlso DRArray.Length > 0 Then
                            If CStr(AlertDataGrid.Item(_HoverCell)) = CStr(DRArray(0).Item("ALERT_TERM_REASON")) Then
                                ToolTip1.Active = True
                                ToolTip1.SetToolTip(AlertDataGrid, CStr(DRArray(0).Item("ALERT_TERM_REASON_DESC")))
                            Else
                                ToolTip1.Active = False
                                ToolTip1.SetToolTip(AlertDataGrid, "")
                            End If
                        End If

                    Else
                        ToolTip1.Active = False
                        ToolTip1.SetToolTip(AlertDataGrid, "")
                    End If
                Else
                    ToolTip1.Active = False
                    ToolTip1.SetToolTip(AlertDataGrid, "")
                End If
            End If
        Catch ex As Exception

                Throw
        Finally
            HTI = Nothing
        End Try
    End Sub

    Private Sub cmbrelationid_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbRelationID.SelectedIndexChanged

        Dim DR As DataRow

        Try
            If _AlertBS Is Nothing OrElse _AlertBS.Position < 0 OrElse _AlertBS.Count < 1 Then Return

            DR = DirectCast(_AlertBS.Current, DataRowView).Row

            If IsDBNull(DR("RELATION_ID")) OrElse (Not IsDBNull(DR("RELATION_ID")) AndAlso Not DR("RELATION_ID").ToString.Equals(If(IsNothing(CType(sender, ComboBox).SelectedValue), DBNull.Value, CType(sender, ComboBox).SelectedValue))) Then
                If Not IsDBNull(DR("RELATION_ID")) Then
                    DR("RELATION_ID") = If(IsNothing(CType(sender, ComboBox).SelectedValue), 0, CType(sender, ComboBox).SelectedValue)
                End If
            End If

        Catch ex As Exception

                Throw
        Finally
        End Try
    End Sub

#End Region

#Region "Custom Subs\Functions"

    Public Sub LoadALERTS(ByVal familyID As Integer, ByVal relationID As Integer?, ByVal appID As Integer, ByVal relationIDs As ArrayList)
        Try
            _FamilyID = familyID
            _RelationID = relationID
            _CallingAppID = appID
            _RelationIDs = relationIDs

            ProcessControls(Me, True, False)

            LoadALERTS()

        Catch ex As Exception

                Throw
        End Try
    End Sub

    Public Sub LoadALERTS(ByVal familyID As Integer, ByVal relationID As Integer?, ByVal dt As DataTable)
        Try

            _TotalAlertDS = New DataSet
            _TotalAlertDS.Tables.Add(dt.Clone)

            _FamilyID = familyID
            _RelationID = relationID

            For Each TypeDR As DataRow In dt.Rows
                _TotalAlertDS.Tables("REG_ALERTS").ImportRow(TypeDR)  '' Filter the rows basing on seleted record from regemail grid
            Next

            If dt.Rows.Count > 0 Then LoadALERTS()

        Catch ex As Exception

                Throw
        End Try
    End Sub

    Public Sub LoadALERTS()

        Dim DRs As DataRow()
        Dim AlertDRs As DataRow()
        Dim RelationArray As New ArrayList

        Try

            ClearErrors()
            ClearDataBindings(Me)

            If cmbAlert.DataSource Is Nothing Then
                LoadAlertValues()
            End If

            If _RelationIDs IsNot Nothing Then LoadRelationID(_RelationIDs)

            If _TotalAlertDS IsNot Nothing AndAlso (_TotalAlertDS.Tables.Count = 0 OrElse _TotalAlertDS.Tables("REG_ALERTS").Rows.Count = 0) Then
                _TotalAlertDS = RegMasterDAL.RetrieveRegAlertsByFamilyID(CInt(_FamilyID), _TotalAlertDS)
            End If

            _AlertDS = New DataSet
            _AlertDS.Tables.Add(_TotalAlertDS.Tables("REG_ALERTS").Clone)

            DRs = _TotalAlertDS.Tables("REG_ALERTS").Select(If(_RelationID > -1, "RELATION_ID = " & _RelationID.ToString, "")) 'create copied cloned rows to remove reference to original data
            For Each DR As DataRow In DRs
                _AlertDS.Tables("REG_ALERTS").ImportRow(DR)
            Next

            If _TotalAlertDS.Tables("REG_ALERTS").Rows.Count > 0 AndAlso _RelationID > -1 Then
                DRs = _TotalAlertDS.Tables("REG_ALERTS").Select("RELATION_ID=" & _RelationID)

                If DRs.Length > 0 Then   '' Record exist for relation id
                    chkSameAlert.Visible = False
                Else
                    If _CallingAppID = 2 Then
                        DRs = _TotalAlertDS.Tables("REG_ALERTS").Select("RELATION_ID=0") ''Displaying Member Alert Always if dependent have no record
                        If DRs.Length > 0 Then chkSameAlert.Checked = True
                        chkSameAlert.ForeColor = Color.Red
                        chkSameAlert.Visible = True
                    End If
                End If
            End If

            AddHandler _AlertDS.Tables("REG_ALERTS").RowChanging, AddressOf DS_RowChanging
            AddHandler _AlertDS.Tables("REG_ALERTS").RowChanged, AddressOf DS_RowChanged
            AddHandler _AlertDS.Tables("REG_ALERTS").ColumnChanging, AddressOf DS_ColumnChanging
            AddHandler _AlertDS.Tables("REG_ALERTS").ColumnChanged, AddressOf DS_ColumnChanged

            _AlertBS = New BindingSource
            _AlertBS.DataMember = "REG_ALERTS"
            _AlertBS.DataSource = _AlertDS

            AlertDataGrid.DataMember = ""
            AlertDataGrid.DataSource = _AlertBS
            AlertDataGrid.SetTableStyle()

            LoadALERTDataBindings()

            AddHandler _AlertBS.CurrentItemChanged, AddressOf AlertBS_CurrentItemChanged
            AddHandler _AlertBS.PositionChanged, AddressOf AlertBS_PositionChanged
            AddHandler _AlertBS.CurrentChanged, AddressOf AlertBS_CurrentChanged

            If _RelationID > 0 Then
                AlertDataGrid.CaptionText = "Alerts for Relation (" & _RelationID.ToString & ")"
            Else
                AlertDataGrid.CaptionText = "Alerts for Family (" & _FamilyID.ToString & ")"
            End If

            cmbRelationID.Text = _RelationID.ToString

            If _CallingAppID = 2 Then
                cmbRelationID.Visible = False
                lblRelationID.Visible = False
            End If

        Catch ex As Exception

                Throw
        Finally
            DRs = Nothing
            AlertDRs = Nothing
            If _AlertBS IsNot Nothing Then
                If _AlertBS.Position > -1 Then AlertDataGrid.Select(_AlertBS.Position)
            End If
        End Try
    End Sub

    Public Sub ProcessControls()
        'Impact Entire control

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ProcessControls(Me, _ReadOnlyMode, False)

        Catch ex As Exception

                Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try


    End Sub

    Private Sub ProcessControls(ByRef controlContainer As Object, readOnlyMode As Boolean, Optional excludeButtons As Boolean = True)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ProcessControlsInContainer(CType(controlContainer, Object), readOnlyMode, excludeButtons)

            SetUIElements(readOnlyMode)

        Catch ex As Exception

                Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub
    Private Sub SetUIElements(readOnlyMode As Boolean)
        Dim DR As DataRow
        Dim DRs As DataRow()

        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

        Try

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

            grpEditPanel.SuspendLayout()

            If Not readOnlyMode Then readOnlyMode = _REGMReadOnlyAccess
            If _REGMVendorAccess Then _ViewHistory = False

            If _AlertBS IsNot Nothing AndAlso _AlertBS.Position > -1 AndAlso _AlertBS.Current IsNot Nothing AndAlso _AlertBS.Count > 0 Then
                DR = CType(_AlertBS.Current, DataRowView).Row
            End If

            If _ViewHistory AndAlso _FamilyID IsNot Nothing AndAlso _FamilyID > -1 Then
                Me.HistoryButton.Enabled = True
                Me.HistoryButton.Visible = True
            End If

            CancelEventButton.Enabled = False
            SaveEventButton.Enabled = False

            ProcessSubControls(CType(grpEditPanel, Object), True, True) 'lock everything down except buttons

            If readOnlyMode OrElse _FamilyID Is Nothing OrElse _RelationID Is Nothing OrElse _RelationID < 0 Then

                AddEventButton.Visible = False
                ModifyEventButton.Visible = False
                TerminateEventButton.Visible = False
                VoidEventButton.Visible = False

                CancelEventButton.Visible = False
                SaveEventButton.Visible = False

            Else

                AddEventButton.Visible = True
                ModifyEventButton.Visible = True

                If _REGMSupervisor Then
                    TerminateEventButton.Visible = True
                    VoidEventButton.Visible = True
                End If

                CancelEventButton.Visible = True
                SaveEventButton.Visible = True

                If DR IsNot Nothing Then 'based upon row status / content decide how to present controls

                    If DR.RowState = DataRowState.Added Then

                        cmbRelationID.ReadOnly = True

                        If _CallingAppID = 2 Then
                            cmbRelationID.Visible = False
                            lblRelationID.Visible = False
                        End If

                        txtFromDate.ReadOnly = False
                        txtThruDate.ReadOnly = True
                        cmbAlert.ReadOnly = False
                        cmbTermReason.ReadOnly = True

                        AddEventButton.Enabled = False
                        ModifyEventButton.Enabled = False
                        TerminateEventButton.Enabled = False

                    ElseIf DR.RowState = DataRowState.Modified Then

                        txtpasscode.ReadOnly = True

                        If (Not CStr(DR("THRU_DATE")) = "12/31/9999") AndAlso (CDate((DR("THRU_DATE"))) >= UFCWGeneral.NowDate) Then
                            txtThruDate.ReadOnly = False
                        Else
                            txtThruDate.ReadOnly = True
                        End If

                        txtComments.ReadOnly = False
                        txtFromDate.ReadOnly = False
                        cmbAlert.ReadOnly = False
                        cmbTermReason.ReadOnly = False

                        AddEventButton.Enabled = False
                        ModifyEventButton.Enabled = False
                        TerminateEventButton.Enabled = False

                    ElseIf DR.RowState = DataRowState.Unchanged Then

                        AddEventButton.Enabled = True

                        If _AlertDS.Tables("REG_ALERTS") IsNot Nothing AndAlso _AlertDS.Tables("REG_ALERTS").Select("RELATION_ID=" & _RelationID).Length > 0 Then

                            ModifyEventButton.Enabled = True
                            TerminateEventButton.Enabled = True

                        ElseIf _AlertDS.Tables("REG_ALERTS") IsNot Nothing AndAlso _AlertDS.Tables("REG_ALERTS").Select("RELATION_ID=" & _RelationID).Length = 0 Then

                            ModifyEventButton.Enabled = False
                            TerminateEventButton.Enabled = False

                        End If

                    Else

                        AddEventButton.Enabled = False
                        ModifyEventButton.Enabled = False

                    End If

                    If _AlertDS.HasChanges Then
                        CancelEventButton.Enabled = True
                        SaveEventButton.Enabled = True
                    End If

                Else
                    AddEventButton.Enabled = True

                    ModifyEventButton.Enabled = False
                    TerminateEventButton.Enabled = False

                End If
            End If

            grpEditPanel.ResumeLayout() 'needed to ensure transparent controls child controls draw correctly

        Catch ex As Exception

                Throw

        Finally
            Me.AutoValidate = HoldAutoValidate
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

            UFCWLastKeyData.TEXT = "Control: " & CtrlName


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

    Private Sub LoadAlertValues()
        Dim DT As DataTable
        Dim DV As DataView

        Try
            DT = RegMasterDAL.RetrieveRegAlertReasonValues()

            cmbAlert.DataSource = Nothing
            cmbAlert.Items.Clear()
            DT.Rows.Add(DT.NewRow)

            _RegAlertReasonValuesDT = DT

            DV = New DataView(DT, "", "ALERT_REASON", DataViewRowState.CurrentRows)
            cmbAlert.ValueMember = "ALERT_REASON"
            cmbAlert.DisplayMember = "ALERT_REASON_DESC"
            cmbAlert.DataSource = DV

        Catch ex As Exception

                Throw
        End Try
    End Sub

    Private Sub LoadPHIAlertValuesforApplication()
        Dim DT As DataTable
        Dim DV As DataView

        Try
            DT = RegMasterDAL.RetrieveRegAlertReasonValues()

            cmbAlert.DataSource = Nothing
            cmbAlert.Items.Clear()
            ' DT.Rows.Add(DT.NewRow)

            DV = New DataView(DT, "APPLICATION= " & _CallingAppID & " OR APPLICATION IS NULL", "ALERT_REASON", DataViewRowState.CurrentRows)
            cmbAlert.ValueMember = "ALERT_REASON"
            cmbAlert.DisplayMember = "ALERT_REASON_DESC"
            cmbAlert.DataSource = DV

        Catch ex As Exception

                Throw
        End Try
    End Sub

    Private Sub LoadAlertTerminationValues()
        Dim DT As DataTable
        Dim DV As DataView

        Try
            DT = RegMasterDAL.RetrieveRegAlertTerminationValues()

            cmbTermReason.DataSource = Nothing
            cmbTermReason.Items.Clear()
            DT.Rows.Add(DT.NewRow)

            _RegAlertTerminationValuesDT = DT

            DV = New DataView(DT, "", "ALERT_TERM_REASON", DataViewRowState.CurrentRows)
            cmbTermReason.ValueMember = "ALERT_TERM_REASON"
            cmbTermReason.DisplayMember = "ALERT_TERM_REASON_DESC"
            cmbTermReason.DataSource = DV

        Catch ex As Exception

                Throw
        End Try
    End Sub

    Private Sub LoadALERTDataBindings()
        Dim Bind As Binding

        Try

            txtFromDate.DataBindings.Clear()
            Bind = New Binding("Text", _AlertBS, "FROM_DATE", True)
            Bind.FormatString = "MM-dd-yyyy"
            AddHandler Bind.BindingComplete, AddressOf DateOnlyBinding_BindingComplete
            txtFromDate.DataBindings.Add(Bind)

            txtThruDate.DataBindings.Clear()
            Bind = New Binding("Text", _AlertBS, "THRU_DATE", True)
            Bind.FormatString = "MM-dd-yyyy"
            AddHandler Bind.BindingComplete, AddressOf DateOnlyBinding_BindingComplete
            txtThruDate.DataBindings.Add(Bind)

            txtpasscode.DataBindings.Clear()
            Bind = New Binding("Text", _AlertBS, "PASSPHRASE")
            txtpasscode.DataBindings.Add(Bind)

            txtComments.DataBindings.Clear()
            Bind = New Binding("Text", _AlertBS, "COMMENTS")
            txtComments.DataBindings.Add(Bind)

            cmbAlert.DataBindings.Clear()
            Bind = New Binding("SelectedValue", _AlertBS, "ALERT_REASON", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            Bind.NullValue = Nothing
            cmbAlert.DataBindings.Add(Bind)

            cmbTermReason.DataBindings.Clear()
            Bind = New Binding("SelectedValue", _AlertBS, "TERMINATION_REASON", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            Bind.NullValue = Nothing
            cmbTermReason.DataBindings.Add(Bind)

        Catch ex As Exception


                Throw

        Finally

        End Try

    End Sub

    Private Sub ClearDataBindings(parentCtrl As Control)

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

    End Sub

    Public Sub ClearErrors()
        ErrorProvider1.Clear()
    End Sub

    Public Sub ClearAll()

        ClearErrors()
        ClearDataBindings(Me)

        _TotalAlertDS = New DataSet
        _AlertDS = New DataSet

        _ButtonName = ""
        AlertDataGrid.CaptionText = ""

        cmbTermReason.SelectedIndex = -1
        cmbAlert.SelectedIndex = -1
        cmbRelationID.SelectedIndex = -1

        SaveEventButton.Enabled = False
        CancelEventButton.Enabled = False
        AddEventButton.Enabled = False
        TerminateEventButton.Enabled = False
        VoidEventButton.Enabled = False
        ModifyEventButton.Enabled = False
        HistoryButton.Enabled = False

        If _ChangedAlertRowsDS IsNot Nothing Then _ChangedAlertRowsDS.Clear()
        _ChangedAlertRowsDS = Nothing
        If _DisplayColumnNamesDV IsNot Nothing Then _DisplayColumnNamesDV = Nothing

        _CallingAppID = 0

        _RelationIDs = Nothing
    End Sub

    Private Sub AddALERTLine()

        Dim DR As DataRow
        Try

            DR = _AlertDS.Tables("REG_ALERTS").NewRow

            DR("FAMILY_ID") = _FamilyID
            DR("RELATION_ID") = _RelationID
            DR("FROM_DATE") = DBNull.Value
            DR("THRU_DATE") = "12-31-9999"
            DR("PASSPHRASE") = DBNull.Value
            DR("CREATE_USERID") = _DomainUser.ToUpper
            DR("CREATE_DATE") = RegMasterDAL.NowDate
            DR("BATCH_USERID") = _DomainUser.ToUpper
            DR("BATCH_DATE") = RegMasterDAL.NowDate
            DR("ONLINE_USERID") = _DomainUser.ToUpper
            DR("ONLINE_DATE") = RegMasterDAL.NowDate
            DR("FAMILY_ALERT") = 1
            DR("SYSTEM_ALERT") = 0
            DR("TERMINATION_DATE") = DBNull.Value
            DR("TERMINATION_USERID") = DBNull.Value
            DR("TERMINATION_REASON") = DBNull.Value
            DR("COMMENTS") = DBNull.Value

            _AlertDS.Tables("REG_ALERTS").Rows.Add(DR)

            SaveEventButton.Enabled = True
            CancelEventButton.Enabled = True

        Catch ex As Exception


                Throw
        Finally
        End Try

    End Sub

    Public Function VerifyALERTChanges() As Boolean
        Dim DR As DataRow
        Dim AlertDV As DataView
        Dim DV As DataView

        Try

            ClearErrors()

            If _AlertBS Is Nothing OrElse _AlertBS.Position < 0 OrElse _AlertBS.Count < 1 Then Return False

            DR = DirectCast(_AlertBS.Current, DataRowView).Row

            If IsDate(DR("FROM_DATE")) Then
            Else
                SetErrorWithTracking(ErrorProvider1, Me.txtFromDate, " From Date is invalid (MM-dd-yyyy)")
            End If

            If IsDate(DR("THRU_DATE")) Then
            Else
                SetErrorWithTracking(ErrorProvider1, Me.txtThruDate, " Thru Date is invalid (MM-dd-yyyy)")
            End If

            If IsDate(DR("FROM_DATE")) AndAlso IsDate(DR("THRU_DATE")) Then
                If CDate(DR("FROM_DATE")) > CDate(DR("THRU_DATE")) Then
                    SetErrorWithTracking(ErrorProvider1, txtThruDate, " End Date must be after Begin Date")
                End If
            End If

            If cmbAlert.SelectedValue.ToString.Length < 1 OrElse cmbAlert.SelectedIndex < 0 Then
                SetErrorWithTracking(ErrorProvider1, cmbAlert, "Alert Description is required.")
            End If

            ''Remarks only required for db column REMARKS_NEEDED_SW = 1 Then
            If Not IsDBNull(DR("ALERT_REASON")) Then
                AlertDV = New DataView(_RegAlertReasonValuesDT, "ALERT_REASON = " & DR("ALERT_REASON").ToString, "", DataViewRowState.CurrentRows)
                If AlertDV.Count > 0 AndAlso CInt(AlertDV(0)("REMARKS_NEEDED_SW")) = 1 Then
                    txtpasscode.ReadOnly = False
                    If txtpasscode.Text.Trim.Length = 0 Then
                        SetErrorWithTracking(ErrorProvider1, txtpasscode, "Passcode / Remarks required.")
                    End If
                End If
            End If

            '' Duplicate Alerts
            If DR.RowState = DataRowState.Added Then
                If Not IsDBNull(DR("ALERT_REASON")) AndAlso IsDate(DR("FROM_DATE")) Then
                    If _AlertDS IsNot Nothing Then
                        DV = New DataView(_AlertDS.Tables("REG_ALERTS"), "RELATION_ID= " & CStr(DR("RELATION_ID")) & " AND FROM_DATE= '" & CStr(DR("FROM_DATE")) & "' AND THRU_DATE= '" & CStr(DR("THRU_DATE")) & "' AND ALERT_REASON= " & CStr(cmbAlert.SelectedValue) & " AND TERMINATION_REASON IS NULL", "", DataViewRowState.OriginalRows)
                        If DV.Count > 0 Then
                            SetErrorWithTracking(ErrorProvider1, txtFromDate, "Duplicate Alert")
                        End If
                    End If
                End If
            ElseIf DR.RowState = DataRowState.Modified Then
                If Not IsDBNull(DR("ALERT_REASON")) AndAlso IsDate(DR("FROM_DATE")) Then
                    If _AlertDS IsNot Nothing Then
                        DV = New DataView(_AlertDS.Tables("REG_ALERTS"), "RELATION_ID= " & CStr(DR("RELATION_ID")) & " AND FROM_DATE= '" & CStr(DR("FROM_DATE")) & "' AND THRU_DATE= '" & CStr(DR("THRU_DATE")) & "' AND ALERT_REASON= " & CStr(cmbAlert.SelectedValue) & " AND TERMINATION_REASON IS NULL", "", DataViewRowState.CurrentRows)
                        If DV.Count > 1 Then
                            SetErrorWithTracking(ErrorProvider1, txtFromDate, "Duplicate Alert")
                        End If
                    End If
                End If
            End If

            If _ButtonName.Length > 0 Then
                If _ButtonName.ToUpper = "VOID" Then

                    If Not IsDBNull(DR("TERMINATION_REASON")) Then
                        SetErrorWithTracking(ErrorProvider1, cmbTermReason, "Void Reason is required.")
                    End If
                ElseIf _ButtonName.ToUpper = "TERMINATE" Then

                    If IsDBNull(DR("THRU_DATE")) Then
                        SetErrorWithTracking(ErrorProvider1, txtThruDate, "TermDate is required.")
                    End If

                    ''If IsDate(dr("THRU_DATE")) Then
                    ''    If CDate(dr("THRU_DATE")) < Generals.NowDate.Date Then
                    ''        SetErrorWithTracking(ErrorProvider1, txtthruDt, " Term date should not be past date.")
                    ''    End If
                    ''End If

                    If IsDate(DR("THRU_DATE")) AndAlso CStr(DR("THRU_DATE")) = "12/31/9999" Then
                        SetErrorWithTracking(ErrorProvider1, txtThruDate, " Enter Alert Termination Date")
                    End If
                End If
            End If

            If Not IsDBNull(DR("ALERT_REASON")) Then
                DV = New DataView(_RegAlertReasonValuesDT, "ALERT_REASON = " & CStr(DR("ALERT_REASON")), "", DataViewRowState.CurrentRows)
                ''  checking if it is family level alert or  not . ie., If family level alert the relation id for the alert is always 0 . 
                If DV.Count > 0 AndAlso CInt(DV(0)("FAMILY_ALERT")) = 1 Then
                    DR("RELATION_ID") = 0
                End If

            End If

            If ErrorProviderErrorsList(ErrorProvider1).Length > 0 Then
                Return True
            End If

            Return False

        Catch ex As Exception


                Throw

        Finally

            If AlertDV IsNot Nothing Then AlertDV.Dispose()
            AlertDV = Nothing

            If DV IsNot Nothing Then DV.Dispose()
            DV = Nothing
        End Try

    End Function

    Public Function ApplyALERTChanges() As Boolean

        Dim Cnt As Integer = 0
        Dim NewVal As String = ""
        Dim OrigVal As String = ""

        Dim ChgCnt As Integer = 0
        Dim HistSum As String = ""
        Dim HistDetail As String = ""
        Dim DV As DataView
        Dim Transaction As DbTransaction


        Try
            Transaction = RegMasterDAL.BeginTransaction

            _ChangedAlertRowsDS = _AlertDS.GetChanges()

            For Each DR As DataRow In _ChangedAlertRowsDS.Tables("REG_ALERTS").Rows

                HistSum = ""
                HistDetail = ""

                If DR.RowState <> DataRowState.Added AndAlso DR.RowState <> DataRowState.Deleted Then

                    HistDetail = DataGridCustom.IdentifyChanges(DR, AlertDataGrid)

                    If HistDetail.Length > 0 Then

                        HistSum = "ALERT OF FAMILYID: " & CStr(DR("FAMILY_ID")) & " WAS MODIFIED"
                        HistDetail = "MODIFIED BY: " & _DomainUser.ToUpper & Microsoft.VisualBasic.vbCrLf & " MODIFICATIONS WERE: " & Microsoft.VisualBasic.vbCrLf & HistDetail

                        RegMasterDAL.UpdateRegAlerts(CInt(DR("FAMILY_ID")), CInt(DR("RELATION_ID")), CDate(DR("FROM_DATE")), CDate(DR("THRU_DATE")), CInt(DR("ALERT_REASON")), txtpasscode.Text, DR("COMMENTS").ToString, If(IsDBNull(DR("TERMINATION_REASON")), Nothing, CType(DR("TERMINATION_REASON"), Integer?)), CDate(DR("ONLINE_DATE", DataRowVersion.Original)), Transaction)

                        RegMasterDAL.CreateRegHistory(CInt(DR("FAMILY_ID")), CInt(DR("RELATION_ID")), Nothing, Nothing, "REGALERTUPDATE", Nothing, Nothing, Nothing, HistSum, HistDetail, _DomainUser.ToUpper, Transaction)
                    End If

                ElseIf DR.RowState = DataRowState.Deleted Then 'DELETE 


                ElseIf DR.RowState = DataRowState.Added Then 'ADD

                    HistDetail = DataGridCustom.IdentifyChanges(DR, AlertDataGrid)

                    If HistDetail.Length > 0 Then

                        HistSum = "ALERT OF FAMILYID: " & CStr(DR("FAMILY_ID")) & " AND RELATIONID: " & CInt(DR("RELATION_ID")) & " ADDED"
                        HistDetail = "ADDED BY: " & _DomainUser.ToUpper & Microsoft.VisualBasic.vbCrLf & " ADDITIONS WERE: " & Microsoft.VisualBasic.vbCrLf & HistDetail

                        ChgCnt += 1

                        RegMasterDAL.CreateRegAlertComments(CInt(_FamilyID), CInt(DR("RELATION_ID")), CDate(DR("FROM_DATE")), CDate(DR("THRU_DATE")), CInt(DR("ALERT_REASON")), txtpasscode.Text.Trim, _DomainUser.ToUpper, DR("COMMENTS").ToString, Transaction)

                        DV = New DataView(_RegAlertReasonValuesDT, "ALERT_REASON = " & CStr(DR("ALERT_REASON")), "", DataViewRowState.CurrentRows)
                        If DV.Count > 0 AndAlso CInt(DV(0)("REMARKS_NEEDED_SW")) = 1 Then
                            RegMasterDAL.CreateRegRemarks(CInt(DR("FAMILY_ID")), CInt(DR("RELATION_ID")), UFCWGeneral.NowDate, txtpasscode.Text, _DomainUser.ToUpper, 0, Transaction)
                        End If

                        RegMasterDAL.CreateRegHistory(CInt(DR("FAMILY_ID")), CInt(DR("RELATION_ID")), Nothing, Nothing, "REGALERTCREATE", Nothing, Nothing, Nothing, HistSum, HistDetail, _DomainUser.ToUpper, Transaction)

                    End If

                End If
            Next

            RegMasterDAL.CommitTransaction(Transaction)

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
                                   vbCrLf & "Exit and re-enter the Alert Tab to refresh the Alert data.", "Save rejected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Case -803
                    MessageBox.Show("The item you attempted to create duplicates another item" & vbCrLf & "From/Thru/Alert/Void Reason cannot be the same for a patient. ", "Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Case Else
                    Dim Rethrow As Boolean = ExceptionPolicy.HandleException(db2ex, "General")
                    If (Rethrow) Then
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
            DV = Nothing
        End Try
    End Function

    Public Function ErrorProviderErrorsList(ByVal provider As ErrorProvider) As String()
        Dim S As String
        Dim Errors As New ArrayList
        For Cnt As Integer = 0 To grpEditPanel.Controls.Count - 1
            If grpEditPanel.Controls(Cnt).GetType.Name.Contains("Label") Then Continue For
            S = provider.GetError(grpEditPanel.Controls(Cnt))
            If S.Length > 0 Then
                Errors.Add(S)
            End If
        Next
        Return DirectCast(Errors.ToArray(GetType(String)), String())

    End Function

    'Public Function PendingChanges() As Boolean
    '    Dim DR As DataRow

    '    Try
    '        _ChangedAlertRowsDS = CType(_AlertDS.GetChanges(), UFCW.REGMASTER.AlertsDS)

    '        If _AlertDataGrid IsNot Nothing AndAlso _ChangedAlertRowsDS IsNot Nothing AndAlso _ChangedAlertRowsDS.Tables("REG_ALERTS").Rows.Count > 0 Then
    '            If (_ChangedAlertRowsDS IsNot Nothing) AndAlso (_ChangedAlertRowsDS.Tables("REG_ALERTS").Rows.Count > 0) Then
    '                For I = 0 To _ChangedAlertRowsDS.Tables("REG_ALERTS").Rows.Count - 1
    '                    DR = _ChangedAlertRowsDS.Tables("REG_ALERTS").Rows(I)
    '                    If DR.RowState <> DataRowState.Added Then
    '                        If DataGridCustom.IdentifyChanges(DR, _AlertDataGrid).ToString.Length > 0 Then
    '                            Return True
    '                        ElseIf DataGridCustom.IdentifyChanges(DR, _AlertDataGrid).ToString.Length = 0 Then
    '                            _ChangedAlertRowsDS.Tables("REG_ALERTS").Rows.Remove(DR)
    '                            _ChangedAlertRowsDS = CType(_AlertDS.GetChanges(), UFCW.REGMASTER.AlertsDS)
    '                        End If
    '                    ElseIf DR.RowState = DataRowState.Added Then
    '                        Return True
    '                    End If
    '                Next
    '            End If
    '        End If

    '        ''  Return False

    '    Catch ex As Exception
    '        Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '        If (Rethrow) Then
    '            Throw
    '        End If
    '    End Try
    '    Return False
    'End Function
    Private Function UnCommittedChangesExist() As Boolean

        Dim Modifications As String = ""

        Try
            If _AlertDS IsNot Nothing Then

                _ChangedAlertRowsDS = _AlertDS.GetChanges()

                If AlertDataGrid IsNot Nothing AndAlso _ChangedAlertRowsDS IsNot Nothing AndAlso _ChangedAlertRowsDS.Tables("REG_ALERTS").Rows.Count > 0 Then

                    For Each DR As DataRow In _ChangedAlertRowsDS.Tables("REG_ALERTS").Rows
                        If DR.RowState <> DataRowState.Added Then
                            'attempt to exclude rows accidently changed during navigation operations
                            Modifications = DataGridCustom.IdentifyChanges(DR, AlertDataGrid)

                            If Modifications IsNot Nothing AndAlso Modifications.Length > 0 Then
                                Return True
                            End If

                        ElseIf DR.RowState = DataRowState.Added Then
                            Return True
                        End If
                    Next

                End If
            End If

            Return False

        Catch ex As Exception

                Throw
        End Try

    End Function

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Indicates that Data has been modified and Save is Pending.")>
    Public Function ChangesPending() As Boolean

        Try
            Return UnCommittedChangesExist()

            Return False

        Catch ex As Exception

                Throw
        End Try

    End Function

    Private Sub LoadRelationID(ByVal relations As ArrayList)
        Try
            If relations IsNot Nothing Then
                cmbRelationID.DataSource = Nothing
                cmbRelationID.Items.Clear()
                '' relation.Add(DBNull.Value)
                cmbRelationID.DataSource = relations
            End If
        Catch ex As Exception

                Throw
        End Try
    End Sub

#End Region

#Region "Formatting"

    Private Sub DateOnlyBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Dim TextBox As TextBox

        Try
            TextBox = CType(DirectCast(sender, Binding).Control, TextBox)
            If IsDBNull(e.Value) = False Then
                e.Value = Convert.ToDateTime(String.Format("{0:MM-dd-yyyy}", e.Value)) 'handles mmddyy entry
                e.Value = Format(e.Value, "MM-dd-yyyy")
            End If

        Catch ex As Exception


                Throw

        End Try
    End Sub

    Private Sub DateOnlyBinding_BindingComplete(ByVal sender As Object, ByVal e As BindingCompleteEventArgs)
        Try
            SetErrorWithTracking(ErrorProvider1, CType(e.Binding.BindableComponent, Control), "")
            If e.BindingCompleteState <> BindingCompleteState.Success Then
                SetErrorWithTracking(ErrorProvider1, CType(e.Binding.BindableComponent, Control), "Date format invalid. Use mmddyy or mmddyyyy")
            End If

        Catch ex As Exception


                Throw

        End Try
    End Sub

    Private Sub DS_ColumnChanged(sender As Object, e As DataColumnChangeEventArgs)
        Dim BS As BindingSource

        Try

            BS = DirectCast(_AlertBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub DS_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs)
        Dim BS As BindingSource

        Try

            BS = DirectCast(_AlertBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " : " & e.Row(e.Column).ToString & "\" & If(e.ProposedValue Is Nothing, " ", e.ProposedValue.ToString) & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " : " & e.Row(e.Column).ToString & "\" & If(e.ProposedValue Is Nothing, " ", e.ProposedValue.ToString) & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub DS_RowChanged(sender As Object, e As DataRowChangeEventArgs)
        Dim BS As BindingSource

        Try

            BS = DirectCast(_AlertBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " :  " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " :  " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try


    End Sub

    Private Sub DS_RowChanging(sender As Object, e As DataRowChangeEventArgs)

        Dim BS As BindingSource

        Try

            BS = DirectCast(_AlertBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & e.Action.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & e.Action.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

    Private Sub cmb_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbTermReason.SelectionChangeCommitted, cmbAlert.SelectionChangeCommitted
        Dim DR As DataRow
        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If _AlertBS Is Nothing OrElse _AlertBS.Position < 0 OrElse CBox.Enabled = False OrElse CBox.ReadOnly = True OrElse CBox.SelectedIndex < 0 Then Exit Sub

            DR = DirectCast(_AlertBS.Current, DataRowView).Row

            If CBox.DataBindings.Count > 0 Then

                Select Case CBox.Name
                    Case "cmbAlert"

                        If DR("ALERT_REASON").ToString <> cmbAlert.SelectedValue.ToString Then
                            CBox.DataBindings("SelectedValue").WriteValue()
                            DR.EndEdit() 'to ensure changeditem triggered
                        End If

                    Case "cmbTermReason"

                        If DR("TERMINATION_REASON").ToString <> cmbTermReason.SelectedValue.ToString Then
                            CBox.DataBindings("SelectedValue").WriteValue()
                            DR.EndEdit() 'to ensure changeditem triggered
                        End If

                End Select

            End If

        Catch ex As Exception

                Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try
    End Sub

    Private Sub _AlertBS_ListChanged(sender As Object, e As ListChangedEventArgs) Handles _AlertBS.ListChanged
        Dim BS As BindingSource

        Try
            BS = CType(sender, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") O(" & e.OldIndex.ToString & ") N(" & e.NewIndex.ToString & ") CT(" & e.ListChangedType.ToString & ") " & Me.Name & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Select Case e.ListChangedType
                Case ListChangedType.ItemDeleted 'account for rows deleted due to a cancel action

                Case ListChangedType.ItemMoved

                    Select Case DirectCast(BS.Current, System.Data.DataRowView).Row.RowState
                        Case DataRowState.Modified

                        Case DataRowState.Added

                            If e.OldIndex <> e.NewIndex OrElse BS.Position > -1 AndAlso BS.Count = 1 Then
                                AlertDataGrid.Select(e.NewIndex)
                            End If

                    End Select

                Case ListChangedType.ItemChanged

                    If BS.Count = 0 Then 'item was changed in some way that excludes it from list due to filter exclusion
                        Return
                    End If

                Case ListChangedType.Reset 'triggered by sorts or changes in grid filter

                    If BS.Position > -1 AndAlso BS.Position <> e.NewIndex Then
                        If e.NewIndex > -1 Then
                            BS.Position = e.NewIndex
                            BS.ResetCurrentItem()
                        End If
                    End If

                Case ListChangedType.ItemAdded

                    If BS.Position <> e.NewIndex OrElse (BS.Position > -1 AndAlso BS.Count = 1) Then 'first item added

                        If e.NewIndex <> e.OldIndex AndAlso e.OldIndex > -1 Then
                            AlertDataGrid.UnSelect(e.OldIndex)
                        End If

                        If e.NewIndex > -1 Then BS.Position = e.NewIndex
                        If e.NewIndex > -1 Then AlertDataGrid.Select(e.NewIndex)
                    End If

            End Select

            ProcessControls()

        Catch ex As Exception

                Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") O(" & e.OldIndex.ToString & ") N(" & e.NewIndex.ToString & ") CT(" & e.ListChangedType.ToString & ") " & Me.Name & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

    Private Sub AlertControl_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged

        Try
            'reset controls to appropriate state
            If Not _Disposed Then

                If Me.Visible Then
                    Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

                    Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
                End If
            End If

        Catch ex As Exception

                Throw
        End Try

    End Sub

    Private Sub AlertBS_CurrentChanged(sender As Object, e As EventArgs) Handles _AlertBS.CurrentChanged
        'called when Current item of the bindingsource is changed (So different row)

        Dim BS As BindingSource

        Try

            BS = DirectCast(_AlertBS, BindingSource)

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

    Private Sub AlertBS_CurrentItemChanged(sender As Object, e As EventArgs) Handles _AlertBS.CurrentItemChanged
        'Called after CurrentChanged and one of the properties of Current is changed

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
    Private Sub AlertBS_PositionChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim BS As BindingSource

        Try

            BS = DirectCast(_AlertBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If BS Is Nothing OrElse BS.Position < 0 OrElse BS.Count < 1 Then Return 'no current row, most likely an item filter value was changed

            Dim DR As DataRow = DirectCast(BS.Current, DataRowView).Row

        Catch ex As Exception

                Throw
        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub txtFromDate_Validating(sender As Object, e As CancelEventArgs) Handles txtFromDate.Validating

        Dim Tbox As TextBox = CType(sender, TextBox)

        Try

            If _AlertBS Is Nothing OrElse _AlertBS.Position < 0 OrElse Tbox.ReadOnly Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ErrorProvider1.ClearError(Tbox)

            If Tbox.Text.Trim.Length < 1 Then
                ErrorProvider1.SetErrorWithTracking(Tbox, " From Date is required.")
            Else
                Dim HoldDate As Date? = UFCWGeneral.ValidateDate(Tbox.Text)
                If HoldDate Is Nothing Then
                    ErrorProvider1.SetErrorWithTracking(Tbox, " Date format must be mm/dd/yyyy or mm-dd-yyyy.")
                Else

                    If Tbox.Text <> CDate(HoldDate).ToString("MM-dd-yyyy") Then
                        Tbox.Text = CDate(HoldDate).ToString("MM-dd-yyyy")
                    End If

                End If

            End If

            If ErrorProvider1.GetError(Tbox).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
            End If

        Catch ex As Exception

                Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

#End Region

End Class
