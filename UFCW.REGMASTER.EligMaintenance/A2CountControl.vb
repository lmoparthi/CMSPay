Option Strict On

Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Data.Common
Imports DDTek.DB2
Imports System.Reflection

Public Class A2CountControl
    Inherits System.Windows.Forms.UserControl

#Region "Variables"

    Private _FamilyID As Integer = -1
    Private _RelationID As Integer = -1
    Private _PartSSNo As Integer = -1
    Private _APPKEY As String = "UFCW\RegMaster\"
    Private _ReadOnlyMode As Boolean = False
    Private _DisplayColumnNames As DataView
    Private _ChangedA2Rows As A2countDS

    'Private _A2CountBS As BindingManagerBase
    Private WithEvents _A2CountBS As BindingSource
    Private _A2OverrideReasonsBS As BindingSource

    Private _TotalA2countDS As New A2countDS
    Public Shared _A2CountOverrideDT As New DataTable
    Private _Memtype As String

    ReadOnly _REGMSupervisor As Boolean = UFCWGeneralAD.REGMSupervisor

#End Region

    Private _Disposed As Boolean = False
    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.
            '
            If _ChangedA2Rows IsNot Nothing Then
                _ChangedA2Rows.Dispose()
            End If
            _ChangedA2Rows = Nothing

            If _A2CountOverrideDT IsNot Nothing Then
                _A2CountOverrideDT.Dispose()
            End If
            _A2CountOverrideDT = Nothing

            If _TotalA2countDS IsNot Nothing Then
                _TotalA2countDS.Dispose()
            End If
            _TotalA2countDS = Nothing

            If A2CountDataGrid IsNot Nothing Then
                A2CountDataGrid.Dispose()
            End If
            A2CountDataGrid = Nothing

            If _A2CountBS IsNot Nothing Then
                _A2CountBS.Dispose()
            End If
            _A2CountBS = Nothing

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

    'Public Property A2countDataset() As DataSet
    '    Get
    '        Return _TotalA2countDS
    '    End Get
    '    Set(ByVal Value As DataSet)
    '        _TotalA2countDS = Value
    '    End Set
    'End Property

#End Region

#Region "Constructor"
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Dim designMode As Boolean = (LicenseManager.UsageMode = LicenseUsageMode.Designtime)
        If Not designMode Then
            LoadA2CountOverideReasons()
        End If

        'dont want to display the default table style
    End Sub

    Public Sub New(ByVal familyID As Integer, ByVal relationID As Integer, ByVal ssn As Integer, Optional ByVal readonlymode As Boolean = False)
        Me.New()
        _FamilyID = familyID
        _RelationID = relationID
        _ReadOnlyMode = readonlymode
        _PartSSNo = ssn

        controlGroupBox.Enabled = False

    End Sub
#End Region

