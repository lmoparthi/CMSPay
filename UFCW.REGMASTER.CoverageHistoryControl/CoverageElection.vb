Option Strict On

Imports System.Windows.Forms
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ComponentModel
Imports DDTek.DB2
Imports System.Data.Common
Imports System.Text
Imports System.Threading
Imports System.Reflection
Imports System.Windows

Public Class CoverageElectionForm

#Region "Variables"
    Private _HMONetworksValuesWaitHandle As WaitHandle() = {New AutoResetEvent(False)}
    Private _MedCoverageValuesWaitHandle As WaitHandle() = {New AutoResetEvent(False)}
    Private _DenCoverageValuesWaitHandle As WaitHandle() = {New AutoResetEvent(False)}

    Private _FamilyID As Integer = -1
    Private _RelationID As Integer = -1
    Private _APPKEY As String = "UFCW\RegMaster\"
    Private _ReadOnlyMode As Boolean = True

    Private _UIState As UIStates
    Private _ViewHistory As Boolean?

    Private WithEvents _MedCoverageValuesComboBS As New BindingSource
    Private WithEvents _MedCoverageValuesDS As New DataSet

    Private WithEvents _MedCoverageBS As New BindingSource
    Private WithEvents _MedCoverageDS As New DataSet

    Private _MedChangedDRs As DataSet

    Private WithEvents _DenCoverageValuesComboBS As New BindingSource
    Private WithEvents _DenCoverageValuesDS As New DataSet

    Private WithEvents _DenCoverageBS As New BindingSource
    Private WithEvents _DenCoverageDS As New DataSet

    Private _DenChangedDRs As DataSet

    Private WithEvents _HMONetworkValuesBS As New BindingSource
    Private WithEvents _HMONetworksValuesDS As New DataSet

    Private _HMONetworkDS As DataSet

    Private _UpdatedRecord As Boolean = False '' If any changes made to records this will help to load data for CoverageHistory tab

    Private _AllCoverageDS As New DataSet
    Private _FamilyHMONetworkHistoryDS As New DataSet

    Private _Zipcode As Object
    Private _SubscriberStatus As String
    Private _Memtype As String = ""

    ReadOnly _REGMRegMasterDeleteAccess As Boolean = UFCWGeneralAD.REGMRegMasterDeleteAccess
    ReadOnly _REGMReports As Boolean = UFCWGeneralAD.REGMReports
    ReadOnly _REGMSupervisor As Boolean = UFCWGeneralAD.REGMSupervisor

#End Region

    Private _Disposed As Boolean = False

    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then

            ClearDataBindings(Me)

            ' Free any other managed objects here.
            '
            If _DenCoverageValuesComboBS IsNot Nothing Then
                _DenCoverageValuesComboBS.Dispose()
            End If
            _DenCoverageValuesComboBS = Nothing

            If _HMONetworkValuesBS IsNot Nothing Then
                _HMONetworkValuesBS.Dispose()
            End If
            _HMONetworkValuesBS = Nothing

            If _MedCoverageValuesComboBS IsNot Nothing Then
                _MedCoverageValuesComboBS.Dispose()
            End If
            _MedCoverageValuesComboBS = Nothing

            If _MedCoverageBS IsNot Nothing Then
                _MedCoverageBS.Dispose()
            End If
            _MedCoverageBS = Nothing

            _MedCoverageDS = Nothing
            _HMONetworkDS = Nothing

            If (MedCoverageDataGrid.GetCurrentDataTable) IsNot Nothing Then
                MedCoverageDataGrid.TableStyles.Clear()
            End If

            If MedCoverageDataGrid IsNot Nothing Then
                MedCoverageDataGrid.Dispose()
            End If
            MedCoverageDataGrid = Nothing

            If DenCoverageDataGrid IsNot Nothing Then
                DenCoverageDataGrid.Dispose()
            End If
            DenCoverageDataGrid = Nothing

            If _MedChangedDRs IsNot Nothing Then
                _MedChangedDRs.Dispose()
            End If
            _MedChangedDRs = Nothing

            If _DenChangedDRs IsNot Nothing Then
                _DenChangedDRs.Dispose()
            End If
            _DenChangedDRs = Nothing

            If _AllCoverageDS IsNot Nothing Then
                _AllCoverageDS.Dispose()
            End If
            _AllCoverageDS = Nothing

            If _FamilyHMONetworkHistoryDS IsNot Nothing Then
                _FamilyHMONetworkHistoryDS.Dispose()
            End If
            _FamilyHMONetworkHistoryDS = Nothing

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
            _UIState = If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable)
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

    Public Property MemType() As String
        Get
            Return _Memtype
        End Get
        Set(ByVal Value As String)

            _Memtype = Value
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
            ThreadPool.QueueUserWorkItem(AddressOf RetrieveHMONetworkValues, _HMONetworksValuesWaitHandle(0))
            ThreadPool.QueueUserWorkItem(AddressOf RetrieveMedCoverageValues, _MedCoverageValuesWaitHandle(0))
            ThreadPool.QueueUserWorkItem(AddressOf RetrieveDenCoverageValues, _DenCoverageValuesWaitHandle(0))
        End If

    End Sub

    Public Sub New(ByVal familyID As Integer, ByVal readonlymode As Boolean, ByVal zipCode As Object, ByVal subscriberStatus As String)

        Me.New()

        _FamilyID = familyID
        _Zipcode = zipCode
        _SubscriberStatus = subscriberStatus

        Me.ReadOnlyMode = readonlymode  ' use property to control UIMode

        LoadCoverage()

    End Sub

#End Region

