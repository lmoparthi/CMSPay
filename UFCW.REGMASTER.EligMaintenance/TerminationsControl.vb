Option Strict On

Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Data.Common
Imports DDTek.DB2
Imports System.Reflection

Public Class TerminationsControl
    Inherits System.Windows.Forms.UserControl

#Region "Variables"

    Private _FamilyID As Integer = -1
    Private _RelationID As Integer = -1
    Private _PartSSNO As Integer = -1

    Private _APPKEY As String = "UFCW\RegMaster\"

    Private _ReadOnlyMode As Boolean = False
    Private _DisplayColumnNames As DataView
    Private _ChangedDRs As TerminationsDS
    Private _TotalTermDS As New TerminationsDS
    Private _EligSpecialAcctValuesDT As DataTable
    Private _WorkersCompDT As DataTable
    Private _DisabilityDT As DataTable
    Private _CobraDT As DataTable

    Private _EligCalcDS As DataSet
    Private _EligcalcDT As DataTable

    Private _Override As String = "Y"
    Private _TermDate As Date

    Private _ViewHistory As Boolean?

    Private WithEvents _TBS As BindingSource

    ReadOnly _REGMEmployeeAccess As Boolean = UFCWGeneralAD.REGMEmployeeAccess
    ReadOnly _REGMReadOnlyAccess As Boolean = UFCWGeneralAD.REGMReadOnlyAccess
    ReadOnly _REGMVendorAccess As Boolean = UFCWGeneralAD.REGMVendorAccess
    ReadOnly _REGMTermAccess As Boolean = UFCWGeneralAD.REGMTermAccess

    Private _Disposed As Boolean = False

#End Region
    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.
            '
            If _ChangedDRs IsNot Nothing Then
                _ChangedDRs.Dispose()
            End If
            _ChangedDRs = Nothing

            If _TotalTermDS IsNot Nothing Then
                _TotalTermDS.Dispose()
            End If
            _TotalTermDS = Nothing

            If _EligSpecialAcctValuesDT IsNot Nothing Then
                _EligSpecialAcctValuesDT.Dispose()
            End If
            _EligSpecialAcctValuesDT = Nothing

            If _WorkersCompDT IsNot Nothing Then
                _WorkersCompDT.Dispose()
            End If
            _WorkersCompDT = Nothing

            If _DisabilityDT IsNot Nothing Then
                _DisabilityDT.Dispose()
            End If
            _DisabilityDT = Nothing

            If _CobraDT IsNot Nothing Then
                _CobraDT.Dispose()
            End If
            _CobraDT = Nothing

            If _TotalTermDS IsNot Nothing Then
                _TotalTermDS.Dispose()
            End If
            _TotalTermDS = Nothing

            If TerminationLookupDataGrid IsNot Nothing Then
                TerminationLookupDataGrid.Dispose()
            End If
            TerminationLookupDataGrid = Nothing

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

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Indicates that Save has not been used.")>
    Public ReadOnly Property ChangesPending() As Boolean
        Get
            Return UnCommittedChangesExist()
        End Get
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

#End Region

#Region "Constructor"
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        Dim designMode As Boolean = (LicenseManager.UsageMode = LicenseUsageMode.Designtime)
        If Not designMode Then
            LoadSpecialAccounts()   '' this is for workers comp validation
        End If
        'dont want to display the default table style
    End Sub

    Public Sub New(ByVal familyID As Integer, ByVal relationID As Integer, ByVal ssn As Integer, Optional ByVal readOnlyMode As Boolean? = Nothing, Optional ByVal memName As String = "")
        Me.New()

        _FamilyID = familyID
        _RelationID = relationID
        _PartSSNO = ssn

        txtFamilyID.Text = _FamilyID.ToString

        _ReadOnlyMode = If(readOnlyMode IsNot Nothing, CBool(readOnlyMode), _ReadOnlyMode)

        grpEditPanel.Enabled = False
    End Sub
#End Region

