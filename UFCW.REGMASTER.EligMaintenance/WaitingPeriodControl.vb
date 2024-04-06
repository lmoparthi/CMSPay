Option Strict On
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Configuration
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Data.Common
Imports DDTek.DB2
Imports System.Reflection

Public Class WaitingPeriodControl
    Inherits System.Windows.Forms.UserControl
#Region "Variables"

    Private _FamilyID As Integer = -1
    Private _RelationID As Integer = -1
    Private _PartSSNo As Integer = -1
    Private _APPKEY As String = "UFCW\RegMaster\"
    Private _ReadOnlyMode As Boolean = False
    Private _DisplayColumnNamesDV As DataView
    Private _ChangedWaitPeriodRows As WaitPeriodDS
    Private _WaitBindingManagerBase As BindingManagerBase
    Private _TotalWaitPeriodDS As New WaitPeriodDS
    Private _DOB As Date
    Private Shared _DomainUser As String = SystemInformation.UserName
#End Region

    Private _Disposed As Boolean = False
    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.
            '

            If _ChangedWaitPeriodRows IsNot Nothing Then
                _ChangedWaitPeriodRows.Dispose()
            End If
            _ChangedWaitPeriodRows = Nothing

            If _TotalWaitPeriodDS IsNot Nothing Then
                _TotalWaitPeriodDS.Dispose()
            End If
            _TotalWaitPeriodDS = Nothing

            If WaitPeriodDataGrid IsNot Nothing Then
                WaitPeriodDataGrid.Dispose()
            End If
            WaitPeriodDataGrid = Nothing

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

    Public Property WaitPeriodDataset() As DataSet
        Get
            Return _TotalWaitPeriodDS
        End Get
        Set(ByVal Value As DataSet)
            _TotalWaitPeriodDS = CType(Value, WaitPeriodDS)
        End Set
    End Property

#End Region

#Region "Constructor"
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        'dont want to display the default table style
    End Sub

    Public Sub New(ByVal FamilyID As Integer, ByVal RelationID As Integer, ByVal SSN As Integer, Optional ByVal readonlymode As Boolean = False)
        Me.New()
        _FamilyID = FamilyID
        _RelationID = RelationID
        _ReadOnlyMode = readonlymode
        _PartSSNo = SSN
        txtfamilyid.Text = _FamilyID.ToString
        txtrelationid.Text = _RelationID.ToString
        grpEditPanel.Enabled = False
    End Sub
#End Region

