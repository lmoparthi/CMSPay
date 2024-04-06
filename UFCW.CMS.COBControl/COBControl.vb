Option Strict On
Option Infer On

Imports System.ComponentModel

Imports DDTek.DB2
Imports System.Data.Common
Imports System.Threading.Tasks
Imports System.Reflection

Public Class COBControl
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _FamilyID As Integer = -1
    Private _RelationID As Short? = Nothing
    Private _ClaimID As Integer = -1
    Private _ReadOnlyMode As Boolean = True
    Private _ViewHistory As Boolean?
    Private _UIState As UIStates

    Private _APPKEY As String

    Private WithEvents _COBBS As New BindingSource
    Private WithEvents _AllCOBDS As New DataSet
    Private WithEvents _COBDS As DataSet

    Private _PayerValuesBS As BindingSource
    Private _PayerValuesDT As DataTable
    Private _RelationShipValuesBS As BindingSource
    Private _RelationShipValuesDT As DataTable

    Private _ChangedDRs As DataSet

    ReadOnly _REGMRegMasterDeleteAccess As Boolean = UFCWGeneralAD.REGMRegMasterDeleteAccess
    ReadOnly _REGMReadOnlyAccess As Boolean = UFCWGeneralAD.REGMReadOnlyAccess
    ReadOnly _CMSCanModifyCOB As Boolean = UFCWGeneralAD.CMSCanModifyCOB
    ReadOnly _REGMVendorAccess As Boolean = UFCWGeneralAD.REGMVendorAccess

    Private _PopulatePayersTask As Task
    Private _PopulateRelationShipsTask As Task

    ReadOnly _DomainUser As String = SystemInformation.UserName

    Private _ControlIsReadOnly As UIStates = UIStates.None Or UIStates.NotModifiable Or UIStates.Modifiable

    Private _Disposed As Boolean = False

    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then

            If COBDataGrid IsNot Nothing Then

                COBDataGrid.Dispose()
            End If

            COBDataGrid = Nothing

            If components IsNot Nothing Then
                components.Dispose()
            End If

        End If

        _Disposed = True
    End Sub


#Region "Constructor"

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        Dim TheDesignMode As Boolean = (LicenseManager.UsageMode = LicenseUsageMode.Designtime)

        If Not TheDesignMode Then

            PreLoad()

        End If

    End Sub
    Private Sub COBControl_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try

            '   Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & "  : (" & _UIState.ToString & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            'If _PopulatePayersTask IsNot Nothing AndAlso _PopulateRelationShipsTask IsNot Nothing Then
            '    Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Mid: Task(" & If(_PopulatePayersTask Is Nothing, "Nothing", "Ready") & ") " & Me.Name & "  : (" & _UIState.ToString & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
            '    Task.WaitAll(_PopulateRelationShipsTask, _PopulatePayersTask)
            'End If
            If _PopulatePayersTask IsNot Nothing Then
                _PopulatePayersTask.Wait()
            End If
            If _PopulateRelationShipsTask IsNot Nothing Then
                _PopulateRelationShipsTask.Wait()
            End If

            ProcessControls()

            '    Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & "  : (" & _UIState.ToString & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Sub

    Public Sub RetrievePayers()

        Try

            '  Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & "  : (" & _UIState.ToString & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            RemoveHandler PayerComboBox.SelectedIndexChanged, AddressOf PayerComboBox_SelectedIndexChanged

            ' If _PopulatePayersTask IsNot Nothing Then

            _PayerValuesDT = CMSDALFDBMD.RetrievePayers().Tables(0)

            _PayerValuesBS = New BindingSource(_PayerValuesDT, "")


            _PopulatePayersTask = Nothing
            '  End If

            ' Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & "  : (" & _UIState.ToString & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            AddHandler PayerComboBox.SelectedIndexChanged, AddressOf PayerComboBox_SelectedIndexChanged
        End Try

    End Sub

    Public Sub RetrieveRelationShipValues()
        Try

            '  Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & "  : (" & _UIState.ToString & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            RemoveHandler OTH_RELATIONComboBox.SelectedIndexChanged, AddressOf OTH_RELATIONComboBox_SelectedIndexChanged

            ' If _PopulateRelationShipsTask IsNot Nothing Then

            _RelationShipValuesDT = CMSDALFDBMD.RetrieveRelationShipValues()
            ' _PopulateRelationShipsTask = Nothing
            '   End If
            If _RelationShipValuesDT IsNot Nothing Then
                Dim NewRow As DataRow = _RelationShipValuesDT.NewRow()
                NewRow(0) = ""
                NewRow(1) = DBNull.Value
                _RelationShipValuesDT.Rows.InsertAt(NewRow, 0)
            End If

            _RelationShipValuesBS = New BindingSource(_RelationShipValuesDT, "")

            ' Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & "  : (" & _UIState.ToString & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        Catch ex As Exception
            Throw
        Finally
            AddHandler OTH_RELATIONComboBox.SelectedIndexChanged, AddressOf OTH_RELATIONComboBox_SelectedIndexChanged
        End Try

    End Sub

    Public Sub PreLoad()

        Try

            _PopulatePayersTask = Task.Factory.StartNew(Sub() RetrievePayers())
            _PopulateRelationShipsTask = Task.Factory.StartNew(Sub() RetrieveRelationShipValues())

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Sub New(ByVal familyID As Integer, ByVal relationID As Short?)
        Me.New()

        _FamilyID = familyID
        _RelationID = relationID
        _ClaimID = -1

    End Sub
    Public Sub New(ByVal familyID As Integer, ByVal relationID As Short?, ByVal claimID As Integer)
        Me.New()

        _FamilyID = familyID
        _RelationID = relationID
        _ClaimID = claimID

    End Sub

#End Region

#Region "Properties"
    <System.ComponentModel.Browsable(True), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Determines if control is in Read Only Mode.")>
    Public Property ReadOnlyMode() As Boolean
        Get

            Return If(_RelationID Is Nothing OrElse _RelationID < 0, True, _ReadOnlyMode) 'In FID only mode the program can only process as Read Only
        End Get

        Set(ByVal Value As Boolean)

            _ReadOnlyMode = Value

            'When forced to Read Only the UIState can only be used NotModifiable
            _UIState = If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable)

            ProcessControls()

        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Indicates that Save has not been used.")>
    Public ReadOnly Property ChangesPending() As Boolean
        Get
            Return If(_COBDS Is Nothing, False, _COBDS.HasChanges)
        End Get
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the FamilyID of the Document.")>
    Public Property FamilyID() As Integer
        Get
            Return _FamilyID
        End Get
        Set(ByVal value As Integer)
            _FamilyID = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the FamilyID of the Document.")>
    Public Property ClaimID() As Integer
        Get
            Return _ClaimID
        End Get
        Set(ByVal value As Integer)
            _ClaimID = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the FamilyID of the Document.")>
    Public Property RelationID() As Short?
        Get
            Return _RelationID
        End Get
        Set(ByVal value As Short?)
            _RelationID = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Can provide COB data to process for caching purpose")>
    Public Property COBDataSet() As DataSet
        Get
            Return _COBDS
        End Get
        Set(ByVal value As DataSet)
            _COBDS.Clear()
            _COBDS.Merge(value)
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

#End Region

