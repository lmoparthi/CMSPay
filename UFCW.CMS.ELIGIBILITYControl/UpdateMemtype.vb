Option Strict On
Imports System.Data.Common
Imports System.ComponentModel
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class UpdateMemtypeForm
#Region "Variables"

    Private _FamilyID As Integer
    Private _RelationID As Short
    Private _APPKEY As String = "UFCW\RegMaster\"
    Private _ReadOnlyMode As Boolean = True

    Private _SelectedEligPeriod As Date?
    ReadOnly _DomainUser As String = SystemInformation.UserName
    Private _ChangedDRs As EligMthdtlDS
    Private _TotalSADS As New EligMthdtlDS
    Private _MemTypeBS As BindingSource

    Private _UpdatedRecord As Boolean = False '' If any changes made to records this will help to load data for elighours tab
    Private _MemPlanDT As New DataTable
    Private _MemPlanBS As BindingSource

#End Region

    Private _Disposed As Boolean = False

    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.
            If MemTypeDataGrid IsNot Nothing Then
                MemTypeDataGrid.Dispose()
            End If
            MemTypeDataGrid = Nothing

            If _ChangedDRs IsNot Nothing Then
                _ChangedDRs.Dispose()
            End If
            _ChangedDRs = Nothing

            If _TotalSADS IsNot Nothing Then
                _TotalSADS.Dispose()
            End If
            _TotalSADS = Nothing

            If _MemPlanDT IsNot Nothing Then
                _MemPlanDT.Dispose()
            End If
            _MemPlanDT = Nothing

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
        Set(ByVal value As Integer)
            _FamilyID = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the RelationID of the Document.")>
    Public Property RelationID() As Short
        Get
            Return _RelationID
        End Get
        Set(ByVal value As Short)
            _RelationID = value
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

#End Region

#Region "Constructor"
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Dim designMode As Boolean = (LicenseManager.UsageMode = LicenseUsageMode.Designtime)
        If Not designMode Then
            LoadMemtypes()
        End If


        'dont want to display the default table style
    End Sub

    Public Sub New(ByVal familyID As Integer, ByVal readOnlyMode As Boolean, ByVal eligPeriod As Date?)

        Me.New()

        _FamilyID = familyID
        txtFamilyID.Text = familyID.ToString
        txtRelationID.Text = CStr(0)

        _TotalSADS.EnforceConstraints = False
        _SelectedEligPeriod = eligPeriod

        _ReadOnlyMode = readOnlyMode

        If _ReadOnlyMode Then
            ModifyActionButton.Visible = False
            CancelActionButton.Visible = False
            SaveActionButton.Visible = False

        Else

            ModifyActionButton.Visible = True
            CancelActionButton.Visible = True
            SaveActionButton.Visible = True
            CancelActionButton.Enabled = False
            SaveActionButton.Enabled = False

        End If
    End Sub

#End Region