#Region "Form\Button Events"

    Private Sub btnAdd_Click(sender As System.Object, e As System.EventArgs) Handles btnAdd.Click
        Try
            If (_TotalWaitPeriodDS IsNot Nothing AndAlso _TotalWaitPeriodDS.Tables("MEMBER_WAIT").Rows.Count = 0) Then

                MessageBox.Show("Participant is not Plan A or Plan B ", "PlanType", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            grpEditPanel.Enabled = True
            btnAdd.Enabled = False
            AddWaitingPeriodLine()
            txtEligDt.ReadOnly = False
            txtEligDt.Focus()

        Catch ex As Exception


                Throw

        Finally
            ''WaitPeriodDataGrid.Select(WaitBindingManagerBase.Position)
        End Try
    End Sub

    Private Sub btncancel_Click(sender As System.Object, e As System.EventArgs) Handles CancelButton.Click
        Dim Result As DialogResult = DialogResult.None
        Dim ChangedRMRows As WaitPeriodDS = Nothing

        Try
            Result = MessageBox.Show(Me, "Do you want to Cancel the changes?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If Result = DialogResult.Yes Then

                WPDS.Tables("WAITING_PERIOD").RejectChanges()
                LoadwpDataBindings()
                ChangedRMRows = CType(WPDS.GetChanges(), WaitPeriodDS)
                ClearErrors()
                grpEditPanel.Enabled = False
                CancelButton.Enabled = False
                SaveButton.Enabled = False
                btnAdd.Enabled = True
                btnModify.Enabled = False
                LoadWaitingPeriod()

            ElseIf Result = DialogResult.No Then
                CancelButton.Enabled = True
                SaveButton.Enabled = True
            End If
        Catch ex As Exception

        Finally

            If _WaitBindingManagerBase IsNot Nothing AndAlso _WaitBindingManagerBase.Count > 0 Then
                _WaitBindingManagerBase.Position = 0
                WaitPeriodDataGrid.Select(_WaitBindingManagerBase.Position)
            End If

            If IsNothing(ChangedRMRows) Then
                Debug.Print("Rows Changed: 0")
            Else
                Debug.Print("Rows Changed: " & ChangedRMRows.Tables(0).Rows.Count)
            End If

        End Try
    End Sub

    Private Sub SaveButton_Click(sender As System.Object, e As System.EventArgs) Handles SaveButton.Click

        Dim Transaction As DbTransaction = Nothing
        Try
            SaveButton.Enabled = False
            CancelButton.Enabled = False

            _WaitBindingManagerBase.EndCurrentEdit()

            If VerifyWPChanges() Then
                SaveButton.Enabled = True
                CancelButton.Enabled = True
                Exit Sub
            End If

            _ChangedWaitPeriodRows = CType(WPDS.GetChanges(), WaitPeriodDS)

            If _ChangedWaitPeriodRows Is Nothing Then     '' when new row added,deleted then cancel, save buttons are enabled
                MessageBox.Show("There are no changes to the record.", "No Changes", MessageBoxButtons.OK, MessageBoxIcon.Information)
                SaveButton.Enabled = False
                CancelButton.Enabled = False
                Exit Sub
            End If


            If _ChangedWaitPeriodRows IsNot Nothing Then
                Transaction = RegMasterDAL.BeginTransaction

                Try

                    If SaveWaitingPeriodchanges(Transaction) = True Then
                        RegMasterDAL.CommitTransaction(Transaction)
                        MessageBox.Show("Waiting Period Record Saved Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        _TotalWaitPeriodDS = Nothing
                        'RegMasterControl.changesmade = True
                        'RegMasterControl.riskfromdate = CDate(txtEligDt.Text)
                        WPDS.Clear()
                        LoadWaitingPeriod()
                    Else
                        RegMasterDAL.RollbackTransaction(Transaction)
                        MessageBox.Show("Error while saving Waiting Period Record." & vbCrLf & "Please try again ", "Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

            SaveButton.Enabled = True
            CancelButton.Enabled = True


                Throw

        Finally

            If ErrorProviderErrorsList(ErrorProvider1).Length = 0 AndAlso _WaitBindingManagerBase.Count > 0 Then
                _WaitBindingManagerBase.Position = 0
                WaitPeriodDataGrid.Select(_WaitBindingManagerBase.Position)
            End If
            If Transaction IsNot Nothing Then Transaction.Dispose()
            Transaction = Nothing
        End Try

    End Sub

    Private Sub waitingperiod_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs)
        Me.Dispose()
    End Sub

    Private Sub waitingperiod_dispose(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        Me.Dispose()
    End Sub

    Private Sub btnModify_Click(sender As Object, e As EventArgs) Handles btnModify.Click
        If _WaitBindingManagerBase.Position = -1 Then Return

        grpEditPanel.Enabled = True

        btnModify.Enabled = False
        btnAdd.Enabled = False

        SaveButton.Enabled = True
        CancelButton.Enabled = True

        txtEligDt.Enabled = True

    End Sub

    Private Sub txtEligDt_Leave(sender As Object, e As EventArgs) Handles txtEligDt.LostFocus
        If IsNothing(_WaitBindingManagerBase) OrElse _WaitBindingManagerBase.Position = -1 Then Exit Sub
        If _WaitBindingManagerBase.Position < 0 Then Exit Sub
        Dim dr As DataRow = DirectCast(_WaitBindingManagerBase.Current, DataRowView).Row
        Try
            '' date is always start of the month.

            Dim dtfrom As Date = CDate(txtEligDt.Text)
            Dim startday As Date = dtfrom.AddDays(-(DatePart(DateInterval.Day, dtfrom) * 1.0 - 1))
            If DateDiff(DateInterval.Day, startday, CDate(dr("WAITPER_DATE"))) <> 0 Then
                txtEligDt.Text = Format(startday, "MM-dd-yyyy")
                dr("WAITPER_DATE") = txtEligDt.Text
            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "Custom Subs\Functions"

    Public Sub LoadWaitingPeriod(ByVal FamilyID As Integer, ByVal RelationID As Integer, ByVal SSN As Integer)

        _FamilyID = FamilyID
        _RelationID = RelationID
        _ReadOnlyMode = ReadOnlyMode
        _PartSSNo = SSN
        txtfamilyid.Text = _FamilyID.ToString
        txtrelationid.Text = _RelationID.ToString
        LoadWaitingPeriod()

    End Sub

    Public Sub LoadWaitingPeriod()

        Try

            ClearErrors()
            ClearDataBindings()

            If _TotalWaitPeriodDS IsNot Nothing Then
                If Me._TotalWaitPeriodDS.Tables("WAITING_PERIOD").Rows.Count > 0 Then
                    WPDS.Tables("WAITING_PERIOD").Rows.Clear()
                    WPDS = CType(_TotalWaitPeriodDS.Copy, WaitPeriodDS)
                End If
            End If

            If WPDS.Tables("WAITING_PERIOD").Rows.Count = 0 Then  '' only retrieve data for first time
                WPDS = CType(RegMasterDAL.RetrieveWaitingPeriodByFamilyid(_FamilyID, WPDS), WaitPeriodDS)
                Me._TotalWaitPeriodDS = CType(WPDS.Copy, WaitPeriodDS)
            End If

            WaitPeriodDataGrid.DataSource = WPDS.WAITING_PERIOD
            WaitPeriodDataGrid.SetTableStyle()
            WaitPeriodDataGrid.Sort = If(WaitPeriodDataGrid.LastSortedBy, WaitPeriodDataGrid.DefaultSort)

            LoadwpDataBindings()
            ProcessControls(_ReadOnlyMode, False)

            If Not _ReadOnlyMode AndAlso WPDS.Tables("WAITING_PERIOD").Rows.Count < 1 Then
                ProcessControls(True, True) ' disable UI Input elements until a row is added
            End If

            grpEditPanel.Enabled = False

        Catch ex As Exception

                Throw
        End Try
    End Sub

    Private Sub LoadwpDataBindings()
        Dim Bind As Binding

        Try

            WaitPeriodDataGrid.DataMember = ""
            WaitPeriodDataGrid.DataSource = WPDS.WAITING_PERIOD

            _WaitBindingManagerBase = Me.BindingContext(WPDS.WAITING_PERIOD)


            txtfamilyid.DataBindings.Clear()
            Bind = New Binding("Text", WPDS.WAITING_PERIOD, "FAMILY_ID")
            txtfamilyid.DataBindings.Add(Bind)

            txtrelationid.DataBindings.Clear()
            Bind = New Binding("Text", WPDS.WAITING_PERIOD, "RELATION_ID")
            txtrelationid.DataBindings.Add(Bind)

            txtEligDt.DataBindings.Clear()
            Bind = New Binding("Text", WPDS.WAITING_PERIOD, "WAITPER_DATE")
            AddHandler Bind.Format, AddressOf DateOnlyBinding_Format
            AddHandler Bind.Parse, AddressOf DateOnlyBinding_Parse
            txtEligDt.DataBindings.Add(Bind)

            AddHandler _WaitBindingManagerBase.PositionChanged, AddressOf WaitBindingManagerBase_PositionChanged
            AddHandler _WaitBindingManagerBase.CurrentChanged, AddressOf WaitBindingManagerBase_CurrentChanged

            _WaitBindingManagerBase.ResumeBinding()

        Catch ex As Exception


                Throw

        Finally

        End Try

    End Sub

    Public Sub ProcessControls(ByVal readOnlyMode As Boolean, Optional ByVal excludeButtons As Boolean = False)

        Try

            For Each ChildCtrl As Object In Me.Controls 'recursive to accomodate groupings

                If Not (ExtensionMethods.HasProperty(ChildCtrl, NameOf(readOnlyMode)) OrElse ExtensionMethods.HasMethod(ChildCtrl, "ProcessControls")) Then
                    ProcessSubControls(ChildCtrl, readOnlyMode, excludeButtons)
                Else
                    Dim CtrlMethod As MethodInfo
                    Dim CtrlProperty As PropertyInfo
                    Dim result As Object

                    Dim SharedCtrl As Control

                    SharedCtrl = DirectCast(ChildCtrl, Control)

                    Debug.Print(Me.Name & ": (" & readOnlyMode.ToString & ") " & If(SharedCtrl.Parent.Name IsNot Nothing, SharedCtrl.Parent.Name & " : ", "") & SharedCtrl.Name & " : " & SharedCtrl.GetType.ToString)

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

            If _ReadOnlyMode And excludeButtons Then
                Me.CancelButton.Enabled = False
                Me.SaveButton.Enabled = False
            End If

            If Not _ReadOnlyMode Then
                If _FamilyID <> -1 And _RelationID <> -1 Then
                    If WPDS.Tables("WAITING_PERIOD").Rows.Count > 0 Then  ''To disable buttons in un modifiable state
                        btnModify.Visible = True
                        btnModify.Enabled = True
                        btnAdd.Visible = False
                    ElseIf WPDS.Tables("WAITING_PERIOD").Rows.Count = 0 Then
                        btnModify.Visible = False
                        btnAdd.Visible = True
                        btnAdd.Enabled = True
                        CancelButton.Enabled = False
                        grpEditPanel.Enabled = False
                    End If
                End If
            Else
                btnAdd.Visible = False
                btnAdd.Enabled = False
                btnModify.Visible = False
                btnModify.Enabled = False
                CancelButton.Enabled = False
                SaveButton.Enabled = False

            End If
        Catch ex As Exception

                Throw
        End Try

    End Sub

    Public Sub ProcessSubControls(ByRef ctrl As Object, ByVal readOnlyMode As Boolean, Optional ByVal excludeButtons As Boolean = False)

        Dim ParentCtrl As Control

        Try
            ParentCtrl = DirectCast(ctrl, Control)

            If ParentCtrl.IsDisposed Then Return

            '  Ignore the control unless it's a textbox.
            'Debug.Print(If(ParentCtrl.Parent.Name IsNot Nothing, ParentCtrl.Parent.Name & " : ", "") & ParentCtrl.Name & " : " & ctrl.GetType.ToString)

            If TypeOf (ctrl) Is RadioButton OrElse TypeOf (ctrl) Is TextBox OrElse TypeOf (ctrl) Is ComboBox OrElse TypeOf (ctrl) Is DateTimePicker OrElse TypeOf (ctrl) Is Button OrElse TypeOf (ctrl) Is CheckBox OrElse TypeOf (ctrl) Is Label OrElse TypeOf (ctrl) Is DataGrid Then
                If TypeOf (ctrl) Is Label OrElse TypeOf (ctrl) Is DataGrid Then
                ElseIf TypeOf (ctrl) Is TextBox Then
                    CType(ctrl, TextBox).ReadOnly = readOnlyMode
                ElseIf TypeOf (ctrl) Is ComboBox Then
                    CType(ctrl, ComboBox).Enabled = Not readOnlyMode
                ElseIf TypeOf (ctrl) Is DateTimePicker Then
                    CType(ctrl, DateTimePicker).Enabled = Not readOnlyMode
                ElseIf TypeOf (ctrl) Is Button AndAlso Not excludeButtons Then
                    CType(ctrl, Button).Visible = Not readOnlyMode
                ElseIf TypeOf (ctrl) Is RadioButton Then
                    CType(ctrl, RadioButton).Enabled = Not readOnlyMode
                ElseIf TypeOf (ctrl) Is CheckBox Then
                    CType(ctrl, CheckBox).Enabled = Not readOnlyMode
                End If

                Return 'not a container

            End If

            For Each ChildCtrl As Object In ParentCtrl.Controls
                ProcessSubControls(ChildCtrl, readOnlyMode, excludeButtons)
            Next

        Catch ex As Exception

                Throw
        Finally

        End Try

    End Sub

    Private Sub ClearDataBindings()

        For Each C As Control In grpEditPanel.Controls
            C.DataBindings.Clear()
        Next

        If _WaitBindingManagerBase IsNot Nothing Then
            RemoveHandler _WaitBindingManagerBase.PositionChanged, AddressOf WaitBindingManagerBase_PositionChanged
            RemoveHandler _WaitBindingManagerBase.CurrentChanged, AddressOf WaitBindingManagerBase_CurrentChanged
            _WaitBindingManagerBase.SuspendBinding()
        End If

        If WaitPeriodDataGrid IsNot Nothing Then
            WaitPeriodDataGrid.DataBindings.Clear()
            WaitPeriodDataGrid.DataMember = ""
            WaitPeriodDataGrid.DataSource = Nothing
        End If

    End Sub

    Private Sub WaitBindingManagerBase_CurrentChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    End Sub

    Private Sub WaitBindingManagerBase_PositionChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    End Sub

    Public Sub ClearAll()

        ClearDataBindings()

        If _TotalWaitPeriodDS IsNot Nothing Then _TotalWaitPeriodDS.Clear()

        _TotalWaitPeriodDS = Nothing

        WPDS.Clear()
        ClearErrors()
        txtfamilyid.Clear()
        txtrelationid.Clear()
        txtEligDt.Clear()

        SaveButton.Enabled = False
        CancelButton.Enabled = False
        btnAdd.Enabled = False
        btnModify.Enabled = False
        _ReadOnlyMode = False

        If _ChangedWaitPeriodRows IsNot Nothing Then _ChangedWaitPeriodRows.Clear()
        _ChangedWaitPeriodRows = Nothing
        If _DisplayColumnNamesDV IsNot Nothing Then _DisplayColumnNamesDV = Nothing

        If _WaitBindingManagerBase IsNot Nothing Then
            _WaitBindingManagerBase.SuspendBinding()
        End If

        _FamilyID = -1
        _RelationID = -1
        _PartSSNo = -1

    End Sub

    Private Sub AddWaitingPeriodLine()
        Dim DR As DataRow

        Try
            WPDS.EnforceConstraints = False
            DR = WPDS.WAITING_PERIOD.NewRow
            DR("SSNO") = _PartSSNo
            DR("CONSEC_MO_SW") = 0
            DR("CONSEC_MO_DATE") = "12-31-9999"
            DR("ELGQUAL_DATE") = "12-31-9999"
            DR("WAITPER_DATE") = DBNull.Value
            DR("WAIT_MO_CNTR") = 0
            DR("WAIT_MO_SW") = 0
            DR("USERID") = UFCWGeneral.DomainUser.ToUpper      ''BATCH_USERID
            DR("LASTUPDT") = CDate(Now)     ''BATCH_DATE
            DR("ONLNOPRID") = UFCWGeneral.DomainUser.ToUpper   '' ONLINE_USERID
            DR("ONLSTUPDT") = CDate(Now)  ''ONLINE_DATE
            DR("FAMILY_ID") = _FamilyID
            DR("RELATION_ID") = _RelationID
            DR("CREATE_USERID") = UFCWGeneral.DomainUser.ToUpper     ''CREATE_USERID
            DR("CREATE_DATE") = CDate(Now)

            LoadwpDataBindings()
            WaitPeriodDataGrid.SetTableStyle()

            WPDS.WAITING_PERIOD.Rows.Add(DR)

            SaveButton.Enabled = True
            CancelButton.Enabled = True

            _WaitBindingManagerBase.Position = _WaitBindingManagerBase.Count - 1

        Catch ex As Exception


                Throw

        End Try
    End Sub

    Private Function VerifyWPChanges() As Boolean
        Try

            ClearErrors()

            If _WaitBindingManagerBase.Count > 0 AndAlso _WaitBindingManagerBase.Position > -1 Then

                Dim dr As DataRow = DirectCast(_WaitBindingManagerBase.Current, DataRowView).Row


                If IsDate(dr("WAITPER_DATE")) Then
                Else
                    ErrorProvider1.SetError(Me.txtEligDt, " First Eligibility Period is invalid (MM-dd-yyyy)")
                End If

                If txtEligDt.Text.Length > 0 Then
                    Dim dtfrom1 As Date = CDate(txtEligDt.Text)
                    If Not IsDBNull(dtfrom1) Then
                        Dim firstday As Date = dtfrom1.AddDays(-(DatePart(DateInterval.Day, dtfrom1) * 1.0 - 1))
                        If DateDiff(DateInterval.Day, firstday, CDate(dr("WAITPER_DATE"))) <> 0 Then
                            txtEligDt.Text = Format(firstday, "MM-dd-yyyy")
                            dr("WAITPER_DATE") = txtEligDt.Text
                        End If
                    End If
                End If

                If ErrorProviderErrorsList(ErrorProvider1).Length > 0 Then
                    Return True
                End If
                '' Return False
            End If
        Catch ex As Exception


                Throw

        End Try
        Return False
    End Function

    Public Function SaveWaitingPeriodchanges(ByRef Transaction As DbTransaction) As Boolean
        Dim HistSum As String = ""
        Dim HistDetail As String = ""


        Try
            _ChangedWaitPeriodRows = CType(WPDS.GetChanges(), WaitPeriodDS)

            For Each r As DataRow In _ChangedWaitPeriodRows.Tables("WAITING_PERIOD").Rows
                If r.RowState = DataRowState.Modified Then
                    HistSum = "WAITING PERIOD FOR FAMILYID: " & Me.FamilyID & " RELATIONID " & CStr(r("RELATION_ID")) & " WAS MODIFIED"
                    HistDetail = UFCWGeneral.DomainUser.ToUpper & " MODIFIED WAITING PERIOD VALUE " & Environment.NewLine &
                                                " OF RELATIONID: " & Me.RelationID.ToString & " THE MODIFICATIONS WERE: " & Environment.NewLine & IdentifyChanges(r)

                    RegMasterDAL.UpdateWaitingPeriod(CInt(r("FAMILY_ID")), CDate(r("WAITPER_DATE")), UFCWGeneral.DomainUser.ToUpper, Transaction)

                    RegMasterDAL.CreateRegHistory(Me.FamilyID, Me.RelationID, Nothing, Nothing, "WAITUPDATE", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, Transaction)


                ElseIf r.RowState = DataRowState.Added Then 'ADD

                    HistSum = "WAITING PERIOD VALUE OF FAMILYID: " & Me.FamilyID & " WAS ADDED"
                    HistDetail = UFCWGeneral.DomainUser.ToUpper & " ADDED THE WAITING PERIOD RECORD " & Environment.NewLine &
                                                " FOR FAMILYID: " & Me.FamilyID.ToString & " AND RELATIONID: " & Me.RelationID.ToString & Environment.NewLine & IdentifyChanges(r)


                    RegMasterDAL.AddWaitingPeriod(_PartSSNo, CInt(r("FAMILY_ID")), CInt(r("RELATION_ID")), CDate(r("WAITPER_DATE")), UFCWGeneral.DomainUser.ToUpper, Transaction)

                    RegMasterDAL.CreateRegHistory(Me.FamilyID, Me.RelationID, Nothing, Nothing, "WAITADD", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, Transaction)

                End If
            Next
            HistSum = ""
            HistDetail = ""
            Return True
        Catch ex As Exception


                Throw
            Return False
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
        ErrorProvider1.SetError(txtEligDt, "")
    End Sub

    Public Function PendingChanges() As Boolean
        Dim strchangesmade As String = ""
        Try
            _ChangedWaitPeriodRows = CType(WPDS.GetChanges(), WaitPeriodDS)
            If (Not IsNothing(_ChangedWaitPeriodRows)) AndAlso _ChangedWaitPeriodRows.Tables("WAITING_PERIOD").Rows.Count > 0 Then
                If (Not IsNothing(_ChangedWaitPeriodRows)) AndAlso (_ChangedWaitPeriodRows.Tables("WAITING_PERIOD").Rows.Count > 0) Then

                    For i = 0 To _ChangedWaitPeriodRows.Tables("WAITING_PERIOD").Rows.Count - 1
                        Dim dr As DataRow = _ChangedWaitPeriodRows.Tables("WAITING_PERIOD").Rows(i)
                        If dr.RowState <> DataRowState.Added Then
                            strchangesmade = IdentifyChanges(dr)
                            If strchangesmade.Length > 0 Then
                                Return True
                            ElseIf strchangesmade.Length = 0 Then
                                _ChangedWaitPeriodRows.Tables("WAITING_PERIOD").Clear()
                                _ChangedWaitPeriodRows = CType(WPDS.GetChanges(), WaitPeriodDS)
                            End If
                        ElseIf dr.RowState = DataRowState.Added Then
                            Return True
                        End If
                    Next
                End If
            End If
            '' Return False
        Catch ex As Exception

                Throw
        End Try
        Return False
    End Function

    Public Function IdentifyChanges(ByVal dr As DataRow, Optional originalDR As DataRow = Nothing) As String

        Dim ColumnNum As Integer = 0
        Dim DispName As String = ""
        Dim HistRow As String = ""
        Dim ColName As String = ""
        Dim NewVal As String
        Dim OrigVal As String

        Try

            For Each DGCS As DataGridColumnStyle In WaitPeriodDataGrid.GetCurrentTableStyle().GridColumnStyles

                DispName = DGCS.HeaderText
                ColName = DGCS.MappingName

                If dr.RowState = DataRowState.Added OrElse IsDBNull(dr(ColName, DataRowVersion.Original)) Then
                    OrigVal = "NULL"
                Else
                    OrigVal = UFCWGeneral.IsNullStringHandler(dr(ColName, DataRowVersion.Original), "").ToUpper.Trim
                End If

                If dr.RowState <> DataRowState.Added AndAlso (dr.RowState = DataRowState.Deleted OrElse IsDBNull(dr(ColName, DataRowVersion.Current))) Then
                    NewVal = "NULL"
                Else
                    NewVal = UFCWGeneral.IsNullStringHandler(dr(ColName, DataRowVersion.Current), "").ToUpper.Trim
                End If

                If DispName <> "" AndAlso (dr.RowState = DataRowState.Added OrElse Not dr(ColName, DataRowVersion.Current).Equals(dr(ColName, DataRowVersion.Original))) Then
                    HistRow &= DispName & " = " & NewVal & " (was '" & OrigVal & "') " & Environment.NewLine
                End If
            Next

            If String.Compare(HistRow, "Type =  (was '') " & vbCrLf & "") = 0 Then
                HistRow = ""
            End If

            Return HistRow

        Catch ex As Exception


                Throw
        End Try

    End Function

    Private Shared Function GetColumnDisplayName(ByVal ColumnName As String) As String
        Dim DispName As String = ""
        Try
            DispName = CType(ConfigurationManager.GetSection("DisplayColumnNames"), IDictionary)(ColumnName).ToString

            ''  Return DispName

        Catch ex As Exception


            Throw

        End Try
        Return DispName
    End Function

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

            'SetTimePickerValues(txtbox)

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
            Dim txtbox As TextBox = CType(DirectCast(sender, Binding).Control, TextBox)
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

#End Region

End Class