#Region "Form\Button Events"

    Private Sub AddActionButton_Click(sender As System.Object, e As System.EventArgs) Handles AddActionButton.Click

        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

        Try

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            AddActionButton.Enabled = False

            AddTerm("T")

            cmbRetirement.SelectedIndex = -1
            cmbCobraLetter.SelectedIndex = -1

        Catch ex As Exception


                Throw

        Finally
            Me.AutoValidate = HoldAutoValidate

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub DeleteActionButton_Click(sender As Object, e As EventArgs) Handles RemoveActionButton.Click


        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

        Try

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            RemoveActionButton.Enabled = False

            AddTerm("R")

            cmbRetirement.SelectedIndex = -1
            cmbCobraLetter.SelectedIndex = -1

        Catch ex As Exception


                Throw

        Finally
            Me.AutoValidate = HoldAutoValidate

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub CancelActionButton_Click(sender As System.Object, e As System.EventArgs) Handles CancelActionButton.Click

        Dim Result As DialogResult = DialogResult.None

        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

        Try

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

            CancelActionButton.Enabled = False
            SaveActionButton.Enabled = False

            _ChangedDRs = CType(_TDS.GetChanges(), TerminationsDS)

            If _ChangedDRs IsNot Nothing Then
                Result = MessageBox.Show(Me, "Do you want to Cancel the changes?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            End If

            If Result = DialogResult.Yes Then

                _TBS.CancelEdit()
                _TDS.Tables("TERMS").RejectChanges()

                ClearErrors()

                _TBS.ResetBindings(False)

                cmbRetirement.SelectedIndex = -1
                cmbCobraLetter.SelectedIndex = -1

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

    Private Sub SaveButton_Click(sender As System.Object, e As System.EventArgs) Handles SaveActionButton.Click

        Dim Transaction As DbTransaction = Nothing
        Dim EligCalc As CalculateEligibility = Nothing
        Dim WaitFrm As New Waitmessage

        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

        Try

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

            SaveActionButton.Enabled = False
            CancelActionButton.Enabled = False

            ClearErrors()

            If Not ValidateChildren() Then
                SaveActionButton.Enabled = True
                CancelActionButton.Enabled = True
                Return
            End If

            _TBS.EndEdit()

            SetOverrideValue()

            _ChangedDRs = CType(_TDS.GetChanges(), TerminationsDS)

            If _ChangedDRs Is Nothing Then     '' when new row added,deleted then cancel, save buttons are enabled
                MessageBox.Show("There are no changes to the record.", "No Changes", MessageBoxButtons.OK, MessageBoxIcon.Information)
                SaveActionButton.Enabled = False
                CancelActionButton.Enabled = False
                Exit Sub
            End If

            If _ChangedDRs IsNot Nothing Then

                If SaveChanges() Then

                    MessageBox.Show("Terms Record Saved Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    _TBS.ResetBindings(False)

                    _TotalTermDS = Nothing
                    _TDS.Clear()

                    LoadTerms()

                    Dim MonthStart As Date = CDate(UFCWGeneral.MonthEndDate(_TermDate).AddDays(1))

                    EligCalc = New CalculateEligibility(_FamilyID, MonthStart)

                    WaitFrm.Show()
                    WaitFrm.Activate()

                    Cursor.Current = Cursors.WaitCursor

                    Application.DoEvents()

                    If EligCalc.DetermineEligibility(_FamilyID, MonthStart) Then

                        '' Message to user to refresh Elig Hours tab
                        MessageBox.Show("Check EligHours Tab to review changes. ", "Refresh EligHours", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Cursor.Current = Cursors.Default
                        WaitFrm.Hide()

                        '' Message to user to calculate eligiblity 
                        RegMasterDAL.MadeDBChanges = True  '' this is to get elig_acct_hours from database to show pending/ or not
                        RegMasterDAL.MadeEligibilityChanges = True  '' this is to get elig_mthdtl 

                    Else
                        MessageBox.Show("Error while Calculating Eligibility." & vbCrLf & "Please try again ", "Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If


                Else
                    MessageBox.Show("Error while saving Terms Record." & vbCrLf & "Please try again ", "Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Cursor.Current = Cursors.Default
                    WaitFrm.Hide()
                End If

            End If

        Catch ex As Exception

            CancelActionButton.Enabled = True


                Throw

        Finally

        End Try

    End Sub

    Private Sub Terms_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs)
        Me.Dispose()
    End Sub

    Private Sub Terms_dispose(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        Me.Dispose()
    End Sub


#End Region

#Region "Custom Subs\Functions"

    Private Function UnCommittedChangesExist() As Boolean

        Dim Modifications As String = ""
        Try

            If _TDS Is Nothing Then Return False

            _ChangedDRs = CType(_TDS.GetChanges, TerminationsDS)

            If TerminationLookupDataGrid IsNot Nothing AndAlso _ChangedDRs IsNot Nothing AndAlso _ChangedDRs.Tables("TERMS").Rows.Count > 0 Then

                For Each DR As DataRow In _ChangedDRs.Tables("TERMS").Rows
                    If DR.RowState <> DataRowState.Added Then
                        'attempt to exclude rows accidently changed during navigation operations
                        Modifications = DataGridCustom.IdentifyChanges(DR, TerminationLookupDataGrid)

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

    Public Sub LoadTerms(ByVal familyID As Integer, ByVal relationID As Integer, ByVal ssn As Integer, Optional ByVal memName As String = "", Optional ByVal readOnlyMode As Boolean? = Nothing)

        _FamilyID = familyID
        _RelationID = relationID
        _ReadOnlyMode = If(readOnlyMode IsNot Nothing, CBool(readOnlyMode), _ReadOnlyMode)
        _PartSSNO = ssn

        LoadTerms()

        txtMemName.Text = memName
        txtFamilyID.Text = _FamilyID.ToString
        txtPartSSN.Text = _PartSSNO.ToString

    End Sub

    Public Sub LoadTerms()

        Try

            grpEditPanel.SuspendLayout()

            ClearErrors()
            ClearDataBindings(Me)

            If _TotalTermDS IsNot Nothing Then
                If _TotalTermDS.Tables("TERMS").Rows.Count > 0 Then
                    _TDS.Tables("TERMS").Rows.Clear()
                    _TDS = CType(_TotalTermDS.Copy, TerminationsDS)
                End If
            End If

            If _TDS.Tables("TERMS").Rows.Count = 0 Then  '' only retrieve data for first time
                _TDS = CType(RegMasterDAL.RetrieveTerminationsBySSN(_PartSSNO, _TDS), TerminationsDS)

                _TotalTermDS = CType(_TDS.Copy, TerminationsDS)
                '_FamilyID = FamilyID
                '_RelationID = RelationID
                '_PartSSNO = ssn

                _EligCalcDS = RegMasterDAL.RetrieveEligCalcElementsByFamilyID(_FamilyID)

                lblCurrentStatus.Text = ""

                Dim DisabilityQuery =
                    From SA In _EligSpecialAcctValuesDT.AsEnumerable()
                    Where SA.Field(Of String)("GROUP_FUNCTIONALITY") = "DISABILITY" _
                    AndAlso SA.Field(Of Integer)("ACCTNO") = CInt(_EligCalcDS.Tables(0).Rows(0)("LAST_EMPLOYER"))

                If DisabilityQuery.Any Then
                    lblCurrentStatus.Text = "Disability"
                End If

                Dim WorkersCompQuery =
                    From SA In _EligSpecialAcctValuesDT.AsEnumerable()
                    Where SA.Field(Of String)("GROUP_FUNCTIONALITY") = "WORKERS COMP" _
                    AndAlso SA.Field(Of Integer)("ACCTNO") = CInt(_EligCalcDS.Tables(0).Rows(0)("LAST_EMPLOYER"))

                If WorkersCompQuery.Any Then
                    lblCurrentStatus.Text = "Workers Comp."
                End If

                Dim CobraQuery =
                    From SA In _EligSpecialAcctValuesDT.AsEnumerable()
                    Where SA.Field(Of String)("GROUP_FUNCTIONALITY") = "COBRA" _
                    AndAlso SA.Field(Of Integer)("ACCTNO") = CInt(_EligCalcDS.Tables(0).Rows(0)("LAST_EMPLOYER"))

                If CobraQuery.Any Then
                    lblCurrentStatus.Text = "Cobra"
                End If
            End If

            RemoveHandler _TDS.TERMS.ColumnChanging, AddressOf TDS_ColumnChanging
            AddHandler _TDS.TERMS.ColumnChanging, AddressOf TDS_ColumnChanging
            RemoveHandler _TDS.TERMS.RowChanging, AddressOf TDS_RowChanging
            AddHandler _TDS.TERMS.RowChanging, AddressOf TDS_RowChanging

            _TBS = New BindingSource
            _TBS.RaiseListChangedEvents = False

            _TBS.DataSource = _TDS.Tables("TERMS")
            _TBS.Sort = If(TerminationLookupDataGrid.LastSortedBy, TerminationLookupDataGrid.DefaultSort)

            TerminationLookupDataGrid.DataSource = _TBS
            TerminationLookupDataGrid.SetTableStyle()

            LoadDataBindings()

            _TBS.RaiseListChangedEvents = True
            _TBS.ResetBindings(False)

            grpEditPanel.ResumeLayout()

        Catch ex As Exception

                Throw
        End Try
    End Sub

    Private Sub TDS_RowChanged(sender As Object, e As DataRowChangeEventArgs)
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

    Private Sub TDS_RowChanging(sender As Object, e As DataRowChangeEventArgs)
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

    Private Sub TDS_ColumnChanged(sender As Object, e As DataColumnChangeEventArgs)
        Dim BS As BindingSource

        Try

            BS = DirectCast(_TBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

    Private Sub TDS_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs)
        Dim BS As BindingSource

        Try

            BS = DirectCast(_TBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))


        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

    Private Sub LoadDataBindings()
        Dim Bind As Binding

        Try


            'txtFamilyID.DataBindings.Clear()
            'Bind = New Binding("Text", _TBS, "FAMILY_ID", True, DataSourceUpdateMode.OnValidation)
            'txtFamilyID.DataBindings.Add(Bind)

            'txtPartSSN.DataBindings.Clear()
            'Bind = New Binding("Text", _TBS, "SSNO", True, DataSourceUpdateMode.OnValidation)
            'txtPartSSN.DataBindings.Add(Bind)

            txtTermDt.DataBindings.Clear()
            Bind = New Binding("Text", _TBS, "TERM_DATE", True, DataSourceUpdateMode.OnValidation)
            Bind.FormatString = "MM-dd-yyyy"
            AddHandler Bind.Parse, AddressOf UFCWGeneral.DateOnlyBinding_Parse
            txtTermDt.DataBindings.Add(Bind)

        Catch ex As Exception


                Throw

        Finally

        End Try

    End Sub

    Private Sub SetUIElements(readOnlyMode As Boolean)
        Dim DR As DataRow
        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

        Try

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            grpEditPanel.SuspendLayout()

            If Not readOnlyMode Then readOnlyMode = _REGMReadOnlyAccess
            If UFCWGeneralAD.REGMVendorAccess Then _ViewHistory = False

            If _TBS IsNot Nothing AndAlso _TBS.Position > -1 AndAlso _TBS.Current IsNot Nothing AndAlso _TBS.Count > 0 Then
                DR = CType(_TBS.Current, DataRowView).Row
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

            Me.RemoveActionButton.Visible = Not readOnlyMode
            Me.RemoveActionButton.Enabled = False

            Dim AddedQuery = (From Added As DataRow In _TDS.Tables("TERMS").AsEnumerable()
                              Where Added.RowState = DataRowState.Added)

            ProcessSubControls(CType(grpEditPanel, Object), True, True) 'lock everything down except buttons

            If Not AddedQuery.Any Then

            End If

            If readOnlyMode OrElse _FamilyID < 0 Then

                AddActionButton.Visible = False
                RemoveActionButton.Visible = False

                CancelActionButton.Visible = False
                SaveActionButton.Visible = False

            Else

                AddActionButton.Visible = True
                RemoveActionButton.Visible = True

                If lblCurrentStatus.Text.ToUpper.Contains("DISABILITY") AndAlso Not _REGMTermAccess Then
                    AddActionButton.Visible = False
                    RemoveActionButton.Visible = False
                End If

                CancelActionButton.Visible = True
                SaveActionButton.Visible = True

                If DR IsNot Nothing Then 'based upon row status / content decide how to present controls

                    If _TDS.HasChanges Then
                        Me.SaveActionButton.Enabled = True
                        Me.CancelActionButton.Enabled = True
                    End If

                    If DR.RowState = DataRowState.Added OrElse DR.RowState = DataRowState.Modified Then


                    End If

                    If DR.RowState = DataRowState.Added Then

                        cmbRetirement.ReadOnly = False
                        cmbCobraLetter.ReadOnly = False
                        txtTermDt.ReadOnly = False

                        AddActionButton.Enabled = False
                        RemoveActionButton.Enabled = False

                    ElseIf DR.RowState = DataRowState.Modified Then

                        cmbRetirement.ReadOnly = False
                        cmbCobraLetter.ReadOnly = False

                        AddActionButton.Enabled = False
                        RemoveActionButton.Enabled = False

                    ElseIf DR.RowState = DataRowState.Unchanged Then

                        SaveActionButton.Enabled = False

                        If Not AddedQuery.Any Then
                            AddActionButton.Enabled = True
                            RemoveActionButton.Enabled = True
                        End If

                    Else

                        AddActionButton.Enabled = False
                        RemoveActionButton.Enabled = False

                    End If

                Else
                    AddActionButton.Enabled = True
                    RemoveActionButton.Enabled = True

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

    Public Sub ClearAll()

        _ReadOnlyMode = True

        If _TDS IsNot Nothing Then
            _TDS.TERMS.Clear() 'clear's databinding
        End If

        If _TBS IsNot Nothing Then
            _TBS.Dispose()
        End If
        _TBS = Nothing

        ClearDataBindings(Me)

        _TDS = New TerminationsDS 'recreate for possible reentry

        TerminationLookupDataGrid.DataSource = Nothing
        TerminationLookupDataGrid.CaptionText = ""

        If _TotalTermDS IsNot Nothing Then _TotalTermDS.Dispose()
        _TotalTermDS = Nothing

        _ChangedDRs = Nothing

        _FamilyID = -1
        _RelationID = -1
        _PartSSNO = -1
        _TermDate = Nothing

    End Sub

    Private Sub AddTerm(ByVal status As String)
        Dim DR As DataRow

        Try
            _TDS.EnforceConstraints = False
            DR = _TDS.TERMS.NewRow
            DR.BeginEdit()

            DR("FAMILY_ID") = _FamilyID
            DR("RELATION_ID") = _RelationID

            DR("EMPRNO") = 0
            DR("EMPRSUB") = 0
            DR("SSNO") = _PartSSNO
            DR("TERM_DATE") = DBNull.Value
            DR("DATE_ADDED") = CDate(Now)
            DR("STATUS") = status

            DR("USERID") = UFCWGeneral.DomainUser.ToUpper
            DR("LASTUPDT") = CDate(Now)

            DR.EndEdit()

            _TDS.TERMS.Rows.Add(DR)

            _TBS.EndEdit()

        Catch ex As Exception


                Throw

        End Try
    End Sub

    'Private Function VerifyTermsChanges() As Boolean

    '    Dim WorkDV As DataView
    '    Dim DR As DataRow

    '    Try

    '        ClearErrors()

    '        If _TermBindingManagerBase.Count > 0 AndAlso _TermBindingManagerBase.Position > -1 Then

    '            DR = DirectCast(_TermBindingManagerBase.Current, DataRowView).Row


    '            If IsDate(DR("TERM_DATE")) Then
    '            Else
    '                ErrorProvider1.SetError(Me.txtTermDt, "TermDate is invalid (MM-dd-yyyy)")
    '            End If

    '            '' Term date should not be future date
    '            If Not IsDBNull(DR("TERM_DATE")) Then
    '                If DateDiff("d", Now.Date, DR("TERM_DATE")) > 0 Then
    '                    ErrorProvider1.SetError(Me.txtTermDt, "Term Date should not be Future date")
    '                    txtTermDt.Focus()
    '                End If
    '            End If

    '            '' if termination is add means value T then Retirement Pending is required..
    '            If CStr(DR("STATUS")) = "T" Then
    '                If cmbRetirement.Text.Length > 0 Then
    '                Else
    '                    ErrorProvider1.SetError(Me.cmbRetirement, " Select whether Retirement pending or Not")
    '                End If

    '                If cmbRetirement.Text = "N" Then   '' not retirement pending

    '                    '' retirement is not pending and the last employer is workers comp
    '                    If (_EligcalcDT) IsNot Nothing AndAlso _EligcalcDT.Rows.Count > 0 Then
    '                        Dim EligAcctno As Integer = CInt(_EligcalcDT.Rows(0)("LAST_EMPLOYER")) '' get the employer acctno in ELG_CALC_ELEMENTS
    '                        WorkDV = New DataView(_WorkersCompDT, "ACCTNO = " & EligAcctno, "", DataViewRowState.CurrentRows)
    '                        If WorkDV IsNot Nothing AndAlso WorkDV.Count > 0 Then
    '                            ErrorProvider1.SetError(Me.cmbRetirement, " You cannot add term when Participient is on Workers Comp and " & Environment.NewLine &
    '                                                                         " Retirement is not Pending.")
    '                        End If
    '                    End If
    '                    ''
    '                End If
    '            End If

    '            If CStr(DR("STATUS")) = "T" Then
    '                If cmbCobraLetter.Text.Length > 0 Then
    '                Else
    '                    ErrorProvider1.SetError(Me.cmbCobraLetter, " Select whether to send COBRA letter or not")
    '                End If
    '            End If

    '            If ErrorProviderErrorsList(ErrorProvider1).Length > 0 Then
    '                Return True
    '            End If
    '            '' Return False
    '        End If
    '    Catch ex As Exception

    '        Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '        If (rethrow) Then
    '            Throw
    '        End If
    '    Finally
    '        If WorkDV IsNot Nothing Then
    '            WorkDV.Dispose()
    '        End If
    '        WorkDV = Nothing
    '        DR = Nothing
    '    End Try

    'End Function

    Public Function SaveChanges() As Boolean
        Dim HistSum As String = ""
        Dim HistDetail As String = ""
        Dim ValidationsStr As String = ""

        Dim ActivityTimeStamp As DateTime = Date.Now

        Dim Transaction As DbTransaction

        Try

            Transaction = RegMasterDAL.BeginTransaction

            _ChangedDRs = CType(_TDS.GetChanges(), TerminationsDS)

            For Each DR As DataRow In _ChangedDRs.Tables("TERMS").Rows

                HistSum = ""
                HistDetail = ""

                HistDetail = DataGridCustom.IdentifyChanges(DR, TerminationLookupDataGrid)

                If DR.RowState = DataRowState.Added Then 'ADD

                    ''Retirement Pending
                    If cmbRetirement.Text = "N" Then
                        ValidationsStr = " AND For Retirement Pending User chosen:  N " & Environment.NewLine
                    ElseIf cmbRetirement.Text = "Y" Then
                        ValidationsStr = " AND For Retirement Pending User chosen: Y " & Environment.NewLine
                    End If

                    ''Sent COBRA Letter

                    If cmbCobraLetter.Text = "N" Then
                        ValidationsStr += " AND For Send COBRA letter User chosen:  N " & Environment.NewLine
                    ElseIf cmbCobraLetter.Text = "Y" Then
                        ValidationsStr += " AND For Send COBRA letter User chosen:  Y " & Environment.NewLine
                    End If


                    HistSum = "TERM FOR FAMILYID: " & _FamilyID & " WAS ADDED"
                    HistDetail = UFCWGeneral.DomainUser.ToUpper & " ADDED THE TERM RECORD " & Environment.NewLine &
                                                                " THE MODIFICATIONS WERE: " & HistDetail & Environment.NewLine & ValidationsStr

                    If CStr(DR("STATUS")) = "T" Then    '' Adding Terms

                        RegMasterDAL.AddTerminations(CInt(DR("SSNO")), CDate(DR("TERM_DATE")), CStr(DR("STATUS")), UFCWGeneral.DomainUser.ToUpper, CInt(DR("FAMILY_ID")), CInt(DR("RELATION_ID")), _Override, Transaction)


                        RegMasterDAL.CreateRegHistory(Me.FamilyID, Me.RelationID, Nothing, Nothing, "TERMADD", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, Transaction)

                    ElseIf CStr(DR("STATUS")) = "R" Then  ''Removal terms

                        HistSum = "TERM FOR FAMILYID: " & _FamilyID & " WAS REMOVED"
                        HistDetail = UFCWGeneral.DomainUser.ToUpper & " ADDED THE TERM RECORD " & Environment.NewLine &
                                                                " THE MODIFICATIONS WERE: " & HistDetail

                        RegMasterDAL.RemoveTerminations(CInt(DR("SSNO")), CDate(DR("TERM_DATE")), UFCWGeneral.DomainUser.ToUpper, CInt(DR("FAMILY_ID")), CInt(DR("RELATION_ID")), Transaction)
                        RegMasterDAL.CreateRegHistory(Me.FamilyID, Me.RelationID, Nothing, Nothing, "TERMREMOVAL", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, Transaction)

                    End If

                End If

                _TermDate = CDate(DR("TERM_DATE"))
            Next

            RegMasterDAL.CommitTransaction(Transaction)

            Dim TQuery = (From T As DataRow In _TDS.Tables("TERMS").AsEnumerable()
                          Where T.RowState = DataRowState.Modified OrElse T.RowState = DataRowState.Added)

            For Each DR As DataRow In TQuery
                DR("LASTUPDT") = ActivityTimeStamp
                DR("USERID") = UFCWGeneral.DomainUser.ToUpper
            Next

            _TDS.AcceptChanges()

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

    Public Sub ClearErrors()
        ErrorProvider1.Clear()
    End Sub

    Public Sub LoadSpecialAccounts()

        Try

            _EligSpecialAcctValuesDT = RegMasterDAL.RetrieveEligSpecialAcctValues

        Catch ex As Exception

                Throw

        Finally

        End Try
    End Sub

    Private Sub SetOverrideValue()

        Try
            If _EligcalcDT IsNot Nothing AndAlso _EligcalcDT.Rows.Count > 0 Then
                Dim EligAcctno As Integer = CInt(_EligcalcDT.Rows(0)("LAST_EMPLOYER")) '' get the employer acctno in ELG_CALC_ELEMENTS
                Dim CobraDV As DataView = New DataView(_CobraDT, "ACCTNO = " & EligAcctno, "", DataViewRowState.CurrentRows)
                If CobraDV.Count > 0 Then
                    If cmbCobraLetter.Text = "Y" Then
                        ''If cmboverride.Text = "Y" Then
                        ''    _override = "Y"
                        ''ElseIf cmboverride.Text = "N" Then
                        ''    _override = "N"
                        ''End If
                        _Override = "Y"
                    ElseIf cmbCobraLetter.Text = "N" Then '' No cobra letter to be sent
                        _Override = "N"
                    End If
                Else '' Not cobra account we are entering an entry in COBRA_QE
                    If cmbCobraLetter.Text = "Y" Then
                        _Override = "Y"
                    ElseIf cmbCobraLetter.Text = "N" Then
                        _Override = "N"
                    End If
                End If
            End If

        Catch ex As Exception

                Throw

        Finally
        End Try
    End Sub

    Private Sub TBS_ListChanged(sender As Object, e As ListChangedEventArgs) Handles _TBS.ListChanged

        Dim BS As BindingSource

        Try
            BS = DirectCast(_TBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") RS(" & If(BS.Position < 0 ORELSE BS.Current Is Nothing, "N/A", DirectCast(BS.Current, System.Data.DataRowView).Row.RowState.ToString) & ") O(" & e.OldIndex.ToString & ") N(" & e.NewIndex.ToString & ") CT(" & e.ListChangedType.ToString & ") SEL(" & If(BS Is Nothing OrElse BS.Count < 1 OrElse BS.Position < 0 OrElse TerminationLookupDataGrid.DataSource Is Nothing, "N/A", TerminationLookupDataGrid.IsSelected(BS.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Select Case e.ListChangedType
                Case ListChangedType.ItemDeleted 'account for rows deleted due to a cancel action

                Case ListChangedType.ItemMoved

                    Select Case DirectCast(BS.Current, System.Data.DataRowView).Row.RowState
                        Case DataRowState.Modified

                        Case DataRowState.Added

                            If e.OldIndex <> e.NewIndex OrElse (BS.Position > -1 AndAlso BS.Count = 1) Then
                                TerminationLookupDataGrid.Select(BS.Position)
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
                        If e.NewIndex > -1 Then TerminationLookupDataGrid.Select(e.NewIndex)
                    End If

            End Select

            ProcessControls(CType(grpEditPanel, Object), _ReadOnlyMode)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") RS(" & If(BS.Position < 0 ORELSE BS.Current Is Nothing, "N/A", DirectCast(BS.Current, System.Data.DataRowView).Row.RowState.ToString) & ") O(" & e.OldIndex.ToString & ") N(" & e.NewIndex.ToString & ") CT(" & e.ListChangedType.ToString & ") SEL(" & If(BS Is Nothing OrElse BS.Count < 1 OrElse BS.Position < 0 OrElse TerminationLookupDataGrid.DataSource Is Nothing, "N/A", TerminationLookupDataGrid.IsSelected(BS.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub TBS_CurrentChanged(sender As Object, e As EventArgs) Handles _TBS.CurrentChanged
        Dim BS As BindingSource

        Try

            BS = DirectCast(_TBS, BindingSource)

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

    Private Sub HistoryButton_Click(sender As Object, e As EventArgs) Handles HistoryButton.Click

        Dim HistoryF As RegMasterHistoryForm

        Try

            HistoryF = New RegMasterHistoryForm
            HistoryF.FamilyID = _FamilyID
            HistoryF.RelationID = -1
            HistoryF.Mode = REGMasterHistoryMode.TERM
            HistoryF.ShowDialog()

            HistoryF.Close()

        Catch ex As Exception
            Throw
        Finally
            HistoryF.Dispose()
            HistoryF = Nothing
        End Try

    End Sub

    Private Sub txtTermDt_Validating(sender As Object, e As CancelEventArgs) Handles txtTermDt.Validating

        Dim Tbox As TextBox = CType(sender, TextBox)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If _TBS Is Nothing OrElse _TBS.Position < 0 OrElse Tbox.ReadOnly Then Return

            ErrorProvider1.ClearError(Tbox)

            If Tbox.Text.Trim.Length < 1 Then

                ErrorProvider1.SetErrorWithTracking(Tbox, " Term Date required for selected Term Description.")

            Else

                Dim HoldDate As Date? = UFCWGeneral.ValidateDate(Tbox.Text)
                If HoldDate Is Nothing Then
                    ErrorProvider1.SetErrorWithTracking(Tbox, " Date format must be mm/dd/yyyy or mm-dd-yyyy.")
                Else
                    If HoldDate > Now.Date AndAlso (cmbRetirement.SelectedIndex < 0 OrElse cmbRetirement.Text <> "Y") Then
                        ErrorProvider1.SetErrorWithTracking(Tbox, " Term Date cannot be in future unless Retirement Pending is Y.")
                    End If

                    If IsDate(_EligCalcDS.Tables(0).Rows(0)("LAST_DATE_WORKED")) Then
                        If HoldDate < CDate(_EligCalcDS.Tables(0).Rows(0)("LAST_DATE_WORKED")) Then
                            ErrorProvider1.SetErrorWithTracking(Tbox, " Term Date cannot be before Participant Last Date Worked.")
                        End If
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

    Private Sub txtTermDate_Validated(sender As Object, e As System.EventArgs) Handles txtTermDt.Validated

        Dim Tbox As TextBox = CType(sender, TextBox)
        Dim DR As DataRow

        Try
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If _TBS Is Nothing OrElse _TBS.Position < 0 OrElse Tbox.ReadOnly Then Return

            DR = DirectCast(_TBS.Current, DataRowView).Row

        Catch ex As Exception

                Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

    Private Sub cmb_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbRetirement.SelectedIndexChanged, cmbCobraLetter.SelectedIndexChanged

        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Dim BS As BindingSource

        Try

            If _TBS Is Nothing OrElse _TBS.Position < 0 OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

            BS = DirectCast(_TBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & CBox.Name & " BS(" & BS.Position.ToString & ") Val(" & CBox.SelectedText.ToString & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            _TBS.ResetCurrentItem()
            TerminationLookupDataGrid.RefreshData()

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
        End Try

    End Sub

    Private Sub cmb_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbRetirement.SelectionChangeCommitted, cmbCobraLetter.SelectionChangeCommitted
        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Dim DR As DataRow
        Dim BS As BindingSource

        Try

            If _TBS Is Nothing OrElse _TBS.Position < 0 OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

            BS = DirectCast(_TBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & CBox.Name & " BS(" & BS.Position.ToString & ") Val(" & CBox.SelectedText.ToString & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = DirectCast(_TBS.Current, DataRowView).Row

            CType(CBox.Parent, TransparentContainer).ValidateChildren() 'this will trigger validation of the cmbbox triggering write of value to DS

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
        End Try

    End Sub

    Private Sub cmbRetirement_Validating(sender As Object, e As CancelEventArgs) Handles cmbRetirement.Validating

        Dim CBox As ExComboBox = CType(sender, ExComboBox)
        Dim BS As BindingSource
        Dim DR As DataRow

        Try

            ErrorProvider1.ClearError(CBox)

            If _TBS Is Nothing OrElse _TBS.Position < 0 OrElse CBox.ReadOnly Then Return

            BS = DirectCast(_TBS, BindingSource)
            DR = DirectCast(_TBS.Current, DataRowView).Row

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & CBox.Name & " BS(" & BS.Position.ToString & ") Val(" & If(CBox.SelectedText Is Nothing, "N/A", CBox.SelectedText.ToString) & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If DR("STATUS").ToString = "T" Then
                If CBox.SelectedIndex < 0 Then
                    ErrorProvider1.SetErrorWithTracking(CBox, " Retirement Pending Status Required.")
                Else
                    If cmbRetirement.Text = "N" AndAlso lblCurrentStatus.Text.ToUpper = "WORKERS COMP." Then   '' not retirement pending

                        ErrorProvider1.SetErrorWithTracking(CBox, " You cannot add term when Participant is on Workers Comp and " & Environment.NewLine &
                                                                     " Retirement is not Pending.")
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
    Private Sub cmbCobraLetter_Validating(sender As Object, e As CancelEventArgs) Handles cmbCobraLetter.Validating

        Dim CBox As ExComboBox = CType(sender, ExComboBox)
        Dim BS As BindingSource
        Dim DR As DataRow

        Try

            ErrorProvider1.ClearError(CBox)

            If _TBS Is Nothing OrElse _TBS.Position < 0 OrElse CBox.ReadOnly Then Return

            BS = DirectCast(_TBS, BindingSource)
            DR = DirectCast(_TBS.Current, DataRowView).Row

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & CBox.Name & " BS(" & BS.Position.ToString & ") Val(" & If(CBox.SelectedText Is Nothing, "N/A", CBox.SelectedText.ToString) & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If CBox.SelectedIndex < 0 AndAlso DR("STATUS").ToString = "T" Then
                ErrorProvider1.SetErrorWithTracking(CBox, " Cobra Letter request decision required.")
            Else
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
    Private Sub cmb_Validated(sender As Object, e As EventArgs) Handles cmbRetirement.Validated, cmbCobraLetter.Validated

        Dim DR As DataRow
        Dim CBox As ExComboBox = CType(sender, ExComboBox)
        Dim BS As BindingSource

        Try

            If _TBS Is Nothing OrElse _TBS.Position < 0 OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

            BS = DirectCast(_TBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & CBox.Name & " BS(" & BS.Position.ToString & ") Val(" & CBox.SelectedText.ToString & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = DirectCast(_TBS.Current, DataRowView).Row

            'DONOT use BindingSource or Row ENDEDIT Here, it causes intefering events to trigger

            SetUIElements(_REGMReadOnlyAccess) 'call to establish any form changes triggered by selection

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Sub


#End Region

End Class
