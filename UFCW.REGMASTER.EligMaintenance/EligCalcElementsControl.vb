Option Strict On
Option Infer On

Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Data.Common
Imports DDTek.DB2
Imports System.Reflection


Public Class EligCalcElementsControl
    Inherits System.Windows.Forms.UserControl
#Region "Variables"

    Private _FamilyID As Integer = -1
    Private _RelationID As Integer = -1
    Private _APPKEY As String = "UFCW\RegMaster\"
    Private _ReadOnlyMode As Boolean = False
    Private _DisplayColumnNames As DataView
    Private _ChangedDRs As DataSet
    Private _MemTypeBS As BindingSource
    Private _TotalEligCalcElementsDS As New DataSet
    Private _LocalsDT As New DataTable
    Private _LocalsBS As BindingSource

    Private _MemPlanDT As New DataTable
    Private _ECEDS As New DataSet

    ReadOnly _REGMEmployeeAccess As Boolean = UFCWGeneralAD.REGMEmployeeAccess
    ReadOnly _REGMReadOnlyAccess As Boolean = UFCWGeneralAD.REGMReadOnlyAccess
    ReadOnly _REGMVendorAccess As Boolean = UFCWGeneralAD.REGMVendorAccess
    ReadOnly _REGMSupervisor As Boolean = UFCWGeneralAD.REGMSupervisor

    Private _ViewHistory As Boolean?

    Private WithEvents _ECEBS As BindingSource

#End Region

    Private _Disposed As Boolean = False

    Protected Overrides Sub Dispose(disposing As Boolean)

        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.

            ' Free any other managed objects here.
            '
            If _LocalsDT IsNot Nothing Then
                _LocalsDT.Dispose()
            End If
            _LocalsDT = Nothing

            If _MemPlanDT IsNot Nothing Then
                _MemPlanDT.Dispose()
            End If
            _MemPlanDT = Nothing

            If _ChangedDRs IsNot Nothing Then
                _ChangedDRs.Dispose()
            End If
            _ChangedDRs = Nothing

            If _TotalEligCalcElementsDS IsNot Nothing Then
                _TotalEligCalcElementsDS.Dispose()
            End If
            _TotalEligCalcElementsDS = Nothing

            If ChecklistDataGrid IsNot Nothing Then
                ChecklistDataGrid.Dispose()
            End If
            ChecklistDataGrid = Nothing

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

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("History Button is visible")>
    Public Property VisibleHistory() As Boolean
        Get
            Return If(_ViewHistory Is Nothing, False, CBool(_ViewHistory))
        End Get
        Set(ByVal value As Boolean)
            _ViewHistory = value
        End Set
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
            ProcessControls(CType(grpEditPanel, Object), _ReadOnlyMode)
        End Set
    End Property

    Public Property WaitPeriodDataset() As DataSet
        Get
            Return _TotalEligCalcElementsDS
        End Get
        Set(ByVal Value As DataSet)
            _TotalEligCalcElementsDS = Value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Indicates that changes are pending.")>
    Public ReadOnly Property ChangesPending() As Boolean
        Get
            Return UnCommittedChangesExist()
        End Get
    End Property

    Private Function UnCommittedChangesExist() As Boolean

        Dim Modifications As String = ""
        Try

            If _ECEDS Is Nothing Then Return False

            _ChangedDRs = _ECEDS.GetChanges

            If ChecklistDataGrid IsNot Nothing AndAlso _ChangedDRs IsNot Nothing AndAlso _ChangedDRs.Tables("ELGCALC_ELEMENTS").Rows.Count > 0 Then

                For Each DR As DataRow In _ChangedDRs.Tables("ELGCALC_ELEMENTS").Rows
                    If DR.RowState <> DataRowState.Added Then
                        'attempt to exclude rows accidently changed during navigation operations
                        Modifications = DataGridCustom.IdentifyChanges(DR, ChecklistDataGrid)

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

#End Region

#Region "Constructor"
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Dim designMode As Boolean = (LicenseManager.UsageMode = LicenseUsageMode.Designtime)
        If Not designMode Then
            LoadMemTypes()
            LoadLocalsList()
        End If

        'dont want to display the default table style
    End Sub

    Public Sub New(ByVal FamilyID As Integer, ByVal RelationID As Integer, Optional ByVal readonlymode As Boolean = False)

        Me.New()
        _FamilyID = FamilyID
        _RelationID = RelationID
        _ReadOnlyMode = readonlymode

        txtfamilyid.Text = _FamilyID.ToString
        txtrelationid.Text = _RelationID.ToString

    End Sub