#Region "Custom Subs\Functions"

    Public Sub LoadCOB(ByVal theFamilyID As Integer, ByVal theRelationID As Short?)
        Try
            ClearAll()

            _FamilyID = theFamilyID
            _RelationID = theRelationID

            LoadCOB()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub LoadCOB(ByVal theFamilyID As Integer, ByVal theRelationID As Short?, ByVal cobDS As DataSet)
        Try

            ClearAll()

            _FamilyID = theFamilyID
            _RelationID = theRelationID
            _AllCOBDS = cobDS

            LoadCOB()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub LoadCOB(ByVal theFamilyID As Integer, ByVal theRelationID As Short?, ByVal theClaimID As Integer)
        Try

            ClearAll()

            _FamilyID = theFamilyID
            _RelationID = theRelationID
            _ClaimID = theClaimID

            LoadCOB()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub LoadCOB()

        Try
            Me.SuspendLayout()

            If Not _CMSCanModifyCOB Then 'override externally selected access level if user does not have appropriate security
                _ReadOnlyMode = True
            End If

            ProcessControls(Me, True, False)

            ClearErrors()
            ClearDataBindings(Me)

            If _AllCOBDS Is Nothing OrElse _AllCOBDS.Tables.Count < 1 OrElse _AllCOBDS.Tables("MEDOTHER_INS").Rows.Count < 1 Then 'Data may have been provided to control, so don't repeat data retrieval
                _AllCOBDS = CType(CMSDALFDBMD.RetrieveCOBInfo(_FamilyID, -1, _AllCOBDS), DataSet)
            End If

            _COBDS = New DataSet
            _COBDS.Tables.Add(_AllCOBDS.Tables("MEDOTHER_INS").Clone)

            For Each DR As DataRow In _AllCOBDS.Tables("MEDOTHER_INS").Select(If(_RelationID Is Nothing OrElse _RelationID < 0, "", "RELATION_ID=" & _RelationID.ToString))
                _COBDS.Tables("MEDOTHER_INS").ImportRow(DR)
            Next

            _COBDS.Tables("MEDOTHER_INS").AcceptChanges() 'remove modified row flag

            AddHandler _COBDS.Tables("MEDOTHER_INS").RowChanging, AddressOf _COBDS_RowChanging
            AddHandler _COBDS.Tables("MEDOTHER_INS").RowChanged, AddressOf _COBDS_RowChanged
            AddHandler _COBDS.Tables("MEDOTHER_INS").ColumnChanging, AddressOf _COBDS_ColumnChanging
            AddHandler _COBDS.Tables("MEDOTHER_INS").ColumnChanged, AddressOf _COBDS_ColumnChanged

            _COBBS = New BindingSource
            _COBBS.RaiseListChangedEvents = False
            _COBBS.DataMember = "MEDOTHER_INS"
            _COBBS.DataSource = _COBDS

            COBDataGrid.DataMember = ""
            COBDataGrid.DataSource = _COBBS
            COBDataGrid.SetTableStyle()
            COBDataGrid.Sort = If(COBDataGrid.LastSortedBy, COBDataGrid.DefaultSort)

            _COBBS.RaiseListChangedEvents = True

            LoadCOBDataBindings()

            _COBBS.ResetBindings(False)

        Catch ex As Exception
            Throw
        Finally

            SetUIElements()

            Me.ResumeLayout(True)
        End Try
    End Sub

    Private Sub _COBDS_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs)
        Dim BS As BindingSource

        Try

            BS = DirectCast(_COBBS, BindingSource)

            '    Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " : " & e.Row(e.Column).ToString & "\" & If(e.ProposedValue Is Nothing, "NULL", e.ProposedValue.ToString) & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            '    Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " : " & e.Row(e.Column).ToString & "\" & If(e.ProposedValue Is Nothing, "NULL", e.ProposedValue.ToString) & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub _COBDS_ColumnChanged(sender As Object, e As DataColumnChangeEventArgs)
        Dim BS As BindingSource

        Try

            BS = DirectCast(_COBBS, BindingSource)

            ' Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If BS Is Nothing OrElse BS.Position < 0 OrElse BS.Current Is Nothing OrElse BS.Count < 1 Then Return

            If Not (_UIState And UIStates.[ReadOnly]) = _UIState Then

                Dim DR As DataRow = e.Row

                If _UIState <> UIStates.Canceling Then

                    _ChangedDRs = _COBDS.GetChanges(DataRowState.Added Or DataRowState.Modified Or DataRowState.Deleted)

                    If DR.HasVersion(DataRowVersion.Proposed) OrElse (_ChangedDRs IsNot Nothing AndAlso _ChangedDRs.Tables("MEDOTHER_INS").Rows.Count > 0) Then
                        '   Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Mi1:  BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
                        Me.SaveActionButton.Enabled = True
                        Me.CancelActionButton.Enabled = True

                    End If

                End If
            End If

        Catch ex As Exception
            Throw
        Finally
            'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub _COBDS_RowChanging(sender As Object, e As DataRowChangeEventArgs)

        Dim BS As BindingSource

        Try

            BS = DirectCast(_COBBS, BindingSource)

            ' Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & e.Action.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            ' Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & e.Action.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

    Private Sub _COBDS_RowChanged(sender As Object, e As DataRowChangeEventArgs)
        Dim BS As BindingSource

        Try

            BS = DirectCast(_COBBS, BindingSource)

            ' Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " :  " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            SetUIElements()

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " :  " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    'Private Sub SetUIElements(readOnlyMode As Boolean)

    '    Dim DR As DataRow

    '    Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

    '    Try

    '        Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

    '        grpEditPanel.SuspendLayout()

    '        If Not readOnlyMode Then readOnlyMode = Not If(_APPKEY.ToUpper.Contains("CLAIMS"), _CMSCanModifyCOB, _REGMReadOnlyAccess)

    '        If _REGMVendorAccess Then _ViewHistory = False

    '        If _COBBS IsNot Nothing AndAlso _COBBS.Position > -1 AndAlso _COBBS.Current IsNot Nothing AndAlso _COBBS.Count > 0 Then
    '            DR = CType(_COBBS.Current, DataRowView).Row
    '        End If

    '        If DR IsNot Nothing AndAlso (_ViewHistory Is Nothing OrElse _ViewHistory) Then
    '            Me.HistoryButton.Enabled = True
    '            Me.HistoryButton.Visible = True
    '        End If

    '        CancelActionButton.Enabled = False
    '        NextButton.Enabled = False
    '        PrevButton.Enabled = False

    '        Dim AddedQuery As EnumerableRowCollection(Of DataRow) = (From Added As DataRow In _COBDS.Tables("MEDOTHER_INS").AsEnumerable()
    '                                                                 Where Added.RowState = DataRowState.Added)

    '        ProcessSubControls(CType(grpEditPanel, Object), If(DR Is Nothing OrElse readOnlyMode, True, False), True) 'lock everything down except buttons

    '        If readOnlyMode Then

    '            AddActionButton.Visible = False
    '            DeleteActionButton.Visible = False

    '            CancelActionButton.Visible = False
    '            SaveActionButton.Visible = False

    '            If _COBBS IsNot Nothing AndAlso _COBBS.Count > 0 Then

    '                Me.PrevButton.Visible = True
    '                Me.NextButton.Visible = True

    '                If _COBBS.Position < _COBBS.Count AndAlso _COBBS.Count > 1 Then
    '                    NextButton.Enabled = True
    '                End If

    '                If _COBBS.Position > 0 AndAlso _COBBS.Count > 1 Then
    '                    PrevButton.Enabled = True
    '                End If

    '            End If

    '        Else

    '            AddActionButton.Visible = True

    '            If _REGMRegMasterDeleteAccess Then
    '                DeleteActionButton.Visible = True
    '            End If

    '            CancelActionButton.Visible = True
    '            SaveActionButton.Visible = True

    '            If _COBDS.HasChanges Then
    '                Me.CancelActionButton.Enabled = True
    '            End If

    '            If DR IsNot Nothing Then 'based upon row status / content decide how to present controls

    '                InsurerFreeFormButton.Enabled = True

    '                If DR.RowState = DataRowState.Added Then

    '                    'txtFromDate.ReadOnly = False
    '                    'txtThruDate.ReadOnly = False
    '                    'cmbLifeEvent.ReadOnly = False
    '                    'txtEventDate.ReadOnly = False
    '                    'cmbTermCode.ReadOnly = False
    '                    'txtTermDate.ReadOnly = False

    '                    AddActionButton.Enabled = False
    '                    DeleteActionButton.Enabled = False

    '                ElseIf DR.RowState = DataRowState.Modified Then

    '                    'txtThruDate.ReadOnly = False
    '                    'cmbLifeEvent.ReadOnly = False
    '                    'txtEventDate.ReadOnly = False
    '                    'cmbTermCode.ReadOnly = False
    '                    'txtTermDate.ReadOnly = False

    '                    AddActionButton.Enabled = False
    '                    DeleteActionButton.Enabled = False

    '                ElseIf DR.RowState = DataRowState.Unchanged Then

    '                    If _COBBS IsNot Nothing AndAlso _COBBS.Count > 0 Then

    '                        Me.PrevButton.Visible = True
    '                        Me.NextButton.Visible = True

    '                        If _COBBS.Position < _COBBS.Count AndAlso _COBBS.Count > 1 Then
    '                            NextButton.Enabled = True
    '                        End If

    '                        If _COBBS.Position > 0 AndAlso _COBBS.Count > 1 Then
    '                            PrevButton.Enabled = True
    '                        End If

    '                    End If

    '                    SaveActionButton.Enabled = False

    '                    If Not AddedQuery.Any Then
    '                        AddActionButton.Enabled = True
    '                    End If

    '                Else

    '                    AddActionButton.Enabled = False

    '                End If

    '            Else
    '                AddActionButton.Enabled = True
    '                DeleteActionButton.Enabled = False

    '            End If
    '        End If

    '        grpEditPanel.ResumeLayout() 'needed to ensure transparent controls child controls draw correctly 
    '        grpEditPanel.Refresh()

    '    Catch ex As Exception

    '        Throw

    '    Finally
    '        Me.AutoValidate = HoldAutoValidate
    '    End Try

    'End Sub

    Private Sub SetUIElements(Optional userIntefaceState As UIStates = UIStates.AsIs)

        If Not userIntefaceState.HasFlag(UIStates.AsIs) Then _UIState = userIntefaceState

        _UIState = If(_RelationID Is Nothing OrElse _RelationID < 0, UIStates.NotModifiable, _UIState)

        Dim DR As DataRow

        Dim SavedAutoValidateState As AutoValidate
        Dim SavedBSRaiseListChangedEvents As Boolean

        Try

            If _COBBS IsNot Nothing Then
                SavedBSRaiseListChangedEvents = _COBBS.RaiseListChangedEvents
            End If

            'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & If(_COBBS Is Nothing, "N/A", _COBBS.Position.ToString) & ") " & Me.Name & "  : (" & _UIState.ToString & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            SavedAutoValidateState = Me.AutoValidate

            Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable

            If _REGMVendorAccess Then _ViewHistory = False

            ProcessControls(CType(grpEditPanel, Object), True) 'does not affect visibility only edit yes/no

            Me.CancelActionButton.Enabled = False
            Me.CancelActionButton.Visible = False

            Me.SaveActionButton.Enabled = False
            Me.SaveActionButton.Visible = False

            Me.AddActionButton.Enabled = False
            Me.AddActionButton.Visible = False

            Me.DeleteActionButton.Enabled = False
            Me.DeleteActionButton.Visible = False

            Me.HistoryButton.Enabled = False
            Me.HistoryButton.Visible = False

            Me.NextButton.Enabled = False
            Me.PrevButton.Enabled = False

            InsurerFreeFormButton.Enabled = False

            If COBDataGrid IsNot Nothing AndAlso COBDataGrid.DataSource IsNot Nothing Then
                If _COBBS IsNot Nothing AndAlso _COBBS.Count > 0 Then

                    Me.PrevButton.Visible = True
                    Me.NextButton.Visible = True

                    If _COBBS.Position < _COBBS.Count - 1 AndAlso _COBBS.Count > 1 Then
                        Me.NextButton.Enabled = True
                    End If

                    If _COBBS.Position > 0 AndAlso _COBBS.Count > 1 Then
                        Me.PrevButton.Enabled = True
                    End If

                    COBDataGrid.CaptionText = _COBBS.Count.ToString & " of " & _COBDS.Tables("MEDOTHER_INS").Select("FAMILY_ID = " & _FamilyID & If(_RelationID Is Nothing, "", " AND RELATION_ID = " & _RelationID.ToString)).Length & If(_COBDS.Tables("MEDOTHER_INS").Rows.Count = 1, " Insurance", " Insurance(s)")
                Else
                    COBDataGrid.CaptionText = "0 of " & _COBDS.Tables("MEDOTHER_INS").Select("FAMILY_ID = " & _FamilyID & If(_RelationID Is Nothing, "", " AND RELATION_ID = " & _RelationID.ToString)).Length & If(_COBDS.Tables("MEDOTHER_INS").Rows.Count = 1, " Insurance", " Insurance(s)")
                End If
                '    COBDataGrid.CaptionText &= If(_RelationID Is Nothing, "", " displayed for RelationID: " & _RelationID.ToString)
                'ElseIf _RelationID IsNot Nothing AndAlso _RelationID > -1 Then
                '    COBDataGrid.CaptionText = "Relation: " & _RelationID.ToString & " displayed"
                'Else
                '    COBDataGrid.CaptionText = ""
            End If

            If Not _UIState = UIStates.None AndAlso (_ViewHistory Is Nothing OrElse _ViewHistory) Then
                HistoryButton.Enabled = True
                HistoryButton.Visible = True
            End If

            If SavedBSRaiseListChangedEvents Then
                _COBBS.RaiseListChangedEvents = False
            End If

            If _UIState <> UIStates.Canceling AndAlso _COBBS IsNot Nothing AndAlso _COBBS.Count > 0 AndAlso _COBBS.Position > -1 Then
                DR = DirectCast(_COBBS.Current, DataRowView).Row
            End If

            Dim ModifiedQuery As EnumerableRowCollection(Of DataRow)

            If _COBDS IsNot Nothing AndAlso _COBDS.Tables("MEDOTHER_INS") IsNot Nothing Then
                ModifiedQuery = (From Modified As DataRow In _COBDS.Tables("MEDOTHER_INS").AsEnumerable()
                                 Where Modified.RowState <> DataRowState.Unchanged)
            End If

            Select Case _UIState 'this block resets state for remainder of control processing 

                Case UIStates.Canceled

                    _UIState = If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable)

                Case UIStates.Saved

                    _UIState = If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable)

                Case UIStates.Rejected

                    _UIState = If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable)

                Case UIStates.Approved

                    _UIState = If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable)

                Case UIStates.Deleted

                    _UIState = If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable)

                Case UIStates.Archived 'not implemented

                    _UIState = If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable)
            End Select

            Select Case _UIState
                Case UIStates.None

                    Return

                Case UIStates.Rejecting

                    Me.AddActionButton.Visible = True
                    Me.DeleteActionButton.Visible = True
                    Me.CancelActionButton.Visible = True
                    Me.SaveActionButton.Visible = True

                Case UIStates.Saving

                    ProcessControls(CType(grpEditPanel, Object), False, True) 'allow controls to validate

                Case UIStates.Deleting

                    If ModifiedQuery.Any Then

                        Me.CancelActionButton.Visible = True
                        Me.SaveActionButton.Visible = True

                        SaveActionButton.Enabled = True
                        SaveActionButton.Text = "Save"
                    End If

                Case UIStates.Canceling

                Case UIStates.Viewing

                Case UIStates.NotModifiable

                Case UIStates.Modifiable, UIStates.Adding, UIStates.Modifying

                    Me.AddActionButton.Visible = True
                    Me.DeleteActionButton.Visible = True
                    Me.SaveActionButton.Visible = True
                    Me.CancelActionButton.Visible = True

                    Select Case _UIState

                        Case UIStates.Modifiable

                            If _COBDS IsNot Nothing AndAlso _COBDS.Tables("MEDOTHER_INS") IsNot Nothing Then
                                If Not ModifiedQuery.Any Then

                                    If _COBDS.Tables("MEDOTHER_INS").Rows.Count > 0 Then
                                        DeleteActionButton.Enabled = True
                                        SaveActionButton.Enabled = True
                                        SaveActionButton.Text = "Update"
                                    End If

                                    AddActionButton.Enabled = True
                                End If
                            End If

                        Case UIStates.Adding, UIStates.Modifying

                            SaveActionButton.Text = "Save"

                            If DR.RowState = DataRowState.Added Then
                                OCC_TO_DATETextBox.ReadOnly = True
                            End If

                            Select Case PayerComboBox.Text.Trim.ToUpper
                                Case "OTHER"
                                    InsurerFreeFormButton.Enabled = True

                            End Select

                            ProcessControls(CType(grpEditPanel, Object), False, True)

                            'If DR IsNot Nothing AndAlso CDate(DR("OCC_TO_DATE")).Year = 9999 AndAlso (DR.HasVersion(DataRowVersion.Proposed) OrElse DR.RowState = DataRowState.Modified OrElse DR.RowState = DataRowState.Added) Then

                            '    ProcessControls(CType(grpEditPanel, Object), False, True)

                            'Else
                            '    ClearErrors()
                            'End If

                            Me.CancelActionButton.Enabled = True

                    End Select
            End Select

            If DR IsNot Nothing AndAlso Not _UIState = UIStates.Saving Then

                If DR.HasVersion(DataRowVersion.Proposed) OrElse DR.RowState <> DataRowState.Unchanged Then
                    Me.SaveActionButton.Enabled = True
                End If
            End If

        Catch ex As Exception

            Throw

        Finally

            If OCC_FROM_DATETextBox.ReadOnly = True Then
                ClearErrors()
            End If

            If _COBDS IsNot Nothing AndAlso Not (_UIState = UIStates.Saving OrElse _UIState = UIStates.Canceling) Then
                _ChangedDRs = _COBDS.GetChanges(DataRowState.Added Or DataRowState.Modified Or DataRowState.Deleted)
                If _ChangedDRs IsNot Nothing AndAlso _ChangedDRs.Tables("MEDOTHER_INS").Rows.Count > 0 Then
                    Me.CancelActionButton.Enabled = True
                End If
            End If

            Me.AutoValidate = SavedAutoValidateState

            If SavedBSRaiseListChangedEvents Then
                _COBBS.RaiseListChangedEvents = True
            End If

            grpEditPanel.Refresh()

            ' Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & If(_COBBS Is Nothing, "N/A", _COBBS.Position.ToString) & ") " & Me.Name & "  : (" & _UIState.ToString & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

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

            'do not dispose as items are passed byref
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
                Else
                    Stop
                End If

            End If

            For Each ChildCtrl As Object In ProcessingControl.Controls
                ProcessSubControls(ChildCtrl, readOnlyMode, excludeButtons)
            Next

            'Debug.Print(If(ProcessingControl.Parent.Name IsNot Nothing, ProcessingControl.Parent.Name & " : ", "") & ProcessingControl.Name & " : " & ctrl.GetType.ToString & " : " & If(TypeOf (ctrl) Is TextBox, CType(ctrl, TextBox).ReadOnly, ProcessingControl.Enabled).ToString & " -> " & If(TypeOf (ctrl) Is TextBox, readOnlyMode, Not readOnlyMode).ToString)

        Catch ex As Exception
            Throw
        Finally

            'do not dispose as items are passed byref
        End Try

    End Sub

    Public Sub ProcessControls()

        If _Disposed Then Return

        Try
            SetUIElements()

        Catch ex As Exception
            Throw
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

    Private Sub LoadCOBDataBindings()
        Dim Bind As Binding

        Try

            OTH_SEXComboBox.DataBindings.Clear()
            Bind = New Binding("SelectedValue", _COBBS, "OTH_SEX", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            Bind.DataSourceNullValue = DBNull.Value
            AddHandler Bind.BindingComplete, AddressOf StringBinding_BindingComplete
            OTH_SEXComboBox.DataBindings.Add(Bind)

            RemoveHandler OTH_RELATIONComboBox.SelectedIndexChanged, AddressOf OTH_RELATIONComboBox_SelectedIndexChanged
            RemoveHandler OTH_RELATIONComboBox.SelectedValueChanged, AddressOf OTH_RELATIONComboBox_SelectedValueChanged

            Me.OTH_RELATIONComboBox.DataSource = _RelationShipValuesBS
            Me.OTH_RELATIONComboBox.DisplayMember = "DESCRIPTION"

            OTH_RELATIONComboBox.DataBindings.Clear()
            Bind = New Binding("SelectedValue", _COBBS, "OTH_RELATION", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            Bind.DataSourceNullValue = DBNull.Value
            AddHandler Bind.BindingComplete, AddressOf StringBinding_BindingComplete
            OTH_RELATIONComboBox.DataBindings.Add(Bind)

            If _COBBS.Count < 1 Then OTH_RELATIONComboBox.SelectedIndex = -1

            AddHandler OTH_RELATIONComboBox.SelectedValueChanged, AddressOf OTH_RELATIONComboBox_SelectedValueChanged
            AddHandler OTH_RELATIONComboBox.SelectedIndexChanged, AddressOf OTH_RELATIONComboBox_SelectedIndexChanged

            RemoveHandler PayerComboBox.SelectedIndexChanged, AddressOf PayerComboBox_SelectedIndexChanged
            RemoveHandler PayerComboBox.SelectedValueChanged, AddressOf PayerComboBox_SelectedValueChanged

            'Debug.Print("Me.PayerComboBox.DataSource: " & _PayerValuesBS.Count.ToString)
            Me.PayerComboBox.DataSource = _PayerValuesBS
            Me.PayerComboBox.DisplayMember = "PAYER_NAME"

            PayerComboBox.DataBindings.Clear()
            Bind = New Binding("SelectedValue", _COBBS, "OTH_PAYER_ID", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            Bind.DataSourceNullValue = DBNull.Value
            AddHandler Bind.BindingComplete, AddressOf StringBinding_BindingComplete
            PayerComboBox.DataBindings.Add(Bind)

            If _COBBS.Count < 1 Then PayerComboBox.SelectedIndex = -1

            AddHandler PayerComboBox.SelectedValueChanged, AddressOf PayerComboBox_SelectedValueChanged
            AddHandler PayerComboBox.SelectedIndexChanged, AddressOf PayerComboBox_SelectedIndexChanged

            FamilyMembersWithOILabel.DataBindings.Clear()
            Bind = New Binding("Text", _AllCOBDS.Tables("MEDOTHER_INS_COUNT"), "FAMILY_MEMBERS_WITH_OI")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.Never
            FamilyMembersWithOILabel.DataBindings.Add(Bind)

            OCC_TO_DATETextBox.DataBindings.Clear()
            Bind = New Binding("Text", _COBBS, "OCC_TO_DATE", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            Bind.FormatString = "MM-dd-yyyy"
            AddHandler Bind.Parse, AddressOf UFCWGeneral.DateOnlyBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf DateOnlyBinding_BindingComplete
            OCC_TO_DATETextBox.DataBindings.Add(Bind)

            OCC_FROM_DATETextBox.DataBindings.Clear()
            Bind = New Binding("Text", _COBBS, "OCC_FROM_DATE", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            Bind.FormatString = "MM-dd-yyyy"
            AddHandler Bind.Parse, AddressOf UFCWGeneral.DateOnlyBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf DateOnlyBinding_BindingComplete
            OCC_FROM_DATETextBox.DataBindings.Add(Bind)

            OTH_DOBTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _COBBS, "OTH_DOB", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            Bind.FormatString = "MM-dd-yyyy"
            AddHandler Bind.Parse, AddressOf UFCWGeneral.DateOnlyBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf DateOnlyBinding_BindingComplete
            OTH_DOBTextBox.DataBindings.Add(Bind)

            UPDATE_REASONTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _COBBS, "UPDATE_REASON")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            UPDATE_REASONTextBox.DataBindings.Add(Bind)

            OTH_INS_REFUSAL_SWCheckBox.DataBindings.Clear()
            Bind = New Binding("Checked", _COBBS, "OTH_INS_REFUSAL_SW")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            OTH_INS_REFUSAL_SWCheckBox.DataBindings.Add(Bind)

            WORKING_SPOUSE_SWCheckBox.DataBindings.Clear()
            Bind = New Binding("Checked", _COBBS, "WORKING_SPOUSE_SW")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged
            WORKING_SPOUSE_SWCheckBox.DataBindings.Add(Bind)

            OTH_PAT_ACCT_NBRTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _COBBS, "OTH_PAT_ACCT_NBR")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            OTH_PAT_ACCT_NBRTextBox.DataBindings.Add(Bind)

            HICNTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _COBBS, "HICN")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            HICNTextBox.DataBindings.Add(Bind)

            OTH_POLICYTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _COBBS, "OTH_POLICY")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            OTH_POLICYTextBox.DataBindings.Add(Bind)

            PHONETextBox.DataBindings.Clear()
            Bind = New Binding("Text", _COBBS, "OTH_TAXID")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            PHONETextBox.DataBindings.Add(Bind)

            OTH_SSNTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _COBBS, "OTH_SSN")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            AddHandler Bind.Format, AddressOf SSNBinding_Format
            AddHandler Bind.Parse, AddressOf SSNBinding_Parse
            OTH_SSNTextBox.DataBindings.Add(Bind)

            OTH_FNAMETextBox.DataBindings.Clear()
            Bind = New Binding("Text", _COBBS, "OTH_FNAME")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            OTH_FNAMETextBox.DataBindings.Add(Bind)

            OTH_LNAMETextBox.DataBindings.Clear()
            Bind = New Binding("Text", _COBBS, "OTH_LNAME")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            OTH_LNAMETextBox.DataBindings.Add(Bind)

            DOCIDTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _COBBS, "DOCID")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            AddHandler Bind.Parse, AddressOf Numeric2NullBinding_Parse
            DOCIDTextBox.DataBindings.Add(Bind)

            OTH_SUB_ACCT_NBRTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _COBBS, "OTH_SUB_ACCT_NBR")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            OTH_SUB_ACCT_NBRTextBox.DataBindings.Add(Bind)

            ADDRESS_LINE1TextBox.DataBindings.Clear()
            Bind = New Binding("Text", _COBBS, "ADDRESS_LINE1")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            ADDRESS_LINE1TextBox.DataBindings.Add(Bind)

            ADDRESS_LINE2TextBox.DataBindings.Clear()
            Bind = New Binding("Text", _COBBS, "ADDRESS_LINE2")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            ADDRESS_LINE2TextBox.DataBindings.Add(Bind)

            CITYTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _COBBS, "CITY")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            CITYTextBox.DataBindings.Add(Bind)

            STATETextBox.DataBindings.Clear()
            Bind = New Binding("Text", _COBBS, "STATE")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            STATETextBox.DataBindings.Add(Bind)

            ZIPTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _COBBS, "ZIP", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            AddHandler Bind.Format, AddressOf LeadingZeroesSize5Binding_Format
            AddHandler Bind.Parse, AddressOf Numeric2NullBinding_Parse
            ZIPTextBox.DataBindings.Add(Bind)

            ZIP_4TextBox.DataBindings.Clear()
            Bind = New Binding("Text", _COBBS, "ZIP_4", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            AddHandler Bind.Format, AddressOf LeadingZeroesSize4Binding_format
            AddHandler Bind.Parse, AddressOf Numeric2NullBinding_Parse
            ZIP_4TextBox.DataBindings.Add(Bind)

            COUNTRYTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _COBBS, "COUNTRY")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            COUNTRYTextBox.DataBindings.Add(Bind)

            PHONETextBox.DataBindings.Clear()
            Bind = New Binding("Text", _COBBS, "PHONE")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            AddHandler Bind.Parse, AddressOf Numeric2NullBinding_Parse
            PHONETextBox.DataBindings.Add(Bind)

            EXTENSION1TextBox.DataBindings.Clear()
            Bind = New Binding("Text", _COBBS, "EXTENSION1")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            AddHandler Bind.Parse, AddressOf Numeric2NullBinding_Parse
            EXTENSION1TextBox.DataBindings.Add(Bind)

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Sub

    Private Sub OTH_RELATIONComboBox_SelectedValueChanged(sender As Object, e As EventArgs)
        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Try

            'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _COBBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If ErrorProvider1.GetError(Me.OTH_RELATIONComboBox) = " Select a valid relationship " Then
                ClearError(ErrorProvider1, Me.OTH_RELATIONComboBox)
            End If

        Catch ex As Exception
            Throw
        Finally

            ' Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _COBBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try
    End Sub

    Private Sub StringBinding_BindingComplete(sender As Object, e As BindingCompleteEventArgs)
        Dim ControlBinding As Binding
        Dim BS As BindingSource

        Try

            ControlBinding = CType(sender, Binding)
            BS = DirectCast(_COBBS, BindingSource)

            'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " : (" & ControlBinding.Control.Name & ") " & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If Not e.BindingCompleteState = BindingCompleteState.Success Then
                Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " IO:BS(" & BS.Position.ToString & ") " & Me.Name & " : (" & ControlBinding.Control.Name & ") " & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
                MessageBox.Show("Control " & e.Binding.Control.Name & " " & e.ErrorText, "Problem converting data to database format", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            Throw
        Finally
            'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " : (" & ControlBinding.Control.Name & ") " & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

    'Private Sub SetUINavigation(uiState As UIStates)

    '    Try

    '        Me.PrevButton.Enabled = False
    '        Me.NextButton.Enabled = False
    '        Me.DeleteActionButton.Enabled = False
    '        Me.AddActionButton.Enabled = False
    '        Me.SaveActionButton.Enabled = False
    '        Me.CancelActionButton.Enabled = False
    '        Me.AddActionButton.Enabled = False

    '        Me.PrevButton.Visible = False
    '        Me.NextButton.Visible = False
    '        Me.DeleteActionButton.Visible = False
    '        Me.SaveActionButton.Visible = False
    '        Me.CancelActionButton.Visible = False
    '        Me.AddActionButton.Visible = False

    '        If uiState = UIStates.None Then Return

    '        Select Case uiState
    '            Case UIStates.NotModifiable
    '                Me.PayerTextBox.Visible = True
    '                Me.OTH_RELATIONTextBox.Visible = True
    '                Me.OTH_SEXTextBox.Visible = True

    '            Case UIStates.Modifiable, UIStates.Adding, UIStates.Modifying

    '                Me.AddActionButton.Visible = True

    '                If uiState = UIStates.Modifiable Then
    '                    Me.AddActionButton.Enabled = True
    '                End If

    '                If _COBBS IsNot Nothing AndAlso _COBBS.Position > -1 Then

    '                    Me.DeleteActionButton.Visible = True
    '                    Me.SaveActionButton.Visible = True
    '                    Me.CancelActionButton.Visible = True

    '                    Dim DR As DataRow = DirectCast(_COBBS.Current, DataRowView).Row
    '                    If DR.RowState <> DataRowState.Unchanged Then
    '                        SaveActionButton.Enabled = True
    '                        CancelActionButton.Enabled = True
    '                    ElseIf uiState = UIStates.Modifiable Then
    '                        Me.DeleteActionButton.Enabled = True
    '                    End If

    '                End If

    '                If Not uiState = UIStates.Modifiable Then Return

    '            Case UIStates.Deleting

    '        End Select

    '        If _COBBS IsNot Nothing AndAlso _COBBS.Count > 0 Then

    '            Me.PrevButton.Visible = True
    '            Me.NextButton.Visible = True

    '            If _COBBS.Position < _COBBS.Count AndAlso _COBBS.Count > 1 Then
    '                NextButton.Enabled = True
    '            End If

    '            If _COBBS.Position > 0 AndAlso _COBBS.Count > 1 Then
    '                PrevButton.Enabled = True
    '            End If

    '        End If

    '    Catch ex As Exception
    '        Throw
    '    Finally

    '    End Try

    'End Sub

    'Private Sub SetNonBoundItems()

    '    Try


    '        If _COBBS IsNot Nothing AndAlso _COBBS.Position > -1 Then

    '            Dim DR As DataRow = DirectCast(_COBBS.Current, DataRowView).Row
    '            If Not IsDBNull(DR("OTH_SEX")) AndAlso DR("OTH_SEX").ToString.Trim.Length > 0 Then
    '                OTH_SEXTextBox.Text = DR("OTH_SEX").ToString
    '                OTH_SEXComboBox.SelectedItem = DR("OTH_SEX")
    '                OTH_SEXComboBox.SelectedValue = DR("OTH_SEX")
    '            Else
    '                OTH_SEXTextBox.Text = ""
    '                OTH_SEXComboBox.SelectedIndex = -1
    '            End If

    '            If Not IsDBNull(DR("OTH_RELATION")) AndAlso DR("OTH_RELATION").ToString.Trim.Length > 0 Then
    '                OTH_RELATIONTextBox.Text = DR("DESCRIPTION").ToString
    '                OTH_RELATIONComboBox.Text = DR("DESCRIPTION").ToString
    '                OTH_RELATIONComboBox.SelectedValue = DR("OTH_RELATION")
    '            Else
    '                OTH_RELATIONTextBox.Text = ""
    '                OTH_RELATIONComboBox.SelectedIndex = -1
    '            End If

    '            If Not IsDBNull(DR("PAYER_NAME")) Then
    '                PayerTextBox.Text = DR("PAYER_NAME").ToString
    '                PayerComboBox.Text = DR("PAYER_NAME").ToString
    '                PayerComboBox.SelectedValue = DR("OTH_PAYER_ID")
    '            Else
    '                PayerComboBox.SelectedIndex = -1
    '            End If

    '        Else

    '            PayerTextBox.Text = ""
    '            PayerComboBox.SelectedIndex = -1

    '            OTH_SEXTextBox.Text = ""
    '            OTH_RELATIONTextBox.Text = ""

    '        End If

    '    Catch ex As Exception
    '        Throw
    '    Finally

    '    End Try

    'End Sub

    Private Sub PayerComboBox_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles PayerComboBox.SelectionChangeCommitted

        Dim DR As DataRow
        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Try

            If _COBBS Is Nothing OrElse _COBBS.Position < 0 OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

            '  Debug.Print(UFCWGeneral.NowDate.ToString("HH: mm:ss.fffffff") & " In:  BS(" & _COBBS.Position.ToString & ") SI(" & CBox.Text & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            'CType(CBox.Parent, TransparentContainer).ValidateChildren() 'this will trigger validation of the cmbbox triggering write of value to DS

            '   Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _COBBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally

        End Try


    End Sub

    Private Sub PayerComboBox_Validating(sender As Object, e As CancelEventArgs) Handles PayerComboBox.Validating
        Dim Cbox As ExComboBox = CType(sender, ExComboBox)

        Dim DR As DataRow

        Try

            ClearError(ErrorProvider1, Cbox)

            If _COBBS Is Nothing OrElse _COBBS.Position < 0 OrElse _COBBS.Count < 1 OrElse Cbox.SelectedIndex < 0 OrElse Cbox.ReadOnly Then Return

            DR = DirectCast(_COBBS.Current, DataRowView).Row

            If Cbox.SelectedIndex < 0 Then
                ErrorProvider1.SetErrorWithTracking(Cbox, " Must Provide a Insurer")
            End If

            If ErrorProvider1.GetError(Cbox).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
            Else
                ' Add
                ' Current will exist with a value of null until a item has been selected and validated
                ' Proposed will not exist until beginedit and will cease to exist when Endedit is called
                ' Original will not exist until Save (AcceptChanges) is called
                If DR("OTH_PAYER_ID").ToString <> Cbox.SelectedValue.ToString Then

                    DR.BeginEdit() 'indicate push to datasource is expected.

                    ' Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " BE:  BS(" & _COBBS.Position.ToString & ") SI(" & Cbox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

                End If
            End If

        Catch ex As Exception
            Throw
        Finally

        End Try
    End Sub

    Private Sub PayerComboBox_Validated(sender As Object, e As EventArgs) Handles PayerComboBox.Validated
        Dim Cbox As ExComboBox = CType(sender, ExComboBox)

        Dim DR As DataRow

        If _Disposed OrElse Cbox.ReadOnly OrElse Cbox.SelectedIndex < 0 Then Return

        Try

            ' Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _COBBS.Position.ToString & ") SI(" & Cbox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = DirectCast(_COBBS.Current, DataRowView).Row


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


            If DR.HasVersion(DataRowVersion.Proposed) AndAlso (DR("OTH_PAYER_ID", DataRowVersion.Proposed).ToString <> DR("OTH_PAYER_ID", DataRowVersion.Current).ToString OrElse DR("OTH_PAYER_ID", DataRowVersion.Proposed).ToString <> Cbox.SelectedValue.ToString) Then
                'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " EE: BS(" & _COBBS.Position.ToString & ") SI(" & Cbox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
                DR("PAYER_NAME") = Cbox.Text
                DR.EndEdit()
            End If

            Me.ToolTip1.SetToolTip(Cbox, Cbox.Text)

        Catch ex As Exception
            Throw
        Finally

            'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _COBBS.Position.ToString & ") SI(" & Cbox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try
    End Sub

    Private Sub PayerComboBox_SelectedValueChanged(sender As Object, e As EventArgs) Handles PayerComboBox.SelectedValueChanged
        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        If _Disposed OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

        Try

            ' Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _COBBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally

            '   Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _COBBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try
    End Sub

    Private Sub PayerComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PayerComboBox.SelectedIndexChanged

        Dim Cbox As ExComboBox = CType(sender, ExComboBox)
        Dim DR As DataRow

        If _Disposed OrElse Cbox.ReadOnly OrElse Cbox.SelectedIndex < 0 Then Return

        Try

            '  Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _COBBS.Position.ToString & ") SI(" & Cbox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If Not (_UIState And UIStates.[ReadOnly]) = _UIState Then
                DR = DirectCast(_COBBS.Current, DataRowView).Row
            End If

            'If IsDBNull(DR("PAYER_NAME")) OrElse (Not IsDBNull(DR("PAYER_NAME")) AndAlso Not DR("PAYER_NAME").ToString.Equals(Cbox.Text)) Then
            '        DR("PAYER_NAME") = Cbox.Text
            '    End If

            '    If IsDBNull(DR("OTH_PAYER_ID")) OrElse (Not IsDBNull(DR("OTH_PAYER_ID")) AndAlso Not DR("OTH_PAYER_ID").ToString.Equals(If(Cbox.SelectedValue Is Nothing, Nothing, Cbox.SelectedValue.ToString))) Then
            '        DR("OTH_PAYER_ID") = If(Cbox.SelectedValue Is Nothing, DBNull.Value, Cbox.SelectedValue)
            '    End If

            '    Me.ToolTip1.SetToolTip(Cbox, Cbox.Text)

            '    ErrorProvider1.SetErrorWithTracking(InsurerFreeFormButton, "")

            '    InsurerFreeFormButton.Enabled = False

            '    Me.ToolTip1.SetToolTip(Me.InsurerFreeFormButton, "")

            '    Select Case DR("PAYER_NAME").ToString.Trim.ToUpper.ToString
            '        Case "OTHER"
            '            InsurerFreeFormButton.Enabled = True

            '            If IsDBNull(DR("OTH_COMMENTS")) OrElse DR("OTH_COMMENTS").ToString.Trim.Length = 0 Then
            '                ErrorProvider1.SetErrorWithTracking(Me.InsurerFreeFormButton, " Must select a Insurer")
            '                Me.ToolTip1.SetToolTip(Me.InsurerFreeFormButton, "")
            '            Else
            '                Me.ToolTip1.SetToolTip(Me.InsurerFreeFormButton, DR("OTH_COMMENTS").ToString)
            '            End If

            '        Case Nothing

            '            ErrorProvider1.SetErrorWithTracking(Cbox, " Must Provide a Insurer")

            '    End Select


        Catch ex As Exception
            Throw
        Finally

            '  Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _COBBS.Position.ToString & ") SI(" & Cbox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

    Private Sub OTH_SEXComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim Cbox As ComboBox = CType(sender, ComboBox)
        Dim DR As DataRow

        Try

            If _COBBS IsNot Nothing AndAlso _COBBS.Count > 0 AndAlso _COBBS.Position > -1 Then
                DR = DirectCast(_COBBS.Current, DataRowView).Row
                If Not DR("OTH_SEX").ToString = Cbox.Text Then
                    DR("OTH_SEX") = Cbox.Text
                End If
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub OTH_RELATIONComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim Cbox As ComboBox = CType(sender, ComboBox)
        Dim DR As DataRow

        Try

            If _COBBS IsNot Nothing AndAlso _COBBS.Count > 0 AndAlso _COBBS.Position > -1 Then
                DR = DirectCast(_COBBS.Current, DataRowView).Row
                If Not DR("DESCRIPTION").ToString = Cbox.Text Then

                    DR("DESCRIPTION") = Cbox.Text
                    DR("OTH_RELATION") = Cbox.SelectedValue

                    SaveActionButton.Enabled = True
                    CancelActionButton.Enabled = True

                End If
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub DataBinding_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles OTH_DOBTextBox.Validated,
                                                                                                    OTH_RELATIONComboBox.Validated,
                                                                                                    OTH_SEXComboBox.Validated,
                                                                                                    OCC_FROM_DATETextBox.Validated,
                                                                                                    OCC_TO_DATETextBox.Validated,
                                                                                                    OTH_FNAMETextBox.Validated,
                                                                                                    OTH_LNAMETextBox.Validated,
                                                                                                    OTH_PAT_ACCT_NBRTextBox.Validated,
                                                                                                    HICNTextBox.Validated,
                                                                                                    OTH_POLICYTextBox.Validated,
                                                                                                    OTH_RELATIONComboBox.Validated,
                                                                                                    OTH_SEXComboBox.Validated,
                                                                                                    OTH_SSNTextBox.Validated,
                                                                                                    UPDATE_REASONTextBox.Validated,
                                                                                                    DOCIDTextBox.Validated,
                                                                                                    PHONETextBox.Validated,
                                                                                                    OTH_SSNTextBox.Validated,
                                                                                                    OTH_SUB_ACCT_NBRTextBox.Validated,
                                                                                                    ADDRESS_LINE1TextBox.Validated,
                                                                                                    ADDRESS_LINE2TextBox.Validated,
                                                                                                    CITYTextBox.Validated,
                                                                                                    STATETextBox.Validated,
                                                                                                    ZIPTextBox.Validated,
                                                                                                    ZIP_4TextBox.Validated,
                                                                                                    COUNTRYTextBox.Validated,
                                                                                                    PHONETextBox.Validated,
                                                                                                    EXTENSION1TextBox.Validated

        '    Private Sub DataBinding_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles OCC_FROM_DATETextBox.Validated, OCC_TO_DATETextBox.Validated, OTH_RELATIONComboBox.Validated, OTH_SEXComboBox.Validated, UPDATE_REASONTextBox.Validated, DOCIDTextBox.Validated

        Try

            If ErrorProvider1.GetErrorCount = 0 AndAlso _COBDS IsNot Nothing AndAlso _COBDS.Tables("MEDOTHER_INS") IsNot Nothing AndAlso _COBBS IsNot Nothing Then

                _COBBS.EndEdit()

            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Sub ClearAll()

        If _AllCOBDS IsNot Nothing Then
            _AllCOBDS.Clear()
            _AllCOBDS.AcceptChanges()
        End If

        _COBBS.RaiseListChangedEvents = False
        If _COBDS IsNot Nothing Then
            _COBDS.Clear()
            _COBDS.AcceptChanges()
        End If

        If _COBBS IsNot Nothing Then
            _COBBS.ResetBindings(False)
        End If
        COBDataGrid.DataSource = Nothing

        ClearErrors()
        ClearDataBindings(grpEditPanel)

        PayerComboBox.SelectedIndex = -1
        OTH_RELATIONComboBox.SelectedIndex = -1
        OTH_SEXComboBox.SelectedIndex = -1

        _UIState = If(_ReadOnlyMode, UIStates.NotModifiable, UIStates.Modifiable)

    End Sub

    Private Sub PrevButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrevButton.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' moves the current index and displays in the edit window the previous row
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	4/11/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            '_COBBS.EndEdit()

            COBDataGrid.UnSelect(_COBBS.Position)

            If _COBBS.Position > -1 Then
                'If ValidateCOBChanges() Then
                '    Exit Sub
                'End If

                _COBBS.Position -= 1

            End If

            COBDataGrid.Select(_COBBS.Position)

        Catch ex As Exception
            Throw
        Finally

        End Try
    End Sub

    Private Sub NextButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NextButton.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' moves the current index and displays in the edit window the next row
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	4/11/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            '_COBBS.EndEdit()

            COBDataGrid.UnSelect(_COBBS.Position)

            If _COBBS.Position > -1 Then

                'If ValidateCOBChanges() Then
                '    Exit Sub
                'End If

                _COBBS.Position += 1
            End If

            COBDataGrid.Select(_COBBS.Position)

        Catch ex As Exception
            Throw
        Finally

        End Try
    End Sub

    Private Sub AddButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddActionButton.Click

        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

        Try
            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

            AddActionButton.Enabled = False 'this will reenable if the underlying dataset changes

            _UIState = UIStates.Adding

            AddCOBLine()

            OCC_FROM_DATETextBox.Focus()

            'VerifyCOBChanges() 'will highlight required fields

        Catch ex As Exception
            Throw
        Finally

            SetUIElements()

            Me.AutoValidate = HoldAutoValidate

        End Try

    End Sub

    Private Sub DeleteMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteActionButton.Click

        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

        Try
            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

            DeleteActionButton.Enabled = False 'this will reenable if the underlying dataset changes

            If Not COBDataGrid.IsSelected(COBDataGrid.CurrentRowIndex) Then
                MessageBox.Show(Me, "A item must be selected in the Grid before using the delete function", "Confirm Delete", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Else

                _UIState = UIStates.Deleting

                'RemoveHandler OTH_RELATIONComboBox.SelectedIndexChanged, AddressOf OTH_RELATIONComboBox_SelectedIndexChanged
                'RemoveHandler OTH_SEXComboBox.SelectedIndexChanged, AddressOf OTH_SEXComboBox_SelectedIndexChanged
                'RemoveHandler PayerComboBox.SelectedIndexChanged, AddressOf PayerComboBox_SelectedIndexChanged

                DeleteCOBLine()

                'AddHandler OTH_RELATIONComboBox.SelectedIndexChanged, AddressOf OTH_RELATIONComboBox_SelectedIndexChanged
                'AddHandler OTH_SEXComboBox.SelectedIndexChanged, AddressOf OTH_SEXComboBox_SelectedIndexChanged
                'AddHandler PayerComboBox.SelectedIndexChanged, AddressOf PayerComboBox_SelectedIndexChanged

            End If

        Catch ex As Exception
            Throw
        Finally

            Me.AutoValidate = HoldAutoValidate

        End Try

    End Sub

    Private Sub DeleteCOBLine()
        Dim DR As DataRow

        Try

            DR = DirectCast(_COBBS.Current, DataRowView).Row

            If DR.RowState = DataRowState.Added OrElse MessageBox.Show(Me, "Are you sure you want to DELETE the current entry?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                DR.BeginEdit()

                DR.Delete()

                DR.EndEdit()
                _COBBS.EndEdit()

            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub AddCOBLine()

        Dim DR As DataRow

        Try

            DR = _COBDS.Tables("MEDOTHER_INS").NewRow
            DR.BeginEdit()

            DR("MEDOTHER_INS_ID") = _COBDS.Tables("MEDOTHER_INS").Rows.Count
            DR("FAMILY_ID") = _FamilyID
            DR("RELATION_ID") = If(_RelationID, 0)
            DR("OCC_FROM_DATE") = DBNull.Value
            DR("OCC_TO_DATE") = "12/31/9999" 'default date agreed upon
            DR("WORKING_SPOUSE_SW") = 0
            DR("OTH_INS_REFUSAL_SW") = 0
            DR("OTH_PAT_ACCT_NBR") = DBNull.Value
            'DR("HICN") = DBNull.Value
            DR("OTH_POLICY") = DBNull.Value
            DR("OTH_TAXID") = DBNull.Value
            DR("OTH_PAYER") = DBNull.Value
            DR("OTH_SSN") = DBNull.Value
            DR("OTH_FNAME") = DBNull.Value
            DR("OTH_LNAME") = DBNull.Value
            DR("OTH_SEX") = DBNull.Value
            DR("OTH_RELATION") = DBNull.Value
            DR("OTH_DOB") = DBNull.Value
            DR("OTH_PAYER_ID") = DBNull.Value
            DR("OTH_COMMENTS") = DBNull.Value
            DR("UPDATE_REASON") = DBNull.Value
            DR("DOCID") = DBNull.Value
            DR("OTH_SUB_ACCT_NBR") = DBNull.Value
            DR("ADDRESS_LINE1") = DBNull.Value
            DR("ADDRESS_LINE2") = DBNull.Value
            DR("CITY") = DBNull.Value
            DR("STATE") = DBNull.Value
            DR("ZIP") = DBNull.Value
            DR("ZIP_4") = DBNull.Value
            DR("COUNTRY") = DBNull.Value
            DR("PHONE") = DBNull.Value
            DR("EXTENSION1") = DBNull.Value

            _UIState = UIStates.Adding

            DR.EndEdit()

            _COBDS.Tables("MEDOTHER_INS").Rows.Add(DR)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SaveButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveActionButton.Click

        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate
        Dim StartingUISTate As UIStates = _UIState
        Dim Deleted As Boolean = False

        Try
            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

            SaveActionButton.Enabled = False 'this will reenable if the underlying dataset changes
            CancelActionButton.Enabled = False 'this will reenable if the underlying dataset changes

            If _UIState = UIStates.Modifiable Then

                Dim DR As DataRow = DirectCast(_COBBS.Current, DataRowView).Row

                DR.BeginEdit()

                _UIState = UIStates.Modifying
                Return

            ElseIf Not _UIState = UIStates.Deleting Then
                If Not ValidateChildren(ValidationConstraints.Enabled) OrElse ValidateCOBChanges() Then
                    SaveActionButton.Enabled = True
                    CancelActionButton.Enabled = True
                    _UIState = StartingUISTate
                    Return
                Else
                    _UIState = UIStates.Saving
                End If

            Else
                Deleted = True
            End If

            _COBBS.EndEdit()

            _ChangedDRs = _COBDS.GetChanges()

            If _ChangedDRs IsNot Nothing Then

                SetUIElements()

                If SaveCOBChanges(_ChangedDRs) Then

                    _UIState = UIStates.Saved

                    If Deleted Then
                        _UIState = UIStates.Deleted
                    End If

                    _COBBS.ResetBindings(True)

                    SetUIElements()

                    MessageBox.Show("Medical Coverage Record " & If(Deleted, "Deleted", "Saved") & " Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)


                Else

                    SaveActionButton.Enabled = True
                    CancelActionButton.Enabled = True
                    _UIState = StartingUISTate

                    Return

                End If

            Else

                Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable

                CancelCOBChanges(_ChangedDRs)

                MessageBox.Show("No changes were made. Save request resulted in no action being taken.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If

        Catch ex As Exception

            CancelCOBChanges(_ChangedDRs)

            Throw

        Finally

            SetUIElements()

            Me.AutoValidate = HoldAutoValidate

        End Try

    End Sub

    Private Sub CancelChanges()

        Dim HoldState As UIStates = _UIState

        Dim BS As BindingSource

        Try

            BS = DirectCast(_COBBS, BindingSource)

            '  Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            SetUIElements(UIStates.Canceling)

            If UnCommittedMedChangesExist() Then

                Dim Result As DialogResult = MessageBox.Show(Me, "Do you want to Cancel Medical changes?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                If Result = DialogResult.Yes Then

                    CancelCOBChanges(_ChangedDRs)

                Else
                    _UIState = HoldState
                End If

            Else
                CancelCOBChanges()

            End If

        Catch ex As Exception
            Throw
        Finally

            '    Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            SetUIElements()

        End Try

    End Sub
    ' Hide the tooltip if the user starts typing again before the five-second display limit on the tooltip expires.
    Private Sub OTH_DOBTextBox_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
        Me.ToolTip1.Hide(Me.OTH_DOBTextBox)
    End Sub

    Public Function ValidateCOBChanges() As Boolean
        Dim DR As DataRow

        Try

            ClearErrors()

            If Not _ReadOnlyMode AndAlso _COBBS.Count > 0 AndAlso _COBBS.Position > -1 Then

                DR = DirectCast(_COBBS.Current, DataRowView).Row

                If OTH_INS_REFUSAL_SWCheckBox.Checked = False Then
                End If

                If DR("PAYER_NAME").ToString.Trim.ToUpper = "OTHER" AndAlso (IsDBNull(DR("OTH_COMMENTS")) OrElse DR("OTH_COMMENTS").ToString.Trim.Length = 0) Then
                    ErrorProvider1.SetErrorWithTracking(Me.InsurerFreeFormButton, " Must Provide a Insurer")
                End If

                If DR("PAYER_NAME").ToString.Trim.Length = 0 Then
                    ErrorProvider1.SetErrorWithTracking(Me.PayerComboBox, " Must select a Insurer, or Non Specified")
                End If

                If ErrorProvider1.GetErrorCount > 0 Then
                    Return True
                End If

            End If

            Return False

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Sub ClearErrors()

        ErrorProvider1.Clear()

    End Sub

    Private Sub CancelCOBChanges(Optional changedDRs As DataSet = Nothing)

        Dim GoToPosition As Integer

        Try

            GoToPosition = _COBBS.Position

            If changedDRs IsNot Nothing Then
                Dim DR = (From a In changedDRs.Tables("MEDOTHER_INS").Rows.Cast(Of DataRow)() Where a.RowState = DataRowState.Added Select a).FirstOrDefault
                If DR IsNot Nothing Then GoToPosition = 0
            End If

            COBDataGrid.UnSelect(_COBBS.Position)

            _COBBS.CancelEdit()
            _COBDS.Tables("MEDOTHER_INS").RejectChanges()

            _COBBS.ResetBindings(False)

            If _COBBS IsNot Nothing AndAlso _COBBS.Position > -1 AndAlso _COBBS.Count > 0 AndAlso _COBBS.Count > GoToPosition Then
                If _COBBS.Position <> GoToPosition Then
                    COBDataGrid.UnSelect(_COBBS.Position)
                    _COBBS.Position = GoToPosition
                    COBDataGrid.Select(_COBBS.Position)
                End If
            End If

            ClearErrors()

        Catch ex As Exception
            Throw
        Finally

            _UIState = UIStates.Canceled
        End Try
    End Sub

    Public Function SaveCOBChanges(changedDRs As DataSet) As Boolean

        Dim HistEntry As DataTable
        Dim HistRow As DataRow
        Dim ChgCnt As Integer = 0
        Dim COBChangesDetail As String = ""

        Dim Transaction As DbTransaction
        Dim NewLASTUPDT As Date = Date.Now
        Dim NewInsId As Integer

        Try

            If changedDRs IsNot Nothing Then

                HistEntry = New DataTable("HistoryEntries")
                HistEntry.Columns.Add("RowNum", System.Type.GetType("System.Int32"))
                HistEntry.Columns.Add("EntryPosition", System.Type.GetType("System.Int32"))
                HistEntry.Columns.Add("TransactionType", System.Type.GetType("System.Int32"))
                HistEntry.Columns.Add("Detail", System.Type.GetType("System.String"))

                Transaction = CMSDALCommon.BeginTransaction

                For Each DR As DataRow In changedDRs.Tables("MEDOTHER_INS").Rows

                    COBChangesDetail = DataGridCustom.IdentifyChanges(DR, COBDataGrid)

                    If DR.RowState <> DataRowState.Added AndAlso DR.RowState <> DataRowState.Deleted Then

                        HistRow = HistEntry.NewRow

                        HistRow("RowNum") = 0
                        HistRow("EntryPosition") = ChgCnt
                        HistRow("TransactionType") = 13
                        HistRow("Detail") = "UPDATED LINE :" & COBChangesDetail

                        If COBChangesDetail.Length > 0 Then 'why the row shows that it was updated I have yet to figure out :(

                            HistEntry.Rows.Add(HistRow)

                            NewLASTUPDT = CMSDALFDBMD.UpdateMEDOTHER_INS(CInt(DR("MEDOTHER_INS_ID")), CInt(DR("FAMILY_ID")), CShort(DR("RELATION_ID")),
                                                  UFCWGeneral.IsNullDateHandler(DR("OCC_FROM_DATE"), "OCC_FROM_DATE"), UFCWGeneral.IsNullDateHandler(DR("OCC_TO_DATE"), "OCC_TO_DATE"),
                                                  Math.Abs(CDec(DR("WORKING_SPOUSE_SW"))), Math.Abs(CDec(DR("OTH_INS_REFUSAL_SW"))),
                                                  DR("OTH_PAT_ACCT_NBR").ToString.Trim, DR("HICN").ToString.Trim, DR("OTH_POLICY").ToString.Trim,
                                                  UFCWGeneral.IsNullIntegerHandler(DR("OTH_TAXID"), "OTH_TAXID"), DR("OTH_PAYER").ToString.Trim,
                                                  UFCWGeneral.IsNullIntegerHandler(DR("OTH_SSN"), "OTH_SSN"),
                                                  DR("OTH_FNAME").ToString.Trim, DR("OTH_LNAME").ToString.Trim, TryCast(DR("OTH_SEX"), String), TryCast(DR("OTH_RELATION"), String),
                                                  UFCWGeneral.IsNullDateHandler(DR("OTH_DOB"), "OTH_DOB"), UFCWGeneral.IsNullIntegerHandler(DR("OTH_PAYER_ID"), "OTH_PAYER_ID"), DR("OTH_COMMENTS").ToString.Trim,
                                                  DR("UPDATE_REASON").ToString.Trim,
                                                  UFCWGeneral.IsNullLongHandler(DR("DOCID"), "DOCID"),
                                                  DR("OTH_SUB_ACCT_NBR").ToString.Trim,
                                                  DR("ADDRESS_LINE1").ToString.Trim, DR("ADDRESS_LINE2").ToString.Trim, DR("CITY").ToString.Trim,
                                                  DR("STATE").ToString.Trim, UFCWGeneral.IsNullIntegerHandler(DR("ZIP"), ""),
                                                  UFCWGeneral.IsNullShortHandler(DR("ZIP_4"), "ZIP_4"),
                                                  DR("COUNTRY").ToString.Trim, DR("EMAIL").ToString.Trim,
                                                  UFCWGeneral.IsNullDecimalHandler(DR("PHONE"), "PHONE"),
                                                  UFCWGeneral.IsNullShortHandler(DR("EXTENSION1"), "EXTENSION1"), DR("CONTACT1").ToString.Trim, _DomainUser.ToUpper,
                                                  UFCWGeneral.IsNullDateHandler(DR("LASTUPDT"), "LASTUPDT"), Transaction)

                        End If

                    ElseIf DR.RowState = DataRowState.Deleted Then 'DELETE - Note: This functionality only applies to the consolidation process which can only delete a single row

                        HistRow = HistEntry.NewRow

                        HistRow("RowNum") = 0
                        HistRow("EntryPosition") = ChgCnt
                        HistRow("TransactionType") = 14
                        HistRow("Detail") = "DELETED LINE : " & COBChangesDetail

                        HistEntry.Rows.Add(HistRow)

                        CMSDALFDBMD.DeleteMEDOTHER_INS(CInt(DR("MEDOTHER_INS_ID", DataRowVersion.Original)), Transaction)

                    ElseIf DR.RowState = DataRowState.Added Then 'ADD

                        HistRow = HistEntry.NewRow

                        HistRow("RowNum") = 0
                        HistRow("EntryPosition") = ChgCnt
                        HistRow("TransactionType") = 12
                        HistRow("Detail") = "ADDED LINE ENTRY : " & COBChangesDetail

                        HistEntry.Rows.Add(HistRow)

                        ChgCnt += 1
                        NewLASTUPDT = Date.Now
                        CMSDALFDBMD.CreateMEDOTHER_INS(CInt(DR("MEDOTHER_INS_ID")), CInt(DR("FAMILY_ID")), CShort(DR("RELATION_ID")),
                                                        UFCWGeneral.IsNullDateHandler(DR("OCC_FROM_DATE"), "OCC_FROM_DATE"), UFCWGeneral.IsNullDateHandler(DR("OCC_TO_DATE"), "OCC_TO_DATE"),
                                                        Math.Abs(CDec(DR("WORKING_SPOUSE_SW"))), Math.Abs(CDec(DR("OTH_INS_REFUSAL_SW"))),
                                                        TryCast(DR("OTH_PAT_ACCT_NBR"), String), TryCast(DR("HICN"), String), TryCast(DR("OTH_POLICY"), String),
                                                        UFCWGeneral.IsNullIntegerHandler(DR("OTH_TAXID"), "OTH_TAXID"),
                                                        TryCast(DR("OTH_PAYER"), String), UFCWGeneral.IsNullIntegerHandler(DR("OTH_SSN")),
                                                        TryCast(DR("OTH_FNAME"), String), TryCast(DR("OTH_LNAME"), String), TryCast(DR("OTH_SEX"), String), TryCast(DR("OTH_RELATION"), String),
                                                        UFCWGeneral.IsNullDateHandler(DR("OTH_DOB"), "OTH_DOB"), UFCWGeneral.IsNullIntegerHandler(DR("OTH_PAYER_ID"), "OTH_PAYER_ID"), TryCast(DR("OTH_COMMENTS"), String), TryCast(DR("UPDATE_REASON"), String),
                                                        UFCWGeneral.IsNullLongHandler(DR("DOCID"), "DOCID"),
                                                        TryCast(DR("OTH_SUB_ACCT_NBR"), String), TryCast(DR("ADDRESS_LINE1"), String), TryCast(DR("ADDRESS_LINE2"), String), TryCast(DR("CITY"), String), TryCast(DR("STATE"), String),
                                                        UFCWGeneral.IsNullIntegerHandler(DR("ZIP"), "ZIP"), UFCWGeneral.IsNullShortHandler(DR("ZIP_4"), "ZIP_4"),
                                                        TryCast(DR("COUNTRY"), String), TryCast(DR("EMAIL"), String),
                                                        UFCWGeneral.IsNullDecimalHandler(DR("PHONE"), "PHONE"), UFCWGeneral.IsNullShortHandler(DR("EXTENSION1"), "EXTENSION1"),
                                                        TryCast(DR("CONTACT1"), String),
                                                        _DomainUser.ToUpper, Transaction)

                    End If
                Next

                CreateHistory(HistEntry, Transaction)

            End If

            CMSDALCommon.CommitTransaction(Transaction)

            Dim ModifiedQuery =
                        From Modified In _COBBS.List
                        Where CType(Modified, DataRowView).Row.RowState = DataRowState.Modified OrElse CType(Modified, DataRowView).Row.RowState = DataRowState.Added
                        Order By CType(Modified, DataRowView).Row.Field(Of Date)("OCC_TO_DATE") Descending

            For Each ModifiedDR As DataRowView In ModifiedQuery

                ModifiedDR.Row.BeginEdit()

                ModifiedDR.Row("LASTUPDT") = NewLASTUPDT
                ModifiedDR.Row("USERID") = UFCWGeneral.DomainUser.ToUpper

                ModifiedDR.Row.EndEdit()
            Next
            'Dim AddedQuery =
            '           (From Added In _COBBS.List
            '            Where CType(Added, DataRowView).Row.RowState = DataRowState.Added
            '            Order By CType(Added, DataRowView).Row.Field(Of Date)("OCC_TO_DATE") Descending Select Added).FirstOrDefault

            'Dim AddedDR As DataRowView = DirectCast(AddedQuery, DataRowView)

            'AddedDR.Row.BeginEdit()
            'AddedDR.Row("MEDOTHER_INS_ID") = NewInsId
            'AddedDR.Row("LASTUPDT") = NewLASTUPDT
            'AddedDR.Row("USERID") = UFCWGeneral.DomainUser.ToUpper
            'AddedDR.Row.EndEdit()

            _COBDS.AcceptChanges()

            Return True

        Catch ex As System.Data.DataException

            Try

                If Transaction IsNot Nothing AndAlso Transaction.Connection IsNot Nothing AndAlso Transaction.Connection.State <> ConnectionState.Closed Then
                    CMSDALCommon.RollbackTransaction(Transaction)
                End If

            Finally
            End Try

            MsgBox("COB data has been updated since being retrieved. Refresh COB data and retry action.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Refresh required to complete action")

        Catch ex As DB2Exception When ex.Number = -803

            Try

                If Transaction IsNot Nothing AndAlso Transaction.Connection IsNot Nothing AndAlso Transaction.Connection.State <> ConnectionState.Closed Then
                    CMSDALCommon.RollbackTransaction(Transaction)
                End If

            Finally
            End Try

            ErrorProvider1.SetErrorWithTracking(Me.OCC_FROM_DATETextBox, " Payer already used for specified period")
            ErrorProvider1.SetErrorWithTracking(Me.OCC_TO_DATETextBox, " Payer already used for specified period")
            ErrorProvider1.SetErrorWithTracking(Me.PayerComboBox, " Payer already used for specified period")
            MsgBox("Combination of From/To Dates and Payer already used, change a Date or Payer to continue.", MsgBoxStyle.Exclamation And MsgBoxStyle.OkOnly, "Date Range / Payer Combination already used.")

        Catch ex As Exception

            Try

                If Transaction IsNot Nothing AndAlso Transaction.Connection IsNot Nothing AndAlso Transaction.Connection.State <> ConnectionState.Closed Then
                    CMSDALCommon.RollbackTransaction(Transaction)
                End If

            Finally
            End Try

            Throw

        Finally
            If Transaction IsNot Nothing Then Transaction.Dispose()
            Transaction = Nothing

            If HistEntry IsNot Nothing Then HistEntry.Dispose()
            HistEntry = Nothing

        End Try
    End Function

    Public Sub CreateHistory(ByRef histEntry As DataTable, ByRef transaction As DbTransaction)

        Try

            For Each DR As DataRow In histEntry.Rows

                CMSDALFDBMD.CreateHistory(CInt(DR("TRANSACTIONTYPE")), _FamilyID, CShort(If(_RelationID Is Nothing, 0S, _RelationID)), CStr(DR("Detail")), _DomainUser.ToUpper, transaction)

            Next

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub InsurerFreeFormButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InsurerFreeFormButton.Click

        Dim AButton As Button = CType(sender, Button)

        Dim DR As DataRow

        Try
            ErrorProvider1.ClearError(AButton)

            If _COBBS.Count > 0 AndAlso _COBBS.Position > -1 Then

                DR = DirectCast(_COBBS.Current, DataRowView).Row

                Using InsurerPayerDialog As InsurerPayerDialog = New InsurerPayerDialog

                    InsurerPayerDialog.PayerFreeTextBox.Text = DR("OTH_COMMENTS").ToString()

                    Select Case InsurerPayerDialog.ShowDialog
                        Case System.Windows.Forms.DialogResult.OK
                            DR("OTH_COMMENTS") = InsurerPayerDialog.PayerFreeTextBox.Text

                    End Select

                    If DR("PAYER_NAME").ToString.Trim.ToUpper = "OTHER" AndAlso (IsDBNull(DR("OTH_COMMENTS")) OrElse DR("OTH_COMMENTS").ToString.Trim.Length = 0) Then
                        ErrorProvider1.SetErrorWithTracking(AButton, " Must Provide a Insurer")
                    End If

                End Using

            End If
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub CancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelActionButton.Click

        Dim HoldAutoValidateState As AutoValidate = Me.AutoValidate

        Dim BS As BindingSource

        Try

            BS = DirectCast(_COBBS, BindingSource)

            '  Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from occuring when buttons are disabled

            CancelChanges()

        Catch ex As Exception
            Throw
        Finally

            Me.AutoValidate = HoldAutoValidateState

            '  Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

    Private Function UnCommittedMedChangesExist() As Boolean

        Dim Modifications As String = ""

        Dim BS As BindingSource

        Try

            BS = DirectCast(_COBBS, BindingSource)

            '  Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            _ChangedDRs = _COBDS.GetChanges()

            If COBDataGrid IsNot Nothing AndAlso _ChangedDRs IsNot Nothing AndAlso _ChangedDRs.Tables("MEDOTHER_INS").Rows.Count > 0 Then

                For Each DR As DataRow In _ChangedDRs.Tables("MEDOTHER_INS").Rows
                    If DR.RowState <> DataRowState.Added Then
                        'attempt to exclude rows accidently changed during navigation operations
                        Modifications = DataGridCustom.IdentifyChanges(DR, COBDataGrid)

                        If Modifications IsNot Nothing AndAlso Modifications.Length > 0 Then
                            Return True
                        End If

                    ElseIf DR.RowState = DataRowState.Added Then
                        Return True
                    End If
                Next

            End If

            ' Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Function

    Private Sub CheckBox_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Try

            If _COBBS IsNot Nothing AndAlso _COBBS.Position > -1 AndAlso _COBBS.Count > 0 Then
                _COBBS.EndEdit()
                COBDataGrid.RefreshData()
            End If

            ValidateCOBChanges()

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Sub

    Private Sub NumericOnlyTextBox_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles EXTENSION1TextBox.KeyPress, DOCIDTextBox.KeyPress, PHONETextBox.KeyPress, ZIPTextBox.KeyPress, ZIP_4TextBox.KeyPress
        If Char.IsDigit(e.KeyChar) OrElse Char.IsControl(e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

#End Region

#Region "Formatting"

    Private Sub ZIPBinding_BindingComplete(ByVal sender As Object, ByVal e As BindingCompleteEventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' adjusts date values entered for a databinding
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            If e.BindingCompleteState <> BindingCompleteState.Success Then
                ErrorProvider1.SetErrorWithTracking(CType(e.Binding.BindableComponent, Control), "ZIP format invalid. Must be 5 digits including leading zeroes (ie 01234)")
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DateOnlyBinding_BindingComplete(ByVal sender As Object, ByVal e As BindingCompleteEventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' adjusts date values entered for a databinding
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try
            ErrorProvider1.SetErrorWithTracking(CType(e.Binding.BindableComponent, Control), "")

            If e.BindingCompleteState <> BindingCompleteState.Success Then
                ErrorProvider1.SetErrorWithTracking(CType(e.Binding.BindableComponent, Control), "Date format invalid. Use mmddyy or mmddyyyy")
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DateOnlyBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' formats date values entered for a databinding
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            If IsDBNull(e.Value) = False Then
                e.Value = Convert.ToDateTime(String.Format("{0:MM-dd-yyyy}", e.Value)) 'handles mmddyy entry
                e.Value = Format(e.Value, "MM-dd-yyyy")
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub UCaseBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            If IsDBNull(e.Value) = False AndAlso CStr(e.Value).Trim <> "" Then
                e.Value = CStr(e.Value).ToUpper.Trim
            Else
                e.Value = ""
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub UCaseBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try

            If IsDBNull(e.Value) = False AndAlso CStr(e.Value).Trim <> "" Then
                e.Value = CStr(e.Value).ToUpper
            Else
                e.Value = System.DBNull.Value
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DateBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            If IsDBNull(e.Value) = False AndAlso CStr(e.Value).Trim <> "" Then
                e.Value = Convert.ToDateTime(String.Format("{0:MM/dd/yyyy}", e.Value))
            Else
                e.Value = ""
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DateBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            If IsDBNull(e.Value) = False AndAlso CStr(e.Value).Trim <> "" Then
                e.Value = CStr(e.Value).ToUpper
            Else
                e.Value = System.DBNull.Value
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SEXBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try

            If IsDBNull(e.Value) = False AndAlso CStr(e.Value).Trim <> "" Then
                Select Case e.Value.ToString
                    Case "M"
                        e.Value = "Male"
                    Case "F"
                        e.Value = "Female"
                    Case Else
                        e.Value = ""
                End Select
            Else
                e.Value = ""
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SEXBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            If IsDBNull(e.Value) = False AndAlso CStr(e.Value).Trim <> "" Then
                e.Value = CStr(e.Value).ToUpper
            Else
                e.Value = System.DBNull.Value
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub StateBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            If IsDBNull(e.Value) = False AndAlso CStr(e.Value).Trim <> "" Then
                e.Value = CStr(e.Value).ToUpper
            Else
                e.Value = ""
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub StateBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            If IsDBNull(e.Value) = False AndAlso CStr(e.Value).Trim <> "" Then
                e.Value = CStr(e.Value).ToUpper
            Else
                e.Value = System.DBNull.Value
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SSNBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            If IsDBNull(e.Value) = False AndAlso IsNumeric(e.Value) = False Then
                e.Value = UFCWGeneral.UnFormatSSN(e.Value.ToString)
            End If

            If e.Value.ToString.Trim.Length = 0 Then e.Value = System.DBNull.Value

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub CheckBoxBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            If IsDBNull(e.Value) = False AndAlso IsNumeric(e.Value) Then
                Select Case CInt(e.Value)
                    Case 0
                        e.Value = False
                    Case 1
                        e.Value = True
                End Select
            Else
                e.Value = False
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub CheckBoxBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            If IsDBNull(e.Value) = False AndAlso IsNumeric(e.Value) = True Then
                e.Value = 1
            Else
                e.Value = 0
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub TAXIDBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            If IsDBNull(e.Value) = False AndAlso IsNumeric(e.Value) = True Then
                e.Value = FormatTAXID(e.Value.ToString)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub TAXIDBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            If IsDBNull(e.Value) = False AndAlso IsNumeric(e.Value) = False Then
                e.Value = UnFormatTAXID(e.Value.ToString)
            Else
                e.Value = System.DBNull.Value
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LeadingZeroesSize5Binding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            If IsDBNull(e.Value) = False AndAlso IsNumeric(e.Value) = True Then
                e.Value = CInt(e.Value).ToString("D5")
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LeadingZeroesSize4Binding_format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            If IsDBNull(e.Value) = False AndAlso IsNumeric(e.Value) = True Then
                e.Value = CInt(e.Value).ToString("D4")
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub Numeric2NullBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try

            If IsDBNull(e.Value) = False AndAlso IsNumeric(e.Value) = False Then
                e.Value = System.DBNull.Value
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SSNBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            If IsDBNull(e.Value) = False AndAlso IsNumeric(e.Value) = True Then
                e.Value = UFCWGeneral.FormatSSN(e.Value.ToString)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Function FormatTAXID(ByVal strTAXID As String) As String
        Dim StrTemp As String

        StrTemp = UnFormatTAXID(strTAXID)
        If StrTemp.Trim <> "" Then
            Return Microsoft.VisualBasic.Left(StrTemp, 2) & "-" & Microsoft.VisualBasic.Mid(StrTemp, 3, 7)
        Else
            Return ""
        End If
    End Function

    Public Shared Function UnFormatTAXID(ByVal strTAXID As String) As String
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' unformats an ssn
        ' </summary>
        ' <param name="strSSN"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            If Replace(Replace(Replace(strTAXID, " ", ""), "-", ""), "/", "") <> "" Then
                Return Format(CLng(Replace(Replace(Replace(strTAXID, " ", ""), "-", ""), "/", "")), "0########")
            End If

            Return ""

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub AllowOnlyNumeric_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DOCIDTextBox.TextChanged, PHONETextBox.TextChanged, EXTENSION1TextBox.TextChanged, ZIP_4TextBox.TextChanged, ZIPTextBox.TextChanged

        Dim TBox As TextBox
        Dim IntCnt As Integer
        Dim StrTmp As String

        Try

            TBox = CType(sender, TextBox)

            If IsNumeric(TBox.Text) = False AndAlso Len(TBox.Text) > 0 Then
                StrTmp = TBox.Text
                For IntCnt = 1 To Len(StrTmp)
                    If IsNumeric(Mid(StrTmp, IntCnt, 1)) = False AndAlso Len(StrTmp) > 0 AndAlso Mid(StrTmp, IntCnt, 1) <> "-" Then
                        StrTmp = Replace(StrTmp, Mid(StrTmp, IntCnt, 1), "")
                    End If
                Next
                TBox.Text = StrTmp
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub OCC_FROM_DATETextBox_Validating(sender As Object, e As CancelEventArgs) Handles OCC_FROM_DATETextBox.Validating

        Dim Tbox As TextBox = CType(sender, TextBox)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If _COBBS Is Nothing OrElse _COBBS.Position < 0 OrElse Tbox.ReadOnly Then Return

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

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub OCC_TO_DATETextBox_Validating(sender As Object, e As CancelEventArgs) Handles OCC_TO_DATETextBox.Validating

        Dim Tbox As TextBox = CType(sender, TextBox)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If _COBBS Is Nothing OrElse _COBBS.Position < 0 OrElse Tbox.ReadOnly Then Return

            ErrorProvider1.ClearError(Tbox)

            If Tbox.Text.Trim.Length < 1 Then
                ErrorProvider1.SetErrorWithTracking(Tbox, " To Date is required.")
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

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub OTH_DOBTextBox_Validating(sender As Object, e As CancelEventArgs) Handles OTH_DOBTextBox.Validating

        Dim Tbox As TextBox = CType(sender, TextBox)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ErrorProvider1.ClearError(Tbox)

            If _COBBS Is Nothing OrElse _COBBS.Position < 0 OrElse Tbox.ReadOnly OrElse Tbox.Text.Trim.Length < 1 Then Return

            Dim HoldDate As Date? = UFCWGeneral.ValidateDate(Tbox.Text)
            If HoldDate Is Nothing Then
                ErrorProvider1.SetErrorWithTracking(Tbox, " Date format must be mm/dd/yyyy or mm-dd-yyyy.")
            Else

                If Tbox.Text <> CDate(HoldDate).ToString("MM-dd-yyyy") Then
                    Tbox.Text = CDate(HoldDate).ToString("MM-dd-yyyy")
                End If

            End If

            If ErrorProvider1.GetError(Tbox).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub ZIPTextBox_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ZIPTextBox.Validating

        Dim Tbox As TextBox = CType(sender, TextBox)

        Try

            ErrorProvider1.ClearError(Tbox)

            If Tbox.Text.Length > 0 AndAlso Not Tbox.Text Like "#####" Then
                ' The Zip code is invalid.
                ' Cancel the event moving off of the control.
                e.Cancel = True

                ' Select the offending text.
                Tbox.Select(0, Tbox.Text.Length)

                ' Give the ErrorProvider the error message to
                ' display.
                ErrorProvider1.SetErrorWithTracking(Tbox, "Invalid ZIP code " & "ZIP - 00000")
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub ZIP_4TextBox_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ZIP_4TextBox.Validating

        Dim Tbox As TextBox = CType(sender, TextBox)

        Try

            ErrorProvider1.ClearError(Tbox)

            If Tbox.Text.Length > 0 AndAlso Not Tbox.Text Like "####" Then
                ' The Zip code is invalid.
                ' Cancel the event moving off of the control.
                e.Cancel = True

                ' Select the offending text.
                Tbox.Select(0, CType(sender, TextBox).Text.Length)

                ' Give the ErrorProvider the error message to
                ' display.
                ErrorProvider1.SetErrorWithTracking(Tbox, "Invalid ZIP+4 code " & "ZIP+4 - 0000")
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub HistoryButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HistoryButton.Click

        Dim Frm As History

        Try

            Frm = New History

            Frm.FamilyID = _FamilyID
            Frm.RelationID = _RelationID

            Frm.ShowDialog(Me)

        Catch ex As Exception
            Throw
        Finally
            If Frm IsNot Nothing Then
                Frm.Dispose()
            End If
            Frm = Nothing
        End Try

    End Sub

    Private Sub _COBBS_ListChanged(sender As Object, e As ListChangedEventArgs) Handles _COBBS.ListChanged

        Dim BS As BindingSource

        Try
            BS = CType(sender, BindingSource)

            'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") RS(" & If(BS Is Nothing OrElse BS.Position < 0, "N/A", DirectCast(BS.Current, System.Data.DataRowView).Row.RowState.ToString) & ") O(" & e.OldIndex.ToString & ") N(" & e.NewIndex.ToString & ") CT(" & e.ListChangedType.ToString & ") ST(" & _UIState.ToString & ") SEL(" & If(BS Is Nothing OrElse BS.Position < 0 OrElse COBDataGrid.DataSource Is Nothing, "N/A", COBDataGrid.IsSelected(BS.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Select Case e.ListChangedType
                Case ListChangedType.ItemDeleted 'account for rows deleted due to a cancel action

                    Select Case _UIState
                        Case UIStates.Modifying


                        Case Else

                            If BS.Position > -1 AndAlso BS.Position <> e.NewIndex Then
                                If e.NewIndex > -1 Then BS.Position = e.NewIndex
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
                                        COBDataGrid.Select(BS.Position)
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
                                        SetUIElements()
                                        If e.NewIndex > -1 Then COBDataGrid.Select(e.NewIndex)
                                    End If
                                Case Else
                            End Select

                    End Select

                Case ListChangedType.Reset 'triggered by sorts or changes in grid filter

                    If BS.Position > -1 AndAlso BS.Position <> e.NewIndex Then
                        If e.NewIndex > -1 Then BS.Position = e.NewIndex
                        BS.ResetCurrentItem()
                        SetUIElements()
                    End If

                Case ListChangedType.ItemAdded 'includes items reincluded when filters change

                    Select Case _UIState
                        Case UIStates.Adding
                            If BS.Position <> e.NewIndex OrElse (BS.Position > -1 AndAlso BS.Count = 1) Then 'first item added

                                If e.NewIndex <> e.OldIndex AndAlso e.OldIndex > -1 Then
                                    COBDataGrid.UnSelect(e.OldIndex)
                                End If

                                If e.NewIndex > -1 Then BS.Position = e.NewIndex
                                If e.NewIndex > -1 Then COBDataGrid.Select(e.NewIndex)
                            End If

                        Case Else
                    End Select

                Case Else

            End Select

        Catch ex As Exception
            Throw
        Finally
            '  Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") RS(" & If(BS Is Nothing OrElse BS.Position < 0, "N/A", DirectCast(BS.Current, System.Data.DataRowView).Row.RowState.ToString) & ") O(" & e.OldIndex.ToString & ") N(" & e.NewIndex.ToString & ") CT(" & e.ListChangedType.ToString & ") ST(" & _UIState.ToString & ") SEL(" & If(BS Is Nothing OrElse BS.Position < 0 OrElse COBDataGrid.DataSource Is Nothing, "N/A", COBDataGrid.IsSelected(BS.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub COBBS_CurrentChanged(sender As Object, e As EventArgs) Handles _COBBS.CurrentChanged

        Dim BS As BindingSource

        Try

            BS = DirectCast(_COBBS, BindingSource)

            '  Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If BS Is Nothing OrElse BS.Position < 0 OrElse BS.Count < 1 Then Return 'no current row, most likely an item filter value was changed

            Dim DR As DataRow = DirectCast(BS.Current, DataRowView).Row

        Catch ex As Exception
            Throw
        Finally

            ' Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub InsurerFreeFormButton_Validating(sender As Object, e As CancelEventArgs) Handles InsurerFreeFormButton.Validating

        Dim Bton As Button = CType(sender, Button)

        Dim DR As DataRow

        ErrorProvider1.ClearError(InsurerFreeFormButton)

        If _COBBS Is Nothing OrElse _COBBS.Position < 0 OrElse _COBBS.Count < 1 OrElse Not Bton.Enabled Then Return

        Try

            'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _COBBS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = DirectCast(_COBBS.Current, DataRowView).Row

            Select Case PayerComboBox.Text.Trim.ToUpper
                Case "OTHER"

                    If IsDBNull(DR("OTH_COMMENTS")) OrElse DR("OTH_COMMENTS").ToString.Trim.Length = 0 Then
                        ErrorProvider1.SetErrorWithTracking(Bton, " Must specify an Insurer")
                        Me.ToolTip1.SetToolTip(Me.InsurerFreeFormButton, "")
                    Else
                        Me.ToolTip1.SetToolTip(Me.InsurerFreeFormButton, DR("OTH_COMMENTS").ToString)
                    End If

            End Select

            If ErrorProvider1.GetError(Bton).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
            End If

        Catch ex As Exception
            Throw
        Finally

            '    Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _COBBS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

    Private Sub _COBBS_CurrentItemChanged(sender As Object, e As EventArgs) Handles _COBBS.CurrentItemChanged

        'Called after CurrentChanged and one of the properties of Current is changed
        Dim BS As BindingSource

        Try


            BS = DirectCast(sender, BindingSource)

            If BS Is Nothing OrElse BS.Position < 0 OrElse BS.Current Is Nothing OrElse BS.Count < 1 Then Return

            '  Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Dim DR As DataRow = DirectCast(BS.Current, DataRowView).Row

            If Not (_UIState And UIStates.[ReadOnly]) = _UIState Then

                If Not (_UIState And UIStates.[ReadOnly]) = _UIState Then
                    If DR IsNot Nothing AndAlso CDate(DR("OCC_TO_DATE")).Year = 9999 AndAlso (DR.HasVersion(DataRowVersion.Proposed) OrElse (DR.RowState = DataRowState.Modified OrElse DR.RowState = DataRowState.Added)) Then 'in addition to current/future active addresses, items awaiting approval will also have 9999 thru date 

                    End If
                End If

            End If

            SetUIElements()

            '     Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Sub



#End Region

End Class