#Region "Form\Button Events"

    Private Sub btnModify_Click(sender As System.Object, e As System.EventArgs) Handles ModifyActionButton.Click

        If _MemTypeBS.Position < 0 Then Return

        Try
            GroupBox2.Enabled = True

            ModifyActionButton.Enabled = False
            CancelActionButton.Enabled = True
            SaveActionButton.Enabled = True
            cmbMemType.ReadOnly = False

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles CancelActionButton.Click
        Dim Result As DialogResult = DialogResult.None

        Try
            Result = MessageBox.Show(Me, "Do you want to Cancel the changes?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If Result = DialogResult.Yes Then
                _MemTypeDS.Tables("ELIG_MTHDTL").RejectChanges()
                _MemTypeBS.EndEdit()

                ClearErrors()

                CancelActionButton.Enabled = False
                SaveActionButton.Enabled = False
                ModifyActionButton.Enabled = True

                cmbMemType.ReadOnly = True

                _MemTypeBS.ResetCurrentItem()

                MemTypeDataGrid.Focus()

            ElseIf Result = DialogResult.No Then
                CancelActionButton.Enabled = True
                SaveActionButton.Enabled = True
                cmbMemType.ReadOnly = False
            End If

        Catch ex As Exception

        Finally
        End Try
    End Sub

    Private Sub SaveButton_Click(sender As System.Object, e As System.EventArgs) Handles SaveActionButton.Click

        Dim Transaction As DbTransaction = Nothing
        Try
            SaveActionButton.Enabled = False
            CancelActionButton.Enabled = False
            cmbMemType.ReadOnly = True

            If Not Me.ValidateChildren(ValidationConstraints.Enabled) Then
                SaveActionButton.Enabled = True
                CancelActionButton.Enabled = True
                Return
            End If

            _MemTypeBS.EndEdit()

            _UpdatedRecord = False
            _ChangedDRs = CType(_MemTypeDS.GetChanges(), EligMthdtlDS)

            If _ChangedDRs IsNot Nothing Then
                Transaction = CMSDALCommon.BeginTransaction

                Try

                    If SaveMemTypeChanges(Transaction) Then

                        CMSDALCommon.CommitTransaction(Transaction)

                        MessageBox.Show("Memtype on Eligibility record Saved Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        _MemTypeDS.AcceptChanges()
                        _UpdatedRecord = True

                        _TotalSADS = _MemTypeDS

                        MemTypeDataGrid.Focus()

                        If _SelectedEligPeriod <= CMSDALFDBEL.GlobalEligPeriod Then
                            '' Message to user to calculate eligiblity for current an retro periods
                            MessageBox.Show("For this Memtype change Eligibility calculation is required. ", "Calculate Eligibility", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                        RegMasterDAL.MadeDBChanges = True  '' this is to get elig_acct_hours from database to show pending/ or not

                    Else
                        CMSDALCommon.RollbackTransaction(Transaction)
                        MessageBox.Show("Error while saving Memtype on Eligibility record." & vbCrLf & "Please try again ", "Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If

                Catch ex As Exception

                    Try

                        CMSDALCommon.RollbackTransaction(Transaction)

                    Catch ex2 As Exception

                    End Try

                    Throw

                End Try
            End If

        Catch ex As Exception

            SaveActionButton.Enabled = True
            CancelActionButton.Enabled = True

            Throw

        Finally
            If Transaction IsNot Nothing Then Transaction.Dispose()
            Transaction = Nothing
        End Try

    End Sub

    Private Sub UpdateMemtype_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        If _UpdatedRecord Then
            Me.DialogResult = System.Windows.Forms.DialogResult.Yes
        End If
        SaveSettings()
        Me.Dispose()
    End Sub

    Private Sub UpdateMemtype_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If Me.PendingChanges Then
                MessageBox.Show(Me, "Changes have been made to Modify Memtype screen." & vbCrLf &
                                             "Please Complete the changes before continuing", "Save changes", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)

                e.Cancel = True

            End If
        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub cmbmemtype_LostFocus(sender As Object, e As EventArgs)

        Dim DR As DataRow
        Dim Result As DialogResult = DialogResult.None
        Dim strfilter As String = ""
        Dim RetireeMEMValues As DataTable
        Dim SelectedRetireeDV As DataView
        Dim EligRetireeDV As DataView
        Dim RetireeAcctsDV As DataView

        Try

            DR = DirectCast(_MemTypeBS.Current, DataRowView).Row

            If Not IsDBNull(DR("MEMTYPE")) AndAlso cmbMemType.SelectedIndex > -1 Then

                '' This code is for when user select retiree MEMTYPE we need to check if there is row in elig retire elements table.if not we r giving msg. to user

                ''get the retiree MEMTYPES
                RetireeAcctsDV = New DataView(_MemPlanDT, "STATUS LIKE 'RETIREE%'", "", DataViewRowState.CurrentRows)

                If RetireeAcctsDV IsNot Nothing Then
                    RetireeMEMValues = RetireeAcctsDV.ToTable
                End If

                '' chk the selected MEMTYPE is retiree
                SelectedRetireeDV = New DataView(RetireeMEMValues, "MEMTYPE = '" & CStr(DR("MEMTYPE")) & "'", "", DataViewRowState.CurrentRows)
                If SelectedRetireeDV IsNot Nothing Then
                    If SelectedRetireeDV.Count > 0 Then
                        Dim EligRetireElementsDS As DataSet = RegMasterDAL.RetrieveEligRetireeElementsByFamilyID(_FamilyID)

                        If EligRetireElementsDS IsNot Nothing And EligRetireElementsDS.Tables("ELGRETIRE_ELEMENTS").Rows.Count > 0 Then
                            strfilter = "'" & CDate(_SelectedEligPeriod) & "'  >= FROM_DATE AND '" & CDate(_SelectedEligPeriod) & "' <= THRU_DATE "

                            EligRetireeDV = New DataView(EligRetireElementsDS.Tables("ELGRETIRE_ELEMENTS"), strfilter, "", DataViewRowState.CurrentRows)
                            If EligRetireeDV IsNot Nothing AndAlso EligRetireeDV.Count > 0 Then
                                ''There is valid retirement entry in elig retire elements table.
                            Else
                                '' mesg to user to set up row in elig retire elements table and then add here
                                MessageBox.Show("Retiree information is missing. Please add it first before adding memtype ", "Set up Retiree", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Return
                            End If
                        Else
                            '' no rows in elig retire elements table
                            '' mesg to user to set up row in elig retire elements table and then add here
                            MessageBox.Show("Retiree information is missing." & Environment.NewLine & " Please add it first before adding Retiree memtype ", "Set up Retiree", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            cmbMemType.SelectedIndex = -1
                            cmbMemType.Focus()
                            Return
                        End If
                    End If
                End If         ''End chk the selected account is retiree

                '' end of retiree validation
            End If

        Catch ex As Exception
            Throw
        Finally

            If RetireeAcctsDV IsNot Nothing Then RetireeAcctsDV.Dispose()
            RetireeAcctsDV = Nothing

            If EligRetireeDV IsNot Nothing Then EligRetireeDV.Dispose()
            EligRetireeDV = Nothing

            If SelectedRetireeDV IsNot Nothing Then SelectedRetireeDV.Dispose()
            SelectedRetireeDV = Nothing

        End Try

    End Sub

    Private Sub cmbmemtype_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim CBox As ExComboBox

        CBox = CType(sender, ExComboBox)

        Try

            If _Disposed OrElse _MemTypeBS Is Nothing OrElse _MemTypeBS.Position < 0 OrElse CBox.SelectedIndex < 0 Then Return

            Dim DR As DataRow = DirectCast(_MemTypeBS.Current, DataRowView).Row

            If IsDBNull(DR("MEMTYPE")) OrElse (DR("MEMTYPE").ToString <> CBox.SelectedValue.ToString) Then
                DR("MEMTYPE") = If(CBox.Text.Trim.Length < 1, "", CBox.Text)
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

#End Region

#Region "Custom Subs\Functions"

    Public Sub LoadEligibility()

        Dim DR As DataRow
        Dim DRs As DataRow()

        Try

            ClearErrors()
            ClearMemTypeDataBindings()
            SetSettings()

            _MemTypeDS = New EligMthdtlDS
            _MemTypeDS.EnforceConstraints = False

            If _TotalSADS IsNot Nothing Then
                If _TotalSADS.Tables("ELIG_MTHDTL").Rows.Count > 0 Then
                    _MemTypeDS = CType(_TotalSADS.Copy, EligMthdtlDS)
                End If
            End If

            If _MemTypeDS.Tables("ELIG_MTHDTL").Rows.Count = 0 Then  '' only retrieve data for first time
                _MemTypeDS = CType(RegMasterDAL.RetrieveEligMTHDTLByFamilyID(_FamilyID, _MemTypeDS), EligMthdtlDS)
                _TotalSADS = CType(_MemTypeDS.Copy, EligMthdtlDS)
            End If

            MemTypeDataGrid.CaptionText = "Eligibility Record for Period " & If(_SelectedEligPeriod Is Nothing, "N/A", CType(_SelectedEligPeriod, Date).ToShortDateString)
            Me.Text = "Modify Memtype for the family " & _FamilyID.ToString

            DRs = _TotalSADS.Tables("ELIG_MTHDTL").Select("ELIG_PERIOD= '" & Format(_SelectedEligPeriod, "yyyy-MM-dd") & "'")

            _MemTypeDS.ELIG_MTHDTL.Rows.Clear()

            For Each DR In DRs
                _MemTypeDS.ELIG_MTHDTL.ImportRow(DR)
            Next

            AddHandler _MemTypeDS.Tables("ELIG_MTHDTL").ColumnChanging, AddressOf MemTypeDS_ColumnChanging
            AddHandler _MemTypeDS.Tables("ELIG_MTHDTL").ColumnChanged, AddressOf MemTypeDS_ColumnChanged
            AddHandler _MemTypeDS.Tables("ELIG_MTHDTL").RowChanging, AddressOf MemTypeDS_RowChanging
            AddHandler _MemTypeDS.Tables("ELIG_MTHDTL").RowChanged, AddressOf MemTypeDS_RowChanged

            _MemTypeBS = New BindingSource
            _MemTypeBS.DataSource = _MemTypeDS.Tables("ELIG_MTHDTL")

            MemTypeDataGrid.DataMember = ""
            MemTypeDataGrid.DataSource = _MemTypeBS
            MemTypeDataGrid.SetTableStyle()
            MemTypeDataGrid.Sort = If(MemTypeDataGrid.LastSortedBy, MemTypeDataGrid.DefaultSort)

            LoadMemTypeDataBindings()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadMemTypeDataBindings()

        Dim Bind As Binding

        Try

            txtFamilyID.DataBindings.Clear()
            Bind = New Binding("Text", _MemTypeBS, "FAMILY_ID")
            txtFamilyID.DataBindings.Add(Bind)

            txtRelationID.DataBindings.Clear()
            Bind = New Binding("Text", _MemTypeBS, "RELATION_ID")
            txtRelationID.DataBindings.Add(Bind)

            txtEligibiltyMonth.DataBindings.Clear()
            Bind = New Binding("Text", _MemTypeBS, "ELIG_PERIOD", True)
            Bind.FormatString = "MM-dd-yyyy"
            AddHandler Bind.Parse, AddressOf UFCWGeneral.DateOnlyBinding_Parse
            txtEligibiltyMonth.DataBindings.Add(Bind)

            cmbMemType.DataBindings.Clear()
            Bind = New Binding("SelectedValue", _MemTypeBS, "MEMTYPE", True)
            cmbMemType.DataBindings.Add(Bind)

        Catch ex As Exception

            Throw

        Finally

        End Try

    End Sub

    Private Sub ClearMemTypeDataBindings()

        If _MemTypeBS IsNot Nothing Then
            RemoveHandler _MemTypeBS.CurrentItemChanged, AddressOf _MemTypeBindingManagerBase_CurrentItemChanged
            RemoveHandler _MemTypeBS.PositionChanged, AddressOf _MemTypeBindingManagerBase_PositionChanged
            RemoveHandler _MemTypeBS.CurrentChanged, AddressOf _MemTypeBindingManagerBase_CurrentChanged
            _MemTypeBS.SuspendBinding()
        End If

        txtFamilyID.DataBindings.Clear()
        txtRelationID.DataBindings.Clear()
        txtEligibiltyMonth.DataBindings.Clear()

        MemTypeDataGrid.DataMember = ""
        MemTypeDataGrid.DataSource = Nothing

    End Sub

    Public Sub LoadMemtypes()
        Dim DT As DataTable = Nothing
        Dim strfilter As String = ""

        Try
            If _MemPlanDT Is Nothing OrElse _MemPlanDT.Rows.Count < 1 Then
                _MemPlanDT = RegMasterDAL.RetrieveMemplans
            End If

            _MemPlanBS = New BindingSource
            _MemPlanBS.DataSource = _MemPlanDT

            cmbMemType.DataSource = _MemPlanBS
            cmbMemType.ValueMember = "MEMTYPE"
            cmbMemType.DisplayMember = "MEMTYPEPLANTYPE"
            cmbMemType.SelectedIndex = -1

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

    Private Sub _MemTypeBindingManagerBase_CurrentChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    End Sub

    Private Sub _MemTypeBindingManagerBase_PositionChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        '   If Not _ReadOnlyMode Then SetUINavigation()

    End Sub

    Private Sub _MemTypeBindingManagerBase_CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        '  SetUIDataAwareness()

    End Sub

    Public Sub ClearErrors()
        ErrorProvider1.SetError(cmbMemType, "")
    End Sub

    Public Sub ClearAll()

        MemTypeDataGrid.DataSource = Nothing
        MemTypeDataGrid.CaptionText = ""
        If _TotalSADS IsNot Nothing Then
            _TotalSADS = Nothing
        End If

        If _ChangedDRs IsNot Nothing Then
            _ChangedDRs = Nothing
        End If

        If _MemTypeDS IsNot Nothing Then
            _MemTypeDS = Nothing
        End If

    End Sub

    Public Function PendingChanges() As Boolean

        Dim strchangesmade As String = ""
        Dim i As Integer

        Try
            _ChangedDRs = CType(_MemTypeDS.GetChanges(), EligMthdtlDS)

            If MemTypeDataGrid IsNot Nothing AndAlso _ChangedDRs IsNot Nothing AndAlso _ChangedDRs.Tables("ELIG_MTHDTL").Rows.Count > 0 Then
                If ((_ChangedDRs) IsNot Nothing) AndAlso (_ChangedDRs.Tables("ELIG_MTHDTL").Rows.Count > 0) Then
                    For i = 0 To _ChangedDRs.Tables("ELIG_MTHDTL").Rows.Count - 1
                        Dim DR As DataRow = _ChangedDRs.Tables("ELIG_MTHDTL").Rows(i)

                        If DR.RowState <> DataRowState.Added Then

                            Dim Changes As String = DataGridCustom.IdentifyChanges(DR, MemTypeDataGrid)

                            If Changes.Length > 0 Then
                                Return True
                            ElseIf Changes.Length = 0 Then
                                _ChangedDRs.Tables("ELIG_MTHDTL").Rows.Remove(DR)
                                _ChangedDRs = CType(_MemTypeDS.GetChanges(), EligMthdtlDS)
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

    Public Function SaveMemTypeChanges(ByRef Transaction As DbTransaction) As Boolean

        Dim ChgCnt As Integer = 0
        Dim HistSum As String = ""
        Dim HistDetail As String = ""

        Try
            _ChangedDRs = CType(_MemTypeDS.GetChanges(), EligMthdtlDS)

            For Each DR As DataRow In _ChangedDRs.Tables("ELIG_MTHDTL").Rows

                If DR.RowState = DataRowState.Modified Then 'ADD

                    Dim Changes As String = DataGridCustom.IdentifyChanges(DR, MemTypeDataGrid)

                    HistSum = "MEMTYPE FOR FAMILYID: " & _FamilyID.ToString & " WAS MODIFIED"
                    HistDetail = _DomainUser.ToUpper & " MODIFIED MEMTYPE " &
                                                       " FOR THE ELIGIBILITY PERIOD " & CStr(DR("ELIG_PERIOD")) &
                                                       " THE MODIFICATIONS WERE: " & Changes

                    If Changes.Trim.Length > 0 Then
                        RegMasterDAL.UpdateEligMTHDTLMemtype(CInt(DR("FAMILY_ID")), CDate(DR("ELIG_PERIOD")), CStr(DR("MEMTYPE")), Transaction)
                        RegMasterDAL.CreateRegHistory(_FamilyID, 0, Nothing, Nothing, "MEMTYPEUPDATE", Nothing, Nothing, Nothing, HistSum, HistDetail, _DomainUser.ToUpper, Transaction)
                    End If

                End If
            Next

            Return True

        Catch ex As Exception

            Throw

        End Try
    End Function

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

    Private Sub HistoryButton_Click(sender As Object, e As EventArgs) Handles HistoryButton.Click
        Dim HistoryF As RegMasterHistoryForm

        Try

            HistoryF = New RegMasterHistoryForm
            HistoryF.FamilyID = _FamilyID
            HistoryF.RelationID = -1
            HistoryF.Mode = REGMasterHistoryMode.Memtype
            HistoryF.ShowDialog()

        Catch ex As Exception
            Throw
        Finally

            If HistoryF IsNot Nothing Then
                HistoryF.Close()
                HistoryF.Dispose()
            End If
            HistoryF = Nothing
        End Try

    End Sub

    Private Sub cmbMemtype_Validating(sender As Object, e As CancelEventArgs)

        Dim Cbox As ExComboBox = CType(sender, ExComboBox)
        Dim DR As DataRow

        Try

            If _MemTypeBS Is Nothing OrElse _MemTypeBS.Position < 0 OrElse Cbox.ReadOnly Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ErrorProvider1.ClearError(Cbox)

            If Cbox.SelectedIndex < 0 Then
                ErrorProvider1.SetErrorWithTracking(Cbox, " Life Event is required.")
            End If

            If ErrorProvider1.GetError(Cbox).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub cmbMemtype_Validated(sender As Object, e As EventArgs)

        Dim DR As DataRow
        Dim CBox As ExComboBox = CType(sender, ExComboBox)
        Dim BS As BindingSource

        Try

            BS = DirectCast(_MemTypeBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " IO:  BS(" & BS.Position.ToString & ") Val(" & CBox.SelectedValue.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If _MemTypeBS Is Nothing OrElse _MemTypeBS.Position < 0 OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

            DR = DirectCast(_MemTypeBS.Current, DataRowView).Row

            If CBox.DataBindings.Count > 0 Then

                Select Case CBox.Name

                    Case "cmbMemtype"

                        If DR("MEMTYPE").ToString <> CBox.SelectedValue.ToString Then
                            CBox.DataBindings("SelectedValue").WriteValue()

                            _MemTypeBS.EndEdit()

                        End If

                End Select

            End If

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

    Private Sub cmbMemType_SelectionChangeCommitted(sender As Object, e As EventArgs)

        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Dim DR As DataRow
        Dim BS As BindingSource

        Try

            If _MemTypeBS Is Nothing OrElse _MemTypeBS.Position < 0 OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

            BS = DirectCast(_MemTypeBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & CBox.Name & " BS(" & BS.Position.ToString & ") Val(" & CBox.Text & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = DirectCast(_MemTypeBS.Current, DataRowView).Row

            CType(CBox.Parent, TransparentContainer).ValidateChildren() 'this will trigger validation of the cmbbox triggering write of value to DS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: (" & Me.Name & ":" & CBox.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
        End Try


    End Sub

    Private Sub MemTypeDS_RowChanging(sender As Object, e As DataRowChangeEventArgs)

        Dim BS As BindingSource
        Dim CM As CurrencyManager

        Try

            CM = CType(BindingContext(CType(sender, DataTable)), CurrencyManager)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid):  BS(" & CM.Position.ToString & "/" & CM.Count.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out(Grid): BS(" & CM.Position.ToString & "/" & CM.Count.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

    Private Sub MemTypeDS_RowChanged(sender As Object, e As DataRowChangeEventArgs)
        Dim BS As BindingSource
        Dim CM As CurrencyManager

        Try

            CM = CType(BindingContext(CType(sender, DataTable)), CurrencyManager)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid):  BS(" & CM.Position.ToString & "/" & CM.Count.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out(Grid): BS(" & CM.Position.ToString & "/" & CM.Count.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

    Private Sub MemTypeDS_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs)
        Dim BS As BindingSource

        Try

            BS = DirectCast(_MemTypeBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If _MemTypeDS IsNot Nothing AndAlso _MemTypeBS.Position > -1 AndAlso _MemTypeBS.Count > 0 AndAlso _MemTypeBS.Current IsNot Nothing Then

                Dim DR As DataRow = e.Row

            End If

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub MemTypeDS_ColumnChanged(sender As Object, e As DataColumnChangeEventArgs)
        Dim BS As BindingSource

        Try

            BS = DirectCast(_MemTypeBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If _MemTypeDS IsNot Nothing AndAlso _MemTypeBS.Position > -1 AndAlso _MemTypeBS.Count > 0 AndAlso _MemTypeBS.Current IsNot Nothing Then

                Dim DR As DataRow = e.Row

            End If

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

#End Region


End Class