#End Region

#Region "Form\Button Events"

    Private Sub AddActionButton_Click(sender As System.Object, e As System.EventArgs) Handles AddActionButton.Click

        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

        Try

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

            AddActionButton.Enabled = False
            ModifyActionButton.Enabled = False

            AddECELine()

            cmbMemType.SelectedIndex = -1
            cmbLocal.SelectedIndex = -1

        Catch ex As Exception


                Throw

        Finally

            Me.AutoValidate = HoldAutoValidate

        End Try
    End Sub

    Private Sub CancelActionButton_Click(sender As System.Object, e As System.EventArgs) Handles CancelActionButton.Click

        Dim Result As DialogResult = DialogResult.None 'indicates no changes to roll back

        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

        Try

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

            CancelActionButton.Enabled = False
            SaveActionButton.Enabled = False

            _ChangedDRs = _ECEDS.GetChanges()

            If _ChangedDRs IsNot Nothing Then
                Result = MessageBox.Show(Me, "Do you want to Cancel the changes?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            End If

            If Result = DialogResult.Yes OrElse Result = DialogResult.None Then

                _ECEBS.CancelEdit()
                _ECEDS.Tables("ELGCALC_ELEMENTS").RejectChanges()

                ClearErrors()

                _ECEBS.ResetBindings(False)

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

            SaveActionButton.Enabled = False
            CancelActionButton.Enabled = False

            If Not Me.ValidateChildren(ValidationConstraints.Enabled) OrElse VerifyECEChanges() Then
                SaveActionButton.Enabled = True
                CancelActionButton.Enabled = True
                Return
            End If

            _ECEBS.EndEdit()

            _ChangedDRs = _ECEDS.GetChanges()

            If _ChangedDRs Is Nothing Then     '' when new row added,deleted then cancel, save buttons are enabled
                MessageBox.Show("There are no changes to the record.", "No Changes", MessageBoxButtons.OK, MessageBoxIcon.Information)
                SaveActionButton.Enabled = False
                CancelActionButton.Enabled = False
                Return
            End If

            If _ChangedDRs IsNot Nothing Then

                If SaveECEchanges() Then

                    MessageBox.Show("Checklist Record Saved Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    _TotalEligCalcElementsDS = Nothing
                    _ECEDS.Clear()

                    LoadECE()

                    '' Message to user to calculate eligiblity for current an retro periods
                    MessageBox.Show("Eligibility Calculation is required for this change. ", "Calculate Eligibility", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegMasterDAL.MadeDBChanges = True  '' this is to get elig_acct_hours from database to show pending/ or not
                    RegMasterDAL.MadeEligibilityChanges = True

                Else
                    MessageBox.Show("Error while saving CheckList record." & vbCrLf & "Please try again ", "Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    SaveActionButton.Enabled = True
                    CancelActionButton.Enabled = True
                End If

            End If

        Catch ex As Exception

            SaveActionButton.Enabled = True
            CancelActionButton.Enabled = True


                Throw

        Finally

            Me.AutoValidate = HoldAutoValidate

        End Try

    End Sub

    Private Sub EligCalcElements_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs)
        Me.Dispose()
    End Sub

    Private Sub ModifyActionButton_Click(sender As Object, e As EventArgs) Handles ModifyActionButton.Click

        If _ECEBS Is Nothing OrElse _ECEBS.Position < 0 OrElse _ECEBS.Count < 1 Then Return

        Dim DR As DataRow

        Try

            ModifyActionButton.Enabled = False
            AddActionButton.Enabled = False

            DR = DirectCast(_ECEBS.Current, DataRowView).Row

            DR.BeginEdit() 'sets modified status.
            DR.EndEdit() 'sets modified status.

            ProcessControls(CType(grpEditPanel, Object), _ReadOnlyMode)

        Catch ex As Exception

                Throw
        Finally
            If DR IsNot Nothing Then DR = Nothing
        End Try

    End Sub

    Private Sub cmb_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMemType.SelectedIndexChanged, cmbLocal.SelectedIndexChanged

        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Dim BS As BindingSource

        Try

            If _ECEBS Is Nothing OrElse _ECEBS.Position < 0 OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

            BS = DirectCast(_ECEBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " IO:  " & Me.Name & ":" & CBox.Name & " BS(" & BS.Position.ToString & ") Val(" & CBox.SelectedValue.ToString & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
        End Try


    End Sub

#End Region

#Region "Custom Subs\Functions"

    Public Sub LoadEligCalcElements(ByVal familyID As Integer, ByVal relationID As Integer)

        _FamilyID = familyID
        _RelationID = relationID
        _ReadOnlyMode = ReadOnlyMode

        txtfamilyid.Text = _FamilyID.ToString
        txtrelationid.Text = _RelationID.ToString

        ProcessControls(Me, True, False)

        LoadECE()

    End Sub

    Public Sub LoadECE() 'Elig Calc Elements

        Try

            grpEditPanel.SuspendLayout()

            ClearErrors()
            ClearDataBindings(Me)

            If _TotalEligCalcElementsDS Is Nothing OrElse _TotalEligCalcElementsDS.Tables.Count < 1 OrElse _TotalEligCalcElementsDS.Tables("ELGCALC_ELEMENTS").Rows.Count < 1 Then
                _TotalEligCalcElementsDS = RegMasterDAL.RetrieveEligCalcElementsByFamilyID(_FamilyID)
            End If

            _ECEDS = _TotalEligCalcElementsDS.Clone
            _ECEDS.Tables("ELGCALC_ELEMENTS").Rows.Clear()
            For Each DR As DataRow In _TotalEligCalcElementsDS.Tables("ELGCALC_ELEMENTS").Rows
                _ECEDS.Tables("ELGCALC_ELEMENTS").ImportRow(DR)
            Next

            _ECEBS = New BindingSource
            _ECEBS.RaiseListChangedEvents = False

            _ECEBS.DataSource = _ECEDS.Tables("ELGCALC_ELEMENTS")
            _ECEBS.Sort = If(ChecklistDataGrid.LastSortedBy, ChecklistDataGrid.DefaultSort)

            ChecklistDataGrid.DataSource = _ECEBS
            ChecklistDataGrid.SetTableStyle()

            LoadECEDataBindings()

            _ECEBS.RaiseListChangedEvents = True
            _ECEBS.ResetBindings(False)

            grpEditPanel.ResumeLayout()

        Catch ex As Exception

                Throw
        End Try
    End Sub

    Private Sub LoadECEDataBindings()
        Dim Bind As Binding

        Try

            txtfamilyid.DataBindings.Clear()
            Bind = New Binding("Text", _ECEBS, "FAMILY_ID", True)
            txtfamilyid.DataBindings.Add(Bind)

            txtrelationid.DataBindings.Clear()
            Bind = New Binding("Text", _ECEBS, "RELATION_ID", True)
            txtrelationid.DataBindings.Add(Bind)

            txtEntryDate.DataBindings.Clear()
            Bind = New Binding("Text", _ECEBS, "ENTRY_DATE", True)
            Bind.FormatString = "MM-dd-yyyy"
            AddHandler Bind.Parse, AddressOf UFCWGeneral.DateOnlyBinding_Parse
            txtEntryDate.DataBindings.Add(Bind)

            txtTermDate.DataBindings.Clear()
            Bind = New Binding("Text", _ECEBS, "TERM_DATE", True)
            Bind.FormatString = "MM-dd-yyyy"
            AddHandler Bind.Parse, AddressOf UFCWGeneral.DateOnlyBinding_Parse
            txtTermDate.DataBindings.Add(Bind)

            cmbMemType.DataBindings.Clear()
            Bind = New Binding("SelectedValue", _ECEBS, "LAST_MEMTYPE", True)
            cmbMemType.DataBindings.Add(Bind)

            cmbLocal.DataBindings.Clear()
            Bind = New Binding("SelectedValue", _ECEBS, "LAST_LOCAL", True)
            cmbLocal.DataBindings.Add(Bind)

        Catch ex As Exception


                Throw

        Finally

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

        Me.SuspendLayout()

        ClearErrors()
        ClearDataBindings(Me)

        If _ECEBS IsNot Nothing Then _ECEBS = New BindingSource

        _ECEDS = New DataSet
        _ECEBS = New BindingSource
        _TotalEligCalcElementsDS = New DataSet

        If ChecklistDataGrid.DataSource IsNot Nothing Then ChecklistDataGrid.DataSource = Nothing

        SaveActionButton.Enabled = False
        CancelActionButton.Enabled = False
        AddActionButton.Enabled = False
        ModifyActionButton.Enabled = False

        _ChangedDRs = Nothing

        _FamilyID = -1
        _RelationID = -1

        Me.ResumeLayout()
        Me.Refresh()

    End Sub

    Private Sub SetUIElements(readOnlyMode As Boolean)
        Dim DR As DataRow
        Dim DRs As DataRow()

        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

        Try

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

            grpEditPanel.SuspendLayout()

            If Not readOnlyMode Then readOnlyMode = _REGMReadOnlyAccess
            If UFCWGeneralAD.REGMVendorAccess Then _ViewHistory = False

            If _ECEBS IsNot Nothing AndAlso _ECEBS.Position > -1 AndAlso _ECEBS.Current IsNot Nothing AndAlso _ECEBS.Count > 0 Then
                DR = CType(_ECEBS.Current, DataRowView).Row
            End If

            If DR IsNot Nothing AndAlso (_ViewHistory Is Nothing OrElse _ViewHistory) Then
                Me.HistoryButton.Enabled = True
                Me.HistoryButton.Visible = True
            End If

            CancelActionButton.Enabled = False
            SaveActionButton.Enabled = False

            ProcessSubControls(CType(grpEditPanel, Object), True, True) 'lock everything down except buttons

            If readOnlyMode Then

                AddActionButton.Visible = False
                ModifyActionButton.Visible = False

                CancelActionButton.Visible = False
                SaveActionButton.Visible = False

            Else

                AddActionButton.Visible = True
                ModifyActionButton.Visible = True

                CancelActionButton.Visible = True
                SaveActionButton.Visible = True

                If DR IsNot Nothing Then 'based upon row status / content decide how to present controls

                    CancelActionButton.Enabled = True
                    SaveActionButton.Enabled = True

                    If DR.RowState = DataRowState.Added Then

                        txtEntryDate.ReadOnly = False
                        txtTermDate.ReadOnly = False
                        cmbLocal.ReadOnly = False
                        cmbMemType.ReadOnly = False

                        AddActionButton.Enabled = False
                        ModifyActionButton.Enabled = False

                    ElseIf DR.RowState = DataRowState.Modified Then

                        txtEntryDate.ReadOnly = False
                        txtTermDate.ReadOnly = False
                        cmbLocal.ReadOnly = False
                        cmbMemType.ReadOnly = False

                        AddActionButton.Enabled = False
                        ModifyActionButton.Enabled = False

                        CancelActionButton.Enabled = True
                        SaveActionButton.Enabled = True

                    ElseIf DR.RowState = DataRowState.Unchanged Then

                        CancelActionButton.Enabled = False
                        SaveActionButton.Enabled = False

                        AddActionButton.Enabled = False

                        If _ECEDS.Tables("ELGCALC_ELEMENTS").Select("RELATION_ID=" & _RelationID).Length > 0 Then

                            ModifyActionButton.Enabled = True

                        ElseIf _ECEDS.Tables("ELGCALC_ELEMENTS").Select("RELATION_ID=" & _RelationID).Length = 0 Then

                            ModifyActionButton.Enabled = False

                        End If

                    Else

                        AddActionButton.Enabled = False
                        ModifyActionButton.Enabled = False

                    End If
                Else
                    AddActionButton.Enabled = True
                    ModifyActionButton.Enabled = False

                End If
            End If

            grpEditPanel.ResumeLayout() 'needed to ensure transparent controls child controls draw correctly
            grpEditPanel.Refresh()

        Catch ex As Exception

                Throw

        Finally
            Me.AutoValidate = HoldAutoValidate
        End Try

        'If _ReadOnlyMode And excludeButtons Then
        '    Me.CancelActionButton.Enabled = False
        '    Me.SaveActionButton.Enabled = False
        'End If

        'If Not _ReadOnlyMode Then
        '    If _FamilyID <> -1 And _RelationID <> -1 Then
        '        If _ECEDS.Tables("ELGCALC_ELEMENTS").Rows.Count > 0 Then  ''To disable buttons in un modifiable state
        '            ModifyActionButton.Visible = True
        '            ModifyActionButton.Enabled = True
        '            AddActionButton.Visible = False
        '        ElseIf _ECEDS.Tables("ELGCALC_ELEMENTS").Rows.Count = 0 Then
        '            ModifyActionButton.Visible = False
        '            AddActionButton.Visible = True
        '            AddActionButton.Enabled = True
        '            CancelActionButton.Enabled = False
        '            grpEditPanel.Enabled = False
        '        End If
        '    End If
        'Else
        '    AddActionButton.Visible = False
        '    AddActionButton.Enabled = False
        '    ModifyActionButton.Visible = False
        '    ModifyActionButton.Enabled = False
        '    CancelActionButton.Enabled = False
        '    SaveActionButton.Enabled = False

        'End If

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

    Private Sub AddECELine()
        Dim DR As DataRow

        Try
            _ECEDS.EnforceConstraints = False
            DR = _ECEDS.Tables("ELGCALC_ELEMENTS").NewRow

            DR.BeginEdit()

            DR("FAMILY_ID") = _FamilyID
            DR("RELATION_ID") = _RelationID
            DR("ENTRY_DATE") = DBNull.Value
            DR("BREAK_IN_SERVICE") = CDate("12-31-9999")
            DR("GRAD_COUNT") = 0
            DR("GRAD_STATUS") = "Y"
            DR("LAST_DATE_WORKED") = CDate("12-31-9999")
            DR("LAST_EMPLOYER") = 0
            DR("LAST_VACATION_UPDATE") = CDate("12-31-9999")
            DR("VACATION_DATE") = CDate("12-31-9999")
            DR("TERM_DATE") = CDate("12-31-9999")
            DR("VACATION_MONTHS") = 0
            DR("CREATE_USERID") = UFCWGeneral.DomainUser.ToUpper
            DR("CREATE_DATE") = CDate(Now)
            DR("BATCH_USERID") = UFCWGeneral.DomainUser.ToUpper   '' ONLINE_USERID
            DR("BATCH_DATE") = CDate(Now)  ''ONLINE_DATE
            DR("ONLINE_USERID") = UFCWGeneral.DomainUser.ToUpper     ''CREATE_USERID
            DR("ONLINE_DATE") = CDate(Now)
            DR("LAST_HOURLY_ACCTNO") = 0
            DR("LAST_HOURLY_WORKPERIOD") = CDate("12-31-9999")
            DR("POSTING_ACTIVITY_IND") = "N"

            DR.EndEdit()

            _ECEDS.Tables("ELGCALC_ELEMENTS").Rows.Add(DR)

        Catch ex As Exception

            Throw

        End Try
    End Sub

    Private Function VerifyECEChanges() As Boolean
        Try

            ClearErrors()

            If Not _ReadOnlyMode AndAlso _ECEBS IsNot Nothing AndAlso _ECEBS.Position > -1 AndAlso _ECEBS.Count > 0 AndAlso _ECEBS.Current IsNot Nothing Then

                Dim dr As DataRow = DirectCast(_ECEBS.Current, DataRowView).Row

                ''Only supervisor have authority to change the entry date on elig_cal_elements (check list)
                If dr.RowState = DataRowState.Modified Then
                    If (IsDate(dr("ENTRY_DATE")) AndAlso CDate(dr("ENTRY_DATE")) <> CDate(dr("ENTRY_DATE", DataRowVersion.Original))) Then
                        If Not _REGMSupervisor Then
                            ErrorProvider1.SetErrorWithTracking(Me.txtEntryDate, " You don't have authority to change Entry Date.")
                        End If
                    End If

                    If (IsDate(dr("TERM_DATE")) AndAlso CDate(dr("TERM_DATE")) <> CDate(dr("TERM_DATE", DataRowVersion.Original))) Then
                        If Not _REGMSupervisor Then
                            ErrorProvider1.SetErrorWithTracking(Me.txtTermDate, " You don't have authority to change Term Date.")
                        End If
                    End If
                End If

            End If

            If ErrorProvider1.GetErrorCount > 0 Then
                Return True
            End If

        Catch ex As Exception


                Throw
        End Try
        Return False
    End Function

    Public Function SaveECEchanges() As Boolean

        Dim Transaction As DbTransaction

        Dim HistSum As String = ""
        Dim HistDetail As String = ""

        Dim ECEChangesDetail As String = ""
        Dim ActivityTimeStamp As DateTime = Date.Now

        Try
            Transaction = RegMasterDAL.BeginTransaction

            _ChangedDRs = _ECEDS.GetChanges()

            For Each ECEDR As DataRow In _ChangedDRs.Tables("ELGCALC_ELEMENTS").Rows

                HistSum = ""
                HistDetail = ""

                ECEChangesDetail = DataGridCustom.IdentifyChanges(ECEDR, ChecklistDataGrid)

                If ECEDR.RowState = DataRowState.Modified Then
                    HistSum = "ELIG CHECKLIST FOR FAMILYID: " & Me.FamilyID & " WAS MODIFIED"
                    HistDetail = UFCWGeneral.DomainUser.ToUpper & " MODIFIED ELIG CHECKLIST RECORD.THE MODIFICATIONS WERE: " & Environment.NewLine & ECEChangesDetail

                    RegMasterDAL.UpdateEligCalcElements(CInt(ECEDR("FAMILY_ID")), CDate(ECEDR("ENTRY_DATE")), CStr(ECEDR("LAST_MEMTYPE")), CInt(ECEDR("LAST_LOCAL")), CDate(ECEDR("TERM_DATE")), Transaction)

                    RegMasterDAL.CreateRegHistory(Me.FamilyID, Me.RelationID, Nothing, Nothing, "CHECKLISTUPDATE", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, Transaction)


                ElseIf ECEDR.RowState = DataRowState.Added Then 'ADD

                    HistSum = "ELIG CHECKLIST OF FAMILYID: " & Me.FamilyID & " WAS ADDED"
                    HistDetail = UFCWGeneral.DomainUser.ToUpper & " ADDED THE ELIG CHECKLIST RECORD " & Environment.NewLine &
                                                " FOR FAMILYID: " & Me.FamilyID.ToString & " AND RELATIONID: " & Me.RelationID.ToString & Environment.NewLine & ECEChangesDetail

                    RegMasterDAL.AddEligCalcElements(CInt(ECEDR("FAMILY_ID")), CDate(ECEDR("ENTRY_DATE")), CStr(ECEDR("LAST_MEMTYPE")), CInt(ECEDR("LAST_LOCAL")), Transaction)

                    RegMasterDAL.CreateRegHistory(Me.FamilyID, Me.RelationID, Nothing, Nothing, "CHECKLISTADD", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, Transaction)

                End If
            Next

            RegMasterDAL.CommitTransaction(Transaction)

            Dim ECEQuery = (From ECE As DataRow In _ECEDS.Tables("ELGCALC_ELEMENTS").AsEnumerable()
                            Where ECE.RowState = DataRowState.Modified OrElse ECE.RowState = DataRowState.Added)

            For Each DR As DataRow In ECEQuery
                DR("ONLINE_DATE") = ActivityTimeStamp
                DR("ONLINE_USERID") = UFCWGeneral.DomainUser.ToUpper
            Next

            _ECEDS.AcceptChanges()

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
                    MessageBox.Show("The item(s) you are attempting to update has been changed by another process." & vbCrLf & "Exit and re-enter the CheckList Form to refresh the data.", "Save rejected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
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

    Public Sub ClearErrors()
        ErrorProvider1.Clear()
    End Sub

    Public Sub LoadMemTypes()

        Try

            If _MemPlanDT Is Nothing OrElse _MemPlanDT.Rows.Count < 1 Then
                _MemPlanDT = RegMasterDAL.RetrieveMemplans
            End If

            _MemTypeBS = New BindingSource
            _MemTypeBS.DataSource = _MemPlanDT
            _MemTypeBS.Sort = "MEMTYPE"

            cmbMemType.ValueMember = "MEMTYPE"
            cmbMemType.DisplayMember = "MEMTYPEPLANTYPE"
            cmbMemType.DataSource = _MemTypeBS

            cmbMemType.SelectedIndex = -1

        Catch ex As Exception

                Throw

        Finally

        End Try
    End Sub

    Public Sub LoadLocalsList()

        Try
            If _LocalsDT Is Nothing OrElse _LocalsDT.Rows.Count < 1 Then
                _LocalsDT = RegMasterDAL.RetrieveLocals
            End If

            _LocalsBS = New BindingSource
            _LocalsBS.DataSource = _LocalsDT
            _LocalsBS.Sort = "LOCALNO"

            cmbLocal.ValueMember = "LOCALNO"
            cmbLocal.DisplayMember = "LOCALNO"
            cmbLocal.DataSource = _LocalsBS

            cmbLocal.SelectedIndex = -1

        Catch ex As Exception

                Throw

        Finally

        End Try
    End Sub

#End Region

#Region "Formatting"

    Private Sub DateOnlyBinding_BindingComplete(ByVal sender As Object, ByVal e As BindingCompleteEventArgs)
        Try
            ErrorProvider1.SetErrorWithTracking(CType(e.Binding.BindableComponent, Control), "")

            If e.BindingCompleteState <> BindingCompleteState.Success Then
                ErrorProvider1.SetErrorWithTracking(CType(e.Binding.BindableComponent, Control), "Date format invalid. Use mmddyy or mmddyyyy")
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

    Private Sub ECEBS_ListChanged(sender As Object, e As ListChangedEventArgs) Handles _ECEBS.ListChanged

        Dim BS As BindingSource

        Try
            BS = DirectCast(_ECEBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") RS(" & If(BS.Position < 0 ORELSE BS.Current Is Nothing, "N/A", DirectCast(BS.Current, System.Data.DataRowView).Row.RowState.ToString) & ") O(" & e.OldIndex.ToString & ") N(" & e.NewIndex.ToString & ") CT(" & e.ListChangedType.ToString & ") SEL(" & If(BS Is Nothing OrElse BS.Count < 1 OrElse BS.Position < 0 OrElse ChecklistDataGrid.DataSource Is Nothing, "N/A", ChecklistDataGrid.IsSelected(BS.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Select Case e.ListChangedType
                Case ListChangedType.ItemDeleted 'account for rows deleted due to a cancel action

                Case ListChangedType.ItemMoved

                    Select Case DirectCast(BS.Current, System.Data.DataRowView).Row.RowState
                        Case DataRowState.Modified

                        Case DataRowState.Added

                            If e.OldIndex <> e.NewIndex OrElse (BS.Position > -1 AndAlso BS.Count = 1) Then
                                ChecklistDataGrid.Select(BS.Position)
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
                        If e.NewIndex > -1 Then ChecklistDataGrid.Select(e.NewIndex)
                    End If

            End Select

            ProcessControls()

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") RS(" & If(BS.Position < 0 ORELSE BS.Current Is Nothing, "N/A", DirectCast(BS.Current, System.Data.DataRowView).Row.RowState.ToString) & ") O(" & e.OldIndex.ToString & ") N(" & e.NewIndex.ToString & ") CT(" & e.ListChangedType.ToString & ") SEL(" & If(BS Is Nothing OrElse BS.Count < 1 OrElse BS.Position < 0 OrElse ChecklistDataGrid.DataSource Is Nothing, "N/A", ChecklistDataGrid.IsSelected(BS.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub ECEBS_CurrentChanged(sender As Object, e As EventArgs) Handles _ECEBS.CurrentChanged

        Dim BS As BindingSource

        Try

            BS = DirectCast(_ECEBS, BindingSource)

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

    Private Sub _ECEBS_CurrentItemChanged(sender As Object, e As EventArgs) Handles _ECEBS.CurrentItemChanged

        Dim BS As BindingSource

        Try

            BS = DirectCast(_ECEBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If BS Is Nothing OrElse BS.Position < 0 OrElse BS.Count < 1 Then Return 'no current row, most likely an item filter value was changed

            Dim DR As DataRow = DirectCast(BS.Current, DataRowView).Row

        Catch ex As Exception
            Throw
        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub txtEntrydate_Validating(sender As Object, e As CancelEventArgs) Handles txtEntryDate.Validating

        Dim Tbox As TextBox = CType(sender, TextBox)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If _ECEBS Is Nothing OrElse _ECEBS.Position < 0 OrElse Tbox.ReadOnly Then Return

            ErrorProvider1.ClearError(Tbox)

            If Tbox.Text.Trim.Length < 1 Then
                ErrorProvider1.SetErrorWithTracking(Tbox, " Entry Date is required.")
            Else
                Dim HoldDate As Date? = UFCWGeneral.ValidateDate(Tbox.Text)
                If HoldDate Is Nothing Then
                    ErrorProvider1.SetErrorWithTracking(Tbox, " Date format must be mm/dd/yyyy or mm-dd-yyyy.")
                Else

                    If HoldDate > Now.Date Then
                        ErrorProvider1.SetErrorWithTracking(Tbox, " Entry Date cannot be future date.")
                    ElseIf CDate(HoldDate).Day <> 1 Then
                        HoldDate = CDate(CDate(HoldDate).Month.ToString & "-01-" & CDate(HoldDate).Year.ToString)
                        ErrorProvider1.SetErrorWithTracking(Tbox, " Date modified to use 1st day of month.")
                    End If

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

    Private Sub txtTermDate_Validating(sender As Object, e As CancelEventArgs) Handles txtTermDate.Validating

        Dim Tbox As TextBox = CType(sender, TextBox)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ErrorProvider1.ClearError(Tbox)

            If _ECEBS Is Nothing OrElse _ECEBS.Position < 0 OrElse Tbox.ReadOnly OrElse Tbox.Text.Trim.Length < 1 Then Return

            Dim HoldDate As Date? = UFCWGeneral.ValidateDate(Tbox.Text)
            If HoldDate Is Nothing Then
                ErrorProvider1.SetErrorWithTracking(Tbox, " Date format must be mm/dd/yyyy or mm-dd-yyyy.")
            Else

                If HoldDate > Now.Date AndAlso HoldDate <> CDate("9999-12-31") Then
                    ErrorProvider1.SetErrorWithTracking(Tbox, " Term Date cannot be future date.")
                End If

                If Tbox.Text <> CDate(HoldDate).ToString("MM-dd-yyyy") Then
                    Tbox.Text = CDate(HoldDate).ToString("MM-dd-yyyy")
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

    Private Sub HistoryButton_Click(sender As System.Object, e As System.EventArgs) Handles HistoryButton.Click

        Dim FRM As RegMasterHistoryForm

        Try

            FRM = New RegMasterHistoryForm
            FRM.FamilyID = _FamilyID
            FRM.RelationID = 0
            FRM.Mode = REGMasterHistoryMode.Memtype
            FRM.ShowDialog()

            FRM.Close()

        Catch ex As Exception
            Throw
        Finally
            If FRM IsNot Nothing Then FRM.Dispose()
            FRM = Nothing
        End Try

    End Sub

    Private Sub _ECEBS_PositionChanged(sender As Object, e As EventArgs) Handles _ECEBS.PositionChanged

        Dim BS As BindingSource

        Try

            BS = DirectCast(_ECEBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If BS Is Nothing OrElse BS.Position < 0 OrElse BS.Count < 1 Then Return 'no current row, most likely an item filter value was changed

            Dim DR As DataRow = DirectCast(BS.Current, DataRowView).Row

        Catch ex As Exception
            Throw
        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub cmbLocal_Validating(sender As Object, e As CancelEventArgs) Handles cmbLocal.Validating
        Dim Cbox As ExComboBox = CType(sender, ExComboBox)
        Dim DR As DataRow

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If _ECEBS Is Nothing OrElse _ECEBS.Position < 0 OrElse Cbox.ReadOnly Then Return

            ErrorProvider1.ClearError(Cbox)

            If Cbox.SelectedIndex < 0 Then
                ErrorProvider1.SetErrorWithTracking(Cbox, " Local is required.")
            End If

            If ErrorProvider1.GetError(Cbox).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
            End If

        Catch ex As Exception

                Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub cmbMemType_Validating(sender As Object, e As CancelEventArgs) Handles cmbMemType.Validating
        Dim Cbox As ExComboBox = CType(sender, ExComboBox)
        Dim DR As DataRow

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If _ECEBS Is Nothing OrElse _ECEBS.Position < 0 OrElse Cbox.ReadOnly Then Return

            ErrorProvider1.ClearError(Cbox)

            If Cbox.SelectedIndex < 0 Then
                ErrorProvider1.SetErrorWithTracking(Cbox, " MemType is required.")
            End If

            If ErrorProvider1.GetError(Cbox).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
            End If

        Catch ex As Exception

                Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub cmbLocal_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbLocal.SelectionChangeCommitted, cmbMemType.SelectionChangeCommitted
        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Dim DR As DataRow
        Dim BS As BindingSource

        Try

            If _ECEBS Is Nothing OrElse _ECEBS.Position < 0 OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

            BS = DirectCast(_ECEBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & CBox.Name & " BS(" & BS.Position.ToString & ") Val(" & CBox.Text & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = DirectCast(_ECEBS.Current, DataRowView).Row

            CType(CBox.Parent, TransparentContainer).ValidateChildren() 'this will trigger validation of the cmbbox triggering write of value to DS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: (" & Me.Name & ":" & CBox.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
        End Try

    End Sub

    Private Sub cmb_Validated(sender As Object, e As EventArgs) Handles cmbLocal.Validated, cmbMemType.Validated

        Dim DR As DataRow
        Dim CBox As ExComboBox = CType(sender, ExComboBox)
        Dim BS As BindingSource

        Try

            If _ECEBS Is Nothing OrElse _ECEBS.Position < 0 OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

            BS = DirectCast(_ECEBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & CBox.Name & " BS(" & BS.Position.ToString & ") Val(" & CBox.Text & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = DirectCast(_ECEBS.Current, DataRowView).Row

            If CBox.DataBindings.Count > 0 Then

                Select Case CBox.Name
                    Case "cmbLocal"

                        If DR("LAST_LOCAL").ToString <> CBox.SelectedValue.ToString Then
                            CBox.DataBindings("SelectedValue").WriteValue()
                        End If
                    Case "cmbMemType"

                        If DR("LAST_MEMTYPE").ToString <> CBox.SelectedValue.ToString Then
                            CBox.DataBindings("SelectedValue").WriteValue()
                        End If

                End Select
                _ECEBS.EndEdit()

            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & ":" & CBox.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally

        End Try
    End Sub

    Private Sub cmb_DropDown(sender As Object, e As EventArgs) Handles cmbLocal.DropDown, cmbMemType.DropDown
        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _ECEBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Try

            ErrorProvider1.ClearError(CBox)
            CBox.Invalidate()
            grpEditPanel.Refresh()

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _ECEBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

#End Region

End Class