#Region "Form\Button Events"

    Private Sub btnAdd_Click(sender As System.Object, e As System.EventArgs) Handles AddActionButton.Click

        Dim EligCalcDS As DataSet
        Dim EligCalcDT As New DataTable

        Try
            If Not _REGMSupervisor Then                 '' Few people have access to add a2count
                MessageBox.Show("You are not authorized to add A2count.", "Access Restricted", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Return
            End If

            EligCalcDS = RegMasterDAL.RetrieveEligCalcElementsByFamilyID(_FamilyID)  '' cheking if there is a row in elg calc elements table

            EligCalcDT = EligCalcDS.Tables("ELGCALC_ELEMENTS")

            If EligCalcDT Is Nothing AndAlso EligCalcDT.Rows.Count = 0 Then
                MessageBox.Show("There is no row existing in Elig Calc Elements table. " & Environment.NewLine & "Please insert row before continuing", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            If _TotalA2countDS IsNot Nothing AndAlso _TotalA2countDS.Tables("MEMBER_A2BKCNT").Rows.Count = 0 Then
                MessageBox.Show("Participant is not Plan 110 ", "PlanType", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            _Memtype = CStr(EligCalcDT.Rows(0)("LAST_MEMTYPE"))

            controlGroupBox.Enabled = True

            cmbReasons.SelectedIndex = 0
            cmbReasons.Enabled = True

            rsStartHours.Enabled = True
            rbA2Count.Enabled = True


            SaveActionButton.Enabled = True
            CancelActionButton.Enabled = True
            ModifyActionButton.Enabled = False
            AddActionButton.Enabled = False

            grpStart.Enabled = True
            txtStartHoursDate.ReadOnly = False
            txtStartHoursDate.Enabled = True

            lblACAElgDate.Enabled = False
            txtACAEligDate.Enabled = False
            AddA2CountLine()

        Catch ex As Exception


                Throw

        Finally
            '' A2countDataGrid.Select(_A2cntBindingManagerBase.Position)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles CancelActionButton.Click
        Dim Result As DialogResult = DialogResult.None

        Try
            _ChangedA2Rows = CType(_A2DS.GetChanges(), A2countDS)

            Result = MessageBox.Show(Me, "Do you want to Cancel the changes?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If Result = DialogResult.Yes Then

                _A2DS.Tables("A2BKCNT").RejectChanges()

                ClearErrors()

                _A2CountBS.ResetBindings(False)

                controlGroupBox.Enabled = False
                CancelActionButton.Enabled = False
                SaveActionButton.Enabled = False

                cmbReasons.SelectedIndex = -1

                lblACAElgDate.Visible = False
                txtACAEligDate.Visible = False
                ModifyActionButton.Enabled = False
                AddActionButton.Enabled = True
                rsStartHours.Enabled = True
                rbA2Count.Enabled = True

            ElseIf Result = DialogResult.No Then

                CancelActionButton.Enabled = True
                SaveActionButton.Enabled = True

            End If

        Catch ex As Exception

            Throw

        Finally

            If _A2CountBS IsNot Nothing AndAlso _A2CountBS.Count > 0 Then
                _A2CountBS.Position = 0
                A2CountDataGrid.Select(_A2CountBS.Position)
            End If

            If _ChangedA2Rows Is Nothing Then
                Debug.Print("Rows Changed: 0")
            Else
                Debug.Print("Rows Changed: " & _ChangedA2Rows.Tables(0).Rows.Count)
            End If

        End Try
    End Sub

    Private Sub SaveButton_Click(sender As System.Object, e As System.EventArgs) Handles SaveActionButton.Click

        Try
            SaveActionButton.Enabled = False
            CancelActionButton.Enabled = False

            _A2CountBS.EndEdit()

            If VerifyA2countChanges() Then
                SaveActionButton.Enabled = True
                CancelActionButton.Enabled = True
                Exit Sub
            End If

            _ChangedA2Rows = CType(_A2DS.GetChanges(), A2countDS)

            If _ChangedA2Rows Is Nothing Then     '' when new row added,deleted then cancel, save buttons are enabled
                MessageBox.Show("There are no changes to the record.", "No Changes", MessageBoxButtons.OK, MessageBoxIcon.Information)
                SaveActionButton.Enabled = False
                CancelActionButton.Enabled = False
                Exit Sub
            End If

            If _ChangedA2Rows IsNot Nothing Then

                If SaveA2CountChanges() Then
                    MessageBox.Show("A2count Record Saved Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    _TotalA2countDS = Nothing

                    _A2DS.Clear()

                    LoadA2Count()

                    '' calculate eligibilty for the current elig period

                    ''eligcalc = New CalculateEligibility(_FamilyID, CDate(_GlobalEligPeriod))

                    ''If eligcalc.RetrieveRowStatusbeforeEligcalculation(_FamilyID, CDate(_GlobalEligPeriod)) Then
                    ''    frmwait.Show()
                    ''    frmwait.Activate()
                    ''    Cursor.Current = Cursors.WaitCursor
                    ''    Application.DoEvents()
                    ''    eligcalc.determineEligibility(_FamilyID, CDate(_GlobalEligPeriod))

                    ''    '' Message to user to refresh eligiblity tab
                    ''    MessageBox.Show("Please refresh Eligibility Tab to see these changes. ", "Refresh Eligibility", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ''    Cursor.Current = Cursors.Default
                    ''    frmwait.Hide()
                    ''End If

                    '' Message to user to calculate eligiblity 
                    MessageBox.Show("Eligibility Recalculation is required. ", "Calculate Eligibility", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RegMasterDAL.MadeDBChanges = True  '' this is to get elig_acct_hours from database to show pending/ or not

                End If

            End If

        Catch ex As Exception

            SaveActionButton.Enabled = True
            CancelActionButton.Enabled = True


                Throw

        Finally

            If ErrorProviderErrorsList(ErrorProvider1).Length = 0 AndAlso _A2CountBS.Count > 0 Then
                _A2CountBS.Position = 0
                A2CountDataGrid.Select(_A2CountBS.Position)
            End If

        End Try

    End Sub

    Private Sub btnModify_Click(sender As Object, e As EventArgs) Handles ModifyActionButton.Click
        Try


            If (Not IsNothing(_TotalA2countDS) AndAlso _TotalA2countDS.Tables("MEMBER_A2BKCNT").Rows.Count = 0) Then

                MessageBox.Show("Participant is not Plan 110 ", "PlanType", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            If _A2CountBS.Position < 0 Then Return

            controlGroupBox.Enabled = True

            ModifyActionButton.Enabled = False
            rsStartHours.Checked = True
            grpStart.Enabled = True
            grpA2cnt.Enabled = False
            SaveActionButton.Enabled = True
            CancelActionButton.Enabled = True

            lblACAElgDate.Enabled = True
            txtACAEligDate.Enabled = True

        Catch ex As Exception


                Throw

        Finally
        End Try

    End Sub

    Private Sub rbstarthours_CheckedChanged(sender As Object, e As EventArgs) Handles rsStartHours.CheckedChanged

        grpStart.Enabled = True
        grpA2cnt.Enabled = False
        rbA2Count.Enabled = True
        txtA2count.Clear()
    End Sub

    Private Sub rbA2count_CheckedChanged(sender As Object, e As EventArgs) Handles rbA2Count.CheckedChanged
        txtStartHoursDate.Enabled = True
        grpStart.Enabled = False
        txtStartHoursDate.Clear()
        grpA2cnt.Enabled = True
        rsStartHours.Enabled = True
        txtA2count.Enabled = True
        txtA2count.ReadOnly = False
    End Sub

    Private Sub txtstarthours_Leave(sender As Object, e As EventArgs) Handles txtStartHoursDate.Leave
        If _A2CountBS Is Nothing OrElse _A2CountBS.Position < 0 Then Exit Sub

        Dim dr As DataRow = DirectCast(_A2CountBS.Current, DataRowView).Row

        Try

            ''If txtstarthours.Text.Length = 0 OrElse Not IsDate(CDate(txtstarthours.Text)) Then
            ''    MessageBox.Show("Please enter valid Start of consecutive worked hours ", "Valid Date", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ''    txtstarthours.Focus()
            ''    Exit Sub
            ''End If

        Catch ex As Exception
        End Try
    End Sub

    Private Sub txtstarthours_LostFocus(sender As Object, e As EventArgs) Handles txtStartHoursDate.LostFocus
        If _A2CountBS Is Nothing OrElse _A2CountBS.Position < 0 Then Exit Sub

        Dim dr As DataRow = DirectCast(_A2CountBS.Current, DataRowView).Row

        Try

            If txtStartHoursDate.Text.Length > 0 AndAlso IsDate(txtStartHoursDate.Text) Then


                '' date is always start of the month.

                Dim dtstartdate = CDate(txtStartHoursDate.Text)
                Dim startday As Date = dtstartdate.AddDays(-(DatePart(DateInterval.Day, dtstartdate) * 1.0 - 1))
                If DateDiff(DateInterval.Day, startday, CDate(dr("OLDESTHRS"))) <> 0 Then
                    txtStartHoursDate.Text = Format(startday, "MM-dd-yyyy")
                    dr("OLDESTHRS") = txtStartHoursDate.Text
                End If


                '' 
                If Not IsDBNull(dr("OLDESTHRS")) Then
                    If CDate(dr("OLDESTHRS")) > RegMasterDAL.GlobalEligPeriod.AddMonths(-2) Then
                        MessageBox.Show("Start of consecutive worked hours  is invalid", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        txtStartHoursDate.Clear()
                        txtStartHoursDate.Focus()
                        Exit Sub
                    End If
                End If



                '' date should not be future
                If Not IsDBNull(dr("OLDESTHRS")) Then
                    If DateDiff("d", Now.Date, dr("OLDESTHRS")) > 0 Then
                        MessageBox.Show("Start of consecutive worked hours  should not be Future date", "Future Date", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        txtStartHoursDate.Clear()
                        txtStartHoursDate.Focus()
                        Exit Sub
                    End If
                End If

                '' get the eligibility period 

                ''Dim dtfrom As Date = Globaleligperiod
                ''Dim startdayofmonth As Date = dtfrom.AddDays(-(DatePart(DateInterval.Day, dtfrom) * 1.0 - 1))

                '' get the no of months
                Dim NumOfMonths As Integer = MonthDifference(startday, RegMasterDAL.GlobalEligPeriod) + 1
                txtA2count.Text = NumOfMonths.ToString
                If NumOfMonths < 3 Then
                    dr("A2COUNT") = 0
                Else
                    dr("A2COUNT") = txtA2count.Text
                End If

            End If
        Catch ex As Exception

                Throw
        End Try
    End Sub

    Private Sub txtA2count_LostFocus(sender As Object, e As EventArgs) Handles txtA2count.LostFocus

        If _A2CountBS Is Nothing OrElse _A2CountBS.Position < 0 Then Exit Sub

        Dim dr As DataRow = DirectCast(_A2CountBS.Current, DataRowView).Row

        Try

            If AddActionButton.Enabled = False Then  '' only adding new row
                If txtA2count.Text.Length > 0 Then
                    If CInt(txtA2count.Text) < 3 Then
                        MessageBox.Show("A2count value is invalid", "A2count", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        txtA2count.Clear()
                        txtA2count.Focus()
                        Return
                    End If
                End If
            ElseIf ModifyActionButton.Enabled = False Then
                '' for update button

                If txtA2count.Text.Length > 0 Then
                    If CInt(txtA2count.Text) >= 0 Then
                        If CInt(txtA2count.Text) = 0 Then
                            dr("A2COUNT") = 0
                        ElseIf CInt(txtA2count.Text) > 0 AndAlso CInt(txtA2count.Text) < 3 Then
                            dr("A2COUNT") = 0
                            MessageBox.Show("A2count value is invalid", "A2count", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            txtA2count.Clear()
                            txtA2count.Focus()
                            Exit Sub
                        Else
                            dr("A2COUNT") = txtA2count.Text

                            '' A2COUNT = MONTHS_BETWEEN (CURRENT ELIGBILITY PERIOD - START OF CONSECUTIVE DATE ) + 1 MONTH

                            txtStartHoursDate.Text = Format(RegMasterDAL.GlobalEligPeriod.AddMonths(-(CInt(txtA2count.Text))).AddMonths(1), "MM-dd-yyyy")

                            dr("OLDESTHRS") = txtStartHoursDate.Text

                        End If
                    End If
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub txtstarthours_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtStartHoursDate.KeyPress

        If Char.IsDigit(e.KeyChar) OrElse (Microsoft.VisualBasic.Asc(e.KeyChar) = 45) OrElse Char.IsControl(e.KeyChar) Then
            'do nothing
        Else
            e.Handled = True
            MsgBox("Please enter Date", MsgBoxStyle.Information, "Verify")
            txtStartHoursDate.Focus()
        End If

    End Sub

    Private Sub txtA2count_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtA2count.KeyPress

        If Char.IsDigit(e.KeyChar) OrElse (Microsoft.VisualBasic.Asc(e.KeyChar) = 45) OrElse Char.IsControl(e.KeyChar) Then
            'do nothing
        Else
            e.Handled = True
            MsgBox("Please enter Count", MsgBoxStyle.Information, "Verify")
            txtA2count.Focus()
        End If

    End Sub

    Private Sub cmbreason_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbReasons.SelectedIndexChanged
        If cmbReasons.SelectedIndex < 0 Then Return

        Dim DR As DataRow

        Try

            If _A2CountBS IsNot Nothing AndAlso _A2CountBS.Count > 0 AndAlso CType(sender, ComboBox).SelectedIndex > -1 Then
                DR = DirectCast(_A2CountBS.Current, DataRowView).Row

                If IsDBNull(DR("OVRIDE_RSNCD")) OrElse (Not IsDBNull(DR("OVRIDE_RSNCD")) AndAlso Not DR("OVRIDE_RSNCD").ToString.Equals(If(CType(sender, ComboBox).SelectedValue Is Nothing, DBNull.Value, CType(sender, ComboBox).SelectedValue).ToString)) Then
                    DR("OVRIDE_RSNCD") = If(CType(sender, ComboBox).SelectedValue Is Nothing, DBNull.Value, CType(sender, ComboBox).SelectedValue)
                End If

                If DR IsNot Nothing AndAlso CStr(DR("OVRIDE_RSNCD")) = "A" Then
                    lblACAElgDate.Visible = True
                    txtACAEligDate.Visible = True
                    If txtACAEligDate.Text = "12-31-9999" Then txtACAEligDate.Text = ""
                Else
                    lblACAElgDate.Visible = False
                    txtACAEligDate.Visible = False
                End If

            End If


        Catch ex As Exception

                Throw
        Finally
        End Try
    End Sub

    Private Sub A2CountControl_dispose(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        Me.Dispose()
    End Sub

    Private Sub txtACAEligdt_LostFocus(sender As Object, e As EventArgs) Handles txtACAEligDate.LostFocus

        If _A2CountBS Is Nothing OrElse _A2CountBS.Position = -1 Then Exit Sub

        If _A2CountBS.Position < 0 Then Exit Sub

        Dim DR As DataRow = DirectCast(_A2CountBS.Current, DataRowView).Row

        Try

            If txtACAEligDate.Text.Length > 0 AndAlso IsDate(txtACAEligDate.Text) Then


                '' date is always start of the month.

                Dim dtstartdate = CDate(txtACAEligDate.Text)
                Dim startday As Date = dtstartdate.AddDays(-(DatePart(DateInterval.Day, dtstartdate) * 1.0 - 1))
                If DateDiff(DateInterval.Day, startday, CDate(DR("ACA_HLPR_ELIG_DATE"))) <> 0 Then
                    txtACAEligDate.Text = Format(startday, "MM-dd-yyyy")
                    DR("ACA_HLPR_ELIG_DATE") = txtACAEligDate.Text
                End If

                '' date should not be future
                If Not IsDBNull(DR("ACA_HLPR_ELIG_DATE")) Then
                    If DateDiff("d", RegMasterDAL.GlobalEligPeriod, DR("ACA_HLPR_ELIG_DATE")) > 0 Then
                        MessageBox.Show("ACA Helper elig date should not be Future date", "Future Date", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        txtACAEligDate.Clear()
                        txtACAEligDate.Focus()
                    End If
                End If
            End If
        Catch ex As Exception

                Throw
        End Try
    End Sub

#End Region

#Region "Custom Subs\Functions"

    Public Sub LoadA2Count(ByVal familyID As Integer, ByVal relationID As Integer, ByVal ssn As Integer)

        _FamilyID = familyID
        _ReadOnlyMode = ReadOnlyMode
        _RelationID = 0
        _PartSSNo = ssn

        LoadA2Count()

    End Sub

    Public Sub LoadA2Count()

        Try

            ClearErrors()
            ClearDataBindings()

            If _TotalA2countDS IsNot Nothing Then
                If _TotalA2countDS.Tables("A2BKCNT").Rows.Count > 0 Then
                    _A2DS.Tables("A2BKCNT").Rows.Clear()
                    _A2DS = CType(_TotalA2countDS.Copy, A2countDS)
                End If
            End If

            If _A2DS.Tables("A2BKCNT").Rows.Count = 0 Then  '' only retrieve data for first time
                _A2DS = CType(RegMasterDAL.RetrieveA2CountByFamilyid(_FamilyID, _A2DS), A2countDS)
                _TotalA2countDS = CType(_A2DS.Copy, A2countDS)
            End If

            _A2CountBS = New BindingSource
            _A2CountBS.DataSource = _A2DS.Tables("A2BKCNT")

            A2CountDataGrid.DataSource = _A2CountBS
            A2CountDataGrid.SetTableStyle()
            A2CountDataGrid.Sort = If(A2CountDataGrid.LastSortedBy, A2CountDataGrid.DefaultSort)

            LoadA2CountDataBindings()
            ProcessControls(Me, _ReadOnlyMode, False)

            SetNonBoundItems()

            If Not _ReadOnlyMode AndAlso _A2DS.Tables("A2BKCNT").Rows.Count < 1 Then
                ProcessControls(Me, True, True) ' disable UI Input elements until a row is added
            End If

            controlGroupBox.Enabled = False

            If _A2DS.A2BKCNT.Rows.Count = 0 Then
                AddActionButton.Visible = True
            Else
                AddActionButton.Visible = False
            End If

        Catch ex As Exception

                Throw
        End Try
    End Sub

    Private Sub LoadA2CountDataBindings()
        Dim Bind As Binding

        Try

            RemoveHandler cmbReasons.SelectedIndexChanged, AddressOf cmbreason_SelectedIndexChanged

            txtA2count.DataBindings.Clear()
            Bind = New Binding("Text", _A2CountBS, "A2COUNT", True)
            txtA2count.DataBindings.Add(Bind)

            txtStartHoursDate.DataBindings.Clear()
            Bind = New Binding("Text", _A2CountBS, "OLDESTHRS", True)
            AddHandler Bind.Format, AddressOf DateOnlyBinding_Format
            Bind.FormatString = "MM-dd-yyyy"
            txtStartHoursDate.DataBindings.Add(Bind)

            txtACAEligDate.DataBindings.Clear()
            Bind = New Binding("Text", _A2CountBS, "ACA_HLPR_ELIG_DATE", True)
            AddHandler Bind.Format, AddressOf DateOnlyBinding_Format
            Bind.FormatString = "MM-dd-yyyy"
            txtACAEligDate.DataBindings.Add(Bind)

            cmbReasons.DataBindings.Clear()
            Bind = New Binding("SelectedValue", _A2CountBS, "OVRIDE_RSNCD", True)
            cmbReasons.DataBindings.Add(Bind)

        Catch ex As Exception


                Throw

        Finally

            AddHandler cmbReasons.SelectedIndexChanged, AddressOf cmbreason_SelectedIndexChanged

        End Try

    End Sub

    Public Sub ProcessControls(ByRef parentCtrl As Object, ByVal readOnlyMode As Boolean, Optional ByVal excludeButtons As Boolean = False)

        Dim Ctrl As Control

        Try
            If parentCtrl Is Nothing Then Return

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
                Me.CancelActionButton.Enabled = False
                Me.SaveActionButton.Enabled = False
            End If

            If Not _ReadOnlyMode Then
                If _FamilyID <> -1 And _RelationID <> -1 Then
                    CancelActionButton.Enabled = False
                    If _A2DS.Tables("A2BKCNT").Rows.Count > 0 Then  ''To disable buttons in un modifiable state
                        ModifyActionButton.Visible = True
                        ModifyActionButton.Enabled = True
                        AddActionButton.Visible = False
                    ElseIf _A2DS.Tables("A2BKCNT").Rows.Count = 0 Then
                        ModifyActionButton.Visible = False
                        AddActionButton.Visible = True
                        AddActionButton.Enabled = True
                        controlGroupBox.Enabled = False
                    End If
                End If
            Else
                ModifyActionButton.Visible = False
                ModifyActionButton.Enabled = False
                CancelActionButton.Enabled = False
                SaveActionButton.Enabled = False
                AddActionButton.Visible = False
            End If
        Catch ex As Exception

                Throw
        End Try

    End Sub

    Public Shared Sub ProcessSubControls(ByRef ctrl As Object, ByVal readOnlyMode As Boolean, Optional ByVal excludeButtons As Boolean = False)

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
    Private Sub AddA2CountLine()
        Dim DR As DataRow

        Try
            _A2DS.EnforceConstraints = False

            DR = _A2DS.A2BKCNT.NewRow
            DR.BeginEdit()

            DR("SSNO") = _PartSSNo
            DR("WRKPERIOD") = "12-31-9999"
            DR("A2COUNT") = 0
            DR("MEMTYPE") = _Memtype
            DR("OLDESTHRS") = DBNull.Value
            DR("PARDTHRS") = "12-31-9999"
            DR("PARELG") = "12-31-9999"

            DR("DEPDTHRS") = "12-31-9999"
            DR("DEPELG") = "12-31-9999"

            DR("LASTUPDT") = CDate(Now)     ''BATCH_DATE
            DR("USERID") = UFCWGeneral.DomainUser.ToUpper      ''BATCH_USERID

            DR("ONLNOPRID") = "ONLINE"   '' ONLINE_USERID
            DR("ONLSTUPDT") = CDate(Now)  ''ONLINE_DATE

            DR("INSTELGSW") = DBNull.Value
            DR("OVRIDE_RSNCD") = DBNull.Value
            DR("CNT_AT_BREAK") = DBNull.Value
            DR("BREAK_DATE") = CDate(Now)

            DR("FAMILY_ID") = _FamilyID
            DR("RELATION_ID") = 0

            DR("CREATE_USERID") = UFCWGeneral.DomainUser.ToUpper
            DR("CREATE_DATE") = CDate(Now)
            DR("ONLINE_USERID") = UFCWGeneral.DomainUser.ToUpper
            DR("ONLINE_DATE") = CDate(Now)
            DR("ACA_HLPR_ELIG_DATE") = "12-31-9999"

            DR.EndEdit()

            _A2DS.A2BKCNT.Rows.Add(DR)

            SaveActionButton.Enabled = True
            CancelActionButton.Enabled = True

        Catch ex As Exception


                Throw

        End Try
    End Sub

    Private Sub ClearDataBindings()

        For Each C As Control In grpEditPanel.Controls
            C.DataBindings.Clear()
        Next
        For Each C As Control In grpStart.Controls
            C.DataBindings.Clear()
        Next
        For Each C As Control In grpA2cnt.Controls
            C.DataBindings.Clear()
        Next

        If A2CountDataGrid IsNot Nothing Then
            A2CountDataGrid.DataBindings.Clear()
            A2CountDataGrid.DataMember = ""
            A2CountDataGrid.DataSource = Nothing
        End If

    End Sub

    Private Sub _A2CountBindingManagerBase_CurrentChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        SetNonBoundItems()
    End Sub

    Public Sub ClearAll()

        ClearDataBindings()

        If A2CountDataGrid.DataSource IsNot Nothing AndAlso A2CountDataGrid.DataSource IsNot Nothing Then
            A2CountDataGrid.DataSource = Nothing
        End If

        If _TotalA2countDS IsNot Nothing Then _TotalA2countDS.Clear()
        _TotalA2countDS = Nothing
        If _TotalA2countDS IsNot Nothing Then
            A2CountDataGrid.DataMember = ""
            A2CountDataGrid.DataSource = Nothing
        End If

        _A2DS.Clear()
        ClearErrors()
        txtStartHoursDate.Clear()
        txtA2count.Clear()


        SaveActionButton.Enabled = False
        CancelActionButton.Enabled = False
        AddActionButton.Visible = False
        ModifyActionButton.Enabled = False
        _ReadOnlyMode = False

        If _ChangedA2Rows IsNot Nothing Then _ChangedA2Rows.Clear()
        _ChangedA2Rows = Nothing
        If _DisplayColumnNames IsNot Nothing Then _DisplayColumnNames = Nothing

        If _A2CountBS IsNot Nothing Then
            _A2CountBS.SuspendBinding()
        End If

        _FamilyID = -1
        _RelationID = -1
        _PartSSNo = -1
        _Memtype = ""
    End Sub

    Private Function VerifyA2countChanges() As Boolean
        Try

            ClearErrors()

            If _A2CountBS.Count > 0 AndAlso _A2CountBS.Position > -1 Then

                Dim dr As DataRow = DirectCast(_A2CountBS.Current, DataRowView).Row

                If rsStartHours.Checked Then
                    If Not IsDBNull(dr("OLDESTHRS")) Then
                    Else
                        ErrorProvider1.SetError(Me.txtStartHoursDate, " Start of consecutive worked hours should not be Empty")
                    End If


                    If Not IsDBNull(dr("OLDESTHRS")) Then
                        If DateDiff("d", Now.Date, dr("OLDESTHRS")) > 0 Then
                            ErrorProvider1.SetError(Me.txtStartHoursDate, " Start of Consecutive worked hours should not be Future date")
                        End If
                    End If
                End If

                If txtStartHoursDate.Text.Length > 0 AndAlso IsDate(dr("OLDESTHRS")) Then
                    Dim dtstartdate = CDate(txtStartHoursDate.Text)
                    Dim startday As Date = dtstartdate.AddDays(-(DatePart(DateInterval.Day, dtstartdate) * 1.0 - 1))
                    If rsStartHours.Checked Then
                        ''Dim dtstartdate = CDate(txtstarthours.Text)
                        ''Dim startday As Date = dtstartdate.AddDays(-(DatePart(DateInterval.Day, dtstartdate) * 1.0 - 1))
                        Dim NumOfMonths As Integer = MonthDifference(startday, RegMasterDAL.GlobalEligPeriod) + 1
                        txtA2count.Text = NumOfMonths.ToString
                        If NumOfMonths < 3 Then
                            dr("A2COUNT") = 0
                        Else
                            dr("A2COUNT") = txtA2count.Text
                        End If
                        dr("OLDESTHRS") = startday
                    ElseIf rbA2Count.Checked Then
                        If CInt(txtA2count.Text) = 0 Then
                            dr("A2COUNT") = 0
                            '' dr("OLDESTHRS") = 
                        ElseIf CInt(txtA2count.Text) > 0 AndAlso CInt(txtA2count.Text) < 3 Then
                            dr("A2COUNT") = 0
                            txtStartHoursDate.Text = Format(RegMasterDAL.GlobalEligPeriod.AddMonths(-(CInt(txtA2count.Text))).AddMonths(1), "MM-dd-yyyy")
                            dr("OLDESTHRS") = txtStartHoursDate.Text
                        Else
                            dr("A2COUNT") = txtA2count.Text
                            txtStartHoursDate.Text = Format(RegMasterDAL.GlobalEligPeriod.AddMonths(-(CInt(txtA2count.Text))).AddMonths(1), "MM-dd-yyyy")
                            dr("OLDESTHRS") = txtStartHoursDate.Text
                        End If
                    End If
                    A2CountDataGrid.Refresh()
                End If

                If Not cmbReasons.SelectedIndex = 0 Then
                Else
                    ErrorProvider1.SetError(Me.cmbReasons, " Please Provide Override Reason")
                End If


                If IsDate(dr("OLDESTHRS")) Then

                    If Not IsDBNull(dr("OVRIDE_RSNCD")) AndAlso CStr(dr("OVRIDE_RSNCD")) = "A" Then

                        ''ACA helper date is required
                        If Not IsDBNull(dr("ACA_HLPR_ELIG_DATE")) Then
                        Else
                            ErrorProvider1.SetError(Me.txtACAEligDate, " Please provide ACA Helper Eligibility date")
                        End If

                        ''User needs to enter a valid date and should not 12-31-9999

                        If Not IsDBNull(dr("ACA_HLPR_ELIG_DATE")) AndAlso Format(dr("ACA_HLPR_ELIG_DATE"), "MM-dd-yyyy") <> "12-31-9999" Then
                        Else
                            ErrorProvider1.SetError(Me.txtACAEligDate, " Please provide valid ACA Helper Eligibility date")
                        End If

                        ''ACA helper Date is later than than start of consecutive hours date
                        If IsDate(dr("OLDESTHRS")) AndAlso IsDate(dr("ACA_HLPR_ELIG_DATE")) Then
                            If (CDate(dr("OLDESTHRS")) < CDate(dr("ACA_HLPR_ELIG_DATE"))) Then
                            Else
                                txtACAEligDate.Focus()
                                ErrorProvider1.SetError(Me.txtACAEligDate, "ACA Helper Eligibility date should be later than  Start of Consecutive hours")
                            End If
                        End If

                        '' date should not be future eligibility
                        If Not IsDBNull(dr("ACA_HLPR_ELIG_DATE")) Then
                            If DateDiff("d", RegMasterDAL.GlobalEligPeriod, dr("ACA_HLPR_ELIG_DATE")) > 0 Then
                                ErrorProvider1.SetError(Me.txtACAEligDate, " ACA Helper elig date should not be Future date")
                            End If
                        End If

                        ''  a2 count is 14 or gretaer than that
                        If MonthDifference(CDate(dr("OLDESTHRS")), CDate(dr("ACA_HLPR_ELIG_DATE"))) + 1 < 14 Then
                            ErrorProvider1.SetError(Me.txtACAEligDate, "Difference between ACA Helper elig date and Start of Consecutive worked hours should not be less than 13 months")
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

    Public Function SaveA2CountChanges() As Boolean

        Dim HistSum As String = ""
        Dim HistDetail As String = ""
        Dim Transaction As DbTransaction

        Try
            Transaction = RegMasterDAL.BeginTransaction

            _ChangedA2Rows = CType(_A2DS.GetChanges(), A2countDS)

            For Each DR As DataRow In _ChangedA2Rows.Tables("A2BKCNT").Rows
                If DR.RowState = DataRowState.Modified Then
                    HistSum = "A2COUNT FOR FAMILYID: " & Me.FamilyID & " RELATIONID " & CStr(DR("RELATION_ID")) & " WAS MODIFIED"
                    HistDetail = UFCWGeneral.DomainUser.ToUpper & " MODIFIED A2COUNT RECORD VALUE " & Environment.NewLine &
                                                " OF RELATIONID: " & Me.RelationID.ToString & " THE MODIFICATIONS WERE: " & Environment.NewLine & IdentifyChanges(DR) & " Reason for Change is : " & CStr(cmbReasons.Text)

                    RegMasterDAL.UpdateA2Count(CInt(DR("FAMILY_ID")), CDate(DR("OLDESTHRS")), CInt(DR("A2COUNT")), CStr(cmbReasons.SelectedValue), UFCWGeneral.DomainUser.ToUpper, CDate(DR("ACA_HLPR_ELIG_DATE")), transaction)

                    RegMasterDAL.CreateRegHistory(Me.FamilyID, Me.RelationID, Nothing, Nothing, "A2COUNTUPDATE", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, transaction)


                ElseIf DR.RowState = DataRowState.Added Then 'ADD

                    HistSum = "A2COUNT VALUE OF FAMILYID: " & Me.FamilyID & " WAS ADDED"
                    HistDetail = UFCWGeneral.DomainUser.ToUpper & " ADDED THE A2COUNT RECORD " & Environment.NewLine &
                                                " FOR FAMILYID: " & Me.FamilyID.ToString & Environment.NewLine & IdentifyChanges(DR)


                    RegMasterDAL.AddA2count(_PartSSNo, CInt(DR("FAMILY_ID")), CInt(DR("RELATION_ID")), CDate(DR("OLDESTHRS")), CInt(DR("A2COUNT")), CStr(DR("OVRIDE_RSNCD")), CStr(DR("MEMTYPE")), UFCWGeneral.DomainUser.ToUpper, transaction)

                    RegMasterDAL.CreateRegHistory(Me.FamilyID, Me.RelationID, Nothing, Nothing, "A2COUNTADD", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, transaction)

                End If

                HistSum = ""
                HistDetail = ""

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


                Throw
        Finally
            If Transaction IsNot Nothing Then Transaction.Dispose()
            Transaction = Nothing

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
        ErrorProvider1.SetError(txtA2count, "")
        ErrorProvider1.SetError(txtStartHoursDate, "")
        ErrorProvider1.SetError(cmbReasons, "")
        ErrorProvider1.SetError(txtACAEligDate, "")
    End Sub

    Public Function PendingChanges() As Boolean
        Dim strchangesmade As String = ""
        Try
            _ChangedA2Rows = CType(_A2DS.GetChanges(), A2countDS)
            If (_ChangedA2Rows IsNot Nothing) AndAlso _ChangedA2Rows.Tables("A2BKCNT").Rows.Count > 0 Then
                If (_ChangedA2Rows IsNot Nothing) AndAlso (_ChangedA2Rows.Tables("A2BKCNT").Rows.Count > 0) Then

                    For i = 0 To _ChangedA2Rows.Tables("A2BKCNT").Rows.Count - 1
                        Dim dr As DataRow = _ChangedA2Rows.Tables("A2BKCNT").Rows(i)
                        If dr.RowState <> DataRowState.Added Then
                            strchangesmade = IdentifyChanges(dr)
                            If strchangesmade.Length > 0 Then
                                Return True
                            ElseIf strchangesmade.Length = 0 Then
                                _ChangedA2Rows.Tables("A2BKCNT").Rows.Remove(dr)
                                _ChangedA2Rows = CType(_A2DS.GetChanges(), A2countDS)
                            End If
                        ElseIf dr.RowState = DataRowState.Added Then
                            Return True
                        End If
                    Next
                End If
            End If
            ''  Return False
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

            For Each DGCS As DataGridColumnStyle In A2CountDataGrid.GetCurrentTableStyle().GridColumnStyles

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

    Public Shared Function MonthDifference(ByVal first As DateTime, ByVal second As DateTime) As Integer
        Return Math.Abs((first.Month - second.Month) + 12 * (first.Year - second.Year))
    End Function

    Public Sub LoadA2CountOverideReasons()

        Try
            If _A2CountOverrideDT Is Nothing OrElse _A2CountOverrideDT.Rows.Count < 1 Then
                _A2CountOverrideDT = RegMasterDAL.RetrieveA2countOverrideReasons
            End If

            _A2OverrideReasonsBS = New BindingSource
            _A2OverrideReasonsBS.DataSource = _A2CountOverrideDT
            _A2OverrideReasonsBS.Sort = "DESCRIPTION"

            cmbReasons.DataSource = _A2OverrideReasonsBS

            cmbReasons.ValueMember = "REASON_CODE"
            cmbReasons.DisplayMember = "DESCRIPTION"
            cmbReasons.SelectedIndex = -1

        Catch ex As Exception

                Throw

        Finally

        End Try
    End Sub

    Private Sub SetNonBoundItems()
        Dim DR As DataRow

        Try

            If _A2CountBS.Position > -1 Then
                DR = DirectCast(_A2CountBS.Current, DataRowView).Row

                If Not IsDBNull(DR("OVRIDE_RSNCD")) AndAlso DR("OVRIDE_RSNCD").ToString.Trim.Length > 0 Then
                    cmbReasons.SelectedValue = CStr(DR("OVRIDE_RSNCD"))
                End If
            End If

        Catch ex As Exception


                Throw
        Finally
        End Try
    End Sub

#End Region

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

    Private Sub A2CountBS_CurrentChanged(sender As Object, e As EventArgs) Handles _A2CountBS.CurrentChanged

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

    Private Sub _A2CountBS_CurrentItemChanged(sender As Object, e As EventArgs) Handles _A2CountBS.CurrentItemChanged

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

    Private Sub _A2CountBS_PositionChanged(sender As Object, e As EventArgs) Handles _A2CountBS.PositionChanged

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

    Private Sub A2CountBS_ListChanged(sender As Object, e As ListChangedEventArgs) Handles _A2CountBS.ListChanged

        Dim BS As BindingSource

        Try
            BS = CType(sender, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") RS(" & If(BS.Position < 0 ORELSE BS.Current Is Nothing, "N/A", DirectCast(BS.Current, System.Data.DataRowView).Row.RowState.ToString) & ") O(" & e.OldIndex.ToString & ") N(" & e.NewIndex.ToString & ") CT(" & e.ListChangedType.ToString & ") SEL(" & If(BS Is Nothing OrElse BS.Count < 1 OrElse BS.Position < 0 OrElse A2CountDataGrid.DataSource Is Nothing, "N/A", A2CountDataGrid.IsSelected(BS.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Select Case e.ListChangedType
                Case ListChangedType.ItemDeleted 'account for rows deleted due to a cancel action

                Case ListChangedType.ItemMoved

                    Select Case DirectCast(BS.Current, System.Data.DataRowView).Row.RowState
                        Case DataRowState.Modified

                        Case DataRowState.Added

                            If e.OldIndex <> e.NewIndex OrElse BS.Position > -1 AndAlso BS.Count = 1 Then
                                A2CountDataGrid.Select(e.NewIndex)
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

                    If BS.Position <> e.NewIndex OrElse BS.Position > -1 AndAlso BS.Count = 1 Then 'first item added
                        If e.NewIndex > -1 Then BS.Position = e.NewIndex
                        If e.NewIndex > -1 Then A2CountDataGrid.Select(e.NewIndex)
                    End If

            End Select

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") RS(" & If(BS.Position < 0 ORELSE BS.Current Is Nothing, "N/A", DirectCast(BS.Current, System.Data.DataRowView).Row.RowState.ToString) & ") O(" & e.OldIndex.ToString & ") N(" & e.NewIndex.ToString & ") CT(" & e.ListChangedType.ToString & ") SEL(" & If(BS Is Nothing OrElse BS.Count < 1 OrElse BS.Position < 0 OrElse A2CountDataGrid.DataSource Is Nothing, "N/A", A2CountDataGrid.IsSelected(BS.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub txtDate_Validating(sender As Object, e As CancelEventArgs) Handles txtACAEligDate.Validating, txtStartHoursDate.Validating

        Dim TBox As TextBox = CType(sender, TextBox)
        Dim HoldDate As Date?

        If _Disposed OrElse _A2CountBS Is Nothing OrElse _A2CountBS.Position < 0 OrElse TBox.ReadOnly Then Return

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _A2CountBS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ErrorProvider1.ClearError(TBox)

            If TBox.Text.Trim.Length < 1 Then
                SetErrorWithTracking(ErrorProvider1, TBox, "From date is required.")
            Else
                HoldDate = UFCWGeneral.ValidateDate(TBox.Text)
                If HoldDate Is Nothing Then
                    SetErrorWithTracking(ErrorProvider1, TBox, "Date format must be mm/dd/yyyy or mm-dd-yyyy.")
                Else

                    If TBox.Text <> CDate(HoldDate).ToString("MM-dd-yyyy") Then
                        TBox.Text = CDate(HoldDate).ToString("MM-dd-yyyy")
                    End If

                End If

            End If

            If ErrorProvider1.GetError(TBox).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
            End If

        Catch ex As Exception

                Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _A2CountBS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

End Class