#Region "Form\Button Events"
    Private Sub Form1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Escape Then Me.Close() ' this will be voided if changes have been made 
    End Sub

    Private Sub frmCoverageElection_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        If _UpdatedRecord Then
            Me.DialogResult = DialogResult.Yes
        End If
        SaveSettings()
        Me.Dispose()
    End Sub

    Private Sub frmCoverageElection_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try

            If Me.MedPendingChanges Then
                MessageBox.Show(Me, "Changes have been made to Medical Coverage Election Screen." & vbCrLf &
                                                 "Please Complete the changes before continuing", "Save changes", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                e.Cancel = True
            End If

            If Me.DenPendingChanges Then
                MessageBox.Show(Me, "Changes have been made to Dental Coverage Election Screen." & vbCrLf &
                                             "Please Complete the changes before continuing", "Save changes", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                e.Cancel = True
            End If

        Catch ex As Exception

                Throw
        Finally
        End Try
    End Sub

    Private Sub ProcessControls(ByRef parentCtrl As Object, ByVal readOnlyMode As Boolean, Optional ByVal excludeButtons As Boolean = False)
        'called internally and externally to switch mode of control between Read & Modify

        Dim Ctrl As Control

        Try
            If parentCtrl Is Nothing OrElse _Disposed Then Return

            Ctrl = CType(parentCtrl, Control)

            If Ctrl Is Nothing Then Return

            For Each ChildCtrl As Object In Ctrl.Controls 'recursive to accomodate groupings

                If Not (ExtensionMethods.HasProperty(ChildCtrl, NameOf(readOnlyMode)) OrElse ExtensionMethods.HasMethod(ChildCtrl, "ProcessControls")) Then
                    ProcessSubControls(ChildCtrl, readOnlyMode, excludeButtons)
                Else
                    Dim CtrlMethod As MethodInfo
                    Dim CtrlProperty As PropertyInfo
                    Dim result As Object

                    Dim SharedCtrl As Control

                    SharedCtrl = DirectCast(ChildCtrl, Control)

                    'Debug.Print(Me.Name & ": (" & readOnlyMode.ToString & ") " & If(SharedCtrl.Parent.Name IsNot Nothing, SharedCtrl.Parent.Name & " : ", "") & SharedCtrl.Name & " : " & SharedCtrl.GetType.ToString)

                    If ExtensionMethods.HasProperty(ChildCtrl, NameOf(readOnlyMode)) Then
                        CtrlProperty = ChildCtrl.GetType().GetProperty("ReadOnlyMode")
                        CtrlProperty.SetValue(ChildCtrl, readOnlyMode, Nothing)
                    End If

                    If ExtensionMethods.HasMethod(ChildCtrl, "ProcessControls") Then
                        CtrlMethod = ChildCtrl.GetType().GetMethod("ProcessControls")

                        Select Case CtrlMethod.GetParameters().Length
                            Case 1
                                result = CtrlMethod.Invoke(ChildCtrl, New Object() {ChildCtrl})
                            Case 2
                                result = CtrlMethod.Invoke(ChildCtrl, New Object() {ChildCtrl, readOnlyMode})
                            Case 3
                                result = CtrlMethod.Invoke(ChildCtrl, New Object() {ChildCtrl, readOnlyMode, Nothing})
                        End Select
                    End If

                End If
            Next

        Catch ex As Exception

                Throw
        Finally
        End Try

    End Sub

    Public Shared Sub ProcessSubControls(ByRef ctrl As Object, ByVal readOnlyMode As Boolean, Optional ByVal excludeButtons As Boolean = False)

        Dim ProcessingControl As Control

        Try
            ProcessingControl = DirectCast(ctrl, Control)

            If ProcessingControl.IsDisposed Then Return

            '  Ignore the control unless it's a textbox.
            'Debug.Print(If(ProcessingControl.Parent.Name IsNot Nothing, ProcessingControl.Parent.Name & " : ", "") & ProcessingControl.Name & " : " & ctrl.GetType.ToString & " : " & If(TypeOf (ctrl) Is TextBox, CType(ctrl, TextBox).ReadOnly, ProcessingControl.Enabled).ToString & " -> " & If(TypeOf (ctrl) Is TextBox, readOnlyMode, Not readOnlyMode).ToString)

            If TypeOf (ctrl) Is RadioButton OrElse TypeOf (ctrl) Is TextBox OrElse TypeOf (ctrl) Is ComboBox OrElse TypeOf (ctrl) Is DateTimePicker OrElse TypeOf (ctrl) Is Button OrElse TypeOf (ctrl) Is CheckBox OrElse TypeOf (ctrl) Is Label OrElse TypeOf (ctrl) Is DataGrid Then
                If TypeOf (ctrl) Is Label Then
                    CType(ctrl, Label).Enabled = True
                ElseIf TypeOf (ctrl) Is DataGrid Then
                ElseIf TypeOf (ctrl) Is TextBox Then
                    If CType(ctrl, TextBox).ReadOnly <> readOnlyMode Then
                        CType(ctrl, TextBox).ReadOnly = readOnlyMode
                    End If
                ElseIf TypeOf (ctrl) Is ExComboBox Then
                    If CType(ctrl, ExComboBox).ReadOnly <> readOnlyMode Then
                        CType(ctrl, ExComboBox).ReadOnly = readOnlyMode
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
                ElseIf TypeOf (ctrl) Is ExCheckBox Then
                    If CType(ctrl, ExCheckBox).ReadOnly <> readOnlyMode Then
                        CType(ctrl, ExCheckBox).ReadOnly = readOnlyMode
                    End If
                ElseIf TypeOf (ctrl) Is CheckBox Then
                    If CType(ctrl, CheckBox).Enabled = readOnlyMode Then
                        CType(ctrl, CheckBox).Enabled = Not readOnlyMode
                    End If
                End If

            End If

            For Each ChildCtrl As Object In ProcessingControl.Controls
                ProcessSubControls(ChildCtrl, readOnlyMode, excludeButtons)
            Next

            'Debug.Print(If(ProcessingControl.Parent.Name IsNot Nothing, ProcessingControl.Parent.Name & " : ", "") & ProcessingControl.Name & " : " & ctrl.GetType.ToString & " : " & If(TypeOf (ctrl) Is TextBox, CType(ctrl, TextBox).ReadOnly, ProcessingControl.Enabled).ToString & " -> " & If(TypeOf (ctrl) Is TextBox, readOnlyMode, Not readOnlyMode).ToString)

        Catch ex As Exception

                Throw
        Finally

        End Try

    End Sub

#End Region

#Region "Dental"
    Private Sub cmbDen_DropDown(sender As Object, e As EventArgs) Handles cmbDenCoverageValues.DropDown

        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _DenCoverageBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Try

            ClearError(ErrorProvider1, CBox)

            CBox.Invalidate()
            grpDenEditPanel.Refresh()

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _DenCoverageBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub DenCoverageValuesComboBS_CurrentItemChanged(sender As Object, e As EventArgs) Handles _DenCoverageValuesComboBS.CurrentItemChanged
        Dim BS As BindingSource

        Try

            BS = CType(sender, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If Not (_UIState And UIStates.[ReadOnly]) = _UIState Then

                _DenCoverageBS.EndEdit()

            End If

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub RetrieveDenCoverageValues(threadState As [Object])

        Dim DT As DataTable
        Dim AREvent As AutoResetEvent = CType(threadState, AutoResetEvent)

        Try

            DT = RegMasterDAL.RetrieveDentalCoverageValues
            _DenCoverageValuesDS.Merge(DT)

        Catch ex As Exception

                Throw

        Finally
            AREvent.Set()

        End Try
    End Sub

    Private Sub SetDenUIElements(Optional uiState As UIStates = UIStates.AsIs)

        Dim BS As BindingSource

        Dim ControlIsReadOnly As UIStates = UIStates.None Or UIStates.NotModifiable Or UIStates.Modifiable

        Dim CM As CurrencyManager
        Dim DR As DataRow

        Dim HoldAutoValidateState As AutoValidate
        Dim SavedDenCoverageBSRaiseListChangedEvents As Boolean

        Try

            BS = DirectCast(_DenCoverageBS, BindingSource)

            HoldAutoValidateState = Me.AutoValidate

            Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable 'prevent validation from firing during control manipulation

            If Not uiState.HasFlag(UIStates.AsIs) Then _UIState = uiState

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") State:" & _UIState.ToString & " " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ProcessControls(CType(grpDenEditPanel, Object), True) 'does not affect visibility only edit yes/no

            Me.DenDeleteButton.Enabled = False
            Me.DenDeleteButton.Visible = False

            Me.DenModifyButton.Enabled = False
            Me.DenModifyButton.Visible = False

            Me.DenSaveButton.Enabled = False
            Me.DenSaveButton.Visible = False

            Me.DenCancelButton.Enabled = False
            Me.DenCancelButton.Visible = False

            Me.DenAddButton.Enabled = False
            Me.DenAddButton.Visible = False

            If _UIState = UIStates.None Then Return

            If Not _UIState = UIStates.None AndAlso (_ViewHistory Is Nothing OrElse _ViewHistory) Then
                DenHistoryButton.Visible = True
                DenHistoryButton.Enabled = True
            End If

            CM = _DenCoverageBS.CurrencyManager

            If SavedDenCoverageBSRaiseListChangedEvents Then
                _DenCoverageBS.RaiseListChangedEvents = False
            End If

            If _UIState <> UIStates.Canceling AndAlso CM IsNot Nothing AndAlso CM.Count > 0 AndAlso CM.Position > -1 Then
                DR = DirectCast(CM.Current, DataRowView).Row
            End If

            Select Case _UIState 'this block resets state for remainder of control processing

                Case UIStates.Canceled

                    lblUIMessaging.Text = "Change(s) Cancelled."
                    lblUIMessaging.Visible = True

                    _UIState = If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable)

                Case UIStates.Saved

                    lblUIMessaging.Text = "Change(s) Saved."
                    lblUIMessaging.Visible = True

                    _UIState = If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable)

                Case UIStates.Rejected

                    lblUIMessaging.Text = "Coverage Rejected."
                    lblUIMessaging.Visible = True

                    _UIState = If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable)

                Case UIStates.Approved

                    lblUIMessaging.Text = "Coverage Approved."
                    lblUIMessaging.Visible = True

                    _UIState = If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable)

                Case UIStates.Deleted 'not implemented

                    lblUIMessaging.Text = "Coverage Deleted."
                    lblUIMessaging.Visible = True

                    _UIState = If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable)

                Case UIStates.Archived 'not implemented

                    lblUIMessaging.Text = "Coverage Archived."
                    lblUIMessaging.Visible = True

                    _UIState = If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable)
            End Select

            Select Case _UIState
                Case UIStates.None

                    Return

                Case UIStates.Rejecting

                Case UIStates.Saving

                    ProcessControls(CType(grpDenEditPanel, Object), False, True) 'allow controls to validate

                Case UIStates.Canceling

                Case UIStates.Viewing

                Case UIStates.NotModifiable

                Case UIStates.Modifiable, UIStates.Adding, UIStates.Modifying

                    Me.DenAddButton.Visible = True
                    Me.DenModifyButton.Visible = True
                    Me.DenSaveButton.Visible = True
                    Me.DenCancelButton.Visible = True

                    Select Case _UIState
                        Case UIStates.Modifiable

                            'only most recent row can be modified or deleted.
                            If DR IsNot Nothing Then

                                If _REGMRegMasterDeleteAccess Then Me.DenDeleteButton.Visible = True

                                If Not IsDBNull(DR("FROM_DATE")) Then
                                    If _REGMReports Then
                                        Me.DenDeleteButton.Enabled = True
                                        Me.DenModifyButton.Enabled = True
                                    ElseIf (DateDiff(DateInterval.Month, Now, CDate(DR("FROM_DATE"))) > -12 AndAlso CDate(DR("THRU_DATE")).Year = 9999) Then
                                        If _REGMRegMasterDeleteAccess Then Me.DenDeleteButton.Enabled = True
                                        Me.DenModifyButton.Enabled = True
                                    End If
                                End If
                            End If

                            If _DenCoverageDS.Tables("ELIG_COVERAGE") IsNot Nothing Then
                                Dim DRs As DataRow() = _DenCoverageDS.Tables("ELIG_COVERAGE").Select("THRU_DATE = #12/31/9999#")
                                If DRs.Length < 1 Then
                                    Me.DenAddButton.Enabled = True
                                Else
                                    If CDate(DRs(0)("FROM_DATE")) < Date.Now Then Me.DenAddButton.Enabled = True
                                End If
                            End If

                        Case UIStates.Adding, UIStates.Modifying

                            If DR IsNot Nothing AndAlso CDate(DR("THRU_DATE")).Year = 9999 AndAlso (DR.HasVersion(DataRowVersion.Proposed) OrElse DR.RowState = DataRowState.Modified OrElse DR.RowState = DataRowState.Added) Then 'in addition to current/future active addresses, items awaiting approval will also have 9999 thru date 

                                ProcessControls(CType(grpDenEditPanel, Object), False, True)

                            Else
                                ClearErrors()
                            End If

                            Me.DenCancelButton.Enabled = True

                    End Select

                Case UIStates.Deleting

                    If CM.Position < 0 Then
                        cmbDenCoverageValues.SelectedIndex = -1
                    End If

                    Me.DenSaveButton.Visible = True
                    Me.DenCancelButton.Visible = True

                    lblUIMessaging.Text = "Delete Pending."
                    lblUIMessaging.Visible = True

                Case UIStates.Canceling

                    _DenCoverageValuesComboBS.RemoveFilter()

            End Select

        Catch ex As Exception
            Throw
        Finally

            txtDenThruDate.ReadOnly = True

            If txtDenFromDate.ReadOnly = True Then
                ClearErrors()
            End If

            If _UIState <> UIStates.Canceling Then
                _DenChangedDRs = _DenCoverageDS.GetChanges(DataRowState.Added Or DataRowState.Modified Or DataRowState.Deleted)
                If _DenChangedDRs IsNot Nothing AndAlso _DenChangedDRs.Tables("ELIG_COVERAGE").Rows.Count > 0 Then
                    Me.DenSaveButton.Enabled = True
                    Me.DenCancelButton.Enabled = True
                End If
            End If

            Me.AutoValidate = HoldAutoValidateState

            If SavedDenCoverageBSRaiseListChangedEvents Then
                _DenCoverageBS.RaiseListChangedEvents = True
            End If

            grpDenEditPanel.Refresh()

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out:  BS(" & BS.Position.ToString & ") State:" & _UIState.ToString & " " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

    Private Sub DenHistoryButton_Click(sender As Object, e As EventArgs) Handles DenHistoryButton.Click
        Dim FRM As RegMasterHistoryForm

        Try

            FRM = New RegMasterHistoryForm
            FRM.FamilyID = _FamilyID
            FRM.RelationID = _RelationID
            FRM.Mode = REGMasterHistoryMode.DentalCoverage
            FRM.ShowDialog()

            FRM.Close()

        Catch ex As Exception
            Throw
        Finally
            If FRM IsNot Nothing Then FRM.Dispose()
            FRM = Nothing
        End Try

    End Sub

    Private Sub DenDeleteButton_Click(sender As Object, e As EventArgs) Handles DenDeleteButton.Click

        Try

            DeleteDenCoverageLine()

        Catch ex As Exception


                Throw

        Finally
        End Try

    End Sub

    Private Sub cmbDenCoverageValues_Validated(sender As Object, e As EventArgs) Handles cmbDenCoverageValues.Validated

        Dim CBox As ExComboBox = CType(sender, ExComboBox)
        Dim BS As BindingSource
        Dim PBS As BindingSource

        Try

            If _Disposed OrElse _DenCoverageBS Is Nothing OrElse _DenCoverageBS.Position < 0 OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

            BS = _DenCoverageBS
            PBS = _DenCoverageValuesComboBS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Dim DR As DataRow = DirectCast(_DenCoverageBS.Current, DataRowView).Row

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Mid: BS(" & _DenCoverageBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If Not DR.HasVersion(DataRowVersion.Original) OrElse DR("COVERAGE_CODE").ToString <> CBox.SelectedValue.ToString Then
                CBox.DataBindings("SelectedValue").WriteValue()
            End If

            _DenCoverageBS.EndEdit() 'triggers uisetup

            CType(_DenCoverageValuesComboBS.Current, DataRowView).Row.BeginEdit() 'used to trigger combo BS change and update HMO Networks
            CType(_DenCoverageValuesComboBS.Current, DataRowView).Row.EndEdit()

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally

        End Try
    End Sub
    Private Sub cmbDenCoverageValues_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbDenCoverageValues.SelectionChangeCommitted

        Dim DR As DataRow
        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Try

            If _DenCoverageBS Is Nothing OrElse _DenCoverageBS.Position < 0 OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _DenCoverageBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            CType(CBox.Parent, TransparentContainer).ValidateChildren() 'this will trigger validation of the cmbbox triggering write of value to DS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _DenCoverageBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally


        End Try

    End Sub

    Private Sub DenCoverageBS_OnListChanged(sender As Object, e As System.ComponentModel.ListChangedEventArgs) Handles _DenCoverageBS.ListChanged

        Dim BS As BindingSource

        Try
            BS = CType(sender, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") RS(" & If(BS.Position < 0 OrElse BS.Current Is Nothing, "N/A", DirectCast(BS.Current, System.Data.DataRowView).Row.RowState.ToString) & ") O(" & e.OldIndex.ToString & ") N(" & e.NewIndex.ToString & ") CT(" & e.ListChangedType.ToString & ") ST(" & _UIState.ToString & ") SEL(" & If(BS Is Nothing OrElse BS.Count < 1 OrElse BS.Position < 0 OrElse DenCoverageDataGrid.DataSource Is Nothing, "N/A", DenCoverageDataGrid.IsSelected(BS.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Select Case e.ListChangedType
                Case ListChangedType.ItemDeleted 'account for rows deleted due to a cancel action

                    Select Case _UIState
                        Case UIStates.Modifying


                        Case Else

                            If BS.Position > -1 AndAlso BS.Position <> e.NewIndex Then
                                If e.NewIndex > -1 Then BS.Position = e.NewIndex
                                SetDenUIElements()
                            End If

                    End Select

                Case ListChangedType.ItemMoved

                    Select Case DirectCast(BS.Current, System.Data.DataRowView).Row.RowState
                        Case DataRowState.Modified

                            Select Case _UIState
                                Case UIStates.Adding
                                Case Else
                            End Select

                        Case DataRowState.Added
                            Select Case _UIState
                                Case UIStates.Adding

                                    If e.OldIndex <> e.NewIndex OrElse (BS.Position > -1 AndAlso BS.Count = 1) Then
                                        DenCoverageDataGrid.Select(BS.Position)
                                    End If

                                Case Else
                            End Select

                        Case Else

                            Select Case _UIState
                                Case UIStates.Adding

                                Case Else
                            End Select

                    End Select

                Case ListChangedType.ItemChanged

                    If BS.Count = 0 Then 'item was changed in some way that excludes it from list due to filter exclusion
                        Return
                    End If

                    Select Case DirectCast(BS.Current, System.Data.DataRowView).Row.RowState
                        Case DataRowState.Modified

                            Select Case _UIState
                                Case UIStates.Adding
                                Case Else
                            End Select

                        Case DataRowState.Added
                            Select Case _UIState
                                Case UIStates.Adding
                                    'do nothing
                                Case Else
                            End Select

                        Case Else

                            Select Case _UIState
                                Case UIStates.Adding
                                    If BS.Position <> e.NewIndex OrElse BS.Position > -1 AndAlso BS.Count = 1 Then
                                        If e.NewIndex > -1 Then BS.Position = e.NewIndex
                                        SetDenUIElements()
                                        If e.NewIndex > -1 Then DenCoverageDataGrid.Select(e.NewIndex)
                                    End If
                                Case Else
                            End Select

                    End Select

                Case ListChangedType.Reset 'triggered by sorts or changes in grid filter

                    If BS.Position > -1 AndAlso BS.Position <> e.NewIndex Then
                        If e.NewIndex > -1 Then BS.Position = e.NewIndex
                        BS.ResetCurrentItem()
                        SetDenUIElements()
                    End If

                Case ListChangedType.ItemAdded 'includes items reincluded when filters change

                    Select Case _UIState
                        Case UIStates.Adding
                            If e.NewIndex <> e.OldIndex AndAlso e.OldIndex > -1 Then
                                DenCoverageDataGrid.UnSelect(e.OldIndex)
                            End If

                            If BS.Position <> e.NewIndex OrElse (BS.Position > -1 AndAlso BS.Count = 1) Then 'first item added
                                If e.NewIndex > -1 Then BS.Position = e.NewIndex
                                If e.NewIndex > -1 Then DenCoverageDataGrid.Select(e.NewIndex)
                            End If
                        Case Else
                    End Select

                Case Else

            End Select

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") RS(" & If(BS.Position < 0 OrElse BS.Current Is Nothing, "N/A", DirectCast(BS.Current, System.Data.DataRowView).Row.RowState.ToString) & ") O(" & e.OldIndex.ToString & ") N(" & e.NewIndex.ToString & ") CT(" & e.ListChangedType.ToString & ") ST(" & _UIState.ToString & ") SEL(" & If(BS Is Nothing OrElse BS.Count < 1 OrElse BS.Position < 0 OrElse DenCoverageDataGrid.DataSource Is Nothing, "N/A", DenCoverageDataGrid.IsSelected(BS.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try


    End Sub

    Private Sub cmbDenLetter_Validating(sender As Object, e As CancelEventArgs) Handles cmbDenLetter.Validating

        Dim CBox As ExComboBox = CType(sender, ExComboBox)
        Dim BS As BindingSource
        Dim PBS As BindingSource
        Dim DR As DataRow

        Try

            If _Disposed OrElse _DenCoverageBS Is Nothing OrElse _DenCoverageBS.Position < 0 OrElse CBox.ReadOnly Then Return

            BS = _DenCoverageBS
            PBS = _DenCoverageValuesComboBS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
            DR = CType(_DenCoverageBS.Current, DataRowView).Row

            ClearError(ErrorProvider1, CBox)

            If Not (_UIState And UIStates.[ReadOnly]) = _UIState Then

                If CBox.SelectedIndex < 0 Then
                    ErrorProvider1.SetErrorWithTracking(CBox, " Letter decision required.")
                End If
            End If

            If ErrorProvider1.GetError(CBox).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally

        End Try

    End Sub

    Private Function UnCommittedDenChangesExist() As Boolean

        Dim Modifications As String = ""
        Try

            _DenChangedDRs = _DenCoverageDS.GetChanges()

            If DenCoverageDataGrid IsNot Nothing AndAlso _DenChangedDRs IsNot Nothing AndAlso _DenChangedDRs.Tables("ELIG_COVERAGE").Rows.Count > 0 Then

                For Each DR As DataRow In _DenChangedDRs.Tables("ELIG_COVERAGE").Rows
                    If DR.RowState <> DataRowState.Added Then
                        'attempt to exclude rows accidently changed during navigation operations
                        Modifications = DataGridCustom.IdentifyChanges(DR, DenCoverageDataGrid)

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

    Private Sub CancelDenChanges()

        Dim GoToPosition As Integer

        Try
            _DenChangedDRs = _DenCoverageDS.GetChanges()

            GoToPosition = _DenCoverageBS.Position

            If _DenChangedDRs IsNot Nothing Then
                Dim DR = (From a In _DenChangedDRs.Tables("ELIG_COVERAGE").Rows.Cast(Of DataRow)() Where a.RowState = DataRowState.Added Select a).FirstOrDefault
                If DR IsNot Nothing Then GoToPosition = 0
            End If

            _DenCoverageBS.RaiseListChangedEvents = False

            _DenCoverageDS.Tables("ELIG_COVERAGE").RejectChanges()

            _DenCoverageBS.RaiseListChangedEvents = True
            _DenCoverageBS.ResetBindings(False)

            If _DenCoverageBS IsNot Nothing AndAlso _DenCoverageBS.Position > -1 AndAlso _DenCoverageBS.Count > 0 AndAlso _DenCoverageBS.Count > GoToPosition Then
                If _DenCoverageBS.Position <> GoToPosition Then
                    DenCoverageDataGrid.UnSelect(_DenCoverageBS.Position)
                    _DenCoverageBS.Position = GoToPosition
                    DenCoverageDataGrid.Select(_DenCoverageBS.Position)
                End If
            End If

            ClearErrors()

        Catch ex As Exception

                Throw
        Finally

            _UIState = UIStates.Canceled
        End Try
    End Sub

    Private Sub DenCancelChanges()

        Dim HoldState As UIStates = _UIState

        Try

            SetDenUIElements(UIStates.Canceling)

            If UnCommittedDenChangesExist() = False Then

                CancelDenChanges()

            Else

                Dim Result As DialogResult = MessageBox.Show(Me, "Do you want to Cancel Dental changes?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                If Result = DialogResult.Yes Then

                    CancelDenChanges()

                Else
                    _UIState = HoldState
                End If

            End If

        Catch ex As Exception
            Throw
        Finally

            SetDenUIElements()

        End Try
    End Sub

    Private Sub ModifyButton_Click(sender As Object, e As EventArgs) Handles DenModifyButton.Click, MedModifyButton.Click

        Dim CButton As Button
        Dim DR As DataRow

        Try
            CButton = CType(sender, Button)
            CButton.Enabled = False

            If CButton.Name = "MedModifyButton" Then
                DR = CType(_MedCoverageBS.Current, DataRowView).Row
                txtMedFromDate.Focus()
            Else
                DR = CType(_DenCoverageBS.Current, DataRowView).Row
                txtDenFromDate.Focus()
            End If

            DR.BeginEdit() 'will create a Proposed Version that can be used to indicate edits are underway.

            If CButton.Name = "MedModifyButton" Then
                SetMedUIElements(UIStates.Modifying)
            Else
                SetDenUIElements(UIStates.Modifying)
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub DenAddButton_Click(sender As System.Object, e As System.EventArgs) Handles DenAddButton.Click

        Dim EligcalcDS As DataSet
        Dim EligCalcDT As New DataTable

        Try

            DenAddButton.Enabled = False

            EligcalcDS = RegMasterDAL.RetrieveEligCalcElementsByFamilyID(_FamilyID)  '' checking if there is a row in elg calc elements table
            EligCalcDT = EligcalcDS.Tables("ELGCALC_ELEMENTS")

            If (EligCalcDT) IsNot Nothing AndAlso EligCalcDT.Rows.Count = 0 Then
                MessageBox.Show(" Please establish Entrydate, Local and Memtype" & Environment.NewLine & " before adding coverage.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            ElseIf (EligCalcDT IsNot Nothing) AndAlso (EligCalcDT.Rows.Count > 0) AndAlso (CStr(EligCalcDT.Rows(0)("LAST_MEMTYPE")).Trim.Length = 0) Then
                MessageBox.Show(" Please establish Memtype before adding coverage.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            _UIState = UIStates.Adding

            AddDenCoverageLine()

            txtDenFromDate.Focus()

        Catch ex As Exception


                Throw

        Finally

            If EligcalcDS IsNot Nothing Then EligcalcDS.Dispose()
            EligcalcDS = Nothing

            If EligCalcDT IsNot Nothing Then EligCalcDT.Dispose()
            EligCalcDT = Nothing

            SetDenUIElements()
        End Try
    End Sub

    Private Sub DenSaveButton_Click(sender As Object, e As EventArgs) Handles DenSaveButton.Click

        Dim HoldAutoValidateState As AutoValidate
        Dim Deleted As Boolean = False
        Dim StartingUISTate As UIStates = _UIState

        Try

            HoldAutoValidateState = Me.AutoValidate

            DenSaveButton.Enabled = False 'this will reenable if the underlying dataset changes
            DenCancelButton.Enabled = False

            If Not _UIState = UIStates.Deleting Then

                If Not DenUIContainer.ValidateChildren(ValidationConstraints.Enabled) OrElse ValidateDenCoverageChanges() Then
                    DenSaveButton.Enabled = True
                    DenCancelButton.Enabled = True
                    _UIState = StartingUISTate
                    Return
                Else
                    _UIState = UIStates.Saving
                End If

            Else
                Deleted = True
            End If

            _DenCoverageBS.EndEdit()

            _DenChangedDRs = _DenCoverageDS.GetChanges()

            If _DenChangedDRs IsNot Nothing Then

                SetDenUIElements(UIStates.Saving)

                If SaveDenChanges() Then

                    _UIState = UIStates.Saved

                    If Deleted Then
                        _UIState = UIStates.Deleted
                    End If

                    SetDenUIElements()

                    MessageBox.Show("Dental Coverage Record " & If(Deleted, "Deleted", "Saved") & " Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    '' calculate eligibilty for the current elig period

                    RegMasterDAL.MadeDBChanges = True  '' this is to get elig_acct_hours from database to show pending/ or not
                    RegMasterDAL.MadeEligibilityChanges = True  '' FOR summary screen to display correct coverage

                    _UpdatedRecord = True

                End If

            Else

                Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable

                DenCancelChanges()

                MessageBox.Show("No changes were made. Save request resulted in no action being taken.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If

        Catch db2ex As DB2Exception

            Dim MessageText As String
            Dim TitleText As String

            Select Case db2ex.Number
                Case -438, -1822
                    MessageText = "The item(s) you are attempting to update has been modified." &
                                       vbCrLf & "Attempted changes was not allowed. " &
                                       vbCrLf & "Data has been reset to original values." & vbCrLf &
                                       vbCrLf & "Refresh data before attempting additional changes."

                    TitleText = "Changes Rejected"

                Case Else

                    MessageText = "Critical database error. " & db2ex.Message
                    TitleText = "Critical database error. "

            End Select

            MessageBox.Show(MessageText, TitleText, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            DenCancelChanges()

        Catch ex As Exception

            DenCancelChanges()


                Throw

        Finally

            SetDenUIElements()

            Me.AutoValidate = HoldAutoValidateState

        End Try
    End Sub

    Private Sub LoadDenCoverageValuesCombo(Optional denFromDt As Date? = Nothing)

        Try

            'wait for thread.
            If _DenCoverageValuesWaitHandle IsNot Nothing Then
                WaitHandle.WaitAll(_DenCoverageValuesWaitHandle)
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _MedCoverageValuesComboBS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

#If DEBUG Then
            RemoveHandler cmbDenCoverageValues.SelectedIndexChanged, AddressOf cmbDenCoverageValues_SelectedIndexChanged
#End If

            cmbDenCoverageValues.DataSource = Nothing
            cmbDenCoverageValues.Items.Clear()

            _DenCoverageValuesComboBS.DataSource = _DenCoverageValuesDS.Tables("COVERAGE_VALUES")
            _DenCoverageValuesComboBS.Sort = "COVERAGE_VALUE"

        Catch ex As Exception

                Throw

        Finally

#If DEBUG Then
            AddHandler cmbDenCoverageValues.SelectedIndexChanged, AddressOf cmbDenCoverageValues_SelectedIndexChanged
#End If
            _DenCoverageValuesWaitHandle = Nothing

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _MedCoverageValuesComboBS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

    Private Sub AddDenCoverageLine()

        Dim DR As DataRow

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _DenCoverageBS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = _DenCoverageDS.Tables("ELIG_COVERAGE").NewRow
            DR.BeginEdit()

            DR("FAMILY_ID") = _FamilyID
            DR("RELATION_ID") = 0

            DR("COVERAGE_TYPE") = "DENTAL"
            DR("COVERAGE_CODE") = System.DBNull.Value
            DR("FROM_DATE") = CDate(Date.Now.AddMonths(+1).Month & "/01/" & Date.Now.AddMonths(+1).Year)
            DR("THRU_DATE") = "12/31/9999"

            DR("CREATE_USERID") = UFCWGeneral.DomainUser.ToUpper
            DR("CREATE_DATE") = Date.Now

            _UIState = UIStates.Adding

            DR.EndEdit()

            _DenCoverageDS.Tables("ELIG_COVERAGE").Rows.Add(DR)

        Catch ex As Exception


                Throw
        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _DenCoverageBS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

    Private Sub LoadDenDataBindings()
        Dim Bind As Binding

        Try

            txtDenFromDate.DataBindings.Clear()
            Bind = New Binding("Text", _DenCoverageBS, "FROM_DATE", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            Bind.FormatString = "MM-dd-yyyy"
            AddHandler Bind.Parse, AddressOf UFCWGeneral.DateOnlyBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf txtDenFromDateBindingComplete
            txtDenFromDate.DataBindings.Add(Bind)

            txtDenThruDate.DataBindings.Clear()
            Bind = New Binding("Text", _DenCoverageBS, "THRU_DATE", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            Bind.FormatString = "MM-dd-yyyy"
            AddHandler Bind.Parse, AddressOf UFCWGeneral.DateOnlyBinding_Parse
            txtDenThruDate.DataBindings.Add(Bind)

            cmbDenCoverageValues.DataSource = _DenCoverageValuesComboBS
            cmbDenCoverageValues.ValueMember = "COVERAGE_VALUE"
            cmbDenCoverageValues.DisplayMember = "DESC"

            cmbDenCoverageValues.DataBindings.Clear()
            Bind = New Binding("SelectedValue", _DenCoverageBS, "COVERAGE_CODE", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            cmbDenCoverageValues.DataBindings.Add(Bind)

        Catch ex As Exception


                Throw

        Finally

        End Try
    End Sub

    Private Sub txtDenFromDateBindingComplete(sender As Object, e As BindingCompleteEventArgs)

        Dim ControlBinding As Binding

        Try

            ControlBinding = CType(sender, Binding)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _DenCoverageBS.Position.ToString & ") " & Me.Name & " : (" & ControlBinding.Control.Name & ") " & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If Not e.BindingCompleteState = BindingCompleteState.Success Then
                MessageBox.Show("Control " & e.Binding.Control.Name & " " & e.ErrorText, "Problem converting data to database format", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _DenCoverageBS.Position.ToString & ") " & Me.Name & " : (" & ControlBinding.Control.Name & ") " & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

    Private Function DenPendingChanges() As Boolean
        Dim Modifications As String = ""

        Try

            _DenChangedDRs = _DenCoverageDS.GetChanges()

            If _DenChangedDRs IsNot Nothing Then

                For Each DR As DataRow In _DenChangedDRs.Tables("ELIG_COVERAGE").Rows

                    If DR.RowState <> DataRowState.Added Then

                        Modifications = DataGridCustom.IdentifyChanges(DR, DenCoverageDataGrid)

                        If Modifications.Length > 0 Then
                            Return True
                        End If

                    ElseIf DR.RowState = DataRowState.Added Then
                        Return True
                    End If

                Next
            End If

            Return False

        Catch ex As Exception

                Throw
        End Try

    End Function

    Private Function ValidateDenCoverageChanges() As Boolean
        Dim DenCoverageDRs() As DataRow

        Try
            ClearErrors()

            Dim DR As DataRow = DirectCast(_DenCoverageBS.Current, DataRowView).Row

            If IsDate(DR("FROM_DATE")) OrElse IsDate(txtDenFromDate.Text) Then
            Else
                ErrorProvider1.SetErrorWithTracking(txtDenFromDate, " From Date is invalid (MM-dd-yyyy)")
            End If

            If IsDate(DR("THRU_DATE")) OrElse IsDate(txtDenThruDate.Text) Then
            Else
                ErrorProvider1.SetErrorWithTracking(txtDenThruDate, " Thru Date is invalid (MM-dd-yyyy)")
            End If

            If IsDate(DR("FROM_DATE")) AndAlso IsDate(DR("THRU_DATE")) Then
                If (CDate(DR("FROM_DATE")) < CDate(DR("THRU_DATE"))) Then

                Else
                    ErrorProvider1.SetErrorWithTracking(Me.txtDenThruDate, " Thru Date must be after From Date")
                End If
            End If

            If DR("COVERAGE_CODE").ToString.Trim.Length = 0 Then
                ErrorProvider1.SetErrorWithTracking(Me.cmbDenCoverageValues, " Select a valid Dental Plan Coverage code")
            End If

            '' Duplicate from date and coveragetype for same period
            If IsDate(DR("FROM_DATE")) AndAlso Not IsDBNull(DR("COVERAGE_TYPE")) Then
                DenCoverageDRs = _DenCoverageDS.Tables("ELIG_COVERAGE").Select(" FROM_DATE= '" & CStr(DR("FROM_DATE")) & "' ", "", DataViewRowState.CurrentRows)
                If DenCoverageDRs.Length > 1 Then
                    ErrorProvider1.SetErrorWithTracking(txtDenFromDate, "Another coverage record starts from same From Date. Please provide different From date.")
                End If
            End If

            ''  Finding any overlaping period is occuring when adding record at the end
            If DR.RowState = DataRowState.Added Then

                If IsDate(DR("FROM_DATE")) AndAlso IsDate(DR("THRU_DATE")) AndAlso CDate(DR("THRU_DATE")) = CDate("12-31-9999") Then
                    DenCoverageDRs = _DenCoverageDS.Tables("ELIG_COVERAGE").Select("'" & CStr(DR("FROM_DATE")) & "' > FROM_DATE  AND '" & CStr(DR("FROM_DATE")) & "' < THRU_DATE", "", DataViewRowState.CurrentRows)
                    If DenCoverageDRs.Length > 1 Then
                        ErrorProvider1.SetErrorWithTracking(txtDenFromDate, "Overlapping of coverage is not valid. Please provide different FROM Date.")
                    End If
                End If
            End If

            ''  Finding any overlaping period is occuring when modifying the record
            Dim OverLapping As Boolean = False
            If DR.RowState = DataRowState.Modified Then
                If (Not IsDBNull(DR("FROM_DATE"))) AndAlso (Not IsDBNull(DR("THRU_DATE"))) AndAlso (Not IsDBNull(DR("COVERAGE_TYPE"))) Then
                    DenCoverageDRs = _DenCoverageDS.Tables("ELIG_COVERAGE").Select("COVERAGE_TYPE = '" & CStr(DR("COVERAGE_TYPE")) & "'", "", DataViewRowState.Unchanged)
                    If DenCoverageDRs.Length > 0 Then
                        For Each CoverageDR As DataRow In DenCoverageDRs
                            OverLapping = OverlappingPeriods(CDate(DR("FROM_DATE")), CDate(DR("THRU_DATE")), CDate(CoverageDR("FROM_DATE")), CDate(CoverageDR("THRU_DATE")))
                            If OverLapping = True Then
                                MessageBox.Show("Overlapping of coverage is not valid." & Environment.NewLine & " Please correct the dates before Continuing", "Overlapping", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Return True
                            End If
                        Next
                    End If
                End If
            End If


            ''  Finding any overlaping period is occuring when adding record in the middle of coverage periods
            OverLapping = False
            If IsDate(DR("THRU_DATE")) Then
                If DR.RowState = DataRowState.Added AndAlso CDate(DR("THRU_DATE")) <> CDate("12-31-9999") Then
                    If (Not IsDBNull(DR("FROM_DATE"))) AndAlso (Not IsDBNull(DR("THRU_DATE"))) AndAlso (Not IsDBNull(DR("COVERAGE_TYPE"))) Then
                        DenCoverageDRs = _DenCoverageDS.Tables("ELIG_COVERAGE").Select("COVERAGE_TYPE = '" & CStr(DR("COVERAGE_TYPE")) & "'", "", DataViewRowState.Unchanged)
                        If DenCoverageDRs.Length > 0 Then
                            For Each drrow As DataRow In DenCoverageDRs
                                OverLapping = OverlappingPeriods(CDate(DR("FROM_DATE")), CDate(DR("THRU_DATE")), CDate(drrow("FROM_DATE")), CDate(drrow("THRU_DATE")))
                                If OverLapping = True Then
                                    MessageBox.Show("Overlapping of coverage is not valid." & Environment.NewLine & " Please correct the dates before Continuing", "Overlapping", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    Return True
                                End If
                            Next
                        End If
                    End If
                End If
            End If

            Dim OuterQuery =
                    From ExistingCoverage In CType(_DenCoverageBS.DataSource, DataTable).AsEnumerable()
                    Where ExistingCoverage.RowState <> DataRowState.Deleted
                    Order By ExistingCoverage.Field(Of Date)("THRU_DATE") Descending

            For Each CurrentDR As DataRow In OuterQuery

                Dim InnerQuery =
                        From ExistingCoverage In CType(_DenCoverageBS.DataSource, DataTable).AsEnumerable()
                        Where ExistingCoverage.RowState <> DataRowState.Deleted _
                        AndAlso CDate(CurrentDR("FROM_DATE")) >= ExistingCoverage.Field(Of Date)("FROM_DATE") _
                        AndAlso CDate(CurrentDR("THRU_DATE")) <= ExistingCoverage.Field(Of Date)("THRU_DATE")

                If InnerQuery.Count > 1 Then
                    MessageBox.Show("Coverage rows overlap one or more times. Contact I.T for data correction." & Environment.NewLine & " Please correct the data before Continuing", "From Date", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return True
                End If

            Next

            '' Letter generation
            If cmbDenLetter.Text.ToString.Trim.Length = 0 Then
                ErrorProvider1.SetErrorWithTracking(Me.cmbDenLetter, " Please choose Letter option")
            End If

            If GetErrorCount(ErrorProvider1) > 0 Then
                Return True
            End If

            Return False

        Catch ex As Exception

                Throw

        End Try
    End Function

    Private Sub txtDenFromDate_Validated(sender As Object, e As EventArgs) Handles txtDenFromDate.Validated

        Dim TBox As TextBox = CType(sender, TextBox)
        Dim BS As BindingSource

        Try
            If _Disposed OrElse _DenCoverageBS Is Nothing OrElse _DenCoverageBS.Position < 0 OrElse TBox.ReadOnly Then Return

            BS = _DenCoverageBS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Dim DR As DataRow = DirectCast(BS.Current, DataRowView).Row 'need to check if binding is working

            If Not DR.HasVersion(DataRowVersion.Original) OrElse UFCWGeneral.ValidateDate(DR("FROM_DATE", DataRowVersion.Original)) <> UFCWGeneral.ValidateDate(txtDenFromDate.Text) Then

                If AdjustCurrentAndPriorCoverage(BS, _DenCoverageDS.Tables("ELIG_COVERAGE")) Then
                    DenCoverageDataGrid.RefreshData()
                End If

                _DenCoverageBS.EndEdit() 'trigger CurrentItemChanged 
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally

        End Try

    End Sub

    Private Sub txtDenFromDate_Validating(sender As Object, e As CancelEventArgs) Handles txtDenFromDate.Validating

        Dim TBox As TextBox = CType(sender, TextBox)
        Dim HoldDate As Date?
        Dim DR As DataRow

        Dim BS As BindingSource

        Try

            If _Disposed OrElse _DenCoverageBS Is Nothing OrElse _DenCoverageBS.Position < 0 OrElse TBox.ReadOnly Then Return

            BS = _DenCoverageBS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ClearError(ErrorProvider1, TBox)

            HoldDate = UFCWGeneral.ValidateDate(TBox.Text)
            If HoldDate Is Nothing Then
                ErrorProvider1.SetErrorWithTracking(TBox, " Date format must be mm/01/yyyy or mm-01-yyyy.")
            Else

                'if From Date has been altered all other rows need to be restored
                DR = CType(BS.Current, DataRowView).Row
                If DR.HasVersion(DataRowVersion.Original) Then

                    If CDate(HoldDate) <> CDate(DR("FROM_DATE")) AndAlso CDate(DR("FROM_DATE", DataRowVersion.Original)) <> CDate(DR("FROM_DATE")) Then

                        Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Mid: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

                        Dim RestoreQuery =
                                        From ExistingCoverage In _DenCoverageDS.Tables("ELIG_COVERAGE")
                                        Where ExistingCoverage.RowState = DataRowState.Deleted

                        If RestoreQuery.Any Then
                            _DenCoverageBS.RaiseListChangedEvents = False 'suppress BS activity
                            _DenCoverageDS.Tables("ELIG_COVERAGE").RejectChanges()
                            _DenCoverageBS.RaiseListChangedEvents = True
                            _DenCoverageBS.ResetBindings(True)
                            cmbDenCoverageValues.SelectedIndex = -1 'force reselection of plan
                        End If

                    End If
                Else
                    'added coverage
                    If IsDBNull(DR("FROM_DATE")) OrElse CDate(HoldDate) <> CDate(DR("FROM_DATE")) Then 'AndAlso CDate(DR("FROM_DATE", DataRowVersion.Original)) <> CDate(DR("FROM_DATE")) Then
                        cmbDenCoverageValues.SelectedIndex = -1 'force reselection of plan
                    End If
                End If

                'Changes involving From Date can only impact the prior row.

                Dim CurrentDR As DataRow 'row being expired, changed, etc
                Dim PriorDR As DataRow 'row possiblly modified to reflect new THRU_DATE

                Dim CurrentQuery =
                                    From ExistingCoverage In CType(_DenCoverageBS.DataSource, DataTable).AsEnumerable()
                                    Where ExistingCoverage.RowState <> DataRowState.Deleted AndAlso ExistingCoverage.RowState <> DataRowState.Added
                                    Order By ExistingCoverage.Field(Of Date)("THRU_DATE") Descending

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
                    If CDate(HoldDate) < CDate(PriorDR("FROM_DATE")) Then
                        MessageBox.Show("Retroactive changes may only impact the prior Coverage row. Contact I.T for support", "RetroActive From Date Invalid.", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        ErrorProvider1.SetErrorWithTracking(TBox, " From Date impacts too many Coverage rows.")
                    End If
                End If

                If CDate(HoldDate).Day <> 1 Then
                    HoldDate = CDate(CDate(HoldDate).Month.ToString & "/01/" & CDate(HoldDate).Year.ToString)
                    ErrorProvider1.SetErrorWithTracking(TBox, " From Date modified to use 1st day of month.")
                End If

                If Not _REGMSupervisor Then

                    If DateDiff(DateInterval.Month, Now, CDate(HoldDate)) > 3 AndAlso Not CDate(HoldDate) = CDate("1/1/" & Now.AddYears(+1).Year.ToString) Then
                        MessageBox.Show("Future From Date(s) can either be 1/1/" & Now.AddYears(+1).Year.ToString & " or up to 3 month(s) in the future.", "Future From Date invalid.", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        ErrorProvider1.SetErrorWithTracking(TBox, " Future From date exceeds allowable range.")
                    End If

                    If DateDiff(DateInterval.Month, Now, CDate(HoldDate)) < -5 Then
                        MessageBox.Show("Retroactive From Dates can only be within the prior 6 months.", "RetroActive date invalid.", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        ErrorProvider1.SetErrorWithTracking(TBox, " From date exceeds allowable 6 month range.")
                    End If

                Else
                    If DateDiff(DateInterval.Month, Now, CDate(HoldDate)) < -17 Then
                        MessageBox.Show("Warning: You are specifying a date over 18 Months in the past.", "Unusual From Date.", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If

                If TBox.Text <> CDate(HoldDate).ToString("MM-dd-yyyy") Then
                    TBox.Text = CDate(HoldDate).ToString("MM-dd-yyyy")
                End If

            End If

            If ErrorProvider1.GetError(TBox).Trim.Length > 0 Then 'are there any errors 
                TBox.Select(0, TBox.Text.Length)
                e.Cancel = True
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
        End Try

    End Sub

    Private Function SaveDenChanges() As Boolean

        Dim Transaction As DbTransaction
        Dim ActivityTimestamp As DateTime = DateTime.Now
        Dim HistSum As String = ""
        Dim HistDetail As String = ""
        Dim CoverageID As Integer

        Try

            Transaction = RegMasterDAL.BeginTransaction

            For Each CoverageDR As DataRow In _DenChangedDRs.Tables(0).Rows

                Select Case CoverageDR.RowState
                    Case DataRowState.Modified  'Update

                        HistDetail = DataGridCustom.IdentifyChanges(CoverageDR, DenCoverageDataGrid)

                        HistSum = "DENTAL COVERAGE OF FAMILYID:  " & CStr(CoverageDR("FAMILY_ID")) & " WAS MODIFIED"
                        HistDetail = UFCWGeneral.DomainUser.ToUpper & " MODIFIED THE DENTAL COVERAGE RECORD " & vbCrLf &
                                                    "THE MODIFICATIONS WERE: " & HistDetail & vbCrLf & If(cmbDenLetter.Text = "Y", " Letter was requested.", " No Letter Requested.")

                        RegMasterDAL.UpdateEligCoverage(ActivityTimestamp, CDate(CoverageDR("ONLINE_DATE", DataRowVersion.Original)), CInt(CoverageDR("FAMILY_ID")), CInt(CoverageDR("RELATION_ID")), CDate(CoverageDR("FROM_DATE")), CDate(CoverageDR("THRU_DATE")), CInt(CoverageDR("COVERAGE_CODE")), CInt(CoverageDR("COVERAGE_ID")), If(cmbDenLetter.Text = "Y", "Y", "N"), Transaction)
                        RegMasterDAL.CreateRegHistory(CInt(CoverageDR("FAMILY_ID")), CInt(CoverageDR("RELATION_ID")), Nothing, Nothing, "ELIGCOVDENUPD", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, Transaction)

                    Case DataRowState.Added   ' 

                        HistDetail = DataGridCustom.IdentifyChanges(CoverageDR, DenCoverageDataGrid)

                        HistSum = "DENTAL COVERAGE OF FAMILYID: " & Me.FamilyID & " WAS ADDED"
                        HistDetail = UFCWGeneral.DomainUser.ToUpper & " ADDED THE DENTAL COVERAGE RECORD " & vbCrLf &
                                                    "THE ADDITIONS WERE: " & HistDetail & vbCrLf & If(cmbDenLetter.Text = "Y", " Letter was requested.", " No Letter Requested.")

                        CoverageID = RegMasterDAL.AddEligCoverage(ActivityTimestamp, CInt(CoverageDR("FAMILY_ID")), CInt(CoverageDR("RELATION_ID")), CStr(CoverageDR("COVERAGE_TYPE")), CDate(CoverageDR("FROM_DATE")), CDate(CoverageDR("THRU_DATE")), CInt(CoverageDR("COVERAGE_CODE")), cmbDenLetter.Text, Transaction)
                        RegMasterDAL.CreateRegHistory(CInt(CoverageDR("FAMILY_ID")), CInt(CoverageDR("RELATION_ID")), Nothing, Nothing, "ELIGCOVDENADD", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, Transaction)

                    Case DataRowState.Deleted   ' 

                        HistDetail = DataGridCustom.IdentifyChanges(CoverageDR, DenCoverageDataGrid)

                        HistSum = "DENTAL COVERAGE OF FAMILYID: " & Me.FamilyID & " WAS DELETED"
                        HistDetail = UFCWGeneral.DomainUser.ToUpper & " DELETED COVERAGE " & Microsoft.VisualBasic.vbCrLf & "THE DELETION WAS: " & Microsoft.VisualBasic.vbCrLf & HistDetail

                        RegMasterDAL.DeleteEligCoverage(CDate(CoverageDR("ONLINE_DATE", DataRowVersion.Original)), CInt(CoverageDR("FAMILY_ID", DataRowVersion.Original)), CInt(CoverageDR("RELATION_ID", DataRowVersion.Original)), CDate(CoverageDR("FROM_DATE", DataRowVersion.Original)), CDate(CoverageDR("THRU_DATE", DataRowVersion.Original)), CInt(CoverageDR("COVERAGE_CODE", DataRowVersion.Original)), CoverageDR("COVERAGE_TYPE", DataRowVersion.Original).ToString, Transaction)
                        RegMasterDAL.CreateRegHistory(CInt(CoverageDR("FAMILY_ID", DataRowVersion.Original)), CInt(CoverageDR("RELATION_ID", DataRowVersion.Original)), Nothing, Nothing, "ELIGCOVDENDEL", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, Transaction)

                End Select
            Next

            RegMasterDAL.CommitTransaction(Transaction)

            Dim ModifiedQuery =
                        From ExistingCoverage In CType(_DenCoverageBS.DataSource, DataTable).AsEnumerable
                        Where ExistingCoverage.RowState = DataRowState.Added OrElse ExistingCoverage.RowState = DataRowState.Modified
                        Order By ExistingCoverage.Field(Of Date)("THRU_DATE") Descending

            For Each ModifiedDR As DataRow In ModifiedQuery

                ModifiedDR.BeginEdit()

                If ModifiedDR.RowState = DataRowState.Added Then
                    ModifiedDR("COVERAGE_ID") = CoverageID
                End If

                ModifiedDR("EC_ONLINE_DATE") = ActivityTimestamp
                ModifiedDR("EC_ONLINE_USERID") = UFCWGeneral.DomainUser.ToUpper

                ModifiedDR("ONLINE_DATE") = ActivityTimestamp
                ModifiedDR("ONLINE_USERID") = UFCWGeneral.DomainUser.ToUpper

                ModifiedDR.EndEdit()
            Next

            _DenCoverageDS.AcceptChanges()

            Return True

        Catch db2ex As DB2Exception

            Try
                RegMasterDAL.RollbackTransaction(Transaction)
            Catch ex2 As Exception
            End Try

            Throw

        Catch ex As Exception


                Throw

        Finally

            If Transaction IsNot Nothing Then Transaction.Dispose()
            Transaction = Nothing

        End Try

    End Function

    Private Sub DeleteDenCoverageLine()

        Dim OriginalCurrentDR As DataRow 'row being expired, changed, etc
        Dim OriginalPriorDR As DataRow 'row possiblly modified to reflect new THRU_DATE

        Try
#If DEBUG Then
            RemoveHandler _DenCoverageBS.PositionChanged, AddressOf CoverageBS_PositionChanged
#End If

            Dim OriginalCurrentQuery =
                        From ExistingCoverage In CType(_DenCoverageBS.DataSource, DataTable).AsEnumerable
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

            _UIState = UIStates.Deleting

            OriginalCurrentDR.Delete()
            OriginalCurrentDR.EndEdit()

            _DenCoverageBS.EndEdit()

            If _DenCoverageBS.Position > -1 Then
                DenCoverageDataGrid.Select(_DenCoverageBS.Position)
            End If

        Catch ex As Exception


                Throw
        Finally

#If DEBUG Then
            AddHandler _DenCoverageBS.PositionChanged, AddressOf CoverageBS_PositionChanged
#End If

        End Try
    End Sub

    Private Sub DenCoverageBS_CurrentItemChanged(sender As Object, e As EventArgs) Handles _DenCoverageBS.CurrentItemChanged
        'Called after CurrentChanged and one of the properties of Current is changed
        Dim BS As BindingSource

        Try

            If _Disposed OrElse _DenCoverageBS Is Nothing OrElse _DenCoverageBS.Position < 0 Then Return

            BS = DirectCast(sender, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Dim DR As DataRow = DirectCast(BS.Current, DataRowView).Row

            If Not (_UIState And UIStates.[ReadOnly]) = _UIState Then

                If Not (_UIState And UIStates.[ReadOnly]) = _UIState Then
                    If DR IsNot Nothing AndAlso CDate(DR("THRU_DATE")).Year = 9999 AndAlso (DR.HasVersion(DataRowVersion.Proposed) OrElse (DR.RowState = DataRowState.Modified OrElse DR.RowState = DataRowState.Added)) Then 'in addition to current/future active addresses, items awaiting approval will also have 9999 thru date 

                    End If
                End If

            End If

            SetDenUIElements()

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally

        End Try
    End Sub

    Private Sub cmbDenCoverageValues_Validating(sender As Object, e As CancelEventArgs) Handles cmbDenCoverageValues.Validating

        Dim CBox As ExComboBox = CType(sender, ExComboBox)
        Dim BS As BindingSource
        Dim PBS As BindingSource
        Dim DR As DataRow

        Try

            If _Disposed OrElse _DenCoverageBS Is Nothing OrElse _DenCoverageBS.Position < 0 OrElse cmbDenCoverageValues.ReadOnly Then Return

            BS = _DenCoverageBS
            PBS = _DenCoverageValuesComboBS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
            DR = CType(_DenCoverageBS.Current, DataRowView).Row

            ClearError(ErrorProvider1, CBox)

            If Not (_UIState And UIStates.[ReadOnly]) = _UIState Then

                If CBox.SelectedIndex < 0 Then
                    ErrorProvider1.SetErrorWithTracking(CBox, " Coverage selection required.")
                Else

                    Dim DenCoverageQuery =
                                From AssignableCoverage In CType(_DenCoverageValuesComboBS.DataSource, DataTable).AsEnumerable()
                                Where (Not IsDBNull(DR("FROM_DATE")) AndAlso (CDate(DR("FROM_DATE")) >= AssignableCoverage.Field(Of Date)("FROM_DATE") _
                                                                        AndAlso CDate(DR("FROM_DATE")) <= AssignableCoverage.Field(Of Date)("THRU_DATE"))) _
                                AndAlso cmbDenCoverageValues.SelectedValue.ToString = AssignableCoverage.Field(Of Int16?)("COVERAGE_VALUE").ToString

                    If Not DenCoverageQuery.Any Then
                        ErrorProvider1.SetErrorWithTracking(CBox, " Coverage invalid for FROM date specified.")
                    End If
                End If
            End If

            If ErrorProvider1.GetError(CBox).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally

        End Try

    End Sub

    Private Sub DenCoverageBS_CurrentChanged(sender As Object, e As EventArgs) Handles _DenCoverageBS.CurrentChanged

        Dim BS As BindingSource

        Dim CoverageFilterDate As Date = Date.Now
        Dim CoverageCode As String = Nothing
        Dim CoverageDate As Date = Date.Now

        Try

            If _DenCoverageBS Is Nothing OrElse _DenCoverageBS.Position < 0 OrElse _DenCoverageBS.Current Is Nothing OrElse _DenCoverageBS.Count < 1 Then Return

            BS = DirectCast(sender, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Dim DR As DataRow = DirectCast(_DenCoverageBS.Current, DataRowView).Row

            If DR IsNot Nothing Then
                If DR.HasVersion(DataRowVersion.Proposed) AndAlso Not IsDBNull(DR("FROM_DATE", DataRowVersion.Proposed)) Then
                    CoverageFilterDate = CDate(DR("FROM_DATE", DataRowVersion.Proposed))
                    CoverageCode = DR("COVERAGE_CODE", DataRowVersion.Proposed).ToString
                ElseIf Not IsDBNull(DR("FROM_DATE", DataRowVersion.Current)) Then
                    CoverageFilterDate = CDate(DR("FROM_DATE", DataRowVersion.Current))
                    CoverageCode = DR("COVERAGE_CODE", DataRowVersion.Current).ToString
                End If

                If Not IsDBNull(DR("FROM_DATE")) AndAlso IsDate(DR("FROM_DATE")) Then
                    CoverageDate = CType(DR("FROM_DATE"), Date)
                End If

                If FilterAndSelectDenCoverageValues(CoverageFilterDate, CoverageCode) Then

                End If 'This is not limited to modifications as descriptions of code values have changed with time.


            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Function FilterAndSelectDenCoverageValues(proposedDate As Date, coverageCode As String) As Boolean
        'limit Coverage choices to specific period, triggered when either new row or from/thru dates change.
        Dim CoverageFilter As New StringBuilder
        Dim BS As BindingSource

        Try

            BS = DirectCast(_DenCoverageBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            CoverageFilter.Append("'" & proposedDate.ToShortDateString & "' >= FROM_DATE AND '" & proposedDate.ToShortDateString & "' <= THRU_DATE ")

            If _DenCoverageValuesComboBS.Filter <> CoverageFilter.ToString Then

                _DenCoverageValuesComboBS.Filter = CoverageFilter.ToString

                If cmbDenCoverageValues.DataBindings.Count > 0 Then cmbDenCoverageValues.DataBindings(0).ReadValue() 'filter changes list content so a refresh of the bound data is needed to rectify index vs data mistmatch

                Return True

            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Return False

        Catch ex As Exception

                Throw

        Finally

        End Try
    End Function

    Private Sub cmbDenLetter_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbDenLetter.SelectionChangeCommitted
        Dim DR As DataRow
        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Try

            If _DenCoverageBS Is Nothing OrElse _DenCoverageBS.Position < 0 OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _DenCoverageBS.Position.ToString & ") SI(" & CBox.Text & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            CType(CBox.Parent, TransparentContainer).ValidateChildren() 'this will trigger validation of the cmbbox triggering write of value to DS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _DenCoverageBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally

        End Try

    End Sub
#End Region

#Region "Medical"
    Private Sub HMONetworkValuesBS_CurrentItemChanged(sender As Object, e As EventArgs) Handles _HMONetworkValuesBS.CurrentItemChanged
        Dim BS As BindingSource

        Try

            BS = CType(sender, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If Not (_UIState And UIStates.[ReadOnly]) = _UIState Then

                '                _MedCoverageBS.EndEdit()

            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub cmbMed_DropDown(sender As Object, e As EventArgs) Handles cmbMedCoverage.DropDown

        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _MedCoverageBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Try

            ClearError(ErrorProvider1, CBox)

            CBox.Invalidate()
            grpMedEditPanel.Refresh()

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _MedCoverageBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Function UnCommittedMedChangesExist() As Boolean

        Dim Modifications As String = ""

        Dim BS As BindingSource

        Try

            BS = DirectCast(_MedCoverageBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            _MedChangedDRs = _MedCoverageDS.GetChanges()

            If MedCoverageDataGrid IsNot Nothing AndAlso _MedChangedDRs IsNot Nothing AndAlso _MedChangedDRs.Tables("ELIG_COVERAGE").Rows.Count > 0 Then

                For Each DR As DataRow In _MedChangedDRs.Tables("ELIG_COVERAGE").Rows
                    If DR.RowState <> DataRowState.Added Then
                        'attempt to exclude rows accidently changed during navigation operations
                        Modifications = DataGridCustom.IdentifyChanges(DR, MedCoverageDataGrid)

                        If Modifications IsNot Nothing AndAlso Modifications.Length > 0 Then
                            Return True
                        End If

                    ElseIf DR.RowState = DataRowState.Added Then
                        Return True
                    End If
                Next

            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Function

    Private Sub txtMedFromDate_Validating(sender As Object, e As CancelEventArgs) Handles txtMedFromDate.Validating

        Dim TBox As TextBox = CType(sender, TextBox)
        Dim HoldDate As Date?
        Dim DR As DataRow
        Dim BS As BindingSource

        Try

            If _Disposed OrElse _MedCoverageBS Is Nothing OrElse _MedCoverageBS.Position < 0 OrElse _MedCoverageBS.Current Is Nothing OrElse TBox.ReadOnly Then Return

            BS = _MedCoverageBS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ClearError(ErrorProvider1, TBox)

            HoldDate = UFCWGeneral.ValidateDate(TBox.Text)
            If HoldDate Is Nothing Then
                ErrorProvider1.SetErrorWithTracking(TBox, " Date format must be mm/01/yyyy or mm-01-yyyy.")
            Else

                'if From Date has been altered all other rows need to be restored
                DR = CType(BS.Current, DataRowView).Row
                If DR.HasVersion(DataRowVersion.Original) Then

                    If CDate(HoldDate) <> CDate(DR("FROM_DATE")) AndAlso CDate(DR("FROM_DATE", DataRowVersion.Original)) <> CDate(DR("FROM_DATE")) Then

                        Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Res: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

                        Dim RestoreQuery =
                                        From ExistingCoverage In _MedCoverageDS.Tables("ELIG_COVERAGE")
                                        Where ExistingCoverage.RowState = DataRowState.Deleted

                        If RestoreQuery.Any Then
                            _MedCoverageBS.RaiseListChangedEvents = False 'suppress BS activity
                            _MedCoverageDS.Tables("ELIG_COVERAGE").RejectChanges()
                            _MedCoverageBS.RaiseListChangedEvents = True
                            _MedCoverageBS.ResetBindings(True)
                            cmbMedCoverage.SelectedIndex = -1 'force reselection of plan
                        End If

                    End If
                Else
                    'added coverage
                    If IsDBNull(DR("FROM_DATE")) OrElse CDate(HoldDate) <> CDate(DR("FROM_DATE")) Then 'AndAlso CDate(DR("FROM_DATE", DataRowVersion.Original)) <> CDate(DR("FROM_DATE")) Then
                        cmbMedCoverage.SelectedIndex = -1 'force reselection of plan
                    End If
                End If

                'Changes involving From Date can only impact the prior row.

                Dim CurrentDR As DataRow 'row being expired, changed, etc
                Dim PriorDR As DataRow 'row possiblly modified to reflect new THRU_DATE
                Dim AddedDR As DataRow

                Dim CurrentQuery =
                                    From ExistingCoverage In CType(_MedCoverageBS.DataSource, DataTable).AsEnumerable()
                                    Where ExistingCoverage.RowState <> DataRowState.Deleted AndAlso ExistingCoverage.RowState <> DataRowState.Added
                                    Order By ExistingCoverage.Field(Of Date)("THRU_DATE") Descending

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
                    If CDate(HoldDate) < CDate(PriorDR("FROM_DATE")) Then
                        MessageBox.Show("Retroactive changes may only impact the prior Coverage row. Contact I.T for support", "RetroActive From Date Invalid.", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        ErrorProvider1.SetErrorWithTracking(TBox, " From Date impacts too many Coverage rows.")
                    End If
                End If

                If CDate(HoldDate).Day <> 1 Then
                    HoldDate = CDate(CDate(HoldDate).Month.ToString & "-01-" & CDate(HoldDate).Year.ToString)
                    ErrorProvider1.SetErrorWithTracking(TBox, " From Date modified to use 1st day of month.")
                End If

                Dim AddedQuery =
                From Addresses In CType(_MedCoverageBS.DataSource, DataTable).AsEnumerable()
                Where Addresses.RowState = DataRowState.Added

                If AddedQuery.Any Then
                    AddedDR = AddedQuery.First
                    If CurrentDR IsNot Nothing AndAlso CDate(HoldDate) = CDate(CurrentDR("FROM_DATE")) Then
                        ErrorProvider1.SetErrorWithTracking(TBox, " From Date of Added record cannot use the same from date of the record being replaced.")
                    End If
                End If

                If Not _REGMSupervisor Then

                    If DateDiff(DateInterval.Month, Now, CDate(HoldDate)) > 3 AndAlso Not CDate(HoldDate) = CDate("1/1/" & Now.AddYears(+1).Year.ToString) Then
                        MessageBox.Show("Future From Date(s) can either be 1/1/" & Now.AddYears(+1).Year.ToString & " or up to 3 month(s) in the future.", "Future From Date invalid.", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        ErrorProvider1.SetErrorWithTracking(TBox, " Future From date exceeds allowable range.")
                    End If

                    If DateDiff(DateInterval.Month, Now, CDate(HoldDate)) < -5 Then
                        MessageBox.Show("Retroactive From Dates can only be within the prior 6 months.", "RetroActive date invalid.", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        ErrorProvider1.SetErrorWithTracking(TBox, " From date exceeds allowable 6 month range.")
                    End If

                Else
                    If DateDiff(DateInterval.Month, Now, CDate(HoldDate)) < -11 Then
                        MessageBox.Show("Warning: You are specifying a date over 1 Year in the past.", "Unusual From Date.", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If

                If TBox.Text <> CDate(HoldDate).ToString("MM-dd-yyyy") Then
                    TBox.Text = CDate(HoldDate).ToString("MM-dd-yyyy")
                End If

            End If

            If ErrorProvider1.GetError(TBox).Trim.Length > 0 Then 'are there any errors 
                TBox.Select(0, TBox.Text.Length)
                e.Cancel = True
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
        End Try

    End Sub

    Private Sub txtMedFromDate_Validated(sender As Object, e As EventArgs) Handles txtMedFromDate.Validated

        Dim TBox As TextBox = CType(sender, TextBox)
        Dim BS As BindingSource

        Try

            If _Disposed OrElse _MedCoverageBS Is Nothing OrElse _MedCoverageBS.Position < 0 OrElse TBox.ReadOnly Then Return

            BS = _MedCoverageBS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Dim DR As DataRow = DirectCast(BS.Current, DataRowView).Row 'need to check if binding is working

            If Not DR.HasVersion(DataRowVersion.Original) OrElse UFCWGeneral.ValidateDate(DR("FROM_DATE", DataRowVersion.Original)) <> UFCWGeneral.ValidateDate(txtMedFromDate.Text) Then

                If AdjustCurrentAndPriorCoverage(BS, _MedCoverageDS.Tables("ELIG_COVERAGE")) Then
                    Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Mid: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
                End If

                'DR.EndEdit() ' triggers CurrentItemChanged and thus AdjustCurrentAndPriorCoverage
                '_MedCoverageBS.EndEdit()
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally

        End Try

    End Sub

    Private Sub MedAddButton_Click(sender As System.Object, e As System.EventArgs) Handles MedAddButton.Click

        Dim EligcalcDS As DataSet
        Dim EligCalcDT As New DataTable

        Try

            MedAddButton.Enabled = False

            EligcalcDS = RegMasterDAL.RetrieveEligCalcElementsByFamilyID(_FamilyID)  '' checking if there is a row in elg calc elements table
            EligCalcDT = EligcalcDS.Tables("ELGCALC_ELEMENTS")

            If (EligCalcDT) IsNot Nothing AndAlso EligCalcDT.Rows.Count = 0 Then
                MessageBox.Show(" Please establish Entrydate, Local and Memtype" & Environment.NewLine & " before adding coverage.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            ElseIf (EligCalcDT IsNot Nothing) AndAlso (EligCalcDT.Rows.Count > 0) AndAlso (CStr(EligCalcDT.Rows(0)("LAST_MEMTYPE")).Trim.Length = 0) Then
                MessageBox.Show(" Please establish Memtype before adding coverage.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            AddMedCoverageLine()

            txtMedFromDate.Focus()

        Catch ex As Exception


                Throw

        Finally
            If EligcalcDS IsNot Nothing Then EligcalcDS.Dispose()
            EligcalcDS = Nothing

            If EligCalcDT IsNot Nothing Then EligCalcDT.Dispose()
            EligCalcDT = Nothing

            SetMedUIElements()
        End Try

    End Sub

    Private Sub cmbMedLetter_Validating(sender As Object, e As CancelEventArgs) Handles cmbMedLetter.Validating

        Dim CBox As ExComboBox = CType(sender, ExComboBox)
        Dim BS As BindingSource
        Dim PBS As BindingSource
        Dim DR As DataRow

        Try

            If _Disposed OrElse _MedCoverageBS Is Nothing OrElse _MedCoverageBS.Position < 0 OrElse CBox.ReadOnly Then Return

            BS = _MedCoverageBS
            PBS = _MedCoverageValuesComboBS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
            DR = CType(_MedCoverageBS.Current, DataRowView).Row

            ClearError(ErrorProvider1, CBox)

            If Not (_UIState And UIStates.[ReadOnly]) = _UIState Then

                If CBox.SelectedIndex < 0 Then
                    ErrorProvider1.SetErrorWithTracking(CBox, " Letter decision required.")
                End If
            End If

            If ErrorProvider1.GetError(CBox).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally

        End Try

    End Sub

    Private Sub CancelMedChanges()

        Dim GoToPosition As Integer

        Try
            _MedChangedDRs = _MedCoverageDS.GetChanges()

            GoToPosition = _MedCoverageBS.Position

            If _MedChangedDRs IsNot Nothing Then
                Dim DR = (From a In _MedChangedDRs.Tables("ELIG_COVERAGE").Rows.Cast(Of DataRow)() Where a.RowState = DataRowState.Added Select a).FirstOrDefault
                If DR IsNot Nothing Then GoToPosition = 0
            End If

            MedCoverageDataGrid.UnSelect(_MedCoverageBS.Position)

            _MedCoverageBS.CancelEdit()
            _MedCoverageDS.Tables("ELIG_COVERAGE").RejectChanges()
            _MedCoverageBS.ResetBindings(False)

            If _MedCoverageBS IsNot Nothing AndAlso _MedCoverageBS.Position > -1 AndAlso _MedCoverageBS.Count > 0 AndAlso _MedCoverageBS.Count > GoToPosition Then
                If _MedCoverageBS.Position <> GoToPosition Then
                    _MedCoverageBS.Position = GoToPosition
                    MedCoverageDataGrid.Select(_MedCoverageBS.Position)
                End If
            End If

            ClearErrors()

        Catch ex As Exception

                Throw
        Finally

            _UIState = UIStates.Canceled
        End Try
    End Sub

    'Private Sub MedCancelChanges()

    '    Dim HoldState As UIStates = _UIState

    '    Try

    '        SetMedUIElements(UIStates.Canceling)

    '        If UnCommittedMedChangesExist() = False Then

    '            CancelMedChanges()

    '        Else

    '            Dim Result As DialogResult = MessageBox.Show(Me, "Do you want to Cancel Medical changes?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

    '            If Result = DialogResult.Yes Then

    '                CancelMedChanges()

    '            Else
    '                _UIState = HoldState
    '            End If

    '        End If

    '        SetMedUIElements()

    '    Catch ex As Exception
    '        Throw
    '    Finally


    '    End Try
    'End Sub

    Private Sub MedHMONetworkButton_Click(sender As Object, e As EventArgs) Handles MedHMONetworkButton.Click

        If MedCoverageDataGrid.DataSource Is Nothing Then Return

        Dim HMONetworkForm As HMONetworkInfoForm

        Try

            If Me.MedPendingChanges = True Then
                MessageBox.Show(Me, "Changes have been made to Medical Coverage Election Screen." & vbCrLf &
                                             "Please Complete the changes before adding the HMO Network value", "Made changes", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                MedHMONetworkButton.Enabled = False
                Return
            End If

            '' when added new Network it should show the newly added Network

            Dim ActiveCoverage() As DataRow = _MedCoverageDS.Tables("ELIG_COVERAGE").Select("#" & Now.ToShortDateString & "# >= FROM_DATE And #" & Now.ToShortDateString & "# <= THRU_DATE AND COVERAGE_TYPE = 'MEDICAL'")

            HMONetworkForm = New HMONetworkInfoForm(_FamilyID, CInt(ActiveCoverage(0)("COVERAGE_CODE")), _HMONetworkDS, True, _Zipcode, _SubscriberStatus)

            HMONetworkForm.ShowDialog(Me)
            _HMONetworkDS = HMONetworkForm.HMONetworkDataSet

            HMONetworkForm.Close()

        Catch ex As Exception

	Throw
        Finally
            If HMONetworkForm IsNot Nothing Then
                HMONetworkForm.Dispose()
                HMONetworkForm = Nothing
            End If
        End Try
    End Sub

    Private Sub MedSaveButton_Click(sender As Object, e As EventArgs) Handles MedSaveButton.Click

        Dim HoldAutoValidateState As AutoValidate = Me.AutoValidate
        Dim Deleted As Boolean = False
        Dim StartingUISTate As UIStates = _UIState

        Try

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

            MedSaveButton.Enabled = False 'this will reenable if the underlying dataset changes
            MedCancelButton.Enabled = False

            If Not _UIState = UIStates.Deleting Then
                If Not MedUIContainer.ValidateChildren(ValidationConstraints.Enabled) OrElse ValidateMedCoverageChanges() Then
                    MedSaveButton.Enabled = True
                    MedCancelButton.Enabled = True
                    _UIState = StartingUISTate
                    Return
                Else
                    _UIState = UIStates.Saving
                End If

            Else
                Deleted = True
            End If

            _MedCoverageBS.EndEdit()

            _MedChangedDRs = _MedCoverageDS.GetChanges()

            If _MedChangedDRs IsNot Nothing Then

                SetMedUIElements()

                If SaveMedChanges(_MedChangedDRs) Then

                    _UIState = UIStates.Saved

                    If Deleted Then
                        _UIState = UIStates.Deleted
                    End If

                    SetMedUIElements()

                    MessageBox.Show("Medical Coverage Record " & If(Deleted, "Deleted", "Saved") & " Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    '' calculate eligibilty for the current elig period

                    RegMasterDAL.MadeDBChanges = True  '' this is to get elig_acct_hours from database to show pending/ or not
                    RegMasterDAL.MadeEligibilityChanges = True  '' FOR summary screen to display correct coverage

                    _UpdatedRecord = True
                Else

                    MedSaveButton.Enabled = True
                    MedCancelButton.Enabled = True
                    _UIState = StartingUISTate

                    Return

                End If

            Else

                Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable

                CancelMedChanges()

                MessageBox.Show("No changes were made. Save request resulted in no action being taken.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If

        Catch ex As Exception

            CancelMedChanges()


                Throw

        Finally

            SetMedUIElements()

            Me.AutoValidate = HoldAutoValidateState

        End Try

    End Sub

    Private Sub LoadMedDataBindings()

        Dim Bind As Binding

        Try
            'Debug.Print("MedCoverageDataBindings (In): ")

            txtMedFromDate.DataBindings.Clear()
            Bind = New Binding("Text", _MedCoverageBS, "FROM_DATE", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            Bind.FormatString = "MM-dd-yyyy"
            AddHandler Bind.Parse, AddressOf UFCWGeneral.DateOnlyBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf txtMedFromDateBindingComplete
            txtMedFromDate.DataBindings.Add(Bind)

            txtMedThruDate.DataBindings.Clear()
            Bind = New Binding("Text", _MedCoverageBS, "THRU_DATE", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            Bind.FormatString = "MM-dd-yyyy"
            AddHandler Bind.Parse, AddressOf UFCWGeneral.DateOnlyBinding_Parse
            txtMedThruDate.DataBindings.Add(Bind)

            RemoveHandler cmbHMONetwork.SelectedIndexChanged, AddressOf cmbHMONetwork_SelectedIndexChanged
            RemoveHandler cmbHMONetwork.SelectedValueChanged, AddressOf cmbHMONetwork_SelectedValueChanged
            cmbHMONetwork.DataSource = _HMONetworkValuesBS
            cmbHMONetwork.ValueMember = "HMO_NETWORK"
            cmbHMONetwork.DisplayMember = "DESCRIPTION"

            cmbHMONetwork.DataBindings.Clear()
            Bind = New Binding("SelectedValue", _MedCoverageBS, "HMO_NETWORK", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            Bind.NullValue = Nothing
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            cmbHMONetwork.DataBindings.Add(Bind)

            AddHandler cmbHMONetwork.SelectedValueChanged, AddressOf cmbHMONetwork_SelectedValueChanged
            AddHandler cmbHMONetwork.SelectedIndexChanged, AddressOf cmbHMONetwork_SelectedIndexChanged

            RemoveHandler cmbMedCoverage.SelectedIndexChanged, AddressOf cmbMedCoverage_SelectedIndexChanged
            RemoveHandler cmbMedCoverage.SelectedValueChanged, AddressOf cmbMedCoverage_SelectedValueChanged
            cmbMedCoverage.DataSource = _MedCoverageValuesComboBS
            cmbMedCoverage.ValueMember = "COVERAGE_VALUE"
            cmbMedCoverage.DisplayMember = "DESC"

            cmbMedCoverage.DataBindings.Clear()
            Bind = New Binding("SelectedValue", _MedCoverageBS, "COVERAGE_CODE", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            cmbMedCoverage.DataBindings.Add(Bind)

            If _MedCoverageBS.Count < 1 Then cmbMedCoverage.SelectedIndex = -1

            AddHandler cmbMedCoverage.SelectedValueChanged, AddressOf cmbMedCoverage_SelectedValueChanged
            AddHandler cmbMedCoverage.SelectedIndexChanged, AddressOf cmbMedCoverage_SelectedIndexChanged

        Catch ex As Exception


                Throw

        Finally
            'Debug.Print("MedCoverageDataBindings (Out): ")
        End Try
    End Sub

    Private Sub txtMedFromDateBindingComplete(sender As Object, e As BindingCompleteEventArgs)

        Dim ControlBinding As Binding

        Try

            ControlBinding = CType(sender, Binding)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _MedCoverageBS.Position.ToString & ") " & Me.Name & " : (" & ControlBinding.Control.Name & ") " & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If Not e.BindingCompleteState = BindingCompleteState.Success Then
                MessageBox.Show("Control " & e.Binding.Control.Name & " " & e.ErrorText, "Problem converting data to database format", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _MedCoverageBS.Position.ToString & ") " & Me.Name & " : (" & ControlBinding.Control.Name & ") " & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw

            End If
        Finally

        End Try

    End Sub

    Private Sub ComboBindingCompleteEventHandler(ByVal sender As Object, ByVal e As System.Windows.Forms.BindingCompleteEventArgs)

        Dim CBox As ExComboBox
        Dim ComboBinding As Binding
        Dim CM As CurrencyManager
        Dim DR As DataRow

        Try

            ComboBinding = CType(sender, Binding)
            CBox = CType(ComboBinding.Control, ExComboBox)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _MedCoverageBS.Position.ToString & ") " & Me.Name & " : (" & ComboBinding.Control.Name & ") " & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            CM = CType(ComboBinding.BindingManagerBase, CurrencyManager)
            DR = CType(CM.Current, DataRowView).Row

            If Not e.BindingCompleteState = BindingCompleteState.Success Then
                MessageBox.Show("Control " & e.Binding.Control.Name & " " & e.ErrorText, "Problem converting data to database format", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _MedCoverageBS.Position.ToString & ") " & Me.Name & " : (" & ComboBinding.Control.Name & ") " & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Finally

        End Try

    End Sub

    Private Sub AddMedCoverageLine()
        Dim DR As DataRow

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _MedCoverageBS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = _MedCoverageDS.Tables("ELIG_COVERAGE").NewRow
            DR.BeginEdit()

            DR("FAMILY_ID") = _FamilyID
            DR("RELATION_ID") = 0

            DR("COVERAGE_TYPE") = "MEDICAL"
            DR("COVERAGE_CODE") = System.DBNull.Value
            DR("FROM_DATE") = CDate(Date.Now.AddMonths(+1).Month & "/01/" & Date.Now.AddMonths(+1).Year)
            DR("THRU_DATE") = "12/31/9999"

            DR("CREATE_USERID") = UFCWGeneral.DomainUser.ToUpper
            DR("CREATE_DATE") = Date.Now

            _UIState = UIStates.Adding

            DR.EndEdit()

            _MedCoverageDS.Tables("ELIG_COVERAGE").Rows.Add(DR)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _MedCoverageBS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception


                Throw
        Finally

        End Try
    End Sub

    Private Function AssignedHMONetwork(fromDate As Date) As String

        Dim HMONetworksQuery =
                From AssignedHMONetworks In _FamilyHMONetworkHistoryDS.Tables("HMO_NETWORK").AsEnumerable()
                Where fromDate >= AssignedHMONetworks.Field(Of Date)("FROM_DATE") _
                And fromDate <= AssignedHMONetworks.Field(Of Date)("THRU_DATE")
                Order By AssignedHMONetworks.Field(Of Date)("FROM_DATE")

        If HMONetworksQuery.Any Then
            Dim DR As DataRow = HMONetworksQuery.First
            If Not IsDBNull(DR("HMO_NETWORK")) Then
                Return DR("HMO_NETWORK").ToString
            End If
        End If

        Return Nothing

    End Function

    Private Function SaveMedChanges(changedMedCoverageDRs As DataSet) As Boolean

        Dim cnt As Integer = 0

        Dim ChgCnt As Integer = 0
        Dim HistSum As String = ""
        Dim HistDetail As String = ""
        Dim Modifications As String = ""

        Dim Transaction As DbTransaction
        Dim ActivityTimestamp As DateTime = DateTime.Now

        Try

            If changedMedCoverageDRs.Tables("ELIG_COVERAGE").Rows.Count > 2 Then
                Throw New ApplicationException("Modified Rows exceeds allowed limit.")
            End If

            Transaction = RegMasterDAL.BeginTransaction

            PerformNetworkChanges(changedMedCoverageDRs, ActivityTimestamp, Transaction)
            PerformCoverageChanges(changedMedCoverageDRs, ActivityTimestamp, Transaction)

            RegMasterDAL.CommitTransaction(Transaction)

            Dim ModifiedQuery =
                        From ExistingCoverage In CType(_MedCoverageBS.DataSource, DataTable).AsEnumerable
                        Where ExistingCoverage.RowState = DataRowState.Added OrElse ExistingCoverage.RowState = DataRowState.Modified
                        Order By ExistingCoverage.Field(Of Date)("THRU_DATE") Descending

            For Each ModifiedDR As DataRow In ModifiedQuery

                ModifiedDR("EC_ONLINE_DATE") = ActivityTimestamp
                ModifiedDR("EC_ONLINE_USERID") = UFCWGeneral.DomainUser.ToUpper

                ModifiedDR("ONLINE_DATE") = ActivityTimestamp
                ModifiedDR("ONLINE_USERID") = UFCWGeneral.DomainUser.ToUpper

                ModifiedDR.EndEdit()
            Next

            _MedCoverageDS.AcceptChanges()

            Return True

        Catch db2ex As DB2Exception

            Dim MessageText As String
            Dim TitleText As String

            Select Case db2ex.Number
                Case -438, -1822
                    MessageText = "The item(s) you are attempting to update has been modified." &
                                       vbCrLf & "Attempted changes was not allowed. " &
                                       vbCrLf & "Data has been reset to original values." & vbCrLf &
                                       vbCrLf & "Refresh data before attempting additional changes."

                    TitleText = "Changes Rejected"

                Case -803
                    MessageText = "The item(s) you are attempting to update has inconsistent Network data." &
                                       vbCrLf & "Contact I.T to have record repaired."

                    TitleText = "Changes Rejected"

                Case Else

                    MessageText = "Critical database error. " & db2ex.Message
                    TitleText = "Critical database error. "

            End Select

            Try
                RegMasterDAL.RollbackTransaction(Transaction)
            Catch ex2 As Exception
            End Try

            MessageBox.Show(MessageText, TitleText, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            Return False

        Catch ex As Exception


                Throw
        Finally

            If Transaction IsNot Nothing Then Transaction.Dispose()
            Transaction = Nothing

        End Try
    End Function

    Private Sub DeleteMedCoverageLine()

        Dim OriginalCurrentDR As DataRow 'row being expired, changed, etc
        Dim OriginalPriorDR As DataRow 'row possiblly modified to reflect new THRU_DATE

        Try

#If DEBUG Then
            RemoveHandler _MedCoverageBS.PositionChanged, AddressOf CoverageBS_PositionChanged
#End If

            Dim OriginalCurrentQuery =
                        From ExistingCoverage In CType(_MedCoverageBS.DataSource, DataTable).AsEnumerable
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

            _UIState = UIStates.Deleting

            OriginalCurrentDR.Delete()
            OriginalCurrentDR.EndEdit()

            If OriginalPriorDR IsNot Nothing Then
                OriginalPriorDR.BeginEdit()
                OriginalPriorDR("THRU_DATE") = CDate("9999-12-31")
                OriginalPriorDR.EndEdit()
            End If

            _MedCoverageBS.EndEdit()

            If _MedCoverageBS.Position > -1 Then

                MedCoverageDataGrid.Select(_MedCoverageBS.Position)
            End If

        Catch ex As Exception


                Throw
        Finally
#If DEBUG Then
            AddHandler _MedCoverageBS.PositionChanged, AddressOf CoverageBS_PositionChanged
#End If
        End Try
    End Sub

    Private Sub PerformCoverageChanges(changedMedCoverageDRs As DataSet, ByVal activityTimestamp As DateTime, ByRef transaction As DbTransaction)

        Dim OriginalCurrentDR As DataRow 'row being expired, changed, etc
        Dim OriginalPriorDR As DataRow 'row possiblly modified to reflect new THRU_DATE

        Dim ProposedCurrentDR As DataRow 'Final Current Row, could represent future date

        Dim SharedCoverage As Boolean = False

        Dim CurrentCoverageChanged As Boolean = False
        Dim CurrentNetworkChanged As Boolean = False
        Dim PriorNetworkChanged As Boolean = False

        Dim CurrentCoverageAdded As Boolean = False

        Dim CurrentCoverageErased As Boolean = False
        Dim PriorCoverageErased As Boolean = False

        Dim CurrentCoveragePeriodChanged As Boolean = False
        Dim PriorCoveragePeriodChanged As Boolean = False

        Try
            'All changes in Coverage or Network result in a change to the Coverage table.
            'Resolve if change is new Coverage choice, existing extended or trimmed coverage or network change

            'Add Possibilities
            'New Coverage (No Network)
            'New Coverage + Network
            'Existing Coverage + Existing Network (used to trigger Coverage letter)

            'Modify Possibilities
            'Coverage Code changed
            'Coverage Code started earlier affecting network assignment
            'Coverage Code started earlier no associated network assignment

            'Coverage Code started later
            'Network updated
            '

            Dim OriginalCurrentQuery =
                From ExistingCoverage In CType(_MedCoverageBS.DataSource, DataTable).AsEnumerable()
                Where ExistingCoverage.RowState <> DataRowState.Added
                Order By ExistingCoverage.Field(Of Date)("THRU_DATE", DataRowVersion.Original) Descending

            For Each CoverageDR As DataRow In OriginalCurrentQuery

                If OriginalCurrentDR IsNot Nothing AndAlso OriginalPriorDR Is Nothing Then
                    OriginalPriorDR = CoverageDR
                    Exit For
                End If

                If OriginalCurrentDR Is Nothing Then
                    OriginalCurrentDR = CoverageDR
                End If
            Next

            Dim AddedCurrentQuery =
                From AddedCoverage In CType(_MedCoverageBS.DataSource, DataTable).AsEnumerable()
                Where AddedCoverage.RowState = DataRowState.Added

            If AddedCurrentQuery.Any Then
                ProposedCurrentDR = AddedCurrentQuery.First
            End If

            If ProposedCurrentDR IsNot Nothing Then 'Coverage is being added

                If OriginalCurrentDR IsNot Nothing Then
                    If OriginalCurrentDR.RowState = DataRowState.Modified Then 'Added row impacted thru date

                        CurrentCoveragePeriodChanged = True

                    ElseIf OriginalCurrentDR.RowState = DataRowState.Deleted Then 'Added row used From date of current row, thus replacing the current row.

                        PriorCoverageErased = True
                    End If

                End If

            End If


            If OriginalCurrentDR IsNot Nothing Then 'possible if first coverage row to be added 

                If OriginalCurrentDR.RowState = DataRowState.Modified Then

                    If OriginalCurrentDR("COVERAGE_CODE").ToString <> OriginalCurrentDR("COVERAGE_CODE", DataRowVersion.Original).ToString Then
                        CurrentCoverageChanged = True
                    End If

                    If OriginalCurrentDR("FROM_DATE").ToString <> OriginalCurrentDR("FROM_DATE", DataRowVersion.Original).ToString Then
                        CurrentCoveragePeriodChanged = True 'indicates current coverage row modified
                    End If

                    If OriginalCurrentDR("THRU_DATE").ToString <> OriginalCurrentDR("THRU_DATE", DataRowVersion.Original).ToString Then '#0.5
                        CurrentCoveragePeriodChanged = True 'indicates current coverage row was replaced by an addition
                    End If

                ElseIf OriginalCurrentDR.RowState = DataRowState.Deleted Then
                    If Not IsDBNull(OriginalCurrentDR("COVERAGE_CODE", DataRowVersion.Original)) Then
                        CurrentCoverageErased = True
                    End If
                End If
            End If

            If OriginalPriorDR IsNot Nothing AndAlso OriginalPriorDR.RowState <> DataRowState.Unchanged Then

                If OriginalPriorDR.RowState = DataRowState.Modified Then
                    'Prior rows can only either be deleted or have their thru date manipulated
                    PriorCoveragePeriodChanged = True

                ElseIf OriginalPriorDR.RowState = DataRowState.Deleted Then
                    PriorCoverageErased = True
                End If

            End If

            If PriorCoverageErased AndAlso (CurrentCoverageChanged OrElse CurrentCoveragePeriodChanged) Then
                CurrentCoverageAdded = True
            End If

            If CurrentCoveragePeriodChanged AndAlso PriorCoveragePeriodChanged Then 'A coverage row can cover multiple networks. If changes happen with coverage period no change is necassary.

                If CDate(OriginalCurrentDR("EC_FROM_DATE", DataRowVersion.Original)) = CDate(OriginalPriorDR("EC_FROM_DATE", DataRowVersion.Original)) AndAlso
                    CDate(OriginalCurrentDR("EC_THRU_DATE", DataRowVersion.Original)) = CDate(OriginalPriorDR("EC_THRU_DATE", DataRowVersion.Original)) AndAlso
                    CDate(OriginalCurrentDR("EC_FROM_DATE", DataRowVersion.Original)) = CDate(OriginalPriorDR("EC_FROM_DATE", DataRowVersion.Original)) AndAlso
                    CDate(OriginalCurrentDR("EC_THRU_DATE", DataRowVersion.Original)) = CDate(OriginalPriorDR("EC_THRU_DATE", DataRowVersion.Original)) Then

                    SharedCoverage = True 'indicates prior coverage row and network needs to be trimmed

                End If
            End If

            If ProposedCurrentDR Is Nothing AndAlso (OriginalCurrentDR Is Nothing OrElse (OriginalCurrentDR IsNot Nothing AndAlso (OriginalCurrentDR.RowState = DataRowState.Unchanged OrElse SharedCoverage))) Then Return

            'Add could result in prior row being completely eliminated which would be implemented as an update of the current row.
            If ProposedCurrentDR IsNot Nothing AndAlso OriginalCurrentDR IsNot Nothing AndAlso OriginalCurrentDR.RowState = DataRowState.Deleted Then
                ProposedCurrentDR("EC_ONLINE_DATE") = OriginalCurrentDR("EC_ONLINE_DATE")
                SaveHMOCoverageChange(ProposedCurrentDR, DestinationActions.Modify, activityTimestamp, transaction) 'update current row
                Return
            End If

            If PriorCoveragePeriodChanged Then '#5, 5.5, 6, 6.5
                If CDate(OriginalPriorDR("EC_FROM_DATE")) <> CDate(OriginalPriorDR("FROM_DATE")) Then OriginalPriorDR("EC_FROM_DATE") = OriginalPriorDR("FROM_DATE")
                If CDate(OriginalPriorDR("EC_THRU_DATE")) <> CDate(OriginalPriorDR("THRU_DATE")) Then OriginalPriorDR("EC_THRU_DATE") = OriginalPriorDR("THRU_DATE")
                SaveHMOCoverageChange(OriginalPriorDR, DestinationActions.Modify, activityTimestamp, transaction) 'update prior row
            ElseIf PriorCoverageErased Then
                SaveHMOCoverageChange(OriginalPriorDR, DestinationActions.Delete, activityTimestamp, transaction) 'update prior row
            End If

            If CurrentCoverageErased Then
                SaveHMOCoverageChange(OriginalCurrentDR, DestinationActions.Delete, activityTimestamp, transaction) 'extend existing coverage

            ElseIf CurrentCoverageAdded OrElse CurrentCoverageChanged OrElse CurrentCoveragePeriodChanged Then 'update coverage row to force Elig SP to process change #1, 2, 3, 3.5, 4, 5, 5.5, 6, 6.5, 7, 9
                'perform any update of current row before attempting to add a new coverage row.
                If CurrentCoveragePeriodChanged Then
                    If CDate(OriginalCurrentDR("EC_FROM_DATE")) <> CDate(OriginalCurrentDR("FROM_DATE")) Then OriginalCurrentDR("EC_FROM_DATE") = OriginalCurrentDR("FROM_DATE")
                    If CDate(OriginalCurrentDR("EC_THRU_DATE")) <> CDate(OriginalCurrentDR("THRU_DATE")) Then OriginalCurrentDR("EC_THRU_DATE") = OriginalCurrentDR("THRU_DATE")
                End If

                SaveHMOCoverageChange(OriginalCurrentDR, DestinationActions.Modify, activityTimestamp, transaction) 'extend existing coverage

            End If

            If ProposedCurrentDR IsNot Nothing Then

                ProposedCurrentDR("EC_FROM_DATE") = ProposedCurrentDR("FROM_DATE")
                ProposedCurrentDR("EC_THRU_DATE") = ProposedCurrentDR("THRU_DATE")

                SaveHMOCoverageChange(ProposedCurrentDR, DestinationActions.Add, activityTimestamp, transaction)

            End If

            Return

        Catch db2ex As DB2Exception

            Throw

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub PerformNetworkChanges(ByRef changedMedCoverageDRs As DataSet, ByVal activityTimestamp As DateTime, ByRef transaction As DbTransaction)

        Dim OriginalCurrentDR As DataRow 'row being expired, changed, etc
        Dim OriginalPriorDR As DataRow 'row possiblly modified to reflect new THRU_DATE

        Dim ProposedCurrentDR As DataRow 'Final Current Row, could represent future date
        Dim UnTerminatedDR As DataRow 'HMO Network that was detatched from Coverage in the past.
        Dim UnTerminatedDT As DataTable 'HMO Network that was detatched from Coverage in the past.

        'Dim CoverageChanged As Boolean = False
        Dim ProposedNetworkChanged As Boolean = False
        Dim CurrentNetworkChanged As Boolean = False
        Dim PriorNetworkChanged As Boolean = False

        Dim ProposedNetworkAdded As Boolean = False
        Dim CurrentNetworkAdded As Boolean = False

        Dim CurrentNetworkErased As Boolean = False
        Dim PriorNetworkErased As Boolean = False

        Dim CurrentCoveragePeriodChanged As Boolean = False
        Dim PriorCoveragePeriodChanged As Boolean = False

        Try

            'Resolve if change is new Coverage choice, existing extended or trimmed coverage or network change

            'Add Possibilities
            'New Coverage (No Network)
            'New Coverage + Network

            'Modify Possibilities
            'Coverage Code changed
            'Coverage Code started earlier affecting network assignment
            'Coverage Code started earlier no associated network assignment

            'Coverage Code started later
            'Network updated
            '

            Dim OriginalCurrentQuery =
                From ExistingCoverage In CType(_MedCoverageBS.DataSource, DataTable).AsEnumerable()
                Where ExistingCoverage.RowState <> DataRowState.Added
                Order By ExistingCoverage.Field(Of Date)("THRU_DATE", DataRowVersion.Original) Descending

            For Each DR As DataRow In OriginalCurrentQuery

                If OriginalCurrentDR IsNot Nothing AndAlso OriginalPriorDR Is Nothing Then
                    OriginalPriorDR = DR
                    Exit For
                End If

                If OriginalCurrentDR Is Nothing Then
                    OriginalCurrentDR = DR
                End If
            Next

            Dim AddedCurrentQuery =
                From AddedCoverage In changedMedCoverageDRs.Tables("ELIG_COVERAGE").AsEnumerable()
                Where AddedCoverage.RowState = DataRowState.Added

            If AddedCurrentQuery.Any Then
                ProposedCurrentDR = AddedCurrentQuery.First
            End If

            If ProposedCurrentDR IsNot Nothing Then 'Coverage is being added

                If OriginalCurrentDR Is Nothing Then

                    If Not IsDBNull(ProposedCurrentDR("HMO_NETWORK")) Then
                        ProposedNetworkAdded = True
                    End If

                ElseIf OriginalCurrentDR.RowState = DataRowState.Modified Then '#0.5

                    If ProposedCurrentDR("HMO_NETWORK", DataRowVersion.Current).ToString <> OriginalCurrentDR("HMO_NETWORK", DataRowVersion.Original).ToString Then

                        If IsDBNull(ProposedCurrentDR("HMO_NETWORK")) Then
                            CurrentNetworkChanged = True
                        Else
                            ProposedNetworkAdded = True
                        End If
                    Else
                        If Not IsDBNull(ProposedCurrentDR("HMO_NETWORK")) Then
                            ProposedNetworkAdded = True
                        End If
                    End If
                End If

            End If

            If OriginalCurrentDR IsNot Nothing Then 'possible if first coverage row to be added 

                If OriginalCurrentDR.RowState = DataRowState.Modified Then

                    If OriginalCurrentDR("HMO_NETWORK").ToString <> OriginalCurrentDR("HMO_NETWORK", DataRowVersion.Original).ToString Then
                        CurrentNetworkChanged = True

                        If IsDBNull(OriginalCurrentDR("HMO_NETWORK")) Then
                            CurrentNetworkErased = True
                        ElseIf IsDBNull(OriginalCurrentDR("HMO_NETWORK", DataRowVersion.Original)) Then
                            CurrentNetworkAdded = True
                        End If
                    End If

                    If Not (IsDBNull(OriginalCurrentDR("HMO_NETWORK")) AndAlso IsDBNull(OriginalCurrentDR("HMO_NETWORK", DataRowVersion.Original))) Then
                        If OriginalCurrentDR("FROM_DATE").ToString <> OriginalCurrentDR("FROM_DATE", DataRowVersion.Original).ToString Then
                            CurrentCoveragePeriodChanged = True 'indicates current coverage row and network needs to be modified
                        End If

                        If OriginalCurrentDR("THRU_DATE").ToString <> OriginalCurrentDR("THRU_DATE", DataRowVersion.Original).ToString Then '#0.5
                            CurrentCoveragePeriodChanged = True 'indicates current coverage row was replaced by an addition
                        End If

                        If OriginalCurrentDR("FROM_DATE").ToString <> OriginalCurrentDR("FROM_DATE", DataRowVersion.Original).ToString AndAlso OriginalCurrentDR("FROM_DATE", DataRowVersion.Original).ToString = OriginalCurrentDR("EC_FROM_DATE", DataRowVersion.Original).ToString Then '#0.5
                            CurrentNetworkChanged = True 'indicates current coverage row was replaced by an addition
                        End If
                    End If

                ElseIf OriginalCurrentDR.RowState = DataRowState.Deleted Then
                    If Not IsDBNull(OriginalCurrentDR("HMO_NETWORK", DataRowVersion.Original)) Then
                        CurrentNetworkErased = True
                    End If
                End If
            End If

            If OriginalPriorDR IsNot Nothing Then

                If OriginalPriorDR.RowState = DataRowState.Modified Then

                    If Not IsDBNull(OriginalPriorDR("HMO_NETWORK")) Then
                        PriorCoveragePeriodChanged = True 'if prior coverage row has a network and is modified then that indicates the Thru date has changed.
                    End If

                ElseIf OriginalPriorDR.RowState = DataRowState.Deleted Then
                    If Not IsDBNull(OriginalPriorDR("HMO_NETWORK", DataRowVersion.Original)) Then
                        If CDate(OriginalPriorDR("EC_FROM_DATE", DataRowVersion.Original)) = CDate(OriginalPriorDR("NET_FROM_DATE", DataRowVersion.Original)) AndAlso CDate(OriginalPriorDR("EC_THRU_DATE", DataRowVersion.Original)) = CDate(OriginalPriorDR("NET_THRU_DATE", DataRowVersion.Original)) Then
                            PriorNetworkErased = True
                        ElseIf CDate(OriginalPriorDR("EC_THRU_DATE", DataRowVersion.Original)) = CDate(OriginalPriorDR("NET_THRU_DATE", DataRowVersion.Original)) Then
                            PriorNetworkChanged = True
                        End If
                    End If
                End If

            End If

            'first coverage row for participant
            If ProposedCurrentDR IsNot Nothing Then

                If ProposedNetworkChanged OrElse ProposedNetworkAdded Then

                    ProposedCurrentDR("EC_FROM_DATE") = CDate(ProposedCurrentDR("FROM_DATE"))
                    ProposedCurrentDR("EC_THRU_DATE") = CDate(ProposedCurrentDR("THRU_DATE"))

                    SaveHMONetworkChange(ProposedCurrentDR, DestinationActions.Add, activityTimestamp, transaction) '# 0.5
                    If OriginalCurrentDR Is Nothing Then Return
                End If

                'Add could result in prior row being completely eliminated which would be implemented as an update of the current row.
                If CurrentNetworkErased Then
                    SaveHMONetworkChange(OriginalCurrentDR, DestinationActions.Delete, activityTimestamp, transaction) ' current row merged with proposed row
                End If

            End If

            If OriginalPriorDR IsNot Nothing AndAlso OriginalPriorDR.RowState <> DataRowState.Unchanged Then

                If PriorNetworkErased Then '# 7.5
                    SaveHMONetworkChange(OriginalPriorDR, DestinationActions.Delete, activityTimestamp, transaction)

                ElseIf PriorNetworkChanged OrElse PriorCoveragePeriodChanged Then '#
                    If OriginalPriorDR.RowState <> DataRowState.Deleted Then
                        SaveHMONetworkChange(OriginalPriorDR, DestinationActions.Modify, activityTimestamp, transaction) 'update prior row if a network is involved
                    Else
                        Dim OriginalDR = (From ExistingCoverage In CType(_MedCoverageBS.DataSource, DataTable).AsEnumerable()
                                          Where ExistingCoverage.RowState = DataRowState.Deleted).First

                        Dim CloneDT As DataTable = CType(_MedCoverageBS.DataSource, DataTable).Clone
                        Dim CloneDR As DataRow = CloneDT.NewRow

                        For Each Col As DataColumn In OriginalDR.Table.Columns
                            CloneDR(Col.ColumnName) = OriginalDR(Col.ColumnName, DataRowVersion.Original)
                        Next

                        CloneDR("FROM_DATE") = CDate(CloneDR("NET_FROM_DATE")) 'perform change after accept to show row as modified
                        CloneDR("THRU_DATE") = CDate(CloneDR("NET_THRU_DATE")) 'perform change after accept to show row as modified

                        '                        CloneDR("THRU_DATE") = CDate(CloneDR("NET_THRU_DATE")) 'perform change after accept to show row as modified

                        CloneDT.Rows.Add(CloneDR)
                        CloneDT.AcceptChanges()

                        CloneDR("THRU_DATE") = CDate(OriginalCurrentDR("FROM_DATE")).AddDays(-1) 'perform change after accept to show row as modified

                        SaveHMONetworkChange(CloneDR, DestinationActions.Modify, activityTimestamp, transaction) 'update prior row if a network is involved
                    End If
                End If
            End If

            If OriginalCurrentDR IsNot Nothing AndAlso OriginalCurrentDR.RowState <> DataRowState.Unchanged Then

                'perform any update of current row before attempting to add a new coverage row. 
                If CurrentNetworkErased Then
                    SaveHMONetworkChange(OriginalCurrentDR, DestinationActions.Delete, activityTimestamp, transaction) 'Network removed
                ElseIf CurrentNetworkAdded Then '# 00
                    SaveHMONetworkChange(OriginalCurrentDR, DestinationActions.Add, activityTimestamp, transaction) '#2.5, 6.5 
                ElseIf CurrentNetworkChanged OrElse CurrentCoveragePeriodChanged Then '# 0.5, 1
                    SaveHMONetworkChange(OriginalCurrentDR, DestinationActions.Modify, activityTimestamp, transaction) 'Network And/Or Period changed
                End If
            End If

            If UnTerminatedDR IsNot Nothing AndAlso Not IsDBNull(UnTerminatedDR("HMO_NETWORK")) Then 'Add proposed network  #1, 7
                SaveHMONetworkChange(UnTerminatedDR, DestinationActions.Modify, activityTimestamp, transaction)
            End If

        Catch db2ex As DB2Exception

            Throw

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub SaveHMOCoverageChange(ByVal coverageDR As DataRow, ByVal actions As DestinationActions, ByVal activityTimestamp As DateTime, ByRef transaction As DbTransaction)

        Dim HistSum As String = ""
        Dim HistDetail As String = ""

        Try

            If actions = DestinationActions.Modify Then 'Update

                HistDetail = DataGridCustom.IdentifyChanges(coverageDR, MedCoverageDataGrid)

                HistSum = "MEDICAL COVERAGE OF FAMILYID:  " & CStr(coverageDR("FAMILY_ID")) & " WAS MODIFIED"
                HistDetail = UFCWGeneral.DomainUser.ToUpper & " MODIFIED THE MEDICAL COVERAGE RECORD " & vbCrLf &
                                        "THE MODIFICATIONS WERE: " & HistDetail & vbCrLf & If(cmbMedLetter.Text = "Y", " Letter was requested.", " No Letter Requested.")

                RegMasterDAL.UpdateEligCoverage(activityTimestamp, CDate(coverageDR("EC_ONLINE_DATE", DataRowVersion.Original)), CInt(coverageDR("FAMILY_ID")), CInt(coverageDR("RELATION_ID")), CDate(coverageDR("EC_FROM_DATE")), CDate(coverageDR("EC_THRU_DATE")), CInt(coverageDR("COVERAGE_CODE")), CInt(coverageDR("COVERAGE_ID")), If(cmbMedLetter.Text = "Y", "Y", "N"), transaction)
                RegMasterDAL.CreateRegHistory(CInt(coverageDR("FAMILY_ID")), CInt(coverageDR("RELATION_ID")), Nothing, Nothing, "ELIGCOVMEDUPD", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, transaction)

            End If

            If actions = DestinationActions.Add Then ' 

                HistDetail = DataGridCustom.IdentifyChanges(coverageDR, MedCoverageDataGrid)

                HistSum = "MEDICAL COVERAGE OF FAMILYID: " & Me.FamilyID & " WAS ADDED"
                HistDetail = UFCWGeneral.DomainUser.ToUpper & " ADDED THE MEDICAL COVERAGE RECORD " & vbCrLf &
                                        "THE ADDITIONS WERE: " & HistDetail & vbCrLf & If(cmbMedLetter.Text = "Y", " Letter was requested.", " No Letter Requested.")

                coverageDR("Coverage_ID") = RegMasterDAL.AddEligCoverage(activityTimestamp, CInt(coverageDR("FAMILY_ID")), CInt(coverageDR("RELATION_ID")), CStr(coverageDR("COVERAGE_TYPE")), CDate(coverageDR("EC_FROM_DATE")), CDate(coverageDR("EC_THRU_DATE")), CInt(coverageDR("COVERAGE_CODE")), cmbMedLetter.Text, transaction)
                RegMasterDAL.CreateRegHistory(CInt(coverageDR("FAMILY_ID")), CInt(coverageDR("RELATION_ID")), Nothing, Nothing, "ELIGCOVMEDADD", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, transaction)

            End If

            If actions = DestinationActions.Delete Then ' 

                HistDetail = DataGridCustom.IdentifyChanges(coverageDR, MedCoverageDataGrid)

                HistSum = "MEDICAL COVERAGE OF FAMILYID: " & Me.FamilyID & " WAS DELETED"
                HistDetail = UFCWGeneral.DomainUser.ToUpper & " DELETED COVERAGE " & Microsoft.VisualBasic.vbCrLf & "THE DELETION WAS: " & Microsoft.VisualBasic.vbCrLf & HistDetail

                RegMasterDAL.DeleteEligCoverage(CDate(coverageDR("EC_ONLINE_DATE", DataRowVersion.Original)), CInt(coverageDR("FAMILY_ID", DataRowVersion.Original)), CInt(coverageDR("RELATION_ID", DataRowVersion.Original)), CDate(coverageDR("FROM_DATE", DataRowVersion.Original)), CDate(coverageDR("THRU_DATE", DataRowVersion.Original)), CInt(coverageDR("COVERAGE_CODE", DataRowVersion.Original)), coverageDR("COVERAGE_TYPE", DataRowVersion.Original).ToString, transaction)
                RegMasterDAL.CreateRegHistory(CInt(coverageDR("FAMILY_ID", DataRowVersion.Original)), CInt(coverageDR("RELATION_ID", DataRowVersion.Original)), Nothing, Nothing, "ELIGCOVMEDDEL", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, transaction)

            End If

        Catch db2ex As DB2Exception

            Throw

        Catch ex As Exception


                Throw
        End Try

    End Sub

    Private Sub SaveHMONetworkChange(ByVal networkDR As DataRow, ByVal actions As DestinationActions, ByVal activityTimestamp As DateTime, ByRef transaction As DbTransaction)

        Dim HistSum As String = ""
        Dim HistDetail As String = ""

        Try

            If actions = DestinationActions.Modify Then 'Update

                HistDetail = DataGridCustom.IdentifyChanges(networkDR, MedCoverageDataGrid) 'Send XML to build style on the fly

                HistSum = "HMO NETWORK OF FAMILYID: " & _FamilyID & " WAS MODIFIED"
                HistDetail = UFCWGeneral.DomainUser.ToUpper & " MODIFIED THE HMO NETWORK RECORD " & Microsoft.VisualBasic.vbCrLf & "THE MODIFICATIONS WERE: " & Microsoft.VisualBasic.vbCrLf & HistDetail

                RegMasterDAL.UpdateHMONetworkInfo(activityTimestamp, If(IsDBNull(networkDR("HMO_ID")), Nothing, CType(networkDR("HMO_ID"), Integer?)), CDate(networkDR("FROM_DATE")), CDate(networkDR("THRU_DATE")), CInt(networkDR("FAMILY_ID")), CInt(networkDR("RELATION_ID")), CStr(networkDR("HMO_NETWORK")), CDate(networkDR("ONLINE_DATE", DataRowVersion.Original)), UFCWGeneral.DomainUser.ToUpper, CDate(networkDR("FROM_DATE", DataRowVersion.Original)), If(Not IsDBNull(networkDR("NET_THRU_DATE", DataRowVersion.Original)) AndAlso (CDate(networkDR("THRU_DATE", DataRowVersion.Original)) <> CDate(networkDR("NET_THRU_DATE", DataRowVersion.Original))), CDate(networkDR("NET_THRU_DATE", DataRowVersion.Original)), CDate(networkDR("THRU_DATE", DataRowVersion.Original))), transaction)

                RegMasterDAL.CreateRegHistory(_FamilyID, 0, Nothing, Nothing, "ELIGCOVHMONETUPD", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, transaction)

            End If

            If actions = DestinationActions.Delete Then

                HistDetail = DataGridCustom.IdentifyChanges(networkDR, MedCoverageDataGrid)

                HistSum = "HMO NETWORK OF FAMILYID: " & _FamilyID & " WAS DELETED"
                HistDetail = UFCWGeneral.DomainUser.ToUpper & " DELETED HMO NETWORK " & Microsoft.VisualBasic.vbCrLf & "THE DELETION WAS: " & Microsoft.VisualBasic.vbCrLf & HistDetail

                RegMasterDAL.DeleteHMONetworkInfo(If(IsDBNull(networkDR("HMO_ID", DataRowVersion.Original)), Nothing, CType(networkDR("HMO_ID", DataRowVersion.Original), Integer?)), CInt(networkDR("FAMILY_ID", DataRowVersion.Original)), CInt(networkDR("RELATION_ID", DataRowVersion.Original)), CDate(networkDR("ONLINE_DATE", DataRowVersion.Original)), CDate(networkDR("FROM_DATE", DataRowVersion.Original)), CDate(networkDR("THRU_DATE", DataRowVersion.Original)), transaction)

                RegMasterDAL.CreateRegHistory(_FamilyID, 0, Nothing, Nothing, "ELIGCOVHMONETDEL", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, transaction)
            End If

            If actions = DestinationActions.Add Then ' 

                HistDetail = DataGridCustom.IdentifyChanges(networkDR, MedCoverageDataGrid)

                HistSum = "HMO NETWORK OF FAMILYID: " & _FamilyID & " WAS ADDED"
                HistDetail = UFCWGeneral.DomainUser.ToUpper & " ADDED THE HMO NETWORK RECORD " & Microsoft.VisualBasic.vbCrLf & "THE ADDITION WAS: " & Microsoft.VisualBasic.vbCrLf & HistDetail

                RegMasterDAL.AddHMONetworkInfo(activityTimestamp, If(IsDBNull(networkDR("HMO_ID")), Nothing, CType(networkDR("HMO_ID"), Integer?)), CDate(networkDR("FROM_DATE")), CDate(networkDR("THRU_DATE")), CInt(networkDR("FAMILY_ID")), CInt(networkDR("RELATION_ID")), CStr(networkDR("HMO_NETWORK")), UFCWGeneral.DomainUser.ToUpper, transaction)

                RegMasterDAL.CreateRegHistory(_FamilyID, 0, Nothing, Nothing, "ELIGCOVHMONETADD", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, transaction)
            End If

        Catch db2ex As DB2Exception

            Throw

        Catch ex As Exception


                Throw
        End Try

    End Sub

    Private Function MedPendingChanges() As Boolean

        Dim Modifications As String = ""

        Try

            _MedChangedDRs = _MedCoverageDS.GetChanges()

            If _MedChangedDRs IsNot Nothing Then

                For Each DR As DataRow In _MedChangedDRs.Tables("ELIG_COVERAGE").Rows

                    If DR.RowState <> DataRowState.Added Then
                        'attempt to exclude rows accidently changed during navigation operations
                        Modifications = DataGridCustom.IdentifyChanges(DR, MedCoverageDataGrid)

                        If Modifications.Length > 0 Then
                            Return True
                        End If

                    ElseIf DR.RowState = DataRowState.Added Then
                        Return True
                    End If

                Next
            End If

            Return False

        Catch ex As Exception

                Throw
        End Try

    End Function

    Private Sub RetrieveMedCoverageValues(threadState As [Object])

        Dim DT As DataTable
        Dim AREvent As AutoResetEvent = CType(threadState, AutoResetEvent)

        Try

            DT = RegMasterDAL.RetrieveMedicalCoverageValues
            _MedCoverageValuesDS.Merge(DT)

        Catch ex As Exception

                Throw

        Finally
            AREvent.Set()

        End Try
    End Sub

    Private Sub LoadMedCoverageValuesCombo()

        Try

            'wait for thread.
            If _MedCoverageValuesWaitHandle IsNot Nothing Then
                WaitHandle.WaitAll(_MedCoverageValuesWaitHandle)
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _MedCoverageValuesComboBS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            RemoveHandler cmbMedCoverage.SelectedIndexChanged, AddressOf cmbMedCoverage_SelectedIndexChanged

            cmbMedCoverage.DataSource = Nothing
            cmbMedCoverage.Items.Clear()

            _MedCoverageValuesComboBS.DataSource = _MedCoverageValuesDS.Tables("COVERAGE_VALUES")
            _MedCoverageValuesComboBS.Sort = "COVERAGE_VALUE"

        Catch ex As Exception

                Throw

        Finally

            AddHandler cmbMedCoverage.SelectedIndexChanged, AddressOf cmbMedCoverage_SelectedIndexChanged
            _MedCoverageValuesWaitHandle = Nothing

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _MedCoverageValuesComboBS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try
    End Sub

    Private Function ValidateMedCoverageChanges() As Boolean
        Dim MedCoverageDRs() As DataRow

        Try
            ClearErrors()

            'MedUIContainer.ValidateChildren()

            If _MedCoverageBS.Count > 0 AndAlso _MedCoverageBS.Position > -1 Then

                Dim DR As DataRow = DirectCast(_MedCoverageBS.Current, DataRowView).Row

                If IsDate(DR("FROM_DATE")) OrElse IsDate(txtMedFromDate.Text) Then
                Else
                    ErrorProvider1.SetErrorWithTracking(txtMedFromDate, " From Date is invalid (MM-dd-yyyy)")
                End If

                If IsDate(DR("THRU_DATE")) OrElse IsDate(txtMedThruDate.Text) Then
                Else
                    ErrorProvider1.SetErrorWithTracking(txtMedThruDate, " Thru Date is invalid (MM-dd-yyyy)")
                End If

                If IsDate(DR("FROM_DATE")) AndAlso IsDate(DR("THRU_DATE")) Then
                    If (CDate(DR("FROM_DATE")) < CDate(DR("THRU_DATE"))) Then

                    Else
                        ErrorProvider1.SetErrorWithTracking(Me.txtMedThruDate, " Thru Date must be after From Date")
                    End If
                End If

                If DR("COVERAGE_CODE").ToString.Trim.Length = 0 Then
                    ErrorProvider1.SetErrorWithTracking(Me.cmbMedCoverage, " Select a valid Medical Plan / Coverage code")
                End If

                '' Duplicate from date and coveragetype for same period
                If IsDate(DR("FROM_DATE")) AndAlso Not IsDBNull(DR("COVERAGE_TYPE")) Then
                    MedCoverageDRs = _MedCoverageDS.Tables("ELIG_COVERAGE").Select(" FROM_DATE= '" & CStr(DR("FROM_DATE")) & "' ", "", DataViewRowState.CurrentRows)
                    If MedCoverageDRs.Length > 1 Then
                        ErrorProvider1.SetErrorWithTracking(txtMedFromDate, "Another coverage record starts from same From Date. Please provide different From date.")
                    End If
                End If

                ''  Finding any overlaping period is occuring when adding record at the end
                If DR.RowState = DataRowState.Added Then

                    If cmbHMONetwork.Visible AndAlso DR("HMO_NETWORK").ToString.Trim.Length = 0 AndAlso CType(_HMONetworkValuesBS.DataSource, DataTable).Select("SPLIT_MEDICARE_ONLY_SW = 0").ToArray.Any Then
                        ErrorProvider1.SetErrorWithTracking(Me.cmbMedCoverage, " Select a valid HMO Network")
                    End If

                    If IsDate(DR("FROM_DATE")) AndAlso IsDate(DR("THRU_DATE")) AndAlso CDate(DR("THRU_DATE")) = CDate("12-31-9999") Then
                        MedCoverageDRs = _MedCoverageDS.Tables("ELIG_COVERAGE").Select("'" & CStr(DR("FROM_DATE")) & "' > FROM_DATE  AND '" & CStr(DR("FROM_DATE")) & "' < THRU_DATE", "", DataViewRowState.CurrentRows)
                        If MedCoverageDRs.Length > 1 Then
                            ErrorProvider1.SetErrorWithTracking(txtMedFromDate, "Overlapping of coverage is not valid. Please provide different FROM Date.")
                        End If
                    End If
                End If

                ''  Finding any overlaping period is occuring when modifying the record
                Dim OverLapping As Boolean = False
                If DR.RowState = DataRowState.Modified Then
                    If (Not IsDBNull(DR("FROM_DATE"))) AndAlso (Not IsDBNull(DR("THRU_DATE"))) AndAlso (Not IsDBNull(DR("COVERAGE_TYPE"))) Then
                        MedCoverageDRs = _MedCoverageDS.Tables("ELIG_COVERAGE").Select("COVERAGE_TYPE = '" & CStr(DR("COVERAGE_TYPE")) & "'", "", DataViewRowState.Unchanged)
                        If MedCoverageDRs.Length > 0 Then
                            For Each CoverageDR As DataRow In MedCoverageDRs
                                OverLapping = OverlappingPeriods(CDate(DR("FROM_DATE")), CDate(DR("THRU_DATE")), CDate(CoverageDR("FROM_DATE")), CDate(CoverageDR("THRU_DATE")))
                                If OverLapping = True Then
                                    MessageBox.Show("Overlapping of coverage is not valid." & Environment.NewLine & " Please correct the dates before Continuing", "Overlapping", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    Return True
                                End If
                            Next
                        End If
                    End If
                End If


                ''  Finding any overlaping period is occuring when adding record in the middle of coverage periods
                OverLapping = False
                If IsDate(DR("THRU_DATE")) Then
                    If DR.RowState = DataRowState.Added AndAlso CDate(DR("THRU_DATE")) <> CDate("12-31-9999") Then
                        If (Not IsDBNull(DR("FROM_DATE"))) AndAlso (Not IsDBNull(DR("THRU_DATE"))) AndAlso (Not IsDBNull(DR("COVERAGE_TYPE"))) Then
                            MedCoverageDRs = _MedCoverageDS.Tables("ELIG_COVERAGE").Select("COVERAGE_TYPE = '" & CStr(DR("COVERAGE_TYPE")) & "'", "", DataViewRowState.Unchanged)
                            If MedCoverageDRs.Length > 0 Then
                                For Each drrow As DataRow In MedCoverageDRs
                                    OverLapping = OverlappingPeriods(CDate(DR("FROM_DATE")), CDate(DR("THRU_DATE")), CDate(drrow("FROM_DATE")), CDate(drrow("THRU_DATE")))
                                    If OverLapping = True Then
                                        MessageBox.Show("Overlapping of coverage is not valid." & Environment.NewLine & " Please correct the dates before Continuing", "Overlapping", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        Return True
                                    End If
                                Next
                            End If
                        End If
                    End If
                End If

                Dim OuterQuery =
                    From ExistingCoverage In CType(_MedCoverageBS.DataSource, DataTable).AsEnumerable()
                    Where ExistingCoverage.RowState <> DataRowState.Deleted
                    Order By ExistingCoverage.Field(Of Date)("THRU_DATE") Descending

                For Each CurrentDR As DataRow In OuterQuery

                    Dim InnerQuery =
                        From ExistingCoverage In CType(_MedCoverageBS.DataSource, DataTable).AsEnumerable()
                        Where ExistingCoverage.RowState <> DataRowState.Deleted _
                        AndAlso CDate(CurrentDR("FROM_DATE")) >= ExistingCoverage.Field(Of Date)("FROM_DATE") _
                        AndAlso CDate(CurrentDR("THRU_DATE")) <= ExistingCoverage.Field(Of Date)("THRU_DATE")

                    If InnerQuery.Count > 1 Then
                        MessageBox.Show("Coverage rows overlap one or more times. " & Environment.NewLine & "Contact I.T for data correction." & Environment.NewLine, "From Date", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return True
                    End If

                Next

                '' For any HMO we allow 12 months future from date
                If IsDate(DR("FROM_DATE")) AndAlso Not IsDBNull(DR("COVERAGE_CODE")) Then
                    If CInt(DR("COVERAGE_CODE")) > 6 And CInt(DR("COVERAGE_CODE")) < 91 Then 'HMO
                        If (DateDiff(DateInterval.Month, Now.Date, CDate(DR("FROM_DATE"))) > 12) Then
                            ErrorProvider1.SetErrorWithTracking(txtMedFromDate, "For HMOs 12 months is the maximum future date allowed. ")
                        End If
                    Else 'PPO

                        If Not _REGMSupervisor AndAlso (DateDiff(DateInterval.Month, Now.Date, CDate(DR("FROM_DATE"))) > 5) Then
                            ErrorProvider1.SetErrorWithTracking(txtMedFromDate, "From date over 5 months requires supervisors priviledges. ")
                        End If

                    End If
                End If

                '' Letter generation
                If cmbMedLetter.Text.ToString.Trim.Length = 0 Then
                    ErrorProvider1.SetErrorWithTracking(Me.cmbMedLetter, " Please choose Letter option")
                End If

                If cmbHMONetwork.Visible AndAlso Not IsDBNull(DR("HMO_NETWORK")) Then

                    Dim NetworkQuery =
                        From SelectedNetwork In CType(_HMONetworkValuesBS.DataSource, DataTable).AsEnumerable()
                        Where DR("HMO_NETWORK").ToString = SelectedNetwork.Field(Of String)("HMO_NETWORK")

                    Dim NetworkDR As DataRow = NetworkQuery.First

                    If CInt(NetworkDR("SPLIT_MEDICARE_ONLY_SW")) = 1 Then
                        Dim Result As DialogResult = CType(MsgBox("The Network selected is valid for Families with both Medicare and Non Medicare members. Select Yes to Confirm selection", CType(MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, MsgBoxStyle), "Medicare Split Family"), DialogResult)
                        If Result <> DialogResult.Yes Then
                            cmbHMONetwork.SelectedIndex = -1
                            Return True
                        End If
                    End If

                End If

                If CDate(CType(_MedCoverageValuesComboBS.Current, DataRowView).Row("THRU_DATE")) < CDate(DR("THRU_DATE")) Then
                    Dim Result As DialogResult = CType(MsgBox("The Coverage selected is valid until " & CDate(CType(_MedCoverageValuesComboBS.Current, DataRowView).Row("THRU_DATE")).Date.ToShortDateString & ". Additional coverage is required for eligibility to be active. ", CType(MsgBoxStyle.OkCancel + MsgBoxStyle.Exclamation, MsgBoxStyle), "Active Coverage required."), DialogResult)
                    If Result <> DialogResult.OK Then
                        Return True
                    End If
                End If

                If GetErrorCount(ErrorProvider1) > 0 Then
                    Return True
                End If
            End If

            Return False
        Catch ex As Exception

                Throw

        End Try
    End Function

    Private Sub frmCoverageElection_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged

        'first time showing form to set defaults.
        If Not _Disposed Then
            If Me.Visible Then
                SetMedUIElements()
                SetDenUIElements()
            Else
                SetSettings()
            End If
        End If

    End Sub

    Private Sub cmbMedCoverage_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbMedCoverage.SelectionChangeCommitted
        ' specifically called when mouse interacts to select list item, not called when mouse is used to select item

        Dim DR As DataRow
        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Try

            If _MedCoverageBS Is Nothing OrElse _MedCoverageBS.Position < 0 OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH: mm:ss.fffffff") & " In:  BS(" & _MedCoverageBS.Position.ToString & ") SI(" & CBox.Text & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            'CType(CBox.Parent, TransparentContainer).ValidateChildren() 'this will trigger validation of the cmbbox triggering write of value to DS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _MedCoverageBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally

        End Try

    End Sub

    Private Sub RetrieveHMONetworkValues(threadState As [Object])

        Dim DT As DataTable
        Dim AREvent As AutoResetEvent = CType(threadState, AutoResetEvent)

        Try

            DT = RegMasterDAL.RetrieveHMONetworks

            _HMONetworksValuesDS.Merge(DT)

        Catch ex As Exception

                Throw

        Finally
            AREvent.Set()

        End Try
    End Sub

    Private Sub LoadHMONetworksCombo()
        Try

            If _HMONetworksValuesWaitHandle IsNot Nothing Then
                WaitHandle.WaitAll(_HMONetworksValuesWaitHandle)
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _HMONetworkValuesBS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            RemoveHandler cmbHMONetwork.SelectedIndexChanged, AddressOf cmbHMONetwork_SelectedIndexChanged

            cmbHMONetwork.DataSource = Nothing
            cmbHMONetwork.Items.Clear()

            _HMONetworkValuesBS.DataSource = _HMONetworksValuesDS.Tables("HMO_NETWORK_LOOKUP")
            _HMONetworkValuesBS.Sort = "HMO_NETWORK"

        Catch ex As Exception

                Throw

        Finally
            _HMONetworksValuesWaitHandle = Nothing
            AddHandler cmbHMONetwork.SelectedIndexChanged, AddressOf cmbHMONetwork_SelectedIndexChanged

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out:  BS(" & _HMONetworkValuesBS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try
    End Sub

    Private Function FilterAndSelectHMONetworkValues() As Boolean

        'limit Coverage choices to specific period, triggered when either new row or from/thru dates change.
        Dim NetworksFilter As New StringBuilder
        Dim BS As BindingSource
        Dim CoverageFilterDate As Date
        Dim CoverageCode As String = Nothing
        Dim DR As DataRow

        Try

            BS = DirectCast(_MedCoverageBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If _MedCoverageBS.Position > -1 AndAlso _MedCoverageBS.Current IsNot Nothing Then
                DR = DirectCast(_MedCoverageBS.Current, DataRowView).Row

                If DR.HasVersion(DataRowVersion.Proposed) AndAlso Not IsDBNull(DR("FROM_DATE", DataRowVersion.Proposed)) Then
                    CoverageFilterDate = CDate(DR("FROM_DATE", DataRowVersion.Proposed))
                    CoverageCode = DR("COVERAGE_CODE", DataRowVersion.Proposed).ToString
                ElseIf Not IsDBNull(DR("FROM_DATE", DataRowVersion.Current)) Then
                    CoverageFilterDate = CDate(DR("FROM_DATE", DataRowVersion.Current))
                    CoverageCode = DR("COVERAGE_CODE", DataRowVersion.Current).ToString
                End If
            End If

            If (CoverageCode Is Nothing OrElse CoverageCode.Trim.Length < 1) OrElse ErrorProvider1.GetError(cmbMedCoverage).Trim.Length > 0 Then
                NetworksFilter.Append("")
            Else
                NetworksFilter.Append("'" & CoverageFilterDate.ToShortDateString & "' >= FROM_DATE AND '" & CoverageFilterDate.ToShortDateString & "' <= THRU_DATE ")
                NetworksFilter.Append(" AND MEDICAL_PLAN = " & CoverageCode)
            End If

            If _HMONetworkValuesBS.Filter <> NetworksFilter.ToString Then

                RemoveHandler cmbHMONetwork.SelectedIndexChanged, AddressOf cmbHMONetwork_SelectedIndexChanged 'suppress effects of index changing while connecting to BS

                _HMONetworkValuesBS.SuspendBinding()

                cmbHMONetwork.DataSource = Nothing
                cmbHMONetwork.ValueMember = ""
                cmbHMONetwork.DisplayMember = ""

                'If NetworksFilter.Length < 1 Then 'No HMO Networks available, BS is detached from ComboBox

                '    cmbHMONetwork.DataSource = Nothing
                '    cmbHMONetwork.ValueMember = ""
                '    cmbHMONetwork.DisplayMember = ""

                'ElseIf cmbHMONetwork.DataSource Is Nothing Then

                '    cmbHMONetwork.DataSource = _HMONetworkValuesBS
                '    cmbHMONetwork.ValueMember = "HMO_NETWORK"
                '    cmbHMONetwork.DisplayMember = "DESCRIPTION"

                'End If

                cmbHMONetwork.SelectedIndex = -1
                _HMONetworkValuesBS.Filter = NetworksFilter.ToString

                If _HMONetworkValuesBS.Count > 0 Then
                    cmbHMONetwork.DataSource = _HMONetworkValuesBS
                    cmbHMONetwork.ValueMember = "HMO_NETWORK"
                    cmbHMONetwork.DisplayMember = "DESCRIPTION"
                End If

                _HMONetworkValuesBS.ResumeBinding()

                AddHandler cmbHMONetwork.SelectedIndexChanged, AddressOf cmbHMONetwork_SelectedIndexChanged

                If cmbHMONetwork.DataBindings.Count > 0 Then cmbHMONetwork.DataBindings(0).ReadValue() 'filter changes list content so a refresh of the bound data is needed to rectify index vs data mistmatch

                If _HMONetworkValuesBS.Count > 0 AndAlso _HMONetworkValuesBS.Filter.Length > 0 Then Return True

            End If

            Return False

        Catch ex As Exception

                Throw

        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Function

    Private Function FilterAndSelectMedCoverageValues(ByRef bs As BindingSource) As Boolean

        'limit Coverage choices to specific period, triggered when either new row or from/thru dates change. Executes even when ReadOnly as content of list needs to be reset for historical rows
        Dim CoverageFilter As New StringBuilder
        Dim DR As DataRow

        Dim CoverageFilterDate As Date = Date.Now
        Dim CoverageDate As Date = Date.Now

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & bs.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = DirectCast(bs.Current, DataRowView).Row

            If DR IsNot Nothing Then

                If DR.HasVersion(DataRowVersion.Proposed) AndAlso Not IsDBNull(DR("FROM_DATE", DataRowVersion.Proposed)) Then
                    CoverageFilterDate = CDate(DR("FROM_DATE", DataRowVersion.Proposed))
                ElseIf Not IsDBNull(DR("FROM_DATE", DataRowVersion.Current)) Then
                    CoverageFilterDate = CDate(DR("FROM_DATE", DataRowVersion.Current))
                End If

                If Not IsDBNull(DR("FROM_DATE")) AndAlso IsDate(DR("FROM_DATE")) Then
                    CoverageDate = CType(DR("FROM_DATE"), Date)
                End If

                'CoverageFilter.Append("'" & CoverageDate.ToShortDateString & "' >= FROM_DATE AND '" & CoverageDate.ToShortDateString & "' <= THRU_DATE ")
                CoverageFilter.Append("FROM_DATE <= '" & CoverageDate.ToShortDateString & "' AND '" & CoverageDate.ToShortDateString & "' <= THRU_DATE ")

                If _MedCoverageValuesComboBS.Filter <> CoverageFilter.ToString Then

                    _MedCoverageValuesComboBS.Filter = CoverageFilter.ToString

                    If cmbMedCoverage.DataBindings.Count > 0 Then cmbMedCoverage.DataBindings(0).ReadValue() 'filter changes list content so a refresh of the bound data is needed to rectify index vs data mistmatch

                    Return True
                End If

            End If

            Return False

        Catch ex As Exception

                Throw

        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out:  BS(" & bs.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try
    End Function

    Private Sub MedCoverageBS_OnListChanged(sender As Object, e As System.ComponentModel.ListChangedEventArgs) Handles _MedCoverageBS.ListChanged

        Dim BS As BindingSource

        Try
            BS = CType(sender, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") RS(" & If(BS Is Nothing OrElse BS.Position < 0, "N/A", DirectCast(BS.Current, System.Data.DataRowView).Row.RowState.ToString) & ") O(" & e.OldIndex.ToString & ") N(" & e.NewIndex.ToString & ") CT(" & e.ListChangedType.ToString & ") ST(" & _UIState.ToString & ") SEL(" & If(BS Is Nothing OrElse BS.Position < 0 OrElse MedCoverageDataGrid.DataSource Is Nothing, "N/A", MedCoverageDataGrid.IsSelected(BS.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Select Case e.ListChangedType
                Case ListChangedType.ItemDeleted 'account for rows deleted due to a cancel action

                    Select Case _UIState
                        Case UIStates.Modifying


                        Case Else

                            If BS.Position > -1 AndAlso BS.Position <> e.NewIndex Then
                                If e.NewIndex > -1 Then BS.Position = e.NewIndex
                            End If

                    End Select

                    SetMedUIElements()

                Case ListChangedType.ItemMoved

                    Select Case DirectCast(BS.Current, System.Data.DataRowView).Row.RowState
                        Case DataRowState.Modified

                            Select Case _UIState
                                Case UIStates.Adding
                                Case Else
                            End Select

                        Case DataRowState.Added
                            Select Case _UIState
                                Case UIStates.Adding

                                    If e.OldIndex <> e.NewIndex OrElse (BS.Position > -1 AndAlso BS.Count = 1) Then
                                        MedCoverageDataGrid.Select(BS.Position)
                                    End If

                                Case Else
                            End Select

                        Case Else

                            Select Case _UIState
                                Case UIStates.Adding

                                Case Else
                            End Select

                    End Select

                Case ListChangedType.ItemChanged

                    If BS.Count = 0 Then 'item was changed in some way that excludes it from list due to filter exclusion
                        Return
                    End If

                    Select Case DirectCast(BS.Current, System.Data.DataRowView).Row.RowState
                        Case DataRowState.Modified

                            Select Case _UIState
                                Case UIStates.Adding
                                Case Else
                            End Select

                        Case DataRowState.Added
                            Select Case _UIState
                                Case UIStates.Adding
                                    'do nothing
                                Case Else
                            End Select

                        Case Else

                            Select Case _UIState
                                Case UIStates.Adding
                                    If BS.Position <> e.NewIndex OrElse BS.Position > -1 AndAlso BS.Count = 1 Then
                                        If e.NewIndex > -1 Then BS.Position = e.NewIndex
                                        SetMedUIElements()
                                        If e.NewIndex > -1 Then MedCoverageDataGrid.Select(e.NewIndex)
                                    End If
                                Case Else
                            End Select

                    End Select

                Case ListChangedType.Reset 'triggered by sorts or changes in grid filter

                    If BS.Position > -1 AndAlso BS.Position <> e.NewIndex Then
                        If e.NewIndex > -1 Then BS.Position = e.NewIndex
                        BS.ResetCurrentItem()
                        SetMedUIElements()
                    End If

                Case ListChangedType.ItemAdded 'includes items reincluded when filters change

                    Select Case _UIState
                        Case UIStates.Adding
                            If BS.Position <> e.NewIndex OrElse (BS.Position > -1 AndAlso BS.Count = 1) Then 'first item added

                                If e.NewIndex <> e.OldIndex AndAlso e.OldIndex > -1 Then
                                    MedCoverageDataGrid.UnSelect(e.OldIndex)
                                End If

                                If e.NewIndex > -1 Then BS.Position = e.NewIndex
                                If e.NewIndex > -1 Then MedCoverageDataGrid.Select(e.NewIndex)
                            End If

                        Case Else
                    End Select

                Case Else

            End Select

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") RS(" & If(BS Is Nothing OrElse BS.Position < 0, "N/A", DirectCast(BS.Current, System.Data.DataRowView).Row.RowState.ToString) & ") O(" & e.OldIndex.ToString & ") N(" & e.NewIndex.ToString & ") CT(" & e.ListChangedType.ToString & ") ST(" & _UIState.ToString & ") SEL(" & If(BS Is Nothing OrElse BS.Position < 0 OrElse MedCoverageDataGrid.DataSource Is Nothing, "N/A", MedCoverageDataGrid.IsSelected(BS.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

#End Region

#Region "Common Custom Subs\Functions"

    Private Function AdjustCurrentAndPriorCoverage(bs As BindingSource, dt As DataTable) As Boolean
        'Item was changed so determine if dates need to be adjusted.

        Dim CurrentDR As DataRow 'row being expired, changed, etc
        Dim PriorDR As DataRow 'row possiblly modified to reflect new THRU_DATE
        Dim NewDR As DataRow 'Final Current Row, could represent future date, or the approved row.
        Dim ChangesMade As Boolean

        Try
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & bs.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Dim CurrentQuery =
                        From Addresses In dt.AsEnumerable()
                        Where Addresses.RowState <> DataRowState.Added
                        Order By Addresses.Field(Of Date)("THRU_DATE", DataRowVersion.Original) Descending

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
                From Addresses In dt.AsEnumerable()
                Where Addresses.RowState = DataRowState.Added

            If AddedQuery.Any Then
                NewDR = AddedQuery.First
            End If

            'Terminate prior active row
            If _UIState = UIStates.Approving Then '' we are archiving other rows

                NewDR = DirectCast(bs.Current, DataRowView).Row

            End If

            If _UIState = UIStates.Deleting AndAlso CurrentDR.RowState = DataRowState.Deleted Then

                Dim ProposedCurrentQuery =
                            From ExistingCoverage In dt.AsEnumerable
                            Where ExistingCoverage.RowState <> DataRowState.Deleted
                            Order By ExistingCoverage.Field(Of Date)("THRU_DATE") Descending

                If ProposedCurrentQuery.Any Then
                    PriorDR = ProposedCurrentQuery.First

                    If PriorDR IsNot Nothing Then
                        PriorDR.BeginEdit()
                        PriorDR("THRU_DATE") = CDate("9999-12-31")
                        PriorDR.EndEdit()
                        ChangesMade = True
                    End If
                End If

            ElseIf NewDR IsNot Nothing AndAlso (CurrentDR IsNot Nothing AndAlso CurrentDR.RowState <> DataRowState.Deleted) Then

                If Not IsDBNull(NewDR("FROM_DATE")) Then
                    If CDate(CurrentDR("THRU_DATE")) <> CDate(NewDR("FROM_DATE")).AddDays(-1) Then
                        CurrentDR.BeginEdit()
                        CurrentDR("THRU_DATE") = CDate(NewDR("FROM_DATE")).AddDays(-1) 'this can go into the past
                        CurrentDR.EndEdit()
                        ChangesMade = True
                    End If
                End If

            ElseIf PriorDR IsNot Nothing AndAlso CurrentDR.RowState <> DataRowState.Deleted Then

                If CDate(CurrentDR("FROM_DATE")) = CDate(PriorDR("FROM_DATE", DataRowVersion.Original)) Then

                    PriorDR.Delete() 'prior row completely removed. This will change bindingsource content.
                    ChangesMade = True

                ElseIf CDate(PriorDR("THRU_DATE")) <> CDate(CurrentDR("FROM_DATE")).AddDays(-1) Then
                    'prior row is adjusted
                    PriorDR.BeginEdit()
                    PriorDR("THRU_DATE") = CDate(CurrentDR("FROM_DATE")).AddDays(-1)
                    PriorDR.EndEdit()

                    ChangesMade = True

                End If

                Dim RepositionQuery =
                                From ExistingCoverage In _MedCoverageDS.Tables("ELIG_COVERAGE")
                                Where ExistingCoverage.RowState = DataRowState.Deleted

                If RepositionQuery.Any Then
                    For Each DRV As DataRowView In _MedCoverageBS
                        If CDate(DRV("THRU_DATE")) = CDate("9999-12-31") AndAlso DRV.Row.RowState = DataRowState.Modified Then
                            _MedCoverageBS.Position = _MedCoverageBS.IndexOf(DRV)
                            MedCoverageDataGrid.Select(_MedCoverageBS.IndexOf(DRV))
                            Exit For
                        End If
                    Next
                End If

            End If

            If ChangesMade Then
                Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " BEE: BS(" & bs.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
            End If

            Return ChangesMade

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & bs.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Function

    Private Sub CancelChanges(ButtonName As String)

        Dim HoldState As UIStates = _UIState

        Dim BS As BindingSource

        Try

            BS = DirectCast(_MedCoverageBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If ButtonName = "MedCancelButton" Then

                SetMedUIElements(UIStates.Canceling)

                If UnCommittedMedChangesExist() = False Then

                    CancelMedChanges()

                Else

                    Dim Result As DialogResult = MessageBox.Show(Me, "Do you want to Cancel Medical changes?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                    If Result = DialogResult.Yes Then

                        CancelMedChanges()

                    Else
                        _UIState = HoldState
                    End If

                End If
            Else
                SetDenUIElements(UIStates.Canceling)

                If UnCommittedDenChangesExist() = False Then

                    CancelDenChanges()

                Else

                    Dim Result As DialogResult = MessageBox.Show(Me, "Do you want to Cancel Dental changes?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                    If Result = DialogResult.Yes Then

                        CancelDenChanges()

                    Else
                        _UIState = HoldState
                    End If

                End If
            End If


        Catch ex As Exception
            Throw
        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If ButtonName = "MedCancelButton" Then
                SetMedUIElements()
            Else
                SetDenUIElements()
            End If

        End Try

    End Sub

    Private Sub CancelButton_Click(sender As System.Object, e As System.EventArgs) Handles MedCancelButton.Click, DenCancelButton.Click

        Dim CButton As Button
        Dim HoldAutoValidateState As AutoValidate = Me.AutoValidate

        Dim BS As BindingSource

        Try

            BS = DirectCast(_MedCoverageBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from occuring when buttons are disabled

            CButton = CType(sender, Button)
            CButton.Enabled = False

            CancelChanges(CButton.Name)

        Catch ex As Exception
            Throw
        Finally

            Me.AutoValidate = HoldAutoValidateState

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

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

    Private Sub LoadCoverage()
        Dim DRs() As DataRow

        Try

            ClearErrors()
            ClearDataBindings(Me)

            LoadHMONetworksCombo()
            LoadMedCoverageValuesCombo()

            If _AllCoverageDS Is Nothing OrElse _AllCoverageDS.Tables.Count = 0 OrElse _AllCoverageDS.Tables("ELIG_COVERAGE").Rows.Count = 0 Then  '' only retrieve data for first time
                _AllCoverageDS = If(_REGMSupervisor, RegMasterDAL.RetrieveEligCoverageWithSplitByFamilyIDNoOverlap(_FamilyID, _AllCoverageDS), RegMasterDAL.RetrieveEligCoverageWithSplitByFamilyID(_FamilyID, _AllCoverageDS))
            End If

            If _FamilyHMONetworkHistoryDS Is Nothing OrElse _FamilyHMONetworkHistoryDS.Tables.Count < 1 OrElse _FamilyHMONetworkHistoryDS.Tables("HMO_NETWORK").Rows.Count < 1 Then
                _FamilyHMONetworkHistoryDS = RegMasterDAL.RetrieveHMONetworkInfo(_FamilyID, _FamilyHMONetworkHistoryDS)
            End If

            _MedCoverageDS = New DataSet
            _MedCoverageDS.Tables.Add(_AllCoverageDS.Tables("ELIG_COVERAGE").Clone)

            DRs = _AllCoverageDS.Tables("ELIG_COVERAGE").Select("COVERAGE_TYPE='MEDICAL'")
            For Each DR As DataRow In DRs
                _MedCoverageDS.Tables("ELIG_COVERAGE").ImportRow(DR)
            Next

            _MedCoverageDS.Tables("ELIG_COVERAGE").Columns.Add("FilterTHRU_DATE", Type.GetType("System.DateTime"))

            For Each DR As DataRow In _MedCoverageDS.Tables("ELIG_COVERAGE").Rows
                DR("FilterTHRU_DATE") = DR("THRU_DATE")
            Next

            _MedCoverageDS.Tables("ELIG_COVERAGE").AcceptChanges() 'remove modified row flag

            '_MemberBaseFilter = "RELATION_ID=" & _RelationID.ToString

            AddHandler _MedCoverageDS.Tables("ELIG_COVERAGE").RowChanging, AddressOf MedCoverageDS_RowChanging
            AddHandler _MedCoverageDS.Tables("ELIG_COVERAGE").RowChanged, AddressOf MedCoverageDS_RowChanged
            AddHandler _MedCoverageDS.Tables("ELIG_COVERAGE").ColumnChanging, AddressOf MedCoverageDS_ColumnChanging
            AddHandler _MedCoverageDS.Tables("ELIG_COVERAGE").ColumnChanged, AddressOf MedCoverageDS_ColumnChanged

            AddHandler _MedCoverageValuesDS.Tables("COVERAGE_VALUES").RowChanging, AddressOf DS_RowChanging
            AddHandler _MedCoverageValuesDS.Tables("COVERAGE_VALUES").RowChanged, AddressOf DS_RowChanged
            AddHandler _MedCoverageValuesDS.Tables("COVERAGE_VALUES").ColumnChanging, AddressOf DS_ColumnChanging
            AddHandler _MedCoverageValuesDS.Tables("COVERAGE_VALUES").ColumnChanged, AddressOf DS_ColumnChanged

            _MedCoverageBS = New BindingSource

            _MedCoverageBS.RaiseListChangedEvents = False
            _MedCoverageBS.DataSource = _MedCoverageDS.Tables("ELIG_COVERAGE")
            _MedCoverageBS.Sort = If(MedCoverageDataGrid.LastSortedBy, MedCoverageDataGrid.DefaultSort)
            _MedCoverageBS.RaiseListChangedEvents = True

            _MedCoverageBS.ResetBindings(False)

            MedCoverageDataGrid.DataSource = _MedCoverageBS
            MedCoverageDataGrid.SetTableStyle()

            LoadMedDataBindings()

            '' Dental

            LoadDenCoverageValuesCombo()

            _DenCoverageDS = New DataSet
            _DenCoverageDS.Tables.Add(_AllCoverageDS.Tables("ELIG_COVERAGE").Clone)

            DRs = _AllCoverageDS.Tables("ELIG_COVERAGE").Select("COVERAGE_TYPE='DENTAL'")
            For Each DR As DataRow In DRs
                _DenCoverageDS.Tables("ELIG_COVERAGE").ImportRow(DR)
            Next

            _DenCoverageDS.Tables("ELIG_COVERAGE").Columns.Add("FilterTHRU_DATE", Type.GetType("System.DateTime"))

            For Each DR As DataRow In _DenCoverageDS.Tables("ELIG_COVERAGE").Rows
                DR("FilterTHRU_DATE") = DR("THRU_DATE")
            Next

            _DenCoverageDS.Tables("ELIG_COVERAGE").AcceptChanges() 'remove modified row flag

            AddHandler _DenCoverageDS.Tables("ELIG_COVERAGE").RowChanging, AddressOf MedCoverageDS_RowChanging
            AddHandler _DenCoverageDS.Tables("ELIG_COVERAGE").RowChanged, AddressOf MedCoverageDS_RowChanged
            AddHandler _DenCoverageDS.Tables("ELIG_COVERAGE").ColumnChanging, AddressOf MedCoverageDS_ColumnChanging
            AddHandler _DenCoverageDS.Tables("ELIG_COVERAGE").ColumnChanged, AddressOf MedCoverageDS_ColumnChanged

            AddHandler _DenCoverageValuesDS.Tables("COVERAGE_VALUES").RowChanging, AddressOf DS_RowChanging
            AddHandler _DenCoverageValuesDS.Tables("COVERAGE_VALUES").RowChanged, AddressOf DS_RowChanged
            AddHandler _DenCoverageValuesDS.Tables("COVERAGE_VALUES").ColumnChanging, AddressOf DS_ColumnChanging
            AddHandler _DenCoverageValuesDS.Tables("COVERAGE_VALUES").ColumnChanged, AddressOf DS_ColumnChanged

            _DenCoverageBS = New BindingSource

            _DenCoverageBS.RaiseListChangedEvents = False
            _DenCoverageBS.DataSource = _DenCoverageDS.Tables("ELIG_COVERAGE")
            _DenCoverageBS.Sort = If(DenCoverageDataGrid.LastSortedBy, DenCoverageDataGrid.DefaultSort)
            _DenCoverageBS.RaiseListChangedEvents = True

            _DenCoverageBS.ResetBindings(False)

            DenCoverageDataGrid.DataSource = _DenCoverageBS
            DenCoverageDataGrid.SetTableStyle()

            LoadDenDataBindings()

            Me.Text = "Medical and Dental Coverage Election for Family (" & _FamilyID.ToString & ")"

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

    Private Sub ClearErrors()

        ClearError(ErrorProvider1, txtMedFromDate)
        ClearError(ErrorProvider1, cmbMedCoverage)
        ClearError(ErrorProvider1, cmbHMONetwork)
        ClearError(ErrorProvider1, cmbMedCoverage)
        ClearError(ErrorProvider1, cmbMedLetter)

        ClearError(ErrorProvider1, txtDenFromDate)
        ClearError(ErrorProvider1, cmbDenCoverageValues)
        ClearError(ErrorProvider1, cmbDenLetter)

    End Sub

    Public Sub ClearAll()
        'only called from outside the component

        ClearErrors()
        ClearDataBindings(Me)

        _UpdatedRecord = False

        _MedCoverageDS = New DataSet
        _DenCoverageDS = New DataSet

        _MedChangedDRs = Nothing
        _DenChangedDRs = Nothing

        _ReadOnlyMode = True
        _Zipcode = Nothing
        _SubscriberStatus = ""

    End Sub

    Private Shared Function OverlappingPeriods(ByVal period1_start As Date, ByVal period1_end As Date, ByVal period2_start As Date, ByVal period2_end As Date) As Boolean

        If Date.Compare(period1_start, period2_end) <= 0 And Date.Compare(period1_end, period2_start) >= 0 Then

            ''  MessageBox.Show("Overlapping of addresses is not valid." & Environment.NewLine & " Please correct the date before Continuing", "Overlapping", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return True

        End If

        Return False

    End Function

    Private Sub DateOnlyBinding_BindingComplete(ByVal sender As Object, ByVal e As BindingCompleteEventArgs)
        Try
            ClearError(ErrorProvider1, CType(e.Binding.BindableComponent, Control))

            If e.BindingCompleteState <> BindingCompleteState.Success Then
                ErrorProvider1.SetErrorWithTracking(CType(e.Binding.BindableComponent, Control), "Date format invalid. Use mmddyy or mmddyyyy")
            End If

        Catch ex As Exception


                Throw

        End Try
    End Sub

    Private Sub BindingCompleteEventHandler(ByVal sender As Object, ByVal e As System.Windows.Forms.BindingCompleteEventArgs)

        Dim ControlBinding As Binding
        Dim BS As BindingSource

        Try

            ControlBinding = CType(sender, Binding)
            BS = DirectCast(_MedCoverageBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " : (" & ControlBinding.Control.Name & ") " & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If Not e.BindingCompleteState = BindingCompleteState.Success Then
                MessageBox.Show("Control " & e.Binding.Control.Name & " " & e.ErrorText, "Problem converting data to database format", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out:  BS(" & BS.Position.ToString & ") " & Me.Name & " : (" & ControlBinding.Control.Name & ") " & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub SetMedUIElements(Optional uiState As UIStates = UIStates.AsIs)

        Dim BS As BindingSource

        Dim ControlIsReadOnly As UIStates = UIStates.None Or UIStates.NotModifiable Or UIStates.Modifiable

        Dim CM As CurrencyManager
        Dim DR As DataRow

        Dim HoldAutoValidateState As AutoValidate
        Dim SavedMedCoverageBSRaiseListChangedEvents As Boolean

        Try

            BS = DirectCast(_MedCoverageBS, BindingSource)

            HoldAutoValidateState = Me.AutoValidate

            Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable 'prevent validation from firing during control manipulation

            If Not uiState.HasFlag(UIStates.AsIs) Then _UIState = uiState

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") State:" & _UIState.ToString & " " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ProcessControls(CType(grpMedEditPanel, Object), True) 'does not affect visibility only edit yes/no

            Me.MedDeleteButton.Enabled = False
            Me.MedDeleteButton.Visible = False

            Me.MedModifyButton.Enabled = False
            Me.MedModifyButton.Visible = False

            Me.MedSaveButton.Enabled = False
            Me.MedSaveButton.Visible = False

            Me.MedCancelButton.Enabled = False
            Me.MedCancelButton.Visible = False

            Me.MedAddButton.Enabled = False
            Me.MedAddButton.Visible = False

            Me.MedHMONetworkButton.Enabled = False
            Me.MedHMONetworkButton.Visible = False

            cmbHMONetwork.Visible = False
            LabelNetwork.Visible = False

            If _UIState = UIStates.None Then Return

            If Not _UIState = UIStates.None AndAlso (_ViewHistory Is Nothing OrElse _ViewHistory) Then
                MedHistoryButton.Visible = True
                MedHistoryButton.Enabled = True
            End If

            CM = _MedCoverageBS.CurrencyManager

            If SavedMedCoverageBSRaiseListChangedEvents Then
                _MedCoverageBS.RaiseListChangedEvents = False
            End If

            If _UIState <> UIStates.Canceling AndAlso CM IsNot Nothing AndAlso CM.Count > 0 AndAlso CM.Position > -1 Then
                DR = DirectCast(CM.Current, DataRowView).Row

                If _HMONetworkValuesBS.Filter IsNot Nothing AndAlso _HMONetworkValuesBS.Filter.Trim.Length > 0 AndAlso _HMONetworkValuesBS.Count > 0 Then 'selected Coverage Code / Medical Plan has associated networks for the time period
                    cmbHMONetwork.Visible = True 'activates binding
                    LabelNetwork.Visible = True

                End If

            End If

            lblUIMessaging.Text = ""

            Select Case _UIState 'this block resets state for remainder of control processing

                Case UIStates.Canceled

                    lblUIMessaging.Text = "Change(s) Cancelled."
                    lblUIMessaging.Visible = True

                    _UIState = If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable)

                Case UIStates.Saved

                    lblUIMessaging.Text = "Change(s) Saved."
                    lblUIMessaging.Visible = True

                    _UIState = If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable)

                Case UIStates.Rejected

                    lblUIMessaging.Text = "Coverage Rejected."
                    lblUIMessaging.Visible = True

                    _UIState = If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable)

                Case UIStates.Approved

                    lblUIMessaging.Text = "Coverage Approved."
                    lblUIMessaging.Visible = True

                    _UIState = If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable)

                Case UIStates.Deleted 'not implemented

                    lblUIMessaging.Text = "Coverage Deleted."
                    lblUIMessaging.Visible = True

                    _UIState = If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable)

                Case UIStates.Archived 'not implemented

                    lblUIMessaging.Text = "Coverage Archived."
                    lblUIMessaging.Visible = True

                    _UIState = If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable)
            End Select

            Select Case _UIState
                Case UIStates.None

                    Return

                Case UIStates.Rejecting

                Case UIStates.Saving

                    ProcessControls(CType(grpMedEditPanel, Object), False, True) 'allow controls to validate

                Case UIStates.Canceling

                Case UIStates.Viewing

                Case UIStates.NotModifiable

                Case UIStates.Modifiable, UIStates.Adding, UIStates.Modifying

                    Me.MedAddButton.Visible = True
                    Me.MedModifyButton.Visible = True
                    Me.MedHMONetworkButton.Visible = True
                    Me.MedSaveButton.Visible = True
                    Me.MedCancelButton.Visible = True

                    Select Case _UIState
                        Case UIStates.Modifiable

                            If DR IsNot Nothing Then

                                'only most recent row can be modified or deleted.

                                If _REGMRegMasterDeleteAccess Then Me.MedDeleteButton.Visible = True

                                If Not IsDBNull(DR("FROM_DATE")) Then
                                    If _REGMReports Then
                                        Me.MedDeleteButton.Enabled = True
                                        Me.MedModifyButton.Enabled = True
                                    ElseIf (DateDiff(DateInterval.Month, Now, CDate(DR("FROM_DATE"))) > -12 AndAlso CDate(DR("THRU_DATE")).Year = 9999) Then
                                        If _REGMRegMasterDeleteAccess Then Me.MedDeleteButton.Enabled = True
                                        Me.MedModifyButton.Enabled = True
                                    End If
                                End If

                            End If

                            If _MedCoverageDS.Tables("ELIG_COVERAGE") IsNot Nothing Then
                                Dim DRs As DataRow() = _MedCoverageDS.Tables("ELIG_COVERAGE").Select("THRU_DATE = #12/31/9999#")
                                If DRs.Length < 1 Then
                                    Me.MedAddButton.Enabled = True
                                Else
                                    If CDate(DRs(0)("FROM_DATE")) < Date.Now Then Me.MedAddButton.Enabled = True
                                End If
                            End If

                        Case UIStates.Adding, UIStates.Modifying

                            If DR IsNot Nothing AndAlso CDate(DR("THRU_DATE")).Year = 9999 AndAlso (DR.HasVersion(DataRowVersion.Proposed) OrElse DR.RowState = DataRowState.Modified OrElse DR.RowState = DataRowState.Added) Then 'in addition to current/future active addresses, items awaiting approval will also have 9999 thru date 

                                ProcessControls(CType(grpMedEditPanel, Object), False, True)

                                If _HMONetworkValuesBS.Filter IsNot Nothing AndAlso _HMONetworkValuesBS.Filter.Trim.Length > 0 AndAlso _HMONetworkValuesBS.Count > 0 Then 'selected Coverage Code / Medical Plan has associated networks for the time period
                                    cmbHMONetwork.Visible = True 'activates binding
                                    LabelNetwork.Visible = True
                                    cmbHMONetwork.Enabled = True

                                    If _UIState = UIStates.Adding AndAlso DR.RowState = DataRowState.Added Then
                                        cmbHMONetwork.ResetText()
                                    End If

                                End If

                            Else
                                ClearErrors()
                            End If

                            Me.MedCancelButton.Enabled = True

                    End Select

                    If DR IsNot Nothing Then

                        If Not IsDBNull(DR("COVERAGE_CODE")) Then
                            Select Case CInt(DR("COVERAGE_CODE"))
                                Case 40, 41, 80, 81
                                    MedHMONetworkButton.Enabled = True
                            End Select
                        End If

                    End If

                Case UIStates.Deleting

                    If CM.Position < 0 Then
                        cmbHMONetwork.SelectedIndex = -1
                        cmbMedCoverage.SelectedIndex = -1
                    End If

                    Me.MedSaveButton.Visible = True
                    Me.MedCancelButton.Visible = True

                    lblUIMessaging.Text = "Delete Pending."
                    lblUIMessaging.Visible = True

                Case UIStates.Canceling

                    _MedCoverageValuesComboBS.RemoveFilter()
                    _HMONetworkValuesBS.RemoveFilter()

            End Select

        Catch ex As Exception
            Throw
        Finally

            txtMedThruDate.ReadOnly = True

            If txtMedFromDate.ReadOnly = True Then
                ClearErrors()
            End If

            If _UIState <> UIStates.Canceling Then
                _MedChangedDRs = _MedCoverageDS.GetChanges(DataRowState.Added Or DataRowState.Modified Or DataRowState.Deleted)
                If _MedChangedDRs IsNot Nothing AndAlso _MedChangedDRs.Tables("ELIG_COVERAGE").Rows.Count > 0 Then
                    Me.MedSaveButton.Enabled = True
                    Me.MedCancelButton.Enabled = True
                End If
            End If

            Me.AutoValidate = HoldAutoValidateState

            If SavedMedCoverageBSRaiseListChangedEvents Then
                _MedCoverageBS.RaiseListChangedEvents = True
            End If

            grpMedEditPanel.Refresh()

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out:  BS(" & BS.Position.ToString & ") State:" & _UIState.ToString & " " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

    Private Sub cmbMedCoverage_Validating(sender As Object, e As CancelEventArgs) Handles cmbMedCoverage.Validating

        Dim CBox As ExComboBox = CType(sender, ExComboBox)
        Dim BS As BindingSource
        Dim PBS As BindingSource
        Dim DR As DataRow

        Try

            If _Disposed OrElse _MedCoverageBS Is Nothing OrElse _MedCoverageBS.Position < 0 OrElse CBox.ReadOnly Then Return

            BS = _MedCoverageBS
            PBS = _MedCoverageValuesComboBS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
            DR = CType(_MedCoverageBS.Current, DataRowView).Row

            ClearError(ErrorProvider1, CBox)

            If Not (_UIState And UIStates.[ReadOnly]) = _UIState Then

                If CBox.SelectedIndex < 0 Then
                    ErrorProvider1.SetErrorWithTracking(CBox, " Coverage selection required.")
                Else
                    Select Case CBox.Name
                        Case "cmbMedCoverageValues"
                            Dim MedCoverageQuery =
                                From AssignableCoverage In CType(_MedCoverageValuesComboBS.DataSource, DataTable).AsEnumerable()
                                Where (Not IsDBNull(DR("FROM_DATE")) AndAlso (CDate(DR("FROM_DATE")) >= AssignableCoverage.Field(Of Date)("FROM_DATE") _
                                                                     AndAlso CDate(DR("FROM_DATE")) <= AssignableCoverage.Field(Of Date)("THRU_DATE"))) _
                                AndAlso cmbMedCoverage.SelectedValue.ToString = AssignableCoverage.Field(Of Int16)("COVERAGE_VALUE").ToString

                            If Not MedCoverageQuery.Any Then
                                ErrorProvider1.SetErrorWithTracking(CBox, " Coverage invalid for FROM date specified.")
                            End If


                            If cmbMedCoverage.SelectedValue.ToString = "81" Then
                                If CDate(DR("FROM_DATE")) < CDate(CType(cmbMedCoverage.SelectedItem, DataRowView)("FROM_DATE")) Then 'Anthem Medicare is only available for those with prior Anthem non Medicare coverage (e.g 80)
                                    Dim AnthemMedicareQuery =
                                        From AssignableCoverage In CType(_MedCoverageBS.DataSource, DataTable).AsEnumerable()
                                        Where AssignableCoverage.Field(Of Int32?)("COVERAGE_CODE") = 80 _
                                        And AssignableCoverage.Field(Of Date)("FROM_DATE") <= Date.Now.AddMonths(-1)

                                    If Not AnthemMedicareQuery.Any Then
                                        ErrorProvider1.SetErrorWithTracking(CBox, " Coverage only valid for prior Anthem non medicare enrollees.")
                                    End If
                                End If

                            End If

                    End Select

                End If
            End If

            If ErrorProvider1.GetError(CBox).Trim.Length > 0 Then 'are there any errors  
                e.Cancel = True
            Else
                ' Add
                ' Current will exist with a value of null until a item has been selected and validated
                ' Proposed will not exist until beginedit and will cease to exist when Endedit is called
                ' Original will not exist until Save (AcceptChanges) is called
                If DR("COVERAGE_CODE").ToString <> CBox.SelectedValue.ToString Then

                    '                    CType(_MedCoverageBS.Current, DataRowView).BeginEdit()
                    DR.BeginEdit() 'indicate push to datasource is expected.

                    Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " BE:  BS(" & _MedCoverageBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

                End If
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally

        End Try

    End Sub

    Private Sub cmbMedCoverage_Validated(sender As Object, e As EventArgs) Handles cmbMedCoverage.Validated

        Dim CBox As ExComboBox = CType(sender, ExComboBox)
        Dim BS As BindingSource
        Dim PBS As BindingSource

        Try

            If _Disposed OrElse _MedCoverageBS Is Nothing OrElse _MedCoverageBS.Position < 0 OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

            BS = _MedCoverageBS
            PBS = _MedCoverageValuesComboBS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Dim DR As DataRow = DirectCast(_MedCoverageBS.Current, DataRowView).Row


            ' During adding
            '   Proposed will not exist until BeginEdit
            '   SelectedValue will not have a value in SelectedIndex > -1
            '   Proposed will equal Default until EndEdit occurs
            '   Current will become populated once EndEdit occurs and will equal Default

            '   Original will not exist until changes are saved

            ' During modification
            '   Original will exist
            '   Proposed will not exist until new value is selected or BeginEdit
            '   Current will difer from original become populated once EndEdit occurs


            If DR.HasVersion(DataRowVersion.Proposed) AndAlso (DR("COVERAGE_CODE", DataRowVersion.Proposed).ToString <> DR("COVERAGE_CODE", DataRowVersion.Current).ToString OrElse DR("COVERAGE_CODE", DataRowVersion.Proposed).ToString <> cmbMedCoverage.SelectedValue.ToString) Then
                Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " EE: BS(" & _MedCoverageBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
                DR.EndEdit()
            End If

            'If DR("COVERAGE_CODE").ToString <> CBox.SelectedValue.ToString Then

            '    Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Mid: BS(" & _MedCoverageBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            '    CBox.DataBindings("SelectedValue").WriteValue()
            '    'the act of changing the coverage value resets any network selection
            '    cmbHMONetwork.SelectedIndex = -1
            '    _MedCoverageBS.EndEdit() 'triggers uisetup
            'End If

            'when an plan allows for Medicare / Non Medicare family members.
            If cmbHMONetwork.SelectedIndex < 0 AndAlso CType(_HMONetworkValuesBS.DataSource, DataTable).Select("MEDICAL_PLAN = " & cmbMedCoverage.SelectedValue.ToString & " AND SPLIT_MEDICARE_ONLY_SW = 1").ToArray.Any Then
                If _AllCoverageDS.Tables("SPLIT_COVERAGE").Rows.Count = 1 Then
                    If CInt(_AllCoverageDS.Tables("SPLIT_COVERAGE").Rows(0)("NON_MEDICARE_COUNT")) = 0 Then
                        cmbHMONetwork.SelectedValue = "NA" 'No Split family members so NA should be selected
                    End If
                End If
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally

        End Try

    End Sub


    Private Sub cmbHMONetwork_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbHMONetwork.SelectionChangeCommitted
        'called only when user interaction causes change to dropdown

        Dim CBox As ExComboBox
        Dim BS As BindingSource
        Dim PBS As BindingSource
        Dim NBS As BindingSource

        Try
            CBox = DirectCast(sender, ExComboBox)
            BS = _MedCoverageBS
            PBS = _MedCoverageValuesComboBS
            NBS = _HMONetworkValuesBS

            If _Disposed OrElse _MedCoverageBS Is Nothing OrElse _MedCoverageBS.Position < 0 OrElse _MedCoverageBS.Current Is Nothing OrElse CBox.ReadOnly Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") NBS(" & NBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ClearError(ErrorProvider1, CBox)

            If cmbHMONetwork.SelectedIndex > -1 Then
                If Not IsDBNull(CType(_MedCoverageBS.Current, DataRowView)("HMO_NETWORK")) Then
                    If cmbHMONetwork.SelectedValue.ToString <> CType(_MedCoverageBS.Current, DataRowView)("HMO_NETWORK").ToString Then
                        CType(_MedCoverageBS.Current, DataRowView).Row("HMO_NETWORK") = cmbHMONetwork.SelectedValue
                    End If
                Else
                    CType(_MedCoverageBS.Current, DataRowView).Row("HMO_NETWORK") = cmbHMONetwork.SelectedValue
                End If
            Else
                If Not IsDBNull(CType(_MedCoverageBS.Current, DataRowView)("HMO_NETWORK").ToString) Then
                    CType(_MedCoverageBS.Current, DataRowView).Row("HMO_NETWORK") = DBNull.Value
                End If
            End If

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") NBS(" & NBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

    Private Sub cmbHMONetwork_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbHMONetwork.SelectedIndexChanged

        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Try

            If _Disposed OrElse _MedCoverageBS Is Nothing OrElse _MedCoverageBS.Position < 0 OrElse _MedCoverageBS.Current Is Nothing OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _MedCoverageBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Dim DR As DataRow = DirectCast(_MedCoverageBS.Current, DataRowView).Row

            If Not (_UIState And UIStates.[ReadOnly]) = _UIState Then
                If DR IsNot Nothing AndAlso CDate(DR("THRU_DATE")).Year = 9999 AndAlso (DR.HasVersion(DataRowVersion.Proposed) OrElse (DR.RowState = DataRowState.Modified OrElse DR.RowState = DataRowState.Added)) Then 'in addition to current/future active addresses, items awaiting approval will also have 9999 thru date 

                    CType(CBox.Parent, TransparentContainer).ValidateChildren() 'this will trigger validation of the cmbbox triggering write of value to DS

                End If
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _MedCoverageBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally

        End Try

    End Sub

    Private Sub MedHistoryButton_Click(sender As Object, e As EventArgs) Handles MedHistoryButton.Click
        Dim FRM As RegMasterHistoryForm

        Try

            FRM = New RegMasterHistoryForm
            FRM.FamilyID = _FamilyID
            FRM.RelationID = _RelationID
            FRM.Mode = REGMasterHistoryMode.MedicalCoverage
            FRM.ShowDialog()

            FRM.Close()

        Catch ex As Exception
            Throw
        Finally
            FRM.Dispose()
            FRM = Nothing
        End Try

    End Sub

    Private Sub MedDeleteButton_Click(sender As Object, e As EventArgs) Handles MedDeleteButton.Click

        Try

            DeleteMedCoverageLine()

        Catch ex As Exception


                Throw

        Finally
        End Try

    End Sub

    Private Sub MedCoverageValuesComboBS_CurrentItemChanged(sender As Object, e As EventArgs) Handles _MedCoverageValuesComboBS.CurrentItemChanged

        Dim BS As BindingSource

        Try

            BS = CType(sender, BindingSource)

            If _MedCoverageBS Is Nothing OrElse _MedCoverageBS.Position < 0 OrElse _MedCoverageBS.Current Is Nothing OrElse _MedCoverageBS.Count < 1 Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If _MedCoverageValuesComboBS IsNot Nothing AndAlso _MedCoverageValuesComboBS.Position > -1 AndAlso _MedCoverageValuesComboBS.Current IsNot Nothing AndAlso _MedCoverageValuesComboBS.Count > 0 Then

                Dim DR As DataRow = DirectCast(_MedCoverageBS.Current, DataRowView).Row

                If DR IsNot Nothing Then

                    If FilterAndSelectHMONetworkValues() Then
                        '    'filter changes occured
                    End If 'This is not limited to modifications as descriptions of code values have changed with time.              

                    'SetMedUIElements()

                End If
            End If

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub MedCoverageValuesComboBS_CurrentChanged(sender As Object, e As EventArgs) Handles _MedCoverageValuesComboBS.CurrentChanged

        Dim BS As BindingSource

        Try

            BS = CType(sender, BindingSource)

            If _MedCoverageBS Is Nothing OrElse _MedCoverageBS.Position < 0 OrElse _MedCoverageBS.Current Is Nothing OrElse _MedCoverageBS.Count < 1 Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If _MedCoverageValuesComboBS IsNot Nothing AndAlso _MedCoverageValuesComboBS.Position > -1 AndAlso _MedCoverageValuesComboBS.Current IsNot Nothing AndAlso _MedCoverageValuesComboBS.Count > 0 Then

                Dim DR As DataRow = DirectCast(_MedCoverageBS.Current, DataRowView).Row

                If DR IsNot Nothing Then

                    'If FilterAndSelectHMONetworkValues() Then
                    '    'filter changes occured
                    '    SetMedUIElements()
                    'End If 'This is not limited to modifications as descriptions of code values have changed with time.              

                End If
            End If

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub MedCoverageBS_CurrentItemChanged(sender As Object, e As EventArgs) Handles _MedCoverageBS.CurrentItemChanged
        'Called after CurrentChanged and one of the properties of Current is changed
        Dim BS As BindingSource

        Try


            BS = DirectCast(sender, BindingSource)

            If BS Is Nothing OrElse BS.Position < 0 OrElse BS.Current Is Nothing OrElse BS.Count < 1 Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Dim DR As DataRow = DirectCast(BS.Current, DataRowView).Row

            If Not (_UIState And UIStates.[ReadOnly]) = _UIState Then

                If Not (_UIState And UIStates.[ReadOnly]) = _UIState Then
                    If DR IsNot Nothing AndAlso CDate(DR("THRU_DATE")).Year = 9999 AndAlso (DR.HasVersion(DataRowVersion.Proposed) OrElse (DR.RowState = DataRowState.Modified OrElse DR.RowState = DataRowState.Added)) Then 'in addition to current/future active addresses, items awaiting approval will also have 9999 thru date 

                    End If
                End If

            End If

            FilterAndSelectMedCoverageValues(BS)

            SetMedUIElements()

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally

        End Try
    End Sub

    Private Sub MedCoverageBS_CurrentChanged(sender As Object, e As EventArgs) Handles _MedCoverageBS.CurrentChanged

        Dim BS As BindingSource

        Try

            BS = DirectCast(sender, BindingSource) 'always use BindingSource through event

            If BS Is Nothing OrElse BS.Position < 0 OrElse BS.Current Is Nothing OrElse BS.Count < 1 Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            FilterAndSelectMedCoverageValues(BS)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub cmbMedLetter_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbMedLetter.SelectionChangeCommitted
        Dim DR As DataRow
        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Try

            If _MedCoverageBS Is Nothing OrElse _MedCoverageBS.Position < 0 OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _MedCoverageBS.Position.ToString & ") SI(" & CBox.Text & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            CType(CBox.Parent, TransparentContainer).ValidateChildren() 'this will trigger validation of the cmbbox triggering write of value to DS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _MedCoverageBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally

        End Try
    End Sub

    Private Sub MedCoverageDS_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs)
        Dim BS As BindingSource

        Try

            BS = DirectCast(_MedCoverageBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " : " & e.Row(e.Column).ToString & "\" & If(e.ProposedValue Is Nothing, "NULL", e.ProposedValue.ToString) & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " : " & e.Row(e.Column).ToString & "\" & If(e.ProposedValue Is Nothing, "NULL", e.ProposedValue.ToString) & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub MedCoverageDS_ColumnChanged(sender As Object, e As DataColumnChangeEventArgs)
        Dim BS As BindingSource

        Try

            BS = DirectCast(_MedCoverageBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If BS Is Nothing OrElse BS.Position < 0 OrElse BS.Current Is Nothing OrElse BS.Count < 1 Then Return

            If Not (_UIState And UIStates.[ReadOnly]) = _UIState Then

                Dim DR As DataRow = e.Row

                If _UIState <> UIStates.Canceling Then

                    _MedChangedDRs = _MedCoverageDS.GetChanges(DataRowState.Added Or DataRowState.Modified Or DataRowState.Deleted)

                    If DR.HasVersion(DataRowVersion.Proposed) OrElse (_MedChangedDRs IsNot Nothing AndAlso _MedChangedDRs.Tables("ELIG_COVERAGE").Rows.Count > 0) Then
                        Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Mi1:  BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
                        Me.MedSaveButton.Enabled = True
                        Me.MedCancelButton.Enabled = True

                        If e.Column.ColumnName.Contains("FROM_DATE") Then FilterAndSelectMedCoverageValues(BS)

                    End If

                End If
            End If

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub MedCoverageDS_RowChanging(sender As Object, e As DataRowChangeEventArgs)

        Dim BS As BindingSource

        Try

            BS = DirectCast(_MedCoverageBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & e.Action.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & e.Action.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

    Private Sub MedCoverageDS_RowChanged(sender As Object, e As DataRowChangeEventArgs)
        Dim BS As BindingSource

        Try

            BS = DirectCast(_MedCoverageBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " :  " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If FilterAndSelectHMONetworkValues() Then
                'filter changes occured
            End If 'This is not limited to modifications as descriptions of code values have changed with time.              

            SetMedUIElements()

        Catch ex As Exception

                Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " :  " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub DS_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs)
        Dim BS As BindingSource

        Try

            BS = DirectCast(_MedCoverageValuesComboBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " : " & e.Row(e.Column).ToString & "\" & If(e.ProposedValue Is Nothing, "NULL", e.ProposedValue.ToString) & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " : " & e.Row(e.Column).ToString & "\" & If(e.ProposedValue Is Nothing, "NULL", e.ProposedValue.ToString) & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub DS_ColumnChanged(sender As Object, e As DataColumnChangeEventArgs)
        Dim BS As BindingSource

        Try

            BS = DirectCast(_MedCoverageValuesComboBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If Not (_UIState And UIStates.[ReadOnly]) = _UIState Then

                If _MedCoverageValuesComboBS IsNot Nothing AndAlso _MedCoverageValuesComboBS.Position > -1 AndAlso _MedCoverageValuesComboBS.Current IsNot Nothing AndAlso _MedCoverageValuesComboBS.Count > 0 Then

                    Dim DR As DataRow = e.Row

                End If

            End If

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub DS_RowChanging(sender As Object, e As DataRowChangeEventArgs)

        Dim BS As BindingSource

        Try

            BS = DirectCast(_MedCoverageValuesComboBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & e.Action.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & e.Action.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

    Private Sub DS_RowChanged(sender As Object, e As DataRowChangeEventArgs)
        Dim BS As BindingSource

        Try

            BS = DirectCast(_MedCoverageValuesComboBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " :  " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " :  " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub cmbHMONetwork_DropDown(sender As Object, e As EventArgs) Handles cmbHMONetwork.DropDown

        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _HMONetworkValuesBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If CBox.Enabled = False OrElse CBox.ReadOnly Then Return

            If _HMONetworkValuesBS.Position > -1 Then

                Dim DR As DataRow = DirectCast(_HMONetworkValuesBS.Current, DataRowView).Row

                If Not (_UIState And UIStates.[ReadOnly]) = _UIState Then
                End If

            End If

        Catch ex As Exception

                Throw
        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _HMONetworkValuesBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

    Private Sub cmbHMONetwork_Validating(sender As Object, e As CancelEventArgs) Handles cmbHMONetwork.Validating
        Dim CBox As ExComboBox
        Dim BS As BindingSource
        Dim PBS As BindingSource
        Dim NBS As BindingSource
        Dim DR As DataRow

        Try
            CBox = DirectCast(sender, ExComboBox)
            BS = _MedCoverageBS
            PBS = _MedCoverageValuesComboBS
            NBS = _HMONetworkValuesBS

            If _Disposed OrElse _MedCoverageBS Is Nothing OrElse _MedCoverageBS.Position < 0 OrElse _MedCoverageBS.Current Is Nothing OrElse CBox.ReadOnly Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In: BS(" & BS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = CType(_MedCoverageBS.Current, DataRowView).Row

            ClearError(ErrorProvider1, CBox)

            If Not CType(_HMONetworkValuesBS.DataSource, DataTable).Select("SPLIT_MEDICARE_ONLY_SW = 0").ToArray.Any Then
                If CBox.SelectedIndex < 0 Then
                    ErrorProvider1.SetErrorWithTracking(CBox, " Network selection required.")
                End If
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally

        End Try
    End Sub

    Private Sub cmbHMONetwork_Validated(sender As Object, e As EventArgs) Handles cmbHMONetwork.Validated

        Dim CBox As ExComboBox

        Dim BS As BindingSource
        Dim PBS As BindingSource
        Dim NBS As BindingSource
        Dim DR As DataRow

        Try

            CBox = DirectCast(sender, ExComboBox)
            BS = _MedCoverageBS
            PBS = _MedCoverageValuesComboBS
            NBS = _HMONetworkValuesBS

            If _Disposed OrElse _MedCoverageBS Is Nothing OrElse _MedCoverageBS.Position < 0 OrElse _MedCoverageBS.Current Is Nothing OrElse CBox.ReadOnly Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") NBS(" & NBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = CType(_MedCoverageBS.Current, DataRowView).Row

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") NBS(" & NBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally

        End Try
    End Sub

    Private Sub cmbDenCoverageValues_SelectedValueChanged(sender As Object, e As EventArgs) Handles cmbDenCoverageValues.SelectedValueChanged

        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _DenCoverageBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _DenCoverageBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

    Private Sub cmbMedCoverage_SelectedValueChanged(sender As Object, e As EventArgs) Handles cmbMedCoverage.SelectedValueChanged
        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _MedCoverageBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _MedCoverageBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

    Private Sub cmbHMONetwork_SelectedValueChanged(sender As Object, e As EventArgs) Handles cmbHMONetwork.SelectedValueChanged
        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _MedCoverageBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If ErrorProvider1.GetError(Me.cmbMedCoverage) = " Select a valid HMO Network" Then
                ClearError(ErrorProvider1, Me.cmbMedCoverage)
            End If

        Catch ex As Exception

                Throw
        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _MedCoverageBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

    Private Sub ComboOnlyBinding_Format(sender As Object, e As ConvertEventArgs)
        Dim CBox As ExComboBox
        Dim ComboBinding As Binding
        Dim CM As CurrencyManager
        Dim DR As DataRow

        Try

            ComboBinding = CType(sender, Binding)
            CM = CType(ComboBinding.BindingManagerBase, CurrencyManager)
            DR = CType(CM.Current, DataRowView).Row

            CBox = CType(ComboBinding.Control, ExComboBox)

            Debug.Print("ComboOnlyBinding_Parse: (In) " & CBox.Name & " : " & If(e.Value Is Nothing, "Nothing", If(IsDBNull(e.Value), "Null", e.Value.ToString)))

        Catch ex As Exception
            Throw
        Finally
            Debug.Print("ComboOnlyBinding_Parse: (Out) " & CBox.Name & " : " & If(e.Value Is Nothing, "Nothing", If(IsDBNull(e.Value), "Null", e.Value.ToString)))

        End Try

    End Sub

    Private Sub CoverageValuesComboBS_OnListChanged(sender As Object, e As System.ComponentModel.ListChangedEventArgs) Handles _MedCoverageValuesComboBS.ListChanged, _DenCoverageValuesComboBS.ListChanged

        Dim BS As BindingSource
        Try

            BS = CType(sender, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In: RS(" & If(BS.Position < 0 OrElse BS.Current Is Nothing, "N/A", DirectCast(BS.Current, System.Data.DataRowView).Row.RowState.ToString) & ") BS(" & BS.Position.ToString & ") O(" & e.OldIndex.ToString & ") N(" & e.NewIndex.ToString & ") CT(" & e.ListChangedType.ToString & ") ST(" & _UIState.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If BS.Current Is Nothing Then Return

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: RS(" & If(BS.Position < 0 OrElse BS.Current Is Nothing, "N/A", DirectCast(BS.Current, System.Data.DataRowView).Row.RowState.ToString) & ") BS(" & BS.Position.ToString & ") O(" & e.OldIndex.ToString & ") N(" & e.NewIndex.ToString & ") CT(" & e.ListChangedType.ToString & ") ST(" & _UIState.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub cmb_DenBindingContextChanged(sender As Object, e As EventArgs) Handles cmbDenCoverageValues.BindingContextChanged

        Dim CBox As ComboBox
        Dim BS As BindingSource
        Dim PBS As BindingSource

        Try
            CBox = DirectCast(sender, ComboBox)
            BS = _DenCoverageBS
            PBS = _DenCoverageValuesComboBS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))


        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try
    End Sub
    Private Sub cmb_MedBindingContextChanged(sender As Object, e As EventArgs) Handles cmbMedCoverage.BindingContextChanged, cmbHMONetwork.BindingContextChanged

        Dim CBox As ComboBox
        Dim BS As BindingSource
        Dim PBS As BindingSource

        Try
            CBox = DirectCast(sender, ComboBox)
            BS = _MedCoverageBS
            PBS = _MedCoverageValuesComboBS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))


        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try
    End Sub


    Private Sub cmbDenCoverageValues_DataSourceChanged(sender As Object, e As EventArgs) Handles cmbDenCoverageValues.DataSourceChanged

        Dim CBox As ComboBox
        Dim BS As BindingSource
        Dim PBS As BindingSource

        Try
            CBox = DirectCast(sender, ComboBox)
            BS = _DenCoverageBS
            PBS = _DenCoverageValuesComboBS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))


        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try
    End Sub

    Private Sub cmbMedCoverage_DataSourceChanged(sender As Object, e As EventArgs) Handles cmbMedCoverage.DataSourceChanged

        Dim CBox As ComboBox
        Dim BS As BindingSource
        Dim PBS As BindingSource

        Try
            CBox = DirectCast(sender, ComboBox)
            BS = _MedCoverageBS
            PBS = _MedCoverageValuesComboBS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))


        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try
    End Sub

    Private Sub cmbMedCoverage_Format(sender As Object, e As ListControlConvertEventArgs) 'Handles cmbMedCoverageValues.Format

        Dim CBox As ComboBox
        Dim BS As BindingSource
        Dim PBS As BindingSource

        Try
            CBox = DirectCast(sender, ComboBox)
            BS = _MedCoverageBS
            PBS = _MedCoverageValuesComboBS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))


        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try
    End Sub

    Private Sub cmbMedCoverage_FormatStringChanged(sender As Object, e As EventArgs) 'Handles cmbMedCoverageValues.FormatStringChanged

        Dim CBox As ComboBox
        Dim BS As BindingSource
        Dim PBS As BindingSource

        Try
            CBox = DirectCast(sender, ComboBox)
            BS = _MedCoverageBS
            PBS = _MedCoverageValuesComboBS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try
    End Sub

    Private Sub cmbMedCoverage_FormatInfoChanged(sender As Object, e As EventArgs) 'Handles cmbMedCoverageValues.FormatInfoChanged

        Dim CBox As ComboBox
        Dim BS As BindingSource
        Dim PBS As BindingSource

        Try
            CBox = DirectCast(sender, ComboBox)
            BS = _MedCoverageBS
            PBS = _MedCoverageValuesComboBS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))


        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try
    End Sub

    Private Sub cmbDenCoverageValues_VisibleChanged(sender As Object, e As EventArgs) Handles cmbDenCoverageValues.VisibleChanged

        Dim CBox As ComboBox
        Dim BS As BindingSource
        Dim PBS As BindingSource

        Try
            CBox = DirectCast(sender, ComboBox)
            BS = _DenCoverageBS
            PBS = _DenCoverageValuesComboBS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))


        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try
    End Sub

    Private Sub cmbMedCoverage_VisibleChanged(sender As Object, e As EventArgs) Handles cmbMedCoverage.VisibleChanged

        Dim CBox As ComboBox
        Dim BS As BindingSource
        Dim PBS As BindingSource

        Try
            CBox = DirectCast(sender, ComboBox)
            BS = _MedCoverageBS
            PBS = _MedCoverageValuesComboBS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))


        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") PBS(" & PBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try
    End Sub

    Private Sub CoverageBS_PositionChanged(sender As Object, e As EventArgs) Handles _MedCoverageBS.PositionChanged, _DenCoverageBS.PositionChanged

        Dim BS As BindingSource

        Try

            BS = CType(sender, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub
    Private Sub CoverageValuesComboBS_DataError(sender As Object, e As BindingManagerDataErrorEventArgs) Handles _MedCoverageValuesComboBS.DataError, _DenCoverageValuesComboBS.DataError
        Dim BS As BindingSource

        Try

            BS = CType(sender, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

    Private Sub CoverageValuesComboBS_PositionChanged(sender As Object, e As EventArgs) Handles _MedCoverageValuesComboBS.PositionChanged, _DenCoverageValuesComboBS.PositionChanged

        Dim BS As BindingSource

        Try

            BS = CType(sender, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If Not (_UIState And UIStates.[ReadOnly]) = _UIState Then


            End If

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub DenCoverageValuesComboBS_CurrentChanged(sender As Object, e As EventArgs) Handles _DenCoverageValuesComboBS.CurrentChanged

        Dim BS As BindingSource

        Dim CoverageFilterDate As Date = Date.Now
        Dim CoverageCode As String = Nothing

        Try

            BS = CType(sender, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If _DenCoverageBS IsNot Nothing AndAlso _DenCoverageBS.Position > -1 AndAlso _DenCoverageBS.Current IsNot Nothing AndAlso _DenCoverageBS.Count > 0 Then
                If _DenCoverageValuesComboBS IsNot Nothing AndAlso _DenCoverageValuesComboBS.Position > -1 AndAlso _DenCoverageValuesComboBS.Current IsNot Nothing AndAlso _DenCoverageValuesComboBS.Count > 0 Then

                End If
            End If

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub DenCoverageValuesBS_PositionChanged(sender As Object, e As EventArgs) Handles _DenCoverageValuesComboBS.PositionChanged

        Dim BS As BindingSource

        Try

            BS = CType(sender, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If Not (_UIState And UIStates.[ReadOnly]) = _UIState Then


            End If

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub cmbDenCoverageValues_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbDenCoverageValues.SelectedIndexChanged

        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _DenCoverageBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If CBox.Enabled = False OrElse CBox.ReadOnly Then Return

            If Not (_UIState And UIStates.[ReadOnly]) = _UIState Then
                Dim DR As DataRow = DirectCast(_DenCoverageBS.Current, DataRowView).Row

                If DR IsNot Nothing AndAlso CDate(DR("THRU_DATE")).Year = 9999 AndAlso (DR.HasVersion(DataRowVersion.Proposed) OrElse (DR.RowState = DataRowState.Modified OrElse DR.RowState = DataRowState.Added)) Then 'in addition to current/future active addresses, items awaiting approval will also have 9999 thru date 

                End If
            End If

        Catch ex As Exception

                Throw
        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _DenCoverageBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

    Private Sub cmbMedCoverage_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMedCoverage.SelectedIndexChanged

        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Try

            If _Disposed OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _MedCoverageBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If Not (_UIState And UIStates.[ReadOnly]) = _UIState Then
                Dim DR As DataRow = DirectCast(_MedCoverageBS.Current, DataRowView).Row

            End If

        Catch ex As Exception

                Throw
        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _MedCoverageBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

#End Region

End Class

<Flags()>
Public Enum DestinationActions As Int32
    None = 0
    Delete = 1
    Add = 2
    Modify = 4
    Replace = 8
End Enum
