Option Strict On

Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Windows.Forms
Imports System.ComponentModel
Imports DDTek.DB2
Imports System.Data.Common

Public Class HMONetworkInfoForm
    'Private Shared _TraceSwitch As New BooleanSwitch("CloneTraceSwitch", "Trace Switch in App.Config")

    Private _FamilyID As Integer
    Private _RelationID As Integer
    Private _CoverageCode As Integer
    Private _APPKEY As String = "UFCW\RegMaster\"
    Private _ReadOnlyMode As Boolean = True
    Private _ChangedHMONetworkDRs As DataSet

    Private _FamilyHMONetworkHistoryDS As New DataSet
    Private _FamilyHMONetworkHistoryBS As New BindingSource

    Private _ZipCode As Object
    Private _SubscriberStatus As String
    Private _AllHMONetworkDT As DataTable

    Private _UIState As UIStates

#Region "Properties"

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the FamilyID of the Document.")>
    Public Property FamilyID() As Integer
        Get
            Return _FamilyID
        End Get
        Set(ByVal value As Integer)
            _FamilyID = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the RelationID of the Document.")>
    Public Property RelationID() As Integer
        Get
            Return _RelationID
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

    <System.ComponentModel.Browsable(True), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Determines if control is in Read Only Mode.")>
    Public Property ReadOnlyMode() As Boolean
        Get
            Return _ReadOnlyMode
        End Get
        Set(ByVal value As Boolean)
            _ReadOnlyMode = value
            _UIState = If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable)
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Strongly Typed HMO Network DataSet.")>
    Public Property HMONetworkDataSet() As DataSet
        Get
            Return _FamilyHMONetworkHistoryDS
        End Get
        Set(ByVal value As DataSet)
            _FamilyHMONetworkHistoryDS = value
        End Set
    End Property


#End Region
    Private _Disposed As Boolean = False

    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.
            '

            If _FamilyHMONetworkHistoryBS IsNot Nothing Then
                _FamilyHMONetworkHistoryBS.Dispose()
            End If
            _FamilyHMONetworkHistoryBS = Nothing

            If HMONetworkDataGrid IsNot Nothing Then
                HMONetworkDataGrid.Dispose()
            End If
            HMONetworkDataGrid = Nothing

            If _AllHMONetworkDT IsNot Nothing Then
                _AllHMONetworkDT.Dispose()
            End If
            _AllHMONetworkDT = Nothing

            If _FamilyHMONetworkHistoryDS IsNot Nothing Then
                _FamilyHMONetworkHistoryDS.Dispose()
            End If
            _FamilyHMONetworkHistoryDS = Nothing

            If _ChangedHMONetworkDRs IsNot Nothing Then
                _ChangedHMONetworkDRs.Dispose()
            End If
            _ChangedHMONetworkDRs = Nothing

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
    End Sub

    Public Sub New(ByVal familyID As Integer, ByVal coverageCode As Integer, ByVal hmoNetworkDS As DataSet, Optional ByVal readonlymode As Boolean = True, Optional ByVal zip As Object = Nothing, Optional ByVal memstatus As String = "")

        Me.New()

        _FamilyID = familyID
        _CoverageCode = coverageCode
        _FamilyHMONetworkHistoryDS = hmoNetworkDS

        Me.ReadOnlyMode = readonlymode ' use property to control UIMode

        _ZipCode = zip
        _SubscriberStatus = memstatus

        LoadHMONetworks()
        LoadHMONetworkParticipantInfo()

    End Sub
#End Region


#Region "Form\Button Events"
    Private Sub HMONetworkInfo_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        If Not _Disposed Then
            SetUIElements()
        End If
    End Sub

    Private Sub btnAdd_Click(sender As System.Object, e As System.EventArgs) Handles AddButton.Click

        Try
            AddButton.Enabled = False

            AddHMONetworkLine()

            txtFromDate.Focus()

        Catch ex As Exception


                Throw

        Finally
        End Try

    End Sub

    Private Sub CancelButton_Click(sender As System.Object, e As System.EventArgs) Handles CancelChangeButton.Click
        Dim Result As DialogResult = DialogResult.None

        Dim SavedAutoValidateState As AutoValidate

        Dim CM As CurrencyManager
        Dim DGDRV As DataRowView
        Dim DGRow As DataRow

        Try

            SavedAutoValidateState = Me.AutoValidate

            Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable

            CancelChangeButton.Enabled = False

            If Me.HMONetworkDataGrid.DataSource IsNot Nothing AndAlso Me.HMONetworkDataGrid.BindingContext IsNot Nothing Then
                CM = DirectCast(Me.HMONetworkDataGrid.BindingContext(Me.HMONetworkDataGrid.DataSource, Me.HMONetworkDataGrid.DataMember), CurrencyManager)
                If CM IsNot Nothing AndAlso CM.Count > 0 Then
                    DGDRV = DirectCast(CM.Current, DataRowView)
                    DGRow = DGDRV.Row
                End If
            End If

            If DGRow.RowState = DataRowState.Modified OrElse DGRow.RowState = DataRowState.Added Then

                Result = MessageBox.Show(Me, "Do you want to Cancel the changes?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                If Result = DialogResult.Yes Then

                    CancelChanges()

                ElseIf Result = DialogResult.No Then

                    SetUIElements()

                End If
            Else
                SetUIElements(If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable))
            End If

        Catch ex As Exception
            Throw
        Finally
            Me.AutoValidate = SavedAutoValidateState

        End Try
    End Sub
    Private Sub CancelChanges()

        Try

            Me.HMONetworkDataGrid.SuspendLayout()

            SetUIElements(UIStates.Canceling)

            _FamilyHMONetworkHistoryDS.RejectChanges()

            ClearErrors()

        Catch ex As Exception

                Throw
        Finally

            SetUIElements(If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable))
            Me.HMONetworkDataGrid.ResumeLayout()

        End Try

    End Sub
    Private Sub SaveButton_Click(sender As System.Object, e As System.EventArgs) Handles SaveButton.Click

        Dim Transaction As DbTransaction
        Dim ActivityTimestamp As DateTime = DateTime.Now

        Dim SavedAutoValidateState As AutoValidate

        Try

            SavedAutoValidateState = Me.AutoValidate

            SaveButton.Enabled = False

            HMONetworkDataGrid.SuspendLayout()

            _FamilyHMONetworkHistoryBS.EndEdit()

            If VerifyHMONetworkChanges() Then
                Return 'verification failed
            End If

            _ChangedHMONetworkDRs = _FamilyHMONetworkHistoryDS.GetChanges()

            If _ChangedHMONetworkDRs IsNot Nothing Then

                Transaction = RegMasterDAL.BeginTransaction

                SetUIElements(UIStates.Saving)

                If SaveHMONetworkChanges(ActivityTimestamp, Transaction) Then
                    RegMasterDAL.CommitTransaction(Transaction)

                    _FamilyHMONetworkHistoryDS.AcceptChanges()

                    SetUIElements(UIStates.Saved)

                    MessageBox.Show("HMO Network Record Saved Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    _UIState = If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable)

                End If

            Else

                Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable

                CancelChanges()

                _UIState = If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable)

                MessageBox.Show("No changes were made. Save request resulted in no action being taken.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If

        Catch db2ex As DB2Exception
            Dim MessageText As String
            Dim TitleText As String

            Select Case db2ex.Number
                Case -438, -1822
                    MessageText = "The item(s) you are attempting to update has been modified by another." &
                                       vbCrLf & "Attempted changes have been reversed. Data has been refreshed to reflect current values."

                    TitleText = "Save rejected"

                Case Else

                    MessageText = "Critical database error. " & db2ex.Message
                    TitleText = "Critical database error. "

            End Select

            Try
                RegMasterDAL.RollbackTransaction(Transaction)
            Catch ex2 As Exception
            End Try

            MessageBox.Show(MessageText, TitleText, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            CancelChanges()

            _UIState = If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable)

        Catch ex As Exception

            Try
                RegMasterDAL.RollbackTransaction(Transaction)
            Catch ex2 As Exception
            End Try

            CancelChanges()


                Throw

        Finally

            SetUIElements()

            Me.AutoValidate = SavedAutoValidateState
            HMONetworkDataGrid.ResumeLayout()

            If Transaction IsNot Nothing Then Transaction.Dispose()
            Transaction = Nothing

        End Try

    End Sub

    Private Sub txtDt_Validating(ByVal sender As Object, ByVal e As CancelEventArgs) Handles txtFromDate.Validating, txtThruDate.Validating

        Dim TBox As TextBox = CType(sender, TextBox)
        Dim HoldDate As Date?

        If TBox.Enabled = False OrElse TBox.ReadOnly OrElse TBox.TextLength < 1 Then Return

        Try
            ErrorProvider1.ClearError(TBox)

            HoldDate = UFCWGeneral.ValidateDate(TBox.Text)
            If HoldDate Is Nothing Then
                ErrorProvider1.SetError(TBox, " Date format must be mm/01/yyyy or mm-01-yyyy.")
            Else

                If TBox.Name = "txtFromDt" Then
                    ''In from date  Allow only on or after 01-01-2012
                    If DateDiff(DateInterval.Day, CDate("01-01-2012"), CDate(HoldDate)) < 0 Then
                        MessageBox.Show("HMO Network date cannot be start before 2012.", "HMO Change", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        ErrorProvider1.SetError(TBox, "HMO Network date cannot start before 2012.")
                    End If

                    ''warning when entering retroactive month
                    If DateDiff(DateInterval.Month, Now.Date, CDate(HoldDate)) < 0 Then
                        MessageBox.Show("Requested modification will result in a Retroactive change to HMO Network.", "HMO Change", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If

                    If CDate(TBox.Text).Day <> 1 Then
                        HoldDate = CDate(CDate(HoldDate).Month.ToString & "-01-" & CDate(HoldDate).Year.ToString)
                        ErrorProvider1.SetError(TBox, " From Date modified to use 1st day of month.")
                    End If

                    If ((CDate(HoldDate).Year < CDate(Format(Date.Now, "MM-dd-yyyy")).Year)) Then
                        ErrorProvider1.SetError(TBox, " From date cannot be a prior year.")
                    End If

                    If DateDiff(DateInterval.Month, Now, CDate(HoldDate)) > 1 AndAlso Not CDate(HoldDate) <> CDate("1/1/" & Now.AddYears(+1).Year.ToString) Then
                        MessageBox.Show("Future From Date(s) can either be 1/1/" & Now.AddYears(+1).Year.ToString & " or next month ", "Future date invalid.", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        ErrorProvider1.SetError(TBox, " From date cannot be more than 3 months in the future.")
                    End If

                End If

                If TBox.Text <> CDate(HoldDate).ToString("MM-dd-yyyy") Then
                    TBox.Text = CDate(HoldDate).ToString("MM-dd-yyyy")
                End If

            End If

            If ErrorProvider1.GetError(TBox).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
            End If

        Catch ex As Exception

                Throw
        Finally
        End Try

    End Sub
    'Private Sub txtThruDt_Validated(sender As Object, e As EventArgs) Handles txtThruDt.Validating, txtThruDt.Validated

    '    If _FamilyHMONetworkHistoryBS.Position < 0 Then Exit Sub

    '    Dim TBox As TextBox = CType(sender, TextBox)

    '    Dim DR As DataRow = DirectCast(_FamilyHMONetworkHistoryBS.Current, DataRowView).Row

    '    Try

    '        If txtFromDt.Text.Trim.Length > 0 AndAlso txtThruDt.Text.Trim.Length > 0 Then

    '        End If

    '    Catch ex As Exception
    '        Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '        If (rethrow) Then
    '            Throw
    '        End If
    '    Finally
    '    End Try

    'End Sub

    Private Sub txtFromDt_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtFromDate.KeyPress, txtThruDate.KeyPress

        Dim TBox As TextBox = CType(sender, TextBox)

        If Char.IsDigit(e.KeyChar) OrElse (Microsoft.VisualBasic.Asc(e.KeyChar) = 45) OrElse (Microsoft.VisualBasic.Asc(e.KeyChar) = 47) OrElse Char.IsControl(e.KeyChar) Then
            'do nothing
        Else
            e.Handled = True
            'MsgBox("Please enter Date", MsgBoxStyle.Information, "Verify")
            TBox.Focus()
        End If

    End Sub

    Private Sub NetworkInfo_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Try
            If Me.PendingChanges = True Then
                MessageBox.Show(Me, "Changes have been made to Network Screen." & vbCrLf &
                                             "Please Complete the changes before continuing", "Save changes", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)

                e.Cancel = True

            End If
        Catch ex As Exception

                Throw
        Finally
        End Try

    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearErrors()
    End Sub

    Private Sub cmbHMONetwork_SelectionChangeCommitted(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbHMONetwork.SelectionChangeCommitted

        Dim CBox As ComboBox = CType(sender, ComboBox)
        Dim DRV As DataRowView = CType(CBox.SelectedItem, DataRowView)

        'Debug.Print("cmbHMONetwork_SelectionChangeCommitted (In): ")

        ErrorProvider1.SetError(cmbHMONetwork, "")

        If Not ValidateZip(DRV) Then
            RemoveHandler cmbHMONetwork.SelectedValueChanged, AddressOf cmbHMONetwork_SelectedValueChanged
            RemoveHandler cmbHMONetwork.SelectedIndexChanged, AddressOf cmbHMONetwork_SelectedIndexChanged

            CBox.ResetText()
            CBox.SelectedIndex = -1
            CBox.Focus()

            AddHandler cmbHMONetwork.SelectedValueChanged, AddressOf cmbHMONetwork_SelectedValueChanged
            AddHandler cmbHMONetwork.SelectedIndexChanged, AddressOf cmbHMONetwork_SelectedIndexChanged

        End If

        'Debug.Print("cmbHMONetwork_SelectionChangeCommitted (Out): ")

    End Sub

    Private Function ValidateZip(drv As DataRowView) As Boolean

        Dim HMOZipNetworkDR As DataRow
        Dim CM As CurrencyManager
        Dim DVCurrent As DataRowView

        Try

            CM = _FamilyHMONetworkHistoryBS.CurrencyManager
            DVCurrent = CType(CM.Current, DataRowView)

            HMOZipNetworkDR = RegMasterDAL.RetrieveZipHMONetworkInfo(_CoverageCode, CInt(_ZipCode), CDate(DVCurrent("FROM_DATE")))

            If _CoverageCode.ToString.StartsWith("4") OrElse _CoverageCode.ToString.StartsWith("8") Then 'warn if Zip is outside of network area
                Dim Result As DialogResult

                If _FamilyHMONetworkHistoryBS.Position > -1 Then

                    If _SubscriberStatus.Length > 0 Then
                        If _SubscriberStatus.ToUpper = "ACTIVE" OrElse _SubscriberStatus.ToUpper = "COBRA" Then
                            If HMOZipNetworkDR IsNot Nothing AndAlso Not IsDBNull(HMOZipNetworkDR("ZIP1")) Then

                                If drv("HMO_NETWORK").ToString = "0" Then
                                    Result = MessageBox.Show(Me, " Member is living in HMO Network Area." & Environment.NewLine &
                                                     " Please choose Network other than Non Flex (0). " & Environment.NewLine & Environment.NewLine &
                                                     " Would you like to change the HMO Network?", "HMO Network", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                                    If Result = DialogResult.Yes Then
                                        Return False
                                    End If
                                End If

                            ElseIf drv("HMO_NETWORK").ToString = "NA" Then
                                MessageBox.Show("Please choose a HMO Network. " & Environment.NewLine &
                                        "NOT APPLICABLE (NA) is not a valid HMO Network", "HMO Network", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Return False
                            End If
                        End If

                    ElseIf _SubscriberStatus.ToUpper = "RETIREE" Then
                        If drv("HMO_NETWORK").ToString <> "NA" Then
                            Result = MessageBox.Show(Me, " HMO Network is Not Available." & Environment.NewLine &
                                                 " Please choose NA. " & Environment.NewLine & Environment.NewLine &
                                                 " Would you like to change the HMO Network?", "HMO Network", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                            If Result = DialogResult.Yes Then
                                Return False
                            End If

                        End If
                    End If
                End If

            End If

            Return True

            'Debug.Print("ValidateZip (Out): ")

        Catch ex As Exception

                Throw
        Finally
        End Try
    End Function

    Private Sub cmbHMONetwork_SelectedIndexChanged(sender As Object, e As EventArgs) 'Handles cmbHMONetwork.SelectedIndexChanged
        'Debug.Print("cmbHMONetwork_SelectedIndexChanged (Out): " & cmbHMONetwork.SelectedIndex.ToString)
    End Sub

    Private Sub cmbHMONetwork_SelectedValueChanged(sender As Object, e As EventArgs) 'Handles cmbHMONetwork.SelectedValueChanged
        'Debug.Print("cmbHMONetwork_SelectedValueChanged (Out): " & If(cmbHMONetwork.SelectedValue Is Nothing, "Nothing", cmbHMONetwork.SelectedValue.ToString))
    End Sub

#End Region

#Region "Custom Subs\Functions"


    Private Sub FamilyHMONetworkHistoryDT_RowChanged(sender As Object, e As DataRowChangeEventArgs)
        'Debug.Print("FamilyHMONetworkHistoryDT_RowChanged: " & _FamilyHMONetworkHistoryBS.Count.ToString & " " & _FamilyHMONetworkHistoryBS.Position.ToString)
    End Sub

    Private Sub FamilyHMONetworkHistoryDT_RowChanging(sender As Object, e As DataRowChangeEventArgs)
        'Debug.Print("FamilyHMONetworkHistoryDT_RowChanging: " & _FamilyHMONetworkHistoryBS.Count.ToString & " " & _FamilyHMONetworkHistoryBS.Position.ToString)
    End Sub

    Private Sub FamilyHMONetworkHistoryBS_CurrentItemChanged(sender As Object, e As EventArgs)
        'Debug.Print("FamilyHMONetworkHistoryBS_CurrentItemChanged: " & _FamilyHMONetworkHistoryBS.Count.ToString & " " & _FamilyHMONetworkHistoryBS.Position.ToString)
    End Sub

    Private Sub FamilyHMONetworkHistoryBS_CurrentChanged(sender As Object, e As EventArgs)
        'Debug.Print("FamilyHMONetworkHistoryBS_CurrentChanged: " & _FamilyHMONetworkHistoryBS.Count.ToString & " " & _FamilyHMONetworkHistoryBS.Position.ToString)
    End Sub

    Private Sub FamilyHMONetworkHistoryBS_PositionChanged(sender As Object, e As EventArgs)

        'Debug.Print("FamilyHMONetworkHistoryBS_PositionChanged (In): " & _FamilyHMONetworkHistoryBS.Count.ToString & " " & _FamilyHMONetworkHistoryBS.Position.ToString)
        SetUIElements()
        'Debug.Print("FamilyHMONetworkHistoryBS_PositionChanged (Out): " & _FamilyHMONetworkHistoryBS.Count.ToString & " " & _FamilyHMONetworkHistoryBS.Position.ToString)

    End Sub

    Private Sub OnListChanged(sender As Object, args As System.ComponentModel.ListChangedEventArgs)

        'Debug.Print("OnListChanged (In):" & args.ListChangedType.ToString & " " & args.OldIndex.ToString & ":" & args.NewIndex.ToString)

        Select Case args.ListChangedType
            Case ListChangedType.ItemDeleted

            Case ListChangedType.ItemChanged
                SetUIElements()
            Case ListChangedType.Reset
                SetUIElements()
            Case ListChangedType.ItemAdded
                If CType(sender, BindingSource).CurrencyManager.Position <> args.NewIndex Then
                    CType(sender, BindingSource).CurrencyManager.Position = args.NewIndex
                    SetUIElements()
                End If
            Case Else
        End Select

        'Debug.Print("OnListChanged (Out):" & args.ListChangedType.ToString & " " & args.OldIndex.ToString & ":" & args.NewIndex.ToString)

    End Sub

    Private Sub SetUIElements(Optional userIntefaceState As UIStates = UIStates.AsIs)

        If Not userIntefaceState.HasFlag(UIStates.AsIs) Then _UIState = userIntefaceState

        Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " " & Me.Name & " : (" & _UIState.ToString & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Dim ControlIsReadOnly As UIStates = UIStates.None Or UIStates.NotModifiable Or UIStates.Modifiable

        Dim CM As CurrencyManager
        Dim DR As DataRow

        Try

            Me.SaveButton.Enabled = False
            Me.SaveButton.Visible = False

            Me.CancelChangeButton.Enabled = False
            Me.CancelChangeButton.Visible = False

            Me.AddButton.Enabled = False
            Me.AddButton.Visible = False

            Me.ModifyButton.Enabled = False
            Me.ModifyButton.Visible = False

            txtFromDate.Enabled = False
            txtThruDate.Enabled = False
            cmbHMONetwork.Enabled = False

            If _UIState = UIStates.None Then Return

            CM = _FamilyHMONetworkHistoryBS.CurrencyManager

            Select Case _UIState
                Case UIStates.NotModifiable

                Case UIStates.Modifiable, UIStates.Adding, UIStates.Modifying

                    Me.AddButton.Visible = True
                    Me.ModifyButton.Visible = True
                    Me.SaveButton.Visible = True
                    Me.CancelChangeButton.Visible = True

                    If CM IsNot Nothing AndAlso CM.Count > 0 AndAlso CM.Position > -1 Then
                        DR = DirectCast(CM.Current, DataRowView).Row
                    End If

                    If _UIState = UIStates.Modifiable Then 'can only modify current & 2 months prior and previous months must be in the current year.
                        If DR IsNot Nothing AndAlso Not IsDBNull(DR("FROM_DATE")) AndAlso (DateDiff(DateInterval.Month, Now, CDate(DR("FROM_DATE"))) > -3 AndAlso CDate(DR("FROM_DATE")).Year = Date.Now.Year) Then
                            Me.ModifyButton.Enabled = True
                        End If

                        Dim DRs As DataRow() = _FamilyHMONetworkHistoryDS.Tables("HMO_NETWORK").Select("THRU_DATE = #12/31/9999#")
                        If DRs.Length < 1 Then
                            Me.AddButton.Enabled = True
                        Else
                            If CDate(DRs(0)("FROM_DATE")).AddMonths(-3) < Date.Now Then Me.AddButton.Enabled = True
                        End If

                    Else
                        If _UIState = UIStates.Adding AndAlso DR.RowState = DataRowState.Added Then
                            txtFromDate.Enabled = True
                        End If
                        If DR IsNot Nothing AndAlso Not IsDBNull(DR("FROM_DATE")) AndAlso (DateDiff(DateInterval.Month, Now, CDate(DR("FROM_DATE"))) > -3 AndAlso CDate(DR("FROM_DATE")).Year = Date.Now.Year) Then
                            cmbHMONetwork.Enabled = True
                        End If
                        Me.CancelChangeButton.Enabled = True
                    End If

                    If DR IsNot Nothing AndAlso DR.RowState <> DataRowState.Unchanged OrElse _UIState = UIStates.Modifying Then
                        Me.SaveButton.Enabled = True
                    End If

                Case UIStates.Deleting

            End Select

            'If CM IsNot Nothing AndAlso CM.Count > 0 AndAlso CM.Position > -1 Then

            '    FilterHMONetworkValues(DirectCast(CM.Current, DataRowView), If(IsDBNull(DR("FROM_DATE")), Now.Date, CDate(DR("FROM_DATE")))) ' Due to 80 being used for a discontinued plan the tables values must be filtered to ensure the correct descriptions display

            'End If

            'Debug.Print("SetUIElements (Out):" & _UIState.ToString & " " & uiState.ToString)

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Sub

    Public Sub LoadHMONetworkParticipantInfo()
        Try

            ClearErrors()
            ClearHMONetworkDataBindings()

            If _FamilyHMONetworkHistoryDS Is Nothing OrElse _FamilyHMONetworkHistoryDS.Tables.Count < 1 OrElse _FamilyHMONetworkHistoryDS.Tables("HMO_NETWORK").Rows.Count < 1 Then
                _FamilyHMONetworkHistoryDS = RegMasterDAL.RetrieveHMONetworkInfo(_FamilyID, _FamilyHMONetworkHistoryDS)
            End If

            RemoveHandler _FamilyHMONetworkHistoryDS.Tables("HMO_NETWORK").RowChanging, AddressOf FamilyHMONetworkHistoryDT_RowChanging
            RemoveHandler _FamilyHMONetworkHistoryDS.Tables("HMO_NETWORK").RowChanged, AddressOf FamilyHMONetworkHistoryDT_RowChanged
            RemoveHandler _FamilyHMONetworkHistoryBS.PositionChanged, AddressOf FamilyHMONetworkHistoryBS_PositionChanged
            RemoveHandler _FamilyHMONetworkHistoryBS.CurrentChanged, AddressOf FamilyHMONetworkHistoryBS_CurrentChanged
            RemoveHandler _FamilyHMONetworkHistoryBS.CurrentItemChanged, AddressOf FamilyHMONetworkHistoryBS_CurrentItemChanged
            RemoveHandler _FamilyHMONetworkHistoryBS.ListChanged, AddressOf OnListChanged

            _FamilyHMONetworkHistoryBS.AllowNew = False
            _FamilyHMONetworkHistoryBS.DataMember = "HMO_NETWORK"
            _FamilyHMONetworkHistoryBS.DataSource = _FamilyHMONetworkHistoryDS

            HMONetworkDataGrid.CaptionText = "HMO Network for Family (" & _FamilyID.ToString & ")"
            HMONetworkDataGrid.DataSource = _FamilyHMONetworkHistoryBS
            HMONetworkDataGrid.SetTableStyle()
            HMONetworkDataGrid.Sort = If(HMONetworkDataGrid.LastSortedBy, HMONetworkDataGrid.DefaultSort)

            LoadHMONetworkDataBindings()

        Catch ex As Exception

                Throw
        Finally
            AddHandler _FamilyHMONetworkHistoryDS.Tables("HMO_NETWORK").RowChanging, AddressOf FamilyHMONetworkHistoryDT_RowChanging
            AddHandler _FamilyHMONetworkHistoryDS.Tables("HMO_NETWORK").RowChanged, AddressOf FamilyHMONetworkHistoryDT_RowChanged
            AddHandler _FamilyHMONetworkHistoryBS.PositionChanged, AddressOf FamilyHMONetworkHistoryBS_PositionChanged
            AddHandler _FamilyHMONetworkHistoryBS.CurrentChanged, AddressOf FamilyHMONetworkHistoryBS_CurrentChanged
            AddHandler _FamilyHMONetworkHistoryBS.CurrentItemChanged, AddressOf FamilyHMONetworkHistoryBS_CurrentItemChanged
            AddHandler _FamilyHMONetworkHistoryBS.ListChanged, AddressOf OnListChanged

            'AddHandler HMONetworkDataGrid.GridSorted, AddressOf HMONetworkDataGrid_GridSorted

            If _FamilyHMONetworkHistoryBS.Count > 0 Then _FamilyHMONetworkHistoryBS.Position = 0
        End Try
    End Sub

    Private Sub HMONetworkDataGrid_GridSorted(sender As Object, e As CurrentRowChangedEventArgs)
        Dim DG As DataGridCustom = CType(sender, DataGridCustom)

        'Debug.Print("HMONetworkDataGrid_GridSorted: " & DG.Sort)
        If e.LastDataRow IsNot Nothing AndAlso e.LastDataRow.RowState <> DataRowState.Detached Then
            '            _FamilyHMONetworkHistoryBS.Position = _FamilyHMONetworkHistoryBS.Find("THRU_DATE", e.LastDataRow("THRU_DATE"))
            '           If e.LastRowIsSelected Then DG.Select(_FamilyHMONetworkHistoryBS.Position)
        End If

    End Sub

    Private Sub LoadHMONetworkDataBindings()
        Dim Bind As Binding

        Try

            txtFromDate.DataBindings.Clear()
            Bind = New Binding("Text", _FamilyHMONetworkHistoryBS, "FROM_DATE")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged
            AddHandler Bind.Format, AddressOf DateOnlyBinding_Format
            AddHandler Bind.Parse, AddressOf DateOnlyBinding_Parse
            txtFromDate.DataBindings.Add(Bind)

            txtThruDate.DataBindings.Clear()
            Bind = New Binding("Text", _FamilyHMONetworkHistoryBS, "THRU_DATE")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged
            AddHandler Bind.Format, AddressOf DateOnlyBinding_Format
            AddHandler Bind.Parse, AddressOf DateOnlyBinding_Parse
            txtThruDate.DataBindings.Add(Bind)

            cmbHMONetwork.DataBindings.Clear()
            Bind = New Binding("SelectedValue", _FamilyHMONetworkHistoryBS, "HMO_NETWORK", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged
            Bind.NullValue = Nothing
            AddHandler Bind.Format, AddressOf ComboOnlyBinding_Format
            AddHandler Bind.Parse, AddressOf ComboOnlyBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf ComboOnlyBindingComplete
            cmbHMONetwork.DataBindings.Add(Bind)

        Catch ex As Exception


                Throw

        Finally

        End Try

    End Sub

    Private Sub ComboOnlyBinding_Parse(sender As Object, e As ConvertEventArgs)
        'Debug.Print("ComboOnlyBinding_Parse (Out): " & If(e.Value Is Nothing, "Nothing", If(IsDBNull(e.Value), "Null", e.Value.ToString)))
    End Sub

    Private Sub ComboOnlyBinding_Format(sender As Object, e As ConvertEventArgs)
        'Debug.Print("ComboOnlyBinding_Format (In): " & If(e.Value Is Nothing, "Nothing", If(IsDBNull(e.Value), "Null", e.Value.ToString)))

        Dim ComboBinding As Binding = CType(sender, Binding)
        Dim CM As CurrencyManager = CType(ComboBinding.BindingManagerBase, CurrencyManager)
        Dim ListControl As ComboBox = CType(ComboBinding.Control, ComboBox)

        If CM.Position > -1 Then

            Dim DRV As DataRowView = CType(CM.Current, DataRowView)

            FilterHMONetworkValues(DRV, If(IsDBNull(DRV("FROM_DATE")), Now.Date, CDate(DRV("FROM_DATE")))) ' Due to 80 being used for a discontinued plan the tables values must be filtered to ensure the correct descriptions display

            If IsDBNull(DRV("HMO_NETWORK")) OrElse DRV("HMO_NETWORK") Is Nothing Then
                'Debug.Print("ComboOnlyBinding_Format (Reset4Add): " & If(e.Value Is Nothing, "Nothing", If(IsDBNull(e.Value), "Null", e.Value.ToString)))
                ListControl.ResetText()
                ListControl.SelectedIndex = -1
            End If
        End If

        'Debug.Print("ComboOnlyBinding_Format (Out): " & If(e.Value Is Nothing, "Nothing", If(IsDBNull(e.Value), "Null", e.Value.ToString)))
    End Sub

    Private Sub ComboOnlyBindingComplete(ByVal sender As Object, ByVal e As System.Windows.Forms.BindingCompleteEventArgs)

        Try

            If Not e.BindingCompleteState = BindingCompleteState.Success Then
                MessageBox.Show("Control " & e.Binding.Control.Name & " " & e.ErrorText, "Problem converting data to database format", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                Dim ComboBinding As Binding = CType(sender, Binding)
                Dim CM As CurrencyManager = CType(ComboBinding.BindingManagerBase, CurrencyManager)
                Dim ListControl As ComboBox = CType(ComboBinding.Control, ComboBox)

                If CM.Position > -1 AndAlso ListControl.SelectedItem IsNot Nothing Then
                    Dim DRV As DataRowView = CType(CM.Current, DataRowView)
                    If Not (IsDBNull(DRV("HMO_NETWORK")) OrElse DRV("HMO_NETWORK") Is Nothing) Then
                        If DRV.Row("DESCRIPTION").ToString <> CType(ListControl.SelectedItem, DataRowView)("DESCRIPTION").ToString Then
                            DRV.Row("DESCRIPTION") = CType(ListControl.SelectedItem, DataRowView)("DESCRIPTION")
                        End If
                    End If
                    _FamilyHMONetworkHistoryBS.EndEdit()
                End If

            End If

            'Debug.Print("ComboOnlyBindingComplete (Out): ")

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try

    End Sub

    Private Sub ClearHMONetworkDataBindings()

        txtFromDate.DataBindings.Clear()
        txtThruDate.DataBindings.Clear()
        cmbHMONetwork.DataBindings.Clear()

        HMONetworkDataGrid.DataMember = ""
        HMONetworkDataGrid.DataSource = Nothing

    End Sub

    Public Sub ClearAll()

        HMONetworkDataGrid.DataSource = Nothing
        HMONetworkDataGrid.CaptionText = ""

        _FamilyHMONetworkHistoryDS.Clear()

        _ZipCode = Nothing
        _SubscriberStatus = ""
        cmbHMONetwork.DataSource = Nothing

    End Sub

    Private Sub AddHMONetworkLine()
        Dim DR As DataRow

        Try

            RemoveHandler cmbHMONetwork.SelectedValueChanged, AddressOf cmbHMONetwork_SelectedValueChanged
            RemoveHandler cmbHMONetwork.SelectedIndexChanged, AddressOf cmbHMONetwork_SelectedIndexChanged

            'Debug.Print("AddHMONetworkLine (Before ThruDate): ")

            DR = _FamilyHMONetworkHistoryDS.Tables("HMO_NETWORK").NewRow
            DR("FROM_DATE") = CDate(Now.AddMonths(+1).Month & "/01/" & Now.AddMonths(+1).Year)

            If _FamilyHMONetworkHistoryDS.Tables("HMO_NETWORK").Rows.Count > 0 Then
                Dim DRs As DataRow() = _FamilyHMONetworkHistoryDS.Tables("HMO_NETWORK").Select("THRU_DATE = #12/31/9999#")
                If DRs.Length = 1 Then
                    If CDate(DRs(0)("FROM_DATE")) > Now.Date Then
                        DRs(0)("THRU_DATE") = CDate(CDate(DRs(0)("FROM_DATE")).AddMonths(+1).Month & "/01/" & CDate(DRs(0)("FROM_DATE")).AddMonths(+1).Year).AddDays(-1)
                        DR("FROM_DATE") = CDate(DRs(0)("THRU_DATE")).AddDays(1)
                    Else
                        DRs(0)("THRU_DATE") = CDate(Now.AddMonths(+1).Month & "/01/" & Now.AddMonths(+1).Year).AddDays(-1)
                    End If
                End If
            End If

            Dim CreateDate As DateTime = DateTime.Now

            'Debug.Print("AddHMONetworkLine (Before NewRow): ")

            DR("THRU_DATE") = "12/31/9999"
            DR("FAMILY_ID") = _FamilyID
            DR("RELATION_ID") = _RelationID
            DR("HMO_NETWORK") = Nothing
            DR("MEDICAL_PLAN") = _CoverageCode
            DR("CREATE_USERID") = UFCWGeneral.DomainUser.ToUpper
            DR("CREATE_DATE") = CreateDate
            DR("ONLINE_DATE") = CreateDate

            If _CoverageCode.ToString.StartsWith("4") Then 'Manages existing vs new HMO table
                DR("HMO_ID") = -1
            Else
                DR("HMO_ID") = DBNull.Value
            End If

            _UIState = UIStates.Adding

            'Debug.Print("AddHMONetworkLine (Before Add): ")
            _FamilyHMONetworkHistoryDS.Tables("HMO_NETWORK").Rows.Add(DR)

            'Debug.Print("AddHMONetworkLine (Before EndEdit): ")
            _FamilyHMONetworkHistoryBS.EndEdit()

            HMONetworkDataGrid.Select(_FamilyHMONetworkHistoryBS.Position)

        Catch ex As Exception


                Throw
        Finally
            'Debug.Print("AddHMONetworkLine (In Finally): ")

            AddHandler cmbHMONetwork.SelectedValueChanged, AddressOf cmbHMONetwork_SelectedValueChanged
            AddHandler cmbHMONetwork.SelectedIndexChanged, AddressOf cmbHMONetwork_SelectedIndexChanged

            'Debug.Print("AddHMONetworkLine (Out Finally): ")

        End Try
    End Sub

    Private Function VerifyHMONetworkChanges() As Boolean

        Dim AddDRs() As DataRow

        Try
            ClearErrors()

            If _FamilyHMONetworkHistoryBS.Count > 0 AndAlso _FamilyHMONetworkHistoryBS.Position > -1 Then

                If Not Me.ValidateChildren(ValidationConstraints.Enabled) Then
                    Return False
                End If

                Dim DR As DataRow = DirectCast(_FamilyHMONetworkHistoryBS.Current, DataRowView).Row

                'start the regular validation 
                If (txtFromDate.Text.Length = 0) Then 'make sure not a blank date
                    ErrorProvider1.SetError(Me.txtFromDate, " From Date is required.")
                Else 'check the rest

                    If cmbHMONetwork.Text.Length = 0 Then
                        ErrorProvider1.SetError(cmbHMONetwork, " Please select HMO Network Value.")
                    End If

                    'new requirement(s) is to only allow prior 3 months back

                    If IsDate(DR("FROM_DATE")) AndAlso IsDate(DR("THRU_DATE")) Then
                        If (CDate(DR("FROM_DATE")) > CDate(DR("THRU_DATE"))) Then
                            ErrorProvider1.SetError(txtThruDate, " End Date must be after Begin Date")
                        End If
                    End If

                    'cannot add if in Jan or Feb, not before 1/1; go to the first of the current year in add mode                                     
                    'check in case user added then clicked off on another record and force back;              
                    AddDRs = _FamilyHMONetworkHistoryDS.Tables("HMO_NETWORK").Select("RELATION_ID=" & CInt(DR("RELATION_ID")), "FROM_DATE", DataViewRowState.Added)
                    If AddDRs.Length = 1 AndAlso (DR.RowState = DataRowState.Modified OrElse DR.RowState = DataRowState.Unchanged) Then 'Add but another record clicked
                        ErrorProvider1.SetError(txtFromDate, "Add already clicked.")
                        _FamilyHMONetworkHistoryBS.Position = _FamilyHMONetworkHistoryBS.Count
                    Else
                        If AddDRs.Length = 1 Then

                            If (IsDBNull(DR("FROM_DATE"))) Then
                                ErrorProvider1.SetError(Me.txtFromDate, " From Date is blank.")
                            End If

                            If Not CDate(DR("THRU_DATE")) = CDate("12-31-9999") Then
                                txtThruDate.Text = "12-31-9999"
                                DR("THRU_DATE") = "12-31-9999"
                            End If

                            'check current from date is later than previous date                     
                            'Dim LastFromDate As Date
                            'AddressDRs = _FamilyHMONetworkHistoryDS.Tables("HMO_NETWORK").Select("RELATION_ID=" & CInt(DR("RELATION_ID")), "FROM_DATE", DataViewRowState.OriginalRows)
                            'If AddressDRs.Count > 0 Then
                            '    For Each AddDR As DataRow In AddressDRs
                            '        LastFromDate = CDate(AddDR("FROM_DATE"))
                            '    Next
                            '    If (CDate(txtFromDt.Text) > CDate(LastFromDate)) Then
                            '        'ok
                            '    Else
                            '        ErrorProvider1.SetError(txtFromDt, "Overlapping of dates is not valid.")
                            '    End If
                            'End If

                            If (CDate(DR("FROM_DATE")) < CDate(Format(DateTime.Now, "MM/dd/yyyy")) AndAlso (DateDiff(DateInterval.Month, CDate(DR("FROM_DATE")), DateTime.Now) > 3)) Then
                                ErrorProvider1.SetError(txtFromDate, " From date is more than 3 months ago.")
                            End If

                            'cannot add more than 1 month from current date month or Jan of the next calendar year
                            If ((CDate(DR("FROM_DATE")) > CDate(Format(DateTime.Now, "MM/dd/yyyy")) AndAlso (DateDiff(DateInterval.Month, DateTime.Now, CDate(DR("FROM_DATE"))) > 1) AndAlso CDate(DR("FROM_DATE")).Year = CDate(Format(DateTime.Now, "MM-dd-yyyy")).Year) OrElse ((CDate(DR("FROM_DATE")).Year > CDate(Format(DateTime.Now, "MM-dd-yyyy")).Year) AndAlso (CDate(DR("FROM_DATE")).AddYears(-1).Year - CDate(Format(DateTime.Now, "MM-dd-yyyy")).Year = 0) AndAlso CDate(DR("FROM_DATE")).Month <> 1) OrElse ((CDate(DR("FROM_DATE")).Year > CDate(Format(DateTime.Now, "MM-dd-yyyy")).Year) AndAlso (CDate(DR("FROM_DATE")).Year - CDate(Format(DateTime.Now, "MM-dd-yyyy")).Year > 1))) Then
                                ErrorProvider1.SetError(txtFromDate, " From date is more than 1 month ahead or not January of next year.")
                            End If

                        End If 'end if for add                
                    End If 'end if for add with another record clicked

                End If 'end if for blank date                                    

                If ErrorProviderErrorsList(ErrorProvider1).Length > 0 Then
                    Return True
                End If
                Return False
            End If
        Catch ex As Exception


                Throw

        End Try
    End Function

    Public Function SaveHMONetworkChanges(ByVal activityTimestamp As DateTime, ByRef transaction As DbTransaction) As Boolean

        Dim HistSum As String = ""
        Dim HistDetail As String = ""

        Try

            _ChangedHMONetworkDRs = _FamilyHMONetworkHistoryDS.GetChanges()

            For Each DR As DataRow In _ChangedHMONetworkDRs.Tables("HMO_NETWORK").Rows

                If DR.RowState <> DataRowState.Added AndAlso DR.RowState <> DataRowState.Deleted Then 'Update

                    HistDetail = DataGridCustom.IdentifyChanges(DR, HMONetworkDataGrid)

                    HistSum = "HMO NETWORK OF FAMILYID: " & CStr(DR("FAMILY_ID")) & " WAS MODIFIED"
                    HistDetail = UFCWGeneral.DomainUser.ToUpper & " MODIFIED THE HMO NETWORK RECORD " & Microsoft.VisualBasic.vbCrLf & "THE MODIFICATIONS WERE: " & Microsoft.VisualBasic.vbCrLf & HistDetail

                    RegMasterDAL.UpdateHMONetworkInfo(activityTimestamp, If(IsDBNull(DR("HMO_ID")), Nothing, CType(DR("HMO_ID"), Integer?)), CDate(DR("FROM_DATE")), CDate(DR("THRU_DATE")), CInt(DR("FAMILY_ID")), CInt(DR("RELATION_ID")), CStr(DR("HMO_NETWORK")), CDate(DR("ONLINE_DATE")), UFCWGeneral.DomainUser.ToUpper, CDate(DR("FROM_DATE", DataRowVersion.Original)), CDate(DR("THRU_DATE", DataRowVersion.Original)), transaction)

                    RegMasterDAL.CreateRegHistory(Me.FamilyID, Me.RelationID, Nothing, Nothing, "ELIGCOVHMONETUPD", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, transaction)

                End If

                If DR.RowState = DataRowState.Added Then 'ADD

                    HistDetail = DataGridCustom.IdentifyChanges(DR, HMONetworkDataGrid)

                    HistSum = "HMO NETWORK OF FAMILYID: " & Me.FamilyID & " WAS ADDED"
                    HistDetail = UFCWGeneral.DomainUser.ToUpper & " ADDED THE HMO NETWORK RECORD " & Microsoft.VisualBasic.vbCrLf & "THE ADDITION WAS: " & Microsoft.VisualBasic.vbCrLf & HistDetail

                    RegMasterDAL.AddHMONetworkInfo(activityTimestamp, If(IsDBNull(DR("HMO_ID")), Nothing, CType(DR("HMO_ID"), Integer?)), CDate(DR("FROM_DATE")), CDate(DR("THRU_DATE")), CInt(DR("FAMILY_ID")), CInt(DR("RELATION_ID")), CStr(DR("HMO_NETWORK")), UFCWGeneral.DomainUser.ToUpper, transaction)

                    RegMasterDAL.CreateRegHistory(Me.FamilyID, Me.RelationID, Nothing, Nothing, "ELIGCOVHMONETADD", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, transaction)
                End If

            Next

            Return True

        Catch ex As Exception


                Throw
        End Try

    End Function

    Public Shared Function ErrorProviderErrorsList(ByVal provider As ErrorProvider) As String()
        Dim errors As New ArrayList
        ErrorProviderErrorsList(provider, provider.ContainerControl.Controls, errors)
        Return DirectCast(errors.ToArray(GetType(String)), String())
    End Function

    Private Shared Sub ErrorProviderErrorsList(ByVal provider As ErrorProvider, ByVal controls As Control.ControlCollection, ByVal errors As ArrayList)
        Dim s As String
        For Each ctl As Control In controls
            s = provider.GetError(ctl)
            If s.Length > 0 Then
                errors.Add(s)
            End If

            ErrorProviderErrorsList(provider, ctl.Controls, errors)
        Next
    End Sub

    Public Sub ClearErrors()
        ErrorProvider1.SetError(txtFromDate, "")
        ErrorProvider1.SetError(txtThruDate, "")
        ErrorProvider1.SetError(cmbHMONetwork, "")
    End Sub

    Public Function PendingChanges() As Boolean

        Try
            _ChangedHMONetworkDRs = _FamilyHMONetworkHistoryDS.GetChanges()
            If _ChangedHMONetworkDRs IsNot Nothing AndAlso _ChangedHMONetworkDRs.Tables("HMO_NETWORK").Rows.Count > 0 Then
                If _ChangedHMONetworkDRs IsNot Nothing AndAlso (_ChangedHMONetworkDRs.Tables("HMO_NETWORK").Rows.Count > 0) Then

                    For i = 0 To _ChangedHMONetworkDRs.Tables("HMO_NETWORK").Rows.Count - 1
                        Dim DR As DataRow = _ChangedHMONetworkDRs.Tables("HMO_NETWORK").Rows(i)
                        If DR.RowState = DataRowState.Added Then
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

    Public Sub LoadHMONetworks()
        Try

            If _AllHMONetworkDT Is Nothing OrElse _AllHMONetworkDT.Rows.Count < 1 Then
                _AllHMONetworkDT = RegMasterDAL.RetrieveHMONetworks
            End If

            'Dim HMONetworksDV As New DataView(_AllHMONetworkDT, "MEDICAL_PLAN = " & _CoverageCode.ToString, "", DataViewRowState.CurrentRows)

            cmbHMONetwork.SuspendLayout()

            RemoveHandler cmbHMONetwork.SelectedValueChanged, AddressOf cmbHMONetwork_SelectedValueChanged
            RemoveHandler cmbHMONetwork.SelectedIndexChanged, AddressOf cmbHMONetwork_SelectedIndexChanged

            cmbHMONetwork.SelectedIndex = -1

            cmbHMONetwork.ValueMember = "HMO_NETWORK"
            cmbHMONetwork.DisplayMember = "DESCRIPTION"
            cmbHMONetwork.DataSource = _AllHMONetworkDT.DefaultView

            cmbHMONetwork.ResumeLayout()

        Catch ex As Exception

                Throw

        Finally
            AddHandler cmbHMONetwork.SelectedValueChanged, AddressOf cmbHMONetwork_SelectedValueChanged
            AddHandler cmbHMONetwork.SelectedIndexChanged, AddressOf cmbHMONetwork_SelectedIndexChanged

        End Try
    End Sub
    Public Sub FilterHMONetworkValues(ByVal drv As DataRowView, Optional medFromDt As Date? = Nothing)

        Dim strfilter As String = ""

        Try

            'Debug.Print("FilterHMONetworkValues (In): " & cmbHMONetwork.SelectedIndex.ToString & " : " & If(cmbHMONetwork Is Nothing OrElse cmbHMONetwork.SelectedValue Is Nothing, "N/A", cmbHMONetwork.SelectedValue.ToString))

            If medFromDt IsNot Nothing Then
                If medFromDt = Date.Now.Date Then
                    strfilter = "'" & CDate(medFromDt).ToShortDateString & "' <= THRU_DATE "
                ElseIf medFromDt > Now.Date Then
                    strfilter = "'" & CDate(medFromDt).AddMonths(-3).ToShortDateString & "' >= FROM_DATE AND '" & CDate(medFromDt).ToShortDateString & "' <= THRU_DATE "
                Else
                    strfilter = "'" & CDate(medFromDt).ToShortDateString & "' >= FROM_DATE AND '" & CDate(medFromDt).ToShortDateString & "' <= THRU_DATE "
                End If
            End If

            If Not IsDBNull(drv("COVERAGE_CODE")) Then
                strfilter &= " AND MEDICAL_PLAN = " & drv("COVERAGE_CODE").ToString
            Else
                strfilter &= " AND MEDICAL_PLAN = " & _CoverageCode
            End If

            If _AllHMONetworkDT.DefaultView.RowFilter <> strfilter Then
                _AllHMONetworkDT.DefaultView.RowFilter = strfilter

                'Debug.Print("FilterMedCoverageValues (Mid): " & cmbHMONetwork.SelectedIndex.ToString & " : " & drv("HMO_NETWORK").ToString)
                If IsDBNull(drv("HMO_NETWORK")) Then
                    If cmbHMONetwork.SelectedIndex <> -1 Then cmbHMONetwork.SelectedIndex = -1
                Else
                    If cmbHMONetwork.SelectedValue Is Nothing OrElse (cmbHMONetwork.SelectedValue.ToString <> drv("HMO_NETWORK").ToString) Then
                        cmbHMONetwork.SelectedValue = drv("HMO_NETWORK")
                    End If
                End If

            End If

            'Debug.Print("FilterMedCoverageValues (Out): " & cmbHMONetwork.SelectedIndex.ToString & " : " & If(cmbHMONetwork Is Nothing OrElse cmbHMONetwork.SelectedValue Is Nothing, "N/A", cmbHMONetwork.SelectedValue.ToString))

        Catch ex As Exception

                Throw

        Finally

        End Try
    End Sub

    Private Sub ExitButton_Click(sender As System.Object, e As System.EventArgs) Handles ExitButton.Click
        Me.Close()
    End Sub


#End Region

#Region "Formatting"
    Private Sub DateOnlyBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            Dim txtbox As TextBox = CType(DirectCast(sender, Binding).Control, TextBox)

            If IsDBNull(e.Value) = False AndAlso Not IsDate(e.Value) Then
                If IsNumeric(e.Value) = True Then
                    Select Case e.Value.ToString.Trim.Length
                        Case Is = 8
                            e.Value = Microsoft.VisualBasic.Left(e.Value.ToString, 2) & "-" & Microsoft.VisualBasic.Mid(e.Value.ToString, 3, 2) & "-" & Microsoft.VisualBasic.Right(e.Value.ToString, 4)
                        Case Is = 6
                            e.Value = Microsoft.VisualBasic.Left(e.Value.ToString, 2) & "-" & Microsoft.VisualBasic.Mid(e.Value.ToString, 3, 2) & "-" & Microsoft.VisualBasic.Right(e.Value.ToString, 2)
                        Case Is = 5
                            e.Value = Microsoft.VisualBasic.Left(e.Value.ToString, 1) & "-" & Microsoft.VisualBasic.Mid(e.Value.ToString, 2, 2) & "-" & Microsoft.VisualBasic.Right(e.Value.ToString, 2)
                        Case Is = 4
                            e.Value = Microsoft.VisualBasic.Left(e.Value.ToString, 1) & "-" & Microsoft.VisualBasic.Mid(e.Value.ToString, 2, 1) & "-" & Microsoft.VisualBasic.Right(e.Value.ToString, 2)
                    End Select

                ElseIf CStr(e.Value).Trim.Length = 0 Then
                    e.Value = System.DBNull.Value
                End If
            End If

        Catch ex As Exception


                Throw

        End Try
    End Sub

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

    Private Sub DateOnlyBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            Dim TBox As TextBox = CType(DirectCast(sender, Binding).Control, TextBox)
            If IsDBNull(e.Value) = False Then
                e.Value = Convert.ToDateTime(String.Format("{0:MM-dd-yyyy}", e.Value)) 'handles mmddyy entry
                e.Value = Format(e.Value, "MM-dd-yyyy")
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

    Private Sub ModifyButton_Click(sender As Object, e As EventArgs) Handles ModifyButton.Click

        SetUIElements(UIStates.Modifying)

    End Sub

    Private Sub txtFromDt_Validated(sender As Object, e As EventArgs) Handles txtFromDate.Validated

        Dim TBox As TextBox = CType(sender, TextBox)

        If TBox.Enabled = False Then Return

        Dim DRs As DataRow() = _FamilyHMONetworkHistoryDS.Tables("HMO_NETWORK").Select("THRU_DATE = #12/31/9999#", "", DataViewRowState.OriginalRows)
        If DRs.Length = 1 Then
            If CDate(DRs(0)("THRU_DATE", DataRowVersion.Current)).AddDays(1) <> CDate(TBox.Text) Then
                DRs(0)("THRU_DATE") = CDate(TBox.Text).AddDays(-1)
                _FamilyHMONetworkHistoryBS.EndEdit()
            End If
        End If

    End Sub

    Private Sub HistoryButton_Click(sender As Object, e As EventArgs) Handles HistoryButton.Click
        Dim HistoryF As CoverageAndNetworkHistoryForm

        Try

            HistoryF = New CoverageAndNetworkHistoryForm
            HistoryF.FamilyID = _FamilyID
            HistoryF.RelationID = _RelationID
            HistoryF.Mode = REGMasterHistoryMode.HMONetwork
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
End Class
